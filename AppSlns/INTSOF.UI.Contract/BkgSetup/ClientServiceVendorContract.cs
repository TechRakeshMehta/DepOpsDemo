using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.BkgSetup
{
    public class ClientServiceVendorContract
    {
        public String BkgServiceName { get; set; }
        public Int32 BkgServiceID { get; set; }
        public String ExtServiceName { get; set; }
        public Int32 ExtServiceID { get; set; }
        public String ExtServiceCode { get; set; }
        public String State { get; set; }

    }
}
