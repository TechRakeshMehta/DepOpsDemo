using System;
using System.Collections.Generic;
using System.Text;
using INTSOF.UI.Contract.ComplianceManagement;
using Entity.ClientEntity;
using INTSOF.Utils;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.ServiceDataContracts.Modules.Common;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IVerificationApplicantPanelView
    {

        Entity.Tenant Tenant { get; }
        Entity.Tenant LoggedInUser { get; }
        Int32 CurrentLoggedInUserId { get; }
        List<INTSOF.Utils.CommonPocoClasses.ComplianceCategoryPocoClass> ApplicantComplianceCategoryDataList { get; set; }
        //List<Int32?> PackageSubscriptionIdList { get; set; }
        List<PkgSubscriptionIDList> PackageSubscriptionIdList { get; set; }
        Entity.OrganizationUser OrganizationUserData { get; set; }
        IVerificationApplicantPanelView CurrentViewContext { get; }

        WorkQueueType WorkQueue { get; set; }

        String UIInputException { get; set; }

        Int32 TenantId_Global { get; set; }
        /// <summary>
        /// set package id for return back to queue.
        /// </summary>
        Int32 PackageId { get; set; }

        /// <summary>
        /// set Category id for return back to queue.
        /// </summary>
        Int32 CategoryId { get; set; }

        /// <summary>
        /// set user group id for return back to queue.
        /// </summary>
        Int32 UserGroupId { get; set; }

        //String Notes { get; set; }

        Int32 ItemDataId_Global { get; set; }


        CustomPagingArgsContract VerificationGridCustomPaging { get; }

        /// <summary>
        ///get and  set package id .
        /// </summary>
        Int32 CurrentCompliancePackageId_Global { get; set; }

        String CurrentCompliancePackageStatus { get; set; }
        String CurrentCompliancePackageStatusCode { get; set; }

        String CurrentPackageBredCrum { get; set; }

        /// <summary>
        ///get and  set Applicant Id 
        /// </summary>
        Int32 CurrentApplicantId_Global { get; set; }

        /// <summary>
        ///get and  set package id .
        /// </summary>
        Int32 SelectedPackageSubscriptionID_Global { get; set; }
        Int32 PrevPackageSubscriptionID { get; set; }
        Int32 NextPackageSubscriptionID { get; set; }
        Int32 PrevAppCmpItemID { get; set; }
        Int32 NextAppCmpItemID { get; set; }

        /// <summary>
        ///get and  set Category id 
        /// </summary>
        Int32 SelectedComplianceCategoryId_Global { get; set; }
        Int32 PrevComplianceCategoryId_Global { get; set; }
        Int32 NextComplianceCategoryId_Global { get; set; }


        Int32 ReviewerUserId { get; set; }
        Int32 SelectedOrderId { get; set; }
        String SelectedOrderNumber { get; set; }
        Boolean IncludeIncompleteItems { get; set; }
        Boolean ShowOnlyRushOrders { get; set; }
        Boolean IsRushOrder { get; set; }
        void SetPageDataAndLayout(Boolean GetFreshData);
        System.Delegate ReLoadDataItemPanel { set; }
        String viewType { get; set; }
        Int32 SubPageIndex { get; set; }
        Int32 SubTotalPages { get; set; }
        String packageName { get; set; }

        Boolean IsException { get; set; }
        Boolean IsEscalationRecords { get; set; }

        #region UAT-806 Creation of granular permissions for Client Admin users
        Boolean IsDOBDisable { get; set; }
        #endregion

        #region UAT-749:WB: Addition of "User Groups" to left panel of Verification Details screen
        List<Entity.ClientEntity.UserGroup> UserGroupDataList { get; set; }
        #endregion

        /// <summary>
        /// Data of the Applicant.
        /// </summary>
        OrganizationUserContract ApplicantData
        {
            get;
            set;
        }

        String SSNPermissionCode { get; set; }

        DateTime? OrderApprovalDate { get; set; }

        DateTime? SubscriptionExpirationDate { get; set; }

        //UAT-2460
        String SelectedArchiveStateCode { get; set; }
        Int32 TenantId { get; set; }

        String AllowedFileExtensions { get; set; }
    }
}




