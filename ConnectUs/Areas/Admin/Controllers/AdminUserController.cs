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
    [Route("api/admin/users")]
    [ApiController]
    [Authorize(Roles = Role.Admin)]

    public class AdminUserController : ControllerBase
    {
        private readonly IUserService _userService;

        public AdminUserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpDelete("{id}")]
        public ActionResult<ResponseModel<User>> DeleteById(string id)
        {

            // only allow admins to access other user records
            var result = _userService.Delete(id);
            if (result.Result == false)
            {
                return BadRequest(new ResponseModel<User>("user not found"));
            }
            return Ok(new ResponseModel<User>());
        }
    }
}
