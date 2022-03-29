using Microsoft.EntityFrameworkCore;
using MTRS.Core.DTOs;
using MTRS.Core.Entities;
using MTRS.Core.Enums;
using MTRS.DAL.DbContexts;
using MTRS.DAL.Interfaces;
using MTRS.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTRS.DAL.Interfaces
{
    public class TimeSheetRepository : GenericRepository<TimeSheet, long>, ITimeSheetRepository
    {
        public TimeSheetRepository(MTRSDBContext context) : base(context)
        {
        }

        public List<TimeSheet> GetPendingTimeSheets(long employeeId)
        {
            var query = (from user in _context.Users
                        join timesheet in _context.TimeSheets
                        on user.Id equals timesheet.UserId
                        where (timesheet.StatusId == TimeSheetStatus.Draft || timesheet.StatusId == TimeSheetStatus.Submited
                        || timesheet.StatusId == TimeSheetStatus.Resubmited)
                        && timesheet.UserId == employeeId
                        orderby timesheet.FromDate ascending
                        select timesheet).Distinct();

            var timesheets = query.ToList();
            var sheets = new List<TimeSheet>();
            sheets.AddRange(timesheets.ToArray());

            foreach (var timesheet in timesheets)
            {
                var sheetDetailsList = (from sheet in _context.TimeSheets
                            join timesheetDetails in _context.TimeSheetDetails
                            on sheet.Id equals timesheetDetails.TimeSheetId
                            where sheet.Id == timesheet.Id
                            select timesheetDetails);

                int? totalHours = 0;
                foreach (var details in sheetDetailsList)
                {
                    totalHours += details.Sat + details.SatOver + details.Sun + details.SunOver + details.Mon + details.MonOver + details.Tu + details.TuOver
                        + details.We + details.WeOver + details.Th + details.ThOver + details.Fri + details.FriOver;
                }

                timesheet.TotalHours = totalHours;

                var status = GetStatus(timesheet.Id);
                if (status == TimeSheetStatus.Approved || status == TimeSheetStatus.Submited || status == TimeSheetStatus.Resubmited)
                {
                    sheets.Remove(timesheet);
                }

                timesheet.StatusId = GetStatus(timesheet.Id);
            }

            return sheets;
        }

        public List<TimeSheet> GetNewByUserId(long userId)
        {
            var query = from user in _context.Users
                        join timesheet in _context.TimeSheets
                        on user.Id equals timesheet.UserId
                        where (timesheet.StatusId == TimeSheetStatus.Draft || timesheet.StatusId == TimeSheetStatus.Submited 
                        || timesheet.StatusId == TimeSheetStatus.Resubmited)
                        && timesheet.UserId == userId
                        orderby timesheet.FromDate ascending
                        select timesheet;

            var timesheets = query.ToList();
            var sheets = new List<TimeSheet>();
            sheets.AddRange(timesheets.ToArray());

            foreach (var timesheet in timesheets)
            {
                var status = GetStatus(timesheet.Id);
                if (status == TimeSheetStatus.Approved || status == TimeSheetStatus.Submited || status == TimeSheetStatus.Resubmited)
                {
                    sheets.Remove(timesheet);
                }
            }

            return sheets;
        }

        public TimeSheet GetTimeSheetByDate(long userId, DateTime startDate)
        {
            var query = from timesheet in _context.TimeSheets
                        where timesheet.UserId == userId && timesheet.FromDate.Date == startDate.Date
                        orderby timesheet.FromDate ascending
                        select timesheet;

            return query.FirstOrDefault();
        }

        public TimeSheet GetByDate(long userId, DateTime startDate)
        {
            var query = from timesheet in _context.TimeSheets
                        where timesheet.StatusId != Core.Enums.TimeSheetStatus.Draft 
                        && timesheet.StatusId != Core.Enums.TimeSheetStatus.Rejected && 
                        timesheet.UserId == userId && timesheet.FromDate.Date == startDate.Date
                        orderby timesheet.FromDate ascending
                        select timesheet;

            return query.FirstOrDefault();
        }

        public List<TimeSheet> GetHistory(long userId)
        {
            var sheets = (from timesheet in _context.TimeSheets
                          where timesheet.StatusId != TimeSheetStatus.Draft
                          && timesheet.UserId == userId && timesheet.FromDate.Date.Year > DateTime.Now.Year - 1
                          orderby timesheet.FromDate ascending
                          select timesheet).Distinct().ToList();

            foreach (var timesheet in sheets)
            {
                var sheetDetailsList = (from sheet in _context.TimeSheets
                                        join timesheetDetails in _context.TimeSheetDetails
                                        on sheet.Id equals timesheetDetails.TimeSheetId
                                        where sheet.Id == timesheet.Id
                                        select timesheetDetails);

                int? totalHours = 0;
                foreach (var details in sheetDetailsList)
                {
                    totalHours += details.Sat + details.SatOver + details.Sun + details.SunOver + details.Mon + details.MonOver + details.Tu + details.TuOver
                        + details.We + details.WeOver + details.Th + details.ThOver + details.Fri + details.FriOver;
                }

                timesheet.TotalHours = totalHours;

                timesheet.StatusId = GetStatus(timesheet.Id);
            }

            return sheets;
        }

        public List<TimeSheet> GetNewSubmitedByProjectAndUser(long userId, long projectId)
        {
            var query = from user in _context.Users
                        join timesheet in _context.TimeSheets
                        on user.Id equals timesheet.UserId
                        join timesheetDetails in _context.TimeSheetDetails
                        on timesheet.Id equals timesheetDetails.TimeSheetId
                        join activity in _context.Activities
                        on timesheetDetails.ActivityId equals activity.Id
                        join project in _context.Projects
                        on activity.ProjectId equals project.Id
                        where timesheet.StatusId == TimeSheetStatus.Submited
                        && project.Id == projectId && user.Id == userId
                        orderby timesheet.FromDate ascending
                        select timesheet;

            return query.ToList();
        }

        public List<TimeSheet> GetProjectHistoryByUser(long userId, long projectId)
        {
            var query = from timesheet in _context.TimeSheets
                        join user in _context.Users
                        on timesheet.UserId equals user.Id
                        join timesheetDetails in _context.TimeSheetDetails
                        on timesheet.Id equals timesheetDetails.TimeSheetId
                        join activity in _context.Activities
                        on timesheetDetails.ActivityId equals activity.Id
                        join project in _context.Projects
                        on activity.ProjectId equals project.Id
                        where timesheet.StatusId != TimeSheetStatus.Draft
                        && project.Id == projectId && timesheet.UserId == userId
                        orderby timesheet.FromDate ascending
                        select timesheet;

            return query.ToList();
        }

        public List<User> GetEmployeesByProject(long projectId)
        {
            var uesrs = (from user in _context.Users
                              join timesheet in _context.TimeSheets
                              on user.Id equals timesheet.UserId
                              join timesheetDetails in _context.TimeSheetDetails
                              on timesheet.Id equals timesheetDetails.TimeSheetId
                              join activity in _context.Activities
                              on timesheetDetails.ActivityId equals activity.Id
                              join project in _context.Projects
                              on activity.ProjectId equals project.Id
                              where project.Id == projectId && (timesheet.StatusId == TimeSheetStatus.Submited
                              || timesheet.StatusId == TimeSheetStatus.Resubmited || timesheet.StatusId == TimeSheetStatus.Approved)
                              orderby timesheet.FromDate ascending
                              select user).Distinct().ToList();

            return uesrs;
        }
        
        public List<User> GetPendingEmployeesByProject(long projectId)
        {
            var timeSheets = (from user in _context.Users
                        join timesheet in _context.TimeSheets
                        on user.Id equals timesheet.UserId
                        join timesheetDetails in _context.TimeSheetDetails
                        on timesheet.Id equals timesheetDetails.TimeSheetId
                        join activity in _context.Activities
                        on timesheetDetails.ActivityId equals activity.Id
                        join project in _context.Projects
                        on activity.ProjectId equals project.Id
                        where project.Id == projectId && (timesheet.StatusId == TimeSheetStatus.Submited
                        || timesheet.StatusId == TimeSheetStatus.Resubmited)
                        orderby timesheet.FromDate ascending
                        select timesheet).Distinct().ToList();

            List<User> pendingUsers = new List<User>();
            
            foreach (var timesheet in timeSheets)
            {
                var managerId = _context.Projects.Where(x => x.Id == projectId).First().ManagerId;
                var currentTransaction = _context.TimeSheetApprovals.Where(x => x.TimeSheetId == timesheet.Id && x.ManagerId == managerId)
                    .OrderByDescending(x => x.TransactionDate).FirstOrDefault();

                if (currentTransaction != null)
                {
                    if (currentTransaction.Status == TimeSheetStatus.Rejected 
                        && timesheet.StatusId == TimeSheetStatus.Resubmited)
                    {
                        var user = _context.Users.Where(x => x.Id == timesheet.UserId).FirstOrDefault();
                        var userResubmit = _context.TimeSheetApprovals
                            .Where(x => x.TimeSheetId == currentTransaction.TimeSheetId && x.UserId == user.Id)
                            .OrderByDescending(x=>x.TransactionDate).FirstOrDefault();
                        if (userResubmit != null)
                        {
                            if (userResubmit.TransactionDate > currentTransaction.TransactionDate)
                            {
                                if (!pendingUsers.Contains(user))
                                {
                                    pendingUsers.Add(user);
                                }
                            }
                        }
                    }
                }
                else
                {
                    var user = _context.Users.Where(x => x.Id == timesheet.UserId).FirstOrDefault();
                    if (!pendingUsers.Contains(user))
                    {
                        pendingUsers.Add(user);
                    }
                }
            }

            return pendingUsers;
        }

        public List<User> GetTeamPendingEmployees(long managerId)
        {
            var timeSheets = (from user in _context.Users
                              join timesheet in _context.TimeSheets
                              on user.Id equals timesheet.UserId
                              where (timesheet.StatusId == TimeSheetStatus.Submited
                              || timesheet.StatusId == TimeSheetStatus.Resubmited)
                              && user.ManagerId == managerId
                              orderby timesheet.FromDate ascending
                              select timesheet).Distinct().ToList();

            List<User> pendingUsers = new List<User>();

            foreach (var timesheet in timeSheets)
            {
                var currentTransaction = _context.TimeSheetApprovals.Where(x => x.TimeSheetId == timesheet.Id && x.ManagerId == managerId)
                    .OrderByDescending(x => x.TransactionDate).FirstOrDefault();

                if (currentTransaction != null)
                {
                    if (currentTransaction.Status == TimeSheetStatus.Rejected
                        && timesheet.StatusId == TimeSheetStatus.Resubmited)
                    {
                        var user = _context.Users.Where(x => x.Id == timesheet.UserId).FirstOrDefault();
                        var userResubmit = _context.TimeSheetApprovals
                            .Where(x => x.TimeSheetId == currentTransaction.TimeSheetId && x.UserId == user.Id)
                            .OrderByDescending(x => x.TransactionDate).FirstOrDefault();
                        if (userResubmit != null)
                        {
                            if (userResubmit.TransactionDate > currentTransaction.TransactionDate)
                            {
                                if (!pendingUsers.Contains(user))
                                {
                                    pendingUsers.Add(user);
                                }
                            }
                        }
                    }
                }
                else
                {
                    var user = _context.Users.Where(x => x.Id == timesheet.UserId).FirstOrDefault();
                    if (!pendingUsers.Contains(user))
                    {
                        pendingUsers.Add(user);
                    }
                }
            }

            return pendingUsers;
        }

        public List<TimeSheet> GetEmployeeTimeSheet(long managerId, long employeeId)
        {
            var timeSheets = (from user in _context.Users
                              join timesheet in _context.TimeSheets
                              on user.Id equals timesheet.UserId
                              join timesheetDetails in _context.TimeSheetDetails
                              on timesheet.Id equals timesheetDetails.TimeSheetId
                              where (timesheet.StatusId == TimeSheetStatus.Submited
                              || timesheet.StatusId == TimeSheetStatus.Resubmited)
                              && user.ManagerId == managerId 
                              && user.Id == employeeId
                              orderby timesheet.FromDate ascending
                              select timesheet).Distinct().ToList();

            List<TimeSheet> pendingTimeSheets = new List<TimeSheet>();

            foreach (var timesheet in timeSheets)
            {
                var currentTransaction = _context.TimeSheetApprovals.Where(x => x.TimeSheetId == timesheet.Id && x.ManagerId == managerId)
                    .OrderByDescending(x => x.TransactionDate).FirstOrDefault();

                if (currentTransaction != null)
                {
                    if (currentTransaction.Status == TimeSheetStatus.Rejected
                      && timesheet.StatusId == TimeSheetStatus.Resubmited)
                    {
                        var user = _context.Users.Where(x => x.Id == timesheet.UserId).FirstOrDefault();
                        var userResubmit = _context.TimeSheetApprovals
                            .Where(x => x.TimeSheetId == currentTransaction.TimeSheetId && x.UserId == user.Id)
                            .OrderByDescending(x => x.TransactionDate).FirstOrDefault();
                        if (userResubmit != null)
                        {
                            if (userResubmit.TransactionDate > currentTransaction.TransactionDate)
                            {
                                if (!pendingTimeSheets.Contains(timesheet))
                                {
                                    pendingTimeSheets.Add(timesheet);
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (!pendingTimeSheets.Contains(timesheet))
                    {
                        pendingTimeSheets.Add(timesheet);
                    }
                }
            }

            return pendingTimeSheets;
        }

        public List<TimeSheet> GetEmployeeTimeSheetByProject(long projectId, long employeeId)
        {
            var timeSheets = (from user in _context.Users
                              join timesheet in _context.TimeSheets
                              on user.Id equals timesheet.UserId
                              join timesheetDetails in _context.TimeSheetDetails
                              on timesheet.Id equals timesheetDetails.TimeSheetId
                              join activity in _context.Activities
                              on timesheetDetails.ActivityId equals activity.Id
                              join project in _context.Projects
                              on activity.ProjectId equals project.Id
                              where project.Id == projectId && (timesheet.StatusId == TimeSheetStatus.Submited
                              || timesheet.StatusId == TimeSheetStatus.Resubmited)
                              orderby timesheet.FromDate ascending
                              select timesheet).Distinct().ToList();

            List<TimeSheet> pendingTimeSheets = new List<TimeSheet>();

            foreach (var timesheet in timeSheets)
            {
                var managerId = _context.Projects.Where(x => x.Id == projectId).First().ManagerId;
                var currentTransaction = _context.TimeSheetApprovals.Where(x => x.TimeSheetId == timesheet.Id && x.ManagerId == managerId)
                    .OrderByDescending(x => x.TransactionDate).FirstOrDefault();

                if (currentTransaction != null)
                {
                    if (currentTransaction.Status == TimeSheetStatus.Rejected
                      && timesheet.StatusId == TimeSheetStatus.Resubmited)
                    {
                        var user = _context.Users.Where(x => x.Id == timesheet.UserId).FirstOrDefault();
                        var userResubmit = _context.TimeSheetApprovals
                            .Where(x => x.TimeSheetId == currentTransaction.TimeSheetId && x.UserId == user.Id)
                            .OrderByDescending(x => x.TransactionDate).FirstOrDefault();
                        if (userResubmit != null)
                        {
                            if (userResubmit.TransactionDate > currentTransaction.TransactionDate)
                            {
                                if (!pendingTimeSheets.Contains(timesheet))
                                {
                                    pendingTimeSheets.Add(timesheet);
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (!pendingTimeSheets.Contains(timesheet))
                    {
                        pendingTimeSheets.Add(timesheet);
                    }
                }
            }

            return pendingTimeSheets;
        }

        public bool ApproveTimeSheetProjectManager(long managerId, long timesheetId, bool IsApproved, string comments)
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
                            where timesheet.Id == timesheetId && project.ManagerId == managerId
                            select manager)
                            .Union(from user in _context.Users
                                  join timesheet in _context.TimeSheets
                                  on user.Id equals timesheet.UserId
                                  join manager in _context.Users
                                  on user.ManagerId equals manager.Id
                                  where timesheet.Id == timesheetId
                                  select manager).ToList();

            var timeSheet = _context.TimeSheets.Where(x => x.Id == timesheetId).FirstOrDefault();
            var status = timeSheet.StatusId;

            var isAllApproved = false;

            managers.Remove(managers.Where(x => x.ManagerId == managerId).FirstOrDefault());
            foreach (var manager in managers)
            {
                var managerApproval = _context.TimeSheetApprovals.Where(x => x.ManagerId == manager.Id && x.TimeSheetId == timesheetId)
                    .OrderByDescending(x => x.TransactionDate).FirstOrDefault();

                if (managerApproval != null)
                {
                    if (managerApproval.Status == TimeSheetStatus.Approved)
                    {
                        isAllApproved = true;
                    }
                    else
                    {
                        isAllApproved = false;
                    }
                }
                else
                {
                    isAllApproved = false;
                }
            }

            if (isAllApproved && IsApproved)
            {
                var currentTimesheet = _context.TimeSheets.Where(x => x.Id == timesheetId).FirstOrDefault();
                currentTimesheet.StatusId = TimeSheetStatus.Approved;

                _context.TimeSheets.Update(currentTimesheet);

                var activitiesDetails = (from user in _context.Users
                                         join timesheet in _context.TimeSheets
                                         on user.Id equals timesheet.UserId
                                         join timesheetDetails in _context.TimeSheetDetails
                                         on timesheet.Id equals timesheetDetails.TimeSheetId
                                         join activity in _context.Activities
                                         on timesheetDetails.ActivityId equals activity.Id
                                         where timesheetDetails.TimeSheetId == timesheetId
                                         select timesheetDetails).Distinct().ToList();


                foreach (var activityDetail in activitiesDetails)
                {
                    var activity = _context.Activities.Where(x => x.Id == activityDetail.ActivityId).FirstOrDefault();

                    if (activity.LoggedHours == null)
                    {
                        activity.LoggedHours = 0;
                    }

                    activity.LoggedHours += activityDetail.Sun + activityDetail.Mon + activityDetail.Tu + activityDetail.We + activityDetail.Th;

                    _context.Activities.Update(activity);
                }
            }

            var projectManager = managers.Where(x => x.ManagerId == managerId).FirstOrDefault();
            var managerName = projectManager != null ? projectManager.FirstName + " " + projectManager.LastName : "";

            var timesheetApproval = new TimeSheetApproval() {
                Comments = comments,
                ManagerId = managerId,
                ManagerName = managerName,
                TimeSheetId = timesheetId,
                Status = IsApproved ? TimeSheetStatus.Approved : TimeSheetStatus.Rejected,
                TransactionDate = DateTime.Now
            };

            _context.TimeSheetApprovals.Add(timesheetApproval);
            _context.SaveChanges();

            return true;
        }

        public bool ApproveTimeSheetManager(long managerId, long timesheetId, bool IsApproved, string comments)
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
                            where timesheet.Id == timesheetId
                            select manager)
                            .Union(from user in _context.Users
                                   join timesheet in _context.TimeSheets
                                   on user.Id equals timesheet.UserId
                                   join manager in _context.Users
                                   on user.ManagerId equals manager.Id
                                   where timesheet.Id == timesheetId
                                   select manager).ToList();

            var timeSheet = _context.TimeSheets.Where(x => x.Id == timesheetId).FirstOrDefault();
            var status = timeSheet.StatusId;

            var isAllApproved = false;

            managers.Remove(managers.Where(x => x.ManagerId == managerId).FirstOrDefault());
            foreach (var manager in managers)
            {
                var managerApproval = _context.TimeSheetApprovals.Where(x => x.ManagerId == manager.Id && x.TimeSheetId == timesheetId)
                    .OrderByDescending(x => x.TransactionDate).FirstOrDefault();

                if (managerApproval != null)
                {
                    if (managerApproval.Status == TimeSheetStatus.Approved)
                    {
                        isAllApproved = true;
                    }
                    else
                    {
                        isAllApproved = false;
                    }
                }
                else
                {
                    isAllApproved = false;
                }
            }

            if (isAllApproved && IsApproved)
            {
                var currentTimesheet = _context.TimeSheets.Where(x => x.Id == timesheetId).FirstOrDefault();
                currentTimesheet.StatusId = TimeSheetStatus.Approved;

                _context.TimeSheets.Update(currentTimesheet);

                var activitiesDetails = (from user in _context.Users
                                         join timesheet in _context.TimeSheets
                                         on user.Id equals timesheet.UserId
                                         join timesheetDetails in _context.TimeSheetDetails
                                         on timesheet.Id equals timesheetDetails.TimeSheetId
                                         join activity in _context.Activities
                                         on timesheetDetails.ActivityId equals activity.Id
                                         where timesheetDetails.TimeSheetId == timesheetId
                                         select timesheetDetails).Distinct().ToList();


                foreach (var activityDetail in activitiesDetails)
                {
                    var activity = _context.Activities.Where(x => x.Id == activityDetail.ActivityId).FirstOrDefault();

                    if (activity.LoggedHours == null)
                    {
                        activity.LoggedHours = 0;
                    }

                    activity.LoggedHours += activityDetail.Sun + activityDetail.Mon + activityDetail.Tu + activityDetail.We + activityDetail.Th;

                    _context.Activities.Update(activity);
                }
            }

            var projectManager = managers.Where(x => x.ManagerId == managerId).FirstOrDefault();
            var managerName = projectManager != null ? projectManager.FirstName + " " + projectManager.LastName : "";

            var timesheetApproval = new TimeSheetApproval()
            {
                Comments = comments,
                ManagerId = managerId,
                ManagerName = managerName,
                TimeSheetId = timesheetId,
                Status = IsApproved ? TimeSheetStatus.Approved : TimeSheetStatus.Rejected,
                TransactionDate = DateTime.Now
            };

            _context.TimeSheetApprovals.Add(timesheetApproval);
            _context.SaveChanges();

            return true;
        }

        public TimeSheetStatus GetStatus(long timesheetId)
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
                            where timesheet.Id == timesheetId
                            select manager)
                           .Union(from user in _context.Users
                                  join timesheet in _context.TimeSheets
                                   on user.Id equals timesheet.UserId
                                  join manager in _context.Users
                                   on user.ManagerId equals manager.Id
                                  where timesheet.Id == timesheetId
                                  select manager).ToList();

            var timeSheet = _context.TimeSheets.Where(x => x.Id == timesheetId).FirstOrDefault();
            var status = timeSheet.StatusId;

            var isRejected = false;

            foreach (var manager in managers)
            {
                var managerApproval = _context.TimeSheetApprovals.Where(x => x.ManagerId == manager.Id && x.TimeSheetId == timesheetId)
                    .OrderByDescending(x => x.TransactionDate).FirstOrDefault();

                var user = _context.Users.Where(x => x.Id == timeSheet.UserId).FirstOrDefault();
                if (managerApproval != null)
                {
                    var userResubmit = _context.TimeSheetApprovals
                        .Where(x => x.TimeSheetId == managerApproval.TimeSheetId && x.UserId == user.Id)
                        .OrderByDescending(x=>x.TransactionDate).FirstOrDefault();

                    if (userResubmit != null)
                    {
                        if (managerApproval != null)
                        {
                            if (managerApproval.Status == TimeSheetStatus.Rejected)
                            {
                                if (managerApproval.TransactionDate > userResubmit.TransactionDate)
                                {
                                    isRejected = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (managerApproval != null)
                        {
                            if (managerApproval.Status == TimeSheetStatus.Rejected)
                            {
                                isRejected = true;
                            }
                        }
                    }
                }
            }
            var isAllApprovd = false;

            if (!isRejected)
            {
                foreach (var manager in managers)
                {
                    var managerApproval = _context.TimeSheetApprovals.Where(x => x.ManagerId == manager.Id && x.TimeSheetId == timesheetId)
                         .OrderByDescending(x => x.TransactionDate).FirstOrDefault();

                    if (managerApproval != null)
                    {
                        if (managerApproval.Status == TimeSheetStatus.Approved)
                        {
                            isAllApprovd = true;
                        }
                        else
                        {
                            isAllApprovd = false;
                            break;
                        }
                    }
                    else
                    {
                        isAllApprovd = false;
                        break;
                    }
                }
            }

            var result = isRejected ? TimeSheetStatus.Rejected : isAllApprovd ? TimeSheetStatus.Approved : status;
            return result;
        }

        public bool AddTimeSheet(TimeSheet sheet)
        {
            var existSheet = _context.TimeSheets.Where(x => x.FromDate == sheet.FromDate && x.UserId == sheet.UserId).FirstOrDefault();
            if (existSheet != null)
                return false;

            sheet.StatusId = TimeSheetStatus.Draft;
            sheet.ToDate = sheet.FromDate.AddDays(6);

            _context.TimeSheets.Add(sheet);
            _context.SaveChanges();

            return true;
        }
    }
}