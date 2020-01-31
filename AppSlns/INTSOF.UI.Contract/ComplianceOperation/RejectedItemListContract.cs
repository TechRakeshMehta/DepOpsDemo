using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ComplianceOperation
{
    public class RejectedItemListContract
    {
        public Int32 ApplicantComplianceItemID { get; set; }
        public Int32 ComplianceItemID { get; set; }
        public String ItemName { get; set; }
        public String RejectionNotes { get; set; }
        public String PackageName { get; set; }
        public String CategoryName { get; set; }
    }
}
