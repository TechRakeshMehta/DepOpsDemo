#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  IManagePolicyView.ascx..cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System.Linq;
using System.Data.Entity.Core.Objects.DataClasses;

#endregion

#region Application Specific

using Entity;
using INTSOF.UI.Contract.IntsofSecurityModel;

#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel.Views
{
    /// <summary>
    /// This interface handles the declaration of variables, properties, methods , events for managing policies details.
    /// </summary>
    public interface IManagePolicyView
    {
        #region Variables

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the expanded policyset user control.
        /// </summary>
        PolicySetUserControl ExpandedPolicySetUserControl
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the list of all registered controls.
        /// </summary>
        IQueryable<PolicyControl> RegisteredControls
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the value for PolicySets.
        /// </summary>
        PolicySet PolicySets
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the list of controls registered with policies.
        /// </summary>
        IQueryable<PolicyRegisterUserControl> PolicyRegisterControls
        {
            set;
        }

        /// <summary>
        /// Gets or sets policyset user controls.
        /// </summary>
        EntityCollection<PolicySetUserControl> PolicySetUserControls
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets all policy set user controls.
        /// </summary>
        PolicySetUserControl AllPolicySetUserControls
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the view contract.
        /// </summary>
        /// <remarks></remarks>
        MapRolePolicyContract ViewContract
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