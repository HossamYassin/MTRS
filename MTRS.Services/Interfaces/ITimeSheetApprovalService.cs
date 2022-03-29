using MTRS.Core.DTOs;
using MTRS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace MTRS.Services.Interfaces
{
    public interface ITimeSheetApprovalService
    {
        List<TimeSheetApprovalDto> GetAll();
        TimeSheetApprovalDto GetById(long id);
        IList<TimeSheetApprovalDto> Get(Expression<Func<TimeSheetApproval, bool>> expression, string sortColumn, bool isSortAscending, int page, int pageSize);
        void Update(TimeSheetApprovalDto TimeSheetApprovalDto);
        void Remove(TimeSheetApprovalDto TimeSheetApprovalDto);
        void Add(TimeSheetApprovalDto TimeSheetApprovalDto);
        int Count();
        List<TimeSheetApprovalDto> GetByTimeSheetId(long timesheetId);
    }
}
