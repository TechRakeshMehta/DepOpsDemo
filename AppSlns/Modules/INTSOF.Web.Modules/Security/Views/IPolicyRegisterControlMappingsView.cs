#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  IPolicyRegisterControlMappingsView.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Collections.Generic;

#endregion

#region Application Specific

using System.Linq;
using Entity;
using INTSOF.UI.Contract.IntsofSecurityModel;

#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel.Views
{
    /// <summary>
    /// This interface handles the operation of assigning the polices to any controls of the whole application.
    /// </summary>
    public interface IPolicyRegisterControlMappingsView
    {
        #region Variables

        #endregion

        #region Properties

        /// <summary>
        /// SuccessMessage</summary>
        /// <value>
        /// Gets or sets the value for Success message.</value>
        String SuccessMessage
        {
            get;
            set;
        }

        /// <summary>
        /// PolicyRegisterControls</summary>
        /// <value>
        /// Sets the value for policy register controls.</value>
        IQueryable<PolicyRegisterUserControl> PolicyRegisterControls
        {
            set;
        }

        /// <summary>
        /// PermissionTypes</summary>
        /// <value>
        /// Gets or sets the list of all permission types.</value>
        IQueryable<PermissionType> PermissionTypes
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the view contract.
        /// </summary>
        /// <remarks></remarks>
        PolicyRegisterControlMappingsContract ViewContract
        {
            get;
        }

        /// <summary>
        /// ErrorMessage</summary>
        /// <value>
        /// Gets or sets the value for ErrorMessage.</value>
        String ErrorMessage
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