using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MTRS.Core.Entities
{
    public class ActivityUser : BaseEntity<long>
    {
        [ForeignKey("Activity")]
        public long ActivityId { get; set; }
        public Activity Activity { get; set; }

        [ForeignKey("User")]
        public long UserId { get; set; }
        public User User { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
