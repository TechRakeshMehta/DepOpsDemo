#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  SysXCacheConfigHandler.cs
// Purpose:   System X Cache Config Handler
//

#endregion

#region NameSpace

#region System Defined
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;
#endregion

#region Application Specific

using INTSOF.Utils;

#endregion

#endregion


namespace CoreWeb.IntsofCachingModel.Config
{
    /// <summary>
    /// SysX Cache Config Handler
    /// </summary>
    public class SysXCacheConfigHandler : IConfigurationSectionHandler
    {
        /// <summary>
        /// Create Cache Config Handler
        /// </summary>
        /// <param name="parent">Parent</param>
        /// <param name="configContext">configContext</param>
        /// <param name="section">section as System.Xml.XmlNode</param>
        /// <returns></returns>
        public Object Create(Object parent, Object configContext, System.Xml.XmlNode section)
        {
            List<String> sqlCacheTables = new List<String>();
            
            //Find SQL Cache Tables
            foreach (XmlNode sqlCacheTableNode in section.SelectNodes(SysXCachingConst.SQL_CACHE_TABLES_CACHETABLE))
            {  
                sqlCacheTables.Add(sqlCacheTableNode.InnerText);
            }

            SysXCacheConfiguration cacheConfig = new SysXCacheConfiguration(sqlCacheTables);
            return cacheConfig;
        }
    }
}
