#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ManageUsers.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

#region Application Specific

#endregion

#endregion

namespace INTSOF.UI.Contract.IntsofSecurityModel
{
    /// <summary>
    /// This contract gets or sets the properties for manage user section.
    /// </summary>
    /// 
    [Serializable]
    public class ManageUsersContract
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the tenant id.
        /// </summary>
        /// <value>The tenant id.</value>
        /// <remarks></remarks>
        public Int32 TenantId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the organization id.
        /// </summary>
        /// <value>The organization id.</value>
        /// <remarks></remarks>
        public Int32 OrganizationId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the organization user id.
        /// </summary>
        /// <value>The organization user id.</value>
        /// <remarks></remarks>
        public Int32 OrganizationUserId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the organization user id.
        /// </summary>
        /// <value>The organization user id.</value>
        /// <remarks></remarks>
        public String UserId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the organization user id.
        /// </summary>
        /// <value>The organization user id.</value>
        /// <remarks></remarks>
        public String ExistingUserName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        /// <remarks></remarks>
        public String UserName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>The first name.</value>
        /// <remarks></remarks>
        public String FirstName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>The last name.</value>
        /// <remarks></remarks>
        public String LastName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        /// <remarks></remarks>
        public String EmailAddress
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        /// <remarks></remarks>
        public String Password
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the default password.
        /// </summary>
        /// <value>The default password.</value>
        /// <remarks></remarks>
        public String DefaultPassword
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the mobile alias.
        /// </summary>
        /// <value>The mobile alias.</value>
        /// <remarks></remarks>
        public String MobileAlias
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the organizations.
        /// </summary>
        /// <value>The organizations.</value>
        /// <remarks></remarks>
        public String Organizations
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the display type of the org.
        /// </summary>
        /// <value>The display type of the org.</value>
        /// <remarks></remarks>
        public String DisplayOrgType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the last activity.
        /// </summary>
        /// <value>The last activity.</value>
        /// <remarks></remarks>
        public DateTime LastActivity
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ManageUsers"/> is active.
        /// </summary>
        /// <value><c>true</c> if active; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
        public Boolean Active
        {
            get;
            set;
        }


        /// Gets or sets a value indicating whether this instance is locked out.
        /// </summary>
        /// <value><c>true</c> if this instance is locked out; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
        public Boolean IsLockedOut
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is messaging use.
        /// </summary>
        /// <value><c>true</c> if this instance is messaging use; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
        public Boolean IsMessagingUse
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is locked update.
        /// </summary>
        /// <value><c>true</c> if this instance is locked update; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
        public Boolean IsLockedUpdate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the assign to role id.
        /// </summary>
        /// <value>The role id.</value>
        /// <remarks></remarks>
        public String AssignToRoleId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the assign to role Name.
        /// </summary>
        /// <value>The role Name.</value>
        /// <remarks></remarks>
        public String[] AssignToRoleName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the assign to product id.
        /// </summary>
        /// <value>The product id.</value>
        /// <remarks></remarks>
        public Int32 AssignToProductId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the assign to department id.
        /// </summary>
        /// <value>The department id.</value>
        /// <remarks></remarks>
        public Int32 AssignToDepartmentId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the value if the link is given on Manage Tenant.
        /// </summary>
        /// <value>yes if the control is getting load on navigating through Manage Tenant else no.</value>
        /// <remarks></remarks>
        public Boolean IsLinkOnTenant
        {
            get;
            set;
        }

        /// <summary>
        /// To verify user navigation for manage user link.
        /// </summary>
        public Boolean IsComingThroughTenant
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the created by id.
        /// </summary>
        /// <value>The created by id.</value>
        /// <remarks></remarks>
        public Int32 CreatedById
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the full name of the created by user.
        /// </summary>
        /// <value>The full name of the created by user.</value>
        /// <remarks></remarks>
        public String CreatedByUserFullName
        {
            get;
            set;
        }

        /// <summary>
        /// Check the Organization/Department For Current User.
        /// </summary>
        public Boolean IsMyOrganizationExists
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the assign to product id.
        /// </summary>
        /// <value>The product id.</value>
        /// <remarks></remarks>
        public Int32 ProductIdFromClientWizard
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the value for Tenant's Name.
        /// </summary>
        /// <value>The tenants name.</value>
        /// <remarks></remarks>
        public String TenantName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or Sets the value for Username's prefix.
        /// </summary>
        public String PrefixName
        {
            get;
            set;
        }

        /// <summary>
        /// Returns true if the current organization is of Supplier else false.
        /// </summary>
        public Boolean IsSupplier
        {
            get;
            set;
        }

        /// <summary>
        /// Returns true if the organization user is Applicant else false.
        /// </summary>
        public Boolean IsApplicant
        {
            get;
            set;
        }

        //UAT-985: WB: Simplification of Admin Account creation.
        /// <summary>
        /// Returns true if the password is new(first time user) else false.
        /// </summary>
        public Boolean IsNewPassword
        {
            get;
            set;
        }

        public Boolean IsInternationalPhoneNumber { get; set; }

        #region Filtering

        public List<String> FilterColumns
        {
            get;
            set;
        }

        public List<String> FilterOperators
        {
            get;
            set;
        }

        public List<String> FilterTypes
        {
            get;
            set;
        }

        public ArrayList FilterValues
        {
            get;
            set;
        }


        #endregion

        #endregion

        #region Events

        #endregion

        #region Methods

        #region Public Methods

        #endregion

        #region Private Methods

        #endregion

        #endregion
    }
}