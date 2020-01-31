#region Namespaces

#region System Defined Namespaces

using INTSOF.SharedObjects;
using System.Linq;


#endregion

#region User Defined Namespaces

using Business.RepoManagers;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;


#endregion

#endregion

namespace CoreWeb.Search.Views
{
    public class ApplicantRequirementRotationsPresenter : Presenter<IApplicantRequirementRotationsView>
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        /// <summary>
        /// To check if Tenant is Admin/DefaultTenant
        /// </summary>
        public bool IsAdminLoggedIn()
        {
            return SecurityManager.DefaultTenantID == View.LoggedInUserTenantId;
        }

        /// <summary>
        /// Gets Clinical Rotations list
        /// </summary>
        public void GetClinicalRotations()
        {
            if (IsAdminLoggedIn())
                View.lstApplicantRotations = ApplicantClinicalRotationManager.GetApplicantClinicalRotationDetails(View.TenantId, View.OrganizationUserId, null);
            else
                View.lstApplicantRotations = ApplicantClinicalRotationManager.GetApplicantClinicalRotationDetails(View.TenantId, View.OrganizationUserId, View.CurrentLoggedInUserId);
        }

    }
}




