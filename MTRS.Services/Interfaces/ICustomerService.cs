using MTRS.Core.DTOs;
using MTRS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace MTRS.Services.Interfaces
{
    public interface ICustomerService
    {
        List<CustomerDto> GetAll();
        CustomerDto GetById(long id);
        IList<CustomerDto> Get(Expression<Func<Customer, bool>> expression, string sortColumn, bool isSortAscending, int page, int pageSize);
        void Update(CustomerDto customerDto);
        void Remove(CustomerDto customerDto);
        void Add(CustomerDto customerDto);
        int Count();
    }
}
