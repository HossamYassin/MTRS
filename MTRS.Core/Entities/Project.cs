using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MTRS.Core.Entities
{
    public class Project : BaseEntity<long>
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(20)]
        public string Code { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string Description { get; set; }

        [ForeignKey("Manager")]
        [Required(ErrorMessage = "The Manager is required")]
        public long ManagerId { get; set; }
        public User Manager { get; set; }

        [ForeignKey("Customer")]
        [Required(ErrorMessage = "The Customer is required")]
        public long CustomerId { get; set; }
        public Customer Customer { get; set; }

        public bool IsCompleted { get; set; }

        public virtual ICollection<Activity> Activities { get; set; }
    }
}
