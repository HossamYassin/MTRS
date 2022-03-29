using MTRS.Core.DTOs;
using MTRS.Core.Entities;
using MTRS.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTRS.DAL.Interfaces
{
    public interface IUserRepository : IGenericRepository<User, long>
    {
        List<UserListItem> GetList();
        List<User> GetMyEmployees(long managerId, int level);
        UserDashboardDto GetDashboard(long userId);
        TeamDashboardDto GetTeamDashboard(long managerId);
    }
}
