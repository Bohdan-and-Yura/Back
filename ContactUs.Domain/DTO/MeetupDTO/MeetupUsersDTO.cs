﻿using System;
using System.Collections.Generic;
using ConnectUs.Domain.DTO.UserDTO;

namespace ConnectUs.Domain.DTO.MeetupDTO
{
    public class MeetupUsersDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime MeetupDate { get; set; }
        public string City { get; set; }
        public string MeetupImgPath { get; set; }
        public string UserCreatorId { get; set; }
        public string UserCreatorName { get; set; }
        public ICollection<UserDataDTO> UsersJoined { get; set; }
    }
}