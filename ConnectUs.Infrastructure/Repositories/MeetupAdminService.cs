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

        public async Task<bool> Delete(string meetupId, List<Claim> user)
        {

            var meetup = _context.Meetups.FirstOrDefault(c => c.Id == Guid.Parse(meetupId));
            if (meetup.CreatedByUser == user.FirstOrDefault().Value || user[1].Value == "Admin")
            {
                _context.Meetups.Remove(meetup);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<Meetup> GetById(string meetupId, List<Claim> user)
        {
            return await _context.Meetups.FirstOrDefaultAsync(c => c.Id == Guid.Parse(meetupId));
        }

        public IEnumerable<Meetup> GetMeetups(List<Claim> user)
        {
            return _context.Meetups.ToList().Where(c => c.CreatedByUser == user.FirstOrDefault().Value);
        }

        public async Task<bool> Update(Meetup meetup, List<Claim> user)
        {
            if (meetup.CreatedByUser == user.FirstOrDefault().Value || user[1].Value == "Admin")
            {

                _context.Meetups.Update(meetup);
                await _context.SaveChangesAsync();
                return true;

            }
            return false;

        }
    }
}
