using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreWeb.ApplicantRotationRequirement.Views;
using INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.Utils;
using iTextSharp.text.pdf;
using Newtonsoft.Json;
using RadPdf.Web.UI;
using RadPdf.Data.Document;
using INTSOF.UI.Contract.ComplianceOperation;

namespace CoreWeb.ApplicantRotationRequirement.Pages
{
    public partial class ViewDocument : BaseWebPage, IViewDocumentView
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

        Int32 IViewDocumentView.ReqFieldId
        {
            get;
            set;
        }

        Int32 IViewDocumentView.TenantId
        {
            get;
            set;
        }

        ApplicantDocumentContract IViewDocumentView.ClientSystemDocContract
        {
            get;
            set;
        }

        Int32 IViewDocumentView.ReqObjectTreeId
        {
            get;
            set;
        }

        Int32 IViewDocumentView.ApplicantDocId
        {
            get;
            set;
        }

        ObjectAttributeContract IViewDocumentView.objectAttributeContract
        {
            get;
            set;
        }

        ViewDocumentContract IViewDocumentView.ViewDocContract
        {
            get;
            set;
        }

        Boolean ShouldDeleteTempFile = false;

        Int32 IViewDocumentView.OrganizationUserID { get; set; }

        Entity.OrganizationUser IViewDocumentView.OrganizationUserData { get; set; }
        AddressContract IViewDocumentView.Addresses { get; set; }

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
                if (!IsPostBack)
                {
                    if (!Request.QueryString["ClientSysDocID"].IsNullOrEmpty())
                    {
                        CurrentViewContext.ClientSysDocId = Convert.ToInt32(Request.QueryString["ClientSysDocID"]);
                    }
                    if (!Request.QueryString["ReqFieldID"].IsNullOrEmpty())
                    {
                        CurrentViewContext.ReqFieldId = Convert.ToInt32(Request.QueryString["ReqFieldID"]);
                    }
                    if (!Request.QueryString["TenantID"].IsNullOrEmpty())
                    {
                        CurrentViewContext.TenantId = Convert.ToInt32(Request.QueryString["TenantID"]);
                    }
                    if (!Request.QueryString["ReqObjectTreeID"].IsNullOrEmpty())
                    {
                        CurrentViewContext.ReqObjectTreeId = Convert.ToInt32(Request.QueryString["ReqObjectTreeID"]);
                    }
                    if (!Request.QueryString["ApplicantDocID"].IsNullOrEmpty())
                    {
                        CurrentViewContext.ApplicantDocId = Convert.ToInt32(Request.QueryString["ApplicantDocID"]);
                    }
                    if (!Request.QueryString["OrganizationUserId"].IsNullOrEmpty())
                    {
                        CurrentViewContext.OrganizationUserID = Convert.ToInt32(Request.QueryString["OrganizationUserId"]);
                    }
                    Presenter.GetOrganizationUserDetails();
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
                //iframePdfDocViewer.Src = String.Format("/ComplianceOperations/UserControl/DocumentViewer.aspx?DocumentType={0}&FileName={1}&tenantId={2}", "ClientSystemDocument", "DocumentViewer.pdf", hdnTenantId.Value);
                //btnSavePdf.OnClientClicked= "btnSavePdfClick";
                //btnSavePdf.OnClientClick = "return PdfSave();";
                AddSignatureOnPdf();
                //Page.ClientScript.RegisterStartupScript(Page.GetType(), Guid.NewGuid().ToString(), "btnSavePdfClick();", true);
                //System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "btnSavePdfClick();", true);

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
            //Presenter.GetSystemDocumentData();
            ViewState["DocumentUrl"] = CurrentViewContext.ViewDocContract.DocumentPath;
            hdnTenantId.Value = Convert.ToString(CurrentViewContext.TenantId);
            hfIsSignatureRequired.Value = CurrentViewContext.ViewDocContract.IsSignatureRequired ? "True" : "False";
            hdnFileName.Value = CurrentViewContext.ViewDocContract.DocumentName;
        }

        /// <summary>
        /// 
        /// </summary>
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
                //AcroFields.FieldPosition signatureImagePosition = null;

                //Fill-in the form values
                AcroFields af = stamper.AcroFields;

                stamper.FormFlattening = true;
                float left = 0;
                float right = 0;
                float top = 0;
                float heigth = 0;

                int i = 0;
                foreach (var signature in af.GetFieldPositions("Signature"))
                {
                    try
                    {
                        AcroFields.FieldPosition signatureImagePosition = af.GetFieldPositions("Signature")[i];
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
                        i = i + 1;
                    }
                    catch (Exception)
                    {
                    }

                }
                #region Old Commented Code As Per the implementation of UAT-3263: Ability to add multiple occurrences of fields on the rotation complete document.
                //try { signatureImagePosition = af.GetFieldPositions("Signature")[0]; }
                //catch { }

                //if (signatureImagePosition != null && imageToAddToDocument != null)
                //{
                //    left = signatureImagePosition.position.Left;
                //    right = signatureImagePosition.position.Right;
                //    top = signatureImagePosition.position.Top;
                //    heigth = signatureImagePosition.position.Height;

                //    iTextSharp.text.Image signatureImage = iTextSharp.text.Image.GetInstance(imageToAddToDocument);
                //    //PdfContentByte contentByte = stamper.GetOverContent(1);
                //    PdfContentByte contentByte = stamper.GetOverContent(signatureImagePosition.page); // uat - 856 : WB: Signature on disclosure is not placing properly when there are multiple pages in the document
                //    float currentImageHeigth = 0;
                //    currentImageHeigth = signatureImage.Height;
                //    float ratio = 0;
                //    ratio = heigth / currentImageHeigth;
                //    float width = signatureImage.Width * ratio;
                //    signatureImage.ScaleAbsoluteHeight(heigth);
                //    signatureImage.ScaleAbsoluteWidth(width);
                //    signatureImage.SetAbsolutePosition(left, top - signatureImage.ScaledHeight);
                //    contentByte.AddImage(signatureImage);
                //}
                #endregion
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
        public bool AddSignatureOnPdf()
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

                //Fill-in the form values
                AcroFields acroF = stamper.AcroFields;

                if (hfIsSignatureRequired.Value == "True" && acroF.GetField("Signature").IsNotNull() && hdnOutput.Value.IsNullOrEmpty())
                {
                    lblErrorSig.Visible = true;
                    lblErrorSig.Text = "Please Sign the document.";
                    return false;
                }

                if (hfIsSignatureRequired.Value == "True" && acroF.GetField("Signature").IsNotNull())
                {
                    //string representing the user's signature

                    string jsonStr = hdnOutput.Value;
                    // iTextSharpPDFWrapper its = new iTextSharpPDFWrapper();

                    //create image from json
                    System.Drawing.Bitmap signatureImage = SigJsonToImage(jsonStr);
                    // Save out to memory and then to a file.
                    MemoryStream mm = new MemoryStream();
                    signatureImage.Save(mm, System.Drawing.Imaging.ImageFormat.Png);
                    bufferSignature = mm.GetBuffer();

                    //Checking Signature length
                    if (bufferSignature.IsNotNull() && bufferSignature.Length < Convert.ToInt32(WebConfigurationManager.AppSettings["MinApplicantSignLength"]))
                    {
                        lblErrorSig.Visible = true;
                        lblErrorSig.Text = "Provided text does not qualify as valid Signature. Please provide valid Signature.";
                        return false;
                    }

                    //We dispose of all objects to make sure the files don't stay locked.
                    signatureImage.Dispose();
                    mm.Dispose();
                    mergedSignedDocumentBuffer = FillSignatureInDocument(buffer, bufferSignature);
                }
                else
                {
                    stamper.Close();
                    mergedSignedDocumentBuffer = ms.ToArray();
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
                    iframePdfDocViewer.Src = String.Format("/ComplianceOperations/UserControl/DocumentViewer.aspx?DocumentType={0}&FileName={1}&tenantId={2}", "ClientSystemDocument", fileName, hdnTenantId.Value);
                    hfTemporaryApplicantDocPath.Value = TemporaryFilePath;
                    btnSavePdf.Visible = false;
                    signatureDiv.Visible = false;
                    //ViewState["TempFilePath"] = TemporaryFilePath;
                    //ShouldDeleteTempFile = true;
                    //System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "btnSavePdfClick();", true);
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
                byte[] pdfData = CommonFileManager.RetrieveDocument(ViewState["DocumentUrl"].ToString(), FileType.SystemDocumentLocation.GetStringValue());
                if (pdfData.IsNotNull())
                {
                    PdfReader reader = new PdfReader(pdfData);
                    MemoryStream ms = new MemoryStream();
                    PdfStamper stamper = new PdfStamper(reader, ms);
                    AcroFields af = stamper.AcroFields;

                    if (af.GetField("FullName").IsNotNull())
                    {
                        af.SetField("FullName", CurrentViewContext.ViewDocContract.FullName);
                        af.SetFieldProperty("FullName",
                                            "setfflags",
                                             PdfFormField.FF_READ_ONLY,
                                             null);
                    }

                    if (af.GetField("CurrentDate").IsNotNull())
                    {
                        af.SetField("CurrentDate", DateTime.Now.ToString("MM/dd/yyyy"));
                        af.SetFieldProperty("CurrentDate",
                                            "setfflags",
                                             PdfFormField.FF_READ_ONLY,
                                             null);
                    }

                    if (af.GetField("FName").IsNotNull())
                    {
                        af.SetField("FName", CurrentViewContext.OrganizationUserData.FirstName);
                        af.SetFieldProperty("FName",
                                            "setfflags",
                                             PdfFormField.FF_READ_ONLY,
                                             null);

                    }
                    if (af.GetField("MName").IsNotNull())
                    {
                        af.SetField("MName", CurrentViewContext.OrganizationUserData.MiddleName);
                        af.SetFieldProperty("MName",
                                            "setfflags",
                                             PdfFormField.FF_READ_ONLY,
                                             null);

                    }
                    if (af.GetField("LName").IsNotNull())
                    {
                        af.SetField("LName", CurrentViewContext.OrganizationUserData.LastName);
                        af.SetFieldProperty("LName",
                                            "setfflags",
                                             PdfFormField.FF_READ_ONLY,
                                             null);

                    }
                    if (af.GetField("FullSSN").IsNotNull())
                    {
                        af.SetField("FullSSN", Presenter.GetApplicantSSN());
                        af.SetFieldProperty("FullSSN",
                                            "setfflags",
                                             PdfFormField.FF_READ_ONLY,
                                             null);

                    }
                    if (af.GetField("DOB").IsNotNull())
                    {
                        af.SetField("DOB", CurrentViewContext.OrganizationUserData.DOB.Value.ToShortDateString());
                        af.SetFieldProperty("DOB",
                                            "setfflags",
                                             PdfFormField.FF_READ_ONLY,
                                             null);

                    }
                    if (af.GetField("PhoneNumber").IsNotNull())
                    {
                        af.SetField("PhoneNumber", CurrentViewContext.OrganizationUserData.PhoneNumber);
                        af.SetFieldProperty("PhoneNumber",
                                            "setfflags",
                                             PdfFormField.FF_READ_ONLY,
                                             null);

                    }
                    if (af.GetField("EmailAddress").IsNotNull())
                    {
                        af.SetField("EmailAddress", CurrentViewContext.OrganizationUserData.PrimaryEmailAddress);
                        af.SetFieldProperty("EmailAddress",
                                            "setfflags",
                                             PdfFormField.FF_READ_ONLY,
                                             null);

                    }
                    if (af.GetField("Gender").IsNotNull())
                    {
                        af.SetField("Gender", CurrentViewContext.OrganizationUserData.Gender.HasValue ? CurrentViewContext.OrganizationUserData.lkpGender.GenderName : "N/A");
                        af.SetFieldProperty("Gender",
                                            "setfflags",
                                             PdfFormField.FF_READ_ONLY,
                                             null);

                    }
                    if (af.GetField("aliasName").IsNotNull())
                    {                      
                        String FirstAliasName = String.Empty;
                        if (!CurrentViewContext.OrganizationUserData.IsNullOrEmpty())
                        {
                            Entity.PersonAlia alias = CurrentViewContext.OrganizationUserData.PersonAlias.Where(x => !x.PA_IsDeleted).FirstOrDefault();
                            if(alias.IsNotNull())
                            {
                                //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                                String middleName = alias.PA_MiddleName.IsNullOrEmpty() ? NoMiddleNameText : alias.PA_MiddleName;
                                FirstAliasName = (Convert.ToString(alias.PA_FirstName) + " " + middleName + " " + Convert.ToString(alias.PA_LastName));                              
                            }                          
                        }
                        af.SetField("aliasName", FirstAliasName);
                        af.SetFieldProperty("aliasName",
                                            "setfflags",
                                             PdfFormField.FF_READ_ONLY,
                                             null);

                    }
                    if (af.GetField("Address1").IsNotNull())
                    {
                        af.SetField("Address1", CurrentViewContext.Addresses.Address1);
                        af.SetFieldProperty("Address1",
                                            "setfflags",
                                             PdfFormField.FF_READ_ONLY,
                                             null);

                    }
                    if (af.GetField("Address2").IsNotNull())
                    {
                        af.SetField("Address2", CurrentViewContext.Addresses.Address2);
                        af.SetFieldProperty("Address2",
                                            "setfflags",
                                             PdfFormField.FF_READ_ONLY,
                                             null);

                    }
                    if (af.GetField("City").IsNotNull())
                    {
                        af.SetField("City", CurrentViewContext.Addresses.CityName);
                        af.SetFieldProperty("City",
                                            "setfflags",
                                             PdfFormField.FF_READ_ONLY,
                                             null);

                    }
                    if (af.GetField("State").IsNotNull())
                    {
                        af.SetField("State", CurrentViewContext.Addresses.StateName);
                        af.SetFieldProperty("State",
                                            "setfflags",
                                             PdfFormField.FF_READ_ONLY,
                                             null);

                    }
                    if (af.GetField("PostalCode").IsNotNull())
                    {
                        af.SetField("PostalCode", CurrentViewContext.Addresses.Zipcode);
                        af.SetFieldProperty("PostalCode",
                                            "setfflags",
                                             PdfFormField.FF_READ_ONLY,
                                             null);

                    }
                    if (af.GetField("County").IsNotNull())
                    {
                        af.SetField("County", CurrentViewContext.Addresses.CountyName);
                        af.SetFieldProperty("County",
                                            "setfflags",
                                             PdfFormField.FF_READ_ONLY,
                                             null);

                    }
                    if (af.GetField("Country").IsNotNull())
                    {
                        af.SetField("Country", CurrentViewContext.Addresses.Country);
                        af.SetFieldProperty("Country",
                                            "setfflags",
                                             PdfFormField.FF_READ_ONLY,
                                             null);

                    }
                    if (af.GetField("InstitutionName").IsNotNull())
                    {
                        af.SetField("InstitutionName", CurrentViewContext.OrganizationUserData.Organization.Tenant.TenantName);
                        af.SetFieldProperty("InstitutionName",
                                            "setfflags",
                                             PdfFormField.FF_READ_ONLY,
                                             null);

                    }
                    if (af.GetField("LastFourDigitofSSN").IsNotNull())
                    {
                        af.SetField("LastFourDigitofSSN", CurrentViewContext.OrganizationUserData.SSNL4);
                        af.SetFieldProperty("LastFourDigitofSSN",
                                            "setfflags",
                                             PdfFormField.FF_READ_ONLY,
                                             null);

                    }

                    #region Disable Signature
                    if (af.GetField("SignatureImage").IsNotNull())
                    {
                        af.SetFieldProperty("SignatureImage", "setfflags", PdfFormField.FF_READ_ONLY, null);
                    }
                    if (af.GetField("Signature").IsNotNull())
                    {
                        af.SetFieldProperty("Signature", "setfflags", PdfFormField.FF_READ_ONLY, null);
                    }
                    if (af.GetField("SignatureImage2").IsNotNull())
                    {
                        af.SetFieldProperty("SignatureImage2", "setfflags", PdfFormField.FF_READ_ONLY, null);
                    }
                    #endregion
                    if (!CurrentViewContext.ViewDocContract.IsSignatureRequired)
                    {
                        signatureDiv.Visible = false;
                        //btnSavePdf.Visible = false;
                    }
                    if (af.GetField("BlankField").IsNotNull())
                    {
                        af.SetFieldProperty("BlankField", "setfflags", PdfFormField.FF_EDIT, null);
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
                        //fileName = "ApplicantDocument.pdf";
                        TemporaryFilePath = String.Format(TemporaryDirectoryPath + fileName);
                        //System.IO.File.WriteAllBytes(TemporaryFilePath, updatedDocument);
                        //iframePdfDocViewer.Src = String.Format("/ComplianceOperations/UserControl/DocumentViewer.aspx?DocumentType={0}&FileName={1}&tenantId={2}", "ClientSystemDocument", fileName, hdnTenantId.Value);
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

        //protected void PdfWebControl_Saved(object sender, RadPdf.Integration.DocumentSavedEventArgs e)
        //{
        //    //AddSignatureOnPdf();
        //    var FilePath = String.Empty;
        //    var docData = e.DocumentData;
        //    if (!ViewState["DocumentUrl"].IsNullOrEmpty())
        //    {
        //        FilePath = ViewState["DocumentUrl"].ToString();
        //        System.IO.File.WriteAllBytes(FilePath, docData);
        //    }
        //    else
        //    {
        //        if (Request.QueryString["DocumentPath"] != null && !Request.QueryString["DocumentPath"].Trim().Equals(""))
        //            FilePath = Convert.ToString(Request.QueryString["DocumentPath"]);
        //        System.IO.File.WriteAllBytes(FilePath, docData);
        //    }

        //}
    }
}