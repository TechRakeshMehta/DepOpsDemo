#region Namespaces

#region SystemDefined

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using System.Linq;
using System.Data.Entity.Core.Objects;

#endregion

#region UserDefined

using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.Utils;

#endregion

#endregion

namespace CoreWeb.ComplianceAdministration.Views
{
    public class ManageSubscriptionOptionPresenter : Presenter<IManageSubscriptionOptionView>
    {
        #region Variables

        #region Private Variables
        #endregion

        #region Public Variables
        #endregion

        #endregion

        #region Properties

        #region Private Properties
        #endregion

        #region Public Properties
        #endregion

        #endregion

        #region Methods

        #region Public Methods

        public override void OnViewLoaded()
        {
            View.ListTenants = ComplianceDataManager.getClientTenant();
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        public void GetSubscriptionOptionsList()
        {
            View.ListSubscriptionOptions = ComplianceSetupManager.GetSubscriptionOptionsList(View.SelectedTenantID);
        }

        public Boolean IsUniqueSubscriptionOptionLabel(SubscriptionOption newSubscriptionOption, Int32? subscriptionOptionID = null)
        {
            View.ListSubscriptionOptions = ComplianceSetupManager.GetSubscriptionOptionsList(View.SelectedTenantID);

            if (View.ListSubscriptionOptions.Any(x => x.Label == newSubscriptionOption.Label && x.SubscriptionOptionID != subscriptionOptionID))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public Boolean SaveSubscriptionOption(SubscriptionOption newSubscriptionOption, Int32? subscriptionOptionID = null)
        {
            if (subscriptionOptionID.IsNull())
            {
                newSubscriptionOption.Code = Guid.NewGuid();
                newSubscriptionOption.CreatedByID = View.CurrentUserId;
                newSubscriptionOption.CreatedOn = DateTime.Now;
            }
            else
            {
                newSubscriptionOption.ModifiedByID = View.CurrentUserId;
                newSubscriptionOption.ModifiedOn = DateTime.Now;
            }
            ComplianceSetupManager.SaveSubscriptionOption(View.SelectedTenantID, newSubscriptionOption, subscriptionOptionID);
            return true;
        }

        public Boolean IsSubscriptionOptionUsedByPackage()
        {
            var deptProgramPackageSubscription = View.ListSubscriptionOptions.FirstOrDefault(x => x.SubscriptionOptionID == View.SubscriptionOptionID).
                DeptProgramPackageSubscriptions;

            if (deptProgramPackageSubscription.IsNotNull() && deptProgramPackageSubscription.Any(x => x.DPPS_IsDeleted == false))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean DeleteSubscriptionOption()
        {
            SubscriptionOption subscriptionOption = new SubscriptionOption();
            subscriptionOption.SubscriptionOptionID = View.SubscriptionOptionID;
            subscriptionOption.ModifiedByID = View.CurrentUserId;
            ComplianceSetupManager.DeletSubscriptionOption(View.SelectedTenantID, subscriptionOption);
            return true;
        }

        /// <summary>
        /// Checked if logged user is admin or not.
        /// </summary>
        /// <returns>True if logged in user is admin.</returns>
        public Boolean IsAdminLoggedIn()
        {
            //View.CurrentUserTenantId = GetTenantId();
            //Checked if logged user is admin or not.
            if (View.CurrentUserTenantId == Business.RepoManagers.SecurityManager.DefaultTenantID)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the tanent id for the current logged-in user.
        /// </summary>
        /// <returns></returns>
        public Int32 GetTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentUserId).Organization.TenantID.Value;
        }

        #endregion

        #region Private Methods

        

        #endregion

        #endregion
    }
}




