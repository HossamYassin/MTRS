using MTRS.Core.Entities;
using MTRS.DAL.DbContexts;
using MTRS.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTRS.DAL.Repositories
{
    public class LogRepository : LoggerGenericRepository<NLog, long>, ILogRepository
    {
        public LogRepository(LoggerDBContext context) : base(context)
        {
        }
    }
}
