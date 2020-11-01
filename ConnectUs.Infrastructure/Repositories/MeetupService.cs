using ConnectUs.Domain.Entities;
using ConnectUs.Domain.Enums;
using ConnectUs.Domain.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ConnectUs.Infrastructure.Repositories
{
    public class MeetupService : IMeetupService
    {
        private readonly BaseDbContext _context;

        public MeetupService(BaseDbContext baseDbContext)
        {
            _context = baseDbContext;
        }
        public IQueryable<Meetup> GetList(string searchQuery, SortState sortState, bool isDescending = false)
        {
            IQueryable<Meetup> meetups = _context.Meetups.Include(c=>c.User);
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
                    case SortState.AuthorName:
                        meetups.OrderBy(c => c.User.UserName);
                        if (!string.IsNullOrEmpty(searchQuery))
                        {
                            meetups.Where(c => c.User.UserName.Contains(searchQuery));
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
                    case SortState.AuthorName:
                        meetups.OrderByDescending(c => c.User.UserName);
                        if (!string.IsNullOrEmpty(searchQuery))
                        {
                            meetups.Where(c => c.User.UserName.Contains(searchQuery));
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
