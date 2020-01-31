#region Header Comment Block
// 
// Copyright BestX, Inc. 2012
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename: IManageCategoryView
#endregion

#region Namespaces

#region System Defined

using System;


#endregion

#region Application Specific

using BESTX.Entity;
using INTSOF.UI.Contract.IntsofSecurityModel;
using System.Linq;
#endregion

#endregion

namespace  BESTX.WEB.IntsofSecurityModel.Views
{
    public interface IManageCategoryView
    {

        #region Variables

        #endregion

        #region Properties


        /// <summary>
        /// Categories</summary>
        /// <value>
        /// Sets the value for Categories.</value>
        IQueryable<RuleCategory> StateCategories
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




