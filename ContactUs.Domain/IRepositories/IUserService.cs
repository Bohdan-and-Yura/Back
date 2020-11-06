﻿using ConnectUs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConnectUs.Domain.IRepositories
{
    public interface IUserService
    {
        IEnumerable<User> GetAll();
        Task<User> GetByIdAsync(string id);
        Task<bool> Delete(string id);
    }
}
