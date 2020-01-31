#region Copyright

// **************************************************************************************************
// LegacyArchiveUtil.cs
// 
// 
//   Comments
// -----------------------------------------------------
//  Initial Coding
// Code Cleanup
// 
//                          Copyright 2011 Intersoft Data Labs.
//      All rights are reserved.  Reproduction or transmission in whole or in part, in any form or
//    by any means, electronic, mechanical or otherwise, is prohibited without the prior written
//    consent of the copyright owner.
// *************************************************************************************************
#endregion
namespace INTSOF.Utils.LegacyArchive
{
    using System;

    /// <summary>Display flags for the DataItem and DynamicSection controls</summary>
    [Flags]
    public enum DisplayFlags
    {
        /// <summary>
        /// Default display mode
        /// </summary>
        Default = 0,

        /// <summary>
        /// Don't draw a border around the text element
        /// </summary>
        NoTextBorder = 1,

        /// <summary>
        /// Only show the Text element
        /// </summary>
        HideLabelBlock = 2,

        /// <summary>
        /// Only show the Label element
        /// </summary>
        HideTextBlock = 4,

        /// <summary>
        /// Do not render any hidden controls
        /// </summary>
        OnlyRenderVisible = 8,
    }
}
