namespace Tarumt.CC.Ecommerce.Middlewares
{
    public static class UserMiddlewareExtension
    {
        public static IApplicationBuilder UseUserMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<UserMiddleware>();
        }
    }
}
