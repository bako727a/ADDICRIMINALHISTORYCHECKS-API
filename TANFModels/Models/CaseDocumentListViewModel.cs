using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace TANFModels.Models
{
    public class CaseDocumentListViewModel
    {

        //Case Information regarding the document list.
        public string CaseSSN { get; set; }
        public string CaseStatus { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CountyName { get; set; }
        public string UserCredentials { get; set; }
        public bool IsConfidential { get; set; }

        //Document List retrieval 
        public IEnumerable<Document> DocumentsList { get; set; }
    }
}
