using Business.RepoManagers;
using Entity;
using INTSOF.ServiceUtil;
using INTSOF.UI.Contract.Services;
using INTSOF.Utils;
using INTSOF.Utils.Consts;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.ServiceProcess;
using System.Timers;
using System.Data.SqlClient;
using EmailDispatcherService.ComplioTalkDesk;
using System.Threading.Tasks;

namespace EmailDispatcherService
{
    public partial class EmailDispatcherService : ServiceBase
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static Boolean _isServiceLoggingEnabled = ConfigurationManager.AppSettings["IsServiceLoggingEnabled"].IsNotNull() ?
                                                            Convert.ToBoolean(ConfigurationManager.AppSettings["IsServiceLoggingEnabled"]) : false;

        private Timer scheduleTimer = null;
        private Boolean isExecuting = false;
        private Object lockObject = new Object();

        private Timer scheduleNotificationTimer = null;
        private Boolean isNotificationExecuting = false;
        private Object lockNotificationObject = new Object();

        private Timer complianceNotificationTimer = null;
        private Boolean isComplianceExecuting = false;
        private Object lockComplianceObject = new Object();

        private Timer scheduleActionTimer = null;
        private Boolean isActionExecuting = false;
        private Object lockActionObject = new Object();

        private Timer mobilityInstanceTimer = null;
        private Boolean isMobilityExecuting = false;
        private Object lockMobilityObject = new Object();

        private Timer copyPackageDataTimer = null;
        private Boolean isCopyPackageDataExecuting = false;
        private Object lockCopyPackageDataObject = new Object();

        private Timer adminWidgetDataTimer = null;
        private Boolean isAdminWidgetDataExecuting = false;
        private Object lockAdminWidgetDataObject = new Object();

        private Timer systemServiceTriggerTimer = null;
        private Boolean isSystemServiceTriggerExecuting = false;
        private Object lockSystemServiceTriggerObject = new Object();

        private Timer nagEmailTimer = null;
        private Boolean isNagEmailExecuting = false;
        private Object lockNagEmailObject = new Object();

        private Timer queueImagingTimer = null;
        private Boolean isQueueImagingExecuting = false;
        private Object lockQueueImagingObject = new Object();

        private Timer deadlineEmailTimer = null;
        private Boolean isDeadlineEmailExecuting = false;
        private Object lockDeadlineEmailObject = new Object();

        private Timer scheduleTaskTimer = null;
        private Boolean isScheduleTaskExecuting = false;
        private Object lockScheduleTaskObject = new Object();

        private Timer categoryComplianceRqdTimer = null;
        private Boolean isCategoryComplianceRqdExecuting = false;
        private Object lockCategoryComplianceRqdObject = new Object();

        private Timer requirementTimer = null;
        private Boolean isRequirementExecuting = false;
        private Object lockRequirementObject = new Object();

        //UAT-1415- Scheduled Invitation Email Service
        private Timer scheduleInvitationTimer = null;
        private Boolean isScheduleInvitationJobExecuting = false;
        private Object lockScheduleInvitationObject = new Object();

        private Timer rotationTimer = null;
        private Boolean isRotationExecuting = false;
        private Object lockRotationObject = new Object();

        private Timer incompletedOnlineOrderNotificationTimer = null;
        private Boolean isIncompletedOnlineOrderNotificationExecuting = false;
        private Object lockIncompletedOnlineOrderNotification = new Object();

        private Timer manageContractExpiringItemsNotificationTimer = null;
        private Boolean isManageContractExpiringItemsNotificationExecuting = false;
        private Object lockManageContractExpiringItemsNotification = new Object();

        private Timer manageContractExpiredItemsNotificationTimer = null;
        private Boolean isManageContractExpiredItemsNotificationExecuting = false;
        private Object lockManageContractExpiredItemsNotification = new Object();

        private Timer markApplicantDocumentsCompleteTimer = null;
        private Boolean ismarkApplicantDocumentsCompleteExecuting = false;
        private Object lockMarkApplicantDocumentsComplete = new Object();

        //UAT-963: WB: As an ADB admin, I should be able to search one, many, or all institutions on the admin data audit history search
        private Timer ComplianceAuditDataSyncTimer = null;
        private Boolean isComplianceAuditDataSyncExecuting = false;
        private Object lockComplianceAuditDataSync = new Object();

        //UAT-2495
        private Timer ClientDataUploadTimer = null;
        private Boolean isClientDataUploadExecuting = false;
        private Object lockClientDataUpload = new Object();

        private Timer ReconcillationQueueSyncTimer = null;
        private Boolean isReconcillationQueueSyncExecuting = false;
        private Object lockReconcillationQueueSyncComplete = new Object();

        private Timer reqScheduleActionTimer = null;
        private Boolean isReqScheduleActionExecuting = false;
        private Object lockReqScheduleActionObject = new Object();

        private Timer reqCategoryComplianceRqdTimer = null;
        private Boolean isReqCategoryComplianceRqdExecuting = false;
        private Object lockReqCategoryComplianceRqdObject = new Object();

        private Timer copyComplianceDataToRequirementTimer = null;
        private Boolean isCopyComplianceDataToRequirementExecuting = false;
        private Object lockCopyComplianceDataToRequirementObject = new Object();

        //UAT-2514 
        private Timer requirementPkgSyncTimer = null;
        private Boolean isRequirementPkgSyncExecuting = false;
        private Object lockRequirementPkgSyncObject = new Object();

        //UAT-2533 Archive all the packages whose end date has passed.
        private Timer requirementPkgAutoArchiveTimer = null;
        private Boolean isRequirementPkgAutoArchiveExecuting = false;
        private Object lockRequirementPkgAutoArchiveObject = new Object();

        //UAT-2414, Create a snapshot on Rotation End Date
        private Timer createRequirementSnapshotOnRotationEndTimer = null;
        private Boolean isCreateRequirementSnapshotOnRotationEndExecuting = false;
        private Object lockCreateRequirementSnapshotOnRotationEndObject = new Object();

        //UAT-2603
        private Timer rotDataMovementTimer = null;
        private Boolean isRotDataMovementExecuting = false;
        private Object lockRotDataMovementObject = new Object();

        //UAT-3112
        private Boolean isBadgeFormNotificationInterval = false;
        private Timer badgeFrmNotifTimer = null;
        private Boolean isBadgeFormNotificationExecuting = false;
        private object lockBadgeFormNotificationObject = new object();

        //UAT-3669
        private Boolean isAlertMailForWebCCFErrorInterval = false;
        private Timer alertMailForWebCCFErrorTimer = null;
        private Boolean isAlertMailForWebCCFErrorExecuting = false;
        private object lockAlertMailForWebCCFErrorObject = new object();

        //UAT-2960
        private Boolean isAlumniAccessNotificationInterval = false;
        private Timer alumniAccessNotifTimer = null;
        private Boolean isAlumniAccessNotificationExecuting = false;
        private object lockAlumniAccessNotificationObject = new object();

        //UAT-2960- Copy Compliance to Compliance 

        // private Boolean isCopyComplianceToComplianceInterval = false;
        private Timer copyComplianceToComplianceTimer = null;
        private Boolean isCopyComplianceToComplianceExecuting = false;
        private Object lockCopyComplianceToComplianceObject = new Object();

        //UAT-2628:
        private Timer ConversionMergingForFailedDocumentTimer = null;
        private Boolean isConversionMergingForFailedDocumentExecuting = false;
        private Object lockConversionMergingForFailedDocument = new Object();

        //UAT-2388 AutomaticPackageInvitation
        private Timer AutomaticPackageInvitationTimer = null;
        private Boolean isAutomaticPackageInvitationExecuting = false;
        private Object lockAutomaticPackageInvitationObject = new Object();

        //UAT-3059 UpdatedApplicantRequirementsNotification
        private Timer UpdatedApplicantRequirementsNotificationTimer = null;
        private Boolean isUpdatedApplicantRequirementsNotificationExecuting = false;
        private Object lockUpdatedApplicantRequirementsNotificationObject = new Object();

        //UAT-2513 Feature for Client Admin to batch upload rotation creation
        private Timer BatchRotationUploadTimer = null;
        private Boolean isBatchRotationUploadExecuting = false;
        private Object lockBatchRotationUploadObject = new Object();

        private Boolean isReminderInterval = false;
        private Boolean isComplianceInterval = false;
        private Boolean isScheduleActionInterval = false;
        private Boolean isMobilityInstanceInterval = false;
        private Boolean isCopyPackageDataInterval = false;
        private Boolean isAdminWidgetDataInterval = false;
        private Boolean isQueueImagingInterval = false;
        private Boolean isSystemServiceTriggerInterval = false;
        private Boolean isNagEmailInterval = false;
        private Boolean isDeadlineEmailInterval = false;
        private Boolean isScheduleTaskInterval = false;
        private Boolean isCategoryComplianceRqdInterval = false;
        private Boolean isRequirementInterval = false;
        private Boolean isScheduleInvitationInterval = false;
        private Boolean isRotationInterval = false;
        private Boolean isIncompletedOnlineOrderNotificationInterval = false;
        private Boolean isManageContractExpiringItemsNotificationInterval = false;
        private Boolean isManageContractExpiredItemsNotificationInterval = false;
        private Boolean isMarkApplicantDocumentsCompleteInterval = false;
        private Boolean isReconcillationQueueSyncInterval = false;
        private Boolean isReqScheduleActionInterval = false;
        private Boolean isReqCategoryComplianceRqdInterval = false;
        private Boolean isCopyComplianceDataTorequirementInterval = false;
        private Boolean isRequirementPkgSyncInterval = false;
        private Boolean isRequirementPkgAutoArchiveInterval = false;
        private Boolean isCreateRequirementSnapshotOnRotationEndInterval = false;
        private Boolean isRotationDataMovementInterval = false;
        private Boolean isCopyComplianceToComplianceInterval = false;
        private Boolean isBkgCopyPackageDataInterval = false;
        private Boolean isBkgDigestionProcedureCallInterval = false;
       // private Int32 emailCounter = 0;
        private DateTime serviceStartTime = DateTime.Now;
        private Boolean isReceivedFromStuServFormStatusInterval = false;



        //UAT-2628:
        private Boolean isConversionMergingForFailedDocumentInterval = false;

        private Boolean isAutomaticPackageInvitationInterval = false;//UAT-2388

        private Boolean isUpdatedApplicantRequirementsNotificationInterval = false;//UAT-3059

        //UAT-963: WB: As an ADB admin, I should be able to search one, many, or all institutions on the admin data audit history search
        private Boolean isComplianceAuditDataSyncInterval = false;

        //UAT-2495
        private Boolean isClientDataUploadInterval = false;
        private Boolean isBatchRotationUploadInterval = false;//UAT-2513 Feature for Client Admin to batch upload rotation creation

        // private Boolean isCopyComplianceToComplianceInterval = false;
        private Timer bkgCopyPackageDataTimer = null;
        private Boolean isbkgCopyPackageDataExecuting = false;
        private Object lockbkgCopyPackageDataObject = new Object();
        //UAT-3545
        private Timer bkgDigestionProcedureTimer = null;
        private Boolean isbkgDigestionProcedureExecuting = false;
        private Object lockbkgDigestionProcedureObject = new Object();


        //UAT-3820
        private Timer receivedFromStuServFormStatusDataTimer = null;
        private Boolean isReceivedFromStuServFormStatusDataExecuting = false;
        private Object lockbkgReceivedFromStuServFormStatusDataObject = new Object();

        //UAT-3485
        private Boolean isSendRotationAbtToExpireInterval = false;
        private Timer sendRotationAbtToExpireTimer = null;
        private Boolean isSendRotationAbtToExpireExecuting = false;
        private Object lockSendRotationAbtToExpireObject = new Object();

        #region Missed Appointment
        private Timer MissedAppointmentEmailTimer = null;
        private Boolean isMissedAppointmentEmailInterval = false;
        private Object lockMissedAppointmentEmailObject = new Object();
        private Boolean isMissedAppointmentEmailExecuting = false;

        private Timer MissedAppointmentTimer = null;
        private Boolean isMissedAppointmentInterval = false;
        private Object lockMissedAppointmentObject = new Object();
        private Boolean isMissedAppointmentExecuting = false;
        #endregion

        private Timer OffTimeRevokedAppointmentEmailTimer = null;
        private Boolean isOffTimeRevokedAppointmentEmailInterval = false;
        private Object lockOffTimeRevokedAppointmentEmailObject = new Object();
        private Boolean isOffTimeRevokedAppointmentEmailExecuting = false;
        //UAT 3950
        private Timer ArchiveRotationDataTimer = null;
        private Boolean isArchiveRotationDataTimerInterval = false;
        private Boolean isArchiveRotationDataExecuting = false;
        private Object lockArchiveRotationData = new Object();


        private Boolean isBKgCompletedOrderToClientAdminInterval = false;
        private Timer BKgCompletedOrderToClientAdminTimer = null;
        private Boolean isBKgCompletedOrderToClientAdminExecuting = false;
        private Object lockBKgCompletedOrderToClientAdminObject = new Object();




        public EmailDispatcherService()
        {

            ServiceContext.init();
            InitializeComponent();

            //Get the current Time
            TimeSpan CurrentTime = DateTime.Now.TimeOfDay;

            logger.Info("Initialize Timer for dispatching emails");
            if (ServiceInterval != SysXDBConsts.MINUS_ONE)
            {
                logger.Info("entered for Initializing Timer for dispatching emails");
                scheduleTimer = new Timer();
                scheduleTimer.Interval = Convert.ToDouble(ServiceInterval);
                scheduleTimer.Elapsed += new ElapsedEventHandler(scheduleTimer_Elapsed);
                logger.Info("Timer elapsed for dispatching emails");
            }

            logger.Info("Initialize Timer for reminder emails");
            if (ReminderServiceInterval != SysXDBConsts.MINUS_ONE)
            {
                logger.Info("entered for Initializing Timer for  reminder emails");
                scheduleNotificationTimer = new Timer();

                //Get the Reminder Service Interval for first time
                scheduleNotificationTimer.Interval = firstTimerInterval(ReminderStartTime, ReminderEndTime, CurrentTime, 1);

                scheduleNotificationTimer.Elapsed += new ElapsedEventHandler(notificationTimer_Elapsed);
                logger.Info("Timer elapsed for  reminder emails");
            }

            logger.Info("Initialize Timer for compliance item emails");
            if (ComplianceServiceInterval != SysXDBConsts.MINUS_ONE)
            {
                logger.Info("entered for Initializing Timer for compliance item  emails");
                complianceNotificationTimer = new Timer();

                //Get the Compliance Service Interval for first time
                complianceNotificationTimer.Interval = (firstTimerInterval(ComplianceStartTime, ComplianceEndTime, CurrentTime, 2));

                complianceNotificationTimer.Elapsed += new ElapsedEventHandler(complianceTimer_Elapsed);
                logger.Info("Timer elapsed for compliance item emails");
            }

            logger.Info("Initialize Timer for action emails");
            if (ScheduleActionServiceInterval != SysXDBConsts.MINUS_ONE)
            {
                logger.Info("entered for Initializing Timer for action emails");
                scheduleActionTimer = new Timer();

                //Get the Schedule Action Service Interval for first time
                scheduleActionTimer.Interval = (firstTimerInterval(ScheduleActionStartTime, ScheduleActionEndTime, CurrentTime, 3));

                scheduleActionTimer.Elapsed += new ElapsedEventHandler(actionTimer_Elapsed);
                logger.Info("Timer elapsed for compliance action emails");
            }

            logger.Info("Initialize Timer for Mobility Instance");
            if (MobilityInstanceServiceInterval != SysXDBConsts.MINUS_ONE)
            {
                logger.Info("entered for Initializing Timer for Mobility Instance");
                mobilityInstanceTimer = new Timer();

                //Get the Mobility Instance Service Interval for first time
                mobilityInstanceTimer.Interval = (firstTimerInterval(MobilityInstanceStartTime, MobilityInstanceEndTime, CurrentTime, 4));

                mobilityInstanceTimer.Elapsed += new ElapsedEventHandler(mobilityTimer_Elapsed);
                logger.Info("Timer elapsed for Mobility Instance");
            }

            logger.Info("Initialize Timer for Copy Package Data");
            if (CopyPackageDataServiceInterval != SysXDBConsts.MINUS_ONE)
            {
                logger.Info("entered for Initializing Timer for Copy Package Data");
                copyPackageDataTimer = new Timer();

                //Get the Mobility Instance Service Interval for first time
                copyPackageDataTimer.Interval = (firstTimerInterval(CopyPackageDataStartTime, CopyPackageDataEndTime, CurrentTime, 5));

                copyPackageDataTimer.Elapsed += new ElapsedEventHandler(copyPackageDataTimer_Elapsed);
                logger.Info("Timer elapsed for Copy Package Data");
            }

            logger.Info("Initialize Timer for Admin Widget Data");
            if (AdminWidgetServiceInterval != SysXDBConsts.MINUS_ONE)
            {
                logger.Info("entered for Initializing Timer for Admin Widget Data");
                adminWidgetDataTimer = new Timer();

                //Get the Admin Widget Data Service Interval for first time
                adminWidgetDataTimer.Interval = (firstTimerInterval(AdminWidgetStartTime, AdminWidgetEndTime, CurrentTime, 6));

                adminWidgetDataTimer.Elapsed += new ElapsedEventHandler(adminWidgetDataTimer_Elapsed);
                logger.Info("Timer elapsed for Admin Widget Data");
            }

            logger.Info("Initialize Timer for System Service Triggers");
            if (SystemServiceTriggerInterval != SysXDBConsts.MINUS_ONE)
            {
                logger.Info("entered for Initializing Timer for System Service Triggers");
                systemServiceTriggerTimer = new Timer();

                //Get the System Service Trigger Interval for first time
                systemServiceTriggerTimer.Interval = (firstTimerInterval(SystemServiceTriggerStartTime, SystemServiceTriggerEndTime, CurrentTime, 7)); //Convert.ToDouble(SystemServiceTriggerInterval);

                systemServiceTriggerTimer.Elapsed += new ElapsedEventHandler(systemServiceTriggerTimer_Elapsed);
                logger.Info("Timer elapsed for System Service Triggers");
            }

            logger.Info("Initialize Timer for Nag Email");
            if (NagEmailInterval != SysXDBConsts.MINUS_ONE)
            {
                logger.Info("entered for Initializing Timer for Nag Email");
                nagEmailTimer = new Timer();

                //Get the Nag Email Interval for first time
                nagEmailTimer.Interval = (firstTimerInterval(NagEmailStartTime, NagEmailEndTime, CurrentTime, 8));

                nagEmailTimer.Elapsed += new ElapsedEventHandler(nagEmailTimer_Elapsed);
                logger.Info("Timer elapsed for Nag Email");
            }

            //UAT-718

            logger.Info("Initialize Timer for queue imaging");
            if (QueueImagingInterval != SysXDBConsts.MINUS_ONE)
            {
                logger.Info("entered for Initializing Timer for  queue imaging");
                queueImagingTimer = new Timer();

                //Get the queue imaging Interval for first time
                queueImagingTimer.Interval = (firstTimerInterval(QueueImagingStartTime, QueueImagingEndTime, CurrentTime, 9));

                queueImagingTimer.Elapsed += new ElapsedEventHandler(QueueImagingTimer_Elapsed);
                logger.Info("Timer elapsed for  queue imaging");
            }

            logger.Info("Initialize Timer for Deadline Email");
            if (DeadlineEmailInterval != SysXDBConsts.MINUS_ONE)
            {
                logger.Info("entered for Initializing Timer for Deadline Email");
                deadlineEmailTimer = new Timer();

                //Get the Deadline Email Interval for first time
                deadlineEmailTimer.Interval = (firstTimerInterval(DeadlineEmailStartTime, DeadlineEmailEndTime, CurrentTime, 10));

                deadlineEmailTimer.Elapsed += new ElapsedEventHandler(deadlineEmailTimer_Elapsed);
                logger.Info("Timer elapsed for Deadline Email");
            }

            logger.Info("Initialize Timer for invoice order approval");
            if (ScheduleTaskServiceInterval != SysXDBConsts.MINUS_ONE)
            {
                logger.Info("entered for Initializing Timer for invoice order approval");
                scheduleTaskTimer = new Timer();

                //Get the Compliance Service Interval for first time
                scheduleTaskTimer.Interval = (firstTimerInterval(ScheduleTaskStartTime, ScheduleTaskEndTime, CurrentTime, 11));

                scheduleTaskTimer.Elapsed += new ElapsedEventHandler(scheduleTask_Elapsed);
                logger.Info("Timer elapsed for invoice order approval");
            }

            logger.Info("Initialize Timer for Category Compliance Rqd");
            if (CategoryComplianceRqdServiceInterval != SysXDBConsts.MINUS_ONE)
            {
                logger.Info("entered for Initializing Timer for Category Compliance Rqd");
                categoryComplianceRqdTimer = new Timer();

                //Get the Compliance Service Interval for first time
                categoryComplianceRqdTimer.Interval = (firstTimerInterval(CategoryComplianceRqdStartTime, CategoryComplianceRqdEndTime, CurrentTime, 12));

                categoryComplianceRqdTimer.Elapsed += new ElapsedEventHandler(categoryComplianceRqd_Elapsed);
                logger.Info("Timer elapsed for Category Compliance Rqd");
            }

            logger.Info("Initialize Timer for Requirement data");
            if (RequiremenServiceInterval != SysXDBConsts.MINUS_ONE)
            {
                logger.Info("entered for Initializing Timer for dispatching emails");
                requirementTimer = new Timer();
                requirementTimer.Interval = (firstTimerInterval(RequirementStartTime, RequirementEndTime, CurrentTime, 13));
                requirementTimer.Elapsed += new ElapsedEventHandler(requirement_Elapsed);
                logger.Info("Timer elapsed for Requirement data");
            }

            //UAT-1415- Scheduled Invitation Email Service
            logger.Info("Initialize Timer for Scheduled Invitation");
            if (ScheduledInvitationServiceInterval != SysXDBConsts.MINUS_ONE)
            {
                logger.Info("entered for Initializing Timer for dispatching invitation emails");
                scheduleInvitationTimer = new Timer();
                scheduleInvitationTimer.Interval = (firstTimerInterval(ScheduledInvitationStartTime, ScheduledInvitationEndTime, CurrentTime, 14));
                scheduleInvitationTimer.Elapsed += new ElapsedEventHandler(ScheduledInvitation_Elapsed);
                logger.Info("Timer elapsed for scheduled invitations");
            }

            logger.Info("Initialize Timer for Rotation Start Remainder");
            if (RotationServiceInterval != SysXDBConsts.MINUS_ONE)
            {
                logger.Info("entered for Initializing Timer for Rotation Start Remainder emails");
                rotationTimer = new Timer();
                rotationTimer.Interval = (firstTimerInterval(RotationStartTime, RotationEndTime, CurrentTime, 15));
                rotationTimer.Elapsed += new ElapsedEventHandler(rotation_Elapsed);
                logger.Info("Timer elapsed for Rotation start remainder");
            }

            logger.Info("Initialize Timer for Incomplete Online Orders");
            if (IncompletedOnlineOrderNotificationServiceInterval != SysXDBConsts.MINUS_ONE)
            {
                logger.Info("entered for Initializing Timer for Incomplete Online Orders");
                incompletedOnlineOrderNotificationTimer = new Timer();
                incompletedOnlineOrderNotificationTimer.Interval = (firstTimerInterval(IncompletedOnlineOrderNotificationStartTime, IncompletedOnlineOrderNotificationEndTime, CurrentTime, 16));
                incompletedOnlineOrderNotificationTimer.Elapsed += new ElapsedEventHandler(IncompletedOnlineOrderNotification_Elapsed);
                logger.Info("Timer elapsed for Incomplete Online Orders");
            }


            logger.Info("Initialize Timer for Manage Contract Expiring Items Notification");
            if (ManageContractExpiringItemsNotificationServiceInterval != SysXDBConsts.MINUS_ONE)
            {
                logger.Info("entered for Initializing Timer for Manage Contract Expiring Items Notification");
                manageContractExpiringItemsNotificationTimer = new Timer();
                manageContractExpiringItemsNotificationTimer.Interval = (firstTimerInterval(ManageContractExpiringItemsNotificationStartTime, ManageContractExpiringItemsNotificationEndTime, CurrentTime, 17));
                manageContractExpiringItemsNotificationTimer.Elapsed += new ElapsedEventHandler(ManageContractExpiringItemsNotification_Elapsed);
                logger.Info("Timer elapsed for Manage Contract Expiring Items Notification");
            }

            logger.Info("Initialize Timer for Manage Contract Expired Items Notification");
            if (ManageContractExpiredItemsNotificationServiceInterval != SysXDBConsts.MINUS_ONE)
            {
                logger.Info("entered for Initializing Timer for Manage Contract Expired Items Notification");
                manageContractExpiredItemsNotificationTimer = new Timer();
                manageContractExpiredItemsNotificationTimer.Interval = (firstTimerInterval(ManageContractExpiredItemsNotificationStartTime, ManageContractExpiredItemsNotificationEndTime, CurrentTime, 18));
                manageContractExpiredItemsNotificationTimer.Elapsed += new ElapsedEventHandler(ManageContractExpiredItemsNotification_Elapsed);
                logger.Info("Timer elapsed for Manage Contract Expired Items Notification");
            }

            logger.Info("Initialize Timer to Mark Applicant Documents Complete");
            if (MarkApplicantDocumentsCompleteServiceInterval != SysXDBConsts.MINUS_ONE)
            {
                logger.Info("entered for Initializing Timer to Mark Applicant Documents Complete");
                markApplicantDocumentsCompleteTimer = new Timer();
                markApplicantDocumentsCompleteTimer.Interval = (firstTimerInterval(MarkApplicantDocumentsCompleteStartTime, MarkApplicantDocumentsCompleteEndTime, CurrentTime, 19));
                markApplicantDocumentsCompleteTimer.Elapsed += new ElapsedEventHandler(MarkApplicantDocumentsComplete_Elapsed);
                logger.Info("Timer elapsed to Mark Applicant Documents Complete Notification");
            }

            logger.Info("Initialize Timer to Compliance Audit Data Syncronise job");
            if (ComplianceAuditDataSyncInterval != SysXDBConsts.MINUS_ONE)
            {
                logger.Info("entered for Initializing Timer to Compliance Audit Data Syncronise job");
                ComplianceAuditDataSyncTimer = new Timer();
                ComplianceAuditDataSyncTimer.Interval = (firstTimerInterval(ComplianceAuditDataSyncStartTime, ComplianceAuditDataSyncEndTime, CurrentTime, 20));
                ComplianceAuditDataSyncTimer.Elapsed += new ElapsedEventHandler(ComplianceAuditDataSync_Elapsed);
                logger.Info("Timer elapsed to Compliance Audit Data Syncronise job");
            }

            logger.Info("Initialize Timer to ReconcillationQueueSync job");
            if (ReconcillationQueueSyncInterval != SysXDBConsts.MINUS_ONE)
            {
                logger.Info("entered for Initializing Timer to ReconcillationQueueSync job");
                ReconcillationQueueSyncTimer = new Timer();
                ReconcillationQueueSyncTimer.Interval = (firstTimerInterval(ReconcillationQueueSyncStartTime, ReconcillationQueueSyncEndTime, CurrentTime, 21));
                ReconcillationQueueSyncTimer.Elapsed += new ElapsedEventHandler(ReconcillationQueueSyncTimer_Elapsed);
                logger.Info("Timer elapsed to ReconcillationQueueSync job");
            }

            logger.Info("Initialize Timer to ReqCategoryComplianceRqd job");
            if (ReqCategoryComplianceRqdServiceInterval != SysXDBConsts.MINUS_ONE)
            {
                logger.Info("entered for Initializing Timer for Requirement Category Compliance Rqd");
                reqCategoryComplianceRqdTimer = new Timer();

                //Get the Compliance Service Interval for first time
                reqCategoryComplianceRqdTimer.Interval = (firstTimerInterval(ReqCategoryComplianceRqdStartTime, ReqCategoryComplianceRqdEndTime, CurrentTime, 22));

                reqCategoryComplianceRqdTimer.Elapsed += new ElapsedEventHandler(ReqCategoryComplianceRqd_Elapsed);
                logger.Info("Timer elapsed for Requirement Category Compliance Rqd");
            }

            logger.Info("Initialize Timer to Copy Compliance Data To Requirement");
            if (CopyComplianceDataToRequirementInterval != SysXDBConsts.MINUS_ONE)
            {
                logger.Info("entered for Initializing Timer for Copy Compliance Data To Requirement");
                copyComplianceDataToRequirementTimer = new Timer();

                //Get the Compliance Service Interval for first time
                copyComplianceDataToRequirementTimer.Interval = (firstTimerInterval(CopyComplianceDataToRequirementStartTime, CopyComplianceDataToRequirementEndTime, CurrentTime, 23));

                copyComplianceDataToRequirementTimer.Elapsed += new ElapsedEventHandler(CopyComplianceDataToRequirement_Elapsed);
                logger.Info("Timer elapsed for Copy Compliance Data To Requirement");
            }

            //UAT-2414, Create a snapshot on Rotation End Date 
            logger.Info("Initialize Timer to Create Requirement Snapshot On Rotation End");
            if (CreateRequirementSnapshotOnRotationEndInterval != SysXDBConsts.MINUS_ONE)
            {
                logger.Info("entered for Initializing Timer for Create Requirement Snapshot On Rotation End");
                createRequirementSnapshotOnRotationEndTimer = new Timer();

                //Get the Compliance Service Interval for first time
                createRequirementSnapshotOnRotationEndTimer.Interval = (firstTimerInterval(CreateRequirementSnapshotOnRotationEndStartTime, CreateRequirementSnapshotOnRotationEndEndTime, CurrentTime, 24));

                createRequirementSnapshotOnRotationEndTimer.Elapsed += new ElapsedEventHandler(CreateRequirementSnapshotOnRotationEnd_Elapsed);
                logger.Info("Timer elapsed for Create Requirement Snapshot On Rotation End");
            }

            //UAT-2495
            logger.Info("Initialize Timer to Client Data Upload Service job");
            if (ClientDataUploadInterval != SysXDBConsts.MINUS_ONE)
            {
                logger.Info("entered for Initializing Timer to  Client Data Upload Service job");
                ClientDataUploadTimer = new Timer();
                ClientDataUploadTimer.Interval = (firstTimerInterval(ClientDataUploadStartTime, ClientDataUploadEndTime, CurrentTime, 25));
                ClientDataUploadTimer.Elapsed += new ElapsedEventHandler(ClientDataUpload_Elapsed);
                logger.Info("Timer elapsed to  Client Data Upload Service job");
            }
            //UAT:2514
            logger.Info("Initialize Timer to Requirement Pkg Sync");
            if (RequirementPkgSyncInterval != SysXDBConsts.MINUS_ONE)
            {
                logger.Info("entered for Initialize Timer to Requirement Pkg Sync");
                requirementPkgSyncTimer = new Timer();

                //Get the Compliance Service Interval for first time
                requirementPkgSyncTimer.Interval = (firstTimerInterval(RequirementPkgSyncStartTime, RequirementPkgSyncEndTime, CurrentTime, 26));

                requirementPkgSyncTimer.Elapsed += new ElapsedEventHandler(RequirementPkgSync_Elapsed);
                logger.Info("Timer elapsed for Requirement Pkg Sync");
            }

            //UAT:2533
            logger.Info("Initialize Timer for Auto Archival");
            if (RequirementPkgAutoArchiveInterval != SysXDBConsts.MINUS_ONE)
            {
                logger.Info("Entered to Initialize Timer for Auto Archival");
                requirementPkgAutoArchiveTimer = new Timer();

                requirementPkgAutoArchiveTimer.Interval = (firstTimerInterval(RequirementPkgAutoArchiveStartTime, RequirementPkgAutoArchiveEndTime, CurrentTime, 27));

                requirementPkgAutoArchiveTimer.Elapsed += new ElapsedEventHandler(RequirementPkgAutoArchive_Elapsed);
                logger.Info("Timer elapsed for Auto Archival");
            }

            //UAt-2603
            logger.Info("Initialize Timer to Rotation Data Movement");
            if (RotationDataMovementInterval != SysXDBConsts.MINUS_ONE)
            {
                logger.Info("entered for Initialize Timer to Rotation Data Movement");
                rotDataMovementTimer = new Timer();


                rotDataMovementTimer.Interval = (firstTimerInterval(RotationDataMovementStartTime, RotationDataMovementEndTime, CurrentTime, 28));

                rotDataMovementTimer.Elapsed += new ElapsedEventHandler(RotationDataMovement_Elapsed);
                logger.Info("Timer elapsed for Rotation Data Movement");
            }

            //UAT-2628:
            logger.Info("Initialize Timer to Conversion Merging for Failed documents");
            if (ConversionMergingFailedDocumentServiceInterval != SysXDBConsts.MINUS_ONE)
            {
                logger.Info("entered for Initializing Timer to Conversion Merging for Failed documents");
                ConversionMergingForFailedDocumentTimer = new Timer();
                ConversionMergingForFailedDocumentTimer.Interval = (firstTimerInterval(ConversionAndMergingFailedDocumentStartTime, ConversionAndMergingFailedDocumentEndTime, CurrentTime, 29));
                ConversionMergingForFailedDocumentTimer.Elapsed += new ElapsedEventHandler(ConversionAndMergingForFailedApplicantDocument_Elapsed);
                logger.Info("Timer elapsed to Conversion Merging for Failed documents Notification");
            }

            //UAT-2388: AutomaticPackageInvitation
            logger.Info("Initialize Timer to Automatic Package Invitation");
            if (AutomaticPackageInvitationInterval != SysXDBConsts.MINUS_ONE)
            {
                logger.Info("entered for Initializing Timer to AutomaticPackageInvitation");
                AutomaticPackageInvitationTimer = new Timer();
                AutomaticPackageInvitationTimer.Interval = (firstTimerInterval(AutomaticPackageInvitationStartTime, AutomaticPackageInvitationEndTime, CurrentTime, 30));
                AutomaticPackageInvitationTimer.Elapsed += new ElapsedEventHandler(AutomaticPackageInvitation_Elapsed);
                logger.Info("Timer elapsed to AutomaticPackageInvitation Notification");
            }
            //UAT-2513 Feature for Client Admin to batch upload rotation creation
            logger.Info("Initialize Timer to BatchRotationUpload");
            if (BatchRotationUploadInterval != SysXDBConsts.MINUS_ONE)
            {
                logger.Info("entered for Initializing Timer to BatchRotationUpload");
                BatchRotationUploadTimer = new Timer();
                BatchRotationUploadTimer.Interval = (firstTimerInterval(BatchRotationUploadStartTime, BatchRotationUploadEndTime, CurrentTime, 31));
                BatchRotationUploadTimer.Elapsed += new ElapsedEventHandler(BatchRotationUpload_Elapsed);
                logger.Info("Timer elapsed to BatchRotationUpload");
            }

            //UAT-3059: Daily notification for agency users with student who have updated rotation items (by agency)  UpdatedApplicantRequirementsNotification
            logger.Info("Initialize Timer to Update Notification To Agency User after rotation item update by applicant");
            if (UpdatedApplicantRequirementsNotificationInterval != SysXDBConsts.MINUS_ONE)
            {
                logger.Info("entered for Initializing Timer to Update Notification To Agency User after rotation item update by applicant");
                UpdatedApplicantRequirementsNotificationTimer = new Timer();
                UpdatedApplicantRequirementsNotificationTimer.Interval = (firstTimerInterval(UpdatedApplicantRequirementsNotificationStartTime, UpdatedApplicantRequirementsNotificationEndTime, CurrentTime, 32));
                UpdatedApplicantRequirementsNotificationTimer.Elapsed += new ElapsedEventHandler(UpdatedApplicantRequirementsNotification_Elapsed);
                logger.Info("Timer elapsed to Update Notification To Agency User after rotation item update by applicant");
            }

            //UAT-3112
            logger.Info("Initialize Timer to Badge Form Notification");
            if (BadgeFormNotificationInterval != SysXDBConsts.MINUS_ONE)
            {
                logger.Info("entered for Initialize Timer to Badge Form Notification");
                badgeFrmNotifTimer = new Timer();
                badgeFrmNotifTimer.Interval = (firstTimerInterval(BadgeFormNotificationStartTime, BadgeFormNotificationEndTime, CurrentTime, 33));
                badgeFrmNotifTimer.Elapsed += new ElapsedEventHandler(BadgeFormNotification_Elapsed);
                logger.Info("Timer elapsed for Badge Form Notification");
            }


            //UAT-2960
            logger.Info("Initialize Timer to Alumni Access Notification");
            if (AlumniAccessNotificationInterval != SysXDBConsts.MINUS_ONE)
            {
                logger.Info("entered for Initialize Timer to Alumni Access Notification");
                alumniAccessNotifTimer = new Timer();
                alumniAccessNotifTimer.Interval = (firstTimerInterval(AlumniAccessNotificationStartTime, AlumniAccessNotificationEndTime, CurrentTime, 34));
                alumniAccessNotifTimer.Elapsed += new ElapsedEventHandler(AlumniAccessNotification_Elapsed);
                logger.Info("Timer elapsed for Alumni Access Notification");
            }

            //UAT-2960-Copy Compliance to Compliance
            logger.Info("Initialize Timer to Copy Compliance to Compliance");
            if (CopyComplianceToComplianceInterval != SysXDBConsts.MINUS_ONE)
            {
                logger.Info("entered for Initialize Timer to Copy Compliance to Compliance");
                copyComplianceToComplianceTimer = new Timer();

                copyComplianceToComplianceTimer.Interval = (firstTimerInterval(CopyComplianceToComplianceStartTime, CopyComplianceToComplianceEndTime, CurrentTime, 35));

                copyComplianceToComplianceTimer.Elapsed += new ElapsedEventHandler(CopyComplianceToCompliance_Elapsed);
                logger.Info("Timer elapsed for Copy Compliance To Compliance");
            }
            logger.Info("Initialize Timer to Bkg Copy Package Data");
            if (BkgCopyPackageDataIntervalInterval != SysXDBConsts.MINUS_ONE)
            {
                logger.Info("entered for Initialize Timer to Bkg Copy Package Data");
                bkgCopyPackageDataTimer = new Timer();

                bkgCopyPackageDataTimer.Interval = (firstTimerInterval(BkgCopyPackageDataStartTime, BkgCopyPackageDataEndTime, CurrentTime, 36));

                bkgCopyPackageDataTimer.Elapsed += new ElapsedEventHandler(BkgCopyPackageDatae_Elapsed);
                logger.Info("Timer elapsed for Bkg Copy Package Data");
            }
            //UAT-3485
            logger.Info("Initialize Timer to Send Mail before Requirement item expires.");
            if (SendRotationAbtToExpireEmailInterval != SysXDBConsts.MINUS_ONE)
            {
                logger.Info("entered for Initialize Timer to Send Mail before Requirement item expires.");
                sendRotationAbtToExpireTimer = new Timer();

                sendRotationAbtToExpireTimer.Interval = (firstTimerInterval(SendRotationAbtToExpireStartTime, SendRotationAbtToExpireEndTime, CurrentTime, 37));

                sendRotationAbtToExpireTimer.Elapsed += new ElapsedEventHandler(SendRotationAbtToExpire_Elapsed);
                logger.Info("Timer elapsed for Send Mail before Requirement item expires");
            }


            //UAT-3137
            logger.Info("Initialize Timer to Send Mail before Requirement Category going to be required.");
            if (SendRotationCategoryBeforeGoingToBeRequiredEmailInterval != SysXDBConsts.MINUS_ONE)
            {
                logger.Info("entered for Initialize Timer to Send Mail before Requirement Category going to be required");
                sendRequiredRotationCategoryBeforeGoingToBeRequiredTimer = new Timer();

                sendRequiredRotationCategoryBeforeGoingToBeRequiredTimer.Interval = (firstTimerInterval(SendRotationCategoryBeforeGoingToBeRequiredStartTime, SendRotationCategoryBeforeGoingToBeRequiredEndTime, CurrentTime, 38));

                sendRequiredRotationCategoryBeforeGoingToBeRequiredTimer.Elapsed += new ElapsedEventHandler(SendRequiredRotationCategoryBeforeGoingToBeRequired_Elapsed);
                logger.Info("Timer elapsed for Send Mail before Requirement Category going to be required");
            }


            //UAT-3545-CBI Background Digestion
            logger.Info("Initialize Timer to Bkg Digestion Procedure Call");
            if (BkgDigestionProcedureCallInterval != SysXDBConsts.MINUS_ONE)
            {
                logger.Info("entered for Initialize Timer to Bkg Digestion Procedure Call");
                bkgDigestionProcedureTimer = new Timer();

                bkgDigestionProcedureTimer.Interval = (firstTimerInterval(BkgDigestionProcedureStartTime, BkgDigestionProcedureEndTime, CurrentTime, 39));

                bkgDigestionProcedureTimer.Elapsed += new ElapsedEventHandler(bkgDigestionProcedure_Elapsed);
                logger.Info("Timer elapsed for Bkg Digestion Procedure Call");
            }

            //UAT-3669
            logger.Info("Initialize Timer to Alert Mail For WebCCF Error");
            if (AlertMailForWebCCFErrorInterval != SysXDBConsts.MINUS_ONE)
            {
                logger.Info("entered for Initialize Timer to Alert Mail For WebCCF Error");
                alertMailForWebCCFErrorTimer = new Timer();
                alertMailForWebCCFErrorTimer.Interval = (firstTimerInterval(AlertMailForWebCCFErrorStartTime, AlertMailForWebCCFErrorEndTime, CurrentTime, 40));
                alertMailForWebCCFErrorTimer.Elapsed += new ElapsedEventHandler(AlertMailForWebCCFError_Elapsed);
                logger.Info("Timer elapsed for Badge Form Notification");
            }
            //UAT-3734            
            logger.Info("Initialize Timer to Send Mail for Off Time Revoked Appointment");
            if (OffTimeRevokedAppointmentEmailInterval != SysXDBConsts.MINUS_ONE)
            {
                logger.Info("entered for Initialize Timer to Send Mail for Off Time Revoked Appointments.");
                OffTimeRevokedAppointmentEmailTimer = new Timer();

                OffTimeRevokedAppointmentEmailTimer.Interval = (firstTimerInterval(OffTimeRevokedAppointmentEmailStartTime, OffTimeRevokedAppointmentEmailEndTime, CurrentTime, 41));

                OffTimeRevokedAppointmentEmailTimer.Elapsed += new ElapsedEventHandler(OffTimeRevokedAppointmentEmail_Elapsed);
                logger.Info("Timer elapsed for Send Mail for Off Time Revoked Appointments");
            }

            #region FingerPrint Missed Appointment Update status and send mail
            logger.Info("Initialize Timer to Send Mail for Missed Appointment");
            if (MissedAppointmentEmailInterval != SysXDBConsts.MINUS_ONE)
            {
                logger.Info("entered for Initialize Timer to Send Mail for missed Appointments.");
                MissedAppointmentEmailTimer = new Timer();
                MissedAppointmentEmailTimer.Interval = (firstTimerInterval(MissedAppointmentEmailStartTime, MissedAppointmentEmailEndTime, CurrentTime, 42));
                MissedAppointmentEmailTimer.Elapsed += new ElapsedEventHandler(MissedAppointmentEmail_Elapsed);
                logger.Info("Timer elapsed for Send Mail for Missed Appointments");
            }
            logger.Info("Initialize Timer to update appointment status to Missed");
            if (MissedAppointmentInterval != SysXDBConsts.MINUS_ONE)
            {
                logger.Info("entered for Initialize Timer to update appointment status to Missed.");
                MissedAppointmentTimer = new Timer();
                MissedAppointmentTimer.Interval = (firstTimerInterval(MissedAppointmentStartTime, MissedAppointmentEndTime, CurrentTime, 43));
                MissedAppointmentTimer.Elapsed += new ElapsedEventHandler(MissedAppointment_Elapsed);
                logger.Info("Timer elapsed for update appointment status to Missed.");
            }
            #endregion

            //UAT-3950
            logger.Info("Initialize Timer to Automatically Archive Rotation Service");
            if (ArchiveRotationDataInterval != SysXDBConsts.MINUS_ONE)
            {
                logger.Info("entered for Initializing Timer to Automatically Archive Rotation Service");
                ArchiveRotationDataTimer = new Timer();
                ArchiveRotationDataTimer.Interval = (firstTimerInterval(ArchiveRotationDataStartTime, ArchiveRotationDataEndTime, CurrentTime, 44));
                ArchiveRotationDataTimer.Elapsed += new ElapsedEventHandler(ArchiveRotationdata_Elapsed);
                logger.Info("Timer elapsed to Automatically Archive Rotation Service");
            }


            //UAT-3820
            logger.Info("Initialize Timer to Receive From student Service Form Status Procedure Call");
            if (ReceivedFromStudentServiceFormStatusInterval != SysXDBConsts.MINUS_ONE)
            {
                logger.Info("entered for Initialize Timer to Receive From student Service Form Status Procedure Call");
                receivedFromStuServFormStatusDataTimer = new Timer();
                receivedFromStuServFormStatusDataTimer.Interval = (firstTimerInterval(ReceivedFromStuServFormStatusDataStartTime, ReceivedFromStuServFormStatuDataEndTime, CurrentTime, 45));
                receivedFromStuServFormStatusDataTimer.Elapsed += new ElapsedEventHandler(ReceivedFromStuServFormStatus_Elapsed);
                logger.Info("Timer elapsed for Receive From student Service Form Status Procedure Call");
            }

            //Complio TalkDesk Integration - Create Report API Call
            logger.Info("Initialize Timer to start the Complio TalkDesk Integration - Create Report API Call.");
            if (ComplioTalkDeskCreateReportJobInterval != SysXDBConsts.MINUS_ONE)
            {
                logger.Info("entered for Initialize Timer for Complio TalkDesk Integration - Create Report API Call.");
                complioTalkDeskCreateReportJobTimer = new Timer();
                complioTalkDeskCreateReportJobTimer.Interval = (firstTimerInterval(ComplioTalkDeskCreateReportJobStartTime, ComplioTalkDeskCreateReportJobEndTime, CurrentTime, 46));
                complioTalkDeskCreateReportJobTimer.Elapsed += new ElapsedEventHandler(ComplioTalkDeskCreateReportJob_Elapsed);
                logger.Info("Timer elapsed for for Complio TalkDesk Integration - Create Report API Call.");
            }

            //Complio TalkDesk Integration - Create Report API Call
            logger.Info("Initialize Timer to start the Complio TalkDesk Integration - Update Report API Call.");
            if (ComplioTalkDeskUpdateReportJobInterval != SysXDBConsts.MINUS_ONE)
            {
                logger.Info("entered for Initialize Timer for Complio TalkDesk Integration - Update Report API Call.");
                complioTalkDeskUpdateReportJobTimer = new Timer();
                complioTalkDeskUpdateReportJobTimer.Interval = (firstTimerInterval(ComplioTalkDeskUpdateReportJobStartTime, ComplioTalkDeskUpdateReportJobEndTime, CurrentTime, 47));
                complioTalkDeskUpdateReportJobTimer.Elapsed += new ElapsedEventHandler(ComplioTalkDeskUpdateReportJob_Elapsed);
                logger.Info("Timer elapsed for for Complio TalkDesk Integration - Update Report API Call.");
            }

            //Complio TalkDesk Integration - Pull Call data from Report Job API Call
            logger.Info("Initialize Timer to start the Complio TalkDesk Integration - Pull Call data from Report Job API Call.");
            if (ComplioTalkDeskPullCallDataJobInterval != SysXDBConsts.MINUS_ONE)
            {
                logger.Info("entered for Initialize Timer for Complio TalkDesk Integration - Pull Call data from Report Job API Call.");
                complioTalkDeskPullCallDataJobTimer = new Timer();
                complioTalkDeskPullCallDataJobTimer.Interval = (firstTimerInterval(ComplioTalkDeskPullCallDataJobStartTime, ComplioTalkDeskPullCallDataJobEndTime, CurrentTime, 48));
                complioTalkDeskPullCallDataJobTimer.Elapsed += new ElapsedEventHandler(ComplioTalkDeskPullCallDataJob_Elapsed);
                logger.Info("Timer elapsed for for Complio TalkDesk Integration - Pull Call data from Report Job API Call.");
            }

            //Notification To Admin For Draft Orders
            logger.Info("Initialize Timer for Notification To Admin For Draft Orders.");
            if (SendDarftOrderNotificationtoAdminInterval != SysXDBConsts.MINUS_ONE)
            {
                logger.Info("entered for Initialize Timer to Send Mail Darft Order Notification to Admin.");
                SendDarftOrderNotificationtoAdminTimer = new Timer();

                SendDarftOrderNotificationtoAdminTimer.Interval = (firstTimerInterval(SendDarftOrderNotificationtoAdminStartTime, SendDarftOrderNotificationtoAdminEndTime, CurrentTime, 49));

                SendDarftOrderNotificationtoAdminTimer.Elapsed += new ElapsedEventHandler(SendDarftOrderNotificationtoAdmin_Elapsed);
                logger.Info("Timer elapsed for Send Mail Darft Order Notification to Admin.");
            }

            //Send Invitation Pending Order Notification to Applicant
            logger.Info("Initialize Timer for Send Invitation Pending Order Notification to Applicant.");
            if (SendInvitationPendingOrderNotificationtoApplicantInterval != SysXDBConsts.MINUS_ONE)
            {
                logger.Info("entered for Initialize Timer to Send Invitation Pending Order Notification to Applicant.");
                SendInvitationPendingOrderNotificationtoApplicantTimer = new Timer();

                SendInvitationPendingOrderNotificationtoApplicantTimer.Interval = (firstTimerInterval(SendInvitationPendingOrderNotificationtoApplicantStartTime, SendInvitationPendingOrderNotificationtoApplicantEndTime, CurrentTime, 50));

                SendInvitationPendingOrderNotificationtoApplicantTimer.Elapsed += new ElapsedEventHandler(SendInvitationPendingOrderNotificationtoApplicant_Elapsed);
                logger.Info("Timer elapsed for Send Invitation Pending Order Notification to Applicant.");
            }

            //Delete Draft Order Status
            logger.Info("Initialize Timer for Delete Draft Order Status.");
            if (DeleteDraftOrderStatusInterval != SysXDBConsts.MINUS_ONE)
            {
                logger.Info("entered for Initialize Timer to Delete Draft Order Status.");
                DeleteDraftOrderStatusTimer = new Timer();

                DeleteDraftOrderStatusTimer.Interval = (firstTimerInterval(DeleteDraftOrderStatusStartTime, DeleteDraftOrderStatusEndTime, CurrentTime, 51));

                DeleteDraftOrderStatusTimer.Elapsed += new ElapsedEventHandler(DeleteDraftOrderStatus_Elapsed);
                logger.Info("Timer elapsed for Delete Draft Order Status.");
            }

            //Change Order Status Complete To Archived
            logger.Info("Initialize Timer for Change Order Status Complete To Archived.");
            if (ChangeOrderStatusCompleteToArchivedInterval != SysXDBConsts.MINUS_ONE)
            {
                logger.Info("entered for Initialize Timer for Change Order Status Complete To Archived.");
                ChangeOrderStatusCompleteToArchivedTimer = new Timer();

                ChangeOrderStatusCompleteToArchivedTimer.Interval = (firstTimerInterval(ChangeOrderStatusCompleteToArchivedStartTime, ChangeOrderStatusCompleteToArchivedEndTime, CurrentTime, 52));

                ChangeOrderStatusCompleteToArchivedTimer.Elapsed += new ElapsedEventHandler(ChangeOrderStatusCompleteToArchived_Elapsed);
                logger.Info("Timer elapsed for Change Order Status Complete To Archived.");
            }

            //BKG Completed Status Order to Client Admin Notification
            logger.Info("Initialize Timer for BKG Completed Status Order to Client Admin Notification.");
            if (BKgCompletedOrderToClientAdminInterval != SysXDBConsts.MINUS_ONE)
            {
                logger.Info("entered for Initialize Timer for BKG Completed Status Order to Client Admin Notification.");
                BKgCompletedOrderToClientAdminTimer = new Timer();

                BKgCompletedOrderToClientAdminTimer.Interval = (firstTimerInterval(BKgCompletedOrderToClientAdminStartTime, BKgCompletedOrderToClientAdminEndTime, CurrentTime, 53));

                BKgCompletedOrderToClientAdminTimer.Elapsed += new ElapsedEventHandler(BKgCompletedOrderToClientAdmin_Elapsed);
                logger.Info("Timer elapsed for BKG Completed Status Order to Client Admin Notification.");
            }

            //BKG Completed Status Order with Attached PDF to Client Admin Notification
            logger.Info("Initialize Timer for BKG Completed Status Order with Attached PDF to Client Admin Notification.");
            if (BKgCompletedOrderWithAttachedPDFToClientAdminInterval != SysXDBConsts.MINUS_ONE)
            {
                logger.Info("entered for Initialize Timer for BKG Completed Status Order with Attached PDF to Client Admin Notification.");
                BKgCompletedOrderWithAttachedPDFToClientAdminTimer = new Timer();

                BKgCompletedOrderWithAttachedPDFToClientAdminTimer.Interval = (firstTimerInterval(BKgCompletedOrderWithAttachedPDFToClientAdminStartTime, BKgCompletedOrderWithAttachedPDFToClientAdminEndTime, CurrentTime, 54));

                BKgCompletedOrderWithAttachedPDFToClientAdminTimer.Elapsed += new ElapsedEventHandler(BKgCompletedOrderWithAttachedPDFToClientAdmin_Elapsed);
                logger.Info("Timer elapsed for BKG Completed Status Order with Attached PDF to Client Admin Notification.");
            }

            //UAT-4613
            logger.Info("Initialize Timer to InProcessAgency From student Service Form Status Procedure Call");
            if (NotificationForServiceFormInIPAStatusFromStudentInterval != SysXDBConsts.MINUS_ONE)
            {
                logger.Info("entered for Initialize Timer to InProcessAgency From student Service Form Status Procedure Call");
                notificationForServiceFormInIPAStatusFromStudentTimer = new Timer();
                notificationForServiceFormInIPAStatusFromStudentTimer.Interval = (firstTimerInterval(NotificationForServiceFormInIPAStatusFromStudentStartTime, NotificationForServiceFormInIPAStatusFromStudentEndTime, CurrentTime, 55));
                notificationForServiceFormInIPAStatusFromStudentTimer.Elapsed += new ElapsedEventHandler(NotificationForServiceFormInIPAStatusFromStudent_Elapsed);
                logger.Info("Timer elapsed for InProcessAgency From student Service Form Status Procedure Call");
            }
        }
        private Double firstTimerInterval(TimeSpan startTime, TimeSpan EndTime, TimeSpan currentTime, Int32 multiplier)
        {
            double firstInterval = 0;
            if (currentTime >= startTime && currentTime <= EndTime)
            {
                firstInterval = 1000 * multiplier;
            }
            else
            {
                firstInterval = (startTime - currentTime).TotalMilliseconds;
                if (firstInterval < 0)
                {
                    firstInterval = NextTimeSpanSeconds + firstInterval;
                }
            }
            return firstInterval;
        }

        void scheduleTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            logger.Info("Timer invoked for dispatching emails");

            lock (this.lockObject)
            {
                if (isExecuting)
                    return;
                isExecuting = true;
            }

            logger.Info("Entered processing for dispatching emails");

            try
            {
                GetSystemCommunicationDelivery();
            }
            catch (Exception ex)
            {
                isExecuting = false;
                logger.Error("An Error has occured in Email Dispatcher, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                isExecuting = false;
            }
        }

        public void GetSystemCommunicationDelivery()
        {
            try
            {
                //String errormessage = String.Empty;
                //Email email = null;
                DateTime currentTime = DateTime.Now;

                Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                Int32 systemUserId = AppConsts.BACKGROUND_PROCESS_USER_VALUE;
                if (appConfiguration.IsNotNull())
                {
                    systemUserId = Convert.ToInt32(appConfiguration.AC_Value);
                }
                DateTime jobStartTime = DateTime.Now;
                DateTime jobEndTime;
                List<lkpDocumentAttachmentType> docAttachmentType = CommunicationManager.GetDocumentAttachmentType();
                String docAttachmentTypeCode = DocumentAttachmentType.ORDER_COMPLETION_DOCUMENT.GetStringValue();
                Int16 docAttachmentTypeID = docAttachmentType.IsNotNull() && docAttachmentType.Count > 0 ?
                    Convert.ToInt16(docAttachmentType.FirstOrDefault(cond => cond.DAT_Code == docAttachmentTypeCode).DAT_ID) : Convert.ToInt16(0);

                #region UAT-1578:WB: Addition of SMS notification
                List<lkpNotificationDeliveryType> notificationDeliveryType = CommunicationManager.GetNotificationDeliveryType();
                String notificationEmailTypeCode = NotificationDeliveryType.EMAIL.GetStringValue();
                Int32 notificationEmailTypeID = notificationDeliveryType.IsNotNull() && notificationDeliveryType.Count > 0 ?
                    Convert.ToInt16(notificationDeliveryType.FirstOrDefault(cond => cond.NDT_Code == notificationEmailTypeCode).NDT_ID) : 0;
                String notificationSMSTypeCode = NotificationDeliveryType.SMS.GetStringValue();
                Int32 notificationSMSTypeID = notificationDeliveryType.IsNotNull() && notificationDeliveryType.Count > 0 ?
                    Convert.ToInt16(notificationDeliveryType.FirstOrDefault(cond => cond.NDT_Code == notificationSMSTypeCode).NDT_ID) : 0;
                #endregion

                List<SystemCommunication> systemCommunications = CommunicationManager.GetSystemCommunications(currentTime, MaxRetryCount, EmailChunkSize).ToList();

                while (systemCommunications != null && systemCommunications.Count > 0)
                {
                    logger.Trace("******************* Dispatching Emails for a chunk: " + DateTime.Now.ToString() + " *******************");
                    Parallel.ForEach(systemCommunications,
                     new ParallelOptions { MaxDegreeOfParallelism = EmailMaxParallelThreadCount },
                    (item, loopState) =>
                    {
                        String errormessage = String.Empty;
                        Email email = null;
                        SystemCommunication sysComm = item;

                        // foreach (SystemCommunication sysComm in systemCommunications)
                        //{
                        List<SystemCommunicationDelivery> deliveries = sysComm.SystemCommunicationDeliveries.Where(x => !x.IsDispatched && x.NotificationDeliveryTypeID == notificationEmailTypeID).OrderBy(x => x.CreatedOn).ToList();
                        List<SystemCommunicationDelivery> deliveriesForSMS = sysComm.SystemCommunicationDeliveries.Where(x => !x.IsDispatched && x.NotificationDeliveryTypeID == notificationSMSTypeID).OrderBy(x => x.CreatedOn).ToList();

                        logger.Trace("******************* Sending Email:" + DateTime.Now.ToString() + " *******************");

                        //if (isEmailThresholdRequired == AppConsts.ONE)
                        //{
                        //    DateTime localvarMailTime = DateTime.Now;
                        //    if (emailCounter > emailThreshold && (localvarMailTime.Second - serviceStartTime.Second) <= EmailTimeLimit)
                        //    {
                        //        systemCommunications.Add(new SystemCommunication());
                        //    }
                        //}

                        if (deliveries != null && deliveries.Count > 0)
                        {
                            email = new Email();
                            email.ToAddresses = new List<MailAddress>();
                            email.CcAddresses = new List<MailAddress>();
                            email.BccAddresses = new List<MailAddress>();
                            email.From = new MailAddress(sysComm.SenderEmailID, sysComm.SenderName);
                            email.Body = sysComm.Content;
                            email.Subject = sysComm.Subject;


                            if (sysComm.SystemCommunicationAttachments.IsNotNull() && sysComm.SystemCommunicationAttachments.Count > AppConsts.NONE)
                            {
                                email.MailAttachments = new Dictionary<Int32, DocumentInfo>();
                                foreach (SystemCommunicationAttachment sysCommAttachment in sysComm.SystemCommunicationAttachments)
                                {
                                    DocumentInfo docInfo = new DocumentInfo();
                                    docInfo.DocumentPath = sysCommAttachment.SCA_DocumentPath;
                                    docInfo.DocumentName = sysCommAttachment.SCA_OriginalDocumentName;
                                    if (!sysCommAttachment.lkpDocumentAttachmentType.IsNullOrEmpty())
                                        docInfo.AttachmentTypeCode = sysCommAttachment.lkpDocumentAttachmentType.DAT_Code;
                                    email.MailAttachments.Add(sysCommAttachment.SCA_ID, docInfo);
                                }
                            }

                            foreach (SystemCommunicationDelivery delivery in deliveries)
                            {
                                if (delivery.IsCC == true)
                                    email.CcAddresses.Add(new MailAddress(delivery.RecieverEmailID, delivery.RecieverName));
                                else if (delivery.IsBCC == true)
                                    email.BccAddresses.Add(new MailAddress(delivery.RecieverEmailID, delivery.RecieverName));
                                else
                                {
                                    if (delivery.ReceiverOrganizationUserID != AppConsts.BACKGROUND_PROCESS_USER_VALUE)
                                        email.ToAddresses.Add(new MailAddress(delivery.RecieverEmailID, delivery.RecieverName));
                                }
                            }

                            if (SendMail(email, out errormessage))
                            {
                                CommunicationManager.SetDispatchedTrue(deliveries, systemUserId);
                                if (sysComm.SystemCommunicationAttachments.IsNotNull())
                                {
                                    foreach (SystemCommunicationAttachment sysCommAttachment in sysComm.SystemCommunicationAttachments)
                                    {
                                        if (sysCommAttachment.SCA_DocAttachmentTypeID == docAttachmentTypeID)
                                        {
                                            CommonFileManager.DeleteDocument(sysCommAttachment.SCA_DocumentPath, FileType.ApplicantFileLocation.GetStringValue());
                                        }
                                    }
                                }
                            }
                            else
                            {
                                CommunicationManager.SetRetryCountAndMessage(deliveries, systemUserId, errormessage);
                            }

                            logger.Trace("******************* Email sent:" + DateTime.Now.ToString() + " *******************");
                        }

                        //else
                        //{
                        //    if (deliveries != null && isEmailThresholdRequired == AppConsts.ONE)
                        //    {
                        //        string connectString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                        //        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectString);
                        //        // Retrieve the DataSource property.    
                        //        string SqlServerName = builder.DataSource;
                        //        if (!ToAddressesformailcountexceeded.IsNullOrEmpty())
                        //        {
                        //            ToAddressesformailcountexceededList = ToAddressesformailcountexceeded.Split(',').ToList();
                        //        }
                        //        email = new Email();
                        //        foreach (var toAddress in ToAddressesformailcountexceededList)
                        //        {
                        //            email.ToAddresses.Add(new MailAddress(toAddress));
                        //        }
                        //        email.CcAddresses = new List<MailAddress>();
                        //        email.BccAddresses = new List<MailAddress>();
                        //        email.From = new MailAddress("non-reply@americandatabank.com", "ADB");
                        //        email.Body = @"<p>Hi,</p>
                        //                    <br />
                        //                        Number of emails have exceeded the threshold!<br />
                        //                    <br />
                        //                       AppServerName : " + System.Environment.MachineName
                        //                           + "<br /> SQL Server Name : " + SqlServerName

                        //                           ;


                        //        email.Subject = "Email Count Exceeded";
                        //        emailCounter = 0;
                        //        serviceStartTime = DateTime.Now;
                        //        loopState.Break();
                        //        // break;
                        //    }
                        //}


                        //UAT-1578:WB: Addition of SMS notification
                        logger.Trace("******************* Sending SMS:" + DateTime.Now.ToString() + " *******************");

                        foreach (SystemCommunicationDelivery deliverySMS in deliveriesForSMS)
                        {
                            CommunicationManager.SendSMSToUser(deliverySMS, sysComm.Content, systemUserId);
                        }

                        logger.Trace("******************* SMS sent:" + DateTime.Now.ToString() + " *******************");
                        //}
                    });
                    logger.Trace("******************* Dispatched Emails for a chunk:" + DateTime.Now.ToString() + " *******************");
                    ServiceContext.ReleaseDBContextItems();
                    System.Threading.Thread.Sleep(SleepTime);

                    systemCommunications = CommunicationManager.GetSystemCommunications(currentTime, MaxRetryCount, EmailChunkSize).ToList();
                }
                //Save service logging data to DB
                if (_isServiceLoggingEnabled)
                {
                    jobEndTime = DateTime.Now;
                    ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                    serviceLoggingContract.ServiceName = INTSOF.Utils.ServiceName.EmailDispatcherService.GetStringValue();
                    serviceLoggingContract.JobName = JobName.SystemCommunicationDelivery.GetStringValue();
                    //serviceLoggingContract.TenantID = tenantId;
                    serviceLoggingContract.JobStartTime = jobStartTime;
                    serviceLoggingContract.JobEndTime = jobEndTime;
                    //serviceLoggingContract.Comments = "";
                    serviceLoggingContract.IsDeleted = false;
                    serviceLoggingContract.CreatedBy = systemUserId;
                    serviceLoggingContract.CreatedOn = DateTime.Now;
                    SecurityManager.SaveServiceLoggingDetail(serviceLoggingContract);
                }
            }
            catch (AggregateException ae)
            {
                foreach (Exception e in ae.InnerExceptions)
                    logger.Error("An Error has occured in GetSystemCommunicationDelivery, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", e.Message, e.InnerException, e.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
           catch (Exception ex)
           {
               logger.Error("An Error has occured in GetSystemCommunicationDelivery, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
           }
        }

        void notificationTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!isReminderInterval)
            {
                scheduleNotificationTimer.Interval = Convert.ToDouble(ReminderServiceInterval);
                isReminderInterval = true;
            }

            TimeSpan CurrentTime = DateTime.Now.TimeOfDay;

            logger.Info("Timer invoked for reminder emails");
            lock (this.lockNotificationObject)
            {
                if (isNotificationExecuting)
                    return;
                isNotificationExecuting = true;
            }
            logger.Info("Entered processing for reminder emails");
            try
            {
                if (CurrentTime >= ReminderStartTime && CurrentTime <= ReminderEndTime)
                {
                    logger.Trace("******************* Started placing email in Queue and make an entry in Notification Delivery corresponding to the Email Queue: " + DateTime.Now.ToString() + " *******************");

                    NotificationService.SendMailBeforeExpiry();
                    NotificationService.SendMailAfterExpiry();
                    NotificationService.SendMailForPendingPackage();

                    logger.Trace("******************* Ended placing email in Queue:" + DateTime.Now.ToString() + " *******************");
                }
                else
                {
                    logger.Trace("******************* Exit Notification Delivery:" + DateTime.Now.ToString() + " *******************");
                }
            }
            catch (Exception ex)
            {
                isNotificationExecuting = false;
                logger.Error("An Error has occured in Notification, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                isNotificationExecuting = false;
            }
        }

       public void complianceTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!isComplianceInterval)
            {
                complianceNotificationTimer.Interval = Convert.ToDouble(ComplianceServiceInterval);
                isComplianceInterval = true;
            }

            TimeSpan CurrentTime = DateTime.Now.TimeOfDay;

            logger.Info("Timer invoked for compliance item emails");
            lock (this.lockComplianceObject)
            {
                if (isComplianceExecuting)
                    return;
                isComplianceExecuting = true;
            }
            logger.Info("Entered processing for compliance item emails");
            try
            {
                if (CurrentTime >= ComplianceStartTime && CurrentTime <= ComplianceEndTime)
                {
                    logger.Trace("******************* Started placing email in Queue for compliance items: " + DateTime.Now.ToString() + " *******************");

                    ComplianceExpiry.ProcessItemExpiry();
                    ComplianceExpiry.SendMailForExpiringItems();
                    ComplianceExpiry.SendMailForExpiredItems();
                    ComplianceExpiry.ProcessCategoryExpiry();
                    ComplianceExpiry.SendMailForDispatchedServiceForm();
                    ComplianceExpiry.SendMailForComplianceExceptionExpiry();
                    logger.Trace("******************* Ended placing email in Queue:" + DateTime.Now.ToString() + " *******************");
                }
                else
                {
                    logger.Trace("******************* Exit Compliance Item:" + DateTime.Now.ToString() + " *******************");
                }
            }
            catch (Exception ex)
            {
                isComplianceExecuting = false;
                logger.Error("An Error has occured in Compliance item notification, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                isComplianceExecuting = false;

            }
        }

        void actionTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!isScheduleActionInterval)
            {
                scheduleActionTimer.Interval = Convert.ToDouble(ScheduleActionServiceInterval);
                isScheduleActionInterval = true;
            }

            TimeSpan CurrentTime = DateTime.Now.TimeOfDay;

            logger.Info("Timer invoked for action emails");
            lock (this.lockActionObject)
            {
                if (isActionExecuting)
                    return;
                isActionExecuting = true;
            }
            logger.Info("Entered processing for action emails");
            try
            {
                if (CurrentTime >= ScheduleActionStartTime && CurrentTime <= ScheduleActionEndTime)
                {
                    logger.Trace("******************* Started placing email in Queue for action items: " + DateTime.Now.ToString() + " *******************");

                    ScheduleAction.SendMailForScheduleActionExecuteCategoryrules();
                    ScheduleAction.ScheduleActionExecuteRulesOnObjectDeletion();
                    logger.Trace("******************* Ended placing email in Queue:" + DateTime.Now.ToString() + " *******************");

                    logger.Trace("******************* Started placing email in Queue for Non Compliance Categories: " + DateTime.Now.ToString() + " *******************");

                    ScheduleAction.SendMailForNonComplianceCategories();

                    logger.Trace("******************* Ended placing email in Queue for Non Compliance Categories:" + DateTime.Now.ToString() + " *******************");

                    //UAT-3080
                    logger.Trace("******************* Started Processing RequirementSchedule CategoryComplianceRules: " + DateTime.Now.ToString() + " *******************");
                    RequirementScheduleAction.ProcessRequirementScheduleCategoryComplianceRules();
                    logger.Trace("******************* Ended Processing RequirementSchedule CategoryComplianceRules: " + DateTime.Now.ToString() + " *******************");
                }
                else
                {
                    logger.Trace("******************* Exit action Item:" + DateTime.Now.ToString() + " *******************");
                }
            }
            catch (Exception ex)
            {
                isActionExecuting = false;
                logger.Error("An Error has occured in action notification, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                isActionExecuting = false;

            }
        }

        void mobilityTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!isMobilityInstanceInterval)
            {
                mobilityInstanceTimer.Interval = Convert.ToDouble(MobilityInstanceServiceInterval);
                isMobilityInstanceInterval = true;
            }

            TimeSpan CurrentTime = DateTime.Now.TimeOfDay;

            logger.Info("Timer invoked for Mobility Instance");
            lock (this.lockMobilityObject)
            {
                if (isMobilityExecuting)
                    return;
                isMobilityExecuting = true;
            }
            logger.Info("Entered processing for Mobility Instance");
            try
            {
                if (CurrentTime >= MobilityInstanceStartTime && CurrentTime <= MobilityInstanceEndTime)
                {
                    logger.Trace("******************* Started placing entry for Mobility Instance: " + DateTime.Now.ToString() + " *******************");

                    MobilityInstance.CreateMobilityInstance();

                    MobilityInstance.InsertNodeTransition();

                    MobilityInstance.AutomaticNodeTransitionMovement();

                    logger.Trace("******************* Ended placing entry for Mobility Instance:" + DateTime.Now.ToString() + " *******************");
                }
                else
                {
                    logger.Trace("******************* Exit Mobility Instance:" + DateTime.Now.ToString() + " *******************");
                }
            }
            catch (Exception ex)
            {
                isMobilityExecuting = false;
                logger.Error("An Error has occured in Mobility Instance, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                isMobilityExecuting = false;
            }
        }

        void copyPackageDataTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!isCopyPackageDataInterval)
            {
                copyPackageDataTimer.Interval = Convert.ToDouble(CopyPackageDataServiceInterval);
                isCopyPackageDataInterval = true;
            }

            TimeSpan CurrentTime = DateTime.Now.TimeOfDay;

            logger.Info("Timer invoked for Copy Package Data");
            lock (this.lockCopyPackageDataObject)
            {
                if (isCopyPackageDataExecuting)
                {
                    logger.Info("copy package data was already in progress. Thread id {0} will exit now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                    return;
                }
                logger.Info("Thread id {0} will execute data copy now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isCopyPackageDataExecuting = true;

            }
            logger.Info("Entered processing for Copy Package Data");
            try
            {
                if (CurrentTime >= CopyPackageDataStartTime && CurrentTime <= CopyPackageDataEndTime)
                {
                    logger.Trace("******************* Started placing entry for Copy Package Data: " + DateTime.Now.ToString() + " *******************");

                    ScheduleAction.CopyPackgeDataExecuteRule();

                    logger.Trace("******************* Ended placing entry for Copy Package Data:" + DateTime.Now.ToString() + " *******************");
                }
                else
                {
                    logger.Trace("******************* Exit Copy Package Data:" + DateTime.Now.ToString() + " *******************");
                }
            }
            catch (Exception ex)
            {
                logger.Info("Error in datacopy. Thread id {0} will exit data copy now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isCopyPackageDataExecuting = false;
                logger.Error("An Error has occured in Copy Package Data, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                logger.Info("Data copy complete. Thread id {0} will exit data copy now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isCopyPackageDataExecuting = false;
            }
        }

        void adminWidgetDataTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!isAdminWidgetDataInterval)
            {
                adminWidgetDataTimer.Interval = Convert.ToDouble(AdminWidgetServiceInterval);
                isAdminWidgetDataInterval = true;
            }

            TimeSpan CurrentTime = DateTime.Now.TimeOfDay;

            logger.Info("Timer invoked for Admin Widget Data");
            lock (this.lockAdminWidgetDataObject)
            {
                if (isAdminWidgetDataExecuting)
                {
                    return;
                }
                isAdminWidgetDataExecuting = true;
            }

            logger.Info("Entered processing for Admin Widget Data");
            try
            {
                if (CurrentTime >= AdminWidgetStartTime && CurrentTime <= AdminWidgetEndTime)
                {
                    logger.Trace("******************* Started populating entry for Admin Widget Data: " + DateTime.Now.ToString() + " *******************");

                    AdminWidgetService.PopulateAdminWidgetData();

                    logger.Trace("******************* Ended populating entry for Admin Widget Data:" + DateTime.Now.ToString() + " *******************");
                }
                else
                {
                    logger.Trace("******************* Exit Admin Widget Data:" + DateTime.Now.ToString() + " *******************");
                }
            }
            catch (Exception ex)
            {
                isAdminWidgetDataExecuting = false;
                logger.Error("An Error has occured in Admin Widget Data, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                isAdminWidgetDataExecuting = false;
            }
        }

        void systemServiceTriggerTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!isSystemServiceTriggerInterval)
            {
                systemServiceTriggerTimer.Interval = Convert.ToDouble(SystemServiceTriggerInterval);
                isSystemServiceTriggerInterval = true;
            }

            TimeSpan CurrentTime = DateTime.Now.TimeOfDay;

            logger.Info("Timer invoked for System Service Trigger");
            lock (this.lockSystemServiceTriggerObject)
            {
                if (isSystemServiceTriggerExecuting)
                {
                    return;
                }
                isSystemServiceTriggerExecuting = true;
            }

            logger.Info("Entered processing for System Service Trigger");
            try
            {
                if (CurrentTime >= SystemServiceTriggerStartTime && CurrentTime <= SystemServiceTriggerEndTime)
                {
                    logger.Trace("******************* Started System Service Trigger: " + DateTime.Now.ToString() + " *******************");

                    //Get BackgroundProcessUserId
                    Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                    Int32 backgroundProcessUserId = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.BACKGROUND_PROCESS_USER_VALUE;

                    List<Entity.SystemServiceTrigger> lstSystemServiceTrigger = SecurityManager.GetSystemServiceTriggers();

                    foreach (Entity.SystemServiceTrigger sysServiceTrigger in lstSystemServiceTrigger)
                    {
                        DateTime jobStartTime = DateTime.Now;
                        DateTime jobEndTime;
                        Int32 tenantId = sysServiceTrigger.SST_TenantID.HasValue ? sysServiceTrigger.SST_TenantID.Value : 0;
                        sysServiceTrigger.SST_IsActive = false;
                        String code = sysServiceTrigger.lkpSystemService.IsNotNull() ? sysServiceTrigger.lkpSystemService.SS_Code : String.Empty;
                        switch (code)
                        {
                            case LkpSystemService.EMAIL_SERVICE:
                                if (ServiceInterval != SysXDBConsts.MINUS_ONE && !isExecuting)
                                {
                                    try
                                    {
                                        lock (this.lockObject)
                                        {
                                            if (isExecuting)
                                            {
                                                break;
                                            }
                                            isExecuting = true;
                                        }

                                        //Call Email Service Method Explicitly
                                        GetSystemCommunicationDelivery();

                                        //Update System Service Trigger IsActive to false
                                        sysServiceTrigger.SST_ModifiedByID = backgroundProcessUserId;
                                        sysServiceTrigger.SST_ModifiedOn = DateTime.Now;
                                        SecurityManager.UpdateSystemServiceTrigger(sysServiceTrigger);
                                    }
                                    catch (Exception ex)
                                    {
                                        isExecuting = false;
                                        logger.Error("An Error has occured in Email Service trigger, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                                    }
                                    finally
                                    {
                                        ServiceContext.ReleaseContextItems();
                                        isExecuting = false;
                                    }
                                }
                                break;
                            case LkpSystemService.SUBSCRIPTION_EXPIRY_SERVICE:
                                if (ReminderServiceInterval != SysXDBConsts.MINUS_ONE && !isNotificationExecuting)
                                {
                                    try
                                    {
                                        lock (this.lockNotificationObject)
                                        {
                                            if (isNotificationExecuting)
                                            {
                                                break;
                                            }
                                            isNotificationExecuting = true;
                                        }

                                        //Call Subscription Expiry Service Method Explicitly
                                        NotificationService.SendMailBeforeExpiry(tenantId);
                                        NotificationService.SendMailAfterExpiry(tenantId);
                                        NotificationService.SendMailForPendingPackage(tenantId);

                                        //Update System Service Trigger IsActive to false
                                        sysServiceTrigger.SST_ModifiedByID = backgroundProcessUserId;
                                        sysServiceTrigger.SST_ModifiedOn = DateTime.Now;
                                        SecurityManager.UpdateSystemServiceTrigger(sysServiceTrigger);
                                    }
                                    catch (Exception ex)
                                    {
                                        isNotificationExecuting = false;
                                        logger.Error("An Error has occured in Subscription Expiry Service trigger, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                                    }
                                    finally
                                    {
                                        ServiceContext.ReleaseContextItems();
                                        isNotificationExecuting = false;
                                    }
                                }
                                break;
                            case LkpSystemService.COMPLIANCE_EXPIRY_SERVICE:
                                if (ComplianceServiceInterval != SysXDBConsts.MINUS_ONE && !isComplianceExecuting)
                                {
                                    try
                                    {
                                        lock (this.lockComplianceObject)
                                        {
                                            if (isComplianceExecuting)
                                            {
                                                break;
                                            }
                                            isComplianceExecuting = true;
                                        }

                                        //Call Compliance Service Method Explicitly
                                        ComplianceExpiry.ProcessItemExpiry(tenantId);
                                        ComplianceExpiry.SendMailForExpiringItems(tenantId);
                                        ComplianceExpiry.SendMailForExpiredItems(tenantId);

                                        //Update System Service Trigger IsActive to false
                                        sysServiceTrigger.SST_ModifiedByID = backgroundProcessUserId;
                                        sysServiceTrigger.SST_ModifiedOn = DateTime.Now;
                                        SecurityManager.UpdateSystemServiceTrigger(sysServiceTrigger);
                                    }
                                    catch (Exception ex)
                                    {
                                        isComplianceExecuting = false;
                                        logger.Error("An Error has occured in Compliance Service trigger, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                                    }
                                    finally
                                    {
                                        ServiceContext.ReleaseContextItems();
                                        isComplianceExecuting = false;
                                    }
                                }
                                break;
                            case LkpSystemService.REOCCUR_RULES_SERVICE:
                                if (ScheduleActionServiceInterval != SysXDBConsts.MINUS_ONE && !isActionExecuting)
                                {
                                    try
                                    {
                                        lock (this.lockActionObject)
                                        {
                                            if (isActionExecuting)
                                            {
                                                break;
                                            }
                                            isActionExecuting = true;
                                        }

                                        //Call Rule Service Method Explicitly
                                        ScheduleAction.SendMailForScheduleActionExecuteCategoryrules(tenantId);

                                        //Update System Service Trigger IsActive to false
                                        sysServiceTrigger.SST_ModifiedByID = backgroundProcessUserId;
                                        sysServiceTrigger.SST_ModifiedOn = DateTime.Now;
                                        SecurityManager.UpdateSystemServiceTrigger(sysServiceTrigger);
                                    }
                                    catch (Exception ex)
                                    {
                                        isActionExecuting = false;
                                        logger.Error("An Error has occured in Rule Service trigger, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                                    }
                                    finally
                                    {
                                        ServiceContext.ReleaseContextItems();
                                        isActionExecuting = false;
                                    }
                                }
                                break;
                            case LkpSystemService.MOBILITY_INSTANCE_SERVICE:
                                if (MobilityInstanceServiceInterval != SysXDBConsts.MINUS_ONE && !isMobilityExecuting)
                                {
                                    try
                                    {
                                        lock (this.lockMobilityObject)
                                        {
                                            if (isMobilityExecuting)
                                            {
                                                break;
                                            }
                                            isMobilityExecuting = true;
                                        }

                                        //Call Mobility Instance Service Method Explicitly
                                        MobilityInstance.CreateMobilityInstance();
                                        MobilityInstance.InsertNodeTransition();
                                        MobilityInstance.AutomaticNodeTransitionMovement();

                                        //Update System Service Trigger IsActive to false
                                        sysServiceTrigger.SST_ModifiedByID = backgroundProcessUserId;
                                        sysServiceTrigger.SST_ModifiedOn = DateTime.Now;
                                        SecurityManager.UpdateSystemServiceTrigger(sysServiceTrigger);
                                    }
                                    catch (Exception ex)
                                    {
                                        isMobilityExecuting = false;
                                        logger.Error("An Error has occured in Mobility Instance Service trigger, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                                    }
                                    finally
                                    {
                                        ServiceContext.ReleaseContextItems();
                                        isMobilityExecuting = false;
                                    }
                                }
                                break;
                            case LkpSystemService.COPY_PACKAGE_DATA_SERVICE:
                                if (CopyPackageDataServiceInterval != SysXDBConsts.MINUS_ONE && !isCopyPackageDataExecuting)
                                {
                                    try
                                    {
                                        lock (this.lockCopyPackageDataObject)
                                        {
                                            if (isCopyPackageDataExecuting)
                                            {
                                                break;
                                            }
                                            isCopyPackageDataExecuting = true;
                                        }

                                        //Call Copy Package Data Service Method Explicitly
                                        ScheduleAction.CopyPackgeDataExecuteRule();

                                        //Update System Service Trigger IsActive to false
                                        sysServiceTrigger.SST_ModifiedByID = backgroundProcessUserId;
                                        sysServiceTrigger.SST_ModifiedOn = DateTime.Now;
                                        SecurityManager.UpdateSystemServiceTrigger(sysServiceTrigger);
                                    }
                                    catch (Exception ex)
                                    {
                                        isCopyPackageDataExecuting = false;
                                        logger.Error("An Error has occured in Copy Package Data Service trigger, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                                    }
                                    finally
                                    {
                                        ServiceContext.ReleaseContextItems();
                                        isCopyPackageDataExecuting = false;
                                    }
                                }
                                break;
                            case LkpSystemService.DASHBOARD_SUMMARY_SERVICE:
                                if (AdminWidgetServiceInterval != SysXDBConsts.MINUS_ONE && !isAdminWidgetDataExecuting)
                                {
                                    try
                                    {
                                        lock (this.lockAdminWidgetDataObject)
                                        {
                                            if (isAdminWidgetDataExecuting)
                                            {
                                                break;
                                            }
                                            isAdminWidgetDataExecuting = true;
                                        }

                                        //Call Admin Widget Service Method Explicitly
                                        AdminWidgetService.PopulateAdminWidgetData();

                                        //Update System Service Trigger IsActive to false
                                        sysServiceTrigger.SST_ModifiedByID = backgroundProcessUserId;
                                        sysServiceTrigger.SST_ModifiedOn = DateTime.Now;
                                        SecurityManager.UpdateSystemServiceTrigger(sysServiceTrigger);
                                    }
                                    catch (Exception ex)
                                    {
                                        isAdminWidgetDataExecuting = false;
                                        logger.Error("An Error has occured in Admin Widget Service trigger, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                                    }
                                    finally
                                    {
                                        ServiceContext.ReleaseContextItems();
                                        isAdminWidgetDataExecuting = false;
                                    }
                                }
                                break;

                            case LkpSystemService.EXECUTE_MULTI_RULES_SERVICE:
                                if (ScheduleActionServiceInterval != SysXDBConsts.MINUS_ONE && !isActionExecuting)
                                {
                                    try
                                    {
                                        lock (this.lockActionObject)
                                        {
                                            if (isActionExecuting)
                                            {
                                                break;
                                            }
                                            isActionExecuting = true;
                                        }

                                        //Call Rule Service Method Explicitly
                                        ScheduleAction.ScheduleActionExecuteRulesOnObjectDeletion(tenantId);

                                        //Update System Service Trigger IsActive to false
                                        sysServiceTrigger.SST_ModifiedByID = backgroundProcessUserId;
                                        sysServiceTrigger.SST_ModifiedOn = DateTime.Now;
                                        SecurityManager.UpdateSystemServiceTrigger(sysServiceTrigger);
                                    }
                                    catch (Exception ex)
                                    {
                                        isActionExecuting = false;
                                        logger.Error("An Error has occured in multi Rule Service trigger, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                                    }
                                    finally
                                    {
                                        ServiceContext.ReleaseContextItems();
                                        isActionExecuting = false;
                                    }
                                }
                                break;

                            case LkpSystemService.EXECUTE_REQUIREMENT_RULES_SERVICE:
                                if (ReqScheduleActionServiceInterval != SysXDBConsts.MINUS_ONE && !isReqScheduleActionExecuting)
                                {
                                    try
                                    {
                                        lock (this.lockReqScheduleActionObject)
                                        {
                                            if (isReqScheduleActionExecuting)
                                            {
                                                break;
                                            }
                                            isReqScheduleActionExecuting = true;
                                        }

                                        //Call Rule Service Method Explicitly
                                        RequirementScheduleAction.ProcessRequirementScheduleActionExecuteCategoryRules(tenantId);
                                        RequirementScheduleAction.ProcessRequirementScheduleActionExecutePackageRules(tenantId);
                                        RequirementScheduleAction.ProcessRequirementScheduleActionAfterDataSync(tenantId);
                                        RequirementScheduleAction.ProcessRequirementScheduleCategoryComplianceRules(tenantId); //UAT 3080
                                        //Update System Service Trigger IsActive to false
                                        sysServiceTrigger.SST_ModifiedByID = backgroundProcessUserId;
                                        sysServiceTrigger.SST_ModifiedOn = DateTime.Now;
                                        SecurityManager.UpdateSystemServiceTrigger(sysServiceTrigger);
                                    }
                                    catch (Exception ex)
                                    {
                                        isReqScheduleActionExecuting = false;
                                        logger.Error("An Error has occured in requirement Rule Service trigger, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                                    }
                                    finally
                                    {
                                        ServiceContext.ReleaseContextItems();
                                        isReqScheduleActionExecuting = false;
                                    }
                                }
                                break;

                            //UAT-3224:-System queue to retry the data sync which was not completed	
                            case LkpSystemService.BACKGROUND_COPY_PACKAGE_DATA:
                                if (BkgCopyPackageDataIntervalInterval != SysXDBConsts.MINUS_ONE && !isBkgCopyPackageDataInterval)
                                {
                                    try
                                    {
                                        lock (this.lockbkgCopyPackageDataObject)
                                        {
                                            if (isbkgCopyPackageDataExecuting)
                                            {
                                                break;
                                            }
                                            isbkgCopyPackageDataExecuting = true;
                                        }

                                        //Call Background copy package data
                                        ScheduleAction.BackgroundCopyPackgeData();
                                        //Update System Service Trigger IsActive to false
                                        sysServiceTrigger.SST_ModifiedByID = backgroundProcessUserId;
                                        sysServiceTrigger.SST_ModifiedOn = DateTime.Now;
                                        SecurityManager.UpdateSystemServiceTrigger(sysServiceTrigger);
                                    }
                                    catch (Exception ex)
                                    {
                                        isbkgCopyPackageDataExecuting = false;
                                        logger.Error("An Error has occured in background copy package data service trigger, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                                    }
                                    finally
                                    {
                                        ServiceContext.ReleaseContextItems();
                                        isbkgCopyPackageDataExecuting = false;
                                    }
                                }
                                break;
                        }

                        //Save service logging data to DB
                        if (_isServiceLoggingEnabled)
                        {
                            jobEndTime = DateTime.Now;
                            ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                            serviceLoggingContract.ServiceName = INTSOF.Utils.ServiceName.EmailDispatcherService.GetStringValue();
                            serviceLoggingContract.JobName = JobName.SystemServiceTrigger.GetStringValue();
                            serviceLoggingContract.TenantID = tenantId;
                            serviceLoggingContract.JobStartTime = jobStartTime;
                            serviceLoggingContract.JobEndTime = jobEndTime;
                            //serviceLoggingContract.Comments = "";
                            serviceLoggingContract.IsDeleted = false;
                            serviceLoggingContract.CreatedBy = backgroundProcessUserId;
                            serviceLoggingContract.CreatedOn = DateTime.Now;
                            SecurityManager.SaveServiceLoggingDetail(serviceLoggingContract);
                        }

                        ServiceContext.ReleaseDBContextItems();
                    }

                    logger.Trace("******************* Ended System Service Trigger:" + DateTime.Now.ToString() + " *******************");
                }
                else
                {
                    logger.Trace("******************* Exit System Service Trigger:" + DateTime.Now.ToString() + " *******************");
                }
            }
            catch (Exception ex)
            {
                isSystemServiceTriggerExecuting = false;
                logger.Error("An Error has occured in System Service Triggers, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                isSystemServiceTriggerExecuting = false;
            }
        }

        void nagEmailTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!isNagEmailInterval)
            {
                nagEmailTimer.Interval = Convert.ToDouble(NagEmailInterval);
                isNagEmailInterval = true;
            }

            TimeSpan CurrentTime = DateTime.Now.TimeOfDay;

            logger.Info("Timer invoked for Nag Email");
            lock (this.lockNagEmailObject)
            {
                if (isNagEmailExecuting)
                {
                    return;
                }
                isNagEmailExecuting = true;
            }

            logger.Info("Entered processing for Nag Email");
            try
            {
                if (CurrentTime >= NagEmailStartTime && CurrentTime <= NagEmailEndTime)
                {
                    logger.Trace("******************* Started populating entry for Nag Email: " + DateTime.Now.ToString() + " *******************");

                    NotificationService.SendNagMails();

                    logger.Trace("******************* Ended populating entry for Nag Email:" + DateTime.Now.ToString() + " *******************");
                }
                else
                {
                    logger.Trace("******************* Exit Nag Email:" + DateTime.Now.ToString() + " *******************");
                }
            }
            catch (Exception ex)
            {
                isNagEmailExecuting = false;
                logger.Error("An Error has occured in Nag Email, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                isNagEmailExecuting = false;
            }
        }

        void QueueImagingTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!isQueueImagingInterval)
            {
                queueImagingTimer.Interval = Convert.ToDouble(QueueImagingInterval);
                isQueueImagingInterval = true;
            }

            TimeSpan CurrentTime = DateTime.Now.TimeOfDay;

            logger.Info("Timer invoked for queue imaging");
            lock (this.lockQueueImagingObject)
            {
                if (isQueueImagingExecuting)
                {
                    return;
                }
                isQueueImagingExecuting = true;
            }

            logger.Info("Entered processing for queue imaging");
            try
            {
                if (CurrentTime >= QueueImagingStartTime && CurrentTime <= QueueImagingEndTime)
                {
                    logger.Trace("******************* Started populating entry for queue imaging: " + DateTime.Now.ToString() + " *******************");

                    NotificationService.QueueImagingData();

                    //logic here

                    logger.Trace("******************* Ended populating entry for queue imaging:" + DateTime.Now.ToString() + " *******************");
                }
                else
                {
                    logger.Trace("******************* Exit queue imaging:" + DateTime.Now.ToString() + " *******************");
                }
            }
            catch (Exception ex)
            {
                isQueueImagingExecuting = false;
                logger.Error("An Error has occured in queue imaging, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                isQueueImagingExecuting = false;
            }
        }

        void deadlineEmailTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!isDeadlineEmailInterval)
            {
                deadlineEmailTimer.Interval = Convert.ToDouble(DeadlineEmailInterval);
                isDeadlineEmailInterval = true;
            }

            TimeSpan CurrentTime = DateTime.Now.TimeOfDay;

            logger.Info("Timer invoked for Deadline Email");
            lock (this.lockDeadlineEmailObject)
            {
                if (isDeadlineEmailExecuting)
                {
                    return;
                }
                isDeadlineEmailExecuting = true;
            }

            logger.Info("Entered processing for Deadline Email");
            try
            {
                if (CurrentTime >= DeadlineEmailStartTime && CurrentTime <= DeadlineEmailEndTime)
                {
                    logger.Trace("******************* Started populating entry for Deadline Email: " + DateTime.Now.ToString() + " *******************");

                    NotificationService.SendMailForDeadline();

                    logger.Trace("******************* Ended populating entry for Deadline Email:" + DateTime.Now.ToString() + " *******************");
                }
                else
                {
                    logger.Trace("******************* Exit Deadline Email:" + DateTime.Now.ToString() + " *******************");
                }
            }
            catch (Exception ex)
            {
                isDeadlineEmailExecuting = false;
                logger.Error("An Error has occured in Deadline Email, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                isDeadlineEmailExecuting = false;
            }
        }

        void scheduleTask_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!isScheduleTaskInterval)
            {
                scheduleTaskTimer.Interval = Convert.ToDouble(ScheduleTaskServiceInterval);
                isScheduleTaskInterval = true;
            }

            TimeSpan CurrentTime = DateTime.Now.TimeOfDay;

            logger.Info("Timer invoked for Schedule Task");
            lock (this.lockScheduleTaskObject)
            {
                if (isScheduleTaskExecuting)
                {
                    logger.Info("Schedule Task was already in progress. Thread id {0} will exit now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                    return;
                }
                logger.Info("Thread id {0} will execute Schedule Task now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isScheduleTaskExecuting = true;

            }
            logger.Info("Entered processing for Schedule Task");
            try
            {

                if (CurrentTime >= ScheduleTaskStartTime && CurrentTime <= ScheduleTaskEndTime)
                {
                    logger.Trace("******************* Started placing entry for Schedule Task for Approve Invoice Order: " + DateTime.Now.ToString() + " *******************");

                    ScheduleAction.ApproveInvoiceOrderExecuteRule();

                    logger.Trace("******************* Ended placing entry for Schedule Task for Approve Invoice Order:" + DateTime.Now.ToString() + " *******************");

                    logger.Trace("******************* Started placing entry for Schedule Task for Auto Renew Expired invoice: " + DateTime.Now.ToString() + " *******************");

                    // Process of Auto Renew Expired invoice Subscription
                    ScheduleAction.ScheduleTaskForAutoRenewExpiredinvoiceSubscription();

                    logger.Trace("******************* Ended placing entry for Schedule Task for Auto Renew Expired invoice:" + DateTime.Now.ToString() + " *******************");

                    logger.Trace("******************* Started placing entry for Schedule Task for AutoArchiveExpiredSubscriptions: " + DateTime.Now.ToString() + " *******************");

                    ScheduleAction.AutoArchiveExpiredSubscriptions();

                    logger.Trace("******************* Ended placing entry for Schedule Task for AutoArchiveExpiredSubscriptions:" + DateTime.Now.ToString() + " *******************");

                    logger.Trace("******************* Started placing entry for Schedule Task for Recurring Background Reports: " + DateTime.Now.ToString() + " *******************");

                    // Process of Sending Flagged Order Data report/Daily Svc Grp Completion report/Weekly incompleted EDS orders report
                    ScheduleAction.SendRecurringBackgroundReports();

                    logger.Trace("******************* Ended placing entry for Schedule Task for Recurring Background Reports:" + DateTime.Now.ToString() + " *******************");

                    logger.Trace("******************* Started placing entry for Sales Force Schedule Task : " + DateTime.Now.ToString() + " *******************");

                    // Process of Sending data to Sales Force Schedule Task 
                    ScheduleAction.SalesForceScheduleTask();

                    logger.Trace("******************* Ended placing entry for Sales Force Schedule Task :" + DateTime.Now.ToString() + " *******************");

                    logger.Trace("******************* Started placing entry for Schedule Task for Send Notification For Auto Archived Expired Subscriptions: " + DateTime.Now.ToString() + " *******************");

                    // Process of Send Notification For Auto Archived Expired Subscriptions
                    ScheduleAction.SendNotificationForAutoArchivedExpiredSubscriptions();

                    logger.Trace("******************* Ended placing entry for Schedule Task for Send Notification For Auto Archived Expired Subscriptions:" + DateTime.Now.ToString() + " *******************");

                    //UAT-1852 : If a Service Group is complete, but other service groups are not complete within an order, the system should send the applicant an email.
                    logger.Trace("******************* Started placing entry for Schedule Task for Send Mail For BKG Service Group Completion: " + DateTime.Now.ToString() + " *******************");

                    // Process of Send Email on completion of background service group.
                    ScheduleAction.SendMailForBkgSvcGrpCompletion();

                    logger.Trace("******************* Ended placing entry for Schedule Task for Send Mail For BKG Service Group Completion:" + DateTime.Now.ToString() + " *******************");

                    //UAT-3795
                    logger.Trace("******************* Started placing entry for Schedule Task for Send Mail Non Compliant Students Report Weekly: " + DateTime.Now.ToString() + " *******************");
                    ScheduleAction.SendNonCompliantStudentsReportWeekly();
                    logger.Trace("******************* Ended placing entry for Schedule Task for Send Mail Non Compliant Students Report Weekly:" + DateTime.Now.ToString() + " *******************");

                }
                else
                {
                    logger.Trace("******************* Exit Schedule Task" + DateTime.Now.ToString() + " *******************");
                }
            }
            catch (Exception ex)
            {
                logger.Info("Error in Schedule Task. Thread id {0} will exit Schedule Task.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isScheduleTaskExecuting = false;
                logger.Error("An Error has occured in Schedule Task, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                logger.Info("Schedule Task complete. Thread id {0} will exit Schedule Task now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isScheduleTaskExecuting = false;
            }
        }

        void categoryComplianceRqd_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!isCategoryComplianceRqdInterval)
            {
                categoryComplianceRqdTimer.Interval = Convert.ToDouble(CategoryComplianceRqdServiceInterval);
                isCategoryComplianceRqdInterval = true;
            }

            TimeSpan CurrentTime = DateTime.Now.TimeOfDay;

            logger.Info("Timer invoked for Category Compliance Rqd");
            lock (this.lockCategoryComplianceRqdObject)
            {
                if (isCategoryComplianceRqdExecuting)
                {
                    logger.Info("Category Compliance Rqd was already in progress. Thread id {0} will exit now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                    return;
                }
                logger.Info("Thread id {0} will execute Category Compliance Rqd now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isCategoryComplianceRqdExecuting = true;

            }
            logger.Info("Entered processing for Category Compliance Rqd");
            try
            {

                if (CurrentTime >= CategoryComplianceRqdStartTime && CurrentTime <= CategoryComplianceRqdEndTime)
                {
                    CategoryComplianceRqd.ProcessCategoryComplianceRqd();
                    logger.Trace("******************* START Calling Method for Notification Non-Compliance Required Category : " + DateTime.Now.ToString() + " *******************");
                    //Send notification for Non compliant required category
                    CategoryComplianceRqd.NotificationUpcomingNonComplianceRequiredCategoryAction();
                    logger.Trace("******************* END Calling Method for Notification Non-Compliance Required Category : " + DateTime.Now.ToString() + " *******************");
                }
                else
                {
                    logger.Trace("******************* Exit Category Compliance Rqd" + DateTime.Now.ToString() + " *******************");
                }
            }
            catch (Exception ex)
            {
                logger.Info("Error in Category Compliance Rqd. Thread id {0} will exit Category Compliance Rqd.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isCategoryComplianceRqdExecuting = false;
                logger.Error("An Error has occured in Category Compliance Rqd, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                logger.Info("Category Compliance Rqd complete. Thread id {0} will exit Category Compliance Rqd now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isCategoryComplianceRqdExecuting = false;
            }
        }

        void requirement_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!isRequirementInterval)
            {
                requirementTimer.Interval = Convert.ToDouble(RequiremenServiceInterval);
                isRequirementInterval = true;
            }

            TimeSpan CurrentTime = DateTime.Now.TimeOfDay;

            logger.Info("Timer invoked for Category Compliance Rqd");
            lock (this.lockRequirementObject)
            {
                if (isRequirementExecuting)
                {
                    logger.Info("Requiremnt timer was already in progress. Thread id {0} will exit now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                    return;
                }
                logger.Info("Thread id {0} will execute Requiremnt timer.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isRequirementExecuting = true;

            }
            logger.Info("Entered processing for Requiremnt timer");
            try
            {

                if (CurrentTime >= RequirementStartTime && CurrentTime <= RequirementEndTime)
                {
                    logger.Trace("******************* START Calling Method Process Requirement Item Expiry : " + DateTime.Now.ToString() + " *******************");
                    //Send notification for Non compliant required category
                    Requirement.ProcessRequirementItemExpiry();
                    logger.Trace("******************* END Calling Method Process Requirement Item Expiry : " + DateTime.Now.ToString() + " *******************");
                    //UAT-3139 Client Admin Auto-archive rotations 1 year following the rotation end date
                    logger.Trace("******************* START Calling Method Process Rotation To Archive : " + DateTime.Now.ToString() + " *******************");
                    Requirement.ProcessRotationToArchive();
                    logger.Trace("******************* END Calling Method Process Rotation To Archive : " + DateTime.Now.ToString() + " *******************");
                }
                else
                {
                    logger.Trace("******************* Exit Requiremnt timer" + DateTime.Now.ToString() + " *******************");
                }
            }
            catch (Exception ex)
            {
                logger.Info("Error in Requiremnt timer. Thread id {0} will exit Requiremnt timer.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isRequirementExecuting = false;
                logger.Error("An Error has occured in Requiremnt timer, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                logger.Info("Requiremnt timer complete. Thread id {0} will exit Requiremnt timer now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isRequirementExecuting = false;
            }
        }

        void IncompletedOnlineOrderNotification_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!isIncompletedOnlineOrderNotificationInterval)
            {
                incompletedOnlineOrderNotificationTimer.Interval = Convert.ToDouble(IncompletedOnlineOrderNotificationServiceInterval);
                isIncompletedOnlineOrderNotificationInterval = true;
            }

            TimeSpan CurrentTime = DateTime.Now.TimeOfDay;

            logger.Info("Timer invoked for Incomplete Online Orders");
            lock (this.lockIncompletedOnlineOrderNotification)
            {
                if (isIncompletedOnlineOrderNotificationExecuting)
                {
                    logger.Info("Incomplete Online Orders timer was already in progress. Thread id {0} will exit now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                    return;
                }
                logger.Info("Thread id {0} will execute Incomplete Online Orders timer.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isIncompletedOnlineOrderNotificationExecuting = true;

            }
            logger.Info("Entered processing for Incomplete Online Orders timer");
            try
            {
                if (CurrentTime >= IncompletedOnlineOrderNotificationStartTime && CurrentTime <= IncompletedOnlineOrderNotificationEndTime)
                {
                    logger.Trace("******************* START Calling Method Send Order Notifications For Incomplete Online Orders : " + DateTime.Now.ToString() + " *******************");
                    OrderNotification.SendOrderNotificationsForIncompleteOnlineOrders();
                    logger.Trace("******************* END Calling Method Send Order Notifications For Incomplete Online Orders : " + DateTime.Now.ToString() + " *******************");
                }
                else
                {
                    logger.Trace("******************* Exit  Incomplete Online Orders timer" + DateTime.Now.ToString() + " *******************");
                }
            }
            catch (Exception ex)
            {
                logger.Info("Error in Incomplete Online Orders timer. Thread id {0} will exit Incomplete Online Orders timer.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isIncompletedOnlineOrderNotificationExecuting = false;
                logger.Error("An Error has occured in Incomplete Online Orders timer, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                logger.Info("Incomplete Online Orders timer complete. Thread id {0} will exit Incomplete Online Orders timer now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isIncompletedOnlineOrderNotificationExecuting = false;
            }
        }

        void ManageContractExpiringItemsNotification_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!isManageContractExpiringItemsNotificationInterval)
            {
                manageContractExpiringItemsNotificationTimer.Interval = Convert.ToDouble(ManageContractExpiringItemsNotificationServiceInterval);
                isManageContractExpiringItemsNotificationInterval = true;
            }

            TimeSpan CurrentTime = DateTime.Now.TimeOfDay;

            logger.Info("Timer invoked for Manage Contract Expiring Items Notification");
            lock (this.lockManageContractExpiringItemsNotification)
            {
                if (isManageContractExpiringItemsNotificationExecuting)
                {
                    logger.Info("Manage Contract Expiring Items Notification timer was already in progress. Thread id {0} will exit now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                    return;
                }
                logger.Info("Thread id {0} will execute Manage Contract Expiring Items Notification timer.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isManageContractExpiringItemsNotificationExecuting = true;
            }

            logger.Info("Entered processing for Manage Contract Expiring Items Notification");
            try
            {
                if (CurrentTime >= ManageContractExpiringItemsNotificationStartTime && CurrentTime <= ManageContractExpiringItemsNotificationEndTime)
                {
                    logger.Trace("******************* START Calling Method Send Mail For Expiring Items : " + DateTime.Now.ToString() + " *******************");
                    ManageContracts.SendMailForExpiringOrExpiredItems(CommunicationSubEvents.CONTRACT_ABOUT_TO_EXPIRE, true);
                    ManageContracts.SendMailForExpiringOrExpiredItems(CommunicationSubEvents.CONTRACT_ABOUT_TO_EXPIRE, false);
                    logger.Trace("******************* END Calling Method Send Mail For Expiring Items  : " + DateTime.Now.ToString() + " *******************");
                }
                else
                {
                    logger.Trace("******************* Exit Manage Contract Expiring Items Notification timer" + DateTime.Now.ToString() + " *******************");
                }
            }
            catch (Exception ex)
            {
                logger.Info("Error in Manage Contract Expiring Items Notification timer. Thread id {0} will exit Manage Contract Expiring Items Notification.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isManageContractExpiringItemsNotificationExecuting = false;
                logger.Error("An Error has occured in Manage Contract Expiring Items Notification timer, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                logger.Info("Manage Contract Expiring Items Notification timer complete. Thread id {0} will exit Manage Contract Expiring Items Notification timer now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isManageContractExpiringItemsNotificationExecuting = false;
            }
        }

        void ManageContractExpiredItemsNotification_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!isManageContractExpiredItemsNotificationInterval)
            {
                manageContractExpiredItemsNotificationTimer.Interval = Convert.ToDouble(ManageContractExpiredItemsNotificationServiceInterval);
                isManageContractExpiredItemsNotificationInterval = true;
            }

            TimeSpan CurrentTime = DateTime.Now.TimeOfDay;

            logger.Info("Timer invoked for Manage Contract Expired Items Notification");
            lock (this.lockManageContractExpiredItemsNotification)
            {
                if (isManageContractExpiredItemsNotificationExecuting)
                {
                    logger.Info("Manage Contract Expired Items Notification timer was already in progress. Thread id {0} will exit now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                    return;
                }
                logger.Info("Thread id {0} will execute Manage Contract Expired Items Notification timer.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isManageContractExpiredItemsNotificationExecuting = true;
            }

            logger.Info("Entered processing for Manage Contract Expired Items Notification");
            try
            {
                if (CurrentTime >= ManageContractExpiredItemsNotificationStartTime && CurrentTime <= ManageContractExpiredItemsNotificationEndTime)
                {
                    logger.Trace("******************* START Calling Method Send Mail For Expired Items : " + DateTime.Now.ToString() + " *******************");
                    ManageContracts.SendMailForExpiringOrExpiredItems(CommunicationSubEvents.CONTRACT_EXPIRED, true);
                    ManageContracts.SendMailForExpiringOrExpiredItems(CommunicationSubEvents.CONTRACT_EXPIRED, false);
                    logger.Trace("******************* END Calling Method Send Mail For Expired Items  : " + DateTime.Now.ToString() + " *******************");
                }
                else
                {
                    logger.Trace("******************* Exit Manage Contract Expired Items Notification timer" + DateTime.Now.ToString() + " *******************");
                }
            }
            catch (Exception ex)
            {
                logger.Info("Error in Manage Contract Expired Items Notification timer. Thread id {0} will exit Manage Contract Expired Items Notification.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isManageContractExpiredItemsNotificationExecuting = false;
                logger.Error("An Error has occured in Manage Contract Expired Items Notification timer, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                logger.Info("Manage Contract Expired Items Notification timer complete. Thread id {0} will exit Manage Contract Expired Items Notification timer now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isManageContractExpiredItemsNotificationExecuting = false;
            }
        }

        public void ScheduledInvitation_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!isScheduleInvitationInterval)
            {
                scheduleInvitationTimer.Interval = Convert.ToDouble(ScheduledInvitationServiceInterval);
                isScheduleInvitationInterval = true;
            }

            TimeSpan CurrentTime = DateTime.Now.TimeOfDay;

            logger.Info("Timer invoked for Scheduled Invitation Email");
            lock (this.lockScheduleInvitationObject)
            {
                if (isScheduleInvitationJobExecuting)
                {
                    logger.Info("Schedule Invitation timer was already in progress. Thread id {0} will exit now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                    return;
                }
                logger.Info("Thread id {0} will execute Schedule Invitation timer.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isScheduleInvitationJobExecuting = true;

            }
            logger.Info("Entered processing for Schedule Invitation timer");
            try
            {

                if (CurrentTime >= ScheduledInvitationStartTime && CurrentTime <= ScheduledInvitationEndTime)
                {
                    logger.Trace("******************* Started placing entry for Schedule Invitation: " + DateTime.Now.ToString() + " *******************");

                    Requirement.CompleteScheduledInvitations();

                    logger.Trace("******************* Ended placing entry for Schedule Invitation:" + DateTime.Now.ToString() + " *******************");
                }
                else
                {
                    logger.Trace("******************* Exit Schedule Invitation Job:" + DateTime.Now.ToString() + " *******************");
                }
            }
            catch (Exception ex)
            {
                logger.Info("Error in scheduled invitation job. Thread id {0} will exit scheduled invitation job now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isScheduleInvitationJobExecuting = false;
                logger.Error("An Error has occured in scheduled invitation job, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                logger.Info("Scheduled Invitation are completed. Thread id {0} will exit scheduled invitation job now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isScheduleInvitationJobExecuting = false;
            }
        }

        void rotation_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!isRotationInterval)
            {
                rotationTimer.Interval = Convert.ToDouble(RotationServiceInterval);
                isRotationInterval = true;
            }

            TimeSpan CurrentTime = DateTime.Now.TimeOfDay;

            logger.Info("Timer invoked for Rotation");
            lock (this.lockRotationObject)
            {
                if (isRotationExecuting)
                {
                    logger.Info("Rotation timer was already in progress. Thread id {0} will exit now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                    return;
                }
                logger.Info("Thread id {0} will execute Rotation timer.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isRotationExecuting = true;

            }
            logger.Info("Entered processing for Rotation timer");
            try
            {

                if (CurrentTime >= RotationStartTime && CurrentTime <= RotationEndTime)
                {
                    logger.Trace("******************* START Calling Method Process Rotation about to start : " + DateTime.Now.ToString() + " *******************");
                    Requirement.SendRotationAbtToStartNotification();
                    logger.Trace("******************* END Calling Method Process Rotation about to start : " + DateTime.Now.ToString() + " *******************");
                }
                else
                {
                    logger.Trace("******************* Exit Rotation timer" + DateTime.Now.ToString() + " *******************");
                }
            }
            catch (Exception ex)
            {
                logger.Info("Error in Rotation timer. Thread id {0} will exit Requiremnt timer.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isRotationExecuting = false;
                logger.Error("An Error has occured in Rotation timer, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                logger.Info("Rotation timer complete. Thread id {0} will exit Requiremnt timer now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isRotationExecuting = false;
            }
        }

        void MarkApplicantDocumentsComplete_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!isMarkApplicantDocumentsCompleteInterval)
            {
                markApplicantDocumentsCompleteTimer.Interval = Convert.ToDouble(MarkApplicantDocumentsCompleteServiceInterval);
                isMarkApplicantDocumentsCompleteInterval = true;
            }

            TimeSpan CurrentTime = DateTime.Now.TimeOfDay;

            logger.Info("Timer invoked to Mark Applicant Documents Complete");
            lock (this.lockMarkApplicantDocumentsComplete)
            {
                if (ismarkApplicantDocumentsCompleteExecuting)
                {
                    logger.Info("Mark Mark Applicant Documents Complete timer was already in progress. Thread id {0} will exit now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                    return;
                }
                logger.Info("Thread id {0} will execute Mark Applicant Documents Complete timer.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                ismarkApplicantDocumentsCompleteExecuting = true;
            }

            logger.Info("Entered processing to Mark Applicant Documents Complete");
            try
            {
                if (CurrentTime >= MarkApplicantDocumentsCompleteStartTime && CurrentTime <= MarkApplicantDocumentsCompleteEndTime)
                {
                    ComplianceDocumentCompletion.MarkApplicantDocumentsComplete();
                }
                else
                {
                    logger.Trace("******************* Exit Mark Applicant Documents Complete timer" + DateTime.Now.ToString() + " *******************");
                }
            }
            catch (Exception ex)
            {
                logger.Info("Error in Mark Applicant Documents Complete timer. Thread id {0} will exit Applicant Documents Complete.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                ismarkApplicantDocumentsCompleteExecuting = false;
                logger.Error("An Error has occured in Mark Applicant Documents Complete timer, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                logger.Info("Mark Applicant Documents Complete timer complete. Thread id {0} will exit Mark Applicant Documents Complete timer now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                ismarkApplicantDocumentsCompleteExecuting = false;
            }
        }
        #region UAT-963
        void ComplianceAuditDataSync_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!isComplianceAuditDataSyncInterval)
            {
                ComplianceAuditDataSyncTimer.Interval = Convert.ToDouble(ComplianceAuditDataSyncInterval);
                isComplianceAuditDataSyncInterval = true;
            }

            TimeSpan CurrentTime = DateTime.Now.TimeOfDay;

            logger.Info("Timer invoked to Compliance Audit Data Synchronise");
            lock (this.lockComplianceAuditDataSync)
            {
                if (isComplianceAuditDataSyncExecuting)
                {
                    logger.Info("Compliance Audit Data Synchronise timer was already in progress. Thread id {0} will exit now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                    return;
                }
                logger.Info("Thread id {0} will execute Compliance Audit Data Synchronise timer.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isComplianceAuditDataSyncExecuting = true;
            }

            logger.Info("Entered processing to Compliance Audit Data Synchronise");
            try
            {
                if (CurrentTime >= ComplianceAuditDataSyncStartTime && CurrentTime <= ComplianceAuditDataSyncEndTime)
                {
                    //TODO
                    ComplianceAuditDataSynchronise.SyncApplicantDataAuditData();
                }
                else
                {
                    logger.Trace("******************* Exit Compliance Audit Data Synchronise timer" + DateTime.Now.ToString() + " *******************");
                }
            }
            catch (Exception ex)
            {
                logger.Info("Error in Compliance Audit Data Synchronise timer. Thread id {0} will exit Compliance Audit Data Synchronise.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isComplianceAuditDataSyncExecuting = false;
                logger.Error("An Error has occured in Compliance Audit Data Synchronise timer, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                logger.Info("Compliance Audit Data Synchronise timer complete. Thread id {0} will exit Compliance Audit Data Synchronise timer now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isComplianceAuditDataSyncExecuting = false;
            }
        }

        #endregion

        #region UAT-2495
        void ClientDataUpload_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!isClientDataUploadInterval)
            {
                ClientDataUploadTimer.Interval = Convert.ToDouble(ClientDataUploadInterval);
                isClientDataUploadInterval = true;
            }

            TimeSpan CurrentTime = DateTime.Now.TimeOfDay;

            logger.Info("Timer invoked to Client Data Upload Service");
            lock (this.lockClientDataUpload)
            {
                if (isClientDataUploadExecuting)
                {
                    logger.Info("Client Data Upload Service timer was already in progress. Thread id {0} will exit now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                    return;
                }
                logger.Info("Thread id {0} will execute Client Data Upload Service timer.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isClientDataUploadExecuting = true;
            }

            logger.Info("Entered processing to Client Data Upload Service");
            try
            {
                if (CurrentTime >= ClientDataUploadStartTime && CurrentTime <= ClientDataUploadEndTime)
                {
                    //TODO
                    ClientDataUploadService.UploadClientData();
                }
                else
                {
                    logger.Trace("******************* Exit Client Data Upload Service timer" + DateTime.Now.ToString() + " *******************");
                }
            }
            catch (Exception ex)
            {
                logger.Info("Error in Client Data Upload Service timer. Thread id {0} will exit Client Data Upload Service.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isClientDataUploadExecuting = false;
                logger.Error("An Error has occured in Client Data Upload Service timer, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                logger.Info("Client Data Upload Service timer complete. Thread id {0} will exit Client Data Upload Service timer now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isClientDataUploadExecuting = false;
            }
        }

        #endregion

        void ReconcillationQueueSyncTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!isReconcillationQueueSyncInterval)
            {
                ReconcillationQueueSyncTimer.Interval = Convert.ToDouble(ReconcillationQueueSyncInterval);
                isReconcillationQueueSyncInterval = true;
            }

            TimeSpan CurrentTime = DateTime.Now.TimeOfDay;

            logger.Info("Timer invoked to synching ReconcillationQueue");
            lock (this.lockReconcillationQueueSyncComplete)
            {
                if (isReconcillationQueueSyncExecuting)
                {
                    logger.Info("ReconcillationQueueSync timer was already in progress. Thread id {0} will exit now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                    return;
                }
                logger.Info("Thread id {0} will execute for  ReconcillationQueueSync timer.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isReconcillationQueueSyncExecuting = true;
            }

            logger.Info("Entered processing to ReconcillationQueueSync timer");
            try
            {
                if (CurrentTime >= ReconcillationQueueSyncStartTime && CurrentTime <= ReconcillationQueueSyncEndTime)
                {
                    ComplianceAuditDataSynchronise.SyncReconcillationQueueData();
                }
                else
                {
                    logger.Trace("******************* Exit ReconcillationQueueSync timer" + DateTime.Now.ToString() + " *******************");
                }
            }
            catch (Exception ex)
            {
                isReconcillationQueueSyncExecuting = false;
                logger.Info("Error in ReconcillationQueueSync timer. Thread id {0} will exit ReconcillationQueueSync.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                logger.Error("An Error has occured in ReconcillationQueueSync timer, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                logger.Info("ReconcillationQueueSync timer complete. Thread id {0} will exit ReconcillationQueueSync timer now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isReconcillationQueueSyncExecuting = false;
            }
        }

        void ReqCategoryComplianceRqd_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!isReqCategoryComplianceRqdInterval)
            {
                reqCategoryComplianceRqdTimer.Interval = Convert.ToDouble(ReqCategoryComplianceRqdServiceInterval);
                isReqCategoryComplianceRqdInterval = true;
            }

            TimeSpan CurrentTime = DateTime.Now.TimeOfDay;

            logger.Info("Timer invoked for Category Compliance Rqd");
            lock (this.lockReqCategoryComplianceRqdObject)
            {
                if (isReqCategoryComplianceRqdExecuting)
                {
                    logger.Info(" Requiremnt Category Compliance Rqd was already in progress. Thread id {0} will exit now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                    return;
                }
                logger.Info("Thread id {0} will execute Requiremnt Category Compliance Rqd now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isReqCategoryComplianceRqdExecuting = true;

            }
            logger.Info("Entered processing for Requiremnt Category Compliance Rqd");
            try
            {

                if (CurrentTime >= ReqCategoryComplianceRqdStartTime && CurrentTime <= ReqCategoryComplianceRqdEndTime)
                {
                    logger.Trace("******************* START Calling Method ProcessRequiremntCategoryComplianceRqd : " + DateTime.Now.ToString() + " *******************");
                    CategoryComplianceRqd.ProcessRotationCategoryComplianceRqd();
                    logger.Trace("******************* END Calling Method for ProcessRequiremntCategoryComplianceRqd : " + DateTime.Now.ToString() + " *******************");
                }
                else
                {
                    logger.Trace("******************* Exit Requiremnt Category Compliance Rqd" + DateTime.Now.ToString() + " *******************");
                }
            }
            catch (Exception ex)
            {
                logger.Info("Error in Category Compliance Rqd. Thread id {0} will exit  Requiremnt Category Compliance Rqd.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isReqCategoryComplianceRqdExecuting = false;
                logger.Error("An Error has occured in Requiremnt Category Compliance Rqd, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                logger.Info("Category Compliance Rqd complete. Thread id {0} will exit Rotation Category Compliance Rqd now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isReqCategoryComplianceRqdExecuting = false;
            }
        }

        void CopyComplianceDataToRequirement_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!isCopyComplianceDataTorequirementInterval)
            {
                copyComplianceDataToRequirementTimer.Interval = Convert.ToDouble(CopyComplianceDataToRequirementInterval);
                isCopyComplianceDataTorequirementInterval = true;
            }

            TimeSpan CurrentTime = DateTime.Now.TimeOfDay;

            logger.Info("Timer invoked for Copy Compliance Data To Requirement");
            lock (this.lockCopyComplianceDataToRequirementObject)
            {
                if (isCopyComplianceDataToRequirementExecuting)
                {
                    logger.Info(" Copy Compliance Data To Requirement was already in progress. Thread id {0} will exit now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                    return;
                }
                logger.Info("Thread id {0} will execute Copy Compliance Data To Requirement now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isCopyComplianceDataToRequirementExecuting = true;

            }
            logger.Info("Entered processing for Copy Compliance Data To Requirement");
            try
            {

                if (CurrentTime >= CopyComplianceDataToRequirementStartTime && CurrentTime <= CopyComplianceDataToRequirementEndTime)
                {
                    logger.Trace("******************* START Calling Method Copy Compliance Data To Requirement : " + DateTime.Now.ToString() + " *******************");
                    ComplianceDataRequirement.CopyComplianceDataToRequirement();
                    logger.Trace("******************* END Calling Method for Copy Compliance Data To Requirement : " + DateTime.Now.ToString() + " *******************");
                }
                else
                {
                    logger.Trace("******************* Exit Copy Compliance Data To Requirement" + DateTime.Now.ToString() + " *******************");
                }
            }
            catch (Exception ex)
            {
                logger.Info("Error in Copy Compliance Data To Requirement Thread id {0} will exit  Copy Compliance Data To Requirement.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isCopyComplianceDataToRequirementExecuting = false;
                logger.Error("An Error has occured in Copy Compliance Data To Requirement, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                logger.Info("Copy Compliance Data To Requirement complete. Thread id {0} will exit Copy Compliance Data To Requirement now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isCopyComplianceDataToRequirementExecuting = false;
            }
        }

        void CreateRequirementSnapshotOnRotationEnd_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!isCreateRequirementSnapshotOnRotationEndInterval)
            {
                createRequirementSnapshotOnRotationEndTimer.Interval = Convert.ToDouble(CreateRequirementSnapshotOnRotationEndInterval);
                isCreateRequirementSnapshotOnRotationEndInterval = true;
            }

            TimeSpan CurrentTime = DateTime.Now.TimeOfDay;

            logger.Info("Timer invoked for create requirment snapshot On Rotation End");
            lock (this.lockCreateRequirementSnapshotOnRotationEndObject)
            {
                if (isCreateRequirementSnapshotOnRotationEndExecuting)
                {
                    logger.Info("Create Requirement Snapshot On Rotation End was already in progress. Thread id {0} will exit now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                    return;
                }
                logger.Info("Thread id {0} will execute create requirement snapshot on Rotation End now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isCreateRequirementSnapshotOnRotationEndExecuting = true;

            }
            logger.Info("Entered processing for create Requirement Snapshot On Rotation End");
            try
            {

                if (CurrentTime >= CreateRequirementSnapshotOnRotationEndStartTime && CurrentTime <= CreateRequirementSnapshotOnRotationEndEndTime)
                {
                    logger.Trace("******************* START Calling Method Create Requirment Snapshot On Rotation End : " + DateTime.Now.ToString() + " *******************");
                    Requirement.CreateRequirementSnapshotOnRotationEnd();
                    logger.Trace("******************* END Calling Method for Create Requirment Snapshot On Rotation End : " + DateTime.Now.ToString() + " *******************");
                }
                else
                {
                    logger.Trace("******************* Exit Create Requirement Snapshot On Rotation End" + DateTime.Now.ToString() + " *******************");
                }
            }
            catch (Exception ex)
            {
                logger.Info("Error in Create Requirement Snapshot On Rotation End Thread id {0} will exit  Create Requirment Snapshot On Rotation End.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isCreateRequirementSnapshotOnRotationEndExecuting = false;
                logger.Error("An Error has occured in Create Requirement Snapshot On Rotation End, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                logger.Info("Create Requirement Snapshot On RotationEnd complete. Thread id {0} will exit Create Requirement Snapshot On Rotation End now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isCreateRequirementSnapshotOnRotationEndExecuting = false;
            }
        }

        void RequirementPkgSync_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!isRequirementPkgSyncInterval)
            {
                requirementPkgSyncTimer.Interval = Convert.ToDouble(RequirementPkgSyncInterval);
                isRequirementPkgSyncInterval = true;
            }

            TimeSpan CurrentTime = DateTime.Now.TimeOfDay;

            logger.Info("Timer invoked for Requirement Pkg Sync");
            lock (this.lockRequirementPkgSyncObject)
            {
                if (isRequirementPkgSyncExecuting)
                {
                    logger.Info(" Requirement Pkg Sync was already in progress. Thread id {0} will exit now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                    return;
                }
                logger.Info("Thread id {0} will execute Requirement Pkg Sync now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isRequirementPkgSyncExecuting = true;

            }
            logger.Info("Entered processing for Requirement Pkg Sync");
            try
            {

                if (CurrentTime >= RequirementPkgSyncStartTime && CurrentTime <= RequirementPkgSyncEndTime)
                {
                    #region Requirement Package versioning
                    try
                    {
                        logger.Trace("******************* START Calling Method RequirementPackageVersioning : " + DateTime.Now.ToString() + " *******************");
                        Requirement.RequirementPackageVersioning();
                        logger.Trace("******************* END Calling Method for RequirementPackageVersioning : " + DateTime.Now.ToString() + " *******************");
                    }
                    catch (Exception ex)
                    {
                        logger.Info("Error in Requirement Pkg versioning Thread id {0} will exit  Copy Compliance Data To Requirement.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                        logger.Error("An Error has occured in Requirement Pkg versioning, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                    }
                    #endregion

                    #region Requirement Category Disassociation
                    try
                    {
                        logger.Trace("******************* START Calling Method ProcessRequirementCategoryDisassociation : " + DateTime.Now.ToString() + " *******************");
                        Requirement.ProcessRequirementCategoryDisassociation();
                        logger.Trace("******************* END Calling Method for ProcessRequirementCategoryDisassociation : " + DateTime.Now.ToString() + " *******************");
                    }
                    catch (Exception ex)
                    {
                        logger.Info("Error in Requirement category disassociation Thread id {0} will exit  Copy Compliance Data To Requirement.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                        logger.Error("An Error has occured in Requirement category disassociation, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                    }

                    #endregion

                    #region Requirement Package Sync
                    try
                    {
                        logger.Trace("******************* START Calling Method Requirement Pkg Sync : " + DateTime.Now.ToString() + " *******************");
                        Requirement.RequirementPkgSync();
                        logger.Trace("******************* END Calling Method for Requirement Pkg Sync : " + DateTime.Now.ToString() + " *******************");
                    }
                    catch (Exception ex)
                    {
                        logger.Info("Error in Requirement Pkg Sync Thread id {0} will exit  Copy Compliance Data To Requirement.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                        isRequirementPkgSyncExecuting = false;
                        logger.Error("An Error has occured in Requirement Pkg Sync, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                        //throw;
                    }
                    #endregion
                }

                else
                {
                    logger.Trace("******************* Exit Requirement Pkg Sync" + DateTime.Now.ToString() + " *******************");
                }
            }
            catch (Exception ex)
            {
                logger.Info("Error in Requirement Pkg Sync Thread id {0} will exit  Copy Compliance Data To Requirement.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isRequirementPkgSyncExecuting = false;
                logger.Error("An Error has occured in Requirement Pkg Sync, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                logger.Info("Requirement Pkg Sync complete. Thread id {0} will exit Requirement Pkg Sync now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isRequirementPkgSyncExecuting = false;
            }
        }

        //UAT-2533 Archive all the packages whose end date has passed.
        void RequirementPkgAutoArchive_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!isRequirementPkgAutoArchiveInterval)
            {
                requirementPkgAutoArchiveTimer.Interval = Convert.ToDouble(RequirementPkgAutoArchiveInterval);
                isRequirementPkgAutoArchiveInterval = true;
            }

            TimeSpan CurrentTime = DateTime.Now.TimeOfDay;

            logger.Info("Timer invoked for Requirement Pkg Auto Archive");
            lock (this.lockRequirementPkgAutoArchiveObject)
            {
                if (isRequirementPkgAutoArchiveExecuting)
                {
                    logger.Info(" Requirement Pkg Auto Archive was already in progress. Thread id {0} will exit now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                    return;
                }
                logger.Info("Thread id {0} will execute Requirement Pkg Auto Archive now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isRequirementPkgAutoArchiveExecuting = true;

            }
            logger.Info("Entered processing for Requirement Pkg Auto Archive ");
            try
            {

                if (CurrentTime >= RequirementPkgAutoArchiveStartTime && CurrentTime <= RequirementPkgAutoArchiveEndTime)
                {
                    logger.Trace("******************* START Calling Method Requirement Pkg AutoArchive : " + DateTime.Now.ToString() + " *******************");
                    Requirement.RequirementPkgAutoArchive();
                    logger.Trace("******************* END Calling Method for Requirement Pkg AutoArchive : " + DateTime.Now.ToString() + " *******************");
                }
                else
                {
                    logger.Trace("******************* Exit Requirement Pkg Auto Archive" + DateTime.Now.ToString() + " *******************");
                }
            }
            catch (Exception ex)
            {
                logger.Info("Error in Requirement Pkg Auto Archive Thread id {0} will exit Requirement Pkg Auto Archive.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isRequirementPkgAutoArchiveExecuting = false;
                logger.Error("An Error has occured in Requirement Pkg Auto Archive, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                logger.Info("Requirement Pkg AutoArchive complete. Thread id {0} will exit Requirement Pkg Auto Archive now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isRequirementPkgAutoArchiveExecuting = false;
            }
        }

        //UAT-2603
        void RotationDataMovement_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!isRotationDataMovementInterval)
            {
                rotDataMovementTimer.Interval = Convert.ToDouble(RotationDataMovementInterval);
                isRotationDataMovementInterval = true;
            }

            TimeSpan CurrentTime = DateTime.Now.TimeOfDay;

            logger.Info("Timer invoked for Rotation Data Movement");
            lock (this.lockRotDataMovementObject)
            {
                if (isRotDataMovementExecuting)
                {
                    logger.Info(" Rotation Data Movement was already in progress. Thread id {0} will exit now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                    return;
                }
                logger.Info("Thread id {0} will execute Rotation Data Movement now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isRotDataMovementExecuting = true;

            }
            logger.Info("Entered processing for Rotation Data Movement");
            try
            {

                if (CurrentTime >= RotationDataMovementStartTime && CurrentTime <= RotationDataMovementEndTime)
                {
                    logger.Trace("******************* START Calling Method Rotation Data Movement : " + DateTime.Now.ToString() + " *******************");
                    Requirement.RotationDataMovement();
                    logger.Trace("******************* END Calling Method for Rotation Data Movement : " + DateTime.Now.ToString() + " *******************");
                }
                else
                {
                    logger.Trace("******************* Exit Rotation Data Movement" + DateTime.Now.ToString() + " *******************");
                }
            }
            catch (Exception ex)
            {
                logger.Info("Error in Rotation Data Movement Thread id {0} will exit Rotation Data Movement.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isRotDataMovementExecuting = false;
                logger.Error("An Error has occured in Rotation Data Movement, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                logger.Info("Rotation Data Movement complete. Thread id {0} will exit Rotation Data Movement now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isRotDataMovementExecuting = false;
            }
        }

        //UAT-3112
        void BadgeFormNotification_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!isBadgeFormNotificationInterval)
            {
                badgeFrmNotifTimer.Interval = Convert.ToDouble(BadgeFormNotificationInterval);
                isBadgeFormNotificationInterval = true;
            }

            TimeSpan CurrentTime = DateTime.Now.TimeOfDay;

            logger.Info("Timer invoked for Badge Form Notification");
            lock (this.lockBadgeFormNotificationObject)
            {
                if (isBadgeFormNotificationExecuting)
                {
                    logger.Info("Badge Form Notification was already in progress. Thread id {0} will exit now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                    return;
                }
                logger.Info("Thread id {0} will execute Badge Form Notification now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isBadgeFormNotificationExecuting = true;
            }
            logger.Info("Entered processing for Badge Form Notification");
            try
            {

                if (CurrentTime >= BadgeFormNotificationStartTime && CurrentTime <= BadgeFormNotificationEndTime)
                {
                    logger.Trace("******************* START Calling Method Badge Form Notification  : " + DateTime.Now.ToString() + " *******************");

                    Requirement.SendBadgeFormNotifications();

                    logger.Trace("******************* END Calling Method for Badge Form Notification  : " + DateTime.Now.ToString() + " *******************");
                }
                else
                {
                    logger.Trace("******************* Exit Badge Form Notification " + DateTime.Now.ToString() + " *******************");
                }
            }
            catch (Exception ex)
            {
                logger.Info("Error in Badge Form Notification Thread id {0} will exit Badge Form Notification .", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isBadgeFormNotificationExecuting = false;
                logger.Error("An Error has occured in Badge Form Notification , the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                logger.Info("Badge Form Notification  complete. Thread id {0} will exit Rotation Data Movement now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isBadgeFormNotificationExecuting = false;
            }
        }

        //UAT-3669
        void AlertMailForWebCCFError_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!isAlertMailForWebCCFErrorInterval)
            {
                alertMailForWebCCFErrorTimer.Interval = Convert.ToDouble(AlertMailForWebCCFErrorInterval);
                isAlertMailForWebCCFErrorInterval = true;
            }

            TimeSpan CurrentTime = DateTime.Now.TimeOfDay;

            logger.Info("Timer invoked for Alert Mail For WebCCF Error");
            lock (this.lockAlertMailForWebCCFErrorObject)
            {
                if (isAlertMailForWebCCFErrorExecuting)
                {
                    logger.Info("Alert Mail For WebCCF Error was already in progress. Thread id {0} will exit now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                    return;
                }
                logger.Info("Thread id {0} will execute Alert Mail For WebCCF Error now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isAlertMailForWebCCFErrorExecuting = true;
            }
            logger.Info("Entered processing for Alert Mail For WebCCF Error");
            try
            {

                if (CurrentTime >= AlertMailForWebCCFErrorStartTime && CurrentTime <= AlertMailForWebCCFErrorEndTime)
                {
                    logger.Trace("******************* START Calling Method Alert Mail For WebCCF Error  : " + DateTime.Now.ToString() + " *******************");

                    OrderNotification.SendAlertMailForWebCCFError();

                    logger.Trace("******************* END Calling Method for Alert Mail For WebCCF Error  : " + DateTime.Now.ToString() + " *******************");
                }
                else
                {
                    logger.Trace("******************* Exit Alert Mail For WebCCF Error " + DateTime.Now.ToString() + " *******************");
                }
            }
            catch (Exception ex)
            {
                logger.Info("Error in Alert Mail For WebCCF Error Thread id {0} will exit Alert Mail For WebCCF Error.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isAlertMailForWebCCFErrorExecuting = false;
                logger.Error("An Error has occured in Alert Mail For WebCCF Error , the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                logger.Info("Alert Mail For WebCCF Error  complete. Thread id {0} will exit Rotation Data Movement now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isAlertMailForWebCCFErrorExecuting = false;
            }

        }

        #region UAT-2960

        //UAT-2960
        void AlumniAccessNotification_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!isAlumniAccessNotificationInterval)
            {
                alumniAccessNotifTimer.Interval = Convert.ToDouble(AlumniAccessNotificationInterval);
                isAlumniAccessNotificationInterval = true;
            }

            TimeSpan CurrentTime = DateTime.Now.TimeOfDay;

            logger.Info("Timer invoked for Alumni Access Notification");
            lock (this.lockAlumniAccessNotificationObject)
            {
                if (isAlumniAccessNotificationExecuting)
                {
                    logger.Info("Alumni Access Notification was already in progress. Thread id {0} will exit now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                    return;
                }
                logger.Info("Thread id {0} will execute Alumni Access Notification now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isAlumniAccessNotificationExecuting = true;
            }
            logger.Info("Entered processing for Alumni Access Notification");
            try
            {
                if (CurrentTime >= AlumniAccessNotificationStartTime && CurrentTime <= AlumniAccessNotificationEndTime)
                {
                    logger.Trace("******************* START Calling Method Alumni Access Notification  : " + DateTime.Now.ToString() + " *******************");

                    AlumniAccess.UpdateApplicantForAlumniAccess();


                    logger.Trace("******************* END Calling Method for Alumni Access Notification  : " + DateTime.Now.ToString() + " *******************");
                }
                else
                {
                    logger.Trace("******************* Exit Alumni Access Notification " + DateTime.Now.ToString() + " *******************");
                }
            }
            catch (Exception ex)
            {
                logger.Info("Error in Alumni Access Notification Thread id {0} will exit Alumni Access Notification .", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isAlumniAccessNotificationExecuting = false;
                logger.Error("An Error has occured in Alumni Access Notification , the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                logger.Info("Alumni Access Notification  complete. Thread id {0} will exit Rotation Data Movement now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isAlumniAccessNotificationExecuting = false;
            }
        }


        void CopyComplianceToCompliance_Elapsed(object sender, ElapsedEventArgs e)
        {

            if (!isCopyComplianceToComplianceInterval)
            {
                copyComplianceToComplianceTimer.Interval = Convert.ToDouble(CopyComplianceToComplianceInterval);
                isCopyComplianceToComplianceInterval = true;
            }

            TimeSpan CurrentTime = DateTime.Now.TimeOfDay;

            logger.Info("Timer invoked for Copy Compliance To Compliance");
            lock (this.lockCopyComplianceToComplianceObject)
            {
                if (isCopyComplianceToComplianceExecuting)
                {
                    logger.Info(" Copy Compliance To Compliance was already in progress. Thread id {0} will exit now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                    return;
                }
                logger.Info("Thread id {0} will execute Copy Compliance To Compliance now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isCopyComplianceToComplianceExecuting = true;

            }
            logger.Info("Entered processing for Copy Compliance To Compliance");
            try
            {

                if (CurrentTime >= CopyComplianceToComplianceStartTime && CurrentTime <= CopyComplianceToComplianceEndTime)
                {
                    logger.Trace("******************* START Calling Method Copy Compliance To Compliance: " + DateTime.Now.ToString() + " *******************");
                    AlumniAccess.CopyDataFromComplianceToCompliance();
                    logger.Trace("******************* END Calling Method for Copy Compliance To Compliance : " + DateTime.Now.ToString() + " *******************");
                }
                else
                {
                    logger.Trace("******************* Exit Copy Compliance To Compliance" + DateTime.Now.ToString() + " *******************");
                }
            }
            catch (Exception ex)
            {
                logger.Info("Error in Rotation Data Movement Thread id {0} will exit Copy Compliance To Compliance.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isCopyComplianceToComplianceExecuting = false;
                logger.Error("An Error has occured in Copy Compliance To Compliance, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                logger.Info("Copy Compliance To Compliance complete. Thread id {0} will exit Copy Compliance To Compliance now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isCopyComplianceToComplianceExecuting = false;
            }
        }

        void BkgCopyPackageDatae_Elapsed(object sender, ElapsedEventArgs e)
        {

            if (!isBkgCopyPackageDataInterval)
            {
                bkgCopyPackageDataTimer.Interval = Convert.ToDouble(BkgCopyPackageDataIntervalInterval);
                isBkgCopyPackageDataInterval = true;
            }

            TimeSpan CurrentTime = DateTime.Now.TimeOfDay;

            logger.Info("Timer invoked for BackgroundCopyPackgeData");
            lock (this.lockbkgCopyPackageDataObject)
            {
                if (isbkgCopyPackageDataExecuting)
                {
                    logger.Info(" Copy Compliance To Compliance was already in progress. Thread id {0} will exit now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                    return;
                }
                logger.Info("Thread id {0} will execute BackgroundCopyPackgeData now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isbkgCopyPackageDataExecuting = true;

            }
            logger.Info("Entered processing for BackgroundCopyPackgeData");
            try
            {

                if (CurrentTime >= BkgCopyPackageDataStartTime && CurrentTime <= BkgCopyPackageDataEndTime)
                {
                    logger.Trace("******************* START Calling Method BackgroundCopyPackgeData: " + DateTime.Now.ToString() + " *******************");
                    ScheduleAction.BackgroundCopyPackgeData();
                    logger.Trace("******************* END Calling Method for BackgroundCopyPackgeData : " + DateTime.Now.ToString() + " *******************");
                }
                else
                {
                    logger.Trace("******************* Exit Copy BackgroundCopyPackgeData" + DateTime.Now.ToString() + " *******************");
                }
            }
            catch (Exception ex)
            {
                logger.Info("Error in Rotation Data Movement Thread id {0} will exit BackgroundCopyPackgeData.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isbkgCopyPackageDataExecuting = false;
                logger.Error("An Error has occured in BackgroundCopyPackgeData, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                logger.Info("BackgroundCopyPackgeData complete. Thread id {0} will exit BackgroundCopyPackgeData now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isbkgCopyPackageDataExecuting = false;
            }
        }

        #endregion

        #region UAT-3485

        void SendRotationAbtToExpire_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!isSendRotationAbtToExpireInterval)
            {
                sendRotationAbtToExpireTimer.Interval = Convert.ToDouble(SendRotationAbtToExpireEmailInterval);
                isSendRotationAbtToExpireInterval = true;
            }
            TimeSpan CurrentTime = DateTime.Now.TimeOfDay;

            logger.Info("Timer invoked for Rotation Item About To Expire Notification");
            lock (this.lockSendRotationAbtToExpireObject)
            {
                if (isSendRotationAbtToExpireExecuting)
                {
                    logger.Info("Rotation Item About To Expire Notification was already in progress. Thread id {0} will exit now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                    return;
                }
                logger.Info("Thread id {0} will execute Rotation Item About To Expire Notification now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isSendRotationAbtToExpireExecuting = true;
            }

            logger.Info("Entered processing for Rotation Item About To Expire Notification");
            try
            {
                if (CurrentTime >= SendRotationAbtToExpireStartTime && CurrentTime <= SendRotationAbtToExpireEndTime)
                {
                    logger.Trace("******************* START Calling Method Rotation Item About To Expire Notification  : " + DateTime.Now.ToString() + " *******************");

                    Requirement.SendMailForRequirementExpiringItems();

                    logger.Trace("******************* END Calling Method for Rotation Item About To Expire Notification  : " + DateTime.Now.ToString() + " *******************");
                }
                else
                {
                    logger.Trace("******************* Exit Rotation Item About To Expire Notification " + DateTime.Now.ToString() + " *******************");
                }
            }
            catch (Exception ex)
            {
                logger.Info("Error in Rotation Item About To Expire Notification Thread id {0} will exit Rotation Item About To Expire Notificationn .", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isSendRotationAbtToExpireExecuting = false;
                logger.Error("An Error has occured in Alumni Access Notification , the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                logger.Info("Rotation Item About To Expire Notification  complete. Thread id {0} will exit Rotation Item About To Expire Notification now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isSendRotationAbtToExpireExecuting = false;
            }
        }

        #endregion

        #region Missed Appointments Methods
        void MissedAppointmentEmail_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!isMissedAppointmentEmailInterval)
            {
                MissedAppointmentEmailTimer.Interval = Convert.ToDouble(MissedAppointmentEmailInterval);
                isMissedAppointmentEmailInterval = true;
            }
            TimeSpan CurrentTime = DateTime.Now.TimeOfDay;

            logger.Info("Timer invoked for Missed Appointment Mail");
            lock (this.lockMissedAppointmentEmailObject)
            {
                if (isMissedAppointmentEmailExecuting)
                {
                    logger.Info("Missed Appointment Email was already in progress. Thread id {0} will exit now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                    return;
                }
                logger.Info("Thread id {0} will execute Missed Appointment Email now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isMissedAppointmentEmailExecuting = true;
            }

            logger.Info("Entered processing for Missed Appointment Mail");
            try
            {
                if (CurrentTime >= MissedAppointmentEmailStartTime && CurrentTime <= MissedAppointmentEmailEndTime)
                {
                    logger.Trace("******************* START Calling Method Missed Appointment Email  : " + DateTime.Now.ToString() + " *******************");

                    ABILocationService.SendMailForMissedAppointmentEmail();
                    //Requirement.SendMailForRequirementExpiringItems();

                    logger.Trace("******************* END Calling Method for Missed Appointment Email  : " + DateTime.Now.ToString() + " *******************");
                }
                else
                {
                    logger.Trace("******************* Exit Missed Appointment Email " + DateTime.Now.ToString() + " *******************");
                }
            }
            catch (Exception ex)
            {
                logger.Info("Error in Missed Appointment Email Thread id {0} .", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isMissedAppointmentEmailExecuting = false;
                logger.Error("An Error has occured in Alumni Access Notification , the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                logger.Info("Missed Appointment Email complete. Thread id {0} .", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isMissedAppointmentEmailExecuting = false;
            }
        }


        void MissedAppointment_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!isMissedAppointmentInterval)
            {
                MissedAppointmentTimer.Interval = Convert.ToDouble(MissedAppointmentInterval);
                isMissedAppointmentInterval = true;
            }
            TimeSpan CurrentTime = DateTime.Now.TimeOfDay;

            logger.Info("Timer invoked for Missed Appointment Status Change");
            lock (this.lockMissedAppointmentObject)
            {
                if (isMissedAppointmentExecuting)
                {
                    logger.Info("Appointment Status Change to Missed was already in progress. Thread id {0} will exit now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                    return;
                }
                logger.Info("Thread id {0} will execute Appointment Status Change to Missed now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isMissedAppointmentExecuting = true;
            }

            logger.Info("Entered processing for Appointment Status Change to Missed");
            try
            {
                if (CurrentTime >= MissedAppointmentStartTime && CurrentTime <= MissedAppointmentEndTime)
                {
                    logger.Trace("******************* START Calling Method Appointment Status Change to Missed  : " + DateTime.Now.ToString() + " *******************");

                    ABILocationService.UpdateStatusForMissedAppointments();
                    //Requirement.SendMailForRequirementExpiringItems();

                    logger.Trace("******************* END Calling Method for Appointment Status Change to Missed  : " + DateTime.Now.ToString() + " *******************");
                }
                else
                {
                    logger.Trace("******************* Exit Appointment Status Change to Missed " + DateTime.Now.ToString() + " *******************");
                }
            }
            catch (Exception ex)
            {
                logger.Info("Error in Changing Appointment Status to Missed Thread id {0} .", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isMissedAppointmentExecuting = false;
                logger.Error("An Error has occured in Alumni Access Notification , the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                logger.Info("Changing Appointment Status to Missed complete. Thread id {0} .", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isMissedAppointmentExecuting = false;
            }
        }

        #endregion

        #region UAT-3734

        void OffTimeRevokedAppointmentEmail_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!isOffTimeRevokedAppointmentEmailInterval)
            {
                OffTimeRevokedAppointmentEmailTimer.Interval = Convert.ToDouble(OffTimeRevokedAppointmentEmailInterval);
                isOffTimeRevokedAppointmentEmailInterval = true;
            }
            TimeSpan CurrentTime = DateTime.Now.TimeOfDay;

            logger.Info("Timer invoked for Off Time Revoked Appointment Mail");
            lock (this.lockOffTimeRevokedAppointmentEmailObject)
            {
                if (isOffTimeRevokedAppointmentEmailExecuting)
                {
                    logger.Info("Off Time Revoked Appointment Email was already in progress. Thread id {0} will exit now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                    return;
                }
                logger.Info("Thread id {0} will execute Off Time Revoked Appointment Email now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isOffTimeRevokedAppointmentEmailExecuting = true;
            }

            logger.Info("Entered processing for Off Time Revoked Appointment Mail");
            try
            {
                if (CurrentTime >= OffTimeRevokedAppointmentEmailStartTime && CurrentTime <= OffTimeRevokedAppointmentEmailEndTime)
                {
                    logger.Trace("******************* START Calling Method Off Time Revoked Appointment Email  : " + DateTime.Now.ToString() + " *******************");

                    ABILocationService.SendMailForOffTimeRevokedAppointmentEmail();
                    //Requirement.SendMailForRequirementExpiringItems();

                    logger.Trace("******************* END Calling Method for Off Time Revoked Appointment Email  : " + DateTime.Now.ToString() + " *******************");
                }
                else
                {
                    logger.Trace("******************* Exit Off Time Revoked Appointment Email " + DateTime.Now.ToString() + " *******************");
                }
            }
            catch (Exception ex)
            {
                logger.Info("Error in Off Time Revoked Appointment Email Thread id {0} will exit Rotation Item About To Expire Notificationn .", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isOffTimeRevokedAppointmentEmailExecuting = false;
                logger.Error("An Error has occured in Alumni Access Notification , the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                logger.Info("OffTimeRevoked Appointment Email complete. Thread id {0} will exit Rotation Item About To Expire Notification now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isOffTimeRevokedAppointmentEmailExecuting = false;
            }
        }
        #endregion
        #region UAT-3137

        private Boolean isSendRequiredRotationCategoryBeforeGoingToBeRequiredInterval = false;
        private Timer sendRequiredRotationCategoryBeforeGoingToBeRequiredTimer = null;
        private Boolean isSendRequiredRotationCategoryBeforeGoingToBeRequiredExecuting = false;
        private Object lockSendRequiredRotationCategoryBeforeFallGoingToBeRequiredObject = new Object();


        void SendRequiredRotationCategoryBeforeGoingToBeRequired_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!isSendRequiredRotationCategoryBeforeGoingToBeRequiredInterval)
            {
                sendRequiredRotationCategoryBeforeGoingToBeRequiredTimer.Interval = Convert.ToDouble(SendRotationCategoryBeforeGoingToBeRequiredEmailInterval);
                isSendRequiredRotationCategoryBeforeGoingToBeRequiredInterval = true;
            }
            TimeSpan CurrentTime = DateTime.Now.TimeOfDay;

            logger.Info("Timer invoked for Rotation Category required Notification");
            lock (this.lockSendRequiredRotationCategoryBeforeFallGoingToBeRequiredObject)
            {
                if (isSendRequiredRotationCategoryBeforeGoingToBeRequiredExecuting)
                {
                    logger.Info("Rotation Category required Notification Before Going To Be Required was already in progress. Thread id {0} will exit now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                    return;
                }
                logger.Info("Thread id {0} will execute Rotation Category required Notification Before Going To Be Required now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isSendRequiredRotationCategoryBeforeGoingToBeRequiredExecuting = true;
            }

            logger.Info("Entered processing for Rotation Category required Notification Before Going To Be Required");
            try
            {
                if (CurrentTime >= SendRotationCategoryBeforeGoingToBeRequiredStartTime && CurrentTime <= SendRotationCategoryBeforeGoingToBeRequiredEndTime)
                {
                    logger.Trace("******************* START Calling Method Rotation Category required Notification Before Going To Be Required  : " + DateTime.Now.ToString() + " *******************");

                    Requirement.SendMailRequirementCategoriesBeforeGoingToBeRequired();

                    logger.Trace("******************* END Calling Method for Rotation Category required Notification Before Going To Be Required  : " + DateTime.Now.ToString() + " *******************");
                }
                else
                {
                    logger.Trace("******************* Exit Rotation Category required Notification Before Going To Be Required " + DateTime.Now.ToString() + " *******************");
                }
            }
            catch (Exception ex)
            {
                logger.Info("Error in Rotation Category required Notification Before Going To Be Required Thread id {0} will exit Rotation Category required Notification Before Going To Be Required.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isSendRequiredRotationCategoryBeforeGoingToBeRequiredExecuting = false;
                logger.Error("An Error has occured in Rotation Category required Notification Before Going To Be Required , the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                logger.Info("Rotation Category required Notification Before Going To Be Required  complete. Thread id {0} will exit Rotation Category To Going To Be Required Notification now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isSendRequiredRotationCategoryBeforeGoingToBeRequiredExecuting = false;
            }
        }


        #endregion

        #region UAT-3137

        public TimeSpan SendRotationCategoryBeforeGoingToBeRequiredStartTime
        {
            get
            {
                return new TimeSpan(_sendRotationCategoryBeforeGoingToBeRequiredEmailFromHour, SendRotationCategoryBeforeGoingToBeRequiredEmailFromMinute, 0);
            }
        }
        public TimeSpan SendRotationCategoryBeforeGoingToBeRequiredEndTime
        {
            get
            {
                return new TimeSpan(SendRotationCategoryBeforeGoingToBeRequiredEmailToHour, SendRotationCategoryBeforeGoingToBeRequiredEmailToMinute, 0);
            }
        }
        private Int32 _sendRotationCategoryBeforeGoingToBeRequiredEmailInterval = 86400000;
        public Int32 SendRotationCategoryBeforeGoingToBeRequiredEmailInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("SendRotationCategoryBeforeGoingToBeRequiredEmailInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["SendRotationCategoryBeforeGoingToBeRequiredEmailInterval"]) ? ConfigurationManager.AppSettings["SendRotationCategoryBeforeGoingToBeRequiredEmailInterval"] : _sendRotationCategoryBeforeGoingToBeRequiredEmailInterval.ToString());
                else
                    return _sendRotationCategoryBeforeGoingToBeRequiredEmailInterval;
            }
        }

        private Int32 _sendRotationCategoryBeforeGoingToBeRequiredEmailFromHour = 0;
        public Int32 SendRotationCategoryBeforeGoingToBeRequiredEmailFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("SendRotationCategoryBeforeGoingToBeRequiredEmailFromHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["SendRotationCategoryBeforeGoingToBeRequiredEmailFromHour"]) ? ConfigurationManager.AppSettings["SendRotationCategoryBeforeGoingToBeRequiredEmailFromHour"] : _sendRotationCategoryBeforeGoingToBeRequiredEmailFromHour.ToString());
                else
                    return _sendRotationCategoryBeforeGoingToBeRequiredEmailFromHour;
            }
        }

        private Int32 _sendRotationCategoryBeforeGoingToBeRequiredEmailFromMinute = 0;
        public Int32 SendRotationCategoryBeforeGoingToBeRequiredEmailFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("SendRotationCategoryBeforeGoingToBeRequiredEmailFromMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["SendRotationCategoryBeforeGoingToBeRequiredEmailFromMinute"]) ? ConfigurationManager.AppSettings["SendRotationCategoryBeforeGoingToBeRequiredEmailFromMinute"] : _sendRotationCategoryBeforeGoingToBeRequiredEmailFromMinute.ToString());
                else
                    return _sendRotationCategoryBeforeGoingToBeRequiredEmailFromMinute;
            }
        }

        private Int32 _sendRotationCategoryBeforeGoingToBeRequiredEmailToHour = 24;
        public Int32 SendRotationCategoryBeforeGoingToBeRequiredEmailToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("SendRotationCategoryBeforeGoingToBeRequiredEmailToHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["SendRotationCategoryBeforeGoingToBeRequiredEmailToHour"]) ? ConfigurationManager.AppSettings["SendRotationCategoryBeforeGoingToBeRequiredEmailToHour"] : _sendRotationCategoryBeforeGoingToBeRequiredEmailToHour.ToString());
                else
                    return _sendRotationCategoryBeforeGoingToBeRequiredEmailToHour;
            }
        }

        private Int32 _sendRotationCategoryBeforeGoingToBeRequiredEmailToMinute = 0;
        public Int32 SendRotationCategoryBeforeGoingToBeRequiredEmailToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("SendRotationCategoryBeforeGoingToBeRequiredEmailToMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["SendRotationCategoryBeforeGoingToBeRequiredEmailToMinute"]) ? ConfigurationManager.AppSettings["SendRotationCategoryBeforeGoingToBeRequiredEmailToMinute"] : _sendRotationCategoryBeforeGoingToBeRequiredEmailToMinute.ToString());
                else
                    return _sendRotationCategoryBeforeGoingToBeRequiredEmailToMinute;
            }
        }

        #endregion


        #region UAT-2628:
        void ConversionAndMergingForFailedApplicantDocument_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!isConversionMergingForFailedDocumentInterval)
            {
                ConversionMergingForFailedDocumentTimer.Interval = Convert.ToDouble(ConversionMergingFailedDocumentServiceInterval);
                isConversionMergingForFailedDocumentInterval = true;
            }

            TimeSpan CurrentTime = DateTime.Now.TimeOfDay;

            logger.Info("Timer invoked to Conversion Merging for Failed Document");
            lock (this.lockConversionMergingForFailedDocument)
            {
                if (isConversionMergingForFailedDocumentExecuting)
                {
                    logger.Info("Conversion Merging for Failed Document timer was already in progress. Thread id {0} will exit now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                    return;
                }
                logger.Info("Thread id {0} will execute Conversion Merging for Failed Document timer.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isConversionMergingForFailedDocumentExecuting = true;
            }

            logger.Info("Entered processing to Conversion Merging for Failed Document");
            try
            {
                if (CurrentTime >= ConversionAndMergingFailedDocumentStartTime && CurrentTime <= ConversionAndMergingFailedDocumentEndTime)
                {
                    ComplianceDocumentCompletion.ConvertAndMergeFailedApplicantDocument();
                }
                else
                {
                    logger.Trace("******************* Exit Conversion Merging for Failed Document timer" + DateTime.Now.ToString() + " *******************");
                }
            }
            catch (Exception ex)
            {
                logger.Info("Error in Conversion Merging for Failed Document timer. Thread id {0} will exit Conversion Merging for Failed Document.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isConversionMergingForFailedDocumentExecuting = false;
                logger.Error("An Error has occured in Conversion Merging for Failed Document timer, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                logger.Info("Conversion Merging for Failed Document timer complete. Thread id {0} will exit Conversion Merging for Failed Document timer now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isConversionMergingForFailedDocumentExecuting = false;
            }
        }
        #endregion

        #region UAT-2388:AutomaticPackageInvitation
        void AutomaticPackageInvitation_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!isAutomaticPackageInvitationInterval)
            {
                AutomaticPackageInvitationTimer.Interval = Convert.ToDouble(AutomaticPackageInvitationInterval);
                isAutomaticPackageInvitationInterval = true;
            }

            TimeSpan CurrentTime = DateTime.Now.TimeOfDay;

            logger.Info("Timer invoked to Automatic Package Invitation");
            lock (this.lockAutomaticPackageInvitationObject)
            {
                if (isAutomaticPackageInvitationExecuting)
                {
                    logger.Info("AutomaticPackageInvitation timer was already in progress. Thread id {0} will exit now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                    return;
                }
                logger.Info("Thread id {0} will execute AutomaticPackageInvitation timer.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isAutomaticPackageInvitationExecuting = true;
            }

            logger.Info("Entered processing to AutomaticPackageInvitation");
            try
            {
                if (CurrentTime >= AutomaticPackageInvitationStartTime && CurrentTime <= AutomaticPackageInvitationEndTime)
                {
                    ComplianceDataRequirement.SendAutomaticPackageInvitationEmail();
                }
                else
                {
                    logger.Trace("******************* Exit AutomaticPackageInvitation timer" + DateTime.Now.ToString() + " *******************");
                }
            }
            catch (Exception ex)
            {
                logger.Info("Error in AutomaticPackageInvitation timer. Thread id {0} will exit AutomaticPackageInvitation.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isAutomaticPackageInvitationExecuting = false;
                logger.Error("An Error has occured in AutomaticPackageInvitation timer, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                logger.Info("Conversion Merging for AutomaticPackageInvitation timer complete. Thread id {0} will exit AutomaticPackageInvitation timer now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isAutomaticPackageInvitationExecuting = false;
            }
        }
        #endregion

        #region UAT-3059:UpdatedApplicantRequirementsNotification
        void UpdatedApplicantRequirementsNotification_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!isUpdatedApplicantRequirementsNotificationInterval)
            {
                UpdatedApplicantRequirementsNotificationTimer.Interval = Convert.ToDouble(UpdatedApplicantRequirementsNotificationInterval);
                isUpdatedApplicantRequirementsNotificationInterval = true;
            }

            TimeSpan CurrentTime = DateTime.Now.TimeOfDay;

            logger.Info("Timer invoked to UpdatedApplicantRequirementsNotification");
            lock (this.lockUpdatedApplicantRequirementsNotificationObject)
            {
                if (isUpdatedApplicantRequirementsNotificationExecuting)
                {
                    logger.Info("UpdatedApplicantRequirementsNotification timer was already in progress. Thread id {0} will exit now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                    return;
                }
                logger.Info("Thread id {0} will execute UpdatedApplicantRequirementsNotification timer.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isUpdatedApplicantRequirementsNotificationExecuting = true;
            }

            logger.Info("Entered processing to UpdatedApplicantRequirementsNotification");
            try
            {
                if (CurrentTime >= UpdatedApplicantRequirementsNotificationStartTime && CurrentTime <= UpdatedApplicantRequirementsNotificationEndTime)
                {
                    Requirement.UpdatedApplicantRequirementsNotification();
                }
                else
                {
                    logger.Trace("******************* Exit UpdatedApplicantRequirementsNotification timer" + DateTime.Now.ToString() + " *******************");
                }
            }
            catch (Exception ex)
            {
                logger.Info("Error in UpdatedApplicantRequirementsNotification timer. Thread id {0} will exit UpdatedApplicantRequirementsNotification.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isUpdatedApplicantRequirementsNotificationExecuting = false;
                logger.Error("An Error has occured in UpdatedApplicantRequirementsNotification timer, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                logger.Info("Conversion Merging for UpdatedApplicantRequirementsNotification timer complete. Thread id {0} will exit UpdatedApplicantRequirementsNotification timer now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isUpdatedApplicantRequirementsNotificationExecuting = false;
            }
        }
        #endregion

        #region UAT-2513:Feature for Client Admin to batch upload rotation creation
        void BatchRotationUpload_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!isBatchRotationUploadInterval)
            {
                BatchRotationUploadTimer.Interval = Convert.ToDouble(BatchRotationUploadInterval);
                isBatchRotationUploadInterval = true;
            }

            TimeSpan CurrentTime = DateTime.Now.TimeOfDay;

            logger.Info("Timer invoked to Batch Rotation Upload");
            lock (this.lockBatchRotationUploadObject)
            {
                if (isBatchRotationUploadExecuting)
                {
                    logger.Info("BatchRotationUpload timer was already in progress. Thread id {0} will exit now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                    return;
                }
                logger.Info("Thread id {0} will execute BatchRotationUpload timer.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isBatchRotationUploadExecuting = true;
            }

            logger.Info("Entered processing to BatchRotationUpload");
            try
            {
                if (CurrentTime >= BatchRotationUploadStartTime && CurrentTime <= BatchRotationUploadEndTime)
                {
                    Requirement.BatchRotationUploadThroughExcel();
                }
                else
                {
                    logger.Trace("******************* Exit BatchRotationUpload timer" + DateTime.Now.ToString() + " *******************");
                }
            }
            catch (Exception ex)
            {
                logger.Info("Error in BatchRotationUpload timer. Thread id {0} will exit BatchRotationUpload.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isBatchRotationUploadExecuting = false;
                logger.Error("An Error has occured in BatchRotationUpload timer, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                logger.Info("Conversion Merging for BatchRotationUpload timer complete. Thread id {0} will exit BatchRotationUpload timer now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isBatchRotationUploadExecuting = false;
            }
        }
        #endregion

        //UAT:CAB Digestion Procedure Call
        #region DigestionProcedureCall


        void bkgDigestionProcedure_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!isBkgDigestionProcedureCallInterval)
            {
                bkgDigestionProcedureTimer.Interval = Convert.ToDouble(BkgDigestionProcedureCallInterval);
                isBkgDigestionProcedureCallInterval = true;
            }

            TimeSpan CurrentTime = DateTime.Now.TimeOfDay;

            logger.Info("Timer invoked to Digestion Procedure Call");
            lock (this.lockbkgDigestionProcedureObject)
            {
                if (isbkgDigestionProcedureExecuting)
                {
                    logger.Info("Digestion Procedure Call timer was already in progress. Thread id {0} will exit now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                    return;
                }
                logger.Info("Thread id {0} will execute Digestion Procedure Call timer.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isbkgDigestionProcedureExecuting = true;
            }

            logger.Info("Entered processing to Digestion Procedure Call");
            try
            {
                if (CurrentTime >= BkgDigestionProcedureStartTime && CurrentTime <= BkgDigestionProcedureEndTime)
                {
                    ScheduleAction.BackgroundAppointmentScheduleDigestion();
                }
                else
                {
                    logger.Trace("******************* Exit Digestion Procedure Call timer" + DateTime.Now.ToString() + " *******************");
                }
            }
            catch (Exception ex)
            {
                logger.Info("Error in Digestion Procedure Call timer. Thread id {0} will exit bkgDigestionProcedure.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isbkgDigestionProcedureExecuting = false;
                logger.Error("An Error has occured in bkgDigestionProcedure timer, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                logger.Info("Conversion Merging for bkgDigestionProcedure timer complete. Thread id {0} will exit bkgDigestionProcedure timer now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isbkgDigestionProcedureExecuting = false;
            }
        }
        #endregion

        #region UAT-3950
        void ArchiveRotationdata_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!isArchiveRotationDataTimerInterval)
            {
                ArchiveRotationDataTimer.Interval = Convert.ToDouble(ArchiveRotationDataInterval);
                isArchiveRotationDataTimerInterval = true;
            }

            TimeSpan CurrentTime = DateTime.Now.TimeOfDay;

            logger.Info("Timer invoked to Archive Rotation Data Service");
            lock (this.lockArchiveRotationData)
            {
                if (isArchiveRotationDataExecuting)
                {
                    logger.Info("Archive Rotation Data Service timer was already in progress. Thread id {0} will exit now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                    return;
                }
                logger.Info("Thread id {0} will execute Archive Rotation Data Service timer.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isArchiveRotationDataExecuting = true;
            }

            logger.Info("Entered processing to Archive Rotation Data Service");
            try
            {
                if (CurrentTime >= ArchiveRotationDataStartTime && CurrentTime <= ArchiveRotationDataEndTime)
                {
                    Requirement.AutomaticallyArchiveRotation();
                }
                else
                {
                    logger.Trace("******************* Exit Archive Rotation Data Service timer" + DateTime.Now.ToString() + " *******************");
                }
            }
            catch (Exception ex)
            {
                logger.Info("Error in Archive Rotation Data Service timer. Thread id {0} will exitArchive Rotation Data Service.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isArchiveRotationDataExecuting = false;
                logger.Error("An Error has occured in Archive Rotation Data Service timer, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                logger.Info("Archive Rotation Data Service timer complete. Thread id {0} will exit Client Data Upload Service timer now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isArchiveRotationDataExecuting = false;
            }
        }

        #endregion
        #region UAT-3820
        void ReceivedFromStuServFormStatus_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!isReceivedFromStuServFormStatusInterval)
            {
                receivedFromStuServFormStatusDataTimer.Interval = Convert.ToDouble(ReceivedFromStudentServiceFormStatusInterval);
                isReceivedFromStuServFormStatusInterval = true;
            }

            TimeSpan CurrentTime = DateTime.Now.TimeOfDay;

            logger.Info("Timer invoked for SendMailForReceivedFromStudentServiceFormStatus");
            lock (this.lockbkgReceivedFromStuServFormStatusDataObject)
            {
                if (isReceivedFromStuServFormStatusDataExecuting)
                {
                    logger.Info("Send mail for received from student service form status was already in progress. Thread id {0} will exit now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                    return;
                }
                logger.Info("Thread id {0} will execute SendMailForReceivedFromStudentServiceFormStatus now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isReceivedFromStuServFormStatusDataExecuting = true;

            }
            logger.Info("Entered processing for SendMailForReceivedFromStudentServiceFormStatus");
            try
            {

                if (CurrentTime >= ReceivedFromStuServFormStatusDataStartTime && CurrentTime <= ReceivedFromStuServFormStatuDataEndTime)
                {
                    logger.Trace("******************* START Calling Method SendMailForReceivedFromStudentServiceFormStatus: " + DateTime.Now.ToString() + " *******************");
                    NotificationService.SendMailForReceivedFromStudentServiceFormStatus();//ScheduleAction.BackgroundCopyPackgeData();
                    logger.Trace("******************* END Calling Method for SendMailForReceivedFromStudentServiceFormStatus : " + DateTime.Now.ToString() + " *******************");
                }
                else
                {
                    logger.Trace("******************* Exit Copy SendMailForReceivedFromStudentServiceFormStatus" + DateTime.Now.ToString() + " *******************");
                }
            }
            catch (Exception ex)
            {
                logger.Info("Error in Rotation Data Movement Thread id {0} will exit SendMailForReceivedFromStudentServiceFormStatus.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isReceivedFromStuServFormStatusDataExecuting = false;
                logger.Error("An Error has occured in SendMailForReceivedFromStudentServiceFormStatus, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                logger.Info("SendMailForReceivedFromStudentServiceFormStatus complete. Thread id {0} will exit SendMailForReceivedFromStudentServiceFormStatus now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isReceivedFromStuServFormStatusDataExecuting = false;
            }
        }
        #endregion



        private bool SendMail(Email email, out String errormessage)
        {
            //SmtpClient smtpClient = new SmtpClient();
            Guid temp = Guid.NewGuid();
            errormessage = String.Empty;
            try
            {
                using (MailMessage message = new MailMessage())
                {
                    //MailMessage message = new MailMessage();
                    //smtpClient.Host = ConfigurationManager.AppSettings["host"].ToString();
                    //smtpClient.Port = Convert.ToInt32(ConfigurationManager.AppSettings["port"]);
                    //smtpClient.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["userName"].ToString(), ConfigurationManager.AppSettings["password"].ToString());

                    message.From = email.From;
                    foreach (MailAddress toAddress in email.ToAddresses)
                        message.To.Add(toAddress);

                    foreach (MailAddress ccAddress in email.CcAddresses)
                        message.CC.Add(ccAddress);

                    foreach (MailAddress bccAddress in email.BccAddresses)
                        message.Bcc.Add(bccAddress);

                    if (email.MailAttachments.IsNotNull())
                    {
                        foreach (var mailAttachment in email.MailAttachments)
                        {
                            DocumentInfo docInfo = (DocumentInfo)mailAttachment.Value;

                            byte[] contentByte = null;
                            if (!docInfo.AttachmentTypeCode.IsNullOrEmpty() && docInfo.AttachmentTypeCode == DocumentAttachmentType.REQUIREMENT_EXPLANATION.GetStringValue())
                            {
                                if (File.Exists(docInfo.DocumentPath))
                                {
                                    contentByte = File.ReadAllBytes(docInfo.DocumentPath);
                                }
                            }
                            else
                                contentByte = CommonFileManager.RetrieveDocument(docInfo.DocumentPath, FileType.ApplicantFileLocation.GetStringValue());
                            if (contentByte != null)
                            {
                                MemoryStream stream = new MemoryStream(contentByte);
                                System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(stream, docInfo.DocumentName);
                                message.Attachments.Add(attachment);
                            }
                            else
                            {
                                logger.Error("An Error has occured sending the mail, the details of which are: Cannot get file bytes for attchment. File path:" + docInfo.DocumentPath + ". Current context key : " + ServiceContext.currentThreadContextKeyString);
                                errormessage = "An Error has occured sending the mail, the details of which are: Cannot get file bytes for attchment. File path:" + docInfo.DocumentPath + ". Current context key : " + ServiceContext.currentThreadContextKeyString;
                                return false;
                            }
                        }
                    }

                    message.Subject = email.Subject;
                    message.IsBodyHtml = true;
                    message.Priority = email.Priority;
                    message.Body = email.Body;
                    logger.Info(DateTime.Now.ToString() + ": " + " : sending email " + temp);
                    if ((email.ToAddresses != null && email.ToAddresses.Count > 0) || (email.CcAddresses != null && email.CcAddresses.Count > 0)
                        || (email.BccAddresses != null && email.BccAddresses.Count > 0))
                    {
                        Intsof.SMTPService.SMTPService smtpService = new Intsof.SMTPService.SMTPService();
                        smtpService.SendMail(message);
                        //smtpClient.Send(message);
                    }
                    logger.Info(DateTime.Now.ToString() + " : sent email " + temp);
                    //emailCounter = emailCounter + 1;
                }
            }
            catch (Exception ex)
            {
                logger.Error("An Error has occured sending the mail, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                errormessage = "An Error has occured sending the mail, the details of which are: " + ex.Message + ", Inner Exception: " + ex.InnerException + ", Stack Trace: " + ex.StackTrace + ", current context key : " + ServiceContext.currentThreadContextKeyString;
                System.Threading.Thread.Sleep(15000);
                return false;
            }
            finally
            {
                // smtpClient.Dispose();
            }
            return true;
        }

        protected override void OnStart(string[] args)
        {
            if (scheduleTimer != null)
            {
                scheduleTimer.Enabled = true;
                scheduleTimer.Start();
                logger.Info("EmailDispatcherService start..");
            }

            if (scheduleNotificationTimer != null)
            {
                scheduleNotificationTimer.Enabled = true;
                scheduleNotificationTimer.Start();
                logger.Info("scheduleNotificationTimer started..");
            }

            if (complianceNotificationTimer != null)
            {
                complianceNotificationTimer.Enabled = true;
                complianceNotificationTimer.Start();
                logger.Info("complianceNotificationTimer started..");
            }

            if (scheduleActionTimer != null)
            {
                scheduleActionTimer.Enabled = true;
                scheduleActionTimer.Start();
                logger.Info("complianceItemReoccurNotificationTimer started..");
            }

            if (mobilityInstanceTimer != null)
            {
                mobilityInstanceTimer.Enabled = true;
                mobilityInstanceTimer.Start();
                logger.Info("mobilityInstanceTimer started..");
            }

            if (copyPackageDataTimer != null)
            {
                copyPackageDataTimer.Enabled = true;
                copyPackageDataTimer.Start();
                logger.Info("copyPackageDataTimer started..");
            }

            if (adminWidgetDataTimer != null)
            {
                adminWidgetDataTimer.Enabled = true;
                adminWidgetDataTimer.Start();
                logger.Info("adminWidgetDataTimer started..");
            }

            if (systemServiceTriggerTimer != null)
            {
                systemServiceTriggerTimer.Enabled = true;
                systemServiceTriggerTimer.Start();
                logger.Info("systemServiceTriggerTimer started..");
            }

            if (nagEmailTimer != null)
            {
                nagEmailTimer.Enabled = true;
                nagEmailTimer.Start();
                logger.Info("nagEmailTimer started..");
            }

            if (deadlineEmailTimer != null)
            {
                deadlineEmailTimer.Enabled = true;
                deadlineEmailTimer.Start();
                logger.Info("deadlineEmailTimer started..");
            }

            if (scheduleTaskTimer != null)
            {
                scheduleTaskTimer.Enabled = true;
                scheduleTaskTimer.Start();
                logger.Info("scheduleTaskTimer started..");
            }

            if (queueImagingTimer != null)
            {
                queueImagingTimer.Enabled = true;
                queueImagingTimer.Start();
                logger.Info("queueImagingTimer started..");
            }

            if (categoryComplianceRqdTimer != null)
            {
                categoryComplianceRqdTimer.Enabled = true;
                categoryComplianceRqdTimer.Start();
                logger.Info("categoryComplianceRqdTimer started..");
            }

            if (requirementTimer != null)
            {
                requirementTimer.Enabled = true;
                requirementTimer.Start();
                logger.Info("requirementTimer started..");
            }

            if (scheduleInvitationTimer != null)
            {
                scheduleInvitationTimer.Enabled = true;
                scheduleInvitationTimer.Start();
                logger.Info("scheduleInvitationTimer started..");
            }

            if (rotationTimer != null)
            {
                rotationTimer.Enabled = true;
                rotationTimer.Start();
                logger.Info("RotationTimer started..");
            }

            if (incompletedOnlineOrderNotificationTimer != null)
            {
                incompletedOnlineOrderNotificationTimer.Enabled = true;
                incompletedOnlineOrderNotificationTimer.Start();
                logger.Info("IncompletedOnlineOrderNotificationTimer started..");
            }

            if (manageContractExpiringItemsNotificationTimer != null)
            {
                manageContractExpiringItemsNotificationTimer.Enabled = true;
                manageContractExpiringItemsNotificationTimer.Start();
                logger.Info("ManageContractExpiringItemsNotificationTimer started..");
            }

            if (manageContractExpiredItemsNotificationTimer != null)
            {
                manageContractExpiredItemsNotificationTimer.Enabled = true;
                manageContractExpiredItemsNotificationTimer.Start();
                logger.Info("manageContractExpiredItemsNotificationTimer started..");
            }

            if (markApplicantDocumentsCompleteTimer != null)
            {
                markApplicantDocumentsCompleteTimer.Enabled = true;
                markApplicantDocumentsCompleteTimer.Start();
                logger.Info("applicantDocumentsCompleteNotificationTimer started..");
            }

            //UAT-963:
            if (ComplianceAuditDataSyncTimer != null)
            {
                ComplianceAuditDataSyncTimer.Enabled = true;
                ComplianceAuditDataSyncTimer.Start();
                logger.Info("ComplianceAuditDataSyncTimer started..");
            }

            if (ReconcillationQueueSyncTimer != null)
            {
                ReconcillationQueueSyncTimer.Enabled = true;
                ReconcillationQueueSyncTimer.Start();
                logger.Info("ReconcillationQueueSync started..");
            }

            if (reqCategoryComplianceRqdTimer != null)
            {
                reqCategoryComplianceRqdTimer.Enabled = true;
                reqCategoryComplianceRqdTimer.Start();
                logger.Info("reqCategoryComplianceRqdTimer started..");
            }

            //UAT-2305
            if (copyComplianceDataToRequirementTimer != null)
            {
                copyComplianceDataToRequirementTimer.Enabled = true;
                copyComplianceDataToRequirementTimer.Start();
                logger.Info("copyComplianceDataToRequirementTimer started..");
            }


            //UAT-2414
            if (createRequirementSnapshotOnRotationEndTimer != null)
            {
                createRequirementSnapshotOnRotationEndTimer.Enabled = true;
                createRequirementSnapshotOnRotationEndTimer.Start();
                logger.Info("createRequirementSnapshotOnRotationEndTimer started..");
            }

            //UAT-2495
            if (ClientDataUploadTimer != null)
            {
                ClientDataUploadTimer.Start();
                logger.Info("ClientDataUploadTimer on Continue started..");
            }

            //UAT-2514
            if (requirementPkgSyncTimer != null)
            {
                requirementPkgSyncTimer.Start();
                logger.Info("requirementPkgSyncTimer on Continue started..");
            }

            //UAT-2533
            if (requirementPkgAutoArchiveTimer != null)
            {
                requirementPkgAutoArchiveTimer.Start();
                logger.Info("requirementPkgAutoArchiveTimer on Continue started..");
            }

            //UAT-2603
            if (rotDataMovementTimer != null)
            {
                rotDataMovementTimer.Start();
                logger.Info("rotDataMovementTimer on Continue started..");
            }

            //UAT-2628
            if (ConversionMergingForFailedDocumentTimer != null)
            {
                ConversionMergingForFailedDocumentTimer.Enabled = true;
                ConversionMergingForFailedDocumentTimer.Start();
                logger.Info("ConversionMergingForFailedDocumentTimer started..");
            }

            //UAT-2388 AutomaticPackageInvitation
            if (AutomaticPackageInvitationTimer != null)
            {
                AutomaticPackageInvitationTimer.Enabled = true;
                AutomaticPackageInvitationTimer.Start();
                logger.Info("AutomaticPackageInvitationTimer started..");
            }
            //UAT-2513 Feature for Client Admin to batch upload rotation creation
            if (BatchRotationUploadTimer != null)
            {
                BatchRotationUploadTimer.Enabled = true;
                BatchRotationUploadTimer.Start();
                logger.Info("BatchRotationUploadTimer started..");
            }

            //UAT-3059: UpdatedApplicantRequirementsNotification
            if (UpdatedApplicantRequirementsNotificationTimer != null)
            {
                UpdatedApplicantRequirementsNotificationTimer.Enabled = true;
                UpdatedApplicantRequirementsNotificationTimer.Start();
                logger.Info("UpdatedApplicantRequirementsNotification started..");
            }

            //UAT-3112
            if (badgeFrmNotifTimer != null)
            {
                badgeFrmNotifTimer.Enabled = true;
                badgeFrmNotifTimer.Start();
                logger.Info("badgeFrmNotifTimer  on Continue started..");
            }
            //UAT-3669
            if (alertMailForWebCCFErrorTimer != null)
            {
                alertMailForWebCCFErrorTimer.Enabled = true;
                alertMailForWebCCFErrorTimer.Start();
                logger.Info("alertMailForWebCCFErrorTimer  on Continue started..");
            }

            //UAT-2960
            if (alumniAccessNotifTimer != null)
            {
                alumniAccessNotifTimer.Enabled = true;
                alumniAccessNotifTimer.Start();
                logger.Info("alumniAccessNotifTimer  on Continue started..");
            }

            if (copyComplianceToComplianceTimer != null)
            {
                copyComplianceToComplianceTimer.Enabled = true;
                copyComplianceToComplianceTimer.Start();
                logger.Info("copyComplianceToComplianceTimer  on Continue started..");
            }

            if (bkgCopyPackageDataTimer != null)
            {
                bkgCopyPackageDataTimer.Enabled = true;
                bkgCopyPackageDataTimer.Start();
                logger.Info("BackgroundCopyPackgeData  on Continue started..");
            }
            //UAT-3485
            if (sendRotationAbtToExpireTimer != null)
            {
                sendRotationAbtToExpireTimer.Enabled = true;
                sendRotationAbtToExpireTimer.Start();
                logger.Info("SendRotationEmailAbtToExpier Continue started..");
            }
            if (bkgDigestionProcedureTimer != null)
            {
                bkgDigestionProcedureTimer.Enabled = true;
                bkgDigestionProcedureTimer.Start();
                logger.Info("BackgroundDigestionProcedureCall  on Continue started..");

            }
            //UAT-3137
            if (sendRequiredRotationCategoryBeforeGoingToBeRequiredTimer != null)
            {
                sendRequiredRotationCategoryBeforeGoingToBeRequiredTimer.Enabled = true;
                sendRequiredRotationCategoryBeforeGoingToBeRequiredTimer.Start();
                logger.Info("sendRequiredRotationCategoryBeforeGoingToBeRequiredTimer Continue started..");
            }
            if (OffTimeRevokedAppointmentEmailTimer != null)
            {
                OffTimeRevokedAppointmentEmailTimer.Enabled = true;
                OffTimeRevokedAppointmentEmailTimer.Start();
                logger.Info("OffTimeRevokedAppointmentEmailTimer Continue started..");
            }
            if (MissedAppointmentEmailTimer != null)
            {
                MissedAppointmentEmailTimer.Enabled = true;
                MissedAppointmentEmailTimer.Start();
                logger.Info("MissedAppointmentEmailTimer Continue started..");
            }
            if (MissedAppointmentTimer != null)
            {
                MissedAppointmentTimer.Enabled = true;
                MissedAppointmentTimer.Start();
                logger.Info("MissedAppointmentTimer Continue started..");
            }
            if (ArchiveRotationDataTimer != null)
            {
                ArchiveRotationDataTimer.Enabled = true;
                ArchiveRotationDataTimer.Start();
                logger.Info("ArchiveRotationDataTimer Continue started..");
            }
            //UAT-3820
            if (receivedFromStuServFormStatusDataTimer != null)
            {
                receivedFromStuServFormStatusDataTimer.Enabled = true;
                receivedFromStuServFormStatusDataTimer.Start();
                logger.Info("Received From Student service Form status Continue started..");
            }

            //Complio TalkDesk Integration - Create Report API Call
            if (complioTalkDeskCreateReportJobTimer != null)
            {
                complioTalkDeskCreateReportJobTimer.Enabled = true;
                complioTalkDeskCreateReportJobTimer.Start();
                logger.Info("Complio TalkDesk Integration - Create Report API Call Continue started..");
            }

            //Complio TalkDesk Integration - Update Report API Call
            if (complioTalkDeskUpdateReportJobTimer != null)
            {
                complioTalkDeskUpdateReportJobTimer.Enabled = true;
                complioTalkDeskUpdateReportJobTimer.Start();
                logger.Info("Complio TalkDesk Integration - Update Report API Call Continue started..");
            }

            //Complio TalkDesk Integration - Pull Call data from Report Job API Call
            if (complioTalkDeskPullCallDataJobTimer != null)
            {
                complioTalkDeskPullCallDataJobTimer.Enabled = true;
                complioTalkDeskPullCallDataJobTimer.Start();
                logger.Info("Complio TalkDesk Integration - Pull Call data from Report Job API Call Continue started..");
            }

            if (SendDarftOrderNotificationtoAdminTimer != null)
            {
                SendDarftOrderNotificationtoAdminTimer.Enabled = true;
                SendDarftOrderNotificationtoAdminTimer.Start();
                logger.Info("Send Darft Order Notification to Admin Timer Started.");
            }

            if (SendInvitationPendingOrderNotificationtoApplicantTimer != null)
            {
                SendInvitationPendingOrderNotificationtoApplicantTimer.Enabled = true;
                SendInvitationPendingOrderNotificationtoApplicantTimer.Start();
                logger.Info("Send Invitation Pending Order Notification to ApplicantTimer Started.");
            }

            if (DeleteDraftOrderStatusTimer != null)
            {
                DeleteDraftOrderStatusTimer.Enabled = true;
                DeleteDraftOrderStatusTimer.Start();
                logger.Info("Delete Draft Order Status Timer Started.");
            }

            if (ChangeOrderStatusCompleteToArchivedTimer != null)
            {
                ChangeOrderStatusCompleteToArchivedTimer.Enabled = true;
                ChangeOrderStatusCompleteToArchivedTimer.Start();
                logger.Info("Change Order Status Complete To Archived Timer Started.");
            }

            if (BKgCompletedOrderToClientAdminTimer != null)
            {
                BKgCompletedOrderToClientAdminTimer.Enabled = true;
                BKgCompletedOrderToClientAdminTimer.Start();
                logger.Info("BKg Completed Order To Client Admin Timer Started.");
            }

            if (BKgCompletedOrderWithAttachedPDFToClientAdminTimer != null)
            {
                BKgCompletedOrderWithAttachedPDFToClientAdminTimer.Enabled = true;
                BKgCompletedOrderWithAttachedPDFToClientAdminTimer.Start();
                logger.Info("BKg Completed Order With Attached PDF To Client Admin Timer Started.");
            }
            //UAT-4613
            if (notificationForServiceFormInIPAStatusFromStudentTimer != null)
            {
                notificationForServiceFormInIPAStatusFromStudentTimer.Enabled = true;
                notificationForServiceFormInIPAStatusFromStudentTimer.Start();
                logger.Info("Notification For Service Form In IPA Status From Student Timer Started.");
            }
        }

        protected override void OnStop()
        {
            if (scheduleTimer != null)
            {
                scheduleTimer.Stop();
                logger.Info("scheduleTimer stopped..");
            }

            if (scheduleNotificationTimer != null)
            {
                scheduleNotificationTimer.Stop();
                logger.Info("scheduleNotificationTimer stopped..");
            }

            if (complianceNotificationTimer != null)
            {
                complianceNotificationTimer.Stop();
                logger.Info("complianceNotificationTimer stopped..");
            }

            if (scheduleActionTimer != null)
            {
                scheduleActionTimer.Stop();
                logger.Info("complianceItemReoccurNotificationTimer stopped..");
            }

            if (mobilityInstanceTimer != null)
            {
                mobilityInstanceTimer.Stop();
                logger.Info("mobilityInstanceTimer stopped..");
            }

            if (copyPackageDataTimer != null)
            {
                copyPackageDataTimer.Stop();
                logger.Info("copyPackageDataTimer stopped..");
            }

            if (adminWidgetDataTimer != null)
            {
                adminWidgetDataTimer.Stop();
                logger.Info("adminWidgetDataTimer stopped..");
            }

            if (systemServiceTriggerTimer != null)
            {
                systemServiceTriggerTimer.Stop();
                logger.Info("systemServiceTriggerTimer stopped..");
            }

            if (nagEmailTimer != null)
            {
                nagEmailTimer.Stop();
                logger.Info("nagEmailTimer stopped..");
            }

            if (deadlineEmailTimer != null)
            {
                deadlineEmailTimer.Stop();
                logger.Info("deadlineEmailTimer stopped..");
            }

            if (scheduleTaskTimer != null)
            {
                scheduleTaskTimer.Stop();
                logger.Info("scheduleTaskTimer stopped..");
            }

            if (queueImagingTimer != null)
            {
                queueImagingTimer.Stop();
                logger.Info("queueImagingTimer stopped..");
            }

            if (categoryComplianceRqdTimer != null)
            {
                categoryComplianceRqdTimer.Stop();
                logger.Info("categoryComplianceRqdTimer stopped..");
            }

            if (requirementTimer != null)
            {
                requirementTimer.Stop();
                logger.Info("requirementTimer stopped..");
            }

            if (scheduleInvitationTimer != null)
            {
                scheduleInvitationTimer.Stop();
                logger.Info("scheduleInvitationTimer stopped..");
            }

            if (rotationTimer != null)
            {
                rotationTimer.Stop();
                logger.Info("RotationTimer stopped..");
            }

            if (incompletedOnlineOrderNotificationTimer != null)
            {
                incompletedOnlineOrderNotificationTimer.Stop();
                logger.Info("IncompletedOnlineOrderNotificationTimer stopped..");
            }

            if (manageContractExpiringItemsNotificationTimer != null)
            {
                manageContractExpiringItemsNotificationTimer.Stop();
                logger.Info("ManageContractExpiringItemsNotificationTimer stopped..");
            }

            if (manageContractExpiredItemsNotificationTimer != null)
            {
                manageContractExpiredItemsNotificationTimer.Stop();
                logger.Info("ManageContractExpiredItemsNotificationTimer stopped..");
            }

            if (markApplicantDocumentsCompleteTimer != null)
            {
                markApplicantDocumentsCompleteTimer.Stop();
                logger.Info("ApplicantDocumentsCompleteNotificationTimer stopped..");
            }

            //UAT-963
            if (ComplianceAuditDataSyncTimer != null)
            {
                ComplianceAuditDataSyncTimer.Stop();
                logger.Info("ComplianceAuditDataSyncTimer stopped..");
            }

            //UAT-2495
            if (ClientDataUploadTimer != null)
            {
                ClientDataUploadTimer.Stop();
                logger.Info("ClientDataUploadTimer stopped..");
            }

            //UAT-2514
            if (requirementPkgSyncTimer != null)
            {
                requirementPkgSyncTimer.Stop();
                logger.Info("requirementPkgSyncTimer stopped..");
            }

            if (ReconcillationQueueSyncTimer != null)
            {
                ReconcillationQueueSyncTimer.Stop();
                logger.Info("ReconcillationQueueSyncTimer stopped..");
            }

            if (reqCategoryComplianceRqdTimer != null)
            {
                reqCategoryComplianceRqdTimer.Stop();
                logger.Info("reqCategoryComplianceRqdTimer stopped..");
            }
            //UAT-2305
            if (copyComplianceDataToRequirementTimer != null)
            {
                copyComplianceDataToRequirementTimer.Stop();
                logger.Info("copyComplianceDataToRequirementTimer stopped..");
            }

            //UAT-2414
            if (createRequirementSnapshotOnRotationEndTimer != null)
            {
                createRequirementSnapshotOnRotationEndTimer.Stop();
                logger.Info("createRequirementSnapshotOnRotationEndTimer stopped..");
            }

            //UAT-2533
            if (requirementPkgAutoArchiveTimer != null)
            {
                requirementPkgAutoArchiveTimer.Stop();
                logger.Info("requirementPkgAutoArchiveTimer stopped..");
            }

            //UAT-2603
            if (rotDataMovementTimer != null)
            {
                rotDataMovementTimer.Stop();
                logger.Info("rotDataMovementTimer stopped..");
            }

            //UAT-2628
            if (ConversionMergingForFailedDocumentTimer != null)
            {
                ConversionMergingForFailedDocumentTimer.Stop();
                logger.Info("ConversionMergingForFailedDocumentTimer stopped..");
            }

            //UAT-2388 AutomaticPackageInvitation
            if (AutomaticPackageInvitationTimer != null)
            {
                AutomaticPackageInvitationTimer.Stop();
                logger.Info("AutomaticPackageInvitationTimer stopped..");
            }
            //UAT-2513 Feature for Client Admin to batch upload rotation creation
            if (BatchRotationUploadTimer != null)
            {
                BatchRotationUploadTimer.Stop();
                logger.Info("BatchRotationUploadTimer stopped..");
            }

            //UAT-3059: UpdatedApplicantRequirementsNotification
            if (UpdatedApplicantRequirementsNotificationTimer != null)
            {
                UpdatedApplicantRequirementsNotificationTimer.Stop();
                logger.Info("UpdatedApplicantRequirementsNotification stopped..");
            }

            //UAT-3112
            if (badgeFrmNotifTimer != null)
            {
                badgeFrmNotifTimer.Stop();
                logger.Info("badgeFrmNotifTimer stopped..");
            }
            //UAT-3669
            if (alertMailForWebCCFErrorTimer != null)
            {
                alertMailForWebCCFErrorTimer.Stop();
                logger.Info("alertMailForWebCCFErrorTimer stopped..");
            }

            //UAT-2960
            if (alumniAccessNotifTimer != null)
            {
                alumniAccessNotifTimer.Stop();
                logger.Info("alumniAccessNotifTimer stopped..");
            }

            if (copyComplianceToComplianceTimer != null)
            {
                copyComplianceToComplianceTimer.Stop();
                logger.Info("copyComplianceToComplianceTimer stopped..");
            }

            if (bkgCopyPackageDataTimer != null)
            {
                bkgCopyPackageDataTimer.Stop();
                logger.Info("BackgroundCopyPackgeData stopped..");
            }
            //UAT-3485
            if (sendRotationAbtToExpireTimer != null)
            {
                sendRotationAbtToExpireTimer.Stop();
                logger.Info("sendRotationAbtToExpire stopped");
            }
            if (bkgDigestionProcedureTimer != null)
            {
                bkgDigestionProcedureTimer.Stop();
                logger.Info("BackgroundDigestionProcedureCall stopped..");
            }


            //UAT-3137
            if (sendRequiredRotationCategoryBeforeGoingToBeRequiredTimer != null)
            {
                sendRequiredRotationCategoryBeforeGoingToBeRequiredTimer.Stop();
                logger.Info("sendRequiredRotationCategoryBeforeGoingToBeRequiredTimer stopped");
            }
            if (OffTimeRevokedAppointmentEmailTimer != null)
            {
                OffTimeRevokedAppointmentEmailTimer.Stop();
                logger.Info("OffTimeRevokedAppointmentEmailTimer stopped");
            }
            if (MissedAppointmentEmailTimer != null)
            {
                MissedAppointmentEmailTimer.Stop();
                logger.Info("MissedAppointmentEmailTimer stopped");
            }
            if (MissedAppointmentTimer != null)
            {
                MissedAppointmentTimer.Stop();
                logger.Info("MissedAppointmentTimer stopped");
            }

            //UAT-3820
            if (receivedFromStuServFormStatusDataTimer != null)
            {
                receivedFromStuServFormStatusDataTimer.Stop();
                logger.Info("Received From Student service Form status stopped.");
            }

            if (ArchiveRotationDataTimer != null)
            {
                ArchiveRotationDataTimer.Stop();
                logger.Info("ArchiveRotationDataTimer stopped.");
            }

            //Complio TalkDesk Integration - Create Report API Call
            if (complioTalkDeskCreateReportJobTimer != null)
            {
                complioTalkDeskCreateReportJobTimer.Stop();
                logger.Info("ComplioTalkDeskCreateReportJobTimer stopped.");
            }

            //Complio TalkDesk Integration - Update Report API Call
            if (complioTalkDeskUpdateReportJobTimer != null)
            {
                complioTalkDeskUpdateReportJobTimer.Stop();
                logger.Info("ComplioTalkDeskUpdateReportJobTimer stopped.");
            }

            //Complio TalkDesk Integration - Pull Call data from Report Job API Call
            if (complioTalkDeskPullCallDataJobTimer != null)
            {
                complioTalkDeskPullCallDataJobTimer.Stop();
                logger.Info("ComplioTalkDeskPullCallDataJobTimer stopped.");
            }

            if (SendDarftOrderNotificationtoAdminTimer != null)
            {
                SendDarftOrderNotificationtoAdminTimer.Stop();
                logger.Info("SendDarftOrderNotificationtoAdminTimer stopped.");
            }

            if (SendInvitationPendingOrderNotificationtoApplicantTimer != null)
            {
                SendInvitationPendingOrderNotificationtoApplicantTimer.Stop();
                logger.Info("SendInvitationPendingOrderNotificationtoApplicantTimer stopped.");
            }

            if (DeleteDraftOrderStatusTimer != null)
            {
                DeleteDraftOrderStatusTimer.Stop();
                logger.Info("DeleteDraftOrderStatusTimer stopped.");
            }

            if (ChangeOrderStatusCompleteToArchivedTimer != null)
            {
                ChangeOrderStatusCompleteToArchivedTimer.Stop();
                logger.Info("ChangeOrderStatusCompleteToArchivedTimer stopped.");
            }

            if (BKgCompletedOrderToClientAdminTimer != null)
            {
                BKgCompletedOrderToClientAdminTimer.Stop();
                logger.Info("BKgCompletedOrderToClientAdminTimer stopped.");
            }

            if (BKgCompletedOrderWithAttachedPDFToClientAdminTimer != null)
            {
                BKgCompletedOrderWithAttachedPDFToClientAdminTimer.Stop();
                logger.Info("BKgCompletedOrderWithAttachedPDFToClientAdminTimer stopped.");
            }

            //UAT-4613
            if (notificationForServiceFormInIPAStatusFromStudentTimer != null)
            {
                notificationForServiceFormInIPAStatusFromStudentTimer.Stop();
                logger.Info("notification For Service Form In IPA Status From Student Timer stopped.");
            }
        }

        protected override void OnPause()
        {
            if (scheduleTimer != null)
            {
                scheduleTimer.Stop();
                logger.Info("scheduleTimer on Pause stopped..");
            }

            if (scheduleNotificationTimer != null)
            {
                scheduleNotificationTimer.Stop();
                logger.Info("scheduleNotificationTimer on Pause stopped..");
            }

            if (complianceNotificationTimer != null)
            {
                complianceNotificationTimer.Stop();
                logger.Info("complianceNotificationTimer on Pause stopped..");
            }

            if (scheduleActionTimer != null)
            {
                scheduleActionTimer.Stop();
                logger.Info("complianceItemReoccurNotificationTimer on Pause stopped..");
            }

            if (mobilityInstanceTimer != null)
            {
                mobilityInstanceTimer.Stop();
                logger.Info("mobilityInstanceTimer stopped..");
            }

            if (copyPackageDataTimer != null)
            {
                copyPackageDataTimer.Stop();
                logger.Info("copyPackageDataTimer stopped..");
            }

            if (adminWidgetDataTimer != null)
            {
                adminWidgetDataTimer.Stop();
                logger.Info("adminWidgetDataTimer stopped..");
            }

            if (systemServiceTriggerTimer != null)
            {
                systemServiceTriggerTimer.Stop();
                logger.Info("systemServiceTriggerTimer stopped..");
            }

            if (nagEmailTimer != null)
            {
                nagEmailTimer.Stop();
                logger.Info("nagEmailTimer stopped..");
            }

            if (deadlineEmailTimer != null)
            {
                deadlineEmailTimer.Stop();
                logger.Info("deadlineEmailTimer stopped..");
            }

            if (scheduleTaskTimer != null)
            {
                scheduleTaskTimer.Stop();
                logger.Info("scheduleTaskTimer stopped..");
            }

            if (queueImagingTimer != null)
            {
                queueImagingTimer.Stop();
                logger.Info("queueImagingTimer stopped..");
            }

            if (categoryComplianceRqdTimer != null)
            {
                categoryComplianceRqdTimer.Stop();
                logger.Info("categoryComplianceRqdTimer stopped..");
            }

            if (requirementTimer != null)
            {
                requirementTimer.Stop();
                logger.Info("requirementTimer on Pause stopped..");
            }

            if (scheduleInvitationTimer != null)
            {
                scheduleInvitationTimer.Stop();
                logger.Info("scheduleInvitationTimer stopped..");
            }

            if (rotationTimer != null)
            {
                rotationTimer.Stop();
                logger.Info("RotationTimer on Pause stopped..");
            }

            if (incompletedOnlineOrderNotificationTimer != null)
            {
                incompletedOnlineOrderNotificationTimer.Stop();
                logger.Info("IncompletedOnlineOrderNotificationTimer on Pause stopped..");
            }

            if (manageContractExpiringItemsNotificationTimer != null)
            {
                manageContractExpiringItemsNotificationTimer.Stop();
                logger.Info("ManageContractExpiringItemsNotificationTimer on Pause stopped..");
            }

            if (manageContractExpiringItemsNotificationTimer != null)
            {
                manageContractExpiringItemsNotificationTimer.Stop();
                logger.Info("ManageContractExpiringItemsNotificationTimer on Pause stopped..");
            }

            if (markApplicantDocumentsCompleteTimer != null)
            {
                markApplicantDocumentsCompleteTimer.Stop();
                logger.Info("ApplicantDocumentsCompleteNotificationTimer on Pause stopped..");
            }

            //UAT-963
            if (ComplianceAuditDataSyncTimer != null)
            {
                ComplianceAuditDataSyncTimer.Stop();
                logger.Info("ComplianceAuditDataSyncTimer on Pause stopped..");
            }

            //UAT-2495
            if (ClientDataUploadTimer != null)
            {
                ClientDataUploadTimer.Stop();
                logger.Info("ClientDataUploadTimer on Pause stopped..");
            }

            if (ReconcillationQueueSyncTimer != null)
            {
                ReconcillationQueueSyncTimer.Stop();
                logger.Info("ReconcillationQueueSyncTimer on Pause stopped..");
            }

            if (reqCategoryComplianceRqdTimer != null)
            {
                reqCategoryComplianceRqdTimer.Stop();
                logger.Info("reqCategoryComplianceRqdTimer on Pause stopped..");
            }
            //UAT-2305
            if (copyComplianceDataToRequirementTimer != null)
            {
                copyComplianceDataToRequirementTimer.Stop();
                logger.Info("copyComplianceDataToRequirementTimer on Pause stopped..");
            }

            //UAT-2414
            if (createRequirementSnapshotOnRotationEndTimer != null)
            {
                createRequirementSnapshotOnRotationEndTimer.Stop();
                logger.Info("createRequirementSnapshotOnRotationEndTimer on Pause stopped..");
            }

            //UAT-2514
            if (requirementPkgSyncTimer != null)
            {
                requirementPkgSyncTimer.Stop();
                logger.Info("requirementPkgSyncTimer on Pause stopped..");
            }

            //UAT-2533
            if (requirementPkgAutoArchiveTimer != null)
            {
                requirementPkgAutoArchiveTimer.Stop();
                logger.Info("requirementPkgAutoArchiveTimer on Pause stopped..");
            }

            //UAT-2603
            //UAT-2514
            if (rotDataMovementTimer != null)
            {
                rotDataMovementTimer.Stop();
                logger.Info("rotDataMovementTimer on Pause stopped..");
            }

            //UAT-2628
            if (ConversionMergingForFailedDocumentTimer != null)
            {
                ConversionMergingForFailedDocumentTimer.Stop();
                logger.Info("ConversionMergingForFailedDocumentTimer on Pause stopped..");
            }
            //UAT-2388 AutomaticPackageInvitation
            if (AutomaticPackageInvitationTimer != null)
            {
                AutomaticPackageInvitationTimer.Stop();
                logger.Info("AutomaticPackageInvitationTimer on Pause stopped..");
            }
            //UAT-2513 Feature for Client Admin to batch upload rotation creation
            if (BatchRotationUploadTimer != null)
            {
                BatchRotationUploadTimer.Stop();
                logger.Info("BatchRotationUploadTimer on Pause stopped..");
            }

            //UAT-3059: UpdatedApplicantRequirementsNotification
            if (UpdatedApplicantRequirementsNotificationTimer != null)
            {
                UpdatedApplicantRequirementsNotificationTimer.Stop();
                logger.Info("UpdatedApplicantRequirementsNotificationTimer on Pause stopped..");
            }
            //UAT-3112
            if (badgeFrmNotifTimer != null)
            {
                badgeFrmNotifTimer.Stop();
                logger.Info("badgeFrmNotifTimer on Pause stopped..");
            }
            //UAT-3669
            if (alertMailForWebCCFErrorTimer != null)
            {
                alertMailForWebCCFErrorTimer.Stop();
                logger.Info("alertMailForWebCCFErrorTimer on Pause stopped..");
            }
            //UAT-2960
            if (alumniAccessNotifTimer != null)
            {
                alumniAccessNotifTimer.Stop();
                logger.Info("alumniAccessNotifTimer on Pause stopped..");
            }

            if (copyComplianceToComplianceTimer != null)
            {
                copyComplianceToComplianceTimer.Stop();
                logger.Info("copyComplianceToComplianceTimer on Pause stopped..");
            }
            if (bkgCopyPackageDataTimer != null)
            {
                bkgCopyPackageDataTimer.Stop();
                logger.Info("BackgroundCopyPackgeData stopped..");
            }
            //UAT-3485
            if (sendRotationAbtToExpireTimer != null)
            {
                sendRotationAbtToExpireTimer.Stop();
                logger.Info("sendRotationAbtToExpireTimer stopped..");
            }
            //UAT-3137
            if (sendRequiredRotationCategoryBeforeGoingToBeRequiredTimer != null)
            {
                sendRequiredRotationCategoryBeforeGoingToBeRequiredTimer.Stop();
                logger.Info("sendRequiredRotationCategoryBeforeGoingToBeRequiredTimer stopped..");
            }
            if (bkgDigestionProcedureTimer != null)
            {
                bkgDigestionProcedureTimer.Stop();
                logger.Info("BackgroundDigestionProcedure stopped..");
            }
            if (OffTimeRevokedAppointmentEmailTimer != null)
            {
                OffTimeRevokedAppointmentEmailTimer.Stop();
                logger.Info("OffTimeRevokedAppointmentEmailTimer stopped");
            }
            if (MissedAppointmentEmailTimer != null)
            {
                MissedAppointmentEmailTimer.Stop();
                logger.Info("MissedAppointmentEmailTimer stopped");
            }
            if (MissedAppointmentTimer != null)
            {
                MissedAppointmentTimer.Stop();
                logger.Info("MissedAppointmentTimer stopped");
            }

            //UAT-3820
            if (receivedFromStuServFormStatusDataTimer != null)
            {
                receivedFromStuServFormStatusDataTimer.Stop();
                logger.Info("Received From Student service Form status stopped.");
            }

            if (ArchiveRotationDataTimer != null)
            {
                ArchiveRotationDataTimer.Stop();
                logger.Info("ArchiveRotationDataTimer stopped.");
            }

            //Complio TalkDesk Integration - Create Report API Call
            if (complioTalkDeskCreateReportJobTimer != null)
            {
                complioTalkDeskCreateReportJobTimer.Stop();
                logger.Info("ComplioTalkDeskCreateReportJobTimer paused stopped.");
            }

            //Complio TalkDesk Integration - Update Report API Call
            if (complioTalkDeskUpdateReportJobTimer != null)
            {
                complioTalkDeskUpdateReportJobTimer.Stop();
                logger.Info("ComplioTalkDeskUpdateReportJobTimer paused stopped.");
            }

            //Complio TalkDesk Integration - Pull Call data from Report Job API Call
            if (complioTalkDeskPullCallDataJobTimer != null)
            {
                complioTalkDeskPullCallDataJobTimer.Stop();
                logger.Info("ComplioTalkDeskPullCallDataJobTimer paused stopped.");
            }

            if (SendDarftOrderNotificationtoAdminTimer != null)
            {
                SendDarftOrderNotificationtoAdminTimer.Stop();
                logger.Info("SendDarftOrderNotificationtoAdminTimer stopped.");
            }

            if (SendInvitationPendingOrderNotificationtoApplicantTimer != null)
            {
                SendInvitationPendingOrderNotificationtoApplicantTimer.Stop();
                logger.Info("SendInvitationPendingOrderNotificationtoApplicantTimer stopped.");
            }

            if (DeleteDraftOrderStatusTimer != null)
            {
                DeleteDraftOrderStatusTimer.Stop();
                logger.Info("DeleteDraftOrderStatusTimer stopped.");
            }

            if (ChangeOrderStatusCompleteToArchivedTimer != null)
            {
                ChangeOrderStatusCompleteToArchivedTimer.Stop();
                logger.Info("ChangeOrderStatusCompleteToArchivedTimer stopped.");
            }

            if (BKgCompletedOrderToClientAdminTimer != null)
            {
                BKgCompletedOrderToClientAdminTimer.Stop();
                logger.Info("BKgCompletedOrderToClientAdminTimer stopped.");
            }

            if (BKgCompletedOrderWithAttachedPDFToClientAdminTimer != null)
            {
                BKgCompletedOrderWithAttachedPDFToClientAdminTimer.Stop();
                logger.Info("BKgCompletedOrderWithAttachedPDFToClientAdminTimer stopped.");
            }

            //UAT-4613
            if (notificationForServiceFormInIPAStatusFromStudentTimer != null)
            {
                notificationForServiceFormInIPAStatusFromStudentTimer.Stop();
                logger.Info("Notification For Service Form In IPA Status From Student Timer stopped.");
            }
        }

        protected override void OnContinue()
        {
            if (scheduleTimer != null)
            {
                scheduleTimer.Start();
                logger.Info("scheduleTimer on Continue started..");
            }

            if (scheduleNotificationTimer != null)
            {
                scheduleNotificationTimer.Start();
                logger.Info("scheduleNotificationTimer on Continue started..");
            }

            if (complianceNotificationTimer != null)
            {
                complianceNotificationTimer.Start();
                logger.Info("complianceNotificationTimer on Continue started..");
            }

            if (scheduleActionTimer != null)
            {
                scheduleActionTimer.Start();
                logger.Info("complianceItemReoccurNotificationTimer on Continue started..");
            }

            if (mobilityInstanceTimer != null)
            {
                mobilityInstanceTimer.Start();
                logger.Info("mobilityInstanceTimer on Continue started..");
            }

            if (copyPackageDataTimer != null)
            {
                copyPackageDataTimer.Start();
                logger.Info("copyPackageDataTimer on Continue started..");
            }

            if (adminWidgetDataTimer != null)
            {
                adminWidgetDataTimer.Start();
                logger.Info("adminWidgetDataTimer on Continue started..");
            }

            if (systemServiceTriggerTimer != null)
            {
                systemServiceTriggerTimer.Start();
                logger.Info("systemServiceTriggerTimer on Continue started..");
            }

            if (nagEmailTimer != null)
            {
                nagEmailTimer.Start();
                logger.Info("nagEmailTimer on Continue started..");
            }

            if (deadlineEmailTimer != null)
            {
                deadlineEmailTimer.Start();
                logger.Info("deadlineEmailTimer on Continue started..");
            }

            if (scheduleTaskTimer != null)
            {
                scheduleTaskTimer.Start();
                logger.Info("scheduleTaskTimer on Continue started..");
            }

            if (queueImagingTimer != null)
            {
                queueImagingTimer.Start();
                logger.Info("queueImagingTimer on Continue started..");
            }

            if (categoryComplianceRqdTimer != null)
            {
                categoryComplianceRqdTimer.Start();
                logger.Info("categoryComplianceRqdTimer on Continue started..");
            }

            if (requirementTimer != null)
            {
                requirementTimer.Start();
                logger.Info("requirementTimer on Continue started..");
            }

            if (scheduleInvitationTimer != null)
            {
                scheduleInvitationTimer.Start();
                logger.Info("scheduleInvitationTimer on Continue started..");
            }

            if (rotationTimer != null)
            {
                rotationTimer.Start();
                logger.Info("RotationTimer on Continue started..");
            }

            if (incompletedOnlineOrderNotificationTimer != null)
            {
                incompletedOnlineOrderNotificationTimer.Start();
                logger.Info("IncompletedOnlineOrderNotificationTimer on Continue started..");
            }

            if (manageContractExpiringItemsNotificationTimer != null)
            {
                manageContractExpiringItemsNotificationTimer.Start();
                logger.Info("ManageContractExpiringItemsNotificationTimer on Continue started..");
            }

            if (manageContractExpiredItemsNotificationTimer != null)
            {
                manageContractExpiredItemsNotificationTimer.Start();
                logger.Info("ManageContractExpiredItemsNotificationTimer on Continue started..");
            }

            if (markApplicantDocumentsCompleteTimer != null)
            {
                markApplicantDocumentsCompleteTimer.Start();
                logger.Info("ApplicantDocumentsCompleteNotificationTimer on Continue started..");
            }

            //UAT-963
            if (ComplianceAuditDataSyncTimer != null)
            {
                ComplianceAuditDataSyncTimer.Start();
                logger.Info("ComplianceAuditDataSyncTimer on Continue started..");
            }

            //UAT-2495
            if (ClientDataUploadTimer != null)
            {
                ClientDataUploadTimer.Start();
                logger.Info("ClientDataUploadTimer on Continue started..");
            }

            if (ReconcillationQueueSyncTimer != null)
            {
                ReconcillationQueueSyncTimer.Start();
                logger.Info("ReconcillationQueueSyncTimer on Continue started..");
            }

            if (reqCategoryComplianceRqdTimer != null)
            {
                reqCategoryComplianceRqdTimer.Start();
                logger.Info("reqCategoryComplianceRqdTimer on Continue started..");
            }
            //UAT-2305 
            if (copyComplianceDataToRequirementTimer != null)
            {
                copyComplianceDataToRequirementTimer.Start();
                logger.Info("copyComplianceDataToRequirementTimer on Continue started..");
            }
            //UAT-2414 
            if (createRequirementSnapshotOnRotationEndTimer != null)
            {
                createRequirementSnapshotOnRotationEndTimer.Start();
                logger.Info("createRequirementSnapshotOnRotationEndTimer on Continue started..");
            }
            //
            //UAT-2514 
            if (requirementPkgSyncTimer != null)
            {
                requirementPkgSyncTimer.Start();
                logger.Info("requirementPkgSyncTimer on Continue started..");
            }

            //UAT-2533 
            if (requirementPkgAutoArchiveTimer != null)
            {
                requirementPkgAutoArchiveTimer.Start();
                logger.Info("requirementPkgAutoArchiveTimer on Continue started..");
            }

            //UAT-2603
            if (rotDataMovementTimer != null)
            {
                rotDataMovementTimer.Start();
                logger.Info("rotDataMovementTimer on Continue started..");
            }
            //UAT-2628
            if (ConversionMergingForFailedDocumentTimer != null)
            {
                ConversionMergingForFailedDocumentTimer.Start();
                logger.Info("ConversionMergingForFailedDocumentTimer on Continue started..");
            }

            //UAT-2388 AutomaticPackageInvitation
            if (AutomaticPackageInvitationTimer != null)
            {
                AutomaticPackageInvitationTimer.Start();
                logger.Info("AutomaticPackageInvitationTimer on Continue started..");
            }
            //UAT-2513 Feature for Client Admin to batch upload rotation creation
            if (BatchRotationUploadTimer != null)
            {
                BatchRotationUploadTimer.Start();
                logger.Info("BatchRotationUploadTimer on Continue started..");
            }

            //UAT-3059: UpdatedApplicantRequirementsNotification
            if (UpdatedApplicantRequirementsNotificationTimer != null)
            {
                UpdatedApplicantRequirementsNotificationTimer.Start();
                logger.Info("UpdatedApplicantRequirementsNotificationTimer on Continue started..");
            }

            //UAT-3112
            if (badgeFrmNotifTimer != null)
            {
                badgeFrmNotifTimer.Start();
                logger.Info("badgeFrmNotifTimer  on Continue started..");
            }
            //UAT-3669
            if (alertMailForWebCCFErrorTimer != null)
            {
                alertMailForWebCCFErrorTimer.Start();
                logger.Info("alertMailForWebCCFErrorTimer  on Continue started..");
            }
            //UAT-2960
            if (alumniAccessNotifTimer != null)
            {
                alumniAccessNotifTimer.Start();
                logger.Info("alumniAccessNotifTimer  on Continue started..");
            }

            if (copyComplianceToComplianceTimer != null)
            {
                copyComplianceToComplianceTimer.Start();
                logger.Info("copyComplianceToComplianceTimer  on Continue started..");
            }
            if (bkgCopyPackageDataTimer != null)
            {
                bkgCopyPackageDataTimer.Start();
                logger.Info("BackgroundCopyPackgeData stopped..");
            }
            //UAT-3485
            if (sendRotationAbtToExpireTimer != null)
            {
                sendRotationAbtToExpireTimer.Start();
                logger.Info("sendRotationAbtToExpireTimer on Continue started..");
            }
            if (bkgDigestionProcedureTimer != null)
            {
                bkgDigestionProcedureTimer.Start();
                logger.Info("BackgroundDigestionProcedure started..");
            }

            //UAT-3137
            if (sendRequiredRotationCategoryBeforeGoingToBeRequiredTimer != null)
            {
                sendRequiredRotationCategoryBeforeGoingToBeRequiredTimer.Start();
                logger.Info("sendRequiredRotationCategoryBeforeGoingToBeRequiredTimer on Continue started..");
            }
            if (OffTimeRevokedAppointmentEmailTimer != null)
            {
                OffTimeRevokedAppointmentEmailTimer.Start();
                logger.Info("OffTimeRevokedAppointmentEmailTimer on Continue started..");
            }
            if (MissedAppointmentEmailTimer != null)
            {
                MissedAppointmentEmailTimer.Start();
                logger.Info("MissedAppointmentEmailTimer on Continue started..");
            }
            if (MissedAppointmentTimer != null)
            {
                MissedAppointmentTimer.Start();
                logger.Info("MissedAppointmentTimer on Continue started..");
            }
            if (ArchiveRotationDataTimer != null)
            {
                ArchiveRotationDataTimer.Start();
                logger.Info("ArchiveRotationDataTimer on Continue started.");
            }

            //UAT-3820
            if (receivedFromStuServFormStatusDataTimer != null)
            {
                receivedFromStuServFormStatusDataTimer.Start();
                logger.Info("Received From Student service Form status Continue started.");
            }

            //Complio TalkDesk Integration - Create Report API Call
            if (complioTalkDeskCreateReportJobTimer != null)
            {
                complioTalkDeskCreateReportJobTimer.Start();
                logger.Info("ComplioTalkDeskCreateReportJobTimer Continue started.");
            }

            //Complio TalkDesk Integration - Update Report API Call
            if (complioTalkDeskUpdateReportJobTimer != null)
            {
                complioTalkDeskUpdateReportJobTimer.Start();
                logger.Info("ComplioTalkDeskUpdateReportJobTimer Continue started.");
            }

            //Complio TalkDesk Integration - Pull Call data from Report Job API Call
            if (complioTalkDeskPullCallDataJobTimer != null)
            {
                complioTalkDeskPullCallDataJobTimer.Start();
                logger.Info("ComplioTalkDeskPullCallDataJobTimer Continue started.");
            }

            if (SendDarftOrderNotificationtoAdminTimer != null)
            {
                SendDarftOrderNotificationtoAdminTimer.Start();
                logger.Info("Send Darft Order Notification to Admin Timer Continue Started.");
            }

            if (SendInvitationPendingOrderNotificationtoApplicantTimer != null)
            {
                SendInvitationPendingOrderNotificationtoApplicantTimer.Start();
                logger.Info("Send Invitation Pending Order Notification to ApplicantTimer Continue Started.");
            }

            if (DeleteDraftOrderStatusTimer != null)
            {
                DeleteDraftOrderStatusTimer.Start();
                logger.Info("Delete Draft Order Status Timer Continue Started.");
            }

            if (ChangeOrderStatusCompleteToArchivedTimer != null)
            {
                ChangeOrderStatusCompleteToArchivedTimer.Start();
                logger.Info("Change Order Status Complete To Archived Timer Continue Started.");
            }

            if (BKgCompletedOrderToClientAdminTimer != null)
            {
                BKgCompletedOrderToClientAdminTimer.Start();
                logger.Info("BKg Completed Order To Client Admin Timer Continue Started.");
            }

            if (BKgCompletedOrderWithAttachedPDFToClientAdminTimer != null)
            {
                BKgCompletedOrderWithAttachedPDFToClientAdminTimer.Start();
                logger.Info("BKg Completed Order With Attached PDF To Client Admin Timer Continue Started.");
            }

            if (notificationForServiceFormInIPAStatusFromStudentTimer != null)
            {
                notificationForServiceFormInIPAStatusFromStudentTimer.Start();
                logger.Info("Notification For Service Form In IPA Status From Student Timer Continue Started.");
            }
        }

        protected override void OnShutdown()
        {
            if (scheduleTimer != null)
            {
                scheduleTimer.Stop();
                logger.Info("scheduleTimer on shut down stopped..");
            }

            if (scheduleNotificationTimer != null)
            {
                scheduleNotificationTimer.Stop();
                logger.Info("scheduleNotificationTimer on shut down stopped..");
            }

            if (complianceNotificationTimer != null)
            {
                complianceNotificationTimer.Stop();
                logger.Info("complianceNotificationTimer on shut down stopped..");
            }

            if (scheduleActionTimer != null)
            {
                scheduleActionTimer.Stop();
                logger.Info("complianceItemReoccurNotificationTimer on shut down stopped..");
            }

            if (mobilityInstanceTimer != null)
            {
                mobilityInstanceTimer.Stop();
                logger.Info("mobilityInstanceTimer on shut down stopped..");
            }

            if (copyPackageDataTimer != null)
            {
                copyPackageDataTimer.Stop();
                logger.Info("copyPackageDataTimer on shut down stopped..");
            }

            if (adminWidgetDataTimer != null)
            {
                adminWidgetDataTimer.Stop();
                logger.Info("adminWidgetDataTimer on shut down stopped..");
            }

            if (systemServiceTriggerTimer != null)
            {
                systemServiceTriggerTimer.Stop();
                logger.Info("systemServiceTriggerTimer on shut down stopped..");
            }

            if (nagEmailTimer != null)
            {
                nagEmailTimer.Stop();
                logger.Info("nagEmailTimer on shut down stopped..");
            }

            if (deadlineEmailTimer != null)
            {
                deadlineEmailTimer.Stop();
                logger.Info("deadlineEmailTimer on shut down stopped..");
            }

            if (scheduleTaskTimer != null)
            {
                scheduleTaskTimer.Stop();
                logger.Info("scheduleTaskTimer on shut down stopped..");
            }

            if (queueImagingTimer != null)
            {
                queueImagingTimer.Stop();
                logger.Info("queueImagingTimer on shut down stopped..");
            }

            if (categoryComplianceRqdTimer != null)
            {
                categoryComplianceRqdTimer.Stop();
                logger.Info("categoryComplianceRqdTimer on shut down stopped..");
            }

            if (requirementTimer != null)
            {
                requirementTimer.Stop();
                logger.Info("requirementTimer on shut down stopped..");
            }

            if (scheduleInvitationTimer != null)
            {
                scheduleInvitationTimer.Stop();
                logger.Info("scheduleInvitationTimer on shut down stopped..");
            }

            if (rotationTimer != null)
            {
                rotationTimer.Stop();
                logger.Info("requirementTimer on shut down stopped..");
            }

            if (incompletedOnlineOrderNotificationTimer != null)
            {
                incompletedOnlineOrderNotificationTimer.Stop();
                logger.Info("IncompletedOnlineOrderNotificationTimer on shut down stopped..");
            }

            if (manageContractExpiringItemsNotificationTimer != null)
            {
                manageContractExpiringItemsNotificationTimer.Stop();
                logger.Info("ManageContractExpiringItemsNotificationTimer on shut down stopped..");
            }

            if (manageContractExpiredItemsNotificationTimer != null)
            {
                manageContractExpiredItemsNotificationTimer.Stop();
                logger.Info("ManageContractExpiredItemsNotificationTimer on shut down stopped..");
            }

            if (markApplicantDocumentsCompleteTimer != null)
            {
                markApplicantDocumentsCompleteTimer.Stop();
                logger.Info("ApplicantDocumentsCompleteNotificationTimer on shut down stopped..");
            }

            //UAT-963
            if (ComplianceAuditDataSyncTimer != null)
            {
                ComplianceAuditDataSyncTimer.Stop();
                logger.Info("ComplianceAuditDataSyncTimer on shut down stopped..");
            }

            //UAT-2495
            if (ClientDataUploadTimer != null)
            {
                ClientDataUploadTimer.Stop();
                logger.Info("ClientDataUploadTimer on shut down stopped..");
            }

            if (ReconcillationQueueSyncTimer != null)
            {
                ReconcillationQueueSyncTimer.Stop();
                logger.Info("ReconcillationQueueSyncTimer on shut down stopped..");
            }

            if (reqCategoryComplianceRqdTimer != null)
            {
                reqCategoryComplianceRqdTimer.Stop();
                logger.Info("  reqCategoryComplianceRqdTimer on shut down stopped..");
            }
            //UAT-2305 
            if (copyComplianceDataToRequirementTimer != null)
            {
                copyComplianceDataToRequirementTimer.Stop();
                logger.Info("  copyComplianceDataToRequirementTimer on shut down stopped..");
            }
            //UAT-2414
            if (createRequirementSnapshotOnRotationEndTimer != null)
            {
                createRequirementSnapshotOnRotationEndTimer.Stop();
                logger.Info("createRequirementSnapshotOnRotationEndTimer on shut down stopped..");
            }

            //UAT-2305 
            if (requirementPkgSyncTimer != null)
            {
                requirementPkgSyncTimer.Stop();
                logger.Info("  requirementPkgSyncTimer on shut down stopped..");
            }

            //UAT-2533 
            if (requirementPkgAutoArchiveTimer != null)
            {
                requirementPkgAutoArchiveTimer.Stop();
                logger.Info("RequirementPkgAutoArchiveTimer on shut down stopped..");
            }

            //UAT-2603
            if (rotDataMovementTimer != null)
            {
                rotDataMovementTimer.Stop();
                logger.Info("  rotDataMovementTimer on shut down stopped..");
            }

            //UAT-2628
            if (ConversionMergingForFailedDocumentTimer != null)
            {
                ConversionMergingForFailedDocumentTimer.Stop();
                logger.Info("ConversionMergingForFailedDocumentTimer on shut down stopped..");
            }

            //UAT-2388 AutomaticPackageInvitation
            if (AutomaticPackageInvitationTimer != null)
            {
                AutomaticPackageInvitationTimer.Stop();
                logger.Info("AutomaticPackageInvitationTimer on shut down stopped..");
            }
            //UAT-2513 Feature for Client Admin to batch upload rotation creation
            if (BatchRotationUploadTimer != null)
            {
                BatchRotationUploadTimer.Stop();
                logger.Info("BatchRotationUploadTimer on shut down stopped..");
            }

            //UAT-3059: UpdatedApplicantRequirementsNotification
            if (UpdatedApplicantRequirementsNotificationTimer != null)
            {
                UpdatedApplicantRequirementsNotificationTimer.Stop();
                logger.Info("UpdatedApplicantRequirementsNotificationTimer on shut down stopped..");
            }

            //UAT-3112
            if (badgeFrmNotifTimer != null)
            {
                badgeFrmNotifTimer.Stop();
                logger.Info("badgeFrmNotifTimer  on shut down stopped..");
            }
            //UAT-3669
            if (alertMailForWebCCFErrorTimer != null)
            {
                alertMailForWebCCFErrorTimer.Stop();
                logger.Info("alertMailForWebCCFErrorTimer  on shut down stopped..");
            }
            //UAT-2960
            if (alumniAccessNotifTimer != null)
            {
                alumniAccessNotifTimer.Stop();
                logger.Info("alumniAccessNotifTimer  on shut down stopped..");
            }

            if (copyComplianceToComplianceTimer != null)
            {
                copyComplianceToComplianceTimer.Stop();
                logger.Info("copyComplianceToComplianceTimer  on shut down stopped..");
            }
            if (bkgCopyPackageDataTimer != null)
            {
                bkgCopyPackageDataTimer.Stop();
                logger.Info("BackgroundCopyPackgeData stopped..");
            }
            //UAT-3485
            if (sendRotationAbtToExpireTimer != null)
            {
                sendRotationAbtToExpireTimer.Stop();
                logger.Info("sendRotationAbtToExpireTimer on shut down stopped..");
            }

            if (bkgDigestionProcedureTimer != null)
            {
                bkgDigestionProcedureTimer.Stop();
                logger.Info("BackgroundDigestionProcedure stopped..");
            }

            //UAT-3137
            if (sendRequiredRotationCategoryBeforeGoingToBeRequiredTimer != null)
            {
                sendRequiredRotationCategoryBeforeGoingToBeRequiredTimer.Stop();
                logger.Info("sendRequiredRotationCategoryBeforeGoingToBeRequiredTimer on shut down stopped..");
            }

            if (OffTimeRevokedAppointmentEmailTimer != null)
            {
                OffTimeRevokedAppointmentEmailTimer.Stop();
                logger.Info("OffTimeRevokedAppointmentEmailTimer on shut down stopped..");
            }
            if (MissedAppointmentEmailTimer != null)
            {
                MissedAppointmentEmailTimer.Stop();
                logger.Info("MissedAppointmentEmailTimer on shut down stopped..");
            }
            if (MissedAppointmentTimer != null)
            {
                MissedAppointmentTimer.Stop();
                logger.Info("MissedAppointmentTimer on shut down stopped..");
            }

            //UAT-3820
            if (receivedFromStuServFormStatusDataTimer != null)
            {
                receivedFromStuServFormStatusDataTimer.Stop();
                logger.Info("Received From Student service Form status stopped...");
            }

            if (ArchiveRotationDataTimer != null)
            {
                ArchiveRotationDataTimer.Stop();
                logger.Info("ArchiveRotationDataTimer on shut down stopped.");
            }

            //Complio TalkDesk Integration - Create Report API Call
            if (complioTalkDeskCreateReportJobTimer != null)
            {
                complioTalkDeskCreateReportJobTimer.Stop();
                logger.Info("ComplioTalkDeskCreateReportJobTimer on shut down stopped.");
            }

            //Complio TalkDesk Integration - Update Report API Call
            if (complioTalkDeskUpdateReportJobTimer != null)
            {
                complioTalkDeskUpdateReportJobTimer.Stop();
                logger.Info("ComplioTalkDeskUpdateReportJobTimer on shut down stopped.");
            }

            //Complio TalkDesk Integration - Pull Call data from Report Job API Call
            if (complioTalkDeskPullCallDataJobTimer != null)
            {
                complioTalkDeskPullCallDataJobTimer.Stop();
                logger.Info("ComplioTalkDeskPullCallDataJobTimer on shut down stopped.");
            }

            if (SendDarftOrderNotificationtoAdminTimer != null)
            {
                SendDarftOrderNotificationtoAdminTimer.Stop();
                logger.Info("SendDarftOrderNotificationtoAdminTimer on shut down stopped.");
            }

            if (SendInvitationPendingOrderNotificationtoApplicantTimer != null)
            {
                SendInvitationPendingOrderNotificationtoApplicantTimer.Stop();
                logger.Info("SendInvitationPendingOrderNotificationtoApplicantTimer on shut down stopped.");
            }

            if (DeleteDraftOrderStatusTimer != null)
            {
                DeleteDraftOrderStatusTimer.Stop();
                logger.Info("DeleteDraftOrderStatusTimer on shut down stopped.");
            }

            if (ChangeOrderStatusCompleteToArchivedTimer != null)
            {
                ChangeOrderStatusCompleteToArchivedTimer.Stop();
                logger.Info("ChangeOrderStatusCompleteToArchivedTimer on shut down stopped.");
            }

            if (BKgCompletedOrderToClientAdminTimer != null)
            {
                BKgCompletedOrderToClientAdminTimer.Stop();
                logger.Info("BKgCompletedOrderToClientAdminTimer on shut down stopped.");
            }

            if (BKgCompletedOrderWithAttachedPDFToClientAdminTimer != null)
            {
                BKgCompletedOrderWithAttachedPDFToClientAdminTimer.Stop();
                logger.Info("BKgCompletedOrderWithAttachedPDFToClientAdminTimer on shut down stopped.");
            }
            //UAT-4613
            if (notificationForServiceFormInIPAStatusFromStudentTimer != null)
            {
                notificationForServiceFormInIPAStatusFromStudentTimer.Stop();
                logger.Info("notification For Service Form In IPA Status From Student Timer on shut down stopped.");
            }
        }

        private Int32 _serviceInterval = 10000;
        public Int32 ServiceInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("serviceInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["serviceInterval"]) ? ConfigurationManager.AppSettings["serviceInterval"] : _serviceInterval.ToString());
                else
                    return _serviceInterval;
            }
        }

        private Int32 _sleepTime = 50000;
        public Int32 SleepTime
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("sleepTime"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["sleepTime"]) ? ConfigurationManager.AppSettings["sleepTime"] : _sleepTime.ToString());
                else
                    return _sleepTime;
            }
        }

        private Int32 _emailChunkSize = 50;
        public Int32 EmailChunkSize
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("emailChunkSize"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["emailChunkSize"]) ? ConfigurationManager.AppSettings["emailChunkSize"] : _emailChunkSize.ToString());
                else
                    return _emailChunkSize;
            }
        }

        private Int32 _complianceServiceInterval = 86400000;
        public Int32 ComplianceServiceInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("complianceServiceInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["complianceServiceInterval"]) ? ConfigurationManager.AppSettings["complianceServiceInterval"] : _complianceServiceInterval.ToString());
                else
                    return _complianceServiceInterval;
            }
        }

        private Int32 _complianceFromHour = 0;
        public Int32 ComplianceFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("complianceFromHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["complianceFromHour"]) ? ConfigurationManager.AppSettings["complianceFromHour"] : _complianceFromHour.ToString());
                else
                    return _complianceFromHour;
            }
        }

        private Int32 _complianceFromMinute = 0;
        public Int32 ComplianceFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("complianceFromMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["complianceFromMinute"]) ? ConfigurationManager.AppSettings["complianceFromMinute"] : _complianceFromMinute.ToString());
                else
                    return _complianceFromMinute;
            }
        }

        private Int32 _complianceToHour = 24;
        public Int32 ComplianceToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("complianceToHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["complianceToHour"]) ? ConfigurationManager.AppSettings["complianceToHour"] : _complianceToHour.ToString());
                else
                    return _complianceToHour;
            }
        }

        private Int32 _complianceToMinute = 0;
        public Int32 ComplianceToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("complianceToMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["complianceToMinute"]) ? ConfigurationManager.AppSettings["complianceToMinute"] : _complianceToMinute.ToString());
                else
                    return _complianceToMinute;
            }
        }

        private Int32 _reminderServiceInterval = 86400000;
        public Int32 ReminderServiceInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("reminderServiceInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["reminderServiceInterval"]) ? ConfigurationManager.AppSettings["reminderServiceInterval"] : _reminderServiceInterval.ToString());
                else
                    return _reminderServiceInterval;
            }
        }

        private Int32 _reminderFromHour = 0;
        public Int32 ReminderFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("reminderFromHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["reminderFromHour"]) ? ConfigurationManager.AppSettings["reminderFromHour"] : _reminderFromHour.ToString());
                else
                    return _reminderFromHour;
            }
        }

        private Int32 _reminderFromMinute = 0;
        public Int32 ReminderFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("reminderFromMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["reminderFromMinute"]) ? ConfigurationManager.AppSettings["reminderFromMinute"] : _reminderFromMinute.ToString());
                else
                    return _reminderFromMinute;
            }
        }

        private Int32 _reminderToHour = 24;
        public Int32 ReminderToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("reminderToHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["reminderToHour"]) ? ConfigurationManager.AppSettings["reminderToHour"] : _reminderToHour.ToString());
                else
                    return _reminderToHour;
            }
        }

        private Int32 _reminderToMinute = 0;
        public Int32 ReminderToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("reminderToMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["reminderToMinute"]) ? ConfigurationManager.AppSettings["reminderToMinute"] : _reminderToMinute.ToString());
                else
                    return _reminderToMinute;
            }
        }

        private Int32 _scheduleActionInterval = 86400000;
        public Int32 ScheduleActionServiceInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("scheduleActionInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["scheduleActionInterval"]) ? ConfigurationManager.AppSettings["scheduleActionInterval"] : _scheduleActionInterval.ToString());
                else
                    return _scheduleActionInterval;
            }
        }

        private Int32 _scheduleActionFromHour = 0;
        public Int32 ScheduleActionFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("scheduleActionFromHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["scheduleActionFromHour"]) ? ConfigurationManager.AppSettings["scheduleActionFromHour"] : _scheduleActionFromHour.ToString());
                else
                    return _scheduleActionFromHour;
            }
        }

        private Int32 _scheduleActionFromMinute = 0;
        public Int32 ScheduleActionFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("scheduleActionMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["scheduleActionMinute"]) ? ConfigurationManager.AppSettings["scheduleActionMinute"] : _scheduleActionFromMinute.ToString());
                else
                    return _scheduleActionFromMinute;
            }
        }

        private Int32 _scheduleActionToHour = 24;
        public Int32 ScheduleActionToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("scheduleActionToHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["scheduleActionToHour"]) ? ConfigurationManager.AppSettings["scheduleActionToHour"] : _scheduleActionToHour.ToString());
                else
                    return _scheduleActionToHour;
            }
        }

        private Int32 _scheduleActionToMinute = 0;
        public Int32 ScheduleActionToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("scheduleActionToMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["scheduleActionToMinute"]) ? ConfigurationManager.AppSettings["scheduleActionToMinute"] : _scheduleActionToMinute.ToString());
                else
                    return _scheduleActionToMinute;
            }
        }

        private Int32 _mobilityInstanceServiceInterval = 86400000;
        public Int32 MobilityInstanceServiceInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("mobilityInstanceInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["mobilityInstanceInterval"]) ? ConfigurationManager.AppSettings["mobilityInstanceInterval"] : _mobilityInstanceServiceInterval.ToString());
                else
                    return _mobilityInstanceServiceInterval;
            }
        }

        private Int32 _nagEmailInterval = 86400000;
        public Int32 NagEmailInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("nagEmailInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["nagEmailInterval"]) ? ConfigurationManager.AppSettings["nagEmailInterval"] : _nagEmailInterval.ToString());
                else
                    return _nagEmailInterval;
            }
        }

        private Int32 _queueImaging = 86400000;
        public Int32 QueueImagingInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("queueImagingInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["queueImagingInterval"]) ? ConfigurationManager.AppSettings["queueImagingInterval"] : _queueImaging.ToString());
                else
                    return _queueImaging;
            }
        }

        private Int32 _deadlineEmailInterval = 86400000;
        public Int32 DeadlineEmailInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("deadlineEmailInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["deadlineEmailInterval"]) ? ConfigurationManager.AppSettings["deadlineEmailInterval"] : _deadlineEmailInterval.ToString());
                else
                    return _deadlineEmailInterval;
            }
        }

        private Int32 _mobilityInstanceFromHour = 0;
        public Int32 MobilityInstanceFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("mobilityInstanceFromHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["mobilityInstanceFromHour"]) ? ConfigurationManager.AppSettings["mobilityInstanceFromHour"] : _mobilityInstanceFromHour.ToString());
                else
                    return _mobilityInstanceFromHour;
            }
        }

        private Int32 _mobilityInstanceFromMinute = 0;
        public Int32 MobilityInstanceFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("mobilityInstanceFromMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["mobilityInstanceFromMinute"]) ? ConfigurationManager.AppSettings["mobilityInstanceFromMinute"] : _mobilityInstanceFromMinute.ToString());
                else
                    return _mobilityInstanceFromMinute;
            }
        }

        private Int32 _mobilityInstanceToHour = 24;
        public Int32 MobilityInstanceToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("mobilityInstanceToHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["mobilityInstanceToHour"]) ? ConfigurationManager.AppSettings["mobilityInstanceToHour"] : _mobilityInstanceToHour.ToString());
                else
                    return _mobilityInstanceToHour;
            }
        }

        private Int32 _mobilityInstanceToMinute = 0;
        public Int32 MobilityInstanceToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("mobilityInstanceToMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["mobilityInstanceToMinute"]) ? ConfigurationManager.AppSettings["mobilityInstanceToMinute"] : _mobilityInstanceToMinute.ToString());
                else
                    return _mobilityInstanceToMinute;
            }
        }

        private Int32 _copyPackageDataServiceInterval = 86400000;
        public Int32 CopyPackageDataServiceInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("copyPackageDataInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["copyPackageDataInterval"]) ? ConfigurationManager.AppSettings["copyPackageDataInterval"] : _copyPackageDataServiceInterval.ToString());
                else
                    return _copyPackageDataServiceInterval;
            }
        }

        private Int32 _copyPackageDataFromHour = 0;
        public Int32 CopyPackageDataFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("copyPackageDataFromHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["copyPackageDataFromHour"]) ? ConfigurationManager.AppSettings["copyPackageDataFromHour"] : _copyPackageDataFromHour.ToString());
                else
                    return _copyPackageDataFromHour;
            }
        }

        private Int32 _copyPackageDataFromMinute = 0;
        public Int32 CopyPackageDataFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("copyPackageDataFromMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["copyPackageDataFromMinute"]) ? ConfigurationManager.AppSettings["copyPackageDataFromMinute"] : _copyPackageDataFromMinute.ToString());
                else
                    return _copyPackageDataFromMinute;
            }
        }

        private Int32 _copyPackageDataToHour = 24;
        public Int32 CopyPackageDataToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("copyPackageDataToHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["copyPackageDataToHour"]) ? ConfigurationManager.AppSettings["copyPackageDataToHour"] : _copyPackageDataToHour.ToString());
                else
                    return _copyPackageDataToHour;
            }
        }

        private Int32 _copyPackageDataToMinute = 0;
        public Int32 CopyPackageDataToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("copyPackageDataToMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["copyPackageDataToMinute"]) ? ConfigurationManager.AppSettings["copyPackageDataToMinute"] : _copyPackageDataToMinute.ToString());
                else
                    return _copyPackageDataToMinute;
            }
        }

        private Int32 _adminWidgetServiceInterval = 86400000;
        public Int32 AdminWidgetServiceInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("adminWidgetInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["adminWidgetInterval"]) ? ConfigurationManager.AppSettings["adminWidgetInterval"] : _adminWidgetServiceInterval.ToString());
                else
                    return _adminWidgetServiceInterval;
            }
        }

        private Int32 _adminWidgetFromHour = 0;
        public Int32 AdminWidgetFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("adminWidgetFromHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["adminWidgetFromHour"]) ? ConfigurationManager.AppSettings["adminWidgetFromHour"] : _adminWidgetFromHour.ToString());
                else
                    return _adminWidgetFromHour;
            }
        }

        private Int32 _adminWidgetFromMinute = 0;
        public Int32 AdminWidgetFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("adminWidgetFromMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["adminWidgetFromMinute"]) ? ConfigurationManager.AppSettings["adminWidgetFromMinute"] : _adminWidgetFromMinute.ToString());
                else
                    return _adminWidgetFromMinute;
            }
        }

        private Int32 _adminWidgetToHour = 24;
        public Int32 AdminWidgetToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("adminWidgetToHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["adminWidgetToHour"]) ? ConfigurationManager.AppSettings["adminWidgetToHour"] : _adminWidgetToHour.ToString());
                else
                    return _adminWidgetToHour;
            }
        }

        private Int32 _adminWidgetToMinute = 0;
        public Int32 AdminWidgetToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("adminWidgetToMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["adminWidgetToMinute"]) ? ConfigurationManager.AppSettings["adminWidgetToMinute"] : _adminWidgetToMinute.ToString());
                else
                    return _adminWidgetToMinute;
            }
        }

        private Int32 _systemServiceTriggerInterval = 10000;
        public Int32 SystemServiceTriggerInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("systemServiceTriggerInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["systemServiceTriggerInterval"]) ? ConfigurationManager.AppSettings["systemServiceTriggerInterval"] : _systemServiceTriggerInterval.ToString());
                else
                    return _systemServiceTriggerInterval;
            }
        }

        private Int32 _systemServiceTriggerFromHour = 0;
        public Int32 SystemServiceTriggerFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("systemServiceTriggerFromHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["systemServiceTriggerFromHour"]) ? ConfigurationManager.AppSettings["systemServiceTriggerFromHour"] : _systemServiceTriggerFromHour.ToString());
                else
                    return _systemServiceTriggerFromHour;
            }
        }

        private Int32 _systemServiceTriggerFromMinute = 0;
        public Int32 SystemServiceTriggerFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("systemServiceTriggerFromMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["systemServiceTriggerFromMinute"]) ? ConfigurationManager.AppSettings["systemServiceTriggerFromMinute"] : _systemServiceTriggerFromMinute.ToString());
                else
                    return _systemServiceTriggerFromMinute;
            }
        }

        private Int32 _nagEmailFromHour = 0;
        public Int32 NagEmailFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("nagEmailFromHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["nagEmailFromHour"]) ? ConfigurationManager.AppSettings["nagEmailFromHour"] : _nagEmailFromHour.ToString());
                else
                    return _nagEmailFromHour;
            }
        }

        private Int32 _nagEmailFromMinute = 0;
        public Int32 NagEmailFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("nagEmailFromMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["nagEmailFromMinute"]) ? ConfigurationManager.AppSettings["nagEmailFromMinute"] : _nagEmailFromMinute.ToString());
                else
                    return _nagEmailFromMinute;
            }
        }

        private Int32 _nagEmailToHour = 24;
        public Int32 NagEmailToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("nagEmailToHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["nagEmailToHour"]) ? ConfigurationManager.AppSettings["nagEmailToHour"] : _nagEmailToHour.ToString());
                else
                    return _nagEmailToHour;
            }
        }

        private Int32 _nagEmailToMinute = 0;
        public Int32 NagEmailToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("nagEmailToMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["nagEmailToMinute"]) ? ConfigurationManager.AppSettings["nagEmailToMinute"] : _nagEmailToMinute.ToString());
                else
                    return _nagEmailToMinute;
            }
        }

        private Int32 _queueImagingFromHour = 0;
        public Int32 QueueImagingFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("queueImagingFromHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["queueImagingFromHour"]) ? ConfigurationManager.AppSettings["queueImagingFromHour"] : _queueImagingFromHour.ToString());
                else
                    return _queueImagingFromHour;
            }
        }

        private Int32 _queueImagingFromMinute = 0;
        public Int32 QueueImagingFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("queueImagingFromMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["queueImagingFromMinute"]) ? ConfigurationManager.AppSettings["queueImagingFromMinute"] : _queueImagingFromMinute.ToString());
                else
                    return _queueImagingFromMinute;
            }
        }

        private Int32 _queueImagingToHour = 24;
        public Int32 QueueImagingToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("queueImagingToHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["queueImagingToHour"]) ? ConfigurationManager.AppSettings["queueImagingToHour"] : _queueImagingToHour.ToString());
                else
                    return _queueImagingToHour;
            }
        }

        private Int32 _queueImagingToMinute = 0;
        public Int32 QueueImagingToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("queueImagingToMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["queueImagingToMinute"]) ? ConfigurationManager.AppSettings["queueImagingToMinute"] : _queueImagingToMinute.ToString());
                else
                    return _queueImagingToMinute;
            }
        }

        private Int32 _deadlineEmailFromHour = 0;
        public Int32 DeadlineEmailFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("deadlineEmailFromHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["deadlineEmailFromHour"]) ? ConfigurationManager.AppSettings["deadlineEmailFromHour"] : _deadlineEmailFromHour.ToString());
                else
                    return _deadlineEmailFromHour;
            }
        }

        private Int32 _deadlineEmailFromMinute = 0;
        public Int32 DeadlineEmailFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("deadlineEmailFromMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["deadlineEmailFromMinute"]) ? ConfigurationManager.AppSettings["deadlineEmailFromMinute"] : _deadlineEmailFromMinute.ToString());
                else
                    return _deadlineEmailFromMinute;
            }
        }

        private Int32 _deadlineEmailToHour = 24;
        public Int32 DeadlineEmailToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("deadlineEmailToHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["deadlineEmailToHour"]) ? ConfigurationManager.AppSettings["deadlineEmailToHour"] : _deadlineEmailToHour.ToString());
                else
                    return _deadlineEmailToHour;
            }
        }

        private Int32 _deadlineEmailToMinute = 0;
        public Int32 DeadlineEmailToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("deadlineEmailToMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["deadlineEmailToMinute"]) ? ConfigurationManager.AppSettings["deadlineEmailToMinute"] : _deadlineEmailToMinute.ToString());
                else
                    return _deadlineEmailToMinute;
            }
        }

        private Int32 _systemServiceTriggerToHour = 24;
        public Int32 SystemServiceTriggerToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("systemServiceTriggerToHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["systemServiceTriggerToHour"]) ? ConfigurationManager.AppSettings["systemServiceTriggerToHour"] : _systemServiceTriggerToHour.ToString());
                else
                    return _systemServiceTriggerToHour;
            }
        }

        private Int32 _systemServiceTriggerToMinute = 0;
        public Int32 SystemServiceTriggerToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("systemServiceTriggerToMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["systemServiceTriggerToMinute"]) ? ConfigurationManager.AppSettings["systemServiceTriggerToMinute"] : _systemServiceTriggerToMinute.ToString());
                else
                    return _systemServiceTriggerToMinute;
            }
        }

        private Int32 _scheduleTaskServiceInterval = 86400000;
        public Int32 ScheduleTaskServiceInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("scheduleTaskInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["scheduleTaskInterval"]) ? ConfigurationManager.AppSettings["scheduleTaskInterval"] : _scheduleTaskServiceInterval.ToString());
                else
                    return _scheduleTaskServiceInterval;
            }
        }

        private Int32 _scheduleTaskFromHour = 0;
        public Int32 ScheduleTaskFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("scheduleTaskFromHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["scheduleTaskFromHour"]) ? ConfigurationManager.AppSettings["scheduleTaskFromHour"] : _scheduleTaskFromHour.ToString());
                else
                    return _scheduleTaskFromHour;
            }
        }

        private Int32 _scheduleTaskFromMinute = 0;
        public Int32 ScheduleTaskFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("scheduleTaskFromMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["scheduleTaskFromMinute"]) ? ConfigurationManager.AppSettings["scheduleTaskFromMinute"] : _scheduleTaskFromMinute.ToString());
                else
                    return _scheduleTaskFromMinute;
            }
        }

        private Int32 _scheduleTaskToHour = 24;
        public Int32 ScheduleTaskToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("scheduleTaskToHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["scheduleTaskToHour"]) ? ConfigurationManager.AppSettings["scheduleTaskToHour"] : _scheduleTaskToHour.ToString());
                else
                    return _scheduleTaskToHour;
            }
        }

        private Int32 _scheduleTaskToMinute = 0;
        public Int32 ScheduleTaskToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("scheduleTaskToMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["scheduleTaskToMinute"]) ? ConfigurationManager.AppSettings["scheduleTaskToMinute"] : _scheduleTaskToMinute.ToString());
                else
                    return _scheduleTaskToMinute;
            }
        }

        private Int32 _categoryComplianceRqdServiceInterval = 86400000;
        public Int32 CategoryComplianceRqdServiceInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("CategoryComplianceRqdInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["CategoryComplianceRqdInterval"]) ? ConfigurationManager.AppSettings["CategoryComplianceRqdInterval"] : _categoryComplianceRqdServiceInterval.ToString());
                else
                    return _categoryComplianceRqdServiceInterval;
            }
        }

        private Int32 _CategoryComplianceRqdFromHour = 0;
        public Int32 CategoryComplianceRqdFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("CategoryComplianceRqdFromHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["CategoryComplianceRqdFromHour"]) ? ConfigurationManager.AppSettings["CategoryComplianceRqdFromHour"] : _CategoryComplianceRqdFromHour.ToString());
                else
                    return _CategoryComplianceRqdFromHour;
            }
        }

        private Int32 _CategoryComplianceRqdFromMinute = 0;
        public Int32 CategoryComplianceRqdFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("CategoryComplianceRqdFromMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["CategoryComplianceRqdFromMinute"]) ? ConfigurationManager.AppSettings["CategoryComplianceRqdFromMinute"] : _CategoryComplianceRqdFromMinute.ToString());
                else
                    return _CategoryComplianceRqdFromMinute;
            }
        }

        private Int32 _CategoryComplianceRqdToHour = 24;
        public Int32 CategoryComplianceRqdToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("CategoryComplianceRqdToHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["CategoryComplianceRqdToHour"]) ? ConfigurationManager.AppSettings["CategoryComplianceRqdToHour"] : _CategoryComplianceRqdToHour.ToString());
                else
                    return _CategoryComplianceRqdToHour;
            }
        }

        private Int32 _CategoryComplianceRqdToMinute = 0;
        public Int32 CategoryComplianceRqdToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("CategoryComplianceRqdToMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["CategoryComplianceRqdToMinute"]) ? ConfigurationManager.AppSettings["CategoryComplianceRqdToMinute"] : _CategoryComplianceRqdToMinute.ToString());
                else
                    return _CategoryComplianceRqdToMinute;
            }
        }

        private Int32 _RequiremenServiceInterval = 86400000;
        public Int32 RequiremenServiceInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("RequirementInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["RequirementInterval"]) ? ConfigurationManager.AppSettings["RequirementInterval"] : _RequiremenServiceInterval.ToString());
                else
                    return _RequiremenServiceInterval;
            }
        }

        private Int32 _ScheduledInvitationServiceInterval = 86400000;
        public Int32 ScheduledInvitationServiceInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ScheduledInvitationInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ScheduledInvitationInterval"]) ? ConfigurationManager.AppSettings["ScheduledInvitationInterval"] : _ScheduledInvitationServiceInterval.ToString());
                else
                    return _ScheduledInvitationServiceInterval;
            }
        }

        private Int32 _RotationServiceInterval = 86400000;
        public Int32 RotationServiceInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("RotationInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["RotationInterval"]) ? ConfigurationManager.AppSettings["RotationInterval"] : _RequiremenServiceInterval.ToString());
                else
                    return _RotationServiceInterval;
            }
        }

        private Int32 _RequirementFromHour = 0;
        public Int32 RequirementFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("RequirementFromHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["RequirementFromHour"]) ? ConfigurationManager.AppSettings["RequirementFromHour"] : _RequirementFromHour.ToString());
                else
                    return _RequirementFromHour;
            }
        }

        private Int32 _RequirementFromMinute = 0;
        public Int32 RequirementFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("RequirementFromMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["RequirementFromMinute"]) ? ConfigurationManager.AppSettings["RequirementFromMinute"] : _RequirementFromMinute.ToString());
                else
                    return _RequirementFromMinute;
            }
        }

        private Int32 _RequirementToHour = 24;
        public Int32 RequirementToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("RequirementToHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["RequirementToHour"]) ? ConfigurationManager.AppSettings["RequirementToHour"] : _RequirementToHour.ToString());
                else
                    return _RequirementToHour;
            }
        }

        private Int32 _RequirementToMinute = 0;
        public Int32 RequirementToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("RequirementToMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["RequirementToMinute"]) ? ConfigurationManager.AppSettings["RequirementToMinute"] : _RequirementToMinute.ToString());
                else
                    return _RequirementToMinute;
            }
        }

        #region UAT-1415 Scheduled Invitation Service
        private Int32 _ScheduledInvitationFromHour = 0;
        public Int32 ScheduledInvitationFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ScheduledInvitationFromHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ScheduledInvitationFromHour"]) ? ConfigurationManager.AppSettings["ScheduledInvitationFromHour"] : _ScheduledInvitationFromHour.ToString());
                else
                    return _ScheduledInvitationFromHour;
            }
        }

        private Int32 _ScheduledInvitationFromMinute = 0;
        public Int32 ScheduledInvitationFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ScheduledInvitationFromMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ScheduledInvitationFromMinute"]) ? ConfigurationManager.AppSettings["ScheduledInvitationFromMinute"] : _ScheduledInvitationFromMinute.ToString());
                else
                    return _ScheduledInvitationFromMinute;
            }
        }


        private Int32 _ScheduledInvitationToHour = 24;
        public Int32 ScheduledInvitationToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ScheduledInvitationToHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ScheduledInvitationToHour"]) ? ConfigurationManager.AppSettings["ScheduledInvitationToHour"] : _ScheduledInvitationToHour.ToString());
                else
                    return _ScheduledInvitationToHour;
            }
        }

        private Int32 _ScheduledInvitationToMinute = 0;
        public Int32 ScheduledInvitationToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ScheduledInvitationToMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ScheduledInvitationToMinute"]) ? ConfigurationManager.AppSettings["ScheduledInvitationToMinute"] : _ScheduledInvitationToMinute.ToString());
                else
                    return _ScheduledInvitationToMinute;
            }
        }
        #endregion

        private Int32 _RotationFromHour = 0;
        public Int32 RotationFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("RotationFromHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["RotationFromHour"]) ? ConfigurationManager.AppSettings["RotationFromHour"] : _RotationFromHour.ToString());
                else
                    return _RotationFromHour;
            }
        }

        private Int32 _RotationFromMinute = 0;
        public Int32 RotationFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("RotationFromMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["RotationFromMinute"]) ? ConfigurationManager.AppSettings["RotationFromMinute"] : _RequirementFromMinute.ToString());
                else
                    return _RotationFromMinute;
            }
        }

        private Int32 _RotationToHour = 24;
        public Int32 RotationToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("RotationToHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["RotationToHour"]) ? ConfigurationManager.AppSettings["RotationToHour"] : _RequirementToHour.ToString());
                else
                    return _RotationToHour;
            }
        }

        private Int32 _RotationToMinute = 0;
        public Int32 RotationToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("RotationToMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["RotationToMinute"]) ? ConfigurationManager.AppSettings["RotationToMinute"] : _RotationToMinute.ToString());
                else
                    return _RotationToMinute;
            }
        }

        private Int32 _IncompletedOnlineOrderNotificationFromHour = 0;
        public Int32 IncompletedOnlineOrderNotificationFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("IncompletedOnlineOrderNotificationFromHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["IncompletedOnlineOrderNotificationFromHour"]) ? ConfigurationManager.AppSettings["IncompletedOnlineOrderNotificationFromHour"] : _IncompletedOnlineOrderNotificationFromHour.ToString());
                else
                    return _IncompletedOnlineOrderNotificationFromHour;
            }
        }

        private Int32 _IncompletedOnlineOrderNotificationFromMinute = 0;
        public Int32 IncompletedOnlineOrderNotificationFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("IncompletedOnlineOrderNotificationFromMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["IncompletedOnlineOrderNotificationFromMinute"]) ? ConfigurationManager.AppSettings["IncompletedOnlineOrderNotificationFromMinute"] : _IncompletedOnlineOrderNotificationFromMinute.ToString());
                else
                    return _IncompletedOnlineOrderNotificationFromMinute;
            }
        }

        private Int32 _IncompletedOnlineOrderNotificationToHour = 24;
        public Int32 IncompletedOnlineOrderNotificationToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("IncompletedOnlineOrderNotificationToHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["IncompletedOnlineOrderNotificationToHour"]) ? ConfigurationManager.AppSettings["IncompletedOnlineOrderNotificationToHour"] : _IncompletedOnlineOrderNotificationToHour.ToString());
                else
                    return _IncompletedOnlineOrderNotificationToHour;
            }
        }

        private Int32 _IncompletedOnlineOrderNotificationToMinute = 0;
        public Int32 IncompletedOnlineOrderNotificationToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("IncompletedOnlineOrderNotificationToMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["IncompletedOnlineOrderNotificationToMinute"]) ? ConfigurationManager.AppSettings["IncompletedOnlineOrderNotificationToMinute"] : _IncompletedOnlineOrderNotificationToMinute.ToString());
                else
                    return _IncompletedOnlineOrderNotificationToMinute;
            }
        }



        #region ManageContractExpiringItemsNotification

        private Int32 _ManageContractExpiringItemsNotificationFromHour = 0;
        public Int32 ManageContractExpiringItemsNotificationFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ManageContractExpiringItemsNotificationFromHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ManageContractExpiringItemsNotificationFromHour"]) ? ConfigurationManager.AppSettings["ManageContractExpiringItemsNotificationFromHour"] : _ManageContractExpiringItemsNotificationFromHour.ToString());
                else
                    return _ManageContractExpiringItemsNotificationFromHour;
            }
        }

        private Int32 _ManageContractExpiringItemsNotificationFromMinute = 0;
        public Int32 ManageContractExpiringItemsNotificationFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ManageContractExpiringItemsNotificationFromMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ManageContractExpiringItemsNotificationFromMinute"]) ? ConfigurationManager.AppSettings["ManageContractExpiringItemsNotificationFromMinute"] : _ManageContractExpiringItemsNotificationFromMinute.ToString());
                else
                    return _ManageContractExpiringItemsNotificationFromMinute;
            }
        }

        private Int32 _ManageContractExpiringItemsNotificationToHour = 24;
        public Int32 ManageContractExpiringItemsNotificationToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ManageContractExpiringItemsNotificationToHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ManageContractExpiringItemsNotificationToHour"]) ? ConfigurationManager.AppSettings["ManageContractExpiringItemsNotificationToHour"] : _ManageContractExpiringItemsNotificationToHour.ToString());
                else
                    return _ManageContractExpiringItemsNotificationToHour;
            }
        }

        private Int32 _ManageContractExpiringItemsNotificationToMinute = 0;
        public Int32 ManageContractExpiringItemsNotificationToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ManageContractExpiringItemsNotificationToMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ManageContractExpiringItemsNotificationToMinute"]) ? ConfigurationManager.AppSettings["ManageContractExpiringItemsNotificationToMinute"] : _ManageContractExpiringItemsNotificationToMinute.ToString());
                else
                    return _ManageContractExpiringItemsNotificationToMinute;
            }
        }

        #endregion

        #region ManageContractExpiredItemsNotification

        private Int32 _ManageContractExpiredItemsNotificationFromHour = 0;
        public Int32 ManageContractExpiredItemsNotificationFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ManageContractExpiredItemsNotificationFromHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ManageContractExpiredItemsNotificationFromHour"]) ? ConfigurationManager.AppSettings["ManageContractExpiredItemsNotificationFromHour"] : _ManageContractExpiredItemsNotificationFromHour.ToString());
                else
                    return _ManageContractExpiredItemsNotificationFromHour;
            }
        }

        private Int32 _ManageContractExpiredItemsNotificationFromMinute = 0;
        public Int32 ManageContractExpiredItemsNotificationFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ManageContractExpiredItemsNotificationFromMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ManageContractExpiredItemsNotificationFromMinute"]) ? ConfigurationManager.AppSettings["ManageContractExpiredItemsNotificationFromMinute"] : _ManageContractExpiredItemsNotificationFromMinute.ToString());
                else
                    return _ManageContractExpiredItemsNotificationFromMinute;
            }
        }

        private Int32 _ManageContractExpiredItemsNotificationToHour = 24;
        public Int32 ManageContractExpiredItemsNotificationToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ManageContractExpiredItemsNotificationToHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ManageContractExpiredItemsNotificationToHour"]) ? ConfigurationManager.AppSettings["ManageContractExpiredItemsNotificationToHour"] : _ManageContractExpiredItemsNotificationToHour.ToString());
                else
                    return _ManageContractExpiredItemsNotificationToHour;
            }
        }

        private Int32 _ManageContractExpiredItemsNotificationToMinute = 0;
        public Int32 ManageContractExpiredItemsNotificationToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ManageContractExpiredItemsNotificationToMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ManageContractExpiredItemsNotificationToMinute"]) ? ConfigurationManager.AppSettings["ManageContractExpiredItemsNotificationToMinute"] : _ManageContractExpiredItemsNotificationToMinute.ToString());
                else
                    return _ManageContractExpiredItemsNotificationToMinute;
            }
        }

        #endregion

        #region MarkApplicantDocumentComplete
        private Int32 _MarkApplicantDocumentsCompleteFromHour = 0;
        public Int32 MarkApplicantDocumentsCompleteFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("MarkApplicantDocumentsCompleteFromHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["MarkApplicantDocumentsCompleteFromHour"]) ? ConfigurationManager.AppSettings["MarkApplicantDocumentsCompleteFromHour"] : _MarkApplicantDocumentsCompleteFromHour.ToString());
                else
                    return _MarkApplicantDocumentsCompleteFromHour;
            }
        }

        private Int32 _MarkApplicantDocumentsCompleteFromMinute = 0;
        public Int32 MarkApplicantDocumentsCompleteFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("MarkApplicantDocumentsCompleteFromMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["MarkApplicantDocumentsCompleteFromMinute"]) ? ConfigurationManager.AppSettings["MarkApplicantDocumentsCompleteFromMinute"] : _MarkApplicantDocumentsCompleteFromMinute.ToString());
                else
                    return _MarkApplicantDocumentsCompleteFromMinute;
            }
        }

        private Int32 _MarkApplicantDocumentsCompleteToHour = 24;
        public Int32 MarkApplicantDocumentsCompleteToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("MarkApplicantDocumentsCompleteToHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["MarkApplicantDocumentsCompleteToHour"]) ? ConfigurationManager.AppSettings["MarkApplicantDocumentsCompleteToHour"] : _MarkApplicantDocumentsCompleteToHour.ToString());
                else
                    return _MarkApplicantDocumentsCompleteToHour;
            }
        }

        private Int32 _MarkApplicantDocumentsCompleteToMinute = 0;
        public Int32 MarkApplicantDocumentsCompleteToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("MarkApplicantDocumentsCompleteToMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["MarkApplicantDocumentsCompleteToMinute"]) ? ConfigurationManager.AppSettings["MarkApplicantDocumentsCompleteToMinute"] : _MarkApplicantDocumentsCompleteToMinute.ToString());
                else
                    return _MarkApplicantDocumentsCompleteToMinute;
            }
        }
        #endregion

        #region UAT-963:Compliance Audit Data Synchronise
        private Int32 _ComplianceAuditDataSyncFromHour = 0;
        public Int32 ComplianceAuditDataSyncFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("AuditDataSyncInvitationFromHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["AuditDataSyncInvitationFromHour"]) ? ConfigurationManager.AppSettings["AuditDataSyncInvitationFromHour"] : _ComplianceAuditDataSyncFromHour.ToString());
                else
                    return _ComplianceAuditDataSyncFromHour;
            }
        }

        private Int32 _ComplianceAuditDataSyncFromMinute = 0;
        public Int32 ComplianceAuditDataSyncFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("AuditDataSyncInvitationFromMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["AuditDataSyncInvitationFromMinute"]) ? ConfigurationManager.AppSettings["AuditDataSyncInvitationFromMinute"] : _ComplianceAuditDataSyncFromMinute.ToString());
                else
                    return _ComplianceAuditDataSyncFromMinute;
            }
        }

        private Int32 _ComplianceAuditDataSyncToHour = 24;
        public Int32 ComplianceAuditDataSyncToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("AuditDataSyncInvitationToHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["AuditDataSyncInvitationToHour"]) ? ConfigurationManager.AppSettings["AuditDataSyncInvitationToHour"] : _ComplianceAuditDataSyncToHour.ToString());
                else
                    return _ComplianceAuditDataSyncToHour;
            }
        }

        private Int32 _ComplianceAuditDataSyncToMinute = 0;
        public Int32 ComplianceAuditDataSyncToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("AuditDataSyncInvitationToMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["AuditDataSyncInvitationToMinute"]) ? ConfigurationManager.AppSettings["AuditDataSyncInvitationToMinute"] : _ComplianceAuditDataSyncToMinute.ToString());
                else
                    return _ComplianceAuditDataSyncToMinute;
            }
        }
        #endregion

        #region UAT-2495
        private Int32 _ClientDataUploadFromHour = 0;
        public Int32 ClientDataUploadFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ClientDataUploadServiceFromHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ClientDataUploadServiceFromHour"]) ? ConfigurationManager.AppSettings["ClientDataUploadServiceFromHour"] : _ClientDataUploadFromHour.ToString());
                else
                    return _ClientDataUploadFromHour;
            }
        }

        private Int32 _ClientDataUploadFromMinute = 0;
        public Int32 ClientDataUploadFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ClientDataUploadServiceFromMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ClientDataUploadServiceFromMinute"]) ? ConfigurationManager.AppSettings["ClientDataUploadServiceFromMinute"] : _ClientDataUploadFromMinute.ToString());
                else
                    return _ClientDataUploadFromMinute;
            }
        }

        private Int32 _ClientDataUploadToHour = 24;
        public Int32 ClientDataUploadToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ClientDataUploadServiceToHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ClientDataUploadServiceToHour"]) ? ConfigurationManager.AppSettings["ClientDataUploadServiceToHour"] : _ClientDataUploadToHour.ToString());
                else
                    return _ClientDataUploadToHour;
            }
        }

        private Int32 _ClientDataUploadToMinute = 0;
        public Int32 ClientDataUploadToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ClientDataUploadServiceToMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ClientDataUploadServiceToMinute"]) ? ConfigurationManager.AppSettings["ClientDataUploadServiceToMinute"] : _ClientDataUploadToMinute.ToString());
                else
                    return _ClientDataUploadToMinute;
            }
        }
        #endregion

        #region UAT-2628:
        private Int32 _ConversionMergingForFailedDocumentFromHour = 0;
        public Int32 ConversionMergingForFailedDocumentFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("DocumentMergingFailedNotificationFromHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["DocumentMergingFailedNotificationFromHour"]) ? ConfigurationManager.AppSettings["DocumentMergingFailedNotificationFromHour"] : _ConversionMergingForFailedDocumentFromHour.ToString());
                else
                    return _ConversionMergingForFailedDocumentFromHour;
            }
        }

        private Int32 _ConversionMergingForFailedDocumentFromMinute = 0;
        public Int32 ConversionMergingForFailedDocumentFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("DocumentMergingFailedNotificationFromMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["DocumentMergingFailedNotificationFromMinute"]) ? ConfigurationManager.AppSettings["DocumentMergingFailedNotificationFromMinute"] : _ConversionMergingForFailedDocumentFromMinute.ToString());
                else
                    return _ConversionMergingForFailedDocumentFromMinute;
            }
        }

        private Int32 _ConversionMergingForFailedDocumentToHour = 24;
        public Int32 ConversionMergingForFailedDocumentToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("DocumentMergingFailedNotificationToHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["DocumentMergingFailedNotificationToHour"]) ? ConfigurationManager.AppSettings["DocumentMergingFailedNotificationToHour"] : _ConversionMergingForFailedDocumentToHour.ToString());
                else
                    return _ConversionMergingForFailedDocumentToHour;
            }
        }

        private Int32 _ConversionMergingForFailedDocumentToMinute = 0;
        public Int32 ConversionMergingForFailedDocumentToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("DocumentMergingFailedNotificationToMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["DocumentMergingFailedNotificationToMinute"]) ? ConfigurationManager.AppSettings["DocumentMergingFailedNotificationToMinute"] : _ConversionMergingForFailedDocumentToMinute.ToString());
                else
                    return _ConversionMergingForFailedDocumentToMinute;
            }
        }

        #endregion

        #region UAT-2388:AutomaticPackageInvitation
        private Int32 _AutomaticPackageInvitationFromHour = 0;
        public Int32 AutomaticPackageInvitationFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("AutomaticPackageInvitationFromHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["AutomaticPackageInvitationFromHour"]) ? ConfigurationManager.AppSettings["AutomaticPackageInvitationFromHour"] : _AutomaticPackageInvitationFromHour.ToString());
                else
                    return _AutomaticPackageInvitationFromHour;
            }
        }

        private Int32 _AutomaticPackageInvitationFromMinute = 0;
        public Int32 AutomaticPackageInvitationFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("AutomaticPackageInvitationFromMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["AutomaticPackageInvitationFromMinute"]) ? ConfigurationManager.AppSettings["AutomaticPackageInvitationFromMinute"] : _AutomaticPackageInvitationFromMinute.ToString());
                else
                    return _AutomaticPackageInvitationFromMinute;
            }
        }

        private Int32 _AutomaticPackageInvitationToHour = 24;
        public Int32 AutomaticPackageInvitationToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("AutomaticPackageInvitationToHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["AutomaticPackageInvitationToHour"]) ? ConfigurationManager.AppSettings["AutomaticPackageInvitationToHour"] : _AutomaticPackageInvitationToHour.ToString());
                else
                    return _AutomaticPackageInvitationToHour;
            }
        }

        private Int32 _AutomaticPackageInvitationToMinute = 0;
        public Int32 AutomaticPackageInvitationToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("AutomaticPackageInvitationToMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["AutomaticPackageInvitationToMinute"]) ? ConfigurationManager.AppSettings["AutomaticPackageInvitationToMinute"] : _AutomaticPackageInvitationToMinute.ToString());
                else
                    return _AutomaticPackageInvitationToMinute;
            }
        }

        #endregion

        //UpdatedApplicantRequirementsNotificationFromHour

        #region UAT-3059: UpdatedApplicantRequirementsNotification
        private Int32 _UpdatedApplicantRequirementsNotificationFromHour = 0;
        public Int32 UpdatedApplicantRequirementsNotificationFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("UpdatedApplicantRequirementsNotificationFromHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["UpdatedApplicantRequirementsNotificationFromHour"]) ? ConfigurationManager.AppSettings["UpdatedApplicantRequirementsNotificationFromHour"] : _UpdatedApplicantRequirementsNotificationFromHour.ToString());
                else
                    return _UpdatedApplicantRequirementsNotificationFromHour;
            }
        }

        private Int32 _UpdatedApplicantRequirementsNotificationFromMinute = 0;
        public Int32 UpdatedApplicantRequirementsNotificationFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("UpdatedApplicantRequirementsNotificationFromMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["UpdatedApplicantRequirementsNotificationFromMinute"]) ? ConfigurationManager.AppSettings["UpdatedApplicantRequirementsNotificationFromMinute"] : _UpdatedApplicantRequirementsNotificationFromMinute.ToString());
                else
                    return _UpdatedApplicantRequirementsNotificationFromMinute;
            }
        }

        private Int32 _UpdatedApplicantRequirementsNotificationToHour = 24;
        public Int32 UpdatedApplicantRequirementsNotificationToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("UpdatedApplicantRequirementsNotificationToHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["UpdatedApplicantRequirementsNotificationToHour"]) ? ConfigurationManager.AppSettings["UpdatedApplicantRequirementsNotificationToHour"] : _UpdatedApplicantRequirementsNotificationToHour.ToString());
                else
                    return _UpdatedApplicantRequirementsNotificationToHour;
            }
        }

        private Int32 _UpdatedApplicantRequirementsNotificationToMinute = 0;
        public Int32 UpdatedApplicantRequirementsNotificationToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("UpdatedApplicantRequirementsNotificationToMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["UpdatedApplicantRequirementsNotificationToMinute"]) ? ConfigurationManager.AppSettings["UpdatedApplicantRequirementsNotificationToMinute"] : _UpdatedApplicantRequirementsNotificationToMinute.ToString());
                else
                    return _UpdatedApplicantRequirementsNotificationToMinute;
            }
        }

        #endregion

        #region UAT-2513 Feature for Client Admin to batch upload rotation creation
        private Int32 _BatchRotationUploadFromHour = 0;
        public Int32 BatchRotationUploadFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("BatchRotationUploadFromHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["BatchRotationUploadFromHour"]) ? ConfigurationManager.AppSettings["BatchRotationUploadFromHour"] : _BatchRotationUploadFromHour.ToString());
                else
                    return _BatchRotationUploadFromHour;
            }
        }

        private Int32 _BatchRotationUploadFromMinute = 0;
        public Int32 BatchRotationUploadFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("BatchRotationUploadFromMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["BatchRotationUploadFromMinute"]) ? ConfigurationManager.AppSettings["BatchRotationUploadFromMinute"] : _BatchRotationUploadFromMinute.ToString());
                else
                    return _BatchRotationUploadFromMinute;
            }
        }

        private Int32 _BatchRotationUploadToHour = 24;
        public Int32 BatchRotationUploadToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("BatchRotationUploadToHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["BatchRotationUploadToHour"]) ? ConfigurationManager.AppSettings["BatchRotationUploadToHour"] : _BatchRotationUploadToHour.ToString());
                else
                    return _BatchRotationUploadToHour;
            }
        }

        private Int32 _BatchRotationUploadToMinute = 0;
        public Int32 BatchRotationUploadToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("BatchRotationUploadToMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["BatchRotationUploadToMinute"]) ? ConfigurationManager.AppSettings["BatchRotationUploadToMinute"] : _BatchRotationUploadToMinute.ToString());
                else
                    return _BatchRotationUploadToMinute;
            }
        }
        #endregion

        private Int32 _ReconcillationQueueSyncFromHour = 0;
        public Int32 ReconcillationQueueSyncFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ReconcillationQueueSyncFromHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ReconcillationQueueSyncFromHour"]) ? ConfigurationManager.AppSettings["ReconcillationQueueSyncFromHour"] : _ComplianceAuditDataSyncFromHour.ToString());
                else
                    return _ComplianceAuditDataSyncFromHour;
            }
        }

        private Int32 _ReconcillationQueueSyncFromMinute = 0;
        public Int32 ReconcillationQueueSyncFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ReconcillationQueueSyncFromMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ReconcillationQueueSyncFromMinute"]) ? ConfigurationManager.AppSettings["ReconcillationQueueSyncFromMinute"] : _ComplianceAuditDataSyncFromMinute.ToString());
                else
                    return _ComplianceAuditDataSyncFromMinute;
            }
        }

        private Int32 _ReconcillationQueueSyncToHour = 24;
        public Int32 ReconcillationQueueSyncToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ReconcillationQueueSyncToHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ReconcillationQueueSyncToHour"]) ? ConfigurationManager.AppSettings["ReconcillationQueueSyncToHour"] : _ComplianceAuditDataSyncToHour.ToString());
                else
                    return _ReconcillationQueueSyncToHour;
            }
        }

        private Int32 _ReconcillationQueueSyncToMinute = 0;
        public Int32 ReconcillationQueueSyncToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ReconcillationQueueSyncToMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ReconcillationQueueSyncToMinute"]) ? ConfigurationManager.AppSettings["ReconcillationQueueSyncToMinute"] : _ComplianceAuditDataSyncToMinute.ToString());
                else
                    return _ReconcillationQueueSyncToMinute;
            }
        }

        private Int32 _maxRetryCount = 3;
        public Int32 MaxRetryCount
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("MaxRetryCount"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["MaxRetryCount"]) ? ConfigurationManager.AppSettings["MaxRetryCount"] : _maxRetryCount.ToString());
                else
                    return _maxRetryCount;
            }
        }

        private int _emailMaxParallelThreadCount = 1;
        public int EmailMaxParallelThreadCount
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("EmailMaxParallelThreadCount"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["EmailMaxParallelThreadCount"]) ? ConfigurationManager.AppSettings["EmailMaxParallelThreadCount"] : _emailMaxParallelThreadCount.ToString());
                else
                    return _emailMaxParallelThreadCount;
            }
        }

        private Int32 _IncompletedOnlineOrderNotificationServiceInterval = 86400000;
        public Int32 IncompletedOnlineOrderNotificationServiceInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("IncompletedOnlineOrderNotificationInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["IncompletedOnlineOrderNotificationInterval"]) ? ConfigurationManager.AppSettings["IncompletedOnlineOrderNotificationInterval"] : _IncompletedOnlineOrderNotificationServiceInterval.ToString());
                else
                    return _IncompletedOnlineOrderNotificationServiceInterval;
            }
        }

        private Int32 _ManageContractExpiringItemsNotificationServiceInterval = 86400000;
        public Int32 ManageContractExpiringItemsNotificationServiceInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ManageContractExpiringItemsNotificationInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ManageContractExpiringItemsNotificationInterval"]) ? ConfigurationManager.AppSettings["ManageContractExpiringItemsNotificationInterval"] : _ManageContractExpiringItemsNotificationServiceInterval.ToString());
                else
                    return _ManageContractExpiringItemsNotificationServiceInterval;
            }
        }

        private Int32 _ManageContractExpiredItemsNotificationServiceInterval = 86400000;
        public Int32 ManageContractExpiredItemsNotificationServiceInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ManageContractExpiredItemsNotificationInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ManageContractExpiredItemsNotificationInterval"]) ? ConfigurationManager.AppSettings["ManageContractExpiredItemsNotificationInterval"] : _ManageContractExpiredItemsNotificationServiceInterval.ToString());
                else
                    return _ManageContractExpiredItemsNotificationServiceInterval;
            }
        }


        private Int32 _MarkApplicantDocumentsCompleteServiceInterval = 86400000;
        public Int32 MarkApplicantDocumentsCompleteServiceInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("MarkApplicantDocumentsCompleteInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["MarkApplicantDocumentsCompleteInterval"]) ? ConfigurationManager.AppSettings["MarkApplicantDocumentsCompleteInterval"] : _MarkApplicantDocumentsCompleteServiceInterval.ToString());
                else
                    return _MarkApplicantDocumentsCompleteServiceInterval;
            }
        }

        //UAT-2628:
        private Int32 _ConversionMergingFailedDocumentServiceInterval = 86400000;
        public Int32 ConversionMergingFailedDocumentServiceInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("DocumentMergingFailedNotificationInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["DocumentMergingFailedNotificationInterval"]) ? ConfigurationManager.AppSettings["DocumentMergingFailedNotificationInterval"] : _ConversionMergingFailedDocumentServiceInterval.ToString());
                else
                    return _ConversionMergingFailedDocumentServiceInterval;
            }
        }
        //UAT-2388: AutomaticPackageInvitation
        private Int32 _AutomaticPackageInvitationInterval = 86400000;
        public Int32 AutomaticPackageInvitationInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("AutomaticPackageInvitationInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["AutomaticPackageInvitationInterval"]) ? ConfigurationManager.AppSettings["AutomaticPackageInvitationInterval"] : _AutomaticPackageInvitationInterval.ToString());
                else
                    return _AutomaticPackageInvitationInterval;
            }
        }
        //UAT-3059: UpdatedApplicantRequirementsNotification
        private Int32 _UpdatedApplicantRequirementsNotificationInterval = 86400000;
        public Int32 UpdatedApplicantRequirementsNotificationInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("UpdatedApplicantRequirementsNotificationInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["UpdatedApplicantRequirementsNotificationInterval"]) ? ConfigurationManager.AppSettings["UpdatedApplicantRequirementsNotificationInterval"] : _UpdatedApplicantRequirementsNotificationInterval.ToString());
                else
                    return _UpdatedApplicantRequirementsNotificationInterval;
            }
        }
        //UAT-2513 : Feature for Client Admin to batch upload rotation creation
        private Int32 _BatchRotationUploadInterval = 86400000;
        public Int32 BatchRotationUploadInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("BatchRotationUploadInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["BatchRotationUploadInterval"]) ? ConfigurationManager.AppSettings["BatchRotationUploadInterval"] : _BatchRotationUploadInterval.ToString());
                else
                    return _BatchRotationUploadInterval;
            }
        }

        //UAT-963
        private Int32 _ComplianceAuditDataSyncInterval = 86400000;
        public Int32 ComplianceAuditDataSyncInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("AuditDataSyncInvitationInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["AuditDataSyncInvitationInterval"]) ? ConfigurationManager.AppSettings["AuditDataSyncInvitationInterval"] : _ComplianceAuditDataSyncInterval.ToString());
                else
                    return _ComplianceAuditDataSyncInterval;
            }
        }

        //UAT-2495
        private Int32 _ClientDataUploadInterval = 86400000;
        public Int32 ClientDataUploadInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ClientDataUploadServiceInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ClientDataUploadServiceInterval"]) ? ConfigurationManager.AppSettings["ClientDataUploadServiceInterval"] : _ClientDataUploadInterval.ToString());
                else
                    return _ClientDataUploadInterval;
            }
        }

        private Int32 _ReconcillationQueueSyncInterval = 86400000;
        public Int32 ReconcillationQueueSyncInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ReconcillationQueueSyncInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ReconcillationQueueSyncInterval"]) ? ConfigurationManager.AppSettings["ReconcillationQueueSyncInterval"] : _ReconcillationQueueSyncInterval.ToString());
                else
                    return _ReconcillationQueueSyncInterval;
            }
        }


        private Int32 _reqScheduleActionInterval = 86400000;
        public Int32 ReqScheduleActionServiceInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ReqScheduleActionInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ReqScheduleActionInterval"]) ? ConfigurationManager.AppSettings["ReqScheduleActionInterval"] : _reqScheduleActionInterval.ToString());
                else
                    return _reqScheduleActionInterval;
            }
        }

        private Int32 _reqScheduleActionFromHour = 0;
        public Int32 ReqScheduleActionFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ReqScheduleActionFromHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ReqScheduleActionFromHour"]) ? ConfigurationManager.AppSettings["ReqScheduleActionFromHour"] : _reqScheduleActionFromHour.ToString());
                else
                    return _reqScheduleActionFromHour;
            }
        }

        private Int32 _reqScheduleActionFromMinute = 0;
        public Int32 ReqScheduleActionFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ReqScheduleActionMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ReqScheduleActionMinute"]) ? ConfigurationManager.AppSettings["ReqScheduleActionMinute"] : _reqScheduleActionFromMinute.ToString());
                else
                    return _reqScheduleActionFromMinute;
            }
        }

        private Int32 _reqScheduleActionToHour = 24;
        public Int32 ReqScheduleActionToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ReqScheduleActionToHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ReqScheduleActionToHour"]) ? ConfigurationManager.AppSettings["ReqScheduleActionToHour"] : _reqScheduleActionToHour.ToString());
                else
                    return _reqScheduleActionToHour;
            }
        }

        private Int32 _reqScheduleActionToMinute = 0;
        public Int32 ReqScheduleActionToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ReqScheduleActionToMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ReqScheduleActionToMinute"]) ? ConfigurationManager.AppSettings["ReqScheduleActionToMinute"] : _reqScheduleActionToMinute.ToString());
                else
                    return _reqScheduleActionToMinute;
            }
        }

        private Int32 _reqcategoryComplianceRqdServiceInterval = 86400000;
        public Int32 ReqCategoryComplianceRqdServiceInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ReqCategoryComplianceRqdInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ReqCategoryComplianceRqdInterval"]) ? ConfigurationManager.AppSettings["ReqCategoryComplianceRqdInterval"] : _reqcategoryComplianceRqdServiceInterval.ToString());
                else
                    return _reqcategoryComplianceRqdServiceInterval;
            }
        }

        private Int32 _ReqCategoryComplianceRqdFromHour = 0;
        public Int32 ReqCategoryComplianceRqdFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ReqCategoryComplianceRqdFromHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ReqCategoryComplianceRqdFromHour"]) ? ConfigurationManager.AppSettings["ReqCategoryComplianceRqdFromHour"] : _ReqCategoryComplianceRqdFromHour.ToString());
                else
                    return _ReqCategoryComplianceRqdFromHour;
            }
        }

        private Int32 _ReqCategoryComplianceRqdFromMinute = 0;
        public Int32 ReqCategoryComplianceRqdFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ReqCategoryComplianceRqdFromMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ReqCategoryComplianceRqdFromMinute"]) ? ConfigurationManager.AppSettings["ReqCategoryComplianceRqdFromMinute"] : _ReqCategoryComplianceRqdFromMinute.ToString());
                else
                    return _ReqCategoryComplianceRqdFromMinute;
            }
        }

        private Int32 _ReqCategoryComplianceRqdToHour = 24;
        public Int32 ReqCategoryComplianceRqdToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ReqCategoryComplianceRqdToHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ReqCategoryComplianceRqdToHour"]) ? ConfigurationManager.AppSettings["ReqCategoryComplianceRqdToHour"] : _ReqCategoryComplianceRqdToHour.ToString());
                else
                    return _ReqCategoryComplianceRqdToHour;
            }
        }

        private Int32 _ReqCategoryComplianceRqdToMinute = 0;
        public Int32 ReqCategoryComplianceRqdToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ReqCategoryComplianceRqdToMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ReqCategoryComplianceRqdToMinute"]) ? ConfigurationManager.AppSettings["ReqCategoryComplianceRqdToMinute"] : _ReqCategoryComplianceRqdToMinute.ToString());
                else
                    return _ReqCategoryComplianceRqdToMinute;
            }
        }

        #region Copy Compliance Data To Requirement
        private Int32 _copyComplianceDataToRequirementInterval = 86400000;
        public Int32 CopyComplianceDataToRequirementInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("CopyComplianceDataToRequirementInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["CopyComplianceDataToRequirementInterval"]) ? ConfigurationManager.AppSettings["CopyComplianceDataToRequirementInterval"] : _copyComplianceDataToRequirementInterval.ToString());
                else
                    return _copyComplianceDataToRequirementInterval;
            }
        }

        private Int32 _copyComplianceDataToRequirementFromHour = 0;
        public Int32 CopyComplianceDataToRequirementFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("CopyComplianceDataToRequirementFromHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["CopyComplianceDataToRequirementFromHour"]) ? ConfigurationManager.AppSettings["CopyComplianceDataToRequirementFromHour"] : _copyComplianceDataToRequirementFromHour.ToString());
                else
                    return _copyComplianceDataToRequirementFromHour;
            }
        }

        private Int32 _copyComplianceDataToRequirementFromMinute = 0;
        public Int32 CopyComplianceDataToRequirementFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("CopyComplianceDataToRequirementFromMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["CopyComplianceDataToRequirementFromMinute"]) ? ConfigurationManager.AppSettings["CopyComplianceDataToRequirementFromMinute"] : _copyComplianceDataToRequirementFromMinute.ToString());
                else
                    return _copyComplianceDataToRequirementFromMinute;
            }
        }

        private Int32 _copyComplianceDataToRequirementToHour = 24;
        public Int32 CopyComplianceDataToRequirementToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("CopyComplianceDataToRequirementToHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["CopyComplianceDataToRequirementToHour"]) ? ConfigurationManager.AppSettings["CopyComplianceDataToRequirementToHour"] : _copyComplianceDataToRequirementToHour.ToString());
                else
                    return _copyComplianceDataToRequirementToHour;
            }
        }

        private Int32 _copyComplianceDataToRequirementToMinute = 0;
        public Int32 CopyComplianceDataToRequirementToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("CopyComplianceDataToRequirementToMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["CopyComplianceDataToRequirementToMinute"]) ? ConfigurationManager.AppSettings["CopyComplianceDataToRequirementToMinute"] : _copyComplianceDataToRequirementToMinute.ToString());
                else
                    return _copyComplianceDataToRequirementToMinute;
            }
        }
        #endregion

        #region Requirement Pkg Sync
        private Int32 _requirementPkgSyncInterval = 86400000;
        public Int32 RequirementPkgSyncInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("RequirementPkgSyncInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["RequirementPkgSyncInterval"]) ? ConfigurationManager.AppSettings["RequirementPkgSyncInterval"] : _requirementPkgSyncInterval.ToString());
                else
                    return _requirementPkgSyncInterval;
            }
        }

        private Int32 _requirementPkgSyncFromHour = 0;
        public Int32 RequirementPkgSyncFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("RequirementPkgSyncFromHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["RequirementPkgSyncFromHour"]) ? ConfigurationManager.AppSettings["RequirementPkgSyncFromHour"] : _requirementPkgSyncFromHour.ToString());
                else
                    return _requirementPkgSyncFromHour;
            }
        }

        private Int32 _requirementPkgSyncFromMinute = 0;
        public Int32 RequirementPkgSyncFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("RequirementPkgSyncFromMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["RequirementPkgSyncFromMinute"]) ? ConfigurationManager.AppSettings["RequirementPkgSyncFromMinute"] : _requirementPkgSyncFromMinute.ToString());
                else
                    return _requirementPkgSyncFromMinute;
            }
        }

        private Int32 _requirementPkgSyncToHour = 24;
        public Int32 RequirementPkgSyncToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("RequirementPkgSyncToHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["RequirementPkgSyncToHour"]) ? ConfigurationManager.AppSettings["RequirementPkgSyncToHour"] : _requirementPkgSyncToHour.ToString());
                else
                    return _requirementPkgSyncToHour;
            }
        }

        private Int32 _requirementPkgSyncToMinute = 0;
        public Int32 RequirementPkgSyncToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("RequirementPkgSyncToMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["RequirementPkgSyncToMinute"]) ? ConfigurationManager.AppSettings["RequirementPkgSyncToMinute"] : _requirementPkgSyncToMinute.ToString());
                else
                    return _requirementPkgSyncToMinute;
            }
        }
        #endregion

        #region Auto Archive Pkg UAT-2533
        private Int32 _RequirementPkgAutoArchiveInterval = 86400000;
        public Int32 RequirementPkgAutoArchiveInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("RequirementPkgAutoArchiveInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["RequirementPkgAutoArchiveInterval"]) ? ConfigurationManager.AppSettings["RequirementPkgAutoArchiveInterval"] : _RequirementPkgAutoArchiveInterval.ToString());
                else
                    return _RequirementPkgAutoArchiveInterval;
            }
        }

        private Int32 _RequirementPkgAutoArchiveFromHour = 0;
        public Int32 RequirementPkgAutoArchiveFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("RequirementPkgAutoArchiveFromHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["RequirementPkgAutoArchiveFromHour"]) ? ConfigurationManager.AppSettings["RequirementPkgAutoArchiveFromHour"] : _RequirementPkgAutoArchiveFromHour.ToString());
                else
                    return _RequirementPkgAutoArchiveFromHour;
            }
        }

        private Int32 _RequirementPkgAutoArchiveFromMinute = 0;
        public Int32 RequirementPkgAutoArchiveFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("RequirementPkgAutoArchiveFromMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["RequirementPkgAutoArchiveFromMinute"]) ? ConfigurationManager.AppSettings["RequirementPkgAutoArchiveFromMinute"] : _RequirementPkgAutoArchiveFromMinute.ToString());
                else
                    return _RequirementPkgAutoArchiveFromMinute;
            }
        }

        private Int32 _RequirementPkgAutoArchiveToHour = 24;
        public Int32 RequirementPkgAutoArchiveToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("RequirementPkgAutoArchiveToHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["RequirementPkgAutoArchiveToHour"]) ? ConfigurationManager.AppSettings["RequirementPkgAutoArchiveToHour"] : _RequirementPkgAutoArchiveToHour.ToString());
                else
                    return _RequirementPkgAutoArchiveToHour;
            }
        }

        private Int32 _RequirementPkgAutoArchiveToMinute = 0;
        public Int32 RequirementPkgAutoArchiveToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("RequirementPkgAutoArchiveToMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["RequirementPkgAutoArchiveToMinute"]) ? ConfigurationManager.AppSettings["RequirementPkgAutoArchiveToMinute"] : _RequirementPkgAutoArchiveToMinute.ToString());
                else
                    return _RequirementPkgAutoArchiveToMinute;
            }
        }
        #endregion

        #region UAT-2603
        private Int32 _rotationDataMovementInterval = 86400000;
        public Int32 RotationDataMovementInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("RotationDataMovementInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["RotationDataMovementInterval"]) ? ConfigurationManager.AppSettings["RotationDataMovementInterval"] : _rotationDataMovementInterval.ToString());
                else
                    return _rotationDataMovementInterval;
            }
        }

        private Int32 _rotationDataMovementFromHour = 0;
        public Int32 RotationDataMovementFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("RotationDataMovementFromHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["RotationDataMovementFromHour"]) ? ConfigurationManager.AppSettings["RotationDataMovementFromHour"] : _rotationDataMovementFromHour.ToString());
                else
                    return _rotationDataMovementFromHour;
            }
        }

        private Int32 _rotationDataMovementFromMinute = 0;
        public Int32 RotationDataMovementFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("RotationDataMovementFromMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["RotationDataMovementFromMinute"]) ? ConfigurationManager.AppSettings["RotationDataMovementFromMinute"] : _rotationDataMovementFromMinute.ToString());
                else
                    return _rotationDataMovementFromMinute;
            }
        }

        private Int32 _rotationDataMovementToHour = 24;
        public Int32 RotationDataMovementToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("RotationDataMovementToHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["RotationDataMovementToHour"]) ? ConfigurationManager.AppSettings["RotationDataMovementToHour"] : _rotationDataMovementToHour.ToString());
                else
                    return _rotationDataMovementToHour;
            }
        }

        private Int32 _rotationDataMovementToMinute = 0;
        public Int32 RotationDataMovementToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("RotationDataMovementToMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["RotationDataMovementToMinute"]) ? ConfigurationManager.AppSettings["RotationDataMovementToMinute"] : _rotationDataMovementToMinute.ToString());
                else
                    return _rotationDataMovementToMinute;
            }
        }

        #endregion

        #region UAT-3112

        private Int32 _badgeFormNotificationInterval = 86400000;
        public Int32 BadgeFormNotificationInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("BadgeFormNotificationInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["BadgeFormNotificationInterval"]) ? ConfigurationManager.AppSettings["BadgeFormNotificationInterval"] : _badgeFormNotificationInterval.ToString());
                else
                    return _badgeFormNotificationInterval;
            }
        }

        private Int32 _badgeFormNotificationFromHour = 0;
        public Int32 BadgeFormNotificationFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("BadgeFormNotificationFromHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["BadgeFormNotificationFromHour"]) ? ConfigurationManager.AppSettings["BadgeFormNotificationFromHour"] : _badgeFormNotificationFromHour.ToString());
                else
                    return _badgeFormNotificationFromHour;
            }
        }

        private Int32 _badgeFormNotificationFromMinute = 0;
        public Int32 BadgeFormNotificationFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("BadgeFormNotificationFromMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["BadgeFormNotificationFromMinute"]) ? ConfigurationManager.AppSettings["BadgeFormNotificationFromMinute"] : _badgeFormNotificationFromMinute.ToString());
                else
                    return _badgeFormNotificationFromMinute;
            }
        }

        private Int32 _badgeFormNotificationToHour = 24;
        public Int32 BadgeFormNotificationToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("BadgeFormNotificationToHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["BadgeFormNotificationToHour"]) ? ConfigurationManager.AppSettings["BadgeFormNotificationToHour"] : _badgeFormNotificationToHour.ToString());
                else
                    return _badgeFormNotificationToHour;
            }
        }

        private Int32 _badgeFormNotificationToMinute = 0;
        public Int32 BadgeFormNotificationToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("BadgeFormNotificationToMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["BadgeFormNotificationToMinute"]) ? ConfigurationManager.AppSettings["BadgeFormNotificationToMinute"] : _badgeFormNotificationToMinute.ToString());
                else
                    return _badgeFormNotificationToMinute;
            }
        }


        #endregion

        #region UAT-3669

        private Int32 _alertMailForWebCCFErrorInterval = 86400000;
        public Int32 AlertMailForWebCCFErrorInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("AlertMailForWebCCFErrorInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["AlertMailForWebCCFErrorInterval"]) ? ConfigurationManager.AppSettings["AlertMailForWebCCFErrorInterval"] : _alertMailForWebCCFErrorInterval.ToString());
                else
                    return _alertMailForWebCCFErrorInterval;
            }
        }

        private Int32 _alertMailForWebCCFErrorFromHour = 0;
        public Int32 AlertMailForWebCCFErrorFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("AlertMailForWebCCFErrorFromHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["AlertMailForWebCCFErrorFromHour"]) ? ConfigurationManager.AppSettings["AlertMailForWebCCFErrorFromHour"] : _alertMailForWebCCFErrorFromHour.ToString());
                else
                    return _alertMailForWebCCFErrorFromHour;
            }
        }

        private Int32 _alertMailForWebCCFErrorFromMinute = 0;
        public Int32 AlertMailForWebCCFErrorFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("AlertMailForWebCCFErrorFromMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["AlertMailForWebCCFErrorFromMinute"]) ? ConfigurationManager.AppSettings["AlertMailForWebCCFErrorFromMinute"] : _alertMailForWebCCFErrorFromMinute.ToString());
                else
                    return _alertMailForWebCCFErrorFromMinute;
            }
        }

        private Int32 _alertMailForWebCCFErrorToHour = 24;
        public Int32 AlertMailForWebCCFErrorToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("AlertMailForWebCCFErrorToHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["AlertMailForWebCCFErrorToHour"]) ? ConfigurationManager.AppSettings["AlertMailForWebCCFErrorToHour"] : _alertMailForWebCCFErrorToHour.ToString());
                else
                    return _alertMailForWebCCFErrorToHour;
            }
        }

        private Int32 _alertMailForWebCCFErrorToMinute = 0;
        public Int32 AlertMailForWebCCFErrorToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("AlertMailForWebCCFErrorToMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["AlertMailForWebCCFErrorToMinute"]) ? ConfigurationManager.AppSettings["AlertMailForWebCCFErrorToMinute"] : _alertMailForWebCCFErrorToMinute.ToString());
                else
                    return _alertMailForWebCCFErrorToMinute;
            }
        }


        #endregion


        #region UAT-2960

        private Int32 _alumniAccessNotificationInterval = 86400000;
        public Int32 AlumniAccessNotificationInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("AlumniAccessNotificationInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["AlumniAccessNotificationInterval"]) ? ConfigurationManager.AppSettings["AlumniAccessNotificationInterval"] : _alumniAccessNotificationInterval.ToString());
                else
                    return _alumniAccessNotificationInterval;
            }
        }

        private Int32 _alumniAccessNotificationFromHour = 0;
        public Int32 AlumniAccessNotificationFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("AlumniAccessNotificationFromHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["AlumniAccessNotificationFromHour"]) ? ConfigurationManager.AppSettings["AlumniAccessNotificationFromHour"] : _alumniAccessNotificationFromHour.ToString());
                else
                    return _alumniAccessNotificationFromHour;
            }
        }

        private Int32 _alumniAccessNotificationFromMinute = 0;
        public Int32 AlumniAccessNotificationFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("AlumniAccessNotificationFromMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["AlumniAccessNotificationFromMinute"]) ? ConfigurationManager.AppSettings["AlumniAccessNotificationFromMinute"] : _alumniAccessNotificationFromMinute.ToString());
                else
                    return _alumniAccessNotificationFromMinute;
            }
        }

        private Int32 _alumniAccessNotificationToHour = 24;
        public Int32 AlumniAccessNotificationToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("AlumniAccessNotificationToHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["AlumniAccessNotificationToHour"]) ? ConfigurationManager.AppSettings["AlumniAccessNotificationToHour"] : _alumniAccessNotificationToHour.ToString());
                else
                    return _alumniAccessNotificationToHour;
            }
        }

        private Int32 _alumniAccessNotificationToMinute = 0;
        public Int32 AlumniAccessNotificationToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("AlumniAccessNotificationToMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["AlumniAccessNotificationToMinute"]) ? ConfigurationManager.AppSettings["AlumniAccessNotificationToMinute"] : _alumniAccessNotificationToMinute.ToString());
                else
                    return _alumniAccessNotificationToMinute;
            }
        }


        //UAT-2960 Copy Compliance to compliance
        private Int32 _copyComplianceToComplianceInterval = 86400000;
        public Int32 CopyComplianceToComplianceInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("CopyComplianceToComplianceInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["CopyComplianceToComplianceInterval"]) ? ConfigurationManager.AppSettings["CopyComplianceToComplianceInterval"] : _copyComplianceToComplianceInterval.ToString());
                else
                    return _copyComplianceToComplianceInterval;
            }
        }

        private Int32 _copyComplianceToComplianceFromHour = 0;
        public Int32 CopyComplianceToComplianceFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("CopyComplianceToComplianceFromHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["CopyComplianceToComplianceFromHour"]) ? ConfigurationManager.AppSettings["CopyComplianceToComplianceFromHour"] : _copyComplianceToComplianceFromHour.ToString());
                else
                    return _copyComplianceToComplianceFromHour;
            }
        }

        private Int32 __copyComplianceToComplianceFromMinute = 0;
        public Int32 CopyComplianceToComplianceFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("CopyComplianceToComplianceFromMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["CopyComplianceToComplianceFromMinute"]) ? ConfigurationManager.AppSettings["CopyComplianceToComplianceFromMinute"] : __copyComplianceToComplianceFromMinute.ToString());
                else
                    return __copyComplianceToComplianceFromMinute;
            }
        }

        private Int32 __copyComplianceToComplianceToHour = 24;
        public Int32 CopyComplianceToComplianceToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("CopyComplianceToComplianceToHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["CopyComplianceToComplianceToHour"]) ? ConfigurationManager.AppSettings["CopyComplianceToComplianceToHour"] : __copyComplianceToComplianceToHour.ToString());
                else
                    return __copyComplianceToComplianceToHour;
            }
        }

        private Int32 __copyComplianceToComplianceToMinute = 0;
        public Int32 CopyComplianceToComplianceToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("CopyComplianceToComplianceToMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["CopyComplianceToComplianceToMinute"]) ? ConfigurationManager.AppSettings["CopyComplianceToComplianceToMinute"] : __copyComplianceToComplianceToMinute.ToString());
                else
                    return __copyComplianceToComplianceToMinute;
            }
        }

        #endregion

        #region 3224
        //UAT-3224 System queue to retry the data sync which was not completed 
        private Int32 _bkgCopyPackageDataInterval = 86400000;
        public Int32 BkgCopyPackageDataIntervalInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("BkgCopyPackageDataIntervalInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["BkgCopyPackageDataIntervalInterval"]) ? ConfigurationManager.AppSettings["BkgCopyPackageDataIntervalInterval"] : _bkgCopyPackageDataInterval.ToString());
                else
                    return _bkgCopyPackageDataInterval;
            }
        }

        private Int32 _bkgCopyPackageDataFromHour = 0;
        public Int32 BkgCopyPackageDataFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("BkgCopyPackageDataFromHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["BkgCopyPackageDataFromHour"]) ? ConfigurationManager.AppSettings["BkgCopyPackageDataFromHour"] : _bkgCopyPackageDataFromHour.ToString());
                else
                    return _bkgCopyPackageDataFromHour;
            }
        }

        private Int32 _bkgCopyPackageDataFromMinute = 0;
        public Int32 BkgCopyPackageDataFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("BkgCopyPackageDataFromMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["BkgCopyPackageDataFromMinute"]) ? ConfigurationManager.AppSettings["BkgCopyPackageDataFromMinute"] : _bkgCopyPackageDataFromMinute.ToString());
                else
                    return _bkgCopyPackageDataFromMinute;
            }
        }

        private Int32 _bkgCopyPackageDataToHour = 24;
        public Int32 BkgCopyPackageDataToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("BkgCopyPackageDataToHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["BkgCopyPackageDataToHour"]) ? ConfigurationManager.AppSettings["BkgCopyPackageDataToHour"] : _bkgCopyPackageDataToHour.ToString());
                else
                    return _bkgCopyPackageDataToHour;
            }
        }

        private Int32 _bkgCopyPackageDataToMinute = 0;
        public Int32 BkgCopyPackageDataToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("BkgCopyPackageDataToMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["BkgCopyPackageDataToMinute"]) ? ConfigurationManager.AppSettings["BkgCopyPackageDataToMinute"] : _bkgCopyPackageDataToMinute.ToString());
                else
                    return _bkgCopyPackageDataToMinute;
            }
        }
        #endregion

        #region UAT-3820
        private Int32 _receivedFromStuServFormStatusInterval = 86400000;
        public Int32 ReceivedFromStudentServiceFormStatusInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ReceivedFromStudentServiceFormStatusInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ReceivedFromStudentServiceFormStatusInterval"]) ? ConfigurationManager.AppSettings["ReceivedFromStudentServiceFormStatusInterval"] : _receivedFromStuServFormStatusInterval.ToString());
                else
                    return _receivedFromStuServFormStatusInterval;
            }
        }

        private Int32 _receivedFromStuServFormStatusDataFromHour = 0;
        public Int32 ReceivedFromStuServFormStatusFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ReceivedFromStuServFormStatusFromHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ReceivedFromStuServFormStatusFromHour"]) ? ConfigurationManager.AppSettings["ReceivedFromStuServFormStatusFromHour"] : _receivedFromStuServFormStatusDataFromHour.ToString());
                else
                    return _receivedFromStuServFormStatusDataFromHour;
            }
        }

        private Int32 _receivedFromStuServFormStatusDataFromMinute = 0;
        public Int32 ReceivedFromStuServFormStatusDataFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ReceivedFromStuServFormStatusDataFromMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ReceivedFromStuServFormStatusDataFromMinute"]) ? ConfigurationManager.AppSettings["ReceivedFromStuServFormStatusDataFromMinute"] : _receivedFromStuServFormStatusDataFromMinute.ToString());
                else
                    return _receivedFromStuServFormStatusDataFromMinute;
            }
        }

        private Int32 _receivedFromStuServFormStatusDataToHour = 24;
        public Int32 ReceivedFromStuServFormStatusDataToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ReceivedFromStuServFormStatusDataToHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ReceivedFromStuServFormStatusDataToHour"]) ? ConfigurationManager.AppSettings["ReceivedFromStuServFormStatusDataToHour"] : _receivedFromStuServFormStatusDataToHour.ToString());
                else
                    return _receivedFromStuServFormStatusDataToHour;
            }
        }

        private Int32 _receivedFromStuServFormStatusDataToMinute = 0;
        public Int32 ReceivedFromStuServFormStatusDataToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ReceivedFromStuServFormStatusDataToMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ReceivedFromStuServFormStatusDataToMinute"]) ? ConfigurationManager.AppSettings["ReceivedFromStuServFormStatusDataToMinute"] : _receivedFromStuServFormStatusDataToMinute.ToString());
                else
                    return _receivedFromStuServFormStatusDataToMinute;
            }
        }

        #endregion



        #region UAT-3734

        private Int32 _OffTimeRevokedAppointmentEmailInterval = 86400000;
        public Int32 OffTimeRevokedAppointmentEmailInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("OffTimeRevokedAppointmentEmailInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["OffTimeRevokedAppointmentEmailInterval"]) ? ConfigurationManager.AppSettings["OffTimeRevokedAppointmentEmailInterval"] : _OffTimeRevokedAppointmentEmailInterval.ToString());
                else
                    return _OffTimeRevokedAppointmentEmailInterval;
            }
        }
        private Int32 _OffTimeRevokedAppointmentEmailFromHour = 0;
        public Int32 OffTimeRevokedAppointmentEmailFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("OffTimeRevokedAppointmentEmailFromHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["OffTimeRevokedAppointmentEmailFromHour"]) ? ConfigurationManager.AppSettings["OffTimeRevokedAppointmentEmailFromHour"] : _OffTimeRevokedAppointmentEmailFromHour.ToString());
                else
                    return _OffTimeRevokedAppointmentEmailFromHour;
            }
        }

        private Int32 _OffTimeRevokedAppointmentEmailFromMinute = 0;
        public Int32 OffTimeRevokedAppointmentEmailFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("OffTimeRevokedAppointmentEmailFromMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["OffTimeRevokedAppointmentEmailFromMinute"]) ? ConfigurationManager.AppSettings["OffTimeRevokedAppointmentEmailFromMinute"] : _OffTimeRevokedAppointmentEmailFromMinute.ToString());
                else
                    return _OffTimeRevokedAppointmentEmailFromMinute;
            }
        }

        private Int32 _OffTimeRevokedAppointmentEmailToHour = 24;
        public Int32 OffTimeRevokedAppointmentEmailToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("OffTimeRevokedAppointmentEmailToHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["OffTimeRevokedAppointmentEmailToHour"]) ? ConfigurationManager.AppSettings["OffTimeRevokedAppointmentEmailToHour"] : _OffTimeRevokedAppointmentEmailToHour.ToString());
                else
                    return _OffTimeRevokedAppointmentEmailToHour;
            }
        }

        private Int32 _OffTimeRevokedAppointmentEmailToMinute = 0;
        public Int32 OffTimeRevokedAppointmentEmailToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("OffTimeRevokedAppointmentEmailToMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["OffTimeRevokedAppointmentEmailToMinute"]) ? ConfigurationManager.AppSettings["OffTimeRevokedAppointmentEmailToMinute"] : _OffTimeRevokedAppointmentEmailToMinute.ToString());
                else
                    return _OffTimeRevokedAppointmentEmailToMinute;
            }
        }
        #endregion
        #region UAT-3485

        private Int32 _sendRotationAbtToExpireEmailInterval = 86400000;
        public Int32 SendRotationAbtToExpireEmailInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("SendRotationAbtToExpireEmailInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["SendRotationAbtToExpireEmailInterval"]) ? ConfigurationManager.AppSettings["SendRotationAbtToExpireEmailInterval"] : _sendRotationAbtToExpireEmailInterval.ToString());
                else
                    return _sendRotationAbtToExpireEmailInterval;
            }
        }

        private Int32 _sendRotationAbtToExpireEmailFromHour = 0;
        public Int32 SendRotationAbtToExpireEmailFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("SendRotationAbtToExpireEmailFromHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["SendRotationAbtToExpireEmailFromHour"]) ? ConfigurationManager.AppSettings["SendRotationAbtToExpireEmailFromHour"] : _sendRotationAbtToExpireEmailFromHour.ToString());
                else
                    return _sendRotationAbtToExpireEmailFromHour;
            }
        }

        private Int32 _sendRotationAbtToExpireEmailFromMinute = 0;
        public Int32 SendRotationAbtToExpireEmailFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("SendRotationAbtToExpireEmailFromMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["SendRotationAbtToExpireEmailFromMinute"]) ? ConfigurationManager.AppSettings["SendRotationAbtToExpireEmailFromMinute"] : _sendRotationAbtToExpireEmailFromMinute.ToString());
                else
                    return _sendRotationAbtToExpireEmailFromMinute;
            }
        }

        private Int32 _sendRotationAbtToExpireEmailToHour = 24;
        public Int32 SendRotationAbtToExpireEmailToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("SendRotationAbtToExpireEmailToHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["SendRotationAbtToExpireEmailToHour"]) ? ConfigurationManager.AppSettings["SendRotationAbtToExpireEmailToHour"] : _sendRotationAbtToExpireEmailToHour.ToString());
                else
                    return _sendRotationAbtToExpireEmailToHour;
            }
        }

        private Int32 _sendRotationAbtToExpireEmailToMinute = 0;
        public Int32 SendRotationAbtToExpireEmailToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("SendRotationAbtToExpireEmailToMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["SendRotationAbtToExpireEmailToMinute"]) ? ConfigurationManager.AppSettings["SendRotationAbtToExpireEmailToMinute"] : _sendRotationAbtToExpireEmailToMinute.ToString());
                else
                    return _sendRotationAbtToExpireEmailToMinute;
            }
        }

        #endregion


        #region Appointment Status Chnage and Mail send for Missed Appointments

        private Int32 _MissedAppointmentEmailInterval = 86400000;
        public Int32 MissedAppointmentEmailInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("MissedAppointmentEmailInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["MissedAppointmentEmailInterval"]) ? ConfigurationManager.AppSettings["MissedAppointmentEmailInterval"] : _MissedAppointmentEmailInterval.ToString());
                else
                    return _MissedAppointmentEmailInterval;
            }
        }
        private Int32 _MissedAppointmentEmailFromHour = 0;
        public Int32 MissedAppointmentEmailFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("MissedAppointmentEmailFromHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["MissedAppointmentEmailFromHour"]) ? ConfigurationManager.AppSettings["MissedAppointmentEmailFromHour"] : _MissedAppointmentEmailFromHour.ToString());
                else
                    return _MissedAppointmentEmailFromHour;
            }
        }

        private Int32 _MissedAppointmentEmailFromMinute = 0;
        public Int32 MissedAppointmentEmailFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("MissedAppointmentEmailFromMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["MissedAppointmentEmailFromMinute"]) ? ConfigurationManager.AppSettings["MissedAppointmentEmailFromMinute"] : _MissedAppointmentEmailFromMinute.ToString());
                else
                    return _MissedAppointmentEmailFromMinute;
            }
        }

        private Int32 _MissedAppointmentEmailToHour = 24;
        public Int32 MissedAppointmentEmailToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("MissedAppointmentEmailToHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["MissedAppointmentEmailToHour"]) ? ConfigurationManager.AppSettings["MissedAppointmentEmailToHour"] : _MissedAppointmentEmailToHour.ToString());
                else
                    return _MissedAppointmentEmailToHour;
            }
        }

        private Int32 _MissedAppointmentEmailToMinute = 0;
        public Int32 MissedAppointmentEmailToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("MissedAppointmentEmailToMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["MissedAppointmentEmailToMinute"]) ? ConfigurationManager.AppSettings["MissedAppointmentEmailToMinute"] : _MissedAppointmentEmailToMinute.ToString());
                else
                    return _MissedAppointmentEmailToMinute;
            }
        }



        private Int32 _MissedAppointmentInterval = 86400000;
        public Int32 MissedAppointmentInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("MissedAppointmentInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["MissedAppointmentInterval"]) ? ConfigurationManager.AppSettings["MissedAppointmentInterval"] : _MissedAppointmentInterval.ToString());
                else
                    return _MissedAppointmentInterval;
            }
        }
        private Int32 _MissedAppointmentFromHour = 0;
        public Int32 MissedAppointmentFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("MissedAppointmentFromHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["MissedAppointmentFromHour"]) ? ConfigurationManager.AppSettings["MissedAppointmentFromHour"] : _MissedAppointmentFromHour.ToString());
                else
                    return _MissedAppointmentFromHour;
            }
        }

        private Int32 _MissedAppointmentFromMinute = 0;
        public Int32 MissedAppointmentFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("MissedAppointmentFromMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["MissedAppointmentFromMinute"]) ? ConfigurationManager.AppSettings["MissedAppointmentFromMinute"] : _MissedAppointmentFromMinute.ToString());
                else
                    return _MissedAppointmentFromMinute;
            }
        }

        private Int32 _MissedAppointmentToHour = 24;
        public Int32 MissedAppointmentToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("MissedAppointmentToHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["MissedAppointmentToHour"]) ? ConfigurationManager.AppSettings["MissedAppointmentToHour"] : _MissedAppointmentToHour.ToString());
                else
                    return _MissedAppointmentToHour;
            }
        }

        private Int32 _MissedAppointmentToMinute = 0;
        public Int32 MissedAppointmentToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("MissedAppointmentToMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["MissedAppointmentToMinute"]) ? ConfigurationManager.AppSettings["MissedAppointmentToMinute"] : _MissedAppointmentToMinute.ToString());
                else
                    return _MissedAppointmentToMinute;
            }
        }


        #endregion


        #region Create Requirement Snapshot On Rotation End
        private Int32 _createRequirementSnapshotOnRotationEndInterval = 86400000;
        public Int32 CreateRequirementSnapshotOnRotationEndInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("CreateRequirementSnapshotOnRotationEndInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["CreateRequirementSnapshotOnRotationEndInterval"]) ? ConfigurationManager.AppSettings["CreateRequirementSnapshotOnRotationEndInterval"] : _createRequirementSnapshotOnRotationEndInterval.ToString());
                else
                    return _createRequirementSnapshotOnRotationEndInterval;
            }
        }

        private Int32 _createRequirementSnapshotOnRotationEndFromHour = 0;
        public Int32 CreateRequirementSnapshotOnRotationEndFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("CreateRequirementSnapshotOnRotationEndFromHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["CreateRequirementSnapshotOnRotationEndFromHour"]) ? ConfigurationManager.AppSettings["CreateRequirementSnapshotOnRotationEndFromHour"] : _createRequirementSnapshotOnRotationEndFromHour.ToString());
                else
                    return _createRequirementSnapshotOnRotationEndFromHour;
            }
        }

        private Int32 _createRequirementSnapshotOnRotationEndFromMinute = 0;
        public Int32 CreateRequirementSnapshotOnRotationEndFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("CreateRequirementSnapshotOnRotationEndFromMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["CreateRequirementSnapshotOnRotationEndFromMinute"]) ? ConfigurationManager.AppSettings["CreateRequirementSnapshotOnRotationEndFromMinute"] : _createRequirementSnapshotOnRotationEndFromMinute.ToString());
                else
                    return _createRequirementSnapshotOnRotationEndFromMinute;
            }
        }

        private Int32 _createRequirementSnapshotOnRotationEndToHour = 24;
        public Int32 CreateRequirementSnapshotOnRotationEndToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("CreateRequirementSnapshotOnRotationEndToHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["CreateRequirementSnapshotOnRotationEndToHour"]) ? ConfigurationManager.AppSettings["CreateRequirementSnapshotOnRotationEndToHour"] : _createRequirementSnapshotOnRotationEndToHour.ToString());
                else
                    return _createRequirementSnapshotOnRotationEndToHour;
            }
        }

        private Int32 _createRequirementSnapshotOnRotationEndToMinute = 0;
        public Int32 CreateRequirementSnapshotOnRotationEndToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("CreateRequirementSnapshotOnRotationEndToMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["CreateRequirementSnapshotOnRotationEndToMinute"]) ? ConfigurationManager.AppSettings["CreateRequirementSnapshotOnRotationEndToMinute"] : _createRequirementSnapshotOnRotationEndToMinute.ToString());
                else
                    return _createRequirementSnapshotOnRotationEndToMinute;
            }
        }
        #endregion

        #region Background Digestion Procedure Call
        private Int32 _bkgDigestionProcedureCallInterval = 0;
        public Int32 BkgDigestionProcedureCallInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("BkgDigestionProcedureCallInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["BkgDigestionProcedureCallInterval"]) ? ConfigurationManager.AppSettings["BkgDigestionProcedureCallInterval"] : _bkgDigestionProcedureCallInterval.ToString());
                else
                    return _bkgDigestionProcedureCallInterval;
            }
        }

        private Int32 _bkgProcedureCallFromHour = 0;
        public Int32 BkgProcedureCallFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("BkgDigestionProcedureCallFromHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["BkgDigestionProcedureCallFromHour"]) ? ConfigurationManager.AppSettings["BkgDigestionProcedureCallFromHour"] : _bkgProcedureCallFromHour.ToString());
                else
                    return _bkgProcedureCallFromHour;
            }
        }

        private Int32 _bkgProcedureCallFromMinute = 0;
        public Int32 BkgProcedureCallFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("BkgDigestionProcedureCallFromMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["BkgDigestionProcedureCallFromMinute"]) ? ConfigurationManager.AppSettings["BkgDigestionProcedureCallFromMinute"] : _bkgProcedureCallFromMinute.ToString());
                else
                    return _bkgProcedureCallFromMinute;
            }
        }

        private Int32 _bkgProcedureCallToHour = 24;
        public Int32 BkgProcedureCallToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("BkgDigestionProcedureCallToHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["BkgDigestionProcedureCallToHour"]) ? ConfigurationManager.AppSettings["BkgDigestionProcedureCallToHour"] : _bkgProcedureCallToHour.ToString());
                else
                    return _bkgProcedureCallToHour;
            }
        }

        private Int32 _bkgProcedureCallToMinute = 0;
        public Int32 BkgProcedureCallToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("BkgDigestionProcedureCallToMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["BkgDigestionProcedureCallToMinute"]) ? ConfigurationManager.AppSettings["BkgDigestionProcedureCallToMinute"] : _bkgProcedureCallToMinute.ToString());
                else
                    return _bkgProcedureCallToMinute;
            }
        }


        #endregion

        #region UAT 3950


        public TimeSpan ArchiveRotationDataStartTime
        {
            get
            {
                return new TimeSpan(ArchiveRotationDataFromHour, ArchiveRotationDataFromMinute, 0);
            }
        }
        public TimeSpan ArchiveRotationDataEndTime
        {
            get
            {

                return new TimeSpan(ArchiveRotationDataToHour, ArchiveRotationDataToMinute, 0);
            }
        }


        private Int32 _archiveRotationDataInterval = 86400000;
        public Int32 ArchiveRotationDataInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ArchiveRotationDataInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ArchiveRotationDataInterval"]) ? ConfigurationManager.AppSettings["ArchiveRotationDataInterval"] : _archiveRotationDataInterval.ToString());
                else
                    return _archiveRotationDataInterval;
            }
        }
        private Int32 _archiveRotationDataFromHour = 0;
        public Int32 ArchiveRotationDataFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ArchiveRotationDataFromHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ArchiveRotationDataFromHour"]) ? ConfigurationManager.AppSettings["ArchiveRotationDataFromHour"] : _archiveRotationDataFromHour.ToString());
                else
                    return _archiveRotationDataFromHour;
            }
        }

        private Int32 _archiveRotationDataFromMinute = 0;
        public Int32 ArchiveRotationDataFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ArchiveRotationDataFromMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ArchiveRotationDataFromMinute"]) ? ConfigurationManager.AppSettings["ArchiveRotationDataFromMinute"] : _archiveRotationDataFromMinute.ToString());
                else
                    return _archiveRotationDataFromMinute;
            }
        }

        private Int32 _archiveRotationDataToHour = 24;
        public Int32 ArchiveRotationDataToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ArchiveRotationDataToHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ArchiveRotationDataToHour"]) ? ConfigurationManager.AppSettings["ArchiveRotationDataToHour"] : _archiveRotationDataToHour.ToString());
                else
                    return _archiveRotationDataToHour;
            }
        }

        private Int32 _archiveRotationDataToMinute = 0;
        public Int32 ArchiveRotationDataToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ArchiveRotationDataToMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ArchiveRotationDataToMinute"]) ? ConfigurationManager.AppSettings["ArchiveRotationDataToMinute"] : _archiveRotationDataToMinute.ToString());
                else
                    return _archiveRotationDataToMinute;
            }
        }

        #endregion

        #region Complio TalkDesk Integration

        #region Complio TalkDesk Integration - Create Report API Call

        private Int32 _complioTalkDeskCreateReportJobInterval = 3000000;
        public Int32 ComplioTalkDeskCreateReportJobInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ComplioTalkDeskCreateReportJobInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ComplioTalkDeskCreateReportJobInterval"]) ? ConfigurationManager.AppSettings["ComplioTalkDeskCreateReportJobInterval"] : _complioTalkDeskCreateReportJobInterval.ToString());
                else
                    return _complioTalkDeskCreateReportJobInterval;
            }
        }

        private Int32 _complioTalkDeskCreateReportJobFromHour = 0;
        public Int32 ComplioTalkDeskCreateReportJobFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ComplioTalkDeskCreateReportJobFromHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ComplioTalkDeskCreateReportJobFromHour"]) ? ConfigurationManager.AppSettings["ComplioTalkDeskCreateReportJobFromHour"] : _complioTalkDeskCreateReportJobFromHour.ToString());
                else
                    return _complioTalkDeskCreateReportJobFromHour;
            }
        }

        private Int32 _complioTalkDeskCreateReportJobFromMinute = 0;
        public Int32 ComplioTalkDeskCreateReportJobFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ComplioTalkDeskCreateReportJobFromMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ComplioTalkDeskCreateReportJobFromMinute"]) ? ConfigurationManager.AppSettings["ComplioTalkDeskCreateReportJobFromMinute"] : _complioTalkDeskCreateReportJobFromMinute.ToString());
                else
                    return _complioTalkDeskCreateReportJobFromMinute;
            }
        }

        private Int32 _complioTalkDeskCreateReportJobToHour = 24;
        public Int32 ComplioTalkDeskCreateReportJobToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ComplioTalkDeskCreateReportJobToHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ComplioTalkDeskCreateReportJobToHour"]) ? ConfigurationManager.AppSettings["ComplioTalkDeskCreateReportJobToHour"] : _complioTalkDeskCreateReportJobToHour.ToString());
                else
                    return _complioTalkDeskCreateReportJobToHour;
            }
        }

        private Int32 _complioTalkDeskCreateReportJobToMinute = 0;
        public Int32 ComplioTalkDeskCreateReportJobToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ComplioTalkDeskCreateReportJobToMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ComplioTalkDeskCreateReportJobToMinute"]) ? ConfigurationManager.AppSettings["ComplioTalkDeskCreateReportJobToMinute"] : _complioTalkDeskCreateReportJobToMinute.ToString());
                else
                    return _complioTalkDeskCreateReportJobToMinute;
            }
        }

        public TimeSpan ComplioTalkDeskCreateReportJobStartTime
        {
            get
            {
                return new TimeSpan(ComplioTalkDeskCreateReportJobFromHour, ComplioTalkDeskCreateReportJobFromMinute, 0);
            }
        }

        public TimeSpan ComplioTalkDeskCreateReportJobEndTime
        {
            get
            {
                return new TimeSpan(ComplioTalkDeskCreateReportJobToHour, ComplioTalkDeskCreateReportJobToMinute, 0);
            }
        }

        ////Complio TalkDesk Integration - Create Report API Call
        private Boolean isComplioTalkDeskCreateReportJobInterval = false;
        private Timer complioTalkDeskCreateReportJobTimer = null;
        private Boolean isComplioTalkDeskCreateReportJobDataExecuting = false;
        private Object lockComplioTalkDeskCreateReportJobDataObject = new Object();

        void ComplioTalkDeskCreateReportJob_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!isComplioTalkDeskCreateReportJobInterval)
            {
                complioTalkDeskCreateReportJobTimer.Interval = Convert.ToDouble(ComplioTalkDeskCreateReportJobInterval);
                isComplioTalkDeskCreateReportJobInterval = true;
            }

            TimeSpan CurrentTime = DateTime.Now.TimeOfDay;

            logger.Info("Timer invoked for Complio TalkDesk Integration - Create Report API Call.");
            lock (this.lockComplioTalkDeskCreateReportJobDataObject)
            {
                if (isComplioTalkDeskCreateReportJobDataExecuting)
                    return;
                isComplioTalkDeskCreateReportJobDataExecuting = true;
            }
            logger.Info("Entered processing for Complio TalkDesk Integration - Create Report API Call.");
            try
            {
                if (CurrentTime >= ComplioTalkDeskCreateReportJobStartTime && CurrentTime <= ComplioTalkDeskCreateReportJobEndTime)
                {
                    logger.Trace("Started calling Complio TalkDesk Integration - Create Report API Call: " + DateTime.Now.ToString());
                    ComplioTalkDeskJobTask.CreateTalkDeskJobs();
                    logger.Trace("End calling Complio TalkDesk Integration - Create Report API Call:" + DateTime.Now.ToString());
                }
                else
                {
                    logger.Trace("Exit Complio TalkDesk Integration - Create Report API Call:" + DateTime.Now.ToString());
                }
            }
            catch (Exception ex)
            {
                isComplioTalkDeskCreateReportJobDataExecuting = false;
                logger.Error("An Error has occured in Complio TalkDesk Integration - Create Report API Call, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                isComplioTalkDeskCreateReportJobDataExecuting = false;

            }
        }

        #endregion

        #region Complio TalkDesk Integration - Update Report API Call

        private Int32 _complioTalkDeskUpdateReportJobInterval = 3000000;
        public Int32 ComplioTalkDeskUpdateReportJobInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ComplioTalkDeskUpdateReportJobInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ComplioTalkDeskUpdateReportJobInterval"]) ? ConfigurationManager.AppSettings["ComplioTalkDeskUpdateReportJobInterval"] : _complioTalkDeskUpdateReportJobInterval.ToString());
                else
                    return _complioTalkDeskUpdateReportJobInterval;
            }
        }

        private Int32 _complioTalkDeskUpdateReportJobFromHour = 0;
        public Int32 ComplioTalkDeskUpdateReportJobFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ComplioTalkDeskUpdateReportJobFromHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ComplioTalkDeskUpdateReportJobFromHour"]) ? ConfigurationManager.AppSettings["ComplioTalkDeskUpdateReportJobFromHour"] : _complioTalkDeskUpdateReportJobFromHour.ToString());
                else
                    return _complioTalkDeskUpdateReportJobFromHour;
            }
        }

        private Int32 _complioTalkDeskUpdateReportJobFromMinute = 0;
        public Int32 ComplioTalkDeskUpdateReportJobFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ComplioTalkDeskUpdateReportJobFromMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ComplioTalkDeskUpdateReportJobFromMinute"]) ? ConfigurationManager.AppSettings["ComplioTalkDeskUpdateReportJobFromMinute"] : _complioTalkDeskUpdateReportJobFromMinute.ToString());
                else
                    return _complioTalkDeskUpdateReportJobFromMinute;
            }
        }

        private Int32 _complioTalkDeskUpdateReportJobToHour = 24;
        public Int32 ComplioTalkDeskUpdateReportJobToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ComplioTalkDeskUpdateReportJobToHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ComplioTalkDeskUpdateReportJobToHour"]) ? ConfigurationManager.AppSettings["ComplioTalkDeskUpdateReportJobToHour"] : _complioTalkDeskUpdateReportJobToHour.ToString());
                else
                    return _complioTalkDeskUpdateReportJobToHour;
            }
        }

        private Int32 _complioTalkDeskUpdateReportJobToMinute = 0;
        public Int32 ComplioTalkDeskUpdateReportJobToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ComplioTalkDeskUpdateReportJobToMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ComplioTalkDeskUpdateReportJobToMinute"]) ? ConfigurationManager.AppSettings["ComplioTalkDeskUpdateReportJobToMinute"] : _complioTalkDeskUpdateReportJobToMinute.ToString());
                else
                    return _complioTalkDeskUpdateReportJobToMinute;
            }
        }

        public TimeSpan ComplioTalkDeskUpdateReportJobStartTime
        {
            get
            {
                return new TimeSpan(ComplioTalkDeskUpdateReportJobFromHour, ComplioTalkDeskUpdateReportJobFromMinute, 0);
            }
        }

        public TimeSpan ComplioTalkDeskUpdateReportJobEndTime
        {
            get
            {
                return new TimeSpan(ComplioTalkDeskUpdateReportJobToHour, ComplioTalkDeskUpdateReportJobToMinute, 0);
            }
        }

        //Complio TalkDesk Integration - Update Report API Call
        private Boolean isComplioTalkDeskUpdateReportJobInterval = false;
        private Timer complioTalkDeskUpdateReportJobTimer = null;
        private Boolean isComplioTalkDeskUpdateReportJobDataExecuting = false;
        private Object lockComplioTalkDeskUpdateReportJobDataObject = new Object();

        void ComplioTalkDeskUpdateReportJob_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!isComplioTalkDeskUpdateReportJobInterval)
            {
                complioTalkDeskUpdateReportJobTimer.Interval = Convert.ToDouble(ComplioTalkDeskUpdateReportJobInterval);
                isComplioTalkDeskUpdateReportJobInterval = true;
            }

            TimeSpan CurrentTime = DateTime.Now.TimeOfDay;

            logger.Info("Timer invoked for Complio TalkDesk Integration - Update Report API Call.");
            lock (this.lockComplioTalkDeskUpdateReportJobDataObject)
            {
                if (isComplioTalkDeskUpdateReportJobDataExecuting)
                    return;
                isComplioTalkDeskUpdateReportJobDataExecuting = true;
            }
            logger.Info("Entered processing for Complio TalkDesk Integration - Update Report API Call.");
            try
            {
                if (CurrentTime >= ComplioTalkDeskUpdateReportJobStartTime && CurrentTime <= ComplioTalkDeskUpdateReportJobEndTime)
                {
                    logger.Trace("Started calling Complio TalkDesk Integration - Update Report API Call: " + DateTime.Now.ToString());
                    ComplioTalkDeskJobTask.UpdateTalkDeskJobs();
                    logger.Trace("End calling Complio TalkDesk Integration - Update Report API Call:" + DateTime.Now.ToString());
                }
                else
                {
                    logger.Trace("Exit Complio TalkDesk Integration - Update Report API Call:" + DateTime.Now.ToString());
                }
            }
            catch (Exception ex)
            {
                isComplioTalkDeskUpdateReportJobDataExecuting = false;
                logger.Error("An Error has occured in Complio TalkDesk Integration - Update Report API Call, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                isComplioTalkDeskUpdateReportJobDataExecuting = false;

            }
        }

        #endregion

        #region Complio TalkDesk Integration - Update Report API Call

        private Int32 _complioTalkDeskPullCallDataJobInterval = 900000;
        public Int32 ComplioTalkDeskPullCallDataJobInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ComplioTalkDeskPullCallDataJobInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ComplioTalkDeskPullCallDataJobInterval"]) ? ConfigurationManager.AppSettings["ComplioTalkDeskPullCallDataJobInterval"] : _complioTalkDeskPullCallDataJobInterval.ToString());
                else
                    return _complioTalkDeskPullCallDataJobInterval;
            }
        }

        private Int32 _complioTalkDeskPullCallDataJobFromHour = 0;
        public Int32 ComplioTalkDeskPullCallDataJobFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ComplioTalkDeskPullCallDataJobFromHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ComplioTalkDeskPullCallDataJobFromHour"]) ? ConfigurationManager.AppSettings["ComplioTalkDeskPullCallDataJobFromHour"] : _complioTalkDeskPullCallDataJobFromHour.ToString());
                else
                    return _complioTalkDeskPullCallDataJobFromHour;
            }
        }

        private Int32 _complioTalkDeskPullCallDataJobFromMinute = 0;
        public Int32 ComplioTalkDeskPullCallDataJobFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ComplioTalkDeskPullCallDataJobFromMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ComplioTalkDeskPullCallDataJobFromMinute"]) ? ConfigurationManager.AppSettings["ComplioTalkDeskPullCallDataJobFromMinute"] : _complioTalkDeskPullCallDataJobFromMinute.ToString());
                else
                    return _complioTalkDeskPullCallDataJobFromMinute;
            }
        }

        private Int32 _complioTalkDeskPullCallDataJobToHour = 24;
        public Int32 ComplioTalkDeskPullCallDataJobToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ComplioTalkDeskPullCallDataJobToHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ComplioTalkDeskPullCallDataJobToHour"]) ? ConfigurationManager.AppSettings["ComplioTalkDeskPullCallDataJobToHour"] : _complioTalkDeskPullCallDataJobToHour.ToString());
                else
                    return _complioTalkDeskPullCallDataJobToHour;
            }
        }

        private Int32 _complioTalkDeskPullCallDataJobToMinute = 0;
        public Int32 ComplioTalkDeskPullCallDataJobToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ComplioTalkDeskPullCallDataJobToMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ComplioTalkDeskPullCallDataJobToMinute"]) ? ConfigurationManager.AppSettings["ComplioTalkDeskPullCallDataJobToMinute"] : _complioTalkDeskPullCallDataJobToMinute.ToString());
                else
                    return _complioTalkDeskPullCallDataJobToMinute;
            }
        }

        public TimeSpan ComplioTalkDeskPullCallDataJobStartTime
        {
            get
            {
                return new TimeSpan(ComplioTalkDeskPullCallDataJobFromHour, ComplioTalkDeskPullCallDataJobFromMinute, 0);
            }
        }

        public TimeSpan ComplioTalkDeskPullCallDataJobEndTime
        {
            get
            {
                return new TimeSpan(ComplioTalkDeskPullCallDataJobToHour, ComplioTalkDeskPullCallDataJobToMinute, 0);
            }
        }

        //Complio TalkDesk Integration - Pull Call data from Report Job API Call
        private Boolean isComplioTalkDeskPullCallDataJobTimerInterval = false;
        private Timer complioTalkDeskPullCallDataJobTimer = null;
        private Boolean isComplioTalkDeskPullCallDataJobDataExecuting = false;
        private Object lockComplioTalkDeskPullCallDataJobDataObject = new Object();

        void ComplioTalkDeskPullCallDataJob_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!isComplioTalkDeskPullCallDataJobTimerInterval)
            {
                complioTalkDeskPullCallDataJobTimer.Interval = Convert.ToDouble(ComplioTalkDeskPullCallDataJobInterval);
                isComplioTalkDeskPullCallDataJobTimerInterval = true;
            }

            TimeSpan CurrentTime = DateTime.Now.TimeOfDay;

            logger.Info("Timer invoked for Complio TalkDesk Integration - Pull Call Data API Call.");
            lock (this.lockComplioTalkDeskPullCallDataJobDataObject)
            {
                if (isComplioTalkDeskPullCallDataJobDataExecuting)
                    return;
                isComplioTalkDeskPullCallDataJobDataExecuting = true;
            }
            logger.Info("Entered processing for Complio TalkDesk Integration -  Pull Call Data API Call.");
            try
            {
                if (CurrentTime >= ComplioTalkDeskPullCallDataJobStartTime && CurrentTime <= ComplioTalkDeskPullCallDataJobEndTime)
                {
                    logger.Trace("Started calling Complio TalkDesk Integration -  Pull Call Data API Call: " + DateTime.Now.ToString());
                    ComplioTalkDeskCallDataTask.GetJobCallData();
                    logger.Trace("End calling Complio TalkDesk Integration -  Pull Call Data API Call:" + DateTime.Now.ToString());
                }
                else
                {
                    logger.Trace("Exit Complio TalkDesk Integration -  Pull Call Data API Call:" + DateTime.Now.ToString());
                }
            }
            catch (Exception ex)
            {
                isComplioTalkDeskPullCallDataJobDataExecuting = false;
                logger.Error("An Error has occured in Complio TalkDesk Integration -  Pull Call Data API Call, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                isComplioTalkDeskPullCallDataJobDataExecuting = false;

            }
        }

        #endregion

        #endregion



        public TimeSpan ReminderStartTime
        {
            get
            {
                return new TimeSpan(ReminderFromHour, ReminderFromMinute, 0);
            }
        }

        public TimeSpan ReminderEndTime
        {
            get
            {
                return new TimeSpan(ReminderToHour, ReminderToMinute, 0);
            }
        }

        public TimeSpan ComplianceStartTime
        {
            get
            {
                return new TimeSpan(ComplianceFromHour, ComplianceFromMinute, 0);
            }
        }

        public TimeSpan ComplianceEndTime
        {
            get
            {
                return new TimeSpan(ComplianceToHour, ComplianceToMinute, 0);
            }
        }

        public TimeSpan ScheduleActionStartTime
        {
            get
            {
                return new TimeSpan(ScheduleActionFromHour, ScheduleActionFromMinute, 0);
            }
        }

        public TimeSpan ScheduleActionEndTime
        {
            get
            {
                return new TimeSpan(ScheduleActionToHour, ScheduleActionToMinute, 0);
            }
        }

        public TimeSpan MobilityInstanceStartTime
        {
            get
            {
                return new TimeSpan(MobilityInstanceFromHour, MobilityInstanceFromMinute, 0);
            }
        }

        public TimeSpan MobilityInstanceEndTime
        {
            get
            {
                return new TimeSpan(MobilityInstanceToHour, MobilityInstanceToMinute, 0);
            }
        }

        public TimeSpan CopyPackageDataStartTime
        {
            get
            {
                return new TimeSpan(CopyPackageDataFromHour, CopyPackageDataFromMinute, 0);
            }
        }

        public TimeSpan CopyPackageDataEndTime
        {
            get
            {
                return new TimeSpan(CopyPackageDataToHour, CopyPackageDataToMinute, 0);
            }
        }

        public TimeSpan AdminWidgetStartTime
        {
            get
            {
                return new TimeSpan(AdminWidgetFromHour, AdminWidgetFromMinute, 0);
            }
        }

        public TimeSpan AdminWidgetEndTime
        {
            get
            {
                return new TimeSpan(AdminWidgetToHour, AdminWidgetToMinute, 0);
            }
        }

        public TimeSpan SystemServiceTriggerStartTime
        {
            get
            {
                return new TimeSpan(SystemServiceTriggerFromHour, SystemServiceTriggerFromMinute, 0);
            }
        }

        public TimeSpan SystemServiceTriggerEndTime
        {
            get
            {
                return new TimeSpan(SystemServiceTriggerToHour, SystemServiceTriggerToMinute, 0);
            }
        }

        public TimeSpan NagEmailStartTime
        {
            get
            {
                return new TimeSpan(NagEmailFromHour, NagEmailFromMinute, 0);
            }
        }

        public TimeSpan NagEmailEndTime
        {
            get
            {
                return new TimeSpan(NagEmailToHour, NagEmailToMinute, 0);
            }
        }

        public TimeSpan QueueImagingStartTime
        {
            get
            {
                return new TimeSpan(QueueImagingFromHour, QueueImagingFromMinute, 0);
            }
        }

        public TimeSpan QueueImagingEndTime
        {
            get
            {
                return new TimeSpan(QueueImagingToHour, QueueImagingToMinute, 0);
            }
        }

        public TimeSpan DeadlineEmailStartTime
        {
            get
            {
                return new TimeSpan(DeadlineEmailFromHour, DeadlineEmailFromMinute, 0);
            }
        }

        public TimeSpan DeadlineEmailEndTime
        {
            get
            {
                return new TimeSpan(DeadlineEmailToHour, DeadlineEmailToMinute, 0);
            }
        }

        public TimeSpan NextTimeSpan
        {
            get
            {
                return new TimeSpan(24, 0, 0);
            }
        }

        public Double NextTimeSpanSeconds
        {
            get
            {
                return 86400000;
            }
        }

        public TimeSpan ScheduleTaskStartTime
        {
            get
            {
                return new TimeSpan(ScheduleTaskFromHour, ScheduleTaskFromMinute, 0);
            }
        }

        public TimeSpan ScheduleTaskEndTime
        {
            get
            {
                return new TimeSpan(ScheduleTaskToHour, ScheduleTaskToMinute, 0);
            }
        }

        public TimeSpan CategoryComplianceRqdStartTime
        {
            get
            {
                return new TimeSpan(CategoryComplianceRqdFromHour, CategoryComplianceRqdFromMinute, 0);
            }
        }

        public TimeSpan CategoryComplianceRqdEndTime
        {
            get
            {
                return new TimeSpan(CategoryComplianceRqdToHour, CategoryComplianceRqdToMinute, 0);
            }
        }

        public TimeSpan RequirementStartTime
        {
            get
            {
                return new TimeSpan(RequirementFromHour, RequirementFromMinute, 0);
            }
        }

        public TimeSpan RequirementEndTime
        {
            get
            {
                return new TimeSpan(RequirementToHour, RequirementToMinute, 0);
            }
        }

        public TimeSpan ScheduledInvitationStartTime
        {
            get
            {
                return new TimeSpan(ScheduledInvitationFromHour, ScheduledInvitationFromMinute, 0);
            }
        }

        public TimeSpan ScheduledInvitationEndTime
        {
            get
            {
                return new TimeSpan(ScheduledInvitationToHour, ScheduledInvitationToMinute, 0);
            }
        }

        public TimeSpan RotationStartTime
        {
            get
            {
                return new TimeSpan(RotationFromHour, RotationFromMinute, 0);
            }
        }

        public TimeSpan RotationEndTime
        {
            get
            {
                return new TimeSpan(RotationToHour, RotationToMinute, 0);
            }
        }

        public TimeSpan IncompletedOnlineOrderNotificationStartTime
        {
            get
            {
                return new TimeSpan(IncompletedOnlineOrderNotificationFromHour, IncompletedOnlineOrderNotificationFromMinute, 0);
            }
        }

        public TimeSpan IncompletedOnlineOrderNotificationEndTime
        {
            get
            {
                return new TimeSpan(IncompletedOnlineOrderNotificationToHour, IncompletedOnlineOrderNotificationToMinute, 0);
            }
        }

        public TimeSpan ManageContractExpiringItemsNotificationStartTime
        {
            get
            {
                return new TimeSpan(ManageContractExpiringItemsNotificationFromHour, ManageContractExpiringItemsNotificationFromMinute, 0);
            }
        }

        public TimeSpan ManageContractExpiringItemsNotificationEndTime
        {
            get
            {
                return new TimeSpan(ManageContractExpiringItemsNotificationToHour, ManageContractExpiringItemsNotificationToMinute, 0);
            }
        }

        public TimeSpan ManageContractExpiredItemsNotificationStartTime
        {
            get
            {
                return new TimeSpan(ManageContractExpiredItemsNotificationFromHour, ManageContractExpiredItemsNotificationFromMinute, 0);
            }
        }

        public TimeSpan ManageContractExpiredItemsNotificationEndTime
        {
            get
            {
                return new TimeSpan(ManageContractExpiredItemsNotificationToHour, ManageContractExpiredItemsNotificationToMinute, 0);
            }
        }

        public TimeSpan MarkApplicantDocumentsCompleteStartTime
        {
            get
            {
                return new TimeSpan(MarkApplicantDocumentsCompleteFromHour, MarkApplicantDocumentsCompleteFromMinute, 0);
            }
        }

        public TimeSpan MarkApplicantDocumentsCompleteEndTime
        {
            get
            {
                return new TimeSpan(MarkApplicantDocumentsCompleteToHour, MarkApplicantDocumentsCompleteToMinute, 0);
            }
        }

        #region UAT-2628
        public TimeSpan ConversionAndMergingFailedDocumentStartTime
        {
            get
            {
                return new TimeSpan(ConversionMergingForFailedDocumentFromHour, ConversionMergingForFailedDocumentFromMinute, 0);
            }
        }

        public TimeSpan ConversionAndMergingFailedDocumentEndTime
        {
            get
            {
                return new TimeSpan(ConversionMergingForFailedDocumentToHour, ConversionMergingForFailedDocumentToMinute, 0);
            }
        }
        #endregion

        #region UAT-2388 : AutomaticPackageInvitation
        public TimeSpan AutomaticPackageInvitationStartTime
        {
            get
            {
                return new TimeSpan(AutomaticPackageInvitationFromHour, AutomaticPackageInvitationFromMinute, 0);
            }
        }

        public TimeSpan AutomaticPackageInvitationEndTime
        {
            get
            {
                return new TimeSpan(AutomaticPackageInvitationToHour, AutomaticPackageInvitationToMinute, 0);
            }
        }
        #endregion

        #region UAT-3059 : UpdatedApplicantRequirementsNotification
        public TimeSpan UpdatedApplicantRequirementsNotificationStartTime
        {
            get
            {
                return new TimeSpan(UpdatedApplicantRequirementsNotificationFromHour, UpdatedApplicantRequirementsNotificationFromMinute, 0);
            }
        }

        public TimeSpan UpdatedApplicantRequirementsNotificationEndTime
        {
            get
            {
                return new TimeSpan(UpdatedApplicantRequirementsNotificationToHour, UpdatedApplicantRequirementsNotificationToMinute, 0);
            }
        }
        #endregion

        #region UAT-2513 Feature for Client Admin to batch upload rotation creation
        public TimeSpan BatchRotationUploadStartTime
        {
            get
            {
                return new TimeSpan(BatchRotationUploadFromHour, BatchRotationUploadFromMinute, 0);
            }
        }
        public TimeSpan BatchRotationUploadEndTime
        {
            get
            {
                return new TimeSpan(BatchRotationUploadToHour, BatchRotationUploadToMinute, 0);
            }
        }

        #endregion

        public TimeSpan ComplianceAuditDataSyncStartTime
        {
            get
            {
                return new TimeSpan(ComplianceAuditDataSyncFromHour, ComplianceAuditDataSyncFromMinute, 0);
            }
        }

        public TimeSpan ComplianceAuditDataSyncEndTime
        {
            get
            {
                return new TimeSpan(ComplianceAuditDataSyncToHour, ComplianceAuditDataSyncToMinute, 0);
            }
        }

        public TimeSpan ReconcillationQueueSyncStartTime
        {
            get
            {
                return new TimeSpan(ReconcillationQueueSyncFromHour, ReconcillationQueueSyncFromMinute, 0);
            }
        }

        public TimeSpan ReconcillationQueueSyncEndTime
        {
            get
            {
                return new TimeSpan(ReconcillationQueueSyncToHour, ReconcillationQueueSyncToMinute, 0);
            }
        }

        public TimeSpan ReqCategoryComplianceRqdStartTime
        {
            get
            {
                return new TimeSpan(ReqCategoryComplianceRqdFromHour, ReqCategoryComplianceRqdFromMinute, 0);
            }
        }

        public TimeSpan ReqCategoryComplianceRqdEndTime
        {
            get
            {
                return new TimeSpan(ReqCategoryComplianceRqdToHour, ReqCategoryComplianceRqdToMinute, 0);
            }
        }

        public TimeSpan CopyComplianceDataToRequirementStartTime
        {
            get
            {
                return new TimeSpan(CopyComplianceDataToRequirementFromHour, CopyComplianceDataToRequirementFromMinute, 0);
            }
        }

        public TimeSpan CopyComplianceDataToRequirementEndTime
        {
            get
            {
                return new TimeSpan(CopyComplianceDataToRequirementToHour, CopyComplianceDataToRequirementToMinute, 0);
            }
        }

        public TimeSpan RequirementPkgSyncStartTime
        {
            get
            {
                return new TimeSpan(RequirementPkgSyncFromHour, RequirementPkgSyncFromMinute, 0);
            }
        }

        public TimeSpan RequirementPkgSyncEndTime
        {
            get
            {
                return new TimeSpan(RequirementPkgSyncToHour, RequirementPkgSyncToMinute, 0);
            }
        }


        public TimeSpan RequirementPkgAutoArchiveStartTime
        {
            get
            {
                return new TimeSpan(RequirementPkgAutoArchiveFromHour, RequirementPkgAutoArchiveFromMinute, 0);
            }
        }

        public TimeSpan RequirementPkgAutoArchiveEndTime
        {
            get
            {
                return new TimeSpan(RequirementPkgAutoArchiveToHour, RequirementPkgAutoArchiveToMinute, 0);
            }
        }


        public TimeSpan CreateRequirementSnapshotOnRotationEndStartTime
        {
            get
            {
                return new TimeSpan(CreateRequirementSnapshotOnRotationEndFromHour, CreateRequirementSnapshotOnRotationEndFromMinute, 0);
            }
        }

        public TimeSpan CreateRequirementSnapshotOnRotationEndEndTime
        {
            get
            {
                return new TimeSpan(CreateRequirementSnapshotOnRotationEndToHour, CreateRequirementSnapshotOnRotationEndToMinute, 0);
            }
        }

        public TimeSpan ClientDataUploadStartTime
        {
            get
            {
                return new TimeSpan(ClientDataUploadFromHour, ClientDataUploadFromMinute, 0);
            }
        }

        public TimeSpan ClientDataUploadEndTime
        {
            get
            {
                return new TimeSpan(ClientDataUploadToHour, ClientDataUploadToMinute, 0);
            }
        }

        //UAT-2603
        public TimeSpan RotationDataMovementStartTime
        {
            get
            {
                return new TimeSpan(RotationDataMovementFromHour, RotationDataMovementFromMinute, 0);
            }
        }

        public TimeSpan RotationDataMovementEndTime
        {
            get
            {
                return new TimeSpan(RotationDataMovementToHour, RotationDataMovementToMinute, 0);
            }
        }

        #region UAT-3112

        public TimeSpan BadgeFormNotificationStartTime
        {
            get
            {
                return new TimeSpan(BadgeFormNotificationFromHour, BadgeFormNotificationFromMinute, 0);
            }
        }
        public TimeSpan BadgeFormNotificationEndTime
        {
            get
            {

                return new TimeSpan(BadgeFormNotificationToHour, BadgeFormNotificationToMinute, 0);
            }
        }

        #endregion

        #region UAT-3669

        public TimeSpan AlertMailForWebCCFErrorStartTime
        {
            get
            {
                return new TimeSpan(AlertMailForWebCCFErrorFromHour, AlertMailForWebCCFErrorFromMinute, 0);
            }
        }
        public TimeSpan AlertMailForWebCCFErrorEndTime
        {
            get
            {

                return new TimeSpan(AlertMailForWebCCFErrorToHour, AlertMailForWebCCFErrorToMinute, 0);
            }
        }

        #endregion

        #region UAT-2960

        public TimeSpan AlumniAccessNotificationStartTime
        {
            get
            {
                return new TimeSpan(AlumniAccessNotificationFromHour, AlumniAccessNotificationFromMinute, 0);
            }
        }
        public TimeSpan AlumniAccessNotificationEndTime
        {
            get
            {

                return new TimeSpan(AlumniAccessNotificationToHour, AlumniAccessNotificationToMinute, 0);
            }
        }

        //UAT-2960-Copy Compliance to Compliance

        public TimeSpan CopyComplianceToComplianceStartTime
        {
            get
            {
                return new TimeSpan(CopyComplianceToComplianceFromHour, CopyComplianceToComplianceFromMinute, 0);
            }
        }

        public TimeSpan CopyComplianceToComplianceEndTime
        {
            get
            {
                return new TimeSpan(CopyComplianceToComplianceToHour, CopyComplianceToComplianceToMinute, 0);
            }
        }



        public TimeSpan BkgCopyPackageDataStartTime
        {
            get
            {
                return new TimeSpan(BkgCopyPackageDataFromHour, BkgCopyPackageDataFromMinute, 0);
            }
        }

        public TimeSpan BkgCopyPackageDataEndTime
        {
            get
            {
                return new TimeSpan(BkgCopyPackageDataToHour, BkgCopyPackageDataToMinute, 0);
            }
        }

        //UAT-3820
        public TimeSpan ReceivedFromStuServFormStatusDataStartTime
        {
            get
            {
                return new TimeSpan(ReceivedFromStuServFormStatusFromHour, ReceivedFromStuServFormStatusDataFromMinute, 0);
            }
        }

        public TimeSpan ReceivedFromStuServFormStatuDataEndTime
        {
            get
            {
                return new TimeSpan(ReceivedFromStuServFormStatusDataToHour, ReceivedFromStuServFormStatusDataToMinute, 0);
            }
        }

        //UAT-3485
        public TimeSpan SendRotationAbtToExpireStartTime
        {
            get
            {
                return new TimeSpan(SendRotationAbtToExpireEmailFromHour, SendRotationAbtToExpireEmailFromMinute, 0);
            }
        }
        public TimeSpan SendRotationAbtToExpireEndTime
        {
            get
            {
                return new TimeSpan(SendRotationAbtToExpireEmailToHour, SendRotationAbtToExpireEmailToMinute, 0);
            }
        }
        public TimeSpan BkgDigestionProcedureStartTime
        {
            get
            {
                return new TimeSpan(BkgProcedureCallFromHour, BkgProcedureCallFromMinute, 0);
            }
        }
        public TimeSpan BkgDigestionProcedureEndTime
        {
            get
            {
                return new TimeSpan(BkgProcedureCallToHour, BkgProcedureCallToMinute, 0);
            }
        }

        //UAT-3734
        #region  Missed Appointment Start-End Time

        public TimeSpan MissedAppointmentEmailStartTime
        {
            get
            {
                return new TimeSpan(MissedAppointmentEmailFromHour, MissedAppointmentEmailFromMinute, 0);
            }
        }
        public TimeSpan MissedAppointmentEmailEndTime
        {
            get
            {
                return new TimeSpan(MissedAppointmentEmailToHour, MissedAppointmentEmailToMinute, 0);
            }
        }

        public TimeSpan MissedAppointmentStartTime
        {
            get
            {
                return new TimeSpan(MissedAppointmentFromHour, MissedAppointmentFromMinute, 0);
            }
        }
        public TimeSpan MissedAppointmentEndTime
        {
            get
            {
                return new TimeSpan(MissedAppointmentToHour, MissedAppointmentToMinute, 0);
            }
        }

        #endregion

        public TimeSpan OffTimeRevokedAppointmentEmailStartTime
        {
            get
            {
                return new TimeSpan(OffTimeRevokedAppointmentEmailFromHour, OffTimeRevokedAppointmentEmailFromMinute, 0);
            }
        }
        public TimeSpan OffTimeRevokedAppointmentEmailEndTime
        {
            get
            {
                return new TimeSpan(OffTimeRevokedAppointmentEmailToHour, OffTimeRevokedAppointmentEmailToMinute, 0);
            }
        }

        #region To control the number of email send in the defined time interval.

        Int32 _emailThreshold = 100;
        public Int32 emailThreshold
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("emailThreshold"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["emailThreshold"]) ? ConfigurationManager.AppSettings["emailThreshold"] : _emailThreshold.ToString());
                else
                    return _emailThreshold;
            }
        }

        Int32 _EmailTimeLimit = 3600;
        public Int32 EmailTimeLimit
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("EmailTimeLimit"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["EmailTimeLimit"]) ? ConfigurationManager.AppSettings["EmailTimeLimit"] : _EmailTimeLimit.ToString());
                else
                    return _EmailTimeLimit;
            }
        }

        Int32 _isEmailThresholdRequired = 0;
        public Int32 isEmailThresholdRequired
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("isEmailThresholdRequired"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["isEmailThresholdRequired"]) ? ConfigurationManager.AppSettings["isEmailThresholdRequired"] : _isEmailThresholdRequired.ToString());
                else
                    return _isEmailThresholdRequired;
            }
        }

        String ToAddressesformailcountexceeded = ConfigurationManager.AppSettings["ToAddressesformailcountexceeded"].IsNotNull() ?
                                                               Convert.ToString(ConfigurationManager.AppSettings["ToAddressesformailcountexceeded"])
                                                              : String.Empty;
        List<String> ToAddressesformailcountexceededList = new List<String>();
        #endregion

        #endregion


        #region Send 20 days old Draft Order Notification to Admin

        public TimeSpan SendDarftOrderNotificationtoAdminStartTime
        {
            get
            {
                return new TimeSpan(SendDarftOrderNotificationtoAdminFromHour, SendDarftOrderNotificationtoAdminFromMinute, 0);
            }
        }
        public TimeSpan SendDarftOrderNotificationtoAdminEndTime
        {
            get
            {
                return new TimeSpan(SendDarftOrderNotificationtoAdminToHour, SendDarftOrderNotificationtoAdminToMinute, 0);
            }
        }
        private Int32 _SendDarftOrderNotificationtoAdminInterval = 86400000;
        public Int32 SendDarftOrderNotificationtoAdminInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("SendDarftOrderNotificationtoAdminInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["SendDarftOrderNotificationtoAdminInterval"])
                        ? ConfigurationManager.AppSettings["SendDarftOrderNotificationtoAdminInterval"] : _SendDarftOrderNotificationtoAdminInterval.ToString());
                else
                    return _SendDarftOrderNotificationtoAdminInterval;
            }
        }

        private Int32 _SendDarftOrderNotificationtoAdminFromHour = 0;
        public Int32 SendDarftOrderNotificationtoAdminFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("SendDarftOrderNotificationtoAdminFromHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["SendDarftOrderNotificationtoAdminFromHour"])
                        ? ConfigurationManager.AppSettings["SendDarftOrderNotificationtoAdminFromHour"] : _SendDarftOrderNotificationtoAdminFromHour.ToString());
                else
                    return _SendDarftOrderNotificationtoAdminFromHour;
            }
        }

        private Int32 _SendDarftOrderNotificationtoAdminFromMinute = 0;
        public Int32 SendDarftOrderNotificationtoAdminFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("SendDarftOrderNotificationtoAdminFromMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["SendDarftOrderNotificationtoAdminFromMinute"]) ? ConfigurationManager.AppSettings["SendDarftOrderNotificationtoAdminFromMinute"] : _SendDarftOrderNotificationtoAdminFromMinute.ToString());
                else
                    return _SendDarftOrderNotificationtoAdminFromMinute;
            }
        }

        private Int32 _SendDarftOrderNotificationtoAdminToHour = 24;
        public Int32 SendDarftOrderNotificationtoAdminToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("SendDarftOrderNotificationtoAdminToHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["SendDarftOrderNotificationtoAdminToHour"]) ? ConfigurationManager.AppSettings["SendDarftOrderNotificationtoAdminToHour"] : _SendDarftOrderNotificationtoAdminToHour.ToString());
                else
                    return _SendDarftOrderNotificationtoAdminToHour;
            }
        }

        private Int32 _SendDarftOrderNotificationtoAdminToMinute = 0;
        public Int32 SendDarftOrderNotificationtoAdminToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("SendDarftOrderNotificationtoAdminToMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["SendDarftOrderNotificationtoAdminToMinute"]) ? ConfigurationManager.AppSettings["SendDarftOrderNotificationtoAdminToMinute"] : _SendDarftOrderNotificationtoAdminToMinute.ToString());
                else
                    return _SendDarftOrderNotificationtoAdminToMinute;
            }
        }




        #endregion

        #region Send 20 days old Draft Order Notification to Admin

        private Boolean isSendDarftOrderNotificationtoAdminInterval = false;
        private Timer SendDarftOrderNotificationtoAdminTimer = null;
        private Boolean isSendDarftOrderNotificationtoAdminExecuting = false;
        private Object lockSendDarftOrderNotificationtoAdminObject = new Object();


        void SendDarftOrderNotificationtoAdmin_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!isSendDarftOrderNotificationtoAdminInterval)
            {
                SendDarftOrderNotificationtoAdminTimer.Interval = Convert.ToDouble(SendDarftOrderNotificationtoAdminInterval);
                isSendDarftOrderNotificationtoAdminInterval = true;
            }
            TimeSpan CurrentTime = DateTime.Now.TimeOfDay;

            logger.Info("Timer invoked for Send Darft Order Notification to Admin");
            lock (this.lockSendDarftOrderNotificationtoAdminObject)
            {
                if (isSendDarftOrderNotificationtoAdminExecuting)
                {
                    logger.Info("Send Darft Order Notification to Admin was already in progress. Thread id {0} will exit now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                    return;
                }
                logger.Info("Thread id {0} will execute Send Darft Order Notification to Admin now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isSendDarftOrderNotificationtoAdminExecuting = true;
            }

            logger.Info("Entered processing for Send Darft Order Notification to Admin");
            try
            {
                if (CurrentTime >= SendDarftOrderNotificationtoAdminStartTime && CurrentTime <= SendDarftOrderNotificationtoAdminEndTime)
                {
                    logger.Trace("******************* START Calling Method Send Darft Order Notification to Admin  : " + DateTime.Now.ToString() + " *******************");

                    AdminEntryPortalJob.SendMailDarftOrderNotificationtoAdmin();

                    logger.Trace("******************* END Calling Method for Send Darft Order Notification to Admin  : " + DateTime.Now.ToString() + " *******************");
                }
                else
                {
                    logger.Trace("******************* Exit Send Darft Order Notification to Admin " + DateTime.Now.ToString() + " *******************");
                }
            }
            catch (Exception ex)
            {
                logger.Info("Error in Send Darft Order Notification to Admin Thread id {0} will exit Rotation Category required Notification Before Going To Be Required.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isSendDarftOrderNotificationtoAdminExecuting = false;
                logger.Error("An Error has occured in Send Darft Order Notification to Admin , the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                logger.Info("Send Darft Order Notification to Admin  complete. Thread id {0} will exit Send Darft Order Notification to Admin.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isSendDarftOrderNotificationtoAdminExecuting = false;
            }
        }

        #endregion

        #region Send 20 days after Invitation Send Order to Applicant

        public TimeSpan SendInvitationPendingOrderNotificationtoApplicantStartTime
        {
            get
            {
                return new TimeSpan(SendInvitationPendingOrderNotificationtoApplicantFromHour, SendInvitationPendingOrderNotificationtoApplicantFromMinute, 0);
            }
        }
        public TimeSpan SendInvitationPendingOrderNotificationtoApplicantEndTime
        {
            get
            {
                return new TimeSpan(SendInvitationPendingOrderNotificationtoApplicantToHour, SendInvitationPendingOrderNotificationtoApplicantToMinute, 0);
            }
        }
        private Int32 _SendInvitationPendingOrderNotificationtoApplicantInterval = 86400000;
        public Int32 SendInvitationPendingOrderNotificationtoApplicantInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("SendInvitationPendingOrderNotificationtoApplicantInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["SendInvitationPendingOrderNotificationtoApplicantInterval"]) ? ConfigurationManager.AppSettings["SendInvitationPendingOrderNotificationtoApplicantInterval"] : _SendInvitationPendingOrderNotificationtoApplicantInterval.ToString());
                else
                    return _SendInvitationPendingOrderNotificationtoApplicantInterval;
            }
        }

        private Int32 _SendInvitationPendingOrderNotificationtoApplicantFromHour = 0;
        public Int32 SendInvitationPendingOrderNotificationtoApplicantFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("SendInvitationPendingOrderNotificationtoApplicantFromHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["SendInvitationPendingOrderNotificationtoApplicantFromHour"]) ? ConfigurationManager.AppSettings["SendInvitationPendingOrderNotificationtoApplicantFromHour"] : _SendInvitationPendingOrderNotificationtoApplicantFromHour.ToString());
                else
                    return _SendInvitationPendingOrderNotificationtoApplicantFromHour;
            }
        }

        private Int32 _SendInvitationPendingOrderNotificationtoApplicantFromMinute = 0;
        public Int32 SendInvitationPendingOrderNotificationtoApplicantFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("SendInvitationPendingOrderNotificationtoApplicantFromMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["SendDarftOrderNotificationtoAdminFromMinute"]) ? ConfigurationManager.AppSettings["SendInvitationPendingOrderNotificationtoApplicantFromMinute"] : _SendInvitationPendingOrderNotificationtoApplicantFromMinute.ToString());
                else
                    return _SendInvitationPendingOrderNotificationtoApplicantFromMinute;
            }
        }

        private Int32 _SendInvitationPendingOrderNotificationtoApplicantToHour = 24;
        public Int32 SendInvitationPendingOrderNotificationtoApplicantToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("SendInvitationPendingOrderNotificationtoApplicantToHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["SendInvitationPendingOrderNotificationtoApplicantToHour"]) ? ConfigurationManager.AppSettings["SendInvitationPendingOrderNotificationtoApplicantToHour"] : _SendInvitationPendingOrderNotificationtoApplicantToHour.ToString());
                else
                    return _SendInvitationPendingOrderNotificationtoApplicantToHour;
            }
        }

        private Int32 _SendInvitationPendingOrderNotificationtoApplicantToMinute = 0;
        public Int32 SendInvitationPendingOrderNotificationtoApplicantToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("SendInvitationPendingOrderNotificationtoApplicantToMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["SendInvitationPendingOrderNotificationtoApplicantToMinute"]) ? ConfigurationManager.AppSettings["SendInvitationPendingOrderNotificationtoApplicantToMinute"] : _SendInvitationPendingOrderNotificationtoApplicantToMinute.ToString());
                else
                    return _SendInvitationPendingOrderNotificationtoApplicantToMinute;
            }
        }

        //private Int32 _SendInvitationPendingOrderNotificationtoApplicantDaysOlder = 0;

        //public Int32 SendInvitationPendingOrderNotificationtoApplicantDaysOlder
        //{
        //    get
        //    {
        //        if (ConfigurationManager.AppSettings.AllKeys.Contains("SendInvitationPendingOrderNotificationtoApplicantDaysOlder"))
        //            return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["SendInvitationPendingOrderNotificationtoApplicantToMinute"]) ? ConfigurationManager.AppSettings["SendInvitationPendingOrderNotificationtoApplicantDaysOlder"] : _SendInvitationPendingOrderNotificationtoApplicantDaysOlder.ToString());
        //        else
        //            return _SendInvitationPendingOrderNotificationtoApplicantDaysOlder;
        //    }
        //}


        #endregion

        #region Send 20 days after Invitation Send Order to Applicant

        private Boolean isSendInvitationPendingOrderNotificationtoApplicantInterval = false;
        private Timer SendInvitationPendingOrderNotificationtoApplicantTimer = null;
        private Boolean isSendInvitationPendingOrderNotificationtoApplicantExecuting = false;
        private Object lockSendInvitationPendingOrderNotificationtoApplicantObject = new Object();


        void SendInvitationPendingOrderNotificationtoApplicant_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!isSendInvitationPendingOrderNotificationtoApplicantInterval)
            {
                SendInvitationPendingOrderNotificationtoApplicantTimer.Interval = Convert.ToDouble(SendInvitationPendingOrderNotificationtoApplicantInterval);
                isSendInvitationPendingOrderNotificationtoApplicantInterval = true;
            }
            TimeSpan CurrentTime = DateTime.Now.TimeOfDay;

            logger.Info("Timer invoked for Send Invitation Pending Order Notification to Applicant");
            lock (this.lockSendInvitationPendingOrderNotificationtoApplicantObject)
            {
                if (isSendInvitationPendingOrderNotificationtoApplicantExecuting)
                {
                    logger.Info("Send Invitation Pending Order Notification to Applicant was already in progress. Thread id {0} will exit now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                    return;
                }
                logger.Info("Thread id {0} will execute Send Invitation Pending Order Notification to Applicant now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isSendInvitationPendingOrderNotificationtoApplicantExecuting = true;
            }

            logger.Info("Entered processing for Send Invitation Pending Order Notification to Applicant");
            try
            {
                if (CurrentTime >= SendInvitationPendingOrderNotificationtoApplicantStartTime && CurrentTime <= SendInvitationPendingOrderNotificationtoApplicantEndTime)
                {
                    logger.Trace("******************* START Calling Method Send Invitation Pending Order Notification to Applicant  : " + DateTime.Now.ToString() + " *******************");

                    AdminEntryPortalJob.SendMailPendingInvitaiontoApplicant();

                    logger.Trace("******************* END Calling Method for Send Invitation Pending Order Notification to Applicant  : " + DateTime.Now.ToString() + " *******************");
                }
                else
                {
                    logger.Trace("******************* Exit Send Invitation Pending Order Notification to Applicant " + DateTime.Now.ToString() + " *******************");
                }
            }
            catch (Exception ex)
            {
                logger.Info("Error in Send Invitation Pending Order Notification to Applicant Thread id {0}", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isSendInvitationPendingOrderNotificationtoApplicantExecuting = false;
                logger.Error("An Error has occured in Send Invitation Pending Order Notification to Applicant , the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                logger.Info("Send Invitation Pending Order Notification to Applicant complete. Thread id {0} ", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isSendInvitationPendingOrderNotificationtoApplicantExecuting = false;
            }
        }

        #endregion

        #region Delete Draft Status Order 30 days older

        public TimeSpan DeleteDraftOrderStatusStartTime
        {
            get
            {
                return new TimeSpan(DeleteDraftOrderStatusFromHour, DeleteDraftOrderStatusFromMinute, 0);
            }
        }
        public TimeSpan DeleteDraftOrderStatusEndTime
        {
            get
            {
                return new TimeSpan(DeleteDraftOrderStatusToHour, DeleteDraftOrderStatusToMinute, 0);
            }
        }
        private Int32 _DeleteDraftOrderStatusInterval = 86400000;
        public Int32 DeleteDraftOrderStatusInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("DeleteDraftOrderStatusInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["DeleteDraftOrderStatusInterval"]) ? ConfigurationManager.AppSettings["DeleteDraftOrderStatusInterval"] : _DeleteDraftOrderStatusInterval.ToString());
                else
                    return _DeleteDraftOrderStatusInterval;
            }
        }

        private Int32 _DeleteDraftOrderStatusFromHour = 0;
        public Int32 DeleteDraftOrderStatusFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("DeleteDraftOrderStatusFromHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["DeleteDraftOrderStatusFromHour"]) ? ConfigurationManager.AppSettings["DeleteDraftOrderStatusFromHour"] : _DeleteDraftOrderStatusFromHour.ToString());
                else
                    return _DeleteDraftOrderStatusFromHour;
            }
        }

        private Int32 _DeleteDraftOrderStatusFromMinute = 0;
        public Int32 DeleteDraftOrderStatusFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("DeleteDraftOrderStatusFromMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["DeleteDraftOrderStatusFromMinute"]) ? ConfigurationManager.AppSettings["DeleteDraftOrderStatusFromMinute"] : _DeleteDraftOrderStatusFromMinute.ToString());
                else
                    return _DeleteDraftOrderStatusFromMinute;
            }
        }

        private Int32 _DeleteDraftOrderStatusToHour = 24;
        public Int32 DeleteDraftOrderStatusToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("DeleteDraftOrderStatusToHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["DeleteDraftOrderStatusToHour"]) ? ConfigurationManager.AppSettings["DeleteDraftOrderStatusToHour"] : _DeleteDraftOrderStatusToHour.ToString());
                else
                    return _DeleteDraftOrderStatusToHour;
            }
        }

        private Int32 _DeleteDraftOrderStatusToMinute = 0;
        public Int32 DeleteDraftOrderStatusToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("DeleteDraftOrderStatusToMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["DeleteDraftOrderStatusToMinute"]) ? ConfigurationManager.AppSettings["DeleteDraftOrderStatusToMinute"] : _DeleteDraftOrderStatusToMinute.ToString());
                else
                    return _DeleteDraftOrderStatusToMinute;
            }
        }

        //private Int32 _DeleteDraftOrderStatusDaysOlder = 0;

        //public Int32 DeleteDraftOrderStatusDaysOlder
        //{
        //    get
        //    {
        //        if (ConfigurationManager.AppSettings.AllKeys.Contains("DeleteDraftOrderStatusDaysOlder"))
        //            return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["DeleteDraftOrderStatusToMinute"]) ? ConfigurationManager.AppSettings["DeleteDraftOrderStatusDaysOlder"] : _DeleteDraftOrderStatusDaysOlder.ToString());
        //        else
        //            return _DeleteDraftOrderStatusDaysOlder;
        //    }
        //}


        #endregion

        #region Delete Draft Status Order 30 days older

        private Boolean isDeleteDraftOrderStatusInterval = false;
        private Timer DeleteDraftOrderStatusTimer = null;
        private Boolean isDeleteDraftOrderStatusExecuting = false;
        private Object lockDeleteDraftOrderStatusObject = new Object();


        void DeleteDraftOrderStatus_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!isDeleteDraftOrderStatusInterval)
            {
                DeleteDraftOrderStatusTimer.Interval = Convert.ToDouble(DeleteDraftOrderStatusInterval);
                isDeleteDraftOrderStatusInterval = true;
            }
            TimeSpan CurrentTime = DateTime.Now.TimeOfDay;

            logger.Info("Timer invoked for Delete Draft Order Status");
            lock (this.lockDeleteDraftOrderStatusObject)
            {
                if (isDeleteDraftOrderStatusExecuting)
                {
                    logger.Info("Delete Draft Order Status was already in progress. Thread id {0} will exit now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                    return;
                }
                logger.Info("Thread id {0} will execute Delete Draft Order Status.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isDeleteDraftOrderStatusExecuting = true;
            }

            logger.Info("Entered processing for Delete Draft Order Status");
            try
            {
                if (CurrentTime >= DeleteDraftOrderStatusStartTime && CurrentTime <= DeleteDraftOrderStatusEndTime)
                {
                    logger.Trace("******************* START Calling Method DeleteDraftOrder  : " + DateTime.Now.ToString() + " *******************");

                    AdminEntryPortalJob.DeleteDraftOrder();

                    logger.Trace("******************* END Calling Method for DeleteDraftOrder  : " + DateTime.Now.ToString() + " *******************");
                }
                else
                {
                    logger.Trace("******************* Exit Delete Draft Order " + DateTime.Now.ToString() + " *******************");
                }
            }
            catch (Exception ex)
            {
                logger.Info("Error in Delete Draft Order Status Thread id {0} ", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isDeleteDraftOrderStatusExecuting = false;
                logger.Error("An Error has occured in Send Invitation Pending Order Notification to Applicant , the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                logger.Info("Delete Draft Order Status complete. Thread id {0} will exit Delete Draft Order Status.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isDeleteDraftOrderStatusExecuting = false;
            }
        }

        #endregion

        #region Change Order Status from Complete to Archived

        public TimeSpan ChangeOrderStatusCompleteToArchivedStartTime
        {
            get
            {
                return new TimeSpan(ChangeOrderStatusCompleteToArchivedFromHour, ChangeOrderStatusCompleteToArchivedFromMinute, 0);
            }
        }
        public TimeSpan ChangeOrderStatusCompleteToArchivedEndTime
        {
            get
            {
                return new TimeSpan(ChangeOrderStatusCompleteToArchivedToHour, ChangeOrderStatusCompleteToArchivedToMinute, 0);
            }
        }
        private Int32 _ChangeOrderStatusCompleteToArchivedInterval = 86400000;
        public Int32 ChangeOrderStatusCompleteToArchivedInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ChangeOrderStatusCompleteToArchivedInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ChangeOrderStatusCompleteToArchivedInterval"])
                        ? ConfigurationManager.AppSettings["ChangeOrderStatusCompleteToArchivedInterval"] : _ChangeOrderStatusCompleteToArchivedInterval.ToString());
                else
                    return _ChangeOrderStatusCompleteToArchivedInterval;
            }
        }

        private Int32 _ChangeOrderStatusCompleteToArchivedFromHour = 0;
        public Int32 ChangeOrderStatusCompleteToArchivedFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ChangeOrderStatusCompleteToArchivedFromHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ChangeOrderStatusCompleteToArchivedFromHour"])
                        ? ConfigurationManager.AppSettings["ChangeOrderStatusCompleteToArchivedFromHour"] : _ChangeOrderStatusCompleteToArchivedFromHour.ToString());
                else
                    return _ChangeOrderStatusCompleteToArchivedFromHour;
            }
        }

        private Int32 _ChangeOrderStatusCompleteToArchivedFromMinute = 0;
        public Int32 ChangeOrderStatusCompleteToArchivedFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ChangeOrderStatusCompleteToArchivedFromMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ChangeOrderStatusCompleteToArchivedFromMinute"])
                        ? ConfigurationManager.AppSettings["ChangeOrderStatusCompleteToArchivedFromMinute"] : _ChangeOrderStatusCompleteToArchivedFromMinute.ToString());
                else
                    return _ChangeOrderStatusCompleteToArchivedFromMinute;
            }
        }

        private Int32 _ChangeOrderStatusCompleteToArchivedToHour = 24;
        public Int32 ChangeOrderStatusCompleteToArchivedToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ChangeOrderStatusCompleteToArchivedToHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ChangeOrderStatusCompleteToArchivedToHour"])
                        ? ConfigurationManager.AppSettings["ChangeOrderStatusCompleteToArchivedToHour"] : _ChangeOrderStatusCompleteToArchivedToHour.ToString());
                else
                    return _ChangeOrderStatusCompleteToArchivedToHour;
            }
        }

        private Int32 _ChangeOrderStatusCompleteToArchivedToMinute = 0;
        public Int32 ChangeOrderStatusCompleteToArchivedToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ChangeOrderStatusCompleteToArchivedToMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ChangeOrderStatusCompleteToArchivedToMinute"])
                        ? ConfigurationManager.AppSettings["ChangeOrderStatusCompleteToArchivedToMinute"] : _ChangeOrderStatusCompleteToArchivedToMinute.ToString());
                else
                    return _ChangeOrderStatusCompleteToArchivedToMinute;
            }
        }

        //private Int32 _ChangeOrderStatusCompleteToArchivedDefaultDaysOlder = 0;

        //public Int32 ChangeOrderStatusCompleteToArchivedDefaultDaysOlder
        //{
        //    get
        //    {
        //        if (ConfigurationManager.AppSettings.AllKeys.Contains("ChangeOrderStatusCompleteToArchivedDefaultDaysOlder"))
        //            return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ChangeOrderStatusCompleteToArchivedDefaultDaysOlder"])
        //                ? ConfigurationManager.AppSettings["ChangeOrderStatusCompleteToArchivedDefaultDaysOlder"] : _ChangeOrderStatusCompleteToArchivedDefaultDaysOlder.ToString());
        //        else
        //            return _ChangeOrderStatusCompleteToArchivedDefaultDaysOlder;
        //    }
        //}


        #endregion

        #region Change Order Status from Complete to Archived

        private Boolean isChangeOrderStatusCompleteToArchivedInterval = false;
        private Timer ChangeOrderStatusCompleteToArchivedTimer = null;
        private Boolean isChangeOrderStatusCompleteToArchivedExecuting = false;
        private Object lockChangeOrderStatusCompleteToArchivedObject = new Object();


        void ChangeOrderStatusCompleteToArchived_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!isChangeOrderStatusCompleteToArchivedInterval)
            {
                ChangeOrderStatusCompleteToArchivedTimer.Interval = Convert.ToDouble(ChangeOrderStatusCompleteToArchivedInterval);
                isChangeOrderStatusCompleteToArchivedInterval = true;
            }
            TimeSpan CurrentTime = DateTime.Now.TimeOfDay;

            logger.Info("Timer invoked for Change Order Status From Complete to Archived.");
            lock (this.lockChangeOrderStatusCompleteToArchivedObject)
            {
                if (isChangeOrderStatusCompleteToArchivedExecuting)
                {
                    logger.Info("Change Order Status From Complete to Archived was already in progress. Thread id {0} will exit now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                    return;
                }
                logger.Info("Thread id {0} will execute Change Order Status From Complete to Archived.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isChangeOrderStatusCompleteToArchivedExecuting = true;
            }

            logger.Info("Entered processing for DChange Order Status From Complete to Archived.");
            try
            {
                if (CurrentTime >= ChangeOrderStatusCompleteToArchivedStartTime && CurrentTime <= ChangeOrderStatusCompleteToArchivedEndTime)
                {
                    logger.Trace("******************* START Calling Method Order Status From Complete to Archived  : " + DateTime.Now.ToString() + " *******************");

                    AdminEntryPortalJob.ChangeOrdersStatusCompletedToArchived();

                    logger.Trace("******************* END Calling Method for Order Status From Complete to Archived  : " + DateTime.Now.ToString() + " *******************");
                }
                else
                {
                    logger.Trace("******************* Exit Order Status From Complete to Archived " + DateTime.Now.ToString() + " *******************");
                }
            }
            catch (Exception ex)
            {
                logger.Info("Error in Order Status From Complete to Archived Thread id {0}.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isChangeOrderStatusCompleteToArchivedExecuting = false;
                logger.Error("An Error has occured in Order Status From Complete to Archived, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                logger.Info("Order Status From Complete to Archived. Thread id {0} will exit Delete Draft Order Status.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isChangeOrderStatusCompleteToArchivedExecuting = false;
            }
        }

        #endregion

        #region BKG Completed Status Order to Client Admin Notification

        public TimeSpan BKgCompletedOrderToClientAdminStartTime
        {
            get
            {
                return new TimeSpan(BKgCompletedOrderToClientAdminFromHour, BKgCompletedOrderToClientAdminFromMinute, 0);
            }
        }
        public TimeSpan BKgCompletedOrderToClientAdminEndTime
        {
            get
            {
                return new TimeSpan(BKgCompletedOrderToClientAdminToHour, BKgCompletedOrderToClientAdminToMinute, 0);
            }
        }
        private Int32 _BKgCompletedOrderToClientAdminInterval = 86400000;
        public Int32 BKgCompletedOrderToClientAdminInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("BKgCompletedOrderToClientAdminInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["BKgCompletedOrderToClientAdminInterval"]) ? ConfigurationManager.AppSettings["BKgCompletedOrderToClientAdminInterval"] : _BKgCompletedOrderToClientAdminInterval.ToString());
                else
                    return _BKgCompletedOrderToClientAdminInterval;
            }
        }

        private Int32 _BKgCompletedOrderToClientAdminFromHour = 0;
        public Int32 BKgCompletedOrderToClientAdminFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("BKgCompletedOrderToClientAdminFromHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["BKgCompletedOrderToClientAdminFromHour"]) ? ConfigurationManager.AppSettings["BKgCompletedOrderToClientAdminFromHour"] : _BKgCompletedOrderToClientAdminFromHour.ToString());
                else
                    return _BKgCompletedOrderToClientAdminFromHour;
            }
        }

        private Int32 _BKgCompletedOrderToClientAdminFromMinute = 0;
        public Int32 BKgCompletedOrderToClientAdminFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("BKgCompletedOrderToClientAdminFromMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["BKgCompletedOrderToClientAdminFromMinute"]) ? ConfigurationManager.AppSettings["BKgCompletedOrderToClientAdminFromMinute"] : _BKgCompletedOrderToClientAdminFromMinute.ToString());
                else
                    return _BKgCompletedOrderToClientAdminFromMinute;
            }
        }

        private Int32 _BKgCompletedOrderToClientAdminToHour = 24;
        public Int32 BKgCompletedOrderToClientAdminToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("BKgCompletedOrderToClientAdminToHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["BKgCompletedOrderToClientAdminToHour"]) ? ConfigurationManager.AppSettings["BKgCompletedOrderToClientAdminToHour"] : _BKgCompletedOrderToClientAdminToHour.ToString());
                else
                    return _BKgCompletedOrderToClientAdminToHour;
            }
        }

        private Int32 _BKgCompletedOrderToClientAdminToMinute = 0;
        public Int32 BKgCompletedOrderToClientAdminToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("BKgCompletedOrderToClientAdminToMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["BKgCompletedOrderToClientAdminToMinute"]) ? ConfigurationManager.AppSettings["BKgCompletedOrderToClientAdminToMinute"] : _BKgCompletedOrderToClientAdminToMinute.ToString());
                else
                    return _BKgCompletedOrderToClientAdminToMinute;
            }
        }




        #endregion

        #region BKG Completed Status Order to Client Admin Notification



        void BKgCompletedOrderToClientAdmin_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!isBKgCompletedOrderToClientAdminInterval)
            {
                BKgCompletedOrderToClientAdminTimer.Interval = Convert.ToDouble(BKgCompletedOrderToClientAdminInterval);
                isBKgCompletedOrderToClientAdminInterval = true;
            }
            TimeSpan CurrentTime = DateTime.Now.TimeOfDay;

            logger.Info("Timer invoked for BKG Completed Status Order to Client Admin Notification");
            lock (this.lockBKgCompletedOrderToClientAdminObject)
            {
                if (isBKgCompletedOrderToClientAdminExecuting)
                {
                    logger.Info("BKG Completed Status Order to Client Admin Notification was already in progress. Thread id {0} will exit now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                    return;
                }
                logger.Info("Thread id {0} will execute BKG Completed Status Order to Client Admin Notification.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isBKgCompletedOrderToClientAdminExecuting = true;
            }

            logger.Info("Entered processing for BKG Completed Status Order to Client Admin Notification");
            try
            {
                if (CurrentTime >= BKgCompletedOrderToClientAdminStartTime && CurrentTime <= BKgCompletedOrderToClientAdminEndTime)
                {
                    logger.Trace("******************* START Calling Method SendMailBkgSvcGroupCompletedOrderNotificationtoAdmin  : " + DateTime.Now.ToString() + " *******************");

                    AdminEntryPortalJob.SendMailBkgSvcGroupCompletedOrderNotificationtoAdmin();

                    logger.Trace("******************* END Calling Method for SendMailBkgSvcGroupCompletedOrderNotificationtoAdmin  : " + DateTime.Now.ToString() + " *******************");
                }
                else
                {
                    logger.Trace("******************* Exit Send Mail BkgSvcGroupCompleted OrderNotificationtoAdmin " + DateTime.Now.ToString() + " *******************");
                }
            }
            catch (Exception ex)
            {
                logger.Info("Error in BKG Completed Status Order to Client Admin Notification Thread id {0}.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isBKgCompletedOrderToClientAdminExecuting = false;
                logger.Error("An Error has occured in BKG Completed Status Order to Client Admin Notification , the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                logger.Info("BKG Completed Status Order to Client Admin Notification complete. Thread id {0} will exit BKG Completed Status Order to Client Admin Notification.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isBKgCompletedOrderToClientAdminExecuting = false;
            }
        }

        #endregion

        #region BKG Completed Status Order with Attached PDF to Client Admin Notification

        public TimeSpan BKgCompletedOrderWithAttachedPDFToClientAdminStartTime
        {
            get
            {
                return new TimeSpan(BKgCompletedOrderWithAttachedPDFToClientAdminFromHour, BKgCompletedOrderWithAttachedPDFToClientAdminFromMinute, 0);
            }
        }
        public TimeSpan BKgCompletedOrderWithAttachedPDFToClientAdminEndTime
        {
            get
            {
                return new TimeSpan(BKgCompletedOrderWithAttachedPDFToClientAdminToHour, BKgCompletedOrderWithAttachedPDFToClientAdminToMinute, 0);
            }
        }
        private Int32 _BKgCompletedOrderWithAttachedPDFToClientAdminInterval = 86400000;
        public Int32 BKgCompletedOrderWithAttachedPDFToClientAdminInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("BKgCompletedOrderWithAttachedPDFToClientAdminInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["BKgCompletedOrderWithAttachedPDFToClientAdminInterval"]) ? ConfigurationManager.AppSettings["BKgCompletedOrderWithAttachedPDFToClientAdminInterval"] : _BKgCompletedOrderWithAttachedPDFToClientAdminInterval.ToString());
                else
                    return _BKgCompletedOrderWithAttachedPDFToClientAdminInterval;
            }
        }

        private Int32 _BKgCompletedOrderWithAttachedPDFToClientAdminFromHour = 0;
        public Int32 BKgCompletedOrderWithAttachedPDFToClientAdminFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("BKgCompletedOrderWithAttachedPDFToClientAdminFromHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["BKgCompletedOrderWithAttachedPDFToClientAdminFromHour"]) ? ConfigurationManager.AppSettings["BKgCompletedOrderWithAttachedPDFToClientAdminFromHour"] : _BKgCompletedOrderWithAttachedPDFToClientAdminFromHour.ToString());
                else
                    return _BKgCompletedOrderWithAttachedPDFToClientAdminFromHour;
            }
        }

        private Int32 _BKgCompletedOrderWithAttachedPDFToClientAdminFromMinute = 0;
        public Int32 BKgCompletedOrderWithAttachedPDFToClientAdminFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("BKgCompletedOrderWithAttachedPDFToClientAdminFromMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["BKgCompletedOrderWithAttachedPDFToClientAdminFromMinute"]) ? ConfigurationManager.AppSettings["BKgCompletedOrderWithAttachedPDFToClientAdminFromMinute"] : _BKgCompletedOrderWithAttachedPDFToClientAdminFromMinute.ToString());
                else
                    return _BKgCompletedOrderWithAttachedPDFToClientAdminFromMinute;
            }
        }

        private Int32 _BKgCompletedOrderWithAttachedPDFToClientAdminToHour = 24;
        public Int32 BKgCompletedOrderWithAttachedPDFToClientAdminToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("BKgCompletedOrderWithAttachedPDFToClientAdminToHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["BKgCompletedOrderWithAttachedPDFToClientAdminToHour"]) ? ConfigurationManager.AppSettings["BKgCompletedOrderWithAttachedPDFToClientAdminToHour"] : _BKgCompletedOrderWithAttachedPDFToClientAdminToHour.ToString());
                else
                    return _BKgCompletedOrderWithAttachedPDFToClientAdminToHour;
            }
        }

        private Int32 _BKgCompletedOrderWithAttachedPDFToClientAdminToMinute = 0;
        public Int32 BKgCompletedOrderWithAttachedPDFToClientAdminToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("BKgCompletedOrderWithAttachedPDFToClientAdminToMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["BKgCompletedOrderWithAttachedPDFToClientAdminToMinute"]) ? ConfigurationManager.AppSettings["BKgCompletedOrderWithAttachedPDFToClientAdminToMinute"] : _BKgCompletedOrderWithAttachedPDFToClientAdminToMinute.ToString());
                else
                    return _BKgCompletedOrderWithAttachedPDFToClientAdminToMinute;
            }
        }




        #endregion

        #region BKG Completed Status Order with Attached PDF to Client Admin Notification

        private Boolean isBKgCompletedOrderWithAttachedPDFToClientAdminInterval = false;
        private Timer BKgCompletedOrderWithAttachedPDFToClientAdminTimer = null;
        private Boolean isBKgCompletedOrderWithAttachedPDFToClientAdminExecuting = false;
        private Object lockBKgCompletedOrderWithAttachedPDFToClientAdminObject = new Object();


        void BKgCompletedOrderWithAttachedPDFToClientAdmin_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!isBKgCompletedOrderWithAttachedPDFToClientAdminInterval)
            {
                BKgCompletedOrderWithAttachedPDFToClientAdminTimer.Interval = Convert.ToDouble(BKgCompletedOrderWithAttachedPDFToClientAdminInterval);
                isBKgCompletedOrderWithAttachedPDFToClientAdminInterval = true;
            }
            TimeSpan CurrentTime = DateTime.Now.TimeOfDay;

            logger.Info("Timer invoked for BKG Completed Status Order with Attached PDF to Client Admin Notification");
            lock (this.lockBKgCompletedOrderWithAttachedPDFToClientAdminObject)
            {
                if (isBKgCompletedOrderWithAttachedPDFToClientAdminExecuting)
                {
                    logger.Info("BKG Completed Status Order with Attached PDF to Client Admin  Notification was already in progress. Thread id {0} will exit now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                    return;
                }
                logger.Info("Thread id {0} will execute BKG Completed Status Order with Attached PDF to Client Admin  Notification.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isBKgCompletedOrderWithAttachedPDFToClientAdminExecuting = true;
            }

            logger.Info("Entered processing for BKG Completed Status Order with Attached PDF to Client Admin  Notification");
            try
            {
                if (CurrentTime >= BKgCompletedOrderWithAttachedPDFToClientAdminStartTime && CurrentTime <= BKgCompletedOrderWithAttachedPDFToClientAdminEndTime)
                {
                    logger.Trace("******************* START Calling Method BKgCompletedOrderWithAttachedPDFToClientAdmin  : " + DateTime.Now.ToString() + " *******************");

                    AdminEntryPortalJob.SendMailCompletedOrderWithAttachmentNotificationtoAdmin();

                    logger.Trace("******************* END Calling Method for BKgCompletedOrderWithAttachedPDFToClientAdmin  : " + DateTime.Now.ToString() + " *******************");
                }
                else
                {
                    logger.Trace("******************* Exit BKG Completed Status Order with Attached PDF to Client Admin " + DateTime.Now.ToString() + " *******************");
                }
            }
            catch (Exception ex)
            {
                logger.Info("Error in BKG Completed Status Order with Attached PDF to Client Admin  Notification Thread id {0} .", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isBKgCompletedOrderWithAttachedPDFToClientAdminExecuting = false;
                logger.Error("An Error has occured in BKG Completed Status Order with Attached PDF to Client Admin , the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                logger.Info("BKG Completed Status Order with Attached PDF to Client Admin  Notification complete. Thread id {0} will exit BKG Completed Status Order with Attached PDF to Client Admin Notification.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isBKgCompletedOrderWithAttachedPDFToClientAdminExecuting = false;
            }
        }

        #endregion


        #region UAT-4613

        private Boolean isNotificationForServiceFormInIPAStatusFromStudentInterval = false;
        private Timer notificationForServiceFormInIPAStatusFromStudentTimer = null;
        private Boolean isNotificationForServiceFormInIPAStatusFromStudentExecuting = false;
        private Object lockbkgNotificationForServiceFormInIPAStatusFromStudentObject = new Object();
        public TimeSpan NotificationForServiceFormInIPAStatusFromStudentStartTime
        {
            get
            {
                return new TimeSpan(NotificationForServiceFormInIPAStatusFromStudentFromHour, NotificationForServiceFormInIPAStatusFromStudentFromMinute, 0);
            }
        }
        public TimeSpan NotificationForServiceFormInIPAStatusFromStudentEndTime
        {
            get
            {
                return new TimeSpan(NotificationForServiceFormInIPAStatusFromStudentToHour, NotificationForServiceFormInIPAStatusFromStudentToMinute, 0);
            }
        }

        private Int32 _notificationForServiceFormInIPAStatusFromStudentInterval = 86400000;
        public Int32 NotificationForServiceFormInIPAStatusFromStudentInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("NotificationForServiceFormInIPAStatusFromStudentInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["NotificationForServiceFormInIPAStatusFromStudentInterval"])
                        ? ConfigurationManager.AppSettings["NotificationForServiceFormInIPAStatusFromStudentInterval"] : _notificationForServiceFormInIPAStatusFromStudentInterval.ToString());
                else
                    return _notificationForServiceFormInIPAStatusFromStudentInterval;
            }
        }

        private Int32 _notificationForServiceFormInIPAStatusFromStudentFromHour = 0;
        public Int32 NotificationForServiceFormInIPAStatusFromStudentFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("NotificationForServiceFormInIPAStatusFromStudentFromHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["NotificationForServiceFormInIPAStatusFromStudentFromHour"])
                        ? ConfigurationManager.AppSettings["NotificationForServiceFormInIPAStatusFromStudentFromHour"] : _notificationForServiceFormInIPAStatusFromStudentFromHour.ToString());
                else
                    return _notificationForServiceFormInIPAStatusFromStudentFromHour;
            }
        }

        private Int32 _notificationForServiceFormInIPAStatusFromStudentFromMinute = 0;
        public Int32 NotificationForServiceFormInIPAStatusFromStudentFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("NotificationForServiceFormInIPAStatusFromStudentFromMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["NotificationForServiceFormInIPAStatusFromStudentFromMinute"])
                        ? ConfigurationManager.AppSettings["NotificationForServiceFormInIPAStatusFromStudentFromMinute"] : _notificationForServiceFormInIPAStatusFromStudentFromMinute.ToString());
                else
                    return _notificationForServiceFormInIPAStatusFromStudentFromMinute;
            }
        }

        private Int32 _notificationForServiceFormInIPAStatusFromStudentToHour = 24;
        public Int32 NotificationForServiceFormInIPAStatusFromStudentToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("NotificationForServiceFormInIPAStatusFromStudentToHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["NotificationForServiceFormInIPAStatusFromStudentToHour"])
                        ? ConfigurationManager.AppSettings["NotificationForServiceFormInIPAStatusFromStudentToHour"] : _notificationForServiceFormInIPAStatusFromStudentToHour.ToString());
                else
                    return _notificationForServiceFormInIPAStatusFromStudentToHour;
            }
        }

        private Int32 _notificationForServiceFormInIPAStatusFromStudentToMinute = 0;
        public Int32 NotificationForServiceFormInIPAStatusFromStudentToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("NotificationForServiceFormInIPAStatusFromStudentToMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["NotificationForServiceFormInIPAStatusFromStudentToMinute"])
                        ? ConfigurationManager.AppSettings["NotificationForServiceFormInIPAStatusFromStudentToMinute"] : _notificationForServiceFormInIPAStatusFromStudentToMinute.ToString());
                else
                    return _notificationForServiceFormInIPAStatusFromStudentToMinute;
            }
        }



        void NotificationForServiceFormInIPAStatusFromStudent_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!isNotificationForServiceFormInIPAStatusFromStudentInterval)
            {
                notificationForServiceFormInIPAStatusFromStudentTimer.Interval = Convert.ToDouble(NotificationForServiceFormInIPAStatusFromStudentInterval);
                isNotificationForServiceFormInIPAStatusFromStudentInterval = true;
            }

            TimeSpan CurrentTime = DateTime.Now.TimeOfDay;

            logger.Info("Timer invoked for SendNotificationForOrderServiceStatusInProcessAgency");
            lock (this.lockbkgNotificationForServiceFormInIPAStatusFromStudentObject)
            {
                if (isNotificationForServiceFormInIPAStatusFromStudentExecuting)
                {
                    logger.Info("Send mail for in process agency status form student service form status was already in progress. Thread id {0} will exit now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                    return;
                }
                logger.Info("Thread id {0} will execute SendNotificationForOrderServiceStatusInProcessAgency now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isNotificationForServiceFormInIPAStatusFromStudentExecuting = true;

            }
            logger.Info("Entered processing for SendNotificationForOrderServiceStatusInProcessAgency");
            try
            {

                if (CurrentTime >= NotificationForServiceFormInIPAStatusFromStudentStartTime && CurrentTime <= NotificationForServiceFormInIPAStatusFromStudentEndTime)
                {
                    logger.Trace("******************* START Calling Method SendNotificationForOrderServiceStatusInProcessAgency: " + DateTime.Now.ToString() + " *******************");
                    OrderNotification.SendNotificationForOrderServiceStatusInProcessAgency();
                    logger.Trace("******************* END Calling Method for SendNotificationForOrderServiceStatusInProcessAgency : " + DateTime.Now.ToString() + " *******************");
                }
                else
                {
                    logger.Trace("******************* Exit Copy SendNotificationForOrderServiceStatusInProcessAgency" + DateTime.Now.ToString() + " *******************");
                }
            }
            catch (Exception ex)
            {
                logger.Info("Error in Thread id {0} will exit SendNotificationForOrderServiceStatusInProcessAgency.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isNotificationForServiceFormInIPAStatusFromStudentExecuting = false;
                logger.Error("An Error has occured in SendNotificationForOrderServiceStatusInProcessAgency, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                logger.Info("SendNotificationForOrderServiceStatusInProcessAgency complete. Thread id {0} will exit SendNotificationForOrderServiceStatusInProcessAgency now.", System.Threading.Thread.CurrentThread.ManagedThreadId);
                isNotificationForServiceFormInIPAStatusFromStudentExecuting = false;
            }
        }
        #endregion
    }
}
