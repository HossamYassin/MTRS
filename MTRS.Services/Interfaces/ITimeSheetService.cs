using MTRS.Core.DTOs;
using MTRS.Core.Entities;
using MTRS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace MTRS.Services.Interfaces
{
    public interface ITimeSheetService
    {
        List<TimeSheetDto> GetAll();
        TimeSheetDto GetById(long id);
        IList<TimeSheetDto> Get(Expression<Func<TimeSheet, bool>> expression, string sortColumn, bool isSortAscending, int page, int pageSize);
        void Update(TimeSheetDto timeSheetDto);
        void Remove(TimeSheetDto timeSheetDto);
        void Add(TimeSheetDto timeSheetDto);
        int Count();
        List<TimeSheetDto> GetNewByUserId(long userId);
        public TimeSheetDto GetByDate(long userId, DateTime startDate);
        public List<TimeSheetDto> GetHistory(long userId);
        List<TimeSheetDto> GetNewSubmitedByProjectAndUser(long userId, long projectId);
        List<TimeSheetDto> GetProjectHistoryByUser(long userId, long projectId);
        List<UserDto> GetPendingEmployeesByProject(long projectId);
        List<TimeSheetDto> GetEmployeeTimeSheetByProject(long projectId, long employeeId);
        bool ApproveTimeSheetProjectManager(long managerId, long timesheetId, bool IsApproved, string comments);
        TimeSheetStatus GetStatus(long timesheetId);
        List<UserDto> GetEmployeesByProject(long projectId);
        List<UserDto> GetTeamPendingEmployees(long managerId);
        List<TimeSheetDto> GetEmployeeTimeSheet(long managerId, long employeeId);
        bool ApproveTimeSheetManager(long managerId, long timesheetId, bool IsApproved, string comments);
        List<TimeSheetDto> GetPendingTimeSheets(long employeeId);
        bool AddTimeSheet(TimeSheetDto sheet);
        TimeSheetDto GetTimeSheetByDate(long userId, DateTime startDate);
    }
}
