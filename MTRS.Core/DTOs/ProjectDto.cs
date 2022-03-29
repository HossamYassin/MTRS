using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MTRS.Core.DTOs
{
    public class ProjectDto : BaseDto<long>
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [MaxLength(20)]
        public string Code { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "The Manager is required")]
        public long ManagerId { get; set; }
        public UserDto Manager { get; set; }

        [Required(ErrorMessage = "The Customer is required")]
        public long CustomerId { get; set; }
        public CustomerDto Customer { get; set; }

        public bool IsCompleted { get; set; }

        public virtual ICollection<ActivityDto> Activities { get; set; }
    }
}
