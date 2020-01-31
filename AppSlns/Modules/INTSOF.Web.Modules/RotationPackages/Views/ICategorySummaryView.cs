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
    public interface ICategorySummaryView
    {
        ICategorySummaryView CurrentViewContext { get; }
        String ErrorMessage { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        Int32 SelectedTenantId { get; set; }
        Int32 DefaultTenantId { get; set; }
        Int32 RotationPackageID { get; set; }
        String CategoryName { get; set; }
        String ItemName { get; set; }
        RequirementPackageContract RequirementPackageContractSessionData { get; set; }
        Boolean IsViewOnly { get; set; }
        Boolean IsSharedUser { get; }
        //UAT-2551
        Int32 OrganisationUserID { get; }
    }
}




