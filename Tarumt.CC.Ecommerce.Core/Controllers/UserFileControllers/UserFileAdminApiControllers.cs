using Ky.Web.CMS.SharedLibarary.Infrastructure.Requests.Admin;
using Ky.Web.CMS.SharedLibarary.Infrastructure.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;
using Tarumt.CC.Ecommerce.Infrastructure.Models;
using Tarumt.CC.Ecommerce.Services;

namespace Tarumt.CC.Ecommerce.Controllers.UserFileControllers
{
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    [ApiController]
    [IgnoreAntiforgeryToken]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class UserFileAdminApiControllers : ControllerBase
    {
        private readonly UserFileService _service;

        public UserFileAdminApiControllers(UserFileService service)
        {
            _service = service;
        }

        [HttpPost("/api/admin/user_file/")]
        public async Task<UserFileResponse> Create([FromForm] UserFileRequest userFileRequest)
        {
            UserFile data = await _service.Create(userFileRequest.file, "product/");
            return (UserFile)data;
        }
    }
}
