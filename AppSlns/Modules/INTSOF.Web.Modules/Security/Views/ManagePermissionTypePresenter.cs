#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ManagePermissionTypePresenter.cs
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

using Entity;
using Business.RepoManagers;
using INTSOF.Utils;

#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel.Views
{
    /// <summary>
    /// This class has the method's implementation which performs all the CRUD(Create/ Read/ Update/ Delete) operation for managing permission type with its details.
    /// </summary>
    public class ManagePermissionTypePresenter : Presenter<IManagePermissionTypeView>
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
        /// Method get data PermissionTypes list from Business and assign to View's source property(PermissionTypes) 
        /// </summary>
        public void RetrievingPermissionTypes()
        {
            IQueryable<PermissionType> permissionTypeList = SecurityManager.GetPermissionTypes();
            View.PermissionTypes = permissionTypeList;
            View.ViewContract.TotalRowCount = permissionTypeList.Count();
        }

        /// <summary>
        /// Performs the operation to remove a permission type.
        /// </summary>
        public void DeletePermissionType()
        {
            PermissionType permissionType = SecurityManager.GetPermissionType(View.ViewContract.PermissionTypeId);

            if (!SecurityManager.IsPermissionTypeAssignToAnyPermission(View.ViewContract.PermissionTypeId).Equals(false))
            {
                throw new SysXException(SysXUtils.GetMessage(ResourceConst.SECURITY_PERMISSION_TYPE_NOT_REMOVE) + SysXUtils.GetMessage(ResourceConst.SPACE) + permissionType.Name +
                                    SysXUtils.GetMessage(ResourceConst.SPACE) +
                                    SysXUtils.GetMessage(ResourceConst.SECURITY_FEATURE_DETAILS));
            }
            else
            {
                permissionType.IsDeleted = true;
                permissionType.ModifiedOn = DateTime.Now;
                permissionType.ModifiedByID = View.CurrentUserId;
                permissionType.Name = permissionType.Name + "_" + Guid.NewGuid();
                SecurityManager.DeletePermissionType(permissionType);
                View.SuccessMessage = SysXUtils.GetMessage(ResourceConst.PERMISSION_TYPE) + SysXUtils.GetMessage(ResourceConst.SPACE) + permissionType.Name + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.DELETED_SUCCESSFULLY);
            }
        }

        #endregion

        #region Private Methods

        #endregion

        #endregion
    }
}