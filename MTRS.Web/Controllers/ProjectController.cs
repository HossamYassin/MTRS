using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MTRS.Core.DTOs;
using MTRS.Services.Interfaces;
using MTRS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MTRS.Web.Controllers
{
    [Authorize]
    public class ProjectController : BaseController
    {
        private readonly ILogger<ProjectController> _logger;
        private readonly IProjectService _projectService;
        private readonly ITimeSheetService _timeSheetService;
        private readonly IActivityService _activityService;
        private readonly ITimeSheetDetailsService _timeSheetDetailsService;
        private readonly ITimeSheetApprovalService _timeSheetApprovalService;
        private readonly IUserService _userService;
        private readonly IBaseActivityService _baseActivityService;

        private readonly INotyfService _notyf;

        public ProjectController(ILogger<ProjectController> logger,
            IProjectService projectService,
            INotyfService notyf,
            IActivityService activityService,
            ITimeSheetService timeSheetService, 
            ITimeSheetDetailsService timeSheetDetailsService,
            ITimeSheetApprovalService timeSheetApprovalService,
            IUserService userService,
            IBaseActivityService baseActivityService)
        {
            _projectService = projectService;
            _activityService = activityService;
            _timeSheetService = timeSheetService;
            _timeSheetDetailsService = timeSheetDetailsService;
            _timeSheetApprovalService = timeSheetApprovalService;
            _userService = userService;
            _baseActivityService = baseActivityService;

            _logger = logger;
            _notyf = notyf;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewData["PageTitle"] = "My Projects";

            long userId = long.Parse(this.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            ViewBag.Projects = _projectService.GetProjectManagerProjects(userId);
            
            return View();
        }

        [HttpGet]
        public IActionResult GetUserProjectWithActivity()
        {
            long userId = long.Parse(this.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var projects = _projectService.GetUserProjectWithActivity(userId);

            return Json(projects);
        }

        [HttpGet]
        public IActionResult GetUserProjects()
        {
            long userId = long.Parse(this.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var projects = _projectService.GetUserProject(userId);

            return Json(projects);
        }

        [HttpGet]
        public IActionResult CompleteProject(bool isCompleted, long projectId)
        {
           var project = _projectService.GetById(projectId);
            project.IsCompleted = isCompleted;
            
            _projectService.Update(project);
            return Json("true");
        }

        [HttpGet]
        public IActionResult GetProjects(long userId)
        {
            var projects = _projectService.GetUserProject(userId);

            return Json(projects);
        }

        [HttpGet]
        public IActionResult GetProjectById(long projectId)
        {
            var project = _projectService.GetProjectWithDetails(projectId);
            return Json(project);
        }

        [HttpGet]
        public IActionResult GetProjectActivities(long projectId)
        {
            var activities = _activityService.GetByProjectId(projectId);
            return Json(activities);
        }

        [HttpPost]
        public IActionResult AddActivity([FromBody]ActivityDto activityDto)
        {
            activityDto.LoggedHours = 0;
            activityDto.Type = Core.Enums.ActivityType.Project;

            _activityService.Add(activityDto);
            _notyf.Success("Activity added successfully");

            return Json("Data Saved");
        }

        [HttpPost]
        public IActionResult EditActivity([FromBody]ActivityDto activityDto)
        {
            activityDto.LoggedHours = 0;
            activityDto.Type = Core.Enums.ActivityType.Project;

            _activityService.Update(activityDto);
            _notyf.Success("Activity updated successfully");

            return Json("Data Saved");
        }

        [HttpGet]
        public IActionResult GetEmployees()
        {
            var employees = _userService.GetAll();
            foreach (var employee in employees)
            {
                employee.FirstName = employee.FirstName + " " + employee.LastName;
            }
            return Json(employees.OrderBy(x => x.FirstName));
        }

        [HttpGet]
        public IActionResult GetAssignedEmployeesByActivityId(long activityId)
        {
            var employees = _activityService.GetAssignedEmployeesByActivityId(activityId);
            foreach (var employee in employees)
            {
                employee.FirstName = employee.FirstName + " " + employee.LastName;
            }
            return Json(employees.OrderBy(x => x.FirstName));
        }

        [HttpPost]
        public IActionResult AssignEmployee([FromBody] ActivityUserDto activityUserDto)
        {
            var result = _activityService.AssignEmployeeToActivity(activityUserDto);
            if (result == 0)
            {
                _notyf.Error("User already exist");
            }
            else
            {
                _notyf.Success("User assigned successfully");
            }
            return Json(result);
        }

        [HttpPost]
        public IActionResult UnassignEmployee([FromBody] ActivityUserDto activityUserDto)
        {
            var result = _activityService.UnassignUser(activityUserDto);
            _notyf.Success("User unassigned successfully");
            return Json(result);
        }

        [HttpGet]
        public IActionResult GetPendingEmployeesByProject(long projectId)
        {
            var employees = _timeSheetService.GetPendingEmployeesByProject(projectId);
            foreach (var employee in employees)
            {
                employee.FirstName = employee.FirstName + " " + employee.LastName;
            }
            return Json(employees.OrderBy(x => x.FirstName));
        }

        [HttpGet]
        public IActionResult GetEmployeesByProject(long projectId)
        {
            var employees = _timeSheetService.GetEmployeesByProject(projectId);
            foreach (var employee in employees)
            {
                employee.FirstName = employee.FirstName + " " + employee.LastName;
            }
            return Json(employees.OrderBy(x => x.FirstName));
        }

        [HttpGet]
        public IActionResult GetEmployeeTimeSheetByProject(long projectId, long employeeId)
        {
            var timesheets = _timeSheetService.GetEmployeeTimeSheetByProject(projectId, employeeId);
            return Json(timesheets.OrderByDescending(x => x.FromDate));
        }

        [HttpGet]
        public IActionResult GetProjectDashboard(long projectId)
        {
            var dashboard = _projectService.GetProjectDashboard(projectId);
            return Json(dashboard);
        }

        [HttpGet]
        public IActionResult GetProjectBaseActivities()
        {
            var activities = _baseActivityService.GetAll();
            return Json(activities);
        }

        [HttpGet]
        public IActionResult GetActiveProjects(bool isActive)
        {
            long managerId = long.Parse(this.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            if (isActive)
            {
                var projects = _projectService.GetActiveProjects(managerId);

                var projectsNodes = new List<ProjectNode>(); 
                foreach (var project in projects)
                {
                    var projectNode = new ProjectNode()
                    {
                        Text = project.Name + "<i class='far fa-eye' onclick='viewProject(" + project.Id + ");' style='float:right;cursor: pointer;font-size: x-large;'></i>",
                        Icon = "fas fa-project-diagram"
                    };

                    if (project.Activities.Any()) 
                    {
                        projectNode.Nodes = new List<ActivityNode>();

                        foreach (var activity in project.Activities)
                        {
                            string style = "";
                            if (activity.LoggedHours.Value > activity.PlannedHours.Value)
                            {
                                style = "color:red;font-weight: bold;";
                            }

                            projectNode.Nodes.Add(new ActivityNode()
                            {
                                Text = "<div class='row' style='width:100%;' ><div style='margin-left:120px;width:140px;'>" + activity.Name + "</div><div style='width:90px;margin-left:80px;'>" + activity.StartDate.Value.ToString("dd/MM/yyyy")
                                    + "</div><div style='width:90px;margin-left:100px;'>" + activity.EndDate.Value.ToString("dd/MM/yyyy") + "</div><div style='width:60px;margin-left:100px;'>"  + activity.PlannedHours.Value + "</div><div style='width:60px;margin-left:90px;"+ style +"'>" + activity.LoggedHours.Value.ToString() + "</div></div>"
                            });
                        }
                    }

                    projectsNodes.Add(projectNode);
                }

                return Json(projectsNodes);
            }
            else
            {
                var projects = _projectService.GetInActiveProjects(managerId);

                var projectsNodes = new List<ProjectNode>();
                foreach (var project in projects)
                {
                    var projectNode = new ProjectNode()
                    {
                        Text = project.Name + "<i class='far fa-eye' onclick='viewProject(" + project.Id + ");' style='float:right;cursor: pointer;font-size: x-large;'></i>",
                        Icon = "fas fa-project-diagram"
                    };

                    if (project.Activities.Any())
                    {
                        projectNode.Nodes = new List<ActivityNode>();

                        foreach (var activity in project.Activities)
                        {
                            string style = "";
                            if (activity.LoggedHours.Value > activity.PlannedHours.Value)
                            {
                                style = "color:red;font-weight: bold;";
                            }

                            projectNode.Nodes.Add(new ActivityNode()
                            {
                                Text = "<div class='row' style='width:100%;' ><div style='margin-left:120px;width:140px;'>" + activity.Name + "</div><div style='width:90px;margin-left:80px;'>" + activity.StartDate.Value.ToString("dd/MM/yyyy")
                                     + "</div><div style='width:90px;margin-left:100px;'>" + activity.EndDate.Value.ToString("dd/MM/yyyy") + "</div><div style='width:60px;margin-left:100px;'>" + activity.PlannedHours.Value + "</div><div style='width:60px;margin-left:90px;" + style + "'>" + activity.LoggedHours.Value.ToString() + "</div></div>"
                            });
                        }
                    }

                    projectsNodes.Add(projectNode);
                }

                return Json(projectsNodes);
            }
        }

        [HttpGet]
        public IActionResult MyProjects()
        {
            ViewData["PageTitle"] = "My Projects";
            return View();
        }
    }
}
