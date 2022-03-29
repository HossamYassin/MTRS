using Microsoft.EntityFrameworkCore;
using MTRS.Core.DTOs;
using MTRS.Core.Entities;
using MTRS.DAL.DbContexts;
using MTRS.DAL.Interfaces;
using MTRS.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace MTRS.DAL.Interfaces
{
    public class ProjectRepository : GenericRepository<Project, long>, IProjectRepository
    {
        public ProjectRepository(MTRSDBContext context) : base(context)
        {
        }

        public List<Project> GetList()
        {
            var query = _context.Projects.AsQueryable().
                Include(x => x.Manager).Include(y => y.Customer);

            return query.ToList();
        }

        public List<Project> GetUserProjectWithActivity(long userId)
        {
            var query = from activity in _context.Activities
                        join project in _context.Projects
                        on activity.ProjectId equals project.Id
                        join userActivity in _context.ActivityUsers
                        on activity.Id equals userActivity.ActivityId
                        where userActivity.IsActive == true && project.IsCompleted == false && userActivity.UserId == userId
                        && (DateTime.Now.Date >= activity.StartDate.Value.Date && DateTime.Now.Date <= activity.EndDate.Value.Date) 
                        select project;

            return query.Distinct().ToList();
        }

        public List<Project> GetUserProject(long userId)
        {
            var query = from activity in _context.Activities
                        join project in _context.Projects
                        on activity.ProjectId equals project.Id
                        join userActivity in _context.ActivityUsers
                        on activity.Id equals userActivity.ActivityId
                        where userActivity.IsActive == true && userActivity.UserId == userId
                        select project;

            return query.Distinct().ToList();
        }

        public List<Project> GetProjectManagerProjects(long managerId)
        {
            var projects = _context.Projects.Where(x => x.ManagerId == managerId).ToList();
            return projects;
        }

        public Project GetProjectWithDetails(long projectId)
        {
            var project = _context.Projects.Where(x => x.Id == projectId).Include(x => x.Customer).FirstOrDefault();
            return project;
        }

        public List<Project> GetActiveProjects(long managerId)
        {
            var projects = (from project in _context.Projects
                           join activity in _context.Activities
                           on project.Id equals activity.Id
                           where activity.EndDate.Value >= DateTime.Now && 
                           project.IsCompleted == false
                           && project.ManagerId == managerId
                           select project).Include(x=>x.Activities).ToList();

            return projects;
        }

        public List<Project> GetInActiveProjects(long managerId)
        {
            var projects = (from project in _context.Projects
                            join activity in _context.Activities
                            on project.Id equals activity.Id
                            where activity.EndDate.Value <= DateTime.Now || project.IsCompleted == true
                            && project.ManagerId == managerId
                            select project).Include(x => x.Activities).ToList();

            return projects;
        }

        public ProjectDashboard GetProjectDashboard(long projectId)
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
                              where project.Id == projectId && timesheet.StatusId == Core.Enums.TimeSheetStatus.Submited
                              || timesheet.StatusId == Core.Enums.TimeSheetStatus.Resubmited
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
                    if (currentTransaction.Status == Core.Enums.TimeSheetStatus.Rejected
                       && timesheet.StatusId == Core.Enums.TimeSheetStatus.Resubmited)
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
            var pendingTimesheetCount = pendingTimeSheets.Count();

            var employeeCount = (from activity in _context.Activities
                                 join project in _context.Projects
                                 on activity.ProjectId equals project.Id
                                 join userActivity in _context.ActivityUsers
                                 on activity.Id equals userActivity.ActivityId
                                 join user in _context.Users
                                 on userActivity.UserId equals user.Id
                                 where userActivity.IsActive == true &&
                                 project.Id == projectId
                                 select user).Distinct().Count();

            var dashboard = new ProjectDashboard();

            dashboard.EmployeesCount = employeeCount;
            dashboard.PendingTimeSheetCount = pendingTimesheetCount;

            return dashboard;
        }

        public int GetAcctuleCostByGrad(string grad, DateTime startDate, DateTime endDate, long projectId)
        {
            var timesheetEntries = (from activity in _context.Activities
                                 join project in _context.Projects
                                 on activity.ProjectId equals project.Id
                                 join userActivity in _context.ActivityUsers
                                 on activity.Id equals userActivity.ActivityId
                                 join user in _context.Users
                                 on userActivity.UserId equals user.Id
                                 join grade in _context.Grades
                                 on user.GradeId equals grade.Id
                                 join timesheet in _context.TimeSheets
                                 on user.Id equals timesheet.UserId
                                 join timesheetDetails in _context.TimeSheetDetails
                                 on timesheet.Id equals timesheetDetails.TimeSheetId
                                 where userActivity.IsActive == true &&
                                 project.Id == projectId && grade.Name == grad
                                 && (timesheet.FromDate >= startDate && timesheet.ToDate <= endDate)
                                 select timesheetDetails).ToList();

            int? acctualCost = 0;
            foreach (var activityDetail in timesheetEntries)
            {
                acctualCost += activityDetail.Sun + activityDetail.Mon + activityDetail.Tu + activityDetail.We + activityDetail.Th;
            }

            return acctualCost.Value;
        }

    }
}