#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ChangePassword.cs
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
    /// This contract gets or sets the properties for change password section.
    /// </summary>
    public class ChangePasswordContract
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// OperationStatus</summary>
        /// <value>
        /// Gets or sets the value for Operation Status.</value>
        public Boolean OperationStatus
        {
            get;
            set;
        }

        /// <summary>
        /// OrganizationUserID</summary>
        /// <value>
        /// Gets or sets the value for organization user's id.</value>
        public Int32 OrganizationUserId
        {
            get;
            set;
        }

        /// <summary>
        /// email</summary>
        /// <value>
        /// Gets or sets the value for Email id.</value>
        public String Email
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