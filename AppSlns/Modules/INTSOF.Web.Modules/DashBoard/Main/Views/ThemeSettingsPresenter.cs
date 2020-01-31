using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Entity;
using Business.RepoManagers;
using System.Linq;

namespace CoreWeb.Main.Views
{
    public class ThemeSettingsPresenter : Presenter<IThemeSettingsView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IMainController _controller;
        // public ThemeSettingsPresenter([CreateNew] IMainController controller)
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

        // TODO: Handle other view events and set state in the view

        /// Gets the Logged-in UserId.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public aspnet_Membership GetUserById(Guid userId)
        {
            return SecurityManager.GetAspnetMembershipById(userId);
        }

        /// <summary>
        /// Retrieves all the Line of Businesses based on current user's Id.
        /// </summary>
        /// <param name="currentUserId">value of current user's Id.</param>
        /// <returns></returns>
        public IQueryable<vw_UserAssignedBlocks> GetLineOfBusinessesByUser(String currentUserId)
        {
            return SecurityManager.GetLineOfBusinessesByUser(currentUserId);
        }

        /// <summary>
        /// Update the default line of business for current logged in user.
        /// </summary>
        /// <param name="organizationUserId">The organization user id.</param>
        public void UpdateOrganizationUser(Int32 organizationUserId)
        {
            OrganizationUser organizationUser = SecurityManager.GetOrganizationUser(organizationUserId);
            organizationUser.SysXBlockID = Convert.ToInt32(View.DefaultLineOfBusiness);
            SecurityManager.UpdateOrganizationUser(organizationUser);
        }

        /// <summary>
        /// Retrieves default Line of Businesses based on current user's Id.
        /// </summary>
        /// <param name="currentUserId">value of current user's Id.</param>
        /// <returns></returns>
        public String GetDefaultLineOfBusinessOfLoggedInUser(Int32 currentUserId)
        {
            return SecurityManager.GetOrganizationUser(currentUserId).SysXBlockID.GetValueOrDefault().ToString();
        }

        public Boolean CheckIfUserIsApplicant(Int32 currentLoggedInUserId)
        {
            Boolean? isapplicant = SecurityManager.GetOrganizationUser(currentLoggedInUserId).IsApplicant;
            if (isapplicant.HasValue)
                return isapplicant.Value;
            return false;
        }
    }
}




