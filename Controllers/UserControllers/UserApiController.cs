using Ky.Web.CMS.SharedLibarary.Infrastructure.Requests;
using Microsoft.AspNetCore.Mvc;
using Tarumt.CC.Ecommerce.Services;

namespace Tarumt.CC.Ecommerce.Controllers.UserControllers
{
    [ApiController]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class UserApiController : ControllerBase
    {
        private readonly UserService _service;

        public UserApiController(UserService service)
        {
            _service = service;
        }

        [HttpPost("/api/user/")]
        public async Task<ActionResult> Create(UserCreateRequest userCreateRequest)
        {
            if (!await _service.RegisterAsync(userCreateRequest))
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
