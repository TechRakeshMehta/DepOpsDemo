using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Entity;
using Business.RepoManagers;
using System.Linq;
using INTSOF.Utils;
using System.Reflection;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils.Consts;


namespace CoreWeb.ApplicantModule.Views
{
    public class EditProfilePresenter : Presenter<IEditProfileView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IApplicantModuleController _controller;
        // public EditProfilePresenter([CreateNew] IApplicantModuleController controller)
        // {
        // 		_controller = controller;
        // }

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
            // View.AdminProgramStudies = new List<AdminProgramStudy>();
            GetGranularPermissionForDOBandSSN();
        }

        public void GetDepartments()
        {
            Organization organization = SecurityManager.GetOrganizationForTenantID(View.SelectedTenantId);
            //View.Departments = SecurityManager.GetDepartments(organization.OrganizationID).ToList();
        }

        public List<lkpGender> GetGenderList()
        {
            return SecurityManager.GetGender().ToList();
        }

        public OrganizationUser GetUserData()
        {
            OrganizationUser organizationUser = SecurityManager.GetOrganizationUser(View.loggedInUserId);
            return organizationUser;
        }
        //public aspnet_Membership GetEmailID()
        //{
        //    aspnet_Membership membership = new aspnet_Membership();
        //    return membership;
        //}

        public Int32 GetTenantID()
        {
            var orgUserData = SecurityManager.GetOrganizationUser(View.loggedInUserId);
            if (!orgUserData.IsNullOrEmpty() && !orgUserData.Organization.IsNullOrEmpty())
            {
                return orgUserData.Organization.TenantID.Value;
            }
            else
            {
                return AppConsts.NONE;
            }

        }

        public void UpdateAddress(Address adress)
        {
            adress.Address1 = View.Address1;
            adress.Address2 = View.Address2;
            adress.ZipCodeID = View.ZipId;
            adress.CreatedByID = 0;
            adress.IsActive = true;
            adress.CreatedOn = DateTime.Now;
        }

        /// <summary>
        /// To save user profile data
        /// </summary>
        /// <returns></returns>
        public Boolean SaveUserData()
        {
            OrganizationUser organizationUser = SecurityManager.GetOrganizationUser(View.loggedInUserId);
            //var orgUserPrevious = organizationUser.DeepClone();

            organizationUser.aspnet_Users.UserName = View.UserName;
            organizationUser.aspnet_Users.LoweredUserName = View.UserName.ToLower();
            organizationUser.aspnet_Users.MobileAlias = View.PrimaryPhone;
            organizationUser.Organization = SecurityManager.GetOrganizationForTenantID(View.SelectedTenantId);
            organizationUser.FirstName = View.FirstName;
            organizationUser.LastName = View.LastName;
            organizationUser.MiddleName = View.MiddleName;
            organizationUser.IsApplicant = true;
            //UAT-2447
            organizationUser.IsInternationalPhoneNumber = View.IsInternationalPhoneNumber;
            organizationUser.IsInternationalSecondaryPhone = View.IsInternationalSecondaryPhone;
            //UAT-1261: WB: As an ADB admin, I should be able to "login" as any student to see what they see.
            //organizationUser.ModifiedByID = View.loggedInUserId;
            //organizationUser.CreatedByID = View.loggedInUserId;
            organizationUser.ModifiedByID = View.OrgUsrID;

            organizationUser.ModifiedOn = DateTime.Now;
            organizationUser.DOB = View.DOB;
            organizationUser.Gender = View.SelectedGenderId;
            organizationUser.PhoneNumber = View.PrimaryPhone;
            organizationUser.SecondaryPhone = View.SecondaryPhone;
            if (!View.IsApplicant)
            {
                if (View.UpdateAspnetEmail)
                {
                    organizationUser.aspnet_Users.aspnet_Membership.Email = View.PrimaryEmail;
                    organizationUser.aspnet_Users.aspnet_Membership.LoweredEmail = View.PrimaryEmail.ToLower();
                    View.PswdRecoveryEmail = View.PrimaryEmail.ToLower();
                }
                organizationUser.PrimaryEmailAddress = View.PrimaryEmail;
            }
            else if (View.UpdateAspnetEmail && View.PrimaryEmail.ToLower() == View.CurrentEmailAddress.ToLower())
            {
                organizationUser.aspnet_Users.aspnet_Membership.Email = View.PrimaryEmail;
                organizationUser.aspnet_Users.aspnet_Membership.LoweredEmail = View.PrimaryEmail.ToLower();
                View.PswdRecoveryEmail = View.PrimaryEmail.ToLower();
            }
            organizationUser.SecondaryEmailAddress = View.SecondaryEmail;
            organizationUser.SSN = View.SSN;

            //CBI || CABS || Added Suffix iD In UserTypeID
            //     organizationUser.UserTypeID = View.SelectedSuffixID.IsNullOrEmpty() ? (Int32?)null : View.SelectedSuffixID;
            //organizationUser.Suffix = View.Suffix;
            if (View.Suffix.IsNullOrEmpty() && (View.SelectedSuffixID == 0 || View.SelectedSuffixID == null))
            {
                organizationUser.UserTypeID = null;
            }
            else
            {
                if (View.IsSuffixDropDownType)
                {
                    organizationUser.UserTypeID = View.SelectedSuffixID.Value;
                }
                else
                {
                    organizationUser.UserTypeID = GetSuffixIdBasedOnSuffixText(View.Suffix);
                }
            }
            //UAT-3084
            View.IsPersonAliasAddUpdate = IsPersonAliasAddedUpdated(organizationUser);

            //Adds and updates the Person Alias.
            AddUpdatePersonAlias(organizationUser);

            Address addressNew = null;
            AddressExt addressExtNew = null;
            //Check if Address Handle is not null
            if (organizationUser.AddressHandle.IsNotNull())
            {
                var address = organizationUser.AddressHandle.Addresses.FirstOrDefault();
                //Check if Address not null
                if (address.IsNotNull())
                {
                    //Check if current address has been modified.
                    if (CheckIfAddressUpdated(address))
                    {
                        addressNew = new Address();
                        Dictionary<String, Object> dicAddressData = GetAddressDataDictionary();
                        //UAT-3910
                        if (View.IsLocationServiceTenant)
                        {
                            addressExtNew = new AddressExt();
                            addressExtNew.AE_CountryID = AppConsts.ONE;//UAT-3910 
                            addressExtNew.AE_StateName = View.StateName;
                            addressExtNew.AE_CityName = View.CityName;
                            addressExtNew.AE_ZipCode = View.PostalCode;
                            addressExtNew.AE_County = View.IsLocationServiceTenant ? Convert.ToString(View.CountryId) : null;//UAT-3910
                        }
                        else
                        {
                            if (View.ZipId == 0)
                            {
                                addressExtNew = new AddressExt();
                                addressExtNew.AE_CountryID = View.CountryId;
                                addressExtNew.AE_StateName = View.StateName;
                                addressExtNew.AE_CityName = View.CityName;
                                addressExtNew.AE_ZipCode = View.PostalCode;
                            }
                        }
                        Guid addressHandleId = Guid.NewGuid();
                        SecurityManager.AddAddressHandle(addressHandleId);
                        SecurityManager.AddAddress(dicAddressData, addressHandleId, View.OrgUsrID, addressNew, addressExtNew);

                        SecurityManager.UpdateChanges();
                        ClientSecurityManager.AddAddressHandle(View.SelectedTenantId, addressHandleId);
                        ClientSecurityManager.AddAddress(View.SelectedTenantId, dicAddressData, addressHandleId, View.OrgUsrID, addressNew, addressExtNew);
                        organizationUser.AddressHandleID = addressHandleId;
                        organizationUser.AddressHandle.Addresses.Add(addressNew);
                    }
                }
            }
            ////Adds and updates the residential histories.
            AddUpdateResidentialHistory(organizationUser, addressNew);

            if (View.FilePath != null)
            {
                organizationUser.PhotoName = View.FilePath;
            }
            if (View.OriginalFileName != null)
            {
                organizationUser.OriginalPhotoName = View.OriginalFileName;
            }
            UserAuthRequest tempUserAuthRequest = null;
            OrganizationUserProfile organizationUserProfile = SecurityManager.AddOrganizationUserProfile(organizationUser, View.SelectedCommLang);
            #region UAT-1578:Addition of SMS notification
            //SaveSMSNotificationData();
            #endregion
            if (View.PrimaryEmail.ToLower() != View.CurrentEmailAddress.ToLower() && View.IsApplicant)
            {
                Int16 authTypeId = SecurityManager.GetAuthRequestTypeIdByCode(AuthRequestType.Email_Confirmation.GetStringValue());
                tempUserAuthRequest = SecurityManager.GenerateEmailConfirmationReq(View.loggedInUserId, organizationUserProfile.OrganizationUserProfileID, View.PrimaryEmail, View.UpdateAspnetEmail, View.OrgUsrID, authTypeId);
                if (tempUserAuthRequest.IsNotNull())
                {
                    //Get Website Url
                    Entity.WebSite webSite = WebSiteManager.GetWebSiteDetail(organizationUser.Organization.Tenant.TenantID);
                    String applicationUrl = String.Empty;
                    if (webSite.IsNotNull() && webSite.WebSiteID != SysXDBConsts.NONE)
                    {
                        applicationUrl = webSite.URL;
                    }
                    else
                    {
                        webSite = WebSiteManager.GetWebSiteDetail(SecurityManager.DefaultTenantID);
                        applicationUrl = webSite.URL;
                    }
                    applicationUrl = applicationUrl + "/Login.aspx?AuthReqVerCode=" + tempUserAuthRequest.UAR_VerificationCode;
                    if (!SecurityManager.SendEmailForEmailChange(organizationUser, applicationUrl, View.PrimaryEmail))
                    {
                        View.ErrorMessage = "Some error has occured.Please contact administrator.";
                    }
                    else
                    {
                        View.SuccessMessage = "An email has been sent with a verification link to your email, " + View.PrimaryEmail.ToLower() + ". Please click the link to update your primary email address.";
                    }
                    SecurityManager.SendAlertForEmailChange(organizationUser);
                }
            }
            if (SecurityManager.UpdateOrganizationUser(organizationUser))
            {
                SecurityManager.SynchoniseUserProfile(organizationUser.OrganizationUserID, View.SelectedTenantId, View.OrgUsrID);
                //List<string> lstChangedProperties = GetChangedProperties(orgUserPrevious, organizationUser, addressNew);
                //if (!lstChangedProperties.IsNullOrEmptyCollection() && lstChangedProperties.Count > 0)
                CommunicationManager.SendMailOnProfileChange(organizationUser, View.SelectedTenantId);
                return true;
            }
            else
            {
                return false;
            }
        }

        private Boolean CheckIfAddressUpdated(Address objAddress)
        {
            if (objAddress.Address1.ToLower().Trim() != View.Address1.ToLower().Trim()
                 || objAddress.Address2.ToLower().Trim() != View.Address2.ToLower().Trim()
                 || objAddress.ZipCodeID != View.ZipId)
            {
                return true;
            }
            var addressExt = objAddress.AddressExts.FirstOrDefault();
            if (addressExt.IsNotNull())
            {
                if (addressExt.AE_CountryID != View.CountryId
                 || addressExt.AE_StateName.ToLower().Trim() != View.StateName.ToLower().Trim()
                 || addressExt.AE_CityName.ToLower().Trim() != View.CityName.ToLower().Trim()
                 || addressExt.AE_ZipCode.ToLower().Trim() != View.PostalCode.ToLower().Trim())
                {
                    return true;
                }
            }
            return false;
        }

        private void AddUpdatePersonAlias(OrganizationUser organizationUser)
        {
            if (View.PersonAliasList.IsNotNull())
            {
                List<PersonAlia> currentAliasList = organizationUser.PersonAlias.Where(x => x.PA_IsDeleted == false).ToList();
                foreach (PersonAliasContract tempPersonAlias in View.PersonAliasList)
                {
                    if (tempPersonAlias.ID > 0)
                    {
                        PersonAlia personAlias = currentAliasList.FirstOrDefault(x => x.PA_ID == tempPersonAlias.ID);
                        if (personAlias.IsNotNull())
                        {
                            personAlias.PA_FirstName = tempPersonAlias.FirstName;
                            personAlias.PA_LastName = tempPersonAlias.LastName;
                            //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                            personAlias.PA_MiddleName = tempPersonAlias.MiddleName.IsNullOrEmpty() ? View.NoMiddleNameText : tempPersonAlias.MiddleName;
                            personAlias.PA_ModifiedBy = View.OrgUsrID;
                            personAlias.PA_ModifiedOn = DateTime.Now;
                            personAlias.PA_CreatedBy = View.OrgUsrID;

                            //CBI || CABS
                            if (View.IsLocationServiceTenant)
                            {
                                if (!personAlias.PersonAliasExtensions.IsNullOrEmpty() && personAlias.PersonAliasExtensions.Any(Cond => !Cond.PAE_IsDeleted))
                                {
                                    PersonAliasExtension personAliasExtension = personAlias.PersonAliasExtensions.FirstOrDefault(Cond => !Cond.PAE_IsDeleted);
                                    if (!personAliasExtension.IsNullOrEmpty())
                                    {
                                        if (!tempPersonAlias.IsNullOrEmpty())//&& !tempPersonAlias.Suffix.IsNullOrEmpty())
                                        {
                                            personAliasExtension.PAE_Suffix = tempPersonAlias.Suffix;
                                            personAliasExtension.PAE_ModifiedBy = View.OrgUsrID;
                                            personAliasExtension.PAE_ModifiedOn = DateTime.Now;
                                        }
                                        else
                                        {
                                            personAliasExtension.PAE_IsDeleted = true;
                                            personAliasExtension.PAE_ModifiedBy = View.OrgUsrID;
                                            personAliasExtension.PAE_ModifiedOn = DateTime.Now;
                                        }
                                    }
                                }
                                else
                                {
                                    if (!tempPersonAlias.IsNullOrEmpty() && !tempPersonAlias.Suffix.IsNullOrEmpty())
                                    {
                                        PersonAliasExtension personAliasExtension = new PersonAliasExtension();
                                        personAliasExtension.PAE_Suffix = tempPersonAlias.Suffix;
                                        personAliasExtension.PAE_CreatedBy = View.OrgUsrID;
                                        personAliasExtension.PAE_CreatedOn = DateTime.Now;
                                        personAlias.PersonAliasExtensions.Add(personAliasExtension);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        PersonAlia personAlias = new PersonAlia();
                        personAlias.PA_FirstName = tempPersonAlias.FirstName;
                        personAlias.PA_LastName = tempPersonAlias.LastName;
                        //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                        personAlias.PA_MiddleName = tempPersonAlias.MiddleName.IsNullOrEmpty() ? View.NoMiddleNameText : tempPersonAlias.MiddleName;
                        personAlias.PA_IsDeleted = false;
                        personAlias.PA_CreatedBy = View.OrgUsrID;
                        personAlias.PA_CreatedOn = DateTime.Now;
                        personAlias.PA_AliasIdentifier = Guid.NewGuid();
                        organizationUser.PersonAlias.Add(personAlias);

                        //CBI || CABS
                        if (View.IsLocationServiceTenant && !tempPersonAlias.IsNullOrEmpty() && !tempPersonAlias.Suffix.IsNullOrEmpty())
                        {
                            PersonAliasExtension personAliasExtension = new PersonAliasExtension();
                            personAliasExtension.PAE_Suffix = !tempPersonAlias.IsNullOrEmpty() && !tempPersonAlias.Suffix.IsNullOrEmpty() ? tempPersonAlias.Suffix : null;
                            personAliasExtension.PAE_CreatedBy = View.OrgUsrID;
                            personAliasExtension.PAE_CreatedOn = DateTime.Now;
                            personAlias.PersonAliasExtensions.Add(personAliasExtension);
                        }

                    }
                }
                List<Int32> aliasIDToBeDeleted = currentAliasList.Select(x => x.PA_ID).Except(View.PersonAliasList.Select(y => y.ID)).ToList();
                foreach (Int32 delAliasID in aliasIDToBeDeleted)
                {
                    PersonAlia delAlias = currentAliasList.FirstOrDefault(x => x.PA_IsDeleted == false && x.PA_ID == delAliasID);
                    delAlias.PA_IsDeleted = true;
                    delAlias.PA_ModifiedBy = View.OrgUsrID;
                    delAlias.PA_ModifiedOn = DateTime.Now;

                    //to delete 
                    if (View.IsLocationServiceTenant && !delAlias.PersonAliasExtensions.IsNullOrEmpty())
                    {
                        PersonAliasExtension delPersonAliasExtension = delAlias.PersonAliasExtensions.Where(cond => !cond.PAE_IsDeleted).FirstOrDefault();
                        if (!delPersonAliasExtension.IsNullOrEmpty())
                        {
                            delPersonAliasExtension.PAE_IsDeleted = true;
                            delPersonAliasExtension.PAE_ModifiedBy = AppConsts.NONE;
                            delPersonAliasExtension.PAE_ModifiedOn = DateTime.Now;
                        }
                    }
                }


            }
        }

        #region UAT-3084
        private Boolean IsPersonAliasAddedUpdated(OrganizationUser organizationUser)
        {
            if (View.PersonAliasList.IsNotNull())
            {
                List<PersonAlia> currentAliasList = organizationUser.PersonAlias.Where(x => x.PA_IsDeleted == false).ToList();
                foreach (PersonAliasContract tempPersonAlias in View.PersonAliasList)
                {
                    if (tempPersonAlias.ID > 0)
                    {
                        PersonAlia personAlias = currentAliasList.FirstOrDefault(x => x.PA_ID == tempPersonAlias.ID);
                        if (personAlias.IsNotNull())
                        {
                            if ((tempPersonAlias.FirstName.Trim().ToLower() != personAlias.PA_FirstName.Trim().ToLower())
                                || (tempPersonAlias.MiddleName.Trim().ToLower() != personAlias.PA_MiddleName.Trim().ToLower())
                                || (tempPersonAlias.LastName.Trim().ToLower() != personAlias.PA_LastName.Trim().ToLower()))
                            {
                                return true;
                            }

                        }
                    }
                    else
                    {
                        return true;
                    }

                }
                return false;
            }
            return false;
        }

        public List<RejectedItemListContract> GetRejectedItemListForReSubmission()
        {
            return ComplianceDataManager.GetRejectedItemListForReSubmission(View.SelectedTenantId, View.loggedInUserId);

        }

        #endregion

        private void AddUpdateResidentialHistory(OrganizationUser organizationUser, Address addressNew)
        {
            //Current residential Address
            ResidentialHistory currentResedentialHistory = organizationUser.ResidentialHistories.FirstOrDefault(x => x.RHI_IsCurrentAddress == true && x.RHI_IsDeleted == false);
            if (currentResedentialHistory.IsNotNull())
            {
                currentResedentialHistory.Address = addressNew == null ? organizationUser.AddressHandle.Addresses.FirstOrDefault() : addressNew;
                currentResedentialHistory.RHI_ResidenceStartDate = View.DateResidentFrom;
                currentResedentialHistory.RHI_ModifiedByID = View.OrgUsrID;
                currentResedentialHistory.RHI_CreatedByID = View.OrgUsrID;
                currentResedentialHistory.RHI_SequenceOrder = AppConsts.ONE;
                currentResedentialHistory.RHI_ModifiedOn = DateTime.Now;
            }
            else
            {
                currentResedentialHistory = new ResidentialHistory();
                currentResedentialHistory.RHI_IsCurrentAddress = true;
                currentResedentialHistory.RHI_IsPrimaryResidence = false;
                currentResedentialHistory.RHI_ResidenceStartDate = View.DateResidentFrom;
                currentResedentialHistory.RHI_IsDeleted = false;
                currentResedentialHistory.RHI_CreatedByID = View.OrgUsrID;
                currentResedentialHistory.RHI_CreatedOn = DateTime.Now;
                currentResedentialHistory.RHI_OrganizationUserID = organizationUser.OrganizationUserID;
                currentResedentialHistory.RHI_SequenceOrder = AppConsts.ONE;
                currentResedentialHistory.Address = addressNew == null ? organizationUser.AddressHandle.Addresses.FirstOrDefault() : addressNew;
                organizationUser.ResidentialHistories.Add(currentResedentialHistory);
            }
            //List of residential histories.
            List<PreviousAddressContract> previousAddressList = View.ResidentialHistoryList;
            if (previousAddressList.IsNotNull())
            {
                previousAddressList = previousAddressList.Where(x => x.isDeleted == true || x.isNew == true || x.isUpdated == true).ToList();
                if (previousAddressList.Count > 0)
                {
                    // List of Resedential Histories associated with the organisaion User ID.
                    List<ResidentialHistory> lstResedentialHistory = organizationUser.ResidentialHistories.Where(x => x.RHI_IsDeleted == false).ToList();
                    // List of Resedential Histories to be deleted.
                    List<Int32> lstResHisIDToBeDel = previousAddressList.Where(x => x.isDeleted == true).Select(y => y.ID).ToList();
                    List<ResidentialHistory> lstResHisToBeDel = lstResedentialHistory.Where(x => lstResHisIDToBeDel.Contains(x.RHI_ID)).ToList();
                    foreach (var prevAddress in lstResHisToBeDel)
                    {
                        prevAddress.RHI_IsDeleted = true;
                        prevAddress.RHI_ModifiedByID = View.OrgUsrID;
                        prevAddress.RHI_ModifiedOn = DateTime.Now;
                    }

                    // List of Resedential Histories to be added.
                    List<PreviousAddressContract> lstResHisIDToBeAdded = previousAddressList.Where(x => x.isNew == true).ToList();
                    foreach (var prevAddress in lstResHisIDToBeAdded)
                    {
                        Address addressPervious = AddNewPreviousAddress(prevAddress);

                        ResidentialHistory newResidentialHistory = new ResidentialHistory();
                        newResidentialHistory.RHI_AddressId = addressPervious.AddressID;
                        newResidentialHistory.RHI_ResidenceStartDate = prevAddress.ResidenceStartDate;
                        newResidentialHistory.RHI_ResidenceEndDate = prevAddress.ResidenceEndDate;
                        newResidentialHistory.RHI_IsCurrentAddress = false;
                        newResidentialHistory.RHI_IsDeleted = false;
                        newResidentialHistory.RHI_CreatedByID = View.OrgUsrID;
                        newResidentialHistory.RHI_CreatedOn = DateTime.Now;
                        newResidentialHistory.RHI_SequenceOrder = prevAddress.ResHistorySeqOrdID;
                        organizationUser.ResidentialHistories.Add(newResidentialHistory);
                    }

                    // List of Resedential Histories to be updated.
                    List<PreviousAddressContract> lstResHisToBeUpdated = previousAddressList.Where(x => x.isUpdated == true).ToList();
                    foreach (var prevAddress in lstResHisToBeUpdated)
                    {
                        ResidentialHistory resHistory = lstResedentialHistory.FirstOrDefault(x => x.RHI_ID == prevAddress.ID && x.RHI_IsDeleted == false);
                        if (resHistory.IsNotNull())
                        {
                            if (CheckIfPreviousAddressUpdated(resHistory.Address, prevAddress))
                            {
                                Address addressPervious = AddNewPreviousAddress(prevAddress);
                                resHistory.RHI_AddressId = addressPervious.AddressID;
                            }
                            resHistory.RHI_ResidenceStartDate = prevAddress.ResidenceStartDate;
                            resHistory.RHI_ResidenceEndDate = prevAddress.ResidenceEndDate;
                            resHistory.Address.ModifiedByID = View.OrgUsrID;
                            resHistory.RHI_ModifiedByID = View.OrgUsrID;
                            resHistory.RHI_CreatedByID = View.OrgUsrID;
                            resHistory.Address.ModifiedOn = DateTime.Now;
                            resHistory.RHI_ModifiedOn = DateTime.Now;
                            resHistory.RHI_SequenceOrder = prevAddress.ResHistorySeqOrdID;
                        }
                    }
                }
            }
        }

        private Boolean CheckIfPreviousAddressUpdated(Address objAddress, PreviousAddressContract prevAddress)
        {
            if (objAddress.Address1.ToLower().Trim() != prevAddress.Address1.ToLower().Trim()
                 || objAddress.Address2.ToLower().Trim() != prevAddress.Address2.ToLower().Trim()
                 || objAddress.ZipCodeID != prevAddress.ZipCodeID)
            {
                return true;
            }
            var addressExt = objAddress.AddressExts.FirstOrDefault();
            if (addressExt.IsNotNull())
            {
                if (addressExt.AE_CountryID != prevAddress.CountryId
                 || addressExt.AE_StateName.ToLower().Trim() != prevAddress.StateName.ToLower().Trim()
                 || addressExt.AE_CityName.ToLower().Trim() != prevAddress.CityName.ToLower().Trim()
                 || addressExt.AE_ZipCode.ToLower().Trim() != prevAddress.Zipcode.ToLower().Trim())
                {
                    return true;
                }
            }
            return false;
        }

        private Address AddNewPreviousAddress(PreviousAddressContract prevAddress)
        {
            Address addressPervious = new Address();
            AddressExt addressExtPervious = null;
            Dictionary<String, Object> dicAddressData = GetPrevAddressDataDictionary(prevAddress);
            if (prevAddress.ZipCodeID == 0)
            {
                addressExtPervious = new AddressExt();
                addressExtPervious.AE_CountryID = prevAddress.CountryId;
                addressExtPervious.AE_StateName = prevAddress.StateName;
                addressExtPervious.AE_CityName = prevAddress.CityName;
                addressExtPervious.AE_ZipCode = prevAddress.Zipcode;
            }
            Guid addressHandleId = Guid.NewGuid();
            SecurityManager.AddAddressHandle(addressHandleId);
            SecurityManager.AddAddress(dicAddressData, addressHandleId, View.OrgUsrID, addressPervious, addressExtPervious);
            SecurityManager.UpdateChanges();
            ClientSecurityManager.AddAddressHandle(View.SelectedTenantId, addressHandleId);
            ClientSecurityManager.AddAddress(View.SelectedTenantId, dicAddressData, addressHandleId, View.OrgUsrID, addressPervious, addressExtPervious);
            return addressPervious;
        }

        private List<string> GetChangedProperties(OrganizationUser orgUserPrevious, OrganizationUser organizationUser, Address addressNew)
        {
            List<string> notToinclude = new List<string>(new string[] { "AddressHandleID", "AddressID", "EntityState", "CreatedOn", "lkpGender",
                "Organization", "lkpAddressType", "ZipCode", "EntityState", "CreatedByID", "ModifiedByID", "ModifiedOn" });
            List<string> lstProperties = new List<string>();
            var properties = orgUserPrevious.GetType().GetProperties();
            foreach (PropertyInfo prop in properties)
            {
                String propName = prop.Name;
                //if (propName != "CreatedByID" && propName != "CreatedOn" && propName != "ModifiedByID" && propName != "ModifiedOn")
                if (!notToinclude.Contains(propName))
                {
                    //Get Property from both objects
                    var prevPropValue = prop.GetValue(orgUserPrevious, null) ?? string.Empty;
                    var newPropValue = prop.GetValue(organizationUser, null) ?? string.Empty;

                    //Check if the property values are not equal
                    if (prevPropValue.ToString() != newPropValue.ToString())
                        lstProperties.Add(propName);
                }
            }
            if (orgUserPrevious.AddressHandle.IsNotNull())
            {
                Address address = orgUserPrevious.AddressHandle.Addresses.FirstOrDefault();
                var propertiesAddress = address.GetType().GetProperties();
                foreach (PropertyInfo prop in propertiesAddress)
                {
                    //PropertyInfo propInfo = prop.GetType().GetProperty();
                    String propName = prop.Name;
                    if (!notToinclude.Contains(propName))
                    {
                        //Get Property from both objects
                        var prevPropValue = prop.GetValue(address, null) ?? string.Empty;
                        var newPropValue = prop.GetValue(addressNew, null) ?? string.Empty;

                        //Check if the property values are not equal
                        if (prevPropValue.ToString() != newPropValue.ToString())
                            lstProperties.Add(propName);
                    }
                }
            }
            //lstProperties = lstProperties.Intersect(notToinclude).ToList();
            return lstProperties;
        }

        public void AddApplicantUploadedDocuments()
        {
            ComplianceDataManager.AddApplicantUploadedDocuments(View.ToSaveApplicantUploadedDocuments, SecurityManager.GetOrganizationUser(View.loggedInUserId).Organization.TenantID.Value);
        }

        public Dictionary<String, Object> GetAddressDataDictionary()
        {
            Dictionary<String, Object> dicAddressData = new Dictionary<String, Object>();
            dicAddressData.Add("address1", View.Address1);
            dicAddressData.Add("address2", View.Address2);
            dicAddressData.Add("zipcodeid", View.ZipId);
            return dicAddressData;
        }

        public Dictionary<String, Object> GetPrevAddressDataDictionary(PreviousAddressContract prevAddress)
        {
            Dictionary<String, Object> dicAddressData = new Dictionary<String, Object>();
            dicAddressData.Add("address1", prevAddress.Address1);
            dicAddressData.Add("address2", prevAddress.Address2);
            dicAddressData.Add("zipcodeid", prevAddress.ZipCodeID);
            return dicAddressData;
        }

        /// <summary>
        /// Method to update the Profile Photo.
        /// </summary>
        /// <returns>Boolean</returns>
        public Boolean SaveProfilePhoto()
        {
            OrganizationUser organizationUser = SecurityManager.GetOrganizationUser(View.loggedInUserId);
            organizationUser.ModifiedByID = View.OrgUsrID;
            organizationUser.ModifiedOn = DateTime.Now;
            if (View.FilePath != null)
            {
                organizationUser.PhotoName = View.FilePath;
            }
            if (View.OriginalFileName != null)
            {
                organizationUser.OriginalPhotoName = View.OriginalFileName;
            }

            if (SecurityManager.SyncUsersProfilePictureInAllTenant(organizationUser))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// To check if Username exists
        /// </summary>
        /// <returns></returns>
        public bool IsExistsUserName(Guid userId)
        {
            System.Web.Security.MembershipUser user = System.Web.Security.Membership.GetUser(View.UserName);
            if (user.IsNotNull() && user.ProviderUserKey.ToString() == userId.ToString())
                return false;
            return user != null;
        }

        /// <summary>
        /// To check if User exists
        /// </summary>
        /// <returns></returns>
        public Boolean IsExistingUser()
        {
            List<LookupContract> lstExistingUser = SecurityManager.GetExistingUserLists(View.UserName, View.DOB == null ? DateTime.Now : View.DOB.Value, View.SSN, View.FirstName, View.LastName);
            if (lstExistingUser.IsNotNull() && lstExistingUser.Any())
            {
                var _tempUserList = lstExistingUser.Where(x => x.UserID != View.loggedInUserId).ToList();
                if (_tempUserList.Any(x => x.Code.Trim().ToLower() == View.UserName.Trim().ToLower()))
                    return true;
                else
                    return false;
            }
            return false;
        }

        /// <summary>
        /// To check if Primary email exists
        /// </summary>
        /// <returns></returns>
        public Boolean IsExistsPrimaryEmail(Guid userId)
        {
            //var organizationUserList = SecurityManager.GetOrganizationUsersByEmail(View.PrimaryEmail);
            //if (organizationUserList.IsNotNull() && organizationUserList.Any())
            //{
            //    var _tempUserList = organizationUserList.Where(x => x.UserID != userId).ToList();
            //    if (_tempUserList.Any(x => x.aspnet_Users.aspnet_Membership.Email.Trim().ToLower() == View.PrimaryEmail.ToLower()))
            //        return true;
            //}
            //UserAuthRequest tempObject = SecurityManager.GetUserAuthRequestByEmail(View.PrimaryEmail);
            //if (tempObject.IsNotNull())
            //    return true;
            //return false;
            ////String userName = System.Web.Security.Membership.GetUserNameByEmail(View.PrimaryEmail);
            ////return !String.IsNullOrEmpty(userName);

            string userName = System.Web.Security.Membership.GetUserNameByEmail(View.PrimaryEmail);
            if (userName.IsNullOrEmpty())
            {
                UserAuthRequest tempObject = SecurityManager.GetUserAuthRequestByEmail(View.PrimaryEmail);
                if (tempObject.IsNotNull())
                    return true;
                return false;
            }
            return true;
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
                ResHistorySeqOrdID = x.RHI_SequenceOrder.IsNotNull() ? x.RHI_SequenceOrder.Value : AppConsts.NONE
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
                ResHistorySeqOrdID = x.RHI_SequenceOrder.IsNotNull() ? x.RHI_SequenceOrder.Value : AppConsts.NONE
            }).ToList());
            return tempList;
        }
        /// <summary>
        ///Get Applicant Institution Hierarchy Mapping  
        /// </summary>
        public void GetApplicantInstitutionHierarchyMapping()
        {
            View.lstApplicantInstitutionHierarchyMapping = StoredProcedureManagers.GetApplicantInstitutionHierarchyMapping(View.SelectedTenantId, View.loggedInUserId.ToString());
        }
        /// <summary>
        /// Save and update custom Attributes for an applicant. 
        /// </summary>
        /// <param name="applicantCustomAttributeContract"></param>
        /// <returns></returns>
        public Boolean SaveUpdateApplicantCustomAttribute(List<ApplicantCustomAttributeContract> applicantCustomAttributeContract)
        {
            return ComplianceDataManager.SaveUpdateApplicantCustomAttribute(View.SelectedTenantId, applicantCustomAttributeContract, View.loggedInUserId, View.OrgUsrID);
        }

        public void GetSSNSetting()
        {
            View.IsSSNDisabled = (View.SelectedTenantId > 0 && View.SelectedTenantId != SecurityManager.DefaultTenantID) ? ComplianceDataManager.GetSSNSetting(View.SelectedTenantId, Setting.DISABLE_SSN.GetStringValue()) : false;
        }

        #region UAT-806 Creation of granular permissions for Client Admin users

        public void GetGranularPermissionForDOBandSSN()
        {
            View.IsDOBDisable = false;
            View.SSNPermissionCode = EnumSystemPermissionCode.FULL_PERMISSION.GetStringValue();
            Dictionary<String, String> dicPermissions = new Dictionary<String, String>();
            if (SecurityManager.GetUserGranularPermission(View.CurrentUserId, out dicPermissions))
            {
                if (dicPermissions.ContainsKey(EnumSystemEntity.DOB.GetStringValue()) && dicPermissions[EnumSystemEntity.DOB.GetStringValue()].ToUpper() == EnumSystemPermissionCode.NO_ACCESS_PERMISSION.GetStringValue().ToUpper())
                {
                    View.IsDOBDisable = true;
                }
                if (dicPermissions.ContainsKey(EnumSystemEntity.SSN.GetStringValue()))
                {
                    View.SSNPermissionCode = dicPermissions[EnumSystemEntity.SSN.GetStringValue()];
                }
            }
        }

        /// <summary>
        /// Getting Masked SSN
        /// </summary>
        /// <param name="unformattedSSN"></param>
        /// <returns></returns>
        public String GetMaskedSSN(String unMaskedSSN)
        {
            return ApplicationDataManager.GetMaskedSSN(unMaskedSSN);
        }

        #endregion

        public String GetUserNodePermission()
        {
            View.lstUserNodePermissionsContract = ComplianceSetupManager.GetUserNodePermissionForVerificationAndProfile(View.SelectedTenantId, View.CurrentUserId);
            List<Int32> lstLoggedInUserNodes = View.lstUserNodePermissionsContract.Select(col => col.DPM_ID).ToList();
            List<Int32> lstApplicantUserNodes = View.lstApplicantInstitutionHierarchyMapping.Select(col => col.DPM_ID).ToList();
            List<Int32> lstMatchingUserNodes = lstLoggedInUserNodes.Intersect(lstApplicantUserNodes).ToList();
            if (!lstMatchingUserNodes.IsNullOrEmpty())
            {
                Int32 profilePermissionIdForMinimumPermission = Convert.ToInt32(View.lstUserNodePermissionsContract.Where(cond => lstMatchingUserNodes.Contains(cond.DPM_ID)).Max(col => col.ProfilePermissionID));
                return View.lstUserNodePermissionsContract.Where(cond => cond.ProfilePermissionID == profilePermissionIdForMinimumPermission).Select(col => col.ProfilePermissionCode).FirstOrDefault();
            }
            else if (View.lstUserNodePermissionsContract.Any(cond => cond.ParentNodeID == null))
            {
                return View.lstUserNodePermissionsContract.Where(cond => cond.ParentNodeID == null).Select(col => col.ProfilePermissionCode).FirstOrDefault();
            }
            return LkpPermission.FullAccess.GetStringValue();
        }

        #region UAT-968:-As an ADB admin, I should be able to create/view/edit "notes" in a student's profile search details.
        /// <summary>
        /// To check if Tenant is Admin/DefaultTenant
        /// </summary>
        public Boolean IsDefaultTenant(Int32 currentLoggedInUserId)
        {
            Int32 currentLoggedInTenantId = SecurityManager.GetOrganizationUser(currentLoggedInUserId).Organization.TenantID.Value;
            if (SecurityManager.DefaultTenantID == currentLoggedInTenantId)
                return true;
            return false;
        }
        #endregion

        #region UAT-781 ENCRYPTED SSN
        /// <summary>
        /// Method to Get Decrypted SSN
        /// </summary>
        /// <param name="encryptedSSN"></param>
        public void GetDecryptedSSN(Int32 orgUserID, Boolean isOrgUserProfile)
        {
            View.DecryptedSSN = ComplianceSetupManager.GetFormattedString(orgUserID, isOrgUserProfile, View.SelectedTenantId);
        }
        #endregion


        #region UAT 1438: Enhancement to allow students to select a User Group.
        public void SaveUpdateApplicantUserGroupCustomAttribute(List<Entity.ClientEntity.ApplicantUserGroupMapping> lstApplicantuserGroupMapping)
        {
            Boolean status = ComplianceDataManager.SaveUpdateApplicantUserGroupCustomAttribute(View.SelectedTenantId, lstApplicantuserGroupMapping, View.loggedInUserId, View.OrgUsrID);
        }

        public Boolean IsUserGroupCustomAttributeExist(List<Int32> lstHierarchyNodeIds)
        {
            return ComplianceDataManager.IsUserGroupCustomAttributeExist(View.SelectedTenantId, lstHierarchyNodeIds, View.loggedInUserId);
        }
        #endregion

        #region UAT-1578 : Addition of SMS notification
        /// <summary>
        /// get the details of applicant SMS notification on the basis of applicantID.
        /// </summary>
        public void GetUserSMSNotificationData()
        {
            View.OrganisationUserTextMessageSettingData = SMSNotificationManager.GetSMSDataByApplicantId(View.loggedInUserId);
        }
        /// <summary>
        /// this method is used to save update delete the data for Applicant SMSNotification
        /// </summary>
        /// <returns></returns>
        public Boolean SaveSMSNotificationData()
        {
            try
            {
                SMSNotificationManager.SaveUpdateSMSData(View.loggedInUserId, View.PhoneNumber, View.OrgUsrID, View.IsReceiveTextNotification);
                return true;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Invalid parameter"))
                {
                    View.SMSNotificationErrorMessage = "Provided Cellular Phone Number is Invalid";
                }
            }
            return false;
        }
        /// <summary>
        /// get the Updated status of subscription from amazon.
        /// </summary>
        /// <returns></returns>
        //public Boolean UpdateSubscriptionStatusFromAmazon()
        //{
        //    //return SMSNotificationManager.UpdateSubscriptionStatusFromAmazon(View.loggedInUserId, View.OrgUsrID);
        //}
        #endregion

        public void SaveUpdateProfileCustomAttributes()
        {
            ComplianceDataManager.AddUpdateProfileCustomAttributes(View.ProfileCustomAttributeList, View.OrgUsrID, View.loggedInUserId, View.SelectedTenantId);
        }

        //UAT-2930
        public Boolean ShowhideTwoFactorAuthentication(String userId)
        {
            return SecurityManager.ShowhideTwoFactorAuthentication(userId);
        }

        //UAT-3608
        public Boolean SaveAuthenticationData(String userID, String authenticationType)
        {
            //View.CurrentUserId
            return SecurityManager.SaveAuthenticationData(userID, authenticationType, View.CurrentUserId);
        }
        //UAT-3133
        public Boolean IsIntegrationClientOrganisationUser()
        {
            return SecurityManager.IsIntegrationClientOrganisationUser(View.OrgUsrID, AppConsts.MAPPING_GROUP_CODE_UCONN);
        }

        //CBI || CABS  || Check whether the tenant is location service tenant or not.
        public void IsLocationServiceTenant()
        {
            View.IsLocationServiceTenant = SecurityManager.IsLocationServiceTenant(View.SelectedTenantId);
        }

        public void GetSuffixes()
        {
            View.lstSuffixes = new List<lkpSuffix>();
            View.lstSuffixes = SecurityManager.GetSuffixes();
        }

        //UAT 3824
        public void GetCommLang(Guid UserID)
        {
            View.CommLanguage = SecurityManager.GetCommLang().ToList();
            View.SelectedCommLang = SecurityManager.GetSelectedlang(UserID);
        }

        public int GetSuffixIdBasedOnSuffixText(string suffix)
        {
            return SecurityManager.GetSuffixIdBasedOnSuffixText(suffix);
        }

        #region UAT 4280: Enhancement to allow students to select a User Group.
        public void GetAllUserGroup()
        {
            if (View.SelectedTenantId > 0)
            {
                View.lstUserGroups = ComplianceSetupManager.GetAllUserGroup(View.SelectedTenantId).OrderBy(ex => ex.UG_Name);
            }

        }

        public void GetUserGroupsForUser()
        {

            View.lstUserGroupsForUser = ComplianceDataManager.GetUserGroupsForUser(View.SelectedTenantId, View.loggedInUserId);

        }
        #endregion

        public void IsDropDownSuffixType()
        {
            AppConfiguration appConfig = SecurityManager.GetAppConfiguration(AppConsts.SUFFIX_TYPE.ToString());
            View.IsSuffixDropDownType = !appConfig.IsNullOrEmpty() && appConfig.AC_Value == "1" ? true : false;
        }

        #region UAT-4731 : Restrict Applicant To One User Group In Order Process
        public Boolean IsMultiUserGroupSelectionAllowed()
        {
            var restrictAppToOneUGInOrderProcessSetting =  ComplianceDataManager.GetClientSetting(View.SelectedTenantId, Setting.RESTRICT_APPLICANT_TO_ONE_USER_GROUP_IN_ORDER_PROCESS.GetStringValue());
            if(restrictAppToOneUGInOrderProcessSetting != null)
            {
                return restrictAppToOneUGInOrderProcessSetting.CS_SettingValue == "1" ? false : true;
            }
            return true;
        }
        #endregion
    }
}




