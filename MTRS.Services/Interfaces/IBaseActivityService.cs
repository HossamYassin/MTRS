using MTRS.Core.DTOs;
using MTRS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace MTRS.Services.Interfaces
{
    public interface IBaseActivityService
    {
        List<BaseActivityDto> GetAll();
        BaseActivityDto GetById(int id);
        IList<BaseActivityDto> Get(Expression<Func<BaseActivity, bool>> expression, string sortColumn, bool isSortAscending, int page, int pageSize);
        void Update(BaseActivityDto baseActivityDto);
        void Remove(BaseActivityDto baseActivityDto);
        void Add(BaseActivityDto baseActivityDto);
        int Count();
    }
}
