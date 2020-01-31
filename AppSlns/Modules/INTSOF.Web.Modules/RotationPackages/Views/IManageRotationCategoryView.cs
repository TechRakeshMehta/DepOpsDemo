#region Namespaces

#region SystemDefined

using System;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;

#endregion

#region UserDefined



#endregion

#endregion

namespace CoreWeb.RotationPackages.Views
{
    public interface IManageRotationCategoryView
    {
        IManageRotationCategoryView CurrentViewContext { get; }
        String ErrorMessage { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        Int32 SelectedTenantId { get; set; }
        Int32 DefaultTenantId { get; set; }
        Int32 RequirementPackageID { get; set; }
        Boolean IsFromPackageScreen { get; set; }
        Boolean IsNewPackage { get; set; }
        RequirementPackageContract RequirementPackageContractSessionData { get; set; } 
        //UAT-2551
        Int32 OrganisationUserID { get; }
    }
}


