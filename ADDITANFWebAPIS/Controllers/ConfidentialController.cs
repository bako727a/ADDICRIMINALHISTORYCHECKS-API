using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TANFInterfaces.Contracts;
using TANFModels.enums;
using TANFModels.Helper;
using TANFModels.Models;

namespace ADDITANFWebAPIS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfidentialController : ControllerBase
    {
        private readonly IConfidentialService _confidentialCase;
        private readonly IAdvanceSearchService _advancedSearch;
        private readonly ILookupService _lookupService;
        private readonly ISecurityService _securityService;
        public ConfidentialController(IConfidentialService confidentialCase, IAdvanceSearchService advancedSearch, ILookupService lookupService, ISecurityService securityService)
        {
            _confidentialCase = confidentialCase;
            _advancedSearch = advancedSearch;
            _lookupService = lookupService;
            _securityService = securityService;
        }
        //POST/confidentialcaseassignment
        [HttpPost("confidentialcaseassignment")]
        public void confidentialcaseassignment(ConfidentialCaseInfo Confidential)
        {
            foreach (var item in Confidential.checkArray)
            {
                ConfidentialCaseInfo CCM = new ConfidentialCaseInfo();
                CCM.CaseSSN = item.ToStr();
                _confidentialCase.confidentialcaseassignment(CCM);
            }
        }
        //POST/removeconfidentialcaseassignment
        [HttpPost("removeconfidentialcaseassignment")]
        public void removeconfidentialcaseassignment(ConfidentialCaseInfo Confidential)
        {
            foreach (var item in Confidential.checkArray)
            {
                ConfidentialCaseInfo CCM = new ConfidentialCaseInfo();
                CCM.CaseSSN = item.ToStr();
                CCM.UserCountyID = Confidential.UserCountyID.ToInt();
                _confidentialCase.removeconfidentialcaseassignment(CCM);
            }
        }
        //POST/ConfidentialSearch
        [HttpPost("ConfidentialSearch")]
        public async Task<GridResult<AdvanceSearchViewModel>> ConfidentialSearch(CaseAdvanceSearchCriteria criteria)
        {
            if (criteria.ReviewPeriod != null)
            {
                criteria.ReviewDate = criteria.ReviewPeriod.Value.ToShortDateString();
            }
            var UserInformation = _lookupService.GetUserList(User.Identity.Name.Replace("DHRAL\\", ""), 0).Result.FirstOrDefault();
            var UserCredentials = _securityService.GetUserEntitlements(UserInformation.ID);
            if (UserInformation.RoleID == (int)SecurityEnums.CountyDirector || UserInformation.RoleID == (int)SecurityEnums.CountySupervisor || UserInformation.RoleID == (int)SecurityEnums.CountyWorker)
            {
                if (criteria.CountyID == null)
                {
                    //var CaseInformation = _advancedSearch.caserecordinfo(criteria.CaseSSN.ToInt());                  
                    var CaseInformation = _advancedSearch.AdvancedSearch(criteria).Result.Data.FirstOrDefault();
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
                    criteria.UserCredentials = _securityService.GetUserEntitlements(UserInformation.ID).Replace("IMPORT_CASE,", "");
                }
            }
            else
            {
                criteria.UserCredentials = _securityService.GetUserEntitlements(UserInformation.ID);
            }
            criteria.LoginUserID = UserInformation.ID;
            criteria.LoginUserRoleID = UserInformation.RoleID.ToInt();
            criteria.userCountyId = UserInformation.CountyID;
            return await _confidentialCase.ConfidentialSearch(criteria);
        }
    }
}
