using System;
using System.Collections.Generic;
using System.Text;

namespace MTRS.Core.DTOs
{
    public class UserDashboardDto
    {
        public int PendingTimesheets { get; set; }

        public int PendingApprovals { get; set; }

        public int MyProjects { get; set; }

        public int LastMonthHours { get; set; }
    }
}
