using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TANFModels.Helper
{
    public class GridResult<T>
    {
        public int TotalRecords { get; set; }
        public IEnumerable<T> Data { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
