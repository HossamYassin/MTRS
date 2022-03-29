using MTRS.Core.DTOs;
using MTRS.Core.Entities;
using MTRS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace MTRS.Services.Interfaces
{
    public interface ITimeSheetDetailsService
    {
        List<TimeSheetDetailsDto> GetAll();
        TimeSheetDetailsDto GetById(long id);
        IList<TimeSheetDetailsDto> Get(Expression<Func<TimeSheetDetails, bool>> expression, string sortColumn, bool isSortAscending, int page, int pageSize);
        void Update(TimeSheetDetailsDto timeSheetDetailsDto);
        void Remove(TimeSheetDetailsDto timeSheetDetailsDto);
        void Add(TimeSheetDetailsDto timeSheetDetailsDto);
        int Count();
        List<TimeSheetDetailsDto> GetByTimeSheetId(long timesheetId);
        bool SaveSheetActivities(List<TimeSheetDetailsDto> timeSheetDetails, TimeSheetStatus sheetStatus);
    }
}
