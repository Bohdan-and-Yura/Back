﻿using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ConnectUs.Domain.DTO.UserDTO;
using ConnectUs.Domain.Helpers;
using ConnectUs.Domain.IRepositories.Public;
using Microsoft.AspNetCore.Mvc;

namespace ConnectUs.Web.Areas.Public.Controllers
{
    /// <summary>
    ///     for users control
    /// </summary>
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public UsersController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet("creators")]
        public ActionResult<ResponseModel<IEnumerable<UserListDTO>>> GetAll()
        {
            var users = _userService.GetAll();
            return Ok(new ResponseModel<IEnumerable<UserListDTO>>(users));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseModel<UserMeetupsDTO>>> Fetch(string id)
        {
            // only allow admins to access other user records
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
                return NotFound(new ResponseModel<UserMeetupsDTO>("User not found"));

            var result = _mapper.Map<UserMeetupsDTO>(user);
            return Ok(new ResponseModel<UserMeetupsDTO>(result));
        }
    }
}