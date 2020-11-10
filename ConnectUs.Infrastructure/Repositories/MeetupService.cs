using AutoMapper;
using ConnectUs.Domain.DTO.AccountDTO;
using ConnectUs.Domain.DTO.MeetupDTO;
using ConnectUs.Domain.Entities;
using ConnectUs.Domain.Enums;
using ConnectUs.Domain.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConnectUs.Infrastructure.Repositories
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
        public List<MeetupUser> GetJoinedMeetups(string userId)
        {

            var user = _context.Users.FirstOrDefault(c => c.Id == userId);
            var meetups = user.MeetupsJoined.ToList();
            return meetups;
        }

        public void UnjoinMeetup(string userId, string meetupId)
        {
            var user = _context.Users.FirstOrDefault(c => c.Id == userId);

        }
        public async Task AddUserToMeetup(Meetup meetup, User user)
        {
            MeetupUser mu = new MeetupUser();
            mu.Meetup = meetup;
            mu.User = user;
            user.MeetupsJoined.Add(mu);
            meetup.UsersJoined.Add(mu);
            _context.MeetupsUsers.Add(mu);
            await _context.SaveChangesAsync();

        }
       
        public async Task<Meetup> GetByIdAsync(string meetupId)
        {
            try
            {
                return await _context.Meetups/*.Include(c=>c.Users)*/.FirstOrDefaultAsync(c => c.Id == Guid.Parse(meetupId));
            }
            catch
            {
                return null;
            }
        }

        public async Task<IEnumerable<MeetupResponseDTO>> GetList(string searchQuery, SortState sortState, bool isDescending = false)
        {
            var meetups = await _context.Meetups.ToListAsync();

            var result = _mapper.Map<IEnumerable<MeetupResponseDTO>>(meetups);
            if (isDescending == false)
            {
                switch (sortState)
                {
                    case SortState.Title:
                        result.OrderBy(c => c.Title);
                        if (!string.IsNullOrEmpty(searchQuery))
                        {
                            result.Where(c => c.Title.Contains(searchQuery));
                        }
                        break;
                    case SortState.MeetupDate:
                        result.OrderBy(c => c.MeetupDate);
                        break;
                    case SortState.City:
                        result.OrderBy(c => c.City);
                        if (!string.IsNullOrEmpty(searchQuery))
                        {
                            result.Where(c => c.City.Contains(searchQuery));
                        }
                        break;
                    default:
                        result.OrderBy(c => c.Title);
                        break;
                }

            }
            if (isDescending == true)
            {
                switch (sortState)
                {
                    case SortState.Title:
                        result.OrderByDescending(c => c.Title);
                        if (!string.IsNullOrEmpty(searchQuery))
                        {
                            result.Where(c => c.Title.Contains(searchQuery));
                        }
                        break;
                    case SortState.MeetupDate:
                        result.OrderByDescending(c => c.MeetupDate);
                        break;
                    case SortState.City:
                        result.OrderByDescending(c => c.City);
                        if (!string.IsNullOrEmpty(searchQuery))
                        {
                            result.Where(c => c.City.Contains(searchQuery));
                        }
                        break;
                    default:
                        result.OrderBy(c => c.Title);
                        break;
                }
            }

            return result;
        }
    }
}
