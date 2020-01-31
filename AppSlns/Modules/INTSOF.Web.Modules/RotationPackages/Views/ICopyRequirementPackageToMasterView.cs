using System;
using System.Collections.Generic;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;

namespace CoreWeb.RotationPackages.Views
{
    public interface ICopyRequirementPackageToMasterView
    {
        ICopyRequirementPackageToMasterView CurrentViewContext { get; }
        String ErrorMessage { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        Int32 SelectedTenantId { get; set; }
        RequirementPackageContract RequirementPackageContractSessionData { get; set; }
        List<Int32> LstSelectedAgencyIDs { get; set; }
        List<AgencyDetailContract> LstAgencyDetailContract
        {
            get;
            set;
        }
        Boolean IsSharedUser { get; }
        Guid CurrentUserID { get; }

        Int32 TenantID { get; set; }
        //UAT-2647/
        List<Int32> LstSelectedAgencyHierarchyIDs { get; set; }
    }
}



