using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.BkgSetup
{
    public class ServiceAttachedFormContract
    {
        public Int32 SF_ID { get; set; }
        public String FormName { get; set; }
        public String ParentFormName { get; set; }
        public Boolean ServiceFormDispatchMode { get; set; }
        public Int32 ParentSvcFormID { get; set; }
        public Int32 SystemDocumentID { get; set; }
        public String SystemDocumentFileName { get; set; }
        public String ServiceFormDispatchType { get; set; }

    }
}
