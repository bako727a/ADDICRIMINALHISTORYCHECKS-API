using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCModels.Models
{
    public class ImportedDocument
    {
        public int Id { get; set; }

        public string CaseNumber { get; set; } = "";
        public string CaseStatus { get; set; } = "";
        public string CountyName { get; set; } = "";
        public string DocumentNumber { get; set; } = "";
        public string DocumentTypeName { get; set; } = "";
        public string DocumentSubtypeName { get; set; } = "";
        public string Description { get; set; } = "";
        public string ResultMessage { get; set; } = "";
        public string Username { get; set; } = "";

        public string OriginalFileName { get; set; } = "";
        public string ExtractedText { get; set; } = "";     // DB column for extracted text
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}
