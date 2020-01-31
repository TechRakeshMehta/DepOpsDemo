#region Namespaces

#region SystemDefined

using System;
using System.Collections.Generic;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;

#endregion

#region UserDefined



#endregion

#endregion

namespace CoreWeb.RotationPackages.Views
{
    public interface IManageRequirementPackageView
    {
        IManageRequirementPackageView CurrentViewContext { get; }
        String ErrorMessage { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        Int32 SelectedTenantId { get; set; }
        Int32 DefaultTenantId { get; set; }
        Boolean IsDataSavedSucessfully { get; set; }
        Boolean IsFromRotationScreen { get; set; }
        Int32 RequirementPackageID { get; set; }
        List<RequirementPackageDetailsContract> LstRequirementPackageDetailsContract { get; set; }
        RequirementPackageContract RequirementPackageContractSessionData { get; set; }
        Boolean IsSharedUser { get; }
        //UAT-2551
        Int32 OrganisationUserID { get; }
    }
}

