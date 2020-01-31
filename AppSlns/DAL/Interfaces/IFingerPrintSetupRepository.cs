using INTSOF.UI.Contract.FingerPrintSetup;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;
using Entity.LocationEntity;
using INTSOF.UI.Contract.ComplianceOperation;
using System.Data;

namespace DAL.Interfaces
{
    public interface IFingerPrintSetupRepository
    {
        #region UAT-3484

        Boolean SaveEnrollerMapping(ManageEnrollerMappingContract enrollerPermissionContract, Int32 currentLoggedInUserId);
        Boolean DeleteEnrollerMapping(Int32 currentLoggedInUserID, Int32 tenantUserMappingId);
        List<Int32> GetEnrollerMappedWithLocation(Int32 locationId);
        List<ManageEnrollerMappingContract> GetEnrollerMappings(CustomPagingArgsContract customPagingArgsContract, Int32 locationId);
        //List<OrganizationUser> GetOganisationUsersByTanentId(Int32 tenantId);
        Int32 SaveFingerPrintLocations(Int32 currentLoggedIdUserID, LocationContract locationContract);
        Boolean DeleteFingerPrintLocations(Int32 currentLoggedIdUserID, Int32 selectedLocationID);
        List<LocationContract> GetFingerprintLocations(Int32 orgUserId, Boolean IsEnroller, Boolean getFullAccessOnly = false);
        LocationContract GetSelectedLocationDetails(Int32 selectedLocationId);
        #endregion

        #region Order FLOW
        Int32 PublishTimeOff(ManageScheduleTimeContract manageScheduleTimeContract, int currentLoggedInUserID);
        Boolean SaveAndUpdateTimeOff(ManageScheduleTimeContract manageScheduleTimeContract, int currentLoggedInUserID);
        Boolean DeleteTimeOff(ManageScheduleTimeContract manageScheduleTimeContract, Int32 currentLoggedIdUserID);
        List<ManageScheduleTimeContract> GetLocationDataForScheduleOff(Int32 locationID);
        List<LocationContract> GetValidateEventCodeStatusAndEventDetails(FingerPrintAppointmentContract locationID, Int32 TenantId);
        List<LocationContract> GetLocationDataForOutOfState(int locationId, Int32 tenantId);
        #endregion

        List<LocationContract> GetLocationAvailable(Int32 tenantId);
        Boolean IsPrinterAvailableNewLoc(Int32 locationId);
        List<LocationContract> GetApplicantAvailableLocation(Int32 tenantId, string lng, string lat,string orderRequestType);
        List<AppointmentContract> GetFingerPrintAppointmentScheduleData(Int32 locationID, Int32 ScheduleMasterID);
        List<AppointmentOrderScheduleContract> GetOrderFulFillment(Boolean IsAdminLogin, Int32 organizationUserId, AppointmentOrderScheduleContract filterContract, CustomPagingArgsContract GridCustomPagingArgs, String TenantIds);
        AppointmentSlotContract GetAppointmentScheduleBySlotID(Int32 slotId);
        List<AppointmentSlotContract> GetAppointmentSlotsAvailable(Int32 LocationId, Boolean IsApplicant);
        List<AppointmentOrderScheduleContract> GetAppointmentOrders(Boolean IsAdminLogin, Int32 organizationUserId, AppointmentOrderScheduleContract filterContract, CustomPagingArgsContract GridCustomPagingArgs, String TenantIds);
        Boolean SaveUpdateApplicantAppointment(AppointmentSlotContract scheduleAppointmentContract, Int32 currentLoggedInUserID, out Int32 oldStatusID);
        Boolean ResetOutOfStateApplicantAppointment(AppointmentSlotContract scheduleAppointmentContract, Int32 currentLoggedInUserID, out Int32 oldStatusId);
        IQueryable<LocationPaymentOption> GetMappedLoactionPaymentOption(Int32 SelectedLocationID);
        IQueryable<Entity.LocationEntity.lkpPaymentOption> GetAllPaymentOption();
        Boolean SaveMappedPaymentOptions(Int32 SelectedLocationID, List<Int32> paymentOptions, Int32 CurrentLoggedInUserID);

        ManageEnrollerMappingContract GetEnrollerPermission(Int32 orgUserId, Int32 selectedLocationID);

        List<LocationContract> GetAssignedFingerprintLocations(string selectedTenantIds, Int32? organizationUserId, CustomPagingArgsContract customPagingArgsContract);
        List<AppointmentSlotContract> GetLocationAppointmentSlots(AppointmentSlotContract searchContract, Int32 locationId);
        Boolean UpdateAppointmentSlot(AppointmentSlotContract appointmentSlotContract, Int32 currentLoggedInUserId);
        Boolean IsLocationInUse(Int32 TenantId, List<Int32> lstLocationIds);
        Boolean IsAnyScheduleForLocation(Int32 selectedLocationID);

        #region CBI Result FILE

        Boolean InsertDataCBIResultFile(FingerPrintOrderContract fingerPrintOrderContract, Int32 bkgProcessUserID);
        Boolean UpdateDataCBIResultFile(string PCNNumber, Int32 bkgProcessUserID);


        #endregion
        #region Event Scheduler UAT-3849
        AppointmentSlotContract GetEventAppointmentScheduleBySlotID(Int32 slotId);
        List<AppointmentSlotContract> GetEventAppointmentSlotsAvailable(Int32 EventSlotId);
        #endregion



        #region AppointmentScheduleSetUp

        Int32 ScheduleAppointment(AppointmentContract appointmentContract, Int32 currentLoggedInUserId);
        Int32 UpdateScheduleAppointment(AppointmentContract appointmentContract, Int32 currentLoggedInUserId);
        String CommitScheduleAppointment(Int32 LocationID, Int32 ScheduleMasterID, Int32 NewScheduleMasterID, Int32 CurrentLoggedInUserID);
        void DiscardScheduleAppointment(Int32 NewScheduleMasterID);
        void CallDigestionProcedure(Dictionary<String, Object> param);
        void CallBackgroundDigestionProcedure(Int32 backgroundProcessUserId, String ScheduleMasterId);
        List<Int32> GetLocationMasterSchedules();

        Int32 DeleteScheduleAppointment(AppointmentContract appointmentContract, Int32 CurrentLoggedInUserID);
        AppointmentOrderScheduleContract GetAppointmentOrderDetailData(Int32 UserID, Boolean IsAdmin, String TenantId, Int32 ApplicantAppointmentId);
        AppointmentOrderScheduleContract GetAppointmentFulFillmentQueueOrderDetailData(Int32 UserID, Boolean IsAdmin, String TenantId, Int32 ApplicantAppointmentId);
        
        #endregion

        //AppointmentSlotContract GetBkgOrderWithAppointmentData(Int32 tenantId, Int32 OrderId, Int32 ApplicantOrgUserID);
        Boolean IsBkgOrderWithAppointment(Int32 tenantId, Int32 orderId, Int32 applicantOrgUserID);
        ReserveSlotContract ReserveSlot(Int32 reservedSlotID, Int32 selectedSlotID, Int32 currentLoggedInUserId);

        AppointmentSlotContract ReserveSlotForEventCodeType(Int32 reservedSlotID, Int32 selectedSlotID, Int32 currentLoggedInUserId);

        List<FingerprintLocationGroupContract> GetLocationGroupList(CustomPagingArgsContract GridCustomPagingArgs, FingerprintLocationGroupContract filterContract);
        Boolean SaveUpdateLocationGroup(FingerprintLocationGroupContract LocationGroupData, Int32 organizationUserId);
        Boolean DeleteLocationGroup(Int32 LocationGroupId, Int32 OrganizationUserID);
        List<FingerprintLocationGroupContract> GetLocationGroupCompleteList();

        Boolean AddLocationImages(List<FingerPrintLocationImagesContract> AddedLocationImagesContract, Int32 LocationId, Int32 CurrentUserId);
        List<FingerPrintLocationImagesContract> GetLocationImages(CustomPagingArgsContract CustomGridArg, Int32 LocationId);
        Boolean DeleteLocationImage(Int32 ImageId, Int32 CurrentUserId);

        #region UAT-3734
        List<AppointmentOrderScheduleContract> GetApplicantsMissedAppointments(String LocationTenantIds, Int32 chunkSize, Boolean IsMissedNotify);
        Boolean UpdateAppointmentStatus(Int32 ApplicantAppointmentID, String StatusCode, Int32 BackgroundProcessId, out Int32 OldStatusId);
        List<AppointmentOrderScheduleContract> GetApplicantsOffTimeRevokedAppointments(String LocationTenantIds, Int32 chunkSize);

        #endregion
        IQueryable<Entity.LocationEntity.lkpAppointmentStatu> GetAllAppointmentStatus();
        List<LocationServiceAppointmentAuditContract> GetAuditHistoryList(String tenantIds, CustomPagingArgsContract GridCustomPagingArgs, Int32 organizationUserId, LocationServiceAppointmentAuditContract filterContract);
        #region Onsite Events
        List<ManageOnsiteEventsContract> GetOnsiteEvents(CustomPagingArgsContract customPagingArgsContract, Int32 locationId);
        int SaveOnsiteEvent(ManageOnsiteEventsContract onsiteEventContract, Int32 currentLoggedInUserId);
        Boolean DeleteOnsiteEvent(Int32 eventId, Int32 locationId, Int32 currentLoggedInUserId);
        ManageOnsiteEventsContract GetSelectedEventDetails(Int32 eventId);
        List<FingerPrintEventSlotContract> GetEventSlots(CustomPagingArgsContract CustomGridArg, Int32 eventId);
        bool SaveOnsiteEventSlot(FingerPrintEventSlotContract eventSlotContract, Int32 currentLoggedInUserId);
        bool DeleteOnsiteEventSlot(int eventSlotId, Int32 currentLoggedInUserId);
        bool PublishEvent(int eventId, Int32 currentLoggedInUserId);
        bool IsLocationMapped(int locationId);
        #endregion

        FingerPrintLocation GetLocationById(Int32 Locationid);

        FingerPrintApplicantLocationImageContract GetImagesWithLocationId(Int32 LocationId);
        Boolean ChangeAppointmentStatus(Int32 orderID, Int32 currentLoggedInUserID, Boolean IsDeleted, Int32 reservedSlotId, Boolean reservedSlotNewStatus);
        List<AppointmentOrderScheduleContract> GetLocationEnrollerList(Int32 LocationId);
        Boolean UpdateManualRevokedAppointment(String OrderID, String NewStatusCode, Int32 CurrentLoggedInUserId, Boolean IsManualRevokedStatus);
        ScheduleInformationContract GetLastBookedAppointmentDate(ScheduleInformationContract scheduleInformationContract);
        Boolean IsAppointmentBookedForTheSelectedDate(DateTime AppointmentDate, Int32 ScheduleId);

        List<LookupContract> GetAllFingerPrintAppointmentStatus(Int32 tenantID);
        Int32 SaveFingerPrintLocationTimeFrame(Int32 cuurentLoggedInUserId, Int32 locationID, Int32? TimeFrame);

        #region  UAT - 4025
        Dictionary<Int32, String> GetCABSUsers();
        List<String> GetPermittedCBIId(Int32 CurrentLoggedInUserID);
        List<AppointmentOrderScheduleContract> GetHrAdminAppointmentOrders(Int32 organizationUserId, AppointmentOrderScheduleContract filterContract, CustomPagingArgsContract GridCustomPagingArgs, String TenantIds, Boolean IsHrAdminEnroller);
        List<HrAdminPermissionContract> GetAllHrAdmins(HrAdminPermissionContract filterContract, CustomPagingArgsContract GridCustomPagingArgs);


        Boolean SaveHrAdminPermission(HrAdminPermissionContract PermissionContract, Int32 currentLoggedInUserId);
        Boolean DeleteHrAdminPermission(Int32 currentLoggedInUserID, Int32 UserId);
        #endregion
        List<String> FilterAccountNamesToSave(Int32 selectedOrganizationUserID, List<String> lstAccountName);
        Boolean AssignAccountNames(Int32 currentLoggedInUserID, Int32 selectedOrganizationUserID, List<String> lstAccountName);
        List<String> FilterCBIUniqueIdsToSave(Int32 selectedOrganizationUserID, List<String> lstCBIUniqueId);
        Boolean AssignCBIUniqueIds(Int32 currentLoggedInUserID,Int32 SelectedOrganizationUserID, List<String> lstCBIUniqueId);
        Boolean UnAssignHRAdminPermission(Int32 currentLoggedInUserID, Int32 permissionID);
        List<Entity.LocationEntity.UserCABSPermissionMapping> GetHRAdminPermissions(Int32 OrgUserID);
        bool SaveFingerPrintOrderKeyData(Int32 TenantID,Int32 currentLoggedIdUserID, List<LookupContract> lstLookupContract, Int32 orderId);
        FingerPrintOrderKeyDataContract GetFingerPrintOrderKeydata(Int32 OrderID);
        bool IsReservedSlotExpired(Int32 reservedSlotId, Int32 currentLoggedIdUserID, bool IsCCPayment);
    }

}

