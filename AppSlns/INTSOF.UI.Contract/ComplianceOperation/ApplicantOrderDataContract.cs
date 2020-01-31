using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;
using INTSOF.UI.Contract.BkgOperations;

namespace INTSOF.UI.Contract.ComplianceOperation
{
    /// <summary>
    /// Contract to carry Applicant order submission related Data
    /// </summary>
    public class ApplicantOrderDataContract
    {
        public Entity.ClientEntity.OrganizationUserProfile OrganizationUserProfile { get; set; }
        public Int32 AddressIdMaster { get; set; }
        public Guid AddressHandleIdMaster { get; set; }
        public Int32 ProgramPackageSubscriptionId { get; set; }
        //public Int32 SelectedPaymentModeId { get; set; }
        public List<TypeCustomAttributes> lstAttributeValues { get; set; }
        //UAT 1438: Enhancement to allow students to select a User Group.
        public List<ApplicantUserGroupMapping> lstAttributeValuesForUserGroup { get; set; }
        public Boolean IsUserGroupCustomAttributeExist { get; set; }

        public Int32 LastNodeDPMId { get; set; }
        public List<BackgroundPackagesContract> lstBackgroundPackages { get; set; }
        public Int32 TenantId { get; set; }
        public Int32 OrderCreatedStatusId { get; set; }
        public List<Entity.ResidentialHistoryProfile> lstResidentialHistoryProfile { get; set; }
        public List<Entity.PersonAliasProfile> lstPersonAliasProfile { get; set; }

        /// <summary>
        /// Data of the Custom forms of the Background orders, if any
        /// </summary>
        public List<BackgroundOrderData> lstBkgOrderData { get; set; }

        /// <summary>
        /// OrderStatusTypeID - PK of ams.lkpOrderStatusType
        /// </summary>
        public Int32 BkgOrderStatusTypeId { get; set; }

        /// <summary>
        /// Stores the Data extracted from the Pricing XML
        /// </summary>
        public List<Package_PricingData> lstPricingData { get; set; }

        /// <summary>
        /// Status-Id for OrderLineItem which is being inserted
        /// </summary>
        public Int32 OrderLineItemStatusId { get; set; }

        /// <summary>
        /// List of BkgSvcAttributeGrps. Will be available, only ig BkgPackage is selected
        /// </summary>
        public List<BkgSvcAttributeGroup> lstSvcAttributeGrps { get; set; }

        /// <summary>
        /// Set true/false to Send Background Report of order-from BkgOrder-orderresultrequestedbyapplciant 
        /// </summary>
        public Boolean IsSendBackgroundReport { get; set; }

        /// <summary>
        /// Set true/false to Is Compliance Package Selected
        /// </summary>
        public Boolean IsCompliancePackageSelected { get; set; }

        /// <summary>
        /// String to store the UserBrowser agent in order flow
        /// </summary>
        public String UserBrowserAgentString { get; set; }

        /// <summary>
        /// List of Payment Options selected for each Compliance and Background Package
        /// </summary>
        public List<PkgPaymentGrouping> lstGroupedData { get; set; }

        public List<lkpOrderPackageType> lstOrderPackageTypes { get; set; }

        public List<lkpOrderStatu> lstOrderStatus { get; set; }

        /// <summary>
        /// PaymentOptionId selected for the Compliance Package
        /// </summary>
        public Int32 CompliancePkgPaymentOptionId { get; set; }

        /// <summary>
        ///  OPSG_SvcGrpStatusTypeID of the ams.BkgOrderPackageSvcGroup table,
        ///  to represnt the LookUp id for the 'New' Status of the Service group
        /// </summary>
        public Int32 NewSvcGrpStatusTypeId { get; set; }

        /// <summary>
        ///  OPSG_SvcGrpReviewStatusTypeID of the ams.BkgOrderPackageSvcGroup table,
        ///  to represnt the LookUp id for the 'New' REVIEW Status of the Service group
        /// </summary>
        public Int32 NewSvcGrpReviewStatusTypeId { get; set; }

        //UAT-3850
        public Boolean IsBillingCodeAmountAvlbl { get; set; }

        public Decimal BillingCodeAmount { get; set; }

        /// <summary>
        /// To Store ApplicantInviteSubmitStatus Type Code, either Draft or Transmit on the current DPM_ID.
        /// </summary>
        public String ApplicantInviteSubmitStatusTypeCode { get; set; }

        public Int16? AdminEntryLineItemStatusId { get; set; }
        public Int16? BkgAdminEntryOrderDetailDraftStatusId { get; set; }
        public Int16? BkgAdminEntryOrderDetailStatusId { get; set; }

        public DateTime? BkgAdminEntryOrderDetail_TransmittDate { get; set; }

        public Int32 AutoCompletedOrderLineItemStatusId { get; set; } //UAT-4498
        public Int32 CompletedSvcGrpStatusTypeId { get; set; } //UAT-4498
        public Int32 AutoReviewSvcGrpReviewStatusTypeId { get; set; } //UAT-4498
        public Int16 DispatchedSvcLineItemDispatchStatusId { get; set; } //UAT-4498

        //UAT-4775
        public Int16? BkgAdminEntryOrderDetailHoldStatusId { get; set; }
        public Int32 AdminOrderInprogressEventID { get; set; }
        public Int32 AdminOrderOnHoldEventID { get; set; }
    }

    /// <summary>
    /// Class to store the Pkg level payment options for the Packages.
    /// Parameter names are same as received from Client JSON received, to make coversion possible back to C# List
    /// </summary>
    public class ApplicantOrderPaymentOptions
    {
        /// <summary>
        /// Package Id, could be Compliance or Background
        /// </summary>
        public Int32 pkgid { get; set; }

        /// <summary>
        /// Payment OptionId selected for the Package
        /// </summary>
        public Int32 poid { get; set; }

        /// <summary>
        /// Stores whether it is a background package 
        /// </summary>
        public Boolean isbkg { get; set; }

        /// <summary>
        /// Identify the zero payment groups
        /// </summary>
        public Boolean isZP { get; set; }

        /// <summary>
        /// To store additional price payment type ID if exists.
        /// </summary>
        public String additionalPoid { get; set; }

        /// <summary>
        /// To store Any Options Approval Required
        /// </summary>
        public Boolean isAnyOptionsApprovalReq { get; set; }
    }
}
