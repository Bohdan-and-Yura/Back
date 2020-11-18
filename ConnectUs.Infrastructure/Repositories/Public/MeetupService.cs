using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ConnectUs.Domain.DTO.JoinedDTO;
using ConnectUs.Domain.DTO.MeetupDTO;
using ConnectUs.Domain.Entities;
using ConnectUs.Domain.Enums;
using ConnectUs.Domain.IRepositories.Public;
using Microsoft.EntityFrameworkCore;

namespace ConnectUs.Infrastructure.Repositories.Public
{
    public class MeetupService : IMeetupService
    {
        private readonly BaseDbContext _context;
        private readonly IMapper _mapper;

        public MeetupService(BaseDbContext baseDbContext, IMapper mapper)
        {
            _context = baseDbContext;
            _mapper = mapper;
        }

        public List<JoinedListDTO> GetJoinedMeetups(string userId)
        {
            var meetups = _context.MeetupsUsers.Include(c => c.Meetup).Where(c => c.UserId == userId).AsNoTracking()
                .ToList();
            var result = _mapper.Map<List<JoinedListDTO>>(meetups);
            return result;
        }


        public async Task<bool> JoinMeetup(Meetup meetup, User user)
        {
            var result = _context.MeetupsUsers.Any(c => c.MeetupId == meetup.Id && c.UserId == user.Id);
            if (result) return false;
            var mu = new MeetupUser();
            mu.Meetup = meetup;
            mu.User = user;
            user.MeetupsJoined.Add(mu);
            meetup.UsersJoined.Add(mu);
            _context.MeetupsUsers.Add(mu);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Meetup> GetByIdAsync(string meetupId)
        {
            try
            {
                return await _context.Meetups.Include(c => c.UsersJoined)
                    .FirstOrDefaultAsync(c => c.Id == Guid.Parse(meetupId));
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<MeetupResponseDTO> GetList(string searchQuery, SortState sortState,
            bool isDescending = false)
        {
            IQueryable<Meetup> meetups = _context.Meetups;

            if (isDescending == false)
            {
                switch (sortState)
                {
                    case SortState.Title:
                        if (!string.IsNullOrEmpty(searchQuery)) meetups = meetups.Where(c => c.Title.Contains(searchQuery)).OrderBy(c => c.Title);
                        break;
                    case SortState.MeetupDate:
                        meetups = meetups.OrderBy(c => c.MeetupDate).Reverse();
                        break;
                    case SortState.City:
                        if (!string.IsNullOrEmpty(searchQuery)) meetups = meetups.Where(c => c.City.Contains(searchQuery)).OrderBy(c => c.City);
                        break;
                    default:
                        meetups = meetups.OrderBy(c => c.Title);
                        break;
                }
            }

            if (isDescending)
            {
                switch (sortState)
                {
                    case SortState.Title:
                        if (!string.IsNullOrEmpty(searchQuery)) meetups = meetups.Where(c => c.Title.Contains(searchQuery)).OrderByDescending(c => c.Title);
                        break;
                    case SortState.MeetupDate:
                        meetups = meetups.OrderByDescending(c => c.MeetupDate).Reverse();
                        break;
                    case SortState.City:
                        if (!string.IsNullOrEmpty(searchQuery)) meetups = meetups.Where(c => c.City.Contains(searchQuery)).OrderByDescending(c => c.City); ;
                        break;
                    default:
                        meetups = meetups.OrderBy(c => c.Title);
                        break;
                }
            }
            return  _mapper.Map<IEnumerable<MeetupResponseDTO>>(meetups.ToList());

        }

        public async Task<bool> UnjoinMeetup(string userId, string meetupId)
        {
            try
            {
                var result =
                    await _context.MeetupsUsers.FirstOrDefaultAsync(c =>
                        c.MeetupId == Guid.Parse(meetupId) && c.UserId == userId);
                _context.MeetupsUsers.Remove(result);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return true;
            }
        }
    }
}