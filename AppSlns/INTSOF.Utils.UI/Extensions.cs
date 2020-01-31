#region Copyright

// **************************************************************************************************
// Extensions.cs
//  
// 
// Comments
// 	-----------------------------------------------------
// Initial Coding
// 
// 
//                          Copyright 2011 Intersoft Data Labs.
// 
//      All rights are reserved.  Reproduction or transmission in whole or in part, in any form or
//    by any means, electronic, mechanical or otherwise, is prohibited without the prior written
//    consent of the copyright owner.
// *************************************************************************************************

#endregion

namespace INTSOF.Utils.UI
{
    #region Using Directives

    using System;
    using System.Collections.Generic;

    using INTERSOFT.WEB.UI.WebControls;
    using Telerik.Web.UI;
    using INTSOF.Utils;

    #endregion

    /// <summary>
    ///   Extension methods related to UI
    /// </summary>
    public static class Extensions
    {
        #region Public Methods

        /// <summary>
        ///   Adds the first empty item.
        /// </summary>
        /// <param name="comboBox">The combo box.</param>
        /// <remarks>
        /// </remarks>
        public static void AddFirstEmptyItem(this WclComboBox comboBox)
        {
            comboBox.Items.Insert(AppConsts.NONE, new RadComboBoxItem { Selected = true, Text = AppConsts.COMBOBOX_ITEM_SELECT });
        }

        /// <summary>
        ///   This is an extension method for decrypting the query string.
        /// </summary>
        /// <param name="sourceDictionary">Source dictionary value.</param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static void ToDecryptedQueryString(this Dictionary<String, String> sourceDictionary, String args)
        {
            if (sourceDictionary.None())
            {
                throw new ArgumentNullException("sourceDictionary");
            }

            if (args.IsNullOrEmpty())
            {
                throw new ArgumentNullException("args");
            }

            var queryString = new EncryptedQueryString(args);

            sourceDictionary.AddRange(queryString);
        }

        /// <summary>
        ///   This is an extension method for EncryptedQueryString which returns the encrypted string for string type dictionary.
        /// </summary>
        /// <param name="sourceDictionary">Source dictionary value.</param>
        /// <returns></returns>
        public static String ToEncryptedQueryString(this Dictionary<String, String> sourceDictionary)
        {
            if (sourceDictionary.None())
            {
                throw new ArgumentNullException("sourceDictionary");
            }

            return new EncryptedQueryString(sourceDictionary).ToString();
        }

        #endregion
    }
}