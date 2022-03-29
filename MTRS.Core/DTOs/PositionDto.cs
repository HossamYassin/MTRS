using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MTRS.Core.DTOs
{
    public class PositionDto
    {
        public Int16 Id { get; set; }

        [MaxLength(100)]
        [Required(ErrorMessage = "The position name is required")]
        public string Name { get; set; }
    }
}
