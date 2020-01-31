#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ManageDepartment.cs
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
    /// This contract gets or sets the properties for manage department section.
    /// </summary>
    public class ManageDepartmentContract
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// OrganizationID</summary>
        /// <value>
        /// Gets or sets the value for organization's id.</value>
        public Int32 OrganizationId
        {
            get;
            set;
        }

        /// <summary>
        /// OrganizationName</summary>
        /// <value>
        /// Gets or sets the value for organization's name.</value>
        public String OrganizationName
        {
            get;
            set;
        }

        /// <summary>
        /// Description</summary>
        /// <value>
        /// Gets or sets the value for organization's description.</value>
        public String OrganizationDesc
        {
            get;
            set;
        }

        /// <summary>
        /// Address</summary>
        /// <value>
        /// Gets or sets the value for Address.</value>
        public String Address
        {
            get;
            set;
        }

        /// <summary>
        /// City</summary>
        /// <value>
        /// Gets or sets the value for City.</value>
        public String City
        {
            get;
            set;
        }

        /// <summary>
        /// State</summary>
        /// <value>
        /// Gets or sets the value for State.</value>
        public Int32 State
        {
            get;
            set;
        }

        /// <summary>
        /// Zip</summary>
        /// <value>
        /// Gets or sets the value for zip code.</value>
        public String Zip
        {
            get;
            set;
        }

        /// <summary>
        /// Phone</summary>
        /// <value>
        /// Gets or sets the value for phone number.</value>
        public String Phone
        {
            get;
            set;
        }

        /// <summary>
        /// ParentID</summary>
        /// <value>
        /// Gets or sets the value for parent's id.</value>
        public Int32 ParentId
        {
            get;
            set;
        }

        /// <summary>
        /// Active</summary>
        /// <value>
        /// Gets or sets the value for Active.</value>
        public Boolean Active
        {
            get;
            set;
        }

        /// <summary>
        /// Deleted</summary>
        /// <value>
        /// Gets or sets the value for Deleted.</value>
        public Boolean Deleted
        {
            get;
            set;
        }

        /// <summary>
        /// CreatedOn</summary>
        /// <value>
        /// Gets or sets the value for created on date.</value>
        public DateTime CreatedOn
        {
            get;
            set;
        }

        /// <summary>
        /// ModifiedOn</summary>
        /// <value>
        /// Gets or sets the value for modified on date.</value>
        public DateTime ModifiedOn
        {
            get;
            set;
        }

        /// <summary>
        /// CreatedByID</summary>
        /// <value>
        /// Gets or sets the value for created by user's id.</value>
        public Int32 CreatedById
        {
            get;
            set;
        }

        /// <summary>
        /// ModifiedByID</summary>
        /// <value>
        /// Gets or sets the value for modified by user's id.</value>
        public Int32 ModifiedById
        {
            get;
            set;
        }

        /// <summary>
        /// OrganizationUserFullName</summary>
        /// <value>
        /// Gets or sets the value for organization user's full name.</value>
        public String OrganizationUserFullName
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