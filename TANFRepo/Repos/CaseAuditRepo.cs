using AutoMapper;
using Dapper;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TANFModels.Models;

namespace TANFRepo.Repos
{
    public class CaseAuditRepo
    {
        public readonly IDbConnection _connection;
        public IMapper _mapper;
        public CaseAuditRepo(IDbConnection dbContext, IMapper mapper)
        {
            _connection = dbContext;
            _mapper = mapper;
        }
        public async Task<IEnumerable<CaseAuditLogViewModel>> GetCaseAuditList(CaseAuditLog criteria)
        {
            IEnumerable<CaseAuditLogViewModel> list = new List<CaseAuditLogViewModel>();
            var parameters = new
            {
                CountyID = criteria.CountyID == 0 ? null : criteria.CountyID,
                FromDate = "2022-08-01",//criteria.FromDate,
                StringToDateOnlyConverter = criteria.ToDate,
            };
            await _connection.QueryAsync<CaseAuditLogViewModel>(
               "[dbo].[USP_CASE_AUDIT_SEARCH]",
               parameters,
               commandType: CommandType.StoredProcedure
           ).ContinueWith(task =>
           {
               if (task.Exception == null)
               {
                   list = task.Result.ToList();
               }
           });
            return list;
        }
    }
}
