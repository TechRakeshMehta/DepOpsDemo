using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.RotationPackages.Views
{
    public interface IBulkPackageCopy
    {
        IBulkPackageCopy CurrentViewContext { get; }
        //Int32 CurrentLoggedInUserId { get; set; }
        //String ReqRotPackageIDs { get; set; }
        //Int32 TenantID { get; set; }
        DateTime RotationEffectiveStartDate { get; set; }

    }
}
