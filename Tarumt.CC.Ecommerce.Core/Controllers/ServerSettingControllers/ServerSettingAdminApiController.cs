using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;
using Tarumt.CC.Ecommerce.Core.Infrastructure.Models;
using Tarumt.CC.Ecommerce.Core.Services;
using Tarumt.CC.Ecommerce.SharedLibrary.Infrastructure.Requests.Admin;

namespace Tarumt.CC.Ecommerce.Core.Controllers.ServerSettingControllers
{
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    [ApiController]
    [IgnoreAntiforgeryToken]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class ServerSettingAdminApiController(ServerSettingService _service) : ControllerBase
    {
        [HttpGet("/api/admin/server_settings/")]
        public async Task<ServerSetting> Get()
        {
            ServerSetting data = await _service.GetAsync();
            return data;
        }

        [HttpPut("/api/admin/server_settings/")]
        public async Task<ActionResult> Update(ServerSettingAdminRequest serverSettingAdminRequest)
        {
            bool data = await _service.UpdateAsync(serverSettingAdminRequest);
            return data ? Ok() : BadRequest();
        }
    }
}
