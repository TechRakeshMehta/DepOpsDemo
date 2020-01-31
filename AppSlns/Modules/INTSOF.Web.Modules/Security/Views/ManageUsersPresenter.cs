#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ManageUsersPresenter.cs
// Purpose:   
//

#endregion

#region Namespace

#region System Defined

using System;
using System.Collections.Generic;
using System.Web.Security;
using INTSOF.SharedObjects;
using System.Linq;

#endregion

#region Application Specific

using Business.RepoManagers;
using Entity;
using INTSOF.Utils;

#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel.Views
{
    /// <summary>
    /// This class has the method's implementation which performs all the CRUD(Create/ Read/ Update/ Delete) operation for managing user with its details.
    /// </summary>
    public class ManageUsersPresenter : Presenter<IManageUsersView>
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        #endregion

        #region Private Properties

        #endregion

        #endregion

        #region Events

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// This method is invoked by the view every time it loads.
        /// </summary>
        public override void OnViewLoaded()
        {
            //View.ViewContract.TenantName = SecurityManager.GetTenantName(View.ViewContract.TenantId, View.ViewContract.AssignToProductId, View.ViewContract.AssignToDepartmentId);
            // GetTenantName();
        }

        public void GetTenantName()
        {
            View.ViewContract.TenantName = SecurityManager.GetTenantName(View.ViewContract.TenantId, View.ViewContract.AssignToProductId, View.ViewContract.AssignToDepartmentId);
        }

        /// <summary>
        /// Performs an insert operation for User with it's details.
        /// </summary>
        public void AddUser()
        {
            aspnet_Applications application = SecurityManager.GetApplication();
            aspnet_Users aspnetUsers = new aspnet_Users();
            aspnet_Membership memberShip = new aspnet_Membership();
            OrganizationUser organizationUser = new OrganizationUser();

            //if (SecurityManager.IsUserExists(View.ViewContract.UserName))
            if (!System.Web.Security.Membership.GetUser(View.ViewContract.UserName).IsNullOrEmpty())
            {
                View.ErrorMessage = View.ViewContract.UserName + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.SECURITY_USER_EXISTS);
            }
            else if (!System.Web.Security.Membership.GetUserNameByEmail(View.ViewContract.EmailAddress).IsNullOrEmpty())
            {
                View.ErrorMessage = View.ViewContract.EmailAddress + SysXUtils.GetMessage(ResourceConst.SPACE) + "Email address already exists.";
            }
            else
            {
                aspnetUsers.MobileAlias = View.ViewContract.MobileAlias;
                aspnetUsers.LastActivityDate = DateTime.MaxValue;

                // Email address will be used as a username when the Organization is of Supplier, else the username filed value will be used as username.
                aspnetUsers.UserName = IsOrganizationOfSupplier(Convert.ToInt32(View.ViewContract.Organizations)) ? View.ViewContract.EmailAddress : String.Concat(View.ViewContract.PrefixName, View.ViewContract.UserName);
                aspnetUsers.LoweredUserName = aspnetUsers.UserName.ToLower();
                aspnetUsers.ApplicationId = application.ApplicationId;
                aspnetUsers.UserId = Guid.NewGuid();

                memberShip.PasswordSalt = SysXMembershipUtil.GenerateSalt();
                //UAT-985: WB: Simplification of Admin Account creation.
                //memberShip.Password = SysXMembershipUtil.HashPasswordIWithSalt(View.ViewContract.DefaultPassword, memberShip.PasswordSalt);
                memberShip.Password = SysXMembershipUtil.HashPasswordIWithSalt(View.ViewContract.Password, memberShip.PasswordSalt);
                memberShip.Email = View.ViewContract.EmailAddress;
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
                memberShip.IsLockedOut = View.ViewContract.IsLockedOut;
                aspnetUsers.aspnet_Membership = memberShip;

                organizationUser.Organization = SecurityManager.GetOrganization(Convert.ToInt32(View.ViewContract.Organizations));
                organizationUser.aspnet_Users = aspnetUsers;
                organizationUser.FirstName = View.ViewContract.FirstName;
                organizationUser.LastName = View.ViewContract.LastName;
                organizationUser.CreatedOn = DateTime.Now;
                organizationUser.CreatedByID = View.CurrentUserId;
                //UAT-985: WB: Simplification of Admin Account creation.
                //organizationUser.IsNewPassword = true;
                organizationUser.IsNewPassword = View.ViewContract.IsNewPassword;
                organizationUser.IsDeleted = false;
                organizationUser.IsActive = View.ViewContract.Active;
                //Added IsApplicant status    
                organizationUser.IsApplicant = View.ViewContract.IsApplicant;
                organizationUser.IsOutOfOffice = false;
                organizationUser.IgnoreIPRestriction = true;
                organizationUser.IsMessagingUser = true;
                organizationUser.IsSystem = false;
                // UAT 891
                organizationUser.IsInternalMsgEnabled = true;

                organizationUser.IsMessagingUser = View.ViewContract.IsMessagingUse;
                //UAT-2447
                organizationUser.IsInternationalPhoneNumber = View.ViewContract.IsInternationalPhoneNumber;

                SecurityManager.AddOrganizationUser(organizationUser, aspnetUsers);

                if (!String.IsNullOrEmpty(View.ViewContract.AssignToRoleId))
                {
                    var newMappedRoleList = new List<String> { View.ViewContract.AssignToRoleId };
                    //UAT-985: WB: Simplification of Admin Account creation.
                    //SecurityManager.SaveMappingOfRolesWithSelectedUser(organizationUser, newMappedRoleList, View.ViewContract.AssignToRoleName, View.ViewContract.DefaultPassword);
                    SecurityManager.SaveMappingOfRolesWithSelectedUser(organizationUser, newMappedRoleList, View.ViewContract.AssignToRoleName, View.ViewContract.Password);

                    //UAT-3228
                    if (View.CopyFromClientAdminOrgID.IsNullOrEmpty() || View.CopyFromClientAdminOrgID == AppConsts.NONE)
                    {
                        SecurityManager.InsertDefaultColumnConfiguration(View.CurrentUserId, organizationUser, newMappedRoleList);
                    }
                }

                if (!View.CopyFromClientAdminOrgID.IsNullOrEmpty() && View.CopyFromClientAdminOrgID > AppConsts.NONE)
                {
                    ClientSecurityManager.CopyClientAdminPermissions(View.SelectedTenantId, View.CopyFromClientAdminUserID, organizationUser.UserID, View.CurrentUserId, organizationUser.OrganizationUserID);
                    //UAT-3228
                    SecurityManager.CopyDefaultColumnConfiguration(View.SelectedTenantId, View.CopyFromClientAdminUserID, organizationUser.OrganizationUserID, View.CurrentUserId);
                }

                // Sets default subscription for user
                SetDefaultSubscription(organizationUser.OrganizationUserID);


                View.SuccessMessage = SysXUtils.GetMessage(ResourceConst.USER) + SysXUtils.GetMessage(ResourceConst.SPACE) + organizationUser.FirstName + SysXUtils.GetMessage(ResourceConst.SPACE) + organizationUser.LastName + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.SAVED_SUCCESSFULLY);
            }
        }



        /// <summary>
        /// Retrieves a list of all users with it's details.
        /// </summary>
        public void RetrievingUsers()
        {
            View.ManageUserCustomPaging.DefaultSortExpression = "OrganizationUserID";

            // This section handles the listing of users when the Manage Users page is browsed through link on Manage Departments.
            if (View.ViewContract.AssignToDepartmentId > AppConsts.NONE)
            {
                //Int32? CurrentUserId, Int32? TenantProductId, Boolean? IsAdmin, Int32? OrganizationID, Boolean? IsApplicantCheckRequred, Boolean? IsParentOrgCheckRequired, CustomPagingArgsContract sortingAndFilteringData
                //View.CreatedByOrganizationUsers = SecurityManager.GetMappedOrganizationUsers(View.IsAdmin, View.CurrentUserId).Where(condition => condition.OrganizationID == View.ViewContract.AssignToDepartmentId && condition.IsDeleted == false && condition.IsSharedUser != true).ToList();

                //CASE 1
                View.CreatedByOrganizationUsers = SecurityManager.GetMappedOrganizationUsers(View.CurrentUserId, null, View.IsAdmin, View.ViewContract.AssignToDepartmentId, null, null, null, View.ManageUserCustomPaging);
                View.MappedOrganizationUsers = View.CreatedByOrganizationUsers;
                View.ViewContract.OrganizationId = View.ViewContract.AssignToDepartmentId;
            }

                // This section handles the listing of users when the Manage Users page is browsed through link on Manage Organizations.
            else if (View.ViewContract.TenantId > AppConsts.NONE || View.ViewContract.IsLinkOnTenant) // When the Manage Users page is browsed through Manage Tenant Page.
            {
                Organization organization = SecurityManager.GetOrganizationForTenant(View.ViewContract.TenantId);

                if (!organization.IsNull())
                {
                    if (View.IsClientOnBoardingWizard || View.IsClientProfile || View.ViewContract.IsLinkOnTenant)
                    {
                        //View.CreatedByOrganizationUsers = SecurityManager.GetMappedOrganizationUsers(true, View.CurrentUserId).Where(condition => condition.OrganizationID == organization.OrganizationID && condition.IsDeleted == false && condition.IsSharedUser != true).ToList();
                        //CASE 2 - Called from 'Manage Institutions' -> 'Manage Users'
                        View.CreatedByOrganizationUsers = SecurityManager.GetMappedOrganizationUsers(null, null, true, organization.OrganizationID, null, null, null, View.ManageUserCustomPaging);
                    }
                    else
                    {
                        //View.CreatedByOrganizationUsers = SecurityManager.GetMappedOrganizationUsers(View.IsAdmin, View.CurrentUserId).Where(condition => (condition.Organization.OrganizationID == organization.OrganizationID || condition.Organization.ParentOrganizationID == organization.OrganizationID) && condition.IsDeleted == false && condition.IsSharedUser != true).ToList();
                        //CASE 3
                        View.CreatedByOrganizationUsers = SecurityManager.GetMappedOrganizationUsers(View.CurrentUserId, null, View.IsAdmin, organization.OrganizationID, null, true, null, View.ManageUserCustomPaging);
                    }

                    View.MappedOrganizationUsers = View.CreatedByOrganizationUsers;
                    View.ViewContract.OrganizationId = organization.OrganizationID;
                }
            }
            else
            {
                // Check for the case that the Manage user page is browsed through Manage Users link on Manage Role , while coming through Manage Tenant Section.
                if (View.ViewContract.AssignToProductId > AppConsts.NONE)
                {
                    //CASE -- Called from 'Manage Roles' -> 'Manage Users'
                    //View.MappedOrganizationUsers = View.CreatedByOrganizationUsers = SecurityManager.GetUsersByProductId(View.ViewContract.AssignToProductId).Where(cond => cond.IsSharedUser != true).ToList();
                    View.MappedOrganizationUsers = View.CreatedByOrganizationUsers = SecurityManager.GetMappedOrganizationUsers(null, View.ViewContract.AssignToProductId, null, null, null, null, View.ViewContract.AssignToRoleId, View.ManageUserCustomPaging);
                }
                else
                {
                    //Check for removal of applicant records
                    //View.MappedOrganizationUsers = View.CreatedByOrganizationUsers = SecurityManager.GetMappedOrganizationUsers(View.IsAdmin, View.CurrentUserId).Where(condition => condition.IsApplicant == false && condition.IsSharedUser != true).ToList();
                    //CASE 4 - Called from 'Manage Users'
                    View.MappedOrganizationUsers = View.CreatedByOrganizationUsers = SecurityManager.GetMappedOrganizationUsers(View.CurrentUserId, null, View.IsAdmin, null, true, null, null, View.ManageUserCustomPaging);
                }
            }

            //These Properties are set when stored procedure is called with View.ManageUserCustomPaging
            View.VirtualPageCount = View.ManageUserCustomPaging.VirtualPageCount;
            View.CurrentPageIndex = View.ManageUserCustomPaging.CurrentPageIndex;

            if (String.IsNullOrEmpty(View.ViewContract.AssignToRoleId))
            {
                return;
            }

            //aspnet_Roles aspnetRoles = SecurityManager.GetRolesByAssignToRoleId(false, View.ViewContract.AssignToRoleId).SingleOrDefault();

            //if (!aspnetRoles.IsNull())
            //{
            //    List<aspnet_Users> aspnetUsers = SecurityManager.GetUsersInRole(aspnetRoles.RoleName);
            //    List<String> aspnetallusr = aspnetUsers.Select(aspnetUserDetail => aspnetUserDetail.UserId.ToString()).ToList();
            //    View.CreatedByOrganizationUsers = View.CreatedByOrganizationUsers.Where(createdByOrganizationUserDetails => aspnetallusr.Contains(createdByOrganizationUserDetails.UserID.ToString()) && createdByOrganizationUserDetails.IsSharedUser != true).ToList();
            //}

            //View.MappedOrganizationUsers = View.CreatedByOrganizationUsers;
        }

        public Boolean IsExistsPrimaryEmail()
        {
            //UAT-2199: checks whether email addtress exists in aspnet_membership based on user id.
            String UserID = SecurityManager.GetUserIdByEmail(View.ViewContract.EmailAddress);
            /// String userName = System.Web.Security.Membership.GetUserNameByEmail(View.ViewContract.EmailAddress);
            if (UserID == View.ViewContract.UserId)
            {
                return false;
            }
            return !String.IsNullOrEmpty(UserID);
        }
        //UAT-2199: checks whether username exists in aspnet_users based on user id.
        public Boolean IsExistsUserName()
        {
            String UserID = SecurityManager.GetUserIdByUserName(View.ViewContract.UserName);
            if (UserID == View.ViewContract.UserId)
            {
                return false;
            }
            return !String.IsNullOrEmpty(UserID);
        }

        /// <summary>
        /// Performs an update operation for User with it's details.
        /// </summary>
        public void UpdateUser()
        {
            if (IsExistsPrimaryEmail())
            {
                View.ErrorMessage = View.ViewContract.EmailAddress + SysXUtils.GetMessage(ResourceConst.SPACE) + "Email address already exists.";
            }
            else if (IsExistsUserName())
            {
                View.ErrorMessage = View.ViewContract.UserName + SysXUtils.GetMessage(ResourceConst.SPACE) + "Username already exists.";
            }
            else
            {
                if (SecurityManager.IsUserExists(View.ViewContract.UserName, View.ViewContract.ExistingUserName))
                {
                    View.ErrorMessage = View.ViewContract.UserName + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.SECURITY_USER_EXISTS);
                }
                else
                {
                    OrganizationUser existingOrganizationUser = SecurityManager.GetOrganizationUser(View.ViewContract.OrganizationUserId);
                    if (existingOrganizationUser.IsApplicant == true)
                    {
                        UpdateUserFields(existingOrganizationUser);
                        View.SuccessMessage = SysXUtils.GetMessage(ResourceConst.USER) + SysXUtils.GetMessage(ResourceConst.SPACE) + existingOrganizationUser.FirstName + SysXUtils.GetMessage(ResourceConst.SPACE) + existingOrganizationUser.LastName + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.UPDATED_SUCCESSFULLY);
                    }
                    else
                    {
                        List<OrganizationUser> lstOrganizationUser = SecurityManager.GetOrganizationUserListForUserId(View.ViewContract.UserId);
                        foreach (OrganizationUser organizationUser in lstOrganizationUser)
                        {
                            UpdateUserFields(organizationUser);
                            if (!View.CopyFromClientAdminOrgID.IsNullOrEmpty() && View.CopyFromClientAdminOrgID > AppConsts.NONE)
                            {
                                ClientSecurityManager.CopyClientAdminPermissions(View.SelectedTenantId, View.CopyFromClientAdminUserID, organizationUser.UserID, View.CurrentUserId, organizationUser.OrganizationUserID);
                            }
                        }
                        if (lstOrganizationUser.Count() > 0)
                        {
                            View.SuccessMessage = SysXUtils.GetMessage(ResourceConst.USER) + SysXUtils.GetMessage(ResourceConst.SPACE) + lstOrganizationUser.FirstOrDefault().FirstName + SysXUtils.GetMessage(ResourceConst.SPACE) + lstOrganizationUser.FirstOrDefault().LastName + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.UPDATED_SUCCESSFULLY);
                        }
                    }
                }
            }
        }

        private void UpdateUserFields(OrganizationUser organizationUser)
        {
            organizationUser.aspnet_Users.MobileAlias = View.ViewContract.MobileAlias;
            // organizationUser.Organization = SecurityManager.GetOrganization(Convert.ToInt32(View.ViewContract.Organizations));
            organizationUser.FirstName = View.ViewContract.FirstName;
            organizationUser.LastName = View.ViewContract.LastName;
            organizationUser.IsActive = View.ViewContract.Active;
            //UAT-887: WB: Delay Automatic emails going out after activation
            if (organizationUser.ActiveDate == null && organizationUser.IsApplicant == true)
                organizationUser.ActiveDate = DateTime.Now;
            //Updated the User IsApplicant Status
            //organizationUser.IsApplicant = View.ViewContract.IsApplicant;
            organizationUser.ModifiedByID = View.CurrentUserId;
            organizationUser.ModifiedOn = DateTime.Now;
            organizationUser.aspnet_Users.aspnet_Membership.Email = View.ViewContract.EmailAddress;
            organizationUser.aspnet_Users.aspnet_Membership.LoweredEmail = View.ViewContract.EmailAddress.ToLower();
            //UAT-2199: As an adb admin, I should be able to edit admin usernames on the manage users screen.
            organizationUser.aspnet_Users.UserName = View.ViewContract.UserName;
            //UAT-2447
            organizationUser.IsInternationalPhoneNumber = View.ViewContract.IsInternationalPhoneNumber;
            if (organizationUser.aspnet_Users.aspnet_Membership.IsLockedOut != View.ViewContract.IsLockedOut)
            {
                OrganizationUser organizationUserDetails = SecurityManager.GetOrganizationUser(Convert.ToInt32(View.ViewContract.OrganizationUserId));

                View.ViewContract.EmailAddress = organizationUserDetails.aspnet_Users.aspnet_Membership.Email;
                View.ViewContract.FirstName = organizationUserDetails.FirstName;
                View.ViewContract.LastName = organizationUserDetails.LastName;
                organizationUser.aspnet_Users.aspnet_Membership.IsLockedOut = View.ViewContract.IsLockedOut;
                organizationUser.aspnet_Users.aspnet_Membership.FailedPasswordAttemptCount = AppConsts.NONE;

                if (!organizationUser.IsNewPassword)
                {
                    organizationUser.IsNewPassword = false;
                }

                View.ViewContract.IsLockedUpdate = true;
            }
            SecurityManager.UpdateOrganizationUser(organizationUser);
        }

        /// <summary>
        /// Retrieve all UserName prefix
        /// </summary>
        public void RetrievingOrganizationUserNamePrefix()
        {
            IQueryable<OrganizationUserNamePrefix> userNamePrefix = SecurityManager.GetOrganizationUserNamePrefix(View.ViewContract.OrganizationId);
            View.AllUserNamePrefix = userNamePrefix.ToList();
        }

        /// <summary>
        /// Retrieves a list of all organizations with it's details.
        /// </summary>
        public void RetrievingOrganizations()
        {
            //Added code to set Organization label, Only Organization will be shown for the super admin and all client admins
            View.ViewContract.DisplayOrgType = SysXUtils.GetMessage(ResourceConst.SECURITY_ORGANIZATION);

            if (View.ViewContract.AssignToProductId > AppConsts.NONE || View.ViewContract.IsLinkOnTenant)
            {
                Int32 tenantId = View.ViewContract.IsLinkOnTenant ? View.ViewContract.TenantId : SecurityManager.GetTenantProduct(View.ViewContract.AssignToProductId).TenantID;
                IQueryable<Organization> organizations = SecurityManager.GetOrganizationsByTenantId(tenantId);

                if (View.IsAdmin || View.ViewContract.TenantId > AppConsts.NONE || View.ViewContract.IsComingThroughTenant)
                {
                    View.AllOrganization = organizations.ToList();
                    var allOrganization = View.AllOrganization.FirstOrDefault();

                    if (!allOrganization.IsNull())
                    {
                        View.ViewContract.OrganizationId = allOrganization.OrganizationID;
                    }

                    //Commented code to set Organization label, Organization will be shown for the super admin and all client admins
                    //View.ViewContract.DisplayOrgType = SysXUtils.GetMessage(ResourceConst.SECURITY_ORGANIZATION_SLASH_DEPARTMENT);
                }
                else
                {
                    //View.AllOrganization = SecurityManager.GetDepartmentsForProductAdmin(Convert.ToInt32(View.ViewContract.AssignToProductId)).ToList();
                    View.AllOrganization = SecurityManager.GetOrganizations(View.IsAdmin, Convert.ToInt32(View.ViewContract.AssignToProductId)).ToList();
                    var allOrganization = View.AllOrganization.FirstOrDefault();

                    if (allOrganization.IsNotNull())
                    {
                        View.ViewContract.OrganizationId = allOrganization.OrganizationID;
                    }

                    //Commented code to set Organization label, Organization will be shown for the super admin and all client admins
                    //View.ViewContract.DisplayOrgType = SysXUtils.GetMessage(ResourceConst.SECURITY_ORGANIZATION_SLASH_DEPARTMENT);
                }
            }
            else if (Convert.ToInt32(View.ViewContract.OrganizationUserId).Equals(AppConsts.NONE)) // Handles the operation for the Grid in Insert Mode
            {
                var organizations = SecurityManager.GetOrganizations(View.IsAdmin, Convert.ToInt32(View.ProductId));

                if (View.IsAdmin || View.IsClientOnBoardingWizard || View.IsClientProfile)
                {
                    if (View.IsClientOnBoardingWizard || View.IsClientProfile)
                    {
                        View.AllOrganization = SecurityManager.GetOrganizationsForProduct(true, Convert.ToInt32(View.ProductId)).ToList();
                    }
                    else
                    {
                        View.AllOrganization = organizations.ToList();
                    }

                    //View.ViewContract.DisplayOrgType = SysXUtils.GetMessage(ResourceConst.SECURITY_ORGANIZATION);
                }
                else
                {
                    if (View.ViewContract.AssignToDepartmentId.Equals(AppConsts.NONE))
                    {
                        var currentUserDetails = SecurityManager.GetOrganizationUser(View.CurrentUserId);
                        var currentUserOrganizationDetails = SecurityManager.GetOrganization(currentUserDetails.OrganizationID);

                        if (currentUserOrganizationDetails.ParentOrganizationID == null && !View.IsAdmin) // It handles the case for Product Admin
                        {
                            //Commented code because there will not be department wise admin and only organization will be shown for client admin
                            //View.AllOrganization = SecurityManager.GetDepartmentsForProductAdmin(Convert.ToInt32(View.ProductId)).ToList();
                            View.AllOrganization = organizations.ToList();
                            var allOrganization = View.AllOrganization.FirstOrDefault();

                            if (allOrganization.IsNotNull())
                            {
                                View.ViewContract.OrganizationId = allOrganization.OrganizationID;
                            }

                            //View.ViewContract.DisplayOrgType = SysXUtils.GetMessage(ResourceConst.SECURITY_ORGANIZATION_SLASH_DEPARTMENT);
                        }
                        //Commented code because there will not be department wise admin and only organization will be shown for client admin
                        //else 
                        //{
                        //    View.AllOrganization = SecurityManager.GetDepartmentsForDepartmentAdmin(Convert.ToInt32(View.ProductId), View.CurrentUserId).ToList();
                        //}

                        //View.ViewContract.DisplayOrgType = SysXUtils.GetMessage(ResourceConst.SECURITY_ORGANIZATION);
                    }
                    else
                    {
                        View.AllOrganization = new List<Organization> { SecurityManager.GetOrganization(View.ViewContract.AssignToDepartmentId) };
                    }

                    //Commented code to set Organization label
                    //View.ViewContract.DisplayOrgType = SysXUtils.GetMessage(ResourceConst.SECURITY_DEPARTMENT);
                }

                return;
            }
            else // Handles the operation for the Grid in Edit Mode.
            {
                OrganizationUser organizationUser = SecurityManager.GetOrganizationUser(View.ViewContract.OrganizationUserId);

                if (!organizationUser.IsNull())
                {
                    Organization organization = SecurityManager.GetAllTypeOrganization(organizationUser.OrganizationID);

                    if (!organization.IsNull())
                    {
                        Int32 tenantProductId = Convert.ToInt32(SecurityManager.GetTenantProductId(Convert.ToInt32(organization.TenantID)));

                        if (organization.ParentOrganizationID == null && (View.IsAdmin || View.IsClientOnBoardingWizard || View.IsClientProfile))
                        {
                            if (View.IsClientOnBoardingWizard || View.IsClientProfile)
                            {
                                View.AllOrganization = SecurityManager.GetOrganizationsForProduct(true, Convert.ToInt32(View.ProductId)).ToList();
                            }
                            else
                            {
                                View.AllOrganization = SecurityManager.GetOrganizations(true, Convert.ToInt32(View.ProductId)).ToList();
                            }

                            //View.ViewContract.DisplayOrgType = SysXUtils.GetMessage(ResourceConst.SECURITY_ORGANIZATION);
                        }
                        else
                        {
                            //Modified code to get organization of client admin
                            //View.AllOrganization = SecurityManager.GetDepartmentsForProductAdmin(tenantProductId).ToList();
                            //View.ViewContract.DisplayOrgType = SysXUtils.GetMessage(ResourceConst.SECURITY_DEPARTMENT);

                            View.AllOrganization = SecurityManager.GetOrganizations(View.IsAdmin, tenantProductId).ToList();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Performs a delete operation for User.
        /// </summary>
        public void DeleteUser()
        {
            if (!SecurityManager.IsUserTiedToSeveralOtherUsers(View.ViewContract.OrganizationUserId).Equals(false))
            {
                throw new SysXException(
                    String.Format("This user can not be deleted as {0}&nbsp;{1} is tied to several other users.",
                                  SecurityManager.GetOrganizationUser(View.ViewContract.OrganizationUserId).FirstName,
                                  SecurityManager.GetOrganizationUser(View.ViewContract.OrganizationUserId).LastName));
            }

            OrganizationUser organizationUser =
                SecurityManager.GetOrganizationUser(View.ViewContract.OrganizationUserId);
            organizationUser.IsDeleted = true;
            organizationUser.ModifiedByID = View.CurrentUserId;
            organizationUser.ModifiedOn = DateTime.Now;
            SecurityManager.DeleteOrganizationUser(organizationUser);
            View.SuccessMessage = SysXUtils.GetMessage(ResourceConst.USER) + SysXUtils.GetMessage(ResourceConst.SPACE) + organizationUser.FirstName + SysXUtils.GetMessage(ResourceConst.SPACE) + organizationUser.LastName + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.DELETED_SUCCESSFULLY);
        }

        /// <summary>
        /// Performs reset of password operation.
        /// </summary>
        public void ResetPassword()
        {
            OrganizationUser organizationUserDetails = SecurityManager.GetOrganizationUsersDetails(View.ResetUserName);
            SecurityManager.UpdateOrganizationUser(ResetUserPassword(organizationUserDetails.OrganizationUserID));
        }

        /// <summary>
        /// Retrieves created by name for all users.
        /// </summary>
        public void RetrievingCreatedByName()
        {
            if (!View.ViewContract.OrganizationUserId.IsNull())
            {
                OrganizationUser createdByUserDetails = SecurityManager.GetOrganizationUser(View.ViewContract.CreatedById);

                if (!createdByUserDetails.IsNull())
                {
                    View.ViewContract.CreatedByUserFullName = createdByUserDetails.LastName + SysXUtils.GetMessage(ResourceConst.SECURITY_COMMA) + SysXUtils.GetMessage(ResourceConst.SPACE) + createdByUserDetails.FirstName;
                }
                else
                {
                    View.ViewContract.CreatedByUserFullName = SysXUtils.GetMessage(ResourceConst.SPACE);
                }
            }
            else
            {
                View.ViewContract.CreatedByUserFullName = SysXUtils.GetMessage(ResourceConst.SPACE);
            }
        }

        /// <summary>
        /// Check Organization/Department For the Current User.
        /// </summary>
        public void IsDepartmentOrOrganizationExistsForProduct()
        {
            View.ViewContract.IsMyOrganizationExists = SecurityManager.IsDepartmentOrOrganizationExistsForProduct(View.IsAdmin, View.ProductId);
        }

        /// <summary>
        /// Checks whether the currently selected organization is of the supplier type tenant.
        /// </summary>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        public Boolean IsOrganizationOfSupplier(Int32 selectedOrganizationId)
        {
            return SecurityManager.IsOrganizationOfSupplier(selectedOrganizationId);
        }

        #endregion

        #region Private Methods

        private OrganizationUser ResetUserPassword(Int32 organizationUserId)
        {
            OrganizationUser organizationUser = SecurityManager.GetOrganizationUser(organizationUserId);
            organizationUser.aspnet_Users.aspnet_Membership.Password = SysXMembershipUtil.HashPasswordIWithSalt(View.ViewContract.Password, organizationUser.aspnet_Users.aspnet_Membership.PasswordSalt);
            organizationUser.aspnet_Users.aspnet_Membership.IsLockedOut = false;
            organizationUser.aspnet_Users.aspnet_Membership.FailedPasswordAttemptCount = AppConsts.NONE;
            organizationUser.aspnet_Users.aspnet_Membership.LastPasswordChangedDate = DateTime.Now;
            organizationUser.IsNewPassword = true;

            View.ViewContract.EmailAddress = organizationUser.aspnet_Users.aspnet_Membership.Email;
            View.ViewContract.FirstName = organizationUser.FirstName;
            View.ViewContract.LastName = organizationUser.LastName;

            return organizationUser;
        }

        #region Set Default Subscription

        /// <summary>
        /// 
        /// </summary>
        Int32 notificationCommunicationTypeId = 0;
        private Int32 NotificationCommunicationTypeId
        {
            get
            {
                if (notificationCommunicationTypeId == 0)
                    notificationCommunicationTypeId = MessageManager.GetCommuncationTypeByCode(lkpCommunicationTypeContext.NOTIFICATION.GetStringValue()).CommunicationTypeID;
                return notificationCommunicationTypeId;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        Int32 alertCommunicationTypeId = 0;
        private Int32 AlertCommunicationTypeId
        {
            get
            {
                if (alertCommunicationTypeId == 0)
                    alertCommunicationTypeId = MessageManager.GetCommuncationTypeByCode(lkpCommunicationTypeContext.ALERTS.GetStringValue()).CommunicationTypeID;
                return alertCommunicationTypeId;

            }
        }

        /// <summary>
        /// 
        /// </summary>
        Int32 reminderCommunicationTypeId = 0;
        private Int32 ReminderCommunicationTypeId
        {
            get
            {
                if (reminderCommunicationTypeId == 0)
                    reminderCommunicationTypeId = MessageManager.GetCommuncationTypeByCode(lkpCommunicationTypeContext.REMINDERS.GetStringValue()).CommunicationTypeID;
                return reminderCommunicationTypeId;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationUserId"></param>
        private void SetDefaultSubscription(Int32 organizationUserId)
        {

            List<UserCommunicationSubscriptionSetting> userCommunicationSubscriptionSettings = new List<UserCommunicationSubscriptionSetting>();
            List<UserCommunicationSubscriptionSetting> mappedSubscriptionSettings = null;
            IEnumerable<lkpCommunicationEvent> communicationEvents = null;

            communicationEvents = CommunicationManager.GetCommunicationEvents(AlertCommunicationTypeId);
            mappedSubscriptionSettings = GetMappedUserCommunicationSubscriptionSettings(organizationUserId, View.CurrentUserId, AlertCommunicationTypeId, communicationEvents);
            if (mappedSubscriptionSettings != null && mappedSubscriptionSettings.Count > 0)
                userCommunicationSubscriptionSettings.AddRange(mappedSubscriptionSettings);

            communicationEvents = CommunicationManager.GetCommunicationEvents(NotificationCommunicationTypeId);
            mappedSubscriptionSettings = GetMappedUserCommunicationSubscriptionSettings(organizationUserId, View.CurrentUserId, NotificationCommunicationTypeId, communicationEvents);
            if (mappedSubscriptionSettings != null && mappedSubscriptionSettings.Count > 0)
                userCommunicationSubscriptionSettings.AddRange(mappedSubscriptionSettings);

            communicationEvents = CommunicationManager.GetCommunicationEvents(ReminderCommunicationTypeId);
            mappedSubscriptionSettings = GetMappedUserCommunicationSubscriptionSettings(organizationUserId, View.CurrentUserId, ReminderCommunicationTypeId, communicationEvents);
            if (mappedSubscriptionSettings != null && mappedSubscriptionSettings.Count > 0)
                userCommunicationSubscriptionSettings.AddRange(mappedSubscriptionSettings);

            if (userCommunicationSubscriptionSettings != null && userCommunicationSubscriptionSettings.Count > 0)
                CommunicationManager.AddUserCommunicationSubscriptionSettings(userCommunicationSubscriptionSettings);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationUserId"></param>
        /// <param name="ById"></param>
        /// <param name="communicationTypeId"></param>
        /// <param name="communicationEvents"></param>
        /// <returns></returns>
        private List<UserCommunicationSubscriptionSetting> GetMappedUserCommunicationSubscriptionSettings(
            Int32 organizationUserId,
            Int32 ById,
            Int32 communicationTypeId,
            IEnumerable<lkpCommunicationEvent> communicationEvents)
        {
            List<UserCommunicationSubscriptionSetting> userCommunicationSubscriptionSettings = null;
            if (communicationEvents != null && communicationEvents.Count() > 0)
            {
                userCommunicationSubscriptionSettings = new List<UserCommunicationSubscriptionSetting>();
                foreach (lkpCommunicationEvent communicationEvent in communicationEvents)
                {
                    userCommunicationSubscriptionSettings.Add(new UserCommunicationSubscriptionSetting()
                    {
                        OrganizationUserID = organizationUserId,
                        CommunicationTypeID = communicationTypeId,
                        CommunicationEventID = communicationEvent.CommunicationEventID,
                        IsSubscribedToAdmin = true,
                        IsSubscribedToUser = true,
                        CreatedByID = ById,
                        CreatedOn = DateTime.Now,
                        ModifiedByID = ById,
                        ModifiedOn = DateTime.Now
                    });
                }
            }
            return userCommunicationSubscriptionSettings;
        }
        #endregion

        #endregion

        #endregion

        public String GetFormattedPhoneNumber(String unformattedPhoneNumber)
        {
            return ApplicationDataManager.GetFormattedPhoneNumber(unformattedPhoneNumber);
        }

        /// <summary>
        /// UAT-2257-Create client admins to mirror permissions of existing admins
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public void GetAdminUserByTenantId(Int32 tenantId)
        {
            View.lstClientAdminUsers = SecurityManager.GetClientAdminUsersByTanentId(tenantId).Where(cond => cond.IsActive == true
                                       && cond.IsDeleted == false).Select(x => new Entity.OrganizationUser
                                       {
                                           FirstName = x.FirstName + " " + x.LastName,
                                           OrganizationUserID = x.OrganizationUserID,
                                           PrimaryEmailAddress = x.aspnet_Users.aspnet_Membership.Email,
                                           UserID = x.UserID
                                       }).ToList();
        }

        #region UAT-3360
         
        public Boolean CheckAccountLinkingExistingProfile(String username, String userEmail)
        {
            var userListsWithUsername = SecurityManager.GetOrgUserListIfUsernameExistInSecuritytDBForAccountLinking(username);
            var userListsWithUserEmail = SecurityManager.GetOrgUserListIfUsernameExistInSecuritytDBForAccountLinking(userEmail);
            var finalUserList = new List<OrganizationUser>();
            finalUserList.AddRange(userListsWithUsername);
            finalUserList.AddRange(userListsWithUserEmail);
            if (finalUserList.Count > AppConsts.NONE)
            {
                if (finalUserList.Where(obj => (obj.IsSharedUser ?? false) == false && (obj.IsApplicant ?? false) == false && obj.OrganizationID != SecurityManager.DefaultTenantID).Any())
                    return false;
                if (finalUserList.Where(obj => (obj.IsSharedUser ?? false) == false && (obj.IsApplicant ?? false) == false && obj.OrganizationID == SecurityManager.DefaultTenantID).Any())
                    return false;
                else
                    return true;
            }
            return false;
        }

        public void AddLinkedProfile()
        {
            OrganizationUser organizationUser = new OrganizationUser();
            organizationUser.Organization = SecurityManager.GetOrganization(Convert.ToInt32(View.ViewContract.Organizations));
            organizationUser.UserID = View.ExistingOrganisationUser.UserID;
            organizationUser.FirstName = View.ViewContract.FirstName;
            organizationUser.LastName = View.ViewContract.LastName;
            organizationUser.PrimaryEmailAddress = View.ViewContract.EmailAddress; //View.ExistingOrganisationUser.aspnet_Users.aspnet_Membership.Email;
            organizationUser.CreatedOn = DateTime.Now;
            organizationUser.CreatedByID = View.CurrentUserId;
            organizationUser.IsNewPassword = View.ViewContract.IsNewPassword;
            organizationUser.IsDeleted = false;
            organizationUser.IsActive = View.ViewContract.Active;
            organizationUser.IsApplicant = View.ViewContract.IsApplicant;
            organizationUser.IsOutOfOffice = false;
            organizationUser.IgnoreIPRestriction = true;
            organizationUser.IsMessagingUser = true;
            organizationUser.IsSystem = false;
            organizationUser.IsInternalMsgEnabled = true;
            organizationUser.IsMessagingUser = View.ViewContract.IsMessagingUse;
            organizationUser.IsInternationalPhoneNumber = View.ViewContract.IsInternationalPhoneNumber;


            OrganizationUser orgUserObj = SecurityManager.AddOrganizationUser(organizationUser);
            if (orgUserObj.IsNotNull())
            {
                if (!String.IsNullOrEmpty(View.ViewContract.AssignToRoleId))
                {
                    var newMappedRoleList = new List<String> { View.ViewContract.AssignToRoleId };
                    //UAT-985: WB: Simplification of Admin Account creation.
                    //SecurityManager.SaveMappingOfRolesWithSelectedUser(organizationUser, newMappedRoleList, View.ViewContract.AssignToRoleName, View.ViewContract.DefaultPassword);
                    SecurityManager.SaveMappingOfRolesWithSelectedUser(organizationUser, newMappedRoleList, View.ViewContract.AssignToRoleName, View.ViewContract.Password);

                    //UAT-3228
                    if (View.CopyFromClientAdminOrgID.IsNullOrEmpty() || View.CopyFromClientAdminOrgID == AppConsts.NONE)
                    {
                        SecurityManager.InsertDefaultColumnConfiguration(View.CurrentUserId, organizationUser, newMappedRoleList);
                    }
                }

                if (!View.CopyFromClientAdminOrgID.IsNullOrEmpty() && View.CopyFromClientAdminOrgID > AppConsts.NONE)
                {
                    ClientSecurityManager.CopyClientAdminPermissions(View.SelectedTenantId, View.CopyFromClientAdminUserID, organizationUser.UserID, View.CurrentUserId, organizationUser.OrganizationUserID);
                    //UAT-3228
                    SecurityManager.CopyDefaultColumnConfiguration(View.SelectedTenantId, View.CopyFromClientAdminUserID, organizationUser.OrganizationUserID, View.CurrentUserId);
                }

                // Sets default subscription for user
                SetDefaultSubscription(organizationUser.OrganizationUserID);


                View.SuccessMessage = SysXUtils.GetMessage(ResourceConst.USER) + SysXUtils.GetMessage(ResourceConst.SPACE) + organizationUser.FirstName + SysXUtils.GetMessage(ResourceConst.SPACE) + organizationUser.LastName + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.SAVED_SUCCESSFULLY);
            }
            
        }

        public void BindExistingProfile()
        {
            View.ExistingOrganisationUser = SecurityManager.GetOrgUserIfUsernameExistInSecuritytDBForAccountLinking(View.SelectedLinkingProfileOrgUsername);
        }

        #endregion

        #region UAT- 4306
        public string GetInstitutionURL()
        {
            String institutionURL = WebSiteManager.GetInstitutionUrl(View.SelectedTenantId);
            return institutionURL;
        }
        #endregion
    }
}