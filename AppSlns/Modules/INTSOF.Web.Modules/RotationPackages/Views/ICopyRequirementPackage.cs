using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.RotationPackages.Views
{
    public interface ICopyRequirementPackage
    {
        ICopyRequirementPackage CurrentViewContext { get; }
        String ErrorMessage { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        RequirementPackageContract RequirementPackageContractSessionData { get; set; }
        List<Int32> LstSelectedAgencyIDs { get; set; }
        List<AgencyDetailContract> LstAgencyDetailContract
        {
            get;
            set;
        }
        Guid CurrentUserID { get; }
        Int32 TenantID { get; set; }

        List<Int32> LstAgencyHierarchyIDs { get; set; } //UAT-2648
    }
}
