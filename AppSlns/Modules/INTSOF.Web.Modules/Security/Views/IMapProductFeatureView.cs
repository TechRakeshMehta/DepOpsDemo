#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  IMapProductFeatureView.cs
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
    /// This interface handles the declaration of variables, properties, methods , events for mapping between product and features.
    /// </summary>
    public interface IMapProductFeatureView
    {
        #region Variables

        #endregion

        #region Properties

        Int32 OrganizationUserId { get; }

        #region ClientOnBoardingWizard

        Boolean IsDataLoad
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is client on boarding wizard.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is client on boarding wizard; otherwise, <c>false</c>.
        /// </value>
        Boolean IsClientOnBoardingWizard
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the value of Validation Group.
        /// </summary>
        String ValidationGroup
        {
            get;
        }

        /// <summary>
        /// Sets the mapping success message.
        /// </summary>
        String SetSuccessMessage
        {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// Gets or sets the list of all features.
        /// </summary>
        IEnumerable<SysXBlocksFeature> Features
        {
            set;
            get;
        }

        /// <summary>
        /// Gets or sets the list of all permissions.
        /// </summary>
        List<Permission> Permissions
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the product's features.
        /// </summary>
        IEnumerable<TenantProductFeature> ProductFeatures
        {
            get;
            set;
        }

        /// <summary>
        /// Sets the list of blocks.
        /// </summary>
        IQueryable<lkpSysXBlock> Blocks
        {
            set;
        }

        /// <summary>
        /// Sets the error message.
        /// </summary>
        String SetErrorMessage
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the mapping for features.
        /// </summary>
        IEnumerable<BlockFeaturePermissionMapper> FeatureMappings
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the view contract.
        /// </summary>
        /// <remarks></remarks>
        MapProductFeatureContract ViewContract
        {
            get;
        }

        #endregion

        #region Events

        #endregion

        #region Methods

        /// <summary>
        /// This method helps in redirecting to manage tenant page.
        /// </summary>
        void RedirectToManageTenant();

        /// <summary>
        /// Saves from wizard.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void SaveFromWizard(object sender, EventArgs e);

        #endregion
    }
}