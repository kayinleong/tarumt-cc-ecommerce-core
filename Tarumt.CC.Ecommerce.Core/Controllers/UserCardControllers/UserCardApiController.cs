using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;
using Tarumt.CC.Ecommerce.Core.Infrastructure.Models;
using Tarumt.CC.Ecommerce.Core.Services;
using Tarumt.CC.Ecommerce.SharedLibrary.Infrastructure.Requests;
using Tarumt.CC.Ecommerce.SharedLibrary.Infrastructure.Responses;

namespace Tarumt.CC.Ecommerce.Core.Controllers.UserCardControllers
{
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    [ApiController]
    [IgnoreAntiforgeryToken]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class UserCardApiController(UserCardService _service) : ControllerBase
    {
        [HttpGet("/api/user_card/")]
        public async Task<UserCardResponse> GetById()
        {
            User user = (HttpContext.Items["User"] as User)!;

            var data = await _service.GetByUserIdAsync(user.Id, false);
            return (UserCardResponse)data;
        }

        [HttpPost("/api/user_card/")]
        public async Task<ActionResult> Create(UserCardRequest userCardRequest)
        {
            User user = (HttpContext.Items["User"] as User)!;

            var data = await _service.AddAsync(userCardRequest, user);
            return data ? Ok() : BadRequest();
        }

        [HttpPost("/api/user_card/verify/")]
        public async Task<ActionResult> Verify(UserCardRequest userCardRequest)
        {
            User user = (HttpContext.Items["User"] as User)!;
            UserCard userCard = await _service.GetByUserIdAsync(user.Id, false);

            var data = await _service.VerifyCardAsync(userCardRequest, userCard.Id);
            return data ? Ok() : BadRequest();
        }

        [HttpPut("/api/user_card/")]
        public async Task<ActionResult> Update(UserCardRequest userCardRequest)
        {
            User user = (HttpContext.Items["User"] as User)!;
            UserCard userCard = await _service.GetByUserIdAsync(user.Id, false);

            var data = await _service.UpdateByIdAsync(userCardRequest, userCard.Id);
            return data ? Ok() : BadRequest();
        }

        [HttpDelete("/api/user_card/")]
        public async Task<ActionResult> Delete()
        {
            User user = (HttpContext.Items["User"] as User)!;
            UserCard userCard = await _service.GetByUserIdAsync(user.Id, false);

            var data = await _service.DeleteByIdAsync(userCard.Id);
            return data ? Ok() : BadRequest();
        }
    }
}
