using AutoMapper;
using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;
using CCModels.GenericModelElements;
using CCModels.Models;
using CCRepo.IRepos;
using System;

namespace CCRepo.Repos
{
    public class LookupRepo :ILookupRepo
    {
        private readonly IDbConnection _connection;
        private IMapper _mapper;
        public LookupRepo(IDbConnection connection, IMapper mapper)
        {
            _connection = connection;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<IEnumerable<KeyValue>> GetList(string type, int CountyID)
        {          
            List<KeyValue> list = new List<KeyValue>();
            var parameters = new
            {
                Type = type,
                CountyID = CountyID
            };

            return await _connection.QueryAsync<KeyValue>(
                "USP_GET_LOOKUP_VALUES",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
        public async Task<IEnumerable<KeyValue>> GetDocSubTypeList(int typeId)
        {
            List<KeyValue> list = new List<KeyValue>();     
            var parameters = new
            {
                TypeId = typeId
            };

            return await _connection.QueryAsync<KeyValue>(
                "usp_get_documentsubtypevalues",
                parameters,
                commandType: CommandType.StoredProcedure
            );       
        }
        public async Task<IEnumerable<Users>> GetUserList(string username, int id)
        {
            var parameters = new
            {
                UserName = username,
                ID = id
            };

            return await _connection.QueryAsync<Users>(
                "[dbo].[USP_USER_INFO]",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
