using Microsoft.AspNetCore.Mvc;
using TANFInterfaces.Contracts;
using TANFModels.Helper;
using TANFModels.Models;

namespace ADDITANFWebAPIS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private readonly ISecurityService _securityService;
        public SecurityController(ISecurityService securityService)
        {
            _securityService = securityService;
        }
        //Get/GetUserEntitlements
        [HttpGet("GetUserEntitlements")]
        public string GetUserEntitlements(int userID)
        {
            return  _securityService.GetUserEntitlements(userID);
        }
        //Get/GetSecurityRoles
        [HttpGet("GetSecurityRoles")]
        public async Task<IEnumerable<SecurityRole>> GetSecurityRoles()
        {
            return await _securityService.GetSecurityRoles();
        }
        //Post/GetEntitlements
        [HttpPost("GetEntitlements/{roleID}")]
        public async Task<GridResult<Entitlement>> GetEntitlements(int roleID)
        {
            return await _securityService.GetEntitlements(roleID);
        }
        //POST/SetUserEntitlements
        [HttpPost("SetUserEntitlements")]
        public bool SetUserEntitlements(int roleID, List<Entitlement> entitlements)
        {
            return _securityService.SetUserEntitlements(roleID, entitlements);
        }
    }
}
