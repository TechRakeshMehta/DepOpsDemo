#region Namespaces

#region SystemDefined

using System.Collections.Generic;
using System.Linq;
using System;

#endregion

#region UserDefined

using Entity.ClientEntity;
using INTSOF.Utils;
using INTSOF.UI.Contract.ProfileSharing;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;

#endregion

#endregion

namespace CoreWeb.ProfileSharing.Views
{
    public interface IManageInvitationsSharedUserView
    {
        Int32 CurrentLoggedInUserId { get; }
        Int32 TenantId { get; set; }
        String CurrentUserId { get; }
        IManageInvitationsSharedUserView CurrentViewContext { get; }

        List<String> SharedUserTypeCodes
        { get; }
    }
}
