using System.Collections.Generic;
using System.Threading.Tasks;
using ConnectUs.Domain.DTO.MeetupDTO;
using ConnectUs.Domain.Entities;

namespace ConnectUs.Domain.IRepositories.Admin
{
    public interface IAdminMeetupService
    {
        Task CreateAsync(Meetup meetup);
        Task<bool> Delete(string meetupId, string userId);
        IEnumerable<Meetup> GetMyMeetups(string userId);
        Task<bool> Update(MeetupUpdateDTO meetup, string userId, string meetupId);
        Task<Meetup> GetById(string meetupId);
    }
}