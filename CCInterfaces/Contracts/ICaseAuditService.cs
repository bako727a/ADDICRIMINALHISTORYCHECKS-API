using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCModels.Helper;
using CCModels.Models;

namespace CCInterfaces.Contracts
{
    public interface ICaseAuditService
    {
        Task<GridResult<CaseAuditLogViewModel>> AuditSearch(CaseAuditLog CaseAuditLog);
    }
}
