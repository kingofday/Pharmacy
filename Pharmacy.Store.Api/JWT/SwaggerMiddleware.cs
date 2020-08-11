using Microsoft.AspNetCore.Builder;

namespace Pharmacy.API
{
    public static class SwaggerMiddleware
    {
        public static IApplicationBuilder UseCustomizedSwagger(this IApplicationBuilder app)
        {
            app.UseStaticFiles();
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = "help";
            });
            return app;
        }
    }
}
