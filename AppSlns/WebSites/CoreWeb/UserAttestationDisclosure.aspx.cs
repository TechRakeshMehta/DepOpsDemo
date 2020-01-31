using CoreWeb.IntsofSecurityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTSOF.Utils;
using System.Web.Security;
using System.Drawing;
using System.Drawing.Drawing2D;
using Newtonsoft.Json;
using System.Drawing.Text;
using System.Configuration;
using System.IO;
using System.Web.Configuration;

namespace CoreWeb.Shell.Views
{
    public partial class UserAttestationDisclosure : BaseWebPage, IUserAttestationDisclosure
    {
        #region VARIABLES
        private UserAttestationDisclosurePresenter _presenter = new UserAttestationDisclosurePresenter();
        private const string FONT_FAMILY = "Journal";
        #endregion

        #region PROPERTIES

        #region PRIVATE PROPERTIES
        #region SIGNATURE PROPERTIES
        private Color PenColor { get; set; }
        private Color Background { get; set; }
        private int Height { get; set; }
        private int Width { get; set; }
        private int PenWidth { get; set; }
        private int FontSize { get; set; }
        #endregion
        #endregion

        #region PUBLIC PROPERTIES

        public UserAttestationDisclosurePresenter Presenter
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

        public Int32 TenantID
        {
            get
            {
                return Convert.ToInt32(ViewState["TenantID"]);
            }
            set
            {
                ViewState["TenantID"] = value;
            }
        }

        public Int32 OrganizationUserID
        {
            get
            {
                return Convert.ToInt32(ViewState["OrganizationUserID"]);
            }
            set
            {
                ViewState["OrganizationUserID"] = value;
            }
        }
        public String DocumentTypeCode
        {
            get
            {
                return Convert.ToString(ViewState["DocumentTypeCode"]);
            }
            set
            {
                ViewState["DocumentTypeCode"] = value;
            }
        }

        public Boolean NeedToShowEmploymentResultReport
        {
            get
            {
                return Convert.ToBoolean(ViewState["NeedToShowEmploymentResultReport"]);
            }
            set
            {
                ViewState["NeedToShowEmploymentResultReport"] = value;
            }
        }

        public byte[] PdfBuffer
        {
            get
            {
                return (byte[])ViewState["PdfBuffer"];
            }
            set
            {
                ViewState["PdfBuffer"] = value;
            }
        }

        public Entity.UserAttestationDetail UserAttestationDetails
        {
            get
            {
                return (Entity.UserAttestationDetail)ViewState["UserAttestationDetails"];
            }
            set
            {
                ViewState["UserAttestationDetails"] = value;
            }
        }

        public byte[] MergedSignedDocumentBuffer
        {
            get
            {
                return (byte[])ViewState["MergedSignedDocumentBuffer"];
            }
            set
            {
                ViewState["MergedSignedDocumentBuffer"] = value;
            }
        }

        public List<Entity.OrganizationUserTypeMapping> OrganizationUserTypeMapping { get; set; }

        public Guid UserId
        {
            get
            {
                return new Guid(SysXWebSiteUtils.SessionService.UserId);
            }
        }     

        #endregion

        #endregion

        #region EVENTS

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region INITIALIZE SIGNATURE PROPERTIES
            PenColor = Color.Black;
            Background = Color.White;
            Height = 150;
            Width = 648;
            PenWidth = 2;
            FontSize = 24;
            #endregion

            // get members user
            if (!IsPostBack)
            {
                SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;

                if (user.IsNotNull())
                {
                    Dictionary<String, String> args = new Dictionary<String, String>();
                    if (!Request.QueryString["args"].IsNull())
                    {
                        args.ToDecryptedQueryString(Request.QueryString["args"]);
                        if (args.ContainsKey("TenantID"))
                            TenantID = Convert.ToInt32(args["TenantID"]);
                        if (args.ContainsKey("OrganizationUserID"))
                            OrganizationUserID = Convert.ToInt32(args["OrganizationUserID"]);
                        if (args.ContainsKey("DocumentTypeCode"))
                            DocumentTypeCode = Convert.ToString(args["DocumentTypeCode"]);
                        if (args.ContainsKey("NeedToShowEmploymentResultReport"))
                            NeedToShowEmploymentResultReport = Convert.ToBoolean(args["NeedToShowEmploymentResultReport"]);                      

                        //partially Fill User Attestation Document with pre-required data and load into iframe
                        if (UserAttestationDetails.IsNullOrEmpty())
                        {
                            UserAttestationDetails = Presenter.FillUserAttestationDocumentWithPrePopulatedData();
                            LoadUserAttestationDocument(AppConsts.UAF_PARTIALLY_FILLED_MODE);
                        }
                    }
                }
                else
                {
                    //Redirecting to login if session is null
                    RedirectToLogin();
                }
            }
        }



        /// <summary>
        /// Submit Button Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            //1.Save Filled Attestation Form to DB
            //2.Check whether need to open ED Form or not if client logged-in
            //3. if 2. yes - Redirect to ED Form page
            //4. if 2. No - Redirect to Business Channel Page
            //5. Redirect to Manage Invitation if shared user logged in 

            var signatureBuffer = GetSignatureImageBuffer(hiddenOutput.Value);
            if (signatureBuffer.IsNotNull() && signatureBuffer.Length < Convert.ToInt32(WebConfigurationManager.AppSettings["MinApplicantSignLength"]))
            {
                //signatureDiv.Style.Add("display", "block");
                //signature.Attributes.Remove("disabled");
                dvSignQualifyError.Visible = true;
                return;
            }
            else
            {
                dvSignQualifyError.Visible = false;
            }

            if (btnSubmit.Value == "Accept")
            {
                if (CreateAndStoreESignature())
                {
                    btnSubmit.Value = "Proceed";
                    signatureDiv.Visible = false;
                    signatureDiv.Disabled = true;
                    chkAccept.Visible = false;
                    chkAccept.Enabled = false;
                    SaveFullyFilledAttestationDocument(AppConsts.UAF_PREVIEW_MODE);
                    LoadUserAttestationDocument(AppConsts.UAF_PREVIEW_MODE);
                }
            }

            //Make Entry in DB and Redirect to Select Business Channel if doc is submitted successfully with e-sign
            else if (btnSubmit.Value == "Proceed")
            {
                if (SaveFullyFilledAttestationDocument(AppConsts.UAF_FULLY_FILLED_MODE))
                {
                    //2 -3.
                    if (DocumentTypeCode == DislkpDocumentType.USER_ATTESTATION_DISCLOSURE_FORM_CLIENT_ADMIN.GetStringValue())
                    {
                        if (NeedToShowEmploymentResultReport && Presenter.CheckEmploymentNodePermission() && !Presenter.IsEDFormPreviouslyAccepted(OrganizationUserID))
                        {
                            //UAT-1741: 604 notification should only have to be clicked upon login once per 24 hours 
                            //Check EmploymentDisclosureDetails for 24H.

                            Dictionary<String, String> queryString = new Dictionary<String, String>();
                            queryString = new Dictionary<String, String>
                                                                 { 
                                                                    {"DocumentTypeCode", DislkpDocumentType.EMPLOYMENT_DISCLOSURE_FORM.GetStringValue()},
                                                                    {"TenantID", Convert.ToString(TenantID)},
                                                                    {"OrganizationUserID", Convert.ToString(OrganizationUserID)}
                                                                 };
                            String url = String.Format(AppConsts.EMPLOYMENT_DISCLOSURE + "?args={0}", queryString.ToEncryptedQueryString());
                            Response.Redirect(url);
                        }
                        //2 -4.
                        else
                        {
                            SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                            SysXWebSiteUtils.SessionService.SetSysXMembershipUser(user);
                            Int32 TenantId = user.TenantId.HasValue ? user.TenantId.Value : AppConsts.NONE;
                            if (Presenter.GetLineOfBusinessesByUser(Convert.ToString(user.UserId), TenantId) > AppConsts.ONE)
                            {
                                Response.Redirect(AppConsts.SELECT_BUSINESS_CHANNEL_URL);
                            }
                            else
                            {                               
                                FormsAuthentication.RedirectFromLoginPage(user.UserName, false);
                            }
                         //   Response.Redirect(AppConsts.SELECT_BUSINESS_CHANNEL_URL);
                        }
                    }
                    else if (DocumentTypeCode == DislkpDocumentType.USER_ATTESTATION_DISCLOSURE_FORM_SHARED_USER.GetStringValue())
                    {
                        Presenter.GetOrganizationUserTypeMapping();

                        if (!OrganizationUserTypeMapping.IsNullOrEmpty())
                        {
                            //String inst_code = OrganizationUserType.Instructor.GetStringValue();
                            //String precpt_code = OrganizationUserType.Preceptor.GetStringValue();
                            //String agency_code = OrganizationUserType.AgencyUser.GetStringValue();
                            //String appsharedusr_code = OrganizationUserType.ApplicantsSharedUser.GetStringValue();

                            Boolean checkIfInstructor = OrganizationUserTypeMapping.Any(x => x.lkpOrgUserType.OrgUserTypeCode.Contains(OrganizationUserType.Instructor.GetStringValue()));
                            Boolean checkIfPreceptor = OrganizationUserTypeMapping.Any(x => x.lkpOrgUserType.OrgUserTypeCode.Contains(OrganizationUserType.Preceptor.GetStringValue()));
                            Boolean checkIfAgencyUser = OrganizationUserTypeMapping.Any(x => x.lkpOrgUserType.OrgUserTypeCode.Contains(OrganizationUserType.AgencyUser.GetStringValue()));
                            Boolean checkIfApplicantsSharedUser = OrganizationUserTypeMapping.Any(x => x.lkpOrgUserType.OrgUserTypeCode.Contains(OrganizationUserType.ApplicantsSharedUser.GetStringValue()));

                            Dictionary<String, String> queryString = new Dictionary<String, String>();
                            if (checkIfAgencyUser)
                            {
                                //Agency User
                                queryString = new Dictionary<String, String>
                                                                     { 
                                                                        { "Child", ChildControls.SharedUserDashboard}
                                                                     };
                                Response.Redirect(String.Format("~/ProfileSharing/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));

                            }
                            else if (checkIfInstructor || checkIfPreceptor)
                            {
                                //InstructorPreceptor
                                queryString = new Dictionary<String, String>
                                                                     { 
                                                                        { "Child", AppConsts.INSTRUCTOR_PRESEPTOR_DASHBOARD}
                                                                     };
                                Response.Redirect(String.Format("~/ProfileSharing/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
                            }
                            else
                            {
                                queryString = new Dictionary<String, String>
                                                                     { 
                                                                        { "Child", ChildControls.SharedUserDashboard}
                                                                     };
                                Response.Redirect(String.Format("~/ProfileSharing/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
                            }
                        }
                    }
                    else
                    {
                        base.ShowErrorMessage("Some error occured. Please try again or contact system administrator.");
                    }
                }
                else
                {
                    base.ShowErrorMessage("An error occured while saving User Attestation Disclosure Form. Please try again or contact system administrator.");
                }
            }
            else
            {
                base.ShowErrorMessage("Some error occured. Please try again or contact system administrator.");
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
        /// Cancel Button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>k
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (!UserAttestationDetails.IsNullOrEmpty() && !String.IsNullOrEmpty(UserAttestationDetails.UAD_DocumentPath))
                File.Delete(UserAttestationDetails.UAD_DocumentPath);
            RedirectToLogin();
        }

        #endregion

        #region PRIVATE METHODS

        /// <summary>
        /// Method to Redirect the User to login page
        /// </summary>
        private void RedirectToLogin()
        {
            SysXWebSiteUtils.SessionService.ClearSession(true);
            Response.Redirect(FormsAuthentication.LoginUrl);
        }

        /// <summary>
        /// Draws a signature based on the JSON provided by Signature Pad.
        /// </summary>
        /// <param name="json">JSON string of line drawing commands.</param>
        /// <returns>Bitmap image containing the user's signature.</returns>
        private Bitmap SigJsonToImage(string json)
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
        private Bitmap SigNameToImage(string name, string fontPath)
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
        /// This method read the signature and fill into the pdf 
        /// </summary>
        /// <returns></returns>
        private bool CreateAndStoreESignature()
        {
            Boolean aWSUseS3 = false;
            if (!ConfigurationManager.AppSettings["AWSUseS3"].IsNullOrEmpty())
            {
                aWSUseS3 = Convert.ToBoolean(ConfigurationManager.AppSettings["AWSUseS3"]);
            }

            if (UserAttestationDetails.UAD_DocumentPath != null && UserAttestationDetails.UAD_IsActive == false)
            {
                bool status = false;
                try
                {
                    byte[] buffer = null;
                    //byte[] mergedSignedDocumentBuffer = null;
                    byte[] bufferSignature = null;
                    FileStream _fileStream = null;
                    long _totalBytes = 0;


                    if (File.Exists(UserAttestationDetails.UAD_DocumentPath))
                    {
                        _fileStream = new FileStream(UserAttestationDetails.UAD_DocumentPath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                        _totalBytes = new System.IO.FileInfo(UserAttestationDetails.UAD_DocumentPath).Length;
                        BinaryReader _binaryReader = new BinaryReader(_fileStream);
                        //read entire file into buffer
                        buffer = _binaryReader.ReadBytes((Int32)_totalBytes);
                        _fileStream.Close();
                        File.Delete(UserAttestationDetails.UAD_DocumentPath); //Deleting the partially filled doc from temp file location after getting into buffer.
                    }
                    else
                    {
                        base.ShowErrorInfoMessage("No disclosure form to display");
                        btnSubmit.Enabled = false;
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

                    MergedSignedDocumentBuffer = _presenter.InsertSignatureToAttestationDocument(buffer, bufferSignature);
                    status = true;
                }
                catch (Exception ex)
                {
                    btnSubmit.Enabled = false;
                    base.LogError(ex);
                    base.ShowErrorMessage(ex.Message);
                }
                return status;
            }
            else
            {
                base.ShowErrorInfoMessage("No disclosure form to display");
                return false;
            }
        }

        private bool SaveFullyFilledAttestationDocument(String documentStage)
        {
            Boolean aWSUseS3 = false;
            if (!ConfigurationManager.AppSettings["AWSUseS3"].IsNullOrEmpty())
            {
                aWSUseS3 = Convert.ToBoolean(ConfigurationManager.AppSettings["AWSUseS3"]);
            }
            Boolean status = false;
            if (MergedSignedDocumentBuffer != null)
            {
                //saving final document 
                if (Presenter.SaveFullyFilledAttestationDocument(MergedSignedDocumentBuffer, aWSUseS3, documentStage))
                {
                    status = true;
                }
            }
            return status;
        }

        /// <summary>
        /// Method to load document into iframe
        /// </summary>
        private void LoadUserAttestationDocument(String documentStage)
        {
            iframeAttestDisclosure.Src = String.Format("/ComplianceOperations/UserControl/DocumentViewer.aspx?UserAttestationDocumentID={0}&OrganizationUserID={1}&UAFDocumentStage={2}", UserAttestationDetails.UAD_ID, OrganizationUserID, documentStage);
        }
        #endregion

    }

    #region SIGNATURELINE CLASS
    /// <summary>
    /// Line drawing commands as generated by the Signature Pad JSON export option.
    /// </summary>
    class SignatureLine
    {
        public int lx { get; set; }
        public int ly { get; set; }
        public int mx { get; set; }
        public int my { get; set; }
    }
    #endregion
}