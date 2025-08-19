using TANFModels.GenericModelElements;
using TANFModels.Models;

namespace TANFInterfaces.Contracts
{
    public interface ILookupService
    {
        Task<IEnumerable<KeyValue>> GetList(string type, int CountyID);      
     
        Task<IEnumerable<KeyValue>> GetDocSubTypeList(int typeId);
        Task<IEnumerable<Users>> GetUserList(string username, int id);
    }
}
