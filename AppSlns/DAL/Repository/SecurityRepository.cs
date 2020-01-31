#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  SecurityRepository.cs
// Purpose:
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Linq;
using System.Data.Entity.Core.Objects;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects.DataClasses;

#endregion

#region Application Specific

using DAL.Interfaces;
using Entity;
using INTSOF.Utils;
using INTSOF.Utils.Consts;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.AppFabricCacheServer;
using INTSOF.UI.Contract.SysXSecurityModel;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.UI.Contract.DataFeed_Framework;
using INTSOF.UI.Contract.Services;
using INTSOF.UI.Contract.ProfileSharing;
using INTSOF.UI.Contract.PackageBundleManagement;
using INTSOF.UI.Contract.IntsofSecurityModel;
using INTSOF.UI.Contract.CommonControls;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.UI.Contract.Alumni;
using INTSOF.UI.Contract.PersonalSettings;
using INTSOF.UI.Contract.SearchUI;
using INTSOF.UI.Contract.QueueManagement; //UAT-3326
//using Microsoft.Data.Extensions;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation; //UAT-4013
using INTSOF.UI.Contract.AdminEntryPortal;
using INTSOF.UI.Contract.RecounciliationQueue;

#endregion

#endregion

namespace DAL.Repository
{
    /// <summary>
    /// This is the class for security module. 
    /// All functionality related to security module like : Role(Edit/Add/Delete), User(Edit/Add/Delete) will be written in this class only.
    /// </summary>
    public class SecurityRepository : BaseRepository, ISecurityRepository
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private SysXAppDBEntities _dbNavigation;
        private List<Int32> featureIdsToBeDeleted = new List<Int32>();

        private const string _USP_GETMENUITEMS = "GetMenuFeatures";
        private const string _PARAM_USERID = "UserID";
        private const string _PARAM_BLOCKID = "BlockID";
        private const string _PARAM_UI_BUSINESSCHANNEL_TYPEID = "BusinessChannelTypeID";

        #endregion

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor to initialize class level variables.
        /// </summary>
        public SecurityRepository()
        {
            _dbNavigation = base.Context;
        }

        #endregion

        #region Properties

        #region Public Properties

        #endregion

        #region Private Properties

        /// <summary>
        /// Gets the current repository context.
        /// </summary>
        /// <remarks></remarks>
        ISecurityRepository CurrentRepositoryContext
        {
            get
            {
                return this;
            }
        }

        #endregion

        #endregion

        #region Methods

        #region Public Methods

        #region Manage User Account

        /// <summary>
        /// Count the number of attempt to reset the password.
        /// </summary>
        /// <param name="userName">The current user's name.</param>
        void ISecurityRepository.ResetPasswordAttemptCount(String userName)
        {
            aspnet_Membership user = _dbNavigation.aspnet_Membership.FirstOrDefault(aspnetMembership => aspnetMembership.aspnet_Users.UserName.ToLower() == userName.ToLower());

            if (!user.IsNull())
            {
                user.IsLockedOut = false;
                user.FailedPasswordAttemptCount = AppConsts.NONE;
                user.LastLoginDate = DateTime.Now;
            }
            _dbNavigation.SaveChanges();
        }

        /// <summary>
        /// Counts the number of failed attempt while logging in the user account.
        /// </summary>
        /// <param name="userName">The Current User's Name</param>
        /// <param name="maxPasswordAttemptCount">The counter for maximum password attempt</param>
        void ISecurityRepository.FailedPasswordAttemptCount(String userName, Int32 maxPasswordAttemptCount)
        {
            aspnet_Membership user = _dbNavigation.aspnet_Membership.FirstOrDefault(aspnetMembership => aspnetMembership.aspnet_Users.UserName.ToLower() == userName.ToLower());

            if (user.IsNull())
            {
                return;
            }
            if (user.FailedPasswordAttemptCount.Equals(maxPasswordAttemptCount))
            {
                user.IsLockedOut = true;
                user.LastLockoutDate = DateTime.Now;
            }
            else
            {
                user.FailedPasswordAttemptCount = user.FailedPasswordAttemptCount + AppConsts.ONE;
            }

            _dbNavigation.SaveChanges();
        }

        /// <summary>
        /// Query if 'currentUserId' is current user role exists.
        /// </summary>
        /// <param name="currentUserId">The current user id.</param>
        /// <returns>
        /// true if current user role exists, false if not.
        /// </returns>
        Boolean ISecurityRepository.IsCurrentUserRoleExists(Guid currentUserId)
        {
            return CompiledIsCurrentUserRoleExists(_dbNavigation, currentUserId);
        }

        /// <summary>
        /// Query if 'currentUserId' is current user role exists.
        /// </summary>
        /// <param name="currentUserId">The current user id.</param>
        /// <returns>
        /// true if current user role exists, false if not.
        /// </returns>
        Boolean ISecurityRepository.IsCurrentLoggedInUserRoleExists(Guid currentUserId)
        {
            return CompiledIsCurrentLoggedInUserRoleExists(_dbNavigation, currentUserId);
        }

        /// <summary>
        /// Query if Line of business for current user exists.
        /// </summary>
        /// <param name="userName">The current user name.</param>
        /// <returns>
        /// Default line of business for the current user.
        /// </returns>
        lkpSysXBlock ISecurityRepository.GetDefaultLineOfBusinessByUserName(String userName, Int32? tenantId)
        {
            Int32 defaultLOBId;
            String Complio_Code = BusinessChannelType.COMPLIO.GetStringValue();
            Int32 ImmunizationComplianceTypeID = _dbNavigation.lkpBusinessChannelTypes.Where(cond => cond.Code == Complio_Code).FirstOrDefault().BusinessChannelTypeID;

            aspnet_Users userInfo = _dbNavigation.aspnet_Users.Where(condition => condition.UserName == userName).FirstOrDefault();
            List<vw_UserAssignedBlocks> lstcurrentuserLOB = _dbNavigation.vw_UserAssignedBlocks.Where(condition => condition.UserId == userInfo.UserId && condition.TenantID == tenantId).ToList();
            vw_UserAssignedBlocks currentuserLOB = lstcurrentuserLOB.FirstOrDefault();
            if (!userInfo.IsNull())
            {
                OrganizationUser orgUser = userInfo.OrganizationUsers.Where(cond => cond.Organization.TenantID == tenantId).FirstOrDefault();
                if (orgUser.IsNotNull())
                {
                    if (!orgUser.SysXBlockID.IsNull() || !currentuserLOB.IsNull())
                    {
                        //UAT-2583 : Resolved SysXBlockId issue: If default buisnesschaneel of user is not assigned to user currently.
                        Boolean ifDefaultBlockMapped = true;
                        if (orgUser.SysXBlockID.IsNotNull() && !orgUser.IsSystem)
                        {
                            ifDefaultBlockMapped = lstcurrentuserLOB.Any(cond => cond.SysXBlockId == orgUser.SysXBlockID);
                            if (!ifDefaultBlockMapped)
                            {
                                //Remove default buisness channel from orgUser.
                                orgUser.SysXBlockID = null;
                                orgUser.ModifiedByID = orgUser.OrganizationUserID;
                                orgUser.ModifiedOn = DateTime.Now;
                                _dbNavigation.SaveChanges();
                            }
                        }
                        defaultLOBId = (orgUser.SysXBlockID.IsNotNull() && ifDefaultBlockMapped) ? orgUser.SysXBlockID.GetValueOrDefault() : (currentuserLOB.IsNotNull() && currentuserLOB.SysXBlockId.IsNotNull() ? currentuserLOB.SysXBlockId : AppConsts.NONE);

                        if (lstcurrentuserLOB.Count > AppConsts.NONE
                            && lstcurrentuserLOB.Where(cond => cond.SysXBlockId == defaultLOBId).FirstOrDefault().BusinessChannelTypeID != ImmunizationComplianceTypeID
                            && !orgUser.IsSystem)
                        {
                            if (lstcurrentuserLOB.Where(cond => cond.BusinessChannelTypeID == ImmunizationComplianceTypeID).Any())
                            {
                                defaultLOBId = lstcurrentuserLOB.Where(cond => cond.BusinessChannelTypeID == 1).FirstOrDefault().SysXBlockId;
                            }
                        }
                        return CurrentRepositoryContext.GetLineOfBusiness(defaultLOBId);
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// To get Third Party Organization IDs
        /// </summary>
        /// <param name="organizationID"></param>
        /// <returns></returns>
        List<Int32> ISecurityRepository.GetThirdPartyOrgIDs(Int32 organizationID)
        {
            List<Organization> orgList = new List<Organization>();
            var organization = _dbNavigation.Organizations.FirstOrDefault(x => x.OrganizationID == organizationID && x.IsDeleted == false && x.IsActive == true);

            if (organization.IsNotNull())
            {
                List<ClientRelation> clientRelations = _dbNavigation.ClientRelations.Where(x => x.TenantID == organization.TenantID && x.IsDeleted == false && x.IsActive == true).ToList();
                //var clientRelations = _dbNavigation.Organizations.Where(x => x.OrganizationID == organizationID).Select(y => y.Tenant.ClientRelations).ToList();

                if (clientRelations.IsNotNull())
                {
                    List<Int32> relatedTenantIDs = clientRelations.Select(x => x.RelatedTenantID).ToList();
                    orgList = _dbNavigation.Organizations.Where(x => relatedTenantIDs.Contains(x.TenantID ?? 0)).ToList();
                }
            }

            return orgList.Select(x => x.OrganizationID).ToList();
        }

        #region To handle: last 5 password can't be used while user tries to change their password.

        /// <summary>
        /// Finds out that is the entered password has been used earlier.
        /// </summary>
        /// <param name="currentUserId">Current user's Id.</param>
        /// <param name="newPassword">The new password entered on the form.</param>
        /// <returns></returns>
        Boolean ISecurityRepository.IsPasswordExistsInHistory(Guid currentUserId, String newPassword, Boolean isUserExixtInLocationTenants)
        {
            //UAT-4097            
            if (isUserExixtInLocationTenants)
            {
                return CompiledGetPasswordHistoryByUserId(_dbNavigation, currentUserId).ToList().Any(condition => condition.Password.Trim().Equals(SysXMembershipUtil.HashPasswordIWithSalt(newPassword, CompiledGetPasswordSaltByUserId(_dbNavigation, currentUserId))));
            }
            else
            {
                //var aa = CompiledGetPasswordHistoryByUserId(_dbNavigation, currentUserId).OrderByDescending(n => n.PasswordChangedDate).Take(4).ToList();
                return CompiledGetPasswordHistoryByUserId(_dbNavigation, currentUserId).OrderByDescending(n => n.PasswordChangedDate).Take(4).ToList().Any(condition => condition.Password.Trim().Equals(SysXMembershipUtil.HashPasswordIWithSalt(newPassword, CompiledGetPasswordSaltByUserId(_dbNavigation, currentUserId))));
            }
        }

        /// <summary>
        /// Performs an update operation for Password Details.
        /// </summary>
        /// <param name="organizationUser">OrganizationUser.</param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        /// <exception cref="Exception">Thrown when an exception error condition occurs.</exception>
        Boolean ISecurityRepository.UpdatePasswordDetails(OrganizationUser organizationUser, Int32 orgUsrID, String oldPassword)
        {
            Int32 passwordHistoryCount = CompiledGetCountOfPasswordHistoryByUserId(_dbNavigation, organizationUser.UserID);

            if (passwordHistoryCount > AppConsts.NINE)
            {
                PasswordHistory passwordHistory = CompiledGetPasswordHistoryByUserId(_dbNavigation, organizationUser.UserID).OrderBy(condition => condition.PasswordHistoryID).OrderBy(condition => condition.PasswordChangedDate).FirstOrDefault();

                if (!passwordHistory.IsNull())
                {
                    passwordHistory.UserID = organizationUser.UserID;
                    passwordHistory.PasswordChangedDate = DateTime.Now;
                    passwordHistory.Password = organizationUser.aspnet_Users.aspnet_Membership.Password;
                    //passwordHistory.CreatedByID = organizationUser.OrganizationUserID;
                    passwordHistory.CreatedByID = orgUsrID;
                    passwordHistory.CreatedOn = DateTime.Now;

                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                PasswordHistory passwordHistory = new PasswordHistory
                {
                    UserID = organizationUser.UserID,
                    PasswordChangedDate = organizationUser.aspnet_Users.aspnet_Membership.LastPasswordChangedDate,
                    Password = organizationUser.aspnet_Users.aspnet_Membership.Password,
                    CreatedByID = orgUsrID,
                    CreatedOn = DateTime.Now
                };

                base.AddObjectEntityInTransaction(passwordHistory);
                return true;
            }
        }

        #endregion

        #endregion

        #region Manage User Personalization

        public aspnet_PersonalizationPerUser GetPersonalizedPreference(Guid userID, short businessChannelTypeId)
        {
            return CompiledGetPersonalizedPreference(_dbNavigation, userID, businessChannelTypeId);
        }

        public aspnet_PersonalizationAllUsers GetGroupPreference(Int32 dashBoardUser, Int32 tenantID)
        {
            // The code is added temporary and will be reverted 

            return CompiledGetGroupPreference(_dbNavigation, dashBoardUser, tenantID);
        }

        public void ClearDashboardStates(Guid userid, short businessChannelTypeId)
        {
            aspnet_PersonalizationPerUser entity = GetPersonalizedPreference(userid, businessChannelTypeId);
            if (entity != null)
            {
                entity.LastUpdatedDate = DateTime.Now;
                entity.WidgetState = string.Empty;
                _dbNavigation.SaveChanges();
            }
        }

        public void SavePersonalizedPreference(Guid userid, Dictionary<string, string> dashboard, short businessChannelTypeId)
        {
            SavePersonalizedPreference(userid, dashboard, string.Empty, businessChannelTypeId);
        }

        public void SavePersonalizedPreference(Guid userid, Dictionary<string, string> dashboard, string pathID, short businessChannelTypeId)
        {
            aspnet_PersonalizationPerUser entity = GetPersonalizedPreference(userid, businessChannelTypeId);
            if (entity == null)
            {
                entity = new aspnet_PersonalizationPerUser();
                entity.Id = Guid.NewGuid();
                entity.UserId = userid;
                entity.LastUpdatedDate = DateTime.Now;
                entity.DashboardLayout = dashboard[AppConsts.DASHBOARD_MARKUP_KEY];
                entity.WidgetState = dashboard[AppConsts.DASHBOARD_WIDGETSTATE_KEY];
                if (!string.IsNullOrEmpty(pathID))
                    entity.PathId = new Guid(pathID);
                else
                    entity.PathId = null;
                entity.BusinessChannelTypeID = businessChannelTypeId;
                _dbNavigation.aspnet_PersonalizationPerUser.AddObject(entity);
            }
            else
            {
                entity.LastUpdatedDate = DateTime.Now;
                entity.DashboardLayout = dashboard[AppConsts.DASHBOARD_MARKUP_KEY];
                entity.WidgetState = dashboard[AppConsts.DASHBOARD_WIDGETSTATE_KEY];
            }
            _dbNavigation.SaveChanges();
        }

        public void SaveGroupPreference(aspnet_Paths path, aspnet_PersonalizationAllUsers groupPreference)
        {
            // The code is changed temporary and will be reverted 
            aspnet_PersonalizationAllUsers entity = GetGroupPreference(groupPreference.ForExternalUser, 0);//groupPreference.TenantID.To<Int32>()
            if (entity == null)
            {
                Guid appID = _dbNavigation.aspnet_Applications.FirstOrDefault().ApplicationId;
                path.ApplicationId = appID;
                path.aspnet_PersonalizationAllUsers = groupPreference;
                _dbNavigation.aspnet_Paths.AddObject(path);
            }
            else
            {
                entity.aspnet_Paths.Path = path.Path;
                entity.aspnet_Paths.LoweredPath = path.LoweredPath;
                entity.DashboardLayout = groupPreference.DashboardLayout;
                entity.WidgetState = groupPreference.WidgetState;
            }
            _dbNavigation.SaveChanges();
        }
        #endregion

        #region Cities

        /// <summary>
        /// Retrieves list of all Cities.
        /// </summary>
        /// <returns>A <see cref="City"/> list of data from the underlying data storage.</returns>
        IQueryable<City> ISecurityRepository.GetCities()
        {
            return CompiledGetCities(_dbNavigation);
        }

        #endregion

        #region Manage Countries

        /// <summary>
        /// Gets the Countries.
        /// </summary>
        /// <returns>
        /// The Countries.
        /// </returns>     
        IQueryable<Country> ISecurityRepository.GetCountries()
        {
            return CompiledGetCountries(_dbNavigation);
        }

        #endregion

        #region States

        /// <summary>
        /// Retrieves list of States with its details.
        /// </summary>
        /// <returns>A <see cref="State"/> list of data from the underlying data storage.</returns>
        IQueryable<State> ISecurityRepository.GetStates()
        {
            return CompiledGetStates(_dbNavigation);
        }



        #endregion

        #region ZipCodes

        /// <summary>
        /// Retrieves ZipCode details cod based on zipID.
        /// </summary>
        /// <param name="zipId">current zip code number</param>
        /// <returns>A <see cref="ZipCode"/> data from the underlying data storage.</returns>
        ZipCode ISecurityRepository.GetZip(Int32 zipId)
        {
            return CompiledGetZip(_dbNavigation, zipId);
        }

        IQueryable<ZipCode> ISecurityRepository.GetZipcodes()
        {
            return _dbNavigation.ZipCodes.Where(zipcode => zipcode.IsActive == true);
        }

        #endregion

        #region Manage Counties

        IQueryable<County> ISecurityRepository.GetCounties()
        {
            return _dbNavigation.Counties.Select(x => x);
        }

        #endregion

        #region Manage PermissionTypes

        /// <summary>
        /// Retrieves a list of all active permission types.
        /// </summary>
        /// <returns>A <see cref="PermissionType"/> list of data from the underlying data storage.</returns>
        IQueryable<PermissionType> ISecurityRepository.GetPermissionTypes()
        {
            return CompiledGetPermissionTypes(_dbNavigation);
        }

        /// <summary>
        /// Retrieves all active permission types based on permission type id.
        /// </summary>
        /// <param name="permissionTypeId">The permission type Id.</param>
        /// <returns></returns>
        PermissionType ISecurityRepository.GetPermissionType(Int32 permissionTypeId)
        {
            var permissionTypes = CompiledGetPermissionTypeById(_dbNavigation, permissionTypeId);
            return !permissionTypes.IsNull() ? permissionTypes : null;
        }

        /// <summary>
        /// Query if 'enteredPermissionTypeName' is permission type exists.
        /// </summary>
        /// <param name="enteredPermissionTypeName"> Name of the entered permission type.</param>
        /// <param name="existingPermissionTypeName">(optional) name of the existing permission type.</param>
        /// <returns>
        /// true if permission type exists, false if not.
        /// </returns>
        Boolean ISecurityRepository.IsPermissionTypeExists(String enteredPermissionTypeName, String existingPermissionTypeName = null)
        {
            // executes when the form is in insert mode.
            if (String.IsNullOrEmpty(existingPermissionTypeName))
            {
                return CompiledGetPermissionTypes(_dbNavigation).Where(condition => condition.Name.ToLower() == enteredPermissionTypeName.ToLower()).Count() > AppConsts.NONE;
            }
            // executes when the form is in update mode.
            else
            {
                return CompiledGetPermissionTypes(_dbNavigation).ToList().FindAll(blockDetails => blockDetails.Name.Equals(enteredPermissionTypeName, StringComparison.InvariantCultureIgnoreCase))
                    .SkipWhile(blockDetails => existingPermissionTypeName.Equals(enteredPermissionTypeName, StringComparison.InvariantCultureIgnoreCase))
                    .Any(nameChecks => nameChecks.Name.Equals(enteredPermissionTypeName, StringComparison.InvariantCultureIgnoreCase));
            }
        }

        /// <summary>
        /// Query if 'permission type' is assign to any permission.
        /// </summary>
        /// <param name="enteredPermissionName"> Id of the current permission type</param>
        /// <returns>
        /// true if permission type assign to any permission, false if not.
        /// </returns>
        Boolean ISecurityRepository.IsPermissionTypeAssignToAnyPermission(Int32 permissionTypeId)
        {
            return CompiledIsPermissionTypeAssignToAnyPermission(_dbNavigation, permissionTypeId).ToList().Count() > AppConsts.NONE;
        }

        #endregion

        #region Manage Permissions

        /// <summary>
        /// Retrieves a list of all active permissions.
        /// </summary>
        /// <returns>A <see cref="Permission"/> list of data from the underlying data storage.</returns>
        IQueryable<Permission> ISecurityRepository.GetPermissions()
        {
            return CompiledGetPermissions(_dbNavigation);
        }

        /// <summary>
        /// Retrieves a list of all permissions based on permission id.
        /// </summary>
        /// <param name="permissionId">permission id value.</param>
        /// <returns>A <see cref="Permission"/> data from the underlying data storage.</returns>
        Permission ISecurityRepository.GetPermission(Int32 permissionId)
        {
            var permission = CompiledGetPermission(_dbNavigation, permissionId);
            return !permission.IsNull() ? permission : null;
        }

        /// <summary>
        /// Retrieves a list of al Permissions based on userId, featureId, blockId.
        /// </summary>
        /// <param name="userId">The user's Id.</param>
        /// <param name="featureId">The Feature's Id.</param>
        /// <param name="blockId">The Block's Id.</param>
        /// <returns>A <see cref="Permission"/> data from the underlying data storage.</returns>
        Permission ISecurityRepository.GetPermission(String userId, List<Int32> lstFeatureID, Int32 blockId)
        {
            IQueryable<Guid> userRoles = _dbNavigation.vw_aspnet_UsersInRoles.Where(condition => condition.UserId == new Guid(userId)).Select(ss => ss.RoleId);

            IQueryable<Permission> permissions = _dbNavigation.RolePermissionProductFeatures.Include(SysXEntityConstants.TABLE_PERMISSION)
                .Where(condition => userRoles.Contains(condition.RoleId)
                            && lstFeatureID.Contains(condition.SysXBlocksFeature.FeatureID)
                            && condition.SysXBlocksFeature.SysXBlockID == blockId)
                .Select(ss => ss.Permission);

            if (permissions.Any())
            {
                return permissions.FirstOrDefault(perm => perm.Name.Equals("NoAccess")).IsNull() ?
                                (permissions.FirstOrDefault(perm => perm.Name == "ReadOnly").IsNull() ?
                                    (permissions.FirstOrDefault(perm => perm.Name == "FullAccess").IsNull() ? permissions.FirstOrDefault() : permissions.FirstOrDefault(perm => perm.Name == "FullAccess"))
                                : permissions.FirstOrDefault(perm => perm.Name == "ReadOnly"))
                            : permissions.FirstOrDefault(perm => perm.Name == "NoAccess");
            }

            return null;

            //RolePermissionProductFeature rolePermission = CompiledGetMyPermission(_dbNavigation, userId, featureId, blockId);
            //return rolePermission.IsNull() ? null : rolePermission.Permission;
        }

        IEnumerable<Permission> ISecurityRepository.GetUserGroupPermission(Guid userId, Int32 featureId, Int32 blockId)
        {

            return CompiledGetUserGroupPermission(_dbNavigation, userId, featureId, blockId).ToList();
        }

        IEnumerable<Permission> ISecurityRepository.GetUserGroupUserPermission(Guid userId, Int32 featureId, Int32 blockId)
        {
            return CompiledGetUserGroupUserPermission(_dbNavigation, userId, featureId, blockId);
        }

        /// <summary>
        /// Query if 'enteredLineOfBusinessName' is permission exists.
        /// </summary>
        /// <param name="enteredPermissionName"> Name of the entered line of business.</param>
        /// <param name="existingPermissionName">Name of the existing line of business.</param>
        /// <returns>
        /// true if permission exists, false if not.
        /// </returns>
        Boolean ISecurityRepository.IsPermissionExists(String enteredPermissionName, String existingPermissionName = null)
        {
            // executes when the form is in insert mode.
            if (String.IsNullOrEmpty(existingPermissionName))
            {
                return CompiledGetPermissions(_dbNavigation).Where(condition => condition.Name.ToLower() == enteredPermissionName.ToLower()).Count() > AppConsts.NONE;
            }
            // executes when the form is in update mode.
            else
            {
                return CompiledGetPermissions(_dbNavigation).ToList().FindAll(condition => condition.Name.Equals(enteredPermissionName, StringComparison.InvariantCultureIgnoreCase))
                    .SkipWhile(condition => existingPermissionName.Equals(enteredPermissionName, StringComparison.InvariantCultureIgnoreCase))
                    .Any(nameChecks => nameChecks.Name.Equals(enteredPermissionName, StringComparison.InvariantCultureIgnoreCase));
            }
        }

        /// <summary>
        /// Query if 'permission' is assign to any feature.
        /// </summary>
        /// <param name="enteredPermissionName"> Id of the current permission</param>
        /// <returns>
        /// true if permission assign to any feature, false if not.
        /// </returns>
        Boolean ISecurityRepository.IsPermissionAssignToAnyFeature(Int32 permissionId)
        {
            return CompiledIsPermissionAssignToAnyFeature(_dbNavigation, permissionId).ToList().Count() > AppConsts.NONE;
        }

        #endregion

        #region Manage Product's Feature

        /// <summary>
        /// Retrieves a list of  all active product features.
        /// </summary>
        /// <returns>A <see cref="Permission"/> list of data from the underlying data storage.</returns>
        IQueryable<ProductFeature> ISecurityRepository.GetProductFeatures()
        {
            try
            {
                return CompiledGetProductFeature(_dbNavigation);
            }
            catch (Exception ex)
            {
                DALUtils.LoggerService.GetLogger().Error("DAL SecurityRepository GetProductFeatures", ex);
                return null;
            }
        }


        /// <summary>
        /// Retrieves a list of all product feature based on it's id.
        /// </summary>
        /// <param name="productFeatureId">.</param>
        /// <returns>
        /// The product feature.
        /// </returns>
        ProductFeature ISecurityRepository.GetProductFeature(Int32 productFeatureId)
        {
            var productFeature = CompiledGetProductFeatureById(_dbNavigation, productFeatureId);
            return !productFeature.IsNull() ? productFeature : null;
        }

        /// <summary>
        /// Determines whether the specified feature is associated with any block.
        /// </summary>
        /// <param name="productFeatureId">The product feature id that is requested by the user.</param>
        /// <returns>
        /// true if feature associated with line of business, false if not.
        /// </returns>
        Boolean ISecurityRepository.IsFeatureAssociatedWithLineOfBusiness(Int32 productFeatureId)
        {
            return CompiledIsFeatureAssociatedWithBlock(_dbNavigation, productFeatureId) > AppConsts.NONE;
        }

        /// <summary>
        /// Determines whether the specified block is associated with any product.
        /// </summary>
        /// <param name="sysXBlockId">The Block's Id that is requested by the user.</param>
        /// <returns>
        /// true if line of business associated with product, false if not.
        /// </returns>
        Boolean ISecurityRepository.IsLineOfBusinessAssociatedWithProduct(Int32 sysXBlockId)
        {
            //TODO: Below line is commented so as to allow user to remove the Line Of Business if it not being mapped to any active product.
            //return CompiledIsBlockAssociatedWithProdcut(_dbNavigation, sysXBlockId) || CompiledIsAssetLOBorSysxBlock(_dbNavigation, sysXBlockId);

            IQueryable<Int32> tenantProductIds = _dbNavigation.TenantProductFeatures.Include(SysXEntityConstants.TABLE_SYSX_BLOCKS_FEATURE).Where(tpf => tpf.SysXBlocksFeature.SysXBlockID.Equals(sysXBlockId)).Select(sec => sec.TenantProductID).Distinct();
            return _dbNavigation.TenantProducts.Any(tp => tp.IsActive && tp.IsDeleted == false && tenantProductIds.Contains(tp.TenantProductID)) || CompiledIsAssetLOBorSysxBlock(_dbNavigation, sysXBlockId) || IsLineOfBusinessHasAssociation(sysXBlockId);
        }

        private Boolean IsLineOfBusinessHasAssociation(Int32 sysXBlockId)
        {
            return _dbNavigation.OrganizationUsers.Any(condition => condition.SysXBlockID == sysXBlockId && condition.IsDeleted == false);
        }

        /// <summary>
        /// Determines whether the specified user is tied to several other user(s).
        /// </summary>
        /// <param name="organizationUserId">The organization user id of the current user.</param>
        /// <returns>
        /// true if user tied to several other users, false if not.
        /// </returns>
        Boolean ISecurityRepository.IsUserTiedToSeveralOtherUsers(Int32 organizationUserId)
        {
            return CompiledIsUserTiedToSeveralOtherUsers(_dbNavigation, organizationUserId) > AppConsts.NONE;
        }

        /// <summary>
        /// Performs a delete operation for product feature based on it's id.
        /// </summary>
        /// <param name="productFeature">.</param>
        /// <param name="modifiedById">The value for modifiedById</param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        Boolean ISecurityRepository.DeleteProductFeaturesCascade(ProductFeature productFeature, Int32 modifiedById)
        {
            // TODO: Commented because the below code is not working for third level.
            //List<ProductFeature> childProductFeatures = CompiledGetChidlProductFeature(_dbNavigation, productFeature.ProductFeatureID).ToList();//_dbNavigation.ProductFeatures.WhereSelect(fx => fx.ParentProductFeatureID == productFeature.ProductFeatureID).ToList();

            //foreach (ProductFeature feature in childProductFeatures.ToArray())
            //{
            //    DeleteRelatedEnityOfProductFeature(feature);
            //    _dbNavigation.ProductFeatures.DeleteObject(feature);
            //}

            //DeleteRelatedEnityOfProductFeature(productFeature);
            //_dbNavigation.ProductFeatures.DeleteObject(productFeature);
            //_dbNavigation.SaveChanges();

            DeleteCascadeFeaturesbasedonCurrentProductFeatureId(productFeature.ProductFeatureID, modifiedById);
            _dbNavigation.SaveChanges();
            return true;
        }

        /// <summary>
        /// This method deletes the feature and all the dependent entry in rest of the tables.
        /// </summary>
        /// <param name="productFeatureId">The value for productFeatureId.</param>
        /// <param name="modifiedById">The value for modifiedById.</param>
        private void DeleteCurrentFeatureAndItsDependents(Int32 productFeatureId, Int32 modifiedById)
        {
            ProductFeature productFeature = _dbNavigation.ProductFeatures.FirstOrDefault(condition => condition.ProductFeatureID == productFeatureId);
            DeleteRelatedEnityOfProductFeature(productFeature);

            if (!productFeature.IsNull())
            {
                productFeature.IsDeleted = true;
                productFeature.Name = productFeature.Name + "_" + Guid.NewGuid();
                productFeature.ModifiedByID = modifiedById;
                productFeature.ModifiedOn = DateTime.Now;
                base.DeleteObjectEntityInTransaction(productFeature);
            }
        }

        /// <summary>
        /// This method gets all the dependent features one by one, and then passed to a separate method which handles the deletion.
        /// </summary>
        /// <param name="currentProductFeatureId">The value for currentProductFeatureId.</param>
        /// <param name="modifiedById">The value for modifiedById.</param>
        private void DeleteCascadeFeaturesbasedonCurrentProductFeatureId(Int32 currentProductFeatureId, Int32 modifiedById)
        {
            List<Int32> productFeatureIds = CompiledGetChidlProductFeature(_dbNavigation, currentProductFeatureId).ToList().Select(productFeatureInfo => productFeatureInfo.ProductFeatureID).ToList();

            foreach (Int32 productFeatureId in productFeatureIds)
            {
                DeleteCascadeFeaturesbasedonCurrentProductFeatureId(productFeatureId, modifiedById);
            }

            DeleteCurrentFeatureAndItsDependents(currentProductFeatureId, modifiedById);

            //if (productFeatureIds.Count <= AppConsts.NONE)
            //{
            //    DeleteCurrentFeatureAndItsDependents(currentProductFeatureId, modifiedById);
            //}
            //else
            //{
            //    foreach (Int32 productFeatureId in productFeatureIds)
            //    {
            //        DeleteCascadeFeaturesbasedonCurrentProductFeatureId(productFeatureId, modifiedById);
            //    }

            //    DeleteCurrentFeatureAndItsDependents(currentProductFeatureId, modifiedById);
            //}
        }

        //private void DeleteNode(TreeNode node)
        //{
        //    foreach (TreeNode childNode in node.Nodes)
        //    {
        //        DeleteNode(childNode);
        //    }
        //    TestDataSet.ReceiversRow receiver = node.Tag as TestDataSet.ReceiversRow;
        //    receiver.Delete();
        //    node.Remove();
        //}

        /// <summary>
        /// Retrieves a list of all features for product based on productID.
        /// </summary>
        /// <param name="productId">.</param>
        /// <returns>
        /// A <see cref="TenantProductFeature"/> list of data from the underlying data storage.
        /// </returns>
        IQueryable<TenantProductFeature> ISecurityRepository.GetFeaturesForProduct(Int32 productId)
        {
            var tenantProductFeatures = CompiledGetFeaturesForProduct(_dbNavigation, productId);
            return !tenantProductFeatures.IsNull() ? tenantProductFeatures : null;
        }

        /// <summary>
        /// Deletes the product features described by productId.
        /// </summary>
        /// <param name="productId">.</param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        Boolean ISecurityRepository.DeleteProductFeatures(Int32 productId)
        {
            var tenantProductFeatures = _dbNavigation.TenantProductFeatures.Where(tenantProdFeature => tenantProdFeature.TenantProductID == productId);

            IQueryable<TenantProductFeature> productFeatures = tenantProductFeatures;

            foreach (TenantProductFeature prodFeature in productFeatures)
            {
                DeleteObjectEntityInTransaction(prodFeature, true);
            }

            return true;
        }

        #endregion

        #region Manage SysXConfig

        /// <summary>
        /// Retrieves the application configuration value based on SysXKey.
        /// </summary>
        /// <param name="sysXKey">The sysx key value.</param>
        /// <returns>
        /// The system x coordinate configuration value.
        /// </returns>
        String ISecurityRepository.GetSysXConfigValue(String sysXKey)
        {
            SysXConfig config = CompiledGetSysxConfigValue(_dbNavigation, sysXKey);
            return config.IsNull() ? String.Empty : config.Value;
        }

        /// <summary>
        /// Retrieves the Application Configuration based on SysXKey.
        /// </summary>
        /// <param name="sysXKey">.</param>
        /// <returns>
        /// The system x coordinate configuration.
        /// </returns>
        SysXConfig ISecurityRepository.GetSysXConfig(String sysXKey)
        {
            SysXConfig config = CompiledGetSysXConfig(_dbNavigation, sysXKey);
            return config.IsNull() ? null : config;
        }

        /// <summary>
        /// Retrieves Application configurations.
        /// </summary>
        /// <returns>
        /// The system x coordinate configs.
        /// </returns>
        IQueryable<SysXConfig> ISecurityRepository.GetSysXConfigs()
        {
            return CompiledGetSysXConfigs(_dbNavigation);
        }

        /// <summary>
        /// Checks if the SysXKey exists in system database.
        /// </summary>
        /// <param name="enteredSysXKey"> The value for entered SysXKey Name. </param>
        /// <param name="existingSysXKey">The value for existing SysXKey Name. </param>
        /// <returns>
        /// true if line of business exists, false if not.
        /// </returns>
        Boolean ISecurityRepository.IsSysXKeyExists(String enteredSysXKey, String existingSysXKey = null)
        {
            // executes when the form is in insert mode.
            if (String.IsNullOrEmpty(existingSysXKey))
            {
                return _dbNavigation.SysXConfigs.Where(condition => condition.SysXKey.ToLower() == enteredSysXKey.ToLower()).Count() > AppConsts.NONE;
            }
            // executes when the form is in update mode.
            else
            {
                return _dbNavigation.SysXConfigs.ToList().FindAll(blockDetails => blockDetails.SysXKey.ToLower() == enteredSysXKey.ToLower())
                    .SkipWhile(blockDetails => existingSysXKey.ToLower() == enteredSysXKey.ToLower())
                    .Any(nameChecks => nameChecks.SysXKey.ToLower().Equals(enteredSysXKey.ToLower()));
            }
        }

        #endregion

        #region Manage Tenants

        /// <summary>
        /// Performs an insert operation for tenant using address, contact, tenant.
        /// </summary>
        /// <param name="tenant">The Tenant value.</param>
        /// <param name="address">The address value.</param>
        /// <param name="contact">The contact value.</param>
        /// <returns></returns>
        Tenant ISecurityRepository.AddTenantInTransaction(Contact contact, Tenant tenant)
        {
            //address = base.AddObjectEntityInTransaction(address);
            //tenant.ValidatedAddress = address;

            contact = base.AddObjectEntityInTransaction(contact);
            tenant.Contact = contact;


            return base.AddObjectEntity(tenant);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tenantTypeId"></param>
        /// <returns></returns>
        List<DefaultRole> ISecurityRepository.GetDefaultRolesByTenantTypeId(Int32 tenantTypeId)
        {
            return _dbNavigation.DefaultRoles.Where(obj => obj.TenantTypeID == tenantTypeId && obj.IsDeleted == false).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roles"></param>
        /// <returns></returns>
        Boolean ISecurityRepository.AddRoles(List<aspnet_Roles> roles)
        {
            if (roles != null && roles.Count > 0)
            {
                foreach (aspnet_Roles role in roles)
                    _dbNavigation.aspnet_Roles.AddObject(role);
                _dbNavigation.SaveChanges();

            }
            return true;
        }

        Boolean ISecurityRepository.SetDefaultBusinessChannel(Int32 organizationUserId)
        {
            _dbNavigation.SetDefaultBusinessChannel(organizationUserId);
            return true;
        }

        /// <summary>
        /// Set shared user default business channel
        /// </summary>
        /// <param name="organizationUserId"></param>
        /// <param name="sharedUserSysxBlockId"></param>
        /// <returns></returns>
        Boolean ISecurityRepository.SetSharedUserDefaultBusinessChannel(Int32 organizationUserId, Int32 sharedUserSysxBlockId)
        {
            OrganizationUser organizationUser = _dbNavigation.OrganizationUsers.FirstOrDefault(x => x.OrganizationUserID == organizationUserId && x.IsDeleted == false && x.IsActive == true
                                                && x.IsSharedUser == true);
            if (organizationUser.IsNotNull())
            {
                organizationUser.SysXBlockID = sharedUserSysxBlockId;
                if (_dbNavigation.SaveChanges() > 0)
                    return true;
            }
            return false;
        }


        Boolean ISecurityRepository.SetDefaultFeatures(Guid roleId)
        {
            _dbNavigation.SetDefaultFeatures(roleId);
            return true;
        }


        /// <summary>
        /// Performs a delete operation for tenant including it's departments.
        /// </summary>
        /// <param name="tenant">.</param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        Boolean ISecurityRepository.DeleteTenantWithAllDependent(Tenant tenant)
        {

            foreach (Organization organization in tenant.Organizations)
            {
                organization.IsDeleted = true;
                organization.ModifiedByID = tenant.ModifiedByID;
                organization.ModifiedOn = DateTime.Now;
                CurrentRepositoryContext.DeleteDependentUserForOrganization(organization.OrganizationID);
                base.UpdateObjectEntity(organization);
                base.DeleteObjectEntityInTransaction(organization);
            }

            foreach (TenantProduct tenantProduct in tenant.TenantProducts)
            {
                // Below code remove the entry done while mapping the product with features.
                TenantProduct product = tenantProduct;
                IQueryable<TenantProductFeature> tenantProductFeatures = _dbNavigation.TenantProductFeatures.Include(SysXEntityConstants.TABLE_FEATUREPERMISSIONS).Where(condition => condition.TenantProductID == product.TenantProductID);

                foreach (TenantProductFeature tenantProductFeature in tenantProductFeatures)
                {
                    // Remove the entry from FeaturePermission
                    foreach (FeaturePermission featurePermission in tenantProductFeature.FeaturePermissions)
                    {
                        base.DeleteObjectEntity(featurePermission);
                    }

                    // Remove the entry from TenantProductFeature
                    base.DeleteObjectEntity(tenantProductFeature);
                }
                // End

                tenantProduct.IsDeleted = true;
                tenantProduct.ModifiedByID = tenant.ModifiedByID;
                tenantProduct.ModifiedOn = DateTime.Now;
                tenantProduct.Name = tenantProduct.Name + "_" + Guid.NewGuid();

                foreach (RoleDetail roleDetail in tenantProduct.RoleDetails)
                {
                    roleDetail.IsDeleted = true;
                    roleDetail.ModifiedByID = tenant.ModifiedByID;
                    roleDetail.ModifiedOn = DateTime.Now;
                    roleDetail.Name = roleDetail.Name + "_" + Guid.NewGuid();
                    aspnet_Roles role = CurrentRepositoryContext.GetAspnetRole(roleDetail.RoleDetailID.ToString());
                    role.RoleName = role.RoleName + "_" + Guid.NewGuid();
                    role.LoweredRoleName = role.LoweredRoleName + "_" + Guid.NewGuid();
                    base.UpdateObjectEntity(roleDetail);
                }

                base.UpdateObjectEntity(tenantProduct);
                base.DeleteObjectEntityInTransaction(tenantProduct);
            }

            base.DeleteObjectEntityInTransaction(tenant);
            _dbNavigation.SaveChanges();
            return true;
        }

        /// <summary>
        /// Use to delete all users based on a particular OrganizationId.
        /// </summary>
        /// <param name="organizationId">The organization's Id value.</param>
        void ISecurityRepository.DeleteDependentUserForOrganization(Int32 organizationId)
        {
            List<OrganizationUser> organizationUsers = _dbNavigation.OrganizationUsers.Where(orgUsers => orgUsers.OrganizationID == organizationId).ToList();
            foreach (OrganizationUser organizationUser in organizationUsers.ToList())
            {
                organizationUser.IsDeleted = true;
                aspnet_Users user = CurrentRepositoryContext.GetAspnetUser(organizationUser.UserID.ToString());
                user.UserName = user.UserName + "_" + Guid.NewGuid();
                user.LoweredUserName = user.LoweredUserName + "_" + Guid.NewGuid();
                DeleteObjectEntityInTransaction(organizationUser);
            }
        }

        /// <summary>
        /// Retrieves a list of all active tenants.
        /// </summary>
        /// <returns>
        /// A <see cref="Tenant"/> list of data from the underlying data storage.
        /// </returns>
        IQueryable<Tenant> ISecurityRepository.GetTenants(Boolean SortByName, Boolean getTenantDetails = true)
        {
            if (getTenantDetails)
            {
                return (SortByName ? CompiledGetTenants(_dbNavigation).OrderBy(o => o.TenantName) : CompiledGetTenants(_dbNavigation));
            }
            else
            {
                return (SortByName ? CompiledGetTenantLists(_dbNavigation).OrderBy(o => o.TenantName) : CompiledGetTenantLists(_dbNavigation));
            }
        }

        /// <summary>
        /// Retrieves a list of all active tenants of a user.
        /// </summary>
        /// <returns>
        /// A <see cref="Tenant"/> list of data from the underlying data storage.
        /// </returns>
        IQueryable<Tenant> ISecurityRepository.GetUserTenants(String currentOrgUserId)
        {
            return CompiledGetUserTenants(_dbNavigation, currentOrgUserId.ToUpper()).OrderBy(o => o.TenantName);
        }

        /// <summary>
        /// Retrieves a list of all active tenants of a clent admin.
        /// </summary>
        /// <returns>
        /// A <see cref="Tenant"/> list of data from the underlying data storage.
        /// </returns>
        IQueryable<Tenant> ISecurityRepository.GetClientAdminTenants(String userId, Boolean isAtLoginTime = false)
        {
            Guid userGUId = new Guid(userId.ToUpper());

            IQueryable<Tenant> lstTenant = _dbNavigation.OrganizationUsers
                         .Include(SysXEntityConstants.LKPTENANTTYPE)
                         .Where(condition => condition.UserID == userGUId && condition.IsApplicant == false
                             && condition.IsDeleted == false && condition.IsSystem == false
                             && condition.Organization.IsActive == true && condition.Organization.IsDeleted == false
                             && condition.Organization.Tenant.IsActive == true
                             && condition.Organization.Tenant.IsDeleted == false)
                            .Select(con => con.Organization.Tenant).AsQueryable().OrderBy(col => col.TenantName);
            if (isAtLoginTime)
            {
                List<Int32> lstTenantId = _dbNavigation.vw_UserAssignedBlocks.Where(cond => cond.UserId == userGUId).Select(col => col.TenantID).ToList();
                return lstTenant.Where(con => lstTenantId.Contains(con.TenantID));
            }
            else
            {
                return lstTenant;
            }
        }


        /// <summary>
        /// Retrieves tenant based on tenantID.
        /// </summary>
        /// <param name="tenantId">.</param>
        /// <returns>
        /// The tenant.
        /// </returns>
        public Tenant GetTenant(Int32 tenantId)
        {
            var tenant = CompiledGetTenantById(_dbNavigation, tenantId);
            return !tenant.IsNull() ? tenant : null;
        }

        /// <summary>
        /// Retrieves a list of all tenant's product feature for each block.
        /// </summary>
        /// <param name="tenantProductId">The tenant product's Id.</param>
        /// <param name="sysXBlockId">    The Sysx block's Id.</param>
        /// <returns>
        /// A <see cref="TenantProductFeature"/> list of data from the underlying data storage.
        /// </returns>
        IQueryable<TenantProductFeature> ISecurityRepository.GetTenantProductFeaturesForLineOfBusiness(Int32 tenantProductId, Int32 sysXBlockId)
        {
            return CompiledGetTenantProductFeaturesForBlock(_dbNavigation, tenantProductId, sysXBlockId);
        }

        /// <summary>
        /// Retrieves tenant product based on tenantId.
        /// </summary>
        /// <param name="tenantId">The Tenant's Id.</param>
        /// <returns>
        /// The tenant product identifier.
        /// </returns>
        Int32? ISecurityRepository.GetTenantProductId(Int32 tenantId)
        {
            TenantProduct product = CompiledGetTenantProductId(_dbNavigation, tenantId);

            if (product.IsNull())
            {
                return null;
            }
            return product.TenantProductID;
        }

        /// <summary>
        /// Checks if the tenant exists in system database.
        /// </summary>
        /// <param name="enteredTenantName"> The entered tenant name.</param>
        /// <param name="existingTenantName">The existing Tenant name.</param>
        /// <returns>
        /// true if tenant exists, false if not.
        /// </returns>
        Boolean ISecurityRepository.IsTenantExists(String enteredTenantName, String existingTenantName = null)
        {
            // executes when the form is in insert mode.
            if (String.IsNullOrEmpty(existingTenantName))
            {
                return CompiledGetTenants(_dbNavigation).Where(tenantDetails => tenantDetails.TenantName.ToLower() == enteredTenantName.ToLower()).Count() > AppConsts.NONE;
            }
            // executes when the form is in update mode.
            else
            {
                return CompiledGetTenants(_dbNavigation).ToList().FindAll(tenantDetails => tenantDetails.TenantName.ToLower() == enteredTenantName.ToLower())
                    .SkipWhile(tenantDetails => existingTenantName.ToLower() == enteredTenantName.ToLower())
                    .Any(nameChecks => nameChecks.TenantName.ToLower().Equals(enteredTenantName.ToLower()));
            }
        }

        /// <summary>
        /// Return ZipCode by zipCodeNumber.
        /// </summary>
        /// <param name="zipCodeNumber">.</param>
        /// <returns>
        /// The zip codes by zip code number.
        /// </returns>
        IQueryable<ZipCode> ISecurityRepository.GetZipCodesByZipCodeNumber(String zipCodeNumber)
        {
            IQueryable<ZipCode> zipCodeDetails = CompiledGetStateCities.Invoke(_dbNavigation, zipCodeNumber);
            return zipCodeDetails.Count() > AppConsts.NONE ? zipCodeDetails : null;
        }
        /// <summary>
        /// Retrieves a Connection String of Tenant
        /// </summary>
        /// <param name="tenantId">The tenantId.</param>

        /// <returns>
        /// </returns>
        String ISecurityRepository.GetClientConnectionString(Int32 tenantId)
        {
            //ClientDBConfiguration tenantDBCon = _dbNavigation.ClientDBConfigurations.Where(x => x.CDB_TenantID == tenantId).FirstOrDefault();
            //return tenantDBCon.IsNull() ? string.Empty : tenantDBCon.CDB_ConnectionString;
            string tenantConnectionString = SysXCacheUtils.GetAddCacheLookup<ClientDBConfiguration>(ADBDatabaseDetail.ADBDatabaseType.SECURITY_DB.GetStringValue()).Where(fx => fx.CDB_TenantID == tenantId).Select(fd => fd.CDB_ConnectionString).FirstOrDefault();
            return tenantConnectionString;
        }

        //Tenant ISecurityRepository.GetRootTenant(Int32 tenantId)
        //{
        //    Tenant tenant = _dbNavigation.Tenants.FirstOrDefault(x => x.TenantID == tenantId);
        //    if (tenant.lkpTenantType.TenantTypeCode == TenantType.Compliance_Reviewer.GetStringValue())
        //        return _dbNavigation.ClientRelations.FirstOrDefault(x => x.RelatedTenantID == tenantId).Tenant1;

        //    return tenant;
        //}

        Boolean ISecurityRepository.IsTenantThirdPartyType(Int32 tenantId, String tenantTypeCode)
        {
            Tenant tenant = _dbNavigation.Tenants.FirstOrDefault(x => x.TenantID == tenantId && x.IsDeleted == false && x.IsActive == true);
            if (tenant.IsNotNull())
            {
                return tenant.lkpTenantType.TenantTypeCode.Equals(tenantTypeCode);
            }
            return false;
        }
        #endregion

        #region Manage Sub Tenant

        /// <summary>
        /// Return the list of SupplierRelations by supplierId
        /// </summary>
        /// <param name="supplierId">Supplier Id.</param>
        /// <param name="isParent">Is Parent</param>
        /// <returns>Supplier relation entity.</returns>
        IQueryable<ClientRelation> GetTenantRelationById(Int32 tenantId, Boolean isParent)
        {
            return _dbNavigation.ClientRelations.Where(condition => condition.TenantID.Equals(tenantId)
                                && condition.IsParent.Equals(isParent)
                                && !condition.IsDeleted);
        }

        /// <summary>
        /// retrieve a collection of child tenants.
        /// </summary>
        /// <returns></returns>
        List<Tenant> ISecurityRepository.GetChildTenants(Int32 tenantId, Boolean isParent)
        {
            List<Int32> childTenantId = GetTenantRelationById(tenantId, isParent).Select(condition => condition.RelatedTenantID).ToList();
            if (childTenantId.Count() > AppConsts.NONE)
            {
                return _dbNavigation.Tenants.Where(condition => !condition.IsDeleted && childTenantId.Contains(condition.TenantID)).ToList();
            }
            return new List<Tenant>();
        }

        /// <summary>
        /// retrieve a list of all tenants except child tenants.
        /// </summary>
        /// <returns></returns>
        List<Tenant> ISecurityRepository.GetAllTenantsForMapping(List<Int32> childTenantId)
        {
            childTenantId = childTenantId.Distinct().ToList();
            String TenantTypeCodeForReviewer = TenantType.Compliance_Reviewer.GetStringValue();
            Int32 tenantTypeIdForReviewer = _dbNavigation.lkpTenantTypes.FirstOrDefault(condition => condition.TenantTypeCode == TenantTypeCodeForReviewer && condition.IsActive).TenantTypeID;
            return _dbNavigation.Tenants.Where(condition => !condition.IsDeleted && !childTenantId.Contains(condition.TenantID) && condition.TenantTypeID == tenantTypeIdForReviewer).ToList();
        }

        /// <summary>
        /// retrieve supplier relation byrelated supplier id and supplier id.
        /// </summary>
        /// <param name="supplierId">supplierId</param>
        /// <param name="relatedSupplierId">relatedSupplierId</param>
        /// <returns></returns>
        ClientRelation ISecurityRepository.GetTenantRelationByRelatedTenantIdAndTenantId(Int32 tenantId, Int32 relatedTenantId)
        {
            return _dbNavigation.ClientRelations.Where(condition => condition.TenantID == tenantId && condition.RelatedTenantID == relatedTenantId && condition.IsActive && !condition.IsDeleted).FirstOrDefault();
        }

        List<ClientRelation> ISecurityRepository.GetClientRelationBasedonRelatedID(Int32 relatedTenantId)
        {
            return _dbNavigation.ClientRelations.Where(condition => condition.RelatedTenantID == relatedTenantId && condition.IsActive && !condition.IsDeleted).ToList();

        }

        /// <summary>
        /// Return the list of ClientRelation
        /// </summary>
        /// <param name="isParent">Is Parent</param>
        /// <param name="currentTenantId">Is Parent</param>
        /// <returns>Client Relation entity.</returns>
        IQueryable<ClientRelation> ISecurityRepository.GetClientRelation(Boolean isParent, Int32 currentTenantId)
        {
            return _dbNavigation.ClientRelations.Where(condition => condition.IsParent == isParent && condition.TenantID == currentTenantId && condition.IsDeleted.Equals(false));
        }


        Boolean ISecurityRepository.AddClientRelation(List<ClientRelation> clientChildRelationList)
        {
            foreach (ClientRelation clientRelation in clientChildRelationList)
            {
                _dbNavigation.ClientRelations.AddObject(clientRelation);
            }
            _dbNavigation.SaveChanges();
            return true;
        }
        #endregion

        #region Manage Products

        /// <summary>
        /// Enumerates get role details by product identifier in this collection.
        /// </summary>
        /// <param name="productId">.</param>
        /// <returns>
        /// An enumerator that allows foreach to be used to process get role details by product
        /// identifier in this collection.
        /// </returns>
        IQueryable<RoleDetail> ISecurityRepository.GetRoleDetailsByProductId(Int32 productId)
        {
            return CompiledGetProduct(_dbNavigation, productId).RoleDetails.Where(condition => condition.IsActive && condition.IsDeleted == false).OrderBy(role => role.CreatedOn).AsQueryable();
        }

        /// <summary>
        /// Retrieves a list of all active products.
        /// </summary>
        /// <returns>
        /// A <see cref="TenantProduct"/> list of data from the underlying data storage.
        /// </returns>
        IQueryable<TenantProduct> ISecurityRepository.GetProducts()
        {
            return CompiledGetProducts(_dbNavigation);
        }

        /// <summary>
        /// Checks if the Product exists in system database.
        /// </summary>
        /// <param name="enteredProductName"> Name of the entered product.</param>
        /// <param name="existingProductName">(optional) name of the existing product.</param>
        /// <returns>
        /// true if product exists, false if not.
        /// </returns>
        Boolean ISecurityRepository.IsProductExists(String enteredProductName, String existingProductName = null)
        {
            // executes when the form is in insert mode.
            if (String.IsNullOrEmpty(existingProductName))
            {
                return CompiledGetProducts(_dbNavigation).Where(productDetails => productDetails.Name.ToLower() == enteredProductName.ToLower()).Count() > AppConsts.NONE;
            }
            // executes when the form is in update mode.
            else
            {
                return CompiledGetProducts(_dbNavigation).ToList().FindAll(productDetails => productDetails.Name.ToLower() == enteredProductName.ToLower())
                    .SkipWhile(productDetails => existingProductName.ToLower() == enteredProductName.ToLower())
                    .Any(nameChecks => nameChecks.Name.ToLower().Equals(enteredProductName.ToLower()));
            }

        }

        /// <summary>
        /// Retrieves product details based on product id.
        /// </summary>
        /// <param name="productId">.</param>
        /// <returns>
        /// The product.
        /// </returns>
        TenantProduct ISecurityRepository.GetProduct(Int32 productId)
        {
            var product = CompiledGetProduct(_dbNavigation, productId);
            return !product.IsNull() ? product : null;
        }

        #endregion

        #region Manage Organizations

        /// <summary>
        /// Gets the organizations by tenant identifier.
        /// </summary>
        /// <param name="tenantId">The value for tenant's id.</param>
        /// <returns>
        /// The organizations by tenant identifier.
        /// </returns>
        IQueryable<Organization> ISecurityRepository.GetOrganizationsByTenantId(Int32 tenantId)
        {
            IQueryable<Organization> organizations = CompiledGetOrganizationsByTenantId(_dbNavigation, tenantId);
            return !organizations.IsNull() ? organizations : null;
        }

        /// <summary>
        /// Enumerates get organizations by current user identifier in this collection.
        /// </summary>
        /// <param name="isAdmin">      The value for isAdmin</param>
        /// <param name="productId">    The value for product's id.</param>
        /// <param name="currentUserId">The value for current user's Id.</param>
        /// <returns>
        /// An enumerator that allows foreach to be used to process get organizations by current user
        /// identifier in this collection.
        /// </returns>
        IQueryable<Organization> ISecurityRepository.GetOrganizationsByCurrentUserId(Boolean isAdmin, Int32 productId, Int32 currentUserId)
        {
            if (isAdmin)
            {
                var organizations = _dbNavigation.Organizations.Where(organizationsDetails => organizationsDetails.IsActive && organizationsDetails.IsDeleted == false && organizationsDetails.ParentOrganizationID == null);

                return organizations;
            }
            TenantProduct product = _dbNavigation.TenantProducts.Where(tenantProduct => tenantProduct.TenantProductID == productId).FirstOrDefault();

            if (!product.IsNull())
            {
                Int32 tenantId = product.TenantID;

                return _dbNavigation.Organizations.Include(SysXEntityConstants.TABLE_TENANT).Where(organizationsDetails => (organizationsDetails.IsActive && organizationsDetails.IsDeleted == false && organizationsDetails.ParentOrganizationID != null && organizationsDetails.Tenant.TenantID == tenantId) && organizationsDetails.CreatedByID == currentUserId);
            }
            return null;
        }

        /// <summary>
        /// Retrieves a list of all active organizations.
        /// </summary>
        /// <param name="isAdmin">The value for IsAdmin.</param>
        /// <param name="productId">The product's Id.</param>
        /// <returns>
        /// A <see cref="Organization"/> list of data from the underlying data storage.
        /// </returns>
        IQueryable<Organization> ISecurityRepository.GetOrganizations(Boolean isAdmin, Int32 productId)
        {
            if (isAdmin)
            {
                return _dbNavigation.Organizations.Include(SysXEntityConstants.TABLE_TENANT).Include(SysXEntityConstants.TABLE_TENANT_LKPTENANTTYPE).Where(organizationsDetails => organizationsDetails.IsActive && organizationsDetails.IsDeleted == false && organizationsDetails.ParentOrganizationID == null);
            }

            TenantProduct product = _dbNavigation.TenantProducts.Where(tenantProduct => tenantProduct.TenantProductID == productId).FirstOrDefault();
            return _dbNavigation.Organizations.Include(SysXEntityConstants.TABLE_TENANT).Include(SysXEntityConstants.TABLE_TENANT_LKPTENANTTYPE).Where(organizationsDetails => organizationsDetails.IsActive && organizationsDetails.IsDeleted == false && organizationsDetails.ParentOrganizationID == null && organizationsDetails.Tenant.TenantID == product.TenantID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IQueryable<Organization> ISecurityRepository.GetOrganizations()
        {
            return _dbNavigation.Organizations
                .Include(SysXEntityConstants.TABLE_TENANT)
                .Include(SysXEntityConstants.TABLE_TENANT_LKPTENANTTYPE)
                .Where(organizationsDetails => organizationsDetails.IsActive
                    && organizationsDetails.IsDeleted == false
                    && organizationsDetails.ParentOrganizationID == null && organizationsDetails.Tenant.TenantTypeID == (Int32)TenantType.Institution);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IQueryable<lkpGender> ISecurityRepository.GetGender()
        {
            return _dbNavigation.lkpGenders;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IQueryable<lkpSecurityQuestion> ISecurityRepository.GetSecurityQuestion()
        {
            return _dbNavigation.lkpSecurityQuestions.Where(SecurityQuestions => SecurityQuestions.IsDeleted == false);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        IQueryable<OrganizationLocation> ISecurityRepository.GetOrganizationLocations(Int32 organizationId)
        {
            return _dbNavigation.OrganizationLocations
                .Where(organizationLocation => organizationLocation.OrganizationID.Equals(organizationId));
        }


        /// <summary>
        /// Retrieves an organization details based on organizationID.
        /// </summary>
        /// <param name="organizationId">.</param>
        /// <returns>
        /// The organization.
        /// </returns>
        Organization ISecurityRepository.GetOrganization(Int32 organizationId)
        {
            Organization organization = CompiledGetOrganization(_dbNavigation, organizationId);
            return !organization.IsNull() ? organization : null;
        }

        IQueryable<Organization> ISecurityRepository.GetDepartments(Int32 organizationId)
        {
            return _dbNavigation.Organizations.Where(obj => obj.ParentOrganizationID == organizationId && obj.IsActive && obj.IsDeleted == false);
        }
        //List<DeptProgramMapping> ISecurityRepository.GetDeptFromDeptPrgMaping(Int32 organizationId)
        //{

        //    return _dbNavigation.DeptProgramMappings.Where(obj => obj.DPM_OrganizationID == organizationId).ToList();
        //}
        //List<OrganizationUserProgram> ISecurityRepository.GetProgramsFromOrgUserProgrm(Int32 organizationId)
        //{
        //    return _dbNavigation.OrganizationUserPrograms.Where(obj => obj.OrganizationUserID == organizationId).ToList();

        //}

        //Entity.ClientEntity.OrganizationUserDepartment GetUserDept(Int32 organizationId)
        //{ 
        //    return _dbNavigation


        //}
        //IQueryable<DeptProgramMapping> ISecurityRepository.GetDepartments(Int32 organizationId)
        //{
        //    return _dbNavigation.DeptProgramMappings.Where(obj => obj.DPM_OrganizationID == organizationId && obj.DPM_IsDeleted==false);
        //}

        //DeptProgramMapping ISecurityRepository.GetDepartment()
        //{ }

        /// <summary>
        /// Retrieves an organization details based on tenantID.
        /// </summary>
        /// <param name="tenantId">.</param>
        /// <returns>
        /// The organization for tenant.
        /// </returns>
        Organization ISecurityRepository.GetOrganizationForTenant(Int32 tenantId)
        {
            Organization organization = CompiledGetOrganizationForTenant(_dbNavigation, tenantId);
            return !organization.IsNull() ? organization : null;
        }

        /// <summary>
        /// Retrieves all users based on product id.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <param name="assignToProductId">The product id.</param>
        /// <returns>
        /// The users for product.
        /// </returns>
        List<OrganizationUser> ISecurityRepository.GetUsersByProductId(Int32 assignToProductId)
        {
            TenantProduct tenantProduct = _dbNavigation.TenantProducts.FirstOrDefault(condition => condition.TenantProductID == assignToProductId);

            if (!tenantProduct.IsNull())
            {
                Int32 tenantId = tenantProduct.TenantID;
                List<Int32> organizationAndDepartmentIds = _dbNavigation.Organizations.Where(condition => condition.TenantID == tenantId).Select(ss => ss.OrganizationID).ToList();

                return _dbNavigation.OrganizationUsers
                    .Include(SysXEntityConstants.TABLE_ASPNET_USERS)
                    .Include(SysXEntityConstants.TABLE_ASPNET_USERS_DOT_ASPNET_MEMBERSHIP)
                    .Include(SysXEntityConstants.TABLE_ORGANIZATION)
                    .Where(condition => organizationAndDepartmentIds.Contains(condition.OrganizationID) && condition.IsDeleted == false).ToList();
            }

            return null;
        }

        Int32 ISecurityRepository.GetOrganizationIdByProductId(Int32 assignToProductId)
        {
            TenantProduct tenantProduct = _dbNavigation.TenantProducts.FirstOrDefault(condition => condition.TenantProductID == assignToProductId);

            if (tenantProduct != null)
            {
                Int32 tenantId = tenantProduct.TenantID;
                Organization organization = _dbNavigation.Organizations.FirstOrDefault(fod => fod.TenantID == tenantId && fod.IsDeleted == false && fod.IsActive);

                if (organization != null)
                {
                    return organization.OrganizationID;
                }
            }

            return 0;
        }

        /// <summary>
        /// Checks if the Organization exists in system database.
        /// </summary>
        /// <param name="enteredOrganizationName">The entered Organization Name.</param>
        /// <param name="existingOrganizationName">The existing Organization Name.</param>
        /// <returns>
        /// true if organization exists, false if not.
        /// </returns>
        Boolean ISecurityRepository.IsOrganizationExists(String enteredOrganizationName, String existingOrganizationName = null)
        {
            // executes when the form is in insert mode.
            if (String.IsNullOrEmpty(existingOrganizationName))
            {
                return CompiledGetAllOrganization(_dbNavigation).Where(organizationDetails => organizationDetails.OrganizationName.ToLower() == enteredOrganizationName.ToLower()).Count() > AppConsts.NONE;
            }
            // executes when the form is in update mode.
            else
            {
                return CompiledGetAllOrganization(_dbNavigation).ToList().FindAll(organizationDetails => organizationDetails.OrganizationName.ToLower() == enteredOrganizationName.ToLower())
                    .SkipWhile(organizationDetails => existingOrganizationName.ToLower() == enteredOrganizationName.ToLower())
                    .Any(nameChecks => nameChecks.OrganizationName.ToLower().Equals(enteredOrganizationName.ToLower()));
            }
        }

        /// <summary>
        /// Retrieves all organizations for each product.
        /// </summary>
        /// <param name="isAdmin">  .</param>
        /// <param name="productId">.</param>
        /// <returns>
        /// A <see cref="Organization"/> list of data from the underlying data storage.
        /// </returns>
        IQueryable<Organization> ISecurityRepository.GetOrganizationsForProduct(Boolean isAdmin, Int32 productId)
        {
            TenantProduct tenantProduct = CompiledGetTenantProductInformation(_dbNavigation, productId);

            if (tenantProduct.IsNull())
            {
                return null;
            }
            else
            {
                Int32 tenantProductTenantId = Convert.ToInt32(tenantProduct.TenantID);
                return CompiledGetOrganizationsForProduct(_dbNavigation, isAdmin, tenantProductTenantId);
            }
        }

        /// <summary>
        /// Retrieves all Organizations based on organizationID.
        /// </summary>
        /// <param name="organizationId">.</param>
        /// <returns>
        /// all type organization.
        /// </returns>
        Organization ISecurityRepository.GetAllTypeOrganization(Int32 organizationId)
        {
            Organization organization = CompiledGetAllTypeOrganization(_dbNavigation, organizationId);
            return !organization.IsNull() ? organization : null;
        }

        /// <summary>
        /// Check organization by current user identifier.
        /// </summary>
        /// <param name="currentUserId">The current user id.</param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        Boolean ISecurityRepository.CheckOrganizationByCurrentUserId(Int32 currentUserId)
        {
            return CompiledCheckOrganizationByCurrentUserId(_dbNavigation, currentUserId).Count() > AppConsts.NONE;
        }

        /// <summary>
        /// Check organization by current user identifier.
        /// </summary>
        /// <param name="isAdmin">Checks if the logged in user is admin.</param>
        /// <param name="productId">Logged in user's productId.</param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        Boolean ISecurityRepository.IsDepartmentOrOrganizationExistsForProduct(Boolean isAdmin, Int32? productId)
        {
            //Int32 tenantId = CompiledGetTenantIdByProductId(_dbNavigation, productId);
            return isAdmin ? CompiledIsOrganizationExists(_dbNavigation) : CompiledIsDepartmentExists(_dbNavigation, CompiledGetTenantIdByProductId(_dbNavigation, productId));
        }

        /// <summary>
        /// Retrieves the tenant's id based on product's id.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, Int32?, Int32> CompiledGetTenantIdByProductId = CompiledQuery.Compile
            <SysXAppDBEntities, Int32?, Int32>((dbNavigation, productId) => dbNavigation.TenantProducts.Where(condition => condition.TenantProductID == productId).FirstOrDefault().TenantID);

        /// <summary>
        /// Checks is there any Organization created so far.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, Boolean> CompiledIsOrganizationExists = CompiledQuery.Compile
            <SysXAppDBEntities, Boolean>((dbNavigation) => dbNavigation.Organizations.Where(
                        condition =>
                        condition.ParentOrganizationID == null && condition.IsActive && condition.IsDeleted == false).
                        Count() > AppConsts.NONE);

        /// <summary>
        /// Checks is there any department exist for current product.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, Int32, Boolean> CompiledIsDepartmentExists = CompiledQuery.Compile
            <SysXAppDBEntities, Int32, Boolean>((dbNavigation, tenantId) => dbNavigation.Organizations.Where(
                condition => condition.TenantID == tenantId && condition.IsActive && condition.IsDeleted == false && condition.ParentOrganizationID != null).
                Count() > AppConsts.NONE);

        /// <summary>
        /// Query if department is assign to any user.
        /// </summary>
        /// <param name="departmentId"> Id of the current department </param>
        /// <returns>
        /// true if department assign to any user, false if not.
        /// </returns>
        Boolean ISecurityRepository.IsUserExistsForDepartment(Int32 departmentId)
        {
            return CompiledIsUserExistsForDepartment(_dbNavigation, departmentId).ToList().Count() > AppConsts.NONE;
        }

        List<OrganizationUserNamePrefix> ISecurityRepository.getAllUserNamePrefix()
        {
            return CompiledUserNamePrefix(_dbNavigation).ToList();
        }

        /// <summary>
        /// Gets the organizations by tenant identifier.
        /// </summary>
        /// <param name="tenantId">The value for tenant's id.</param>
        /// <returns>
        /// The organizations by tenant identifier.
        /// </returns>
        IQueryable<OrganizationUserNamePrefix> ISecurityRepository.GetOrganizationUserNamePrefix(Int32 organizationId)
        {
            IQueryable<OrganizationUserNamePrefix> organizationPrefix = CompiledGetOrganizationUserNamePrefix(_dbNavigation, organizationId);
            return !organizationPrefix.IsNull() ? organizationPrefix : null;
        }

        #endregion

        #region Manage Programs

        ///// <summary>
        ///// Gets the organizations by tenant identifier.
        ///// </summary>
        ///// <param name="tenantId">The value for tenant's id.</param>
        ///// <returns>
        ///// The organizations by tenant identifier.
        ///// </returns>
        //IQueryable<AdminProgramStudy> ISecurityRepository.GetProgramsByOrganizationId(Int32 OrganizationId)
        //{
        //    IQueryable<AdminProgramStudy> programs = CompiledGetProgramsByOrganizationId(_dbNavigation, OrganizationId);
        //    return !programs.IsNull() ? programs : null;
        //}

        ///// <summary>
        ///// Gets List of programs for the given tenant ID.
        ///// </summary>
        ///// <param name="clientId">Tenant ID</param>
        ///// <returns>List of active programs</returns>
        //List<AdminProgramStudy> ISecurityRepository.GetAllProgramsForTenantID(Int32 clientId)
        //{
        //    Entity.Organization organization = GetOrganizationForTenantID(clientId);

        //    if (organization.IsNotNull())
        //        return _dbNavigation.AdminProgramStudies.Where(programDetails => programDetails.DeleteFlag == false
        //             && programDetails.OrganizationID == organization.OrganizationID).ToList();

        //    return new List<Entity.AdminProgramStudy>();
        //}

        /// <summary>
        /// Gets the organization for the given client ID.
        /// </summary>
        /// <param name="clientId">Tenant ID</param>
        /// <returns>Organization Entity Object</returns>
        public Entity.Organization GetOrganizationForTenantID(Int32 clientId)
        {
            return _dbNavigation.Organizations.FirstOrDefault(org => org.TenantID == clientId
                && org.ParentOrganizationID == null && org.IsActive == true && org.IsDeleted == false);
        }

        ///// <summary>
        ///// Enumerates get organizations by current user identifier in this collection.
        ///// </summary>
        ///// <param name="isAdmin">      The value for isAdmin</param>
        ///// <param name="productId">    The value for product's id.</param>
        ///// <param name="currentUserId">The value for current user's Id.</param>
        ///// <returns>
        ///// An enumerator that allows foreach to be used to process get organizations by current user
        ///// identifier in this collection.
        ///// </returns>
        //IQueryable<AdminProgramStudy> ISecurityRepository.GetProgramsByCurrentUserId(Boolean isAdmin, Int32 productId, Int32 currentUserId)
        //{
        //    if (isAdmin)
        //    {
        //        var ProgramStudy = _dbNavigation.AdminProgramStudies.Where(programStudyDetails => programStudyDetails.DeleteFlag == null || programStudyDetails.DeleteFlag == false);

        //        return ProgramStudy;
        //    }
        //    TenantProduct product = _dbNavigation.TenantProducts.Where(tenantProduct => tenantProduct.TenantProductID == productId).FirstOrDefault();

        //    if (!product.IsNull())
        //    {
        //        Int32 tenantId = product.TenantID;

        //        var ProgramStudy = _dbNavigation.AdminProgramStudies.Include(SysXEntityConstants.TABLE_ORGANIZATION).Include(SysXEntityConstants.TABLE_ORGANIZATION_USER).Where(programStudyDetails => programStudyDetails.DeleteFlag == null || programStudyDetails.DeleteFlag == false);

        //        return ProgramStudy;
        //    }
        //    return null;
        //}


        ///// <summary>
        ///// Gets the Programs by DepartmentId.
        ///// </summary>
        ///// The Programs by tenant identifier.
        ///// </returns>
        //public IQueryable<DeptProgramMapping> GetOrganizationProgramList(Int32 OrganizationId)
        //{
        //    return _dbNavigation.DeptProgramMappings.Where(cond => cond.DPM_OrganizationID == OrganizationId && !cond.DPM_IsDeleted);
        //}

        ///// <summary>
        ///// Gets the Program by organizationId.
        ///// </summary>
        ///// The Programs by tenant identifier.
        ///// </returns>
        //public DeptProgramMapping GetOrganizationProgram(Int32 depProgramId)
        //{
        //    return _dbNavigation.DeptProgramMappings.Where(cond => cond.DPM_ID == depProgramId && !cond.DPM_IsDeleted).FirstOrDefault();
        //}

        /// <summary>
        /// Retrieves a list of all active organizations.
        /// </summary>
        /// <param name="isAdmin">The value for IsAdmin.</param>
        /// <param name="productId">The product's Id.</param>
        /// <returns>
        /// A <see cref="Organization"/> list of data from the underlying data storage.
        /// </returns>
        //IQueryable<AdminProgramStudy> ISecurityRepository.GetPrograms(Boolean isAdmin, Int32 productId)
        //{
        //    if (isAdmin)
        //    {
        //        return _dbNavigation.AdminProgramStudies.Where(programStudyDetails => programStudyDetails.DeleteFlag == null || programStudyDetails.DeleteFlag == false);
        //    }

        //    TenantProduct product = _dbNavigation.TenantProducts.Where(tenantProduct => tenantProduct.TenantProductID == productId).FirstOrDefault();
        //    return _dbNavigation.Organizations.Include(SysXEntityConstants.TABLE_TENANT).Include(SysXEntityConstants.TABLE_TENANT_LKPTENANTTYPE)
        //        .Where(organizationsDetails => organizationsDetails.IsActive && organizationsDetails.IsDeleted == false 
        //                && organizationsDetails.ParentOrganizationID != null && organizationsDetails.Tenant.TenantID == product.TenantID);
        //}

        ///// <summary>
        ///// Retrieves a Program details based on ProgramID.
        ///// </summary>
        ///// <param name="organizationId">.</param>
        ///// <returns>
        ///// The organization.
        ///// </returns>
        //AdminProgramStudy ISecurityRepository.GetProgram(Int32 programId)
        //{
        //    AdminProgramStudy program = CompiledGetProgram(_dbNavigation, programId);
        //    return !program.IsNull() ? program : null;
        //}

        ///// <summary>
        ///// Retrieves an organization details based on tenantID.
        ///// </summary>
        ///// <param name="tenantId">.</param>
        ///// <returns>
        ///// The organization for tenant.
        ///// </returns>
        //Organization ISecurityRepository.GetOrganizationForTenant(Int32 tenantId)
        //{
        //    Organization organization = CompiledGetOrganizationForTenant(_dbNavigation, tenantId);
        //    return !organization.IsNull() ? organization : null;
        //}

        ///// <summary>
        ///// Retrieves all users based on product id.
        ///// </summary>
        ///// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        ///// <param name="assignToProductId">The product id.</param>
        ///// <returns>
        ///// The users for product.
        ///// </returns>
        //List<OrganizationUser> ISecurityRepository.GetUsersByProductId(Int32 assignToProductId)
        //{
        //    TenantProduct tenantProduct = _dbNavigation.TenantProducts.FirstOrDefault(condition => condition.TenantProductID == assignToProductId);

        //    if (!tenantProduct.IsNull())
        //    {
        //        Int32 tenantId = tenantProduct.TenantID;
        //        List<Int32> organizationAndDepartmentIds = _dbNavigation.Organizations.Where(condition => condition.TenantID == tenantId).Select(ss => ss.OrganizationID).ToList();

        //        return _dbNavigation.OrganizationUsers
        //            .Include(SysXEntityConstants.TABLE_ASPNET_USERS)
        //            .Include(SysXEntityConstants.TABLE_ASPNET_USERS_DOT_ASPNET_MEMBERSHIP)
        //            .Include(SysXEntityConstants.TABLE_ORGANIZATION)
        //            .Where(condition => organizationAndDepartmentIds.Contains(condition.OrganizationID) && condition.IsDeleted == false).ToList();
        //    }

        //    return null;
        //}

        //Int32 ISecurityRepository.GetOrganizationIdByProductId(Int32 assignToProductId)
        //{
        //    TenantProduct tenantProduct = _dbNavigation.TenantProducts.FirstOrDefault(condition => condition.TenantProductID == assignToProductId);

        //    if (tenantProduct != null)
        //    {
        //        Int32 tenantId = tenantProduct.TenantID;
        //        Organization organization = _dbNavigation.Organizations.FirstOrDefault(fod => fod.TenantID == tenantId && fod.IsDeleted == false && fod.IsActive);

        //        if (organization != null)
        //        {
        //            return organization.OrganizationID;
        //        }
        //    }

        //    return 0;
        //}

        /// <summary>
        /// Checks if the Organization exists in system database.
        /// </summary>
        /// <param name="enteredOrganizationName">The entered Organization Name.</param>
        /// <param name="existingOrganizationName">The existing Organization Name.</param>
        /// <returns>
        /// true if organization exists, false if not.
        /// </returns>
        Boolean ISecurityRepository.IsProgramExists(String enteredProgramName, String existingProgramName = null)
        {
            // executes when the form is in insert mode.
            if (String.IsNullOrEmpty(existingProgramName))
            {
                //return CompiledGetAllProgram(_dbNavigation).Where(ProgramDetails => ProgramDetails.ProgramStudy.ToLower() == enteredProgramName.ToLower()).Count() > AppConsts.NONE;
                return false;
            }
            // executes when the form is in update mode.
            else
            {
                //return CompiledGetAllProgram(_dbNavigation).ToList().FindAll(ProgramDetails => ProgramDetails.ProgramStudy.ToLower() == enteredProgramName.ToLower())
                //    .SkipWhile(ProgramDetails => existingProgramName.ToLower() == enteredProgramName.ToLower())
                //    .Any(nameChecks => nameChecks.ProgramStudy.ToLower().Equals(enteredProgramName.ToLower()));
                return false;
            }
        }

        public Boolean UpdateProgramObject()
        {
            if (_dbNavigation.SaveChanges() > 0)
                return true;
            return false;
        }

        ///// <summary>
        ///// Retrieves all organizations for each product.
        ///// </summary>
        ///// <param name="isAdmin">  .</param>
        ///// <param name="productId">.</param>
        ///// <returns>
        ///// A <see cref="Organization"/> list of data from the underlying data storage.
        ///// </returns>
        //IQueryable<Organization> ISecurityRepository.GetOrganizationsForProduct(Boolean isAdmin, Int32 productId)
        //{
        //    TenantProduct tenantProduct = CompiledGetTenantProductInformation(_dbNavigation, productId);

        //    if (tenantProduct.IsNull())
        //    {
        //        return null;
        //    }
        //    else
        //    {
        //        Int32 tenantProductTenantId = Convert.ToInt32(tenantProduct.TenantID);
        //        return CompiledGetOrganizationsForProduct(_dbNavigation, isAdmin, tenantProductTenantId);
        //    }
        //}

        ///// <summary>
        ///// Retrieves all Organizations based on organizationID.
        ///// </summary>
        ///// <param name="organizationId">.</param>
        ///// <returns>
        ///// all type organization.
        ///// </returns>
        //Organization ISecurityRepository.GetAllTypeOrganization(Int32 organizationId)
        //{
        //    Organization organization = CompiledGetAllTypeOrganization(_dbNavigation, organizationId);
        //    return !organization.IsNull() ? organization : null;
        //}

        ///// <summary>
        ///// Check organization by current user identifier.
        ///// </summary>
        ///// <param name="currentUserId">The current user id.</param>
        ///// <returns>
        ///// true if it succeeds, false if it fails.
        ///// </returns>
        //Boolean ISecurityRepository.CheckOrganizationByCurrentUserId(Int32 currentUserId)
        //{
        //    return CompiledCheckOrganizationByCurrentUserId(_dbNavigation, currentUserId).Count() > AppConsts.NONE;
        //}

        ///// <summary>
        ///// Check organization by current user identifier.
        ///// </summary>
        ///// <param name="isAdmin">Checks if the logged in user is admin.</param>
        ///// <param name="productId">Logged in user's productId.</param>
        ///// <returns>
        ///// true if it succeeds, false if it fails.
        ///// </returns>
        //Boolean ISecurityRepository.IsDepartmentOrOrganizationExistsForProduct(Boolean isAdmin, Int32? productId)
        //{
        //    //Int32 tenantId = CompiledGetTenantIdByProductId(_dbNavigation, productId);
        //    return isAdmin ? CompiledIsOrganizationExists(_dbNavigation) : CompiledIsDepartmentExists(_dbNavigation, CompiledGetTenantIdByProductId(_dbNavigation, productId));
        //}

        ///// <summary>
        ///// Retrieves the tenant's id based on product's id.
        ///// </summary>
        //public static readonly Func<SysXAppDBEntities, Int32?, Int32> CompiledGetTenantIdByProductId = CompiledQuery.Compile
        //    <SysXAppDBEntities, Int32?, Int32>((dbNavigation, productId) => dbNavigation.TenantProducts.Where(condition => condition.TenantProductID == productId).FirstOrDefault().TenantID);

        ///// <summary>
        ///// Checks is there any Organization created so far.
        ///// </summary>
        //public static readonly Func<SysXAppDBEntities, Boolean> CompiledIsOrganizationExists = CompiledQuery.Compile
        //    <SysXAppDBEntities, Boolean>((dbNavigation) => dbNavigation.Organizations.Where(
        //                condition =>
        //                condition.ParentOrganizationID == null && condition.IsActive && condition.IsDeleted == false).
        //                Count() > AppConsts.NONE);

        ///// <summary>
        ///// Checks is there any department exist for current product.
        ///// </summary>
        //public static readonly Func<SysXAppDBEntities, Int32, Boolean> CompiledIsDepartmentExists = CompiledQuery.Compile
        //    <SysXAppDBEntities, Int32, Boolean>((dbNavigation, tenantId) => dbNavigation.Organizations.Where(
        //        condition => condition.TenantID == tenantId && condition.IsActive && condition.IsDeleted == false && condition.ParentOrganizationID != null).
        //        Count() > AppConsts.NONE);

        ///// <summary>
        ///// Query if department is assign to any user.
        ///// </summary>
        ///// <param name="departmentId"> Id of the current department </param>
        ///// <returns>
        ///// true if department assign to any user, false if not.
        ///// </returns>
        //Boolean ISecurityRepository.IsUserExistsForDepartment(Int32 departmentId)
        //{
        //    return CompiledIsUserExistsForDepartment(_dbNavigation, departmentId).ToList().Count() > AppConsts.NONE;
        //}

        //List<OrganizationUserNamePrefix> ISecurityRepository.getAllUserNamePrefix()
        //{
        //    return CompiledUserNamePrefix(_dbNavigation).ToList();
        //}

        ///// <summary>
        ///// Gets the organizations by tenant identifier.
        ///// </summary>
        ///// <param name="tenantId">The value for tenant's id.</param>
        ///// <returns>
        ///// The organizations by tenant identifier.
        ///// </returns>
        //IQueryable<OrganizationUserNamePrefix> ISecurityRepository.GetOrganizationUserNamePrefix(Int32 organizationId)
        //{
        //    IQueryable<OrganizationUserNamePrefix> organizationPrefix = CompiledGetOrganizationUserNamePrefix(_dbNavigation, organizationId);
        //    return !organizationPrefix.IsNull() ? organizationPrefix : null;
        //}

        #endregion


        #region Manage Grades
        /// <summary>
        /// Get all GradeLevelGroups
        /// </summary>
        /// <returns>
        /// Returns all the GradeLevelGroups
        /// </returns>
        List<lkpGradeLevelGroup> ISecurityRepository.GetAllGradeLevelGroups()
        {
            return (_dbNavigation.lkpGradeLevelGroups).ToList<lkpGradeLevelGroup>();
        }

        /// <summary>
        /// Gets the Grades by tenant identifier.
        /// </summary>
        /// <param name="tenantId">The value for tenant's id.</param>
        /// <returns>
        /// The Grades by tenant identifier.
        /// </returns>
        IQueryable<lkpGradeLevel> ISecurityRepository.GetGradesByOrganizationId(Int32 GradeId)
        {
            IQueryable<lkpGradeLevel> grades = CompiledGetGradesByOrganizationId(_dbNavigation, GradeId);
            return !grades.IsNull() ? grades : null;
        }

        IQueryable<lkpGradeLevel> ISecurityRepository.GetAllGrades()
        {

            var grade = _dbNavigation.lkpGradeLevels.Where(gradeDetails => gradeDetails.DeleteFlag == false);

            return grade;
        }


        IQueryable<lkpGradeLevel> ISecurityRepository.GetGradesByCurrentUserId(Boolean isAdmin, Int32 productId, Int32 currentUserId)
        {
            if (isAdmin)
            {
                var grade = _dbNavigation.lkpGradeLevels.Where(gradeDetails => gradeDetails.DeleteFlag == null || gradeDetails.DeleteFlag == false);

                return grade;
            }
            TenantProduct product = _dbNavigation.TenantProducts.Where(tenantProduct => tenantProduct.TenantProductID == productId).FirstOrDefault();

            if (!product.IsNull())
            {
                Int32 tenantId = product.TenantID;

                var grade = _dbNavigation.lkpGradeLevels.Include(SysXEntityConstants.TABLE_ORGANIZATION).Include(SysXEntityConstants.TABLE_ORGANIZATION_USER).Where(gradeDetails => gradeDetails.DeleteFlag == null || gradeDetails.DeleteFlag == false);

                return grade;
            }
            return null;
        }


        lkpGradeLevel ISecurityRepository.GetGrade(Int32 GradeId)
        {
            lkpGradeLevel grade = CompiledGetGrade(_dbNavigation, GradeId);
            return !grade.IsNull() ? grade : null;
        }


        Boolean ISecurityRepository.IsGradeExists(String enteredGradeName, Int32 organizationId, String existingGradeName = null)
        {
            // executes when the form is in insert mode.
            if (String.IsNullOrEmpty(existingGradeName))
            {
                return CompiledGetAllGrade(_dbNavigation).Where(GradeDetails => GradeDetails.Description.ToLower() == enteredGradeName.ToLower() && GradeDetails.OrganizationID == organizationId).Count() > AppConsts.NONE;
            }
            // executes when the form is in update mode.
            else
            {
                return CompiledGetAllGrade(_dbNavigation).ToList().FindAll(GradeDetails => GradeDetails.Description.ToLower() == enteredGradeName.ToLower() && GradeDetails.OrganizationID == organizationId)
                    .SkipWhile(ProgramDetails => existingGradeName.ToLower() == enteredGradeName.ToLower())
                    .Any(nameChecks => nameChecks.Description.ToLower().Equals(enteredGradeName.ToLower()));
            }
        }

        public IQueryable<lkpGradeLevel> GetGradeListByOrganizationId(Int32 organizationId)
        {
            return _dbNavigation.lkpGradeLevels.Where(gradeDetails => gradeDetails.DeleteFlag == false && gradeDetails.OrganizationID == organizationId);
        }


        #endregion

        #region Manage Mapping Methods

        /// <summary>
        /// Mapping between block and features.
        /// </summary>
        /// <param name="blockFeature">              The value for block's Feature.</param>
        /// <param name="blockFeatureIdsToBeAdded">  The value for blockFeatureIds To Be Added.</param>
        /// <param name="blockFeatureIdsToBeRemoved">The value for blockFeatureIdsTo Be Removed.</param>
        /// <param name="createdById"></param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        Boolean ISecurityRepository.LineOfBusinessFeatureMapping(lkpSysXBlock blockFeature, List<Int32> blockFeatureIdsToBeAdded, List<Int32> blockFeatureIdsToBeRemoved, Int32 createdById)
        {

            // This call will remove the features associated with the existing block.
            DeleteSysXBlocksFeatures(blockFeatureIdsToBeRemoved);

            if (blockFeatureIdsToBeAdded.Count <= AppConsts.NONE)
            {
                _dbNavigation.SaveChanges();
            }
            foreach (Int32 featureId in blockFeatureIdsToBeAdded)
            {
                AddLineOfBusinessFeatures(blockFeature, CurrentRepositoryContext.GetProductFeature(featureId), createdById);
                _dbNavigation.SaveChanges();
            }

            return true;
        }

        /// <summary>
        /// Check if the feature is assigned to any role based on featureID.
        /// </summary>
        /// <param name="featureId">The Feature's Id.</param>
        /// <param name="productId">The product's id.</param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        Boolean ISecurityRepository.CheckFeatureAssignToRole(Int32 featureId, Int32 productId)
        {
            TenantProduct tenantProduct = CurrentRepositoryContext.GetProduct(productId);
            List<RoleDetail> roleDetails = tenantProduct.RoleDetails.Where(condition => condition.IsActive && condition.IsDeleted == false).ToList();
            return roleDetails.Select(item => _dbNavigation.RolePermissionProductFeatures.Where(rolePermissionProductFeatures => !rolePermissionProductFeatures.SysXBlocksFeature.ProductFeature.IsDeleted && rolePermissionProductFeatures.SysXBlockFeatureId == featureId && rolePermissionProductFeatures.RoleId == item.RoleDetailID).ToList()).Any(features => features.Count() > AppConsts.NONE);
        }

        /// <summary>
        /// Determines whether the current block exists in Product Feature.
        /// </summary>
        /// <param name="sysXBlockFeatureId">The sysXBlockFeatureId.</param>
        /// <returns>
        /// true if system x coordinate line of business exist in product feature, false if not.
        /// </returns>
        Boolean ISecurityRepository.IsSysXLineOfBusinessExistInProductFeature(Int32 sysXBlockFeatureId)
        {
            var tenantProductFeatures = _dbNavigation.TenantProductFeatures.Where(tenantProduct => tenantProduct.SysXBlockFeatureID == sysXBlockFeatureId);

            if (!tenantProductFeatures.IsNull())
            {
                IQueryable<TenantProductFeature> tenantFeatures = tenantProductFeatures;
                return tenantFeatures.Count() > AppConsts.NONE;
            }

            return false;
        }

        /// <summary>
        /// This method returns all the odd entries from RolePermissionProductFeature table , which is no more associated with the product's feature.
        /// </summary>
        /// <param name="featureList">List of all the new relations made through Map Product Feature page.</param>
        /// <returns></returns>
        List<RolePermissionProductFeature> ISecurityRepository.GetFeatureWithPermissionUsedByRole(IEnumerable<BlockFeaturePermissionMapper> featureList)
        {
            List<RolePermissionProductFeature> rolePermissionProductFeatures = new List<RolePermissionProductFeature>();

            foreach (BlockFeaturePermissionMapper item in featureList)
            {
                List<Int32> permissionList = item.PermissionId;

                IQueryable<RolePermissionProductFeature> result = _dbNavigation.RolePermissionProductFeatures.Where(action => action.SysXBlockFeatureId == item.SysXBlockBlockId && !permissionList.Contains(action.PermissionId));

                rolePermissionProductFeatures.AddRange(result);
            }

            return rolePermissionProductFeatures;
        }

        /// <summary>
        /// Mapping between product and features.
        /// </summary>
        /// <param name="product">            The value for product.</param>
        /// <param name="featureList">        The value for featureList.</param>
        /// <param name="updatedSysXBlockIds">The value for updatedSysXBlockIds.</param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        Boolean ISecurityRepository.ProductFeatureMapping(TenantProduct product, List<BlockFeaturePermissionMapper> featureList, List<Int32> updatedSysXBlockIds, Int32 currentLoggedInUserID)
        {
            // To delete from role mapping.
            DeleteFromRolePermissionProductFeature(CurrentRepositoryContext.GetFeatureWithPermissionUsedByRole(featureList));
            List<Tuple<Int32, Int32, Int32, Boolean>> lstDeletedBookmarkedFeatures = DeleteTenantProductFeature(product.TenantProductID, updatedSysXBlockIds);

            foreach (BlockFeaturePermissionMapper item in featureList)
            {
                SysXBlocksFeature sysXBlockFeature = CurrentRepositoryContext.GetSysXBlockFeature(item.SysXBlockBlockId);
                TenantProductFeature tenantfeature = new TenantProductFeature { TenantProduct = product, SysXBlocksFeature = sysXBlockFeature };
                tenantfeature = base.AddObjectEntityInTransaction(tenantfeature);

                foreach (Int32 permissionId in item.PermissionId)
                {
                    FeaturePermission featurePermission = new FeaturePermission
                    {
                        Permission =
                            CurrentRepositoryContext.GetPermission(
                                permissionId),
                        TenantProductFeature = tenantfeature
                    };

                    base.AddObjectEntityInTransaction(featurePermission);
                }
            }

            _dbNavigation.SaveChanges();

            if (!lstDeletedBookmarkedFeatures.IsNullOrEmpty()
                && lstDeletedBookmarkedFeatures.Count > 0)
            {
                /// List<Tuple<Int32, Int32, Int32, Boolean>>
                /// Tuple<SysXBlockFeatureID, TenantProductID, OrgUserID, IsBookMarked
                foreach (Tuple<Int32, Int32, Int32, Boolean> item in lstDeletedBookmarkedFeatures.Distinct())
                {
                    var tenantProductFeature = _dbNavigation.TenantProductFeatures.Where(cond => cond.SysXBlockFeatureID == item.Item1
                                                                                        && cond.TenantProductID == item.Item2).FirstOrDefault();

                    if (!tenantProductFeature.IsNullOrEmpty() && tenantProductFeature.TenantProductSysXBlockID > 0)
                    {
                        FeaturesBookmark featuresBookmark = new FeaturesBookmark();
                        featuresBookmark.FB_IsBookmarked = item.Item4;
                        featuresBookmark.FB_OrganizationUserID = item.Item3;
                        featuresBookmark.FB_CreatedBy = currentLoggedInUserID;
                        featuresBookmark.FB_CreatedOn = DateTime.Now;
                        featuresBookmark.FB_TenantProductSysXBlockID = tenantProductFeature.TenantProductSysXBlockID;
                        featuresBookmark.FB_IsDeleted = false;
                        _dbNavigation.FeaturesBookmarks.AddObject(featuresBookmark);
                    }
                }

                _dbNavigation.SaveChanges();
            }

            return true;
        }

        /// <summary>
        /// Retrieve features for a block based on blockID.
        /// </summary>
        /// <param name="blockId">The value for block's Id.</param>
        /// <returns>
        /// The features for line of business.
        /// </returns>
        IQueryable<SysXBlocksFeature> ISecurityRepository.GetFeaturesForLineOfBusiness(Int32 blockId)
        {
            return _dbNavigation.SysXBlocksFeatures.Include(SysXEntityConstants.TABLE_PRODUCT_FEATURE).Where(sysXBlocksFeatures => sysXBlocksFeatures.SysXBlockID == blockId
                                                                                                                && !sysXBlocksFeatures.ProductFeature.IsDeleted);
        }

        #region Recursive Mapping Methods

        /// <summary>
        /// Check system x coordinate block feature.
        /// </summary>
        /// <param name="blockId">  The  block id.</param>
        /// <param name="featureId">The Feature Id.</param>
        SysXBlocksFeature ISecurityRepository.CheckSysXBlockFeature(Int32 blockId, Int32 featureId)
        {
            return _dbNavigation.SysXBlocksFeatures.FirstOrDefault(sysXBlocksFeatures => sysXBlocksFeatures.SysXBlockID == blockId && sysXBlocksFeatures.FeatureID == featureId);
        }

        #endregion

        #endregion

        #region Manage LineOfBusinesses

        /// <summary>
        /// Retrieves a list of all active blocks.
        /// </summary>
        /// <param name="userId">The User's Id.</param>
        /// <returns>
        /// A <see cref="vw_UserAssignedBlocks"/> list of data from the underlying data storage.
        /// </returns>
        IQueryable<vw_UserAssignedBlocks> ISecurityRepository.GetLineOfBusinessesByUser(String userId)
        {
            Guid testGuid = Guid.Empty;
            if (!Guid.TryParse(userId, out testGuid))
                return null;// if userid is not valid, then system does not need to evaluate business channels. system will simply return in that case.
            return CompiledGetLineOfBusinessesByUser.Invoke(_dbNavigation, userId);
        }

        /// <summary>
        /// Retrieves a list of all active blocks.
        /// </summary>
        /// <returns>
        /// A <see cref="SysXBlock"/> list of data from the underlying data storage.
        /// </returns>
        IQueryable<lkpSysXBlock> ISecurityRepository.GetLineOfBusinesses()
        {
            return CompiledGetLineOfBusinesses.Invoke(_dbNavigation);
        }

        /// <summary>
        /// Retrieve User's block id based on userId.
        /// </summary>
        /// <param name="userId">The User's Id.</param>
        /// <returns>
        /// The user line of businesses identifiers.
        /// </returns>
        IQueryable<Int32> ISecurityRepository.GetUserLineOfBusinessesIds(String userId)
        {
            List<aspnet_Roles> roles = CurrentRepositoryContext.GetUserRolesById(userId);

            if (roles.IsNull() == false && roles.Count() > AppConsts.NONE)
            {
                Guid[] userRoleIds = (from role in roles
                                      select role.RoleId).ToArray();

                var sysXBlockIds = (from bf in _dbNavigation.SysXBlocksFeatures
                                    join rpp in _dbNavigation.RolePermissionProductFeatures
                                        on bf equals rpp.SysXBlocksFeature
                                    where userRoleIds.Contains(rpp.RoleId)
                                    select bf.SysXBlockID).Distinct();
                return sysXBlockIds;
            }
            return (new List<Int32>()).AsQueryable();
        }

        /// <summary>
        /// Retrieves all blocks based on blockID.
        /// </summary>
        /// <param name="blockId">The block's Id.</param>
        /// <returns>
        /// The line of business.
        /// </returns>
        lkpSysXBlock ISecurityRepository.GetLineOfBusiness(Int32 blockId)
        {
            var sysXBlock = CompiledGetLineOfBusiness.Invoke(_dbNavigation, blockId);
            return !sysXBlock.IsNull() ? sysXBlock : null;
        }

        /// <summary>
        /// Retrieves a list of block based on RoleId.
        /// </summary>
        /// <param name="roleId">The role's Id.</param>
        /// <returns>
        /// A <see cref="SysXBlock"/> list of data from the underlying data storage.
        /// </returns>
        IQueryable<lkpSysXBlock> ISecurityRepository.GetUserLineOfBusinessesByRoleId(String roleId) // left as list because there are operations performed on the list of data obtained.
        {
            Guid roleDetailIdGuid = new Guid(roleId);

            var query = (_dbNavigation.RoleDetails.Join(
                _dbNavigation.TenantProductFeatures,
                rd => rd.ProductID,
                tpf => (Int32?)(tpf.TenantProductID),
                (rd, tpf) => new { rd, tpf })
                .Join(
                    _dbNavigation.SysXBlocksFeatures,
                    roledetails => roledetails.tpf.SysXBlockFeatureID,
                    sfc => sfc.SysXBlockFeatureID,
                    (roledetails, sfc) => new { roledetails, sfc })

                .Join(
                    _dbNavigation.lkpSysXBlocks,
                    tenantProductFeatures => tenantProductFeatures.sfc.SysXBlockID,
                    sb => sb.SysXBlockId,
                    (tenantProductFeatures, sb) => new { tenantProductFeatures, sb })
                .Where(sysXBlocksFeatures => (sysXBlocksFeatures.tenantProductFeatures.roledetails.rd.RoleDetailID == roleDetailIdGuid))
                .Select(sysXBlocksFeatures => sysXBlocksFeatures.sb)).Distinct();
            return !query.IsNull() ? query : null;
        }

        /// <summary>
        /// Checks if the LineOfBusiness exists in system database.
        /// </summary>
        /// <param name="enteredLineOfBusinessName"> The value for entered LineOfBusiness Name. </param>
        /// <param name="existingLineOfBusinessName">The value for existing LineOfBusiness Name. </param>
        /// <returns>
        /// true if line of business exists, false if not.
        /// </returns>
        Boolean ISecurityRepository.IsLineOfBusinessExists(String enteredLineOfBusinessName, String existingLineOfBusinessName = null)
        {
            // executes when the form is in insert mode.
            if (String.IsNullOrEmpty(existingLineOfBusinessName))
            {
                return CompiledGetLineOfBusinesses(_dbNavigation).Where(condition => condition.Name.ToLower() == enteredLineOfBusinessName.ToLower()).Count() > AppConsts.NONE;
            }
            // executes when the form is in update mode.
            else
            {
                return CompiledGetLineOfBusinesses(_dbNavigation).ToList().FindAll(blockDetails => blockDetails.Name.ToLower() == enteredLineOfBusinessName.ToLower())
                    .SkipWhile(blockDetails => existingLineOfBusinessName.ToLower() == enteredLineOfBusinessName.ToLower())
                    .Any(nameChecks => nameChecks.Name.ToLower().Equals(enteredLineOfBusinessName.ToLower()));
            }
        }

        /// <summary>
        /// Performs a delete operation for line of business.
        /// </summary>
        /// <param name="block">The line of business.</param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        Boolean ISecurityRepository.DeleteLineOfBusiness(lkpSysXBlock block)
        {
            // Below code will make InActive column false for all the entry for the current block based on block's id.
            // DeleteAllFeaturesAssociatedWithLOB(block.SysXBlockId);
            return base.DeleteObjectEntity(block);
        }

        /// <summary>
        /// Line of business code exists or not
        /// </summary>
        /// <param name="newCode">New LOB Code</param>
        /// <param name="oldCode">Existing LOB Code</param>
        /// <returns>True, if exists else false</returns>
        Boolean ISecurityRepository.IsLOBCodeExist(String newCode, String oldCode)
        {
            var sql = _dbNavigation.lkpSysXBlocks.AsQueryable();
            if (!String.IsNullOrEmpty(oldCode))
            {
                sql = sql.Where(condition => condition.Code != oldCode);
            }

            return sql.Any(fx => fx.Code == newCode);
        }

        #endregion

        #region Manage Users and Roles

        /// <summary>
        /// Checks if the user exists in system database.
        /// </summary>
        /// <param name="enteredUserName"> The value for entered User Name.</param>
        /// <param name="existingUserName">The value for existing User Name.</param>
        /// <returns>
        /// true if user exists, false if not.
        /// </returns>
        Boolean ISecurityRepository.IsUserExists(String enteredUserName, String existingUserName = null)
        {
            // executes when the form is in insert mode.
            if (String.IsNullOrEmpty(existingUserName))
            {
                return CompiledGetUsers(_dbNavigation).Where(userDetail => userDetail.UserName.ToLower() == enteredUserName.ToLower()).Count() > AppConsts.NONE;
            }
            // executes when the form is in update mode.
            else
            {
                return CompiledGetUsers(_dbNavigation).ToList().FindAll(condition => condition.UserName.ToLower() == enteredUserName.ToLower())
                 .SkipWhile(userDetails => existingUserName.ToLower() == enteredUserName.ToLower())
                 .Any(nameChecks => existingUserName.ToLower().Equals(enteredUserName.ToLower()));
            }
        }

        /// <summary>
        /// Retrieves aspnet users based on userId.
        /// </summary>
        /// <exception cref="Exception">Thrown when an exception error condition occurs.</exception>
        /// <param name="userId">              The value for User's Id.</param>
        /// <param name="loadMembership">      The value for loadMembership.</param>
        /// <param name="loadOrganizationUser">The value for loadOrganizationUser.</param>
        /// <returns>
        /// The user by identifier.
        /// </returns>
        aspnet_Users ISecurityRepository.GetUserById(String userId, Boolean loadMembership, Boolean loadOrganizationUser)
        {
            aspnet_Users user = CompiledGetUserById(_dbNavigation, userId, loadMembership, loadOrganizationUser);

            if (!user.IsNull())
            {
                if (loadMembership)
                {
                    user.aspnet_MembershipReference.Load();
                }

                if (loadOrganizationUser)
                {
                    user.OrganizationUsers.Load();
                }

                return user;
            }

            throw new Exception(SysXUtils.GetMessage(ResourceConst.SECURITY_USER_ID) + SysXUtils.GetMessage(ResourceConst.SPACE) + userId + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.SECURITY_NOT_FOUND));
        }

        /// <summary>
        /// Retrieves all the data from the table OrganizationUser for the given Organization User Ids.
        /// </summary>
        /// <param name="lstUserIds">List of Organization User Ids</param>
        /// <returns>List of active organization users</returns>
        public List<OrganizationUser> GetOrganizationUsersByIds(List<Int32?> lstUserIds)
        {
            return _dbNavigation.OrganizationUsers.Where(obj => obj.IsDeleted == false && lstUserIds.Contains(obj.OrganizationUserID) && obj.IsActive == true).ToList();
        }

        /// <summary>
        /// Retrieves a aspnet user based on user name.
        /// </summary>
        /// <param name="userName">            The value for user name.</param>
        /// <param name="loadMembership">      The value for loadMembership.</param>
        /// <param name="loadOrganizationUser">The value for loadOrganizationUser.</param>
        /// <returns>
        /// The user by name.
        /// </returns>
        aspnet_Users ISecurityRepository.GetUserByName(String userName, Boolean loadMembership, Boolean loadOrganizationUser)
        {
            //_dbNavigation.Refresh(RefreshMode.StoreWins, _dbNavigation.OrganizationUsers);
            //_dbNavigation.Refresh(RefreshMode.StoreWins, _dbNavigation.aspnet_Users);
            //_dbNavigation.Refresh(RefreshMode.StoreWins, _dbNavigation.aspnet_Membership);
            return CompiledGetUserByName.Invoke(_dbNavigation, userName, loadMembership, loadOrganizationUser);
        }

        /// <summary>
        /// Retrieves user name by email.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        String ISecurityRepository.GetUserNameByEmail(String email)
        {
            //_dbNavigation.Refresh(RefreshMode.StoreWins, _dbNavigation.aspnet_Users);
            return CompiledGetUserNameByEmail.Invoke(_dbNavigation, email).IsNull() ? String.Empty : CompiledGetUserNameByEmail.Invoke(_dbNavigation, email).UserName;
        }

        /// <summary>
        /// UAT-2199:As an adb admin, I should be able to edit admin usernames on the manage users screen.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        String ISecurityRepository.GetUserIdByEmail(String email)
        {
            Guid userID = _dbNavigation.aspnet_Membership.Where(cond => cond.Email == email).Select(sel => sel.UserId).FirstOrDefault();
            return (userID == Guid.Empty ? "" : Convert.ToString(userID));
        }

        /// <summary>
        /// UAT-2199: As an adb admin, I should be able to edit admin usernames on the manage users screen.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        String ISecurityRepository.GetUserIdByUserName(String userName)
        {
            Guid userID = _dbNavigation.aspnet_Users.Where(cond => cond.UserName == userName).Select(sel => sel.UserId).FirstOrDefault();
            return (userID == Guid.Empty ? "" : Convert.ToString(userID));
        }

        /// <summary>
        /// Retrieves a list of all of aspnet user.
        /// </summary>
        /// <param name="loadRoleDetails">The value for load Role Details.</param>
        /// <returns>
        /// A <see cref="aspnet_Roles"/> list of data from the underlying data storage.
        /// </returns>
        IQueryable<aspnet_Roles> ISecurityRepository.GetRoles(Boolean loadRoleDetails)
        {
            IQueryable<aspnet_Roles> roles = CompiledGetRoles(_dbNavigation, loadRoleDetails);

            if (loadRoleDetails)
            {
                foreach (aspnet_Roles role in roles)
                {
                    role.RoleDetailReference.Load();
                }
            }
            return roles;
        }

        /// <summary>
        /// Determines whether the current role exists.
        /// </summary>
        /// <param name="roleName">The value for role name.</param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        Boolean ISecurityRepository.RoleExists(String roleName)
        {
            Int32 roleCount = _dbNavigation.aspnet_Roles.Count(aspnetRoles => aspnetRoles.RoleName == roleName);
            return (roleCount > AppConsts.NONE);
        }

        /// <summary>
        /// Determines whether the current role is assigned to any active user.
        /// </summary>
        /// <param name="roleId">The current Role's ID</param>
        /// <returns></returns>
        Boolean ISecurityRepository.IsRoleInUse(String roleId)
        {
            Guid gRoleId = new Guid(roleId);
            return (_dbNavigation.aspnet_Users.Count(user => user.aspnet_Roles.Any(aspnetRoles => aspnetRoles.RoleId == gRoleId)
                                                             && user.OrganizationUsers.Any(organizationUsers => organizationUsers.IsDeleted == false)) > AppConsts.NONE);
        }

        /// <summary>
        /// Retrieves a list of all aspnet users based on userName.
        /// </summary>
        /// <param name="userName">The value for user's name.</param>
        /// <returns>A <see cref="aspnet_Roles"/> list of data from the underlying data storage.</returns>
        List<aspnet_Roles> ISecurityRepository.GetUserRoles(String userName) // left as list because there are operations performed on the list of data obtained.
        {
            aspnet_Users user = CompiledGetUserRoles(_dbNavigation, userName);

            if (!user.IsNull())
            {
                user.aspnet_Roles.Load();
                return user.aspnet_Roles.ToList();
            }
            return new List<aspnet_Roles>();
        }

        /// <summary>
        /// Determines whether the specified feature is available for the user.
        /// </summary>
        /// <param name="userName"> The user name of the current user.</param>
        /// <param name="featureId">The current feature ID.</param>
        /// <param name="blockId">  The current Block ID.</param>
        /// <returns>
        /// true if feature available to user, false if not.
        /// </returns>
        Boolean ISecurityRepository.IsFeatureAvailableToUser(String userName, Int32 featureId, Int32 blockId)
        {
            List<Guid> userRoleIds = _dbNavigation.aspnet_Roles.Where(aspnetRoles => aspnetRoles.aspnet_Users.Any(aspnetUsers => aspnetUsers.UserName == userName)
                                                                                     && aspnetRoles.RoleDetail.IsActive
                                                                                     && aspnetRoles.RoleDetail.IsDeleted == false).Select(roles => roles.RoleId).ToList();
            //embed user group role
            List<Int32> UserGroupIds = _dbNavigation.UsersInUserGroups.Select(condtion => condtion.UserGroupID).ToList();
            List<Guid> userGroupRole = (from c in _dbNavigation.RolesInUserGroups
                                        join role in _dbNavigation.aspnet_Roles.Include("Aspnet_Roles.RoleDetail") on c.RoleID equals role.RoleId
                                        where UserGroupIds.Contains(c.UserGroupID) && role.RoleDetail.IsUserGroupLevel == true
                                        select role.RoleId).ToList();
            List<Guid> UnionRole = userGroupRole.Union(userRoleIds).ToList();

            //end embading usergroup role

            return _dbNavigation.RolePermissionProductFeatures.Count(rolePermissionProductFeatures => UnionRole.Contains(rolePermissionProductFeatures.RoleId)
                                                                                                                  && rolePermissionProductFeatures.SysXBlocksFeature.SysXBlockID == blockId &&
                                                                                                                  rolePermissionProductFeatures.SysXBlocksFeature.FeatureID == featureId) > AppConsts.NONE;
        }

        /// <summary>
        /// Retrieves a list of all roles of user based on User's ID.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>A <see cref="aspnet_Roles"/> list of data from the underlying data storage.</returns>
        List<aspnet_Roles> ISecurityRepository.GetUserRolesById(String userId) // left as list because there are operations performed on the list of data obtained.
        {
            aspnet_Users user = CompiledGetUserRolesById(_dbNavigation, userId);

            if (!user.IsNull())
            {
                user.aspnet_Roles.Load();
                return user.aspnet_Roles.ToList();
            }
            return new List<aspnet_Roles>();
        }

        /// <summary>
        /// Retrieves a list of all users in a particular role.
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns>A <see cref="aspnet_Users"/> list of data from the underlying data storage.</returns>
        List<aspnet_Users> ISecurityRepository.GetUsersInRole(String roleName) // left as list because there are operations performed on the list of data obtained.
        {
            aspnet_Roles role = CompiledGetUsersInRole(_dbNavigation, roleName);

            if (!role.IsNull())
            {
                role.aspnet_Users.Load();
                return role.aspnet_Users.ToList();
            }

            return null;
        }

        /// <summary>
        /// Determines whether the current use is associated with current role.
        /// </summary>
        /// <param name="userName">The current user's name</param>
        /// <param name="roleName">The current role name</param>
        /// <returns></returns>
        Boolean ISecurityRepository.IsUserInRole(String userName, String roleName)
        {
            aspnet_Users user = _dbNavigation.aspnet_Users.FirstOrDefault(aspnetUsers => aspnetUsers.UserName == userName);

            if (!user.IsNull())
            {
                user.aspnet_Roles.Load();
                return (user.aspnet_Roles.Count(aspnetRoles => aspnetRoles.RoleName == roleName) > AppConsts.NONE);
            }
            return false;
        }

        /// <summary>
        /// Retrieves an array for user name in particular role name and name would match.
        /// </summary>
        /// <exception cref="Exception">Thrown when an exception error condition occurs.</exception>
        /// <param name="roleName">       The value for role name.</param>
        /// <param name="userNameToMatch">The value for username to match.</param>
        /// <returns>
        /// An enumerator that allows foreach to be used to find users in role in this collection.
        /// </returns>
        IEnumerable<aspnet_Users> ISecurityRepository.FindUsersInRole(String roleName, String userNameToMatch)
        {
            aspnet_Roles aspnetRoles = _dbNavigation.aspnet_Roles.FirstOrDefault(aspnetRoleDetails => aspnetRoleDetails.RoleName == roleName);
            if (!aspnetRoles.IsNull())
            {
                aspnetRoles.aspnet_Users.Load();
                return aspnetRoles.aspnet_Users.Where(aspnetUsers => aspnetUsers.UserName.Contains(userNameToMatch));
            }
            throw new Exception(SysXUtils.GetMessage(ResourceConst.SECURITY_ROLE_NAME) + SysXUtils.GetMessage(ResourceConst.SPACE) + roleName + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.SECURITY_NOT_FOUND));
        }

        /// <summary>
        /// Save mapping of users and role.
        /// </summary>
        /// <param name="roleMapList">The value for mapping of roles.</param>
        void ISecurityRepository.SaveRoleMapping(List<vw_aspnet_UsersInRoles> roleMapList)
        {
            DeleteExistingMappedRoles(roleMapList[0].UserId);
            foreach (vw_aspnet_UsersInRoles role in roleMapList)
            {
                _dbNavigation.vw_aspnet_UsersInRoles.AddObject(role);
            }
        }

        /// <summary>
        /// Retrieves the User details of Super Admin.
        /// </summary>
        /// <returns>
        /// An enumerator that allows foreach to be used to process get system x coordinate admin user
        /// identifiers in this collection.
        /// </returns>
        IEnumerable<Int32> ISecurityRepository.GetSysXAdminUserIds()
        {
            String sysXAdminRoleName = CurrentRepositoryContext.GetSysXConfigValue(SysXSecurityConst.SYSX_ADMIN_ROLE_KEY_NAME);
            List<OrganizationUser> organizationUsers = CompiledGetSysXAdminUserIds.Invoke(_dbNavigation, sysXAdminRoleName).ToList();
            return (from organizationuserDetails in organizationUsers
                    select organizationuserDetails.OrganizationUserID);
        }

        /// <summary>
        /// Gets the roles by user id.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns>
        /// The roles by user identifier.
        /// </returns>
        IQueryable<aspnet_Roles> ISecurityRepository.GetRolesByUserId(String userId, Int32 passedOrganizationID = -1)
        {
            Int32 organizationId = passedOrganizationID == -1 ? CompiledGetOrganizationUserDetailsByUserId.Invoke(_dbNavigation, userId).OrganizationID : passedOrganizationID;
            Int32 tenantId = Convert.ToInt32(CompiledGetOrganizationDetailsByOrganizationId.Invoke(_dbNavigation, organizationId).TenantID);
            Int32 tenantProductId = CompiledGetTenantProductDetailsByTenantId.Invoke(_dbNavigation, tenantId).TenantProductID;
            return CompiledGetAspnetRolesByTenantProductId(_dbNavigation, tenantProductId);
        }

        /// <summary>
        /// Gets the roles by user id.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns>
        /// The roles by user identifier.
        /// </returns>
        IQueryable<aspnet_Roles> ISecurityRepository.GetRolesByUserIdInUserGroup(Int32 UserGroupId)
        {
            Int32 tenantId = _dbNavigation.UserGroups.Where(condtion => condtion.UserGroupID == UserGroupId).FirstOrDefault().TenantID;
            TenantProduct tenantProduct = CompiledGetTenantProductDetailsByTenantId.Invoke(_dbNavigation, tenantId);
            if (!tenantProduct.IsNull())
            {
                return CompiledGetAspnetRolesByTenantProductIdInUserGroup(_dbNavigation, tenantProduct.TenantProductID);
            }
            else
            {
                //Line can be remove once data would be consistent
                return CompiledGetAspnetRolesByTenantProductIdInUserGroup(_dbNavigation, AppConsts.NONE);
            }
        }

        /// <summary>
        /// Gets the roles by tenant's product id.
        /// </summary>
        /// <param name="tenantProductId">Current tenant's product id.</param>
        /// <returns>
        /// The roles by user identifier.
        /// </returns>
        IQueryable<aspnet_Roles> ISecurityRepository.GetAspnetRolesByTenantProductId(Int32 tenantProductId)
        {
            return CompiledGetAspnetRolesByTenantProductId(_dbNavigation, tenantProductId);
        }

        /// <summary>
        /// Returns the value for tenant name based on any of the input provided.
        /// </summary>
        /// <param name="currentTenantId">The value for Tenant Id.</param>
        /// <param name="assignToProductId">The value for Product Id.</param>
        /// <param name="assignToDepartmentId">The value for department Id.</param>
        /// <returns></returns>
        String ISecurityRepository.GetTenantName(Int32 currentTenantId, Int32 assignToProductId, Int32 assignToDepartmentId)
        {
            Int32? tenantId = currentTenantId;

            if (!assignToProductId.Equals(AppConsts.NONE))
            {
                TenantProduct tenantProduct = _dbNavigation.TenantProducts.FirstOrDefault(condition => condition.TenantProductID == assignToProductId);

                if (tenantProduct != null)
                {
                    tenantId = tenantProduct.TenantID;
                }
            }

            if (!assignToDepartmentId.Equals(AppConsts.NONE))
            {
                Organization organization = _dbNavigation.Organizations.FirstOrDefault(condition => condition.OrganizationID == assignToDepartmentId);

                if (organization != null)
                {
                    tenantId = organization.TenantID;
                }
            }

            if (tenantId.Equals(AppConsts.NONE))
            {
                return String.Empty;
            }

            Tenant tenant = _dbNavigation.Tenants.FirstOrDefault(condition => condition.TenantID == tenantId);
            return tenant != null ? tenant.TenantName : String.Empty;
        }

        #endregion

        #region Manage Role Details

        /// <summary>
        /// Performs an insert operation for RoleDetail.
        /// </summary>
        /// <param name="roleDetail">The value for role detail.</param>
        RoleDetail ISecurityRepository.AddRoleDetail(RoleDetail roleDetail)
        {
            aspnet_Applications application = CurrentRepositoryContext.GetApplication();
            Int32 tenantID = _dbNavigation.Tenants.Where(t => t.TenantID.Equals(roleDetail.TenantProduct.TenantID)).FirstOrDefault().TenantID;
            string roleTypeOther = RoleTypes.AAD.ToString();
            //lkpRoleType lkpRoleType = _dbNavigation.lkpRoleTypes.FirstOrDefault(roleType => roleType.Code.Equals(roleTypeOther));
            //if (lkpRoleType != null)
            //  roleDetail.RoleTypeId = lkpRoleType.RoleTypeId;

            aspnet_Roles aspnetRole = new aspnet_Roles
            {
                ApplicationId = application.ApplicationId,

                //TODO: Below commented line will be removed as the case is finalized.
                //LoweredRoleName = roleDetail.Name.ToLower(),
                //RoleName = roleDetail.Name,

                LoweredRoleName = roleDetail.Name.ToLower() + "_" + tenantID.ToString(),
                RoleName = roleDetail.Name + "_" + tenantID.ToString(),
                RoleId = Guid.NewGuid(),
                Description = roleDetail.Description
            };

            aspnetRole = this.AddObjectEntityInTransaction(aspnetRole);
            roleDetail.RoleDetailID = aspnetRole.RoleId;

            roleDetail.Name = roleDetail.Name + "_" + tenantID.ToString();



            return this.AddObjectEntity(roleDetail);
        }

        /// <summary>
        /// Performs an update operation for RoleDetail Entity.
        /// </summary>
        /// <param name="roleDetail">.</param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        Boolean ISecurityRepository.UpdateRole(RoleDetail roleDetail)
        {
            aspnet_Roles aspnetRoles = roleDetail.aspnet_Roles;

            //TODO: Below commented line will be removed as the case is finalized.
            //aspnetRoles.LoweredRoleName = roleDetail.Name.ToLower();
            //aspnetRoles.RoleName = roleDetail.Name;

            Int32 tenantID = _dbNavigation.Tenants.Where(condition => condition.TenantID.Equals(roleDetail.TenantProduct.TenantID)).FirstOrDefault().TenantID;

            if (!roleDetail.Name.EndsWith("_" + tenantID.ToString()))
            {
                roleDetail.Name = roleDetail.Name + "_" + tenantID.ToString();
                aspnetRoles.LoweredRoleName = roleDetail.Name.ToLower() + "_" + tenantID.ToString();
                //aspnetRoles.RoleName = roleDetail.Name + "_" + tenantID.ToString();
                aspnetRoles.RoleName = roleDetail.Name;
            }
            else
            {
                aspnetRoles.LoweredRoleName = roleDetail.Name.ToLower();
            }

            aspnetRoles.Description = roleDetail.Description;
            this.UpdateObjectEntity(roleDetail);

            return true;
        }

        /// <summary>
        /// Performs a delete operation for RoleDetail Entity.
        /// </summary>
        /// <param name="roleDetail">.</param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        Boolean ISecurityRepository.DeleteRoleDetail(RoleDetail roleDetail)
        {
            roleDetail.IsDeleted = true;
            base.DeleteObjectEntityInTransaction(roleDetail);
            aspnet_Roles role = CurrentRepositoryContext.GetAspnetRole(roleDetail.RoleDetailID.ToString());
            role.RoleName = role.RoleName + "_" + Guid.NewGuid();
            role.LoweredRoleName = role.LoweredRoleName + "_" + Guid.NewGuid();

            //TODO: Removes the mapping of features with the current role.
            // DeleteRolePermissionProductFeatures(roleDetail.RoleDetailID);
            _dbNavigation.SaveChanges();
            return true;
        }

        /// <summary>
        /// Retrieves a list of all role details.
        /// </summary>
        /// <returns>
        /// The role detail.
        /// </returns>
        IQueryable<RoleDetail> ISecurityRepository.GetRoleDetail()
        {
            return CompiledGetRoleDetails(_dbNavigation);
        }

        /// <summary>
        /// Retrieves role details.
        /// </summary>
        /// <param name="isAdmin">      The value for isAdmin</param>
        /// <param name="currentUserId">The value for current user's Id.</param>
        /// <returns>
        /// The role detail.
        /// </returns>
        IQueryable<RoleDetail> ISecurityRepository.GetRoleDetail(Boolean isAdmin, Int32 currentUserId)
        {
            if (isAdmin)
            {
                return CompiledGetAllActiveRolesWithItsDetails.Invoke(_dbNavigation);
            }
            else
            {
                var currentUserDetails = CompiledCurrentUserDetails.Invoke(_dbNavigation, currentUserId).FirstOrDefault();
                Int32 currentUserOrganizationId = currentUserDetails.OrganizationID;
                Int32 currentUserCreatedById = currentUserDetails.CreatedByID;

                var currentUserOrganizationDetails = CompiledCurrentUserOrganizationDetails.Invoke(_dbNavigation, currentUserOrganizationId).FirstOrDefault();


                // Check Whether the logged in user is Product admin.
                if (!currentUserOrganizationDetails.IsNull())
                {
                    Int32 currentUserOrganizationTenantId = Convert.ToInt32(currentUserOrganizationDetails.TenantID);
                    var compiledCurrentUserProductDetails = CompiledCurrentUserProductDetails.Invoke(_dbNavigation, currentUserOrganizationTenantId).FirstOrDefault();

                    if (!compiledCurrentUserProductDetails.IsNull())
                    {
                        Int32 currentUserProductTenantProductId = compiledCurrentUserProductDetails.TenantProductID;
                        return CompiledRoleDetailsByCurrentUserProductTenantProductIdAndCurrentUserCreatedById.Invoke(_dbNavigation, currentUserProductTenantProductId, currentUserCreatedById);
                    }
                }
                else
                {
                    return _dbNavigation.RoleDetails.Include(SysXEntityConstants.TABLE_TENANT_PRODUCT_DOT_TENANT)
                                                                .Include(SysXEntityConstants.TABLE_ORGANIZATION_USER).
                        Where(role => role.IsActive && role.IsDeleted == false && role.CreatedByID == currentUserId).OrderBy(condition => condition.CreatedOn);
                }
            }

            return null;
        }

        /// <summary>
        /// Retrieves role details based on roleDetailId.
        /// </summary>
        /// <exception cref="Exception">Thrown when an exception error condition occurs.</exception>
        /// <param name="roleDetailId">role id</param>
        /// <returns>
        /// The role detail by identifier.
        /// </returns>
        RoleDetail ISecurityRepository.GetRoleDetailById(String roleDetailId)
        {
            RoleDetail roleDetail = CompiledGetRoleDetailById(_dbNavigation, roleDetailId);

            if (!roleDetail.IsNull())
            {
                return roleDetail;
            }
            throw new Exception(SysXUtils.GetMessage(ResourceConst.SECURITY_ROLE_DETAIL_ID) + SysXUtils.GetMessage(ResourceConst.SPACE) + roleDetailId + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.SECURITY_NOT_FOUND));
        }

        /// <summary>
        /// Retrieves an instance of application.
        /// </summary>
        /// <returns>
        /// The application.
        /// </returns>
        aspnet_Applications ISecurityRepository.GetApplication()
        {
            //return _dbNavigation.aspnet_Applications.FirstOrDefault();
            return CompiledGetApplication(_dbNavigation);
        }

        /// <summary>
        /// Retrieves roles based on role id, while navigating through Manage role page.
        /// </summary>
        /// <param name="loadRoleDetails">load role details</param>
        /// <param name="assignToRoleId">role id</param>
        /// <returns></returns>
        IQueryable<aspnet_Roles> ISecurityRepository.GetRolesByAssignToRoleId(Boolean loadRoleDetails, String assignToRoleId)
        {
            IQueryable<aspnet_Roles> roles = CompiledGetRolesByAssignToRoleId(_dbNavigation, loadRoleDetails, assignToRoleId);

            if (loadRoleDetails)
            {
                foreach (aspnet_Roles role in roles)
                {
                    role.RoleDetailReference.Load();
                }
            }
            return roles;
        }

        /// <summary>
        /// Checks if the role exists in system database.
        /// </summary>
        /// <param name="productId">The value for product id.</param>
        /// <param name="enteredRoleName"> The value for entered Role's Name.</param>
        /// <param name="existingRoleName">The value for existing Role Name.</param>
        /// <returns>
        /// true if role exists, false if not.
        /// </returns>
        Boolean ISecurityRepository.IsRoleExists(Int32 productId, String enteredRoleName, String existingRoleName = null)
        {
            // executes when the form is in insert mode.
            if (String.IsNullOrEmpty(existingRoleName))
            {
                return CompiledGetRoleDetailsByProductId(_dbNavigation, productId).Where(roleDetails => roleDetails.Name.ToLower() == enteredRoleName.ToLower()).Count() > AppConsts.NONE;
            }
            // executes when the form is in update mode.
            else
            {
                return CompiledGetRoleDetailsByProductId(_dbNavigation, productId).ToList().FindAll(roleDetails => roleDetails.Name.ToLower() == enteredRoleName.ToLower())
                    .SkipWhile(roleDetails => existingRoleName.ToLower() == enteredRoleName.ToLower())
                    .Any(nameChecks => nameChecks.Name.ToLower().Equals(enteredRoleName.ToLower()));
            }
        }

        List<String> ISecurityRepository.getDefaultRoleDetailIds(List<Int32> productId)
        {
            List<String> defaultRoleDetailIds = new List<String>();
            foreach (Int32 id in productId)
            {
                String roleCode = defaultRole.Applicant.GetStringValue();
                RoleDetail roleDetail = _dbNavigation.RoleDetails.FirstOrDefault(obj => obj.ProductID == id && obj.DefaultRole.Code.Equals(roleCode) && obj.IsDeleted == false);
                defaultRoleDetailIds.Add(roleDetail.RoleDetailID.ToString());
            }
            return defaultRoleDetailIds;
        }

        #endregion

        #region Manage Aspnet Users

        /// <summary>
        /// Retrieves a list of all Aspnet Users.
        /// </summary>
        /// <returns>List<aspnet_Users/></returns>
        IQueryable<aspnet_Users> ISecurityRepository.GetAspnetUsers()
        {
            return CompiledGetAspnetUsers(_dbNavigation);
        }

        /// <summary>
        /// Retrieves a list of Aspnet User based on UserId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>aspnet_Users</returns>
        aspnet_Users ISecurityRepository.GetAspnetUser(String userId)
        {
            aspnet_Users aspnetUser = CompiledGetAspnetUser(_dbNavigation, userId);
            return !aspnetUser.IsNull() ? aspnetUser : null;
        }

        #endregion

        #region Manage Organization Users

        /// <summary>
        /// Gets an organization user information by user identifier.
        /// </summary>
        /// <param name="userId">The value for user's Id.</param>
        /// <returns>
        /// The organization user information by user identifier.
        /// </returns>
        IQueryable<OrganizationUser> ISecurityRepository.GetOrganizationUserInfoByUserId(String userId)
        {
            return CompiledGetOrganizationUserInfoByUserId(_dbNavigation, userId);
        }

        /// <summary>
        /// Gets an organization users details.
        /// </summary>
        /// <param name="userName">The value for user's name.</param>
        /// <returns>
        /// The organization users details.
        /// </returns>
        OrganizationUser ISecurityRepository.GetOrganizationUsersDetails(String userName)
        {
            OrganizationUser organizationUser = CompiledGetOrganizationUsersDetailsByUserName(_dbNavigation, userName);
            return CompiledGetOrganizationUsersDetails(_dbNavigation, organizationUser);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ssn"></param>
        /// <returns></returns>
        OrganizationUser ISecurityRepository.GetOrganizationUsersBySSN(String ssn)
        {
            return _dbNavigation.OrganizationUsers.FirstOrDefault(x => x.SSN.Equals(ssn));
        }



        /// <summary>
        /// Gets an organization users by email.
        /// </summary>
        /// <param name="email">The email address.</param>
        /// <returns>
        /// The organization users by email.
        /// </returns>
        IQueryable<OrganizationUser> ISecurityRepository.GetOrganizationUsersByEmail(String email)
        {
            return CompiledGetOrganizationUsersByEmail(_dbNavigation, email);
        }

        /// <summary>
        ///  Performs an insert operation for Organization User details.
        /// </summary>
        /// <param name="user">The value for user.</param>
        /// <param name="userOrganization">The value for userOrganization.</param>
        /// <returns></returns>
        OrganizationUser ISecurityRepository.AddOrganizationUserInTransaction(aspnet_Users user, OrganizationUser userOrganization)
        {
            //user = base.AddObjectEntityInTransaction(user);
            if (user.IsNotNull())
                userOrganization.aspnet_Users = user;
            return base.AddObjectEntity(userOrganization);
        }

        Boolean ISecurityRepository.UpdateOrganizationUser(Int32 organizationUserId, String newFileName)
        {
            OrganizationUser organizationUserObj = new OrganizationUser();
            organizationUserObj = _dbNavigation.OrganizationUsers.Where(x => x.OrganizationUserID == organizationUserId).FirstOrDefault();
            if (organizationUserObj.IsNullOrEmpty())
            {
                organizationUserObj.PhotoName = newFileName;
            }
            else
            {
                return false;
            }
            _dbNavigation.SaveChanges();
            return true;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationUser"></param>
        /// <returns></returns>
        List<OrganizationUser> ISecurityRepository.SyncUsersProfilePictureInAllTenant(OrganizationUser organizationUser)
        {
            List<OrganizationUser> linkedOrgUserProfile = _dbNavigation.OrganizationUsers.Where(x => x.UserID == organizationUser.UserID && !x.IsDeleted).ToList();
            foreach (OrganizationUser profile in linkedOrgUserProfile)
            {
                profile.PhotoName = organizationUser.PhotoName;
                profile.OriginalPhotoName = organizationUser.OriginalPhotoName;
                profile.ModifiedByID = organizationUser.ModifiedByID;
                profile.ModifiedOn = DateTime.Now;
            }
            if (_dbNavigation.SaveChanges() > 0)
                return linkedOrgUserProfile;
            else
                return new List<OrganizationUser>();
        }


        OrganizationUserProfile ISecurityRepository.AddOrganizationUserProfile(OrganizationUserProfile orgUser)
        {
            return base.AddObjectEntity(orgUser);
        }


        /// <summary>
        /// Retrieves a list of all mapped user with organizations.
        /// </summary>
        /// <param name="isAdmin">The value for isAdmin.</param>
        /// <param name="currentUserId">The value for current user's id.</param>
        /// <returns></returns>
        List<OrganizationUser> ISecurityRepository.GetMappedOrganizationUsers(Boolean isAdmin, Int32 currentUserId)
        {
            if (isAdmin)
            {
                return CompiledGetMappedOrganizationUsersForSystemAdmin.Invoke(_dbNavigation).ToList();
            }
            else
            {
                //Added code to get users for client admin
                var currentUserDetails = CompiledCurrentUserDetails.Invoke(_dbNavigation, currentUserId).FirstOrDefault();
                Int32 currentUserOrganizationId = currentUserDetails.OrganizationID;
                var currentUserOrganizationDetails = CompiledCurrentUserOrganizationDetails.Invoke(_dbNavigation, currentUserOrganizationId).FirstOrDefault();

                // Check Whether the logged in user is Product admin.
                if (currentUserOrganizationDetails.IsNotNull())
                {
                    Int32 currentUserOrganizationOrganizationId = Convert.ToInt32(currentUserOrganizationDetails.OrganizationID);
                    return CompiledGetMappedOrganizationUsersForClientAdmin.Invoke(_dbNavigation, currentUserOrganizationOrganizationId).ToList();
                }
            }

            return null;

            //Commented code to get users for client admin 
            // Check Whether the logged in user is Product admin.
            /*if (!currentUserOrganizationDetails.IsNull())
            {
                Int32 currentUserOrganizationOrganizationId = Convert.ToInt32(currentUserOrganizationDetails.OrganizationID);
                List<OrganizationUser> organizationUsers = new List<OrganizationUser>();
                var departments =
                    CompiledGetDepartmentforProductAdmin.Invoke(_dbNavigation, currentUserOrganizationOrganizationId).
                        ToList();

                foreach (var departmentUsers in departments.Select(department => department.OrganizationID).Select(departmentId => _dbNavigation.OrganizationUsers.Include(SysXEntityConstants.TABLE_ASPNET_USERS)
                                                                                                                                                                    .Include(SysXEntityConstants.TABLE_ASPNET_USERS_DOT_ASPNET_MEMBERSHIP)
                                                                                                                                                                     .Include(SysXEntityConstants.TABLE_ORGANIZATION)
                                                                                                                                                                     .Include(SysXEntityConstants.TABLE_ORGANIZATION_TENANT)
                                                                                                                                                                     .Include(SysXEntityConstants.TABLE_ORGANIZATION_TENANT_LKPTENANTTYPE)
                                                                                                                                                                     .OrderBy(o => o.OrganizationUserID)
                                                                                                                                       .Where(
                                                                                                                                           condition => condition.IsDeleted == false && condition.Organization.IsDeleted == false && condition.OrganizationID == departmentId).ToList()))
                {
                    organizationUsers.AddRange(departmentUsers);
                }

                return organizationUsers.OrderBy(condition => condition.CreatedOn).ToList();
            }
            else
            {
                List<OrganizationUser> organizationUsers = new List<OrganizationUser>();
                var departments = CompiledGetDepartmentforDepartmentAdmin.Invoke(_dbNavigation, currentUserId).ToList();

                foreach (var departmentUsers in departments.Select(department => department.OrganizationID).Select(departmentId => _dbNavigation.OrganizationUsers.Include(SysXEntityConstants.TABLE_ASPNET_USERS)
                                                                                                                                                                    .Include(SysXEntityConstants.TABLE_ASPNET_USERS_DOT_ASPNET_MEMBERSHIP)
                                                                                                                                                                    .Include(SysXEntityConstants.TABLE_ORGANIZATION)
                                                                                                                                                                    .Include(SysXEntityConstants.TABLE_ORGANIZATION_TENANT)
                                                                                                                                                                    .Include(SysXEntityConstants.TABLE_ORGANIZATION_TENANT_LKPTENANTTYPE)
                                                                                                                                                                    .OrderBy(o => o.OrganizationUserID)
                                                                                                                                       .Where(
                                                                                                                                           condition => condition.IsDeleted == false && condition.Organization.IsDeleted == false && condition.OrganizationID == departmentId).ToList()))

                    organizationUsers.AddRange(departmentUsers);

                return organizationUsers.OrderBy(condition => condition.CreatedOn).ToList();
            } */
        }

        /// <summary>
        /// Retrieves a list of all Organization User based on OrganizationUserID.
        /// </summary>
        /// <param name="organizationUserId">The value for organization user's id.</param>
        /// <returns>
        /// OrganizationUser.
        /// </returns>
        OrganizationUser ISecurityRepository.GetOrganizationUser(Int32 organizationUserId)
        {
            OrganizationUser organizationUser = CompiledGetOrganizationUser(_dbNavigation, organizationUserId);
            return !organizationUser.IsNull() ? organizationUser : null;
        }

        /// <summary>
        /// Updates the status of whether the client admin can receive Internal message from Applicants or Not
        /// </summary>
        /// <param name="organizationUserId"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        Boolean ISecurityRepository.UpdateInternalMsgNotificationSettings(Int32 organizationUserId, Int32 currentUserId)
        {
            OrganizationUser organizationUser = CompiledGetOrganizationUser(_dbNavigation, organizationUserId);
            if (organizationUser.IsNullOrEmpty())
                return false;

            organizationUser.IsInternalMsgEnabled = (organizationUser.IsInternalMsgEnabled.IsNullOrEmpty()
                                                     || !Convert.ToBoolean(organizationUser.IsInternalMsgEnabled)) ? true : false;
            organizationUser.ModifiedByID = currentUserId;
            organizationUser.ModifiedOn = DateTime.Now;
            _dbNavigation.SaveChanges();
            return true;
        }

        List<OrganizationUser> ISecurityRepository.GetOrganizationUserListForUserId(String userID)
        {
            return _dbNavigation.OrganizationUsers.Include(SysXEntityConstants.TABLE_ASPNET_USERS)
                                                .Include(SysXEntityConstants.TABLE_ASPNET_USERS_DOT_ASPNET_MEMBERSHIP)
                                                .Include(SysXEntityConstants.TABLE_ADDRESSHANDLE_ADDRESS)
                                                .Include(SysXEntityConstants.TABLE_ADDRESSHANDLE_ADDRESS_ZIPCODE)
                                                .Include(SysXEntityConstants.TABLE_RESIDENTIALHISTORIES)
                                                .Where(orgUser => orgUser.UserID.Equals(new Guid(userID)) && orgUser.IsDeleted == false).ToList();
        }

        List<OrganizationUser> ISecurityRepository.GetOrganizationUserList(List<Int32> lstOrganizationUserIds)
        {
            return _dbNavigation.OrganizationUsers.Where(orgUser => lstOrganizationUserIds.Contains(orgUser.OrganizationUserID) && orgUser.IsDeleted == false)
                .Select(x => x).ToList();
        }

        /// <summary>
        /// Gets the list of residential histories for the given organisation user ID.
        /// </summary>
        /// <param name="organizationUserId">Organisation User ID</param>
        /// <returns>List of residential histories</returns>
        List<ResidentialHistory> ISecurityRepository.GetUserResidentialHistories(Int32 organizationUserId)
        {
            return _dbNavigation.ResidentialHistories
                 .Include(SysXEntityConstants.TABLE_RH_ADDRESS)
                 .Include(SysXEntityConstants.TABLE_RH_ADDRESS_ZIPCODE)
                 .Include(SysXEntityConstants.TABLE_RH_ADDRESS_ZIPCODE_CITY)
                 .Include(SysXEntityConstants.TABLE_RH_ADDRESS_ZIPCODE_COUNTY_STATE)
                 .Where(cond => cond.RHI_OrganizationUserID == organizationUserId && cond.RHI_IsDeleted == false).ToList();
        }

        /// <summary>
        /// Gets the list of residential history profiles for the given organisation user profile ID.
        /// </summary>
        /// <param name="organizationUserProfileId">Organization User Profile Id</param>
        /// <returns>List of residential history profiles</returns>
        List<ResidentialHistoryProfile> ISecurityRepository.GetUserResidentialHistoryProfiles(Int32 organizationUserProfileId)
        {
            return _dbNavigation.ResidentialHistoryProfiles
                 .Include(SysXEntityConstants.TABLE_RH_ADDRESS)
                 .Include(SysXEntityConstants.TABLE_RH_ADDRESS_ZIPCODE)
                 .Include(SysXEntityConstants.TABLE_RH_ADDRESS_ZIPCODE_CITY)
                 .Include(SysXEntityConstants.TABLE_RH_ADDRESS_ZIPCODE_COUNTY_STATE)
                 .Where(cond => cond.RHIP_OrganizationUserProfileID == organizationUserProfileId && cond.RHIP_IsDeleted == false).ToList();
        }

        /// <summary>
        /// Retrieves all the data relevant to a particular user.
        /// </summary>
        /// <param name="organizationUserId">The value for organization user's id.</param>
        /// <returns>
        /// OrganizationUser.
        /// </returns>
        OrganizationUser ISecurityRepository.GetOrganizationUserDetail(Int32 organizationUserId)
        {
            OrganizationUser organizationUser = CompiledGetOrganizationUserDetail(_dbNavigation, organizationUserId);
            return !organizationUser.IsNull() ? organizationUser : null;
        }

        /// <summary>
        /// Retrieves Organization User based on OrganizationUserID.
        /// </summary>
        /// <param name="organizationUserId">The value for organization user's id.</param>
        /// <returns>
        /// OrganizationUser.
        /// </returns>
        OrganizationUser ISecurityRepository.GetOrganizationUserByVerificationCode(String userVerificationCode)
        {
            OrganizationUser organizationUser = CompiledGetOrganizationUserByVerificationCode(_dbNavigation, userVerificationCode);
            return !organizationUser.IsNull() ? organizationUser : null;
        }

        /// <summary>
        /// Performs a delete operation for OrganizationUser Details.
        /// </summary>
        /// <param name="organizationUser">The value for organization User.</param>
        /// <returns></returns>
        public Boolean DeleteOrganizationUser(OrganizationUser organizationUser)
        {
            organizationUser.IsDeleted = true;
            base.DeleteObjectEntityInTransaction(organizationUser);
            if (!(_dbNavigation.OrganizationUsers.Where(cond => cond.UserID == organizationUser.UserID && cond.IsDeleted == false).Any()) || organizationUser.IsApplicant == true)
            {
                aspnet_Users user = CurrentRepositoryContext.GetAspnetUser(organizationUser.UserID.ToString());
                user.UserName = user.UserName + "_" + Guid.NewGuid();
                user.LoweredUserName = user.LoweredUserName + "_" + Guid.NewGuid();

                aspnet_Membership membership = CurrentRepositoryContext.GetAspnetMembershipById(organizationUser.UserID);
                membership.Email = membership.Email + "_" + Guid.NewGuid();
                membership.LoweredEmail = membership.LoweredEmail + "_" + Guid.NewGuid();
            }
            _dbNavigation.SaveChanges();
            return true;
        }

        /// <summary>
        /// Checks whether the currently selected organization is of the supplier type tenant.
        /// </summary>
        /// <param name="selectedOrganizationId">currently selected organization's id.</param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        Boolean ISecurityRepository.IsOrganizationOfSupplier(Int32 selectedOrganizationId)
        {
            Int32? orgId = null;
            Organization currentOrganization = _dbNavigation.Organizations.FirstOrDefault(condition => condition.OrganizationID == selectedOrganizationId);

            if (!currentOrganization.IsNull())
            {
                orgId = currentOrganization.ParentOrganizationID.IsNullOrEmpty() ? currentOrganization.OrganizationID : currentOrganization.ParentOrganizationID;
            }

            Int32? tenantTypeId = _dbNavigation.Tenants.FirstOrDefault(condition => condition.Organizations.Any(item => item.OrganizationID == orgId)).TenantTypeID;
            String tenantTypeCode = _dbNavigation.lkpTenantTypes.FirstOrDefault(condition => condition.TenantTypeID == tenantTypeId).TenantTypeCode;

            return tenantTypeCode.Equals(TenantType.Supplier.GetStringValue());
        }

        /// <summary>
        /// GetAllTenantTypes()
        /// </summary>
        /// <returns></returns>
        public List<lkpTenantType> GetAllTenantTypes()
        {
            return (_dbNavigation.lkpTenantTypes).ToList<lkpTenantType>();
        }

        /// <summary>
        /// GetAllContactTypes()
        /// </summary>
        /// <returns></returns>
        public List<lkpContactType> GetAllContactTypes()
        {
            return (_dbNavigation.lkpContactTypes).ToList<lkpContactType>();
        }

        /// <summary>
        /// Gets the list of users working in the organization associated with the given tenant Id. 
        /// </summary>
        /// <param name="tenantId">Tenant Id</param>
        /// <returns>List of acive Users</returns>
        public List<OrganizationUser> GetOganisationUsersByTanentId(Int32 tenantId, Boolean IsAdbAdmin, Boolean IsComplAssignmentScreen, Boolean RotAssignmentScreen, Boolean IsDataEntryScreen, Boolean IsLocEnrollerScreen)
        {

            //    List<OrganizationUser> OrganizationUsers = new List<OrganizationUser>();
            //    EntityConnection connection = _dbNavigation.Connection as EntityConnection;
            //    using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            //    {
            //        SqlCommand command = new SqlCommand("dbo.usp_GetRoleBasedUsersList", con);
            //        command.CommandType = CommandType.StoredProcedure;
            //        command.Parameters.AddWithValue("@TenantID", tenantId);
            //        command.Parameters.AddWithValue("@IsAdbAdmin", IsAdbAdmin);
            //        command.Parameters.AddWithValue("@IsComplAssignmentScreen", IsComplAssignmentScreen);
            //        command.Parameters.AddWithValue("@RotAssignmentScreen", RotAssignmentScreen);
            //        command.Parameters.AddWithValue("@IsDataEntryScreen", IsDataEntryScreen);
            //        command.Parameters.AddWithValue("@IsLocEnrollerScreen", IsLocEnrollerScreen);
            //        if (con.State == ConnectionState.Closed)
            //            con.Open();
            //        SqlDataReader dr = command.ExecuteReader();
            //        while (dr.Read())
            //        {
            //            OrganizationUser user = new OrganizationUser();
            //            user.OrganizationUserID = dr["OrganizationUserID"] != DBNull.Value ? Convert.ToInt32(dr["OrganizationUserID"]) : 0;
            //            user.UserID = new Guid(Convert.ToString(dr["UserID"]));
            //            user.OrganizationID = dr["OrganizationID"] != DBNull.Value ? Convert.ToInt32(dr["OrganizationID"]) : 0;
            //            user.BillingAddressID = dr["BillingAddressID"] != DBNull.Value ? Convert.ToInt32(dr["BillingAddressID"]) : (int?)null;
            //            user.ContactID = dr["ContactID"] != DBNull.Value ? Convert.ToInt32(dr["ContactID"]) : (int?)null;
            //            user.UserTypeID = dr["UserTypeID"] != DBNull.Value ? Convert.ToInt32(dr["UserTypeID"]) : (int?)null;
            //            user.DepartmentID = dr["DepartmentID"] != DBNull.Value ? Convert.ToInt32(dr["DepartmentID"]) : (int?)null;
            //            user.SysXBlockID = dr["SysXBlockID"] != DBNull.Value ? Convert.ToInt32(dr["SysXBlockID"]) : (int?)null;
            //            user.AddressHandleID = dr["AddressHandleID"] != DBNull.Value ? new Guid(Convert.ToString(dr["AddressHandleID"])) : (Guid?)null;
            //            user.FirstName = Convert.ToString(dr["FirstName"]);
            //            user.LastName = Convert.ToString(dr["LastName"]);
            //            user.VerificationCode = Convert.ToString(dr["VerificationCode"]);
            //            user.OfficeReturnDateTime = dr["OfficeReturnDateTime"] != DBNull.Value ? Convert.ToDateTime(dr["OfficeReturnDateTime"]) : (DateTime?)null;
            //            user.IsOutOfOffice = Convert.ToBoolean(dr["IsOutOfOffice"]);
            //            user.IsNewPassword = Convert.ToBoolean(dr["IsNewPassword"]);
            //            user.IgnoreIPRestriction = Convert.ToBoolean(dr["IgnoreIPRestriction"]);
            //            user.IsMessagingUser = Convert.ToBoolean(dr["IsMessagingUser"]);
            //            user.IsSystem = Convert.ToBoolean(dr["IsSystem"]);
            //            user.IsDeleted = Convert.ToBoolean(dr["IsDeleted"]);
            //            user.IsActive = Convert.ToBoolean(dr["IsActive"]);
            //            user.ExpireDate = dr["ExpireDate"] != DBNull.Value ? Convert.ToDateTime(dr["ExpireDate"]) : (DateTime?)null;
            //            user.CreatedByID = dr["CreatedByID"] != DBNull.Value ? Convert.ToInt32(dr["CreatedByID"]) : 0;
            //            user.CreatedOn = Convert.ToDateTime(dr["CreatedOn"]);
            //            user.ModifiedByID = dr["ModifiedByID"] != DBNull.Value ? Convert.ToInt32(dr["ModifiedByID"]) : (int?)null;
            //            user.ModifiedOn = dr["ModifiedOn"] != DBNull.Value ? Convert.ToDateTime(dr["ModifiedOn"]) : (DateTime?)null;
            //            user.IsSubscribeToEmail = Convert.ToBoolean(dr["IsSubscribeToEmail"]);
            //            user.IsApplicant = dr["IsApplicant"] != DBNull.Value ? Convert.ToBoolean(dr["IsApplicant"]) : (Boolean?)null;
            //            user.PhotoName = Convert.ToString(dr["PhotoName"]);
            //            user.OriginalPhotoName = Convert.ToString(dr["OriginalPhotoName"]);
            //            user.DOB = dr["DOB"] != DBNull.Value ? Convert.ToDateTime(dr["DOB"]) : (DateTime?)null;
            //            user.SSN = Convert.ToString(dr["SSN"]);
            //            user.Gender = dr["Gender"] != DBNull.Value ? Convert.ToInt32(dr["Gender"]) : (int?)null;
            //            user.PhoneNumber = Convert.ToString(dr["PhoneNumber"]);
            //            user.MiddleName = Convert.ToString(dr["MiddleName"]);
            //            user.Alias1 = Convert.ToString(dr["Alias1"]);
            //            user.Alias2 = Convert.ToString(dr["Alias2"]);
            //            user.Alias3 = Convert.ToString(dr["Alias3"]);
            //            user.PrimaryEmailAddress = Convert.ToString(dr["PrimaryEmailAddress"]);
            //            user.SecondaryEmailAddress = Convert.ToString(dr["SecondaryEmailAddress"]);
            //            user.SecondaryPhone = Convert.ToString(dr["SecondaryPhone"]);
            //            user.UserVerificationCode = Convert.ToString(dr["UserVerificationCode"]);
            //            user.ActiveDate = dr["ActiveDate"] != DBNull.Value ? Convert.ToDateTime(dr["ActiveDate"]) : (DateTime?)null;
            //            user.IsInternalMsgEnabled = dr["IsInternalMsgEnabled"] != DBNull.Value ? Convert.ToBoolean(dr["IsInternalMsgEnabled"]) : (Boolean?)null;
            //            user.IsSharedUser = dr["IsSharedUser"] != DBNull.Value ? Convert.ToBoolean(dr["IsSharedUser"]) : (Boolean?)null;
            //            user.SSNL4 = Convert.ToString(dr["SSNL4"]);
            //            user.IsInternationalPhoneNumber = Convert.ToBoolean(dr["IsInternationalPhoneNumber"]);
            //            user.IsInternationalSecondaryPhone = Convert.ToBoolean(dr["IsInternationalSecondaryPhone"]);
            //            OrganizationUsers.Add(user);
            //    }
            //        con.Close();
            //}
            //    return OrganizationUsers;
            Int32 organizationId = 0;
            Organization organization = _dbNavigation.Organizations.Where(obj => obj.TenantID == tenantId && obj.IsActive == true && obj.IsDeleted == false).FirstOrDefault();
            if (organization.IsNotNull())
            {
                organizationId = organization.OrganizationID;
            }


            var tempUserList = _dbNavigation.OrganizationUsers.Where(obj => obj.OrganizationID == organizationId && obj.IsActive == true && obj.IsDeleted == false
                && obj.IsApplicant == false
                && (obj.IsSharedUser ?? false) == false)//check for Shared User UAT-1122- Profile Sharing.
                .ToList();
            if (IsAdbAdmin)
            {
                List<OrganizationUser> roleBasedUsersList = new List<OrganizationUser>();
                foreach (var item in tempUserList)
                {
                    //var aas = tempUserList.Any(x => x.FirstName.Contains("cbi"));
                    var roleUsers = item.aspnet_Users.aspnet_Roles.Where(a => a.aspnet_RolePreferredTenantSetting.Any(asp => !asp.RPTS_IsDeleted && ((IsComplAssignmentScreen && asp.RPTS_IsAllowComplianceVerfication) ||
                         (RotAssignmentScreen && asp.RPTS_IsAllowRotationVerfication) || (IsDataEntryScreen && asp.RPTS_IsAllowDataEntry) || (IsLocEnrollerScreen && asp.RPTS_IsAllowLocationEnroller) ||
                         (!IsComplAssignmentScreen && !RotAssignmentScreen && !IsDataEntryScreen && !IsLocEnrollerScreen)))).ToList();
                    if (roleUsers.Count > 0)
                    {
                        roleBasedUsersList.Add(item);
                    }
                }
                return roleBasedUsersList;
            }
            else
            {
                return tempUserList;
            }
        }

        public List<OrganizationUser> GetOganisationUsersByTanentIdOfLoggedInUser(Int32 tenantId, Int32 currentloggedInUser)
        {
            Int32 organizationId = 0;
            Organization organization = _dbNavigation.Organizations.Where(obj => obj.TenantID == tenantId && obj.IsActive == true && obj.IsDeleted == false).FirstOrDefault();
            if (organization.IsNotNull())
            {
                organizationId = organization.OrganizationID;
            }
            return _dbNavigation.OrganizationUsers.Where(obj => obj.OrganizationID == organizationId && obj.OrganizationUserID == currentloggedInUser && obj.IsActive == true && obj.IsDeleted == false && obj.IsApplicant == false).ToList();
        }

        public List<OrganizationUser> GetOganisationUsersByUserID(Int32 organizationUserID)
        {
            return _dbNavigation.OrganizationUsers.Where(obj => obj.OrganizationUserID == organizationUserID && obj.IsActive == true && obj.IsDeleted == false && obj.IsApplicant == false).ToList();
        }

        /// <summary>
        /// Organization Users list without IsActive check.
        /// </summary>
        /// <param name="organizationUserID"></param>
        /// <returns></returns>
        public List<OrganizationUser> GetOganisationUsersByUserIDForLogin(Int32 organizationUserID)
        {
            return _dbNavigation.OrganizationUsers.Where(obj => obj.OrganizationUserID == organizationUserID && obj.IsDeleted == false && obj.IsApplicant == false).ToList();
        }
        #endregion

        #region Manage SysXLineOfBusinessFeatures

        /// <summary>
        /// Retrieves a list of all LineOfBusiness features based on sysXBlockID.
        /// </summary>
        /// <param name="sysXBlockId">The value for sysxblock's Id.</param>
        /// <returns></returns>
        SysXBlocksFeature ISecurityRepository.GetSysXBlockFeature(Int32 sysXBlockId)
        {
            return CompiledGetSysXBlockFeatures(_dbNavigation, sysXBlockId);
        }

        /// <summary>
        /// Retrieves a list of all LineOfBusiness's features.
        /// </summary>
        /// <param name="blockId">  The value for block's id.</param>
        /// <param name="featureId">The value for feature's Id.</param>
        /// <returns>
        /// The system x coordinate block feature.
        /// </returns>
        SysXBlocksFeature ISecurityRepository.GetSysXBlockFeature(Int32 blockId, Int32 featureId)
        {
            return CompiledGetSysXBlockFeature(_dbNavigation, blockId, featureId);
        }

        #endregion

        #region Manage User Role Mapping

        /// <summary>
        /// Performs mapping between user and roles.
        /// </summary>
        /// <param name="userId"> The value for user's Id.</param>
        /// <param name="roleIds">The value for role Ids.</param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        Boolean ISecurityRepository.UserRoleMapping(String userId, List<String> roleIds, Boolean deleteExistingRoles)
        {
            aspnet_Users user = CurrentRepositoryContext.GetAspnetUser(userId);

            while (user.aspnet_Roles.Count > AppConsts.NONE && deleteExistingRoles)
            {
                List<aspnet_Roles> aspnetRoles = user.aspnet_Roles.ToList();
                user.aspnet_Roles.Remove(aspnetRoles[0]);
            }

            foreach (String role in roleIds)
            {
                user.aspnet_Roles.Add(CurrentRepositoryContext.GetRoleDetailById(role).aspnet_Roles);
            }

            base.UpdateObjectEntity(user);

            //===============================
            if (!(user.aspnet_Roles.Count > AppConsts.NONE))
            {
                OrganizationUser organizationUser =
                    _dbNavigation.OrganizationUsers.Where(orgUser => orgUser.UserID == new Guid(userId)).FirstOrDefault();
                organizationUser.SysXBlockID = null;
                base.UpdateObjectEntity(organizationUser);
            }
            //=======================================
            return true;
        }

        #region UAT-3228

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CurrentUserId"></param>
        /// <param name="userId"></param>
        /// <param name="newMappedRoles"></param>
        /// <param name="featureId"></param>
        /// <returns></returns>
        Boolean ISecurityRepository.InsertDefaultColumnConfiguration(Int32 CurrentUserId, String userId, String newMappedRoles)
        {
            EntityConnection connection = _dbNavigation.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("usp_InsertDefaultColumnConfiguration", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RoleID", newMappedRoles);
                cmd.Parameters.AddWithValue("@ApplicantUserId", userId);
                cmd.Parameters.AddWithValue("@CurrentLoggedInUserID", CurrentUserId);
                cmd.ExecuteScalar();
                return true;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CopyFromUserId"></param>
        /// <param name="CopyToOrganizationUserID"></param>
        /// <param name="CurrentLoggedInUserId"></param>
        /// <returns></returns>
        Boolean ISecurityRepository.CopyDefaultColumnConfiguration(Int32 tenantId, Guid CopyFromUserId, Int32 CopyToOrganizationUserID, Int32 CurrentLoggedInUserId)
        {
            Int32 screenColumnId = AppConsts.NONE;
            String screenCode = INTSOF.Utils.Screen.grdManageComplianceSearch.GetStringValue();
            Int32 screenID = _dbNavigation.Screens.Where(con => con.SR_Code == screenCode && !con.SR_IsDeleted).Select(sel => sel.SR_ID).FirstOrDefault();
            if (!screenID.IsNullOrEmpty() && screenID > AppConsts.NONE)
            {
                screenColumnId = _dbNavigation.ScreenColumns.Where(con => con.SC_ScreenID == screenID && con.SC_UniqueName == "NonCompliantCategories").Select(se => se.SC_ID).FirstOrDefault();
            }
            if (screenColumnId > AppConsts.NONE)
            {
                Int32 organizationId = _dbNavigation.Organizations.Where(con => con.TenantID == tenantId && !con.IsDeleted).Select(sel => sel.OrganizationID).FirstOrDefault();

                List<Int32> lstOrganizationUserIds = _dbNavigation.OrganizationUsers.Where(con => con.UserID == CopyFromUserId && !con.IsDeleted && con.OrganizationID == organizationId).Select(sel => sel.OrganizationUserID).ToList();

                if (!lstOrganizationUserIds.IsNullOrEmpty())
                {
                    UserScreenColumnMapping userScreenColumnMapping = _dbNavigation.UserScreenColumnMappings.Where(con => lstOrganizationUserIds.Contains(con.USCM_OrganizationUserID) && !con.USCM_IsDeleted && con.USCM_ScreenColumnsID == screenColumnId).FirstOrDefault();
                    if (!userScreenColumnMapping.IsNullOrEmpty())
                    {
                        _dbNavigation.UserScreenColumnMappings.AddObject(new UserScreenColumnMapping()
                        {
                            USCM_OrganizationUserID = CopyToOrganizationUserID,
                            USCM_ScreenColumnsID = screenColumnId,
                            USCM_IsVisible = userScreenColumnMapping.USCM_IsVisible,
                            USCM_IsDeleted = false,
                            USCM_CreatedOn = DateTime.Now,
                            USCM_CreatedBy = CurrentLoggedInUserId
                        });

                        _dbNavigation.SaveChanges();
                        return true;
                    }
                }
            }
            return false;
        }

        #endregion



        /// <summary>
        /// Performs mapping between agency shared user and roles.
        /// </summary>
        /// <param name="userId"> The value for user's Id.</param>
        /// <param name="roleIds">The value for role Ids.</param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        Boolean ISecurityRepository.SharedUserRoleMapping(String userId, List<String> roleIds)
        {
            aspnet_Users user = CurrentRepositoryContext.GetAspnetUser(userId);
            List<String> sharedUserRoleIDs = new List<String>();
            sharedUserRoleIDs.Add(SharedUserRoleDetails.AgencyUserRole);
            sharedUserRoleIDs.Add(SharedUserRoleDetails.RotationPackageRole);
            sharedUserRoleIDs.Add(SharedUserRoleDetails.AgencyUserAndRotationPackageRole);

            if (user.aspnet_Roles.Count > AppConsts.NONE)
            {
                List<aspnet_Roles> aspnetRoles = user.aspnet_Roles.Where(x => sharedUserRoleIDs.Contains(x.RoleId.ToString().ToUpper())).ToList();
                if (!aspnetRoles.IsNullOrEmpty() && aspnetRoles.Any())
                    user.aspnet_Roles.Remove(aspnetRoles[0]);
            }

            foreach (String role in roleIds)
            {
                user.aspnet_Roles.Add(CurrentRepositoryContext.GetRoleDetailById(role).aspnet_Roles);
            }

            base.UpdateObjectEntity(user);

            //===============================
            if (!(user.aspnet_Roles.Count > AppConsts.NONE))
            {
                OrganizationUser organizationUser =
                    _dbNavigation.OrganizationUsers.Where(orgUser => orgUser.UserID == new Guid(userId)).FirstOrDefault();
                organizationUser.SysXBlockID = null;
                base.UpdateObjectEntity(organizationUser);
            }
            //=======================================
            return true;
        }


        #endregion

        #region Manage Role Permission ProductFeature

        IQueryable<RolePermissionProductFeature> ISecurityRepository.GetProductFeatureRoles(Int32 productFeatureId)
        {
            try
            {
                return CompiledGetProductFeatureRole(_dbNavigation, productFeatureId);
            }
            catch (Exception ex)
            {
                DALUtils.LoggerService.GetLogger().Error("DAL Security Repository GetProductFeatureRoles", ex);
                return null;
            }
        }

        /// <summary>
        /// Retrieves a list of all features associated with the current role.
        /// </summary>
        /// <param name="roleDetailId">The value for role detail's id.</param>
        /// <param name="productId">   The value for product's Id.</param>
        /// <returns>
        /// A <see cref="RolePermissionProductFeature"/> list of data from the underlying data storage.
        /// </returns>
        IQueryable<RolePermissionProductFeature> ISecurityRepository.GetFeatureForRole(String roleDetailId, Int32 productId)
        {
            return CompiledGetFeatureForRole(_dbNavigation, roleDetailId, productId);
        }

        /// <summary>
        /// Performs mapping between roles and features.
        /// </summary>
        /// <param name="roleId">               The Value for roleId.</param>
        /// <param name="productId">            The Value for productId.</param>
        /// <param name="featurePermissionList">The Value for featurePermissionList.</param>
        /// <param name="updatedSysXBlockIds">  The Value for updatedSysXBlockIds.</param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        Boolean ISecurityRepository.RoleFeatureMapping(String roleId, Int32 productId, Dictionary<Int32, Int32> featurePermissionList, List<Int32> updatedSysXBlockIds, List<RoleFeatureActionContract> roleFeatureActions)
        {
            DeleteRoleFeaturesMapping(roleId, updatedSysXBlockIds);

            foreach (KeyValuePair<Int32, Int32> kvp in featurePermissionList)
            {
                RolePermissionProductFeature rolePermissionProductFeature = new RolePermissionProductFeature
                {
                    Permission =
                        CurrentRepositoryContext.
                        GetPermission(kvp.Value)
                };
                // kvp.Value is permissionID
                SysXBlocksFeature blockFeature = CurrentRepositoryContext.GetSysXBlockFeature(kvp.Key); // kvp.Key is featureID
                rolePermissionProductFeature.SysXBlocksFeature = blockFeature;
                rolePermissionProductFeature.RoleDetail = CurrentRepositoryContext.GetRoleDetailById(roleId);
                rolePermissionProductFeature.RoleDetail.TenantProduct = CurrentRepositoryContext.GetProduct(productId);
                rolePermissionProductFeature.CreatedOn = DateTime.Now;
                if (!roleFeatureActions.IsNull())
                {
                    roleFeatureActions.Where(cond => cond.SysXBlockFeatureID == blockFeature.SysXBlockFeatureID).ForEach
                        (action =>
                        {
                            rolePermissionProductFeature.FeatureRoleActions.Add(new FeatureRoleAction { FeatureActionID = action.FeatureActionID, PermissionID = action.PermissionID, CreatedByID = 1, CreatedOn = DateTime.Now });
                        });
                }
                base.AddObjectEntityInTransaction(rolePermissionProductFeature);
            }

            _dbNavigation.SaveChanges();
            return true;
        }

        /// <summary>
        /// Retrieves a list of all products for each tenant based on tenantID.
        /// </summary>
        /// <param name="tenantId">The value for tenant's Id.</param>
        /// <returns>
        /// The products for tenant.
        /// </returns>
        IQueryable<TenantProduct> ISecurityRepository.GetProductsForTenant(Int32 tenantId)
        {
            return CompiledGetProductsForTenant(_dbNavigation, tenantId);
        }

        /// <summary>
        /// Retrieves all the relations from RolePermissionProductFeature table.
        /// </summary>
        /// <returns></returns>
        IQueryable<RolePermissionProductFeature> ISecurityRepository.GetRolePermissionProductFeatures()
        {
            return _dbNavigation.RolePermissionProductFeatures;
        }

        #endregion

        #region Manage Policies

        /// <summary>
        /// Retrieves a list of all policy register controls based on role id.
        /// </summary>
        /// <param name="currentRoleId">The value for current role's Id.</param>
        /// <returns>
        /// A <see cref="PolicyRegisterUserControl"/> list of data from the underlying data storage.
        /// </returns>
        IQueryable<PolicyRegisterUserControl> ISecurityRepository.GetPolicyRegisterControls(String currentRoleId)
        {
            Guid roleId = new Guid(currentRoleId);
            ObjectSet<ProductFeature> featureList = _dbNavigation.ProductFeatures;
            ObjectSet<PolicyRegisterUserControl> policyRegisterUserControlList = _dbNavigation.PolicyRegisterUserControls;
            ObjectSet<SysXBlocksFeature> sysXBlocksFeatureList = _dbNavigation.SysXBlocksFeatures;
            ObjectSet<RolePermissionProductFeature> rolePermProdFeatureList = _dbNavigation.RolePermissionProductFeatures;

            // Violates the checkList no. this , because it throws an error(To resolve this error The method 'Include' is only supported by LINQ to Entities when the argument is a string constant.)
            var query = (from regControl1 in policyRegisterUserControlList
                         from regControl2 in
                             (
                                 from regControl in policyRegisterUserControlList.Include("PolicyRegisterUserControl2")
                                 join fList in featureList on regControl.ControlPath.Replace("\\\\", "\\") equals (fList.NavigationURL.Substring(2, fList.NavigationURL.IndexOf("Default.aspx") - 2).Replace("/", "\\") + fList.UIControlID)
                                 join blockFeatureList in sysXBlocksFeatureList on fList.ProductFeatureID equals blockFeatureList.FeatureID
                                 join rpfList in rolePermProdFeatureList on blockFeatureList.SysXBlockFeatureID equals rpfList.SysXBlockFeatureId
                                 where fList.NavigationURL.IndexOf("Default.aspx") > 0 && rpfList.RoleId.Equals(roleId)
                                 select regControl
                             )
                         where regControl1.ParentUserControlID == regControl2.RegisterUserControlID || regControl1.RegisterUserControlID == regControl2.RegisterUserControlID
                         select regControl1).Distinct();

            return query;
        }

        /// <summary>
        /// Retrieves a list of all policy register controls.
        /// </summary>
        /// <returns>
        /// A <see cref="PolicyRegisterUserControl"/> list of data from the underlying data storage.
        /// </returns>
        IQueryable<PolicyRegisterUserControl> ISecurityRepository.GetPolicyRegisterControls()
        {
            return CompiledGetPolicyRegisterControls(_dbNavigation);
        }

        /// <summary>
        /// Retrieves the value for selected control.
        /// </summary>
        /// <param name="roleId">               The value for role's Id.</param>
        /// <param name="registerUserControlId">The value for register User ControlId.</param>
        /// <returns>
        /// The control selected values.
        /// </returns>
        PolicySetUserControl ISecurityRepository.GetControlSelectedValues(String roleId, Int32 registerUserControlId)
        {
            return CompiledGetControlSelectedValues(_dbNavigation, roleId, registerUserControlId);
        }

        /// <summary>
        /// Retrieves the set of policies based on role's id.
        /// </summary>
        /// <param name="roleId">The vlaue for role's Id.</param>
        /// <returns>
        /// The policy set by role identifier.
        /// </returns>
        PolicySet ISecurityRepository.GetPolicySetByRoleId(String roleId)
        {
            return CompiledGetPolicySetByRoleId(_dbNavigation, roleId);
        }

        /// <summary>
        /// Retrieves the list of all policy controls.
        /// </summary>
        /// <returns>
        /// A <see cref="PolicyControl"/> list of data from the underlying data storage.
        /// </returns>
        IQueryable<PolicyControl> ISecurityRepository.GetRegisteredControlList()
        {
            return CompiledGetRegisteredControlList(_dbNavigation);
        }

        /// <summary>
        /// Performs an update operation for policy based on policySet.
        /// </summary>
        /// <param name="policySet">The value for policy set.</param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        Boolean ISecurityRepository.SavePolicies(PolicySet policySet)
        {

            var data = policySet.PolicySetUserControls.ToList();
            Guid roleId = new Guid(policySet.RoleID.ToString());

            if (!_dbNavigation.PolicySets.Include(SysXEntityConstants.TABLE_POLICY_SET_USER_CONTROLS).Where(policySets => policySets.RoleID == roleId).FirstOrDefault().IsNull())
            {
                var policySetValue = _dbNavigation.PolicySets.Include(SysXEntityConstants.TABLE_POLICY_SET_USER_CONTROLS).Where(policySets => policySets.RoleID == roleId).FirstOrDefault();
                if (!policySetValue.IsNull() && policySetValue.PolicySetUserControls.Count > AppConsts.NONE)
                {
                    for (Int32 counter = AppConsts.NONE; counter < data.Count; counter++)
                    {
                        long registerUserControlId = data[counter].RegisterUserControlID;
                        Int32 policySetId = data[counter].PolicySetID;
                        var puList = _dbNavigation.PolicySetUserControls.Include(SysXEntityConstants.TABLE_POLICY_SET)
                            .Where(ps => ps.PolicySet.RoleID == roleId && ps.PolicySetID == policySetId
                                         && ps.RegisterUserControlID == registerUserControlId).ToList();

                        foreach (PolicySetUserControl policySetUserControl in puList)
                        {
                            _dbNavigation.PolicySetUserControls.DeleteObject(policySetUserControl);
                        }
                    }

                    _dbNavigation.SaveChanges();
                }

                base.UpdateObjectEntity(policySet);
            }
            else
            {
                base.AddObjectEntity(policySet);
            }
            return true;

        }

        /// <summary>
        /// Performs an update operation for policy based on policySetUserControlList and roleID.
        /// </summary>
        /// <param name="policySetUserControlList">The value for policySet UserControl List.</param>
        /// <param name="currentRoleId">           The value for current role's Id.</param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        Boolean ISecurityRepository.SavePolicies(EntityCollection<PolicySetUserControl> policySetUserControlList, String currentRoleId)
        {
            Guid roleId = new Guid(currentRoleId);
            var data = policySetUserControlList.ToList();

            if (!_dbNavigation.PolicySets.Include(SysXEntityConstants.TABLE_POLICY_SET_USER_CONTROLS).Where(policySets => policySets.RoleID == roleId).FirstOrDefault().IsNull())
            {
                var policySetsInfo = _dbNavigation.PolicySets.Include(SysXEntityConstants.TABLE_POLICY_SET_USER_CONTROLS).Where(policySets => policySets.RoleID == roleId).FirstOrDefault();
                if (policySetsInfo != null && policySetsInfo.PolicySetUserControls.Count > AppConsts.NONE)
                {
                    var policySetsPolicySetUserControls = _dbNavigation.PolicySets.Include(SysXEntityConstants.TABLE_POLICY_SET_USER_CONTROLS).Where(ps => ps.RoleID == roleId).ToList();
                    for (Int32 counter = AppConsts.NONE; counter < policySetsPolicySetUserControls.Count; counter++)
                    {
                        var registerUserControlId = data[counter].RegisterUserControlID;
                        var policySetId = policySetsPolicySetUserControls[counter].PolicySetID;
                        var puList = _dbNavigation.PolicySetUserControls.Include(SysXEntityConstants.TABLE_POLICY_SET)
                            .Where(ps => ps.PolicySet.RoleID == roleId && ps.PolicySetID == policySetId
                                         && ps.RegisterUserControlID == registerUserControlId).ToList();

                        foreach (PolicySetUserControl policySetUserControl in puList)
                        {
                            _dbNavigation.PolicySetUserControls.DeleteObject(policySetUserControl);
                        }
                    }

                    _dbNavigation.SaveChanges();
                }

                PolicySet policySet = CurrentRepositoryContext.GetPolicySetByRoleId(currentRoleId);
                policySet.PolicySetName = currentRoleId;
                policySet.RoleID = roleId;

                var data2 = CurrentRepositoryContext.SetNewPolicySetUserControlForSave(data).ToList();

                foreach (PolicySetUserControl policySetUserControl in data2.ToList().Where(policySetUserControl => policySetUserControl.Policies.Count > AppConsts.NONE))
                {
                    policySet.PolicySetUserControls.Add(policySetUserControl);

                }

                base.UpdateObjectEntity(policySet);
            }
            else
            {
                PolicySet policySet = new PolicySet { PolicySetName = currentRoleId, RoleID = roleId };

                var data2 = CurrentRepositoryContext.SetNewPolicySetUserControlForSave(data).ToList();

                foreach (PolicySetUserControl policySetUserControl in data2.Where(policySetUserControl => policySetUserControl.Policies.Count > AppConsts.NONE))
                {
                    policySet.PolicySetUserControls.Add(policySetUserControl);
                }
                base.AddObjectEntity(policySet);
            }
            return true;
        }

        /// <summary>
        /// Retrieves a list of all policies.
        /// </summary>
        /// <param name="ucName">       The value for ucName.</param>
        /// <param name="roleList">     The value for roleList.</param>
        /// <param name="sysXAdminList">The value for sysXAdminList.</param>
        /// <param name="currentUserId">The value for currentUserId.</param>
        /// <returns>
        /// all policies.
        /// </returns>
        PolicySetUserControl ISecurityRepository.GetAllPolicies(String ucName, List<aspnet_Roles> roleList, List<Int32> sysXAdminList, Int32 currentUserId)
        {
            PolicySetUserControl controlPolicy = new PolicySetUserControl();
            List<Policy> policies = new List<Policy>();
            foreach (aspnet_Roles role in roleList)
            {
                CurrentRepositoryContext.GetAllControlPolicies(policies, sysXAdminList, ucName, role.RoleId.ToString(), currentUserId);

                foreach (Policy policy in policies)
                {
                    controlPolicy.Policies.Add(policy);
                }
            }
            return controlPolicy;


        }

        /// <summary>
        /// Retrieves the sub parent of all policies.
        /// </summary>
        /// <param name="ucName">       The value for user control name.</param>
        /// <param name="roleId">       The value for role's Id.</param>
        /// <param name="sysXAdminList">The value for sysXAdminList.</param>
        /// <param name="currentUserId">The value for current user's Id.</param>
        /// <returns>
        /// all policies sub parent.
        /// </returns>
        PolicySetUserControl ISecurityRepository.GetAllPoliciesSubParent(String ucName, String roleId, List<Int32> sysXAdminList, Int32 currentUserId)
        {
            PolicySetUserControl controlPolicy = new PolicySetUserControl();
            List<Policy> policies = new List<Policy>();
            GetAllControlPoliciesSubParent(policies, sysXAdminList, ucName, roleId, currentUserId);

            foreach (Policy policy in policies)
            {
                controlPolicy.Policies.Add(policy);
            }
            return controlPolicy;
        }

        /// <summary>
        /// Enumerates set new policy set user control for save in this collection.
        /// </summary>
        /// <param name="policySetUserControlList">The policy set user control list.</param>
        /// <returns>
        /// An enumerator that allows foreach to be used to process set new policy set user control for
        /// save in this collection.
        /// </returns>
        IEnumerable<PolicySetUserControl> ISecurityRepository.SetNewPolicySetUserControlForSave(IEnumerable<PolicySetUserControl> policySetUserControlList)
        {
            EntityCollection<PolicySetUserControl> policySetUserControlNewList = new EntityCollection<PolicySetUserControl>();
            Boolean isPolicyApply = false;

            foreach (PolicySetUserControl item in policySetUserControlList.ToList())
            {
                PolicySetUserControl controlPol = new PolicySetUserControl();

                foreach (var policies in item.Policies.ToList())
                {
                    Boolean flag = false;
                    isPolicyApply = true;
                    Policy policy = new Policy { ControlID = policies.ControlID, ControlType = policies.ControlType, CreatedOn = DateTime.Now };

                    foreach (var policyProperties in policies.PolicyProperties.ToList().Where(policyProperties => policyProperties.PolicyValue))
                    {
                        flag = true;
                        PolicyProperty property = new PolicyProperty
                        {
                            PolicyValue = policyProperties.PolicyValue,
                            PolicyPropertyName = policyProperties.PolicyPropertyName
                        };
                        property.PolicyValue = true;
                        policy.PolicyProperties.Add(property);
                    }
                    if (flag)
                    {
                        controlPol.Policies.Add(policy);
                    }
                }
                if (isPolicyApply)
                {
                    controlPol.RegisterUserControlID = item.RegisterUserControlID;
                    policySetUserControlNewList.Add(controlPol);
                }
            }
            return policySetUserControlNewList;
        }

        /// <summary>
        /// Gets all control policies.
        /// </summary>
        /// <param name="policies">     The policies.</param>
        /// <param name="sysXAdminList">The sys X admin list.</param>
        /// <param name="ucName">       Name of the user control.</param>
        /// <param name="currentRoleId">The current role id.</param>
        /// <param name="currentUserId">The current user id.</param>
        void ISecurityRepository.GetAllControlPolicies(List<Policy> policies, List<Int32> sysXAdminList, String ucName, String currentRoleId, Int32 currentUserId)
        {
            Guid gRoleId = new Guid(currentRoleId);
            List<Policy> localPolicy = new List<Policy>();
            //Used another context
            SysXAppDBEntities policyContext = new SysXAppDBEntities();
            var controlPolicy = policyContext.PolicySetUserControls.Include(SysXEntityConstants.TABLE_POLICY_SET)
                                                                    .Include(SysXEntityConstants.TABLE_POLICIES)
                                                                    .Include(SysXEntityConstants.TABLE_POLICIES_DOT_POLICY_PROPERTIES)
                                                                    .Include(SysXEntityConstants.TABLE_POLICY_REGISTER_USER_CONTROL).Where(policy => policy.PolicyRegisterUserControl.ControlPath.Replace("\\\\", "\\").Equals(ucName) && policy.PolicySet.RoleID == gRoleId).FirstOrDefault();

            if (!controlPolicy.IsNull())
            {
                localPolicy.AddRange(controlPolicy.Policies.ToList());
                policies.AddRange(localPolicy);
                policyContext.Detach(controlPolicy);

                //Call recursively till reach top 
                if (sysXAdminList.Contains(currentUserId).Equals(false))
                {
                    OrganizationUser user = CurrentRepositoryContext.GetOrganizationUser(currentUserId);
                    if (!user.IsNull())
                    {
                        user = CurrentRepositoryContext.GetOrganizationUser(user.CreatedByID);
                        List<aspnet_Roles> roles = CurrentRepositoryContext.GetUserRolesById(user.UserID.ToString());
                        foreach (aspnet_Roles role in roles)
                        {
                            CurrentRepositoryContext.GetAllControlPolicies(policies, sysXAdminList, ucName, role.RoleId.ToString(), user.CreatedByID);
                        }
                    }
                }
            }
            else
            {
                if (sysXAdminList.Contains(currentUserId).Equals(false))
                {
                    OrganizationUser user = CurrentRepositoryContext.GetOrganizationUser(currentUserId);
                    if (!user.IsNull())
                    {
                        user = CurrentRepositoryContext.GetOrganizationUser(user.CreatedByID);
                        if (user.IsNull()) return;

                        List<aspnet_Roles> roles = CurrentRepositoryContext.GetUserRolesById(user.UserID.ToString());
                        foreach (aspnet_Roles role in roles)
                        {
                            CurrentRepositoryContext.GetAllControlPolicies(policies, sysXAdminList, ucName, role.RoleId.ToString(), user.CreatedByID);
                        }
                    }
                }
            }

        }

        #endregion

        #region Manage Policy Register Control

        /// <summary>
        /// Retrieves policy register control.
        /// </summary>
        /// <param name="policyRegisterControlId">The value for policyRegisterControlId.</param>
        /// <returns>
        /// The policy register control.
        /// </returns>
        PolicyRegisterUserControl ISecurityRepository.GetPolicyRegisterControl(Int32 policyRegisterControlId)
        {
            return CompiledGetPolicyRegisterControl(_dbNavigation, policyRegisterControlId);
        }

        /// <summary>
        /// Chooses the child policy control.
        /// </summary>
        /// <param name="registerUserControlId">The value for registerUserControlId.</param>
        Int32 ISecurityRepository.SelectChildPolicyControls(long registerUserControlId)
        {
            return CompiledSelectChildPolicyControls(_dbNavigation, registerUserControlId);
        }

        /// <summary>
        /// Query if 'enteredControlName' is exists.
        /// </summary>
        /// <param name="enteredControlName"> Name of the entered Control.</param>
        /// <param name="existingControlName">Name of the existing Control.</param>
        /// <returns>
        /// true if Policy Register Control exists, false if not.
        /// </returns>
        Boolean ISecurityRepository.IsControlNameExists(String enteredControlName, String existingControlName = null)
        {
            // executes when the form is in insert mode.
            if (String.IsNullOrEmpty(existingControlName))
            {
                return CompiledGetPolicyRegisterControls(_dbNavigation).Where(condition => condition.ControlName.ToLower() == enteredControlName.ToLower()).Count() > AppConsts.NONE;
            }
            // executes when the form is in update mode.
            else
            {
                return CompiledGetPolicyRegisterControls(_dbNavigation).ToList().FindAll(condition => condition.ControlName.Equals(enteredControlName, StringComparison.InvariantCultureIgnoreCase))
                    .SkipWhile(condition => existingControlName.Equals(enteredControlName, StringComparison.InvariantCultureIgnoreCase))
                    .Any(nameChecks => nameChecks.ControlName.Equals(enteredControlName, StringComparison.InvariantCultureIgnoreCase));
            }
        }

        #endregion

        #region Manage Aspnet Roles

        /// <summary>
        /// Get the Aspnet Role based on RoleId.
        /// </summary>
        /// <param name="roleId">String.</param>
        /// <returns>
        /// aspnet_Roles.
        /// </returns>
        aspnet_Roles ISecurityRepository.GetAspnetRole(String roleId)
        {
            aspnet_Roles aspnetRole = CompiledGetAspnetRole(_dbNavigation, roleId);
            return !aspnetRole.IsNull() ? aspnetRole : null;
        }

        #endregion

        #region Manage FeaturePermissions

        /// <summary>
        /// Retrieves all feature permissions.
        /// </summary>
        /// <param name="featurePermissionId">.</param>
        /// <returns>
        /// The feature permission.
        /// </returns>
        FeaturePermission ISecurityRepository.GetFeaturePermission(Int32 featurePermissionId)
        {
            return CompiledGetFeaturePermission(_dbNavigation, featurePermissionId);
        }

        #endregion

        #region Manage Departments

        /// <summary>
        /// Retrieves all departments for Super Admin.
        /// </summary>
        /// <returns>
        /// A <see cref="Organization"/> list of data from the underlying data storage.
        /// </returns>
        IQueryable<Organization> ISecurityRepository.GetDepartmentsForSuperAdmin()
        {
            IQueryable<Organization> departments = CompiledGetDepartmentsForSuperAdmin(_dbNavigation);
            return !departments.IsNull() ? departments : null;
        }

        /// <summary>
        /// Retrieves a list of all departments for Product admin.
        /// </summary>
        /// <param name="productId">.</param>
        /// <returns>
        /// A <see cref="Organization"/> list of data from the underlying data storage.
        /// </returns>
        IQueryable<Organization> ISecurityRepository.GetDepartmentsForProductAdmin(Int32 productId)
        {
            Int32 tenantProductTenantId = Convert.ToInt32(CompiledGetTenantProductInformation(_dbNavigation, productId).TenantID);
            IQueryable<Organization> departments = CompiledGetDepartmentsForProductAdmin(_dbNavigation, tenantProductTenantId);
            return !departments.IsNull() ? departments : null;
        }

        /// <summary>
        /// Retrieves a list of all departments for Product admin.
        /// </summary>
        /// <param name="productId">    The product id.</param>
        /// <param name="currentUserId">The current user id.</param>
        /// <returns>
        /// A <see cref="Organization"/> list of data from the underlying data storage.
        /// </returns>
        IQueryable<Organization> ISecurityRepository.GetDepartmentsForDepartmentAdmin(Int32 productId, Int32 currentUserId)
        {
            Int32 tenantProductTenantId = Convert.ToInt32(CompiledGetTenantProductInformation(_dbNavigation, productId).TenantID);
            IQueryable<Organization> departments = CompiledGetDepartmentsForDepartmentAdmin(_dbNavigation, tenantProductTenantId, currentUserId);
            return !departments.IsNull() ? departments : null;
        }

        ///// <summary>
        ///// Method to check department mapping
        ///// </summary>
        ///// <param name="organizationId">organizationId</param>
        ///// <returns></returns>
        //public Boolean IsDepartmentMapped(Int32 organizationId)
        //{
        //    return _dbNavigation.DeptProgramMappings.Where(cond => cond.DPM_OrganizationID == organizationId && !cond.DPM_IsDeleted).Count() > AppConsts.NONE;
        //}

        #endregion

        #region Manage User Themes

        /// <summary>
        /// Updates the theme selected by Logged-in User to db.
        /// </summary>
        /// <param name="aspnetMembership"></param>
        /// <returns>
        /// Updates the aspnet_membership table.
        /// </returns>
        Boolean ISecurityRepository.UpdateAspnetMembershipForTheme(aspnet_Membership aspnetMembership)
        {
            this.UpdateObjectEntity(aspnetMembership);
            return true;
        }

        /// <summary>
        /// Gets the theme for the User.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>
        /// Logged-in UserId.
        /// </returns>
        aspnet_Membership ISecurityRepository.GetAspnetMembershipById(Guid userId)
        {
            return CompiledGetAspnetMembership.Invoke(_dbNavigation, userId);
        }

        #endregion

        #region UserGroup

        /// <summary>
        /// Get all user group collection
        /// </summary>
        /// <param name="IsAdmin">Boolean</param>
        /// <param name="CreatedById">Int32</param>
        /// <returns>IQueryable</returns>
        List<UserGroup> ISecurityRepository.GetAllUserGroup(Boolean IsAdmin, Int32 CreatedById)
        {

            IQueryable<UserGroup> usergroup = CompiledGetAllUserGroup(_dbNavigation);
            List<OrganizationUser> users = CurrentRepositoryContext.GetMappedOrganizationUsers(IsAdmin, CreatedById);
            users.Add(CompiledCurrentUserDetails.Invoke(_dbNavigation, CreatedById).FirstOrDefault());
            List<Int32> OrganizationUserIds = users.Select(con => con.OrganizationUserID).ToList();
            if (!IsAdmin)
            {
                IEnumerable<UserGroup> usergroups = from c in usergroup
                                                    where OrganizationUserIds.Contains(c.CreatedByID)
                                                    select c;
                return usergroups.ToList();
            }
            return usergroup.ToList();

        }

        /// <summary>
        /// Get all users in a group
        /// </summary>
        /// <param name="userGroupId">Int32 UserGroupId</param>
        /// <returns>IQueryable</returns>
        IQueryable<UsersInUserGroup> ISecurityRepository.GetAllUsersInUserGroup(Int32 userGroupId)
        {
            return CompiledGetAllUserInAGroup(_dbNavigation, userGroupId);
        }

        /// <summary>
        /// Map user group role
        /// </summary>
        /// <param name="userGroupId"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        Boolean ISecurityRepository.UserGroupRoleMapping(Int32 userGroupId, List<String> roleIds)
        {
            var rolesInuserGroup = _dbNavigation.RolesInUserGroups.Where(condtion => condtion.UserGroupID == userGroupId);
            var assignedRole = CompiledUserGroupAssignedRole(_dbNavigation, userGroupId);
            var TodeleteRole = rolesInuserGroup.Except(assignedRole);
            List<Guid> TempuseresRoleIds = assignedRole.Select(condtion => condtion.RoleID).ToList();
            List<String> useresRoleIds = TempuseresRoleIds.ConvertAll<String>(delegate (Guid id) { return id.ToString(); });
            List<String> NewRoleIds = roleIds.Except(useresRoleIds).ToList();
            foreach (var rolesinusergroup in TodeleteRole)
            {
                _dbNavigation.RolesInUserGroups.DeleteObject(rolesinusergroup);
            }

            _dbNavigation.SaveChanges();

            foreach (String RoleId in NewRoleIds)
            {
                base.AddObjectEntity(new RolesInUserGroup
                {
                    RoleID = Guid.Parse(RoleId),
                    UserGroupID = userGroupId
                }
                );
            }
            return true;
        }

        /// <summary>
        /// Get all role of user group.
        /// </summary>
        /// <param name="UserGroupId"></param>
        /// <returns></returns>
        List<aspnet_Roles> ISecurityRepository.GetAllRoleOfUserGroup(Int32 UserGroupId)
        {
            IQueryable<aspnet_Roles> roles = CompiledGetUserGroupRoles(_dbNavigation, UserGroupId);
            foreach (aspnet_Roles role in roles)
            {
                // Explicitly load the customer for each order.
                _dbNavigation.LoadProperty(role, "RoleDetail");
            }
            return roles.ToList();
        }

        /// <summary>
        /// Map user group role
        /// </summary>
        /// <param name="userGroupId"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        Boolean ISecurityRepository.UserGroupsUserRoleMapping(Int32 UserGroupId, String UserId, List<String> RoleInUserGroupIds)
        {
            Guid RoleId;
            Guid userid = Guid.Parse(UserId);
            IQueryable<UserGroupRolePermissionProductFeature> rolesInuserGroup = CompiledGetUserGroupRolePermissionProductFeature(_dbNavigation, UserGroupId, userid);
            Int32 userInUserGroupId = CompiledGetAllUserInAGroup(_dbNavigation, UserGroupId).Where(condition => condition.UserID == userid).FirstOrDefault().UsersInUserGroupID;
            foreach (var rolesinusergroup in rolesInuserGroup)
            {
                _dbNavigation.UserGroupRolePermissionProductFeatures.DeleteObject(rolesinusergroup);
            }

            _dbNavigation.SaveChanges();
            foreach (String roleusergroupId in RoleInUserGroupIds)
            {
                RoleId = Guid.Parse(roleusergroupId);
                base.AddObjectEntity(new UserGroupRolePermissionProductFeature
                {
                    UsersInUserGroupID = userInUserGroupId,
                    RolesInUserGroupID = _dbNavigation.RolesInUserGroups.Where(condition => condition.UserGroupID == UserGroupId && condition.RoleID == RoleId).FirstOrDefault().RolesInUserGroupID
                }
                );
            }
            return true;
        }
        List<aspnet_Roles> ISecurityRepository.GetAllRoleOfUserGroupUser(String UserId)
        {
            IQueryable<aspnet_Roles> roles = CompiledGetUserRoleInUserGroup(_dbNavigation, Guid.Parse(UserId));
            foreach (aspnet_Roles role in roles)
            {
                // Explicitly load the customer for each order.
                _dbNavigation.LoadProperty(role, "RoleDetail");
            }
            return roles.ToList();

        }

        List<UserGroupRolePermissionProductFeature> ISecurityRepository.getUserGroupRolePermissionProductFeature(Guid RoleId, Int32 UserGroupId)
        {
            return CompiledGetUserGroupRolePermissionProductFeatureByRoleId(_dbNavigation, RoleId, UserGroupId).ToList();
        }

        #endregion

        #region City

        //Used By Location Control
        /// <summary>
        ///  Return ZipCode by zipCodeNumber
        /// </summary>
        /// <param name="zipCodeNumber"></param>
        /// <returns></returns>
        List<ZipCode> ISecurityRepository.GetCityState(String zipCodeNumber)
        {
            List<ZipCode> zipCode = CompiledGetCityState.Invoke(_dbNavigation, zipCodeNumber).ToList();
            return zipCode;
        }

        //Used By Location Control
        /// <summary>
        ///  Return ZipCode by zipCodeNumber
        /// </summary>
        /// <param name="zipCodeNumber"></param>
        /// <returns></returns>
        List<ZipCode> ISecurityRepository.GetCityState(Int32 zipCodeId)
        {
            List<ZipCode> zipCode = CompiledGetCityStateByID.Invoke(_dbNavigation, zipCodeId).ToList();
            return zipCode;
        }

        /// <summary>
        /// Get applicant region information based on the selection. Used in package purchase by applicant
        /// </summary>
        /// <param name="zipCodeId"></param>
        /// <returns></returns>
        public ZipCode GetApplicantZipCodeDetails(Int32 zipCodeId)
        {
            return _dbNavigation.ZipCodes.Include("City").Include("County").Where(zipCode => zipCode.ZipCodeID == zipCodeId).FirstOrDefault();
        }


        #endregion

        #region Manage Organization User Location

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationUserId"></param>
        /// <param name="organizationLocationId"></param>
        /// <returns></returns>
        Boolean ISecurityRepository.AddOrganizationUserLocation(Int32 organizationUserId, Int32 organizationLocationId)
        {
            _dbNavigation.OrganizationUserLocations.AddObject(new OrganizationUserLocation()
            {
                OrganizationUserID = organizationUserId,
                OrganizationLocationID = organizationLocationId

            });

            _dbNavigation.SaveChanges();
            return true;
        }

        #endregion

        #region Manage Organization User Program

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="organizationUserId"></param>
        ///// <param name="programStudyId"></param>
        ///// <returns></returns>
        //Boolean ISecurityRepository.AddOrganizationUserProgram(Int32 organizationUserId, Int32 programStudyId)
        //{
        //    _dbNavigation.OrganizationUserPrograms.AddObject(new OrganizationUserProgram()
        //    {
        //        OrganizationUserID = organizationUserId,
        //        //ProgramStudyID = programStudyId

        //    });
        //    _dbNavigation.SaveChanges();
        //    return true;
        //}
        #endregion

        #region User Authorization Request

        public void AddUserAuthRequest(UserAuthRequest userAuthRequest, Int32 currentUserId)
        {
            userAuthRequest.UAR_IsDeleted = false;
            userAuthRequest.UAR_CreatedByID = currentUserId;
            userAuthRequest.UAR_CreatedOn = DateTime.Now;
            _dbNavigation.AddToUserAuthRequests(userAuthRequest);
            _dbNavigation.SaveChanges();
        }

        public void UpdateUserAuthRequest(UserAuthRequest userAuthRequest, Int32 currentUserId)
        {
            userAuthRequest.UAR_ModifiedByID = currentUserId;
            userAuthRequest.UAR_ModifiedOn = DateTime.Now;
            _dbNavigation.SaveChanges();
        }

        public void CancelPreviousAuthRequest(Int32 orgUserId, Int16 reqTypeId, Int32 loggedInUserId)
        {
            List<UserAuthRequest> lstUserAuthRequest = _dbNavigation.UserAuthRequests.Where(obj => obj.UAR_OrganizationUserID == orgUserId &&
                obj.UAR_AuthRequestTypeID == reqTypeId && obj.UAR_IsDeleted == false && obj.UAR_IsActive == true).ToList();
            foreach (var userAuthRequest in lstUserAuthRequest)
            {
                userAuthRequest.UAR_IsDeleted = true;
                userAuthRequest.UAR_ModifiedByID = loggedInUserId;
                userAuthRequest.UAR_ModifiedOn = DateTime.Now;
            }
            _dbNavigation.SaveChanges();
        }

        public Int16 GetAuthRequestTypeIdByCode(String code)
        {
            return _dbNavigation.lkpAuthRequestTypes.FirstOrDefault(obj => obj.Code.Equals(code) && obj.IsDeleted == false).AuthRequestTypeID;
        }

        public UserAuthRequest GetUserAuthRequestByVerCode(String verificationCode)
        {
            return _dbNavigation.UserAuthRequests.FirstOrDefault(uar => uar.UAR_VerificationCode.Equals(verificationCode)
                && uar.UAR_IsDeleted == false && uar.UAR_IsActive == true);
        }

        public UserAuthRequest GetUserAuthRequestByEmail(String emailAddress)
        {
            return _dbNavigation.UserAuthRequests.FirstOrDefault(uar => uar.UAR_NewValue.ToLower().Equals(emailAddress.ToLower())
                && uar.lkpAuthRequestType.Code.Equals("AA") && uar.UAR_IsDeleted == false && uar.UAR_IsActive == true && uar.UAR_SecurityEmailUpdateChecked == true);
        }
        #endregion


        #endregion

        #region Private Methods

        private void DeleteRelatedEnityOfProductFeature(ProductFeature productFeature)
        {
            productFeature.SysXBlocksFeatures.Load();
            foreach (SysXBlocksFeature feature in productFeature.SysXBlocksFeatures.ToArray())
            {
                if (!feature.RolePermissionProductFeatures.IsLoaded)
                    feature.RolePermissionProductFeatures.Load();
                foreach (RolePermissionProductFeature rolepermissionProductFeature in feature.RolePermissionProductFeatures.ToArray())
                {
                    _dbNavigation.RolePermissionProductFeatures.DeleteObject(rolepermissionProductFeature);
                }
                if (!feature.TenantProductFeatures.IsLoaded)
                    feature.TenantProductFeatures.Load();
                foreach (TenantProductFeature tenantfeature in feature.TenantProductFeatures.ToArray())
                {
                    if (!tenantfeature.FeaturePermissions.IsLoaded)
                        tenantfeature.FeaturePermissions.Load();
                    foreach (FeaturePermission permission in tenantfeature.FeaturePermissions.ToArray())
                    {
                        _dbNavigation.FeaturePermissions.DeleteObject(permission);
                    }

                    _dbNavigation.TenantProductFeatures.DeleteObject(tenantfeature);
                }
                _dbNavigation.SysXBlocksFeatures.DeleteObject(feature);
            }

            List<Int64> featureActionsForFeature = _dbNavigation.FeatureActions.Where(cond => cond.ProductFeatureID == productFeature.ProductFeatureID).Select(col => col.FeatureActionID).ToList();
            featureActionsForFeature.ForEach(action =>
                {
                    FeatureAction fAction = _dbNavigation.FeatureActions.Include("FeatureRoleActions").FirstOrDefault(cond => cond.FeatureActionID == action);
                    List<Int32> featureRoleActions = fAction.FeatureRoleActions.Select(col => col.FeatureRoleActionID).ToList();
                    featureRoleActions.ForEach(frAction =>
                        {
                            _dbNavigation.DeleteObject(_dbNavigation.FeatureRoleActions.FirstOrDefault(cond => cond.FeatureRoleActionID == frAction));
                        });
                    _dbNavigation.DeleteObject(fAction);
                });
        }

        private Boolean DeleteFromRolePermissionProductFeature(IEnumerable<RolePermissionProductFeature> rolePermissionProductFeatures)
        {
            foreach (var item in rolePermissionProductFeatures)
            {
                base.DeleteObjectEntityInTransaction(item, true);
            }

            return true;
        }

        private void AddLineOfBusinessFeatures(lkpSysXBlock block, ProductFeature feature, Int32 createdById)
        {
            SysXBlocksFeature sysXBlocksFeature = new SysXBlocksFeature
            {
                ProductFeature = feature,
                lkpSysXBlock = block,
                CreatedOn = DateTime.Now,
                IsActive = true,
                CreatedByID = createdById
            };

            base.AddObjectEntityInTransaction(sysXBlocksFeature);

            if (!feature.ProductFeature2.IsNull())
            {
                if (CurrentRepositoryContext.CheckSysXBlockFeature(block.SysXBlockId, feature.ProductFeature2.ProductFeatureID).IsNull())
                {
                    AddLineOfBusinessFeatures(block, CurrentRepositoryContext.GetProductFeature(feature.ProductFeature2.ProductFeatureID), createdById);
                }
            }

            return;
        }

        private void DeleteExistingMappedRoles(Guid userId)
        {
            var userInRole = _dbNavigation.vw_aspnet_UsersInRoles.Where(vwaspnetUsersInRoles => vwaspnetUsersInRoles.UserId == userId);

            if (!userInRole.IsNull())
            {
                List<vw_aspnet_UsersInRoles> objuserList = userInRole.ToList();

                foreach (vw_aspnet_UsersInRoles userRole in objuserList)
                {
                    base.DeleteObjectEntityInTransaction(userRole);
                }
            }

            return;
        }

        private void GetAllControlPoliciesSubParent(List<Policy> policies, List<Int32> sysXAdminList, String ucName, String currentRoleId, Int32 currentUserId)
        {
            if (!currentRoleId.IsNullOrEmpty())
            {
                Guid gRoleId = new Guid(currentRoleId);
                List<Policy> localPolicy = new List<Policy>();
                var controlPolicy = _dbNavigation.PolicySetUserControls.Include(SysXEntityConstants.TABLE_POLICY_SET)
                                                                        .Include(SysXEntityConstants.TABLE_POLICIES)
                                                                        .Include(SysXEntityConstants.TABLE_POLICIES_DOT_POLICY_PROPERTIES)
                                                                        .Include(SysXEntityConstants.TABLE_POLICY_REGISTER_USER_CONTROL).Where(policy => policy.PolicyRegisterUserControl.ControlPath.Replace("\\\\", "\\").Equals(ucName) && policy.PolicySet.RoleID == gRoleId).FirstOrDefault();

                if (!controlPolicy.IsNull())
                {
                    localPolicy.AddRange(controlPolicy.Policies.ToList());
                    policies.AddRange(localPolicy);
                    _dbNavigation.Detach(controlPolicy);

                    //Call recursively till reach top 
                    if (sysXAdminList.Contains(currentUserId).Equals(false))
                    {
                        OrganizationUser user = CurrentRepositoryContext.GetOrganizationUser(currentUserId);

                        if (!user.IsNull())
                        {
                            List<aspnet_Roles> roles = CurrentRepositoryContext.GetUserRolesById(user.UserID.ToString());

                            foreach (aspnet_Roles role in roles)
                            {
                                GetAllControlPoliciesSubParent(policies, sysXAdminList, ucName, role.RoleId.ToString(), user.CreatedByID);
                            }
                        }
                    }
                }
            }
            else
            {
                if (sysXAdminList.Contains(currentUserId).Equals(false))
                {
                    OrganizationUser user = CurrentRepositoryContext.GetOrganizationUser(currentUserId);

                    if (!user.IsNull())
                    {
                        List<aspnet_Roles> roles = CurrentRepositoryContext.GetUserRolesById(user.UserID.ToString());

                        foreach (aspnet_Roles role in roles)
                        {
                            GetAllControlPoliciesSubParent(policies, sysXAdminList, ucName, role.RoleId.ToString(), user.CreatedByID);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// List<Tuple<Int32, Int32, Int32, Boolean>>
        /// Tuple<SysXBlockFeatureID, TenantProductID, OrgUserID, IsBookMarked
        /// </summary>
        /// <param name="tenantProductId"></param>
        /// <param name="updatedSysXBlockIds"></param>
        /// <returns></returns>
        private List<Tuple<Int32, Int32, Int32, Boolean>> DeleteTenantProductFeature(Int32 tenantProductId, IEnumerable<Int32> updatedSysXBlockIds)
        {
            List<Tuple<Int32, Int32, Int32, Boolean>> lstDeletedBookmarks = new List<Tuple<int, int, int, bool>>();
            foreach (TenantProductFeature tenantProductFeature in updatedSysXBlockIds.Select(sysXBlockId => _dbNavigation.TenantProductFeatures.Include(SysXEntityConstants.TABLE_SYSX_BLOCKS_FEATURE).
                                                                                                                Where(condition => condition.TenantProductID == tenantProductId && condition.SysXBlocksFeature.SysXBlockID == sysXBlockId)).SelectMany(tenantProductFeature => tenantProductFeature))
            {
                List<Tuple<Int32, Int32, Int32, Boolean>> lst = DeleteFeatureBookmark(tenantProductFeature.TenantProductSysXBlockID, tenantProductFeature.SysXBlockFeatureID, tenantProductFeature.TenantProductID);
                DeleteFeaturePermission(tenantProductFeature.TenantProductSysXBlockID);
                base.DeleteObjectEntityInTransaction(tenantProductFeature, true);
                lstDeletedBookmarks.AddRange(lst);
            }
            return lstDeletedBookmarks;
        }

        private Boolean DeleteFeaturePermission(Int32 tenantProductFeaureId)
        {
            var featurePermissions = _dbNavigation.FeaturePermissions.Where(featurePerm => featurePerm.TenantProductFeatureID == tenantProductFeaureId);

            if (!featurePermissions.IsNull())
            {
                List<FeaturePermission> featurePermList = featurePermissions.ToList();

                foreach (FeaturePermission featureP in featurePermList)
                {
                    DeleteObjectEntityInTransaction(featureP, true);
                }
            }

            return true;
        }

        /// <summary>
        /// Tuple<Int32, Int32, Int32, Boolean>
        /// Tuple<SysXBlockFeatureID, TenantProductID, OrgUserID, IsBookMarked
        /// </summary>
        /// <param name="tenantProductFeaureId"></param>
        /// <returns></returns>
        private List<Tuple<Int32, Int32, Int32, Boolean>> DeleteFeatureBookmark(Int32 tenantProductFeaureId, Int32 sysXBlockFeatureID, Int32 tenantProductID)
        {
            List<Tuple<Int32, Int32, Int32, Boolean>> lstDeletedFeatureBookmark = new List<Tuple<int, int, int, bool>>();

            var featureBookmarks = _dbNavigation.FeaturesBookmarks.Where(featurePerm => featurePerm.FB_TenantProductSysXBlockID == tenantProductFeaureId);

            if (!featureBookmarks.IsNull())
            {
                foreach (FeaturesBookmark featureB in featureBookmarks.ToList())
                {
                    lstDeletedFeatureBookmark.Add(new Tuple<int, int, int, bool>(sysXBlockFeatureID, tenantProductID, featureB.FB_OrganizationUserID, featureB.FB_IsBookmarked));
                    DeleteObjectEntityInTransaction(featureB, true);
                }
            }
            return lstDeletedFeatureBookmark;
        }

        private Boolean DeleteRoleFeaturesMapping(String roleId, List<Int32> updatedSysXBlockIDs)
        {
            Guid oRoleDetailId = new Guid(roleId);

            foreach (Int32 sysXBlockId in updatedSysXBlockIDs)
            {
                var rolePermissionFeatures = _dbNavigation.RolePermissionProductFeatures.Include(SysXEntityConstants.TABLE_SYSX_BLOCKS_FEATURE).
                                            Where(rolePermissionFeature => rolePermissionFeature.RoleId == oRoleDetailId && rolePermissionFeature.SysXBlocksFeature.SysXBlockID == sysXBlockId);

                foreach (RolePermissionProductFeature roleFeature in rolePermissionFeatures.ToList())
                {
                    List<Int32> featureRoleActionIDs = roleFeature.FeatureRoleActions.Select(col => col.FeatureRoleActionID).ToList();

                    featureRoleActionIDs.ForEach(item =>
                    {
                        _dbNavigation.DeleteObject(_dbNavigation.FeatureRoleActions.FirstOrDefault(cond => cond.FeatureRoleActionID == item));
                    });
                    base.DeleteObjectEntityInTransaction(roleFeature, true);
                }
            }

            return true;
        }

        private Boolean DeleteSysXBlocksFeatures(List<Int32> sysXBlockfeatureIDs)
        {

            foreach (SysXBlocksFeature sysXBlockFeatures in sysXBlockfeatureIDs.Select(id => _dbNavigation.SysXBlocksFeatures.Include(SysXEntityConstants.ROLE_PERMISSION_PRODUCT_FEATURES).FirstOrDefault(block => block.SysXBlockFeatureID == id)))
            {
                base.DeleteObjectEntityInTransaction(sysXBlockFeatures, true);
            }

            return true;
        }

        private Boolean DeleteRolePermissionProductFeatures(Guid roleId)
        {
            IQueryable<RolePermissionProductFeature> rolePermissionProductFeature = _dbNavigation.RolePermissionProductFeatures.Where(condition => condition.RoleId == roleId);

            foreach (RolePermissionProductFeature permissionProductFeature in rolePermissionProductFeature)
            {
                _dbNavigation.DeleteObject(permissionProductFeature);
            }

            return true;
        }

        private void DeleteAllFeaturesAssociatedWithLOB(Int32 sysXBlockId)
        {
            IQueryable<SysXBlocksFeature> sysXBlocksFeatures = _dbNavigation.SysXBlocksFeatures.Where(condition => condition.SysXBlockID == sysXBlockId);

            // Remove all the associated features with the current Line Of Business based on SysXBlockID.
            foreach (SysXBlocksFeature sysXBlocksFeature in sysXBlocksFeatures)
            {
                base.DeleteObjectEntityInTransaction(sysXBlocksFeature, true);
            }
        }

        #endregion

        #endregion

        #region Compiled Queries

        #region Manage User's Account

        /// <summary> Check if any role is being assigned to currently logged in user. </summary>
        public static readonly Func<SysXAppDBEntities, Guid, Boolean> CompiledIsCurrentUserRoleExists = CompiledQuery.Compile
    <SysXAppDBEntities, Guid, Boolean>((dbNavigation, currentUserId) =>
                                                 dbNavigation.vw_aspnet_UsersInRoles.Where(condition => condition.UserId == currentUserId).Count().Equals(AppConsts.NONE));

        /// <summary> Check if any role is being assigned to currently logged in user. </summary>
        public static readonly Func<SysXAppDBEntities, Guid, Boolean> CompiledIsCurrentLoggedInUserRoleExists = CompiledQuery.Compile
    <SysXAppDBEntities, Guid, Boolean>((dbNavigation, currentUserId) =>
                                                 !(dbNavigation
                                                 .OrganizationUsers
                                                 .Include(SysXEntityConstants.TABLE_ASPNET_USERS)
                                                 .Include(SysXEntityConstants.TABLE_ASPNET_MEMBERSHIP)
                                                 .Include(SysXEntityConstants.TABLE_ASPNET_ROLES)
                                                 .Where(condition => condition.UserID == currentUserId &&
                                                    condition.aspnet_Users.aspnet_Membership.IsLockedOut.Equals(false)
                                                    && condition.IsActive && !condition.IsDeleted
                                                    && !condition.aspnet_Users.aspnet_Roles.Any()).Any()));

        #region To handle: last 5 password can't be used while user tries to change their password.

        /// <summary> Retrieves the PasswordSalt for the given UserId. </summary>
        public static readonly Func<SysXAppDBEntities, Guid, String> CompiledGetPasswordSaltByUserId = CompiledQuery.Compile
    <SysXAppDBEntities, Guid, String>((dbNavigation, currentUserId) =>
                                                 dbNavigation.aspnet_Membership.Where(condition => condition.UserId == currentUserId).Select(condition => condition.PasswordSalt).FirstOrDefault());

        /// <summary> Retrieves the PasswordHistory for the given UserId. </summary>
        public static readonly Func<SysXAppDBEntities, Guid, IQueryable<PasswordHistory>> CompiledGetPasswordHistoryByUserId = CompiledQuery.Compile
    <SysXAppDBEntities, Guid, IQueryable<PasswordHistory>>((dbNavigation, currentUserId) =>
                                                 dbNavigation.PasswordHistories.Where(condition => condition.UserID == currentUserId));

        /// <summary> Retrieves the count of PasswordHistory for the given UserId. </summary>
        public static readonly Func<SysXAppDBEntities, Guid, Int32> CompiledGetCountOfPasswordHistoryByUserId = CompiledQuery.Compile
    <SysXAppDBEntities, Guid, Int32>((dbNavigation, currentUserId) =>
                                                 dbNavigation.PasswordHistories.Where(condition => condition.UserID == currentUserId).Count());

        #endregion


        /// <summary>
        /// Check if the user belongs to Multi Tenants
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Boolean IsMultiTenantUser(Guid userId)
        {
            Boolean _isMultiTenant = false;
            Int32 applicantUserCount = AppConsts.NONE;
            Int32 clientAdminUserCount = AppConsts.NONE;
            Int32 adminUserCount = AppConsts.NONE;

            var _lstOrgUsers = _dbNavigation.OrganizationUsers.Where(orgUsr => orgUsr.UserID == userId && (orgUsr.IsSharedUser ?? false) == false
                                                                 && !orgUsr.IsDeleted).ToList(); //UAT-1218 SharedUser check

            if (_lstOrgUsers.Count() > AppConsts.ONE)
            {
                //UAT-1218
                foreach (var orgUser in _lstOrgUsers)
                {
                    if ((orgUser.IsApplicant ?? false) == true)
                    {
                        applicantUserCount = applicantUserCount + AppConsts.ONE;
                    }
                    else if ((orgUser.IsApplicant ?? false) == false && orgUser.Organization.TenantID != AppConsts.SUPER_ADMIN_TENANT_ID)
                    {
                        clientAdminUserCount = clientAdminUserCount + AppConsts.ONE;
                    }
                    else if ((orgUser.IsApplicant ?? false) == false && orgUser.Organization.TenantID == AppConsts.SUPER_ADMIN_TENANT_ID)
                    {
                        adminUserCount = adminUserCount + AppConsts.ONE;
                    }
                }
                if (applicantUserCount > AppConsts.ONE || clientAdminUserCount > AppConsts.ONE || adminUserCount > AppConsts.ONE)
                {
                    _isMultiTenant = true;
                }
            }
            return _isMultiTenant;
        }

        #endregion

        #region Manage User Themes

        /// <summary>
        /// Retreives the Logged-in UserId.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, Guid, aspnet_Membership> CompiledGetAspnetMembership =
            CompiledQuery.Compile<SysXAppDBEntities, Guid, aspnet_Membership>((dbNavigation, userId) => dbNavigation.aspnet_Membership.Where(condition => condition.UserId == userId).FirstOrDefault());

        #endregion

        #region Manage User Personalization
        public static readonly Func<SysXAppDBEntities, Guid, short, aspnet_PersonalizationPerUser> CompiledGetPersonalizedPreference = CompiledQuery.Compile<SysXAppDBEntities, Guid, short, aspnet_PersonalizationPerUser>((dbNavigation, userID, businessChannelTypeID) => dbNavigation.aspnet_PersonalizationPerUser.Include("aspnet_Paths")
                                                                                                                                                                                                 .FirstOrDefault(personalizationDetails => personalizationDetails.UserId == userID && personalizationDetails.BusinessChannelTypeID == businessChannelTypeID));
        public static readonly Func<SysXAppDBEntities, Int32, Int32, aspnet_PersonalizationAllUsers> CompiledGetGroupPreference = CompiledQuery.Compile<SysXAppDBEntities, Int32, Int32, aspnet_PersonalizationAllUsers>((dbNavigation, dashBoardUser, tenantID) => dbNavigation.aspnet_PersonalizationAllUsers.Include("aspnet_Paths")
                                                                                                                                                                                                  .FirstOrDefault(personalizationDetails => (personalizationDetails.ForExternalUser == dashBoardUser)));// && personalizationDetails.TenantID==tenantID
        #endregion

        #region Cities

        /// <summary>
        /// Retrieves list of all Cities.
        /// </summary>
        /// <returns>A <see cref="City"/> list of data from the underlying data storage.</returns>
        public static readonly Func<SysXAppDBEntities, IQueryable<City>> CompiledGetCities = CompiledQuery.Compile<SysXAppDBEntities, IQueryable<City>>(dbNavigation =>
                                                                                                                                                        dbNavigation.Cities
                                                                                                                                                        .OrderBy(city => city.CityName));

        #endregion

        #region Countries

        /// <summary>
        /// Retrieves list of all Cities.
        /// </summary>
        /// <returns>A <see cref="City"/> list of data from the underlying data storage.</returns>
        public static readonly Func<SysXAppDBEntities, IQueryable<Country>> CompiledGetCountries = CompiledQuery.Compile<SysXAppDBEntities, IQueryable<Country>>(dbNavigation =>
                                                                                                                                                        dbNavigation.Countries.OrderBy(country => country.FullName));

        #endregion

        #region States

        /// <summary>
        /// Retrieves list of States with its details.
        /// </summary>
        /// <returns>A <see cref="State"/> list of data from the underlying data storage.</returns>
        public static readonly Func<SysXAppDBEntities, IQueryable<State>> CompiledGetStates = CompiledQuery.Compile
            <SysXAppDBEntities, IQueryable<State>>(dbNavigation => dbNavigation.States);

        #endregion

        #region ZipCodes

        /// <summary>
        /// Retrieves ZipCode details cod based on zipID.
        /// </summary>
        /// <returns>A <see cref="ZipCode"/> data from the underlying data storage.</returns>
        public static readonly Func<SysXAppDBEntities, Int32, ZipCode> CompiledGetZip = CompiledQuery.Compile
            <SysXAppDBEntities, Int32, ZipCode>((dbNavigation, zipId) => dbNavigation.ZipCodes
                .Include(SysXEntityConstants.TABLE_COUNTY)
                .Include(SysXEntityConstants.TABLE_CITY)
                .Include(SysXEntityConstants.TABLE_CITY_STATE)
                .FirstOrDefault(zipcodes => zipcodes.ZipCodeID == zipId));

        #endregion

        #region Manage PermissionType

        /// <summary>
        /// Retrieves a list of all active permission types.
        /// </summary>
        /// <returns>A <see cref="PermissionType"/> list of data from the underlying data storage.</returns>
        public static readonly Func<SysXAppDBEntities, IQueryable<PermissionType>> CompiledGetPermissionTypes = CompiledQuery.Compile
            <SysXAppDBEntities, IQueryable<PermissionType>>(dbNavigation => dbNavigation.PermissionTypes.Where(pt => pt.IsActive && pt.IsDeleted == false));

        /// <summary>
        /// Retrieves all active permission types based on permission type id.
        /// </summary>
        /// <returns></returns>
        public static readonly Func<SysXAppDBEntities, Int32, PermissionType> CompiledGetPermissionTypeById = CompiledQuery.Compile
            <SysXAppDBEntities, Int32, PermissionType>((dbNavigation, permissionTypeId) => dbNavigation.PermissionTypes.FirstOrDefault(permissionTypeDetails => permissionTypeDetails.PermissionTypeID == permissionTypeId && permissionTypeDetails.IsActive && permissionTypeDetails.IsDeleted == false));

        /// <summary>
        /// Returns the permission assigned to the current permission type.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, Int32, IQueryable<Permission>> CompiledIsPermissionTypeAssignToAnyPermission = CompiledQuery.Compile
            <SysXAppDBEntities, Int32, IQueryable<Permission>>((dbNavigation, permissionTypeId) => dbNavigation.Permissions.Include(SysXEntityConstants.TABLE_PERMISSION_TYPE).Where(perm => perm.PermissionTypeId == permissionTypeId && perm.IsActive && perm.IsDeleted == false));

        #endregion

        #region Manage Permissions

        /// <summary>
        /// Retrieves a list of all active permissions.
        /// </summary>
        /// <returns>A <see cref="Permission"/> list of data from the underlying data storage.</returns>
        public static readonly Func<SysXAppDBEntities, IQueryable<Permission>> CompiledGetPermissions = CompiledQuery.Compile
            <SysXAppDBEntities, IQueryable<Permission>>(dbNavigation => dbNavigation.Permissions.Include(SysXEntityConstants.TABLE_PERMISSION_TYPE).Where(perm => perm.IsActive && perm.IsDeleted == false));

        /// <summary>
        /// Retrieves a list of all permissions based on permission id.
        /// </summary>
        /// <returns>A <see cref="Permission"/> data from the underlying data storage.</returns>
        public static readonly Func<SysXAppDBEntities, Int32, Permission> CompiledGetPermission = CompiledQuery.Compile
            <SysXAppDBEntities, Int32, Permission>((dbNavigation, permissionId) => dbNavigation.Permissions.Include(SysXEntityConstants.TABLE_PERMISSION_TYPE).FirstOrDefault(perm => perm.PermissionId == permissionId && perm.IsActive && perm.IsDeleted == false));

        /// <summary>
        /// Retrieves a list of al Permissions based on userId, featureId, blockId.
        /// </summary>
        /// <returns>A <see cref="Permission"/> data from the underlying data storage.</returns>
        public static readonly Func<SysXAppDBEntities, String, Int32, Int32, RolePermissionProductFeature> CompiledGetMyPermission = CompiledQuery.Compile
            <SysXAppDBEntities, String, Int32, Int32, RolePermissionProductFeature>((dbNavigation, userId, featureId, blockId) =>
                                                                                    dbNavigation.RolePermissionProductFeatures.Include(SysXEntityConstants.TABLE_PERMISSION).FirstOrDefault(rpp => rpp.RoleDetail.aspnet_Roles.aspnet_Users.Any(user => user.UserId == new Guid(userId))
                                                                                                                                                                           && rpp.SysXBlocksFeature.FeatureID == featureId
                                                                                                                                                                           && rpp.SysXBlocksFeature.SysXBlockID == blockId));


        /// <summary>
        /// Returns the features assigned to the current permission.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, Int32, IQueryable<FeaturePermission>> CompiledIsPermissionAssignToAnyFeature = CompiledQuery.Compile
            <SysXAppDBEntities, Int32, IQueryable<FeaturePermission>>((dbNavigation, permissionId) => dbNavigation.FeaturePermissions.Include(SysXEntityConstants.TABLE_PERMISSION).Where(featurePerm => featurePerm.PermissionID == permissionId));


        #endregion

        #region Manage Product's Feature

        /// <summary>
        /// Retrieves a list of  all active product features.
        /// </summary>
        /// <returns>A <see cref="Permission"/> list of data from the underlying data storage.</returns>
        public static readonly Func<SysXAppDBEntities, IQueryable<ProductFeature>> CompiledGetProductFeature = CompiledQuery.Compile
            <SysXAppDBEntities, IQueryable<ProductFeature>>(dbNavigation => dbNavigation.ProductFeatures.Include(SysXEntityConstants.TABLE_PRODUCT_FEATURE2).OrderBy(p => p.ProductFeatureID).Where(pFeature => pFeature.IsActive && pFeature.IsDeleted == false));

        /// <summary>
        /// Retrieves a list of all product feature based on it's id.
        /// </summary>
        /// <returns></returns>
        public static readonly Func<SysXAppDBEntities, Int32, ProductFeature> CompiledGetProductFeatureById = CompiledQuery.Compile
            <SysXAppDBEntities, Int32, ProductFeature>((dbNavigation, productFeatureId) => dbNavigation.ProductFeatures.Include(SysXEntityConstants.TABLE_PRODUCT_FEATURE2).FirstOrDefault(pFeature => pFeature.ProductFeatureID == productFeatureId && pFeature.IsActive && pFeature.IsDeleted == false));

        /// <summary>
        /// Determines whether the specified feature is associated with any block.
        /// </summary>
        /// <returns></returns>
        public static readonly Func<SysXAppDBEntities, Int32, Int32> CompiledIsFeatureAssociatedWithBlock = CompiledQuery.Compile
            <SysXAppDBEntities, Int32, Int32>((dbNavigation, productFeatureId) => dbNavigation.SysXBlocksFeatures.Count(sbf => sbf.FeatureID == productFeatureId));

        /// <summary>
        /// Determines whether the specified block is associated with any product.
        /// </summary>
        /// <returns></returns>
        public static readonly Func<SysXAppDBEntities, Int32, Boolean> CompiledIsBlockAssociatedWithProdcut = CompiledQuery.Compile
            <SysXAppDBEntities, Int32, Boolean>((dbNavigation, sysXBlockId) => dbNavigation.TenantProductFeatures.Include(SysXEntityConstants.TABLE_SYSX_BLOCKS_FEATURE).Any(tpf => tpf.SysXBlocksFeature.SysXBlockID.Equals(sysXBlockId)));

        /// <summary>
        /// Determines whether the specified block is associated with any product.
        /// </summary>
        /// <returns>
        /// true if line of business associated with product, false if not.
        /// </returns>
        public static readonly Func<SysXAppDBEntities, Int32, Boolean> CompiledIsAssetLOBorSysxBlock = CompiledQuery.Compile
            <SysXAppDBEntities, Int32, Boolean>((dbNavigation, sysXBlockId) => dbNavigation.lkpSysXBlocks.Any(action => action.SysXBlockId == sysXBlockId && (action.IsAssetLOB || action.IsSysXBlock)));

        /// <summary>
        /// Determines whether the specified user is tied to several other user(s).
        /// </summary>
        /// <returns></returns>
        public static readonly Func<SysXAppDBEntities, Int32, Int32> CompiledIsUserTiedToSeveralOtherUsers = CompiledQuery.Compile
            <SysXAppDBEntities, Int32, Int32>((dbNavigation, organizationUserId) => dbNavigation.OrganizationUsers.Where(organizationUserDetails => organizationUserDetails.CreatedByID == organizationUserId && organizationUserDetails.IsActive && organizationUserDetails.IsDeleted == false).Count());

        /// <summary>
        /// Retrieves a list of all features for product based on productID.
        /// </summary>
        /// <returns>A <see cref="TenantProductFeature"/> list of data from the underlying data storage.</returns>
        public static readonly Func<SysXAppDBEntities, Int32, IQueryable<TenantProductFeature>> CompiledGetFeaturesForProduct = CompiledQuery.Compile
            <SysXAppDBEntities, Int32, IQueryable<TenantProductFeature>>((dbNavigation, productId) => dbNavigation.TenantProductFeatures.Include(SysXEntityConstants.TABLE_FEATUREPERMISSIONS)
                                                                                                                                        .Include(SysXEntityConstants.TABLE_FEATUREPERMISSIONS_DOT_PERMISSION)
                                                                                                                                        .Include(SysXEntityConstants.TABLE_SYSX_BLOCKS_FEATURE)
                                                                                                                                        .Include(SysXEntityConstants.TABLE_SYSX_BLOCKS_FEATURE_DOT_SYSXBLOCK)
                                                                                                                                        .Include(SysXEntityConstants.TABLE_SYSX_BLOCKS_FEATURE_DOT_PRODUCT_FEATURE).Where(tenantProduct => tenantProduct.TenantProductID == productId
                                                                                                                                           && !tenantProduct.SysXBlocksFeature.ProductFeature.IsDeleted));

        #endregion

        #region Manage SysXConfig

        /// <summary>
        /// Retrieves the application configuration value based on SysXKey.
        /// </summary>
        /// <returns></returns>
        public static readonly Func<SysXAppDBEntities, String, SysXConfig> CompiledGetSysxConfigValue = CompiledQuery.Compile
            <SysXAppDBEntities, String, SysXConfig>((dbNavigation, sysKey) => dbNavigation.SysXConfigs.FirstOrDefault(conf => conf.SysXKey == sysKey));

        /// <summary>
        /// Retrieves the Application Configuration based on SysXKey.
        /// </summary>
        /// <returns></returns>
        public static readonly Func<SysXAppDBEntities, String, SysXConfig> CompiledGetSysXConfig = CompiledQuery.Compile
            <SysXAppDBEntities, String, SysXConfig>((dbNavigation, sysXKey) => dbNavigation.SysXConfigs.FirstOrDefault(conf => conf.SysXKey == sysXKey));

        /// <summary>
        /// Retrieves Application configurations.
        /// </summary>
        /// <returns></returns>
        public static readonly Func<SysXAppDBEntities, IQueryable<SysXConfig>> CompiledGetSysXConfigs = CompiledQuery.Compile
            <SysXAppDBEntities, IQueryable<SysXConfig>>(dbNavigation => dbNavigation.SysXConfigs);

        #endregion

        #region Manage Tenants

        /// <summary>
        /// Compiled query to get all tenant records.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, IQueryable<Tenant>> CompiledGetTenants = CompiledQuery.Compile<SysXAppDBEntities, IQueryable<Tenant>>(dbNavigation =>
                                                                                                            dbNavigation.Tenants.Include(SysXEntityConstants.TABLE_TENANT_PRODUCTS)
                                                                                                            .Include(SysXEntityConstants.TABLE_ORGANIZATIONS)
                                                                                                            .Include(SysXEntityConstants.TABLE_ADDRESSHANDLE_ADDRESSES_ZIPCODE_CITY_STATE)
                                                                                                            .Include(SysXEntityConstants.TABLE_ORGANIZATIONS_ADDRESSHANDLE_ADDRESSES_ZIPCODE_CITY_STATE)
                                                                                                            .Include(SysXEntityConstants.TABLE_CONTACT)
                                                                                                            .Include(SysXEntityConstants.ORGANIZATIONS_DOT_CONTACT)
                                                                                                            .Include(SysXEntityConstants.ORGANIZATIONS_DOT_CONTACT_DOT_CONTACTDETAILS)
                                                                                                            .Include(SysXEntityConstants.ORGANIZATIONS_DOT_VALIDATEDADDRESS)
                                                                                                            .Include(SysXEntityConstants.CONTACT_DOT_CONTACTDETAILS)
                                                                                                            .Include(SysXEntityConstants.LKPTENANTTYPE)
                                                                                                            //.include(SysXEntityConstants.TABLE_SUPPLIERS)   // change to create manage sub tenant link for supplier created tenent.
                                                                                                            //.Include(SysXEntityConstants.TABLE_CLIENTS)     // change to create manage sub tenant link for client created tenant.
                                                                                                            .Include("Organizations.OrganizationUserNamePrefixes")
                                                                                                            .Where(condition => condition.IsActive &&
                                                                                                                       condition.IsDeleted == false));

        /// <summary>
        /// Compiled query to get all tenant records.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, IQueryable<Tenant>> CompiledGetTenantLists = CompiledQuery.Compile<SysXAppDBEntities, IQueryable<Tenant>>(dbNavigation =>
                                                                                                            dbNavigation.Tenants.Where(condition => condition.IsActive &&
                                                                                                                       condition.IsDeleted == false));

        /// <summary>
        /// Compiled query to get all tenant records.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, String, IQueryable<Tenant>> CompiledGetUserTenants = CompiledQuery.Compile<SysXAppDBEntities, String, IQueryable<Tenant>>((dbNavigation, currentOrgUserId) =>
                                                                                                            dbNavigation.OrganizationUsers
                                                                                                            .Include(SysXEntityConstants.LKPTENANTTYPE)
                                                                                                            .Where(condition => condition.UserID == new Guid(currentOrgUserId) && condition.IsApplicant == true
                                                                                                                && condition.IsActive && condition.IsDeleted == false
                                                                                                                && condition.Organization.Tenant.IsActive == true
                                                                                                                && condition.Organization.Tenant.IsDeleted == false)
                                                                                                                .Select(con => con.Organization.Tenant));


        /// <summary>
        /// Compiled query to get all tenant records.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, String, IQueryable<Tenant>> CompiledGetUserTenantsForAllTypeofUsers = CompiledQuery.Compile<SysXAppDBEntities, String, IQueryable<Tenant>>((dbNavigation, currentOrgUserId) =>
                                                                                                            dbNavigation.OrganizationUsers
                                                                                                            .Include(SysXEntityConstants.LKPTENANTTYPE)
                                                                                                            .Where(condition => condition.UserID == new Guid(currentOrgUserId)
                                                                                                                && condition.IsActive && condition.IsDeleted == false
                                                                                                                && condition.Organization.Tenant.IsActive == true
                                                                                                                && condition.Organization.Tenant.IsDeleted == false)
                                                                                                                .Select(con => con.Organization.Tenant));







        /// <summary>
        /// Get Tenant Information by Tenant Id
        /// </summary>
        public static readonly Func<SysXAppDBEntities, Int32, Tenant> CompiledGetTenantById = CompiledQuery.Compile<SysXAppDBEntities, Int32, Tenant>((dbNavigation, tenantId) => dbNavigation.Tenants
                                                                                                                                                                                      .Include(SysXEntityConstants.TABLE_ORGANIZATIONS)
                                                                                                                                                                                      .Include(SysXEntityConstants.TABLE_TENANT_PRODUCTS)
                                                                                                                                                                                      .Include(SysXEntityConstants.TABLE_TENANT_PRODUCTS_DOT_ROLE_DETAILS).Include(SysXEntityConstants.TABLE_CONTACT).Include(SysXEntityConstants.TABLE_CONTACT_DOT_CONTACT_DETAILS)
                                                                                                                                                                                      .FirstOrDefault(tenantDetails => tenantDetails.TenantID == tenantId
                                                                                                                                                                                                                       && tenantDetails.IsActive
                                                                                                                                                                                                                       && tenantDetails.IsDeleted == false));

        /// <summary>
        /// Retrieves a list of all tenant's product feature for each block.
        /// </summary>
        /// <returns>A <see cref="TenantProductFeature"/> list of data from the underlying data storage.</returns>
        public static readonly Func<SysXAppDBEntities, Int32, Int32, IQueryable<TenantProductFeature>> CompiledGetTenantProductFeaturesForBlock =
            CompiledQuery.Compile<SysXAppDBEntities, Int32, Int32, IQueryable<TenantProductFeature>>((dbNavigation, tenantProductId, sysXBlockId) =>
                                                                                                     dbNavigation.TenantProductFeatures
                                                                                                         .Include(SysXEntityConstants.TABLE_SYSX_BLOCKS_FEATURE)
                                                                                                         .Include(SysXEntityConstants.TABLE_SYSX_BLOCKS_FEATURE_DOT_PRODUCT_FEATURE)
                                                                                                         .Include(SysXEntityConstants.TABLE_SYSX_BLOCKS_FEATURE_DOT_SYSXBLOCK)
                                                                                                         .Include(SysXEntityConstants.TABLE_SYSXBLOCKS_FEATURE_DOT_PRODUCT_FEATURE_DOT_PRODUCT_FEATURE2)
                                                                                                         .Include(SysXEntityConstants.TABLE_FEATUREPERMISSIONS)
                                                                                                         .Include(SysXEntityConstants.TABLE_FEATUREPERMISSIONS_DOT_PERMISSION)
                                                                                                         .Where(a => a.TenantProductID == tenantProductId && a.SysXBlocksFeature.SysXBlockID == sysXBlockId
                                                                                                         && !a.SysXBlocksFeature.ProductFeature.IsDeleted));

        /// <summary>
        /// Retrieves tenant product based on tenantId.
        /// </summary>
        /// <returns></returns>
        public static readonly Func<SysXAppDBEntities, Int32, TenantProduct> CompiledGetTenantProductId =
            CompiledQuery.Compile<SysXAppDBEntities, Int32, TenantProduct>((dbNavigation, tenantId) =>
                                                                           dbNavigation.TenantProducts.FirstOrDefault(cp => cp.TenantID == tenantId));

        /// <summary>
        /// Compiled query to return ZipCode by zipCodeNumber
        /// </summary>
        public static readonly Func<SysXAppDBEntities, String, IQueryable<ZipCode>> CompiledGetStateCities
                                                    = CompiledQuery.Compile<SysXAppDBEntities, String, IQueryable<ZipCode>>
                                                      (
                                                      (dbNavigation, zipCodeNumber)
                                                      => (from zipCode in dbNavigation.ZipCodes
                                                           .Include(SysXEntityConstants.TABLE_CITY)
                                                           .Include(SysXEntityConstants.TABLE_CITY_STATE)
                                                           .Where(zipCodeDetails => zipCodeDetails.ZipCode1.Equals(zipCodeNumber))
                                                          select zipCode)
                                                      );

        #endregion

        #region Manage Products

        /// <summary>
        /// Retrieves a list of all active products.
        /// </summary>
        /// <returns>A <see cref="TenantProduct"/> list of data from the underlying data storage.</returns>
        public static readonly Func<SysXAppDBEntities, IQueryable<TenantProduct>> CompiledGetProducts =
            CompiledQuery.Compile<SysXAppDBEntities, IQueryable<TenantProduct>>(dbNavigation =>
                                                                                dbNavigation.TenantProducts.Where(tenantProducts => tenantProducts.IsActive && tenantProducts.IsDeleted == false));

        /// <summary>
        /// Retrieves product details based on product id.
        /// </summary>
        /// <returns></returns>
        public static readonly Func<SysXAppDBEntities, Int32, TenantProduct> CompiledGetProduct = CompiledQuery.Compile<SysXAppDBEntities, Int32, TenantProduct>((dbNavigation, productId) => dbNavigation.TenantProducts.Include(SysXEntityConstants.TABLE_ROLE_DETAILS)
                                                                                                                                                                                                                          .Include(SysXEntityConstants.TABLE_TENANT_PRODUCT_FEATURES)
                                                                                                                                                                                                                          .Include(SysXEntityConstants.TABLE_TENANT).Include(SysXEntityConstants.TABLE_TENANT_ROLEDETAILS_DOT_ORGANIZATIONUSER).FirstOrDefault(tenantProducts => tenantProducts.TenantProductID == productId && tenantProducts.IsActive && tenantProducts.IsDeleted == false));

        #endregion

        #region Manage Organizations



        /// <summary>
        /// Retrieves the organization based on Tenant's Id.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, Int32, IQueryable<Organization>> CompiledGetOrganizationsByTenantId =
            CompiledQuery.Compile<SysXAppDBEntities, Int32, IQueryable<Organization>>((dbNavigation, tenantId) => dbNavigation.Organizations.Include(SysXEntityConstants.TABLE_TENANT).Include(SysXEntityConstants.TABLE_TENANT_LKPTENANTTYPE).Where(organizationDetails => organizationDetails.IsActive && organizationDetails.IsDeleted == false && organizationDetails.TenantID == tenantId));



        /// <summary>
        /// Retrieves an organization details based on organizationID.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, Int32, Organization> CompiledGetOrganization =
            CompiledQuery.Compile<SysXAppDBEntities, Int32, Organization>((dbNavigation, organizationId) => dbNavigation.Organizations.FirstOrDefault(organizationsDetails => organizationsDetails.OrganizationID == organizationId && organizationsDetails.IsActive && organizationsDetails.IsDeleted == false));

        /// <summary>
        /// Retrieves departments
        /// </summary>
        public static readonly Func<SysXAppDBEntities, Int32, IQueryable<Organization>> CompiledGetDepartment =
            CompiledQuery.Compile<SysXAppDBEntities, Int32, IQueryable<Organization>>((dbNavigation, organizationId) => dbNavigation.Organizations.Where(organizationsDetails => organizationsDetails.OrganizationID == organizationId && organizationsDetails.IsActive && organizationsDetails.IsDeleted == false && organizationsDetails.ParentOrganizationID == organizationId && organizationsDetails.ParentOrganizationID != null));


        /// <summary>
        /// Retrieves an organization details based on tenantID.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, Int32, Organization> CompiledGetOrganizationForTenant =
            CompiledQuery.Compile<SysXAppDBEntities, Int32, Organization>((dbNavigation, tenantId) => dbNavigation.Organizations
                .Include(SysXEntityConstants.TABLE_TENANT).Include(SysXEntityConstants.TABLE_TENANT_LKPTENANTTYPE)
                .Include(SysXEntityConstants.TABLE_ORGANIZATION_USERS)
                .Include(SysXEntityConstants.TABLE_ORGANIZATION_USERS_DOT_ASPNET_USERS)
                .Include(SysXEntityConstants.TABLE_ADDRESSHANDLE_ADDRESSES_ZIPCODE_CITY_STATE)
                .Include(SysXEntityConstants.TABLE_CONTACT)
                .FirstOrDefault(organizationsDetail => organizationsDetail.Tenant.TenantID == tenantId
                    && organizationsDetail.ParentOrganizationID == null
                    && organizationsDetail.IsActive
                    && organizationsDetail.IsDeleted == false));

        /// <summary>
        ///  Retrieves all Organizations including deleted and active.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, IQueryable<Organization>> CompiledGetAllOrganization =
            CompiledQuery.Compile<SysXAppDBEntities, IQueryable<Organization>>(dbNavigation => dbNavigation.Organizations.Where(organizationDetails => organizationDetails.IsActive && organizationDetails.IsDeleted == false && organizationDetails.ParentOrganizationID == null));

        /// <summary>
        ///  Retrieves all Organizations based on organizationID.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, Int32, Organization> CompiledGetAllTypeOrganization =
            CompiledQuery.Compile<SysXAppDBEntities, Int32, Organization>((dbNavigation, organizationId) => dbNavigation.Organizations.Include(SysXEntityConstants.TABLE_TENANT).Include(SysXEntityConstants.TABLE_TENANT_LKPTENANTTYPE).FirstOrDefault(organizationDetails => organizationDetails.OrganizationID == organizationId));

        /// <summary>
        /// Retrieves product's information based on productID.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, Int32, TenantProduct> CompiledGetTenantProductInformation =
            CompiledQuery.Compile<SysXAppDBEntities, Int32, TenantProduct>((dbNavigation, productId) => dbNavigation.TenantProducts.Include(SysXEntityConstants.TABLE_TENANT).FirstOrDefault(prod => prod.IsActive && prod.IsDeleted == false && prod.TenantProductID == productId));

        /// <summary>
        /// Retrieves Organization details based on product information.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, Boolean, Int32, IQueryable<Organization>> CompiledGetOrganizationsForProduct =
            CompiledQuery.Compile<SysXAppDBEntities, Boolean, Int32, IQueryable<Organization>>((dbNavigation, isAdmin, compiledGetOrganizationsForProduct) => dbNavigation.Organizations.Include(SysXEntityConstants.TABLE_TENANT).Include(SysXEntityConstants.TABLE_TENANT_LKPTENANTTYPE).Where(organizationDetails => organizationDetails.IsActive &&
                                                                                                                                                                                                                                        organizationDetails.IsDeleted == false &&
                                                                                                                                                                                                                                        organizationDetails.Tenant.TenantID == compiledGetOrganizationsForProduct));
        /// <summary>
        /// Check Organization/Department for the current User.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, Int32, IQueryable<Organization>> CompiledCheckOrganizationByCurrentUserId =
           CompiledQuery.Compile<SysXAppDBEntities, Int32, IQueryable<Organization>>((dbNavigation, currentUserId) => dbNavigation.Organizations.Where(organization => organization.CreatedByID == currentUserId && organization.IsActive && organization.IsDeleted == false));


        /// <summary>
        /// Check Users for the current Department.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, Int32, IQueryable<OrganizationUser>> CompiledIsUserExistsForDepartment = CompiledQuery.Compile
            <SysXAppDBEntities, Int32, IQueryable<OrganizationUser>>((dbNavigation, departmentId) => dbNavigation.OrganizationUsers.Where(orgUser => orgUser.OrganizationID == departmentId && orgUser.IsActive && orgUser.IsDeleted == false));

        public static readonly Func<SysXAppDBEntities, IQueryable<OrganizationUserNamePrefix>> CompiledUserNamePrefix = CompiledQuery.Compile
            <SysXAppDBEntities, IQueryable<OrganizationUserNamePrefix>>((dbNavigation) => dbNavigation.OrganizationUserNamePrefixes);


        /// <summary>
        /// Retrieves all UserNamePrefix based on Organization id.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, Int32, IQueryable<OrganizationUserNamePrefix>> CompiledGetOrganizationUserNamePrefix = CompiledQuery.Compile
            <SysXAppDBEntities, Int32, IQueryable<OrganizationUserNamePrefix>>((dbNavigation, organizationId) => dbNavigation.OrganizationUserNamePrefixes.Where(orgUserpf => orgUserpf.OrganizationID == organizationId && orgUserpf.IsActive));


        #endregion

        #region Manage Programs

        //public static readonly Func<SysXAppDBEntities, Int32, IQueryable<AdminProgramStudy>> CompiledGetProgramsByOrganizationId =
        //    CompiledQuery.Compile<SysXAppDBEntities, Int32, IQueryable<AdminProgramStudy>>((dbNavigation, OrganizationId) => dbNavigation.AdminProgramStudies.
        //        Include(SysXEntityConstants.TABLE_ORGANIZATION).Where(ProgramDetails => ProgramDetails.DeleteFlag == false
        //            && ProgramDetails.OrganizationID == OrganizationId));

        //public static readonly Func<SysXAppDBEntities, Int32, AdminProgramStudy> CompiledGetProgram =
        //    CompiledQuery.Compile<SysXAppDBEntities, Int32, AdminProgramStudy>((dbNavigation, ProgramId) => dbNavigation.AdminProgramStudies.Where(programDetails => programDetails.AdminProgramStudyID == ProgramId).FirstOrDefault());

        //public static readonly Func<SysXAppDBEntities, IQueryable<AdminProgramStudy>> CompiledGetAllProgram =
        //    CompiledQuery.Compile<SysXAppDBEntities, IQueryable<AdminProgramStudy>>(dbNavigation => dbNavigation.AdminProgramStudies.Where(ProgramDetails => ProgramDetails.DeleteFlag == false));

        #endregion

        #region Manage Grades

        public static readonly Func<SysXAppDBEntities, Int32, IQueryable<lkpGradeLevel>> CompiledGetGradesByOrganizationId =
            CompiledQuery.Compile<SysXAppDBEntities, Int32, IQueryable<lkpGradeLevel>>((dbNavigation, OrganizationId) => dbNavigation.lkpGradeLevels.Include(SysXEntityConstants.TABLE_ORGANIZATION).Where(GradeDetails => GradeDetails.DeleteFlag == false));

        public static readonly Func<SysXAppDBEntities, Int32, lkpGradeLevel> CompiledGetGrade =
            CompiledQuery.Compile<SysXAppDBEntities, Int32, lkpGradeLevel>((dbNavigation, GradeId) => dbNavigation.lkpGradeLevels.Where(GradeDetails => GradeDetails.GradeLevelID == GradeId).FirstOrDefault());


        public static readonly Func<SysXAppDBEntities, IQueryable<lkpGradeLevel>> CompiledGetAllGrade =
            CompiledQuery.Compile<SysXAppDBEntities, IQueryable<lkpGradeLevel>>(dbNavigation => dbNavigation.lkpGradeLevels.Where(GradeDetails => GradeDetails.DeleteFlag == false));

        #endregion

        #region Manage Mapping Method

        /// <summary>
        /// Retrieves features for line of business and all of its delegation.
        /// </summary>
        /// <param name="blockId"></param>
        /// <returns></returns>
        IQueryable<SysXBlocksFeature> ISecurityRepository.GetFeaturesForLineOfBusinessAndAllowDelegation(Int32 blockId)
        {
            return _dbNavigation.SysXBlocksFeatures.Include(SysXEntityConstants.TABLE_PRODUCT_FEATURE).Where(sysXBlocksFeatures => sysXBlocksFeatures.SysXBlockID == blockId
                                                                                                              && !sysXBlocksFeatures.ProductFeature.IsDeleted).Where(bFeature => bFeature.ProductFeature.AllowDelegation);
        }

        /// <summary>
        /// Retrieves a list of all features for Tenant's product.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, Int32, IQueryable<TenantProductFeature>> CompiledGetTenantProductFeatures =
            CompiledQuery.Compile<SysXAppDBEntities, Int32, IQueryable<TenantProductFeature>>((dbNavigation, tenantProductId) => dbNavigation.TenantProductFeatures.Where(tenantProductFeatures => tenantProductFeatures.TenantProductID == tenantProductId));

        /// <summary>
        /// Retrieves product's feature.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, Int32, Int32, TenantProductFeature> CompiledGetTenantProductFeature =
            CompiledQuery.Compile<SysXAppDBEntities, Int32, Int32, TenantProductFeature>((dbNavigation, tenantProductId, sysXBlockFeatureId) => dbNavigation.TenantProductFeatures.FirstOrDefault(tenantProductFeatures => tenantProductFeatures.TenantProductID == tenantProductId && tenantProductFeatures.SysXBlockFeatureID == sysXBlockFeatureId));

        /// <summary>
        /// Retrieve features for a block based on blockID.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, Int32, IQueryable<SysXBlocksFeature>> CompiledGetTenantProductFeatureByBlockId =
            CompiledQuery.Compile<SysXAppDBEntities, Int32, IQueryable<SysXBlocksFeature>>((dbNavigation, blockId) => dbNavigation.SysXBlocksFeatures.Include(SysXEntityConstants.TABLE_PRODUCT_FEATURE).Where(sysXBlocksFeatures => sysXBlocksFeatures.SysXBlockID == blockId));

        #endregion

        #region Manage LineOfBusinesses

        /// <summary>
        /// Retrieves a list of all active blocks.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, String, IQueryable<vw_UserAssignedBlocks>> CompiledGetLineOfBusinessesByUser =
            CompiledQuery.Compile<SysXAppDBEntities, String, IQueryable<vw_UserAssignedBlocks>>((dbNavigation, userId) => dbNavigation.vw_UserAssignedBlocks.Where(userDetails => userDetails.UserId == new Guid(userId)));


        /// <summary>
        /// Retrieves a list of all active blocks.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, IQueryable<lkpSysXBlock>> CompiledGetLineOfBusinesses =
            CompiledQuery.Compile<SysXAppDBEntities, IQueryable<lkpSysXBlock>>(dbNavigation => dbNavigation.lkpSysXBlocks.Include(SysXEntityConstants.TABLE_SYSX_BLOCKS_FEATURES).Include("lkpBusinessChannelType").Where(sysXBlock => sysXBlock.IsActive && sysXBlock.IsDeleted == false && sysXBlock.SysXBlockId != AppConsts.NONE));

        /// <summary>
        /// Retrieves all blocks based on blockID.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, Int32, lkpSysXBlock> CompiledGetLineOfBusiness =
            CompiledQuery.Compile<SysXAppDBEntities, Int32, lkpSysXBlock>((dbNavigation, blockId) => dbNavigation.lkpSysXBlocks.FirstOrDefault(block => block.SysXBlockId == blockId && block.IsActive && block.IsDeleted == false));


        #endregion

        #region Manage User and Roles

        /// <summary>
        /// Retrieves roles based on role id, while navigating through Manage role page.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, Boolean, String, IQueryable<aspnet_Roles>> CompiledGetRolesByAssignToRoleId =
            CompiledQuery.Compile<SysXAppDBEntities, Boolean, String, IQueryable<aspnet_Roles>>((dbNavigation, loadRoleDetails, assignToRoleId) => dbNavigation.aspnet_Roles.Include(SysXEntityConstants.TABLE_ROLE_DETAIL).Where(aspnetRoles => aspnetRoles.RoleDetail.IsActive && aspnetRoles.RoleDetail.IsDeleted == false).Where(condition => condition.RoleId == new Guid(assignToRoleId)));

        /// <summary>
        ///  Retrieves a list of all Users.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, IQueryable<aspnet_Users>> CompiledGetUsers =
            CompiledQuery.Compile<SysXAppDBEntities, IQueryable<aspnet_Users>>(dbNavigation => dbNavigation.aspnet_Users);

        /// <summary>
        ///  Retrieves an array of authorization rules.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, String, IQueryable<RolePermissionProductFeature>> CompiledGetAuthorizationRules =
            CompiledQuery.Compile<SysXAppDBEntities, String, IQueryable<RolePermissionProductFeature>>((dbNavigation, urlPath) => dbNavigation.RolePermissionProductFeatures.Include(SysXEntityConstants.TABLE_ROLE_DETAIL).Where(rolePermissionProductFeatures => urlPath.StartsWith(rolePermissionProductFeatures.SysXBlocksFeature.ProductFeature.NavigationURL)));

        /// <summary>
        ///  Retrieves a list of all of aspnet user.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, Boolean, IQueryable<aspnet_Roles>> CompiledGetRoles =
            CompiledQuery.Compile<SysXAppDBEntities, Boolean, IQueryable<aspnet_Roles>>((dbNavigation, loadRoleDetails) => dbNavigation.aspnet_Roles.Include(SysXEntityConstants.TABLE_ROLE_DETAIL).Where(aspnetRoles => aspnetRoles.RoleDetail.IsActive && aspnetRoles.RoleDetail.IsDeleted == false));

        /// <summary>
        ///  Retrieves aspnet users based on userId.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, String, Boolean, Boolean, aspnet_Users> CompiledGetUserById =
            CompiledQuery.Compile<SysXAppDBEntities, String, Boolean, Boolean, aspnet_Users>((dbNavigation, userId, loadMembership, loadOrganizationUser) => dbNavigation.aspnet_Users.FirstOrDefault(aspnetUsers => aspnetUsers.UserId == new Guid(userId)));

        /// <summary>
        /// Compiled query to retrieve a aspnet user based on user name.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, String, Boolean, Boolean, aspnet_Users> CompiledGetUserByName =
            CompiledQuery.Compile<SysXAppDBEntities, String, Boolean, Boolean, aspnet_Users>((dbNavigation, userName, loadMembership, loadOrganizationUser) =>
                                                                                             (dbNavigation.aspnet_Users.FirstOrDefault(aspnetUsers => aspnetUsers.UserName == userName)));

        /// <summary>
        /// Compiled query to retrieve user's name by email address.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, String, aspnet_Users> CompiledGetUserNameByEmail =
            CompiledQuery.Compile<SysXAppDBEntities, String, aspnet_Users>((dbNavigation, email) => (dbNavigation.aspnet_Users.FirstOrDefault(aspnetUsers => aspnetUsers.aspnet_Membership.Email == email)));

        /// <summary>
        /// Retrieves a list of all aspnet users based on userName.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, String, aspnet_Users> CompiledGetUserRoles =
            CompiledQuery.Compile<SysXAppDBEntities, String, aspnet_Users>((dbNavigation, userName) => dbNavigation.aspnet_Users.FirstOrDefault(aspnetUsers => aspnetUsers.UserName == userName && aspnetUsers.aspnet_Roles.Any(aspnetRoles => aspnetRoles.RoleDetail.IsActive)));

        /// <summary>
        /// Retrieves a list of all roles of user based on User's ID.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, String, aspnet_Users> CompiledGetUserRolesById =
            CompiledQuery.Compile<SysXAppDBEntities, String, aspnet_Users>((dbNavigation, userId) => dbNavigation.aspnet_Users.Include(SysXEntityConstants.TABLE_ASPNET_ROLES_ROLEDETAIL).FirstOrDefault(aspnetUsers => aspnetUsers.UserId == new Guid(userId) && aspnetUsers.aspnet_Roles.Any(aspnetRoles => aspnetRoles.RoleDetail.IsActive && !aspnetRoles.RoleDetail.IsUserGroupLevel)));

        /// <summary>
        /// Retrieves a list of all users in a particular role.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, String, aspnet_Roles> CompiledGetUsersInRole =
            CompiledQuery.Compile<SysXAppDBEntities, String, aspnet_Roles>((dbNavigation, roleName) => dbNavigation.aspnet_Roles.FirstOrDefault(aspnetRoles => aspnetRoles.RoleName == roleName));

        /// <summary>
        /// Compiled query to retrieve SysxAdmin's user id.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, String, IQueryable<OrganizationUser>> CompiledGetSysXAdminUserIds =
            CompiledQuery.Compile<SysXAppDBEntities, String, IQueryable<OrganizationUser>>((dbNavigation, sysXAdminRoleName) => (dbNavigation.OrganizationUsers.Where(organizationUserDetails => organizationUserDetails.aspnet_Users.aspnet_Roles.Any(role => role.RoleName == sysXAdminRoleName))));

        #region  GetRolesByUserID Compiled Query

        /// <summary>
        /// Compiled query to retrieve OrganizationUser details based on userID.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, String, OrganizationUser> CompiledGetOrganizationUserDetailsByUserId =
            CompiledQuery.Compile<SysXAppDBEntities, String, OrganizationUser>((dbNavigation, userId) => dbNavigation.OrganizationUsers.Where(condition => condition.UserID == new Guid(userId) && condition.IsDeleted == false).FirstOrDefault());

        /// <summary>
        /// Compiled query to retrieve Organization details based on organizationID.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, Int32, Organization> CompiledGetOrganizationDetailsByOrganizationId =
            CompiledQuery.Compile<SysXAppDBEntities, Int32, Organization>((dbNavigation, organizationId) => dbNavigation.Organizations.Where(condition => condition.OrganizationID == organizationId && condition.IsDeleted == false).FirstOrDefault());

        /// <summary>
        /// Compiled query to retrieve TenantProduct details based on tenantID.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, Int32, TenantProduct> CompiledGetTenantProductDetailsByTenantId =
            CompiledQuery.Compile<SysXAppDBEntities, Int32, TenantProduct>((dbNavigation, tenantId) => dbNavigation.TenantProducts.FirstOrDefault(condition => condition.TenantID == tenantId));

        /// <summary>
        /// Compiled query to retrieve AspnetRoles details based on tenantProductID.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, Int32, IQueryable<aspnet_Roles>> CompiledGetAspnetRolesByTenantProductId =
            CompiledQuery.Compile<SysXAppDBEntities, Int32, IQueryable<aspnet_Roles>>((dbNavigation, tenantProductId) => dbNavigation.aspnet_Roles.Include(SysXEntityConstants.TABLE_ROLE_DETAIL).Where(role => role.RoleDetail.IsActive && role.RoleDetail.IsDeleted == false && role.RoleDetail.ProductID == tenantProductId && !role.RoleDetail.IsUserGroupLevel));
        #endregion

        #endregion

        #region Manage Role Details

        /// <summary>
        /// Compiled query for retrieving a list of all role details.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, IQueryable<RoleDetail>> CompiledGetRoleDetails =
            CompiledQuery.Compile<SysXAppDBEntities, IQueryable<RoleDetail>>(dbNavigation => dbNavigation.RoleDetails.Include(SysXEntityConstants.TABLE_TENANT_PRODUCT).Where(role => role.IsActive && role.IsDeleted == false));

        /// <summary>
        /// Compiled query for retrieving a list of all role details.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, Int32, IQueryable<RoleDetail>> CompiledGetRoleDetailsByProductId =
            CompiledQuery.Compile<SysXAppDBEntities, Int32, IQueryable<RoleDetail>>((dbNavigation, productId) => dbNavigation.RoleDetails.Include(SysXEntityConstants.TABLE_TENANT_PRODUCT).Where(role => role.IsActive && role.IsDeleted == false && role.ProductID == productId));

        /// <summary>
        /// Retrieves role details based on roleDetailId.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, String, RoleDetail> CompiledGetRoleDetailById =
            CompiledQuery.Compile<SysXAppDBEntities, String, RoleDetail>((dbNavigation, roleDetailId) => dbNavigation.RoleDetails.Include(SysXEntityConstants.TABLE_ASPNET_ROLES).FirstOrDefault(roleDetails => roleDetails.RoleDetailID == new Guid(roleDetailId)));

        /// <summary>
        /// Retrieves an instance of application.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, aspnet_Applications> CompiledGetApplication =
            CompiledQuery.Compile<SysXAppDBEntities, aspnet_Applications>(dbNavigation => dbNavigation.aspnet_Applications.FirstOrDefault());

        #region GetRoleDetail(Boolean isAdmin, Int32 currentUserID) Compiled Query

        /// <summary>
        /// Compiled query for retrieving a list of all active role details. 
        /// </summary>
        public static readonly Func<SysXAppDBEntities, IQueryable<RoleDetail>> CompiledGetAllActiveRolesWithItsDetails =
            CompiledQuery.Compile<SysXAppDBEntities, IQueryable<RoleDetail>>(dbNavigation => dbNavigation.RoleDetails.Include(SysXEntityConstants.TABLE_TENANT_PRODUCT_DOT_TENANT)
                                                                                                                     .Include(SysXEntityConstants.TABLE_ORGANIZATION_USER).Where(role => role.IsActive && role.IsDeleted == false).OrderBy(role => role.CreatedOn));

        /// <summary>
        /// Compiled query for retrieving current user details.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, Int32, IQueryable<OrganizationUser>> CompiledCurrentUserDetails =
            CompiledQuery.Compile<SysXAppDBEntities, Int32, IQueryable<OrganizationUser>>((dbNavigation, currentUserId) => dbNavigation.OrganizationUsers.Where(organizationuserinfo => organizationuserinfo.OrganizationUserID.Equals(currentUserId)));

        /// <summary>
        /// Compiled query for retrieving current user organization details.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, Int32, IQueryable<Organization>> CompiledCurrentUserOrganizationDetails =
            CompiledQuery.Compile<SysXAppDBEntities, Int32, IQueryable<Organization>>((dbNavigation, currentUserOrganizationId) => dbNavigation.Organizations.Where(condition => (condition.OrganizationID == currentUserOrganizationId && condition.ParentOrganizationID == null)));

        /// <summary>
        /// Compiled query for retrieving current user product details.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, Int32, IQueryable<TenantProduct>> CompiledCurrentUserProductDetails =
            CompiledQuery.Compile<SysXAppDBEntities, Int32, IQueryable<TenantProduct>>((dbNavigation, currentUserOrganizationTenantId) => dbNavigation.TenantProducts.Where(condition => condition.TenantID == currentUserOrganizationTenantId));

        /// <summary>
        /// Compiled query for retrieving role information's based on tenant product id and current user created by ID.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, Int32, Int32, IQueryable<RoleDetail>> CompiledRoleDetailsByCurrentUserProductTenantProductIdAndCurrentUserCreatedById =
            CompiledQuery.Compile<SysXAppDBEntities, Int32, Int32, IQueryable<RoleDetail>>((dbNavigation, currentUserProductTenantProductId, currentUserCreatedById) => dbNavigation.RoleDetails.Include(SysXEntityConstants.TABLE_TENANT_PRODUCT_DOT_TENANT)
                                                                                                                                                                                                .Include(SysXEntityConstants.TABLE_ORGANIZATION_USER)
                                                                                                                                                                                                .Where(role => role.IsActive && role.IsDeleted == false && role.ProductID == currentUserProductTenantProductId) //removed role.CreatedByID != currentUserCreatedById to display all roles related to that client admin
                                                                                                                                                                                                .OrderBy(condition => condition.CreatedOn));
        #endregion

        #endregion

        #region Manage Aspnet Users

        /// <summary>
        /// Retrieves a list of all Aspnet Users.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, IQueryable<aspnet_Users>> CompiledGetAspnetUsers =
            CompiledQuery.Compile<SysXAppDBEntities, IQueryable<aspnet_Users>>(dbNavigation => dbNavigation.aspnet_Users);

        /// <summary>
        /// Retrieves a list of Aspnet User based on UserId.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, String, aspnet_Users> CompiledGetAspnetUser =
            CompiledQuery.Compile<SysXAppDBEntities, String, aspnet_Users>((dbNavigation, userId) => dbNavigation.aspnet_Users.Include(SysXEntityConstants.TABLE_ASPNET_ROLES_ROLEDETAIL).FirstOrDefault(aspnetUsers => aspnetUsers.UserId == new Guid(userId)));

        #endregion

        #region Manage Organization Users

        /// <summary>
        /// Retrieves organization user information based on user id.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, String, IQueryable<OrganizationUser>> CompiledGetOrganizationUserInfoByUserId =
            CompiledQuery.Compile<SysXAppDBEntities, String, IQueryable<OrganizationUser>>((dbNavigation, userId) => dbNavigation.OrganizationUsers.Include(SysXEntityConstants.TABLE_ASPNET_USERS)
                                                                                                                                   .Include(SysXEntityConstants.TABLE_ASPNET_USERS_DOT_ASPNET_MEMBERSHIP)
                                                                                                                                   .Include(SysXEntityConstants.TABLE_ORGANIZATION).Where(condition => condition.UserID == new Guid(userId) && condition.IsDeleted == false));

        /// <summary>
        /// Retrieves the organization user details by User name.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, String, OrganizationUser> CompiledGetOrganizationUsersDetailsByUserName =
            CompiledQuery.Compile<SysXAppDBEntities, String, OrganizationUser>((dbNavigation, userName) => dbNavigation.OrganizationUsers.Include(SysXEntityConstants.TABLE_ASPNET_USERS)
                                                                                                                                   .Include(SysXEntityConstants.TABLE_ASPNET_USERS_DOT_ASPNET_MEMBERSHIP)
                                                                                                                                   .Include(SysXEntityConstants.TABLE_ORGANIZATION).Where(condition => condition.aspnet_Users.UserName == userName && condition.IsDeleted == false).FirstOrDefault());

        /// <summary>
        /// Retrieves organization user details.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, OrganizationUser, OrganizationUser> CompiledGetOrganizationUsersDetails =
            CompiledQuery.Compile<SysXAppDBEntities, OrganizationUser, OrganizationUser>((dbNavigation, organizationUser) => dbNavigation.OrganizationUsers.Include(SysXEntityConstants.TABLE_ASPNET_USERS)
                                                                                                                                   .Include(SysXEntityConstants.TABLE_ASPNET_USERS_DOT_ASPNET_MEMBERSHIP)
                                                                                                                                   .Include(SysXEntityConstants.TABLE_ORGANIZATION).Where(condition => condition.aspnet_Users.UserId == organizationUser.UserID && condition.IsDeleted == false).FirstOrDefault());

        /// <summary>
        /// Retrieves organization user by Email address.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, String, IQueryable<OrganizationUser>> CompiledGetOrganizationUsersByEmail =
           CompiledQuery.Compile<SysXAppDBEntities, String, IQueryable<OrganizationUser>>((dbNavigation, email) => dbNavigation.OrganizationUsers.Include(SysXEntityConstants.TABLE_ASPNET_USERS)
                                                                                                                                  .Include(SysXEntityConstants.TABLE_ASPNET_USERS_DOT_ASPNET_MEMBERSHIP)
                                                                                                                                  .Include(SysXEntityConstants.TABLE_ORGANIZATION).Where(organizationUserDetails => organizationUserDetails.aspnet_Users.aspnet_Membership.Email == email && organizationUserDetails.IsDeleted.Equals(false) && organizationUserDetails.IsActive.Equals(true)));

        /// <summary>
        /// Retrieves a list of all Organization User based on OrganizationUserID
        /// </summary>
        public static readonly Func<SysXAppDBEntities, Int32, OrganizationUser> CompiledGetOrganizationUser =
            CompiledQuery.Compile<SysXAppDBEntities, Int32, OrganizationUser>((dbNavigation, organizationUserId) => dbNavigation.OrganizationUsers.Include(SysXEntityConstants.TABLE_ASPNET_USERS)
                                                                                                                                                  .Include(SysXEntityConstants.TABLE_ASPNET_USERS_DOT_ASPNET_MEMBERSHIP)
                                                                                                                                                  .Include(SysXEntityConstants.TABLE_ADDRESSHANDLE_ADDRESS)
                                                                                                                                                  .Include(SysXEntityConstants.TABLE_ADDRESSHANDLE_ADDRESS_ZIPCODE)
                                                                                                                                                  .Include(SysXEntityConstants.TABLE_RESIDENTIALHISTORIES)
                                                                                                                        .FirstOrDefault(orgUser => orgUser.OrganizationUserID == organizationUserId && orgUser.IsDeleted == false));

        /// <summary>
        /// Retrieves a list of Organization User detail.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, Int32, OrganizationUser> CompiledGetOrganizationUserDetail =
            CompiledQuery.Compile<SysXAppDBEntities, Int32, OrganizationUser>((dbNavigation, organizationUserId) => dbNavigation.OrganizationUsers.Include(SysXEntityConstants.TABLE_ADDRESSHANDLE_ADDRESS_ZIPCODE_CITY_STATE)
                                                                                                                        .FirstOrDefault(orgUser => orgUser.OrganizationUserID == organizationUserId && orgUser.IsDeleted == false));


        /// <summary>
        /// Retrieves Organization User based on VerificationCode
        /// </summary>
        public static readonly Func<SysXAppDBEntities, String, OrganizationUser> CompiledGetOrganizationUserByVerificationCode =
            CompiledQuery.Compile<SysXAppDBEntities, String, OrganizationUser>((dbNavigation, userVerificationCode) => dbNavigation.OrganizationUsers.Include(SysXEntityConstants.TABLE_ASPNET_USERS)
                                                                                                                                                    .Include(SysXEntityConstants.TABLE_ASPNET_USERS_DOT_ASPNET_MEMBERSHIP)
                                                                                                                                                     .Include(SysXEntityConstants.TABLE_ADDRESSHANDLE_ADDRESS)
                                                                                                                        .FirstOrDefault(orgUser => orgUser.UserVerificationCode == userVerificationCode && orgUser.IsDeleted == false && orgUser.IsActive == false));

        #region GetMappedOrganizationUsers Compiled Query

        /// <summary>
        /// Compiled query to retrieve mapped organization users for system Admin.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, IQueryable<OrganizationUser>> CompiledGetMappedOrganizationUsersForSystemAdmin =
            CompiledQuery.Compile<SysXAppDBEntities, IQueryable<OrganizationUser>>(dbNavigation => dbNavigation.OrganizationUsers.Include(SysXEntityConstants.TABLE_ASPNET_USERS)
                                                                                                                                   .Include(SysXEntityConstants.TABLE_ASPNET_USERS_DOT_ASPNET_MEMBERSHIP)
                                                                                                                                   .Include(SysXEntityConstants.TABLE_ORGANIZATION)
                                                                                                                                   .Include(SysXEntityConstants.TABLE_ORGANIZATION_TENANT)
                                                                                                                                   .Include(SysXEntityConstants.TABLE_ORGANIZATION_TENANT_LKPTENANTTYPE)
                                                                                                         .Where(
                                                                                                             organizationUserDetails => organizationUserDetails.IsDeleted == false && organizationUserDetails.Organization.IsDeleted == false).OrderBy(condition => condition.CreatedOn));

        /// <summary>
        /// Compiled Query to retrieve all departments based on current user's OrganizatioID for Product Admin.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, Int32, IQueryable<Organization>> CompiledGetDepartmentforProductAdmin =
            CompiledQuery.Compile<SysXAppDBEntities, Int32, IQueryable<Organization>>((dbNavigation, currentUserOrganizationOrganizationId) => dbNavigation.Organizations.Where(organizationDetails => organizationDetails.ParentOrganizationID == currentUserOrganizationOrganizationId));

        /// <summary>
        /// Compiled Query to retrieve all departments based on current user's OrganizatioID for Department Admin.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, Int32, IQueryable<Organization>> CompiledGetDepartmentforDepartmentAdmin =
            CompiledQuery.Compile<SysXAppDBEntities, Int32, IQueryable<Organization>>((dbNavigation, currentUserId) => dbNavigation.Organizations.Where(condition => condition.CreatedByID == currentUserId && condition.IsActive && condition.IsDeleted == false));

        /// <summary>
        /// Compiled query to retrieve mapped organization users for client Admin.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, Int32, IQueryable<OrganizationUser>> CompiledGetMappedOrganizationUsersForClientAdmin =
             CompiledQuery.Compile<SysXAppDBEntities, Int32, IQueryable<OrganizationUser>>((dbNavigation, currentUserOrganizationId) => dbNavigation.OrganizationUsers.Include(SysXEntityConstants.TABLE_ASPNET_USERS)
                                                                                                                                  .Include(SysXEntityConstants.TABLE_ASPNET_USERS_DOT_ASPNET_MEMBERSHIP)
                                                                                                                                  .Include(SysXEntityConstants.TABLE_ORGANIZATION)
                                                                                                                                  .Include(SysXEntityConstants.TABLE_ORGANIZATION_TENANT)
                                                                                                                                  .Include(SysXEntityConstants.TABLE_ORGANIZATION_TENANT_LKPTENANTTYPE)
                                                                                                                                  .Where(organizationUserDetails =>
                                                                                                                                  organizationUserDetails.OrganizationID == currentUserOrganizationId &&
                                                                                                                                  organizationUserDetails.IsDeleted == false && organizationUserDetails.Organization.IsDeleted == false)
                                                                                                                                  .OrderBy(organizationUserDetails => organizationUserDetails.CreatedOn));

        #endregion

        #endregion

        #region Manage SysXLineOfBusinessFeatures

        /// <summary>
        /// Retrieves a list of all block features based on sysXBlockID.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, Int32, SysXBlocksFeature> CompiledGetSysXBlockFeatures =
            CompiledQuery.Compile<SysXAppDBEntities, Int32, SysXBlocksFeature>((dbNavigation, sysXBlockId) => dbNavigation.SysXBlocksFeatures.Include(SysXEntityConstants.TABLE_PRODUCT_FEATURE)
                                                                                                                                                .Include(SysXEntityConstants.TABLE_PRODUCT_FEATURE_DOT_PRODUCT_FEATURE2)
                                                                                                                                                .Include(SysXEntityConstants.TABLE_SYSX_BLOCK).FirstOrDefault(block => block.SysXBlockFeatureID == sysXBlockId));

        /// <summary>
        ///  Retrieves SysX block features.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, Int32, Int32, SysXBlocksFeature> CompiledGetSysXBlockFeature =
            CompiledQuery.Compile<SysXAppDBEntities, Int32, Int32, SysXBlocksFeature>((dbNavigation, blockId, featureId) => dbNavigation.SysXBlocksFeatures.FirstOrDefault(block => block.SysXBlockID == blockId && block.FeatureID == featureId));

        #endregion

        #region Manage User Role Mapping

        #endregion

        #region  Manage Role Permission ProductFeatures

        public static readonly Func<SysXAppDBEntities, Int32, IQueryable<RolePermissionProductFeature>> CompiledGetProductFeatureRole =
            CompiledQuery.Compile<SysXAppDBEntities, Int32, IQueryable<RolePermissionProductFeature>>((dbNavigation, productFeatureId) => dbNavigation.RolePermissionProductFeatures.Include(SysXEntityConstants.TABLE_ROLE_DETAIL)
                                                                                                                                                             .Where(rpp => rpp.SysXBlocksFeature.FeatureID == productFeatureId && rpp.RoleDetail.IsActive && rpp.RoleDetail.IsDeleted == false));



        /// <summary>
        /// Retrieves a list of all features associated with the current role.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, String, Int32, IQueryable<RolePermissionProductFeature>> CompiledGetFeatureForRole =
            CompiledQuery.Compile<SysXAppDBEntities, String, Int32, IQueryable<RolePermissionProductFeature>>((dbNavigation, roleDetailId, productId) => dbNavigation.RolePermissionProductFeatures.Include(SysXEntityConstants.TABLE_SYSX_BLOCKS_FEATURE)
                                                                                                                                                                                                    .Include(SysXEntityConstants.TABLE_SYSX_BLOCKS_FEATURE_DOT_PRODUCT_FEATURE)
                                                                                                                                                                                                    .Include(SysXEntityConstants.TABLE_PERMISSION).Include("FeatureRoleActions")
                                                                                                                                                             .Where(rolePermissionProductFeature => rolePermissionProductFeature.RoleId == new Guid(roleDetailId) && rolePermissionProductFeature.SysXBlocksFeature.SysXBlockID == productId
                                                                                                                                                             && !rolePermissionProductFeature.SysXBlocksFeature.ProductFeature.IsDeleted));

        /// <summary>
        /// Retrieves a list of all products for each tenant based on tenantID.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, Int32, IQueryable<TenantProduct>> CompiledGetProductsForTenant =
            CompiledQuery.Compile<SysXAppDBEntities, Int32, IQueryable<TenantProduct>>((dbNavigation, tenantId) => dbNavigation.TenantProducts.Include(SysXEntityConstants.TABLE_TENANT)
                                                                                                                                               .Include(SysXEntityConstants.TABLE_ROLE_DETAILS).Where(prod => prod.Tenant.TenantID == tenantId && prod.IsActive && prod.IsDeleted == false).OrderBy(condition => condition.TenantProductID).Take(1));

        /// <summary>
        /// Returns permission for each features.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, Guid, Int32, RolePermissionProductFeature> CompiledGetRolePermissionFeatures =
            CompiledQuery.Compile<SysXAppDBEntities, Guid, Int32, RolePermissionProductFeature>((dbNavigation, roleId, sysXBlockFeatureId) => dbNavigation.RolePermissionProductFeatures.FirstOrDefault(rolePermissionProductFeatures => rolePermissionProductFeatures.RoleId == roleId && rolePermissionProductFeatures.SysXBlockFeatureId == sysXBlockFeatureId));

        public static readonly Func<SysXAppDBEntities, Int32, IQueryable<ProductFeature>> CompiledGetChidlProductFeature =
            CompiledQuery.Compile<SysXAppDBEntities, Int32, IQueryable<ProductFeature>>((dbnavigation, productFeatureId) => dbnavigation.ProductFeatures.Where(feature => feature.ParentProductFeatureID == productFeatureId));
        #endregion

        #region Manage Policies

        /// <summary>
        /// Retrieves a list of all policy register controls.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, IQueryable<PolicyRegisterUserControl>> CompiledGetPolicyRegisterControls =
            CompiledQuery.Compile<SysXAppDBEntities, IQueryable<PolicyRegisterUserControl>>(dbNavigation => dbNavigation.PolicyRegisterUserControls.Include(SysXEntityConstants.TABLE_POLICY_REGISTER_USER_CONTROL2));

        /// <summary>
        ///  Retrieves the value for selected control.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, String, Int32, PolicySetUserControl> CompiledGetControlSelectedValues =
            CompiledQuery.Compile<SysXAppDBEntities, String, Int32, PolicySetUserControl>((dbNavigation, roleId, registerUserControlId) => dbNavigation.PolicySetUserControls.Include(SysXEntityConstants.TABLE_POLICIES)
                                                                                                                                                                              .Include(SysXEntityConstants.TABLE_POLICIES_DOT_POLICY_PROPERTIES)
                                                                                                                                                                              .Include(SysXEntityConstants.TABLE_POLICY_REGISTER_USER_CONTROL).FirstOrDefault(c => c.PolicySet.RoleID == new Guid(roleId) && c.PolicyRegisterUserControl.RegisterUserControlID == registerUserControlId));
        /// <summary>
        /// Retrieves the set of policies based on role's id.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, String, PolicySet> CompiledGetPolicySetByRoleId =
            CompiledQuery.Compile<SysXAppDBEntities, String, PolicySet>((dbNavigation, roleId) => dbNavigation.PolicySets.Where(ps => ps.RoleID == new Guid(roleId)).FirstOrDefault());

        /// <summary>
        ///  Retrieves the list of all policy controls.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, IQueryable<PolicyControl>> CompiledGetRegisteredControlList =
            CompiledQuery.Compile<SysXAppDBEntities, IQueryable<PolicyControl>>(dbNavigation => dbNavigation.PolicyControls.Include(SysXEntityConstants.TABLE_POLICY_CONTROL_PROPERTY_TYPES)
                                                                                                                            .Include(SysXEntityConstants.TABLE_POLICY_CONTROL_PROPERTY_TYPES_DOT_POLICY_PROPERTY_TYPE));

        #endregion

        #region Manage Policy Register Control

        /// <summary>
        ///  Retrieves policy register control.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, Int32, PolicyRegisterUserControl> CompiledGetPolicyRegisterControl =
            CompiledQuery.Compile<SysXAppDBEntities, Int32, PolicyRegisterUserControl>((dbNavigation, policyRegisterControlId) => dbNavigation.PolicyRegisterUserControls.FirstOrDefault(policyRegisterUserControls => policyRegisterUserControls.RegisterUserControlID == policyRegisterControlId));

        /// <summary>
        /// Chooses the child policy control.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, long, Int32> CompiledSelectChildPolicyControls =
            CompiledQuery.Compile<SysXAppDBEntities, long, Int32>((dbNavigation, registerUserControlId) => dbNavigation.PolicyRegisterUserControls.Count(policyRegisterUserControls => policyRegisterUserControls.ParentUserControlID == registerUserControlId));

        #endregion

        #region Manage Aspnet Roles

        /// <summary>
        /// Get the Aspnet Role based on RoleId.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, String, aspnet_Roles> CompiledGetAspnetRole =
            CompiledQuery.Compile<SysXAppDBEntities, String, aspnet_Roles>((dbNavigation, roleId) => dbNavigation.aspnet_Roles.FirstOrDefault(aspnetRoles => aspnetRoles.RoleId == new Guid(roleId)));

        #endregion

        #region Manage FeaturePermissions

        /// <summary>
        /// Retrieves all feature permissions.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, Int32, FeaturePermission> CompiledGetFeaturePermission =
            CompiledQuery.Compile<SysXAppDBEntities, Int32, FeaturePermission>((dbNavigation, featurePermissionId) => dbNavigation.FeaturePermissions.FirstOrDefault(featurePermissions => featurePermissions.PermissionID == featurePermissionId));

        #endregion

        #region Manage Departments

        /// <summary>
        /// Retrieves all departments for Super Admin.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, IQueryable<Organization>> CompiledGetDepartmentsForSuperAdmin =
            CompiledQuery.Compile<SysXAppDBEntities, IQueryable<Organization>>(dbNavigation => dbNavigation.Organizations.Where(organizationDetails => organizationDetails.IsActive && organizationDetails.OrganizationID != AppConsts.NONE && organizationDetails.IsDeleted == false && organizationDetails.ParentOrganizationID != null));

        /// <summary>
        /// Retrieves a list of all departments for Product admin.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, Int32, IQueryable<Organization>> CompiledGetDepartmentsForProductAdmin =
            CompiledQuery.Compile<SysXAppDBEntities, Int32, IQueryable<Organization>>((dbNavigation, compiledGetOrganizationsForProduct) => dbNavigation.Organizations.Include(SysXEntityConstants.TABLE_TENANT).Include(SysXEntityConstants.TABLE_TENANT_LKPTENANTTYPE).Where(organizationDetails => organizationDetails.IsActive && organizationDetails.OrganizationID != AppConsts.NONE &&
                                                                                                                                                                                                                                        organizationDetails.IsDeleted == false &&
                                                                                                                                                                                                                                        organizationDetails.Tenant.TenantID == compiledGetOrganizationsForProduct && organizationDetails.ParentOrganizationID != null).OrderBy(organizationDetails => organizationDetails.CreatedOn));
        /// <summary>
        /// Retrieves a list of all departments for Product admin.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, Int32, Int32, IQueryable<Organization>> CompiledGetDepartmentsForDepartmentAdmin =
            CompiledQuery.Compile<SysXAppDBEntities, Int32, Int32, IQueryable<Organization>>((dbNavigation, compiledGetOrganizationsForProduct, currentUserId) => dbNavigation.Organizations.Include(SysXEntityConstants.TABLE_TENANT).Where(organizationDetails => organizationDetails.IsActive && organizationDetails.OrganizationID != AppConsts.NONE &&
                                                                                                                                                                                                                                        organizationDetails.IsDeleted == false &&
                                                                                                                                                                                                                                        organizationDetails.Tenant.TenantID == compiledGetOrganizationsForProduct && organizationDetails.ParentOrganizationID != null && organizationDetails.CreatedByID == currentUserId).OrderBy(organizationDetails => organizationDetails.CreatedOn));

        #endregion

        #region UserGroup

        /// <summary>
        /// Retrive all user group
        /// </summary>
        public static readonly Func<SysXAppDBEntities, IQueryable<UserGroup>> CompiledGetAllUserGroup =
            CompiledQuery.Compile<SysXAppDBEntities, IQueryable<UserGroup>>(dbNavigation => dbNavigation.UserGroups.Include("UsersInUserGroups").Include("RolesInUserGroups.aspnet_Roles").Where(condtion => condtion.IsActive == true));

        /// <summary>
        /// Retrive all users in a user group.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, Int32, IQueryable<UsersInUserGroup>> CompiledGetAllUserInAGroup =
           CompiledQuery.Compile<SysXAppDBEntities, Int32, IQueryable<UsersInUserGroup>>((dbNavigation, userGroupId) => dbNavigation.UsersInUserGroups.Include("aspnet_Users.OrganizationUsers.Organization.Tenant.lkpTenantType")
                                .Where(condtion => condtion.IsActive == true && condtion.UserGroupID == userGroupId));



        /// <summary>
        /// Reterive permission of user group.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, Guid, Int32, Int32, IQueryable<Permission>> CompiledGetUserGroupPermission =
            CompiledQuery.Compile<SysXAppDBEntities, Guid, Int32, Int32, IQueryable<Permission>>((dbNavigation, UserId, featureId, SysxBlockId) => from c in dbNavigation.RolesInUserGroups
                                                                                                                                                   join ug in dbNavigation.UsersInUserGroups on c.UserGroupID equals ug.UserGroupID
                                                                                                                                                   join rp in dbNavigation.RolePermissionProductFeatures on c.RoleID equals rp.RoleId
                                                                                                                                                   join sys in dbNavigation.SysXBlocksFeatures on rp.SysXBlockFeatureId equals sys.SysXBlockFeatureID
                                                                                                                                                   join role in dbNavigation.aspnet_Roles on c.RoleID equals role.RoleId
                                                                                                                                                   join permission in dbNavigation.Permissions on rp.PermissionId equals permission.PermissionId
                                                                                                                                                   where sys.FeatureID == featureId && sys.SysXBlockID == SysxBlockId && ug.UserID == UserId
                                                                                                                                                   select permission);

        /// <summary>
        /// Reterive permission of user of user group
        /// </summary>
        public static readonly Func<SysXAppDBEntities, Guid, Int32, Int32, IQueryable<Permission>> CompiledGetUserGroupUserPermission =
           CompiledQuery.Compile<SysXAppDBEntities, Guid, Int32, Int32, IQueryable<Permission>>((dbNavigation, UserId, featureId, SysxBlockId) => from c in dbNavigation.UsersInUserGroups
                                                                                                                                                  join rpf in dbNavigation.UserGroupRolePermissionProductFeatures on c.UsersInUserGroupID equals rpf.UsersInUserGroupID
                                                                                                                                                  join role in dbNavigation.RolesInUserGroups on rpf.RolesInUserGroupID equals role.RolesInUserGroupID
                                                                                                                                                  join rolepermission in dbNavigation.RolePermissionProductFeatures on role.RoleID equals rolepermission.RoleId
                                                                                                                                                  join sys in dbNavigation.SysXBlocksFeatures on rolepermission.SysXBlockFeatureId equals sys.SysXBlockFeatureID
                                                                                                                                                  join permission in dbNavigation.Permissions on rolepermission.PermissionId equals permission.PermissionId
                                                                                                                                                  where c.UserID == UserId && sys.FeatureID == featureId && sys.SysXBlockID == SysxBlockId
                                                                                                                                                  select permission);
        /// <summary>
        /// Reterive permission of user of user group
        /// </summary>
        public static readonly Func<SysXAppDBEntities, Int32, IQueryable<aspnet_Roles>> CompiledGetUserGroupRoles =
           CompiledQuery.Compile<SysXAppDBEntities, Int32, IQueryable<aspnet_Roles>>((dbNavigation, UserGroupId) => from c in dbNavigation.RolesInUserGroups
                                                                                                                    join role in dbNavigation.aspnet_Roles.Include("Aspnet_Roles.RoleDetail") on c.RoleID equals role.RoleId
                                                                                                                    where c.UserGroupID == UserGroupId && role.RoleDetail.IsUserGroupLevel == true
                                                                                                                    select role);

        /// <summary>
        /// Reterive permission of user of user group
        /// </summary>
        public static readonly Func<SysXAppDBEntities, Int32, Guid, IQueryable<UserGroupRolePermissionProductFeature>> CompiledGetUserGroupRolePermissionProductFeature =
           CompiledQuery.Compile<SysXAppDBEntities, Int32, Guid, IQueryable<UserGroupRolePermissionProductFeature>>((dbNavigation, UserGroupId, UserId) => from c in dbNavigation.UsersInUserGroups
                                                                                                                                                           join feature in dbNavigation.UserGroupRolePermissionProductFeatures on c.UsersInUserGroupID equals feature.UsersInUserGroupID
                                                                                                                                                           where c.UserGroupID == UserGroupId && c.UserID == UserId && c.IsActive == true
                                                                                                                                                           select feature);
        /// <summary>
        /// Reterive permission of user of user group
        /// </summary>
        public static readonly Func<SysXAppDBEntities, Guid, IQueryable<aspnet_Roles>> CompiledGetUserRoleInUserGroup =
           CompiledQuery.Compile<SysXAppDBEntities, Guid, IQueryable<aspnet_Roles>>((dbNavigation, UserId) => from c in dbNavigation.UsersInUserGroups
                                                                                                              join feature in dbNavigation.UserGroupRolePermissionProductFeatures on c.UsersInUserGroupID equals feature.UsersInUserGroupID
                                                                                                              join userrole in dbNavigation.RolesInUserGroups on feature.RolesInUserGroupID equals userrole.RolesInUserGroupID
                                                                                                              join role in dbNavigation.aspnet_Roles.Include("Aspnet_Roles.RoleDetail") on userrole.RoleID equals role.RoleId
                                                                                                              where c.UserID == UserId && c.IsActive == true && role.RoleDetail.IsUserGroupLevel == true
                                                                                                              select role);
        /// <summary>
        /// Compiled query to retrieve AspnetRoles details based on tenantProductID.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, Int32, IQueryable<aspnet_Roles>> CompiledGetAspnetRolesByTenantProductIdInUserGroup =
            CompiledQuery.Compile<SysXAppDBEntities, Int32, IQueryable<aspnet_Roles>>((dbNavigation, tenantProductId) => dbNavigation.aspnet_Roles.Include(SysXEntityConstants.TABLE_ROLE_DETAIL).Where(role => role.RoleDetail.IsActive && role.RoleDetail.IsDeleted == false && role.RoleDetail.ProductID == tenantProductId && role.RoleDetail.IsUserGroupLevel));

        /// <summary>
        /// Compiled query to retrieve AspnetRoles details based on tenantProductID.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, Guid, Int32, IQueryable<UserGroupRolePermissionProductFeature>> CompiledGetUserGroupRolePermissionProductFeatureByRoleId =
            CompiledQuery.Compile<SysXAppDBEntities, Guid, Int32, IQueryable<UserGroupRolePermissionProductFeature>>((dbNavigation, Roleid, UserGroupId) => from feature in dbNavigation.UserGroupRolePermissionProductFeatures
                                                                                                                                                            join role in dbNavigation.RolesInUserGroups on feature.RolesInUserGroupID equals role.RolesInUserGroupID
                                                                                                                                                            where role.UserGroupID == UserGroupId && role.RoleID == Roleid
                                                                                                                                                            select feature);

        /// <summary>
        /// Compiled query to retrieve configured role of user group assigned to a user.
        /// </summary>
        public static readonly Func<SysXAppDBEntities, Int32, IQueryable<RolesInUserGroup>> CompiledUserGroupAssignedRole =
            CompiledQuery.Compile<SysXAppDBEntities, Int32, IQueryable<RolesInUserGroup>>((dbNavigation, UserGroupId) => from c in dbNavigation.RolesInUserGroups
                                                                                                                         join role in dbNavigation.UserGroupRolePermissionProductFeatures on c.RolesInUserGroupID equals role.RolesInUserGroupID
                                                                                                                         where c.UserGroupID == UserGroupId
                                                                                                                         select c);


        #endregion

        #region Address
        /// <summary>
        /// Compiled query to return ZipCode by zipCodeNumber
        /// </summary>        
        public static readonly
         Func<SysXAppDBEntities, String, IQueryable<ZipCode>> CompiledGetCityState
                                                     = CompiledQuery.Compile<SysXAppDBEntities, String, IQueryable<ZipCode>>
                                                       (
                                                       (dbNavigation, zipCodeNumber)
                                                       => (from zipCode in dbNavigation.ZipCodes
                                                            .Include(SysXEntityConstants.TABLE_CITY)
                                                            .Include(SysXEntityConstants.TABLE_CITY_STATE)
                                                            .Include(SysXEntityConstants.TABLE_COUNTY)
                                                            .Where(zipCodeDetails => zipCodeDetails.ZipCode1.Equals(zipCodeNumber))
                                                           select zipCode)
                                                       );

        /// <summary>
        /// Compiled query to return ZipCode by zipCodeNumber
        /// </summary>
        public static readonly
        Func<SysXAppDBEntities, Int32, IQueryable<ZipCode>> CompiledGetCityStateByID
                                                    = CompiledQuery.Compile<SysXAppDBEntities, Int32, IQueryable<ZipCode>>
                                                      (
                                                      (dbNavigation, zipCodeID)
                                                      => (from zipCode in dbNavigation.ZipCodes
                                                           .Include(SysXEntityConstants.TABLE_CITY)
                                                           .Include(SysXEntityConstants.TABLE_CITY_STATE)
                                                           .Include(SysXEntityConstants.TABLE_COUNTY)
                                                           .Include(SysXEntityConstants.TABLE_CITY_STATE_COUNTRY)
                                                           .Where(zipCodeDetails => zipCodeDetails.ZipCodeID == zipCodeID)
                                                          select zipCode)
                                                      );
        #endregion


        #endregion

        #region User Order placement Related Functionality

        public void UpdateApplicanDetailsMaster(OrganizationUser orgUser, Dictionary<String, Object> dicAddressData, out Int32 addressMasterId, List<PreviousAddressContract> lstPrevAddress, PreviousAddressContract mailingAddress, out List<ResidentialHistory> lstResendentialHistory, ref List<PersonAlia> lstPersonAlias, Boolean isLocationServiceTenant)
        {
            addressMasterId = -1;
            lstResendentialHistory = new List<ResidentialHistory>();
            OrganizationUser orgUserToUpdate = _dbNavigation.OrganizationUsers.Where(oUser => oUser.OrganizationUserID == orgUser.OrganizationUserID).FirstOrDefault();

            orgUserToUpdate.FirstName = orgUser.FirstName;
            orgUserToUpdate.MiddleName = orgUser.MiddleName;
            orgUserToUpdate.LastName = orgUser.LastName;
            orgUserToUpdate.DOB = orgUser.DOB;
            orgUserToUpdate.Gender = orgUser.Gender;
            orgUserToUpdate.Alias1 = orgUser.Alias1;
            orgUserToUpdate.Alias2 = orgUser.Alias2;
            orgUserToUpdate.Alias3 = orgUser.Alias3;
            orgUserToUpdate.SSN = orgUser.SSN;
            orgUserToUpdate.PhoneNumber = orgUser.PhoneNumber;
            orgUserToUpdate.aspnet_Users.MobileAlias = orgUser.PhoneNumber;
            orgUserToUpdate.SecondaryPhone = orgUser.SecondaryPhone;
            orgUserToUpdate.SecondaryEmailAddress = orgUser.SecondaryEmailAddress.Trim();
            orgUserToUpdate.PrimaryEmailAddress = orgUser.PrimaryEmailAddress.Trim();
            //orgUserToUpdate.aspnet_Users.aspnet_Membership.Email = orgUser.PrimaryEmailAddress;
            //orgUserToUpdate.aspnet_Users.aspnet_Membership.LoweredEmail = orgUser.PrimaryEmailAddress.ToLower();

            //orgUserToUpdate.ModifiedByID = orgUser.OrganizationUserID;
            orgUserToUpdate.ModifiedByID = orgUser.ModifiedByID;
            orgUserToUpdate.ModifiedOn = DateTime.Now;

            //UAT-2447
            orgUserToUpdate.IsInternationalPhoneNumber = orgUser.IsInternationalPhoneNumber;
            orgUserToUpdate.IsInternationalSecondaryPhone = orgUser.IsInternationalSecondaryPhone;

            //CBI|| CABS || Added Suffix ID in UserTypeID
            orgUserToUpdate.UserTypeID = !orgUser.UserTypeID.IsNullOrEmpty() ? orgUser.UserTypeID : (Int32?)null;

            Address addressNew = new Address();
            Address mailingAdressNew = new Address();
            AddressExt addressExtNew = null;
            AddressExt mailingaddressExtNew = new AddressExt();

            if (orgUserToUpdate.AddressHandle.IsNotNull())
            {
                Address userAddress = orgUserToUpdate.AddressHandle.Addresses.Where(add => add.AddressHandleID == orgUserToUpdate.AddressHandleID).FirstOrDefault();
                PreviousAddressContract currentAddress = lstPrevAddress.FirstOrDefault(x => x.isCurrent == true);
                if (userAddress.IsNotNull() && currentAddress.IsNotNull())
                {
                    if (CheckIfAddressUpdated(userAddress, currentAddress))
                    {
                        if (currentAddress.ZipCodeID == 0)
                        {
                            addressExtNew = new AddressExt();
                            //UAT-3910
                            if (isLocationServiceTenant)
                            {
                                addressExtNew.AE_CountryID = AppConsts.ONE;
                                addressExtNew.AE_County = Convert.ToString(currentAddress.CountryId);
                            }
                            else
                            {
                                addressExtNew.AE_CountryID = currentAddress.CountryId;
                            }
                            addressExtNew.AE_StateName = currentAddress.StateName;
                            addressExtNew.AE_CityName = currentAddress.CityName;
                            addressExtNew.AE_ZipCode = currentAddress.Zipcode;
                        }

                        if (mailingAddress.IsNotNull())
                        {
                            if (mailingAddress.ZipCodeID == 0)
                            {


                                if (isLocationServiceTenant)
                                {
                                    mailingaddressExtNew.AE_CountryID = AppConsts.ONE;
                                    mailingaddressExtNew.AE_County = Convert.ToString(mailingAddress.CountryId);

                                }
                                else
                                {

                                    mailingaddressExtNew.AE_CountryID = mailingAddress.CountryId;
                                }

                                mailingaddressExtNew.AE_StateName = mailingAddress.StateName;
                                mailingaddressExtNew.AE_CityName = mailingAddress.CityName;
                                mailingaddressExtNew.AE_ZipCode = currentAddress.Zipcode;

                            }
                            if (mailingAddress.IsNotNull())
                            {
                                if (mailingAddress.ZipCodeID == 0)
                                {


                                    if (isLocationServiceTenant)
                                    {
                                        mailingaddressExtNew.AE_CountryID = AppConsts.ONE;
                                        mailingaddressExtNew.AE_County = Convert.ToString(mailingAddress.CountryId);

                                    }
                                    else
                                    {

                                        mailingaddressExtNew.AE_CountryID = mailingAddress.CountryId;
                                    }

                                    mailingaddressExtNew.AE_StateName = mailingAddress.StateName;
                                    mailingaddressExtNew.AE_CityName = mailingAddress.CityName;
                                    mailingaddressExtNew.AE_ZipCode = currentAddress.Zipcode;

                                }
                            }
                        }

                        Guid addressHandleId = Guid.NewGuid();
                        AddAddressHandle(addressHandleId);
                        AddAddress(dicAddressData, addressHandleId, Convert.ToInt32(orgUser.ModifiedByID), addressNew, addressExtNew);
                        orgUserToUpdate.AddressHandleID = addressHandleId;
                        if (mailingAddress.IsNotNull())
                        {
                            Guid mailingaddressHandleId = Guid.NewGuid();
                            AddAddressHandle(mailingaddressHandleId);
                            AddMailingAddress(mailingAddress, mailingaddressHandleId, Convert.ToInt32(orgUser.ModifiedByID), mailingAdressNew, mailingaddressExtNew);
                        }
                    }
                }
            }

            if (lstPrevAddress.IsNotNull())
            {
                PreviousAddressContract currentAddress = lstPrevAddress.FirstOrDefault(x => x.isCurrent == true);
                if (currentAddress.IsNotNull())
                {
                    // List of Resedential Histories associated with the organisaion User ID.
                    ResidentialHistory currentResedentialHistory = orgUserToUpdate.ResidentialHistories.FirstOrDefault(x => x.RHI_IsCurrentAddress == true && x.RHI_IsDeleted == false);
                    if (currentResedentialHistory.IsNotNull())
                    {
                        currentResedentialHistory.Address = orgUserToUpdate.AddressHandle == null ? addressNew : orgUserToUpdate.AddressHandle.Addresses.FirstOrDefault();
                        currentResedentialHistory.RHI_ResidenceStartDate = currentAddress.ResidenceStartDate;
                        currentResedentialHistory.RHI_SequenceOrder = AppConsts.ONE;
                        //currentResedentialHistory.RHI_ModifiedByID = orgUser.OrganizationUserID; 
                        currentResedentialHistory.RHI_ModifiedByID = orgUser.ModifiedByID;
                        currentResedentialHistory.RHI_ModifiedOn = DateTime.Now;
                        currentResedentialHistory.RHI_MotherMaidenName = currentAddress.MotherName;
                        currentResedentialHistory.RHI_IdentificationNumber = currentAddress.IdentificationNumber;
                        currentResedentialHistory.RHI_DriverLicenseNumber = currentAddress.LicenseNumber;
                    }
                    else
                    {
                        currentResedentialHistory = new ResidentialHistory();
                        currentResedentialHistory.RHI_IsCurrentAddress = true;
                        currentResedentialHistory.RHI_IsPrimaryResidence = false;
                        currentResedentialHistory.RHI_ResidenceStartDate = currentAddress.ResidenceStartDate;
                        currentResedentialHistory.RHI_IsDeleted = false;
                        //currentResedentialHistory.RHI_CreatedByID = orgUser.OrganizationUserID;
                        currentResedentialHistory.RHI_CreatedByID = Convert.ToInt32(orgUser.ModifiedByID);
                        currentResedentialHistory.RHI_CreatedOn = DateTime.Now;
                        currentResedentialHistory.RHI_OrganizationUserID = orgUser.OrganizationUserID;
                        currentResedentialHistory.RHI_SequenceOrder = AppConsts.ONE;
                        currentResedentialHistory.Address = orgUserToUpdate.AddressHandle == null ? addressNew : orgUserToUpdate.AddressHandle.Addresses.FirstOrDefault();
                        currentResedentialHistory.RHI_MotherMaidenName = currentAddress.MotherName;
                        currentResedentialHistory.RHI_IdentificationNumber = currentAddress.IdentificationNumber;
                        currentResedentialHistory.RHI_DriverLicenseNumber = currentAddress.LicenseNumber;
                        _dbNavigation.ResidentialHistories.AddObject(currentResedentialHistory);
                    }
                    lstResendentialHistory.Add(currentResedentialHistory);
                }
            }

            if (lstPrevAddress.IsNotNull())
            {
                lstPrevAddress = lstPrevAddress.Where(x => x.isDeleted == true || x.isNew == true || x.isUpdated == true).ToList();
                if (lstPrevAddress.Count > 0)
                {
                    // List of Resedential Histories associated with the organisaion User ID.
                    List<ResidentialHistory> lstResedentialHistory = orgUserToUpdate.ResidentialHistories.Where(x => x.RHI_IsDeleted == false).ToList();

                    // List of Resedential Histories to be deleted.
                    List<Int32> lstResHisIDToBeDel = lstPrevAddress.Where(x => x.isDeleted == true).Select(y => y.ID).ToList();
                    List<ResidentialHistory> lstResHisToBeDel = lstResedentialHistory.Where(x => lstResHisIDToBeDel.Contains(x.RHI_ID)).ToList();
                    foreach (var prevAddress in lstResHisToBeDel)
                    {
                        prevAddress.RHI_IsDeleted = true;
                        //prevAddress.RHI_ModifiedByID = orgUser.OrganizationUserID;
                        prevAddress.RHI_ModifiedByID = orgUser.ModifiedByID;
                        prevAddress.RHI_ModifiedOn = DateTime.Now;
                        lstResendentialHistory.Add(prevAddress);
                    }

                    // List of Resedential Histories to be added.
                    List<PreviousAddressContract> lstResHisIDToBeAdded = lstPrevAddress.Where(x => x.isNew == true).ToList();
                    foreach (var prevAddress in lstResHisIDToBeAdded)
                    {
                        Guid addressHandleId = Guid.NewGuid();
                        AddAddressHandle(addressHandleId);
                        lstResendentialHistory.Add(AddResidentialHistoryAddress(prevAddress, addressHandleId, orgUser.OrganizationUserID, Convert.ToInt32(orgUser.ModifiedByID)));
                    }

                    // List of Resedential Histories to be updated.
                    List<PreviousAddressContract> lstResHisToBeUpdated = lstPrevAddress.Where(x => x.isUpdated == true).ToList();
                    foreach (var prevAddress in lstResHisToBeUpdated)
                    {
                        ResidentialHistory resHistory = lstResedentialHistory.FirstOrDefault(x => x.RHI_ID == prevAddress.ID && x.RHI_IsDeleted == false);
                        if (resHistory.IsNotNull())
                        {
                            if (CheckIfAddressUpdated(resHistory.Address, prevAddress))
                            {
                                Guid addressHandleId = Guid.NewGuid();
                                AddAddressHandle(addressHandleId);
                                //Address PreviousAddressNew = AddPreviousAddress(prevAddress, addressHandleId, orgUser.OrganizationUserID);
                                Address PreviousAddressNew = AddPreviousAddress(prevAddress, addressHandleId, Convert.ToInt32(orgUser.ModifiedByID));
                                resHistory.Address = PreviousAddressNew;
                            }
                            resHistory.RHI_SequenceOrder = prevAddress.ResHistorySeqOrdID;
                            resHistory.RHI_ResidenceStartDate = prevAddress.ResidenceStartDate;
                            resHistory.RHI_ResidenceEndDate = prevAddress.ResidenceEndDate;
                            //resHistory.RHI_ModifiedByID = orgUser.OrganizationUserID;
                            resHistory.RHI_ModifiedByID = orgUser.ModifiedByID;
                            resHistory.RHI_ModifiedOn = DateTime.Now;
                            resHistory.RHI_MotherMaidenName = prevAddress.MotherName;
                            resHistory.RHI_IdentificationNumber = prevAddress.IdentificationNumber;
                            resHistory.RHI_DriverLicenseNumber = prevAddress.LicenseNumber;
                            lstResendentialHistory.Add(resHistory);
                        }
                    }
                }
            }

            if (lstPersonAlias.IsNotNull())
            {
                List<PersonAlia> currentAliasList = orgUserToUpdate.PersonAlias.Where(x => x.PA_IsDeleted == false).ToList();
                foreach (PersonAlia tempPersonAlias in lstPersonAlias)
                {
                    if (tempPersonAlias.PA_ID > 0)
                    {
                        PersonAlia personAlias = currentAliasList.FirstOrDefault(x => x.PA_IsDeleted == false && x.PA_ID == tempPersonAlias.PA_ID);
                        List<PersonAliasExtension> lstPersonAliasExtension = orgUserToUpdate.PersonAlias
                                                                .FirstOrDefault(x => x.PA_IsDeleted == false && x.PA_ID == tempPersonAlias.PA_ID)
                                                                .PersonAliasExtensions.Where(cond => !cond.PAE_IsDeleted).ToList();
                        //.FirstOrDefault().PersonAliasExtensions;
                        if (personAlias.IsNotNull())
                        {
                            personAlias.PA_FirstName = tempPersonAlias.PA_FirstName;
                            personAlias.PA_LastName = tempPersonAlias.PA_LastName;
                            //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                            personAlias.PA_MiddleName = tempPersonAlias.PA_MiddleName;
                            //personAlias.PA_ModifiedBy = orgUser.OrganizationUserID;
                            personAlias.PA_ModifiedBy = orgUser.ModifiedByID;
                            personAlias.PA_ModifiedOn = DateTime.Now;
                            tempPersonAlias.PA_AliasIdentifier = personAlias.PA_AliasIdentifier;

                            //Cbi||CABS

                            PersonAliasExtension personAliasExtension = lstPersonAliasExtension.Where(cond => !cond.PAE_IsDeleted).FirstOrDefault();
                            var data = tempPersonAlias.PersonAliasExtensions.Where(cond => cond.PAE_PersonAliasID == tempPersonAlias.PA_ID).FirstOrDefault();
                            Int32 currentUserId = Convert.ToInt32(orgUser.ModifiedByID);
                            if (!data.IsNullOrEmpty())
                            {
                                AddDeletePersonAliasExtension(data, currentUserId, false);
                            }
                            else
                            {
                                AddDeletePersonAliasExtension(personAliasExtension, currentUserId, true);
                            }
                        }
                    }
                    else
                    {
                        tempPersonAlias.PA_AliasIdentifier = Guid.NewGuid();
                        //tempPersonAlias.PA_CreatedBy = orgUser.OrganizationUserID;
                        tempPersonAlias.PA_CreatedBy = Convert.ToInt32(orgUser.ModifiedByID);
                        tempPersonAlias.PA_CreatedOn = DateTime.Now;
                        //Cbi||CABS
                        if (!tempPersonAlias.PersonAliasExtensions.IsNullOrEmpty() && !tempPersonAlias.PersonAliasExtensions.FirstOrDefault().PAE_Suffix.IsNullOrEmpty())
                        {
                            tempPersonAlias.PersonAliasExtensions.FirstOrDefault().PAE_CreatedBy = Convert.ToInt32(orgUser.ModifiedByID);
                            tempPersonAlias.PersonAliasExtensions.FirstOrDefault().PAE_CreatedOn = DateTime.Now;
                            // AddDeletePersonAliasExtension(tempPersonAlias.PersonAliasExtensions.FirstOrDefault(), Convert.ToInt32(orgUser.ModifiedByID), false);
                        }
                        orgUserToUpdate.PersonAlias.Add(tempPersonAlias);
                    }
                }
                List<Int32> aliasIDToBeDeleted = currentAliasList.Select(x => x.PA_ID).Except(lstPersonAlias.Select(y => y.PA_ID)).ToList();
                foreach (Int32 delAliasID in aliasIDToBeDeleted)
                {
                    PersonAlia delAlias = currentAliasList.FirstOrDefault(x => x.PA_IsDeleted == false && x.PA_ID == delAliasID);
                    delAlias.PA_IsDeleted = true;
                    //delAlias.PA_ModifiedBy = orgUser.OrganizationUserID;
                    delAlias.PA_ModifiedBy = orgUser.ModifiedByID;
                    delAlias.PA_ModifiedOn = DateTime.Now;
                    lstPersonAlias.Add(delAlias);

                    if (!delAlias.PersonAliasExtensions.IsNullOrEmpty())
                    {
                        PersonAliasExtension delPersonAliasExtension = delAlias.PersonAliasExtensions.Where(cond => !cond.PAE_IsDeleted).FirstOrDefault();
                        Int32 currentUserId = Convert.ToInt32(orgUser.ModifiedByID);
                        AddDeletePersonAliasExtension(delPersonAliasExtension, currentUserId, true);
                    }
                }
            }
            _dbNavigation.SaveChanges();
            if (addressNew.AddressID > 0)
                addressMasterId = addressNew.AddressID;
        }

        private void AddDeletePersonAliasExtension(PersonAliasExtension personAliasExtension, Int32 currentUserId, Boolean isDeleted)
        {
            if (!personAliasExtension.IsNullOrEmpty())
            {
                //Update // Delete
                PersonAliasExtension existingPersonAliasExtension = _dbNavigation.PersonAliasExtensions.Where(x => x.PAE_PersonAliasID == personAliasExtension.PAE_PersonAliasID && !x.PAE_IsDeleted).FirstOrDefault();
                if (!existingPersonAliasExtension.IsNullOrEmpty())
                {
                    //  AddUpdatepersonAliasExtension = _dbNavigation.PersonAliasExtensions.Where(cond => !cond.PAE_IsDeleted && cond.PAE_PersonAliasID == personAliasExtension.PAE_PersonAliasID).FirstOrDefault();
                    existingPersonAliasExtension.PAE_Suffix = personAliasExtension.PAE_Suffix;
                    existingPersonAliasExtension.PAE_ModifiedBy = currentUserId;
                    existingPersonAliasExtension.PAE_ModifiedOn = DateTime.Now;
                    existingPersonAliasExtension.PAE_IsDeleted = isDeleted;
                }
                //Add
                else
                {
                    PersonAliasExtension AddUpdatepersonAliasExtension = new PersonAliasExtension();
                    AddUpdatepersonAliasExtension.PAE_Suffix = personAliasExtension.PAE_Suffix;
                    AddUpdatepersonAliasExtension.PAE_PersonAliasID = personAliasExtension.PAE_PersonAliasID;
                    AddUpdatepersonAliasExtension.PAE_CreatedBy = currentUserId;
                    AddUpdatepersonAliasExtension.PAE_CreatedOn = DateTime.Now;

                    _dbNavigation.PersonAliasExtensions.AddObject(AddUpdatepersonAliasExtension);
                    // _dbNavigation.SaveChanges();
                }
            }
        }

        private void AddDeletePersonAliasProfileExtension(PersonAliasProfileExtension personAliasProfileExtension, Int32 currentUserId, Boolean isDeleted)
        {
            if (!personAliasProfileExtension.IsNullOrEmpty())
            {
                //Update // Delete
                PersonAliasProfileExtension AddUpdatepersonAliasProfileExtension = new PersonAliasProfileExtension();
                if (personAliasProfileExtension.PAPE_ID > AppConsts.NONE || personAliasProfileExtension.PAPE_PersonAliasProfileID > AppConsts.NONE)
                {
                    //AddUpdatepersonAliasProfileExtension = _dbNavigation.PersonAliasExtensions.Where(cond => !cond.PAE_IsDeleted && cond.PAE_ID == personAliasExtension.PAE_ID).FirstOrDefault();
                    //AddUpdatepersonAliasProfileExtension.PAE_Suffix = personAliasExtension.PAE_Suffix;
                    //AddUpdatepersonAliasProfileExtension.PAE_ModifiedBy = currentUserId;
                    //AddUpdatepersonAliasProfileExtension.PAE_ModifiedOn = DateTime.Now;
                    //AddUpdatepersonAliasProfileExtension.PAE_IsDeleted = isDeleted;
                }
                //Add
                else
                {
                    //AddUpdatepersonAliasProfileExtension.PAE_Suffix = personAliasExtension.PAE_Suffix;
                    //AddUpdatepersonAliasProfileExtension.PAE_CreatedBy = currentUserId;
                    //AddUpdatepersonAliasProfileExtension.PAE_CreatedOn = DateTime.Now;

                    //_dbNavigation.PersonAliasProfileExtensions.AddObject(AddUpdatepersonAliasProfileExtension);
                }
            }
        }

        private Boolean CheckIfAddressUpdated(Address objAddress, PreviousAddressContract prevAddress)
        {
            if (objAddress.Address1.ToLower().Trim() != prevAddress.Address1.ToLower().Trim()
                 || objAddress.Address2.ToLower().Trim() != prevAddress.Address2.ToLower().Trim()
                 || objAddress.ZipCodeID != prevAddress.ZipCodeID)
            {
                return true;
            }
            if (objAddress.AddressExts.IsNotNull() && objAddress.AddressExts.Count > 0)
            {
                var addressExt = objAddress.AddressExts.FirstOrDefault();
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

        public void AddAddress(Dictionary<String, Object> dicAddressData, Guid addressHandleId, Int32 currentUserId, Address addressNew, AddressExt addressExtNew = null)
        {
            addressNew.Address1 = Convert.ToString(dicAddressData.GetValue("address1"));
            addressNew.Address2 = Convert.ToString(dicAddressData.GetValue("address2"));
            addressNew.ZipCodeID = Convert.ToInt32(dicAddressData.GetValue("zipcodeid"));
            if (addressExtNew.IsNotNull())
            {
                addressNew.AddressExts.Add(addressExtNew);
            }
            addressNew.AddressHandleID = addressHandleId;
            addressNew.CreatedOn = DateTime.Now;
            addressNew.CreatedByID = currentUserId;
            _dbNavigation.Addresses.AddObject(addressNew);
        }

        public void AddMailingAddress(PreviousAddressContract mailingAddress, Guid addressHandleId, Int32 currentUserId, Address addressNew, AddressExt addressExtNew = null)
        {
            addressNew.Address1 = Convert.ToString(mailingAddress.Address1);
            addressNew.Address2 = Convert.ToString(mailingAddress.Address2);
            addressNew.ZipCodeID = Convert.ToInt32(mailingAddress.ZipCodeID);

            if (addressExtNew.IsNotNull())
            {
                addressNew.AddressExts.Add(addressExtNew);

            }
            addressNew.AddressHandleID = addressHandleId;
            addressNew.IsActive = true;
            addressNew.CreatedOn = DateTime.Now;
            addressNew.CreatedByID = currentUserId;
            _dbNavigation.Addresses.AddObject(addressNew);
            _dbNavigation.SaveChanges();

            mailingAddress.MailingAddressId = addressNew.AddressID;
            mailingAddress.MailingExtensionAddressId = addressExtNew.AE_ID;
            mailingAddress.MailingAddressHandleId = addressHandleId;
        }
        public ResidentialHistory AddResidentialHistoryAddress(PreviousAddressContract PrevAddress, Guid addressHandleId, Int32 currentUserId, Int32 orgUserId)
        {
            Address addressNew = AddPreviousAddress(PrevAddress, addressHandleId, orgUserId);

            Entity.ResidentialHistory objResHistory = new ResidentialHistory();
            objResHistory.RHI_IsCurrentAddress = false;
            objResHistory.RHI_IsPrimaryResidence = false;
            objResHistory.RHI_ResidenceStartDate = PrevAddress.ResidenceStartDate;
            objResHistory.RHI_ResidenceEndDate = PrevAddress.ResidenceEndDate;
            objResHistory.RHI_IsDeleted = false;
            //objResHistory.RHI_CreatedByID = currentUserId;
            objResHistory.RHI_CreatedByID = orgUserId;
            objResHistory.RHI_CreatedOn = DateTime.Now;
            objResHistory.RHI_OrganizationUserID = currentUserId;
            objResHistory.RHI_SequenceOrder = PrevAddress.ResHistorySeqOrdID;
            objResHistory.RHI_MotherMaidenName = PrevAddress.MotherName;
            objResHistory.RHI_IdentificationNumber = PrevAddress.IdentificationNumber;
            objResHistory.RHI_DriverLicenseNumber = PrevAddress.LicenseNumber;
            addressNew.ResidentialHistories.Add(objResHistory);
            _dbNavigation.Addresses.AddObject(addressNew);
            return objResHistory;
        }

        private static Address AddPreviousAddress(PreviousAddressContract PrevAddress, Guid addressHandleId, Int32 currentUserId)
        {
            Address PrevAddressNew = new Address();
            PrevAddressNew.Address1 = PrevAddress.Address1;
            PrevAddressNew.Address2 = PrevAddress.Address2;
            if (PrevAddress.ZipCodeID > 0)
                PrevAddressNew.ZipCodeID = PrevAddress.ZipCodeID;
            else
            {
                AddressExt addressExtNew = new AddressExt();
                addressExtNew.AE_CountryID = PrevAddress.CountryId;
                addressExtNew.AE_StateName = PrevAddress.StateName;
                addressExtNew.AE_CityName = PrevAddress.CityName;
                addressExtNew.AE_ZipCode = PrevAddress.Zipcode;
                PrevAddressNew.AddressExts.Add(addressExtNew);
            }
            PrevAddressNew.AddressHandleID = addressHandleId;
            PrevAddressNew.CreatedOn = DateTime.Now;
            PrevAddressNew.CreatedByID = currentUserId;
            return PrevAddressNew;
        }

        public void AddAddressHandle(Guid addressHandleId)
        {
            AddressHandle addressHandleNew = new AddressHandle();
            addressHandleNew.AddressHandleID = addressHandleId;
            _dbNavigation.AddressHandles.AddObject(addressHandleNew);
        }

        public void SaveApplicantOrderProcessMaster(OrganizationUserProfile orgProfileMaster, Dictionary<String, Object> dicAddressData, out Int32 profileIdMaster, out Int32 addressIdMaster, out Guid addressHandlerId, List<PreviousAddressContract> lstPrevAddress, out List<ResidentialHistoryProfile> lstResidentialHistoryProfile, ref List<PersonAliasProfile> lstPersonAliasProfile, Boolean isLocationServiceTenant)
        {
            DateTime dtCreationDateTime = DateTime.Now;

            addressIdMaster = -1;
            lstResidentialHistoryProfile = new List<ResidentialHistoryProfile>();
            addressHandlerId = Guid.NewGuid();


            orgProfileMaster.AddressHandle = new AddressHandle();
            orgProfileMaster.AddressHandle.AddressHandleID = addressHandlerId;
            _dbNavigation.AddressHandles.AddObject(orgProfileMaster.AddressHandle);

            orgProfileMaster.AddressHandleID = addressHandlerId;

            PreviousAddressContract currentAddress = lstPrevAddress.Where(x => x.isCurrent == true).FirstOrDefault();
            Address address = new Address();
            address.AddressTypeID = 0;
            address.AddressHandleID = addressHandlerId;
            //address.CreatedByID = orgProfileMaster.OrganizationUserID;
            address.CreatedByID = orgProfileMaster.CreatedByID;
            address.CreatedOn = dtCreationDateTime;
            address.Address1 = Convert.ToString(dicAddressData.GetValue("address1"));
            address.Address2 = Convert.ToString(dicAddressData.GetValue("address2"));
            address.ZipCodeID = Convert.ToInt32(dicAddressData.GetValue("zipcodeid"));
            if (address.ZipCodeID == 0)
            {

                if (currentAddress.IsNotNull())
                {
                    AddressExt addressExt = new AddressExt();
                    //UAT-3910
                    if (isLocationServiceTenant)
                    {
                        addressExt.AE_CountryID = AppConsts.ONE;
                        addressExt.AE_County = Convert.ToString(currentAddress.CountryId);
                    }
                    else
                    {
                        addressExt.AE_CountryID = currentAddress.CountryId;
                    }
                    addressExt.AE_StateName = currentAddress.StateName;
                    addressExt.AE_CityName = currentAddress.CityName;
                    addressExt.AE_ZipCode = currentAddress.Zipcode;
                    address.AddressExts.Add(addressExt);
                }
            }
            orgProfileMaster.AddressHandleID = addressHandlerId;
            _dbNavigation.Addresses.AddObject(address);
            //Add current address into residential history profile table.
            ResidentialHistoryProfile currentResHistoryProfile = new ResidentialHistoryProfile();
            currentResHistoryProfile.RHIP_IsCurrentAddress = true;
            currentResHistoryProfile.RHIP_IsPrimaryResidence = false;
            currentResHistoryProfile.RHIP_ResidenceStartDate = currentAddress.IsNotNull() ? currentAddress.ResidenceStartDate : (DateTime?)null;
            currentResHistoryProfile.RHIP_ResidenceEndDate = currentAddress.IsNotNull() ? currentAddress.ResidenceEndDate : (DateTime?)null;
            currentResHistoryProfile.RHIP_IsDeleted = false;
            //currentResHistoryProfile.RHIP_CreatedBy = orgProfileMaster.OrganizationUserID;
            currentResHistoryProfile.RHIP_CreatedBy = orgProfileMaster.CreatedByID;
            currentResHistoryProfile.RHIP_CreatedOn = DateTime.Now;
            currentResHistoryProfile.OrganizationUserProfile = orgProfileMaster;
            currentResHistoryProfile.Address = address;
            //            currentResHistoryProfile.RHIP_SequenceId = currentAddress.ResHistorySeqOrdID;

            currentResHistoryProfile.RHIP_SequenceOrder = AppConsts.ONE;
            currentResHistoryProfile.RHIP_IdentificationNumber = currentAddress.IsNotNull() ? currentAddress.IdentificationNumber : null;
            currentResHistoryProfile.RHIP_DriverLicenseNumber = currentAddress.IsNotNull() ? currentAddress.LicenseNumber : null;
            currentResHistoryProfile.RHIP_MotherMaidenName = currentAddress.IsNotNull() ? currentAddress.MotherName : null;

            _dbNavigation.ResidentialHistoryProfiles.AddObject(currentResHistoryProfile);
            lstResidentialHistoryProfile.Add(currentResHistoryProfile);

            //Commented to display current address Residing From and To fields in the order confirmation screen
            //lstPrevAddress.Remove(currentAddress);

            if (lstPrevAddress.IsNotNull())
            {
                lstPrevAddress = lstPrevAddress.Where(x => x.isDeleted != true
                    && x.isCurrent == false) //Is current check added as the current address is NOT removed from the list now (Comment above)
                    .ToList();
                if (lstPrevAddress.Count > 0)
                {
                    foreach (var prevAddress in lstPrevAddress)
                    {
                        Guid addressHandleId = Guid.NewGuid();
                        AddAddressHandle(addressHandleId);
                        Address addressNew = new Address();
                        addressNew.Address1 = prevAddress.Address1;
                        addressNew.Address2 = prevAddress.Address2;
                        addressNew.ZipCodeID = prevAddress.ZipCodeID;
                        if (addressNew.ZipCodeID == 0)
                        {
                            AddressExt addressExtNew = new AddressExt();
                            addressExtNew.AE_CountryID = prevAddress.CountryId;
                            addressExtNew.AE_StateName = prevAddress.StateName;
                            addressExtNew.AE_CityName = prevAddress.CityName;
                            addressExtNew.AE_ZipCode = prevAddress.Zipcode;
                            addressNew.AddressExts.Add(addressExtNew);
                        }
                        addressNew.AddressHandleID = addressHandleId;
                        addressNew.CreatedOn = DateTime.Now;
                        //addressNew.CreatedByID = orgProfileMaster.OrganizationUserID;
                        addressNew.CreatedByID = orgProfileMaster.CreatedByID;

                        ResidentialHistoryProfile objResHistoryProfile = new ResidentialHistoryProfile();
                        objResHistoryProfile.RHIP_IsCurrentAddress = false;
                        objResHistoryProfile.RHIP_IsPrimaryResidence = false;
                        objResHistoryProfile.RHIP_ResidenceStartDate = prevAddress.ResidenceStartDate;
                        objResHistoryProfile.RHIP_ResidenceEndDate = prevAddress.ResidenceEndDate;
                        objResHistoryProfile.RHIP_IsDeleted = false;
                        //objResHistoryProfile.RHIP_CreatedBy = orgProfileMaster.OrganizationUserID;
                        objResHistoryProfile.RHIP_CreatedBy = orgProfileMaster.CreatedByID;
                        objResHistoryProfile.RHIP_CreatedOn = DateTime.Now;
                        objResHistoryProfile.OrganizationUserProfile = orgProfileMaster;
                        //objResHistoryProfile.RHIP_SequenceId = prevAddress.ResHistorySeqOrdID;
                        objResHistoryProfile.RHIP_SequenceOrder = prevAddress.ResHistorySeqOrdID;
                        objResHistoryProfile.RHIP_MotherMaidenName = prevAddress.MotherName;
                        objResHistoryProfile.RHIP_IdentificationNumber = prevAddress.IdentificationNumber;
                        objResHistoryProfile.RHIP_DriverLicenseNumber = prevAddress.LicenseNumber;
                        addressNew.ResidentialHistoryProfiles.Add(objResHistoryProfile);
                        _dbNavigation.Addresses.AddObject(addressNew);
                        lstResidentialHistoryProfile.Add(objResHistoryProfile);
                    }
                }
            }

            if (lstPersonAliasProfile.IsNotNull())
            {
                foreach (PersonAliasProfile tempPersonAlias in lstPersonAliasProfile)
                {
                    //tempPersonAlias.PAP_CreatedBy = orgProfileMaster.OrganizationUserID;
                    tempPersonAlias.PAP_CreatedBy = orgProfileMaster.CreatedByID;
                    tempPersonAlias.PAP_CreatedOn = DateTime.Now;
                    orgProfileMaster.PersonAliasProfiles.Add(tempPersonAlias);

                    //CBI || CABS
                    if (!tempPersonAlias.PersonAliasProfileExtensions.IsNullOrEmpty())
                    {
                        PersonAliasProfileExtension personAliasProfileExtension = tempPersonAlias.PersonAliasProfileExtensions.FirstOrDefault();
                        personAliasProfileExtension.PAPE_CreatedBy = orgProfileMaster.CreatedByID;
                        personAliasProfileExtension.PAPE_CreatedOn = DateTime.Now;
                    }//
                }
            }

            //orgProfileMaster.CreatedByID = orgProfileMaster.OrganizationUserID;
            orgProfileMaster.CreatedOn = dtCreationDateTime;

            _dbNavigation.OrganizationUserProfiles.AddObject(orgProfileMaster);
            _dbNavigation.SaveChanges();

            if (address.IsNotNull())
                addressIdMaster = address.AddressID;

            profileIdMaster = orgProfileMaster.OrganizationUserProfileID;
        }

        public List<PaymentIntegrationSetting> GetPaymentIntegrationSettingsByName(String name)
        {
            return _dbNavigation.PaymentIntegrationSettings.Where(settings => settings.Name.ToLower().Trim() == name.ToLower().Trim()).ToList();
        }
        #endregion

        #region UserProgram
        //public Boolean CopyOrganizationUserProgram(List<OrganizationUserProgram> organizationUserProgram)
        //{
        //    organizationUserProgram.ForEach(cond => { _dbNavigation.OrganizationUserPrograms.AddObject(cond); });
        //    if (_dbNavigation.SaveChanges() > 0)
        //        return true;
        //    return false;
        //}

        public Boolean UpdateChanges()
        {
            if (_dbNavigation.SaveChanges() > 0)
                return true;
            return false;
        }

        //public Boolean UpdateUserProgram(List<OrganizationUserProgram> lstOtganizationUserProgram, Int32 organizationUserId)
        //{
        //    List<OrganizationUserProgram> tempListorganizationUserProgram = _dbNavigation.OrganizationUserPrograms.Where(cond => cond.OrganizationUserID == organizationUserId && !cond.IsDeleted).ToList();
        //    List<OrganizationUserProgram> orgUserProgramForDelete = tempListorganizationUserProgram.Where(x => !lstOtganizationUserProgram.Any(cnd => cnd.DeptProgramMappingID == x.DeptProgramMappingID)).ToList();
        //    List<OrganizationUserProgram> orgUserProgramIdsToSave = lstOtganizationUserProgram.Where(y => !tempListorganizationUserProgram.Any(cd => cd.DeptProgramMappingID == y.DeptProgramMappingID)).ToList();
        //    orgUserProgramForDelete.ForEach(cond =>
        //    {
        //        cond.IsDeleted = true;
        //        cond.ModifiedByID = organizationUserId;
        //        cond.ModifiedOn = DateTime.Now;
        //    });
        //    orgUserProgramIdsToSave.ForEach(con =>
        //    {
        //        _dbNavigation.OrganizationUserPrograms.AddObject(con);
        //    });
        //    if (_dbNavigation.SaveChanges() > 0)
        //        return true;
        //    return false;
        //}

        //public List<OrganizationUserProgram> GetAllUserProgram(Int32 organizationUserId)
        //{
        //    return _dbNavigation.OrganizationUserPrograms.Where(cond => cond.OrganizationUserID == organizationUserId && !cond.IsDeleted).ToList();
        //}
        #endregion

        #region Client DB Configuiration

        /// <returns>
        /// <see cref="TenantConnectionString"/> 
        /// </returns>
        Boolean ISecurityRepository.GetTenantConnectionStringByConnectionStringAndUserId(String connectionString, Int32? tenantId)
        {
            if (tenantId == null)
            {
                ClientDBConfiguration tenantDBCon = _dbNavigation.ClientDBConfigurations.Where(x => x.CDB_ConnectionString.Contains(connectionString)).FirstOrDefault();
                if (tenantDBCon.IsNull())
                    return false;
                else
                    return true;
            }
            else
            {
                ClientDBConfiguration tenantDBCon = _dbNavigation.ClientDBConfigurations.Where(x => x.CDB_ConnectionString.Contains(connectionString) && x.CDB_TenantID != tenantId).FirstOrDefault();
                if (tenantDBCon.IsNull())
                    return false;
                else
                    return true;
            }
        }

        /// <summary>
        /// Retrieves all Client DB Configuration
        /// </summary>
        /// <returns>
        /// </returns>
        public List<ClientDBConfiguration> GetClientDBConfiguration()
        {
            String code = TenantType.Institution.GetStringValue();
            return _dbNavigation.ClientDBConfigurations.Where(cond => cond.Tenant.lkpTenantType.TenantTypeCode == code && cond.Tenant.IsDeleted == false).DistinctBy(cond => cond.CDB_ConnectionString).ToList();
        }

        /// <summary>
        /// Retrieves App Configuration on basis of Key
        /// </summary>
        /// <returns>
        /// </returns>
        public AppConfiguration GetAppConfiguration(String key)
        {
            return _dbNavigation.AppConfigurations.Where(cond => cond.AC_Key == key).FirstOrDefault();
        }

        /// <summary>
        /// UAT-2319
        /// </summary>
        /// <param name="key"></param>
        public void UpdateAppConfiguration(String key, String UpdatedValue)
        {
            AppConfiguration appConfiguration = _dbNavigation.AppConfigurations.Where(cmd => cmd.AC_Key == key).FirstOrDefault();
            if (appConfiguration != null)
            {
                appConfiguration.AC_Value = UpdatedValue;
                _dbNavigation.SaveChanges();
            }
        }

        #endregion

        #region Document

        public Boolean CheckDocumentDeletionAllowed(Int32 applicantUploadedDocumentID, Int32 tenantID)
        {
            Boolean? canDocumentBeDeleted = _dbNavigation.usp_IsDocumentDeletionAllowed(tenantID, applicantUploadedDocumentID).SingleOrDefault();
            return canDocumentBeDeleted == null ? false : canDocumentBeDeleted.Value;
        }

        public void SynchronizeApplicantDocument(Int32 organisationUserId, Int32 tenantID, Int32 currentLoggedInUseID)
        {
            _dbNavigation.SyncApplicantDocsAllTenant(tenantID, organisationUserId, currentLoggedInUseID);
        }

        #endregion

        #region User Profile Synchronisation

        public Boolean SynchoniseUserProfile(Int32 organisationUserId, Int32 tenantID, Int32 orgUsrID)
        {
            _dbNavigation.SyncApplicantProfilesAllTenant(tenantID, organisationUserId, orgUsrID);
            return true;
        }

        #endregion


        public Guid UpdateMailingAddress(PreviousAddressContract mailingAddress, Boolean isLocationServiceTenant, Int32 orgUserID)
        {
            Address mailingAdressNew = new Address();
            AddressExt mailingaddressExtNew = new AddressExt();
            if (mailingAddress.IsNotNull())
            {
                if (mailingAddress.ZipCodeID == 0)
                {
                    if (isLocationServiceTenant)
                    {
                        mailingaddressExtNew.AE_CountryID = AppConsts.ONE;
                        mailingaddressExtNew.AE_County = Convert.ToString(mailingAddress.CountryId);

                    }
                    else
                    {

                        mailingaddressExtNew.AE_CountryID = mailingAddress.CountryId;
                    }

                    mailingaddressExtNew.AE_StateName = mailingAddress.StateName;
                    mailingaddressExtNew.AE_CityName = mailingAddress.CityName;
                    mailingaddressExtNew.AE_ZipCode = mailingAddress.Zipcode;

                }
            }

            Guid mailingaddressHandleId = Guid.NewGuid();
            AddAddressHandle(mailingaddressHandleId);
            AddMailingAddress(mailingAddress, mailingaddressHandleId, orgUserID, mailingAdressNew, mailingaddressExtNew);
            return mailingaddressHandleId;
        }


        public List<OrganizationUser> getOrganizationUserByIdList(List<Int32?> userIds)
        {
            return _dbNavigation.OrganizationUsers.Where(x => userIds.Contains(x.OrganizationUserID)).ToList();
        }

        public List<OrganizationUser> GetOrganizationUserByIds(List<Int32> lstOrgUserId)
        {
            return _dbNavigation.OrganizationUsers.Where(x => lstOrgUserId.Contains(x.OrganizationUserID)).ToList();
        }

        /// <summary>
        /// Gets the list of existing users.
        /// </summary>
        /// <param name="userName">User Name</param>
        /// <param name="dOB">Date Of Birth</param>
        /// <param name="sSN">Social Security Number</param>
        /// <param name="firstName">First Name</param>
        /// <param name="lastName">Last Name</param>
        /// <returns>list of active users</returns>
        public List<LookupContract> GetExistingUserLists(String userName, DateTime dOB, String sSN, String firstName, String lastName, Int32 defaultTenantId, String email, Boolean isApplicant = false, Boolean isSharedUser = false, String languageCode = default(String))
        {
            dOB = dOB.Date;

            List<Guid> lstUserId = _dbNavigation.aspnet_Users.Where(cond => cond.UserName.ToLower().Equals(userName.ToLower())).Select(sel => sel.UserId).ToList();

            List<OrganizationUser> lstOrgUsers = _dbNavigation.OrganizationUsers.Where(obj => (
                   (lstUserId.Contains(obj.UserID))
                || (obj.DOB.Value == dOB && obj.SSN.ToLower() == sSN.ToLower())
                || (obj.FirstName.ToLower().Equals(firstName.ToLower()) && obj.LastName.ToLower().Equals(lastName.ToLower()) && obj.DOB.Value == dOB)
                || (obj.FirstName.ToLower().Equals(firstName.ToLower()) && obj.LastName.ToLower().Equals(lastName.ToLower()) && obj.SSN.ToLower() == sSN.ToLower())
                || (obj.FirstName.ToLower().Equals(firstName.ToLower()) && obj.LastName.ToLower().Equals(lastName.ToLower()) && obj.aspnet_Users.aspnet_Membership.Email.ToLower() == email.ToLower())) //UAT-1218
                && obj.IsActive == true
                && obj.IsDeleted == false).ToList();

            if (isApplicant || isSharedUser)//check added corresponding for UAT - 927 : While creating a new account if applicant satisfies account link conditions, then application shows client admin users list also. 
            {
                //For ADB admin
                if (lstOrgUsers.Any(obj => obj.IsApplicant == false && (obj.IsSharedUser ?? false) == false && obj.Organization.TenantID == AppConsts.SUPER_ADMIN_TENANT_ID))
                {
                    return lstOrgUsers.Where(obj => obj.IsApplicant == false && (obj.IsSharedUser ?? false) == false && obj.Organization.TenantID == AppConsts.SUPER_ADMIN_TENANT_ID).Select(obj => new LookupContract
                    {
                        Name = "I am '" + obj.FirstName + " " + obj.LastName + "' (ADB Admin). '", //+ obj.Organization.Tenant.TenantName + "'.",
                        Code = obj.aspnet_Users.UserName,
                        ID = obj.OrganizationID,
                        UserID = obj.OrganizationUserID
                    }).ToList();
                }

                //For client admin
                if (lstOrgUsers.Any(obj => obj.IsApplicant == false && (obj.IsSharedUser ?? false) == false && obj.Organization.TenantID != AppConsts.SUPER_ADMIN_TENANT_ID))
                {
                    return lstOrgUsers.Where(obj => obj.IsApplicant == false && (obj.IsSharedUser ?? false) == false && obj.Organization.TenantID != AppConsts.SUPER_ADMIN_TENANT_ID).Select(obj => new LookupContract
                    {
                        Name = "I am '" + obj.FirstName + " " + obj.LastName + "' (Client Admin) from '" + obj.Organization.Tenant.TenantName + "'.",
                        Code = obj.aspnet_Users.UserName,
                        ID = obj.OrganizationID,
                        UserID = obj.OrganizationUserID
                    }).ToList();
                }

                //For Applicant
                else if (lstOrgUsers.Any(obj => obj.IsApplicant == true && obj.Organization.OrganizationID != defaultTenantId))
                {
                    if (languageCode == Languages.SPANISH.GetStringValue())
                    {
                        return lstOrgUsers.Where(obj => obj.IsApplicant == true && obj.Organization.OrganizationID != defaultTenantId).Select(obj => new LookupContract
                        {
                            Name = AppConsts.I_AM_IN_SPANISH + " " + obj.FirstName + " " + obj.LastName + " " + AppConsts.FROM_IN_SPANISH + " " + obj.Organization.Tenant.TenantName + "'.",
                            Code = obj.aspnet_Users.UserName,
                            ID = obj.OrganizationID,
                            UserID = obj.OrganizationUserID
                        }).ToList();
                    }
                    else
                    {
                        return lstOrgUsers.Where(obj => obj.IsApplicant == true && obj.Organization.OrganizationID != defaultTenantId).Select(obj => new LookupContract
                        {
                            Name = "I am '" + obj.FirstName + " " + obj.LastName + "' from '" + obj.Organization.Tenant.TenantName + "'.",
                            Code = obj.aspnet_Users.UserName,
                            ID = obj.OrganizationID,
                            UserID = obj.OrganizationUserID
                        }).ToList();
                    }
                }

                //For Shared User
                else if (lstOrgUsers.Any(obj => obj.IsApplicant == false && obj.IsSharedUser == true))
                {
                    Int32 orgUserID = lstOrgUsers.Where(obj => obj.IsSharedUser == true).Select(col => col.OrganizationUserID).FirstOrDefault();
                    Guid userID = lstOrgUsers.Where(obj => obj.IsSharedUser == true).Select(col => col.UserID).FirstOrDefault();
                    List<String> lstUserTypeCodes = _dbNavigation.OrganizationUserTypeMappings.Where(cond => cond.OTM_OrgUserID == orgUserID && cond.OTM_IsDeleted == false)
                        .Select(col => col.lkpOrgUserType.OrgUserTypeCode).ToList();
                    String userType = String.Empty;

                    if (lstUserTypeCodes.Contains(OrganizationUserType.AgencyUser.GetStringValue()))
                    {
                        String agencyName = String.Empty;
                        List<String> lstUserAgencyName = SharedDataDBContext.UserAgencyMappings.Where(cond => cond.UAM_IsVerified
                                                                                                        && !cond.UAM_IsDeleted
                                                                                                        && cond.AgencyUser.AGU_UserID == userID
                                                                                                        && !cond.AgencyUser.AGU_IsDeleted
                                                                                                        && !cond.UAM_IsDeleted
                                                                                                        && !cond.Agency.AG_IsDeleted)
                                                                                                       .Select(col => col.Agency.AG_Name)
                                                                                                       .ToList();
                        if (!lstUserAgencyName.IsNullOrEmpty())
                        {
                            agencyName = String.Join(",", lstUserAgencyName);
                        }
                        //base.SharedDataDBContext.AgencyUsers.Where(cond => cond.AGU_UserID == userID && !cond.AGU_IsDeleted).Select(col => col.Agency.AG_Name).FirstOrDefault(); //UAT-1641
                        userType = "(Agency User) from " + agencyName;
                    }
                    else if (lstUserTypeCodes.Contains(OrganizationUserType.ApplicantsSharedUser.GetStringValue()))
                    {
                        userType = "(Shared User)";
                    }
                    else if (lstUserTypeCodes.Contains(OrganizationUserType.Instructor.GetStringValue()))
                    {
                        userType = "(Instructor)";
                    }
                    else if (lstUserTypeCodes.Contains(OrganizationUserType.Preceptor.GetStringValue()))
                    {
                        userType = "(Preceptor)";
                    }

                    return lstOrgUsers.Where(obj => obj.IsSharedUser == true).Select(obj => new LookupContract
                    {
                        Name = "I am '" + obj.FirstName + " " + obj.LastName + " " + userType + "'.",
                        Code = obj.aspnet_Users.UserName,
                        ID = obj.OrganizationID,
                        UserID = obj.OrganizationUserID
                    }).ToList();
                }
                else
                    return new List<LookupContract>();
            }
            else
            {
                return lstOrgUsers.Select(obj => new LookupContract
                {
                    Name = "I am '" + obj.FirstName + " " + obj.LastName + "' from '" + obj.Organization.Tenant.TenantName + "'.",
                    Code = obj.aspnet_Users.UserName,
                    ID = obj.OrganizationID,
                    UserID = obj.OrganizationUserID
                }).ToList();
            }
        }

        public Int32 GetTenantIdFromOrganizationId(Int32 organisationId)
        {
            return Convert.ToInt32(_dbNavigation.Organizations.FirstOrDefault(obj => obj.OrganizationID == organisationId && obj.IsActive && obj.IsDeleted == false).TenantID);

        }

        /// <summary>
        /// Finds if the username is already present in tenant database
        /// </summary>
        /// <param name="userName">User Name</param>
        /// <param name="tenantId">Tenant Id</param>
        /// <returns>True if username exists</returns>
        public Boolean IsUsernameExistInTenantDB(String userName, Int32 tenantId)
        {
            return _dbNavigation.OrganizationUsers.Any(x => x.Organization.TenantID == tenantId
                && x.aspnet_Users.UserName.ToLower().Equals(userName)
                && x.IsApplicant == true //UAT-1218
                && x.IsDeleted == false);
        }



        #region System Service

        /// <summary>
        /// Get Active System Service Triggers
        /// </summary>
        /// <returns>List of System Service Trigger</returns>
        public List<SystemServiceTrigger> GetSystemServiceTriggers()
        {
            return _dbNavigation.SystemServiceTriggers.Where(x => x.SST_IsActive == true && x.SST_IsDeleted == false).ToList();
        }

        /// <summary>
        /// Get System Service Triggers By Id
        /// </summary>
        /// <returns>System Service Trigger</returns>
        public SystemServiceTrigger GetSystemServiceTriggerByID(Int32 id)
        {
            return _dbNavigation.SystemServiceTriggers.Where(x => x.SST_ID == id && x.SST_IsActive == true && x.SST_IsDeleted == false).FirstOrDefault();
        }

        /// <summary>
        /// Get System Service Triggers By SystemServiceId
        /// </summary>
        /// <returns>System Service Trigger</returns>
        public SystemServiceTrigger GetSystemServiceTriggerBySystemServiceID(Int16 systemServiceID, Int32 tenantID)
        {
            return _dbNavigation.SystemServiceTriggers.Where(x => x.SST_SystemServiceID == systemServiceID && x.SST_TenantID == tenantID && x.SST_IsActive == true && x.SST_IsDeleted == false).FirstOrDefault();
        }

        /// <summary>
        /// Get System Service LookUp
        /// </summary>
        /// <returns>List of System Service LookUp</returns>
        public List<lkpSystemService> GetSystemServiceLookUp()
        {
            return _dbNavigation.lkpSystemServices.Where(x => x.SS_IsDeleted == false).ToList();
        }

        /// <summary>
        /// Get System Service by Code
        /// </summary>
        /// <param name="code"></param>
        /// <returns>System Service</returns>
        public lkpSystemService GetSystemServiceByCode(String code)
        {
            return _dbNavigation.lkpSystemServices.Where(x => x.SS_Code.Equals(code) && x.SS_IsDeleted == false).FirstOrDefault();
        }

        /// <summary>
        /// Add System Service Trigger
        /// </summary>
        /// <param name="systemServiceTrigger"></param>
        /// <returns>true/false</returns>
        public Boolean AddSystemServiceTrigger(SystemServiceTrigger systemServiceTrigger)
        {
            _dbNavigation.SystemServiceTriggers.AddObject(systemServiceTrigger);
            if (_dbNavigation.SaveChanges() > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Update System Service Trigger
        /// </summary>
        /// <param name="systemServiceTrigger"></param>
        /// <returns>true/false</returns>
        public Boolean UpdateSystemServiceTrigger(SystemServiceTrigger systemServiceTrigger)
        {
            SystemServiceTrigger sysServiceTrigger = GetSystemServiceTriggerByID(systemServiceTrigger.SST_ID);
            if (sysServiceTrigger != null)
            {
                sysServiceTrigger.SST_ID = systemServiceTrigger.SST_ID;
                sysServiceTrigger.SST_SystemServiceID = systemServiceTrigger.SST_SystemServiceID;
                sysServiceTrigger.SST_TenantID = systemServiceTrigger.SST_TenantID;
                sysServiceTrigger.SST_IsActive = systemServiceTrigger.SST_IsActive;
                sysServiceTrigger.SST_IsDeleted = systemServiceTrigger.SST_IsDeleted;
                sysServiceTrigger.SST_CreatedByID = systemServiceTrigger.SST_CreatedByID;
                sysServiceTrigger.SST_CreatedOn = systemServiceTrigger.SST_CreatedOn;
                sysServiceTrigger.SST_ModifiedByID = systemServiceTrigger.SST_ModifiedByID;
                sysServiceTrigger.SST_ModifiedOn = systemServiceTrigger.SST_ModifiedOn;
                if (_dbNavigation.SaveChanges() > 0)
                    return true;
                else
                    return false;
            }
            return false;
        }

        #endregion

        #region Temporary files for pdf conversion
        public Boolean SavePageHtmlContentLocation(TempFile tempFiles)
        {
            if (tempFiles.IsNotNull())
            {
                _dbNavigation.AddToTempFiles(tempFiles);
                _dbNavigation.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<TempFile> GetFilePath(Guid Id)
        {
            if (Id.IsNotNull())
            {
                return _dbNavigation.TempFiles.Where(obj => obj.TF_Identifier == Id).ToList();
            }
            else
            {
                return null;
            }
        }
        public Boolean DeleteTempFile(Guid Id, Int32 CurrentLoggedInUserID)
        {
            if (Id.IsNotNull())
            {
                TempFile tempRecords = _dbNavigation.TempFiles.Where(obj => obj.TF_Identifier == Id && obj.TF_IsDeleted == false).FirstOrDefault();
                if (tempRecords.IsNotNull())
                {
                    tempRecords.TF_IsDeleted = true;
                    tempRecords.TF_ModifiedOn = DateTime.Now;
                    tempRecords.TF_ModifiedByID = CurrentLoggedInUserID;
                    _dbNavigation.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        #endregion

        /// <summary>
        /// Gets the list of client admin users working in the organization associated with the given tenant Id. 
        /// </summary>
        /// <param name="tenantId">Tenant Id</param>
        /// <returns>List of active Users</returns>
        public List<OrganizationUser> GetClientAdminUsersByTanentId(Int32 tenantId)
        {
            Int32 organizationId = 0;
            Organization organization = _dbNavigation.Organizations.Where(obj => obj.TenantID == tenantId && obj.IsActive == true && obj.IsDeleted == false).FirstOrDefault();
            if (organization.IsNotNull())
            {
                organizationId = organization.OrganizationID;
            }
            return _dbNavigation.OrganizationUsers
                .Include(SysXEntityConstants.TABLE_ASPNET_USERS)
                .Include(SysXEntityConstants.TABLE_MESSAGING_ASPNET_USERS_DOT_ASPNET_MEMBERSHIP)
                .Where(obj => obj.OrganizationID == organizationId && obj.IsActive == true && obj.IsDeleted == false
                && obj.IsApplicant == false).ToList();
        }

        public List<FeatureAction> GetFeatureAction(Int32 productFeatureID, Int32 rolePermissionID)
        {
            return _dbNavigation.FeatureActions.Where(cond => cond.ProductFeatureID == productFeatureID).ToList();
        }

        public List<FeatureRoleAction> GetRoleActionFeatures(Int32 featureID, Int32 userID, Int32 sysXBlockID)
        {
            Guid user = _dbNavigation.OrganizationUsers.FirstOrDefault(cond => cond.OrganizationUserID == userID).UserID;
            IEnumerable<Guid> usersInRoles = _dbNavigation.vw_aspnet_UsersInRoles.Where(cond => cond.UserId == user).Select(col => col.RoleId);
            IEnumerable<Int32> sysXBlocks = _dbNavigation.SysXBlocksFeatures.Where(cond => cond.SysXBlockID == sysXBlockID).Select(col => col.SysXBlockFeatureID);
            IEnumerable<Int32> rolePermissionFeatures = _dbNavigation.RolePermissionProductFeatures.Where(cond => usersInRoles.Contains(cond.RoleId) && sysXBlocks.Contains(cond.SysXBlockFeatureId.Value)).Select(col => col.RolePermissionFeature);
            return _dbNavigation.FeatureRoleActions.Include("FeatureAction").Where(fx => rolePermissionFeatures.Contains(fx.RolePermissionProductFeature.Value)).ToList();



        }


        public List<FeatureRoleAction> GetRoleActionFeaturesTemp(Int32 featureID, Int32 userID, Int32 sysXBlockID)
        {
            Guid user = _dbNavigation.OrganizationUsers.FirstOrDefault(cond => cond.OrganizationUserID == userID).UserID;
            IEnumerable<Guid> usersInRoles = _dbNavigation.vw_aspnet_UsersInRoles.Where(cond => cond.UserId == user).Select(col => col.RoleId);
            IEnumerable<Int32> sysXBlocks = _dbNavigation.SysXBlocksFeatures.Where(cond => cond.SysXBlockID == sysXBlockID && cond.FeatureID == featureID).Select(col => col.SysXBlockFeatureID);
            IEnumerable<Int32> rolePermissionFeatures = _dbNavigation.RolePermissionProductFeatures.Where(cond => usersInRoles.Contains(cond.RoleId) && sysXBlocks.Contains(cond.SysXBlockFeatureId.Value)).Select(col => col.RolePermissionFeature);
            return _dbNavigation.FeatureRoleActions.Include("FeatureAction").Where(fx => rolePermissionFeatures.Contains(fx.RolePermissionProductFeature.Value)).ToList();

        }



        public List<Entity.Tenant> GetTenantsBasedOnBusinessChannelType(short businessChannelTypeID)
        {
            return _dbNavigation.vw_GetTenants.ToList().Where(cond => cond.BCT.Value == businessChannelTypeID)
                            .Select(col =>
                            new Entity.Tenant
                            {
                                TenantID = col.TenantID,
                                TenantName = col.TenantName,
                                TenantTypeID = col.TenantTypeID

                            }).ToList();
        }

        public List<Entity.ClientEntity.Tenant> GetClientTenantsBasedOnBusinessChannelType(short businessChannelTypeID)
        {
            return _dbNavigation.vw_GetTenants.ToList().Where(cond => cond.BCT.Value == businessChannelTypeID)
                            .Select(col =>
                            new Entity.ClientEntity.Tenant
                            {
                                TenantID = col.TenantID,
                                TenantName = col.TenantName,
                                TenantTypeID = col.TenantTypeID

                            }).ToList();
        }



        #region FeatureActionT

        DataTable ISecurityRepository.GetFeatureActionT(Int32 productFeatureID)
        {
            EntityConnection connection = _dbNavigation.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("GetFeatureActionList", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@featureId", productFeatureID);
                //command.Parameters.AddWithValue("@filteringSortingData", verificationGridCustomPaging.XML);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
            }
            return new DataTable();
        }

        public List<Permission> GetListOFPermissions()
        {
            return _dbNavigation.Permissions.Where(x => x.IsActive && !x.IsDeleted).ToList();
        }

        #endregion

        #region D & R Document Entity Mapping

        Boolean ISecurityRepository.SaveUpdateDisclosureDocumentMapping(Entity.DisclosureDocumentMapping newDisclosureDocumentMapping)
        {
            if (newDisclosureDocumentMapping.DDM_ID == 0)
                _dbNavigation.DisclosureDocumentMappings.AddObject(newDisclosureDocumentMapping);
            _dbNavigation.SaveChanges();
            return true;
        }

        Entity.DisclosureDocumentMapping ISecurityRepository.GetDisclosureDocumentMappingById(Int32 disclosureDocumentMappingId)
        {
            return _dbNavigation.DisclosureDocumentMappings.Where(cond => cond.DDM_ID == disclosureDocumentMappingId && cond.DDM_IsDeleted == false).FirstOrDefault();
        }

        #endregion

        public DataTable GetBuisnessChannelTypeByTenantId(Int32 tenantId)
        {
            EntityConnection connection = _dbNavigation.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("usp_GetBuisnessChannelTypeByTenantId", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@tenantId", tenantId);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
            }
            return new DataTable();
        }

        #region Payment Instruction

        public String GetPaymentInstruction(String paymentModeCode)
        {
            lkpPaymentOption paymentMode = _dbNavigation.lkpPaymentOptions.Where(cond => cond.Code.Equals(paymentModeCode) && !cond.IsDeleted).FirstOrDefault();
            if (paymentMode.IsNotNull())
            {
                return paymentMode.InstructionText;
            }
            return null;
        }

        /// <summary>
        /// Gets the list of all the Master lkpPaymentOption
        /// </summary>
        /// <param name="paymentModeCode"></param>
        /// <returns></returns>
        public List<lkpPaymentOption> GetMasterPaymentOptions()
        {
            return _dbNavigation.lkpPaymentOptions.Where(po => !po.IsDeleted).ToList();
        }

        #endregion


        public DataTable GetMultiInstitutionAssignmentData(String inuputXml, CustomPagingArgsContract gridCustomPaging)
        {
            EntityConnection connection = _dbNavigation.Connection as EntityConnection;

            String orderBy = QueueConstants.APPLICANT_SEARCH_DEFAULT_SORTING_FIELDS;
            String ordDirection = null;

            orderBy = String.IsNullOrEmpty(gridCustomPaging.SortExpression) ? orderBy : gridCustomPaging.SortExpression;
            ordDirection = gridCustomPaging.SortDirectionDescending == false ? String.IsNullOrEmpty(gridCustomPaging.SortExpression) ? "desc" : "asc" : "desc";

            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("ams.usp_GetVerificationDataFromMultipleInstitutions", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@InuputXml", inuputXml);
                command.Parameters.AddWithValue("@filterXml", gridCustomPaging.XML);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 1)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        gridCustomPaging.CurrentPageIndex = Convert.ToInt32(Convert.ToString(ds.Tables[0].Rows[0]["CurrentPageIndex"]));
                        gridCustomPaging.VirtualPageCount = Convert.ToInt32(Convert.ToString(ds.Tables[0].Rows[0]["VirtualCount"]));
                    }
                    return ds.Tables[1];
                }
            }
            return new DataTable();
        }

        AuthNetCustomerProfile ISecurityRepository.GetCustomerProfile(Guid userId, int defaultTenantID, int tenantID)
        {
            var _paymentIntSettingClientMappings = _dbNavigation.PaymentIntegrationSettingClientMappings.ToList();

            int paymentIntegrationSettingID = 0;

            if (_paymentIntSettingClientMappings.Any(col => col.TenantID == tenantID))
            {
                paymentIntegrationSettingID = _paymentIntSettingClientMappings.Where(col => col.TenantID == tenantID).FirstOrDefault().PaymentIntegrationSettingID;
            }
            else
            {
                paymentIntegrationSettingID = _paymentIntSettingClientMappings.Where(col => col.TenantID == defaultTenantID).FirstOrDefault().PaymentIntegrationSettingID;
            }

            return _dbNavigation.AuthNetCustomerProfiles.Where(cond => cond.UserID == userId && cond.PaymentIntegrationSettingID == paymentIntegrationSettingID).FirstOrDefault();
        }

        long ISecurityRepository.CreateNewAuthNetCustomerProfile(Entity.AuthNetCustomerProfile authNetCustomerProfile)
        {
            _dbNavigation.AuthNetCustomerProfiles.AddObject(authNetCustomerProfile);
            _dbNavigation.SaveChanges();
            return Convert.ToInt64(authNetCustomerProfile.CustomerProfileID);
        }

        List<PaymentIntegrationSettingClientMapping> ISecurityRepository.GetPaymentIntegrationSettingsClientMappings()
        {
            return _dbNavigation.PaymentIntegrationSettingClientMappings.ToList();
        }

        //#region UAT-259: CLIENT USER SEARCH CONTROL
        //public List<ClientUserSearchContract> GetClientUserSearchData(String searchType, String tenantIDList, String agencyIDList, ClientUserSearchContract clientUserSearchContract, CustomPagingArgsContract customPagingArgsContract)
        //{
        //    EntityConnection connection = _dbNavigation.Connection as EntityConnection;
        //    List<ClientUserSearchContract> clientUserSearchData = new List<ClientUserSearchContract>();
        //    using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
        //    {
        //        SqlCommand cmd = new SqlCommand("usp_GetDataForClientSearch", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@searchType", searchType);
        //        cmd.Parameters.AddWithValue("@tenantIDList", tenantIDList);
        //        cmd.Parameters.AddWithValue("@agencyIDList", agencyIDList);
        //        cmd.Parameters.AddWithValue("@dataXML", clientUserSearchContract.CreateXml());
        //        cmd.Parameters.AddWithValue("@sortingAndPagingData", customPagingArgsContract.CreateXml());
        //        SqlDataAdapter adp = new SqlDataAdapter();
        //        adp.SelectCommand = cmd;
        //        DataSet ds = new DataSet();
        //        adp.Fill(ds);
        //        if (ds.Tables.Count > 0)
        //        {
        //            if (ds.Tables[0].Rows.Count > 0)
        //            {
        //                clientUserSearchData = ds.Tables[0].AsEnumerable().Select(col =>
        //                    new ClientUserSearchContract
        //                    {
        //                        ClientFirstName = col["ClientFirstName"] == DBNull.Value ? String.Empty : Convert.ToString(col["ClientFirstName"]),
        //                        ClientLastName = col["ClientLastName"] == DBNull.Value ? String.Empty : Convert.ToString(col["ClientLastName"]),
        //                        ClientUserName = col["ClientUserName"] == DBNull.Value ? String.Empty : Convert.ToString(col["ClientUserName"]),
        //                        EmailAddress = col["EmailAddress"] == DBNull.Value ? String.Empty : Convert.ToString(col["EmailAddress"]),
        //                        Phone = col["Phone"] == DBNull.Value ? String.Empty : Convert.ToString(col["Phone"]), // col["Phone"].ToString(),
        //                        OrganizationUserID = col["OrganizationUserID"] == DBNull.Value ? 0 : Convert.ToInt32(col["OrganizationUserID"]),
        //                        ClientTenantID = col["TenantID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(col["TenantID"]),
        //                        AgencyUserID = col["AgencyUserID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(col["AgencyUserID"]),
        //                        UserType = col["UserType"] == DBNull.Value ? String.Empty : Convert.ToString(col["UserType"]),
        //                        TenantName = col["TenantName"] == DBNull.Value ? String.Empty : Convert.ToString(col["TenantName"]),
        //                        AgencyName = col["AgencyName"] == DBNull.Value ? String.Empty : Convert.ToString(col["AgencyName"]),
        //                        AssignedRoles = col["AssignedRoles"] == DBNull.Value ? String.Empty : Convert.ToString(col["AssignedRoles"]),
        //                        UserID = col["UserID"] == DBNull.Value ? String.Empty : Convert.ToString(col["UserID"]),
        //                        LastLoginDateTime = col["LastLoginDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(col["LastLoginDate"]),
        //                        TotalCount = Convert.ToInt32(col["TotalCount"])
        //                    }).ToList();
        //            }
        //        }
        //    }
        //    return clientUserSearchData;
        //}
        //#endregion


        //UAT-4257
        public List<ClientUserSearchContract> GetClientUserSearchData(String searchType, String tenantIDList, String hierarchyNode, String agencyRootNodeIDList, String SelectedAgecnyIds, ClientUserSearchContract clientUserSearchContract, CustomPagingArgsContract customPagingArgsContract)
        {
            EntityConnection connection = _dbNavigation.Connection as EntityConnection;
            List<ClientUserSearchContract> clientUserSearchData = new List<ClientUserSearchContract>();
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("usp_GetDataForClientSearch", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@searchType", searchType);
                cmd.Parameters.AddWithValue("@tenantIDList", tenantIDList);
                cmd.Parameters.AddWithValue("@DPMIDs", hierarchyNode);
                cmd.Parameters.AddWithValue("@AgencyRootNodeIDs", agencyRootNodeIDList);
                cmd.Parameters.AddWithValue("@AgencyHierarchyIDs", SelectedAgecnyIds);
                cmd.Parameters.AddWithValue("@dataXML", clientUserSearchContract.CreateXml());
                cmd.Parameters.AddWithValue("@sortingAndPagingData", customPagingArgsContract.CreateXml());
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        clientUserSearchData = ds.Tables[0].AsEnumerable().Select(col =>
                            new ClientUserSearchContract
                            {
                                ClientFirstName = col["ClientFirstName"] == DBNull.Value ? String.Empty : Convert.ToString(col["ClientFirstName"]),
                                ClientLastName = col["ClientLastName"] == DBNull.Value ? String.Empty : Convert.ToString(col["ClientLastName"]),
                                ClientUserName = col["ClientUserName"] == DBNull.Value ? String.Empty : Convert.ToString(col["ClientUserName"]),
                                EmailAddress = col["EmailAddress"] == DBNull.Value ? String.Empty : Convert.ToString(col["EmailAddress"]),
                                Phone = col["Phone"] == DBNull.Value ? String.Empty : Convert.ToString(col["Phone"]), // col["Phone"].ToString(),
                                OrganizationUserID = col["OrganizationUserID"] == DBNull.Value ? 0 : Convert.ToInt32(col["OrganizationUserID"]),
                                ClientTenantID = col["TenantID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(col["TenantID"]),
                                AgencyUserID = col["AgencyUserID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(col["AgencyUserID"]),
                                UserType = col["UserType"] == DBNull.Value ? String.Empty : Convert.ToString(col["UserType"]),
                                TenantName = col["TenantName"] == DBNull.Value ? String.Empty : Convert.ToString(col["TenantName"]),
                                AgencyName = col["AgencyName"] == DBNull.Value ? String.Empty : Convert.ToString(col["AgencyName"]),
                                AssignedRoles = col["AssignedRoles"] == DBNull.Value ? String.Empty : Convert.ToString(col["AssignedRoles"]),
                                UserID = col["UserID"] == DBNull.Value ? String.Empty : Convert.ToString(col["UserID"]),
                                LastLoginDateTime = col["LastLoginDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(col["LastLoginDate"]),
                                TotalCount = Convert.ToInt32(col["TotalCount"])
                            }).ToList();
                    }
                }
            }
            return clientUserSearchData;
        }

        #region DataFeedFormatter



        List<Tenant> ISecurityRepository.GetListOfTenant()
        {
            return _dbNavigation.Tenants.Where(x => x.IsActive && !x.IsDeleted && x.lkpTenantType.TenantTypeCode.Equals("TTYCLI")).ToList();
        }

        public Boolean IsTenantActive(Int32 tenantId)
        {
            Tenant tenant = _dbNavigation.Tenants.Where(x => x.IsActive && !x.IsDeleted && x.lkpTenantType.TenantTypeCode.Equals("TTYCLI") && x.TenantID.Equals(tenantId)).FirstOrDefault();
            if (tenant.IsNotNull())
            {
                return tenant.IsActive;
            }
            return false;
        }

        List<DataFeedSettingContract> ISecurityRepository.GetDataFeedSetting(Int32 tenantId)
        {
            List<DataFeedSettingContract> dataFeedSettingContractList = new List<DataFeedSettingContract>();

            List<DataFeedSetting> dataFeedSettingList = _dbNavigation.DataFeedSettings.Include("lkpDataFeedIntervalMode")
                                                      .Where(cond => cond.WCFAccess.WCFA_TenantID == tenantId
                                                      && cond.WCFAccess.WCFA_IsDeleted == false && cond.DFS_IsDeleted == false
                                                      && cond.lkpEntityType.Code == DataFeedUtilityConstants.LKP_ENTITY_TYPE_BACKGROUND_ORDER)
                                                      .ToList();

            if (dataFeedSettingList.IsNotNull() && dataFeedSettingList.Count() > AppConsts.NONE)
            {
                foreach (DataFeedSetting dfs in dataFeedSettingList)
                {
                    DataFeedSettingContract dataFeedSettingCntrct = new DataFeedSettingContract();

                    dataFeedSettingCntrct.DataFeedSettingId = dfs.DFS_SettingID;
                    dataFeedSettingCntrct.AccessKey = dfs.WCFAccess.WCFA_AccessKey;
                    dataFeedSettingCntrct.TenantID = dfs.WCFAccess.WCFA_TenantID;
                    dataFeedSettingCntrct.ServiceIntervalDays = dfs.DFS_ServiceIntervalDays.IsNotNull()
                                                                        ? dfs.DFS_ServiceIntervalDays.Value : AppConsts.NONE;
                    dataFeedSettingCntrct.ServiceIntervalMonth = dfs.DFS_ServiceIntervalMonth.IsNotNull()
                                                                        ? dfs.DFS_ServiceIntervalMonth.Value : AppConsts.NONE;
                    dataFeedSettingCntrct.DataFeedIntervalModeID = dfs.DFS_DataFeedIntervalModeID;
                    dataFeedSettingCntrct.DataFeedIntervalModeCode = dfs.lkpDataFeedIntervalMode.IsNotNull() ? dfs.lkpDataFeedIntervalMode.Code : String.Empty;
                    dataFeedSettingCntrct.NoOfDays = dfs.DFS_NoOfDays.IsNotNull() ? dfs.DFS_NoOfDays.Value : AppConsts.NONE;
                    dataFeedSettingCntrct.NoOfMonths = dfs.DFS_NoOfMonths.IsNotNull() ? dfs.DFS_NoOfMonths.Value : AppConsts.NONE;
                    dataFeedSettingCntrct.IncludeOnlyNew = dfs.DFS_IncludeOnlyNew.IsNotNull() ? dfs.DFS_IncludeOnlyNew : false;
                    dataFeedSettingCntrct.IncludeCustomFields = dfs.DFS_IncludeCustomFields.IsNotNull() ? dfs.DFS_IncludeCustomFields : true;
                    dataFeedSettingCntrct.IncludeServiceGroup = dfs.DFS_IncludeAllServiceGroups.IsNotNull() ? dfs.DFS_IncludeAllServiceGroups : true;
                    dataFeedSettingCntrct.ReportingEmails = dfs.DFS_ReportingEmails;
                    dataFeedSettingCntrct.ReportingEmailSubject = dfs.DFS_ReportingEmailSubject.IsNullOrEmpty() ? "Today's ADB Shipment of Data is complete"
                                                                                                                : dfs.DFS_ReportingEmailSubject;

                    OutputSetting outputSettingTemp = dfs.OutputSettings.FirstOrDefault(cnd => cnd.OTS_IsDeleted == false);
                    if (!outputSettingTemp.IsNullOrEmpty())
                    {
                        dataFeedSettingCntrct.OutputID = outputSettingTemp.OTS_OutputID;
                        dataFeedSettingCntrct.OutputCode = outputSettingTemp.lkpOutput.IsNotNull() ? outputSettingTemp.lkpOutput.Code : String.Empty;
                        dataFeedSettingCntrct.DeliveryTypeId = outputSettingTemp.OTS_DeliveryTypeID;
                        dataFeedSettingCntrct.DeliveryTypeCode = outputSettingTemp.lkpDeliveryType.IsNotNull() ? outputSettingTemp.lkpDeliveryType.Code : String.Empty;
                        dataFeedSettingCntrct.FormatID = outputSettingTemp.OTS_FormatID;
                        dataFeedSettingCntrct.FieldSeparator = outputSettingTemp.OTS_FieldSeparator.IsNullOrEmpty() ? "," : outputSettingTemp.OTS_FieldSeparator;
                        dataFeedSettingCntrct.RowSeparator = outputSettingTemp.OTS_RowSeparator.IsNullOrEmpty()
                                                             ? String.Empty : outputSettingTemp.OTS_RowSeparator;
                        dataFeedSettingCntrct.DataFeedFTPDetailID = outputSettingTemp.DataFeedFTPDetail.IsNotNull()
                                                                                ? outputSettingTemp.DataFeedFTPDetail.DataFeedFTPDetailID
                                                                                : AppConsts.NONE;
                        dataFeedSettingCntrct.FileNameSuffix = outputSettingTemp.OTS_FileNameSuffix.IsNotNull()
                                                                                ? outputSettingTemp.OTS_FileNameSuffix
                                                                                : String.Empty;
                    }

                    List<DataFeedInvokeHistory> dataFeedInvokeHistoryData = dfs.DataFeedInvokeHistories.Where(cond => cond.DFIH_SettingID == dfs.DFS_SettingID
                                                                            && cond.lkpDataFeedInvokeResult.Code == DataFeedInvokeResult.Passed.GetStringValue()
                                                                            && cond.DFIH_IsDeleted == false)
                                                                        .ToList();

                    if (dataFeedInvokeHistoryData.IsNotNull() && dataFeedInvokeHistoryData.Count() > AppConsts.NONE)
                    {
                        dataFeedSettingCntrct.DataFeedInvokeDateTime = dataFeedInvokeHistoryData.Max(cond => cond.DFIH_DateTime).Value;

                        List<Int32> lstInvokeHistoryIds = dataFeedInvokeHistoryData.Select(slct => slct.DFIH_ID).ToList();
                        List<DataFeedInvokeHistoryDetail> lstDataFeedInvokeHistoryDetail = _dbNavigation.DataFeedInvokeHistoryDetails.Where(cond =>
                                                                                            lstInvokeHistoryIds.Contains(cond.DFIHD_DataFeedInvokeHistoryID)
                                                                                            && cond.DFIHD_IsDeleted == false).ToList();

                        dataFeedSettingCntrct.DataFeedInvokeDetailList = lstDataFeedInvokeHistoryDetail;
                    }

                    dataFeedSettingContractList.Add(dataFeedSettingCntrct);
                }
            }
            return dataFeedSettingContractList;
        }


        DataFeedSettingContract ISecurityRepository.GetDataFeedInvokeHistoryData(DataFeedSettingContract dataObject)
        {
            if (dataObject.IsNotNull())
            {
                List<DataFeedInvokeHistory> lstdataFeedInvokeHistory = _dbNavigation.DataFeedInvokeHistories.Where(cond => cond.DFIH_SettingID == dataObject.DataFeedSettingId
                                                                        && cond.DFIH_IsDeleted == false).ToList();
                if (lstdataFeedInvokeHistory.IsNotNull() && lstdataFeedInvokeHistory.Count > 0)
                {
                    List<Int32> lstInvokeHistoryIds = lstdataFeedInvokeHistory.Select(slct => slct.DFIH_ID).ToList();
                    List<DataFeedInvokeHistoryDetail> lstDataFeedInvokeHistoryDetail = _dbNavigation.DataFeedInvokeHistoryDetails.Where(cond =>
                                                                                        lstInvokeHistoryIds.Contains(cond.DFIHD_DataFeedInvokeHistoryID)
                                                                                        && cond.DFIHD_IsDeleted == false).ToList();
                    if (dataObject.RecordOriginStartDate.IsNullOrEmpty())
                    {
                        dataObject.RecordOriginStartDate = lstdataFeedInvokeHistory.Max(cond => cond.DFIH_DateTime);
                    }
                    dataObject.DataFeedInvokeDetailList = lstDataFeedInvokeHistoryDetail;
                }
            }
            return dataObject;
        }

        DataFeedFTPDetail ISecurityRepository.GetDataFeedFTPDetail(Int32 dataFeedFTPDetailID)
        {
            return _dbNavigation.DataFeedFTPDetails.Where(cond => cond.DataFeedFTPDetailID == dataFeedFTPDetailID && cond.IsDeleted == false).FirstOrDefault();
        }

        Boolean ISecurityRepository.SaveLogInInvokeHistory(String parameterXml, List<DataFeedInvokeHistoryDetail> lstdataFeedInvokeHistoryDetail,
            Int32 settingId, Int32 utilityUserId, Int32 dataFeedInvokeResultID)
        {
            DataFeedInvokeHistory newObjDataFeedInvokeHistory = new DataFeedInvokeHistory();

            newObjDataFeedInvokeHistory.DFIH_DateTime = DateTime.Now;
            newObjDataFeedInvokeHistory.DFIH_SettingID = settingId;
            newObjDataFeedInvokeHistory.DFIH_Parameter = parameterXml;
            newObjDataFeedInvokeHistory.DFIH_DataFeedInvokeResultID = dataFeedInvokeResultID;
            newObjDataFeedInvokeHistory.DFIH_IsDeleted = false;
            newObjDataFeedInvokeHistory.DFIH_CreatedOn = DateTime.Now;
            newObjDataFeedInvokeHistory.DFIH_CreatedBy = utilityUserId;

            if (lstdataFeedInvokeHistoryDetail.Count() > 0 && lstdataFeedInvokeHistoryDetail.IsNotNull())
            {
                foreach (DataFeedInvokeHistoryDetail dataFeedInvokeHistoryDetail in lstdataFeedInvokeHistoryDetail)
                {
                    newObjDataFeedInvokeHistory.DataFeedInvokeHistoryDetails.Add(dataFeedInvokeHistoryDetail);
                }
            }
            _dbNavigation.DataFeedInvokeHistories.AddObject(newObjDataFeedInvokeHistory);
            if (_dbNavigation.SaveChanges() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region UAT-806 Creation of granular permissions for Client Admin users

        //Boolean ISecurityRepository.GetUserGranularPermission(Int32 organizationUserId, List<Int32> lstSystemEntityIds, out Dictionary<String, String> dicPermissionList)
        //{
        //    List<SystemEntityUserPermission> lstSystemEntityUserPermission = _dbNavigation.SystemEntityUserPermissions.Where(cond => cond.SEUP_OrganisationUserId == organizationUserId && lstSystemEntityIds.Contains(cond.SystemEntityPermission.SEP_EntityId) && cond.SEUP_IsDeleted == false && cond.SystemEntityPermission.SEP_IsDeleted == false).ToList();
        //    if (lstSystemEntityUserPermission.IsNotNull() && lstSystemEntityUserPermission.Count > 0)
        //    {
        //        Dictionary<String, String> tempDicPermission = new Dictionary<String, String>();
        //        foreach (SystemEntityUserPermission obj in lstSystemEntityUserPermission)
        //        {
        //            if (obj.SystemEntityPermission.IsNotNull() && obj.SystemEntityPermission.LkpSystemEntity.IsNotNull() && !tempDicPermission.ContainsKey(obj.SystemEntityPermission.LkpSystemEntity.SE_CODE))
        //                tempDicPermission.Add(obj.SystemEntityPermission.LkpSystemEntity.SE_CODE, obj.SystemEntityPermission.SEP_PermissionCode);
        //        }
        //        dicPermissionList = tempDicPermission;
        //        return true;
        //    }
        //    else
        //    {
        //        dicPermissionList = new Dictionary<string, string>();
        //        return false;
        //    }
        //}

        //UAT-1416 Granular permissions additions to background order result permission
        Boolean ISecurityRepository.GetUserGranularPermission(Int32 organizationUserId, out Dictionary<String, String> dicPermissionList)
        {
            List<SystemEntityUserPermission> lstSystemEntityUserPermission = _dbNavigation.SystemEntityUserPermissions
                    .Where(cond => cond.SEUP_OrganisationUserId == organizationUserId && cond.SEUP_IsDeleted == false
                        && cond.SystemEntityPermission.SEP_IsDeleted == false).ToList();
            if (lstSystemEntityUserPermission.IsNotNull() && lstSystemEntityUserPermission.Count > 0)
            {
                Dictionary<String, String> tempDicPermission = new Dictionary<String, String>();
                foreach (SystemEntityUserPermission obj in lstSystemEntityUserPermission)
                {
                    if (obj.SystemEntityPermission.IsNotNull() && obj.SystemEntityPermission.LkpSystemEntity.IsNotNull())
                    {
                        if (!tempDicPermission.ContainsKey(obj.SystemEntityPermission.LkpSystemEntity.SE_CODE))
                        {
                            tempDicPermission.Add(obj.SystemEntityPermission.LkpSystemEntity.SE_CODE, obj.SystemEntityPermission.SEP_PermissionCode);
                        }
                        else if (tempDicPermission.ContainsKey(obj.SystemEntityPermission.LkpSystemEntity.SE_CODE)
                            && obj.SystemEntityPermission.LkpSystemEntity.SE_CODE == EnumSystemEntity.BKG_ORDER_RESULT_REPORT.GetStringValue())
                        {
                            String value = tempDicPermission.GetValue(EnumSystemEntity.BKG_ORDER_RESULT_REPORT.GetStringValue());
                            value = value + "," + obj.SystemEntityPermission.SEP_PermissionCode;
                            tempDicPermission[EnumSystemEntity.BKG_ORDER_RESULT_REPORT.GetStringValue()] = value;
                        }
                        else if (tempDicPermission.ContainsKey(obj.SystemEntityPermission.LkpSystemEntity.SE_CODE)
                            && obj.SystemEntityPermission.LkpSystemEntity.SE_CODE == EnumSystemEntity.STUDENT_BUCKET_ASSIGNMENT.GetStringValue())
                        {
                            String value = tempDicPermission.GetValue(EnumSystemEntity.STUDENT_BUCKET_ASSIGNMENT.GetStringValue());
                            value = value + "," + obj.SystemEntityPermission.SEP_PermissionCode;
                            tempDicPermission[EnumSystemEntity.STUDENT_BUCKET_ASSIGNMENT.GetStringValue()] = value;
                        }
                    }
                }
                dicPermissionList = tempDicPermission;
                return true;
            }
            else
            {
                dicPermissionList = new Dictionary<string, string>();
                return false;
            }
        }

        #endregion

        #region Manage System Entity Permission
        List<SystemEntityUserPermissionData> ISecurityRepository.GetSystemEntityUserPermissionList(SearchItemDataContract searchDataContract, CustomPagingArgsContract gridCustomPaging, Int32 selectedTenantID, Int32 selectedEntityId)
        {
            string orderBy = QueueConstants.USER_SEARCH_DEFAULT_SORTING_FIELDS;
            string ordDirection = null;

            orderBy = String.IsNullOrEmpty(gridCustomPaging.SortExpression) ? orderBy : gridCustomPaging.SortExpression;
            ordDirection = gridCustomPaging.SortDirectionDescending == false ? null : "desc";

            EntityConnection connection = _dbNavigation.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetSystemEntityUserPermissions", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TenantId", selectedTenantID);
                command.Parameters.AddWithValue("@EntityID", selectedEntityId);
                command.Parameters.AddWithValue("@FirstName", searchDataContract.ApplicantFirstName);
                command.Parameters.AddWithValue("@LastName", searchDataContract.ApplicantLastName);
                command.Parameters.AddWithValue("@EmailAddress", searchDataContract.EmailAddress);
                command.Parameters.AddWithValue("@OrderBy", orderBy);
                command.Parameters.AddWithValue("@OrderDirection", ordDirection);
                command.Parameters.AddWithValue("@PageIndex", gridCustomPaging.CurrentPageIndex);
                command.Parameters.AddWithValue("@PageSize", gridCustomPaging.PageSize);

                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    List<SystemEntityUserPermissionData> systemEntityUserPermissionData = new List<SystemEntityUserPermissionData>();
                    systemEntityUserPermissionData = ds.Tables[0].AsEnumerable().Select(col =>
                          new SystemEntityUserPermissionData
                          {
                              SEUP_ID = Convert.ToInt32(col["SEUP_ID"]),
                              EntityPermissionId = col["EntityPermissionId"] == DBNull.Value ? "0" : Convert.ToString(col["EntityPermissionId"]),
                              PermissionName = col["PermissionName"] == DBNull.Value ? String.Empty : Convert.ToString(col["PermissionName"]),
                              Permissioncode = col["Permissioncode"] == DBNull.Value ? String.Empty : Convert.ToString(col["Permissioncode"]),
                              OrganizationUserId = Convert.ToInt32(col["OrganizationUserId"]),
                              UserFirstName = col["UserFirstName"] == DBNull.Value ? String.Empty : Convert.ToString(col["UserFirstName"]),
                              UserLastName = col["UserLastName"] == DBNull.Value ? String.Empty : Convert.ToString(col["UserLastName"]),
                              EmailAddress = col["EmailAddress"] == DBNull.Value ? String.Empty : Convert.ToString(col["EmailAddress"]),
                              TenantID = Convert.ToInt32(col["TenantID"]),
                              TotalCount = col["TotalCount"] == DBNull.Value ? 0 : Convert.ToInt32(col["TotalCount"]),
                              HierarchyNodeId = col["DPMId"] == DBNull.Value ? 0 : Convert.ToInt32(col["DPMId"]),
                              HierarchyNodeLabel = col["DPMLabel"] == DBNull.Value ? String.Empty : Convert.ToString(col["DPMLabel"])

                          }).ToList();
                    return systemEntityUserPermissionData;
                }
                return new List<SystemEntityUserPermissionData>();
            }
        }

        List<OrgUser> ISecurityRepository.GetOrgUserListForAsigningPermission(Int32 CurrentUserId, Int32 entityId, Int32 selectedTenantId)
        {
            Boolean IsBkgOrderResultReportEntity = false;
            String bkgOrderResultReport = EnumSystemEntity.BKG_ORDER_RESULT_REPORT.GetStringValue();
            IsBkgOrderResultReportEntity = _dbNavigation.LkpSystemEntities.Where(x => x.SE_ID == entityId && x.SE_CODE == bkgOrderResultReport && !x.SE_IsDeleted).Any();
            if (IsBkgOrderResultReportEntity)
            {
                return _dbNavigation.OrganizationUsers.Include("Organization").Include("Organization.Tenant")
                                                            .Where(cond => cond.Organization.Tenant.TenantID == selectedTenantId
                                                            //&& !UserIdAlreadyMapped.Contains(cond.OrganizationUserID)
                                                            && !cond.IsApplicant.Value
                                                            && !cond.Organization.IsDeleted
                                                            && !cond.IsDeleted)
                                                            .Select(col => new OrgUser
                                                            {
                                                                UserID = col.OrganizationUserID,
                                                                UserName = col.FirstName + "," + col.LastName
                                                            }).ToList();
            }
            else
            {
                var UserIdAlreadyMapped = _dbNavigation.SystemEntityUserPermissions.Where(x => x.SystemEntityPermission.SEP_EntityId == entityId
                                                                                        && !x.SEUP_IsDeleted && !x.SystemEntityPermission.SEP_IsDeleted
                                                                                        && x.SEUP_OrganisationUserId != CurrentUserId)
                                                                                        .Select(x => x.SEUP_OrganisationUserId);

                return _dbNavigation.OrganizationUsers.Include("Organization").Include("Organization.Tenant")
                                                             .Where(cond => cond.Organization.Tenant.TenantID == selectedTenantId
                                                             && !UserIdAlreadyMapped.Contains(cond.OrganizationUserID)
                                                             && !cond.IsApplicant.Value
                                                             && !cond.Organization.IsDeleted
                                                             && !cond.IsDeleted)
                                                             .Select(col => new OrgUser
                                                             {
                                                                 UserID = col.OrganizationUserID,
                                                                 UserName = col.FirstName + "," + col.LastName
                                                             }).ToList();
            }
        }

        List<SystemEntityPermission> ISecurityRepository.GetPermissionByEntityId(Int32 entityId)
        {
            return _dbNavigation.SystemEntityPermissions.Where(cond => cond.SEP_EntityId == entityId && !cond.SEP_IsDeleted).ToList();
        }

        Boolean ISecurityRepository.SaveUpdateEntityUserPermission(SystemEntityUserPermission systemEntityUserPermission, Int32 userId, Dictionary<int, bool> lstSelectedBkgOdrResPermissions)
        {
            if (lstSelectedBkgOdrResPermissions.IsNull())
            {
                if (systemEntityUserPermission.SEUP_ID > AppConsts.NONE)
                {
                    SystemEntityUserPermission existingSystemEntityUserPermission = _dbNavigation.SystemEntityUserPermissions.Where(cond => cond.SEUP_ID == systemEntityUserPermission.SEUP_ID
                                                                                                        && !cond.SEUP_IsDeleted).FirstOrDefault();
                    existingSystemEntityUserPermission.SEUP_EntityPermissionId = systemEntityUserPermission.SEUP_EntityPermissionId;
                    existingSystemEntityUserPermission.SEUP_ModifiedByID = userId;
                    existingSystemEntityUserPermission.SEUP_ModifiedOn = DateTime.UtcNow;
                }
                else
                {
                    systemEntityUserPermission.SEUP_IsDeleted = false;
                    systemEntityUserPermission.SEUP_CreatedByID = userId;
                    systemEntityUserPermission.SEUP_CreatedOn = DateTime.UtcNow;
                    _dbNavigation.SystemEntityUserPermissions.AddObject(systemEntityUserPermission);
                }
            }
            else
            {
                if (systemEntityUserPermission.SEUP_ID > AppConsts.NONE)
                {
                    var existingPermissions= new List<SystemEntityUserPermission>();
                    //4522 Ability to set granular permissions by node for background checks by admin. 
                    if (systemEntityUserPermission.SEUP_DPMId.HasValue)
                    {
                        existingPermissions= _dbNavigation.SystemEntityUserPermissions.Where(cond =>
                                                                 cond.SEUP_OrganisationUserId == systemEntityUserPermission.SEUP_OrganisationUserId
                                                                 && lstSelectedBkgOdrResPermissions.Keys.Contains(cond.SEUP_EntityPermissionId)
                                                                 && cond.SEUP_IsDeleted != true && cond.SEUP_DPMId== systemEntityUserPermission.SEUP_DPMId.Value
                                                            ).ToList();

                    }
                    else
                    {
                         existingPermissions = _dbNavigation.SystemEntityUserPermissions.Where(cond =>
                                                                cond.SEUP_OrganisationUserId == systemEntityUserPermission.SEUP_OrganisationUserId
                                                                && lstSelectedBkgOdrResPermissions.Keys.Contains(cond.SEUP_EntityPermissionId)
                                                                && cond.SEUP_IsDeleted != true
                                                            ).ToList();

                    }
                    //Remove permissions
                    List<int> needToRemoveItems = lstSelectedBkgOdrResPermissions.Where(cond => cond.Value == false).Select(n => n.Key).ToList();
                    var needToRemoveUserPermission = existingPermissions.Where(cond => needToRemoveItems.Contains(cond.SEUP_EntityPermissionId)).ToList();
                    if (needToRemoveUserPermission != null && needToRemoveUserPermission.Count > 0)
                    {
                        foreach (var item in needToRemoveUserPermission)
                        {
                            item.SEUP_IsDeleted = true;
                            item.SEUP_ModifiedByID = userId;
                            item.SEUP_ModifiedOn = DateTime.Now;
                        }
                    }

                    //Add Permissions
                    List<int> needToAddItems = lstSelectedBkgOdrResPermissions.Where(cond => cond.Value == true).Select(n => n.Key).ToList();
                    var alredyExistUserPermission = existingPermissions.Where(cond => needToAddItems.Contains(cond.SEUP_EntityPermissionId)).ToList();

                    lstSelectedBkgOdrResPermissions.Where(cond => cond.Value == true).ForEach(item =>
                    {
                        if (!(alredyExistUserPermission.Exists(cond => cond.SEUP_EntityPermissionId == Convert.ToInt32(item.Key))))
                        {
                            SystemEntityUserPermission entityUserPermission = new SystemEntityUserPermission();
                            entityUserPermission.SEUP_EntityPermissionId = Convert.ToInt32(item.Key);
                            entityUserPermission.SEUP_OrganisationUserId = systemEntityUserPermission.SEUP_OrganisationUserId;
                            entityUserPermission.SEUP_IsDeleted = false;
                            entityUserPermission.SEUP_CreatedByID = userId;
                            entityUserPermission.SEUP_CreatedOn = DateTime.UtcNow;
                            //4522 Ability to set granular permissions by node for background checks by admin. 
                            if (systemEntityUserPermission.SEUP_DPMId.HasValue)
                                entityUserPermission.SEUP_DPMId = systemEntityUserPermission.SEUP_DPMId.Value;
                            //4522 Ability to set granular permissions by node for background checks by admin. 
                            if (systemEntityUserPermission.SEUP_TenantId.HasValue)
                                entityUserPermission.SEUP_TenantId = systemEntityUserPermission.SEUP_TenantId.Value;
                            _dbNavigation.SystemEntityUserPermissions.AddObject(entityUserPermission);
                        }
                    });
                }
                else
                {
                    lstSelectedBkgOdrResPermissions.Where(cond => cond.Value == true).ForEach(item =>
                    {
                        SystemEntityUserPermission entityUserPermission = new SystemEntityUserPermission();
                        entityUserPermission.SEUP_EntityPermissionId = Convert.ToInt32(item.Key);
                        entityUserPermission.SEUP_OrganisationUserId = systemEntityUserPermission.SEUP_OrganisationUserId;
                        entityUserPermission.SEUP_IsDeleted = false;
                        entityUserPermission.SEUP_CreatedByID = userId;
                        entityUserPermission.SEUP_CreatedOn = DateTime.UtcNow;
                        //4522 Ability to set granular permissions by node for background checks by admin. 
                        if (systemEntityUserPermission.SEUP_DPMId.HasValue)
                            entityUserPermission.SEUP_DPMId = systemEntityUserPermission.SEUP_DPMId.Value;
                        //4522 Ability to set granular permissions by node for background checks by admin. 
                        if (systemEntityUserPermission.SEUP_TenantId.HasValue)
                            entityUserPermission.SEUP_TenantId = systemEntityUserPermission.SEUP_TenantId.Value;
                        _dbNavigation.SystemEntityUserPermissions.AddObject(entityUserPermission);
                    });
                }
            }
            if (_dbNavigation.SaveChanges() > 0)
                return true;
            return false;
        }

        Boolean ISecurityRepository.DeleteEntityUserPermission(Int32 systemEntityUserPermissionId, Int32 userId, Int32 organisationUserId, List<Int32> lstEntityPermissionIds,Int32? dpmId)
        {
            if (lstEntityPermissionIds.IsNull())
            {
                SystemEntityUserPermission existingSystemEntityUserPermission = _dbNavigation.SystemEntityUserPermissions.Where(cond => cond.SEUP_ID == systemEntityUserPermissionId
                                                                                                        && !cond.SEUP_IsDeleted).FirstOrDefault();
                if (existingSystemEntityUserPermission != null)
                {
                    existingSystemEntityUserPermission.SEUP_IsDeleted = true;
                    existingSystemEntityUserPermission.SEUP_ModifiedByID = userId;
                    existingSystemEntityUserPermission.SEUP_ModifiedOn = DateTime.UtcNow;
                }
            }
            else
            {
                List<SystemEntityUserPermission> lstExistingSystemEntityUserPermission = new List<SystemEntityUserPermission>();
                //4522 Ability to set granular permissions by node for background checks by admin. 
                if (dpmId.HasValue)
                {
                    lstExistingSystemEntityUserPermission = _dbNavigation.SystemEntityUserPermissions.Where(cond =>
                                                                        cond.SEUP_OrganisationUserId == organisationUserId
                                                                        && lstEntityPermissionIds.Contains(cond.SEUP_EntityPermissionId) && cond.SEUP_DPMId==dpmId.Value
                                                                        && !cond.SEUP_IsDeleted).ToList();
                }
                else
                {
                    lstExistingSystemEntityUserPermission = _dbNavigation.SystemEntityUserPermissions.Where(cond =>
                                                                                         cond.SEUP_OrganisationUserId == organisationUserId
                                                                                         && lstEntityPermissionIds.Contains(cond.SEUP_EntityPermissionId)
                                                                                         && !cond.SEUP_IsDeleted).ToList();
                }
                foreach (var item in lstExistingSystemEntityUserPermission)
                {
                    item.SEUP_IsDeleted = true;
                    item.SEUP_ModifiedByID = userId;
                    item.SEUP_ModifiedOn = DateTime.UtcNow;
                }
            }

            if (_dbNavigation.SaveChanges() > 0)
                return true;
            return false;
        }
        #endregion

        #region User Last Login Activity.
        UserLoginHistory ISecurityRepository.AddUserLoginActivity(Int32 organizationUserId, String currentSessionId)
        {
            UserLoginHistory existingCurrentUserLoginHistory = _dbNavigation.UserLoginHistories.Where(cond => cond.ULH_OrganisationUserID == organizationUserId
                                                                                              && cond.ULH_SessionID == currentSessionId && cond.ULH_IsDeleted == false
                                                                                              && cond.ULH_LogoutTime == null).FirstOrDefault();
            if (existingCurrentUserLoginHistory.IsNull())
            {
                UserLoginHistory organizationUserLoginHistory = new UserLoginHistory();
                organizationUserLoginHistory.ULH_OrganisationUserID = organizationUserId;
                organizationUserLoginHistory.ULH_SessionID = currentSessionId;
                organizationUserLoginHistory.ULH_LoginTime = DateTime.UtcNow;
                _dbNavigation.UserLoginHistories.AddObject(organizationUserLoginHistory);
                if (_dbNavigation.SaveChanges() > 0)
                {
                    return organizationUserLoginHistory;
                }
            }
            return existingCurrentUserLoginHistory;
        }

        Boolean ISecurityRepository.UpdateUserLoginActivity(Int32 organizationUserId, String currentSessionId, Boolean IsSessionTimeout, Int32 userLoginHistoryID)
        {
            //UserLoginHistory CurrentUserLoginHistory = _dbNavigation.UserLoginHistories.Where(cond => cond.ULH_OrganisationUserID == organizationUserId
            //                                                                           && cond.ULH_SessionID == currentSessionId && cond.ULH_IsDeleted == false).FirstOrDefault();


            UserLoginHistory CurrentUserLoginHistory = _dbNavigation.UserLoginHistories.Where(cond => cond.ULH_ID == userLoginHistoryID
                                                                                       && cond.ULH_IsDeleted == false).FirstOrDefault();


            if (userLoginHistoryID <= 0)
            {
                CurrentUserLoginHistory = _dbNavigation.UserLoginHistories.Where(cond => cond.ULH_OrganisationUserID == organizationUserId
                                                                                          && cond.ULH_SessionID == currentSessionId
                                                                                          && cond.ULH_IsDeleted == false).FirstOrDefault();
            }


            if (CurrentUserLoginHistory.IsNotNull())
            {
                CurrentUserLoginHistory.ULH_LogoutTime = DateTime.UtcNow;
                CurrentUserLoginHistory.ULH_IsSessionTimeout = IsSessionTimeout;
                if (_dbNavigation.SaveChanges() > 0)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        List<UserLoginHistory> ISecurityRepository.GetApplicantLastLoginDetail(Int32 organizationUserId, String currentSessionId)
        {
            List<UserLoginHistory> currentUserLastLoginHistoryList = new List<UserLoginHistory>();
            currentUserLastLoginHistoryList = _dbNavigation.UserLoginHistories.Where(cond => cond.ULH_OrganisationUserID == organizationUserId
                                                       && cond.ULH_IsDeleted == false).ToList();

            return currentUserLastLoginHistoryList;
        }
        #endregion

        /// <summary>
        /// To save service logging data
        /// </summary>
        /// <param name="serviceLoggingContract"></param>
        /// <returns></returns>
        public Boolean SaveServiceLoggingDetail(ServiceLoggingContract serviceLoggingContract)
        {
            ServiceLoggingDetail serviceLogging = new ServiceLoggingDetail();
            serviceLogging.SLD_ServiceName = serviceLoggingContract.ServiceName;
            serviceLogging.SLD_JobName = serviceLoggingContract.JobName;
            serviceLogging.SLD_TenantID = serviceLoggingContract.TenantID;
            serviceLogging.SLD_JobStartTime = serviceLoggingContract.JobStartTime;
            serviceLogging.SLD_JobEndTime = serviceLoggingContract.JobEndTime;
            serviceLogging.SLD_Comments = serviceLoggingContract.Comments;
            serviceLogging.SLD_IsDeleted = serviceLoggingContract.IsDeleted;
            serviceLogging.SLD_CreatedBy = serviceLoggingContract.CreatedBy;
            serviceLogging.SLD_CreatedOn = serviceLoggingContract.CreatedOn;

            _dbNavigation.ServiceLoggingDetails.AddObject(serviceLogging);
            _dbNavigation.SaveChanges();
            return true;
        }

        #region UAT-1049: Data Entry Enhanchment

        public DataTable GetDataEntryQueueData(String inuputXml, CustomPagingArgsContract gridCustomPaging, Int32? CurrentLoggedInUserID)
        {
            EntityConnection connection = _dbNavigation.Connection as EntityConnection;

            String orderBy = "DocumentName";
            String ordDirection = null;

            orderBy = String.IsNullOrEmpty(gridCustomPaging.SortExpression) ? orderBy : gridCustomPaging.SortExpression;
            ordDirection = gridCustomPaging.SortDirectionDescending == false ? String.IsNullOrEmpty(gridCustomPaging.SortExpression) ? "desc" : "asc" : "desc";

            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("usp_GetDataEntryQueueData", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@InuputXml", inuputXml);
                command.Parameters.AddWithValue("@filterXml", gridCustomPaging.XML);
                command.Parameters.AddWithValue("@CurrentLoggedInUserID", CurrentLoggedInUserID);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 1)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        gridCustomPaging.CurrentPageIndex = Convert.ToInt32(Convert.ToString(ds.Tables[0].Rows[0]["CurrentPageIndex"]));
                        gridCustomPaging.VirtualPageCount = Convert.ToInt32(Convert.ToString(ds.Tables[0].Rows[0]["VirtualCount"]));
                    }
                    return ds.Tables[1];
                }
            }
            return new DataTable();
        }

        public Boolean AssignDocumentToUserForDataEntry(Int32 selectedAssigneeUserId, String selectedAssigneeUserName, List<Int32> documentIdsToAssign,
                                                        Int32 currentloggedInUserId)
        {
            List<FlatDataEntryQueue> lstDataEntryQueueToUpdate = _dbNavigation.FlatDataEntryQueues.
                                                                  Where(cnd => documentIdsToAssign.Contains(cnd.FDEQ_ID) && cnd.FDEQ_IsDeleted == false).ToList();
            lstDataEntryQueueToUpdate.ForEach(dataEntryData =>
                {
                    dataEntryData.FDEQ_ModifiedByID = currentloggedInUserId;
                    dataEntryData.FDEQ_ModifiedOn = DateTime.Now;
                    dataEntryData.FDEQ_AssignToUserID = selectedAssigneeUserId;
                    dataEntryData.FDEQ_AssignToUserName = selectedAssigneeUserName;
                });

            if (_dbNavigation.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }

        public Boolean DeleteDocumentFromFlatDataEntry(Int32 applicantDocumentId, Int32 tenantId, Int32 applicantOrgUserId, Int32 currentloggedInUserId)
        {
            FlatDataEntryQueue dataEntryQueueDataToDelete = _dbNavigation.FlatDataEntryQueues.Where(cnd => cnd.FDEQ_ApplicantDocumentID == applicantDocumentId
                                                             && cnd.FDEQ_ApplicantOrganizationUserID == applicantOrgUserId && cnd.FDEQ_TenantID == tenantId).FirstOrDefault();

            if (dataEntryQueueDataToDelete.IsNotNull())
            {
                dataEntryQueueDataToDelete.FDEQ_IsDeleted = true;
                dataEntryQueueDataToDelete.FDEQ_ModifiedByID = currentloggedInUserId;
                dataEntryQueueDataToDelete.FDEQ_ModifiedOn = DateTime.Now;
                if (_dbNavigation.SaveChanges() > AppConsts.NONE)
                    return true;
                return false;
            }
            return false;
        }

        /// <summary>
        /// Gets the FlatDataEntryQueue, by Primary Key of table
        /// </summary>
        /// <param name="fdeqId"></param>
        /// <returns></returns>
        FlatDataEntryQueue ISecurityRepository.GetFlatDataEntryQueueRecord(Int32 fdeqId)
        {
            return _dbNavigation.FlatDataEntryQueues.Where(cnd => cnd.FDEQ_ID == fdeqId && cnd.FDEQ_IsDeleted == false).First();
        }

        #endregion

        #region Profile Sharing (Some Methods can not be moved to ProfileSharingRepo Beacause they are accessing security DB entities like OrganizationUser)

        /// <summary>
        /// Returns whether the Shared user is being invited
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        Boolean ISecurityRepository.IsSharedUserInvited(String emailAddress)
        {
            var _memberShip = _dbNavigation.aspnet_Membership.Where(amu => amu.Email.ToLower() == emailAddress.ToLower()).FirstOrDefault();
            if (_memberShip.IsNullOrEmpty())
            {
                return true;
            }
            else
            {
                var _orgUser = _dbNavigation.OrganizationUsers.Where(ou => ou.UserID == _memberShip.UserId).First();
                if (_orgUser.IsSharedUser.IsNull())
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// Get the OrganizationUserID of the user to whom invitation is being sent or else return 0.
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        Int32 ISecurityRepository.GetSharedUserOrgId(String emailAddress)
        {
            var _orgUserId = AppConsts.NONE;

            var _existingUser = _dbNavigation.aspnet_Membership.Where(anm => anm.Email.ToLower() == emailAddress.ToLower()).FirstOrDefault();
            if (_existingUser.IsNotNull())
            {
                _orgUserId = _dbNavigation.OrganizationUsers.Where(ou => ou.UserID == _existingUser.UserId && (ou.IsSharedUser ?? false) == true).Select(x => x.OrganizationUserID).FirstOrDefault();
            }
            return _orgUserId;
        }


        /// <summary>
        /// Get the OrganizationUserID of the All the users to whom invitation is being sent by Admin, Client Admin
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        Dictionary<String, Int32> ISecurityRepository.GetSharedUserOrgIds(List<String> lstEmailAddress)
        {
            var lstEmailAddresses = lstEmailAddress.Select(x => x.ToLower()).ToList();
            var _data = new Dictionary<String, Int32>();
            var _lstExistingUser = _dbNavigation.aspnet_Membership.Where(anm => lstEmailAddresses.Contains(anm.Email.ToLower())).Select(anm => anm.UserId).ToList();

            if (!_lstExistingUser.IsNullOrEmpty())
            {
                var _allUsers = _dbNavigation.OrganizationUsers.Where(ou => _lstExistingUser.Contains(ou.UserID) && ou.IsSharedUser != null && ou.IsSharedUser == true).ToList();
                List<String> sharedUserTypeCodes = new List<String>();
                sharedUserTypeCodes.Add(OrganizationUserType.AgencyUser.GetStringValue());
                sharedUserTypeCodes.Add(OrganizationUserType.ApplicantsSharedUser.GetStringValue());
                foreach (var userData in _allUsers)
                {
                    if (userData.OrganizationUserTypeMappings.Where(d => sharedUserTypeCodes.Contains(d.lkpOrgUserType.OrgUserTypeCode)).Any())
                        _data.Add(userData.aspnet_Users.aspnet_Membership.Email.ToLower(), userData.OrganizationUserID);
                }
            }
            return _data;
        }

        //#region Attestation Document Code

        //Int32 ISecurityRepository.SaveAttestationDocument(String pdfDocPath, String documentTypeCode, Int32 currentLoggedInUserID)
        //{
        //    if (!String.IsNullOrEmpty(pdfDocPath))
        //    {
        //        Int32 documentTypeId = _dbNavigation.lkpDocumentTypes.Where(cond => cond.DT_Code == documentTypeCode).FirstOrDefault().DT_ID;

        //        InvitationDocument invitationDocument = new InvitationDocument()
        //        {
        //            IND_DocumentFilePath = pdfDocPath,
        //            IND_DocumentType = documentTypeId,
        //            IND_IsDeleted = false,
        //            IND_CreatedByID = currentLoggedInUserID,
        //            IND_CreatedOn = DateTime.Now
        //        };

        //        _dbNavigation.InvitationDocuments.AddObject(invitationDocument);
        //        if (_dbNavigation.SaveChanges() > 0)
        //        {
        //            return invitationDocument.IND_ID;
        //        }
        //        else
        //        {
        //            return 0;
        //        }
        //    }
        //    return 0;
        //}

        //Boolean ISecurityRepository.SaveInvitationDocumentMapping(List<InvitationDocumentMapping> lstInvitationDocumentMapping)
        //{
        //    foreach (InvitationDocumentMapping newInvitationDocumentMapping in lstInvitationDocumentMapping)
        //    {
        //        _dbNavigation.InvitationDocumentMappings.AddObject(newInvitationDocumentMapping);
        //    }
        //    if (_dbNavigation.SaveChanges() > 0)
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        ///// <summary>
        ///// Method to Get Invitation Data by Invite Token
        ///// </summary>
        ///// <param name="inviteToken"></param>
        ///// <returns></returns>
        //ProfileSharingInvitation ISecurityRepository.GetInvitationDataByToken(Guid inviteToken)
        //{
        //    ProfileSharingInvitation invitation = _dbNavigation.ProfileSharingInvitations.Where(cond => cond.PSI_Token == inviteToken && !cond.PSI_IsDeleted).FirstOrDefault();
        //    if (invitation.IsNotNull())
        //    {
        //        return invitation;
        //    }
        //    return new ProfileSharingInvitation();
        //}

        //#endregion

        ///// <summary>
        ///// Gets the list of invitations that has been sent by the applicant
        ///// </summary>
        ///// <param name="applicantOrgUserId"></param>
        ///// <param name="tenantId"></param>
        ///// <returns></returns>
        //DataTable ISecurityRepository.GetApplicantInvitations(Int32 applicantOrgUserId, Int32 tenantId)
        //{
        //    EntityConnection connection = _dbNavigation.Connection as EntityConnection;
        //    using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
        //    {

        //        SqlCommand command = new SqlCommand("usp_GetApplicantInvitations", con);
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.Parameters.AddWithValue("@ApplicantOrgUserId", applicantOrgUserId);
        //        command.Parameters.AddWithValue("@TenantId", tenantId);
        //        SqlDataAdapter adp = new SqlDataAdapter();
        //        adp.SelectCommand = command;
        //        DataSet ds = new DataSet();
        //        adp.Fill(ds);
        //        if (ds.Tables.Count > 0)
        //        {
        //            return ds.Tables[0];
        //        }
        //    }
        //    return new DataTable();
        //}

        ///// <summary>
        ///// Method to check whether shared user has already account or not(RG)
        ///// </summary>
        ///// <param name="inviteToken"></param>
        ///// <returns></returns>
        //public Boolean IsSharedUserExists(Guid inviteToken)
        //{
        //    //String inviteeEmail = _dbNavigation.ProfileSharingInvitations.Where(cond => cond.PSI_Token == inviteToken && !cond.PSI_IsDeleted).Select(x => x.PSI_InviteeEmail).FirstOrDefault();
        //    //if (!inviteeEmail.IsNullOrEmpty())
        //    //{
        //    //    return _dbNavigation.aspnet_Membership.Any(cond => cond.LoweredEmail == inviteeEmail.ToLower());
        //    //}
        //    //return false;
        //    if (!inviteToken.IsNullOrEmpty())
        //    {
        //        Int32 inviteeOrgUserID = _dbNavigation.ProfileSharingInvitations.Where(cond => cond.PSI_Token == inviteToken && !cond.PSI_IsDeleted).Select(x => x.PSI_InviteeOrgUserID).FirstOrDefault() ?? 0;
        //        if (inviteeOrgUserID > 0)
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        ///// <summary>
        ///// Method to get Shared User Data from Invitation Sent by applicant(currently only Email)(RG)
        ///// </summary>
        ///// <param name="inviteToken"></param>
        ///// <returns></returns>
        //public String GetSharedUserDataFromInvitation(Guid inviteToken)
        //{
        //    if (!inviteToken.IsNullOrEmpty())
        //    {
        //        return _dbNavigation.ProfileSharingInvitations.Where(cond => cond.PSI_Token == inviteToken && !cond.PSI_IsDeleted).Select(x => x.PSI_InviteeEmail).FirstOrDefault();
        //    }
        //    return null;
        //}

        ///// <summary>
        ///// Method to Update Invitee Organization UserID in ProfileSharingInvitation and AgencyUser table
        ///// </summary>
        ///// <param name="orgUserID"></param>
        ///// <param name="inviteToken"></param>
        ///// <returns></returns>
        //public Boolean UpdateInviteeOrganizationUserID(Int32 orgUserID, Guid inviteToken, Int32 adminInitializedStatusID)
        //{
        //    if (!inviteToken.IsNullOrEmpty())
        //    {
        //        //Updating invitee OrganizationUserID in ProfileSharingInvitation for all invitation sent to that invitee.
        //        ProfileSharingInvitation tempIinvitation = _dbNavigation.ProfileSharingInvitations.Where(cond => cond.PSI_Token == inviteToken && !cond.PSI_IsDeleted).FirstOrDefault();

        //        List<ProfileSharingInvitation> lstProfileSharingInvitation = _dbNavigation.ProfileSharingInvitations
        //                                                                                .Where(cond => cond.PSI_InviteeEmail.ToLower() == tempIinvitation.PSI_InviteeEmail.ToLower()
        //                                                                                    && cond.PSI_InvitationStatusID != adminInitializedStatusID
        //                                                                                    && !cond.PSI_IsDeleted).ToList();
        //        foreach (var invitation in lstProfileSharingInvitation)
        //        {
        //            if (invitation.PSI_InviteeOrgUserID.IsNull())
        //            {
        //                invitation.PSI_InviteeOrgUserID = orgUserID;
        //            }
        //        }

        //        //Updating userid in AgencyUser if invitation sent by admin or client admin
        //        if (tempIinvitation.lkpInvitationSource.Code == InvitationSourceTypes.ADMIN.GetStringValue() || tempIinvitation.lkpInvitationSource.Code == InvitationSourceTypes.CLIENTADMIN.GetStringValue())
        //        {
        //            Guid inviteeUserID = _dbNavigation.OrganizationUsers.Where(cond => cond.OrganizationUserID == orgUserID).Select(x => x.UserID).FirstOrDefault();
        //            if (!inviteeUserID.IsNullOrEmpty())
        //            {
        //                //getting object of AgencyUser basis on EmailID 
        //                AgencyUser agencyUser = _dbNavigation.AgencyUsers.Where(cond => cond.AGU_Email == tempIinvitation.PSI_InviteeEmail && !cond.AGU_IsDeleted).FirstOrDefault();
        //                if (agencyUser.IsNotNull())
        //                {
        //                    agencyUser.AGU_UserID = inviteeUserID;
        //                }
        //            }
        //        }

        //        if (_dbNavigation.SaveChanges() > 0)
        //        {
        //            return true;
        //        }
        //        return false;
        //    }
        //    return false;
        //}

        ///// <summary>
        ///// Save the New Invitation and return the ID of the invitation generated.
        ///// </summary>
        ///// <param name="invitationDetails"></param>
        ///// <returns>Tuple with InvitationID & its related Token</returns>
        //Tuple<Int32, Guid> ISecurityRepository.SaveProfileSharingInvitation(InvitationDetailsContract invitationDetails, Int32 generatedInvitationGroupID)
        //{
        //    var _invitationGroup = new ProfileSharingInvitationGroup();

        //    if (generatedInvitationGroupID == 0) //invitation sent by applicant
        //    {
        //        _invitationGroup.PSIG_AgencyID = invitationDetails.AgencyId.IsNotNull() ? invitationDetails.AgencyId : (Int32?)null;
        //        _invitationGroup.PSIG_InvitationInitiatedByID = invitationDetails.CurrentUserId;
        //        _invitationGroup.PSIG_TenantID = invitationDetails.TenantID;
        //        _invitationGroup.PSIG_IsDeleted = false;
        //        _invitationGroup.PSIG_CreatedByID = invitationDetails.CurrentUserId;
        //        _invitationGroup.PSIG_CreatedOn = invitationDetails.CurrentDateTime;
        //    }

        //    var _invitation = new ProfileSharingInvitation();
        //    var _token = Guid.NewGuid();

        //    if (generatedInvitationGroupID == 0)//invitation sent by applicant
        //    {
        //        _invitation.ProfileSharingInvitationGroup = _invitationGroup;
        //    }
        //    else//invitation sent by admin/client admin
        //    {
        //        _invitation.PSI_ProfileSharingInvitationGroupID = generatedInvitationGroupID;
        //    }
        //    _invitation.PSI_InviteeName = invitationDetails.Name;
        //    _invitation.PSI_InviteeEmail = invitationDetails.EmailAddress;
        //    _invitation.PSI_InviteePhone = invitationDetails.Phone;
        //    _invitation.PSI_InviteeAgency = invitationDetails.Agency;
        //    _invitation.PSI_InvitationMessage = invitationDetails.CustomMessage;
        //    _invitation.PSI_ApplicantOrgUserID = invitationDetails.ApplicantId;
        //    _invitation.PSI_InviteeOrgUserID = invitationDetails.InviteeOrgUserId;
        //    _invitation.PSI_ExpirationTypeID = invitationDetails.ExpirationTypeId;
        //    _invitation.PSI_InvitationSourceID = invitationDetails.InvitationSourceId;
        //    _invitation.PSI_InvitationStatusID = invitationDetails.InvitationStatusId;
        //    _invitation.PSI_MaxViews = invitationDetails.MaxViews;
        //    _invitation.PSI_ExpirationDate = invitationDetails.ExpirationDate.IsNotNull() ? invitationDetails.ExpirationDate : (DateTime?)null;
        //    _invitation.PSI_InvitationDate = invitationDetails.CurrentDateTime;
        //    _invitation.PSI_CreatedById = invitationDetails.CurrentUserId;
        //    _invitation.PSI_CreatedOn = invitationDetails.CurrentDateTime;
        //    _invitation.PSI_IsDeleted = false;
        //    _invitation.PSI_Token = _token;
        //    _invitation.PSI_TenantID = invitationDetails.TenantID;
        //    _invitation.PSI_PreviousInvitationID = (invitationDetails.PreviousPSIId.IsNotNull() && invitationDetails.PreviousPSIId > AppConsts.NONE)
        //        ? invitationDetails.PreviousPSIId
        //        : (Int32?)null;

        //    _invitation.PSI_InitiatedByID = invitationDetails.CurrentUserId;
        //    //invitationDetails.InitiatedById.IsNotNull() && invitationDetails.InitiatedById > AppConsts.NONE
        //    //? invitationDetails.InitiatedById
        //    //: (Int32?)null;

        //    _invitation.PSI_AgencyUserID = invitationDetails.AgencyUserId.IsNotNull() ? invitationDetails.AgencyUserId : (Int32?)null;

        //    _dbNavigation.ProfileSharingInvitations.AddObject(_invitation);

        //    foreach (var amdId in invitationDetails.SharedApplicantMetaDataIds)
        //    {
        //        var _applicantMetaData = new ApplicantSharedInvitationMetaData();
        //        _applicantMetaData.ProfileSharingInvitation = _invitation;
        //        _applicantMetaData.SIMD_ApplicantInvitationMetaDataID = amdId;
        //        _applicantMetaData.SIMD_CreatedByID = invitationDetails.CurrentUserId;
        //        _applicantMetaData.SIMD_CreatedOn = invitationDetails.CurrentDateTime;
        //        _applicantMetaData.SIMD_IsDeleted = false;
        //        _dbNavigation.ApplicantSharedInvitationMetaDatas.AddObject(_applicantMetaData);
        //    }

        //    _dbNavigation.SaveChanges();

        //    var _info = new Tuple<Int32, Guid>(_invitation.PSI_ID, _token);
        //    return _info;
        //}


        ///// <summary>
        ///// Save the Bulk Invitations sent by admin/client admin
        ///// </summary>
        ///// <param name="lstInvitationDetails"></param>
        ///// <param name="invitationGroup"></param>
        ///// <param name="generatedInvitationGroupID"></param>
        ///// <returns></returns>
        //List<ProfileSharingInvitation> ISecurityRepository.SaveAdminInvitations(List<InvitationDetailsContract> lstInvitationDetails, ProfileSharingInvitationGroup invitationGroup)
        //{
        //    var _lstInvitations = new List<ProfileSharingInvitation>();
        //    _dbNavigation.ProfileSharingInvitationGroups.AddObject(invitationGroup);

        //    foreach (var invitation in lstInvitationDetails)
        //    {
        //        var _invitation = new ProfileSharingInvitation();
        //        var _token = Guid.NewGuid();


        //        _invitation.ProfileSharingInvitationGroup = invitationGroup;

        //        _invitation.PSI_InviteeName = invitation.Name;
        //        _invitation.PSI_InviteeEmail = invitation.EmailAddress;
        //        _invitation.PSI_InviteePhone = invitation.Phone;
        //        _invitation.PSI_InviteeAgency = invitation.Agency;
        //        _invitation.PSI_InvitationMessage = invitation.CustomMessage;
        //        _invitation.PSI_ApplicantOrgUserID = invitation.ApplicantId;
        //        _invitation.PSI_InviteeOrgUserID = invitation.InviteeOrgUserId;
        //        _invitation.PSI_ExpirationTypeID = invitation.ExpirationTypeId;
        //        _invitation.PSI_InvitationSourceID = invitation.InvitationSourceId;
        //        _invitation.PSI_InvitationStatusID = invitation.InvitationStatusId;
        //        _invitation.PSI_MaxViews = invitation.MaxViews;
        //        _invitation.PSI_ExpirationDate = invitation.ExpirationDate.IsNotNull() ? invitation.ExpirationDate : (DateTime?)null;
        //        _invitation.PSI_InvitationDate = invitation.CurrentDateTime;
        //        _invitation.PSI_CreatedById = invitation.CurrentUserId;
        //        _invitation.PSI_CreatedOn = invitation.CurrentDateTime;
        //        _invitation.PSI_IsDeleted = false;
        //        _invitation.PSI_Token = _token;
        //        _invitation.PSI_TenantID = invitation.TenantID;
        //        _invitation.PSI_PreviousInvitationID = (invitation.PreviousPSIId.IsNotNull() && invitation.PreviousPSIId > AppConsts.NONE)
        //            ? invitation.PreviousPSIId
        //            : (Int32?)null;

        //        _invitation.InvitationIdentifier = invitation.InvitationIdentifier;
        //        _invitation.PSI_InitiatedByID = invitation.CurrentUserId;

        //        _invitation.PSI_AgencyUserID = invitation.AgencyUserId.IsNotNull() ? invitation.AgencyUserId : (Int32?)null;

        //        _dbNavigation.ProfileSharingInvitations.AddObject(_invitation);

        //        foreach (var amdId in invitation.SharedApplicantMetaDataIds)
        //        {
        //            var _applicantMetaData = new ApplicantSharedInvitationMetaData();
        //            _applicantMetaData.ProfileSharingInvitation = _invitation;
        //            _applicantMetaData.SIMD_ApplicantInvitationMetaDataID = amdId;
        //            _applicantMetaData.SIMD_CreatedByID = invitation.CurrentUserId;
        //            _applicantMetaData.SIMD_CreatedOn = invitation.CurrentDateTime;
        //            _applicantMetaData.SIMD_IsDeleted = false;
        //            _dbNavigation.ApplicantSharedInvitationMetaDatas.AddObject(_applicantMetaData);
        //        }
        //        _lstInvitations.Add(_invitation);
        //    }
        //    _dbNavigation.SaveChanges();
        //    return _lstInvitations;
        //}




        ///// <summary>
        ///// Gets the master details for the selected Invitation
        ///// </summary>
        ///// <param name="invitationId"></param>
        ///// <returns></returns>
        //ProfileSharingInvitation ISecurityRepository.GetInvitationDetails(Int32 invitationId)
        //{
        //    return _dbNavigation.ProfileSharingInvitations.Include("lkpInvitationExpirationType")
        //         .Where(psi => psi.PSI_ID == invitationId && psi.PSI_IsDeleted == false).First();
        //}

        ///// <summary>
        ///// Update the Status of the Invitation
        ///// </summary>
        ///// <param name="statusId"></param>
        ///// <param name="invitationId"></param>
        ///// <param name="currentUserId"></param>
        //void ISecurityRepository.UpdateInvitationStatus(Int32 statusId, Int32 invitationId, Int32 currentUserId)
        //{
        //    var _invitationData = _dbNavigation.ProfileSharingInvitations.Where(psi => psi.PSI_ID == invitationId && psi.PSI_IsDeleted == false).First();
        //    _invitationData.PSI_InvitationStatusID = statusId;
        //    _invitationData.PSI_ModifiedOn = DateTime.Now;
        //    _invitationData.PSI_ModifiedById = currentUserId;

        //    _dbNavigation.SaveChanges();
        //}


        ///// <summary>
        ///// Update the Status of the Invitation
        ///// </summary>
        ///// <param name="statusId"></param>
        ///// <param name="invitationId"></param>
        ///// <param name="currentUserId"></param>
        //void ISecurityRepository.UpdateBulkInvitationStatus(Int32 statusId, List<Int32> invitationId, Int32 currentUserId)
        //{
        //    var _lstInvitations = _dbNavigation.ProfileSharingInvitations.Where(psi => invitationId.Contains(psi.PSI_ID) && psi.PSI_IsDeleted == false).ToList();

        //    foreach (var _invitationData in _lstInvitations)
        //    {
        //        _invitationData.PSI_InvitationStatusID = statusId;
        //        _invitationData.PSI_ModifiedOn = DateTime.Now;
        //        _invitationData.PSI_ModifiedById = currentUserId;
        //    }
        //    _dbNavigation.SaveChanges();
        //}

        ///// <summary>
        ///// To get invitation data
        ///// </summary>
        ///// <param name="searchContract"></param>
        ///// <param name="gridCustomPaging"></param>
        ///// <returns></returns>
        //public DataTable GetInvitationData(InvitationSearchContract searchContract, CustomPagingArgsContract gridCustomPaging)
        //{
        //    EntityConnection connection = _dbNavigation.Connection as EntityConnection;
        //    using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
        //    {

        //        SqlCommand command = new SqlCommand("usp_GetInvitationSearchData", con);
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.Parameters.AddWithValue("@xmldata", searchContract.XML);
        //        command.Parameters.AddWithValue("@filteringSortingData", gridCustomPaging.XML);
        //        SqlDataAdapter da = new SqlDataAdapter();
        //        da.SelectCommand = command;
        //        DataSet ds = new DataSet();
        //        da.Fill(ds);

        //        if (ds.Tables.Count > 0)
        //        {
        //            return ds.Tables[0];
        //        }
        //        return new DataTable();
        //    }
        //}




        ///////// <summary>
        ///////// Get the OrganizationUserID of the user to whom invitation is being sent or else return 0.
        ///////// </summary>
        ///////// <param name="emailAddress"></param>
        ///////// <returns></returns>
        //////List<OrganizationUser> ISecurityRepository.GetSharedUserOrgId(List<String> emailAddress)
        //////{
        //////    var _orgUserId = AppConsts.NONE;

        //////    var _existingUser = _dbNavigation.aspnet_Membership.Where(anm => anm.Email.ToLower() == emailAddress.ToLower()).FirstOrDefault();
        //////    if (_existingUser.IsNotNull())
        //////    {
        //////        _orgUserId = _dbNavigation.OrganizationUsers.Where(ou => ou.UserID == _existingUser.UserId).First().OrganizationUserID;
        //////    }
        //////    return _orgUserId;
        //////}

        ///// <summary>
        ///// Get All Agencies for an institution
        ///// </summary>
        ///// <param name="InstitutionID"></param>
        ///// <returns></returns>
        //List<Agency> ISecurityRepository.GetAllAgency(Int32 institutionID)
        //{
        //    List<Agency> lstAgency = _dbNavigation.AgencyInstitutions.Where(cond => cond.AGI_TenantID == institutionID && !cond.AGI_IsDeleted).Select(x => x.Agency).ToList();
        //    if (lstAgency.IsNotNull())
        //        return lstAgency;
        //    return new List<Agency>();
        //}

        ///// <summary>
        ///// Method to Get Agency User Data by Agency ID and InstitutionID
        ///// </summary>
        ///// <param name="p"></param>
        ///// <returns></returns>
        //List<usp_GetAgencyUserData_Result> ISecurityRepository.GetAgencyUserData(Int32 institutionID, Int32 agencyID)
        //{
        //    List<usp_GetAgencyUserData_Result> agencyUserData = _dbNavigation.GetAgencyUserData(institutionID, agencyID).ToList();
        //    if (agencyUserData.IsNotNull())
        //    {
        //        return agencyUserData;
        //    }
        //    return new List<usp_GetAgencyUserData_Result>();

        //    //EntityConnection connection = _dbNavigation.Connection as EntityConnection;
        //    //using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
        //    //{

        //    //    SqlCommand command = new SqlCommand("usp_GetAgencyUserData", con);
        //    //    command.CommandType = CommandType.StoredProcedure;
        //    //    command.Parameters.AddWithValue("@TenantID", institutionID);
        //    //    command.Parameters.AddWithValue("@AgencyID", agencyID);
        //    //    SqlDataAdapter adp = new SqlDataAdapter();
        //    //    adp.SelectCommand = command;
        //    //    DataSet ds = new DataSet();
        //    //    adp.Fill(ds);
        //    //    if (ds.Tables.Count > 0)
        //    //    {
        //    //        return ds.Tables[0];
        //    //    }
        //    //}
        //    //return new DataTable();
        //    //return new List<AgencyUser>();
        //}

        ///// <summary>
        ///// Get Invitations based upon PSI_InviteeOrgUserID
        ///// </summary>
        ///// <param name="inviteeOrgUserID"></param>
        ///// <returns></returns>
        //IEnumerable<ProfileSharingInvitation> ISecurityRepository.GetInvitationsByInviteeOrgUserID(Int32 inviteeOrgUserID)
        //{
        //    return _dbNavigation.ProfileSharingInvitations.Where(psi => psi.PSI_InviteeOrgUserID == inviteeOrgUserID && psi.PSI_IsDeleted == false);
        //}

        ///// Update the Views remaining and last viewed of the Invitation
        ///// </summary>
        ///// <param name="statusId"></param>
        ///// <param name="invitationId"></param>
        ///// <param name="currentUserId"></param>
        //Boolean ISecurityRepository.UpdateInvitationViewsRemaining(Int32 invitationId, Int32 currentUserId, Int32 expiredInvitationTypeId)
        //{
        //    var _invitationData = _dbNavigation.ProfileSharingInvitations.Where(psi => psi.PSI_ID == invitationId && psi.PSI_IsDeleted == false).FirstOrDefault();
        //    if (_invitationData.IsNotNull() && _invitationData.PSI_MaxViews > _invitationData.PSI_InviteeViewCount)
        //    {
        //        _invitationData.PSI_InviteeLastViewed = DateTime.Now;
        //        _invitationData.PSI_InviteeViewCount = _invitationData.PSI_InviteeViewCount + AppConsts.ONE;
        //        if (_invitationData.PSI_MaxViews == _invitationData.PSI_InviteeViewCount)
        //            _invitationData.PSI_InvitationStatusID = expiredInvitationTypeId;
        //        _invitationData.PSI_ModifiedOn = DateTime.Now;
        //        _invitationData.PSI_ModifiedById = currentUserId;
        //        if (_dbNavigation.SaveChanges() > AppConsts.NONE)
        //            return true;
        //    }
        //    return false;
        //}

        ///// Update Notes of the Invitation
        ///// </summary>
        ///// <param name="statusId"></param>
        ///// <param name="invitationId"></param>
        ///// <param name="currentUserId"></param>
        //Boolean ISecurityRepository.UpdateInvitationNotes(Int32 invitationId, Int32 currentUserId, String notes)
        //{
        //    var _invitationData = _dbNavigation.ProfileSharingInvitations.Where(psi => psi.PSI_ID == invitationId && psi.PSI_IsDeleted == false).FirstOrDefault();
        //    if (_invitationData.IsNotNull())
        //    {
        //        _invitationData.PSI_InviteeNotes = notes;
        //        _invitationData.PSI_ModifiedOn = DateTime.Now;
        //        _invitationData.PSI_ModifiedById = currentUserId;
        //        if (_dbNavigation.SaveChanges() > AppConsts.NONE)
        //            return true;
        //    }
        //    return false;
        //}

        ///// <summary>
        ///// Method to genarate New Invitation Group
        ///// </summary>
        ///// <param name="agencyID"></param>
        ///// <param name="initiatedByID"></param>
        ///// <returns></returns>
        //Int32 ISecurityRepository.GenarateNewInvitationGroup(ProfileSharingInvitationGroup invitationGroupObj)
        //{
        //    //ProfileSharingInvitationGroup invitationGroup = new ProfileSharingInvitationGroup();
        //    //invitationGroup.PSIG_AgencyID = agencyID;
        //    //invitationGroup.PSIG_InvitationInitiatedByID = initiatedByID;
        //    //invitationGroup.PSIG_IsDeleted = false;
        //    //invitationGroup.PSIG_CreatedByID = initiatedByID;
        //    //invitationGroup.PSIG_CreatedOn = DateTime.Now;
        //    _dbNavigation.AddToProfileSharingInvitationGroups(invitationGroupObj);
        //    if (_dbNavigation.SaveChanges() > 0)
        //    {
        //        return invitationGroupObj.PSIG_ID;
        //    }
        //    return 0;
        //}

        //DataTable ISecurityRepository.GetAttestationDocumentData(String clientInvitationIDs)
        //{
        //    EntityConnection connection = _dbNavigation.Connection as EntityConnection;
        //    using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
        //    {

        //        SqlCommand command = new SqlCommand("usp_GetAttestationDocuments", con);
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.Parameters.AddWithValue("@InvitationIDs", clientInvitationIDs);
        //        SqlDataAdapter da = new SqlDataAdapter();
        //        da.SelectCommand = command;
        //        DataSet ds = new DataSet();
        //        da.Fill(ds);

        //        if (ds.Tables.Count > 0)
        //        {
        //            return ds.Tables[0];
        //        }
        //        return new DataTable();
        //    }
        //}
        //#region Profile Sharing Attestion Document
        //InvitationDocument ISecurityRepository.GetInvitationDocuments(Int32 invitationId)
        //{
        //    InvitationDocumentMapping invDocMappObj = _dbNavigation.InvitationDocumentMappings.Where(cond => cond.IDM_ProfileSharingInvitationID == invitationId
        //                                                             && cond.InvitationDocument.IND_IsDeleted == false && cond.IDM_IsDeleted == false).FirstOrDefault();
        //    if (invDocMappObj.IsNotNull())
        //    {
        //        return invDocMappObj.InvitationDocument;
        //    }
        //    return null;
        //}
        //#endregion

        #endregion

        #region UAT-1176/UAT-1178 - Employment Disclosure and User Attestation Disclosure

        /// <summary>
        /// Method to Get Disclosure Document by System Document ID 
        /// </summary>
        /// <param name="aWSUseS3"></param>
        public Entity.SystemDocument GetSystemDocumentByDocTypeID(Int32 docTypeID)
        {
            SystemDocument systemDoc = _dbNavigation.SystemDocuments.Where(cond => cond.SD_DocType_ID == docTypeID && !cond.IsDeleted).FirstOrDefault();
            if (systemDoc.IsNotNull())
                return systemDoc;
            return new SystemDocument();
        }

        /// <summary>
        /// Method to save Employment Disclosure Details
        /// </summary>
        /// <param name="p"></param>
        public Boolean SaveEDDetails(Int32 organizationUserID)
        {
            OrganizationUser organizationUser = _dbNavigation.OrganizationUsers
                                                    .Where(cond => cond.OrganizationUserID == organizationUserID && !cond.IsDeleted)
                                                    .FirstOrDefault();

            EmploymentDisclosureDetail EDD = new EmploymentDisclosureDetail();
            EDD.EDD_OrganizationUserID = organizationUserID;
            EDD.EDD_EntityName = organizationUser.Organization.Tenant.TenantName;
            EDD.EDD_FirstName = organizationUser.FirstName;
            EDD.EDD_LastName = organizationUser.LastName;
            EDD.EDD_CreatedByID = organizationUserID;
            EDD.EDD_CreatedOn = DateTime.Now;
            EDD.EDD_IsDeleted = false;

            _dbNavigation.AddToEmploymentDisclosureDetails(EDD);

            if (_dbNavigation.SaveChanges() > 0)
                return true;
            return false;
        }
        #endregion

        #region UAT-1086 WB: creation of video tutorial widget for admin (client and ADB) dashboard

        IEnumerable<ApplicationVideo> ISecurityRepository.GetApplicationVideos()
        {
            return _dbNavigation.ApplicationVideos.Include("lkpVideoType").Where(cond => cond.APV_IsDeleted == false);
        }

        ApplicationVideo ISecurityRepository.GetApplicationVideo(Int32 applicationVideoID)
        {
            return _dbNavigation.ApplicationVideos.Where(cond => cond.APV_IsDeleted == false && cond.APV_ID == applicationVideoID).FirstOrDefault();
        }

        #endregion

        #region UAT-1178 USER ATTESTATION DISCLOSURE FORM
        /// <summary>
        /// Method to Check whether Client admin has any Bkg Feature or not
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public Boolean CheckForClientRoleFeatures(Guid userID, List<short> lstbusinessChannelTypeID)
        {
            return _dbNavigation.vw_UserAssignedBlocks.Where(cond => cond.UserId == userID).Any(x => lstbusinessChannelTypeID.Contains(x.BusinessChannelTypeID.Value));
        }

        ///// <summary>
        ///// Method to cehck whether Shared User recieved any Bkg Order invitation
        ///// </summary>
        ///// <returns></returns>
        //public Boolean CheckForBkgInvitation(Int32 orgUserID)
        //{
        //    EntityConnection connection = _dbNavigation.Connection as EntityConnection;
        //    using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
        //    {

        //        SqlCommand command = new SqlCommand("usp_CheckSharedUserBkgOrderInvitation", con);
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.Parameters.AddWithValue("@OrgUserID", orgUserID);
        //        command.Parameters.Add("@resultValue", SqlDbType.Bit);
        //        command.Parameters["@resultValue"].Direction = ParameterDirection.Output;
        //        con.Open();
        //        command.ExecuteNonQuery();
        //        con.Close();
        //        return (Boolean)command.Parameters["@resultValue"].Value;
        //    }
        //}

        public UserAttestationDetail SaveUpdateUserAttestationDocument(UserAttestationDetail userAttestationDetails, Boolean isUpdateMode)
        {
            if (!isUpdateMode)
            {
                _dbNavigation.UserAttestationDetails.AddObject(userAttestationDetails);
            }
            else
            {
                UserAttestationDetail userAttestationNew = _dbNavigation.UserAttestationDetails.Where(x => x.UAD_ID == userAttestationDetails.UAD_ID && !x.UAD_IsDeleted).FirstOrDefault();
                userAttestationNew.UAD_Description = userAttestationDetails.UAD_Description;
                userAttestationNew.UAD_DocumentPath = userAttestationDetails.UAD_DocumentPath;
                userAttestationNew.UAD_ModifiedByID = userAttestationDetails.UAD_ModifiedByID;
                userAttestationNew.UAD_ModifiedOn = userAttestationDetails.UAD_ModifiedOn;
                userAttestationNew.UAD_Size = userAttestationDetails.UAD_Size;
                userAttestationNew.UAD_IsActive = userAttestationDetails.UAD_IsActive;
            }
            if (_dbNavigation.SaveChanges() > 0)
            {
                return userAttestationDetails;
            }
            return new UserAttestationDetail();
        }

        public UserAttestationDetail GetPartiallyFilledUserAttestationDocument(Int32 userAttestationDocumentID)
        {
            UserAttestationDetail userAttestationDetails = _dbNavigation.UserAttestationDetails.Where(cond => cond.UAD_ID == userAttestationDocumentID && !cond.UAD_IsDeleted).FirstOrDefault();
            if (!userAttestationDetails.IsNullOrEmpty())
                return userAttestationDetails;
            return new UserAttestationDetail();
        }

        public Boolean IsAttestationDocumentAlreadySubmitted(Int32 orgUserID)
        {
            return _dbNavigation.UserAttestationDetails.Any(cond => cond.UAD_OrganizationUserID == orgUserID
                                                                 && cond.UAD_IsActive
                                                                 && !cond.UAD_IsDeleted);
        }

        #endregion

        public List<GetEmploymentDocumentUserInfo_Result> GetEmploymentDisclosureUserInfo(int orgUserID)
        {
            return _dbNavigation.GetEmploymentDocumentUserInfo(orgUserID).ToList();
        }

        /// <summary>
        /// Get the Mapped Users for the 'Manage Users', for different scenarios
        /// </summary>
        /// <param name="currentUserId"></param>
        /// <param name="tenantProductId"></param>
        /// <param name="isAdmin"></param>
        /// <param name="organizationID"></param>
        /// <param name="isApplicantCheckRequred"></param>
        /// <param name="isParentOrgCheckRequired"></param>
        /// <param name="roleId"></param>
        /// <param name="sortingAndFilteringData"></param>
        /// <returns></returns>
        public DataTable GetMappedOrganizationUsers(Int32? currentUserId, Int32? tenantProductId, Boolean? isAdmin, Int32? organizationID,
            Boolean? isApplicantCheckRequred, Boolean? isParentOrgCheckRequired, String roleId, CustomPagingArgsContract sortingAndFilteringData)
        {
            EntityConnection connection = _dbNavigation.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetMappedOrganizationUsers", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CurrentUserId", currentUserId);
                command.Parameters.AddWithValue("@TenantProductId", tenantProductId);
                command.Parameters.AddWithValue("@IsAdmin", isAdmin);
                command.Parameters.AddWithValue("@OrganizationID", organizationID);
                command.Parameters.AddWithValue("@IsApplicantCheckRequred", isApplicantCheckRequred);
                command.Parameters.AddWithValue("@IsParentOrgCheckRequired", isParentOrgCheckRequired);
                command.Parameters.AddWithValue("@RoleID", roleId);
                command.Parameters.AddWithValue("@SortingFilteringData", sortingAndFilteringData.CreateXml());
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);

                sortingAndFilteringData.CurrentPageIndex = Convert.ToInt32(Convert.ToString(ds.Tables[0].Rows[0]["CurrentPageIndex"]));
                sortingAndFilteringData.VirtualPageCount = Convert.ToInt32(Convert.ToString(ds.Tables[0].Rows[0]["VirtualCount"]));
                return ds.Tables[1];
            }
        }

        #region UAT 1230:As an admin, I should be able to invite a person (or group of people) to create an applicant account
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountCreationContactList"></param>
        /// <returns></returns>
        Boolean ISecurityRepository.SaveAccountCreationContactList(List<AccountCreationContact> accountCreationContactList)
        {
            foreach (AccountCreationContact accountCreationContact in accountCreationContactList)
            {
                _dbNavigation.AccountCreationContacts.AddObject(accountCreationContact);
            }
            if (_dbNavigation.SaveChanges() > AppConsts.NONE)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region UAT-1218: Any User should be able to be 1 or more of the following: Applicant, Client admin, Agency User, Instructor/Preceprtor

        /// <summary>
        /// Method to check whether the User invited is already existing in DB, If So then return its Organization UserID
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        Int32 ISecurityRepository.IsExistingUserInvited(String emailAddress)
        {
            var memberShip = _dbNavigation.aspnet_Membership.Where(amu => amu.Email.ToLower() == emailAddress.ToLower()).FirstOrDefault();
            if (!memberShip.IsNullOrEmpty())
            {
                var orgUserID = _dbNavigation.OrganizationUsers.Where(cond => cond.UserID == memberShip.UserId && !cond.IsDeleted)
                                                             .Select(x => x.OrganizationUserID)
                                                             .FirstOrDefault();
                return orgUserID;
            }
            return AppConsts.NONE;
        }

        /// <summary>
        /// Method to Get Organization User Type Mapping List on the basis of Organization UserID
        /// </summary>
        /// <param name="organizationUserID"></param>
        /// <returns></returns>
        List<OrganizationUserTypeMapping> ISecurityRepository.GetOrganizationUserTypeMapping(Guid userID)
        {
            var orgUserTypeMapping = _dbNavigation.OrganizationUserTypeMappings.Where(cond => cond.OrganizationUser.UserID == userID && cond.OTM_IsDeleted == false).ToList();
            if (!orgUserTypeMapping.IsNullOrEmpty())
            {
                return orgUserTypeMapping;
            }
            return new List<OrganizationUserTypeMapping>();
        }

        /// <summary>
        /// Method to Add entry in OrganizationUserTypeMappings
        /// </summary>
        /// <param name="orgUserID"></param>
        /// <param name="userTypeID"></param>
        void ISecurityRepository.AddOrganizationUserTypeMapping(Int32 orgUserID, Int32 userTypeID)
        {
            OrganizationUserTypeMapping orgUserTypeMapping = new OrganizationUserTypeMapping();
            orgUserTypeMapping.OTM_OrgUserID = orgUserID;
            orgUserTypeMapping.OTM_OrgUserTypeID = userTypeID;
            orgUserTypeMapping.OTM_CreatedByID = orgUserID;
            orgUserTypeMapping.OTM_CreatedOn = DateTime.Now;
            orgUserTypeMapping.OTM_IsDeleted = false;

            _dbNavigation.AddToOrganizationUserTypeMappings(orgUserTypeMapping);
            _dbNavigation.SaveChanges();
        }

        /// <summary>
        /// Finds if the sharedUser username is already exists in Security database
        /// </summary>
        /// <param name="userName">User Name</param>
        /// <param name="tenantId">Tenant Id</param>
        /// <returns>True if username exists</returns>
        Boolean ISecurityRepository.IsUsernameExistInSecuritytDB(String userName)
        {
            return _dbNavigation.OrganizationUsers.Any(x => x.aspnet_Users.UserName.ToLower().Equals(userName)
                && x.Organization.TenantID == AppConsts.SHARED_USER_TENANT_ID
                && x.IsDeleted == false
                && (x.IsSharedUser ?? false) == true);
        }

        /// <summary>
        /// Finds if the sharedUser username is already exists in Security database and get Organization User
        /// </summary>
        /// <param name="userName">User Name</param>
        /// <returns></returns>
        OrganizationUser ISecurityRepository.GetOrgUserIfUsernameExistInSecuritytDB(String userName)
        {
            return _dbNavigation.OrganizationUsers.Include(SysXEntityConstants.TABLE_ORGANIZATION)
              .Where(x => x.aspnet_Users.UserName.ToLower().Equals(userName)
              && x.Organization.TenantID == AppConsts.SHARED_USER_TENANT_ID
              && x.IsDeleted == false
              && (x.IsSharedUser ?? false) == true).FirstOrDefault();
        }

        #endregion

        #region UAT 1304 Instructor/Preceptor screens and functionality
        public Boolean UpdateClientContactOrganisationUser(INTSOF.ServiceDataContracts.Modules.Common.OrganizationUserContract organizationUserContract, String userID)
        {
            Guid membershipUserID = Guid.Parse(userID);
            List<OrganizationUser> organisatonUserLst = _dbNavigation.OrganizationUsers.Where(x => x.UserID == membershipUserID && x.IsActive && !x.IsDeleted).ToList();
            if (!organisatonUserLst.IsNullOrEmpty())
            {
                foreach (OrganizationUser orgUser in organisatonUserLst)
                {
                    orgUser.FirstName = organizationUserContract.FirstName;
                    orgUser.MiddleName = organizationUserContract.MiddleName;
                    orgUser.LastName = organizationUserContract.LastName;
                    orgUser.SSNL4 = organizationUserContract.SSN;
                    orgUser.ModifiedByID = organizationUserContract.OrganizationUserID;
                    orgUser.ModifiedOn = DateTime.Now;
                }

                if (_dbNavigation.SaveChanges() > AppConsts.NONE)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        public List<ExternalVendorFieldOption> GetExternalVendorFieldOptionByVendorID(int vendorID)
        {
            return _dbNavigation.ExternalVendorFieldOptions.Where(n => n.EVFO_ExternalVendorID == vendorID).ToList();
        }

        public List<Int32> GetOrganizationUserIdsByUserIds(List<Guid?> lstUserID)
        {
            return _dbNavigation.OrganizationUsers.Where(cond => lstUserID.Contains(cond.UserID)
                && !cond.IsDeleted
                && cond.IsActive
                && cond.IsSharedUser == true)
                .Select(col => col.OrganizationUserID).ToList();
        }

        String ISecurityRepository.GetFormattedString(Int32 orgUserID, Boolean isOrgUserProfileID)
        {
            return _dbNavigation.usp_GetFormattedString(orgUserID, isOrgUserProfileID).FirstOrDefault();
        }

        List<SystemEntityUserPermissionData> ISecurityRepository.GetSystemEntityUserPermissionData(Int32 organizationUserID)
        {
            EntityConnection connection = _dbNavigation.Connection as EntityConnection;
            List<SystemEntityUserPermissionData> systemEntityUserPermissionDataList = new List<SystemEntityUserPermissionData>();
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("usp_GetGranularPermissionForClientAdmin", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OrganizationUserID", organizationUserID);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        systemEntityUserPermissionDataList = ds.Tables[0].AsEnumerable().Select(col =>
                            new SystemEntityUserPermissionData
                            {
                                PermissionTypeID = Convert.ToInt32(col["PermissionTypeID"]),
                                PermissionTypeName = col["PermissionTypeName"].ToString(),
                                PermissionName = col["PermissionName"].ToString(),
                                HierarchyNodeId = col["HierarchyID"] == DBNull.Value ? 0: Convert.ToInt32(col["HierarchyID"]),
                                HierarchyNodeLabel = col["Hierarchy"] == DBNull.Value ? "" : col["Hierarchy"].ToString()

                            }).ToList();
                    }
                }
            }
            return systemEntityUserPermissionDataList;
        }

        #region Announcement

        /// <summary>
        /// Get Announcement Detail
        /// </summary>
        /// <returns></returns>
        List<AnnouncementContract> ISecurityRepository.GetAnnouncementDetail()
        {
            var announcementList = _dbNavigation.Announcements.Where(con => con.AN_IsDeleted == false).ToList();
            List<AnnouncementContract> lstAnnouncementContract = new List<AnnouncementContract>();

            if (!announcementList.IsNullOrEmpty())
            {
                announcementList.ForEach(x =>
                {
                    AnnouncementContract announcementContract = new AnnouncementContract();
                    announcementContract.AnnouncementID = x.AN_ID;
                    announcementContract.AnnouncementName = x.AN_AnnouncementName;
                    announcementContract.AnnouncementText = x.AN_AnnouncementText;

                    lstAnnouncementContract.Add(announcementContract);
                });
            }
            return lstAnnouncementContract;
        }

        /// <summary>
        /// Save Update Announcement
        /// </summary>
        /// <param name="announcementContract"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        Boolean ISecurityRepository.SaveUpdateAnnouncement(AnnouncementContract announcementContract, Int32 currentUserId)
        {
            //Update
            if (announcementContract.AnnouncementID > AppConsts.NONE)
            {
                var announcement = _dbNavigation.Announcements.FirstOrDefault(con => con.AN_ID == announcementContract.AnnouncementID && !con.AN_IsDeleted);
                if (announcement.IsNotNull())
                {
                    announcement.AN_AnnouncementName = announcementContract.AnnouncementName;
                    announcement.AN_AnnouncementText = announcementContract.AnnouncementText;
                    announcement.AN_ModifiedByID = currentUserId;
                    announcement.AN_ModifiedOn = DateTime.Now;

                    if (_dbNavigation.SaveChanges() > AppConsts.NONE)
                        return true;
                }
            }
            else //Insert
            {
                Announcement announcement = new Announcement();
                announcement.AN_AnnouncementName = announcementContract.AnnouncementName;
                announcement.AN_AnnouncementText = announcementContract.AnnouncementText;
                announcement.AN_IsDeleted = false;
                announcement.AN_CreatedByID = currentUserId;
                announcement.AN_CreatedOn = DateTime.Now;

                _dbNavigation.Announcements.AddObject(announcement);
                if (_dbNavigation.SaveChanges() > AppConsts.NONE)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Delete Announcement
        /// </summary>
        /// <param name="announcementID"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        Boolean ISecurityRepository.DeleteAnnouncement(Int32 announcementID, Int32 currentUserId)
        {
            var announcement = _dbNavigation.Announcements.FirstOrDefault(con => con.AN_ID == announcementID && !con.AN_IsDeleted);
            if (announcement.IsNotNull())
            {
                announcement.AN_IsDeleted = true;
                announcement.AN_ModifiedByID = currentUserId;
                announcement.AN_ModifiedOn = DateTime.Now;

                if (_dbNavigation.SaveChanges() > AppConsts.NONE)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Get Announcement Popup Detail
        /// </summary>
        /// <param name="announcementID"></param>
        /// <returns></returns>
        AnnouncementContract ISecurityRepository.GetAnnouncementPopupDetail(Int32 announcementID)
        {
            var announcement = _dbNavigation.Announcements.FirstOrDefault(con => con.AN_ID == announcementID && con.AN_IsDeleted == false);
            AnnouncementContract announcementContract = new AnnouncementContract();

            if (announcement.IsNotNull())
            {
                announcementContract.AnnouncementID = announcement.AN_ID;
                announcementContract.AnnouncementName = announcement.AN_AnnouncementName;
                announcementContract.AnnouncementText = announcement.AN_AnnouncementText;
            }
            return announcementContract;
        }

        /// <summary>
        /// Save Announcement Mapping
        /// </summary>
        /// <param name="announcementID"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        Boolean ISecurityRepository.SaveAnnouncementMapping(Int32 announcementID, Int32 currentUserId)
        {
            AnnouncementMapping announcementMapping = new AnnouncementMapping();
            announcementMapping.AM_AnnouncementID = announcementID;
            announcementMapping.AM_OrganizationUserID = currentUserId;
            announcementMapping.AM_IsDeleted = false;
            announcementMapping.AM_CreatedByID = currentUserId;
            announcementMapping.AM_CreatedOn = DateTime.Now;

            _dbNavigation.AnnouncementMappings.AddObject(announcementMapping);
            if (_dbNavigation.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }

        /// <summary>
        /// Get Announcements
        /// </summary>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        List<Int32> ISecurityRepository.GetAnnouncements(Int32 currentUserId)
        {
            EntityConnection connection = _dbNavigation.Connection as EntityConnection;
            List<Int32> announcementIDList = new List<Int32>();
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("usp_GetAnnouncements", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@LoggedInOrgUserID", currentUserId);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        announcementIDList = ds.Tables[0].AsEnumerable().Select(col =>
                                             Convert.ToInt32(col["AnnouncementID"])).ToList();
                    }
                }
            }
            return announcementIDList;
        }

        #endregion

        #region Bulletin

        /// <summary>
        /// Getting list of bulletin for binding grid using Stored Procedure(usp_GetBulletInData).
        /// </summary>
        /// <returns></returns>
        List<BulletinContract> ISecurityRepository.GetBulletin(string selectedInstitutionIds)
        {
            EntityConnection connection = _dbNavigation.Connection as EntityConnection;
            List<BulletinContract> lstBulletIn = new List<BulletinContract>();
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("usp_GetBulletInData", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SelectedInstitutionIds", selectedInstitutionIds);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        lstBulletIn = ds.Tables[0].AsEnumerable().Select(col =>
                            new BulletinContract
                            {
                                BulletinID = Convert.ToInt32(col["BulletInId"]),
                                BulletinTitle = Convert.ToString(col["BullentInTitle"]),
                                BulletinContent = Convert.ToString(col["BulletInContent"]),
                                InstitutionIds = Convert.ToString(col["InstitutionIds"]),
                                InstitutionName = Convert.ToString(col["InstitutionName"]),
                            }).ToList();
                    }
                }
            }
            return lstBulletIn;
        }

        /// <summary>
        /// Method for ADD and UPDATE Bulletin and and tenant mapping.
        /// </summary>
        /// <param name="bulletinContract"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public Int32 SaveUpdateBulletin(BulletinContract bulletinContract, Int32 currentUserId)
        {
            var _currentDateTime = DateTime.Now;
            var _bulletinId = AppConsts.NONE;

            if (bulletinContract.BulletinID > AppConsts.NONE) //UPDATE
            {
                var bulletin = _dbNavigation.Bulletins.FirstOrDefault(con => con.BU_ID == bulletinContract.BulletinID && !con.BU_IsDeleted);
                if (bulletin.IsNotNull())
                {
                    bulletin.BU_BulletinTitle = bulletinContract.BulletinTitle;
                    bulletin.BU_BulletinContent = bulletinContract.BulletinContent;
                    bulletin.BU_ModifiedByID = currentUserId;
                    bulletin.BU_ModifiedOn = _currentDateTime;

                    foreach (var btm in bulletin.BulletinTenantMappings)
                    {
                        btm.BTM_IsDeleted = true;
                        btm.BTM_ModifiedByID = currentUserId;
                        btm.BTM_ModifiedOn = _currentDateTime;
                    }

                    foreach (var tenantId in bulletinContract.LstSelectedTenantID)
                    {
                        BulletinTenantMapping _bulletinTenantMapping = new BulletinTenantMapping();
                        _bulletinTenantMapping.BTM_BulletinID = bulletin.BU_ID;
                        _bulletinTenantMapping.BTM_TenantID = tenantId;
                        _bulletinTenantMapping.BTM_CreatedByID = currentUserId;
                        _bulletinTenantMapping.BTM_CreatedOn = _currentDateTime;
                        bulletin.BulletinTenantMappings.Add(_bulletinTenantMapping);
                    }

                    _dbNavigation.SaveChanges();
                    _bulletinId = bulletin.BU_ID;
                }
            }
            else  //INSERT
            {
                Bulletin bulletin = new Bulletin();
                bulletin.BU_BulletinTitle = bulletinContract.BulletinTitle;
                bulletin.BU_BulletinContent = bulletinContract.BulletinContent;
                bulletin.BU_IsDeleted = false;
                bulletin.BU_CreatedByID = currentUserId;
                bulletin.BU_CreatedOn = _currentDateTime;
                bulletin.BU_IsCreatedByAdmin = bulletinContract.IsCreatedByADBAdmin;

                foreach (var tenantId in bulletinContract.LstSelectedTenantID)
                {
                    BulletinTenantMapping _bulletinTenantMapping = new BulletinTenantMapping();
                    _bulletinTenantMapping.BTM_BulletinID = bulletin.BU_ID;
                    _bulletinTenantMapping.BTM_TenantID = tenantId;
                    _bulletinTenantMapping.BTM_CreatedByID = currentUserId;
                    _bulletinTenantMapping.BTM_CreatedOn = _currentDateTime;
                    bulletin.BulletinTenantMappings.Add(_bulletinTenantMapping);
                }

                _dbNavigation.Bulletins.AddObject(bulletin);
                _dbNavigation.SaveChanges();

                _bulletinId = bulletin.BU_ID;
            }
            return _bulletinId;
        }

        /// <summary>
        /// Method for deleting bulletin and tenant mapping.
        /// </summary>
        /// <param name="BulletinID"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public bool DeleteBulletin(Int32 BulletinID, Int32 currentUserId)
        {
            var _currentDateTime = DateTime.Now;
            if (BulletinID > 0)
            {
                Bulletin bulletin = _dbNavigation.Bulletins.Where(ex => ex.BU_ID == BulletinID && !ex.BU_IsDeleted).FirstOrDefault();
                bulletin.BU_IsDeleted = true;
                bulletin.BU_ModifiedByID = currentUserId;
                bulletin.BU_ModifiedOn = _currentDateTime;

                foreach (var btm in bulletin.BulletinTenantMappings)
                {
                    btm.BTM_IsDeleted = true;
                    btm.BTM_ModifiedByID = currentUserId;
                    btm.BTM_ModifiedOn = _currentDateTime;
                }
                _dbNavigation.SaveChanges();
                return true;
            }
            return true;
        }

        #endregion

        #region Bulletins Popup

        /// <summary>
        /// Get Bulletin Popup detail data
        /// </summary>
        /// <param name="bulletinID"></param>
        /// <returns></returns>
        BulletinContract ISecurityRepository.GetBulletinPopupDetail(Int32 bulletinID)
        {
            var bulletin = _dbNavigation.Bulletins.FirstOrDefault(con => con.BU_ID == bulletinID && con.BU_IsDeleted == false);
            BulletinContract bulletinContract = new BulletinContract();

            if (bulletin.IsNotNull())
            {
                bulletinContract.BulletinID = bulletin.BU_ID;
                bulletinContract.BulletinTitle = bulletin.BU_BulletinTitle;
                bulletinContract.BulletinContent = bulletin.BU_BulletinContent;
            }
            return bulletinContract;
        }

        /// <summary>
        /// Save Bulletin Mapping
        /// </summary>
        /// <param name="bulletinID"></param>
        /// <param name="currentUserID"></param>
        /// <returns></returns>
        Boolean ISecurityRepository.SaveBulletinMapping(Int32 bulletinID, Int32 currentUserID)
        {
            BulletinUserMapping bulletinUserMapping = new BulletinUserMapping();
            bulletinUserMapping.BUM_BulletinID = bulletinID;
            bulletinUserMapping.BUM_OrganizationUserID = currentUserID;
            bulletinUserMapping.BUM_IsDeleted = false;
            bulletinUserMapping.BUM_CreatedByID = currentUserID;
            bulletinUserMapping.BUM_CreatedOn = DateTime.Now;

            _dbNavigation.BulletinUserMappings.AddObject(bulletinUserMapping);
            if (_dbNavigation.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }

        #endregion


        List<AgencyUserPermissionContract> ISecurityRepository.GetOrganizationUserListByUserIds(String userIDsXML)
        {
            EntityConnection connection = _dbNavigation.Connection as EntityConnection;
            List<AgencyUserPermissionContract> agencyUserPermissionDataList = new List<AgencyUserPermissionContract>();
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("usp_GetAgencyUserLockedStatusByUserIds", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@xmldata", userIDsXML);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        agencyUserPermissionDataList = ds.Tables[0].AsEnumerable().Select(col =>
                            new AgencyUserPermissionContract
                            {
                                UserID = Convert.ToString(col["UserID"]),
                                IsActive = Convert.ToBoolean(col["IsActive"]),
                                IsLocked = Convert.ToBoolean(col["IsLocked"]),
                                FirstName = col["FirstName"] == DBNull.Value ? String.Empty : Convert.ToString(col["FirstName"]),
                                LastName = col["LastName"] == DBNull.Value ? String.Empty : Convert.ToString(col["LastName"])
                            }).ToList();
                    }
                }
            }
            return agencyUserPermissionDataList;
        }

        List<ManageRoleContract> ISecurityRepository.GetRolesMappedWithUserType(String SelectedUserTypeIds)
        {
            EntityConnection connection = _dbNavigation.Connection as EntityConnection;
            List<ManageRoleContract> lstRoles = new List<ManageRoleContract>();
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("usp_GetRolesMappedWithUserType", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SelectedUserTypeIds", SelectedUserTypeIds);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        lstRoles = ds.Tables[0].AsEnumerable().Select(col =>
                            new ManageRoleContract
                            {
                                RoleDetailId = Convert.ToString(col["RoleDetailID"]),
                                Name = col["RoleName"].ToString(),
                                TenantName = col["TenantName"].ToString()
                            }).ToList();
                    }
                }
            }
            return lstRoles;
        }

        void ISecurityRepository.InsertBulkRoleFeatures(String featuresIds, String userTypeIds, String roleDetailIds, Int32 currentLoggedinUserId)
        {
            EntityConnection connection = _dbNavigation.Connection as EntityConnection;
            List<ManageRoleContract> lstRoles = new List<ManageRoleContract>();
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("usp_InsertBulkRoleFeatures", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FeaturesIds", featuresIds);
                cmd.Parameters.AddWithValue("@UserTypeIds", userTypeIds);
                cmd.Parameters.AddWithValue("@RoleDetailIds", roleDetailIds);
                cmd.Parameters.AddWithValue("@UserId", currentLoggedinUserId);
                con.Open();
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                con.Close();
            }
        }

        /// <summary>
        /// Method to Get Disclosure Document by System Document ID 
        /// </summary>
        /// <param name="aWSUseS3"></param>
        public List<Entity.SystemDocument> GetSystemDocumentListByDocTypeID(Int32 docTypeID)
        {
            return _dbNavigation.SystemDocuments.Where(cond => cond.SD_DocType_ID == docTypeID && !cond.IsDeleted).ToList();
        }
        /// <summary>
        /// if User get garduated then all document for that user are get deleted 
        /// </summary>
        /// <param name="tenantID"></param>
        /// <param name="organizationUserID"></param>
        /// <returns></returns>
        public Boolean DeleteDocsFromDataEntryQueueForGraduated(Int32 tenantID, Int32 organizationUserID, Int32 orgUsrId)
        {
            var listDataEntryQueue = _dbNavigation.FlatDataEntryQueues.Where(cond => cond.FDEQ_TenantID == tenantID
                                                                        && cond.FDEQ_ApplicantOrganizationUserID == organizationUserID && !cond.FDEQ_IsDeleted).ToList();
            foreach (var dataEntryQueue in listDataEntryQueue)
            {
                dataEntryQueue.FDEQ_IsDeleted = true;
                //dataEntryQueue.FDEQ_ModifiedByID = organizationUserID;
                dataEntryQueue.FDEQ_ModifiedByID = orgUsrId;
                dataEntryQueue.FDEQ_ModifiedOn = DateTime.Now;
            }

            if (_dbNavigation.SaveChanges() > 0)
                return true;
            return false;
        }
        #region Dummy Complio API
        /// <summary>
        /// method to get all the data of entity type.
        /// </summary>
        /// <returns></returns>
        public List<APIMetaData> GetAPIMetaDataList()
        {
            return _dbNavigation.APIMetaDatas.Where(cnd => !cnd.API_IsDeleted).ToList();
        }
        #endregion


        public Boolean UpdateStatusOfAppDocumentForDataEntry(List<Int32> lstUsers, Int32 currentloggedInUserId, Int16 docStatusId, Int32 tenantID)
        {
            List<FlatDataEntryQueue> lstDataEntryQueueToUpdate = _dbNavigation.FlatDataEntryQueues.
                                                                  Where(cnd => lstUsers.Contains(cnd.FDEQ_ApplicantOrganizationUserID)
                                                                        && cnd.FDEQ_TenantID == tenantID
                                                                        && cnd.FDEQ_IsDeleted == false
                                                                        && cnd.FDEQ_DataEntryDocumentStatusID != docStatusId
                                                                        ).ToList();
            lstDataEntryQueueToUpdate.ForEach(dataEntryData =>
            {
                dataEntryData.FDEQ_ModifiedByID = currentloggedInUserId;
                dataEntryData.FDEQ_ModifiedOn = DateTime.Now;
                dataEntryData.FDEQ_DataEntryDocumentStatusID = docStatusId;
            });

            if (_dbNavigation.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }

        #region Invoice Group

        /// <summary>
        /// Get Invoice Group Details
        /// </summary>
        /// <returns></returns>
        List<InvoiceGroupContract> ISecurityRepository.GetInvoiceGroupDetails()
        {
            EntityConnection connection = _dbNavigation.Connection as EntityConnection;
            List<InvoiceGroupContract> lstInvoiceGroup = new List<InvoiceGroupContract>();

            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("usp_GetInvoiceGroupDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@SelectedInstitutionIds", selectedInstitutionIds);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adp.Fill(ds);

                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        lstInvoiceGroup = ds.Tables[0].AsEnumerable().Select(col =>
                            new InvoiceGroupContract
                            {
                                InvoiceGroupID = Convert.ToInt32(col["InvoiceGroupId"]),
                                InvoiceGroupName = Convert.ToString(col["InvoiceGroupName"]),
                                InvoiceGroupDescription = Convert.ToString(col["InvoiceGroupDescription"]),
                                InstitutionIDs = Convert.ToString(col["InstitutionIds"]),
                                InstitutionNames = Convert.ToString(col["InstitutionNames"]),
                                InstitutionHierarchyIDs = Convert.ToString(col["DPMIds"]),
                                InstitutionHierarchyLabels = Convert.ToString(col["DPMLabels"]),
                                ReportColumnIDs = Convert.ToString(col["ReportColumnIds"]),
                                ReportColumnNames = Convert.ToString(col["ReportColumnNames"]),
                                InvoiceGroupReportColumnIDs = Convert.ToString(col["InvoiceGroupReportColumnIds"]),
                            }).ToList();
                    }
                }
            }
            return lstInvoiceGroup;
        }

        /// <summary>
        /// Save Update Invoice Group Detail
        /// </summary>
        /// <param name="currentLoggedInUserId"></param>
        /// <param name="invoiceGroupId"></param>
        /// <param name="invoiceGroupName"></param>
        /// <param name="invoiceGroupDesc"></param>
        /// <param name="selectedDPMIds"></param>
        /// <param name="lstSelectedReportColumnIds"></param>
        /// <returns></returns>
        public Boolean SaveUpdateInvoiceGroupInformation(Int32 currentLoggedInUserId, Int32 invoiceGroupId, String invoiceGroupName, String invoiceGroupDesc, List<String> selectedDPMIds, List<Int32> lstSelectedReportColumnIds)
        {
            InvoiceGroup invoiceGroup;

            //Update Group Information
            if (invoiceGroupId > 0)
            {
                invoiceGroup = _dbNavigation.InvoiceGroups.Where(cond => cond.IG_IsDeleted != true && cond.IG_ID == invoiceGroupId).SingleOrDefault();
                if (!invoiceGroup.IsNullOrEmpty())
                {
                    invoiceGroup.IG_Name = invoiceGroupName;
                    invoiceGroup.IG_Description = invoiceGroupDesc;
                    invoiceGroup.IG_ModifiedBy = currentLoggedInUserId;
                    invoiceGroup.IG_ModifiedOn = DateTime.Now;

                    //Deleting Existing Node Mappings
                    foreach (var item in invoiceGroup.InvoiceGroupNodeMappings)
                    {
                        item.INGM_IsDeleted = true;
                        item.INGM_ModifiedBy = currentLoggedInUserId;
                        item.INGM_ModifiedOn = DateTime.Now;
                    }

                    foreach (var rc in invoiceGroup.InvoiceGroupReportColumns)
                    {
                        rc.IGRC_IsDeleted = true;
                        rc.IGRC_ModifiedBy = currentLoggedInUserId;
                        rc.IGRC_ModifiedOn = DateTime.Now;
                    }
                }
            }
            else
            {
                //Adding new group
                invoiceGroup = new InvoiceGroup();
                invoiceGroup.IG_Name = invoiceGroupName;
                invoiceGroup.IG_Description = invoiceGroupDesc;
                invoiceGroup.IG_IsDeleted = false;
                invoiceGroup.IG_CreatedBy = currentLoggedInUserId;
                invoiceGroup.IG_CreatedOn = DateTime.Now;
            }

            //Adding Invoice Group Node Mapping
            if (!invoiceGroup.IsNullOrEmpty() && !selectedDPMIds.IsNullOrEmpty())
            {
                foreach (String item in selectedDPMIds)
                {
                    if (!String.IsNullOrEmpty(item))
                    {
                        Int32 tenantID = 0;
                        Int32 dpmId = 0;

                        Int32.TryParse(item.Split('_')[0], out tenantID);

                        if (item.Split('_').Length > 1)
                            Int32.TryParse(item.Split('_')[1], out dpmId);

                        if (tenantID > 0 && dpmId > 0)
                        {
                            InvoiceGroupNodeMapping invoiceGroupNodeMapping = new InvoiceGroupNodeMapping();
                            invoiceGroupNodeMapping.IGNM_TenantID = tenantID;
                            invoiceGroupNodeMapping.IGNM_DPMID = dpmId;
                            invoiceGroupNodeMapping.INGM_CreatedBy = currentLoggedInUserId;
                            invoiceGroupNodeMapping.INGM_CreatedOn = DateTime.Now;
                            invoiceGroupNodeMapping.INGM_IsDeleted = false;
                            invoiceGroup.InvoiceGroupNodeMappings.Add(invoiceGroupNodeMapping);
                        }
                    }
                }
            }

            //Adding Report Columns Selection
            if (!invoiceGroup.IsNullOrEmpty() && !lstSelectedReportColumnIds.IsNullOrEmpty())
            {
                foreach (Int32 rpId in lstSelectedReportColumnIds)
                {
                    InvoiceGroupReportColumn invoiceGroupReportColumn = new InvoiceGroupReportColumn();
                    invoiceGroupReportColumn.IGRC_ReportColumnID = rpId;
                    invoiceGroupReportColumn.IGRC_IsDeleted = false;
                    invoiceGroupReportColumn.IGRC_CreatedBy = currentLoggedInUserId;
                    invoiceGroupReportColumn.IGRC_CreatedOn = DateTime.Now;
                    invoiceGroup.InvoiceGroupReportColumns.Add(invoiceGroupReportColumn);
                }
            }
            if (!invoiceGroup.IsNullOrEmpty())
            {
                if (invoiceGroupId <= 0)
                    _dbNavigation.InvoiceGroups.AddObject(invoiceGroup);

                if (_dbNavigation.SaveChanges() > 0)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        /// <summary>
        /// Delete Existing Group Invoice Detail
        /// </summary>
        /// <param name="currentLoggedInUserId"></param>
        /// <param name="invoiceGroupId"></param>
        /// <returns></returns>
        public Boolean DeleteInvoiceGroupInformation(Int32 currentLoggedInUserId, Int32 invoiceGroupId)
        {
            InvoiceGroup invoiceGroup = _dbNavigation.InvoiceGroups.Where(cond => cond.IG_IsDeleted != true && cond.IG_ID == invoiceGroupId).SingleOrDefault();

            if (!invoiceGroup.IsNullOrEmpty())
            {
                invoiceGroup.IG_IsDeleted = true;
                invoiceGroup.IG_ModifiedBy = currentLoggedInUserId;
                invoiceGroup.IG_ModifiedOn = DateTime.Now;

                //Deleting Existing Node Mappings
                foreach (var item in invoiceGroup.InvoiceGroupNodeMappings)
                {
                    item.INGM_IsDeleted = true;
                    item.INGM_ModifiedBy = currentLoggedInUserId;
                    item.INGM_ModifiedOn = DateTime.Now;
                }

                //Deleting Reort Columns Mappings
                foreach (var rc in invoiceGroup.InvoiceGroupReportColumns)
                {
                    rc.IGRC_IsDeleted = true;
                    rc.IGRC_ModifiedBy = currentLoggedInUserId;
                    rc.IGRC_ModifiedOn = DateTime.Now;
                }

                if (_dbNavigation.SaveChanges() > 0)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        #endregion

        public ReconciliationQueueConfiguration GetCurrentReconciliationAssignmentConfiguration()
        {
            return _dbNavigation.ReconciliationQueueConfigurations.FirstOrDefault(cond => !cond.RQC_IsDeleted);
        }

        //public List<Entity.ClientEntity.GetQueueAssigneeList> GetCurrentReconciliationAssigneeList(Int32 currentAssignmentConfigurationId)
        //{
        //    EntityConnection connection = _dbNavigation.Connection as EntityConnection;
        //    List<Entity.ClientEntity.GetQueueAssigneeList> lstAssigneeList = new List<Entity.ClientEntity.GetQueueAssigneeList>();
        //    using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
        //    {
        //        SqlCommand cmd = new SqlCommand("usp_GetReconciliationQueueAssigneeList", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@AssignmentConfigurationId", currentAssignmentConfigurationId);
        //        SqlDataAdapter adp = new SqlDataAdapter();
        //        adp.SelectCommand = cmd;
        //        DataSet ds = new DataSet();
        //        adp.Fill(ds);
        //        if (ds.Tables.Count > 0)
        //        {
        //            if (ds.Tables[0].Rows.Count > 0)
        //            {
        //                lstAssigneeList = ds.Tables[0].AsEnumerable().Select(col =>
        //                    new Entity.ClientEntity.GetQueueAssigneeList
        //                    {
        //                        AssigneeId = Convert.ToInt32(col["AssigneeId"]),
        //                        OrganizationUserID = Convert.ToInt32(col["OrganizationUserID"]),
        //                        ReviewerName = Convert.ToString(col["ReviewerName"]),
        //                        ReviewerLevel = col["ReviewerLevel"] == DBNull.Value ? String.Empty : Convert.ToString(col["ReviewerLevel"]),
        //                    }).ToList();
        //            }
        //        }
        //    }
        //    return lstAssigneeList;
        //}

        //public List<Entity.ClientEntity.GetUserListApplicableForReview> GetUserListApplicableForReconciliationReview(Int32 assignmentConfigurationId, Int32 currentReviewerId = 0)
        //{
        //    EntityConnection connection = _dbNavigation.Connection as EntityConnection;
        //    List<Entity.ClientEntity.GetUserListApplicableForReview> lstUserList = new List<Entity.ClientEntity.GetUserListApplicableForReview>();
        //    using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
        //    {
        //        SqlCommand cmd = new SqlCommand("usp_GetUserListApplicableForReview", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@AssignmentConfigurationId", assignmentConfigurationId);
        //        cmd.Parameters.AddWithValue("@CurrentReviewerId", currentReviewerId);
        //        SqlDataAdapter adp = new SqlDataAdapter();
        //        adp.SelectCommand = cmd;
        //        DataSet ds = new DataSet();
        //        adp.Fill(ds);
        //        if (ds.Tables.Count > 0)
        //        {
        //            if (ds.Tables[0].Rows.Count > 0)
        //            {
        //                lstUserList = ds.Tables[0].AsEnumerable().Select(col =>
        //                    new Entity.ClientEntity.GetUserListApplicableForReview
        //                    {
        //                        FullName = Convert.ToString(col["FullName"]),
        //                        ID = Convert.ToInt32(col["ID"]),
        //                    }).ToList();
        //            }
        //        }
        //    }
        //    return lstUserList;
        //}

        public Boolean SaveUpdateReconciliationQueueAssignmentConfiguration(ReconciliationQueueConfiguration queueAssignmentConfiguration, Int32 currentLoggedInUserId)
        {
            if (queueAssignmentConfiguration.RQC_ID > AppConsts.NONE)
            {
                ReconciliationQueueConfiguration existingQueueAssignmentConfiguration = GetAssignmentConfigurationById(queueAssignmentConfiguration.RQC_ID);
                existingQueueAssignmentConfiguration.RQC_Description = queueAssignmentConfiguration.RQC_Description;
                existingQueueAssignmentConfiguration.RQC_ReviewerCount = queueAssignmentConfiguration.RQC_ReviewerCount;
                existingQueueAssignmentConfiguration.RQC_IsDeleted = queueAssignmentConfiguration.RQC_IsDeleted;
                existingQueueAssignmentConfiguration.RQC_ModifiedBy = currentLoggedInUserId;
                existingQueueAssignmentConfiguration.RQC_ModifiedOn = DateTime.Now;
                existingQueueAssignmentConfiguration.RQC_RecordPercentage = queueAssignmentConfiguration.RQC_RecordPercentage;

                List<Int32> mappedTenantIDs = queueAssignmentConfiguration.ReconciliationQueueConfigurationTenantMappings.Select(cond => cond.RQCTM_TenantID).ToList();

                IEnumerable<ReconciliationQueueConfigurationTenantMapping> existingReconciliationQueueConfigurationTenantMappings = existingQueueAssignmentConfiguration.ReconciliationQueueConfigurationTenantMappings
                                                                                                                .Where(cond => !cond.RQCTM_IsDeleted);
                foreach (ReconciliationQueueConfigurationTenantMapping existingReconciliationQueueConfigurationTenantMapping in existingReconciliationQueueConfigurationTenantMappings)
                {
                    if (!mappedTenantIDs.Contains(existingReconciliationQueueConfigurationTenantMapping.RQCTM_TenantID))
                    {
                        existingReconciliationQueueConfigurationTenantMapping.RQCTM_IsDeleted = true;
                        existingReconciliationQueueConfigurationTenantMapping.RQCTM_ModifiedBy = currentLoggedInUserId;
                        existingReconciliationQueueConfigurationTenantMapping.RQCTM_ModifiedOn = DateTime.Now;
                    }
                    else
                    {
                        mappedTenantIDs.Remove(existingReconciliationQueueConfigurationTenantMapping.RQCTM_TenantID);
                    }
                }

                foreach (Int32 tenantID in mappedTenantIDs)
                {
                    ReconciliationQueueConfigurationTenantMapping newReconciliationQueueConfigurationTenantMapping = new ReconciliationQueueConfigurationTenantMapping()
                    {
                        RQCTM_TenantID = tenantID,
                        RQCTM_IsDeleted = false,
                        RQCTM_CreatedBy = currentLoggedInUserId,
                        RQCTM_CreatedOn = DateTime.Now,
                    };
                    existingQueueAssignmentConfiguration.ReconciliationQueueConfigurationTenantMappings.Add(newReconciliationQueueConfigurationTenantMapping);
                }
                _dbNavigation.SaveChanges();
                return true;
            }
            else
            {
                _dbNavigation.ReconciliationQueueConfigurations.AddObject(queueAssignmentConfiguration);
                _dbNavigation.SaveChanges();
                return true;
            }
        }

        public ReconciliationQueueConfiguration GetAssignmentConfigurationById(Int32 assignmentConfigurationId)
        {
            return _dbNavigation.ReconciliationQueueConfigurations.FirstOrDefault(x => x.RQC_ID == assignmentConfigurationId && !x.RQC_IsDeleted);
        }

        #region Data Reconciliation Queue

        /// <summary>
        /// Getting 2 dataset from the sp and filling values in contract for further use.
        /// </summary>
        /// <param name="selectedInstitutionIds"></param>
        /// <param name="gridCustomPaging"></param>
        /// <returns></returns>
        List<DataReconciliationQueueContract> ISecurityRepository.GetQueueData(String selectedInstitutionIds, CustomPagingArgsContract gridCustomPaging)
        {
            EntityConnection connection = _dbNavigation.Connection as EntityConnection;
            List<DataReconciliationQueueContract> lstDataReconciliationQueueContract = new List<DataReconciliationQueueContract>();
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("usp_GetReconciliationQueueData", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@InstitutionIds", selectedInstitutionIds);
                cmd.Parameters.AddWithValue("@FilterXml", gridCustomPaging.XML);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        gridCustomPaging.CurrentPageIndex = Convert.ToInt32(Convert.ToString(ds.Tables[0].Rows[0]["CurrentPageIndex"]));
                        gridCustomPaging.VirtualPageCount = Convert.ToInt32(Convert.ToString(ds.Tables[0].Rows[0]["VirtualCount"]));
                    }
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        lstDataReconciliationQueueContract = ds.Tables[1].AsEnumerable().Select(col =>
                            new DataReconciliationQueueContract
                            {
                                FlatComplianceItemReconciliationDataID = (col["FlatComplianceItemReconciliationDataID"]) == DBNull.Value ? 0 : Convert.ToInt32(col["FlatComplianceItemReconciliationDataID"]),
                                ApplicantComplianceItemId = (col["ApplicantComplianceItemId"]) == DBNull.Value ? 0 : Convert.ToInt32(col["ApplicantComplianceItemId"]),
                                ApplicantName = Convert.ToString(col["ApplicantName"]),
                                ApplicantComplianceCategoryId = (col["ApplicantComplianceCategoryId"]) == DBNull.Value ? 0 : Convert.ToInt32(col["ApplicantComplianceCategoryId"]),
                                CategoryID = (col["CategoryID"]) == DBNull.Value ? 0 : Convert.ToInt32(col["CategoryID"]),
                                ComplianceItemId = (col["ComplianceItemId"]) == DBNull.Value ? 0 : Convert.ToInt32(col["ComplianceItemId"]),
                                CategoryName = Convert.ToString(col["CategoryName"]),
                                InstitutionName = Convert.ToString(col["InstitutionName"]),
                                ItemName = Convert.ToString(col["ItemName"]),
                                PackageName = Convert.ToString(col["PackageName"]),
                                Reviewers = Convert.ToString(col["Reviewers"]),
                                SubmissionDate = (col["SubmissionDate"]) == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(col["SubmissionDate"]),
                                //SubmissionDate = Convert.ToDateTime(col["SubmissionDate"]),                                
                                IsActive = Convert.ToBoolean(col["IsActive"]),
                                PackageID = (col["PackageID"]) == DBNull.Value ? 0 : Convert.ToInt32(col["PackageID"]),
                                ApplicantId = (col["ApplicantId"]) == DBNull.Value ? 0 : Convert.ToInt32(col["ApplicantId"]),
                                PackageSubscriptionID = (col["PackageSubscriptionID"]) == DBNull.Value ? 0 : Convert.ToInt32(col["PackageSubscriptionID"]),
                                OrderId = (col["OrderId"]) == DBNull.Value ? 0 : Convert.ToInt32(col["OrderId"]),
                                TenantId = (col["TenantId"]) == DBNull.Value ? 0 : Convert.ToInt32(col["TenantId"])
                            }).ToList();
                    }

                }
            }
            return lstDataReconciliationQueueContract;
        }

        //UAT-1718 : Get reconciliation Item ids list
        List<DataReconciliationQueueContract> ISecurityRepository.GetReconciledItemsList(String institutionIds, Int32 ComplianceItemReconciliationDataID)
        {
            EntityConnection connection = _dbNavigation.Connection as EntityConnection;
            List<DataReconciliationQueueContract> lstReconciliationItem = new List<DataReconciliationQueueContract>();
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("usp_GetReconciledItemsList", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@InstitutionIds", institutionIds);
                cmd.Parameters.AddWithValue("@ComplianceItemReconciliationDataID", ComplianceItemReconciliationDataID);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        lstReconciliationItem = ds.Tables[0].AsEnumerable().Select(col => new DataReconciliationQueueContract
                        {
                            FlatComplianceItemReconciliationDataID = (col["FlatComplianceItemReconciliationDataID"]) == DBNull.Value ? 0 : Convert.ToInt32(col["FlatComplianceItemReconciliationDataID"]),
                            ApplicantComplianceItemId = (col["ApplicantComplianceItemId"]) == DBNull.Value ? 0 : Convert.ToInt32(col["ApplicantComplianceItemId"]),
                            ApplicantComplianceCategoryId = (col["ApplicantComplianceCategoryId"]) == DBNull.Value ? 0 : Convert.ToInt32(col["ApplicantComplianceCategoryId"]),
                            CategoryID = (col["CategoryID"]) == DBNull.Value ? 0 : Convert.ToInt32(col["CategoryID"]),
                            ComplianceItemId = (col["ComplianceItemId"]) == DBNull.Value ? 0 : Convert.ToInt32(col["ComplianceItemId"]),
                            PackageID = (col["PackageID"]) == DBNull.Value ? 0 : Convert.ToInt32(col["PackageID"]),
                            ApplicantId = (col["ApplicantId"]) == DBNull.Value ? 0 : Convert.ToInt32(col["ApplicantId"]),
                            PackageSubscriptionID = (col["PackageSubscriptionID"]) == DBNull.Value ? 0 : Convert.ToInt32(col["PackageSubscriptionID"]),
                            TenantId = (col["TenantId"]) == DBNull.Value ? 0 : Convert.ToInt32(col["TenantId"])
                        }).ToList();
                    }
                }
            }
            return lstReconciliationItem;
        }

        Dictionary<Int32, Boolean> ISecurityRepository.CheckIfItemsAreInReconciliationProcess(List<Int32> itemDataIDs, Int32 tenantID)
        {
            List<Int32> lstItemIdsInReconciliation = _dbNavigation.FlatComplianceItemReconciliationDatas
                                                                                                    .Where(cond => cond.FCIRD_IsActive
                                                                                                    && !cond.FCIRD_IsDeleted
                                                                                                    && itemDataIDs.Contains(cond.FCIRD_ApplicantComplianceItemId))
                                                                                                    .Select(col => col.FCIRD_ApplicantComplianceItemId)
                                                                                                    .ToList();
            Dictionary<Int32, Boolean> dic = new Dictionary<int, bool>();
            foreach (Int32 id in itemDataIDs)
            {
                dic.Add(id, (!lstItemIdsInReconciliation.IsNullOrEmpty() && lstItemIdsInReconciliation.Contains(id)));
            }
            return dic;
        }

        #endregion


        #region UAT-1741:604 notification should only have to be clicked upon login once per 24 hours.
        public EmploymentDisclosureDetail GetEmploymentDisclosureDetails(Int32 organizationUserID)
        {
            return _dbNavigation.EmploymentDisclosureDetails.Where(cond => cond.EDD_OrganizationUserID == organizationUserID && !cond.EDD_IsDeleted)
                                                            .OrderByDescending(x => x.EDD_ID).FirstOrDefault();
        }
        #endregion

        //UAT-2264
        public List<ColumnsConfigurationContract> GetScreenColumns(String ScreenName, Int32 CurrentLoggedInUserID)
        {
            EntityConnection connection = _dbNavigation.Connection as EntityConnection;
            List<ColumnsConfigurationContract> ColumnsConfigurationData = new List<ColumnsConfigurationContract>();
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("usp_GetScreenColumns", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ScreenName", ScreenName);
                cmd.Parameters.AddWithValue("@OrgUserID", CurrentLoggedInUserID);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ColumnsConfigurationData = ds.Tables[0].AsEnumerable().Select(col =>
                            new ColumnsConfigurationContract
                            {
                                GridDisplayName = col["GridDisplayName"] == DBNull.Value ? String.Empty : Convert.ToString(col["GridDisplayName"]),
                                GridCode = col["GridCode"] == DBNull.Value ? String.Empty : Convert.ToString(col["GridCode"]),
                                ColumnID = Convert.ToInt32(col["ColumnID"]),
                                ColumnName = col["ColumnName"] == DBNull.Value ? String.Empty : Convert.ToString(col["ColumnName"]),
                                ColumnUniqueName = col["ColumnUniqueName"] == DBNull.Value ? String.Empty : Convert.ToString(col["ColumnUniqueName"]),
                                IsColumnVisible = Convert.ToBoolean(col["IsVisible"])
                            }).ToList();
                    }
                }
            }
            return ColumnsConfigurationData;
        }

        public Boolean SaveUserScreenColumnMapping(Dictionary<Int32, Boolean> columnVisibility, Int32 CurrentLoggedInUserID, Int32 OrganisationUserID)
        {
            List<UserScreenColumnMapping> obj = _dbNavigation.UserScreenColumnMappings.Where(cond => cond.USCM_OrganizationUserID == OrganisationUserID && !cond.USCM_IsDeleted).ToList();

            foreach (var item in columnVisibility)
            {
                UserScreenColumnMapping userScreenColumnMapping = obj.Where(cond => cond.USCM_OrganizationUserID == OrganisationUserID
                                    && !cond.USCM_IsDeleted && cond.USCM_ScreenColumnsID == item.Key).FirstOrDefault();

                if (!userScreenColumnMapping.IsNullOrEmpty())
                {
                    //Update
                    if (userScreenColumnMapping.USCM_IsVisible != item.Value)
                    {
                        userScreenColumnMapping.USCM_IsVisible = item.Value;
                        //userScreenColumnMapping.USCM_ModifiedBy = CurrentLoggedInUserID;
                        //userScreenColumnMapping.USCM_ModifiedOn = DateTime.Now;

                    }
                    obj.ForEach(s =>
                    {
                        s.USCM_ModifiedBy = CurrentLoggedInUserID;
                        s.USCM_ModifiedOn = DateTime.Now;
                    });
                }
                else
                {
                    //Add new
                    userScreenColumnMapping = new UserScreenColumnMapping();
                    userScreenColumnMapping.USCM_OrganizationUserID = OrganisationUserID;
                    userScreenColumnMapping.USCM_ScreenColumnsID = item.Key;
                    userScreenColumnMapping.USCM_IsVisible = item.Value;
                    userScreenColumnMapping.USCM_IsDeleted = false;
                    userScreenColumnMapping.USCM_CreatedBy = CurrentLoggedInUserID;
                    userScreenColumnMapping.USCM_CreatedOn = DateTime.Now;
                    _dbNavigation.UserScreenColumnMappings.AddObject(userScreenColumnMapping);
                }

            }

            _dbNavigation.SaveChanges();
            return true;
        }

        public List<String> GetScreenColumnsToHide(String GridCode, Int32 CurrentLoggedInUserID)
        {
            EntityConnection connection = _dbNavigation.Connection as EntityConnection;
            List<String> ColumnsToHide = new List<String>();
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("usp_GetScreenColumnsToHide", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@GridCode", GridCode);
                cmd.Parameters.AddWithValue("@OrgUserID", CurrentLoggedInUserID);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ColumnsToHide = ds.Tables[0].AsEnumerable().Select(col =>
                                             Convert.ToString(col["ColumnName"])).ToList();
                    }
                }
            }
            return ColumnsToHide;
        }

        /// <summary>
        /// UAT-2296 For schools where the document association at time of upload is turned on, edit document on manage documents screen should allow additional items/exceptions to be added to uploaded documents
        /// </summary>
        /// <param name="applicantDocumentId"></param>
        /// <param name="InProgressDocumentStatus"></param>
        /// <param name="OrgID"></param>
        /// <returns></returns>
        Boolean ISecurityRepository.UpdateFlatDataEntryQueueRecord(Int32 applicantDocumentId, short InProgressDocumentStatus, Int32 OrgID, Int32 tenantID)
        {
            FlatDataEntryQueue newRecord = _dbNavigation.FlatDataEntryQueues.Where(cnd => cnd.FDEQ_ApplicantDocumentID == applicantDocumentId && cnd.FDEQ_IsDeleted == false && cnd.FDEQ_TenantID == tenantID).FirstOrDefault();
            if (!newRecord.IsNullOrEmpty())
            {
                newRecord.FDEQ_DataEntryDocumentStatusID = InProgressDocumentStatus;
                newRecord.FDEQ_ModifiedOn = DateTime.Now;
                newRecord.FDEQ_ModifiedByID = OrgID;
                _dbNavigation.SaveChanges();
                return true;
            }
            return false;
        }

        #region UAT-2302-As a student, I should see a popup on login if i have logged in using a non-preferred browser.
        public Boolean CheckUserNonPrefferedBrowserOption(Int32 currentLoggedInUserID, Int16 utilityFeaturesId)
        {
            if (_dbNavigation.UtilityFeatureUsages.Where(s => s.UFU_UtilityFeatureID == utilityFeaturesId && s.UFU_OrgUserID == currentLoggedInUserID && s.UFU_IgnoreAlert == true && s.UFU_IsDeleted == false).Any())
            { return true; }
            else
            { return false; }
        }
        public Boolean SaveNonPreferredBrowserLog(Int32 currentLoggedInUserID, Int16 utilityFeaturesId)
        {
            //checking if any log already present then do nothing simply return true
            if (_dbNavigation.UtilityFeatureUsages.Where(s => s.UFU_UtilityFeatureID == utilityFeaturesId && s.UFU_OrgUserID == currentLoggedInUserID && s.UFU_IgnoreAlert == true && s.UFU_IsDeleted == false).Any())
            { return true; }
            else
            {
                //Insert log into UtilityFeatureUsage table with utilityFeaturesId=5 .Here 5 represent the UFU_UtilityFeatureID from lkpUtilityFeatures table 
                UtilityFeatureUsage dbInsert = new UtilityFeatureUsage();
                dbInsert.UFU_UtilityFeatureID = utilityFeaturesId;//NonPreferredBrowser
                dbInsert.UFU_OrgUserID = currentLoggedInUserID;
                dbInsert.UFU_Count = 1;
                dbInsert.UFU_IgnoreAlert = true;
                dbInsert.UFU_IsDeleted = false;
                dbInsert.UFU_CreatedBy = currentLoggedInUserID;
                dbInsert.UFU_CreatedOn = DateTime.Now.Date;
                _dbNavigation.UtilityFeatureUsages.AddObject(dbInsert);
                if (_dbNavigation.SaveChanges() > AppConsts.NONE)
                    return true;
                else
                    return false;
            }
        }
        #endregion

        //UAT-2266
        public String GetAssignedUserRoleNames(Int32 OrgUserId, Int32 TenantId)
        {
            EntityConnection connection = _dbNavigation.Connection as EntityConnection;
            String RoleNames = String.Empty;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("usp_GetAssignedUserRoleName", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OrgUserID", OrgUserId);
                cmd.Parameters.AddWithValue("@TenantID", TenantId);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        RoleNames = Convert.ToString(ds.Tables[0].Rows[0]["RoleNames"]);
                    }
                }
            }
            return RoleNames;
        }

        #region UAT-2304: Random review of auto completed supplements

        /// <summary>
        /// Get Current Supplement Automation Configuration
        /// </summary>
        /// <returns></returns>
        SupplementAutomationConfiguration ISecurityRepository.GetCurrentSupplementAutomationConfiguration()
        {
            return _dbNavigation.SupplementAutomationConfigurations.FirstOrDefault(cond => !cond.SAC_IsDeleted);
        }

        /// <summary>
        /// Save/Update Supplement Automation Configuration
        /// </summary>
        /// <param name="supplementAutomationConfiguration"></param>
        /// <param name="currentUserID"></param>
        /// <returns></returns>
        Boolean ISecurityRepository.SaveUpdateSupplementAutomationConfiguration(SupplementAutomationConfiguration supplementAutomationConfiguration, Int32 currentUserID)
        {
            if (supplementAutomationConfiguration.SAC_ID > AppConsts.NONE)
            {
                SupplementAutomationConfiguration existingSupplementAutomationConfiguration = GetSupplementAutomationConfigurationById(supplementAutomationConfiguration.SAC_ID);
                existingSupplementAutomationConfiguration.SAC_Description = supplementAutomationConfiguration.SAC_Description;
                existingSupplementAutomationConfiguration.SAC_RecordPercentage = supplementAutomationConfiguration.SAC_RecordPercentage;
                existingSupplementAutomationConfiguration.SAC_IsDeleted = supplementAutomationConfiguration.SAC_IsDeleted;
                existingSupplementAutomationConfiguration.SAC_ModifiedBy = currentUserID;
                existingSupplementAutomationConfiguration.SAC_ModifiedOn = DateTime.Now;

                List<Int32> mappedTenantIDs = supplementAutomationConfiguration.SupplementAutomationConfigurationTenantMappings.Select(cond => cond.SACTM_TenantID).ToList();

                IEnumerable<SupplementAutomationConfigurationTenantMapping> existingSupplementAutomationConfigurationTenantMappings = existingSupplementAutomationConfiguration.SupplementAutomationConfigurationTenantMappings
                                                                                                                .Where(cond => !cond.SACTM_IsDeleted);
                foreach (SupplementAutomationConfigurationTenantMapping existingSupplementAutomationConfigurationTenantMapping in existingSupplementAutomationConfigurationTenantMappings)
                {
                    if (!mappedTenantIDs.Contains(existingSupplementAutomationConfigurationTenantMapping.SACTM_TenantID))
                    {
                        existingSupplementAutomationConfigurationTenantMapping.SACTM_IsDeleted = true;
                        existingSupplementAutomationConfigurationTenantMapping.SACTM_ModifiedBy = currentUserID;
                        existingSupplementAutomationConfigurationTenantMapping.SACTM_ModifiedOn = DateTime.Now;
                    }
                    else
                    {
                        mappedTenantIDs.Remove(existingSupplementAutomationConfigurationTenantMapping.SACTM_TenantID);
                    }
                }

                foreach (Int32 tenantID in mappedTenantIDs)
                {
                    SupplementAutomationConfigurationTenantMapping newSupplementAutomationConfigurationTenantMapping = new SupplementAutomationConfigurationTenantMapping()
                    {
                        SACTM_TenantID = tenantID,
                        SACTM_IsDeleted = false,
                        SACTM_CreatedBy = currentUserID,
                        SACTM_CreatedOn = DateTime.Now,
                    };
                    existingSupplementAutomationConfiguration.SupplementAutomationConfigurationTenantMappings.Add(newSupplementAutomationConfigurationTenantMapping);
                }
                _dbNavigation.SaveChanges();
                return true;
            }
            else
            {
                _dbNavigation.SupplementAutomationConfigurations.AddObject(supplementAutomationConfiguration);
                _dbNavigation.SaveChanges();
                return true;
            }
        }

        /// <summary>
        /// Get Supplement Automation Configuration By Id
        /// </summary>
        /// <param name="supplementAutomationConfigurationID"></param>
        /// <returns></returns>
        public SupplementAutomationConfiguration GetSupplementAutomationConfigurationById(Int32 supplementAutomationConfigurationID)
        {
            return _dbNavigation.SupplementAutomationConfigurations.FirstOrDefault(x => x.SAC_ID == supplementAutomationConfigurationID && !x.SAC_IsDeleted);
        }

        #endregion


        #region UAT-2515
        Tuple<Int32, Int32> ISecurityRepository.ExternalUserTenantId(String externalID, String mappingCode)
        {
            var ExternalUserDetails = _dbNavigation.IntegrationClientOrganizationUserMaps.Where(cond => cond.ICOUM_ExternalID.ToLower() == externalID.ToLower() && cond.IntegrationClient.MappingGroup.AMG_Code.ToString() == mappingCode.ToString() && !cond.ICOUM_IsDeleted).FirstOrDefault();
            if (ExternalUserDetails.IsNotNull() && ExternalUserDetails.OrganizationUser.IsNotNull() && ExternalUserDetails.OrganizationUser.Organization.IsNotNull()
               && ExternalUserDetails.OrganizationUser.Organization.TenantID.HasValue)
            {
                return new Tuple<Int32, Int32>(ExternalUserDetails.OrganizationUser.Organization.TenantID.Value, ExternalUserDetails.ICOUM_IntegrationClientID);
            }
            else
            {

                return new Tuple<Int32, Int32>(AppConsts.NONE, _dbNavigation.IntegrationClients.Where(sel => sel.MappingGroup.AMG_Code.ToString() == mappingCode).Select(d => d.IC_ID).FirstOrDefault());
            }
        }

        Dictionary<Int32, Int32> ISecurityRepository.ExternalUserTenantIdBySchoolName(String token, String schoolName)
        {
            Dictionary<Int32, Int32> UserDetails = new Dictionary<Int32, Int32>();
            var ExternalUserDetails = _dbNavigation.SecurityTokens.Where(cond => cond.ST_Token.ToLower() == token.ToLower()).FirstOrDefault();
            if (ExternalUserDetails.IsNotNull())
            {
                var data = ExternalUserDetails.IntegrationClient.MappingGroup.MappingGroupMappingTypes.FirstOrDefault();
                if (data.IsNotNull())
                {
                    var tenantDetails = data.Mappings.Where(d => d.AM_Source.ToLower() == schoolName.ToLower() && !d.AM_IsDeleted).Select(d => new { d.AM_Target, d.AM_Source }).FirstOrDefault();
                    if (tenantDetails.IsNotNull())
                    {
                        UserDetails.Add(Convert.ToInt32(tenantDetails.AM_Target), ExternalUserDetails.IntegrationClient.IC_ID);
                        return UserDetails;
                    }
                }
            }
            UserDetails.Add(0, 0);
            return UserDetails;
        }
        Boolean ISecurityRepository.InsertExternalUserRegistrationLogInIntegrationClientOrganizationUserMap(Int32 organizationUserId, Int32 integrationClientId, String externalId)
        {
            if (!_dbNavigation.IntegrationClientOrganizationUserMaps.Where(con => con.ICOUM_OrgUserID == organizationUserId && con.ICOUM_ExternalID == externalId && con.ICOUM_IntegrationClientID == integrationClientId).Any())
            {
                var DbInsertLogExternalUserRegistrationLogInIntegrationClientOrganizationUserMap = new IntegrationClientOrganizationUserMap();
                DbInsertLogExternalUserRegistrationLogInIntegrationClientOrganizationUserMap.ICOUM_IntegrationClientID = integrationClientId;
                DbInsertLogExternalUserRegistrationLogInIntegrationClientOrganizationUserMap.ICOUM_OrgUserID = organizationUserId;
                DbInsertLogExternalUserRegistrationLogInIntegrationClientOrganizationUserMap.ICOUM_ExternalID = externalId;
                DbInsertLogExternalUserRegistrationLogInIntegrationClientOrganizationUserMap.ICOUM_CreatedByID = organizationUserId;
                DbInsertLogExternalUserRegistrationLogInIntegrationClientOrganizationUserMap.ICOUM_CreatedOn = DateTime.Now;
                DbInsertLogExternalUserRegistrationLogInIntegrationClientOrganizationUserMap.ICOUM_IsDeleted = false;
                _dbNavigation.IntegrationClientOrganizationUserMaps.AddObject(DbInsertLogExternalUserRegistrationLogInIntegrationClientOrganizationUserMap);
                var result = _dbNavigation.SaveChanges();
                if (result > 0)
                    return true;
                else
                    return false;
            }
            else
                return true;

        }
        Boolean ISecurityRepository.ValidateToken(String token)
        {
            EntityConnection connection = _dbNavigation.Connection as EntityConnection;

            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("api.usp_VerifyToken", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Token", token);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        //getTokenDetails = ds.Tables[0].AsEnumerable().Select(col =>
                        //    new ExternalLoginDataContract
                        //    {
                        //        IntegrationClientID = col["ST_IntegrationClientID"] == DBNull.Value ? 0 : Convert.ToInt32(col["ST_IntegrationClientID"]),
                        //        Token = col["ST_Token"] == DBNull.Value ? String.Empty : Convert.ToString(col["ST_Token"])
                        //    }).ToList();
                        return true;
                    }
                }
            }
            return false;
        }

        List<ExternalLoginDataContract> ISecurityRepository.GetMatchingOrganisationUserList(ExternalLoginDataContract objExternalLoginDataContract)
        {

            EntityConnection connection = _dbNavigation.Connection as EntityConnection;


            List<ExternalLoginDataContract> matchingUserSearchData = new List<ExternalLoginDataContract>();
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("api.usp_GetMatchingApplicants_Default", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@XmlData", objExternalLoginDataContract.CreateXml());
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        matchingUserSearchData = ds.Tables[0].AsEnumerable().Select(col =>
                            new ExternalLoginDataContract
                            {
                                FirstName = col["FirstName"] == DBNull.Value ? String.Empty : Convert.ToString(col["FirstName"]),
                                LastName = col["LastName"] == DBNull.Value ? String.Empty : Convert.ToString(col["LastName"]),
                                UserName = col["UserName"] == DBNull.Value ? String.Empty : Convert.ToString(col["UserName"]),
                                Email1 = col["PrimaryEmailAddress"] == DBNull.Value ? String.Empty : Convert.ToString(col["PrimaryEmailAddress"]),
                                Email2 = col["SecondaryEmailAddress"] == DBNull.Value ? String.Empty : Convert.ToString(col["SecondaryEmailAddress"]),
                                PhoneNumber = col["PhoneNumber"] == DBNull.Value ? String.Empty : Convert.ToString(col["PhoneNumber"]),
                                OrganizationUserID = col["OrganizationUserID"] == DBNull.Value ? 0 : Convert.ToInt32(col["OrganizationUserID"]),
                                OrganizationID = col["OrganizationID"] == DBNull.Value ? 0 : Convert.ToInt32(col["OrganizationID"]),
                                SSN = col["SSN"] == DBNull.Value ? String.Empty : Convert.ToString(col["SSN"]),
                                TenantID = col["TenantID"] == DBNull.Value ? 0 : Convert.ToInt32(col["TenantID"]),
                                TenantName = col["TenantName"] == DBNull.Value ? String.Empty : Convert.ToString(col["TenantName"])
                            }).ToList();
                    }
                }
            }
            return matchingUserSearchData;
        }
        List<ExternalDataFromTokenDataContract> ISecurityRepository.GetDataFromSecurityToken(String tokenId)
        {
            EntityConnection connection = _dbNavigation.Connection as EntityConnection;
            List<ExternalDataFromTokenDataContract> ExternalDataFromTokenData = new List<ExternalDataFromTokenDataContract>();
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("api.usp_GetDataFromSecurityToken", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Token", tokenId);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ExternalDataFromTokenData = ds.Tables[0].AsEnumerable().Select(col =>
                             new ExternalDataFromTokenDataContract
                             {
                                 Name = col["Name"] == DBNull.Value ? String.Empty : Convert.ToString(col["Name"]),
                                 Value = col["Value"] == DBNull.Value ? String.Empty : Convert.ToString(col["Value"])
                             }).ToList();
                    }
                }
            }

            return ExternalDataFromTokenData;
        }
        #endregion

        //UAT-2448
        List<CountryIdentificationDetailContract> ISecurityRepository.GetCountryIdentificationDetails()
        {
            List<CountryIdentificationDetailContract> lstCountryIdentificationDetailContract = new List<CountryIdentificationDetailContract>();

            List<CountryIdentificationNumber> LstCountryIdentificationDetail = _dbNavigation.CountryIdentificationNumbers.Where(cond => cond.CIN_IsDeleted == false).OrderBy(x => x.CIN_Country_Name).ToList();
            if (!LstCountryIdentificationDetail.IsNullOrEmpty())
            {

                foreach (CountryIdentificationNumber item in LstCountryIdentificationDetail)
                {
                    CountryIdentificationDetailContract CountryIdentificationDetailContract = new CountryIdentificationDetailContract();
                    CountryIdentificationDetailContract.CIN_ID = item.CIN_ID;
                    CountryIdentificationDetailContract.CIN_Country_Name = item.CIN_Country_Name;
                    CountryIdentificationDetailContract.CIN_Identification_Number = item.CIN_Identification_Number;
                    lstCountryIdentificationDetailContract.Add(CountryIdentificationDetailContract);
                }
            }
            return lstCountryIdentificationDetailContract;
        }

        #region Get ClientDBConfig for Selected TenantIds
        /// <summary>
        /// 
        /// </summary>
        /// <returns>
        /// </returns>
        ClientDBConfiguration ISecurityRepository.GetClientDBConfigurationForSelectedTenants(Int32 TenantId)
        {
            String code = TenantType.Institution.GetStringValue();
            return _dbNavigation.ClientDBConfigurations.Where(cond => cond.Tenant.lkpTenantType.TenantTypeCode == code
                        && cond.CDB_TenantID == TenantId
                        && cond.Tenant.IsDeleted == false).FirstOrDefault();
        }
        #endregion

        #region UAT-2792:
        Boolean ISecurityRepository.SaveShibbolethSSOSessionData(String SessionData, String TargetURL)
        {
            ShibbolethSSOSessionData ShibbolethSSOSession = new ShibbolethSSOSessionData();

            ShibbolethSSOSession.SSSD_SessionData = SessionData;
            ShibbolethSSOSession.SSSD_TargetURL = TargetURL;
            ShibbolethSSOSession.SSSD_CreatedDate = DateTime.Now;

            _dbNavigation.ShibbolethSSOSessionDatas.AddObject(ShibbolethSSOSession);
            _dbNavigation.SaveChanges();

            return true;
        }

        Int32 ISecurityRepository.GetIntegrationClientForShibboleth(String MappingGroupCode)
        {
            Int32 mappingGroupID = _dbNavigation.MappingGroups.Where(cond => cond.AMG_Code == new Guid(MappingGroupCode) && !cond.AMG_IsDeleted).Select(sel => sel.AMG_ID).FirstOrDefault();
            if (mappingGroupID > AppConsts.NONE)
            {
                return _dbNavigation.IntegrationClients.Where(cond => cond.IC_MappingGroupID == mappingGroupID).Select(sel => sel.IC_ID).FirstOrDefault();
            }
            return AppConsts.NONE;
        }
        List<String> ISecurityRepository.GetClientIdpUrl(String ClientUrl)
        {
            EntityConnection connection = _dbNavigation.Connection as EntityConnection;
            //String RoleNames = String.Empty;
            List<String> outputData = new List<String>();
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("usp_GetClientIdpUrl", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ClientUrl", ClientUrl);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        outputData.Add(Convert.ToString(ds.Tables[0].Rows[0]["AM_ID"]));
                        outputData.Add(Convert.ToString(ds.Tables[0].Rows[0]["AM_Source"]));
                        outputData.Add(Convert.ToString(ds.Tables[0].Rows[0]["AM_Target"]));
                    }
                }
            }
            return outputData;
        }
        //UAT-2883
        String ISecurityRepository.GetClientByHostName(String ClientUrl)
        {
            EntityConnection connection = _dbNavigation.Connection as EntityConnection;
            String clientName = String.Empty;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("usp_GetClientByHostName", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ClientUrl", ClientUrl);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        clientName = Convert.ToString(ds.Tables[0].Rows[0]["IC_Name"]);
                    }
                }
            }
            return clientName;
        }

        List<ExternalLoginDataContract> ISecurityRepository.GetMatchingUsersForShibboleth(String shibbolethUniqueId, String shibbolethAttributeId, String shibbolethEmail, Int32 tenantID, Boolean isApplicant, String shibbolethHandlerType, String ShibbolethFirstName, String ShibbolethLastName,String ShibbolethRoleString)
        {
            EntityConnection connection = _dbNavigation.Connection as EntityConnection;
            List<ExternalLoginDataContract> matchingUserSearchData = new List<ExternalLoginDataContract>();

            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                //Calling SP for matching logic UCONN
                if (shibbolethHandlerType.ToLower() == AppConsts.SHIBBOLETH_UCONN)
                {
                    SqlCommand cmd = new SqlCommand("api.usp_GetExistingUserForShibboleth", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PeopleSoftID", shibbolethUniqueId);
                    cmd.Parameters.AddWithValue("@NetID", shibbolethAttributeId);
                    cmd.Parameters.AddWithValue("@EmailID", shibbolethEmail);
                    cmd.Parameters.AddWithValue("@TenantID", tenantID);
                    cmd.Parameters.AddWithValue("@IsApplicant", isApplicant);
                    SqlDataAdapter adp = new SqlDataAdapter();
                    adp.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    adp.Fill(ds);

                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            matchingUserSearchData = ds.Tables[0].AsEnumerable().Select(col =>
                                new ExternalLoginDataContract
                                {
                                    FirstName = col["FirstName"] == DBNull.Value ? String.Empty : Convert.ToString(col["FirstName"]),
                                    LastName = col["LastName"] == DBNull.Value ? String.Empty : Convert.ToString(col["LastName"]),
                                    UserName = col["UserName"] == DBNull.Value ? String.Empty : Convert.ToString(col["UserName"]),
                                    Email1 = col["PrimaryEmailAddress"] == DBNull.Value ? String.Empty : Convert.ToString(col["PrimaryEmailAddress"]),
                                    Email2 = col["SecondaryEmailAddress"] == DBNull.Value ? String.Empty : Convert.ToString(col["SecondaryEmailAddress"]),
                                    PhoneNumber = col["PhoneNumber"] == DBNull.Value ? String.Empty : Convert.ToString(col["PhoneNumber"]),
                                    OrganizationUserID = col["OrganizationUserID"] == DBNull.Value ? 0 : Convert.ToInt32(col["OrganizationUserID"]),
                                    OrganizationID = col["OrganizationID"] == DBNull.Value ? 0 : Convert.ToInt32(col["OrganizationID"]),
                                    SSN = col["SSN"] == DBNull.Value ? String.Empty : Convert.ToString(col["SSN"]),
                                    TenantID = col["TenantID"] == DBNull.Value ? 0 : Convert.ToInt32(col["TenantID"]),
                                    TenantName = col["TenantName"] == DBNull.Value ? String.Empty : Convert.ToString(col["TenantName"]),
                                    IsFirstLogin = col["IsFirstLogin"] == DBNull.Value ? false : Convert.ToBoolean(col["IsFirstLogin"])
                                }).ToList();
                        }
                    }
                }
                //Calling SP for matching logic WGU.       
                else if (shibbolethHandlerType.ToLower() == AppConsts.SHIBBOLETH_WGU)
                {
                    SqlCommand cmd = new SqlCommand("api.usp_GetExistingUserForShibboleth_WGU", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ShibbolethUniqueID", shibbolethUniqueId);
                    cmd.Parameters.AddWithValue("@ShibbolethCustomAttributeValue", shibbolethAttributeId);
                    cmd.Parameters.AddWithValue("@ShibbolethEmail", shibbolethEmail);
                    cmd.Parameters.AddWithValue("@TenantID", tenantID);
                    cmd.Parameters.AddWithValue("@IsApplicant", isApplicant);
                    cmd.Parameters.AddWithValue("@ShibbolethFirstName", ShibbolethFirstName);
                    cmd.Parameters.AddWithValue("@ShibbolethLastName", ShibbolethLastName);
                    SqlDataAdapter adp = new SqlDataAdapter();
                    adp.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    adp.Fill(ds);

                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            matchingUserSearchData = ds.Tables[0].AsEnumerable().Select(col =>
                                new ExternalLoginDataContract
                                {
                                    FirstName = col["FirstName"] == DBNull.Value ? String.Empty : Convert.ToString(col["FirstName"]),
                                    LastName = col["LastName"] == DBNull.Value ? String.Empty : Convert.ToString(col["LastName"]),
                                    UserName = col["UserName"] == DBNull.Value ? String.Empty : Convert.ToString(col["UserName"]),
                                    Email1 = col["PrimaryEmailAddress"] == DBNull.Value ? String.Empty : Convert.ToString(col["PrimaryEmailAddress"]),
                                    Email2 = col["SecondaryEmailAddress"] == DBNull.Value ? String.Empty : Convert.ToString(col["SecondaryEmailAddress"]),
                                    PhoneNumber = col["PhoneNumber"] == DBNull.Value ? String.Empty : Convert.ToString(col["PhoneNumber"]),
                                    OrganizationUserID = col["OrganizationUserID"] == DBNull.Value ? 0 : Convert.ToInt32(col["OrganizationUserID"]),
                                    OrganizationID = col["OrganizationID"] == DBNull.Value ? 0 : Convert.ToInt32(col["OrganizationID"]),
                                    SSN = col["SSN"] == DBNull.Value ? String.Empty : Convert.ToString(col["SSN"]),
                                    TenantID = col["TenantID"] == DBNull.Value ? 0 : Convert.ToInt32(col["TenantID"]),
                                    TenantName = col["TenantName"] == DBNull.Value ? String.Empty : Convert.ToString(col["TenantName"]),
                                    IsFirstLogin = col["IsFirstLogin"] == DBNull.Value ? false : Convert.ToBoolean(col["IsFirstLogin"])
                                }).ToList();
                        }
                    }
                }

                //Release 181:4998

                //Calling SP for matching logic Ross.       
                else if (shibbolethHandlerType.ToLower() == AppConsts.SHIBBOLETH_ROSS)
                {
                    SqlCommand cmd = new SqlCommand("api.usp_GetExistingUserForShibboleth_Ross", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ShibbolethUniqueID", shibbolethUniqueId);
                    cmd.Parameters.AddWithValue("@ShibbolethEmail", shibbolethEmail);
                    cmd.Parameters.AddWithValue("@ShibbolethFirstName", ShibbolethFirstName);
                    cmd.Parameters.AddWithValue("@ShibbolethLastName", ShibbolethLastName);
                    cmd.Parameters.AddWithValue("@ShibbolethRoleName", ShibbolethRoleString);
                    cmd.Parameters.AddWithValue("@IsApplicant", isApplicant);

                    SqlDataAdapter adp = new SqlDataAdapter();
                    adp.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    adp.Fill(ds);

                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            matchingUserSearchData = ds.Tables[0].AsEnumerable().Select(col =>
                                new ExternalLoginDataContract
                                {
                                    FirstName = col["FirstName"] == DBNull.Value ? String.Empty : Convert.ToString(col["FirstName"]),
                                    LastName = col["LastName"] == DBNull.Value ? String.Empty : Convert.ToString(col["LastName"]),
                                    UserName = col["UserName"] == DBNull.Value ? String.Empty : Convert.ToString(col["UserName"]),
                                    Email1 = col["PrimaryEmailAddress"] == DBNull.Value ? String.Empty : Convert.ToString(col["PrimaryEmailAddress"]),
                                    Email2 = col["SecondaryEmailAddress"] == DBNull.Value ? String.Empty : Convert.ToString(col["SecondaryEmailAddress"]),
                                    PhoneNumber = col["PhoneNumber"] == DBNull.Value ? String.Empty : Convert.ToString(col["PhoneNumber"]),
                                    OrganizationUserID = col["OrganizationUserID"] == DBNull.Value ? 0 : Convert.ToInt32(col["OrganizationUserID"]),
                                    OrganizationID = col["OrganizationID"] == DBNull.Value ? 0 : Convert.ToInt32(col["OrganizationID"]),
                                    SSN = col["SSN"] == DBNull.Value ? String.Empty : Convert.ToString(col["SSN"]),
                                    TenantID = col["TenantID"] == DBNull.Value ? 0 : Convert.ToInt32(col["TenantID"]),
                                    TenantName = col["TenantName"] == DBNull.Value ? String.Empty : Convert.ToString(col["TenantName"]),
                                    IsFirstLogin = col["IsFirstLogin"] == DBNull.Value ? false : Convert.ToBoolean(col["IsFirstLogin"])
                                }).ToList();
                        }
                    }
                }
                //Calling SP for matching logic UPENN.  
                else if (shibbolethHandlerType.ToLower() == AppConsts.SHIBBOLETH_UPENN)
                {
                    SqlCommand cmd = new SqlCommand("api.usp_GetExistingUserForShibboleth_UPENN", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ShibbolethUniqueID", shibbolethUniqueId);
                    cmd.Parameters.AddWithValue("@IsApplicant", isApplicant);
                    cmd.Parameters.AddWithValue("@ShibbolethFirstName", ShibbolethFirstName);
                    cmd.Parameters.AddWithValue("@ShibbolethLastName", ShibbolethLastName);
                    SqlDataAdapter adp = new SqlDataAdapter();
                    adp.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    adp.Fill(ds);

                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            matchingUserSearchData = ds.Tables[0].AsEnumerable().Select(col =>
                                new ExternalLoginDataContract
                                {
                                    FirstName = col["FirstName"] == DBNull.Value ? String.Empty : Convert.ToString(col["FirstName"]),
                                    LastName = col["LastName"] == DBNull.Value ? String.Empty : Convert.ToString(col["LastName"]),
                                    UserName = col["UserName"] == DBNull.Value ? String.Empty : Convert.ToString(col["UserName"]),
                                    Email1 = col["PrimaryEmailAddress"] == DBNull.Value ? String.Empty : Convert.ToString(col["PrimaryEmailAddress"]),
                                    Email2 = col["SecondaryEmailAddress"] == DBNull.Value ? String.Empty : Convert.ToString(col["SecondaryEmailAddress"]),
                                    PhoneNumber = col["PhoneNumber"] == DBNull.Value ? String.Empty : Convert.ToString(col["PhoneNumber"]),
                                    OrganizationUserID = col["OrganizationUserID"] == DBNull.Value ? 0 : Convert.ToInt32(col["OrganizationUserID"]),
                                    OrganizationID = col["OrganizationID"] == DBNull.Value ? 0 : Convert.ToInt32(col["OrganizationID"]),
                                    SSN = col["SSN"] == DBNull.Value ? String.Empty : Convert.ToString(col["SSN"]),
                                    TenantID = col["TenantID"] == DBNull.Value ? 0 : Convert.ToInt32(col["TenantID"]),
                                    TenantName = col["TenantName"] == DBNull.Value ? String.Empty : Convert.ToString(col["TenantName"]),
                                    IsFirstLogin = col["IsFirstLogin"] == DBNull.Value ? false : Convert.ToBoolean(col["IsFirstLogin"])
                                }).ToList();
                        }
                    }

                }
                //UAT-3540 
                //For NYU
                else if (shibbolethHandlerType.ToLower() == AppConsts.SHIBBOLETH_NYU)
                {
                    SqlCommand cmd = new SqlCommand("api.usp_GetExistingUserForShibboleth_NYU", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ShibbolethUniqueID", shibbolethUniqueId);
                    cmd.Parameters.AddWithValue("@IsApplicant", isApplicant);
                    cmd.Parameters.AddWithValue("@ShibbolethFirstName", ShibbolethFirstName);
                    cmd.Parameters.AddWithValue("@ShibbolethLastName", ShibbolethLastName);
                    cmd.Parameters.AddWithValue("@ShibbolethEmal", shibbolethEmail);
                    SqlDataAdapter adp = new SqlDataAdapter();
                    adp.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    adp.Fill(ds);

                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            matchingUserSearchData = ds.Tables[0].AsEnumerable().Select(col =>
                                new ExternalLoginDataContract
                                {
                                    FirstName = col["FirstName"] == DBNull.Value ? String.Empty : Convert.ToString(col["FirstName"]),
                                    LastName = col["LastName"] == DBNull.Value ? String.Empty : Convert.ToString(col["LastName"]),
                                    UserName = col["UserName"] == DBNull.Value ? String.Empty : Convert.ToString(col["UserName"]),
                                    Email1 = col["PrimaryEmailAddress"] == DBNull.Value ? String.Empty : Convert.ToString(col["PrimaryEmailAddress"]),
                                    Email2 = col["SecondaryEmailAddress"] == DBNull.Value ? String.Empty : Convert.ToString(col["SecondaryEmailAddress"]),
                                    PhoneNumber = col["PhoneNumber"] == DBNull.Value ? String.Empty : Convert.ToString(col["PhoneNumber"]),
                                    OrganizationUserID = col["OrganizationUserID"] == DBNull.Value ? 0 : Convert.ToInt32(col["OrganizationUserID"]),
                                    OrganizationID = col["OrganizationID"] == DBNull.Value ? 0 : Convert.ToInt32(col["OrganizationID"]),
                                    SSN = col["SSN"] == DBNull.Value ? String.Empty : Convert.ToString(col["SSN"]),
                                    TenantID = col["TenantID"] == DBNull.Value ? 0 : Convert.ToInt32(col["TenantID"]),
                                    TenantName = col["TenantName"] == DBNull.Value ? String.Empty : Convert.ToString(col["TenantName"]),
                                    IsFirstLogin = col["IsFirstLogin"] == DBNull.Value ? false : Convert.ToBoolean(col["IsFirstLogin"])
                                }).ToList();
                        }
                    }

                }

                //Release 175 NSC SSO
                else if (shibbolethHandlerType.ToLower() == AppConsts.SHIBBOLETH_NSC)
                {
                    SqlCommand cmd = new SqlCommand("api.usp_GetExistingUserForShibboleth_NSC", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ShibbolethUniqueID", shibbolethUniqueId);
                    cmd.Parameters.AddWithValue("@ShibbolethEmail", shibbolethEmail);
                    cmd.Parameters.AddWithValue("@IsApplicant", isApplicant);
                    cmd.Parameters.AddWithValue("@ShibbolethFirstName", ShibbolethFirstName);
                    cmd.Parameters.AddWithValue("@ShibbolethLastName", ShibbolethLastName);

                    SqlDataAdapter adp = new SqlDataAdapter();
                    adp.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    adp.Fill(ds);

                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            matchingUserSearchData = ds.Tables[0].AsEnumerable().Select(col =>
                                new ExternalLoginDataContract
                                {
                                    FirstName = col["FirstName"] == DBNull.Value ? String.Empty : Convert.ToString(col["FirstName"]),
                                    LastName = col["LastName"] == DBNull.Value ? String.Empty : Convert.ToString(col["LastName"]),
                                    UserName = col["UserName"] == DBNull.Value ? String.Empty : Convert.ToString(col["UserName"]),
                                    Email1 = col["PrimaryEmailAddress"] == DBNull.Value ? String.Empty : Convert.ToString(col["PrimaryEmailAddress"]),
                                    Email2 = col["SecondaryEmailAddress"] == DBNull.Value ? String.Empty : Convert.ToString(col["SecondaryEmailAddress"]),
                                    PhoneNumber = col["PhoneNumber"] == DBNull.Value ? String.Empty : Convert.ToString(col["PhoneNumber"]),
                                    OrganizationUserID = col["OrganizationUserID"] == DBNull.Value ? 0 : Convert.ToInt32(col["OrganizationUserID"]),
                                    OrganizationID = col["OrganizationID"] == DBNull.Value ? 0 : Convert.ToInt32(col["OrganizationID"]),
                                    SSN = col["SSN"] == DBNull.Value ? String.Empty : Convert.ToString(col["SSN"]),
                                    TenantID = col["TenantID"] == DBNull.Value ? 0 : Convert.ToInt32(col["TenantID"]),
                                    TenantName = col["TenantName"] == DBNull.Value ? String.Empty : Convert.ToString(col["TenantName"]),
                                    IsFirstLogin = col["IsFirstLogin"] == DBNull.Value ? false : Convert.ToBoolean(col["IsFirstLogin"])
                                }).ToList();
                        }
                    }

                }
                //UPENN DENTAL SSO)
                else if (shibbolethHandlerType.ToLower() == AppConsts.SHIBBOLETH_UPENN_DENTAL)
                {
                    SqlCommand cmd = new SqlCommand("api.usp_GetExistingUserForShibboleth_UPENN_DENTAL", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ShibbolethUniqueID", shibbolethUniqueId);
                    cmd.Parameters.AddWithValue("@IsApplicant", isApplicant);
                    cmd.Parameters.AddWithValue("@ShibbolethFirstName", ShibbolethFirstName);
                    cmd.Parameters.AddWithValue("@ShibbolethLastName", ShibbolethLastName);

                    SqlDataAdapter adp = new SqlDataAdapter();
                    adp.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    adp.Fill(ds);

                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            matchingUserSearchData = ds.Tables[0].AsEnumerable().Select(col =>
                                new ExternalLoginDataContract
                                {
                                    FirstName = col["FirstName"] == DBNull.Value ? String.Empty : Convert.ToString(col["FirstName"]),
                                    LastName = col["LastName"] == DBNull.Value ? String.Empty : Convert.ToString(col["LastName"]),
                                    UserName = col["UserName"] == DBNull.Value ? String.Empty : Convert.ToString(col["UserName"]),
                                    Email1 = col["PrimaryEmailAddress"] == DBNull.Value ? String.Empty : Convert.ToString(col["PrimaryEmailAddress"]),
                                    Email2 = col["SecondaryEmailAddress"] == DBNull.Value ? String.Empty : Convert.ToString(col["SecondaryEmailAddress"]),
                                    PhoneNumber = col["PhoneNumber"] == DBNull.Value ? String.Empty : Convert.ToString(col["PhoneNumber"]),
                                    OrganizationUserID = col["OrganizationUserID"] == DBNull.Value ? 0 : Convert.ToInt32(col["OrganizationUserID"]),
                                    OrganizationID = col["OrganizationID"] == DBNull.Value ? 0 : Convert.ToInt32(col["OrganizationID"]),
                                    SSN = col["SSN"] == DBNull.Value ? String.Empty : Convert.ToString(col["SSN"]),
                                    TenantID = col["TenantID"] == DBNull.Value ? 0 : Convert.ToInt32(col["TenantID"]),
                                    TenantName = col["TenantName"] == DBNull.Value ? String.Empty : Convert.ToString(col["TenantName"]),
                                    IsFirstLogin = col["IsFirstLogin"] == DBNull.Value ? false : Convert.ToBoolean(col["IsFirstLogin"])
                                }).ToList();
                        }
                    }

                }

                else if(shibbolethHandlerType.ToLower() == AppConsts.SHIBBOLETH_BALL_STATE)
                {
                    SqlCommand cmd = new SqlCommand("api.usp_GetExistingUserForShibboleth_BSU", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ShibbolethUniqueID", shibbolethUniqueId);
                    cmd.Parameters.AddWithValue("@ShibbolethEmail", shibbolethEmail);
                    cmd.Parameters.AddWithValue("@IsApplicant", isApplicant);
                    cmd.Parameters.AddWithValue("@ShibbolethFirstName", ShibbolethFirstName);
                    cmd.Parameters.AddWithValue("@ShibbolethLastName", ShibbolethLastName);

                    SqlDataAdapter adp = new SqlDataAdapter();
                    adp.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    adp.Fill(ds);

                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            matchingUserSearchData = ds.Tables[0].AsEnumerable().Select(col =>
                                new ExternalLoginDataContract
                                {
                                    FirstName = col["FirstName"] == DBNull.Value ? String.Empty : Convert.ToString(col["FirstName"]),
                                    LastName = col["LastName"] == DBNull.Value ? String.Empty : Convert.ToString(col["LastName"]),
                                    UserName = col["UserName"] == DBNull.Value ? String.Empty : Convert.ToString(col["UserName"]),
                                    Email1 = col["PrimaryEmailAddress"] == DBNull.Value ? String.Empty : Convert.ToString(col["PrimaryEmailAddress"]),
                                    Email2 = col["SecondaryEmailAddress"] == DBNull.Value ? String.Empty : Convert.ToString(col["SecondaryEmailAddress"]),
                                    PhoneNumber = col["PhoneNumber"] == DBNull.Value ? String.Empty : Convert.ToString(col["PhoneNumber"]),
                                    OrganizationUserID = col["OrganizationUserID"] == DBNull.Value ? 0 : Convert.ToInt32(col["OrganizationUserID"]),
                                    OrganizationID = col["OrganizationID"] == DBNull.Value ? 0 : Convert.ToInt32(col["OrganizationID"]),
                                    SSN = col["SSN"] == DBNull.Value ? String.Empty : Convert.ToString(col["SSN"]),
                                    TenantID = col["TenantID"] == DBNull.Value ? 0 : Convert.ToInt32(col["TenantID"]),
                                    TenantName = col["TenantName"] == DBNull.Value ? String.Empty : Convert.ToString(col["TenantName"]),
                                    IsFirstLogin = col["IsFirstLogin"] == DBNull.Value ? false : Convert.ToBoolean(col["IsFirstLogin"])
                                }).ToList();
                        }
                    }
                }
            }
            return matchingUserSearchData;
        }

        Boolean ISecurityRepository.GetShibbolethSettingForHistoryLogging()
        {
            String shibbolethHistorySetting = _dbNavigation.AppConfigurations.Where(con => con.AC_Key == AppConsts.IS_SHIBBOLETH_HISTORY_TO_BE_LOGGED).Select(sel => sel.AC_Value).FirstOrDefault();
            if (shibbolethHistorySetting.Trim() == "1")
            {
                return true;
            }
            return false;
        }

        Boolean ISecurityRepository.ShibbolethPeopleSoftIDEntryInIntegrationClientOrganizationUserMap(Int32 organizationUserId, Int32 integrationClientId, String externalId)
        {
            IntegrationClientOrganizationUserMap IntClientOrgUserMap = new IntegrationClientOrganizationUserMap();
            IntClientOrgUserMap = _dbNavigation.IntegrationClientOrganizationUserMaps.Where(con => con.ICOUM_ExternalID == externalId && !con.ICOUM_IsDeleted).FirstOrDefault();
            if (IntClientOrgUserMap.IsNullOrEmpty())
            {
                IntClientOrgUserMap = new IntegrationClientOrganizationUserMap();
                IntClientOrgUserMap.ICOUM_IntegrationClientID = integrationClientId;
                IntClientOrgUserMap.ICOUM_OrgUserID = organizationUserId;
                IntClientOrgUserMap.ICOUM_ExternalID = externalId;
                IntClientOrgUserMap.ICOUM_CreatedByID = organizationUserId;
                IntClientOrgUserMap.ICOUM_CreatedOn = DateTime.Now;
                IntClientOrgUserMap.ICOUM_IsDeleted = false;
                _dbNavigation.IntegrationClientOrganizationUserMaps.AddObject(IntClientOrgUserMap);
                var result = _dbNavigation.SaveChanges();
                if (result > 0)
                    return true;
                else
                    return false;
            }
            return false;
        }

        #endregion

        #region UAT-2696
        /// <summary>
        /// In this method we are getting Column configuration setting corresponding to 
        /// </summary>
        /// <param name="OrganizationUserId"></param>
        /// <param name="UniqueName"></param>
        /// <param name="GrdCode"></param>
        /// <returns></returns>
        Boolean ISecurityRepository.IsCustomAttributeChecked(Int32 OrganizationUserId, String UniqueName, String GrdCode)
        {
            return (from S in _dbNavigation.Screens
                    join SC in _dbNavigation.ScreenColumns on S.SR_ID equals SC.SC_ScreenID
                    join USCM in _dbNavigation.UserScreenColumnMappings
                        on SC.SC_ID equals USCM.USCM_ScreenColumnsID
                    where (SC.SC_UniqueName == UniqueName) && (S.SR_Code == GrdCode)
                    select USCM.USCM_IsVisible).FirstOrDefault();
        }

        #endregion

        #region UAT-2842
        OrganizationUser ISecurityRepository.GetOrganizationUserOfAdminOrder(Guid UserID)
        {
            return _dbNavigation.OrganizationUsers.Where(cond => cond.UserID == UserID && cond.IsDeleted).FirstOrDefault();
        }
        Boolean ISecurityRepository.DeleteAdminOrganizationUser(OrganizationUser OrgsUser, Int32 currentLoggedInUserId)
        {
            OrgsUser.IsDeleted = true;
            OrgsUser.ModifiedByID = currentLoggedInUserId;
            OrgsUser.ModifiedOn = DateTime.Now;

            OrgsUser.OrganizationUserProfiles.ForEach(x =>
            {
                x.IsDeleted = true;
                x.ModifiedByID = currentLoggedInUserId;
                x.ModifiedOn = DateTime.Now;
            });

            if (_dbNavigation.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }
        #endregion

        /// <summary>
        /// UAT- 2809 Check if selected user is already assigned to the selected items for review
        /// </summary>
        /// <param name="tenantXML"></param>
        /// <param name="assignToUserID"></param>
        /// <returns></returns>
        String ISecurityRepository.CheckIfUserAlreadyAssigned(String tenantXML, Int32 assignToUserID)
        {
            String ErrorMessage = String.Empty;
            EntityConnection connection = _dbNavigation.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("usp_CheckIfUserAlreadyAssigned", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ItemIDs", tenantXML);
                command.Parameters.AddWithValue("@AssignToUserID", assignToUserID);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > AppConsts.NONE && ds.Tables[0].Rows.Count > AppConsts.NONE)
                {
                    ErrorMessage = Convert.ToString(ds.Tables[0].Rows[0]["ErrorMessage"]);
                }
            }
            return ErrorMessage;
        }

        /// <summary>
        /// UAT-2958
        /// </summary>
        /// <param name="ShibbolethHandlerType"></param>
        /// <returns></returns>
        Boolean ISecurityRepository.IsRandomGeneratedPassword(String ShibbolethHandlerType)
        {
            EntityConnection connection = _dbNavigation.Connection as EntityConnection;
            Boolean IsRandomPassword = false;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("api.GetMappingByMappingGroupMappingType", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Client", ShibbolethHandlerType);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        IsRandomPassword = Convert.ToBoolean(ds.Tables[0].Rows[0]["AM_target"]);
                    }
                }
            }
            return IsRandomPassword;
        }


        #region UAT-2918
        DataTable ISecurityRepository.GetIntegrationList(Int32 organizationUserId)
        {
            DataTable integrationList = new DataTable();
            integrationList.Columns.Add("IntegrationName", typeof(String));
            //UAT 4454
            integrationList.Columns.Add("IsAdminCanRemoveLinking", typeof(Boolean));
            integrationList.Columns.Add("IntegrationClientOrganizationUserMapID", typeof(Int32));

            List<String> lstcode = new List<String>();
            lstcode.Add(AppConsts.MAPPING_GROUP_CODE_ACEMAPP);
            lstcode.Add(AppConsts.MAPPING_GROUP_CODE_MCE);
            //UAT-4492 start
            lstcode.Add(AppConsts.MAPPING_GROUP_CODE_UCONN);
            lstcode.Add(AppConsts.MAPPING_GROUP_CODE_WGU);
            lstcode.Add(AppConsts.MAPPING_GROUP_CODE_UPENN);
            lstcode.Add(AppConsts.MAPPING_GROUP_CODE_NYU);
            lstcode.Add(AppConsts.CORE_ACCOUNT_LINKING_MAPPING_GROUP_CODE);
            lstcode.Add(AppConsts.MAPPING_GROUP_CODE_INPLACE);
            lstcode.Add(AppConsts.MAPPING_GROUP_CODE_NSC);
            //Release : 181/4998
            lstcode.Add(AppConsts.MAPPING_GROUP_CODE_ROSS);
            lstcode.Add(AppConsts.MAPPING_GROUP_CODE_UPENN_DENTAL_UPPERCASE);
            lstcode.Add(AppConsts.MAPPING_GROUP_CODE_BALL_STATE);
            //UAT-4492 End
            //END Change 4454
            var result = _dbNavigation.IntegrationClientOrganizationUserMaps.Where(con => con.ICOUM_OrgUserID == organizationUserId && !con.ICOUM_IsDeleted).ToList();
            if (result.Count > AppConsts.NONE)
            {
                result.ForEach(sel => integrationList.Rows.Add(new object[] { String.Format("{0} - {1}", sel.IntegrationClient.IC_Name, sel.ICOUM_ExternalID), lstcode.Contains(sel.IntegrationClient.MappingGroup.AMG_Code.ToString().ToUpper()) ? true : false, sel.ICOUM_ID }));
            }
            else
            {
                integrationList.Rows.Add(new object[] { "No Record Found", false, AppConsts.NONE });
            }
            return integrationList;
        }
        #endregion

        #region UAT-2930

        Boolean ISecurityRepository.SaveTwoFactorAuthenticationData(String UserId, String AuthenticationTitle, String AuthenticationCode, Int32 CurrentLoggedInUserID, String AuthenticationModeCode)
        {
            Int32 authenticationModeId = _dbNavigation.lkpAuthenticationModes.Where(con => con.LAM_Code == AuthenticationModeCode && !con.LAM_IsDeleted).Select(sel => sel.LAM_ID).FirstOrDefault();

            Guid UTFA_userId = new Guid(UserId);
            UserTwoFactorAuthentication obj = _dbNavigation.UserTwoFactorAuthentications.Where(con => con.UTFA_UserID == UTFA_userId && !con.UTFA_IsDeleted && con.UTFA_AuthenticationModeID == authenticationModeId).FirstOrDefault();
            if (obj.IsNullOrEmpty())
            {
                UserTwoFactorAuthentication userTwoFactorAuthentication = new UserTwoFactorAuthentication();
                userTwoFactorAuthentication.UTFA_UserID = new Guid(UserId);
                userTwoFactorAuthentication.UTFA_AuthenticationData = AuthenticationCode;
                userTwoFactorAuthentication.UTFA_AccountTitle = AuthenticationTitle;
                userTwoFactorAuthentication.UTFA_IsVerified = true;
                userTwoFactorAuthentication.UTFA_IsDeleted = false;
                userTwoFactorAuthentication.UTFA_CreatedOn = DateTime.Now;
                userTwoFactorAuthentication.UTFA_CreatedBy = CurrentLoggedInUserID;
                userTwoFactorAuthentication.UTFA_AuthenticationModeID = authenticationModeId;

                _dbNavigation.UserTwoFactorAuthentications.AddObject(userTwoFactorAuthentication);
                if (_dbNavigation.SaveChanges() > AppConsts.NONE)
                {
                    if (_dbNavigation.OrganizationUsers.Any(con => con.UserID == UTFA_userId && con.IsApplicant == true && !con.IsDeleted))
                    {
                        return true;
                    }
                    else
                    {
                        String useTypeCode = lkpAuthenticationUseTypes.Application_Login.GetStringValue();
                        Int32 useTypeId = _dbNavigation.lkpAuthenticationUseTypes.Where(con => con.LAUT_Code == useTypeCode && !con.LAUT_IsDeleted).Select(sel => sel.LAUT_ID).FirstOrDefault();

                        UserAuthenticationUseType userAuthenticationUseType = new UserAuthenticationUseType();
                        userAuthenticationUseType.UAUT_UserID = UTFA_userId;
                        userAuthenticationUseType.UAUT_UseTypeID = useTypeId;
                        userAuthenticationUseType.UAUT_IsDeleted = false;
                        userAuthenticationUseType.UAUT_CreatedByID = CurrentLoggedInUserID;
                        userAuthenticationUseType.UAUT_CreatedOn = DateTime.Now;
                        userAuthenticationUseType.UAUT_UserTwoFactorAuthenticationID = userTwoFactorAuthentication.UTFA_ID;
                        _dbNavigation.UserAuthenticationUseTypes.AddObject(userAuthenticationUseType);
                        _dbNavigation.SaveChanges();
                        return true;
                    }
                }
            }
            return true;
        }

        UserTwoFactorAuthentication ISecurityRepository.GetTwofactorAuthenticationForUserID(String userId)
        {
            Guid UTFA_userId = new Guid(userId);

            String authenticationType = AuthenticationMode.Google_Authenticator.GetStringValue();
            Int32 authenticationModeId = _dbNavigation.lkpAuthenticationModes.Where(con => con.LAM_Code == authenticationType && !con.LAM_IsDeleted).Select(sel => sel.LAM_ID).FirstOrDefault();

            return _dbNavigation.UserTwoFactorAuthentications.Where(con => con.UTFA_UserID == UTFA_userId && !con.UTFA_IsDeleted && con.UTFA_AuthenticationModeID == authenticationModeId).FirstOrDefault();
        }

        Boolean ISecurityRepository.VerifyTwofactorAuthenticationForUserID(String UserId, Int32 CurrentLoggedInUserID)
        {
            Guid UTFA_userId = new Guid(UserId);
            UserTwoFactorAuthentication userTwoFactorAuthentication = _dbNavigation.UserTwoFactorAuthentications.Where(con => con.UTFA_UserID == UTFA_userId && !con.UTFA_IsDeleted).FirstOrDefault();
            if (!userTwoFactorAuthentication.IsNullOrEmpty())
            {
                userTwoFactorAuthentication.UTFA_IsVerified = true;
                userTwoFactorAuthentication.UTFA_ModifiedOn = DateTime.Now;
                userTwoFactorAuthentication.UTFA_ModifiedBy = CurrentLoggedInUserID;
            }
            if (_dbNavigation.SaveChanges() > AppConsts.NONE)
            {
                return true;
            }
            return false;

        }

        Boolean ISecurityRepository.DeleteTwofactorAuthenticationForUserID(String UserId, Int32 CurrentLoggedInUserID)
        {
            Guid UTFA_userId = new Guid(UserId);
            String authenticationType = AuthenticationMode.Google_Authenticator.GetStringValue();
            Int32 authenticationModeId = _dbNavigation.lkpAuthenticationModes.Where(con => con.LAM_Code == authenticationType && !con.LAM_IsDeleted).Select(sel => sel.LAM_ID).FirstOrDefault();


            UserTwoFactorAuthentication userTwoFactorAuthentication = _dbNavigation.UserTwoFactorAuthentications.Where(con => con.UTFA_UserID == UTFA_userId && !con.UTFA_IsDeleted
                                                                                                                && con.UTFA_AuthenticationModeID == authenticationModeId).FirstOrDefault();
            if (!userTwoFactorAuthentication.IsNullOrEmpty())
            {
                userTwoFactorAuthentication.UTFA_IsDeleted = true;
                userTwoFactorAuthentication.UTFA_ModifiedOn = DateTime.Now;
                userTwoFactorAuthentication.UTFA_ModifiedBy = CurrentLoggedInUserID;
            }
            if (_dbNavigation.SaveChanges() > AppConsts.NONE)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Retrieves a list of all active tenants of a user.
        /// </summary>
        /// <returns>
        /// A <see cref="Tenant"/> list of data from the underlying data storage.
        /// </returns>
        IQueryable<Tenant> ISecurityRepository.GetUserTenantsForAlltypesOfUsers(String currentOrgUserId)
        {
            return CompiledGetUserTenantsForAllTypeofUsers(_dbNavigation, currentOrgUserId.ToUpper()).OrderBy(o => o.TenantName);
        }

        #endregion
        #region UAT-2310:- Tracking Assignment Efficiencies

        Boolean ISecurityRepository.SaveAdminsConfig(List<TrackingAssignmentConfigurationContract> lstAdminsConfig, Int32 currentLoggedInUserId)
        {
            if (!lstAdminsConfig.IsNullOrEmpty())
            {
                foreach (var item in lstAdminsConfig)
                {
                    TrackingAssignmentConfiguration trackingConfiguration = new TrackingAssignmentConfiguration();
                    trackingConfiguration.TAC_OrganizationUserID = item.AdminId;
                    trackingConfiguration.TAC_AssignmentCount = item.AssignmentCount;
                    // Commented FOR UAT-3075  trackingConfiguration.TAC_SubmissionStartDate = item.DateFrom; 
                    // Commented FOR UAT-3075 trackingConfiguration.TAC_SubmissionEndDate = item.DateTo;
                    trackingConfiguration.TAC_CreatedOn = DateTime.Now;
                    trackingConfiguration.TAC_CreatedBy = currentLoggedInUserId;
                    //UAT-3075
                    trackingConfiguration.TAC_DaysFrom = item.DaysFrom;
                    trackingConfiguration.TAC_DaysTo = item.DaysTo;

                    foreach (var objMappingItem in item.lstConfigObjMapping)
                    {
                        TrackingConfigurationObjectMapping trackingConfObjMapping = new TrackingConfigurationObjectMapping();
                        trackingConfObjMapping.TCOM_ComplianceObjectID = objMappingItem.TCOM_ComplianceObjectID;
                        trackingConfObjMapping.TCOM_Priority = objMappingItem.TCOM_Priority;
                        trackingConfObjMapping.TCOM_CreatedBy = currentLoggedInUserId;
                        trackingConfObjMapping.TCOM_CreatedOn = DateTime.Now;
                        trackingConfiguration.TrackingConfigurationObjectMappings.Add(trackingConfObjMapping);
                    }
                    //End UAT-3075
                    _dbNavigation.TrackingAssignmentConfigurations.AddObject(trackingConfiguration);
                }
            }

            if (_dbNavigation.SaveChanges() > AppConsts.NONE)
            {
                return true;
            }
            return false;
        }

        List<TrackingAssignmentConfigurationContract> ISecurityRepository.GetAdminTrackingAssignmentConfiguration()
        {
            try
            {
                List<TrackingAssignmentConfigurationContract> lstAdminsConfigurations = (from tac in _dbNavigation.TrackingAssignmentConfigurations
                                                                                         join ou in _dbNavigation.OrganizationUsers on tac.TAC_OrganizationUserID equals ou.OrganizationUserID
                                                                                         where tac.TAC_IsDeleted == false && ou.IsDeleted == false
                                                                                         select
                                                                                         new TrackingAssignmentConfigurationContract()
                                                                                         {
                                                                                             TAC_ID = tac.TAC_ID,
                                                                                             AdminId = tac.TAC_OrganizationUserID,
                                                                                             AdminFirstName = ou.FirstName,
                                                                                             AdminLastName = ou.LastName,
                                                                                             AssignmentCount = tac.TAC_AssignmentCount,
                                                                                             // DateFrom = tac.TAC_SubmissionStartDate,
                                                                                             // DateTo = tac.TAC_SubmissionEndDate,
                                                                                             DaysFrom = tac.TAC_DaysFrom,
                                                                                             DaysTo = tac.TAC_DaysTo,
                                                                                             lstConfigObjMapping = (from tcom in _dbNavigation.TrackingConfigurationObjectMappings
                                                                                                                    join cpo in _dbNavigation.CompliancePriorityObjects on tcom.TCOM_ComplianceObjectID equals cpo.CPO_ID
                                                                                                                    where cpo.CPO_IsDeleted == false && tcom.TCOM_IsDeleted == false
                                                                                                                    && tcom.TCOM_ConfigurationID == tac.TAC_ID
                                                                                                                    select
                                                                                                                    new TrackingConfigObjectMappingContract()
                                                                                                                    {
                                                                                                                        ObjectName = cpo.CPO_Name,
                                                                                                                        TCOM_ComplianceObjectID = cpo.CPO_ID
                                                                                                                    }).ToList()
                                                                                         }).ToList();
                return lstAdminsConfigurations;
            }
            catch (SysXException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw (new SysXException(ex.Message, ex));
            }
        }

        Boolean ISecurityRepository.UpdateConfiguration(TrackingAssignmentConfigurationContract trackingConfigurationContract, Int32 currentLoggedInUserId, List<TrackingConfigObjectMappingContract> lstTrackObjMappingToDelete)
        {
            try
            {
                TrackingAssignmentConfiguration updateTrackingConfig = _dbNavigation.TrackingAssignmentConfigurations.Where(cond => cond.TAC_ID == trackingConfigurationContract.TAC_ID && !cond.TAC_IsDeleted).FirstOrDefault();
                //UAT-3075
                List<TrackingConfigurationObjectMapping> lstUpdateTrackConfObjMapping = _dbNavigation.TrackingConfigurationObjectMappings.Where(cond => cond.TCOM_ConfigurationID == trackingConfigurationContract.TAC_ID && !cond.TCOM_IsDeleted).ToList();
                if (!updateTrackingConfig.IsNullOrEmpty())
                {
                    updateTrackingConfig.TAC_AssignmentCount = trackingConfigurationContract.AssignmentCount;
                    //Commente for UAT-3075 updateTrackingConfig.TAC_SubmissionStartDate = trackingConfigurationContract.DateFrom;
                    //Commente for UAT-3075 updateTrackingConfig.TAC_SubmissionEndDate = trackingConfigurationContract.DateTo;
                    updateTrackingConfig.TAC_ModifiedBy = currentLoggedInUserId;
                    updateTrackingConfig.TAC_ModifiedOn = DateTime.Now;
                    //UAT-3075 
                    updateTrackingConfig.TAC_DaysFrom = trackingConfigurationContract.DaysFrom;
                    updateTrackingConfig.TAC_DaysTo = trackingConfigurationContract.DaysTo;
                    foreach (var item in trackingConfigurationContract.lstConfigObjMapping)
                    {
                        if (item.TCOM_ID > AppConsts.NONE)
                        {
                            //Update mapping
                            TrackingConfigurationObjectMapping trackingConfigObjMapping = lstUpdateTrackConfObjMapping.Where(cond => cond.TCOM_ID == item.TCOM_ID && !cond.TCOM_IsDeleted).FirstOrDefault();
                            trackingConfigObjMapping.TCOM_Priority = item.TCOM_Priority;
                            trackingConfigObjMapping.TCOM_ModifiedBy = currentLoggedInUserId;
                            trackingConfigObjMapping.TCOM_ModifiedOn = DateTime.Now;
                        }
                        else
                        {
                            //Save new mapping
                            TrackingConfigurationObjectMapping trackingConfObjMapping = new TrackingConfigurationObjectMapping();
                            trackingConfObjMapping.TCOM_ConfigurationID = trackingConfigurationContract.TAC_ID;
                            trackingConfObjMapping.TCOM_ComplianceObjectID = item.TCOM_ComplianceObjectID;
                            trackingConfObjMapping.TCOM_Priority = item.TCOM_Priority;
                            trackingConfObjMapping.TCOM_CreatedBy = currentLoggedInUserId;
                            trackingConfObjMapping.TCOM_CreatedOn = DateTime.Now;
                            updateTrackingConfig.TrackingConfigurationObjectMappings.Add(trackingConfObjMapping);
                        }
                    }

                    //Delete the tracking Configuration object mappings for unchecked objects
                    foreach (TrackingConfigObjectMappingContract trackObjMappingToDelete in lstTrackObjMappingToDelete)
                    {
                        if (trackObjMappingToDelete.TCOM_ID > AppConsts.NONE)
                        {
                            //Delete Mapping
                            TrackingConfigurationObjectMapping trackingConfigObjMappingtoDelete = lstUpdateTrackConfObjMapping.Where(cond => cond.TCOM_ID == trackObjMappingToDelete.TCOM_ID && !cond.TCOM_IsDeleted).FirstOrDefault();
                            trackingConfigObjMappingtoDelete.TCOM_IsDeleted = true;
                            trackingConfigObjMappingtoDelete.TCOM_ModifiedBy = currentLoggedInUserId;
                            trackingConfigObjMappingtoDelete.TCOM_ModifiedOn = DateTime.Now;
                        }
                    }

                }
                if (_dbNavigation.SaveChanges() > 0)
                    return true;
                return false;
            }
            catch (SysXException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw (new SysXException(ex.Message, ex));
            }
        }

        Boolean ISecurityRepository.DeleteConfiguration(Int32 TAC_ID, Int32 currentLoggedInUserId)
        {
            try
            {
                TrackingAssignmentConfiguration trackingConfiguration = _dbNavigation.TrackingAssignmentConfigurations.Where(cond => cond.TAC_ID == TAC_ID).FirstOrDefault();

                if (!trackingConfiguration.IsNullOrEmpty())
                {
                    trackingConfiguration.TAC_IsDeleted = true;
                    trackingConfiguration.TAC_ModifiedBy = currentLoggedInUserId;
                    trackingConfiguration.TAC_ModifiedOn = DateTime.Now;
                }

                //UAT-3075
                List<TrackingConfigurationObjectMapping> lstTrackConfObjectMapping = _dbNavigation.TrackingConfigurationObjectMappings.Where(cond => cond.TCOM_ConfigurationID == TAC_ID).ToList();
                if (!lstTrackConfObjectMapping.IsNullOrEmpty())
                {
                    foreach (TrackingConfigurationObjectMapping trackConfObjectMapping in lstTrackConfObjectMapping)
                    {
                        trackConfObjectMapping.TCOM_IsDeleted = true;
                        trackConfObjectMapping.TCOM_ModifiedBy = currentLoggedInUserId;
                        trackConfObjectMapping.TCOM_ModifiedOn = DateTime.Now;
                    }
                }
                //END UAT-3075

                if (_dbNavigation.SaveChanges() > 0)
                    return true;
                return false;
            }
            catch (SysXException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion


        #region UAT-3052
        /// <summary>
        /// Used Only for Reports
        /// </summary>
        /// <param name="tenantID"></param>
        /// <param name="loggedInEmailId"></param>
        /// <returns></returns>
        Dictionary<String, String> ISecurityRepository.GetItemListForReportsByTenantIdLoggedInEmail(String tenantID, String loggedInEmailId)
        {
            Dictionary<String, String> dicCategoryList = new Dictionary<String, String>();
            EntityConnection connection = _dbNavigation.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("Report.usp_Report_Filter_ComplianceAndRotationItems", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@LoggedInUserEmailId", loggedInEmailId);
                command.Parameters.AddWithValue("@TenantIDs", tenantID);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > AppConsts.NONE && ds.Tables[0].Rows.Count > AppConsts.NONE)
                {
                    dicCategoryList = ds.Tables[0].AsEnumerable()
                                   .ToDictionary<DataRow, String, String>(row => row.Field<String>("ItemID").Replace(",", "~"),
                                       row => row.Field<String>("ItemName"));
                }
            }
            return dicCategoryList;
        }

        /// <summary>
        /// Used Only for Reports
        /// </summary>
        /// <param name="tenantID"></param>
        /// <param name="loggedInEmailId"></param>
        /// <returns></returns>
        List<Entity.ClientEntity.Tenant> ISecurityRepository.GetTenantsByTenantId(String tenantIDs)
        {
            List<Entity.ClientEntity.Tenant> lstTenants = new List<Entity.ClientEntity.Tenant>();
            EntityConnection connection = _dbNavigation.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("Report.usp_Report_Filter_Multi_InstituteList", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TenantIds", tenantIDs);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        lstTenants = ds.Tables[0].AsEnumerable().Select(col =>
                            new Entity.ClientEntity.Tenant
                            {
                                TenantID = Convert.ToInt32(col["TenantID"]),
                                TenantName = col["TenantName"] == DBNull.Value ? String.Empty : Convert.ToString(col["TenantName"]),
                            }).ToList();
                    }
                }
            }
            return lstTenants;
        }
        #endregion

        /// <summary>        
        /// </summary>
        /// <param name="tenantID"></param>
        /// <param name="loggedInEmailId"></param>
        /// <returns></returns>
        Dictionary<String, String> ISecurityRepository.GetAllItemListForReportsByTenantIdLoggedInEmail(String tenantID, String loggedInEmailId)
        {
            Dictionary<String, String> dicCategoryList = new Dictionary<String, String>();
            EntityConnection connection = _dbNavigation.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("Report.usp_Report_Filter_ComplianceAndRotationItems_ForAppAndInstr", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@LoggedInUserEmailId", loggedInEmailId);
                command.Parameters.AddWithValue("@TenantIDs", tenantID);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > AppConsts.NONE && ds.Tables[0].Rows.Count > AppConsts.NONE)
                {
                    dicCategoryList = ds.Tables[0].AsEnumerable()
                                   .ToDictionary<DataRow, String, String>(row => row.Field<String>("ItemID").Replace(",", "~"),
                                       row => row.Field<String>("ItemName"));
                }
            }
            return dicCategoryList;
        }

        #region UAT-3068
        String ISecurityRepository.GetUserAuthenticationUseTypeForUserID(String UserId)
        {
            Guid userId = new Guid(UserId);
            String authModeCode = String.Empty;
            UserAuthenticationUseType userAuthenticationUseType = _dbNavigation.UserAuthenticationUseTypes.Where(con => con.UAUT_UserID == userId && !con.UAUT_IsDeleted && !con.UserTwoFactorAuthentication.UTFA_IsDeleted).FirstOrDefault();
            if (!userAuthenticationUseType.IsNullOrEmpty() && !userAuthenticationUseType.UserTwoFactorAuthentication.IsNullOrEmpty())
            {

                authModeCode = userAuthenticationUseType.UserTwoFactorAuthentication.lkpAuthenticationMode.LAM_Code;
            }
            return authModeCode;
        }

        Boolean ISecurityRepository.SaveAuthenticationData(String userID, String authenticationType, Int32 currentUserId)
        {
            Guid userId = new Guid(userID);
            String useTypeCode = lkpAuthenticationUseTypes.Application_Login.GetStringValue();
            Int32 useTypeId = _dbNavigation.lkpAuthenticationUseTypes.Where(con => con.LAUT_Code == useTypeCode && !con.LAUT_IsDeleted).Select(sel => sel.LAUT_ID).FirstOrDefault();

            if (!authenticationType.IsNullOrEmpty() && authenticationType != AuthenticationMode.None.GetStringValue())
            {
                Int32 authenticationModeId = _dbNavigation.lkpAuthenticationModes.Where(con => con.LAM_Code == authenticationType && !con.LAM_IsDeleted).Select(sel => sel.LAM_ID).FirstOrDefault();

                UserTwoFactorAuthentication userTwoFactorAuthentication = _dbNavigation.UserTwoFactorAuthentications.Where(con => con.UTFA_UserID == userId && !con.UTFA_IsDeleted
                    && con.UTFA_AuthenticationModeID == authenticationModeId).FirstOrDefault();

                Int32 userTwoFactorAuthId = AppConsts.NONE;
                if (userTwoFactorAuthentication.IsNullOrEmpty())
                {
                    UserTwoFactorAuthentication userTwofactorAuthToSet = new UserTwoFactorAuthentication();
                    userTwofactorAuthToSet.UTFA_UserID = userId;
                    userTwofactorAuthToSet.UTFA_IsVerified = true;
                    userTwofactorAuthToSet.UTFA_IsDeleted = false;
                    userTwofactorAuthToSet.UTFA_CreatedOn = DateTime.Now;
                    userTwofactorAuthToSet.UTFA_CreatedBy = currentUserId;
                    userTwofactorAuthToSet.UTFA_AuthenticationModeID = authenticationModeId;

                    _dbNavigation.UserTwoFactorAuthentications.AddObject(userTwofactorAuthToSet);
                    if (_dbNavigation.SaveChanges() > AppConsts.NONE)
                    {
                        userTwoFactorAuthId = userTwofactorAuthToSet.UTFA_ID;
                    }
                }
                else
                {
                    userTwoFactorAuthId = userTwoFactorAuthentication.UTFA_ID;
                }

                //check for existing userAuthenticationUsetype
                UserAuthenticationUseType userAuthenticationUseType = _dbNavigation.UserAuthenticationUseTypes.Where(con => con.UAUT_UserID == userId && !con.UAUT_IsDeleted).FirstOrDefault();
                if (!userAuthenticationUseType.IsNullOrEmpty())
                {
                    //update UserTwoFactorAuthenticationID in UserAuthenticationUseType Table 
                    userAuthenticationUseType.UAUT_UserTwoFactorAuthenticationID = userTwoFactorAuthId;
                    userAuthenticationUseType.UAUT_ModifiedByID = currentUserId;
                    userAuthenticationUseType.UAUT_ModifiedOn = DateTime.Now;
                }
                else
                {
                    //Add new row in UserAuthenticationUseType
                    UserAuthenticationUseType UserAuthenticationUseType = new UserAuthenticationUseType();
                    UserAuthenticationUseType.UAUT_UserID = userId;
                    UserAuthenticationUseType.UAUT_UseTypeID = useTypeId;
                    UserAuthenticationUseType.UAUT_IsDeleted = false;
                    UserAuthenticationUseType.UAUT_CreatedByID = currentUserId;
                    UserAuthenticationUseType.UAUT_CreatedOn = DateTime.Now;
                    UserAuthenticationUseType.UAUT_UserTwoFactorAuthenticationID = userTwoFactorAuthId;
                    _dbNavigation.UserAuthenticationUseTypes.AddObject(UserAuthenticationUseType);
                }
            }
            else
            {
                //if user selects none for login authentication type then we will delete entry in userAuthenticationUseType Table.
                UserAuthenticationUseType userAuthenticationUseType = _dbNavigation.UserAuthenticationUseTypes.Where(con => con.UAUT_UserID == userId && !con.UAUT_IsDeleted).FirstOrDefault();
                if (!userAuthenticationUseType.IsNullOrEmpty())
                {
                    userAuthenticationUseType.UAUT_IsDeleted = true;
                    userAuthenticationUseType.UAUT_ModifiedByID = currentUserId;
                    userAuthenticationUseType.UAUT_ModifiedOn = DateTime.Now;
                }
            }

            if (_dbNavigation.SaveChanges() > AppConsts.NONE)
            {
                return true;
            }
            return false;


        }

        String ISecurityRepository.GetUserIdFromOrgUserId(Int32 currentUserId)
        {
            Guid userId = _dbNavigation.OrganizationUsers.Where(con => con.OrganizationUserID == currentUserId).Select(sel => sel.UserID).FirstOrDefault();
            return Convert.ToString(userId);
        }
        #endregion

        #region UAT-3032:- Sticky "Institution Selection" for ADB admins.

        List<RoleConfigurationContract> ISecurityRepository.GetRolesSetting()
        {
            try
            {
                EntityConnection connection = _dbNavigation.Connection as EntityConnection;
                List<RoleConfigurationContract> lstRoleSettings = new List<RoleConfigurationContract>();
                using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("usp_GetRoleSettings", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter adp = new SqlDataAdapter();
                    adp.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    adp.Fill(ds);
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            lstRoleSettings = ds.Tables[0].AsEnumerable().Select(col =>
                         new RoleConfigurationContract
                         {
                             RPTS_ID = col["RPTS_ID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["RPTS_ID"]),
                             RPTS_RoleID = col["RoleDetailID"] == DBNull.Value ? String.Empty : Convert.ToString(col["RoleDetailID"]),
                             RoleDescription = col["RoleDescription"] == DBNull.Value ? String.Empty : Convert.ToString(col["RoleDescription"]),
                             RoleName = col["RoleName"] == DBNull.Value ? String.Empty : Convert.ToString(col["RoleName"]),
                             RPTS_IsAllowPreferredTenant = col["RPTS_IsAllowPreferredTenant"] == DBNull.Value ? false : Convert.ToBoolean(col["RPTS_IsAllowPreferredTenant"]),
                             RPTS_IsAllowDataEntry = col["RPTS_IsAllowDataEntry"] == DBNull.Value ? false : Convert.ToBoolean(col["RPTS_IsAllowDataEntry"]),
                             RPTS_IsAllowComplianceVerfication = col["RPTS_IsAllowComplianceVerfication"] == DBNull.Value ? false : Convert.ToBoolean(col["RPTS_IsAllowComplianceVerfication"]),
                             RPTS_IsAllowRotationVerfication = col["RPTS_IsAllowRotationVerfication"] == DBNull.Value ? false : Convert.ToBoolean(col["RPTS_IsAllowRotationVerfication"]),
                             RPTS_IsAllowLocationEnroller = col["RPTS_IsAllowLocationEnroller"] == DBNull.Value ? false : Convert.ToBoolean(col["RPTS_IsAllowLocationEnroller"]),

                         }).ToList();
                        }
                    }
                }

                return lstRoleSettings;
            }
            catch (SysXException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw (new SysXException(ex.Message, ex));
            }
        }

        Boolean ISecurityRepository.SaveRolePreferredTenantSetting(RoleConfigurationContract roleSettingContract, Int32 currentLoggedInUserId)
        {
            try
            {
                //Update role setting//
                if (roleSettingContract.RPTS_ID > AppConsts.NONE)
                {
                    aspnet_RolePreferredTenantSetting rolePreferredTenantSetting = _dbNavigation.aspnet_RolePreferredTenantSetting.Where(cond => cond.RPTS_ID == roleSettingContract.RPTS_ID).FirstOrDefault();
                    if (!rolePreferredTenantSetting.IsNullOrEmpty())
                    {
                        rolePreferredTenantSetting.RPTS_IsAllowPreferredTenant = roleSettingContract.RPTS_IsAllowPreferredTenant;
                        rolePreferredTenantSetting.RPTS_IsAllowDataEntry = roleSettingContract.RPTS_IsAllowDataEntry;
                        rolePreferredTenantSetting.RPTS_IsAllowComplianceVerfication = roleSettingContract.RPTS_IsAllowComplianceVerfication;
                        rolePreferredTenantSetting.RPTS_IsAllowRotationVerfication = roleSettingContract.RPTS_IsAllowRotationVerfication;
                        rolePreferredTenantSetting.RPTS_IsAllowLocationEnroller = roleSettingContract.RPTS_IsAllowLocationEnroller;
                        rolePreferredTenantSetting.RPTS_ModifiedBy = currentLoggedInUserId;
                        rolePreferredTenantSetting.RPTS_ModifiedOn = DateTime.Now;
                    }
                }
                //Save role setting//
                else
                {
                    aspnet_RolePreferredTenantSetting rolePreferredTenantSetting = new aspnet_RolePreferredTenantSetting();
                    rolePreferredTenantSetting.RPTS_RoleID = new Guid(roleSettingContract.RPTS_RoleID);
                    rolePreferredTenantSetting.RPTS_IsAllowPreferredTenant = roleSettingContract.RPTS_IsAllowPreferredTenant;
                    rolePreferredTenantSetting.RPTS_IsAllowDataEntry = roleSettingContract.RPTS_IsAllowDataEntry;
                    rolePreferredTenantSetting.RPTS_IsAllowComplianceVerfication = roleSettingContract.RPTS_IsAllowComplianceVerfication;
                    rolePreferredTenantSetting.RPTS_IsAllowRotationVerfication = roleSettingContract.RPTS_IsAllowRotationVerfication;
                    rolePreferredTenantSetting.RPTS_IsAllowLocationEnroller = roleSettingContract.RPTS_IsAllowLocationEnroller;
                    rolePreferredTenantSetting.RPTS_CreatedBy = currentLoggedInUserId;
                    rolePreferredTenantSetting.RPTS_CreatedOn = DateTime.Now;
                    _dbNavigation.aspnet_RolePreferredTenantSetting.AddObject(rolePreferredTenantSetting);
                }

                if (_dbNavigation.SaveChanges() > 0)
                    return true;
                return false;
            }
            catch (SysXException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw (new SysXException(ex.Message, ex));
            }
        }

        Boolean ISecurityRepository.DeleteRolePreferredTenantSetting(Int32 rolePreferredTenantSettingId, Int32 currentLoggedInUserId)
        {
            try
            {
                aspnet_RolePreferredTenantSetting rolePreferredTenantSetting = _dbNavigation.aspnet_RolePreferredTenantSetting.Where(cond => cond.RPTS_ID == rolePreferredTenantSettingId).FirstOrDefault();

                rolePreferredTenantSetting.RPTS_IsDeleted = true;
                rolePreferredTenantSetting.RPTS_ModifiedBy = currentLoggedInUserId;
                rolePreferredTenantSetting.RPTS_ModifiedOn = DateTime.Now;
                if (_dbNavigation.SaveChanges() > AppConsts.NONE)
                    return true;
                return false;
            }
            catch (SysXException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw (new SysXException(ex.Message, ex));
            }
        }

        Int32 ISecurityRepository.GetPreferredSelectedTenant()
        {
            try
            {
                return 104;
            }
            catch (SysXException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw (new SysXException(ex.Message, ex));
            }
        }

        Boolean ISecurityRepository.IsUserAllowedPreferredTenant(string userID)
        {
            var User_Guid = Guid.Parse(userID);
            var lstRoles = _dbNavigation.vw_aspnet_UsersInRoles
                                .Where(cond => cond.UserId == User_Guid)
                                .Select(s => s.RoleId).ToList();


            var res = _dbNavigation.aspnet_RolePreferredTenantSetting
                            .Where(cond => !cond.RPTS_IsDeleted
                                    && cond.RPTS_IsAllowPreferredTenant == true
                                    && lstRoles.Contains(cond.RPTS_RoleID));


            if (!res.IsNullOrEmpty() && res.Count() > 0)
                return true;
            else
                return false;
        }

        #endregion

        #region UAT-3054
        Boolean ISecurityRepository.IsStudentEmailDOBLastNameExist(Int32 tenantID, String emailId, String lastName, DateTime? dob)
        {
            Int32 organizationId = 0;
            Organization organization = _dbNavigation.Organizations.Where(cond => cond.IsDeleted == false && cond.TenantID == tenantID && cond.IsActive == true).FirstOrDefault();

            if (organization.IsNotNull())
            {
                organizationId = organization.OrganizationID;
            }

            if (!emailId.IsNullOrEmpty() && !lastName.IsNullOrEmpty() && dob.HasValue)
            {
                return _dbNavigation.OrganizationUsers.Where(cond => cond.OrganizationID == organizationId && cond.PrimaryEmailAddress == emailId && cond.LastName == lastName && cond.DOB == dob.Value).Any();
            }

            return false;
        }

        String ISecurityRepository.GetUserNameByExternald(String externalId, Int32 integrationClientID)
        {
            IntegrationClientOrganizationUserMap integrationClientOrganizationUserMap = _dbNavigation.IntegrationClientOrganizationUserMaps.Where(cond => cond.ICOUM_IsDeleted == false && cond.ICOUM_IntegrationClientID == integrationClientID && cond.ICOUM_ExternalID == externalId).FirstOrDefault();

            if (integrationClientOrganizationUserMap.IsNotNull())
            {
                return integrationClientOrganizationUserMap.OrganizationUser.aspnet_Users.UserName;
            }
            return String.Empty;
        }
        #endregion

        #region UAT-3067
        Dictionary<String, String> ISecurityRepository.GetCustomAttrMappedWithShibbolethAttr(String mappingGroupCode)
        {
            Dictionary<String, String> dcCustomAttrMappedWithShibbolethAttr = new Dictionary<String, String>();

            Guid mappingGroupCodeGuid = new Guid(mappingGroupCode);
            Int32 mappingTypeID = _dbNavigation.lkpMappingType1.Where(cond => cond.AMT_Code == "AAAF" && !cond.AMT_IsDeleted).Select(sel => sel.AMT_ID).FirstOrDefault();
            if (mappingTypeID > AppConsts.NONE)
            {
                List<Int32> lstMappingGroupMappingTypeIDs = _dbNavigation.MappingGroupMappingTypes.Where(con => con.MappingGroup.AMG_Code == mappingGroupCodeGuid && !con.MappingGroup.AMG_IsDeleted && con.AMGT_MappingTypeID == mappingTypeID && !con.AMGT_IsDeleted).Select(sel => sel.AMGT_ID).ToList();
                if (!lstMappingGroupMappingTypeIDs.IsNullOrEmpty())
                {
                    List<Mapping> lstMappings = _dbNavigation.Mappings.Where(con => lstMappingGroupMappingTypeIDs.Contains(con.AM_MappingGroupMappingTypeID) && !con.AM_IsDeleted).ToList();
                    if (!lstMappings.IsNullOrEmpty())
                    {
                        lstMappings.ForEach(x =>
                        {
                            dcCustomAttrMappedWithShibbolethAttr.Add(x.AM_Source, x.AM_Target);
                        });
                    }
                }
            }
            return dcCustomAttrMappedWithShibbolethAttr;
        }
        #endregion

        #region UAT-3075
        List<CompliancePriorityObjectContract> ISecurityRepository.GetCompliancePriorityObjects()
        {
            try
            {
                List<CompliancePriorityObjectContract> lstCompPriorityObject = _dbNavigation.CompliancePriorityObjects.Where(cond => !cond.CPO_IsDeleted).Select(sel =>
                                                                                    new CompliancePriorityObjectContract()
                                                                                    {
                                                                                        CPO_ID = sel.CPO_ID,
                                                                                        CPO_Name = sel.CPO_Name,
                                                                                        CPO_Description = sel.CPO_Description
                                                                                    }).ToList();
                return lstCompPriorityObject;
            }
            catch (SysXException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw (new SysXException(ex.Message, ex));
            }
        }

        Boolean ISecurityRepository.SaveComplPriorityObject(Int32 currentLoggedInUserID, CompliancePriorityObjectContract compPriorityObject)
        {
            try
            {
                if (!compPriorityObject.CPO_ID.IsNullOrEmpty() && compPriorityObject.CPO_ID > AppConsts.NONE)
                {
                    //Update Compliance priority Object
                    CompliancePriorityObject updateCompPriorityObject = _dbNavigation.CompliancePriorityObjects.Where(cond => !cond.CPO_IsDeleted && cond.CPO_ID == compPriorityObject.CPO_ID).FirstOrDefault();
                    updateCompPriorityObject.CPO_Name = compPriorityObject.CPO_Name;
                    updateCompPriorityObject.CPO_Description = compPriorityObject.CPO_Description;
                    updateCompPriorityObject.CPO_ModifiedBy = currentLoggedInUserID;
                    updateCompPriorityObject.CPO_ModifiedOn = DateTime.Now;
                }
                else
                {
                    //Save Compliance Priority Object
                    CompliancePriorityObject saveCompPriorityObject = new CompliancePriorityObject();
                    saveCompPriorityObject.CPO_Name = compPriorityObject.CPO_Name;
                    saveCompPriorityObject.CPO_Description = compPriorityObject.CPO_Description;
                    saveCompPriorityObject.CPO_CreatedBy = currentLoggedInUserID;
                    saveCompPriorityObject.CPO_CreatedOn = DateTime.Now;

                    _dbNavigation.CompliancePriorityObjects.AddObject(saveCompPriorityObject);
                }

                if (_dbNavigation.SaveChanges() > AppConsts.NONE)
                    return true;
                return false;
            }
            catch (SysXException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw (new SysXException(ex.Message, ex));
            }
        }

        Boolean ISecurityRepository.DeleteComplPriorityObject(Int32 currentLoggedInUserID, Int32 compPriorityObjectID)
        {
            try
            {
                //Delete Compliance priority Object
                CompliancePriorityObject compPriorityObject = _dbNavigation.CompliancePriorityObjects.Where(cond => !cond.CPO_IsDeleted && cond.CPO_ID == compPriorityObjectID).FirstOrDefault();
                compPriorityObject.CPO_IsDeleted = true;
                compPriorityObject.CPO_ModifiedBy = currentLoggedInUserID;
                compPriorityObject.CPO_ModifiedOn = DateTime.Now;

                if (_dbNavigation.SaveChanges() > AppConsts.NONE)
                    return true;
                return false;
            }
            catch (SysXException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw (new SysXException(ex.Message, ex));
            }
        }

        List<TenantUserMappingContract> ISecurityRepository.GetTenantUserMappings()
        {
            try
            {
                List<TenantUserMappingContract> lstTenantUserMapping = (from tum in _dbNavigation.TenantUserMappings
                                                                        join ten in _dbNavigation.Tenants on tum.TUM_TenantID equals ten.TenantID
                                                                        join ou in _dbNavigation.OrganizationUsers on tum.TUM_OrganizationUserID equals ou.OrganizationUserID
                                                                        where tum.TUM_IsDeleted == false && ten.IsDeleted == false && ou.IsDeleted == false
                                                                        select
                                                                        new TenantUserMappingContract()
                                                                        {
                                                                            TUM_ID = tum.TUM_ID,
                                                                            TenantID = ten.TenantID,
                                                                            TenantName = ten.TenantName,
                                                                            OrganizationUserID = ou.OrganizationUserID,
                                                                            UserName = ou.FirstName + " " + ou.LastName
                                                                        }).ToList();
                return lstTenantUserMapping;
            }
            catch (SysXException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw (new SysXException(ex.Message, ex));
            }
        }

        Boolean ISecurityRepository.SaveTenantUserMapping(List<TenantUserMappingContract> lstTenantUserMappings, Int32 currentLoggedInUserId)
        {
            if (!lstTenantUserMappings.IsNullOrEmpty())
            {
                //Save Tenant User Mappings
                foreach (var item in lstTenantUserMappings)
                {
                    TenantUserMapping tenantUserMapping = new TenantUserMapping();
                    tenantUserMapping.TUM_OrganizationUserID = item.OrganizationUserID;
                    tenantUserMapping.TUM_TenantID = item.TenantID;
                    tenantUserMapping.TUM_CreatedOn = DateTime.Now;
                    tenantUserMapping.TUM_CreatedBy = currentLoggedInUserId;

                    _dbNavigation.TenantUserMappings.AddObject(tenantUserMapping);
                }
            }

            if (_dbNavigation.SaveChanges() > AppConsts.NONE)
            {
                return true;
            }
            return false;
        }

        Boolean ISecurityRepository.UpdateTenantUserMapping(Int32 currentLoggedInUserID, TenantUserMappingContract tenantUserMapping)
        {
            try
            {
                if (!tenantUserMapping.TUM_ID.IsNullOrEmpty() && tenantUserMapping.TUM_ID > AppConsts.NONE)
                {
                    //Update Tenant User Mapping
                    TenantUserMapping updateTenantUserMapping = _dbNavigation.TenantUserMappings.Where(cond => !cond.TUM_IsDeleted && cond.TUM_ID == tenantUserMapping.TUM_ID).FirstOrDefault();
                    updateTenantUserMapping.TUM_TenantID = tenantUserMapping.TenantID;
                    updateTenantUserMapping.TUM_OrganizationUserID = tenantUserMapping.OrganizationUserID;
                    updateTenantUserMapping.TUM_ModifiedBy = currentLoggedInUserID;
                    updateTenantUserMapping.TUM_ModifiedOn = DateTime.Now;
                }

                if (_dbNavigation.SaveChanges() > AppConsts.NONE)
                    return true;
                return false;
            }
            catch (SysXException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw (new SysXException(ex.Message, ex));
            }
        }

        List<Int32> ISecurityRepository.GetUsersMappedWithTenant(Int32 tenantID)
        {
            try
            {
                return _dbNavigation.TenantUserMappings.Where(cond => cond.TUM_TenantID == tenantID && !cond.TUM_IsDeleted).Select(sel => sel.TUM_OrganizationUserID).ToList();
            }
            catch (SysXException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw (new SysXException(ex.Message, ex));
            }
        }

        Boolean ISecurityRepository.DeleteTenantUserMapping(Int32 currentLoggedInUserID, Int32 tenantUserMappingId)
        {
            try
            {
                TenantUserMapping tenantUserMapping = _dbNavigation.TenantUserMappings.Where(cond => cond.TUM_ID == tenantUserMappingId).FirstOrDefault();
                if (!tenantUserMapping.IsNullOrEmpty())
                {
                    tenantUserMapping.TUM_IsDeleted = true;
                    tenantUserMapping.TUM_ModifiedBy = currentLoggedInUserID;
                    tenantUserMapping.TUM_ModifiedOn = DateTime.Now;
                }
                if (_dbNavigation.SaveChanges() > AppConsts.NONE)
                    return true;
                return false;
            }
            catch (SysXException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw (new SysXException(ex.Message, ex));
            }
        }

        //Boolean ISecurityRepository.SaveTrackingConfObjMapping(List<TrackingConfigObjectMappingContract> lstTrackingConfObjMapping, Int32 currentLoggedInUserId)
        //{
        //    try
        //    {
        //        if (!lstTrackingConfObjMapping.IsNullOrEmpty())
        //        {                    //List<TrackingConfigurationObjectMapping> lstTrackingConfigurationObjectMapping = new List<TrackingConfigurationObjectMapping>();

        //            foreach (var item in lstTrackingConfObjMapping)
        //            {
        //                TrackingConfigurationObjectMapping trackingConfObjMapping = new TrackingConfigurationObjectMapping();
        //                trackingConfObjMapping.TCOM_ConfigurationID = item.TCOM_ConfigurationID;
        //                trackingConfObjMapping.TCOM_ComplianceObjectID = item.TCOM_ComplianceObjectID;
        //                trackingConfObjMapping.TCOM_Priority = item.TCOM_Priority;
        //                trackingConfObjMapping.TCOM_CreatedOn = DateTime.Now;
        //                trackingConfObjMapping.TCOM_CreatedBy = currentLoggedInUserId;

        //                _dbNavigation.TrackingConfigurationObjectMappings.AddObject(trackingConfObjMapping);
        //            }
        //        }
        //        if (_dbNavigation.SaveChanges() > AppConsts.NONE)
        //            return true;
        //        return false;
        //    }
        //    catch (SysXException ex)
        //    {
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //}
        List<TrackingConfigObjectMappingContract> ISecurityRepository.GetTrackConfigObjectMapped(Int32 trackConfigId)
        {
            try
            {
                List<TrackingConfigObjectMappingContract> lstTrackConfigObjectsMapped = new List<TrackingConfigObjectMappingContract>();
                lstTrackConfigObjectsMapped = _dbNavigation.TrackingConfigurationObjectMappings.Where(cond => !cond.TCOM_IsDeleted && cond.TCOM_ConfigurationID == trackConfigId).Select(sel =>
                                                                new TrackingConfigObjectMappingContract()
                                                                {
                                                                    TCOM_ID = sel.TCOM_ID,
                                                                    TCOM_ComplianceObjectID = sel.TCOM_ComplianceObjectID,
                                                                    TCOM_ConfigurationID = sel.TCOM_ConfigurationID,
                                                                    TCOM_Priority = sel.TCOM_Priority
                                                                }).ToList();


                return lstTrackConfigObjectsMapped;
            }
            catch (SysXException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        //UAT-3133
        Boolean ISecurityRepository.IsIntegrationClientOrganisationUser(Int32 currentUserId, String MAPPING_GROUP_CODE_UCONN)
        {
            var lstIntegration = _dbNavigation.IntegrationClientOrganizationUserMaps.Where(cond => cond.ICOUM_OrgUserID == currentUserId && cond.IntegrationClient.MappingGroup.AMG_Code.ToString() == MAPPING_GROUP_CODE_UCONN.ToString() && !cond.ICOUM_IsDeleted && !cond.IntegrationClient.IC_IsDeleted && !cond.IntegrationClient.MappingGroup.AMG_IsDeleted).Select(sel => sel.ICOUM_ID).ToList();
            if (lstIntegration.Count > AppConsts.NONE)
            {
                return true;
            }
            return false;
        }

        #region UAT-3112
        List<SystemDocument> ISecurityRepository.GetBadgeFormDocuments(Int32 docTypeID)
        {
            return _dbNavigation.SystemDocuments.Where(cond => !cond.IsDeleted && cond.SD_DocType_ID == docTypeID).ToList();
        }
        #endregion

        /// <summary>
        /// UAT -3143
        /// </summary>
        /// <param name="selectedTenantIDs"></param>
        /// <param name="loggedInUserEmailId"></param>
        /// <returns></returns>
        Dictionary<String, String> ISecurityRepository.GetCategoryListFilterForLoggedInAgencyUserReports(String selectedTenantIDs, String loggedInUserEmailId)
        {
            Dictionary<String, String> dicCategoryList = new Dictionary<String, String>();
            EntityConnection connection = _dbNavigation.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("Report.usp_Report_Filter_ComplianceAndRotationCategories", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@LoggedInUserEmailId", loggedInUserEmailId);
                command.Parameters.AddWithValue("@TenantIDs", selectedTenantIDs);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > AppConsts.NONE && ds.Tables[0].Rows.Count > AppConsts.NONE)
                {
                    dicCategoryList = ds.Tables[0].AsEnumerable()
                                   .ToDictionary<DataRow, String, String>(row => row.Field<String>("CategoryID").Replace(",", "~"),
                                       row => row.Field<String>("CategoryName"));
                }
            }
            return dicCategoryList;
        }

        /// <summary>
        /// UAT-4509
        /// </summary>
        /// <param name="selectedTenantIDs"></param>
        /// <param name="loggedInUserEmailId"></param>
        /// <returns></returns>
        Dictionary<String, String> ISecurityRepository.GetAllCategoryListFilterForLoggedInAgencyUserReports(String selectedTenantIDs, String loggedInUserEmailId)
        {
            Dictionary<String, String> dicCategoryList = new Dictionary<String, String>();
            EntityConnection connection = _dbNavigation.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("Report.usp_Report_Filter_ComplianceAndRotationCategories_ForApplicantAndInstr", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@LoggedInUserEmailId", loggedInUserEmailId);
                command.Parameters.AddWithValue("@TenantIDs", selectedTenantIDs);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > AppConsts.NONE && ds.Tables[0].Rows.Count > AppConsts.NONE)
                {
                    dicCategoryList = ds.Tables[0].AsEnumerable()
                                   .ToDictionary<DataRow, String, String>(row => row.Field<String>("CategoryID").Replace(",", "~"),
                                       row => row.Field<String>("CategoryName"));
                }
            }
            return dicCategoryList;
        }

        #region UAT-2960

        Boolean ISecurityRepository.UpdateApplicantForAlumniAccess(Int32 currentLoggedInUserId)
        {
            EntityConnection connection = _dbNavigation.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("usp_UpdateApplicantForAlumniAccess", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CurrentLoggedInUserID", currentLoggedInUserId);
                cmd.ExecuteScalar();
                return true;
            }
        }

        List<OrganizationUserAlumniAccess> ISecurityRepository.GetApplicantDataForEmail(Int32 chunkSize, Int32 currentLoggedInUserId, Int32 tenantId)
        {
            String AlumniStatusDueCode = lkpAlumniStatus.Due.GetStringValue();
            String emailStatusCode = lkpEmailNotificationStatus.Pending.GetStringValue();

            List<OrganizationUserAlumniAccess> lstOrganizationUserAlumniAccess = new List<OrganizationUserAlumniAccess>();
            lstOrganizationUserAlumniAccess = _dbNavigation.OrganizationUserAlumniAccesses.Where(con => con.lkpAlumniStatu.LAS_Code == AlumniStatusDueCode && !con.lkpAlumniStatu.LAS_IsDeleted
                && con.lkpEmailNotificationStatu.ENS_Code == emailStatusCode && !con.lkpEmailNotificationStatu.ENS_IsDeleted
                && !con.OUAA_IsDeleted && con.OUAA_TenantID == tenantId).OrderByDescending(ord => ord.OUAA_ID).Take(chunkSize).ToList();

            if (!lstOrganizationUserAlumniAccess.IsNullOrEmpty())
                return lstOrganizationUserAlumniAccess;
            return new List<OrganizationUserAlumniAccess>();
        }


        Boolean ISecurityRepository.UpdateStatusInOrganizationUserAlumniAccess(Int32 OrgUserAlumniAccessId, String emailStatusCode, Int32 currentLoggedInUserId)
        {
            if (OrgUserAlumniAccessId > AppConsts.NONE)
            {
                Int32 emailStatusId = _dbNavigation.lkpEmailNotificationStatus.Where(con => con.ENS_Code == emailStatusCode && !con.ENS_IsDeleted).Select(sel => sel.ENS_ID).FirstOrDefault();
                OrganizationUserAlumniAccess organizationUserAlumniAccess = _dbNavigation.OrganizationUserAlumniAccesses.Where(con => con.OUAA_ID == OrgUserAlumniAccessId && !con.OUAA_IsDeleted).FirstOrDefault();
                if (!organizationUserAlumniAccess.IsNullOrEmpty())
                {
                    organizationUserAlumniAccess.OUAA_EmailNotificationStatusID = emailStatusId;
                    organizationUserAlumniAccess.OUAA_ModifiedBy = currentLoggedInUserId;
                    organizationUserAlumniAccess.OUAA_ModifiedOn = DateTime.Now;
                }
                if (_dbNavigation.SaveChanges() > AppConsts.NONE)
                    return true;
            }
            return false;
        }
        Boolean ISecurityRepository.CheckForAlumnAccessStatus(String StatusInitiatedCode, Guid Token, Int32 orgUserId)
        {
            OrganizationUserAlumniAccess organizationUserAlumniAccess = _dbNavigation.OrganizationUserAlumniAccesses.Where(con => con.OUAA_SecurityToken == Token && con.OUAA_OrganizationUserId == orgUserId && !con.OUAA_IsDeleted).FirstOrDefault();
            if (!organizationUserAlumniAccess.IsNullOrEmpty())
            {
                if (organizationUserAlumniAccess.lkpAlumniStatu.LAS_Code == StatusInitiatedCode)
                    return true;
            }
            return false;
        }


        Boolean ISecurityRepository.ComplianceDataMovementInsertLog(Int32 sourceTenantID, Int32 targetTenantID, Int32 pkgSubscriptionID, Int32 complianceDatamovementStatusID, Int32 currentLoggedInUserID)
        {
            _dbNavigation.ComplianceDataMovements.AddObject(new ComplianceDataMovement()
            {
                CDM_IsDeleted = false,
                CDM_CreatedOn = DateTime.Now,
                CDM_CreatedBy = currentLoggedInUserID,
                CDM_PackageSubscriptionID = pkgSubscriptionID,
                CDM_SourceTenantID = sourceTenantID,
                CDM_StatusID = complianceDatamovementStatusID,
                CDM_TargetTenantID = targetTenantID
            });

            _dbNavigation.SaveChanges();
            return true;
        }


        void ISecurityRepository.ComplianceDataMovementUpdateLog(Int32 orderID, Int32 tenentID, Int32 pkgSubscriptionID, Int32 complianceDatamovementStatusID, Int32 currentLoggedInUserID)
        {
            AlumniActivationDetail alumniActivationDetail = null;

            alumniActivationDetail = _dbNavigation.AlumniActivationDetails.Where(s => !s.AAD_IsDeleted && s.AAD_OrderID == orderID).FirstOrDefault();
            Int32 SourceTenantID = AppConsts.NONE;
            if (!alumniActivationDetail.IsNullOrEmpty())
            {
                SourceTenantID = alumniActivationDetail.AAD_SourceTenantID;
                alumniActivationDetail.AAD_PackageSubscriptionID = pkgSubscriptionID;
            }
            else
            {
                SourceTenantID = tenentID;
            }
            if (pkgSubscriptionID > AppConsts.NONE)
            {
                ComplianceDataMovement complianceDataMovement = new ComplianceDataMovement();
                complianceDataMovement.CDM_SourceTenantID = SourceTenantID;
                complianceDataMovement.CDM_TargetTenantID = tenentID;
                complianceDataMovement.CDM_PackageSubscriptionID = pkgSubscriptionID;
                complianceDataMovement.CDM_StatusID = complianceDatamovementStatusID;
                complianceDataMovement.CDM_CreatedOn = DateTime.Now;
                complianceDataMovement.CDM_CreatedBy = currentLoggedInUserID;
                _dbNavigation.ComplianceDataMovements.AddObject(complianceDataMovement);
                _dbNavigation.SaveChanges();
            }
        }

        String ISecurityRepository.GetAlumniSettingByCode(String AlumniTenantCode)
        {
            return _dbNavigation.AlumniSettings.Where(con => con.ALS_Code == AlumniTenantCode && !con.ALS_IsDeleted).Select(sel => sel.ALS_Value).FirstOrDefault();
        }

        List<AlumniPackageSubscription> ISecurityRepository.CopyComplianceDataToCompliance(Int32 backgroundProcessUserId, Int32 chunkSize)
        {
            try
            {
                List<AlumniPackageSubscription> lstTarSubscriptions = new List<AlumniPackageSubscription>();
                EntityConnection connection = _dbNavigation.Connection as EntityConnection;
                using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("usp_ComplianceDataMovement", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CurrentLoggedInUserID", backgroundProcessUserId);
                    cmd.Parameters.AddWithValue("@ChunkSize", chunkSize);
                    SqlDataAdapter adp = new SqlDataAdapter();
                    adp.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    adp.Fill(ds);
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            lstTarSubscriptions = ds.Tables[0].AsEnumerable().Select(col =>
                         new AlumniPackageSubscription
                         {
                             TarPackageSubscriptionID = col["PSID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["PSID"]),
                             ApplicantOrgUserID = col["OrgUserID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["OrgUserID"]),
                             SubscriptionMobilityStatusID = col["SubscriptionMobilityStatusID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["SubscriptionMobilityStatusID"]),
                             TarTenantID = col["TarTenantID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["TarTenantID"]),
                             TarPackageID = col["PkgID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["PkgID"]),
                             //RPTS_RoleID = col["RoleDetailID"] == DBNull.Value ? String.Empty : Convert.ToString(col["RoleDetailID"]),
                             //RoleDescription = col["RoleDescription"] == DBNull.Value ? String.Empty : Convert.ToString(col["RoleDescription"]),
                             //RoleName = col["RoleName"] == DBNull.Value ? String.Empty : Convert.ToString(col["RoleName"]),
                             //RPTS_IsAllowPreferredTenant = col["RPTS_IsAllowPreferredTenant"] == DBNull.Value ? false : Convert.ToBoolean(col["RPTS_IsAllowPreferredTenant"]),
                         }).ToList();
                        }
                    }
                }

                return lstTarSubscriptions;
            }
            catch (SysXException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw (new SysXException(ex.Message, ex));
            }
        }

        List<Int32> ISecurityRepository.GetOrganizationUserAlumniAccessIds(Int32 currentLoggedInUserID, Int32 TenantID)
        {
            return _dbNavigation.OrganizationUserAlumniAccesses.Where(con => con.OUAA_OrganizationUserId == currentLoggedInUserID && con.OUAA_TenantID == TenantID && !con.OUAA_IsDeleted).Select(sel => sel.OUAA_ID).ToList();
        }

        Boolean ISecurityRepository.CheckForAlumnAccessStatusDue(Int32 orgUserId, Int32 tenantId, String statusCode)
        {

            OrganizationUserAlumniAccess organizationUserAlumniAccess = _dbNavigation.OrganizationUserAlumniAccesses.Where(con => con.OUAA_OrganizationUserId == orgUserId && con.OUAA_TenantID == tenantId && !con.OUAA_IsDeleted).FirstOrDefault();
            if (!organizationUserAlumniAccess.IsNullOrEmpty())
            {
                //String statusDueCode = lkpAlumniStatus.Due.GetStringValue();
                // String statusInitiatedCode = lkpAlumniStatus.Initiated.GetStringValue();
                if (organizationUserAlumniAccess.lkpAlumniStatu.LAS_Code == statusCode)// || organizationUserAlumniAccess.lkpAlumniStatu.LAS_Code == statusInitiatedCode)
                    return true;
            }
            return false;
        }

        Boolean ISecurityRepository.AddAlumniActivationDetails(Int32 sourceTenantID, Int32 orgUserAlumniAccessID, Int32 orderID, Int32 pkgSubscriptionID, Int32 currentLoggedInUserId)
        {
            AlumniActivationDetail alumniActivationDetail = null;
            alumniActivationDetail = _dbNavigation.AlumniActivationDetails.Where(con => con.AAD_OrgUserAlumniAccessID == orgUserAlumniAccessID && con.AAD_OrderID == orderID && !con.AAD_IsDeleted).FirstOrDefault();
            if (alumniActivationDetail.IsNullOrEmpty())
            {
                alumniActivationDetail = new AlumniActivationDetail();
                alumniActivationDetail.AAD_CreatedBy = currentLoggedInUserId;
                alumniActivationDetail.AAD_CreatedOn = DateTime.Now;
                alumniActivationDetail.AAD_IsDeleted = false;
                alumniActivationDetail.AAD_OrderID = orderID;
                alumniActivationDetail.AAD_OrgUserAlumniAccessID = orgUserAlumniAccessID;
                alumniActivationDetail.AAD_PackageSubscriptionID = pkgSubscriptionID;
                alumniActivationDetail.AAD_SourceTenantID = sourceTenantID;
                _dbNavigation.AlumniActivationDetails.AddObject(alumniActivationDetail);

            }
            else
            {
                alumniActivationDetail.AAD_ModifiedBy = currentLoggedInUserId;
                alumniActivationDetail.AAD_ModifiedOn = DateTime.Now;
            }
            if (_dbNavigation.SaveChanges() > AppConsts.NONE)
                return true;
            else
                return false;
        }

        OrganizationUserAlumniAccess ISecurityRepository.UpdateOrganizationUserAlumniAccess(Int32 aluminAccessId, String alumniStatus, Int32 currentLoggedInUserId)
        {
            OrganizationUserAlumniAccess organizationUserAlumniAccess = null;
            if (aluminAccessId > AppConsts.NONE)
            {
                Int32 alumniStatusID = _dbNavigation.lkpAlumniStatus.Where(con => con.LAS_Code == alumniStatus && !con.LAS_IsDeleted).Select(sel => sel.LAS_ID).FirstOrDefault();
                organizationUserAlumniAccess = _dbNavigation.OrganizationUserAlumniAccesses.Where(con => con.OUAA_ID == aluminAccessId && !con.OUAA_IsDeleted).FirstOrDefault();
                if (!organizationUserAlumniAccess.IsNullOrEmpty())
                {
                    organizationUserAlumniAccess.OUAA_AlumniStatusId = alumniStatusID;
                    organizationUserAlumniAccess.OUAA_ModifiedBy = currentLoggedInUserId;
                    organizationUserAlumniAccess.OUAA_ModifiedOn = DateTime.Now;
                    _dbNavigation.SaveChanges();
                }
            }
            return organizationUserAlumniAccess;
        }

        Boolean ISecurityRepository.IsMultiTenantUserExceptAlumni(Guid userId)
        {
            String alumniTenantCode = AlumniSettings.AlumniTenantID.GetStringValue();
            Int32 alumniTenantId = AppConsts.NONE;
            var result = _dbNavigation.AlumniSettings.Where(con => con.ALS_Code == alumniTenantCode && !con.ALS_IsDeleted).FirstOrDefault();
            if (!result.IsNullOrEmpty() && !result.ALS_Value.IsNullOrEmpty())
            {
                alumniTenantId = Int32.Parse(result.ALS_Value);
            }

            Boolean _isMultiTenant = false;
            Int32 applicantUserCount = AppConsts.NONE;
            Int32 clientAdminUserCount = AppConsts.NONE;
            Int32 adminUserCount = AppConsts.NONE;

            var _lstOrgUsers = _dbNavigation.OrganizationUsers.Where(orgUsr => orgUsr.UserID == userId && (orgUsr.IsSharedUser ?? false) == false
                                                                 && !orgUsr.IsDeleted).ToList(); //UAT-1218 SharedUser check

            if (!_lstOrgUsers.IsNullOrEmpty() && alumniTenantId > AppConsts.NONE)
            {
                _lstOrgUsers = _lstOrgUsers.Where(con => con.Organization.TenantID != alumniTenantId).ToList();
            }

            if (_lstOrgUsers.Count() > AppConsts.ONE)
            {
                //UAT-1218
                foreach (var orgUser in _lstOrgUsers)
                {
                    if ((orgUser.IsApplicant ?? false) == true)
                    {
                        applicantUserCount = applicantUserCount + AppConsts.ONE;
                    }
                    else if ((orgUser.IsApplicant ?? false) == false && orgUser.Organization.TenantID != AppConsts.SUPER_ADMIN_TENANT_ID)
                    {
                        clientAdminUserCount = clientAdminUserCount + AppConsts.ONE;
                    }
                    else if ((orgUser.IsApplicant ?? false) == false && orgUser.Organization.TenantID == AppConsts.SUPER_ADMIN_TENANT_ID)
                    {
                        adminUserCount = adminUserCount + AppConsts.ONE;
                    }
                }
                if (applicantUserCount > AppConsts.ONE || clientAdminUserCount > AppConsts.ONE || adminUserCount > AppConsts.ONE)
                {
                    _isMultiTenant = true;
                }
            }
            return _isMultiTenant;

        }


        #endregion

        public List<BookmarkedFeatureContact> GetAccessibleFeatures(Int32 orgUserID, Int32 tenantID, Int32 sysxBlockID, bool isSuperAdmin)
        {
            List<BookmarkedFeatureContact> lstAccessibleFeatures = new List<BookmarkedFeatureContact>();

            EntityConnection connection = _dbNavigation.Connection as EntityConnection;

            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter("@OrgUserID", orgUserID),
                             new SqlParameter("@TenantID", tenantID),
                             new SqlParameter("@SysxBlockID", sysxBlockID),
                             new SqlParameter("@IsSuperAdmin", isSuperAdmin)
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetAccessibleFeaturesByUserID", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            BookmarkedFeatureContact feature = new BookmarkedFeatureContact();
                            feature.TreeNodeTypeID = dr["TreeNodeTypeID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["TreeNodeTypeID"]);
                            feature.NodeId = dr["NodeId"] == DBNull.Value ? String.Empty : Convert.ToString(dr["NodeId"]);
                            feature.ParentNodeId = dr["ParentNodeId"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ParentNodeId"]);
                            feature.Level = dr["Level"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["Level"]);
                            feature.DataID = dr["DataID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["DataID"]);
                            feature.FeatureName = dr["FeatureName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["FeatureName"]);
                            feature.FeatureDesc = dr["FeatureDesc"] == DBNull.Value ? String.Empty : Convert.ToString(dr["FeatureDesc"]);
                            feature.ParentDataID = dr["ParentDataID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(dr["ParentDataID"]);
                            feature.UICode = dr["UICode"] == DBNull.Value ? String.Empty : Convert.ToString(dr["UICode"]);
                            feature.IsChildFetaure = dr["IsChildFeature"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsChildFeature"]);
                            feature.TenantProductSysXBlockID = dr["TenantProductSysXBlockID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(dr["TenantProductSysXBlockID"]);
                            feature.IsFeatureBookmarked = dr["IsFeatureBookmarked"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsFeatureBookmarked"]);
                            lstAccessibleFeatures.Add(feature);
                        }
                    }
                }
                base.CloseSQLDataReaderConnection(con);
            }
            return lstAccessibleFeatures;
        }

        public Boolean SaveUpdateBookmarkedFeatures(Int32? tenantProductSysXBlockID, Int32 productFeatureID, Int32 orgUserID, bool isBookmarked)
        {
            var bookmarkedFeature = new FeaturesBookmark();

            if (tenantProductSysXBlockID.HasValue)
            {
                bookmarkedFeature = _dbNavigation.FeaturesBookmarks.Where(cond => cond.FB_TenantProductSysXBlockID == tenantProductSysXBlockID
                                                                       && cond.FB_OrganizationUserID == orgUserID
                                                                       && !cond.FB_IsDeleted)
                                                                       .FirstOrDefault();
            }
            else
            {
                bookmarkedFeature = _dbNavigation.FeaturesBookmarks.Where(cond => cond.FB_ProductFeatureID == productFeatureID
                                                                        && cond.FB_OrganizationUserID == orgUserID
                                                                        && !cond.FB_IsDeleted)
                                                                        .FirstOrDefault();
            }


            if (!bookmarkedFeature.IsNullOrEmpty())
            {
                bookmarkedFeature.FB_ModifiedBy = orgUserID;
                bookmarkedFeature.FB_ModifiedOn = DateTime.Now;
                bookmarkedFeature.FB_IsBookmarked = isBookmarked;
            }
            else
            {
                FeaturesBookmark featuresBookmark = new FeaturesBookmark();
                featuresBookmark.FB_OrganizationUserID = orgUserID;

                if (tenantProductSysXBlockID.HasValue)
                    featuresBookmark.FB_TenantProductSysXBlockID = tenantProductSysXBlockID;

                if (tenantProductSysXBlockID.HasValue == false)
                    featuresBookmark.FB_ProductFeatureID = productFeatureID;

                featuresBookmark.FB_IsBookmarked = isBookmarked;
                featuresBookmark.FB_IsDeleted = false;
                featuresBookmark.FB_CreatedBy = orgUserID;
                featuresBookmark.FB_CreatedOn = DateTime.Now;
                _dbNavigation.FeaturesBookmarks.AddObject(featuresBookmark);
            }

            if (_dbNavigation.SaveChanges() > 0)
                return true;
            else
                return false;
        }

        #region [UAT-3326] Creation of internal notes function (like on portfolio details) for client admins on the Client User Details screen.
        DataTable ISecurityRepository.GetAdminProfileNotesList(Int32 organizationUserID)
        {
            EntityConnection connection = _dbNavigation.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetAdminProfileNotes", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@OrganizationUserID", organizationUserID);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);

                if (ds.Tables.Count > AppConsts.NONE)
                {
                    return ds.Tables[0];
                }
            }
            return null;
        }

        /// <summary>
        /// Method to save, update and delete Client Admin's profile notes.
        /// </summary>
        /// <param name="applicantProfileNoteList"></param>
        /// <returns></returns>
        Boolean ISecurityRepository.SaveUpdateAdminProfileNotes(List<ApplicantProfileNotesContract> adminProfileNoteList)
        {
            List<ApplicantProfileNotesContract> adminNoteListToSave = new List<ApplicantProfileNotesContract>();
            List<ApplicantProfileNotesContract> adminNoteListToUpdate = new List<ApplicantProfileNotesContract>();
            List<ApplicantProfileNotesContract> adminNoteListToDelete = new List<ApplicantProfileNotesContract>();
            if (adminProfileNoteList.IsNotNull() && adminProfileNoteList.Count > 0)
            {
                adminNoteListToSave = adminProfileNoteList.Where(cond => cond.IsNew == true).ToList();
                adminNoteListToUpdate = adminProfileNoteList.Where(cond => cond.IsUpdated == true).ToList();
                adminNoteListToDelete = adminProfileNoteList.Where(cond => cond.APN_IsDeleted == true).ToList();
                foreach (ApplicantProfileNotesContract tempNoteToSave in adminNoteListToSave)
                {
                    AdminProfileNote adminNoteToSave = new AdminProfileNote();
                    adminNoteToSave.APN_OrganizationUserID = tempNoteToSave.APN_OrganizationUserID;
                    adminNoteToSave.APN_ProfileNotes = tempNoteToSave.APN_ProfileNote;
                    adminNoteToSave.APN_CreatedBy = tempNoteToSave.APN_CreatedBy;
                    adminNoteToSave.APN_CreatedOn = DateTime.Now;
                    adminNoteToSave.APN_IsDeleted = false;
                    _dbNavigation.AdminProfileNotes.AddObject(adminNoteToSave);

                }
                foreach (ApplicantProfileNotesContract tempNoteToUpdate in adminNoteListToUpdate)
                {

                    AdminProfileNote adminNoteToUpdate = _dbNavigation.AdminProfileNotes.FirstOrDefault(cond => cond.APN_ID == tempNoteToUpdate.APN_ID && cond.APN_IsDeleted == false);
                    adminNoteToUpdate.APN_ProfileNotes = tempNoteToUpdate.APN_ProfileNote;
                    adminNoteToUpdate.APN_ModifiedBy = tempNoteToUpdate.APN_CreatedBy;
                    adminNoteToUpdate.APN_ModifiedOn = DateTime.Now;
                }
                foreach (ApplicantProfileNotesContract tempNoteToDelete in adminNoteListToDelete)
                {
                    AdminProfileNote adminNoteToDelete = _dbNavigation.AdminProfileNotes.FirstOrDefault(cond => cond.APN_ID == tempNoteToDelete.APN_ID && cond.APN_IsDeleted == false);
                    adminNoteToDelete.APN_IsDeleted = true;
                    adminNoteToDelete.APN_ModifiedBy = tempNoteToDelete.APN_CreatedBy;
                    adminNoteToDelete.APN_ModifiedOn = DateTime.Now;
                }

                if (_dbNavigation.SaveChanges() > 0)
                    return true;
            }
            return false;
        }

        Boolean ISecurityRepository.SaveAdminProfileNotes(AdminProfileNote adminProfileNoteObj)
        {
            if (adminProfileNoteObj.IsNotNull())
            {
                _dbNavigation.AdminProfileNotes.AddObject(adminProfileNoteObj);
                if (_dbNavigation.SaveChanges() > 0)
                    return true;
            }
            return false;
        }

        AdminProfileNote ISecurityRepository.GetAdminProfileNotesByNoteID(Int32 adminProfileNoteID)
        {
            return _dbNavigation.AdminProfileNotes.FirstOrDefault(cond => cond.APN_ID == adminProfileNoteID && cond.APN_IsDeleted == false);
        }

        Boolean ISecurityRepository.UpdateAdminProfileNote()
        {
            if (_dbNavigation.SaveChanges() > 0)
            {
                return true;
            }
            return false;
        }

        #endregion


        #region UAT-3319

        DataTable ISecurityRepository.GetClientDataForAgencyAndAgencyHierarchy(Int32 LoggedInUserId, String AGencyID, String Tenantid)
        {
            EntityConnection connection = _dbNavigation.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetClientDataForAgencyAndAgencyHierarchy", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@LoggedInCurrentUserID", LoggedInUserId);
                command.Parameters.AddWithValue("@AGencyID", AGencyID);
                command.Parameters.AddWithValue("@Tenantid", Tenantid);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);

                if (ds.Tables.Count > AppConsts.NONE)
                {
                    return ds.Tables[0];
                }
            }
            return null;
        }

        #endregion


        #region UAt-3364
        public List<SystemEntityUserPermission> GetRotationCreatorGranularPermissionsByOrgUserID(Int32 orgUserID)
        {
            return _dbNavigation.SystemEntityUserPermissions.Where(cond => cond.SEUP_OrganisationUserId == orgUserID && !cond.SEUP_IsDeleted).ToList();
        }
        #endregion


        #region UAT-3347 : "Client Login" functionality from new screen
        public List<ClientLoginSearchContract> GetClientLoginSearchData(String tenantIDList, ClientLoginSearchContract clientUserSearchContract, CustomPagingArgsContract customPagingArgsContract)
        {
            EntityConnection connection = _dbNavigation.Connection as EntityConnection;
            List<ClientLoginSearchContract> clientLoginSearchData = new List<ClientLoginSearchContract>();
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("usp_GetDataForClientLoginSearch", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tenantIDList", tenantIDList);
                cmd.Parameters.AddWithValue("@dataXML", clientUserSearchContract.CreateXml());
                cmd.Parameters.AddWithValue("@sortingAndPagingData", customPagingArgsContract.CreateXml());
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        clientLoginSearchData = ds.Tables[0].AsEnumerable().Select(col =>
                            new ClientLoginSearchContract
                            {
                                ClientFirstName = col["ClientFirstName"] == DBNull.Value ? String.Empty : Convert.ToString(col["ClientFirstName"]),
                                ClientLastName = col["ClientLastName"] == DBNull.Value ? String.Empty : Convert.ToString(col["ClientLastName"]),
                                ClientUserName = col["ClientUserName"] == DBNull.Value ? String.Empty : Convert.ToString(col["ClientUserName"]),
                                EmailAddress = col["EmailAddress"] == DBNull.Value ? String.Empty : Convert.ToString(col["EmailAddress"]),
                                Phone = col["Phone"] == DBNull.Value ? String.Empty : Convert.ToString(col["Phone"]), // col["Phone"].ToString(),
                                OrganizationUserID = col["OrganizationUserID"] == DBNull.Value ? 0 : Convert.ToInt32(col["OrganizationUserID"]),
                                ClientTenantID = col["TenantID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(col["TenantID"]),
                                TenantName = col["TenantName"] == DBNull.Value ? String.Empty : Convert.ToString(col["TenantName"]),
                                UserID = col["UserID"] == DBNull.Value ? String.Empty : Convert.ToString(col["UserID"]),
                                TotalCount = Convert.ToInt32(col["TotalCount"]),
                                IsActive = Convert.ToBoolean(col["IsActive"])
                            }).ToList();
                    }
                }
            }
            return clientLoginSearchData;
        }


        Boolean ISecurityRepository.AddImpersonationHistory(Int32 clientAdminUserID, Int32 CurrentLoggedInUserID)
        {
            ImpersonationHistory objImpersonationHistory = new ImpersonationHistory();
            objImpersonationHistory.IH_ActorUserId = CurrentLoggedInUserID;
            objImpersonationHistory.IH_TargetUserId = clientAdminUserID;
            objImpersonationHistory.IH_InitiatedOn = DateTime.Now;
            objImpersonationHistory.IH_CreatedBy = CurrentLoggedInUserID;
            objImpersonationHistory.IH_CreatedOn = DateTime.Now;
            objImpersonationHistory.IH_IsDeleted = 0;
            _dbNavigation.ImpersonationHistories.AddObject(objImpersonationHistory);
            _dbNavigation.SaveChanges();
            return true;
        }

        public List<LookupContract> GetExistingUserProfileLists(String userName, String email, Int32 defaultTenantId)
        {
            //List<OrganizationUser> lstOrgUsers = _dbNavigation.OrganizationUsers.Where(obj => (obj.aspnet_Users.UserName.ToLower().Equals(userName.ToLower())
            //    ||  obj.aspnet_Users.aspnet_Membership.Email.ToLower() == email.ToLower()))
            //    && obj.IsActive == true
            //    && obj.IsDeleted == false).ToList();

            List<OrganizationUser> lstOrgUsers = new List<OrganizationUser>();
            var userNameList = _dbNavigation.OrganizationUsers
                                                                                .Where(
                                                                                cond =>
                                                                                (cond.aspnet_Users.UserName.ToLower().Equals(userName.ToLower()))
                                                                                &&
                                                                                cond.IsActive == true
                                                                                &&
                                                                                cond.IsDeleted == false
                                                                                ).ToList();

            var useremailList = _dbNavigation.OrganizationUsers
                                                                                .Where(
                                                                                cond =>
                                                                                (cond.aspnet_Users.aspnet_Membership.Email.ToLower() == email.ToLower())
                                                                                &&
                                                                                cond.IsActive == true
                                                                                &&
                                                                                cond.IsDeleted == false
                                                                                ).ToList();
            var combinedList = new List<OrganizationUser>();
            combinedList.AddRange(userNameList);
            combinedList.AddRange(useremailList);
            lstOrgUsers.AddRange(combinedList.Distinct());

            List<LookupContract> MatchingUserList = new List<LookupContract>();
            //For client admin
            if (lstOrgUsers.Any(obj => obj.IsApplicant == false && (obj.IsSharedUser ?? false) == false && obj.Organization.TenantID != AppConsts.SUPER_ADMIN_TENANT_ID))
            {
                MatchingUserList.AddRange(lstOrgUsers.Where(obj => obj.IsApplicant == false && (obj.IsSharedUser ?? false) == false && obj.Organization.TenantID != AppConsts.SUPER_ADMIN_TENANT_ID).Select(obj => new LookupContract
                {
                    Name = " I am '" + obj.FirstName + " " + obj.LastName + "' (Client Admin) from '" + obj.Organization.Tenant.TenantName + "'.",
                    Code = obj.aspnet_Users.UserName,
                    ID = obj.OrganizationID,
                    UserID = obj.OrganizationUserID
                }).ToList());
            }

            //For Applicant
            if (lstOrgUsers.Any(obj => obj.IsApplicant == true && obj.Organization.OrganizationID != defaultTenantId))
            {
                MatchingUserList.AddRange(lstOrgUsers.Where(obj => obj.IsApplicant == true && obj.Organization.OrganizationID != defaultTenantId).Select(obj => new LookupContract
                {
                    Name = " I am '" + obj.FirstName + " " + obj.LastName + "' (Applicant) from '" + obj.Organization.Tenant.TenantName + "'.",
                    Code = obj.aspnet_Users.UserName,
                    ID = obj.OrganizationID,
                    UserID = obj.OrganizationUserID
                }).ToList());
            }

            //For Shared User
            if (lstOrgUsers.Any(obj => obj.IsApplicant == false && obj.IsSharedUser == true))
            {

                foreach (var item in lstOrgUsers.Where(obj => obj.IsApplicant == false && obj.IsSharedUser == true && obj.IsActive).ToList())
                {
                    Int32 orgUserID = item.OrganizationUserID;
                    Guid userID = item.UserID;
                    List<String> lstUserTypeCodes = _dbNavigation.OrganizationUserTypeMappings.Where(cond => cond.OTM_OrgUserID == orgUserID && cond.OTM_IsDeleted == false)
                        .Select(col => col.lkpOrgUserType.OrgUserTypeCode).ToList();
                    String userType = String.Empty;

                    if (lstUserTypeCodes.Contains(OrganizationUserType.AgencyUser.GetStringValue()))
                    {
                        String agencyName = String.Empty;
                        List<String> lstUserAgencyName = SharedDataDBContext.UserAgencyMappings.Where(cond => cond.UAM_IsVerified
                                                                                                        && !cond.UAM_IsDeleted
                                                                                                        && cond.AgencyUser.AGU_UserID == userID
                                                                                                        && !cond.AgencyUser.AGU_IsDeleted
                                                                                                        && !cond.UAM_IsDeleted
                                                                                                        && !cond.Agency.AG_IsDeleted)
                                                                                                       .Select(col => col.Agency.AG_Name)
                                                                                                       .ToList();
                        if (!lstUserAgencyName.IsNullOrEmpty())
                        {
                            agencyName = String.Join(",", lstUserAgencyName);
                        }
                        //base.SharedDataDBContext.AgencyUsers.Where(cond => cond.AGU_UserID == userID && !cond.AGU_IsDeleted).Select(col => col.Agency.AG_Name).FirstOrDefault(); //UAT-1641
                        userType = "(Agency User) ";//from " + agencyName;
                    }
                    else if (lstUserTypeCodes.Contains(OrganizationUserType.ApplicantsSharedUser.GetStringValue()))
                    {
                        userType = "(Shared User)";
                    }
                    else if (lstUserTypeCodes.Contains(OrganizationUserType.Instructor.GetStringValue()))
                    {
                        userType = "(Instructor)";
                    }
                    else if (lstUserTypeCodes.Contains(OrganizationUserType.Preceptor.GetStringValue()))
                    {
                        userType = "(Preceptor)";
                    }

                    MatchingUserList.AddRange(lstOrgUsers.Where(obj => obj.IsSharedUser == true && obj.OrganizationUserID == orgUserID).Select(obj => new LookupContract
                    {
                        Name = " I am '" + obj.FirstName + " " + obj.LastName + " " + userType + "'.",
                        Code = obj.aspnet_Users.UserName,
                        ID = obj.OrganizationID,
                        UserID = obj.OrganizationUserID
                    }).ToList());

                }



            }
            return MatchingUserList;
        }

        OrganizationUser ISecurityRepository.GetOrgUserIfUsernameExistInSecuritytDBForAccountLinking(String userName)
        {
            return _dbNavigation.OrganizationUsers.Include(SysXEntityConstants.TABLE_ORGANIZATION)
              .Where(x => x.aspnet_Users.UserName.ToLower().Equals(userName)
                  && x.IsActive == true
              && x.IsDeleted == false).FirstOrDefault();
        }

        List<OrganizationUser> ISecurityRepository.GetOrgUserListIfUsernameExistInSecuritytDBForAccountLinking(String userName)
        {
            return _dbNavigation.OrganizationUsers.Include(SysXEntityConstants.TABLE_ORGANIZATION)
              .Where(x =>
                  (
                  (x.aspnet_Users.UserName.ToLower().Equals(userName.ToLower()))
                  ||
                  (x.aspnet_Users.aspnet_Membership.Email.ToLower().Equals(userName.ToLower()))
                  )
                  && x.IsActive == true

              && x.IsDeleted == false).ToList();
        }
        #endregion

        #region UAT-3540
        Boolean ISecurityRepository.CheckRoleForShibbolethNYU(Boolean IsApplicantRoleCheck, String Roles)
        {
            EntityConnection connection = _dbNavigation.Connection as EntityConnection;
            Boolean IsRolePresent = false;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[api].[CheckRoleForShibbolethNYU]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IsApplicantRoleCheck", IsApplicantRoleCheck);
                cmd.Parameters.AddWithValue("@Roles", Roles);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        IsRolePresent = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsRoleSpecified"]);
                    }
                }
            }
            return IsRolePresent;
        }
        #endregion
        #region UAT-3461
        //List<ManageRandomReviewsContract> ISecurityRepository.GetManageRandomReviewsList(Int32? queueConfigurationID, Int32? tenantId)
        //{
        //    List<ManageRandomReviewsContract> result = new List<ManageRandomReviewsContract>();

        //    if (queueConfigurationID > AppConsts.NONE)
        //    {
        //        ManageRandomReviewsContract manageRandomReviewsContract = _dbNavigation.ReconciliationQueueConfigurations.Where(d => d.RQC_ID == queueConfigurationID && !d.RQC_IsDeleted)
        //            .Select(sel => new ManageRandomReviewsContract()
        //            {
        //                ReconciliationQueueConfigurationID = sel.RQC_ID,
        //                TenantID = sel.ReconciliationQueueConfigurationTenantMappings.Where(f => !f.RQCTM_IsDeleted).FirstOrDefault().RQCTM_TenantID,
        //                TenantName = sel.ReconciliationQueueConfigurationTenantMappings.Where(f => !f.RQCTM_IsDeleted).FirstOrDefault().Tenant.TenantName,
        //                Description = sel.RQC_Description,
        //                Percentage = sel.RQC_RecordPercentage.Value,
        //                Reviews = sel.RQC_ReviewerCount.Value
        //            })
        //            .FirstOrDefault();
        //        if (!manageRandomReviewsContract.IsNullOrEmpty())
        //            result.Add(manageRandomReviewsContract);
        //    }
        //    else
        //    {
        //        List<ManageRandomReviewsContract> manageRandomReviewsContractList = _dbNavigation.ReconciliationQueueConfigurations.Where(d => !d.RQC_IsDeleted)
        //            .Select(sel => new ManageRandomReviewsContract()
        //            {
        //                ReconciliationQueueConfigurationID = sel.RQC_ID,
        //                TenantID = sel.ReconciliationQueueConfigurationTenantMappings.Where(f => !f.RQCTM_IsDeleted).FirstOrDefault().RQCTM_TenantID,
        //                TenantName = sel.ReconciliationQueueConfigurationTenantMappings.Where(f => !f.RQCTM_IsDeleted).FirstOrDefault().Tenant.TenantName,
        //                Description = sel.RQC_Description,
        //                Percentage = sel.RQC_RecordPercentage.Value,
        //                Reviews = sel.RQC_ReviewerCount.Value
        //            })
        //            .ToList();
        //        if (!manageRandomReviewsContractList.IsNullOrEmpty() && manageRandomReviewsContractList.Count > AppConsts.NONE)
        //            result.AddRange(manageRandomReviewsContractList);
        //    }
        //    return result;
        //}

        Boolean ISecurityRepository.DeleteReconciliationQueueConfiguration(Int32 queueConfigurationID, Int32 currentLoggedInID)
        {
            var queueConfigurationDetails = _dbNavigation.ReconciliationQueueConfigurations.Where(d => d.RQC_ID == queueConfigurationID && !d.RQC_IsDeleted).FirstOrDefault();
            if (!queueConfigurationDetails.IsNullOrEmpty())
            {
                queueConfigurationDetails.RQC_IsDeleted = true;
                queueConfigurationDetails.RQC_ModifiedOn = DateTime.Now;
                queueConfigurationDetails.RQC_ModifiedBy = currentLoggedInID;
                queueConfigurationDetails.ReconciliationQueueConfigurationTenantMappings.Where(f => !f.RQCTM_IsDeleted)
                    .ForEach(d =>
                        {
                            d.RQCTM_IsDeleted = true;
                            d.RQCTM_ModifiedBy = currentLoggedInID;
                            d.RQCTM_ModifiedOn = DateTime.Now;
                        }
                        );
                _dbNavigation.SaveChanges();
            }
            return true;
        }
        //String ISecurityRepository.SaveReconciliationQueueConfiguration(Int32 queueConfigurationID, Int32 tenantID, String description, Decimal percentage, Int32 reviews, Int32 currentLoggedInID)
        //{
        //    String errorMessage = String.Empty;
        //    if (queueConfigurationID > AppConsts.NONE)
        //    {
        //        //dbUpdate
        //        var queueConfigurationDetails = _dbNavigation.ReconciliationQueueConfigurations.Where(d => d.RQC_ID == queueConfigurationID && !d.RQC_IsDeleted).FirstOrDefault();
        //        queueConfigurationDetails.RQC_ModifiedBy = currentLoggedInID;
        //        queueConfigurationDetails.RQC_ModifiedOn = DateTime.Now;
        //        queueConfigurationDetails.RQC_Description = description;
        //        queueConfigurationDetails.RQC_RecordPercentage = percentage;
        //        queueConfigurationDetails.RQC_ReviewerCount = reviews;
        //    }
        //    else
        //    {
        //        ReconciliationQueueConfiguration dbInsert = new ReconciliationQueueConfiguration();
        //        dbInsert.RQC_ReviewerCount = reviews;
        //        dbInsert.RQC_RecordPercentage = percentage;
        //        dbInsert.RQC_IsDeleted = false;
        //        dbInsert.RQC_Description = description;
        //        dbInsert.RQC_CreatedOn = DateTime.Now;
        //        dbInsert.RQC_CreatedBy = currentLoggedInID;

        //        ReconciliationQueueConfigurationTenantMapping _dbInsertReconciliationQueueConfigurationTenantMapping = new ReconciliationQueueConfigurationTenantMapping();
        //        _dbInsertReconciliationQueueConfigurationTenantMapping.RQCTM_IsDeleted = false;
        //        _dbInsertReconciliationQueueConfigurationTenantMapping.RQCTM_CreatedOn = DateTime.Now;
        //        _dbInsertReconciliationQueueConfigurationTenantMapping.RQCTM_CreatedBy = currentLoggedInID;
        //        _dbInsertReconciliationQueueConfigurationTenantMapping.RQCTM_TenantID = tenantID;

        //        dbInsert.ReconciliationQueueConfigurationTenantMappings.Add(_dbInsertReconciliationQueueConfigurationTenantMapping);
        //        _dbNavigation.ReconciliationQueueConfigurations.AddObject(dbInsert);
        //    }
        //    _dbNavigation.SaveChanges();
        //    return errorMessage;
        //}
        #endregion

        Int32 ISecurityRepository.GetTenantIdByOrganizationUserID(Int32 orgUserID)
        {

            OrganizationUser organizationUser = _dbNavigation.OrganizationUsers.Include("Organization").Where(cond => cond.OrganizationUserID == orgUserID
                                                                                                                        && !cond.IsDeleted && cond.IsActive
                                                                                                                        && cond.Organization.IsActive && !cond.Organization.IsDeleted).FirstOrDefault();
            if (!organizationUser.Organization.IsNullOrEmpty())
                return GetTenantIdFromOrganizationId(organizationUser.Organization.OrganizationID);
            return AppConsts.NONE;
        }
        Boolean ISecurityRepository.CheckIfUserIsEnroller(string userID)
        {
            var User_Guid = Guid.Parse(userID);
            var lstRoles = _dbNavigation.vw_aspnet_UsersInRoles
                               .Where(cond => cond.UserId == User_Guid).Select(s => s.RoleId).ToList();


            var res = _dbNavigation.aspnet_RolePreferredTenantSetting
                            .Where(cond => !cond.RPTS_IsDeleted
                                    && cond.RPTS_IsAllowLocationEnroller == true
                                    && lstRoles.Contains(cond.RPTS_RoleID));


            if (!res.IsNullOrEmpty() && res.Count() > 0)
                return true;
            else
                return false;
        }
        Boolean ISecurityRepository.IsLocationServiceTenant(Int32 tenantID)
        {
            List<String> locationServiceTenantIds = _dbNavigation.AppConfigurations.Where(cond => cond.AC_Key == "LocationServiceTenantIds").FirstOrDefault().AC_Value.Split(',').ToList();
            if (!locationServiceTenantIds.IsNullOrEmpty() && locationServiceTenantIds.Count > AppConsts.NONE)
            {
                if (locationServiceTenantIds.Contains(tenantID.ToString()))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        String ISecurityRepository.GetLocationTenantCompanyName()
        {
            return _dbNavigation.AppConfigurations
                .Where(cond => cond.AC_Key == "LocationTenantCompanyName")
                .Select(a => a.AC_Value)
                .FirstOrDefault();
        }

        List<Tenant> ISecurityRepository.GetListOfTenantWithLocationService()
        {
            List<Tenant> result = new List<Tenant>();
            var locationServiceTenantIds = _dbNavigation.AppConfigurations.Where(cond => cond.AC_Key == "LocationServiceTenantIds")
                .FirstOrDefault()
                .AC_Value.Split(',')
                .Select(t => Convert.ToInt32(t))
                .ToList();
            if (locationServiceTenantIds.Any())
            {
                result = _dbNavigation.Tenants.Where(x => x.IsActive
                && !x.IsDeleted
                && locationServiceTenantIds.Any(l => l == x.TenantID)).ToList();
            }
            return result;
        }

        int ISecurityRepository.GetTenantID(int webSiteId)
        {
            if (!webSiteId.IsNullOrEmpty() && webSiteId > AppConsts.NONE)
                return _dbNavigation.TenantWebsiteMappings.Where(cond => !cond.TWM_IsDeleted && cond.TWM_WebSiteID == webSiteId && cond.Tenant.IsActive && !cond.Tenant.IsDeleted).FirstOrDefault().TWM_TenantID;
            return AppConsts.NONE;
        }

        Boolean ISecurityRepository.CheckRoleForShibbolethUCONN(Boolean IsApplicantRoleCheck, String Roles)
        {
            EntityConnection connection = _dbNavigation.Connection as EntityConnection;
            Boolean IsRolePresent = false;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[api].[CheckRoleForShibbolethUCONN]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IsApplicantRoleCheck", IsApplicantRoleCheck);
                cmd.Parameters.AddWithValue("@Roles", Roles);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        IsRolePresent = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsRoleSpecified"]);
                    }
                }
            }
            return IsRolePresent;
        }
        #region UAT-3669
        Boolean ISecurityRepository.UpdateBlockedOrdersHistoryData(Int32 TenantId, Int32 selectedHierarchyNodeID, String selectedPackageIds, Int32 applicantOrgUserId, String firstName, String lastName, Int32 blockedReasonId)
        {
            BlockedOrdersHistory blockedOrdersHistory = new BlockedOrdersHistory();
            blockedOrdersHistory.BOH_ApplicantOrgUserId = applicantOrgUserId;
            blockedOrdersHistory.BOH_BlockedOrderReasonId = blockedReasonId;
            blockedOrdersHistory.BOH_ApplicantFirstName = firstName;
            blockedOrdersHistory.BOH_ApplicantLastName = lastName;
            blockedOrdersHistory.BOH_SelectedPackageIds = selectedPackageIds;
            blockedOrdersHistory.BOH_SelectedHierarchyNodeId = selectedHierarchyNodeID;
            blockedOrdersHistory.BOH_TenantId = TenantId;
            blockedOrdersHistory.BOH_IsActive = true;
            blockedOrdersHistory.BOH_IsDeleted = false;
            blockedOrdersHistory.BOH_CreatedBy = applicantOrgUserId;
            blockedOrdersHistory.BOH_CreatedOn = DateTime.Now;
            _dbNavigation.BlockedOrdersHistories.AddObject(blockedOrdersHistory);
            if (_dbNavigation.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }


        #endregion

        #region UAT-3734

        String ISecurityRepository.GetLocTenMaxAllowedDays()
        {
            return _dbNavigation.AppConfigurations
                .Where(cond => cond.AC_Key == "MaxAllowedRescheduleDays")
                .Select(a => a.AC_Value)
                .FirstOrDefault();
        }
        #endregion

        #region UAT-3737

        String ISecurityRepository.GetInstructorNameByOrganizationUserId(Int32 organizationUserId)
        {
            String userName = String.Empty;
            OrganizationUser orgUser = _dbNavigation.OrganizationUsers.Where(con => con.OrganizationUserID == organizationUserId && !con.IsDeleted).FirstOrDefault();
            if (!orgUser.IsNullOrEmpty())
                userName = orgUser.FirstName + " " + orgUser.LastName;
            return userName;
        }
        #endregion

        #region UAT-3744

        List<DataReconciliationQueueContract> ISecurityRepository.GetNextActiveReconciledItem(String selectedInstitutionIds, Int32 ComplianceItemReconciliationDataID)
        {
            EntityConnection connection = _dbNavigation.Connection as EntityConnection;
            List<DataReconciliationQueueContract> lstReconciliationItem = new List<DataReconciliationQueueContract>();
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("usp_GetNextActiveReconciledItem", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@InstitutionIds", selectedInstitutionIds);
                cmd.Parameters.AddWithValue("@ComplianceItemReconciliationDataID", ComplianceItemReconciliationDataID);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        lstReconciliationItem = ds.Tables[0].AsEnumerable().Select(col => new DataReconciliationQueueContract
                        {
                            FlatComplianceItemReconciliationDataID = (col["FlatComplianceItemReconciliationDataID"]) == DBNull.Value ? 0 : Convert.ToInt32(col["FlatComplianceItemReconciliationDataID"]),
                            ApplicantComplianceItemId = (col["ApplicantComplianceItemId"]) == DBNull.Value ? 0 : Convert.ToInt32(col["ApplicantComplianceItemId"]),
                            ApplicantComplianceCategoryId = (col["ApplicantComplianceCategoryId"]) == DBNull.Value ? 0 : Convert.ToInt32(col["ApplicantComplianceCategoryId"]),
                            CategoryID = (col["CategoryID"]) == DBNull.Value ? 0 : Convert.ToInt32(col["CategoryID"]),
                            ComplianceItemId = (col["ComplianceItemId"]) == DBNull.Value ? 0 : Convert.ToInt32(col["ComplianceItemId"]),
                            PackageID = (col["PackageID"]) == DBNull.Value ? 0 : Convert.ToInt32(col["PackageID"]),
                            ApplicantId = (col["ApplicantId"]) == DBNull.Value ? 0 : Convert.ToInt32(col["ApplicantId"]),
                            PackageSubscriptionID = (col["PackageSubscriptionID"]) == DBNull.Value ? 0 : Convert.ToInt32(col["PackageSubscriptionID"]),
                            TenantId = (col["TenantId"]) == DBNull.Value ? 0 : Convert.ToInt32(col["TenantId"])
                        }).ToList();
                    }
                }
            }
            return lstReconciliationItem;
        }
        #endregion

        //CBI || CABS // Method to get suffixes from lkpSuffix
        List<lkpSuffix> ISecurityRepository.GetSuffixes()
        {
            return _dbNavigation.lkpSuffixes.Where(cond => !cond.IsDeleted).ToList();
        }

        List<lkpAdminEntrySuffix> ISecurityRepository.GetAdminEntrySuffixes()
        {
            return _dbNavigation.lkpAdminEntrySuffixes.Where(cond => !cond.LAES_IsDeleted).ToList();
        }

        #region UAT-3824 :-Code to save user language mapping

        Boolean ISecurityRepository.AddUpdateLanguageMapping(OrganizationUser orgUser, Int32? SelectedCommLang)
        {
            if (SelectedCommLang == AppConsts.NONE || SelectedCommLang.IsNullOrEmpty())
            {
                var defaultLanguageCode = CommunicationLanguages.DEFAULT.GetStringValue();
                SelectedCommLang = _dbNavigation.lkpLanguages.FirstOrDefault(con => con.LAN_Code == defaultLanguageCode && !con.LAN_IsDeleted).LAN_ID;
            }
            //Get UserId
            var userobject = _dbNavigation.OrganizationUsers.FirstOrDefault(cond => cond.OrganizationUserID == orgUser.OrganizationUserID && !cond.IsDeleted);
            if (userobject.IsNotNull())
            {
                Guid userid = userobject.UserID;

                //Check already Exist or not
                UserLanguageMapping user = _dbNavigation.UserLanguageMappings.Where(cond => cond.ULM_UserId == userid).FirstOrDefault();
                //update
                if (!user.IsNullOrEmpty())
                {
                    user.ULM_LanguageId = SelectedCommLang;
                    user.ULM_ModifiedBy = orgUser.OrganizationUserID;
                    user.ULM_ModifiedOn = DateTime.Now;
                }
                //add
                else
                {
                    user = new UserLanguageMapping();
                    user.ULM_UserId = userid;
                    user.ULM_LanguageId = SelectedCommLang;
                    user.ULM_CreatedOn = DateTime.Now;
                    user.ULM_CreatedBy = orgUser.OrganizationUserID;
                    user.ULM_IsDeleted = false;
                    _dbNavigation.UserLanguageMappings.AddObject(user);
                }

                if (_dbNavigation.SaveChanges() > AppConsts.NONE)
                    return true;
            }
            return false;
        }

        Int32? ISecurityRepository.GetSelectedlang(Guid UserID)
        {
            UserLanguageMapping UserMappinglanID = _dbNavigation.UserLanguageMappings.FirstOrDefault(cond => cond.ULM_UserId.HasValue && cond.ULM_UserId.Value == UserID && !cond.ULM_IsDeleted);

            if (!UserMappinglanID.IsNullOrEmpty())
            {
                return UserMappinglanID.ULM_LanguageId;
            }
            return null;
        }

        Int32 ISecurityRepository.GetLanguageIdByGuid(Int32? OrganisationUserId, Int32? TenantId, out Int32 defaultLanguageId)
        {
            //get value from cache if possible
            defaultLanguageId = 0;
            //get default value code
            var defaultLanguageCode = CommunicationLanguages.DEFAULT.GetStringValue();
            // get default langugae id by default language code
            Int32 defaultLanId = _dbNavigation.lkpLanguages.Where(x => x.LAN_Code == defaultLanguageCode && !x.LAN_IsDeleted).FirstOrDefault().LAN_ID;
            defaultLanguageId = defaultLanId;
            //// return user specified language id, if language specified id is not available than return default language id
            if (OrganisationUserId.HasValue && OrganisationUserId.Value > AppConsts.NONE && TenantId.HasValue && TenantId > AppConsts.NONE)
            {
                var organizationUserData = _dbNavigation.OrganizationUsers.Where(x => x.OrganizationUserID == OrganisationUserId && x.Organization.TenantID == TenantId && !x.IsDeleted).FirstOrDefault();
                if (!organizationUserData.IsNullOrEmpty())
                {
                    var userID = organizationUserData.UserID;
                    var UserLanguage = _dbNavigation.UserLanguageMappings.Where(x => x.ULM_UserId == userID && !x.ULM_IsDeleted).FirstOrDefault();
                    if (!UserLanguage.IsNullOrEmpty())
                        return UserLanguage.ULM_LanguageId.HasValue ? UserLanguage.ULM_LanguageId.Value : defaultLanId;
                }
            }
            return defaultLanId;
        }

        int ISecurityRepository.GetSuffixIdBasedOnSuffixText(string suffix)
        {
            EntityConnection connection = _dbNavigation.Connection as EntityConnection;

            SqlParameter outputParam = new SqlParameter("@ReturnValue", SqlDbType.Int) { Direction = ParameterDirection.Output };
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("dbo.usp_GetSuffixIdBySuffixText", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@suffix", suffix);
                //command.Parameters.AddWithValue("@CurrentLoggedInUserId", currentLoggedInUserId);
                command.Parameters.Add(outputParam);
                if (con.State == ConnectionState.Closed)
                    con.Open();

                command.ExecuteNonQuery();
                con.Close();
            }
            return Convert.ToInt32(outputParam.Value);

        }
        string ISecurityRepository.GetCountryByCountryId(int countryId)
        {
            return _dbNavigation.CBICountryDatas.Where(cond => cond.Id == countryId).Select(cond => cond.StateType).FirstOrDefault();
        }
        #endregion

        #region UAT-3910
        List<LookupContract> ISecurityRepository.GetLocationSpecifictenantAllCountriesList(Boolean isStateSearch, Int32 countryId)
        {
            List<LookupContract> lstLookupContract = new List<LookupContract>();

            EntityConnection connection = _dbNavigation.Connection as EntityConnection;

            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter("@IsStateSearch", isStateSearch),
                             new SqlParameter("@CountryId", countryId)
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetLocationSpecificCountryOrState", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            LookupContract lookupContract = new LookupContract();
                            lookupContract.Name = dr["Name"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Name"]);
                            lookupContract.ID = dr["Id"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["Id"]);
                            lstLookupContract.Add(lookupContract);
                        }
                    }
                }
                base.CloseSQLDataReaderConnection(con);
            }
            return lstLookupContract;
        }
        #endregion

        #region Globalization Repo Methods

        lkpLanguage ISecurityRepository.GetLanguageCulture(Guid userId)
        {
            //String userLanguageCulture = LanguageCultures.ENGLISH_CULTURE.GetStringValue();
            UserLanguageMapping userLanguageMapping = _dbNavigation.UserLanguageMappings.Where(cond => !cond.ULM_IsDeleted && cond.ULM_UserId == userId).FirstOrDefault();
            if (!userLanguageMapping.IsNullOrEmpty())
            {
                return userLanguageMapping.lkpLanguage;
            }
            return null;
        }

        #endregion

        String ISecurityRepository.GetUserPreferLanguageCode(Int32 UserId, Int32 TenantId)
        {
            var defaultLanguageCode = Languages.ENGLISH.GetStringValue();
            if (UserId > AppConsts.NONE && TenantId > AppConsts.NONE)
            {
                var userID = _dbNavigation.OrganizationUsers.Where(x => x.OrganizationUserID == UserId && x.Organization.TenantID == TenantId && !x.IsDeleted).FirstOrDefault().UserID;
                var UserLanguage = _dbNavigation.UserLanguageMappings.Where(x => x.ULM_UserId == userID && !x.ULM_IsDeleted).FirstOrDefault();
                if (!UserLanguage.IsNullOrEmpty())
                    return UserLanguage.ULM_LanguageId.HasValue ? UserLanguage.lkpLanguage.LAN_Code : defaultLanguageCode;
            }
            return defaultLanguageCode;
        }

        #region UAT-4097
        Boolean ISecurityRepository.IsUserExixtInLocationTenants(Guid currentUserId)
        {
            Boolean isCBITenantExixts = false;
            List<Int32> lstTenantIds = _dbNavigation.OrganizationUsers.Where(con => con.UserID == currentUserId && con.IsDeleted == false && con.Organization.TenantID != null)
                            .Select(sel => sel.Organization.TenantID.Value).ToList();
            if (!lstTenantIds.IsNullOrEmpty() && lstTenantIds.Count > AppConsts.NONE)
            {
                List<String> locationServiceTenantIds = _dbNavigation.AppConfigurations.Where(cond => cond.AC_Key == "LocationServiceTenantIds").FirstOrDefault().AC_Value.Split(',').ToList();
                if (!locationServiceTenantIds.IsNullOrEmpty() && locationServiceTenantIds.Count > AppConsts.NONE)
                {
                    foreach (Int32 tenantId in lstTenantIds)
                    {
                        if (locationServiceTenantIds.Contains(tenantId.ToString()))
                        {
                            isCBITenantExixts = true;
                            break;
                        }
                    }
                }
            }
            return isCBITenantExixts;
        }

        Boolean ISecurityRepository.IsPasswordNeedToBeChanged(Guid userId, Int32 expiryDays)
        {
            DateTime lastUpdatedDate;
            //List<PasswordHistory> lstPasswordHistory = CompiledGetPasswordHistoryByUserId(_dbNavigation, userId).ToList();

            //if (!lstPasswordHistory.IsNullOrEmpty())
            //{
            //    lastUpdatedDate = lstPasswordHistory.OrderByDescending(n => n.PasswordChangedDate).Take(1).Select(sel => sel.CreatedOn).FirstOrDefault();
            //}
            //else
            //{
            lastUpdatedDate = _dbNavigation.aspnet_Membership.Where(con => con.UserId == userId).Select(sel => sel.LastPasswordChangedDate).FirstOrDefault();
            //}
            if (!lastUpdatedDate.IsNullOrEmpty())
            {
                if ((DateTime.Now - lastUpdatedDate).TotalDays >= expiryDays)
                {
                    return true;
                }
            }
            return false;
        }

        //UAT-4013
        /// <summary>
        /// Get Rotation Student details for Instructor Preceptor
        /// </summary>
        /// <param name="tenantID"></param>
        /// <param name="userID"></param>
        /// <param name="searchContract"></param>
        /// <param name="customPagingArgsContract"></param>
        /// <returns></returns>
        List<RotationMemberSearchDetailContract> ISecurityRepository.GetInstrctrPreceptrRotationStudents(String tenantID, Guid userID, RotationMemberSearchDetailContract searchContract, CustomPagingArgsContract customPagingArgsContract)
        {
            List<RotationMemberSearchDetailContract> rotationDetailList = new List<RotationMemberSearchDetailContract>();
            EntityConnection connection = _dbNavigation.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter("@searchDataXML", searchContract.XML),
                             new SqlParameter("@customFilteringXML", customPagingArgsContract.XML),
                             new SqlParameter("@UserID", userID),
                             new SqlParameter("@TenantID", tenantID)
                        };

                base.OpenSQLDataReaderConnection(con);
                using (SqlDataReader col = base.ExecuteSQLDataReader(con, "usp_GetRotationStudentSearchData", sqlParameterCollection))
                {
                    while (col.Read())
                    {
                        RotationMemberSearchDetailContract rotationDetail = new RotationMemberSearchDetailContract();

                        rotationDetail.RotationID = col["ClinicalRotationID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["ClinicalRotationID"]);
                        rotationDetail.ComplioID = col["ComplioID"] == DBNull.Value ? String.Empty : Convert.ToString(col["ComplioID"]);
                        rotationDetail.RotationName = col["RotationName"] == DBNull.Value ? String.Empty : Convert.ToString(col["RotationName"]);
                        rotationDetail.Department = col["Department"] == DBNull.Value ? String.Empty : Convert.ToString(col["Department"]);
                        rotationDetail.Program = col["Program"] == DBNull.Value ? String.Empty : Convert.ToString(col["Program"]);
                        rotationDetail.Course = col["Course"] == DBNull.Value ? String.Empty : Convert.ToString(col["Course"]);
                        rotationDetail.StartDate = col["StartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(col["StartDate"]);
                        rotationDetail.EndDate = col["EndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(col["EndDate"]);
                        rotationDetail.UnitFloorLoc = col["UnitFloorLoc"] == DBNull.Value ? String.Empty : Convert.ToString(col["UnitFloorLoc"]);
                        rotationDetail.RecommendedHours = col["NoOfHours"] == DBNull.Value ? (float?)null : (float?)(Convert.ToDouble(col["NoOfHours"]));
                        rotationDetail.Shift = col["RotationShift"] == DBNull.Value ? String.Empty : Convert.ToString(col["RotationShift"]);
                        rotationDetail.StartTime = col["StartTime"] == DBNull.Value ? (TimeSpan?)null : (TimeSpan?)(col["StartTime"]);
                        rotationDetail.EndTime = col["EndTime"] == DBNull.Value ? (TimeSpan?)null : (TimeSpan?)(col["EndTime"]);
                        rotationDetail.Time = col["Times"] == DBNull.Value ? String.Empty : Convert.ToString(col["Times"]);
                        rotationDetail.AgencyName = col["AgencyName"] == DBNull.Value ? String.Empty : Convert.ToString(col["AgencyName"]);
                        rotationDetail.DaysName = col["Days"].GetType().Name == "DBNull" ? null : Convert.ToString(col["Days"]);
                        rotationDetail.Term = col["Term"] == DBNull.Value ? String.Empty : Convert.ToString(col["Term"]);
                        rotationDetail.TypeSpecialty = col["TypeSpecialty"] == DBNull.Value ? String.Empty : Convert.ToString(col["TypeSpecialty"]);
                        rotationDetail.ApplicantFirstName = col["ApplicantFirstName"] == DBNull.Value ? String.Empty : Convert.ToString(col["ApplicantFirstName"]);
                        rotationDetail.ApplicantLastName = col["ApplicantLastName"] == DBNull.Value ? String.Empty : Convert.ToString(col["ApplicantLastName"]);
                        rotationDetail.ApplicantSSN = col["ApplicantSSN"] == DBNull.Value ? String.Empty : Convert.ToString(col["ApplicantSSN"]);
                        rotationDetail.DateOfBirth = col["DateOfBirth"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(col["DateOfBirth"]);
                        rotationDetail.TotalRecordCount = col["TotalCount"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["TotalCount"]);

                        //UAT-4013
                        rotationDetail.SlctdAgencyID = col["AgencyID"] == DBNull.Value ? String.Empty : Convert.ToString(col["AgencyID"]);
                        rotationDetail.OrganizationUserID = col["ApplicantID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["ApplicantID"]);
                        rotationDetail.TenantID = col["TenantID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["TenantID"]);
                        rotationDetailList.Add(rotationDetail);
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }
            return rotationDetailList;
        }

        #endregion

        //UAT-4013
        Dictionary<Int32, String> ISecurityRepository.GetOrgUserDetailsByOrgUserID(List<Int32> lstOrgUserIDs)
        {
            if (!lstOrgUserIDs.IsNullOrEmpty())
            {
                return _dbNavigation.OrganizationUsers.Where(x => lstOrgUserIDs.Contains(x.OrganizationUserID) && x.IsActive && !x.IsDeleted).ToDictionary(v => v.OrganizationUserID, v => String.Concat(v.FirstName, " ", v.LastName));
            }
            return null;
        }

        #region UAT-4114
        List<ManageRandomReviewsContract> ISecurityRepository.GetAllManageRandomReviewsList()
        {
            EntityConnection connection = _dbNavigation.Connection as EntityConnection;
            List<ManageRandomReviewsContract> lstRandomReviewContract = new List<ManageRandomReviewsContract>();
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("usp_GetAllManageRandomReviewsList", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        lstRandomReviewContract = ds.Tables[0].AsEnumerable().Select(col =>
                            new ManageRandomReviewsContract
                            {
                                ReconciliationQueueConfigurationID = col["ReconciliationQueueConfigurationID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["ReconciliationQueueConfigurationID"]),
                                TenantName = col["TenantName"] == DBNull.Value ? String.Empty : Convert.ToString(col["TenantName"]),
                                TenantID = col["TenantID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["TenantID"]),
                                HierarchyNodeID = col["HierarchyNodeID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["HierarchyNodeID"]),
                                InstitutionHierarchy = col["InstitutionHierarchy"] == DBNull.Value ? String.Empty : Convert.ToString(col["InstitutionHierarchy"]),
                                Description = col["Description"] == DBNull.Value ? String.Empty : Convert.ToString(col["Description"]),
                                Reviews = col["ReviewCount"] == DBNull.Value ? 0 : Convert.ToInt32(col["ReviewCount"]),
                                Percentage = col["RecordPercentage"] == DBNull.Value ? (Decimal?)null : Convert.ToDecimal(col["RecordPercentage"])
                            }).ToList();
                    }
                }
            }
            return lstRandomReviewContract;
        }
        #endregion

        List<ExternalLoginDataContract> ISecurityRepository.GetMatchingOrganisationUserListForCoreLinking(ExternalLoginDataContract objExternalLoginDataContract)
        {

            EntityConnection connection = _dbNavigation.Connection as EntityConnection;


            List<ExternalLoginDataContract> matchingUserSearchData = new List<ExternalLoginDataContract>();
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("api.usp_GetMatchingApplicants_CORE", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@XmlData", objExternalLoginDataContract.CreateXml());
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        matchingUserSearchData = ds.Tables[0].AsEnumerable().Select(col =>
                            new ExternalLoginDataContract
                            {
                                FirstName = col["FirstName"] == DBNull.Value ? String.Empty : Convert.ToString(col["FirstName"]),
                                LastName = col["LastName"] == DBNull.Value ? String.Empty : Convert.ToString(col["LastName"]),
                                UserName = col["UserName"] == DBNull.Value ? String.Empty : Convert.ToString(col["UserName"]),
                                Email1 = col["PrimaryEmailAddress"] == DBNull.Value ? String.Empty : Convert.ToString(col["PrimaryEmailAddress"]),
                                Email2 = col["SecondaryEmailAddress"] == DBNull.Value ? String.Empty : Convert.ToString(col["SecondaryEmailAddress"]),
                                PhoneNumber = col["PhoneNumber"] == DBNull.Value ? String.Empty : Convert.ToString(col["PhoneNumber"]),
                                OrganizationUserID = col["OrganizationUserID"] == DBNull.Value ? 0 : Convert.ToInt32(col["OrganizationUserID"]),
                                OrganizationID = col["OrganizationID"] == DBNull.Value ? 0 : Convert.ToInt32(col["OrganizationID"]),
                                SSN = col["SSN"] == DBNull.Value ? String.Empty : Convert.ToString(col["SSN"]),
                                TenantID = col["TenantID"] == DBNull.Value ? 0 : Convert.ToInt32(col["TenantID"]),
                                TenantName = col["TenantName"] == DBNull.Value ? String.Empty : Convert.ToString(col["TenantName"])
                            }).ToList();
                    }
                }
            }
            return matchingUserSearchData;
        }

        #region UAT-4151

        Boolean ISecurityRepository.LinkOtherAccount(Guid SourceUserID, Guid TargetUserID, Int32 currentLoggedInUserId)
        {
            EntityConnection connection = _dbNavigation.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("usp_LinkOtherUserAccount", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SourceUserID", SourceUserID);
                cmd.Parameters.AddWithValue("@TargetUserId", TargetUserID);
                cmd.Parameters.AddWithValue("@CurrentLoggedInUserID", currentLoggedInUserId);
                cmd.ExecuteScalar();
                return true;
            }
        }

        /// <summary>
        /// In below method we are fetching those users who can be linked (having same first name, last name and same recovery email address which users wants to link) in LookupContract
        /// After fetching above unlocked records we check whether any role of wanna link to be user(user which we get from email) has any inactive roles in boolean of tuple
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <param name="userid"></param>
        /// <param name="currentLoggedInUserId"></param>
        /// <returns></returns>
        Tuple<LookupContract, Boolean> ISecurityRepository.GetExistingUserBasedOnEmailId(String emailAddress, Guid userid, Int32 currentLoggedInUserId)
        {
            LookupContract lookupContract = new LookupContract();
            Boolean isAnyRoleInActive = false;

            Boolean isCurrentLoggedInUserHaveApplicantRole = false;
            Boolean isCurrentLoggedInUserHaveClientAdminRole = false;
            Boolean isCurrentLoggedInUserhaveAgencyUser = false;
            Boolean isCurrentLoggedInUserHaveSharedUser = false;
            Boolean isCurrentLoggedInUserHaveInstructorPreceptor = false;

            List<Int32> lstClientContactsTenantIds = new List<Int32>();

            List<OrganizationUser> lstcurrentLoggedInUser = _dbNavigation.OrganizationUsers.Where(con => con.UserID == userid && !con.IsDeleted && !con.aspnet_Users.aspnet_Membership.IsLockedOut).ToList();
            if (!lstcurrentLoggedInUser.IsNullOrEmpty())
            {
                isCurrentLoggedInUserHaveApplicantRole = !lstcurrentLoggedInUser.Where(con => con.IsApplicant.HasValue && con.IsApplicant == true).IsNullOrEmpty();

                isCurrentLoggedInUserHaveClientAdminRole = lstcurrentLoggedInUser.Where(con => (!con.IsSharedUser.HasValue || con.IsSharedUser.Value == false)
                                                    && (!con.IsApplicant.HasValue || con.IsApplicant.Value == false)
                                                    && con.OrganizationID != AppConsts.ONE).IsNullOrEmpty() ? false : true;

                var ouSharedUser = lstcurrentLoggedInUser.Where(con => con.IsSharedUser.HasValue && con.IsSharedUser == true).FirstOrDefault();
                if (!ouSharedUser.IsNullOrEmpty() && !ouSharedUser.OrganizationUserTypeMappings.IsNullOrEmpty())
                {
                    List<OrganizationUserTypeMapping> lstOrganizationUserTypeMapping = ouSharedUser.OrganizationUserTypeMappings.Where(con => con.OTM_IsDeleted.HasValue
                                                                                        && !con.OTM_IsDeleted.Value).ToList();
                    if (!lstOrganizationUserTypeMapping.IsNullOrEmpty())
                    {
                        foreach (OrganizationUserTypeMapping item in lstOrganizationUserTypeMapping)
                        {
                            if (item.lkpOrgUserType.OrgUserTypeCode == OrganizationUserType.AgencyUser.GetStringValue())
                            {
                                isCurrentLoggedInUserhaveAgencyUser = true;
                            }
                            else if (item.lkpOrgUserType.OrgUserTypeCode == OrganizationUserType.ApplicantsSharedUser.GetStringValue())
                            {
                                isCurrentLoggedInUserHaveSharedUser = true;
                            }
                            else if (item.lkpOrgUserType.OrgUserTypeCode == OrganizationUserType.Instructor.GetStringValue()
                                || item.lkpOrgUserType.OrgUserTypeCode == OrganizationUserType.Preceptor.GetStringValue())
                            {
                                isCurrentLoggedInUserHaveInstructorPreceptor = true;
                                lstClientContactsTenantIds.AddRange(
                                  SharedDataDBContext.ClientContacts.Where(con => con.CC_UserID == userid && !con.CC_IsDeleted).Distinct().Select(sel => sel.CC_TenantID).ToList());
                            }
                            //else if (item.lkpOrgUserType.OrgUserTypeCode == OrganizationUserType.Preceptor.GetStringValue())
                            //{
                            //    isCurrentLoggedInUserHaveInstructorPreceptor = true;
                            //}
                        }
                    }
                }
            }

            //if (isCurrentLoggedInUserHaveInstructorPreceptor)
            //{
            //    lstClientContactsTenantIds.AddRange(
            //    SharedDataDBContext.ClientContacts.Where(con => con.CC_UserID == userid && !con.CC_IsDeleted).Select(sel => sel.CC_TenantID).ToList());
            //}

            String firstName = lstcurrentLoggedInUser.Where(con => con.OrganizationUserID == currentLoggedInUserId).Select(sel => sel.FirstName).FirstOrDefault();
            String lastName = lstcurrentLoggedInUser.Where(con => con.OrganizationUserID == currentLoggedInUserId).Select(sel => sel.LastName).FirstOrDefault();

            if (!emailAddress.IsNullOrEmpty() && !firstName.IsNullOrEmpty() && !lastName.IsNullOrEmpty())
            {
                List<OrganizationUser> lstExistingOrgUsers = _dbNavigation.OrganizationUsers.Where(con => con.FirstName == firstName
                                                                && con.LastName == lastName && !con.IsDeleted && !con.aspnet_Users.aspnet_Membership.IsLockedOut).ToList();

                if (!lstExistingOrgUsers.IsNullOrEmpty() && lstExistingOrgUsers.Count > AppConsts.NONE)
                {
                    lstExistingOrgUsers = lstExistingOrgUsers.Where(con => con.aspnet_Users.aspnet_Membership.LoweredEmail == emailAddress.ToLower()).ToList();

                    if (lstExistingOrgUsers.Any(con => !con.IsActive))
                        isAnyRoleInActive = true;

                    if (!lstExistingOrgUsers.IsNullOrEmpty())
                    {
                        List<Guid> lstUserIdsWhichNeedTobeemovedFromFinal = new List<Guid>();
                        if (isCurrentLoggedInUserHaveApplicantRole)
                        {
                            //organization Ids for which currentLoggedIn applicant belongs to
                            List<Int32> lstOrganizationIds = lstcurrentLoggedInUser.Where(con => con.IsApplicant.HasValue && con.IsApplicant == true).Select(sel => sel.OrganizationID).ToList();
                            lstUserIdsWhichNeedTobeemovedFromFinal = lstExistingOrgUsers.Where(con => lstOrganizationIds.Any(l => l == con.OrganizationID)
                                                                                            && con.IsApplicant.HasValue && con.IsApplicant == true).Select(sel => sel.UserID).ToList();
                        }
                        if (isCurrentLoggedInUserHaveClientAdminRole)
                        {
                            //organization Ids for which currentLoggedIn clientadmin belongs to
                            List<Int32> lstOrganizationIds = lstcurrentLoggedInUser.Where(con => (!con.IsSharedUser.HasValue || con.IsSharedUser.Value == false)
                                                    && (con.IsApplicant.Value.IsNull() || con.IsApplicant.Value == false)
                                                    && con.OrganizationID != AppConsts.ONE).Select(sel => sel.OrganizationID).ToList();

                            lstUserIdsWhichNeedTobeemovedFromFinal.AddRange(lstExistingOrgUsers.Where(con => lstOrganizationIds.Contains(con.OrganizationID)
                                                                                            && (!con.IsSharedUser.HasValue || con.IsSharedUser.Value == false)
                                                                                            && (!con.IsApplicant.HasValue || con.IsApplicant.Value == false)
                                                                                            && con.OrganizationID != AppConsts.ONE).Select(sel => sel.UserID).ToList());
                        }
                        if (isCurrentLoggedInUserhaveAgencyUser || isCurrentLoggedInUserHaveSharedUser || isCurrentLoggedInUserHaveInstructorPreceptor)
                        {
                            List<OrganizationUser> lstOrgUsersSharedUsers = lstExistingOrgUsers.Where(con => con.IsSharedUser.HasValue && con.IsSharedUser == true).ToList();
                            if (!lstOrgUsersSharedUsers.IsNullOrEmpty())
                            {
                                if (isCurrentLoggedInUserhaveAgencyUser)
                                {
                                    List<OrganizationUser> lstOUSharedUsers = lstOrgUsersSharedUsers.Where(cond => !cond.IsDeleted && cond.OrganizationUserTypeMappings.Any(con => con.OTM_IsDeleted == false
                                                                                                        && con.lkpOrgUserType.OrgUserTypeCode == OrganizationUserType.AgencyUser.GetStringValue())).ToList();
                                    if (!lstOUSharedUsers.IsNullOrEmpty() && lstOUSharedUsers.Any())
                                        lstUserIdsWhichNeedTobeemovedFromFinal.AddRange(lstOUSharedUsers.Select(sel => sel.UserID).ToList());

                                }
                                if (isCurrentLoggedInUserHaveSharedUser)
                                {
                                    List<OrganizationUser> lstOUApplicantsSharedUser = lstOrgUsersSharedUsers.Where(cond => !cond.IsDeleted && cond.OrganizationUserTypeMappings.Any(con => con.OTM_IsDeleted == false
                                                                                                           && con.lkpOrgUserType.OrgUserTypeCode == OrganizationUserType.ApplicantsSharedUser.GetStringValue())).ToList();

                                    if (!lstOUApplicantsSharedUser.IsNullOrEmpty() && lstOUApplicantsSharedUser.Any())
                                        lstUserIdsWhichNeedTobeemovedFromFinal.AddRange(lstOUApplicantsSharedUser.Select(sel => sel.UserID).ToList());
                                }
                                if (isCurrentLoggedInUserHaveInstructorPreceptor)
                                {
                                    List<OrganizationUser> lstOUInstructorPreceptor = lstOrgUsersSharedUsers.Where(cond => !cond.IsDeleted && cond.OrganizationUserTypeMappings.Any(con => con.OTM_IsDeleted == false
                                                                                                           && (con.lkpOrgUserType.OrgUserTypeCode == OrganizationUserType.Instructor.GetStringValue()
                                                                                                           || con.lkpOrgUserType.OrgUserTypeCode == OrganizationUserType.Preceptor.GetStringValue()))).ToList();

                                    if (!lstOUInstructorPreceptor.IsNullOrEmpty() && lstOUInstructorPreceptor.Any())
                                    {
                                        List<Guid> lstGuids = lstOUInstructorPreceptor.Select(sel => sel.UserID).ToList();

                                        lstClientContactsTenantIds = lstClientContactsTenantIds.Distinct().ToList();
                                        List<Guid> lstAlreadyCreatedInstructors = SharedDataDBContext.ClientContacts.Where(con => con.CC_UserID.Value != null && lstGuids.Contains(con.CC_UserID.Value)
                                                                                   && lstClientContactsTenantIds.Contains(con.CC_TenantID) && !con.CC_IsDeleted)
                                                                                                       .Select(sel => sel.CC_UserID.Value).ToList();

                                        if (!lstAlreadyCreatedInstructors.IsNullOrEmpty() && lstAlreadyCreatedInstructors.Any())
                                            lstUserIdsWhichNeedTobeemovedFromFinal.AddRange(lstAlreadyCreatedInstructors);

                                        //if (!lstOUInstructorPreceptor.IsNullOrEmpty() && lstOUInstructorPreceptor.Any())
                                        //    lstUserIdsWhichNeedTobeemovedFromFinal.AddRange(lstOUInstructorPreceptor.Select(sel => sel.UserID).ToList());

                                    }
                                }
                            }
                        }
                        //Remove adb admins from list
                        List<Guid> lstAdbAdmins = lstExistingOrgUsers.Where(con => con.OrganizationID == AppConsts.ONE
                                                                                        && (con.IsSharedUser == null || con.IsSharedUser.Value == false)
                                                                                          && (con.IsApplicant == null || con.IsApplicant.Value == false) && !con.IsDeleted)
                                                                                          .Select(sel => sel.UserID).ToList();
                        if (!lstAdbAdmins.IsNullOrEmpty() && lstAdbAdmins.Any())
                            lstUserIdsWhichNeedTobeemovedFromFinal.AddRange(lstAdbAdmins);


                        lstExistingOrgUsers.RemoveAll(x => lstUserIdsWhichNeedTobeemovedFromFinal.Contains(x.UserID));

                        if (!lstExistingOrgUsers.IsNullOrEmpty() && lstExistingOrgUsers.Any())
                        {
                            lstExistingOrgUsers.DistinctBy(x => x.UserID).ForEach(x =>
                            {
                                lookupContract.Name = "Please enter password for '" + x.aspnet_Users.UserName + "'";
                                lookupContract.Code = x.aspnet_Users.UserName;
                                lookupContract.ID = x.OrganizationID;
                                lookupContract.UserID = x.OrganizationUserID;
                                lookupContract.OrganizationUserId = x.aspnet_Users.UserId;
                            });
                        }
                    }
                }
            }
            var _info = new Tuple<LookupContract, Boolean>(lookupContract, isAnyRoleInActive);
            return _info;
        }
        //UAT-4407:- Currently we will not copy data on linking
        Boolean ISecurityRepository.RotationDataMovementOnAccountLinking(Guid SourceUserID, int CurrentUserLogIn)
        {
            EntityConnection connection = _dbNavigation.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("usp_RotationDataMovementOnAccountLinking", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", SourceUserID);
                cmd.Parameters.AddWithValue("@CurrentLoggedInUserID", CurrentUserLogIn);
                cmd.ExecuteScalar();
                return true;
            }
        }
        Boolean ISecurityRepository.IsOrganizationUserExistsForEmail(String emailAddress)
        {
            return _dbNavigation.OrganizationUsers.Where(con => !con.IsDeleted && con.aspnet_Users.aspnet_Membership.Email == emailAddress).Any();
        }

        #endregion

        #region UAT-4239
        OrganizationUser ISecurityRepository.GetSharedUserOrganizationUser(Guid userId)
        {
            return _dbNavigation.OrganizationUsers.Where(cond => cond.UserID == userId && !cond.IsDeleted && cond.IsSharedUser == true).FirstOrDefault();
        }

        #endregion

        #region UAT-4270
        Boolean ISecurityRepository.GetNotificationNeedToSendForEnroller(Int32 OrgUserID)
        {
            return _dbNavigation.OrganizationUsers.Where(cond => cond.OrganizationUserID == OrgUserID && !cond.IsDeleted).Any();
        }
        String ISecurityRepository.GetEnrollerPhoneNumberForSMSNotification(Int32 OrgUserID)
        {
            OrganizationUser OrgUserDetails = _dbNavigation.OrganizationUsers.Where(cond => cond.OrganizationUserID == OrgUserID && !cond.IsDeleted && cond.aspnet_Users != null).FirstOrDefault();
            if (!OrgUserDetails.IsNullOrEmpty())
            {
                return OrgUserDetails.aspnet_Users.MobileAlias;
            }
            return String.Empty;

        }
        #endregion



        #region UAT 4025
        /// <summary>
        /// gets the pipe seperated CBI Unique IDs
        /// </summary>
        /// <param name="CBIUniqueID"></param>
        /// <returns></returns>
        String ISecurityRepository.ValidateCBIUniqueID(List<String> lstCBIUniqueID)
        {

            List<String> _ValidCBIUniqueIDs = _dbNavigation.CascadingAttributeOptions.Where(cond => cond.BkgSvcAttribute.BSA_Name == "CBIUniqueID"
                                                       && !cond.BkgSvcAttribute.BSA_IsDeleted
                                                       && lstCBIUniqueID.Contains(cond.CAO_Value)
                                                       && !cond.CAO_IsDeleted).Select(sel => sel.CAO_Value).ToList();
            List<String> InValidCBIUniqueID = new List<String>();
            InValidCBIUniqueID = lstCBIUniqueID.Where(cond => !_ValidCBIUniqueIDs
            .Any(vc => vc.Equals(cond,
            StringComparison.InvariantCultureIgnoreCase)))
            .ToList();

            return String.Join(",", InValidCBIUniqueID).ToString();
        }

        /// <summary>
        /// gets the pipe seperated CBI Unique IDs
        /// </summary>
        /// <param name="CBIUniqueID"></param>
        /// <returns></returns>
        String ISecurityRepository.ValidateAccountName(List<String> lstAccountName)
        {

            List<String> _ValidAccountNames = _dbNavigation.CascadingAttributeOptions.Where(cond => cond.BkgSvcAttribute.BSA_Name == "AcctNam (Literal)"
                                                       && !cond.BkgSvcAttribute.BSA_IsDeleted
                                                       && lstAccountName.Any(la => la == cond.CAO_Value)
                                                       && !cond.CAO_IsDeleted).Select(sel => sel.CAO_Value).ToList();

            List<String> InValidAccountNames = new List<String>();

            InValidAccountNames = lstAccountName.Where(cond => !_ValidAccountNames
                .Any(va => va.Equals(cond,
                StringComparison.InvariantCultureIgnoreCase)))
                .ToList();

            return String.Join(",", InValidAccountNames).ToString();
        }



        #endregion

        #region Admin Entry Portal

        CrossApplicationData ISecurityRepository.GetSessionUsingToken(Guid? token)
        {
            CrossApplicationData applicationData = new CrossApplicationData();
            applicationData = _dbNavigation.CrossApplicationDatas.Where(con => con.CAD_IsActive && con.CAD_Token == token).FirstOrDefault();
            return applicationData;
        }


        Boolean ISecurityRepository.SetSessionData(CrossApplicationData applicationData)
        {

            if (!applicationData.IsNullOrEmpty())
                _dbNavigation.CrossApplicationDatas.AddObject(applicationData);
            if (_dbNavigation.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }

        void ISecurityRepository.UpdateSessionActiveState(Guid? token)
        {
            if (token != null)
            {
                CrossApplicationData applicationData = new CrossApplicationData();
                applicationData = _dbNavigation.CrossApplicationDatas.Where(con => con.CAD_IsActive && con.CAD_Token == token).FirstOrDefault();
                applicationData.CAD_IsActive = false;

                _dbNavigation.SaveChanges();
            }
        }
        AdminEntryUserLoginContract ISecurityRepository.GetAdminEntryUserByToken(Guid tokenKey)
        {
            EntityConnection connection = _dbNavigation.Connection as EntityConnection;
            AdminEntryUserLoginContract applicantData = new AdminEntryUserLoginContract();
            applicantData.organizationUser = new OrganizationUser();

            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter("@AdminEntryUserToken", tokenKey)

                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "ams.GetAdminEntryApplicantByToken", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            applicantData.organizationUser.OrganizationUserID = dr["OrganizationUserID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["OrganizationUserID"]);
                            applicantData.organizationUser.UserID = dr["UserID"] == DBNull.Value ? new Guid() : new Guid(dr["UserID"].ToString());
                            applicantData.organizationUser.OrganizationID = dr["OrganizationID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["OrganizationID"]);
                            applicantData.organizationUser.SysXBlockID = dr["SysXBlockID"] == DBNull.Value ? (int?)0 : Convert.ToInt32(dr["SysXBlockID"]);
                            applicantData.organizationUser.FirstName = Convert.ToString(dr["FirstName"]);
                            applicantData.organizationUser.LastName = Convert.ToString(dr["LastName"]);
                            applicantData.organizationUser.OfficeReturnDateTime = dr["OfficeReturnDateTime"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["OfficeReturnDateTime"]);
                            applicantData.organizationUser.IsOutOfOffice = dr["IsOutOfOffice"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsOutOfOffice"]);
                            applicantData.organizationUser.IsNewPassword = dr["IsNewPassword"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsNewPassword"]);
                            applicantData.organizationUser.IgnoreIPRestriction = dr["IgnoreIPRestriction"] == DBNull.Value ? false : Convert.ToBoolean(dr["IgnoreIPRestriction"]);
                            applicantData.organizationUser.IsSystem = dr["IsSystem"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsSystem"]);
                            applicantData.organizationUser.IsDeleted = dr["IsDeleted"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsDeleted"]);
                            applicantData.organizationUser.IsActive = dr["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsActive"]);
                            applicantData.organizationUser.CreatedByID = dr["CreatedByID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["CreatedByID"]);
                            applicantData.organizationUser.CreatedOn = dr["CreatedOn"] == DBNull.Value ? new DateTime() : Convert.ToDateTime(dr["CreatedOn"]);
                            applicantData.organizationUser.ModifiedByID = dr["ModifiedByID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ModifiedByID"]);
                            applicantData.organizationUser.ModifiedOn = dr["ModifiedOn"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["ModifiedOn"]);
                            applicantData.organizationUser.IsApplicant = dr["IsApplicant"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsApplicant"]);
                            applicantData.organizationUser.PrimaryEmailAddress = Convert.ToString(dr["PrimaryEmailAddress"]);
                            applicantData.organizationUser.ActiveDate = dr["ActiveDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["ActiveDate"]);
                            applicantData.organizationUser.IsSharedUser = dr["IsSharedUser"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsSharedUser"]);
                            applicantData.OrderId = dr["OrderId"] == DBNull.Value ? 0 : Convert.ToInt32(dr["OrderId"]);
                            applicantData.TenantId = dr["TenantId"] == DBNull.Value ? 0 : Convert.ToInt32(dr["TenantId"]);
                        }
                    }
                    else
                    {
                        applicantData = null;
                    }
                }
                base.CloseSQLDataReaderConnection(con);
            }
            return applicantData;
        }

        Boolean ISecurityRepository.IsTokenExpired(Int32 tenantId, Int32 orderId, String TokenKey)
        {
            if (tenantId > AppConsts.NONE && orderId > AppConsts.NONE)
            {
                AdminEntryApplicantInviteTokenDetail applicantInviteTokenDetail = _dbNavigation.AdminEntryApplicantInviteTokenDetails
                                                                               .Where(Con => Con.AEAITD_IsActive && Con.AEAITD_TenantID == tenantId && Con.AEAITD_MasterOrderID == orderId
                                                                               && Con.AEAITD_Token == new Guid(TokenKey)
                                                                               && Con.AEAITD_CreatedOn <= DateTime.Now).ToList().FirstOrDefault();

                // .Where(cond => DateTime.Now <= cond.AEAITD_CreatedOn.AddDays(Convert.ToDouble(cond.AEAITD_Timespan))).FirstOrDefault();

                if (!applicantInviteTokenDetail.IsNullOrEmpty() && !applicantInviteTokenDetail.AEAITD_Timespan.IsNullOrEmpty() &&
                    DateTime.Now <= applicantInviteTokenDetail.AEAITD_CreatedOn.AddDays(Convert.ToDouble(applicantInviteTokenDetail.AEAITD_Timespan)))
                {
                    return false;
                }
            }
            return true;
        }

        Boolean ISecurityRepository.UpdateApplicatInviteToken(Int32 tenantId, Int32 orderId)
        {
            if (tenantId > AppConsts.NONE && orderId > AppConsts.NONE)
            {
                AdminEntryApplicantInviteTokenDetail applicantInviteTokenDetail = _dbNavigation.AdminEntryApplicantInviteTokenDetails
                                                                                .Where(Con => Con.AEAITD_IsActive && Con.AEAITD_TenantID == tenantId && Con.AEAITD_MasterOrderID == orderId).FirstOrDefault();

                if (!applicantInviteTokenDetail.IsNullOrEmpty())
                {
                    applicantInviteTokenDetail.AEAITD_IsActive = false;
                }
            }

            if (_dbNavigation.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }

        List<PersonAliasProfile> ISecurityRepository.GetUserPersonAliasProfiles(Int32 organizationUserProfileId)
        {
            return _dbNavigation.PersonAliasProfiles.Where(cond => cond.PAP_OrganizationUserProfileID == organizationUserProfileId && cond.PAP_IsDeleted == false).ToList();
        }
        #endregion

        #region UAT-4454
        Boolean ISecurityRepository.RemoveIntegrationClientOrganizationUserMapping(Int32 IntegrationClientOrganizationUserMapID, Int32 OrganizationUserID)
        {
            IntegrationClientOrganizationUserMap result = _dbNavigation.IntegrationClientOrganizationUserMaps.Where(con => con.ICOUM_ID == IntegrationClientOrganizationUserMapID && !con.ICOUM_IsDeleted).FirstOrDefault();
            if (result != null)
            {
                result.ICOUM_IsDeleted = true;
                result.ICOUM_ModifiedByID = OrganizationUserID;
                result.ICOUM_ModifiedOn = DateTime.Now;
                _dbNavigation.SaveChanges();
                return true;
            }
            return false;
        }
        #endregion

        /// </summary>
        /// <param name="organizationUserId">The value for organization user's id.</param>
        /// <returns>
        /// OrganizationUser.
        /// </returns>
        OrganizationUser ISecurityRepository.GetOrganizationUserDetailForAdminOrder(Int32 organizationUserId)
        {
            //OrganizationUser organizationUser = CompiledGetOrganizationUserDetail(_dbNavigation, organizationUserId);
            OrganizationUser organizationUser = _dbNavigation.OrganizationUsers.Where(bb => bb.OrganizationUserID == organizationUserId).FirstOrDefault();
            return !organizationUser.IsNull() ? organizationUser : null;
        }


        Boolean ISecurityRepository.CheckRoleForShibbolethNSC(Boolean IsApplicantRoleCheck, String Roles)
        {
            EntityConnection connection = _dbNavigation.Connection as EntityConnection;
            Boolean IsRolePresent = false;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[api].[CheckRoleForShibbolethNSC]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IsApplicantRoleCheck", IsApplicantRoleCheck);
                cmd.Parameters.AddWithValue("@Roles", Roles);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        IsRolePresent = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsRoleSpecified"]);
                    }
                }
            }
            return IsRolePresent;
        }

        /// <summary>
        /// Release 181:4998
        /// </summary>
        /// <param name="IsApplicantRoleCheck"></param>
        /// <param name="Roles"></param>
        /// <returns></returns>
        Boolean ISecurityRepository.CheckRoleForShibbolethRoss(Boolean IsApplicantRoleCheck, String Roles)
        {
            EntityConnection connection = _dbNavigation.Connection as EntityConnection;
            Boolean IsRolePresent = false;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[api].[CheckRoleForShibbolethRoss]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IsApplicantRoleCheck", IsApplicantRoleCheck);
                cmd.Parameters.AddWithValue("@Roles", Roles);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        IsRolePresent = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsRoleSpecified"]);
                    }
                }
            }
            return IsRolePresent;
        }

        Boolean ISecurityRepository.CheckRoleForShibbolethUpennDental(Boolean IsApplicantRoleCheck, String Roles)
        {
            EntityConnection connection = _dbNavigation.Connection as EntityConnection;
            Boolean IsRolePresent = false;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[api].[CheckRoleForShibbolethUPENNDENTAL]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IsApplicantRoleCheck", IsApplicantRoleCheck);
                cmd.Parameters.AddWithValue("@Roles", Roles);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        IsRolePresent = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsRoleSpecified"]);
                    }
                }
            }
            return IsRolePresent;
        }

        Boolean ISecurityRepository.GetMenuItemsForRedirection(string UserID, Int32 BlockID, Int32 BusinessChannelTypeID)
        {
            EntityConnection connection = _dbNavigation.Connection as EntityConnection;
            Boolean IsReactAppUrl = false;


            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter(_PARAM_USERID, UserID),
                             new SqlParameter(_PARAM_BLOCKID, BlockID),
                             new SqlParameter(_PARAM_UI_BUSINESSCHANNEL_TYPEID, BusinessChannelTypeID)
            };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, _USP_GETMENUITEMS, sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            IsReactAppUrl = dr["IsReactAppUrl"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsReactAppUrl"]);
                            if (IsReactAppUrl == true)
                                return IsReactAppUrl;
                        }
                    }
                }
                base.CloseSQLDataReaderConnection(con);
            }
            return IsReactAppUrl;
        }

        Boolean ISecurityRepository.IsSystemEntityUserPermissionExists(Int32 organizationUserID, Int32 entityId, Int32? dpmId)
        {
            if (dpmId.HasValue)
            {
                return _dbNavigation.SystemEntityUserPermissions.Where(cond => cond.SEUP_OrganisationUserId == organizationUserID && cond.SystemEntityPermission.SEP_EntityId == entityId && !cond.SEUP_IsDeleted && cond.SEUP_DPMId == dpmId.Value).Any();
            }
            return false;
        }

        #region UAT-4592
        /// <summary>
        /// Get the disclaimer document on the basis of systemDocumentId
        /// </summary>
        /// <param name="systemDocumentId"></param>
        /// <returns></returns>
        SystemDocument ISecurityRepository.GetSystemDocumentByID(Int32 systemDocumentID)
        {
            SystemDocument systemDoc = _dbNavigation.SystemDocuments.Where(cond => cond.SystemDocumentID == systemDocumentID && !cond.IsDeleted).FirstOrDefault();
            if (systemDoc.IsNotNull())
                return systemDoc;
            return new SystemDocument();
        }
        #endregion

        Boolean ISecurityRepository.CheckRoleForShibbolethBSU(Boolean IsApplicantRoleCheck, String Roles)
        {
            EntityConnection connection = _dbNavigation.Connection as EntityConnection;
            Boolean IsRolePresent = false;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[api].[CheckRoleForShibbolethBSU]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IsApplicantRoleCheck", IsApplicantRoleCheck);
                cmd.Parameters.AddWithValue("@Roles", Roles);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        IsRolePresent = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsRoleSpecified"]);
                    }
                }
            }
            return IsRolePresent;
        }

        String ISecurityRepository.CheckRoleForBSU(String Email, String FirstName, String LastName)
        {
            EntityConnection connection = _dbNavigation.Connection as EntityConnection;
            String roleAssign = String.Empty;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[api].[CheckRoleForBSU]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ShibbolethEmail", Email);
                cmd.Parameters.AddWithValue("@ShibbolethFirstName", FirstName);
                cmd.Parameters.AddWithValue("@ShibbolethLastName", LastName);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        roleAssign = Convert.ToString(ds.Tables[0].Rows[0]["RoleAssign"]);
                    }
                }
            }
            return roleAssign;
        }
        /// <summary>
        /// Release 181:4998
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="FirstName"></param>
        /// <param name="LastName"></param>
        /// <returns></returns>
        String ISecurityRepository.CheckRoleForROSS(String email, String firstName, String lastName,String userName)
        {
            EntityConnection connection = _dbNavigation.Connection as EntityConnection;
            String roleAssign = String.Empty;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[api].[CheckRoleForROSS]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ShibbolethEmail", email);
                cmd.Parameters.AddWithValue("@ShibbolethFirstName", firstName);
                cmd.Parameters.AddWithValue("@ShibbolethLastName", lastName);
                cmd.Parameters.AddWithValue("@ShibbolethUserName", userName);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        roleAssign = Convert.ToString(ds.Tables[0].Rows[0]["RoleAssign"]);
                    }
                }
            }
            return roleAssign;
        }

        Int32 ISecurityRepository.GetFeeItemID()
        {
            var ShiffingCode = ServiceItemFeeType.SHIPPING_FEE.GetStringValue();
            Int32 ServiceFeeTypeID = _dbNavigation.lkpServiceItemFeeTypes.FirstOrDefault(x => x.SIFT_Code == ShiffingCode && !x.SIFT_IsDeleted).SIFT_ID;
            return _dbNavigation.PackageServiceItemFees.FirstOrDefault(cond => cond.PSIF_ServiceItemFeeType == ServiceFeeTypeID && !cond.PSIF_IsDeleted).PSIF_ID;
        }

        Int32 ISecurityRepository.GetAdditionalServiceFeeItemID()
        {
            var AdditionalServiceCode = ServiceItemFeeType.ADDITIONAL_SERVICE_FEE.GetStringValue();
            Int32 ServiceFeeTypeID = _dbNavigation.lkpServiceItemFeeTypes.FirstOrDefault(x => x.SIFT_Code == AdditionalServiceCode && !x.SIFT_IsDeleted).SIFT_ID;
            int pSIF_ID = 0;
            var packageServiceItemFee  =  _dbNavigation.PackageServiceItemFees.FirstOrDefault(cond => cond.PSIF_ServiceItemFeeType == ServiceFeeTypeID && !cond.PSIF_IsDeleted);
            if (packageServiceItemFee != null)
            {
                pSIF_ID = packageServiceItemFee.PSIF_ID;
            }
            return pSIF_ID;
        }

        bool ISecurityRepository.InsertReconciliationProductivityData(List<RecounciliationProductivityData> lstProductivitydata,int tenantID,int UserId, DateTime startDT)
        {
            FlatReconciliationProductivityData recounciliationProductivityData;
            if (lstProductivitydata.Count > 0)
            {
                foreach (var item in lstProductivitydata)
                {
                    recounciliationProductivityData = new FlatReconciliationProductivityData();
                    recounciliationProductivityData.FRPD_TenantID = tenantID;
                    recounciliationProductivityData.FRPD_FirstName = item.FirstName;
                    recounciliationProductivityData.FRPD_LastName = item.LastName;
                    recounciliationProductivityData.FRPD_OrganizationUserID = item.OrganizationUserID;
                    recounciliationProductivityData.FRPD_TotalReviewedCount = item.TotalCount;
                    recounciliationProductivityData.FRPD_ApprovedCount = item.ApprovedCount;
                    recounciliationProductivityData.FRPD_RejectedCount = item.RejectedCount;
                    recounciliationProductivityData.FRPD_ProductivityDate = startDT.AddDays(1);
                    recounciliationProductivityData.FRPD_CreatedOn = DateTime.Now;
                    recounciliationProductivityData.FRPD_CreatedBy = UserId;

                    _dbNavigation.FlatReconciliationProductivityDatas.AddObject(recounciliationProductivityData);
                }
                _dbNavigation.SaveChanges();
            }
            DateTime jobLastExecutedDT = startDT.Date < DateTime.Now.Date ? startDT.AddDays(1) : startDT.Date == DateTime.Now.Date ? DateTime.Now : DateTime.Now;
            ReconciliationProductivityJobHistory reconciliationProductivityJobHistory 
                = _dbNavigation.ReconciliationProductivityJobHistories.Where(x => x.RPJH_TenantID == tenantID).FirstOrDefault();
            if(reconciliationProductivityJobHistory!=null)
            {
                //reconciliationProductivityJobHistory.RPJH_TenantID = tenantID;
                reconciliationProductivityJobHistory.RPJH_JobLastExecutedFor = jobLastExecutedDT;
                //reconciliationProductivityJobHistory.RPJH_IsDeleted = false;
                reconciliationProductivityJobHistory.RPJH_CreatedBy = DateTime.Now;
                reconciliationProductivityJobHistory.RPJH_CreatedOn = DateTime.Now;
                //reconciliationProductivityJobHistory.
               
            }
            else
            {
                ReconciliationProductivityJobHistory ReconProdHistory = new ReconciliationProductivityJobHistory();
                ReconProdHistory.RPJH_TenantID = tenantID;
                ReconProdHistory.RPJH_JobLastExecutedFor = jobLastExecutedDT;
                ReconProdHistory.RPJH_IsDeleted = false;
                ReconProdHistory.RPJH_CreatedBy = DateTime.Now;
                ReconProdHistory.RPJH_CreatedOn = DateTime.Now;
                _dbNavigation.ReconciliationProductivityJobHistories.AddObject(ReconProdHistory);
            }

            _dbNavigation.SaveChanges();
            return true;
        }

        DateTime? ISecurityRepository.GetReconciliationJobHistoryDate(int tenantID)
        {

            ReconciliationProductivityJobHistory objReconProdJobHistory = _dbNavigation.ReconciliationProductivityJobHistories.Where(x => x.RPJH_TenantID == tenantID).FirstOrDefault();
            if (objReconProdJobHistory != null)
                return objReconProdJobHistory.RPJH_JobLastExecutedFor;
            else
                return null;
        }


        bool ISecurityRepository.InsertUpdateReconciliationProductivityData(RecounciliationProductivityData objProductivitydataint)
        {


            EntityConnection connection = _dbNavigation.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[dbo].[usp_InsertUpdateReconciliationProductivityData]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TenantID", objProductivitydataint.TenantID);
                cmd.Parameters.AddWithValue("@OrganizationUserID", objProductivitydataint.OrganizationUserID);
                cmd.Parameters.AddWithValue("@ApprovedCount", objProductivitydataint.ApprovedCount);
                cmd.Parameters.AddWithValue("@RejectedCount", objProductivitydataint.RejectedCount);
                cmd.Parameters.AddWithValue("@TotalReviewedCount", objProductivitydataint.TotalCount);
                cmd.Parameters.AddWithValue("@ProductivityDate", objProductivitydataint.ProductivityDate);
                cmd.Parameters.AddWithValue("@CreatedBy", objProductivitydataint.CreatedBy);

                con.Open();
                int i=cmd.ExecuteNonQuery();
                cmd.Dispose();
                con.Close();
                if(i==0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }
}