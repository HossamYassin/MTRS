using Microsoft.EntityFrameworkCore;
using MTRS.Core.Entities;
using MTRS.Core.Enums;
using MTRS.DAL.DbContexts;
using MTRS.DAL.Interfaces;
using MTRS.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTRS.DAL.Repositories
{
    public class TimeSheetDetailsRepository : GenericRepository<TimeSheetDetails, long>, ITimeSheetDetailsRepository
    {
        public TimeSheetDetailsRepository(MTRSDBContext context) : base(context)
        {
        }

        public List<TimeSheetDetails> GetByTimeSheetId(long timesheetId)
        {
            var query = (from timesheet in _context.TimeSheets
                        join timesheetDetails in _context.TimeSheetDetails
                        on timesheet.Id equals timesheetDetails.TimeSheetId
                        where timesheetDetails.TimeSheetId == timesheetId
                        select timesheetDetails).Include(x=>x.TimeSheet).Include(y=>y.Activity);

            return query.ToList();
        }

        public bool SaveSheetActivities(List<TimeSheetDetails> timeSheetDetails, TimeSheetStatus status)
        {
            var entries = (from timesheet in _context.TimeSheets
                              join timesheetDetails in _context.TimeSheetDetails
                              on timesheet.Id equals timesheetDetails.TimeSheetId
                              where timesheetDetails.TimeSheetId == timeSheetDetails[0].TimeSheetId
                              select timesheetDetails).ToList();

            foreach (var entry in entries)
            {
                _context.Remove<TimeSheetDetails>(entry);
            }

            foreach (var activity in timeSheetDetails)
            {
                _context.Add(activity);
            }

            var tsObj = _context.TimeSheets.Where(x => x.Id == timeSheetDetails[0].TimeSheetId).FirstOrDefault();

            if (status == TimeSheetStatus.Draft)
            {
                if (tsObj.StatusId != TimeSheetStatus.Submited)
                {
                    tsObj.StatusId = status;
                }
            }

            if (status == TimeSheetStatus.Submited)
            {
                tsObj.StatusId = TimeSheetStatus.Submited;
                tsObj.SubmitedOn = DateTime.Now;
            }

            if (status == TimeSheetStatus.Resubmited)
            {
                tsObj.StatusId = TimeSheetStatus.Resubmited;

                var timesheetApproval = new TimeSheetApproval()
                {
                    Status = TimeSheetStatus.Resubmited,
                    TransactionDate = DateTime.Now,
                    UserId = tsObj.UserId,
                    TimeSheetId = tsObj.Id
                };

                _context.TimeSheetApprovals.Add(timesheetApproval);
            }

            //auto approval
            var currentUser = _context.Users.Where(x => x.Id == tsObj.UserId && x.IsProjectManager).FirstOrDefault();
            if (currentUser != null)
            {
                var managers = (from user in _context.Users
                                join timesheet in _context.TimeSheets
                                on user.Id equals timesheet.UserId
                                join timesheetDetails in _context.TimeSheetDetails
                                on timesheet.Id equals timesheetDetails.TimeSheetId
                                join activity in _context.Activities
                                on timesheetDetails.ActivityId equals activity.Id
                                join project in _context.Projects
                                on activity.ProjectId equals project.Id
                                join manager in _context.Users
                                on project.ManagerId equals manager.Id
                                where timesheet.Id == timeSheetDetails[0].TimeSheetId
                                select manager)
                          .Union(from user in _context.Users
                                 join timesheet in _context.TimeSheets
                                  on user.Id equals timesheet.UserId
                                 join manager in _context.Users
                                  on user.ManagerId equals manager.Id
                                 where timesheet.Id == timeSheetDetails[0].TimeSheetId
                                 select manager).ToList();

                if (managers.Contains(currentUser))
                {
                    var approved = _context.TimeSheetApprovals.Where(x => x.TimeSheetId == tsObj.Id && x.ManagerId == tsObj.UserId).FirstOrDefault();
                    if (approved == null)
                    {
                        var timesheetApproval = new TimeSheetApproval()
                        {
                            Status = TimeSheetStatus.Approved,
                            TransactionDate = DateTime.Now,
                            ManagerId = tsObj.UserId,
                            TimeSheetId = tsObj.Id
                        };

                        _context.TimeSheetApprovals.Add(timesheetApproval);
                    }
                }
            }

            _context.SaveChanges();
            return true;
        }
    }
}
