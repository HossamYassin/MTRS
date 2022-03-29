using MTRS.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MTRS.Core.Entities
{
    public class TimeSheet : BaseEntity<long>
    {
        [ForeignKey("User")]
        public long UserId { get; set; }
        public User User { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int? TotalHours { get; set; }

        [Required]
        public TimeSheetStatus StatusId { get; set; }

        public DateTime? SubmitedOn { get; set; }

        public DateTime? ApprovalDate { get; set; }

        public string Comments { get; set; }
    }
}
