#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  IChangePasswordView.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;

#endregion

#region Application Specific

using INTSOF.UI.Contract.IntsofSecurityModel;

#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel.Views
{
    /// <summary>
    /// This class handles all the CRUD(Create/ Read/ Update/ Delete) operations for change password.
    /// </summary>
    public interface IChangePasswordView
    {
        #region Variables

        #endregion

        #region Properties

        /// <summary>
        /// ErrorMessage</summary>
        /// <value>
        /// Gets or sets the value for error message.</value>
        String ErrorMessage
        {
            get;
            set;
        }

        /// <summary>
        /// OldPassword</summary>
        /// <value>
        /// Gets or sets the value for old password.</value>
        String OldPassword
        {
            get;
            set;
        }

        /// <summary>
        /// NewPassword</summary>
        /// <value>
        /// Gets or sets the value for new password.</value>
        String NewPassword
        {
            get;
            set;
        }

        /// <summary>
        /// ConfirmPassword</summary>
        /// <value>
        /// Gets or sets the value for confirm password.</value>
        String ConfirmPassword
        {
            get;
            set;
        }

        /// <summary>
        /// DefaultLineOfBusiness</summary>
        /// <value>
        /// Gets or sets the value for Default Line of Business.</value>
        String DefaultLineOfBusiness
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the view contract.
        /// </summary>
        /// <remarks></remarks>
        ChangePasswordContract ViewContract
        {
            get;
        }

        Int32 OrgUsrID
        {
            get;
        }

        string OLDPASWRDNOTMATCH
        {
            get;
            
        }

        string PSWRDCHNGESUCSESFLY
        {
            get;            
        }

        #endregion

        #region Events

        #endregion

        #region Methods

        #endregion
    }
}