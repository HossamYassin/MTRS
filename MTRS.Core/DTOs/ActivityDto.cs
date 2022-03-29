using MTRS.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MTRS.Core.DTOs
{
    public class ActivityDto
    {
        public long Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public int? BaseActivityId { get; set; }
        public BaseActivityDto BaseActivity { get; set; }

        public long? ProjectId { get; set; }
        public ProjectDto Project { get; set; }

        public int? PlannedHours { get; set; }

        public int? LoggedHours { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public ActivityType Type { get; set; }
    }
}
