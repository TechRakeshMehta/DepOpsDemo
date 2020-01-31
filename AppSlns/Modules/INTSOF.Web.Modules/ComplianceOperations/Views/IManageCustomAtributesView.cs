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
    public interface IManageCustomAttributesView
    {
        List<Tenant> lstTenant
        {
            get;
            set;
        }

        Int32 SelectedTenantID
        {
            get;
            set;
        }

        Int32 CurrentUserId
        {
            get;
        }

        IQueryable<lkpCustomAttributeDataType> lstCustomAttDataType
        {
            get;
            set;
        }

        IList<lkpCustomAttributeDataType> lstCustomAttDataTypeList
        {
            get;
            set;
        }

        IQueryable<lkpCustomAttributeUseType> lstCustomAttUseType
        {
            get;
            set;
        }

        IList<lkpCustomAttributeUseType> lstCustomAttUseTypeList
        {
            get;
            set;
        }

        IQueryable<CustomAttribute> lstCustomAttributes
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

        String InfoMessage
        {
            get;
            set;
        }

        List<CustomAttribute> lstProfileCustomAttributes
        {
            get;
            set;
        }
    }
}




