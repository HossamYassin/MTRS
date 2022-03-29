using MTRS.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTRS.DAL.Interfaces
{
    public interface IPLData
    {
        List<PLDataDto> GetByOpportunityID(string opportunityId);
        bool IsOpenOpportunity(string opportunityId);
    }
}
