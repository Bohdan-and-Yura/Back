using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ConnectUs.Domain.Core;
using ConnectUs.Domain.DTO;
using ConnectUs.Domain.DTO.AccountDTO;
using ConnectUs.Domain.Entities;
using ConnectUs.Domain.Helpers;
using ConnectUs.Domain.IRepositories;
using ConnectUs.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ConnectUs.Web.Areas.Public.Controllers
{
    /// <summary>
    /// for user authenticate
    /// </summary>
    [Route("api/account")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService account;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        public AccountController(IAccountService accountService, SignInManager<User> signInManager, ILogger<AccountController> logger, IMapper mapper)
        {
            account = accountService;
            _signInManager = signInManager;
            _logger = logger;
            _mapper = mapper;
        }


        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<RegisterDTO>> Register([FromBody] RegisterDTO registerDTO)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(registerDTO.Password))
                    return BadRequest(new ResponseModel<RegisterDTO>("Passord cannot be empty", registerDTO));


                var user = _mapper.Map<User>(registerDTO);
                user.Role = Role.User;
                var result = await account.CreateAsync(user);
                if (result != null)
                {
                    // cookie
                    await _signInManager.SignInAsync(user, false);

                    return Ok(new ResponseModel<RegisterDTO>());

                }
                else
                {
                    return BadRequest(new ResponseModel<RegisterDTO>("Email is taken", registerDTO));
                }
            }

            return BadRequest(new ResponseModel<RegisterDTO>("Invalid data", registerDTO));

        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDTO>> Login([FromBody] LoginRequestDTO model)
        {
            var user = account.Authenticate(model.Email, model.Password);

            if (user == null)
                return BadRequest(new ResponseModel<LoginRequestDTO>("Username or password is incorrect", model));


            var responseData = await TokenAuthenticate(user);
            // returns basic user info and authentication token
            return Ok(new ResponseModel<LoginResponseDTO>(responseData));
        }
        private async Task<LoginResponseDTO> TokenAuthenticate(User user)
        {
            var jwt = new JwtSecurityToken(
                     issuer: AuthOptions.ISSUER,
                     audience: AuthOptions.AUDIENCE,
                     notBefore: DateTime.UtcNow,
                     expires: DateTime.UtcNow.Add(TimeSpan.FromDays(AuthOptions.LIFETIME)),
                     signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var tokenString = new JwtSecurityTokenHandler().WriteToken(jwt);


            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("access_token", tokenString)
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);


            await HttpContext.SignInAsync(
               CookieAuthenticationDefaults.AuthenticationScheme,
               new ClaimsPrincipal(claimsIdentity));

            return new LoginResponseDTO
            {
                Id = user.Id,
                Username = user.UserName,
                Token = tokenString
            };
        }

        [HttpPost("logout")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            // also delete authenticated  cookie  
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Accepted((new ResponseModel<LoginResponseDTO>()));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<EditUserDTO>> EditAccount( [FromBody] EditUserDTO model)
        {
            if (ModelState.IsValid)
            {
                var userId=HttpContext.User.Claims.FirstOrDefault();
                var result = await account.UpdateAsync(userId.Value, model);
                if (result != null)
                {
                    return Ok(new ResponseModel<EditUserDTO>(model));
                }
                else
                {
                    return BadRequest(new ResponseModel<EditUserDTO>("Updating error", model));
                }

            }
            return BadRequest(new ResponseModel<EditUserDTO>("Data is not valid", model));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAccount()
        {
            var userId = HttpContext.User.Claims.FirstOrDefault();

            await account.DeleteAsync(userId.Value);
            return Accepted(new ResponseModel<EditUserDTO>());
        }

    }
}
