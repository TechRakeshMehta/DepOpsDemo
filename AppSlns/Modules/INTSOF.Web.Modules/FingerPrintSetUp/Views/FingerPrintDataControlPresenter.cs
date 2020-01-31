using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.UI.Contract.FingerPrintSetup;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
namespace CoreWeb.FingerPrintSetUp.Views
{
    public class FingerPrintDataControlPresenter : Presenter<IFingerPrintDataControlView>
    {
        public override void OnViewInitialized()
        {

        }

        public void GetLocationAvailable(String lng, String lat, String orderRequestType,Int32 OrderID = 0)
        {
            if (View.IsApplicant)
            {
                View.lstLocations = new List<LocationContract>();
               
                if (!View.TenantId.IsNullOrEmpty() && !String.IsNullOrEmpty(lng) && !String.IsNullOrEmpty(lat))
                {
                    //Removed check - Location should be filtered based on additional services
                    //if (OrderID > 0)
                    //{
                    //    View.lstLocations = FingerPrintSetUpManager.GetLocationForRescheduling(View.TenantId, OrderID, lng, lat).Take(20).ToList();
                    //}
                    //else
                    //{
                    if (!orderRequestType.IsNullOrEmpty() && orderRequestType == OrderRequestType.CompleteOrderByApplicant.GetStringValue())
                    {
                        View.lstLocations = FingerPrintSetUpManager.GetApplicantAvailableLocation(View.TenantId, lng, lat, orderRequestType, OrderID).Take(20).ToList();
                    }
                    else {
                        View.lstLocations = FingerPrintSetUpManager.GetApplicantAvailableLocation(View.TenantId, lng, lat, orderRequestType).Take(20).ToList();
                    }
                    
                    //}
                }
            }
        }

        #region Order Flow

        public void GetValidateEventCodeStatusAndEventDetails(FingerPrintAppointmentContract locationId)
        {
            
                View.lstLocations = new List<LocationContract>();
                if (!locationId.IsNullOrEmpty() )
                {
                    
                    View.lstLocations = FingerPrintSetUpManager.GetValidateEventCodeStatusAndEventDetails(locationId, View.TenantId).ToList();
                }
            
        }

        
       public List<BackgroundServiceContract> GetOrderBackgroundServices(Int32 OrderID, Int32 TenantId)
       {
            return FingerPrintDataManager.GetOrderBackgroundServices(OrderID, TenantId);
       }

        public bool AdditionalServicesNotshipped(Int32 OrderID, Int32 TenantId)
        {
            return FingerPrintDataManager.AdditionalServicesNotshipped(OrderID, TenantId);
        }

        public bool AdditionalServicesExist(Int32 OrderID, Int32 TenantId)
        {
            return FingerPrintDataManager.AdditionalServicesExist(OrderID, TenantId);
        }

        public LocationContract GetLocationByOrderId(Int32 OrderID, Int32 TenantId)
        {
            return FingerPrintDataManager.GetLocationByOrderId(OrderID, TenantId);
        }

        public LocationContract GetLocationByLocationid(Int32 OrderID, Int32 TenantId)
        {
            return FingerPrintDataManager.GetLocationByOrderId(OrderID, TenantId);
        }

        public void GetLocationDataForOutOfState(int locationId)
        {

            View.lstLocations = new List<LocationContract>();
            if (!locationId.IsNullOrEmpty())
            {

                View.lstLocations = FingerPrintSetUpManager.GetLocationDataForOutOfState(locationId, View.TenantId).ToList();
            }

        }

        public int GetLocationIDForOutOfStateAppointment()
        {
            Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.LOCATION_ID_FOR_OUT_OF_STATE_APPOINTMENT);
            return Convert.ToInt32(appConfiguration.AC_Value);
        }

        #endregion

        public CBIBillingStatu GetCBIBillingStatusData()
        {
            return FingerPrintDataManager.GetCBIBillingStatusData(View.TenantId, View.FingerprintData.CBIUniqueID, View.FingerprintData.BillingCode);
        }

        public String GetBillingCodeByOrderID(Int32 orderID)
        {
            Entity.ClientEntity.OrderBillingCodeMapping orderBillingCode = FingerPrintDataManager.GetOrderBillingCode(View.TenantId, orderID);
            if (!orderBillingCode.IsNullOrEmpty())
            {
                return orderBillingCode.OBCM_BillingCode;
            }
            else
            {
                return String.Empty;
            }
        }

        public ReserveSlotContract ReserveSlot(Int32 ReservedSlotID, Int32 SelectedSlotID)
        {
            if(SelectedSlotID <= 0)
            {
                return null;
            }

            return FingerPrintSetUpManager.ReserveSlot(ReservedSlotID, SelectedSlotID, View.CurrentLoggedInUserID);
        }
    }
}
