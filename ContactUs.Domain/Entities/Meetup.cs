using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace ConnectUs.Domain.Entities
{
    public class Meetup
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Descriptions { get; set; }

        public User User { get; set; }
    }
}
