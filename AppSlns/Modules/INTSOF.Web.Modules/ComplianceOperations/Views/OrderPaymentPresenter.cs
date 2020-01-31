#region Namespaces

#region SystemDefined

using System;
using INTSOF.SharedObjects;
using System.Linq;
using System.Collections.Generic;

#endregion

#region UserDefined

using Business.RepoManagers;
using INTSOF.Utils;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceOperation;

#endregion

#endregion

namespace CoreWeb.ComplianceOperations.Views
{
    public class OrderPaymentPresenter : Presenter<IOrderPaymentView>
    {

        public void GetTenantId()
        {
            View.TenantId = SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
        }

        /// <summary>
        /// Get the payment options
        /// </summary>
        /// <param name="dpmId">Will be used, in case, when NO Compliance package was selected for the purchase.</param>
        public void GetPaymentOptions(Int32 dpmId, Boolean isCompliancePackageSelected)
        {
            //if (View.DPPSId > AppConsts.NONE && isCompliancePackageSelected) // Handle the case when Compliance Package was not selected
            //    View.lstPaymentOptions = ComplianceDataManager.GetPaymentOptions(View.TenantId, View.DPPSId);
            //else
            //    View.lstPaymentOptions = ComplianceDataManager.GetPaymentOptionsByDPMId(View.TenantId, dpmId);
            //View.lstPaymentOptions = ComplianceDataManager.GetPaymentOptionsByDPMId(View.TenantId, dpmId);
            //List<lkpPaymentOption> lstPaymntOptn = ComplianceDataManager.GetPaymentTypeList(View.TenantId);
            //View.PaymentMode_InvoiceId = lstPaymntOptn.Where(t => t.Code == PaymentOptions.InvoiceWithApproval.GetStringValue()).FirstOrDefault().PaymentOptionID;
            //View.PaymentMode_InvoiceWdoutApprvlId = lstPaymntOptn.Where(t => t.Code == PaymentOptions.InvoiceWithOutApproval.GetStringValue()).FirstOrDefault().PaymentOptionID;
        }

        /// <summary>
        /// Get the payment options
        /// </summary>
        /// <param name="dpmId">Will be used, in case, when NO Compliance package was selected for the purchase.</param>
        public void GetPkgPaymentOptions(string dppIds, String bphmIds, Int32 dpmId, String BillingCode = null, Decimal BillingCodeAmount = default(Decimal))
        {
            View.lstPaymentOptions = StoredProcedureManagers.GetPaymentOptions(dppIds, bphmIds, dpmId, View.TenantId);
            var _lkpPaymentOptions = ComplianceDataManager.GetPaymentTypeList(View.TenantId);
            //if (!BillingCode.IsNullOrEmpty())
            //{
            //    foreach (var item in View.lstPaymentOptions)
            //    {
            //        if (item.lstPaymentOptions.Any(t => t.PaymentOptionCode == PaymentOptions.InvoiceWithOutApproval.GetStringValue()))
            //        {
            //            var removeItem = item.lstPaymentOptions.FirstOrDefault(t => t.PaymentOptionCode == PaymentOptions.InvoiceWithOutApproval.GetStringValue());
            //            item.lstPaymentOptions.Remove(removeItem);
            //        }

            //        if (BillingCodeAmount.IsNullOrEmpty() || BillingCodeAmount <= AppConsts.NONE) // Check in UAT-3850
            //        {
            //            var invoiceWOApproval = _lkpPaymentOptions.Where(t => t.Code == PaymentOptions.InvoiceWithOutApproval.GetStringValue()).FirstOrDefault();
            //            PkgPaymentOptions paymentoptions = new PkgPaymentOptions();
            //            paymentoptions.PaymentOptionCode = invoiceWOApproval.Code;
            //            paymentoptions.PaymentOptionId = invoiceWOApproval.PaymentOptionID;
            //            paymentoptions.PaymentOptionName = invoiceWOApproval.Name;
            //            item.lstPaymentOptions.Insert(0, paymentoptions);
            //        }
            //    }
            //}

            View.PaymentMode_InvoiceId = _lkpPaymentOptions.Where(t => t.Code == PaymentOptions.InvoiceWithApproval.GetStringValue()).FirstOrDefault().PaymentOptionID;
            View.PaymentMode_InvoiceWdoutApprvlId = _lkpPaymentOptions.Where(t => t.Code == PaymentOptions.InvoiceWithOutApproval.GetStringValue()).FirstOrDefault().PaymentOptionID;
            View.PaymentMode_CreditCardId = _lkpPaymentOptions.Where(t => t.Code == PaymentOptions.Credit_Card.GetStringValue()).FirstOrDefault().PaymentOptionID;
        }


        public void GetPaymentModeCode(Int32 selectedPaymentModeId)
        {
            View.PaymentModeCode = ComplianceDataManager.GetPaymentOptionCodeById(selectedPaymentModeId, View.TenantId);
        }

        public List<String> GetServiceStatus(Int32 orderID, Int32 orgUserId)
        {
            return FingerPrintDataManager.GetServiceStatus(View.TenantId, orderID, orgUserId);
        }

        public void ShowRushOrderSetting()
        {
            List<lkpSetting> lkpSettingList = ComplianceDataManager.GetSettings(View.TenantId);
            List<ClientSetting> clientSettingList = ComplianceDataManager.GetClientSetting(View.TenantId);
            int rushOrderID = lkpSettingList.WhereSelect(cond => cond.Code == Setting.Enable_Rush_Order.GetStringValue(), col => col.SettingID).FirstOrDefault();

            int rushID = lkpSettingList.WhereSelect(cond => cond.Code == Setting.Enable_Rush_Order_For_Invoice.GetStringValue(), col => col.SettingID).FirstOrDefault();
            string enableRushOrderValue = clientSettingList.WhereSelect(t => t.CS_SettingID == rushOrderID, col => col.CS_SettingValue).FirstOrDefault();
            View.ShowRushOrder = string.IsNullOrEmpty(enableRushOrderValue) ? false : ((enableRushOrderValue == "0") ? false : true);
            string str = clientSettingList.WhereSelect(t => t.CS_SettingID == rushID, col => col.CS_SettingValue).FirstOrDefault();
            View.ShowRushOrderForInvioce = string.IsNullOrEmpty(str) ? false : ((str == "0") ? false : true);
        }

        public String GetNextPagePathByOrderStageID(ApplicantOrderCart applicantOrderCart)
        {
            return ComplianceDataManager.GetNextPagePathByOrderStageID(applicantOrderCart, OrderStages.OrderPayment);
        }

        public Order GetOrderById(Int32 orderID)
        {
            return ComplianceDataManager.GetOrderById(View.TenantId, orderID);
        }
        ////public String GetPaymentInstruction(Int32 selectedPaymentModeId, out String pmCode)
        ////{
        ////    return ComplianceDataManager.GetPaymentInstruction(selectedPaymentModeId, View.TenantId, out pmCode);
        ////}

        public List<Entity.lkpPaymentOption> GetMasterPaymentSettings(out List<Entity.ClientEntity.lkpPaymentOption> lstClientPaymentOptns)
        {
            return ComplianceDataManager.GetMasterPaymentOptions(View.TenantId, out lstClientPaymentOptns);
        }

        public String GetCreditCardPaymentOptionTypeId()
        {
            String paymentOptnTypeId = String.Empty;
            var creditCardPaymntOptn = ComplianceDataManager.GetPaymentTypeList(View.TenantId).FirstOrDefault(x => x.Code == PaymentOptions.Credit_Card.GetStringValue());
            if (creditCardPaymntOptn != null)
            {
                paymentOptnTypeId = creditCardPaymntOptn.PaymentOptionID.ToString();
            }
            return paymentOptnTypeId;
        }

        /// <summary>
        /// Method to get Payment Mode code by payment mode ID
        /// </summary>
        /// <param name="paymentOptionID"></param>
        /// <returns></returns>
        public String GetPaymentCodeByID(Int32 paymentOptionID)
        {
            return ComplianceDataManager.GetPaymentOptionCodeById(paymentOptionID, View.TenantId);
        }

        /// <summary>
        /// Gets the Online payment transaction record by invoice number
        /// </summary>
        /// <param name="orderId">Order Id</param>
        /// <returns></returns>
        public OnlinePaymentTransaction GetOnlinePayTransactionByInvNum(String invoiceNumber)
        {
            return ComplianceDataManager.GetOnlinePayTransactionByInvNum(invoiceNumber, View.TenantId);
        }

        /// <summary>
        /// To get Credit Card Agreement Statement
        /// </summary>
        public String GetCreditCardAgreement()
        {
            String creditCardAgreementStatement = String.Empty;
            String creditCardAgreementStatementKey = AppConsts.CREDIT_CARD_AGREEMENT_STATEMENT_APPCONFIGKEY;
            //Code for globalization//
            if (!View.LanguageCode.IsNullOrEmpty() && View.LanguageCode == Languages.SPANISH.GetStringValue())
            {
                creditCardAgreementStatementKey = AppConsts.CREDIT_CARD_AGREEMENT_STATEMENT_IN_SPANISH_APPCONFIGKEY;
            }
            Entity.ClientEntity.AppConfiguration appConfiguration = ComplianceDataManager.GetAppConfiguration(creditCardAgreementStatementKey, View.TenantId);

            if (appConfiguration.IsNotNull())
            {
                String schoolName = ClientSecurityManager.GetTenantName(View.TenantId);
                creditCardAgreementStatement = appConfiguration.AC_Value;
                creditCardAgreementStatement = creditCardAgreementStatement.Replace(AppConsts.PSIEMAIL_SCHOOLNAME, schoolName);
            }
            return creditCardAgreementStatement;
        }

        //UAT-3268

        public List<PkgAdditionalPaymentInfo> GetAdditionalPriceData()
        {
            List<Int32> lstBkgHierarchyPkgId = new List<int>();

            if (!View.lstRotationQualifyingBkgPkgs.IsNullOrEmpty())
            {
                lstBkgHierarchyPkgId = View.lstRotationQualifyingBkgPkgs.Select(sel => sel.BPHMId).ToList();
            }
            if (!lstBkgHierarchyPkgId.IsNullOrEmpty())
                return BackgroundProcessOrderManager.GetAdditionalPriceData(lstBkgHierarchyPkgId, View.TenantId);
            return new List<PkgAdditionalPaymentInfo>();
        }


        public Entity.AuthNetCustomerProfile GetCustomerProfile(Guid userId)
        {
            return ComplianceDataManager.GetCustomerProfile(userId, View.TenantId);
        }
        #region UAT-3601
        public void GetLabelData(Int32 TenantId)
        {
            List<String> lstCodes = new List<String>();
            lstCodes.Add(Setting.PAYMENTMETHOD_PACKAGE_NAME_LABEL.GetStringValue());
            List<ClientSetting> lstClientSetting = ComplianceDataManager.GetClientSettingsByCodes(View.TenantId, lstCodes, View.LanguageCode);
            var _setting = lstClientSetting.FirstOrDefault(x => x.lkpSetting.Code == Setting.PAYMENTMETHOD_PACKAGE_NAME_LABEL.GetStringValue());
            if (_setting.IsNullOrEmpty())
            {
                View.PackageNameLabel = String.Empty;
            }
            else
            {
                View.PackageNameLabel = _setting.CS_SettingValueLangugaeSpecific;
            }
        }
        #endregion

        #region UAT-3850

        public List<lkpPaymentOption> GetPaymentTypeList()
        {
            return ComplianceDataManager.GetPaymentTypeList(View.TenantId);
        }



        #endregion

        public string GetCreditCardPaymentModeApprovalCode(Int32 dpmId)
        {
            return ComplianceDataManager.GetCreditCardPaymentModeApprovalCode(dpmId, View.TenantId);
        }
    }
}
