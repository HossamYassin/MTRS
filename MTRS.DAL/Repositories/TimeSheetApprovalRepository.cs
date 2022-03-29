using MTRS.Core.Entities;
using MTRS.DAL.DbContexts;
using MTRS.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;

namespace MTRS.DAL.Repositories
{
    public class TimeSheetApprovalRepository : GenericRepository<TimeSheetApproval, long>, ITimeSheetApprovalRepository
    {
        public TimeSheetApprovalRepository(MTRSDBContext context) : base(context)
        {
        }

        public List<TimeSheetApproval> GetByTimeSheetId(long timesheetId) 
        {
            var timesheetApprovals = _context.TimeSheetApprovals.AsQueryable()
                .Include(x=>x.Manager).Where(x => x.TimeSheetId == timesheetId).ToList();
            return timesheetApprovals;
        }

    }
}
