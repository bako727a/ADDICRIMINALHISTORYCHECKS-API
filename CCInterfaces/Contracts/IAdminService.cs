using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCModels.Helper;
using CCModels.Models;

namespace CCInterfaces.Contracts
{
    public interface IAdminService
    {
        Task<GridResult<Users>> GetUsers(Users User);
        void adduser(Users User);
        void updateuser(Users User);
        void deleteuser(Users User);
    }
}
