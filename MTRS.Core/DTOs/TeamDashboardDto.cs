using System;
using System.Collections.Generic;
using System.Text;

namespace MTRS.Core.DTOs
{
    public class TeamDashboardDto
    {
        public int EmployeesCount { get; set; }

        public int PendingTimeSheetCount { get; set; }

        public int TeamProjectsCount { get; set; }
    }
}
