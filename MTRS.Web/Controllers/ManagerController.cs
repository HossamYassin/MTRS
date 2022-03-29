using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MTRS.Core.DTOs;
using MTRS.Services.Interfaces;
using MTRS.Services;
using MTRS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MTRS.Web.Controllers
{
    [Authorize]
    public class ManagerController : BaseController
    {
        private readonly ILogger<ProjectController> _logger;
        private readonly IProjectService _projectService;
        private readonly ITimeSheetService _timeSheetService;
        private readonly IActivityService _activityService;
        private readonly ITimeSheetDetailsService _timeSheetDetailsService;
        private readonly ITimeSheetApprovalService _timeSheetApprovalService;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly INotyfService _notyf;
        private readonly IPLDataService _pLDataService;

        public ManagerController(ILogger<ProjectController> logger,
            IProjectService projectService,
            INotyfService notyf,
            IActivityService activityService,
            ITimeSheetService timeSheetService,
            ITimeSheetDetailsService timeSheetDetailsService,
            ITimeSheetApprovalService timeSheetApprovalService,
            IUserService userService,
            IConfiguration configuration,
            IPLDataService pLDataService)
        {
            _projectService = projectService;
            _activityService = activityService;
            _timeSheetService = timeSheetService;
            _timeSheetDetailsService = timeSheetDetailsService;
            _timeSheetApprovalService = timeSheetApprovalService;
            _userService = userService;
            _pLDataService = pLDataService;

            _configuration = configuration;
            _logger = logger;
            _notyf = notyf;
        }

        public IActionResult Index()
        {
            ViewData["PageTitle"] = "My Team";

            return View();
        }

        [HttpGet]
        public IActionResult GetTeamPendingEmployees()
        {
            long managerId = long.Parse(this.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var employees = _timeSheetService.GetTeamPendingEmployees(managerId);

            foreach (var employee in employees)
            {
                employee.FirstName = employee.FirstName + " " + employee.LastName;
            }

            return Json(employees.OrderBy(x => x.FirstName));
        }

        [HttpGet]
        public IActionResult GetDashboard()
        {
            long managerId = long.Parse(this.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var dashboard = _userService.GetTeamDashboard(managerId);

            return Json(dashboard);
        }

        [HttpGet]
        public IActionResult GetEmployeeTimeSheet(long employeeId)
        {
            long managerId = long.Parse(this.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var timesheets = _timeSheetService.GetEmployeeTimeSheet(managerId, employeeId);

            return Json(timesheets.OrderByDescending(x => x.FromDate));
        }

        [HttpGet]
        public IActionResult GetMyEmployees()
        {
            long userId = long.Parse(this.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = _userService.GetById(userId);

            var employees = _userService.GetMyEmployees(userId, 5);

            var employeeNodes = new EmployeeNode();
            employeeNodes.Text = user.FirstName + " " + user.LastName;
            employeeNodes.Nodes = new List<Node>();

            foreach (var employee in employees)
            {
                var node = new Node();
                employeeNodes.Nodes.Add(node);
                node.Text = employee.FirstName + " " + employee.LastName + " - " + employee.Position.Name + "<i class='far fa-calendar' onclick='viewTimesheet(" + employee.Id + ");' style='float:right;cursor: pointer;font-size: x-large;'></i>";
                node.Icon = "fas fa-user";
                GetSubEmployees(employees, employee, node, 5);
            }

            employeeNodes.Nodes.OrderBy(x => x.Text).ToList();
            return Json(employeeNodes);
        }

        [HttpGet]
        public IActionResult GetMyEmployeesList()
        {
            long userId = long.Parse(this.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var employees = _userService.GetMyEmployees(userId, 5);

            foreach (var employee in employees)
            {
                employee.FirstName = employee.FirstName + " " + employee.LastName;
            }
            
            return Json(employees.OrderBy(x=>x.FirstName).ToList());
        }


        [HttpGet]
        public IActionResult GetPLData(string opportunityId, long projectId)
        {
            var connection = _configuration.GetConnectionString("PLDataConnection");

            _pLDataService.Connection = connection;
            var plData = _pLDataService.GetProjectPLData(opportunityId, projectId);

            return Json(plData);
        }


        [HttpGet]
        public IActionResult SearchEmployees(string name)
        {
            long userId = long.Parse(this.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var employees = _userService.GetMyEmployees(userId, 5);

            foreach (var employee in employees)
            {
                employee.FirstName = employee.FirstName + " " + employee.LastName;
            }

            var result = employees.Where(x => x.FirstName.ToLower().Contains(name.ToLower())).ToList();

            List<Node> employeesNodes = new List<Node>();
            foreach (var employee in result)
            {
                var node = new Node();
                node.Text = employee.FirstName + " - " + employee.Position.Name + "<i class='far fa-calendar' onclick='viewTimesheet(" + employee.Id + ");' style='float:right;cursor: pointer;font-size: x-large;'></i>";
                node.Icon = "fas fa-user";
                employeesNodes.Add(node);
            }

            return Json(employeesNodes.OrderBy(x => x.Text).ToList());
        }

        private void GetSubEmployees(List<UserDto> employees, UserDto currentEmployee, Node node, int level)
        {
            var currentLevel = 0;
            var subEmployees = employees.Where(x => x.ManagerId == currentEmployee.Id).ToList();
            if (subEmployees.Any())
            {
                foreach (var subEmployee in subEmployees)
                {
                    node.Nodes = new List<Node>();

                    if (currentLevel < level)
                    {
                        var subNode = new Node();
                        subNode.Text = subEmployee.FirstName + " " + subEmployee.LastName + " - " + subEmployee.Position.Name + "<i class='far fa-calendar' onclick='viewTimesheet(" + subEmployee.Id + ");' style='float:right;cursor: pointer;font-size: x-large;'></i>";
                        subNode.Icon = "fas fa-user";
                        node.Nodes.Add(subNode);

                        GetSubEmployees(employees, subEmployee, subNode, level);
                        currentLevel++;
                    }
                }
            }
        }
    }
}
