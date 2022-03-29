using MTRS.Core.DTOs;
using MTRS.DAL.Interfaces;
using MTRS.DAL.Repositories;
using MTRS.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTRS.Services
{
    public class PLDataService : IPLDataService
    {
        private IPLData _plDataRepository;
        private readonly IProjectRepository _projectRepository;

        public PLDataService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public string Connection { get; set; }

        public List<PLDataDto> GetByOpportunityID(string opportunityId)
        {
            _plDataRepository = new PLRepository(this.Connection);
            return _plDataRepository.GetByOpportunityID(opportunityId);
        }

        public List<PLDataDto> GetProjectPLData(string opportunityId, long projectId)
        {
            _plDataRepository = new PLRepository(this.Connection);
            var plDate = _plDataRepository.GetByOpportunityID(opportunityId);
            foreach (var entry in plDate)
            {
                entry.AcctualAmount = _projectRepository.GetAcctuleCostByGrad(entry.Grad,
                    entry.StartDate, entry.EndDate, projectId);
            }

            return plDate;
        }

        public bool IsOpenOpportunity(string opportunityId)
        {
            _plDataRepository = new PLRepository(this.Connection);
            return _plDataRepository.IsOpenOpportunity(opportunityId);
        }
    }
}
