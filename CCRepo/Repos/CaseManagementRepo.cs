using AutoMapper;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Data;
using CCModels.Models;

namespace CCRepo.Repos
{
    public class CaseManagementRepo
    {
        public readonly IDbConnection _connection;
        private IMapper _mapper;
        public CaseManagementRepo(IDbConnection connection, IMapper mapper)
        {
            _connection = connection;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<IEnumerable<AdvanceSearchViewModel>> GetList(CaseManagementSearchCriteria criteria)
        {
            IEnumerable<AdvanceSearchViewModel> list = new List<AdvanceSearchViewModel>();           
            var parameters = new
            {
                Type = criteria.CaseSSN != "" && criteria.CaseSSN != null ? "SSN" : criteria.CountyID > 0 && criteria.CountyID != null ? "County" : "Both",
                CountyID = criteria.CountyID,
                CaseSSN = criteria.CaseSSN,
                CaseWorkerID = criteria.CaseWorkerID,
                UserCountyID = criteria.userCountyId,
                LoginUserRoleID = criteria.LoginUserRoleID,
                isConfidentialAllowed = criteria.isConfidentialAllowed ? 'Y' : 'N'
            };
            return await _connection.QueryAsync<AdvanceSearchViewModel>(
                "[dbo].[USP_IBM_CASE_MANAGEMENT_SEARCH]",
                parameters,
                commandType: CommandType.StoredProcedure
            );           
        }

        public async Task<Document> GetDocumentRecordInfo(int DocumentID, int userid)
        {
            Document DVM = new Document();                      
            var parameters = new {
                DocumentID = DocumentID,
            };
            DVM = await _connection.QueryFirstOrDefaultAsync<Document>(
                "[dbo].[USP_DOCUMENT_RECORD_SEARCH]",
                parameters,
                commandType: CommandType.StoredProcedure
            );   
            await _connection.ExecuteAsync(
                "[dbo].[USP_Audit_Logs_information]",
                new
                {
                    CaseInfoID = DVM.CaseInfoID,
                    CreatedBy = userid,
                    DocumentTypeID = DVM.DocumentTypeID,
                    DocumentSubTypeID = DVM.DocumentSubTypeID,
                    DocumentID = DVM.DocumentID,
                    Action = "View Document"
                },
                commandType: CommandType.StoredProcedure
            );
            return DVM;
        }
        public async Task<CaseInfo> GetCaseRecordInfo(int CaseID)
        {
            CaseInfo CVM = new CaseInfo();
            var parameters = new
            {
                CaseID = CaseID
            };
            CVM = await _connection.QueryFirstOrDefaultAsync<CaseInfo>(
                "[dbo].[USP_Case_RECORD_SEARCH]",
                parameters,
                commandType: CommandType.StoredProcedure
            );
            return CVM;
        }
        public void AddCaseNotes(CaseInfo Case)
        {
            Case.CaseNotes = Case.CaseNotes;           
            var parameters = new
            {
                CaseInfoID = Case.CaseInfoID,
                CreatedDate = DateTime.Now,
                CreatedUserBy = Case.AuditAssignedUserID,
                CaseNote = Case.CaseNotes
            };
            _connection.Execute(
                "[dbo].[USP_ADD_CaseNotes]",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
        public List<CaseNote> GetCaseNotes(int Cases)
        {
            List<CaseNote> CN = new List<CaseNote>();      
            var parameters = new
            {
                CaseID = Cases
            };
            CN = _connection.Query<CaseNote>(
                "[dbo].[USP_CASE_NOTES]",
                parameters,
                commandType: CommandType.StoredProcedure
            ).ToList();
            return CN;
        }
        public void DeleteDocumentRecordInfo(int DocumentID)
        {
            List<Document> DVM = new List<Document>();   
            var parameters = new
            {
                DocumentID = DocumentID
            };
            _connection.Execute(
                "[dbo].[USP_DELETE_DOCUMENT_RECORD]",
                parameters,
                commandType: CommandType.StoredProcedure
            );
            _connection.Execute(
                "[dbo].[USP_Audit_Logs_information]",
                new
                {
                    CaseInfoID = DVM[0].CaseInfoID, // Assuming CaseInfoID is not needed for deletion
                    CreatedBy = DVM[0].CreatedBy, // Assuming CreatedBy is not needed for deletion
                    DocumentTypeID = DVM[0].DocumentTypeID, // Assuming DocumentTypeID is not needed for deletion
                    DocumentSubTypeID = DVM[0].DocumentSubTypeID, // Assuming DocumentSubTypeID is not needed for deletion
                    DocumentID = DocumentID,
                    Action = "Delete Document"
                },
                commandType: CommandType.StoredProcedure
            );
            _connection.Execute(
                "[dbo].[USP_DELETE_DOCUMENT_RECORD]",
                new
                {
                    DocumentID = DocumentID
                },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
