#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  MapRoleFeature.cs
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
    /// This contract gets or sets the properties for mapping role with features section.
    /// </summary>
    public class MapRoleFeatureContract
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// FeatureCount</summary>
        /// <value>
        /// Gets or sets the value for number of features.</value>
        public Int32 FeatureCount
        {
            get;
            set;
        }

        /// <summary>
        /// UpdatedSysXBlockIDs</summary>
        /// <value>
        /// Gets or sets the list of all ids for updated sysx block.</value>
        public List<Int32> UpdatedSysXBlockIDs
        {
            get;
            set;
        }

        /// <summary>
        /// FeaturePermissions</summary>
        /// <value>
        /// Gets or sets the list of all permissions of features.</value>
        public Dictionary<Int32, Int32> FeaturePermissions
        {
            set;
            get;
        }

        /// <summary>
        /// RoleID</summary>
        /// <value>
        /// Gets or sets the value for role's id.</value>
        public String RoleId
        {
            get;
            set;
        }

        /// <summary>
        /// BlockId</summary>
        /// <value>
        /// Gets or sets the value for block's id.</value>
        public Int32 BlockId
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