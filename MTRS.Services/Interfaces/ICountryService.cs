using MTRS.Core.DTOs;
using MTRS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace MTRS.Services.Interfaces
{
    public interface ICountryService
    {
        List<CountryDto> GetAll();
        CountryDto GetById(Int16 id);
        IList<CountryDto> Get(Expression<Func<Country, bool>> expression, string sortColumn, bool isSortAscending, int page, int pageSize);
        void Update(CountryDto CountryDto);
        void Remove(CountryDto CountryDto);
        void Add(CountryDto CountryDto);
        int Count();
    }
}
