using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace ConnectUs.Domain.Entities
{
    public class User : IdentityUser
    {
        public new string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }


        public new byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public List<Meetup> Meetups { get; set; }
    }
}
