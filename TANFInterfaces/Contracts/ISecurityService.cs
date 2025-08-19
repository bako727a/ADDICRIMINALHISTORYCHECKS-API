using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TANFModels.Helper;
using TANFModels.Models;

namespace TANFInterfaces.Contracts
{
    public interface ISecurityService
    {
        string GetUserEntitlements(int userID);
        Task<IEnumerable<SecurityRole>> GetSecurityRoles();
        Task<GridResult<Entitlement>> GetEntitlements(int roleID);
        bool SetUserEntitlements(int FKRoleID, List<Entitlement> entitlements);
    }
}
