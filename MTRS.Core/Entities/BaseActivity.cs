using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MTRS.Core.Entities
{
    public class BaseActivity : BaseEntity<int>
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
