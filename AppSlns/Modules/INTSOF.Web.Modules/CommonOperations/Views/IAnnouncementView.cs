using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.UI.Contract.PackageBundleManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.CommonOperations.Views
{
    public interface IAnnouncementView
    {
        Boolean IsAdminLoggedIn { get; set; }
        Int32 TenantID { get; set; }
        Int32 CurrentUserId { get; }
        String ErrorMessage { get; set; }
        String SuccessMessage { get; set; }
        List<AnnouncementContract> AnnouncementDetails { get; set; }
        AnnouncementContract ViewContract { get; }

    }
}
