using MTRS.Core.Entities;
using MTRS.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTRS.DAL.Interfaces
{
    public interface IActivityRepository : IGenericRepository<Activity, long>
    {
        List<Activity> GetList(long projectId);
        List<Activity> GetGeneral();
        List<Activity> GetUserProjectActivity(long projectId, long userId);
        List<Activity> GetByProjectId(long projectId);
        List<User> GetAssignedEmployeesByActivityId(long activityId);
        List<Activity> GetUserActivities(long projectId, long userId);
    }
}
