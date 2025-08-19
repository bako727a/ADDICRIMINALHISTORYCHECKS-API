using TANFModels.Helper;
using TANFModels.Models;

namespace TANFInterfaces.Contracts
{
    public interface IAdvanceSearchService
    {
        Task<GridResult<AdvanceSearchViewModel>> AdvancedSearch(CaseAdvanceSearchCriteria criteria);
        Task<GridResult<AdvanceSearchViewModel>> Importsearch(CaseAdvanceSearchCriteria criteria);
        Task<IEnumerable<AdvanceSearchViewModel>> caserecordinfo(int id);
        Task<GridResult<AdvanceSearchViewModel>> caserecordinfoList(int id, CaseAdvanceSearchCriteria criteria);
        Task<IEnumerable<ScanningInfo>> DocumentNotesbyDocumentID(int id);
    }
}
