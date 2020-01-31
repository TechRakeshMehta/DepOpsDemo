using INTSOF.ServiceUtil;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Entity.LocationEntity
{
    public class ADBLocationDataEntities
    {
        #region Getting DB Contexts

        static String key = "ADB_LocationDataEntities";
        static ADB_LocationDataEntities _appContext;

        public static ADB_LocationDataEntities GetContext()
        {
            if (HttpContext.Current.IsNull())
            {
                ADB_LocationDataEntities objContext = new ADB_LocationDataEntities();
                objContext.CommandTimeout = AppConsts.FIVE_HUNDRED.To<Int32>();
                return objContext;
            }

            else
            {
                if (!HttpContext.Current.Items.Contains(key))
                {
                    ADB_LocationDataEntities objContext = new ADB_LocationDataEntities();
                    objContext.CommandTimeout = AppConsts.FIVE_HUNDRED.To<Int32>();
                    HttpContext.Current.Items.Add(key, objContext);
                }

                return HttpContext.Current.Items[key] as ADB_LocationDataEntities;
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
        public static ADB_LocationDataEntities GetAppContext()
        {
            ADB_LocationDataEntities objContext = new ADB_LocationDataEntities();
            objContext.CommandTimeout = AppConsts.FIVE_HUNDRED.To<Int32>();
            return objContext;

        }

        /// <summary>
        /// Gets the ObjectContext.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static ADB_LocationDataEntities GetServiceContext()
        {
            Object value;
            if (!ServiceContext.Current.DBContexts.TryGetValue(key, out value))
            {
                ADB_LocationDataEntities objContext = new ADB_LocationDataEntities();
                objContext.CommandTimeout = AppConsts.FIVE_HUNDRED.To<Int32>();
                ServiceContext.Current.DBContexts.Add(key, objContext);
            }
            return ServiceContext.Current.DBContexts[key] as ADB_LocationDataEntities;
        }

        /// <summary>
        /// Gets the ObjectContext.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static ADB_LocationDataEntities GetParallelTaskContext()
        {
            Object value;
            if (!ParallelTaskContext.Current.DBContexts.TryGetValue(key, out value))
            {
                ADB_LocationDataEntities objContext = new ADB_LocationDataEntities();
                objContext.CommandTimeout = AppConsts.FIVE_HUNDRED.To<Int32>();
                ParallelTaskContext.Current.DBContexts.Add(key, objContext);
            }

            return ParallelTaskContext.Current.DBContexts[key] as ADB_LocationDataEntities;
        }
        #endregion
    }
}
