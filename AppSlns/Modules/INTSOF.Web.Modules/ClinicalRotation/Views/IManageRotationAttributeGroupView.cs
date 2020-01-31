#region Namespaces

#region SystemDefined

using System.Collections.Generic;
using System.Linq;
using System;

#endregion

#region UserDefined

using Entity.ClientEntity;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;

#endregion

#endregion

namespace CoreWeb.ClinicalRotation.Views
{
    public interface IManageRotationAttributeGroupView
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


        List<RequirementAttributeGroupContract> ListRotationAttributeGroup
        {
            get;
            set;
        }

        IManageRotationAttributeGroupView CurrentViewContext
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
        Int32 RequirementAttributeGroupId
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
