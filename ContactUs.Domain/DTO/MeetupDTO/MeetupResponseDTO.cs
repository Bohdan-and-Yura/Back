﻿using System;

namespace ConnectUs.Domain.DTO.MeetupDTO
{
    public class MeetupResponseDTO
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime MeetupDate { get; set; }
        public string City { get; set; }
        public string MeetupImgPath { get; set; }
        public string UserCreatorId { get; set; }
        public string UserCreatorName { get; set; }
    }
}