using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.BkgOperations
{
    //UAT-2370 : Supplement SSN Processing updates
    public class BkgSSNExceptionNotificationDataContract
    {
        public Int32 OrganizationUserID { get; set; }
        public Int32 BkgServiceID { get; set; }
        public String ApplicantName { get; set; }
        public String SSN { get; set; }
        public String ResultText { get; set; }
        public Int32 VendorOrderLineItem { get; set; }
        public Int32 BkgOrderID { get; set; }
        public Int32 SelectedNodeID { get; set; }
    }
}
