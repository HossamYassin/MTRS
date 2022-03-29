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
    public class TimeSheetApprovalService : ITimeSheetApprovalService
    {
        private readonly ITimeSheetApprovalRepository _timesheetapprovalRepository;
        private readonly IMapper _mapper;

        public TimeSheetApprovalService(ITimeSheetApprovalRepository timeSheetApprovalRepository, IMapper mapper)
        {
            _mapper = mapper;
            _timesheetapprovalRepository = timeSheetApprovalRepository;
        }

        public List<TimeSheetApprovalDto> GetAll()
        {
            return _mapper.Map<List<TimeSheetApprovalDto>>(_timesheetapprovalRepository.GetAll());
        }

        public TimeSheetApprovalDto GetById(long id)
        {
            return _mapper.Map<TimeSheetApprovalDto>(_timesheetapprovalRepository.GetById(id));
        }

        public void Add(TimeSheetApprovalDto timeSheetApprovalDto)
        {
            var entity = _mapper.Map<TimeSheetApproval>(timeSheetApprovalDto);
            _timesheetapprovalRepository.Add(entity);
        }

        public IList<TimeSheetApprovalDto> Get(Expression<Func<TimeSheetApproval, bool>> expression, string sortColumn, bool isSortAscending, int page, int pageSize)
        {
            return _mapper.Map<List<TimeSheetApprovalDto>>(_timesheetapprovalRepository.Find(expression, sortColumn, isSortAscending, page, pageSize).ToList());
        }

        public void Remove(TimeSheetApprovalDto timeSheetApprovalDto)
        {
            var entity = _mapper.Map<TimeSheetApproval>(timeSheetApprovalDto);
            _timesheetapprovalRepository.Remove(entity);
        }

        public void Update(TimeSheetApprovalDto timeSheetApprovalDto)
        {
            var timeSheet = _timesheetapprovalRepository.GetById(timeSheetApprovalDto.Id);
            _mapper.Map(timeSheetApprovalDto, timeSheet);

            _timesheetapprovalRepository.Update();
        }

        public int Count()
        {
            return _timesheetapprovalRepository.Count();
        }

        public List<TimeSheetApprovalDto> GetByTimeSheetId(long timesheetId) 
        {
            return _mapper.Map<List<TimeSheetApprovalDto>>(_timesheetapprovalRepository.GetByTimeSheetId(timesheetId));
        }
    }
}
