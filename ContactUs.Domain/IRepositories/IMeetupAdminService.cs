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
        Task<bool> Delete(string meetupId, List<Claim> user);
        IEnumerable<Meetup> GetMeetups(List<Claim> user);
        Task<bool> Update(Meetup meetup, List<Claim> user);
        Task<Meetup> GetById(string meetupId);
    }
}
