#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ForgotPassword.cs
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
    /// This contract gets or sets the properties for forgot password section.
    /// </summary>
    public class ForgotPasswordContract
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// Email</summary>
        /// <value>
        /// Gets or sets the value for email address.</value>
        public String Email
        {
            get;
            set;
        }

        /// <summary>
        /// VerificationCode</summary>
        /// <value>
        /// Gets or sets the value for Verification Code.</value>
        public String VerificationCode
        {
            get;
            set;
        }

        /// <summary>
        /// ResetPassword</summary>
        /// <value>
        /// Gets or sets the value for Reset Password.</value>
        public String ResetPassword
        {
            get;
            set;
        }

        /// <summary>
        /// UserName</summary>
        /// <value>
        /// Gets or sets the value for user's name.</value>
        public String UserName
        {
            get;
            set;
        }

        /// <summary>
        /// LoginUserName</summary>
        /// <value>
        /// Gets or sets the value for login user's name.</value>
        public String LoginUserName
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
        /// OperationStatus</summary>
        /// <value>
        /// Gets or sets the value for Operation Status.</value>
        public Boolean OperationStatus
        {
            get;
            set;
        }


        /// <summary>
        /// TenantName</summary>
        /// <value>
        /// Gets or sets the value for TenantName</value>
        public string  TenantName
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