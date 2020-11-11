using ConnectUs.Domain.DTO.MeetupDTO;
using ConnectUs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ConnectUs.Domain.IRepositories
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
