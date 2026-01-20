using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCModels.Models
{
    public class CHCDocumentModel
    {
        public int DocumentID { get; set; }
        public int CaseInfoID { get; set; }
        public int CreatedUserID { get; set; }
        public int LastUpdatedUserID { get; set; }
        public string DocumentDescription {  get; set; }
        public string DocumentFilePath { get; set; }
        public string PDFExtractedData { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public string DocumentNote { get; set; }
        public DateTime RevieworCertPeriod {  get; set; }
        public int DocumentCategoryID { get; set; }
        public int DocumentTypeID { get; set; }
        public string CreatedUsername { get; set; }
        public int CountyID { get; set; }
        public string CountyName { get; set; }
    }
}
