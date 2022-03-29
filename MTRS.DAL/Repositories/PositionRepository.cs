using MTRS.Core.Entities;
using MTRS.DAL.DbContexts;
using MTRS.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTRS.DAL.Repositories
{
    public class PositionRepository : GenericRepository<Position, Int16>, IPositionRepository
    {
        public PositionRepository(MTRSDBContext context) : base(context)
        {
        }
    }
}
