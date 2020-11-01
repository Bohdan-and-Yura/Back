using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace ConnectUs.Domain.Entities
{
    public class Meetup
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime MeetupDate { get; set; }
        public User User { get; set; }
    }
}
