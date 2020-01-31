#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ManageUsers.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

#region Application Specific

#endregion

#endregion

namespace INTSOF.UI.Contract.IntsofSecurityModel
{
    /// <summary>
    /// This contract gets or sets the properties for manage user section.
    /// </summary>
    public class ManageItemsContract
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the tenant id.
        /// </summary>
        /// <value>The tenant id.</value>
        /// <remarks></remarks>
        public Int32 ItemID
        {
            get;
            set;
        }

        public String Name
        {
            get;
            set;
        }
        public String Description
        {
            get;
            set;

        }
        public String ItemType
        {
            get;
            set;

        }
        public String ResultType
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