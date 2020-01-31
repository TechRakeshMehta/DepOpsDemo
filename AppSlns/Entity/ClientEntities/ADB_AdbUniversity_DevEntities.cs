#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  SysXAppDBEntities.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Web;


#endregion

#region Application Specific

using INTSOF.Utils;


#endregion

#endregion


namespace Entity.ClientEntities
{
    public partial class ADB_AdbUniversity_DevEntities
    {
        private static String key = "ADB_AdbUniversity_DevEntities";

        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static ADB_AdbUniversity_DevEntities GetContext(String connectionString)
        {
            if (HttpContext.Current.IsNull())
            {
                ADB_AdbUniversity_DevEntities objContext = new ADB_AdbUniversity_DevEntities(connectionString);
                objContext.CommandTimeout = AppConsts.FIVE_HUNDRED.To<Int32>();
                return objContext;
            }

            else
            {
                if (!HttpContext.Current.Items.Contains(key))
                {
                    ADB_AdbUniversity_DevEntities objContext = new ADB_AdbUniversity_DevEntities(connectionString);
                    objContext.CommandTimeout = AppConsts.FIVE_HUNDRED.To<Int32>();
                    HttpContext.Current.Items.Add(key, objContext);
                }

                return HttpContext.Current.Items[key] as ADB_AdbUniversity_DevEntities;
            }
        }
        /// <summary>
        /// Clears the context.
        /// </summary>
        /// <remarks></remarks>
        public static void ClearContext()
        {
            if (!HttpContext.Current.IsNull())
            {
                if (HttpContext.Current.Items.Contains(key))
                {
                    HttpContext.Current.Items.Remove(key);
                }
            }
        }

    }
}
