#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ManagePermission.cs
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
    /// This contract gets or sets the properties for manage permissions section.
    /// </summary>
    public class ManagePermissionContract
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// PermissionID</summary>
        /// <value>
        /// Gets or sets the value for permission's id.</value>
        public Int32 PermissionId
        {
            get;
            set;
        }

        /// <summary>
        /// Name</summary>
        /// <value>
        /// Gets or sets the value for permission's name.</value>
        public String Name
        {
            get;
            set;
        }

        /// <summary>
        /// Description</summary>
        /// <value>
        /// Gets or sets the value for permission's description.</value>
        public String Description
        {
            get;
            set;
        }

        /// <summary>
        /// PermissionTypeID</summary>
        /// <value>
        /// Gets or sets the value for permission type's id.</value>
        public Int32 PermissionTypeId
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