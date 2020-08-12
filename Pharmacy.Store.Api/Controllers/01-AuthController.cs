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
using Microsoft.AspNetCore.Cors;

namespace Pharmacy.API.Controllers
{
    [EnableCors("AllowedOrigins")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        readonly IUserService _userSrv;
        readonly IConfiguration _config;
        readonly IOptions<CustomSetting> _settings;
        public AuthController(IUserService userSrv, IConfiguration config, IOptions<CustomSetting> settings)
        {
            _userSrv = userSrv;
            _config = config;
            _settings = settings;
        }

        [NonAction]
        private string CreateToken(User user, int timeout)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_settings.Value.Jwt.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.MobilePhone, user.MobileNumber.ToString()),
                new Claim(ClaimTypes.Name, user.FullName.ToString()),
                new Claim(ClaimTypes.Email, user.Email.ToString())};
            var token = new JwtSecurityToken(_settings.Value.Jwt.Issuer,
              _settings.Value.Jwt.Issuer,
              claims,
              expires: DateTime.Now.AddMinutes(timeout),
              signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost, Route("SignUp")]
        public async Task<ActionResult<IResponse<string>>> SignUp(SignUpModel model)
        {
            return new Response<string>
            {
                IsSuccessful = true,
                Result = model.MobileNumber
            };
            if (!ModelState.IsValid) return new Response<string> { Message = ModelState.GetModelError() };
            var add = await _userSrv.SignUp(model, _settings.Value.EndUserRoleId);
            if (!add.IsSuccessful) return new Response<string> { Message = add.Message };
            return new Response<string>
            {
                IsSuccessful = true,
                Result = model.MobileNumber
            };
        }

        [HttpPost, Route("Signin")]
        public async Task<ActionResult<IResponse<AuthResponse>>> SignIn(SignInModel model)
        {
            return new Response<AuthResponse>
            {
                IsSuccessful = true,
                Result = new AuthResponse
                {
                    Email = "kingofday.b@gmail.com",
                    Fullname = "شهروز بذرافشان",
                    IsConfirmed = true,
                    MobileNumber = model.Username,
                    Token = CreateToken(new User
                    {
                        MobileNumber = long.Parse(model.Username),
                        Email = "kingofday.b@gmail.com",
                        FullName = "شهروز بذرافشان"
                    }, _settings.Value.Jwt.TimoutInMinutes)
                }
            };
            //if (!ModelState.IsValid) return new Response<AuthResponse> { Message = ModelState.GetModelError() };
            //var add = await _userSrv.SignIn(long.Parse(model.Username), model.Password);
            //if (!add.IsSuccessful) return new Response<AuthResponse> { Message = add.Message };
            //return new Response<AuthResponse>
            //{
            //    IsSuccessful = true,
            //    Result = add.Result
            //};
        }


        [HttpPost, Route("Confirm")]
        public async Task<ActionResult<IResponse<AuthResponse>>> Confirm([FromServices] IOptions<CustomSetting> settings,ConfirmModel model)
        //{
        //    if (!ModelState.IsValid) return new Response<AuthResponse> { Message = ModelState.GetModelError() };
        //    var conf = await _userSrv.Confirm(long.Parse(model.MobileNumber), int.Parse(code));
        //    if (conf.IsSuccessful)
        //    {
        //        conf.Result.IsConfirmed = true;
        //        conf.Result.Token = CreateToken(new User
        //        {
        //            MobileNumber = long.Parse(model.MobileNumber),
        //            Email = conf.Result.Email,
        //            FullName = conf.Result.Fullname
        //        }, settings.Value.Jwt.TimoutInMinutes);
        //    }
        //    return conf;
        //}
        => new Response<AuthResponse>
        {
            IsSuccessful = true,
            Result = new AuthResponse
            {
                Email = "kingofday.b@gmail.com",
                Fullname = "شهروز بذرافشان",
                IsConfirmed = true,
                MobileNumber = model.MobileNumber.ToString(),
                Token = CreateToken(new User
                {
                    MobileNumber = long.Parse(model.MobileNumber),
                    Email = "kingofday.b@gmail.com",
                    FullName = "شهروز بذرافشان"
                }, settings.Value.Jwt.TimoutInMinutes)
            }
        };

        [HttpPost, Route("auth/{mobileNumber:long}")]
        public async Task<ActionResult<IResponse<bool>>> Resend(long mobileNumber)
            => new Response<bool> { IsSuccessful = true };
        //=> await _userSrv.Resend(mobileNumber);
    }
}