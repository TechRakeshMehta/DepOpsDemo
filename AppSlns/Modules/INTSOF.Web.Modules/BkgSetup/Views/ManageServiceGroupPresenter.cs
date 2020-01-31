using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.SharedObjects;
using INTSOF.Utils;

namespace CoreWeb.BkgSetup.Views
{
    public class ManageServiceGroupPresenter : Presenter<IManageServiceGroupView>
    {
        public override void OnViewInitialized()
        {
            View.DefaultTenantId = SecurityManager.DefaultTenantID;
        }

        public void GetTenants()
        {
            View.ListTenants = ComplianceDataManager.getClientTenant();
        }

        /// <summary>
        /// Gets all the service groups for the given tanent ID.
        /// </summary>
        public void GetServiceGroups()
        {
            View.ServiceGroups = BackgroundSetupManager.GetServiceGroups(View.SelectedTenantId);
        }

        /// <summary>
        /// Saves the service groups in the MasterDB followed by ClientDB(in case of client).
        /// </summary>
        public void SaveServiceGroup()
        {
            if (BackgroundSetupManager.CheckIfServiceGroupNameAlreadyExist(View.ViewContract.ServiceGroupName, View.ViewContract.ServiceGroupID, View.SelectedTenantId))
            {
                View.ErrorMessage = "Service Group Name can not be duplicate.";
            }
            else
            {
                BkgSvcGroup newServiceGroup = new BkgSvcGroup
                {
                    BSG_Name = View.ViewContract.ServiceGroupName,
                    BSG_Description = View.ViewContract.ServiceGroupDesc,
                    BSG_IsEditable = true,
                    BSG_IsSystemPreConfigured = false,
                    BSG_Active = View.ViewContract.Active,
                };
                BackgroundSetupManager.SaveServiceGroup(newServiceGroup, View.CurrentLoggedInUserId, View.SelectedTenantId);
            }
        }

        /// <summary>
        /// Updates the service group.
        /// </summary>
        public void UpdateServiceGroup()
        {
            if (BackgroundSetupManager.CheckIfServiceGroupNameAlreadyExist(View.ViewContract.ServiceGroupName, View.ViewContract.ServiceGroupID, View.SelectedTenantId))
            {
                View.ErrorMessage = "Service Group Name can not be duplicate.";
            }
            else
            {
                BkgSvcGroup svcGroup = new BkgSvcGroup
                {
                    BSG_Name = View.ViewContract.ServiceGroupName,
                    BSG_Description = View.ViewContract.ServiceGroupDesc,
                    BSG_Active = View.ViewContract.Active,
                };
                BackgroundSetupManager.UpdateServiceGroupDetail(svcGroup, View.ViewContract.ServiceGroupID, View.CurrentLoggedInUserId, View.SelectedTenantId);
            }
        }

        /// <summary>
        /// Deletes the service groups.
        /// </summary>
        public Boolean DeleteServiceGroup()
        {
            IntegrityCheckResponse response = BackgroundServiceIntegrityManager.IfServiceGroupCanBeDeleted(View.ViewContract.ServiceGroupID, View.SelectedTenantId);
            if (response.CheckStatus == CheckStatus.True)
            {
                BkgSvcGroup svcGrp = BackgroundSetupManager.getCurrentServiceGroupInfo(View.ViewContract.ServiceGroupID, View.SelectedTenantId);
                View.ErrorMessage = String.Format(response.UIMessage, svcGrp.BSG_Name);
                return false;
            }
            else
            {
                return BackgroundSetupManager.DeleteServiceGroup(View.ViewContract.ServiceGroupID, View.CurrentLoggedInUserId, View.SelectedTenantId);
            }
        }

        /// <summary>
        /// Gets the tanent id for the current logged-in user.
        /// </summary>
        /// <returns></returns>
        public Int32 GetTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
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

    }
}
