using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using OpenIddict.Validation.AspNetCore;
using System.Security.Claims;
using Tarumt.CC.Ecommerce.Core.Services;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Tarumt.CC.Ecommerce.Core.Middlewares
{
    public class UserMiddleware
    {
        private readonly RequestDelegate _requestDelegate;

        public UserMiddleware(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }

        public async Task InvokeAsync(HttpContext httpContext, UserService userService)
        {
            if (httpContext.Request.Path.Value!.StartsWith("/api/token/"))
            {
                await _requestDelegate(httpContext);
                return;
            }

            try
            {
                AuthenticateResult authenticateResult = await httpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                if (authenticateResult != null && authenticateResult.Succeeded)
                {
                    ClaimsIdentity identity = authenticateResult.Principal.Identity as ClaimsIdentity;
                    Claim claim = authenticateResult.Principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
                    string userId = claim.Value;
                    if (string.IsNullOrEmpty(userId))
                    {
                        await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                        httpContext.Response.Redirect("/account/login");
                        return;
                    }

                    Infrastructure.Models.User user = await userService.GetByIdAsync(userId, false, false);
                    if (user == null || user.IsDeleted || user.IsSuspended)
                    {
                        await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                        httpContext.Response.Redirect("/account/login");
                        return;
                    }

                    if (httpContext.Request.Path.Value.StartsWith("/api/admin/") && !user.IsAdmin)
                    {
                        await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                        httpContext.Response.Redirect("/account/login");
                        return;
                    }

                    httpContext.Items.Add("User", user);
                    httpContext.Items.Add("UserMfa", user.UserMfa);
                }
                else
                {
                    ClaimsPrincipal claimsPrincipal = (await httpContext.AuthenticateAsync(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)).Principal;
                    string userId = claimsPrincipal.Claims.FirstOrDefault(m => m.Type == Claims.Subject).Value;

                    Infrastructure.Models.User user = await userService.GetByIdAsync(userId, false, false);
                    if (user == null || user.IsDeleted || user.IsSuspended)
                    {
                        await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                        httpContext.Response.Redirect("/account/login");
                        return;
                    }

                    if (httpContext.Request.Path.Value.StartsWith("/api/admin/") && !user.IsAdmin)
                    {
                        await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                        httpContext.Response.Redirect("/account/login");
                        return;
                    }

                    httpContext.Items.Add("User", user);
                    httpContext.Items.Add("UserMfa", user.UserMfa);
                }
            }
            catch { }

            await _requestDelegate(httpContext);
        }
    }
}
