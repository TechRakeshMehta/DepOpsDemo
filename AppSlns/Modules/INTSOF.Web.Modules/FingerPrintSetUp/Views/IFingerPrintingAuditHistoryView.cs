using Entity;
using INTSOF.UI.Contract.FingerPrintSetup;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.FingerPrintSetUp.Views
{
    public interface IFingerPrintingAuditHistoryView
    {
        Int32 TenantId { get; set; }
        Int32 CurrentLoggedInUserID { get; }
        List<Tenant> lstTenant { get; set; }
        Int32 SelectedTenantID { get; set; }
        String TenantIDs { get; set; }
        List<LocationContract> lstAvailableLocations { get; set; }
        String LocationIDs { get; set; }
        Int32 PageSize { get; set; }
        Int32 CurrentPageIndex { get; set; }
        Int32 VirtualRecordCount { get; set; }
        CustomPagingArgsContract GridCustomPaging { get; set; } 
        List<LocationServiceAppointmentAuditContract> lstAppAuditHistory { get; set; }
        LocationServiceAppointmentAuditContract filterContract { get; set; }
    }
}
