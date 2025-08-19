using AutoMapper;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TANFDataAccess.Context;
using TANFModels.Models;

namespace TANFRepo.Repos
{
    public class SecurityRepo
    {
        private readonly IDbConnection _connection;
        private IMapper _mapper;
        public SecurityRepo(IDbConnection connection, IMapper mapper)
        {
            _connection = connection;
            _mapper = mapper;
        }
        public string GetUserEntitlements(int userId)
        {           
            IEnumerable<Entitlement> uem = new List<Entitlement>();
            var parameters = new
            {                
                userId = userId,
            };

            uem = _connection.Query<Entitlement>(
                "[dbo].[USP_GET_USER_ENTITLEMENT]",
                parameters,
                commandType: CommandType.StoredProcedure
            );
            return uem.Any() ? uem.First().EntCd : string.Empty;
        }

        public async Task<IEnumerable<SecurityRole>> GetSecurityRoles()
        {
            IEnumerable<SecurityRole> uem = new List<SecurityRole>();    
            uem = await _connection.QueryAsync<SecurityRole>(
                "[dbo].[USP_GET_SECURITYROLES]",
                commandType: CommandType.StoredProcedure
            );
            return uem;           
        }

        public List<Entitlement> GetEntitlements(int roleID)
        {
            List<Entitlement> uem = new List<Entitlement>();
            var parameters = new
            {
                ROLEID = roleID,
            };
            uem = _connection.Query<Entitlement>(
                "[dbo].[USP_GET_ENTITLEMENT]",
                parameters,
                commandType: CommandType.StoredProcedure
            ).ToList();
            return uem;      
        }

        public bool SetUserEntitlements(int FkRoleID, IEnumerable<Entitlement> entitlements)
        {
            foreach (var item in entitlements)
            {
                var parameters = new
                {
                    ROLEID = FkRoleID,
                    ENTITLEMENTID = item.EntitleID,
                    FLAG = item.IsActive
                };
                _connection.Execute(
                    "[dbo].[USP_SET_USER_ENTITLEMENT]",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );             
            }
            return true;
        }
    }
}
