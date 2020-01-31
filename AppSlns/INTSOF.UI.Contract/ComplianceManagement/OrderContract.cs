using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INTSOF.UI.Contract.ComplianceManagement
{
    public class OrderContract
    {
        //public Int32? OrganizationUserProfileID { get; set; }
        //public Int32? DeptProgramPackageID { get; set; }
        //public Int32? OrderStatusID { get; set; }
        //public Int32? PaymentOptionID { get; set; }
        //public DateTime? OrderDate { get; set; }
        //public DateTime? ApprovalDate { get; set; }
        //public Int32? ApprovedBy { get; set; }
        //public String Notes { get; set; }
        //public String IP { get; set; }

        public String InstituteHierarchy { get; set; }
        public String PackageName { get; set; }
        public String PaymentType { get; set; }
        public String OrderStatusCode { get; set; }
        public String OrderStatusName { get; set; }
        public String SSN { get; set; }
        public Int32 OrderId { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String RushOrderStatus { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Decimal? Amount { get; set; }
        public Boolean IsInvoiceApproval { get; set; }
        public Boolean IsInvoiceApprovalInitiated { get; set; }
        public Boolean AutomaticRenewalTurnedOff { get; set; }
        public Boolean HasActiveComplianceSubscription { get; set; }
        public Int32 PackageID { get; set; }
        public Boolean IsAutomaticRenewalForPackage { get; set; }
        public Int32 TotalCount { get; set; }
        public String OrderNumber { get; set; }
        public Boolean IsCardWithApproval { get; set; }
        // UAT-4490
        public String CancelledBy { get; set; }
        public DateTime? CancelledOn { get; set; }
        // UAT-4490
    }
}
