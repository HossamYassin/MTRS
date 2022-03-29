using MTRS.Core.DTOs;
using MTRS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace MTRS.Services.Interfaces
{
    public interface IProjectService
    {
        List<ProjectDto> GetAll();
        ProjectDto GetById(long id);
        IList<ProjectDto> Get(Expression<Func<Project, bool>> expression, string sortColumn, bool isSortAscending, int page, int pageSize);
        void Update(ProjectDto ProjectDto);
        void Remove(ProjectDto ProjectDto);
        void Add(ProjectDto ProjectDto);
        int Count();
        List<ProjectDto> GetList();
        List<ProjectDto> GetUserProject(long userId);
        List<ProjectDto> GetProjectManagerProjects(long managerId);
        ProjectDto GetProjectWithDetails(long projectId);
        ProjectDashboard GetProjectDashboard(long projectId);
        List<ProjectDto> GetUserProjectWithActivity(long userId);
        List<ProjectDto> GetInActiveProjects(long managerId);
        List<ProjectDto> GetActiveProjects(long managerId);
    }
}
