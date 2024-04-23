using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;
using Tarumt.CC.Ecommerce.Core.Services;
using Tarumt.CC.Ecommerce.SharedLibrary.Infrastructure.Requests.Admin;
using Tarumt.CC.Ecommerce.SharedLibrary.Infrastructure.Responses;

namespace Tarumt.CC.Ecommerce.Core.Controllers.BlogControllers
{
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    [ApiController]
    [IgnoreAntiforgeryToken]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class ProductAdminApiController(ProductService _service) : ControllerBase
    {
        [HttpGet("/api/admin/product/")]
        public PaginatedResponse<IEnumerable<ProductResponse>> GetAll(int pageNumber, int pageSize, string? keyword, bool isDeleted = false)
        {
            PagedList<ProductResponse> data = _service.GetAllResponse(pageNumber, pageSize, keyword, isDeleted);
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

        [HttpGet("/api/admin/product/{id}/")]
        public async Task<ProductResponse> GetById(string id, bool isDeleted)
        {
            ProductResponse data = (ProductResponse)await _service.GetByIdAsync(id, isDeleted);
            return data;
        }

        [HttpPost("/api/admin/product/")]
        public async Task<ActionResult> Create(ProductAdminRequest productAdminRequest)
        {
            bool data = await _service.CreateAsync(productAdminRequest);
            return data ? Ok() : BadRequest();
        }

        [HttpPut("/api/admin/product/{id}/")]
        public async Task<ActionResult> Update(string id, ProductAdminRequest productAdminRequest)
        {
            bool data = await _service.UpdateAsync(productAdminRequest, id, false);
            return data ? Ok() : BadRequest();
        }

        [HttpDelete("/api/admin/product/{id}/")]
        public async Task<ActionResult> Delete(string id)
        {
            bool data = await _service.DeleteAsync(id);
            return data ? Ok() : BadRequest();
        }
    }
}
