using ConnectUs.Domain.Entities;
using ConnectUs.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConnectUs.Domain.IRepositories
{
    public interface IMeetupService
    {
        IQueryable<Meetup> GetList(string searchQuery, SortState sortState,bool isDescending=false);
    }
}
