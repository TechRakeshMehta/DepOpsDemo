#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  SysXExceptionMessageFormatter.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;

#endregion

#region Application Specific

#endregion

#endregion

namespace INTSOF.Utils
{
    /// <summary>
    /// RetrievingEntitiesEventArgs is the class to containing event data for paging.
    /// </summary>
    public class RetrievingEntitiesEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the max rows.
        /// </summary>
        /// <value>The max rows.</value>
        /// <remarks></remarks>
        public Int32 MaxRows { get; set; }

        /// <summary>
        /// Gets or sets the start index.
        /// </summary>
        /// <value>The start index.</value>
        /// <remarks></remarks>
        public Int32 StartIndex { get; set; }

        /// <summary>
        /// Gets or sets the total row count.
        /// </summary>
        /// <value>The total row count.</value>
        /// <remarks></remarks>
        public Int32 TotalRowCount { get; set; }
    }
}