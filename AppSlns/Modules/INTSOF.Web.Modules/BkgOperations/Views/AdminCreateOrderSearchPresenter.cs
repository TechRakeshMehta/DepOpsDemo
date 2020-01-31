using Business.RepoManagers;
using ExternalVendors.ClearStarVendor;
using INTSOF.Contracts;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CoreWeb.BkgOperations.Views
{
    public class AdminCreateOrderSearchPresenter : Presenter<IAdminCreateOrderSearchView>
    {
        public override void OnViewInitialized()
        {
            GetTenants();
        }

        /// <summary>
        /// Get the tenants to bind.
        /// </summary>
        public void GetTenants()
        {
            View.lstTenant = ComplianceDataManager.getClientTenant();
        }

        /// <summary>
        /// To check if Tenant is Admin/DefaultTenant
        /// </summary>
        public void IsAdminLoggedIn()
        {
            View.IsAdminLoggedIn = (SecurityManager.DefaultTenantID == View.TenantId);
        }

        /// <summary>
        /// Get the Admin orders detail list.
        /// </summary>
        public void GetAdminCreateOrderSearchData()
        {
            AdminCreateOrderContract adminOrderContract = new AdminCreateOrderContract();
            if (View.SelectedTenantId <= AppConsts.NONE)
            {
                View.lstAdminOrders = new List<AdminCreateOrderContract>();
                return;
            }
            View.searchContract = new AdminOrderSearchContract();
            View.searchContract.ClientId = View.TenantId;
            if (View.searchContract.ClientId == SecurityManager.DefaultTenantID && View.SelectedTenantId != AppConsts.NONE)
            {
                View.searchContract.ClientId = View.SelectedTenantId;
            }
            View.searchContract.FirstName = View.FirstName;
            View.searchContract.LastName = View.LastName;
            View.searchContract.OrderNumber = View.OrderNumber;
            View.searchContract.SSN = View.SSN;
            View.searchContract.DOB = View.DOB;
            View.searchContract.ReadyToTransmit = View.ReadyToTransmit;
            View.searchContract.OrderHierarchy = View.TargetHierarchyNodeIds;
            View.searchContract.gridCustomPaging = View.GridCustomPaging;
            View.lstAdminOrders = BackgroundProcessOrderManager.GetAdminCreateOrderSearchData(View.SelectedTenantId, View.searchContract, View.GridCustomPaging);

            if (View.lstAdminOrders.IsNullOrEmpty())
                View.VirtualRecordCount = AppConsts.NONE;
            else
                View.VirtualRecordCount = View.lstAdminOrders.FirstOrDefault().TotalCount;
            //View.GridCustomPaging.VirtualPageCount = View.VirtualRecordCount-View.PageSize*View.CurrentPageIndex;
        }

        /// <summary>
        /// To transmit the list of orders selected.
        /// </summary>
        /// <returns></returns>
        public Boolean TransmmitAdminOrders()
        {
            //List<Int32> OrderIDs = new List<Int32>();
            //OrderIDs.Add(View.OrderID);
            if (!View.SelectedOrderIds.IsNullOrEmpty())
            {
                Boolean response = false;
                response = BackgroundProcessOrderManager.TransmmitAdminOrders(View.SelectedTenantId, View.CurrentLoggedInUserId, View.SelectedOrderIds);
                if (response)
                {
                    UpdateEDSStatus(View.SelectedOrderIds);
                }
                return response;
            }
            else
                return false;
        }

        /// <summary>
        /// To check whether the is ready to transmit or not.
        /// </summary>
        /// <returns></returns>
        public Boolean AdminOrderIsReadyToTransmit()
        {
            String orderIds = String.Join(",", View.SelectedOrderIds);
            return BackgroundProcessOrderManager.AdminOrderIsReadyToTransmit(View.SelectedTenantId, View.CurrentLoggedInUserId, orderIds);
        }

        /// <summary>
        /// Method to update the EDS Related Data For customformdata and also update the external vendor dispatch status.
        /// </summary>
        /// <param name="applicantOrderDataContract">applicantOrderDataContract</param>
        /// <param name="orderId">orderId</param>
        /// <param name="userOrder">userOrder</param>
        public void UpdateEDSStatus(List<Int32> orderIds)
        {
            if (!orderIds.IsNullOrEmpty())
            {
                List<Entity.ClientEntity.Order> orderList = new List<Entity.ClientEntity.Order>();
                orderList = ComplianceDataManager.GetOrdersByIds(View.SelectedTenantId, orderIds);
                if (!orderList.IsNullOrEmpty())
                {
                    foreach (Entity.ClientEntity.Order order in orderList)
                    {
                        Entity.ClientEntity.OrderPaymentDetail _orderPaymentDetailEDS = null;
                        Entity.ClientEntity.OrderPaymentDetail orderPaymentDetail = order.OrderPaymentDetails.FirstOrDefault(x => !x.OPD_IsDeleted);


                        if (!orderPaymentDetail.IsNullOrEmpty() && ComplianceDataManager.IsOrderPaymentIncludeEDSService(View.SelectedTenantId, orderPaymentDetail.OPD_ID))
                        {
                            _orderPaymentDetailEDS = orderPaymentDetail;
                        }


                        if (!_orderPaymentDetailEDS.IsNullOrEmpty())
                        {
                            String _prevStatus = ApplicantOrderStatus.Paid.GetStringValue();
                            Int32 orderStatusId = ComplianceDataManager.GetOrderStatusList(View.SelectedTenantId).Where(orderSts => orderSts.Code.ToLower() == _prevStatus.ToLower() && !orderSts.IsDeleted)
                                         .FirstOrDefault().OrderStatusID;
                            #region E-DRUG SCREENING
                            Entity.ClientEntity.BkgOrder bkgOrderObj = BackgroundProcessOrderManager.GetBkgOrderByOrderID(View.SelectedTenantId, order.OrderID);
                            if (!bkgOrderObj.IsNullOrEmpty())
                            {
                                List<Int32> lstBackgroundPackageId = bkgOrderObj.BkgOrderPackages.Where(x => !x.BOP_IsDeleted).Select(x => x.BkgPackageHierarchyMapping.BPHM_BackgroundPackageID).ToList();
                                String extVendorId = String.Empty;
                                ClearStarCCF objClearstarCCf = new ClearStarCCF();

                                ClearStarWebCCFContract clearStarWebCCFContract = new ClearStarWebCCFContract();
                                var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
                                var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;
                                String result = BackgroundProcessOrderManager.GetClearStarServiceId(View.SelectedTenantId, lstBackgroundPackageId, BkgServiceType.ELECTRONICDRUGSCREEN.GetStringValue());
                                if (!result.IsNullOrEmpty())
                                {
                                    String[] separator = { "," };
                                    String[] splitIds = result.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                                    extVendorId = splitIds[1];
                                }

                                //Update BkgOrderSvcLineItem Status to DisptachOnHold_WaitingForEDSData for background package that contains EDS service.
                                if (!extVendorId.IsNullOrEmpty())
                                {
                                    BackgroundProcessOrderManager.UpdateBkgOrderSvcLineItem(View.SelectedTenantId, Convert.ToInt32(extVendorId), bkgOrderObj.BOR_ID, SvcLineItemDispatchStatus.DISPTACH_ON_HOLD_WAITING_FOR_EDS_DATA.GetStringValue(), order.CreatedByID);
                                }
                                //Update status PSLI_DispatchedExternalVendor from DisptachOnHold_WaitingForEDSData to Dispatched
                                if (_orderPaymentDetailEDS.IsNotNull() && _orderPaymentDetailEDS.OPD_OrderStatusID.IsNotNull() && _orderPaymentDetailEDS.OPD_OrderStatusID == orderStatusId && !extVendorId.IsNullOrEmpty())
                                {
                                    //Create dictionary for parallel task parameter.
                                    Dictionary<String, Object> dicParam = new Dictionary<String, Object>();
                                    dicParam.Add("BkgOrderId", bkgOrderObj.BOR_ID);
                                    dicParam.Add("TenantId", View.SelectedTenantId);
                                    dicParam.Add("ExtVendorId", Convert.ToInt32(extVendorId));
                                    dicParam.Add("BPHMId_List", bkgOrderObj.BkgOrderPackages.Where(x => !x.BOP_IsDeleted).Select(x => x.BOP_BkgPackageHierarchyMappingID).ToList());
                                    dicParam.Add("RegistrationId", String.Empty);
                                    dicParam.Add("CurrentLoggedInUserId", order.CreatedByID);
                                    dicParam.Add("OrganizationUserId", bkgOrderObj.OrganizationUserProfile.OrganizationUserID);
                                    dicParam.Add("OrganizationUserProfileId", bkgOrderObj.BOR_OrganizationUserProfileID);
                                    dicParam.Add("ApplicantName", string.Concat(bkgOrderObj.OrganizationUserProfile.FirstName, " ", bkgOrderObj.OrganizationUserProfile.LastName));
                                    dicParam.Add("PrimaryEmailAddress", bkgOrderObj.OrganizationUserProfile.PrimaryEmailAddress);
                                    //Pass selectedNodeId in place of HierarchyId [UAT-1067]
                                    //dicParam.Add("HierarchyNodeId", bkgOrderObj.Order.HierarchyNodeID.Value);
                                    dicParam.Add("HierarchyNodeId", bkgOrderObj.Order.SelectedNodeID.Value);
                                    BackgroundProcessOrderManager.RunParallelTaskSaveCCFDataAndPDF(objClearstarCCf.SaveCCFDataAndPDF, dicParam, LoggerService, ExceptiomService, View.SelectedTenantId);
                                }
                            }
                            #endregion
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public String CheckOrderAvailabilityForTrasmit()
        {
            String Message = String.Empty;
            if (!View.SelectedOrderIds.IsNullOrEmpty())
            {
                String OrderIds = String.Join(",", View.SelectedOrderIds);
                Dictionary<String, Boolean> result = BackgroundProcessOrderManager.CheckOrderAvailabilityForTrasmit(OrderIds, View.SelectedTenantId);
                

                if (!result.IsNullOrEmpty() && !result.Where(cond => !cond.Value).IsNullOrEmpty())
                {
                    Message = "Order(s) cannot be transmitted because added package(s) is no longer available to following order(s): ";
                    //return "This order can't be modify or trasmit, due to changes in package setup";

                    String orderNumbers = String.Join(",", result.Where(cond => !cond.Value).Select(slct => slct.Key));
                    //foreach (var item in result.Where(cond => !cond.Value).ToList())
                    //{
                    //    Message = Message + ", " + item.Key;
                    //}

                    Message = Message + orderNumbers + ". Please uncheck these orders and try again.";
                }
            }
            return Message;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Boolean DeleteAdminOrderDetails(Int32 orderId)
        {
            return BackgroundProcessOrderManager.DeleteAdminOrderDetails(View.SelectedTenantId, View.CurrentLoggedInUserId, orderId);
        }
    }
}
