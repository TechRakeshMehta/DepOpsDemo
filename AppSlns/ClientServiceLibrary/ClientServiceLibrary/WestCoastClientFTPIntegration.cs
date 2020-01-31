using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Xml;
using System.Data;
using INTSOF.Utils;
using WinSCP;
namespace ClientServiceLibrary
{
    public class WestCoastClientFTPIntegration
    {
        /// <summary>
        /// Upload Tracking Data on SFTP
        /// </summary>
        /// <param name="requestParameters">requestParameters</param>
        /// <returns>HttpResponseMessage</returns>
        public HttpResponseMessage UploadData(Object[] requestParameters)
        {
            String responseMessage = String.Empty;
            HttpResponseMessage response = new HttpResponseMessage();
            Boolean result = false;

            try
            {
                String ServiceConfiguration = requestParameters[0].ToString();                
                String DataToUpload = requestParameters[4].ToString();

                SFTPConfiguration sFTPConfiguration = GetSFTPConfiguration(ServiceConfiguration);

                List<WestCoastClientDataContract> dataContract = GetDataByConvertingXMLToDataContract(DataToUpload);

                DataTable fieldData = null;

                if (dataContract.IsNotNull() && dataContract.Count > 0)
                {
                    fieldData = GenerateFieldData(dataContract);
                }

                if (fieldData.IsNotNull())
                {
                    Byte[] fileBytes = GetFileBytes(fieldData);

                    if (fileBytes.IsNotNull())
                    {
                        result = UploadFileOnSFTP(fileBytes, sFTPConfiguration);
                    }
                }

                if (result)
                {
                    responseMessage = "<Response> <ExternalServiceResponse> " + "File uploded to SFTP location Successfully.Remote Path" + sFTPConfiguration.RemotePath + " </ExternalServiceResponse>  <ResponseMessage> Requested Service Successfully Processed. </ResponseMessage> <ResponseCode> 1 </ResponseCode> <Description> Response 1 means data upload successfully and 0 means failed to upload data due to internal server error. </Description> </Response>";
                    response.StatusCode = HttpStatusCode.OK;
                }

            }
            catch (Exception ex)
            {
                String message = String.Format("SFTP File Upload Failed. The exception details are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace);
                responseMessage = "<Response><ResponseMessage>" + message + "</ResponseMessage>  <ResponseCode> 0 </ResponseCode> <Description> Response 1 means data upload successfully and 0 means failed to upload data due to internal server error. </Description> </Response>";
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            response.Content = new StringContent(responseMessage, Encoding.UTF8, "application/xml");
            return response;
        }
        /// <summary>
        /// Get SFTP Configuration
        /// </summary>
        /// <param name="serviceConfiguration">serviceConfiguration</param>
        /// <returns>SFTPConfiguration</returns>
        SFTPConfiguration GetSFTPConfiguration(String serviceConfiguration)
        {
            SFTPConfiguration sFTPConfigurationDetails = new SFTPConfiguration();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(serviceConfiguration);
            XmlNodeList parentNode = xmlDoc.GetElementsByTagName("Configuration");

            foreach (XmlNode node in parentNode)
            {
                sFTPConfigurationDetails.Username = node.SelectSingleNode("Username").InnerText;
                sFTPConfigurationDetails.Password = node.SelectSingleNode("Password").InnerText;
                sFTPConfigurationDetails.HostName = node.SelectSingleNode("HostName").InnerText;
                sFTPConfigurationDetails.PortNumber = node.SelectSingleNode("PortNumber").InnerText;
                sFTPConfigurationDetails.RemotePath = node.SelectSingleNode("RemotePath").InnerText;
                sFTPConfigurationDetails.SshHostKeyFingerprint = node.SelectSingleNode("SshHostKeyFingerprint").InnerText;
                sFTPConfigurationDetails.AcceptAnySSHHostKey = node.SelectSingleNode("AcceptAnySSHHostKey").InnerText;
            }

            return sFTPConfigurationDetails;
        }
        /// <summary>
        /// Get Data By Converting XML To DataContract
        /// </summary>
        /// <param name="serviceConfiguration">serviceConfiguration</param>
        /// <returns> List<WestCoastClientDataContract> </returns>
        List<WestCoastClientDataContract> GetDataByConvertingXMLToDataContract(String serviceConfiguration)
        {
            List<WestCoastClientDataContract> dataContract = new List<WestCoastClientDataContract>();
            WestCoastClientDataContract contract;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(serviceConfiguration);

            XmlNodeList parentNode = xmlDoc.GetElementsByTagName("batchRequests");
            foreach (XmlNode node in parentNode)
            {
                contract = new WestCoastClientDataContract();
                contract.UserName = node.SelectSingleNode("UserName").InnerText;
                contract.UniversityUniqueIdentifier = node.SelectSingleNode("UniversityUniqueIdentifier").InnerText;
                contract.CategoryName = node.SelectSingleNode("CategoryName").InnerText;
                contract.CategoryStatus = node.SelectSingleNode("CategoryStatus").InnerText;
                contract.CategoryExpirationDate = node.SelectSingleNode("CategoryExpirationDate").InnerText;
                contract.CategoryComplianceDate = node.SelectSingleNode("CategoryComplianceDate").InnerText;
                dataContract.Add(contract);
            }

            return dataContract;
        }
        /// <summary>
        /// Generate Field Data
        /// </summary>
        /// <param name="dataContract"></param>
        /// <returns>DataTable</returns>
        private DataTable GenerateFieldData(List<WestCoastClientDataContract> dataContract)
        {
            DataTable dt = new DataTable();

            try
            {
                dt.Clear();
                dt.Columns.Add(new DataColumn("UserName", typeof(string)));
                dt.Columns.Add(new DataColumn("UniversityUniqueIdentifier", typeof(string)));
                dt.Columns.Add(new DataColumn("CategoryName", typeof(string)));
                dt.Columns.Add(new DataColumn("CategoryStatus", typeof(string)));
                dt.Columns.Add(new DataColumn("CategoryExpirationDate", typeof(string)));
                dt.Columns.Add(new DataColumn("CategoryComplianceDate", typeof(string)));

                foreach (WestCoastClientDataContract data in dataContract)
                {
                    DataRow dr = dt.NewRow();
                    dr["UserName"] = data.UserName;
                    dr["UniversityUniqueIdentifier"] = data.UniversityUniqueIdentifier;
                    dr["CategoryName"] = data.CategoryName;
                    dr["CategoryStatus"] = data.CategoryStatus;
                    dr["CategoryExpirationDate"] = data.CategoryExpirationDate;
                    dr["CategoryComplianceDate"] = data.CategoryComplianceDate;
                    dt.Rows.Add(dr);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dt;
        }
        /// <summary>
        /// Get File Bytes
        /// </summary>
        /// <param name="dt"></param>
        /// <returns>Byte</returns>
        private Byte[] GetFileBytes(DataTable dt)
        {
            Byte[] fileBytes = null;

            try
            {
                fileBytes = ExcelReader.GetTrackingUpdateBytes(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return fileBytes;
        }
        /// <summary>
        /// Upload File On SFTP
        /// </summary>
        /// <param name="fileBytes">fileBytes</param>
        /// <param name="sFTPConfiguration">sFTPConfiguration</param>
        /// <returns>Boolean</returns>
        private static Boolean UploadFileOnSFTP(Byte[] fileBytes, SFTPConfiguration sFTPConfiguration)
        {
            Boolean IsFileUploadedOnSFTPServer = false;

            try
            {
                var _fileName = "TrackingUpdate";
                String tempFilePath = System.Configuration.ConfigurationManager.AppSettings["TemporaryFileLocation"];
                if (!tempFilePath.EndsWith(@"\"))
                {
                    tempFilePath += @"\";
                }
                tempFilePath += "TempFiles\\" + "WestCoastUniversity" + "\\" + _fileName;

                if (!Directory.Exists(tempFilePath))
                {
                    Directory.CreateDirectory(tempFilePath);
                }

                String fileName = _fileName + "_" + DateTime.Now.ToString("MMddyyyy_HHmmss") + ".xls";
                String localPath = Path.Combine(tempFilePath, fileName);

                using (var fs = new FileStream(localPath, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(fileBytes, 0, fileBytes.Length);
                }

                SessionOptions sessionOptions = SetSFTPSessionOption(sFTPConfiguration);
                if (sessionOptions.IsNotNull() && !sessionOptions.HostName.IsNullOrEmpty())
                {
                    IsFileUploadedOnSFTPServer = SFTPFileUpload(sFTPConfiguration.RemotePath, fileName, localPath, sessionOptions);
                }               
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return IsFileUploadedOnSFTPServer;
        }
        /// <summary>
        /// SFTP File Upload
        /// </summary>
        /// <param name="remotePath">remotePath</param>
        /// <param name="fileName">fileName</param>
        /// <param name="localPath">localPath</param>
        /// <param name="sessionOptions">sessionOptions</param>
        /// <returns>Boolean</returns>
        public static Boolean SFTPFileUpload(String remotePath, String fileName, String localPath, SessionOptions sessionOptions)
        {
            Boolean IsFileUploaded = false;

            try
            {
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

                    if (transferResult.IsSuccess)
                    {
                        IsFileUploaded = true;
                    }

                    foreach (TransferEventArgs transfer in transferResult.Transfers)
                    {
                        if (File.Exists(localPath))
                        {
                            File.Delete(localPath);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return IsFileUploaded;
        }
        /// <summary>
        /// Set SFTP Session Option
        /// </summary>
        /// <param name="ftpDetail">ftpDetail</param>
        /// <returns>SessionOptions</returns>
        public static SessionOptions SetSFTPSessionOption(SFTPConfiguration ftpDetail)
        {
            SessionOptions sessionOptions = new SessionOptions();

            if (ftpDetail.IsNotNull())
            {
                sessionOptions.HostName = ftpDetail.HostName;
                sessionOptions.PortNumber = Convert.ToInt32(ftpDetail.PortNumber);
                sessionOptions.UserName = ftpDetail.Username;
                sessionOptions.Password = ftpDetail.Password;

                if (Convert.ToBoolean(ftpDetail.AcceptAnySSHHostKey) == false)
                {
                    sessionOptions.SshHostKeyFingerprint = ftpDetail.SshHostKeyFingerprint;
                }
                else
                {
                    sessionOptions.GiveUpSecurityAndAcceptAnySshHostKey = true;
                }
            }

            return sessionOptions;
        }
    }
}
