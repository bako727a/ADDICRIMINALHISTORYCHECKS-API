using AutoMapper;
using Dapper;
using System.Data;
using CCModels.Models;

namespace CCRepo.Repos
{
    public class AdminRepo
    {
        private readonly IDbConnection _connection;
        private IMapper _mapper;
        public AdminRepo(IDbConnection connection, IMapper mapper)
        {
            _connection = connection;
            _mapper = mapper;
        }
        public async Task<IEnumerable<Users>> GetUsers(Users User)
        {  
            var parameters = new
            {
               FirstName = User.FirstName == "string" ? null: User.FirstName,
                LastName = User.LastName == "string" ? null : User.LastName,
                RoleID = User.RoleID == 0 ? null : User.RoleID,
               CountyID = User.CountyID == 0 ? null: User.CountyID,
               UserName = User.UserName == "string" ?  null:User.UserName,
            };

            return await _connection.QueryAsync<Users>(
                "[dbo].[usp_user_search]",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async void adduser(Users User)
        {
            var parameters = new
            {
                FirstName = User.FirstName == "string" ? null : User.FirstName,
                LastName = User.LastName == "string" ? null : User.LastName,
                RoleID = User.RoleID == 0 ? null : User.RoleID,
                CountyID = User.CountyID == 0 ? null : User.CountyID,
                UserName = User.UserName == "string" ? null : User.UserName,
            };
            await _connection.ExecuteAsync(
                "[dbo].[usp_add_user]",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
        public async void updateuser(Users User)
        { 
            var parameters = new
            {
                FirstName = User.FirstName == "string" ? null : User.FirstName,
                LastName = User.LastName == "string" ? null : User.LastName,
                RoleID = User.RoleID == 0 ? null : User.RoleID,
                CountyID = User.CountyID == 0 ? null : User.CountyID,
                UserName = User.UserName == "string" ? null : User.UserName,
            };
            await _connection.ExecuteAsync(
                "[dbo].[usp_update_user]",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
        public async void removeuser(Users User)
        {         
            var parameters = new
            {            
                UserName = User.UserName
            };
            await _connection.ExecuteAsync(
                "[dbo].[usp_delete_user]",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
