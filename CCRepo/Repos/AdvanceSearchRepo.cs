using AutoMapper;
using Dapper;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCDataAccess.Context;
using CCModels.Models;

namespace CCRepo.Repos
{
    public class AdvanceSearchRepo
    {
        public readonly IDbConnection _connection;
        private IMapper _mapper;
        public AdvanceSearchRepo(IDbConnection connection, IMapper mapper)
        {
            _connection = connection;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AdvanceSearchViewModel>> GetList(CaseAdvanceSearchCriteria criteria)
        {
            if (criteria.DOB == DateTime.MinValue || criteria.DateOfBirth == DateTime.MinValue)
            {
                criteria.DOB = null;
            }

            var parameters = new
            {  
                CaseNumber = string.IsNullOrEmpty(criteria.CaseSSN) ? null : criteria.CaseSSN,
                DocumentNumber = criteria.DocumentNumber,
                Criteria = criteria.IsUniqueSearch
                    ? "IsUniqueSearch"
                    : (!string.IsNullOrEmpty(criteria.CaseSSN)
                        ? "CaseNumber"
                        : criteria.DocumentNumber.HasValue
                            ? "DocumentNumber"
                            : "IsAdvanceSearch")
            };

            var list = await _connection.QueryAsync<AdvanceSearchViewModel>(
                        "dbo.USP_AdvancedSearch",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

            return list;
        }


        public async Task<IEnumerable<AdvanceSearchViewModel>> GetImportSearchList(CaseAdvanceSearchCriteria criteria)
        {
            IEnumerable<AdvanceSearchViewModel> list = new List<AdvanceSearchViewModel>();

            try
            {
                var parameters = new { 
                SSN = criteria.CaseSSN,
                CaseStatus = criteria.CaseStatus,
                CountyID = criteria.CountyID,
                LastName = criteria.LastName,
                FirstName = criteria.FirstName,
                CaseWorkerID = criteria.CaseWorkerID  
                };
                list = await _connection.QueryAsync<AdvanceSearchViewModel>(
                    "[dbo].[USP_IBM_import_search]",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );               

            }
            catch (Exception ex)
            {

            }
            return list;
        }
        public async Task<IEnumerable<AdvanceSearchViewModel>> caserecordinfo(int id)
        {
            string CaseNumber = "";
            IEnumerable<AdvanceSearchViewModel> CaseRecord = new List<AdvanceSearchViewModel>();
            var parameters = new
            {
                CaseInfoId = id,
                CaseNumber = CaseNumber
            };
            CaseRecord =await _connection.QueryAsync<AdvanceSearchViewModel>(
                "[dbo].[USP_Casserecord_info]",
                parameters,
                commandType: CommandType.StoredProcedure
            );
            return CaseRecord;
        }
        public async Task<IEnumerable<AdvanceSearchViewModel>> caserecordinfoList(int id, CaseAdvanceSearchCriteria criteria)
        {
            string CaseNumber = "";
            IEnumerable<AdvanceSearchViewModel> list = new List<AdvanceSearchViewModel>();
            var parameters = new
            {
                CaseInfoId = id,
                CaseNumber = CaseNumber
            };
            list =await _connection.QueryAsync<AdvanceSearchViewModel>(
                "[dbo].[USP_Casserecord_info]",
                parameters,
                commandType: CommandType.StoredProcedure
            );      
            return list;
        }
        public async Task<IEnumerable<ScanningInfo>> DocumentNotesInfo(int id)
        {
            IEnumerable<ScanningInfo> CaseRecord = new List<ScanningInfo>();
            var parameters = new
            {
                DocumentID = id
            };
            CaseRecord =await _connection.QueryAsync<ScanningInfo>(
                "[dbo].[USP_DocumentNotes_search]",
                parameters,
                commandType: CommandType.StoredProcedure
            );           
            return CaseRecord;
        }
    }
}

