using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectUs.Domain.DTO.MeetupDTO
{
    public class MeetupUpdateDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime MeetupDate { get; set; }
        public string City { get; set; }
    }
}
