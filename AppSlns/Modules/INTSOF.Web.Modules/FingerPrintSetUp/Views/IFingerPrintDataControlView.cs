using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.UI.Contract.FingerPrintSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.FingerPrintSetUp.Views
{
    public interface IFingerPrintDataControlView
    {
        List<LocationContract> lstLocations { get; set; }
        Int32 LocationId { get; set; }
        Boolean IsApplicant { get; }
        Int32 TenantId { get; set; }
        String ApplicantZipCode { get; set; }
        Int32 CurrentLoggedInUserID { get; }
        string LngLat { get; set; }
        Boolean IsPreviousButtonFire { get; set; }
        string EventName { get; set; }
        FingerPrintAppointmentContract FingerprintData { get; set; }
        String LanguageCode { get; }
        string LocationName { get; set; }
        string LocationAddress { get; set; }
        string LocationDescription { get; set; }
        Int32 EventSlotId { get; set; }
        Int32 SlotID { get; set; }
        DateTime SlotDate { get; set; }
        DateTime StartDateTime { get; set; }
        DateTime SlotEndTime { get; set; }
        Boolean IsFromOrderHistoryScreen { get; set; }
        Decimal BillingCodeAmount { get; }
        Boolean IsFingerPrintSvcSelected { get; set; }
        Boolean IsPassportPhotoSvcSelected { get; set; }
        Int32 OrderId { get; set; }
    }
}


