using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConnectUs.Domain.DTO.PageResponseDTO;
using ConnectUs.Domain.Entities;
using ConnectUs.Domain.Enums;
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

        public HomeController(IMeetupService meetupService)
        {
            _meetup = meetupService;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<HomeIndexResponse>> Index([FromQuery] int page = 1, string searchQuery = "", SortState sortState = SortState.MeetupDate, bool isDescending = false)
        {
            int pageSize = 18;

            List<Meetup> meetups = await _meetup.GetList(searchQuery, sortState, isDescending).AsNoTracking().ToListAsync();

            PageViewModel pageViewModel = new PageViewModel(meetups.Count(), page, pageSize);
            var items = meetups.Skip(page - 1).Take(pageSize);

            return new HomeIndexResponse
            {
                Meetups = items,
                PageView = pageViewModel
            };
        }
    }
}
