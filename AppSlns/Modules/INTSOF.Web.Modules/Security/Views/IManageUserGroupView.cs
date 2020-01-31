#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  IManageUserGroupView.cs
// Purpose:   To Manage user group
//

#endregion

#region Namespace

#region System NameSpace

using System;
using System.Collections.Generic;
using System.Text;

#endregion

#region Application Specific

using INTSOF.UI.Contract.IntsofSecurityModel;

#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel.Views
{
    public interface IManageUserGroupView
    {
        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets the view contract.
        /// </summary>
        /// <remarks></remarks>
        ManageUserGroupContract ViewContract
        {
            get;
        }

        /// <summary>
        /// Gets the CreatedBy field of the User.
        /// </summary>
        Int32 CreatedById
        {
            get;
        }

        /// <summary>
        /// To display message
        /// </summary>
        String SuccessMessage
        {
            set;
        }

        /// <summary>
        /// Assigned Product Id.
        /// </summary>
        Int32? AssignToProductId
        {
            get;
        }

        #endregion

        #endregion

        #region Methods

        #region Public Methods

        #endregion

        #region Private Methods

        #endregion

        #endregion

        #region Events

        #endregion
    }
}