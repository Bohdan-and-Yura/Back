using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ConnectUs.Domain.Core;
using ConnectUs.Domain.DTO.AccountDTO;
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
        private readonly IMapper _mapper;

        public AdminUserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
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
        [HttpGet]
        [Authorize(Roles = Role.Admin)]

        public ActionResult<ResponseModel<IEnumerable<User>>> GetAll()
        {
            var users = _userService.GetAll();
            var result = _mapper.Map<IEnumerable<UserDTO>>(users);
            return Ok(new ResponseModel<IEnumerable<UserDTO>>(result));
        }
    }
}
