﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConnectUs.Domain.DTO.PageResponseDTO;
using ConnectUs.Domain.Entities;
using ConnectUs.Domain.Enums;
using ConnectUs.Domain.IRepositories;
using ConnectUs.Domain.ViewModels;
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
        public async Task<ActionResult<HomeIndexResponse>> Index([FromQuery] int page = 1, string searchQuery = "", SortState sortState = SortState.MeetupDate)
        {
            int pageSize = 18;
            List<Meetup> meetups;
            if (sortState != current)
            {
                meetups = await _meetup.GetList(searchQuery, sortState).AsNoTracking().ToListAsync();
            }
            else
            {
                current = sortState;
                meetups = await _meetup.GetList(searchQuery, sortState, true).AsNoTracking().ToListAsync();
            }
            PageViewModel pageViewModel = new PageViewModel(meetups.Count(), page, pageSize);
            var items = meetups.Skip(page - 1).Take(pageSize);

            HomeIndexResponse response = new HomeIndexResponse
            {
                Meetups = items,
                PageView = pageViewModel
            };
            return response;
        }
    }
}
