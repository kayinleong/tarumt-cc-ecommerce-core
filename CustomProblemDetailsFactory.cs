using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Tarumt.CC.Ecommerce
{
    public class CustomProblemDetailsFactory : ProblemDetailsFactory
    {
        public override ProblemDetails CreateProblemDetails(HttpContext httpContext, int? statusCode = null, string? title = null, string? type = null, string? detail = null, string? instance = null)
        {
            statusCode ??= 500;
            ProblemDetails problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title ?? "A server error occurred.",
                Type = type,
                Detail = detail,
                Instance = instance
            };

            return problemDetails;
        }

        public override ValidationProblemDetails CreateValidationProblemDetails(HttpContext httpContext, ModelStateDictionary modelStateDictionary, int? statusCode = null, string? title = null, string? type = null, string? detail = null, string? instance = null)
        {
            statusCode ??= 400;
            ValidationProblemDetails problemDetails = new ValidationProblemDetails(modelStateDictionary)
            {
                Status = statusCode,
                Title = title ?? "One or more validation errors occurred.",
                Type = type,
                Detail = detail,
                Instance = instance
            };

            return problemDetails;
        }
    }
}
