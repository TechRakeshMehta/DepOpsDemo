#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  IManageUsersView.cs
// Purpose:   
//

#endregion

#region Namespace

#region System Defined

using System;
using System.Linq;
using System.Collections.Generic;

#endregion

#region Application Specific

using Entity;
using INTSOF.UI.Contract.IntsofSecurityModel;
using INTSOF.Utils;
using INTSOF.UI.Contract.SysXSecurityModel;

#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel.Views
{
    /// <summary>
    /// This interface handles the declaration of variables, properties, methods , events for managing user details.
    /// </summary>
    public interface IManageUsersView
    {
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

        #region Client Profile

        Boolean IsClientProfile
        {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        String ErrorMessage
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the reset user name.
        /// </summary>
        String ResetUserName
        {
            get;
            set;
        }

        /// <summary>
        /// Sets the list of all mapped organization user.
        /// </summary>
        List<OrganizationUserContract> MappedOrganizationUsers
        {
            set;
        }

        /// <summary>
        /// Gets or sets the list of all organizations.
        /// </summary>
        List<Organization> AllOrganization
        {
            set;
            get;
        }

        /// <summary>
        /// Gets the value of current user's id.
        /// </summary>
        Int32 CurrentUserId
        {
            get;
        }

        /// <summary>
        /// Gets the value for is the user admin or not?
        /// </summary>
        Boolean IsAdmin
        {
            get;
        }

        /// <summary>
        /// Gets the value of product's id.
        /// </summary>
        Int32? ProductId
        {
            get;
        }

        /// <summary>
        /// Gets or sets the list of all created by organization users.
        /// </summary>
        List<OrganizationUserContract> CreatedByOrganizationUsers
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the view contract.
        /// </summary>
        /// <remarks></remarks>
        ManageUsersContract ViewContract
        {
            get;
        }

        /// <summary>
        /// SuccessMessage</summary>
        /// <value>
        /// Gets or sets the value for Success message.</value>
        String SuccessMessage
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the list of all username prefix.
        /// </summary>
        List<OrganizationUserNamePrefix> AllUserNamePrefix
        {
            set;
            get;
        }

        /// <summary>
        /// get object of shared class of custom paging
        /// </summary>
        CustomPagingArgsContract ManageUserCustomPaging
        {
            get;
        }

        /// <summary>
        /// CurrentPageIndex
        /// </summary>
        /// <value> Gets or sets the value for CurrentPageIndex.</value>
        Int32 CurrentPageIndex
        {
            get;
            set;
        }

        /// <summary>
        /// PageSize
        /// </summary>
        /// <value> Gets the value for PageSize.</value>
        Int32 PageSize
        {
            get;
        }

        /// <summary>
        /// VirtualPageCount
        /// </summary>
        /// <value> Sets the value for VirtualPageCount.</value>
        Int32 VirtualPageCount
        {
            set;
        }

        #endregion

        #region Events

        event EventHandler<EventArgs> MangeRolesClick;
        event EventHandler<EventArgs> MapInstitutionClick;

        #endregion

        #region Methods

        #endregion

        List<OrganizationUser> lstClientAdminUsers { get; set; }

        Int32 CopyFromClientAdminOrgID { get; set; } 
        
        Guid CopyFromClientAdminUserID { get; set; } 

        Int32 SelectedTenantId { get; set; }

        String SelectedLinkingProfileOrgUsername { get; set; }
        Entity.OrganizationUser ExistingOrganisationUser { get; set; }
    }
}