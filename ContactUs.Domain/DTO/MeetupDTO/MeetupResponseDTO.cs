using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectUs.Domain.DTO.MeetupDTO
{
    public class MeetupResponseDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime MeetupDate { get; set; }
        public string City { get; set; }
        public string MeetupImgPath { get; set; }
        public string CreatedByUser { get; set; }


    }
}
