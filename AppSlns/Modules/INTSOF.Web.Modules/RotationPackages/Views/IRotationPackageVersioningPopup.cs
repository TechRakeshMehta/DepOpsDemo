using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.RotationPackages.Views
{
    public interface IRotationPackageVersioningPopup
    {
        IRotationPackageVersioningPopup CurrentViewContext { get; }

        RequirementPackageContract RequirementPackage { get; set; }

       // String ErrorMessage { get; set; }

        Int32 CurrentLoggedInUserId { get; }

        Int32 RequirementPackageID { get; set; }

        String RequirementPackageName { get; set; }

        String RequirementPackageLabel { get; set; }

        //Int32 TenantId { get; set; }

        String agencyHierarchyId { get; set; }

        DateTime? effectiveStartDate { get; set; }

        DateTime? effectiveEndDate { get; set; }
        RequirementPackageContract ExistingRequirementPackage { get; set; }
    }
}
