#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ManagePermissionType.cs
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
    /// This contract gets or sets the properties for manage permission types section.
    /// </summary>
    public class ManagePermissionTypeContract
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// PermissionTypeID
        /// </summary>
        /// <value>Gets or sets the value for permission type's id.</value>
        /// <remarks></remarks>
        public Int32 PermissionTypeId
        {
            get;
            set;
        }

        /// <summary>
        /// TotalRowCount
        /// </summary>
        /// <value>Gets or sets the value for total number of rows.</value>
        /// <remarks></remarks>
        public Int32 TotalRowCount
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