using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;
using Tarumt.CC.Ecommerce.Core.Infrastructure.Models;
using Tarumt.CC.Ecommerce.Core.Services;
using Tarumt.CC.Ecommerce.SharedLibrary.Infrastructure.Requests.Admin;
using Tarumt.CC.Ecommerce.SharedLibrary.Infrastructure.Responses;

namespace Tarumt.CC.Ecommerce.Core.Controllers.UserControllers
{
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    [ApiController]
    [IgnoreAntiforgeryToken]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class UserAdminApiController(UserService _service) : ControllerBase
    {
        [HttpGet("/api/admin/user/")]
        public PaginatedResponse<IEnumerable<User>> GetAll(int pageNumber, int pageSize, string? keyword, bool isDeleted = false, bool isSuspended = false)
        {
            PagedList<User> data = _service.GetAll(pageNumber, pageSize, keyword, isDeleted, isSuspended);
            IOrderedEnumerable<User> users = data.OrderBy(m => m.CreatedAt);

            return new PaginatedResponse<IEnumerable<User>>
            {
                Responses = users,
                CurrentPage = pageNumber,
                TotalPages = data.TotalPages,
                TotalCount = data.TotalCount,
                HasNext = data.HasNext,
                HasPrevious = data.HasPrevious,
            };
        }

        [HttpGet("/api/admin/user/{id}/")]
        public async Task<User> GetById(string id, bool isDeleted, bool isSuspended)
        {
            User data = await _service.GetByIdAsync(id, isDeleted, isSuspended);
            return data;
        }

        [HttpPost("/api/admin/user/")]
        public async Task<ActionResult> Create(UserCreateAdminRequest userCreateRequest)
        {
            bool data = await _service.RegisterAsync(userCreateRequest);
            return data ? Ok() : BadRequest();
        }

        [HttpPut("/api/admin/user/{id}/")]
        public async Task<ActionResult> UpdateById(string id, UserUpdateAdminRequest userUpdateRequest)
        {
            bool data = await _service.UpdateAsync((User)userUpdateRequest, id, false, false);
            return data ? Ok() : BadRequest();
        }

        [HttpDelete("/api/admin/user/{id}/")]
        public async Task<ActionResult> DeleteById(string id)
        {
            bool data = await _service.DeleteAsync(id, false);
            return data ? Ok() : BadRequest();
        }
    }
}
