using INTSOF.ServiceUtil;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Entity.SharedDataEntity
{
    public class ADBSharedDataEntity
    {
        #region Getting DB Contexts

        static String key = "ADB_SharedDataEntities";
        static ADB_SharedDataEntities _appContext;

        public static ADB_SharedDataEntities GetContext()
        {
            if (HttpContext.Current.IsNull())
            {
                ADB_SharedDataEntities objContext = new ADB_SharedDataEntities();
                objContext.CommandTimeout = AppConsts.FIVE_HUNDRED.To<Int32>();
                return objContext;
            }

            else
            {
                if (!HttpContext.Current.Items.Contains(key))
                {
                    ADB_SharedDataEntities objContext = new ADB_SharedDataEntities();
                    objContext.CommandTimeout = AppConsts.FIVE_HUNDRED.To<Int32>();
                    HttpContext.Current.Items.Add(key, objContext);
                }

                return HttpContext.Current.Items[key] as ADB_SharedDataEntities;
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
                    var context = (ObjectContext)HttpContext.Current.Items[key];
                    context.Dispose();
                    context = null;
                    HttpContext.Current.Items.Remove(key);
                }
            }
        }

        /// <summary>
        ///  Dispose Db context.
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

        /// <summary>
        /// Gets the app context.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static ADB_SharedDataEntities GetAppContext()
        {
            ADB_SharedDataEntities objContext = new ADB_SharedDataEntities();
            objContext.CommandTimeout = AppConsts.FIVE_HUNDRED.To<Int32>();
            return objContext;

        }

        /// <summary>
        /// Gets the ObjectContext.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static ADB_SharedDataEntities GetServiceContext()
        {
            Object value;
            if (!ServiceContext.Current.DBContexts.TryGetValue(key, out value))
            {
                ADB_SharedDataEntities objContext = new ADB_SharedDataEntities();
                objContext.CommandTimeout = AppConsts.FIVE_HUNDRED.To<Int32>();
                ServiceContext.Current.DBContexts.Add(key, objContext);
            }
            return ServiceContext.Current.DBContexts[key] as ADB_SharedDataEntities;
        }

        /// <summary>
        /// Gets the ObjectContext.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static ADB_SharedDataEntities GetParallelTaskContext()
        {
            Object value;
            if (!ParallelTaskContext.Current.DBContexts.TryGetValue(key, out value))
            {
                ADB_SharedDataEntities objContext = new ADB_SharedDataEntities();
                objContext.CommandTimeout = AppConsts.FIVE_HUNDRED.To<Int32>();
                ParallelTaskContext.Current.DBContexts.Add(key, objContext);
            }

            return ParallelTaskContext.Current.DBContexts[key] as ADB_SharedDataEntities;
        }
        #endregion
    }
}
