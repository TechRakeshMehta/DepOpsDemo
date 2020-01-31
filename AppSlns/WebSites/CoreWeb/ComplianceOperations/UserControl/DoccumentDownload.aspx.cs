using System;
using Microsoft.Practices.ObjectBuilder;
using System.IO;
using System.Threading;
using System.Configuration;
using INTSOF.Utils;
using Ionic.Zip;
using Entity.SharedDataEntity;
using System.Web.Configuration;
using System.Linq;
using INTSOF.Utils.FingerPrint;

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class DoccumentDownload : System.Web.UI.Page, IDoccumentDownloadView
    {
        private DoccumentDownloadPresenter _presenter = new DoccumentDownloadPresenter();

        public Int32 ApplicantDocumentId
        {
            get
            {
                if (ViewState["ApplicantDocumentId"] != null)
                    return Convert.ToInt32(ViewState["ApplicantDocumentId"]);
                return 0;
            }
            set
            {
                ViewState["ApplicantDocumentId"] = value;
            }
        }

        public Int32 TenantId
        {
            get
            {
                if (ViewState["TenantId"] != null)
                    return Convert.ToInt32(ViewState["TenantId"]);
                return 0;
            }
            set
            {
                ViewState["TenantId"] = value;
            }
        }
        public Boolean IsMultipleFileDownloadInZip
        {
            get;
            set;
        }

        public String MultipleDocZipFilePath
        {
            get;
            set;
        }

        public Int32 SystemDocumentID
        {
            get;
            set;
        }

        public String SystemDocumentType
        {
            get;
            set;
        }

        public int ClientSystemDocumentID
        {
            get;
            set;
        }

        public int SharedSystemDocumentID
        {
            get;
            set;
        }

        public Boolean IsDirectFileDownload
        {
            get;
            set;
        }

        public Boolean IsFileDownloadFromFilePath
        {
            get;
            set;
        }

        public String FilePath
        {
            get;
            set;
        }

        public String FileName
        {
            get;
            set;
        }

        //UAT-1538:Unified Document/single document option and updates to document exports
        public Boolean IsPDFFileDownload
        {
            get;
            set;
        }
        public Boolean IsFulfillmentDetail
        {
            get;
            set;
        }

        //Download report issue fix
        public Boolean IsTempFileLoaction
        {
            get;
            set;
        }

        //UAT-3962
        public Boolean IsComplianceIndividualDoc
        {
            get;
            set;
        }
        //END UAT-3962

        //cabs Additional type File Download
        public Boolean ManageOrderFulfillmentQueueType
        {
            get;
            set;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                if (Request.QueryString["zipFilePath"] != null)
                    MultipleDocZipFilePath = Convert.ToString(Request.QueryString["zipFilePath"]);
                if (Request.QueryString["IsMultipleFileDownloadInZip"] != null)
                    IsMultipleFileDownloadInZip = Convert.ToString(Request.QueryString["IsMultipleFileDownloadInZip"]).ToLower() == "true" ? true : false;
                if (Request.QueryString["IsDirectFileDownload"] != null)
                    IsDirectFileDownload = Convert.ToString(Request.QueryString["IsDirectFileDownload"]).ToLower() == "true" ? true : false;
                if (Request.QueryString["IsFileDownloadFromFilePath"] != null)
                    IsFileDownloadFromFilePath = Convert.ToString(Request.QueryString["IsFileDownloadFromFilePath"]).ToLower() == "true" ? true : false;
                if (Request.QueryString["FilePath"] != null)
                {
                    FilePath = Convert.ToString(Request.QueryString["FilePath"]);
                }
                if (Request.QueryString["FileName"] != null)
                {
                    FileName = Convert.ToString(Request.QueryString["FileName"]);
                }
                //Download report issue fixed.
                if (Request.QueryString["IsTempFileLoaction"] != null)
                {
                    IsTempFileLoaction = Convert.ToBoolean(Request.QueryString["IsTempFileLoaction"]);
                }
                else
                {
                    IsTempFileLoaction = false;
                }

                #region UAT-1405:should be a way to search all students and/ or instructors that were in a rotation during a given date range

                Boolean isRotationAppZipDoc = false;
                String zipfolderName = String.Empty;
                if (Request.QueryString["IsRotationAppZipDoc"] != null)
                {
                    isRotationAppZipDoc = Convert.ToString(Request.QueryString["IsRotationAppZipDoc"]).ToLower() == "true" ? true : false;
                }
                if (Request.QueryString["zipfolderName"] != null)
                {
                    zipfolderName = Convert.ToString(Request.QueryString["zipfolderName"]);
                }

                //UAT-1538:Unified Document/single document option and updates to document exports
                if (Request.QueryString["IsPDFFileDownload"] != null)
                {
                    IsPDFFileDownload = Convert.ToBoolean(Request.QueryString["IsPDFFileDownload"]);
                }
                //For EFT file extension download.
                //if (Request.QueryString["IsFulfillmentDetail"] != null)
                //{
                //    IsFulfillmentDetail = Convert.ToBoolean(Request.QueryString["IsFulfillmentDetail"]);
                //}


                #endregion

                Boolean aWSUseS3 = false;
                if (!ConfigurationManager.AppSettings["AWSUseS3"].IsNullOrEmpty())
                {
                    aWSUseS3 = Convert.ToBoolean(ConfigurationManager.AppSettings["AWSUseS3"]);
                }
                Presenter.OnViewInitialized();
                if (Request.QueryString["documentId"] != null)
                    ApplicantDocumentId = Convert.ToInt32(Request.QueryString["documentId"]);
                if (Request.QueryString["tenantId"] != null)
                    TenantId = Convert.ToInt32(Request.QueryString["tenantId"]);

                if (Request.QueryString["systemDocumentId"] != null)
                    SystemDocumentID = Convert.ToInt32(Request.QueryString["systemDocumentId"]);

                if (Request.QueryString["systemDocumentType"] != null)
                    SystemDocumentType = Convert.ToString(Request.QueryString["systemDocumentType"]);

                if (Request.QueryString["clientSystemDocumentId"] != null)
                    ClientSystemDocumentID = Convert.ToInt32(Request.QueryString["clientSystemDocumentId"]);

                if (Request.QueryString["sharedSystemDocumentID"] != null)
                    SharedSystemDocumentID = Convert.ToInt32(Request.QueryString["sharedSystemDocumentID"]);


                ManageOrderFulfillmentQueueType = false;
                if (Request.QueryString["ManageOrderFulfillmentQueueType"] != null)
                    ManageOrderFulfillmentQueueType = Convert.ToBoolean(Request.QueryString["ManageOrderFulfillmentQueueType"]);

                //UAT-3962
                if (Request.QueryString["IsComplianceIndividualDoc"] != null)
                    IsComplianceIndividualDoc = Convert.ToBoolean(Request.QueryString["IsComplianceIndividualDoc"]);
                //END UAT-3962


                if (IsMultipleFileDownloadInZip)
                {
                    DownloadMultipleFileInZip();
                }
                //UAT-1405:should be a way to search all students and/ or instructors that were in a rotation during a given date range
                else if (isRotationAppZipDoc)
                {
                    DownloadAppRequirementDocInZip(zipfolderName);
                }
                if (SystemDocumentType == "DownloadServiceForm") // Called for downloading the Service forms
                {
                    //Check whether use AWS S3, true if need to use
                    if (aWSUseS3 == false)
                    {
                        DownloadServiceForm(SystemDocumentID);
                    }
                    else
                    {
                        DownloadServiceFormS3(SystemDocumentID);
                    }
                }
                //UAT 1304 Instructor/Preceptor screens and functionality
                else if (ClientSystemDocumentID > AppConsts.NONE) //
                {
                    //Check whether use AWS S3, true if need to use
                    if (aWSUseS3 == false)
                    {
                        DownloadClientSystemDocument();
                    }
                    else
                    {
                        DownloadClientSystemDocumentS3();
                    }
                }
                //UAT 1304 Instructor/Preceptor screens and functionality
                else if (SharedSystemDocumentID > AppConsts.NONE) // Called for downloading the documents uploaded by Instructor/Preceptor. 
                {
                    //Check whether use AWS S3, true if need to use
                    if (aWSUseS3 == false)
                    {
                        DownloadSharedSystemDocument();
                    }
                    else
                    {
                        DownloadSharedSystemDocumentS3();
                    }
                }
                else if (IsDirectFileDownload)
                {
                    DirectlyDownloadFile(zipfolderName);
                }
                else if (IsFileDownloadFromFilePath) // Called for downloading the Contract & Site documents 
                {
                    if (aWSUseS3 == false || IsTempFileLoaction)
                    {
                        DownloadFileFromPath(FileName, FilePath);
                    }
                    else
                    {
                        DownloadFileFromPathS3(FileName, FilePath);
                    }
                }
                else // Called for downloading the Applicant documents from different scenarios
                {



                    //Check whether use AWS S3, true if need to use
                    if (aWSUseS3 == false)
                    {
                        Initialize();
                    }
                    else
                    {
                        InitializeS3Documents();
                    }
                    Presenter.OnViewLoaded();
                }
            }
        }

        public DoccumentDownloadPresenter Presenter
        {
            get
            {
                this._presenter.View = this; return this._presenter;
            }
            set
            {
                this._presenter = value;
                this._presenter.View = this;
            }
        }

        public void Initialize()
        {
            Entity.ClientEntity.ApplicantDocument document = Presenter.GetApplicantDocument();
            if (document != null)
            {
                //UAT-1538:Unified Document/single document option and updates to document exports
                #region UAT-1538:Unified Document/single document option and updates to document exports
                String filepath = null;
                String fileName = GetFileName(document.FileName);

                //UAT-3962
                if (IsComplianceIndividualDoc)
                {
                    String applicantName = document.OrganizationUser.IsNullOrEmpty() ? String.Empty :
                                            (document.OrganizationUser.FirstName + " " + document.OrganizationUser.LastName);
                    fileName = (String.IsNullOrEmpty(applicantName) ? "" : applicantName.Trim().Replace(" ", "_") + "_") + fileName + "_"
                                + (document.OrganizationUser.IsNullOrEmpty() ? String.Empty : document.OrganizationUser.OrganizationUserID.ToString());
                }
                //END UAT-3962

                if (IsPDFFileDownload && document.PdfDocPath != null)
                {
                    filepath = document.PdfDocPath;
                }
                //For EFT file extension download.
                //else if (IsFulfillmentDetail)
                //{

                //    filepath = (document.DocumentPath).Replace("nist","eft");
                //}
                else
                {
                    filepath = document.DocumentPath;
                }
                //String filepath = document.DocumentPath;
                #endregion

                String extension = Path.GetExtension(filepath);
                System.IO.Stream stream = null;
                if (ManageOrderFulfillmentQueueType)
                {
                    extension = ".eft";
                    var OriginalfileBytes  =  System.IO.File.ReadAllBytes(filepath);
                    NISTSpecification nISTSpecification = new NISTSpecification();

                    OriginalfileBytes = nISTSpecification.RemoveField(2, "009", OriginalfileBytes);
                    OriginalfileBytes = nISTSpecification.RemoveField(2, "016", OriginalfileBytes);
                    OriginalfileBytes = nISTSpecification.RemoveField(2, "037", OriginalfileBytes);
                    OriginalfileBytes = nISTSpecification.RemoveField(2, "039", OriginalfileBytes);
                    OriginalfileBytes = nISTSpecification.RemoveField(2, "041", OriginalfileBytes);
                    OriginalfileBytes = nISTSpecification.RemoveField(2, "073", OriginalfileBytes);

                    stream = new MemoryStream(OriginalfileBytes);
                }
                try
                {
                    if(!ManageOrderFulfillmentQueueType)
                    {
                        stream = new System.IO.FileStream(filepath, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
                    }

                    String _validFileName = ValidateFileName(fileName);
                    
                    long bytesToRead = stream.Length;
                    Response.ContentType = GetContentType(extension);
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + _validFileName + extension);

                    while (bytesToRead > 0)
                    {
                        if (Response.IsClientConnected)
                        {
                            byte[] buffer = new Byte[10000];
                            int length = stream.Read(buffer, 0, 10000);
                            Response.OutputStream.Write(buffer, 0, length);
                            Response.Flush();
                            bytesToRead = bytesToRead - length;
                        }
                        else
                        {
                            bytesToRead = -1;
                        }
                    }
                }
                //Do not log thread abort exception if it is caused by Response.Redirect or Response.End
                //catch (ThreadAbortException thex)
                //{
                //    //You can ignore this 
                //}
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                }
                finally
                {
                    if (stream != null)
                    {
                        stream.Close();
                        Response.End();
                    }
                }
            }
        }

        /// <summary>
        /// To initialize or get Document from Amazon S3 server
        /// </summary>
        /// <param name="documentPath"></param>
        /// <param name="fileName"></param>
        public void InitializeS3Documents()
        {
            Response.Clear();
            Response.Buffer = true;
            try
            {
                Entity.ClientEntity.ApplicantDocument document = Presenter.GetApplicantDocument();
                if (document != null)
                {
                    AmazonS3Documents objAmazonS3Documents = new AmazonS3Documents();

                    #region UAT-1538:Unified Document/single document option and updates to document exports
                    //UAT-1538:Unified Document/single document option and updates to document exports
                    String filepath = null;
                    String fileName = GetFileName(document.FileName);

                    //UAT-3962
                    if (IsComplianceIndividualDoc)
                    {
                        String applicantName = document.OrganizationUser.IsNullOrEmpty() ? String.Empty :
                                                (document.OrganizationUser.FirstName + " " + document.OrganizationUser.LastName);
                        fileName = (String.IsNullOrEmpty(applicantName) ? "" : applicantName.Trim().Replace(" ", "_") + "_") + fileName + "_"
                                    + (document.OrganizationUser.IsNullOrEmpty() ? String.Empty : document.OrganizationUser.OrganizationUserID.ToString());
                    }
                    //END UAT-3962

                    if (IsPDFFileDownload && document.PdfDocPath != null)
                    {
                        filepath = document.PdfDocPath;
                    }
                    //For EFT file extension download.
                    //else if (IsFulfillmentDetail)
                    //{

                    //    filepath = (document.DocumentPath).Replace("nist", "eft");
                    //}
                    else
                    {
                        filepath = document.DocumentPath;
                    }
                    #endregion

                    byte[] documentContent = objAmazonS3Documents.RetrieveDocument(filepath);

                    if (!documentContent.IsNullOrEmpty())
                    {
                        String extension = Path.GetExtension(filepath);

                        if (ManageOrderFulfillmentQueueType)
                        {
                            extension = ".eft";
                            NISTSpecification nISTSpecification = new NISTSpecification();
                            documentContent = nISTSpecification.RemoveField(2, "009", documentContent);
                            documentContent = nISTSpecification.RemoveField(2, "016", documentContent);
                            documentContent = nISTSpecification.RemoveField(2, "037", documentContent);
                            documentContent = nISTSpecification.RemoveField(2, "039", documentContent);
                            documentContent = nISTSpecification.RemoveField(2, "041", documentContent);
                            documentContent = nISTSpecification.RemoveField(2, "073", documentContent);
                        }

                        Response.ClearHeaders();
                        Response.Clear();
                        Response.AddHeader("Content-Length", documentContent.Length.ToString());
                        Response.ContentType = GetContentType(extension);

                        String _validFileName = ValidateFileName(fileName);
                        //Add Content-Disposition HTTP header
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + _validFileName + extension);

                        // Send the data to the browser
                        Response.BinaryWrite(documentContent);
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }

            finally
            {
                //Response.Flush();
                Response.End();
            }
        }

        /// <summary>
        /// Get content type based on the file type.
        /// </summary>
        /// <param name="fileExtension">Extension of the file to download.</param>
        /// <returns>Content type for the file.</returns>
        private String GetContentType(String fileExtension)
        {
            switch (fileExtension)
            {
                case ".txt":
                    return "text/plain";
                case ".doc":
                case ".docx":
                    return "application/ms-word";
                case ".tiff":
                case ".tif":
                    return "image/tiff";
                case ".zip":
                    return "application/zip"; 
                case ".xlsx":
                    return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                case ".xls":
                case ".csv":
                    return "application/vnd.ms-excel";
                case ".gif":
                    return "image/gif";
                case ".jpg":
                case "jpeg":
                    return "image/jpeg";
                case ".bmp":
                    return "image/bmp";
                case ".pdf":
                    return "application/pdf";
                case ".ppt":
                    return "application/mspowerpoint";
                default:
                    return "application/octet-stream";
            }
        }

        /// <summary>
        /// Method to download multiple files in zip folder.
        /// </summary>
        private void DownloadMultipleFileInZip()
        {
            Response.Clear();
            Response.BufferOutput = false;
            try
            {
                Response.ContentType = "application/zip";
                Response.AddHeader("Content-Disposition", "attachment; filename=DownloadedDocument.zip");
                ZipFile zip = new ZipFile();
                zip.AddDirectory(MultipleDocZipFilePath);
                zip.Save(Response.OutputStream);
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
            finally
            {
                if (Directory.Exists(MultipleDocZipFilePath))
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(MultipleDocZipFilePath);
                    dirInfo.Delete(true);
                }
                Response.End();
            }
        }

        #region UAT- 1159 WB: Add link to all Electronic service forms where the e-drug link is on the student dashboard.

        /// <summary>
        /// Method is used to download the Pdf from SystemDocument Table 
        /// </summary>
        /// <param name="SystemDocID"></param>
        public void DownloadServiceForm(Int32 SystemDocID)
        {
            Entity.ClientEntity.SystemDocument document = Presenter.GetServiceFormDocumentData();
            if (document != null)
            {
                String filepath = document.DocumentPath;
                String extension = Path.GetExtension(filepath);
                System.IO.Stream stream = null;
                try
                {
                    stream = new System.IO.FileStream(filepath, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);

                    String _validFileName = ValidateFileName(document.FileName);

                    long bytesToRead = stream.Length;
                    Response.ContentType = GetContentType(extension);
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + _validFileName);

                    while (bytesToRead > 0)
                    {
                        if (Response.IsClientConnected)
                        {
                            byte[] buffer = new Byte[10000];
                            int length = stream.Read(buffer, 0, 10000);
                            Response.OutputStream.Write(buffer, 0, length);
                            Response.Flush();
                            bytesToRead = bytesToRead - length;
                        }
                        else
                        {
                            bytesToRead = -1;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                }
                finally
                {
                    if (stream != null)
                    {
                        stream.Close();
                        Response.End();
                    }
                }
            }
        }

        /// <summary>
        /// To initialize or get Document from Amazon S3 server
        /// Method is used to download the Pdf from SystemDocument Table 
        /// </summary>
        /// <param name="documentPath"></param>
        /// <param name="fileName"></param>
        public void DownloadServiceFormS3(Int32 SystemDocID)
        {
            Response.Clear();
            Response.Buffer = true;
            try
            {
                Entity.ClientEntity.SystemDocument document = Presenter.GetServiceFormDocumentData();
                if (document != null)
                {
                    AmazonS3Documents objAmazonS3Documents = new AmazonS3Documents();
                    byte[] documentContent = objAmazonS3Documents.RetrieveDocument(document.DocumentPath);
                    if (!documentContent.IsNullOrEmpty())
                    {
                        String extension = Path.GetExtension(document.DocumentPath);
                        Response.ClearHeaders();
                        Response.Clear();
                        Response.AddHeader("Content-Length", documentContent.Length.ToString());
                        Response.ContentType = GetContentType(extension);

                        String _validFileName = ValidateFileName(document.FileName);
                        //Add Content-Disposition HTTP header
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + _validFileName);

                        // Send the data to the browser
                        Response.BinaryWrite(documentContent);
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }

            finally
            {
                //Response.Flush();
                Response.End();
            }
        }
        #endregion

        #region UAT 1304 Instructor/Preceptor screens and functionality

        private void DownloadClientSystemDocument()
        {
            Entity.ClientEntity.ClientSystemDocument document = Presenter.GetClientSystemDocument();
            if (document != null)
            {
                String filepath = document.CSD_DocumentPath;
                DownloadFileFromPath(document.CSD_FileName, filepath);
            }
        }

        private void DownloadFileFromPath(String fileName, String filepath)
        {
            String extension = Path.GetExtension(filepath);
            System.IO.Stream stream = null;
            try
            {
                if (fileName.IsNullOrEmpty())
                {
                    fileName = Path.GetFileName(filepath);
                }
                stream = new System.IO.FileStream(filepath, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);

                long bytesToRead = stream.Length;
                Response.ContentType = GetContentType(extension);

                String _validFileName = GetFileName(fileName) + Path.GetExtension(fileName);
                Response.AddHeader("Content-Disposition", "attachment; filename=" + _validFileName);

                while (bytesToRead > 0)
                {
                    if (Response.IsClientConnected)
                    {
                        byte[] buffer = new Byte[10000];
                        int length = stream.Read(buffer, 0, 10000);
                        Response.OutputStream.Write(buffer, 0, length);
                        Response.Flush();
                        bytesToRead = bytesToRead - length;
                    }
                    else
                    {
                        bytesToRead = -1;
                    }
                }
            }
            //Do not log thread abort exception if it is caused by Response.Redirect or Response.End
            //catch (ThreadAbortException thex)
            //{
            //    //You can ignore this 
            //}
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                    Response.End();
                }
            }
        }

        private void DownloadClientSystemDocumentS3()
        {
            Response.Clear();
            Response.Buffer = true;
            try
            {
                Entity.ClientEntity.ClientSystemDocument document = Presenter.GetClientSystemDocument();
                if (document != null)
                {
                    DownloadFileFromPathS3(document.CSD_FileName, document.CSD_DocumentPath);
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }

            finally
            {
                //Response.Flush();
                Response.End();
            }
        }

        private void DownloadFileFromPathS3(String fileName, String filepath)
        {
            AmazonS3Documents objAmazonS3Documents = new AmazonS3Documents();
            byte[] documentContent = objAmazonS3Documents.RetrieveDocument(filepath);
            if (!documentContent.IsNullOrEmpty())
            {
                if (fileName.IsNullOrEmpty())
                {
                    fileName = Path.GetFileName(filepath);
                }
                String extension = Path.GetExtension(filepath);
                Response.ClearHeaders();
                Response.Clear();
                Response.AddHeader("Content-Length", documentContent.Length.ToString());
                Response.ContentType = GetContentType(extension);

                //String _validFileName = ValidateFileName(fileName);
                String _validFileName = GetFileName(fileName) + Path.GetExtension(fileName);
                //Add Content-Disposition HTTP header
                Response.AddHeader("Content-Disposition", "attachment; filename=" + _validFileName);

                // Send the data to the browser
                Response.BinaryWrite(documentContent);
            }
        }

        private void DownloadSharedSystemDocument()
        {
            SharedSystemDocument document = Presenter.GetSharedSystemDocument();
            if (document != null)
            {
                String filepath = document.SSD_DocumentPath;
                String extension = Path.GetExtension(filepath);
                System.IO.Stream stream = null;
                try
                {

                    stream = new System.IO.FileStream(filepath, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);

                    long bytesToRead = stream.Length;
                    Response.ContentType = GetContentType(extension);

                    String _validFileName = GetFileName(Path.GetFileName(document.SSD_FileName));
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + _validFileName + extension);

                    while (bytesToRead > 0)
                    {
                        if (Response.IsClientConnected)
                        {
                            byte[] buffer = new Byte[10000];
                            int length = stream.Read(buffer, 0, 10000);
                            Response.OutputStream.Write(buffer, 0, length);
                            Response.Flush();
                            bytesToRead = bytesToRead - length;
                        }
                        else
                        {
                            bytesToRead = -1;
                        }
                    }
                }
                //Do not log thread abort exception if it is caused by Response.Redirect or Response.End
                //catch (ThreadAbortException thex)
                //{
                //    //You can ignore this 
                //}
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                }
                finally
                {
                    if (stream != null)
                    {
                        stream.Close();
                        Response.End();
                    }
                }
            }
        }

        private void DownloadSharedSystemDocumentS3()
        {
            Response.Clear();
            Response.Buffer = true;
            try
            {
                SharedSystemDocument document = Presenter.GetSharedSystemDocument();
                if (document != null)
                {
                    AmazonS3Documents objAmazonS3Documents = new AmazonS3Documents();
                    byte[] documentContent = objAmazonS3Documents.RetrieveDocument(document.SSD_DocumentPath);
                    if (!documentContent.IsNullOrEmpty())
                    {
                        String extension = Path.GetExtension(document.SSD_DocumentPath);
                        Response.ClearHeaders();
                        Response.Clear();
                        Response.AddHeader("Content-Length", documentContent.Length.ToString());
                        Response.ContentType = GetContentType(extension);

                        String _validFileName = GetFileName(Path.GetFileName(document.SSD_FileName));
                        //Add Content-Disposition HTTP header
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + _validFileName + extension);

                        // Send the data to the browser
                        Response.BinaryWrite(documentContent);
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }

            finally
            {
                //Response.Flush();
                Response.End();
            }
        }
        #endregion

        #region UAT-1405:should be a way to search all students and/ or instructors that were in a rotation during a given date range
        private void DownloadAppRequirementDocInZip(String folderName)
        {
            String tempFilePath = WebConfigurationManager.AppSettings["TemporaryFileLocation"];
            if (!tempFilePath.EndsWith(@"\"))
            {
                tempFilePath += @"\";
            }
            tempFilePath += folderName;
            MultipleDocZipFilePath = tempFilePath;
            DownloadMultipleFileInZip();
        }

        #endregion

        private void DirectlyDownloadFile(String folderName)
        {
            Response.Clear();
            Response.BufferOutput = false;
            String tempFilePath = WebConfigurationManager.AppSettings["TemporaryFileLocation"];
            if (!tempFilePath.EndsWith(@"\"))
            {
                tempFilePath += @"\";
            }
            tempFilePath += folderName;
            MultipleDocZipFilePath = tempFilePath;
            System.IO.Stream stream = null;
            if (!MultipleDocZipFilePath.IsNullOrEmpty())
            {
                try
                {
                    String filepath = Directory.GetFiles(MultipleDocZipFilePath).FirstOrDefault();
                    String extension = Path.GetExtension(filepath);


                    stream = new System.IO.FileStream(filepath, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);

                    long bytesToRead = stream.Length;
                    Response.ContentType = GetContentType(extension);
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + '"' + Path.GetFileName(filepath) + '"');

                    while (bytesToRead > 0)
                    {
                        if (Response.IsClientConnected)
                        {
                            byte[] buffer = new Byte[10000];
                            int length = stream.Read(buffer, 0, 10000);
                            Response.OutputStream.Write(buffer, 0, length);
                            Response.Flush();
                            bytesToRead = bytesToRead - length;
                        }
                        else
                        {
                            bytesToRead = -1;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                }
                finally
                {
                    if (stream != null)
                    {
                        stream.Close();
                    }
                    if (Directory.Exists(MultipleDocZipFilePath))
                    {
                        DirectoryInfo dirInfo = new DirectoryInfo(MultipleDocZipFilePath);
                        dirInfo.Delete(true);
                    }
                    Response.End();
                }
            }
        }

        //UAT-1538:Unified Document/single document option and updates to document exports
        private String GetFileName(String fileNameWithExt)
        {
            String[] splitFileName = fileNameWithExt.Split('.');
            String tempFileName = String.Join(".", splitFileName.Take(splitFileName.Length - 1));
            tempFileName = tempFileName.Replace(@",", @"_");
            tempFileName = tempFileName.Replace(@"\", @"-");
            tempFileName = tempFileName.Replace(@" ", @"_");
            return tempFileName;
        }

        /// <summary>
        /// Replace the comma's in the FileName with underscore - UAT 1810 fix for Chrome browser download issue.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private String ValidateFileName(String fileName)
        {
            if (fileName.Contains(','))
            {
                return fileName.Replace(',', '_');
            }
            else
            {
                return fileName;
            }
        }
    }
}

