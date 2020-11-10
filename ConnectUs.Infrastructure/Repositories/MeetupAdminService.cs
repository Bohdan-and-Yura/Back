using ConnectUs.Domain.Entities;
using ConnectUs.Domain.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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

        public async Task<bool> Delete(string meetupId,string userId)
        {

            var meetup = _context.Meetups.FirstOrDefault(c => c.Id == Guid.Parse(meetupId));
            if (meetup.CreatedByUser == userId)
            {
                _context.Meetups.Remove(meetup);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<Meetup> GetById(string meetupId)
        {
            return await _context.Meetups.FirstOrDefaultAsync(c => c.Id == Guid.Parse(meetupId));
        }

        public IEnumerable<Meetup> GetMeetups(string userId)
        {
            return _context.Meetups.ToList().Where(c => c.CreatedByUser == userId).Reverse();
        }

        public async Task<bool> Update(Meetup meetup, string userId)
        {
            if (meetup.CreatedByUser == userId)
            {

                _context.Meetups.Update(meetup);
                await _context.SaveChangesAsync();
                return true;

            }
            return false;

        }
        public List<MeetupUser> GetJoinedMeetups(string userId)
        {
        
            var user = _context.Users.FirstOrDefault(c=>c.Id== userId);
            var meetups = user.MeetupsJoined.ToList();
            return meetups;
        }
    }
}
