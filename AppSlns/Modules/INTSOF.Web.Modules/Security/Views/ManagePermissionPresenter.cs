#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ManagePermissionPresenter.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Linq;
using INTSOF.Utils;
using INTSOF.SharedObjects;

#endregion

#region Application Specific

using Business.RepoManagers;
using Entity;

#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel.Views
{
    /// <summary>
    /// This class has the method's implementation which performs all the CRUD(Create/ Read/ Update/ Delete) operation for managing permission with its details.
    /// </summary>
    public class ManagePermissionPresenter : Presenter<IManagePermissionView>
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
        ///  Performs a delete operation for permissions.
        /// </summary>
        public void DeletePermission()
        {
            Permission permission = SecurityManager.GetPermission(View.ViewContract.PermissionId);

            if (!SecurityManager.IsPermissionAssignToAnyFeature(View.ViewContract.PermissionId).Equals(false))
            {
                throw new SysXException(SysXUtils.GetMessage(ResourceConst.SECURITY_PERMISSION_NOT_REMOVE) + SysXUtils.GetMessage(ResourceConst.SPACE) + permission.Name +
                                    SysXUtils.GetMessage(ResourceConst.SPACE) +
                                    SysXUtils.GetMessage(ResourceConst.SECURITY_FEATURE_DETAILS));
            }
            else
            {
                permission.IsDeleted = true;
                permission.ModifiedByID = View.CurrentUserId;
                permission.ModifiedOn = DateTime.Now;
                View.SuccessMessage = SysXUtils.GetMessage(ResourceConst.PERMISSION) + SysXUtils.GetMessage(ResourceConst.SPACE) + permission.Name + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.DELETED_SUCCESSFULLY);
                permission.Name = permission.Name + "_" + Guid.NewGuid();
                SecurityManager.DeletePermission(permission);
            }
        }

        /// <summary>
        /// Retrieves a list of all permissions.
        /// </summary>
        public void RetrievingPermissions()
        {
            View.Permissions = SecurityManager.GetPermissions();
        }

        /// <summary>
        /// Performs an insert operation for permissions.
        /// </summary>
        public void PermissionSave()
        {
            if (SecurityManager.IsPermissionExists(View.ViewContract.Name))
            {
                View.ErrorMessage = View.ViewContract.Name + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.SECURITY_PERMISSION_ALREADY_EXISTS);
            }
            else
            {
                Permission permission = new Permission
                                            {
                                                Name = View.ViewContract.Name,
                                                Description = View.ViewContract.Description,
                                                PermissionType =
                                                    SecurityManager.GetPermissionType(View.ViewContract.PermissionTypeId),
                                                CreatedByID = View.CurrentUserId,
                                                CreatedOn = DateTime.Now,
                                                IsActive = true,
                                                IsDeleted = false
                                            };

                SecurityManager.AddPermission(permission);
                View.SuccessMessage = SysXUtils.GetMessage(ResourceConst.PERMISSION) + SysXUtils.GetMessage(ResourceConst.SPACE) + permission.Name + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.SAVED_SUCCESSFULLY);
            }
        }

        /// <summary>
        /// Performs an update operation for permissions.
        /// </summary>
        public void PermissionUpdate()
        {
            Permission permission = SecurityManager.GetPermission(View.ViewContract.PermissionId);

            if (SecurityManager.IsPermissionExists(View.ViewContract.Name, permission.Name))
            {
                View.ErrorMessage = View.ViewContract.Name + SysXUtils.GetMessage(ResourceConst.SPACE) +
                                    SysXUtils.GetMessage(ResourceConst.SECURITY_PERMISSION_ALREADY_EXISTS);
            }
            else
            {
                View.ErrorMessage = String.Empty;

                permission.Name = View.ViewContract.Name;
                permission.Description = View.ViewContract.Description;
                permission.PermissionType = SecurityManager.GetPermissionType(View.ViewContract.PermissionTypeId);
                permission.ModifiedByID = View.CurrentUserId;
                permission.ModifiedOn = DateTime.Now;
                permission.IsActive = true;
                permission.IsDeleted = false;

                SecurityManager.UpdatePermision(permission);
                View.SuccessMessage = SysXUtils.GetMessage(ResourceConst.PERMISSION) + SysXUtils.GetMessage(ResourceConst.SPACE) + permission.Name + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.UPDATED_SUCCESSFULLY);
            }
        }

        /// <summary>
        /// Retrieves a list of all Permission types.
        /// </summary>
        public void RetrievingPermissionTypes()
        {
            View.PermissionTypes = SecurityManager.GetPermissionTypes().ToList();
        }

        #endregion

        #region Private Methods

        #endregion

        #endregion
    }
}