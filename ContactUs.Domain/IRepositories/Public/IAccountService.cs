using ConnectUs.Domain.DTO;
using ConnectUs.Domain.DTO.AccountDTO;
using ConnectUs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConnectUs.Domain.IRepositories
{
    public interface IAccountService
    {
        User GetUserByEmail(string username);
        User Authenticate(string email, string password);
        Task<User> CreateAsync(User registerDTO);
        Task<EditUserDTO> UpdateAsync(string id, EditUserDTO user);
        Task DeleteAsync(string id);
        Task<User> GetUserById(string? id);
    }
}
