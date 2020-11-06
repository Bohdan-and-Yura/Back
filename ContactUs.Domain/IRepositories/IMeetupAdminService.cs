using ConnectUs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConnectUs.Domain.IRepositories
{
    public interface IMeetupAdminService
    {
        Task CreateAsync(Meetup meetup);
        Task Delete(string meetupId);
        IEnumerable<Meetup> GetMeetups(string userId);
        Task Update(Meetup meetup);
        Meetup GetById(string meetupId);
    }
}
