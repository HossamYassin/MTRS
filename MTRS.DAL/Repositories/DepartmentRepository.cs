using MTRS.Core.Entities;
using MTRS.DAL.DbContexts;
using MTRS.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTRS.DAL.Repositories
{
    public class DepartmentRepository : GenericRepository<Department, Int16>, IDepartmentRepository
    {
        public DepartmentRepository(MTRSDBContext context) : base(context)
        {
        }
    }
}
