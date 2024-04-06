using Ky.Web.CMS.SharedLibarary.Infrastructure.Responses;
using Microsoft.AspNetCore.Mvc;
using Tarumt.CC.Ecommerce.Infrastructure.Models;
using Tarumt.CC.Ecommerce.Services;

namespace Tarumt.CC.Ecommerce.Controllers.BlogControllers
{
    [ApiController]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class ProductApiController : ControllerBase
    {
        private readonly ProductService _service;

        public ProductApiController(ProductService service)
        {
            _service = service;
        }

        [HttpGet("/api/product/")]
        public PagedList<ProductResponse> GetAll(int pageNumber, int pageSize, string? keyword)
        {
            PagedList<ProductResponse> data = _service.GetAllResponse(pageNumber, pageSize, keyword, false);
            return data;
        }

        [HttpGet("/api/product/{id}/")]
        public async Task<ProductResponse> GetByIdAsync(string id)
        {
            Product data = await _service.GetByIdAsync(id, false);
            return (ProductResponse)data;
        }
    }
}
