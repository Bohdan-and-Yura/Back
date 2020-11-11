using ConnectUs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConnectUs.Domain.IRepositories
{
    public interface IAdminUserService
    {
        Task<bool> Delete(string id);
        IEnumerable<User> GetAll();
        Task<User> GetByIdAsync(string id);
    }
}
