using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CCInterfaces.Contracts;
using CCModels.enums;
using CCModels.Helper;
using CCModels.Models;

namespace ADDICCWebAPIS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class AdvancedSearchController : ControllerBase
    {
        private readonly IAdvanceSearchService _advancedSearch;
        private readonly ILookupService _lookupService;
        private readonly ISecurityService _securityService;
        public AdvancedSearchController(IAdvanceSearchService advancedSearch, ILookupService lookupService, ISecurityService securityService)
        {
            _advancedSearch = advancedSearch;
            _lookupService = lookupService;
            _securityService = securityService;
        }
        string windowsUser = Environment.UserName;
        // POST: api/advsearch/
        [HttpPost("advsearch")]
        public GridResult<AdvanceSearchViewModel> Search(CaseAdvanceSearchCriteria criteria)
        {
           
            if (criteria.ReviewPeriod != null)
            {
                criteria.ReviewDate = criteria.ReviewPeriod.Value.ToShortDateString();
                criteria.ReviewPeriod = null;
            }
            if (criteria.DOB == null)
            {
                criteria.DateOfBirth = DateTime.MinValue;
            }
            var UserInformation = _lookupService.GetUserList(windowsUser, 0).Result;
            criteria.isConfidentialAllowed = UserInformation.FirstOrDefault().RoleID == 3 || UserInformation.FirstOrDefault().RoleID == 5 || UserInformation.FirstOrDefault().RoleID == 6 || UserInformation.FirstOrDefault().RoleID == 7;
            var UserCredentials = _securityService.GetUserEntitlements(UserInformation.FirstOrDefault().ID);
            if (UserInformation.FirstOrDefault().RoleID == (int)SecurityEnums.CountyDirector || UserInformation.FirstOrDefault().RoleID == (int)SecurityEnums.CountySupervisor || UserInformation.FirstOrDefault().RoleID == (int)SecurityEnums.CountyWorker || UserInformation.FirstOrDefault().RoleID == (int)SecurityEnums.Clerical)
            {
                if (criteria.CountyID == null)
                {
                    var CaseInformation = _advancedSearch.AdvancedSearch(criteria).Result.Data.FirstOrDefault();
                    if (CaseInformation != null)
                    {
                        criteria.CountyID = CaseInformation.CountyID;
                    }
                }
                if (UserInformation.FirstOrDefault().CountyID == criteria.CountyID)
                {
                    criteria.UserCredentials = _securityService.GetUserEntitlements(UserInformation.FirstOrDefault().ID);
                }
                else
                {
                    string credentials = _securityService.GetUserEntitlements(UserInformation.FirstOrDefault().ID).Replace("IMPORT_CASE,", "");
                }
            }
            else
            {
                criteria.UserCredentials = _securityService.GetUserEntitlements(UserInformation.FirstOrDefault().ID);
            }
            if (UserInformation.FirstOrDefault().RoleID == 3)
            {
                criteria.userCountyId = UserInformation.FirstOrDefault().CountyID;
                criteria.CaseWorkerID = UserInformation.FirstOrDefault().ID;
                criteria.LoginUserRoleID = Convert.ToInt32( UserInformation.FirstOrDefault().RoleID);
            }
            criteria.DateOfBirth = Convert.ToDateTime(criteria.DOB);
            criteria.DateOfBirth = criteria.DateOfBirth.ToLocalTime();
            criteria.LoginUserRoleID = Convert.ToInt32(UserInformation.FirstOrDefault().RoleID);
            return _advancedSearch.AdvancedSearch(criteria).Result;
        }
        // POST: api/importsearch/
        [HttpPost("importsearch")]
        public GridResult<AdvanceSearchViewModel> Importsearch(CaseAdvanceSearchCriteria criteria)
        {
            if (criteria.ReviewPeriod != null)
            {
                criteria.ReviewDate = criteria.ReviewPeriod.Value.ToShortDateString();
            }
            var UserInformation = _lookupService.GetUserList(windowsUser, 0).Result;
            var UserCredentials = _securityService.GetUserEntitlements(UserInformation.FirstOrDefault().ID);
            criteria.isConfidentialAllowed = UserInformation.FirstOrDefault().RoleID == 3 || UserInformation.FirstOrDefault().RoleID == 5 || UserInformation.FirstOrDefault().RoleID == 6 || UserInformation.FirstOrDefault().RoleID == 7;
            if (UserInformation.FirstOrDefault().RoleID == (int)SecurityEnums.CountyDirector || UserInformation.FirstOrDefault().RoleID == (int)SecurityEnums.CountySupervisor || UserInformation.FirstOrDefault().RoleID == (int)SecurityEnums.CountyWorker || UserInformation.FirstOrDefault().RoleID == (int)SecurityEnums.Clerical)
            {
                if (criteria.CountyID == null)
                {
                    var CaseInformation = _advancedSearch.Importsearch(criteria).Result.Data.FirstOrDefault();
                    if (CaseInformation != null)
                    {
                        criteria.CountyID = CaseInformation.CountyID;
                    }
                }
                if (UserInformation.FirstOrDefault().CountyID == criteria.CountyID)
                {
                    criteria.UserCredentials = _securityService.GetUserEntitlements(UserInformation.FirstOrDefault().ID);
                }
                else
                {
                    criteria.UserCredentials = _securityService.GetUserEntitlements(UserInformation.FirstOrDefault().ID).Replace("IMPORT_CASE,", "").Replace("CONFIDENTIAL_CASE_IMPORT,", "");
                }
            }
            else
            {
                criteria.UserCredentials = _securityService.GetUserEntitlements(UserInformation.FirstOrDefault().ID);
            }
            if (UserInformation.FirstOrDefault().RoleID == 3)
            {
                criteria.userCountyId = UserInformation.FirstOrDefault().CountyID;
                criteria.CaseWorkerID = UserInformation.FirstOrDefault().ID;
                criteria.LoginUserRoleID =Convert.ToInt32( UserInformation.FirstOrDefault().RoleID);
            }
            criteria.LoginUserID = UserInformation.FirstOrDefault().ID;
            criteria.LoginUserRoleID = Convert.ToInt32(UserInformation.FirstOrDefault().RoleID);
            return _advancedSearch.Importsearch(criteria).Result;
        }
        //Get/caserecordinfo
        [HttpGet("caserecordinfo/{id}")]
        public List<AdvanceSearchViewModel> caserecordinfo(int id)
        {
            return _advancedSearch.caserecordinfo(id).Result.ToList();
        }
        //Get/caserecordinfoList
        [HttpGet("caserecordinfoList/{id}")]
        public GridResult<AdvanceSearchViewModel> caserecordinfoList(int id)
        {
            CaseAdvanceSearchCriteria criteria = new CaseAdvanceSearchCriteria();
            var UserInformation = _lookupService.GetUserList(windowsUser, 0).Result;
            criteria.isConfidentialAllowed = UserInformation.FirstOrDefault().RoleID == 3 || UserInformation.FirstOrDefault().RoleID == 5 || UserInformation.FirstOrDefault().RoleID == 6 || UserInformation.FirstOrDefault().RoleID == 7;
            var UserCredentials = _securityService.GetUserEntitlements(UserInformation.FirstOrDefault().ID);
            if (UserInformation.FirstOrDefault().RoleID == (int)SecurityEnums.CountyDirector || UserInformation.FirstOrDefault().RoleID == (int)SecurityEnums.CountySupervisor || UserInformation.FirstOrDefault().RoleID == (int)SecurityEnums.CountyWorker || UserInformation.FirstOrDefault().RoleID == (int)SecurityEnums.Clerical)
            {
                if (criteria.CountyID == null)
                {
                    var CaseInformation = _advancedSearch.AdvancedSearch(criteria).Result.Data.FirstOrDefault();
                    if (CaseInformation != null)
                    {
                        criteria.CountyID = CaseInformation.CountyID;
                    }
                }
                if (UserInformation.FirstOrDefault().CountyID == criteria.CountyID)
                {
                    criteria.UserCredentials = _securityService.GetUserEntitlements(UserInformation.FirstOrDefault().ID);
                }
                else
                {
                    criteria.UserCredentials = _securityService.GetUserEntitlements(UserInformation.FirstOrDefault().ID).Replace("IMPORT_CASE,", "").Replace("CONFIDENTIAL_CASE_IMPORT,", ""); ;
                }
            }
            else
            {
                criteria.UserCredentials = _securityService.GetUserEntitlements(UserInformation.FirstOrDefault().ID);
            }
            if (UserInformation.FirstOrDefault().RoleID == 3)
            {
                criteria.userCountyId = UserInformation.FirstOrDefault().CountyID;
                criteria.CaseWorkerID = UserInformation.FirstOrDefault().ID;
                criteria.LoginUserRoleID =Convert.ToInt32(UserInformation.FirstOrDefault().RoleID);
            }
            return _advancedSearch.caserecordinfoList(id, criteria).Result;
        }
        //Get/documentNotesbyDocumentID
        [HttpGet("documentNotesbyDocumentID/{id}")]
        public List<ScanningInfo> documentNotesbyDocumentID(int id)
        {
            return _advancedSearch.DocumentNotesbyDocumentID(id).Result.ToList();
        }

    }
}
