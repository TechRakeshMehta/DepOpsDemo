using System;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;

namespace CoreWeb.RotationPackages.Views
{
    public interface IAddAnotherFieldPopupView
    {
        IAddAnotherFieldPopupView CurrentViewContext { get; }
        String ErrorMessage { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        Int32 SelectedTenantId { get; set; }
        Int32 DefaultTenantId { get; set; }
        Int32 RotationPackageID { get; set; }
        String PreviousPage { get; set; }
        String CategoryName { get; set; }
        String ItemName { get; set; }
        RequirementPackageContract RequirementPackageContractSessionData { get; set; }
        //UAT-2551
        Int32 OrganisationUserID { get; }
    }
}


