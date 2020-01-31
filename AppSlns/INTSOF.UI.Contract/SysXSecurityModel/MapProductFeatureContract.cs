#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  MapProductFeature.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Collections.Generic;

#endregion

#region Application Specific

#endregion

#endregion

namespace INTSOF.UI.Contract.IntsofSecurityModel
{
    /// <summary>
    /// This contract gets or sets the properties for mapping product with features.
    /// </summary>
    public class MapProductFeatureContract
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// CheckInRole</summary>
        /// <value>
        /// Gets or sets the value for CheckInRole.</value>
        public Boolean CheckInRole
        {
            get;
            set;
        }

        /// <summary>
        /// ProductID</summary>
        /// <value>
        /// Gets or sets the value for product's id.</value>
        public Int32 ProductId
        {
            set;
            get;
        }

        /// <summary>
        /// UpdatedSysXBlockIDs</summary>
        /// <value>
        /// Gets or sets the value for updated sysxblock's id.</value>
        public List<Int32> UpdatedSysXBlockIDs
        {
            get;
            set;
        }

        /// <summary>
        /// BlockId</summary>
        /// <value>
        /// Gets or sets the value for block's id.</value>
        public Int32 BlockId
        {
            get;
            set;
        }

        /// <summary>
        /// FeatureCount</summary>
        /// <value>
        /// Gets or sets the value for number of features.</value>
        public Int32 FeatureCount
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