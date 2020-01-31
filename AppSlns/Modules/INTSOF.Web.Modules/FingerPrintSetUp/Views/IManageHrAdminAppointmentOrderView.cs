using Entity;
using System;
using INTSOF.UI.Contract.FingerPrintSetup;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.Utils;
namespace CoreWeb.FingerPrintSetUp.Views
{
    public interface IManageHrAdminAppointmentOrderView
    {
            
        String CurrentLoggedInUser_Guid { get; }
        Boolean IsEnroller { get;  }

    }
}
