using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.FingerPrintSetUp.Views
{
    public interface IFingerPrintLocationGeneralView
    {
        Boolean IsAdminLoggedIn { get; set; }
        Int32 TenantId { get; set; }

    }
}
