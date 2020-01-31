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
using System.Data.Entity.Core.Objects;


#endregion

#region Application Specific

using INTSOF.Utils;
using INTSOF.ServiceUtil;

#endregion

#endregion


namespace Entity.ClientEntity
{
    public partial class ADB_LibertyUniversity_ReviewEntities
    {
        private static String key = "ADB_LibertyUniversity_ReviewEntities";

        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static ADB_LibertyUniversity_ReviewEntities GetContext(String connectionString)
        {
            ADB_LibertyUniversity_ReviewEntities objContext;
            if (HttpContext.Current.IsNotNull())
            {
                return GetContextForAspNetApplication(connectionString);
            }
            if (ServiceContext.Current.IsNotNull())
            {
                return GetContextForService(connectionString);
            }
            if (ParallelTaskContext.Current.IsNotNull())
            {
                return GetContextForParallelTaskContext(connectionString);
            }
            objContext = new ADB_LibertyUniversity_ReviewEntities(connectionString);
            objContext.CommandTimeout = AppConsts.FIVE_HUNDRED.To<Int32>();
            return objContext;
        }

        private static ADB_LibertyUniversity_ReviewEntities GetContextForService(String connectionString)
        {

            ObjectContext value;
            if (!ServiceContext.Current.DBContexts.TryGetValue(key, out value) || !value.Connection.ConnectionString.Equals(connectionString))
            {
                if (ServiceContext.Current.DBContexts.TryGetValue(key, out value))
                {
                    //Dispose existing context before removing.
                    ObjectContext clientDbContext = ServiceContext.Current.DBContexts.GetValue(key);
                    if (clientDbContext.IsNotNull())
                    {
                        clientDbContext.Dispose();
                    }

                    //remove old key if it has different connectionstring
                    ServiceContext.Current.DBContexts.Remove(key);
                }
                ADB_LibertyUniversity_ReviewEntities objContext = new ADB_LibertyUniversity_ReviewEntities(connectionString);
                objContext.CommandTimeout = AppConsts.FIVE_HUNDRED.To<Int32>();
                ServiceContext.Current.DBContexts.Add(key, objContext);

            }
            return ServiceContext.Current.DBContexts[key] as ADB_LibertyUniversity_ReviewEntities;
        }

        private static ADB_LibertyUniversity_ReviewEntities GetContextForParallelTaskContext(String connectionString)
        {
            ObjectContext value;
            if (!ParallelTaskContext.Current.DBContexts.TryGetValue(key, out value) || !value.Connection.ConnectionString.Equals(connectionString))
            {
                if (ParallelTaskContext.Current.DBContexts.TryGetValue(key, out value))
                {
                    //Dispose existing context before removing.
                    ObjectContext clientDbContext = ParallelTaskContext.Current.DBContexts.GetValue(key);
                    if (clientDbContext.IsNotNull())
                    {
                        clientDbContext.Dispose();
                    }

                    //remove old key if it has different connectionstring
                    ParallelTaskContext.Current.DBContexts.Remove(key);
                }
                ADB_LibertyUniversity_ReviewEntities objContext = new ADB_LibertyUniversity_ReviewEntities(connectionString);
                objContext.CommandTimeout = AppConsts.FIVE_HUNDRED.To<Int32>();
                ParallelTaskContext.Current.DBContexts.Add(key, objContext);
            }
            return ParallelTaskContext.Current.DBContexts[key] as ADB_LibertyUniversity_ReviewEntities;
        }
        private static ADB_LibertyUniversity_ReviewEntities GetContextForAspNetApplication(String connectionString)
        {
            ADB_LibertyUniversity_ReviewEntities objContext;
            if (!HttpContext.Current.Items.Contains(key))
            {
                objContext = new ADB_LibertyUniversity_ReviewEntities(connectionString);
                objContext.CommandTimeout = AppConsts.FIVE_HUNDRED.To<Int32>();
                HttpContext.Current.Items.Add(key, objContext);
            }
            else
            {
                if (!connectionString.Equals((HttpContext.Current.Items[key] as ADB_LibertyUniversity_ReviewEntities).Connection.ConnectionString))
                {
                    ClearContext();
                    objContext = new ADB_LibertyUniversity_ReviewEntities(connectionString);
                    objContext.CommandTimeout = AppConsts.FIVE_HUNDRED.To<Int32>();
                    HttpContext.Current.Items.Add(key, objContext);
                }
            }

            return HttpContext.Current.Items[key] as ADB_LibertyUniversity_ReviewEntities;
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

        /// <summary>
        /// Dispose Db context.
        /// </summary>
        /// <remarks></remarks>
        public static void DisposeDbContext()
        {
            if (!HttpContext.Current.IsNull())
            {
                if (HttpContext.Current.Items.Contains(key))
                {
                    var context = (ObjectContext)HttpContext.Current.Items[key];
                    context.Dispose();
                    context = null;
                    HttpContext.Current.Items.Remove(key);
                }
            }
        }

        ///// <summary>
        ///// Clear DB context from parallel task context to get the fresh data from DB.
        ///// </summary>
        ///// <remarks></remarks>
        //public static void ClearParallelTaskDBContext()
        //{
        //    if (ParallelTaskContext.Current.IsNotNull())
        //    {
        //        ObjectContext value;
        //        if (ParallelTaskContext.Current.DBContexts.TryGetValue(key, out value))
        //        {
        //            ParallelTaskContext.Current.DBContexts.Remove(key);
        //        }
        //    }
        //}

    }
}
