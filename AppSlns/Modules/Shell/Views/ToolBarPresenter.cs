#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:   ToolBarPresenter.cs
// Purpose:
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Linq;
using INTSOF.SharedObjects;

#endregion

#region Application Specific

using Business.RepoManagers;
using Entity;


#endregion

#endregion

namespace CoreWeb.Shell.Views
{
    public class ToolBarPresenter : Presenter<IToolBarView>
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

        #endregion

        #region Events

        #endregion

        #region Methods

        #region Public Methods

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        /// <summary>
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
        /// Retrieves default Line of Businesses based on current user's Id.
        /// </summary>
        /// <param name="currentUserId">value of current user's Id.</param>
        /// <returns></returns>
        public String GetDefaultLineOfBusinessOfLoggedInUser(Int32 currentUserId)
        {
            return SecurityManager.GetOrganizationUser(currentUserId).SysXBlockID.GetValueOrDefault().ToString();
        }

        /// <summary>
        /// Get SysxConfig table values by sysxkey
        /// </summary>
        /// <param name="SysXKey">value of sysxkey Id.</param>
        /// <returns>String key's value</returns>
        public String GetSysXConfigValue(String SysXKey)
        {
             SysXConfig lstsysxConfig = LookupManager.GetLookUpData<SysXConfig>().Where(fx => fx.SysXKey.Equals("HelpFileGetPath")).FirstOrDefault();
             return lstsysxConfig.Value; 
        }
       

        #endregion

        #region Private Methods

        #endregion

        #endregion
    }
}




