#region Copyright

// **************************************************************************************************
// LegacyArchiveUtil.cs
// 
// 
//   Comments
// 	-----------------------------------------------------
//  Initial Coding
// Code Cleanup
// 
//                          Copyright 2011 Intersoft Data Labs.
//      All rights are reserved.  Reproduction or transmission in whole or in part, in any form or
//    by any means, electronic, mechanical or otherwise, is prohibited without the prior written
//    consent of the copyright owner.
// *************************************************************************************************
#endregion

#region Using Directives

using System;

#endregion

namespace INTSOF.Utils.LegacyArchive
{
    /// <summary>Maps the property to an automatically built view.</summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DisplayMappingAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DisplayMappingAttribute"/> class.
        /// </summary>
        public DisplayMappingAttribute()
        {
            this.IsVisible = true;
            this.DisplayFlags = DisplayFlags.Default;
            this.ColSpan = 1;
            this.CommandName = string.Empty;
            this.CommandArgument = string.Empty;
            this.StaticText = string.Empty;
        }

        /// <summary>Gets or sets a value indicating whether this property should be display.</summary>
        /// <value>
        /// 	<c>true</c> if <see langword="true"/>, display, otherwise don't.
        /// </value>
        public bool IsVisible { get; set; }

        /// <summary>Gets or sets the label for the control.</summary>
        /// <value>The label.</value>
        public string Label { get; set; }

        /// <summary>Gets or sets the display order.</summary>
        /// <value>The display order.</value>
        public int DisplayOrder { get; set; }

        /// <summary>Gets or sets the number of columns.</summary>
        /// <value>The number of columns.</value>
        public int ColSpan { get; set; }

        /// <summary>Gets or sets the display flags.</summary>
        /// <value>The display flags.</value>
        public DisplayFlags DisplayFlags { get; set; }

        /// <summary>Gets or sets the name of the command.</summary>
        /// <value>The name of the command.</value>
        public string CommandName { get; set; }

        /// <summary>Gets or sets the command argument.</summary>
        /// <value>The command argument.</value>
        public string CommandArgument { get; set; }

        /// <summary>Gets or sets the static text.</summary>
        /// <value>The static text.</value>
        public string StaticText { get; set; }

        /// <summary>Gets or sets the data format.</summary>
        /// <value>The data format.</value>
        public DataFormat? DataFormat { get; set; }
    }
}
