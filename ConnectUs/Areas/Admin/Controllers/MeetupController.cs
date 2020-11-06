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
    [Route("api/admin/meetup")]
    [ApiController]
    [Authorize]
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
        public async Task<ActionResult<ResponseModel<MeetupResponseDTO>>> Create([FromBody] CreateMeetupDTO meetupDto)
        {
            if (ModelState.IsValid)
            {

                var meetup = _mapper.Map<Meetup>(meetupDto);
                await _meetup.CreateAsync(meetup);
                var response = _mapper.Map<MeetupResponseDTO>(meetup);
                return Ok(new ResponseModel<MeetupResponseDTO>(response));
            }
            return BadRequest(new ResponseModel<CreateMeetupDTO>("Create failed", meetupDto));

        }
        [HttpDelete("{meetupId}")]
        public async Task<ActionResult<ResponseModel<MeetupResponseDTO>>> Delete(string meetupId)
        {
            await _meetup.Delete(meetupId);
            return Ok(new ResponseModel<MeetupResponseDTO>());
        }

        [HttpGet]
        public ActionResult<ResponseModel<IEnumerable<MeetupResponseDTO>>> GetList(string userId)
        {
            var meetups = _meetup.GetMeetups(userId);
            var result = _mapper.Map<IEnumerable<MeetupResponseDTO>>(meetups);
            return (new ResponseModel<IEnumerable<MeetupResponseDTO>>(result));
        }
        [HttpPut("{meetupId}")]
        public async Task<ActionResult<ResponseModel<MeetupUpdateDTO>>> Update(string meetupId, [FromBody] MeetupUpdateDTO meetupUpdateDTO)
        {
            var meetup = _mapper.Map<Meetup>(meetupUpdateDTO);
            meetup.Id = Guid.Parse(meetupId);
            await _meetup.Update(meetup);
            return (new ResponseModel<MeetupUpdateDTO>(meetupUpdateDTO));
        }

        [HttpGet("{meetupId}")]
        public async Task<ActionResult<Meetup>> Fetch(string meetupId)
        {
            return _meetup.GetById(meetupId);
        }

    }
}
