using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MTRS.Core.DTOs
{
    public class DepartmentDto
    {
        public Int16 Id { get; set; }

        [MaxLength(100)]
        [Required]
        public string Name { get; set; }

        [Required(ErrorMessage = "The Manager is required")]
        public long ManagerId { get; set; }
        public UserDto User { get; set; }
    }
}
