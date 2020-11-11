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
