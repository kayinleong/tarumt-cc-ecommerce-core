using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;
using Tarumt.CC.Ecommerce.Core.Infrastructure.Models;
using Tarumt.CC.Ecommerce.Core.Services;
using Tarumt.CC.Ecommerce.SharedLibrary.Infrastructure.Requests.Admin;
using Tarumt.CC.Ecommerce.SharedLibrary.Infrastructure.Responses;

namespace Tarumt.CC.Ecommerce.Core.Controllers.BlogCategoryControllers
{
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    [ApiController]
    [IgnoreAntiforgeryToken]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class ProductCategoryAdminApiController(ProductCategoryService _service) : ControllerBase
    {
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

        [HttpGet("/api/admin/product_category/{id}/")]
        public async Task<ProductCategory> GetById(string id)
        {
            ProductCategory data = await _service.GetByIdAsync(id, false);
            return data;
        }

        [HttpGet("/api/admin/product_category/name/{name}/")]
        public async Task<ProductCategory> GetByName(string name)
        {
            ProductCategory data = await _service.GetByNameAsync(name, false);
            return data;
        }

        [HttpPost("/api/admin/product_category/")]
        public async Task<ActionResult> Create(ProductCategoryAdminRequest productCategoryAdminRequest)
        {
            bool data = await _service.CreateAsync(productCategoryAdminRequest);
            return data ? Ok() : BadRequest();
        }

        [HttpPut("/api/admin/product_category/{id}/")]
        public async Task<ActionResult> Update(ProductCategoryAdminRequest productCategoryAdminRequest, string id)
        {
            bool data = await _service.UpdateAsync(productCategoryAdminRequest, id, false);
            return data ? Ok() : BadRequest();
        }

        [HttpDelete("/api/admin/product_category/{id}/")]
        public async Task<ActionResult> Delete(string id)
        {
            bool data = await _service.DeleteAsync(id);
            return data ? Ok() : BadRequest();
        }
    }
}
