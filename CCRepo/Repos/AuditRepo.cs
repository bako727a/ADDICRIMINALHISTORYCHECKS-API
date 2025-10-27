using AutoMapper;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Data;
using CCModels.Models;

namespace CCRepo.Repos
{
    public class AuditRepo
    {
        public readonly IDbConnection _connection;
        public IMapper _mapper;
        public AuditRepo(IDbConnection dbContext, IMapper mapper) 
        {
            _connection = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AdvanceSearchViewModel>> GetAuditList(CaseAdvanceSearchCriteria criteria)
        {
            IEnumerable<AdvanceSearchViewModel> list = new List<AdvanceSearchViewModel>();  
            var parameters = new
            {
              CountyID = criteria.CountyID,
              CaseType = criteria.CaseType
            };
            await _connection.QueryAsync<AdvanceSearchViewModel>(
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
        //GetAssignedAuditList
        public async Task<IEnumerable<AdvanceSearchViewModel>> GetAssignedAuditList(CaseAdvanceSearchCriteria criteria)
        {
            List<AdvanceSearchViewModel> list = new List<AdvanceSearchViewModel>();           
            var parameters = new
            {
                CountyID = criteria.CountyID               
            };
            await _connection.QueryAsync<AdvanceSearchViewModel>(
             "[dbo].[USP_CASE_ASSIGNED_AUDIT_SEARCH]",
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
        public async void PostAuditCaseAssignment(AuditCaseInfo auditcases)
        {         
            var parameters = new
            {
                SSN = auditcases.CaseSSN
            };
            await _connection.ExecuteAsync(
               "[dbo].[USP_AUDIT_CASES_ASSIGNMENT]",
               parameters,
               commandType: CommandType.StoredProcedure
           );
        }

        public async void PostAuditCaseRemoveAssignment(AuditCaseInfo auditcases)
        {           
            var parameters = new
            {
                SSN = auditcases.CaseSSN
            };
            await _connection.ExecuteAsync(
               "[dbo].[USP_AUDIT_CASES_REMOVE_ASSIGNMENT]",
               parameters,
               commandType: CommandType.StoredProcedure
           );
        }
    }

}
