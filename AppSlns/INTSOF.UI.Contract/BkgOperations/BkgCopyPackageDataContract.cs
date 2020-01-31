using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.BkgOperations
{
    public class BkgCopyPackageDataContract
    {
        public Int32 ID { get; set; }
        public Int32 PackageSubscriptionID { get; set; }
        public String DocXml { get; set; }

        public Int32? BkgOrderID { get; set; }
    }
}
