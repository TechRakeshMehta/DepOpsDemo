#region Header Comment Block
// Copyright Best X, Inc. 2011
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  IManageCountyRulesView.cs
// Purpose:   
//
// Revisions:
// Author           Date                Comment
// ------           ----------          -------------------------------------------------
// Security Team   09/11/2012           Initial.
//
#endregion

#region Namespaces

#region System Defined

using System;
using System.Linq;

#endregion

#region Application Specific

using BESTX.Entity;
using INTSOF.UI.Contract.IntsofSecurityModel;
using System.Collections.Generic;

#endregion

#endregion

namespace  BESTX.WEB.IntsofSecurityModel.Views
{
    public interface IManageCountyRulesView
    {
        #region Variables

        #endregion

        #region Properties
        /// <summary>
        /// Categories</summary>
        /// <value>
        /// Sets the value for Categories.</value>
        List<RuleCategory> Categories
        {
            get;
            set;
        }
        /// <summary>
        /// sets the grid with the existing county rules
        /// </summary>
        List<Rule> CountyRules
        {
            set;
        }
        /// <summary>
        /// sets the grid with the existing judge rules
        /// </summary>
        List<Rule> JudgeRules
        {
            set;
        }
        /// <summary>
        /// Gets the view contract.
        /// </summary>
        /// <remarks></remarks>
        ManageRulesContract ViewContract
        {
            get;
        }
        /// <summary>
        /// To display message
        /// </summary>
        String SuccessMessage
        {
            set;
            get;

        }

        #endregion

        #region Events

        #endregion

        #region Methods

        #endregion
    }
}
