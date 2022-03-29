using MTRS.Core.DTOs;
using MTRS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace MTRS.Services.Interfaces
{
    public interface IDepartmentService
    {
        List<DepartmentDto> GetAll();
        DepartmentDto GetById(Int16 id);
        IList<DepartmentDto> Get(Expression<Func<Department, bool>> expression, string sortColumn, bool isSortAscending, int page, int pageSize);
        void Update(DepartmentDto userDto);
        void Remove(DepartmentDto userDto);
        void Add(DepartmentDto userDto);
        int Count();
    }
}
