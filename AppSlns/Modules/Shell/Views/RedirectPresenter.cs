using Business.RepoManagers;
using Entity.ClientEntity;
using ExternalVendors.ClearStarVendor;
using INTSOF.Contracts;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace CoreWeb.Shell.Views
{
    public class RedirectPresenter : Presenter<IRedirectView>
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

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        public void SaveTransactionDetails(String invoiceNumber, NameValueCollection transactionDetails)
        {
            if (transactionDetails.IsNotNull())
            {
                GetOrderAndTenantID(invoiceNumber);
                Int32 authorizeDotNetUserId = GetAuthorizeDotNetUserID();
                OnlinePaymentTransaction onlinePaymentTransaction = ComplianceDataManager.UpdateOnlineTransactionResults(invoiceNumber, transactionDetails, _tenantID,
                    authorizeDotNetUserId);
                UpdateOrderForOnlinePayment(onlinePaymentTransaction, transactionDetails, authorizeDotNetUserId);
            }
        }

        public void GetURL(String invoiceNumber)
        {
            if (invoiceNumber.IsNotNull())
            {
                View.ApplicationURL = Convert.ToString(ApplicationDataManager.GetObjectDataByKey(invoiceNumber));
            }
        }

        public void RemoveWebApplicationData(String invoiceNumber)
        {
            ApplicationDataManager.RemoveWebApplicationData(invoiceNumber);
        }

        #endregion

        #region Private Methods

        private void GetOrderAndTenantID(String invoiceNumber)
        {
            Dictionary<String, List<Int32>> data = ComplianceDataManager.GetOrderAndTenantID(invoiceNumber);
            _orderIDs = data["OrderID"];
            _tenantID = data["TenantID"][0];
        }

        private static Int32 GetAuthorizeDotNetUserID()
        {
            Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.AUTHORIZE_DOT_NET_USER_ID);
            Int32 authorizeDotNetUserId = AppConsts.AUTHORIZE_DOT_NET_USER_VALUE;

            if (appConfiguration.IsNotNull())
            {
                authorizeDotNetUserId = Convert.ToInt32(appConfiguration.AC_Value);
            }
            return authorizeDotNetUserId;
        }

        private void UpdateOrderForOnlinePayment(OnlinePaymentTransaction onlinePaymentTransaction, NameValueCollection transactionDetails, Int32 authorizeDotNetUserId)
        {
            String amountPaid = transactionDetails["x_amount"];
            String responseCode = transactionDetails["x_response_code"];
            String responseReasonCode = transactionDetails["x_response_reason_code"];
            String responseReasonText = transactionDetails["x_response_reason_text"].ToLower();
            String successResponseText = "This transaction has been approved.";

            if (responseCode == "1" && responseReasonCode == "1" && responseReasonText == successResponseText.ToLower() && _orderIDs.IsNotNull())
            {
                foreach (Int32 _orderID in _orderIDs)
                {
                    Order updatedOrder = ComplianceDataManager.UpdateOrderForOnlinePayment(amountPaid, authorizeDotNetUserId, _orderID, _tenantID, onlinePaymentTransaction);
                    //Method to update EDS related data in CustomFormOrderData and bkgorderPackageServiceLineItem
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
                    ComplianceDataManager.SendAdditionalDocumentsToStudent(_tenantID, onlinePaymentTransaction.OrderPaymentDetails.First().Order, authorizeDotNetUserId);
                    #endregion

                    ComplianceDataManager.InsertAutomaticInvitationLog(_tenantID, _orderID, AppConsts.NONE); //UAT-2388

                    //UAT-4498
                    ComplianceDataManager.CopyDataForDummyLineItem(_orderID, _tenantID, authorizeDotNetUserId);
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




