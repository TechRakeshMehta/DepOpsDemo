using INTSOF.ServiceUtil;
using INTSOF.Utils;
using System;
using System.Web;

namespace Entity
{
    public class SysXQueue
    {
        static String APP_CONTEXT_KEY = "SysXAppDBEntities";
        static SysXAppDBEntities _appContext;

        static String ADB_MESSAGE_QUEUE_KEY = "ADBMessageDB_DevEntities";
        static ADBMessageDB_DevEntities _adbMessageQueueContext;


        static String CLIENT_QUEUE_KEY = "ADBApplicantMessageDB_DevEntities";

        static ADBApplicantMessageDB_DevEntities _applicantQueueContext; 

        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static ADBMessageDB_DevEntities GetADB_MessageQueueContext()
        {
            if (HttpContext.Current.IsNull())
            { 
                ADBMessageDB_DevEntities objContext = new ADBMessageDB_DevEntities();
                objContext.CommandTimeout = AppConsts.FIVE_HUNDRED.To<Int32>();
                return objContext;
            }
            else
            {
                if (!HttpContext.Current.Items.Contains(ADB_MESSAGE_QUEUE_KEY))
                {
                    ADBMessageDB_DevEntities objContext = new ADBMessageDB_DevEntities( );
                    objContext.CommandTimeout = AppConsts.FIVE_HUNDRED.To<Int32>();
                    HttpContext.Current.Items.Add(ADB_MESSAGE_QUEUE_KEY, objContext);
                }

                return HttpContext.Current.Items[ADB_MESSAGE_QUEUE_KEY] as ADBMessageDB_DevEntities;
            }
        }

        /// <summary>
        /// Gets the app context.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static ADBMessageDB_DevEntities GetAppADB_MessageQueueContext()
        {
            if (_adbMessageQueueContext.IsNull())
            {
                _adbMessageQueueContext = new ADBMessageDB_DevEntities();                 
                _adbMessageQueueContext.CommandTimeout = AppConsts.FIVE_HUNDRED.To<Int32>();                 
            }

            return _adbMessageQueueContext;
        }

        /// <summary>
        /// Gets the ObjectContext.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static ADBMessageDB_DevEntities GetMessageServiceContext()
        {
            Object value;
            if (!ServiceContext.Current.DBContexts.TryGetValue(ADB_MESSAGE_QUEUE_KEY, out value))
            {
                ADBMessageDB_DevEntities objContext = new ADBMessageDB_DevEntities();
                objContext.CommandTimeout = AppConsts.FIVE_HUNDRED.To<Int32>();
                ServiceContext.Current.DBContexts.Add(ADB_MESSAGE_QUEUE_KEY, objContext);
            }
            return ServiceContext.Current.DBContexts[ADB_MESSAGE_QUEUE_KEY] as ADBMessageDB_DevEntities;
        }
        /// <summary>
        ///  Gets the Message Parallel Task Context.
        /// </summary>
        /// <returns></returns>
        public static ADBMessageDB_DevEntities GetMessageParallelTaskContext()
        {
            Object value;
            if (!ParallelTaskContext.Current.DBContexts.TryGetValue(ADB_MESSAGE_QUEUE_KEY, out value))
            {
                ADBMessageDB_DevEntities objContext = new ADBMessageDB_DevEntities();
                objContext.CommandTimeout = AppConsts.FIVE_HUNDRED.To<Int32>();
                ParallelTaskContext.Current.DBContexts.Add(ADB_MESSAGE_QUEUE_KEY, objContext);
            }
            return ParallelTaskContext.Current.DBContexts[ADB_MESSAGE_QUEUE_KEY] as ADBMessageDB_DevEntities;
        }
        /// <summary>
        /// Gets the ObjectContext.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static SysXAppDBEntities GetParallelTaskContext()
        {
            Object value;
            if (!ParallelTaskContext.Current.DBContexts.TryGetValue(APP_CONTEXT_KEY, out value))
            {
                SysXAppDBEntities objContext = new SysXAppDBEntities();
                objContext.CommandTimeout = AppConsts.FIVE_HUNDRED.To<Int32>();
                ParallelTaskContext.Current.DBContexts.Add(APP_CONTEXT_KEY, objContext);
            }

            return ParallelTaskContext.Current.DBContexts[APP_CONTEXT_KEY] as SysXAppDBEntities;
        }
        /// <summary>
        /// Gets the app context.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static SysXAppDBEntities GetAppContext()
        {
            if (_appContext.IsNull())
            {
                _appContext = new SysXAppDBEntities(); 
                _appContext.CommandTimeout = AppConsts.FIVE_HUNDRED.To<Int32>();                 
            }

            return _appContext;
        }

        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static SysXAppDBEntities GetContext()
        {
            if (HttpContext.Current.IsNull())
            { 
                SysXAppDBEntities objContext = new SysXAppDBEntities();
                objContext.CommandTimeout = AppConsts.FIVE_HUNDRED.To<Int32>();
                return objContext;
            }

            else
            {
                if (!HttpContext.Current.Items.Contains(APP_CONTEXT_KEY))
                {
                    SysXAppDBEntities objContext = new SysXAppDBEntities();
                    objContext.CommandTimeout = AppConsts.FIVE_HUNDRED.To<Int32>();
                    HttpContext.Current.Items.Add(APP_CONTEXT_KEY, objContext);
                }

                return HttpContext.Current.Items[APP_CONTEXT_KEY] as SysXAppDBEntities;
            }
        }

        /// <summary>
        /// Gets the ObjectContext.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static SysXAppDBEntities GetServiceContext()
        {
            Object value;
            if (!ServiceContext.Current.DBContexts.TryGetValue(APP_CONTEXT_KEY, out value))
            {
                SysXAppDBEntities objContext = new SysXAppDBEntities();
                objContext.CommandTimeout = AppConsts.FIVE_HUNDRED.To<Int32>();
                ServiceContext.Current.DBContexts.Add(APP_CONTEXT_KEY, objContext);
            }
            return ServiceContext.Current.DBContexts[APP_CONTEXT_KEY] as SysXAppDBEntities;
        }


        /// <summary>
        /// Gets the client DB Context
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static ADBApplicantMessageDB_DevEntities GetAppApplicantQueueContext()
        {
            if (_applicantQueueContext.IsNull())
            {
                _applicantQueueContext = new ADBApplicantMessageDB_DevEntities();                 
                _applicantQueueContext.CommandTimeout = AppConsts.FIVE_HUNDRED.To<Int32>(); 
            }

            return _applicantQueueContext;
        }

        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static ADBApplicantMessageDB_DevEntities GetApplicantQueueContext()
        {
            if (HttpContext.Current.IsNull())
            { 
                ADBApplicantMessageDB_DevEntities objContext = new ADBApplicantMessageDB_DevEntities();
                objContext.CommandTimeout = AppConsts.FIVE_HUNDRED.To<Int32>();
                return objContext;
            }

            else
            {
                if (!HttpContext.Current.Items.Contains(CLIENT_QUEUE_KEY))
                {
                    ADBApplicantMessageDB_DevEntities objContext = new ADBApplicantMessageDB_DevEntities();
                    objContext.CommandTimeout = AppConsts.FIVE_HUNDRED.To<Int32>();
                    HttpContext.Current.Items.Add(CLIENT_QUEUE_KEY, objContext);
                }

                return HttpContext.Current.Items[CLIENT_QUEUE_KEY] as ADBApplicantMessageDB_DevEntities;
            }
        }
    }
}
