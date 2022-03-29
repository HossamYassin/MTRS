using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MTRS.Core.DTOs
{
    public class CustomerDto : BaseDto<long>
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Phone]
        public string Phone { get; set; }

        public string ContactName { get; set; }

        public string ContactMobile { get; set; }

        public string Address { get; set; }

        public long? CountryId { get; set; }

        [EmailAddress]
        public string Email { get; set; }

    }
}
