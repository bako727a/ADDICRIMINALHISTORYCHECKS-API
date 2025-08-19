using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TANFModels.Models
{
    public class CaseManagementSearchCriteria
    {
        public string CaseSSN { get; set; }
        public int? CountyID { get; set; }
        public int CaseWorkerID { get; set; }
        public int? LoginUserRoleID { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public string UserCredentials { get; set; }
        public int? userCountyId { get; set; }
        public bool isConfidentialAllowed { get; set; }
        public int SkipRecords { get; private set; }
        public int RowsPerPage { get; set; }
    }
}
