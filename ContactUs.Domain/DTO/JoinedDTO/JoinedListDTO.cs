using ConnectUs.Domain.DTO.MeetupDTO;
using ConnectUs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectUs.Domain.DTO.JoinedDTO
{
    public class JoinedListDTO
    {
        //public Guid MeetupId { get; set; }
        //public string UserId { get; set; }
        public MeetupResponseDTO Meetup { get; set; }
        //public User User { get; set; }
    }
}
