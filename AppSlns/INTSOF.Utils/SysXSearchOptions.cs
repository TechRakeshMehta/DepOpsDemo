#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:   SysXSearchOptions.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Collections.Generic;
using System.Text;

#endregion

#region Application Specific

#endregion

#endregion

namespace INTSOF.Utils
{
	/// <summary>
	/// Represents advance search options.
	/// </summary>
	public class SysXSearchOptions : Dictionary<String, String>
	{
		#region Variables

		#region Public Variables

		#endregion

		#region Private Variables

		#endregion

		#endregion

		#region Properties

		#region Public Properties

		#endregion

		#endregion

		#region Constructor

		/// <summary>
		/// Create empty set of search options.
		/// </summary>
		public SysXSearchOptions()
		{
			// No action need
		}


		/// <summary>
		/// Create search options according to given String.
		/// </summary>
		/// <param name="searchOptions"></param>
		public SysXSearchOptions(String searchOptions)
		{
			if (!searchOptions.Equals(String.Empty))
			{
				String[] keyValues = searchOptions.TrimEnd(SysXSearchConsts.SEARCH_FIELD_SEPERATOR).Split(SysXSearchConsts.SEARCH_FIELD_SEPERATOR);

				foreach (String keyValue in keyValues)
				{
					String key = keyValue.Split(SysXSearchConsts.SEARCH_VALUE_SEPERATOR)[AppConsts.NONE];
					String value = keyValue.Split(SysXSearchConsts.SEARCH_VALUE_SEPERATOR)[AppConsts.ONE];
					base.Add(key, value);
				}
			}
		}

		#endregion

		#region Methods

		#region Public Methods

		/// <summary>
		/// Returns a String that contains advance search options.
		/// </summary>
		/// <returns></returns>
		public override String ToString()
		{
			StringBuilder searchOptions = new StringBuilder();
			foreach (var key in base.Keys)
			{
				searchOptions.AppendFormat("{0}{1}{2}{3}", key, SysXSearchConsts.SEARCH_VALUE_SEPERATOR, base[key], SysXSearchConsts.SEARCH_FIELD_SEPERATOR);
			}
			return searchOptions.ToString();
		}

		#endregion

		#region Private Methods

		#endregion

		#endregion
	}
}
