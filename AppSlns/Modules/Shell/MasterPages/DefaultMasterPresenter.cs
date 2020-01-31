#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  DefaultMasterPresenter.cs
// Purpose:
//

#endregion

#region Namespaces

#region System Defined

using System;
using INTSOF.SharedObjects;
using System.Web.Security;
#endregion

#region Application Specific

using Entity;
using Business.RepoManagers;
using INTSOF.Utils;


#endregion

#endregion

namespace CoreWeb.Shell.MasterPages
{
    /// <summary>
    /// This class handles the operations related to Master page.
    /// </summary>
    /// <remarks></remarks>
    public class DefaultMasterPresenter : Presenter<IDefaultMasterView>
    {
        /// <summary>
        /// Called when [view loaded].
        /// </summary>
        /// <remarks></remarks>
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        /// <summary>
        /// Called when [view initialized].
        /// </summary>
        /// <remarks></remarks>
        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        public aspnet_Membership GetUserById(Guid userId)
        {
            return SecurityManager.GetAspnetMembershipById(userId);
        }

        /// <summary>
        /// Does the log off.
        /// </summary>
        /// <remarks></remarks>
        public void DoLogOff(bool isLoggedIn, Int32 currentLogedInUserId, Int32 userLoginHistoryID)
        {
            //IssessionTimeout will be false as user is switching institution.
            UpdateUserLoginActivity(false, currentLogedInUserId, userLoginHistoryID);
            if (isLoggedIn && !this.View.CurrentSessionId.IsNullOrEmpty())
            {
                View.ViewStateProvider.Delete(this.View.CurrentSessionId);
            }
            SysXWebSiteUtils.SessionService.ClearSession(true);
            FormsAuthentication.SignOut();
            SysXAppDBEntities.ClearContext();
        }

        /// <summary>
        /// Checks the user status.
        /// </summary>
        /// <param name="organizationUserId">The organization user id.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public Boolean CheckUserStatus(Guid userId)
        {
            Boolean status = SecurityManager.IsCurrentLoggedInUserRoleExists(userId); ;
            return status;
        }

        /// <summary>
        /// Update user logout time in User Login Activity
        /// </summary>
        /// <param name="organizationUserId"></param>
        /// <param name="currentSessionId"></param>
        /// <param name="isSessionTimeout"></param>
        public void UpdateUserLoginActivity(Boolean isSessionTimeout, Int32 currentLogedInUserId, Int32 userLoginHistoryID)
        {
            SecurityManager.UpdateUserLoginActivity(currentLogedInUserId, View.CurrentSessionId, isSessionTimeout, userLoginHistoryID);
        }
    }
}