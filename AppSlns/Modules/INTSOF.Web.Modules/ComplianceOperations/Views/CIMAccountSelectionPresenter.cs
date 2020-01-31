#region Namespaces

#region SystemDefined

using INTSOF.SharedObjects;
using System.Linq;

#endregion

#region UserDefined

using Business.RepoManagers;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using System.Collections.Generic;
using System;
using System.Collections.Specialized;
using Entity.ClientEntity;
using ExternalVendors.ClearStarVendor;
using INTSOF.UI.Contract;
using System.Web;
using INTSOF.Contracts;
using INTSOF.UI.Contract.ComplianceRuleEngine;
using INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation;

#endregion

#endregion
namespace CoreWeb.ComplianceOperations.Views
{
    public class CIMAccountSelectionPresenter : Presenter<ICIMAccountSelectionView>
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
            if (View.TenantId == AppConsts.NONE)
                View.TenantId = SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
            //UAT 4537 Allow the CC payment method packages that don’t require approval to go through, even while other packages within the same order are still pending approval 
            List<String> invoiceNumberLst = View.InvoiceNumber.Split(';').ToList();
            List<OnlinePaymentTransaction> lstOnlinePaymentTransaction = new List<OnlinePaymentTransaction>();
            if (invoiceNumberLst.Count > AppConsts.ONE)
            {
                foreach (String _invoiceNumber in invoiceNumberLst)
                {
                    lstOnlinePaymentTransaction.Add(ComplianceDataManager.GetPaymentTransactionDetails(_invoiceNumber, View.TenantId));

                }
                View.LstOnlinePaymentTransactionDetails = lstOnlinePaymentTransaction;
                View.OnlinePaymentTransactionDetails = lstOnlinePaymentTransaction.FirstOrDefault();
            }
            else
                View.OnlinePaymentTransactionDetails = ComplianceDataManager.GetPaymentTransactionDetails(View.InvoiceNumber, View.TenantId);

            // TODO: Implement code that will be executed the first time the view loads
        }

        public void GetNextPagePathByOrderStageID(ApplicantOrderCart applicantOrderCart)
        {
            View.NextPagePath = ComplianceDataManager.GetNextPagePathByOrderStageID(applicantOrderCart, OrderStages.CIMAccountSelection);
        }

        /// <summary>
        /// Method to get Organization User data bases on user ID
        /// </summary>
        /// <param name="CurrentLoggedInUserId"></param>
        public void getOrganizationUserDetails(int CurrentLoggedInUserId)
        {
            View.OrganizationUserData = ComplianceDataManager.GetOrganizationUserDetailByOrganizationUserId(CurrentLoggedInUserId);

        }

        public Entity.AuthNetCustomerProfile GetCustomerProfile(Guid userId)
        {
            return ComplianceDataManager.GetCustomerProfile(userId, View.CurrentViewContext.TenantId);
        }

        public long CreateNewAuthNetCustomerProfile(Entity.AuthNetCustomerProfile authNetCustomerProfile)
        {
            return ComplianceDataManager.CreateNewAuthNetCustomerProfile(authNetCustomerProfile, View.CurrentViewContext.TenantId);
        }

        public String GetTenantName()
        {
            return ComplianceDataManager.GetTenantList(SecurityManager.DefaultTenantID, true).Where(col => col.TenantID == View.CurrentViewContext.TenantId).FirstOrDefault().TenantName;
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

        public Dictionary<String, List<Int32>> GetOrderAndTenantID(String invoiceNumber)
        {
            //UAT 4537 Allow the CC payment method packages that don’t require approval to go through, even while other packages within the same order are still pending approval 
            List<String> invoiceNumberLst = invoiceNumber.Split(';').ToList();
            Dictionary<String, List<Int32>> data = new Dictionary<String, List<Int32>>();
            if (invoiceNumberLst.IsNotNull() && invoiceNumberLst.Count > AppConsts.ONE)
            {
                foreach (String item in invoiceNumberLst)
                {
                    data = ComplianceDataManager.GetOrderAndTenantID(item);
                    _orderIDs = data["OrderID"];
                    _tenantID = data["TenantID"][0];
                }
                return data;
            }
            data = ComplianceDataManager.GetOrderAndTenantID(invoiceNumber);
            _orderIDs = data["OrderID"];
            _tenantID = data["TenantID"][0];
            return data;
        }

        /// <summary>
        /// Update the Status of the OPD for the Credit Card, to the status specified
        /// </summary>
        public void UpdateOPDStatus()
        {
            //ComplianceDataManager.UpdateOPDStatus(ApplicantOrderStatus.Online_Payment_Not_Completed.GetStringValue(), View.CurrentLoggedInUserId, View.OPDId, View.TenantId);
            ComplianceDataManager.UpdateOPDStatus(View.TenantId, ApplicantOrderStatus.Online_Payment_Not_Completed.GetStringValue(), View.OrgUsrID, View.OPDId);
        }

        #region UAT-2073: New Payment setting: School approval for MO and CC Kaplan would like a way to keep students from paying for orders of a specific package without their approval.

        /// <summary>
        /// For UAT-2073: To check if School Approval Required for credit card
        /// </summary>
        /// <param name="orderIDs"></param>
        /// <returns></returns>
        public Boolean IsSchoolApprovalRequired(String orderIDs)
        {
            var paymentApprovalStatusID = ComplianceDataManager.GetPaymentApprovalRequiredSetting(View.TenantId, orderIDs);
            String approvalRequiredCode = PaymentApproval.APPROVAL_REQUIRED_BEFORE_PAYMENT.GetStringValue();
            lkpPaymentApproval lkpPaymentApprovalRequired = LookupManager.GetLookUpData<lkpPaymentApproval>(View.TenantId).FirstOrDefault(con => con.PA_Code == approvalRequiredCode && con.PA_IsDeleted == false);
            if (lkpPaymentApprovalRequired.IsNotNull())
            {
                if (paymentApprovalStatusID == lkpPaymentApprovalRequired.PA_ID)
                    return true;
                else
                    return false;
            }
            return false;
        }

        /// <summary>
        /// Get List of Orders for Order IDs
        /// </summary>
        /// <param name="strOrderIDs"></param>
        /// <returns></returns>
        public List<Order> GetListofOrdersForOrderID(String strOrderIDs)
        {
            return ComplianceDataManager.GetListofOrdersForOrderID(View.TenantId, strOrderIDs);
        }

        /// <summary>
        /// Update the status and payment profile Id of the OPD for the Credit Card to the status specified
        /// </summary>
        /// <param name="paymentProfileId"></param>
        /// <param name="ccOPDList"></param>
        public void UpdateOPDStatusAndPaymentProfileId(long paymentProfileId, List<Int32> ccOPDList)
        {
            ComplianceDataManager.UpdateOPDStatusAndPaymentProfileId(View.TenantId, ApplicantOrderStatus.Pending_School_Approval.GetStringValue(), View.OrgUsrID, ccOPDList, paymentProfileId);
        }

        /// <summary>
        /// For UAT-2073: Send order creation notification for Pending School Approval payment status
        /// </summary>
        /// <param name="orderList"></param>
        public void SendNotificationForPendingSchoolApproval(List<Order> orderList)
        {
            foreach (var applicantOrder in orderList)
            {

                CommunicationManager.SendOrderCreationMailForCreditCard(applicantOrder, applicantOrder.OrganizationUserProfile, View.TenantId);
                CommunicationManager.SendOrderCreationMessageForCreditCard(applicantOrder, applicantOrder.OrganizationUserProfile, View.TenantId);
            }
        }

        #endregion

        #endregion

        #region Private Methods

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

        public Order GetOrderById(Int32 orderID)
        {
            return ComplianceDataManager.GetOrderById(View.TenantId, orderID);
        }

        #endregion

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

        #region UAT-3077
        public void ApprovePaymentItem(ItemPaymentContract itemPaymentContract) //,Int32 orderId, Boolean isRequirementPackage, Int32 pkgSubscriptionId)
        {
            ComplianceDataManager.CreateItemPaymentOrder(itemPaymentContract);
            Tuple<Int32, Int32, Int32> itemResult = StoredProcedureManagers.ApprovedPaymentItem(itemPaymentContract.orderID, View.TenantId, View.CurrentLoggedInUserId);

            if (itemResult.Item1 > 0 && itemResult.Item2 > 0 && itemResult.Item3 > 0)
            {
                if (itemPaymentContract.IsRequirementPackage)
                    EvaluateRequirementBuisnessRules(itemResult.Item2, itemResult.Item3, itemPaymentContract.PkgSubscriptionId);
                else
                    evaluatePostSubmitRules(itemResult.Item1, itemResult.Item2, itemResult.Item3);
            }

        }

        public void evaluatePostSubmitRules(Int32 compliancePackageID, Int32 complianceCategoryId, Int32 complianceItemId)
        {

            List<RuleObjectMapping> ruleObjectMappingList = new List<RuleObjectMapping>();
            RuleObjectMapping ruleObjectMappingForPackage = new RuleObjectMapping
            {
                RuleObjectTypeId = Convert.ToString(RuleManager.GetObjectType(ObjectType.Compliance_Package.GetStringValue(), View.TenantId).OT_ID),
                RuleObjectId = Convert.ToString(compliancePackageID),
                RuleObjectParentId = Convert.ToString(AppConsts.NONE)
            };

            RuleObjectMapping ruleObjectMappingForCategory = new RuleObjectMapping
            {
                RuleObjectTypeId = Convert.ToString(RuleManager.GetObjectType(ObjectType.Compliance_Category.GetStringValue(), View.TenantId).OT_ID),
                //RuleObjectId = Convert.ToString(View.ComplianceCategoryId),
                RuleObjectId = Convert.ToString(complianceCategoryId),
                RuleObjectParentId = Convert.ToString(compliancePackageID)
            };

            RuleObjectMapping ruleObjectMappingForItem = new RuleObjectMapping
            {
                RuleObjectTypeId = Convert.ToString(RuleManager.GetObjectType(ObjectType.Compliance_Item.GetStringValue(), View.TenantId).OT_ID),
                RuleObjectId = Convert.ToString(complianceItemId),
                //RuleObjectParentId = Convert.ToString(View.ComplianceCategoryId)
                RuleObjectParentId = Convert.ToString(complianceCategoryId)
            };


            ruleObjectMappingList.Add(ruleObjectMappingForPackage);
            ruleObjectMappingList.Add(ruleObjectMappingForCategory);
            ruleObjectMappingList.Add(ruleObjectMappingForItem);
            RuleManager.evaluatePostSubmitRules(ruleObjectMappingList, View.CurrentLoggedInUserId, View.OrgUsrID, View.TenantId);
        }

        private void EvaluateRequirementBuisnessRules(Int32 reqCategoryId, Int32 reqItemId, Int32 pkgSubscriptionId)
        {
            List<RequirementRuleObject> ruleObjectMappingList = new List<RequirementRuleObject>();

            RequirementRuleObject ruleObjectMappingForCategory = new RequirementRuleObject
            {
                RuleObjectTypeCode = ObjectType.Compliance_Category.GetStringValue(),
                RuleObjectId = Convert.ToString(reqCategoryId),
                RuleObjectParentId = Convert.ToString(AppConsts.NONE)
            };

            RequirementRuleObject ruleObjectMappingForItem = new RequirementRuleObject
            {
                RuleObjectTypeCode = ObjectType.Compliance_Item.GetStringValue(),
                RuleObjectId = Convert.ToString(reqItemId),
                RuleObjectParentId = Convert.ToString(reqCategoryId)
            };

            ruleObjectMappingList.Add(ruleObjectMappingForCategory);
            ruleObjectMappingList.Add(ruleObjectMappingForItem);

            RequirementRuleManager.EvaluateRequirementPostSubmitRules(ruleObjectMappingList, pkgSubscriptionId, View.CurrentLoggedInUserId, View.TenantId);
        }

        public OrganizationUserProfile GetOrganizationUserProfileByOrganizationUserProfileID(Int32 OrganizationUserProfileID)
        {
            return ComplianceDataManager.GetOrganizationUserProfileByUserProfileID(View.TenantId, OrganizationUserProfileID);
        }
        #endregion
    }
}
