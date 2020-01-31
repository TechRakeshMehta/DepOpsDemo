using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using iTextSharp.text.pdf;
using RadPdf.Data.Document;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web.Configuration;
using System.Web.UI;

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class UnifiedPdfDocViewer : BaseWebPage, IPdfDocumentViewerView
    {
        private PdfDocumentViewerPresenter _presenter = new PdfDocumentViewerPresenter();

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

        public IPdfDocumentViewerView CurrentViewContext => this;

        public String UnifiedFilePath
        {
            get;
            set;
        }

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

        public String DocumentViewType
        {
            get;
            set;
        }

        public Int32 ApplicantDocumentId
        {
            get;
            set;
        }

        //public String DocumentPath
        //{
        //    get;
        //    set;
        //}

        public ApplicantDocument ApplicantDocument
        {
            get;
            set;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                BaseWebPage.LogOrderPDFViewer("Class Name: " + MethodBase.GetCurrentMethod().DeclaringType.Name + ":-  Function Name: " + System.Reflection.MethodBase.GetCurrentMethod().Name + " :- Call Method");
                this.Header.Controls.Add(new LiteralControl(" <link href='" + ResolveUrl("~/App_Themes/Default/core.css") + "' rel='stylesheet' />"));
                Presenter.OnViewLoaded();
                Dictionary<String, String> args = new Dictionary<String, String>();
                if (!Request.QueryString["args"].IsNull())
                {
                    Boolean isAuth = false;
                    args.ToDecryptedQueryString(Request.QueryString["args"]);

                    if (args.ContainsKey("IsRequestAuth"))
                    {
                        isAuth = Convert.ToBoolean(args["IsRequestAuth"]);
                    }
                    if (isAuth)
                    {
                        if (args.ContainsKey("SelectedCatUnifiedStartPageID"))
                        {
                            if (!String.IsNullOrEmpty(args["SelectedCatUnifiedStartPageID"]))
                            {
                                CurrentViewContext.SelectedCatUnifiedStartPageID = Convert.ToInt32(args["SelectedCatUnifiedStartPageID"]);
                            }
                        }
                        if (args.ContainsKey("OrganizationUserId"))
                        {
                            CurrentViewContext.OrganizationUserId = Convert.ToInt32(args["OrganizationUserId"]);
                            hdnApplicantIdCurrent.Value = Convert.ToString(CurrentViewContext.OrganizationUserId);
                        }
                        if (args.ContainsKey("SelectedTenantId"))
                        {
                            CurrentViewContext.SelectedTenantId = Convert.ToInt32(args["SelectedTenantId"]);
                            hdnTenantIdCurrent.Value = Convert.ToString(CurrentViewContext.SelectedTenantId);
                        }
                        #region UAT-1538
                        if (args.ContainsKey("DocumentViewType"))
                        {
                            DocumentViewType = Convert.ToString(args["DocumentViewType"]);
                        }
                        if (args.ContainsKey("DocumentId"))
                        {
                            ApplicantDocumentId = Convert.ToInt32(args["DocumentId"]);
                        }
                        //if (args.ContainsKey("DocumentPath"))
                        //{
                        //    DocumentPath = Convert.ToString(args["DocumentPath"]);
                        //}
                        #endregion
                    }
                    if (!this.IsPostBack)
                    {
                        if (DocumentViewType == UtilityFeatures.Unified_Document.GetStringValue())
                        {
                            RenderUnifiedPdfDocument();
                        }
                        //else if (DocumentViewType == UtilityFeatures.View_Document.GetStringValue())
                        //{
                        //    RenderViewDocPdfDocument();
                        //}
                        else
                        {
                            RenderSinglePdfDocument();
                        }
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

        //#region UAT:4132
        ////UAT:4132
        //private string DeleteExistsFileAndReplaceAfterFlattening(string pdfDocPath, byte[] FileBytes)
        //{
        //    try
        //    {
        //        BaseWebPage.LogOrderPDFViewer("Class Name: " + MethodBase.GetCurrentMethod().DeclaringType.Name + ":-  Function Name: " + System.Reflection.MethodBase.GetCurrentMethod().Name + " :- Start Call Method");
        //        string NameOfData = pdfDocPath;
        //        string returnFilePath = string.Empty;
        //        string FileName = Path.GetFileName(pdfDocPath);
        //        string filePath = string.Empty;
        //        bool aWSUseS3 = false;
        //        StringBuilder corruptedFileMessage = new StringBuilder();
        //        string tempFilePath = WebConfigurationManager.AppSettings["TemporaryFileLocation"];
        //        filePath = WebConfigurationManager.AppSettings[AppConsts.APPLICANT_FILE_LOCATION];

        //        if (!WebConfigurationManager.AppSettings["AWSUseS3"].IsNullOrEmpty())
        //        {
        //            aWSUseS3 = Convert.ToBoolean(WebConfigurationManager.AppSettings["AWSUseS3"]);
        //        }
        //        if (tempFilePath.IsNullOrEmpty())
        //        {
        //            return "";
        //        }
        //        if (!tempFilePath.EndsWith(@"\"))
        //        {
        //            tempFilePath += @"\";
        //        }
        //        tempFilePath += "Tenant(" + CurrentViewContext.SelectedTenantId.ToString() + @")\";

        //        if (!Directory.Exists(tempFilePath))
        //        {
        //            Directory.CreateDirectory(tempFilePath);
        //        }

        //        //Check whether use AWS S3, true if need to use
        //        if (aWSUseS3 == false)
        //        {
        //            if (filePath.IsNullOrEmpty())
        //            {
        //                return "";
        //            }
        //            if (!filePath.EndsWith("\\"))
        //            {
        //                filePath += "\\";
        //            }

        //        }
        //        string newTempFilePath = Path.Combine(tempFilePath, FileName);

        //        if (File.Exists(newTempFilePath))
        //        {
        //            try
        //            {
        //                File.Delete(newTempFilePath);
        //            }
        //            catch (Exception)
        //            {

        //                throw;
        //            }
        //        }


        //        //File Create after Flattening using bytes 
        //        if (!File.Exists(newTempFilePath))
        //        {
        //            File.WriteAllBytes(newTempFilePath, FileBytes);
        //        }
        //        if (aWSUseS3 == false)
        //        {
        //            //Move file to other location
        //            string destFilePath = Path.Combine(filePath, FileName);
        //            if (File.Exists(destFilePath))
        //            {
        //                try
        //                {
        //                    File.Delete(destFilePath);
        //                }
        //                catch (Exception)
        //                {

        //                    throw;
        //                }
        //            }
        //            File.Copy(newTempFilePath, destFilePath);
        //            returnFilePath = destFilePath;

        //        }
        //        else
        //        {
        //            if (filePath.IsNullOrEmpty())
        //            {
        //                return "";
        //            }
        //            if (!filePath.EndsWith("//"))
        //            {
        //                filePath += "//";
        //            }
        //            //AWS code to save document to S3 location
        //            AmazonS3Documents objAmazonS3 = new AmazonS3Documents();
        //            string destFolder = filePath + "Tenant(" + CurrentViewContext.SelectedTenantId.ToString() + @")/";
        //            BaseWebPage.LogOrderPDFViewer("Class Name: " + MethodBase.GetCurrentMethod().DeclaringType.Name + ":-  Function Name: " + System.Reflection.MethodBase.GetCurrentMethod().Name + " :- Call Method" + "Before the Call of Actual Deleted File");

        //            if (objAmazonS3.DeleteDocument(FileName))
        //            {
        //                BaseWebPage.LogOrderPDFViewer("Class Name: " + MethodBase.GetCurrentMethod().DeclaringType.Name + ":-  Function Name: " + System.Reflection.MethodBase.GetCurrentMethod().Name + " :- Call Method" + "Call after the File Detele and before the call of Saving Flattening");
        //                returnFilePath = objAmazonS3.SaveDocument(newTempFilePath, FileName, destFolder);
                       
                       
        //            }

        //            if (returnFilePath.IsNullOrEmpty())
        //            {
        //                corruptedFileMessage.Append("Your file " + FileName + " is not uploaded. \\n");
        //                BaseWebPage.LogOrderPDFViewer("Class Name: " + MethodBase.GetCurrentMethod().DeclaringType.Name + ":-  Function Name: " + System.Reflection.MethodBase.GetCurrentMethod().Name + " :- Call Method" + "Call after the File save in after Flattening data");

        //            }
        //        }
        //        try
        //        {
        //            if (!string.IsNullOrEmpty(newTempFilePath))
        //            {
        //                File.Delete(newTempFilePath);
        //            }
        //        }
        //        catch (Exception ex) { throw ex; }
        //        BaseWebPage.LogOrderPDFViewer("Class Name: " + MethodBase.GetCurrentMethod().DeclaringType.Name + ":-  Function Name: " + System.Reflection.MethodBase.GetCurrentMethod().Name + " :- END Call Method");
        //        return returnFilePath;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }


        //}

        ////UAT:4132
        //private  byte[] ConvertFormFlattening(byte[] fileBytes)
        //{
        //    BaseWebPage.LogOrderPDFViewer("Convert Pdf into Flattening on UnifiedPdfDocViewer");
        //    byte[] tempFileByte = null;
        //    using (PdfReader compressionReader = new PdfReader(fileBytes))
        //    {
        //        using (MemoryStream compressionsMS = new MemoryStream())
        //        {
        //            using (PdfStamper compressionStamper = new PdfStamper(compressionReader, compressionsMS))
        //            {
        //                compressionStamper.FormFlattening = true;
        //                compressionStamper.Close();
        //            }
        //            tempFileByte = compressionsMS.ToArray();

        //        }
        //        return tempFileByte;
        //    }
        //} 
        //#endregion

        private void RenderUnifiedPdfDocument()
        {
            try
            {

                BaseWebPage.LogOrderPDFViewer("Class Name: " + MethodBase.GetCurrentMethod().DeclaringType.Name + ":-  Function Name: " + System.Reflection.MethodBase.GetCurrentMethod().Name + " :- Call Method" + "RenderSinglePdfDocument");
                Presenter.GetUnifiedDocument();
                if (CurrentViewContext.UnifiedPdfDocument.IsNull())
                {
                    //Merging Not Done
                    SetPdfControlMessage("Unified PDF document does not exist in database for the applicant.");
                }
                else if (CurrentViewContext.UnifiedPdfDocument.IsNotNull()
                    && CurrentViewContext.UnifiedPdfDocument.lkpDocumentStatu.DMS_Code == DocumentStatus.MERGING_IN_PROGRESS.GetStringValue())
                {
                    btnReloadPdfVwr.Text = "Load Document";
                    //MERGING_IN_PROGRESS
                    SetPdfControlMessage("Document merging is under processing. Please click 'Load Document' button to try after some time.");
                }
                else
                {
                    byte[] pdfData = Presenter.GetPDFByteData();
                    //Load PDF byte array into RAD PDF
                    if (pdfData.IsNotNull())
                    {
                        // UAT: 4132 Get the details of unifiedPDF document is Fallten or Not..
                        ////if (CurrentViewContext.UnifiedPdfDocument.UPD_IsPdfDocumnetFormFlattening.IsNull() ||
                        ////   CurrentViewContext.UnifiedPdfDocument.UPD_IsPdfDocumnetFormFlattening == false)
                        ////{
                        ////    pdfData = ConvertFormFlattening(pdfData);
                        ////    if (!DeleteExistsFileAndReplaceAfterFlattening(CurrentViewContext.UnifiedPdfDocument.UPD_PdfDocPath, pdfData).IsNullOrEmpty())
                        ////    {
                        //        Presenter.UpdateUnifiedDocumentIsFormFlattening(GetCurrentLoggedUserId().OrganizationUserID);
                        //    }
                        //}

                        if (SelectedCatUnifiedStartPageID > AppConsts.NONE && SelectedCatUnifiedStartPageID.IsNotNull())
                        {
                            PdfWebControl1.ViewerPageDefault = SelectedCatUnifiedStartPageID.Value;
                        }

                        PdfWebControl1.CreateDocument("PDF File", pdfData, PdfDocumentSettings.FlattenForm | PdfDocumentSettings.DisableAddShapes | PdfDocumentSettings.DisableAddWhiteoutShapes |
                           PdfDocumentSettings.DisableAddLineShapes | PdfDocumentSettings.DisableFormFields | PdfDocumentSettings.DisableAlterBookmarks);
                    }
                    else
                    {
                        //btnReloadPdfDoc.Text = "Reload";
                        SetPdfControlMessage("Unable to find the unified PDF document file.");
                    }
                    hdnUnifiedDoc_value.Value = Convert.ToString(CurrentViewContext.UnifiedPdfDocument.UPD_ID);
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

        private void SetPdfControlMessage(string messageToShow)
        {
           
            dvPdfDocuViewer.Visible = false;
            btnRotatePdfPage.Visible = false;
            lblPdfMessage.Text = messageToShow;
            lblPdfMessage.CssClass = "info";
            dvMsgBox.Attributes["class"] = "msgbox shw_dv";
        }

        protected void btnReloadPdfVwr_Click(object sender, EventArgs e)
        {
            if (DocumentViewType == UtilityFeatures.Unified_Document.GetStringValue())
            {
                RenderUnifiedPdfDocument();
            }
            //else if (DocumentViewType == UtilityFeatures.View_Document.GetStringValue())
            //{
            //    RenderViewDocPdfDocument();
            //}
            else
            {
                RenderSinglePdfDocument();
            }
        }

        #region UAT-1538
        private void RenderSinglePdfDocument()
        {
            try
            {
                BaseWebPage.LogOrderPDFViewer("Class Name: " + MethodBase.GetCurrentMethod().DeclaringType.Name + ":-  Function Name: " + System.Reflection.MethodBase.GetCurrentMethod().Name + " :- Call Method"+ "RenderSinglePdfDocument");

                Presenter.GetApplicantDocument();
                if (CurrentViewContext.ApplicantDocument.IsNotNull())
                {
                    byte[] pdfData = Presenter.GetPDFByteDataForSingleDocument();
                    //Load PDF byte array into RAD PDF
                    if (pdfData.IsNotNull())
                    {
                   // UAT: 4132 Check Pdf Flattening
                        //if (CurrentViewContext.ApplicantDocument.IsPdfDocumnetFormFlattening.IsNull() ||
                        //CurrentViewContext.ApplicantDocument.IsPdfDocumnetFormFlattening == false)
                        //{
                        ////UAT: 4132 Convert normal Bytes to Flattening
                        //    pdfData = ConvertFormFlattening(pdfData);
                        //    if (!DeleteExistsFileAndReplaceAfterFlattening(CurrentViewContext.ApplicantDocument.PdfDocPath, pdfData).IsNullOrEmpty())
                        //    {
                              // Presenter.UpdateApplicantIsFormFlattening(GetCurrentLoggedUserId().OrganizationUserID);
                        //    }
                        //}
                        PdfWebControl1.CreateDocument("PDF File", pdfData, PdfDocumentSettings.DisableAlterBookmarks);

                    }
                    else
                    {
                        SetPdfControlMessage("Unable to find the PDF document file.");
                    }
                    hdnUnifiedDoc_value.Value = Convert.ToString(CurrentViewContext.ApplicantDocument.ApplicantDocumentID);
                }
                else
                {
                    SetPdfControlMessage("Unable to find the PDF document file.");
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
        #endregion

        //private void RenderViewDocPdfDocument()
        //{
        //    try
        //    {

        //        byte[] pdfData = Presenter.GetPDFByteDataForViewDocSingleDocument();
        //        //Load PDF byte array into RAD PDF
        //        if (pdfData.IsNotNull())
        //        {
        //            PdfWebControl1.CreateDocument("PDF File", pdfData, PdfDocumentSettings.DisableAlterBookmarks);
        //        }
        //        else
        //        {
        //            SetPdfControlMessage("Unable to find the PDF document file.");
        //        }
        //    }
        //    catch (SysXException ex)
        //    {
        //        SetPdfControlMessage(ex.Message);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        SetPdfControlMessage(ex.Message);
        //    }
        //}
    }
}