using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreWeb.ComplianceAdministration.Views;
using Entity.ClientEntity;
using INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using iTextSharp.text.pdf;
using Newtonsoft.Json;

namespace CoreWeb.ComplianceAdministration.Views
{
    public partial class ViewDocumentPopup : BaseWebPage, IViewDocumentView
    {

        #region Variables

        #region Private Variables

        private ViewDocumentPopupPresenter _presenter = new ViewDocumentPopupPresenter();

        #endregion

        #region Public Variables

        #endregion

        #endregion

        #region Properties

        #region Private Properties

        Int32 IViewDocumentView.ClientSysDocId
        {
            get;
            set;
        }

        Int32 IViewDocumentView.TenantId
        {
            get
            {
                if (!ViewState["TenantId"].IsNull())
                {
                    return Convert.ToInt32(ViewState["TenantId"]);
                }

                return AppConsts.NONE;
            }
            set
            {
                ViewState["TenantId"] = value;
            }
        }

        Int32 IViewDocumentView.ApplicantDocId
        {
            get;
            set;
        }


        ViewDocumentDetailsContract IViewDocumentView.ViewDocContract
        {
            get
            {
                if (!ViewState["ViewDocContract"].IsNull())
                {
                    return (ViewState["ViewDocContract"]) as ViewDocumentDetailsContract;
                }

                return new ViewDocumentDetailsContract();
            }
            set
            {
                ViewState["ViewDocContract"] = value;
            }
        }

        List<Entity.ClientEntity.DocumentFieldMapping> IViewDocumentView.lstDocumentFieldMapping
        {
            get
            {
                if (!ViewState["lstDocumentFieldMapping"].IsNull())
                {
                    return (ViewState["lstDocumentFieldMapping"]) as List<DocumentFieldMapping>;
                }

                return new List<DocumentFieldMapping>();
            }
            set
            {
                ViewState["lstDocumentFieldMapping"] = value;
            }
        }

        Int32 IViewDocumentView.OrganizationUserID { get; set; }

        Entity.OrganizationUser IViewDocumentView.OrganizationUserData { get; set; }

        AddressContract IViewDocumentView.Addresses { get; set; }

        ClientSystemDocument IViewDocumentView.ClientSysDocument { get; set; }

        Entity.ClientEntity.ApplicantDocument IViewDocumentView.ApplicantDocumentData { get; set; }

        Int16 IViewDocumentView.DataEntryDocCompleteStatusId { get; set; }

        /// <summary>
        /// UAT 1261: WB: As an ADB admin, I should be able to "login" as any student to see what they see.
        /// </summary>
        Int32 IViewDocumentView.OrgUsrID
        {
            get
            {
                if (!System.Web.HttpContext.Current.Session["AppViewAdminOrgUsrID"].IsNullOrEmpty())
                {
                    return Convert.ToInt32(System.Web.HttpContext.Current.Session["AppViewAdminOrgUsrID"]);
                }
                else
                {
                    return CurrentViewContext.OrganizationUserID;
                }
            }
        }
        Boolean ShouldDeteleTempFile = false;
        List<ApplicantDocument> IViewDocumentView.ToSaveApplicantUploadedDocuments { get; set; }

        String IViewDocumentView.ErrorMessage { get; set; }

        #region UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
        private String NoMiddleNameText
        {
            get
            {
                String noMiddleNameText = WebConfigurationManager.AppSettings[AppConsts.NO_MIDDLE_NAME_TEXT_KEY];
                if (noMiddleNameText.IsNull())
                {
                    noMiddleNameText = String.Empty;
                }
                return noMiddleNameText;
            }
        }
        #endregion
        #endregion

        #region Public Properties

        public ViewDocumentPopupPresenter Presenter
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

        public IViewDocumentView CurrentViewContext
        {
            get { return this; }
        }

        #endregion

        #endregion

        #region Events

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Request.QueryString["ClientSysDocID"].IsNullOrEmpty())
                {
                    CurrentViewContext.ClientSysDocId = Convert.ToInt32(Request.QueryString["ClientSysDocID"]);
                }
                if (!Request.QueryString["OrganizationUserID"].IsNullOrEmpty())
                {
                    CurrentViewContext.OrganizationUserID = Convert.ToInt32(Request.QueryString["OrganizationUserID"]);
                }
                if (!Request.QueryString["ApplicantDocID"].IsNullOrEmpty())
                {
                    CurrentViewContext.ApplicantDocId = Convert.ToInt32(Request.QueryString["ApplicantDocID"]);
                }
                else
                {
                    //todo:set temp path
                }
                if (!IsPostBack)
                {
                    GetOrganizationUserDetails();
                    GetDocumentToLoad();
                    LoadCLientSystemDocument();
                }

            }
            catch (SysXException ex)
            {
                LogError(ex.Message, ex);
            }
            catch (System.Exception ex)
            {
                LogError(ex.Message, ex);
            }
        }



        #endregion

        #region Button Events

        protected void btnSavePdf_Click(object sender, EventArgs e)
        {
            try
            {
                AddSignatureOnPdf();
                ViewDocumentDetailsContract docContract = Session["ViewDocumentDetailsContract"] as ViewDocumentDetailsContract;
                //Add document to Applicant Document.
                if (docContract.IsNotNull())
                {
                    UploadViewDocument(docContract);
                }

            }
            catch (SysXException ex)
            {
                LogError(ex.Message, ex);
            }
            catch (System.Exception ex)
            {
                LogError(ex.Message, ex);
            }

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                DeleteTempFile(ViewState["TempFilePath"].ToString());
            }
            catch (Exception ex)
            {
                base.LogError(ex);
            }

        }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        public void GetDocumentToLoad()
        {
            Presenter.GetDocumentData();
            ViewState["DocumentUrl"] = CurrentViewContext.ViewDocContract.DocumentPath;
            hdnTenantId.Value = Convert.ToString(CurrentViewContext.TenantId);
            hfIsSignatureRequired.Value = CurrentViewContext.ViewDocContract.IsSignatureRequired.ToString();
            hdnFileName.Value = CurrentViewContext.ViewDocContract.DocumentName;
        }

        /// <summary>
        /// 
        /// </summary>e
        private void LoadCLientSystemDocument()
        {
            if (CurrentViewContext.ViewDocContract.IsApplicantDoc)
            {
                HideWndControls();
            }
            else
            {
                SetFiedsValuesAndPutSignPad();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        private void GetOrganizationUserDetails()
        {
            Presenter.GetOrganizationUserDetails();
            if (!CurrentViewContext.OrganizationUserData.IsNullOrEmpty())
            {
                CurrentViewContext.TenantId = CurrentViewContext.OrganizationUserData.Organization.TenantID.Value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pdfDocumentDataToBeFilledIn"></param>
        /// <param name="imageToAddToDocument"></param>
        /// <param name="txtFullName"></param>
        /// <param name="txtCurrentDate"></param>
        /// <returns></returns>
        private byte[] FillSignatureInDocument(byte[] pdfDocumentDataToBeFilledIn, byte[] imageToAddToDocument)
        {

            byte[] signedDocument = null;

            try
            {
                PdfReader reader = new PdfReader(pdfDocumentDataToBeFilledIn);
                MemoryStream ms = new MemoryStream();
                PdfStamper stamper = new PdfStamper(reader, ms);
                AcroFields.FieldPosition signatureImagePosition = null;

                //Fill-in the form values
                AcroFields af = stamper.AcroFields;

                stamper.FormFlattening = true;
                float left = 0;
                float right = 0;
                float top = 0;
                float heigth = 0;

                try { signatureImagePosition = af.GetFieldPositions("Signature")[0]; }
                catch { }

                if (signatureImagePosition != null && imageToAddToDocument != null)
                {
                    left = signatureImagePosition.position.Left;
                    right = signatureImagePosition.position.Right;
                    top = signatureImagePosition.position.Top;
                    heigth = signatureImagePosition.position.Height;

                    iTextSharp.text.Image signatureImage = iTextSharp.text.Image.GetInstance(imageToAddToDocument);
                    //PdfContentByte contentByte = stamper.GetOverContent(1);
                    PdfContentByte contentByte = stamper.GetOverContent(signatureImagePosition.page); // uat - 856 : WB: Signature on disclosure is not placing properly when there are multiple pages in the document
                    float currentImageHeigth = 0;
                    currentImageHeigth = signatureImage.Height;
                    float ratio = 0;
                    ratio = heigth / currentImageHeigth;
                    float width = signatureImage.Width * ratio;
                    signatureImage.ScaleAbsoluteHeight(heigth);
                    signatureImage.ScaleAbsoluteWidth(width);
                    signatureImage.SetAbsolutePosition(left, top - signatureImage.ScaledHeight);
                    contentByte.AddImage(signatureImage);
                }
                stamper.Close();
                signedDocument = ms.ToArray();
                ms.Close();

                //Recompress final document to further shrink.
                signedDocument = CompressPDFDocument(signedDocument);
            }
            catch (SysXException ex)
            {
                LogError(ex.Message, ex);
            }
            catch (System.Exception ex)
            {
                LogError(ex.Message, ex);
            }
            return signedDocument;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="signedDocument"></param>
        /// <returns></returns>
        private byte[] CompressPDFDocument(byte[] signedDocument)
        {
            try
            {
                PdfReader compressionReader = new PdfReader(signedDocument);
                MemoryStream compressionsMS = new MemoryStream();
                PdfStamper compressionStamper = new PdfStamper(compressionReader, compressionsMS);
                compressionStamper.FormFlattening = true;
                compressionStamper.SetFullCompression();
                compressionStamper.Close();
                signedDocument = compressionsMS.ToArray();
                compressionsMS.Close();
                compressionReader.Close();
            }
            catch (Exception ex)
            {
                base.LogError(ex);
            }

            return signedDocument;
        }

        /// <summary>
        /// This method read the signature and fill into the pdf
        /// </summary>
        /// <returns></returns>
        private bool AddSignatureOnPdf()
        {
            PenColor = Color.Black;
            Background = Color.White;
            Height = 150;
            Width = 648;
            PenWidth = 2;
            FontSize = 24;
            bool status = false;
            FileStream _fileStream = null;
            // SignatureToImage sigGenerator = new SignatureToImage();   
            try
            {
                byte[] buffer = null;
                //byte[] signedDocumentBuffer = null;
                byte[] mergedSignedDocumentBuffer = null;
                byte[] bufferSignature = null;
                long _totalBytes = 0;

                // string orgName = SessionWrapper.ClientSiteController.ClientConfiguration.InstitutionName;
                using (_fileStream = new FileStream(ViewState["TempFilePath"].ToString(), System.IO.FileMode.Open, System.IO.FileAccess.Read))
                {
                    _totalBytes = new System.IO.FileInfo(ViewState["TempFilePath"].ToString()).Length;
                    BinaryReader _binaryReader = new BinaryReader(_fileStream);
                    //// read entire file into buffer
                    buffer = _binaryReader.ReadBytes((Int32)_totalBytes);
                }

                PdfReader reader = new PdfReader(buffer);
                MemoryStream ms = new MemoryStream();
                PdfStamper stamper = new PdfStamper(reader, ms);

                byte[] signedDocument = null;
                AcroFields.FieldPosition signatureImagePosition = null;
                stamper.FormFlattening = true;
                float left = 0;
                float right = 0;
                float top = 0;
                float heigth = 0;
                string jsonStr = hdnOutput.Value;
                // iTextSharpPDFWrapper its = new iTextSharpPDFWrapper();
                if (hfIsSignatureRequired.Value == "True" && hdnOutput.Value.IsNullOrEmpty())
                {
                    lblErrorSig.Visible = true;
                    lblErrorSig.Text = "Please Sign the document.";
                    return false;
                }


                //Fill-in the form values
                AcroFields acroF = stamper.AcroFields;


                if (!CurrentViewContext.lstDocumentFieldMapping.IsNullOrEmpty())
                {
                    foreach (DocumentFieldMapping item in CurrentViewContext.lstDocumentFieldMapping)
                    {
                        if (!item.lkpDocumentFieldType_.IsNullOrEmpty())
                        {
                            switch (item.lkpDocumentFieldType_.DFT_Code)
                            {
                                case "AAAA":
                                    //create image from json
                                    System.Drawing.Bitmap signatureImageBitMap = SigJsonToImage(jsonStr);
                                    // Save out to memory and then to a file.
                                    MemoryStream mm = new MemoryStream();
                                    signatureImageBitMap.Save(mm, System.Drawing.Imaging.ImageFormat.Png);
                                    bufferSignature = mm.GetBuffer();

                                    if (bufferSignature.IsNotNull() && bufferSignature.Length < Convert.ToInt32(WebConfigurationManager.AppSettings["MinApplicantSignLength"]))
                                    {
                                        //Signature disqualified.
                                        lblErrorSig.Visible = true;
                                        lblErrorSig.Text = "Provided text does not qualify as valid Signature. Please provide valid Signature.";
                                        return false;
                                    }

                                    try { signatureImagePosition = acroF.GetFieldPositions(item.DFM_FieldName)[0]; }
                                    catch { }

                                    if (signatureImagePosition != null && bufferSignature != null)
                                    {
                                        left = signatureImagePosition.position.Left;
                                        right = signatureImagePosition.position.Right;
                                        top = signatureImagePosition.position.Top;
                                        heigth = signatureImagePosition.position.Height;

                                        iTextSharp.text.Image signatureImage = iTextSharp.text.Image.GetInstance(bufferSignature);
                                        //PdfContentByte contentByte = stamper.GetOverContent(1);
                                        PdfContentByte contentByte = stamper.GetOverContent(signatureImagePosition.page); // uat - 856 : WB: Signature on disclosure is not placing properly when there are multiple pages in the document
                                        float currentImageHeigth = 0;
                                        currentImageHeigth = signatureImage.Height;
                                        float ratio = 0;
                                        ratio = heigth / currentImageHeigth;
                                        float width = signatureImage.Width * ratio;
                                        signatureImage.ScaleAbsoluteHeight(heigth);
                                        signatureImage.ScaleAbsoluteWidth(width);
                                        signatureImage.SetAbsolutePosition(left, top - signatureImage.ScaledHeight);
                                        contentByte.AddImage(signatureImage);
                                    }

                                    break;

                                //to do more fields
                                default:
                                    break;
                            }


                        }
                    }
                    stamper.Close();
                    signedDocument = ms.ToArray();
                    ms.Close();

                    //Recompress final document to further shrink.
                    mergedSignedDocumentBuffer = CompressPDFDocument(signedDocument);
                }

                ms.Dispose();
                if (mergedSignedDocumentBuffer != null)
                {
                    Guid guid = Guid.NewGuid();
                    var TemporaryDirectoryPath = GetTempPathToSaveFile();
                    var fileName = String.Format("{0}ApplicantDocument.pdf", guid);
                    var TemporaryFilePath = String.Format(TemporaryDirectoryPath + fileName);
                    System.IO.File.WriteAllBytes(TemporaryFilePath, mergedSignedDocumentBuffer);
                    hdnIsDocViewed.Value = "True";
                    iframePdfDocViewer.Src = String.Empty;
                    iframePdfDocViewer.Src = String.Format("/ComplianceOperations/UserControl/DocumentViewer.aspx?DocumentType={0}&FileName={1}&tenantId={2}", "ClientSystemDocument", fileName, hdnTenantId.Value);
                    hfTemporaryApplicantDocPath.Value = TemporaryFilePath;
                    ViewDocumentDetailsContract docContract = CurrentViewContext.ViewDocContract;
                    if (!docContract.IsNullOrEmpty())
                    {
                        docContract.DocumentPath = TemporaryFilePath;
                    }
                    Session["ViewDocumentDetailsContract"] = docContract;
                    btnSavePdf.Visible = false;
                    signatureDiv.Visible = false;
                    ViewState["TempFilePath"] = TemporaryFilePath;
                    //ShouldDeteleTempFile = true;
                    btnCancel.Visible = true;
                }

            }
            catch (Exception ex)
            {
                base.LogError(ex);
            }
            finally
            {
                try { _fileStream.Close(); }
                catch (Exception) { }
            }
            return status;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private String GetTempPathToSaveFile()
        {
            String filePath = String.Empty;
            String tempFilePath = WebConfigurationManager.AppSettings["TemporaryFileLocation"];
            //filePath = WebConfigurationManager.AppSettings[AppConsts.APPLICANT_FILE_LOCATION];
            if (tempFilePath.IsNullOrEmpty())
            {
                base.LogError("Please provide path for TemporaryFileLocation in config.", null);
                return null;
            }
            if (!tempFilePath.EndsWith(@"\"))
            {
                tempFilePath += @"\";
            }
            tempFilePath += "Tenant(" + hdnTenantId.Value + @")\";

            if (!Directory.Exists(tempFilePath))
                Directory.CreateDirectory(tempFilePath);
            Guid guid = Guid.NewGuid();
            return tempFilePath;
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetFiedsValuesAndPutSignPad()
        {
            try
            {
                byte[] updatedDocument = null;
                string TemporaryDirectoryPath = string.Empty;
                string TemporaryFilePath = string.Empty;
                string fileName = String.Empty;
                //UAT-CRASH[DATED:21/08/2015]
                //byte[] pdfData = System.IO.File.ReadAllBytes(ViewState["DocumentUrl"].ToString());
                byte[] pdfData = CommonFileManager.RetrieveDocument(Convert.ToString(ViewState["DocumentUrl"]), FileType.ApplicantFileLocation.GetStringValue());
                if (pdfData.IsNotNull())
                {
                    PdfReader reader = new PdfReader(pdfData);
                    MemoryStream ms = new MemoryStream();
                    PdfStamper stamper = new PdfStamper(reader, ms);
                    AcroFields af = stamper.AcroFields;
                    List<String> AlreadyFilledFields = new List<string>();        // Get already mapped fields for disabling       
                    //Get DocumentFieldMappings
                    //Presenter.GetDocuemntFieldMappingForDoucumentID();
                    if (!CurrentViewContext.lstDocumentFieldMapping.IsNullOrEmpty())
                    {
                        foreach (DocumentFieldMapping item in CurrentViewContext.lstDocumentFieldMapping)
                        {
                            if (!item.lkpDocumentFieldType_.IsNullOrEmpty())
                            {
                                if (af.GetField(item.DFM_FieldName).IsNotNull())
                                {
                                    if (item.DFM_DocumentFieldTypeID.IsNotNull())
                                    {
                                        switch (item.lkpDocumentFieldType_.DFT_Code)
                                        {
                                            case "AAAA":
                                                signatureDiv.Visible = true;
                                                af.SetFieldProperty(item.DFM_FieldName,
                                                             "setfflags",
                                                              PdfFormField.FF_READ_ONLY,
                                                              null);
                                                break;
                                            case "AAAB":
                                                af.SetField(item.DFM_FieldName, DateTime.Now.ToShortDateString());
                                                AlreadyFilledFields.Add(item.DFM_FieldName);
                                                break;
                                            case "AAAC":
                                                af.SetField(item.DFM_FieldName, CurrentViewContext.OrganizationUserData.FirstName);
                                                AlreadyFilledFields.Add(item.DFM_FieldName);
                                                break;
                                            case "AAAD":
                                                af.SetField(item.DFM_FieldName, CurrentViewContext.OrganizationUserData.MiddleName);
                                                AlreadyFilledFields.Add(item.DFM_FieldName);
                                                break;
                                            case "AAAE":
                                                af.SetField(item.DFM_FieldName, CurrentViewContext.OrganizationUserData.LastName);
                                                AlreadyFilledFields.Add(item.DFM_FieldName);
                                                //mappedFieldValue= dcAlreadyFilledFields.ContainsKey(item.DFM_FieldName) == true ? dcAlreadyFilledFields.GetValue(item.DFM_FieldName) : CurrentViewContext.OrganizationUserData.LastName;
                                                break;
                                            case "AAAF":
                                                af.SetField(item.DFM_FieldName, Presenter.GetApplicantSSN());//to change-get decrypted SSN
                                                AlreadyFilledFields.Add(item.DFM_FieldName);
                                                break;
                                            case "AAAG":
                                                af.SetField(item.DFM_FieldName, CurrentViewContext.OrganizationUserData.DOB.Value.ToShortDateString());
                                                AlreadyFilledFields.Add(item.DFM_FieldName);
                                                break;
                                            case "AAAH":
                                                af.SetField(item.DFM_FieldName, CurrentViewContext.OrganizationUserData.PhoneNumber);
                                                AlreadyFilledFields.Add(item.DFM_FieldName);
                                                break;
                                            case "AAAI":
                                                af.SetField(item.DFM_FieldName, CurrentViewContext.OrganizationUserData.PrimaryEmailAddress);
                                                AlreadyFilledFields.Add(item.DFM_FieldName);
                                                break;
                                            case "AAAJ":
                                                af.SetField(item.DFM_FieldName, CurrentViewContext.OrganizationUserData.Gender.HasValue
                                                                ? CurrentViewContext.OrganizationUserData.lkpGender.GenderName : "N/A");
                                                AlreadyFilledFields.Add(item.DFM_FieldName);
                                                break;
                                            case "AAAK":
                                                StringBuilder sbAliasName = new StringBuilder();
                                                String delimittedAliasName = string.Empty;
                                                if (!CurrentViewContext.OrganizationUserData.IsNullOrEmpty())
                                                {
                                                    foreach (Entity.PersonAlia alias in CurrentViewContext.OrganizationUserData.PersonAlias.Where(x => !x.PA_IsDeleted))
                                                    {
                                                        //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                                                        String middleName = alias.PA_MiddleName.IsNullOrEmpty() ? NoMiddleNameText : alias.PA_MiddleName;
                                                        sbAliasName.Append(Convert.ToString(alias.PA_FirstName) + " " + middleName + " " + Convert.ToString(alias.PA_LastName));
                                                        sbAliasName.Append(", ");
                                                    }
                                                    if (sbAliasName.Length >= AppConsts.TWO)
                                                    {
                                                        delimittedAliasName = Convert.ToString(sbAliasName.ToString(0, sbAliasName.Length - AppConsts.TWO));
                                                    }
                                                }
                                                af.SetField(item.DFM_FieldName, delimittedAliasName);
                                                AlreadyFilledFields.Add(item.DFM_FieldName);
                                                break;
                                            case "AAAL":
                                                af.SetField(item.DFM_FieldName, CurrentViewContext.Addresses.Address1);
                                                AlreadyFilledFields.Add(item.DFM_FieldName);
                                                break;
                                            case "AAAM":
                                                af.SetField(item.DFM_FieldName, CurrentViewContext.Addresses.Address2);
                                                AlreadyFilledFields.Add(item.DFM_FieldName);
                                                break;
                                            case "AAAN":
                                                af.SetField(item.DFM_FieldName, CurrentViewContext.Addresses.CityName);
                                                AlreadyFilledFields.Add(item.DFM_FieldName);
                                                break;
                                            case "AAAO":
                                                af.SetField(item.DFM_FieldName, CurrentViewContext.Addresses.StateName);
                                                AlreadyFilledFields.Add(item.DFM_FieldName);
                                                break;
                                            case "AAAP":
                                                af.SetField(item.DFM_FieldName, CurrentViewContext.Addresses.Zipcode);
                                                AlreadyFilledFields.Add(item.DFM_FieldName);
                                                break;
                                            case "AAAQ":
                                                af.SetField(item.DFM_FieldName, CurrentViewContext.Addresses.CountyName);
                                                AlreadyFilledFields.Add(item.DFM_FieldName);
                                                break;
                                            case "AAAR":
                                                af.SetField(item.DFM_FieldName, CurrentViewContext.Addresses.Country);
                                                AlreadyFilledFields.Add(item.DFM_FieldName);
                                                break;
                                            case "AAAS":
                                                af.SetField(item.DFM_FieldName, CurrentViewContext.OrganizationUserData.Organization.Tenant.TenantName);
                                                AlreadyFilledFields.Add(item.DFM_FieldName);
                                                break;
                                            case "AAAT":
                                                af.SetField(item.DFM_FieldName, (CurrentViewContext.OrganizationUserData.FirstName + " " + CurrentViewContext.OrganizationUserData.LastName));
                                                AlreadyFilledFields.Add(item.DFM_FieldName);
                                                break;
                                            //to do more fields
                                            default:
                                                break;
                                        }
                                        if (AlreadyFilledFields.Contains(item.DFM_FieldName))
                                        {
                                            af.SetFieldProperty(item.DFM_FieldName,
                                                             "setfflags",
                                                              PdfFormField.FF_READ_ONLY,
                                                              null);
                                        }
                                    }
                                }

                            }
                        }
                    }
                    if (!CurrentViewContext.ViewDocContract.IsSignatureRequired)
                    {
                        signatureDiv.Visible = false;
                    }
                    stamper.Close();
                    updatedDocument = ms.ToArray();
                    ms.Dispose();
                    //Save To temp Location

                    if (updatedDocument.IsNotNull())
                    {
                        Guid guid = Guid.NewGuid();
                        TemporaryDirectoryPath = GetTempPathToSaveFile();
                        fileName = String.Format("{0}ApplicantDocument.pdf", guid);
                        TemporaryFilePath = String.Format(TemporaryDirectoryPath + fileName);
                        //Guid guid = Guid.NewGuid();
                        //GetDocumentToLoad();
                        //fileName = CurrentViewContext.ViewDocContract.DocumentName;
                        //TemporaryDirectoryPath = GetTempPathToSaveFile();
                        //TemporaryDirectoryPath = Convert.ToString(ViewState["DocumentUrl"]);
                        //fileName = String.Format("{0}ApplicantDocument.pdf", guid);
                        //TemporaryFilePath = TemporaryDirectoryPath;
                        System.IO.File.WriteAllBytes(TemporaryFilePath, updatedDocument);
                        iframePdfDocViewer.Src = String.Format("/ComplianceOperations/UserControl/DocumentViewer.aspx?DocumentType={0}&FileName={1}&tenantId={2}&DocumentPath={3}", "ClientSystemDocument", fileName, hdnTenantId.Value, TemporaryFilePath);
                    }
                    //hfTemporaryApplicantDocPath.Value = TemporaryFilePath;
                }
                ViewState["TempFilePath"] = TemporaryFilePath;
            }
            catch (SysXException ex)
            {
                LogError(ex.Message, ex);
            }
            catch (System.Exception ex)
            {
                LogError(ex.Message, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void HideWndControls()
        {
            iframePdfDocViewer.Src = String.Format("/ComplianceOperations/UserControl/DocumentViewer.aspx?DocumentType={0}&documentId={1}&IsApplicantDocument={2}&tenantId={3}", "ClientSystemDocument", CurrentViewContext.ApplicantDocId, true, hdnTenantId.Value);
            hdnIsDocViewed.Value = "True";
            signatureDiv.Visible = false;
            btnSavePdf.Visible = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        private void DeleteTempFile(string fileName)
        {
            if (!fileName.IsNullOrEmpty())
            {
                if ((System.IO.File.Exists(fileName)))
                {
                    System.IO.File.Delete(fileName);
                }
            }

        }

        private Boolean UploadViewDocument(ViewDocumentDetailsContract docContract)
        {
            String filePath = String.Empty;
            Boolean aWSUseS3 = false;
            String tempFilePath = WebConfigurationManager.AppSettings["TemporaryFileLocation"];
            filePath = WebConfigurationManager.AppSettings[AppConsts.APPLICANT_FILE_LOCATION];
            if (!ConfigurationManager.AppSettings["AWSUseS3"].IsNullOrEmpty())
            {
                aWSUseS3 = Convert.ToBoolean(ConfigurationManager.AppSettings["AWSUseS3"]);
            }
            if (tempFilePath.IsNullOrEmpty())
            {
                base.LogError("Please provide path for TemporaryFileLocation in config.", null);
                return false;
            }
            if (!tempFilePath.EndsWith(@"\"))
            {
                tempFilePath += @"\";
            }
            tempFilePath += "Tenant(" + CurrentViewContext.TenantId.ToString() + @")\";

            if (!Directory.Exists(tempFilePath))
                Directory.CreateDirectory(tempFilePath);

            //Check whether use AWS S3, true if need to use
            if (aWSUseS3 == false)
            {
                if (filePath.IsNullOrEmpty())
                {
                    base.LogError("Please provide path for " + AppConsts.APPLICANT_FILE_LOCATION + " in config.", null);
                    return false;
                }

                if (!filePath.EndsWith(@"\"))
                {
                    filePath += @"\";
                }
                filePath += "Tenant(" + CurrentViewContext.TenantId.ToString() + @")\";

                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);
            }

            StringBuilder docMessage = new StringBuilder();
            //UAT-862 :- WB: As a student or an admin, I should not be allowed to upload documents with a size of 0 
            StringBuilder corruptedFileMessage = new StringBuilder();

            CurrentViewContext.ToSaveApplicantUploadedDocuments = new List<ApplicantDocument>();
            ApplicantDocument applicantDocument = new ApplicantDocument();

            //Code changes for UAT 531- As a student, I should not be able to upload a duplicate document.
            //Get original file bytes and check if same document is already uploaded
            byte[] fileBytes = File.ReadAllBytes(docContract.DocumentPath);

            //Check whether use AWS S3, true if need to use
            if (aWSUseS3 == false)
            {
                //Move file to other location
                String destFilePath = Path.Combine(filePath, docContract.DocumentName);
                File.Copy(docContract.DocumentPath, destFilePath);
                applicantDocument.DocumentPath = destFilePath;
            }
            else
            {
                if (filePath.IsNullOrEmpty())
                {
                    base.LogError("Please provide path for " + AppConsts.APPLICANT_FILE_LOCATION + " in config.", null);
                    return false;
                }
                if (!filePath.EndsWith(@"/"))
                {
                    filePath += @"/";
                }

                //AWS code to save document to S3 location
                AmazonS3Documents objAmazonS3 = new AmazonS3Documents();
                String destFolder = filePath + "Tenant(" + CurrentViewContext.TenantId.ToString() + @")/";
                String returnFilePath = objAmazonS3.SaveDocument(docContract.DocumentPath, docContract.DocumentName, destFolder);
                applicantDocument.DocumentPath = returnFilePath;
            }


            applicantDocument.OrganizationUserID = CurrentViewContext.OrganizationUserID;
            applicantDocument.FileName = docContract.DocumentName;
            applicantDocument.Size = Convert.ToInt32(new FileInfo(docContract.DocumentPath).Length);
            applicantDocument.CreatedByID = CurrentViewContext.OrganizationUserID; //CurrentViewContext.CurrentLoggedInUserId; UAT 1261
            applicantDocument.CreatedOn = DateTime.Now;
            applicantDocument.IsDeleted = false;
            Presenter.GetDataEntryDocStatus();
            applicantDocument.DataEntryDocumentStatusID = CurrentViewContext.DataEntryDocCompleteStatusId;
            //applicantDocument.DocumentType = CurrentViewContext.ViewDocumentTypeID;

            CurrentViewContext.ToSaveApplicantUploadedDocuments.Add(applicantDocument);
            try
            {
                if (!String.IsNullOrEmpty(docContract.DocumentPath))
                {
                    //   File.Delete(docContract.DocumentPath);
                }
            }
            catch (Exception) { }
            if (CurrentViewContext.ToSaveApplicantUploadedDocuments != null && CurrentViewContext.ToSaveApplicantUploadedDocuments.Count > 0)
            {
                String newFilePath = String.Empty;
                if (aWSUseS3 == false)
                {
                    newFilePath = filePath;
                }
                else
                {
                    if (filePath.IsNullOrEmpty())
                    {
                        base.LogError("Please provide path for " + AppConsts.APPLICANT_FILE_LOCATION + " in config.", null);
                        return false;
                    }
                    if (!filePath.EndsWith(@"/"))
                    {
                        filePath += @"/";
                    }
                    newFilePath = filePath + "Tenant(" + CurrentViewContext.TenantId.ToString() + @")/";
                }
                Int32 addedID = Presenter.AddApplicantViewDocuments(newFilePath);
                //Convert and Merge uploaded documents
                Presenter.CallParallelTaskPdfConversionMerging();

                if (!docContract.IsNullOrEmpty())
                {
                    docContract.AddedViewDocId = addedID;
                    docContract.DocumentName = applicantDocument.FileName;
                    docContract.DocumentPath = applicantDocument.DocumentPath;

                    //Set Hidden fields
                    hdnAppDocumentID.Value = Convert.ToString(addedID);
                    hdnFileName.Value = applicantDocument.FileName;
                }
                Session["ViewDocumentDetailsContract"] = docContract;

                return true;
            }

            return false;
        }

        #endregion

        #region SignatureToImage

        #region Public Properties
        public Color PenColor { get; set; }
        public Color Background { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int PenWidth { get; set; }
        public int FontSize { get; set; }
        #endregion



        /// <summary>
        /// Draws a signature based on the JSON provided by Signature Pad.
        /// </summary>
        /// <param name="json">JSON string of line drawing commands.</param>
        /// <returns>Bitmap image containing the user's signature.</returns>
        private Bitmap SigJsonToImage(string json)
        {
            Width = 648;
            PenWidth = 2;
            Height = 150;
            Bitmap signatureImage = new Bitmap(Width, Height);
            signatureImage.MakeTransparent();
            using (Graphics signatureGraphic = Graphics.FromImage(signatureImage))
            {
                signatureGraphic.Clear(Background);
                signatureGraphic.SmoothingMode = SmoothingMode.AntiAlias;
                Pen pen = new Pen(PenColor, PenWidth);
                List<SignatureLine> lines = (List<SignatureLine>)JsonConvert.DeserializeObject(json ?? string.Empty, typeof(List<SignatureLine>));
                foreach (SignatureLine line in lines)
                {
                    signatureGraphic.DrawLine(pen, line.lx, line.ly, line.mx, line.my);
                }
            }
            return signatureImage;
        }

        /// <summary>
        /// Line drawing commands as generated by the Signature Pad JSON export option.
        /// </summary>
        private class SignatureLine
        {
            public int lx { get; set; }
            public int ly { get; set; }
            public int mx { get; set; }
            public int my { get; set; }
        }

        #endregion

    }
}