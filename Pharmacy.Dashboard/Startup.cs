using Elk.Http;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Pharmacy.DependencyResolver;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Pharmacy.Dashboard
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IConfiguration _configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews()
             .AddJsonOptions(opts =>
             {
                 opts.JsonSerializerOptions.PropertyNamingPolicy = null;
                 opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
             });
            //services.AddContext<AppDbContext>(_configuration.GetConnectionString("EfDbContext"));
            //services.AddContext<AuthDbContext>(_configuration.GetConnectionString("AuthDbContext"));

            services.AddMemoryCache();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(opt =>
            {
                opt.Cookie.SameSite = SameSiteMode.Lax;
            });

            services.AddHttpContextAccessor();

            services.AddTransient(_configuration);
            services.AddScoped(_configuration);
            services.AddSingleton(_configuration);
            services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-CSRF-TOKEN"; //may be any other valid header name
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
                app.UseExceptionHandler("/Error/Details");
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
                        {
                            context.Response.Redirect($"/Error/Details?code={statusCode}");
                        }
                    }

                });

            app.UseRouting();

            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=StorePublic}/{action=SignUp}");
            });
        }
    }
}
