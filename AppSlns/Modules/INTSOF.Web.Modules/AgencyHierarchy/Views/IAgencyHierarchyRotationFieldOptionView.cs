using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.AgencyHierarchy.Views
{
    public interface IAgencyHierarchyRotationFieldOptionView
    {
        IAgencyHierarchyRotationFieldOptionView CurrentViewContext { get; }

        Int32 CurrentLoggedInUserId { get; }

        Int32 AgencyHierarchyID { get; set; }

        Boolean IsRotationFieldOptionSettingExisted { get; set; }

        Boolean IsRootNode { get; set; }

        AgencyHierarchyRotationFieldOptionContract AgencyHierarchyRotationFieldOptionContract { get; set; }
    }
}
