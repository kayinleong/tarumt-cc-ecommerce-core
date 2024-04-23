using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;
using Tarumt.CC.Ecommerce.Core.Infrastructure.Models;
using Tarumt.CC.Ecommerce.Core.Services;
using Tarumt.CC.Ecommerce.SharedLibrary.Infrastructure.Requests;

namespace Tarumt.CC.Ecommerce.Core.Controllers.UserControllers
{
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    [ApiController]
    [IgnoreAntiforgeryToken]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class UserApiController(UserService _service) : ControllerBase
    {
        [HttpPut("/api/user/")]
        public async Task<ActionResult> Update(UserUpdateRequest userUpdateRequest)
        {
            User user = (HttpContext.Items["User"] as User)!;
            var data = await _service.UpdateAsync((User)userUpdateRequest, user.Id, false, false);
            return data ? Ok() : BadRequest();
        }
    }
}
