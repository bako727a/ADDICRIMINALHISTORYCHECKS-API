using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCModels.Helper;
using CCModels.Models;

namespace CCInterfaces.Contracts
{
    public interface IAuditService
    {
        Task<GridResult<AdvanceSearchViewModel>> Search(CaseAdvanceSearchCriteria criteria);
        Task<GridResult<AdvanceSearchViewModel>> AssignedCasesSearch(CaseAdvanceSearchCriteria criteria);
        void auditcaseassignment(AuditCaseInfo audit);
        void auditcaseremoveassignment(AuditCaseInfo audit);
    }
}
