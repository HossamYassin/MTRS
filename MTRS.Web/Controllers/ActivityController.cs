using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MTRS.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MTRS.Web.Controllers
{
    [Authorize]
    public class ActivityController : BaseController
    {
        private readonly IActivityService _activityService;
        private readonly ILogger<ActivityController> _logger;

        public ActivityController(ILogger<ActivityController> logger,
             IActivityService activityService)
        {
            _logger = logger;
            _activityService = activityService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetGeneral()
        {
            var activities = _activityService.GetGeneral();
            return Json(activities);
        }

        [HttpGet]
        public IActionResult GetUserActivities(long projectId)
        {
            long userId = long.Parse(this.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var activities = _activityService.GetUserActivities(projectId, userId);
            return Json(activities);
        }

        [HttpGet]
        public IActionResult GetUserProjectActivity(long projectId)
        {
            long userId = long.Parse(this.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var activities = _activityService.GetUserProjectActivity(projectId, userId);
            return Json(activities);
        }

        [HttpGet]
        public IActionResult GetProjectActivity(long projectId, long userId)
        {
            var activities = _activityService.GetUserProjectActivity(projectId, userId);
            return Json(activities);
        }
    }
}
