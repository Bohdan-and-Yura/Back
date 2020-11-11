using System.Collections.Generic;
using ConnectUs.Domain.DTO.MeetupDTO;
using ConnectUs.Domain.ViewModels;

namespace ConnectUs.Domain.DTO.PageResponseDTO
{
    public class HomeIndexResponse
    {
        public IEnumerable<MeetupResponseDTO> Meetups { get; set; }
        public PageViewModel PageView { get; set; }
    }
}