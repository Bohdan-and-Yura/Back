using ConnectUs.Domain.Entities;
using ConnectUs.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectUs.Domain.DTO.PageResponseDTO
{
    public class HomeIndexResponse
    {
        public IEnumerable<Meetup> Meetups { get; set; }
        public PageViewModel PageView { get; set; }
    }
}
