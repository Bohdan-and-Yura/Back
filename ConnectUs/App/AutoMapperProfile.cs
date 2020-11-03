using AutoMapper;
using ConnectUs.Domain.DTO;
using ConnectUs.Domain.DTO.AccountDTO;
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
        }

        public AutoMapperProfile(string profileName) : base(profileName)
        {
            UserModels();
        }
        protected void UserModels()
        {
            CreateMap<RegisterDTO, User>()
                .ForMember(c => c.AccessFailedCount, o => o.Ignore())
                .ForMember(c => c.ConcurrencyStamp, o => o.Ignore())
                .ForMember(c => c.EmailConfirmed, o => o.Ignore())
                .ForMember(c => c.Id, o => o.Ignore())
                .ForMember(c => c.LockoutEnabled, o => o.Ignore())
                .ForMember(c => c.LockoutEnd, o => o.Ignore())
                .ForMember(c => c.Meetups, o => o.Ignore())
                .ForMember(c => c.NormalizedEmail, o => o.Ignore())
                .ForMember(c => c.NormalizedUserName, o => o.Ignore())
                .ForMember(c => c.PasswordHashByte, o => o.Ignore())
                .ForMember(c => c.PasswordHash, o => o.Ignore())
                .ForMember(c => c.PasswordSaltByte, o => o.Ignore())
                .ForMember(c => c.PhoneNumber, o => o.Ignore())
                .ForMember(c => c.PhoneNumberConfirmed, o => o.Ignore())
                .ForMember(c => c.SecurityStamp, o => o.Ignore())
                .ForMember(c => c.Role, o => o.Ignore())
                .ForMember(c => c.Token, o => o.Ignore())
                .ForMember(c => c.BirthDay, o => o.Ignore())
                .ForMember(c => c.TwoFactorEnabled, o => o.Ignore());

            CreateMap<LoginRequestDTO, User>()
                .ForMember(c => c.AccessFailedCount, o => o.Ignore())
                .ForMember(c => c.ConcurrencyStamp, o => o.Ignore())
                .ForMember(c => c.ConfirmPassword, o => o.Ignore())
                .ForMember(c => c.EmailConfirmed, o => o.Ignore())
                .ForMember(c => c.Id, o => o.Ignore())
                .ForMember(c => c.LockoutEnabled, o => o.Ignore())
                .ForMember(c => c.LockoutEnd, o => o.Ignore())
                .ForMember(c => c.Meetups, o => o.Ignore())
                .ForMember(c => c.NormalizedEmail, o => o.Ignore())
                .ForMember(c => c.NormalizedUserName, o => o.Ignore())
                .ForMember(c => c.PasswordHashByte, o => o.Ignore())
                .ForMember(c => c.PasswordHash, o => o.Ignore())
                .ForMember(c => c.PasswordSaltByte, o => o.Ignore())
                .ForMember(c => c.PhoneNumber, o => o.Ignore())
                .ForMember(c => c.PhoneNumberConfirmed, o => o.Ignore())
                .ForMember(c => c.SecurityStamp, o => o.Ignore())
                .ForMember(c => c.TwoFactorEnabled, o => o.Ignore())
                .ForMember(c => c.UserName, o => o.Ignore())
                .ForMember(c => c.Role, o => o.Ignore())
                .ForMember(c => c.Token, o => o.Ignore())
                .ForMember(c => c.BirthDay, o => o.Ignore());
        }
    }
}
