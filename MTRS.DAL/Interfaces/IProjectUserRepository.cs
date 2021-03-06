using MTRS.Core.Entities;
using MTRS.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTRS.DAL.Interfaces
{
    public interface IProjectUserRepository : IGenericRepository<ActivityUser, long>
    {
        int AssignEmployeeToActivity(ActivityUser activityUser);
        bool UnassignUser(ActivityUser activityUser);
    }
}
