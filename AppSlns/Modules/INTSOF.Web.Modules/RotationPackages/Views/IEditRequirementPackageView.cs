using System;
using System.Collections.Generic;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;

namespace CoreWeb.RotationPackages.Views
{
    public interface IEditRequirementPackageView
    {
        IEditRequirementPackageView CurrentViewContext { get; }
        String ErrorMessage { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        Int32 SelectedTenantId { get; set; }
        RequirementPackageContract RequirementPackageContractSessionData { get; set; }
        List<Int32> LstSelectedAgencyIDs { get; set; }
        List<String> LstSelectedAgencyNames { get; }
        List<AgencyDetailContract> LstAgencyDetailContract
        {
            get;
            set;
        }
        Boolean IsSharedUser { get; }

        List<TenantDetailContract> LstTenant
        {
            get;
            set;
        }

        List<Int32> LstSelectedTenantIDs { get; set; }
        List<String> LstSelectedTenantNames { get; }

        Guid CurrentUserID { get; }

        Int32 TenantID { get; set; }

        List<DefinedRequirementContract> lstDefinedRequirement { get; set; }

        List<Int32> LstAgencyHierarchyIDs { get; set; } //UAT-2647
        List<Int32> lstPrevAgencyHierarchyIds { get; set; }
        //UAT-2551
        Int32 OrganisationUserID { get; }
    }
}



