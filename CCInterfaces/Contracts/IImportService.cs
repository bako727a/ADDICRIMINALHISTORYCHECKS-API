using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCModels.Models;

namespace CCInterfaces.Contracts
{
    public interface IImportService
    {
        int ImportDocuments(DocumentViewModel document);
    }
}
