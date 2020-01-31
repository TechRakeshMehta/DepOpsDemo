#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  SecurityManager.cs
// Purpose:
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Configuration;

#endregion

#region Application Specific

using System.Text;
using Entity;
using DAL.Interfaces;
using INTSOF.Utils;
using System.Web;
using System.IO;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.UI.Contract.SysXSecurityModel;
using System.Data;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.UI.Contract.DataFeed_Framework;
using NLog;
using INTSOF.UI.Contract.Services;
using INTSOF.UI.Contract.ProfileSharing;
using iTextSharp.text.pdf;
using INTSOF.UI.Contract.SearchUI;
using INTSOF.UI.Contract.Templates;
using INTSOF.UI.Contract.PackageBundleManagement;
using INTSOF.UI.Contract.IntsofSecurityModel;
using INTSOF.UI.Contract.CommonControls;
using INTSOF.UI.Contract.PersonalSettings;
using INTSOF.UI.Contract.Globalization;
using INTSOF.UI.Contract.AdminEntryPortal;
using INTSOF.UI.Contract.RecounciliationQueue;
#endregion

#endregion

namespace Business.RepoManagers
{
    /// <summary>
    /// This is a business class for security module, which handles the operations at business layer.
    /// </summary>
    /// <remarks></remarks>
    public static class SecurityManager
    {
        #region Variables

        #region public Variables

        #endregion

        #region Private Variables

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor to initialize class level variables.
        /// </summary>
        static SecurityManager()
        {
            BALUtils.ClassModule = "SecurityManager";
        }

        #endregion

        #region Properties

        #region public Properties

        public static Int32 DefaultTenantID
        {
            get
            {
                return 1;
            }
        }

        #endregion

        #region Private Properties

        #endregion

        #endregion

        #region Methods

        #region public Methods

        #region Manage User's Account

        /// <summary>
        /// Counts the number of failed attempt while logging in the user account.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <param name="userName">               The Current User's Name.</param>
        /// <param name="maxPasswordAttemptCount">The counter for maximum password attempt.</param>
        public static void FailedPasswordAttemptCount(String userName, Int32 maxPasswordAttemptCount)
        {
            try
            {
                BALUtils.GetSecurityRepoInstance().FailedPasswordAttemptCount(userName, maxPasswordAttemptCount);
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
        /// Count the number of attempt to reset the password.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <param name="userName">The current user's name.</param>
        public static void ResetPasswordAttempCount(String userName)
        {
            try
            {
                BALUtils.GetSecurityRepoInstance().ResetPasswordAttemptCount(userName);
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
        /// Query if 'currentUserId' is current user role exists.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <param name="currentUserId">The Value for current user's Id.</param>
        /// <returns>
        /// true if current user role exists, false if not.
        /// </returns>
        public static Boolean IsCurrentUserRoleExists(Guid currentUserId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().IsCurrentUserRoleExists(currentUserId);
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
        /// Query if 'currentUserId' is current user role exists.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <param name="currentUserId">The Value for current user's Id.</param>
        /// <returns>
        /// true if current user role exists, false if not.
        /// </returns>
        public static Boolean IsCurrentLoggedInUserRoleExists(Guid currentUserId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().IsCurrentLoggedInUserRoleExists(currentUserId);
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
        /// Query if Line of business for current user exists.
        /// </summary>
        /// <param name="userName">The current user name.</param>
        /// <returns>
        /// Default line of business for the current user.
        /// </returns>
        public static lkpSysXBlock GetDefaultLineOfBusinessByUserName(String userName, Int32? tenantId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetDefaultLineOfBusinessByUserName(userName, tenantId);
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
        /// To get Third Party Organization IDs
        /// </summary>
        /// <param name="organizationID"></param>
        /// <returns></returns>
        public static List<Int32> GetThirdPartyOrgIDs(Int32 organizationID)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetThirdPartyOrgIDs(organizationID);
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


        #region To handle: last 5 password can't be used while user tries to change their password.

        /// <summary>
        /// Finds out that is the entered password has been used earlier.
        /// </summary>
        /// <param name="currentUserId">Current user's Id.</param>
        /// <param name="newPassword">The new password entered on the form.</param>
        /// <returns></returns>
        public static Boolean IsPasswordExistsInHistory(Guid currentUserId, String newPassword, Boolean isUserExixtInLocationTenants = false)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().IsPasswordExistsInHistory(currentUserId, newPassword, isUserExixtInLocationTenants);
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
        /// Performs an update operation for Password Details.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <param name="organizationUser">OrganizationUser.</param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        /// <exception cref="Exception">Thrown when an exception error condition occurs.</exception>
        public static Boolean UpdatePasswordDetails(OrganizationUser organizationUser, Int32 orgUsrID, String oldPassword)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().UpdatePasswordDetails(organizationUser, orgUsrID, oldPassword);// && (BALUtils.GetSecurityRepoInstance() as IBaseRepository).UpdateObjectEntity(organizationUser);
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

        /// <summary>
        /// Check if the user belongs to Multi Tenants
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static Boolean IsMultiTenantUser(Guid userId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().IsMultiTenantUser(userId);
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

        #region Manage Cities

        /// <summary>
        /// Retrieves list of all Cities.
        /// </summary>
        /// <returns>A <see cref="City"/> list of data from the underlying data storage.</returns>
        public static IQueryable<City> GetCities()
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetCities();
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

        #region Manage Countries

        /// <summary>
        /// Retrieves list of all Countries.
        /// </summary>
        /// <returns>A <see cref="Country"/> list of data from the underlying data storage.</returns>
        public static List<Country> GetCountries()
        {
            try
            {
                List<Country> allCountries = LookupManager.GetLookUpData<Entity.Country>().ToList();
                List<Country> selectedCountry = allCountries.Where(x => x.Alpha2Code == "US" || x.Alpha2Code == "CA").OrderByDescending(x => x.Alpha2Code).ToList();
                allCountries.RemoveAll(x => x.Alpha2Code == "US" || x.Alpha2Code == "CA");
                selectedCountry.AddRange(allCountries);
                return selectedCountry;
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

        #region Manage States

        /// <summary>
        /// Retrieves list of States with its details.
        /// </summary>
        /// <returns>A <see cref="State"/> list of data from the underlying data storage.</returns>
        public static IQueryable<State> GetStates()
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetStates();
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

        #region Manage Zipcodes

        /// <summary>
        /// Retrieves ZipCode details cod based on zipId.
        /// </summary>
        /// <param name="zipId">current zip code number</param>
        /// <returns>A <see cref="ZipCode"/> data from the underlying data storage.</returns>
        public static ZipCode GetZip(Int32 zipId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetZip(zipId);
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

        public static IQueryable<ZipCode> GetZipcodes()
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetZipcodes();
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

        #region Manage Counties

        public static IQueryable<County> GetCounties()
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetCounties();
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

        #region Manage PermissionTypes

        /// <summary>
        /// Adds the type of the permission.
        /// </summary>
        /// <param name="permissionType">Type of the permission.</param>
        public static PermissionType AddPermissionType(PermissionType permissionType)
        {
            try
            {
                return (BALUtils.GetSecurityRepoInstance() as IBaseRepository).AddObjectEntity(permissionType);
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
        /// Performs an update operation for permission type.
        /// </summary>
        /// <param name="permissionType">Type of the permission.</param>
        public static Boolean UpdatePermisionType(PermissionType permissionType)
        {
            try
            {
                return (BALUtils.GetSecurityRepoInstance() as IBaseRepository).UpdateObjectEntity(permissionType);
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
        /// Retrieves a list of all active permission types.
        /// </summary>
        /// <returns>A <see cref="PermissionType"/> list of data from the underlying data storage.</returns>
        public static IQueryable<PermissionType> GetPermissionTypes()
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetPermissionTypes();
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
        /// Retrieves all active permission types based on permission type id.
        /// </summary>
        /// <param name="permissionTypeId">The permission type id.</param>
        public static PermissionType GetPermissionType(Int32 permissionTypeId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetPermissionType(permissionTypeId);
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
        /// Performs a delete operation for permission type based on permissionType.
        /// </summary>
        /// <param name="permissionType">Type of the permission.</param>
        public static Boolean DeletePermissionType(PermissionType permissionType)
        {
            try
            {
                return (BALUtils.GetSecurityRepoInstance() as IBaseRepository).DeleteObjectEntity(permissionType);
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
        /// Query if 'enteredPermissionTypeName' is permission type exists.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <exception cref="Exception">    Thrown when an exception error condition occurs.</exception>
        /// <param name="enteredPermissionTypeName"> Name of the entered permission type.</param>
        /// <param name="existingPermissionTypeName">(optional) name of the existing permission type.</param>
        /// <returns>
        /// true if permission type exists, false if not.
        /// </returns>
        public static Boolean IsPermissionTypeExists(String enteredPermissionTypeName, String existingPermissionTypeName = null)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().IsPermissionTypeExists(enteredPermissionTypeName, existingPermissionTypeName);
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
        /// Query if 'permission type' is assign to any permission.
        /// </summary>
        /// <param name="permissionTypeId">Id of the current permission type</param>
        /// <returns>
        /// true if permission type assign to any permission, false if not.
        /// </returns>
        public static Boolean IsPermissionTypeAssignToAnyPermission(Int32 permissionTypeId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().IsPermissionTypeAssignToAnyPermission(permissionTypeId);
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

        #region Manage Permission

        /// <summary>
        /// Performs an insert operation for permission.
        /// </summary>
        /// <param name="permission">The permission.</param>
        public static Permission AddPermission(Permission permission)
        {
            try
            {
                return (BALUtils.GetSecurityRepoInstance() as IBaseRepository).AddObjectEntity(permission);
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
        /// Performs an update operation for permission based on permission Id.
        /// </summary>
        /// <param name="permission">The permission.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Boolean UpdatePermision(Permission permission)
        {
            try
            {
                return (BALUtils.GetSecurityRepoInstance() as IBaseRepository).UpdateObjectEntity(permission);
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
        /// Retrieves a list of all active permissions.
        /// </summary>
        /// <returns>A <see cref="Permission"/> list of data from the underlying data storage.</returns>
        public static IQueryable<Permission> GetPermissions()
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetPermissions();
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
        /// Retrieves a list of all permissions based on permission id.
        /// </summary>
        /// <param name="permissionId">The permission id.</param>
        /// <returns>A <see cref="Permission"/> data from the underlying data storage.</returns>
        /// <remarks></remarks>
        public static Permission GetPermission(Int32 permissionId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetPermission(permissionId);
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
        /// Performs a delete operation for permission based on permission id.
        /// </summary>
        /// <param name="permission">The permission.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Boolean DeletePermission(Permission permission)
        {
            try
            {
                return (BALUtils.GetSecurityRepoInstance() as IBaseRepository).DeleteObjectEntity(permission);
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
        /// Retrieves a list of al Permissions based on userId, featureId, blockId.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="featureId">The feature id.</param>
        /// <param name="blockId">The block id.</param>
        /// <returns>A <see cref="Permission"/> data from the underlying data storage.</returns>
        /// <remarks></remarks>
        public static Permission GetMyPermission(String userId, List<Int32> lstFeatureID, Int32 blockId)
        {
            try
            {
                //Permission permission = GetUserGroupPermission(Guid.Parse(userId), featureId, blockId);
                //if (permission.IsNull())
                //{
                //    permission = BALUtils.GetSecurityRepoInstance().GetPermission(userId, featureId, blockId);
                //}
                Permission permission = BALUtils.GetSecurityRepoInstance().GetPermission(userId, lstFeatureID, blockId);
                return permission;
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
        /// Query if 'permissionName' is permission exists.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <exception cref="Exception">    Thrown when an exception error condition occurs.</exception>
        /// <param name="enteredPermissionName"> Name of the permission.</param>
        /// <param name="existingPermissionName">(optional) name of the existing permission.</param>
        /// <returns>
        /// true if permission exists, false if not.
        /// </returns>
        public static Boolean IsPermissionExists(String enteredPermissionName, String existingPermissionName = null)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().IsPermissionExists(enteredPermissionName, existingPermissionName);
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
        /// Query if 'permission' is assign to any feature.
        /// </summary>
        /// <param name="permissionId"> Id of the current permission</param>
        /// <returns>
        /// true if permission assign to any feature, false if not.
        /// </returns>
        public static Boolean IsPermissionAssignToAnyFeature(Int32 permissionId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().IsPermissionAssignToAnyFeature(permissionId);
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

        #region Manage Product Features

        /// <summary>
        /// Performs an insert operation for Product Feature.
        /// </summary>
        /// <param name="productFeature">The product feature.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static ProductFeature AddProductFeature(ProductFeature productFeature)
        {
            try
            {
                return (BALUtils.GetSecurityRepoInstance() as IBaseRepository).AddObjectEntity(productFeature);
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
        /// Performs an update operation for Product Feature.
        /// </summary>
        /// <param name="productFeature">The product feature.</param>
        public static Boolean UpdateProductFeature(ProductFeature productFeature)
        {
            try
            {
                return (BALUtils.GetSecurityRepoInstance() as IBaseRepository).UpdateObjectEntity(productFeature);
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
        /// Retrieves a list of  all active product features.
        /// </summary>
        /// <returns>A <see cref="Permission"/> list of data from the underlying data storage.</returns>
        public static IQueryable<ProductFeature> GetProductFeatures()
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetProductFeatures();
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
        /// Retrieves a list of all product feature based on it's id.
        /// </summary>
        /// <param name="productFeatureTypeId">The product feature type id.</param>
        public static ProductFeature GetProductFeature(Int32 productFeatureTypeId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetProductFeature(productFeatureTypeId);
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
        /// Performs a delete operation for product feature based on it's id.
        /// </summary>
        /// <param name="productFeature">The product feature.</param>
        public static Boolean DeleteProductFeature(ProductFeature productFeature, Int32 modifiedById)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().DeleteProductFeaturesCascade(productFeature, modifiedById);
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
        /// Retrieves a list of all features for product based on productId.
        /// </summary>
        /// <param name="productId">The product id.</param>
        /// <returns>A <see cref="TenantProductFeature"/> list of data from the underlying data storage.</returns>
        public static IEnumerable<TenantProductFeature> GetFeaturesForProduct(Int32 productId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetFeaturesForProduct(productId);
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
        /// Determines whether the specified feature is associated with any line of business.
        /// </summary>
        /// <param name="productFeatureId">The product feature id that is requested by the user.</param>
        /// <returns><c>true</c> if [is feature associated with block] [the specified product feature id]; otherwise, <c>false</c>.</returns>
        public static Boolean IsFeatureAssociatedWithLineOfBusiness(Int32 productFeatureId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().IsFeatureAssociatedWithLineOfBusiness(productFeatureId);
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
        /// Determines whether the specified line of business is associated with any product.
        /// </summary>
        /// <param name="sysXBlockId">The Block's Id that is requested by the user.</param>
        /// <returns><c>true</c> if [is block associated with product] [the specified sys X block id]; otherwise, <c>false</c>.</returns>
        public static Boolean IsLineOfBusinessAssociatedWithProduct(Int32 sysXBlockId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().IsLineOfBusinessAssociatedWithProduct(sysXBlockId);
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
        /// Determines whether the specified user is tied to several other user(s).
        /// </summary>
        /// <param name="organizationUserId">The organization user id of the current user.</param>
        /// <returns><c>true</c> if [is user tied to several other users] [the specified organization user id]; otherwise, <c>false</c>.</returns>
        public static Boolean IsUserTiedToSeveralOtherUsers(Int32 organizationUserId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().IsUserTiedToSeveralOtherUsers(organizationUserId);
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

        #region Manage Dashboard Personalization During Product feature configuration
        public static void SaveGroupPreference(aspnet_Paths path, aspnet_PersonalizationAllUsers groupPreference)
        {
            try
            {
                BALUtils.GetSecurityRepoInstance().SaveGroupPreference(path, groupPreference);
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
        public static void SavePersonalizedPreference(Guid userid, Dictionary<string, string> dashboard, short businessChannelTypeId)
        {
            try
            {
                BALUtils.GetSecurityRepoInstance().SavePersonalizedPreference(userid, dashboard, businessChannelTypeId);
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

        public static aspnet_PersonalizationPerUser GetPersonalizedPreference(Guid userID, short businessChannelTypeId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetPersonalizedPreference(userID, businessChannelTypeId);
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

        public static aspnet_PersonalizationAllUsers GetGroupPreference(Int32 dashBoardUser, Int32 tenantID)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetGroupPreference(dashBoardUser, tenantID);
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


        public static void ClearDashboardStates(Guid userID, short businessChannelTypeId)
        {
            try
            {
                BALUtils.GetSecurityRepoInstance().ClearDashboardStates(userID, businessChannelTypeId);
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

        public static Int32 GetTenantIdFromOrganizationId(Int32 organisationId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetTenantIdFromOrganizationId(organisationId);
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

        public static Dictionary<string, string> GetDashboardMarkup(Guid userID, int tenantID, short businessChannelTypeId)
        {
            Dictionary<string, string> dashboard = new Dictionary<string, string>();
            dashboard.Add(AppConsts.DASHBOARD_MARKUP_KEY, string.Empty);
            dashboard.Add(AppConsts.DASHBOARD_WIDGETSTATE_KEY, string.Empty);

            OrganizationUser orgUser = GetOrganizationUserInfoByUserId(userID.ToString()).FirstOrDefault();
            if (orgUser != null)
            {
                //OrganizationUser orgUserInfo = orgUser as OrganizationUser;
                aspnet_PersonalizationPerUser userPref = GetPersonalizedPreference(userID, businessChannelTypeId);
                if (userPref == null)
                {
                    Int32 dashBoardUser;
                    // Check for the tenant, applicant , client admin
                    if (orgUser.IsApplicant.Value)
                    {
                        if (businessChannelTypeId == AppConsts.ONE)
                        {
                            dashBoardUser = DashBoardUsers.ApplicantDashBoard;
                        }
                        else
                        {
                            dashBoardUser = DashBoardUsers.AMSApplicantDashBoard;
                        }
                    }
                    else if (SecurityManager.DefaultTenantID == tenantID)
                    {
                        if (businessChannelTypeId == AppConsts.ONE)
                        {
                            dashBoardUser = DashBoardUsers.AdminDashBoard;
                        }
                        else
                        {
                            dashBoardUser = DashBoardUsers.AMSAdminDashBoard;
                        }
                    }
                    else
                    {
                        if (businessChannelTypeId == AppConsts.ONE)
                        {
                            dashBoardUser = DashBoardUsers.ClientAdminDashBoard;
                        }
                        else
                        {
                            dashBoardUser = DashBoardUsers.AMSClientAdminDashBoard;
                        }
                    }
                    aspnet_PersonalizationAllUsers allPref = GetGroupPreference(dashBoardUser, tenantID);
                    if (allPref != null)
                    {
                        dashboard[AppConsts.DASHBOARD_MARKUP_KEY] = allPref.DashboardLayout;
                        dashboard[AppConsts.DASHBOARD_WIDGETSTATE_KEY] = allPref.WidgetState;
                        dashboard[AppConsts.DASHBOARD_LAYOUTPATH_ID] = allPref.aspnet_Paths.PathId.ToString();
                    }
                }
                else
                {
                    dashboard[AppConsts.DASHBOARD_MARKUP_KEY] = userPref.DashboardLayout;
                    dashboard[AppConsts.DASHBOARD_WIDGETSTATE_KEY] = userPref.WidgetState;
                    dashboard[AppConsts.DASHBOARD_LAYOUTPATH_ID] = userPref.PathId.ToString();
                }
            }

            return dashboard;
        }
        #endregion

        #region Manage  SysX Configurations

        /// <summary>
        /// Retrieves the application configuration value based on SysXKey.
        /// </summary>
        /// <param name="sysXKey">The sys X key.</param>
        public static String GetSysXConfigValue(String sysXKey)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetSysXConfigValue(sysXKey);
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
        /// Retrieves the Application Configuration based on SysXKey.
        /// </summary>
        /// <param name="sysXKey">The sys X key.</param>
        public static SysXConfig GetSysXConfig(String sysXKey)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetSysXConfig(sysXKey);
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
        /// Retrieves Application configurations.
        /// </summary>
        public static IQueryable<SysXConfig> GetSysXConfigs()
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetSysXConfigs();
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
        /// Performs an update operation for Application Configurations.
        /// </summary>
        /// <param name="sysXConfig">The sysX configuration.</param>
        public static Boolean UpdateSysXConfig(SysXConfig sysXConfig)
        {
            try
            {
                return (BALUtils.GetSecurityRepoInstance() as IBaseRepository).UpdateObjectEntity(sysXConfig);
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
        /// Performs an insert operation for Database Configurations.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        ///<param name="sysXConfig">SystemX Configurations.</param>
        public static SysXConfig SaveDBConfiguration(SysXConfig sysXConfig)
        {
            try
            {
                return (BALUtils.GetSecurityRepoInstance() as IBaseRepository).AddObjectEntity(sysXConfig);
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
        /// Determines whether [is SysXKey exists] [the specified line of business name].
        /// </summary>
        /// <param name="enteredSysXKey">Name of the SysXKey.</param>
        /// <param name="existingSysXKey">Name of the existing SysXKey.</param>
        /// <returns><c>true</c> if [is line of business exists] [the specified line of business name]; otherwise, <c>false</c>.</returns>
        public static Boolean IsSysXKeyExists(String enteredSysXKey, String existingSysXKey = null)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().IsSysXKeyExists(enteredSysXKey, existingSysXKey);
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

        #region Manage Tenants

        /// <summary>
        /// Retrieves a list of all tenant's product feature for each line of business.
        /// </summary>
        /// <param name="tenantProductId">The tenant product id.</param>
        /// <param name="sysXBlockId">The sys X block id.</param>
        /// <returns>A <see cref="TenantProductFeature"/> list of data from the underlying data storage.</returns>
        public static IQueryable<TenantProductFeature> GetTenantProductFeaturesForLineOfBusiness(Int32 tenantProductId, Int32 sysXBlockId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetTenantProductFeaturesForLineOfBusiness(tenantProductId, sysXBlockId);
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
        /// Performs an insert operation for tenant using address, contact, tenant.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="contact">The contact.</param>
        /// <param name="tenant">The tenant.</param>
        public static Tenant AddTenant(Contact contact, Tenant tenant)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().AddTenantInTransaction(contact, tenant);
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

        public static Boolean AddDefaultRole(Tenant tenant)
        {
            try
            {
                List<aspnet_Roles> lstRoles = new List<aspnet_Roles>();
                ISecurityRepository securityRepository = BALUtils.GetSecurityRepoInstance();
                //Add Default Role for Applicant only, Not for shared user
                String applicantRoleCode = defaultRole.Applicant.GetStringValue();
                List<DefaultRole> defaultRoles = securityRepository.GetDefaultRolesByTenantTypeId(tenant.TenantTypeID.Value).Where(x => x.Code == applicantRoleCode).ToList();
                aspnet_Applications application = securityRepository.GetApplication();

                foreach (DefaultRole role in defaultRoles)
                {
                    aspnet_Roles aspnetRole = new aspnet_Roles();
                    aspnetRole.ApplicationId = application.ApplicationId;

                    aspnetRole.LoweredRoleName = role.RoleName.ToLower() + "_" + tenant.TenantID.ToString();
                    aspnetRole.RoleName = role.RoleName + "_" + tenant.TenantID.ToString();
                    aspnetRole.RoleId = Guid.NewGuid();
                    aspnetRole.Description = role.RoleDescription;
                    aspnetRole.RoleDetail = new RoleDetail()
                    {
                        RoleDetailID = aspnetRole.RoleId,
                        DefaultRole = role,
                        Name = role.RoleName + "_" + tenant.TenantID.ToString(),
                        Description = role.RoleDescription,
                        TenantProduct = tenant.TenantProducts.FirstOrDefault(),
                        CreatedByID = SecurityManager.DefaultTenantID,
                        CreatedOn = DateTime.Now,
                        IsActive = true,
                        IsDeleted = false,
                        IsUserGroupLevel = false
                    };
                    lstRoles.Add(aspnetRole);
                }

                securityRepository.AddRoles(lstRoles);

                foreach (aspnet_Roles role in lstRoles)
                    securityRepository.SetDefaultFeatures(role.RoleId);

                return true;

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

        public static Boolean SetDefaultBusinessChannel(Int32 organizationUserId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().SetDefaultBusinessChannel(organizationUserId);
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

        public static Boolean SetSharedUserDefaultBusinessChannel(Int32 organizationUserId)
        {
            try
            {
                Int32 sharedUserSysxBlockId = GetSysXBlockByCode(AppConsts.SHARED_USER_SYSX_BLOCK_CODE).SysXBlockId;
                return BALUtils.GetSecurityRepoInstance().SetSharedUserDefaultBusinessChannel(organizationUserId, sharedUserSysxBlockId);
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
        /// To get SysX Block by code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static lkpSysXBlock GetSysXBlockByCode(String code)
        {
            try
            {
                return LookupManager.GetLookUpData<lkpSysXBlock>().FirstOrDefault(condition => condition.Code == code);
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
        /// Performs an insert operation for tenant using address, contact, tenant.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="contact">The contact.</param>
        /// <param name="tenant">The tenant.</param>
        public static OrganizationUserNamePrefix AddOrganizationUserPrefix(OrganizationUserNamePrefix prefix)
        {
            try
            {
                return (BALUtils.GetSecurityRepoInstance() as IBaseRepository).AddObjectEntity(prefix);
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
        /// Performs an update operation for tenant.
        /// </summary>
        /// <param name="tenant">The tenant.</param>
        public static Boolean UpdateTenant(Tenant tenant)
        {
            try
            {
                return (BALUtils.GetSecurityRepoInstance() as IBaseRepository).UpdateObjectEntity(tenant);
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
        /// Retrieves a list of all active tenants.
        /// </summary>
        /// <param name="SortByName"></param>
        /// <param name="GetExtendedProperties"></param>
        /// <returns>List of data from the underlying data storage.</returns>
        public static List<Tenant> GetTenants(Boolean SortByName, Boolean GetExtendedProperties = false, String TenantTypeCode = "")
        {
            try
            {
                List<Tenant> tenants = null;
                if (!GetExtendedProperties)
                {
                    //tenants = LookupManager.GetLookUpData<Tenant>().Where(condition => condition.IsActive && !condition.IsDeleted).ToList();
                    short businessChannelTypeID = AppConsts.COMPLIO_BUSINESS_CHANNEL_TYPE;
                    if (BALUtils.SessionService.BusinessChannelType.IsNotNull())
                    {
                        businessChannelTypeID = BALUtils.SessionService.BusinessChannelType.BusinessChannelTypeID;
                    }
                    //tenants = BALUtils.GetSecurityRepoInstance().GetTenantsBasedOnBusinessChannelType(businessChannelTypeID);

                    tenants = LookupManager.GetLookUpData<vw_GetTenants>().Where(cond => cond.BCT.Value == businessChannelTypeID)
                                            .Select(col =>
                                            new Tenant
                                            {
                                                TenantID = col.TenantID,
                                                TenantName = col.TenantName,
                                                TenantTypeID = col.TenantTypeID

                                            }).ToList();
                    if (TenantTypeCode != String.Empty)
                    {
                        Int32 tenantTypeId = LookupManager.GetLookUpData<lkpTenantType>().FirstOrDefault(condition => condition.TenantTypeCode == TenantTypeCode).TenantTypeID;
                        tenants = tenants.Where(x => x.TenantTypeID == tenantTypeId).ToList();
                    }
                    if (SortByName)
                    {
                        tenants = tenants.OrderBy(x => x.TenantName).ToList();
                    }
                }
                else
                {
                    //If GetExtendedProperties is true then get all extended properties of Tenants. 
                    tenants = BALUtils.GetSecurityRepoInstance().GetTenants(SortByName).ToList();
                    tenants.ForEach(item =>
                    {
                        item.TenantTypeDescription = (!item.lkpTenantType.IsNull()) ? item.lkpTenantType.TenantTypeDesc : String.Empty;
                        item.TenantPhone = item.Contact.GetContactDetailValue(ContactType.PrimaryPhone.GetStringValue());
                        item.TenantAddress = (!item.AddressHandle.IsNull()) ? item.AddressHandle.Addresses.FirstOrDefault().Address1 : String.Empty;
                        item.TenantZipCode = (!item.AddressHandle.IsNull()) ? item.AddressHandle.Addresses.FirstOrDefault().ZipCode.ZipCode1 : String.Empty;
                        item.TenantCity = (!item.AddressHandle.IsNull()) ? item.AddressHandle.Addresses.FirstOrDefault().ZipCode.City.CityName : String.Empty;
                        item.TenantState = (!item.AddressHandle.IsNull()) ? item.AddressHandle.Addresses.FirstOrDefault().ZipCode.City.State.StateName : String.Empty;
                    });
                }
                return tenants;
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
        /// Retrieves a list of all active tenants of a user. 
        /// </summary>
        /// <returns>A <see cref="Tenant"/> list of data from the underlying data storage.</returns>
        public static IQueryable<Tenant> GetUserTenants(String currentOrgUserId)
        {
            try
            {
                IQueryable<Tenant> tenants = BALUtils.GetSecurityRepoInstance().GetUserTenants(currentOrgUserId);
                tenants.ForEach(item =>
                {
                    item.TenantTypeDescription = (!item.lkpTenantType.IsNull()) ? item.lkpTenantType.TenantTypeDesc : String.Empty;
                });

                return tenants;
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
        /// Retrieves a list of all active tenants of a client admin. 
        /// </summary>
        /// <returns>A <see cref="Tenant"/> list of data from the underlying data storage.</returns>
        public static IQueryable<Tenant> GetClientAdminTenants(String currentOrgUserId, Boolean isAtLoginTime = false)
        {
            try
            {
                IQueryable<Tenant> tenants = BALUtils.GetSecurityRepoInstance().GetClientAdminTenants(currentOrgUserId, isAtLoginTime);
                tenants.ForEach(item =>
                {
                    item.TenantTypeDescription = (!item.lkpTenantType.IsNull()) ? item.lkpTenantType.TenantTypeDesc : String.Empty;
                });

                return tenants;
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
        /// Retrieves a list of all active Clients.
        /// </summary>
        /// <returns>A <see cref="Tenant"/> list of data from the underlying data storage.</returns>
        public static List<Tenant> GetClientsForBatch()
        {
            try
            {
                Boolean SortByName = false;
                Boolean GetExtendedProperty = false;
                string ClientCode = TenantType.Institution.GetStringValue();
                return GetTenants(SortByName, GetExtendedProperty).Where(x => x.lkpTenantType.TenantTypeCode == ClientCode).ToList();
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
        /// Checks if the Tenant exists in system dataBALUtils.
        /// </summary>
        /// <param name="enteredTenantName">Name of the entered tenant.</param>
        /// <param name="existingTenantName">Name of the existing tenant.</param>
        /// <returns><c>true</c> if [is tenant exists] [the specified entered tenant name]; otherwise, <c>false</c>.</returns>
        public static Boolean IsTenantExists(String enteredTenantName, String existingTenantName = null)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().IsTenantExists(enteredTenantName, existingTenantName);
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
        /// Retrieves tenant based on tenantId.
        /// </summary>
        /// <param name="tenantId">The tenant id.</param>
        public static Tenant GetTenant(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetTenant(tenantId);
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
        /// Retrieves tenant product based on tenantId.
        /// </summary>
        /// <param name="tenantId">The tenant id.</param>
        public static Int32? GetTenantProductId(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetTenantProductId(tenantId);
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
        /// Performs a delete operation for tenant including it's departments.
        /// </summary>
        /// <param name="tenant">The tenant.</param>
        public static Boolean DeleteTenantWithAllDependent(Tenant tenant)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().DeleteTenantWithAllDependent(tenant);
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
        /// Return ZipCode by zipCodeNumber.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <param name="zipCodeNumber">.</param>
        /// <returns>
        /// The zip codes by zip code number.
        /// </returns>
        public static IQueryable<ZipCode> GetZipCodesByZipCodeNumber(String zipCodeNumber)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetZipCodesByZipCodeNumber(zipCodeNumber);
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
        /// Return Tenant Types.
        /// </summary>
        /// <returns>
        /// The Tenant Types.
        /// </returns>
        public static IQueryable<lkpTenantType> GetTenantTypes()
        {
            try
            {
                return LookupManager.GetLookUpData<Entity.lkpTenantType>().AsQueryable();
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

        public static Boolean IsTenantThirdPartyType(Int32 tenanatId, String tenantTypeCode)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().IsTenantThirdPartyType(tenanatId, tenantTypeCode);
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

        #region Manage Sub Tenant

        /// <summary>
        /// retrieve a collection of child suppliers.
        /// </summary>
        /// <returns></returns>
        public static List<Tenant> GetChildTenants(Int32 tenantId, Boolean isParent)
        {
            try
            {
                List<Tenant> tenants = BALUtils.GetSecurityRepoInstance().GetChildTenants(tenantId, isParent);
                tenants.ForEach(item =>
                {
                    item.TenantAddress = (!item.AddressHandle.IsNull()) ? item.AddressHandle.Addresses.FirstOrDefault().Address1 : String.Empty;
                    item.TenantZipCode = (!item.AddressHandle.IsNull()) ? item.AddressHandle.Addresses.FirstOrDefault().ZipCode.ZipCode1 : String.Empty;
                    item.TenantCity = (!item.AddressHandle.IsNull()) ? item.AddressHandle.Addresses.FirstOrDefault().ZipCode.City.CityName : String.Empty;
                    item.TenantState = (!item.AddressHandle.IsNull()) ? item.AddressHandle.Addresses.FirstOrDefault().ZipCode.City.State.StateName : String.Empty;
                });

                return tenants;

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
        /// retrieve a list of all tenants except child tenants.
        /// </summary>
        /// <returns></returns>
        public static List<Tenant> GetAllTenantsForMapping(List<Int32> childTenantId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetAllTenantsForMapping(childTenantId);
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
        /// retrieve supplier relation byrelated supplier id and supplier id.
        /// </summary>
        /// <param name="supplierId">supplierId</param>
        /// <param name="relatedSupplierId">relatedSupplierId</param>
        /// <returns></returns>
        public static ClientRelation GetTenantRelationByRelatedTenantIdAndTenantId(Int32 tenantId, Int32 relatedTenantId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetTenantRelationByRelatedTenantIdAndTenantId(tenantId, relatedTenantId);
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

        public static List<ClientRelation> GetClientRelationBasedonRelatedID(Int32 relatedTenantId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetClientRelationBasedonRelatedID(relatedTenantId);
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
        /// delete client relation.
        /// </summary>
        /// <param name="clientRelation">supplierRelation</param>
        /// <returns></returns>
        public static Boolean DeleteSubTenant(ClientRelation clientRelation)
        {
            try
            {
                return (BALUtils.GetSecurityRepoInstance() as IBaseRepository).UpdateObjectEntity(clientRelation);
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
        /// Retrieve the list of Client Relation.
        /// </summary>
        /// <param name="isParent">Is Parent.</param>
        /// <returns>Supplier Relation entity.</returns>
        public static IQueryable<ClientRelation> GetClientChildRelation(Boolean isParent, Int32 currentTenantId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetClientRelation(isParent, currentTenantId);
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
        /// add supplier relation.
        /// </summary>
        /// <param name="clientRelation">supplierRelation</param>
        /// <returns></returns>
        public static void AddSubTenant(Int32 tenantId, List<Int32> relatedTenantIds, Int32 currentUserId)
        {
            try
            {
                List<ClientRelation> clientChildRelationList = new List<ClientRelation>();
                foreach (Int32 relatedTenantId in relatedTenantIds)
                {
                    ClientRelation clientChildRelation = new ClientRelation();
                    clientChildRelation.TenantID = tenantId;
                    clientChildRelation.RelatedTenantID = relatedTenantId;
                    clientChildRelation.IsActive = true;
                    clientChildRelation.IsDeleted = false;
                    clientChildRelation.IsParent = false;
                    clientChildRelation.CreatedOn = DateTime.Now;
                    clientChildRelation.CreatedByID = currentUserId;
                    // (BALUtils.GetSecurityRepoInstance() as IBaseRepository).AddObjectEntity(clientChildRelation);

                    clientChildRelationList.Add(clientChildRelation);
                }
                if (clientChildRelationList.Count > 0)
                {
                    if (BALUtils.GetSecurityRepoInstance().AddClientRelation(clientChildRelationList))
                    {
                        ComplianceDataManager.checkTenantExistIfNotcreate(relatedTenantIds, tenantId);
                        ComplianceDataManager.CopyTenantToClient(clientChildRelationList, tenantId);
                    }
                }
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

        #region Manage Products

        /// <summary>
        /// Retrieves a list of all active products.
        /// </summary>
        /// <returns>A <see cref="TenantProduct"/> list of data from the underlying data storage.</returns>
        public static IQueryable<TenantProduct> GetProducts()
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetProducts();
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
        /// Checks if the LineOfBusiness exists in system dataBALUtils.
        /// </summary>
        /// <param name="enteredTenantName">Name of the entered tenant.</param>
        /// <param name="existingTenantName">Name of the existing tenant.</param>
        /// <returns><c>true</c> if [is product exists] [the specified entered tenant name]; otherwise, <c>false</c>.</returns>
        public static Boolean IsProductExists(String enteredTenantName, String existingTenantName = null)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().IsProductExists(enteredTenantName, existingTenantName);
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
        /// Retrieves product details based on product id.
        /// </summary>
        /// <param name="tenantProductId">The tenant product id.</param>
        public static TenantProduct GetTenantProduct(Int32 tenantProductId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetProduct(tenantProductId);
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
        /// Retrieves the role's details based on product Id.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <param name="tenantProductId">.</param>
        /// <returns>
        /// An enumerator that allows foreach to be used to process get role details by product
        /// identifier in this collection.
        /// </returns>
        public static IQueryable<RoleDetail> GetRoleDetailsByProductId(Int32 tenantProductId)
        {
            try
            {
                //return BALUtils.GetSecurityRepoInstance().GetRoleDetailsByProductId(tenantProductId);
                IQueryable<RoleDetail> roleDetail = BALUtils.GetSecurityRepoInstance().GetRoleDetailsByProductId(tenantProductId);
                roleDetail.ForEach(item =>
                {
                    item.RoleName = (!item.Name.IsNull()) ? item.Name.Split(new char[] { '_' }).FirstOrDefault() : String.Empty;
                    item.CreatedByUserName = (!item.OrganizationUser.IsNull()) ? item.OrganizationUser.LastName + SysXUtils.GetMessage(ResourceConst.SECURITY_COMMA) + " " + item.OrganizationUser.FirstName : String.Empty;
                });

                return roleDetail;
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

        #region Manage Organizations

        /// <summary>
        /// Performs an insert operation for organization.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <param name="organization">The organization.</param>
        public static Organization AddOrganization(Organization organization)
        {
            try
            {
                return (BALUtils.GetSecurityRepoInstance() as IBaseRepository).AddObjectEntity(organization);
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
        /// Checks if the Organization exists in system dataBALUtils.
        /// </summary>
        /// <param name="enteredOrganizationName">Name of the entered organization.</param>
        /// <param name="existingOrganizationName">Name of the existing organization.</param>
        /// <returns><c>true</c> if [is organization exists] [the specified entered organization name]; otherwise, <c>false</c>.</returns>
        public static Boolean IsOrganizationExists(String enteredOrganizationName, String existingOrganizationName = null)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().IsOrganizationExists(enteredOrganizationName, existingOrganizationName);
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
        /// Performs an update operation for organization based on organization id.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <param name="organization">The organization.</param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        public static Boolean UpdateOrganization(Organization organization)
        {
            try
            {
                return (BALUtils.GetSecurityRepoInstance() as IBaseRepository).UpdateObjectEntity(organization);
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
        /// Retrieves a list of all active organizations.
        /// </summary>
        /// <param name="isAdmin">if set to <c>true</c> [is admin].</param>
        /// <param name="productId">The product id.</param>
        /// <returns>A <see cref="Organization"/> list of data from the underlying data storage.</returns>
        public static IQueryable<Organization> GetOrganizations(Boolean isAdmin, Int32 productId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetOrganizations(isAdmin, productId);
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
        /// 
        /// </summary>
        /// <returns></returns>
        public static IQueryable<Organization> GetOrganizations()
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetOrganizations();
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
        /// 
        /// </summary>
        /// <returns></returns>
        public static IQueryable<lkpGender> GetGender()
        {
            try
            {
                return LookupManager.GetLookUpData<Entity.lkpGender>().AsQueryable();
                //return BALUtils.GetSecurityRepoInstance().GetGender();
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
        /// 
        /// </summary>
        /// <returns></returns>
        public static IQueryable<lkpSecurityQuestion> GetSecurityQuestion()
        {
            try
            {
                return LookupManager.GetLookUpData<Entity.lkpSecurityQuestion>().Where(SecurityQuestions => SecurityQuestions.IsDeleted == false).AsQueryable();
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
        /// 
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public static IQueryable<OrganizationLocation> GetOrganizationLocations(Int32 organizationId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetOrganizationLocations(organizationId);
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
        /// Retrieves all organizations for each product.
        /// </summary>
        /// <param name="isAdmin">if set to <c>true</c> [is admin].</param>
        /// <param name="productId">The product id.</param>
        /// <returns>A <see cref="Organization"/> list of data from the underlying data storage.</returns>
        public static IQueryable<Organization> GetOrganizationsForProduct(Boolean isAdmin, Int32 productId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetOrganizationsForProduct(isAdmin, productId);
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
        /// Retrieves an organization details based on organizationId.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <param name="organizationId">The organization id.</param>
        /// <returns>
        /// The organization.
        /// </returns>
        public static Organization GetOrganization(Int32 organizationId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetOrganization(organizationId);
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
        public static IQueryable<Organization> GetDepartments(Int32 organizationId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetDepartments(organizationId);
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
        //public static Entity.ClientEntity.OrganizationUserDepartment GetUserDept(Int32 organizationId)
        //{
        //    try
        //    {
        //        return BALUtils.GetSecurityRepoInstance().GetUserDept(organizationId);
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
        //public static List<OrganizationUserProgram> GetProgmFromOrgUserPrograms(Int32 organizationId)
        //{
        //    try
        //    {
        //        return BALUtils.GetSecurityRepoInstance().GetProgramsFromOrgUserProgrm(organizationId);
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
        //public static List<DeptProgramMapping> GetDeptFromDeptPrgMaping(Int32 oganizationID)
        //{
        //    try
        //    {
        //        return BALUtils.GetSecurityRepoInstance().GetDeptFromDeptPrgMaping(oganizationID);

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
        /// <summary>
        /// Retrieves an organization details based on tenantId.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <param name="tenantId">The tenant id.</param>
        /// <returns>
        /// The organization for tenant.
        /// </returns>
        public static Organization GetOrganizationForTenant(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetOrganizationForTenant(tenantId);
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
        /// Retrieves all users based on product id.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <param name="assignToProductId">The product id.</param>
        /// <returns>
        /// The users for product.
        /// </returns>
        public static List<OrganizationUser> GetUsersByProductId(Int32 assignToProductId)
        {
            try
            {
                //return BALUtils.GetSecurityRepoInstance().GetUsersByProductId(assignToProductId);
                List<OrganizationUser> organizationUsers = BALUtils.GetSecurityRepoInstance().GetUsersByProductId(assignToProductId);

                organizationUsers.ForEach(item =>
                {
                    OrganizationUser createdByUserDetails = GetOrganizationUser(item.CreatedByID);
                    item.IsLockedOut = (!item.aspnet_Users.IsNull() && !item.aspnet_Users.aspnet_Membership.IsNull()) ? item.aspnet_Users.aspnet_Membership.IsLockedOut : false;
                    item.LastActivityDate = (!item.aspnet_Users.IsNull()) ? item.aspnet_Users.LastActivityDate.ToString() : String.Empty;

                    if (!createdByUserDetails.IsNull())
                    {
                        item.CreatedByUserName = createdByUserDetails.LastName + SysXUtils.GetMessage(ResourceConst.SECURITY_COMMA) + " " + createdByUserDetails.FirstName;
                    }
                    else
                    {
                        item.CreatedByUserName = SysXUtils.GetMessage(ResourceConst.SPACE);
                    }
                });

                return organizationUsers;
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

        public static Int32 GetOrganizationIdByProductId(Int32 assignToProductId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetOrganizationIdByProductId(assignToProductId);
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
        /// Performs a delete operation for organization.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <param name="organization">The organization.</param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        public static Boolean DeleteOrganization(Organization organization)
        {
            try
            {
                return (BALUtils.GetSecurityRepoInstance() as IBaseRepository).DeleteObjectEntity(organization);
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
        /// Retrieves all Organizations based on organizationId.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <param name="organizationId">The organization id.</param>
        /// <returns>
        /// all type organization.
        /// </returns>
        public static Organization GetAllTypeOrganization(Int32 organizationId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetAllTypeOrganization(organizationId);
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
        /// Retrieves the Organization Details based on Tenant's Id.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <param name="tenantId">The value for tenant's id.</param>
        /// <returns>
        /// The organizations by tenant identifier.
        /// </returns>
        public static IQueryable<Organization> GetOrganizationsByTenantId(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetOrganizationsByTenantId(tenantId);
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
        /// Enumerates get organizations by current user identifier in this collection.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <exception cref="Exception">    Thrown when an exception error condition occurs.</exception>
        /// <param name="isAdmin">      The Value for isAdmin</param>
        /// <param name="productId">    The Value for product's Id.</param>
        /// <param name="currentUserId">The Value for current user's Id.</param>
        /// <returns>
        /// An enumerator that allows foreach to be used to process get organizations by current user
        /// identifier in this collection.
        /// </returns>
        public static IQueryable<Organization> GetOrganizationsByCurrentUserId(Boolean isAdmin, Int32 productId, Int32 currentUserId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetOrganizationsByCurrentUserId(isAdmin, productId, currentUserId);
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
        /// Check organization by current user identifier.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <exception cref="Exception">    Thrown when an exception error condition occurs.</exception>
        /// <param name="currentUserId">The Value for current user's Id.</param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        public static Boolean CheckOrganizationByCurrentUserId(Int32 currentUserId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().CheckOrganizationByCurrentUserId(currentUserId);
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
        /// Check organization by current user identifier.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <exception cref="Exception">    Thrown when an exception error condition occurs.</exception>
        /// <param name="isAdmin">Checks if the logged in user is admin.</param>
        /// <param name="productId">Logged in user's productId</param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        public static Boolean IsDepartmentOrOrganizationExistsForProduct(Boolean isAdmin, Int32? productId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().IsDepartmentOrOrganizationExistsForProduct(isAdmin, productId);
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
        /// Checks whether the currently selected organization is of the supplier type tenant.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <exception cref="Exception">    Thrown when an exception error condition occurs.</exception>
        /// <param name="selectedOrganizationId">currently selected organization's id.</param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        public static Boolean IsOrganizationOfSupplier(Int32 selectedOrganizationId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().IsOrganizationOfSupplier(selectedOrganizationId);
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
        /// Query if department is assign to any user.
        /// </summary>
        /// <param name="departmentId"> Id of the current department </param>
        /// <returns>
        /// true if department assign to any user, false if not.
        /// </returns>
        public static Boolean IsUserExistsForDepartment(Int32 departmentId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().IsUserExistsForDepartment(departmentId);
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
        /// Query if department is assign to any user.
        /// </summary>
        /// <param name="departmentId"> Id of the current department </param>
        /// <returns>
        /// true if department assign to any user, false if not.
        /// </returns>
        public static List<OrganizationUserNamePrefix> getAllUserNamePrefix()
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().getAllUserNamePrefix();
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

        public static bool IsUserExist(String PrefixName)
        {
            try
            {
                OrganizationUserNamePrefix prefixname = BALUtils.GetSecurityRepoInstance().getAllUserNamePrefix().Where(cond => cond.UserNamePrefix == PrefixName).FirstOrDefault();
                if (!prefixname.IsNull())
                {
                    return true;
                }
                return false;
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
        /// Retrieves a list of all organization username prefix.
        /// </summary>        
        /// <param name="organizationId">The organization id.</param>
        /// <returns>A <see cref="Organization"/> list of data from the underlying data storage.</returns>
        public static IQueryable<OrganizationUserNamePrefix> GetOrganizationUserNamePrefix(Int32 organizationId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetOrganizationUserNamePrefix(organizationId);
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


        #region Manage Programs

        //public static DeptProgramMapping AddProgram(DeptProgramMapping program)
        //{
        //    try
        //    {
        //        return (BALUtils.GetSecurityRepoInstance() as IBaseRepository).AddObjectEntity(program);
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


        public static Boolean IsProgramExists(String enteredProgramnName, String existingProgramName = null)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().IsProgramExists(enteredProgramnName, existingProgramName);
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


        //public static Boolean UpdateProgram(AdminProgramStudy program)
        //{
        //    try
        //    {
        //        return (BALUtils.GetSecurityRepoInstance() as IBaseRepository).UpdateObjectEntity(program);
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


        //public static IQueryable<AdminProgramStudy> GetPrograms(Boolean isAdmin, Int32 productId, int currentUserId)
        //{
        //    try
        //    {
        //        return BALUtils.GetSecurityRepoInstance().GetProgramsByCurrentUserId(isAdmin, productId, currentUserId);
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


        //public static AdminProgramStudy GetProgram(Int32 ProgramId)
        //{
        //    try
        //    {
        //        return BALUtils.GetSecurityRepoInstance().GetProgram(ProgramId);
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

        //public static IQueryable<AdminProgramStudy> GetAllPrograms(Int32 OrganizationID)
        //{
        //    try
        //    {
        //        return BALUtils.GetSecurityRepoInstance().GetProgramsByOrganizationId(OrganizationID);
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

        ///// <summary>
        ///// Gets List of programs for the given tenant ID.
        ///// </summary>
        ///// <param name="clientId">Tenant ID</param>
        ///// <returns>List of active programs</returns>
        //public static List<AdminProgramStudy> GetAllProgramsForTenantID(Int32 clientId)
        //{
        //    try
        //    {
        //        return BALUtils.GetSecurityRepoInstance().GetAllProgramsForTenantID(clientId);
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

        /// <summary>
        /// Gets the organization for the given client ID.
        /// </summary>
        /// <param name="clientId">Tenant ID</param>
        /// <returns>Organization Entity Object</returns>
        public static Entity.Organization GetOrganizationForTenantID(Int32 clientId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetOrganizationForTenantID(clientId);
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

        //public static IQueryable<OrganizationUserProgram> GetAllPrograms(Int32 OrganizationUserID)


        //public static Boolean DeleteProgram(AdminProgramStudy program)
        //{
        //    try
        //    {
        //        return (BALUtils.GetSecurityRepoInstance() as IBaseRepository).DeleteObjectEntity(program);
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

        //public static IQueryable<DeptProgramMapping> GetOrganizationProgramList(Int32 OrganizationID)
        //{
        //    try
        //    {
        //        return BALUtils.GetSecurityRepoInstance().GetOrganizationProgramList(OrganizationID);
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

        //public static DeptProgramMapping GetOrganizationProgram(Int32 depProgramId)
        //{
        //    try
        //    {
        //        return BALUtils.GetSecurityRepoInstance().GetOrganizationProgram(depProgramId);
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

        public static Boolean UpdateProgramObject()
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().UpdateProgramObject();
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

        #region Manage Grades


        public static lkpGradeLevel AddGrade(lkpGradeLevel grade)
        {
            try
            {
                return (BALUtils.GetSecurityRepoInstance() as IBaseRepository).AddObjectEntity(grade);
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


        public static Boolean IsGradeExists(String enteredgradeName, Int32 organizationId, String existinggradeName = null)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().IsGradeExists(enteredgradeName, organizationId, existinggradeName);
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


        public static Boolean UpdateGrade(lkpGradeLevel grade)
        {
            try
            {
                return (BALUtils.GetSecurityRepoInstance() as IBaseRepository).UpdateObjectEntity(grade);
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


        public static IQueryable<lkpGradeLevel> GetGrades()
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetAllGrades();
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


        public static List<lkpGradeLevelGroup> GetGradeGroups()
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetAllGradeLevelGroups();
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



        public static lkpGradeLevel GetGrade(Int32 GradeId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetGrade(GradeId);
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


        public static Boolean DeleteGrade(lkpGradeLevel Grade)
        {
            try
            {
                return (BALUtils.GetSecurityRepoInstance() as IBaseRepository).DeleteObjectEntity(Grade);
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

        public static IQueryable<lkpGradeLevel> GetGradeListByOrganizationId(Int32 organizationId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetGradeListByOrganizationId(organizationId);
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

        #region Manage Mapping Methods

        /// <summary>
        /// Mapping between line of business and features.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <exception cref="Exception">    Thrown when an exception error condition occurs.</exception>
        /// <param name="blockFeature">              The block feature.</param>
        /// <param name="blockFeatureIdsToBeAdded">  The block feature Ids to be added.</param>
        /// <param name="blockFeatureIdsToBeRemoved">The block feature Ids to be removed.</param>
        /// <param name="createdById"></param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        public static Boolean LineOfBusinessFeatureMapping(lkpSysXBlock blockFeature, List<Int32> blockFeatureIdsToBeAdded, List<Int32> blockFeatureIdsToBeRemoved, Int32 createdById)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().LineOfBusinessFeatureMapping(blockFeature, blockFeatureIdsToBeAdded, blockFeatureIdsToBeRemoved, createdById);
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
        /// Check if the feature is assigned to any role based on featureId.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <exception cref="Exception">    Thrown when an exception error condition occurs.</exception>
        /// <param name="featureId">The feature id.</param>
        /// <param name="productId">The product id.</param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        public static Boolean CheckFeatureAssignToRole(Int32 featureId, Int32 productId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().CheckFeatureAssignToRole(featureId, productId);
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
        /// Retrieve features for a block based on blockId.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <exception cref="Exception">    Thrown when an exception error condition occurs.</exception>
        /// <param name="blockId">The block id.</param>
        /// <returns>
        /// An enumerator that allows foreach to be used to process get features for line of business in
        /// this collection.
        /// </returns>
        public static IQueryable<SysXBlocksFeature> GetFeaturesForLineOfBusiness(Int32 blockId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetFeaturesForLineOfBusiness(blockId);
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
        /// Determines whether the current line of business exists in Product Feature.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <exception cref="Exception">    Thrown when an exception error condition occurs.</exception>
        /// <param name="sysXBlockFeatureId">The sys X block feature id.</param>
        /// <returns>
        /// <c>true</c> if [is sys X block exist in product feature] [the specified sys X block feature
        /// id]; otherwise, <c>false</c>.
        /// </returns>
        public static Boolean IsSysXLineOfBusinessExistInProductFeature(Int32 sysXBlockFeatureId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().IsSysXLineOfBusinessExistInProductFeature(sysXBlockFeatureId);
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
        /// This method returns all the odd entries from RolePermissionProductFeature table , which is no more associated with the product's feature.
        /// </summary>
        /// <param name="featureList">List of all the new relations made through Map Product Feature page.</param>
        /// <returns></returns>
        public static List<RolePermissionProductFeature> GetFeatureWithPermissionUsedByRole(IEnumerable<BlockFeaturePermissionMapper> featureList)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetFeatureWithPermissionUsedByRole(featureList);
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
        /// Mapping between product and features.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <exception cref="Exception">    Thrown when an exception error condition occurs.</exception>
        /// <param name="product">            The product.</param>
        /// <param name="features">           The features.</param>
        /// <param name="updatedSysXBlockIds">The updated sysX block Ids.</param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        public static Boolean ProductFeatureMapping(TenantProduct product, List<BlockFeaturePermissionMapper> features, List<Int32> updatedSysXBlockIds, Int32 currentLoggedInUserID)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().ProductFeatureMapping(product, features, updatedSysXBlockIds, currentLoggedInUserID);
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
        /// Enumerates get features for line of business and allow delegation in this collection.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <exception cref="Exception">    Thrown when an exception error condition occurs.</exception>
        /// <param name="blockId">The value for block's Id.</param>
        /// <returns>
        /// An enumerator that allows foreach to be used to process get features for line of business and
        /// allow delegation in this collection.
        /// </returns>
        public static IEnumerable<SysXBlocksFeature> GetFeaturesForLineOfBusinessAndAllowDelegation(Int32 blockId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetFeaturesForLineOfBusinessAndAllowDelegation(blockId);
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
        /// It handles the mapping of user with roles, and drops a mail to the user with their credentials, based on conditions.
        /// </summary>
        /// <param name="selectedUser">The selected user's details.</param>
        /// <param name="newMappedRoles">The collection of all the newly assigned roles.</param>
        /// <param name="allAssignedRoleName">The collection of all the newly assigned role name.</param>
        /// <param name="defaultPassword">The default password to be send the user.</param>
        public static Boolean SaveMappingOfRolesWithSelectedUser(OrganizationUser selectedUser, List<String> newMappedRoles, String[] allAssignedRoleName, String defaultPassword, Boolean deleteExistingRoles = true)
        {
            try
            {
                //UAT-985: WB: Simplification of Admin Account creation.
                //Boolean userDetailsToBeSend = false;
                Boolean userDetailsToBeSend = true;
                String[] existingMappedRolesCollection = GetAspnetUser(Convert.ToString(selectedUser.UserID)).aspnet_Roles.Select(condition => condition.RoleName.Split(new char[] { '_' }).FirstOrDefault()).ToArray();
                String[] newMappedRolesCollection = allAssignedRoleName;
                Boolean flag = newMappedRolesCollection.Except(existingMappedRolesCollection).Any() || existingMappedRolesCollection.Except(newMappedRolesCollection).Any();
                //UAT-985: WB: Simplification of Admin Account creation.
                //if (flag && Convert.ToString(selectedUser.aspnet_Users.LastActivityDate).Equals(Convert.ToString(DateTime.MaxValue)) && selectedUser.IsNewPassword && GetAspnetUser(Convert.ToString(selectedUser.UserID)).aspnet_Roles.Count.Equals(AppConsts.NONE))
                //{
                //    userDetailsToBeSend = true;
                //}

                if (flag && Convert.ToString(selectedUser.aspnet_Users.LastActivityDate).Equals(Convert.ToString(DateTime.MaxValue))
                         && GetAspnetUser(Convert.ToString(selectedUser.UserID)).aspnet_Roles.Count.Equals(AppConsts.NONE))
                {
                    userDetailsToBeSend = false;
                }

                Boolean status = UserRoleMapping(selectedUser.UserID.ToString(), newMappedRoles, deleteExistingRoles);

                if (flag)
                {
                    //UAT-985: WB: Simplification of Admin Account creation.
                    //if (userDetailsToBeSend)
                    //{
                    //    // Sends the mail to a user to with new activated account details.
                    //    SendEmailForNewlyCreatedUserAccount(selectedUser, String.Join(", ", allAssignedRoleName).Split(new char[] { '_' }).FirstOrDefault(), defaultPassword);
                    //}
                    //else
                    //{
                    //    if (selectedUser.IsApplicant != true)
                    //    {
                    //        // Sends the mail to a user for the change in role(s) assigned.
                    //        SendEmailWhileChangeInAssignedRoles(selectedUser, String.Join(", ", allAssignedRoleName).Split(new char[] { '_' }).FirstOrDefault());
                    //    }
                    //}

                    if (userDetailsToBeSend && selectedUser.IsApplicant != true)
                    {
                        // Sends the mail to a user for the change in role(s) assigned.
                        SendEmailWhileChangeInAssignedRoles(selectedUser, String.Join(", ", allAssignedRoleName).Split(new char[] { '_' }).FirstOrDefault());
                    }
                }

                return status;
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

        #region UAT-3228

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lstFeatureID"></param>
        /// <param name="CurrentUserId"></param>
        /// <param name="selectedUser"></param>
        /// <param name="newMappedRoles"></param>
        /// <returns></returns>
        public static Boolean InsertDefaultColumnConfiguration(Int32 CurrentUserId, OrganizationUser selectedUser = null, List<String> newMappedRoles = null)
        {
            try
            {
                String userId = null;
                if (!selectedUser.IsNullOrEmpty())
                {
                    userId = Convert.ToString(selectedUser.UserID);
                }

                if (!newMappedRoles.IsNullOrEmpty())
                {
                    foreach (String role in newMappedRoles)
                    {
                        return BALUtils.GetSecurityRepoInstance().InsertDefaultColumnConfiguration(CurrentUserId, userId, role);
                    }
                }

                return true;
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
        /// 
        /// </summary>
        /// <param name="CopyFromUserId"></param>
        /// <param name="CopyToOrganizationUserID"></param>
        /// <param name="CurrentLoggedInUserId"></param>
        /// <returns></returns>
        public static Boolean CopyDefaultColumnConfiguration(Int32 tenantId, Guid CopyFromUserId, Int32 CopyToOrganizationUserID, Int32 CurrentLoggedInUserId)
        {
            return BALUtils.GetSecurityRepoInstance().CopyDefaultColumnConfiguration(tenantId, CopyFromUserId, CopyToOrganizationUserID, CurrentLoggedInUserId);
        }

        #endregion

        /// <summary>
        /// Save Mapping Of Roles for Shared User
        /// </summary>
        /// <param name="selectedUser"></param>
        /// <param name="newMappedRoles"></param>
        /// <returns></returns>
        public static Boolean SaveMappingOfRolesForSharedUser(OrganizationUser selectedUser, List<String> newMappedRoles)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().SharedUserRoleMapping(selectedUser.UserID.ToString(), newMappedRoles);
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

        #region Manage Line of Businesses

        /// <summary>
        /// Performs an insert operation for Line of Businesses.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <param name="block">The Line of Businesses.</param>
        public static lkpSysXBlock AddLineOfBusiness(lkpSysXBlock block)
        {
            try
            {
                return (BALUtils.GetSecurityRepoInstance() as IBaseRepository).AddObjectEntity(block);
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
        /// Retrieve User's line of businesses ids based on userId.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <param name="userId">The user id.</param>
        /// <returns>
        /// The user line of businesses identifiers.
        /// </returns>
        public static IQueryable<Int32> GetUserLineOfBusinessesIds(String userId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetUserLineOfBusinessesIds(userId);
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
        /// Performs an update operation for line of business.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <param name="block">The block.</param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        public static Boolean UpdateLineOfBusiness(lkpSysXBlock block)
        {
            try
            {
                return (BALUtils.GetSecurityRepoInstance() as IBaseRepository).UpdateObjectEntity(block);
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
        /// Performs a delete operation for line of business.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <param name="block">The line of business.</param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        public static Boolean DeleteLineOfBusiness(lkpSysXBlock block)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().DeleteLineOfBusiness(block);
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
        /// Retrieves line of business based on blockId.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <param name="blockId">The block id.</param>
        /// <returns>
        /// The line of business.
        /// </returns>
        public static lkpSysXBlock GetLineOfBusiness(Int32 blockId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetLineOfBusiness(blockId);
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
        /// Retrieves a list of all active blocks.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <returns>
        /// A <see cref="SysXBlock"/> list of data from the underlying data storage.
        /// </returns>
        public static IQueryable<lkpSysXBlock> GetLineOfBusinesses()
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetLineOfBusinesses();
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
        /// Retrieves a list of all active line of businesses.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <param name="userId">The user id.</param>
        /// <returns>
        /// A <see cref="vw_UserAssignedBlocks"/> list of data from the underlying data storage.
        /// </returns>
        public static IQueryable<vw_UserAssignedBlocks> GetLineOfBusinessesByUser(String userId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetLineOfBusinessesByUser(userId);
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
        /// Retrieves a list of line of businesses based on RoleId.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <param name="roleId">The role id.</param>
        /// <returns>
        /// A <see cref="SysXBlock"/> list of data from the underlying data storage.
        /// </returns>
        public static IQueryable<lkpSysXBlock> GetUserLineOfBusinessesByRoleId(String roleId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetUserLineOfBusinessesByRoleId(roleId);
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
        /// Determines whether [is line of business exists] [the specified line of business name].
        /// </summary>
        /// <param name="lineOfBusinessName">Name of the line of business.</param>
        /// <param name="existingLineOfBusinessName">Name of the existing line of business.</param>
        /// <returns><c>true</c> if [is line of business exists] [the specified line of business name]; otherwise, <c>false</c>.</returns>
        public static Boolean IsLineOfBusinessExists(String lineOfBusinessName, String existingLineOfBusinessName = null)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().IsLineOfBusinessExists(lineOfBusinessName, existingLineOfBusinessName);
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
        /// Line of business code exists or not
        /// </summary>
        /// <param name="newCode">New LOB Code</param>
        /// <param name="oldCode">Existing LOB Code</param>
        /// <returns>True, if exists else false</returns>
        public static Boolean IsLOBCodeExist(String newCode, String oldCode)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().IsLOBCodeExist(newCode, oldCode);
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

        #region Manage Users and Roles

        /// <summary>
        /// Checks if the user exists in system dataBALUtils.
        /// </summary>
        /// <param name="enteredUserName">Name of the entered user.</param>
        /// <param name="existingUserName">Name of the existing user.</param>
        /// <returns><c>true</c> if [is user exists] [the specified entered user name]; otherwise, <c>false</c>.</returns>
        public static Boolean IsUserExists(String enteredUserName, String existingUserName = null)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().IsUserExists(enteredUserName, existingUserName);
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
        /// Performs an update operation for aspnet user.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <exception cref="Exception">    Thrown when an exception error condition occurs.</exception>
        /// <param name="modifiedUser">The modified user.</param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        public static Boolean UpdateUser(aspnet_Users modifiedUser)
        {
            try
            {
                return (BALUtils.GetSecurityRepoInstance() as IBaseRepository).UpdateObjectEntity(modifiedUser);
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
        /// Retrieves aspnet users based on userId.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <exception cref="Exception">    Thrown when an exception error condition occurs.</exception>
        /// <param name="userId">              The user id.</param>
        /// <param name="loadMembership">      if set to <c>true</c> [load membership].</param>
        /// <param name="loadOrganizationUser">if set to <c>true</c> [load organization user].</param>
        /// <returns>
        /// The user by identifier.
        /// </returns>
        public static aspnet_Users GetUserById(String userId, Boolean loadMembership, Boolean loadOrganizationUser)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetUserById(userId, loadMembership, loadOrganizationUser);
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
        /// Retrieves a aspnet user based on user name.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <exception cref="Exception">    Thrown when an exception error condition occurs.</exception>
        /// <param name="userName">            Name of the user.</param>
        /// <param name="loadMembership">      if set to <c>true</c> [load membership].</param>
        /// <param name="loadOrganizationUser">if set to <c>true</c> [load organization user].</param>
        /// <returns>
        /// The user by name.
        /// </returns>
        public static aspnet_Users GetUserByName(String userName, Boolean loadMembership, Boolean loadOrganizationUser)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetUserByName(userName, loadMembership, loadOrganizationUser);
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
        /// Retrieves user name by email.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <exception cref="Exception">    Thrown when an exception error condition occurs.</exception>
        /// <param name="email">The email.</param>
        /// <returns>
        /// The user name by email.
        /// </returns>
        public static String GetUserNameByEmail(String email)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetUserNameByEmail(email);
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
        /// Retrieves a list of all of aspnet user.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <exception cref="Exception">    Thrown when an exception error condition occurs.</exception>
        /// <param name="loadRoleDetails">if set to <c>true</c> [load role details].</param>
        /// <returns>
        /// A <see cref="aspnet_Roles"/> list of data from the underlying data storage.
        /// </returns>
        public static IQueryable<aspnet_Roles> GetRoles(Boolean loadRoleDetails)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetRoles(loadRoleDetails);
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
        /// Determines whether the current role exists.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <exception cref="Exception">    Thrown when an exception error condition occurs.</exception>
        /// <param name="roleName">Name of the role.</param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        public static Boolean RoleExists(String roleName)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().RoleExists(roleName);
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
        /// Determines whether the current role is assigned to any active user.
        /// </summary>
        /// <param name="roleId">The current Role's Id</param>
        /// <returns><c>true</c> if [is role in use] [the specified role id]; otherwise, <c>false</c>.</returns>
        public static Boolean IsRoleInUse(String roleId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().IsRoleInUse(roleId);
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
        /// Retrieves a list of all aspnet users based on userName.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <exception cref="Exception">    Thrown when an exception error condition occurs.</exception>
        /// <param name="userName">Name of the user.</param>
        /// <returns>
        /// A <see cref="aspnet_Roles"/> list of data from the underlying data storage.
        /// </returns>
        public static List<aspnet_Roles> GetUserRoles(String userName)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetUserRoles(userName);
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
        /// Determines whether the specified feature is available for the user.
        /// </summary>
        /// <param name="userName">The user name of the current user.</param>
        /// <param name="featureId">The current feature Id</param>
        /// <param name="blockId">The current Block Id</param>
        /// <returns><c>true</c> if [is feature available to user] [the specified user name]; otherwise, <c>false</c>.</returns>
        public static Boolean IsFeatureAvailableToUser(String userName, Int32 featureId, Int32 blockId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().IsFeatureAvailableToUser(userName, featureId, blockId);
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
        /// Retrieves a list of all roles of user based on User's Id.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <exception cref="Exception">    Thrown when an exception error condition occurs.</exception>
        /// <param name="userId">The user id.</param>
        /// <returns>
        /// A <see cref="aspnet_Roles"/> list of data from the underlying data storage.
        /// </returns>
        public static List<aspnet_Roles> GetUserRolesById(String userId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetUserRolesById(userId);
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
        /// Retrieves a list of all users in a particular role.
        /// </summary>
        /// <param name="roleName">Name of the role.</param>
        /// <returns>A <see cref="aspnet_Users"/> list of data from the underlying data storage.</returns>
        public static List<aspnet_Users> GetUsersInRole(String roleName)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetUsersInRole(roleName);
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
        /// Determines whether the current use is associated with current role.
        /// </summary>
        /// <param name="userName">The current user's name</param>
        /// <param name="roleName">The current role name</param>
        /// <returns><c>true</c> if [is user in role] [the specified user name]; otherwise, <c>false</c>.</returns>
        public static Boolean IsUserInRole(String userName, String roleName)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().IsUserInRole(userName, roleName);
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
        /// Retrieves an array for user name in particular role name and name would match.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <param name="roleName">       Name of the role.</param>
        /// <param name="userNameToMatch">The user name to match.</param>
        /// <returns>
        /// The found users in role.
        /// </returns>
        public static String[] FindUsersInRole(String roleName, String userNameToMatch)
        {
            try
            {
                IEnumerable<aspnet_Users> aspnetUsers = BALUtils.GetSecurityRepoInstance().FindUsersInRole(roleName, userNameToMatch);
                var userNames = from u in aspnetUsers
                                select u.UserName;
                return userNames.ToArray();
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
        /// Retrieves the User details of Super Admin.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <exception cref="Exception">    Thrown when an exception error condition occurs.</exception>
        /// <returns>
        /// An enumerator that allows foreach to be used to process get system x coordinate admin user
        /// identifiers in this collection.
        /// </returns>
        public static IEnumerable<Int32> GetSysXAdminUserIds()
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetSysXAdminUserIds();
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
        /// Retrieves a list of all Roles based on userId.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <exception cref="Exception">    Thrown when an exception error condition occurs.</exception>
        /// <param name="userId">The user id.</param>
        /// <returns>
        /// The roles by user identifier.
        /// </returns>
        public static IQueryable<aspnet_Roles> GetRolesByUserId(String userId, Boolean IsUserGroup = false, Int32 organizationID = -1)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetRolesByUserId(userId, organizationID);

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
        /// Retrieves a list of all Roles based on userId.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <exception cref="Exception">    Thrown when an exception error condition occurs.</exception>
        /// <param name="userId">The user id.</param>
        /// <returns>
        /// The roles by user identifier.
        /// </returns>
        public static IQueryable<aspnet_Roles> GetRolesByUserId(Int32 UserGroupId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetRolesByUserIdInUserGroup(UserGroupId);


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
        /// Retrieves a list of all Roles based on userId.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <exception cref="Exception">    Thrown when an exception error condition occurs.</exception>
        /// <param name="tenantProductId">Current tenant's product id.</param>
        /// <returns>
        /// The roles by user identifier.
        /// </returns>
        public static IQueryable<aspnet_Roles> GetAspnetRolesByTenantProductId(Int32 tenantProductId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetAspnetRolesByTenantProductId(tenantProductId);
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
        /// Gets the roles by assign to role identifier.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <exception cref="Exception">    Thrown when an exception error condition occurs.</exception>
        /// <param name="loadRoleDetails">The current user's role</param>
        /// <param name="assignToRoleId"> The current assignToRoleId</param>
        /// <returns>
        /// The roles by assign to role identifier.
        /// </returns>
        public static IQueryable<aspnet_Roles> GetRolesByAssignToRoleId(Boolean loadRoleDetails, String assignToRoleId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetRolesByAssignToRoleId(loadRoleDetails, assignToRoleId);
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

        public static String GetTenantName(Int32 tenantId, Int32 assignToProductId, Int32 assignToDepartmentId)
        {

            try
            {
                return BALUtils.GetSecurityRepoInstance().GetTenantName(tenantId, assignToProductId, assignToDepartmentId);
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

        #region Manage Role Details

        /// <summary>
        /// Retrieves a list of all role details.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <exception cref="Exception">    Thrown when an exception error condition occurs.</exception>
        /// <returns>
        /// The role detail.
        /// </returns>
        public static List<RoleDetail> GetRoleDetail()
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetRoleDetail().ToList();
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
        /// Retrieves role details.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <param name="isAdmin">      if set to <c>true</c> [is admin].</param>
        /// <param name="currentUserId">The current user id.</param>
        /// <returns>
        /// The role detail.
        /// </returns>
        public static IQueryable<RoleDetail> GetRoleDetail(Boolean isAdmin, Int32 currentUserId)
        {
            try
            {
                //return BALUtils.GetSecurityRepoInstance().GetRoleDetail(isAdmin, currentUserId);
                IQueryable<RoleDetail> roleDetail = BALUtils.GetSecurityRepoInstance().GetRoleDetail(isAdmin, currentUserId);
                roleDetail.ForEach(item =>
                {
                    item.RoleName = (!item.Name.IsNull()) ? item.Name.Split(new char[] { '_' }).FirstOrDefault() : String.Empty;
                    item.CreatedByUserName = (!item.OrganizationUser.IsNull()) ? item.OrganizationUser.LastName + SysXUtils.GetMessage(ResourceConst.SECURITY_COMMA) + " " + item.OrganizationUser.FirstName : String.Empty;
                });

                return roleDetail;
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
        /// Retrieves role details based on roleDetailId.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <param name="roleDetailId">The role detail id.</param>
        /// <returns>
        /// The role detail by identifier.
        /// </returns>
        public static RoleDetail GetRoleDetailById(String roleDetailId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetRoleDetailById(roleDetailId);
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
        /// Performs an insert operation for RoleDetail.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <param name="roleDetail">The role detail.</param>
        public static RoleDetail AddRoleDetail(RoleDetail roleDetail)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().AddRoleDetail(roleDetail);
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
        /// Performs an update operation for RoleDetail Entity.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <param name="roleDetail">The role detail.</param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        public static Boolean UpdateRoleDetail(RoleDetail roleDetail)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().UpdateRole(roleDetail);
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
        /// Performs a delete operation for RoleDetail Entity.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <param name="roleDetail">The role detail.</param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        public static Boolean DeleteRoleDetail(RoleDetail roleDetail)
        {
            try
            {
                BALUtils.GetSecurityRepoInstance().DeleteRoleDetail(roleDetail);
                return true;
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
        /// Retrieves an instance of application.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <exception cref="Exception">    Thrown when an exception error condition occurs.</exception>
        /// <returns>
        /// The application.
        /// </returns>
        public static aspnet_Applications GetApplication()
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetApplication();
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
        /// Check if the role exists in the system dataBALUtils.
        /// </summary>
        /// <param name="productId">The value for product id.</param>
        /// <param name="enteredRoleName">Name of the entered role.</param>
        /// <param name="existingRoleName">Name of the existing role.</param>
        /// <returns><c>true</c> if [is role exists] [the specified entered role name]; otherwise, <c>false</c>.</returns>
        public static Boolean IsRoleExists(Int32 productId, String enteredRoleName, String existingRoleName = null)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().IsRoleExists(productId, enteredRoleName, existingRoleName);
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
        /// Retrieves role details based on roleDetailId.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <param name="roleDetailId">The role detail id.</param>
        /// <returns>
        /// The role detail by identifier.
        /// </returns>
        public static List<String> getDefaultRoleDetailIds(List<Int32> productId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().getDefaultRoleDetailIds(productId);
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
        /// Get Default Role Detail Ids for Shared User
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static List<String> GetDefaultRoleDetailIdsForSharedUser(List<Int32> productId, String userID)
        {
            try
            {
                List<String> roleDetails = new List<String>();
                Entity.SharedDataEntity.AgencyUser agencyUser = BALUtils.GetProfileSharingRepoInstance().GetAgencyUserByUserID(userID);
                Boolean agencyUserPermission = agencyUser.AGU_AgencyUserPermission;
                Boolean rotationPackagePermission = agencyUser.AGU_RotationPackagePermission;
                if (agencyUserPermission && rotationPackagePermission)
                    roleDetails.Add(SharedUserRoleDetails.AgencyUserAndRotationPackageRole);
                else if (agencyUserPermission)
                    roleDetails.Add(SharedUserRoleDetails.AgencyUserRole);
                else if (rotationPackagePermission)
                    roleDetails.Add(SharedUserRoleDetails.RotationPackageRole);

                return roleDetails;
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

        #region Manage Aspnet Users

        /// <summary>
        /// Retrieves a list of all Aspnet Users.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <returns>
        /// The aspnet users.
        /// </returns>
        public static List<aspnet_Users> GetAspnetUsers()
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetAspnetUsers().ToList();
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
        /// Retrieves a list of Aspnet User based on UserId.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <param name="userId">user's id</param>
        /// <returns>
        /// The aspnet user.
        /// </returns>
        public static aspnet_Users GetAspnetUser(String userId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetAspnetUser(userId);
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
        /// Performs an insert operation for Aspnet_User entity.
        /// </summary>
        /// <param name="aspnetUser">aspnet_Users</param>
        /// <returns>aspnet_Users</returns>
        public static aspnet_Users AddAspnetUser(aspnet_Users aspnetUser)
        {
            try
            {
                return (BALUtils.GetSecurityRepoInstance() as IBaseRepository).AddObjectEntity(aspnetUser);
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
        /// Performs an update operation for Aspnet_User entity in dataBALUtils.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <param name="aspnetUser">aspnet_Users.</param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        public static Boolean UpdateAspnetUser(aspnet_Users aspnetUser)
        {
            try
            {
                return (BALUtils.GetSecurityRepoInstance() as IBaseRepository).UpdateObjectEntity(aspnetUser);
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

        #region Manage Organization Users

        /// <summary>
        /// Retrieves a list of all mapped user with organizations.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <param name="isAdmin">      if set to <c>true</c> [is admin].</param>
        /// <param name="currentUserId">The current user id.</param>
        /// <returns>
        /// An enumerator that allows foreach to be used to process get mapped organization users in this
        /// collection.
        /// </returns>
        public static List<OrganizationUser> GetMappedOrganizationUsers(Boolean isAdmin, Int32 currentUserId)
        {
            try
            {
                //return BALUtils.GetSecurityRepoInstance().GetMappedOrganizationUsers(isAdmin, currentUserId);
                List<OrganizationUser> organizationUser = BALUtils.GetSecurityRepoInstance().GetMappedOrganizationUsers(isAdmin, currentUserId);
                List<Int32> lstCreadedByIds = organizationUser.DistinctBy(x => x.CreatedByID).Select(x => x.CreatedByID).ToList();
                List<OrganizationUser> lstCreatedByOrganizationUsers = BALUtils.GetSecurityRepoInstance().GetOrganizationUserList(lstCreadedByIds);

                organizationUser.ForEach(item =>
                {
                    OrganizationUser createdByUserDetails = lstCreatedByOrganizationUsers.FirstOrDefault(x => x.OrganizationUserID == item.CreatedByID);
                    item.IsLockedOut = (!item.aspnet_Users.IsNull() && !item.aspnet_Users.aspnet_Membership.IsNull()) ? item.aspnet_Users.aspnet_Membership.IsLockedOut : false;
                    item.LastActivityDate = (!item.aspnet_Users.IsNull()) ? item.aspnet_Users.LastActivityDate.ToString() : String.Empty;

                    if (!createdByUserDetails.IsNull())
                    {
                        item.CreatedByUserName = createdByUserDetails.LastName + SysXUtils.GetMessage(ResourceConst.SECURITY_COMMA) + " " + createdByUserDetails.FirstName;
                    }
                    else
                    {
                        item.CreatedByUserName = SysXUtils.GetMessage(ResourceConst.SPACE);
                    }
                });

                return organizationUser;
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
        /// Retrieves a list of all Organization User based on OrganizationUserId.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <param name="organizationUserId">Int32.</param>
        /// <returns>
        /// The organization user.
        /// </returns>
        public static OrganizationUser GetOrganizationUser(Int32 organizationUserId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetOrganizationUser(organizationUserId);
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
        /// Updates the status of whether the client admin can receive Internal message from Applicants or Not
        /// </summary>
        /// <param name="organizationUserId"></param>
        /// <returns></returns>
        public static Boolean UpdateInternalMsgNotificationSettings(Int32 organizationUserId, Int32 currentUserId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().UpdateInternalMsgNotificationSettings(organizationUserId, currentUserId);
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

        public static List<OrganizationUser> GetOrganizationUserListForUserId(String userID)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetOrganizationUserListForUserId(userID);
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
        /// Gets the list of residential histories for the given organisation user ID.
        /// </summary>
        /// <param name="organizationUserId">Organisation User ID</param>
        /// <returns>List of residential histories</returns>
        public static List<ResidentialHistory> GetUserResidentialHistories(Int32 organizationUserId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetUserResidentialHistories(organizationUserId);
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
        /// Gets the list of residential histories for the given organisation user ID.
        /// </summary>
        /// <param name="organizationUserId">Organisation User ID</param>
        /// <returns>List of residential histories</returns>
        public static List<ResidentialHistoryProfile> GetUserResidentialHistoryProfiles(Int32 organizationUserProfileId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetUserResidentialHistoryProfiles(organizationUserProfileId);
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
        /// Get the OrganziationUser Details by OrganizationUserId
        /// </summary>
        /// <param name="organizationUserId"></param>
        /// <returns></returns>
        public static Entity.OrganizationUser GetOrganizationUserDetailByOrganizationUserId(int organizationUserId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetOrganizationUserDetail(organizationUserId);
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

        // <summary>
        /// Retrieves a list of all Organization User based on UserVerificationCode.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <param name="organizationUserId">Int32.</param>
        /// <returns>
        /// The organization user.
        /// </returns>
        public static OrganizationUser GetOrganizationUserByVerificationCode(String userVerificationCode)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetOrganizationUserByVerificationCode(userVerificationCode);
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
        /// Performs an insert operation for Organization User details.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <param name="user">            The user.</param>
        /// <param name="organizationUser">The organization user.</param>
        public static OrganizationUser AddOrganizationUser(OrganizationUser organizationUser, aspnet_Users user = null)
        {
            try
            {
                Boolean aWSUseS3 = false;
                if (!ConfigurationManager.AppSettings["AWSUseS3"].IsNullOrEmpty())
                {
                    aWSUseS3 = Convert.ToBoolean(ConfigurationManager.AppSettings["AWSUseS3"]);
                }
                BALUtils.GetSecurityRepoInstance().AddOrganizationUserInTransaction(user, organizationUser);
                if (organizationUser.IsApplicant == true)
                {
                    if (!organizationUser.PhotoName.IsNullOrEmpty())
                    {
                        String date = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("mmss") + DateTime.Now.Millisecond.ToString();
                        String fileName = "UP_" + organizationUser.Organization.TenantID.Value.ToString() + "_" + organizationUser.OrganizationUserID.ToString() + "_" + date + Path.GetExtension(organizationUser.PhotoName);
                        String newFileName = String.Empty;
                        if (aWSUseS3 == false)
                        {
                            newFileName = organizationUser.PhotoName.Substring(0, organizationUser.PhotoName.LastIndexOf("\\") + 1) + fileName;
                            ComplianceDataManager.RenameFile(organizationUser.PhotoName, newFileName);
                        }
                        else
                        {
                            newFileName = organizationUser.PhotoName.Substring(0, organizationUser.PhotoName.LastIndexOf(@"/") + 1) + fileName;
                            //AWS code to save document to S3 location
                            AmazonS3Documents objAmazonS3 = new AmazonS3Documents();
                            newFileName = objAmazonS3.MoveDocument(organizationUser.PhotoName, newFileName);
                        }

                        BALUtils.GetSecurityRepoInstance().UpdateOrganizationUser(organizationUser.OrganizationUserID, newFileName);
                        organizationUser.PhotoName = newFileName;
                    }
                    ClientSecurityManager.CopyUserToClient(organizationUser);

                    //UAT 1834: NYU Migration 2 of 3: Applicant Complete Order Process.
                    if (organizationUser.Organization.TenantID.IsNotNull() && !organizationUser.PrimaryEmailAddress.IsNullOrEmpty())
                    {
                        ClientSecurityManager.UpdateBulkOrderUploadForOrgUser(organizationUser.Organization.TenantID.Value, organizationUser.OrganizationUserID, organizationUser.PrimaryEmailAddress);
                    }

                }

                //ComplianceDataManager.CopyUserToClientOrgProfile(organizationUser);
                //if (organizationUser.Organization.TenantID != null && organizationUser.IsApplicant != null && organizationUser.IsApplicant.Value)
                //    ComplianceDataManager.SubscribeDefaultPackage(organizationUser, organizationUser.Organization.TenantID.Value);

                return organizationUser;

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

        public static OrganizationUserProfile AddOrganizationUserProfile(OrganizationUser orgUser, Int32? SelectedCommLang = null, Int32? TenantID = null)
        {
            OrganizationUserProfile orgUserProfile = new OrganizationUserProfile();
            try
            {
                orgUserProfile.OrganizationUserID = orgUser.OrganizationUserID;
                orgUserProfile.UserTypeID = orgUser.UserTypeID;
                orgUserProfile.AddressHandleID = orgUser.AddressHandleID;
                //orgUser.AddressHandle = myOrgUser.AddressHandle;
                orgUserProfile.FirstName = orgUser.FirstName;
                orgUserProfile.LastName = orgUser.LastName;
                orgUserProfile.VerificationCode = orgUser.VerificationCode;
                orgUserProfile.OfficeReturnDateTime = orgUser.OfficeReturnDateTime;
                orgUserProfile.IsDeleted = orgUser.IsDeleted;
                orgUserProfile.IsActive = orgUser.IsActive;
                orgUserProfile.ExpireDate = orgUser.ExpireDate;
                orgUserProfile.CreatedByID = orgUser.CreatedByID;
                orgUserProfile.CreatedOn = orgUser.CreatedOn;
                orgUserProfile.ModifiedByID = orgUser.ModifiedByID;
                orgUserProfile.ModifiedOn = orgUser.ModifiedOn;
                orgUserProfile.PhotoName = orgUser.PhotoName;
                orgUserProfile.OriginalPhotoName = orgUser.OriginalPhotoName;
                orgUserProfile.DOB = orgUser.DOB;
                orgUserProfile.SSN = orgUser.SSN;
                orgUserProfile.Gender = orgUser.Gender;
                orgUserProfile.PhoneNumber = orgUser.PhoneNumber;
                orgUserProfile.MiddleName = orgUser.MiddleName;
                orgUserProfile.Alias1 = orgUser.Alias1;
                orgUserProfile.Alias2 = orgUser.Alias2;
                orgUserProfile.Alias3 = orgUser.Alias3;
                orgUserProfile.PrimaryEmailAddress = orgUser.PrimaryEmailAddress;
                orgUserProfile.SecondaryEmailAddress = orgUser.SecondaryEmailAddress;
                orgUserProfile.SecondaryPhone = orgUser.SecondaryPhone;
                //UAT-2447
                orgUserProfile.IsInternationalPhoneNumber = orgUser.IsInternationalPhoneNumber;
                orgUserProfile.IsInternationalSecondaryPhone = orgUser.IsInternationalSecondaryPhone;
                //CBI|| CABS || Add Suffix ID
                orgUserProfile.UserTypeID = orgUser.UserTypeID.IsNullOrEmpty() ? (Int32?)null : orgUser.UserTypeID;


                if (orgUser.PersonAlias.IsNotNull())
                {
                    List<PersonAlia> currentAliasList = orgUser.PersonAlias.Where(x => x.PA_IsDeleted == false).ToList();

                    //CBI|| CABS
                    Boolean isLocationServiceTenant = IsLocationServiceTenant(orgUser.Organization.TenantID.Value);
                    List<Entity.lkpSuffix> lstSuffix = GetSuffixes();
                    //
                    foreach (PersonAlia tempPersonAlias in currentAliasList)
                    {
                        PersonAliasProfile personAliasProfile = new PersonAliasProfile();
                        personAliasProfile.PAP_FirstName = tempPersonAlias.PA_FirstName;
                        personAliasProfile.PAP_LastName = tempPersonAlias.PA_LastName;
                        //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                        personAliasProfile.PAP_MiddleName = tempPersonAlias.PA_MiddleName;
                        personAliasProfile.PAP_IsDeleted = false;
                        //personAliasProfile.PAP_CreatedBy = orgUser.OrganizationUserID;
                        personAliasProfile.PAP_CreatedBy = tempPersonAlias.PA_CreatedBy;
                        personAliasProfile.PAP_CreatedOn = DateTime.Now;
                        orgUserProfile.PersonAliasProfiles.Add(personAliasProfile);

                        //CBI || CABS 
                        if (!isLocationServiceTenant.IsNullOrEmpty() && isLocationServiceTenant && !tempPersonAlias.PersonAliasExtensions.IsNullOrEmpty())
                        {
                            var data = tempPersonAlias.PersonAliasExtensions.FirstOrDefault(cond => !cond.PAE_IsDeleted);
                            if (!data.IsNullOrEmpty() && !data.PAE_Suffix.IsNullOrEmpty())
                            {
                                PersonAliasProfileExtension personAliasProfileExtension = new PersonAliasProfileExtension();
                                personAliasProfileExtension.PAPE_Suffix = data.PAE_Suffix;
                                personAliasProfileExtension.PAPE_CreatedBy = tempPersonAlias.PA_CreatedBy;
                                personAliasProfileExtension.PAPE_CreatedOn = DateTime.Now;
                                personAliasProfile.PAP_IsDeleted = false;
                                personAliasProfile.PersonAliasProfileExtensions.Add(personAliasProfileExtension);
                            }
                        }
                    }
                }
                if (orgUser.ResidentialHistories.IsNotNull())
                {
                    List<ResidentialHistory> lstResidentialHistories = orgUser.ResidentialHistories.Where(x => x.RHI_IsDeleted == false).ToList();
                    foreach (ResidentialHistory resHistory in lstResidentialHistories)
                    {
                        ResidentialHistoryProfile residentialHistoryProfile = new ResidentialHistoryProfile();
                        residentialHistoryProfile.RHIP_AddressId = resHistory.RHI_AddressId;
                        residentialHistoryProfile.RHIP_IsCurrentAddress = resHistory.RHI_IsCurrentAddress.HasValue ? resHistory.RHI_IsCurrentAddress.Value : false;
                        residentialHistoryProfile.RHIP_IsPrimaryResidence = resHistory.RHI_IsPrimaryResidence.HasValue ? resHistory.RHI_IsPrimaryResidence.Value : false;
                        residentialHistoryProfile.RHIP_ResidenceStartDate = resHistory.RHI_ResidenceStartDate;
                        residentialHistoryProfile.RHIP_ResidenceEndDate = resHistory.RHI_ResidenceEndDate;
                        residentialHistoryProfile.RHIP_IsDeleted = resHistory.RHI_IsDeleted.HasValue ? resHistory.RHI_IsDeleted.Value : false;
                        residentialHistoryProfile.RHIP_CreatedBy = resHistory.RHI_CreatedByID;
                        residentialHistoryProfile.RHIP_CreatedOn = resHistory.RHI_CreatedOn;
                        residentialHistoryProfile.RHIP_SequenceOrder = resHistory.RHI_SequenceOrder;
                        orgUserProfile.ResidentialHistoryProfiles.Add(residentialHistoryProfile);
                    }
                }



                BALUtils.GetSecurityRepoInstance().AddUpdateLanguageMapping(orgUser, SelectedCommLang); //UAT 3824



                OrganizationUserProfile orgProfileMaster = BALUtils.GetSecurityRepoInstance().AddOrganizationUserProfile(orgUserProfile);
                //ComplianceDataManager.CopyUserToClientOrgProfile(orgProfileMaster);
                ClientSecurityManager.CopyUserToClientOrgProfile(orgProfileMaster, TenantID);
                return orgProfileMaster;
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

        //public static Entity.ClientEntity.OrganizationUserDepartment GetDeptFromOrgUserDept(OrganizationUser orgUser)
        //{
        //    return ClientSecurityManager.OrganizationUserDepartment(orgUser);

        //}

        /// <summary>
        /// Performs an update operation for OrganizationUser Details.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <param name="organizationUser">OrganizationUser.</param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        /// <exception cref="Exception">Thrown when an exception error condition occurs.</exception>
        public static Boolean UpdateOrganizationUser(OrganizationUser organizationUser)
        {
            try
            {
                if ((BALUtils.GetSecurityRepoInstance() as IBaseRepository).UpdateObjectEntity(organizationUser))
                {
                    if (organizationUser.IsApplicant == true)
                    {
                        ClientSecurityManager.UpdateUserToClient(organizationUser);
                    }
                    return true;
                }
                return false;
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

        public static Boolean SyncUsersProfilePictureInAllTenant(OrganizationUser organizationUser)
        {
            try
            {
                List<OrganizationUser> profileListToBeUpdated = BALUtils.GetSecurityRepoInstance().SyncUsersProfilePictureInAllTenant(organizationUser);
                foreach (OrganizationUser profileToUpdate in profileListToBeUpdated)
                {
                    ClientSecurityManager.UpdateUsersProfilePicture(profileToUpdate);
                }
                return true;
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
        /// Performs a delete operation for OrganizationUser Details.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <param name="organizationUser">The organization user.</param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        /// ### <exception cref="Exception">Thrown when an exception error condition occurs.</exception>
        public static Boolean DeleteOrganizationUser(OrganizationUser organizationUser)
        {
            try
            {
                BALUtils.GetSecurityRepoInstance().DeleteOrganizationUser(organizationUser);
                return true;
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
        /// Gets an organization user information by user identifier.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <exception cref="Exception">    Thrown when an exception error condition occurs.</exception>
        /// <param name="userId">The current user's Id</param>
        /// <returns>
        /// The organization user information by user identifier.
        /// </returns>
        public static IQueryable<OrganizationUser> GetOrganizationUserInfoByUserId(String userId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetOrganizationUserInfoByUserId(userId);
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
        /// Gets an organization users details.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <exception cref="Exception">    Thrown when an exception error condition occurs.</exception>
        /// <param name="userName">The current user's name</param>
        /// <returns>
        /// The organization users details.
        /// </returns>
        public static OrganizationUser GetOrganizationUsersDetails(String userName)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetOrganizationUsersDetails(userName);
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
        /// 
        /// </summary>
        /// <param name="ssn"></param>
        /// <returns></returns>
        public static OrganizationUser GetOrganizationUsersBySSN(String ssn)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetOrganizationUsersBySSN(ssn);
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
        /// Gets an organization users by email.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <exception cref="Exception">    Thrown when an exception error condition occurs.</exception>
        /// <param name="email">The current user's email</param>
        /// <returns>
        /// The organization users by email.
        /// </returns>
        public static List<OrganizationUser> GetOrganizationUsersByEmail(String email)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetOrganizationUsersByEmail(email).ToList();
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
        /// Gets the list of users working in the organization associated with the given tenant Id. 
        /// </summary>
        /// <param name="tenantId">Tenant Id</param>
        /// <returns>List of acive Users</returns>
        public static List<OrganizationUser> GetOganisationUsersByTanentId(Int32 tenantId, Boolean IsADBAdmin = false, Boolean IsComplAssignmentScreen = false, Boolean RotAssignmentScreen = false, Boolean IsDataEntryScreen = false, Boolean IsLocEnrollerScreen = false)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetOganisationUsersByTanentId(tenantId, IsADBAdmin, IsComplAssignmentScreen, RotAssignmentScreen, IsDataEntryScreen, IsLocEnrollerScreen).ToList();
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

        public static List<OrganizationUser> GetOganisationUsersByTanentIdOfLoggedInUser(Int32 tenantId, Int32 currentloggedInUser)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetOganisationUsersByTanentIdOfLoggedInUser(tenantId, currentloggedInUser);
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

        public static List<OrganizationUser> GetOganisationUsersByUserID(Int32 organizationUserID)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetOganisationUsersByUserID(organizationUserID);
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

        #region Manage Mapping of user and roles

        /// <summary>
        /// Performs mapping between user and roles.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <exception cref="Exception">    Thrown when an exception error condition occurs.</exception>
        /// <param name="userId"> The user id.</param>
        /// <param name="roleIds">The role Ids.</param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        public static Boolean UserRoleMapping(String userId, List<String> roleIds, Boolean deleteExistingRoles = true)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().UserRoleMapping(userId, roleIds, deleteExistingRoles);
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

        #region Manage Role Permission ProductFeature

        public static IQueryable<RolePermissionProductFeature> GetProductFeatureRoles(Int32 productFeatureId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetProductFeatureRoles(productFeatureId);
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
        /// Retrieves a list of all features associated with the current role.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <exception cref="Exception">    Thrown when an exception error condition occurs.</exception>
        /// <param name="roleDetailId">The role detail id.</param>
        /// <param name="productId">   The product id.</param>
        /// <returns>
        /// A <see cref="RolePermissionProductFeature"/> list of data from the underlying data storage.
        /// </returns>
        public static IQueryable<RolePermissionProductFeature> GetFeatureForRole(String roleDetailId, Int32 productId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetFeatureForRole(roleDetailId, productId);
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
        /// Performs mapping between roles and features.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <param name="roleId">             The role id.</param>
        /// <param name="productId">          The product id.</param>
        /// <param name="featurePermissions"> The feature permissions.</param>
        /// <param name="updatedSysXBlockIds">The updated sys X block Ids.</param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        /// <exception cref="Exception">Thrown when an exception error condition occurs.</exception>
        public static Boolean RoleFeatureMapping(String roleId, Int32 productId, Dictionary<Int32, Int32> featurePermissions, List<Int32> updatedSysXBlockIds, List<RoleFeatureActionContract> roleFeatureActions)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().RoleFeatureMapping(roleId, productId, featurePermissions, updatedSysXBlockIds, roleFeatureActions);
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
        /// Retrieves a list of all products for each tenant based on tenantId.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <exception cref="Exception">    Thrown when an exception error condition occurs.</exception>
        /// <param name="tenantId">The tenant id.</param>
        /// <returns>
        /// The products for tenant.
        /// </returns>
        public static IQueryable<TenantProduct> GetProductsForTenant(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetProductsForTenant(tenantId);
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
        /// Retrieves all the relations from RolePermissionProductFeature table.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <exception cref="Exception">    Thrown when an exception error condition occurs.</exception>
        public static IQueryable<RolePermissionProductFeature> GetRolePermissionProductFeatures()
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetRolePermissionProductFeatures();
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

        #region Manage Policies

        /// <summary>
        /// Retrieves a list of all policy register controls.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <exception cref="Exception">    Thrown when an exception error condition occurs.</exception>
        /// <returns>
        /// A <see cref="PolicyRegisterUserControl"/> list of data from the underlying data storage.
        /// </returns>
        public static IQueryable<PolicyRegisterUserControl> GetPolicyRegisterControls()
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetPolicyRegisterControls();
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
        /// Retrieves a list of all policy register controls based on role id.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <exception cref="Exception">    Thrown when an exception error condition occurs.</exception>
        /// <param name="roleId">The role id.</param>
        /// <returns>
        /// A <see cref="PolicyRegisterUserControl"/> list of data from the underlying data storage.
        /// </returns>
        public static IQueryable<PolicyRegisterUserControl> GetPolicyRegisterControls(String roleId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetPolicyRegisterControls(roleId);
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
        /// Retrieves the value for selected control.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <exception cref="Exception">    Thrown when an exception error condition occurs.</exception>
        /// <param name="roleId">               The role id.</param>
        /// <param name="registerUserControlId">The register user control id.</param>
        /// <returns>
        /// The control selected values.
        /// </returns>
        public static PolicySetUserControl GetControlSelectedValues(String roleId, Int32 registerUserControlId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetControlSelectedValues(roleId, registerUserControlId);
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
        /// Retrieves the list of all policy controls.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <exception cref="Exception">    Thrown when an exception error condition occurs.</exception>
        /// <returns>
        /// A <see cref="PolicyControl"/> list of data from the underlying data storage.
        /// </returns>
        public static IQueryable<PolicyControl> GetRegisteredControlList()
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetRegisteredControlList();
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
        /// Performs an update operation for policy based on policySet.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <exception cref="Exception">    Thrown when an exception error condition occurs.</exception>
        /// <param name="policySet">The policy set.</param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        public static Boolean SavePolicies(PolicySet policySet)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().SavePolicies(policySet);
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
        /// Performs an update operation for policy based on policySetUserControlList and roleId.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <exception cref="Exception">    Thrown when an exception error condition occurs.</exception>
        /// <param name="policySetUserControls">The policy set user controls.</param>
        /// <param name="roleId">               The role id.</param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        public static Boolean SavePolicies(EntityCollection<PolicySetUserControl> policySetUserControls, String roleId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().SavePolicies(policySetUserControls, roleId);
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
        /// Retrieves a list of all policies.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <exception cref="Exception">    Thrown when an exception error condition occurs.</exception>
        /// <param name="ucName">       Name of the user control.</param>
        /// <param name="aspnetRoles">  The aspnet roles.</param>
        /// <param name="sysXAdminList">The sys X admin list.</param>
        /// <param name="currentUserId">The current user id.</param>
        /// <returns>
        /// all policies.
        /// </returns>
        public static PolicySetUserControl GetAllPolicies(String ucName, List<aspnet_Roles> aspnetRoles, List<Int32> sysXAdminList, Int32 currentUserId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetAllPolicies(ucName, aspnetRoles, sysXAdminList, currentUserId);
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
        /// Retrieves the sub parent of all policies.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <exception cref="Exception">    Thrown when an exception error condition occurs.</exception>
        /// <param name="ucName">       Name of the uc.</param>
        /// <param name="roleId">       The role id.</param>
        /// <param name="allSysXAdmin"> All sys X admin.</param>
        /// <param name="currentUserId">The current user id.</param>
        /// <returns>
        /// all policies sub parent.
        /// </returns>
        public static PolicySetUserControl GetAllPoliciesSubParent(String ucName, String roleId, List<Int32> allSysXAdmin, Int32 currentUserId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetAllPoliciesSubParent(ucName, roleId, allSysXAdmin, currentUserId);
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

        #region Manage Policy Register Control

        /// <summary>
        /// Retrieves policy register control.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <exception cref="Exception">    Thrown when an exception error condition occurs.</exception>
        /// <param name="policyRegisterControlId">The policy register control id.</param>
        /// <returns>
        /// The policy register control.
        /// </returns>
        public static PolicyRegisterUserControl GetPolicyRegisterControl(Int32 policyRegisterControlId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetPolicyRegisterControl(policyRegisterControlId);
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
        /// Performs a delete operation for policy register control.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <exception cref="Exception">    Thrown when an exception error condition occurs.</exception>
        /// <param name="policyRegisterControl">The policy register control.</param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        public static Boolean DeletePolicyRegisterControl(PolicyRegisterUserControl policyRegisterControl)
        {
            try
            {
                return (BALUtils.GetSecurityRepoInstance() as IBaseRepository).DeleteObjectEntity(policyRegisterControl);
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
        /// Chooses the child policy control.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <exception cref="Exception">    Thrown when an exception error condition occurs.</exception>
        /// <param name="registerUserControlId">The register user control id.</param>
        public static Int32 SelectChildPolicyControls(long registerUserControlId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().SelectChildPolicyControls(registerUserControlId);
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
        /// Performs an insert operation for policy register control.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <exception cref="Exception">    Thrown when an exception error condition occurs.</exception>
        /// <param name="policyRegisterControl">The policy register control.</param>
        public static void AddPolicyRegisterControl(PolicyRegisterUserControl policyRegisterControl)
        {
            try
            {
                (BALUtils.GetSecurityRepoInstance() as IBaseRepository).AddObjectEntity(policyRegisterControl);
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
        /// Performs an update operation for policy register control.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <exception cref="Exception">    Thrown when an exception error condition occurs.</exception>
        /// <param name="policyRegisterControl">The policy register control.</param>
        public static void UpdatePolicyRegisterControl(PolicyRegisterUserControl policyRegisterControl)
        {
            try
            {
                (BALUtils.GetSecurityRepoInstance() as IBaseRepository).UpdateObjectEntity(policyRegisterControl);
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
        /// Query if 'enteredControlName' is exists.
        /// </summary>
        /// <param name="enteredPermissionName"> Name of the entered Control.</param>
        /// <param name="existingPermissionName">Name of the existing Control.</param>
        /// <returns>
        /// true if Policy Register Control exists, false if not.
        /// </returns>
        public static Boolean IsControlNameExists(String enteredPermissionName, String existingPermissionName = null)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().IsControlNameExists(enteredPermissionName, existingPermissionName);
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

        #region Manage Departments

        /// <summary>
        /// Retrieves all departments for Super Admin.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <exception cref="Exception">    Thrown when an exception error condition occurs.</exception>
        /// <returns>
        /// A <see cref="Organization"/> list of data from the underlying data storage.
        /// </returns>
        public static IQueryable<Organization> GetDepartmentsForSuperAdmin()
        {
            try
            {
                //return BALUtils.GetSecurityRepoInstance().GetDepartmentsForSuperAdmin();
                IQueryable<Organization> organization = BALUtils.GetSecurityRepoInstance().GetDepartmentsForSuperAdmin();

                organization.ForEach(item =>
                {
                    OrganizationUser organizationUser = SecurityManager.GetOrganizationUser(item.CreatedByID);
                    if (!organizationUser.IsNull())
                    {
                        item.CreatedByUserName = organizationUser.LastName + SysXUtils.GetMessage(ResourceConst.SECURITY_COMMA) + " " + organizationUser.FirstName;
                    }
                    else
                    {
                        item.CreatedByUserName = SysXUtils.GetMessage(ResourceConst.SPACE);
                    }
                });

                return organization;

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
        /// Retrieves a list of all departments for Product admin.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <exception cref="Exception">    Thrown when an exception error condition occurs.</exception>
        /// <param name="productId">The product id.</param>
        /// <returns>
        /// A <see cref="Organization"/> list of data from the underlying data storage.
        /// </returns>
        public static IQueryable<Organization> GetDepartmentsForProductAdmin(Int32 productId)
        {

            try
            {
                //return BALUtils.GetSecurityRepoInstance().GetDepartmentsForProductAdmin(productId);
                IQueryable<Organization> organization = BALUtils.GetSecurityRepoInstance().GetDepartmentsForProductAdmin(productId);
                organization.ForEach(item =>
                {
                    OrganizationUser createdByUserDetails = GetOrganizationUser(item.CreatedByID);
                    if (!createdByUserDetails.IsNull())
                    {
                        item.CreatedByUserName = createdByUserDetails.LastName + SysXUtils.GetMessage(ResourceConst.SECURITY_COMMA) + " " + createdByUserDetails.FirstName;
                    }
                    else
                    {
                        item.CreatedByUserName = SysXUtils.GetMessage(ResourceConst.SPACE);
                    }
                });
                return organization;
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
        /// Retrieves a list of all departments for Product admin.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <exception cref="Exception">    Thrown when an exception error condition occurs.</exception>
        /// <param name="productId">    The product id.</param>
        /// <param name="currentUserId">The current user id.</param>
        /// <returns>
        /// A <see cref="Organization"/> list of data from the underlying data storage.
        /// </returns>
        public static IQueryable<Organization> GetDepartmentsForDepartmentAdmin(Int32 productId, Int32 currentUserId)
        {
            try
            {
                //return BALUtils.GetSecurityRepoInstance().GetDepartmentsForDepartmentAdmin(productId, currentUserId);
                IQueryable<Organization> organization = BALUtils.GetSecurityRepoInstance().GetDepartmentsForDepartmentAdmin(productId, currentUserId);
                organization.ForEach(item =>
                {
                    OrganizationUser createdByUserDetails = GetOrganizationUser(item.CreatedByID);
                    if (!createdByUserDetails.IsNull())
                    {
                        item.CreatedByUserName = createdByUserDetails.LastName + SysXUtils.GetMessage(ResourceConst.SECURITY_COMMA) + " " + createdByUserDetails.FirstName;
                    }
                    else
                    {
                        item.CreatedByUserName = SysXUtils.GetMessage(ResourceConst.SPACE);
                    }
                });

                return organization;
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

        ///// <summary>
        ///// Method to check department mapping
        ///// </summary>
        ///// <param name="organizationId">organizationId</param>
        ///// <returns></returns>
        //public static Boolean IsDepartmentMapped(Int32 organizationId)
        //{
        //    try
        //    {

        //        return BALUtils.GetSecurityRepoInstance().IsDepartmentMapped(organizationId);

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

        #region Contacts

        /// <summary>
        /// Gets the contact type by contact description.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <param name="contactDesc">The contact description like Email, Fax or Phone.</param>
        /// <returns>
        /// lkpContactType.
        /// </returns>
        public static lkpContactType GetContactTypeByContactDesc(String contactDesc)
        {
            try
            {
                return LookupManager.GetAllContactTypes().Where(condition => condition.IsActive).FirstOrDefault(conditon => conditon.ContactDescription == contactDesc);
                // return BALUtils.GetSecurityRepoInstance().GetContactType().FirstOrDefault(conditon => conditon.ContactDescription == contactDesc);
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

        #region Manage User Themes

        /// <summary>
        /// Updates the aspnet_membership for the Logged-in User.
        /// </summary>
        /// <param name="aspnetMembership"></param>
        /// <returns></returns>
        public static Boolean UpdateAspnetMembershipForTheme(aspnet_Membership aspnetMembership)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().UpdateAspnetMembershipForTheme(aspnetMembership);
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
        /// Gets the Logged-in UserId.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static aspnet_Membership GetAspnetMembershipById(Guid id)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetAspnetMembershipById(id);
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

        #region User Group

        /// <summary>
        /// Insert a user group
        /// </summary>
        /// <param name="userGroup"></param>
        public static void InsertUserGroup(UserGroup userGroup)
        {
            try
            {
                (BALUtils.GetSecurityRepoInstance() as IBaseRepository).AddObjectEntity(userGroup);
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
        /// Update a user group
        /// </summary>
        /// <param name="userGroup"></param>
        public static void UpdateUserGroup(UserGroup userGroup)
        {
            try
            {
                (BALUtils.GetSecurityRepoInstance() as IBaseRepository).UpdateObjectEntity(userGroup);
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
        /// Get all user Group according to logged in user
        /// </summary>
        /// <param name="IsAdmin"></param>
        /// <param name="CreatedById"></param>
        /// <returns></returns>
        public static List<UserGroup> GetAllUserGroup(Boolean IsAdmin, Int32 CreatedById)
        {
            try
            {
                return (BALUtils.GetSecurityRepoInstance()).GetAllUserGroup(IsAdmin, CreatedById);
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
        /// Get all user in a group.
        /// </summary>
        /// <param name="userGroupId"></param>
        /// <returns></returns>
        public static IQueryable<UsersInUserGroup> GetAllUsersInAGroup(Int32 userGroupId)
        {
            return (BALUtils.GetSecurityRepoInstance()).GetAllUsersInUserGroup(userGroupId);
        }

        /// <summary>
        /// Insert user in a user group.
        /// </summary>
        /// <param name="userGroup"></param>
        public static void InsertUserInGroup(UsersInUserGroup userGroup)
        {
            try
            {
                (BALUtils.GetSecurityRepoInstance() as IBaseRepository).AddObjectEntity(userGroup);
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
        /// Delete a user from user group.
        /// </summary>
        /// <param name="userGroup"></param>
        public static void DeleteUserInGroup(UsersInUserGroup userGroup)
        {
            try
            {
                (BALUtils.GetSecurityRepoInstance() as IBaseRepository).UpdateObjectEntity(userGroup);
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
        /// Performs mapping between userGroup and roles.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <exception cref="Exception">    Thrown when an exception error condition occurs.</exception>
        /// <param name="userId"> The user id.</param>
        /// <param name="roleIds">The role Ids.</param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        public static Boolean UserGroupRoleMapping(Int32 userGroupId, List<String> roleIds)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().UserGroupRoleMapping(userGroupId, roleIds);
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
        /// Get all roles of user group.
        /// </summary>
        /// <param name="userGroupId"></param>
        /// <returns></returns>
        public static List<aspnet_Roles> GetAllRoleOfUserGroup(Int32 userGroupId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetAllRoleOfUserGroup(userGroupId);
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

        public static bool SaveRoleOfUserGroupUser(Int32 UsersInUserGroupID, String UserId, List<String> RoleInUserGroupIds)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().UserGroupsUserRoleMapping(UsersInUserGroupID, UserId, RoleInUserGroupIds);
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
        /// Get all roles of user group.
        /// </summary>
        /// <param name="userGroupId"></param>
        /// <returns></returns>
        public static List<aspnet_Roles> GetAllRoleOfUserGroupUser(String userGroupId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetAllRoleOfUserGroupUser(userGroupId);
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

        private static Permission GetUserGroupPermission(Guid userId, Int32 featureId, Int32 blockId)
        {
            Permission permission = null;
            try
            {
                IEnumerable<Permission> listpermissionUser = BALUtils.GetSecurityRepoInstance().GetUserGroupUserPermission(userId, featureId, blockId);
                IEnumerable<Permission> listpermissionUserGroup = BALUtils.GetSecurityRepoInstance().GetUserGroupPermission(userId, featureId, blockId);
                if (!listpermissionUser.IsNull() && listpermissionUser.Count() > 0)
                {
                    permission = listpermissionUser.Where(perm => perm.PermissionId == 4).OrderBy(cond => cond.PermissionId).FirstOrDefault();
                    if (permission.IsNull())
                    {
                        permission = listpermissionUser.Where(perm => perm.PermissionId == 3).FirstOrDefault();
                    }
                    if (permission.IsNull())
                    {
                        permission = listpermissionUser.Where(perm => perm.PermissionId == 1).FirstOrDefault();
                    }
                }
                else
                {
                    permission = listpermissionUserGroup.Where(perm => perm.PermissionId == 4).OrderBy(cond => cond.PermissionId).FirstOrDefault();
                    if (permission.IsNull())
                    {
                        permission = listpermissionUserGroup.Where(perm => perm.PermissionId == 3).FirstOrDefault();
                    }
                    if (permission.IsNull())
                    {
                        permission = listpermissionUserGroup.Where(perm => perm.PermissionId == 1).FirstOrDefault();
                    }
                }

                return permission;
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

        public static List<UserGroupRolePermissionProductFeature> getUserGroupRolePermissionProductFeatureByRoleId(Guid RoleId, Int32 UserGroupId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().getUserGroupRolePermissionProductFeature(RoleId, UserGroupId);
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

        #region Manage Organization User Location

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationUserId"></param>
        /// <param name="organizationLocationId"></param>
        /// <returns></returns>

        public static Boolean AddOrganizationUserLocation(Int32 organizationUserId, Int32 organizationLocationId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().AddOrganizationUserLocation(organizationUserId, organizationLocationId);
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



        #region Manage Organization User Program

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationUserId"></param>
        /// <param name="organizationLocationId"></param>
        /// <returns></returns>

        //public static Boolean AddOrganizationUserProgram(Int32 organizationUserId, Int32 programStudyId)
        //{
        //    try
        //    {
        //        return BALUtils.GetSecurityRepoInstance().AddOrganizationUserProgram(organizationUserId, programStudyId);
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

        #region Send Email

        /// <summary>
        /// UAT-2958
        /// </summary>
        /// <param name="selectedUser"></param>
        /// <param name="applicationUrl"></param>
        /// <param name="password"></param>
        public static void SendEmailForNewApplicantThroughSSO(OrganizationUser selectedUser, String applicationUrl, String password)
        {

            if (!(applicationUrl.Trim().StartsWith("http://", StringComparison.OrdinalIgnoreCase) || applicationUrl.Trim().StartsWith("https://", StringComparison.OrdinalIgnoreCase)))
            {
                applicationUrl = string.Concat(HttpContext.Current.Request.Url.Scheme, "://", applicationUrl.Trim());
            }
            CommunicationSubEvents commSubEvent = CommunicationSubEvents.NOTIFICATION_ACCOUNT_CREATION_APPLICANT_THROUGH_SSO;
            Dictionary<String, object> dictMailData = new Dictionary<string, object>();
            dictMailData.Add(EmailFieldConstants.USER_FULL_NAME, selectedUser.FirstName + " " + selectedUser.LastName);
            dictMailData.Add(EmailFieldConstants.INSTITUTE_NAME, selectedUser.Organization.Tenant.TenantName);
            dictMailData.Add(EmailFieldConstants.APPLICATION_URL, applicationUrl);
            dictMailData.Add(EmailFieldConstants.USER_NAME, selectedUser.aspnet_Users.UserName);
            dictMailData.Add(EmailFieldConstants.PASSWORD, password);
            dictMailData.Add(EmailFieldConstants.RECEIVER_ORGANIZATION_USER_ID, selectedUser.OrganizationUserID);

            Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
            mockData.UserName = selectedUser.FirstName + " " + selectedUser.LastName;
            mockData.EmailID = selectedUser.PrimaryEmailAddress;
            mockData.ReceiverOrganizationUserID = selectedUser.OrganizationUserID;

            CommunicationManager.SendEmailForNewApplicantThroughSSO(commSubEvent, mockData, dictMailData, selectedUser.Organization.TenantID.Value);
        }

        public static Boolean SendEmailForNewApplicant(OrganizationUser selectedUser, String applicationUrl, String password)
        {
            if (!(applicationUrl.Trim().StartsWith("http://", StringComparison.OrdinalIgnoreCase) || applicationUrl.Trim().StartsWith("https://", StringComparison.OrdinalIgnoreCase)))
            {
                applicationUrl = string.Concat(HttpContext.Current.Request.Url.Scheme, "://", applicationUrl.Trim());
            }
            Dictionary<String, Object> contents = new Dictionary<String, Object>
                                                      {
                                                          {EmailFieldConstants.USER_FULL_NAME,selectedUser.FirstName + " " +selectedUser.LastName},
                                                          {EmailFieldConstants.INSTITUTE_NAME,selectedUser.Organization.Tenant.TenantName},
                                                          {EmailFieldConstants.APPLICATION_URL,applicationUrl},
                                                          {EmailFieldConstants.USER_NAME,selectedUser.aspnet_Users.UserName},
                                                          {EmailFieldConstants.PASSWORD,password},
                                                          {EmailFieldConstants.RECEIVER_ORGANIZATION_USER_ID,selectedUser.OrganizationUserID}

                                                       };

            //SysXEmailService.SendMail(contents, "NEW ACCOUNT - for " + selectedUser.FirstName + " " +
            //                          selectedUser.LastName + " has been setup", selectedUser.aspnet_Users.aspnet_Membership.LoweredEmail, AppConsts.NewApplicantAccount, AppConsts.Normal);

            //return SysXEmailService.SendMail(contents, "Confirmation related to your New Account Setup", selectedUser.PrimaryEmailAddress.ToLower(), AppConsts.NewApplicantAccount, AppConsts.Normal);
            PrepareAndSendSystemMail(selectedUser.PrimaryEmailAddress.ToLower(), contents, CommunicationSubEvents.NOTIFICATION_ACCOUNT_CREATION_APPLICANT, selectedUser.Organization.TenantID, true); // UAT-4570
            return true;

        }

        public static Boolean SendEmailForInstitutionChange(OrganizationUser selectedUser, String applicationUrl)
        {
            if (!(applicationUrl.Trim().StartsWith("http://", StringComparison.OrdinalIgnoreCase) || applicationUrl.Trim().StartsWith("https://", StringComparison.OrdinalIgnoreCase)))
            {
                applicationUrl = string.Concat(HttpContext.Current.Request.Url.Scheme, "://", applicationUrl.Trim());
            }
            Dictionary<String, Object> contents = new Dictionary<String, Object>
                                                      {
                                                          {EmailFieldConstants.USER_FULL_NAME,selectedUser.FirstName + " " +selectedUser.LastName},
                                                          {EmailFieldConstants.APPLICATION_URL,applicationUrl},
                                                          {EmailFieldConstants.INSTITUTE_NAME, selectedUser.Organization.Tenant.TenantName}

                                                      };

            //SysXEmailService.SendMail(contents, "NEW ACCOUNT - for " + selectedUser.FirstName + " " +
            //                          selectedUser.LastName + " has been setup", selectedUser.aspnet_Users.aspnet_Membership.LoweredEmail, AppConsts.NewApplicantAccount, AppConsts.Normal);
            PrepareAndSendSystemMail(selectedUser.PrimaryEmailAddress.ToLower(), contents, CommunicationSubEvents.NOTIFICATION_APPLICANT_INSTITUTION_CHANGE, selectedUser.Organization.TenantID, false);
            return true;
            //return SysXEmailService.SendMail(contents, "Account Linked with New Institution", selectedUser.PrimaryEmailAddress.ToLower(), AppConsts.ApplicantInstitutionChange, AppConsts.Normal);
        }

        public static Boolean SendEmailForEmailChange(OrganizationUser selectedUser, String applicationUrl, String newEmailAddress)
        {
            if (!(applicationUrl.Trim().StartsWith("http://", StringComparison.OrdinalIgnoreCase) || applicationUrl.Trim().StartsWith("https://", StringComparison.OrdinalIgnoreCase)))
            {
                applicationUrl = string.Concat(HttpContext.Current.Request.Url.Scheme, "://", applicationUrl.Trim());
            }
            Dictionary<String, Object> contents = new Dictionary<String, Object>
                                                      {

                                                       {EmailFieldConstants.USER_FULL_NAME,selectedUser.FirstName + " " +selectedUser.LastName},
                                                       {EmailFieldConstants.INSTITUTE_NAME, selectedUser.Organization.Tenant.TenantName},
                                                         {EmailFieldConstants.APPLICATION_URL,applicationUrl},
                                                         {EmailFieldConstants.NEW_EMAIL_ADDRESS,newEmailAddress.ToLower()},
                                                         {EmailFieldConstants.RECEIVER_ORGANIZATION_USER_ID,selectedUser.OrganizationUserID}
                                                      };
            //return SysXEmailService.SendMail(contents, "Confirmation related to your Email Change Request", newEmailAddress.ToLower(), AppConsts.ApplicantEmailAddressChange, AppConsts.Normal);
            PrepareAndSendSystemMail(newEmailAddress.ToLower(), contents, CommunicationSubEvents.NOTIFICATION_APPLICANT_EMAIL_ADDRESS_CHANGE, selectedUser.Organization.TenantID, true);
            return true;

        }

        public static Boolean SendAlertForEmailChange(OrganizationUser selectedUser)
        {
            Dictionary<String, Object> contents = new Dictionary<String, Object>
                                                      {
                                                          {EmailFieldConstants.USER_FULL_NAME,selectedUser.FirstName + " " + selectedUser.LastName},
                                                          {EmailFieldConstants.INSTITUTE_NAME,selectedUser.Organization.Tenant.TenantName}
                                                      };
            PrepareAndSendSystemMail(selectedUser.PrimaryEmailAddress.ToLower(), contents, CommunicationSubEvents.ALERT_APPLICANT_EMAIL_ADDRESS_CHANGE, selectedUser.Organization.TenantID, false);
            return true;
            //return SysXEmailService.SendMail(contents, "Request for changing Primary Email Address", selectedUser.PrimaryEmailAddress.ToLower(), AppConsts.ApplicantAlertEmailAddressChange, AppConsts.Normal);
        }

        public static Boolean SendVerificationCodeForEmailChange(OrganizationUser selectedUser, String verificationCode, String newEmailAddress)
        {
            Dictionary<String, Object> contents = new Dictionary<String, Object>
                                                      {
                                                          {EmailFieldConstants.USER_FULL_NAME,selectedUser.FirstName + " " + selectedUser.LastName},
                                                          {EmailFieldConstants.INSTITUTE_NAME,selectedUser.Organization.Tenant.TenantName},
                                                          {EmailFieldConstants.VERIFICATION_CODE,verificationCode}
                                                      };
            PrepareAndSendSystemMail(newEmailAddress.ToLower(), contents, CommunicationSubEvents.NOTIFICATION_VERIFICATION_CODE_APPLICANT_EMAIL_ADDRESS_CHANGE, selectedUser.Organization.TenantID, false);
            return true;
            //return SysXEmailService.SendMail(contents, "Confirmation related to your Email Change Request", newEmailAddress.ToLower(), AppConsts.VerificationCodeForEmailChange, AppConsts.Normal);
        }

        #endregion

        #endregion

        #region Private Methods





        /// <summary>
        /// // Sends the mail to a user to which these roles are being mapped.
        /// </summary>
        /// <param name="selectedUser">User for which the role is being mapped.</param>
        /// <param name="allAssignedRoleName">List of all roles mapped to the selected user.</param>
        /// <param name="defaultPassword">Default password which is to be send to the user.</param>
        private static void SendEmailForNewlyCreatedUserAccount(OrganizationUser selectedUser, String allAssignedRoleName, String defaultPassword)
        {
            OrganizationUser organizationUser = GetOrganizationUser(selectedUser.OrganizationUserID);
            organizationUser.aspnet_Users.aspnet_Membership.PasswordSalt = SysXMembershipUtil.GenerateSalt();

            organizationUser.aspnet_Users.aspnet_Membership.Password = SysXMembershipUtil.HashPasswordIWithSalt(defaultPassword, organizationUser.aspnet_Users.aspnet_Membership.PasswordSalt);
            organizationUser.aspnet_Users.aspnet_Membership.LastPasswordChangedDate = DateTime.Now;
            organizationUser.IsNewPassword = true;

            UpdateOrganizationUser(organizationUser);

            // Checks if all of the roles are being removed from already assigned role(s) to the user.
            if (allAssignedRoleName.IsNullOrWhiteSpace())
            {
                allAssignedRoleName = SysXUtils.GetMessage(ResourceConst.SECURITY_NO_ROLES_AVAILABLE); // Sets the message in case of no more roles available fore the user.
            }
            var tenant = organizationUser.Organization.Tenant;
            if (tenant.lkpTenantType.TenantTypeCode == TenantType.Compliance_Reviewer.GetStringValue())
            {
                Dictionary<String, Object> dicContents = new Dictionary<String, Object>
                                                      {
                                                          {EmailFieldConstants.USER_FULL_NAME, selectedUser.FirstName + " " + selectedUser.LastName},
                                                          {EmailFieldConstants.USER_NAME,selectedUser.aspnet_Users.LoweredUserName},
                                                          {EmailFieldConstants.ROLE,allAssignedRoleName},
                                                          {EmailFieldConstants.PASSWORD,defaultPassword},
                                                      };
                PrepareAndSendSystemMail(selectedUser.aspnet_Users.aspnet_Membership.LoweredEmail, dicContents, CommunicationSubEvents.NOTIFICATION_ACCOUNT_CREATION_THIRDPARTY_USERS, tenant.TenantID, false);
            }
            else
            {
                String applicationUrl = WebSiteManager.GetInstitutionUrl(tenant.TenantID);
                Dictionary<String, Object> dicContents = new Dictionary<String, Object>
                                                      {
                                                          {EmailFieldConstants.USER_FULL_NAME, selectedUser.FirstName + " " + selectedUser.LastName},
                                                          {EmailFieldConstants.INSTITUTE_NAME,tenant.TenantName},
                                                          {EmailFieldConstants.APPLICATION_URL,applicationUrl},
                                                          {EmailFieldConstants.USER_NAME,selectedUser.aspnet_Users.LoweredUserName},
                                                          {EmailFieldConstants.PASSWORD,defaultPassword},
                                                      };
                PrepareAndSendSystemMail(selectedUser.aspnet_Users.aspnet_Membership.LoweredEmail, dicContents, CommunicationSubEvents.NOTIFICATION_ACCOUNT_CREATION_ADMIN_USERS, tenant.TenantID, false);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="toAddress"></param>
        /// <param name="dicContents"></param>
        /// <param name="communicationSubEvent"></param>
        /// <param name="tenantId"></param>
        /// <param name="showInCommunicationHistory">Added new parameter. On basis of this parameter, entries in table SystemCommunication and SystemCommuniationDelivery will be done.</param>
        public static void PrepareAndSendSystemMail(String toAddress, Dictionary<String, Object> dicContents
                                                    , CommunicationSubEvents communicationSubEvent, Int32? tenantId, Boolean showInCommunicationHistory)
        {
            String defaultCCuserEmailIds = String.Empty;
            String defaultBCCuserEmailIds = String.Empty;
            String ccCode = CopyType.CC.GetStringValue();
            String bccCode = CopyType.BCC.GetStringValue();
            List<GetCommunicationCCusersList> defaultCCuserList = new List<GetCommunicationCCusersList>();
            List<GetCommunicationCCusersList> defaultBCCuserList = new List<GetCommunicationCCusersList>();
            Int32 communicationSubEventId = 0;
            SystemCommunication systemCommunication = CommunicationManager.PrepareMessageContentForSystemMails(communicationSubEvent, dicContents, tenantId ?? 0, out communicationSubEventId);

            if (tenantId != null && tenantId > 0)
            {
                if (tenantId == DefaultTenantID)
                {
                    List<GetCommunicationCCusersList> ccUserList = BALUtils.GetCommunicationRepoInstance().getCommunicationCCusers(communicationSubEventId, tenantId ?? 0);
                    if (!ccUserList.IsNullOrEmpty() && ccUserList.Count > AppConsts.NONE)
                    {
                        defaultCCuserList = ccUserList.Where(condition => condition.CopyCode == ccCode && condition.IsEmail).ToList();
                        defaultCCuserEmailIds = String.Join(";", ccUserList.Where(condition => condition.CopyCode == ccCode && condition.IsEmail).Select(x => x.EmailAddress));

                        defaultBCCuserList = ccUserList.Where(condition => condition.CopyCode == bccCode && condition.IsEmail).ToList();
                        defaultBCCuserEmailIds = String.Join(";", ccUserList.Where(condition => condition.CopyCode == bccCode && condition.IsEmail).Select(x => x.EmailAddress));
                    }
                }
                else
                {
                    List<Entity.ClientEntity.CommunicationCCUsersList> ccUserList = ComplianceSetupManager.GetCCusers(communicationSubEventId, tenantId ?? 0, null);
                    if (!ccUserList.IsNullOrEmpty() && ccUserList.Count > AppConsts.NONE)
                    {
                        ccUserList.Where(condition => condition.CopyCode == ccCode && condition.IsEmail).ToList().ForEach(col =>
                            {
                                defaultCCuserList.Add(new GetCommunicationCCusersList()
                                {
                                    UserName = col.UserName,
                                    UserID = col.UserID,
                                    EmailAddress = col.EmailAddress,
                                    CopyCode = col.CopyCode
                                }
                                );
                            });
                        defaultCCuserEmailIds = String.Join(";", ccUserList.Where(condition => condition.CopyCode == ccCode && condition.IsEmail).Select(x => x.EmailAddress));

                        defaultBCCuserEmailIds = String.Join(";", ccUserList.Where(condition => condition.CopyCode == bccCode && condition.IsEmail).Select(x => x.EmailAddress));
                        ccUserList.Where(condition => condition.CopyCode == bccCode && condition.IsEmail).ForEach(col =>
                        {
                            defaultBCCuserList.Add(new GetCommunicationCCusersList()
                            {
                                UserName = col.UserName,
                                UserID = col.UserID,
                                EmailAddress = col.EmailAddress,
                                CopyCode = col.CopyCode
                            }
                            );
                        });
                    }
                }
            }
            Dictionary<string, string> dicEmailContent = new Dictionary<string, string>{
                    {"EmailBody", systemCommunication.Content},
                    {"CCAddresses", defaultCCuserEmailIds},
                    {"BCCAddresses", defaultBCCuserEmailIds}
                };


            if (showInCommunicationHistory)
            {
                SystemCommunication systemCommunicationObjToSaveInDB = CommunicationManager.PrepareMessageContentForSystemMails(communicationSubEvent, dicContents, tenantId ?? 0, out communicationSubEventId, true); //UAT-4570
                InsertEntriesInCommunicationTables(toAddress, dicContents,
                        defaultCCuserList, defaultBCCuserList, communicationSubEventId, systemCommunicationObjToSaveInDB, true);
            }

            SysXEmailService.SendSystemMail(dicEmailContent, systemCommunication.Subject, toAddress);
        }

        private static Boolean InsertEntriesInCommunicationTables(String toAddress, Dictionary<String, Object> dicContents, List<GetCommunicationCCusersList> defaultCCuserList, List<GetCommunicationCCusersList> defaultBCCuserList, Int32 communicationSubEventId, SystemCommunication systemCommunication, Boolean isSentSuccessfully)
        {
            String ccCode = CopyType.CC.GetStringValue();
            String bccCode = CopyType.BCC.GetStringValue();

            Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
            Int32 backgroundProcessUserId = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.BACKGROUND_PROCESS_USER_VALUE;

            List<Entity.SystemCommunication> lstSystemCommunicationToBeSaved = new List<Entity.SystemCommunication>();

            #region Place Entries in DB for Messaging table for "To" user

            String receiverName = String.Empty;
            if (dicContents.ContainsKey(EmailFieldConstants.USER_FULL_NAME))
            {
                receiverName = dicContents[EmailFieldConstants.USER_FULL_NAME].ToString();
            }

            Int32 receiverOrganizationUserID = AppConsts.NONE;
            if (dicContents.ContainsKey(EmailFieldConstants.RECEIVER_ORGANIZATION_USER_ID))
            {
                receiverOrganizationUserID = Convert.ToInt32(dicContents[EmailFieldConstants.RECEIVER_ORGANIZATION_USER_ID].ToString());
            }



            //a. Create entry in [Messaging] SystemCommunication table 
            //b. Create entry in [Messaging] SystemCommunicationDelivery table 
            Entity.SystemCommunication systemCommunicationToBeInsterted = GetSystetmCommunicationObject(backgroundProcessUserId, communicationSubEventId, systemCommunication);

            Entity.SystemCommunicationDelivery systemCommunicationDelivery = new Entity.SystemCommunicationDelivery();
            systemCommunicationDelivery.ReceiverOrganizationUserID = receiverOrganizationUserID;
            systemCommunicationDelivery.RecieverEmailID = toAddress;
            systemCommunicationDelivery.RecieverName = receiverName;
            systemCommunicationDelivery.IsDispatched = isSentSuccessfully;
            if (!isSentSuccessfully)
            {
                systemCommunicationDelivery.RetryCount = AppConsts.NONE;
                systemCommunicationDelivery.RetryErrorDate = DateTime.Now;
                systemCommunicationDelivery.RetryErrorMessage = "Some error occured while sending mail.";
                systemCommunicationDelivery.DispatchedDate = null;
            }
            else
            {
                systemCommunicationDelivery.DispatchedDate = DateTime.Now;
            }

            systemCommunicationDelivery.IsCC = null;
            systemCommunicationDelivery.IsBCC = null;
            systemCommunicationDelivery.CreatedByID = backgroundProcessUserId;
            systemCommunicationDelivery.CreatedOn = DateTime.Now;
            systemCommunicationToBeInsterted.SystemCommunicationDeliveries.Add(systemCommunicationDelivery);
            lstSystemCommunicationToBeSaved.Add(systemCommunicationToBeInsterted);

            #endregion

            #region Place Entries in DB for Messaging table for "cc" user

            defaultCCuserList = defaultCCuserList.Union(defaultBCCuserList).ToList();

            foreach (GetCommunicationCCusersList ccUser in defaultCCuserList)
            {
                Entity.SystemCommunication systemCommunicationCC = GetSystetmCommunicationObject(backgroundProcessUserId, communicationSubEventId, systemCommunication);

                Entity.SystemCommunicationDelivery systemCommunicationDeliveryCC = new Entity.SystemCommunicationDelivery();
                systemCommunicationDeliveryCC.ReceiverOrganizationUserID = ccUser.UserID;
                systemCommunicationDeliveryCC.RecieverEmailID = ccUser.EmailAddress;
                systemCommunicationDeliveryCC.RecieverName = ccUser.UserName;
                systemCommunicationDeliveryCC.IsDispatched = isSentSuccessfully;
                if (!isSentSuccessfully)
                {
                    systemCommunicationDeliveryCC.RetryCount = AppConsts.NONE;
                    systemCommunicationDeliveryCC.RetryErrorMessage = "Some error occured while sending e-mail.";
                    systemCommunicationDeliveryCC.DispatchedDate = null;
                }
                else
                {
                    systemCommunicationDeliveryCC.DispatchedDate = DateTime.Now;
                }

                systemCommunicationDeliveryCC.IsCC = ccUser.CopyCode == ccCode;
                systemCommunicationDeliveryCC.IsBCC = ccUser.CopyCode == bccCode;
                systemCommunicationDeliveryCC.CreatedByID = backgroundProcessUserId;
                systemCommunicationDeliveryCC.CreatedOn = DateTime.Now;
                systemCommunicationCC.SystemCommunicationDeliveries.Add(systemCommunicationDeliveryCC);
                lstSystemCommunicationToBeSaved.Add(systemCommunicationCC);
            }

            #endregion

            return BALUtils.GetCommunicationRepoInstance().SaveSysCommunicationAndSysDeliveries(lstSystemCommunicationToBeSaved);
        }

        private static SystemCommunication GetSystetmCommunicationObject(Int32 backgroundProcessUserId, Int32 communicationSubEventId, SystemCommunication systemCommunication)
        {
            Entity.SystemCommunication systemCommunicationToBeInsterted = new Entity.SystemCommunication();
            systemCommunicationToBeInsterted.SenderName = ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SENDER_NAME];
            systemCommunicationToBeInsterted.SenderEmailID = ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SENDER_EMAIL_ID];
            systemCommunicationToBeInsterted.Subject = systemCommunication.Subject;
            systemCommunicationToBeInsterted.CommunicationSubEventID = communicationSubEventId;
            systemCommunicationToBeInsterted.CreatedByID = backgroundProcessUserId;
            systemCommunicationToBeInsterted.CreatedOn = DateTime.Now;
            systemCommunicationToBeInsterted.Content = systemCommunication.Content;
            return systemCommunicationToBeInsterted;
        }

        private static void SendEmailWhileChangeInAssignedRoles(OrganizationUser selectedUser, String allAssignedRoleName)
        {
            // Checks if all of the roles are being removed from already assigned role(s) to the user.
            if (allAssignedRoleName.IsNullOrWhiteSpace())
            {
                allAssignedRoleName = SysXUtils.GetMessage(ResourceConst.SECURITY_NO_ROLES_AVAILABLE); // Sets the message in case of no more roles available fore the user.
            }
            Dictionary<String, Object> contents = new Dictionary<String, Object>
                                                      {
                                                           {EmailFieldConstants.USER_FULL_NAME, selectedUser.FirstName + " " + selectedUser.LastName},
                                                           {EmailFieldConstants.ALL_ASSIGNED_ROLE_NAME,allAssignedRoleName},
                                                           {EmailFieldConstants.RECEIVER_ORGANIZATION_USER_ID,selectedUser.OrganizationUserID}
                                                         };
            PrepareAndSendSystemMail(selectedUser.aspnet_Users.aspnet_Membership.LoweredEmail, contents, CommunicationSubEvents.NOTIFICATION_ROLE_UPDATE, selectedUser.Organization.TenantID, true);
            //SysXEmailService.SendMail(contents, "Role changed - for " + selectedUser.FirstName + " " + selectedUser.LastName, selectedUser.aspnet_Users.aspnet_Membership.LoweredEmail, AppConsts.RoleUpdate, AppConsts.Normal);
        }

        #endregion

        #endregion

        #region Address
        #region City

        /// <summary>
        /// Get State Cities based on zipCodeNumber.
        /// </summary>
        /// <param name="zipCodeNumber">zipCodeNumber as string</param>
        /// <returns>City,State based on zipCodeNumber</returns>
        public static List<ZipCode> GetCityState(String zipCodeNumber)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetCityState(zipCodeNumber);
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


        /// <summary>
        /// Get State Cities based on zipCodeNumber.
        /// </summary>
        /// <param name="zipCodeNumber">zipCodeNumber as string</param>
        /// <returns>City,State based on zipCodeNumber</returns>
        public static List<ZipCode> GetCityStateByID(Int32 zipCodeId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetCityState(zipCodeId);
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

        public static ZipCode GetApplicantZipCodeDetails(Int32 zipCodeId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetApplicantZipCodeDetails(zipCodeId);
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

        #endregion

        #region User Order placement Related Functionality

        /// <summary>
        /// Called from ComplianceDataManager, to update the User details in master, while placing an order
        /// </summary>
        /// <param name="orgUserMaster"></param>
        /// <param name="dicAddressData"></param>
        /// <returns></returns>
        public static void UpdateApplicanDetailsMaster(OrganizationUser orgUserMaster, Dictionary<String, Object> dicAddressData, out Int32 addressMasterId, List<PreviousAddressContract> lstPrevAddress, PreviousAddressContract mailingAddress, out List<ResidentialHistory> lstResendentialHistory, ref List<PersonAlia> lstPersonAlias, Boolean isLocationServiceTenant)
        {
            BALUtils.GetSecurityRepoInstance().UpdateApplicanDetailsMaster(orgUserMaster, dicAddressData, out addressMasterId, lstPrevAddress, mailingAddress, out lstResendentialHistory, ref lstPersonAlias, isLocationServiceTenant);
        }

        public static void SaveApplicantOrderProcessMaster(OrganizationUserProfile orgProfileMaster, Dictionary<String, Object> dicAddressData, out Int32 profileIdMaster, out Int32 addressIdMaster, out Guid addressHandlerId, List<PreviousAddressContract> lstPrevAddress, out List<ResidentialHistoryProfile> lstResidentialHistoryProfile, ref List<PersonAliasProfile> lstPersonAliasProfile, Boolean isLocationServiceTenant)
        {
            BALUtils.GetSecurityRepoInstance().SaveApplicantOrderProcessMaster(orgProfileMaster, dicAddressData, out profileIdMaster, out addressIdMaster, out addressHandlerId, lstPrevAddress, out lstResidentialHistoryProfile, ref lstPersonAliasProfile, isLocationServiceTenant);
        }

        public static List<PaymentIntegrationSetting> GetPaymentIntegrationSettingsByName(String name)
        {
            return BALUtils.GetSecurityRepoInstance().GetPaymentIntegrationSettingsByName(name);
        }

        public static List<PaymentIntegrationSettingClientMapping> GetPaymentIntegrationSettingsClientMappings()
        {
            return BALUtils.GetSecurityRepoInstance().GetPaymentIntegrationSettingsClientMappings();
        }

        #endregion

        #region UserProgram
        //public static Boolean CopyOrganizationUserProgram(List<OrganizationUserProgram> organizationUserProgram)
        //{
        //    try
        //    {
        //        return BALUtils.GetSecurityRepoInstance().CopyOrganizationUserProgram(organizationUserProgram);
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //}

        public static Boolean UpdateChanges()
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().UpdateChanges();
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

        //public static List<OrganizationUserProgram> GetAllUserProgram(Int32 organizationUserId)
        //{
        //    try
        //    {
        //        return BALUtils.GetSecurityRepoInstance().GetAllUserProgram(organizationUserId);
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //}
        //public static Boolean UpdateUserProgram(List<OrganizationUserProgram> lstOtganizationUserProgram, Int32 organizationUserId)
        //{
        //    try
        //    {
        //        return BALUtils.GetSecurityRepoInstance().UpdateUserProgram(lstOtganizationUserProgram, organizationUserId);
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //}
        #endregion

        #region Client DB Configuiration

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static Boolean GetTenantConnectionStringByConnectionStringAndUserId(String connectionString, Int32? tenantId)
        {
            return BALUtils.GetSecurityRepoInstance().GetTenantConnectionStringByConnectionStringAndUserId(connectionString, tenantId);
        }

        /// <summary>
        /// Retrieves all Client DB Configuration
        /// </summary>
        /// <returns>
        /// </returns>
        public static List<ClientDBConfiguration> GetClientDBConfiguration()
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetClientDBConfiguration();
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

        /// <summary>
        /// Retrieves all App Configuration
        /// </summary>
        /// <returns>
        /// </returns>
        public static AppConfiguration GetAppConfiguration(String key)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetAppConfiguration(key);
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
        //UAT-2319
        public static void UpdateAppConfiguration(String key, String UpdatedValue)
        {
            try
            {
                BALUtils.GetSecurityRepoInstance().UpdateAppConfiguration(key, UpdatedValue);
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

        #region Edit Profile
        public static void AddAddressHandle(Guid addressHandleID)
        {
            BALUtils.GetSecurityRepoInstance().AddAddressHandle(addressHandleID);
        }
        public static void AddAddress(Dictionary<String, Object> dicAddressData, Guid addressHandleId, Int32 currentUserId, Address addressNew, AddressExt addressExtNew = null)
        {
            BALUtils.GetSecurityRepoInstance().AddAddress(dicAddressData, addressHandleId, currentUserId, addressNew, addressExtNew);
        }
        public static Int16 GetAuthRequestTypeIdByCode(String authReqType)
        {
            return BALUtils.GetSecurityRepoInstance().GetAuthRequestTypeIdByCode(authReqType);
        }
        public static UserAuthRequest GenerateEmailConfirmationReq(Int32 orgUserId, Int32? orgUserProfileId, String newEmailAddress, Boolean updateSecurityEmail, Int32 loggedInUserId, Int16 authReqType)
        {
            try
            {

                //  var authReqType = BALUtils.GetSecurityRepoInstance().GetAuthRequestTypeIdByCode(AuthRequestType.Email_Confirmation.GetStringValue());
                BALUtils.GetSecurityRepoInstance().CancelPreviousAuthRequest(orgUserId, authReqType, loggedInUserId);
                UserAuthRequest userAuthRequest = new UserAuthRequest();
                userAuthRequest.UAR_OrganizationUserID = orgUserId;
                userAuthRequest.UAR_OrganizationUserProfileID = orgUserProfileId;
                userAuthRequest.UAR_NewValue = newEmailAddress;
                userAuthRequest.UAR_SecurityEmailUpdateChecked = updateSecurityEmail;
                userAuthRequest.UAR_VerificationCode = Guid.NewGuid().ToString();
                userAuthRequest.UAR_IsActive = true;
                userAuthRequest.UAR_AuthRequestTypeID = authReqType;
                BALUtils.GetSecurityRepoInstance().AddUserAuthRequest(userAuthRequest, loggedInUserId);
                return userAuthRequest;
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

        public static Boolean ChangeUserEmailAddressAfterConfirmation(String verificationCode)
        {
            try
            {
                UserAuthRequest userAuthRequest = BALUtils.GetSecurityRepoInstance().GetUserAuthRequestByVerCode(verificationCode.Trim());
                if (userAuthRequest.IsNotNull() && userAuthRequest.lkpAuthRequestType.Code == AuthRequestType.Email_Confirmation.GetStringValue())
                {
                    OrganizationUser organizationUser = GetOrganizationUser(userAuthRequest.UAR_OrganizationUserID);
                    if (organizationUser.IsNotNull())
                    {
                        if (userAuthRequest.UAR_SecurityEmailUpdateChecked == true)
                        {
                            organizationUser.aspnet_Users.aspnet_Membership.Email = userAuthRequest.UAR_NewValue;
                            organizationUser.aspnet_Users.aspnet_Membership.LoweredEmail = userAuthRequest.UAR_NewValue.ToLower();
                        }
                        organizationUser.PrimaryEmailAddress = userAuthRequest.UAR_NewValue;
                        AddOrganizationUserProfile(organizationUser);
                        if (UpdateOrganizationUser(organizationUser))
                        {
                            userAuthRequest.UAR_IsActive = false;
                            BALUtils.GetSecurityRepoInstance().UpdateUserAuthRequest(userAuthRequest, organizationUser.OrganizationUserID);
                            return true;
                        }
                    }
                }
                // UAT 3102: As an ADB Admin, I should be able to update agency user email addresses in Manage Agency Users.
                else if (userAuthRequest.IsNotNull() && userAuthRequest.lkpAuthRequestType.Code == AuthRequestType.Email_Updation_For_SharedUser.GetStringValue())
                {
                    OrganizationUser OrganizationUser = GetOganisationUsersByUserID(userAuthRequest.UAR_OrganizationUserID).FirstOrDefault();
                    if (OrganizationUser != null)
                    {
                        Entity.SharedDataEntity.AgencyUser agencyUser = ProfileSharingManager.GetAgencyUserByUserID(OrganizationUser.UserID.ToString());
                        if (agencyUser != null)
                        {
                            if (ProfileSharingManager.UpdateAgencyUserEmailAddress(agencyUser.AGU_ID, userAuthRequest.UAR_NewValue, agencyUser.AGU_ID))
                            {
                                userAuthRequest.UAR_IsActive = false;
                                BALUtils.GetSecurityRepoInstance().UpdateUserAuthRequest(userAuthRequest, userAuthRequest.UAR_OrganizationUserID);
                                return true;
                            }
                        }
                    }
                }
                return false;
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

        public static UserAuthRequest GetUserAuthRequestByEmail(String emailAddress)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetUserAuthRequestByEmail(emailAddress.Trim());
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

        public static List<OrganizationUser> getOrganizationUserByIdList(List<Int32?> userIds)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().getOrganizationUserByIdList(userIds);
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

        public static List<OrganizationUser> GetOrganizationUserByIds(List<Int32> lstOrgUserId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetOrganizationUserByIds(lstOrgUserId);
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

        public static void SaveModifyShippingData(ApplicantOrderCart applicantOrderCart, Boolean isLocationServiceTenant, Int32 orgUserID)
        {
            SecurityManager.UpdateMailingAddress(applicantOrderCart.MailingAddress, isLocationServiceTenant, orgUserID);
            BALUtils.GetComplianceDataRepoInstance(applicantOrderCart.TenantId).UpdateMailingAddress(applicantOrderCart.MailingAddress, isLocationServiceTenant, orgUserID);
            BALUtils.GetFingerPrintClientRepoInstance(applicantOrderCart.TenantId).UpdateApplicantAppointmentExt(applicantOrderCart.OrderId, applicantOrderCart.MailingAddress, isLocationServiceTenant, orgUserID, applicantOrderCart.IsPaymentReqInMdfyShpng, applicantOrderCart.MailingPrice);
            //BALUtils.GetComplianceDataRepoInstance(applicantOrderCart.TenantId).UpdateOrderPayment(applicantOrderCart, orgUserID);
        }
        public static void UpdateOrderPayment(ApplicantOrderCart applicantOrderCart, Boolean isLocationServiceTenant, Int32 orgUserID)
        {
            BALUtils.GetComplianceDataRepoInstance(applicantOrderCart.TenantId).UpdateOrderPayment(applicantOrderCart, orgUserID);
        }

        public static void UpdateApplicantAppointmentDetailExt(ApplicantOrderCart applicantOrderCart, Boolean isLocationServiceTenant, Int32 orgUserID)
        {
            BALUtils.GetFingerPrintClientRepoInstance(applicantOrderCart.TenantId).UpdateApplicantAppointmentDetailExt(applicantOrderCart.OrderId, applicantOrderCart.MailingAddress, isLocationServiceTenant, orgUserID);
        }

        public static Guid UpdateMailingAddress(PreviousAddressContract mailingAddress, Boolean isLocationServiceTenant, Int32 orgUserID)
        {
            Guid mailingaddressHandleId = BALUtils.GetSecurityRepoInstance().UpdateMailingAddress(mailingAddress, isLocationServiceTenant, orgUserID);
            return mailingaddressHandleId;
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
        public static List<LookupContract> GetExistingUserLists(String userName, DateTime dOB, String sSN, String firstName, String lastName, Boolean isApplicant = false, String email = null, Boolean isSharedUser = false, String languageCode = default(String))
        {
            try
            {
                if (languageCode.IsNullOrEmpty())
                {
                    languageCode = Languages.ENGLISH.GetStringValue();
                }
                return BALUtils.GetSecurityRepoInstance().GetExistingUserLists(userName, dOB, sSN, firstName, lastName, DefaultTenantID, email, isApplicant, isSharedUser, languageCode);
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

        /// <summary>
        /// Finds if the username is already present in tenant database
        /// </summary>
        /// <param name="userName">User Name</param>
        /// <param name="tenantId">Tenant Id</param>
        /// <returns>True if username exists</returns>
        public static Boolean IsUsernameExistInTenantDB(String userName, Int32 tenantId)
        {
            try
            {

                return BALUtils.GetSecurityRepoInstance().IsUsernameExistInTenantDB(userName.Trim().ToLower(), tenantId);
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

        #region Document

        public static Boolean CheckDocumentDeletionAllowed(Int32 applicantUploadedDocumentID, Int32 tenantID)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().CheckDocumentDeletionAllowed(applicantUploadedDocumentID, tenantID);
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

        public static void SynchoniseUserDocuments(Int32 organisationUserId, Int32 tenantID, Int32 currentLoggedInUseID)
        {
            try
            {
                BALUtils.GetSecurityRepoInstance().SynchronizeApplicantDocument(organisationUserId, tenantID, currentLoggedInUseID);
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

        #region User profile Synchronization

        public static Boolean SynchoniseUserProfile(Int32 organisationUserId, Int32 tenantID, Int32 orgUsrID)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().SynchoniseUserProfile(organisationUserId, tenantID, orgUsrID);
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

        #region System Service

        /// <summary>
        /// Get Active System Service Triggers
        /// </summary>
        /// <returns>List of System Service Trigger</returns>
        public static List<SystemServiceTrigger> GetSystemServiceTriggers()
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetSystemServiceTriggers();
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

        /// <summary>
        /// Get System Service Triggers By Id
        /// </summary>
        /// <returns>System Service Trigger</returns>
        public static SystemServiceTrigger GetSystemServiceTriggerByID(Int32 id)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetSystemServiceTriggerByID(id);
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

        /// <summary>
        /// Get System Service Triggers By SystemServiceId
        /// </summary>
        /// <returns>System Service Trigger</returns>
        public static SystemServiceTrigger GetSystemServiceTriggerBySystemServiceID(Int16 systemServiceID, Int32 tenantID)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetSystemServiceTriggerBySystemServiceID(systemServiceID, tenantID);
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

        /// <summary>
        /// Get System Service LookUp
        /// </summary>
        /// <returns>List of System Service LookUp</returns>
        public static List<lkpSystemService> GetSystemServiceLookUp()
        {
            try
            {
                return LookupManager.GetLookUpData<Entity.lkpSystemService>().Where(x => x.SS_IsDeleted == false).ToList();
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

        /// <summary>
        /// Get System Service by Code
        /// </summary>
        /// <param name="code"></param>
        /// <returns>System Service</returns>
        public static lkpSystemService GetSystemServiceByCode(String code)
        {
            try
            {
                return LookupManager.GetLookUpData<Entity.lkpSystemService>().Where(x => x.SS_Code.Equals(code) && x.SS_IsDeleted == false).FirstOrDefault();
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

        /// <summary>
        /// Add System Service Trigger
        /// </summary>
        /// <param name="systemServiceTrigger"></param>
        /// <returns>true/false</returns>
        public static Boolean AddSystemServiceTrigger(SystemServiceTrigger systemServiceTrigger)
        {
            try
            {
                Int16 sysServiceID = systemServiceTrigger.SST_SystemServiceID.HasValue ? systemServiceTrigger.SST_SystemServiceID.Value : Convert.ToInt16(0);
                Int32 tenantID = systemServiceTrigger.SST_TenantID.HasValue ? systemServiceTrigger.SST_TenantID.Value : Convert.ToInt32(0);
                SystemServiceTrigger sysSerTrigger = GetSystemServiceTriggerBySystemServiceID(sysServiceID, tenantID);
                if (sysSerTrigger.IsNull())
                {
                    return BALUtils.GetSecurityRepoInstance().AddSystemServiceTrigger(systemServiceTrigger);
                }
                else
                {
                    return true;
                }
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

        /// <summary>
        /// Update System Service Trigger
        /// </summary>
        /// <param name="systemServiceTrigger"></param>
        /// <returns>true/false</returns>
        public static Boolean UpdateSystemServiceTrigger(SystemServiceTrigger systemServiceTrigger)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().UpdateSystemServiceTrigger(systemServiceTrigger);
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

        #region Temporary files for pdf conversion
        public static Boolean SavePageHtmlContentLocation(TempFile tempFile)
        {
            try
            {

                return BALUtils.GetSecurityRepoInstance().SavePageHtmlContentLocation(tempFile);
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

        public static List<TempFile> GetFilePath(Guid Id)
        {
            try
            {

                return BALUtils.GetSecurityRepoInstance().GetFilePath(Id);
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

        public static Boolean DeleteTempFile(Guid Id, Int32 CurrentLoggedInUserID)
        {
            try
            {

                return BALUtils.GetSecurityRepoInstance().DeleteTempFile(Id, CurrentLoggedInUserID);
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

        /// <summary>
        /// Gets the list of client admin users working in the organization associated with the given tenant Id. 
        /// </summary>
        /// <param name="tenantId">Tenant Id</param>
        /// <returns>List of active Users</returns>
        public static List<OrganizationUser> GetClientAdminUsersByTanentId(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetClientAdminUsersByTanentId(tenantId).ToList();
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

        public static List<FeatureAction> GetFeatureAction(Int32 productFeatureID, Int32 rolePermissionID)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetFeatureAction(productFeatureID, rolePermissionID);
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

        /// <summary>
        /// Gets the list of all business channel types. 
        /// </summary>
        /// <param name="tenantId">Tenant Id</param>
        /// <returns>List of active Users</returns>
        public static List<lkpBusinessChannelType> GetBusinessChannelTypes()
        {
            try
            {
                return LookupManager.GetBusinessChannelTypes();
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
        /// Gets the list of all business channel types. 
        /// </summary>
        /// <param name="tenantId">Tenant Id</param>
        /// <returns>List of active Users</returns>
        public static List<FeatureRoleAction> GetRoleActionFeatures(Int32 featureID, Int32 userID, Int32 sysXBlockID)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetRoleActionFeatures(featureID, userID, sysXBlockID);
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


        public static List<FeatureRoleAction> GetRoleActionFeaturesTemp(Int32 featureID, Int32 userID, Int32 sysXBlockID)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetRoleActionFeaturesTemp(featureID, userID, sysXBlockID);
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

        #region FeatureActionT

        public static List<FeatureActionContract> GetFeatureActionT(Int32 productFeatureID)
        {
            try
            {
                DataTable featureAction = BALUtils.GetSecurityRepoInstance().GetFeatureActionT(productFeatureID);
                return SetDataForTree(featureAction);
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



        private static List<FeatureActionContract> SetDataForTree(DataTable table)
        {
            try
            {
                IEnumerable<DataRow> rows = table.AsEnumerable();
                return rows.Select(x => new FeatureActionContract
                {
                    ParentId = Convert.ToString(x["ParentNodeId"]),
                    NodeId = Convert.ToString(x["NodeId"]),
                    Name = Convert.ToString(x["Name"]),
                    Code = Convert.ToString(x["Code"]),
                    //Level = Convert.ToInt32(Convert.ToString("Levels")),
                }).ToList();
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

        public static List<Permission> GetListOFPermissions()
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetListOFPermissions();

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        #endregion

        public static List<ManageBuisnessChannelTypeContract> GetBuisnessChannelTypeByTenantId(Int32 tenantID)
        {
            try
            {
                Dictionary<String, Int32> buisnessChannelTypeList = new Dictionary<String, Int32>();
                var table = BALUtils.GetSecurityRepoInstance().GetBuisnessChannelTypeByTenantId(tenantID);
                IEnumerable<DataRow> rows = table.AsEnumerable();
                return rows.Select(x => new ManageBuisnessChannelTypeContract
                {
                    BuisnessChannelTypeId = Convert.ToInt32(Convert.ToString(x["BuisnessChannelTypeId"])),
                    BuisnessChannelTypeCode = Convert.ToString(x["BuisnessChannelTypeCode"]),
                }).ToList();
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
        /// Get Verification detail data from multiple institutions
        /// </summary>
        /// <param name="inuputXml">inuputXml</param>
        /// <param name="gridCustomPaging">gridCustomPaging</param>
        /// <returns>List<MultiInstitutionAssignmentDataContract></returns>
        public static List<MultiInstitutionAssignmentDataContract> GetMultiInstitutionAssignmentData(String inuputXml, CustomPagingArgsContract gridCustomPaging)
        {
            try
            {
                List<MultiInstitutionAssignmentDataContract> objectContract = new List<MultiInstitutionAssignmentDataContract>();
                DataTable dataForQueue = BALUtils.GetSecurityRepoInstance().GetMultiInstitutionAssignmentData(inuputXml, gridCustomPaging);
                IEnumerable<DataRow> rows = dataForQueue.AsEnumerable();

                return rows.Select(x => new MultiInstitutionAssignmentDataContract
                {
                    ApplicantName = Convert.ToString(x["ApplicantName"]),
                    ItemName = Convert.ToString(x["ItemName"]),
                    CategoryName = Convert.ToString(x["CategoryName"]),
                    PackageName = Convert.ToString(x["PackageName"]),
                    SubmissionDate = x["SubmissionDate"].GetType().Name == "DBNull" ? null : (DateTime?)x["SubmissionDate"],
                    VerificationStatus = Convert.ToString(x["VerificationStatus"]),
                    SystemStatus = Convert.ToString(x["SystemStatus"]),
                    RushOrderStatus = Convert.ToString(x["RushOrderStatus"]),
                    AssignedUserName = Convert.ToString(x["AssignedUserName"]).Trim(),
                    ReviewLevel = Convert.ToString(x["ReviewLevel"]),
                    //CustomAttributes = Convert.ToString(x["CustomAttributes"]),  UAT 3212
                    ApplicantId = Convert.ToInt32(x["ApplicantId"]),
                    ApplicantComplianceItemId = Convert.ToInt32(x["ApplicantComplianceItemId"]),
                    ApplicantComplianceCategoryId = Convert.ToInt32(x["ApplicantComplianceCategoryId"]),
                    TenantID = Convert.ToInt32(x["TenantID"]),
                    AdminNote = Convert.ToString(x["AdminNote"]),
                    IsDirty = Convert.ToBoolean(x["IsDirty"]),
                    CategoryId = Convert.ToInt32(x["CategoryId"]),
                    ComplianceItemId = Convert.ToInt32(x["ComplianceItemId"]),
                    VerificationStatusCode = Convert.ToString(x["VerificationStatusCode"]),
                    FVDId = Convert.ToInt32(x["FVDId"]),
                    InstitutionName = Convert.ToString(x["InstitutionName"]),
                    IsUiRulesViolate = Convert.ToBoolean(x["IsUiRulesViolate"]),
                    //TotalCount = Convert.ToInt32(x["TotalCount"]), UAT 3212

                }).ToList();
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

        #region UAT-259: CLIENT SEARCH SCREEN
        //public static List<ClientUserSearchContract> GetClientUserSearchData(String searchType, String tenantIDList, String agencyIDList, ClientUserSearchContract clientUserSearchContract, CustomPagingArgsContract customPagingArgsContract)
        //{
        //    try
        //    {
        //        return BALUtils.GetSecurityRepoInstance().GetClientUserSearchData(searchType, tenantIDList, agencyIDList, clientUserSearchContract, customPagingArgsContract);
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //}
        #endregion
        //UAT-4257
        public static List<ClientUserSearchContract> GetClientUserSearchData(String searchType, String tenantIDList, String HierarchyNode, String agencyRootNodeIDList, String SelectedAgecnyIds, ClientUserSearchContract clientUserSearchContract, CustomPagingArgsContract customPagingArgsContract)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetClientUserSearchData(searchType, tenantIDList, HierarchyNode, agencyRootNodeIDList, SelectedAgecnyIds, clientUserSearchContract, customPagingArgsContract);
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

        #region DataFeedFormatter
        private static Logger logger = LogManager.GetCurrentClassLogger();
        #region Manage System Entity Permission

        public static List<LkpSystemEntity> GetSystemEntities()
        {
            try
            {
                return LookupManager.GetLookUpData<LkpSystemEntity>().Where(x => !x.SE_IsDeleted).ToList();
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

        public static List<SystemEntityUserPermissionData> GetSystemEntityUserPermissionList(SearchItemDataContract searchDataContract, CustomPagingArgsContract gridCustomPaging, Int32 selectedTenantID, Int32 selectedEntityId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetSystemEntityUserPermissionList(searchDataContract, gridCustomPaging, selectedTenantID, selectedEntityId);
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

        public static List<OrgUser> GetOrgUserListForAsigningPermission(Int32 currentUserId, Int32 entityId, Int32 selectedTenantId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetOrgUserListForAsigningPermission(currentUserId, entityId, selectedTenantId);
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

        public static List<SystemEntityPermission> GetPermissionByEntityId(Int32 entityId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetPermissionByEntityId(entityId);
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

        public static Boolean SaveUpdateEntityUserPermission(SystemEntityUserPermission systemEntityUserPermission, Int32 userId, Dictionary<int, bool> lstSelectedBkgOdrResPermissions)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().SaveUpdateEntityUserPermission(systemEntityUserPermission, userId, lstSelectedBkgOdrResPermissions);
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

        public static Boolean DeleteEntityUserPermission(Int32 systemEntityUserPermissionId, Int32 userId, Int32 organisationUserId, List<Int32> lstEntityPermissionIds, Int32? dpmId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().DeleteEntityUserPermission(systemEntityUserPermissionId, userId, organisationUserId, lstEntityPermissionIds, dpmId);
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

        //#region DataFeedFormatter

        public static List<Tenant> GetTenantList()
        {
            try
            {
                return BALUtils.GetSecurityInstance().GetListOfTenant();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Boolean IsTenantActive(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetSecurityInstance().IsTenantActive(tenantId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get Data Feed Setting by tenantID
        /// </summary>
        /// <param name="tenantId">tenantId</param>
        /// <returns>DataFeedSettingContract</returns>
        public static List<DataFeedSettingContract> GetDataFeedSetting(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetSecurityInstance().GetDataFeedSetting(tenantId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        ///// <summary>
        /// Save Exported Document Details
        /// </summary>
        /// <param name="exportedDoc"></param>
        /// <returns></returns>
        public static DataFeedSettingContract SetDataFeedInvokeHistoryData(DataFeedSettingContract dataObject)
        {
            try
            {
                DataFeedSettingContract tempDataFeedInvokeHistoryObj = BALUtils.GetSecurityInstance().GetDataFeedInvokeHistoryData(dataObject);
                return tempDataFeedInvokeHistoryObj;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static Boolean SaveLogInInvokeHistory(DataFeedSettingContract settingObject, List<DataFeedInvokeHistoryDetail> lstdataFeedInvokeHistoryDetail,
            Int32 utilityUserId, Int32 dataFeedInvokeResultID)
        {
            try
            {
                StringBuilder parameterXml = new StringBuilder();
                parameterXml.Append("<Parameters>");
                parameterXml.Append("<SettingID>" + settingObject.DataFeedSettingId + "</SettingID>");
                parameterXml.Append("<TenantID>" + settingObject.TenantID + "</TenantID>");
                parameterXml.Append("<RecordOriginStartDate>" + settingObject.RecordOriginStartDate + "</RecordOriginStartDate>");
                parameterXml.Append("<RecordOriginEndDate>" + settingObject.RecordOriginEnddate + "</RecordOriginEndDate>");
                parameterXml.Append("<AccessKey>" + settingObject.AccessKey.ToString() + "</AccessKey>");
                parameterXml.Append("</Parameters>");

                return BALUtils.GetSecurityInstance().SaveLogInInvokeHistory(parameterXml.ToString(), lstdataFeedInvokeHistoryDetail, settingObject.DataFeedSettingId,
                                                                            utilityUserId, dataFeedInvokeResultID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataFeedFTPDetail GetDataFeedFTPDetail(Int32 dataFeedFTPDetailID)
        {
            try
            {
                return BALUtils.GetSecurityInstance().GetDataFeedFTPDetail(dataFeedFTPDetailID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region UAT-806 Creation of granular permissions for Client Admin users

        public static Boolean GetUserGranularPermission(Int32 organizationUserId, out Dictionary<String, String> dicPermissionList)
        {
            try
            {
                //Get system entity for DOB and SSN number for UAT-806
                //List<Int32> lstSystemEntityIds = LookupManager.GetLookUpData<LkpSystemEntity>().Where(cond => (cond.SE_CODE == "AAAB" || cond.SE_CODE == "AAAA" || cond.SE_CODE == "AAAC" || cond.SE_CODE == "AAAD") && cond.SE_IsDeleted == false).Select(slct => slct.SE_ID).ToList();
                //return BALUtils.GetSecurityInstance().GetUserGranularPermission(organizationUserId, lstSystemEntityIds, out dicPermissionList);
                return BALUtils.GetSecurityInstance().GetUserGranularPermission(organizationUserId, out dicPermissionList);
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

        #region User Last Login Activity.
        public static UserLoginHistory AddUserLoginActivity(Int32 organizationUserId, String currentSessionId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().AddUserLoginActivity(organizationUserId, currentSessionId);
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

        public static Boolean UpdateUserLoginActivity(Int32 organizationUserId, String currentSessionId, Boolean isSessionTimeout, Int32 userLoginHistoryID)
        {
            try
            {
                return BALUtils.GetSecurityInstance().UpdateUserLoginActivity(organizationUserId, currentSessionId, isSessionTimeout, userLoginHistoryID);
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

        public static List<UserLoginHistory> GetApplicantLastLoginDetail(Int32 organizationUserId, String currentSessionId)
        {
            try
            {
                return BALUtils.GetSecurityInstance().GetApplicantLastLoginDetail(organizationUserId, currentSessionId);
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

        /// <summary>
        /// To save service logging data
        /// </summary>
        /// <param name="serviceLoggingContract"></param>
        /// <returns></returns>
        public static Boolean SaveServiceLoggingDetail(ServiceLoggingContract serviceLoggingContract)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().SaveServiceLoggingDetail(serviceLoggingContract);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
            }
            return false;
        }

        #region UAT-1049: Data Entry Enhanchment
        /// <summary>
        /// Get Verification detail data from multiple institutions
        /// </summary>
        /// <param name="inuputXml">inuputXml</param>
        /// <param name="gridCustomPaging">gridCustomPaging</param>
        /// <returns>List<MultiInstitutionAssignmentDataContract></returns>
        public static List<DataEntryQueueContract> GetDataEntryQueueData(String inuputXml, CustomPagingArgsContract gridCustomPaging, Int32? CurrentLoggedInUserID, String institutionHierarchyIds, Boolean? IsSingleTenant = false, Int32? tenantId = AppConsts.NONE)
        {
            try
            {
                DataTable dataForQueue = new DataTable();
                if (IsSingleTenant.Value)
                    dataForQueue = ComplianceDataManager.GetDataEntryQueueDataForSingleTenant(gridCustomPaging, CurrentLoggedInUserID, tenantId.Value, institutionHierarchyIds);
                else
                    dataForQueue = BALUtils.GetSecurityRepoInstance().GetDataEntryQueueData(inuputXml, gridCustomPaging, CurrentLoggedInUserID);

                IEnumerable<DataRow> rows = dataForQueue.AsEnumerable();

                return rows.Select(x => new DataEntryQueueContract
                {
                    ApplicantName = Convert.ToString(x["ApplicantName"]),
                    DocumentName = Convert.ToString(x["DocumentName"]),
                    DocumentStatusName = Convert.ToString(x["DocumentStatusName"]),
                    DocumentStatusCode = Convert.ToString(x["DocumentStatusCode"]),
                    DateUploaded = x["DateUploaded"].GetType().Name == "DBNull" ? null : (DateTime?)x["DateUploaded"],
                    DateUploadDatePart = x["DateUploadDatePart"].GetType().Name == "DBNull" ? null : (DateTime?)x["DateUploadDatePart"],
                    DocumentStatusID = Convert.ToInt32(x["DocumentStatusID"]),
                    ApplicantDocumentID = Convert.ToInt32(x["ApplicantDocumentID"]),
                    TenantID = Convert.ToInt32(x["TenantID"]),
                    AssignToUserName = Convert.ToString(x["AssignToUserName"]),
                    ApplicantOrganizationUserID = Convert.ToInt32(x["ApplicantOrganizationUserID"]),
                    FDEQ_ID = Convert.ToInt32(x["FDEQ_ID"]),
                    AssignToUserID = x["AssignToUserID"].GetType().Name == "DBNull" ? AppConsts.NONE : Convert.ToInt32(x["AssignToUserID"]),
                    //UAT-2456
                    DiscardDocumentCount = x["DiscardDocumentCount"].GetType().Name == "DBNull" ? AppConsts.NONE : Convert.ToInt32(x["DiscardDocumentCount"]),
                    TotalCount = Convert.ToInt32(x["TotalCount"]),
                    //UAT-2499
                    TenantName = Convert.ToString(x["TenantName"]),

                }).ToList();
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
        /// Get next record for data entry screen.
        /// </summary>
        /// <param name="inuputXml">inuputXml</param>
        /// <param name="gridCustomPaging">gridCustomPaging</param>
        /// <returns>List<MultiInstitutionAssignmentDataContract></returns>
        public static DataEntryQueueContract GetDataEntryNextRecordData(String inuputXml, CustomPagingArgsContract gridCustomPaging, Int32? CurrentLoggedInUserID, Int32 CurrentFDEQ_Id, String institutionHierarchyIds, Boolean? IsSingleTenant = false, Int32? tenantId = AppConsts.NONE)
        {
            try
            {
                DataTable dataForQueue = new DataTable();
                if (IsSingleTenant.Value)
                    dataForQueue = ComplianceDataManager.GetDataEntryQueueDataForSingleTenant(gridCustomPaging, CurrentLoggedInUserID, tenantId.Value, institutionHierarchyIds);
                else
                    dataForQueue = BALUtils.GetSecurityRepoInstance().GetDataEntryQueueData(inuputXml, gridCustomPaging, CurrentLoggedInUserID);

                IEnumerable<DataRow> rows = dataForQueue.AsEnumerable();

                IEnumerable<DataEntryQueueContract> records = rows.Select(x => new DataEntryQueueContract
                {
                    ApplicantName = Convert.ToString(x["ApplicantName"]),
                    DocumentName = Convert.ToString(x["DocumentName"]),
                    DocumentStatusName = Convert.ToString(x["DocumentStatusName"]),
                    DocumentStatusCode = Convert.ToString(x["DocumentStatusCode"]),
                    DateUploaded = x["DateUploaded"].GetType().Name == "DBNull" ? null : (DateTime?)x["DateUploaded"],
                    DateUploadDatePart = x["DateUploadDatePart"].GetType().Name == "DBNull" ? null : (DateTime?)x["DateUploadDatePart"],
                    DocumentStatusID = Convert.ToInt32(x["DocumentStatusID"]),
                    ApplicantDocumentID = Convert.ToInt32(x["ApplicantDocumentID"]),
                    TenantID = Convert.ToInt32(x["TenantID"]),
                    AssignToUserName = Convert.ToString(x["AssignToUserName"]),
                    ApplicantOrganizationUserID = Convert.ToInt32(x["ApplicantOrganizationUserID"]),
                    FDEQ_ID = Convert.ToInt32(x["FDEQ_ID"]),
                    AssignToUserID = x["AssignToUserID"].GetType().Name == "DBNull" ? AppConsts.NONE : Convert.ToInt32(x["AssignToUserID"]),
                    //UAT-2456
                    DiscardDocumentCount = x["DiscardDocumentCount"].GetType().Name == "DBNull" ? AppConsts.NONE : Convert.ToInt32(x["DiscardDocumentCount"]),
                    TotalCount = Convert.ToInt32(x["TotalCount"]),

                }).SkipWhile(record => record.FDEQ_ID != CurrentFDEQ_Id).Skip(1);

                if (records.Any())
                    return records.FirstOrDefault();
                else
                    return new DataEntryQueueContract();
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

        public static Boolean AssignDocumentToUserForDataEntry(Int32 selectedAssigneeUserId, String selectedAssigneeUserName, List<Int32> documentIdsToAssign,
                                                               Int32 currentloggedInUserId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().AssignDocumentToUserForDataEntry(selectedAssigneeUserId, selectedAssigneeUserName, documentIdsToAssign,
                                                                                           currentloggedInUserId);

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

        public static Boolean DeleteDocumentFromFlatDataEntry(Int32 applicantDocumentId, Int32 tenantId, Int32 applicantOrgUserId,
                                                              Int32 currentloggedInUserId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().DeleteDocumentFromFlatDataEntry(applicantDocumentId, tenantId, applicantOrgUserId, currentloggedInUserId);

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
        /// Gets the FlatDataEntryQueue, by Primary Key of table
        /// </summary>
        /// <param name="fdeqId"></param>
        /// <returns></returns>
        public static FlatDataEntryQueue GetFlatDataEntryQueueRecord(Int32 fdeqId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetFlatDataEntryQueueRecord(fdeqId);
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

        //#region Profile Sharing

        ////UAT-1210 - Method to update Invitation View Status
        //public static void UpdateInvitationViewedStatus(Int32 currentUserID, Int32 invitationID)
        //{
        //    try
        //    {
        //        BALUtils.GetSecurityRepoInstance().UpdateInvitationViewedStatus(currentUserID, invitationID);
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //}

        //#region UAT-1237
        //public static List<SharedUserSearchContract> GetSharedUserSearchData(SharedUserSearchContract sharedUserSearchContract, CustomPagingArgsContract customPagingArgsContract)
        //{
        //    try
        //    {
        //        DataTable dtSharedUserData = BALUtils.GetSecurityRepoInstance().GetSharedUserSearchData(sharedUserSearchContract, customPagingArgsContract);

        //        if (!dtSharedUserData.IsNullOrEmpty() && dtSharedUserData.Rows.Count > 0)
        //        {
        //            IEnumerable<DataRow> rows = dtSharedUserData.AsEnumerable();
        //            return rows.Select(x => new SharedUserSearchContract
        //            {
        //                FirstName = Convert.ToString(x["FirstName"]).Trim(),
        //                LastName = Convert.ToString(x["LastName"]).Trim(),
        //                UserName = Convert.ToString(x["UserName"]).Trim(),
        //                EmailAddress = Convert.ToString(x["EmailAddress"]).Trim(),
        //                SharedUserID = Convert.ToInt32(x["SharedUserID"]),
        //                TotalCount = Convert.ToInt32(x["TotalCount"])
        //            }).ToList();
        //        }
        //        return new List<SharedUserSearchContract>();
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //}
        //#endregion

        //#region UAT-1237 Add Agency/shared users to client user search

        ///// <summary>
        /////  Method to get Shared User Invitation Details based on Shared UserID
        ///// </summary>
        ///// <param name="p"></param>
        //public static List<SharedUserSearchInvitationDetailsContract> GetSharedUserInvitationDetails(Int32 sharedUserID)
        //{
        //    try
        //    {
        //        var lstSharedUserSearchDetails = new List<SharedUserSearchInvitationDetailsContract>();

        //        //Getting result from stored procedure 
        //        var lstInvitationDetails = BALUtils.GetSecurityRepoInstance().GetSharedUserInvitationDetails(sharedUserID);

        //        lstInvitationDetails.Select(col => col.InvitationID).Distinct().ForEach(invitationID =>
        //        {
        //            var invitationDetails = lstInvitationDetails.Where(cond => cond.InvitationID == invitationID).FirstOrDefault();

        //            var currentContract = new SharedUserSearchInvitationDetailsContract();
        //            currentContract.InvitationID = invitationDetails.InvitationID;
        //            currentContract.ApplicantUserID = invitationDetails.ApplicantUserID;
        //            currentContract.ApplicantName = invitationDetails.ApplicantName;
        //            currentContract.InvitationDate = invitationDetails.InvitationDate;
        //            currentContract.InvitationDocumentID = invitationDetails.InvitationDocumentID;
        //            currentContract.InvitationSentStatus = invitationDetails.InvitationSentStatus;
        //            currentContract.TenantID = invitationDetails.TenantID;
        //            currentContract.TenantName = invitationDetails.TenantName;
        //            currentContract.ViewedStatus = invitationDetails.ViewedStatus;
        //            currentContract.InvitationSourceCode = invitationDetails.InvitationSourceCode;

        //            currentContract.lstSharedPackages = new List<INTSOF.UI.Contract.SearchUI.SharedPackages>();

        //            //Add Compliance Packages Data
        //            var compliancePackageSharedList = AddSharedPackagesData(lstInvitationDetails.Where(cond => cond.InvitationID == invitationID).ToList(), true);
        //            currentContract.lstSharedPackages.AddRange(compliancePackageSharedList);

        //            //Add Bkg Package Data
        //            var bkgPackageSharedList = AddSharedPackagesData(lstInvitationDetails.Where(cond => cond.InvitationID == invitationID).ToList(), false);
        //            currentContract.lstSharedPackages.AddRange(bkgPackageSharedList);

        //            lstSharedUserSearchDetails.Add(currentContract);

        //        });
        //        return lstSharedUserSearchDetails;
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //}

        ///// <summary>
        ///// Method to get Shared Packages Data (Compliance and background)
        ///// </summary>
        ///// <param name="list"></param>
        ///// <param name="isCompliancePackage"></param>
        ///// <returns></returns>
        //private static List<INTSOF.UI.Contract.SearchUI.SharedPackages> AddSharedPackagesData(List<GetSharedUserInvitationDetails_Result> lstInvitationDetails, Boolean isCompliancePackage)
        //{
        //    var lstDistinctOrderID = lstInvitationDetails.Where(cond => cond.IsCompliancePackage == isCompliancePackage
        //                                && !cond.OrderID.IsNullOrEmpty()
        //                                && !cond.PackageID.IsNullOrEmpty())
        //                                .Select(col => col.OrderID).Distinct().ToList();

        //    var lstSharedPackages = new List<INTSOF.UI.Contract.SearchUI.SharedPackages>();

        //    lstDistinctOrderID.ForEach(ordID =>
        //    {

        //        lstInvitationDetails.Where(cond => cond.IsCompliancePackage == isCompliancePackage
        //                                && cond.OrderID == ordID).Select(col => col.PackageID).Distinct().ForEach(pkgID =>
        //                                {
        //                                    var currentSharedPkg = new INTSOF.UI.Contract.SearchUI.SharedPackages();
        //                                    var pkgList = lstInvitationDetails.Where(cond => cond.OrderID == ordID && cond.PackageID == pkgID && cond.IsCompliancePackage == isCompliancePackage).ToList();

        //                                    currentSharedPkg.IsCompliancePackage = isCompliancePackage;
        //                                    currentSharedPkg.OrderID = ordID;
        //                                    currentSharedPkg.PackageID = pkgID;
        //                                    currentSharedPkg.PackageName = pkgList.Select(col => col.PackageName).FirstOrDefault();
        //                                    currentSharedPkg.PackageSubscriptionID = pkgList.Select(col => col.PackageSubscriptionID).FirstOrDefault();
        //                                    currentSharedPkg.SnapShotID = pkgList.Select(col => col.SnapShotID).FirstOrDefault();
        //                                    currentSharedPkg.PackageIdentifier = Guid.NewGuid();
        //                                    currentSharedPkg.FlagStatusImagePath = pkgList.Where(cond => !cond.FlagStatusImagePath.IsNullOrEmpty()).Select(col => col.FlagStatusImagePath).FirstOrDefault();
        //                                    currentSharedPkg.ColorFlagPath = pkgList.Where(cond => !cond.ColorFlagPath.IsNullOrEmpty()).Select(col => col.ColorFlagPath).FirstOrDefault();
        //                                    currentSharedPkg.ShowFlagText = pkgList.Where(cond => cond.ColorFlagPath.IsNullOrEmpty()).Any(col => col.ShowFlagText);

        //                                    var currentSharedEntityList = new List<INTSOF.UI.Contract.SearchUI.SharedEntity>();
        //                                    //var distnctCurrentSharedEntityList = new List<INTSOF.UI.Contract.SearchUI.SharedEntity>();
        //                                    //Boolean isResultReportVisible = false;

        //                                    pkgList.Where(cond => !cond.SharedEntityName.IsNullOrEmpty()).Select(col => col.SharedEntityID).Distinct().ForEach(entityID =>
        //                                    {
        //                                        var currentSharedEntity = new INTSOF.UI.Contract.SearchUI.SharedEntity();

        //                                        currentSharedEntity.IsResultReportVisible = pkgList.Where(cond => !cond.SharedEntityName.IsNullOrEmpty() && cond.SharedEntityID == entityID).Any(x => x.IsResultReportVisible == true);
        //                                        currentSharedEntity.SharedEntityID = entityID;
        //                                        currentSharedEntity.SharedEntityName = pkgList.Where(cond => !cond.SharedEntityName.IsNullOrEmpty() && cond.SharedEntityID == entityID).Select(col => col.SharedEntityName).First();
        //                                        if (!entityID.IsNullOrEmpty())
        //                                        {
        //                                            currentSharedEntityList.Add(currentSharedEntity);
        //                                        }
        //                                    });

        //                                    //if (!currentSharedEntity.SharedEntityID.IsNullOrEmpty())
        //                                    //{
        //                                    //    currentSharedEntityList.Add(currentSharedEntity);    
        //                                    //}

        //                                    //currentSharedEntityList = pkgList.Where(cond => !cond.SharedEntityName.IsNullOrEmpty()).DistinctBy(x=>x.SharedEntityID).Select(col => new INTSOF.UI.Contract.SearchUI.SharedEntity
        //                                    //{
        //                                    //    SharedEntityID = col.SharedEntityID,
        //                                    //    SharedEntityName = col.SharedEntityName,
        //                                    //    IsResultReportVisible = col.IsResultReportVisible
        //                                    //}).ToList();

        //                                    currentSharedPkg.lstSharedEntity = new List<INTSOF.UI.Contract.SearchUI.SharedEntity>();
        //                                    if (currentSharedEntityList.IsNullOrEmpty())
        //                                    {
        //                                        currentSharedEntityList = new List<INTSOF.UI.Contract.SearchUI.SharedEntity>();
        //                                    }

        //                                    currentSharedPkg.lstSharedEntity.AddRange(currentSharedEntityList);
        //                                    lstSharedPackages.Add(currentSharedPkg);
        //                                });
        //    });
        //    return lstSharedPackages;
        //}
        //#endregion

        //#region UAT-1201 as a client admin, I should be able to view the attestations for any profile shares that I have sent.
        ///// <summary>
        /////  UAT-1201 - Method to Bind Attestation Details Grid
        ///// </summary>
        ///// <param name="clientID"></param>
        ///// <param name="currentUserID"></param>
        //public static List<Entity.ProfileSharingInvitationGroup> GetAttestationDetailsData(Int32 clientID, Int32 currentUserID)
        //{
        //    try
        //    {
        //        Int32 adminInitializedInvitationStatus = GetInvitationStatusIdByCode(LkpInviationStatusTypes.ADMIN_INITLIAZED.GetStringValue());
        //        return BALUtils.GetSecurityRepoInstance().GetAttestationDetailsData(clientID, currentUserID, adminInitializedInvitationStatus);
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //}

        ///// <summary>
        ///// UAT-1201 - Method to Get Attestation Documents Details By InvitationGroupID 
        ///// </summary>
        ///// <param name="p"></param>
        ///// <returns></returns>
        //public static List<InvitationDocument> GetAttestatationDocumentDetails(Int32 invitationGroupID)
        //{
        //    try
        //    {
        //        return BALUtils.GetSecurityRepoInstance().GetAttestatationDocumentDetails(invitationGroupID);
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //}

        ///// <summary>
        ///// UAT-1201 - Method to Get Invitation Document by invitationDocumentID
        ///// </summary>
        ///// <param name="invitationDocumentID"></param>
        ///// <returns></returns>
        //public static InvitationDocument GetInvitationDocumentByDocumentID(Int32 invitationDocumentID)
        //{
        //    try
        //    {
        //        return BALUtils.GetSecurityRepoInstance().GetInvitationDocumentByDocumentID(invitationDocumentID);
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //}
        //#endregion

        ///// <summary>
        ///// Method to get Invitation Source ID by Invitation Code
        ///// </summary>
        ///// <param name="invitationCode"></param>
        ///// <returns></returns>
        //private static Int32 GetInvitationStatusIdByCode(String invitationStatusCode)
        //{
        //    try
        //    {
        //        lkpInvitationStatu invitationSource = LookupManager.GetLookUpData<lkpInvitationStatu>().FirstOrDefault(x => x.Code == invitationStatusCode && !x.IsDeleted);
        //        if (invitationSource != null)
        //        {
        //            return invitationSource.InvitationStatusID;
        //        }
        //        return AppConsts.NONE;
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

        ///// <summary>
        ///// Method to get Invitation Source ID by Invitation Code
        ///// </summary>
        ///// <param name="invitationCode"></param>
        ///// <returns></returns>
        //private static Int32? GetInvitationSourceIdByCode(String invitationCode)
        //{
        //    try
        //    {
        //        lkpInvitationSource invitationSource = LookupManager.GetLookUpData<lkpInvitationSource>().FirstOrDefault(x => x.Code == invitationCode && !x.IsDeleted);
        //        if (invitationSource != null)
        //        {
        //            return invitationSource.InvitationSourceID;
        //        }
        //        return AppConsts.NONE;
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

        ///// <summary>
        ///// Method to Update Invitee Organization UserID in ProfileSharingInvitation table
        ///// </summary>
        ///// <param name="orgUserID"></param>
        ///// <param name="inviteToken"></param>
        ///// <returns></returns>
        //public static Boolean UpdateInviteeOrganizationUserID(Int32 orgUserID, Guid inviteToken)
        //{
        //    try
        //    {
        //        Int32 adminInitializedID = LookupManager.GetLookUpData<lkpInvitationStatu>().Where(invsts => invsts.Code == LkpInviationStatusTypes.ADMIN_INITLIAZED.GetStringValue()).First().InvitationStatusID;
        //        return BALUtils.GetSecurityRepoInstance().UpdateInviteeOrganizationUserID(orgUserID, inviteToken, adminInitializedID);
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //}

        ///// <summary>
        ///// Gets the list of invitations that has been sent by the applicant
        ///// </summary>
        ///// <param name="applicantOrgUserId"></param>
        ///// <param name="tenantId"></param>
        ///// <returns></returns>
        //public static List<InvitationDataContract> GetApplicantInvitations(Int32 applicantOrgUserId, Int32 tenantId)
        //{
        //    try
        //    {
        //        var _lstInvitations = new List<InvitationDataContract>();
        //        var _dtInvitations = BALUtils.GetSecurityRepoInstance().GetApplicantInvitations(applicantOrgUserId, tenantId);

        //        if (_dtInvitations.Rows.Count > AppConsts.NONE)
        //        {
        //            var _invitationIdColumn = "PSIId";
        //            var _inviteeNameColumn = "InviteeName";
        //            var _inviteeEmailColumn = "InviteeEmail";
        //            var _inviteePhoneColumn = "InviteePhone";
        //            var _inviteeAgencyColumn = "InviteeAgency";
        //            var _invitationDateColumn = "InvitationDate";
        //            var _inviteeLastViewedColumn = "InviteeLastViewed";
        //            var _expireationDateColumn = "ExpirationDate";
        //            var _viewsRemainingColumn = "ViewsRemaining";
        //            var _expirationTypeCodeColumn = "ExpirationTypeCode";

        //            for (int i = 0; i < _dtInvitations.Rows.Count; i++)
        //            {
        //                var _invitationContract = new InvitationDataContract();
        //                _invitationContract.ID = Convert.ToInt32(_dtInvitations.Rows[i][_invitationIdColumn]);
        //                _invitationContract.Name = Convert.ToString(_dtInvitations.Rows[i][_inviteeNameColumn]);
        //                _invitationContract.EmailAddress = Convert.ToString(_dtInvitations.Rows[i][_inviteeEmailColumn]);
        //                _invitationContract.Phone = Convert.ToString(_dtInvitations.Rows[i][_inviteePhoneColumn]);
        //                _invitationContract.Agency = Convert.ToString(_dtInvitations.Rows[i][_inviteeAgencyColumn]);
        //                _invitationContract.InvitationDate = Convert.ToDateTime(_dtInvitations.Rows[i][_invitationDateColumn]);
        //                _invitationContract.LastViewedDate = _dtInvitations.Rows[i][_inviteeLastViewedColumn] == DBNull.Value ? (DateTime?)null :
        //                                                        Convert.ToDateTime(_dtInvitations.Rows[i][_inviteeLastViewedColumn]);
        //                _invitationContract.ExpirationDate = _dtInvitations.Rows[i][_expireationDateColumn] == DBNull.Value ? (DateTime?)null :
        //                                                        Convert.ToDateTime(_dtInvitations.Rows[i][_expireationDateColumn]);
        //                _invitationContract.ViewsRemaining = Convert.ToInt32(_dtInvitations.Rows[i][_viewsRemainingColumn]);
        //                _invitationContract.ExpirationTypeCode = Convert.ToString(_dtInvitations.Rows[i][_expirationTypeCodeColumn]);

        //                _invitationContract.IsExpirationCountVisible = _invitationContract.ExpirationTypeCode == InvitationExpirationTypes.NUMBER_OF_VIEWS.GetStringValue()
        //                                                                ? true : false;

        //                _invitationContract.IsExpirationDateVisible = _invitationContract.ExpirationTypeCode == InvitationExpirationTypes.SPECIFIC_DATE.GetStringValue()
        //                                                              ? true : false;
        //                _lstInvitations.Add(_invitationContract);
        //            }
        //        }
        //        return _lstInvitations;
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }

        //}

        ///// <summary>
        ///// Save the Invitation and return the ID of the invitation generated.
        ///// </summary>
        ///// <param name="invitationDetails"></param>
        ///// <returns>Tuple with InvitationID, its related Token and whether its a new invitation</returns>
        //public static Tuple<Int32, Guid> SaveProfileSharingInvitation(InvitationDetailsContract invitationDetails, Int32 genaratedInvitationGroupID)
        //{
        //    try
        //    {
        //        var _repoInstance = BALUtils.GetSecurityRepoInstance();
        //        invitationDetails.ExpirationTypeId = LookupManager.GetLookUpData<lkpInvitationExpirationType>().Where(iet => iet.Code == invitationDetails.ExpirationTypeCode).First().ExpirationTypeID;
        //        invitationDetails.InvitationStatusId = LookupManager.GetLookUpData<lkpInvitationStatu>().Where(invsts => invsts.Code == invitationDetails.InvitationStatusCode).First().InvitationStatusID;
        //        invitationDetails.InvitationSourceId = LookupManager.GetLookUpData<lkpInvitationSource>().Where(invsrc => invsrc.Code == invitationDetails.InvitationSourceCode).First().InvitationSourceID;
        //        //invitationDetails.InitiatedById = invitationDetails.CurrentUserId;
        //        var _inviteeOrgUserId = _repoInstance.GetSharedUserOrgId(invitationDetails.EmailAddress);
        //        invitationDetails.InviteeOrgUserId = _inviteeOrgUserId > AppConsts.NONE ? _inviteeOrgUserId : (Int32?)null;
        //        return _repoInstance.SaveProfileSharingInvitation(invitationDetails, genaratedInvitationGroupID);
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //}

        ///// <summary>
        ///// Save the Invitation and return the ID of the invitation generated.
        ///// </summary>
        ///// <param name="invitationDetails"></param>
        ///// <returns>Tuple with InvitationID, its related Token and whether its a new invitation</returns>
        //public static List<ProfileSharingInvitation> SaveAdminInvitations(List<InvitationDetailsContract> lstInvitations, ProfileSharingInvitationGroup invitationGroup, String invitationSourceCode)
        //{
        //    try
        //    {
        //        var _repoInstance = BALUtils.GetSecurityRepoInstance();

        //        var _invitationSourceId = LookupManager.GetLookUpData<lkpInvitationSource>().Where(invsrc => invsrc.Code == invitationSourceCode).First().InvitationSourceID;
        //        var _expirationTypeId = LookupManager.GetLookUpData<lkpInvitationExpirationType>().Where(iet => iet.Code == InvitationExpirationTypes.NO_EXPIRATION_CRITERIA.GetStringValue()).First().ExpirationTypeID;
        //        var _invitationStatusId = LookupManager.GetLookUpData<lkpInvitationStatu>().Where(invsts => invsts.Code == LkpInviationStatusTypes.ADMIN_INITLIAZED.GetStringValue()).First().InvitationStatusID;

        //        //var _metaData = LookupManager.GetLookUpData<ApplicantInvitationMetaData>().ToList();
        //        var _allUsers = _repoInstance.GetSharedUserOrgIds(lstInvitations.Select(x => x.EmailAddress).Distinct().ToList());

        //        lstInvitations.ForEach(invitation =>
        //        {
        //            invitation.InvitationStatusId = _invitationStatusId;
        //            invitation.ExpirationTypeId = _expirationTypeId;
        //            invitation.InvitationSourceId = _invitationSourceId;
        //            //var _currentUser = _allUsers.Where(au => au.Key == invitation.EmailAddress).FirstOrDefault();

        //            if (_allUsers.ContainsKey(invitation.EmailAddress))
        //            {
        //                invitation.InviteeOrgUserId = _allUsers.GetValue(invitation.EmailAddress);
        //            }
        //        });

        //        return _repoInstance.SaveAdminInvitations(lstInvitations, invitationGroup);
        //        ////invitationDetails.InviteeOrgUserId = _inviteeOrgUserId > AppConsts.NONE ? _inviteeOrgUserId : (Int32?)null;
        //        //return _repoInstance.SaveProfileSharingInvitation(invitationDetails, genaratedInvitationGroupID);
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //}

        ///// <summary>
        ///// Returns whether the Shared user is being invited
        ///// </summary>
        ///// <param name="emailAddress"></param>
        ///// <returns></returns>
        //public static Boolean IsSharedUserInvited(String emailAddress)
        //{
        //    try
        //    {
        //        return BALUtils.GetSecurityRepoInstance().IsSharedUserInvited(emailAddress);
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //}

        ///// <summary>
        ///// Send the email for the Invitation
        ///// </summary>
        ///// <param name="toAddress"></param>
        ///// <param name="emailContent"></param>
        ///// <param name="subject"></param>
        ///// <param name="dicContent"></param>
        ///// <param name="isContentReplaced"></param>
        //public static Boolean SendInvitationEmail(String toAddress, String emailContent, String subject, Dictionary<String, String> dicContent, Boolean isContentReplaced)
        //{
        //    if (!isContentReplaced)
        //    {
        //        subject = subject.Replace(AppConsts.PSIEMAIL_STUDENTNAME, dicContent.GetValue(AppConsts.PSIEMAIL_STUDENTNAME));
        //        if (dicContent.ContainsKey(AppConsts.PSIEMAIL_SCHOOLNAME))
        //        {
        //            subject = subject.Replace(AppConsts.PSIEMAIL_SCHOOLNAME, dicContent.GetValue(AppConsts.PSIEMAIL_SCHOOLNAME));
        //        }
        //        if (dicContent.ContainsKey(AppConsts.PSIEMAIL_RECIPIENTNAME))
        //        {
        //            emailContent = emailContent.Replace(AppConsts.PSIEMAIL_RECIPIENTNAME, dicContent.GetValue(AppConsts.PSIEMAIL_RECIPIENTNAME));
        //        }

        //        if (dicContent.ContainsKey(AppConsts.PSIEMAIL_PROFILEURL))
        //        {
        //            emailContent = emailContent.Replace(AppConsts.PSIEMAIL_PROFILEURL, dicContent.GetValue(AppConsts.PSIEMAIL_PROFILEURL));
        //        }
        //        if (dicContent.ContainsKey(AppConsts.PSIEMAIL_CENTRALLOGINURL))
        //        {
        //            emailContent = emailContent.Replace(AppConsts.PSIEMAIL_CENTRALLOGINURL, dicContent.GetValue(AppConsts.PSIEMAIL_CENTRALLOGINURL));
        //        }
        //        if (dicContent.ContainsKey(AppConsts.PSIEMAIL_CUSTOMMESSAGE))
        //        {
        //            emailContent = emailContent.Replace(AppConsts.PSIEMAIL_CUSTOMMESSAGE, dicContent.GetValue(AppConsts.PSIEMAIL_CUSTOMMESSAGE));
        //        }
        //        if (dicContent.ContainsKey(AppConsts.PSIEMAIL_STUDENTNAME))
        //        {
        //            emailContent = emailContent.Replace(AppConsts.PSIEMAIL_STUDENTNAME, dicContent.GetValue(AppConsts.PSIEMAIL_STUDENTNAME));
        //        }
        //        if (dicContent.ContainsKey(AppConsts.PSIEMAIL_SELECTEDPACKAGEDATA))
        //        {
        //            emailContent = emailContent.Replace(AppConsts.PSIEMAIL_SELECTEDPACKAGEDATA, dicContent.GetValue(AppConsts.PSIEMAIL_SELECTEDPACKAGEDATA));
        //        }
        //        if (dicContent.ContainsKey(AppConsts.PSIEMAIL_SHAREDAPPLICANTDATA))
        //        {
        //            emailContent = emailContent.Replace(AppConsts.PSIEMAIL_SHAREDAPPLICANTDATA, dicContent.GetValue(AppConsts.PSIEMAIL_SHAREDAPPLICANTDATA));
        //        }
        //    }

        //    Dictionary<String, String> dicEmailContent = new Dictionary<String, String>
        //        {
        //            {"EmailBody", emailContent} 
        //        };

        //    return SysXEmailService.SendSystemMail(dicEmailContent, subject, toAddress);
        //}

        ///// <summary>
        ///// Check Whether shared user exists or not
        ///// </summary>
        ///// <param name="inviteToken"></param>
        ///// <returns></returns>
        //public static Boolean IsSharedUserExists(Guid inviteToken)
        //{
        //    try
        //    {
        //        return BALUtils.GetSecurityRepoInstance().IsSharedUserExists(inviteToken);
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //}

        ///// <summary>
        ///// Method to get Shared User Data from Invitation Sent by applicant(currently only Email)
        ///// </summary>
        ///// <param name="inviteToken"></param>
        ///// <returns></returns>
        //public static String GetSharedUserDataFromInvitation(Guid inviteToken)
        //{
        //    try
        //    {
        //        return BALUtils.GetSecurityRepoInstance().GetSharedUserDataFromInvitation(inviteToken);
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //}

        ///// <summary>
        ///// Save/Update the Invitation and return the ID of the invitation generated.
        ///// </summary>
        ///// <param name="invitationDetails"></param>
        ///// <returns>Tuple with InvitationID, its related Token and whether its a new invitation</returns>
        //public static ProfileSharingInvitation GetInvitationDetails(Int32 invitationId)
        //{
        //    try
        //    {
        //        return BALUtils.GetSecurityRepoInstance().GetInvitationDetails(invitationId);
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //}

        ///// <summary>
        ///// Update the Status of the Invitation
        ///// </summary>
        ///// <param name="statusCode"></param>
        ///// <param name="invitationId"></param>
        ///// <param name="currentUserId"></param>
        //public static void UpdateInvitationStatus(String statusCode, Int32 invitationId, Int32 currentUserId)
        //{
        //    try
        //    {
        //        var _statusId = LookupManager.GetLookUpIDbyCode<lkpInvitationStatu>(invSts => invSts.Code == statusCode);
        //        BALUtils.GetSecurityRepoInstance().UpdateInvitationStatus(_statusId, invitationId, currentUserId);
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //}


        ///// <summary>
        ///// Update the Status of the bulk Invitations
        ///// </summary>
        ///// <param name="statusCode"></param>
        ///// <param name="invitationId"></param>
        ///// <param name="currentUserId"></param>
        //public static void UpdateBulkInvitationStatus(String statusCode, List<Int32> invitationId, Int32 currentUserId)
        //{
        //    try
        //    {
        //        var _statusId = LookupManager.GetLookUpIDbyCode<lkpInvitationStatu>(invSts => invSts.Code == statusCode);
        //        BALUtils.GetSecurityRepoInstance().UpdateBulkInvitationStatus(_statusId, invitationId, currentUserId);
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //}

        ///// <summary>
        ///// To get invitation records
        ///// </summary>
        ///// <param name="searchContract"></param>
        ///// <param name="gridCustomPaging"></param>
        ///// <returns></returns>
        //public static List<InvitationDataContract> GetInvitationData(InvitationSearchContract searchContract, CustomPagingArgsContract gridCustomPaging)
        //{
        //    try
        //    {
        //        DataTable dataForQueue = BALUtils.GetSecurityRepoInstance().GetInvitationData(searchContract, gridCustomPaging);
        //        return NewAssignValuesToDataModel(dataForQueue);
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //}

        //private static List<InvitationDataContract> NewAssignValuesToDataModel(DataTable table)
        //{
        //    try
        //    {
        //        IEnumerable<DataRow> rows = table.AsEnumerable();
        //        return rows.Select(x => new InvitationDataContract
        //        {
        //            ID = x["ID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["ID"]),
        //            Name = Convert.ToString(x["Name"]),
        //            EmailAddress = Convert.ToString(x["EmailAddress"]),
        //            Phone = Convert.ToString(x["Phone"]),
        //            TenantID = Convert.ToInt32(x["TenantID"]),
        //            ExpirationDate = x["ExpirationDate"].GetType().Name == "DBNull" ? null : (DateTime?)x["ExpirationDate"],
        //            InvitationDate = x["InvitationDate"].GetType().Name == "DBNull" ? null : (DateTime?)x["InvitationDate"],
        //            LastViewedDate = x["LastViewedDate"].GetType().Name == "DBNull" ? null : (DateTime?)x["LastViewedDate"],
        //            ViewsRemaining = x["ViewsRemaining"].GetType().Name == "DBNull" ? null : (Int32?)x["ViewsRemaining"],
        //            InviteTypeID = x["InviteTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(x["InviteTypeID"]),
        //            InviteTypeCode = Convert.ToString(x["InviteTypeCode"]),
        //            InviteTypeName = Convert.ToString(x["InviteTypeName"]),
        //            Notes = Convert.ToString(x["Notes"]),
        //            TenantName = Convert.ToString(x["TenantName"]),
        //            IsInvitationVisible = Convert.ToBoolean(x["IsInvitationVisible"]),
        //            TotalCount = x["TotalCount"] == DBNull.Value ? 0 : Convert.ToInt32(x["TotalCount"])
        //        }).ToList();
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //}

        ///// <summary>
        ///// Get Invitations based upon PSI_InviteeOrgUserID
        ///// </summary>
        ///// <param name="inviteeOrgUserID"></param>
        ///// <returns></returns>
        //public static IEnumerable<ProfileSharingInvitation> GetInvitationsByInviteeOrgUserID(Int32 inviteeOrgUserID)
        //{
        //    try
        //    {
        //        return BALUtils.GetSecurityRepoInstance().GetInvitationsByInviteeOrgUserID(inviteeOrgUserID);
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //}

        ///// <summary>
        ///// Update the Views remaining of the Invitation
        ///// </summary>
        ///// <param name="statusCode"></param>
        ///// <param name="invitationId"></param>
        ///// <param name="currentUserId"></param>
        //public static Boolean UpdateInvitationViewsRemaining(Int32 invitationId, Int32 currentUserId, String expiredInvitationTypeCode)
        //{
        //    try
        //    {
        //        Int32 expiredInvitationTypeId = LookupManager.GetLookUpData<Entity.lkpInvitationStatu>().FirstOrDefault(cond => cond.Code == expiredInvitationTypeCode).InvitationStatusID;
        //        return BALUtils.GetSecurityRepoInstance().UpdateInvitationViewsRemaining(invitationId, currentUserId, expiredInvitationTypeId);
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //}

        ///// <summary>
        ///// Update Notes of the Invitation
        ///// </summary>
        ///// <param name="inviteeNotes"></param>
        ///// <param name="invitationId"></param>
        ///// <param name="currentUserId"></param>        
        //public static Boolean UpdateInvitationNotes(Int32 invitationId, Int32 currentUserId, String notes)
        //{
        //    try
        //    {
        //        return BALUtils.GetSecurityRepoInstance().UpdateInvitationNotes(invitationId, currentUserId, notes);
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //}

        ///// <summary>
        ///// Method to Get All Agencies
        ///// </summary>
        ///// <returns></returns>
        //public static List<Agency> GetAllAgency(Int32 institutionID)
        //{
        //    try
        //    {
        //        return BALUtils.GetSecurityRepoInstance().GetAllAgency(institutionID);
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //}

        ///// <summary>
        ///// Method to Get Agency User Data by Agency ID and InstitutionID
        ///// </summary>
        ///// <param name="p"></param>
        ///// <returns></returns>
        //public static List<usp_GetAgencyUserData_Result> GetAgencyUserData(Int32 institutionID, Int32 agencyID)
        //{
        //    try
        //    {
        //        return BALUtils.GetSecurityRepoInstance().GetAgencyUserData(institutionID, agencyID);
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //}

        ///// <summary>
        ///// Method to genarate New Invitation Group
        ///// </summary>
        ///// <param name="agencyID"></param>
        ///// <param name="initiatedByID"></param>
        ///// <returns></returns>
        //public static Int32 GenarateNewInvitationGroup(ProfileSharingInvitationGroup invitationGroupObj)
        //{
        //    try
        //    {
        //        return BALUtils.GetSecurityRepoInstance().GenarateNewInvitationGroup(invitationGroupObj);
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //}

        //public static List<InvitationDocumentContract> GetAttestationDocumentData(List<InvitationIDsContract> invitationIDsContract)
        //{
        //    try
        //    {
        //        String clientInvitationIds = String.Join(",", invitationIDsContract.Select(col => col.ProfileSharingInvitationID).ToList());
        //        DataTable attestationReportData = BALUtils.GetSecurityRepoInstance().GetAttestationDocumentData(clientInvitationIds);
        //        IEnumerable<DataRow> attestationRows = attestationReportData.AsEnumerable();

        //        return attestationRows.Select(col => new InvitationDocumentContract
        //        {
        //            ProfileSharingInvitationID = col["ProfileSharingInvitationID"] == DBNull.Value ? 0 : Convert.ToInt32(col["ProfileSharingInvitationID"]),
        //            Name = Convert.ToString(col["FirstName"]) + " " + Convert.ToString(col["LastName"]),
        //            DocumentPath = Convert.ToString(col["DocumentFilePath"]),
        //            ApplicantDocumentID = col["InvitationDocumentID"] == DBNull.Value ? 0 : Convert.ToInt32(col["InvitationDocumentID"])
        //        }).ToList();
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //}

        //public static InvitationDocument GetInvitationDocuments(Int32 invitationId)
        //{
        //    try
        //    {
        //        return BALUtils.GetSecurityRepoInstance().GetInvitationDocuments(invitationId);
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //}

        //public static Int32 SaveAttestationDocument(String documentTypeCode, String pdfDocPath, Int32 currentLoggedInUserID)
        //{
        //    try
        //    {
        //        return BALUtils.GetSecurityRepoInstance().SaveAttestationDocument(pdfDocPath, documentTypeCode, currentLoggedInUserID);
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }

        //}

        //public static Boolean SaveInvitationDocumentMapping(List<InvitationDocumentMapping> lstInvitationDocumentMapping)
        //{
        //    try
        //    {
        //        return BALUtils.GetSecurityRepoInstance().SaveInvitationDocumentMapping(lstInvitationDocumentMapping);
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }

        //}

        ///// <summary>
        ///// Method to Get Invitation Data by Invite Token
        ///// </summary>
        ///// <param name="inviteToken"></param>
        ///// <returns></returns>
        //public static ProfileSharingInvitation GetInvitationDataByToken(Guid inviteToken)
        //{
        //    try
        //    {
        //        return BALUtils.GetSecurityRepoInstance().GetInvitationDataByToken(inviteToken);
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //}

        //#endregion

        #region UAT-1176 - Employment Disclosure

        /// <summary>
        /// Method to save Employment Disclosure Details
        /// </summary>
        /// <param name="p"></param>
        public static Boolean SaveEDDetails(Int32 organizationUserID)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().SaveEDDetails(organizationUserID);
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



        #region UAT-3321-User Guide
        /// <summary>
        /// Method to Get User Guide For Agency User by System Document ID 
        /// 

        public static Entity.SystemDocument GetUserGuideForAgencyUser()
        {
            try
            {
                Int32 docTypeID = GetDocumentTypeIDByCode(DislkpDocumentType.USER_GUIDE_FOR_AGENCY_USER.GetStringValue());
                return BALUtils.GetSecurityRepoInstance().GetSystemDocumentByDocTypeID(docTypeID);
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



        #region UAT-1176 - EMPLOYMENT DISCLOSURE
        /// <summary>
        /// Method to Get Employment Disclosure Document by System Document ID 
        /// </summary>
        /// <param name="aWSUseS3"></param>
        public static Entity.SystemDocument GetEmploymentDisclosureDocument()
        {
            try
            {
                Int32 docTypeID = GetDocumentTypeIDByCode(DislkpDocumentType.EMPLOYMENT_DISCLOSURE_FORM.GetStringValue());
                return BALUtils.GetSecurityRepoInstance().GetSystemDocumentByDocTypeID(docTypeID);
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
        /// Genric Method to Get DocumentTypeID By DocumentTypeCode
        /// </summary>
        /// <param name="dislkpDocumentType"></param>
        public static Int32 GetDocumentTypeIDByCode(String docTypeCode)
        {
            try
            {
                return LookupManager.GetLookUpData<lkpDocumentType>().FirstOrDefault(x => x.DT_IsActive && x.DT_Code == docTypeCode).DT_ID;
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

        #region UAT-1086 WB: creation of video tutorial widget for admin (client and ADB) dashboard

        public static IEnumerable<ApplicationVideo> GetApplicationVideos()
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetApplicationVideos();
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

        public static Boolean SaveApplicationVideos(ApplicationVideo appVideo)
        {
            try
            {
                (BALUtils.GetSecurityRepoInstance() as IBaseRepository).AddObjectEntity(appVideo);
                return true;
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


        public static Boolean UpdateApplicationVideo(ApplicationVideo appVideo)
        {
            try
            {
                (BALUtils.GetSecurityRepoInstance() as IBaseRepository).UpdateObjectEntity(appVideo);
                return true;
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

        public static ApplicationVideo GetApplicationVideo(Int32 applicationVideoID)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetApplicationVideo(applicationVideoID);
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

        #region UAT-1178 USER ATTESTATION DISCLOSURE FORM
        /// <summary>
        /// Method to Check whether Client admin has any Bkg Feature or not
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static Boolean CheckForClientRoleFeatures(Guid userID)
        {
            try
            {
                List<short> lstbusinessChannelTypeID = GetBusinessChannelTypes().Where(cond => cond.Code == BusinessChannelType.AMS.GetStringValue()
                                                                                             || cond.Code == BusinessChannelType.COMMON.GetStringValue())
                                                                                .Select(col => col.BusinessChannelTypeID).ToList();
                return BALUtils.GetSecurityRepoInstance().CheckForClientRoleFeatures(userID, lstbusinessChannelTypeID);
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


        ///// <summary>
        ///// Method to cehck whether Shared User recieved any Bkg Order invitation
        ///// </summary>
        ///// <returns></returns>
        //public static bool CheckForBkgInvitation(Int32 orgUserID)
        //{
        //    try
        //    {
        //        return BALUtils.GetSecurityRepoInstance().CheckForBkgInvitation(orgUserID);
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //}

        /// <summary>
        /// Method to get User Attestation Master (Empty) Document from System Document table
        /// </summary>
        /// <param name="isClientAdmin"></param>
        /// <returns></returns>
        public static Entity.SystemDocument GetUserAttestationDisclosureDocument(bool isClientAdmin)
        {
            try
            {
                Int32 docTypeID;
                if (isClientAdmin)
                    docTypeID = GetDocumentTypeIDByCode(DislkpDocumentType.USER_ATTESTATION_DISCLOSURE_FORM_CLIENT_ADMIN.GetStringValue());
                else
                    docTypeID = GetDocumentTypeIDByCode(DislkpDocumentType.USER_ATTESTATION_DISCLOSURE_FORM_SHARED_USER.GetStringValue());

                return BALUtils.GetSecurityRepoInstance().GetSystemDocumentByDocTypeID(docTypeID);
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
        /// Method to partially Fill User Attestation Document with pre-required data
        /// </summary>
        /// <param name="orgUserID"></param>
        /// <param name="documentType"></param>
        /// <returns></returns>
        public static UserAttestationDetail FillUserAttestationDocumentWithPrePopulatedData(Int32 orgUserID, String documentType)
        {
            try
            {
                SystemDocument emptyDocument = new SystemDocument();
                if (documentType == DislkpDocumentType.USER_ATTESTATION_DISCLOSURE_FORM_CLIENT_ADMIN.GetStringValue())
                {
                    emptyDocument = GetUserAttestationDisclosureDocument(true);
                }
                else if (documentType == DislkpDocumentType.USER_ATTESTATION_DISCLOSURE_FORM_SHARED_USER.GetStringValue())
                {
                    emptyDocument = GetUserAttestationDisclosureDocument(false);
                }
                byte[] buffer = CommonFileManager.RetrieveDocument(emptyDocument.DocumentPath, FileType.SystemDocumentLocation.GetStringValue());
                byte[] updatedDocument = null;

                //Call SP
                List<Entity.SharedDataEntity.GetAttestationDocumentUserInfo_Result> attestationDocumentUserInfo = BALUtils.GetProfileSharingRepoInstance().AttestationDocumentUserInfo(orgUserID, documentType);

                if (!attestationDocumentUserInfo.IsNullOrEmpty())
                {
                    PdfReader reader = new PdfReader(buffer);
                    MemoryStream ms = new MemoryStream();
                    PdfStamper stamper = new PdfStamper(reader, ms);

                    //Fill-in the form values
                    AcroFields af = stamper.AcroFields;
                    foreach (var item in attestationDocumentUserInfo)
                    {
                        if (af.GetField(item.FieldName).IsNotNull())
                        {
                            af.SetField(item.FieldName, item.FieldValue);
                        }
                    }
                    //stamper.FormFlattening = true;
                    stamper.Close();
                    updatedDocument = ms.ToArray();
                    ms.Close();
                    //Recompress final document to further shrink.
                    //updatedDocument = CompressPDFDocument(updatedDocument);
                    reader.Close();
                }
                if (updatedDocument.IsNotNull())
                {
                    //Save partially filled document in DB
                    Boolean aWSUseS3 = false;
                    if (!ConfigurationManager.AppSettings["AWSUseS3"].IsNullOrEmpty())
                    {
                        aWSUseS3 = Convert.ToBoolean(ConfigurationManager.AppSettings["AWSUseS3"]);
                    }
                    return SaveUserAttestationDocument(updatedDocument, aWSUseS3, documentType, AppConsts.UAF_PARTIALLY_FILLED_MODE, orgUserID, new UserAttestationDetail());
                    // updatedDocument;
                }
                return new UserAttestationDetail();
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Save the partially filled disclosre document 
        /// </summary>
        /// <param name="pdfBytes">pdfBytes</param>
        /// <returns>Boolean</returns>
        public static UserAttestationDetail SaveUserAttestationDocument(byte[] pdfBytes, Boolean aWSUseS3, String documentTypeCode, String documentStage, Int32 orgUserID, UserAttestationDetail partiallyFilledDocumentObj)//Int32? SysDocId = null
        {
            String tempFilePath = ConfigurationManager.AppSettings["TemporaryFileLocation"];
            String userAttestationFormLocation = String.Empty;
            String filename = String.Empty;

            if (tempFilePath.IsNullOrEmpty())
            {
                BALUtils.LogError("Please provide path for TemporaryFileLocation in config.", new SystemException());
                throw new SystemException("Please provide path for TemporaryFileLocation in config.");
            }
            if (!tempFilePath.EndsWith(@"\"))
            {
                tempFilePath += @"\";
            }
            tempFilePath += "UserAttestationForms" + @"\";

            if (!Directory.Exists(tempFilePath))
                Directory.CreateDirectory(tempFilePath);

            //Check whether use AWS S3, true if need to use
            if (aWSUseS3 == false)
            {
                userAttestationFormLocation = ConfigurationManager.AppSettings["UserAttestationFormLocation"];
                if (!userAttestationFormLocation.EndsWith("\\"))
                {
                    userAttestationFormLocation += "\\";
                }
                userAttestationFormLocation += "UserAttestationForms" + @"\";

                if (!Directory.Exists(userAttestationFormLocation))
                {
                    Directory.CreateDirectory(userAttestationFormLocation);
                }
            }

            String date = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("mmss") + DateTime.Now.Millisecond.ToString();
            String destFileName = "UAF" + "_" + orgUserID + "_" + date + ".pdf";
            String newTempFilePath = Path.Combine(tempFilePath, destFileName);
            String newFinalFilePath = String.Empty;

            filename = "User Attestation Disclosure Form" + "_" + orgUserID + "_" + date + ".pdf";
            FileStream _FileStream = null;
            try
            {
                _FileStream = new FileStream(newTempFilePath,
                            System.IO.FileMode.Create,
                            System.IO.FileAccess.Write);
                _FileStream.Write(pdfBytes, 0, pdfBytes.Length);
                long length = new System.IO.FileInfo(newTempFilePath).Length;
                Int32 filesize = 0;
                bool result = Int32.TryParse(length.ToString(), out filesize);
                try
                {
                    _FileStream.Close();
                }
                catch (Exception)
                {
                    BALUtils.LogError("Error while closing fileStream", new SystemException());
                }

                if (documentStage == AppConsts.UAF_PARTIALLY_FILLED_MODE)
                {
                    //Stage 1
                    //Saving new object of Partially filled User Attestation Document 
                    UserAttestationDetail userAttestationDetails = new UserAttestationDetail();

                    userAttestationDetails.UAD_OrganizationUserID = orgUserID;
                    userAttestationDetails.UAD_FileName = filename;
                    userAttestationDetails.UAD_Size = filesize;
                    userAttestationDetails.UAD_Description = "User Attestation Document [partially filled]";
                    userAttestationDetails.UAD_DocumentPath = newTempFilePath;
                    userAttestationDetails.UAD_IsDeleted = false;
                    userAttestationDetails.UAD_IsActive = false; //For Partially Filled Document
                    userAttestationDetails.UAD_CreatedByID = orgUserID;
                    userAttestationDetails.UAD_CreatedOn = DateTime.Now;
                    userAttestationDetails.UAD_DocumentTypeID = GetDocumentTypeIDByCode(documentTypeCode);

                    return BALUtils.GetSecurityRepoInstance().SaveUpdateUserAttestationDocument(userAttestationDetails, false);
                }
                else if (documentStage == AppConsts.UAF_PREVIEW_MODE)
                {
                    partiallyFilledDocumentObj.UAD_OrganizationUserID = orgUserID;
                    partiallyFilledDocumentObj.UAD_FileName = filename;
                    partiallyFilledDocumentObj.UAD_Size = filesize;
                    partiallyFilledDocumentObj.UAD_Description = "User Attestation Document [preview] ";
                    partiallyFilledDocumentObj.UAD_DocumentPath = newTempFilePath;
                    partiallyFilledDocumentObj.UAD_IsDeleted = false;
                    partiallyFilledDocumentObj.UAD_IsActive = false;
                    partiallyFilledDocumentObj.UAD_DocumentTypeID = GetDocumentTypeIDByCode(documentTypeCode);
                    partiallyFilledDocumentObj.UAD_ModifiedByID = orgUserID;
                    partiallyFilledDocumentObj.UAD_ModifiedOn = DateTime.Now;

                    return BALUtils.GetSecurityRepoInstance().SaveUpdateUserAttestationDocument(partiallyFilledDocumentObj, true);
                }
                else if (documentStage == AppConsts.UAF_FULLY_FILLED_MODE)
                {
                    //Stage 3

                    //Check whether use AWS S3, true if need to use
                    if (aWSUseS3 == false)
                    {
                        //Move file to other location
                        String pdfDocPathFileName = userAttestationFormLocation + destFileName;
                        File.Copy(newTempFilePath, pdfDocPathFileName);
                        newFinalFilePath = pdfDocPathFileName;
                    }
                    else
                    {
                        userAttestationFormLocation = ConfigurationManager.AppSettings["UserAttestationFormLocation"];
                        //if (!userAttestationFormLocation.EndsWith("//"))
                        //{
                        //    userAttestationFormLocation += "//";
                        //}
                        //AWS code to save document to S3 location
                        AmazonS3Documents objAmazonS3 = new AmazonS3Documents();
                        String returnFilePath = objAmazonS3.SaveDocument(newTempFilePath, destFileName, userAttestationFormLocation);
                        newFinalFilePath = returnFilePath; //Path.Combine(destFolder, destFileName);
                    }

                    if (!String.IsNullOrEmpty(newTempFilePath))
                        File.Delete(newTempFilePath);

                    if (!String.IsNullOrEmpty(partiallyFilledDocumentObj.UAD_DocumentPath))
                        File.Delete(partiallyFilledDocumentObj.UAD_DocumentPath);

                    //Updating fully filled User Attestation Document 
                    partiallyFilledDocumentObj.UAD_OrganizationUserID = orgUserID;
                    partiallyFilledDocumentObj.UAD_FileName = filename;
                    partiallyFilledDocumentObj.UAD_Size = filesize;
                    partiallyFilledDocumentObj.UAD_Description = "User Attestation Document";
                    partiallyFilledDocumentObj.UAD_DocumentPath = newFinalFilePath;
                    partiallyFilledDocumentObj.UAD_IsDeleted = false;
                    partiallyFilledDocumentObj.UAD_IsActive = true; //For Esigned Filled Document
                    partiallyFilledDocumentObj.UAD_DocumentTypeID = GetDocumentTypeIDByCode(documentTypeCode);
                    partiallyFilledDocumentObj.UAD_ModifiedByID = orgUserID;
                    partiallyFilledDocumentObj.UAD_ModifiedOn = DateTime.Now;

                    return BALUtils.GetSecurityRepoInstance().SaveUpdateUserAttestationDocument(partiallyFilledDocumentObj, true);

                }
                else
                {
                    return new UserAttestationDetail();
                }
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            finally
            {
                try { _FileStream.Close(); }
                catch (Exception)
                {
                    BALUtils.LogError("Error while closing fileStream", new SystemException());
                }
            }
        }

        /// <summary>
        /// Method to get partially Filled User Attestation Document
        /// </summary>
        /// <param name="userAttestationDocumentID"></param>
        /// <returns></returns>
        public static UserAttestationDetail GetPartiallyFilledUserAttestationDocument(Int32 userAttestationDocumentID)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetPartiallyFilledUserAttestationDocument(userAttestationDocumentID);
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

        /// <summary>
        /// Method to Insert e-sign into PDF
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="bufferSignature"></param>
        /// <returns></returns>
        public static byte[] InsertSignatureToAttestationDocument(byte[] pdfDocumentDataToBeFilledIn, byte[] imageToAddToDocument)
        {
            byte[] signedDocument = null;

            try
            {
                PdfReader reader = new PdfReader(pdfDocumentDataToBeFilledIn);
                MemoryStream ms = new MemoryStream();
                PdfStamper stamper = new PdfStamper(reader, ms);
                AcroFields.FieldPosition signatureImagePosition = null;

                //Fill-in the form values
                AcroFields af = stamper.AcroFields;

                af.SetField("CopyRequestedCheckbox", "Yes");
                stamper.FormFlattening = true;
                float left = 0;
                float right = 0;
                float top = 0;
                float heigth = 0;

                try
                {
                    signatureImagePosition = af.GetFieldPositions("SignatureImage")[0];
                }
                catch
                {

                }
                if (signatureImagePosition != null && imageToAddToDocument != null)
                {
                    left = signatureImagePosition.position.Left;
                    right = signatureImagePosition.position.Right;
                    top = signatureImagePosition.position.Top;
                    heigth = signatureImagePosition.position.Height;

                    iTextSharp.text.Image signatureImage = iTextSharp.text.Image.GetInstance(imageToAddToDocument);
                    //PdfContentByte contentByte = stamper.GetOverContent(1);
                    PdfContentByte contentByte = stamper.GetOverContent(signatureImagePosition.page); // uat - 856 : WB: Signature on disclosure is not placing properly when there are multiple pages in the document
                    float currentImageHeigth = 0;
                    currentImageHeigth = signatureImage.Height;
                    float ratio = 0;
                    ratio = heigth / currentImageHeigth;
                    float width = signatureImage.Width * ratio;
                    signatureImage.ScaleAbsoluteHeight(heigth);
                    signatureImage.ScaleAbsoluteWidth(width);
                    signatureImage.SetAbsolutePosition(left, top - signatureImage.ScaledHeight);
                    contentByte.AddImage(signatureImage);
                }
                stamper.Close();
                signedDocument = ms.ToArray();
                ms.Close();

                //Recompress final document to further shrink.
                return ComplianceSetupManager.CompressPDFDocument(signedDocument);

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }

        }

        /// <summary>
        /// Method to check if user has already submitted Attestation Disclosure Form
        /// </summary>
        /// <param name="orgUserID"></param>
        /// <returns></returns>
        public static Boolean IsAttestationDocumentAlreadySubmitted(Int32 orgUserID)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().IsAttestationDocumentAlreadySubmitted(orgUserID);
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

        /// <summary>
        /// Method to fill User Emplyment Disclosure Document with pre-required data
        /// </summary>
        /// <param name="orgUserID"></param>
        /// <param name="documentType"></param>
        /// <returns></returns>
        public static byte[] FillEmploymentDisclosureWithPrePopulatedData(String documentPath, Int32 orgUserID)
        {
            try
            {
                byte[] emptyPdfBuffer = CommonFileManager.RetrieveDocument(documentPath, FileType.SystemDocumentLocation.GetStringValue());
                byte[] updatedDocument = null;

                //Call SP
                List<GetEmploymentDocumentUserInfo_Result> employmentDisclosureUserInfo = BALUtils.GetSecurityRepoInstance().GetEmploymentDisclosureUserInfo(orgUserID);

                if (!employmentDisclosureUserInfo.IsNullOrEmpty())
                {
                    PdfReader reader = new PdfReader(emptyPdfBuffer);
                    MemoryStream ms = new MemoryStream();
                    PdfStamper stamper = new PdfStamper(reader, ms);

                    //Fill-in the form values
                    AcroFields af = stamper.AcroFields;
                    foreach (var item in employmentDisclosureUserInfo)
                    {
                        if (af.GetField(item.FieldName).IsNotNull())
                        {
                            af.SetField(item.FieldName, item.FieldValue);
                        }
                    }
                    //stamper.FormFlattening = true;
                    stamper.Close();
                    updatedDocument = ms.ToArray();
                    ms.Close();
                    //Recompress final document to further shrink.
                    updatedDocument = ComplianceSetupManager.CompressPDFDocument(updatedDocument);
                    reader.Close();
                }
                return updatedDocument;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
        }



        /// <summary>
        /// UAT 1204: Issue with Manage Users, and Manage Users from Manage institution
        /// </summary>
        /// <param name="sortingAndFilteringData"></param>
        /// <returns></returns>
        public static List<OrganizationUserContract> GetMappedOrganizationUsers(Int32? currentUserId, Int32? tenantProductId, Boolean? isAdmin, Int32? organizationID,
                                       Boolean? isApplicantCheckRequred, Boolean? isParentOrgCheckRequired, String roleId, CustomPagingArgsContract sortingAndFilteringData)
        {
            try
            {
                DataTable result = BALUtils.GetSecurityRepoInstance().GetMappedOrganizationUsers(currentUserId, tenantProductId, isAdmin, organizationID, isApplicantCheckRequred, isParentOrgCheckRequired, roleId, sortingAndFilteringData);
                return AssignValuesToMannageUserViewModel(result);

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

        /// <summary>
        /// Method is used to convert Datatable to OrganisationUser list.
        /// UAT 1204: Issue with Manage Users, and Manage Users from Manage institution
        /// </summary>
        /// <param name="result">Datatable</param>
        /// <returns>List of Organisation Users</returns>
        private static List<OrganizationUserContract> AssignValuesToMannageUserViewModel(DataTable result)
        {
            try
            {
                IEnumerable<DataRow> rows = result.AsEnumerable();
                return rows.Select(x => new OrganizationUserContract
                {
                    FirstName = Convert.ToString(x["FirstName"]),
                    LastName = Convert.ToString(x["LastName"]).Trim(),
                    UserName = Convert.ToString(x["UserName"]),
                    MobileAlias = Convert.ToString(x["MobileAlias"]),
                    LastActivityDate = Convert.ToString(x["LastActivityDate"]),
                    OrganizationName = Convert.ToString(x["OrganizationName"]),
                    OrganizationID = Convert.ToInt32(x["OrganizationID"]),
                    CreatedByUserName = Convert.ToString(x["CreatedByUserName"]),
                    IsLockedOut = Convert.ToBoolean(x["IsLockedOut"]),
                    IsActive = Convert.ToBoolean(x["IsActive"]),
                    Email = Convert.ToString(x["Email"]).Trim(),
                    TenantTypeCode = Convert.ToString(x["TenantTypeCode"]).Trim(),
                    TenantID = Convert.ToInt32(x["TenantID"]),
                    CreatedByID = Convert.ToInt32(x["CreatedByID"]),
                    IsSystem = Convert.ToBoolean(x["IsSystem"]),
                    IsApplicant = Convert.ToBoolean(x["IsApplicant"]),
                    OrganizationUserID = Convert.ToInt32(x["OrganizationUserID"]),
                    IsInternationalPhoneNumber = Convert.ToBoolean(x["IsInternationalPhoneNumber"]),//UAT-2447
                    UserID = (Guid)(x["UserID"])
                }).ToList();
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



        /// <summary>
        /// UAT 1230
        /// </summary>
        /// <param name="accountCreationContact"></param>
        /// <param name="dictMailData"></param>
        /// <param name="systemEventTemplatesContract"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static Boolean SaveAccountCreationContact(List<Entity.AccountCreationContact> accountCreationContactList, SystemEventTemplatesContract SystemEventTemplate,
                                                          Int32 SubEventID, Int32 CurrentLoggedInUserId, Int32 tenantID, List<CommunicationTemplatePlaceHolder> placeHoldersToFetch)
        {
            try
            {
                if (BALUtils.GetSecurityRepoInstance().SaveAccountCreationContactList(accountCreationContactList))
                {
                    String applicationUrl = WebSiteManager.GetInstitutionUrl(tenantID);
                    List<SystemCommunication> lstSystemCommunicationToBeSaved = new List<SystemCommunication>();
                    foreach (Entity.AccountCreationContact accountCreationContact in accountCreationContactList)
                    {
                        Dictionary<String, String> dictMailData = new Dictionary<string, String>();
                        dictMailData.Add(EmailFieldConstants.APPLICANT_NAME, string.Concat(accountCreationContact.ACC_FirstName, " ", accountCreationContact.ACC_LastName));
                        dictMailData.Add(EmailFieldConstants.INSTITUTION_URL, string.Concat(applicationUrl));

                        //1. Create entry in [Security] AccountCreationContact table 
                        //2. Create entry in [Messaging] SystemCommunication table 
                        //3. Create entry in [Messaging] SystemCommunicationDelivery table 
                        SystemCommunication systemCommunication = new SystemCommunication();
                        systemCommunication.SenderName = "ADB Account Creation System";
                        systemCommunication.SenderEmailID = Convert.ToString(ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SENDER_EMAIL_ID]);
                        systemCommunication.Subject = SystemEventTemplate.Subject;
                        systemCommunication.CommunicationSubEventID = SubEventID;
                        systemCommunication.CreatedByID = CurrentLoggedInUserId;
                        systemCommunication.CreatedOn = DateTime.Now;
                        systemCommunication.Content = SystemEventTemplate.TemplateContent;
                        //replace the placeholder
                        foreach (var placeHolder in placeHoldersToFetch)
                        {
                            Object obj = dictMailData.GetValue(placeHolder.Property);
                            systemCommunication.Content = systemCommunication.Content.Replace(placeHolder.PlaceHolder, obj.IsNotNull() ? Convert.ToString(obj) : string.Empty);
                        }

                        SystemCommunicationDelivery systemCommunicationDelivery = new SystemCommunicationDelivery();
                        systemCommunicationDelivery.SystemCommunicationTypeID = systemCommunication.SystemCommunicationID;
                        systemCommunicationDelivery.ReceiverOrganizationUserID = accountCreationContact.ACC_ID;
                        systemCommunicationDelivery.RecieverEmailID = accountCreationContact.ACC_Email;
                        systemCommunicationDelivery.RecieverName = accountCreationContact.ACC_FirstName + " " + accountCreationContact.ACC_LastName;
                        systemCommunicationDelivery.IsDispatched = false;
                        systemCommunicationDelivery.IsCC = null;
                        systemCommunicationDelivery.IsBCC = null;
                        systemCommunicationDelivery.CreatedByID = systemCommunication.CreatedByID;
                        systemCommunicationDelivery.CreatedOn = DateTime.Now;
                        systemCommunication.SystemCommunicationDeliveries.Add(systemCommunicationDelivery);
                        lstSystemCommunicationToBeSaved.Add(systemCommunication);
                    }
                    return BALUtils.GetCommunicationRepoInstance().SaveSysCommunicationAndSysDeliveries(lstSystemCommunicationToBeSaved);
                }
                return false;
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

        #region UAT-1218: Any User should be able to be 1 or more of the following: Applicant, Client admin, Agency User, Instructor/Preceprtor

        /// <summary>
        /// Method to check whether the User invited is already existing in DB, If So then return its Organization UserID
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        public static Int32 IsExistingUserInvited(String emailAddress)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().IsExistingUserInvited(emailAddress);
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

        /// <summary>
        /// Method to Get Organization User Type Mapping List on the basis of Organization UserID
        /// </summary>
        /// <param name="organizationUserID"></param>
        /// <returns></returns>
        public static List<OrganizationUserTypeMapping> GetOrganizationUserTypeMapping(Guid userID)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetOrganizationUserTypeMapping(userID);
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

        /// <summary>
        /// Method to Add entry in OrganizationUserTypeMappings
        /// </summary>
        /// <param name="orgUserID"></param>
        /// <param name="userTypeID"></param>
        public static void AddOrganizationUserTypeMapping(Int32 orgUserID, String userTypdeCode)
        {
            try
            {
                Int32 userTypeID = GetUserTypeIDByCode(userTypdeCode);
                BALUtils.GetSecurityRepoInstance().AddOrganizationUserTypeMapping(orgUserID, userTypeID);
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

        /// <summary>
        /// Generic Method to Get UserTypeID based on UserTypeCode.
        /// </summary>
        /// <param name="userTypdeCode"></param>
        /// <returns></returns>
        private static int GetUserTypeIDByCode(String userTypdeCode)
        {
            try
            {
                return LookupManager.GetAllOrgUserTypes().Where(cond => cond.IsActive && cond.OrgUserTypeCode == userTypdeCode).Select(col => col.OrgUserTypeID).FirstOrDefault();
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

        public static Boolean IsUsernameExistInSecurityDB(String userName)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().IsUsernameExistInSecuritytDB(userName);
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
        /// Finds if the sharedUser username is already exists in Security database and get Organization User
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static OrganizationUser GetOrgUserIfUsernameExistInSecuritytDB(String userName)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetOrgUserIfUsernameExistInSecuritytDB(userName);
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
        /// Method to Get All User Type Switch Views
        /// </summary>
        /// <returns></returns>
        public static List<lkpUserTypeSwitchView> GetAllUserTypeSwitchView()
        {
            try
            {
                return LookupManager.GetAllUserTypeSwitchView().Where(cond => cond.UTSV_IsActive).ToList();
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

        //#region UAT 1304 Instructor/Preceptor screens and functionality
        //public static Boolean UpdateClientContactOrganisationUser(INTSOF.ServiceDataContracts.Modules.Common.OrganizationUserContract organizationUserContract, int organisationUserID)
        //{
        //    try
        //    {
        //        return BALUtils.GetSecurityRepoInstance().UpdateClientContactOrganisationUser(organizationUserContract, organisationUserID);
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //} 
        //#endregion

        public static List<ExternalVendorFieldOption> GetExternalVendorFieldOptionByVendorID(int vendorID)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetExternalVendorFieldOptionByVendorID(vendorID);
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

        public static String GetFormattedString(Int32 orgUserID, Boolean isOrgUserProfileID)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetFormattedString(orgUserID, isOrgUserProfileID);
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

        public static List<SystemEntityUserPermissionData> GetSystemEntityUserPermissionData(Int32 organizationUserID)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetSystemEntityUserPermissionData(organizationUserID);
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

        #region Announcement

        /// <summary>
        /// Get Announcement Detail
        /// </summary>
        /// <returns></returns>
        public static List<AnnouncementContract> GetAnnouncementDetail()
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetAnnouncementDetail();
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
        /// Save Update Announcement
        /// </summary>
        /// <param name="announcementContract"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public static Boolean SaveUpdateAnnouncement(AnnouncementContract announcementContract, Int32 currentUserId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().SaveUpdateAnnouncement(announcementContract, currentUserId);
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
        /// Delete Announcement
        /// </summary>
        /// <param name="announcementID"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public static Boolean DeleteAnnouncement(Int32 announcementID, Int32 currentUserId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().DeleteAnnouncement(announcementID, currentUserId);
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
        /// Get Announcement Popup Detail
        /// </summary>
        /// <param name="announcementID"></param>
        /// <returns></returns>
        public static AnnouncementContract GetAnnouncementPopupDetail(Int32 announcementID)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetAnnouncementPopupDetail(announcementID);
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
        /// Save Announcement Mapping
        /// </summary>
        /// <param name="AnnouncementID"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public static Boolean SaveAnnouncementMapping(Int32 announcementID, Int32 currentUserId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().SaveAnnouncementMapping(announcementID, currentUserId);
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
        /// Get Announcements
        /// </summary>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public static List<Int32> GetAnnouncements(Int32 currentUserId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetAnnouncements(currentUserId);
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

        #region Bulletin
        public static List<BulletinContract> GetBulletin(string selectedInstitutionIds)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetBulletin(selectedInstitutionIds);
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

        public static bool SaveUpdateBulletin(BulletinContract bulletinContract, Int32 currentUserId)
        {
            try
            {
                Int32 _bulletinId = BALUtils.GetSecurityRepoInstance().SaveUpdateBulletin(bulletinContract, currentUserId);

                if (!bulletinContract.IsCreatedByADBAdmin)
                {
                    ComplianceDataManager.AddUpdateBulletinNodeMapping(_bulletinId, currentUserId, bulletinContract.LstSelectedDepPrgMappingId, bulletinContract.LstSelectedTenantID.FirstOrDefault());
                    return true;
                }

                if (_bulletinId > 0)
                    return true;

                return false;
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

        public static Boolean DeleteBulletin(Int32 BulletinID, Int32 tenantId, Int32 currentUserId, bool isADBAdmin)
        {
            try
            {
                if (!isADBAdmin)
                    ComplianceDataManager.DeleteBulletinNodeMapping(BulletinID, tenantId, currentUserId);

                return BALUtils.GetSecurityRepoInstance().DeleteBulletin(BulletinID, currentUserId);
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

        #region Bulletins Popup

        /// <summary>
        /// Get Bulletin Popup detail data
        /// </summary>
        /// <param name="bulletinID"></param>
        /// <returns></returns>
        public static BulletinContract GetBulletinPopupDetail(Int32 bulletinID)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetBulletinPopupDetail(bulletinID);
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
        /// Save Bulletin Mapping
        /// </summary>
        /// <param name="bulletinID"></param>
        /// <param name="currentUserID"></param>
        /// <returns></returns>
        public static Boolean SaveBulletinMapping(Int32 bulletinID, Int32 currentUserID)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().SaveBulletinMapping(bulletinID, currentUserID);
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

        /// <summary>
        /// UAT 1529
        /// </summary>
        /// <param name="lstUserIDs"></param>
        /// <returns></returns>
        public static List<AgencyUserPermissionContract> GetOrganizationUserListByUserIds(List<String> lstUserIDs)
        {
            try
            {
                String applicantUserIDsXML = CreateUserIDsXML(lstUserIDs);
                return BALUtils.GetSecurityRepoInstance().GetOrganizationUserListByUserIds(applicantUserIDsXML);
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

        private static string CreateUserIDsXML(List<string> lstUserIDs)
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append("<OrganizationUsers>");
            foreach (string userID in lstUserIDs)
            {
                strBuilder.Append("<OrganizationUser>");
                strBuilder.Append("<UserID>" + userID + "</UserID>");
                strBuilder.Append("</OrganizationUser>");
            }
            strBuilder.Append("</OrganizationUsers>");
            return strBuilder.ToString();
        }

        public static List<ManageRoleContract> GetRolesMappedWithUserType(String selectedUserTypeIds)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetRolesMappedWithUserType(selectedUserTypeIds);
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

        public static void InsertBulkRoleFeatures(String featuresIds, String userTypeIds, String roleDetailIds, Int32 currentLoggedinUserId)
        {
            try
            {
                BALUtils.GetSecurityRepoInstance().InsertBulkRoleFeatures(featuresIds, userTypeIds, roleDetailIds, currentLoggedinUserId);
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

        #region Dummy Complio API

        public static List<APIMetaData> GetAPIMetaDataList()
        {
            try
            {
                return BALUtils.GetSecurityInstance().GetAPIMetaDataList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        public static Boolean UpdateStatusOfAppDocumentForDataEntry(List<Int32> lstUsers, Int32 currentloggedInUserId, Int16 docStatusId, Int32 tenantID)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().UpdateStatusOfAppDocumentForDataEntry(lstUsers, currentloggedInUserId, docStatusId, tenantID);
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
        #region UAT 1560 WB: We should be able to add documents that need to be signed to the order process

        /// <summary>
        /// Methos to Get DocumentTypes
        /// </summary>
        /// <param name="dislkpDocumentType"></param>
        public static List<lkpDocumentType> GetDocumentTypes()
        {
            try
            {
                return LookupManager.GetLookUpData<lkpDocumentType>();
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

        #region Invoice Group

        /// <summary>
        /// Get Invoice Group Details
        /// </summary>
        /// <returns></returns>
        public static List<InvoiceGroupContract> GetInvoiceGroupDetails()
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetInvoiceGroupDetails();
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

        /// <summary>
        /// Get report columns
        /// </summary>
        /// <returns></returns>
        public static List<lkpReportColumn> GetReportColumns()
        {
            try
            {
                return LookupManager.GetLookUpData<lkpReportColumn>().Where(con => !con.RC_IsDeleted).ToList();
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
        /// Save Update Invoice Group Information
        /// </summary>
        /// <param name="currentLoggedInUserId"></param>
        /// <param name="invoiceGroupId"></param>
        /// <param name="invoiceGroupName"></param>
        /// <param name="invoiceGroupDesc"></param>
        /// <param name="selectedDPMIds"></param>
        /// <param name="lstSelectedReportColumnIds"></param>
        /// <returns></returns>
        public static Boolean SaveUpdateInvoiceGroupInformation(Int32 currentLoggedInUserId, Int32 invoiceGroupId, String invoiceGroupName, String invoiceGroupDesc, List<String> selectedDPMIds, List<Int32> lstSelectedReportColumnIds)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().SaveUpdateInvoiceGroupInformation(currentLoggedInUserId, invoiceGroupId, invoiceGroupName, invoiceGroupDesc, selectedDPMIds, lstSelectedReportColumnIds);
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
        /// Delete Invoice Group Information
        /// </summary>
        /// <param name="currentLoggedInUserId"></param>
        /// <param name="invoiceGroupId"></param>
        /// <returns></returns>
        public static Boolean DeleteInvoiceGroupInformation(Int32 currentLoggedInUserId, Int32 invoiceGroupId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().DeleteInvoiceGroupInformation(currentLoggedInUserId, invoiceGroupId);
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

        public static Dictionary<Int32, Boolean> CheckIfItemsAreInReconciliationProcess(List<Int32> itemDataIDs, Int32 tenantID)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().CheckIfItemsAreInReconciliationProcess(itemDataIDs, tenantID);
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

        #region UAT-1741:604 notification should only have to be clicked upon login once per 24 hours.
        public static Boolean IsEDFormPreviouslyAccepted(Int32 organizationUserID, Double employmentDisclosureIntervalHours)
        {
            try
            {
                EmploymentDisclosureDetail employmentDisclosureDetail = BALUtils.GetSecurityRepoInstance().GetEmploymentDisclosureDetails(organizationUserID);
                if (!employmentDisclosureDetail.IsNullOrEmpty())
                {
                    TimeSpan timeDifference = DateTime.Now - employmentDisclosureDetail.EDD_CreatedOn;
                    return (timeDifference.TotalHours < employmentDisclosureIntervalHours);
                }
                return false;
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

        //UAT-2199:As an adb admin, I should be able to edit admin usernames on the manage users screen.
        public static String GetUserIdByEmail(String EmailToCompare)
        {
            try
            {

                return BALUtils.GetSecurityRepoInstance().GetUserIdByEmail(EmailToCompare);
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
        //UAT-2199:As an adb admin, I should be able to edit admin usernames on the manage users screen.
        public static String GetUserIdByUserName(String UserNameToCompare)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetUserIdByUserName(UserNameToCompare);
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

        //UAT-2264
        public static List<ColumnsConfigurationContract> GetScreenColumns(List<String> lstGridCode, Int32 CurrentLoggedInUserID)
        {
            try
            {
                if (!lstGridCode.IsNullOrEmpty())
                {
                    String ScreenName = String.Join(",", lstGridCode);
                    return BALUtils.GetSecurityRepoInstance().GetScreenColumns(ScreenName, CurrentLoggedInUserID);
                }
                return new List<ColumnsConfigurationContract>();
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

        public static Boolean SaveUserScreenColumnMapping(Dictionary<Int32, Boolean> columnVisibility, Int32 CurrentLoggedInUserID, Int32 OrganisationUserID)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().SaveUserScreenColumnMapping(columnVisibility, CurrentLoggedInUserID, OrganisationUserID);
                //return false;
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
        //Method to get column which is hidden on the basis of screen code and current user login.
        public static List<String> GetScreenColumnsToHide(String GridCode, Int32 CurrentLoggedInUserID)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetScreenColumnsToHide(GridCode, CurrentLoggedInUserID);
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

        public static Boolean UpdateFlatDataEntryQueueRecord(Int32 applicantDocumentId, short InProgressDocumentStatus, Int32 OrgID, Int32 tenantID)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().UpdateFlatDataEntryQueueRecord(applicantDocumentId, InProgressDocumentStatus, OrgID, tenantID);
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

        #region UAT-2302-As a student, I should see a popup on login if i have logged in using a non-preferred browser.
        public static Boolean CheckUserOptedNonPrefferedBrowserOption(Int32 currentLoggedInUserID, string utilityFeatureCode)
        {
            try
            {
                Int16 utilityFeatureId = LookupManager.GetLookUpData<Entity.lkpUtilityFeature>().FirstOrDefault(obj => obj.UF_Code.Equals(utilityFeatureCode) && obj.UF_IsDeleted == false).UF_ID;
                return BALUtils.GetSecurityRepoInstance().CheckUserNonPrefferedBrowserOption(currentLoggedInUserID, utilityFeatureId);
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
        /// <summary>
        /// Insert log for Applicant Non-preferred Browser option
        /// </summary>
        /// <param name="currentLoggedInUserID"></param>
        /// <param name="utilityFeatureCode"></param>
        /// <returns></returns>
        public static Boolean SaveNonPreferredBrowserLog(Int32 currentLoggedInUserID, string utilityFeatureCode)
        {
            try
            {
                Int16 utilityFeatureId = LookupManager.GetLookUpData<Entity.lkpUtilityFeature>().FirstOrDefault(obj => obj.UF_Code.Equals(utilityFeatureCode) && obj.UF_IsDeleted == false).UF_ID;
                return BALUtils.GetSecurityRepoInstance().SaveNonPreferredBrowserLog(currentLoggedInUserID, utilityFeatureId);
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

        #region 2266
        public static String GetAssignedUserRoleNames(Int32 OrgUserId, Int32 TenantId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetAssignedUserRoleNames(OrgUserId, TenantId);
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

        #region UAT-2515
        /// <summary>
        /// 
        /// </summary>
        /// <param name="externalID"></param>
        /// <returns></returns>
        public static Tuple<Int32, Int32> ExternalUserTenantId(String externalID, String mappingCode)
        {
            try
            {

                return BALUtils.GetSecurityRepoInstance().ExternalUserTenantId(externalID, mappingCode);
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

        public static List<ExternalLoginDataContract> GetMatchingOrganisationUserList(ExternalLoginDataContract objExternalLoginDataContract)
        {
            try
            {
                return (BALUtils.GetSecurityRepoInstance()).GetMatchingOrganisationUserList(objExternalLoginDataContract);
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

        public static Boolean ValidateToken(String token)
        {
            try
            {

                return BALUtils.GetSecurityRepoInstance().ValidateToken(token);
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

        public static Dictionary<Int32, Int32> ExternalUserTenantIdBySchoolName(String token, String schoolName)
        {
            try
            {

                return BALUtils.GetSecurityRepoInstance().ExternalUserTenantIdBySchoolName(token, schoolName);
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

        public static Boolean InsertExternalUserRegistrationLogInIntegrationClientOrganizationUserMap(Int32 organizationUserId, Int32 integrationClientId, String externalId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().InsertExternalUserRegistrationLogInIntegrationClientOrganizationUserMap(organizationUserId, integrationClientId, externalId);
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
        public static List<ExternalDataFromTokenDataContract> GetDataFromSecurityToken(String tokenId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetDataFromSecurityToken(tokenId);
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

        #region Get ClientDB Config for selected tenant
        public static ClientDBConfiguration GetClientDBConfigurationForSelectedTenants(Int32 TenantId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetClientDBConfigurationForSelectedTenants(TenantId);
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

        #region UAT-2625:
        public static List<lkpDisclosureDocumentAgeGroup> GetAgeGroupTypes()
        {
            try
            {
                return LookupManager.GetLookUpData<lkpDisclosureDocumentAgeGroup>().Where(x => !x.LDDAG_IsDeleted).ToList();
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

        #region UAT-2792:

        public static Boolean SaveShibbolethSSOSessionData(String SessionData, String TargetURL)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().SaveShibbolethSSOSessionData(SessionData, TargetURL);
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

        public static Int32 GetIntegrationClientForShibboleth(String mappingGroupCode)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetIntegrationClientForShibboleth(mappingGroupCode);
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
        public static List<String> GetClientIdpUrl(String ClientUrl)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetClientIdpUrl(ClientUrl);
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
        /// UAT-2883
        /// Gets client name on the basis of URL.
        /// </summary>
        /// <param name="ClientUrl"></param>
        /// <returns></returns>
        public static String GetClientByHostName(String clientUrl)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetClientByHostName(clientUrl);
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
        public static List<ExternalLoginDataContract> GetMatchingUsersForShibboleth(String shibbolethUniqueId, String shibbolethAttributeId, String shibbolethemail, Int32 tenantId, Boolean isApplicant, String shibbolethHandlerType, String ShibbolethFirstName, String ShibbolethLastName, String ShibbolethRoleString)
        {
            try
            {
                return (BALUtils.GetSecurityRepoInstance()).GetMatchingUsersForShibboleth(shibbolethUniqueId, shibbolethAttributeId, shibbolethemail, tenantId, isApplicant, shibbolethHandlerType, ShibbolethFirstName, ShibbolethLastName, ShibbolethRoleString);
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

        public static Boolean GetShibbolethSettingForHistoryLogging()
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetShibbolethSettingForHistoryLogging();
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

        public static Boolean ShibbolethPeopleSoftIDEntryInIntegrationClientOrganizationUserMap(Int32 organizationUserId, Int32 integrationClientId, String externalId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().ShibbolethPeopleSoftIDEntryInIntegrationClientOrganizationUserMap(organizationUserId, integrationClientId, externalId);
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

        #region UAT-2696

        public static Boolean IsCustomAttributeChecked(Int32 organizationUserId, String uniqueName, String grdCode)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().IsCustomAttributeChecked(organizationUserId, uniqueName, grdCode);

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

        #region UAT-2842
        public static OrganizationUser GetOrganizationUserOfAdminOrder(Guid UserID)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetOrganizationUserOfAdminOrder(UserID);
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
        public static Boolean DeleteAdminOrganizationUser(OrganizationUser OrgsUser, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().DeleteAdminOrganizationUser(OrgsUser, currentLoggedInUserId);
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
        public static List<ResidentialHistory> GetUserResidentialHistoriesForAdminOrder(Guid UserId)
        {
            try
            {
                Int32 orgUserId = GetOrganizationUserOfAdminOrder(UserId).OrganizationUserID;
                return BALUtils.GetSecurityRepoInstance().GetUserResidentialHistories(orgUserId);
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

        //UAT-2958
        public static Boolean IsRandomGeneratedPassword(String ShibbolethHandlerType)
        {
            try
            {
                return (BALUtils.GetSecurityRepoInstance()).IsRandomGeneratedPassword(ShibbolethHandlerType);
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

        #region UAT-2918
        public static DataTable GetIntegrationList(Int32 organizationUserId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetIntegrationList(organizationUserId);
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

        #region UAT-2930

        public static IQueryable<Tenant> GetUserTenantsForAlltypesOfUsers(String currentOrgUserId)
        {
            try
            {
                IQueryable<Tenant> tenants = BALUtils.GetSecurityRepoInstance().GetUserTenantsForAlltypesOfUsers(currentOrgUserId);
                tenants.ForEach(item =>
                {
                    item.TenantTypeDescription = (!item.lkpTenantType.IsNull()) ? item.lkpTenantType.TenantTypeDesc : String.Empty;
                });

                return tenants;
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

        public static Boolean SaveTwoFactorAuthenticationData(String UserId, String AuthenticationTitle, String AuthenticationCode, Int32 CurrentLoggedInUserID, String AuthenticationModeCode)
        {
            try
            {
                return (BALUtils.GetSecurityRepoInstance()).SaveTwoFactorAuthenticationData(UserId, AuthenticationTitle, AuthenticationCode, CurrentLoggedInUserID, AuthenticationModeCode);
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

        public static UserTwoFactorAuthentication GetTwofactorAuthenticationForUserID(String userId)
        {
            try
            {
                return (BALUtils.GetSecurityRepoInstance()).GetTwofactorAuthenticationForUserID(userId);
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

        public static Boolean verifyTwofactorAuthenticationForUserID(String UserId, Int32 CurrentLoggedInUserID)
        {
            try
            {
                return (BALUtils.GetSecurityRepoInstance()).VerifyTwofactorAuthenticationForUserID(UserId, CurrentLoggedInUserID);
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


        public static Boolean DeleteTwofactorAuthenticationForUserID(String UserId, Int32 CurrentLoggedInUserID)
        {
            try
            {
                return (BALUtils.GetSecurityRepoInstance()).DeleteTwofactorAuthenticationForUserID(UserId, CurrentLoggedInUserID);
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

        public static Boolean ShowhideTwoFactorAuthentication(String currentOrgUserId)
        {
            List<Tenant> lstTenant = new List<Tenant>();
            lstTenant = GetUserTenantsForAlltypesOfUsers(currentOrgUserId).ToList();
            Boolean needToShowAuthenticationSettings = false;

            if (lstTenant.Count == AppConsts.ONE && lstTenant[0].TenantID == DefaultTenantID)
            {
                needToShowAuthenticationSettings = true;
            }
            else if (!lstTenant.IsNullOrEmpty())
            {
                lstTenant = lstTenant.Where(con => con.TenantID != DefaultTenantID).ToList();
                foreach (var item in lstTenant)
                {
                    List<String> lstCodes = new List<String>();
                    lstCodes.Add(Setting.TWO_FACTOR_AUTHENTICATION_SETTING.GetStringValue());
                    List<Entity.ClientEntity.ClientSetting> lstClientSetting = ComplianceDataManager.GetClientSettingsByCodes(item.TenantID, lstCodes);
                    var _setting = lstClientSetting.FirstOrDefault(cs => cs.lkpSetting.Code == Setting.TWO_FACTOR_AUTHENTICATION_SETTING.GetStringValue());
                    if (!_setting.IsNullOrEmpty() && Convert.ToBoolean(Convert.ToInt32(_setting.CS_SettingValue)))
                    {
                        needToShowAuthenticationSettings = true;
                    }
                }
            }
            return needToShowAuthenticationSettings;
        }

        public static String IsneedToRedirectToGoogleAuthentication(String userId)
        {
            Boolean IsClientSettingsEnabledForAnyTenant = ShowhideTwoFactorAuthentication(userId);
            if (IsClientSettingsEnabledForAnyTenant)
            {

                return (BALUtils.GetSecurityRepoInstance()).GetUserAuthenticationUseTypeForUserID(userId);

                //UserTwoFactorAuthentication UserTwoFactorAuthentication = GetTwofactorAuthenticationForUserID(userId);
                //if (!UserTwoFactorAuthentication.IsNullOrEmpty())
                //{
                //    //clientsetting is on and authentication is enabled but we will rely on verified.
                //    return UserTwoFactorAuthentication.UTFA_IsVerified;
                //}
                //else
                //{
                //    //clientsetting is on but authentication is disabled.
                //    return false;
                //}
            }
            //if clientsetting is off.
            return String.Empty;
        }

        #endregion

        #region UAT-2310:- Tracking Assignment Efficiencies

        public static Boolean SaveAdminsConfig(List<TrackingAssignmentConfigurationContract> lstAdminsConfig, Int32 currentLoggedInUserId)
        {
            try
            {
                //Boolean isTrackConfigSavedSuccessfully = BALUtils.GetSecurityRepoInstance().SaveAdminsConfig(lstAdminsConfig, currentLoggedInUserId);
                ////UAT-3075
                //if (isTrackConfigSavedSuccessfully)
                //{
                //    List<TrackingConfigObjectMappingContract> lstTrackingConfObjMapping = new List<TrackingConfigObjectMappingContract>();
                //    foreach (var item in lstAdminsConfig)
                //    {
                //        foreach (var objMappingItem in item.lstConfigObjMapping)
                //        {
                //            TrackingConfigObjectMappingContract trackingConfObjMapping = new TrackingConfigObjectMappingContract();
                //            trackingConfObjMapping.TCOM_ComplianceObjectID = objMappingItem.TCOM_ComplianceObjectID;
                //            trackingConfObjMapping.TCOM_Priority = objMappingItem.TCOM_Priority;
                //            trackingConfObjMapping.TCOM_ConfigurationID = item.TAC_ID;

                //            lstTrackingConfObjMapping.Add(trackingConfObjMapping);
                //        }
                //    }
                //    return BALUtils.GetSecurityRepoInstance().SaveTrackingConfObjMapping(lstTrackingConfObjMapping, currentLoggedInUserId);
                //}
                //return isTrackConfigSavedSuccessfully;
                return BALUtils.GetSecurityRepoInstance().SaveAdminsConfig(lstAdminsConfig, currentLoggedInUserId);
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

        public static List<TrackingAssignmentConfigurationContract> GetAdminTrackingAssignmentConfiguration()
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetAdminTrackingAssignmentConfiguration();
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

        public static Boolean UpdateConfiguration(TrackingAssignmentConfigurationContract trackingConfigurationContract, Int32 currentLoggedInUserID, List<TrackingConfigObjectMappingContract> lstTrackObjMappingToDelete)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().UpdateConfiguration(trackingConfigurationContract, currentLoggedInUserID, lstTrackObjMappingToDelete);
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

        public static Boolean DeleteConfiguration(Int32 TAC_ID, Int32 currentLoggedInUserID)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().DeleteConfiguration(TAC_ID, currentLoggedInUserID);
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

        #region UAT-3068
        public static String GetUserAuthenticationUseTypeForUserID(String UserId)
        {
            try
            {
                return (BALUtils.GetSecurityRepoInstance()).GetUserAuthenticationUseTypeForUserID(UserId);
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

        public static Boolean SaveAuthenticationData(String userID, String authenticationType, Int32 currentUserId)
        {
            try
            {
                return (BALUtils.GetSecurityRepoInstance()).SaveAuthenticationData(userID, authenticationType, currentUserId);
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

        public static String GetUserIdFromOrgUserId(Int32 currentUserId)
        {
            try
            {
                return (BALUtils.GetSecurityRepoInstance()).GetUserIdFromOrgUserId(currentUserId);
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
        #region UAT-3032 :- Sticky "Institution Selection" for ADB admins.

        public static List<RoleConfigurationContract> GetRolesSetting()
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetRolesSetting();
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

        public static Boolean SaveRolePreferredTenantSetting(RoleConfigurationContract rolePreferredTenantSetting, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().SaveRolePreferredTenantSetting(rolePreferredTenantSetting, currentLoggedInUserId);
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

        public static Boolean DeleteRolePreferredTenantSetting(Int32 rolePreferredTenantSettingId, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().DeleteRolePreferredTenantSetting(rolePreferredTenantSettingId, currentLoggedInUserId);
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

        public static Int32 GetPreferredSelectedTenant()
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetPreferredSelectedTenant();
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

        public static Boolean IsUserAllowedPreferredTenant(String userID)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().IsUserAllowedPreferredTenant(userID);
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
        #region UAT 3054
        public static Boolean IsStudentEmailDOBLastNameExist(Int32 tenantID, String emailId, String lastName, DateTime? dob)
        {
            try
            {
                return (BALUtils.GetSecurityRepoInstance()).IsStudentEmailDOBLastNameExist(tenantID, emailId, lastName, dob);
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
        public static String GetUserNameByExternald(String externalId, Int32 integrationClientID)
        {
            try
            {
                return (BALUtils.GetSecurityRepoInstance()).GetUserNameByExternald(externalId, integrationClientID);
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

        #region UAT-3067
        public static Dictionary<String, String> GetCustomAttrMappedWithShibbolethAttr(String mappingGroupCode)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetCustomAttrMappedWithShibbolethAttr(mappingGroupCode);
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

        #region UAT-3075:- Tracking assignment Efficiencies updates

        public static List<CompliancePriorityObjectContract> GetCompliancePriorityObjects()
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetCompliancePriorityObjects();
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

        public static Boolean SaveComplPriorityObject(Int32 currentLoggedInUserID, CompliancePriorityObjectContract compPriorityObject)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().SaveComplPriorityObject(currentLoggedInUserID, compPriorityObject);
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

        public static Boolean DeleteComplPriorityObject(Int32 currentLoggedInUserID, Int32 compPriorityObjectID)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().DeleteComplPriorityObject(currentLoggedInUserID, compPriorityObjectID);
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

        public static List<TenantUserMappingContract> GetTenantUserMappings()
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetTenantUserMappings();
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

        public static Boolean SaveTenantUserMapping(List<TenantUserMappingContract> lstTenantUserMappings, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().SaveTenantUserMapping(lstTenantUserMappings, currentLoggedInUserId);
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

        public static Boolean UpdateTenantUserMapping(Int32 currentLoggedInUserID, TenantUserMappingContract tenantUserMapping)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().UpdateTenantUserMapping(currentLoggedInUserID, tenantUserMapping);
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

        public static List<Int32> GetUsersMappedWithTenant(Int32 tenantID)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetUsersMappedWithTenant(tenantID);
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

        public static Boolean DeleteTenantUserMapping(Int32 currentLoggedInUserID, Int32 tenantUserMappingId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().DeleteTenantUserMapping(currentLoggedInUserID, tenantUserMappingId);
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

        public static List<TrackingConfigObjectMappingContract> GetTrackConfigObjectMapped(Int32 trackConfigId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetTrackConfigObjectMapped(trackConfigId);
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


        //UAT-3133
        public static Boolean IsIntegrationClientOrganisationUser(Int32 userID, String MAPPING_GROUP_CODE_UCONN)
        {
            try
            {
                return (BALUtils.GetSecurityRepoInstance()).IsIntegrationClientOrganisationUser(userID, MAPPING_GROUP_CODE_UCONN);
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

        #region UAT-3112
        public static List<SystemDocument> GetBadgeFormDocuments()
        {
            try
            {
                Int32 docTypeID = GetDocumentTypeIDByCode(DislkpDocumentType.BADGE_FORM.GetStringValue());
                return BALUtils.GetSecurityRepoInstance().GetBadgeFormDocuments(docTypeID);
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

        public static List<BookmarkedFeatureContact> GetAccessibleFeatures(Int32 orgUserID, Int32 tenantID, Int32 sysxBlockID, bool isSuperAdmin)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetAccessibleFeatures(orgUserID, tenantID, sysxBlockID, isSuperAdmin);
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

        public static Boolean SaveUpdateBookmarkedFeatures(Int32? tenantProductSysXBlockID, Int32 productFeatureID, Int32 orgUserID, bool isBookmarked)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().SaveUpdateBookmarkedFeatures(tenantProductSysXBlockID, productFeatureID, orgUserID, isBookmarked);
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

        #region [UAT-3326] Creation of internal notes function (like on portfolio details) for client admins on the Client User Details screen.
        public static List<ApplicantProfileNotesContract> GetAdminProfileNotesList(Int32 clientAdminID)
        {
            try
            {
                DataTable tempDataTable = BALUtils.GetSecurityRepoInstance().GetAdminProfileNotesList(clientAdminID);
                IEnumerable<DataRow> rows = tempDataTable.AsEnumerable();
                return rows.Select(x => new ApplicantProfileNotesContract
                {
                    APN_ID = x["APN_ID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["APN_ID"]),
                    APN_CreatedBy = x["APN_CreatedBy"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["APN_CreatedBy"]),
                    APN_CreatedOn = x["APN_CreatedOn"].GetType().Name == "DBNull" ? DateTime.MinValue : Convert.ToDateTime(x["APN_CreatedOn"]),
                    APN_ModifiedOn = x["APN_ModifiedOn"].GetType().Name == "DBNull" ? null : (DateTime?)x["APN_ModifiedOn"],
                    APN_ModifiedBy = x["APN_ModifiedBy"].GetType().Name == "DBNull" ? null : (Int32?)x["APN_ModifiedBy"],
                    APN_ProfileNote = x["APN_ProfileNotes"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(x["APN_ProfileNotes"]),
                    CreatedBy = x["CreatedBy"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(x["CreatedBy"]),
                    APN_OrganizationUserID = x["APN_OrganizationUserID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["APN_OrganizationUserID"]),
                    APN_IsDeleted = x["APN_IsDeleted"].GetType().Name == "DBNull" ? false : Convert.ToBoolean(x["APN_IsDeleted"])
                }).ToList();
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

        public static Boolean SaveUpdateAdminProfileNotes(List<ApplicantProfileNotesContract> applicantProfileNoteList)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().SaveUpdateAdminProfileNotes(applicantProfileNoteList);
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

        public static Boolean SaveAdminProfileNotes(AdminProfileNote adminProfileNoteToSave)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().SaveAdminProfileNotes(adminProfileNoteToSave);
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

        public static AdminProfileNote GetAdminProfileNotesByNoteID(Int32 adminProfileNoteID)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetAdminProfileNotesByNoteID(adminProfileNoteID);
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

        public static Boolean UpdateAdminProfileNote()
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().UpdateAdminProfileNote();
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


        #region UAT-3319
        public static List<AgencyClientContact> GetClientDataForAgencyAndAgencyHierarchy(Int32 LoggedInUserId, String AGencyID, String Tenantid, CustomPagingArgsContract customPagingArgsContract)
        {
            try
            {
                return SetDataForAgencyClientContact(BALUtils.GetProfileSharingRepoInstance().GetClientDataForAgencyAndAgencyHierarchyUsingFlatTable(AGencyID, Tenantid, customPagingArgsContract));
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


        private static List<AgencyClientContact> SetDataForAgencyClientContact(DataTable table)
        {
            try
            {
                IEnumerable<DataRow> rows = table.AsEnumerable();
                return rows.Select(x => new AgencyClientContact
                {
                    OrganizationUserID = Convert.ToInt32(x["FSRD_OrganizationUserID"]),
                    Institute = Convert.ToString(x["FSRD_Institution"]),
                    Email = Convert.ToString(x["FSRD_Email"]),
                    Name = Convert.ToString(x["FSRD_Name"]),
                    Phone = Convert.ToString(x["FSRD_PhoneNumber"]),
                    Agency = Convert.ToString(x["FSRD_Agency"]),
                    TotalCount = Convert.ToInt32(x["TotalCount"]),
                    DateOfLastShare = Convert.ToDateTime(x["DateOfLastShare"]) //UAT-3801
                }).ToList();
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



        #region UAT-3364
        public static List<SystemEntityUserPermission> GetRotationCreatorGranularPermissionsByOrgUserID(Int32 orgUserID)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetRotationCreatorGranularPermissionsByOrgUserID(orgUserID);
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

        #region UAT-3347 : "Client Login" functionality from new screen

        public static List<ClientLoginSearchContract> GetClientLoginSearchData(String tenantIDList, ClientLoginSearchContract clientUserSearchContract, CustomPagingArgsContract customPagingArgsContract)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetClientLoginSearchData(tenantIDList, clientUserSearchContract, customPagingArgsContract);
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

        public static Boolean AddImpersonationHistory(Int32 clientAdminUserID, Int32 CurrentLoggedInUserID)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().AddImpersonationHistory(clientAdminUserID, CurrentLoggedInUserID);
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

        public static List<LookupContract> GetExistingUserProfileLists(String userName, String email = null)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetExistingUserProfileLists(userName, email, DefaultTenantID);
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
        /// <summary>
        /// Finds if the sharedUser username is already exists in Security database and get Organization User
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static OrganizationUser GetOrgUserIfUsernameExistInSecuritytDBForAccountLinking(String userName)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetOrgUserIfUsernameExistInSecuritytDBForAccountLinking(userName);
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


        public static List<OrganizationUser> GetOrgUserListIfUsernameExistInSecuritytDBForAccountLinking(String userName)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetOrgUserListIfUsernameExistInSecuritytDBForAccountLinking(userName);
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

        #region UAT-3540
        public static Boolean CheckRoleForShibbolethNYU(Boolean IsApplicantRoleCheck, String Roles)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().CheckRoleForShibbolethNYU(IsApplicantRoleCheck, Roles);
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

        public static Boolean CheckRoleForShibbolethUCONN(Boolean IsApplicantRoleCheck, String Roles)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().CheckRoleForShibbolethUCONN(IsApplicantRoleCheck, Roles);
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

        public static Int32 GetTenantIdByOrganizationUserID(Int32 organizationUserId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetTenantIdByOrganizationUserID(organizationUserId);
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

        #region UAT-3461
        //public static List<INTSOF.UI.Contract.QueueManagement.ManageRandomReviewsContract> GetManageRandomReviewsList(Int32? queueConfigurationID = AppConsts.NONE, Int32? tenantId = AppConsts.NONE)
        //{
        //    try
        //    {
        //        return BALUtils.GetSecurityRepoInstance().GetManageRandomReviewsList(queueConfigurationID, tenantId);
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //}

        public static Boolean DeleteReconciliationQueueConfiguration(Int32 queueConfigurationID, Int32 currentLoggedInID)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().DeleteReconciliationQueueConfiguration(queueConfigurationID, currentLoggedInID);
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

        //public static String SaveReconciliationQueueConfiguration(Int32 queueConfigurationID, Int32 tenantID, String description, Decimal percentage, Int32 reviews, Int32 currentLoggedInID)
        //{
        //    try
        //    {
        //        return BALUtils.GetSecurityRepoInstance().SaveReconciliationQueueConfiguration(queueConfigurationID, tenantID, description, percentage, reviews, currentLoggedInID);
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //}

        #region UAT-4114
        /// <summary>
        /// Fetch All the Random Review/Reconciliation configuration records
        /// </summary>
        /// <returns></returns>
        public static List<INTSOF.UI.Contract.QueueManagement.ManageRandomReviewsContract> GetAllManageRandomReviewsList()
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetAllManageRandomReviewsList();
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
        #endregion

        #region CBI/ABS
        public static Boolean IsLocationServiceTenant(Int32 tenantID)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().IsLocationServiceTenant(tenantID);
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
        public static Boolean CheckIfUserIsEnroller(string userID)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().CheckIfUserIsEnroller(userID);
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

        public static string GetLocationTenantCompanyName()
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetLocationTenantCompanyName();
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

        public static List<Tenant> GetListOfTenantWithLocationService()
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetListOfTenantWithLocationService();
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

        public static int GetTenantID(int webSiteId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetTenantID(webSiteId);
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

        public static List<lkpSuffix> GetSuffixes()
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetSuffixes();
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

        public static List<lkpAdminEntrySuffix> GetAdminEntrySuffixes()
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetAdminEntrySuffixes();
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

        public static Boolean IsTokenExpired(Int32 tenantID, Int32 orderID, String TokenKey)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().IsTokenExpired(tenantID, orderID, TokenKey);
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

        public static string GetCountryByCountryId(int countryId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetCountryByCountryId(countryId);
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

        #region UAT-3669
        public static Boolean UpdateBlockedOrdersHistoryData(Int32 TenantId, Int32 selectedHierarchyNodeID, String selectedPackageIds, Int32 applicantOrgUserId, String firstName, String lastName, String blockedReasonCode)
        {
            try
            {
                List<Entity.lkpBlockedOrderReason> alllkpBlockedOrderReason = LookupManager.GetLookUpData<Entity.lkpBlockedOrderReason>().ToList();
                if (!alllkpBlockedOrderReason.IsNullOrEmpty())
                {
                    Int32 blockedReasonId = alllkpBlockedOrderReason.Where(con => con.LBOR_Code == blockedReasonCode && !con.LBOR_IsDeleted).Select(se => se.LBOR_ID).FirstOrDefault();

                    return BALUtils.GetSecurityRepoInstance().UpdateBlockedOrdersHistoryData(TenantId, selectedHierarchyNodeID, selectedPackageIds, applicantOrgUserId, firstName, lastName, blockedReasonId);
                }
                return false;
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

        #region UAT-3734

        public static string GetLocTenMaxAllowedDays()
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetLocTenMaxAllowedDays();
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

        #region UAT-3737

        public static String GetInstructorNameByOrganizationUserId(Int32 organizationUserId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetInstructorNameByOrganizationUserId(organizationUserId);
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

        #region UAT-3824 :- Code to save user language mapping
        public static Boolean AddUpdateLanguageMapping(OrganizationUser orgUser, Int32? SelectedCommLang = null)
        {
            return BALUtils.GetSecurityRepoInstance().AddUpdateLanguageMapping(orgUser, SelectedCommLang);
        }

        public static List<Entity.lkpLanguage> GetCommLang()
        {
            try
            {
                return LookupManager.GetLookUpData<Entity.lkpLanguage>().ToList();

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
        public static Int32? GetSelectedlang(Guid UserID)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetSelectedlang(UserID);

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

        public static int GetSuffixIdBasedOnSuffixText(string suffix)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetSuffixIdBasedOnSuffixText(suffix);

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

        #region UAT-3910
        public static List<LookupContract> GetLocationSpecifictenantAllCountriesList(Boolean isStateSearch, Int32 countryId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetLocationSpecifictenantAllCountriesList(isStateSearch, countryId);
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
        public static List<LookupContract> GetLocationSpecifictenantAllStatesByCountryId(Boolean isStateSearch, Int32 countryId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetLocationSpecifictenantAllCountriesList(isStateSearch, countryId);
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
        public static Boolean IsCountryUSACanadaMexico(Int32 countryIdLocationServiceTenant)
        {
            try
            {
                List<CBICountryData> allCBICountries = LookupManager.GetLookUpData<Entity.CBICountryData>().ToList();
                if (!allCBICountries.IsNullOrEmpty())
                {
                    CBICountryData cbiCountryData = allCBICountries.Where(con => con.Id == countryIdLocationServiceTenant).FirstOrDefault();
                    if (!cbiCountryData.IsNullOrEmpty())
                    {
                        if (cbiCountryData.StateType.ToLower() == "canada"
                            || cbiCountryData.StateType.ToLower() == "mexico"
                            || cbiCountryData.StateType.ToLower() == "united states of america - state")
                        {
                            return true;
                        }
                    }
                }
                return false;
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
        #region Globalization for multi-Language

        public static LanguageContract GetLanguageCulture(Guid userId)
        {
            try
            {
                var _lkpLanguages = BALUtils.GetSecurityRepoInstance().GetLanguageCulture(userId);
                LanguageContract _lanContract = null;

                if (_lkpLanguages.IsNotNull())
                {
                    _lanContract = new LanguageContract();
                    _lanContract.LanguageID = _lkpLanguages.LAN_ID;
                    _lanContract.LanguageName = _lkpLanguages.LAN_Name;
                    _lanContract.LanguageCode = _lkpLanguages.LAN_Code;
                    _lanContract.LanguageCulture = _lkpLanguages.LAN_Culture;
                }
                return _lanContract;
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

        //public static List<lkpLanguage> GetCommLang()
        //{
        //    try
        //    {
        //        return LookupManager.GetLookUpData<Entity.lkpLanguage>().ToList();

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

        #region UAT-4097
        public static Boolean IsUserExixtInLocationTenants(Guid currentUserId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().IsUserExixtInLocationTenants(currentUserId);
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
        public static Boolean IsPasswordNeedToBeChanged(Guid userId, Int32 expiryDays)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().IsPasswordNeedToBeChanged(userId, expiryDays);
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

        #region UAT-4159
        public static List<ExternalLoginDataContract> GetMatchingOrganisationUserListForCoreLinking(ExternalLoginDataContract objExternalLoginDataContract)
        {
            try
            {
                return (BALUtils.GetSecurityRepoInstance()).GetMatchingOrganisationUserListForCoreLinking(objExternalLoginDataContract);
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

        #region UAT 4151

        public static bool LinkOtherAccount(Guid SourceUserID, Guid TargetUserID, Int32 currentLoggedInUserId)
        {
            try
            {
                Boolean IsAccountLinked = BALUtils.GetSecurityRepoInstance().LinkOtherAccount(SourceUserID, TargetUserID, currentLoggedInUserId);
                //UAT-4407:- Currently we will not copy data on linking
                //if (IsAccountLinked)
                //{
                //SecurityManager.RotationDataMovementOnAccountLinking(SourceUserID, currentLoggedInUserId); 
                //}
                return IsAccountLinked;
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
        //UAT-4407:- Currently we will not copy data on linking
        public static bool RotationDataMovementOnAccountLinking(Guid SourceUserID, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().RotationDataMovementOnAccountLinking(SourceUserID, currentLoggedInUserId);
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

        public static Tuple<LookupContract, Boolean> GetExistingUserBasedOnEmailId(String emailAddress, Guid userid, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetExistingUserBasedOnEmailId(emailAddress, userid, currentLoggedInUserId);
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
        public static Boolean IsOrganizationUserExistsForEmail(String email)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().IsOrganizationUserExistsForEmail(email);
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

        #region UAT-4239
        public static OrganizationUser GetSharedUserOrganizationUser(Guid userId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetSharedUserOrganizationUser(userId);
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

        #region UAT-4270
        public static String GetEnrollerPhoneNumberForSMSNotification(Int32 OrgUserID)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetEnrollerPhoneNumberForSMSNotification(OrgUserID);
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

        #region UAT 4025

        public static String ValidateCBIUniqueIDs(List<String> lstCBIUniqueID)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().ValidateCBIUniqueID(lstCBIUniqueID);
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

        public static String ValidateAccountNames(List<String> lstAccountName)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().ValidateAccountName(lstAccountName);
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

        #region Admin entry Portal


        /// <summary>
        /// Get Admin Entry User Data, based on token key
        /// </summary>
        /// <param name="tokenKey"></param>
        /// <returns></returns>
        public static AdminEntryUserLoginContract GetAdminEntryUserByToken(Guid tokenKey)
        {

            try
            {
                return BALUtils.GetSecurityRepoInstance().GetAdminEntryUserByToken(tokenKey);
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
        /// Method to GetFeatureAreaType. 
        /// </summary>
        /// <returns></returns>
        public static List<lkpFeatureAreaType> GetFeatureAreaType()
        {
            try
            {
                return LookupManager.GetFeatureAreaType();
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
        /// Method to get cross application data using token.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static CrossApplicationData GetSessionUsingToken(Guid? token)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetSessionUsingToken(token);
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
        /// Method To set the cross application data on redirection and after generating new token.
        /// </summary>
        /// <param name="applicationData"></param>
        /// <returns></returns>
        public static Boolean SetSessionData(CrossApplicationData applicationData)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().SetSessionData(applicationData);
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
        /// Method to get the redirect token type id using redirect token type code.
        /// </summary>
        /// <param name="redirectTokenTypeCode"></param>
        /// <returns></returns>

        public static Int32 GetRedirectTokenTypeID(String redirectTokenTypeCode)
        {
            try
            {
                List<lkpRedirectTokenType> lstRedirectTokenType = new List<lkpRedirectTokenType>();
                lstRedirectTokenType = GetRedirectTokenType();

                if (!lstRedirectTokenType.IsNullOrEmpty() && lstRedirectTokenType.Count > AppConsts.NONE)
                {
                    return lstRedirectTokenType.Where(con => con.RTT_Code == redirectTokenTypeCode).FirstOrDefault().RTT_ID;
                }
                return AppConsts.NONE;
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
        /// Method to get look up values for redirect token type.
        /// </summary>
        /// <returns></returns>
        public static List<lkpRedirectTokenType> GetRedirectTokenType()
        {
            try
            {
                return LookupManager.GetLookUpData<Entity.lkpRedirectTokenType>().Where(x => !x.RTT_IsDeleted).ToList();
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


        public static void UpdateSessionActiveState(Guid? token)
        {
            try
            {
                BALUtils.GetSecurityRepoInstance().UpdateSessionActiveState(token);
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
        /// Get the OrganziationUser Details by OrganizationUserId
        /// </summary>
        /// <param name="organizationUserId"></param>
        /// <returns></returns>
        public static Entity.OrganizationUser GetAdminEntryOrganizationUserDetailByOrgUserId(int organizationUserId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetOrganizationUserDetailForAdminOrder(organizationUserId);
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
        public static Boolean UpdateApplicatInviteToken(Int32 tenantId, Int32 orderId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().UpdateApplicatInviteToken(tenantId, orderId);
            }
            catch (INTSOF.Utils.SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new INTSOF.Utils.SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Gets the list of Alias for the given organisation user profile ID.
        /// </summary>
        /// <param name="organizationUserId">Organisation User profile ID</param>
        /// <returns>List of Person Aliases</returns>
        public static List<PersonAliasProfile> GetUserPersonAliasProfiles(Int32 organizationUserProfileId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetUserPersonAliasProfiles(organizationUserProfileId);
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
        #region UAT-4454
        public static Boolean RemoveIntegrationClientOrganizationUserMapping(Int32 IntegrationClientOrganizationUserMapID, Int32 OrganizationUserID)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().RemoveIntegrationClientOrganizationUserMapping(IntegrationClientOrganizationUserMapID, OrganizationUserID);
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

        public static Boolean CheckRoleForShibbolethNSC(Boolean IsApplicantRoleCheck, String Roles)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().CheckRoleForShibbolethNSC(IsApplicantRoleCheck, Roles);
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

        //Release 181:4998
        public static Boolean CheckRoleForShibbolethRoss(Boolean IsApplicantRoleCheck, String Roles)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().CheckRoleForShibbolethRoss(IsApplicantRoleCheck, Roles);
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

        public static Boolean CheckRoleForShibbolethUpennDental(Boolean IsApplicantRoleCheck, String Roles)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().CheckRoleForShibbolethUpennDental(IsApplicantRoleCheck, Roles);
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

        public static Boolean GetMenuItemsForRedirection(string UserID, Int32 BlockID, Int32 BusinessChannelTypeID)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetMenuItemsForRedirection(UserID, BlockID, BusinessChannelTypeID);
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
        public static Boolean IsSystemEntityUserPermissionExists(Int32 organizationUserID, Int32 entityId, Int32? dpmId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().IsSystemEntityUserPermissionExists(organizationUserID, entityId, dpmId);
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

        #region UAT-4592
        public static Entity.SystemDocument GetSystemDocumentByID(Int32 systemDocumentID)
        {
            try
            {

                return BALUtils.GetSecurityRepoInstance().GetSystemDocumentByID(systemDocumentID);
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

        public static Boolean CheckRoleForShibbolethBSU(Boolean IsApplicantRoleCheck, String Roles)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().CheckRoleForShibbolethBSU(IsApplicantRoleCheck, Roles);
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

        public static String CheckRoleForBSU(String Email, String FirstName, String LastName)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().CheckRoleForBSU( Email,  FirstName,  LastName);
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

        /// <summary>
        /// Release 181:4998
        /// </summary>`
        /// <param name="Email"></param>
        /// <param name="FirstName"></param>
        /// <param name="LastName"></param>
        /// <returns></returns>
        public static String CheckRoleForROSS(String email, String firstName, String lastName,String userName)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().CheckRoleForROSS(email, firstName, lastName,userName);
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

        public static List<lkpCabsMailingOption> GetMailingOption()
        {
            try
            {
                return LookupManager.GetLookUpData<lkpCabsMailingOption>().ToList();
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

        public static List<lkpAdditionalServiceFeeType> GetAdditionalServiceFeeOption()
        {
            try
            {
                return LookupManager.GetLookUpData<lkpAdditionalServiceFeeType>().ToList();
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

        public static Int32 GetFeeItemID()
        {
            try
            {

                return BALUtils.GetSecurityRepoInstance().GetFeeItemID();

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

        public static Int32 GetAdditionalServiceFeeItemID()
        {
            try
            {

                return BALUtils.GetSecurityRepoInstance().GetAdditionalServiceFeeItemID();

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

        public static bool InsertReconciliationProductivityData(List<RecounciliationProductivityData> lstRecounciliationProductivityData,int tenantID,int UserId,DateTime startDT)
        {
            try
            {

                return BALUtils.GetSecurityRepoInstance().InsertReconciliationProductivityData(lstRecounciliationProductivityData,tenantID, UserId, startDT);

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

        public static DateTime? GetReconciliationJobHistoryDate(int tenantID)
        {
            try
            {

                return BALUtils.GetSecurityRepoInstance().GetReconciliationJobHistoryDate(tenantID);

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

       


    }
}
