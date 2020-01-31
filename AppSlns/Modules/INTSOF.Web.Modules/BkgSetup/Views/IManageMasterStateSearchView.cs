using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.UI.Contract.BkgSetup;

namespace CoreWeb.BkgSetup.Views
{
    public interface IManageMasterStateSearchView
    {
        String ErrorMessage { get; set; }
        List<Entity.State> lstState { get; set; }
        List<BkgPackageStateSearchContract> lstStateSearchContract { get; set; }
        List<Entity.BkgMasterStateSearch> lstMasterStateSearch { get; set; }
        Int32 DefaultTenantId { get; set; }
        Int32 CurrentLoggedInUserId { get; }
    }
}
