using Ky.Web.CMS.SharedLibarary.Infrastructure.Requests.Admin;
using Ky.Web.CMS.SharedLibarary.Infrastructure.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;
using Tarumt.CC.Ecommerce.Infrastructure.Models;
using Tarumt.CC.Ecommerce.Services;

namespace Tarumt.CC.Ecommerce.Controllers.BlogCategoryControllers
{
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    [ApiController]
    [IgnoreAntiforgeryToken]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class ProductCategoryAdminApiController : ControllerBase
    {
        private readonly ProductCategoryService _service;

        public ProductCategoryAdminApiController(ProductCategoryService service)
        {
            _service = service;
        }

        [HttpGet("/api/admin/product_category/")]
        public PaginatedResponse<IEnumerable<ProductCategory>> GetAll(int pageNumber, int pageSize, string? keyword, bool isDeleted = false)
        {
            PagedList<ProductCategory> data = _service.GetAll(pageNumber, pageSize, keyword, isDeleted);
            IOrderedEnumerable<ProductCategory> blogCategories = data.OrderBy(m => m.CreatedAt);

            return new PaginatedResponse<IEnumerable<ProductCategory>>
            {
                Responses = blogCategories,
                CurrentPage = pageNumber,
                TotalPages = data.TotalPages,
                TotalCount = data.TotalCount,
                HasNext = data.HasNext,
                HasPrevious = data.HasPrevious,
            };
        }

        [HttpPost("/api/admin/product_category/")]
        public async Task<ActionResult> Create(ProductCategoryAdminRequest productCategoryAdminRequest)
        {
            bool data = await _service.CreateAsync(productCategoryAdminRequest);
            if (!data)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPut("/api/admin/product_category/{id}/")]
        public async Task<ActionResult> Update(ProductCategoryAdminRequest productCategoryAdminRequest, string id)
        {
            bool data = await _service.UpdateAsync(productCategoryAdminRequest, id, false);
            if (!data)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpDelete("/api/admin/product_category/{id}/")]
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
