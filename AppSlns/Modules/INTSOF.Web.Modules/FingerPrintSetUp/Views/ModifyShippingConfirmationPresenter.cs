using INTSOF.SharedObjects;
using Business.RepoManagers;
using INTSOF.Utils;
using INTSOF.UI.Contract.ComplianceOperation;
using Entity.ClientEntity;
using System.Collections.Generic;
using INTSOF.UI.Contract.BkgOperations;
using System.Web;
using INTSOF.Contracts;
using INTSOF.ServiceUtil;
using INTSOF.UI.Contract.FingerPrintSetup;
using System.Text;
using System;
using System.Linq;

namespace CoreWeb.FingerPrintSetUp.Views
{
   public class ModifyShippingConfirmationPresenter : Presenter<IModifyShippingConfirmationView>
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

        private void CopyData(Dictionary<String, Object> data)
        {
            Int32 packageSubscriptionID = Convert.ToInt32(data.GetValue("packageSubscriptionID"));
            Int32 tenantId = Convert.ToInt32(data.GetValue("tenantId"));
            Int32 currentLoggedInUserId = Convert.ToInt32(data.GetValue("CurrentLoggedInUserId"));
            //Copy Compliance to compliance package data.
            Int32 OrderId = Convert.ToInt32(data.GetValue("OrderId"));
            MobilityManager.CopyCompPackageDataForNewOrder(tenantId, OrderId, currentLoggedInUserId);
            ComplianceDataManager.CopyData(packageSubscriptionID, tenantId, currentLoggedInUserId, null);

        }

        #region Public Methods

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads

        }

        public override void OnViewInitialized()
        {
            
        }

        public void IsLocationServiceTenant()
        {
            View.IsLocationServiceTenant = SecurityManager.IsLocationServiceTenant(View.TenantId);
        }

        public void GetSSNSetting()
        {
            View.IsSSNDisabled = (View.TenantId > 0 && View.TenantId != SecurityManager.DefaultTenantID) ? ComplianceDataManager.GetSSNSetting(View.TenantId, Setting.DISABLE_SSN.GetStringValue()) : false;
        }

        public void GetNextPagePathByOrderStageID(ApplicantOrderCart applicantOrderCart)
        {
            View.NextPagePath = ComplianceDataManager.GetNextPagePathByOrderStageID(applicantOrderCart, OrderStages.OnlineConfirmation);
        }

        public Boolean IsOrderPaymentDone(String orderIDs)
        {
            Boolean isOrderPaymentDone = false;
            Order order = null;

            if (!orderIDs.IsNullOrEmpty())
            {
                string[] arOrderId = orderIDs.Split(',');
                if (arOrderId.IsNotNull() && arOrderId.Length > 0)
                {
                    View.OrderData = new List<Order>();
                    View.lstOPDs = new List<OrderPaymentDetail>();
                    View.CompliancePackages = new List<OrderCartCompliancePackage>();
                    foreach (string orderId in arOrderId)
                    {
                        order = ComplianceDataManager.GetOrderById(View.TenantId, Convert.ToInt32(orderId.Trim()));
                        if (order.IsNotNull())
                        {
                            View.OrderData.Add(order);
                            if (String.Compare(View.OrderRequestType, OrderRequestType.CompleteOrderByApplicant.GetStringValue(), true) == AppConsts.NONE)
                            {
                                View.lstOPDs.AddRange(order.OrderPaymentDetails.Where(opd => opd.OPD_IsDeleted == false && View.RecentAddedOPDs.Contains(opd.OPD_ID)).ToList());
                            }
                            else
                            {
                                View.lstOPDs.AddRange(order.OrderPaymentDetails.Where(opd => opd.OPD_IsDeleted == false).ToList());
                            }
                        }
                    }

                    if (order.IsNotNull())
                    {
                        var _lstClientPaymentOptns = new List<Entity.ClientEntity.lkpPaymentOption>();
                        var _lstMsaterPaymentSettings = GetMasterPaymentSettings(out _lstClientPaymentOptns);

                        // Remove the Instructions for which the Amount was zero
                        var _zeroPaymentModes = View.lstOPDs.Where(opd => opd.OPD_PaymentOptionID.IsNotNull()
                               && opd.lkpPaymentOption.Code == PaymentOptions.Credit_Card.GetStringValue()
                               && opd.OPD_Amount.IsNotNull() && opd.OPD_Amount == 0);

                        if (_zeroPaymentModes.IsNotNull())
                        {
                            foreach (OrderPaymentDetail opd in _zeroPaymentModes.ToList())
                                View.lstOPDs.Remove(opd);
                        }

                        List<String> _lstPayModeCodes = (from pm in View.lstOPDs select pm.lkpPaymentOption.Code).Distinct().ToList();//View.lstOPDs.Select(opd => opd.lkpPaymentOption.Code).ToList().Distinct(t1=>t1.);


                        var _selectedClientPayInstructions = _lstClientPaymentOptns.Where(po => _lstPayModeCodes.Contains(po.Code)
                                                                        && !po.IsDeleted
                                                                        ).ToList();


                        List<Tuple<String, String>> _instructions = new List<Tuple<String, String>>();

                        foreach (var _clientPI in _selectedClientPayInstructions)
                        {
                            var _masterPI = _lstMsaterPaymentSettings.Where(mpi => mpi.Code == _clientPI.Code).FirstOrDefault();

                            if (_masterPI.IsNotNull() && !_masterPI.InstructionText.IsNullOrEmpty())
                                _instructions.Add(new Tuple<String, String>(_clientPI.Name, _masterPI.InstructionText));
                        }
                        View.lstClientPaymentOptns = _instructions;
                        //UAT 916
                        String orderRequestTypeCode = LookupManager.GetLookUpData<Entity.ClientEntity.lkpOrderRequestType>(View.TenantId)
                                                        .FirstOrDefault(con => con.ORT_ID == order.OrderRequestTypeID).ORT_Code;

                        isOrderPaymentDone = true;
                        if (orderRequestTypeCode == OrderRequestType.NewOrder.GetStringValue() && IsEDSPackagePaid(View.lstOPDs))
                        {
                            View.IsOrderStatusPaid = true;
                        }
                        if (order.DeptProgramMapping1.IsNotNull())
                            View.InstitutionHierarchy = order.DeptProgramMapping1.DPM_Label;
                    }
                }
            }
            if (View.CompliancePackages.Count == AppConsts.NONE && View.DPPSIds.IsNotNull() && View.DPPSIds.Count > AppConsts.NONE)
            {
                View.CompliancePackages = new List<OrderCartCompliancePackage>();
                foreach (int dppsId in View.DPPSIds)
                {
                    var selectedPackageDetails = ComplianceDataManager.GetApplicantPackageDetails(dppsId, View.TenantId);
                    View.InstitutionHierarchy = selectedPackageDetails.DeptProgramPackage.DeptProgramMapping.DPM_Label;

                    //Temporary
                    if (selectedPackageDetails.IsNotNull())
                    {
                        OrderCartCompliancePackage cp = new OrderCartCompliancePackage();

                        cp.PackageName = String.IsNullOrEmpty(selectedPackageDetails.DeptProgramPackage.CompliancePackage.PackageLabel)
                            ? selectedPackageDetails.DeptProgramPackage.CompliancePackage.PackageName
                            : selectedPackageDetails.DeptProgramPackage.CompliancePackage.PackageLabel;
                        cp.CompliancePackageID = selectedPackageDetails.DeptProgramPackage.CompliancePackage.CompliancePackageID; //UAT-3283
                        cp.SubscriptionPeriodMonths = CalculateSubscriptionPeriodMonths(selectedPackageDetails.SubscriptionOption.Year, selectedPackageDetails.SubscriptionOption.Month);
                        View.CompliancePackages.Add(cp);
                    }
                }
            }
            return isOrderPaymentDone;
        }

        public String CalculateSubscriptionPeriodMonths(Int32? subscriptionYear, Int32? subscriptionMonth)
        {
            Int32 subscriptionPeriodMonths = 0;

            if (subscriptionYear != null)
            {
                subscriptionPeriodMonths = (subscriptionYear ?? 0) * 12;
            }

            if (subscriptionMonth != null)
            {
                subscriptionPeriodMonths += (subscriptionMonth ?? 0);
            }
            return Convert.ToString(subscriptionPeriodMonths);
        }
        public String GetMaskDOB(String unMaskedDOB)
        {
            return ApplicationDataManager.GetMaskDOB(unMaskedDOB);
        }
        public String GetMaskedSSN(String unformattedSSN)
        {
            return ApplicationDataManager.GetMaskedSSN(unformattedSSN);
        }
        public Entity.ResidentialHistory GetCurrentResidentialHistory(Int32 orgUserId)
        {
            return SecurityManager.GetUserResidentialHistories(orgUserId).Where(cond => cond.RHI_IsCurrentAddress == true).FirstOrDefault();
        }
        public string GetFormattedPhoneNumber(string phone)
        {
            return ApplicationDataManager.GetFormattedPhoneNumber(phone);
        }
        public void GetDecryptedSSN(Int32 orgUserID, Boolean isOrgUserProfile)
        {
            View.DecryptedSSN = ComplianceSetupManager.GetFormattedString(orgUserID, isOrgUserProfile, View.TenantId);
        }
        public void GetGender()
        {
            if (!View.LanguageCode.IsNullOrEmpty() && View.LanguageCode == Languages.SPANISH.GetStringValue())
            {
                List<Entity.ClientEntity.lkpGender> lstGender = ComplianceDataManager.GetGenderList(View.TenantId);
                View.Gender = lstGender.Where(cond => cond.DefaultLanguageKeyID == View.GenderId && cond.lkpLanguage.LAN_Code == View.LanguageCode).FirstOrDefault().GenderName;
            }
            else
                View.Gender = ComplianceDataManager.GetGenderById(View.GenderId, View.TenantId);
            //View.Gender = ComplianceDataManager.GetGenderById(View.GenderId, View.TenantId);
        }

        private List<Entity.lkpPaymentOption> GetMasterPaymentSettings(out List<Entity.ClientEntity.lkpPaymentOption> lstClientPaymentOptns)
        {
            return ComplianceDataManager.GetMasterPaymentOptions(View.TenantId, out lstClientPaymentOptns);
        }

        public void GetSuffixes()
        {
            View.lstSuffixes = new List<Entity.lkpSuffix>();
            View.lstSuffixes = SecurityManager.GetSuffixes();
        }
        public string GetOrderNumber(int OrderID)
        {
            return FingerPrintDataManager.GetOrderNumber(View.TenantId, View.OrgUsrID, OrderID);
        }
        public void GetAttributesForTheCustomForm(String packageId, Int32 customFormId, string languageCode)
        {
            CustomFormDataContract customFormDataContract = BackgroundProcessOrderManager.GetAttributesForTheCustomForm(View.TenantId, packageId, customFormId, languageCode);
            View.lstCustomFormAttributes = customFormDataContract.lstCustomFormAttributes;

        }
        public List<CustomFormAutoFillDataContract> GetConditionsforAttributes(StringBuilder xmlStringData)
        {
            return BackgroundProcessOrderManager.GetConditionsforAttributes(View.TenantId, xmlStringData, View.LanguageCode);
        }
        public void GetAttributeFieldsOfSelectedPackages(String packageIds)
        {
            if (!packageIds.IsNullOrEmpty())
            {
                List<Entity.ClientEntity.AttributeFieldsOfSelectedPackages> lstAttributeFields = BackgroundProcessOrderManager.GetAttributeFieldsOfSelectedPackages(packageIds, View.TenantId);
                if (!lstAttributeFields.IsNullOrEmpty())
                {
                    View.LstInternationCriminalSrchAttributes = lstAttributeFields.Where(cond => (cond.BSA_Code.ToUpper().Equals("3DA8912A-6337-4B8F-93C4-88BFC3032D2D")
                                                                            || cond.BSA_Code.ToUpper().Equals("AAB51E52-2A9B-42AB-9A9D-D1AFFC18E211")
                                                                            || cond.BSA_Code.ToUpper().Equals("515BEF57-9072-4D2A-A97A-0C248BB045F9"))).ToList();
                }
                else
                {
                    View.LstInternationCriminalSrchAttributes = new List<AttributeFieldsOfSelectedPackages>();
                }
            }
            else
            {
                View.LstInternationCriminalSrchAttributes = new List<AttributeFieldsOfSelectedPackages>();
            }
        }

        private Boolean IsEDSPackagePaid(List<OrderPaymentDetail> lstOPD)
        {
            var lstOPPD = new List<OrderPkgPaymentDetail>();
            List<Int32> lstBOPIds = new List<Int32>();

            foreach (var opd in lstOPD)
            {
                lstBOPIds.AddRange(opd.OrderPkgPaymentDetails.Where(oppd => oppd.OPPD_IsDeleted == false && oppd.OPPD_BkgOrderPackageID.IsNotNull()
                    && oppd.lkpOrderPackageType.OPT_Code == OrderPackageTypes.BACKGROUND_PACKAGE.GetStringValue())
                     .Select(oppd => Convert.ToInt32(oppd.OPPD_BkgOrderPackageID)));

                lstOPPD.AddRange(opd.OrderPkgPaymentDetails);
            }
            var _bkgPkgOpdId = AppConsts.NONE;

            if (lstBOPIds.IsNullOrEmpty())
                return false;

            _bkgPkgOpdId = BackgroundProcessOrderManager.GetEDSBkgOrderPkgId(lstBOPIds, View.TenantId);

            var _edsOPPD = lstOPPD.Where(oppd => oppd.OPPD_BkgOrderPackageID == _bkgPkgOpdId && oppd.OPPD_BkgOrderPackageID.IsNotNull()).FirstOrDefault();

            if (_edsOPPD.IsNull() || _edsOPPD.OrderPaymentDetail.lkpOrderStatu.Code != ApplicantOrderStatus.Paid.GetStringValue())
                return false;

            return true;
        }

        private String GetChangeSubPayTypeCode(Order order)
        {
            var _paymentOptnCode = String.Empty;
            var _opdList = order.OrderPaymentDetails.ToList();

            foreach (var opd in _opdList)
            {
                if (opd.OrderPkgPaymentDetails.Any(oppd => oppd.OPPD_BkgOrderPackageID.IsNull()
                     && oppd.OPPD_IsDeleted == false && oppd.lkpOrderPackageType.OPT_Code == OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue()))
                {
                    _paymentOptnCode = opd.lkpPaymentOption.Code;
                }
            }
            return _paymentOptnCode;
        }

        public void GetAppointmentOrderDetailData(Int32 ApplicantAppointmentId)
        {
            View.AppointmentDetailContract = FingerPrintSetUpManager.GetAppointmentOrderDetailData(View.CurrentLoggedInUserId, false, Convert.ToString(View.TenantId), ApplicantAppointmentId);
        }

        public void GetOrderBkgPackageDetails(String orderIDs)
        {
            if (!orderIDs.IsNullOrEmpty())
            {
                string[] arOrderId = orderIDs.Split(',');
                if (arOrderId.IsNotNull() && arOrderId.Length > 0)
                {
                    foreach (string orderId in arOrderId)
                    {
                        View.lstExternalPackages = ComplianceDataManager.GetBkgOrderById(View.TenantId, 0, Convert.ToInt32(orderId.Trim()));
                        if (View.lstExternalPackages.IsNotNull() && View.lstExternalPackages.Rows.Count > AppConsts.NONE)
                            return;
                    }
                }
            }
        }

        public void CopyBkgDataToCompliancePackage(String orderIDs)
        {
            if (!orderIDs.IsNullOrEmpty())
            {
                string[] arOrderId = orderIDs.Split(',');
                if (arOrderId.IsNotNull() && arOrderId.Length > 0)
                {
                    foreach (string orderId in arOrderId)
                    {
                        Int32 packageSubscriptionID = ComplianceDataManager.GetPackageSubscriptionID(Convert.ToInt32(orderId.Trim()), View.TenantId);
                        if (packageSubscriptionID > 0)
                        {
                            Dictionary<String, Object> dataDict = new Dictionary<String, Object>();
                            dataDict.Add("packageSubscriptionID", packageSubscriptionID);
                            dataDict.Add("tenantId", View.TenantId);
                            //dataDict.Add("CurrentLoggedInUserId", View.CurrentLoggedInUserId);
                            dataDict.Add("CurrentLoggedInUserId", View.OrgUsrID);
                            dataDict.Add("OrderId", orderId);

                            var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
                            var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;
                            ParallelTaskContext.PerformParallelTask(CopyData, dataDict, LoggerService, ExceptiomService);
                        }
                        else
                        {
                            Dictionary<String, Object> dataDict = new Dictionary<String, Object>();
                            dataDict.Add("OrderId", orderId);
                            dataDict.Add("tenantId", View.TenantId);
                            //dataDict.Add("CurrentLoggedInUserId", View.CurrentLoggedInUserId);
                            dataDict.Add("CurrentLoggedInUserId", View.OrgUsrID);

                            var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
                            var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;
                            ParallelTaskContext.PerformParallelTask(CopyCompPackageDataForNewOrderParallelTask, dataDict, LoggerService, ExceptiomService);
                        }
                    }
                }
            }
        }
        public void CopyCompPackageDataForNewOrderParallelTask(Dictionary<String, Object> data)
        {
            Int32 OrderId = Convert.ToInt32(data.GetValue("OrderId"));
            Int32 tenantId = Convert.ToInt32(data.GetValue("tenantId"));
            Int32 currentLoggedInUserId = Convert.ToInt32(data.GetValue("CurrentLoggedInUserId"));
            MobilityManager.CopyCompPackageDataForNewOrder(tenantId, OrderId, currentLoggedInUserId);
        }
        public decimal GetOrderPriceTotal(int OrderID)
        {
            return FingerPrintDataManager.GetOrderPriceTotal(View.TenantId, View.OrgUsrID, OrderID);
        }
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
        #endregion

        #endregion
    }
}
