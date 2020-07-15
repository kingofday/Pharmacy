using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Domain;

namespace Pharmacy.Dashboard.Controllers
{
    public class AuthBaseController : Controller
    {
        IHttpContextAccessor _httpAccessor;
        public AuthBaseController(IHttpContextAccessor httpAccessor)
        {
            _httpAccessor = httpAccessor;
        }
        protected async Task CreateCookie(User user, bool remeberMe)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.MobileNumber.ToString()),
                new Claim(ClaimTypes.MobilePhone, user.MobileNumber.ToString()),
                new Claim("Fullname", user.FullName)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8),
                IsPersistent = remeberMe,
            };
            await _httpAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }
    }
}