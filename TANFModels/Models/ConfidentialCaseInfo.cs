using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TANFModels.Models
{
    public class ConfidentialCaseInfo
    {
        public string[] checkUserArray { get; set; }
        public string[] checkArray { get; set; }
        public string CaseSSN { get; set; }   
        public int CaseWorkerID { get; set; }
        public int? UserCountyID { get; set; }
        public bool IsView { get; set; }
        public bool IsConfidential { get; set; }
    }
}
