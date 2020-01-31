#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  SysXSearchConsts.cs
// Purpose:   
//

#endregion

#region Namespaces



#endregion

namespace INTSOF.Utils.UI
{
   using System.Collections.Generic;

   using System.Text;

   /// <summary>
    /// Represents advance search options.
    /// </summary>
    public class SysXSearchOptions : Dictionary<string, string>
    {
        /// <summary>
        /// Create empty set of seach options.
        /// </summary>
        public SysXSearchOptions()
        {
        }

        /// <summary>
        /// Create search options according to given string.
        /// </summary>
        /// <param name="searchOptions"></param>
        public SysXSearchOptions(string searchOptions)
        {
            if (searchOptions != string.Empty)
            {
                string[] keyValues = searchOptions.TrimEnd('#').Split('#');

                foreach (string keyValue in keyValues)
                {
                    string key = keyValue.Split(';')[0];
                    string value = keyValue.Split(';')[1];
                    base.Add(key, value);
                }
            }
        }

        /// <summary>
        /// Returns a string that contains advance search options.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder searchOptions = new StringBuilder();
            foreach (var key in base.Keys)
            {
                searchOptions.AppendFormat("{0};{1}#", key, base[key]);
            }
            return searchOptions.ToString();
        }
    }
}
