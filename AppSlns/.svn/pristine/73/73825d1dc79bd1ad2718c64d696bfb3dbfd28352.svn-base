using System;
using Microsoft.Practices.ObjectBuilder;
using System.IO;
using System.Text;
using CoreWeb.Shell;
using System.Configuration;
using CoreWeb.IntsofSecurityModel;
using INTSOF.Utils;


namespace CoreWeb.ComplianceOperations.Views
{
    public partial class ManageDownloadPdf : BaseUserControl, IManageDownloadPdfView
    {
        #region Private variables

        private ManageDownloadPdfPresenter _presenter = new ManageDownloadPdfPresenter();
        private Int32 _tenantid = 0;

        #endregion

        #region Public Properties

        public String FileIdentifier
        {
            get
            {
                return hdnFileIdentifier.Value;
            }

            set
            {
                hdnFileIdentifier.Value = value;
            }


        }
        public Int32 CurrentLoggedInUserID
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }

        }
        public String PackageName
        {
            get;
            set;
        }
        public String FilePath
        {
            get;
            set;
        }
        public String PageHTML
        {
            get;
            set;

        }
        public Int32 PackageID
        {
            get
            {
                return (Int32)ViewState["PackageID"];
            }
            set
            {
                ViewState["PackageID"] = value;
            }
        }
        public Int32 TenantID
        {
            get
            {
                if (_tenantid == 0)
                {
                    //_tenantid = Presenter.GetTenantId();
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    if (user.IsNotNull())
                    {
                        _tenantid = user.TenantId.HasValue ? user.TenantId.Value : 0;
                    }
                }
                return _tenantid;
            }
            set
            {
                _tenantid = value;
            }
        }
        public IManageDownloadPdfView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public ManageDownloadPdfPresenter Presenter
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

        public Boolean Visiblity
        {
            get
            {
                if (ViewState["Visiblity"] != null)
                    return (Convert.ToBoolean(ViewState["Visiblity"]));
                return true;
            }
            set
            {
                ViewState["Visiblity"] = value;
            }
        }
        #endregion

        #region Events

        #region PageLoadEvent

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.IsPostBack)
            {
                if (Request.QueryString["PackageId"] != null)
                {
                    CurrentViewContext.PackageID = Convert.ToInt32(Request.QueryString["PackageId"].ToString());
                }
                if (Request.QueryString["PackageName"] != null)
                {
                    CurrentViewContext.PackageName = Request.QueryString["PackageName"].ToString();
                }
                if (Request.QueryString["TenantId"] != null)
                {
                    CurrentViewContext.TenantID = Convert.ToInt32(Request.QueryString["TenantId"].ToString());
                }
                Presenter.OnViewInitialized();
            }

            Presenter.OnViewLoaded();
        }
        #endregion

        #region ClickEvent

        protected void lnkDownloadPdf_click(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(PageHTML))
                {
                    SavePageHtmlContet(PageHTML);
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "DownloadPdf();", true);
                }
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }
        #endregion

        #endregion

        #region Private Method
        /// <summary>
        /// This method write the page html into text file and save the file path in database.
        /// </summary>
        /// <param name="pageHtml"></param>
        public void SavePageHtmlContet(String pageHtml)
        {
            try
            {
                if (!String.IsNullOrEmpty(pageHtml))
                {
                    FilePath = ConfigurationManager.AppSettings["TemporaryFileLocation"].ToString();
                    String dataHelpFileName = String.Empty;
                    String applicantFileLocation = String.Empty;
                    if (!FilePath.EndsWith("\\"))
                    {
                        FilePath += "\\";
                    }
                    FilePath += @"HTMLConversion\";
                    if (!Directory.Exists(FilePath))
                        Directory.CreateDirectory(FilePath);
                    Guid Id = Guid.NewGuid();
                    String fileName = "TempFile_" + Id + ".txt";
                    if (!String.IsNullOrEmpty(PackageName))
                    {
                        dataHelpFileName = PackageName + "_" + "Help";
                    }
                    FileIdentifier = Convert.ToString(Id);
                    if (!File.Exists(fileName))
                    {
                        FilePath = Path.Combine(FilePath, fileName);

                        using (FileStream fs = File.Create(FilePath))
                        {
                            Byte[] info = new UTF8Encoding(true).GetBytes(pageHtml);
                            fs.Write(info, 0, info.Length);
                        }
                        //Check whether use AWS S3, true if need to use
                        Boolean aWSUseS3 = false;
                        if (!ConfigurationManager.AppSettings["AWSUseS3"].IsNullOrEmpty())
                        {
                            aWSUseS3 = Convert.ToBoolean(ConfigurationManager.AppSettings["AWSUseS3"]);
                        }
                        if (aWSUseS3 == false)
                        {
                            //Move file to other location
                            applicantFileLocation = ConfigurationManager.AppSettings["ApplicantFileLocation"].ToString();
                            if (!applicantFileLocation.EndsWith("\\"))
                            {
                                applicantFileLocation += "\\";
                            }
                            applicantFileLocation += @"HTMLConversion\";
                            if (!Directory.Exists(applicantFileLocation))
                            {
                                Directory.CreateDirectory(applicantFileLocation);
                            }
                            applicantFileLocation = Path.Combine(applicantFileLocation, fileName);
                            MoveFile(FilePath, applicantFileLocation);
                        }
                        else
                        {
                            applicantFileLocation = ConfigurationManager.AppSettings["ApplicantFileLocation"].ToString();
                            if (!applicantFileLocation.EndsWith("//"))
                            {
                                applicantFileLocation += "//";
                            }
                            //AWS code to save document to S3 location
                            AmazonS3Documents objAmazonS3 = new AmazonS3Documents();
                            String destFolder = applicantFileLocation + @"HTMLConversion/";
                            String returnFilePath = objAmazonS3.SaveDocument(FilePath, fileName, destFolder);
                            applicantFileLocation = returnFilePath; //destFolder + fileName;
                            try
                            {
                                if (!String.IsNullOrEmpty(FilePath))
                                    File.Delete(FilePath);
                            }
                            catch (Exception) { }
                        }
                        //Presenter.SavePageHtmlContentLocation(FilePath, Id, dataHelpFileName);
                        Presenter.SavePageHtmlContentLocation(applicantFileLocation, Id, dataHelpFileName);
                    }
                }
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// Move file to other location
        /// </summary>
        /// <param name="sourceFilePath"></param>
        /// <param name="destinationFilePath"></param>
        private static void MoveFile(String sourceFilePath, String destinationFilePath)
        {
            if (!String.IsNullOrEmpty(sourceFilePath))
            {
                File.Copy(sourceFilePath, destinationFilePath);
            }
            try
            {
                if (!String.IsNullOrEmpty(sourceFilePath))
                    File.Delete(sourceFilePath);
            }
            catch (Exception) { }
        }

        #endregion
    }
}

