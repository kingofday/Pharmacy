using Elk.Http;
using System.Linq;
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
using Pharmacy.Domain;
using Pharmacy.API.JWT;
using Microsoft.AspNetCore.Cors;

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
            services.AddControllers()
                .AddJsonOptions(opts =>
                {
                    opts.JsonSerializerOptions.PropertyNamingPolicy = null;
                    opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });
            services.AddCors(options =>
            {
                options.AddPolicy(name: AllowedOrigins,
                                  builder =>
                                  {
                                      builder
                                            .AllowAnyOrigin()
                                            //.WithOrigins("https://localhost:44328")
                                            .AllowAnyMethod()
                                            .AllowAnyHeader()
                                            .SetIsOriginAllowed(hostName => true);
                                            //.AllowCredentials();

                                  });
            });
            services.UseCustomizedJWT(_configuration);
            services.AddMemoryCache();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(opt =>
            {
                opt.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax;
            });
            services.AddHttpContextAccessor();
            //services.AddOptions();
            
            services.Configure<CustomSetting>(_configuration.GetSection("CustomSettings"));
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
            app.UseAuthorization();
            //app.UseCustomizedSwagger();
            app.UseCors(AllowedOrigins);
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
