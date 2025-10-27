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
using TANFDataAccess.Context;
using TANFModels.Models;

namespace TANFRepo.Repos
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
            if (criteria.DOB == Convert.ToDateTime("01/01/0001 05:00:00 AM") || criteria.DateOfBirth == Convert.ToDateTime("01/01/0001 12:00:00 AM"))
            {
                criteria.DOB = null;
            }
            IEnumerable<AdvanceSearchViewModel> list = new List<AdvanceSearchViewModel>();
            var parameters = new {
                Type = criteria.IsUniqueSearch
                ? "IsUniqueSearch"
                : (!string.IsNullOrEmpty(criteria.CaseSSN) && criteria.CaseSSN != "string"
                    ? "CaseNumber"
                    : (criteria.DocumentNumber.HasValue && criteria.DocumentNumber > 0
                        ? "DocumentNumber"
                        : "IsAdvanceSearch")),  
             LastName = criteria.LastName == "string" ? null : criteria.LastName,
             FirstName = criteria.FirstName == "string" ? null : criteria.FirstName,
             CountyID = criteria.CountyID == 0 ? null : criteria.CountyID,             
            };
            await _connection.QueryAsync<AdvanceSearchViewModel>(
                "[dbo].[USP_AdvancedSearch]",
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

