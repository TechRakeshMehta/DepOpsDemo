#region Namespaces

#region SystemDefined

using System;
using INTSOF.SharedObjects;
using System.Linq;

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

#endregion

#endregion

namespace CoreWeb.ComplianceOperations.Views
{
    public class ItemPaymentConfirmationPresenter : Presenter<IItemPaymentConfirmationView>
    {
        #region Methods

        #region Public Methods

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads

        }

        public override void OnViewInitialized()
        {

        }
        public void GetGender()
        {
            View.Gender = ComplianceDataManager.GetGenderById(View.GenderId, View.TenantId);
        }
        public void GetApplicantAddressData()
        {
            View.ApplicantZipCodeDetails = SecurityManager.GetApplicantZipCodeDetails(View.ZipCodeId);
        }

        public Boolean IsOrderPaymentDone(Int32 orderID)
        {
            Boolean isOrderPaymentDone = false;
            Order order = null;


            order = ComplianceDataManager.GetOrderById(View.TenantId, orderID);
            if (order.IsNotNull())
            {
                //TODO: Check from data
            }
            return isOrderPaymentDone;
        }

        public void GetPkgPaymentOptions()
        {
            List<PkgPaymentOptions> PkgPaymentOptionsList = new List<PkgPaymentOptions>();
            var result = ComplianceDataManager.GetPaymentTypeList(View.TenantId);
            String PaymentTypeCode = PaymentOptions.Credit_Card.GetStringValue();

            result.ForEach(s => PkgPaymentOptionsList.Add(new PkgPaymentOptions { PaymentOptionId = s.PaymentOptionID, PaymentOptionName = s.Name, PaymentOptionCode = s.Code }));
            View.lstPaymentOptions = PkgPaymentOptionsList;
            var defaultPaymentMethod = result.Where(cond => cond.Code == PaymentTypeCode).FirstOrDefault();
            if (!defaultPaymentMethod.IsNullOrEmpty())
            {
                View.PaymentModeId = defaultPaymentMethod.PaymentOptionID;
                View.PaymentModeCode = defaultPaymentMethod.Code;
                View.PaymentModeDisplayName = defaultPaymentMethod.Name;
                View.InstructionText = defaultPaymentMethod.InstructionText;
            }

            #region Bind Payment Mode With Instruction
            var _lstClientPaymentOptns = new List<Entity.ClientEntity.lkpPaymentOption>();
            var _lstMsaterPaymentSettings = GetMasterPaymentSettings(out _lstClientPaymentOptns);

            List<String> _lstPayModeCodes = new List<String>();
            _lstPayModeCodes.Add(defaultPaymentMethod.Code);

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
            #endregion
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


        /// <summary>
        /// Returns FALSE if ANY of the OrderPaymentDetails is Not Paid
        /// </summary>
        /// <param name="ord"></param>
        /// <returns></returns>
        private Boolean IsOrderStatusPaid(List<OrderPaymentDetail> lstOPD)
        {
            return lstOPD.Any(opd => opd.lkpOrderStatu.Code != ApplicantOrderStatus.Paid.GetStringValue()) ? false : true;
        }



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



        /// </summary>
        /// UAT-2970
        /// To get Credit Card Agreement Statement
        /// </summary>
        public String GetCreditCardAgreement()
        {
            String creditCardAgreementStatement = String.Empty;
            Entity.ClientEntity.AppConfiguration appConfiguration = ComplianceDataManager.GetAppConfiguration(AppConsts.CREDIT_CARD_AGREEMENT_STATEMENT_APPCONFIGKEY, View.TenantId);

            if (appConfiguration.IsNotNull())
            {
                String schoolName = ClientSecurityManager.GetTenantName(View.TenantId);
                creditCardAgreementStatement = appConfiguration.AC_Value;
                creditCardAgreementStatement = creditCardAgreementStatement.Replace(AppConsts.PSIEMAIL_SCHOOLNAME, schoolName);
            }
            return creditCardAgreementStatement;
        }

        public OrganizationUserProfile GetOrganizationUserProfileByOrganizationUserProfileID(Int32 OrganizationUserProfileID)
        {
            return ComplianceDataManager.GetOrganizationUserProfileByUserProfileID(View.TenantId, OrganizationUserProfileID);
        }
        #region SendItemStatusChangeNotification
        public void SendItemStatusChangeNotification(ItemPaymentContract itemData)
        {
            //Send Notification On Item Status Changed To Review Status
            ComplianceDataManager.SendNotificationOnItemStatusChangedToReviewStatus(itemData.IsRequirementPackage, itemData.TenantID, itemData.PkgSubscriptionId,
                                                                                    itemData.PkgId, itemData.CategoryID,
                                                                                    itemData.ItemID, View.CurrentLoggedInUserId, itemData.OrganizationUserID,
                                                                                    String.Empty);
        }
        #endregion
        /// <summary>
        /// Send Payment Notification
        /// </summary>
        /// <param name="itemPaymentContract"></param>
        /// <param name="_orgUserProfile"></param>
        public void SaveItemPaymentCommunication(ItemPaymentContract itemPaymentContract, OrganizationUserProfile _orgUserProfile)
        {
            Entity.OrganizationUser orgUserNew = SecurityManager.GetOrganizationUser(_orgUserProfile.OrganizationUserID);
            CommunicationManager.SendMailForItemPayment(orgUserNew, Convert.ToString(itemPaymentContract.TotalPrice), itemPaymentContract.OrderDate.ToShortDateString(), itemPaymentContract.OrderNumber, itemPaymentContract.ItemName, itemPaymentContract.TenantID);
        }
    }
}





