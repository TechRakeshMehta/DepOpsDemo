#region Namespaces

#region System Defined

using System;
using System.Text;
using System.Collections.Generic;
using INTSOF.SharedObjects;
using System.Linq;

#endregion

#region Application Specific

using Entity.ClientEntity;
using Business.RepoManagers;
using INTSOF.Utils;
using INTSOF.Utils.Consts;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.UI.Contract.ComplianceRuleEngine;
using INTSOF.UI.Contract.ComplianceOperation;

#endregion

#endregion

namespace CoreWeb.ComplianceOperations.Views
{
    public class VerificationDetailsPresenter : Presenter<IVerificationDetailsView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IComplianceOperationsController _controller;
        // public VerificationDetailsPresenter([CreateNew] IComplianceOperationsController controller)
        // {
        // 		_controller = controller;
        // }

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        /// <summary>
        /// gets all the applicant document.
        /// </summary>
        public void GetApplicantDocuments()
        {
            //if (View.OrganizationUserData != null)
            if (!View.CurrentApplicantId_Global.IsNullOrEmpty() && View.CurrentApplicantId_Global > AppConsts.NONE)
            {
                // View.lstApplicantDocument = ComplianceDataManager.GetApplicantDocumentsData(View.CurrentApplicantId_Global, View.TenantId_Global);
                List<ApplicantDocuments> applicantDocuments = ComplianceDataManager.GetApplicantDocumentsData(View.CurrentApplicantId_Global, View.TenantId_Global);
                if (applicantDocuments.IsNotNull())
                {
                    View.lstApplicantDocument = ComplianceDataManager.GetDocumentRelatedToUserExceptEsigned(applicantDocuments, View.TenantId_Global);
                }
            }
        }

        public Int32 GetApplicantIdForSubscription()
        {
            return ComplianceDataManager.GetApplicantIdForSubscription(View.SelectedPackageSubscriptionID_Global, View.TenantId_Global);
        }


        /// <summary>
        /// Gets the tanent id for the current logged-in user.
        /// </summary>
        /// <returns></returns>
        public Entity.Tenant GetTenant()
        {
            Entity.OrganizationUser orgUser = SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId);
            View.LoggedInUserName_Global = orgUser.FirstName + " " + orgUser.LastName;
            View.LoggedInUserInitials_Global = orgUser.FirstName.Substring(0, 1) + (orgUser.MiddleName.IsNullOrEmpty() ? String.Empty : orgUser.MiddleName.Substring(0, 1)) + orgUser.LastName.Substring(0, 1);
            View.CurrentTenantId_Global = orgUser.Organization.Tenant.TenantID;
            View.OrganizationData = orgUser;
            return orgUser.Organization.Tenant;
        }

        /// <summary>
        /// Method to set the verification permission on selected node for the current logged in user.
        /// </summary>
        public void SetUserVerificationPermission()
        {
            View.IsFullPermissionForVerification = true;
            if (View.TenantId_Global > 0 && View.SelectedPackageSubscriptionID_Global > 0)
            {
                PackageSubscription packageSubscription = ComplianceDataManager.GetPackageSubscriptionByID(View.TenantId_Global, View.SelectedPackageSubscriptionID_Global);
                if (packageSubscription.IsNotNull() && packageSubscription.Order.IsNotNull())
                {
                    List<UserNodePermissionsContract> lstUserNodePermission = ComplianceSetupManager.GetUserNodePermissionForVerificationAndProfile(View.TenantId_Global, View.CurrentLoggedInUserId).ToList();
                    UserNodePermissionsContract userNodePermission = lstUserNodePermission.FirstOrDefault(cond => cond.DPM_ID == packageSubscription.Order.SelectedNodeID);
                    if (userNodePermission.IsNotNull() && userNodePermission.VerificationPermissionCode == LkpPermission.ReadOnly.GetStringValue())
                    {
                        View.IsFullPermissionForVerification = false;
                    }
                }
            }
        }
    }
}




