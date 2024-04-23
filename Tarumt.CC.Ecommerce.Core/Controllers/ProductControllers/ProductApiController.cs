using Microsoft.AspNetCore.Mvc;
using Tarumt.CC.Ecommerce.Core.Infrastructure.Models;
using Tarumt.CC.Ecommerce.Core.Services;
using Tarumt.CC.Ecommerce.SharedLibrary.Infrastructure.Responses;

namespace Tarumt.CC.Ecommerce.Controllers.BlogControllers
{
    [ApiController]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class ProductApiController(ProductService service) : ControllerBase
    {
        [HttpGet("/api/product/")]
        public PaginatedResponse<IEnumerable<ProductResponse>> GetAll(int pageNumber, int pageSize, string? keyword)
        {
            PagedList<ProductResponse> data = service.GetAllByIsNotExpiredResponse(pageNumber, pageSize, keyword, false);
            IOrderedEnumerable<ProductResponse> products = data.OrderBy(m => m.CreatedAt);

            return new PaginatedResponse<IEnumerable<ProductResponse>>
            {
                Responses = products,
                CurrentPage = pageNumber,
                TotalPages = data.TotalPages,
                TotalCount = data.TotalCount,
                HasNext = data.HasNext,
                HasPrevious = data.HasPrevious,
            };
        }

        [HttpGet("/api/product/category/")]
        public PaginatedResponse<IEnumerable<ProductResponse>> GetAllByCategory(int pageNumber, int pageSize, string? keyword, string category)
        {
            PagedList<ProductResponse> data = service.GetAllByIsNotAndCategoryExpired(pageNumber, pageSize, keyword, category.Split(","), false);
            IOrderedEnumerable<ProductResponse> products = data.OrderBy(m => m.CreatedAt);

            return new PaginatedResponse<IEnumerable<ProductResponse>>
            {
                Responses = products,
                CurrentPage = pageNumber,
                TotalPages = data.TotalPages,
                TotalCount = data.TotalCount,
                HasNext = data.HasNext,
                HasPrevious = data.HasPrevious,
            };
        }

        [HttpGet("/api/product/{id}/")]
        public async Task<ProductResponse> GetByIdAsync(string id)
        {
            Product data = await service.GetByIdIsNotExpiredAsync(id, false);
            return (ProductResponse)data;
        }
    }
}
