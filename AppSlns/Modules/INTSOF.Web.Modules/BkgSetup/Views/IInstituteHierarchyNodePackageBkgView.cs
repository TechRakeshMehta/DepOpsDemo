using Entity.ClientEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.BkgSetup.Views
{
    public interface IInstituteHierarchyNodePackageBkgView
    {
        IInstituteHierarchyNodePackageBkgView CurrentViewContext { get; }
        String ErrorMessage { get; set; }
        String SuccessMessage { get; set; }
        String InfoMessage { get; set; }
        String PageType { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        Int32 SelectedTenantId { get; set; }
        Int32 DefaultTenantId { get; set; }
        Int32 DeptProgramMappingID { get; set; }
        Int32 BkgPackageHierarchyMappingID { get; set; }
        Int32 ParentID { get; set; }
        List<InstitutionNodeType> ListInstitutionNodeType { set; }
        List<InstitutionNode> ListInstitutionNode { set; }
        Int32 SelectedNodeTypeId { get; set; }
        Int32 SelectedNodeId { get; }
        List<lkpPaymentOption> ListPaymentOption { set; }
        List<Int32> SelectedPaymentOptions { get; set; }
        List<Int32> SelectedMappedPaymentOptions { get; set; }
        Int32 SelectedPackageId { get; set; }
        List<BkgPackageHierarchyMapping> ProgramPackages { get; set; }
        List<GetChildNodesWithPermission> NodeList { get; set; }
        List<Int32> ChildNodeList { get; set; }
        String NodeLabel { get; set; }
        String SelectedNodeLabel { get; set; }
        Int32 NodeId { get; set; }
        String SelectedNodeName { get; }
        List<vwHierarchyPermission> HierarchyPermissionList { get; set; }
        List<Entity.OrganizationUser> OrganizationUserList { get; set; }
        List<lkpPermission> UserPermissionList { get; set; }
        Int32 OrganizationUserID { get; set; }
        Int16 PermissionId { get; set; }
        Int32 HierarchyPermissionID { get; set; }
        String PermissionCode { get; set; }
        decimal BasePrice { get; set; }
        Boolean IsPackageExclusive { get; set; }
        Boolean TransmitToVendor { get; set; }
        Boolean RequireFirstReview { get; set; }
        List<lkpPackageSupplementalType> LstSupplemantalType { get; set; }
        Int16 SelectedSupplemantalTypeID { get; set; }
        String Instruction { get; set; }
        String PriceText { get; set; }
        Int32 SelectedExtVendorAcctID { get; set; }
        List<ExternalVendorAccountMappingDetails> ListExtVendorAcctMappingDetails { get; set; }
        List<Entity.ExternalVendorAccount> ListExtVendorAcct { set; }
        Int32 SelectedExtVendorID { get; set; }
        List<Entity.ExternalVendor> ListExtVendor { set; }
        List<InstHierarchyRegulatoryEntityMappingDetails> ListRegulatoryEntityMappingDetails { get; set; }
        List<Entity.lkpRegulatoryEntityType> ListRegulatoryEntityType { set; }
        Int16 SelectedRegulatoryEntityTypeID { get; set; }
        Int32? MaxNumberOfYearforResidence { get; set; }
        Boolean IsBkgPackageAvailableForOrder { get; set; }
        Boolean IsBkgPackageAvailableForHRPortal { get; set; }

        /// <summary>
        /// Stores the list of Payment Option Ids selected for a Package
        /// </summary>
        List<Int32> lstPaymentOptionIds { get; set; }

        /// <summary>
        /// Identify whether the Node will be available for the Order, in order flow or not
        /// </summary>
        Boolean IsAvailableForOrderAddMode
        {
            get;
        }

        /// <summary>
        /// Identify whether the Node will be available for the Order, in order flow or not
        /// </summary>
        Boolean IsAvailableForOrderEditMode
        {
            get;
            set;
        }

        /// <summary>
        /// Identify whether the Selected Node is a Root Node
        /// </summary>
        Boolean IsRootNode
        {
            get;
            set;
        }

        #region UAT-1176 - Node Employment
        Boolean IsEmploymentTypeAddMode
        {
            get;
            set;
        }
        Boolean IsEmploymentTypeEditMode
        {
            get;
            set;
        }
        #endregion

        String SplashPageUrlAddMode
        {
            get;
            set;
        }

        String SplashPageUrlEditMode
        {
            get;
            set;
        }

        #region UAT-2073: New Payment setting: School approval for MO and CC Kaplan would like a way to keep students from paying for orders of a specific package without their approval.
        Int32 PaymentApprovalIDAddMode
        {
            get;
            set;
        }
        Int32 PaymentApprovalID
        {
            get;
            set;
        }
        Int32 PaymentApprovalIDForPackage
        {
            get;
            set;
        }
        List<lkpPaymentApproval> PaymentApprovalList
        {
            set;
        }

        #endregion

        #region UAT-2438
        List<lkpPDFInclusionOption> ListPDFInclusionOption { set; }
        Int32 PDFInclusionID { get; set; }
        Int32 PDFInclusionIDAddMode { get; set; }

        #endregion

        #region UAT-2842

        List<lkpResultSentToApplicant> ListResultSentToApplicantOptions { set; }
        Int32 ResultSentToApplicantID { get; set; }

        Int32 ResultSentToApplicantIDAddMode { get; set; }

        #endregion
        Boolean IsPackagesNotAvailableForOrder
        {
            get;
            set;
        }

        #region UAT-3268
        List<BackgroundPackage> lstBkgPackages { get; set; }
        Boolean IsReqToQualifyInRotation { get; set; }
        Boolean IsAdditionalPriceAvailable { get; set; }
        List<lkpPaymentOption> AdditionalPaymentOptions { get; set; }
        Decimal? AdditionalPrice { get; set; }
        Int32? SelectedAdditonalPaymentOptionID { get; set; }

        List<lkpBkgHierarchyNodeExemptedType> ListExemptedHierarchyNodeOptions { set; }
        Int32 NodeExemptedInRotaionAddMode
        {
            get;
            set;
        }
        Int32 NodeExemptedInRotaionEditMode
        {
            get;
            set;
        }
        #endregion
        Boolean IsPackageBundleAvailableForOrder
        {
            get;
            set;
        }
    }
}
