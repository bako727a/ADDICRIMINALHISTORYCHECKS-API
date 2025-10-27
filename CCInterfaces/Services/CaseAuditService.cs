using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCInterfaces.Contracts;
using CCModels.Helper;
using CCModels.Models;
using CCRepo.Repos;

namespace CCInterfaces.Services
{
    public class CaseAuditService : ICaseAuditService
    {
        private readonly CaseAuditRepo _repository;
        public CaseAuditService(CaseAuditRepo repository)
        {
            _repository = repository;
        }
        public async Task<GridResult<CaseAuditLogViewModel>> AuditSearch(CaseAuditLog criteria)
        {
            var list = await _repository.GetCaseAuditList(criteria);
            return new GridResult<CaseAuditLogViewModel>()
            {
                Data = list,
                PageIndex = criteria.PageIndex,
                PageSize = criteria.PageSize,
                TotalRecords = list.Count()
            };
        }
    }
}
