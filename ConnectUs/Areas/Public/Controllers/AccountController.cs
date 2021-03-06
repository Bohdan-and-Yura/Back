﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using ConnectUs.Domain.Core;
using ConnectUs.Domain.DTO.AccountDTO;
using ConnectUs.Domain.DTO.UserDTO;
using ConnectUs.Domain.Entities;
using ConnectUs.Domain.Helpers;
using ConnectUs.Domain.IRepositories.Public;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace ConnectUs.Web.Areas.Public.Controllers
{
    /// <summary>
    ///     for user authenticate
    /// </summary>
    [Route("api/account")]
    [ApiController]
    //[Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _account;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public AccountController(IAccountService accountService, ILogger<AccountController> logger, IMapper mapper)
        {
            _account = accountService;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        ///     get my account
        /// </summary>
        /// <returns></returns>
        [HttpGet("myAccount")]
        //[Authorize]
        public async Task<ActionResult<ResponseModel<UserDataDTO>>> MyAccount()
        {
            var userId = HttpContext.Request.Cookies["X-Username"];
            if (string.IsNullOrEmpty(userId ?? ""))
                return Unauthorized(new ResponseModel<UserDataDTO>("Man, you are not logined"));

            var user = await _account.GetUserById(userId);
            var result = _mapper.Map<UserDataDTO>(user);
            if (result == null) return Ok(new ResponseModel<UserDataDTO>("User not found. Wrong id"));
            return Ok(new ResponseModel<UserDataDTO>(result));
        }

        /// <summary>
        ///     login
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<ResponseModel<LoginResponseDTO>>> Login([FromBody] LoginRequestDTO model)
        {
            var user = _account.Authenticate(model.Email, model.Password);

            if (user == null)
                return BadRequest(new ResponseModel<LoginRequestDTO>("Username or password is incorrect", model));
            var responseData = await TokenAuthenticate(user);
            // returns basic user info and authentication token
            return Ok(new ResponseModel<LoginResponseDTO>(responseData));
        }

        private async Task<LoginResponseDTO> TokenAuthenticate(User user)
        {
            user.RefreshToken = Guid.NewGuid().ToString();
            var jwt = new JwtSecurityToken(
                AuthOptions.ISSUER,
                AuthOptions.AUDIENCE,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.Add(TimeSpan.FromDays(AuthOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(),
                    SecurityAlgorithms.HmacSha256));
            var tokenString = new JwtSecurityTokenHandler().WriteToken(jwt);


            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Id),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("access_token", tokenString)
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            Response.Cookies.Append("X-Access-Token", tokenString,
                new CookieOptions {HttpOnly = true, SameSite = SameSiteMode.None});
            Response.Cookies.Append("X-Username", user.Id,
                new CookieOptions {HttpOnly = true, SameSite = SameSiteMode.None});
            Response.Cookies.Append("X-Refresh-Token", user.RefreshToken,
                new CookieOptions {HttpOnly = true, SameSite = SameSiteMode.None});
            return new LoginResponseDTO
            {
                Id = user.Id,
                Username = user.UserName,
                Token = tokenString
            };
        }


        /// <summary>
        ///     register
        /// </summary>
        /// <param name="registerDTO"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<ResponseModel<RegisterDTO>>> Register([FromBody] RegisterDTO registerDTO)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(registerDTO.Password))
                    return BadRequest(new ResponseModel<RegisterDTO>("Password cannot be empty", registerDTO));


                var user = _mapper.Map<User>(registerDTO);
                user.Role = Role.User;
                user.BirthDay = Convert.ToDateTime(user.BirthDay.ToShortDateString());
                var result = await _account.CreateAsync(user);
                if (result != null)
                    return Ok(new ResponseModel<RegisterDTO>());
                return BadRequest(new ResponseModel<RegisterDTO>("Email is taken", registerDTO));
            }

            return BadRequest(new ResponseModel<RegisterDTO>("Invalid data", registerDTO));
        }

        /// <summary>
        ///     logout
        /// </summary>
        /// <returns></returns>
        [HttpGet("logout")]
        [ValidateAntiForgeryToken]
        //[Authorize]
        public async Task<ActionResult<ResponseModel<VoidClass>>> Logout()
        {
            // also delete authenticated  cookie  
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Accepted(new ResponseModel<VoidClass>());
        }

        /// <summary>
        ///     update data account
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        //[Authorize]
        public async Task<ActionResult<ResponseModel<EditUserDTO>>> EditAccount([FromBody] EditUserDTO model)
        {
            if (ModelState.IsValid)
            {
                var userId = HttpContext.Request.Cookies["X-Username"];
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(new ResponseModel<UserDataDTO>("User not loggined"));
                var result = await _account.UpdateAsync(userId, model);
                if (result != null) return Ok(new ResponseModel<EditUserDTO>(model));
            }

            return BadRequest(new ResponseModel<EditUserDTO>("Data is not valid", model));
        }

        /// <summary>
        ///     delete account
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        //[Authorize]
        public async Task<ActionResult<ResponseModel<VoidClass>>> DeleteAccount()
        {
            var userId = HttpContext.Request.Cookies["X-Username"];
            if (string.IsNullOrEmpty(userId)) return Unauthorized(new ResponseModel<VoidClass>("User not loggined"));
            await _account.DeleteAsync(userId);
            return Accepted(new ResponseModel<VoidClass>());
        }
    }
}