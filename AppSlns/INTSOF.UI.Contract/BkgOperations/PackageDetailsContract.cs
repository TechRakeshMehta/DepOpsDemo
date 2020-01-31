using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.BkgOperations
{
    [Serializable]
    public class PackageDetailsContract
    {
        public String BPA_Name { get; set; }
        public String BPA_Description { get; set; }
        public Int32? PackagePrice { get; set; }
    }
}
