using ConnectUs.Domain.Entities;
using ConnectUs.Domain.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ConnectUs.Infrastructure.Repositories
{
    public class MeetupAdminService : IMeetupAdminService
    {
        private readonly BaseDbContext _context;

        public MeetupAdminService(BaseDbContext baseDbContext)
        {
            _context = baseDbContext;
        }
        public async Task CreateAsync(Meetup meetup)
        {
            await _context.Meetups.AddAsync(meetup);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(string meetupId)
        {
            var meetup = _context.Meetups.FirstOrDefault(c => c.Id == Guid.Parse(meetupId));
            _context.Meetups.Remove(meetup);
            await _context.SaveChangesAsync();
        }

        public Meetup GetById(string meetupId)
        {
            return _context.Meetups.FirstOrDefault(c => c.Id == Guid.Parse(meetupId));
        }

        public IEnumerable<Meetup> GetMeetups(string userId)
        {
            return _context.Meetups.ToList().Where(c => c.CreatedByUser == userId);
        }

        public async Task Update(Meetup meetup)
        {
            _context.Meetups.Update(meetup);
            await _context.SaveChangesAsync();

        }
    }
}
