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
    public interface IManageRotationFieldView
    {
        IManageRotationFieldView CurrentViewContext { get; }
        String ErrorMessage { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        Int32 SelectedTenantId { get; set; }
        Int32 DefaultTenantId { get; set; }
        Int32 RotationPackageID { get; set; }
        String ItemName { get; set; }
        List<RotationFieldDataTypeContract> LstRotationFieldDataType { get; set; }
        Boolean IsDocumentSaved { get; }
        List<String> LstDocumentFieldCodes { get; }
        RequirementPackageContract RequirementPackageContractSessionData { get; set; }
        Boolean IsSharedUser { get; }
        //UAT-2551
        Int32 OrganisationUserID { get; }
    }
}




