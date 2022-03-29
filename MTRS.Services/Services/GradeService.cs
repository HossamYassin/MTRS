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
    public class GradeService : IGradeService
    {
        private readonly IGradeRepository _gradeRepository;
        private readonly IMapper _mapper;

        public GradeService(IGradeRepository gradeRepository, IMapper mapper)
        {
            _mapper = mapper;
            _gradeRepository = gradeRepository;
        }

        public List<GradeDto> GetAll()
        {
            return _mapper.Map<List<GradeDto>>(_gradeRepository.GetAll());
        }

        public GradeDto GetById(Int16 id)
        {
            return _mapper.Map<GradeDto>(_gradeRepository.GetById(id));
        }

        public void Add(GradeDto GradeDto)
        {
            var entity = _mapper.Map<Grade>(GradeDto);
            _gradeRepository.Add(entity);
        }

        public IList<GradeDto> Get(Expression<Func<Grade, bool>> expression, string sortColumn, bool isSortAscending, int page, int pageSize)
        {
            return _mapper.Map<List<GradeDto>>(_gradeRepository.Find(expression, sortColumn, isSortAscending, page, pageSize).ToList());
        }

        public void Remove(GradeDto GradeDto)
        {
            var entity = _mapper.Map<Grade>(GradeDto);
            _gradeRepository.Remove(entity);
        }

        public void Update(GradeDto GradeDto)
        {
            var Country = _gradeRepository.GetById(GradeDto.Id);
            _mapper.Map(GradeDto, Country);

            _gradeRepository.Update();
        }

        public int Count()
        {
            return _gradeRepository.Count();
        }
    }
}
