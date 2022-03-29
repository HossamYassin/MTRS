using MTRS.Core.Entities;
using MTRS.DAL.DbContexts;
using MTRS.DAL.Interfaces;
using MTRS.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTRS.DAL.Interfaces
{
    public class GradeRepository : GenericRepository<Grade, Int16>, IGradeRepository
    {
        public GradeRepository(MTRSDBContext context) : base(context)
        {
        }
    }
}
