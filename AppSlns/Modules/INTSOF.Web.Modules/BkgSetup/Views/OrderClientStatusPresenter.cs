using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.BkgSetup;

namespace CoreWeb.BkgSetup.Views
{
    public class OrderClientStatusPresenter : Presenter<IOrderClientStatusView>
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

        #region PUBLIC METHODS

        /// <summary>
        /// Gets the tanent id for the current logged-in user.
        /// </summary>
        /// <returns></returns>
        public Int32 GetTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
        }

        /// <summary>
        /// Method for retrieving list of all the tenants.
        /// </summary>
        public void GetTenants()
        {
            View.TenantsList = BackgroundSetupManager.getClientTenant();
        }

        /// <summary>
        /// Checked if logged user is admin or not.
        /// </summary>
        /// <returns>True if logged in user is admin.</returns>
        public void IsAdminLoggedIn()
        {
            //Checked if logged user is admin or not.
            if (View.TenantId == Business.RepoManagers.SecurityManager.DefaultTenantID)
            {
                View.IsAdminLoggedIn = true;
            }
            else
            {
                View.IsAdminLoggedIn = false;
            }
        }

        public void SaveOrderClientStatus()
        {
            View.IsOrderClientStatusSaved = BackgroundSetupManager.SaveOrderClientStatus(View.SelectedTenantId, View.OrderClientStatusTypeName, View.CurrentLoggedInUserId);
        }

        #endregion


        public void FetchOrderClientStatus()
        {
            View.OrderClientStatusList = BackgroundSetupManager.FetchOrderClientStatus(View.SelectedTenantId);
        }

        public Boolean UpdateClientStatusSequence(IList<Entity.ClientEntity.BkgOrderClientStatu> statusToMove, Int32? destinationIndex)
        {
            return BackgroundSetupManager.UpdateClientStatusSequence(View.SelectedTenantId, statusToMove, destinationIndex, View.CurrentLoggedInUserId);
        }



        public String DeleteOrderClientStatus()
        {
            String errorMessage;
            if (BackgroundSetupManager.CheckIfOrderClientStatusIsUsed(View.SelectedTenantId, View.OrderClientStatusId))
            {
                errorMessage = "You can not delete this Order Client Status as it is currently in use. ";    
            }
            else
            {
                if (BackgroundSetupManager.DeleteOrderClientStatus(View.SelectedTenantId, View.OrderClientStatusId, View.CurrentLoggedInUserId))
                {
                    errorMessage = String.Empty;
                }
                else
                {
                    errorMessage = "Order Client Status deletion failed.";
                }
            }
            return errorMessage;
        }

        public Boolean UpdateOrderClientStatus()
        {
            return BackgroundSetupManager.UpdateOrderClientStatus(View.SelectedTenantId, View.OrderClientStatusId, View.OrderClientStatusTypeName, View.CurrentLoggedInUserId);
        }
    }
}
