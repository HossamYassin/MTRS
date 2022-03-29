using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MTRS.Core.Entities
{
    public class Country : BaseEntity<Int16>
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
