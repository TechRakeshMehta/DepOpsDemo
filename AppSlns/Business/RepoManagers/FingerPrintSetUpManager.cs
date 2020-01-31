using INTSOF.UI.Contract.FingerPrintSetup;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;


#region Application Specific
using Entity.ClientEntity;
using Entity.LocationEntity;
using System.Web;
using INTSOF.Services.Observer;
using INTSOF.UI.Contract.Templates;
using Business.Observer;
using System.Configuration;
using Entity;
using INTSOF.ServiceUtil;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.UI.Contract.ComplianceOperation;

#endregion
namespace Business.RepoManagers
{
    public class FingerPrintSetUpManager
    {
        #region Methods

        public static Int32 SaveFingerPrintLocations(Int32 currentLoggedIdUserID, LocationContract locationContract)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().SaveFingerPrintLocations(currentLoggedIdUserID, locationContract);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean DeleteFingerPrintLocations(Int32 currentLoggedIdUserID, Int32 selectedLocationID)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().DeleteFingerPrintLocations(currentLoggedIdUserID, selectedLocationID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<LocationContract> GetFingerprintLocations(Int32 orgUserId, Boolean isEnroller, Boolean getFullAccessOnly = false)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().GetFingerprintLocations(orgUserId, isEnroller, getFullAccessOnly);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static LocationContract GetSelectedLocationDetails(Int32 selectedLocationId)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().GetSelectedLocationDetails(selectedLocationId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<LocationContract> GetLocationAvailable(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().GetLocationAvailable(tenantId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static List<LocationContract> GetApplicantAvailableLocation(Int32 tenantId, string lng, string lat,string orderRequestType=null, Int32 orderID = 0)
        {
            try
            {
                if (!orderRequestType.IsNullOrEmpty() && orderRequestType == OrderRequestType.CompleteOrderByApplicant.GetStringValue())
                {
                    return BALUtils.GetFingerPrintClientRepoInstance(tenantId).GetApplicantAvailableLocation(tenantId, lng, lat, orderRequestType,orderID);
                }
                else {
                    return BALUtils.GetFingerPrintSetupRepoInstance().GetApplicantAvailableLocation(tenantId, lng, lat, orderRequestType);
                }
        
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }





        public static List<LocationContract> GetLocationForRescheduling(Int32 tenantId, Int32 OrderId, string lng, string lat)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(tenantId).GetLocationForRescheduling(OrderId, lng, lat);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #region Order Flow

        public static List<LocationContract> GetValidateEventCodeStatusAndEventDetails(FingerPrintAppointmentContract locationId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().GetValidateEventCodeStatusAndEventDetails(locationId, tenantId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static List<LocationContract> GetLocationDataForOutOfState(int locationId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().GetLocationDataForOutOfState(locationId, tenantId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }



        #endregion


        public static List<LocationContract> GetAssignedFingerprintLocations(string selectedTenantIds, Int32? organizationUserId, CustomPagingArgsContract customPagingArgsContract)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().GetAssignedFingerprintLocations(selectedTenantIds, organizationUserId, customPagingArgsContract);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<AppointmentSlotContract> GetLocationAppointmentSlots(AppointmentSlotContract searchContract, Int32 locationId)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().GetLocationAppointmentSlots(searchContract, locationId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean UpdateAppointmentSlot(AppointmentSlotContract appointmentSlotContract, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().UpdateAppointmentSlot(appointmentSlotContract, currentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<Int32> GetEnrollerMappedWithLocation(Int32 locationId)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().GetEnrollerMappedWithLocation(locationId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<ManageEnrollerMappingContract> GetEnrollerMappings(CustomPagingArgsContract customPagingArgsContract, Int32 locationId)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().GetEnrollerMappings(customPagingArgsContract, locationId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #region Order Flow Manage Timer Off

        public static Boolean SaveAndUpdateTimeOff(ManageScheduleTimeContract manageScheduleTimeContract, int currentLoggedInUserID)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().SaveAndUpdateTimeOff(manageScheduleTimeContract, currentLoggedInUserID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }

        }
        public static Boolean DeleteTimeOff(Int32 currentLoggedIdUserID, ManageScheduleTimeContract manageScheduleTimeContract)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().DeleteTimeOff(manageScheduleTimeContract, currentLoggedIdUserID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static Int32 PublishTimeOff(Int32 currentLoggedIdUserID, ManageScheduleTimeContract manageScheduleTimeContract)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().PublishTimeOff(manageScheduleTimeContract, currentLoggedIdUserID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static List<ManageScheduleTimeContract> GetManageScheduleTimeData(Int32 locationId)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().GetLocationDataForScheduleOff(locationId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        public static Boolean SaveEnrollerMapping(ManageEnrollerMappingContract enrollerPermissionContract, Int32 currentLoggedInUserId)
        {
            try
            {

                return BALUtils.GetFingerPrintSetupRepoInstance().SaveEnrollerMapping(enrollerPermissionContract, currentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean DeleteEnrollerMapping(Int32 currentLoggedInUserID, Int32 selectedEnrollerMappingID)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().DeleteEnrollerMapping(currentLoggedInUserID, selectedEnrollerMappingID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #region ScheduleAppointment SetUP

        public static List<AppointmentContract> GetFingerPrintAppointmentScheduleData(Int32 ScheduleMasterID, Int32 locationID)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().GetFingerPrintAppointmentScheduleData(locationID, ScheduleMasterID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Int32 ScheduleAppointment(Int32 currentLoggedInUserId, AppointmentContract AppointmentContract)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().ScheduleAppointment(AppointmentContract, currentLoggedInUserId);

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Int32 UpdateScheduleAppointment(AppointmentContract appointmentContract, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().UpdateScheduleAppointment(appointmentContract, currentLoggedInUserId);

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Int32 DeleteScheduleAppointment(AppointmentContract appointmentContract, Int32 currentLoggedInUserID)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().DeleteScheduleAppointment(appointmentContract, currentLoggedInUserID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static string CommitScheduleAppointment(Int32 LocationID, Int32 ScheduleMasterID, Int32 NewScheduleMasterID, Int32 CurrentLoggedInUserID)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().CommitScheduleAppointment(LocationID, ScheduleMasterID, NewScheduleMasterID, CurrentLoggedInUserID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static void DiscardScheduleAppointment(Int32 NewScheduleMasterID)
        {
            try
            {
                BALUtils.GetFingerPrintSetupRepoInstance().DiscardScheduleAppointment(NewScheduleMasterID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static void CallDigestionProcess(Int32 NewScheduleMasterID, Int32 CurrentLoggedInUserID, Int32 ScheduleMasterID)
        {
            try
            {
                var LoggerService = (HttpContext.Current.ApplicationInstance as INTSOF.Contracts.IWebApplication).LoggerService;
                var ExceptiomService = (HttpContext.Current.ApplicationInstance as INTSOF.Contracts.IWebApplication).ExceptionService;

                Dictionary<String, Object> param = new Dictionary<String, Object>();
                param.Add("ScheduleMasterID", NewScheduleMasterID);
                param.Add("CurrentLoggedInUserID", CurrentLoggedInUserID);
                param.Add("oldScheduleMasterId", ScheduleMasterID);
                INTSOF.ServiceUtil.ParallelTaskContext.PerformParallelTask(ExecuteDigestionProcess, param, LoggerService, ExceptiomService);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static void ExecuteDigestionProcess(Dictionary<String, Object> param)
        {
            try
            {
                BALUtils.GetFingerPrintSetupRepoInstance().CallDigestionProcedure(param);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static void CallBackgroungDigestionProcedure(Int32 backgroundProcessUserId, String ScheduleMasterId)
        {
            try
            {
                BALUtils.GetFingerPrintSetupRepoInstance().CallBackgroundDigestionProcedure(backgroundProcessUserId, ScheduleMasterId);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static List<Int32> GetLocationMasterSchedules()
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().GetLocationMasterSchedules();
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        public static AppointmentSlotContract GetAppointmentScheduleBySlotID(Int32 SlotId)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().GetAppointmentScheduleBySlotID(SlotId);

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<AppointmentSlotContract> GetAppointmentSlotByDate(Int32 LocationId, Boolean IsApplicant = true)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().GetAppointmentSlotsAvailable(LocationId, IsApplicant);

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #region Event Scheduler UAT-3849
        public static AppointmentSlotContract GetEventAppointmentScheduleBySlotID(Int32 SlotId)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().GetEventAppointmentScheduleBySlotID(SlotId);

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static List<AppointmentSlotContract> GetEventAppointmentSlotsAvailable(Int32 EventSlotId)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().GetEventAppointmentSlotsAvailable(EventSlotId);

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion
        public static List<AppointmentOrderScheduleContract> GetAppointmentOrders(String tenantId, Boolean IsAdminLogin, CustomPagingArgsContract GridCustomPagingArgs, Int32 organizationUserId, AppointmentOrderScheduleContract filterContract)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().GetAppointmentOrders(IsAdminLogin, organizationUserId, filterContract, GridCustomPagingArgs, tenantId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean SaveUpdateApplicantAppointment(AppointmentOrderScheduleContract AppOrdSchdContract, AppointmentSlotContract scheduleAppointmentContract, Int32 currentLoggedInUserId)
        {
            try
            {
                Int32 oldStatusID = 0;
                if (BALUtils.GetFingerPrintSetupRepoInstance().SaveUpdateApplicantAppointment(scheduleAppointmentContract, currentLoggedInUserId, out oldStatusID))
                {
                    SendFingerPrintAppointmentMailNotification(AppOrdSchdContract, scheduleAppointmentContract, true);
                    FingerPrintDataManager.SaveUpdateAppointmentAudit(AppOrdSchdContract, scheduleAppointmentContract, currentLoggedInUserId, oldStatusID);
                    FingerPrintDataManager.RescheduleApplicantAppointmentHistory(Convert.ToInt32(AppOrdSchdContract.TenantID), scheduleAppointmentContract, currentLoggedInUserId);
                    FingerPrintDataManager.ResetApplicantAppointmenBkgOrderStatus(Convert.ToInt32(AppOrdSchdContract.TenantID), scheduleAppointmentContract, currentLoggedInUserId);
                    return true;
                }
                else
                    return false;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean ResetOutOfStateApplicantAppointment(AppointmentSlotContract scheduleAppointmentContract, Int32 currentLoggedInUserId, Int32 tenantId)
        {
            try
            {
                Int32 oldStatusID = 0;
                if (!string.IsNullOrWhiteSpace(scheduleAppointmentContract.OrderNumber))
                {
                    scheduleAppointmentContract.OrderId = BALUtils.GetFingerPrintClientRepoInstance(tenantId).GetOrderIdByOrderNumber(scheduleAppointmentContract.OrderNumber.Trim());
                }

                if (BALUtils.GetFingerPrintSetupRepoInstance().ResetOutOfStateApplicantAppointment(scheduleAppointmentContract, currentLoggedInUserId, out oldStatusID))
                {
                    //SendFingerPrintAppointmentMailNotification(AppOrdSchdContract, scheduleAppointmentContract, true);
                    AppointmentOrderScheduleContract AppOrdSchdContract = new AppointmentOrderScheduleContract();
                    scheduleAppointmentContract.IsOnsiteAppointment = false;
                    AppOrdSchdContract.IsOnsiteAppointment = scheduleAppointmentContract.IsOnsiteAppointment.Value;
                    AppOrdSchdContract.LocationId = scheduleAppointmentContract.LocationId;
                    AppOrdSchdContract.SlotId = scheduleAppointmentContract.SlotID;
                    AppOrdSchdContract.SlotId = scheduleAppointmentContract.SlotID;
                    AppOrdSchdContract.OrderId = scheduleAppointmentContract.OrderId;
                    AppOrdSchdContract.TenantID = tenantId.ToString();
                    AppOrdSchdContract.ApplicantOrgUserId = scheduleAppointmentContract.ApplicantOrgUserId;

                    FingerPrintDataManager.SaveUpdateAppointmentAudit(AppOrdSchdContract, scheduleAppointmentContract, currentLoggedInUserId, oldStatusID);
                    FingerPrintDataManager.SaveUpdateApplicantAppointmentHistory(tenantId, scheduleAppointmentContract, currentLoggedInUserId);
                    FingerPrintDataManager.ResetApplicantAppointmenBkgOrderStatus(tenantId, scheduleAppointmentContract, currentLoggedInUserId);
                    return true;
                }
                else
                    return false;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static ManageEnrollerMappingContract GetEnrollerPermission(Int32 orgUserId, Int32 selectedLocationID)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().GetEnrollerPermission(orgUserId, selectedLocationID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static IQueryable<Entity.LocationEntity.lkpPaymentOption> GetAllPaymentOption()
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().GetAllPaymentOption();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static IQueryable<LocationPaymentOption> GetMappedLocationPaymentOption(Int32 SelectedLocationID)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().GetMappedLoactionPaymentOption(SelectedLocationID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean SaveMappedPaymentOptions(Int32 SelectedLocationID, List<Int32> paymentOptions, Int32 CurrentLoggedInUserID)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().SaveMappedPaymentOptions(SelectedLocationID, paymentOptions, CurrentLoggedInUserID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }

        }
        public static Boolean IsAnyScheduleForLocation(Int32 selectedLocationID)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().IsAnyScheduleForLocation(selectedLocationID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }

        }
        public static AppointmentOrderScheduleContract GetAppointmentOrderDetailData(Int32 UserID, Boolean IsAdmin, String TenantId, Int32 ApplicantAppointmentId)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().GetAppointmentOrderDetailData(UserID, IsAdmin, TenantId, ApplicantAppointmentId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        //public static AppointmentSlotContract GetBkgOrderWithAppointmentData(Int32 tenantId, Int32 OrderId, Int32 ApplicantOrgUserID)
        //{
        //    try
        //    {
        //        return BALUtils.GetFingerPrintSetupRepoInstance().GetBkgOrderWithAppointmentData(tenantId, OrderId, ApplicantOrgUserID);
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //}

        public static Boolean IsBkgOrderWithAppointment(Int32 tenantId, Int32 orderId, Int32 applicantOrgUserID)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().IsBkgOrderWithAppointment(tenantId, orderId, applicantOrgUserID);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }


        #region mailNotification
       public static Boolean SendFingerPrintAppointmentMailNotification(AppointmentOrderScheduleContract AppOrdSchdContract, AppointmentSlotContract scheduleAppointmentContract, Boolean IsUpdateNotify)
        {
            var tenantUrl = WebSiteManager.GetInstitutionUrl(Convert.ToInt32(AppOrdSchdContract.TenantID));
            var tenant = SecurityManager.GetTenant(Convert.ToInt32(AppOrdSchdContract.TenantID));

            String institutionUrl = tenantUrl.IsNullOrEmpty() ? String.Empty : tenantUrl;
            String institutionName = tenant.IsNull() || tenant.TenantName.IsNullOrEmpty() ? String.Empty : tenant.TenantName;

            if (!(institutionUrl.Trim().StartsWith("http://", StringComparison.OrdinalIgnoreCase) || institutionUrl.Trim().StartsWith("https://", StringComparison.OrdinalIgnoreCase)))
            {
                institutionUrl = string.Concat("http://", institutionUrl.Trim());
            }
            var LocationLink = ConfigurationManager.AppSettings["DrivingDirectionUrl"];
            if (!LocationLink.IsNullOrEmpty())
            {
                var FormattedAddress = AppOrdSchdContract.LocationAddress.Replace(" ", "+");
                LocationLink = string.Format(LocationLink, FormattedAddress);
                FingerPrintLocation LocationData = new FingerPrintLocation();
                if (scheduleAppointmentContract.IsLocationUpdate)
                    LocationData = GetLocationById(scheduleAppointmentContract.LocationId);


                var userProfileName = BALUtils.GetFingerPrintClientRepoInstance(Convert.ToInt32(AppOrdSchdContract.TenantID)).GetUserProfileName(AppOrdSchdContract.OrderId);
                ////Create Dictionary for Mail And Message Data

                Dictionary<String, String> queryString = new Dictionary<String, String>();
                var locationid = (LocationData.IsNullOrEmpty() || LocationData.FPL_ID == AppConsts.NONE) ? AppOrdSchdContract.LocationId : LocationData.FPL_ID;
                queryString = new Dictionary<String, String>{
                                                                    { "LocationId", locationid.ToString()},
                                                                    { "uId",Convert.ToString(scheduleAppointmentContract.ApplicantOrgUserId) },
                                                                    { "tId",AppOrdSchdContract.TenantID}
                                                         };
                var locationImageLink = String.Format(institutionUrl + "/LocationImages.aspx?args={0}", queryString.ToEncryptedQueryString());

                Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                dictMailData.Add(EmailFieldConstants.APPLICANT_NAME, userProfileName.IsNullOrEmpty() ? string.Concat(AppOrdSchdContract.FirstName, " ", AppOrdSchdContract.LastName) : userProfileName);
                dictMailData.Add(EmailFieldConstants.Order_Number, AppOrdSchdContract.OrderNumber);
                dictMailData.Add(EmailFieldConstants.AGENCY_NAME, institutionName);
                dictMailData.Add(EmailFieldConstants.FROM_DATE, scheduleAppointmentContract.StartDateTime.HasValue ? scheduleAppointmentContract.StartDateTime.Value.ToString("MM/dd/yyyy hh:mm tt") : null);
                dictMailData.Add(EmailFieldConstants.END_DATE, scheduleAppointmentContract.EndDateTime.HasValue ? scheduleAppointmentContract.EndDateTime.Value.ToString("MM/dd/yyyy hh:mm tt") : null);
                dictMailData.Add(EmailFieldConstants.ITEM_NAME, scheduleAppointmentContract.IsEventType ? scheduleAppointmentContract.EventName : (LocationData.IsNullOrEmpty() || LocationData.FPL_ID == AppConsts.NONE) ? AppOrdSchdContract.LocationName : LocationData.FPL_Name);
                dictMailData.Add(EmailFieldConstants.INSTITUTE_ADDRESS, (LocationData.IsNullOrEmpty() || LocationData.FPL_ID == AppConsts.NONE) ? AppOrdSchdContract.LocationAddress : LocationData.FPL_FullAddress);
                dictMailData.Add(EmailFieldConstants.PACKAGE_NAME, AppOrdSchdContract.PackageName);
                dictMailData.Add(EmailFieldConstants.INSTITUTION_URL, institutionUrl);
                dictMailData.Add(EmailFieldConstants.LOCATION_LINK, LocationLink);
                dictMailData.Add(EmailFieldConstants.TENANT_ID, AppOrdSchdContract.TenantID);
                dictMailData.Add(EmailFieldConstants.RECEIVER_ORGANIZATION_USER_ID, Convert.ToString(scheduleAppointmentContract.ApplicantOrgUserId));
                dictMailData.Add(EmailFieldConstants.LOCATION_DESCRIPTION, scheduleAppointmentContract.IsEventType ? scheduleAppointmentContract.EventDescription : (LocationData.IsNullOrEmpty() || LocationData.FPL_ID == AppConsts.NONE) ? AppOrdSchdContract.LocationDescription : LocationData.FPL_Description);
                dictMailData.Add(EmailFieldConstants.LOCATION_IMAGE_LINK, locationImageLink);
                Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
                mockData.UserName = string.Concat(AppOrdSchdContract.FirstName, " ", AppOrdSchdContract.LastName);
                mockData.EmailID = AppOrdSchdContract.ApplicantEmail;
                mockData.ReceiverOrganizationUserID = AppOrdSchdContract.ApplicantOrgUserId > AppConsts.NONE ? AppOrdSchdContract.ApplicantOrgUserId : scheduleAppointmentContract.ApplicantOrgUserId;
                CommunicationSubEvents ObjCommunicationSubEvents = new CommunicationSubEvents();               
                    if (!scheduleAppointmentContract.IsEventType && !scheduleAppointmentContract.IsOutOfStateAppointment)
                        ObjCommunicationSubEvents = IsUpdateNotify ? CommunicationSubEvents.NOTIFICATION_FOR_FINGERPRINT_ORDER_APPOINTMENT_CHANGES : CommunicationSubEvents.NOTIFICATION_FOR_FINGERPRINT_ORDER_APPOINTMENT_FIXED;
                    else if (!scheduleAppointmentContract.IsEventType && scheduleAppointmentContract.IsOutOfStateAppointment)
                        ObjCommunicationSubEvents = CommunicationSubEvents.NOTIFICATION_FOR_OUTOFSTATE_APPOINTMENT_FIXED;
                    else
                        ObjCommunicationSubEvents = IsUpdateNotify ? CommunicationSubEvents.NOTIFICATION_FOR_EVENT_APPOINTMENT_CONFIRMATION : CommunicationSubEvents.NOTIFICATION_TO_APPLICANT_FOR_EVENT_INVITATION;              

                //// send mail/message notification
                CommunicationManager.SentMailMessageNotification(ObjCommunicationSubEvents, mockData, dictMailData, scheduleAppointmentContract.ApplicantOrgUserId, Convert.ToInt32(AppOrdSchdContract.TenantID), AppOrdSchdContract.HeirarchyNodeId);

                #region UAT- 4242
                //// send mail/message notification to the enroller
                List<AppointmentOrderScheduleContract> EnrollerList = BALUtils.GetFingerPrintSetupRepoInstance().GetLocationEnrollerList(locationid);
                foreach (var enroller in EnrollerList)
                {
                    // send enroller mail code here
                    Dictionary<String, object> dictEnrollerMailData = new Dictionary<string, object>();
                    dictEnrollerMailData.Add(EmailFieldConstants.APPLICANT_NAME, userProfileName.IsNullOrEmpty() ? string.Concat(AppOrdSchdContract.FirstName, " ", AppOrdSchdContract.LastName) : userProfileName);
                    dictEnrollerMailData.Add(EmailFieldConstants.Order_Number, AppOrdSchdContract.OrderNumber);
                    dictEnrollerMailData.Add(EmailFieldConstants.USER_FULL_NAME, enroller.UserFullName);
                    dictEnrollerMailData.Add(EmailFieldConstants.FROM_DATE, scheduleAppointmentContract.StartDateTime.HasValue ? scheduleAppointmentContract.StartDateTime.Value.ToString("MM/dd/yyyy hh:mm tt") : null);
                    dictEnrollerMailData.Add(EmailFieldConstants.END_DATE, scheduleAppointmentContract.EndDateTime.HasValue ? scheduleAppointmentContract.EndDateTime.Value.ToString("MM/dd/yyyy hh:mm tt") : null);
                    dictEnrollerMailData.Add(EmailFieldConstants.ITEM_NAME, scheduleAppointmentContract.IsEventType ? scheduleAppointmentContract.EventName : (LocationData.IsNullOrEmpty() || LocationData.FPL_ID == AppConsts.NONE) ? AppOrdSchdContract.LocationName : LocationData.FPL_Name);
                    dictEnrollerMailData.Add(EmailFieldConstants.TENANT_ID, AppOrdSchdContract.TenantID);
                    dictEnrollerMailData.Add(EmailFieldConstants.RECEIVER_ORGANIZATION_USER_ID, Convert.ToString(enroller.ApplicantOrgUserId));

                    Entity.CommunicationMockUpData mockEnrollerData = new Entity.CommunicationMockUpData();
                    mockEnrollerData.UserName = string.Concat(enroller.UserFullName);
                    mockEnrollerData.EmailID = enroller.ApplicantEmail;
                    mockEnrollerData.ReceiverOrganizationUserID = enroller.ApplicantOrgUserId;
                    CommunicationSubEvents ObjEnrollerCommunicationSubEvents = new CommunicationSubEvents();
                    ObjEnrollerCommunicationSubEvents = CommunicationSubEvents.NOTIFICATION_FOR_FINGERPRINT_APPOINTMENT_SET_FOR_ENROLLER_SITE;
                    //if (IsUpdateNotify)
                    //{
                    //    if (scheduleAppointmentContract.LocationId != AppOrdSchdContract.LocationId)
                    //    {
                    //        CommunicationManager.SentMailMessageNotification(ObjEnrollerCommunicationSubEvents, mockEnrollerData, dictEnrollerMailData, enroller.ApplicantOrgUserId, Convert.ToInt32(AppOrdSchdContract.TenantID), AppOrdSchdContract.HeirarchyNodeId, true);
                    //    }
                    //}
                    //else
                    //{
                    CommunicationManager.SentMailMessageNotification(ObjEnrollerCommunicationSubEvents, mockEnrollerData, dictEnrollerMailData, enroller.ApplicantOrgUserId, Convert.ToInt32(AppOrdSchdContract.TenantID), AppOrdSchdContract.HeirarchyNodeId, true);
                    //}
                }

                #endregion


                return true;
            }
            return false;
        }
        #endregion
        public static Boolean SendOrderCreateMail(AppointmentOrderScheduleContract AppOrdSchdContract, AppointmentSlotContract scheduleAppointmentContract)
        {
            try
            {
                return SendFingerPrintAppointmentMailNotification(AppOrdSchdContract, scheduleAppointmentContract, false);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static ReserveSlotContract ReserveSlot(Int32 reservedSlotID, Int32 selectedSlotID, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().ReserveSlot(reservedSlotID, selectedSlotID, currentLoggedInUserId);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean IsPrinterAvailableNewLoc(Int32 LocationId)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().IsPrinterAvailableNewLoc(LocationId);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean IsPrinterAvailableAtOldLoc(Int32 OrderId,Int32 tenantId)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(tenantId).IsPrinterAvailableAtOldLoc(OrderId);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        

        public static AppointmentSlotContract ReserveSlotForEventCodeType(Int32 reservedSlotID, Int32 selectedSlotID, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().ReserveSlotForEventCodeType(reservedSlotID, selectedSlotID, currentLoggedInUserId);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<FingerprintLocationGroupContract> GetLocationGroupList(CustomPagingArgsContract GridCustomPagingArgs, FingerprintLocationGroupContract filterContract)
        {
            try
            {
                //return new List<FingerprintLocationGroupContract>();
                return BALUtils.GetFingerPrintSetupRepoInstance().GetLocationGroupList(GridCustomPagingArgs, filterContract);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static Boolean SaveUpdateLocationGroup(FingerprintLocationGroupContract LocationgroupData, Int32 organizationUserId)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().SaveUpdateLocationGroup(LocationgroupData, organizationUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static Boolean DeleteLocationGroup(Int32 LocationGroupID, Int32 OrganizationUserID)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().DeleteLocationGroup(LocationGroupID, OrganizationUserID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static List<FingerprintLocationGroupContract> GetLocationGroupCompleteList()
        {
            try
            {
                //return new List<FingerprintLocationGroupContract>();
                return BALUtils.GetFingerPrintSetupRepoInstance().GetLocationGroupCompleteList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static Boolean SaveLocationImages(List<FingerPrintLocationImagesContract> AddedLocationImagesContract, Int32 LocationId, Int32 CurrentUserId)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().AddLocationImages(AddedLocationImagesContract, LocationId, CurrentUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static List<FingerPrintLocationImagesContract> GetLocationImages(CustomPagingArgsContract CustomGridArg, Int32 LocationId)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().GetLocationImages(CustomGridArg, LocationId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static Boolean DeleteLocationImage(Int32 ImageId, Int32 CurrentUserId)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().DeleteLocationImage(ImageId, CurrentUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #region UAT-3734
        public static List<AppointmentOrderScheduleContract> GetApplicantsMissedAppointments(String LocationTenantIds, Int32 chunkSize, Boolean IsMissedNotify)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().GetApplicantsMissedAppointments(LocationTenantIds, chunkSize, IsMissedNotify);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static Boolean UpdateAppointmentStatus(AppointmentOrderScheduleContract AppointmentData, String StatusCode, Int32 BackgroundProcessId, Int32 TenantID)
        {
            try
            {
                Int32 OldStatusId = 0;
                if (BALUtils.GetFingerPrintSetupRepoInstance().UpdateAppointmentStatus(AppointmentData.ApplicantAppointmentId, StatusCode, BackgroundProcessId, out OldStatusId))
                {
                    BALUtils.GetFingerPrintClientRepoInstance(TenantID).SaveUpdateAppointmentStatusAudit(AppointmentData, StatusCode, BackgroundProcessId, OldStatusId);
                    return true;
                }
                return false;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static List<AppointmentOrderScheduleContract> GetApplicantsOffTimeRevokedAppointments(String LocationTenantIds, Int32 chunkSize)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().GetApplicantsOffTimeRevokedAppointments(LocationTenantIds, chunkSize);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion
        #endregion
        public static IQueryable<Entity.LocationEntity.lkpAppointmentStatu> GetAllAppointmentStatus()
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().GetAllAppointmentStatus();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }


        public static List<LocationServiceAppointmentAuditContract> GetAuditHistoryList(String tenantIds, CustomPagingArgsContract GridCustomPagingArgs, Int32 organizationUserId, LocationServiceAppointmentAuditContract filterContract)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().GetAuditHistoryList(tenantIds, GridCustomPagingArgs, organizationUserId, filterContract);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #region Onsite Events
        public static List<ManageOnsiteEventsContract> GetOnsiteEvents(CustomPagingArgsContract customPagingArgsContract, Int32 locationId)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().GetOnsiteEvents(customPagingArgsContract, locationId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static int SaveOnsiteEvent(ManageOnsiteEventsContract onsiteEventContract, Int32 currentLoggedInUserId)
        {
            try
            {

                return BALUtils.GetFingerPrintSetupRepoInstance().SaveOnsiteEvent(onsiteEventContract, currentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static Boolean DeleteOnsiteEvent(Int32 eventId, Int32 locationId, Int32 currentLoggedInUserId)
        {
            try
            {

                return BALUtils.GetFingerPrintSetupRepoInstance().DeleteOnsiteEvent(eventId, locationId, currentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static ManageOnsiteEventsContract GetSelectedEventDetails(Int32 eventId)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().GetSelectedEventDetails(eventId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static List<FingerPrintEventSlotContract> GetEventSlots(CustomPagingArgsContract CustomGridArg, Int32 eventId)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().GetEventSlots(CustomGridArg, eventId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static bool SaveOnsiteEventSlot(FingerPrintEventSlotContract eventSlotContract, Int32 currentLoggedInUserId)
        {
            try
            {

                return BALUtils.GetFingerPrintSetupRepoInstance().SaveOnsiteEventSlot(eventSlotContract, currentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static bool DeleteOnsiteEventSlot(int eventSlotId, Int32 currentLoggedInUserId)
        {
            try
            {

                return BALUtils.GetFingerPrintSetupRepoInstance().DeleteOnsiteEventSlot(eventSlotId, currentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }


        public static bool PublishEvent(int eventId, Int32 currentLoggedInUserId)
        {
            try
            {

                return BALUtils.GetFingerPrintSetupRepoInstance().PublishEvent(eventId, currentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static bool IsLocationMapped(int locationId)
        {
            try
            {

                return BALUtils.GetFingerPrintSetupRepoInstance().IsLocationMapped(locationId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        public static FingerPrintLocation GetLocationById(Int32 locationId)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().GetLocationById(locationId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #region LanguageTranslation
        public static List<LanguageTranslateContract> GetLanguageTranslationData(Int32 tenantId, CustomPagingArgsContract GridCustomPagingArgs, Int32 organizationUserId, LanguageTranslateContract filterContract)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(tenantId).GetLanguageTranslationData(GridCustomPagingArgs, organizationUserId, filterContract);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static bool SaveLanguageTranslateDetails(LanguageTranslateContract languageTranslateContract, Int32 currentLoggedInUserId, Int32 tenantId)
        {
            try
            {

                return BALUtils.GetFingerPrintClientRepoInstance(tenantId).SaveLanguageTranslateDetails(languageTranslateContract, currentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<SystemSpecificLanguageText> GetSystemSpecificTranslatedText(Int32 tenantId)
        {
            try
            {
                string bkgSvcAttrCode = LanguageTranslationEntityType.BkgSvcAttribute.GetStringValue();
                var _LanguageTranslationEntityType = LookupManager.GetLookUpData<lkpLanguageTranslationEntityType>(tenantId);

                if (_LanguageTranslationEntityType.Any(cond => cond.EntityTypeCode.Trim() == bkgSvcAttrCode.Trim()))
                {
                    var bkgSvcAttrEntityTypeID = _LanguageTranslationEntityType.Where(col => col.EntityTypeCode.Trim() == bkgSvcAttrCode.Trim()).FirstOrDefault().EntityTypeID;
                    var _lstSystemTransaltedText = BALUtils.GetFingerPrintClientRepoInstance(tenantId).GetSystemSpecificTranslatedText(bkgSvcAttrEntityTypeID);
                    return _lstSystemTransaltedText;
                }
                return null;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        public static String getUserPreferLangCode(Int32 userId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetUserPreferLanguageCode(userId, tenantId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static FingerPrintApplicantLocationImageContract GetImagesWithLocationId(Int32 LocationId)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().GetImagesWithLocationId(LocationId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean ChangeAppointmentStatus(Int32 orderID, Int32 currentLoggedInUserID, Boolean IsDeleted,Int32 reservedSlotId,Boolean reservedSlotNewStatus)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().ChangeAppointmentStatus(orderID, currentLoggedInUserID, IsDeleted, reservedSlotId, reservedSlotNewStatus);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #region UAT - 4230
        public static Boolean RevokeSelectedAppointments(List<AppointmentOrderScheduleContract> manualRevokedAppointmentList, Int32 CurrentLoggedInUserId)
        {
            Boolean result = false;
            try
            {
                var orderIds = String.Join(",", manualRevokedAppointmentList.Select(ma => ma.OrderId).ToList());
                if (BALUtils.GetFingerPrintSetupRepoInstance().UpdateManualRevokedAppointment(orderIds, FingerPrintAppointmentStatus.REVOKED.GetStringValue(), CurrentLoggedInUserId, true))
                {
                    result = SendManualRevokedMail(manualRevokedAppointmentList, CurrentLoggedInUserId);
                }
                return result;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean SendManualRevokedMail(List<AppointmentOrderScheduleContract> ManualRevokedAppointmentList, Int32 CurrentLoggedInUserId)
        {
            Boolean result = false;
            List<int> lstOrders = new List<int>();
            foreach (var OffTimeRevokedAppointment in ManualRevokedAppointmentList)
            {
                String applicationUrl = WebSiteManager.GetInstitutionUrl(Convert.ToInt32(OffTimeRevokedAppointment.TenantID));
                var tenant = SecurityManager.GetTenant(Convert.ToInt32(OffTimeRevokedAppointment.TenantID));
                String institutionUrl = applicationUrl.IsNullOrEmpty() ? String.Empty : applicationUrl;
                String institutionName = tenant.IsNull() || tenant.TenantName.IsNullOrEmpty() ? String.Empty : tenant.TenantName;
                if (!(institutionUrl.Trim().StartsWith("http://", StringComparison.OrdinalIgnoreCase) || institutionUrl.Trim().StartsWith("https://", StringComparison.OrdinalIgnoreCase)))
                {
                    institutionUrl = string.Concat("http://", institutionUrl.Trim());
                }

                var FormattedAddress = OffTimeRevokedAppointment.LocationAddress.Replace(" ", "+");
                var LocationLink = ConfigurationManager.AppSettings["DrivingDirectionUrl"];
                LocationLink = LocationLink.IsNullOrEmpty() ? null : string.Format(LocationLink, FormattedAddress);
                ////Create Dictionary for Mail And Message Data
                Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                dictMailData.Add(EmailFieldConstants.APPLICANT_NAME, string.Concat(OffTimeRevokedAppointment.FirstName, " ", OffTimeRevokedAppointment.LastName));
                dictMailData.Add(EmailFieldConstants.Order_Number, OffTimeRevokedAppointment.OrderNumber);
                dictMailData.Add(EmailFieldConstants.AGENCY_NAME, institutionName);
                dictMailData.Add(EmailFieldConstants.FROM_DATE, OffTimeRevokedAppointment.StartDateTime.HasValue ? OffTimeRevokedAppointment.StartDateTime.Value.ToString("MM/dd/yyyy hh:mm tt") : null);
                dictMailData.Add(EmailFieldConstants.END_DATE, OffTimeRevokedAppointment.EndDateTime.HasValue ? OffTimeRevokedAppointment.EndDateTime.Value.ToString("MM/dd/yyyy hh:mm tt") : null);
                dictMailData.Add(EmailFieldConstants.ITEM_NAME, OffTimeRevokedAppointment.LocationName);
                dictMailData.Add(EmailFieldConstants.INSTITUTE_ADDRESS, OffTimeRevokedAppointment.LocationAddress);
                dictMailData.Add(EmailFieldConstants.PACKAGE_NAME, OffTimeRevokedAppointment.PackageName);
                dictMailData.Add(EmailFieldConstants.INSTITUTION_URL, institutionUrl);
                dictMailData.Add(EmailFieldConstants.LOCATION_DESCRIPTION, OffTimeRevokedAppointment.LocationDescription);
                dictMailData.Add(EmailFieldConstants.LOCATION_LINK, LocationLink);
                dictMailData.Add(EmailFieldConstants.TENANT_ID, OffTimeRevokedAppointment.TenantID);
                dictMailData.Add(EmailFieldConstants.RECEIVER_ORGANIZATION_USER_ID, Convert.ToString(OffTimeRevokedAppointment.ApplicantOrgUserId));
                Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
                mockData.UserName = string.Concat(OffTimeRevokedAppointment.FirstName, " ", OffTimeRevokedAppointment.LastName);
                mockData.EmailID = OffTimeRevokedAppointment.ApplicantEmail;
                mockData.ReceiverOrganizationUserID = OffTimeRevokedAppointment.ApplicantOrgUserId > AppConsts.NONE ? OffTimeRevokedAppointment.ApplicantOrgUserId : OffTimeRevokedAppointment.ApplicantOrgUserId;
                var CommSubEvnt = CommunicationSubEvents.NOTIFICATION_TO_APPLICANT_FOR_TEMPORARY_CLOSURE_OF_FINGERPRINT_SITE;

                //// send mail/message notification
                var notificationId = CommunicationManager.SentMailMessageNotification(CommSubEvnt, mockData, dictMailData, OffTimeRevokedAppointment.ApplicantOrgUserId, Convert.ToInt32(OffTimeRevokedAppointment.TenantID), OffTimeRevokedAppointment.HeirarchyNodeId);
                if (notificationId > AppConsts.NONE)
                {
                    lstOrders.Add(OffTimeRevokedAppointment.OrderId);
                }
            }

            if (lstOrders.Any())
            {
                result = BALUtils.GetFingerPrintSetupRepoInstance().UpdateManualRevokedAppointment(String.Join(",", lstOrders), FingerPrintAppointmentStatus.REVOKED_AND_NOTIFIED.GetStringValue(), CurrentLoggedInUserId, false);
            }
            return result;
        }

        public static ScheduleInformationContract GetLastBookedAppointmentDate(ScheduleInformationContract scheduleInformationContract)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().GetLastBookedAppointmentDate(scheduleInformationContract);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean IsAppointmentBookedForTheSelectedDate(DateTime AppointmentDate, Int32 SchedlueID)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().IsAppointmentBookedForTheSelectedDate(AppointmentDate, SchedlueID);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        public static List<LookupContract> GetAllFingerPrintAppointmentStatus(Int32 tenantID)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().GetAllFingerPrintAppointmentStatus(tenantID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Int32 SaveFingerPrintLocationTimeFrame(Int32 currentLoggedIdUserID, Int32 locationID, Int32? TimeFrame)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().SaveFingerPrintLocationTimeFrame(currentLoggedIdUserID, locationID, TimeFrame);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #region UAT - 4025

        public static Dictionary<Int32, String> GetCABSUsers()
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().GetCABSUsers();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<String> GetPermittedCBIId(Int32 CurrentLoggedinUserId)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().GetPermittedCBIId(CurrentLoggedinUserId);

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static List<AppointmentOrderScheduleContract> GetHrAdminAppointmentOrders(String tenantId, CustomPagingArgsContract GridCustomPagingArgs, Int32 organizationUserId, AppointmentOrderScheduleContract filterContract, Boolean IsHrAdminEnroller)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().GetHrAdminAppointmentOrders(organizationUserId, filterContract, GridCustomPagingArgs, tenantId, IsHrAdminEnroller);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<HrAdminPermissionContract> GetAllHrAdmins(CustomPagingArgsContract GridCustomPagingArgs, HrAdminPermissionContract filterContract)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().GetAllHrAdmins(filterContract, GridCustomPagingArgs);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean SaveHrAdminPermission(HrAdminPermissionContract PermissionContract, Int32 currentLoggedInUserId)
        {
            try
            {

                return BALUtils.GetFingerPrintSetupRepoInstance().SaveHrAdminPermission(PermissionContract, currentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean DeleteHrAdminPermission(Int32 currentLoggedInUserID, Int32 selectedUserID)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().DeleteHrAdminPermission(currentLoggedInUserID, selectedUserID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<string> FilterCBIUniqueIdsToSave(Int32 SelectedOrganizationUserID, List<String> CBIUniqueIds)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().FilterCBIUniqueIdsToSave(SelectedOrganizationUserID, CBIUniqueIds);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static bool AssignCBIUniqueIds(Int32 currentLoggedInUserID, Int32 SelectedOrganizationUserID, List<String> CBIUniqueIds)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().AssignCBIUniqueIds(currentLoggedInUserID, SelectedOrganizationUserID, CBIUniqueIds);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<string> FilterAccountNamesToSave(Int32 SelectedOrganizationUserID, List<String> lstAccountName)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().FilterAccountNamesToSave(SelectedOrganizationUserID, lstAccountName);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static bool AssignAccountNames(Int32 currentLoggedInUserID, Int32 SelectedOrganizationUserID, List<String> lstAccountName)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().AssignAccountNames(currentLoggedInUserID, SelectedOrganizationUserID, lstAccountName);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static bool UnAssignHRAdminPermission(Int32 currentLoggedInUserID, Int32 permissionID)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().UnAssignHRAdminPermission(currentLoggedInUserID, permissionID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<Entity.LocationEntity.UserCABSPermissionMapping> GetHRAdminPermissions(Int32 OrgUserID)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().GetHRAdminPermissions(OrgUserID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion
        //UAT-4360
        public static void SaveFingerPrintOrderKeyData(Int32 TenantID,  Int32 currentLoggedIdUserID, List<LookupContract> lstLookupContract, Int32 orderId)
        {
            try
            {
                BALUtils.GetFingerPrintSetupRepoInstance().SaveFingerPrintOrderKeyData(TenantID,currentLoggedIdUserID, lstLookupContract, orderId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static FingerPrintOrderKeyDataContract GetFingerPrintOrderKeydata(Int32 OrderID)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().GetFingerPrintOrderKeydata(OrderID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static bool IsReservedSlotExpired(Int32 reservedSlotId, Int32 currentLoggedIdUserID,bool IsCCPayment)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().IsReservedSlotExpired(reservedSlotId, currentLoggedIdUserID, IsCCPayment);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }



        public static List<AppointmentOrderScheduleContract> GetOrderFulFillment(String tenantId, Boolean IsAdminLogin, CustomPagingArgsContract GridCustomPagingArgs, Int32 organizationUserId, AppointmentOrderScheduleContract filterContract)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().GetOrderFulFillment(IsAdminLogin, organizationUserId, filterContract, GridCustomPagingArgs, tenantId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }       
    }
}