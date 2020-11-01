using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConnectUs.Domain.Entities;
using ConnectUs.Domain.Enums;
using ConnectUs.Domain.IRepositories;
using ConnectUs.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConnectUs.Web.Controllers
{
    [Route("api/home")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IMeetupService _meetup;
        private SortState current = SortState.None;

        public HomeController(IMeetupService meetupService)
        {
            _meetup = meetupService;
        }

        [HttpGet]
        public async Task<List<Meetup>> Index([FromQuery] string searchQuery, SortState sortState = SortState.MeetupDate)
        {
            if (sortState != current)
            {
                return await _meetup.GetList(searchQuery, sortState).AsNoTracking().ToListAsync();

            }
            else
            {
                current = sortState;
                return await _meetup.GetList(searchQuery, sortState, true).AsNoTracking().ToListAsync();
            }
        }
    }
}
