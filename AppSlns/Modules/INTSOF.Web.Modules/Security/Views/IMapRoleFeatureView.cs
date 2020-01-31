#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  IMapRoleFeatureView.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Collections.Generic;

#endregion

#region Application Specific

using System.Linq;
using Entity;
using INTSOF.UI.Contract.IntsofSecurityModel;

#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel.Views
{
    /// <summary>
    /// This interface handles the declaration of variables, properties, methods , events for mapping between features with roles.
    /// </summary>
    public interface IMapRoleFeatureView
    {
        #region Variables

        #endregion

        #region Properties

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

        #endregion

        /// <summary>
        /// Gets or sets the product's feature.
        /// </summary>
        List<TenantProductFeature> ProductFeatures
        {
            set;
            get;
        }

        /// <summary>
        /// Gets or sets the product's id.
        /// </summary>
        Int32 ProductId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the list of all blocks.
        /// </summary>
        IQueryable<lkpSysXBlock> Blocks
        {
            set;
        }

        /// <summary>
        /// Gets or sets the features for role.
        /// </summary>
        List<RolePermissionProductFeature> RoleFeatures
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the view contract.
        /// </summary>
        /// <remarks></remarks>
        MapRoleFeatureContract ViewContract
        {
            get;
        }

        /// <summary>
        /// UAT-3228
        /// </summary>
        Int32 CurrentUserId
        {
            get;
        }

        #endregion

        #region Events

        event EventHandler<EventArgs> SaveMappingClick;

        #endregion

        #region Methods

        /// <summary>
        /// This method helps in redirecting to manage role page.
        /// </summary>
        void RedirectToManageRole();

        #endregion
    }
}