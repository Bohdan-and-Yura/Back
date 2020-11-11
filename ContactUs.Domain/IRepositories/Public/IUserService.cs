using ConnectUs.Domain.DTO.UserDTO;
using ConnectUs.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConnectUs.Domain.IRepositories.Public
{
    public interface IUserService
    {
        IEnumerable<UserListDTO> GetAll();
        Task<User> GetByIdAsync(string id);
    }
}
