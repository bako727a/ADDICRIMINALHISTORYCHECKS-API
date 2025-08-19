using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TANFModels.Models
{
    public class AdvanceSearchViewModel
    {
        public int CaseInfoID { get; set; }
        public string CaseStatus { get; set; }
        public int CaseWorkerID { get; set; }
        public int CountyID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Suffix { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string CaseSSN { get; set; }
        public bool IsConfidential { get; set; }
        public bool IsAudit { get; set; }
        public int DocumentCount { get; set; }
        public int AuditAssignedUserID { get; set; }
        public string CountyName { get; set; }
        public string SearchType { get; set; }
        public string ReviewPeriod { get; set; }
        public int LoginUserID { get; set; }
        public int LoginUserRoleID { get; set; }
        public string UserCredentials { get; set; }
        public bool IsImport { get; set; }
        public int CaseNotesCount { get; set; }
    }
}
