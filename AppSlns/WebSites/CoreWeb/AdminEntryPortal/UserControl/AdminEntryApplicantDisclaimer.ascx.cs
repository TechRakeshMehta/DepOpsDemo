#region Namespaces

#region SystemDefined

using System;
using System.Collections.Generic;


#endregion

#region UserDefined

using INTSOF.Utils;
using CoreWeb.Shell;
using INTSOF.UI.Contract.ComplianceOperation;
using CoreWeb;
using Business.RepoManagers;
using CoreWeb.AdminEntryPortal.Views;
using System.IO;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using Newtonsoft.Json;
using System.Drawing.Text;
using System.Collections;
using System.Linq;
using Entity.ClientEntity;
using iTextSharp.text.pdf;
using System.Web.Configuration;
using CoreWeb.IntsofSecurityModel;
#endregion

#endregion

public partial class AdminEntryPortal_UserControl_AdminEntryApplicantDisclaimer : BaseUserControl, IAdminEntryApplicantDisclaimerView
{
    #region Variables

    #region Private Variables
    private AdminEntryApplicantDisclaimerPresenter _presenter = new AdminEntryApplicantDisclaimerPresenter();
    private ApplicantOrderCart _applicantOrderCart;


    #endregion

    #region Public Variables

    #endregion

    #endregion

    #region Properties

    #region Private Properties

    /// <summary>
    /// Used to identify whether the User has clicked on the Accept button
    /// </summary>
    private Boolean IsAcceptanceClicked
    {
        get
        {
            if (!ViewState["IsAcceptanceClicked"].IsNullOrEmpty())
                return Convert.ToBoolean(ViewState["IsAcceptanceClicked"]);

            return false;
        }
        set
        {
            ViewState["IsAcceptanceClicked"] = value;
        }
    }

    #endregion

    #region Public Properties

    public Int32 CurrentLoggedInUserID
    {
        get
        {
            return SysXWebSiteUtils.SessionService.OrganizationUserId;
        }

    }
    public Int32 TenantId
    {
//UAT-5114
        //get
        //{

        //    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
        //    if (user.IsNotNull())
        //    {
        //        return user.TenantId.HasValue ? user.TenantId.Value : 0;
        //    }

        //    return AppConsts.NONE;
        //}
        get
        {
            ApplicantOrderCart applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);

            if (!applicantOrderCart.IsNullOrEmpty() && applicantOrderCart.IsAdminEntryPortalOrder)
                return applicantOrderCart.TenantId;
            return _presenter.GetTenantId(CurrentLoggedInUserID);
        }
    }
    public Int32 DPP_ID
    {
        get;
        set;
    }
    public String PackageName
    {
        get
        {
            return (String)ViewState["PackageName"];
        }
        set
        {
            ViewState["PackageName"] = value;
        }
    }
    public AdminEntryApplicantDisclaimerPresenter Presenter
    {
        get
        {
            this._presenter.View = this;
            return this._presenter;
        }

    }
    public IAdminEntryApplicantDisclaimerView CurrentViewContext
    {
        get
        {
            return this;
        }
    }

    /// <summary>
    /// Used to identify the current Order Request Type i.e. New order, Change subscription etc.
    /// </summary>
    public String OrderType
    {
        get
        {
            if (!ViewState[AppConsts.ORDER_REQUEST_TYPE_VIEWSTATE].IsNullOrEmpty())
                return Convert.ToString(ViewState[AppConsts.ORDER_REQUEST_TYPE_VIEWSTATE]);

            return String.Empty;
        }
        set
        {
            ViewState[AppConsts.ORDER_REQUEST_TYPE_VIEWSTATE] = value;
        }
    }

    //UAT-1261: WB: As an ADB admin, I should be able to "login" as any student to see what they see.
    public Int32 OrgUsrID
    {
        get
        {
            if (!System.Web.HttpContext.Current.Session["AppViewAdminOrgUsrID"].IsNullOrEmpty())
                return Convert.ToInt32(System.Web.HttpContext.Current.Session["AppViewAdminOrgUsrID"]);
            else
                return CurrentLoggedInUserID;
        }
    }

    #region UAT-5114
    public Boolean SystemDocumentIsDeleted
    {
        get;
        set;
    }

    public String DocumentPath
    {
        get;
        set;
    }

    public Int32 DisclaimerDocumentSystemDocumentID
    {
        get
        {
            if (ViewState["DisclaimerDocumentSystemDocumentID"] != null)
                return Convert.ToInt32(ViewState["DisclaimerDocumentSystemDocumentID"]);
            return 0;
        }
        set
        {
            ViewState["DisclaimerDocumentSystemDocumentID"] = value;
        }
    }

    #endregion

    #endregion

    #endregion

    #region Events

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            RedirectIfIncorrectOrderStage();
            ApplicantOrderCart applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);




            String _currentStep = " (Step " + (applicantOrderCart.lstApplicantOrder[0].PreviousOrderStep) +
                              " of " + applicantOrderCart.GetTotalOrderSteps() + ")";

            base.SetPageTitle(_currentStep);

            if (applicantOrderCart.IsNotNull())
            {
                CurrentViewContext.DPP_ID = applicantOrderCart.DPP_Id.HasValue ? applicantOrderCart.DPP_Id.Value : 0;

            }


            fsucCmdBar1.ClearButton.ToolTip = "Agree to the Disclaimer and proceed to order confirmation";
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
                LoadDisclaimerDocument();
            }
            Presenter.OnViewLoaded();

            //if (!applicantOrderCart.IsNullOrEmpty() &&
            //    (applicantOrderCart.OrderRequestType == OrderRequestType.NewOrder.GetStringValue()
            //    || applicantOrderCart.OrderRequestType == OrderRequestType.ChangeSubscription.GetStringValue()))
            //    (this.Page as CoreWeb.AdminEntryPortal.Views.Default).SetModuleTitle("Create Order");
            //else
            (this.Page as CoreWeb.AdminEntryPortal.Views.Default).SetModuleTitle("Complete Your Order");

            CurrentViewContext.OrderType = applicantOrderCart.OrderRequestType;
            SetButtonText();
            Session["IsSignatureStored"] = null;
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

    protected void Page_PreRender(object sender, EventArgs e)
    {

    }

    #endregion







    #region Button Events

    /// <summary>
    /// Event for Forward navigation i.e. Accept/Proceed/Next Button
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void fsucCmdBar1_SubmitClick(object sender, EventArgs e)
    {
        try
        {


            if (chkAccept.Checked && !hiddenOutput.Value.IsNullOrEmpty())
            {
                var signatureBuffer = GetSignatureImageBuffer(hiddenOutput.Value);
                if (signatureBuffer.IsNotNull() && signatureBuffer.Length < Convert.ToInt32(WebConfigurationManager.AppSettings["MinApplicantSignLength"]))
                {
                    signatureDiv.Style.Add("display", "block");
                    signature.Attributes.Remove("disabled");
                    lblErrorSig.Visible = true;
                    lblErrorSig.Text = "Provided text does not qualify as valid Signature. Please provide valid Signature.";
                    lblErrorChkBox.Visible = false;
                    return;
                }

                lblErrorSig.Visible = false;
                if (fsucCmdBar1.ClearButton.Text == AppConsts.NEXT_BUTTON_TEXT || fsucCmdBar1.ClearButton.Text == AppConsts.ACCEPT_BUTTON_TEXT)
                    this.IsAcceptanceClicked = true;
                else
                    this.IsAcceptanceClicked = false;


                if (fsucCmdBar1.ClearButton.Value == AppConsts.PROCEED_BUTTON_TEXT || fsucCmdBar1.ClearButton.Value == AppConsts.NEXT_BUTTON_TEXT)
                {
                    ApplicantOrderCart applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
                    if (applicantOrderCart != null)
                    {



                        applicantOrderCart.IncrementOrderStepCount();

                        SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.DISCLAIMER_ACCEPTED, true);
                        //Commented Code For UAT-1560
                        Dictionary<String, String> queryString = new Dictionary<String, String>();
                        //Commented below code regarding UAT-5184
                        //if (applicantOrderCart.IsAdditionalDocumentExist)
                        //{
                        //    queryString = new Dictionary<String, String>
                        //                                 {
                        //                                    { AppConsts.CHILD,  ChildControls.AdminEntryApplicantRequiredDocumentationPage}
                        //                                 };
                        //    Response.Redirect(String.Format("~/AdminEntryPortal/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
                        //}
                        //else
                        //{
                        applicantOrderCart.AddOrderStageTrackID(OrderStages.Disclosure);
                        queryString = new Dictionary<String, String>
                                                         {
                                                            { AppConsts.CHILD,  ChildControls.AdminEntryApplicantDisclosurePage}
                                                         };
                            Response.Redirect(String.Format("~/AdminEntryPortal/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
                        //}
                    }
                }



                if (CreateAndStoreESignature())
                {
                    lblMessage.Text = "";
                    LoadESignedDisclaimerDocument();
                }


                SetButtonText();

                chkAccept.Enabled = false;
                signatureDiv.Visible = false;
                signature.Visible = false;
                signature.Attributes.Add("disabled", "disabled");
                lblErrorChkBox.Visible = false;
                lblErrorSig.Visible = false;
                lblSignMessage.Text = "(Scroll down to continue)";
            }
            else
            {
                if (chkAccept.Checked == false)
                {
                    lblErrorChkBox.Visible = true;
                    lblErrorSig.Visible = false;
                }
                else
                {
                    lblErrorChkBox.Visible = false;
                }
                if (hiddenOutput.Value.IsNullOrEmpty())
                {
                    lblErrorSig.Text = "Please Sign the document.";
                    lblErrorSig.Visible = true;
                }
                else
                {
                    lblErrorSig.Visible = false;
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

    /// <summary>
    /// Event for Backward navigation i.e. Previous or Restart Order button
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void fsucCmdBar1_ExtraClick(object sender, EventArgs e)
    {
        try
        {
            ApplicantOrderCart applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
            Dictionary<String, String> queryString;                                         

            if (CurrentViewContext.OrderType == OrderRequestType.NewOrder.GetStringValue())
            {
                //Added below code for UAT-3541 :- CBI || CABS
                //applicantOrderCart.AddOrderStageTrackID(OrderStages.Disclosure);
                //applicantOrderCart.DecrementOrderStepCount();
                applicantOrderCart.AddOrderStageTrackID(OrderStages.ApplicantProfile);
                applicantOrderCart.DecrementOrderStepCount();
                if (applicantOrderCart.lstApplicantOrder[0].lstPackages.IsNullOrEmpty()
                   ||
                   (!applicantOrderCart.lstApplicantOrder[0].lstPackages.IsNullOrEmpty() && applicantOrderCart.lstCustomFormData.IsNullOrEmpty()))
                {
                    // To avoid Redirection again by the browser back button navigation check method
                    applicantOrderCart.AddOrderStageTrackID(OrderStages.ApplicantProfile); //UAT - 5184

                    queryString = new Dictionary<String, String>()
                                                         {
                                                            { "Child",  AppConsts.ADMIN_ENTRY_APPLICANT_INFORMATION}
                                                         };
                    Response.Redirect(String.Format("Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
                }
                else
                {
                    var _formToLoad = applicantOrderCart.lstFormExecuted.LastOrDefault();
                    // Redirect to Custom Forms
                    applicantOrderCart.AddOrderStageTrackID(OrderStages.CustomForms);
                    applicantOrderCart.IsEditMode = true;
                    queryString = new Dictionary<String, String>
                                                                 {
                                                                    { AppConsts.CHILD, ChildControls.AdminEntryCustomFormLoad},
                                                                    {"CustomFormId",Convert.ToString(_formToLoad)},
                                                                    {"IsPrevious","1"}
                                                                 };
                    Response.Redirect(String.Format("~/AdminEntryPortal/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
                }
            }

            else
            {

                applicantOrderCart.ClearOrderCart(applicantOrderCart);
                Session.Remove(AppConsts.DISCLAIMER_ACCEPTED);
                queryString = new Dictionary<String, String>
                                                                 {
                                                                    { AppConsts.CHILD,  AppConsts.ADMIN_ENTRY_APPLICANT_LANDING_SCREEN},
                                                                    { "TenantId",applicantOrderCart.TenantId.ToString()},
                                                                    { "OrderId",applicantOrderCart.OrderId.ToString()}
                                                                 };
                Response.Redirect(String.Format("~/AdminEntryPortal/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
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

    protected void fsucCmdBar1_CancelClick(object sender, EventArgs e)
    {
        try
        {
            ApplicantOrderCart applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
            Session.Remove(ResourceConst.APPLICANT_ORDER_CART);
            Session.Remove(AppConsts.DISCLAIMER_ACCEPTED);
            Session.Remove(AppConsts.DISCLOSURE_ACCEPTED);
            Session.Remove(ResourceConst.APPLICANT_DRUG_SCREENING);
            Dictionary<String, String> queryString;
            queryString = new Dictionary<String, String>

                                                                 {
                                                                    { AppConsts.CHILD,  AppConsts.ADMIN_ENTRY_APPLICANT_LANDING_SCREEN},
                                                                    { "TenantId",applicantOrderCart.TenantId.ToString()},
                                                                    { "OrderId",applicantOrderCart.OrderId.ToString()}




                                                                 };
            Response.Redirect(String.Format("~/AdminEntryPortal/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));


        }










        catch (SysXException ex)
        {
            base.LogError(ex);
            base.ShowErrorMessage(ex.Message);
        }
        catch (Exception ex)
        {
            base.LogError(ex);
            base.ShowErrorMessage(ex.Message);
        }
    }
    #endregion







    #endregion

    #region Methods






    #region Private Methods

    private byte[] GetSignatureImageBuffer(String jsonStr)
    {
        System.Drawing.Bitmap signatureImage = SigJsonToImage(jsonStr);
        // Save out to memory and then to a file.
        MemoryStream mm = new MemoryStream();
        signatureImage.Save(mm, System.Drawing.Imaging.ImageFormat.Png);
        byte[] bufferSignature = mm.GetBuffer();
        //We dispose of all objects to make sure the files don't stay locked.
        signatureImage.Dispose();
        mm.Dispose();
        return bufferSignature;
    }

    /// <summary>
    /// Checks the order status track. If this page is not opened as per correct order status track then, redirected to correct order.
    /// </summary>
    /// <param name="applicantOrderCart"></param>
    /// 
    private void RedirectIfIncorrectOrderStage()
    {
        ApplicantOrderCart applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
        RedirectInvalidOrder(applicantOrderCart);
        CurrentViewContext.OrderType = applicantOrderCart.OrderRequestType;
        String nextPagePath = ComplianceDataManager.GetNextPagePathByOrderStageID(applicantOrderCart, OrderStages.Disclaimer);

        //Redirect to next page path if Order Status track is not correct.
        if (nextPagePath.IsNotNull())
        {
            Response.Redirect(nextPagePath);
        }
        else
        {
            applicantOrderCart.AddOrderStageTrackID(OrderStages.Disclaimer);
        }
    }
    /// <summary>
    /// This method read the signature and fill into the pdf
    /// </summary>
    /// <returns></returns>
    private bool CreateAndStoreESignature()
    {
        bool status = false;
        //UAT-5114
        bool isOverrideDisclaimerDocument = Presenter.IsOverrideDisclaimerDocument();
        // SignatureToImage sigGenerator = new SignatureToImage();   
        try
        {
            String tempDocPath = String.Empty;
            Boolean aWSUseS3 = false;
            if (!Session["tmpFilePathforTrackingDoc"].IsNullOrEmpty())
            {

                tempDocPath = Convert.ToString(Session["tmpFilePathforTrackingDoc"]);
                //firstNonEmptyValue = null;

            }
            if (!ConfigurationManager.AppSettings["AWSUseS3"].IsNullOrEmpty())
            {
                aWSUseS3 = Convert.ToBoolean(ConfigurationManager.AppSettings["AWSUseS3"]);
            }

            Presenter.GetDisclaimerDocumentData();
            byte[] buffer = null;



            byte[] mergedSignedDocumentBuffer = null;
            byte[] bufferSignature = null;

            FileStream _fileStream = null;
            long _totalBytes = 0;
            if (isOverrideDisclaimerDocument && DocumentPath != null && SystemDocumentIsDeleted == false) //UAT-4592
            {
                //Check whether use AWS S3, true if need to use
                if (aWSUseS3 == false)
                {
                    if (File.Exists(tempDocPath))
                    {
                        _fileStream = new FileStream(tempDocPath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                        _totalBytes = new System.IO.FileInfo(tempDocPath).Length;
                        BinaryReader _binaryReader = new BinaryReader(_fileStream);
                        //read entire file into buffer
                        buffer = _binaryReader.ReadBytes((Int32)_totalBytes);
                    }
                    else
                    {
                        base.LogInfo("File not found" + DocumentPath);
                        lblMessage.Text = "No disclaimer form to display";
                        //lnkbtnViewDisclosureForm.Visible = false;
                        return false;
                    }
                }
                else
                {
                    String tempFilePath = ConfigurationManager.AppSettings["TemporaryFileLocation"];
                    String applicantFileLocation = String.Empty;
                    String filename = String.Empty;

                    if (tempFilePath.IsNullOrEmpty())
                    {
                        // base.LogError("Please provide path for TemporaryFileLocation in config.", new SystemException());
                        throw new SystemException("Please provide path for TemporaryFileLocation in config.");
                    }
                    if (!tempFilePath.EndsWith(@"\"))
                    {
                        tempFilePath += @"\";
                    }
                    tempFilePath += "Tenant(" + TenantId.ToString() + @")\";
                    byte[] documentContent = null;
                    if (!Directory.Exists(tempFilePath))
                        Directory.CreateDirectory(tempFilePath);
                    //String newTempFilePathForRadPDFSavedFile = Convert.ToString(Session["newTempFilePathForRadPDFSave"]);
                    if (File.Exists(tempDocPath))
                    {
                        _fileStream = new FileStream(tempDocPath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                        var totalBytes = new System.IO.FileInfo(tempDocPath).Length;
                        BinaryReader _binaryReader = new BinaryReader(_fileStream);
                        //read entire file into buffer
                        documentContent = _binaryReader.ReadBytes((Int32)totalBytes);
                        _fileStream.Close();
                    }
                    //AmazonS3Documents objAmazonS3Documents = new AmazonS3Documents();
                    //byte[] documentContent = objAmazonS3Documents.RetrieveDocument(DocumentPath);

                    if (!documentContent.IsNullOrEmpty())
                    {
                        buffer = documentContent;
                    }
                    else
                    {
                        base.LogInfo("File not found" + DocumentPath);
                        lblMessage.Text = "No disclaimer form to display";
                        return false;
                    }
                }
            }
            else
            {
                byte[] documentContent = null;
                _fileStream = new FileStream(Server.MapPath("~/App_Data/DisclaimerDocument.pdf"), System.IO.FileMode.Open, System.IO.FileAccess.Read);
                _totalBytes = new System.IO.FileInfo(Server.MapPath("~/App_Data/DisclaimerDocument.pdf")).Length;

                BinaryReader _binaryReader = new BinaryReader(_fileStream);

                //// read entire file into buffer
                documentContent = _binaryReader.ReadBytes((Int32)_totalBytes);

                if (!documentContent.IsNullOrEmpty())
                {
                    buffer = documentContent;
                }
                else
                {
                    base.LogInfo("File not found" + Server.MapPath("~/App_Data/DisclaimerDocument.pdf"));
                    lblMessage.Text = "No disclaimer form to display";
                    return false;
                }
            }

            PdfReader reader = new PdfReader(buffer);
            MemoryStream ms = new MemoryStream();
            PdfStamper stamper = new PdfStamper(reader, ms);


            //Fill-in the form values
            AcroFields acroF = stamper.AcroFields;

            if (acroF.GetField("SignatureImage").IsNotNull())
            {
                try
                {
                    //string representing the user's signature

                    string jsonStr = hiddenOutput.Value;


                    //create image from json
                    System.Drawing.Bitmap signatureImage = SigJsonToImage(jsonStr);
                    // Save out to memory and then to a file.
                    MemoryStream mm = new MemoryStream();
                    signatureImage.Save(mm, System.Drawing.Imaging.ImageFormat.Png);
                    bufferSignature = mm.GetBuffer();
                    //We dispose of all objects to make sure the files don't stay locked.
                    signatureImage.Dispose();
                    mm.Dispose();

                    LogInfo("Filling the E-signature in the disclaimer pdf");
                    mergedSignedDocumentBuffer = _presenter.FillSignatureInDisClaimerPDFDocument(buffer, bufferSignature);
                    LogInfo("Filled the E-signature in the disclaimer pdf ");

                    if (mergedSignedDocumentBuffer != null)
                    {
                        if (SaveDisclaimerPdfWithEsignature(mergedSignedDocumentBuffer))
                        {
                            status = true;
                        }
                    }
                    try
                    {
                        if (File.Exists(tempDocPath))
                            File.Delete(tempDocPath);
                    }
                    catch (Exception ex) { }
                    Session["IsSignatureStored"] = "true";
                }
                catch (Exception ex)
                {
                    base.LogError(ex);
                    throw ex;
                }
            }
            else
            {
                base.ShowInfoMessage("Signature cannot be embedded in the pdf.Please contact your system administrator");
                fsucCmdBar1.SubmitButton.Enabled = false;
            }
            return status;
        }
        catch (Exception ex)
        {
            base.LogError(ex);
        }
        return status;
    }
    /// <summary>
    /// Save the ESigned disclaimer document 
    /// </summary>
    /// <param name="pdfBytes">pdfBytes</param>
    /// <returns>Boolean</returns>
    private Boolean SaveDisclaimerPdfWithEsignature(byte[] pdfBytes)
    {
        String tempFilePath = ConfigurationManager.AppSettings["TemporaryFileLocation"];
        String applicantFileLocation = String.Empty;
        String filename = String.Empty;
        Boolean aWSUseS3 = false;
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
        tempFilePath += "Tenant(" + TenantId.ToString() + @")\";

        if (!Directory.Exists(tempFilePath))
            Directory.CreateDirectory(tempFilePath);

        //Check whether use AWS S3, true if need to use
        if (aWSUseS3 == false)
        {
            applicantFileLocation = ConfigurationManager.AppSettings["ApplicantFileLocation"];
            if (!applicantFileLocation.EndsWith("\\"))
            {
                applicantFileLocation += "\\";
            }
            applicantFileLocation += "Tenant(" + TenantId.ToString() + @")\";

            if (!Directory.Exists(applicantFileLocation))
            {
                Directory.CreateDirectory(applicantFileLocation);
            }
        }

        _applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
        if (_applicantOrderCart.IsNotNull())
        {
            _presenter.GetPackageName(Convert.ToInt32(_applicantOrderCart.DPP_Id), TenantId);
        }
        String date = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("mmss") + DateTime.Now.Millisecond.ToString();
        String destFileName = "DD_" + TenantId.ToString() + "_" + CurrentLoggedInUserID + "_" + date + ".pdf";
        String newTempFilePath = Path.Combine(tempFilePath, destFileName);
        String newFinalFilePath = String.Empty;

        if (!String.IsNullOrEmpty(PackageName))
        {
            filename = "Disclaimer" + "_" + PackageName + "_" + date + ".pdf";
        }
        else
        {
            filename = "Disclaimer" + "_" + "Package" + "_" + date + ".pdf";
        }
        FileStream _FileStream = null;
        try
        {
            _FileStream = new FileStream(newTempFilePath,
                        System.IO.FileMode.Create,
                        System.IO.FileAccess.Write);
            _FileStream.Write(pdfBytes, 0, pdfBytes.Length);
            long length = new System.IO.FileInfo(newTempFilePath).Length;
            Int32 filesize = 0;
            bool result = Int32.TryParse(length.ToString(), out filesize);
            try
            {
                _FileStream.Close();
            }
            catch (Exception) { }
            LogInfo("Saving the disclaimer document");

            //Check whether use AWS S3, true if need to use
            if (aWSUseS3 == false)
            {
                //Move file to other location
                String pdfDocPathFileName = applicantFileLocation + destFileName;
                File.Copy(newTempFilePath, pdfDocPathFileName);
                newFinalFilePath = pdfDocPathFileName;

            }
            else
            {
                applicantFileLocation = ConfigurationManager.AppSettings["ApplicantFileLocation"];
                if (!applicantFileLocation.EndsWith("//"))
                {
                    applicantFileLocation += "//";
                }
                //AWS code to save document to S3 location
                AmazonS3Documents objAmazonS3 = new AmazonS3Documents();
                String destFolder = applicantFileLocation + "Tenant(" + TenantId.ToString() + @")/";
                String returnFilePath = objAmazonS3.SaveDocument(newTempFilePath, destFileName, destFolder);
                newFinalFilePath = returnFilePath; //Path.Combine(destFolder, destFileName);
            }
            try
            {
                if (!String.IsNullOrEmpty(newTempFilePath))
                    File.Delete(newTempFilePath);
            }
            catch (Exception) { }

            String documentTypeCode = INTSOF.Utils.DocumentType.DisclaimerDocument.GetStringValue();
            ApplicantDocument applicantDocument = _presenter.SaveDisclaimerAsPdf(TenantId, newFinalFilePath, filename, filesize, documentTypeCode, CurrentLoggedInUserID, OrgUsrID);

            if (applicantDocument.IsNotNull())
            {
                LogInfo(String.Format("Saved the disclaimer document {0} for applicant{1}", applicantDocument.ApplicantDocumentID, applicantDocument.OrganizationUserID));
                _applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
                if (_applicantOrderCart.IsNotNull())
                {
                    _applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
                    _applicantOrderCart.ApplicantDisclaimerDocumentId = applicantDocument.ApplicantDocumentID;
                    SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.APPLICANT_ORDER_CART, _applicantOrderCart);

                }
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            base.LogError(ex);
            return false;
        }
        finally
        {
            try { _FileStream.Close(); }
            catch (Exception) { }
        }
    }

    private void LoadESignedDisclaimerDocument()
    {
        ApplicantOrderCart applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
        if (applicantOrderCart.IsNotNull() && applicantOrderCart.ApplicantDisclaimerDocumentId != null)
        {
            iframePdfDocViewer.Src = String.Format("/ComplianceOperations/UserControl/DocumentViewer.aspx?tenantId={0}&documentId={1}&DocumentType={2}", TenantId, applicantOrderCart.ApplicantDisclaimerDocumentId, "ESignedDisclaimerDocument");
        }
        else
        {
            lblMessage.Text = "No disclaimer form to display";
        }

    }
    /// <summary>
    /// This method load the Esigned disclaimer document
    /// </summary>
    private void LoadDisclaimerDocument()
    {
        iframePdfDocViewer.Src = String.Format("/ComplianceOperations/UserControl/DocumentViewer.aspx?tenantId={0}&DocumentType={1}", TenantId, "DisclaimerDocument");
    }

    /// <summary>
    /// Redirect the user to dashboard, if applicant order cart is empty
    /// </summary>
    /// <param name="applicantOrder"></param>
    private void RedirectInvalidOrder(ApplicantOrderCart applicantOrderCart)
    {
        if (applicantOrderCart.IsNullOrEmpty())
        {
            Dictionary<String, String> queryString;
            queryString = new Dictionary<String, String>
                                                                 {
                                                                    { AppConsts.CHILD,  AppConsts.ADMIN_ENTRY_APPLICANT_LANDING_SCREEN},
                                                                    { "TenantId",CurrentViewContext.TenantId.ToString()},
                                                                    { "OrderId","0"}
                                                                 };
            Response.Redirect(String.Format("~/AdminEntryPortal/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
        }
    }

    /// <summary>
    /// Set the button Text for 'Previous', 'Next' or 'Restart' etc, based on the type of Order
    /// </summary>
    private void SetButtonText()
    {
        if (CurrentViewContext.OrderType == OrderRequestType.NewOrder.GetStringValue())
        {
            if (this.IsAcceptanceClicked)
                fsucCmdBar1.ClearButton.Value = AppConsts.NEXT_BUTTON_TEXT;

            fsucCmdBar1.ClearButtonText = AppConsts.NEXT_BUTTON_TEXT;
            fsucCmdBar1.ClearButtonIconClass = "rbNext";

            fsucCmdBar1.SubmitButtonText = AppConsts.PREVIOUS_BUTTON_TEXT;
        }
        else
        {
            if (this.IsAcceptanceClicked)
            {

                fsucCmdBar1.ClearButton.Value = AppConsts.PROCEED_BUTTON_TEXT;
            }
            else
                fsucCmdBar1.ClearButtonIconClass = "rbNext";
        }
    }

    #endregion

    #endregion

    #region SignatureToImage

    #region Private Variables
    private const string FONT_FAMILY = "Journal";
    #endregion

    #region Public Properties
    public Color PenColor { get; set; }
    public Color Background { get; set; }
    public int Height { get; set; }
    public int Width { get; set; }
    public int PenWidth { get; set; }
    public int FontSize { get; set; }
    #endregion

    /// <summary>
    /// Gets a new signature gernator with the default options.
    /// </summary>
    public AdminEntryPortal_UserControl_AdminEntryApplicantDisclaimer()
    {
        PenColor = Color.Black;
        Background = Color.White;
        Height = 150;
        Width = 648;
        PenWidth = 2;
        FontSize = 24;
    }

    /// <summary>
    /// Draws a signature based on the JSON provided by Signature Pad.
    /// </summary>
    /// <param name="json">JSON string of line drawing commands.</param>
    /// <returns>Bitmap image containing the user's signature.</returns>
    public Bitmap SigJsonToImage(string json)
    {
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
    /// Draws a signature using the journal font.
    /// </summary>
    /// <param name="name">User's name to create a signature for.</param>
    /// <param name="fontPath">Full path of journal.ttf. Should be passed if system doesn't have the font installed.</param>
    /// <returns>Bitmap image containing the user's signature.</returns>
    public Bitmap SigNameToImage(string name, string fontPath)
    {

        if (string.IsNullOrEmpty(fontPath) && !FontFamily.Families.Any(f => f.Name.Equals(FONT_FAMILY)))
        {
            throw new ArgumentException("FontPath must point to the copy of journal.ttf when the system does not have the font installed", "fontPath");
        }

        Bitmap signatureImage = new Bitmap(Width, Height);
        signatureImage.MakeTransparent();
        using (Graphics signatureGraphic = Graphics.FromImage(signatureImage))
        {
            signatureGraphic.Clear(Background);

            Font font;
            if (!string.IsNullOrEmpty(fontPath))
            {
                //to make sure the host doesn't need the font installed, use a private font collection
                PrivateFontCollection collection = new PrivateFontCollection();
                collection.AddFontFile(fontPath);
                font = new Font(collection.Families.First(), FontSize);
            }
            else
            {
                //fall back to the version installed on the host
                font = new Font(FONT_FAMILY, FontSize);
            }

            signatureGraphic.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            signatureGraphic.DrawString(name ?? string.Empty, font, new SolidBrush(PenColor), new PointF(0, 0));
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
