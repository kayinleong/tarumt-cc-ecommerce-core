using Ky.Web.CMS.SharedLibarary.Infrastructure.Requests.Admin;
using Ky.Web.CMS.SharedLibarary.Infrastructure.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;
using Tarumt.CC.Ecommerce.Infrastructure.Models;
using Tarumt.CC.Ecommerce.Services;

namespace Tarumt.CC.Ecommerce.Controllers.BlogControllers
{
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    [ApiController]
    [IgnoreAntiforgeryToken]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class ProductAdminApiController : ControllerBase
    {
        private readonly ProductService _service;

        public ProductAdminApiController(ProductService service)
        {
            _service = service;
        }

        [HttpGet("/api/admin/product/")]
        public PaginatedResponse<IEnumerable<Product>> GetAll(int pageNumber, int pageSize, string? keyword, bool isDeleted = false)
        {
            PagedList<Product> data = _service.GetAll(pageNumber, pageSize, keyword, isDeleted);
            IOrderedEnumerable<Product> blogs = data.OrderBy(m => m.CreatedAt);

            return new PaginatedResponse<IEnumerable<Product>>
            {
                Responses = blogs,
                CurrentPage = pageNumber,
                TotalPages = data.TotalPages,
                TotalCount = data.TotalCount,
                HasNext = data.HasNext,
                HasPrevious = data.HasPrevious,
            };
        }

        [HttpGet("/api/admin/product/{id}/")]
        public async Task<Product> GetById(string id, bool isDeleted)
        {
            Product data = await _service.GetByIdAsync(id, isDeleted);
            return data;
        }

        [HttpPost("/api/admin/product/")]
        public async Task<ActionResult> Create(ProductAdminRequest productAdminRequest)
        {
            bool data = await _service.CreateAsync(productAdminRequest);
            if (!data)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPut("/api/admin/product/{id}/")]
        public async Task<ActionResult> Update(string id, ProductAdminRequest productAdminRequest)
        {
            bool data = await _service.UpdateAsync(productAdminRequest, id, false);
            if (!data)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpDelete("/api/admin/product/{id}/")]
        public async Task<ActionResult> Delete(string id)
        {
            bool data = await _service.DeleteAsync(id);
            if (!data)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
