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
    public class UserRepository : GenericRepository<User, long>, IUserRepository
    {
        public UserRepository(MTRSDBContext context) : base(context)
        {
        }

        public List<UserListItem> GetList()
        {
            var query = from user in _context.Users
                        select new UserListItem() {
                            Name = user.FirstName + " " + user.LastName,
                            DepartmentName = user.Department != null ? user.Department.Name : "",
                            PositionName = user.Position != null ? user.Position.Name : "",
                            Id = user.Id
                        };

            return query.ToList();
        }

        public List<User> GetMyEmployees(long managerId, int level)
        {
            var employees = new List<User>();

            var users = _context.Users.Where(x => x.ManagerId == managerId).Include(x=>x.Position).ToList();
            foreach (var user in users)
            {
                employees.Add(user);

                if (level <= 5)
                {
                    foreach (var subUser in GetMyEmployees(user.Id, level + 1))
                    {
                        employees.Add(subUser);
                    }
                }
            }

            return employees;
        }

        public UserDashboardDto GetDashboard(long userId)
        {
            var timeSheets = (from user in _context.Users
                              join timesheet in _context.TimeSheets
                              on user.Id equals timesheet.UserId
                              join timesheetDetails in _context.TimeSheetDetails
                              on timesheet.Id equals timesheetDetails.TimeSheetId
                              where (timesheet.StatusId == TimeSheetStatus.Submited
                              || timesheet.StatusId == TimeSheetStatus.Resubmited)
                              && user.Id == userId
                              orderby timesheet.FromDate ascending
                              select timesheet).Distinct().ToList();

            List<TimeSheet> pendingTimeSheets = new List<TimeSheet>();

            foreach (var timesheet in timeSheets)
            {
                var currentTransaction = _context.TimeSheetApprovals.Where(x => x.TimeSheetId == timesheet.Id)
                    .OrderByDescending(x => x.TransactionDate).FirstOrDefault();

                if (currentTransaction != null)
                {
                    if(currentTransaction.Status != TimeSheetStatus.Approved) 
                    {
                        if (!pendingTimeSheets.Contains(timesheet))
                        {
                            pendingTimeSheets.Add(timesheet);
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

            var pendingTimeSheetsCount = pendingTimeSheets.Count();


            var PendingCount = (from user in _context.Users
                                         join timesheet in _context.TimeSheets
                                         on user.Id equals timesheet.UserId
                                         where user.Id == userId && timesheet.StatusId == Core.Enums.TimeSheetStatus.Draft
                                         orderby timesheet.FromDate ascending
                                         select timesheet).Distinct().Count();

            var myProjectsCount = (from activity in _context.Activities
                        join project in _context.Projects
                        on activity.ProjectId equals project.Id
                        join userActivity in _context.ActivityUsers
                        on activity.Id equals userActivity.ActivityId
                        where userActivity.IsActive == true && userActivity.UserId == userId
                        && (DateTime.Now.Date >= activity.StartDate.Value.Date && DateTime.Now.Date <= activity.EndDate.Value.Date)
                        select project).Distinct().Count();

            var dashboard = new UserDashboardDto();

            dashboard.PendingApprovals = pendingTimeSheetsCount;
            dashboard.PendingTimesheets = PendingCount;
            dashboard.MyProjects = myProjectsCount;

            return dashboard;
        }

        public TeamDashboardDto GetTeamDashboard(long managerId)
        {
            var dashboard = new TeamDashboardDto();

            dashboard.EmployeesCount = GetMyEmployees(managerId, 5).Count();

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

            dashboard.PendingTimeSheetCount = pendingUsers.Count();


            var myProjectsCount = (from activity in _context.Activities
                                   join project in _context.Projects
                                   on activity.ProjectId equals project.Id
                                   join userActivity in _context.ActivityUsers
                                   on activity.Id equals userActivity.ActivityId
                                   join user in _context.Users
                                   on userActivity.UserId equals user.Id
                                   where userActivity.IsActive == true && user.ManagerId == managerId 
                                   && (DateTime.Now.Date >= activity.StartDate.Value.Date && DateTime.Now.Date <= activity.EndDate.Value.Date)
                                   select project).Distinct().Count();
            
            dashboard.TeamProjectsCount = myProjectsCount;

            return dashboard;
        }
    }
}