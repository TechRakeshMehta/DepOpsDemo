#region Header Comment Block

// 
// Copyright 2014 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  CreateJiraTicketService.cs
// Purpose:
//

#endregion

#region Namespaces

#region System Defined
using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;

#endregion

#region Application Specific
using Business.RepoManagers;
using Entity;
using ExternalVendors;
using INTSOF.ServiceUtil;
using INTSOF.Utils;
using Entity.ClientEntity;
using Atlassian.Jira;
using ExternalVendors.ClearStarVendor;
using System.Text;
using INTSOF.UI.Contract.Services;
#endregion

#endregion

namespace VendorOrderProcessService
{
    public static class CreateJiraTicketService
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private static String JiraTicketServiceLogger;
        private static Int32 recordChunkSize;
        private static Boolean testModeOn;
        private static Int32 createJiraTicketTimeLag;
        private static Int32 retryCountTimeLag;
        private static Int32 maxRetryCount;
        private static String jiraProjectName;
        private static String jiraIssueType;
        private static String jiraIssuePriority;
        private static String jiraURL;
        private static String jiraUserName;
        private static String jiraPassword;
        private static String jiraReporter;
        private static Boolean _isServiceLoggingEnabled;

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        #endregion

        #region Private Properties

        #endregion

        #endregion

        #region Constructor

        static CreateJiraTicketService()
        {

            JiraTicketServiceLogger = "JiraTicketServiceLogger";

            if (ConfigurationManager.AppSettings["RecordChunkSize"].IsNotNull())
            {
                recordChunkSize = Convert.ToInt32(ConfigurationManager.AppSettings["RecordChunkSize"]);
            }
            else
            {
                recordChunkSize = AppConsts.CHUNK_SIZE_FOR_CREATE_ORDER_SERVICE;
            }

            if (ConfigurationManager.AppSettings["TestModeOn"].IsNotNull())
            {
                testModeOn = Convert.ToBoolean(ConfigurationManager.AppSettings["TestModeOn"]);
            }
            else
            {
                testModeOn = false;
            }

            if (ConfigurationManager.AppSettings["CreateJiraTicketTimeLag"].IsNotNull())
            {
                createJiraTicketTimeLag = Convert.ToInt32(ConfigurationManager.AppSettings["CreateJiraTicketTimeLag"]);
            }
            else
            {
                createJiraTicketTimeLag = 12;
            }

            if (ConfigurationManager.AppSettings["RetryCountTimeLag"].IsNotNull())
            {
                retryCountTimeLag = Convert.ToInt32(ConfigurationManager.AppSettings["RetryCountTimeLag"]);
            }
            else
            {
                retryCountTimeLag = 24;
            }


            if (ConfigurationManager.AppSettings["MaxRetryCount"].IsNotNull())
            {
                maxRetryCount = Convert.ToInt32(ConfigurationManager.AppSettings["MaxRetryCount"]);
            }
            else
            {
                maxRetryCount = AppConsts.MAX_RETRY_COUNT_FOR_CREATE_ORDER_SERVICE;
            }

            //<add key="JiraProjectName" value="ADBHELP"/>
            if (ConfigurationManager.AppSettings["JiraProjectName"].IsNotNull())
            {
                jiraProjectName = Convert.ToString(ConfigurationManager.AppSettings["JiraProjectName"]);
            }
            else
            {
                jiraProjectName = "ADBHELP";
            }

            ////<add key="JiraIssueType" value="Technical Incident"/>
            if (ConfigurationManager.AppSettings["JiraIssueType"].IsNotNull())
            {
                jiraIssueType = Convert.ToString(ConfigurationManager.AppSettings["JiraIssueType"]);
            }
            else
            {
                jiraIssueType = "Technical Incident";
            }

            if (ConfigurationManager.AppSettings["JiraIssuePriority"].IsNotNull())
            {
                jiraIssuePriority = Convert.ToString(ConfigurationManager.AppSettings["JiraIssuePriority"]);
            }
            else
            {
                jiraIssuePriority = "Major";
            }

            if (ConfigurationManager.AppSettings["JiraURL"].IsNotNull())
            {
                jiraURL = Convert.ToString(ConfigurationManager.AppSettings["JiraURL"]);
            }
            else
            {
                jiraURL = "https://americandb.atlassian.net";
            }

            if (ConfigurationManager.AppSettings["JiraUserName"].IsNotNull())
            {
                jiraUserName = Convert.ToString(ConfigurationManager.AppSettings["JiraUserName"]);
            }
            else
            {
                jiraUserName = "bhupender";
            }

            if (ConfigurationManager.AppSettings["JiraPassword"].IsNotNull())
            {
                jiraPassword = Convert.ToString(ConfigurationManager.AppSettings["JiraPassword"]);
            }
            else
            {
                jiraPassword = "";
            }

            ////<add key="JiraReporter" value="Complio System"/>
            if (ConfigurationManager.AppSettings["JiraReporter"].IsNotNull())
            {
                jiraReporter = Convert.ToString(ConfigurationManager.AppSettings["JiraReporter"]);
            }
            else
            {
                jiraReporter = "bhupender";
            }
            if (ConfigurationManager.AppSettings["IsServiceLoggingEnabled"].IsNotNull())
            {
                _isServiceLoggingEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["IsServiceLoggingEnabled"]);
            }
            else
            {
                _isServiceLoggingEnabled = false;
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tenant_Id"></param>
        public static void CreateJiraTicketForFailedOrders(Int32? tenant_Id = null)
        {
            try
            {
                //ServiceContext.init();
                ServiceLogger.Info("Calling GetOrderToCreateJiraTicket: " + DateTime.Now.ToString(), JiraTicketServiceLogger);

                List<ClientDBConfiguration> clientDbConfs = SecurityManager.GetClientDBConfiguration().Select(
                 item =>
                 {
                     ClientDBConfiguration config = new ClientDBConfiguration();
                     config.CDB_TenantID = item.CDB_TenantID;
                     config.CDB_ConnectionString = item.CDB_ConnectionString;
                     config.Tenant = new Entity.Tenant();
                     config.Tenant.TenantName = item.Tenant.TenantName; return config;
                 }).ToList();

                if (clientDbConfs != null && clientDbConfs.Count > 0 && tenant_Id != null)
                {
                    ServiceLogger.Debug<Int32?>("TenantID for which Jira Tickets are to be created:", tenant_Id, JiraTicketServiceLogger);
                    clientDbConfs = clientDbConfs.Where(x => x.CDB_TenantID == tenant_Id).ToList();

                }

                ServiceLogger.Info("Started foreach loop on ClientDbConfigurations.", JiraTicketServiceLogger);
                foreach (ClientDBConfiguration clntDbConf in clientDbConfs)
                {
                    if (CheckDBConnection.TestConnString(clntDbConf.CDB_ConnectionString, JiraTicketServiceLogger))
                    {
                        DateTime jobStartTime = DateTime.Now;
                        DateTime jobEndTime;
                        Int32 tenantId = clntDbConf.CDB_TenantID;
                        Boolean executeLoop = true;
                        while (executeLoop)
                        {
                            ServiceLogger.Info("Starting ExecuteLoop", JiraTicketServiceLogger);
                            List<usp_GetOrdersToBeCreateJiraTicket_Result> orderToBeCreateJiraTicket = ExternalVendorOrderManager.GetOrdersToBeCreateJiraTicket(tenantId,
                                                                                                                                    recordChunkSize, maxRetryCount,
                                                                                                                                    createJiraTicketTimeLag, retryCountTimeLag);
                            if (testModeOn && orderToBeCreateJiraTicket.IsNotNull())
                            {
                                ServiceLogger.Info("Total OrdersToBeCreateJiraTicket Records found: " + orderToBeCreateJiraTicket.Count(), JiraTicketServiceLogger);
                                ServiceLogger.Info("Fetching test mode orders from db: " + DateTime.Now.ToString(), JiraTicketServiceLogger);
                                List<Int32> testModeOrderIDs = ExternalVendorOrderManager.GetTestModeBkgOrders(tenantId);
                                ServiceLogger.Info("Total Orders from Test Mode: " + testModeOrderIDs.Count(), JiraTicketServiceLogger);
                                orderToBeCreateJiraTicket = orderToBeCreateJiraTicket.IsNotNull()
                                    ? orderToBeCreateJiraTicket.Where(cond => testModeOrderIDs.Contains(cond.BkgOrderID)).ToList() : null;
                                ServiceLogger.Info("Total OrdersToBeCreateJiraTicket Records found after Test Mode: " + orderToBeCreateJiraTicket.Count(),
                                                   JiraTicketServiceLogger);
                            }

                            if (orderToBeCreateJiraTicket.IsNotNull() && orderToBeCreateJiraTicket.Count() > AppConsts.NONE)
                            {
                                ServiceLogger.Info("Total OrdersToBeCreateJiraTicket Records found: " + orderToBeCreateJiraTicket.Count(), JiraTicketServiceLogger);
                                ServiceLogger.Info("Started foreach loop on OrderToBeCreateJiraTicket based on OrderID", JiraTicketServiceLogger);
                                String tenantName = clntDbConf.Tenant.TenantName;

                                orderToBeCreateJiraTicket.Select(col => col.OrderID).Distinct().ForEach(orderID =>
                                {
                                    ServiceLogger.Debug<Int32>("OrderID from the loop:", orderID, JiraTicketServiceLogger);
                                    CreateJiraTicket(orderToBeCreateJiraTicket.Where(cond => cond.OrderID == orderID), tenantName, tenantId, testModeOn);
                                });

                                ServiceLogger.Info("Ended foreach loop on OrderToBeCreateJiraTicket based on OrderID", JiraTicketServiceLogger);
                                ServiceContext.ReleaseDBContextItems();
                                executeLoop = true;
                            }
                            else
                            {
                                ServiceLogger.Info("Ending ExecuteLoop. No Records found.", JiraTicketServiceLogger);
                                executeLoop = false;
                                ServiceContext.ReleaseDBContextItems();
                            }
                        }
                        //Save service logging data to DB
                        if (_isServiceLoggingEnabled)
                        {
                            jobEndTime = DateTime.Now;
                            Entity.AppConfiguration appConfig = SecurityManager.GetAppConfiguration(AppConsts.BKG_ORDER_SERVICE_USER_ID);
                            Int32 bkgOrderServiceUserId = appConfig.IsNotNull() ? Convert.ToInt32(appConfig.AC_Value) : AppConsts.BKG_ORDER_SERVICE_USER_ID_VALUE;
                            ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                            serviceLoggingContract.ServiceName = ServiceName.BkgOrderService.GetStringValue();
                            serviceLoggingContract.JobName = JobName.CreateJiraTicketForFailedOrders.GetStringValue();
                            serviceLoggingContract.TenantID = tenantId;
                            serviceLoggingContract.JobStartTime = jobStartTime;
                            serviceLoggingContract.JobEndTime = jobEndTime;
                            //serviceLoggingContract.Comments = "";
                            serviceLoggingContract.IsDeleted = false;
                            serviceLoggingContract.CreatedBy = bkgOrderServiceUserId;
                            serviceLoggingContract.CreatedOn = DateTime.Now;
                            SecurityManager.SaveServiceLoggingDetail(serviceLoggingContract);
                        }
                    }
                    ServiceContext.ReleaseDBContextItems();
                }

                ServiceLogger.Info("Ended foreach loop on ClientDbConfigurations.", JiraTicketServiceLogger);
            }
            catch (Exception ex)
            {
                ServiceLogger.Error(String.Format("An Error has occured in CreateJiraTicketForFailedOrders method, the details of which are: {0}, Inner Exception: {1}," +
                                                 " Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace
                                                 + " current context key : " + ServiceContext.currentThreadContextKeyString), JiraTicketServiceLogger);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderToBeCreateJiraTicket"></param>
        /// <param name="tenantName"></param>
        /// <param name="tenantID"></param>
        /// <param name="testModeOn"></param>
        private static void CreateJiraTicket(IEnumerable<usp_GetOrdersToBeCreateJiraTicket_Result> orderToBeCreateJiraTicket, String tenantName,
                                             Int32 tenantID, Boolean testModeOn)
        {
            try
            {
                ServiceLogger.Info("Starting CreateJiraTicket...", JiraTicketServiceLogger);
                Boolean isJiraIssueCreated = false;
                String jiraTicketSummary = String.Empty;
                StringBuilder jiraTicketDescription = new StringBuilder();

                if (orderToBeCreateJiraTicket.IsNotNull() && orderToBeCreateJiraTicket.Count() > AppConsts.NONE)
                {
                    usp_GetOrdersToBeCreateJiraTicket_Result orderToCreateJiraTicket = orderToBeCreateJiraTicket.FirstOrDefault();
                    jiraTicketSummary = (testModeOn) ? "Complio_Test_Jira_Ticket" + " - Complio System unable to upload Order# " + orderToCreateJiraTicket.OrderID
                                                        + " for " + tenantName + " to Clear Star"
                                                        : " - Complio System unable to upload Order : " + orderToCreateJiraTicket.OrderID
                                                        + " for:" + tenantName + " to Clear Star";

                    jiraTicketDescription.Append("Complio Order Details:" + Environment.NewLine);
                    jiraTicketDescription.Append("OrderID:" + orderToCreateJiraTicket.OrderID
                                                + " | " + "Applicant Name: " + orderToCreateJiraTicket.FirstName + " " + orderToCreateJiraTicket.LastName
                                                + " | " + "Email: " + orderToCreateJiraTicket.PrimaryEmailAddress
                                                + " | " + " Institute: " + tenantName + Environment.NewLine);
                    jiraTicketDescription.Append(Environment.NewLine);
                    jiraTicketDescription.Append("Reason:" + Environment.NewLine);

                    if (orderToBeCreateJiraTicket.Any(cond => cond.VendorID.IsNull()))
                    {
                        //Vendor mapping does not exist -> Create JIRA Ticket                       
                        isJiraIssueCreated = true;
                        jiraTicketDescription.Append("Complio Setup Issue." + Environment.NewLine);
                        jiraTicketDescription.Append("External Vendor Account mapping is missing." + Environment.NewLine);
                    }

                    if (!isJiraIssueCreated)
                    {
                        //Get ExtSvcInvokeHistory To Get the Order Dispatch Error for Service Line Item OR Profile Create
                        List<ExtSvcInvokeHistory> extVendorUploadErrorListFromExtSvcInvokeHistory = ExternalVendorOrderManager
                                                                                                    .GetExtVendorUploadErrorListFromExtSvcInvokeHistory(tenantID
                                                                                                                                    , orderToBeCreateJiraTicket);

                        ExtSvcInvokeHistory createProfileHistory = extVendorUploadErrorListFromExtSvcInvokeHistory.Where(x => x.ESIH_MethodName.Contains("CreateProfile"))
                                                                                                                   .FirstOrDefault();
                        if (createProfileHistory.IsNotNull())
                        {
                            String createProfileErrorMessage = ClearStarInvokeDetails.GetCreateProfileErrorMessage(createProfileHistory.ESIH_Response);

                            if (!String.IsNullOrEmpty(createProfileErrorMessage))
                            {
                                //Error while creating Clear Star Profile -> Create JIRA Ticket
                                isJiraIssueCreated = true;
                                jiraTicketDescription.Append("Clear Star Create Profile Issue." + Environment.NewLine);
                                jiraTicketDescription.Append(createProfileErrorMessage.ToString() + Environment.NewLine);
                            }
                        }

                        if (!isJiraIssueCreated)
                        {
                            List<ExtSvcInvokeHistory> addOrderToProfileInvokeHistory = extVendorUploadErrorListFromExtSvcInvokeHistory
                                                                                       .Where(cond => cond.ESIH_MethodName.Contains("AddOrderToProfile"))
                                                                                       .ToList();

                            foreach (var item in addOrderToProfileInvokeHistory)
                            {
                                if (!isJiraIssueCreated)
                                {
                                    String addOrderToProfileErrorMessage = ClearStarInvokeDetails.GetAddOrderToProfileErrorMessage(item.ESIH_Response);
                                    if (!String.IsNullOrEmpty(addOrderToProfileErrorMessage))
                                    {
                                        //Error while adding Order To CS Profile -> Create JIRA Ticket                
                                        jiraTicketDescription.Append("Clear Star Add Order Items to Profile Issue." + Environment.NewLine);
                                        jiraTicketDescription.Append(addOrderToProfileErrorMessage.ToString() + Environment.NewLine);
                                        isJiraIssueCreated = true;
                                    }
                                }
                            }
                        }
                    }

                    if (!isJiraIssueCreated)
                    {
                        jiraTicketDescription.Append("Unknown" + Environment.NewLine);
                        isJiraIssueCreated = true;
                    }

                    if (isJiraIssueCreated && !String.IsNullOrEmpty(jiraTicketDescription.ToString()) && !String.IsNullOrEmpty(jiraTicketSummary))
                    {
                        Issue jiraIssue = null;
                        String jiraIssueFailMessage = String.Empty;
                        try
                        {
                            ServiceLogger.Info("Starting CreateTicketToJiraAccount...", JiraTicketServiceLogger);
                            jiraIssue = CreateTicketToJiraAccount(jiraTicketSummary, jiraTicketDescription.ToString());
                            ServiceLogger.Info("Ending CreateTicketToJiraAccount...", JiraTicketServiceLogger);
                        }
                        catch (Exception ex)
                        {
                            jiraIssueFailMessage = ex.Message;
                            ServiceLogger.Error(String.Format("An Error has occured in CreateTicketToJiraAccount method, the details of which are: {0}, Inner Exception: {1}," +
                                                            " Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace
                                                            + " current context key : " + ServiceContext.currentThreadContextKeyString), JiraTicketServiceLogger);
                            ServiceLogger.Info("Ending CreateTicketToJiraAccount...", JiraTicketServiceLogger);
                        }

                        if (jiraIssue.IsNotNull())
                        {
                            ExternalVendorOrderManager.UpdateBkgOrderIgnoredCreateJiraTicket(tenantID, orderToCreateJiraTicket.BkgOrderID);
                        }

                        CreateJiraTicketDetail(tenantID, orderToCreateJiraTicket.OrderID, jiraIssue, jiraIssueFailMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                ServiceLogger.Error(String.Format("An Error has occured in CreateJiraTicket method, the details of which are: {0}, Inner Exception: {1}," +
                                                " Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace
                                                + " current context key : " + ServiceContext.currentThreadContextKeyString), JiraTicketServiceLogger);
            }

            ServiceLogger.Info("Ending CreateJiraTicket...", JiraTicketServiceLogger);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jiraTicketSummary"></param>
        /// <param name="jiraTicketDescription"></param>
        /// <returns></returns>
        private static Issue CreateTicketToJiraAccount(String jiraTicketSummary, String jiraTicketDescription)
        {
            //Create a connection to JIRA
            var jira = new Jira(jiraURL, jiraUserName, jiraPassword);

            //Create issue in JIRA
            Issue issue = jira.CreateIssue(jiraProjectName);
            issue.Type = jiraIssueType;
            issue.Priority = jiraIssuePriority;
            issue.Summary = jiraTicketSummary;
            issue.Description = jiraTicketDescription;
            issue.Reporter = jiraReporter;
            issue.SaveChanges();
            return issue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tenantID"></param>
        /// <param name="orderID"></param>
        /// <param name="jiraIssue"></param>
        /// <param name="jiraIssueFailMessage"></param>
        private static void CreateJiraTicketDetail(Int32 tenantID, Int32 orderID, Issue jiraIssue, String jiraIssueFailMessage)
        {
            try
            {
                ServiceLogger.Info("Starting CreateJiraTicketDetail...", JiraTicketServiceLogger);
                JiraTicketDetail jtd = new JiraTicketDetail();
                if (jiraIssue.IsNotNull())
                {
                    jtd.OrderID = orderID;
                    jtd.JiraDescription = jiraIssue.Description;
                    jtd.JiraSummary = jiraIssue.Summary;
                    jtd.JiraTicketID = jiraIssue.Key.Value;
                    jtd.JiraReporter = jiraIssue.Reporter;
                    jtd.JiraIdentifier = jiraIssue.JiraIdentifier;
                    jtd.IsDeleted = false;
                }
                else
                {
                    jtd.OrderID = orderID;
                    jtd.JiraDescription = jiraIssueFailMessage;
                    jtd.IsDeleted = false;
                }
                ExternalVendorOrderManager.CreateJiraTicketDetail(tenantID, jtd);
            }
            catch (Exception ex)
            {
                ServiceLogger.Error(String.Format("An Error has occured in CreateJiraTicketDetail method, the details of which are: {0}, Inner Exception: {1}," +
                                                " Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace
                                                + " current context key : " + ServiceContext.currentThreadContextKeyString), JiraTicketServiceLogger);
            }
            ServiceLogger.Info("Ending CreateJiraTicketDetail...", JiraTicketServiceLogger);
        }

        #endregion

        #endregion
    }
}