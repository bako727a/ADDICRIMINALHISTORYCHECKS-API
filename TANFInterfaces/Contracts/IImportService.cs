using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TANFModels.Models;

namespace TANFInterfaces.Contracts
{
    public interface IImportService
    {
        int ImportDocuments(DocumentViewModel document);
    }
}
