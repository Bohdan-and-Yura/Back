using AutoMapper;
using ConnectUs.Domain.DTO.UserDTO;
using ConnectUs.Domain.Entities;
using ConnectUs.Domain.Helpers;
using ConnectUs.Domain.IRepositories.Public;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConnectUs.Infrastructure.Repositories.Public
{
    public class UserService : IUserService
    {
        private readonly BaseDbContext _context;
        private readonly IMapper _mapper;

        public UserService(BaseDbContext baseDbContext, IMapper mapper)
        {
            _context = baseDbContext;
            _mapper = mapper;
        }



        public IEnumerable<UserListDTO> GetAll()
        {
            var user = _context.Users.Include(c => c.MeetupsJoined).ToList().Where(c => c.MeetupsJoined.Count > 0);
            var result = _mapper.Map<IEnumerable<UserListDTO>>(user);
            return result;
        }

        public async Task<User> GetByIdAsync(string id)
        {
            var user = await _context.Users.Include(c => c.MeetupsJoined).FirstOrDefaultAsync(x => x.Id == id);
            return user.WithoutPassword();
        }


    }
}
