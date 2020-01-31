#region Namespaces

#region SystemDefined

using System;
using System.Linq;
using System.Collections.Generic;
using INTSOF.SharedObjects;

#endregion

#region UserDefined

using Business.RepoManagers;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using Entity;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.UI.Contract.FingerPrintSetup;
using System.Text;


#endregion

#endregion

namespace CoreWeb.ComplianceOperations.Views
{
    public class ApplicantProfilePresenter : Presenter<IApplicantProfileView>
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
            if (View.IsFromArchivedOrderScreen && View.ArchivedOrgUserId > 0 && View.IsLocationServiceTenant)
            {
                View.OrganizationUser = SecurityManager.GetOrganizationUser(View.ArchivedOrgUserId);
            }
            else
            {
                View.OrganizationUser = SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId);
            }
            // TODO: Implement code that will be executed the first time the view loads

        }

        public bool IsExistsEmail(String emailId)
        {
            List<Entity.OrganizationUser> organizationUsers = SecurityManager.GetOrganizationUsersByEmail(emailId);
            if (organizationUsers != null && organizationUsers.Any(x => x.OrganizationUserID != View.CurrentLoggedInUserId))
                return true;
            return false;
        }

        public bool IsExistsSSN(String ssn)
        {
            Entity.OrganizationUser organizationUser = SecurityManager.GetOrganizationUsersBySSN(ssn);

            if (organizationUser != null && organizationUser.OrganizationUserID != View.CurrentLoggedInUserId)
                return true;
            return false;
        }

        public List<Entity.lkpGender> GetGender()
        {
            return SecurityManager.GetGender().ToList();
        }

        public void GetNextPagePathByOrderStageID(ApplicantOrderCart applicantOrderCart)
        {
          
                View.NextPagePath = ComplianceDataManager.GetNextPagePathByOrderStageID(applicantOrderCart, OrderStages.ApplicantProfile);
          
        }

        public Int32 GetTenant()
        {
            Entity.Organization _org = SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization;
            return _org.TenantID.Value;
        }

        /// <summary>
        /// Sends an email to the user regarding email change confirmation.
        /// </summary>
        public Boolean SendVerificationCodeForEmailChange(String verificationCode, Entity.OrganizationUser organizationUser, String newEmailAddress)
        {
            if (SecurityManager.SendVerificationCodeForEmailChange(organizationUser, verificationCode.Trim(), newEmailAddress))
            {
                return SecurityManager.SendAlertForEmailChange(organizationUser);
            }
            return false;
        }

        public List<PreviousAddressContract> GetResidentialHistories(Int32 orgUserId)
        {
            List<ResidentialHistory> tempResidentialAddress = SecurityManager.GetUserResidentialHistories(orgUserId).Where(cond => cond.RHI_IsCurrentAddress != true).ToList();
            List<PreviousAddressContract> tempList = tempResidentialAddress.Where(cond => cond.Address.ZipCodeID > 0).Select(x => new PreviousAddressContract
            {
                ID = x.RHI_ID,
                Address1 = x.Address.Address1,
                Address2 = x.Address.Address2,
                ZipCodeID = x.Address.ZipCodeID,
                ResidenceStartDate = x.RHI_ResidenceStartDate,
                ResidenceEndDate = x.RHI_ResidenceEndDate,
                isNew = false,
                isDeleted = false,
                isUpdated = false,
                CityName = x.Address.ZipCode.City.CityName,
                StateName = x.Address.ZipCode.County.State.StateName,
                Country = x.Address.ZipCode.County.State.Country.FullName,
                CountryId = x.Address.ZipCode.County.State.CountryID.Value,
                Zipcode = x.Address.ZipCode.ZipCode1,
                CountyName = x.Address.ZipCode.County.CountyName,
                ResHistorySeqOrdID = x.RHI_SequenceOrder.IsNotNull() ? x.RHI_SequenceOrder.Value : AppConsts.NONE,
                LicenseNumber = x.RHI_DriverLicenseNumber,
                MotherName = x.RHI_MotherMaidenName,
                IdentificationNumber = x.RHI_IdentificationNumber
            }).ToList();

            tempList.AddRange(tempResidentialAddress.Where(cond => cond.Address.ZipCodeID == 0).Select(x => new PreviousAddressContract
            {
                ID = x.RHI_ID,
                Address1 = x.Address.Address1,
                Address2 = x.Address.Address2,
                ZipCodeID = x.Address.ZipCodeID,
                ResidenceStartDate = x.RHI_ResidenceStartDate,
                ResidenceEndDate = x.RHI_ResidenceEndDate,
                isNew = false,
                isDeleted = false,
                isUpdated = false,
                CityName = x.Address.AddressExts.FirstOrDefault().AE_CityName,
                StateName = x.Address.AddressExts.FirstOrDefault().AE_StateName,
                Country = x.Address.AddressExts.FirstOrDefault().Country.FullName,
                Zipcode = x.Address.AddressExts.FirstOrDefault().AE_ZipCode,
                CountryId = x.Address.AddressExts.FirstOrDefault().Country.CountryID,
                ResHistorySeqOrdID = x.RHI_SequenceOrder.IsNotNull() ? x.RHI_SequenceOrder.Value : AppConsts.NONE,
                IdentificationNumber = x.RHI_IdentificationNumber,
                MotherName = x.RHI_MotherMaidenName,
                LicenseNumber = x.RHI_DriverLicenseNumber
            }).ToList());
            return tempList;
        }

        #endregion

        public void GetAttributeFieldsOfSelectedPackages(String packageIds)
        {
            List<Entity.ClientEntity.AttributeFieldsOfSelectedPackages> lstAttributeFields = BackgroundProcessOrderManager.GetAttributeFieldsOfSelectedPackages(packageIds, View.TenantId);
            if (!lstAttributeFields.IsNullOrEmpty())
            {
                View.lstMvrAttGrp = lstAttributeFields.Where(cond => (cond.BSA_Code.ToUpper().Equals("1ADA97AE-9100-4BE6-B829-C914B7FA8750")
                                                                        || cond.BSA_Code.ToUpper().Equals("515BEF57-9072-4D2A-A97A-0C248BB045F9"))
                                                                       && cond.AttributeGrpCode.ToUpper().Equals("CF76960D-2120-46FE-9E03-01C218F8A336")).ToList();
                View.LstInternationCriminalSrchAttributes = lstAttributeFields.Where(cond => (cond.BSA_Code.ToUpper().Equals("3DA8912A-6337-4B8F-93C4-88BFC3032D2D")
                                                                        || cond.BSA_Code.ToUpper().Equals("AAB51E52-2A9B-42AB-9A9D-D1AFFC18E211")
                                                                        || cond.BSA_Code.ToUpper().Equals("515BEF57-9072-4D2A-A97A-0C248BB045F9"))).ToList();
            }
            else
            {
                View.lstMvrAttGrp = new List<Entity.ClientEntity.AttributeFieldsOfSelectedPackages>();
            }
        }

        public void GetAllStates()
        {
            List<Entity.State> lstStateTemp = BackgroundSetupManager.GetAllStates().Where(x => !x.StateAbbreviation.IsNullOrEmpty()).ToList();
            // lstStateTemp.Insert(0, new Entity.State { StateName = "SELECT ALL", StateID = 0 });
            View.ListStates = lstStateTemp;

        }
        //Int32 GetCustomFormIDBYCode(String customFormCode)
        public Int32 GetCustomFormIDBYCode()
        {
            return BackgroundProcessOrderManager.GetCustomFormIDBYCode(BkgCustomForm.Personal_and_residential_Information.GetStringValue());
        }

        public List<PackageGroupContract> CheckShowResidentialHistory(Int32 tenantId, List<Int32> backgroundPackagesID)
        {
            Guid resHistory_ServiceAttributeGroup = new Guid(ServiceAttributeGroup.RESIDENTIAL_HISTORY.GetStringValue());
            Guid persInformation_SvcAttributeGroup = new Guid(ServiceAttributeGroup.PERSONAL_INFORMATION.GetStringValue());
            List<PackageGroupContract> tempList = BackgroundProcessOrderManager.CheckShowResidentialHistory(tenantId, backgroundPackagesID);
            View.IsPersonalInformationGroupExist = tempList.Any(fx => fx.Code.Equals(persInformation_SvcAttributeGroup));
            return tempList.Where(yx => yx.Code.Equals(resHistory_ServiceAttributeGroup)).DistinctBy(y => y.PackageId).ToList();
        }

        public void GetMinMaxResidentailHistoryOccurances(Int32 tenantId, List<Int32> backgroundPackagesID)
        {
            Dictionary<String, Int32?> minMaxResidentailHistoryOccurances = BackgroundProcessOrderManager.GetMinMaxOccurancesForAttributeGroup(tenantId, backgroundPackagesID, new Guid(ServiceAttributeGroup.RESIDENTIAL_HISTORY.GetStringValue()));

            if (minMaxResidentailHistoryOccurances.ContainsKey("MaxOccurrence") && minMaxResidentailHistoryOccurances.ContainsKey("MinOccurrence"))
            {
                //View.MaxResidentailHistoryOccurances = Convert.ToInt32(minMaxResidentailHistoryOccurances["MaxOccurrence"]);
                //Added a check for nullable value UAT-605
                if (!minMaxResidentailHistoryOccurances["MaxOccurrence"].IsNullOrEmpty())
                {
                    View.MaxResidentailHistoryOccurances = Convert.ToInt32(minMaxResidentailHistoryOccurances["MaxOccurrence"]);
                }
                View.MinResidentailHistoryOccurances = Convert.ToInt32(minMaxResidentailHistoryOccurances["MinOccurrence"]);

                //if (PrevResident.ResidentialHistoryList.Count() < minOccurrence)
                //{
                //    base.ShowInfoMessage("Please Enter the Minimum Residentail History required by this Order.");
                //    return;
                //}
            }
        }


        public void GetMinMaxPaersonalAliasOccurances(Int32 tenantId, List<Int32> backgroundPackagesID)
        {
            Dictionary<String, Int32?> minMaxPaersonalAliasOccurances = BackgroundProcessOrderManager.GetMinMaxOccurancesForAttributeGroup(tenantId, backgroundPackagesID, new Guid(ServiceAttributeGroup.PERSONAL_ALIAS.GetStringValue()));

            if (minMaxPaersonalAliasOccurances.ContainsKey("MaxOccurrence") && minMaxPaersonalAliasOccurances.ContainsKey("MinOccurrence"))
            {
                //View.MaxPersonalAliasOccurances = Convert.ToInt32(minMaxPaersonalAliasOccurances["MaxOccurrence"]);
                //Added a check for nullable value UAT-605
                if (!minMaxPaersonalAliasOccurances["MaxOccurrence"].IsNullOrEmpty())
                {
                    View.MaxPersonalAliasOccurances = Convert.ToInt32(minMaxPaersonalAliasOccurances["MaxOccurrence"]);
                }
                View.MinPersonalAliasOccurances = Convert.ToInt32(minMaxPaersonalAliasOccurances["MinOccurrence"]);
            }
        }
        // ShowInstructionTextForResiHistory
        public void ShowInstructionTextForResiHistory(Int32 tenantId, List<Int32> backgroundPackagesID)
        {
            //return 
            Guid resHistory_ServiceAttributeGroup = new Guid(ServiceAttributeGroup.RESIDENTIAL_HISTORY.GetStringValue());
            Guid persInformation_SvcAttributeGroup = new Guid(ServiceAttributeGroup.PERSONAL_INFORMATION.GetStringValue());
            Dictionary<Guid, String> dicInstructionText = BackgroundProcessOrderManager.ShowInstructionTextForResiHistory(tenantId, backgroundPackagesID);
            if (dicInstructionText.ContainsKey(persInformation_SvcAttributeGroup))
                View.PersonalInformationInstructionText = dicInstructionText[persInformation_SvcAttributeGroup];
            if (dicInstructionText.ContainsKey(resHistory_ServiceAttributeGroup))
                View.ResidentialHistoryInstructionText = dicInstructionText[resHistory_ServiceAttributeGroup];
        }

        public void GetSSNSetting()
        {
            View.IsSSNDisabled = (View.TenantId > 0 && View.TenantId != SecurityManager.DefaultTenantID) ? ComplianceDataManager.GetSSNSetting(View.TenantId, Setting.DISABLE_SSN.GetStringValue()) : false;
        }

        #region Private Methods


        #endregion

        #endregion

        #region UAT-781 ENCRYPTED SSN
        /// <summary>
        /// Method to Get Decrypted SSN
        /// </summary>
        /// <param name="encryptedSSN"></param>
        public void GetDecryptedSSN(Int32 orgUserID, Boolean isOrgUserProfileID)
        {
            View.DecryptedSSN = ComplianceSetupManager.GetFormattedString(orgUserID, isOrgUserProfileID, View.TenantId);
        }
        #endregion

        #region UAT 1438: Enhancement to allow students to select a User Group.

        public bool IsUserGroupCustomAttributeExist(String useTypeCode, Int32 selectedDPMId)
        {
            return ComplianceDataManager.GetCustomAttributesByNodes(useTypeCode, selectedDPMId, View.CurrentLoggedInUserId, View.TenantId)
                .Any(x => x.CADataTypeCode.ToLower().Trim() == CustomAttributeDatatype.User_Group.GetStringValue().ToLower().Trim());
        }

        public void GetAllUserGroup()
        {
            if (View.TenantId > 0)
            {
                View.lstUserGroups = ComplianceSetupManager.GetAllUserGroup(View.TenantId).OrderBy(ex => ex.UG_Name);
            }

        }

        public void GetUserGroupsForUser()
        {
            if (View.TenantId > 0 && View.CurrentLoggedInUserId > 0)
            {
                View.lstUserGroupsForUser = ComplianceDataManager.GetUserGroupsForUser(View.TenantId, View.CurrentLoggedInUserId);
            }
        }
        #endregion

        #region UAT-1578 : Addition of SMS notification
        /// <summary>
        /// get the details of applicant SMS notification on the basis of applicantID.
        /// </summary>
        /// <param name="applicantID"></param>
        public void GetUserSMSNotificationData(Int32 applicantID)
        {
            View.OrganisationUserTextMessageSettingData = SMSNotificationManager.GetSMSDataByApplicantId(applicantID);
        }
        /// <summary>
        /// get the Updated status of subscription from amazon.
        /// </summary>
        /// <returns></returns>
        //public Boolean UpdateSubscriptionStatusFromAmazon(Int32 applicantID, Int32 currentLoggedInUserID)
        //{
        //    return SMSNotificationManager.UpdateSubscriptionStatusFromAmazon(applicantID, currentLoggedInUserID);
        //}
        #endregion

        #region UAT-1834

        /// <summary>
        /// Get Default Node Id for tenant
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public Int32 GetDefaultNodeId(Int32 tenantId)
        {
            return ComplianceDataManager.GetDefaultNodeId(tenantId);
        }

        /// <summary>
        /// Gets the total number of Custom forms available for the selected background packages
        /// </summary>
        /// <param name="packageId"></param>
        /// <returns></returns>
        public Int32 GetCustomFormsCount(String packageId)
        {
            List<CustomFormDataContract> lstCustomForm = BackgroundProcessOrderManager.GetCustomFormsForThePackage(View.TenantId, packageId);
            if (!lstCustomForm.IsNullOrEmpty())
                return lstCustomForm.DistinctBy(cstFrm => cstFrm.customFormId).Count();

            return AppConsts.NONE;
        }

        /// <summary>
        /// Get Last Node Institution Id
        /// </summary>
        /// <param name="selectedDPMId"></param>
        /// <returns></returns>
        public Int32 GetLastNodeInstitutionId(Int32 selectedDPMId)
        {
            return ComplianceDataManager.GetLastNodeInstitutionId(selectedDPMId, View.TenantId);
        }

        /// <summary>
        /// Get the payment options
        /// </summary>
        /// <param name="dpmId">Will be used, in case, when NO Compliance package was selected for the purchase.</param>
        public Boolean IfInvoiceIsOnlyPaymentOptions(Int32 dpmId)
        {
            List<Entity.ClientEntity.lkpPaymentOption> paymentOptions = ComplianceDataManager.GetPaymentOptionsByDPMId(View.TenantId, dpmId);
            if (paymentOptions.Count == 1)
            {
                return paymentOptions.Any(t => t.Code == PaymentOptions.InvoiceWithApproval.GetStringValue() || t.Code == PaymentOptions.InvoiceWithOutApproval.GetStringValue());
            }
            else if (paymentOptions.Count == 2)
            {
                return (paymentOptions.Any(t => t.Code == PaymentOptions.InvoiceWithApproval.GetStringValue()) && paymentOptions.Any(t => t.Code == PaymentOptions.InvoiceWithOutApproval.GetStringValue()));
            }
            return false;
        }

        /// <summary>
        /// Get Additional Documents
        /// </summary>
        /// <param name="lstBkgPackages"></param>
        /// <param name="selectedHierarchyId"></param>
        /// <param name="CompliancePackages"></param>
        /// <param name="isCompliancePackageSelected"></param>
        /// <returns></returns>
        public Boolean GetAdditionalDocuments(List<BackgroundPackagesContract> lstBkgPackages, Int32 selectedHierarchyId,
                                            Dictionary<String, OrderCartCompliancePackage> CompliancePackages, Boolean isCompliancePackageSelected)
        {
            var packagesList = lstBkgPackages;
            List<Int32> BkgPackages = new List<Int32>();
            List<Int32> compPackageIdList = new List<Int32>();
            Boolean isAdditionalDocumentExist = false;
            if (!packagesList.IsNullOrEmpty())
            {
                foreach (var item in packagesList)
                {
                    BkgPackages.Add(item.BPAId);
                }
            }

            List<Entity.SystemDocument> additionalDocument = BackgroundSetupManager.GetAdditionalDocuments(BkgPackages, compPackageIdList, selectedHierarchyId, View.TenantId);
            if (!additionalDocument.IsNullOrEmpty())
            {
                isAdditionalDocumentExist = true;
            }
            return isAdditionalDocumentExist;
        }

        /// <summary>
        /// Get Background Package
        /// </summary>
        /// <returns></returns>
        public BackgroundPackagesContract GetBackgroundPackage()
        {
            return BackgroundSetupManager.GetBackgroundPackageByPkgIDAndNodeID(View.TenantId, View.BkgPackageID, View.OrderNodeID, View.HierarchyNodeID);
        }

        #endregion

        //UAT-3133
        public Boolean IsIntegrationClientOrganisationUser(Int32 orgUserID)
        {
            return SecurityManager.IsIntegrationClientOrganisationUser(orgUserID, AppConsts.MAPPING_GROUP_CODE_UCONN);
        }


        #region UAT-3545 CBI || CABS-- Validation Expression --- Location Service Tenant
        //public List<Entity.ClientEntity.lkpSvcAttributeDataType> GetServiceAttributeDataType()
        //{
        //    List<Entity.ClientEntity.lkpSvcAttributeDataType> lstServiceAttributeDataTypes = BackgroundSetupManager.GetServiceAttributeDatatype(View.TenantId);
        //    return lstServiceAttributeDataTypes;
        //}

        public void GetPersonalInformationExpressions()
        {
            View.lstValidateRegexDataContract = new List<ValidateRegexDataContract>();
            View.lstValidateRegexDataContract = FingerPrintDataManager.GetPersonalInformationExpressions(View.TenantId, View.BkgPackageID,View.LanguageCode);
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

        public String validatePageData(StringBuilder xmlStringData)
        {
            return BackgroundProcessOrderManager.ValidatePageData(View.TenantId, xmlStringData, false);
        }
        #endregion


        public void GetCommLang(Guid UserID)
        {
            View.CommLanguage = SecurityManager.GetCommLang().ToList();
            View.SelectedCommLang = SecurityManager.GetSelectedlang(UserID);
        }

        public int GetSuffixIdBasedOnSuffixText(string suffix)
        {
            return SecurityManager.GetSuffixIdBasedOnSuffixText(suffix);
        }

        public void IsDropDownSuffixType()
        {
            AppConfiguration appConfig = SecurityManager.GetAppConfiguration(AppConsts.SUFFIX_TYPE.ToString());
            View.IsSuffixDropDownType= !appConfig.IsNullOrEmpty() && appConfig.AC_Value == "1" ? true : false;
        }

        public void GetMailingOption()
        {
            var FeeItemId = SecurityManager.GetFeeItemID();
            View.lstMailingOptionsWithPrice = BackgroundPricingManager.GetServiceItemFeeRecordContract(SecurityManager.DefaultTenantID, FeeItemId);

        }

        public Boolean IsCountryUSACanadaMexico(Int32 RSLCountryIdLocationServiceTenant)
        {
            return SecurityManager.IsCountryUSACanadaMexico(RSLCountryIdLocationServiceTenant);
        }

        #region UAT-4731 : Restrict Applicant To One User Group In Order Process
        public Boolean IsMultiUserGroupSelectionAllowed()
        {
            var restrictAppToOneUGInOrderProcessSetting = ComplianceDataManager.GetClientSetting(View.TenantId, Setting.RESTRICT_APPLICANT_TO_ONE_USER_GROUP_IN_ORDER_PROCESS.GetStringValue());
            if (restrictAppToOneUGInOrderProcessSetting != null)
            {
                return restrictAppToOneUGInOrderProcessSetting.CS_SettingValue == "1" ? false : true;
            }
            return true;
        }
        #endregion
    }
}





