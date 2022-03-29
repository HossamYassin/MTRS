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
    public class CustomerRepository : GenericRepository<Customer, long>, ICustomerRepository
    {
        public CustomerRepository(MTRSDBContext context) : base(context)
        {
        }
    }
}