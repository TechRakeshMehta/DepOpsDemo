using INTSOF.ServiceUtil;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Core.Objects.DataClasses;
using System.ServiceModel;
using System.Web;
namespace Entity
{
    public partial class ADBMessageDB_DevEntities
    {
        private Guid _auditGroup;

        private Int32 _currentVersion;

        private List<EntityObject> _parentList;

        private Boolean _isTrackingNumberFound;

        static String key = "ADBMessageDB_DevEntities";

        static ADBMessageDB_DevEntities _appContext;

        public static ADBMessageDB_DevEntities GetContext()
        {
            if (HttpContext.Current.IsNull())
            {
                ADBMessageDB_DevEntities objContext = new ADBMessageDB_DevEntities();
                objContext.CommandTimeout = AppConsts.FIVE_HUNDRED.To<Int32>();
                return objContext;
            }

            else
            {
                if (!HttpContext.Current.Items.Contains(key))
                {
                    ADBMessageDB_DevEntities objContext = new ADBMessageDB_DevEntities();
                    objContext.CommandTimeout = AppConsts.FIVE_HUNDRED.To<Int32>();
                    HttpContext.Current.Items.Add(key, objContext);
                }

                return HttpContext.Current.Items[key] as ADBMessageDB_DevEntities;
            }
        }
        //public static ADBMessageDB_DevEntities GetOperationContext()
        //{
        //    if (OperationContext.Current.IsNotNull() && OperationContext.Current.IncomingMessageHeaders.IsNotNull())
        //    {
        //        if (!OperationContext.Current.IncomingMessageProperties.ContainsKey(key))
        //        {
        //            ADBMessageDB_DevEntities objContext = new ADBMessageDB_DevEntities();
        //            objContext.CommandTimeout = AppConsts.FIVE_HUNDRED.To<Int32>();
        //            OperationContext.Current.IncomingMessageProperties.Add(key, objContext);
        //        }

        //        return OperationContext.Current.IncomingMessageProperties[key] as ADBMessageDB_DevEntities;
        //    }
        //    else
        //    {
        //        ADBMessageDB_DevEntities objContext = new ADBMessageDB_DevEntities();
        //        objContext.CommandTimeout = AppConsts.FIVE_HUNDRED.To<Int32>();
        //        return objContext;
        //    }
        //}

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
        public static ADBMessageDB_DevEntities GetAppContext()
        {
            ADBMessageDB_DevEntities objContext = new ADBMessageDB_DevEntities();
            objContext.CommandTimeout = AppConsts.FIVE_HUNDRED.To<Int32>();
            return objContext;

        }

        /// <summary>
        /// Gets the ObjectContext.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static ADBMessageDB_DevEntities GetServiceContext()
        {
            Object value;
            if (!ServiceContext.Current.DBContexts.TryGetValue(key, out value))
            {
                ADBMessageDB_DevEntities objContext = new ADBMessageDB_DevEntities();
                objContext.CommandTimeout = AppConsts.FIVE_HUNDRED.To<Int32>();
                ServiceContext.Current.DBContexts.Add(key, objContext);
            }
            return ServiceContext.Current.DBContexts[key] as ADBMessageDB_DevEntities;
        }

        /// <summary>
        /// Gets the ObjectContext.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static ADBMessageDB_DevEntities GetParallelTaskContext()
        {
            Object value;
            if (!ParallelTaskContext.Current.DBContexts.TryGetValue(key, out value))
            {
                ADBMessageDB_DevEntities objContext = new ADBMessageDB_DevEntities();
                objContext.CommandTimeout = AppConsts.FIVE_HUNDRED.To<Int32>();
                ParallelTaskContext.Current.DBContexts.Add(key, objContext);
            }

            return ParallelTaskContext.Current.DBContexts[key] as ADBMessageDB_DevEntities;
        }
    }
}
