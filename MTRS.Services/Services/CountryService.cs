using AutoMapper;
using MTRS.Core.DTOs;
using MTRS.Core.Entities;
using MTRS.DAL.Interfaces;
using MTRS.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace MTRS.Services
{
    public class CountryService : ICountryService
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public CountryService(ICountryRepository countryRepository, IMapper mapper)
        {
            _mapper = mapper;
            _countryRepository = countryRepository;
        }

        public List<CountryDto> GetAll()
        {
            return _mapper.Map<List<CountryDto>>(_countryRepository.GetAll());
        }

        public CountryDto GetById(Int16 id)
        {
            return _mapper.Map<CountryDto>(_countryRepository.GetById(id));
        }

        public void Add(CountryDto CountryDto)
        {
            var entity = _mapper.Map<Country>(CountryDto);
            _countryRepository.Add(entity);
        }

        public IList<CountryDto> Get(Expression<Func<Country, bool>> expression, string sortColumn, bool isSortAscending, int page, int pageSize)
        {
            return _mapper.Map<List<CountryDto>>(_countryRepository.Find(expression, sortColumn, isSortAscending, page, pageSize).ToList());
        }

        public void Remove(CountryDto CountryDto)
        {
            var entity = _mapper.Map<Country>(CountryDto);
            _countryRepository.Remove(entity);
        }

        public void Update(CountryDto CountryDto)
        {
            var Country = _countryRepository.GetById(CountryDto.Id);
            _mapper.Map(CountryDto, Country);

            _countryRepository.Update();
        }

        public int Count()
        {
            return _countryRepository.Count();
        }
    }
}
