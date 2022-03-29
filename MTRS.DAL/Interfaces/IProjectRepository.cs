using MTRS.Core.DTOs;
using MTRS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTRS.DAL.Interfaces
{
    public interface IProjectRepository : IGenericRepository<Project, long>
    {
        List<Project> GetList();
        List<Project> GetUserProject(long userId);
        List<Project> GetProjectManagerProjects(long managerId);
        Project GetProjectWithDetails(long projectId);
        ProjectDashboard GetProjectDashboard(long project);
        List<Project> GetUserProjectWithActivity(long userId);
        List<Project> GetInActiveProjects(long managerId);
        List<Project> GetActiveProjects(long managerId);
        int GetAcctuleCostByGrad(string grad, DateTime startDate, DateTime endDate, long projectId);
    }
}
