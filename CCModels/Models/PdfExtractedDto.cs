using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCModels.Models
{
    public class PdfExtractedDto
    {
        public string FileName { get; set; }
        public DateTime ExtractedOn { get; set; }

        // Raw extracted text
        public string ExtractedText { get; set; }

        // If the PDF contains key-value structured data
        public Dictionary<string, string> Fields { get; set; } = new();

        // If you extract tables from the PDF
        public List<PdfTableDto> Tables { get; set; } = new();

        // Any errors found during extraction
        public string ErrorMessage { get; set; }
    }

    public class PdfTableDto
    {
        public string TableName { get; set; }
        public List<Dictionary<string, string>> Rows { get; set; } = new();
    }
}
