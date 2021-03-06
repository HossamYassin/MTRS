using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MTRS.Core.DTOs
{
    public class UserDto : BaseDto<long>
    {
        [MaxLength(20)]
        public string Number { get; set; }

        public string UserName { get; set; }

        [Required]
        [MaxLength(20)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(20)]
        public string LastName { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(20)]
        public string Phone { get; set; }

        [MaxLength(10)]
        public string Mobile { get; set; }

        public Int16? DepartmentId { get; set; }
        public DepartmentDto Department { get; set; }

        public Int16? PositionId { get; set; }
        public PositionDto Position { get; set; }

        public Int16? GradeId { get; set; }
        public GradeDto Grade { get; set; }

        public long? ManagerId { get; set; }

        [Required]
        public bool IsAdmin { get; set; }

        [Required]
        public bool IsManager { get; set; }

        [Required]
        public bool IsProjectManager { get; set; }

        [Required]
        public bool IsSuperManager { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public bool AllowTimeSheet { get; set; }

        public DateTime? ActivatedDate { get; set; }

    }
}
