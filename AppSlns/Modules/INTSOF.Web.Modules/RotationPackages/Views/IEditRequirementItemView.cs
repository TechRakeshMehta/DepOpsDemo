using System;
using System.Collections.Generic;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;

namespace CoreWeb.RotationPackages.Views
{
    public interface IEditRequirementItemView
    {
        IEditRequirementItemView CurrentViewContext { get; }
        String ErrorMessage { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        Int32 SelectedTenantId { get; set; }
        Int32 DefaultTenantId { get; set; }
        Int32 RotationPackageID { get; set; }
        String PreviousPage { get; set; }
        String CategoryName { get; set; }
        String CategoryLabel { get; set; }
        List<RulesConstantTypeContract> LstRulesConstantTypeContract { get; set; }
        RequirementPackageContract RequirementPackageContractSessionData { get; set; }
        Boolean IsSharedUser { get; }

        Int32 OrganisationUserID { get; }
    }
}


