using ConnectUs.Domain.DTO;
using ConnectUs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectUs.Domain.IRepositories
{
    public interface IAccountService
    {
        User GetUserByEmail(string username);
    }
}
