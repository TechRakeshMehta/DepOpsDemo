#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  SysXCacheConfiguration.cs
// Purpose:   SysX Cache Configuration
//

#endregion

#region Namespace

#region System Defined

using System;
using System.Collections.Generic;


#endregion

#endregion

namespace CoreWeb.IntsofCachingModel.Config
{
    /// <summary>
    /// SysX Cache Configuration
    /// </summary>
    public class SysXCacheConfiguration
    {
        /// <summary>
        /// get the list of SqlCacheTables
        /// </summary>
        public List<String> SqlCacheTables { get; private set; }

        /// <summary>
        /// SysXCacheConfiguration
        /// </summary>
        /// <param name="SqlCacheTables">SqlCacheTables as List(String)</param>
        public SysXCacheConfiguration(List<String> SqlCacheTables)
        {
            this.SqlCacheTables = SqlCacheTables;
        }
    }
}
