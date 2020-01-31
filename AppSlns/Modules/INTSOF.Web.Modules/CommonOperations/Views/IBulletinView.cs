using Entity;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.UI.Contract.PackageBundleManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.CommonOperations.Views
{
    public interface IBulletinView
    {
        Int32 CurrentUserId { get; }
        Int32 TenantId { get; set; }
        //String BulletinTitle { get; set; }
        //String BulletinContents { get; set; }
        BulletinContract ViewContract { get; set; }
        List<BulletinContract> BulletinDetails { get; set; }
        String ErrorMessage { get; set; }
        String SuccessMessage { get; set; }
        List<Tenant> ListTenants { get; set; }
        List<Int32> SelectedTenantID { set; get; }
        List<Int32> SelectedHierarchyIds { set; get; }
        Boolean IsADBAdmin { get; set; }
    }
}
