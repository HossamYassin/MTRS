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
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public DepartmentService(IDepartmentRepository depRepository, IMapper mapper)
        {
            _mapper = mapper;
            _departmentRepository = depRepository;
        }

        public List<DepartmentDto> GetAll()
        {
            return _mapper.Map<List<DepartmentDto>>(_departmentRepository.GetAll());
        }

        public DepartmentDto GetById(Int16 id)
        {
            return _mapper.Map<DepartmentDto>(_departmentRepository.GetById(id));
        }

        public void Add(DepartmentDto departmentDto)
        {
            var entity = _mapper.Map<Department>(departmentDto);
            _departmentRepository.Add(entity);
        }

        public IList<DepartmentDto> Get(Expression<Func<Department, bool>> expression, string sortColumn, bool isSortAscending, int page, int pageSize)
        {
            return _mapper.Map<List<DepartmentDto>>(_departmentRepository.Find(expression, sortColumn, isSortAscending, page, pageSize).ToList());
        }

        public void Remove(DepartmentDto departmentDto)
        {
            var entity = _mapper.Map<Department>(departmentDto);
            _departmentRepository.Remove(entity);
        }

        public void Update(DepartmentDto departmentDto)
        {
            var department = _departmentRepository.GetById(departmentDto.Id);

            department.Name = departmentDto.Name;
            department.ManagerId = departmentDto.ManagerId;

            _departmentRepository.Update();
        }

        public int Count()
        {
            return _departmentRepository.Count();
        }
    }
}
