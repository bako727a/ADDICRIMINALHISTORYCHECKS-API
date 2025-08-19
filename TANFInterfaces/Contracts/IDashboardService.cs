using TANFModels.Helper;
using TANFModels.Models;

namespace TANFInterfaces.Contracts
{
    public interface IDashboardService
    {
        Task<DashboardStats> GetDashboardStats(int userID);
        Task<GridResult<AdvanceSearchViewModel>> GetDashboardCaseData(DashboardCaseSearchCriteria criteria);
        Task<GridResult<AdvanceSearchViewModel>> GetDashboardCaseInfo(DashboardCaseSearchCriteria criteria);
        Task<GridResult<DocumentViewModel>> GetDashboardDocData(DashboardCaseSearchCriteria criteria); 
        Task<GridResult<DashCountyViewModel>> GetDashboardCountyData(DashboardCaseSearchCriteria criteria);
       
    }
}
