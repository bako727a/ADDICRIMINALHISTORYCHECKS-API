using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TANFModels.Models;

namespace TANFInterfaces.Contracts
{
    public interface ICaseDocumentService
    {
        Task<IEnumerable<CaseDocumentListViewModel>> DocumentsList(int id, CaseDocumentListViewModel CDLV);
        void UpdateDocument(Document document);
        DocumentViewModel CopyDocument(Document document);
        List<CaseInfo> CaseRecordInfo(string casenumber);
        void PostDocumentNotes(Document DOc);
        List<FAJOBSNotes> ViewFAJOBSDocumentNotes(int DOcumentID);
        void DocumentNotes(ScannedAndCreateDocumentInfo scannedAndCreateDocumentInfo);
    }
}
