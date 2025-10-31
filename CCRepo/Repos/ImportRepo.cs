using System.Data;
using AutoMapper;
using Dapper;
using Microsoft.EntityFrameworkCore;
using CCModels.Models;
namespace CCRepo.Repos
{
    public class ImportRepo
    {
        private readonly IDbConnection _connection;
        private readonly IMapper _mapper;
        public ImportRepo(IDbConnection connection, IMapper mapper)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<int> SaveDocument(DocumentViewModel document)
        { 
            var parameters = new
            {
                CountyID = document.CountyID,
                DateAssigned = document.DateAssigned,
                UserAssgined = document.UserAssgined,
                PageCount = document.PageCount,
                IsDeleted = document.IsDeleted,
                CaseInfoID = document.CaseInfoID,
                DocumentTypeID = document.DocumentTypeID,
                DocumentSubTypeID = document.DocumentSubTypeID,
                FilePath = document.FilePath,
                Description = document.Description,
                DocumentNote = document.DocumentNote,
                MFRMDescription = document.MFRMDescription,
                MFRMDocType = document.MFRMDocType,
                ReviewPeriod = string.IsNullOrEmpty(document.ReviewDate) ? (DateTime?)null : DateTime.Parse(document.ReviewDate),
                CreatedDate = DateTime.Now,
                CreatedBy = document.CreatedBy,
                LastUpdateDate = DateTime.Now,
                LastUpdatedBy = document.LastUpdatedBy
            };
            var documentId = _connection.Execute(
                "[dbo].[USP_IMPORT_DOCUMENTS]",
                parameters,
                commandType: CommandType.StoredProcedure
            );
            _connection.Execute(
                "[dbo].[USP_Audit_Logs_information]",
                new
                {
                    CaseInfoID = document.CaseInfoID,
                    CreatedBy = document.CreatedBy,
                    DocumentTypeID = document.DocumentTypeID,
                    DocumentSubTypeID = document.DocumentSubTypeID,
                    DocumentID = documentId,
                    Action = "Import Document"
                },
                commandType: CommandType.StoredProcedure
            );
            return documentId;
        }
        public async Task<DocumentViewModel> UpdateDocument(Document document)
        {
            // Define parameters for the stored procedure
            var parameters = new
            {
                Description = document.Description,
                DocumentID = document.DocumentID,
                PageCount = document.PageCount,
                FilePath = document.FilePath,
                LastUpdatedBy = document.LastUpdatedBy
            };

            // Execute the stored procedure
            await _connection.ExecuteAsync(
                "[dbo].[USP_IMPORT_UPDATE_DOCUMENTS]",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            // Log the audit information
            await _connection.ExecuteAsync(
                "[dbo].[USP_Audit_Logs_information]",
                new
                {
                    CaseInfoID = document.CaseInfoID,
                    CreatedBy = document.CreatedBy,
                    DocumentTypeID = document.DocumentTypeID,
                    DocumentSubTypeID = document.DocumentSubTypeID,
                    DocumentID = document.DocumentID,
                    Action = "Update Document"
                },
                commandType: CommandType.StoredProcedure
            );

            // Map the updated document to the DocumentViewModel
            var DVM = _mapper.Map<DocumentViewModel>(document);

            return DVM;
        }
        public void DocumentNotes(ScannedAndCreateDocumentInfo ScannedAndCreateDocumentInfo)
        {           
            var parameters = new
            {
                DocumentID = ScannedAndCreateDocumentInfo.DocumentId,
                CaseInfoId = ScannedAndCreateDocumentInfo.CaseInfoId,
                DocumentTypeID = ScannedAndCreateDocumentInfo.DocumentTypeID,
                DocumentSubTypeId = ScannedAndCreateDocumentInfo.DocumentSubTypeId,
                ReviewDate = ScannedAndCreateDocumentInfo.ReviewDate,
                DocumentNotesDescription = ScannedAndCreateDocumentInfo.DocumentNotesDescription
            };
            _connection.Execute(
                "[dbo].[USP_DOCUMENT_NOTES]",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
        public List<Document> GetDocumentInfo(DocumentViewModel document)
        {
            List<Document> DVM = new List<Document>();          
            var parameters = new
            {
                DocumentTypeID = document.DocumentTypeID == 0 ? (int?)null : document.DocumentTypeID,
                DocumentSubTypeID = document.DocumentSubTypeID == 0 ? (int?)null : document.DocumentSubTypeID,
                CaseInfoID = document.CaseInfoID == 0 ? (int?)null : document.CaseInfoID,
                ReviewPeriod = string.IsNullOrEmpty(document.ReviewDate) ? null : document.ReviewDate
            };
            DVM = _connection.Query<Document>(
                "[dbo].[USP_DOCUMENT_SEARCH]",
                parameters,
                commandType: CommandType.StoredProcedure
            ).ToList();
            return DVM;
        }
    }
}
