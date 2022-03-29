using Microsoft.EntityFrameworkCore;
using MTRS.Core.DTOs;
using MTRS.Core.Entities;
using MTRS.DAL.DbContexts;
using MTRS.DAL.Interfaces;
using MTRS.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTRS.DAL.Interfaces
{
    public class ActivityRepository : GenericRepository<Activity, long>, IActivityRepository
    {
        public ActivityRepository(MTRSDBContext context) : base(context)
        {
        }

        public List<Activity> GetList(long projectId)
        {
            var query = _context.Activities.AsQueryable().
                Include(x => x.Project).Where(z => z.ProjectId == projectId);

            return query.ToList();
        }

        public List<Activity> GetGeneral()
        {
            var query = _context.Activities.Where(x => x.Type == Core.Enums.ActivityType.General);
            return query.ToList();
        }

        public List<Activity> GetUserProjectActivity(long projectId, long userId)
        {
            var query = from activity in _context.Activities
                        join project in _context.Projects
                        on activity.ProjectId equals project.Id
                        join userActivity in _context.ActivityUsers
                        on activity.Id equals userActivity.ActivityId
                        where userActivity.IsActive == true &&
                        userActivity.UserId == userId &&
                        project.Id == projectId
                        select activity;

            return query.ToList();
        }

        public List<Activity> GetUserActivities(long projectId, long userId)
        {
            var query = from activity in _context.Activities
                        join project in _context.Projects
                        on activity.ProjectId equals project.Id
                        join userActivity in _context.ActivityUsers
                        on activity.Id equals userActivity.ActivityId
                        where userActivity.IsActive == true &&
                        userActivity.UserId == userId && project.IsCompleted == false &&
                        project.Id == projectId && (DateTime.Now.Date >= activity.StartDate.Value.Date && DateTime.Now.Date <= activity.EndDate.Value.Date)
                        select activity;

            return query.ToList();
        }

        public List<Activity> GetByProjectId(long projectId)
        {
            var query = _context.Activities.Where(x => x.ProjectId == projectId);
            return query.ToList();
        }

        public List<User> GetAssignedEmployeesByActivityId(long activityId)
        {
            var query = from activity in _context.Activities
                        join project in _context.Projects
                        on activity.ProjectId equals project.Id
                        join userActivity in _context.ActivityUsers
                        on activity.Id equals userActivity.ActivityId
                        join user in _context.Users
                        on userActivity.UserId equals user.Id
                        where userActivity.IsActive == true && activity.Id == activityId
                        select user;

            return query.ToList();
        }

    }
}
