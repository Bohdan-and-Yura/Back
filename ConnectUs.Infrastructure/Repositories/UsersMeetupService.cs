using ConnectUs.Domain.Entities;
using ConnectUs.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConnectUs.Infrastructure.Repositories
{
    public class UsersMeetupService : IUsersMeetupService
    {
        private readonly BaseDbContext _context;

        public UsersMeetupService(BaseDbContext context)
        {
            _context = context;
        }

        public async Task AddToMyMeetupsAsync(MeetupUser mu, User user)
        {
            
        }
    }
}
