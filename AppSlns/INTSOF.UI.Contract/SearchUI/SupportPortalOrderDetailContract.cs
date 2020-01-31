using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.SearchUI
{
    [Serializable]
    public class SupportPortalOrderDetailContract
    {
        public Int32 OrderId { get; set; }
        public String OrderNumber { get; set; }
        public String InstituteHierarchy { get; set; }
        public String PackageName { get; set; }
        public String PackageLabel { get; set; }
        public String PaymentStatus { get; set; }
        public String OrderFlag { get; set; }
        public String ArchiveStatus { get; set; }
        public String ActiveStatus { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? OrderPaidDate { get; set; }
        public DateTime? OrderCompleteDate { get; set; }
        public Boolean? ExceptionPending { get; set; }
        public String PackageType { get; set; }
        public Int32? ComplianceCategoryID { get; set; }
        public Int32? PackageSubscriptionID { get; set; }
        public Int32? ApplicantComplianceItemID { get; set; }
        public Int32? BkgOrderId { get; set; }
        public String ArchiveStatusCode { get; set; }
        public String PackageTypeCode { get; set; }
        public String CompliancePackageStatusCode { get; set; }
        public String OrderStatus { get; set; }
        public Boolean? IsOrderItemsComplete { get; set; }
        public Boolean? IsServiceGroupStatusComplete { get; set; }
        public Int32? PackageId { get; set; }

        public Boolean? IsServiceGroupFlagged { get; set; }
        public String  ServicreGroupName { get; set; }
        public String OrderStatusCode { get; set; }
        public Boolean IsOrderRenewed { get; set; }

    }
}
