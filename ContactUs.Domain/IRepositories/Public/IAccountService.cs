using System.Threading.Tasks;
using ConnectUs.Domain.DTO.AccountDTO;
using ConnectUs.Domain.Entities;

namespace ConnectUs.Domain.IRepositories.Public
{
    public interface IAccountService
    {
        User Authenticate(string email, string password);
        Task<User> CreateAsync(User user);
        Task<EditUserDTO> UpdateAsync(string id, EditUserDTO user);
        Task DeleteAsync(string id);
        Task<User> GetUserById(string id);
    }
}