using CoreWeb.IntsofSecurityModel.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTSOF.Utils;
using Entity.ClientEntity;
using Telerik.Web.UI;
using RadPdf.Data.Document;
using System.Web.Configuration;
using System.IO;

namespace CoreWeb
{
    public partial class ExternalViewDocument : System.Web.UI.Page, IExternalViewDocument
    {

        #region [Variables]

        #region [Private]
        private ExternalViewDocumentPresenter _presenter = new ExternalViewDocumentPresenter();
        #endregion

        #region [Properties]

        public ExternalViewDocumentPresenter Presenter
        {
            get
            {
                this._presenter.View = this;
                return this._presenter;
            }
            set
            {
                this._presenter = value;
                this._presenter.View = this; ;
            }
        }

        public IExternalViewDocument CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public Int32 TenantID
        {
            get;
            set;
        }

        public String DocumentIDs
        {
            get;
            set;
        }

        public String ApplicantDocumentPath
        {
            get;
            set;
        }

        public List<ApplicantDocument> lstApplicantDocument
        {
            get
            {
                if (ViewState["lstApplicantDocument"].IsNullOrEmpty())
                {
                    Presenter.GetApplicantDocuments();
                }

                return (List<ApplicantDocument>)ViewState["lstApplicantDocument"];
            }
            set
            {
                ViewState["lstApplicantDocument"] = value;
            }
        }

        #endregion

        #endregion

        #region [Page Events]

        protected void Page_Load(object sender, EventArgs e)
        {
            pageMsgBox.Visible = false;
            ifrExportDocument.Src = string.Empty;

            if (!Session["DataToViewDocs"].IsNullOrEmpty())
            {
                Dictionary<int, string> dicDataToViewDocs = (Dictionary<int, string>)Session["DataToViewDocs"];
                CurrentViewContext.TenantID = !dicDataToViewDocs.FirstOrDefault().IsNullOrEmpty() ? dicDataToViewDocs.FirstOrDefault().Key : 0;
                CurrentViewContext.DocumentIDs = !dicDataToViewDocs.FirstOrDefault().IsNullOrEmpty() ? dicDataToViewDocs.FirstOrDefault().Value : string.Empty;
                dvDocsSection.Visible = true;
            }
            else
            {
                pageMsgBox.Visible = true;
                lblError.Text = "Unauthorized Request";
                dvDocsSection.Visible = false;
                lstApplicantDocument = new List<ApplicantDocument>();
                grdDocuments.DataBind();
            }
        }

        #endregion

        #region [Grid Events]

        protected void grdDocuments_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                grdDocuments.DataSource = lstApplicantDocument;

                if (!lstApplicantDocument.IsNullOrEmpty())
                {
                    CurrentViewContext.ApplicantDocumentPath = GetDocumentPath(lstApplicantDocument.FirstOrDefault().PdfDocPath, lstApplicantDocument.FirstOrDefault().DocumentPath);
                    RenderUnifiedPdfDocument();
                }

            }
            catch (SysXException ex)
            {
                lblError.Text = ex.Message;
                pageMsgBox.Visible = true;
            }
            catch (System.Exception ex)
            {
                lblError.Text = ex.Message;
                pageMsgBox.Visible = true;
            }
        }

        protected void grdDocuments_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "ViewDocument")
                {
                    Dictionary<String, String> queryString = new Dictionary<String, String>();
                    string pdfDocPath = Convert.ToString((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["PdfDocPath"]);
                    string documentPath = Convert.ToString((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["DocumentPath"]);

                    CurrentViewContext.ApplicantDocumentPath = GetDocumentPath(pdfDocPath, documentPath);
                    RenderUnifiedPdfDocument();
                }
            }
            catch (SysXException ex)
            {
                lblError.Text = ex.Message;
                pageMsgBox.Visible = true;
            }
            catch (System.Exception ex)
            {
                lblError.Text = ex.Message;
                pageMsgBox.Visible = true;
            }
        }

        #endregion

        #region Private Methods

        private void RenderUnifiedPdfDocument()
        {
            try
            {
                dvPdfDocuViewer.Visible = true;
                dvMsgBox.Attributes["class"] = "hide_dv";

                if (CurrentViewContext.ApplicantDocumentPath.IsNotNull())
                {
                    byte[] pdfData = Presenter.GetPDFByteData();

                    //Load PDF byte array into RAD PDF
                    if (pdfData.IsNotNull())
                    {
                        PdfWebControl1.CreateDocument("PDF File", pdfData, PdfDocumentSettings.DisableAlterBookmarks);
                    }
                    else
                    {
                        SetPdfControlMessage("Unable to find the document.");
                    }
                }
                else
                {
                    SetPdfControlMessage("Unable to find the document.");
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
            lblPdfMessage.Text = messageToShow;
            lblPdfMessage.CssClass = "info";
            dvMsgBox.Attributes["class"] = "msgbox shw_dv";
        }

        private void ExportDocuments()
        {
            List<ApplicantDocument> documentList = new List<ApplicantDocument>();
            Int32 fileCount = AppConsts.NONE;
            documentList = CurrentViewContext.lstApplicantDocument.DistinctBy(x => x.ApplicantDocumentID).ToList();

            if (documentList.IsNotNull() && documentList.Count > 0)
            {
                String tempFilePath = WebConfigurationManager.AppSettings["TemporaryFileLocation"];
                if (tempFilePath.IsNullOrEmpty())
                {

                    lblError.Text = "Please provide path for TemporaryFileLocation in config.";
                    pageMsgBox.Visible = true;
                    return;
                }
                if (!tempFilePath.EndsWith(@"\"))
                {
                    tempFilePath += @"\";
                }
                tempFilePath += "Tenant_" + CurrentViewContext.TenantID.ToString() + "_Zip_" + (DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("mmss")) + @"\";

                if (!Directory.Exists(tempFilePath))
                    Directory.CreateDirectory(tempFilePath);
                DirectoryInfo dirInfo = new DirectoryInfo(tempFilePath);
                try
                {
                    foreach (ApplicantDocument applicantDocumentToExport in documentList)
                    {
                        string docPath = GetDocumentPath(applicantDocumentToExport.PdfDocPath, applicantDocumentToExport.DocumentPath);

                        String fileExtension = Path.GetExtension(docPath);
                        //String fileName = Guid.NewGuid().ToString() + "_" + applicantDocumentToExport.FileName;
                        String fileName = GetFileName(applicantDocumentToExport.FileName);
                        String finalFileName = String.Concat(fileName, fileExtension);

                        String newTempFilePath = Path.Combine(tempFilePath, finalFileName);
                        byte[] fileBytes = null;
                        fileBytes = CommonFileManager.RetrieveDocument(docPath, FileType.ApplicantFileLocation.GetStringValue());

                        if (fileBytes.IsNotNull())
                        {
                            try
                            {
                                File.WriteAllBytes(newTempFilePath, fileBytes);
                            }
                            catch (Exception ex)
                            {
                                lblError.Text = "Error found in bytes write for DocumentID: " + applicantDocumentToExport.ApplicantDocumentID.ToString();
                                pageMsgBox.Visible = true;
                            }
                        }
                    }

                    fileCount = Directory.GetFiles(tempFilePath).Count();
                    if (fileCount > AppConsts.NONE)
                    {
                        ifrExportDocument.Src = "~/ComplianceOperations/UserControl/DoccumentDownload.aspx?zipFilePath=" + tempFilePath + "&IsMultipleFileDownloadInZip=" + "True";
                    }
                    else
                    {
                        lblError.Text = "No document(s) found to export.";
                        pageMsgBox.Visible = true;
                    }
                }
                catch (SysXException ex)
                {
                    lblError.Text = ex.Message;
                    pageMsgBox.Visible = true;
                }
                catch (System.Exception ex)
                {
                    lblError.Text = ex.Message;
                    pageMsgBox.Visible = true;
                }
            }
        }

        private String GetFileName(String fileNameWithExt)
        {
            fileNameWithExt = fileNameWithExt.Replace(@"\", @"-");
            fileNameWithExt = fileNameWithExt.Replace(@" ", @"_");
            return fileNameWithExt;
        }

        private String GetDocumentPath(string pdfDocPath, string documentPath)
        {
            string docPath = string.Empty;
            if (pdfDocPath.IsNullOrEmpty())
            {
                if (!documentPath.IsNullOrEmpty()
                         && documentPath.Length > 0
                          && Convert.ToString(documentPath.Split('.')[documentPath.Split('.').Length - 1]).ToLower() == "pdf")
                {
                    docPath = documentPath;
                }
            }
            else
            {
                docPath = pdfDocPath;
            }

            return docPath;
        }

        #endregion

        protected void fsucCmdExport_ExtraClick(object sender, EventArgs e)
        {
            try
            {
                ExportDocuments();
            }
            catch (SysXException ex)
            {
                lblError.Text = ex.Message;
                pageMsgBox.Visible = true;
            }
            catch (System.Exception ex)
            {
                lblError.Text = ex.Message;
                pageMsgBox.Visible = true;
            }
        }
    }
}