using Microsoft.AspNetCore.Mvc;
using Tarumt.CC.Ecommerce.Core.Infrastructure.Models;
using Tarumt.CC.Ecommerce.Core.Services;
using Tarumt.CC.Ecommerce.SharedLibrary.Infrastructure.Responses;

namespace Tarumt.CC.Ecommerce.Core.Controllers.ProductCategoryControllers
{
    [ApiController]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class ProductCategoryApiController(ProductCategoryService _service)
    {
        [HttpGet("/api/product_category/")]
        public PagedList<ProductCategoryResponse> GetAll(int pageNumber, int pageSize, string? keyword)
        {
            PagedList<ProductCategoryResponse> data = _service.GetAllResponse(pageNumber, pageSize, keyword, false);
            return data;
        }

        [HttpGet("/api/product_category/{id}/")]
        public async Task<ProductCategoryResponse> GetByIdAsync(string id)
        {
            ProductCategory data = await _service.GetByIdAsync(id, false);
            return (ProductCategoryResponse)data;
        }
    }
}
