using ConnectUs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ConnectUs.Domain.IRepositories
{
    public interface IMeetupAdminService
    {
        Task CreateAsync(Meetup meetup);
        Task<bool> Delete(string meetupId, string userId);
        IEnumerable<Meetup> GetMeetups(string userId);
        Task<bool> Update(Meetup meetup, string userId);
        Task<Meetup> GetById(string meetupId);
        
    }
}
