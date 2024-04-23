using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;
using Tarumt.CC.Ecommerce.Core.Infrastructure.Models;
using Tarumt.CC.Ecommerce.Core.Services;
using Tarumt.CC.Ecommerce.SharedLibrary.Infrastructure.Responses;

namespace Tarumt.CC.Ecommerce.Core.Controllers.UserOrderControllers
{
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    [ApiController]
    [IgnoreAntiforgeryToken]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class UserOrderApiController(UserOrderService _service) : ControllerBase
    {
        [HttpGet("/api/user_order/")]
        public PaginatedResponse<IEnumerable<UserOrderResponse>> GetAll(int pageNumber, int pageSize)
        {
            User user = (HttpContext.Items["User"] as User)!;
            PagedList<UserOrderResponse> data = _service.GetAllResponseByUserId(user.Id, pageNumber, pageSize, false);
            IOrderedEnumerable<UserOrderResponse> products = data.OrderBy(m => m.CreatedAt);

            return new PaginatedResponse<IEnumerable<UserOrderResponse>>
            {
                Responses = products,
                CurrentPage = pageNumber,
                TotalPages = data.TotalPages,
                TotalCount = data.TotalCount,
                HasNext = data.HasNext,
                HasPrevious = data.HasPrevious,
            };
        }

        [HttpGet("/api/user_order/{id}/")]
        public async Task<ActionResult<UserOrderResponse>> GetById(string id)
        {
            User user = (HttpContext.Items["User"] as User)!;
            var data = await _service.GetByUserId(user.Id, false);

            return data.User.Id == user.Id ? (UserOrderResponse)data : BadRequest();
        }
    }
}
