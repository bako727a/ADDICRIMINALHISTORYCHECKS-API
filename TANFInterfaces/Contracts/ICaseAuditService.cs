using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TANFModels.Helper;
using TANFModels.Models;

namespace TANFInterfaces.Contracts
{
    public interface ICaseAuditService
    {
        Task<GridResult<CaseAuditLogViewModel>> AuditSearch(CaseAuditLog CaseAuditLog);
    }
}
