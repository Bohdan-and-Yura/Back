﻿using System;
using System.Collections.Generic;
using ConnectUs.Domain.DTO.MeetupDTO;

namespace ConnectUs.Domain.DTO.UserDTO
{
    public class UserMeetupsDTO
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string UserName { get; set; }
        public string UserImgPath { get; set; }
        public DateTime BirthDay { get; set; }
        public List<MeetupResponseDTO> Meetups { get; set; }
    }
}