using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;

namespace CoreWeb.AdminEntryPortal.Views
{
    public partial class AdminEntryApplicantRequiredDocumentation : BaseUserControl, IAdminEntryApplicantRequiredDocumentaionView
    {
        #region VARIABLES

        #region Public Variables

        #endregion

        #region Private Variables

        private AdminEntryApplicantRequiredDocumentationPresenter _presenter = new AdminEntryApplicantRequiredDocumentationPresenter();
        private String _viewType;
        private Int32 _tenantId;
        private ApplicantOrderCart _applicantOrderCart;

        #endregion

        #endregion

        #region PROPERTIES

        #region Public Properties

        public AdminEntryApplicantRequiredDocumentationPresenter Presenter
        {
            get
            {
                this._presenter.View = this;
                return this._presenter;
            }

        }

        public IAdminEntryApplicantRequiredDocumentaionView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public Int32 TenantID
        {
            get
            {
                if (_tenantId == 0)
                {
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    if (user.IsNotNull())
                    {
                        _tenantId = user.TenantId.HasValue ? user.TenantId.Value : 0;
                    }
                }
                return _tenantId;
            }
            set
            {
                _tenantId = value;
            }
        }
        public Int32 SystemDocumentID
        {
            get;
            set;
        }

        public Int32 DPP_ID
        {
            get;
            set;
        }
        public Boolean SystemDocumentIsDeleted
        {
            get;
            set;
        }
        public Int32 CurrentLoggedInUserID
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }

        }

        public String DocumentPath
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

        #endregion

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


        /// <summary>
        /// Used to identify whether the User has clicked on the Accept button
        /// </summary>
        private Boolean IsQAEnv
        {
            get
            {
                if (!ConfigurationManager.AppSettings["RestrictOrderFlow"].IsNullOrEmpty())
                {
                    ViewState["IsQAEnv"] = Convert.ToBoolean(ConfigurationManager.AppSettings["RestrictOrderFlow"]);
                    return Convert.ToBoolean(ViewState["IsQAEnv"]);
                }
                return false;
            }
            set
            {
                ViewState["IsQAEnv"] = value;
            }
        }
        #endregion

        #endregion

        #region EVENTS
        protected void Page_PreRender(object sender, EventArgs e)
        {
            try
            {
                if (ucRequiredDocumentationLoader.RestrictOrderFlow)
                {
                    base.ShowInfoMessage(AppConsts.NO_DISCLOSURE_MAPPED_INFO_MSG);
                    fsucCmdBar1.ClearButton.Enabled = false;
                    return;
                }

                if (IsQAEnv)
                {
                    if (ucRequiredDocumentationLoader.IsErrorOccured)
                    {
                        base.ShowErrorMessage(AppConsts.TECHNICAL_ERROR_MSG_DISCLOSURE_AUTHORIZATION);
                        fsucCmdBar1.ClearButton.Enabled = false;
                        return;
                    }
                }

                String locationField = ucRequiredDocumentationLoader.LstSpecialFields.Where(x => x.SpecialFieldTypeCode == "AAAC").Select(x => x.SpecialFieldTypeCode).FirstOrDefault();
                if (locationField.IsNotNull())
                {
                    dvLocation.Visible = true;
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

        #region Page Load

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //hiddenOutput.Value = "111";
                ApplicantOrderCart applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
                RedirectInvalidOrder(applicantOrderCart);
                CurrentViewContext.OrderType = applicantOrderCart.OrderRequestType;
                String _currentStep = " (Step " + (applicantOrderCart.lstApplicantOrder[0].PreviousOrderStep) +
                                 " of " + applicantOrderCart.GetTotalOrderSteps() + ")";

                base.SetPageTitle(_currentStep);

                if (applicantOrderCart.IsNotNull())
                {
                    CurrentViewContext.DPP_ID = applicantOrderCart.DPP_Id.HasValue ? applicantOrderCart.DPP_Id.Value : 0;
                }

                if (!this.IsPostBack)
                {
                    Presenter.OnViewInitialized();
                    RedirectIfIncorrectOrderStage(applicantOrderCart);
                    divPdfDocViewer.Visible = false;
                    chkAccept.Visible = true;
                }
                Presenter.OnViewLoaded();

                //if (!applicantOrderCart.IsNullOrEmpty() &&
                //(applicantOrderCart.OrderRequestType == OrderRequestType.NewOrder.GetStringValue()
                //|| applicantOrderCart.OrderRequestType == OrderRequestType.ChangeSubscription.GetStringValue()))
                //    (this.Page as CoreWeb.AdminEntryPortal.Views.Default).SetModuleTitle("Create Order");
                //else
                (this.Page as CoreWeb.AdminEntryPortal.Views.Default).SetModuleTitle("Complete Your Order");
                SetButtonText();
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
                //Server check to ensure that user must have signed the disclosure document
                if (!hiddenOutput.Value.IsNullOrEmpty() && chkAccept.Checked)
                {
                    lblValidationMsg.Visible = false;
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
                            SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.APPLICANT_ORDER_CART, applicantOrderCart);

                            SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.REQUIRED_DOCUMENTATION_ACCEPTED, true);
                            Dictionary<String, String> queryString = new Dictionary<String, String>();
                            queryString = new Dictionary<String, String>
                                                         {
                                                            { AppConsts.CHILD,  ChildControls.AdminEntryApplicantOrderReview}
                                                         };
                            Response.Redirect(String.Format("~/AdminEntryPortal/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
                        }

                    }


                    List<SysDocumentFieldMappingContract> lstDocsAttributeMapping = ucRequiredDocumentationLoader.AdditionalDocumentList;
                    List<SysDocumentFieldMappingContract> LstSpecialFields = ucRequiredDocumentationLoader.LstSpecialFields;
                    foreach (var field in LstSpecialFields)
                    {
                        if (field.SpecialFieldTypeCode == "AAAC")
                            field.FieldValue = txtLocation.Text;
                    }
                    foreach (var item in lstDocsAttributeMapping)
                    {
                        DocumentPath = item.DocumentPath;
                        CreateAndStoreESignature(item, LstSpecialFields.Where(cond => cond.ID == item.SystemDocOldID.Value).ToList());
                    }

                    //UAT-3745
                    if (_applicantOrderCart.lstSystemDocBkgSvcMapping.IsNullOrEmpty())
                        _applicantOrderCart.lstSystemDocBkgSvcMapping = new List<SystemDocBkgSvcMapping>();
                    if (!lstDocsAttributeMapping.IsNullOrEmpty())
                    {
                        var _lstSystemDocBkgSvcMapping = lstDocsAttributeMapping.Where(cnd => !cnd.SystemDocBkgSvcMapping.IsNullOrEmpty()
                                                && cnd.SystemDocBkgSvcMapping.SystemDocumentID > AppConsts.NONE).Select(sel => sel.SystemDocBkgSvcMapping).ToList();
                        _applicantOrderCart.lstSystemDocBkgSvcMapping.AddRange(_lstSystemDocBkgSvcMapping);

                        //foreach (SysDocumentFieldMappingContract item in lstDocsAttributeMapping)
                        //{
                        //    if (!item.SystemDocBkgSvcMapping.IsNullOrEmpty() && item.SystemDocBkgSvcMapping.SystemDocumentID > AppConsts.NONE)
                        //        _applicantOrderCart.lstSystemDocBkgSvcMapping.Add(item.SystemDocBkgSvcMapping);
                        //}
                    }

                    //End


                    lblMessage.Text = "";
                    // Comment for UAT-1560
                    //LoadEsignedDisclosureDocument();
                    ucRequiredDocumentationLoader.LoadSignedPdfs = true;
                    SetButtonText();

                    signatureDiv.Style.Add("display", "none");
                    chkAccept.Enabled = false;
                    signature.Attributes.Add("disabled", "disabled");
                    lblErrorSig.Visible = false;
                    lblErrorChkBox.Visible = false;
                    lblSignMessage.Text = "(Scroll down to continue)";
                }
                else
                {
                    if (hiddenOutput.Value.IsNullOrEmpty())
                    {
                        signatureDiv.Style.Add("display", "block");
                        signature.Attributes.Remove("disabled");
                        lblValidationMsg.Visible = true;
                        lblValidationMsg.Text = "Please sign the required documentation form(s).";
                    }
                    else
                    {
                        lblValidationMsg.Visible = true;
                        lblValidationMsg.Enabled = true;
                        lblValidationMsg.Text = "Please agree and accept required documentation form(s).";
                    }
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                if (IsQAEnv)
                {
                    fsucCmdBar1.ClearButton.Enabled = false;
                    base.ShowErrorMessage(AppConsts.TECHNICAL_ERROR_MSG_DISCLOSURE_AUTHORIZATION);
                }
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                if (IsQAEnv)
                {
                    fsucCmdBar1.ClearButton.Enabled = false;
                    base.ShowErrorMessage(AppConsts.TECHNICAL_ERROR_MSG_DISCLOSURE_AUTHORIZATION);
                }
            }
        }

        /// <summary>
        /// Event for Backward navigation i.e. Previous or Restart Order button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucCmdBar1_ExtraClick(object sender, EventArgs e)
        {
            ApplicantOrderCart applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
            Dictionary<String, String> queryString;
            //if (CurrentViewContext.OrderType == OrderRequestType.RenewalOrder.GetStringValue())
            //{

            //    Session.Remove(ResourceConst.APPLICANT_ORDER_CART);
            //    Session.Remove(AppConsts.REQUIRED_DOCUMENTATION_ACCEPTED);
            //    queryString = new Dictionary<String, String>()
            //                                             {
            //                                                {"OrderId",applicantOrderCart.PrevOrderId.ToString()},
            //                                                { "Child",  ChildControls.RenewalOrder}
            //                                             };
            //    Response.Redirect(String.Format("Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
            //}
            if (CurrentViewContext.OrderType == OrderRequestType.NewOrder.GetStringValue())
            {
                // To avoid Redirection again by the browser back button navigation check method
                //applicantOrderCart.AddOrderStageTrackID(OrderStages.Disclaimer);
                //applicantOrderCart.DecrementOrderStepCount();
                //queryString = new Dictionary<String, String>()
                //                                         { 
                //                                            { "Child",  ChildControls.ApplicantDisclaimerPage}
                //                                         };
                //Response.Redirect(String.Format("Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
                RedirectToDisclosureOrDisclaimer(applicantOrderCart);

            }
            else
            {
                //   Session.Remove(ResourceConst.APPLICANT_ORDER_CART);
                applicantOrderCart.ClearOrderCart(applicantOrderCart);
                Session.Remove(AppConsts.REQUIRED_DOCUMENTATION_ACCEPTED);
                Session.Remove(ResourceConst.APPLICANT_DRUG_SCREENING);
                queryString = new Dictionary<String, String>
                                                                 {
                                                                    { AppConsts.CHILD,  AppConsts.ADMIN_ENTRY_APPLICANT_LANDING_SCREEN},
                                                                    { "TenantId",applicantOrderCart.TenantId.ToString()},
                                                                    { "OrderId",applicantOrderCart.OrderId.ToString()}
                                                                 };
                Response.Redirect(String.Format("~/AdminEntryPortal/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));

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
                Session.Remove(AppConsts.REQUIRED_DOCUMENTATION_ACCEPTED);

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

        #region METHODS
        /// <summary>
        /// Set the sorce of iframe with disclosure form document viewer.
        /// </summary>
        private void LoadAdditionalDocument()
        {

            if (_presenter.IsDisclosureDocumentRequired())
            {
                ApplicantOrderCart applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
                if (applicantOrderCart != null)
                {
                    if (applicantOrderCart.IsCompliancePackageSelected)
                    {
                        fsucCmdBar1.SubmitButton.Enabled = false;
                    }
                    //Applicant select AMS package than disclosure document required check is skip by applicant in order flow.
                    else
                    {
                        fsucCmdBar1.SubmitButton.Enabled = true;
                    }
                }
                else
                {
                    fsucCmdBar1.SubmitButton.Enabled = false;
                }
            }
            else
            {
                fsucCmdBar1.SubmitButton.Enabled = true;
            }
            lblMessage.Text = "No disclosure form to display";

        }

        /// <summary>
        /// Checks the order status track. If this page is not opened as per correct order status track then, redirected to correct order.
        /// </summary>
        /// <param name="applicantOrderCart"></param>
        private void RedirectIfIncorrectOrderStage(ApplicantOrderCart applicantOrderCart)
        {
            String _nextPagePath = Presenter.GetNextPagePathByOrderStageID(applicantOrderCart);

            //Redirect to next page path if Order Status track is not correct.
            if (_nextPagePath.IsNotNull())
            {
                Response.Redirect(_nextPagePath);
            }
            else
            {
                applicantOrderCart.AddOrderStageTrackID(OrderStages.RequiredDocumentation);
            }
        }

        /// <summary>
        /// This method read the signature and fill into the pdf
        /// </summary>
        /// <returns></returns>
        private bool CreateAndStoreESignature(SysDocumentFieldMappingContract documentInfo, List<SysDocumentFieldMappingContract> LstSpecialFields = null)
        {
            Boolean aWSUseS3 = false;
            var tempDocPath = String.Empty;
            if (!Session["tmpDocPaths"].IsNullOrEmpty())
            {
                List<String> lsttempDocPaths = new List<string>();
                lsttempDocPaths = (List<String>)Session["tmpDocPaths"];
                var nonEmptyList = lsttempDocPaths.Where(x => !x.IsNullOrEmpty()).ToList();
                var firstNonEmptyValue = nonEmptyList.FirstOrDefault();
                tempDocPath = firstNonEmptyValue;
                lsttempDocPaths.Remove(firstNonEmptyValue);
                Session["tmpDocPaths"] = lsttempDocPaths;
                if (lsttempDocPaths.IsNullOrEmpty())
                {
                    Session["tmpDocPaths"] = null;
                }
                //firstNonEmptyValue = null;

            }
            if (!ConfigurationManager.AppSettings["AWSUseS3"].IsNullOrEmpty())
            {
                aWSUseS3 = Convert.ToBoolean(ConfigurationManager.AppSettings["AWSUseS3"]);
            }

            if (documentInfo.DocumentPath != null)
            {
                bool status = false;
                try
                {
                    byte[] buffer = null;
                    byte[] mergedSignedDocumentBuffer = null;
                    byte[] bufferSignature = null;


                    FileStream _fileStream = null;
                    if (File.Exists(tempDocPath))
                    {
                        _fileStream = new FileStream(tempDocPath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                        var _totalBytes = new System.IO.FileInfo(tempDocPath).Length;
                        BinaryReader _binaryReader = new BinaryReader(_fileStream);
                        //read entire file into buffer
                        buffer = _binaryReader.ReadBytes((Int32)_totalBytes);
                        _fileStream.Close();
                    }
                    if (buffer.IsNullOrEmpty())
                    {
                        base.LogInfo("File not found" + DocumentPath);
                        lblMessage.Text = "No disclosure form to display.";
                        return false;
                    }

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

                    LogInfo("Filling the E-signature in the disclaimer pdf.");


                    mergedSignedDocumentBuffer = _presenter.FillSignatureInDisclosurePDFDocument(buffer, bufferSignature, LstSpecialFields);
                    LogInfo("Filled the E-signature in the disclaimer pdf.");

                    if (mergedSignedDocumentBuffer != null)
                    {
                        if (UpdateDisclosurerPdfWithEsignature(mergedSignedDocumentBuffer, aWSUseS3, documentInfo))
                        {
                            status = true;
                        }
                    }
                    try
                    {
                        if (File.Exists(tempDocPath))
                            File.Delete(tempDocPath);
                    }
                    catch (Exception) { }
                    Session["IsSignatureStored"] = "true";

                }
                catch (Exception ex)
                {
                    base.LogError(ex);
                    throw ex;
                }
                return status;
            }
            else
            {
                lblMessage.Text = "No disclosure form to display";
                return false;
            }
        }

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
        /// Save the ESigned disclosre document 
        /// </summary>
        /// <param name="pdfBytes">pdfBytes</param>
        /// <returns>Boolean</returns>
        private Boolean UpdateDisclosurerPdfWithEsignature(byte[] pdfBytes, Boolean aWSUseS3, SysDocumentFieldMappingContract documentInfo)
        {
            String tempFilePath = ConfigurationManager.AppSettings["TemporaryFileLocation"];
            String applicantFileLocation = String.Empty;
            String filename = String.Empty;

            if (tempFilePath.IsNullOrEmpty())
            {
                base.LogError("Please provide path for TemporaryFileLocation in config.", new SystemException());
                throw new SystemException("Please provide path for TemporaryFileLocation in config.");
            }
            if (!tempFilePath.EndsWith(@"\"))
            {
                tempFilePath += @"\";
            }
            tempFilePath += "Tenant(" + TenantID.ToString() + @")\";

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
                applicantFileLocation += "Tenant(" + TenantID.ToString() + @")\";

                if (!Directory.Exists(applicantFileLocation))
                {
                    Directory.CreateDirectory(applicantFileLocation);
                }
            }
            String newFileName = Path.GetFileName(documentInfo.DocumentPath);

            String newTempFilePath = Path.Combine(tempFilePath, newFileName);
            String newFinalFilePath = String.Empty;

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
                catch (Exception ex)
                {
                    base.LogError(ex);
                    throw ex;
                }
                LogInfo("Saving the required documentation document");

                CommonFileManager.DeleteDocument(documentInfo.DocumentPath, FileType.ApplicantFileLocation.GetStringValue());
                String destFolder = "Tenant(" + TenantID.ToString() + @")\" + newFileName;
                CommonFileManager.SaveDocument(newTempFilePath, destFolder, FileType.ApplicantFileLocation.GetStringValue());
                try
                {
                    if (!String.IsNullOrEmpty(newTempFilePath))
                        File.Delete(newTempFilePath);
                }
                catch (Exception ex)
                {
                    base.LogError(ex);
                    throw ex;
                }

                _applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
                if (_applicantOrderCart.IsNotNull())
                {
                    if (_applicantOrderCart.ApplicantAdditionalDocumentIds.IsNullOrEmpty())
                        _applicantOrderCart.ApplicantAdditionalDocumentIds = new List<int?>();
                    _applicantOrderCart.ApplicantAdditionalDocumentIds.Add(documentInfo.ID);


                    //UAT-1759: Create the ability to mark an "Additional Documents" that the students complete in the order flow as "Send to student"
                    if (documentInfo.SendToStudent.IsNotNull() && documentInfo.SendToStudent == true)
                    {
                        if (_applicantOrderCart.AdditionalDocSendToStudent.IsNullOrEmpty())
                            _applicantOrderCart.AdditionalDocSendToStudent = new List<int?>();
                        _applicantOrderCart.AdditionalDocSendToStudent.Add(documentInfo.ID);
                    }
                    SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.APPLICANT_ORDER_CART, _applicantOrderCart);
                }
                return true;
            }
            catch (Exception ex)
            {
                if (IsQAEnv)
                {
                    fsucCmdBar1.ClearButton.Enabled = false;
                }

                base.LogError(ex);
                throw ex;
            }
            finally
            {
                try { _FileStream.Close(); }
                catch (Exception ex)
                {
                    base.LogError(ex);
                    throw ex;
                }
            }
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
                                                                    { "TenantId",CurrentViewContext.TenantID.ToString()},
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
                    fsucCmdBar1.ClearButton.Text = AppConsts.PROCEED_BUTTON_TEXT;

                }
                fsucCmdBar1.ClearButtonIconClass = "rbOk";
            }
        }

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
        public AdminEntryApplicantRequiredDocumentation()
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
            //we need a reference to the font, be it the .tff in the site project or the version installed on the host
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

        private void RedirectToDisclosureOrDisclaimer(ApplicantOrderCart applicantOrderCart)
        {
            //UAT-2862: Always return to Disclaimer page.
            String _childControl = String.Empty;
            applicantOrderCart.DecrementOrderStepCount();

            // To avoid Redirection again by the browser back button navigation check method
            applicantOrderCart.AddOrderStageTrackID(OrderStages.Disclosure); //UAT-5184

            _childControl = ChildControls.AdminEntryApplicantDisclosurePage; //UAT-5184

            Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 {
                                                                    { AppConsts.CHILD,_childControl  }
                                                                 };
            Response.Redirect(String.Format("~/AdminEntryPortal/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
        }
        #endregion
    }
}