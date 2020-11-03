using ConnectUs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectUs.Domain.IRepositories
{
    public interface IUserService
    {
        IEnumerable<User> GetAll();
        User GetById(string id);
    }
}
