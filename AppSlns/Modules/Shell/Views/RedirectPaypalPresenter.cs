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
    public class RedirectPaypalPresenter : Presenter<IRedirectPaypalView>
    {
        #region Variables

        #region Private Variables

        private Int32 _tenantID = 0;
        private List<Int32> _orderIDs;

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

        public void UpdateOnlineTransactionResults()
        {
            GetOrderAndTenantID();
            Int32 paypalPDTUserID = GetPaypalPDTUserID();
            OnlinePaymentTransaction onlinePaymentTransaction = ComplianceDataManager.UpdateOnlineTransactionResults(View.CurrentViewContext.InvoiceNumber,
                View.CurrentViewContext.TransactionDetails, _tenantID, paypalPDTUserID);
            UpdateOrderForOnlinePayment(onlinePaymentTransaction, paypalPDTUserID);
        }

        public void GetURL()
        {
            if (View.InvoiceNumber.IsNotNull())
            {
                View.ApplicationURL = Convert.ToString(ApplicationDataManager.GetObjectDataByKey(View.InvoiceNumber));
            }
        }

        public void RemoveWebApplicationData()
        {
            ApplicationDataManager.RemoveWebApplicationData(View.InvoiceNumber);
        }

        #endregion

        #region Private Methods

        public Dictionary<String, List<Int32>> GetOrderAndTenantID()
        {
            Dictionary<String, List<Int32>> data = ComplianceDataManager.GetOrderAndTenantID(View.CurrentViewContext.InvoiceNumber);
            _orderIDs = data["OrderID"];
            _tenantID = data["TenantID"][0];
            return data;
        }

        private static Int32 GetPaypalPDTUserID()
        {
            Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.PAYPAL_PDT_USER_ID);
            Int32 paypalPDTUserID = AppConsts.PAYPAL_PDT_USER_VALUE;

            if (appConfiguration.IsNotNull())
            {
                paypalPDTUserID = Convert.ToInt32(appConfiguration.AC_Value);
            }
            return paypalPDTUserID;
        }

        private void UpdateOrderForOnlinePayment(OnlinePaymentTransaction onlinePaymentTransaction, Int32 paypalPDTUserID)
        {
            String amountPaid = View.CurrentViewContext.TransactionDetails["mc_gross"];
            String paymentStatus = View.CurrentViewContext.TransactionDetails["payment_status"];

            if (paymentStatus == "Completed" && _orderIDs.IsNotNull())
            {
                foreach (Int32 _orderID in _orderIDs)
                {
                    Order updatedOrder = ComplianceDataManager.UpdateOrderForOnlinePayment(amountPaid, paypalPDTUserID, _orderID, _tenantID, onlinePaymentTransaction);
                    String paidOrderStatusCode = ApplicantOrderStatus.Paid.GetStringValue();
                    Int32 paidOrderStatusId = ComplianceDataManager.GetOrderStatusList(_tenantID).FirstOrDefault(cond => cond.Code == paidOrderStatusCode).OrderStatusID;

                    if (ComplianceDataManager.IsOrderPaymentIncludeEDSService(_tenantID, onlinePaymentTransaction.OrderPaymentDetails.First().OPD_ID))
                    {
                        var _opdEdSPackageType = onlinePaymentTransaction.OrderPaymentDetails.First();

                        if (_opdEdSPackageType.IsNotNull() && _opdEdSPackageType.OPD_OrderStatusID == paidOrderStatusId)
                        {
                            UpdateEDSStatus(_orderID);
                        }
                    }
                    //Send Print Scan Notification
                    #region UAT-1358:Complio Notification to applicant for PrintScan
                    String bkgServiceTypeCode = BkgServiceType.PRINT_SCAN.GetStringValue();
                    if (BackgroundProcessOrderManager.IsBkgServiceExistInOrder(_tenantID, onlinePaymentTransaction.OrderPaymentDetails.First().OPD_ID, bkgServiceTypeCode))
                    {
                        var _opdPrintScanSvcPackageType = onlinePaymentTransaction.OrderPaymentDetails.First();

                        if (_opdPrintScanSvcPackageType.IsNotNull() && _opdPrintScanSvcPackageType.OPD_OrderStatusID == paidOrderStatusId)
                        {
                            CommunicationManager.SendNotificationForPrintScan(_opdPrintScanSvcPackageType, _tenantID);
                        }
                    }
                    #endregion

                    #region UAT-1560:WB: We should be able to add documents that need to be signed to the order process
                    UpdateAdditionalDocStatus(_tenantID, _orderID);
                    #endregion

                    #region UAT-1759:Create the ability to mark an "Additional Documents" that the students complete in the order flow as "Send to student"
                    ComplianceDataManager.SendAdditionalDocumentsToStudent(_tenantID, onlinePaymentTransaction.OrderPaymentDetails.First().Order, paypalPDTUserID);
                    #endregion

                    ComplianceDataManager.InsertAutomaticInvitationLog(_tenantID, _orderID, AppConsts.NONE); //UAT-2388

                    //UAT-4498
                    ComplianceDataManager.CopyDataForDummyLineItem(_orderID, _tenantID, paypalPDTUserID);
                }
            }
        }

        #endregion
        #region E-DRUG SCREENING
        /// <summary>
        /// Method to update the EDS Related Data For customformdata and also update the external vendor dispatch status.
        /// </summary>
        /// <param name="applicantOrderDataContract">applicantOrderDataContract</param>
        /// <param name="orderId">orderId</param>
        /// <param name="userOrder">userOrder</param>
        public void UpdateEDSStatus(Int32 _orderID)
        {
            BkgOrder bkgOrderObj = BackgroundProcessOrderManager.GetBkgOrderByOrderID(_tenantID, _orderID);
            if (!bkgOrderObj.IsNullOrEmpty())
            {
                List<BkgOrderPackage> lstBkgOrderPackage = BackgroundProcessOrderManager.GetBackgroundPackageIdListByBkgOrderId(_tenantID, bkgOrderObj.BOR_ID);
                if (!lstBkgOrderPackage.IsNullOrEmpty() && (lstBkgOrderPackage.Count() > 0))
                {
                    List<Int32> lstBackgroundPackageId = lstBkgOrderPackage.Select(slct => slct.BkgPackageHierarchyMapping.BackgroundPackage.BPA_ID).ToList();
                    List<Int32> lstBPHM_Id = lstBkgOrderPackage.Select(slt => slt.BOP_BkgPackageHierarchyMappingID.Value).ToList();
                    String extVendorId = String.Empty;
                    ClearStarCCF objClearstarCCf = new ClearStarCCF();

                    ClearStarWebCCFContract clearStarWebCCFContract = new ClearStarWebCCFContract();

                    String result = BackgroundProcessOrderManager.GetClearStarServiceId(_tenantID, lstBackgroundPackageId, BkgServiceType.ELECTRONICDRUGSCREEN.GetStringValue());
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
                            dicParam.Add("TenantId", _tenantID);
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
                            BackgroundProcessOrderManager.RunParallelTaskSaveCCFDataAndPDF(objClearstarCCf.SaveCCFDataAndPDF, dicParam, LoggerService, ExceptiomService, _tenantID);
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
    }
}




