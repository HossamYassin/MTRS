using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MTRS.Core.Entities
{
    public class Department : BaseEntity<Int16>
    {
        [MaxLength(100)]
        [Required]
        public string Name { get; set; }

        public long ManagerId { get; set; }
        public User User { get; set; }
    }
}
