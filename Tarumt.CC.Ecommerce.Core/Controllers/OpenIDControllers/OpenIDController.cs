using Ky.Web.CMS.SharedLibarary.ViewModels;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Security.Claims;
using Tarumt.CC.Ecommerce.Attributes;
using Tarumt.CC.Ecommerce.Extensions;
using Tarumt.CC.Ecommerce.Infrastructure.Models;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Tarumt.CC.Ecommerce.Controllers.OpenIDControllers
{
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class OpenIDController : Controller
    {
        private readonly IOpenIddictApplicationManager _applicationManager;
        private readonly IOpenIddictAuthorizationManager _authorizationManager;
        private readonly IOpenIddictScopeManager _scopeManager;

        public OpenIDController(
            IOpenIddictApplicationManager applicationManager,
            IOpenIddictAuthorizationManager authorizationManager,
            IOpenIddictScopeManager scopeManager)
        {
            _applicationManager = applicationManager;
            _authorizationManager = authorizationManager;
            _scopeManager = scopeManager;
        }

        [HttpPost("/connect/token")]
        public async Task<IActionResult> Token()
        {
            OpenIddictRequest request = HttpContext.GetOpenIddictServerRequest()
                ?? throw new InvalidOperationException("The OpenID Connect request cannot be retrieved");

            if (request.IsAuthorizationCodeGrantType() || request.IsRefreshTokenGrantType())
            {
                return await HandleExchangeCodeGrantType();
            }

            if (request.IsClientCredentialsGrantType())
            {
                return await HandleExchangeClientCredentialsGrantType(request);
            }

            throw new InvalidOperationException("The specific grant type is not supported");
        }

        [HttpGet("/connect/authorize"), HttpPost("/connect/authorize")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Authorize()
        {
            OpenIddictRequest request = HttpContext.GetOpenIddictServerRequest()
                ?? throw new InvalidOperationException("The OpenId Connect request cannot be retrieved");

            if (request.HasPrompt(Prompts.Login))
            {
                string prompt = string.Join(" ", request.GetPrompts().Remove(Prompts.Login));
                List<KeyValuePair<string, StringValues>> parameters = Request.HasFormContentType
                    ? Request.Form.Where(parameter => parameter.Key != Parameters.Prompt).ToList()
                    : Request.Query.Where(parameter => parameter.Key != Parameters.Prompt).ToList();

                parameters.Add(KeyValuePair.Create(Parameters.Prompt, new StringValues(prompt)));

                return Challenge(
                    authenticationSchemes: CookieAuthenticationDefaults.AuthenticationScheme,
                    properties: new AuthenticationProperties
                    {
                        RedirectUri = Request.PathBase + Request.Path + QueryString.Create(
                            Request.HasFormContentType ? Request.Form.ToList() : Request.Query.ToList())
                    });
            }

            AuthenticateResult result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (result == null || !result.Succeeded ||
                (request.MaxAge != null &&
                 result.Properties?.IssuedUtc != null &&
                 DateTimeOffset.UtcNow - result.Properties.IssuedUtc > TimeSpan.FromSeconds(request.MaxAge.Value)))
            {
                if (request.HasPrompt(Prompts.None))
                {
                    return Forbid(
                        authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                        properties: new AuthenticationProperties(new Dictionary<string, string>
                        {
                            [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.LoginRequired,
                            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The user is not logged in."
                        }!));
                }

                return Challenge(
                    authenticationSchemes: CookieAuthenticationDefaults.AuthenticationScheme,
                    properties: new AuthenticationProperties
                    {
                        RedirectUri = Request.PathBase + Request.Path + QueryString.Create(
                            Request.HasFormContentType ? Request.Form.ToList() : Request.Query.ToList())
                    });
            }

            if (HttpContext.Items["User"] is not User user)
            {
                throw new InvalidOperationException("User is invalid");
            }

            if (user.IsDeleted)
            {
                throw new InvalidOperationException("User is invalid");
            }

            object application = await _applicationManager.FindByClientIdAsync(request.ClientId
                ?? throw new InvalidOperationException("Application cannot be found"))
                    ?? throw new InvalidOperationException("Application cannot be found");

            string applicationId = await _applicationManager.GetIdAsync(application)
                ?? throw new InvalidOperationException("Application cannot be found");

            string applicationDisplayName = await _applicationManager.GetDisplayNameAsync(application)
                ?? throw new InvalidOperationException("Application cannot be found");

            List<object> authorizations = await _authorizationManager.FindAsync(
                user.Id,
                applicationId,
                Statuses.Valid,
                AuthorizationTypes.Permanent,
                request.GetScopes()).ToListAsync();

            switch (await _applicationManager.GetConsentTypeAsync(application))
            {
                case ConsentTypes.External when !authorizations.Any():
                    return Forbid(
                        authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                        properties: new AuthenticationProperties(new Dictionary<string, string>
                        {
                            [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.ConsentRequired,
                            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                                "The logged in user is not allowed to access this client application."
                        }!));

                case ConsentTypes.Implicit:
                case ConsentTypes.External when authorizations.Any():
                case ConsentTypes.Explicit when authorizations.Any() && !request.HasPrompt(Prompts.Consent):
                    List<Claim> claims = new()
                    {
                        new(Claims.Subject, user.Id)
                    };

                    ClaimsIdentity claimsIdentity = new(claims, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
                    claimsIdentity.SetDestinations(GetDestinations);
                    ClaimsPrincipal claimsPrincipal = new(claimsIdentity);

                    claimsPrincipal.SetScopes(request.GetScopes());
                    claimsPrincipal.SetResources(await _scopeManager.ListResourcesAsync(claimsPrincipal.GetScopes())
                        .ToListAsync());
                    claimsPrincipal.SetAudiences("ky_web_cms_core");

                    object authorization = authorizations.LastOrDefault()!;
                    authorization ??= await _authorizationManager.CreateAsync(
                        claimsPrincipal,
                        user.Id,
                        applicationId,
                        AuthorizationTypes.Permanent,
                        claimsPrincipal.GetScopes());

                    claimsPrincipal.SetAuthorizationId(await _authorizationManager.GetIdAsync(authorization));

                    return SignIn(claimsPrincipal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

                case ConsentTypes.Explicit when request.HasPrompt(Prompts.None):
                case ConsentTypes.Systematic when request.HasPrompt(Prompts.None):
                    return Forbid(
                        authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                        properties: new AuthenticationProperties(new Dictionary<string, string>
                        {
                            [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.ConsentRequired,
                            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                                "Interactive user consent is required."
                        }!));

                default:
                    return View(new ConsentFormViewModel
                    {
                        ApplicationName = applicationDisplayName,
                        Scope = request.Scope!,
                    });
            }
        }

        [HttpPost("/connect/authorize")]
        [Authorize]
        [ValidateAntiForgeryToken]
        [FormValueRequired("consent.Accept")]
        public async Task<IActionResult> AuthorizeAccept()
        {
            OpenIddictRequest request = HttpContext.GetOpenIddictServerRequest()
                ?? throw new InvalidOperationException("The OpenID Connect request cannot be retrieved");

            if (HttpContext.Items["User"] is not User user)
            {
                throw new InvalidOperationException("User is invalid");
            }

            if (user.IsDeleted)
            {
                throw new InvalidOperationException("User is invalid");
            }

            object application = await _applicationManager.FindByClientIdAsync(request.ClientId
                ?? throw new InvalidOperationException("Application cannot be found"))
                    ?? throw new InvalidOperationException("Application cannot be found");

            string applicationId = await _applicationManager.GetIdAsync(application)
                ?? throw new InvalidOperationException("Application cannot be found");

            List<object> authorizations = await _authorizationManager.FindAsync(
               user.Id,
               applicationId,
               Statuses.Valid,
               AuthorizationTypes.Permanent,
               request.GetScopes()).ToListAsync();

            if (!authorizations.Any() && await _applicationManager.HasClientTypeAsync(application, ConsentTypes.External))
            {
                return Forbid(
                    authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                    properties: new AuthenticationProperties(new Dictionary<string, string>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.ConsentRequired,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                            "The logged in user is not allowed to access this client application."
                    }!));
            }

            List<Claim> claims = new()
            {
                new(Claims.Subject, user.Id)
            };

            ClaimsIdentity claimsIdentity = new(claims, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            claimsIdentity.SetDestinations(GetDestinations);

            ClaimsPrincipal claimsPrincipal = new(claimsIdentity);
            claimsPrincipal.SetScopes(request.GetScopes());
            claimsPrincipal.SetResources(await _scopeManager.ListResourcesAsync(claimsPrincipal.GetScopes()).ToListAsync());

            object authorization = authorizations.LastOrDefault()!;
            authorization ??= await _authorizationManager.CreateAsync(
                claimsPrincipal,
                user.Id,
                applicationId,
                AuthorizationTypes.Permanent,
                claimsPrincipal.GetScopes());

            claimsPrincipal.SetAuthorizationId(await _authorizationManager.GetIdAsync(authorization));
            claimsPrincipal.SetAudiences("ky_web_cms_core");

            return SignIn(claimsPrincipal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        [HttpPost("/connect/authorize")]
        [Authorize]
        [ValidateAntiForgeryToken]
        [FormValueRequired("consent.Deny")]
        public IActionResult AuthorizeDeny()
        {
            return Forbid(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        [HttpGet("/connect/logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return SignOut(
                authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                properties: new AuthenticationProperties
                {
                    RedirectUri = "/"
                });
        }

        private async Task<IActionResult> HandleExchangeCodeGrantType()
        {
            ClaimsPrincipal? principal = (await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)).Principal;

            return SignIn(principal!, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        private async Task<IActionResult> HandleExchangeClientCredentialsGrantType(OpenIddictRequest request)
        {
            object application = await _applicationManager.FindByClientIdAsync(request.ClientId
                ?? throw new InvalidOperationException("The application cannot be found"))
                    ?? throw new InvalidOperationException("The application cannot be found");

            ClaimsIdentity identity = new(
                OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                Claims.Name,
                Claims.Role);

            string clientId = await _applicationManager.GetClientIdAsync(application)
                ?? throw new InvalidOperationException("The application cannot be found");

            string clientDisplayName = await _applicationManager.GetDisplayNameAsync(application)
                ?? throw new InvalidOperationException("The application cannot be found");

            identity.AddClaim(Claims.Subject, clientId);
            identity.AddClaim(Claims.Name, clientDisplayName);
            identity.SetDestinations(GetDestinations);

            ClaimsPrincipal principal = new(identity);
            principal.SetScopes(request.GetScopes());
            principal.SetResources(await _scopeManager.ListResourcesAsync(principal.GetScopes()).ToListAsync());
            principal.SetAudiences("ky_web_cms_core");

            return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        private static IEnumerable<string> GetDestinations(Claim claim)
        {
            return claim.Type switch
            {
                Claims.Name or Claims.Subject => new[] { Destinations.AccessToken, Destinations.IdentityToken },
                _ => new[] { Destinations.AccessToken }
            };
        }
    }
}
