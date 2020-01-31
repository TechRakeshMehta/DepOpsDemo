#region Namespaces

#region SystemDefined

using INTSOF.SharedObjects;


#endregion

#region UserDefined

using Business.RepoManagers;


#endregion

#endregion

namespace CoreWeb.RotationPackages.Views
{
    public class AddAnotherFieldPopupPresenter : Presenter<IAddAnotherFieldPopupView>
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
            View.DefaultTenantId = SecurityManager.DefaultTenantID;
        }

        public void GetInstitutionConfigurationDetails()
        {

        }
    }
}



