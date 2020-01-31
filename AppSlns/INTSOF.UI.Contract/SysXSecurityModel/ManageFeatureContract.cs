#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ManageFeature.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Collections.Generic;
using Entity.ClientEntity;

#endregion

#region Application Specific

#endregion

#endregion

namespace INTSOF.UI.Contract.IntsofSecurityModel
{
    /// <summary>
    /// This contract gets or sets the properties for manage features section.
    /// </summary>
    public class ManageFeatureContract
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// MyProperty</summary>
        /// <value>
        /// Gets or sets the value for my property.</value>
        public Int32 MyProperty
        {
            get;
            set;
        }

        /// <summary>
        /// CurrentWebPageName</summary>
        /// <value>
        /// Gets or sets the name of current webpage.</value>
        public String CurrentWebPageName
        {
            get;
            set;
        }

        /// <summary>
        /// ProductFeatureID</summary>
        /// <value>
        /// Gets or sets the value for product feature's id.</value>
        public Int32 ProductFeatureId
        {
            get;
            set;
        }

        /// <summary>
        /// ParentProductFeatureID</summary>
        /// <value>
        /// Gets or sets the value for parent product feature's id.</value>
        public Int32? ParentProductFeatureId
        {
            get;
            set;
        }

        /// <summary>
        /// Name</summary>
        /// <value>
        /// Gets or sets the value for feature's name.</value>
        public String Name
        {
            get;
            set;
        }

        /// <summary>
        /// Description</summary>
        /// <value>
        /// Gets or sets the value for feature's description.</value>
        public String Description
        {
            get;
            set;
        }

        /// <summary>
        /// UIControlID</summary>
        /// <value>
        /// Gets or sets the value for UIControlID.</value>
        public String UIControlId
        {
            get;
            set;
        }

        /// <summary>
        /// NavigationUrl</summary>
        /// <value>
        /// Gets or sets the value for Navigation Url.</value>
        public String NavigationUrl
        {
            get;
            set;
        }

        /// <summary>
        /// IconImageName</summary>
        /// <value>
        /// Gets or sets the value for name of icon image.</value>
        public String IconImageName
        {
            get;
            set;
        }

        /// <summary>
        /// DisplayOrder</summary>
        /// <value>
        /// Gets or sets the value for display order.</value>
        public Int32 DisplayOrder
        {
            set;
            get;
        }

        /// <summary>
        /// OpeninNewbrowser</summary>
        /// <value>
        /// Gets or sets the value for open in new browser.</value>
        public Boolean OpeninNewbrowser
        {
            get { return false; }
            set { }
        }

        //Onsite Changes for Dashboard - Start
        public Boolean IsDashboardFeature
        {
            set;
            get;
        }

        public Boolean ForExternalUser
        {
            set;
            get;
        }

        public string DashboardTemplatesPath
        {
            set;
            get;
        }
        //End
        #endregion

        #region Events

        #endregion

        #region Methods

        #region Public Methods

        #endregion

        #region Private Methods

        #endregion

        #endregion

        public List<String> ActionName
        {
            get;
            set;
        }

        public List<ClsFeatureAction> ActionCollection
        {
            get;
            set;
        }

        public bool? IsReportFeature
        {
            get;
            set;
        }

        public Int32 FeatureAreaTypeID
        {
            get;
            set;
        }

    }
}