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

namespace CoreWeb.Search.Views
{
    public interface IApplicantRequirementRotationsView
    {
        IApplicantRequirementRotationsView CurrentViewContext { get; }
        Int32 CurrentLoggedInUserId { get; }
        Int32 OrganizationUserId { get; }
        Int32 TenantId { get; }
        Int32 LoggedInUserTenantId { get; }
        List<ClinicalRotationDetailContract> lstApplicantRotations { get; set; }
        String QueueType { get; set; }
        String QueueTypeChild { get; set; }
        String UserId{get;}
      
    }
}




