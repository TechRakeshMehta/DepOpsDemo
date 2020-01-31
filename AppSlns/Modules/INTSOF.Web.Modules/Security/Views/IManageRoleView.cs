#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  IManageRoleView.cs
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

#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel.Views
{
    /// <summary>
    /// This interface handles the declaration of variables, properties, methods , events for managing role details.
    /// </summary>
    public interface IManageRoleView
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
        /// Gets the product's id of logged in user.
        /// </summary>
        Int32? LoginUserProductId
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
        /// Sets the list of all role details.
        /// </summary>
        List<RoleDetail> RoleDetails
        {
            set;
        }

        /// <summary>
        /// Gets or sets a list of all tenant's products.
        /// </summary>
        List<TenantProduct> TenantProducts
        {
            set;
            get;
        }

        /// <summary>
        /// Gets the value for is the logged in user admin or not?
        /// </summary>
        Boolean IsAdmin
        {
            get;
        }

        /// <summary>
        /// Gets or sets the tenant's role.
        /// </summary>
        Tenant TenantsRole
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the view contract.
        /// </summary>
        /// <remarks></remarks>
        ManageRoleContract ViewContract
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

        #endregion

        #region Events

        event EventHandler<EventArgs> MangeFeatureClick;

        #endregion

        #region Methods

        #endregion
    }
}