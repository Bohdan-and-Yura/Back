﻿using AutoMapper;
using ConnectUs.Domain.DTO.AccountDTO;
using ConnectUs.Domain.DTO.JoinedDTO;
using ConnectUs.Domain.DTO.MeetupDTO;
using ConnectUs.Domain.DTO.UserDTO;
using ConnectUs.Domain.Entities;

namespace ConnectUs.Web.App
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            UserModels();
            MeetupAdminModels();
            Joined();
        }

        public AutoMapperProfile(string profileName) : base(profileName)
        {
            UserModels();
            MeetupAdminModels();
            Joined();
        }

        protected void MeetupAdminModels()
        {
            CreateMap<CreateMeetupDTO, Meetup>(MemberList.None);
            CreateMap<Meetup, MeetupResponseDTO>()
                .ForMember(c => c.UserCreatorId, o => o.MapFrom(c => c.UserCreator.Id))
                .ForMember(c => c.UserCreatorName, o => o.MapFrom(c => c.UserCreator.UserName));
            CreateMap<Meetup, MeetupUsersDTO>()
                .ForMember(c => c.UserCreatorId, o => o.MapFrom(c => c.UserCreator.Id))
                .ForMember(c => c.UserCreatorName, o => o.MapFrom(c => c.UserCreator.UserName));

            CreateMap<MeetupUpdateDTO, Meetup>(MemberList.None);
        }

        public void Joined()
        {
            CreateMap<MeetupUser, JoinedListDTO>();
        }

        protected void UserModels()
        {
            CreateMap<User, UserDataDTO>(MemberList.None).ForMember(c => c.Id, o => o.MapFrom(c => c.Id));
            CreateMap<User, UserListDTO>(MemberList.None);
            CreateMap<User, UserMeetupsDTO>(MemberList.None);
            CreateMap<EditUserDTO, User>(MemberList.None);
            CreateMap<MeetupUser, UserDataDTO>(MemberList.None);

            CreateMap<RegisterDTO, User>(MemberList.None);
            //.ForMember(c => c.AccessFailedCount, o => o.Ignore())
            //.ForMember(c => c.ConcurrencyStamp, o => o.Ignore())
            //.ForMember(c => c.EmailConfirmed, o => o.Ignore())
            //.ForMember(c => c.Id, o => o.Ignore())
            //.ForMember(c => c.LockoutEnabled, o => o.Ignore())
            //.ForMember(c => c.LockoutEnd, o => o.Ignore())
            //.ForMember(c => c.Meetups, o => o.Ignore())
            //.ForMember(c => c.NormalizedEmail, o => o.Ignore())
            //.ForMember(c => c.NormalizedUserName, o => o.Ignore())
            //.ForMember(c => c.PasswordHashByte, o => o.Ignore())
            //.ForMember(c => c.PasswordHash, o => o.Ignore())
            //.ForMember(c => c.PasswordSaltByte, o => o.Ignore())
            //.ForMember(c => c.PhoneNumber, o => o.Ignore())
            //.ForMember(c => c.PhoneNumberConfirmed, o => o.Ignore())
            //.ForMember(c => c.SecurityStamp, o => o.Ignore())
            //.ForMember(c => c.Role, o => o.Ignore())
            //.ForMember(c => c.Token, o => o.Ignore())
            //.ForMember(c => c.BirthDay, o => o.Ignore())
            //.ForMember(c => c.TwoFactorEnabled, o => o.Ignore());

            CreateMap<LoginRequestDTO, User>(MemberList.None);
        }
    }
}