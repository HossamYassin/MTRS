using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MTRS.Core.DTOs
{
    public class ActivityUserDto : BaseDto<long>
    {
        public long ActivityId { get; set; }
        public ActivityDto Activity { get; set; }

        public long UserId { get; set; }
        public UserDto User { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
