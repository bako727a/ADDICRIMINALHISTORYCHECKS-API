using Microsoft.AspNetCore.Mvc;
using CCInterfaces.Contracts;
using CCModels.Helper;
using CCModels.Models;

namespace ADDICCWebAPIS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminservice;
        private IConfiguration _configuration { get; }
        private readonly ILookupService _lookupService;
        public AdminController(IAdminService adminService, IConfiguration configuration, ILookupService lookupService)
        {
            _adminservice = adminService;
            _configuration = configuration;
            _lookupService = lookupService;
        }

        // POST: api/users/
        [HttpPost("users")]
        public async Task<GridResult<Users>> users(Users criteria)
        {
            return await _adminservice.GetUsers(criteria);
        }
        //POST: api/adduser/
        [HttpPost("adduser")]
        public void adduser(Users entity)
        {
            var userdeatils = User.Identity.Name.Replace("DHRAL\\", "");
            var user =  _lookupService.GetUserList(userdeatils, 0).Result;
            entity.CreatedBy = user.FirstOrDefault()?.ID ?? 0;
            _adminservice.adduser(entity);
        }
        //POST: api/adduser/
        [HttpPost("UpdateUser")]
        public void UpdateUser(Users entity)
        {
            var userdeatils = User.Identity.Name.Replace("DHRAL\\", "");
            var user = _lookupService.GetUserList(userdeatils, 0).Result;
            entity.CreatedBy = user.FirstOrDefault()?.ID ?? 0;
            _adminservice.updateuser(entity);
        }
        //Post: api/removeuser/
        [HttpPost("RemoveUser")]
        public void RemoveUser(Users entity)
        {
            var userdeatils = User.Identity.Name.Replace("DHRAL\\", "");
            var user = _lookupService.GetUserList(userdeatils, 0).Result;
            entity.CreatedBy = user.FirstOrDefault()?.ID ?? 0;
            _adminservice.deleteuser(entity);
        }
    }
}
