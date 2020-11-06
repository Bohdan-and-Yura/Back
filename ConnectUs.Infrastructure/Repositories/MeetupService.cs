using AutoMapper;
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

        public MeetupService(BaseDbContext baseDbContext)
        {
            _context = baseDbContext;
        }

        public async Task AddUserToMeetup_AddMeetupToUserAsync(Meetup meetup, User user)
        {
            MeetupUser mu = new MeetupUser();
            mu.Meetup = meetup;
            mu.User = user;
            user.Meetups.Add(mu);
            meetup.Users.Add(mu);
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

        public IQueryable<Meetup> GetList(string searchQuery, SortState sortState, bool isDescending = false)
        {
            IQueryable<Meetup> meetups = _context.Meetups;
            if (isDescending == false)
            {
                switch (sortState)
                {
                    case SortState.Title:
                        meetups.OrderBy(c => c.Title);
                        if (!string.IsNullOrEmpty(searchQuery))
                        {
                            meetups.Where(c => c.Title.Contains(searchQuery));
                        }
                        break;
                    case SortState.MeetupDate:
                        meetups.OrderBy(c => c.MeetupDate);
                        break;
                    case SortState.City:
                        meetups.OrderBy(c => c.City);
                        if (!string.IsNullOrEmpty(searchQuery))
                        {
                            meetups.Where(c => c.City.Contains(searchQuery));
                        }
                        break;
                    default:
                        meetups.OrderBy(c => c.Title);
                        break;
                }

            }
            if (isDescending == true)
            {
                switch (sortState)
                {
                    case SortState.Title:
                        meetups.OrderByDescending(c => c.Title);
                        if (!string.IsNullOrEmpty(searchQuery))
                        {
                            meetups.Where(c => c.Title.Contains(searchQuery));
                        }
                        break;
                    case SortState.MeetupDate:
                        meetups.OrderByDescending(c => c.MeetupDate);
                        break;
                    case SortState.City:
                        meetups.OrderByDescending(c => c.City);
                        if (!string.IsNullOrEmpty(searchQuery))
                        {
                            meetups.Where(c => c.City.Contains(searchQuery));
                        }
                        break;
                    default:
                        meetups.OrderBy(c => c.Title);
                        break;
                }
            }
            return meetups;
        }
    }
}
