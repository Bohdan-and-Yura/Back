using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace ConnectUs.Domain.Entities
{
    public class User : IdentityUser
    {
        public string Role { get; set; }
        public string Description { get; set; }
        public string Token { get; set; }
        public string UserImgPath { get; set; }
        public DateTime BirthDay { get; set; }

        public string Password { get; set; }

        [NotMapped] public string ConfirmPassword { get; set; }

        public byte[] PasswordHashByte { get; set; }
        public byte[] PasswordSaltByte { get; set; }

        public List<MeetupUser> MeetupsJoined { get; set; }
        public List<Meetup> MeetupsCreated { get; set; }
        public string RefreshToken { get; set; }
    }
}