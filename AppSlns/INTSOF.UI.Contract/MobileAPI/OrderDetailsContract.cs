using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.MobileAPI
{
    [DataContract]
    public class OrderDetailsContract
    {
        [DataMember]
        public Int32 OrderId { get; set; }
        [DataMember]
        public Int32 LocationID { get; set; }
        [DataMember]
        public Int32 DeptProgramPackageID { get; set; }
        [DataMember]
        public String OrderPackageType { get; set; }
        [DataMember]
        public String InstituteHierarchy { get; set; }
        [DataMember]
        public String PackageName { get; set; }
        [DataMember]
        public Int32 PackageID { get; set; }
        [DataMember]
        public Int32 BackageOrderPackageID { get; set; }
        [DataMember]
        public String PaymentTypeCode { get; set; }
        [DataMember]
        public String PaymentType { get; set; }
        [DataMember]
        public String OrderStatusCode { get; set; }
        [DataMember]
        public String OrderStatusName { get; set; }
        [DataMember]
        public DateTime? OrderDate { get; set; }
        [DataMember]
        public DateTime OrderPaidDate { get; set; }
        [DataMember]
        public decimal? Amount { get; set; }

        [DataMember]
        public Int32 DeptProgramMappingID { get; set; }

        [DataMember]
        public String OrderPackageTypeCode { get; set; }

        [DataMember]
        public String OrderNumber { get; set; }
        [DataMember]
        public Int32? BkgOrderStatusID { get; set; }
        [DataMember]
        public String BkgOrderStatusCode { get; set; }
        [DataMember]
        public String BkgOrderStatus { get; set; }
        [DataMember]
        public Int32 AppointmentStatusID { get; set; }
        [DataMember]
        public String AppointmentStatusCode { get; set; }
        [DataMember]
        public String AppointmentStatus { get; set; }
        [DataMember]
        public Boolean IsFileSentToCBI { get; set; }
        [DataMember]
        public Boolean IsRescheduleAvailable { get; set; }
        [DataMember]
        public Boolean IsOnsiteAppointment { get; set; }
        [DataMember]
        public Boolean IsColoradoFingerPrinting { get; set; }
        [DataMember]
        public Boolean IsOutOfState { get; set; } 
        [DataMember]
        public Boolean IsChangePaymentTypeVisible { get; set; }
        [DataMember]
        public Int32 BkgPackageHierarchyMappingId { get; set; }
        [DataMember]
        public Boolean IsBillingCode { get; set; }
        [DataMember]
        public DateTime StartDateTime { get; set; }
        [DataMember]
        public DateTime EndDateTime { get; set; }
        [DataMember]
        public string LocationName { get; set; }
        [DataMember]
        public Boolean IsSendForOnline { get; set; }
        [DataMember]
        public String BillingCode { get; set; }
    }
}
