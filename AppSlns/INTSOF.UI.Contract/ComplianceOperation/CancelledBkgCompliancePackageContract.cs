using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ComplianceOperation
{
    [Serializable]
    public class CancelledBkgCompliancePackageContract
    {
        public Int32 OrderID { get; set; }
		public Int32 PartialOrderCancellationTypeID { get; set; }
		public Int32 OrderStatusID { get; set; }
		public String PackageName { get; set; }
		public DateTime? CancellationReqDate { get; set; }
        public Boolean BkgIsPartialOrderCancelled { get; set; }
        public Boolean IsCompliancePackage { get; set; }
    }
}
