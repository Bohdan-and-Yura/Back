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
                var user = _mapper.Map<User>(registerDTO);
                user.Role = Role.User;
                var result = await account.CreateAsync(user);
                if (result != null)
                {
                    // установка куки
                    await _signInManager.SignInAsync(user, false);

                    return Ok(0);
                }
            }
           
            return BadRequest(1);

        }

        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult<LoginResponseDTO> Login([FromBody] LoginRequestDTO model)
        {
            var user = account.Authenticate(model.Email, model.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });


            var Subject = new ClaimsIdentity(new Claim[]
               {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
               });
            var jwt = new JwtSecurityToken(
                     issuer: AuthOptions.ISSUER,
                     audience: AuthOptions.AUDIENCE,
                     notBefore: DateTime.UtcNow,
                     claims: Subject.Claims,
                     expires: DateTime.UtcNow.Add(TimeSpan.FromDays(AuthOptions.LIFETIME)),
                     signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var tokenString = new JwtSecurityTokenHandler().WriteToken(jwt);


            // return basic user info and authentication token
            return Ok(new LoginResponseDTO
            {
                Id = user.Id,
                Username = user.UserName,
                Token = tokenString
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // удаляем аутентификационные куки
            await _signInManager.SignOutAsync();
            return RedirectToLocal("Index");
        }

        [HttpPost]
        public async Task<ActionResult<EditUserDTO>> Edit(EditUserDTO model)
        {
            if (ModelState.IsValid)
            {

                var result = await account.UpdateAsync(model);
                if (result != null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    throw new AppException("Updating error");
                }

            }
            return Ok(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            await account.DeleteAsync(id);
            return RedirectToLocal("Index");
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        #endregion
    }
}
