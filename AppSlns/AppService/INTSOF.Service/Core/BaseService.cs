using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoreWeb.IntsofSecurityModel.Interface.Services;
using INTSOF.Contracts;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.Utils;
using NLog;

namespace INTSOF.Service.Core
{
    public class BaseService
    {
        #region Private
        /// <summary>
        /// Logger instance to log the exception of ClientContact
        /// </summary>
        private static NLog.Logger _clientContactlogger;

        /// <summary>
        /// Logger instance to log the exception of ClientRotation
        /// </summary>
        private static NLog.Logger _clinicalRotationLogger;

        /// <summary>
        /// Logger instance to log the exception of RequirementPackage
        /// </summary>
        private static NLog.Logger _requirementPackageLogger;

        /// <summary>
        /// Logger instance to log the exception of ApplicantClinicalRotation Service
        /// </summary>
        private static NLog.Logger _applicantClinicalRotationLogger;

        /// <summary>
        /// Logger instance to log the exception of ApplicantClinicalRotation Service
        /// </summary>
        private static NLog.Logger _AgencyHierarchyLogger;

        private static NLog.Logger _dbLogger;

        /// <summary>
        /// The _active user
        /// </summary>
        protected UserContext _activeUser;

        #endregion

        #region Methods

        #region Protected Methods

        /// <summary>
        /// Logs the Error for ClientContact Service
        /// </summary>
        /// <param name="ex"></param>
        protected void LogClientContactSvcError(Exception ex)
        {
            if (_clientContactlogger == null)
            {
                _clientContactlogger = LogManager.GetLogger("ClientContactLogger");

            }
            _clientContactlogger.Error(String.Format("An Error has occured in WCF Service: {0}, Inner Exception: {1}, Stack Trace: {2}",
                       ex.Message,
                       ex.InnerException,
                       ex.StackTrace));

            ExecuteDBLogger(ex, "ClientContact.svc");
        }

        /// <summary>
        /// Logs the Error for ClinicalRotation Service
        /// </summary>
        /// <param name="ex"></param>
        protected void LogClinicalRotationSvcError(Exception ex)
        {
            if (_clinicalRotationLogger == null)
            {
                _clinicalRotationLogger = LogManager.GetLogger("ClinicalRotationLogger");

            }
            _clinicalRotationLogger.Error(String.Format("An Error has occured in WCF Service: {0}, Inner Exception: {1}, Stack Trace: {2}",
                           ex.Message,
                           ex.InnerException,
                           ex.StackTrace));
            ExecuteDBLogger(ex, "ClinicalRotation.svc");
        }

        /// <summary>
        /// Logs the Error for RequirementPackage Service
        /// </summary>
        /// <param name="ex"></param>
        protected void LogRequirementPkgSvcError(Exception ex)
        {
            if (_requirementPackageLogger == null)
            {
                _requirementPackageLogger = LogManager.GetLogger("RequirementPkgLogger");

            }
            _requirementPackageLogger.Error(String.Format("An Error has occured in WCF Service: {0}, Inner Exception: {1}, Stack Trace: {2}",
                           ex.Message,
                           ex.InnerException,
                           ex.StackTrace));
            ExecuteDBLogger(ex, "RequirementPackage.svc");
        }

        /// <summary>
        /// Logs the Error for ClientContact Service
        /// </summary>
        /// <param name="ex"></param>
        protected void LogApplicantClinicalRotationSvcError(Exception ex)
        {
            if (_applicantClinicalRotationLogger == null)
            {
                _applicantClinicalRotationLogger = LogManager.GetLogger("ApplicantClinicalRotationLogger");
            }
            _applicantClinicalRotationLogger.Error(String.Format("An Error has occured in WCF Service: {0}, Inner Exception: {1}, Stack Trace: {2}",
                       ex.Message,
                       ex.InnerException,
                       ex.StackTrace));

            ExecuteDBLogger(ex, "AgencyHierarchy.svc");
        }

        /// <summary>
        /// Logs the Error for ClientContact Service
        /// </summary>
        /// <param name="ex"></param>
        protected void LogAgencyHierarchySvcError(Exception ex)
        {
            if (_AgencyHierarchyLogger == null)
            {
                _AgencyHierarchyLogger = LogManager.GetLogger("AgencyHierarchyLogger");
            }
            _AgencyHierarchyLogger.Error(String.Format("An Error has occured in WCF Service: {0}, Inner Exception: {1}, Stack Trace: {2}",
                       ex.Message,
                       ex.InnerException,
                       ex.StackTrace));

            ExecuteDBLogger(ex, "AgencyHierarchy.svc");
        }

        /// <summary>
        /// Log the errors in database
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="serviceName"></param>
        private void ExecuteDBLogger(Exception ex, String serviceName)
        {
            if (_dbLogger == null)
            {
                _dbLogger = LogManager.GetLogger("DBLogger");
            }
            _dbLogger.ErrorException(String.Format("An Error has occurred in {0}", serviceName), ex);
        }

        /// <summary>
        /// Method is responsible to log error
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="response"></param>
        protected void HandleError(Exception ex, IBaseServiceResponse response)
        {
            String errorCode = String.Empty; //To Do - We can extend it based on the Exception Type
            if (response != null)
            {
                response.Errors.Add(new ErrorContract
                {
                    ErrorCode = errorCode,
                    ErrorMessage = ex.Message
                });
                Exception innerException = ex.InnerException;
                while (innerException != null)
                {
                    response.Errors.Add(new ErrorContract
                    {
                        ErrorCode = errorCode,
                        ErrorMessage = innerException.Message
                    });
                    innerException = innerException.InnerException;
                }
                response.Status = ResponseStatus.Exception;
            }
            //Log Error Logic should be written here
        }


        protected UserContext ActiveUser
        {
            get
            {
                if (_activeUser == null)
                {
                    ISysXSessionService sessionService = (HttpContext.Current.ApplicationInstance as IWebApplication).SysXSessionService;

                    if (sessionService.IsNotNull())
                    {
                        _activeUser = new UserContext();

                        short businessChannelTypeID = AppConsts.COMPLIO_BUSINESS_CHANNEL_TYPE;
                        if (sessionService.BusinessChannelType != null)
                        {
                            businessChannelTypeID = sessionService.BusinessChannelType.BusinessChannelTypeID;
                        }

                        _activeUser.IsSysXAdmin = sessionService.IsSysXAdmin;
                        _activeUser.OrganizationUserId = sessionService.OrganizationUserId;
                        _activeUser.SysXBlockId = sessionService.SysXBlockId;
                        _activeUser.SysXBlockName = sessionService.SysXBlockName;
                        _activeUser.UserID = sessionService.UserId;
                        _activeUser.BusinessChannelTypeID = businessChannelTypeID;
                    }
                }
                return _activeUser;
            }
            set
            {
                _activeUser = value;
            }
        }

        #endregion

        #endregion
    }
}