using INTSOF.SharedObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using Entity;
using System.Security.Cryptography;
using INTSOF.Utils;
using Entity.SharedDataEntity;
using INTSOF.UI.Contract.ComplianceOperation;
using CoreWeb.IntsofSecurityModel;
using System.Text.RegularExpressions;
using System.Configuration;
using INTSOF.Utils.Consts;
using INTSOF.UI.Contract.SysXSecurityModel;
using INTSOF.ServiceUtil;
using System.Web;
using INTSOF.Contracts;

namespace CoreWeb.ProfileSharing.Views
{
    public class SharedUserCreateAccountPresenter : Presenter<ISharedUserCreateAccount>
    {
        #region EVENTS
        public override void OnViewLoaded()
        {
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }
        #endregion

        #region METHODS

        /// <summary>
        /// Method to get Shared User Data via Invite Token 
        /// </summary>
        public String GetSharedUserDataFromInvitation()
        {
            if (View.ClientContactToken != new Guid()) // UAT-1218 If Client Contact Invitation recieved
            {
                return ProfileSharingManager.GetSharedUserDataFromInvitation(View.ClientContactToken, false, null);
            }
            else if (View.InviteToken != new Guid()) // If Profile Sharing Invitation recieved
            {
                return ProfileSharingManager.GetSharedUserDataFromInvitation(View.InviteToken, true, null);
            }
            else if (View.AgencyUserID.IsNotNull() && View.AgencyUserID != AppConsts.NONE) // If Agency User ID received
            {
                return ProfileSharingManager.GetSharedUserDataFromInvitation(Guid.Empty, false, View.AgencyUserID);
            }
            return String.Empty;
        }

        /// <summary>
        /// Method to check whether entered username already exists
        /// </summary>
        /// <returns></returns>
        public Boolean IsExistsUserName()
        {
            System.Web.Security.MembershipUser user = System.Web.Security.Membership.GetUser(View.UserName);
            return user != null;
        }

        /// <summary>
        /// Method to Add the shared user to the application
        /// </summary>
        /// <returns></returns>
        public Boolean AddSharedUser()
        {
            aspnet_Applications application = SecurityManager.GetApplication();
            aspnet_Users aspnetUsers = new aspnet_Users();
            aspnet_Membership memberShip = new aspnet_Membership();
            OrganizationUser organizationUser = new OrganizationUser();
            Organization org = new Organization();
            ProfileSharingInvitation invitation = new ProfileSharingInvitation();

            //Creating aspnet_users
            aspnetUsers.LastActivityDate = DateTime.MaxValue;
            aspnetUsers.UserName = View.UserName;
            aspnetUsers.LoweredUserName = aspnetUsers.UserName.ToLower();
            aspnetUsers.ApplicationId = application.ApplicationId;
            aspnetUsers.UserId = Guid.NewGuid();

            //Creating aspnet_membership
            memberShip.PasswordSalt = SysXMembershipUtil.GenerateSalt();
            memberShip.Password = SysXMembershipUtil.HashPasswordIWithSalt(View.Password, memberShip.PasswordSalt);
            memberShip.Email = View.EmailAddress;
            memberShip.LoweredEmail = memberShip.Email.ToLower();
            memberShip.aspnet_Applications = application;
            memberShip.IsApproved = true;
            memberShip.PasswordFormat = AppConsts.ONE;
            memberShip.CreateDate = DateTime.Now;
            memberShip.LastLockoutDate = DateTime.Now.AddDays(-1);
            memberShip.LastLoginDate = DateTime.Now.AddDays(-1);
            memberShip.LastPasswordChangedDate = DateTime.Now.AddDays(-1);
            memberShip.FailedPasswordAttemptWindowStart = DateTime.Now.AddDays(-1);
            memberShip.FailedPasswordAnswerAttemptWindowStart = DateTime.Now.AddDays(-1);
            memberShip.IsLockedOut = false;
            aspnetUsers.aspnet_Membership = memberShip;

            //Creating OrganizationUser
            organizationUser.Organization = SecurityManager.GetOrganizationForTenantID(AppConsts.SHARED_USER_TENANT_ID);
            organizationUser.aspnet_Users = aspnetUsers;
            organizationUser.FirstName = View.FirstName;
            organizationUser.MiddleName = View.MiddleName;
            organizationUser.LastName = View.LastName;
            organizationUser.PrimaryEmailAddress = View.EmailAddress;
            organizationUser.SecondaryEmailAddress = null;
            organizationUser.IsNewPassword = false;
            organizationUser.CreatedOn = DateTime.Now;
            organizationUser.ModifiedOn = null;
            organizationUser.IsDeleted = false;
            organizationUser.IsActive = true;
            organizationUser.IsApplicant = false;
            organizationUser.IsOutOfOffice = false;
            organizationUser.IgnoreIPRestriction = true;
            organizationUser.IsMessagingUser = true;
            organizationUser.IsSystem = false;
            organizationUser.PhotoName = null;
            organizationUser.OriginalPhotoName = null;
            organizationUser.DOB = null;
            organizationUser.SSN = null;
            organizationUser.Gender = null;
            organizationUser.PhoneNumber = null;
            organizationUser.SecondaryPhone = null;
            organizationUser.UserVerificationCode = null;
            organizationUser.IsSharedUser = true;

            if ((View.UserTypeCode == OrganizationUserType.Instructor.GetStringValue() 
                || View.UserTypeCode == OrganizationUserType.Preceptor.GetStringValue())
                && !View.Last4SSN.IsNullOrEmpty())
            {
                //UAT-4355
                 organizationUser.SSNL4 = View.Last4SSN;
               // organizationUser.SSN = View.Last4SSN;
                //  organizationUser.IsInstructorPreceptorAccount = true;
            }
            OrganizationUser orgUserObj = SecurityManager.AddOrganizationUser(organizationUser, aspnetUsers);
            if (orgUserObj.IsNotNull())
            {
                // Updating Invitee UserID based on userType recieved
                UpdateInviteeUserID(orgUserObj);

                // Created Mapping of shared user in OrganizationUserTypeMapping
                SecurityManager.AddOrganizationUserTypeMapping(orgUserObj.OrganizationUserID, View.UserTypeCode);

                //Synchronize Client COntact Profiles
                SynchronizeClientContactProfile(orgUserObj);

                //UAT 1346: As an Agency user, I should be able to create and maintain other agency users.
                if (View.UserTypeCode == OrganizationUserType.AgencyUser.GetStringValue())
                {
                    //Assign default roles to the agency shared user
                    ApplicationDataManager.AssignDefaultRolesToAgencyUser(orgUserObj);
                    View.SkipLoginScreen = true;
                    View.AgencyUserGUID = Convert.ToString(orgUserObj.UserID);
                    //End
                }
            }
            return true;
        }

        private void SynchronizeClientContactProfile(OrganizationUser orgUserObj)
        {
            //UAT-1361 - Client Contact Profile Synchcning
            if (View.UserTypeCode == OrganizationUserType.Instructor.GetStringValue() || View.UserTypeCode == OrganizationUserType.Preceptor.GetStringValue())
            {
                //Getting ClientContact TenantIDs for the clinical rot of those tenants in which he is added
                List<Int32> lstClientContactTenantIds = ClientContactManager.GetClientContactTenantIdList(View.ClientContactToken);
                foreach (Int32 tenantID in lstClientContactTenantIds)
                {
                    ClinicalRotationManager.SynchronizeClientContactProfiles(null, orgUserObj.OrganizationUserID, tenantID);
                }
            }
        }

        /// <summary>
        /// Method to update InviteeUserID based on UserType received from querystring
        /// </summary>
        /// <param name="orgUserObj"></param>
        private void UpdateInviteeUserID(OrganizationUser orgUserObj)
        {
            if (!View.UserTypeCode.IsNullOrEmpty())
            {
                if (View.UserTypeCode == OrganizationUserType.AgencyUser.GetStringValue() || View.UserTypeCode == OrganizationUserType.ApplicantsSharedUser.GetStringValue())
                {
                    String profileSharingInvitationIds;
                    ProfileSharingManager.UpdateInviteeOrganizationUserID(orgUserObj.OrganizationUserID, View.InviteToken, View.AgencyUserID, out profileSharingInvitationIds);
                    CallParallelTaskToUpdateSharedUserRotationReview(profileSharingInvitationIds, orgUserObj.OrganizationUserID);
                }
                else if (View.UserTypeCode == OrganizationUserType.Instructor.GetStringValue() || View.UserTypeCode == OrganizationUserType.Preceptor.GetStringValue())
                {
                    ProfileSharingManager.UpdateClientContactUserID(orgUserObj.UserID, View.EmailAddress, orgUserObj.OrganizationUserID);
                }
            }
        }

        #region UAT-3400
        public void CallParallelTaskToUpdateSharedUserRotationReview(String profileSharingInvitationIds, Int32 organizationUserId)
        {
            Dictionary<String, Object> Data = new Dictionary<String, Object>();
            Data.Add("strProfileSharingInvIds", profileSharingInvitationIds);
            Data.Add("organizationUserId", organizationUserId);

            var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
            var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;
            ParallelTaskContext.PerformParallelTask(InsertSharedUserRotReviewForNonRegUser, Data, LoggerService, ExceptiomService);
        }


        public void InsertSharedUserRotReviewForNonRegUser(Dictionary<String, Object> data)
        {
            String profileSharingids = Convert.ToString(data.GetValue("strProfileSharingInvIds"));
            Int32 organizationUserId = Convert.ToInt32(data.GetValue("organizationUserId"));
            Boolean IsSuccess = ProfileSharingManager.InsertSharedUserRotReviewForNonRegUser(profileSharingids, organizationUserId);

        }
        #endregion

        #region UAT-1218 Shared User Account Linking
        public Boolean IsExistingUser()
        {
            List<LookupContract> lstExistingUser = SecurityManager.GetExistingUserLists(View.UserName, DateTime.Now, String.Empty, View.FirstName, View.LastName, false, View.EmailAddress, true);
            if (lstExistingUser.Count > 0)
            {
                View.ExistingUsersList = lstExistingUser;
                return true;
            }
            return false;
        }

        /// <summary>
        /// It validates the user, and then redirect to login page.
        /// </summary>
        public void ValidateUserNameAndPassword()
        {
            SysXMembershipUser user = System.Web.Security.Membership.GetUser(Regex.Replace(View.LoginUserName, @"(?<=^\s*)\s|\s(?=\s*$)", SysXSecurityConst.ASCIISPACE)) as SysXMembershipUser;

            if (System.Web.Security.Membership.ValidateUser(Regex.Replace(View.LoginUserName, @"(?<=^\s*)\s|\s(?=\s*$)", SysXSecurityConst.ASCIISPACE), Regex.Replace(View.LoginPassword, @"(?<=^\s*)\s|\s(?=\s*$)", SysXSecurityConst.ASCIISPACE)))
            {
                // Checks if the user is locked.
                if (!user.IsNull() && user.IsLockedOut)
                {
                    View.LoginErrorMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_ACCOUNT_LOCKED);
                    View.ExistingOrganisationUser = null;
                }
                SecurityManager.ResetPasswordAttempCount(View.LoginUserName);
                View.ExistingOrganisationUser = SecurityManager.GetOrganizationUser(user.OrganizationUserId);
            }
            else
            {
                View.LoginErrorMessage = String.Empty;
                SecurityManager.FailedPasswordAttemptCount(View.LoginUserName, Convert.ToInt32(ConfigurationManager.AppSettings["MaxPasswordAttempt"]));
                View.setSubmitbuttonText = "Try Again";
                if (!user.IsNull())
                {
                    OrganizationUser orgUser = SecurityManager.GetOrganizationUser(user.OrganizationUserId);

                    if (user.IsNewPassword)
                    {
                        View.LoginErrorMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_ENTER_PWD_RECEIVED_IN_EMAIL);
                    }
                    else if (!orgUser.IsActive)
                    {
                        View.LoginErrorMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_ERROR_INACTIVE_ACCOUNT);
                    }
                    else
                    {
                        View.LoginErrorMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_INVALID_USRNAME_PWD);
                    }
                }
                else
                {
                    View.LoginErrorMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_INVALID_USRNAME_PWD);
                }
                View.ExistingOrganisationUser = null;
            }
        }

        public Boolean IsUsernameExistInSecurityDB()
        {
            return SecurityManager.IsUsernameExistInSecurityDB(View.LoginUserName);
        }

        public Boolean AddLinkedUserProfile()
        {
            OrganizationUser organizationUser = new OrganizationUser();
            organizationUser.Organization = SecurityManager.GetOrganizationForTenantID(AppConsts.SHARED_USER_TENANT_ID);
            organizationUser.UserID = View.ExistingOrganisationUser.UserID;
            organizationUser.FirstName = View.FirstName;
            organizationUser.MiddleName = View.MiddleName;
            organizationUser.LastName = View.LastName;
            organizationUser.PrimaryEmailAddress = View.EmailAddress;
            organizationUser.IsNewPassword = false;
            organizationUser.CreatedOn = DateTime.Now;
            organizationUser.ModifiedOn = null;
            organizationUser.IsDeleted = false;
            organizationUser.IsActive = true;
            organizationUser.IsApplicant = false;
            organizationUser.IsOutOfOffice = false;
            organizationUser.IgnoreIPRestriction = true;
            organizationUser.IsMessagingUser = true;
            organizationUser.IsSystem = false;
            organizationUser.IsSharedUser = true;

            if ((View.UserTypeCode == OrganizationUserType.Instructor.GetStringValue() 
                || View.UserTypeCode == OrganizationUserType.Preceptor.GetStringValue())
                && !View.Last4SSN.IsNullOrEmpty())
            {
                //organizationUser.IsInstructorPreceptorAccount = true;

                //UAT-4355
                organizationUser.SSNL4 = View.Last4SSN;
                //organizationUser.SSN = View.Last4SSN;
            }

            OrganizationUser orgUserObj = SecurityManager.AddOrganizationUser(organizationUser);
            if (orgUserObj.IsNotNull())
            {
                // Updating Invitee UserID based on userType recieved
                UpdateInviteeUserID(orgUserObj);

                // Creating Mapping of shared user in OrganizationUserTypeMapping
                SecurityManager.AddOrganizationUserTypeMapping(orgUserObj.OrganizationUserID, View.UserTypeCode);

                //Synchronize ClientContact Profile
                SynchronizeClientContactProfile(orgUserObj);

                //UAT 1346: As an Agency user, I should be able to create and maintain other agency users.
                if (View.UserTypeCode == OrganizationUserType.AgencyUser.GetStringValue())
                {
                    //Assign default roles to the agency shared user
                    ApplicationDataManager.AssignDefaultRolesToAgencyUser(orgUserObj);
                    View.SkipLoginScreen = true;
                    View.AgencyUserGUID = Convert.ToString(orgUserObj.UserID);
                    //UAT-4407
                    //SecurityManager.RotationDataMovementOnAccountLinking(orgUserObj.UserID, orgUserObj.OrganizationUserID); //UAT-4407:- Currently we will not copy data on linking
                    //End
                }
                // else
                //SecurityManager.RotationDataMovementOnAccountLinking(orgUserObj.UserID, orgUserObj.OrganizationUserID); //UAT-4407:- Currently we will not copy data on linking
            }
            return true;
        }
        #endregion

        #endregion

        /// <summary>
        /// UAT-1218- Method to Validate existing Email Address
        /// </summary>
        /// <returns></returns>
        public bool IsExistsPrimaryEmail()
        {
            string userName = System.Web.Security.Membership.GetUserNameByEmail(View.EmailAddress);
            if (userName.IsNullOrEmpty())
            {
                UserAuthRequest tempObject = SecurityManager.GetUserAuthRequestByEmail(View.EmailAddress);
                if (tempObject.IsNotNull())
                    return true;
                return false;
            }
            return true;
        }

        /// <summary>        
        /// Add Update linked shared user profile
        /// </summary>
        /// <returns></returns>
        public Boolean AddUpdateLinkedUserProfile()
        {
            OrganizationUser orgUserObj = SecurityManager.GetOrgUserIfUsernameExistInSecuritytDB(View.LoginUserName);

            if (orgUserObj.IsNotNull())
            {
                //UAT-4355
                //If we are linking 2 profiles then we must add SSNL4 if its entered in creation of an instructor.
                if (View.UserTypeCode == OrganizationUserType.Instructor.GetStringValue() || View.UserTypeCode == OrganizationUserType.Preceptor.GetStringValue())
                {
                    if (!View.Last4SSN.IsNullOrEmpty())
                        orgUserObj.SSNL4 = View.Last4SSN;
                        //orgUserObj.SSN = View.Last4SSN;
                    
                }

                // Updating Invitee UserID based on userType recieved
                UpdateInviteeUserID(orgUserObj);

                // Creating Mapping of shared user in OrganizationUserTypeMapping
                SecurityManager.AddOrganizationUserTypeMapping(orgUserObj.OrganizationUserID, View.UserTypeCode);

                //Synchronize ClientContact Profile
                SynchronizeClientContactProfile(orgUserObj);

                //UAT 1346: As an Agency user, I should be able to create and maintain other agency users.
                if (View.UserTypeCode == OrganizationUserType.AgencyUser.GetStringValue())
                {
                    //Assign default roles to the agency shared user
                    ApplicationDataManager.AssignDefaultRolesToAgencyUser(orgUserObj);
                    View.SkipLoginScreen = true;
                    View.AgencyUserGUID = Convert.ToString(orgUserObj.UserID);
                    //End
                }
                return true;
            }
            return false;
        }

        public Dictionary<String, ApplicantInsituteDataContract> GetDataByKey(String key)
        {
            Object applicationData = ApplicationDataManager.GetObjectDataByKey(key);
            return ApplicationDataManager.DeserializeDictionaryValues<ApplicantInsituteDataContract>(applicationData);
        }

        public void AddWebAgencyUserData(String key, Dictionary<String, ApplicantInsituteDataContract> value)
        {
            Dictionary<String, String> serializedData = ApplicationDataManager.SerializeDictionaryValues<ApplicantInsituteDataContract>(value);
            ApplicationDataManager.AddWebApplicationData(key, serializedData, 300);
        }

        public void UpdateWebAgencyUserData(String key, Dictionary<String, ApplicantInsituteDataContract> value)
        {
            Dictionary<String, String> serializedData = ApplicationDataManager.SerializeDictionaryValues<ApplicantInsituteDataContract>(value);
            ApplicationDataManager.UpdateWebApplicationData(key, serializedData);
        }

    }
}
