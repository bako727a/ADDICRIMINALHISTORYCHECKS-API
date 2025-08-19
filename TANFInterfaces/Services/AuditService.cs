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
    public class AuditService : IAuditService
    {
        private readonly AuditRepo _auditRepo;
        public AuditService(AuditRepo auditRepo) { _auditRepo = auditRepo; }
        public async Task<GridResult<AdvanceSearchViewModel>> Search(CaseAdvanceSearchCriteria criteria)
        {
            var list =await _auditRepo.GetAuditList(criteria);
            return new GridResult<AdvanceSearchViewModel>()
            {
                Data = list,
                PageIndex = criteria.PageIndex,
                PageSize = criteria.PageSize,
                TotalRecords = list.Count()
            };
        }
        //AssignedCasesSearch
        public async Task<GridResult<AdvanceSearchViewModel>> AssignedCasesSearch(CaseAdvanceSearchCriteria criteria)
        {
            var list =await _auditRepo.GetAssignedAuditList(criteria);
            return new GridResult<AdvanceSearchViewModel>()
            {
                Data = list,
                PageIndex = criteria.PageIndex,
                PageSize = criteria.PageSize,
                TotalRecords = list.Count()
            };
        }
        public void auditcaseassignment(AuditCaseInfo audit)
        {
             _auditRepo.PostAuditCaseAssignment(audit);
        }
        public void auditcaseremoveassignment(AuditCaseInfo audit)
        {
            _auditRepo.PostAuditCaseRemoveAssignment(audit);
        }
    }
}
