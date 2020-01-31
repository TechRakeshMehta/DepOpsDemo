using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.FingerPrintSetUp.Views
{
    public interface IManageHrOrderFulfillmentQueueView
    {
        String CurrentLoggedInUser_Guid { get; }
        Boolean IsEnroller { get; }
    }
}
