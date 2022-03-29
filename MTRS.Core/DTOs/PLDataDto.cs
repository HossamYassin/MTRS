using System;
using System.Collections.Generic;
using System.Text;

namespace MTRS.Core.DTOs
{
    public class PLDataDto
    {
        public string OpportunityId { get; set; }

        public string AccountName { get; set; }

        public string StageName { get; set; }

        public string ItemName { get; set; }

        public string Grad { get; set; }

        public float Percentage { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public float TotalAmount { get; set; }

        public int AcctualAmount { get; set; }
    }
}
