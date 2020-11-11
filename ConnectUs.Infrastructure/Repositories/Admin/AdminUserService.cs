using ConnectUs.Domain.Entities;
using ConnectUs.Domain.Helpers;
using ConnectUs.Domain.IRepositories.Admin;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConnectUs.Infrastructure.Repositories.Admin
{
    public class AdminUserService : IAdminUserService
    {
        private readonly BaseDbContext _context;

        public AdminUserService(BaseDbContext context)
        {
            _context = context;
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
            return _context.Users.Include(c => c.MeetupsJoined).ToList().Where(c => c.MeetupsJoined.Count > 0);
        }

        public async Task<User> GetByIdAsync(string id)
        {
            var user = await _context.Users.Include(c => c.MeetupsJoined).FirstOrDefaultAsync(x => x.Id == id);
            return user.WithoutPassword();
        }
    }
}
