using Ky.Web.CMS.SharedLibarary.Infrastructure.Requests.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;
using Tarumt.CC.Ecommerce.Infrastructure.Models;
using Tarumt.CC.Ecommerce.Services;

namespace Tarumt.CC.Ecommerce.Controllers.ServerSettingControllers
{
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    [ApiController]
    [IgnoreAntiforgeryToken]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class ServerSettingAdminApiController : ControllerBase
    {
        private readonly ServerSettingService _service;

        public ServerSettingAdminApiController(ServerSettingService service)
        {
            _service = service;
        }

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
            if (!data)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
