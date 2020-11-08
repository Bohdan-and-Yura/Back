using ConnectUs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectUs.Domain.DTO.AccountDTO
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string UserName { get; set; }
        public string UserImgPath { get; set; }
        public DateTime BirthDay { get; set; }

    }
}
