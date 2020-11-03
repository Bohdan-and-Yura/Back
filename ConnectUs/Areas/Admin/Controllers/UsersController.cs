using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConnectUs.Domain.Core;
using ConnectUs.Domain.Entities;
using ConnectUs.Domain.Helpers;
using ConnectUs.Domain.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConnectUs.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// for users control
    /// </summary>
    [Route("api/admin/users")]
    [ApiController]
    [Authorize(Roles = Role.Admin)]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public ActionResult<ResponseModel<IEnumerable<User>>> GetAll()
        {
            var users = _userService.GetAll();
            return Ok(new ResponseModel<IEnumerable<User>>(users));
        }
         [HttpGet("{id}")]

        public ActionResult<ResponseModel<User>> GetById(string id)
        {
            // only allow admins to access other user records
            var currentUserId = User.Identity.Name;
            if (id != currentUserId && !User.IsInRole(Role.Admin))
                return BadRequest(new ResponseModel<User>("Forbidden"));

            var user = _userService.GetById(id);

            if (user == null)
                return NotFound(new ResponseModel<User>("Forbidden"));

            return Ok(new ResponseModel<User>(user));
        }
    }
}
