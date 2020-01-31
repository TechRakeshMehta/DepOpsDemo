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
    public interface IManageRotationItemView
    {
        IManageRotationItemView CurrentViewContext { get; }
        String ErrorMessage { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        Int32 SelectedTenantId { get; set; }
        Int32 DefaultTenantId { get; set; }
        Int32 RotationPackageID { get; set; }
        String CategoryName { get; set; }
        String ItemName { get; set; }
        Boolean ShowItemNameControls { get; set; }
        Boolean ShowExpiryControls { get; set; }
        RequirementPackageContract RequirementPackageContractSessionData { get; set; }
        List<RulesConstantTypeContract> LstRulesConstantTypeContract { get; set; }
        Boolean IsSharedUser { get; }
        //UAT-2551
        Int32 OrganisationUserID { get; }
    }
}



