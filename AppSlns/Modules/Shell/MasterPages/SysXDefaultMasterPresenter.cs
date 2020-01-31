#region Header Comment Master

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  SysXDefaultMasterPresenter.cs
// Purpose:
// 

#endregion

#region Namespaces

#region System Defined

using System;
using System.Linq;
using INTSOF.SharedObjects;
using System.Web.Security;

#endregion

#region Application Specific

using Entity;
using INTSOF.Utils;
using Business.RepoManagers;


#endregion

#endregion

namespace CoreWeb.Shell.MasterPages
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public class SysXDefaultMasterPresenter : Presenter<ISysXDefaultMasterView>
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        #endregion

        #region Private Properties

        #endregion

        #endregion

        #region Events

        #endregion

        #region Methods

        #region Public Methods

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
            View.AssignedBlocks = SecurityManager.GetLineOfBusinessesByUser(View.CurrentUserId).ToList();
        }

        /// <summary>
        /// Does the log off.
        /// </summary>
        /// <remarks></remarks>
        public void DoLogOff()
        {
            SysXWebSiteUtils.SessionService.ClearSession(true);
            FormsAuthentication.SignOut();
            SysXAppDBEntities.ClearContext();
            View.RedirectToLoginPage();
        }

        /// <summary>
        /// Checks the user status.
        /// </summary>
        /// <param name="organizationUserId">The organization user id.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public Boolean CheckUserStatus(Int32 organizationUserId)
        {
            Boolean status;
            OrganizationUser organizationUser = SecurityManager.GetOrganizationUser(organizationUserId);
            Boolean aspnetUsersInRoles = SecurityManager.IsCurrentUserRoleExists(organizationUser.UserID);

            if (!organizationUser.IsNull())
            {
                if (organizationUser.aspnet_Users.aspnet_Membership.IsLockedOut.Equals(false) && organizationUser.IsActive && !aspnetUsersInRoles)
                {
                    status = organizationUser.IsActive;
                }
                else
                {
                    status = false;
                }
            }
            else
            {
                status = false;
            }
            return status;
        }

        #endregion

        #region Private Methods

        #endregion

        #endregion
    }
}