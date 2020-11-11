using System;

namespace ConnectUs.Domain.DTO.UserDTO
{
    public class UserDataDTO
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string UserName { get; set; }
        public string UserImgPath { get; set; }
        public DateTime BirthDay { get; set; }

    }
}
