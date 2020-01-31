using System;
using System.Collections.Generic;
using Entity;
using Entity.ClientEntity;
using Entity.ExternalVendorContracts;
using ExternalVendors.Interface;
using ExternalVendors.Utility;
using INTSOF.ServiceUtil;
using INTSOF.Utils;

namespace ExternalVendors.ADBVendor
{
    public class ADBAdapter : IVendorServiceAdapter
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables
        private static String CreateOrderServiceLogger;
        private static String UpdateOrderServiceLogger;
        #endregion

        #endregion

        #region Properties

        #region Public Properties

        public List<State> States
        {
            get;
            set;
        }

        #endregion

        #region Private Properties

        #endregion

        #endregion

        #region Public Methods

        public ADBAdapter()
        {
            CreateOrderServiceLogger = "CreateOrderServiceLogger";
            UpdateOrderServiceLogger = "UpdateOrderServiceLogger";
        }

        EvCreateOrderContract IVendorServiceAdapter.DispatchOrderItemsToVendor(EvCreateOrderContract evOrderContract, Int32 tenantID, Boolean isTestModeON)
        {

            ServiceLogger.Info("Started process of disptaching order items to ADB vendor.", CreateOrderServiceLogger);

            evOrderContract.PackageSvcGroups.ForEach(packageSvcGroup =>
                {
                    try
                    {
                        packageSvcGroup.VendorResponse = CommonHelper.SetVendorResponseInContract(String.Empty,
                                                                "Order for ADB Vendor Dispatch successfully. "
                                                                + evOrderContract.BkgOrderID
                                                                , String.Empty);
                        packageSvcGroup.VendorResponse.IsVendorError = false;
                        packageSvcGroup.BkgOrderVendorProfileID = String.Empty;

                        foreach (var item in packageSvcGroup.OrderItems)
                        {
                            if (item.BkgSvcTypeCode == BkgServiceType.OPERATIONSUPPORTAUTOCOMPLETE.GetStringValue())
                            {
                                item.ExternalVendorOrderID = String.Empty;
                                item.VendorResponse = CommonHelper.SetVendorResponseInContract(String.Empty,
                                                                "Order for ADB Vendor Dispatch successfully for Order Service Line Item: "
                                                                + item.BkgOrderPackageSvcLineItemID
                                                                , String.Empty);
                                item.VendorResponse.IsVendorError = false;
                                item.VendorResponse.IsSpecialError = false;
                                packageSvcGroup.IsTransmitted = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        packageSvcGroup.VendorResponse = CommonHelper.SetVendorResponseInContract(String.Empty,
                                                                     ex.Message, "System Exception");
                        packageSvcGroup.VendorResponse.IsVendorError = true;
                        ServiceLogger.Error(String.Format("An Error has occured in DispatchOrderItemsToVendor method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}",
                                           ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString)
                                           , CreateOrderServiceLogger);
                    }
                });

            ServiceLogger.Info("Ended process of disptaching order items to ADB vendor.", CreateOrderServiceLogger);


            return evOrderContract;
        }

        //EvUpdateOrderContract IVendorServiceAdapter.UpdateVendorBkgOrder(EvUpdateOrderContract evUpdateOrderContract,
        //                                                                IEnumerable<usp_GetOrdersToBeUpdatedVendorData_Result> processedOrderItemList, Int32 tenantID)
        EvUpdateOrderContract IVendorServiceAdapter.UpdateVendorBkgOrder(EvUpdateOrderContract evUpdateOrderContract, Int32 tenantID)
        {
            try
            {
                foreach (EvUpdateOrderItemContract item in evUpdateOrderContract.EvUpdateOrderItemContract)
                {
                    item.DateCompleted = DateTime.Now;
                    item.OrderLineItemResultStatusID = 4;

                    evUpdateOrderContract.VendorResponse = CommonHelper.SetVendorResponseInContract(String.Empty,
                                                            "Order for ADB Vendor Updated successfully for Order Service Line Item: "
                                                            + item.BkgOrderPackageSvcLineItemID
                                                            , String.Empty);
                }

                if (CommonHelper.AreAllProfilesComplete(evUpdateOrderContract, new List<usp_GetOrdersToBeUpdatedVendorData_Result>()))
                {
                    evUpdateOrderContract.OrderNewStatusTypeCode = OrderStatusType.COMPLETED.GetStringValue();
                }

                evUpdateOrderContract.VendorResponse = CommonHelper.SetVendorResponseInContract(String.Empty,
                                                            "Order for ADB Vendor Updated successfully for Order ID: "
                                                            + evUpdateOrderContract.BkgOrderID
                                                            , String.Empty);
                evUpdateOrderContract.VendorResponse.IsVendorError = false;

            }
            catch (Exception ex)
            {
                ServiceLogger.Error(String.Format("An Error has occured in DispatchOrderItemsToVendor method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}",
                                   ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString)
                                   , CreateOrderServiceLogger);
                evUpdateOrderContract.VendorResponse = CommonHelper.SetVendorResponseInContract(String.Empty,
                                                             ex.Message, "System Exception");
                evUpdateOrderContract.VendorResponse.IsVendorError = true;

            }
            return evUpdateOrderContract;
        }

        void IVendorServiceAdapter.RevertExternalVendorChanges(String loginName, String password, Int32 boid, String customerID, String vendorProfileNumber,
           Int32 tenantID, Int32 bkgOrderID, Int32 bkgOrderPackageSvcGroupID)
        {
            //Not Implemented 
        }

        #endregion

        #region Private Methods



        #endregion

    }
}
