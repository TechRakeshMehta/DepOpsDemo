using Entity;
using NLog;
using System;
using System.IO;
using WinSCP;

namespace DataFeedAPIService.DataFeedUtils
{
    public static class DataFeedFTPUtils
    {
        private static Logger logger;

        static DataFeedFTPUtils()
        {
            logger = LogManager.GetLogger("DataFeedServiceLogger");
        }

        public static SessionOptions SetFTPSessionOption(DataFeedFTPDetail ftpDetail)
        {
            SessionOptions sessionOptions = new SessionOptions();
            sessionOptions.HostName = ftpDetail.HostName;
            sessionOptions.PortNumber = Convert.ToInt32(ftpDetail.PortNo);
            sessionOptions.UserName = ftpDetail.UserName;
            sessionOptions.Password = ftpDetail.FTPPassword;

            if (Convert.ToBoolean(ftpDetail.AcceptAnySSHHostKey) == false)
            {
                sessionOptions.SshHostKeyFingerprint = ftpDetail.SSHHostKey;
            }
            else
            {
                sessionOptions.GiveUpSecurityAndAcceptAnySshHostKey = true;
            }
            return sessionOptions;
        }

        public static Boolean FTPFileUpload(String remotePath, String fileName, String localPath, SessionOptions sessionOptions)
        {
            try
            {
                logger.Debug(fileName + ": Uploading file to FTP location.");
                //SessionOptions sessionOptions = SetFTPSessionOption(ftpDetail);
                using (Session session = new Session())
                {
                    session.Open(sessionOptions);
                    try
                    {
                        RemoteDirectoryInfo dirInfo = session.ListDirectory(remotePath);
                    }
                    catch (Exception)
                    {
                        session.CreateDirectory(remotePath);
                    }
                    remotePath = remotePath + "//" + fileName;

                    TransferOptions transferOptions = new TransferOptions();
                    transferOptions.TransferMode = TransferMode.Binary;
                    transferOptions.PreserveTimestamp = false;
                    transferOptions.FilePermissions = null;

                    TransferOperationResult transferResult;
                    transferResult = session.PutFiles(localPath, remotePath, false, transferOptions);
                    transferResult.Check();

                    foreach (TransferEventArgs transfer in transferResult.Transfers)
                    {
                        if (File.Exists(localPath))
                        {
                            File.Delete(localPath);
                        }
                    }
                }

                logger.Debug(fileName + " file uploded to SFTP location Successfully. Remote Path : " + remotePath);
            }
            catch (Exception ex)
            {
                logger.Error("FTPFileUpload Failed. The exception details are: {0}, Inner Exception: {1}, Stack Trace: {2}",
                                ex.Message, ex.InnerException, ex.StackTrace);
                return false;
            }
            return true;
        }
    }
}
