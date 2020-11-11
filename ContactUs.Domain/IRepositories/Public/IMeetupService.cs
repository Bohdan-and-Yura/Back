﻿using ConnectUs.Domain.DTO.JoinedDTO;
using ConnectUs.Domain.DTO.MeetupDTO;
using ConnectUs.Domain.Entities;
using ConnectUs.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectUs.Domain.IRepositories
{
    public interface IMeetupService
    {
        Task<IEnumerable<MeetupResponseDTO>> GetList(string searchQuery, SortState sortState,bool isDescending=false);
        Task<Meetup> GetByIdAsync(string meetupId);
        Task<bool> JoinMeetup(Meetup meetup, User user);
        List<JoinedListDTO> GetJoinedMeetups(string userId);
        Task<bool> UnjoinMeetup(string userId, string meetupId);
    }
}