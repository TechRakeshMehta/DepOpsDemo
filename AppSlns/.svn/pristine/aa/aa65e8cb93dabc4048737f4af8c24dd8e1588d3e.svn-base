using CoreWeb.ComplianceOperations.Views;
using Entity.ClientEntity;
using INTSOF.Utils;
using RadPdf.Data.Document;
using System;
using System.Collections.Generic;
using System.Web.UI;

namespace CoreWeb.ComplianceOperations
{
    public partial class AdminDataEntryDocViewer : Page, IAdminDataEntryDocViewer
    {
        #region Properties

        #region Private Properties

        private AdminDataEntryDocViewerPresenter _presenter = new AdminDataEntryDocViewerPresenter();

        #endregion

        #region Public Properties

        public AdminDataEntryDocViewerPresenter Presenter
        {
            get
            {
                this._presenter.View = this;
                return this._presenter;
            }
            set
            {
                this._presenter = value;
                this._presenter.View = this;
            }
        }

        public IAdminDataEntryDocViewer CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        Int32 IAdminDataEntryDocViewer.ApplicantDocumentId
        {
            get;
            set;
        }

        Int32 IAdminDataEntryDocViewer.TenantId
        {
            get;
            set;
        }

        ApplicantDocument IAdminDataEntryDocViewer.ApplicantDocument
        {
            get;
            set;
        }

        #endregion

        #endregion

        #region Events

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.Header.Controls.Add(new LiteralControl(" <link href='" + ResolveUrl("~/App_Themes/Default/core.css") + "' rel='stylesheet' />"));

                Dictionary<String, String> args = new Dictionary<String, String>();
                if (!Request.QueryString["args"].IsNull())
                {
                    args.ToDecryptedQueryString(Request.QueryString["args"]);

                    if (args.ContainsKey("DocumentId"))
                    {
                        if (!String.IsNullOrEmpty(args["DocumentId"]))
                            CurrentViewContext.ApplicantDocumentId = Convert.ToInt32(args["DocumentId"]);
                        hdnDocIDDocViewer.Value = args["DocumentId"];
                    }
                    if (args.ContainsKey("TenantId"))
                    {
                        CurrentViewContext.TenantId = Convert.ToInt32(args["TenantId"]);
                        hdnTenantIdCurrent.Value = Convert.ToString(CurrentViewContext.TenantId);
                    }

                    if (!this.IsPostBack)
                    {
                        RenderUnifiedPdfDocument();
                    }
                }

                if (!this.IsPostBack)
                {
                    Presenter.OnViewInitialized();
                }
            }
            catch (SysXException ex)
            {
                SetPdfControlMessage(ex.Message);
            }
            catch (System.Exception ex)
            {
                SetPdfControlMessage(ex.Message);
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "onDocReady_DocViewer();", true);
        }

        #endregion

        #region Button Events

        protected void btnReloadPdfVwr_Click(object sender, EventArgs e)
        {
            RenderUnifiedPdfDocument();
        }

        #endregion

        #endregion

        #region Methods

        #region Public Methods

        #endregion

        #region Private Methods

        private void RenderUnifiedPdfDocument()
        {
            try
            {
                Presenter.GetApplicantDocument();
                if (CurrentViewContext.ApplicantDocument.IsNotNull())
                {
                    byte[] pdfData = Presenter.GetPDFByteData();
                    //Load PDF byte array into RAD PDF
                    if (pdfData.IsNotNull())
                    {
                        PdfWebControl1.CreateDocument("PDF File", pdfData, PdfDocumentSettings.DisableAlterBookmarks);
                    }
                    else
                    {
                        SetPdfControlMessage("Unable to find the unified PDF document file.");
                    }
                }
                else
                {
                    SetPdfControlMessage("Unable to find the unified PDF document file.");
                }
            }
            catch (SysXException ex)
            {
                SetPdfControlMessage(ex.Message);
            }
            catch (System.Exception ex)
            {
                SetPdfControlMessage(ex.Message);
            }
        }

        private void SetPdfControlMessage(String messageToShow)
        {
            dvPdfDocuViewer.Visible = false;
            btnRotatePdfPage.Visible = false;
            lblPdfMessage.Text = messageToShow;
            lblPdfMessage.CssClass = "info";
            dvMsgBox.Attributes["class"] = "msgbox shw_dv";
        }

        #endregion

        #endregion








    }
}