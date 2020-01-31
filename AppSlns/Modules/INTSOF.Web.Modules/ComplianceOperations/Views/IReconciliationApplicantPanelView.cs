using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IReconciliationApplicantPanelView
    {
        Entity.Tenant Tenant { get; }
        Entity.Tenant LoggedInUser { get; }
        Int32 CurrentLoggedInUserId { get; }
        Entity.OrganizationUser OrganizationUserData { get; set; }
        IReconciliationApplicantPanelView CurrentViewContext { get; }
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

        /// <summary>
        ///get and  set Category id 
        /// </summary>
        Int32 SelectedComplianceCategoryId_Global { get; set; }
        Int32 SelectedOrderId { get; set; }
        Boolean IsRushOrder { get; set; }
        void SetPageDataAndLayout(Boolean GetFreshData);
        System.Delegate ReLoadDataItemPanel { set; }
        String packageName { get; set; }
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
        DateTime? OrderApprovalDate { get; set; }

        DateTime? SubscriptionExpirationDate { get; set; }

        List<DataReconciliationQueueContract> lstNextPrevReconiciliationItem { get; set; }

        Int32 CurrentCompItemRecDataID { get; set; }
        String SelectedInstitutionIds { get; set; }
    }
}
