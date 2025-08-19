using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TANFInterfaces.Contracts;
using TANFModels.Helper;
using TANFModels.Models;
using TANFRepo.Repos;

namespace TANFInterfaces.Services
{
    public class SecurityService : ISecurityService
    {
        public readonly SecurityRepo _repository;

        public SecurityService(SecurityRepo repository)
        {
            _repository = repository;
        }

        public string GetUserEntitlements(int userID)
        {
            return _repository.GetUserEntitlements(userID);
        }

        public async Task<IEnumerable<SecurityRole>> GetSecurityRoles()
        {
            return await _repository.GetSecurityRoles();
        }

        public async Task<GridResult<Entitlement>> GetEntitlements(int roleID)
        {
            var list = _repository.GetEntitlements(roleID);
            return new GridResult<Entitlement>()
            {
                Data = list,
                TotalRecords = list.Count(),
            };

        }

        public bool SetUserEntitlements(int FKRoleID, List<Entitlement> entitlements)
        {
            return _repository.SetUserEntitlements(FKRoleID, entitlements);
        }

    }
}
