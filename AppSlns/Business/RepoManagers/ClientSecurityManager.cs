using Entity.ClientEntity;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business.RepoManagers
{
    public static class ClientSecurityManager
    {
        #region Department
        /// <summary>
        /// Retrieve a list of institution Type tenant.
        /// </summary>
        /// <returns>list of institution Type tenant</returns>
        public static List<Tenant> getClientTenant()
        {
            try
            {
                Int32 defaultTenantId = SecurityManager.DefaultTenantID;
                return BALUtils.GetClientSecurityRepoInstance(defaultTenantId).getClientTenant(defaultTenantId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Save Client organization Data.
        /// </summary>
        /// <param name="departmentObject"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static Boolean SaveClientDepartment(Organization organization, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetClientSecurityRepoInstance(tenantId).SaveClientDepartment(organization);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Get Client Organization Data.
        /// </summary>
        /// <param name="departmentObject"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static Organization GetClientOrganizationById(Int32 tenantId, Int32 organizationId)
        {
            try
            {
                return BALUtils.GetClientSecurityRepoInstance(tenantId).GetClientOrganizationById(organizationId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Update Client Organization Object
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static Boolean UpdateOrganizationObject(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetClientSecurityRepoInstance(tenantId).UpdateOrganizationObject();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        #region Manage Program

        public static Boolean DeleteProgram(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetClientSecurityRepoInstance(tenantId).DeleteProgram();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static DeptProgramMapping GetDepProgramMapping(Int32 tenantId, Int32 depProgramMappingId)
        {
            try
            {
                return BALUtils.GetClientSecurityRepoInstance(tenantId).GetDepProgramMapping(depProgramMappingId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static IQueryable<lkpPaymentOption> GetAllPaymentOption(Int32 tenantId)
        {
            try
            {
                return LookupManager.GetLookUpData<Entity.ClientEntity.lkpPaymentOption>(tenantId).Where(cond => !cond.IsDeleted && !cond.Code.Equals(PaymentOptions.OfflineSettlement.GetStringValue())).AsQueryable();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static IQueryable<DeptProgramPaymentOption> GetMappedDepProgramPaymentOption(Int32 tenantId, Int32 depProgramMappingId)
        {
            try
            {
                return BALUtils.GetClientSecurityRepoInstance(tenantId).GetMappedDepProgramPaymentOption(depProgramMappingId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// To get Duration Types
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static IQueryable<lkpDurationType> GetDurationTypes(Int32 tenantId)
        {
            try
            {
                return LookupManager.GetLookUpData<Entity.ClientEntity.lkpDurationType>(tenantId).Where(cond => cond.DT_IsDeleted == false).AsQueryable();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        #region Manage Grade
        //public static Boolean AddGrade(Int32 tenantId, lkpGradeLevel gradeLevel)
        //{
        //    try
        //    {
        //        return BALUtils.GetClientSecurityRepoInstance(tenantId).AddGrade(gradeLevel);
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(ex);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //}

        //public static Boolean UpdateGradeLevel(Int32 tenantId)
        //{
        //    try
        //    {
        //        return BALUtils.GetClientSecurityRepoInstance(tenantId).UpdateGradeLevel();
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(ex);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //}

        //public static lkpGradeLevel GetGradeLevel(Int32 tenantId, Int32 gradeLavelId)
        //{
        //    try
        //    {
        //        return BALUtils.GetClientSecurityRepoInstance(tenantId).GetGradeLevel(gradeLavelId);
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(ex);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //}
        #endregion

        #region OrganizationUser Db Updates
        /// <summary>
        /// Method to copy user data to Client Db
        /// </summary>
        /// <param name="myOrgUser"></param>
        public static void CopyUserToClient(Entity.OrganizationUser myOrgUser)
        {
            #region copy all Organization User properties
            OrganizationUser orgUser = new OrganizationUser();
            orgUser.OrganizationUserID = myOrgUser.OrganizationUserID;
            orgUser.UserID = myOrgUser.UserID;
            orgUser.OrganizationID = myOrgUser.OrganizationID;
            orgUser.BillingAddressID = myOrgUser.BillingAddressID;
            orgUser.ContactID = myOrgUser.ContactID;
            orgUser.UserTypeID = myOrgUser.UserTypeID;
            orgUser.DepartmentID = myOrgUser.DepartmentID;
            orgUser.SysXBlockID = myOrgUser.SysXBlockID;
            orgUser.AddressHandleID = myOrgUser.AddressHandleID;
            //orgUser.AddressHandle = myOrgUser.AddressHandle;
            orgUser.FirstName = myOrgUser.FirstName;
            orgUser.LastName = myOrgUser.LastName;
            orgUser.VerificationCode = myOrgUser.VerificationCode;
            orgUser.OfficeReturnDateTime = myOrgUser.OfficeReturnDateTime;
            orgUser.IsOutOfOffice = myOrgUser.IsOutOfOffice;
            orgUser.IsNewPassword = myOrgUser.IsNewPassword;
            orgUser.IgnoreIPRestriction = myOrgUser.IgnoreIPRestriction;
            orgUser.IsMessagingUser = myOrgUser.IsMessagingUser;
            orgUser.IsSystem = myOrgUser.IsSystem;
            orgUser.IsDeleted = myOrgUser.IsDeleted;
            orgUser.IsActive = myOrgUser.IsActive;
            orgUser.ExpireDate = myOrgUser.ExpireDate;
            orgUser.CreatedByID = myOrgUser.CreatedByID;
            orgUser.CreatedOn = myOrgUser.CreatedOn;
            orgUser.ModifiedByID = myOrgUser.ModifiedByID;
            orgUser.ModifiedOn = myOrgUser.ModifiedOn;
            orgUser.IsSubscribeToEmail = myOrgUser.IsSubscribeToEmail;
            orgUser.IsApplicant = myOrgUser.IsApplicant;
            orgUser.PhotoName = myOrgUser.PhotoName;
            orgUser.OriginalPhotoName = myOrgUser.OriginalPhotoName;
            orgUser.DOB = myOrgUser.DOB;
            orgUser.SSN = myOrgUser.SSN;
            orgUser.Gender = myOrgUser.Gender;
            orgUser.PhoneNumber = myOrgUser.PhoneNumber;
            orgUser.MiddleName = myOrgUser.MiddleName;
            orgUser.Alias1 = myOrgUser.Alias1;
            orgUser.Alias2 = myOrgUser.Alias2;
            orgUser.Alias3 = myOrgUser.Alias3;
            orgUser.PrimaryEmailAddress = myOrgUser.PrimaryEmailAddress;
            orgUser.SecondaryEmailAddress = myOrgUser.SecondaryEmailAddress;
            orgUser.SecondaryPhone = myOrgUser.SecondaryPhone;
            orgUser.UserVerificationCode = myOrgUser.UserVerificationCode;

            //UAT-2447
            orgUser.IsInternationalPhoneNumber = myOrgUser.IsInternationalPhoneNumber;
            orgUser.IsInternationalSecondaryPhone = myOrgUser.IsInternationalSecondaryPhone;

            //CBI|| CABS // Add Suffix ID in UserTypeID
            orgUser.UserTypeID = !myOrgUser.UserTypeID.IsNullOrEmpty() ? myOrgUser.UserTypeID : (Int32?)null;
            //Personal alias 
            if (myOrgUser.PersonAlias.IsNotNull())
            {
                List<PersonAlia> currentAliasList = orgUser.PersonAlias.Where(x => x.PA_IsDeleted == false).ToList();

                List<Entity.lkpSuffix> lstSuffix = SecurityManager.GetSuffixes();
                Boolean isLocationServiceTenant = SecurityManager.IsLocationServiceTenant(myOrgUser.Organization.TenantID.Value);
                foreach (Entity.PersonAlia tempPersonAlias in myOrgUser.PersonAlias)
                {
                    PersonAlia personAlias = currentAliasList.FirstOrDefault(x => x.PA_ID == tempPersonAlias.PA_ID);

                    if (personAlias.IsNotNull())
                    {
                        personAlias.PA_FirstName = tempPersonAlias.PA_FirstName;
                        personAlias.PA_LastName = tempPersonAlias.PA_LastName;
                        personAlias.PA_IsDeleted = tempPersonAlias.PA_IsDeleted;
                        personAlias.PA_MiddleName = tempPersonAlias.PA_MiddleName;
                        //personAlias.PA_ModifiedBy = tempPersonAlias.PA_OrganizationUserID;
                        personAlias.PA_ModifiedBy = tempPersonAlias.PA_ModifiedBy;
                        personAlias.PA_ModifiedOn = DateTime.Now;

                        if (!isLocationServiceTenant.IsNullOrEmpty() && isLocationServiceTenant && !personAlias.PersonAliasExtensions.IsNullOrEmpty())
                        {
                            var data = tempPersonAlias.PersonAliasExtensions.Where(cond => !cond.PAE_IsDeleted).FirstOrDefault();
                            PersonAliasExtension personAliasExtension = personAlias.PersonAliasExtensions.Where(cond => !cond.PAE_IsDeleted).FirstOrDefault();
                            if (!personAliasExtension.IsNullOrEmpty())
                            {
                                personAliasExtension.PAE_PersonAliasID = personAlias.PA_ID;
                                personAliasExtension.PAE_Suffix = !data.IsNullOrEmpty() && !data.PAE_Suffix.IsNullOrEmpty() ? data.PAE_Suffix : null;
                                personAliasExtension.PAE_ModifiedBy = tempPersonAlias.PA_ModifiedBy;
                                personAliasExtension.PAE_ModifiedOn = DateTime.Now;
                                personAliasExtension.PAE_IsDeleted = tempPersonAlias.PA_IsDeleted;
                            }
                            else
                            {
                                if (!data.IsNullOrEmpty() && !data.PAE_Suffix.IsNullOrEmpty())
                                {
                                    personAliasExtension = new PersonAliasExtension();
                                    personAliasExtension.PAE_Suffix = !data.PAE_Suffix.IsNullOrEmpty() ? data.PAE_Suffix : null;
                                    personAliasExtension.PAE_CreatedBy = tempPersonAlias.PA_CreatedBy;
                                    personAliasExtension.PAE_CreatedOn = DateTime.Now;
                                    personAliasExtension.PAE_IsDeleted = tempPersonAlias.PA_IsDeleted;
                                    personAlias.PersonAliasExtensions.Add(personAliasExtension);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (!tempPersonAlias.PA_IsDeleted)
                        {
                            personAlias = new PersonAlia();
                            personAlias.PA_ID = tempPersonAlias.PA_ID;
                            personAlias.PA_FirstName = tempPersonAlias.PA_FirstName;
                            personAlias.PA_LastName = tempPersonAlias.PA_LastName;
                            personAlias.PA_IsDeleted = tempPersonAlias.PA_IsDeleted;
                            personAlias.PA_MiddleName = tempPersonAlias.PA_MiddleName;
                            personAlias.PA_CreatedBy = tempPersonAlias.PA_CreatedBy;
                            personAlias.PA_CreatedOn = tempPersonAlias.PA_CreatedOn;
                            personAlias.PA_AliasIdentifier = tempPersonAlias.PA_AliasIdentifier;
                            orgUser.PersonAlias.Add(personAlias);

                            //Added 
                            if (!isLocationServiceTenant.IsNullOrEmpty() && isLocationServiceTenant && !tempPersonAlias.PersonAliasExtensions.IsNullOrEmpty())
                            {
                                var data = tempPersonAlias.PersonAliasExtensions.FirstOrDefault(cond => !cond.PAE_IsDeleted);
                                if (!data.IsNullOrEmpty() && !data.PAE_Suffix.IsNullOrEmpty())
                                {
                                    PersonAliasExtension personAliasExtension = new PersonAliasExtension();
                                    personAliasExtension.PAE_Suffix = !data.PAE_Suffix.IsNullOrEmpty() ? data.PAE_Suffix : null;
                                    personAliasExtension.PAE_CreatedBy = tempPersonAlias.PA_CreatedBy;
                                    personAliasExtension.PAE_CreatedOn = DateTime.Now;
                                    personAliasExtension.PAE_IsDeleted = tempPersonAlias.PA_IsDeleted;
                                    personAlias.PersonAliasExtensions.Add(personAliasExtension);
                                }
                            }
                        }
                    }
                }
            }
            #endregion

            #region update Address
            Entity.ClientEntity.AddressHandle objAH = new Entity.ClientEntity.AddressHandle();
            Entity.ClientEntity.Address objAddress = new Entity.ClientEntity.Address();
            Entity.ClientEntity.AddressExt objAddressExtension = new Entity.ClientEntity.AddressExt();
            objAH.AddressHandleID = myOrgUser.AddressHandle.AddressHandleID;
            objAH.ContextID = myOrgUser.AddressHandle.ContextID;

            //orgUser.AddressHandleID = objAH.AddressHandleID;

            Entity.Address objAddrss;
            objAddrss = myOrgUser.AddressHandle.Addresses.FirstOrDefault(p => p.AddressHandleID.Equals(myOrgUser.AddressHandleID));
            objAddress.Address1 = objAddrss.Address1;
            objAddress.Address2 = objAddrss.Address2;
            objAddress.ZipCodeID = objAddrss.ZipCodeID;
            objAddress.CreatedOn = objAddrss.CreatedOn;
            objAddress.AddressTypeID = objAddrss.AddressTypeID;
            objAddress.CreatedByID = objAddrss.CreatedByID;
            objAddress.IsActive = objAddrss.IsActive;
            objAddress.AddressHandleID = objAH.AddressHandleID;
            objAddress.AddressID = objAddrss.AddressID;

            Entity.AddressExt objAddressExt;
            objAddressExt = objAddrss.AddressExts.FirstOrDefault(x => x.AE_AddressID == objAddrss.AddressID && x.Address.IsActive == true);
            if (objAddressExt.IsNotNull())
            {
                objAddressExtension.AE_AddressID = objAddressExt.AE_AddressID;
                objAddressExtension.AE_CountryID = objAddressExt.AE_CountryID;
                objAddressExtension.AE_CityName = objAddressExt.AE_CityName;
                objAddressExtension.AE_County = objAddressExt.AE_County;
                objAddressExtension.AE_StateName = objAddressExt.AE_StateName;
                objAddressExtension.AE_ZipCode = objAddressExt.AE_ZipCode;
                objAddressExtension.AE_ID = objAddressExt.AE_ID;
                objAddress.AddressExts.Add(objAddressExtension);
                //objAddressExtension;
            }
            orgUser.AddressHandle = objAH;
            orgUser.AddressHandle.AddressHandleID = objAH.AddressHandleID;
            orgUser.AddressHandle.ContextID = objAH.ContextID;
            #endregion

            BALUtils.GetClientSecurityRepoInstance(myOrgUser.Organization.TenantID.Value).AddOrganizationUser(orgUser);
            //BALUtils.GetClientSecurityRepoInstance(myOrgUser.Organization.TenantID.Value).AddOrganizationUserDept(orgUserDept);
            BALUtils.GetComplianceDataRepoInstance(myOrgUser.Organization.TenantID.Value).AddAddress(objAddress);
            // BALUtils.GetComplianceDataRepoInstance(myOrgUser.Organization.TenantID.Value).AddAddressHandle(objAH);
        }

        public static void CopyUserToClientOrgProfile(Entity.OrganizationUserProfile myOrgUser, Int32? tenantID)
        {
            Int32 tenantId = myOrgUser.OrganizationUser.Organization.TenantID.Value;
            if (tenantID.HasValue && tenantId == SecurityManager.DefaultTenantID)
                tenantId = tenantID.Value;

            OrganizationUserProfile orgUser = new OrganizationUserProfile();
            orgUser.OrganizationUserProfileID = myOrgUser.OrganizationUserProfileID;
            orgUser.OrganizationUserID = myOrgUser.OrganizationUserID;
            orgUser.AddressHandleID = myOrgUser.AddressHandleID;
            orgUser.FirstName = myOrgUser.FirstName;
            orgUser.LastName = myOrgUser.LastName;
            orgUser.VerificationCode = myOrgUser.VerificationCode;
            orgUser.OfficeReturnDateTime = myOrgUser.OfficeReturnDateTime;
            orgUser.IsDeleted = myOrgUser.IsDeleted;
            orgUser.IsActive = myOrgUser.IsActive;
            orgUser.ExpireDate = myOrgUser.ExpireDate;
            orgUser.CreatedByID = myOrgUser.CreatedByID;
            orgUser.CreatedOn = myOrgUser.CreatedOn;
            orgUser.ModifiedByID = myOrgUser.ModifiedByID;
            orgUser.ModifiedOn = myOrgUser.ModifiedOn;
            orgUser.PhotoName = myOrgUser.PhotoName;
            orgUser.OriginalPhotoName = myOrgUser.OriginalPhotoName;
            orgUser.DOB = myOrgUser.DOB;
            orgUser.SSN = myOrgUser.SSN;
            orgUser.Gender = myOrgUser.Gender;
            orgUser.PhoneNumber = myOrgUser.PhoneNumber;
            orgUser.MiddleName = myOrgUser.MiddleName;
            orgUser.Alias1 = myOrgUser.Alias1;
            orgUser.Alias2 = myOrgUser.Alias2;
            orgUser.Alias3 = myOrgUser.Alias3;
            orgUser.PrimaryEmailAddress = myOrgUser.PrimaryEmailAddress;
            orgUser.SecondaryEmailAddress = myOrgUser.SecondaryEmailAddress;
            orgUser.SecondaryPhone = myOrgUser.SecondaryPhone;

            //UAT-2447
            orgUser.IsInternationalPhoneNumber = myOrgUser.IsInternationalPhoneNumber;
            orgUser.IsInternationalSecondaryPhone = myOrgUser.IsInternationalSecondaryPhone;

            //CBI|| CABS || Add Suffix ID
            orgUser.UserTypeID = myOrgUser.UserTypeID.IsNullOrEmpty() ? (Int32?)null : myOrgUser.UserTypeID;

            if (myOrgUser.PersonAliasProfiles.IsNotNull())
            {
                List<Entity.PersonAliasProfile> currentAliasProfileList = myOrgUser.PersonAliasProfiles.Where(x => x.PAP_IsDeleted == false).ToList();

                //CBI||CABS 
                List<Entity.lkpSuffix> lstSuffix = SecurityManager.GetSuffixes();
                Boolean isLocationServiceTenant = SecurityManager.IsLocationServiceTenant(myOrgUser.OrganizationUser.Organization.TenantID.Value);
                //
                foreach (Entity.PersonAliasProfile tempPersonAlias in currentAliasProfileList)
                {
                    PersonAliasProfile personAliasProfile = new PersonAliasProfile();
                    personAliasProfile.PAP_ID = tempPersonAlias.PAP_ID;
                    personAliasProfile.PAP_FirstName = tempPersonAlias.PAP_FirstName;
                    personAliasProfile.PAP_MiddleName = tempPersonAlias.PAP_MiddleName;
                    personAliasProfile.PAP_LastName = tempPersonAlias.PAP_LastName;
                    personAliasProfile.PAP_IsDeleted = tempPersonAlias.PAP_IsDeleted;
                    personAliasProfile.PAP_CreatedBy = tempPersonAlias.PAP_CreatedBy;
                    personAliasProfile.PAP_CreatedOn = tempPersonAlias.PAP_CreatedOn;
                    orgUser.PersonAliasProfiles.Add(personAliasProfile);

                    //CBI || CABS
                    if (isLocationServiceTenant && !tempPersonAlias.PersonAliasProfileExtensions.IsNullOrEmpty())
                    {
                        var data = tempPersonAlias.PersonAliasProfileExtensions.FirstOrDefault(cond => !cond.PAPE_IsDeleted);
                        if (!data.IsNullOrEmpty() && !data.PAPE_Suffix.IsNullOrEmpty())
                        {
                            PersonAliasProfileExtension personAliasProfileExtension = new PersonAliasProfileExtension();
                            personAliasProfileExtension.PAPE_Suffix = data.PAPE_Suffix;
                            personAliasProfileExtension.PAPE_CreatedBy = tempPersonAlias.PAP_CreatedBy;
                            personAliasProfileExtension.PAPE_CreatedOn = tempPersonAlias.PAP_CreatedOn;
                            personAliasProfileExtension.PAPE_IsDeleted = false;
                            personAliasProfile.PersonAliasProfileExtensions.Add(personAliasProfileExtension);
                        }
                    }
                    //
                }
            }

            if (myOrgUser.ResidentialHistoryProfiles.IsNotNull())
            {
                List<Entity.ResidentialHistoryProfile> lstResidentialHistoryProfile = myOrgUser.ResidentialHistoryProfiles.Where(x => x.RHIP_IsDeleted == false).ToList();
                foreach (Entity.ResidentialHistoryProfile masterResHistory in lstResidentialHistoryProfile)
                {
                    ResidentialHistoryProfile residentialHistoryProfile = new ResidentialHistoryProfile();
                    residentialHistoryProfile.RHIP_ID = masterResHistory.RHIP_ID;
                    residentialHistoryProfile.RHIP_AddressId = masterResHistory.RHIP_AddressId;
                    residentialHistoryProfile.RHIP_IsCurrentAddress = masterResHistory.RHIP_IsCurrentAddress;
                    residentialHistoryProfile.RHIP_IsPrimaryResidence = masterResHistory.RHIP_IsPrimaryResidence;
                    residentialHistoryProfile.RHIP_ResidenceStartDate = masterResHistory.RHIP_ResidenceStartDate;
                    residentialHistoryProfile.RHIP_ResidenceEndDate = masterResHistory.RHIP_ResidenceEndDate;
                    residentialHistoryProfile.RHIP_IsDeleted = masterResHistory.RHIP_IsDeleted;
                    residentialHistoryProfile.RHIP_CreatedBy = masterResHistory.RHIP_CreatedBy;
                    residentialHistoryProfile.RHIP_CreatedOn = masterResHistory.RHIP_CreatedOn;
                    residentialHistoryProfile.RHIP_SequenceOrder = masterResHistory.RHIP_SequenceOrder;
                    orgUser.ResidentialHistoryProfiles.Add(residentialHistoryProfile);
                }
            }

            BALUtils.GetClientSecurityRepoInstance(tenantId).AddOrganizationUserProfile(orgUser);
        }

        public static void UpdateUserToClient(Entity.OrganizationUser myOrgUser)
        {
            var repoInstance = BALUtils.GetClientSecurityRepoInstance(myOrgUser.Organization.TenantID.Value);
            OrganizationUser orgUser = repoInstance.GetOrganisationUser(myOrgUser);

            #region update OrganizationUser
            if (orgUser != null && orgUser.IsApplicant == true)
            {
                orgUser.OrganizationUserID = myOrgUser.OrganizationUserID;
                orgUser.UserID = myOrgUser.UserID;
                orgUser.OrganizationID = myOrgUser.OrganizationID;
                orgUser.BillingAddressID = myOrgUser.BillingAddressID;
                orgUser.ContactID = myOrgUser.ContactID;
                orgUser.UserTypeID = myOrgUser.UserTypeID;
                orgUser.DepartmentID = myOrgUser.DepartmentID;
                orgUser.SysXBlockID = myOrgUser.SysXBlockID;
                orgUser.AddressHandleID = myOrgUser.AddressHandleID;
                orgUser.FirstName = myOrgUser.FirstName;
                orgUser.LastName = myOrgUser.LastName;
                orgUser.VerificationCode = myOrgUser.VerificationCode;
                orgUser.OfficeReturnDateTime = myOrgUser.OfficeReturnDateTime;
                orgUser.IsOutOfOffice = myOrgUser.IsOutOfOffice;
                orgUser.IsNewPassword = myOrgUser.IsNewPassword;
                orgUser.IgnoreIPRestriction = myOrgUser.IgnoreIPRestriction;
                orgUser.IsMessagingUser = myOrgUser.IsMessagingUser;
                orgUser.IsSystem = myOrgUser.IsSystem;
                orgUser.IsDeleted = myOrgUser.IsDeleted;
                orgUser.IsActive = myOrgUser.IsActive;
                orgUser.ExpireDate = myOrgUser.ExpireDate;
                orgUser.CreatedByID = myOrgUser.CreatedByID;
                orgUser.CreatedOn = myOrgUser.CreatedOn;
                orgUser.ModifiedByID = myOrgUser.ModifiedByID;
                orgUser.ModifiedOn = myOrgUser.ModifiedOn;
                orgUser.IsSubscribeToEmail = myOrgUser.IsSubscribeToEmail;
                orgUser.IsApplicant = myOrgUser.IsApplicant;
                orgUser.PhotoName = myOrgUser.PhotoName;
                orgUser.OriginalPhotoName = myOrgUser.OriginalPhotoName;
                orgUser.DOB = myOrgUser.DOB;
                //orgUser.SSN = myOrgUser.SSN;
                orgUser.SSN = (myOrgUser.SSN.IsNullOrEmpty() ? String.Empty
                                                            : (myOrgUser.SSN.All(Char.IsDigit)
                                                            ? myOrgUser.SSN
                                                            : ComplianceSetupManager.GetFormattedString(myOrgUser.OrganizationUserID, false
                                                                                                    , myOrgUser.Organization.TenantID.Value)));
                orgUser.Gender = myOrgUser.Gender;
                orgUser.PhoneNumber = myOrgUser.PhoneNumber;
                orgUser.MiddleName = myOrgUser.MiddleName;
                orgUser.Alias1 = myOrgUser.Alias1;
                orgUser.Alias2 = myOrgUser.Alias2;
                orgUser.Alias3 = myOrgUser.Alias3;
                orgUser.PrimaryEmailAddress = myOrgUser.PrimaryEmailAddress;
                orgUser.SecondaryEmailAddress = myOrgUser.SecondaryEmailAddress;
                orgUser.SecondaryPhone = myOrgUser.SecondaryPhone;
                //UAT-2447
                orgUser.IsInternationalPhoneNumber = myOrgUser.IsInternationalPhoneNumber;
                orgUser.IsInternationalSecondaryPhone = myOrgUser.IsInternationalSecondaryPhone;

                //UAT-887: WB: Delay Automatic emails going out after activation
                orgUser.ActiveDate = myOrgUser.ActiveDate;
                if (myOrgUser.PersonAlias.IsNotNull())
                {
                    List<PersonAlia> currentAliasList = orgUser.PersonAlias.Where(x => x.PA_IsDeleted == false).ToList();
                    Boolean isLocationServiceTenant = SecurityManager.IsLocationServiceTenant(myOrgUser.Organization.TenantID.Value);
                    foreach (Entity.PersonAlia tempPersonAlias in myOrgUser.PersonAlias)
                    {
                        PersonAlia personAlias = currentAliasList.FirstOrDefault(x => x.PA_ID == tempPersonAlias.PA_ID);
                        if (personAlias.IsNotNull())
                        {
                            personAlias.PA_FirstName = tempPersonAlias.PA_FirstName;
                            personAlias.PA_LastName = tempPersonAlias.PA_LastName;
                            personAlias.PA_IsDeleted = tempPersonAlias.PA_IsDeleted;
                            personAlias.PA_MiddleName = tempPersonAlias.PA_MiddleName;
                            //personAlias.PA_ModifiedBy = tempPersonAlias.PA_OrganizationUserID;
                            personAlias.PA_ModifiedBy = tempPersonAlias.PA_ModifiedBy;
                            personAlias.PA_ModifiedOn = DateTime.Now;
                            if (!isLocationServiceTenant.IsNullOrEmpty() && isLocationServiceTenant && !personAlias.PersonAliasExtensions.IsNullOrEmpty())
                            {
                                var data = tempPersonAlias.PersonAliasExtensions.Where(cond => !cond.PAE_IsDeleted).FirstOrDefault();
                                PersonAliasExtension personAliasExtension = personAlias.PersonAliasExtensions.Where(cond => !cond.PAE_IsDeleted).FirstOrDefault();

                                if (!data.IsNullOrEmpty())
                                {
                                    if (!personAliasExtension.IsNullOrEmpty())
                                    {
                                        personAliasExtension.PAE_PersonAliasID = personAlias.PA_ID;
                                        personAliasExtension.PAE_Suffix = !data.PAE_Suffix.IsNullOrEmpty() ? data.PAE_Suffix : null;
                                        personAliasExtension.PAE_ModifiedBy = tempPersonAlias.PA_ModifiedBy;
                                        personAliasExtension.PAE_ModifiedOn = DateTime.Now;
                                    }
                                    else
                                    {
                                        personAliasExtension = new PersonAliasExtension();
                                        personAliasExtension.PAE_PersonAliasID = personAlias.PA_ID;
                                        personAliasExtension.PAE_Suffix = !data.PAE_Suffix.IsNullOrEmpty() ? data.PAE_Suffix : null;
                                        personAliasExtension.PAE_ModifiedBy = tempPersonAlias.PA_ModifiedBy;
                                        personAliasExtension.PAE_ModifiedOn = DateTime.Now;
                                        personAliasExtension.PAE_IsDeleted = tempPersonAlias.PA_IsDeleted;
                                    }
                                }

                            }
                        }
                        else
                        {
                            if (!tempPersonAlias.PA_IsDeleted)
                            {
                                personAlias = new PersonAlia();
                                personAlias.PA_ID = tempPersonAlias.PA_ID;
                                personAlias.PA_FirstName = tempPersonAlias.PA_FirstName;
                                personAlias.PA_LastName = tempPersonAlias.PA_LastName;
                                personAlias.PA_IsDeleted = tempPersonAlias.PA_IsDeleted;
                                personAlias.PA_MiddleName = tempPersonAlias.PA_MiddleName;
                                personAlias.PA_CreatedBy = tempPersonAlias.PA_CreatedBy;
                                personAlias.PA_CreatedOn = tempPersonAlias.PA_CreatedOn;
                                personAlias.PA_AliasIdentifier = tempPersonAlias.PA_AliasIdentifier;
                                orgUser.PersonAlias.Add(personAlias);
                                if (!isLocationServiceTenant.IsNullOrEmpty() && isLocationServiceTenant)
                                {
                                    var data = tempPersonAlias.PersonAliasExtensions.Where(cond => !cond.PAE_IsDeleted).FirstOrDefault();
                                    if (!data.IsNullOrEmpty())
                                    {
                                        PersonAliasExtension personAliasExtension = new PersonAliasExtension();
                                        personAliasExtension.PAE_Suffix = !data.PAE_Suffix.IsNullOrEmpty() ? data.PAE_Suffix : null;
                                        personAliasExtension.PAE_CreatedBy = tempPersonAlias.PA_CreatedBy;
                                        personAliasExtension.PAE_CreatedOn = DateTime.Now;
                                        personAliasExtension.PAE_IsDeleted = false;
                                        personAlias.PersonAliasExtensions.Add(personAliasExtension);
                                    }
                                }
                            }
                        }
                    }
                }
                if (myOrgUser.ResidentialHistories.IsNotNull())
                {
                    // List of Resedential Histories associated with the organisaion User ID.
                    List<Entity.ResidentialHistory> lstMasterResidentialHistory = myOrgUser.ResidentialHistories.ToList();
                    foreach (var prevAddress in lstMasterResidentialHistory)
                    {
                        ResidentialHistory newResHisObj = orgUser.ResidentialHistories.FirstOrDefault(x => x.RHI_ID == prevAddress.RHI_ID);
                        if (newResHisObj.IsNotNull())
                        {
                            if (newResHisObj.RHI_IsCurrentAddress == true && prevAddress.RHI_IsCurrentAddress == true)
                            {
                                newResHisObj.RHI_AddressId = prevAddress.RHI_AddressId;
                                newResHisObj.RHI_ResidenceStartDate = prevAddress.RHI_ResidenceStartDate;
                                newResHisObj.RHI_ModifiedByID = prevAddress.RHI_ModifiedByID;
                                newResHisObj.RHI_ModifiedOn = prevAddress.RHI_ModifiedOn;
                                newResHisObj.RHI_SequenceOrder = prevAddress.RHI_SequenceOrder;
                            }
                            else
                            {
                                if (!(newResHisObj.RHI_IsDeleted == true && prevAddress.RHI_IsDeleted == true))
                                {
                                    if (prevAddress.RHI_IsDeleted == true)
                                    {
                                        newResHisObj.RHI_IsDeleted = true;
                                    }
                                    else
                                    {
                                        newResHisObj.RHI_AddressId = prevAddress.RHI_AddressId;
                                        newResHisObj.RHI_ResidenceStartDate = prevAddress.RHI_ResidenceStartDate;
                                        newResHisObj.RHI_ResidenceEndDate = prevAddress.RHI_ResidenceEndDate;
                                        newResHisObj.RHI_IsCurrentAddress = prevAddress.RHI_IsCurrentAddress;
                                    }
                                    newResHisObj.RHI_ModifiedByID = prevAddress.RHI_ModifiedByID;
                                    newResHisObj.RHI_ModifiedOn = prevAddress.RHI_ModifiedOn;
                                    newResHisObj.RHI_SequenceOrder = prevAddress.RHI_SequenceOrder;
                                }
                            }
                        }
                        else
                        {
                            if (prevAddress.RHI_IsDeleted == false)
                            {
                                newResHisObj = new ResidentialHistory();
                                newResHisObj.RHI_ID = prevAddress.RHI_ID;
                                newResHisObj.RHI_ResidenceStartDate = prevAddress.RHI_ResidenceStartDate;
                                newResHisObj.RHI_ResidenceEndDate = prevAddress.RHI_ResidenceEndDate;
                                newResHisObj.RHI_OrganizationUserID = prevAddress.RHI_OrganizationUserID;
                                newResHisObj.RHI_AddressId = prevAddress.RHI_AddressId;
                                newResHisObj.RHI_IsCurrentAddress = prevAddress.RHI_IsCurrentAddress;
                                newResHisObj.RHI_IsPrimaryResidence = prevAddress.RHI_IsPrimaryResidence;
                                newResHisObj.RHI_IsDeleted = prevAddress.RHI_IsDeleted;
                                newResHisObj.RHI_CreatedByID = prevAddress.RHI_CreatedByID;
                                newResHisObj.RHI_CreatedOn = prevAddress.RHI_CreatedOn;
                                newResHisObj.RHI_SequenceOrder = prevAddress.RHI_SequenceOrder;
                                orgUser.ResidentialHistories.Add(newResHisObj);
                            }
                        }
                    }
                }
            }
            #endregion
            //#region update OrganizationUserDept


            //#endregion
            //# region AddressData
            // Entity.ClientEntity.AddressHandle objAH = orgUser.AddressHandle;
            // Entity.ClientEntity.Address objAddress = orgUser.AddressHandle.Addresses.FirstOrDefault(p => p.AddressHandleID.Equals(myOrgUser.AddressHandleID));
            // objAH.AddressHandleID  = myOrgUser.AddressHandle.AddressHandleID;
            // objAH.ContextID = myOrgUser.AddressHandle.ContextID;
            // orgUser.AddressHandleID = myOrgUser.AddressHandleID;

            // Entity.Address objAddrss;
            // objAddrss = myOrgUser.AddressHandle.Addresses.FirstOrDefault(p => p.AddressHandleID.Equals(myOrgUser.AddressHandleID));
            // objAddress.Address1 = objAddrss.Address1;
            // objAddress.Address2 = objAddrss.Address2;
            // objAddress.ZipCodeID = objAddrss.ZipCodeID;
            // objAddress.CreatedOn = objAddrss.CreatedOn;

            ////objAddress.AddressHandle = objAH;
            // orgUser.AddressHandle = objAH;
            // orgUser.AddressHandle.AddressHandleID = objAH.AddressHandleID;
            // orgUser.AddressHandle.ContextID = objAH.ContextID;
            //#endregion

            repoInstance.UpdateOrganizationData(orgUser);
            // return (BALUtils.GetComplianceDataRepoInstance(organizationUser) as IBaseRepository).UpdateObjectEntity(organizationUser);
        }

        public static void UpdateUsersProfilePicture(Entity.OrganizationUser organizationUser)
        {
            try
            {
                var repoInstance = BALUtils.GetClientSecurityRepoInstance(organizationUser.Organization.TenantID.Value);
                OrganizationUser orgUser = repoInstance.GetOrganisationUser(organizationUser);
                orgUser.PhotoName = organizationUser.PhotoName;
                orgUser.OriginalPhotoName = organizationUser.OriginalPhotoName;
                repoInstance.UpdateOrganizationData(orgUser);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region Edit Profile
        public static void AddAddressHandle(Int32 tenantId, Guid addressHandleID)
        {
            BALUtils.GetClientSecurityRepoInstance(tenantId).AddAddressHandle(addressHandleID);
        }
        public static void AddAddress(Int32 tenantId, Dictionary<String, Object> dicAddressData, Guid addressHandleId, Int32 currentUserId, Entity.Address addressNew, Entity.AddressExt addressExtNew)
        {
            BALUtils.GetClientSecurityRepoInstance(tenantId).AddAddress(dicAddressData, addressHandleId, currentUserId, addressNew, addressExtNew);
        }
        #endregion

        #region Manage Tenant DB Default Entry

        public static Boolean UpdateDefaultEntryForNewClient(Int32 tenantId, String tenantConnectinString = "")
        {
            String parametersPassed = String.Empty;
            try
            {
                parametersPassed = "Parameters Passed - Tenant Id: " + tenantId.ToString() + "";
                if (BALUtils.LoggerService.IsNotNull())
                    BALUtils.LoggerService.GetLogger().Info(BALUtils.ClassModule + "Method Started: UpdateDefaultEntryForNewClient with " + parametersPassed + "");
                return BALUtils.GetClientSecurityRepoInstance(tenantId, tenantConnectinString).UpdateDefaultEntryForNewClient(tenantId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + parametersPassed + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                //BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + parametersPassed + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        /// <summary>
        /// Retrieve tenant name.
        /// </summary>
        /// <param name="tenantId">InstitutionID</param>
        /// <returns>TenantName</returns>
        public static String GetTenantName(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetClientSecurityRepoInstance(tenantId).GetTenantName(tenantId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// UAT 1834: NYU Migration 2 of 3: Applicant Complete Order Process.
        /// </summary>
        /// <param name="nullable"></param>
        /// <param name="p"></param>
        public static void UpdateBulkOrderUploadForOrgUser(Int32 tenantId, Int32 applicantID, String emailAddress)
        {
            try
            {
                BALUtils.GetClientSecurityRepoInstance(tenantId).UpdateBulkOrderUploadForOrgUser(applicantID, emailAddress);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #region UAT-2257
        public static void CopyClientAdminPermissions(Int32 tenantID, Guid CopyFromUserId, Guid CopyToUserId, Int32 CurrentLoggedInUserId, Int32 CopyToOrgUserId)
        {
            BALUtils.GetClientSecurityRepoInstance(tenantID).CopyClientAdminPermissions(tenantID, CopyFromUserId, CopyToUserId, CurrentLoggedInUserId, CopyToOrgUserId);
        }
        #endregion

        #region UAT-2438

        public static IQueryable<lkpPDFInclusionOption> GetPDFInclusionOptions(Int32 tenantId)
        {
            try
            {
                return LookupManager.GetLookUpData<Entity.ClientEntity.lkpPDFInclusionOption>(tenantId).Where(cond => !cond.IsDeleted).AsQueryable();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        #region UAT-2438

        public static IQueryable<lkpResultSentToApplicant> GetResultSentToApplicantOptions(Int32 tenantId)
        {
            try
            {
                return LookupManager.GetLookUpData<Entity.ClientEntity.lkpResultSentToApplicant>(tenantId).Where(cond => !cond.RSTA_IsDeleted).AsQueryable();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        #region 3268
        public static IQueryable<lkpBkgHierarchyNodeExemptedType> GetBkgHierarchyNodeExemptedType(Int32 tenantId)
        {
            try
            {
                return LookupManager.GetLookUpData<Entity.ClientEntity.lkpBkgHierarchyNodeExemptedType>(tenantId).Where(cond => !cond.HNET_IsDeleted).AsQueryable();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion
    }
}
