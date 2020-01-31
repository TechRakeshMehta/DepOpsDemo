#region Namespaces

#region System Defined

using System;
using System.Collections.Generic;
using Microsoft.Practices.ObjectBuilder;


#endregion

#region Application Specific

using INTSOF.Utils;
using CoreWeb.Shell;
using INTSOF.UI.Contract.ComplianceOperation;
using RadPdf.Data.Document;
using RadPdf.Integration;
using System.Web;
using Entity.ClientEntity;

#endregion

#endregion

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class PdfDocumentViewer : BaseUserControl, IPdfDocumentViewerView
    {
        private PdfDocumentViewerPresenter _presenter = new PdfDocumentViewerPresenter();

        #region Variables

        #region Private Variables
        #endregion

        #endregion

        #region Properties

        public PdfDocumentViewerPresenter Presenter
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

        /// <summary>
        /// Gets the current view context.
        /// </summary>
        /// <remarks></remarks>

        public IPdfDocumentViewerView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public String UnifiedFilePath
        {
            get;
            set;
        }

        public String DocumentPath
        { get; set; }

        public Int32 OrganizationUserId
        {
            get;
            set;
        }

        public Int32 SelectedTenantId
        {
            get;
            set;
        }

        public Int32? SelectedCatUnifiedStartPageID
        {
            get;
            set;
        }

        public UnifiedPdfDocument UnifiedPdfDocument
        {
            get;
            set;
        }

        public List<ApplicantDocuments> lstApplicantDocument
        {
            get;
            set;
        }

        public Int32 ApplicantDocumentId
        {
            get;
            set;
        }

        public ApplicantDocument ApplicantDocument
        {
            get;
            set;
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Presenter.OnViewLoaded();
                if (!this.IsPostBack)
                {
                    RenderUnifiedPdfDocument();
                }

                if (!this.IsPostBack)
                {
                    Presenter.OnViewInitialized();
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        protected void PdfWebControl1_Saved(object sender, DocumentSavedEventArgs e)
        {
            switch (e.SaveType)
            {
                case DocumentSaveType.Save:
                    //Get saved PDF
                    byte[] pdfData = e.DocumentData;
                    //If desired, we could save the modified PDF to a file, database, send it via email, etc.
                    //For example:
                    System.IO.File.WriteAllBytes(@"D:\RadPDFOutput.pdf", pdfData);
                    break;
            }
        }

        protected void btnReloadPdfDoc_Click(object sender, EventArgs e)
        {
            Presenter.GetApplicantDocuments();

            if (HttpContext.Current.Items["UpdateDocumentList"] != null)
            {
                del = (UpdateDocumentList)HttpContext.Current.Items["UpdateDocumentList"];
                del(lstApplicantDocument);
            }
            RenderUnifiedPdfDocument();
        }

        private void RenderUnifiedPdfDocument()
        {
            try
            {
                Presenter.GetUnifiedDocument();

                if (CurrentViewContext.UnifiedPdfDocument.IsNull())
                {
                    //Merging Not Done
                    SetPdfControlMessage("Unified PDF document Not found.");

                }
                else if (CurrentViewContext.UnifiedPdfDocument.IsNotNull()
                    && CurrentViewContext.UnifiedPdfDocument.lkpDocumentStatu.DMS_Code == DocumentStatus.MERGING_IN_PROGRESS.GetStringValue())
                {
                    btnReloadPdfDoc.Text = "Load Document";
                    //MERGING_IN_PROGRESS
                    SetPdfControlMessage("Document under processing. Please click 'Load document' button to try again.");
                }
                else
                {
                    byte[] pdfData = Presenter.GetPDFByteData();
                    //Load PDF byte array into RAD PDF
                    if (pdfData.IsNotNull())
                    {
                        if (this.SelectedCatUnifiedStartPageID > AppConsts.NONE && this.SelectedCatUnifiedStartPageID.IsNotNull())
                        {
                            PdfWebControl1.ViewerPageDefault = this.SelectedCatUnifiedStartPageID.Value;
                        }
                        PdfWebControl1.CreateDocument("PDF File", pdfData, PdfDocumentSettings.DisableAlterBookmarks);
                    }
                    else
                    {
                        btnReloadPdfDoc.Text = "Reload";
                        SetPdfControlMessage("Unable to Load Unified PDF document. Please click 'Reload' button to try again.");
                    }
                }

            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        private void SetPdfControlMessage(String messageToShow)
        {
            dvPdfDocuViewer.Visible = false;
            btnRotatePdfDoc.Visible = false;
            hdnIsPdfDocLoaded.Value = AppConsts.ZERO;
            lblPdfMessage.Text = messageToShow;
            lblPdfMessage.CssClass = "info";
        }
    }
}