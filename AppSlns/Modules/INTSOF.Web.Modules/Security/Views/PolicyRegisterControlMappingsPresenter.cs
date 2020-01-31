#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  PolicyRegisterControlMappingsPresenter.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Linq;
using INTSOF.SharedObjects;

#endregion

#region Application Specific

using INTSOF.Utils;
using Business.RepoManagers;
using Entity;

#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel.Views
{
    /// <summary>
    /// This class has the method's implementation which performs all the CRUD(Create/ Read/ Update/
    /// Delete) operation for managing policy register control with its details.
    /// </summary>
    public class PolicyRegisterControlMappingsPresenter : Presenter<IPolicyRegisterControlMappingsView>
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
        /// Removes the mapping between policy and controls.
        /// </summary>
        public void DeletePolicyRegisterControlMapping()
        {
            PolicyRegisterUserControl policyregistercontrol = SecurityManager.GetPolicyRegisterControl(View.ViewContract.PolicyRegisterControlId);
            Int32 numberOfChildPolicyControls = SecurityManager.SelectChildPolicyControls(Convert.ToInt32(View.ViewContract.PolicyRegisterControlId));

            if (numberOfChildPolicyControls <= AppConsts.NONE)
            {
                SecurityManager.DeletePolicyRegisterControl(policyregistercontrol);
                View.SuccessMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_MANAGE_POLICY_REGISTER_CONTROLS) + SysXUtils.GetMessage(ResourceConst.SPACE) + policyregistercontrol.DisplayName + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.DELETED_SUCCESSFULLY);
            }
        }

        /// <summary>
        /// Retrieves all mapping of policy with controls.
        /// </summary>
        public void RetrievingPolicyRegisterControlMappings()
        {
            View.PolicyRegisterControls = SecurityManager.GetPolicyRegisterControls();
        }

        /// <summary>
        /// Performs insertion for Policy Register controls.
        /// </summary>
        public void AddPolicyRegisterControlMapping()
        {
            if (SecurityManager.IsControlNameExists(View.ViewContract.ControlName))
            {
                View.ErrorMessage = View.ViewContract.ControlName + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.SECURITY_CONTROL_NAME_ALREADY_EXISTS);
            }
            else
            {
                var policyregisterControl = new PolicyRegisterUserControl
                                                                      {
                                                                          ControlName = View.ViewContract.ControlName,
                                                                          DisplayName = View.ViewContract.DisplayName,
                                                                          ControlPath = View.ViewContract.ControlPath
                                                                      };
                if (!View.ViewContract.ParentControlId.Equals(AppConsts.NONE))
                {
                    policyregisterControl.ParentUserControlID = View.ViewContract.ParentControlId;
                }

                SecurityManager.AddPolicyRegisterControl(policyregisterControl);
                View.SuccessMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_MANAGE_POLICY_REGISTER_CONTROLS) + SysXUtils.GetMessage(ResourceConst.SPACE) + policyregisterControl.DisplayName + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.SAVED_SUCCESSFULLY);
            }
        }

        /// <summary>
        /// Retrieving permission types.
        /// </summary>
        public void RetrievingPermissionTypes()
        {
            View.PermissionTypes = SecurityManager.GetPermissionTypes();
        }

        /// <summary>
        /// Updates the policy register control mapping.
        /// </summary>
        public void UpdatePolicyRegisterControlMapping()
        {
            PolicyRegisterUserControl policyregistercontrolById = SecurityManager.GetPolicyRegisterControl(View.ViewContract.RegisterControlId);
            if (SecurityManager.IsControlNameExists(View.ViewContract.ControlName, policyregistercontrolById.ControlName))
            {
                View.ErrorMessage = View.ViewContract.ControlName + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.SECURITY_CONTROL_NAME_ALREADY_EXISTS);
            }
            else
            {
                PolicyRegisterUserControl policyregistercontrol = SecurityManager.GetPolicyRegisterControl(View.ViewContract.RegisterControlId);
                policyregistercontrol.ControlName = View.ViewContract.ControlName;
                policyregistercontrol.DisplayName = View.ViewContract.DisplayName;
                policyregistercontrol.ControlPath = View.ViewContract.ControlPath;
                SecurityManager.UpdatePolicyRegisterControl(policyregistercontrol);
                View.SuccessMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_MANAGE_POLICY_REGISTER_CONTROLS) + SysXUtils.GetMessage(ResourceConst.SPACE) + policyregistercontrol.DisplayName + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.UPDATED_SUCCESSFULLY);
            }
        }

        #endregion

        #endregion
    }
}