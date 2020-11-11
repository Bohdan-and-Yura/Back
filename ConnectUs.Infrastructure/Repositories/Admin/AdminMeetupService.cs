using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ConnectUs.Domain.DTO.MeetupDTO;
using ConnectUs.Domain.Entities;
using ConnectUs.Domain.IRepositories.Admin;
using Microsoft.EntityFrameworkCore;

namespace ConnectUs.Infrastructure.Repositories.Admin
{
    public class AdminMeetupService : IAdminMeetupService
    {
        private readonly BaseDbContext _context;
        private readonly IMapper _mapper;

        public AdminMeetupService(BaseDbContext baseDbContext, IMapper mapper)
        {
            _context = baseDbContext;
            _mapper = mapper;
        }

        public async Task CreateAsync(Meetup meetup)
        {
            var user = await _context.Users.FirstOrDefaultAsync(c => c.Id == meetup.CreatedByUser);
            meetup.UserCreator = user;
            await _context.Meetups.AddAsync(meetup);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> Delete(string meetupId, string userId)
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

        public IEnumerable<Meetup> GetMyMeetups(string userId)
        {
            return _context.Meetups.ToList().Where(c => c.CreatedByUser == userId).Reverse();
        }

        public async Task<bool> Update(MeetupUpdateDTO meetup, string userId, string meetupId)
        {
            var meetupOriginal = _context.Meetups.FirstOrDefault(c => c.Id == Guid.Parse(meetupId));
            var res = _mapper.Map(meetup, meetupOriginal);
            if (res.CreatedByUser.ToLower() == userId.ToLower())
            {
                _context.Meetups.Update(res);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}