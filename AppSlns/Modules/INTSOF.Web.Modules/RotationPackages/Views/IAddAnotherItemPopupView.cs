using System;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;

namespace CoreWeb.RotationPackages.Views
{
    public interface IAddAnotherItemPopupView
    {
        IAddAnotherItemPopupView CurrentViewContext { get; }
        String ErrorMessage { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        Int32 SelectedTenantId { get; set; }
        Int32 DefaultTenantId { get; set; }
        Int32 RotationPackageID { get; set; }
        String PreviousPage { get; set; }
        String CategoryName { get; set; }
        RequirementPackageContract RequirementPackageContractSessionData { get; set; }

    }
}

