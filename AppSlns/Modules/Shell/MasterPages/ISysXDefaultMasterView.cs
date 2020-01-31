#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ISysXDefaultMasterView.cs
// Purpose:
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Collections.Generic;

#endregion

#region Application Specific

using Entity;
using System.Web.UI;

#endregion

#endregion

namespace CoreWeb.Shell.MasterPages
{
    /// <summary>
    /// This interface has the declaration for Sysx X Master page.
    /// </summary>
    /// <remarks></remarks>
    public interface ISysXDefaultMasterView
    {
        #region Variables

        #endregion

        #region Properties

        /// <summary>
        /// Gets the current user ID.
        /// </summary>
        /// <remarks></remarks>
        String CurrentUserId
        {
            get;
        }

        /// <summary>
        /// Sets the assigned blocks.
        /// </summary>
        /// <value>The assigned blocks.</value>
        /// <remarks></remarks>
        List<vw_UserAssignedBlocks> AssignedBlocks
        {
            set;
        }

        #endregion

        #region Events

        #endregion

        #region Methods

        /// <summary>
        /// Use this method to add a dock in one of the defined dockzones in the master page
        /// </summary>
        /// <param name="dock">SysXDock to be added in the specified dockzone</param>
        /// <param name="zone">Left or Right dockzone</param>
        /// <remarks></remarks>
        void AddDock(object dock, MPDockZones zone = MPDockZones.LeftZone);

        /// <summary>
        /// Shows the error message.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <remarks></remarks>
        void ShowErrorMessage(String errorMessage);

        /// <summary>
        /// Shows the info message.
        /// </summary>
        /// <param name="infoMessage">The info message.</param>
        /// <remarks></remarks>
        void ShowInfoMessage(String infoMessage);

        /// <summary>
        /// Redirects to login page.
        /// </summary>
        /// <remarks></remarks>
        void RedirectToLoginPage();

        /// <summary>
        /// Hides the error message.
        /// </summary>
        /// <remarks></remarks>
        void HideErrorMessage();

        /// <summary>
        /// Refreshes the menu items.
        /// </summary>
        /// <remarks></remarks>
        void RefreshMenuItems();

        /// <summary>
        /// Register postback controls inside an UpdatePanel control as triggers. 
        /// Controls that are registered by using this method update a whole page instead of updating only the UpdatePanel control's content
        /// </summary>
        /// <param name="registerControl"></param>
        void RegisterControlForPostBack(Control registerControl);

        #endregion
    }
}