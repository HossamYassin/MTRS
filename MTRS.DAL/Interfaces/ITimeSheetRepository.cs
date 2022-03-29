using MTRS.Core.DTOs;
using MTRS.Core.Entities;
using MTRS.Core.Enums;
using MTRS.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTRS.DAL.Interfaces
{
    public interface ITimeSheetRepository : IGenericRepository<TimeSheet, long>
    {
        public List<TimeSheet> GetNewByUserId(long userId);
        public TimeSheet GetByDate(long userId, DateTime startDate);
        public List<TimeSheet> GetHistory(long userId);
        List<TimeSheet> GetNewSubmitedByProjectAndUser(long userId, long projectId);
        List<TimeSheet> GetProjectHistoryByUser(long userId, long projectId);
        List<User> GetPendingEmployeesByProject(long projectId);
        List<TimeSheet> GetEmployeeTimeSheetByProject(long project, long employeeId);
        bool ApproveTimeSheetProjectManager(long managerId, long timesheetId, bool IsApproved, string comments);
        TimeSheetStatus GetStatus(long timesheetId);
        List<User> GetEmployeesByProject(long projectId);
        List<User> GetTeamPendingEmployees(long managerId);
        List<TimeSheet> GetEmployeeTimeSheet(long managerId, long employeeId);
        bool ApproveTimeSheetManager(long managerId, long timesheetId, bool IsApproved, string comments);
        List<TimeSheet> GetPendingTimeSheets(long employeeId);
        bool AddTimeSheet(TimeSheet sheet);
        TimeSheet GetTimeSheetByDate(long userId, DateTime startDate);
    }
}
