using Entity.SharedDataEntity;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.PlacementMatching.Views
{
    public interface IPlacementView
    {
        bool IsAdminLoggedIn { get; set; }
        List<AgencyHierarchyContract> LstAgencies { get; set; }
        Boolean IsGridView { get; set; }
        Boolean IsCalendarView { get; set; }
        Guid CurrentLoggedInUserID { get;  }
        Int32 TenantID { get; set; }
        Int32 AgencyHierarchyID { get; set; }
        String StatusCode { get; set; }
        
    }
}
