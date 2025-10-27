using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CCInterfaces.Contracts;
using CCModels.Helper;
using CCModels.Models;

namespace ADDICCWebAPIS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CaseAuditLogsController : ControllerBase
    {
        private readonly ILookupService lookupService;
        private readonly ICaseAuditService _caseAuditService;
        public CaseAuditLogsController(ILookupService lookupService, ICaseAuditService caseAuditService)
        {
            this.lookupService = lookupService;
            _caseAuditService = caseAuditService;
        }
        [HttpPost("GetCaseAuditLogs")]
        public async Task<GridResult<CaseAuditLogViewModel>> GetCaseAuditLogs(CaseAuditLog CaseAuditLog)
        {
            CaseAuditLog.FromDate = CaseAuditLog.FromDate.ToLocalTime();
            CaseAuditLog.ToDate = CaseAuditLog.ToDate.ToLocalTime();
            CaseAuditLog.LoginUserID = User.Identity.Name.Replace("DHRAL\\", "");
            return await _caseAuditService.AuditSearch(CaseAuditLog);
        }
    }
}
