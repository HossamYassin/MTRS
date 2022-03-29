using MTRS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTRS.DAL.Interfaces
{
    public interface ITimeSheetApprovalRepository : IGenericRepository<TimeSheetApproval, long>
    {
        public List<TimeSheetApproval> GetByTimeSheetId(long timesheeetId);
    }
}
