using AutoMapper;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCDataAccess.Context;
using CCModels.Helper;
using CCModels.Models;

namespace CCRepo.Repos
{
    public class CaseDocumentRepo
    {
        public readonly IDbConnection _connection;
        private IMapper _mapper;
        public CaseDocumentRepo(IDbConnection connection, IMapper mapper)
        {
            _connection = connection;
            _mapper = mapper;
        }
        public async Task<IEnumerable<CaseDocumentListViewModel>> GetList(int id, CaseDocumentListViewModel CDLV)
        {
            List<CaseDocumentListViewModel> list = new List<CaseDocumentListViewModel>();
            List<Document> Doc = new List<Document>();
            string documenttypename = "";
            DateTime CertAccessDateFlag = DateTime.Now;
            var parameters = new {
                CaseinfoID = id,
            };
            return await _connection.QueryAsync<CaseDocumentListViewModel>(
                "[dbo].[usp_document_list_search]",
                parameters,
                commandType: CommandType.StoredProcedure
            );           
        }
        public async void UpdateDocument(Document document)
        {
            if (document.ReviewPeriod.ToStr() == "1/1/0001 12:00:00 AM")
            {
                document.ReviewPeriod = null;
            }           
            var parameters = new
            {
                DocumentId = document.DocumentID,
                DocumentTypeID = document.DocumentTypeID,
                DocumentSubTypeID = document.DocumentSubTypeID,
                ReivewPeriod = document.ReviewPeriod,
                Description = document.Description,
                DocumentNote = document.DocumentNote,
                MFRMDescription = document.MFRMDescription,
                MFRMDocType = document.MFRMDocType,
                CaseTypeID = document.CaseTypeID,
                CreatedBy = document.CreatedBy,
                caseinfoid = document.CaseInfoID,
                Action = "Modify Document"
            };
            await _connection.ExecuteAsync(
              "[dbo].[USP_UPDATE_Document]",
              parameters,
              commandType: CommandType.StoredProcedure
          );
            await _connection.ExecuteAsync(
              "[dbo].[USP_Audit_Logs_information]",
              parameters,
              commandType: CommandType.StoredProcedure
          );
        }
        public DocumentViewModel CopyDocument(Document document)
        {
            if (document.ReviewPeriod.ToStr() == "1/1/0001 12:00:00 AM")
            {
                document.ReviewPeriod = null;
            }
            DocumentViewModel DocumentViewModel = new DocumentViewModel();           
            var parameters = new
            {
                PageCount = document.PageCount,
                CreatedBy = document.CreatedBy,
                FilePath = document.FilePath,
                CaseInfoID = document.CaseInfoID,
                LastUpdatedBy = document.LastUpdatedBy,
                DocumentTypeID = document.DocumentTypeID,
                DocumentSubTypeID = document.DocumentSubTypeID,
                ReviewPeriod = document.ReviewPeriod,
                Description = document.Description,
                Action = "Copy Document"
            };
            _connection.Execute(
                "[dbo].[USP_Copy_Document]",
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
                    DocumentID = document.DocumentID,
                    Action = "Copy Document"
                },
                commandType: CommandType.StoredProcedure
            );
            return DocumentViewModel;
        }
        public List<CaseInfo> CaseRecordInfo(string CaseSSN)
        {
            int CaseInfoId = 0;
            List<CaseInfo> Case = new List<CaseInfo>();           
            var parameters = new
            {
                CaseNumber = CaseSSN,
                CaseInfoId = CaseInfoId
            };
            Case = _connection.Query<CaseInfo>(
                "[dbo].[USP_Casserecord_CaseNumber_info]",
                parameters,
                commandType: CommandType.StoredProcedure
            ).ToList();
            return Case;
        }
        public void SaveFAJOBSDocumentNotes(Document Doc)
        {          
            var parameters = new
            {
                DocumentID = Doc.DocumentID,
                DocumentTypeID = Doc.DocumentTypeID,
                DocumentNotes = Doc.FAJOBSDocumentNote,
                CreatedUserID = Doc.UserAssgined,
                DocumentSubTypeID = Doc.DocumentSubTypeID
            };
            _connection.Execute(
                "[dbo].[USP_FAJOBS_DOCUMENT_NOTES]",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
        public List<FAJOBSNotes> ViewFAJOBSDocumentNotes(int DocumentID)
        {
            List<FAJOBSNotes> DocumentNotes = new List<FAJOBSNotes>();

            var parameters = new
            {
                DocumentID = DocumentID
            };
            DocumentNotes = _connection.Query<FAJOBSNotes>(
                "[dbo].[USP_FAJOBS_DOCUMENT_NOTES_SEARCH]",
                parameters,
                commandType: CommandType.StoredProcedure
            ).ToList();
            return DocumentNotes;
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
        }
    }
}
