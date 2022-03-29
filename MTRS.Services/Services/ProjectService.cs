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
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;

        public ProjectService(IProjectRepository projectRepository, IMapper mapper)
        {
            _mapper = mapper;
            _projectRepository = projectRepository;
        }

        public List<ProjectDto> GetList()
        {
            return _mapper.Map<List<ProjectDto>>(_projectRepository.GetList());
        }

        public List<ProjectDto> GetAll()
        {
            return _mapper.Map<List<ProjectDto>>(_projectRepository.GetAll());
        }

        public ProjectDto GetById(long id)
        {
            return _mapper.Map<ProjectDto>(_projectRepository.GetById(id));
        }

        public void Add(ProjectDto projectDto)
        {
            var entity = _mapper.Map<Project>(projectDto);
            _projectRepository.Add(entity);
        }

        public IList<ProjectDto> Get(Expression<Func<Project, bool>> expression, string sortColumn, bool isSortAscending, int page, int pageSize)
        {
            return _mapper.Map<List<ProjectDto>>(_projectRepository.Find(expression, sortColumn, isSortAscending, page, pageSize).ToList());
        }

        public void Remove(ProjectDto projectDto)
        {
            var entity = _mapper.Map<Project>(projectDto);
            _projectRepository.Remove(entity);
        }

        public void Update(ProjectDto projectDto)
        {
            var project = _projectRepository.GetById(projectDto.Id);

            project.Name = projectDto.Name;
            project.Code = projectDto.Code;
            project.ManagerId = projectDto.ManagerId;
            project.CustomerId = projectDto.CustomerId;
            project.IsCompleted = projectDto.IsCompleted;

            _projectRepository.Update();
        }

        public int Count()
        {
            return _projectRepository.Count();
        }

        public List<ProjectDto> GetUserProject(long userId)
        {
            return _mapper.Map<List<ProjectDto>>(_projectRepository.GetUserProject(userId));
        }

        public List<ProjectDto> GetProjectManagerProjects(long managerId)
        {
            return _mapper.Map<List<ProjectDto>>(_projectRepository.GetProjectManagerProjects(managerId));
        }

        public ProjectDto GetProjectWithDetails(long projectId)
        {
            return _mapper.Map<ProjectDto>(_projectRepository.GetProjectWithDetails(projectId));
        }

        public ProjectDashboard GetProjectDashboard(long projectId)
        {
            return _projectRepository.GetProjectDashboard(projectId);
        }

        public List<ProjectDto> GetUserProjectWithActivity(long userId)
        {
            return _mapper.Map<List<ProjectDto>>(_projectRepository.GetUserProjectWithActivity(userId));
        }

        public List<ProjectDto> GetInActiveProjects(long managerId)
        {
            return _mapper.Map<List<ProjectDto>>(_projectRepository.GetInActiveProjects(managerId));
        }

        public List<ProjectDto> GetActiveProjects(long managerId)
        {
            return _mapper.Map<List<ProjectDto>>(_projectRepository.GetActiveProjects(managerId));
        }
    }
}
