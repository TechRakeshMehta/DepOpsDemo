using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.UI.Contract.FingerPrintSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.FingerPrintSetUp.Views
{
    public interface IAppointmentLocationUpdateView
    {
        List<LocationContract> lstLocations { get; set; }
        Int32 LocationId { get; set; }
        Boolean IsApplicant { get; }
        Int32 TenantId { get; set; }
        String ApplicantZipCode { get; set; }
        Int32 CurrentLoggedInUserID { get; }
        string LngLat { get; set; }
        Int32 OrderID { get; set; }
        FingerPrintAppointmentContract FingerprintData { get; set; }
    }
}


