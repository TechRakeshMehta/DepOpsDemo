#region Namespaces

#region System Defined

using System;
using System.Web;
using System.Web.UI;
using System.Web.Security;
using System.Collections.Generic;
//using Microsoft.Practices.CompositeWeb;
using Microsoft.Practices.ObjectBuilder;
using System.Text;
using System.IO;
#endregion

#region Application Specific

using INTSOF.Utils;
using Entity;
using Telerik.Web.UI;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.IntsofSecurityModel.Providers;
using CoreWeb.IntsofSecurityModel.Interface.Services;
using INTERSOFT.WEB.UI.WebControls;
using INTERSOFT.WEB.UI.Config;
using System.Threading;
using INTSOF.UI.Contract.SysXSecurityModel;
using System.Web.Configuration;
using INTSOF.Logger;
using INTSOF.Logger.factory;
using System.Configuration;

#endregion

#endregion


namespace CoreWeb.Shell.Views
{
    public partial class TempFileViewer : BaseWebPage, ITempFileViewerView
    {
        #region Private Variables
        private TempFileViewerPresenter _presenter = new TempFileViewerPresenter();
        private ILogger _logger;
        #endregion

        #region Public Properties

        #region Constructor

        public TempFileViewer()
        {
            _logger = SysXLoggerFactory.GetInstance().GetLogger();
            _logger.Debug("TempFileViewer Constructor called.");
        }

        #endregion

        public Guid Id
        {
            get;
            set;
        }
        public String FilePath
        {
            get;
            set;
        }
        public Double TotalMinutes
        {
            get;
            set;
        }
        public TempFile TempFileRecord
        {
            get;
            set;
        }



        public TempFileViewerPresenter Presenter
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
        #endregion

        #region Page Load Event

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                GetQueryStringParameters();
                if (Id.IsNotNull())
                {
                    _logger.Debug("Get the File Path based on guid.");
                    Presenter.GetFilePath(Id);
                    if (TotalMinutes <= 2.0)
                    {
                        if (!String.IsNullOrEmpty(FilePath))
                        {
                            //Check whether use AWS S3, true if need to use
                            Boolean aWSUseS3 = false;
                            if (!ConfigurationManager.AppSettings["AWSUseS3"].IsNullOrEmpty())
                            {
                                aWSUseS3 = Convert.ToBoolean(ConfigurationManager.AppSettings["AWSUseS3"]);
                            }
                            Response.Clear();
                            _logger.Debug("Writing file content on TempFileViewer.aspx page.");
                            if (aWSUseS3 == false)
                            {
                                if (File.Exists(FilePath))
                                {
                                    Response.WriteFile(FilePath);
                                }
                                else
                                {
                                    _logger.Debug("Temp File Not Found:" + FilePath);
                                }
                            }
                            else
                            {
                                AmazonS3Documents objAmazonS3Documents = new AmazonS3Documents();
                                byte[] documentContent = objAmazonS3Documents.RetrieveDocument(FilePath);
                                if (!documentContent.IsNullOrEmpty())
                                    Response.BinaryWrite(documentContent);
                            }
                            _logger.Debug("Written file content on TempFileViewer.aspx page.");
                            Response.End();
                            _logger.Debug("Response End.");
                        }
                    }
                    else
                    {
                        Response.StatusCode = 404;
                        Response.SuppressContent = true;
                    }
                }
                if (!this.IsPostBack)
                {
                    Presenter.OnViewInitialized();
                }
                Presenter.OnViewLoaded();
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        #endregion

        #region Private Method

        private void GetQueryStringParameters()
        {
            try
            {
                Dictionary<String, String> queryString = new Dictionary<String, String>();
                if (!Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT].IsNull())
                {
                    queryString.ToDecryptedQueryString(Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT]);
                }
                if (queryString.ContainsKey("ID"))
                {
                    Id = new Guid(queryString["ID"]);
                }
                if (!Request.QueryString.IsNullOrEmpty())
                {
                    if (!Request.QueryString["ID"].IsNullOrEmpty())
                    {
                        Id = new Guid(Request.QueryString["ID"]);
                    }
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }
        #endregion
    }
}

