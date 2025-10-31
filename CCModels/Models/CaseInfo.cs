using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCModels.Models
{
    public class CaseInfo
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
        public string CaseNotes { get; set; }
    }
    public class DashboardCaseSearchCriteria
    {
        public int? UserId { get; set; }
        public int? Type { get; set; }
        public int? CountyID { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
