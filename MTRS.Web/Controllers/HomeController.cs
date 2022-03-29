using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MTRS.Web.Models;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Claims;
using MTRS.DAL.Interfaces;
using System.Linq.Expressions;
using MTRS.Services.Interfaces;
using MTRS.Core.DTOs;
using MTRS.Core.Entities;

namespace MTRS.Web.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICustomerService _customerService;
        private readonly IUserService _userService;

        public HomeController(ILogger<HomeController> logger,
            ICustomerService customerService,
            IUserService userService)
        {
            _userService = userService;
            _customerService = customerService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewData["PageTitle"] = "My Dashboard";
            return View();
        }

        [HttpGet]
        public IActionResult GetDashboard()
        {
            long userId = long.Parse(this.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var dashboard = _userService.GetDashboard(userId);

            return Json(dashboard);
        }


        [Authorize(Roles = "Admin")]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
