using ConnectUs.Domain.DTO.JoinedDTO;
using ConnectUs.Domain.DTO.MeetupDTO;
using ConnectUs.Domain.Entities;
using ConnectUs.Domain.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConnectUs.Domain.IRepositories.Public
{
    public interface IMeetupService
    {
        Task<IEnumerable<MeetupResponseDTO>> GetList(string searchQuery, SortState sortState, bool isDescending = false);
        Task<Meetup> GetByIdAsync(string meetupId);
        Task<bool> JoinMeetup(Meetup meetup, User user);
        List<JoinedListDTO> GetJoinedMeetups(string userId);
        Task<bool> UnjoinMeetup(string userId, string meetupId);
    }
}
