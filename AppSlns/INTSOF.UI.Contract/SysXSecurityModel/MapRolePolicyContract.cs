#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  MapRolePolicy.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Collections.Generic;

#endregion

#region Application Specific

#endregion

#endregion

namespace INTSOF.UI.Contract.IntsofSecurityModel
{
    /// <summary>
    /// This contract gets or sets the properties for mapping role with policy section.
    /// </summary>
    public class MapRolePolicyContract
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// Admins</summary>
        /// <value>
        /// Gets or sets the list of all admins.</value>
        public List<Int32> Admins
        {
            get;
            set;
        }

        /// <summary>
        /// UcName</summary>
        /// <value>
        /// Gets or sets the value for UcName.</value>
        public String UcName
        {
            get;
            set;
        }

        /// <summary>
        /// OrganizationUserID</summary>
        /// <value>
        /// Gets or sets the value for Organization user's id.</value>
        public Int32 OrganizationUserId
        {
            get;
            set;
        }

        public String UserId
        {
            get;
            set;
        }

        /// <summary>
        /// RegisterUserControlID</summary>
        /// <value>
        /// Gets or sets the value for RegisterUserControlID.</value>
        public Int32 RegisterUserControlId
        {
            get;
            set;
        }

        public String MappedRoleId
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