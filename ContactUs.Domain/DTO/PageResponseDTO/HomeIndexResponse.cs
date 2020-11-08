using ConnectUs.Domain.DTO.MeetupDTO;
using ConnectUs.Domain.Entities;
using ConnectUs.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectUs.Domain.DTO.PageResponseDTO
{
    public class HomeIndexResponse
    {
        public IEnumerable<MeetupResponseDTO> Meetups { get; set; }
        public PageViewModel PageView { get; set; }
    }
}
