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
using System.Xml;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.UI.Contract.BkgOperations;
using System.Xml.Linq;
using System.Text;

#endregion

#endregion

namespace CoreWeb.ComplianceOperations.Views
{
    public class OrderReviewPresenter : Presenter<IOrderReviewView>
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

        }

        public override void OnViewInitialized()
        {

        }

        public void GetTenantId()
        {
            View.TenantId = SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
        }

        public void SubmitApplicantOrder(OrganizationUserProfile userProfile, Order userOrder, Int32 selectedPaymentModeId)
        {
            //String paymentModeCode = String.Empty;
            //View.GeneratedOrderId = ComplianceDataManager.SubmitApplicantOrder(userProfile, userOrder, true, View.DPPSId, selectedPaymentModeId, View.TenantId, out paymentModeCode);
            //View.PaymentModeCode = paymentModeCode;
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
                    applicantOrderCart.lstApplicantOrder[0].lstPackages, applicantOrderCart.MailingAddress, out _isXMLGenerated);

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

        #region Private Methods

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

        public void IsLocationServiceTenant()
        {
            View.IsLocationServiceTenant = SecurityManager.IsLocationServiceTenant(View.TenantId);
        }

        public void GetSuffixes()
        {
            View.lstSuffixes = new List<Entity.lkpSuffix>();
            View.lstSuffixes = SecurityManager.GetSuffixes();
        }

        public List<CustomFormAutoFillDataContract> GetConditionsforAttributes(StringBuilder xmlStringData)
        {
            return BackgroundProcessOrderManager.GetConditionsforAttributes(View.TenantId, xmlStringData,View.LanguageCode);
        }
    }
}
