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
        public class AdminService : IAdminService
        {
            private readonly AdminRepo _repository;
            public AdminService(AdminRepo repository)
            {
                _repository = repository;
            }
            public async Task<GridResult<Users>> GetUsers(Users User)
            {
                var list = await _repository.GetUsers(User);
                return new GridResult<Users>()
                {
                    Data = list,
                    PageIndex = User.PageIndex,
                    PageSize = User.PageSize,
                    TotalRecords = list.Count()
                };
            }
            public void adduser(Users User)
            {
                _repository.adduser(User);
            }
            public void updateuser(Users User)
            {
                _repository.updateuser(User);
            }
            public void deleteuser(Users user)
            {
                _repository.removeuser(user);
            }
        }
    }

