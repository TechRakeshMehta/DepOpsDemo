#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ManagePolicyPresenter.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using INTSOF.SharedObjects;

#endregion

#region Application Specific

using Business.RepoManagers;
using Entity;
using INTSOF.Utils;

#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel.Views
{
    /// <summary>
    /// This class has the method's implementation which performs all the CRUD(Create/ Read/ Update/ Delete) operation for managing policy with its details.
    /// </summary>
    public class ManagePolicyPresenter : Presenter<IManagePolicyView>
    {
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

        #region Events

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Parent feature.
        /// </summary>
        public void ParentFeature()
        {
            //here we will set parent role id.
            OrganizationUser user = SecurityManager.GetOrganizationUser(View.ViewContract.OrganizationUserId);

            if (!user.IsNull())
            {
            }

            //end parent Role Id
            View.AllPolicySetUserControls = SecurityManager.GetAllPoliciesSubParent(View.ViewContract.UcName, String.Empty, View.ViewContract.Admins, View.ViewContract.OrganizationUserId);
        }

        /// <summary>
        /// Retrieves a list of all Registered Controls.
        /// </summary>
        public void RetrievingRegisteredControlList()
        {
            View.PolicyRegisterControls = SecurityManager.GetPolicyRegisterControls(View.ViewContract.MappedRoleId);
        }

        /// <summary>
        ///  Performs an insert operation for policy with it's details.
        /// </summary>
        public void SavePolicy()
        {
            SecurityManager.SavePolicies(View.PolicySetUserControls, View.ViewContract.MappedRoleId);
        }

        /// <summary>
        /// Retrieves a list of all registered user controls.
        /// </summary>
        public void RetrievingRegisteredControls()
        {
            View.ExpandedPolicySetUserControl = SecurityManager.GetControlSelectedValues(View.ViewContract.MappedRoleId, View.ViewContract.RegisterUserControlId);
            View.RegisteredControls = SecurityManager.GetRegisteredControlList();
        }

        #endregion

        #region Private Methods

        #endregion

        #endregion
    }
}