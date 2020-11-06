using ConnectUs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConnectUs.Domain.IRepositories
{
    public interface IUsersMeetupService
    {
        Task AddToMyMeetupsAsync(MeetupUser mu, User user);

    }
}
