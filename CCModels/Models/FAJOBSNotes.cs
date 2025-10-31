using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCModels.Models
{
    public class FAJOBSNotes
    {
        public int DocID { get; set; }
        public string DocumentNotes { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedUser { get; set; }
        public string CaseName { get; set; }
    }
}
