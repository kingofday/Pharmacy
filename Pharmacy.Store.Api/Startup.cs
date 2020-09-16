using Elk.Core;
using System.Linq;
using Pharmacy.Domain;
using Microsoft.AspNetCore.Http;
using Pharmacy.DependencyResolver;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Pharmacy.API
{
    public class Startup
    {
        readonly string AllowedOrigins = "AllowedOrigins";
        public IConfiguration _configuration { get; }

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(opts => {
                opts.EnableEndpointRouting = false;
                opts.ReturnHttpNotAcceptable = true;
            })
                .AddJsonOptions(opts =>
                {
                    opts.JsonSerializerOptions.PropertyNamingPolicy = null;
                    opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });
            services.AddCors(options =>
            {
                options.AddPolicy(AllowedOrigins, builder =>
                {
                    builder
                        .WithOrigins(_configuration["CustomSettings:ReactBaseUrl"])
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });
            services.UseCustomizedJWT(_configuration);
            services.AddMemoryCache();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(opt =>
            {
                opt.Cookie.SameSite = SameSiteMode.Lax;
            });
            services.AddHttpContextAccessor();
            
            services.Configure<APICustomSetting>(_configuration.GetSection("CustomSettings"));
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
            }
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    var errorhandler = context.Features.Get<IExceptionHandlerPathFeature>();
                    FileLoger.Error(errorhandler.Error);
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/Json";
                    var bytes = System.Text.Encoding.ASCII.GetBytes(new { IsSuccessful = false, Message= errorhandler.Error?.Message }.ToString());
                    await context.Response.Body.WriteAsync(bytes, 0, bytes.Length);
                });
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            //app.UseCors(AllowedOrigins);
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //});
            app.UseCors(AllowedOrigins);
            app.UseMvcWithDefaultRoute();
        }
    }
}
