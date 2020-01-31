#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ManageDepartmentPresenter.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using INTSOF.SharedObjects;
using System.Linq;

#endregion

#region Application Specific

using Business.RepoManagers;
using Entity;
using INTSOF.Utils;
using System.Collections.Generic;

#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel.Views
{
    /// <summary>
    /// This class has the method's implementation which performs all the CRUD(Create/ Read/ Update/ Delete) operation for managing departments with its details.
    /// </summary>
    public class ManageDepartmentPresenter : Presenter<IManageDepartmentView>
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
        ///  Performs a delete operation for a department.
        /// </summary>
        public void DeleteDepartment()
        {
            Organization organization = SecurityManager.GetOrganization(View.ViewContract.OrganizationId);

            if (!SecurityManager.IsUserExistsForDepartment(View.ViewContract.OrganizationId).Equals(false))
            {
                throw new SysXException(String.Format("{0}{1}{2}{3}{4}",
                                       SysXUtils.GetMessage(ResourceConst.SECURITY_DEPARTMENT_NOT_REMOVE),
                                       SysXUtils.GetMessage(ResourceConst.SPACE),
                                       organization.OrganizationName,
                                       SysXUtils.GetMessage(ResourceConst.SPACE),
                                       SysXUtils.GetMessage(ResourceConst.SECURITY_FEATURE_DETAILS)));
            }
            else
            {
                organization.IsDeleted = true;
                organization.ModifiedByID = View.CurrentUserId;
                organization.ModifiedOn = DateTime.Now;
                if (SecurityManager.DeleteOrganization(organization))
                {
                    Entity.ClientEntity.Organization clientOrganization = ClientSecurityManager.GetClientOrganizationById(Convert.ToInt32(organization.TenantID), organization.OrganizationID);
                    clientOrganization.IsDeleted = true;
                    clientOrganization.ModifiedByID = organization.ModifiedByID;
                    clientOrganization.ModifiedOn = organization.ModifiedOn;
                    ClientSecurityManager.UpdateOrganizationObject(Convert.ToInt32(organization.TenantID));
                    View.SuccessMessage = SysXUtils.GetMessage(ResourceConst.DEPARTMENT) + SysXUtils.GetMessage(ResourceConst.SPACE) + organization.OrganizationName + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.DELETED_SUCCESSFULLY);
                }
            }
        }

        /// <summary>
        /// Retrieves a list of all departments with it's details.
        /// </summary>
        public void RetrievingDepartment()
        {
            var currentUserOrganizationDetails = SecurityManager.GetOrganization(View.ViewContract.OrganizationId);
            var currentUserOrganizationParentId = Convert.ToInt32(currentUserOrganizationDetails.ParentOrganizationID);
            var currentUserTenantId = Convert.ToInt32(currentUserOrganizationDetails.TenantID);

            // Check Whether the logged in user is Super admin.
            if (View.IsAdmin || currentUserTenantId.Equals(SecurityManager.DefaultTenantID))
            {
                View.ParentOrganizationId = currentUserOrganizationDetails.OrganizationID;
                View.TenantId = currentUserTenantId;
                View.OrganizationDepartments = SecurityManager.GetDepartmentsForSuperAdmin();
            }
            else
            {
                // Check Whether the logged in user is Product admin.
                if (currentUserOrganizationParentId.Equals(AppConsts.NONE))
                {
                    View.ParentOrganizationId = currentUserOrganizationDetails.OrganizationID;
                    View.TenantId = currentUserTenantId;
                    View.OrganizationDepartments = SecurityManager.GetDepartmentsForProductAdmin(View.ProductId);

                }
                // For department admin.
                else
                {
                    View.ParentOrganizationId = currentUserOrganizationParentId;
                    View.TenantId = currentUserTenantId;
                    View.OrganizationDepartments = SecurityManager.GetDepartmentsForDepartmentAdmin(View.ProductId, View.CurrentUserId);
                }
            }
        }

        /// <summary>
        /// Performs an insert operation for a department.
        /// </summary>
        public void DepartmentSave()
        {
            IQueryable<Organization> departmentsByProductId = SecurityManager.GetOrganizationsForProduct(false, View.ProductId);
            Int32 parentOrganizationId = 0;
            Organization organizationTemp = SecurityManager.GetOrganizationsByTenantId(View.TenantId).Where(cond => cond.ParentOrganizationID == null).FirstOrDefault();
            if (View.SelectedTenantId.Equals(SecurityManager.DefaultTenantID))
            {
                parentOrganizationId = organizationTemp.OrganizationID;
            }
            else
            {
                parentOrganizationId = Convert.ToInt32(View.ParentOrganizationId);
            }
            if (departmentsByProductId.IsNull())
            {
                View.ErrorMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_ERR_PRODUCT_IS_DELETED);
            }
            else if (departmentsByProductId.ToList().FindAll(organizationDetails => organizationDetails.OrganizationName.Equals(View.ViewContract.OrganizationName, StringComparison.InvariantCultureIgnoreCase)).Count > AppConsts.NONE)
            {
                View.ErrorMessage = View.ViewContract.OrganizationName + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.SECURITY_DEPARTMENT_EXISTS);
            }
            else
            {
                View.ErrorMessage = String.Empty;
                Organization organization = new Organization
                                                {
                                                    OrganizationName = View.ViewContract.OrganizationName,
                                                    OrganizationDesc = View.ViewContract.OrganizationDesc,
                                                    ParentOrganizationID = parentOrganizationId,
                                                    Tenant = SecurityManager.GetTenant(View.TenantId),
                                                    IsActive = true,
                                                    IsDeleted = false,
                                                    CreatedByID = View.CurrentUserId,
                                                    CreatedOn = DateTime.Now
                                                };
                SecurityManager.AddOrganization(organization);
                if (organization.OrganizationID != 0)
                {

                    Entity.ClientEntity.Organization tempOrganization = new Entity.ClientEntity.Organization
                    {
                        OrganizationName = organization.OrganizationName,
                        OrganizationID = organization.OrganizationID,
                        OrganizationDesc = organization.OrganizationDesc,
                        ParentOrganizationID = parentOrganizationId,
                        TenantID = organization.TenantID,
                        IsActive = true,
                        IsDeleted = false,
                        CreatedByID = View.CurrentUserId,
                        CreatedOn = DateTime.Now
                    };

                    ClientSecurityManager.SaveClientDepartment(tempOrganization, View.TenantId);
                    View.SuccessMessage = SysXUtils.GetMessage(ResourceConst.DEPARTMENT) + SysXUtils.GetMessage(ResourceConst.SPACE) + organization.OrganizationName + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.SAVED_SUCCESSFULLY);
                }
            }
        }

        /// <summary>
        /// Performs an update operation for a department.
        /// </summary>
        public void UpdateDepartment()
        {
            Organization organization = SecurityManager.GetOrganization(View.ViewContract.OrganizationId);
            IQueryable<Organization> departmentsByProductId = SecurityManager.GetOrganizationsForProduct(false, View.ProductId);
            Int32 parentOrganizationId = 0;
            if (View.SelectedTenantId.Equals(SecurityManager.DefaultTenantID))
            {
                Organization organizationTemp = SecurityManager.GetOrganizationsByTenantId(View.TenantId).Where(cond => cond.ParentOrganizationID == null).FirstOrDefault();
                parentOrganizationId = organizationTemp.OrganizationID;
            }
            if (departmentsByProductId.IsNull())
            {
                View.ErrorMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_ERR_PRODUCT_IS_DELETED);
            }
            else if (departmentsByProductId.ToList().FindAll(organizationDetails => organizationDetails.OrganizationName.Equals(View.ViewContract.OrganizationName, StringComparison.InvariantCultureIgnoreCase))
                .SkipWhile(organizationDetails => organization.OrganizationName.Equals(View.ViewContract.OrganizationName, StringComparison.InvariantCultureIgnoreCase))
                .Any(nameChecks => nameChecks.OrganizationName.Equals(View.ViewContract.OrganizationName, StringComparison.InvariantCultureIgnoreCase)))
            {
                View.ErrorMessage = View.ViewContract.OrganizationName + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.SECURITY_DEPARTMENT_EXISTS);
            }
            else
            {
                View.ErrorMessage = String.Empty;
                organization.OrganizationName = View.ViewContract.OrganizationName;
                organization.OrganizationDesc = View.ViewContract.OrganizationDesc;
                if (parentOrganizationId != 0)
                    organization.ParentOrganizationID = parentOrganizationId;
                organization.TenantID = View.TenantId;
                organization.ModifiedOn = DateTime.Now;
                organization.ModifiedByID = View.CurrentUserId;

                if (SecurityManager.UpdateOrganization(organization))
                {

                    Entity.ClientEntity.Organization clientOrganization = ClientSecurityManager.GetClientOrganizationById(View.TenantId, View.ViewContract.OrganizationId);
                    clientOrganization.OrganizationName = organization.OrganizationName;
                    clientOrganization.OrganizationDesc = organization.OrganizationDesc;
                    if (parentOrganizationId != 0)
                        organization.ParentOrganizationID = parentOrganizationId;
                    clientOrganization.TenantID = organization.TenantID;
                    clientOrganization.ModifiedOn = organization.ModifiedOn;
                    clientOrganization.ModifiedByID = organization.ModifiedByID;
                    ClientSecurityManager.UpdateOrganizationObject(View.TenantId);
                    View.SuccessMessage = SysXUtils.GetMessage(ResourceConst.DEPARTMENT) + SysXUtils.GetMessage(ResourceConst.SPACE) + organization.OrganizationName + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.UPDATED_SUCCESSFULLY);
                }
            }
        }

        /// <summary>
        /// Retrieves OrganizationUsers's details.
        /// </summary>
        public void RetrievingOragnizationUserDetails()
        {
            View.ViewContract.OrganizationUserFullName = String.Empty;

            if (View.ViewContract.CreatedById.Equals(AppConsts.NONE))
            {
                return;
            }

            OrganizationUser organizationUser = SecurityManager.GetOrganizationUser(View.ViewContract.CreatedById);

            if (!organizationUser.IsNull())
            {
                View.ViewContract.OrganizationUserFullName = organizationUser.LastName + SysXUtils.GetMessage(ResourceConst.SECURITY_COMMA) + SysXUtils.GetMessage(ResourceConst.SPACE) + organizationUser.FirstName;
            }
            else
            {
                View.ViewContract.OrganizationUserFullName = SysXUtils.GetMessage(ResourceConst.SPACE);
            }
        }

        /// <summary>
        /// Retrieves the application configuration value based on SysXKey.
        /// </summary>
        /// <param name="sysXKey">The sys X key.</param>
        public String GetSysXConfigValue(String sysXKey)
        {
            return SecurityManager.GetSysXConfigValue(sysXKey);
        }


        public void GetTenantList()
        {

            List<Entity.ClientEntity.Tenant> tempTenantList = ClientSecurityManager.getClientTenant().ToList();
            tempTenantList.Insert(0, new Entity.ClientEntity.Tenant { TenantID = 0, TenantName = "-- SELECT --" });
            View.TenantList = tempTenantList;
        }

        //public Boolean IsDepartmentMapped(Int32 organizationId)
        //{
        //    return SecurityManager.IsDepartmentMapped(organizationId);
        //}
        #endregion

        #region Private Methods

        #endregion

        #endregion
    }
}