using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation
{

    [Serializable]
    [DataContract]
    public class RequirementItemPaymentContract
    {
        [DataMember]
        public Int32 TenantID { get; set; }
        [DataMember]
        public Int32 OrganizationUserProfileID { get; set; }
        [DataMember]
        public Int32 OrganizationUserID { get; set; }
        [DataMember]
        public Int32 OrderStatusID { get; set; }
        [DataMember]
        public Int32 PaymentOptionID { get; set; }
        [DataMember]
        public Decimal TotalPrice { get; set; }
        [DataMember]
        public Decimal PaidAmount { get; set; }
        [DataMember]
        public String MachineIP { get; set; }
        [DataMember]
        public Decimal GrandTotal { get; set; }
        [DataMember]
        public Int32 OrderRequestTypeID { get; set; }
        [DataMember]
        public Int32 CreatedByID { get; set; }
        [DataMember]
        public DateTime OrderDate { get; set; }
        [DataMember]
        public Boolean isOrderSaved { get; set; }
        [DataMember]
        public String responseMessage { get; set; }
        [DataMember]
        public String OrderNumber { get; set; }
        [DataMember]
        public Int32 orderID { get; set; }
        [DataMember]
        public String invoiceNumber { get; set; }
        [DataMember]
        public Int32 ItemPaymentOrderMappingEntityTypeID { get; set; }
        [DataMember]
        public Int32 OrderPackageTypeID { get; set; }
        [DataMember]
        public Int32 ApplicantComplianceCategoryDataStatusID { get; set; }
        [DataMember]
        public Int32 ApplicantComplianceItemDataStatusID { get; set; }
        [DataMember]
        public Int32 ApplicantRequirementCategoryDataStatusID { get; set; }
        [DataMember]
        public Int32 ApplicantRequirementItemDataStatusID { get; set; }

        //UI
        [DataMember]
        public Decimal ParkingPrice { get; set; }

        /// <summary>
        /// Used to identify the current Request Type i.e. New order, Change subscription,Parking Payment(newly added) etc.
        /// </summary>
        /// 
        [DataMember]
        public String OrderType
        {
            get;
            set;
        }
        [DataMember]
        public String PkgName { get; set; }
        [DataMember]
        public String CategoryName { get; set; }
        [DataMember]
        public String ItemName { get; set; }
        [DataMember]
        public Boolean IsRequirementPackage { get; set; }
        [DataMember]
        public Int32 PkgId { get; set; }
        [DataMember]
        public Int32 PkgSubscriptionId { get; set; }
        [DataMember]
        public Int32 ItemID { get; set; }
        [DataMember]
        public Int32 CategoryID { get; set; }
        [DataMember]
        public Int32 ItemDataId { get; set; }
        [DataMember]
        public Nullable<DateTime> ApprovalDate { get; set; }
        [DataMember]
        public String OrderStatus { get; set; }
        [DataMember]
        public String OrderStatusCode { get; set; }
        [DataMember]
        public Int32 ClinicalRotationID { get; set; }
        [DataMember]
        public Int32 RequirementHierarchyNodeId { get; set; }
        [DataMember]
        public Boolean? IsEditableByApplicant { get; set; }
    }
}
