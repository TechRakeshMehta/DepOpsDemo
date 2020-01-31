using Business.RepoManagers;
using Entity.ClientEntity;
using ExternalVendors.ClearStarVendor;
using INTSOF.Contracts;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoreWeb.Shell.Views
{
    public class RedirectIPNhandlerPresenter : Presenter<IRedirectIPNhandlerView>
    {
        #region Variables

        #region Private Variables



        #endregion

        #region Public Variables



        #endregion

        #endregion

        #region Properties

        #region Private Properties


        #endregion

        #region Public Properties



        #endregion

        #endregion

        #region Methods

        #region Public Methods

        public override void OnViewInitialized()
        {
            View.PaymentIntegrationSettings = SecurityManager.GetPaymentIntegrationSettingsByName("Paypal");
        }

        public Boolean SaveIPNResponse()
        {
            GetIPNPostDataKeyValue();
            return ComplianceDataManager.SaveIPNResponse(View.TenantID, View.IPNTransactionStatus, View.IPNPostData, View.IPNPostDataKeyValue);
        }

        public void CheckAndUpdateOrderByIPN()
        {
            String invoiceNumber = View.IPNPostDataKeyValue["invoice"];
            String amountPaid = View.IPNPostDataKeyValue["mc_gross"];
            String paymentStatus = View.IPNPostDataKeyValue["payment_status"];

            if (paymentStatus == "Completed")
            {
                Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.PAYPAL_IPN_USER_ID);
                Int32 paypalIPNUserID = AppConsts.PAYPAL_IPN_USER_VALUE;

                if (appConfiguration.IsNotNull())
                {
                    paypalIPNUserID = Convert.ToInt32(appConfiguration.AC_Value);
                }
                OnlinePaymentTransaction onlinePaymentTransaction = ComplianceDataManager.GetPaymentTransactionDetails(invoiceNumber, View.TenantID, true);
                foreach (Int32 orderID in View.OrderIDs)
                {
                    Order updatedOrder = ComplianceDataManager.UpdateOrderForOnlinePayment(amountPaid, paypalIPNUserID, orderID, View.TenantID, onlinePaymentTransaction);

                    String paidOrderStatusCode = ApplicantOrderStatus.Paid.GetStringValue();
                    Int32 paidOrderStatusId = ComplianceDataManager.GetOrderStatusList(View.TenantID).FirstOrDefault(cond => cond.Code == paidOrderStatusCode).OrderStatusID;

                    if (ComplianceDataManager.IsOrderPaymentIncludeEDSService(View.TenantID, onlinePaymentTransaction.OrderPaymentDetails.First().OPD_ID))
                    {
                        var _opdEdSPackageType = onlinePaymentTransaction.OrderPaymentDetails.First();

                        if (_opdEdSPackageType.IsNotNull() && _opdEdSPackageType.OPD_OrderStatusID == paidOrderStatusId)
                        {
                            UpdateEDSStatus(orderID);
                        }
                    }

                    //Send Print Scan Notification
                    #region UAT-1358:Complio Notification to applicant for PrintScan
                    String bkgServiceTypeCode = BkgServiceType.PRINT_SCAN.GetStringValue();
                    if (BackgroundProcessOrderManager.IsBkgServiceExistInOrder(View.TenantID, onlinePaymentTransaction.OrderPaymentDetails.First().OPD_ID, bkgServiceTypeCode))
                    {
                        var _opdPrintScanSvcPackageType = onlinePaymentTransaction.OrderPaymentDetails.First();

                        if (_opdPrintScanSvcPackageType.IsNotNull() && _opdPrintScanSvcPackageType.OPD_OrderStatusID == paidOrderStatusId)
                        {
                            CommunicationManager.SendNotificationForPrintScan(_opdPrintScanSvcPackageType, View.TenantID);
                        }
                    }
                    #endregion

                    #region UAT-1560:WB: We should be able to add documents that need to be signed to the order process
                    UpdateAdditionalDocStatus(View.TenantID,orderID);
                    #endregion

                    #region UAT-1759:Create the ability to mark an "Additional Documents" that the students complete in the order flow as "Send to student"
                    ComplianceDataManager.SendAdditionalDocumentsToStudent(View.TenantID, onlinePaymentTransaction.OrderPaymentDetails.First().Order, paypalIPNUserID);
                    #endregion

                    ComplianceDataManager.InsertAutomaticInvitationLog(View.TenantID, orderID, AppConsts.NONE); //UAT-2388

                    //UAT-4498
                    ComplianceDataManager.CopyDataForDummyLineItem(orderID, View.TenantID, paypalIPNUserID);
                }
            }
        }

        #endregion

        #region Private Methods

        private void GetIPNPostDataKeyValue()
        {
            Dictionary<String, String> ipnPostDataKeyValue = new Dictionary<String, String>();
            String[] responseArray = View.IPNPostData.Split('&');

            foreach (var response in responseArray)
            {
                String[] dataArray = response.Split('=');

                if (dataArray.Count() == 2)
                {
                    if (dataArray[0].Equals("invoice"))
                    {
                        ipnPostDataKeyValue.Add(dataArray[0], dataArray[1]);
                        GetOrderAndTenantID(dataArray[1]);
                    }
                    else if (dataArray[0].Equals("mc_gross") || dataArray[0].Equals("payment_status") || dataArray[0].Equals("txn_id") || dataArray[0].Equals("receiver_email"))
                    {
                        ipnPostDataKeyValue.Add(dataArray[0], dataArray[1]);
                    }
                }
            }
            View.IPNPostDataKeyValue = ipnPostDataKeyValue;
        }

        private Dictionary<String, List<Int32>> GetOrderAndTenantID(String invoiceNumber)
        {
            Dictionary<String, List<Int32>> data = ComplianceDataManager.GetOrderAndTenantID(invoiceNumber);
            View.OrderIDs = data["OrderID"];
            View.TenantID = data["TenantID"][0];
            return data;
        }

        #region E-DRUG SCREENING
        /// <summary>
        /// Method to update the EDS Related Data For customformdata and also update the external vendor dispatch status.
        /// </summary>
        /// <param name="applicantOrderDataContract">applicantOrderDataContract</param>
        /// <param name="orderId">orderId</param>
        /// <param name="userOrder">userOrder</param>
        public void UpdateEDSStatus(Int32 orderID)
        {
            BkgOrder bkgOrderObj = BackgroundProcessOrderManager.GetBkgOrderByOrderID(View.TenantID, orderID);
            if (!bkgOrderObj.IsNullOrEmpty())
            {
                List<BkgOrderPackage> lstBkgOrderPackage = BackgroundProcessOrderManager.GetBackgroundPackageIdListByBkgOrderId(View.TenantID, bkgOrderObj.BOR_ID);
                List<Int32> lstBackgroundPackageId = lstBkgOrderPackage.Select(slct => slct.BkgPackageHierarchyMapping.BackgroundPackage.BPA_ID).ToList();
                if (!lstBkgOrderPackage.IsNullOrEmpty() && (lstBkgOrderPackage.Count() > 0))
                {
                    List<Int32> lstBPHM_Id = lstBkgOrderPackage.Select(slt => slt.BOP_BkgPackageHierarchyMappingID.Value).ToList();
                    String extVendorId = String.Empty;
                    ClearStarCCF objClearstarCCf = new ClearStarCCF();

                    ClearStarWebCCFContract clearStarWebCCFContract = new ClearStarWebCCFContract();

                    String result = BackgroundProcessOrderManager.GetClearStarServiceId(View.TenantID, lstBackgroundPackageId, BkgServiceType.ELECTRONICDRUGSCREEN.GetStringValue());
                    if (!result.IsNullOrEmpty())
                    {
                        String[] separator = { "," };
                        String[] splitIds = result.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                        extVendorId = splitIds[1];
                        if (!extVendorId.IsNullOrEmpty())
                        {
                            var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
                            var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;
                            //Create dictionary for parallel task parameter.
                            Dictionary<String, Object> dicParam = new Dictionary<String, Object>();
                            dicParam.Add("BkgOrderId", bkgOrderObj.BOR_ID);
                            dicParam.Add("TenantId", View.TenantID);
                            dicParam.Add("ExtVendorId", Convert.ToInt32(extVendorId));
                            dicParam.Add("BPHMId_List", lstBPHM_Id);
                            dicParam.Add("RegistrationId", String.Empty);
                            dicParam.Add("CurrentLoggedInUserId", SecurityManager.DefaultTenantID);
                            dicParam.Add("OrganizationUserId", bkgOrderObj.OrganizationUserProfile.OrganizationUserID);
                            dicParam.Add("OrganizationUserProfileId", bkgOrderObj.BOR_OrganizationUserProfileID);
                            dicParam.Add("ApplicantName", string.Concat(bkgOrderObj.OrganizationUserProfile.FirstName, " ", bkgOrderObj.OrganizationUserProfile.LastName));
                            dicParam.Add("PrimaryEmailAddress", bkgOrderObj.OrganizationUserProfile.PrimaryEmailAddress);
                            //Pass selectedNodeId in place of HierarchyId [UAT-1067]
                            //dicParam.Add("HierarchyNodeId", bkgOrderObj.Order.HierarchyNodeID.Value);
                            dicParam.Add("HierarchyNodeId", bkgOrderObj.Order.SelectedNodeID.Value);
                            BackgroundProcessOrderManager.RunParallelTaskSaveCCFDataAndPDF(objClearstarCCf.SaveCCFDataAndPDF, dicParam, LoggerService, ExceptiomService, View.TenantID);
                        }
                    }
                }
            }

        }
        #endregion

        #region UAT-1560:WB: We should be able to add documents that need to be signed to the order process
        private void UpdateAdditionalDocStatus(Int32 tenantId, Int32 orderId)
        {
            Int32 orgUserID = AppConsts.NONE;
            Int32 currentLoggedIdUserId = AppConsts.NONE;
            Order updatedOrder = ComplianceDataManager.GetOrderById(tenantId, orderId);
            if (!updatedOrder.IsNullOrEmpty())
            {
                orgUserID = updatedOrder.OrganizationUserProfile.OrganizationUserID;
                currentLoggedIdUserId = updatedOrder.OrganizationUserProfile.OrganizationUserID;
                if (ComplianceDataManager.IsSubscriptionExistForApplicant(orgUserID, tenantId))
                {
                    List<ApplicantDocument> updatedAppDocument = ComplianceDataManager.UpdateAdditionalDocumentStatusForApproveOrder(tenantId, orderId, currentLoggedIdUserId, orgUserID);
                    if (!updatedAppDocument.IsNullOrEmpty())
                    {
                        DocumentManager.CallParallelTaskPdfConversionMergingForAppDoc(updatedAppDocument, tenantId, orgUserID, currentLoggedIdUserId);
                    }
                }
            }
        }
        #endregion

        #endregion

        #endregion

    }
}




