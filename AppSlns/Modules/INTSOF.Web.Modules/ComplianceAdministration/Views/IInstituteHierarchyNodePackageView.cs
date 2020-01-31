using System;
using System.Collections.Generic;
using System.Text;
using Entity.ClientEntity;
using System.Linq;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.UI.Contract.Mobility;

namespace CoreWeb.ComplianceAdministration.Views
{
    public interface IInstituteHierarchyNodePackageView
    {
        String ErrorMessage { get; set; }
        String SuccessMessage { get; set; }
        String InfoMessage { get; set; }
        String PageType { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        Int32 TenantId { get; set; }
        Int32 DefaultTenantId { get; set; }
        Int32 DeptProgramMappingID { get; set; }
        Int32 DeptProgramPackageID { get; set; }
        Int32 ParentID { get; set; }
        IInstituteHierarchyNodePackageView CurrentViewContext { get; }
        List<InstitutionNodeType> ListInstitutionNodeType { set; }
        List<InstitutionNode> ListInstitutionNode { set; }
        //List<DeptProgramMapping> NodeList { get; set; }
        List<GetChildNodesWithPermission> NodeList { get; set; }
        List<Int32> ChildNodeList { get; set; }
        String NodeLabel { get; set; }
        String SelectedNodeLabel { get; set; }
        Int32 NodeId { get; set; }
        List<lkpPaymentOption> ListPaymentOption { set; }
        List<lkpDurationType> ListDurationType { set; }
        List<LookupContract> ListSuccessorNodes { set; }
        List<LookupContract> ListSuccessorPackages { get; set; }
        Int32 SelectedNodeTypeId { get; set; }
        Int32 SelectedNodeId { get; }
        String SelectedNodeName { get; }
        List<Int32> SelectedPaymentOptions { get; set; }
        List<Int32> SelectedMappedPaymentOptions { get; set; }
        Int32 SelectedPackageId { get; set; }
        List<DeptProgramPackage> ProgramPackages { get; set; }
        List<vwHierarchyPermission> HierarchyPermissionList { get; set; }
        List<vwHierarchyPermission> ComplianceHierarchyPermissionList { get; set; }
        List<Entity.OrganizationUser> OrganizationUserList { get; set; }
        List<lkpPermission> UserPermissionList { get; set; }
        Int32 OrganizationUserID { get; set; }
        Int16 PermissionId { get; set; }
        Boolean IsIncludeAnotherHierarchyPermissionType { get; set; }
        Int16 ProfilePermissionId { get; set; }
        Int16 VerificationPermissionId { get; set; }
        Int32 HierarchyPermissionID { get; set; }
        Int16 OrderQueuePermissionId { get; set; }
        String PermissionCode { get; set; }
        DateTime SelectedFirstStartDate { get; set; }
        Int16 SelectedDurationTypeID { get; set; }
        Int32 SelectedDuration { get; set; }
        Int32 SelectedInstanceInterval { get; set; }
        Int32? SelectedSuccessorNodeID { get; set; }
        Boolean SelectedEnableMobility { get; set; }
        List<MobilityPackageRelation> listMobilityPackageRelation { get; set; }

        //Used While Editing Node
        Int32? ArchivalGracePeriodMapped { get; set; }
        //Int32 EffectiveArchivalGracePeriodMapped { get; set; }

        //Used While Adding Node
        Int32? ArchivalGracePeriod { get; set; }
        Int32 EffectiveArchivalGracePeriod { get; set; }

        Int32 NeedEffectiveArchival { get; set; }

        /// <summary>
        /// List of the selected PaymentOptions at the Package Level
        /// </summary>
        List<Int32> lstSelectedOptions { get; set; }

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
        String BeforeExpirationFrequencyAddMode
        {
            get;
            set;
        }
        String BeforeExpirationFrequencyEditMode
        {
            get;
            set;
        }

        Int32? AfterExpirationFrequencyAddMode
        {
            get;
            set;
        }
        Int32? AfterExpirationFrequencyEditMode
        {
            get;
            set;
        }
        Int32? SubscriptionAfterExpiryEditMode
        {
            get;
            set;
        }
        Int32? SubscriptionAfterExpiryAddMode
        {
            get;
            set;
        }
        Int32? SubscriptionBeforeExpiryEditMode
        {
            get;
            set;
        }
        Int32? SubscriptionBeforeExpiryAddMode
        {
            get;
            set;
        }
        Int32? SubscriptionExpiryFrequencyEditMode
        {
            get;
            set;
        }
        Int32? SubscriptionExpiryFrequencyAddMode
        {
            get;
            set;
        }




        #region UAT-1794
        String IsAdminDataEntryAllow
        {
            get;
            set;
        }
        #endregion
        //UAT-2073
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
        Boolean IsPackagesNotAvailableForOrder
        {
            get;
            set;
        }

        Int16? PackagePermissionID { get; set; } //UAT - 2834       
        List<lkpPermission> UserPacakgePermissionList { get; set; } //UAT - 2834
        Boolean IsPackageBundleAvailableForOrder { get; set; }

        #region UAT-3873
        //Int32 BkgPackageHierarchyMappingID { get; set; }
        List<INTSOF.UI.Contract.SystemSetUp.NodePackagesDetails> lstNodePackagesDetails { get; set; }

        #endregion

        List<lkpFileExtension> ListFileExtension { set; }
        List<Int32> SelectedFileExtensions { get; set; }
        List<Int32> SelectedMappedFileExtensions { get; set; }

        #region UAT-3683
        String OptionalCategorySetting
        {
            get;
            set;
        }
        #endregion
    }
}




