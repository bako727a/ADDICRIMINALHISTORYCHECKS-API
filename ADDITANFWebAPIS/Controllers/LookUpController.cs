using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TANFInterfaces.Contracts;
using TANFModels.GenericModelElements;
using TANFModels.Models;

namespace ADDITANFWebAPIS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LookUpController : ControllerBase
    {
        private readonly ILookupService _lookupService;
        public LookUpController(ILookupService lookupService)
        {
            _lookupService = lookupService;
        }
        //Get/getIdentityName
        [HttpGet("getIdentityName")]
        [AllowAnonymous]
        public async Task<IEnumerable<Users>> getIdentityName()
        {
            string windowsUser = Environment.UserName;
            string domainUser = $"{Environment.UserDomainName}\\{Environment.UserName}";
            return await _lookupService.GetUserList(domainUser.Replace("DHRAL\\", ""), 0);
        }

        //Get/GetUserInfo
        [HttpGet("UserName")]
        public async Task<IEnumerable<Users>> GetUserInfo(string username, int id)
        {
            return await _lookupService.GetUserList(username, id);
        }
        //Get/UserInfo
        [HttpGet("UserInfo/{username}")]
        public async Task<IEnumerable<Users>> UserInfo(string username, int id)
        {
            return await _lookupService.GetUserList(username, id);
        }
        //Get/County
        [HttpGet("County")]
        public async Task<IEnumerable<KeyValue>> GetCounties()
        {
            var list = _lookupService.GetList("County", 0);
            //list.Add(new KeyValue() { Key = 999, Value = User.Identity.Name.Replace("DHRAL\\", "") });
            return await list;
        }
        //Get/GetConfidentialCaseCounty
        [HttpGet("GetConfidentialCaseCounty")]
        public async Task<IEnumerable<KeyValue>> GetConfidentialCaseCounty()
        {
            var Userinformation =  _lookupService.GetUserList(User.Identity.Name.Replace("DHRAL\\", ""), 0).Result.FirstOrDefault();
            if (Userinformation.RoleID != 3 && Userinformation.RoleID != 4 && Userinformation.RoleID != 2)
            {
                var list = _lookupService.GetList("County", 0);
                return await list;
            }
            else
            {
                var list = _lookupService.GetList("ConfidentailCounty", Convert.ToInt32(Userinformation.CountyID));
                return await list;
            }
        }
        //Get/GetUsers
        [HttpGet("User")]
        public async Task<IEnumerable<KeyValue>> GetUsers()
        {
            var list = _lookupService.GetList("User", 0);
            return await list;
        }
        //Get/GetSupervisor
        [HttpGet("Supervisor")]
        public async Task<IEnumerable<KeyValue>> GetSupervisor()
        {
            var list = _lookupService.GetList("AllSupervisor", 0);
            return await list;
        }
        //Get/GetCountySupervisorList
        [HttpGet("CountySupervisor/{CountyID}")]
        public async Task<IEnumerable<KeyValue>> GetCountySupervisorList(int CountyID)
        {
            var list = _lookupService.GetList("GetCountySupervisors", CountyID);
            return await list;
        }
        //Get/GetCountyBasedUser
        [HttpGet("countybasedusers/{CountyID}")]
        public async Task<IEnumerable<KeyValue>> GetCountyBasedUser(int CountyID)
        {
            var list = _lookupService.GetList("CountyUser", CountyID);
            return await list;
        }
        //Get/GetDocumentType
        [HttpGet("DocumentType")]
        public async Task<IEnumerable<KeyValue>> GetDocumentType()
        {            
            return await _lookupService.GetList("DocumentType", 0);
        }
        //Get/GetDocumentSubType
        [HttpGet("DocumentSubType")]
        public async Task<IEnumerable<KeyValue>> GetDocumentSubType()
        {
            return await _lookupService.GetList("DocumentSubType", 0);
        }

        // GET: api/LookUp/        
        [HttpGet("DocumentSubTypebyType/{typeId}")]
        public async Task<IEnumerable<KeyValue>> GetDocumentSubTypebyTypeId(int typeId)
        {
            return await _lookupService.GetDocSubTypeList(typeId);
        }
    }
}
