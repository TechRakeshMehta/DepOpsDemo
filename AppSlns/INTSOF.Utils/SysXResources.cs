#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  SysXResources.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Resources;
using System.Web;

#endregion

#region Application Specific

#endregion

#endregion

namespace INTSOF.Utils
{
    /// <summary>
    /// Handles the operations related to sysX resources.
    /// </summary>
    /// <remarks></remarks>
    public static class SysXResources
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private static ResourceManager resourceManager;

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        #endregion

        #region Private Properties

        #endregion

        #endregion

        #region Events

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Gets the string.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static String GetString(String key)
        {
            return resourceManager.GetString(key);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <remarks></remarks>
        static SysXResources()
        {
            resourceManager = ResourceManager.CreateFileBasedResourceManager("SysXResource", HttpContext.Current.ApplicationInstance.Server.MapPath("~"), null);
        }

        #endregion

        #endregion
    }
}