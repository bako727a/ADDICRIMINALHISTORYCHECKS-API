using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TANFInterfaces.Contracts;
using TANFModels.GenericModelElements;
using TANFModels.Models;
using TANFRepo.IRepos;
using TANFRepo.Repos;

namespace TANFInterfaces.Services
{
    public class LookupService : ILookupService
    {
        private readonly ILookupRepo _repository;

        public LookupService(ILookupRepo repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<KeyValue>> GetList(string type, int CountyID)
        {
 
           return await _repository.GetList(type, CountyID);
        }
        public async Task<IEnumerable<KeyValue>> GetDocSubTypeList(int typeId)
        {
            return await _repository.GetDocSubTypeList(typeId);
        }
        public async Task<IEnumerable<Users>> GetUserList(string Username, int id)
        {
            return await _repository.GetUserList(Username, id);
        }
    }
}
