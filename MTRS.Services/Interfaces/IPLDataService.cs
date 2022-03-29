using MTRS.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTRS.Services.Interfaces
{
    public interface IPLDataService
    {
        List<PLDataDto> GetByOpportunityID(string opportunityId);
        List<PLDataDto> GetProjectPLData(string opportunityId, long projectId);
        bool IsOpenOpportunity(string opportunityId);
        string Connection { get; set; }
    }
}
