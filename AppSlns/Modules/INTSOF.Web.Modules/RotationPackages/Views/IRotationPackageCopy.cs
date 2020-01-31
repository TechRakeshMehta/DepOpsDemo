using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.RotationPackages.Views
{
    public interface IRotationPackageCopy
    {
        IRotationPackageCopy CurrentViewContext { get; }

        String ErrorMessage { get; set; }

        Int32 CurrentLoggedInUserId { get; }

        List<Int32> LstSelectedAgencyIDs { get; set; }

        List<AgencyDetailContract> LstAgencyDetailContract { get; set; }

        Int32 RequirementPackageID { get; set; }

        String RequirementPackageName { get; set; }

        String RequirementPackageLabel { get; set; }

        RequirementPackageContract RequirementPackage { get; set; }
        Int32 TenantId { get; set; }

        List<Int32> LstAgencyHierarchyIDs { get; set; }

        Boolean IsRotationPkgCopyFromAgencyHierarchy { get; set; } //UAT-3494
    }
}
