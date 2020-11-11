using AutoMapper;
using ConnectUs.Domain.DTO.JoinedDTO;
using ConnectUs.Domain.DTO.MeetupDTO;
using ConnectUs.Domain.Entities;
using ConnectUs.Domain.Enums;
using ConnectUs.Domain.IRepositories.Public;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

            var meetups = _context.MeetupsUsers.Include(c => c.Meetup).Where(c => c.UserId == userId).AsNoTracking().ToList();
            var result = _mapper.Map<List<JoinedListDTO>>(meetups);
            return result;
        }


        public async Task<bool> JoinMeetup(Meetup meetup, User user)
        {
            var result = _context.MeetupsUsers.Any(c => c.MeetupId == meetup.Id && c.UserId == user.Id);
            if (result)
            {
                return false;
            }
            MeetupUser mu = new MeetupUser();
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
                return await _context.Meetups.Include(c => c.UsersJoined).FirstOrDefaultAsync(c => c.Id == Guid.Parse(meetupId));
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

        public async Task<bool> UnjoinMeetup(string userId, string meetupId)
        {
            try
            {
                var result = await _context.MeetupsUsers.FirstOrDefaultAsync(c => c.MeetupId == Guid.Parse(meetupId) && c.UserId == userId);
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
