using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ConnectUs.Domain.DTO.AccountDTO;
using ConnectUs.Domain.DTO.JoinedDTO;
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
        public async Task<ActionResult<ResponseModel<HomeIndexResponse>>> Index([FromQuery] int page = 1, string searchQuery = "", SortState sortState = SortState.MeetupDate, bool isDescending = false, int meetupsCount = 18)
        {
            int pageSize = 18;

            var meetups = await _meetup.GetList(searchQuery, sortState, isDescending);
            meetups.Reverse();
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
        [HttpPost("join/{meetupId}")]
        //[Authorize]//authme
        public async Task<ActionResult<MeetupUser>> JoinMeetup(string meetupId)
        {
            string userId = HttpContext.Request.Cookies["X-Username"];
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new ResponseModel<UserDataDTO>("User not loggined"));
            }
            var user = await _userService.GetByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new ResponseModel<MeetupUser>("User not found"));
            }
            var meetup = await _meetup.GetByIdAsync(meetupId);
            if (meetup == null)
                return NotFound(new ResponseModel<MeetupResponseDTO>("Meetup not found"));

            var result = await _meetup.JoinMeetup(meetup, user);
            if (result)
            {
                return Ok(new ResponseModel<MeetupUser>());

            }
            return Ok(new ResponseModel<MeetupUser>("You are already joined"));

        }


        [HttpGet("{meetupId}")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseModel<MeetupUsersDTO>>> Fetch(string meetupId)
        {
            var meetups = await _meetup.GetByIdAsync(meetupId);
            if (meetups == null)
            {
                return NotFound(new ResponseModel<MeetupUsersDTO>("Meetup doesn't exist"));
            }
            var result = _mapper.Map<MeetupUsersDTO>(meetups);
            return new ResponseModel<MeetupUsersDTO>(result);

        }

        //[Authorize]
        [HttpGet("joined")]
        public ActionResult<ResponseModel<List<JoinedListDTO>>> JoinedMeetups()
        {
            string userId = HttpContext.Request.Cookies["X-Username"];
            var meetups = _meetup.GetJoinedMeetups(userId);

            return new ResponseModel<List<JoinedListDTO>>(meetups);
        }
        [HttpDelete("unjoin/{meetupId}")]

        public async Task<ActionResult<ResponseModel<MeetupResponseDTO>>> Unjoin(string meetupId)
        {
            string userId = HttpContext.Request.Cookies["X-Username"];
            var result = await _meetup.UnjoinMeetup(userId, meetupId);
            if (result)
            {
                return Ok(new ResponseModel<MeetupResponseDTO>());

            }
            return new ResponseModel<MeetupResponseDTO>("You was not joined this meetup before");


        }
    }
}
