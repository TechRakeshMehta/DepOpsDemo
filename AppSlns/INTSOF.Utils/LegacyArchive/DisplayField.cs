#region Copyright

// **************************************************************************************************
// LegacyArchiveUtil.cs
// 
// 
// Comments
// -----------------------------------------------------
// Initial Coding
// Code Cleanup
// 
//                         Copyright 2011 Intersoft Data Labs.
//      All rights are reserved.  Reproduction or transmission in whole or in part, in any form or
//    by any means, electronic, mechanical or otherwise, is prohibited without the prior written
//    consent of the copyright owner.
// *************************************************************************************************
#endregion

using System;

namespace INTSOF.Utils.LegacyArchive
{
    /// <summary>
    /// The class is specific for the field display
    /// </summary>
    public class DisplayField
    {
        /// <summary>Gets or sets the name.</summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>Gets or sets the value.</summary>
        /// <value>The value.</value>
        public object Value { get; set; }

        /// <summary>Gets or sets the static text.</summary>
        /// <value>The static text.</value>
        public string StaticText { get; set; }

        /// <summary>Gets or sets the col span.</summary>
        /// <value>The col span.</value>
        public int ColSpan { get; set; }

        /// <summary>Gets or sets the name of the command.</summary>
        /// <value>The name of the command.</value>
        public string CommandName { get; set; }

        /// <summary>Gets or sets the display order.</summary>
        /// <value>The display order.</value>
        public int DisplayOrder { get; set; }

        /// <summary>Gets or sets the display flags.</summary>
        /// <value>The display flags.</value>
        public DisplayFlags DisplayFlags { get; set; }

        /// <summary>Gets or sets the name of the debug field.</summary>
        /// <value>The name of the debug field.</value>
        public string DebugFieldName { get; set; }

        /// <summary>Gets or sets the data format.</summary>
        /// <value>The data format.</value>
        public DataFormat? DataFormat { get; set; }
    }
}
