#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  IManageDepartmentView.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Linq;
using System.Collections.Generic;

#endregion

#region Application Specific

using Entity;
using INTSOF.UI.Contract.IntsofSecurityModel;


#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel.Views
{
    /// <summary>
    /// This interface handles the declaration of variables, properties, methods , events for managing departments with its information.
    /// </summary>
    public interface IManageDepartmentView
    {
        #region Variables

        #endregion

        #region Properties

        /// <summary>
        /// ProductID</summary>
        /// <value>
        /// Gets  the value for product's id.</value>
        Int32 ProductId
        {
            get;
        }

        /// <summary>
        /// CurrentUserID</summary>
        /// <value>
        /// Gets the value for current user's id.</value>
        Int32 CurrentUserId
        {
            get;
        }

        /// <summary>
        /// Name</summary>
        /// <value>
        /// Gets or sets the value for permission's id.</value>
        Int32 ParentOrganizationId
        {
            get;
            set;
        }

        /// <summary>
        /// TenantID</summary>
        /// <value>
        /// Gets or sets the value for tenant's id.</value>
        Int32 TenantId
        {
            get;
            set;
        }

        /// <summary>
        /// OrganizationDepartments</summary>
        /// <value>
        /// Gets or sets the list of all departments.</value>
        IQueryable<Organization> OrganizationDepartments
        {
            set;
        }

        /// <summary>
        /// ErrorMessage</summary>
        /// <value>
        /// Gets or sets the value for error message.</value>
        String ErrorMessage
        {
            get;
            set;
        }

        /// <summary>
        /// IsAdmin</summary>
        /// <value>
        /// Gets the value for IsAdmin.</value>
        Boolean IsAdmin
        {
            get;
        }

        /// <summary>
        /// Gets the view contract.
        /// </summary>
        /// <remarks></remarks>
        ManageDepartmentContract ViewContract
        {
            get;
        }

        /// <summary>
        /// SuccessMessage</summary>
        /// <value>
        /// Gets or sets the value for Success message.</value>
        String SuccessMessage
        {
            get;
            set;
        }

        /// <summary>
        /// List of Tenant
        /// </summary>
        List<Entity.ClientEntity.Tenant> TenantList 
        {
            get;
            set;
        }

        /// <summary>
        /// Selected ClientId
        /// </summary>
        Int32 SelectedClientId {
            get;
            set;
        }

        /// <summary>
        /// SelectedTenantId
        /// </summary>
         Int32 SelectedTenantId
        {
            get;
        }
        #endregion

        #region Events

        #endregion

        #region Methods

        #endregion
    }
}