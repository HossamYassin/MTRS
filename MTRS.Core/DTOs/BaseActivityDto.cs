using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MTRS.Core.DTOs
{
    public class BaseActivityDto
    {
        public Int16 Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
