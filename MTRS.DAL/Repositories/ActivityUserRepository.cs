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
    public class ActivityUserRepository : GenericRepository<ActivityUser, long>, IProjectUserRepository
    {
        public ActivityUserRepository(MTRSDBContext context) : base(context)
        {
        }

        public int AssignEmployeeToActivity(ActivityUser activityUser)
        {
            var userExist = _context.ActivityUsers
                .Where(x => x.ActivityId == activityUser.ActivityId && x.UserId == activityUser.UserId).FirstOrDefault();

            if (userExist != null)
            {
                if (userExist.IsActive == true)
                {
                    return 0;
                }
                else
                {
                    userExist.IsActive = true;
                    _context.ActivityUsers.Update(userExist);
                    _context.SaveChanges();
                    return 1;
                }
            }

            _context.ActivityUsers.Add(activityUser);
            _context.SaveChanges();

            return 1;
        }

        public bool UnassignUser(ActivityUser activityUser)
        {
            var assignedActivity = _context.ActivityUsers
                .Where(x => x.UserId == activityUser.UserId && x.ActivityId == activityUser.ActivityId).FirstOrDefault();

            assignedActivity.IsActive = false;
            _context.Update(assignedActivity);
            _context.SaveChanges();

            return true;
        }
    }
}