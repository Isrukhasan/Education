﻿using Education.Core.Api.Services.Security.Auth;
using Education.Core.BLL.Interfaces;
using Education.Core.Common.Dtos.Requests.Security;
using Education.Core.Common.Dtos.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SpAuth.Api.Entity;
using SpAuth.Api.Services.UserService;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
namespace SpAuth.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        public static User user = new User();
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly ISecurityService _securityService;
        private readonly ITokenService _tokenService;


        public SecurityController(IConfiguration configuration,
            IUserService userService, ISecurityService securityService, ITokenService tokenService)
        {
            _configuration = configuration;
            _userService = userService;
            _securityService = securityService;
            _tokenService = tokenService;
        }

        [HttpGet, Authorize]
        public ActionResult<string> GetMe()
        {
            var userName = _userService.GetMyName();
            return Ok(userName);
        }

        //[HttpPost("register")]
        //public async Task<ActionResult<User>> Register(UserDto request)
        //{
        //    CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
        //    user.Username = request.Username;
        //    user.PasswordHash = passwordHash;
        //    user.PasswordSalt = passwordSalt;
        //    return Ok(user);
        //}


        // POST api/security/web/login
        [HttpPost("web/login")]
        [AllowAnonymous]
        public async Task<ActionResult<WebLoginResponseDto>> LoginWebAsync([FromBody] WebUserLoginRequestDto request)
        {
            if (string.IsNullOrEmpty(request.PassWord) || string.IsNullOrEmpty(request.UserName))
                return Unauthorized("Please fill required fields");

            var userAuthData = await _securityService.GetWebUserAuthDataByCredentialsAsync(request);

            if (!userAuthData.IsAuthenticated)
                return Unauthorized("Invalid credentials");

            var accessToken = _tokenService.GetWebAccessToken(userAuthData);

            var response = new WebLoginResponseDto()
            {
                AccessToken = accessToken,
                UserId = userAuthData.UserId
            };
            return Ok();
        }




        //[HttpPost("login")]
        //public async Task<ActionResult<string>> Login(UserDto request)
        //{
        //    if (user.Username != request.Username)
        //    {
        //        return BadRequest("User not found.");
        //    }

        //    if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
        //    {
        //        return BadRequest("Wrong password.");
        //    }
        //    string token = CreateToken(user);
        //    var refreshToken = GenerateRefreshToken();
        //    SetRefreshToken(refreshToken);
        //    return Ok(token);
        //}

        [HttpPost("refresh-token")]
        public async Task<ActionResult<string>> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (!user.RefreshToken.Equals(refreshToken))
            {
                return Unauthorized("Invalid Refresh Token.");
            }
            else if (user.TokenExpires < DateTime.Now)
            {
                return Unauthorized("Token expired.");
            }

            string token = CreateToken(user);
            var newRefreshToken = GenerateRefreshToken();
            SetRefreshToken(newRefreshToken);

            return Ok(token);
        }

        private RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(7),
                Created = DateTime.Now
            };

            return refreshToken;
        }

        private void SetRefreshToken(RefreshToken newRefreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires
            };
            Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);

            user.RefreshToken = newRefreshToken.Token;
            user.TokenCreated = newRefreshToken.Created;
            user.TokenExpires = newRefreshToken.Expires;
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}
