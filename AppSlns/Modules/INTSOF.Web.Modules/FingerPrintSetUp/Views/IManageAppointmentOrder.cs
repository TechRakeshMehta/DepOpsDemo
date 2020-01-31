using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.UI.Contract.FingerPrintSetup;
using INTSOF.Utils;
using Entity;
using INTSOF.UI.Contract.ComplianceOperation;

namespace CoreWeb.FingerPrintSetUp.Views
{
    public interface IManageAppointmentOrder
    {
        Int32 TenantId { get; set; }
        Int32 SelectedTenantID { get; set; }
        List<Tenant> lstTenant { get; set; }
        Int32 CurrentLoggedInUserID { get; }
        String CurrentUserId { get; }
        String TenantIDs { get; set; }
        AppointmentOrderScheduleContract filterContract { get; set; }
        Boolean IsAdminScreen { get; set; }
        List<AppointmentOrderScheduleContract> lstAppointmentOrder { get; set; }
        Int32 PageSize { get; set; }
        Int32 CurrentPageIndex { get; set; }
        Int32 VirtualRecordCount { get; set; }
        CustomPagingArgsContract GridCustomPaging { get; set; }

        Int32 DefaultTenantId
        {
            get;
            set;
        }
        List<LocationContract> lstAvailableLocations { get; set; }
        String LocationIDs { get; set; }
        List<LookupContract> lstAppointmentStatus { get; set; }
        string SelectedAppointmentStatusIds { get; set; }
        List<AppointmentOrderScheduleContract> lstAppointmentOrderScheduleContract { get; set; }
        #region UAT - 4025
        Boolean IsHrAdmin { get; set; }
        List<String> lstPermittedCBIUniqueIds { get; set; }
        Boolean IsHrAdminEnroller { get; set; }
        String SelectedCbiUniqueIds { get; set; }
        #endregion
    }
}
