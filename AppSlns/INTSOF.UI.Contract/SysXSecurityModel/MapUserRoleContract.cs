#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  MapUserRole.cs
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

#endregion

#endregion

namespace INTSOF.UI.Contract.IntsofSecurityModel
{
    /// <summary>
    /// This contract gets or sets the properties for map user role section.
    /// </summary>
    public class MapUserRoleContract
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// UserID</summary>
        /// <value>
        /// Gets or sets the value for user's id.</value>
        public String UserId
        {
            get;
            set;
        }

        /// <summary>
        /// UserID</summary>
        /// <value>
        /// Gets or sets the value for user's id.</value>
        public Int32 OrganizationUserId
        {
            get;
            set;
        }

        /// <summary>
        /// NewMappedRoles</summary>
        /// <value>
        /// Gets or sets the list of all new mapping of roles.</value>
        public List<String> NewMappedRoles
        {
            set;
            get;
        }

        /// <summary>
        /// CreatedByUserID</summary>
        /// <value>
        /// Gets or sets the value for created by user's id.</value>
        public Int32 CreatedByUserId
        {
            get;
            set;
        }

        /// <summary>
        /// RoleDetails</summary>
        /// <value>
        /// Gets or sets the list of role details.</value>
        public List<aspnet_Roles> RoleDetails
        {
            set;
            get;
        }

        /// <summary>
        /// AspnetUserInRoles</summary>
        /// <value>
        /// Gets or sets the value for AspnetUserInRoles.</value>
        public List<vw_aspnet_UsersInRoles> AspnetUserInRoles
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the default password.
        /// </summary>
        /// <value>The default password.</value>
        /// <remarks></remarks>
        public String DefaultPassword
        {
            get;
            set;
        }

        /// <summary>
        /// CreatedByUserID</summary>
        /// <value>
        /// Gets or sets the value for created by user's id.</value>
        public Int32 UserGroupId
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