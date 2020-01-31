using CoreWeb.IntsofExceptionModel.Interface;
using INTSOF.Contracts;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobileWebApi.Service
{
    public static class LogExceptionService
    {
        private static ISysXExceptionService _exceptionService;
        public static ISysXExceptionService ExceptionService
        {
            get
            {
                if (_exceptionService.IsNull())
                {

                    if (HttpContext.Current.IsNotNull())
                    {
                        _exceptionService = (HttpContext.Current.ApplicationInstance as IMobileWebApplication).ExceptionService;
                    }
                    else
                    {
                        _exceptionService = null;
                    }

                }

                return _exceptionService;
            }
        }

        public static void LogMobileApplicationException(ExceptionContract exceptionContract)
        {
            try
            {
                throw new Exception(exceptionContract.ExceptionMessage, new Exception { Source = exceptionContract.ModuleName});
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
           
        }

        //public static void LogInternalException(Exception exception)
        //{
        //    //send exception to logger service
        //    LogError(ex);
        //}

        /// <summary>
        /// Used to Log the Error Message
        /// </summary>
        /// <param name="infoMessage"></param>
        /// <param name="ex">Exception</param>
        public static void LogError(String infoMessage, Exception ex)
        {
            //Returned nothing if call is made from batch job and loging is done using Nlog
            if (ExceptionService != null)
                ExceptionService.HandleError(infoMessage, ex);
        }

        /// <summary>
        /// Used to Log the Error Message
        /// </summary>
        /// <param name="ex">Exception</param>
        /// <remarks></remarks>
        public static void LogError(Exception ex)
        {
            //Returned nothing if call is made from batch job and loging is done using Nlog
            if (ExceptionService != null)
                ExceptionService.HandleError(ex.Message, ex);
        }
    }
}