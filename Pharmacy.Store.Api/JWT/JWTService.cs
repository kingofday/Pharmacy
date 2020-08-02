using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Pharmacy.Store.Api
{
    public static class JWTService
    {
        public static void UseCustomizedJWT(this IServiceCollection services, IConfiguration configure)
        {
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configure["CustomSettings:Jwt:Issuer"],
                    ValidAudience = configure["CustomSettings:Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configure["CustomSettings:Jwt:Key"]))
                };
            });
        }
    }
}
