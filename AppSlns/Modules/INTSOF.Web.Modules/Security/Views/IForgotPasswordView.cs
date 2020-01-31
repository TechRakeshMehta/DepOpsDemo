#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  IForgotPasswordView.cs
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
    /// This interface handles the declaration of variables, properties, methods , events for managing forgot password operations.
    /// </summary>
    public interface IForgotPasswordView
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
        /// Gets the view contract.
        /// </summary>
        /// <remarks></remarks>
        ForgotPasswordContract ViewContract
        {
            get;
        }

        Boolean IsUserNameReset
        {
            get;
        }

        String IncorrectVerificationCode
        {
            get;
        }

        Int32 cbi_TenantId
        {
            get;
            set;
        }

        #endregion

        #region Events

        #endregion

        #region Methods

        #endregion
    }
}