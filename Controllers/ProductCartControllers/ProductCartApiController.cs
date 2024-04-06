using Ky.Web.CMS.SharedLibarary.Infrastructure.Responses;
using Microsoft.AspNetCore.Mvc;
using Tarumt.CC.Ecommerce.Core.Infrastructure.Models;
using Tarumt.CC.Ecommerce.Core.Services;
using Tarumt.CC.Ecommerce.Infrastructure.Models;
using Tarumt.CC.Ecommerce.SharedLibarary.Infrastructure.Requests;

namespace Tarumt.CC.Ecommerce.Core.Controllers.ProductCartControllers
{
    [ApiController]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class ProductCartApiController : ControllerBase
    {
        private readonly ProductCartService _service;

        public ProductCartApiController(ProductCartService service)
        {
            _service = service;
        }

        [HttpGet("/api/product_cart/")]
        public async Task<ProductCart> GetById()
        {
            User user = (HttpContext.Items["User"] as User)!;
            ProductCart data;

            try
            {
                data = await _service.GetByUserIdAsync(user.Id, false);
            }
            catch
            {
                await _service.CreateAsync(new()
                {
                    ProductIds = new string[0],
                    UserId = user.Id,
                });

                data = await _service.GetByUserIdAsync(user.Id, false);
            }

            return data;
        }

        [HttpPut("/api/product_cart/{id}/")]
        public async Task<ActionResult> Update(string id, ProductCartRequest productCartRequest)
        {
            bool data = await _service.UpdateAsync(productCartRequest, id, false);
            if (!data)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
