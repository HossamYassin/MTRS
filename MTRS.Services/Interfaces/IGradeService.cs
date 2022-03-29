using MTRS.Core.DTOs;
using MTRS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace MTRS.Services.Interfaces
{
    public interface IGradeService
    {
        List<GradeDto> GetAll();
        GradeDto GetById(Int16 id);
        IList<GradeDto> Get(Expression<Func<Grade, bool>> expression, string sortColumn, bool isSortAscending, int page, int pageSize);
        void Update(GradeDto gradeDto);
        void Remove(GradeDto gradeDto);
        void Add(GradeDto gradeDto);
        int Count();
    }
}
