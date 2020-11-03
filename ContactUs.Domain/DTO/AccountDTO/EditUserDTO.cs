using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectUs.Domain.DTO.AccountDTO
{
    public class EditUserDTO
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime BirthDay { get; set; }
    }
}
