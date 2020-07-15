using Elk.Http;
using System.Linq;
using Pharmacy.DataAccess.Ef;
using Pharmacy.DependencyResolver;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Pharmacy.Store
{
    public class Startup
    {
        public IConfiguration _configuration { get; }

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews()
                .AddJsonOptions(opts =>
                {
                    opts.JsonSerializerOptions.PropertyNamingPolicy = null;
                    opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });
            services.AddContext<AppDbContext>(_configuration.GetConnectionString("EfDbContext"));
            services.AddContext<AuthDbContext>(_configuration.GetConnectionString("AuthDbContext"));

            services.AddMemoryCache();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(opt =>
            {
                opt.Cookie.SameSite = SameSiteMode.Lax;
            });

            services.AddHttpContextAccessor();

            services.AddTransient(_configuration);
            services.AddScoped(_configuration);
            services.AddSingleton(_configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStaticFiles();
            }
            else
            {
                var cachePeriod = env.IsDevelopment() ? "1" : "604800";
                app.UseStaticFiles(new StaticFileOptions
                {
                    OnPrepareResponse = ctx => { ctx.Context.Response.Headers.Append("Cache-Control", $"public, max-age={cachePeriod}"); }
                });
                app.UseExceptionHandler("/Home/Error");
                //app.UseHsts();
            }
            //app.UseHttpsRedirection();
            if (!env.IsDevelopment())
                app.Use(async (context, next) =>
                {
                    await next.Invoke();
                    if (!context.Request.IsAjaxRequest())
                    {
                        var handled = context.Features.Get<IStatusCodeReExecuteFeature>();
                        var statusCode = context.Response.StatusCode;
                        if (handled == null && statusCode >= 400)
                            context.Response.Redirect($"/Error/Details?code={statusCode}");
                    }

                });

            app.UseRouting();

            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
