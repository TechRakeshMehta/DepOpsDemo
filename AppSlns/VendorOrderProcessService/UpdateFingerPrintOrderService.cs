using Business.RepoManagers;
using Entity;
using ExternalVendors;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.Utils;
using INTSOF.ServiceUtil;
using WinSCP;
using INTSOF.UI.Contract.FingerPrintSetup;
using System.Xml;
using System.Web.Configuration;
using System.IO;
using Entity.ClientEntity;
using INTSOF.UI.Contract.Services;
using System.Text.RegularExpressions;

namespace VendorOrderProcessService
{
    public static class UpdateFingerPrintOrderService
    {
        #region UpDateFingerPrintOrderService

        #region Variables

        private static String UpdateFingerPrintOrderLogger;
        private static Boolean _updateAllFingerPrintOrders;
        private static Boolean _isServiceLoggingEnabled;
        private static String ChangeStatusForUpdatedCBIResultFileServiceLogger;
        private static String CBIFingerprintRecieptServiceLogger;

        #region SFTP Configuration

        private static String FingerPrintFTPUsername { get; set; }
        private static String FingerPrintFTPPassword { get; set; }
        private static String FingerPrintFTPHostName { get; set; }
        private static String FingerPrintFTPPortNumber { get; set; }
        private static String FingerPrintFTPRemotePath { get; set; }
        private static String FingerPrintFTPSshHostKeyFingerprint { get; set; }
        private static String FingerPrintFTPAcceptAnySSHHostKey { get; set; }
        private static Boolean FingerPrintFTPIsArchiveDisable { get; set; }

        private static String ApplicantFingerPrintImages { get; set; }

        #endregion



        static UpdateFingerPrintOrderService()
        {
            UpdateFingerPrintOrderLogger = "UpdateFingerPrintOrderLogger";
            ChangeStatusForUpdatedCBIResultFileServiceLogger = "ChangeStatusForUpdatedCBIResultFileServiceLogger";
            CBIFingerprintRecieptServiceLogger = "CBIFingerprintRecieptServiceLogger";

            if ((ConfigurationManager.AppSettings["UpdateAllFingerPrintOrders"].IsNotNull()))
            {
                _updateAllFingerPrintOrders = Convert.ToBoolean(ConfigurationManager.AppSettings["UpdateAllFingerPrintOrders"]);
            }

            if ((ConfigurationManager.AppSettings["FingerPrintFTPUsername"].IsNotNull()))
            {
                FingerPrintFTPUsername = Convert.ToString(ConfigurationManager.AppSettings["FingerPrintFTPUsername"]);
            }

            if ((ConfigurationManager.AppSettings["FingerPrintFTPPassword"].IsNotNull()))
            {
                FingerPrintFTPPassword = Convert.ToString(ConfigurationManager.AppSettings["FingerPrintFTPPassword"]);
            }

            if ((ConfigurationManager.AppSettings["FingerPrintFTPHostName"].IsNotNull()))
            {
                FingerPrintFTPHostName = Convert.ToString(ConfigurationManager.AppSettings["FingerPrintFTPHostName"]);
            }

            if ((ConfigurationManager.AppSettings["FingerPrintFTPPortNumber"].IsNotNull()))
            {
                FingerPrintFTPPortNumber = Convert.ToString(ConfigurationManager.AppSettings["FingerPrintFTPPortNumber"]);
            }

            if ((ConfigurationManager.AppSettings["FingerPrintFTPRemotePath"].IsNotNull()))
            {
                FingerPrintFTPRemotePath = Convert.ToString(ConfigurationManager.AppSettings["FingerPrintFTPRemotePath"]);
            }
            if ((ConfigurationManager.AppSettings["FingerPrintFTPSshHostKeyFingerprint"].IsNotNull()))
            {
                FingerPrintFTPSshHostKeyFingerprint = Convert.ToString(ConfigurationManager.AppSettings["FingerPrintFTPSshHostKeyFingerprint"]);
            }

            if ((ConfigurationManager.AppSettings["FingerPrintFTPAcceptAnySSHHostKey"].IsNotNull()))
            {
                FingerPrintFTPAcceptAnySSHHostKey = Convert.ToString(ConfigurationManager.AppSettings["FingerPrintFTPAcceptAnySSHHostKey"]);
            }

            if ((ConfigurationManager.AppSettings["FingerPrintFTPIsArchiveDisable"].IsNotNull()))
            {
                FingerPrintFTPIsArchiveDisable = Convert.ToBoolean(ConfigurationManager.AppSettings["FingerPrintFTPIsArchiveDisable"]);
            }

            if (ConfigurationManager.AppSettings["IsServiceLoggingEnabled"].IsNotNull())
            {
                _isServiceLoggingEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["IsServiceLoggingEnabled"]);
            }
            else
            {
                _isServiceLoggingEnabled = false;
            }

            if ((ConfigurationManager.AppSettings["ApplicantFingerPrintImages"].IsNotNull()))
            {
                ApplicantFingerPrintImages = Convert.ToString(ConfigurationManager.AppSettings["ApplicantFingerPrintImages"]);
            }
        }

        #endregion

        #region Properties

        #endregion

        #region Methods


        public static void UpdateFingerPrintOrder(Int32? tenant_Id = null)
        {
            try
            {
                // ServiceContext.init();
                ServiceLogger.Info("Calling UpdateFingerPrintOrder: " + DateTime.Now.ToString(), UpdateFingerPrintOrderLogger);

                Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                Int32 backgroundProcessUserId = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.BACKGROUND_PROCESS_USER_VALUE;

                List<ClientDBConfiguration> clientDbConfs = SecurityManager.GetClientDBConfiguration().Select(
                item =>
                {
                    ClientDBConfiguration config = new ClientDBConfiguration();
                    config.CDB_TenantID = item.CDB_TenantID;
                    config.CDB_ConnectionString = item.CDB_ConnectionString;
                    config.Tenant = new Entity.Tenant();
                    config.Tenant.TenantName = item.Tenant.TenantName; return config;
                }).ToList();

                ServiceLogger.Debug<List<ClientDBConfiguration>>("UpdateFingerPrintOrder: List of Client DbConfigurations from database:", clientDbConfs, UpdateFingerPrintOrderLogger);
                if (clientDbConfs != null && clientDbConfs.Count > 0 && tenant_Id != null)
                {
                    ServiceLogger.Debug<Int32?>("UpdateFingerPrintOrder: TenantID for which external vendor orders are to be dispatched:", tenant_Id, UpdateFingerPrintOrderLogger);
                    clientDbConfs = clientDbConfs.Where(x => x.CDB_TenantID == tenant_Id).ToList();
                }
                ServiceLogger.Info("UpdateFingerPrintOrder: Started foreach loop on ClientDbConfigurations: " + DateTime.Now.ToString(), UpdateFingerPrintOrderLogger);
                var lcoationtenants = SecurityManager.GetListOfTenantWithLocationService().Select(x => x.TenantID);
                var loctiontenantconfig = clientDbConfs.Where(x => lcoationtenants.Contains(x.CDB_TenantID));
                foreach (ClientDBConfiguration cltDbConf in loctiontenantconfig)
                {
                    ServiceLogger.Info("UpdateFingerPrintOrder: Check if location service available Tenant: " + DateTime.Now.ToString(), UpdateFingerPrintOrderLogger);
                    if (!cltDbConf.IsNullOrEmpty() && SecurityManager.IsLocationServiceTenant(cltDbConf.CDB_TenantID))
                    {
                        if (CheckDBConnection.TestConnString(cltDbConf.CDB_ConnectionString, UpdateFingerPrintOrderLogger))
                        {
                            DateTime jobStartTime = DateTime.Now;
                            DateTime jobEndTime;
                            Int32 tenantId = cltDbConf.CDB_TenantID;
                            ServiceLogger.Info("UpdateFingerPrintOrder: Started Update ALL Fingerprint service orderss and getAllInprocessOrders at. " + DateTime.Now.ToString(),
                                        UpdateFingerPrintOrderLogger);

                            UpdateFingerPrintCompletedOrders(tenantId, backgroundProcessUserId);

                            ServiceLogger.Info("UpdateFingerPrintOrder: End Update ALL AMS InProcess orders and getAllInprocessOrders at. " + DateTime.Now.ToString(),
                                UpdateFingerPrintOrderLogger);

                            //Save service logging data to DB
                            if (_isServiceLoggingEnabled)
                            {
                                jobEndTime = DateTime.Now;
                                ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                                serviceLoggingContract.ServiceName = ServiceName.BkgOrderService.GetStringValue();
                                serviceLoggingContract.JobName = JobName.UpdateFingerPrintOrder.GetStringValue();
                                serviceLoggingContract.TenantID = tenantId;
                                serviceLoggingContract.JobStartTime = jobStartTime;
                                serviceLoggingContract.JobEndTime = jobEndTime;
                                //serviceLoggingContract.Comments = "";
                                serviceLoggingContract.IsDeleted = false;
                                serviceLoggingContract.CreatedBy = backgroundProcessUserId;
                                serviceLoggingContract.CreatedOn = DateTime.Now;
                                SecurityManager.SaveServiceLoggingDetail(serviceLoggingContract);
                            }
                        }
                    }
                    ServiceContext.ReleaseDBContextItems();
                }
            }
            catch (Exception ex)
            {
                ServiceLogger.Error(String.Format("An Error has occured in UpdateFingerPrintOrder method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}",
                                    ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString),
                                    UpdateFingerPrintOrderLogger);
            }
        }

        private static void UpdateFingerPrintCompletedOrders(Int32 tenantId, Int32 bkgProcessUserID)
        {
            SessionOptions sessionOptions = new SessionOptions();

            //Call to set SFTP Session
            Protocol Protocol = Protocol.Sftp;
            sessionOptions = SetSFTPSessionOption(FingerPrintFTPHostName, FingerPrintFTPPortNumber, FingerPrintFTPUsername, FingerPrintFTPPassword, Protocol);

            if (!sessionOptions.IsNullOrEmpty())
            {
                Boolean IsFileSavedSuccessfully = SaveSftpfileInStaging(sessionOptions, tenantId, bkgProcessUserID);
            }

            List<String> lstFileNamesInStaging = FingerPrintDataManager.GetFingerPrintDocStaging(tenantId);
            if (!lstFileNamesInStaging.IsNullOrEmpty())
            {
                String remotePath = FingerPrintFTPRemotePath;
                CreateApplicantDocumentForInProgressOrders(tenantId, lstFileNamesInStaging, sessionOptions, remotePath, bkgProcessUserID);
            }
        }

        private static SessionOptions SetSFTPSessionOption(String hostName, String portNumber, String userName, String password, Protocol protocol)
        {
            SessionOptions sessionOptions = new SessionOptions();

            if (!protocol.IsNullOrEmpty() && !userName.IsNullOrEmpty() && !password.IsNullOrEmpty())
            {
                sessionOptions.HostName = hostName;
                sessionOptions.PortNumber = Convert.ToInt32(portNumber);
                sessionOptions.Protocol = protocol;
                sessionOptions.UserName = userName;
                sessionOptions.Password = password;
                if (Convert.ToBoolean(FingerPrintFTPAcceptAnySSHHostKey) == false)
                {
                    sessionOptions.SshHostKeyFingerprint = FingerPrintFTPSshHostKeyFingerprint;
                }
                else
                {
                    sessionOptions.GiveUpSecurityAndAcceptAnySshHostKey = true;
                }
            }
            return sessionOptions;
        }

        public static Boolean SaveSftpfileInStaging(SessionOptions sessionOptions, Int32 tenantId, Int32 bkgProcessUserID)
        {

            Boolean IsFilesSaved = false;
            try
            {
                using (Session session = new Session())
                {
                    session.Open(sessionOptions);
                    if (session.Opened)
                    {
                        bool exists = session.FileExists(FingerPrintFTPRemotePath);
                        TransferOptions transferOption = new TransferOptions();
                        transferOption.TransferMode = TransferMode.Binary;
                        RemoteDirectoryInfo dirInfo = session.ListDirectory(FingerPrintFTPRemotePath);


                        if (!dirInfo.IsNullOrEmpty())
                        {
                            List<String> lstFileName = dirInfo.Files.Select(Sel => Sel.Name).ToList();
                            if (!lstFileName.IsNullOrEmpty())
                            {
                                List<String> lstFileNameToSave = new List<String>();
                                foreach (String fileName in lstFileName)
                                {
                                    if (fileName.ToString() != "." && fileName.ToString() != "..")
                                    {
                                        lstFileNameToSave.Add(fileName);
                                    }
                                }
                                //Save the above list in DB --- FingerPrintDocumentStatging
                                if (FingerPrintDataManager.SaveFingerPrintDocumentsStaging(tenantId, lstFileNameToSave, bkgProcessUserID))
                                {
                                    IsFilesSaved = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return IsFilesSaved;
        }

        private static void CreateApplicantDocumentForInProgressOrders(Int32 tenantId, List<String> lstFileNamesInStaging, SessionOptions sessionOptions, String remotePath, Int32 bkgProcessUserID)
        {
            //Get List of Completed Orders From DB --
            List<String> lstFingerPrintDocStagingToDelete = new List<String>();
            DateTime _currentTimeStamp = DateTime.Now;
            Int32 completedOrderStatusTypeId = LookupManager.GetLookUpData<Entity.ClientEntity.lkpOrderStatusType>(tenantId).FirstOrDefault(x => x.Code == "AAAC").OrderStatusTypeID;
            List<FingerPrintOrderContract> lstFingerPrintOrderContract = FingerPrintDataManager.GetInProgressFingerPrintOrders(tenantId, completedOrderStatusTypeId);  //CompletedOrderList
           
            //Get All the file Names for the completed Orders
            if (!lstFingerPrintOrderContract.IsNullOrEmpty())
            {
                foreach (FingerPrintOrderContract fingerPrintOrderContract in lstFingerPrintOrderContract)
                {
                    String orderCompletedfileName = String.Empty;
                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(fingerPrintOrderContract.OrderResultXML);
                    XmlNodeList nodeList = xml.SelectNodes("xmldata/FingerprintData");
                    foreach (XmlNode node in nodeList)
                    {
                        if (node.IsNotNull())
                        {
                            orderCompletedfileName = node["FileName"].InnerText;
                        }
                    }

                    if (!orderCompletedfileName.IsNullOrEmpty())
                    {
                        if (lstFileNamesInStaging.Any(x => x.ToLower() == orderCompletedfileName.ToLower()))
                        {
                            orderCompletedfileName = lstFileNamesInStaging.Where(x => x.ToLower() == orderCompletedfileName.ToLower()).FirstOrDefault();
                            fingerPrintOrderContract.ApplicantDocument = new ApplicantDocument();

                            String filePath = String.Empty;
                            Boolean aWSUseS3 = false;
                            StringBuilder corruptedFileMessage = new StringBuilder();
                            String tempFilePath = WebConfigurationManager.AppSettings["TemporaryFileLocation"];
                            filePath = WebConfigurationManager.AppSettings[AppConsts.APPLICANT_FILE_LOCATION];
                            StringBuilder docMessage = new StringBuilder();
                            byte[] fileBytes = null;
                            String applicantDocPath = String.Empty;
                            String FinalFileName = String.Empty;

                            if (!WebConfigurationManager.AppSettings["AWSUseS3"].IsNullOrEmpty())
                            {
                                aWSUseS3 = Convert.ToBoolean(WebConfigurationManager.AppSettings["AWSUseS3"]);
                            }
                            //Check whether use AWS S3, true if need to use
                            if (aWSUseS3 == false)
                            {
                                if (!filePath.EndsWith("\\"))
                                {
                                    filePath += "\\";
                                }

                                filePath += "Tenant(" + tenantId.ToString() + @")\";

                                if (!Directory.Exists(filePath))
                                    Directory.CreateDirectory(filePath);
                            }
                            else
                            {
                                if (!filePath.EndsWith("//"))
                                {
                                    filePath += "//";
                                }

                                filePath = filePath + "Tenant(" + tenantId.ToString() + @")/";
                            }

                            String fileName = Guid.NewGuid().ToString() + Path.GetExtension(orderCompletedfileName);
                            String newTempFilePath = Path.Combine(tempFilePath, "Tenant(" + tenantId.ToString() + @")"); // Created Temporary File Path
                            if (!Directory.Exists(newTempFilePath))
                                Directory.CreateDirectory(newTempFilePath);

                            //Get file from sftp to local path.
                            //using (Session session = new Session())
                            //{
                            //    session.Open(sessionOptions);
                            //    session.GetFiles(remotePath + "/", newTempFilePath, false, new TransferOptions { FileMask = remotePath + "/" + orderCompletedfileName });
                            //    //fileBytes = File.ReadAllBytes(newTempFilePath);
                            //    session.Dispose();
                            //}

                            using (Session session1 = new Session())
                            {
                                TransferOptions transferOptions = new TransferOptions();
                                transferOptions.TransferMode = TransferMode.Binary;
                                transferOptions.FileMask = remotePath + "/" + orderCompletedfileName;
                                session1.Open(sessionOptions);
                                session1.GetFiles(remotePath + "/", newTempFilePath, false, transferOptions);
                                session1.Dispose();
                            }


                            newTempFilePath = newTempFilePath + "\\" + orderCompletedfileName;
                            fileBytes = File.ReadAllBytes(newTempFilePath);
                            //FileStream fileStream = new FileStream(newTempFilePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                            //var _totalBytes = new System.IO.FileInfo(newTempFilePath).Length;
                            //BinaryReader _binaryReader = new BinaryReader(fileStream);
                            //var buffer = _binaryReader.ReadBytes((Int32)_totalBytes);

                            FinalFileName = Path.GetFileNameWithoutExtension(orderCompletedfileName) + "_" + fingerPrintOrderContract.ApplicantOrgUserID + "_" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + "_" + DateTime.Now.Minute + DateTime.Now.Millisecond + Path.GetExtension(orderCompletedfileName);

                            if (!fileBytes.IsNullOrEmpty())
                            {
                                //File.WriteAllBytes(newTempFilePath, sftpFileBytes);

                                if (aWSUseS3 == false)
                                {
                                    //Move file to other location
                                    String destFilePath = Path.Combine(filePath, FinalFileName);
                                    File.Copy(newTempFilePath, destFilePath);                                    
                                    applicantDocPath = destFilePath;
                                }
                                else
                                {
                                    if (!filePath.EndsWith("//"))
                                    {
                                        filePath += "//";
                                    }
                                    //AWS code to save document to S3 location
                                    AmazonS3Documents objAmazonS3 = new AmazonS3Documents();
                                    String destFolder = filePath + "Tenant(" + tenantId.ToString() + @")/";
                                    String returnFilePath = objAmazonS3.SaveDocument(newTempFilePath, FinalFileName, destFolder);
                                    if (returnFilePath.IsNullOrEmpty())
                                    {
                                        corruptedFileMessage.Append("Your file " + orderCompletedfileName + " is not uploaded. \\n");
                                        continue;
                                    }
                                    applicantDocPath = returnFilePath;
                                }

                                if (!fingerPrintOrderContract.SkipABIReview)
                                {
                                    //var applicantImages = SaveImagesFromFingerPrintFiles(newTempFilePath,tenantId, fingerPrintOrderContract.AADID.ToString() , aWSUseS3);
                                    
                                    var applicantImages = FPFileApplicantImage.SaveImagesFromFingerPrintFiles(newTempFilePath, tenantId, fingerPrintOrderContract.AADID.ToString(),ApplicantFingerPrintImages,tempFilePath, aWSUseS3);

                                    if (applicantImages.Any())
                                    {
                                        fingerPrintOrderContract.ApplicantFingerPrintFileImages = new List<ApplicantFingerPrintFileImageContract>();
                                        foreach(var images in applicantImages)
                                        {
                                            fingerPrintOrderContract.ApplicantFingerPrintFileImages.Add(new ApplicantFingerPrintFileImageContract
                                            {
                                                AFFI_ApplicantAppointmentDetailID = fingerPrintOrderContract.AADID,
                                                AFFI_FileName = Path.GetFileName(images),
                                                AFFI_FilePath = images                                                
                                            });
                                        }
                                    }
                                }

                                try
                                {
                                    if (!String.IsNullOrEmpty(newTempFilePath))
                                        File.Delete(newTempFilePath);
                                }
                                catch (Exception) { }
                            }

                            //Add Applicant documents.
                            String docTypeCode = DislkpDocumentType.FINGERPRINT_DOCUMENT.GetStringValue();
                            var documentType = LookupManager.GetLookUpData<Entity.ClientEntity.lkpDocumentType>(tenantId).FirstOrDefault(x => x.DMT_Code == docTypeCode && !x.DMT_IsDeleted);

                            fingerPrintOrderContract.ApplicantDocument.DocumentPath = applicantDocPath;
                            fingerPrintOrderContract.ApplicantDocument.OrganizationUserID = fingerPrintOrderContract.ApplicantOrgUserID;
                            fingerPrintOrderContract.ApplicantDocument.FileName = FinalFileName;
                            fingerPrintOrderContract.ApplicantDocument.DocumentType = documentType.IsNullOrEmpty() ? AppConsts.NONE : documentType.DMT_ID;
                            fingerPrintOrderContract.ApplicantDocument.Description = "Fingerprint result document for the applicant bkg fingerprint order";
                            fingerPrintOrderContract.ApplicantDocument.OriginalDocMD5Hash = fileBytes.IsNullOrEmpty() ? null : CommonFileManager.GetMd5Hash(fileBytes);
                            fingerPrintOrderContract.ApplicantDocument.Size = fileBytes.IsNullOrEmpty() ? AppConsts.NONE : fileBytes.Length;
                            fingerPrintOrderContract.ApplicantDocument.CreatedByID = bkgProcessUserID;
                            fingerPrintOrderContract.ApplicantDocument.CreatedOn = _currentTimeStamp;
                            fingerPrintOrderContract.ApplicantDocument.IsDeleted = false;

                            //lstApplicantDocs.Add(applicantDoc);
                            lstFingerPrintDocStagingToDelete.Add(orderCompletedfileName);
                        }
                    }

                }

                //To Save Applicant Documents In database
                ServiceLogger.Info("UpdateFingerPrintOrder: Started SaveFingerprintApplicantDocument at. " + DateTime.Now.ToString(),
                                    UpdateFingerPrintOrderLogger);
                if (FingerPrintDataManager.SaveFingerprintApplicantDocument(tenantId, lstFingerPrintOrderContract.Where(x => x.ApplicantDocument != null).ToList(), bkgProcessUserID))
                {
                    ServiceLogger.Info("UpdateFingerPrintOrder: SaveFingerprintApplicantDocument is Successfull at. " + DateTime.Now.ToString(),
                                        UpdateFingerPrintOrderLogger);

                    // Delete documents Staging.
                    if (!lstFingerPrintDocStagingToDelete.IsNullOrEmpty())
                    {
                        ServiceLogger.Info("UpdateFingerPrintOrder: Started DeleteFingerPrintDocStaging at. " + DateTime.Now.ToString(),
                                    UpdateFingerPrintOrderLogger);
                        if (FingerPrintDataManager.DeleteFingerPrintDocStaging(tenantId, lstFingerPrintDocStagingToDelete, bkgProcessUserID))
                        {
                            ServiceLogger.Info("UpdateFingerPrintOrder: DeleteFingerPrintDocStaging is Successfull at. " + DateTime.Now.ToString(),
                                        UpdateFingerPrintOrderLogger);

                            // Move Files created succssfully from sftp folder to sftp Archive Folder.
                            if (FingerPrintFTPIsArchiveDisable.IsNullOrEmpty() || !FingerPrintFTPIsArchiveDisable)
                            {
                                ServiceLogger.Info("UpdateFingerPrintOrder: Started MoveSavedDocumentsToArhciveFolder() at. " + DateTime.Now.ToString(),
                                        UpdateFingerPrintOrderLogger);
                                try
                                {
                                    MoveSavedDocumentsToArhciveFolder(lstFingerPrintDocStagingToDelete, remotePath);
                                    {
                                        ServiceLogger.Info("UpdateFingerPrintOrder:" + lstFingerPrintDocStagingToDelete.Count() + " - " + "document(s) moved to archive folder Successfully at. " + DateTime.Now.ToString(), UpdateFingerPrintOrderLogger);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    ServiceLogger.Info("UpdateFingerPrintOrder: Error occured in MoveSavedDocumentsToArhciveFolder() at " + DateTime.Now.ToString() + ". The Exception Message is-" + ex.Message, UpdateFingerPrintOrderLogger);
                                }
                            }
                        }
                        else
                        {
                            ServiceLogger.Info("UpdateFingerPrintOrder: DeleteFingerPrintDocStaging is not Successful. Some error might occurred at. " + DateTime.Now.ToString(),
                                                               UpdateFingerPrintOrderLogger);
                        }
                    }

                    //Map Applicant Document id in Client database -- ApplicantAppointmentDetails
                    ServiceLogger.Info("UpdateFingerPrintOrder: Started AddDocInApplicantAppointmentDetail at. " + DateTime.Now.ToString(),
                                   UpdateFingerPrintOrderLogger);
                    if (FingerPrintDataManager.AddDocInApplicantAppointmentDetail(tenantId, lstFingerPrintOrderContract.Where(x => x.ApplicantDocument != null).ToList(), bkgProcessUserID))
                    {
                        ServiceLogger.Info("UpdateFingerPrintOrder: AddDocInApplicantAppointmentDetail is Successfull at. " + DateTime.Now.ToString(),
                                    UpdateFingerPrintOrderLogger);
                    }
                    else
                    {
                        ServiceLogger.Info("UpdateFingerPrintOrder: AddDocInApplicantAppointmentDetail is not Successful. Some error might occurred at. " + DateTime.Now.ToString(),
                                                                                      UpdateFingerPrintOrderLogger);
                    }
                }

                else
                {
                    ServiceLogger.Info("UpdateFingerPrintOrder: SaveFingerprintApplicantDocument is not Successful. Some error might occurred at. " + DateTime.Now.ToString(),
                                                       UpdateFingerPrintOrderLogger);
                }
            }
        }

        


        // Method To Move files to archive folder on sftp//
        private static void MoveSavedDocumentsToArhciveFolder(List<String> lstFingerPrintDocStagingToDelete, String remotePath)
        {
            if (!lstFingerPrintDocStagingToDelete.IsNullOrEmpty())
            {
                SessionOptions sessionOptions = new SessionOptions();

                //Call to set SFTP Session
                Protocol Protocol = Protocol.Sftp;
                sessionOptions = SetSFTPSessionOption(FingerPrintFTPHostName, FingerPrintFTPPortNumber, FingerPrintFTPUsername, FingerPrintFTPPassword, Protocol);
                if (!sessionOptions.IsNullOrEmpty())
                {
                    using (Session session = new Session())
                    {
                        try
                        {
                            session.Open(sessionOptions);
                            if (!remotePath.IsNullOrEmpty() && session.Opened)
                            {
                                String archiveRemotePath = remotePath + "/" + "Archived/";
                                try
                                {
                                    if (!CreateArchiveFolderIfNotExist(session, archiveRemotePath)) return;
                                }
                                catch (Exception)
                                {
                                    ServiceLogger.Info("UpdateFingerPrintOrder: Error occurred in MoveSavedDocumentsToArhciveFolder() while creating the directory or listing the directory." + DateTime.Now.ToString(),
                                                             UpdateFingerPrintOrderLogger);
                                }

                                foreach (String fileName in lstFingerPrintDocStagingToDelete.Distinct().ToList())
                                {
                                    String filePath = remotePath + "/" + fileName;
                                    try
                                    {
                                        if (session.FileExists(filePath))
                                        {
                                            session.MoveFile(filePath, archiveRemotePath);

                                        }
                                    }
                                    catch (Exception)
                                    {
                                        ServiceLogger.Info("UpdateFingerPrintOrder: Error occurred in MoveSavedDocumentsToArhciveFolder() while moving the file to archive folder." + DateTime.Now.ToString(),
                                                                 UpdateFingerPrintOrderLogger);
                                    }
                                }
                            }
                        }
                        finally
                        {
                            session.Dispose();
                        }
                    }
                }
            }
        }

        private static Boolean CreateArchiveFolderIfNotExist(Session session, String archiveRemotePath)
        {
            Boolean result = false;
            RemoteDirectoryInfo dirInfo = null;
            try
            {
                dirInfo = session.ListDirectory(archiveRemotePath);
                result = true;
            }
            catch (Exception)
            {


            }
            try
            {
                if (dirInfo == null)
                    session.CreateDirectory(archiveRemotePath);
                dirInfo = session.ListDirectory(archiveRemotePath);
                if (dirInfo.IsNotNull())
                {
                    result = true;
                }
            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }

        public static void ChangeStatusForUpdatedCBIResultFile(Int32 ChunkSize, Int32? tenant_Id = null)
        {
            try
            {
                // ServiceContext.init();
                ServiceLogger.Info("Calling ChnageStatusForUpdatedCBIResultFile: " + DateTime.Now.ToString(), ChangeStatusForUpdatedCBIResultFileServiceLogger);

                Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                Int32 backgroundProcessUserId = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.BACKGROUND_PROCESS_USER_VALUE;

                List<ClientDBConfiguration> clientDbConfs = SecurityManager.GetClientDBConfiguration().Select(
                item =>
                {
                    ClientDBConfiguration config = new ClientDBConfiguration();
                    config.CDB_TenantID = item.CDB_TenantID;
                    config.CDB_ConnectionString = item.CDB_ConnectionString;
                    config.Tenant = new Entity.Tenant();
                    config.Tenant.TenantName = item.Tenant.TenantName; return config;
                }).ToList();

                ServiceLogger.Debug<List<ClientDBConfiguration>>("ChnageStatusForUpdatedCBIResultFile: List of Client DbConfigurations from database:", clientDbConfs, ChangeStatusForUpdatedCBIResultFileServiceLogger);
                if (clientDbConfs != null && clientDbConfs.Count > 0 && tenant_Id != null)
                {
                    ServiceLogger.Debug<Int32?>("ChnageStatusForUpdatedCBIResultFile: TenantID for which external vendor orders are to be dispatched:", tenant_Id, ChangeStatusForUpdatedCBIResultFileServiceLogger);
                    clientDbConfs = clientDbConfs.Where(x => x.CDB_TenantID == tenant_Id).ToList();
                }
                ServiceLogger.Info("ChnageStatusForUpdatedCBIResultFile: Started foreach loop on ClientDbConfigurations: " + DateTime.Now.ToString(), ChangeStatusForUpdatedCBIResultFileServiceLogger);
                var lcoationtenants = SecurityManager.GetListOfTenantWithLocationService().Select(x => x.TenantID);
                var loctiontenantconfig = clientDbConfs.Where(x => lcoationtenants.Contains(x.CDB_TenantID));
                foreach (ClientDBConfiguration cltDbConf in loctiontenantconfig)
                {
                    ServiceLogger.Info("ChnageStatusForUpdatedCBIResultFile: Check if location service available Tenant: " + DateTime.Now.ToString(), ChangeStatusForUpdatedCBIResultFileServiceLogger);
                    if (!cltDbConf.IsNullOrEmpty() && SecurityManager.IsLocationServiceTenant(cltDbConf.CDB_TenantID))
                    {
                        if (CheckDBConnection.TestConnString(cltDbConf.CDB_ConnectionString, ChangeStatusForUpdatedCBIResultFileServiceLogger))
                        {
                            DateTime jobStartTime = DateTime.Now;
                            DateTime jobEndTime;
                            Int32 tenantId = cltDbConf.CDB_TenantID;
                            ServiceLogger.Info("ChnageStatusForUpdatedCBIResultFile: Started Get All Result File Data at. " + DateTime.Now.ToString(), ChangeStatusForUpdatedCBIResultFileServiceLogger);

                            var tenant = SecurityManager.GetTenant(Convert.ToInt32(tenantId));
                            String institutionName = tenant.IsNull() || tenant.TenantName.IsNullOrEmpty() ? String.Empty : tenant.TenantName;
                            ///// Get Data Here
                            List<FileResultStatusUpdateContract> fileResultStatusUpdateContract = FingerPrintDataManager.GetAllUpdatedFileResults(tenantId, ChunkSize);

                            foreach (var fileResult in fileResultStatusUpdateContract)
                            {
                                ///// Get submitted to Cbi Status
                                Boolean IsSubmittedToCBI = fileResult.AppointmentStatus == FingerPrintAppointmentStatus.SUBMITTED_TO_CBI.GetStringValue();

                                Boolean IsContactAgency = fileResult.AppointmentStatus == FingerPrintAppointmentStatus.CONTACT_AGENCY_EMPLOYER.GetStringValue();

                                ///// Update status inside Get data loop and then send mail
                                if (fileResult.finalresult != "RFBIF" && fileResult.finalresult != "RCBIF")
                                {
                                    FingerPrintDataManager.UpdateStatusForFileResult(fileResult, backgroundProcessUserId, tenantId, IsSubmittedToCBI, IsContactAgency);
                                }
                                if ((!IsSubmittedToCBI && !IsContactAgency) || fileResult.IsDataError)
                                    continue;// do not do further processing if order is currently not in submitted state.

                                if (fileResult.finalresult != "RFBI" && fileResult.finalresult != "RCBI" && fileResult.finalresult != "RCBIA"
                                    && fileResult.finalresult != "SFBI" && fileResult.finalresult != "SCBI" && fileResult.finalresult != "RFBIA"
                                    )

                                {
                                    continue;

                                }//do not send email if order was neither rejected nor processed


                                ////Create Dictionary for Mail And Message Data
                                Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                                dictMailData.Add(EmailFieldConstants.AGENCY_NAME, institutionName);
                                dictMailData.Add(EmailFieldConstants.APPLICANT_NAME, fileResult.ApplicantName);
                                dictMailData.Add(EmailFieldConstants.Order_Number, fileResult.OrderNumber);
                                dictMailData.Add(EmailFieldConstants.CBI_SUCCESS_STATUS, fileResult.Result);
                                dictMailData.Add(EmailFieldConstants.CBI_PCN, fileResult.PCNNumber);
                                dictMailData.Add(EmailFieldConstants.RECEIVER_ORGANIZATION_USER_ID, fileResult.UserId);
                                dictMailData.Add(EmailFieldConstants.TENANT_ID, tenantId);

                                Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
                                mockData.UserName = string.Concat(fileResult.ApplicantName);
                                mockData.EmailID = fileResult.UserEmailId;
                                mockData.ReceiverOrganizationUserID = fileResult.UserId;

                                CommunicationSubEvents ObjCommunicationSubEvents = new CommunicationSubEvents();
                                if (!fileResult.IsOutofStateOrder && fileResult.finalresult == "RFBIA")
                                {
                                    ObjCommunicationSubEvents = CommunicationSubEvents.NOTIFICATION_FOR_FINGERPRINT_FILE_FINALLY_REJECTED;
                                }
                                else if (!fileResult.IsOutofStateOrder && fileResult.finalresult == "RCBIA")
                                {
                                    ObjCommunicationSubEvents = CommunicationSubEvents.NOTIFICATION_FOR_CBI_FINGERPRINT_FILE_SECOND_REJECTION;
                                }
                                else if (fileResult.IsOutofStateOrder && (fileResult.finalresult == "RFBI" || fileResult.finalresult == "RCBI" || fileResult.finalresult == "RFBIA" || fileResult.finalresult == "RCBIA"))//!fileResult.Result.ToLower().Contains("success")
                                {
                                    if (fileResult.finalresult == "RFBI" || fileResult.finalresult == "RCBI")
                                    {
                                        AppointmentSlotContract reserveSlotContract = new AppointmentSlotContract();
                                        reserveSlotContract.OrderNumber = fileResult.OrderNumber;
                                        reserveSlotContract.IsOutOfStateAppointment = true;
                                        reserveSlotContract.IsRejectedReschedule = true;
                                        reserveSlotContract.ApplicantOrgUserId = fileResult.UserId;
                                        if (FingerPrintSetUpManager.ResetOutOfStateApplicantAppointment(reserveSlotContract, backgroundProcessUserId, tenantId))
                                        {
                                            ServiceLogger.Info("ResetOutOfStateApplicantAppointment: Appointment Reset successfully for order " + fileResult.OrderNumber + " " + DateTime.Now.ToString()
                                                , ChangeStatusForUpdatedCBIResultFileServiceLogger);
                                        }
                                        else
                                        {
                                            ServiceLogger.Error("ResetOutOfStateApplicantAppointment: Appointment Reset failed " + fileResult.OrderNumber + " " + DateTime.Now.ToString()
                                                , ChangeStatusForUpdatedCBIResultFileServiceLogger);
                                        }

                                    }
                                    if (fileResult.finalresult == "RFBI")
                                    {
                                        ObjCommunicationSubEvents = CommunicationSubEvents.NOTIFICATION_FOR_FBI_OUT_OF_STATE_FINGERPRINT_REJECTION;
                                    }
                                    else if (fileResult.finalresult == "RFBIA")
                                    {
                                        ObjCommunicationSubEvents = CommunicationSubEvents.NOTIFICATION_FOR_FBI_OUT_OF_STATE_FINGERPRINT_SECOND_REJECTION;
                                    }
                                    else if (fileResult.finalresult == "RCBI")
                                    {
                                        ObjCommunicationSubEvents = CommunicationSubEvents.NOTIFICATION_FOR_CBI_OUT_OF_STATE_FINGERPRINT_REJECTION;
                                    }
                                    else
                                        ObjCommunicationSubEvents = CommunicationSubEvents.NOTIFICATION_FOR_CBI_OUT_OF_STATE_FINGERPRINT_SECOND_REJECTION;
                                }
                                else if (fileResult.finalresult == "RCBI")
                                {
                                    ObjCommunicationSubEvents = CommunicationSubEvents.NOTIFICATION_FOR_CBI_FINGERPRINT_FILE_REJECTED;
                                }
                                else
                                    ObjCommunicationSubEvents = (fileResult.finalresult == "SFBI" || fileResult.finalresult == "SCBI") ? CommunicationSubEvents.NOTIFICATION_FOR_FINGERPRINT_FILE_SUCESSFULLY_PROCESSED : CommunicationSubEvents.NOTIFICATION_FOR_FINGERPRINT_FILE_REJECTED;
                                //// send mail/message notification
                                // if (IsSubmittedToCBI && ((fileResult.isFbiErrSubmit && !fileResult.Result.ToLower().Contains("success")) || (fileResult.Result.ToLower().Contains("success") && ((fileResult.IsStateFBIApplicable && fileResult.isCbiSuccess && fileResult.isFbisuccess) || (fileResult.IsStateApplicable && fileResult.isCbiSuccess)))))
                                CommunicationManager.SentMailMessageNotification(ObjCommunicationSubEvents, mockData, dictMailData, fileResult.UserId, tenantId, fileResult.HierarchyNodeId);
                            }


                            ServiceLogger.Info("ChnageStatusForUpdatedCBIResultFile: End Update ALL AMS InProcess orders and getAllInprocessOrders at. " + DateTime.Now.ToString(),
                                ChangeStatusForUpdatedCBIResultFileServiceLogger);

                            //Save service logging data to DB
                            if (_isServiceLoggingEnabled)
                            {
                                jobEndTime = DateTime.Now;
                                ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                                serviceLoggingContract.ServiceName = ServiceName.BkgOrderService.GetStringValue();
                                serviceLoggingContract.JobName = JobName.ChangeStatusForUpdatedCBIResultFileService.GetStringValue();
                                serviceLoggingContract.TenantID = tenantId;
                                serviceLoggingContract.JobStartTime = jobStartTime;
                                serviceLoggingContract.JobEndTime = jobEndTime;
                                //serviceLoggingContract.Comments = "";
                                serviceLoggingContract.IsDeleted = false;
                                serviceLoggingContract.CreatedBy = backgroundProcessUserId;
                                serviceLoggingContract.CreatedOn = DateTime.Now;
                                SecurityManager.SaveServiceLoggingDetail(serviceLoggingContract);
                            }
                        }
                    }
                    ServiceContext.ReleaseDBContextItems();
                }
            }
            catch (Exception ex)
            {
                ServiceLogger.Error(String.Format("An Error has occured in ChnageStatusForUpdatedCBIResultFile method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}",
                                    ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString),
                                    ChangeStatusForUpdatedCBIResultFileServiceLogger);
            }
        }

        #endregion

        #endregion

        #region CBI Fingerprint Reciept

        public static void SendCBIFingerprintReciept(Int32 ChunkSize, Int32? tenant_Id = null)
        {
            try
            {
                // ServiceContext.init();
                ServiceLogger.Info("Calling SendCBIFingerprintReciept: " + DateTime.Now.ToString(), CBIFingerprintRecieptServiceLogger);

                Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                Int32 backgroundProcessUserId = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.BACKGROUND_PROCESS_USER_VALUE;

                List<ClientDBConfiguration> clientDbConfs = SecurityManager.GetClientDBConfiguration().Select(
                item =>
                {
                    ClientDBConfiguration config = new ClientDBConfiguration();
                    config.CDB_TenantID = item.CDB_TenantID;
                    config.CDB_ConnectionString = item.CDB_ConnectionString;
                    config.Tenant = new Entity.Tenant();
                    config.Tenant.TenantName = item.Tenant.TenantName; return config;
                }).ToList();

                ServiceLogger.Debug<List<ClientDBConfiguration>>("SendCBIFingerprintReciept: List of Client DbConfigurations from database:", clientDbConfs, CBIFingerprintRecieptServiceLogger);
                if (clientDbConfs != null && clientDbConfs.Count > 0 && tenant_Id != null)
                {
                    ServiceLogger.Debug<Int32?>("SendCBIFingerprintReciept: TenantID for which external vendor orders are to be dispatched:", tenant_Id, CBIFingerprintRecieptServiceLogger);
                    clientDbConfs = clientDbConfs.Where(x => x.CDB_TenantID == tenant_Id).ToList();
                }
                ServiceLogger.Info("SendCBIFingerprintReciept: Started foreach loop on ClientDbConfigurations: " + DateTime.Now.ToString(), CBIFingerprintRecieptServiceLogger);
                var lcoationtenants = SecurityManager.GetListOfTenantWithLocationService().Select(x => x.TenantID);
                var loctiontenantconfig = clientDbConfs.Where(x => lcoationtenants.Contains(x.CDB_TenantID));
                foreach (ClientDBConfiguration cltDbConf in loctiontenantconfig)
                {
                    ServiceLogger.Info("SendCBIFingerprintReciept: Check if location service available Tenant: " + DateTime.Now.ToString(), CBIFingerprintRecieptServiceLogger);
                    if (!cltDbConf.IsNullOrEmpty() && SecurityManager.IsLocationServiceTenant(cltDbConf.CDB_TenantID))
                    {
                        if (CheckDBConnection.TestConnString(cltDbConf.CDB_ConnectionString, CBIFingerprintRecieptServiceLogger))
                        {
                            DateTime jobStartTime = DateTime.Now;
                            DateTime jobEndTime;
                            Int32 tenantId = cltDbConf.CDB_TenantID;
                            ServiceLogger.Info("SendCBIFingerprintReciept: Started Get All Result File Data at. " + DateTime.Now.ToString(), CBIFingerprintRecieptServiceLogger);

                            var tenant = SecurityManager.GetTenant(Convert.ToInt32(tenantId));
                            String institutionName = tenant.IsNull() || tenant.TenantName.IsNullOrEmpty() ? String.Empty : tenant.TenantName;
                            ///// Get Data Here
                            List<FingerPrintRecieptContract> fileResultStatusUpdateContract = FingerPrintDataManager.GetUserRicieptFileData(tenantId, ChunkSize);

                            foreach (var fileResult in fileResultStatusUpdateContract)
                            {
                                ///// Update status inside Get data loop and then send mail


                                Int32? systemCommunicationID = null;
                                ////Create Dictionary for Mail And Message Data
                                Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                                dictMailData.Add(EmailFieldConstants.AGENCY_NAME, institutionName);
                                dictMailData.Add(EmailFieldConstants.APPLICANT_NAME, fileResult.ApplicantName);
                                dictMailData.Add(EmailFieldConstants.Order_Number, fileResult.OrderNumber);
                                dictMailData.Add(EmailFieldConstants.TENANT_ID, tenantId);
                                dictMailData.Add(EmailFieldConstants.RECEIVER_ORGANIZATION_USER_ID, fileResult.UserId);
                                dictMailData.Add(EmailFieldConstants.CBI_PCN, fileResult.PCNNumber);
                                Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
                                mockData.UserName = string.Concat(fileResult.ApplicantName);
                                mockData.EmailID = fileResult.UserEmail;
                                mockData.ReceiverOrganizationUserID = fileResult.UserId;
                                CommunicationSubEvents ObjCommunicationSubEvents = new CommunicationSubEvents();
                                ObjCommunicationSubEvents = CommunicationSubEvents.NOTIFICATION_FOR_CBI_FILE_RICIEPT;
                                //// send mail/message notification
                                systemCommunicationID = CommunicationManager.SentMailMessageNotification(ObjCommunicationSubEvents, mockData, dictMailData, fileResult.UserId, tenantId, fileResult.HierarchyNodeID);

                                List<lkpDocumentAttachmentType> docAttachmentType = CommunicationManager.GetDocumentAttachmentType();
                                String docAttachmentTypeCode = DocumentAttachmentType.APPLICANT_DOCUMENT.GetStringValue();
                                Int16 docAttachmentTypeID = docAttachmentType.IsNotNull() && docAttachmentType.Count > 0 ?
                                    Convert.ToInt16(docAttachmentType.FirstOrDefault(cond => cond.DAT_Code == docAttachmentTypeCode).DAT_ID) : Convert.ToInt16(0);


                                systemCommunicationID = systemCommunicationID > 0 ? systemCommunicationID : null;

                                Int32? sysCommAttachmentID = null;
                                if (systemCommunicationID != null)
                                {
                                    FingerPrintDataManager.UpdateFileRecieptDispatched(fileResult.AppointmentId, tenantId, backgroundProcessUserId);
                                    SystemCommunicationAttachment sysCommAttachment = new SystemCommunicationAttachment();
                                    sysCommAttachment.SCA_OriginalDocumentID = fileResult.DocumentID;
                                    sysCommAttachment.SCA_OriginalDocumentName = fileResult.DocFileName;
                                    sysCommAttachment.SCA_DocumentPath = fileResult.DocPath;
                                    sysCommAttachment.SCA_DocumentSize = fileResult.DocSize;
                                    sysCommAttachment.SCA_SystemCommunicationID = systemCommunicationID.Value;
                                    sysCommAttachment.SCA_DocAttachmentTypeID = docAttachmentTypeID;
                                    sysCommAttachment.SCA_TenantID = tenantId;
                                    sysCommAttachment.SCA_IsDeleted = false;
                                    sysCommAttachment.SCA_CreatedBy = backgroundProcessUserId;
                                    sysCommAttachment.SCA_CreatedOn = DateTime.Now;
                                    sysCommAttachment.SCA_ModifiedBy = null;
                                    sysCommAttachment.SCA_ModifiedOn = null;

                                    sysCommAttachmentID = CommunicationManager.SaveSystemCommunicationAttachment(sysCommAttachment);
                                }
                            }


                            ServiceLogger.Info("SendCBIFingerprintReciept: End Update process at. " + DateTime.Now.ToString(),
                                CBIFingerprintRecieptServiceLogger);

                            //Save service logging data to DB
                            if (_isServiceLoggingEnabled)
                            {
                                jobEndTime = DateTime.Now;
                                ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                                serviceLoggingContract.ServiceName = ServiceName.BkgOrderService.GetStringValue();
                                serviceLoggingContract.JobName = JobName.ChangeStatusForUpdatedCBIResultFileService.GetStringValue();
                                serviceLoggingContract.TenantID = tenantId;
                                serviceLoggingContract.JobStartTime = jobStartTime;
                                serviceLoggingContract.JobEndTime = jobEndTime;
                                //serviceLoggingContract.Comments = "";
                                serviceLoggingContract.IsDeleted = false;
                                serviceLoggingContract.CreatedBy = backgroundProcessUserId;
                                serviceLoggingContract.CreatedOn = DateTime.Now;
                                SecurityManager.SaveServiceLoggingDetail(serviceLoggingContract);
                            }
                        }
                    }
                    ServiceContext.ReleaseDBContextItems();
                }
            }
            catch (Exception ex)
            {
                ServiceLogger.Error(String.Format("An Error has occured in SendCBIFingerprintReciept method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}",
                                    ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString),
                                    CBIFingerprintRecieptServiceLogger);
            }
        }

        #endregion

        #region CBI Result File(Rejection)

        #region Variables

        private static String CBIResultFilesLogger;
        private static Boolean _CBIResultFiles;
        private static Boolean _isCBIResultFilesServiceLoggingEnabled;
        private static String _CBIResultFilesTemporaryFileLocation { get; set; }

        #region SFTP Configuration

        private static String CBIResultFilesFTPUsername { get; set; }
        private static String CBIResultFilesFTPPassword { get; set; }
        private static String CBIResultFilesFTPHostName { get; set; }
        private static String CBIResultFilesFTPPortNumber { get; set; }
        private static String CBIResultFilesFTPArchiveRemotePath { get; set; }
        private static String CBIResultFilesFTPInRemotePath { get; set; }
        // We are not using CBIResultFilesFTPErroreRemotePath, now we are using only one input folder i.e. CBIResultFilesFTPInRemotePath 
        //private static string CBIResultFilesFTPErroreRemotePath { get; set; }
        private static String CBIResultFilesFTPSshHostKeyFingerprint { get; set; }
        private static String CBIResultFilesFTPAcceptAnySSHHostKey { get; set; }
        private static Boolean CBIResultFilesFTPIsArchiveDisable { get; set; }
        private static Int32 CBIResultFilesTentantId { get; set; }
        private static Int32 backgroundProcessUserId { get; set; }




        #endregion



        static void CBIResultService()
        {
            CBIResultFilesLogger = "GetCBIResultFilesLogger";

            //if ((ConfigurationManager.AppSettings["CBIResultFilesFTPErroreRemotePath"].IsNotNull()))
            //{
            //    CBIResultFilesFTPErroreRemotePath = Convert.ToString(ConfigurationManager.AppSettings["CBIResultFilesFTPErroreRemotePath"]);
            //}
            if ((ConfigurationManager.AppSettings["CBIResultFilesTentantId"].IsNotNull()))
            {
                CBIResultFilesTentantId = Convert.ToInt32(ConfigurationManager.AppSettings["CBIResultFilesTentantId"]);
            }
            if ((ConfigurationManager.AppSettings["CBIResultFiles"].IsNotNull()))
            {
                _CBIResultFiles = Convert.ToBoolean(ConfigurationManager.AppSettings["CBIResultFiles"]);
            }

            if ((ConfigurationManager.AppSettings["CBIResultFilesFTPUsername"].IsNotNull()))
            {
                CBIResultFilesFTPUsername = Convert.ToString(ConfigurationManager.AppSettings["CBIResultFilesFTPUsername"]);
            }

            if ((ConfigurationManager.AppSettings["CBIResultFilesFTPPassword"].IsNotNull()))
            {
                CBIResultFilesFTPPassword = Convert.ToString(ConfigurationManager.AppSettings["CBIResultFilesFTPPassword"]);
            }

            if ((ConfigurationManager.AppSettings["CBIResultFilesFTPHostName"].IsNotNull()))
            {
                CBIResultFilesFTPHostName = Convert.ToString(ConfigurationManager.AppSettings["CBIResultFilesFTPHostName"]);
            }

            if ((ConfigurationManager.AppSettings["CBIResultFilesFTPPortNumber"].IsNotNull()))
            {
                CBIResultFilesFTPPortNumber = Convert.ToString(ConfigurationManager.AppSettings["CBIResultFilesFTPPortNumber"]);
            }

            if ((ConfigurationManager.AppSettings["CBIResultFilesFTPArchiveRemotePath"].IsNotNull()))
            {
                CBIResultFilesFTPArchiveRemotePath = Convert.ToString(ConfigurationManager.AppSettings["CBIResultFilesFTPArchiveRemotePath"]);
            }
            if ((ConfigurationManager.AppSettings["CBIResultFilesFTPSshHostKeyFingerprint"].IsNotNull()))
            {
                CBIResultFilesFTPSshHostKeyFingerprint = Convert.ToString(ConfigurationManager.AppSettings["CBIResultFilesFTPSshHostKeyFingerprint"]);
            }

            if ((ConfigurationManager.AppSettings["CBIResultFilesFTPAcceptAnySSHHostKey"].IsNotNull()))
            {
                CBIResultFilesFTPAcceptAnySSHHostKey = Convert.ToString(ConfigurationManager.AppSettings["CBIResultFilesFTPAcceptAnySSHHostKey"]);
            }

            if ((ConfigurationManager.AppSettings["CBIResultFilesFTPIsArchiveDisable"].IsNotNull()))
            {
                CBIResultFilesFTPIsArchiveDisable = Convert.ToBoolean(ConfigurationManager.AppSettings["CBIResultFilesFTPIsArchiveDisable"]);
            }

            if (ConfigurationManager.AppSettings["isCBIResultFilesServiceLoggingEnabled"].IsNotNull())
            {
                _isCBIResultFilesServiceLoggingEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["isCBIResultFilesServiceLoggingEnabled"]);
            }
            if (ConfigurationManager.AppSettings["CBIResultFilesTemporaryFileLocation"].IsNotNull())
            {
                _CBIResultFilesTemporaryFileLocation = Convert.ToString(ConfigurationManager.AppSettings["CBIResultFilesTemporaryFileLocation"]);
            }
            if (ConfigurationManager.AppSettings["CBIResultFilesFTPInRemotePath"].IsNotNull())
            {
                CBIResultFilesFTPInRemotePath = Convert.ToString(ConfigurationManager.AppSettings["CBIResultFilesFTPInRemotePath"]);
            }

            else
            {
                _isServiceLoggingEnabled = false;
            }
        }

        #endregion
        public static void GetCBIResultFilesAndUpdateTables()
        {
            try
            {
                Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                backgroundProcessUserId = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.BACKGROUND_PROCESS_USER_VALUE;

                CBIResultService();
                // ServiceContext.init();
                ServiceLogger.Info("Calling GetCBIResultFiles: " + DateTime.Now.ToString(), CBIResultFilesLogger);
                SessionOptions TempSession = CBIResultSetSFTPSessionOption(CBIResultFilesFTPHostName, CBIResultFilesFTPPortNumber, CBIResultFilesFTPUsername, CBIResultFilesFTPPassword, Protocol.Sftp);

                if (!CBIResultSaveSftpfile(TempSession))
                {
                    ServiceLogger.Info("Something Went wrong Please contact to admin: " + DateTime.Now.ToString(), CBIResultFilesLogger);
                }

            }
            catch (Exception ex)
            {
                ServiceLogger.Error(String.Format("An Error has occured in UpdateFingerPrintOrder method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}",
                                    ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString),
                                    UpdateFingerPrintOrderLogger);
            }
        }
        private static SessionOptions CBIResultSetSFTPSessionOption(String hostName, String portNumber, String userName, String password, Protocol protocol)
        {
            SessionOptions sessionOptions = new SessionOptions();
            ServiceLogger.Info("Set Connection open for CBI Result File: " + DateTime.Now.ToString(), CBIResultFilesLogger);

            if (!protocol.IsNullOrEmpty() && !userName.IsNullOrEmpty() && !password.IsNullOrEmpty())
            {
                sessionOptions.HostName = hostName;
                sessionOptions.PortNumber = Convert.ToInt32(portNumber);
                sessionOptions.Protocol = protocol;
                sessionOptions.UserName = userName;
                sessionOptions.Password = password;
                if (Convert.ToBoolean(FingerPrintFTPAcceptAnySSHHostKey) == false)
                {
                    sessionOptions.SshHostKeyFingerprint = FingerPrintFTPSshHostKeyFingerprint;
                }
                else
                {
                    sessionOptions.GiveUpSecurityAndAcceptAnySshHostKey = true;
                }
            }
            return sessionOptions;
        }
        private static bool MoveInFolderArchive(string fileName, SessionOptions sessionOptions, Guid UniqueFileName)
        {
            bool IsFileMoveSuccessfully = true;
            try
            {
                using (Session session = new Session())
                {
                    session.Open(sessionOptions);
                    if (session.Opened)
                    {
                        String sourcePath = CBIResultFilesFTPInRemotePath + fileName;
                        if (session.FileExists(sourcePath))
                        {
                            try
                            {


                                session.MoveFile(sourcePath, CBIResultFilesFTPArchiveRemotePath + UniqueFileName + "_" + fileName);
                                ServiceLogger.Info("File move successfully In to Archive: " + UniqueFileName + fileName + DateTime.Now.ToString(), CBIResultFilesLogger);
                            }
                            catch (Exception)
                            {
                                ServiceLogger.Info("UpdateFingerPrintOrder: Error occurred in MoveSavedDocumentsToArhciveFolder() while moving the file to archive folder." + DateTime.Now.ToString(),
                                                         UpdateFingerPrintOrderLogger);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                IsFileMoveSuccessfully = false;
            }
            return IsFileMoveSuccessfully;
        }

        //private static bool MoveErrorFolderArchive(string fileName, SessionOptions sessionOptions, Guid UniqueFileName)
        //{
        //    bool IsFileMoveSuccessfully = true;
        //    try
        //    {
        //        using (Session session = new Session())
        //        {
        //            session.Open(sessionOptions);
        //            if (session.Opened)
        //            {

        //                session.MoveFile(CBIResultFilesFTPErroreRemotePath + "/" + fileName, CBIResultFilesFTPArchiveRemotePath + UniqueFileName + "_" + fileName);
        //                ServiceLogger.Info("File move successfully Error to Archive: " + fileName + DateTime.Now.ToString(), CBIResultFilesLogger);
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        IsFileMoveSuccessfully = false;
        //    }
        //    return IsFileMoveSuccessfully;
        //}
        private static string GetRejectionReason(string fileContent)
        {
            try
            {
                string RejectionReason = string.Empty;
                if (!fileContent.IsNullOrEmpty())
                {

                    int pFrom = fileContent.IndexOf("1.09:") + "1.09:".Length;
                    int pTo = fileContent.LastIndexOf("1.10:");
                    RejectionReason = fileContent.Substring(pFrom, pTo - pFrom);
                }
                return RejectionReason;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private static Boolean CBIResultFileCreateArchiveFolderIfNotExist(Session session, String archiveRemotePath)
        {
            Boolean result = false;
            RemoteDirectoryInfo dirInfo = null;
            try
            {
                dirInfo = session.ListDirectory(archiveRemotePath);
                result = true;
            }
            catch (Exception)
            {


            }
            try
            {
                if (dirInfo == null)
                    session.CreateDirectory(archiveRemotePath);
                dirInfo = session.ListDirectory(archiveRemotePath);
                if (dirInfo.IsNotNull())
                {
                    result = true;
                }
            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }
        private static Boolean CBIResultSaveSftpfile(SessionOptions sessionOptions)
        {
            Boolean IsFileGetSuccess = true;
            try
            {
                using (Session session = new Session())
                {
                    session.Open(sessionOptions);
                    if (session.Opened)
                    {
                        if (!CBIResultFileCreateArchiveFolderIfNotExist(session, CBIResultFilesFTPInRemotePath))
                        {
                            ServiceLogger.Info("Directory Create: " + DateTime.Now.ToString(), CBIResultFilesLogger);

                        }
                        if (!CBIResultFileCreateArchiveFolderIfNotExist(session, CBIResultFilesFTPArchiveRemotePath))
                        {
                            ServiceLogger.Info("Directory Create: " + DateTime.Now.ToString(), CBIResultFilesLogger);
                        }
                        //if (!CBIResultFileCreateArchiveFolderIfNotExist(session, CBIResultFilesFTPErroreRemotePath))
                        //{
                        //    ServiceLogger.Info("Directory Create: " + DateTime.Now.ToString(), CBIResultFilesLogger);
                        //}
                        bool exists = session.FileExists(CBIResultFilesFTPInRemotePath);
                        //bool ErrorFileExists = session.FileExists(CBIResultFilesFTPErroreRemotePath);
                        TransferOptions transferOption = new TransferOptions();
                        transferOption.TransferMode = TransferMode.Binary;

                        if (!_CBIResultFilesTemporaryFileLocation.IsNullOrEmpty() && !Directory.Exists(_CBIResultFilesTemporaryFileLocation))
                        {
                            Directory.CreateDirectory(_CBIResultFilesTemporaryFileLocation);
                        }
                        if (exists && Directory.Exists(_CBIResultFilesTemporaryFileLocation))
                        {
                            session.GetFiles(CBIResultFilesFTPInRemotePath, _CBIResultFilesTemporaryFileLocation);
                            ServiceLogger.Info("Gets CBI Result Files Successfully: " + DateTime.Now.ToString(), CBIResultFilesLogger);
                        }
                        //if (ErrorFileExists && Directory.Exists(_CBIResultFilesTemporaryFileLocation))
                        //{

                        //    session.GetFiles(CBIResultFilesFTPErroreRemotePath, _CBIResultFilesTemporaryFileLocation);
                        //    ServiceLogger.Info("Gets CBI Result Files Successfully: " + DateTime.Now.ToString(), CBIResultFilesLogger);

                        //}


                        if (!_CBIResultFilesTemporaryFileLocation.IsNullOrEmpty() && Directory.Exists(_CBIResultFilesTemporaryFileLocation))
                        {
                            string[] lstFileName = Directory.GetFiles(_CBIResultFilesTemporaryFileLocation);
                            if (lstFileName.Count() > Convert.ToInt32(AppConsts.ZERO))
                            {
                                foreach (string PathOfFile in lstFileName)
                                {
                                    string Result = string.Empty;
                                    string ResultStatus = string.Empty;
                                    string FileName = string.Empty;
                                    string Fileextension = string.Empty;
                                    string contents = string.Empty;
                                    string RejectionReason = string.Empty;

                                    contents = File.ReadAllText(PathOfFile).Trim();
                                    Fileextension = Path.GetExtension(PathOfFile).Trim();
                                    FileName = Path.GetFileName(PathOfFile).Trim();

                                    if ((Fileextension.ToLower().Contains(".errfbi") || Fileextension.ToLower().Contains(".errstate")) && !contents.IsNullOrEmpty())
                                    {
                                        Result = "Error";
                                    }
                                    else if ((Fileextension.ToLower().Contains(".srestate") || Fileextension.ToLower().Contains(".srefbi")) && !contents.IsNullOrEmpty())
                                    {
                                        Result = "Success";
                                    }
                                    else if (!contents.IsNullOrEmpty()) { Result = "Unknown Error"; }

                                    if (Fileextension.ToLower().Contains("state") && !contents.IsNullOrEmpty())
                                    {
                                        ResultStatus = "State";
                                    }
                                    else if (Fileextension.ToLower().Contains("fbi") && !contents.IsNullOrEmpty())
                                    {
                                        ResultStatus = "FBI";
                                    }

                                    if (contents.IsNullOrEmpty())
                                    {
                                        Result = "Corrupt File";
                                        ResultStatus = "Corrupt File";
                                    }

                                    FingerPrintOrderContract ObjFingerPrintOrderContract = new FingerPrintOrderContract();
                                    ObjFingerPrintOrderContract.Name = FileName;
                                    ObjFingerPrintOrderContract.PCNNumber = Path.GetFileNameWithoutExtension(PathOfFile).Trim();
                                    if (Fileextension.ToLower().Contains(".errfbi") && !contents.IsNullOrEmpty())
                                    {
                                        RejectionReason = GetRejectionReason(contents);
                                        if (RejectionReason.IsNullOrEmpty())
                                        {
                                            Result = "Corrupt File";
                                            ResultStatus = "Corrupt File";
                                            RejectionReason = "Corrupt File";
                                        }
                                        ObjFingerPrintOrderContract.RejectionReason = RejectionReason;
                                    }
                                    ObjFingerPrintOrderContract.Result = Result;
                                    ObjFingerPrintOrderContract.ResultStatus = ResultStatus;
                                    ObjFingerPrintOrderContract.Extension = Fileextension;
                                    ObjFingerPrintOrderContract.FileContent = contents;
                                    ObjFingerPrintOrderContract.CBI_TentantId = CBIResultFilesTentantId;
                                    bool IsFileMoveSuccessfully = false;
                                    if (FingerPrintDataManager.InsertDataCBIResultFile(ObjFingerPrintOrderContract, backgroundProcessUserId))
                                    {
                                        Guid FileNameAppend = Guid.NewGuid();                                       

                                        //if (Result == "Unknown Error")
                                        //{
                                        //IsFileMoveSuccessfully = MoveErrorFolderArchive(FileName, sessionOptions, FileNameAppend);
                                        //}
                                        //else
                                        //{
                                        //  IsFileMoveSuccessfully = MoveInFolderArchive(FileName, sessionOptions, FileNameAppend);
                                        //}

                                        IsFileMoveSuccessfully = MoveInFolderArchive(FileName, sessionOptions, FileNameAppend);
                                    }
                                    if (IsFileMoveSuccessfully && FingerPrintDataManager.UpdateDataCBIResultFile(Path.GetFileNameWithoutExtension(FileName), backgroundProcessUserId))
                                    {
                                        if (IsFileMoveSuccessfully && File.Exists(PathOfFile))
                                        {
                                            File.Delete(PathOfFile);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                IsFileGetSuccess = false;
                ServiceLogger.Info(ex.ToString() + DateTime.Now.ToString(), CBIResultFilesLogger);

            }
            return IsFileGetSuccess;
        }
        #endregion

        
    }
}
