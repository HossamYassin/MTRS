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
    public class PositionService : IPositionService
    {
        private readonly IPositionRepository _positionRepository;
        private readonly IMapper _mapper;

        public PositionService(IPositionRepository PositionRepository, IMapper mapper)
        {
            _mapper = mapper;
            _positionRepository = PositionRepository;
        }

        public List<PositionDto> GetAll()
        {
            return _mapper.Map<List<PositionDto>>(_positionRepository.GetAll());
        }

        public PositionDto GetById(Int16 id)
        {
            return _mapper.Map<PositionDto>(_positionRepository.GetById(id));
        }

        public void Add(PositionDto PositionDto)
        {
            var entity = _mapper.Map<Position>(PositionDto);
            _positionRepository.Add(entity);
        }

        public IList<PositionDto> Get(Expression<Func<Position, bool>> expression, string sortColumn, bool isSortAscending, int page, int pageSize)
        {
            return _mapper.Map<List<PositionDto>>(_positionRepository.Find(expression, sortColumn, isSortAscending, page, pageSize).ToList());
        }

        public void Remove(PositionDto PositionDto)
        {
            var entity = _mapper.Map<Position>(PositionDto);
            _positionRepository.Remove(entity);
        }

        public void Update(PositionDto PositionDto)
        {
            var position = _positionRepository.GetById(PositionDto.Id);
            _mapper.Map(PositionDto, position);

            _positionRepository.Update();
        }

        public int Count()
        {
            return _positionRepository.Count();
        }
    }
}
