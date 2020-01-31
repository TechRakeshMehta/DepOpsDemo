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
namespace CoreWeb.ComplianceAdministration.Views
{
    public interface IManageAttributeGroupView
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

        Int32 SelectedTenantID
        {
            set;
            get;
        }

        Int32 CurrentUserId
        {
            get;
        }


        IQueryable<ComplianceAttributeGroup> ListComplianceAttributeGroup
        {
            get;
            set;
        }

        IManageAttributeGroupView CurrentViewContext
        {
            get;

        }
        String Name
        {
            get;
            set;
        }

        String Label
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
        Int32 ComplianceAttributeGroupId
        {
            get;
            set;
        }
        Int32 DefaultTenantId
        {
            get;
            set;
        }
        Int32 TenantId
        {
            get;
            set;
        }
        #endregion

        #endregion
    }
}




