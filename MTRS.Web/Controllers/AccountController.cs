using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MTRS.Core.Entities;
using MTRS.Web.Auth;
using MTRS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTRS.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager _userManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager userManager, ILogger<AccountController> logger)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public IActionResult Login()
        {
            return View(new LoginModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel form)
        {
            if (!ModelState.IsValid)
                return View(form);

            try
            {
                //TODO authunticate user agenist AD

                //authenticate
                var user = new User()
                {
                    UserName = form.UserName
                };
                var result = await _userManager.SignIn(this.HttpContext, user);

                if (result)
                    return RedirectToAction("Index", "Home", null);
                else
                {
                    ModelState.AddModelError("summery", "Your email or password is incorrect");
                    return View(form);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                ModelState.AddModelError("summery", "Your email or password is incorrect");
                return View(form);
            }
        }

        public IActionResult LogOut()
        {
            _userManager.SignOut(this.HttpContext);
            return RedirectToAction("Login", "Account", null);
        }
    }
}
