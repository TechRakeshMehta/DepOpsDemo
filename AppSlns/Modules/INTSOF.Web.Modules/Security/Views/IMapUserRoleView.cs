#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  IMapUserRoleView.cs
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

using INTSOF.UI.Contract.IntsofSecurityModel;

#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel.Views
{
    /// <summary>
    /// This interface handles the declaration of variables, properties, methods , events for mapping between user and its roles.
    /// </summary>
    public interface IMapUserRoleView
    {
        #region Variables

        #endregion

        #region Properties

        #region ClientOnBoardingWizard

        Boolean IsDataLoad
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is client on boarding wizard.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is client on boarding wizard; otherwise, <c>false</c>.
        /// </value>
        Boolean IsClientOnBoardingWizard
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the value of Validation Group.
        /// </summary>
        String ValidationGroup
        {
            get;
        }

        #endregion

        /// <summary>
        /// Gets or sets the list of all roles.
        /// </summary>
        List<aspnet_Roles> AllRoles
        {
            set;
            get;
        }

        /// <summary>
        /// Gets or sets the list of all roles, created under tenant's product.
        /// </summary>
        List<aspnet_Roles> AllRolesOfProduct
        {
            set;
            get;
        }


        /// <summary>
        /// Gets or sets the list of all current user's role.
        /// </summary>
        List<aspnet_Roles> CurrentUserRoles
        {
            set;
            get;
        }

        /// <summary>
        /// Gets or sets the selected user.
        /// </summary>
        OrganizationUser SelectedUser
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the view contract.
        /// </summary>
        /// <remarks></remarks>
        MapUserRoleContract ViewContract
        {
            get;
        }

        /// <summary>
        /// Gets or Sets the value for all assigned roles to a selected user.
        /// </summary>
        /// <value>
        /// The Role Name.
        /// </value>
        String[] AllAssignedRoleName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the value for is the logged in user admin or not?
        /// </summary>
        Boolean IsAdmin
        {
            get;
        }

        /// <summary>
        /// Gets the current user's id.
        /// </summary>
        Int32 CurrentUserId
        {
            get;
        }

        /// <summary>
        /// Gets or Sets the value for selected roles.
        /// </summary>
        Dictionary<Guid, KeyValuePair<String, Boolean>> SelectedItems
        {
            get; 
            set;
        }

        #endregion

        #region Events

        event EventHandler<EventArgs> SaveMappingClick;

        #endregion

        #region Methods

        /// <summary>
        // This method handles the redirection to manage user page.
        /// </summary>
        void RedirectToManageUser();

        #endregion
    }
}