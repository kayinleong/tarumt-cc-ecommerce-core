using Ky.Web.CMS.SharedLibarary.Infrastructure.Responses;
using Microsoft.AspNetCore.Mvc;
using Tarumt.CC.Ecommerce.Infrastructure.Models;
using Tarumt.CC.Ecommerce.Services;

namespace Tarumt.CC.Ecommerce.Core.Controllers.ProductCategoryControllers
{
    [ApiController]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class ProductCategoryApiController
    {
        private readonly ProductCategoryService _service;

        public ProductCategoryApiController(ProductCategoryService service)
        {
            _service = service;
        }

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
