using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCModels.Models
{
    public class ScannedAndCreateDocumentInfo
    {
        public int? DocumentId { get; set; }
        public int CaseInfoId { get; set; }
        public int DocumentTypeID { get; set; }
        public int DocumentSubTypeId { get; set; }
        public string ReviewDate { get; set; }
        public string DocumentNotesDescription { get; set; }
    }
}
