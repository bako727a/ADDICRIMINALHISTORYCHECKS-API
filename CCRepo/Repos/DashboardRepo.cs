using AutoMapper;
using System.Data;
using Dapper;
using Microsoft.EntityFrameworkCore;
using CCModels.Models;
using Newtonsoft.Json.Linq;
using System.Collections;

namespace CCRepo.Repos
{
    public class DashboardRepo
    {
        private readonly IDbConnection _connection;
        private IMapper _mapper;
        public DashboardRepo(IDbConnection connection, IMapper mapper)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<DashboardStats> GetDashboardStats(int userID)
        {
            DashboardStats uem = new DashboardStats();

            var parameters = new
            {
                UserID = userID
            };
            uem = await _connection.QueryFirstOrDefaultAsync<DashboardStats>(
                "[dbo].[USP_DASHBOARD_STATS]",
                parameters,
                commandType: CommandType.StoredProcedure
            );
            return uem;
        }
        public async Task<IEnumerable<AdvanceSearchViewModel>> GetDashboardCaseData(DashboardCaseSearchCriteria criteria)
        {
            IEnumerable<AdvanceSearchViewModel> list = new List<AdvanceSearchViewModel>();         
            var parameters = new
            {
                Type = criteria.Type,
                CountyID = criteria.CountyID,
                UserID = criteria.UserId
            };
            await _connection.QueryAsync<AdvanceSearchViewModel>(
                "[dbo].[USP_DASHBOARD_CASE_MGMT]",
                parameters,
                commandType: CommandType.StoredProcedure
            ).ContinueWith(task =>
            {
                if (task.Exception == null)
                {
                    list = task.Result.ToList();
                }
            });
            return list;
        }
        public async Task<IEnumerable<AdvanceSearchViewModel>> GetDashboardCaseInfo(DashboardCaseSearchCriteria criteria)
        {
            IEnumerable<AdvanceSearchViewModel> list = new List<AdvanceSearchViewModel>();           
            var parameters = new
            {
                Type = criteria.Type,
                CountyID = criteria.CountyID,
                UserID = criteria.UserId
            };
            await _connection.QueryAsync<AdvanceSearchViewModel>(
                "[dbo].[USP_DASHBOARD_CASE_Info]",
                parameters,
                commandType: CommandType.StoredProcedure
            ).ContinueWith(task =>
            {
                if (task.Exception == null)
                {
                    list = task.Result.ToList();
                }
            });
            return list;
        }
        public async Task<IEnumerable<DocumentViewModel>> GetDashboardDocData(DashboardCaseSearchCriteria criteria)
        {           
            IEnumerable<DocumentViewModel> list = new List<DocumentViewModel>();           
            var parameters = new
            {
                UserID = criteria.UserId
            };
            await _connection.QueryAsync<DocumentViewModel>(
                "[dbo].[USP_DASHBOARD_DASH_MGMT]",
                parameters,
                commandType: CommandType.StoredProcedure
            ).ContinueWith(task =>
            {
                if (task.Exception == null)
                {
                    list = task.Result.ToList();
                }
            });
            return list;
        }
        public async Task<IEnumerable<DashCountyViewModel>> GetDashboardCountyData(DashboardCaseSearchCriteria criteria)
        {
            IEnumerable<DashCountyViewModel> list = new List<DashCountyViewModel>();           
            var parameters = new
            {
                Type = criteria.Type,
                CountyID = criteria.CountyID,
                FromDate = criteria.FromDate,
                ToDate = criteria.ToDate
            };
            await _connection.QueryAsync<DashCountyViewModel>(
                "[dbo].[USP_DASHBOARD_COUNTY_STATS]",
                parameters,
                commandType: CommandType.StoredProcedure
            ).ContinueWith(task =>
            {
                if (task.Exception == null)
                {
                    list = task.Result.ToList();
                }
            });
            return list;
        }
    }
}
