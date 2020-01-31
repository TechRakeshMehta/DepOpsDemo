using CoreWeb.IntsofExceptionModel.Interface;
using CoreWeb.IntsofLoggerModel.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace INTSOF.ServiceUtil
{
    /// <summary>
    /// Parallel Task Context class, used to store context specification information for non HttpApplication,Service context, in parallel to HttpContext,Service context
    /// </summary>
    public class ParallelTaskContext
    {
        public delegate void ParallelTask(Dictionary<String, Object> TaskData);

        /// <summary>
        /// Declare the instance of ISysXLoggerService
        /// </summary>
        private static ISysXLoggerService _loggerService;
        /// <summary>
        /// Declare the instance of ISysXExceptionService
        /// </summary>
        private static ISysXExceptionService _ExceptionService;

        /// <summary>
        /// Stores thread specific context key.
        /// </summary>
        [ThreadStaticAttribute] //Use Thread Local Storage Concept: ThreadStaticAttribute or Thread.SetData, Thread.GetData
        public static String currentParallelTaskContextKey;

        private static Dictionary<string, ServiceContextData> parallelTaskContextItemsData = new Dictionary<string, ServiceContextData>();

        public Dictionary<String, Object> DataDict
        {
            get;
            set;
        }

        private static Object lockObject = new Object();

        /// <summary>
        /// for getting key for current thread
        /// </summary>
        public static String currentThreadContextKeyString
        {
            get { return currentParallelTaskContextKey; }
        }

        /// <summary>
        /// The applications which use Parallel Task context, have to call this function on application start.
        /// </summary> 
        private static void init()
        {
            currentParallelTaskContextKey = Convert.ToString(Guid.NewGuid());
        }

        /// <summary>
        /// Provides thread specific Parallel Task context
        /// </summary>
        public static ServiceContextData Current
        {
            get
            {
                if (currentParallelTaskContextKey == null)
                {
                    return null;
                }

                lock (lockObject)
                {
                    ServiceContextData serviceContextDataObj;
                    if (!parallelTaskContextItemsData.TryGetValue(currentParallelTaskContextKey, out serviceContextDataObj))
                    {
                        serviceContextDataObj = new ServiceContextData();
                        parallelTaskContextItemsData.Add(currentParallelTaskContextKey, serviceContextDataObj);
                    }
                    ServiceContextData serviceContextDataObject;
                    parallelTaskContextItemsData.TryGetValue(currentParallelTaskContextKey, out serviceContextDataObject);
                    return serviceContextDataObject;
                }
            }
        }

        /// <summary>
        /// Clear all context items specific to current thread.
        /// </summary>
        private static void ReleaseContextItems()
        {
            if (Current == null)
            {
                return;
            }
            else
            {
                foreach (var context in Current.DBContexts)
                {
                    context.Value.Dispose();
                }

                if (Current.DataDict != null)
                {
                    //foreach (var dic in Current.DataDict)
                    //{
                    //    Current.DataDict.Remove(dic.Key);
                    //}

                    Current.DataDict.Clear();
                }

                parallelTaskContextItemsData.Remove(currentParallelTaskContextKey);
                currentParallelTaskContextKey = null;
            }
        }
        /// <summary>
        /// Clear database context items specific to current thread.
        /// </summary>
        private static void ReleaseDBContextItems()
        {
            if (Current == null)
            {
                return;
            }
            else
            {
                foreach (var context in Current.DBContexts)
                {
                    context.Value.Dispose();
                }

                if (Current.DataDict != null)
                {
                    //foreach (var dic in Current.DataDict)
                    //{
                    //    Current.DataDict.Remove(dic.Key);
                    //}

                    Current.DataDict.Clear();
                }

                Current.DBContexts.Clear();
            }
        }

        /// <summary>
        /// This method is used to sending email and message by parallel task 
        /// </summary>
        /// <param name="Operation">selegate that conatin reference of method</param>
        /// <param name="Data">Data dictonary for passing to delegate object</param>
        public static void PerformParallelTask(ParallelTask Operation, Dictionary<String, Object> Data, ISysXLoggerService LoggerService, ISysXExceptionService ExceptionService)
        {
            try
            {
                //using common logger for all parallel tasks in single application
                if (_loggerService == null)
                {
                    _loggerService = LoggerService;
                }
                //using common Exception for all parallel tasks in single application
                if (_ExceptionService == null)
                {
                    _ExceptionService = ExceptionService;
                }
                Task task = Task.Factory.StartNew(() =>
                {
                    try
                    {
                        init();
                        Operation(Data);
                    }
                    finally
                    {
                        ReleaseContextItems();
                    }
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// This method return the instance ISysXLoggerService for ParalllelTaskContext
        /// </summary>
        /// <returns></returns>
        public static ISysXLoggerService LoggerService()
        {
            return _loggerService;
        }
        /// <summary>
        /// This method return the instance of ISysXExceptionService for ParalllelTaskContext
        /// </summary>
        /// <returns></returns>
        public static ISysXExceptionService ExceptionService()
        {
            return _ExceptionService;
        }
        /// <summary>
        /// This parallel task method is used to convert the documents into pdf and merge all pdf documents into unified pdf document.
        /// </summary>
        /// <param name="PdfConversion">PdfConversion(delegate that refer to the ConvertDocumentsIntoPdf method references)</param>
        /// <param name="Data">pdfConversionParm (Data dictionary that conatins the applicantdocument table object ,tenantId,currentLoggedUserID)</param>
        /// <param name="PdfMerging">PdfMerging(delegate that refer to the MergeDocIntoUnifiedPdf method references)</param>
        /// <param name="DataDic">pdfMergingParm(Data dictionary that conatins the organizationUserId,tenantId,CurrentLoggedUserID)</param>
        /// <param name="LoggerService">LoggerService (HttpContext.Current.ApplicationInstance of ISysXLoggerService )</param>
        /// <param name="ExceptionService">ExceptionService (HttpContext.Current.ApplicationInstance of ISysXExceptionService)</param>
        public static void ParallelTaskPdfConversionMerging(ParallelTask ConvertToPDF, Dictionary<String, Object> pdfConversionParm, ParallelTask MergePDF, Dictionary<String, Object> pdfMergingParm, ISysXLoggerService loggerService, ISysXExceptionService exceptionService)
        {
            try
            {
                //using common logger for all parallel tasks in single application
                if (_loggerService == null)
                {
                    _loggerService = loggerService;
                }
                //using common Exception for all parallel tasks in single application
                if (_ExceptionService == null)
                {
                    _ExceptionService = exceptionService;
                }

                Task task = Task.Factory.StartNew(() =>
                {
                    try
                    {
                        init();
                        //Delegate to call conversion method 
                        ConvertToPDF(pdfConversionParm);
                    }
                    finally
                    {
                        ReleaseContextItems();
                    }
                }).ContinueWith((conversion) =>
                {
                    try
                    {
                        init();

                        if (conversion.IsCompleted)
                        {
                            //Delegate to call merging method
                            MergePDF(pdfMergingParm);
                        }
                    }
                    finally
                    {
                        ReleaseContextItems();
                    }
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public static void ParallelTaskSendingInvitation(ParallelTask SendInvitations, Dictionary<String, Object> conversionParm, ParallelTask OnInvitationSent, Dictionary<String, Object> invitationSentParams, ISysXLoggerService loggerService, ISysXExceptionService exceptionService)
        //{
        //    try
        //    {
        //        //using common logger for all parallel tasks in single application
        //        if (_loggerService == null)
        //        {
        //            _loggerService = loggerService;
        //        }
        //        //using common Exception for all parallel tasks in single application
        //        if (_ExceptionService == null)
        //        {
        //            _ExceptionService = exceptionService;
        //        }

        //        Task task = Task.Factory.StartNew(() =>
        //        {
        //            try
        //            {
        //                init();
        //                //Delegate to call conversion method 
        //                SendInvitations(conversionParm);
        //            }
        //            catch (Exception ex)
        //            {
        //            }
        //            finally
        //            {
        //                ReleaseContextItems();
        //            }
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
}
