using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreWeb.ClinicalRotation.Views;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using RadPdf.Data.Document;

namespace CoreWeb.ClinicalRotation.Pages
{
    public partial class RequirementVerificationDocViewer : BaseWebPage, IRequirementVerificationDocViewerView
    {
        #region VARIABLES
        private String _viewType;
        private RequirementVerificationDocViewerPresenter _presenter = new RequirementVerificationDocViewerPresenter();

        #endregion

        #region PRESENTER OBJECT

        public RequirementVerificationDocViewerPresenter Presenter
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

        #region PAGE EVENTS
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.Header.Controls.Add(new LiteralControl(" <link href='" + ResolveUrl("~/App_Themes/Default/core.css") + "' rel='stylesheet' />"));
                if (!this.IsPostBack)
                {
                    Dictionary<String, String> args = new Dictionary<String, String>();
                    if (!Request.QueryString["args"].IsNull())
                    {
                        CaptureQuerystringParameters(args);
                    }
                    Presenter.OnViewInitialized();
                }
                BindDocuments();
                Presenter.OnViewLoaded();
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

        protected void btnReloadPdfVwr_Click(object sender, EventArgs e)
        {
            try
            {
                BindDocuments();
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

        #endregion

        #region METHODS

        /// <summary>
        /// This Methods set the different property on the page extracting from the query string.
        /// </summary>
        /// <param name="args">Query string parameter thsat contains the value.</param>
        private void CaptureQuerystringParameters(Dictionary<string, string> args)
        {
            if (args.IsNotNull())
            {
                args.ToDecryptedQueryString(Request.QueryString["args"]);

                if (args.ContainsKey("SelectedTenantID"))
                {
                    CurrentViewContext.SelectedTenantId = Convert.ToInt32(args["SelectedTenantID"]);
                }

                if (args.ContainsKey("DocumentID"))
                {
                    CurrentViewContext.CurrentDocumentId = Convert.ToInt32(args["DocumentID"]);
                }
            }

        }

        private void BindDocuments()
        {
            Presenter.GetApplicantDocument();

            if (CurrentViewContext.ApplicantDocument.IsNull())
            {
                SetPdfControlMessage("Document Not found.");
            }
            else
            {
                byte[] pdfData = Presenter.GetPDFByteData();
                //Load PDF byte array into RAD PDF
                if (pdfData.IsNotNull())
                {
                    PdfWebControl1.CreateDocument("PDF File", pdfData, PdfDocumentSettings.DisableAlterBookmarks);
                }
                else
                {
                    SetPdfControlMessage("Unable to Load PDF document. Please click 'Reload' button to try again.");
                }
            }
        }

        private void SetPdfControlMessage(String messageToShow)
        {
            dvPdfDocuViewer.Visible = false;
            lblPdfMessage.Visible = true;
            lblPdfMessage.Text = messageToShow;
            lblPdfMessage.CssClass = "info";
            //base.ShowInfoMessage(messageToShow);
        }

        #endregion

        #region PROPERTIES
        public IRequirementVerificationDocViewerView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        Int32 IRequirementVerificationDocViewerView.SelectedTenantId
        {
            get
            {
                return (Int32)(ViewState["SelectedTenantId"]);
            }
            set
            {
                ViewState["SelectedTenantId"] = value;
            }
        }

        Int32 IRequirementVerificationDocViewerView.OrganizationUserId
        {
            get
            {
                return (Int32)(ViewState["OrganizationUserId_User"]);
            }
            set
            {
                ViewState["OrganizationUserId_User"] = value;
            }
        }


        Int32 IRequirementVerificationDocViewerView.CurrentDocumentId
        {
            get
            {
                return (Int32)(ViewState["CurrentDocumentId"]);
            }
            set
            {
                ViewState["CurrentDocumentId"] = value;
            }
        }

        ApplicantDocument IRequirementVerificationDocViewerView.ApplicantDocument { get; set; }

        #endregion


    }
}