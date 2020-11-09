using ConnectUs.Domain.Entities;
using ConnectUs.Domain.Helpers;
using ConnectUs.Domain.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectUs.Infrastructure.Repositories
{
    public class UserService : IUserService
    {
        private readonly BaseDbContext _context;

        public UserService(BaseDbContext baseDbContext)
        {
            _context = baseDbContext;
        }

        public async Task<bool> Delete(string id)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == (id));
            if (user == null)
                return false;
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users.ToList();
        }

        public async Task<User> GetByIdAsync(string id)
        {
            var user = await _context.Users.Include(c => c.Meetups).FirstOrDefaultAsync(x => x.Id == id);
            return user.WithoutPassword();
        }


    }
}
