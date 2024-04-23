using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;
using Tarumt.CC.Ecommerce.Core.Infrastructure.Models;
using Tarumt.CC.Ecommerce.Core.Services;
using Tarumt.CC.Ecommerce.SharedLibrary.Infrastructure.Requests;
using Tarumt.CC.Ecommerce.SharedLibrary.Infrastructure.Responses;

namespace Tarumt.CC.Ecommerce.Core.Controllers.ProductCartControllers
{
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    [ApiController]
    [IgnoreAntiforgeryToken]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class UserCartApiController(UserCartService _service, UserCartItemService _userCartItemService) : ControllerBase
    {
        [HttpGet("/api/user_cart/")]
        public async Task<UserCartResponse> GetById()
        {
            User user = (HttpContext.Items["User"] as User)!;
            UserCartResponse data;

            try
            {
                data = (UserCartResponse)await _service.GetByUserIdAsync(user.Id, false);
            }
            catch
            {
                await _service.CreateAsync(new()
                {
                    UserCartItem = []
                }, user);

                data = (UserCartResponse)await _service.GetByUserIdAsync(user.Id, false);
            }

            return data;
        }

        [HttpGet("/api/user_cart/item/{id}/")]
        public async Task<UserCartItemResponse> GetUserCartItemById(string id)
        {
            UserCartItem data = await _userCartItemService.GetById(id, false);
            return (UserCartItemResponse)data;
        }

        [HttpPut("/api/user_cart/item/")]
        public async Task<ActionResult> UpdateCartItem(UserCartRequest productCartRequest)
        {
            User user = (HttpContext.Items["User"] as User)!;
            UserCart cart = await _service.GetByUserIdAsync(user.Id, false);

            bool data = await _service.UpdateCartItemAsync(productCartRequest, cart.Id, false);
            return data ? Ok() : BadRequest();
        }

        [HttpPost("/api/user_cart/checkout/")]
        public async Task<ActionResult> CheckoutCart()
        {
            User user = (HttpContext.Items["User"] as User)!;
            UserCart cart = await _service.GetByUserIdAsync(user.Id, false);

            bool data = await _service.CheckoutAsync(cart.Id, user);
            return data ? Ok() : BadRequest();
        }
    }
}
