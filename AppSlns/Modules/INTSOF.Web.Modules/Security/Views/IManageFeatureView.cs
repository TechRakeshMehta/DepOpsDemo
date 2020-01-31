#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  IManageFeatureView.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
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
    /// This interface handles the declaration of variables, properties, methods , events for managing features with its details.
    /// </summary>
    public interface IManageFeatureView
    {
        #region Variables

        #endregion

        #region Properties

        /// <summary>
        /// ProductFeatures</summary>
        /// <value>
        /// Sets the value for product feature.</value>
        IEnumerable<ProductFeature> ProductFeatures
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
        /// CurrentUserID</summary>
        /// <value>
        /// Gets the value for current user's id.</value>
        Int32 CurrentUserId
        {
            get;
        }

        /// <summary>
        /// Gets the view contract.
        /// </summary>
        /// <remarks></remarks>
        ManageFeatureContract ViewContract
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

        #endregion

        #region Events

        #endregion

        #region Methods

        #endregion

        #region AMS

        String SelectedBusinessChannel
        {
            get;
            set;
        }

        List<lkpBusinessChannelType> BusinessChannels
        {
            set;
        }

        #endregion

        //Admin Entry Portal
        Boolean IsBkgBusinessChannel { get; set; }
        List<lkpFeatureAreaType> lstFeatureAreaType { get; set; }
    }
}