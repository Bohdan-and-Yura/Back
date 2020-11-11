using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ConnectUs.Domain.DTO.UserDTO;
using ConnectUs.Domain.Helpers;
using ConnectUs.Domain.IRepositories.Admin;
using Microsoft.AspNetCore.Mvc;

namespace ConnectUs.Web.Areas.Admin.Controllers
{
    /// <summary>
    ///     only admin
    /// </summary>
    [Route("api/admin/users")]
    [ApiController]
    //[Authorize(Roles = Role.Admin)]
    public class AdminUserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAdminUserService _userService;

        public AdminUserController(IAdminUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        /// <summary>
        ///     delete user by id
        /// </summary>
        /// <param name="id">user id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        //[Authorize(Roles = Role.Admin)]
        public ActionResult<ResponseModel<UserDataDTO>> DeleteById(string id)
        {
            // only allow admins to access other user records
            var result = _userService.Delete(id);
            if (result.Result == false) return BadRequest(new ResponseModel<UserDataDTO>("User not found"));
            return Ok(new ResponseModel<UserDataDTO>());
        }

        /// <summary>
        ///     get all users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //[Authorize(Roles = Role.Admin)]
        public ActionResult<ResponseModel<IEnumerable<UserListDTO>>> GetAll()
        {
            var users = _userService.GetAll();
            var result = _mapper.Map<IEnumerable<UserListDTO>>(users);
            return Ok(new ResponseModel<IEnumerable<UserListDTO>>(result));
        }

        [HttpGet("{userId}")]
        //[Authorize(Roles = Role.Admin)]
        public async Task<ActionResult<ResponseModel<IEnumerable<UserMeetupsDTO>>>> Fetch(string userId)
        {
            var user = await _userService.GetByIdAsync(userId);
            var result = _mapper.Map<UserMeetupsDTO>(user);
            return Ok(new ResponseModel<UserMeetupsDTO>(result));
        }
    }
}