using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCModels.Models
{
    public class CaseNote
    {
        public int CaseInfoID { get; set; }
        public string CaseNotes { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedUser { get; set; }
    }
}
