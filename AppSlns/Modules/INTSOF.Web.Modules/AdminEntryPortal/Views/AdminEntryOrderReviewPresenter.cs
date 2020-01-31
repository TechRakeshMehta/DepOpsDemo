using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.Contracts;
using INTSOF.ServiceUtil;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.AdminEntryPortal.Views
{
    public class AdminEntryOrderReviewPresenter : Presenter<IAdminEntryOrderReviewView>
    {
        #region Methods

        #region Public Methods
        public void GetTenantId()
        {
            View.TenantId = SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
        }


        public void GetPaymentOptions()
        {
            View.lstPaymentOptions = ComplianceDataManager.GetPaymentOptions(View.TenantId, View.DPPSId);
            View.PaymentMode_InvoiceId = View.lstPaymentOptions.WhereSelect(t => t.Code == PaymentOptions.InvoiceWithApproval.GetStringValue()).FirstOrNew().PaymentOptionID;
        }
        public void GetGender()
        {
            //Old Code
            //   View.Gender = ComplianceDataManager.GetGenderById(View.GenderId, View.TenantId);
            //New Code for Globalization
            if (!View.LanguageCode.IsNullOrEmpty() && View.LanguageCode == Languages.SPANISH.GetStringValue())
            {
                List<Entity.ClientEntity.lkpGender> lstGender = ComplianceDataManager.GetGenderList(View.TenantId);
                View.Gender = lstGender.Where(cond => cond.DefaultLanguageKeyID == View.GenderId && cond.lkpLanguage.LAN_Code == View.LanguageCode).FirstOrDefault().GenderName;
            }
            else
                View.Gender = ComplianceDataManager.GetGenderById(View.GenderId, View.TenantId);
        }
        public void GetApplicantAddressData()
        {
            View.ApplicantZipCodeDetails = SecurityManager.GetApplicantZipCodeDetails(View.ZipCodeId);
        }
        public void GetPaymentModeCode(Int32 selectedPaymentModeId)
        {
            View.PaymentModeCode = ComplianceDataManager.GetPaymentOptionCodeById(selectedPaymentModeId, View.TenantId);
        }

        //comment if not needed
        public void ShowRushOrderSetting()
        {
            List<lkpSetting> lkpSettingList = ComplianceDataManager.GetSettings(View.TenantId);
            List<ClientSetting> clientSettingList = ComplianceDataManager.GetClientSetting(View.TenantId);
            int rushOrderID = lkpSettingList.WhereSelect(cond => cond.Code == Setting.Enable_Rush_Order.GetStringValue(), col => col.SettingID).FirstOrDefault();
            //int rushID = ComplianceDataManager.GetSettings(View.TenantId).WhereSelect(cond => cond.Code == Setting.Enable_Rush_Order_For_Invoice.GetStringValue(), col => col.SettingID).FirstOrDefault();
            int rushID = lkpSettingList.WhereSelect(cond => cond.Code == Setting.Enable_Rush_Order_For_Invoice.GetStringValue(), col => col.SettingID).FirstOrDefault();
            string enableRushOrderValue = clientSettingList.WhereSelect(t => t.CS_SettingID == rushOrderID, col => col.CS_SettingValue).FirstOrDefault();
            View.ShowRushOrder = string.IsNullOrEmpty(enableRushOrderValue) ? false : ((enableRushOrderValue == "0") ? false : true);
            //string str = ComplianceDataManager.GetClientSetting(View.TenantId).WhereSelect(t => t.CS_SettingID == rushID, col => col.CS_SettingValue).FirstOrDefault();
            string str = clientSettingList.WhereSelect(t => t.CS_SettingID == rushID, col => col.CS_SettingValue).FirstOrDefault();
            View.ShowRushOrderForInvioce = string.IsNullOrEmpty(str) ? false : ((str == "0") ? false : true);
        }

        public Order GetOrderById(Int32 orderID)
        {
            return ComplianceDataManager.GetOrderById(View.TenantId, orderID);
        }

        public void GetNextPagePathByOrderStageID(ApplicantOrderCart applicantOrderCart)
        {
            View.NextPagePath = ComplianceDataManager.GetNextPagePathByOrderStageID(applicantOrderCart, OrderStages.OrderReview);
        }

        public String GetPricingData(ApplicantOrderCart applicantOrderCart, Int32 tenantId)
        {
            if (!applicantOrderCart.lstApplicantOrder[0].lstPackages.IsNullOrEmpty())
            {
                //Changed on 12-June-14.
                BkgDataStore bkgDataStore = new BkgDataStore();
                String personalDataXML = bkgDataStore.ConvertApplicantDataIntoXML(applicantOrderCart, tenantId, false, false, true);
                Boolean _isXMLGenerated;
                String _pricingInputXML = StoredProcedureManagers.GetPricingDataInputXML(personalDataXML, tenantId, applicantOrderCart.lstApplicantOrder[0].lstBackgroundOrderData,
                    applicantOrderCart.lstApplicantOrder[0].lstPackages, out _isXMLGenerated);

                String _pricingOutputXML = StoredProcedureManagers.GetPricingData(_pricingInputXML, tenantId);
                return _pricingOutputXML;
            }
            return String.Empty;
        }


        public void GetAttributesForTheCustomForm(String packageId, Int32 customFormId, string _languageCode)
        {
            CustomFormDataContract customFormDataContract = BackgroundProcessOrderManager.GetAttributesForTheCustomForm(View.TenantId, packageId, customFormId, _languageCode);
            View.lstCustomFormAttributes = customFormDataContract.lstCustomFormAttributes;

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
        #endregion


        #endregion

        public String GetFormattedSSN(String unformattedSSN)
        {
            return ApplicationDataManager.GetFormattedSSN(unformattedSSN);
        }

        public string GetFormattedPhoneNumber(string phone)
        {
            return ApplicationDataManager.GetFormattedPhoneNumber(phone);
        }

        public void GetSSNSetting()
        {
            View.IsSSNDisabled = (View.TenantId > 0 && View.TenantId != SecurityManager.DefaultTenantID) ? ComplianceDataManager.GetSSNSetting(View.TenantId, Setting.DISABLE_SSN.GetStringValue()) : false;
        }

        /// <summary>
        /// UAT 1438: Enhancement to allow students to select a User Group
        /// </summary>
        /// <param name="list"></param>
        public void GetUserGroupListFromUserIDs(List<Int32> lstUserGroupIDs)
        {
            View.selectedUserGrpList = ComplianceSetupManager.GetAllUserGroup(View.TenantId).Where(x => lstUserGroupIDs.Contains(x.UG_ID)).ToList();
        }

        /// <summary>
        /// Get MVR Attribute Data 
        /// </summary>
        /// <param name="packageIds">packageIds</param>
        /// <returns></returns>
        public void GetAttributeFieldsOfSelectedPackages(String packageIds)
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

        //UAT-1867:Add breakdown of fees in total price on the order review screen (before submit).
        //getting data from db using storedProcedure for binding background service line item price breakdown grid on completing order from order history screen.
        public List<BackroundOrderServiceLinePrice> GetBackgroundOrderServiceLinePriceData(Int32 OrderId, List<Int32> Bkg_pkgIDs)
        {
            List<BackroundOrderServiceLinePrice> _lstbkgOrderSvcLinePrice = new List<BackroundOrderServiceLinePrice>();
            OrderServiceLineItemPriceInfo _lstOrderServiceLineItemPriceInfo = BackgroundProcessOrderManager.GetBackroundOrderServiceLinePriceByOrderID(View.TenantId, OrderId, Bkg_pkgIDs);

            _lstbkgOrderSvcLinePrice = _lstOrderServiceLineItemPriceInfo.BOSLPrice;
            return _lstbkgOrderSvcLinePrice;
        }

        //comment if not needed
        public void IsLocationServiceTenant()
        {
            View.IsLocationServiceTenant = SecurityManager.IsLocationServiceTenant(View.TenantId);
        }

	    public void GetAdminEntrySuffixes()
        {
            View.lstSuffixes = new List<Entity.lkpAdminEntrySuffix>();
            View.lstSuffixes = SecurityManager.GetAdminEntrySuffixes();
        }

        public List<CustomFormAutoFillDataContract> GetConditionsforAttributes(StringBuilder xmlStringData)
        {
            return BackgroundProcessOrderManager.GetConditionsforAttributes(View.TenantId, xmlStringData, View.LanguageCode);
        }
        #region Admin Entry Portal
        public Boolean IsOrderPaymentDetailExist(Int32 OrderId)
        {
            var orderPayemntData = ComplianceDataManager.GetAllPaymentDetailsOfOrderByOrderID(View.TenantId, OrderId);
            if (!orderPayemntData.IsNullOrEmpty() && orderPayemntData.Count > 0)
            {
                String paidStatusCode = ApplicantOrderStatus.Paid.GetStringValue();
                return orderPayemntData.Any(x => x.lkpOrderStatu.Code == paidStatusCode);
            }
            return false;
        }
        public List<lkpPaymentOption> GetClientPaymentOptions()
        {
            return ComplianceDataManager.GetClientPaymentOptions(View.TenantId);
        }
        public Int32 GetHierarchyNodeID(Int32? dppID, Int32? bphmId)
        {
            return ComplianceDataManager.GetHierarchyNodeID(dppID, bphmId, View.TenantId);
        }
        public Int32? GetLKPOrderRequestTypeID(String orderRequestType)
        {
            return ComplianceDataManager.GetLKPOrderRequestType(orderRequestType, View.TenantId).IsNullOrEmpty() ? (Int32?)null : ComplianceDataManager.GetLKPOrderRequestType(orderRequestType, View.TenantId).ORT_ID;
        }
        public List<lkpOrderPackageType> GetOrderPackageTypeList()
        {
            return LookupManager.GetLookUpData<lkpOrderPackageType>(View.TenantId);
        }
        public List<lkpOrderStatu> GetOrderStatusList()
        {
            return LookupManager.GetLookUpData<lkpOrderStatu>(View.TenantId);
        }
        public BkgOrder GetBkgOrderByOrderID(Int32 OrderId)
        {
            return BackgroundProcessOrderManager.GetBkgOrderByOrderID(View.TenantId, OrderId);
        }
        public String GetClearStarServiceId(List<Int32> bkgPackageServiceId, String serviceTypeCode)
        {
            return BackgroundProcessOrderManager.GetClearStarServiceId(View.TenantId, bkgPackageServiceId, serviceTypeCode);
        }
        public Boolean IsOrderPaymentIncludeEDSService(Int32 ordPaymentDetailId)
        {
            return ComplianceDataManager.IsOrderPaymentIncludeEDSService(View.TenantId, ordPaymentDetailId);
        }
        public Boolean IsOrderPaymentDone(String orderIDs, String orderRequestType, List<Int32> RecentAddedOPDs)
        {
            Boolean isOrderPaymentDone = false;
            Order order = null;

            if (!orderIDs.IsNullOrEmpty())
            {
                string[] arOrderId = orderIDs.Split(',');
                if (arOrderId.IsNotNull() && arOrderId.Length > 0)
                {
                    List<Order> OrderData = new List<Order>();
                    View.lstOPDs = new List<OrderPaymentDetail>();
                    List<OrderCartCompliancePackage> CompliancePackages = new List<OrderCartCompliancePackage>();
                    foreach (string orderId in arOrderId)
                    {
                        order = ComplianceDataManager.GetOrderById(View.TenantId, Convert.ToInt32(orderId.Trim()));
                        if (order.IsNotNull())
                        {
                            OrderData.Add(order);
                            //[UAT 264]
                            //Boolean isOrderStatusPaid = orderStatusCode == ApplicantOrderStatus.Paid.GetStringValue();

                            // Handle the case for ChangePaymentType, RushOrder for existing order,
                            // by Applicant, to display only the required Payment instructions
                            //if (View.OrderPaymentDetaildId > AppConsts.NONE)
                            //    View.lstOPDs = order.OrderPaymentDetails.Where(opd => opd.OPD_IsDeleted == false && opd.OPD_ID == View.OrderPaymentDetaildId).ToList();
                            //else
                            //UAT-1648:As an applicant, I should be able to complete payment for an order that is in "sent for online payment"
                            if (String.Compare(orderRequestType, OrderRequestType.CompleteOrderByApplicant.GetStringValue(), true) == AppConsts.NONE)
                            {
                                View.lstOPDs.AddRange(order.OrderPaymentDetails.Where(opd => opd.OPD_IsDeleted == false && RecentAddedOPDs.Contains(opd.OPD_ID)).ToList());
                            }
                            else
                            {
                                View.lstOPDs.AddRange(order.OrderPaymentDetails.Where(opd => opd.OPD_IsDeleted == false).ToList());
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
                                CompliancePackages.Add(cp);
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


                        var _selectedClientPayInstructions = _lstClientPaymentOptns.Where(po => _lstPayModeCodes.Contains(po.Code) && !po.IsDeleted).ToList();


                        List<Tuple<String, String>> _instructions = new List<Tuple<String, String>>();

                        foreach (var _clientPI in _selectedClientPayInstructions)
                        {
                            var _masterPI = _lstMsaterPaymentSettings.Where(mpi => mpi.Code == _clientPI.Code).FirstOrDefault();

                            if (_masterPI.IsNotNull() && !_masterPI.InstructionText.IsNullOrEmpty())
                                _instructions.Add(new Tuple<String, String>(_clientPI.Name, _masterPI.InstructionText));
                        }
                        View.lstClientPaymentOptns = _instructions;
                        //UAT 916
                        String orderRequestTypeCode = LookupManager.GetLookUpData<Entity.ClientEntity.lkpOrderRequestType>(View.TenantId).FirstOrDefault(con => con.ORT_ID == order.OrderRequestTypeID).ORT_Code;

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
        private List<Entity.lkpPaymentOption> GetMasterPaymentSettings(out List<Entity.ClientEntity.lkpPaymentOption> lstClientPaymentOptns)
        {
            return ComplianceDataManager.GetMasterPaymentOptions(View.TenantId, out lstClientPaymentOptns);
        }

        public List<ApplicantUserGroupMapping> AddCustomAttributeValuesForUserGroup(List<int> lstCustomAttributeUserGroupIDs, int organizationUserId, int organizationUserID)
        {
            return AdminEntryPortalManager.AddCustomAttributeValuesForUserGroup(lstCustomAttributeUserGroupIDs, organizationUserId, organizationUserID);
        }

        public Dictionary<string, string> SubmitApplicantOrder(Order order, ApplicantOrderDataContract applicantOrderDataContract, bool isUpdateMainProfile, List<PreviousAddressContract> lstPrevAddresses, List<PersonAliasContract> lstPersonAlias, out string paymentModeCode, out int orderId, int organizationUserID, List<OrderCartCompliancePackage> list, List<ApplicantOrder> lstApplicantOrder)
        {
            return AdminEntryPortalManager.SubmitApplicantOrder(order, applicantOrderDataContract, isUpdateMainProfile, lstPrevAddresses, lstPersonAlias, out paymentModeCode, out orderId, organizationUserID, list, lstApplicantOrder, true);
        }

        public void SaveApplicantEsignatureDocument(int tenantId, int applicantDisclaimerDocumentId, List<int?> applicantDisclosureDocumentIds, int orderId, int organizationUserProfileID, int organizationUserID, string orderNumber)
        {
            AdminEntryPortalManager.SaveApplicantEsignatureDocument(tenantId, applicantDisclaimerDocumentId, applicantDisclosureDocumentIds, orderId, organizationUserProfileID, organizationUserID, orderNumber);
        }

        public bool IsSubscriptionExistForApplicant(int organizationUserID, int tenantId)
        {
            return ComplianceDataManager.IsSubscriptionExistForApplicant(organizationUserID, tenantId);
        }

        public List<ApplicantDocument> UpdateApplicantAdditionalEsignatureDocument(int tenantId, List<int?> applicantAdditionalDocumentIds, int orderId, int organizationUserProfileID, int organizationUserID, bool needToSaveMappingInGenricDocMapping, List<int?> additionalDocSendToStudent, List<SystemDocBkgSvcMapping> lstSystemDocBkgSvcMapping)
        {
            return AdminEntryPortalManager.UpdateApplicantAdditionalEsignatureDocument(tenantId, applicantAdditionalDocumentIds, orderId, organizationUserProfileID, organizationUserID, needToSaveMappingInGenricDocMapping, additionalDocSendToStudent, lstSystemDocBkgSvcMapping);
        }

        public Boolean AddUpdateAdminEntryUserData(OrganizationUserProfile UserProfile, Int32 currentLoggedInUserId, Int32 tenantId, List<PersonAliasContract> lstPersonAlias
                                                   , List<PreviousAddressContract> lstPrevAddress)
        {
            return AdminEntryPortalManager.AddUpdateAdminEntryUserData(UserProfile, currentLoggedInUserId, lstPersonAlias, tenantId, lstPrevAddress);
        }
        public Boolean UpdateApplicatInviteToken(Int32 tenantId, Int32 orderId)
        {
            return AdminEntryPortalManager.UpdateApplicatInviteToken(tenantId, orderId);
        }
        #endregion


    }
}
