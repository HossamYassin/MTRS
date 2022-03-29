using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MTRS.Core.Entities
{
    public class Position : BaseEntity<Int16>
    {
        [MaxLength(100)]
        [Required]
        public string Name { get; set; }
    }
}
