using MTRS.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MTRS.Core.DTOs
{
    public class TimeSheetApprovalDto : BaseDto<long>
    {
        public long TimeSheetId { get; set; }
        public TimeSheetDto TimeSheet { get; set; }

        public long? ManagerId { get; set; }
        public UserDto Manager { get; set; }

        public string ManagerName { get; set; }

        public DateTime TransactionDate { get; set; }

        public string Comments { get; set; }

        public long? UserId { get; set; }
        public UserDto Employee { get; set; }

        public TimeSheetStatus Status { get; set; }
    }
}
