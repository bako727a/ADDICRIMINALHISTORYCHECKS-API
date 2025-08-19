using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TANFModels.Helper;
using TANFModels.Models;

namespace TANFInterfaces.Contracts
{
    public interface IConfidentialService
    {
        void confidentialcaseassignment(ConfidentialCaseInfo Confidential);
        void removeconfidentialcaseassignment(ConfidentialCaseInfo Confidential);
        Task<GridResult<AdvanceSearchViewModel>> ConfidentialSearch(CaseAdvanceSearchCriteria criteria);
    }
}
