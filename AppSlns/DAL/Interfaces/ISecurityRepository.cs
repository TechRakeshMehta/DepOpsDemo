#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ISecurityRepository.cs
// Purpose:
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;

#endregion

#region Application Specific

using Entity;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.UI.Contract.SysXSecurityModel;
using System.Data;
using INTSOF.Utils;
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
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation; //UAT-4013
using INTSOF.UI.Contract.AdminEntryPortal;
using INTSOF.UI.Contract.RecounciliationQueue;
#endregion

#endregion

namespace DAL.Interfaces
{
    /// <summary>
    /// This interface has the method declaration for the security repository.
    /// </summary>

    public interface ISecurityRepository
    {
        #region Manage Aspnet Users

        /// <summary>
        /// Gets the aspnet users.
        /// </summary>
        /// <returns>
        /// The aspnet users.
        /// </returns>
        IQueryable<aspnet_Users> GetAspnetUsers();

        /// <summary>
        /// Gets the aspnet user.
        /// </summary>
        /// <param name="userId"> The user id. </param>
        /// <returns>
        /// The aspnet user.
        /// </returns>
        aspnet_Users GetAspnetUser(String userId);

        #endregion

        #region Manage Dashboard Personalization
        aspnet_PersonalizationPerUser GetPersonalizedPreference(Guid userID, short businessChannelTypeId);
        aspnet_PersonalizationAllUsers GetGroupPreference(Int32 dashBoardUser, Int32 tenantID);
        void SavePersonalizedPreference(Guid userid, Dictionary<string, string> dashboard, short businessChannelTypeId);

        /// <summary>
        /// Clears up the widgets states for the logged in user
        /// </summary>
        /// <param name="userID"></param>
        void ClearDashboardStates(Guid userid, short businessChannelTypeId);

        Int32 GetTenantIdFromOrganizationId(Int32 organisationId);

        void SaveGroupPreference(aspnet_Paths path, aspnet_PersonalizationAllUsers groupPreference);
        #endregion
        #region Manage Line of Businesses

        /// <summary>
        /// Line of business code exists or not
        /// </summary>
        /// <param name="newCode">New LOB Code</param>
        /// <param name="oldCode">Existing LOB Code</param>
        /// <returns>True, if exists else false</returns>
        Boolean IsLOBCodeExist(String newCode, String oldCode);

        /// <summary>
        /// Gets the features for line of business and allow delegation.
        /// </summary>
        /// <param name="blockId"> The block id. </param>
        /// <returns>
        /// The features for line of business and allow delegation.
        /// </returns>
        IQueryable<SysXBlocksFeature> GetFeaturesForLineOfBusinessAndAllowDelegation(Int32 blockId);

        /// <summary>
        /// Gets the line of business.
        /// </summary>
        /// <param name="blockId"> The block id. </param>
        /// <returns>
        /// The line of business.
        /// </returns>
        lkpSysXBlock GetLineOfBusiness(Int32 blockId);

        /// <summary>
        /// Gets the line of businesses.
        /// </summary>
        /// <returns>
        /// The line of businesses.
        /// </returns>
        IQueryable<lkpSysXBlock> GetLineOfBusinesses();

        /// <summary>
        /// Gets the user line of businesses ids.
        /// </summary>
        /// <param name="userId"> The user id. </param>
        /// <returns>
        /// The user line of businesses identifiers.
        /// </returns>
        IQueryable<Int32> GetUserLineOfBusinessesIds(String userId);

        /// <summary>
        /// Gets the user line of businesses by role id.
        /// </summary>
        /// <param name="roleId">The role id.</param>
        /// <returns></returns>
        IQueryable<lkpSysXBlock> GetUserLineOfBusinessesByRoleId(String roleId);

        /// <summary>
        /// Gets a line of businesses by user.
        /// </summary>
        /// <param name="userId"> The user id. </param>
        /// <returns>
        /// The line of businesses by user.
        /// </returns>
        IQueryable<vw_UserAssignedBlocks> GetLineOfBusinessesByUser(String userId);

        /// <summary>
        /// Determines whether [is line of business exists] [the specified entered line of business name].
        /// </summary>
        /// <param name="enteredLineOfBusinessName">Name of the entered line of business.</param>
        /// <param name="existingLineOfBusinessName">Name of the existing line of business.</param>
        /// <returns><c>true</c> if [is line of business exists] [the specified entered line of business name]; otherwise, <c>false</c>.</returns>
        Boolean IsLineOfBusinessExists(String enteredLineOfBusinessName, String existingLineOfBusinessName = null);

        /// <summary>
        /// Performs a delete operation for line of business.
        /// </summary>
        /// <param name="sysXBlock">The line of business.</param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        Boolean DeleteLineOfBusiness(lkpSysXBlock sysXBlock);

        #endregion

        #region Manage mapping of Line of Business and features

        /// <summary>
        /// Lines the of business feature mapping.
        /// </summary>
        /// <param name="blockFeature">The block feature.</param>
        /// <param name="blockFeatureIdsToBeAdded">The block feature Ids to be add.</param>
        /// <param name="blockFeatureIdsToBeRemoved">The block feature Ids to be remove.</param>
        /// <param name="createdById">currently logged in user's Id.</param>
        /// <returns></returns>
        Boolean LineOfBusinessFeatureMapping(lkpSysXBlock blockFeature, List<Int32> blockFeatureIdsToBeAdded, List<Int32> blockFeatureIdsToBeRemoved, Int32 createdById);

        /// <summary>
        /// Gets the features for line of business.
        /// </summary>
        /// <param name="blockId"> The block id. </param>
        /// <returns>
        /// The features for line of business.
        /// </returns>        
        IQueryable<SysXBlocksFeature> GetFeaturesForLineOfBusiness(Int32 blockId);

        /// <summary>
        /// Query if 'sysXBlockFeatureId' is system x coordinate line of business exist in product
        /// feature.
        /// </summary>
        /// <param name="sysXBlockFeatureId"> The sys X block feature id. </param>
        /// <returns>
        /// <c>true</c> if [is sys X block exist in product feature] [the specified sys X block feature
        /// id]; otherwise, <c>false</c>.
        /// </returns>        
        Boolean IsSysXLineOfBusinessExistInProductFeature(Int32 sysXBlockFeatureId);

        #endregion

        #region Manage Organizations

        /// <summary>
        /// Gets the organizations by tenant identifier.
        /// </summary>
        /// <param name="tenantId">The value for tenant's id.</param>
        /// <returns>
        /// The organizations by tenant identifier.
        /// </returns>
        IQueryable<Organization> GetOrganizationsByTenantId(Int32 tenantId);

        /// <summary>
        /// Enumerates get organizations by current user identifier in this collection.
        /// </summary>
        /// <param name="isAdmin"> if set to <c>true</c> [is admin]. </param>
        /// <param name="productId"> The product id. </param>
        /// <param name="currentUserId"> The current user id. </param>
        /// <returns>
        /// An enumerator that allows foreach to be used to process get organizations by current user
        /// identifier in this collection.
        /// </returns>
        IQueryable<Organization> GetOrganizationsByCurrentUserId(Boolean isAdmin, Int32 productId, Int32 currentUserId);

        /// <summary>
        /// Gets an organization for tenant.
        /// </summary>
        /// <param name="tenantId"> The tenant id. </param>
        /// <returns>
        /// The organization for tenant.
        /// </returns>        
        Organization GetOrganizationForTenant(Int32 tenantId);

        /// <summary>
        /// Retrieves all users based on product id.
        /// </summary>
        /// <param name="assignToProductId">The product id.</param>
        /// <returns>
        /// The users for product.
        /// </returns>
        List<OrganizationUser> GetUsersByProductId(Int32 assignToProductId);

        Int32 GetOrganizationIdByProductId(Int32 assignToProductId);

        /// <summary>
        /// Gets an organization.
        /// </summary>
        /// <param name="organizationId"> The organization id. </param>
        /// <returns>
        /// The organization.
        /// </returns>        
        Organization GetOrganization(Int32 organizationId);

        IQueryable<Organization> GetDepartments(Int32 organizationId);

        //List<OrganizationUserProgram> GetProgramsFromOrgUserProgrm(Int32 organizationId);

        // List<DeptProgramMapping> GetDeptFromDeptPrgMaping(Int32 organizationId);
        /// <summary>
        /// Determines whether [is organization exists] [the specified entered organization name].
        /// </summary>
        /// <param name="enteredOrganizationName">Name of the entered organization.</param>
        /// <param name="existingOrganizationName">Name of the existing organization.</param>
        /// <returns><c>true</c> if [is organization exists] [the specified entered organization name]; otherwise, <c>false</c>.</returns>
        Boolean IsOrganizationExists(String enteredOrganizationName, String existingOrganizationName = null);

        /// <summary>
        /// Gets all type organization.
        /// </summary>
        /// <param name="organizationId"> The organization id. </param>
        /// <returns>
        /// all type organization.
        /// </returns>        
        Organization GetAllTypeOrganization(Int32 organizationId);

        /// <summary>
        /// Deletes the dependent user for organization described by organizationId.
        /// </summary>
        /// <param name="organizationId"> The organization id. </param>        
        void DeleteDependentUserForOrganization(Int32 organizationId);

        /// <summary>
        /// Gets the organizations.
        /// </summary>
        /// <param name="isAdmin">   if set to <c>true</c> [is admin]. </param>
        /// <param name="productId"> The product id. </param>
        /// <returns>
        /// The organizations.
        /// </returns>        
        IQueryable<Organization> GetOrganizations(Boolean isAdmin, Int32 productId);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IQueryable<Organization> GetOrganizations();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IQueryable<lkpGender> GetGender();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IQueryable<lkpSecurityQuestion> GetSecurityQuestion();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        IQueryable<OrganizationLocation> GetOrganizationLocations(Int32 organizationId);


        /// <summary>
        /// Gets the organizations for product.
        /// </summary>
        /// <param name="isAdmin">   if set to <c>true</c> [is admin]. </param>
        /// <param name="productId"> The product id. </param>
        /// <returns>
        /// The organizations for product.
        /// </returns>        
        IQueryable<Organization> GetOrganizationsForProduct(Boolean isAdmin, Int32 productId);

        /// <summary>
        /// Check organization by current user identifier.
        /// </summary>
        /// <param name="currentUserId">The current user id.</param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        Boolean CheckOrganizationByCurrentUserId(Int32 currentUserId);

        /// <summary>
        /// Check organization by current user identifier.
        /// </summary>
        /// <param name="isAdmin">Checks if the logged in user is admin.</param>
        /// <param name="productId">Logged in user's productId.</param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        Boolean IsDepartmentOrOrganizationExistsForProduct(Boolean isAdmin, Int32? productId);

        /// <summary>
        /// Checks whether the currently selected organization is of the supplier type tenant.
        /// </summary>
        /// <param name="selectedOrganizationId">currently selected organization's id.</param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        Boolean IsOrganizationOfSupplier(Int32 selectedOrganizationId);

        /// <summary>
        /// Query if department is assign to any user.
        /// </summary>
        /// <param name="departmentId"> Id of the current department </param>
        /// <returns>
        /// true if department assign to any user, false if not.
        /// </returns>
        Boolean IsUserExistsForDepartment(Int32 departmentId);

        /// <summary>
        /// Return all user name prefix.
        /// </summary>
        /// <returns></returns>
        List<OrganizationUserNamePrefix> getAllUserNamePrefix();

        /// <summary>
        /// Get all the OrganizationUserNamePrefix.
        /// </summary>     
        /// <param name="organizationId"> The organization id. </param>
        /// <returns>
        /// The OrganizationUserNamePrefix.
        /// </returns>        
        IQueryable<OrganizationUserNamePrefix> GetOrganizationUserNamePrefix(Int32 organizationId);

        #endregion

        #region [ Manage Programs ]

        /// <summary>
        /// Gets the Programs by tenant identifier.
        /// </summary>
        /// <param name="tenantId">The value for tenant's id.</param>
        /// <returns>
        /// The Programs by tenant identifier.
        /// </returns>
        //IQueryable<AdminProgramStudy> GetProgramsByOrganizationId(Int32 OrganizationId);

        /// <summary>
        /// Gets List of programs for the given tenant ID.
        /// </summary>
        /// <param name="clientId">Tenant ID</param>
        /// <returns>List of active programs</returns>
        //List<AdminProgramStudy> GetAllProgramsForTenantID(Int32 clientId);

        /// <summary>
        /// Gets the organization for the given client ID.
        /// </summary>
        /// <param name="clientId">Tenant ID</param>
        /// <returns>Organization Entity Object</returns>
        Entity.Organization GetOrganizationForTenantID(Int32 clientId);

        /// <summary>
        /// Gets the Programs by DepartmentId.
        /// </summary>
        /// The Programs by tenant identifier.
        /// </returns>
        //IQueryable<DeptProgramMapping> GetOrganizationProgramList(Int32 OrganizationId); 
        /// <summary>
        /// Gets the Program by depProgramMappingId.
        /// </summary>
        /// The Program .
        /// </returns>
        //DeptProgramMapping GetOrganizationProgram(Int32 depProgramId);

        /// <summary>
        /// Enumerates get organizations by current user identifier in this collection.
        /// </summary>
        /// <param name="isAdmin"> if set to <c>true</c> [is admin]. </param>
        /// <param name="productId"> The product id. </param>
        /// <param name="currentUserId"> The current user id. </param>
        /// <returns>
        /// An enumerator that allows foreach to be used to process get Programs by current user
        /// identifier in this collection.
        /// </returns>
        //IQueryable<AdminProgramStudy> GetProgramsByCurrentUserId(Boolean isAdmin, Int32 productId, Int32 currentUserId);

        ///// <summary>
        ///// Gets an organization for tenant.
        ///// </summary>
        ///// <param name="tenantId"> The tenant id. </param>
        ///// <returns>
        ///// The organization for tenant.
        ///// </returns>        
        ////AdminProgramStudy GetProgramForOrganization(Int32 tenantId);

        ///// <summary>
        ///// Retrieves all users based on program id.
        ///// </summary>
        ///// <param name="assignToProductId">The program id.</param>
        ///// <returns>
        ///// The users for program.
        ///// </returns>
        //List<OrganizationUser> GetUsersByProgramId(Int32 ProgramId);

        //Int32 GetOrganizationIdByProgramId(Int32 ProgramId);

        /// <summary>
        /// Gets a Program.
        /// </summary>
        /// <param name="organizationId"> Program id. </param>
        /// <returns>
        /// The Program.
        /// </returns>        
        //AdminProgramStudy GetProgram(Int32 ProgramId);

        /// <summary>
        /// Determines whether [is Program exists] [the specified entered Program name].
        /// </summary>
        /// <param name="enteredOrganizationName">Name of the entered Program.</param>
        /// <param name="existingOrganizationName">Name of the existing Program.</param>
        /// <returns><c>true</c> if [is program exists] [the specified entered organization name]; otherwise, <c>false</c>.</returns>
        Boolean IsProgramExists(String enteredProgramName, String existingProgramName = null);

        Boolean UpdateProgramObject();


        #endregion

        #region [ Manage Grades ]

        /// <summary>
        /// Get all GradeLevelGroups
        /// </summary>
        /// <returns>
        /// Returns all the GradeLevelGroups
        /// </returns>
        List<lkpGradeLevelGroup> GetAllGradeLevelGroups();

        /// <summary>
        /// Gets the Grades by tenant identifier.
        /// </summary>
        /// <param name="tenantId">The value for tenant's id.</param>
        /// <returns>
        /// The Grades by tenant identifier.
        /// </returns>
        IQueryable<lkpGradeLevel> GetGradesByOrganizationId(Int32 OrganizationId);

        /// <summary>
        /// Enumerates get Grades by current user identifier in this collection.
        /// </summary>
        /// <param name="isAdmin"> if set to <c>true</c> [is admin]. </param>
        /// <param name="productId"> The product id. </param>
        /// <param name="currentUserId"> The current user id. </param>
        /// <returns>
        /// An enumerator that allows foreach to be used to process get Programs by current user
        /// identifier in this collection.
        /// </returns>

        IQueryable<lkpGradeLevel> GetAllGrades();

        IQueryable<lkpGradeLevel> GetGradesByCurrentUserId(Boolean isAdmin, Int32 productId, Int32 currentUserId);


        /// <summary>
        /// Gets a Grade.
        /// </summary>
        /// <param name="organizationId"> Grade id. </param>
        /// <returns>
        /// The Grade.
        /// </returns>        
        lkpGradeLevel GetGrade(Int32 GradeId);

        /// <summary>
        /// Determines whether [is Program exists] [the specified entered Program name].
        /// </summary>
        /// <param name="enteredOrganizationName">Name of the entered Program.</param>
        /// <param name="existingOrganizationName">Name of the existing Program.</param>
        /// <returns><c>true</c> if [is program exists] [the specified entered organization name]; otherwise, <c>false</c>.</returns>
        Boolean IsGradeExists(String enteredGradeName, Int32 organizationId, String existingGradeName = null);

        IQueryable<lkpGradeLevel> GetGradeListByOrganizationId(Int32 organizationId);
        #endregion

        #region Manage Organization Users

        /// <summary>
        /// Gets an organization user information by user identifier.
        /// </summary>
        /// <param name="userId">The Current user Id.</param>
        /// <returns>
        /// The organization user information by user identifier.
        /// </returns>
        IQueryable<OrganizationUser> GetOrganizationUserInfoByUserId(String userId);

        /// <summary>
        /// Gets an organization users details.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <returns>
        /// The organization users details.
        /// </returns>
        OrganizationUser GetOrganizationUsersDetails(String userName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ssn"></param>
        /// <returns></returns>
        OrganizationUser GetOrganizationUsersBySSN(String ssn);

        /// <summary>
        /// Gets an organization users by email. 
        /// </summary>
        /// <param name="email"> The email address/</param>
        /// <returns> The organization users by email. </returns>
        IQueryable<OrganizationUser> GetOrganizationUsersByEmail(String email);

        /// <summary>
        /// Gets the organization user.
        /// </summary>
        /// <param name="organizationUserId">The organization user id.</param>
        /// <returns></returns>
        OrganizationUser GetOrganizationUser(Int32 organizationUserId);

        /// <summary>
        /// Updates the status of whether the client admin can receive Internal message from Applicants or Not
        /// </summary>
        /// <param name="organizationUserId"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        Boolean UpdateInternalMsgNotificationSettings(Int32 organizationUserId, Int32 currentUserId);

        List<OrganizationUser> GetOrganizationUserListForUserId(String userID);

        List<OrganizationUser> GetOrganizationUserList(List<Int32> lstOrganizationUserIds);

        /// <summary>
        /// Gets the list of residential histories for the given organisation user ID.
        /// </summary>
        /// <param name="organizationUserId">Organisation User ID</param>
        /// <returns>List of residential histories</returns>
        List<ResidentialHistory> GetUserResidentialHistories(Int32 organizationUserId);
        List<ResidentialHistoryProfile> GetUserResidentialHistoryProfiles(Int32 organizationUserProfileId);
        OrganizationUser GetOrganizationUserDetail(Int32 organizationUserId);

        /// <summary>
        /// Gets the organization user.
        /// </summary>
        /// <param name="organizationUserId">The organization user id.</param>
        /// <returns></returns>
        OrganizationUser GetOrganizationUserByVerificationCode(String userVerificationCode);

        /// <summary>
        /// Gets the mapped organization users.
        /// </summary>
        /// <param name="isAdmin">      if set to <c>true</c> [is admin].</param>
        /// <param name="currentUserId">The current user id.</param>
        /// <returns>
        /// An enumerator that allows foreach to be used to process get mapped organization users in this
        /// collection.
        /// </returns>        
        List<OrganizationUser> GetMappedOrganizationUsers(Boolean isAdmin, Int32 currentUserId);

        /// <summary>
        /// Adds the organization user in transaction.
        /// </summary>
        /// <param name="user">            The user.</param>
        /// <param name="userOrganization">The user organization.</param>
        /// <returns>
        /// .
        /// </returns>        
        OrganizationUser AddOrganizationUserInTransaction(aspnet_Users user, OrganizationUser userOrganization);

        /// <summary>
        /// ZFunction to rename the profile pic Name
        /// </summary>
        /// <param name="organizationUserId"></param>
        /// <param name="newFileName"></param>
        /// <returns></returns>
        Boolean UpdateOrganizationUser(Int32 organizationUserId, String newFileName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationUser"></param>
        /// <returns></returns>
        List<OrganizationUser> SyncUsersProfilePictureInAllTenant(OrganizationUser organizationUser);
        OrganizationUserProfile AddOrganizationUserProfile(OrganizationUserProfile orgUserProfile);
        #endregion

        #region Manage Permission Types

        /// <summary>
        /// Gets the permission types.
        /// </summary>
        /// <returns>
        /// The permission types.
        /// </returns>        
        IQueryable<PermissionType> GetPermissionTypes();

        /// <summary>
        /// Gets the type of the permission.
        /// </summary>
        /// <param name="permissionTypeId">The permission type id.</param>
        /// <returns>
        /// The permission type.
        /// </returns>        
        PermissionType GetPermissionType(Int32 permissionTypeId);

        /// <summary>
        /// Gets the permissions.
        /// </summary>
        /// <returns>
        /// The permissions.
        /// </returns>        
        IQueryable<Permission> GetPermissions();

        /// <summary>
        /// Gets the permission.
        /// </summary>
        /// <param name="permissionId">The permission id.</param>
        /// <returns>
        /// The permission.
        /// </returns>        
        Permission GetPermission(Int32 permissionId);

        /// <summary>
        /// Gets my permission.
        /// </summary>
        /// <param name="userId">   The user id.</param>
        /// <param name="featureId">The feature id.</param>
        /// <param name="blockId">  The block id.</param>
        /// <returns>
        /// The permission.
        /// </returns>        
        Permission GetPermission(String userId, List<Int32> lstFeatureID, Int32 blockId);

        /// <summary>
        /// Query if 'enteredLineOfBusinessName' is permission exists.
        /// </summary>
        /// <param name="enteredPermissionName"> Name of the entered line of business.</param>
        /// <param name="existingPermissionName">Name of the existing line of business.</param>
        /// <returns>
        /// true if permission exists, false if not.
        /// </returns>
        Boolean IsPermissionExists(String enteredPermissionName, String existingPermissionName = null);

        /// <summary>
        /// Query if 'enteredPermissionTypeName' is permission type exists.
        /// </summary>
        /// <param name="enteredPermissionTypeName"> Name of the entered permission type.</param>
        /// <param name="existingPermissionTypeName">(optional) name of the existing permission type.</param>
        /// <returns>
        /// true if permission type exists, false if not.
        /// </returns>
        Boolean IsPermissionTypeExists(String enteredPermissionTypeName, String existingPermissionTypeName = null);

        /// <summary>
        /// Query if 'permission' is assign to any feature.
        /// </summary>
        /// <param name="permissionId">Identifier for the permission.</param>
        /// <returns>
        /// true if permission assign to any feature, false if not.
        /// </returns>
        Boolean IsPermissionAssignToAnyFeature(Int32 permissionId);

        /// <summary>
        /// Query if 'permission type' is assign to any permission.
        /// </summary>
        /// <param name="permissionTypeId"> Id of the current permission type</param>
        /// <returns>
        /// true if permission type assign to any permission, false if not.
        /// </returns>
        Boolean IsPermissionTypeAssignToAnyPermission(Int32 permissionTypeId);

        #endregion

        #region Manage Policies

        /// <summary>
        /// Enumerates set new policy set user control for save in this collection.
        /// </summary>
        /// <param name="policySetUserControlList">The policy set user control list.</param>
        /// <returns>
        /// An enumerator that allows foreach to be used to process set new policy set user control for
        /// save in this collection.
        /// </returns>
        IEnumerable<PolicySetUserControl> SetNewPolicySetUserControlForSave(IEnumerable<PolicySetUserControl> policySetUserControlList);

        /// <summary>
        /// Gets all control policies.
        /// </summary>
        /// <param name="policies">     The policies.</param>
        /// <param name="sysXAdminList">The sys X admin list.</param>
        /// <param name="ucName">       Name of the user control.</param>
        /// <param name="currentRoleId">The current role id.</param>
        /// <param name="currentUserId">The current user id.</param>
        void GetAllControlPolicies(List<Policy> policies, List<Int32> sysXAdminList, String ucName, String currentRoleId, Int32 currentUserId);

        /// <summary>
        /// Gets the control selected values.
        /// </summary>
        /// <param name="roleId">               The role id.</param>
        /// <param name="registerUserControlId">The register user control id.</param>
        /// <returns>
        /// The control selected values.
        /// </returns>        
        PolicySetUserControl GetControlSelectedValues(String roleId, Int32 registerUserControlId);

        /// <summary>
        /// Gets the registered control list.
        /// </summary>
        /// <returns>
        /// The registered control list.
        /// </returns>        
        IQueryable<PolicyControl> GetRegisteredControlList();

        /// <summary>
        /// Saves the policies.
        /// </summary>
        /// <param name="policySet">The policy set.</param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>        
        Boolean SavePolicies(PolicySet policySet);

        /// <summary>
        /// Saves the policies.
        /// </summary>
        /// <param name="policySetUserControlList">The policy set user control list.</param>
        /// <param name="currentRoleId">           The current role id.</param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>        
        Boolean SavePolicies(EntityCollection<PolicySetUserControl> policySetUserControlList, String currentRoleId);

        /// <summary>
        /// Gets all policies.
        /// </summary>
        /// <param name="ucName">       Name of the user control.</param>
        /// <param name="roleList">     The role list.</param>
        /// <param name="sysXAdminList">The sys X admin list.</param>
        /// <param name="currentUserId">The current user id.</param>
        /// <returns>
        /// all policies.
        /// </returns>        
        PolicySetUserControl GetAllPolicies(String ucName, List<aspnet_Roles> roleList, List<Int32> sysXAdminList, Int32 currentUserId);

        /// <summary>
        /// Gets the policy register controls.
        /// </summary>
        /// <returns>
        /// The policy register controls.
        /// </returns>        
        IQueryable<PolicyRegisterUserControl> GetPolicyRegisterControls();

        /// <summary>
        /// Gets the policy register controls.
        /// </summary>
        /// <param name="currentRoleId">The role Id.</param>
        /// <returns>
        /// The policy register controls.
        /// </returns>        
        IQueryable<PolicyRegisterUserControl> GetPolicyRegisterControls(String currentRoleId);

        /// <summary>
        /// Gets the policy set by role id.
        /// </summary>
        /// <param name="roleId">The role id.</param>
        /// <returns>
        /// The policy set by role identifier.
        /// </returns>        
        PolicySet GetPolicySetByRoleId(String roleId);

        /// <summary>
        /// Gets all policies sub parent.
        /// </summary>
        /// <param name="ucName">       Name of the uc.</param>
        /// <param name="roleId">       The role id.</param>
        /// <param name="sysXAdminList">The sys X admin list.</param>
        /// <param name="currentUserId">The current user id.</param>
        /// <returns>
        /// all policies sub parent.
        /// </returns>        
        PolicySetUserControl GetAllPoliciesSubParent(String ucName, String roleId, List<Int32> sysXAdminList, Int32 currentUserId);

        /// <summary>
        /// Checks the feature assign to role.
        /// </summary>
        /// <param name="featureId">The feature id.</param>
        /// <param name="productId">The product id.</param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>        
        Boolean CheckFeatureAssignToRole(Int32 featureId, Int32 productId);

        #endregion

        #region Manage Policy Register Control

        /// <summary>
        /// Gets the policy register control.
        /// </summary>
        /// <param name="policyRegisterControlId">The policy register control id.</param>
        /// <returns>
        /// The policy register control.
        /// </returns>        
        PolicyRegisterUserControl GetPolicyRegisterControl(Int32 policyRegisterControlId);

        /// <summary>
        /// Selects the child policy controls.
        /// </summary>
        /// <param name="registerUserControlId">The register user control id.</param>
        /// <returns>
        /// .
        /// </returns>        
        Int32 SelectChildPolicyControls(long registerUserControlId);

        /// <summary>
        /// Query if 'enteredControlName' is exists.
        /// </summary>
        /// <param name="enteredControlName"> Name of the entered Control.</param>
        /// <param name="existingControlName">Name of the existing Control.</param>
        /// <returns>
        /// true if Policy Register Control exists, false if not.
        /// </returns>
        Boolean IsControlNameExists(String enteredControlName, String existingControlName = null);

        #endregion

        #region Manage Products

        /// <summary>
        /// Enumerates get role details by product identifier in this collection.
        /// </summary>
        /// <param name="productId">The Product Id.</param>
        /// <returns>
        /// An enumerator that allows foreach to be used to process get role details by product
        /// identifier in this collection.
        /// </returns>
        IQueryable<RoleDetail> GetRoleDetailsByProductId(Int32 productId);

        /// <summary>
        /// Gets the products.
        /// </summary>
        /// <returns>
        /// The products.
        /// </returns>        
        IQueryable<TenantProduct> GetProducts();

        /// <summary>
        /// Determines whether [is product exists] [the specified entered tenant name].
        /// </summary>
        /// <param name="enteredTenantName"> Name of the entered tenant.</param>
        /// <param name="existingTenantName">Name of the existing tenant.</param>
        /// <returns>
        /// <c>true</c> if [is product exists] [the specified entered tenant name]; otherwise,
        /// <c>false</c>.
        /// </returns>        
        Boolean IsProductExists(String enteredTenantName, String existingTenantName = null);

        /// <summary>
        /// Gets the product.
        /// </summary>
        /// <param name="productId">The product id.</param>
        /// <returns>
        /// The product.
        /// </returns>        
        TenantProduct GetProduct(Int32 productId);

        /// <summary>
        /// Gets the features for product.
        /// </summary>
        /// <param name="productId">The product id.</param>
        /// <returns>
        /// The features for product.
        /// </returns>        
        IQueryable<TenantProductFeature> GetFeaturesForProduct(Int32 productId);

        #endregion

        #region Manage Product Features

        /// <summary>
        /// Retrieves all feature permissions.
        /// </summary>
        /// <param name="featurePermissionId">.</param>
        /// <returns>
        /// The feature permission.
        /// </returns>
        FeaturePermission GetFeaturePermission(Int32 featurePermissionId);

        /// <summary>
        /// Deletes the product features described by productId.
        /// </summary>
        /// <param name="productId">The product id.</param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        Boolean DeleteProductFeatures(Int32 productId);

        /// <summary> Gets the product features. </summary>
        /// <returns> The product features. </returns>
        /// <remarks> . </remarks>
        IQueryable<ProductFeature> GetProductFeatures();

        /// <summary>
        /// Gets the product feature.
        /// </summary>
        /// <param name="productFeatureId">The product feature id.</param>
        /// <returns>
        /// The product feature.
        /// </returns>        
        ProductFeature GetProductFeature(Int32 productFeatureId);

        /// <summary>
        /// Query if 'productFeatureId' is feature associated with line of business.
        /// </summary>
        /// <param name="productFeatureId"> The product feature id. </param>
        /// <returns>
        /// true if feature associated with line of business, false if not.
        /// </returns>
        Boolean IsFeatureAssociatedWithLineOfBusiness(Int32 productFeatureId);

        /// <summary>
        /// Determines whether [is line of business associated with product] [the specified sys X block
        /// id].
        /// </summary>
        /// <param name="sysXBlockId">The sys X block id.</param>
        /// <returns>
        /// <c>true</c> if [is line of business associated with product] [the specified sys X block id];
        /// otherwise, <c>false</c>.
        /// </returns>        
        Boolean IsLineOfBusinessAssociatedWithProduct(Int32 sysXBlockId);

        /// <summary>
        /// Query if 'organizationUserId' is user tied to several other users.
        /// </summary>
        /// <param name="organizationUserId">The organization user id.</param>
        /// <returns>
        /// true if user tied to several other users, false if not.
        /// </returns>
        Boolean IsUserTiedToSeveralOtherUsers(Int32 organizationUserId);

        /// <summary>
        /// Deletes the product features cascade.
        /// </summary>
        /// <param name="productFeature">The product feature.</param>
        /// <param name="modifiedById">The value for modifiedById</param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>        
        Boolean DeleteProductFeaturesCascade(ProductFeature productFeature, Int32 modifiedById);

        #endregion

        #region Manage mapping of product and features

        /// <summary>
        /// This method returns all the odd entries from RolePermissionProductFeature table , which is no more associated with the product's feature.
        /// </summary>
        /// <param name="featureList">List of all the new relations made through Map Product Feature page.</param>
        /// <returns></returns>
        List<RolePermissionProductFeature> GetFeatureWithPermissionUsedByRole(IEnumerable<BlockFeaturePermissionMapper> featureList);

        /// <summary>
        /// Products the feature mapping.
        /// </summary>
        /// <param name="product">            The product.</param>
        /// <param name="featureList">        The feature list.</param>
        /// <param name="updatedSysXBlockIds">The updated sys X block Ids.</param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>        
        Boolean ProductFeatureMapping(TenantProduct product, List<BlockFeaturePermissionMapper> featureList, List<Int32> updatedSysXBlockIds, Int32 currentLoggedInUserID);

        /// <summary>
        /// Gets the tenant product features for line of business.
        /// </summary>
        /// <param name="tenantProductId">The tenant product id.</param>
        /// <param name="sysXBlockId">    The sys X block id.</param>
        /// <returns>
        /// The tenant product features for line of business.
        /// </returns>        
        IQueryable<TenantProductFeature> GetTenantProductFeaturesForLineOfBusiness(Int32 tenantProductId, Int32 sysXBlockId);

        #endregion

        #region Manage Role Details

        /// <summary>
        /// Gets the role detail by id.
        /// </summary>
        /// <param name="roleDetailId">The role detail id.</param>
        /// <returns>
        /// The role detail by identifier.
        /// </returns>        
        RoleDetail GetRoleDetailById(String roleDetailId);

        /// <summary>
        /// Gets the role detail.
        /// </summary>
        /// <returns>
        /// The role detail.
        /// </returns>        
        IQueryable<RoleDetail> GetRoleDetail();

        /// <summary>
        /// Adds the role detail.
        /// </summary>
        /// <param name="roleDetail">The role detail.</param>       
        RoleDetail AddRoleDetail(RoleDetail roleDetail);

        /// <summary>
        /// Updates the role.
        /// </summary>
        /// <param name="roleDetail">The role detail.</param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>        
        Boolean UpdateRole(RoleDetail roleDetail);

        /// <summary>
        /// Gets the role detail.
        /// </summary>
        /// <param name="isAdmin">      if set to <c>true</c> [is admin].</param>
        /// <param name="currentUserId">The current user id.</param>
        /// <returns>
        /// The role detail.
        /// </returns>        
        IQueryable<RoleDetail> GetRoleDetail(Boolean isAdmin, Int32 currentUserId);

        /// <summary>
        /// Deletes the role detail.
        /// </summary>
        /// <param name="roleDetail">The role detail.</param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>        
        Boolean DeleteRoleDetail(RoleDetail roleDetail);

        /// <summary>
        /// Determines whether [is role exists] [the specified entered role name].
        /// </summary>
        /// <param name="productId">The value for product id.</param>
        /// <param name="enteredRoleName"> Name of the entered role.</param>
        /// <param name="existingRoleName">Name of the existing role.</param>
        /// <returns>
        /// <c>true</c> if [is role exists] [the specified entered role name]; otherwise, <c>false</c>.
        /// </returns>        
        Boolean IsRoleExists(Int32 productId, String enteredRoleName, String existingRoleName = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        List<String> getDefaultRoleDetailIds(List<Int32> productId);
        #endregion

        #region Manage Role Permission ProductFeature

        IQueryable<RolePermissionProductFeature> GetProductFeatureRoles(Int32 productFeatureId);

        /// <summary>
        /// Gets the feature for role.
        /// </summary>
        /// <param name="roleDetailId">The role detail id.</param>
        /// <param name="productId">   The product id.</param>
        /// <returns>
        /// The feature for role.
        /// </returns>        
        IQueryable<RolePermissionProductFeature> GetFeatureForRole(String roleDetailId, Int32 productId);

        /// <summary>
        /// Roles the feature mapping.
        /// </summary>
        /// <param name="roleId">               The role id.</param>
        /// <param name="productId">            The product id.</param>
        /// <param name="featurePermissionList">The feature permission list.</param>
        /// <param name="updatedSysXBlockIds">  The updated sysX block Ids.</param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>        
        Boolean RoleFeatureMapping(String roleId, Int32 productId, Dictionary<Int32, Int32> featurePermissionList, List<Int32> updatedSysXBlockIds, List<RoleFeatureActionContract> roleFeatureActions);

        /// <summary>
        /// Gets the products for tenant.
        /// </summary>
        /// <param name="tenantId">The tenant id.</param>
        /// <returns>
        /// The products for tenant.
        /// </returns>        
        IQueryable<TenantProduct> GetProductsForTenant(Int32 tenantId);

        /// <summary>
        /// Retrieves all the relations from RolePermissionProductFeature table.
        /// </summary>
        /// <returns></returns>
        IQueryable<RolePermissionProductFeature> GetRolePermissionProductFeatures();

        #endregion

        #region Manage States

        /// <summary>
        /// Gets the states.
        /// </summary>
        /// <returns>
        /// The states.
        /// </returns>        
        IQueryable<State> GetStates();

        #endregion

        #region Manage Cities

        /// <summary>
        /// Gets the cities.
        /// </summary>
        /// <returns>
        /// The cities.
        /// </returns>        
        IQueryable<City> GetCities();

        #endregion

        #region Manage Countries

        /// <summary>
        /// Gets the Countries.
        /// </summary>
        /// <returns>
        /// The Countries.
        /// </returns>        
        IQueryable<Country> GetCountries();

        #endregion

        #region Manage ZipCodes

        /// <summary>
        /// Gets the zip.
        /// </summary>
        /// <param name="zipId">The zip id.</param>
        /// <returns>
        /// The zip.
        /// </returns>        
        ZipCode GetZip(Int32 zipId);

        IQueryable<ZipCode> GetZipcodes();
        #endregion

        #region Manage Counties

        IQueryable<County> GetCounties();

        #endregion

        #region Manage SysX configurations

        /// <summary>
        /// Gets the sysX configuration value.
        /// </summary>
        /// <param name="sysXKey">The sys X key.</param>
        /// <returns>
        /// The system x coordinate configuration value.
        /// </returns>        
        String GetSysXConfigValue(String sysXKey);

        /// <summary>
        /// Gets the sys X configuration.
        /// </summary>
        /// <param name="sysXKey">The sys X key.</param>
        /// <returns>
        /// The system x coordinate configuration.
        /// </returns>        
        SysXConfig GetSysXConfig(String sysXKey);

        /// <summary>
        /// Gets the sys X configurations.
        /// </summary>
        /// <returns>
        /// The system x coordinate configs.
        /// </returns>        
        IQueryable<SysXConfig> GetSysXConfigs();

        /// <summary>
        /// Determines whether [is user exists] [the specified entered user name].
        /// </summary>
        /// <param name="enteredSysXKey"> Name of the entered SysXKey.</param>
        /// <param name="existingSysXKey">Name of the existing SysXKey.</param>
        /// <returns>
        /// <c>true</c> if [is user exists] [the specified entered user name]; otherwise, <c>false</c>.
        /// </returns>  
        Boolean IsSysXKeyExists(String enteredSysXKey, String existingSysXKey);

        #endregion

        #region Manage SysXBlockFeature

        /// <summary>
        /// Check system x coordinate block feature.
        /// </summary>
        /// <param name="blockId">  The  block id.</param>
        /// <param name="featureId">The Feature Id.</param>
        SysXBlocksFeature CheckSysXBlockFeature(Int32 blockId, Int32 featureId);

        /// <summary>
        /// Retrieves a list of all LineOfBusiness's features.
        /// </summary>
        /// <param name="blockId"> The block's Id.</param>
        /// <param name="featureId">The feature's Id.</param>
        /// <returns>
        /// The system x coordinate block feature.
        /// </returns>
        SysXBlocksFeature GetSysXBlockFeature(Int32 blockId, Int32 featureId);

        /// <summary>
        /// Retrieves a list of all LineOfBusiness features based on sysXBlockID.
        /// </summary>
        /// <param name="sysXBlockId">.</param>
        /// <returns>
        /// The system x coordinate block feature.
        /// </returns>
        SysXBlocksFeature GetSysXBlockFeature(Int32 sysXBlockId);

        #endregion

        #region Manage Tenants

        /// <summary>
        /// Get all contact types
        /// </summary>
        /// <returns>
        /// Returns all the contact types
        /// </returns>
        List<lkpContactType> GetAllContactTypes();


        /// <summary>
        /// Get all contact types
        /// </summary>
        /// <returns>
        /// Returns all the contact types
        /// </returns>
        List<lkpTenantType> GetAllTenantTypes();

        /// <summary>
        /// Gets the tenants.
        /// </summary>
        /// <returns>
        /// The tenants.
        /// </returns>        
        IQueryable<Tenant> GetTenants(Boolean SortByName, Boolean getTenantDetails = true);

        /// <summary>
        /// Retrieves a list of all active tenants of a user.
        /// </summary>
        /// <returns>
        /// The tenants.
        /// </returns>        
        IQueryable<Tenant> GetUserTenants(String currentOrgUserId);

        /// <summary>
        /// Retrieves a list of all active tenants of a client admin.
        /// </summary>
        /// <returns>
        /// The tenants.
        /// </returns>        
        IQueryable<Tenant> GetClientAdminTenants(String currentOrgUserId, Boolean isAtLoginTime = false);


        /// <summary>
        /// Gets the tenant.
        /// </summary>
        /// <param name="tenantId">The tenant id.</param>
        /// <returns>
        /// The tenant.
        /// </returns>        
        Tenant GetTenant(Int32 tenantId);

        /// <summary>
        /// Gets the tenant product id.
        /// </summary>
        /// <param name="tenantId">The tenant id.</param>
        /// <returns>
        /// The tenant product identifier.
        /// </returns>        
        Int32? GetTenantProductId(Int32 tenantId);

        /// <summary>
        /// Adds the tenant in transaction.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="contact">The contact.</param>
        /// <param name="tenant"> The tenant.</param>
        Tenant AddTenantInTransaction(Contact contact, Tenant tenant);

        List<DefaultRole> GetDefaultRolesByTenantTypeId(Int32 tenantTypeId);

        Boolean AddRoles(List<aspnet_Roles> roles);

        Boolean SetDefaultBusinessChannel(Int32 organizationUserId);

        Boolean SetSharedUserDefaultBusinessChannel(Int32 organizationUserId, Int32 sharedUserSysxBlockId);

        Boolean SetDefaultFeatures(Guid roleId);

        /// <summary>
        /// Deletes the tenant with all dependent.
        /// </summary>
        /// <param name="tenant">The tenant.</param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>        
        Boolean DeleteTenantWithAllDependent(Tenant tenant);

        /// <summary>
        /// Determines whether [is tenant exists] [the specified entered tenant name].
        /// </summary>
        /// <param name="enteredTenantName"> Name of the entered tenant.</param>
        /// <param name="existingTenantName">Name of the existing tenant.</param>
        /// <returns>
        /// <c>true</c> if [is tenant exists] [the specified entered tenant name]; otherwise,
        /// <c>false</c>.
        /// </returns>        
        Boolean IsTenantExists(String enteredTenantName, String existingTenantName = null);

        /// <summary>
        /// Return ZipCode by zipCodeNumber.
        /// </summary>
        /// <param name="zipCodeNumber">.</param>
        /// <returns>
        /// The zip codes by zip code number.
        /// </returns>
        IQueryable<ZipCode> GetZipCodesByZipCodeNumber(String zipCodeNumber);
        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <param name="tenantId">The tenant Id.</param>
        /// <returns>
        /// The connection string of Client.
        /// </returns>        
        String GetClientConnectionString(Int32 tenantId);

        //Tenant GetRootTenant(Int32 tenantId);

        Boolean IsTenantThirdPartyType(Int32 tenantId, String tenantTypeCode);

        #endregion

        #region Manage Sub Tenant

        /// <summary>
        /// Return the list of SupplierRelations by supplierId
        /// </summary>
        /// <param name="supplierId">Supplier Id.</param>
        /// <param name="isParent">Is Parent</param>
        /// <returns>Supplier relation entity.</returns>
        //IQueryable<ClientRelation> GetTenantRelationById(Int32 tenantId, Boolean isParent);

        /// <summary>
        /// retrieve a collection of child suppliers.
        /// </summary>
        /// <returns></returns>
        List<Tenant> GetChildTenants(Int32 tenantId, Boolean isParent);

        /// <summary>
        /// retrieve a list of all tenants except child tenants.
        /// </summary>
        /// <returns></returns>
        List<Tenant> GetAllTenantsForMapping(List<Int32> childTenantId);

        /// <summary>
        /// retrieve supplier relation byrelated supplier id and supplier id.
        /// </summary>
        /// <param name="supplierId">supplierId</param>
        /// <param name="relatedSupplierId">relatedSupplierId</param>
        /// <returns></returns>
        ClientRelation GetTenantRelationByRelatedTenantIdAndTenantId(Int32 tenantId, Int32 relatedTenantId);


        List<ClientRelation> GetClientRelationBasedonRelatedID(Int32 relatedTenantId);
        /// <summary>
        /// Get all child tenants.
        /// </summary>
        /// <param name="isParent"> Is Parent</param>
        /// <returns>Supplier Relation.</returns>
        IQueryable<ClientRelation> GetClientRelation(Boolean isParent, Int32 currentTenantId);

        Boolean AddClientRelation(List<ClientRelation> clientChildRelationList);
        #endregion

        #region Manage Users

        /// <summary>
        /// Gets the user by id.
        /// </summary>
        /// <param name="userId">              The user id.</param>
        /// <param name="loadMembership">      if set to <c>true</c> [load membership].</param>
        /// <param name="loadOrganizationUser">if set to <c>true</c> [load organization user].</param>
        /// <returns>
        /// The user by identifier.
        /// </returns>        
        aspnet_Users GetUserById(String userId, Boolean loadMembership, Boolean loadOrganizationUser);

        /// <summary>
        /// Retrieves all the data from the table OrganizationUser for the given Organization User Ids.
        /// </summary>
        /// <param name="lstUserIds">List of Organization User Ids</param>
        /// <returns>List of active organization users</returns>
        List<OrganizationUser> GetOrganizationUsersByIds(List<Int32?> lstUserIds);

        /// <summary>
        /// Gets the name of the user by.
        /// </summary>
        /// <param name="userName">            Name of the user.</param>
        /// <param name="loadMembership">      if set to <c>true</c> [load membership].</param>
        /// <param name="loadOrganizationUser">if set to <c>true</c> [load organization user].</param>
        /// <returns>
        /// The user by name.
        /// </returns>        
        aspnet_Users GetUserByName(String userName, Boolean loadMembership, Boolean loadOrganizationUser);

        /// <summary>
        /// Gets the user name by email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>
        /// The user name by email.
        /// </returns>        
        String GetUserNameByEmail(String email);

        ///UAT-2199: As an adb admin, I should be able to edit admin usernames on the manage users screen.
        /// <summary>
        /// Gets the user id by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        String GetUserIdByEmail(String email);

        /// <summary>
        /// UAT-2199: As an adb admin, I should be able to edit admin usernames on the manage users screen.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        String GetUserIdByUserName(String userName);

        /// <summary>
        /// Deletes the organization user.
        /// </summary>
        /// <param name="organizationUser">The organization user.</param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>        
        Boolean DeleteOrganizationUser(OrganizationUser organizationUser);

        /// <summary>
        /// Gets the list of users working in the organization associated with the given tenant Id. 
        /// </summary>
        /// <param name="tenantId">Tenant Id</param>
        /// <returns>List of acive Users</returns>
        List<OrganizationUser> GetOganisationUsersByTanentId(Int32 tenantId, Boolean IsAdbAdmin, Boolean IsComplAssignmentScreen, Boolean RotAssignmentScreen, Boolean IsDataEntryScreen, Boolean IsLocEnrollerScreen);

        List<OrganizationUser> GetOganisationUsersByTanentIdOfLoggedInUser(Int32 tenantId, Int32 currentloggedInUser);

        List<OrganizationUser> GetOganisationUsersByUserID(Int32 organizationUserID);

        List<OrganizationUser> GetOganisationUsersByUserIDForLogin(Int32 organizationUserID);
        #endregion

        #region Manage User's Account

        /// <summary>
        /// Failed the password attempt count.
        /// </summary>
        /// <param name="userName">               Name of the user.</param>
        /// <param name="maxPasswordAttemptCount">The max password attempt count.</param>        
        void FailedPasswordAttemptCount(String userName, Int32 maxPasswordAttemptCount);

        /// <summary>
        /// Resets the password attempt count.
        /// </summary>
        /// <param name="userName">Name of the user.</param>        
        void ResetPasswordAttemptCount(String userName);

        /// <summary>
        /// Query if 'currentUserId' is current user role exists.
        /// </summary>
        /// <param name="currentUserId">The current user id.</param>
        /// <returns>
        /// true if current user role exists, false if not.
        /// </returns>
        Boolean IsCurrentUserRoleExists(Guid currentUserId);

        /// <summary>
        /// Query if 'currentUserId' is current user role exists.
        /// </summary>
        /// <param name="currentUserId">The current user id.</param>
        /// <returns>
        /// true if current user role exists, false if not.
        /// </returns>
        Boolean IsCurrentLoggedInUserRoleExists(Guid currentUserId);

        /// <summary>
        /// Query if Line of business for current user exists.
        /// </summary>
        /// <param name="userName">The current user name.</param>
        /// <returns>
        /// Default line of business for the current user.
        /// </returns>
        lkpSysXBlock GetDefaultLineOfBusinessByUserName(String userName, Int32? tenantId);

        /// <summary>
        /// To get Third Party Organization IDs
        /// </summary>
        /// <param name="organizationID"></param>
        /// <returns></returns>
        List<Int32> GetThirdPartyOrgIDs(Int32 organizationID);

        #region To handle: last 5 password can't be used while user tries to change their password.

        /// <summary>
        /// Finds out that is the entered password has been used earlier.
        /// </summary>
        /// <param name="currentUserId">Current user's Id.</param>
        /// <param name="newPassword">The new password entered on the form.</param>
        /// <returns></returns>
        Boolean IsPasswordExistsInHistory(Guid currentUserId, String newPassword, Boolean isUserExixtInLocationTenants);

        /// <summary>
        /// Performs an update operation for Password Details.
        /// </summary>
        /// <param name="organizationUser">OrganizationUser.</param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        /// <exception cref="Exception">Thrown when an exception error condition occurs.</exception>
        Boolean UpdatePasswordDetails(OrganizationUser organizationUser, Int32 orgUsrID, String oldPassword);

        #endregion

        /// <summary>
        /// Check if the user belongs to Multi Tenants
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Boolean IsMultiTenantUser(Guid userId);

        #endregion

        #region Manage User and Roles

        /// <summary>
        /// Gets the roles by assign to role identifier.
        /// </summary>
        /// <param name="loadRoleDetails">The load role details.</param>
        /// <param name="assignToRoleId"> The value for assigned to role Id.</param>
        /// <returns>
        /// The roles by assign to role identifier.
        /// </returns>
        IQueryable<aspnet_Roles> GetRolesByAssignToRoleId(Boolean loadRoleDetails, String assignToRoleId);

        /// <summary>
        /// Get the Aspnet Role based on RoleId.
        /// </summary>
        /// <param name="roleId">String.</param>
        /// <returns>
        /// aspnet_Roles.
        /// </returns>
        aspnet_Roles GetAspnetRole(String roleId);

        /// <summary>
        /// Gets the roles.
        /// </summary>
        /// <param name="loadRoleDetails">if set to <c>true</c> [load role details].</param>
        /// <returns>
        /// The roles.
        /// </returns>        
        IQueryable<aspnet_Roles> GetRoles(Boolean loadRoleDetails);

        /// <summary>
        /// Roles the exists.
        /// </summary>
        /// <param name="roleName">Name of the role.</param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>        
        Boolean RoleExists(String roleName);

        /// <summary>
        /// Determines whether [is role in use] [the specified role id].
        /// </summary>
        /// <param name="roleId">The role id.</param>
        /// <returns>
        /// <c>true</c> if [is role in use] [the specified role id]; otherwise, <c>false</c>.
        /// </returns>        
        Boolean IsRoleInUse(String roleId);

        /// <summary>
        /// Gets the user roles.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>
        /// The user roles.
        /// </returns>        
        List<aspnet_Roles> GetUserRoles(String userName);

        /// <summary>
        /// Determines whether [is feature available to user] [the specified user name].
        /// </summary>
        /// <param name="userName"> Name of the user.</param>
        /// <param name="featureId">The feature id.</param>
        /// <param name="blockId">  The block id.</param>
        /// <returns>
        /// <c>true</c> if [is feature available to user] [the specified user name]; otherwise,
        /// <c>false</c>.
        /// </returns>        
        Boolean IsFeatureAvailableToUser(String userName, Int32 featureId, Int32 blockId);

        /// <summary>
        /// Gets the user roles by id.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns>
        /// The user roles by identifier.
        /// </returns>        
        List<aspnet_Roles> GetUserRolesById(String userId);

        /// <summary>
        /// Gets the users in role.
        /// </summary>
        /// <param name="roleName">Name of the role.</param>
        /// <returns>
        /// The users in role.
        /// </returns>        
        List<aspnet_Users> GetUsersInRole(String roleName);

        /// <summary>
        /// Determines whether [is user in role] [the specified user name].
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="roleName">Name of the role.</param>
        /// <returns>
        /// <c>true</c> if [is user in role] [the specified user name]; otherwise, <c>false</c>.
        /// </returns>        
        Boolean IsUserInRole(String userName, String roleName);

        /// <summary>
        /// Finds the users in role.
        /// </summary>
        /// <param name="roleName">       Name of the role.</param>
        /// <param name="userNameToMatch">The user name to match.</param>
        /// <returns>
        /// An enumerator that allows foreach to be used to find users in role in this collection.
        /// </returns>        
        IEnumerable<aspnet_Users> FindUsersInRole(String roleName, String userNameToMatch);

        /// <summary>
        /// Gets the application.
        /// </summary>
        /// <returns>
        /// The application.
        /// </returns>        
        aspnet_Applications GetApplication();

        /// <summary>
        /// Saves the role mapping.
        /// </summary>
        /// <param name="roleMapList">The role map list.</param>        
        void SaveRoleMapping(List<vw_aspnet_UsersInRoles> roleMapList);

        /// <summary>
        /// Gets the sys X admin user ids.
        /// </summary>
        /// <returns>
        /// An enumerator that allows foreach to be used to process get system x coordinate admin user
        /// identifiers in this collection.
        /// </returns>        
        IEnumerable<Int32> GetSysXAdminUserIds();

        /// <summary>
        /// Gets the roles by user id.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns>
        /// The roles by user identifier.
        /// </returns>        
        IQueryable<aspnet_Roles> GetRolesByUserId(String userId, Int32 organizationID = -1);

        /// <summary>
        /// Gets the roles by tenant's product id.
        /// </summary>
        /// <param name="tenantProductId">Current tenant's product id.</param>
        /// <returns>
        /// The roles by user identifier.
        /// </returns>     
        IQueryable<aspnet_Roles> GetAspnetRolesByTenantProductId(Int32 tenantProductId);

        /// <summary>
        /// Returns the value for tenant name based on any of the input provided.
        /// </summary>
        /// <param name="tenantId">The value for Tenant Id.</param>
        /// <param name="assignToProductId">The value for Product Id.</param>
        /// <param name="assignToDepartmentId">The value for department Id.</param>
        /// <returns></returns>
        String GetTenantName(Int32 tenantId, Int32 assignToProductId, Int32 assignToDepartmentId);

        /// <summary>
        /// Determines whether [is user exists] [the specified entered user name].
        /// </summary>
        /// <param name="enteredUserName"> Name of the entered user.</param>
        /// <param name="existingUserName">Name of the existing user.</param>
        /// <returns>
        /// <c>true</c> if [is user exists] [the specified entered user name]; otherwise, <c>false</c>.
        /// </returns>        
        Boolean IsUserExists(String enteredUserName, String existingUserName = null);

        #endregion

        #region Manage mapping of user and roles

        /// <summary>
        /// Users the role mapping.
        /// </summary>
        /// <param name="userId"> The user id.</param>
        /// <param name="roleIds">The role Ids.</param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>        
        Boolean UserRoleMapping(String userId, List<String> roleIds, Boolean deleteExistingRoles);

        /// <summary>
        /// Users the role mapping.
        /// </summary>
        /// <param name="userId"> The user id.</param>
        /// <param name="roleIds">The role Ids.</param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>        
        Boolean SharedUserRoleMapping(String userId, List<String> roleIds);

        #endregion

        #region Manage Departments

        /// <summary>
        /// Retrieves all departments for Super Admin.
        /// </summary>
        /// <returns>
        /// The departments for super admin.
        /// </returns>
        IQueryable<Organization> GetDepartmentsForSuperAdmin();

        /// <summary>
        /// Retrieves a list of all departments for Product admin.
        /// </summary>
        /// <param name="productId">The product id.</param>
        /// <returns>
        /// The departments for product admin.
        /// </returns>
        IQueryable<Organization> GetDepartmentsForProductAdmin(Int32 productId);

        /// <summary>
        /// Retrieves a list of all departments for Product admin.
        /// </summary>
        /// <param name="productId">    The product id.</param>
        /// <param name="currentUserId">The current user id.</param>
        /// <returns>
        /// A <see cref="Organization"/> list of data from the underlying data storage.
        /// </returns>
        IQueryable<Organization> GetDepartmentsForDepartmentAdmin(Int32 productId, Int32 currentUserId);

        /// <summary>
        /// Method to check department mapping
        /// </summary>
        /// <param name="organizationId">organizationId</param>
        /// <returns></returns>
        //Boolean IsDepartmentMapped(Int32 organizationId);


        #endregion

        #region Manage User Themes

        /// <summary>
        /// Updates the aspnet_membership table.
        /// </summary>
        /// <param name="aspnetMembership"></param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        Boolean UpdateAspnetMembershipForTheme(aspnet_Membership aspnetMembership);

        /// <summary>
        /// Gets the theme for the User.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>
        /// Logged-in UserId.
        /// </returns>
        aspnet_Membership GetAspnetMembershipById(Guid userId);

        #endregion

        #region UserGroup

        /// <summary>
        /// Get all user group collection
        /// </summary>
        /// <param name="IsAdmin">Boolean</param>
        /// <param name="CreatedById">Int32</param>
        /// <returns>IQueryable</returns>
        List<UserGroup> GetAllUserGroup(Boolean IsAdmin, Int32 CreatedById);

        /// <summary>
        /// Get all users in a group
        /// </summary>
        /// <param name="userGroupId">Int32 UserGroupId</param>
        /// <returns>IQueryable</returns>
        IQueryable<UsersInUserGroup> GetAllUsersInUserGroup(Int32 userGroupId);

        /// <summary>
        /// Map user group role
        /// </summary>
        /// <param name="userGroupId"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        Boolean UserGroupRoleMapping(Int32 userGroupId, List<String> roleIds);

        /// <summary>
        /// Get all role of user group.
        /// </summary>
        /// <param name="UserGroupId"></param>
        /// <returns></returns>
        List<aspnet_Roles> GetAllRoleOfUserGroup(Int32 UserGroupId);

        /// <summary>
        /// Add role of user group users.
        /// </summary>
        /// <param name="UsersInUserGroupID"></param>
        /// <param name="RoleInUserGroupIds"></param>
        /// <returns></returns>
        Boolean UserGroupsUserRoleMapping(Int32 UsersInUserGroupID, String UserId, List<String> RoleInUserGroupIds);

        List<aspnet_Roles> GetAllRoleOfUserGroupUser(String UserId);

        IEnumerable<Permission> GetUserGroupPermission(Guid userId, Int32 featureId, Int32 blockId);
        IEnumerable<Permission> GetUserGroupUserPermission(Guid userId, Int32 featureId, Int32 blockId);
        IQueryable<aspnet_Roles> GetRolesByUserIdInUserGroup(Int32 UserGroupId);
        List<UserGroupRolePermissionProductFeature> getUserGroupRolePermissionProductFeature(Guid RoleId, Int32 UserGroupId);

        #endregion

        #region Address

        #region City

        List<ZipCode> GetCityState(String zipCodeNumber);
        List<ZipCode> GetCityState(Int32 zipCodeId);

        /// <summary>
        /// Get applicant region information based on the selection
        /// </summary>
        /// <param name="zipCodeId"></param>
        /// <returns></returns>
        ZipCode GetApplicantZipCodeDetails(Int32 zipCodeId);

        #endregion
        #endregion


        #region Manage Organization User Location

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationUserId"></param>
        /// <param name="organizationLocationId"></param>
        /// <returns></returns>
        Boolean AddOrganizationUserLocation(Int32 organizationUserId, Int32 organizationLocationId);

        #endregion

        #region Manage Organization User Program

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationUserId"></param>
        /// <param name="programStudyId"></param>
        /// <returns></returns>
        //Boolean AddOrganizationUserProgram(Int32 organizationUserId, Int32 programStudyId);

        #endregion

        #region User Order placement Related Functionality

        void UpdateApplicanDetailsMaster(OrganizationUser orgUser, Dictionary<String, Object> dicAddressData, out Int32 addressMasterId, List<PreviousAddressContract> lstPrevAddress, PreviousAddressContract mailingAddress, out List<ResidentialHistory> lstResendentialHistory, ref List<PersonAlia> lstPersonAlias, Boolean isLocationServiceTenant);

        void SaveApplicantOrderProcessMaster(OrganizationUserProfile orgProfileMaster, Dictionary<String, Object> dicAddressData, out Int32 profileIdMaster, out Int32 addressIdMaster, out Guid addressHandlerId, List<PreviousAddressContract> lstPrevAddress, out List<ResidentialHistoryProfile> lstResidentialHistoryProfile, ref List<PersonAliasProfile> lstPersonAliasProfile, Boolean isLocationServiceTenant);

        List<PaymentIntegrationSetting> GetPaymentIntegrationSettingsByName(String name);

        List<PaymentIntegrationSettingClientMapping> GetPaymentIntegrationSettingsClientMappings();

        #endregion

        #region UserProgram
        //Boolean CopyOrganizationUserProgram(List<OrganizationUserProgram> organizationUserProgram);
        Boolean UpdateChanges();
        //List<OrganizationUserProgram> GetAllUserProgram(Int32 organizationUserId);
        //Boolean UpdateUserProgram(List<OrganizationUserProgram> lstOtganizationUserProgram, Int32 organizationUserId);
        #endregion

        #region Client DB Configuration

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <param name="tenantId">The tenant Id.</param>
        /// <returns>
        /// The connection string of Tenant.
        /// </returns>        
        Boolean GetTenantConnectionStringByConnectionStringAndUserId(String connectionString, Int32? tenantId);

        List<ClientDBConfiguration> GetClientDBConfiguration();

        AppConfiguration GetAppConfiguration(String key);

        //UAT-2319
        void UpdateAppConfiguration(String key, String UpdatedValue);

        #endregion

        #region Document

        Boolean CheckDocumentDeletionAllowed(Int32 applicantUploadedDocumentID, Int32 tenantID);
        void SynchronizeApplicantDocument(Int32 organisationUserId, Int32 tenantID, Int32 currentLoggedInUseID);
        #endregion

        #region EditProfile
        void AddAddressHandle(Guid addressHandleId);
        void AddAddress(Dictionary<String, Object> dicAddressData, Guid addressHandleId, Int32 currentUserId, Address addressNew, AddressExt addressExtNew = null);
        void AddMailingAddress(PreviousAddressContract mailingAddress, Guid addressHandleId, Int32 currentUserId, Address addressNew, AddressExt addressExtNew = null);
        Boolean SynchoniseUserProfile(Int32 organisationUserId, Int32 tenantID, Int32 orgUsrID);

        #endregion

        #region User Authorization Request

        void AddUserAuthRequest(UserAuthRequest userAuthRequest, Int32 currentUserId);
        void UpdateUserAuthRequest(UserAuthRequest userAuthRequest, Int32 currentUserId);
        void CancelPreviousAuthRequest(Int32 orgUserId, Int16 reqTypeId, Int32 loggedInUserId);
        UserAuthRequest GetUserAuthRequestByVerCode(String verificationCode);
        UserAuthRequest GetUserAuthRequestByEmail(String emailAddress);
        Int16 GetAuthRequestTypeIdByCode(String code);

        #endregion

        List<OrganizationUser> getOrganizationUserByIdList(List<Int32?> userIds);

        Guid UpdateMailingAddress(PreviousAddressContract mailingAddress, Boolean isLocationServiceTenant, Int32 orgUserID);

        /// <summary>
        /// Gets the list of existing users.
        /// </summary>
        /// <param name="userName">User Name</param>
        /// <param name="dOB">Date Of Birth</param>
        /// <param name="sSN">Social Security Number</param>
        /// <param name="firstName">First Name</param>
        /// <param name="lastName">Last Name</param>
        /// <returns>list of active users</returns>
        List<LookupContract> GetExistingUserLists(String userName, DateTime dOB, String sSN, String firstName, String lastName, Int32 defaultTenantId, String email, Boolean isApplicant = false, Boolean isSharedUser = false, String languageCode = default(String));

        /// <summary>
        /// Finds if the username is already present in tenant database
        /// </summary>
        /// <param name="userName">User Name</param>
        /// <param name="tenantId">Tenant Id</param>
        /// <returns>True if username exists</returns>
        Boolean IsUsernameExistInTenantDB(String userName, Int32 tenantId);
        #region System Service

        /// <summary>
        /// Get Active System Service Triggers
        /// </summary>
        /// <returns>List of System Service Trigger</returns>
        List<SystemServiceTrigger> GetSystemServiceTriggers();

        /// <summary>
        /// Get System Service Triggers By Id
        /// </summary>
        /// <returns>System Service Trigger</returns>
        SystemServiceTrigger GetSystemServiceTriggerByID(Int32 id);

        /// <summary>
        /// Get System Service Triggers By SystemServiceId
        /// </summary>
        /// <returns>System Service Trigger</returns>
        SystemServiceTrigger GetSystemServiceTriggerBySystemServiceID(Int16 systemServiceID, Int32 tenantID);

        /// <summary>
        /// Get System Service LookUp
        /// </summary>
        /// <returns>List of System Service LookUp</returns>
        List<lkpSystemService> GetSystemServiceLookUp();

        /// <summary>
        /// Get System Service by Code
        /// </summary>
        /// <param name="code"></param>
        /// <returns>System Service</returns>
        lkpSystemService GetSystemServiceByCode(String code);

        /// <summary>
        /// Add System Service Trigger
        /// </summary>
        /// <param name="systemServiceTrigger"></param>
        /// <returns>true/false</returns>
        Boolean AddSystemServiceTrigger(SystemServiceTrigger systemServiceTrigger);

        /// <summary>
        /// Update System Service Trigger
        /// </summary>
        /// <param name="systemServiceTrigger"></param>
        /// <returns>true/false</returns>
        Boolean UpdateSystemServiceTrigger(SystemServiceTrigger systemServiceTrigger);

        #endregion

        #region Temporary files for pdf conversion
        Boolean SavePageHtmlContentLocation(TempFile tempFile);
        List<TempFile> GetFilePath(Guid Id);
        Boolean DeleteTempFile(Guid Id, Int32 CurrentLoggedInUserID);
        #endregion

        /// <summary>
        /// Gets the list of client admin users working in the organization associated with the given tenant Id. 
        /// </summary>
        /// <param name="tenantId">Tenant Id</param>
        /// <returns>List of active Users</returns>
        List<OrganizationUser> GetClientAdminUsersByTanentId(Int32 tenantId);

        List<FeatureAction> GetFeatureAction(Int32 productFeatureID, Int32 rolePermissionID);

        List<FeatureRoleAction> GetRoleActionFeatures(Int32 featureID, Int32 userID, Int32 sysXBlockID);

        List<Tenant> GetTenantsBasedOnBusinessChannelType(short businessChannelTypeID);

        List<Entity.ClientEntity.Tenant> GetClientTenantsBasedOnBusinessChannelType(short businessChannelTypeID);


        List<FeatureRoleAction> GetRoleActionFeaturesTemp(Int32 featureID, Int32 userID, Int32 sysXBlockID);



        #region FeatureActionT

        DataTable GetFeatureActionT(Int32 productFeatureID);

        List<Permission> GetListOFPermissions();
        #endregion

        #region D & R Document Entity Mapping

        Boolean SaveUpdateDisclosureDocumentMapping(Entity.DisclosureDocumentMapping newDisclosureDocumentMapping);
        Entity.DisclosureDocumentMapping GetDisclosureDocumentMappingById(Int32 disclosureDocumentMappingId);

        #endregion

        DataTable GetBuisnessChannelTypeByTenantId(Int32 tenantID);

        #region Payment Instruction

        String GetPaymentInstruction(String paymentModeCode);

        /// <summary>
        /// Gets the list of all the Master lkpPaymentOption
        /// </summary>
        /// <param name="paymentModeCode"></param>
        /// <returns></returns>
        List<lkpPaymentOption> GetMasterPaymentOptions();

        #endregion
        Boolean RotationDataMovementOnAccountLinking(Guid SourceUserID, int CurrentUserLogIn);
        DataTable GetMultiInstitutionAssignmentData(String inuputXml, CustomPagingArgsContract gridCustomPaging);

        //AuthNetCustomerProfile GetCustomerProfile(Guid userId);
        AuthNetCustomerProfile GetCustomerProfile(Guid userId, int defaultTenantID, int tenantID);

        long CreateNewAuthNetCustomerProfile(Entity.AuthNetCustomerProfile authNetCustomerProfile);

        //List<ClientUserSearchContract> GetClientUserSearchData(String searchType, String tenantIDList, String agencyIDList, ClientUserSearchContract clientUserSearchContract, CustomPagingArgsContract customPagingArgsContract);

        //UAT-4257
        List<ClientUserSearchContract> GetClientUserSearchData(String searchType, String tenantIDList, String hierarchyNode, String agencyRootNodeIDList, String SelectedAgecnyIds, ClientUserSearchContract clientUserSearchContract, CustomPagingArgsContract customPagingArgsContract);


        #region DataFeedFormatter

        List<Tenant> GetListOfTenant();
        Boolean IsTenantActive(Int32 tenantId);
        //DataFeedSettingContract GetDataFeedSetting(Int32 tenantId);
        List<DataFeedSettingContract> GetDataFeedSetting(Int32 tenantId);
        DataFeedSettingContract GetDataFeedInvokeHistoryData(DataFeedSettingContract dataObject);
        Boolean SaveLogInInvokeHistory(String parameterXml, List<DataFeedInvokeHistoryDetail> lstdataFeedInvokeHistoryDetail, Int32 settingID,
                                        Int32 utilityUserId, Int32 dataFeedInvokeResultID);
        DataFeedFTPDetail GetDataFeedFTPDetail(Int32 dataFeedFTPDetailID);
        //lkpDataFeedInvokeResult GetDataFeedInvokeResult(Int32 dataFeedFTPDetailID);

        #endregion

        #region UAT-806 Creation of granular permissions for Client Admin users
        //Boolean GetUserGranularPermission(Int32 organizationUserId, List<Int32> lstSystemEntityIds, out Dictionary<String, String> dicPermissionList);
        Boolean GetUserGranularPermission(Int32 organizationUserId, out Dictionary<String, String> dicPermissionList);
        #endregion

        #region Manage System Entity Permission
        List<SystemEntityUserPermissionData> GetSystemEntityUserPermissionList(SearchItemDataContract searchDataContract, CustomPagingArgsContract gridCustomPaging, Int32 tenantID, Int32 selectedEntityId);
        List<OrgUser> GetOrgUserListForAsigningPermission(Int32 CurrentUserId, Int32 entityId, Int32 selectedTenantId);
        List<SystemEntityPermission> GetPermissionByEntityId(Int32 entityId);
        Boolean SaveUpdateEntityUserPermission(SystemEntityUserPermission systemEntityUserPermission, Int32 userId, Dictionary<int, bool> lstSelectedBkgOdrResPermissions);
        Boolean DeleteEntityUserPermission(Int32 systemEntityUserPermissionId, Int32 userId, Int32 organisationUserId, List<Int32> lstEntityPermissionIds,Int32? dpmId);
        #endregion

        #region User Last Login Activity.
        UserLoginHistory AddUserLoginActivity(Int32 organizationUserId, String currentSessionId);
        Boolean UpdateUserLoginActivity(Int32 organizationUserId, String currentSessionId, Boolean IsSessionTimeout, Int32 userLoginHistoryID);
        List<UserLoginHistory> GetApplicantLastLoginDetail(Int32 organizationUserId, String currentSessionId);
        #endregion

        #region Service Logging
        Boolean SaveServiceLoggingDetail(ServiceLoggingContract serviceLoggingContract);
        #endregion

        #region UAT-1049: Data Entry Enhanchment
        DataTable GetDataEntryQueueData(String inuputXml, CustomPagingArgsContract gridCustomPaging, Int32? CurrentLoggedInUserID);
        Boolean AssignDocumentToUserForDataEntry(Int32 selectedAssigneeUserId, String selectedAssigneeUserName, List<Int32> documentIdsToAssign, Int32 currentloggedInUserId);
        Boolean DeleteDocumentFromFlatDataEntry(Int32 applicantDocumentId, Int32 tenantId, Int32 applicantOrgUserId, Int32 currentloggedInUserId);

        /// <summary>
        /// Gets the FlatDataEntryQueue, by Primary Key of table
        /// </summary>
        /// <param name="fdeqId"></param>
        /// <returns></returns>
        FlatDataEntryQueue GetFlatDataEntryQueueRecord(Int32 fdeqId);

        #endregion

        #region Profile Sharing (Some Methods can not be moved to ProfileSharingRepo Beacause they are accessing security DB entities like OrganizationUser)

        /// <summary>
        /// Get the OrganizationUserID of the user to whom invitation is being sent or else return 0.
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        Int32 GetSharedUserOrgId(String emailAddress);

        /// <summary>
        /// Get the OrganizationUserID of the All the users to whom invitation is being sent by Admin, Client Admin
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        Dictionary<String, Int32> GetSharedUserOrgIds(List<String> lstEmailAddress);

        /// <summary>
        /// Returns whether the Shared user is being invited
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        Boolean IsSharedUserInvited(String emailAddress);

        //#region UAT-1237 Add Agency/shared users to client user search
        //DataTable GetSharedUserSearchData(INTSOF.UI.Contract.SearchUI.SharedUserSearchContract sharedUserSearchContract, CustomPagingArgsContract customPagingArgsContract);
        //List<GetSharedUserInvitationDetails_Result> GetSharedUserInvitationDetails(Int32 sharedUserID);
        //#endregion

        //void UpdateInvitationViewedStatus(Int32 currentUserID, Int32 invitationID);

        //List<InvitationDocument> GetAttestatationDocumentDetails(Int32 invitationGroupID);

        //InvitationDocument GetInvitationDocumentByDocumentID(Int32 invitationDocumentID);


        //List<Entity.ProfileSharingInvitationGroup> GetAttestationDetailsData(Int32 clientID, Int32 currentUserID, Int32 adminInitializedInvitationStatus);

        //#region Attestation Document Code

        //Int32 SaveAttestationDocument(String pdfDocPath, String documentTypeCode, Int32 currentLoggedInUserID);

        //Boolean SaveInvitationDocumentMapping(List<InvitationDocumentMapping> lstInvitationDocumentMapping);

        //#endregion

        //ProfileSharingInvitation GetInvitationDataByToken(Guid inviteToken);

        ///// <summary>
        ///// Gets the list of invitations that has been sent by the applicant
        ///// </summary>
        ///// <param name="applicantOrgUserId"></param>
        ///// <param name="tenantId"></param>
        ///// <returns></returns>
        //DataTable GetApplicantInvitations(Int32 applicantOrgUserId, Int32 tenantId);

        ///// <summary>
        ///// Check Whether shared user exists or not
        ///// </summary>
        ///// <param name="inviteToken"></param>
        ///// <returns></returns>
        //Boolean IsSharedUserExists(Guid inviteToken);

        ///// <summary>
        ///// Method to get Shared User Data from Invitation Sent by applicant(currently only Email)
        ///// </summary>
        ///// <param name="inviteToken"></param>
        ///// <returns></returns>
        //String GetSharedUserDataFromInvitation(Guid inviteToken);

        ///// <summary>
        ///// Method to Update Invitee Organization UserID in ProfileSharingInvitation table
        ///// </summary>
        ///// <param name="orgUserID"></param>
        ///// <param name="inviteToken"></param>
        ///// <returns></returns>
        //Boolean UpdateInviteeOrganizationUserID(Int32 orgUserID, Guid inviteToken, Int32 invitationStatusID);

        ///// <summary>
        ///// Save the New Invitation and return the ID of the invitation generated.
        ///// </summary>
        ///// <param name="invitationDetails"></param>
        ///// <returns>Tuple with InvitationID & its related Token</returns>
        //Tuple<Int32, Guid> SaveProfileSharingInvitation(InvitationDetailsContract invitationDetails, Int32 genaratedInvitationGroupID);

        ///// <summary>
        ///// Save the Bulk Invitations sent by admin/client admin
        ///// </summary>
        ///// <param name="lstInvitationDetails"></param>
        ///// <param name="invitationGroup"></param>
        ///// <param name="generatedInvitationGroupID"></param>
        ///// <returns></returns>
        //List<ProfileSharingInvitation> SaveAdminInvitations(List<InvitationDetailsContract> lstInvitationDetails, ProfileSharingInvitationGroup invitationGroup);

        ///// <summary>
        ///// Gets the master details for the selected Invitation
        ///// </summary>
        ///// <param name="invitationId"></param>
        ///// <returns></returns>
        //ProfileSharingInvitation GetInvitationDetails(Int32 invitationId);

        //void UpdateInvitationStatus(Int32 statusId, Int32 invitationId, Int32 currentUserId);

        ///// <summary>
        ///// Update the Status of the Invitation
        ///// </summary>
        ///// <param name="statusId"></param>
        ///// <param name="invitationId"></param>
        ///// <param name="currentUserId"></param>
        //void UpdateBulkInvitationStatus(Int32 statusId, List<Int32> invitationId, Int32 currentUserId);



        //DataTable GetInvitationData(InvitationSearchContract searchContract, CustomPagingArgsContract gridCustomPaging);

        ///// <summary>
        ///// Update the Views remaining of the Invitation
        ///// </summary>
        ///// <param name="statusCode"></param>
        ///// <param name="invitationId"></param>
        ///// <param name="currentUserId"></param>
        //Boolean UpdateInvitationViewsRemaining(Int32 invitationId, Int32 currentUserId, Int32 expiredInvitationTypeId);

        ///// <summary>
        ///// Update Notes of the Invitation
        ///// </summary>
        ///// <param name="inviteeNotes"></param>
        ///// <param name="invitationId"></param>
        ///// <param name="currentUserId"></param>
        //Boolean UpdateInvitationNotes(Int32 invitationId, Int32 currentUserId, String notes);

        //List<Agency> GetAllAgency(Int32 institutionID);

        //IEnumerable<ProfileSharingInvitation> GetInvitationsByInviteeOrgUserID(Int32 inviteeOrgUserID);

        //List<usp_GetAgencyUserData_Result> GetAgencyUserData(Int32 institutionID, Int32 agencyID);

        //DataTable GetAttestationDocumentData(String clientInvitationIds);

        //Int32 GenarateNewInvitationGroup(ProfileSharingInvitationGroup invitationGroupObj);

        //#region Profile Sharing Attestion Document
        //InvitationDocument GetInvitationDocuments(Int32 invitationId);
        //#endregion
        #endregion

        #region UAT-1176/UAT-1178 EMPLOYMENT DISCLOSURE AND USER ATTESTATION DISCLOSURE
        Entity.SystemDocument GetSystemDocumentByDocTypeID(Int32 docTypeID);
        Boolean SaveEDDetails(Int32 organizationUserID);
        #endregion

        #region UAT-1086 WB: creation of video tutorial widget for admin (client and ADB) dashboard

        IEnumerable<ApplicationVideo> GetApplicationVideos();

        ApplicationVideo GetApplicationVideo(Int32 applicationVideoID);

        #endregion

        #region UAT-1178 USER ATTESTATION DISCLOSURE
        Boolean CheckForClientRoleFeatures(Guid userID, List<short> lstbusinessChannelTypeID);
        //Boolean CheckForBkgInvitation(Int32 orgUserID);
        #endregion

        UserAttestationDetail SaveUpdateUserAttestationDocument(UserAttestationDetail userAttestationDetails, Boolean isUpdateMode);

        UserAttestationDetail GetPartiallyFilledUserAttestationDocument(Int32 userAttestationDocumentID);

        Boolean IsAttestationDocumentAlreadySubmitted(Int32 orgUserID);

        List<GetEmploymentDocumentUserInfo_Result> GetEmploymentDisclosureUserInfo(int orgUserID);

        #region GetMappedOrganizationUsers
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
        DataTable GetMappedOrganizationUsers(Int32? currentUserId, Int32? tenantProductId, Boolean? isAdmin, Int32? organizationID,
            Boolean? isApplicantCheckRequred, Boolean? isParentOrgCheckRequired, String roleId, CustomPagingArgsContract sortingAndFilteringData);

        #endregion

        Boolean SaveAccountCreationContactList(List<AccountCreationContact> accountCreationContactList);

        #region UAT-1218: Any User should be able to be 1 or more of the following: Applicant, Client admin, Agency User, Instructor/Preceprtor
        Int32 IsExistingUserInvited(String emailAddress);
        List<OrganizationUserTypeMapping> GetOrganizationUserTypeMapping(Guid organizationUserID);
        void AddOrganizationUserTypeMapping(Int32 orgUserID, Int32 userTypeID);
        Boolean IsUsernameExistInSecuritytDB(String userName);
        OrganizationUser GetOrgUserIfUsernameExistInSecuritytDB(String userName);

        #endregion

        #region UAT 1304 Instructor/Preceptor screens and functionality
        Boolean UpdateClientContactOrganisationUser(INTSOF.ServiceDataContracts.Modules.Common.OrganizationUserContract organizationUserContract, String userID);
        #endregion

        List<ExternalVendorFieldOption> GetExternalVendorFieldOptionByVendorID(int vendorID);


        List<OrganizationUser> GetOrganizationUserByIds(List<int> lstOrgUserId);
        String GetFormattedString(Int32 orgUserID, Boolean isOrgUserProfileID);
        List<SystemEntityUserPermissionData> GetSystemEntityUserPermissionData(Int32 organizationUserID);

        #region Announcement
        List<AnnouncementContract> GetAnnouncementDetail();
        Boolean SaveUpdateAnnouncement(AnnouncementContract announcementContract, Int32 currentUserId);
        Boolean DeleteAnnouncement(Int32 announcementID, Int32 currentUserId);
        AnnouncementContract GetAnnouncementPopupDetail(Int32 announcementID);
        Boolean SaveAnnouncementMapping(Int32 announcementID, Int32 currentUserId);
        List<Int32> GetAnnouncements(Int32 currentUserId);

        #endregion

        #region Bulletin

        /// <summary>
        /// Method to get data for binding grid.
        /// </summary>
        /// <param name="selectedInstitutionIds"></param>
        /// <returns></returns>
        List<BulletinContract> GetBulletin(string selectedInstitutionIds);

        /// <summary>
        /// Method to save and update Bulletin Contract 
        /// </summary>
        /// <param name="bulletinContract"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        Int32 SaveUpdateBulletin(BulletinContract bulletinContract, Int32 currentUserId);

        /// <summary>
        /// Method to delete Bulletin.
        /// </summary>
        /// <param name="BulletinID"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        Boolean DeleteBulletin(Int32 BulletinID, Int32 currentUserId);
        #endregion

        #region Bulletins Popup
        BulletinContract GetBulletinPopupDetail(Int32 bulletinID);
        Boolean SaveBulletinMapping(Int32 bulletinID, Int32 currentUserID);

        #endregion

        /// <summary>
        /// UAT 1529
        /// </summary>
        /// <param name="lstUserIDs"></param>
        /// <returns></returns>
        List<AgencyUserPermissionContract> GetOrganizationUserListByUserIds(String userIDsXML);

        List<ManageRoleContract> GetRolesMappedWithUserType(String SelectedUserTypeIds);

        void InsertBulkRoleFeatures(String featuresIds, String userTypeIds, String roleDetailIds, Int32 currentLoggedinUserId);

        List<Entity.SystemDocument> GetSystemDocumentListByDocTypeID(Int32 docTypeID);

        Boolean DeleteDocsFromDataEntryQueueForGraduated(Int32 tenantID, Int32 organizationUserID, Int32 orgUsrId);

        List<APIMetaData> GetAPIMetaDataList();

        Boolean UpdateStatusOfAppDocumentForDataEntry(List<Int32> lstUsers, Int32 currentloggedInUserId, Int16 docStatusId, Int32 tenantID);

        #region Invoice Group

        List<InvoiceGroupContract> GetInvoiceGroupDetails();
        Boolean SaveUpdateInvoiceGroupInformation(Int32 currentLoggedInUserId, Int32 invoiceGroupId, String invoiceGroupName, String invoiceGroupDesc, List<String> selectedDPMIds, List<Int32> lstSelectedReportColumnIds);
        Boolean DeleteInvoiceGroupInformation(Int32 currentLoggedInUserId, Int32 invoiceGroupId);

        #endregion

        ReconciliationQueueConfiguration GetCurrentReconciliationAssignmentConfiguration();

        //List<Entity.ClientEntity.GetQueueAssigneeList> GetCurrentReconciliationAssigneeList(Int32 currentAssignmentConfigurationId);

        //List<Entity.ClientEntity.GetUserListApplicableForReview> GetUserListApplicableForReconciliationReview(int assignmentConfigurationId, int currentReviewerId);

        Boolean SaveUpdateReconciliationQueueAssignmentConfiguration(ReconciliationQueueConfiguration queueAssignmentConfiguration, Int32 currentLoggedInUserId);

        #region  Data Reconciliation Queue

        List<DataReconciliationQueueContract> GetQueueData(string selectedInstitutionIds, CustomPagingArgsContract gridCustomPagingus);
        //UAT-1718 : Get reconciliation Item ids list
        List<DataReconciliationQueueContract> GetReconciledItemsList(String institutionIds, Int32 ComplianceItemReconciliationDataID);
        #endregion

        Dictionary<int, bool> CheckIfItemsAreInReconciliationProcess(List<int> itemDataIDs, int tenantID);

        #region UAT-1741:604 notification should only have to be clicked upon login once per 24 hours.

        EmploymentDisclosureDetail GetEmploymentDisclosureDetails(Int32 organizationUserID);

        #endregion

        //UAT-2264
        List<ColumnsConfigurationContract> GetScreenColumns(String ScreenName, Int32 CurrentLoggedInUserID);
        //UAT-2264
        Boolean SaveUserScreenColumnMapping(Dictionary<Int32, Boolean> columnVisibility, Int32 CurrentLoggedInUserID, Int32 OrganisationUserID);
        //UAT-2264
        List<String> GetScreenColumnsToHide(String GridCode, Int32 CurrentLoggedInUserID);
        //UAT-2266
        String GetAssignedUserRoleNames(Int32 OrgUserId, Int32 TenantId);
        //UAT-2296
        Boolean UpdateFlatDataEntryQueueRecord(Int32 applicantDocumentId, short InProgressDocumentStatus, Int32 OrgID, Int32 tenantID);

        #region UAT-2302-As a student, I should see a popup on login if i have logged in using a non-preferred browser.
        Boolean CheckUserNonPrefferedBrowserOption(Int32 currentLoggedInUserID, Int16 utilityFeaturesId);
        Boolean SaveNonPreferredBrowserLog(Int32 currentLoggedInUserID, Int16 utilityFeaturesId);
        #endregion

        #region UAT-2304: Random review of auto completed supplements

        SupplementAutomationConfiguration GetCurrentSupplementAutomationConfiguration();
        Boolean SaveUpdateSupplementAutomationConfiguration(SupplementAutomationConfiguration supplementAutomationConfiguration, Int32 currentUserID);

        #endregion

        #region UAT-2515
        Tuple<Int32, Int32> ExternalUserTenantId(String ExternalID, String mappingCode); //UAT-2515
        List<ExternalLoginDataContract> GetMatchingOrganisationUserList(ExternalLoginDataContract objExternalLoginDataContract); //UAT-2515
        Boolean ValidateToken(String token);//UAT-2515
        Dictionary<Int32, Int32> ExternalUserTenantIdBySchoolName(String token, String schoolName); //UAT-2515 
        Boolean InsertExternalUserRegistrationLogInIntegrationClientOrganizationUserMap(Int32 organizationUserId, Int32 integrationClientId, String externalId);//UAT-2515
        List<ExternalDataFromTokenDataContract> GetDataFromSecurityToken(String tokenId);
        #endregion

        //UAT-2448
        List<CountryIdentificationDetailContract> GetCountryIdentificationDetails();

        #region Get ClientDBConfig for selected tenantids
        ClientDBConfiguration GetClientDBConfigurationForSelectedTenants(Int32 TenantId);
        #endregion


        #region UAT-2792
        Boolean SaveShibbolethSSOSessionData(String SessionData, String TargetURL);
        Int32 GetIntegrationClientForShibboleth(String MappingGroupCode);
        List<ExternalLoginDataContract> GetMatchingUsersForShibboleth(String shibbolethUniqueId, String shibbolethAttributeId, String shibbolethemail, Int32 TenantID, Boolean isApplicant, String shibbolethHandlerType, String ShibbolethFirstName, String ShibbolethLastName, String ShibbolethRoleString);//UAT-2883
        Boolean GetShibbolethSettingForHistoryLogging();
        Boolean ShibbolethPeopleSoftIDEntryInIntegrationClientOrganizationUserMap(Int32 organizationUserId, Int32 integrationClientId, String externalId);
        #endregion

        List<String> GetClientIdpUrl(String ClientUrl); //UAT-2792

        #region UAT-2696
        Boolean IsCustomAttributeChecked(Int32 organizationUserId, String uniqueName, String grdCode);
        #endregion

        #region UAT-2842
        OrganizationUser GetOrganizationUserOfAdminOrder(Guid UserID);
        Boolean DeleteAdminOrganizationUser(OrganizationUser OrgsUser, Int32 currentLoggedInUserId);
        #endregion

        String CheckIfUserAlreadyAssigned(String tenantXML, Int32 AssignToUserID); //UAT 2809
        String GetClientByHostName(String ClientUrl);//UAT-2883
        Boolean IsRandomGeneratedPassword(String ShibbolethHandlerType);//UAT-2958

        DataTable GetIntegrationList(Int32 organizationUserId); //UAT-2918

        #region UAT-2930
        Boolean SaveTwoFactorAuthenticationData(String UserId, String AuthenticationTitle, String AuthenticationCode, Int32 CurrentLoggedInUserID, String AuthenticationModeCode);
        UserTwoFactorAuthentication GetTwofactorAuthenticationForUserID(String userId);
        Boolean VerifyTwofactorAuthenticationForUserID(String UserId, Int32 CurrentLoggedInUserID);
        Boolean DeleteTwofactorAuthenticationForUserID(String UserId, Int32 CurrentLoggedInUserID);
        IQueryable<Tenant> GetUserTenantsForAlltypesOfUsers(String currentOrgUserId);

        #endregion

        #region UAT-2310:- Tracking Assignment Efficiencies
        Boolean SaveAdminsConfig(List<TrackingAssignmentConfigurationContract> lstAdminsConfig, Int32 currentLoggedInUserId);
        List<TrackingAssignmentConfigurationContract> GetAdminTrackingAssignmentConfiguration();
        Boolean UpdateConfiguration(TrackingAssignmentConfigurationContract trackingConfigurationContract, Int32 currentLoggedInUserId, List<TrackingConfigObjectMappingContract> lstTrackObjMappingToDelete);
        Boolean DeleteConfiguration(Int32 TAC_ID, Int32 currentLoggedInUserID);
        #endregion

        #region UAT-3052
        Dictionary<String, String> GetItemListForReportsByTenantIdLoggedInEmail(String tenantIds, String loggedInEmailId);        

        List<Entity.ClientEntity.Tenant> GetTenantsByTenantId(String tenantIds);
        #endregion

        Dictionary<String, String> GetAllItemListForReportsByTenantIdLoggedInEmail(String tenantIds, String loggedInEmailId);//[UAT-4509]

        #region UAT-3068
        String GetUserAuthenticationUseTypeForUserID(String UserId);
        Boolean SaveAuthenticationData(String userID, String authenticationType, Int32 currentUserId);
        String GetUserIdFromOrgUserId(Int32 currentUserId);

        #endregion

        #region UAT-3032

        List<RoleConfigurationContract> GetRolesSetting();
        Boolean SaveRolePreferredTenantSetting(RoleConfigurationContract roleSettingContract, Int32 currentLoggedInUserId);
        Boolean DeleteRolePreferredTenantSetting(Int32 rolePreferredTenantSettingId, Int32 currentLoggedInUserId);
        Int32 GetPreferredSelectedTenant();
        Boolean IsUserAllowedPreferredTenant(String userID);
        #endregion

        #region UAT-3054
        Boolean IsStudentEmailDOBLastNameExist(Int32 tenantID, String emailId, String lastName, DateTime? dob);
        String GetUserNameByExternald(String externalId, Int32 integrationClientID);
        #endregion

        #region UAT-3067
        Dictionary<String, String> GetCustomAttrMappedWithShibbolethAttr(String mappingGroupCode);
        #endregion

        #region UAT-3075
        List<CompliancePriorityObjectContract> GetCompliancePriorityObjects();
        Boolean SaveComplPriorityObject(Int32 currentLoggedInUserID, CompliancePriorityObjectContract compPriorityObject);
        Boolean DeleteComplPriorityObject(Int32 currentLoggedInUserID, Int32 compPriorityObjectID);
        List<TenantUserMappingContract> GetTenantUserMappings();
        Boolean SaveTenantUserMapping(List<TenantUserMappingContract> lstTenantUserMappings, Int32 currentLoggedInUserID);
        Boolean UpdateTenantUserMapping(Int32 currentLoggedInUserID, TenantUserMappingContract tenantUserMapping);
        List<Int32> GetUsersMappedWithTenant(Int32 tenantID);
        Boolean DeleteTenantUserMapping(Int32 currentLoggedInUserID, Int32 tenantUserMappingId);
        //  Boolean SaveTrackingConfObjMapping(List<TrackingConfigObjectMappingContract> lstTrackingConfObjMapping, Int32 currentLoggedInUserId);
        List<TrackingConfigObjectMappingContract> GetTrackConfigObjectMapped(Int32 trackConfigId);
        #endregion

        Boolean IsIntegrationClientOrganisationUser(Int32 userID, String MAPPING_GROUP_CODE_UCONN); //UAT-3133
        #region UAT-3112
        List<SystemDocument> GetBadgeFormDocuments(Int32 docTypeID);
        #endregion

        Dictionary<String, String> GetCategoryListFilterForLoggedInAgencyUserReports(String selectedTenantIDs, String loggedInUserEmailId); //UAT -3143

        Dictionary<String, String> GetAllCategoryListFilterForLoggedInAgencyUserReports(String selectedTenantIDs, String loggedInUserEmailId); //[UAT-4509]
        
        #region UAT-2960
        Boolean UpdateApplicantForAlumniAccess(Int32 currentLoggedInUserId);//UAT-2960
        List<OrganizationUserAlumniAccess> GetApplicantDataForEmail(Int32 chunkSize, Int32 currentLoggedInUserId, Int32 tenantId);//2960
        Boolean UpdateStatusInOrganizationUserAlumniAccess(Int32 OrgUserAlumniAccessId, String emailStatusCode, Int32 currentLoggedInUserId);
        Boolean CheckForAlumnAccessStatus(String StatusInitiatedCode, Guid Token, Int32 orgUserId);
        Boolean ComplianceDataMovementInsertLog(Int32 sourceTenantID, Int32 targetTenantID, Int32 pkgSubscriptionID, Int32 complianceDatamovementStatusID, Int32 currentLoggedInUserID);
        void ComplianceDataMovementUpdateLog(Int32 orderID, Int32 tenentID, Int32 pkgSubscriptionID, Int32 complianceDatamovementStatusID, Int32 currentLoggedInUserID);
        String GetAlumniSettingByCode(String AlumniTenantCode);
        List<AlumniPackageSubscription> CopyComplianceDataToCompliance(Int32 backgroundProcessUserId, Int32 chunkSize);
        List<Int32> GetOrganizationUserAlumniAccessIds(Int32 currentLoggedInUserID, Int32 TenantID);
        Boolean CheckForAlumnAccessStatusDue(Int32 orgUserId, Int32 tenantId, String statusCode);
        Boolean AddAlumniActivationDetails(Int32 sourceTenantID, Int32 orgUserAlumniAccessID, Int32 orderID, Int32 pkgSubscriptionID, Int32 currentLoggedInUserId);
        OrganizationUserAlumniAccess UpdateOrganizationUserAlumniAccess(Int32 aluminAccessId, String alumniStatus, Int32 currentLoggedInUserId);
        Boolean IsMultiTenantUserExceptAlumni(Guid userId);
        #endregion

        Boolean InsertDefaultColumnConfiguration(Int32 CurrentUserId, String userId, String newMappedRoles);//UAT-3228
        Boolean CopyDefaultColumnConfiguration(Int32 tenantId, Guid CopyFromUserId, Int32 CopyToOrganizationUserID, Int32 CurrentLoggedInUserId);//UAT-3228

        List<BookmarkedFeatureContact> GetAccessibleFeatures(Int32 orgUserID, Int32 tenantID, Int32 sysxBlockID, bool isSuperAdmin);

        Boolean SaveUpdateBookmarkedFeatures(Int32? tenantProductSysXBlockID, Int32 productFeatureID, Int32 orgUserID, bool isBookmarked);

        #region [UAT-3326] Creation of internal notes function (like on portfolio details) for client admins on the Client User Details screen.
        DataTable GetAdminProfileNotesList(Int32 organizationUserID);
        Boolean SaveUpdateAdminProfileNotes(List<ApplicantProfileNotesContract> adminProfileNoteList);
        Boolean SaveAdminProfileNotes(AdminProfileNote adminProfileNoteObj);
        AdminProfileNote GetAdminProfileNotesByNoteID(Int32 adminProfileNoteID);
        Boolean UpdateAdminProfileNote();
        #endregion
        DataTable GetClientDataForAgencyAndAgencyHierarchy(Int32 LoggedInUserId, String AGencyID, String Tenantid);

        #region UAT-3364
        List<SystemEntityUserPermission> GetRotationCreatorGranularPermissionsByOrgUserID(Int32 orgUserID);
        #endregion

        #region UAT-3347 : "Client Login" functionality from new screen
        List<ClientLoginSearchContract> GetClientLoginSearchData(String tenantIDList, ClientLoginSearchContract clientUserSearchContract, CustomPagingArgsContract customPagingArgsContract);
        Boolean AddImpersonationHistory(Int32 clientAdminUserID, Int32 CurrentLoggedInUserID);
        #endregion

        #region UAt-3360
        List<LookupContract> GetExistingUserProfileLists(String userName, String email, Int32 defaultTenantId);
        OrganizationUser GetOrgUserIfUsernameExistInSecuritytDBForAccountLinking(String userName);
        List<OrganizationUser> GetOrgUserListIfUsernameExistInSecuritytDBForAccountLinking(String userName);
        #endregion

        Boolean CheckRoleForShibbolethNYU(Boolean IsApplicantRoleCheck, String Roles);//UAT-3540
        Boolean CheckRoleForShibbolethUCONN(Boolean IsApplicantRoleCheck, String Roles);

        #region UAT-3461
        //List<ManageRandomReviewsContract> GetManageRandomReviewsList(Int32? queueConfigurationID, Int32? tenantId); //UAT-3461
        Boolean DeleteReconciliationQueueConfiguration(Int32 queueConfigurationID, Int32 currentLoggedInID);
        //String SaveReconciliationQueueConfiguration(Int32 queueConfigurationID, Int32 tenantID, String description, Decimal percentage, Int32 reviews, Int32 currentLoggedInID);
        #endregion


        Int32 GetTenantIdByOrganizationUserID(Int32 OrganizationUserID);
        Boolean CheckIfUserIsEnroller(string userID);
        Boolean IsLocationServiceTenant(Int32 tenantID);
        List<Tenant> GetListOfTenantWithLocationService();
        int GetTenantID(int webSiteId);
        String GetLocationTenantCompanyName();

        //UAT-3669
        Boolean UpdateBlockedOrdersHistoryData(Int32 TenantId, Int32 selectedHierarchyNodeID, String selectedPackageIds, Int32 applicantOrgUserId, String firstName, String lastName, Int32 blockedReasonId);
        //UAT-3734

        String GetLocTenMaxAllowedDays();
        String GetInstructorNameByOrganizationUserId(Int32 organizationUserId);

        #region UAT-3744
        List<DataReconciliationQueueContract> GetNextActiveReconciledItem(String selectedInstitutionIds, Int32 complianceItemReconciliationDataId);
        #endregion

        //CBI|| CABS || Get Suffixes
        List<lkpSuffix> GetSuffixes();

        List<lkpAdminEntrySuffix> GetAdminEntrySuffixes();

        #region UAT-3824

        Boolean AddUpdateLanguageMapping(OrganizationUser orgUser, Int32? SelectedCommLang);
        Int32? GetSelectedlang(Guid UserID);
        //Guid GetOrganizationUserIDByID(Int32 OrganisationUserId,Int32 TenantId);
        Int32 GetLanguageIdByGuid(Int32? OrganisationUserId, Int32? TenantId, out Int32 defaultLanguageId);
        int GetSuffixIdBasedOnSuffixText(string suffix);
        string GetCountryByCountryId(int countryId);
        #endregion

        #region UAT-3910
        List<LookupContract> GetLocationSpecifictenantAllCountriesList(Boolean isStateSearch, Int32 countryId);

        #endregion

        #region Globalization

        lkpLanguage GetLanguageCulture(Guid userId);

        #endregion

        String GetUserPreferLanguageCode(Int32 UserId, Int32 TenantId);

        #region UAT-4097
        Boolean IsUserExixtInLocationTenants(Guid currentUserId);

        Boolean IsPasswordNeedToBeChanged(Guid currentUserId, Int32 expiryDays);

        #endregion

        //UAT-4013
        List<RotationMemberSearchDetailContract> GetInstrctrPreceptrRotationStudents(String tenantID, Guid userID, RotationMemberSearchDetailContract searchContract, CustomPagingArgsContract customPagingArgsContract);
        Dictionary<Int32, String> GetOrgUserDetailsByOrgUserID(List<Int32> lstOrgUserIDs);

        #region UAT-4114
        List<ManageRandomReviewsContract> GetAllManageRandomReviewsList();
        #endregion
        List<ExternalLoginDataContract> GetMatchingOrganisationUserListForCoreLinking(ExternalLoginDataContract objExternalLoginDataContract);

        Boolean LinkOtherAccount(Guid SourceUserID, Guid TargetUserID, Int32 currentLoggedInUserId);
        Tuple<LookupContract, Boolean> GetExistingUserBasedOnEmailId(String emailAddress, Guid userid, Int32 currentLoggedInUserId);
        OrganizationUser GetSharedUserOrganizationUser(Guid userId);
        Boolean IsOrganizationUserExistsForEmail(String emailAddress);

        #region UAT-4270
        Boolean GetNotificationNeedToSendForEnroller(Int32 OrgUserID);
        String GetEnrollerPhoneNumberForSMSNotification(Int32 OrgUserID);
        #endregion

        String ValidateAccountName(List<String> lstAccountName);
        String ValidateCBIUniqueID(List<String> lstCBIUniqueID);

        #region Admin Entry Portal

        CrossApplicationData GetSessionUsingToken(Guid? token);
        Boolean SetSessionData(CrossApplicationData applicationData);
        void UpdateSessionActiveState(Guid? token);
        AdminEntryUserLoginContract GetAdminEntryUserByToken(Guid tokenKey);
        Boolean IsTokenExpired(Int32 tenantId, Int32 orderId, String TokenKey);
        OrganizationUser GetOrganizationUserDetailForAdminOrder(Int32 organizationUserId);
        Boolean UpdateApplicatInviteToken(Int32 tenantId, Int32 orderId);

        List<PersonAliasProfile> GetUserPersonAliasProfiles(Int32 organizationUserProfileId);
        #endregion

        #region UAT-4454
        Boolean RemoveIntegrationClientOrganizationUserMapping(Int32 IntegrationClientOrganizationUserMapID, Int32 OrganizationUserID);
        #endregion

        Boolean CheckRoleForShibbolethNSC(Boolean IsApplicantRoleCheck, String Roles);
        //Release 181:4998
        Boolean CheckRoleForShibbolethRoss(Boolean IsApplicantRoleCheck, String Roles);
        Boolean CheckRoleForShibbolethUpennDental(Boolean IsApplicantRoleCheck, String Roles);
        Boolean GetMenuItemsForRedirection(string UserID, Int32 BlockID, Int32 BusinessChannelTypeID);

        Boolean IsSystemEntityUserPermissionExists(Int32 organizationUserID, Int32 entityId, Int32? dpmId);

        #region UAT-4592
        /// <summary>
        /// Get Disclaimer document on the basis of System Document ID
        /// </summary>
        /// <param name="systemDocumentId"></param>
        /// <returns></returns>
        SystemDocument GetSystemDocumentByID(Int32 systemDocumentID);
        #endregion
        Boolean CheckRoleForShibbolethBSU(Boolean IsApplicantRoleCheck, String Roles);

        String CheckRoleForBSU(String Email, String FirstName, String LastName);
        /// <summary>
        /// Release 181:4998
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="FirstName"></param>
        /// <param name="LastName"></param>
        /// <returns></returns>
        String CheckRoleForROSS(String Email, String firstName, String lastName,String userName);
        Int32 GetFeeItemID();

        Int32 GetAdditionalServiceFeeItemID();
        bool InsertReconciliationProductivityData(List<RecounciliationProductivityData> lstProductivitydata, int tenantID, int UserId, DateTime startDT);

        DateTime? GetReconciliationJobHistoryDate(int tenantID);
        bool InsertUpdateReconciliationProductivityData(RecounciliationProductivityData objProductivitydataint);


    }
}