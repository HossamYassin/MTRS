using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
    [Authorize(Roles = "Admin")]
    public class AdminController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICustomerService _customerService;
        private readonly IProjectService _projectService;
        private readonly IUserService _userService;
        private readonly IPositionService _positionService;
        private readonly IDepartmentService _departmentService;
        private readonly IActivityService _activityService;
        private readonly IGradeService _gradeService;
        private readonly IBaseActivityService _baseActivityService;
        private readonly INotyfService _notyf;
        private readonly IConfiguration _configuration;
        private readonly IPLDataService _pLDataService;

        public AdminController(ILogger<HomeController> logger, 
            IProjectService projectService, ICustomerService customerService,
            INotyfService notyf, IUserService userService,
            IPositionService positionService, IDepartmentService departmentService,
            IActivityService activityService,
            IGradeService gradeService,
            IBaseActivityService baseActivityService,
            IConfiguration configuration,
            IPLDataService pLDataService)
        {
            _customerService = customerService;
            _projectService = projectService;
            _userService = userService;
            _gradeService = gradeService;
            _baseActivityService = baseActivityService;
            _logger = logger;
            _notyf = notyf;
            _activityService = activityService;
            _positionService = positionService;
            _departmentService = departmentService;
            _configuration = configuration;
            _pLDataService = pLDataService;
        }

        #region Customers
        public IActionResult Customers()
        {
            ViewData["PageTitle"] = "Customers";
            return View();
        }

        [HttpGet]
        public IActionResult GetCustomers()
        {
            var customers = _customerService.GetAll();
            return Json(customers);
        }

        [HttpGet]
        public IActionResult EditCustomer(long id)
        {
            ViewData["PageTitle"] = "Customers";

            var customer = _customerService.GetById(id);
            return View(customer);
        }

        [HttpPost]
        public IActionResult EditCustomer(CustomerDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                _customerService.Update(model);
                _notyf.Success("Customer updated successfully");
                return RedirectToAction("Customers", "Admin", null);  
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult AddCustomer()
        {
            ViewData["PageTitle"] = "Customers";
            return View(new CustomerDto());
        }

        [HttpPost]
        public IActionResult AddCustomer(CustomerDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                _customerService.Add(model);
                _notyf.Success("Customer added successfully");
                return RedirectToAction("Customers", "Admin", null);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return View(model);
            }
        }
        #endregion

        #region Projects
        public IActionResult Projects()
        {
            ViewData["PageTitle"] = "Projects";
            return View();
        }

        [HttpGet]
        public IActionResult GetProjects()
        {
            var projects = _projectService.GetList();
            return Json(projects);
        }

        [HttpGet]
        public IActionResult GetPLData(string opportunityId)
        {
            var connection = _configuration.GetConnectionString("PLDataConnection");
            _pLDataService.Connection = connection;
            var plData = _pLDataService.GetByOpportunityID(opportunityId);

            return Json(plData);
        }

        [HttpGet]
        public IActionResult EditProject(long id)
        {
            ViewData["PageTitle"] = "Projects";

            ViewBag.Customers = _customerService.GetAll();
            var users = _userService.GetAll().Where(x=>x.PositionId == 4 && x.IsActive == true).ToList();
            foreach (var user in users)
            {
                user.FirstName = user.FirstName + " " + user.LastName;
            }
            ViewBag.Managers = users.OrderBy(x=>x.FirstName).ToList();

            var customer = _projectService.GetById(id);
            return View(customer);
        }

        [HttpPost]
        public IActionResult EditProject(ProjectDto model)
        {
            ViewBag.Customers = _customerService.GetAll();
            var users = _userService.GetAll().Where(x=>x.IsActive == true && x.PositionId == 4).ToList();
            foreach (var user in users)
            {
                user.FirstName = user.FirstName + " " + user.LastName;
            }
            ViewBag.Managers = users.OrderBy(x => x.FirstName).ToList();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                _projectService.Update(model);
                _notyf.Success("Project updated successfully");
                return RedirectToAction("Projects", "Admin", null);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult AddProject()
        {
            ViewData["PageTitle"] = "Projects";

            ViewBag.Customers = _customerService.GetAll();
            var users = _userService.GetAll().Where(x=>x.IsActive == true && x.PositionId == 4).ToList();
            foreach (var user in users)
            {
                user.FirstName = user.FirstName + " " + user.LastName;
            }
            ViewBag.Managers = users.OrderBy(x => x.FirstName).ToList();

            return View(new ProjectDto());
        }

        [HttpPost]
        public IActionResult AddProject(ProjectDto model)
        {
            ViewBag.Customers = _customerService.GetAll();
            var users = _userService.GetAll().Where(x=>x.IsActive == true && x.PositionId == 4).ToList();
            foreach (var user in users)
            {
                user.FirstName = user.FirstName + " " + user.LastName;
            }
            ViewBag.Managers = users.OrderBy(x => x.FirstName).ToList();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                _projectService.Add(model);
                _notyf.Success("Project added successfully");
                return RedirectToAction("Projects", "Admin", null);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return View(model);
            }
        }
        #endregion

        #region Employees
        public IActionResult Employees()
        {
            ViewData["PageTitle"] = "Employees";
            return View();
        }

        [HttpGet]
        public IActionResult GetEmployees()
        {
            var employees = _userService.GetList();
            return Json(employees);
        }

        [HttpGet]
        public IActionResult EditEmployee(long id)
        {
            ViewData["PageTitle"] = "Employees";

            ViewBag.Positions = _positionService.GetAll();
            ViewBag.Departments = _departmentService.GetAll();
            ViewBag.Grades = _gradeService.GetAll();
            var users = _userService.GetAll().Where(x => x.IsActive == true).ToList();
            foreach (var user in users)
            {
                user.FirstName = user.FirstName + " " + user.LastName;
            }
            ViewBag.Managers = users.OrderBy(x => x.FirstName).ToList();

            var employee = _userService.GetById(id);
            return View(employee);
        }

        [HttpPost]
        public IActionResult EditEmployee(UserDto model)
        {
            ViewData["PageTitle"] = "Employees";

            ViewBag.Positions = _positionService.GetAll();
            ViewBag.Departments = _departmentService.GetAll();
            ViewBag.Grades = _gradeService.GetAll();
            var users = _userService.GetAll().Where(x => x.IsActive == true).ToList();
            foreach (var user in users)
            {
                user.FirstName = user.FirstName + " " + user.LastName;
            }
            ViewBag.Managers = users.OrderBy(x => x.FirstName).ToList();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                _userService.Update(model);
                _notyf.Success("Employee updated successfully");
                return RedirectToAction("Employees", "Admin", null);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return View(model);
            }
        }
        #endregion

        #region Departments
        public IActionResult Departments()
        {
            ViewData["PageTitle"] = "Departments";
            return View();
        }

        [HttpGet]
        public IActionResult GetDepartments()
        {
            var customers = _departmentService.GetAll();
            return Json(customers);
        }

        [HttpGet]
        public IActionResult EditDepartment(Int16 id)
        {
            ViewData["PageTitle"] = "Departments";

            var users = _userService.GetAll().Where(x => x.IsActive == true).ToList();
            foreach (var user in users)
            {
                user.FirstName = user.FirstName + " " + user.LastName;
            }
            ViewBag.Managers = users.OrderBy(x => x.FirstName).ToList();

            var customer = _departmentService.GetById(id);
            return View(customer);
        }

        [HttpPost]
        public IActionResult EditDepartment(DepartmentDto model)
        {
            var users = _userService.GetAll().Where(x => x.IsActive == true).ToList();
            foreach (var user in users)
            {
                user.FirstName = user.FirstName + " " + user.LastName;
            }
            ViewBag.Managers = users.OrderBy(x => x.FirstName).ToList();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                _departmentService.Update(model);
                _notyf.Success("Department updated successfully");
                return RedirectToAction("Departments", "Admin", null);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult AddDepartment()
        {
            ViewData["PageTitle"] = "Departments";

            var users = _userService.GetAll().Where(x => x.IsActive == true).ToList();
            foreach (var user in users)
            {
                user.FirstName = user.FirstName + " " + user.LastName;
            }
            ViewBag.Managers = users.OrderBy(x => x.FirstName).ToList();

            return View(new DepartmentDto());
        }

        [HttpPost]
        public IActionResult AddDepartment(DepartmentDto model)
        {
            var users = _userService.GetAll().Where(x => x.IsActive == true).ToList();
            foreach (var user in users)
            {
                user.FirstName = user.FirstName + " " + user.LastName;
            }
            ViewBag.Managers = users.OrderBy(x => x.FirstName).ToList();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                _departmentService.Add(model);
                _notyf.Success("Department added successfully");
                return RedirectToAction("Departments", "Admin", null);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return View(model);
            }
        }
        #endregion

        #region Activites
        public IActionResult Activites()
        {
            ViewData["PageTitle"] = "Activites";
            long userId = long.Parse(this.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            ViewBag.Projects = _projectService.GetAll().Where(x=>x.ManagerId == userId);

            return View();
        }

        [HttpGet]
        public IActionResult GetActivites(long projectId)
        {
            var projects = _activityService.GetList(projectId);
            return Json(projects);
        }

        [HttpGet]
        public IActionResult EditActivity(long id)
        {
            ViewData["PageTitle"] = "Activites";

            ViewBag.Projects = _projectService.GetAll();

            var activity = _activityService.GetById(id);
            return View(activity);
        }

        [HttpPost]
        public IActionResult EditActivity(ActivityDto model)
        {
            ViewBag.Projects = _projectService.GetAll();
      
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                _activityService.Update(model);
                _notyf.Success("Activity updated successfully");
                return RedirectToAction("Activites", "Admin", null);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult AddActivity()
        {
            ViewData["PageTitle"] = "Activites";
            ViewBag.Customers = _customerService.GetAll();
            
            return View(new ActivityDto());
        }

        [HttpPost]
        public IActionResult AddActivity(ActivityDto model)
        {
            ViewBag.Projects = _projectService.GetAll();
            
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                _activityService.Add(model);
                _notyf.Success("Activity added successfully");
                return RedirectToAction("Activites", "Admin", null);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return View(model);
            }
        }
        #endregion

        #region GeneralActivities
        public IActionResult GeneralActivities()
        {
            ViewData["PageTitle"] = "General Activities";
            return View();
        }

        [HttpGet]
        public IActionResult GetGeneralActivities()
        {
            var activities = _activityService.GetGeneral();
            return Json(activities);
        }

        [HttpGet]
        public IActionResult EditGeneralActivity(long id)
        {
            ViewData["PageTitle"] = "General Activities";

            var activity = _activityService.GetById(id);
            return View(activity);
        }

        [HttpPost]
        public IActionResult EditGeneralActivity(ActivityDto model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                model.Type = Core.Enums.ActivityType.General;
                _activityService.Update(model);
                _notyf.Success("Activity updated successfully");
                return RedirectToAction("GeneralActivities", "Admin", null);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult AddGeneralActivity()
        {
            ViewData["PageTitle"] = "General Activities";
            return View(new ActivityDto());
        }

        [HttpPost]
        public IActionResult AddGeneralActivity(ActivityDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                model.Type = Core.Enums.ActivityType.General;
                _activityService.Add(model);
                _notyf.Success("Activity added successfully");
                return RedirectToAction("GeneralActivities", "Admin", null);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return View(model);
            }
        }
        #endregion

        #region Positions
        public IActionResult Positions()
        {
            ViewData["PageTitle"] = "Roles";
            return View();
        }

        [HttpGet]
        public IActionResult GetPositions()
        {
            var positions = _positionService.GetAll();
            return Json(positions);
        }

        [HttpGet]
        public IActionResult EditPosition(Int16 id)
        {
            ViewData["PageTitle"] = "Roles";

            var position = _positionService.GetById(id);
            return View(position);
        }

        [HttpPost]
        public IActionResult EditPosition(PositionDto model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                _positionService.Update(model);
                _notyf.Success("Role updated successfully");
                return RedirectToAction("Positions", "Admin", null);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult AddPosition()
        {
            ViewData["PageTitle"] = "Roles";
            return View(new PositionDto());
        }

        [HttpPost]
        public IActionResult AddPosition(PositionDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                _positionService.Add(model);
                _notyf.Success("Role added successfully");
                return RedirectToAction("Positions", "Admin", null);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return View(model);
            }
        }
        #endregion

        #region Grades
        public IActionResult Grades()
        {
            ViewData["PageTitle"] = "Grades";
            return View();
        }

        [HttpGet]
        public IActionResult GetGrades()
        {
            var grades = _gradeService.GetAll();
            return Json(grades);
        }

        [HttpGet]
        public IActionResult EditGrade(Int16 id)
        {
            ViewData["PageTitle"] = "Grades";

            var grade = _gradeService.GetById(id);
            return View(grade);
        }

        [HttpPost]
        public IActionResult EditGrade(GradeDto model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                _gradeService.Update(model);
                _notyf.Success("Grade updated successfully");
                return RedirectToAction("Grades", "Admin", null);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult AddGrade()
        {
            ViewData["PageTitle"] = "Grades";
            return View(new GradeDto());
        }

        [HttpPost]
        public IActionResult AddGrade(GradeDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                _gradeService.Add(model);
                _notyf.Success("Grade added successfully");
                return RedirectToAction("Grades", "Admin", null);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return View(model);
            }
        }
        #endregion

        #region BaseActivity
        public IActionResult ProjectActivities()
        {
            ViewData["PageTitle"] = "Project Activities";
            return View();
        }

        [HttpGet]
        public IActionResult GetProjectActivities()
        {
            var activities = _baseActivityService.GetAll();
            return Json(activities);
        }

        [HttpGet]
        public IActionResult EditProjectActivity(int id)
        {
            ViewData["PageTitle"] = "Project Activitiy";

            var grade = _baseActivityService.GetById(id);
            return View(grade);
        }

        [HttpPost]
        public IActionResult EditProjectActivity(BaseActivityDto model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                _baseActivityService.Update(model);
                _notyf.Success("Activity updated successfully");
                return RedirectToAction("ProjectActivities", "Admin", null);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult AddProjectActivity()
        {
            ViewData["PageTitle"] = "Project Activitiy";
            return View(new BaseActivityDto());
        }

        [HttpPost]
        public IActionResult AddProjectActivity(BaseActivityDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                _baseActivityService.Add(model);
                _notyf.Success("Activity added successfully");
                return RedirectToAction("ProjectActivities", "Admin", null);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return View(model);
            }
        }
        #endregion

    }
}
