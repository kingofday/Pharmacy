using Pharmacy.Service;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Domain;
using System.Collections.Generic;
using Elk.Core;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System;
using Elk.AspNetCore;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.Web.CodeGeneration;

namespace Pharmacy.Store.Api.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        readonly IUserService _userSrv;
        readonly IConfiguration _config;
        public AuthController(IUserService userSrv, IConfiguration config)
        {
            _userSrv = userSrv;
            _config = config;
        }

        [NonAction]
        private string CreateToken(User user, int timeout)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["CustomSettings:Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.MobilePhone, user.MobileNumber.ToString()),
                new Claim(ClaimTypes.Name, user.FullName.ToString()),
                new Claim(ClaimTypes.Email, user.Email.ToString())};
            var token = new JwtSecurityToken(_config["CustomSettings:Jwt:Issuer"],
              _config["CustomSettings:Jwt:Issuer"],
              claims,
              expires: DateTime.Now.AddMinutes(timeout),
              signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost, Route("[controller]")]
        public async Task<ActionResult<IResponse<string>>> SignUp([FromServices] IOptions<CustomSetting> settings, SignUpModel model)
        {
            if (true)
            {
                var t = CreateToken(new Domain.User
                {
                    MobileNumber = long.Parse(model.MobileNumber),
                    Email = model.Email,
                    FullName = model.Fullname
                }, settings.Value.Jwt.TimoutInMinutes);
                return new Response<string>
                {
                    IsSuccessful = true,
                    Result = model.MobileNumber
                };
            }
            if (!ModelState.IsValid) return new Response<string> { Message = ModelState.GetModelError() };
            var add = await _userSrv.SignUp(model, settings.Value.EndUserRoleId);
            if (!add.IsSuccessful) return new Response<string> { Message = add.Message };
            var token = CreateToken(add.Result, settings.Value.Jwt.TimoutInMinutes);
            return new Response<string>
            {
                IsSuccessful = true,
                Result = model.MobileNumber
            };
        }

        [HttpPost, Route("auth/{mobileNumber:long}")]
        public async Task<ActionResult<IResponse<AuthResponse>>> Confirm(long mobileNumber, AuthConfirm model)
            => await _userSrv.Confirm(mobileNumber, int.Parse(model.Code));
    }
}