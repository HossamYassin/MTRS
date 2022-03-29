using Coravel.Invocable;
using Microsoft.Extensions.Logging;
using MTRS.Core.DTOs;
using MTRS.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTRS.Web.Services
{
    public class GenerateTimeSheetService : IInvocable
    {
        private readonly ITimeSheetService _timesheetService;
        private readonly IUserService _userService;
        private readonly ILogger<GenerateTimeSheetService> _logger;

        public GenerateTimeSheetService(ITimeSheetService timeSheetService,
            IUserService userService,
            ILogger<GenerateTimeSheetService> logger)
        {
            _logger = logger;
            _userService = userService;
            _timesheetService = timeSheetService;
        }

        public Task Invoke()
        {
            if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday)
            {
                _logger.LogInformation("Generate timesheet service started at: " + DateTime.Now.ToString());

                var employees = _userService.GetAll().Where(x => x.IsActive == true && x.AllowTimeSheet == true).ToList();

                foreach (var employee in employees)
                {
                    var timesheet = _timesheetService.GetTimeSheetByDate(employee.Id, DateTime.Now.AddDays(1));
                    if (timesheet == null)
                    {
                        var employeeTimesheet = new TimeSheetDto()
                        {
                            FromDate = DateTime.Now.AddDays(1),
                            UserId = employee.Id,
                            StatusId = Core.Enums.TimeSheetStatus.Draft,
                            ToDate = DateTime.Now.AddDays(7)
                        };

                        _timesheetService.Add(employeeTimesheet);
                    }
                }

            }

            _logger.LogInformation("timesheeets generated for all employees at: " + DateTime.Now.ToString());
            return Task.CompletedTask;
        }
    }
}