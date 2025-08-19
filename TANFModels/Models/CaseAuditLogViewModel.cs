using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TANFModels.Models
{
    public class CaseAuditLogViewModel
    {
        public int AuditID { get; set; }
        public int CaseInfoID { get; set; }
        public int UserID { get; set; }
        public string CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public string DocumentType { get; set; }
        public string DocumentSubType { get; set; }
        public int DocumentID { get; set; }
        public int CountyID { get; set; }
        public string Action { get; set; }
        public string ClientName { get; set; }
        public string CountyName { get; set; }
        public string CreatedByUserName { get; set; }
        public int CaseNumber { get; set; }
    }
}
