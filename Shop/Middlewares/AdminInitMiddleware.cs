using Shop.Services;

namespace Shop.Middlewares
{
    public class AdminInitMiddleware
    {
        private readonly RequestDelegate _next;

        public AdminInitMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var service = context.RequestServices.GetRequiredService<AdminInitializer>();
            if (service != null) await service.InitializeAdminAsync();
            await _next(context);
        }
    }

    public static class AdminInitExtention {
        public static IApplicationBuilder InitAdmin(this IApplicationBuilder app)
        {
            return app.UseMiddleware<AdminInitMiddleware>();
        }
    }
}
