#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  SysXExceptionMessageFormatter.cs
// Purpose:
//

#endregion

#region Namespaces

#region System Defined

using System.Web;

using System;

#endregion

#region Application Specific

using CoreWeb.IntsofLoggerModel.Interface;
using INTSOF.Utils;
using INTSOF.Logger;
using DAL.Repository;
using DAL.Interfaces;
using INTSOF.Contracts;
using CoreWeb.IntsofExceptionModel.Interface;
using INTSOF.ServiceUtil;
using CoreWeb.IntsofSecurityModel.Interface.Services;
using System.ServiceModel;
#endregion

#endregion

namespace Business
{
    /// <summary>
    /// This class handles the operation related to BAL.
    /// </summary>
    internal static class BALUtils
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private static ISysXLoggerService _sysXLoggerService;
        private static ILogger _logger = null;
        private static String _classModule;
        //Added AMS
        private static ISysXSessionService _sysxSessionService = null;

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Current Class Module
        /// </summary>
        public static String ClassModule
        {
            get
            {
                return _classModule;
            }
            set
            {
                _classModule = value;
            }
        }

        /// <summary>
        /// Get instance of ISysXLoggerService
        /// </summary>
        public static ISysXLoggerService LoggerService
        {
            get
            {
                if (_sysXLoggerService.IsNull())
                {
                    //Returned null instance of ExceptionService if call is made from batch job
                    if (HttpContext.Current.IsNotNull())
                    {
                        //if (HttpContext.Current.ApplicationInstance is IMVCApplication)
                        //{
                        //    _sysXLoggerService = (HttpContext.Current.ApplicationInstance as IMVCApplication).LoggerService;
                        //}
                        //else
                        //{
                        if ((HttpContext.Current.ApplicationInstance as IWebApplication).IsNotNull())
                        {
                            _sysXLoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
                        }
                        //}
                    }
                    //Check if ParallelTaskContext is not null and return the instance of ISysXLoggerService
                    else if (ParallelTaskContext.Current.IsNotNull())
                    {
                        _sysXLoggerService = ParallelTaskContext.LoggerService();
                    }
                    else
                    {
                        _sysXLoggerService = null;
                    }
                }
                return _sysXLoggerService;
            }
        }

        #endregion

        #region Private Properties
        private static ISysXExceptionService _exceptionService;
        public static ISysXExceptionService ExceptionService
        {
            get
            {
                if (_exceptionService.IsNull())
                {

                    if (HttpContext.Current.IsNotNull())
                    {
                        if (HttpContext.Current.ApplicationInstance is IWebApplication)
                        {
                            _exceptionService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;
                        }
                        else if (HttpContext.Current.ApplicationInstance is INTSOF.ServiceModelInterface.IIntsofService)
                        {
                            _exceptionService = (HttpContext.Current.ApplicationInstance as INTSOF.ServiceModelInterface.IIntsofService).ExceptionService;
                        }
                        //else 
                        //{ 
                        //    _exceptionService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;
                        //}
                    }
                    ////check for the operation context in the case of net.tcp for exception context
                    //else if (_exceptionService == null && OperationContext.Current.IsNotNull())
                    //{
                    //    _exceptionService = ServiceSingleton.Instance.ExceptionService;
                    //}
                    //Check if ParallelTaskContext is not null and return the instance of ISysXExceptionService
                    else if (ParallelTaskContext.Current.IsNotNull())
                    {
                        _exceptionService = ParallelTaskContext.ExceptionService();
                    }
                    else
                    {
                        _exceptionService = null;
                    }

                }

                return _exceptionService;
            }
        }
        private static ILogger Logger
        {
            get
            {
                if (_logger.IsNull())
                {
                    //Returned null instance of logger if call is made from batch job
                    if (LoggerService != null)
                        _logger = BALUtils.LoggerService.GetLogger();
                }
                return _logger;
            }
        }

        #endregion

        #endregion

        #region Events

        #endregion

        #region Methods

        #region Public Methods

        #region Repository Methods

        /// <summary>
        /// Get Search Repository Instance.
        /// </summary>
        /// <returns></returns>
        public static ISecurityRepository GetSecurityRepoInstance()
        {
            return new SecurityRepository();
        }

        #endregion

        #region Logging Methods

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
        /// <summary>
        /// Used to Log the debug Message
        /// </summary>
        /// <param name="message"></param>
        public static void LogDebug(string message)
        {
            if (ExceptionService != null)
                ExceptionService.HandleDebug(message);
        }
        #endregion

        #endregion

        #region Private Methods

        #endregion

        #endregion

        public static ISecurityRepository UpdateAspnetMembership()
        {
            return new SecurityRepository();
        }

        public static ISysXSessionService SessionService
        {
            get
            {
                if (_sysxSessionService.IsNull())
                {
                    if (!HttpContext.Current.IsNull())
                    {
                        _sysxSessionService = (HttpContext.Current.ApplicationInstance as IWebApplication).SysXSessionService;
                    }

                }

                return _sysxSessionService;
            }
        }

        public static ISecurityRepository GetAspnetMembershipBy()
        {
            return new SecurityRepository();
        }

        /// <summary>
        /// Get Instance of Help RepoInstance
        /// </summary>
        /// <returns></returns>
        //public static ILookUpTableMaintenanceRepo GetLookTableMaintenanceRepoInstance()
        //{
        //    return new LookUpMaintenanceRepository();
        //}

        /// <summary>
        /// GetLookupRepository
        /// </summary>
        /// <returns></returns>
        public static ILookupRepository GetLookupRepository()
        {
            return new LookupRepository();
        }

        public static IWebSiteRepository GetWebSiteRepository()
        {
            return new WebSiteRepository();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IMessageRepository GetMessageRepoInstance()
        {
            return new MessageRepository();
        }

        /// <summary>
        /// Gets the repository instance for Message Rules
        /// </summary>
        /// <returns>New instance of Message Rules</returns>
        public static IMessageRulesRepository GetMessageRulesRepoInstance()
        {
            return new MessageRulesRepository();
        }

        /// <summary>
        /// Gets the repository instance for Templates class
        /// </summary>
        /// <returns>New instance of Templates repository</returns>
        public static ITemplates GetTemplatesRepoInstance()
        {
            return new TemplatesRepository();
        }

        /// <summary>
        /// Gets instance of communication repository
        /// </summary>
        /// <returns></returns>
        public static ICommunicationRepository GetCommunicationRepoInstance()
        {
            return new CommunicationRepository();
        }

        /// <summary>
        /// Gets instance of Report repository
        /// </summary>
        /// <returns></returns>
        public static IReportRepository GetReportRepoInstance()
        {
            return new ReportRepository();
        }

        ///// <summary>
        ///// Gets instance of compliance setup repository
        ///// </summary>
        ///// <returns></returns>
        //public static IComplianceSetupRepository GetComplianceSetupRepoInstance()
        //{
        //    return new ComplianceSetupRepository();
        //}



        /// <summary>
        /// Gets instance of Client compliance management repository
        /// </summary>
        /// <returns></returns>
        public static IComplianceSetupRepository GetComplianceSetupRepoInstance(Int32 TenantId)
        {
            return new ComplianceSetupRepository(TenantId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tenantID"></param>
        /// <returns></returns>
        public static IComplianceDataRepository GetComplianceDataRepoInstance(Int32 tenantID)
        {
            return new ComplianceDataRepository(tenantID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tenantID"></param>
        /// <returns></returns>
        public static IQueueImagingRepository GetQueueImagingRepoInstance()
        {
            return new QueueImagingRepository();
        }

        public static IRuleRepository GetRuleRepoInstance(Int32 TenantID)
        {
            return new RuleRepository(TenantID);
        }

        /// <summary>
        /// Get Repo Instance of ClientSecurity Repository.
        /// </summary>
        /// <param name="tenantID"></param>
        /// <returns></returns>
        public static IClientSecurityRepository GetClientSecurityRepoInstance(Int32 tenantID, String connectinString = "")
        {
            return new ClientSecurityRepository(tenantID, connectinString);
        }

        /// <summary>
        /// Get Repo Instance of Integrity Repository.
        /// </summary>
        /// <param name="tenantID"></param>
        /// <returns></returns>
        public static IIntegrityRepository GetIntegrityRepoInstance(Int32 tenantID)
        {
            return new IntegrityRepository(tenantID);
        }

        /// <summary>
        /// Get Repo Instance of Mobility Repository.
        /// </summary>
        /// <returns></returns>
        public static IMobilityRepository GetMobilityRepoInstance()
        {
            return new MobilityRepository();
        }

        /// <summary>
        /// Get Repo Instance of Client Mobility Repository.
        /// </summary>
        /// <param name="tenantID"></param>
        /// <returns></returns>
        public static IClientMobilityRepository GetClientMobilityRepoInstance(Int32 tenantID)
        {
            return new ClientMobilityRepository(tenantID);
        }

        /// <summary>
        /// Get Repo Instance of Application Data Repository.
        /// </summary>
        /// <param name="tenantID"></param>
        /// <returns></returns>
        public static IApplicationDataRepository GetApplicationDataRepoInstance()
        {
            return new ApplicationDataRepository();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tenantID"></param>
        /// <returns></returns>
        public static IStoredProcedures GetStoredProceduresRepoInstance(Int32 tenantID)
        {
            return new StoredProceduresRepository(tenantID);
        }

        public static IDocumentRepository GetDocumentRepositoryRepoInstance(Int32 tenantID)
        {
            return new DocumentRepository(tenantID);
        }


        /// <summary>
        /// Get Repo Instance of QueueManagement Repository.
        /// </summary>
        /// <param name="tenantID"></param>
        /// <returns></returns>
        public static IQueueManagementRepository GetQueueManagementRepoInstance(Int32 tenantID)
        {
            return new QueueManagementRepository(tenantID);
        }

        /// <summary>
        /// Get Repo Instance of ExternalVendorOrder Repository for Client DB.
        /// </summary>
        /// <param name="tenantID">tenantID</param>
        /// <returns>ExternalVendorDataRepository</returns>
        public static IEVOrderClientRepository GetEVOrderClientRepoInstance(Int32 tenantID)
        {
            return new EVOrderClientRepository(tenantID);
        }

        /// <summary>
        /// Get Repo Instance of ExternalVendorOrder Repository for SecurityDB.
        /// </summary>
        /// <returns>IEVOrderSecurityRepository object</returns>
        public static IEVOrderSecurityRepository GetEVOrderSecurityRepoInstance()
        {
            return new EVOrderSecurityRepository();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IBackgroundSetupRepository GetBackgroundSetupRepoInstance(Int32 tenantID)
        {
            return new BackgoundSetupRepository(tenantID);
        }

        /// <summary>
        /// Get Repo Instance of Integrity Repository.
        /// </summary>
        /// <param name="tenantID"></param>
        /// <returns></returns>
        public static IBackgroundServiceIntegrityRepository GetBackgroundServiceIntegrityRepoInstance(Int32 tenantID)
        {
            return new BackgroundServiceIntegrityRepository(tenantID);
        }
        /// <summary>
        /// Get Repo Instance of Background Process Order Repository
        /// </summary>
        /// <param name="tenantID"></param>
        /// <returns></returns>

        public static IBackgroundProcessOrderRepository GetBackgroundProcessOrderRepositoryRepoInstance(Int32 tenantID)
        {
            return new BackgroundProcessOrderRepository(tenantID);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IBackgroundPricingRepository GetBackgroundPricingRepoInstance(Int32 tenantID)
        {
            return new BackgroundPricingRepository(tenantID);
        }

        /// <summary>
        /// Get Repo Instance of Background Rule Repository
        /// </summary>
        /// <returns></returns>
        public static IBackgroundRuleRepository GetBackgroundRuleRepoInstance(Int32 tenantID)
        {
            return new BackgroundRuleRepository(tenantID);
        }

        /// <summary>
        /// Get Repo Instance of ProfileSharing Repository 
        /// </summary>
        /// <param name="tenantID"></param>
        /// <returns></returns>
        public static IProfileSharingRepository GetProfileSharingRepoInstance()
        {
            return new ProfileSharingRepository();
        }

        /// <summary>
        /// Get Repo Instance of AgencyReviewQueueRepository 
        /// </summary>
        /// <param name="tenantID"></param>
        /// <returns></returns>
        public static IAgencyReviewRepository GetAgencyReviewRepoInstance()
        {
            return new AgencyReviewRepository();
        }

        /// <summary>

        /// <summary>
        /// Get Repo Instance of ProfileSharing Client Repository 
        /// </summary>
        /// <param name="tenantID"></param>
        /// <returns></returns>
        public static IProfileSharingClientRepository GetProfileSharingClientRepoInstance(Int32 tenantID)
        {
            return new ProfileSharingClientRepository(tenantID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tenantID"></param>
        /// <returns></returns>
        public static IClinicalRotationRepository GetClinicalRotationRepoInstance(Int32 tenantID)
        {
            return new ClinicalRotationRepository(tenantID);
        }

        //public static IContractRepository GetContractRepoInstance(Int32 tenantID)
        //{
        //    return new  ContractRepository(tenantID);
        //}
        public static IPackageBundleRepository GetPackageBundleRepoInstance(Int32 tenantID)
        {
            return new PackageBundleRepository(tenantID);
        }
        /// <summary>
        /// Get Repo Instance of ProfileSharing Repository 
        /// </summary>
        /// <param name="tenantID"></param>
        /// <returns></returns>
        public static IClientContactRepository GetClientContactRepoInstance()
        {
            return new ClientContactRepository();
        }

        /// <summary>
        /// Get Repo Instance of RequirementPackageRepository 
        /// </summary>
        /// <param name="tenantID"></param>
        /// <returns></returns>
        public static IRequirementPackageRepository GetRequirementPackageRepoInstance(Int32 tenantID)
        {
            return new RequirementPackageRepository(tenantID);
        }

        /// <summary>
        /// Get Repo Instance of ApplicantClinicalRotationRepository 
        /// </summary>
        /// <param name="tenantID"></param>
        /// <returns></returns>
        public static IApplicantClinicalRotationRepository GetApplicantClinicalRotationRepoInstance(Int32 tenantID)
        {
            return new ApplicantClinicalRotationRepository(tenantID);
        }

        /// <summary>
        /// Get Repo Instance of ApplicantClinicalRotationRepository 
        /// </summary>
        /// <param name="tenantID"></param>
        /// <returns></returns>
        public static IApplicantRequirementRepository GetApplicantRequirementRepoInstance(Int32 tenantID)
        {
            return new ApplicantRequirementRepository(tenantID);
        }

        /// <summary>
        /// Get Repo Instance of Shared User Clinical Rotation Repository 
        /// </summary>
        /// <param name="tenantID"></param>
        /// <returns></returns>
        public static ISharedUserClinicalRotationRepository GetSharedUserClinicalRotationRepoInstance()
        {
            return new SharedUserClinicalRotationRepository();
        }

        /// <summary>
        /// Get Repo Instance of Requirement Verification Repository 
        /// </summary>
        /// <param name="tenantID"></param>
        /// <returns></returns>
        public static IRequirementVerificationRepository GetRequirementVerificationRepoInstance(Int32 tenantID)
        {
            return new RequirementVerificationRepository(tenantID);
        }

        /// <summary>
        /// Get Repo Instance of Requirement Verification Repository 
        /// </summary>
        /// <param name="tenantID"></param>
        /// <returns></returns>
        public static IRequirementRuleRepository GetRequirementRuleRepoInstance(Int32 tenantID)
        {
            return new RequirementRuleRepository(tenantID);
        }

        /// <summary>
        /// Get Repo Instance of Contract Repository 
        /// </summary>
        /// <param name="tenantID"></param>
        /// <returns></returns>
        public static IContractRepository GetContractRepoInstance(Int32 tenantID)
        {
            return new ContractRepository(tenantID);
        }

        #region DataFeedFormatter
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tenantID"></param>
        /// <returns></returns>
        public static IDataFeedRepository GetDataFeedRepoInstance(Int32 tenantID)
        {
            return new DataFeedRepository(tenantID);
        }


        public static ISecurityRepository GetSecurityInstance()
        {
            return new SecurityRepository();
        }


        #endregion

        /// <summary>
        /// Get Repo Instance of Shared Requirement package Repositiory.
        /// </summary>
        /// <param name="tenantID"></param>
        /// <returns></returns>
        public static ISharedRequirementPackageRepository GetSharedRequirementPackageRepoInstance()
        {
            return new SharedRequirementPackageRepository();
        }

        /// <summary>
        /// Get Search Repository Instance.
        /// </summary>
        /// <returns></returns>
        public static ISMSNotificationRepository GetSMSNotificationRepoInstance()
        {
            return new SMSNotificationRepository();
        }

        #region UAT-2305:Tracking to Rotation category/item/attribute mapping
        /// <summary>
        /// Get Universal Mapping Data Repository Instance.
        /// </summary>
        /// <returns></returns>
        public static IUniversalMappingDataRepository GetUniversalMappingDataRepoInstance(Int32 tenantID)
        {
            return new UniversalMappingDataRepository(tenantID);
        }
        #endregion

        public static ISharedRequirementRuleRepository GetSharedRequirementRuleRepoInstance()
        {
            return new SharedRequirementRuleRepository();
        }

        #region Agency Hierarchy
        public static IAgencyHierarchyRepository GetAgencyHierarchyRepoInstance(Int32 tenantID)
        {
            return new AgencyHierarchyRepository(tenantID);
        }
        #endregion

        #region Agency Jobs

        public static IAgencyJobBoardRepository GetAgencyJobBoardRepoInstance(Int32 tenantID)
        {
            return new AgencyJobBoardRepository(tenantID);
        }

        #endregion
        #region Alumni

        public static IAlumniRepository GetAlumniRepoInstance(Int32 tenantID)
        {
            return new AlumniRepository(tenantID);
        }

        #endregion
        #region Print Scan Appointment Setup

        public static IFingerPrintSetupRepository GetFingerPrintSetupRepoInstance()
        {
            return new FingerPrintSetupRepository();
        }

        public static IFingerPrintClientRepository GetFingerPrintClientRepoInstance(Int32 tenantID)
        {
            return new FingerPrintClientRepository(tenantID);
        }

        #endregion

        #region Placement Matching
        /// <summary>
        /// Placement Matching setup repository instance. Pass tenantId as AppConst.NONE for shared DB Context.
        /// </summary>
        /// <param name="tenantID"></param>
        /// <returns></returns>
        public static IPlacementMatchingSetupRepository GetPlacementMatchingSetupRepoInstance(Int32 tenantID)
        {
            return new PlacementMatchingSetupRepository(tenantID);
        }


        #endregion


        #region Ticket Centre
        public static ITicketsCentreRepository GetTicketsCentreRepoInstance(Int32 tenantID)
        {
            return new TicketsCentreRepository(tenantID);
        }
        #endregion

        #region Admin Entry Portal 

        public static IAdminEntryPortalRepository GetAdminEntryPortalRepoInstance(Int32 tenantID)
        {
            return new AdminEntryPortalRepository(tenantID);
        }

        #endregion
    }
}
