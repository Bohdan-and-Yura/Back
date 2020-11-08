using System;
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
    [Route("api/admin/meetups")]
    [ApiController]
    [Authorize(Roles = Role.Admin)]
    [Authorize(Roles = Role.User)]
    public class MeetupController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMeetupAdminService _meetup;

        public MeetupController(IMapper mapper, IMeetupAdminService meetup)
        {
            _mapper = mapper;
            _meetup = meetup;
        }
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ResponseModel<MeetupResponseDTO>>> Create([FromBody] CreateMeetupDTO meetupDto)
        {
            if (ModelState.IsValid)
            {

                var meetup = _mapper.Map<Meetup>(meetupDto);
                meetup.CreatedByUser = HttpContext.User.Claims.FirstOrDefault().Value;
                await _meetup.CreateAsync(meetup);
                var response = _mapper.Map<MeetupResponseDTO>(meetup);
                return Ok(new ResponseModel<MeetupResponseDTO>(response));
            }
            return BadRequest(new ResponseModel<CreateMeetupDTO>("Create failed", meetupDto));

        }
        [Authorize]
        [HttpDelete("{meetupId}")]
        public async Task<ActionResult<ResponseModel<MeetupResponseDTO>>> Delete(string meetupId)
        {
            var user = HttpContext.User.Claims.ToList();

            var result = await _meetup.Delete(meetupId, user);
            if (result)
            {
                return Ok(new ResponseModel<MeetupResponseDTO>());
            }
            return (new ResponseModel<MeetupResponseDTO>("Forbidden"));
            //return Forbid();
        }

        [Authorize]
        [HttpGet]
        public ActionResult<ResponseModel<IEnumerable<MeetupResponseDTO>>> GetList()
        {
            var user = HttpContext.User.Claims.ToList();

            var meetups = _meetup.GetMeetups(user);
            var result = _mapper.Map<IEnumerable<MeetupResponseDTO>>(meetups);
            return (new ResponseModel<IEnumerable<MeetupResponseDTO>>(result));
        }
        [Authorize]
        [HttpPut("{meetupId}")]
        public async Task<ActionResult<ResponseModel<MeetupUpdateDTO>>> Update(string meetupId, [FromBody] MeetupUpdateDTO meetupUpdateDTO)
        {
            var meetup = _mapper.Map<Meetup>(meetupUpdateDTO);
            meetup.Id = Guid.Parse(meetupId);
            var user = HttpContext.User.Claims.ToList();

            var result = await _meetup.Update(meetup, user);
            if (result)
            {
                return Ok(new ResponseModel<MeetupUpdateDTO>(meetupUpdateDTO));
            }
            return Forbid();

        }

        [Authorize]
        [HttpGet("{meetupId}")]
        public async Task<ActionResult<MeetupResponseDTO>> Fetch(string meetupId)
        {
            var user = HttpContext.User.Claims.ToList();

            var meetup= await _meetup.GetById(meetupId, user);
            var result = _mapper.Map<MeetupResponseDTO>(meetup);
            return Ok(new ResponseModel<MeetupResponseDTO>(result));


        }

    }
}
