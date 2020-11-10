using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ConnectUs.Domain.DTO.MeetupDTO
{
    public class CreateMeetupDTO
    {
        [Required]
        [StringLength(20, MinimumLength = 6)]
        public string Title { get; set; }

        [Required]
        [StringLength(300, MinimumLength = 20)]

        public string Description { get; set; }

        [Required]
        public DateTime MeetupDate { get; set; }
        [Required]
        public string City { get; set; }

        public string MeetupImgPath { get; set; }
    }
}
