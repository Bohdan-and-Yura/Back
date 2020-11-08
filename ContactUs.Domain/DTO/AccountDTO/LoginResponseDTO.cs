using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace ConnectUs.Domain.DTO.AccountDTO
{
    public class LoginResponseDTO
    {
        public string  Id { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }

               
    }
}
