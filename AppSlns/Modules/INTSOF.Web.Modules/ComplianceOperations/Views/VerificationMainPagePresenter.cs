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

#endregion

#endregion
namespace CoreWeb.ComplianceOperations.Views
{
    public class VerificationMainPagePresenter : Presenter<IVerificationMainPageView>
    {
        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IComplianceOperationsController _controller;
        // public CategoryDataEntryPresenter([CreateNew] IComplianceOperationsController controller)
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
            //  View.ApplicantCategoryData = ApplicantManager.GetApplicantComplianceData(View.PackageSubscriptionId, View.ClientComplianceItemsContract.ClientComplianceCategoryId, View.TenantId);
        }

        /// <summary>
        /// gets all the applicant document.
        /// </summary>
        public void GetApplicantDocuments()
        {
            //if (View.OrganizationUserData != null)
            if (!View.CurrentApplicantId_Global.IsNullOrEmpty() && View.CurrentApplicantId_Global > AppConsts.NONE)
                View.lstApplicantDocument = ComplianceDataManager.GetApplicantDocumentsData(View.CurrentApplicantId_Global, View.TenantId_Global);
        }


        /// <summary>
        /// Gets the tanent id for the current logged-in user.
        /// </summary>
        /// <returns></returns>
        public Entity.Tenant GetTenant()
        {
            Entity.OrganizationUser orgUser = SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId);
            View.LoggedInUserName_Global = orgUser.FirstName + " " + orgUser.LastName;
            View.CurrentTenantId_Global = orgUser.Organization.Tenant.TenantID;
            View.OrganizationData = orgUser;
            return orgUser.Organization.Tenant;
        }
    }
}
