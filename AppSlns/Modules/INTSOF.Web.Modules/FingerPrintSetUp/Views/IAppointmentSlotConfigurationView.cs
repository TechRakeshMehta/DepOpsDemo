using INTSOF.UI.Contract.FingerPrintSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using INTSOF.Utils;

namespace CoreWeb.FingerPrintSetUp.Views
{
    public interface IAppointmentSlotConfigurationView
    {
        List<LocationContract> FingerprintLocations { get; set; }
        Int32 SelectedLocationID { get; set; }
        List<Tenant> Tenants { get; set; }
        Int32 SelectedTenantID { get; set; }

        Int32 VirtualRecordCount { get; set; }
        Int32 CurrentPageIndex { get; set; }

        Int32 PageSize { get; set; }
        CustomPagingArgsContract GridCustomPaging { get; set; }

        Boolean IsAdminLoggedIn { get; set; }
        int TenantId { get; set; }
        int CurrentLoggedInUserId { get; }
        string SelectedTenantIDs{ get; set; }
    }
}
