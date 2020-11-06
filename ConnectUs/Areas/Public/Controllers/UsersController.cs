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
    /// <summary>
    /// for users control
    /// </summary>
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UsersController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<ResponseModel<IEnumerable<User>>> GetAll()
        {
            var users = _userService.GetAll();
            var result = _mapper.Map<IEnumerable<UserDTO>>(users);
            return Ok(new ResponseModel<IEnumerable<UserDTO>>(result));
        }


        [HttpGet("{id}")]
        public ActionResult<ResponseModel<UserDTO>> GetById(string id)
        {
            // only allow admins to access other user records
            var user = _userService.GetByIdAsync(id);
            if (user == null)
                return NotFound(new ResponseModel<UserDTO>("User not found"));

            var result = _mapper.Map<UserDTO>(user);
            return Ok(new ResponseModel<UserDTO>(result));
        }


        //does administrator can create or update user??

    }
}
