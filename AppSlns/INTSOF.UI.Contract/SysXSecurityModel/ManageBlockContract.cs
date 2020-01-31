#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ManageBlockContract.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;

#endregion

#region Application Specific

#endregion

#endregion

namespace INTSOF.UI.Contract.IntsofSecurityModel
{
    /// <summary>
    /// This contract gets or sets the properties for manage blocks section.
    /// </summary>
    public class ManageBlockContract
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// SysXBlockID</summary>
        /// <value>
        /// Gets or sets the value for SysXBlock's id.</value>
        public Int32 SysXBlockId
        {
            get;
            set;
        }

        /// <summary>
        /// Name</summary>
        /// <value>
        /// Gets or sets the value for block's name.</value>
        public String Name
        {
            get;
            set;
        }

        /// <summary>
        /// Description</summary>
        /// <value>
        /// Gets or sets the value for block's description.</value>
        public String Description
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

        public String Code { get; set; }

        public Int16 BusinessChannelTypeID { get; set; }
    }
}
