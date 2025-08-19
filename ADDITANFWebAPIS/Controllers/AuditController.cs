using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TANFInterfaces.Contracts;
using TANFInterfaces.Services;
using TANFModels.enums;
using TANFModels.Helper;
using TANFModels.Models;

namespace ADDITANFWebAPIS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditController : ControllerBase
    {
        private readonly IAuditService _auditservice;
        private readonly ILookupService _lookupService;
        private readonly ISecurityService _securityService;
        private IConfiguration _configuration;
        public AuditController(IAuditService auditservice, IConfiguration configuration, ILookupService lookupService, ISecurityService securityService) 
        {
            _auditservice = auditservice;
            _configuration = configuration;
            _lookupService = lookupService;
            _securityService = securityService;
        }
        //Search standard cases for audit assignments!
        //POST/AuditSearch
        [HttpPost("auditcasesearch")]
        public async Task<GridResult<AdvanceSearchViewModel>> AuditSearch(CaseAdvanceSearchCriteria criteria)
        {
            var UserInformation = _lookupService.GetUserList(User.Identity.Name.Replace("DHRAL\\", ""), 0).Result;
            var Userdetails = UserInformation.FirstOrDefault();
            criteria.isConfidentialAllowed = Userdetails.RoleID == 3 || Userdetails.RoleID == 5 || Userdetails.RoleID == 6 || Userdetails.RoleID == 7;
            var UserCredentials = _securityService.GetUserEntitlements(Userdetails.ID);
            if (Userdetails.RoleID == (int)SecurityEnums.CountyDirector || Userdetails.RoleID == (int)SecurityEnums.CountySupervisor || Userdetails.RoleID == (int)SecurityEnums.CountyWorker)
            {
                if (criteria.CountyID == null)
                {
                    var CaseInformation = _auditservice.Search(criteria).Result.Data.FirstOrDefault();
                    if (CaseInformation != null)
                    {
                        criteria.CountyID = CaseInformation.CountyID;
                    }
                }
                if (Userdetails.CountyID == criteria.CountyID)
                {
                    criteria.UserCredentials = _securityService.GetUserEntitlements(Userdetails.ID);
                }
                else
                {
                    criteria.UserCredentials = _securityService.GetUserEntitlements(Userdetails.ID).Replace("IMPORT_CASE,", "").Replace("CONFIDENTIAL_CASE_IMPORT,", "");
                }
            }
            else
            {
                criteria.UserCredentials = _securityService.GetUserEntitlements(Userdetails.ID);
            }
            if (Userdetails.RoleID == 3)
            {
                criteria.userCountyId = Userdetails.CountyID;
                criteria.CaseWorkerID = Userdetails.ID;               
            }
            criteria.userCountyId = Userdetails.CountyID;
            criteria.CaseWorkerID = Userdetails.ID;           
            return await _auditservice.Search(criteria);
        }
        // Assiging the cases for Audit!
        //Post/auditcaseassignment
        [HttpPost("assignment")]
        public void auditcaseassignment(AuditCaseInfo audit)
        {
            foreach (var item in audit.checkArray)
            {
                AuditCaseInfo CCM = new AuditCaseInfo();
                CCM.CaseSSN = item.ToStr();
                _auditservice.auditcaseassignment(CCM);
            }
        }
        //Post/auditcaseremoveassignment
        [HttpPost("removeassignment")]
        public void auditcaseremoveassignment(AuditCaseInfo audit)
        {
            foreach (var item in audit.checkArray)
            {
                AuditCaseInfo CCM = new AuditCaseInfo();
                CCM.CaseSSN = item.ToStr();
                _auditservice.auditcaseremoveassignment(CCM);
            }
        }
        //Post/assignedauditcasesearch
        [HttpPost("assignedauditcasesearch")]
        public async Task<GridResult<AdvanceSearchViewModel>> assignedauditcasesearch(CaseAdvanceSearchCriteria criteria)
        {
            return await _auditservice.AssignedCasesSearch(criteria);
        }


    }
}
