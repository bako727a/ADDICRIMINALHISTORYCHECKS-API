using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CCInterfaces.Contracts;
using CCModels.enums;
using CCModels.Helper;
using CCModels.Models;

namespace ADDICCWebAPIS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CaseManagementController : ControllerBase
    {
        private readonly ICaseManagementService _caseManagement;
        private readonly ILookupService _lookupService;
        private readonly ISecurityService _securityService;
        private IConfiguration _configuration { get; }

        public CaseManagementController(ICaseManagementService caseManagement, IConfiguration configuration, ILookupService lookupService, ISecurityService securityService)
        {
            _caseManagement = caseManagement;
            _configuration = configuration;
            _lookupService = lookupService;
            _securityService = securityService;
        }
        [HttpOptions]
        [Route("{*path}")]
        public IActionResult Options()
        {
            return Ok();
        }
        //Post/Search
        [Authorize]
        [HttpPost]
        [Route("casemanagementsearch")]
        public async Task<GridResult<AdvanceSearchViewModel>> casemanagementsearch([FromBody] CaseManagementSearchCriteria criteria)
        {
            var windowsUser = HttpContext.User.Identity;
            var UserInformation = _lookupService.GetUserList(windowsUser.Name.Replace("DHRAL\\", ""), 0).Result.FirstOrDefault();
            criteria.isConfidentialAllowed = UserInformation.RoleID == 3 || UserInformation.RoleID == 5 || UserInformation.RoleID == 6 || UserInformation.RoleID == 7;
            var UserCredentials = _securityService.GetUserEntitlements(UserInformation.ID);
            if (UserInformation.RoleID == (int)SecurityEnums.CountyDirector || UserInformation.RoleID == (int)SecurityEnums.CountySupervisor || UserInformation.RoleID == (int)SecurityEnums.CountyWorker)
            {
                if (criteria.CountyID == null)
                {
                    var CaseInformation = _caseManagement.Search(criteria).Result.Data.FirstOrDefault();
                    if (CaseInformation != null)
                    {
                        criteria.CountyID = CaseInformation.CountyID;
                    }
                }
                if (UserInformation.CountyID == criteria.CountyID)
                {
                    criteria.UserCredentials = _securityService.GetUserEntitlements(UserInformation.ID);
                }
                else
                {
                    criteria.UserCredentials = _securityService.GetUserEntitlements(UserInformation.ID).Replace("IMPORT_CASE,", "").Replace("CONFIDENTIAL_CASE_IMPORT,", "");
                }
            }
            else
            {
                criteria.UserCredentials = _securityService.GetUserEntitlements(UserInformation.ID);
            }
            if (UserInformation.RoleID == 3)
            {
                criteria.userCountyId = UserInformation.CountyID;
                criteria.CaseWorkerID = UserInformation.ID;
                criteria.LoginUserRoleID = UserInformation.RoleID;
            }
            criteria.userCountyId = UserInformation.CountyID;
            criteria.CaseWorkerID = UserInformation.ID;
            criteria.LoginUserRoleID = UserInformation.RoleID;
            return await _caseManagement.Search(criteria);
        }


        //Get/documentrecordinfo
        [HttpGet("documentrecordinfo/{DocumentID}")]
        public async Task<Document> documentrecordinfo(int DocumentID)
        {
            // var user = User.Identity.Name.Replace("DHRAL\\", "");
            // var userinfo = _lookupService.GetUserList(user, 0).Result.FirstOrDefault();
            // return await _caseManagement.documentrecordinfo(DocumentID, userinfo.ID);
            return await _caseManagement.documentrecordinfo(DocumentID, 1);
        }
        //Get/deletedocument
        [HttpGet("deletedocument/{DocumentID}")]
        public async Task deletedocumentAsync(int DocumentID)
        {
            var UserInformation = _lookupService.GetUserList(User.Identity.Name.Replace("DHRAL\\", ""), 0).Result.FirstOrDefault();

            if (UserInformation.RoleID == 1 || UserInformation.RoleID == 2 || UserInformation.RoleID == 6)
            {
                throw new ArgumentException("UnAuthorized");
            }
            CaseManagementSearchCriteria criteria = new CaseManagementSearchCriteria();
            var Document = await _caseManagement.documentrecordinfo(DocumentID, UserInformation.ID);
            string filepath = Document.FilePath;
            System.IO.File.Delete(filepath);
            _caseManagement.deleterecordinfo(DocumentID);
        }
        //Case notes functionality!
        //Get/CaseRecordInfo
        [HttpGet("CaseRecord/{CaseID}")]
        public async Task<CaseInfo> CaseRecordInfo(int CaseID)
        {
            return await _caseManagement.caserecordinfo(CaseID);
        }
        //Get/CaseNotesInfo
        [HttpGet("CaseNotesInfo/{ID}")]
        public async Task<IEnumerable<CaseNote>> CaseNotesInfo(int ID)
        {
            return await _caseManagement.GetCasenotesInfo(ID);
        }
        //Post/saveCaseNotes
        [HttpPost("saveCaseNotes")]
        public void saveCaseNotes(CaseInfo Case)
        {
            var user = User.Identity.Name.Replace("DHRAL\\", "");
            var userinfo = _lookupService.GetUserList(user, 0).Result.FirstOrDefault();
            Case.AuditAssignedUserID = userinfo.ID;
            _caseManagement.SaveCaseNotes(Case);
        }
        //Post/Case registration 
        [HttpPost("CaseRegistration")]
        public Boolean CaseRegistration(CaseRegistration CaseRegistration)
        {
            try
            {
                _caseManagement.CaseRegistration(CaseRegistration);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }            
        }
    }
}
