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
    public interface IManageUserGroupsView
    {
        #region Properties

        #region Private Properties

        #endregion

        #region Public Properties

        List<Tenant> ListTenants
        {
            set;
            get;
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

        Int32 CurrentUserId
        {
            get;
        }


        List<UserGroup> ListUserGroup
        {
            get;
            set;
        }

        IManageUserGroupsView CurrentViewContext
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
        Int32 UserGroupId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the current view context.
        /// </summary>
        /// <remarks></remarks>
        //IManageInstitutionNodeTypeView CurrentViewContext
        //{
        //    get;

        //}

        String HierarchyNode
        {
            get;
            set;
        }

        List<Int32> ListSelectedUserGroupIds { get; set; }
        #endregion

        #endregion
    }
}




