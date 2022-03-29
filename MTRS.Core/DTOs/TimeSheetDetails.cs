using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MTRS.Core.DTOs
{
    public class TimeSheetDetailsDto : BaseDto<long>
    {
        public long TimeSheetId { get; set; }
        public TimeSheetDto TimeSheet { get; set; }
        
        public long ActivityId { get; set; }
        public ActivityDto Activity { get; set; }

        public Int16? Sat { get; set; }

        public Int16? SatOver { get; set; }

        public Int16? Sun { get; set; }

        public Int16? SunOver { get; set; }

        public Int16? Mon { get; set; }

        public Int16? MonOver { get; set; }

        public Int16? Tu { get; set; }

        public Int16? TuOver { get; set; }

        public Int16? We { get; set; }

        public Int16? WeOver { get; set; }

        public Int16? Th { get; set; }

        public Int16? ThOver { get; set; }

        public Int16? Fri { get; set; }

        public Int16? FriOver { get; set; }

        public string Comments { get; set; }

        public string OpportunityId { get; set; }
    }
}
