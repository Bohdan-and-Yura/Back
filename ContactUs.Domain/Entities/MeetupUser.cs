using System;

namespace ConnectUs.Domain.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class MeetupUser
    {
        public Guid MeetupId { get; set; }
        public string UserId { get; set; }
        public Meetup Meetup { get; set; }
        public User User { get; set; }


    }
}
