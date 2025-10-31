using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Dapper;
using CCModels.Models;

namespace CCRepo.Repos
{
    public class ConfidentialRepo
    {
        private readonly IDbConnection _connection;
        private IMapper _mapper;
        public ConfidentialRepo(IDbConnection connection, IMapper mapper)
        {
            _connection = connection;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public void PostConfidentialCaseAssignment(ConfidentialCaseInfo ConfidentialCaseInfo)
        {
            var parameters = new
            {
                CaseNumber = ConfidentialCaseInfo.CaseSSN,
                IsConfidential = ConfidentialCaseInfo.IsConfidential ? 1 : 0,
            };
            _connection.Execute(
                "[dbo].[USP_AssignConfidentialCases]",
                parameters,
                commandType: CommandType.StoredProcedure // Fix: Use the correct overload
            );
        }
        public void PostremoveConfidentialCaseAssignment(ConfidentialCaseInfo ConfidentialCaseInfo)
        {          
            var parameters = new
            {
                CaseNumber = ConfidentialCaseInfo.CaseSSN,
                IsConfidential = 0,
            };
            _connection.Execute(
                "[dbo].[USP_ReAssignConfidentialCases]",
                parameters,
                commandType: CommandType.StoredProcedure // Fix: Use the correct overload
            );
        }
        public async Task<IEnumerable<AdvanceSearchViewModel>> ConfidentialSearch(CaseAdvanceSearchCriteria criteria)
        {
            IEnumerable<AdvanceSearchViewModel> list = new List<AdvanceSearchViewModel>();          
            var parameters = new { 
            Casenumber = criteria.CaseSSN,
            SSN = criteria.CaseSSN,
            UserID = criteria.CaseWorkerID,
            CaseType = criteria.CaseType,
            DocumentNumber = criteria.DocumentNumber,
            LastName = criteria.LastName,
            FirstName = criteria.FirstName,
            DOB = criteria.DOB,
            CaseStatus = criteria.CaseStatus,
            CountyID = criteria.CountyID,
            ReviewPeriod = criteria.ReviewDate,
            DocumentType = criteria.DocumentTypeID,
            DocumentSubType = criteria.DocumentSubTypeID,
            LoginUserRoleID = criteria.LoginUserRoleID,
            LoginUserID = criteria.LoginUserID,
            UserCountyID = criteria.userCountyId,
            };
            list = await _connection.QueryAsync<AdvanceSearchViewModel>(
                "[dbo].[usp_confidential_search]",
                parameters,
                commandType: CommandType.StoredProcedure
            );
            return list;
        }
    }
}
