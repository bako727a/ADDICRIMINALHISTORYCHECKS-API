using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCModels.Models
{
    public class Users
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public DateTime LastLoginDateTime { get; set; }
        public int? RoleID { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int CreatedBy { get; set; }
        public DateTime ModifiedDatetime { get; set; }
        public int ModifiedBy { get; set; }
        public int? CountyID { get; set; }
        public string CountyName { get; set; }
        public string RoleName { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public class Entitlement
    {
        public string EntCd { get; set; }
        public string EntDesc { get; set; }
        public int EntitleID { get; set; }
        public bool IsActive { get; set; }
    }

    public class SecurityRole
    {
        public int RoleID { get; set; }
        public string RoleName { get; set; }
    }

    public class DashboardStats
    {
        public int DocumentsCount { get; set; }

        public int CasesCount { get; set; }
    }
}
