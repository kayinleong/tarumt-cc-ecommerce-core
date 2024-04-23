using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Tarumt.CC.Ecommerce.Core.Infrastructure.Models;
using Tarumt.CC.Ecommerce.Core.Services;
using Tarumt.CC.Ecommerce.SharedLibrary.ViewModels;

namespace Tarumt.CC.Ecommerce.Core.Controllers.UserControllers
{
    public class UserController(UserService _service) : Controller
    {
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

            if (await _service.LoginAsync(userLoginViewModel.Username!.Trim(), userLoginViewModel.Password!.Trim()))
            {
                User user = await _service.GetByUsernameAsync(userLoginViewModel.Username!.Trim(), false, false);
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
