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
        }

        public AutoMapperProfile(string profileName) : base(profileName)
        {
            UserModels();
        }
        protected void UserModels()
        {
            CreateMap<RegisterDTO, User>();
        }
    }
}
