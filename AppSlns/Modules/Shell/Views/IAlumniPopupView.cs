using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.Shell.Views
{
    public interface IAlumniPopupView
    {
        Int32 LoggedInUserID { get; }
        Int32 TenantId { get; set; }
        Int32 OrgUserId { get; set; }
        Int32 AlumniTenantId { get; set; }
        String CurrentSessionId { get; }
        IPersistViewState ViewStateProvider { get; }
    }
}
