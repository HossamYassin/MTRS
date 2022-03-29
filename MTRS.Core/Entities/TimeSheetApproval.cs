using MTRS.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MTRS.Core.Entities
{
    public class TimeSheetApproval : BaseEntity<long>
    {
        [ForeignKey("TimeSheet")]
        public long TimeSheetId { get; set; }
        public TimeSheet TimeSheet { get; set; }

        [ForeignKey("Manager")]
        public long? ManagerId { get; set; }
        public User Manager { get; set; }

        public string ManagerName { get; set; }

        public DateTime TransactionDate { get; set; }

        public string Comments { get; set; }
        
        [ForeignKey("Employee")]
        public long? UserId { get; set; }
        public User Employee { get; set; }

        public TimeSheetStatus Status { get; set; }
    }
}
