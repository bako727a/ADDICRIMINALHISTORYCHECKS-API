using CCInterfaces.Contracts;
using CCModels.Helper;
using CCModels.Models;
using CCRepo.Repos;

namespace CCInterfaces.Services
{
    public class CaseManagementService : ICaseManagementService
    {
        private readonly CaseManagementRepo _repository;
        public CaseManagementService(CaseManagementRepo repository)
        {
            _repository = repository;
        }

        public async Task<GridResult<AdvanceSearchViewModel>> Search(CaseManagementSearchCriteria criteria)
        {
            var list =await  _repository.GetList(criteria);
            return new GridResult<AdvanceSearchViewModel>()
            {
                Data = list,
                PageIndex = criteria.PageIndex,
                PageSize = criteria.PageSize,
                TotalRecords = list.Count()
            };
        }


        public async Task<Document> documentrecordinfo(int DocumentID, int userid)
        {
            Document DVM = new Document();
            DVM =await _repository.GetDocumentRecordInfo(DocumentID, userid);
            return DVM;
        }

        public void SaveCaseNotes(CaseInfo Case)
        {
            _repository.AddCaseNotes(Case);
        }


        //Case record infor for Case notes!
        public async Task<CaseInfo> caserecordinfo(int CaseID)
        {
            CaseInfo DVM = new CaseInfo();
            DVM =await _repository.GetCaseRecordInfo(CaseID);
            return DVM;
        }

        public async Task<IEnumerable<CaseNote>> GetCasenotesInfo(int casenotes)
        {
            List<CaseNote> CN = new List<CaseNote>();
            CN = _repository.GetCaseNotes(casenotes);
            return CN;
        }

        public void deleterecordinfo(int DocumentID)
        {
            _repository.DeleteDocumentRecordInfo(DocumentID);
        }
    }
}
