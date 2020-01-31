using DataMart.Business.RepoManagers;
using DataMart.Models;
using DataMart.Utils;
using Entity;
using INTSOF.ServiceUtil;
using INTSOF.Utils;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace DataMartSyncService
{
    static class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static Boolean _isServiceLoggingEnabled = ConfigurationManager.AppSettings["IsServiceLoggingEnabled"].IsNotNull() ?
                                                            Convert.ToBoolean(ConfigurationManager.AppSettings["IsServiceLoggingEnabled"]) : false;
        private static Int32 _maxParallelThreads = ConfigurationManager.AppSettings["MaxParallelThreads"].IsNotNull() ?
                                                           Convert.ToInt32(ConfigurationManager.AppSettings["MaxParallelThreads"]) : 10;

        private static Int32 _dataChunkSize = ConfigurationManager.AppSettings["DataChunkSize"].IsNotNull() ?
                                                           Convert.ToInt32(ConfigurationManager.AppSettings["DataChunkSize"]) : 100;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            ServiceContext.init();
            if (args.Count() >= AppConsts.ONE)
            {
                logger.Info("Initialize task for Data Mart sync");
                String taskName = args[AppConsts.NONE];

                if (taskName.Equals(ScheduledTasks.AgencyUsers.GetStringValue()))
                {
                    logger.Info("Entered processing " + taskName);
                    SyncAgencyUsers();
                }
                if (taskName.Equals(ScheduledTasks.InvitationGroups.GetStringValue()))
                {
                    logger.Info("Entered processing " + taskName);
                    SyncInvitationGroups();
                }
                if (taskName.Equals(ScheduledTasks.RotationDetails.GetStringValue()))
                {
                    logger.Info("Entered processing " + taskName);
                    SyncRotationDetails();
                }
                if (taskName.Equals(ScheduledTasks.InitializeCollections.GetStringValue()))
                {
                    logger.Info("Entered processing " + taskName);
                    InitializeCollections();
                }
            }
        }

        private static void SyncAgencyUsers()
        {
            DateTime currentDate = DateTime.Now;
            logger.Debug(ScheduledTasks.AgencyUsers.GetStringValue() + "==> Current Date is: " + currentDate.ToString());
            List<Int32> modifiedAgencyUsers = new List<Int32>();
            try
            {
                logger.Info("Task invoked for agency user sync");
                logger.Debug(ScheduledTasks.AgencyUsers.GetStringValue() + "==> Getting the last sync date for this collection.");
                DateTime lastSyncDate = DataMartManager.GetLastSyncDate(DataMartCollections.AgencyUsers.GetStringValue());
                lastSyncDate = lastSyncDate.ToLocalTime();
                logger.Debug(ScheduledTasks.AgencyUsers.GetStringValue() + "==> Last Sync Date for this collection is: " + lastSyncDate.ToString());
                logger.Debug(ScheduledTasks.AgencyUsers.GetStringValue() + "==> Getting the agency users that have been impacted after last sync.");
                List<AgencyUser> agencyUsers = DataMartManager.GetAgencyUsers(lastSyncDate);
                logger.Debug(ScheduledTasks.AgencyUsers.GetStringValue() + "==> Got the agency users and their count is: " + agencyUsers.Count);
                logger.Debug(ScheduledTasks.AgencyUsers.GetStringValue() + "==> Saving the agency users in the warehouse.");
                DataMartManager.SaveAgencyUsers(agencyUsers);
                modifiedAgencyUsers.AddRange(agencyUsers.Select(x => x.AgencyUserID));
                logger.Debug(ScheduledTasks.AgencyUsers.GetStringValue() + "==> Saved the agency users in the warehouse.");
                logger.Debug(ScheduledTasks.AgencyUsers.GetStringValue() + "==> Locking the write version so that it is not impacted anymore.");
                DataMartManager.LockAgencyUsers(modifiedAgencyUsers, currentDate, true);
                logger.Debug(ScheduledTasks.AgencyUsers.GetStringValue() + "==> Locked the write version and create a copy of it for future modifications.");
                logger.Info("Task completed for agency user sync");
            }
            catch (Exception ex)
            {
                logger.Error("An Error has occured in agency user sync, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                DataMartManager.LockAgencyUsers(modifiedAgencyUsers, currentDate, false);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
            }
        }

        private static void InitializeCollections()
        {
            try
            {
                logger.Info("Task invoked for initiliazing collections");
                DataMartManager.InitializeCollections();
                logger.Info("Task completed for initiliazing collections");
            }
            catch (Exception ex)
            {
                logger.Error("An Error has occured in initiliazing collections, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
            }
        }

        private static void SyncInvitationGroups()
        {
            DateTime currentDate = DateTime.Now;
            logger.Debug(ScheduledTasks.InvitationGroups.GetStringValue() + "==> Current Date is: " + currentDate.ToString());
            List<Int32> modifiedInvitationGroups = new List<Int32>();
            try
            {
                logger.Info("Task invoked for invitation group sync");
                logger.Debug(ScheduledTasks.InvitationGroups.GetStringValue() + "==> Getting the last sync date for this collection.");
                DateTime lastSyncDate = DataMartManager.GetLastSyncDate(DataMartCollections.SharedItems.GetStringValue());
                lastSyncDate = lastSyncDate.ToLocalTime();
                logger.Debug(ScheduledTasks.InvitationGroups.GetStringValue() + "==> Last Sync Date for this collection is: " + lastSyncDate.ToString());
                logger.Debug(ScheduledTasks.InvitationGroups.GetStringValue() + "==> Getting the list of all tenants for which this operation is going to run.");
                List<ClientDBConfiguration> clientDbConfs = DataMartManager.GetClientDBConfiguration().Select(
                item =>
                {
                    ClientDBConfiguration config = new ClientDBConfiguration();
                    config.CDB_TenantID = item.CDB_TenantID;
                    config.CDB_ConnectionString = item.CDB_ConnectionString;
                    config.Tenant = new Tenant();
                    config.Tenant.TenantName = item.Tenant.TenantName; return config;
                }).ToList();
                logger.Debug(ScheduledTasks.InvitationGroups.GetStringValue() + "==> Got the list, now starting the paralled threads for each tenant in the list.");
                Parallel.ForEach(clientDbConfs, new ParallelOptions { MaxDegreeOfParallelism = _maxParallelThreads }, config =>
                {
                    logger.Debug(ScheduledTasks.InvitationGroups.GetStringValue() + "==> Thread started for Tenant ID: " + config.CDB_TenantID + " and Tenant Name: " + config.Tenant.TenantName);
                    logger.Debug(ScheduledTasks.InvitationGroups.GetStringValue() + "==> Getting the modified Invitation Groups for Tenant ID: " + config.CDB_TenantID + " and Tenant Name: " + config.Tenant.TenantName);
                    var invitationGroups = DataMartManager.GetProfileSharingInvitationGroups(config.CDB_TenantID, lastSyncDate);
                    logger.Debug(ScheduledTasks.InvitationGroups.GetStringValue() + "==> Got " + invitationGroups.Count + " modified Invitation Groups for Tenant ID: " + config.CDB_TenantID + " and Tenant Name: " + config.Tenant.TenantName);
                    while(invitationGroups.IsNotNull() && invitationGroups.Count > 0)
                    {
                        List<Int32> topChunk = invitationGroups.Take(_dataChunkSize).ToList();
                        String invitationGroupIds = String.Join(",", topChunk);
                        List<SharedItem> sharedItems = new List<SharedItem>();
                        logger.Debug(ScheduledTasks.InvitationGroups.GetStringValue() + "==> Thread started for Invitation Group ID: " + invitationGroupIds + " and Tenant ID: " + config.CDB_TenantID + " and Tenant Name: " + config.Tenant.TenantName);
                        logger.Debug(ScheduledTasks.InvitationGroups.GetStringValue() + "==> Getting shared items for Invitation Group ID: " + invitationGroupIds + " and Tenant ID: " + config.CDB_TenantID + " and Tenant Name: " + config.Tenant.TenantName);
                        sharedItems = DataMartManager.GetSharedItemsOfInvitationGroup(config.CDB_TenantID, invitationGroupIds);
                        logger.Debug(ScheduledTasks.InvitationGroups.GetStringValue() + "==> Got " + sharedItems.Count + " shared items for Invitation Group ID: " + invitationGroupIds + " and Tenant ID: " + config.CDB_TenantID + " and Tenant Name: " + config.Tenant.TenantName);
                        logger.Debug(ScheduledTasks.InvitationGroups.GetStringValue() + "==> Saving " + sharedItems.Count + " shared items for Invitation Group ID: " + invitationGroupIds + " and Tenant ID: " + config.CDB_TenantID + " and Tenant Name: " + config.Tenant.TenantName);
                        DataMartManager.SaveSharedItems(sharedItems, topChunk, config.CDB_TenantID);
                        logger.Debug(ScheduledTasks.InvitationGroups.GetStringValue() + "==> Successfully saved " + sharedItems.Count + " shared items for Invitation Group ID: " + invitationGroups + " and Tenant ID: " + config.CDB_TenantID + " and Tenant Name: " + config.Tenant.TenantName);
                        modifiedInvitationGroups.AddRange(sharedItems.Select(x => x.InvitationGroupID));
                        invitationGroups.RemoveAll(x => topChunk.Contains(x));
                    }
                });
                logger.Debug(ScheduledTasks.InvitationGroups.GetStringValue() + "==> Locking the write version so that it is not impacted anymore.");
                DataMartManager.LockInvitationGroups(modifiedInvitationGroups, currentDate, true);
                logger.Debug(ScheduledTasks.InvitationGroups.GetStringValue() + "==> Locked the write version so that it is not impacted anymore.");
                logger.Info("Task completed for invitation group sync");
            }
            catch (Exception ex)
            {
                logger.Error("An Error has occured in invitation group sync, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                DataMartManager.LockInvitationGroups(modifiedInvitationGroups, currentDate, false);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
            }
        }

        private static void SyncRotationDetails()
        {
            DateTime currentDate = DateTime.Now;
            logger.Debug(ScheduledTasks.RotationDetails.GetStringValue() + "==> Current Date is: " + currentDate.ToString());
            List<Int32> modifiedRotationDetails = new List<Int32>();
            try
            {
                logger.Info("Task invoked for rotation details sync");
                logger.Debug(ScheduledTasks.RotationDetails.GetStringValue() + "==> Getting the last sync date for this collection.");
                DateTime lastSyncDate = DataMartManager.GetLastSyncDate(DataMartCollections.RotationDetails.GetStringValue());
                lastSyncDate = lastSyncDate.ToLocalTime();
                logger.Debug(ScheduledTasks.RotationDetails.GetStringValue() + "==> Last Sync Date for this collection is: " + lastSyncDate.ToString());
                logger.Debug(ScheduledTasks.RotationDetails.GetStringValue() + "==> Getting the list of all tenants for which this operation is going to run.");
                List<ClientDBConfiguration> clientDbConfs = DataMartManager.GetClientDBConfiguration().Select(
                item =>
                {
                    ClientDBConfiguration config = new ClientDBConfiguration();
                    config.CDB_TenantID = item.CDB_TenantID;
                    config.CDB_ConnectionString = item.CDB_ConnectionString;
                    config.Tenant = new Tenant();
                    config.Tenant.TenantName = item.Tenant.TenantName; return config;
                }).ToList();
                logger.Debug(ScheduledTasks.RotationDetails.GetStringValue() + "==> Got the list, now starting the paralled threads for each tenant in the list.");
                Parallel.ForEach(clientDbConfs, new ParallelOptions { MaxDegreeOfParallelism = _maxParallelThreads }, config =>
                {
                    logger.Debug(ScheduledTasks.RotationDetails.GetStringValue() + "==> Thread started for Tenant ID: " + config.CDB_TenantID + " and Tenant Name: " + config.Tenant.TenantName);
                    logger.Debug(ScheduledTasks.RotationDetails.GetStringValue() + "==> Getting the modified rotation details for Tenant ID: " + config.CDB_TenantID + " and Tenant Name: " + config.Tenant.TenantName);
                    var RotationDetails = DataMartManager.GetModifiedRotationDetails(config.CDB_TenantID, lastSyncDate);
                    logger.Debug(ScheduledTasks.RotationDetails.GetStringValue() + "==> Got " + RotationDetails.Count + " modified rotation details for Tenant ID: " + config.CDB_TenantID + " and Tenant Name: " + config.Tenant.TenantName);
                    while(RotationDetails.IsNotNull() && RotationDetails.Count > 0)
                    {
                        List<RotationDetail> rotationDetails = new List<RotationDetail>();
                        List<Int32> topChunk = RotationDetails.Take(_dataChunkSize).ToList();
                        String invitationGroups = String.Join(",", topChunk);
                        logger.Debug(ScheduledTasks.RotationDetails.GetStringValue() + "==> Thread started for invitation group ID: " + invitationGroups + " and Tenant ID: " + config.CDB_TenantID + " and Tenant Name: " + config.Tenant.TenantName);
                        logger.Debug(ScheduledTasks.RotationDetails.GetStringValue() + "==> Getting rotation details for invitation group ID: " + invitationGroups + " and Tenant ID: " + config.CDB_TenantID + " and Tenant Name: " + config.Tenant.TenantName);
                        rotationDetails = DataMartManager.GetRotationDetailsOfInvitationGroup(config.CDB_TenantID, invitationGroups);
                        logger.Debug(ScheduledTasks.RotationDetails.GetStringValue() + "==> Got " + rotationDetails.Count + " rotation details for invitation group ID: " + invitationGroups + " and Tenant ID: " + config.CDB_TenantID + " and Tenant Name: " + config.Tenant.TenantName);
                        logger.Debug(ScheduledTasks.RotationDetails.GetStringValue() + "==> Saving " + rotationDetails.Count + " rotation details for invitation group ID: " + invitationGroups + " and Tenant ID: " + config.CDB_TenantID + " and Tenant Name: " + config.Tenant.TenantName);
                        DataMartManager.SaveRotationDetails(rotationDetails, topChunk, config.CDB_TenantID);
                        logger.Debug(ScheduledTasks.RotationDetails.GetStringValue() + "==> Successfully saved " + rotationDetails.Count + " rotation details for invitation group ID: " + invitationGroups + " and Tenant ID: " + config.CDB_TenantID + " and Tenant Name: " + config.Tenant.TenantName);
                        modifiedRotationDetails.AddRange(rotationDetails.Select(x => x.InvitationGroupID));
                        RotationDetails.RemoveAll(x => topChunk.Contains(x));
                    }
                });
                logger.Debug(ScheduledTasks.RotationDetails.GetStringValue() + "==> Locking the write version so that it is not impacted anymore.");
                DataMartManager.LockRotationDetails(modifiedRotationDetails, currentDate, true);
                logger.Debug(ScheduledTasks.RotationDetails.GetStringValue() + "==> Locked the write version so that it is not impacted anymore.");
                logger.Info("Task completed for rotation detail sync");
            }
            catch (Exception ex)
            {
                logger.Error("An Error has occured in rotation detail sync, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                DataMartManager.LockRotationDetails(modifiedRotationDetails, currentDate, false);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
            }
        }
    }
}
