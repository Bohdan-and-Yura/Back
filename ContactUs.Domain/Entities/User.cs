using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;
using System.Text;

namespace ConnectUs.Domain.Entities
{
    public class User : IdentityUser
    {
        public string Role { get; set; }
        public string Description { get; set; }
        public string Token { get; set; }
        public DateTime BirthDay { get; set; }

        public string Password { get; set; }
        [NotMapped]
        public string ConfirmPassword { get; set; }

        public byte[] PasswordHashByte { get; set; }
        public byte[] PasswordSaltByte { get; set; }

        public ICollection<MeetupUser> Meetups { get; set; }
    }
}
