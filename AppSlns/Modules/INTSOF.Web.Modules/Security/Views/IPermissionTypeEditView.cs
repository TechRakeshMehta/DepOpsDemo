#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  IPermissionEditView.cs
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
    /// This interface handles the declaration of variables, properties, methods , events for managing Permission Types.
    /// </summary>
    public interface IPermissionTypeEditView
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
        /// Gets or sets the permission type Id.
        /// </summary>
        Int32 PermissionTypeId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        String Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        String Description
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the current user's id.
        /// </summary>
        Int32 CurrentUserId
        {
            get;
        }

        /// <summary>
        /// Gets the view contract.
        /// </summary>
        /// <remarks></remarks>
        PermissionTypeEditContract ViewContract
        {
            get;
        }

        #endregion

        #region Events

        #endregion

        #region Methods

        /// <summary>
        /// Saves the permission type information.
        /// </summary>
        void SavePermissionType();

        /// <summary>
        /// Updates the permission type information.
        /// </summary>
        void UpdatePermissionType();

        #endregion
    }
}