using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCModels.Models
{
    public class CaseAdvanceSearchCriteria
    {
        public string CaseSSN { get; set; }
        //public int? CaseNumber { get; set; }
        public int? DocumentNumber { get; set; }
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime? DOB { get; set; }
        public string? CaseStatus { get; set; }
        public int? CaseWorkerID { get; set; }
        public int? CaseType { get; set; }
        public int? CountyID { get; set; }
        public int? DocumentTypeID { get; set; }
        public int? DocumentSubTypeID { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        [DataType(DataType.Date)]
        public DateTime? ReviewPeriod { get; set; }
        public string? ReviewDate { get; set; }
        public bool IsUniqueSearch { get; set; }
        public int LoginUserID { get; set; }
        public int? LoginUserRoleID { get; set; }
        public string UserCredentials { get; set; }
        public int? userCountyId { get; set; }
        public bool isConfidentialAllowed { get; set; }
        public bool isAudit { get; set; }
        public string Criteria { get; set; }
    }
}
