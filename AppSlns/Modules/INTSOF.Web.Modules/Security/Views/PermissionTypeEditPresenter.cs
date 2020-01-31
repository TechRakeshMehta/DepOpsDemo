#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  PermissionTypeEditPresenter.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using INTSOF.Utils;
using INTSOF.SharedObjects;

#endregion

#region Application Specific

using Entity;
using Business.RepoManagers;

#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel.Views
{
    /// <summary>
    /// This class handles all the CRUD(Create/ Read/ Update/ Delete) operations for permission types section of security module.
    /// </summary>
    public class PermissionTypeEditPresenter : Presenter<IPermissionTypeEditView>
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
        /// Performs an insert operation for PermissionType.
        /// </summary>
        public void PermissionTypeSave()
        {
            if (SecurityManager.IsPermissionTypeExists(View.Name))
            {
                View.ErrorMessage = View.Name + SysXUtils.GetMessage(ResourceConst.SPACE) +
                                    SysXUtils.GetMessage(ResourceConst.SECURITY_PERMISSIONTYPE_ALREADY_EXISTS);
            }
            else
            {
                SecurityManager.AddPermissionType(new PermissionType
                                                      {
                                                          Name = View.Name,
                                                          Description = View.Description,
                                                          IsActive = true,
                                                          IsDeleted = false,
                                                          CreatedByID = View.CurrentUserId,
                                                          CreatedOn = DateTime.Now
                                                      });
            }
        }

        /// <summary>
        /// Performs an update operation PermissionType.
        /// </summary>
        public void PermissionTypeUpdate()
        {
            PermissionType permissionType = SecurityManager.GetPermissionType(View.PermissionTypeId);

            if (SecurityManager.IsPermissionTypeExists(View.Name, permissionType.Name))
            {
                View.ErrorMessage = View.Name + " " + SysXUtils.GetMessage(ResourceConst.SECURITY_PERMISSION_ALREADY_EXISTS);
            }
            else
            {
                View.ErrorMessage = String.Empty;

                permissionType.Name = View.Name;
                permissionType.Description = View.Description;
                permissionType.ModifiedByID = View.CurrentUserId;
                permissionType.ModifiedOn = DateTime.Now;
                SecurityManager.UpdatePermisionType(permissionType);
            }
        }

        #endregion

        #region Private Methods

        #endregion

        #endregion
    }
}