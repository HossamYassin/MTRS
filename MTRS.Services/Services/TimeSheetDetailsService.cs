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
    public class TimeSheetDetailsService : ITimeSheetDetailsService
    {
        private readonly ITimeSheetDetailsRepository _timesheetDetailsRepository;
        private readonly IMapper _mapper;

        public TimeSheetDetailsService(ITimeSheetDetailsRepository timeSheetDetailsRepository, IMapper mapper)
        {
            _mapper = mapper;
            _timesheetDetailsRepository = timeSheetDetailsRepository;
        }

        public List<TimeSheetDetailsDto> GetAll()
        {
            return _mapper.Map<List<TimeSheetDetailsDto>>(_timesheetDetailsRepository.GetAll());
        }

        public TimeSheetDetailsDto GetById(long id)
        {
            return _mapper.Map<TimeSheetDetailsDto>(_timesheetDetailsRepository.GetById(id));
        }

        public void Add(TimeSheetDetailsDto timeSheetDetailsDto)
        {
            var entity = _mapper.Map<TimeSheetDetails>(timeSheetDetailsDto);
            _timesheetDetailsRepository.Add(entity);
        }

        public IList<TimeSheetDetailsDto> Get(Expression<Func<TimeSheetDetails, bool>> expression, string sortColumn, bool isSortAscending, int page, int pageSize)
        {
            return _mapper.Map<List<TimeSheetDetailsDto>>(_timesheetDetailsRepository.Find(expression, sortColumn, isSortAscending, page, pageSize).ToList());
        }

        public void Remove(TimeSheetDetailsDto timeSheetDetailsDto)
        {
            var entity = _mapper.Map<TimeSheetDetails>(timeSheetDetailsDto);
            _timesheetDetailsRepository.Remove(entity);
        }

        public void Update(TimeSheetDetailsDto timeSheetDetailsDto)
        {
            var timeSheet = _timesheetDetailsRepository.GetById(timeSheetDetailsDto.Id);
            _mapper.Map(timeSheetDetailsDto, timeSheet);

            _timesheetDetailsRepository.Update();
        }

        public int Count()
        {
            return _timesheetDetailsRepository.Count();
        }

        public List<TimeSheetDetailsDto> GetByTimeSheetId(long timesheetId) 
        {
            return _mapper.Map<List<TimeSheetDetailsDto>>(_timesheetDetailsRepository.GetByTimeSheetId(timesheetId));
        }

        public bool SaveSheetActivities(List<TimeSheetDetailsDto> timeSheetDetails, TimeSheetStatus sheetStatus)
        {
            var sheetEntries = _mapper.Map<List<TimeSheetDetails>>(timeSheetDetails);
            _timesheetDetailsRepository.SaveSheetActivities(sheetEntries, sheetStatus);
            return true;
        }
    }
}
