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
        public List<ConnectUs.Domain.Entities.Meetup> Meetups { get; set; }
    }
}
