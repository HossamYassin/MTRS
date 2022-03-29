using MTRS.Core.Entities;
using MTRS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTRS.DAL.Interfaces
{
    public interface ITimeSheetDetailsRepository : IGenericRepository<TimeSheetDetails, long>
    {
        List<TimeSheetDetails> GetByTimeSheetId(long timesheetId);
        bool SaveSheetActivities(List<TimeSheetDetails> timeSheetDetails, TimeSheetStatus status);
    }
}
