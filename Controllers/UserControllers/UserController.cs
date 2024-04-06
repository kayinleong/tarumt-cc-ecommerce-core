using Ky.Web.CMS.SharedLibarary.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Tarumt.CC.Ecommerce.Services;

namespace Tarumt.CC.Ecommerce.Controllers.UserControllers
{
    public class UserController : Controller
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet("/account/login/")]
        public ActionResult<UserLoginViewModel> Login(string returnUrl)
        {
            return View(new UserLoginViewModel
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost("/account/login/")]
        public async Task<ActionResult<UserLoginViewModel>> Login(UserLoginViewModel userLoginViewModel)
        {
            if (!ModelState.IsValid)
            {
                userLoginViewModel.Password = "";
                return View(userLoginViewModel);
            }

            if (await _userService.LoginAsync(userLoginViewModel.Username!.Trim(), userLoginViewModel.Password!.Trim()))
            {
                Infrastructure.Models.User user = await _userService.GetByUsernameAsync(userLoginViewModel.Username!.Trim(), false, false);
                ClaimsIdentity claimsIdentity = new(new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, user.Id),
                    new(ClaimTypes.Name, user.Username),
                    new(ClaimTypes.Email, user.Email),
                }, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(new ClaimsPrincipal(claimsIdentity));
                if (!string.IsNullOrEmpty(userLoginViewModel.ReturnUrl))
                {
                    return Redirect(userLoginViewModel.ReturnUrl);
                }

                return Ok();
            }

            return View(userLoginViewModel);
        }
    }
}
