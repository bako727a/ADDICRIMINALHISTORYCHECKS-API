using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TANFModels.Helper;
using TANFModels.Models;

namespace TANFInterfaces.Contracts
{
    public interface ICaseManagementService
    {
        Task<GridResult<AdvanceSearchViewModel>> Search(CaseManagementSearchCriteria criteria);
        Task<Document> documentrecordinfo(int DocumentID, int userid);
        Task<CaseInfo> caserecordinfo(int CaseID);
        void deleterecordinfo(int DocumentID);
        void SaveCaseNotes(CaseInfo Case);
        Task<IEnumerable<CaseNote>> GetCasenotesInfo(int casenotes);
    }
}
