using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MTRS.Core.DTOs;
using MTRS.Core.Enums;
using MTRS.Services.Interfaces;
using MTRS.Web.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MTRS.Web.Controllers
{
    [Authorize]
    public class TimeSheetController : BaseController
    {
        private readonly ILogger<TimeSheetController> _logger;
        private readonly ITimeSheetService _timeSheetService;
        private readonly ITimeSheetDetailsService _timesheetDetailsService;
        private readonly ITimeSheetApprovalService _timesheetApprovalService;
        private readonly EmailSettings _emailSettings;
        private readonly EmailService _emailService;
        private readonly IUserService _userService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IPLDataService _pLDataService;
        private readonly INotyfService _notyf;
        private readonly IConfiguration _configuration;

        public TimeSheetController(ILogger<TimeSheetController> logger,
            ITimeSheetService timeSheetService, INotyfService notyfService,
            ITimeSheetDetailsService timeSheetDetailsService,
            ITimeSheetApprovalService timeSheetApprovalService,
            IUserService userService,
            IOptions<EmailSettings> emailSettings,
            IWebHostEnvironment environment,
            IPLDataService pLDataService,
            IConfiguration configuration)
        {
            _hostingEnvironment = environment;
            _emailSettings = emailSettings.Value;
            _userService = userService;
            _emailService = new EmailService(_emailSettings, _userService);
            _logger = logger;
            _timeSheetService = timeSheetService;
            _notyf = notyfService;
            _timesheetDetailsService = timeSheetDetailsService;
            _timesheetApprovalService = timeSheetApprovalService;
            _pLDataService = pLDataService;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            ViewData["PageTitle"] = "Submit TimeSheet";
            return View();
        }

        [HttpGet]
        public IActionResult MyTimeSheets()
        {
            ViewData["PageTitle"] = "My TimeSheets";
            return View();
        }

        [HttpGet]
        public IActionResult GetPendingTimeSheets()
        {
            long employeeId = long.Parse(this.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var timesheets = _timeSheetService.GetPendingTimeSheets(employeeId).OrderByDescending(x => x.FromDate);
            return Json(timesheets);
        }

        [HttpGet]
        public IActionResult GetNewByUser()
        {
            long userId = long.Parse(this.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var timesheets = _timeSheetService.GetNewByUserId(userId).OrderByDescending(x => x.FromDate);
            return Json(timesheets);
        }

        [HttpGet]
        public IActionResult GetDetailsByTimeSheetId(long timesheetId)
        {
            var timesheetDetails = _timesheetDetailsService.GetByTimeSheetId(timesheetId);
            return Json(timesheetDetails);
        }

        [HttpPost]
        public IActionResult SaveSheetActivities([FromBody]List<TimeSheetDetailsDto> activityDetails)
        {
            bool result = false;
            if (activityDetails.Any())
            { 
                result = _timesheetDetailsService.SaveSheetActivities(activityDetails, TimeSheetStatus.Draft);
            }
            return Json(result);
        }

        [HttpPost]
        public IActionResult SubmitSheetActivities([FromBody] List<TimeSheetDetailsDto> activityDetails)
        {
            bool result = false;
            if (activityDetails.Any())
            {
                result = _timesheetDetailsService.SaveSheetActivities(activityDetails, TimeSheetStatus.Submited);
            }
            return Json(result);
        }

        [HttpPost]
        public IActionResult ResubmitSheetActivities([FromBody] List<TimeSheetDetailsDto> activityDetails)
        {
            bool result = false;
            if (activityDetails.Any())
            {
                result = _timesheetDetailsService.SaveSheetActivities(activityDetails, TimeSheetStatus.Resubmited);
            }
            return Json(result);
        }

        [HttpGet]
        public IActionResult UserTimeSheetHistory()
        {
            ViewData["PageTitle"] = "My TimeSheets History";
            return View();
        }

        [HttpGet]
        public IActionResult GetUserTimeSheetHistory(string sheetDate)
        {
            var startDate = DateTime.Parse(sheetDate);
            long userId = long.Parse(this.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var timesheet = _timeSheetService.GetByDate(userId, startDate);
            if (timesheet != null)
            {
                var timesheetDetails = _timesheetDetailsService.GetByTimeSheetId(timesheet.Id);
                ViewBag.Approvals = _timesheetApprovalService.GetByTimeSheetId(timesheet.Id);
                return Json(timesheetDetails);
            }
            return Json("No date found!");
        }

        [HttpGet]
        public IActionResult GetHistory()
        {
            long userId = long.Parse(this.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var timesheets = _timeSheetService.GetHistory(userId).OrderByDescending(x=>x.FromDate).ToList();
            return Json(timesheets);
        }

        [HttpGet]
        public IActionResult IsOpenOpportunity(string opportunityId)
        {
            var connection = _configuration.GetConnectionString("PLDataConnection");
            _pLDataService.Connection = connection;
            var isValid = _pLDataService.IsOpenOpportunity(opportunityId);
            return Json(isValid);
        }

        [HttpGet]
        public IActionResult GetTimeSheetHistory(long userId)
        {
            var timesheets = _timeSheetService.GetHistory(userId).OrderByDescending(x => x.FromDate);
            return Json(timesheets);
        }

        [HttpGet]
        public IActionResult GetApprovalTimeLine(long timesheetId)
        {
            ViewBag.TimeSheet = _timeSheetService.GetById(timesheetId);
            ViewBag.Approvals = _timesheetApprovalService.GetByTimeSheetId(timesheetId);
            ViewBag.Status = _timeSheetService.GetStatus(timesheetId);

            return PartialView("_approvalTimeline");
        }

        [HttpGet]
        public IActionResult ApproveTimeSheet(long managerId, long timesheetId, bool isApproved, string comments)
        {
            var timesheet = _timeSheetService.GetById(timesheetId);
            var result = _timeSheetService.ApproveTimeSheetProjectManager(managerId, timesheetId, isApproved, comments);

            if (result)
            {
                SendEmail(timesheet.UserId, managerId, isApproved);
            }

            return Json(result);
        }

        [HttpGet]
        public IActionResult ApproveTimeSheetManager(long timesheetId, bool isApproved, string comments)
        {
            long managerId = long.Parse(this.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var timesheet = _timeSheetService.GetById(timesheetId);

            var result = _timeSheetService.ApproveTimeSheetManager(managerId, timesheetId, isApproved, comments);
            if (result)
            {
                SendEmail(timesheet.UserId, managerId, isApproved);
            }

            return Json(result);
        }

        [HttpPost]
        public IActionResult AddTimeSheet([FromBody] TimeSheetDto sheet)
        {
            long userId = long.Parse(this.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            sheet.UserId = userId;

            var result = _timeSheetService.AddTimeSheet(sheet);
            return Json(result);
        }

        private void SendEmail(long userId, long managerId, bool IsApproved)
        {
            var path = Path.Combine(_hostingEnvironment.WebRootPath, "emailtemplates/approveEmail.html");
            _emailService.Send(userId, managerId, IsApproved, "MTRS Notification", path);
        }
    }
}
