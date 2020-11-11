using System.Collections.Generic;
using System.Threading.Tasks;
using ConnectUs.Domain.Entities;

namespace ConnectUs.Domain.IRepositories.Admin
{
    public interface IAdminUserService
    {
        Task<bool> Delete(string id);
        IEnumerable<User> GetAll();
        Task<User> GetByIdAsync(string id);
    }
}