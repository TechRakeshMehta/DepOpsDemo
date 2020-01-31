#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ManageTenant.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;

#endregion

#region Application Specific

#endregion

#endregion

namespace INTSOF.UI.Contract.IntsofSecurityModel
{
    /// <summary>
    /// This contract gets or sets the properties for manage tenant section.
    /// </summary>
    public class ManageTenantContract
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        #endregion

        #endregion

        #region Properties

        // Tenant related properties.

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
        /// Gets or sets the tenant type id.
        /// </summary>
        /// <value>The tenant type id.</value>
        /// <remarks></remarks>
        public Int32 TenantTypeId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the tenant.
        /// </summary>
        /// <value>The name of the tenant.</value>
        /// <remarks></remarks>
        public String TenantName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the tenant description.
        /// </summary>
        /// <value>The tenant description.</value>
        /// <remarks></remarks>
        public String TenantDesc
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the tenant address.
        /// </summary>
        /// <value>The tenant address.</value>
        /// <remarks></remarks>
        public String TenantAddress
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the tenant phone.
        /// </summary>
        /// <value>The tenant phone.</value>
        /// <remarks></remarks>
        public String TenantPhone
        {
            get;
            set;
        }

        // Product related properties.

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        /// <value>The name of the product.</value>
        /// <remarks></remarks>
        public String ProductName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the product description.
        /// </summary>
        /// <value>The product description.</value>
        /// <remarks></remarks>
        public String ProductDescription
        {
            get;
            set;
        }

        // Organization related properties.

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
        /// Gets or sets the name of the organization.
        /// </summary>
        /// <value>The name of the organization.</value>
        /// <remarks></remarks>
        public String OrganizationName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the short name of the organization.
        /// </summary>
        /// <value>The short name of the organization.</value>
        /// <remarks></remarks>
        public String OrganizationShortName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the organization description.
        /// </summary>
        /// <value>The organization description.</value>
        /// <remarks></remarks>
        public String OrganizationDescription
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the organization address.
        /// </summary>
        /// <value>The organization address.</value>
        /// <remarks></remarks>
        public String OrganizationAddress
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the organization city.
        /// </summary>
        /// <value>The organization city.</value>
        /// <remarks></remarks>
        public String OrganizationCity
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the state of the organization.
        /// </summary>
        /// <value>The state of the organization.</value>
        /// <remarks></remarks>
        public Int32 OrganizationState
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the organization zip.
        /// </summary>
        /// <value>The organization zip.</value>
        /// <remarks></remarks>
        public String OrganizationZip
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the identifier of the organization zip.
        /// </summary>
        /// <value>
        /// The identifier of the organization zip.
        /// </value>
        public Int32 OrganizationZipId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the organization phone.
        /// </summary>
        /// <value>The organization phone.</value>
        /// <remarks></remarks>
        public String OrganizationPhone
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the organization tax id.
        /// </summary>
        /// <value>The organization tax id.</value>
        /// <remarks></remarks>
        public String OrganizationTaxId
        {
            get;
            set;
        }

        // Commonly used properties.

        /// <summary>
        /// Gets or sets the created on.
        /// </summary>
        /// <value>The created on.</value>
        /// <remarks></remarks>
        public DateTime CreatedOn
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the modified on.
        /// </summary>
        /// <value>The modified on.</value>
        /// <remarks></remarks>
        public DateTime ModifiedOn
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
        /// Gets or sets the modified by id.
        /// </summary>
        /// <value>The modified by id.</value>
        /// <remarks></remarks>
        public Int32 ModifiedById
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the zip code.
        /// </summary>
        /// <value>The zip code.</value>
        /// <remarks></remarks>
        public String ZipCode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the state id.
        /// </summary>
        /// <value>The state id.</value>
        /// <remarks></remarks>
        public Int32 StateId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the city id.
        /// </summary>
        /// <value>The city id.</value>
        /// <remarks></remarks>
        public Int32 CityId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the zip id.
        /// </summary>
        /// <value>The zip id.</value>
        /// <remarks></remarks>
        public Int32 TenantZipId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public String TenantConnectinString
        {
            set;
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public String TenantDBName
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public String TenantDBServer
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public String TenantDBUserName
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public String TenantDBPassword
        {
            get;
            set;
        }

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