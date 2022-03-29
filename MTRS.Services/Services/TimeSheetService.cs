using AutoMapper;
using MTRS.Core.DTOs;
using MTRS.Core.Entities;
using MTRS.Core.Enums;
using MTRS.DAL.Interfaces;
using MTRS.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace MTRS.Services
{
    public class TimeSheetService : ITimeSheetService
    {
        private readonly ITimeSheetRepository _timesheetRepository;
        private readonly IMapper _mapper;

        public TimeSheetService(ITimeSheetRepository timeSheetRepository, IMapper mapper)
        {
            _mapper = mapper;
            _timesheetRepository = timeSheetRepository;
        }

        public List<TimeSheetDto> GetAll()
        {
            return _mapper.Map<List<TimeSheetDto>>(_timesheetRepository.GetAll());
        }

        public TimeSheetDto GetById(long id)
        {
            return _mapper.Map<TimeSheetDto>(_timesheetRepository.GetById(id));
        }

        public void Add(TimeSheetDto timeSheetDto)
        {
            var entity = _mapper.Map<TimeSheet>(timeSheetDto);
            _timesheetRepository.Add(entity);
        }

        public IList<TimeSheetDto> Get(Expression<Func<TimeSheet, bool>> expression, string sortColumn, bool isSortAscending, int page, int pageSize)
        {
            return _mapper.Map<List<TimeSheetDto>>(_timesheetRepository.Find(expression, sortColumn, isSortAscending, page, pageSize).ToList());
        }

        public void Remove(TimeSheetDto timeSheetDto)
        {
            var entity = _mapper.Map<TimeSheet>(timeSheetDto);
            _timesheetRepository.Remove(entity);
        }

        public void Update(TimeSheetDto timeSheetDto)
        {
            var timeSheet = _timesheetRepository.GetById(timeSheetDto.Id);
            _mapper.Map(timeSheetDto, timeSheet);

            _timesheetRepository.Update();
        }

        public int Count()
        {
            return _timesheetRepository.Count();
        }

        public List<TimeSheetDto> GetNewByUserId(long userId)
        {
            return _mapper.Map<List<TimeSheetDto>>(_timesheetRepository.GetNewByUserId(userId));
        }

        public TimeSheetDto GetByDate(long userId, DateTime startDate)
        {
            return _mapper.Map<TimeSheetDto>(_timesheetRepository.GetByDate(userId, startDate));
        }

        public List<TimeSheetDto> GetHistory(long userId)
        {
            return _mapper.Map<List<TimeSheetDto>>(_timesheetRepository.GetHistory(userId));
        }

        public List<TimeSheetDto> GetNewSubmitedByProjectAndUser(long userId, long projectId)
        {
            return _mapper.Map<List<TimeSheetDto>>(_timesheetRepository.GetNewSubmitedByProjectAndUser(userId, projectId));
        }

        public List<TimeSheetDto> GetProjectHistoryByUser(long userId, long projectId)
        {
            return _mapper.Map<List<TimeSheetDto>>(_timesheetRepository.GetProjectHistoryByUser(userId, projectId));
        }

        public List<UserDto> GetPendingEmployeesByProject(long projectId)
        {
            return _mapper.Map<List<UserDto>>(_timesheetRepository.GetPendingEmployeesByProject(projectId));
        }

        public List<UserDto> GetEmployeesByProject(long projectId)
        {
            return _mapper.Map<List<UserDto>>(_timesheetRepository.GetEmployeesByProject(projectId));
        }

        public List<TimeSheetDto> GetEmployeeTimeSheetByProject(long projectId, long employeeId)
        {
            return _mapper.Map<List<TimeSheetDto>>(_timesheetRepository.GetEmployeeTimeSheetByProject(projectId, employeeId));
        }

        public bool ApproveTimeSheetProjectManager(long managerId, long timesheetId, bool IsApproved, string comments)
        {
            return _timesheetRepository.ApproveTimeSheetProjectManager(managerId, timesheetId, IsApproved, comments);
        }

        public TimeSheetStatus GetStatus(long timesheetId)
        {
            return _timesheetRepository.GetStatus(timesheetId);
        }

        public List<UserDto> GetTeamPendingEmployees(long managerId)
        {
            return _mapper.Map<List<UserDto>>(_timesheetRepository.GetTeamPendingEmployees(managerId));
        }

        public List<TimeSheetDto> GetEmployeeTimeSheet(long managerId, long employeeId)
        {
            return _mapper.Map<List<TimeSheetDto>>(_timesheetRepository.GetEmployeeTimeSheet(managerId, employeeId));
        }

        public bool ApproveTimeSheetManager(long managerId, long timesheetId, bool IsApproved, string comments)
        {
            return _timesheetRepository.ApproveTimeSheetManager(managerId, timesheetId, IsApproved, comments);
        }

        public List<TimeSheetDto> GetPendingTimeSheets(long employeeId)
        {
            return _mapper.Map<List<TimeSheetDto>>(_timesheetRepository.GetPendingTimeSheets(employeeId));
        }

        public bool AddTimeSheet(TimeSheetDto sheet)
        {
            return _timesheetRepository.AddTimeSheet(_mapper.Map<TimeSheet>(sheet));
        }

        public TimeSheetDto GetTimeSheetByDate(long userId, DateTime startDate)
        {
            return _mapper.Map<TimeSheetDto>(_timesheetRepository.GetTimeSheetByDate(userId, startDate));
        }
    }
}
