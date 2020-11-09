using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ConnectUs.Domain.DTO.AccountDTO;
using ConnectUs.Domain.DTO.MeetupDTO;
using ConnectUs.Domain.DTO.PageResponseDTO;
using ConnectUs.Domain.Entities;
using ConnectUs.Domain.Enums;
using ConnectUs.Domain.Helpers;
using ConnectUs.Domain.IRepositories;
using ConnectUs.Domain.ViewModels;
using ConnectUs.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConnectUs.Web.Areas.Public.Controllers
{
    /// <summary>
    /// public
    /// </summary>
    [Route("api/home")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IMeetupService _meetup;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public HomeController(IMeetupService meetupService, IUserService userService, IMapper mapper)
        {
            _meetup = meetupService;
            _userService = userService;
            _mapper = mapper;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseModel<HomeIndexResponse>>> Index([FromQuery] int page = 1, string searchQuery = "", SortState sortState = SortState.MeetupDate, bool isDescending = false, int meetupsCount=18)
        {
            int pageSize = 18;

            var meetups = await _meetup.GetList(searchQuery, sortState, isDescending);
            
            PageViewModel pageViewModel = new PageViewModel(meetups.Count(), page, pageSize);
            var items = meetups.Skip(page - 1).Take(meetupsCount);

            var result = new HomeIndexResponse
            {
                Meetups = items,
                PageView = pageViewModel
            };
            return new ResponseModel<HomeIndexResponse>(result);

        }

        /// <summary>
        /// onClick btnJoin
        /// </summary>
        /// <param name="attenderId">from cookie</param>
        /// <param name="meetupId">from query</param>
        /// <returns></returns>
        [HttpPost("join")]
        [Authorize]//authme
        public async Task<IActionResult> JoinMeetup([FromQuery] string meetupId)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault();
            if (string.IsNullOrEmpty(userId?.Value))
            {
                return Unauthorized(new ResponseModel<UserDataDTO>("User not loggined"));
            }
            var user = await _userService.GetByIdAsync(userId.Value);
            if (user == null)
            {
                return NotFound(new ResponseModel<User>("User not found"));
            }
            var meetup = await _meetup.GetByIdAsync(meetupId);
            if (meetup == null)
                return NotFound(new ResponseModel<Meetup>("Meetup not found"));

            await _meetup.AddUserToMeetup_AddMeetupToUserAsync(meetup, user);

            return Ok(new ResponseModel<MeetupUser>());
        }
    }
}
