using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TANFInterfaces.Contracts;
using TANFModels.Helper;
using TANFModels.Models;
using TANFRepo.Repos;

namespace TANFInterfaces.Services
{
    public class AdvancedSearchService : IAdvanceSearchService
    {
        private readonly AdvanceSearchRepo _repository;
        public AdvancedSearchService(AdvanceSearchRepo repository)
        {
            _repository = repository;
        }

        public async Task<GridResult<AdvanceSearchViewModel>> AdvancedSearch(CaseAdvanceSearchCriteria criteria)
        {
            var list =await _repository.GetList(criteria);
            return new GridResult<AdvanceSearchViewModel>()
            {
                Data = list,
                PageIndex = criteria.PageIndex,
                PageSize = criteria.PageSize,
                TotalRecords = list.Count()
            };
        }
        public async Task<GridResult<AdvanceSearchViewModel>> Importsearch(CaseAdvanceSearchCriteria criteria)
        {
            var list =await _repository.GetImportSearchList(criteria);
            return new GridResult<AdvanceSearchViewModel>()
            {
                Data = list,
                PageIndex = criteria.PageIndex,
                PageSize = criteria.PageSize,
                TotalRecords = list.Count()
            };
        }
        public async Task<IEnumerable<AdvanceSearchViewModel>> caserecordinfo(int id)
        {
            var list =await _repository.caserecordinfo(id);
            return list;
        }

        public async Task<GridResult<AdvanceSearchViewModel>> caserecordinfoList(int id, CaseAdvanceSearchCriteria criteria)
        {
            var list =await _repository.caserecordinfoList(id, criteria);
            return new GridResult<AdvanceSearchViewModel>()
            {
                Data = list,
                TotalRecords = list.Count()
            };
        }
        public async Task<IEnumerable<ScanningInfo>> DocumentNotesbyDocumentID(int id)
        {
            var list =await _repository.DocumentNotesInfo(id);
            return list;
        }
    }
}
