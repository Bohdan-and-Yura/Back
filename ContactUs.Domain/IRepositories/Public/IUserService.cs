using ConnectUs.Domain.DTO.AccountDTO;
using ConnectUs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConnectUs.Domain.IRepositories
{
    public interface IUserService
    {
        IEnumerable<UserListDTO> GetAll();
        Task<User> GetByIdAsync(string id);
    }
}
