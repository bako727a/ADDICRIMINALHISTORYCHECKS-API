using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCInterfaces.Contracts;
using CCModels.Helper;
using CCModels.Models;
using CCRepo.Repos;

namespace CCInterfaces.Services
{
    public class DashboardService :IDashboardService
    {
        private readonly DashboardRepo _dashboardRepository;
        public DashboardService(DashboardRepo repository)
        {
            _dashboardRepository = repository ?? throw new ArgumentNullException(nameof(repository));
        }
        public async Task<DashboardStats> GetDashboardStats(int userID)
        {
            return await _dashboardRepository.GetDashboardStats(userID);
        }

        public async Task<GridResult<AdvanceSearchViewModel>> GetDashboardCaseData(DashboardCaseSearchCriteria criteria)
        {
            var list = await _dashboardRepository.GetDashboardCaseData(criteria);
            return new GridResult<AdvanceSearchViewModel>()
            {
                Data = list,
                PageIndex = criteria.PageIndex,
                PageSize = criteria.PageSize,
                TotalRecords = list.Count()
            };
        }
        public async Task<GridResult<AdvanceSearchViewModel>> GetDashboardCaseInfo(DashboardCaseSearchCriteria criteria)
        {
            var list =await _dashboardRepository.GetDashboardCaseInfo(criteria);
            return new GridResult<AdvanceSearchViewModel>()
            {
                Data = list,
                PageIndex = criteria.PageIndex,
                PageSize = criteria.PageSize,
                TotalRecords = list.Count()
            };
        }
        public async Task<GridResult<DocumentViewModel>> GetDashboardDocData(DashboardCaseSearchCriteria criteria)
        {
            var list =await _dashboardRepository.GetDashboardDocData(criteria);
            return new GridResult<DocumentViewModel>()
            {
                Data = list,
                PageIndex = criteria.PageIndex,
                PageSize = criteria.PageSize,
                TotalRecords = list.Count()
            };
        }

        public async Task<GridResult<DashCountyViewModel>> GetDashboardCountyData(DashboardCaseSearchCriteria criteria)
        {
            var list =await _dashboardRepository.GetDashboardCountyData(criteria);
            return new GridResult<DashCountyViewModel>()
            {
                Data = list,
                PageIndex = criteria.PageIndex,
                PageSize = criteria.PageSize,
                TotalRecords = list.Count()
            };
        }
    }
}
