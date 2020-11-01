using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace ConnectUs.Domain.Entities
{
    public class User : IdentityUser
    {
        public new byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public List<Meetup> Meetups { get; set; }
    }
}
