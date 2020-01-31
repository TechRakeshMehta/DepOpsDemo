using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.RotationPackages.Views
{
    public interface IBulkRotationPackageCopyView
    {
        String ReqRotPackageIDs { get; set; }

        List<AgencyDetailContract> lstAgency { get; set; }

        Int32 CurrentLoggedInUserId { get; set; }
        Int32 TenantID { get; set; }

        List<RequirementPackageContract> lstRequirementPackageContract
        { get; set; }
        DateTime RotationReqEffecticeStartDate { get; set; }
    }
}
