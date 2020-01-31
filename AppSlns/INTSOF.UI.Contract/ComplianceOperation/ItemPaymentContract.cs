using Entity.ClientEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ComplianceOperation
{
    [Serializable]
    public class ItemPaymentContract
    {
        public ItemPaymentContract()
        {
            RemoveItemPaymentSession = true;
        }
        public Int32 TenantID { get; set; }
        public Int32 OrganizationUserProfileID { get; set; }
        public Int32 OrganizationUserID { get; set; }
        public Int32 OrderStatusID { get; set; }
        public Int32 PaymentOptionID { get; set; }
        public Decimal TotalPrice { get; set; }
        public Decimal PaidAmount { get; set; }
        public String MachineIP { get; set; }
        public Decimal GrandTotal { get; set; }
        public Int32 OrderRequestTypeID { get; set; }
        public Int32 CreatedByID { get; set; }
        public DateTime OrderDate { get; set; }

        public String PrimaryEmailAddress { get; set; }

        public Boolean isOrderSaved { get; set; }
        public String responseMessage { get; set; }
        public String OrderNumber { get; set; }
        public Int32 orderID { get; set; }
        public String invoiceNumber { get; set; }
        public Int32 ItemPaymentOrderMappingEntityTypeID { get; set; }
        public Int32 OrderPackageTypeID { get; set; }
        public Int32 ApplicantComplianceCategoryDataStatusID { get; set; }
        public Int32 ApplicantComplianceItemDataStatusID { get; set; }

        public Int32 ApplicantRequirementCategoryDataStatusID { get; set; }
        public Int32 ApplicantRequirementItemDataStatusID { get; set; }
        //---------------------
        public Boolean? IsReviewerTypeAdmin { get; set; }
        public Boolean? IsReviewerTypeClientAdmin { get; set; }
        public DateTime? SubmissionDate { get; set; }
        public Int32? AssignedToUser { get; set; }
        public String ApplicantName { get; set; }
        public Int32 ApplicantComplianceItemnId { get; set; }
        public Int32? HierarchyNodeID { get; set; }
        //------------


        //UI
        public Decimal ParkingPrice { get; set; }

        /// <summary>
        /// Used to identify the current Request Type i.e. New order, Change subscription,Parking Payment(newly added) etc.
        /// </summary>
        public String OrderType
        {
            get;
            set;
        }
        public String PkgName { get; set; }
        public String CategoryName { get; set; }
        public String ItemName { get; set; }
        public Boolean IsRequirementPackage { get; set; }
        public Int32 PkgId { get; set; }
        public Int32 PkgSubscriptionId { get; set; }
        public Int32 ItemID { get; set; }
        public Int32 CategoryID { get; set; }
        public Int32 ItemDataId { get; set; }
        public Nullable<DateTime> ApprovalDate { get; set; }
        public String OrderStatus { get; set; }
        public String OrderStatusCode { get; set; }

        public Int32 ClinicalRotationID { get; set; }
        public Int32 RequirementHierarchyNodeId { get; set; }
        public AssignmentProperty AssignmentProperty { get; set; }
        public List<lkpItemMovementType> ListItemMovementType { get; set; }
        public Boolean RemoveItemPaymentSession { get; set; }
        public String CompleteItemPaymentClickHtml { get; set; }
        public Boolean RefreshItemSectionHtml { get; set; }
        public Boolean IsPaid { get; set; }
        public Boolean ShowSubmitButton { get; set; }
    }
}
