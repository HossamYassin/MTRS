using MTRS.Core.Entities;
using MTRS.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTRS.DAL.Interfaces
{
    public interface ICustomerRepository : IGenericRepository<Customer, long>
    {
    }
}
