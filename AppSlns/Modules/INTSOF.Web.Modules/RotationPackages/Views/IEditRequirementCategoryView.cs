using System;
using System.Collections.Generic;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;

namespace CoreWeb.RotationPackages.Views
{
    public interface IEditRequirementCategoryView
    {
        IEditRequirementCategoryView CurrentViewContext { get; }
        String ErrorMessage { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        Int32 SelectedTenantId { get; set; }
        Int32 DefaultTenantId { get; set; }
        Int32 RotationPackageID { get; set; }
        String PreviousPage { get; set; }
        String CategoryName { get; set; }
        RequirementPackageContract RequirementPackageContractSessionData { get; set; }
        Boolean IsSharedUser { get; }

        Int32 RequirementCategoryID { get; set; }

        List<RequirementItemContract> lstCategoryItems { get; set; }

        Int32 OrganisationUserID { get; }
    }
}


