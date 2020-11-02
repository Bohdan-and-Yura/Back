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
        public string Password { get; set; }
        [NotMapped]
        public string ConfirmPassword { get; set; }


        public byte[] PasswordHashByte { get; set; }
        public byte[] PasswordSaltByte { get; set; }

        public List<Meetup> Meetups { get; set; }
    }
}
