#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ManageRole.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

#region Application Specific

#endregion

#endregion

namespace INTSOF.UI.Contract.IntsofSecurityModel
{
    /// <summary>
    /// This contract gets or sets the properties for manage roles section.
    /// </summary>
    public class ManageRoleContract
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// TenantID
        /// </summary>
        /// <value>Gets or sets the value for tenant's id.</value>
        /// <remarks></remarks>
        public Int32 TenantId
        {
            get;
            set;
        }

        /// <summary>
        /// TenantName
        /// </summary>
        /// <value>Gets or sets the value for TenantName.</value>
        /// <remarks></remarks>
        public String TenantName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the value if the link is given on Manage Tenant.
        /// </summary>
        /// <value>yes if the control is getting load on navigating through Manage Tenant else no.</value>
        /// <remarks></remarks>
        public Boolean IsLinkOnTenant
        {
            get;
            set;
        }

        /// <summary>
        /// RoleDetailID
        /// </summary>
        /// <value>Gets or sets the value for role detail's id.</value>
        /// <remarks></remarks>
        public String RoleDetailId
        {
            get;
            set;
        }

        /// <summary>
        /// Name
        /// </summary>
        /// <value>Gets or sets the value for role's name.</value>
        /// <remarks></remarks>
        public String Name
        {
            get;
            set;
        }

        /// <summary>
        /// Description
        /// </summary>
        /// <value>Gets or sets the value for role's description.</value>
        /// <remarks></remarks>
        public String Description
        {
            get;
            set;
        }

        /// <summary>
        /// TenantProductID
        /// </summary>
        /// <value>Gets or sets the value for product's id.</value>
        /// <remarks></remarks>
        public Int32 TenantProductId
        {
            get;
            set;
        }

        /// <summary>
        /// LoginUserProductName
        /// </summary>
        /// <value>Gets or sets the value for product name for logged in user.</value>
        /// <remarks></remarks>
        public String LoginUserProductName
        {
            get;
            set;
        }

        /// <summary>
        /// CreatedOn
        /// </summary>
        /// <value>Gets or sets the value for created on date.</value>
        /// <remarks></remarks>
        public DateTime CreatedOn
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the role is User Group Level.
        /// </summary>
        /// <value><c>true</c> if IsUserGroupLevel; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
        public Boolean IsUserGroupLevel
        {
            get;
            set;
        }

        //Admin Entry Portal
        /// <summary>
        /// Gets or sets a value indicating whether the role is to show admin entry portal dashboard.
        /// </summary>
        public Boolean ShowAdminEntryPortal
        {
            get;
            set;
        }

        #endregion

        #region Events

        #endregion

        #region Methods

        #region Public Methods

        #endregion

        #region Private Methods

        #endregion

        #endregion
    }
}