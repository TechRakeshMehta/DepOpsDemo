#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  DefaultViewPresenter.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using INTSOF.SharedObjects;
using Microsoft.Practices.ObjectBuilder;
using System.Collections.Generic;

#endregion

#region Application Specific

using Entity;
using Business.RepoManagers;


#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel.Views
{
    /// <summary>
    /// This class handles all the CRUD(Create/ Read/ Update/ Delete) operations for default page of security module.
    /// </summary>
    public class DefaultViewPresenter : Presenter<IDefaultView>
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the DefaultViewPresenter class.
        /// </summary>
        /// <param name="controller">controller value.</param>
        public DefaultViewPresenter([CreateNew] ISysXSecurityModelController controller)
        {
            // TODO: need to be removed, after discussion. 
        }

        public DefaultViewPresenter()
        {
            // TODO: need to be removed, after discussion. 
        }

        #endregion

        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        #endregion

        #region Private Properties

        #endregion

        #endregion

        #region Methods

        #region Public Methods
        public List<aspnet_Roles>  GetAllRoleOfUserGroup(int UserGroupId)
        {
            return SecurityManager.GetAllRoleOfUserGroup(UserGroupId);
        }

        #endregion

        #region Private Methods

        #endregion

        #endregion
    }
}