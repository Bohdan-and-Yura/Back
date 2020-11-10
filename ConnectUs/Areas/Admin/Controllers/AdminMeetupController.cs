﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ConnectUs.Domain.Core;
using ConnectUs.Domain.DTO.MeetupDTO;
using ConnectUs.Domain.Entities;
using ConnectUs.Domain.Helpers;
using ConnectUs.Domain.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConnectUs.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// for authorized user to meetups managment created by current authorized user
    /// </summary>
    [Route("api/admin/meetups")]
    [ApiController]
    //[Authorize]
    public class AdminMeetupController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMeetupAdminService _meetup;

        public AdminMeetupController(IMapper mapper, IMeetupAdminService meetup)
        {
            _mapper = mapper;
            _meetup = meetup;
        }
        /// <summary>
        /// Create meetup
        /// </summary>
        /// <param name="meetupDto"></param>
        /// <returns></returns>
        [HttpPost]
        //[Authorize]
        public async Task<ActionResult<ResponseModel<MeetupResponseDTO>>> Create([FromBody] CreateMeetupDTO meetupDto)
        {
            if (ModelState.IsValid)
            {

                string userId = HttpContext.Request.Cookies["X-Username"];
                var meetup = _mapper.Map<Meetup>(meetupDto);

                meetup.CreatedByUser = userId;
                await _meetup.CreateAsync(meetup);
                var response = _mapper.Map<MeetupResponseDTO>(meetup);
                return Ok(new ResponseModel<MeetupResponseDTO>(response));
            }
            return BadRequest(new ResponseModel<CreateMeetupDTO>("Create failed", meetupDto));

        }
        /// <summary>
        /// delete meetup
        /// </summary>
        /// <param name="meetupId"></param>
        /// <returns></returns>
        //[Authorize]
        [HttpDelete("{meetupId}")]
        public async Task<ActionResult<ResponseModel<MeetupResponseDTO>>> Delete(string meetupId)
        {
            string userId = HttpContext.Request.Cookies["X-Username"];

            var result = await _meetup.Delete(meetupId, userId);
            if (result)
            {
                return Ok(new ResponseModel<MeetupResponseDTO>());
            }
            return (new ResponseModel<MeetupResponseDTO>("Forbidden"));
            //return Forbid();
        }
        /// <summary>
        /// meetups created by user 
        /// </summary>
        /// <returns></returns>
        //[Authorize]
        [HttpGet]
        public ActionResult<ResponseModel<IEnumerable<MeetupResponseDTO>>> MyMeetups()
        {
            string userId = HttpContext.Request.Cookies["X-Username"];
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new ResponseModel<MeetupResponseDTO>("Smth went wrong"));
            }
            var meetups = _meetup.GetMeetups(userId);
            var result = _mapper.Map<IEnumerable<MeetupResponseDTO>>(meetups);
            return (new ResponseModel<IEnumerable<MeetupResponseDTO>>(result));
        }
        /// <summary>
        /// update meetup
        /// </summary>
        /// <param name="meetupId"></param>
        /// <param name="meetupUpdateDTO"></param>
        /// <returns></returns>
        //[Authorize]
        [HttpPut("{meetupId}")]
        public async Task<ActionResult<ResponseModel<MeetupUpdateDTO>>> Update(string meetupId, [FromBody] MeetupUpdateDTO meetupUpdateDTO)
        {
            var meetup = _mapper.Map<Meetup>(meetupUpdateDTO);
            meetup.Id = Guid.Parse(meetupId);
            string userId = HttpContext.Request.Cookies["X-Username"];

            var result = await _meetup.Update(meetup, userId);
            if (result)
            {
                return Ok(new ResponseModel<MeetupUpdateDTO>(meetupUpdateDTO));
            }
            return Forbid();

        }
        /// <summary>
        /// get by id beetup
        /// </summary>
        /// <param name="meetupId"></param>
        /// <returns></returns>
        ////[Authorize]
        [HttpGet("{meetupId}")]
        public async Task<ActionResult<ResponseModel<MeetupResponseDTO>>> Fetch(string meetupId)
        {

            var meetup= await _meetup.GetById(meetupId);
            var result = _mapper.Map<MeetupResponseDTO>(meetup);
            return Ok(new ResponseModel<MeetupResponseDTO>(result));


        }
       

    }
}
