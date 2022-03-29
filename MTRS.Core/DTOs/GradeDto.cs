using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MTRS.Core.DTOs
{
    public class GradeDto
    {
        public Int16 Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
