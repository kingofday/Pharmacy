﻿using Pharmacy.Service;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Domain;
using Elk.Core;
using Microsoft.IdentityModel.Tokens;
using System.Text;
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
        readonly APICustomSetting _settings;
        public AuthController(IUserService userSrv, IOptions<APICustomSetting> settings)
        {
            _userSrv = userSrv;
            _settings = settings.Value;
        }

        [NonAction]
        private string CreateToken(User user, int timeout)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_settings.Jwt.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.MobilePhone, user.MobileNumber.ToString()),
                new Claim(ClaimTypes.Name, user.FullName.ToString()),
                new Claim(ClaimTypes.Email, user.Email.ToString())};
            var token = new JwtSecurityToken(_settings.Jwt.Issuer,
              _settings.Jwt.Issuer,
              claims,
              expires: DateTime.Now.AddMinutes(timeout),
              signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost, Route("SignUp")]
        public async Task<ActionResult<IResponse<string>>> SignUp(SignUpModel model)
        {
            if (!ModelState.IsValid) return new Response<string> { Message = ModelState.GetModelError() };
            var add = await _userSrv.SignUp(model, _settings.EndUserRoleId);
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
            if (!ModelState.IsValid) return new Response<AuthResponse> { Message = ModelState.GetModelError() };
            var auth = await _userSrv.SignIn(long.Parse(model.Username), model.Password);
            if (!auth.IsSuccessful) return new Response<AuthResponse> { Message = auth.Message };
            if (auth.Result.IsConfirmed) auth.Result.Token = CreateToken(new User
            {
                UserId = auth.Result.UserId,
                MobileNumber = long.Parse(model.Username),
                Email = auth.Result.Email,
                FullName = auth.Result.Fullname
            }, _settings.Jwt.TimoutInMinutes);
            return new Response<AuthResponse>
            {
                IsSuccessful = true,
                Result = auth.Result
            };
        }


        [HttpPost, Route("Confirm")]
        public async Task<ActionResult<IResponse<AuthResponse>>> Confirm(ConfirmModel model)
        {
            if (!ModelState.IsValid) return new Response<AuthResponse> { Message = ModelState.GetModelError() };
            var conf = await _userSrv.Confirm(long.Parse(model.MobileNumber), int.Parse(model.Code));
            if (conf.IsSuccessful)
            {
                conf.Result.IsConfirmed = true;
                conf.Result.Token = CreateToken(new User
                {
                    UserId = conf.Result.UserId,
                    MobileNumber = long.Parse(model.MobileNumber),
                    Email = conf.Result.Email,
                    FullName = conf.Result.Fullname
                }, _settings.Jwt.TimoutInMinutes);
            }
            return conf;
        }

        [HttpPost, Route("auth/{mobileNumber:long}")]
        public async Task<ActionResult<IResponse<bool>>> Resend(long mobileNumber)
            => await _userSrv.Resend(mobileNumber);
    }
}