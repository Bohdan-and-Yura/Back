using System;
using System.Collections.Generic;

namespace ConnectUs.Domain.Entities
{
    public class Meetup
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string MeetupImgPath { get; set; }
        public DateTime MeetupDate { get; set; }
        public string City { get; set; }
        public string CreatedByUser { get; set; }
        public ICollection<MeetupUser> UsersJoined { get; set; }
        public User UserCreator { get; set; }
    }
}