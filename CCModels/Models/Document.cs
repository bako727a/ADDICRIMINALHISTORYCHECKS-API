using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCModels.Models
{
    public class Document
    {
        public int? DocumentID { get; set; }
        public DateTime DateAssigned { get; set; }
        public int? UserAssgined { get; set; }
        public int? PageCount { get; set; }
        public bool IsDeleted { get; set; }
        public int? CaseInfoID { get; set; }
        public int? DocumentTypeID { get; set; }
        public int? DocumentSubTypeID { get; set; }
        public string FilePath { get; set; }
        public string Description { get; set; }
        public string DocumentNote { get; set; }
        public DateTime DocumentScannedDateTime { get; set; }
        public string MFRMDescription { get; set; }
        public bool MFRMDocType { get; set; }
        public int? CaseTypeID { get; set; }
        public DateTime? ReviewPeriod { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public int? LastUpdatedBy { get; set; }
        public string DocumentTypeName { get; set; }
        public string DocumentSubtypeName { get; set; }
        public string CreatedByName { get; set; }
        public string LastUpdatedByName { get; set; }
        public string CaseSSN { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string CaseName { get; set; }
        public string OldCaseSSN { get; set; }
        public string UserCredentials { get; set; }
        public Boolean fullCalendarDate { get; set; }
        public int FAJOBSDocumentNoteCount { get; set; }
        public string FAJOBSDocumentNote { get; set; }
        public bool CertAccessDate { get; set; }
        public string PDFextractdata { get; set; }
        public string ColorCode
        {
            get
            {
                if ((this.DocumentTypeName == "PA-Eligibility Period" && this.DocumentSubtypeName == "PA- Application") || (this.DocumentTypeName == "KINSHARE-Referral Period" && this.DocumentSubtypeName == "KINSHARE- Application Referral Forms")
                    || (this.DocumentTypeName == "SUP-Eligibility Period" && this.DocumentSubtypeName == "SUP- Application"))
                {
                    return "#82CAFF";
                }
                else if ((this.DocumentTypeName == "PA-Eligibility Period" && this.DocumentSubtypeName == "PA- Redetermination/Review") || (this.DocumentTypeName == "SUP-Eligibility Period" && this.DocumentSubtypeName == "SUP- Redetermination/Review"))
                {
                    return "#BDB76B";
                }
                else if ((this.DocumentTypeName == "PA- Permanent Docs" && this.DocumentSubtypeName == "PA- Narrative") || (this.DocumentTypeName == "KINSHARE- Referral Period" && this.DocumentSubtypeName == "KINSHARE- Narrative")
                    || (this.DocumentTypeName == "SUP- Permanent Docs" && this.DocumentSubtypeName == "SUP- Narrative") || (this.DocumentTypeName == "JOBS- Case Management" && this.DocumentSubtypeName == "JOBS- Narrative"))
                {
                    return "#FFB6C1";
                }
                else if ((this.DocumentTypeName == "JOBS- Case Management" && this.DocumentSubtypeName == "JOBS- Assessment Forms"))
                {
                    return "#7171BC";
                }
                else if ((this.DocumentTypeName == "JOBS- Case Management" && this.DocumentSubtypeName == "JOBS- Correspondence"))
                {
                    return "#ECC4F1";
                }
                else
                {
                    return "black";
                }
            }
        }

    }
}
