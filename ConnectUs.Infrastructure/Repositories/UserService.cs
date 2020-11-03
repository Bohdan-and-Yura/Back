using ConnectUs.Domain.Entities;
using ConnectUs.Domain.Helpers;
using ConnectUs.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConnectUs.Infrastructure.Repositories
{
    public class UserService : IUserService
    {
        private readonly BaseDbContext _context;

        public UserService(BaseDbContext baseDbContext)
        {
            _context = baseDbContext;
        }

        

        public IEnumerable<User> GetAll()
        {
            throw new NotImplementedException();
        }

        public User GetById(string id)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == id);
            return user.WithoutPassword();
        }
    }
}
