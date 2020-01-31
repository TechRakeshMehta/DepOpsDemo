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
using System.Web;
using INTSOF.Contracts;

#endregion

#endregion

namespace CoreWeb.ComplianceOperations.Views
{
    public class ItemPaymentPresenter : Presenter<IItemPaymentView>
    {

        public void GetTenantId()
        {
            View.TenantId = SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
        }

        /// <summary>
        /// Get the payment options
        /// </summary>
        /// <param name="dpmId">Will be used, in case, when NO Compliance package was selected for the purchase.</param>
        public void GetPkgPaymentOptions()
        {
            List<PkgPaymentOptions> PkgPaymentOptionsList = new List<PkgPaymentOptions>();
            var result = ComplianceDataManager.GetPaymentTypeList(View.TenantId);
            String PaymentTypeCode = PaymentOptions.Credit_Card.GetStringValue();

            result.ForEach(s => PkgPaymentOptionsList.Add(new PkgPaymentOptions { PaymentOptionId = s.PaymentOptionID, PaymentOptionName = s.Name, PaymentOptionCode = s.Code }));
            View.lstPaymentOptions = PkgPaymentOptionsList;
            //  View.lstPaymentOptions.AddRange(result.Select(s => new PkgPaymentOptions { PaymentOptionId = s.PaymentOptionID, PaymentOptionName = s.Name, PaymentOptionCode = s.Code }));
            var defaultPaymentMethod = result.Where(cond => cond.Code == PaymentTypeCode).FirstOrDefault();
            if (!defaultPaymentMethod.IsNullOrEmpty())
            {
                View.PaymentModeId = defaultPaymentMethod.PaymentOptionID;
                View.PaymentModeCode = defaultPaymentMethod.Code;
            }
        }

        public void GetPaymentModeCode(Int32 selectedPaymentModeId)
        {
            View.PaymentModeCode = ComplianceDataManager.GetPaymentOptionCodeById(selectedPaymentModeId, View.TenantId);
        }

        /// <summary>
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

        public ItemPaymentContract GenerateInvoiceNumber()
        {
            Entity.OrganizationUser organizationUser = new Entity.OrganizationUser();
            //get organizationUser from apporguserID
            organizationUser = SecurityManager.GetOrganizationUser(View.OrganizationUserID);
            organizationUser.SSN = ComplianceSetupManager.GetFormattedString(organizationUser.OrganizationUserID, false, View.TenantId);
            var orgUserProfile = SecurityManager.AddOrganizationUserProfile(organizationUser,null,View.TenantId);
            View.itemPaymentContract.OrganizationUserProfileID = orgUserProfile.OrganizationUserProfileID;
            View.itemPaymentContract.CreatedByID = View.CurrentLoggedInUserId;
            View.itemPaymentContract.OrganizationUserID = View.OrganizationUserID;
            View.itemPaymentContract.TenantID = View.TenantId;
            View.itemPaymentContract.OrderDate = DateTime.Now;
            //----
            View.itemPaymentContract.ApplicantName = orgUserProfile.FirstName + " " + orgUserProfile.LastName;
            View.itemPaymentContract.PrimaryEmailAddress = orgUserProfile.PrimaryEmailAddress;

            //Create order by orguserprofileID-DONE
            //create OnlinePaymentTransaction-DONE
            //create OrderPaymentDetails-DONE
            //Add ACID & ACCD entry
            //new parking log table(itemID,OPDid,ItemTypeID(req./Tracking),PkgSubsID)
            ItemPaymentContract result = null;
            result = ComplianceDataManager.CreateItemPaymentOrder(View.itemPaymentContract);

            //Queue Assignment.
            if (result.IsNotNull() && !result.IsRequirementPackage)
            {
                SaveSystComDeliverySettingForFirstItemSubmit(result);

                var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
                var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;

                Dictionary<String, Object> dicHandleAssignmentData = ComplianceDataManager.SetHandleAssignmentForItemPaymentData(result, true, View.CurrentLoggedInUserId);
                QueueManagementManager.RunParallelTaskHandleAssignment(dicHandleAssignmentData, LoggerService, ExceptiomService, View.TenantId);
            }
            return result;
        }

        public List<Entity.lkpPaymentOption> GetMasterPaymentSettings(out List<Entity.ClientEntity.lkpPaymentOption> lstClientPaymentOptns)
        {
            return ComplianceDataManager.GetMasterPaymentOptions(View.TenantId, out lstClientPaymentOptns);
        }

        #region SEND NOTIFICATION ON FIRST ITEM SUBMITT

        /// <summary>
        /// Save system communication delivert setting for first item submitt
        /// </summary>
        /// <param name="itemData">ApplicantComplianceItemData</param>
        public void SaveSystComDeliverySettingForFirstItemSubmit(ItemPaymentContract itemData)
        {
            CommunicationSubEvents subEventFirstItemSubmission = CommunicationSubEvents.NOTIFICATION_FOR_FIRST_ITEM_SUBMISSION;
            if (!CommunicationManager.IsFirstItemNotificationExist(subEventFirstItemSubmission.GetStringValue(), itemData.OrganizationUserID))
            {

                String applicationUrl = WebSiteManager.GetInstitutionUrl(View.TenantId);
                String institutionName = ClientSecurityManager.GetTenantName(View.TenantId);
                String surveyMonkeyLink = ComplianceSetupManager.GetSurveyMonkeyLink(View.TenantId, View.CurrentLoggedInUserId, subEventFirstItemSubmission.GetStringValue(), itemData.PkgName, itemData.CategoryName, itemData.ItemName, itemData.ItemID, itemData.PkgId, itemData.CategoryID);
                //Create Dictionary
                Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                dictMailData.Add(EmailFieldConstants.USER_FULL_NAME, string.Concat(itemData.ApplicantName));
                //dictMailData.Add(EmailFieldConstants.COMPLIANCE_ITEM_NAME, itemData.ComplianceItem.Name);
                dictMailData.Add(EmailFieldConstants.COMPLIANCE_ITEM_NAME, itemData.ItemName);
                //dictMailData.Add(EmailFieldConstants.CATEGORY_NAME, itemData.ApplicantComplianceCategoryData.ComplianceCategory.CategoryName);
                dictMailData.Add(EmailFieldConstants.CATEGORY_NAME, itemData.CategoryName);
                dictMailData.Add(EmailFieldConstants.PACKAGE_NAME, itemData.PkgName);
                dictMailData.Add(EmailFieldConstants.SURVEY_MONKEY_LINK, surveyMonkeyLink);
                dictMailData.Add(EmailFieldConstants.INSTITUTE_NAME, institutionName);
                dictMailData.Add(EmailFieldConstants.APPLICATION_URL, applicationUrl);

                Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
                mockData.UserName = itemData.ApplicantName;
                mockData.EmailID = itemData.PrimaryEmailAddress;
                mockData.ReceiverOrganizationUserID = itemData.OrganizationUserID;

                //Send mail
                //CommunicationManager.SendPackageNotificationMail(subEventFirstItemSubmission, dictMailData, mockData, View.TenantID, View.HierarchyID.HasValue ? View.HierarchyID.Value : 0);
                //UAT - 1067 : Hierarchy for orders (background and screening) should display as the full hierarchy sleected during the order, not the node the package lives on. 
                CommunicationManager.SendPackageNotificationMail(subEventFirstItemSubmission, dictMailData, mockData, View.TenantId, itemData.HierarchyNodeID.HasValue ? itemData.HierarchyNodeID.Value : 0);


                //Send Message
                CommunicationManager.SaveMessageContent(subEventFirstItemSubmission, dictMailData, View.CurrentLoggedInUserId, View.TenantId);
            }
        }
        #endregion
     
    }
}
