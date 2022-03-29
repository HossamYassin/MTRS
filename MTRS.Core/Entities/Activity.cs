using MTRS.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MTRS.Core.Entities
{
    public class Activity : BaseEntity<long>
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [ForeignKey("BaseActivity")]
        public int? BaseActivityId { get; set; }
        public BaseActivity BaseActivity { get; set; }

        [ForeignKey("Project")]
        public long? ProjectId { get; set; }
        public Project Project { get; set; }

        public int? PlannedHours { get; set; }

        public int? LoggedHours { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public ActivityType Type { get; set; }
    }
}
