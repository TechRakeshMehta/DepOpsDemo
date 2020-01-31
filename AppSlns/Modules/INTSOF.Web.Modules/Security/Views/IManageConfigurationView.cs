#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  IManageConfigurationView.ascx..cs
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
    /// This interface handles the declaration of variables, properties, methods , events for managing configurations.
    /// </summary>
    public interface IManageConfigurationView
    {
        #region Variables

        #endregion

        #region Properties

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
        /// WebConfigurationFullName</summary>
        /// <value>
        /// Gets the value for web configuration full name.</value>
        String WebConfigurationFullName
        {
            get;
        }

        /// <summary>
        /// DbConfigurations</summary>
        /// <value>
        /// Gets or sets the list of all database configuration.</value>
        IQueryable<SysXConfig> DbConfigurations
        {
            set;
        }

        /// <summary>
        /// Sets the value for application configurations.
        /// </summary>
        Dictionary<String, String> AppConfigurations
        {
            set;
        }

        /// <summary>
        /// Gets the view contract.
        /// </summary>
        /// <remarks></remarks>
        ManageConfigurationContract ViewContract
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