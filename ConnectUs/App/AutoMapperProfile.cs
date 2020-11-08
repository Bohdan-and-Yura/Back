using AutoMapper;
using ConnectUs.Domain.DTO;
using ConnectUs.Domain.DTO.AccountDTO;
using ConnectUs.Domain.DTO.MeetupDTO;
using ConnectUs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConnectUs.Web.App
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            
            UserModels();
            MeetupAdminModels();
        }

        public AutoMapperProfile(string profileName) : base(profileName)
        {
            UserModels();
            MeetupAdminModels();
        }
        protected void MeetupAdminModels()
        {
            CreateMap<CreateMeetupDTO, Meetup>(MemberList.None);
            CreateMap<Meetup, MeetupResponseDTO>();
            CreateMap<MeetupUpdateDTO, Meetup>(MemberList.None);
        }
        protected void UserModels()
        {
            CreateMap<User, UserDTO>(MemberList.None).ForMember(c=>c.Id,o=>o.MapFrom(c=>c.Id));
            CreateMap<User, UserListDTO>(MemberList.None);
            CreateMap<EditUserDTO, User>(MemberList.None);

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
