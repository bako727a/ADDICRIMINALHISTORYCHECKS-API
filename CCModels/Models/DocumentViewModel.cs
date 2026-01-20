using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCModels.Models
{
    public class DocumentViewModel
    {
        public int DocumentID { get; set; }
        public DateTime? DateAssigned { get; set; }
        public int UserAssgined { get; set; }
        public int PageCount { get; set; }
        public bool IsDeleted { get; set; }
        public int CaseInfoID { get; set; }
        public int DocumentTypeID { get; set; }
        public int DocumentSubTypeID { get; set; }
        public string FilePath { get; set; }
        public string Description { get; set; }
        public string DocumentNote { get; set; }
        public string MFRMDescription { get; set; }
        public int MFRMDocType { get; set; }
        public DateTime? ReviewPeriod { get; set; }
        public string ReviewDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public int LastUpdatedBy { get; set; }
        public List<PdfViewModel> DataPdfFiles { get; set; }
        public string Username { get; set; }
        public DateTime DocumentScannedDateTime { get; set; }
        public string DocumentTypeName { get; set; }
        public string DocumentSubtypeName { get; set; }
        public int CountyID { get; set; }
        public string CountyName { get; set; }
        public string DocumentNumber { get; set; }
        public Boolean fullCalendarDate { get; set; }
        public string PdfExtractData { get; set; }
        public string ResultMessage { get; set; }
    }
}
