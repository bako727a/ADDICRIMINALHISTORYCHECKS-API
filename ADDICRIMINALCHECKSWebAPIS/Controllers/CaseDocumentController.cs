using Microsoft.AspNetCore.Mvc;
using CCInterfaces.Contracts;
using CCModels.enums;
using CCModels.Helper;
using CCModels.Models;

namespace ADDICCWebAPIS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CaseDocumentController : ControllerBase
    {
        private readonly ICaseDocumentService _documentService;
        private readonly ILookupService _lookupservice;
        private readonly ISecurityService _securityService;
        private readonly IAdvanceSearchService _advanceSearchService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CaseDocumentController(ICaseDocumentService documentService, ILookupService lookupservice, ISecurityService securityService, IAdvanceSearchService advanceSearchService, IHttpContextAccessor httpContextAccessor)
        {
            _documentService = documentService;
            _lookupservice = lookupservice;
            _securityService = securityService;
            _advanceSearchService = advanceSearchService;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: api/CaseDocumentList/        
        [HttpGet("CaseDocumentList/{id}")]
        public async Task<CaseDocumentListViewModel> CaseDocumentList(int id)
        {
            var windowsUser = _httpContextAccessor.HttpContext?.User?.Identity;
            CaseDocumentListViewModel CDLV = new CaseDocumentListViewModel();
            return await _documentService.DocumentsList(id, CDLV);
            //var CaseDetails = _advanceSearchService.caserecordinfo(id);
            //var CaseInfo = CaseDetails.Result.FirstOrDefault();
            //var UserInformation = _lookupservice.GetUserList(windowsUser.Name.Replace("DHRAL\\",""), 0);
            //var userdetails = UserInformation.Result.FirstOrDefault();
            //var UserCredentials = _securityService.GetUserEntitlements(userdetails.ID);
            //if (userdetails.RoleID == (int)SecurityEnums.CountyDirector)
            //{
            //    if (userdetails.CountyID == CaseInfo.CountyID)
            //    {
            //        CDLV.UserCredentials = _securityService.GetUserEntitlements(userdetails.ID);
            //    }
            //    else
            //    {
            //        CDLV.UserCredentials = _securityService.GetUserEntitlements(userdetails.ID).Replace("COPY,", "").Replace("EDIT", "").Replace("DELETE", "");
            //    }
            //}
            //else if (userdetails.RoleID == (int)SecurityEnums.CountySupervisor || userdetails.RoleID == (int)SecurityEnums.CountyWorker)
            //{
            //    if (userdetails.CountyID == CaseInfo.CountyID)
            //    {
            //        CDLV.UserCredentials = _securityService.GetUserEntitlements(userdetails.ID);
            //    }
            //    else
            //    {
            //        CDLV.UserCredentials = _securityService.GetUserEntitlements(userdetails.ID).Replace("EDIT", "").Replace("DELETE", "");
            //    }
            //}
            //else
            //{
            //    CDLV.UserCredentials = _securityService.GetUserEntitlements(userdetails.ID);
            //}
            //return await _documentService.DocumentsList(id, CDLV);
        }
        //Post/UpdateDocument
        [HttpPost("UpdateDocument")]
        public async Task<CaseDocumentListViewModel> UpdateDocument(CCModels.Models.Document document)
        {
            var windowsUser = _httpContextAccessor.HttpContext?.User?.Identity;
            var userinfo = _lookupservice.GetUserList(windowsUser.Name.Replace("DHRAL\\",""), 0).Result.FirstOrDefault();
            document.CreatedBy = userinfo.ID;
            CaseDocumentListViewModel CDLV = new CaseDocumentListViewModel();
            _documentService.UpdateDocument(document);
            int id = document.CaseInfoID.ToInt();
            return _documentService.DocumentsList(id, CDLV).Result;
        }

        [HttpPost("CopyDocument")]
        public DocumentViewModel CopyDocument(CCModels.Models.Document document)
        {
            DocumentViewModel CDLV = new DocumentViewModel();
            var windowsUser = _httpContextAccessor.HttpContext?.User?.Identity;
            string casenumber = document.CaseSSN;
            var CaseRecord = _documentService.CaseRecordInfo(casenumber);
            Users userinfo = new Users();
            userinfo = _lookupservice.GetUserList(windowsUser.Name.Replace("DHRAL\\",""), 0).Result.FirstOrDefault();
            document.CreatedBy = userinfo.ID;
            document.CaseInfoID = CaseRecord[0].CaseInfoID;
            if (userinfo.RoleID != 3 && userinfo.RoleID != 4)
            {
                string outputFile = document.FilePath.Replace(".pdf", "");
                Random rnd = new Random();
                outputFile = outputFile + rnd.Next(0, 100) + ".pdf";
                System.IO.File.Copy(document.FilePath, outputFile);
                document.FilePath = outputFile;
                var result = _documentService.CopyDocument(document);
                ScannedAndCreateDocumentInfo ScannedAndCreateDocumentInfo = new ScannedAndCreateDocumentInfo();
                ScannedAndCreateDocumentInfo.CaseInfoId = document.CaseInfoID.ToInt();
                ScannedAndCreateDocumentInfo.DocumentId = result.DocumentID;
                ScannedAndCreateDocumentInfo.DocumentNotesDescription = "This particular document is copied from:" + document.OldCaseSSN + " on " + DateTime.Now;
                _documentService.DocumentNotes(ScannedAndCreateDocumentInfo);
                CDLV.DocumentID = result.DocumentID;
                CDLV.ResultMessage = "You successfully copied!";
            }
            else
            {
                if (CaseRecord[0].CountyID == userinfo.CountyID)
                {
                    string outputFile = document.FilePath.Replace(".pdf", "");
                    Random rnd = new Random();
                    outputFile = outputFile + rnd.Next(0, 9) + ".pdf";
                    System.IO.File.Copy(document.FilePath, outputFile);
                    document.FilePath = outputFile;
                    var result = _documentService.CopyDocument(document);
                    ScannedAndCreateDocumentInfo ScannedAndCreateDocumentInfo = new ScannedAndCreateDocumentInfo();
                    ScannedAndCreateDocumentInfo.CaseInfoId = document.CaseInfoID.ToInt();
                    ScannedAndCreateDocumentInfo.DocumentId = result.DocumentID;
                    ScannedAndCreateDocumentInfo.DocumentNotesDescription = "This particular document is copied from:" + document.OldCaseSSN + " on " + DateTime.Now;
                    _documentService.DocumentNotes(ScannedAndCreateDocumentInfo);
                    CDLV.DocumentID = result.DocumentID;
                    CDLV.ResultMessage = "You successfully copied!";
                }
                else
                {
                    CDLV.ResultMessage = "You cannot copy the cases from other county to your county cases!";
                }
            }
            return CDLV;
        }
        //Adding document notes!
        [HttpPost("saveFAJOBSDocumentNotes")]
        public void saveFAJOBSDocumentNotes(CCModels.Models.Document Doc)
        {
            var windowsUser = _httpContextAccessor.HttpContext?.User?.Identity;
            var userinfo = _lookupservice.GetUserList(windowsUser.Name.Replace("DHRAL\\",""), 0).Result.FirstOrDefault();
            Doc.UserAssgined = userinfo.ID;
            try
            {
                _documentService.PostDocumentNotes(Doc);
            }
            catch (Exception ex)
            {

            }
        }
        [HttpGet("viewFAJOBSDocumentNotes/{ID}")]
        public List<FAJOBSNotes> ViewFAJOBSDocumentNotes(int ID)
        {
            List<FAJOBSNotes> list = new List<FAJOBSNotes>();
            try
            {
                list = _documentService.ViewFAJOBSDocumentNotes(ID);
            }
            catch (Exception ex)
            { }
            return list;
        }
    }
}
