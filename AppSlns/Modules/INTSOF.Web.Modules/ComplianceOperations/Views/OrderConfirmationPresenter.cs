#region Namespaces

#region SystemDefined

using System;
using INTSOF.SharedObjects;
using System.Linq;
using System.Xml.Linq;

#endregion

#region UserDefined

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

#endregion

#endregion

namespace CoreWeb.ComplianceOperations.Views
{
    public class OrderConfirmationPresenter : Presenter<IOrderConfirmationView>
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

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads

        }

        public override void OnViewInitialized()
        {
            //View.SelectedPackageDetails = ComplianceDataManager.GetApplicantPackageDetails(View.DPPSId, View.TenantId);
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
        public void GetApplicantAddressData()
        {
            View.ApplicantZipCodeDetails = SecurityManager.GetApplicantZipCodeDetails(View.ZipCodeId);
        }
        //public void GetTenantId()
        //{
        //    View.TenantId = SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
        //}

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
                            //[UAT 264]
                            //Boolean isOrderStatusPaid = orderStatusCode == ApplicantOrderStatus.Paid.GetStringValue();

                            // Handle the case for ChangePaymentType, RushOrder for existing order,
                            // by Applicant, to display only the required Payment instructions
                            //if (View.OrderPaymentDetaildId > AppConsts.NONE)
                            //    View.lstOPDs = order.OrderPaymentDetails.Where(opd => opd.OPD_IsDeleted == false && opd.OPD_ID == View.OrderPaymentDetaildId).ToList();
                            //else
                            //UAT-1648:As an applicant, I should be able to complete payment for an order that is in "sent for online payment"
                            if (View.IsLocationServiceTenant)
                            {
                                View.lstOPDs.AddRange(order.OrderPaymentDetails.Where(opd => opd.OPD_IsDeleted == false).ToList());
                            }
                            else
                            {
                                if (String.Compare(View.OrderRequestType, OrderRequestType.CompleteOrderByApplicant.GetStringValue(), true) == AppConsts.NONE)
                                {
                                    View.lstOPDs.AddRange(order.OrderPaymentDetails.Where(opd => opd.OPD_IsDeleted == false && View.RecentAddedOPDs.Contains(opd.OPD_ID)).ToList());
                                }
                                else
                                {
                                    View.lstOPDs.AddRange(order.OrderPaymentDetails.Where(opd => opd.OPD_IsDeleted == false).ToList());
                                }
                            }
                           

                            //Boolean isOrderStatusPaid = IsOrderStatusPaid(lstOPD);

                            //Temporary
                            if (order.DeptProgramPackage.IsNotNull())
                            {
                                OrderCartCompliancePackage cp = new OrderCartCompliancePackage();

                                cp.PackageName = String.IsNullOrEmpty(order.DeptProgramPackage.CompliancePackage.PackageLabel)
                                    ? order.DeptProgramPackage.CompliancePackage.PackageName
                                    : order.DeptProgramPackage.CompliancePackage.PackageLabel;
                                cp.CompliancePackageID = order.DeptProgramPackage.CompliancePackage.CompliancePackageID;//UAT-3283
                                cp.SubscriptionPeriodMonths = CalculateSubscriptionPeriodMonths(order.SubscriptionYear, order.SubscriptionMonth);
                                cp.OrderId = order.OrderID;
                                View.CompliancePackages.Add(cp);
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

                        if (orderRequestTypeCode == OrderRequestType.ChangeSubscriptionByAdmin.GetStringValue())
                        {
                            //View.InstitutionHierarchy = order.DeptProgramMapping.DPM_Label;
                            View.AppChangeSubPaymentTypeCode = GetChangeSubPayTypeCode(order);
                        }
                        //else
                        //{
                        //    //Temporary
                        //    //if (order.DeptProgramPackage.IsNotNull())
                        //    //    View.InstitutionHierarchy = order.DeptProgramPackage.DeptProgramMapping.DPM_Label;
                        //    //else
                        //    //    View.InstitutionHierarchy = order.DeptProgramMapping.DPM_Label; 
                        //}
                        // UAT 1067 - Hierarchy for orders (background and screening) should display as the full hierarchy sleected during the order, not the node the package lives on. 
                        if (order.DeptProgramMapping1.IsNotNull())
                            View.InstitutionHierarchy = order.DeptProgramMapping1.DPM_Label;
                    }
                }
            }
            // Confirm
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

        public void GetNextPagePathByOrderStageID(ApplicantOrderCart applicantOrderCart)
        {
            View.NextPagePath = ComplianceDataManager.GetNextPagePathByOrderStageID(applicantOrderCart, OrderStages.OnlineConfirmation);
        }

        public decimal GetOrderPriceTotal(int OrderID)
        {
            return FingerPrintDataManager.GetOrderPriceTotal(View.TenantId, View.OrgUsrID, OrderID);
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

        public void GetAttributesForTheCustomForm(String packageId, Int32 customFormId, string languageCode)
        {
            CustomFormDataContract customFormDataContract = BackgroundProcessOrderManager.GetAttributesForTheCustomForm(View.TenantId, packageId, customFormId, languageCode);
            View.lstCustomFormAttributes = customFormDataContract.lstCustomFormAttributes;

        }
        public String GetPaymentInstruction(String paymentModeCode)
        {
            return ComplianceDataManager.GetPaymentInstructionByCode(paymentModeCode);
        }

        ///// <summary>
        ///// Gets the Master Payment Instructions for display
        ///// </summary>
        ///// <param name="lstClientPaymentOptns"></param>
        ///// <returns></returns>
        //public List<Entity.ClientEntity.lkpPaymentOption> GetClientPaymentInstructions()
        //{
        //    return ComplianceDataManager.GetClientPaymentOptions(View.TenantId);
        //}

        private List<Entity.lkpPaymentOption> GetMasterPaymentSettings(out List<Entity.ClientEntity.lkpPaymentOption> lstClientPaymentOptns)
        {
            return ComplianceDataManager.GetMasterPaymentOptions(View.TenantId, out lstClientPaymentOptns);
        }

        #region E Drug Screening
        public void GetEDrugAttributeGroupIdAndFormId()
        {
            String eDrugScrnAttributeGrpCode = AppConsts.ELECTRONIC_DRUG_SCREEN_ATT_GROUP_CODE;
            String result = BackgroundProcessOrderManager.GetEDrugAttributeGroupId(View.TenantId, eDrugScrnAttributeGrpCode);
            if (!result.IsNullOrEmpty())
            {
                String[] separator = { "," };
                String[] splitIds = result.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                View.EDrugScreenCustomFormId = Convert.ToInt32(splitIds[0]);
                View.EDrugScreenAttributeGroupId = Convert.ToInt32(splitIds[1]);
            }
        }
        #endregion

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

        public String GetMaskedSSN(String unformattedSSN)
        {
            return ApplicationDataManager.GetMaskedSSN(unformattedSSN);
        }

        public string GetFormattedPhoneNumber(string phone)
        {
            return ApplicationDataManager.GetFormattedPhoneNumber(phone);
        }
        public void GetSSNSetting()
        {
            View.IsSSNDisabled = (View.TenantId > 0 && View.TenantId != SecurityManager.DefaultTenantID) ? ComplianceDataManager.GetSSNSetting(View.TenantId, Setting.DISABLE_SSN.GetStringValue()) : false;
        }

        public Entity.ResidentialHistory GetCurrentResidentialHistory(Int32 orgUserId)
        {
            return SecurityManager.GetUserResidentialHistories(orgUserId).Where(cond => cond.RHI_IsCurrentAddress == true).FirstOrDefault();
        }

        #endregion

        #region Private Methods

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

        /// <summary>
        /// Calculate Subscription duration in months.
        /// </summary>
        /// <param name="subscriptionYear">subscriptionYear</param>
        /// <param name="subscriptionMonth">subscriptionMonth</param>
        /// <returns>Subscription duration in months</returns>
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

        /// <summary>
        /// Returns FALSE if ANY of the OrderPaymentDetails is Not Paid
        /// </summary>
        /// <param name="ord"></param>
        /// <returns></returns>
        private Boolean IsOrderStatusPaid(List<OrderPaymentDetail> lstOPD)
        {
            return lstOPD.Any(opd => opd.lkpOrderStatu.Code != ApplicantOrderStatus.Paid.GetStringValue()) ? false : true;
        }

        /// <summary>
        /// Returns if the Order Status for any OrderPaymentDetails, which is having the BkgPAckage with EDS is Paid or not 
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get the Payment Type Code selected for the Change Subscription Payment OptionID selected
        /// for the Compliance Package
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
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

        //public OrderPaymentDetail GetComplianceOrderPayDetail()
        //{
        //    return ComplianceDataManager.GetComplianceOrdPayDetail(View.OrderData);
        //}

        #endregion

        #endregion

        #region UAT-781 ENCRYPTED SSN
        /// <summary>
        /// Method to Get Decrypted SSN
        /// </summary>
        /// <param name="encryptedSSN"></param>
        public void GetDecryptedSSN(Int32 orgUserID, Boolean isOrgUserProfile)
        {
            View.DecryptedSSN = ComplianceSetupManager.GetFormattedString(orgUserID, isOrgUserProfile, View.TenantId);
        }
        #endregion

        #region UAT 1190 Mask DOB on order summary
        public String GetMaskDOB(String unMaskedDOB)
        {
            return ApplicationDataManager.GetMaskDOB(unMaskedDOB);
        }
        #endregion

        /// <summary>
        /// UAT 1438: Enhancement to allow students to select a User Group
        /// </summary>
        /// <param name="list"></param>
        public void GetUserGroupListFromUserIDs(List<Int32> lstUserGroupIDs)
        {
            View.selectedUserGrpList = ComplianceSetupManager.GetAllUserGroup(View.TenantId).Where(x => lstUserGroupIDs.Contains(x.UG_ID)).ToList();
        }

        #region UAT-1476:When a tracking package is ordered and there was already a previous package with entered data,
        //then there would be data movement as if there were a subscription change.

        public void CopyCompPackageDataForNewOrder(String orderIDs)
        {
            if (!orderIDs.IsNullOrEmpty())
            {
                string[] arOrderId = orderIDs.Split(',');
                if (arOrderId.IsNotNull() && arOrderId.Length > 0)
                {
                    foreach (string orderId in arOrderId)
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

        public void CopyCompPackageDataForNewOrderParallelTask(Dictionary<String, Object> data)
        {
            Int32 OrderId = Convert.ToInt32(data.GetValue("OrderId"));
            Int32 tenantId = Convert.ToInt32(data.GetValue("tenantId"));
            Int32 currentLoggedInUserId = Convert.ToInt32(data.GetValue("CurrentLoggedInUserId"));
            MobilityManager.CopyCompPackageDataForNewOrder(tenantId, OrderId, currentLoggedInUserId);
        }

        #endregion

        /// <summary>
        /// Get MVR Attribute Data 
        /// </summary>
        /// <param name="packageIds">packageIds</param>
        /// <returns></returns>
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

        /// </summary>
        /// UAT-2970
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

        #region UAT-3268
        public List<OrderPaymentDetail> GetAdditionalPaymentModes()
        {
            List<Int32> lstOPdIds = View.lstOPDs.Where(opd => opd.OPD_Amount.IsNotNull() && opd.OPD_IsDeleted == false && opd.OPD_Amount > 0).Select(sel => sel.OPD_ID).ToList();
            List<OrderPaymentDetail> lstAdditionalPaymentOrderDetails = BackgroundProcessOrderManager.GetAdditionalPaymentModes(lstOPdIds, View.TenantId);
            if (!lstAdditionalPaymentOrderDetails.IsNullOrEmpty())
                return lstAdditionalPaymentOrderDetails;
            else
                return new List<OrderPaymentDetail>();
        }

        #endregion

        #region UAT-3521 CBI || CABS

        public ReserveSlotContract SubmitApplicantAppointment(Int32 orderId, ApplicantOrderCart appOrderCart, String BillingCode = null, String CbiUniqueId = null, Boolean isCompleteYourOrderClick = false)
        {
            ReserveSlotContract reserveSlotContract = new ReserveSlotContract();           
            reserveSlotContract.SlotID = appOrderCart.FingerPrintData.SlotID;                      
            reserveSlotContract.TenantID = View.TenantId;
            reserveSlotContract.AppOrgUserID = View.CurrentLoggedInUserId;
            reserveSlotContract.OrderID = orderId;
            reserveSlotContract.LocationID = appOrderCart.FingerPrintData.LocationId;
            reserveSlotContract.ReservedSlotID = appOrderCart.FingerPrintData.ReserverSlotID;
            reserveSlotContract.IsEventTypeCode = appOrderCart.FingerPrintData.IsEventCode;
            reserveSlotContract.IsOutOfState = appOrderCart.FingerPrintData.IsOutOfState;
            reserveSlotContract.BillingCode = BillingCode;
            reserveSlotContract.CbiUniqueId = CbiUniqueId;
            reserveSlotContract.IsLocationUpdate = isCompleteYourOrderClick|| reserveSlotContract.IsLocationUpdate;
            reserveSlotContract.MailingOptionID = appOrderCart.lstApplicantOrder[0].SelectedMailingOption.HasValue ? appOrderCart.lstApplicantOrder[0].SelectedMailingOption : null;
            reserveSlotContract.IsFingerPrintAndPassPhotoService = appOrderCart.FingerPrintData.IsFingerPrintAndPassPhotoService;


            //bool IsAllOrderHistoryChecked = reserveSlotContract.IsFingerPrintAndPassPhotoService == true ? appOrderCart.lstApplicantOrder[0].lstPackages.All(x => x.IsOrderHistory == true) : false;
            if (appOrderCart.FingerPrintData != null 
                && appOrderCart.FingerPrintData.IsFingerPrintAndPassPhotoService && appOrderCart.FingerPrintData.IsLocationServiceTenant &&
                reserveSlotContract.MailingOptionID.IsNotNull() && reserveSlotContract.MailingOptionID > 0 && appOrderCart.FingerPrintData.IsFromArchivedOrderScreen)
            {
                Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.LOCATION_ID_FOR_DOWNTOWN);
                int LocationId = Convert.ToInt32(appConfiguration.AC_Value);

                if(LocationId > 0)
                {
                    reserveSlotContract.LocationID = LocationId;
                    reserveSlotContract.SlotID = null;
                    reserveSlotContract.ReservedSlotID = 0;
                }
            }

            if (!reserveSlotContract.IsNullOrEmpty())
                return FingerPrintDataManager.SubmitApplicantAppointment(reserveSlotContract, View.CurrentLoggedInUserId, isCompleteYourOrderClick);
            else
                return new ReserveSlotContract();
        }

        public void IsLocationServiceTenant()
        {
            View.IsLocationServiceTenant = SecurityManager.IsLocationServiceTenant(View.TenantId);
        }

        #endregion


        #region UAT-3541

        public void GetAppointmentOrderDetailData(Int32 ApplicantAppointmentId)
        {
            View.AppointmentDetailContract = FingerPrintSetUpManager.GetAppointmentOrderDetailData(View.CurrentLoggedInUserId, false, Convert.ToString(View.TenantId), ApplicantAppointmentId);
        }
        public Boolean SendOrderCreateMail(AppointmentSlotContract scheduleAppointmentContract)
        {
            return FingerPrintSetUpManager.SendOrderCreateMail(View.AppointmentDetailContract, scheduleAppointmentContract);
        }
        #endregion

        //CBI || CABS || Add Suffix
        public void GetSuffixes()
        {
            View.lstSuffixes = new List<Entity.lkpSuffix>();
            View.lstSuffixes = SecurityManager.GetSuffixes();
        }

        public List<CustomFormAutoFillDataContract> GetConditionsforAttributes(StringBuilder xmlStringData)
        {
            return BackgroundProcessOrderManager.GetConditionsforAttributes(View.TenantId, xmlStringData,View.LanguageCode);
        }

        #region UAT-5031
        public bool SaveOrderPaymentInvoice(Int32 tenantId, Int32 orderID, Int32 currentLoggedInUserId, Boolean modifyShipping)
        {
            return ComplianceDataManager.SaveOrderPaymentInvoice(tenantId, orderID, currentLoggedInUserId, modifyShipping);
        }
        #endregion
    }
}





