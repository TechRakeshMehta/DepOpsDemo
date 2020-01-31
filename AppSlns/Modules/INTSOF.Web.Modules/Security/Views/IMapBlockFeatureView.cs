#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  IMapBlockFeatureView.cs
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
    /// This interface handles the declaration of variables, properties, methods , events for mapping between block with features.
    /// </summary>
    public interface IMapBlockFeatureView
    {
        #region Variables

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets all product's features.
        /// </summary>
        IQueryable<ProductFeature> ProductFeatures
        {
            set;
        }

        /// <summary>
        /// Gets or Sets all sysx block's features.
        /// </summary>
        IEnumerable<SysXBlocksFeature> SysXBlocksFeatures
        {
            set;
            get;
        }

        /// <summary>
        /// Gets the view contract.
        /// </summary>
        /// <remarks></remarks>
        MapBlockFeatureContract ViewContract
        {
            get;
        }

        /// <summary>
        /// Gets the current user id.
        /// </summary>
        /// <remarks></remarks>
        Int32 CurrentUserId
        {
            get;
        }

        #endregion

        #region Events

        #endregion

        #region Methods

        /// <summary>
        /// This method helps in redirecting the page to manage block section.
        /// </summary>
        void RedirectToManageBlock();

        #endregion

        Int16 BusinessChannelTypeID
        {
            get;
            set;
        }

    }
}