using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TANFInterfaces.Contracts;
using TANFModels.Models;
using TANFRepo.Repos;

namespace TANFInterfaces.Services
{
    public class CaseDocumentService : ICaseDocumentService
    {
        public readonly CaseDocumentRepo _repository;
        public CaseDocumentService(CaseDocumentRepo repository)
        {
            _repository = repository;
        }

        public async  Task<IEnumerable<CaseDocumentListViewModel>> DocumentsList(int id, CaseDocumentListViewModel CDLV)
        {

            return await _repository.GetList(id, CDLV);
        }
        public void UpdateDocument(Document document)
        {
            _repository.UpdateDocument(document);
        }
        public DocumentViewModel CopyDocument(Document document)
        {
            return _repository.CopyDocument(document);
        }
        public List<CaseInfo> CaseRecordInfo(string casenumber)
        {
            return _repository.CaseRecordInfo(casenumber);
        }

        public void PostDocumentNotes(Document Doc)
        {
            _repository.SaveFAJOBSDocumentNotes(Doc);
        }
        public List<FAJOBSNotes> ViewFAJOBSDocumentNotes(int DocumentID)
        {
            return _repository.ViewFAJOBSDocumentNotes(DocumentID);
        }
        public void DocumentNotes(ScannedAndCreateDocumentInfo scannedAndCreateDocumentInfo)
        {
            _repository.DocumentNotes(scannedAndCreateDocumentInfo);
        }
    }
}
