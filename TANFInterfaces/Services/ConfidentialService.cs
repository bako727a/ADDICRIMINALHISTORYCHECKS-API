using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TANFInterfaces.Contracts;
using TANFModels.Helper;
using TANFModels.Models;
using TANFRepo.Repos;

namespace TANFInterfaces.Services
{
    public class ConfidentialService : IConfidentialService
    {
        private readonly ConfidentialRepo _confidentialRepo;
        public ConfidentialService(ConfidentialRepo confidentialRepo)
        {
            _confidentialRepo = confidentialRepo ?? throw new ArgumentNullException(nameof(confidentialRepo));
        }
        public void confidentialcaseassignment(ConfidentialCaseInfo Confidential)
        {
            _confidentialRepo.PostConfidentialCaseAssignment(Confidential);
        }
        public void removeconfidentialcaseassignment(ConfidentialCaseInfo Confidential)
        {
            _confidentialRepo.PostremoveConfidentialCaseAssignment(Confidential);
        }
        public async Task<GridResult<AdvanceSearchViewModel>> ConfidentialSearch(CaseAdvanceSearchCriteria criteria)
        {
            var list =await _confidentialRepo.ConfidentialSearch(criteria);
            return new GridResult<AdvanceSearchViewModel>()
            {
                Data = list,
                PageIndex = criteria.PageIndex,
                PageSize = criteria.PageSize,
                TotalRecords = list.Count()
            };
        }
    }
}
