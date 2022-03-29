using MTRS.Core.DTOs;
using MTRS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace MTRS.Services.Interfaces
{
    public interface IPositionService
    {
        List<PositionDto> GetAll();
        PositionDto GetById(Int16 id);
        IList<PositionDto> Get(Expression<Func<Position, bool>> expression, string sortColumn, bool isSortAscending, int page, int pageSize);
        void Update(PositionDto userDto);
        void Remove(PositionDto userDto);
        void Add(PositionDto userDto);
        int Count();
    }
}
