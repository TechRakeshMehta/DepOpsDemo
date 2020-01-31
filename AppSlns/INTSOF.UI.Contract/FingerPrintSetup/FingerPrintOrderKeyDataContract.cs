using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INTSOF.UI.Contract.FingerPrintSetup
{
    public class FingerPrintOrderKeyDataContract
    {
       
        public String CBIUniqueID { get; set; }
        public String ReasonFingerprinted { get; set; }
        public String AcctName { get; set; }
        public String BillingORI { get; set; }
 
    }
}
