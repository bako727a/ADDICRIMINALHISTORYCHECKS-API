using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCModels.Models
{
    public class CaseRegistration
    {
        public int CaseSSN { get; set; }
        public string CaseStatus { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int CountyID { get; set; }
        public int SecurityRoleID { get; set; }
    }
}
