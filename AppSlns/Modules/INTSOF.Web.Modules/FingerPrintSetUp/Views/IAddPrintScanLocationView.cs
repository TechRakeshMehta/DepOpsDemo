using Entity;
using INTSOF.UI.Contract.FingerPrintSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.FingerPrintSetUp.Views
{
    public interface IAddPrintScanLocationView
    {
        Int32 CurrentLoggedInUserID { get; }
       // Int32 SelectedTenantID { get; set; }
        Int32 TenantId { get; set; }
        List<LocationContract> lstFingerprintLocations { get; set; }
        Int32 SelectedLocationID { get; set; }
        List<Tenant> lstTenant { get; set; }
        Boolean IsAdminLoggedIn { get; set; }
        Boolean IsEnroller { get; set; }
    }
}
