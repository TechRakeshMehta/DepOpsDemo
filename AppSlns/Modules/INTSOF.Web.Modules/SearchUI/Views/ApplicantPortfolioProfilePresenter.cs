#region Namespaces

#region SystemDefined

using System;
using System.Linq;
using System.Collections.Generic;

#endregion

#region UserDefined

using Business.RepoManagers;
using INTSOF.Utils;
using INTSOF.UI.Contract.ComplianceOperation;
using Entity;
using INTSOF.SharedObjects;
using INTSOF.Utils.Consts;

#endregion

#endregion

namespace CoreWeb.Search.Views
{
    public class ApplicantPortfolioProfilePresenter : Presenter<IApplicantPortfolioProfileView>
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
            GetGranularPermissionForDOBandSSN();
        }

        public void GetOrganizationUser()
        {
            View.OrganizationUser = SecurityManager.GetOrganizationUser(View.OrganizationUserId);
        }

        public Int32 GetTenantID()
        {
            // UAT-2276: Regression testing and performance optimization
            //if (View.OrganizationUser != null)
            //    return View.OrganizationUser.Organization.TenantID.Value;
            //else return 0;
            return SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;

        }

        public void GetGender()
        {
            View.Gender = ComplianceDataManager.GetGenderById(View.GenderId, View.TenantId);
        }

        public void GetApplicantAddressData()
        {
            View.ApplicantZipCodeDetails = SecurityManager.GetApplicantZipCodeDetails(View.ZipCodeId);
        }

        #region UAT-806 Creation of granular permissions for Client Admin users

        public void GetGranularPermissionForDOBandSSN()
        {
            View.IsDOBDisable = false;
            View.SSNPermissionCode = EnumSystemPermissionCode.FULL_PERMISSION.GetStringValue();
            Dictionary<String, String> dicPermissions = new Dictionary<String, String>();
            if (SecurityManager.GetUserGranularPermission(View.CurrentLoggedInUserId, out dicPermissions))
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

        #region UAT-968:As an ADB admin, I should be able to create/view/edit "notes" in a student's profile search details.
        /// <summary>
        /// To check if Tenant is Admin/DefaultTenant
        /// </summary>
        public Boolean IsDefaultTenant
        {
            get
            {
                if (SecurityManager.DefaultTenantID == GetTenantID())
                    return true;
                else
                    return false;
            }
        }
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

        #region  UAT-1180: WB: Combine applicant and portfolio search.

        public List<lkpGender> GetGenderList()
        {
            return SecurityManager.GetGender().ToList();
        }

        public String GetUserNodePermission()
        {
            View.lstUserNodePermissionsContract = ComplianceSetupManager.GetUserNodePermissionForVerificationAndProfile(View.TenantId, View.CurrentLoggedInUserId);
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

        /// <summary>
        ///Get Applicant Institution Hierarchy Mapping  
        /// </summary>
        public void GetApplicantInstitutionHierarchyMapping()
        {
            View.lstApplicantInstitutionHierarchyMapping = StoredProcedureManagers.GetApplicantInstitutionHierarchyMapping(View.TenantId, View.OrganizationUserId.ToString());
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
        /// To check if Primary email exists
        /// </summary>
        /// <returns></returns>
        public Boolean IsExistsPrimaryEmail(Guid userId)
        {
            String userName = System.Web.Security.Membership.GetUserNameByEmail(View.PrimaryEmail);
            if (userName.IsNullOrEmpty())
            {
                UserAuthRequest tempObject = SecurityManager.GetUserAuthRequestByEmail(View.PrimaryEmail);
                if (tempObject.IsNotNull())
                    return true;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Method to update the Profile Photo.
        /// </summary>
        /// <returns>Boolean</returns>
        public Boolean SaveProfilePhoto()
        {
            OrganizationUser organizationUser = SecurityManager.GetOrganizationUser(View.OrganizationUserId);
            organizationUser.ModifiedByID = View.OrganizationUserId;
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
        /// To save user profile data
        /// </summary>
        /// <returns></returns>
        public Boolean SaveUserData()
        {
            OrganizationUser organizationUser = SecurityManager.GetOrganizationUser(View.OrganizationUserId);

            //organizationUser.aspnet_Users.UserName = View.UserName;
            //organizationUser.aspnet_Users.LoweredUserName = View.UserName.ToLower();
            organizationUser.aspnet_Users.MobileAlias = View.PrimaryPhone;
            organizationUser.Organization = SecurityManager.GetOrganizationForTenantID(View.TenantId);
            organizationUser.FirstName = View.FirstName;
            organizationUser.LastName = View.LastName;
            organizationUser.MiddleName = View.MiddleName;
            organizationUser.IsApplicant = true;
            organizationUser.ModifiedByID = View.CurrentLoggedInUserId;//UAT-2823
            organizationUser.ModifiedOn = DateTime.Now;
            organizationUser.CreatedByID = organizationUser.CreatedByID;//UAT-2823
            organizationUser.DOB = View.DOB;
            organizationUser.Gender = View.SelectedGenderId;
            organizationUser.PhoneNumber = View.PrimaryPhone;
            organizationUser.SecondaryPhone = View.SecondaryPhone;
            //UAT-2447
            organizationUser.IsInternationalPhoneNumber = View.IsInternationalPhoneNumber;
            organizationUser.IsInternationalSecondaryPhone = View.IsInternationalSecondaryPhone;
            if (View.UpdateAspnetEmail)
            {
                organizationUser.aspnet_Users.aspnet_Membership.Email = View.PrimaryEmail;
                organizationUser.aspnet_Users.aspnet_Membership.LoweredEmail = View.PrimaryEmail.ToLower();
                View.PswdRecoveryEmail = View.PrimaryEmail.ToLower();
            }
            organizationUser.PrimaryEmailAddress = View.PrimaryEmail;
            organizationUser.SecondaryEmailAddress = View.SecondaryEmail;
            organizationUser.SSN = View.SSN;
            organizationUser.Suffix = View.Suffix;
            if (View.Suffix.IsNullOrEmpty() &&( View.SelectedSuffixID == 0 || View.SelectedSuffixID==null))
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
                        if (View.ZipId == 0)
                        {
                            addressExtNew = new AddressExt();
                            addressExtNew.AE_CountryID = View.CountryId;
                            addressExtNew.AE_StateName = View.StateName;
                            addressExtNew.AE_CityName = View.CityName;
                            addressExtNew.AE_ZipCode = View.PostalCode;
                        }
                        Guid addressHandleId = Guid.NewGuid();
                        SecurityManager.AddAddressHandle(addressHandleId);
                        SecurityManager.AddAddress(dicAddressData, addressHandleId, View.OrganizationUserId, addressNew, addressExtNew);

                        SecurityManager.UpdateChanges();
                        ClientSecurityManager.AddAddressHandle(View.TenantId, addressHandleId);
                        ClientSecurityManager.AddAddress(View.TenantId, dicAddressData, addressHandleId, View.OrganizationUserId, addressNew, addressExtNew);
                        organizationUser.AddressHandleID = addressHandleId;
                        organizationUser.AddressHandle.Addresses.Add(addressNew);
                    }
                }
            }
            //Adds and updates the residential histories.
            AddUpdateResidentialHistory(organizationUser, addressNew);

            if (View.FilePath != null)
            {
                organizationUser.PhotoName = View.FilePath;
            }
            if (View.OriginalFileName != null)
            {
                organizationUser.OriginalPhotoName = View.OriginalFileName;
            }


            if (SecurityManager.UpdateOrganizationUser(organizationUser))
            {
                SecurityManager.AddOrganizationUserProfile(organizationUser);//UAT-2823
                //UAT-1579:WB: Add SMS opt in status to portfolio details screen for ADB and Client Admins
                //SaveUpdateSMSData();
                SecurityManager.SynchoniseUserProfile(organizationUser.OrganizationUserID, View.TenantId, organizationUser.OrganizationUserID);
                CommunicationManager.SendMailOnProfileChange(organizationUser, View.TenantId);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Save and update custom Attributes for an applicant. 
        /// </summary>
        /// <param name="applicantCustomAttributeContract"></param>
        /// <returns></returns>
        public Boolean SaveUpdateApplicantCustomAttribute(List<ApplicantCustomAttributeContract> applicantCustomAttributeContract)
        {
            return ComplianceDataManager.SaveUpdateApplicantCustomAttribute(View.TenantId, applicantCustomAttributeContract, View.OrganizationUserId, View.OrganizationUserId);
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
                            personAlias.PA_ModifiedBy = View.OrganizationUserId;
                            personAlias.PA_ModifiedOn = DateTime.Now;
                            if (View.IsLocationServiceTenant && !personAlias.PersonAliasExtensions.IsNullOrEmpty())
                            {
                                PersonAliasExtension personAliasExtension = personAlias.PersonAliasExtensions.Where(cond => !cond.PAE_IsDeleted).FirstOrDefault();
                                if (!personAliasExtension.IsNullOrEmpty())
                                {
                                    
                                    //personAliasExtension.PAE_Suffix = !tempPersonAlias.SuffixID.IsNullOrEmpty() && tempPersonAlias.SuffixID > AppConsts.NONE ? View.lstSuffixes.Where(cond => cond.SuffixID == tempPersonAlias.SuffixID).FirstOrDefault().Suffix : null;
                                    personAliasExtension.PAE_Suffix = tempPersonAlias.Suffix;
                                    personAliasExtension.PAE_ModifiedBy = AppConsts.NONE;
                                    personAliasExtension.PAE_ModifiedOn = DateTime.Now;
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
                        personAlias.PA_CreatedBy = View.OrganizationUserId;

                        personAlias.PA_CreatedOn = DateTime.Now;
                        personAlias.PA_AliasIdentifier = Guid.NewGuid();
                        organizationUser.PersonAlias.Add(personAlias);
                        //Added 
                        if (View.IsLocationServiceTenant)
                        {
                            PersonAliasExtension personAliasExtension = new PersonAliasExtension();
                            //personAliasExtension.PAE_Suffix = !tempPersonAlias.SuffixID.IsNullOrEmpty() && tempPersonAlias.SuffixID > AppConsts.NONE ? View.lstSuffixes.Where(cond => cond.SuffixID == tempPersonAlias.SuffixID).FirstOrDefault().Suffix : null;
                            personAliasExtension.PAE_Suffix = tempPersonAlias.Suffix;
                            personAliasExtension.PAE_CreatedBy = AppConsts.ONE;
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
                    delAlias.PA_ModifiedBy = View.OrganizationUserId;
                    delAlias.PA_ModifiedOn = DateTime.Now;
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

        private void AddUpdateResidentialHistory(OrganizationUser organizationUser, Address addressNew)
        {
            //Current residential Address
            ResidentialHistory currentResedentialHistory = organizationUser.ResidentialHistories.FirstOrDefault(x => x.RHI_IsCurrentAddress == true && x.RHI_IsDeleted == false);
            if (currentResedentialHistory.IsNotNull())
            {
                currentResedentialHistory.Address = addressNew == null ? organizationUser.AddressHandle.Addresses.FirstOrDefault() : addressNew;
                currentResedentialHistory.RHI_ResidenceStartDate = View.DateResidentFrom;
                currentResedentialHistory.RHI_ModifiedByID = View.OrganizationUserId;
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
                currentResedentialHistory.RHI_CreatedByID = View.OrganizationUserId;
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
                        prevAddress.RHI_ModifiedByID = View.OrganizationUserId;
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
                        newResidentialHistory.RHI_CreatedByID = View.OrganizationUserId;
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
                            resHistory.Address.ModifiedByID = View.OrganizationUserId;
                            resHistory.RHI_ModifiedByID = View.OrganizationUserId;
                            resHistory.Address.ModifiedOn = DateTime.Now;
                            resHistory.RHI_ModifiedOn = DateTime.Now;
                            resHistory.RHI_SequenceOrder = prevAddress.ResHistorySeqOrdID;
                        }
                    }
                }
            }
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
            SecurityManager.AddAddress(dicAddressData, addressHandleId, View.OrganizationUserId, addressPervious, addressExtPervious);
            SecurityManager.UpdateChanges();
            ClientSecurityManager.AddAddressHandle(View.TenantId, addressHandleId);
            ClientSecurityManager.AddAddress(View.TenantId, dicAddressData, addressHandleId, View.OrganizationUserId, addressPervious, addressExtPervious);
            return addressPervious;
        }

        public Dictionary<String, Object> GetPrevAddressDataDictionary(PreviousAddressContract prevAddress)
        {
            Dictionary<String, Object> dicAddressData = new Dictionary<String, Object>();
            dicAddressData.Add("address1", prevAddress.Address1);
            dicAddressData.Add("address2", prevAddress.Address2);
            dicAddressData.Add("zipcodeid", prevAddress.ZipCodeID);
            return dicAddressData;
        }

        public Dictionary<String, Object> GetAddressDataDictionary()
        {
            Dictionary<String, Object> dicAddressData = new Dictionary<String, Object>();
            dicAddressData.Add("address1", View.Address1);
            dicAddressData.Add("address2", View.Address2);
            dicAddressData.Add("zipcodeid", View.ZipId);
            return dicAddressData;
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

        #endregion

        #region UAT 1438: Enhancement to allow students to select a User Group.
        public void SaveUpdateApplicantUserGroupCustomAttribute(List<Entity.ClientEntity.ApplicantUserGroupMapping> lstApplicantuserGroupMapping)
        {
            bool status = ComplianceDataManager.SaveUpdateApplicantUserGroupCustomAttribute(View.TenantId, lstApplicantuserGroupMapping, View.OrganizationUserId, View.OrganizationUserId);
        }

        public Boolean IsUserGroupCustomAttributeExist(List<int> lstHierarchyNodeIds)
        {
            return ComplianceDataManager.IsUserGroupCustomAttributeExist(View.TenantId, lstHierarchyNodeIds, View.OrganizationUserId);
        }

        #endregion


        #region UAT-1579:WB: Add SMS opt in status to portfolio details screen for ADB and Client Admins
        public Boolean SaveUpdateSMSData()
        {
            try
            {
                //UAT-2276:Regression testing and performance optimization
                if (View.IsSMSDataAvailableForSave)
                {
                    SMSNotificationManager.SaveUpdateSMSData(View.OrganizationUserId, View.PhoneNumber, View.CurrentLoggedInUserId, View.IsReceiveTextNotification);
                }
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
        public void GetUserSMSNotificationData()
        {
            View.OrganisationUserTextMessageSettingData = SMSNotificationManager.GetSMSDataByApplicantId(View.OrganizationUserId); ;
        }
        //public Boolean UpdateSubscriptionStatusFromAmazon()
        //{
        //    return SMSNotificationManager.UpdateSubscriptionStatusFromAmazon(View.OrganizationUserId, View.CurrentLoggedInUserId);
        //}
        #endregion

        public void SaveUpdateProfileCustomAttributes()
        {
            ComplianceDataManager.AddUpdateProfileCustomAttributes(View.ProfileCustomAttributeList, View.OrganizationUserId, View.CurrentLoggedInUserId, View.TenantId);
        }


        /// <summary>
        /// Checked if logged user is admin or not.
        /// </summary>
        /// <returns>True if logged in user is admin.</returns>
        public void IsAdminLoggedIn()
        {
            Int32 AdminTenantID = SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
            //Checked if logged user is admin or not.
            if (AdminTenantID == Business.RepoManagers.SecurityManager.DefaultTenantID)
            {
                View.IsAdminLoggedIn = true;
            }
            else
            {
                View.IsAdminLoggedIn = false;
            }
        }
        public Boolean IsApplicantGraduated()
        {
            return ComplianceDataManager.IsApplicantGraduated(View.TenantId, View.OrganizationUserId);
        }

        #region UAT-2930 (Changes Related to UAT-3068)
        public void IsUserTwoFactorAuthenticated()
        {
            String authenticationMode = SecurityManager.GetUserAuthenticationUseTypeForUserID(Convert.ToString(View.OrganizationUser.UserID));
            View.SelectedAuthenticationType = View.IsUserTwoFactorAuthenticatedPrevious = authenticationMode;
        }

        public Boolean ShowhideTwoFactorAuthentication()
        {
            Boolean IsClientSettingsEnabledForAnyTenant = SecurityManager.ShowhideTwoFactorAuthentication(Convert.ToString(View.OrganizationUser.UserID));
            return IsClientSettingsEnabledForAnyTenant;
        }
        #endregion

        #region UAT-3047
        /// <summary>
        /// Performs an update operation for User with it's details.
        /// </summary>
        public Boolean UpdateUser()
        {
            OrganizationUser organizationUser = GetOrganizationUserData();
            if (!organizationUser.IsNullOrEmpty())
            {
                organizationUser.IsActive = View.IsActive;
                if (View.IsActive && organizationUser.ActiveDate == null && organizationUser.IsApplicant == true)
                    organizationUser.ActiveDate = DateTime.Now;
                organizationUser.ModifiedByID = View.CurrentLoggedInUserId;
                organizationUser.ModifiedOn = DateTime.Now;
                if (organizationUser.aspnet_Users.aspnet_Membership.IsLockedOut != View.IsLocked)
                {
                    organizationUser.aspnet_Users.aspnet_Membership.IsLockedOut = View.IsLocked;
                }
                #region UAT-2930
                //if (View.IsUserTwoFactorAuthenticatedPrevious && !View.IsUserTwoFactorAuthenticated)
                //{
                //    if (SecurityManager.DeleteTwofactorAuthenticationForUserID(Convert.ToString(View.OrganizationUser.UserID), View.OrganizationUserId))
                //    {
                //        CommunicationManager.SendMailOnTwoFactorAuthentication(organizationUser, View.TenantId);
                //    }
                //}
                #endregion

                #region UAT-3068
                if (!View.IsUserTwoFactorAuthenticatedPrevious.Equals(View.SelectedAuthenticationType) && View.SelectedAuthenticationType == AuthenticationMode.None.GetStringValue())
                {
                    if (SecurityManager.SaveAuthenticationData(Convert.ToString(organizationUser.UserID), View.SelectedAuthenticationType, View.OrganizationUserId))
                    {
                        CommunicationManager.SendMailOnTwoFactorAuthentication(organizationUser, View.TenantId);
                    }
                    View.IsUserTwoFactorAuthenticatedPrevious = View.SelectedAuthenticationType;
                }
                #endregion

                return SecurityManager.UpdateOrganizationUser(organizationUser);
            }
            return false;
        }

        public OrganizationUser GetOrganizationUserData()
        {
            return SecurityManager.GetOrganizationUser(View.OrganizationUserId);
        }

        #endregion

        //UAT-3608
        public Boolean SaveAuthenticationData(String userID, String authenticationType)
        {
            return SecurityManager.SaveAuthenticationData(userID, authenticationType, View.CurrentLoggedInUserId);
        }

        //CBI || CABS  || Check whether the tenant is location service tenant or not.
        public void IsLocationServiceTenant()
        {
            View.IsLocationServiceTenant = SecurityManager.IsLocationServiceTenant(View.TenantId);
        }

        public void GetSuffixes()
        {
            View.lstSuffixes = new List<Entity.lkpSuffix>();
            View.lstSuffixes = SecurityManager.GetSuffixes();
        }
        public int GetSuffixIdBasedOnSuffixText(string suffix)
        {
            return SecurityManager.GetSuffixIdBasedOnSuffixText(suffix);
        }

        #region UAT 4280: Enhancement to allow students to select a User Group.
        public void GetAllUserGroup()
        {
            if (View.TenantId > 0)
            {
                View.lstUserGroups = ComplianceSetupManager.GetAllUserGroup(View.TenantId).OrderBy(ex => ex.UG_Name);
            }

        }
        public void GetUserGroupsForUser()
        {

            View.lstUserGroupsForUser = ComplianceDataManager.GetUserGroupsForUser(View.TenantId, View.OrganizationUserId);

        }
        #endregion

        public void IsDropDownSuffixType()
        {
            AppConfiguration appConfig = SecurityManager.GetAppConfiguration(AppConsts.SUFFIX_TYPE.ToString());
            View.IsSuffixDropDownType = !appConfig.IsNullOrEmpty() && appConfig.AC_Value == "1" ? true : false;
        }
    }
}




