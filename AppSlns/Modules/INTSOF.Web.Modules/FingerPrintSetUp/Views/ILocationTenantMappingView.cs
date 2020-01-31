using INTSOF.UI.Contract.FingerPrintSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.FingerPrintSetUp.Views
{
  public interface ILocationTenantMappingView
    {

      List<LocationContract> lstMappedLocations { get; set; }
      Int32 TenantId { get; set; }
      List<LocationContract> lstAvailableLocations { get; set; }
      Int32 CurrentLoggedInUserID { get; }
      Int32 SelectedLocationID { get; set; }
      Int32 SelectedDPMID { get; set; }
      List<LocationContract> DPMMappedLocations { get; set; }
    }
}
