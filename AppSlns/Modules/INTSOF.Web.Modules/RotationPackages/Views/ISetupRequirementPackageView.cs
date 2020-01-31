using System;
using System.Collections.Generic;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;

namespace CoreWeb.RotationPackages.Views
{
    public interface ISetupRequirementPackageView
    {
        ISetupRequirementPackageView CurrentViewContext { get; }
        String ErrorMessage { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        List<Int32> LstSelectedAgencyIDs { get; set; }
        List<AgencyDetailContract> LstAgencyDetailContract
        {
            get;
            set;
        }
        Guid CurrentUserID { get; }
        Int32 SelectedPackageID { get; set; }
        RequirementPackageContract RequirementPackageContract { get; set; }
        List<RequirementPackageTypeContract> LstRequirementPackageType { get; set; }
        Boolean IsPackageUsed { get; set; }

        bool IsEditMode { get; set; }

        bool IsNewPackage { get; set; }

        bool IsViewOnly { get; set; }

        Int32 TenantId { get; set; }

        List<DefinedRequirementContract> lstDefinedRequirement { get; set; }

        List <Int32> LstAgencyHierarchyIds { get; set; }

        List<Int32> lstPrevAgencyHierarchyIds { get; set; }
    }
}