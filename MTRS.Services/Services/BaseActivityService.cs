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
    public class BaseActivityService : IBaseActivityService
    {
        private readonly IBaseActivityRepository _baseActivityRepository;
        private readonly IMapper _mapper;

        public BaseActivityService(IBaseActivityRepository baseActivityService, IMapper mapper)
        {
            _mapper = mapper;
            _baseActivityRepository = baseActivityService;
        }

        public List<BaseActivityDto> GetAll()
        {
            return _mapper.Map<List<BaseActivityDto>>(_baseActivityRepository.GetAll());
        }

        public BaseActivityDto GetById(int id)
        {
            return _mapper.Map<BaseActivityDto>(_baseActivityRepository.GetById(id));
        }

        public void Add(BaseActivityDto BaseActivityDto)
        {
            var entity = _mapper.Map<BaseActivity>(BaseActivityDto);
            _baseActivityRepository.Add(entity);
        }

        public IList<BaseActivityDto> Get(Expression<Func<BaseActivity, bool>> expression, string sortColumn, bool isSortAscending, int page, int pageSize)
        {
            return _mapper.Map<List<BaseActivityDto>>(_baseActivityRepository.Find(expression, sortColumn, isSortAscending, page, pageSize).ToList());
        }

        public void Remove(BaseActivityDto BaseActivityDto)
        {
            var entity = _mapper.Map<BaseActivity>(BaseActivityDto);
            _baseActivityRepository.Remove(entity);
        }

        public void Update(BaseActivityDto BaseActivityDto)
        {
            var baseActivity = _baseActivityRepository.GetById(BaseActivityDto.Id);
            _mapper.Map(BaseActivityDto, baseActivity);

            _baseActivityRepository.Update();
        }

        public int Count()
        {
            return _baseActivityRepository.Count();
        }
    }
}
