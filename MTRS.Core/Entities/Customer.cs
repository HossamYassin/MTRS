using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MTRS.Core.Entities
{
    public class Customer : BaseEntity<long>
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public string Phone { get; set; }

        public string ContactName { get; set; }

        public string ContactMobile { get; set; }

        public string Address { get; set; }

        public Int16? CountryId { get; set; }

        public string Email { get; set; }

    }
}
