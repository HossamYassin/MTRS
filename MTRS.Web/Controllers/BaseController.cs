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
using Microsoft.AspNetCore.Mvc.Filters;

namespace MTRS.Web.Controllers
{
    public class BaseController : Controller
    {
        private string _userName;
        private List<string> _roles;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _userName = filterContext.HttpContext.User?.FindFirst(ClaimTypes.Name)?.Value;
            _roles = filterContext.HttpContext.User?.FindAll(ClaimTypes.Role)?.Select(x => x.Value).ToList();

            ViewBag.UserName = _userName;
            ViewBag.Roles = _roles;
        }
    }
}
