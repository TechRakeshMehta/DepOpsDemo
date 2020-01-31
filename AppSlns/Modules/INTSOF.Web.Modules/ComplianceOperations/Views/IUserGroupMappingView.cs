#region Namespaces

#region SystemDefined

using System.Collections.Generic;
using System.Linq;
using System;

#endregion

#region UserDefined

using Entity.ClientEntity;

#endregion

#endregion

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IUserGroupMappingView
    {
        #region Properties

        #region Public Properties

        String ErrorMessage
        {
            get;
            set;
        }
        String SuccessMessage
        {
            get;
            set;
        }
        Int32 TenantId
        {
            set;
            get;
        }
        Int32 SelectedTenantID
        {
            set;
            get;
        }
        Int32 CurrentLoggedInUserId
        {
            get;
        }
        List<UserGroup> ListUserGroup
        {
            get;
            set;
        }
        IUserGroupMappingView CurrentViewContext
        {
            get;

        }
        String UserGroupName
        {
            get;
            set;
        }
        String UserGroupDescription
        {
            get;
            set;
        }
        Int32 UserGroupId
        {
            get;
            set;
        }
        List<Int32> AssignUserGroupIds
        {
            get;
            set;
        }
        List<Int32> ApplicantUserIds
        {
            get;
        }
        String ScreenMode
        {
            get;
            set;
        }

        //UAT-3381
        String HierarchyNode
        {
            get;
            set;
        }

        #endregion

        #endregion
    }
}




