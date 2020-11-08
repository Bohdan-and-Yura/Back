using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ConnectUs.Domain.DTO.AccountDTO
{
    public class EditUserDTO
    {
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        public string UserImgPath { get; set; }
        public DateTime BirthDay { get; set; }
        public string Description { get; set; }

        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
