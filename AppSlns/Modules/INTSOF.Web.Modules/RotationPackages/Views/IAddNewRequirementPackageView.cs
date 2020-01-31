#region Namespaces

#region SystemDefined

using System;
using System.Collections.Generic;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;

#endregion

#region UserDefined



#endregion

#endregion

namespace CoreWeb.RotationPackages.Views
{
    public interface IAddNewRequirementPackageView
    {
        IAddNewRequirementPackageView CurrentViewContext { get; }
        String ErrorMessage { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        Int32 SelectedTenantId { get; set; }
        Boolean IsSharedUser { get; }
        Int32 RotationPackageID { get; set; }
        Int32 AgencyId { get; set; }
        RequirementPackageContract RequirementPackageContractSessionData { get; set; }
        List<AgencyDetailContract> LstAgencyDetailContract { get; set; }
        List<Int32> LstSelectedAgencyIDs { get; set; }
        List<String> LstSelectedAgencyNames { get; }
        Boolean IsFromRotationScreen { get; set; }
        //UAT 1352:As a rotation package creation admin, I should be able to create packages rotation for the instructor/preceptor to use
        List<RequirementPackageTypeContract> LstRequirementPackageType { get; set; }
        String RequirementPkgTypeCode{ get; set; }
        List<TenantDetailContract> LstTenant
        {
            get;
            set;
        }
        Guid CurrentUserID { get; }
        List<Int32> LstSelectedTenantIDs { get; set; }
        List<String> LstSelectedTenantNames { get; }

        Int32 TenantID { get; set; }

        List<DefinedRequirementContract> lstDefinedRequirement { get; set; }

        List<Int32> LstAgencyHierarchyIDs { get; set; }

        String AgencyIDs { get; set; }

        //UAT-2551
        Int32 OrganisationUserID { get; }
    }
}


