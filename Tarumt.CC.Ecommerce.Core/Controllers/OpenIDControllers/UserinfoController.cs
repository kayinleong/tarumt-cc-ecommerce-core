using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using Tarumt.CC.Ecommerce.Core.Infrastructure.Models;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Tarumt.CC.Ecommerce.Core.Controllers.OpenIDControllers
{
    [Authorize(AuthenticationSchemes = OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)]
    [ApiController]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class UserinfoController : ControllerBase
    {
        [HttpGet("/connect/userinfo")]
        [HttpPost("/connect/userinfo")]
        public async Task<IActionResult> Userinfo()
        {
            User user = (HttpContext.Items["User"] as User)!;
            Dictionary<string, object> claims = new Dictionary<string, object>(StringComparer.Ordinal)
            {
                [Claims.Subject] = user.Id
            };

            if (User.HasScope(Scopes.Profile))
            {
                claims[Claims.Profile] = new
                {
                    user.Username,
                    user.FirstName,
                    user.LastName,
                    user.Gender,
                    user.Culture,
                    user.IsAdmin,
                    DateOfBirth = user.DateOfBirth.ToString("dd/MM/yyyy"),
                };
            }

            if (User.HasScope(Scopes.Email))
            {
                claims[Claims.Email] = user.Email;
                claims[Claims.EmailVerified] = user.IsEmailVerified;
            }

            if (User.HasScope(Scopes.Phone))
            {
                claims[Claims.PhoneNumber] = user.PhoneNumber;
                claims[Claims.PhoneNumberVerified] = user.IsPhoneNumberVerified;
            }

            if (User.HasScope(Scopes.Address))
            {
                claims[Claims.Address] = user.Address ?? string.Empty;
            }

            return Ok(claims);
        }
    }
}
