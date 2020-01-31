#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  IManageTenantView.cs
// Purpose:   
//

#endregion

#region Namespace

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
    /// This interface handles the declaration of variables, properties, methods , events for managing tenant information.
    /// </summary>
    public interface IManageTenantView
    {
        #region Variables

        #endregion

        #region Properties

        // Tenant related properties.

        /// <summary>
        /// Tenants</summary>
        /// <value>
        /// Sets the list of all tenants.</value>
        List<Tenant> Tenants
        {
            set;
        }

        /// <summary>
        /// Tenant Types</summary>
        /// <value>
        /// Sets the list of all tenant types.</value>
        List<lkpTenantType> TenantTypes
        {
            get;
            set;
        }

        // Product related properties.

        /// <summary>
        /// TenantProducts</summary>
        /// <value>
        /// Gets or sets the list of all tenant's products.</value>
        List<TenantProduct> TenantProducts
        {
            get;
            set;
        }

        // Organization related properties.

        /// <summary>
        /// Organizations</summary>
        /// <value>
        /// Gets or sets the value for list of all organizations.</value>
        List<Organization> Organizations
        {
            get;
            set;
        }

        // Commonly used properties.

        /// <summary>
        /// States</summary>
        /// <value>
        /// Gets or sets the list of all states.</value>
        List<State> States
        {
            get;
            set;
        }

        /// <summary>
        /// Cities</summary>
        /// <value>
        /// Gets or sets the list of all cities.</value>
        List<City> Cities
        {
            get;
            set;
        }

        /// <summary>
        /// ErrorMessage</summary>
        /// <value>
        /// Gets or sets the value for ErrorMessage.</value>
        String ErrorMessage
        {
            get;
            set;
        }

        /// <summary>
        /// CurrentUserID</summary>
        /// <value>
        /// Gets or sets the value for current user's id.</value>
        Int32 CurrentUserId
        {
            get;
        }

        /// <summary>
        /// Gets the view contract.
        /// </summary>
        /// <remarks></remarks>
        ManageTenantContract ViewContract
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
        /// Will Hold organization user prefix.
        /// </summary>
        List<OrganizationUserNamePrefix> OrganizationPrefixes
        {
            get;
            set;
        }

        #endregion

        #region Events

        #endregion

        #region Methods

        #endregion
    }
}