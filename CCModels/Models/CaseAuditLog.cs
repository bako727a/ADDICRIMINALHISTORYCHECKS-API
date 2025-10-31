using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCModels.Models
{
    public class CaseAuditLog
    {
        public int? CountyID { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string LoginUserID { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
}
