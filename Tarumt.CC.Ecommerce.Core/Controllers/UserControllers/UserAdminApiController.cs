using Ky.Web.CMS.SharedLibarary.Infrastructure.Requests.Admin;
using Ky.Web.CMS.SharedLibarary.Infrastructure.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;
using Tarumt.CC.Ecommerce.Infrastructure.Models;
using Tarumt.CC.Ecommerce.Services;

namespace Tarumt.CC.Ecommerce.Controllers.UserControllers
{
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    [ApiController]
    [IgnoreAntiforgeryToken]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class UserAdminApiController : ControllerBase
    {
        private readonly UserService _service;

        public UserAdminApiController(UserService service)
        {
            _service = service;
        }

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
            if (!data)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPut("/api/admin/user/{id}/")]
        public async Task<ActionResult> UpdateById(string id, UserUpdateAdminRequest userUpdateRequest)
        {
            bool data = await _service.UpdateAsync((User)userUpdateRequest, id, false, false);
            if (!data)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpDelete("/api/admin/user/{id}/")]
        public async Task<ActionResult> DeleteById(string id)
        {
            bool data = await _service.DeleteAsync(id, false);
            if (!data)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
