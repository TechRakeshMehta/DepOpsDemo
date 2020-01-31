using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTSOF.AuthNet.Business;
using INTSOF.AuthNet.Business.CustomerProfileWS;
using Telerik.Web.UI;
using INTSOF.Utils;
using CoreWeb.Shell;
using INTSOF.UI.Contract.ComplianceOperation;
using Entity.ClientEntity;
using System.Text;
using CoreWeb.IntsofSecurityModel;
using System.Configuration;
using System.Collections.Specialized;
using System.Web.Services;
using System.IO;
using Business.RepoManagers;

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class ItemPaymentConfirmationPopUp : BaseWebPage, IItemPaymentConfirmationView
    {
        #region Variables

        #region Private Variables

        private ItemPaymentConfirmationPresenter _presenter = new ItemPaymentConfirmationPresenter();
        private OrganizationUserProfile _orgUserProfile;

        OrganizationUserProfile _organizationUserProfile;

        private Guid LCNAttCode = new Guid("515BEF57-9072-4D2A-A97A-0C248BB045F9");////License Number
        private Guid MotherNameAttrCode = new Guid("3DA8912A-6337-4B8F-93C4-88BFC3032D2D");////Mother's Maiden Name
        private Guid IdentificationNumberAttrCode = new Guid("AAB51E52-2A9B-42AB-9A9D-D1AFFC18E211");////Identification Number

        #endregion

        #region Public Variables

        #endregion
        #endregion


        #region Properties
        #region Public
        public Entity.ZipCode ApplicantZipCodeDetails
        {
            get;
            set;
        }

        public ItemPaymentConfirmationPresenter Presenter
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

        public Int32 TenantId
        {
            get
            {
                if (ViewState["TenantId"] == null)
                {
                    //Get User from Session
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    ViewState["TenantId"] = user.TenantId.HasValue ? user.TenantId.Value : 0;
                    return Convert.ToInt32(ViewState["TenantId"]);
                }
                else
                {
                    return Convert.ToInt32(ViewState["TenantId"]);
                }
            }

            set
            {
                ViewState["TenantId"] = value;
            }
        }

        public Int32 ZipCodeId
        {
            get;
            set;
        }

        public IItemPaymentConfirmationView CurrentViewContext
        {
            get { return this; }
        }
        public Int32 CurrentLoggedInUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }
        public String Gender
        {
            get;
            set;
        }

        public Int32 GenderId
        {
            get;
            set;
        }
        List<PkgPaymentOptions> IItemPaymentConfirmationView.lstPaymentOptions
        {
            get;
            set;
        }
        String IItemPaymentConfirmationView.PaymentModeCode
        {
            get;
            set;
        }
        String IItemPaymentConfirmationView.PaymentModeDisplayName
        {
            get;
            set;
        }

        String IItemPaymentConfirmationView.InstructionText
        {
            get;
            set;
        }

        /// <summary>
        /// Id for the Credit Card Payment Mode
        /// </summary>
        Int32 IItemPaymentConfirmationView.PaymentModeId
        {
            get
            {
                return Convert.ToInt32(ViewState["CCId"]);
            }
            set
            {
                ViewState["CCId"] = value;
            }
        }
        public Int32 OrgUsrID
        {
            get
            {
                if (!System.Web.HttpContext.Current.Session["AppViewAdminOrgUsrID"].IsNullOrEmpty())
                    return Convert.ToInt32(System.Web.HttpContext.Current.Session["AppViewAdminOrgUsrID"]);
                else
                    return CurrentLoggedInUserId;
            }
        }

        Boolean IItemPaymentConfirmationView.IsSSNDisabled
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsSSNDisabled"] ?? false);
            }
            set
            {
                ViewState["IsSSNDisabled"] = value;
            }
        }

        /// <summary>
        /// List of Instructions to bind 
        /// </summary>
        public List<Tuple<String, String>> lstClientPaymentOptns
        {
            get;
            set;
        }

        String IItemPaymentConfirmationView.DecryptedSSN { get; set; }

        Boolean IItemPaymentConfirmationView.IsInstructorPreceptorPackage
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsInstructorPreceptorPackage"] ?? false);
            }
            set
            {
                ViewState["IsInstructorPreceptorPackage"] = value;
            }
        }
        #endregion
        #endregion


        #region [Page Events]
        /// <summary>
        /// OnInit event to set page titles
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            try
            {
                // _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                // base.OnInit(e);
                //  base.Title = "Order Confirmation";
                //CoreWeb.ComplianceOperations.Views.ComplianceOperationsDefault basePage = base.Page as CoreWeb.ComplianceOperations.Views.ComplianceOperationsDefault;
                //basePage.SetModuleTitle("Order Confirmation");
                //basePage.Title = "Order Confirmation";
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
        protected void Page_Load(object sender, EventArgs e)
        {
            CoreWeb.Shell.MasterPages.DefaultMaster masterPage = this.Master as CoreWeb.Shell.MasterPages.DefaultMaster;
            if (masterPage != null)
            {
                masterPage.HideTitleBars(true);
                masterPage.UseAsPopUpWindow = true;
            }
            if (!this.IsPostBack)
            {
                Dictionary<String, String> queryString = new Dictionary<string, string>();
                queryString.ToDecryptedQueryString(Request.QueryString["args"]);

                if (queryString.ContainsKey("IsInstructorPreceptorPackage"))
                {
                    CurrentViewContext.IsInstructorPreceptorPackage = Convert.ToBoolean(queryString["IsInstructorPreceptorPackage"]);

                    if (queryString.ContainsKey("SelectedTenantID"))
                    {
                        CurrentViewContext.TenantId = Convert.ToInt32(queryString["SelectedTenantID"]);
                    }
                }
                if (!queryString.ContainsKey("IsPaymentProcessCompleted"))
                {
                    BindItemPaymentConfirmationData();
                    BindCreditCardUserAgreement();
                    BindInstructions();
                    //TODO: Send Payment Notification
                    SaveItemPaymentCommunication();
                }
                else if (queryString.ContainsKey("ItemPaymentError"))
                {
                    base.ShowErrorMessage("An error occured while placing the order.");
                    // BaseUserControl.LogOrderFlowSteps("ItemPaymentConfirmation.ascx - STEP 1: Error : " + queryString["error"] + " occured, while paying the item payment by OrgUserId: " + CurrentViewContext.OrgUsrID);
                    //pnlDetail.Visible = false;
                }
            }

            hdnTenantID.Value = Convert.ToString(CurrentViewContext.TenantId);
           
        }
        #endregion
        [WebMethod]
        public static String CreateHtmlFile(String strHtmlCode, String orderNo, String orderNumber)
        {

            strHtmlCode = HttpUtility.UrlDecode(strHtmlCode);
            return SavePageHtmlContet(strHtmlCode, orderNo, orderNumber);

            //Commented below code related to Print Receipt bug[SS]:[07/04/2016]
            //if (SavePageHtmlContet(strHtmlCode, orderNo))
            //{
            //    return _fileIdentifier;
            //}
            //else
            //{
            //    return String.Empty;
            //}
        }
        private static String SavePageHtmlContet(String pageHtml, String orderNo, String orderNumber)
        {
            if (!String.IsNullOrEmpty(pageHtml))
            {
                String filePath = String.Empty;
                String fileIdentifier = String.Empty;
                String orderConfirnmationfileName = String.Empty;
                String applicantFileLocation = String.Empty;
                filePath = ConfigurationManager.AppSettings["TemporaryFileLocation"].ToString();
                if (!filePath.EndsWith("\\"))
                {
                    filePath += "\\";
                }
                filePath += @"HTMLConversion\";
                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);
                Guid Id = Guid.NewGuid();
                String fileName = "TempFile_" + Id + ".txt";
                if (!String.IsNullOrEmpty(orderNo))
                {
                    orderConfirnmationfileName = "OrderConfirmation" + "_" + orderNumber;
                }
                fileIdentifier = Convert.ToString(Id);
                if (!File.Exists(fileName))
                {
                    filePath = Path.Combine(filePath, fileName);

                    using (FileStream fs = File.Create(filePath))
                    {
                        //UAT 1212 Complio Receipt (Order Summary) Enhancement
                        StringBuilder customHtmlString = new StringBuilder();
                        //Without address header
                        //customHtmlString.Append("<div><img src=\"\\Resources\\Mod\\Shared\\images\\login\\adbRecieptLogo.png\" style=\"height:80px;\"><hr/></div>");
                        //With address header
                        customHtmlString.Append("<div><img src=\"\\Resources\\Mod\\Shared\\images\\login\\adbRecieptLogo.png\" style=\"height: 80px;\"><div style=\"float: right; height: 80px; width: 200px;\"><p style=\"line-height:2px;\">American DataBank, L.L.C.</p><p style=\"line-height:2px;\"> 110 16th Street 8th Floor</p><p style=\"line-height:2px;\"> Denver, CO 80202</p> <p style=\"line-height:2px;\">Phone: (800) 200-0853</p> </div><hr /></div>");
                        Int32 startIndex = pageHtml.IndexOf("150%");
                        StringBuilder tmp = new StringBuilder();
                        tmp.Append(pageHtml);
                        if (startIndex > -1)
                        {
                            tmp.Replace("150%", "120%", startIndex, 4);
                        }

                        customHtmlString.Append(tmp.ToString());
                        Byte[] info = new UTF8Encoding(true).GetBytes(customHtmlString.ToString());
                        fs.Write(info, 0, info.Length);
                    }
                    //Check whether use AWS S3, true if need to use
                    Boolean aWSUseS3 = false;
                    if (!ConfigurationManager.AppSettings["AWSUseS3"].IsNullOrEmpty())
                    {
                        aWSUseS3 = Convert.ToBoolean(ConfigurationManager.AppSettings["AWSUseS3"]);
                    }
                    if (aWSUseS3 == false)
                    {
                        //Move file to other location
                        applicantFileLocation = ConfigurationManager.AppSettings["ApplicantFileLocation"].ToString();
                        if (!applicantFileLocation.EndsWith("\\"))
                        {
                            applicantFileLocation += "\\";
                        }
                        applicantFileLocation += @"HTMLConversion\";
                        if (!Directory.Exists(applicantFileLocation))
                        {
                            Directory.CreateDirectory(applicantFileLocation);
                        }
                        applicantFileLocation = Path.Combine(applicantFileLocation, fileName);
                        MoveFile(filePath, applicantFileLocation);
                    }
                    else
                    {
                        applicantFileLocation = ConfigurationManager.AppSettings["ApplicantFileLocation"].ToString();
                        if (!applicantFileLocation.EndsWith("//"))
                        {
                            applicantFileLocation += "//";
                        }
                        //AWS code to save document to S3 location
                        AmazonS3Documents objAmazonS3 = new AmazonS3Documents();
                        String destFolder = applicantFileLocation + @"HTMLConversion/";
                        String returnFilePath = objAmazonS3.SaveDocument(filePath, fileName, destFolder);
                        applicantFileLocation = returnFilePath; //destFolder + fileName;
                        try
                        {
                            if (!String.IsNullOrEmpty(filePath))
                                File.Delete(filePath);
                        }
                        catch (Exception) { }
                    }
                    SaveFilePath(applicantFileLocation, Id, orderConfirnmationfileName);
                    return fileIdentifier;
                }
                else
                {
                    return String.Empty;
                }
            }
            else
            {
                return String.Empty;
            }
        }
        private static Boolean SaveFilePath(String filePath, Guid Id, String fileName)
        {
            Entity.TempFile tempFile = new Entity.TempFile();
            tempFile.TF_Path = filePath;
            tempFile.TF_Identifier = Id;
            tempFile.TF_IsDeleted = false;
            tempFile.TF_CreatedOn = DateTime.Now;
            tempFile.TF_CreatedByID = 1;
            tempFile.TF_FileName = fileName;
            return SecurityManager.SavePageHtmlContentLocation(tempFile);
        }

        /// <summary>
        /// Move file to other location
        /// </summary>
        /// <param name="sourceFilePath"></param>
        /// <param name="destinationFilePath"></param>
        private static void MoveFile(String sourceFilePath, String destinationFilePath)
        {
            if (!String.IsNullOrEmpty(sourceFilePath))
            {
                File.Copy(sourceFilePath, destinationFilePath);
            }
            try
            {
                if (!String.IsNullOrEmpty(sourceFilePath))
                    File.Delete(sourceFilePath);
            }
            catch (Exception) { }
        }

        #region [Button CLick]
        protected void CmdBarSubmit_Click(object sender, EventArgs e)
        {

            if (CurrentViewContext.IsInstructorPreceptorPackage)
            {
                ItemPaymentContract itemPaymentContract = SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.INSTRUCTOR_PRECEPTOR_PARKING_CART) as ItemPaymentContract;
                Session.Remove(ResourceConst.INSTRUCTOR_PRECEPTOR_PARKING_CART);
                itemPaymentContract.RemoveItemPaymentSession = false;
                SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.INSTRUCTOR_PRECEPTOR_PARKING_CART, itemPaymentContract);
                String script = "function f(){closeWin(); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), script, true);
            }
            else
            {
                ItemPaymentContract itemPaymentContract = SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_PARKING_CART) as ItemPaymentContract;
                Session.Remove(ResourceConst.APPLICANT_PARKING_CART);
                itemPaymentContract.RemoveItemPaymentSession = false;
                SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.APPLICANT_PARKING_CART, itemPaymentContract);
                String script = "function f(){closeWin(); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), script, true);
            }
            #region Old Code


            //if (itemPaymentContract.IsRequirementPackage)
            //{
            //    String menuId = "10";
            //    // BaseUserControl.LogOrderFlowSteps("OrderConfirmation.ascx - STEP 5: 'Finish' clicked, Redirecitng to dashboard wih url: " + AppConsts.DASHBOARD_URL + "?MenuId=" + menuId + "&ReqPkgSubscriptionId=" + itemPaymentContract.PkgSubscriptionId + "&ClinicalRotationId=" + itemPaymentContract.ClinicalRotationID + ", for OrderId(s): " + lblOrderId.Text);
            //    Session.Remove(ResourceConst.APPLICANT_PARKING_CART);
            //    RedirectUrl = RedirectUrl + "?MenuId=" + menuId + "&ReqPkgSubscriptionId=" + itemPaymentContract.PkgSubscriptionId + "&ClinicalRotationId=" + itemPaymentContract.ClinicalRotationID;

            //    //  Response.Redirect(AppConsts.DASHBOARD_URL + "?MenuId=" + menuId + "&ReqPkgSubscriptionId=" + itemPaymentContract.PkgSubscriptionId + "&ClinicalRotationId=" + itemPaymentContract.ClinicalRotationID, true);
            //}
            //// BaseUserControl.LogOrderFlowSteps("OrderConfirmation.ascx - STEP 5: 'Finish' clicked, Redirecitng to dashboard wih url: " + AppConsts.DASHBOARD_URL + "?ItemPaymentPkgSubscriptionId=" + itemPaymentContract.PkgSubscriptionId + ", for OrderId(s): " + lblOrderId.Text);

            //RedirectUrl = RedirectUrl + "?ItemPaymentPkgSubscriptionId=" + itemPaymentContract.PkgSubscriptionId;
            //  Response.Redirect(AppConsts.DASHBOARD_URL + "?ItemPaymentPkgSubscriptionId=" + itemPaymentContract.PkgSubscriptionId, true);
            //String script = "function f(){closeWin('" + RedirectUrl + "'); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
            #endregion
        }
        #endregion

        #region [Methods]
        #region [Private Method]

        private void BindItemPaymentConfirmationData()
        {
            ItemPaymentContract itemPaymentContract = SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_PARKING_CART) as ItemPaymentContract;
            if (CurrentViewContext.IsInstructorPreceptorPackage)
                itemPaymentContract = SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.INSTRUCTOR_PRECEPTOR_PARKING_CART) as ItemPaymentContract;
            lblItemName.Text = itemPaymentContract.ItemName.HtmlEncode();
            //  lblCompliancePkgPaymentType.Text = itemPaymentContract.IsRequirementPackage ? "Requirement/Rotation" : "Compliance/Tracking";
            lblPrice.Text = String.Concat("$", Convert.ToString(Math.Round(itemPaymentContract.TotalPrice, 2, MidpointRounding.AwayFromZero)));
            //lblTotalPrice.Text = String.Concat(Convert.ToString(Math.Round(itemPaymentContract.TotalPrice, 2, MidpointRounding.AwayFromZero)), "$");
            lblOrderId.Text = itemPaymentContract.orderID.ToString();
            lblOrderNumber.Text = (!String.IsNullOrEmpty(itemPaymentContract.OrderNumber) ? itemPaymentContract.OrderNumber : itemPaymentContract.orderID.ToString()).HtmlEncode();

            #region Bind Personal Info

            _orgUserProfile = Presenter.GetOrganizationUserProfileByOrganizationUserProfileID(itemPaymentContract.OrganizationUserProfileID);
            if (_orgUserProfile.IsNotNull())
            {
                #region UAT-781 ENCRYPTED SSN
                Presenter.GetDecryptedSSN(_orgUserProfile.OrganizationUserProfileID, true);
                #endregion

                //Show Personal Information
                CurrentViewContext.GenderId = Convert.ToInt32(_orgUserProfile.Gender);
                if (CurrentViewContext.GenderId > AppConsts.NONE)
                    Presenter.GetGender();
                lblFirstName.Text = _orgUserProfile.FirstName.HtmlEncode();
                lblLastName.Text = _orgUserProfile.LastName.HtmlEncode();
                //lblMiddleName.Text = _orgUserProfile.MiddleName;
                //UAT-2212: Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                lblMiddleName.Text = _orgUserProfile.MiddleName.IsNullOrEmpty() ? String.Empty : _orgUserProfile.MiddleName.HtmlEncode();

                if (_orgUserProfile.DOB.HasValue)
                {
                    lblDateOfBirth.Text = Presenter.GetMaskDOB(_orgUserProfile.DOB.Value.ToShortDateString());
                }
                lblGender.Text = CurrentViewContext.Gender;
                //Commented SSN: UAT-1059:Remove I.P. address and mask social security number from order summary
                //lblSSN.Text = Presenter.GetMaskedSSN(_orgUserProfile.SSN); //UAT-781
                // Presenter.GetDecryptedSSN(itemPaymentContract.CreatedByID, false);
                lblSSN.Text = Presenter.GetMaskedSSN(CurrentViewContext.DecryptedSSN); //UAT-781
                lblEmail.Text = _orgUserProfile.PrimaryEmailAddress.HtmlEncode();
                //UAT-2447
                if (_orgUserProfile.IsInternationalPhoneNumber)
                {
                    lblPhone.Text = _orgUserProfile.PhoneNumber.HtmlEncode();
                }
                else
                {
                    lblPhone.Text = Presenter.GetFormattedPhoneNumber(_orgUserProfile.PhoneNumber).HtmlEncode();
                }

                Entity.ResidentialHistory currentResHistory = Presenter.GetCurrentResidentialHistory(_orgUserProfile.OrganizationUserID);
                if (currentResHistory.IsNotNull())
                {
                    lblAddress1.Text = (currentResHistory.Address.Address1 + "," + currentResHistory.Address.Address2).HtmlEncode();
                    if (currentResHistory.Address.ZipCodeID > 0)
                    {
                        lblZip.Text = currentResHistory.Address.ZipCode.ZipCode1.HtmlEncode();
                        lblCity.Text = currentResHistory.Address.ZipCode.City.CityName.HtmlEncode();
                        lblState.Text = currentResHistory.Address.ZipCode.City.State.StateName.HtmlEncode();
                        lblCountry.Text = currentResHistory.Address.ZipCode.City.State.Country.FullName;
                    }
                    else
                    {
                        if (currentResHistory.Address.AddressExts.IsNotNull() && currentResHistory.Address.AddressExts.Count > 0)
                        {
                            Entity.AddressExt addressExt = currentResHistory.Address.AddressExts.FirstOrDefault();
                            lblZip.Text = addressExt.AE_ZipCode.HtmlEncode();
                            lblCity.Text = addressExt.AE_CityName.HtmlEncode();
                            lblState.Text = addressExt.AE_StateName.HtmlEncode();
                            lblCountry.Text = addressExt.Country.FullName;
                        }
                    }
                    lblResidingFrom.Text = currentResHistory.RHI_ResidenceStartDate.HasValue ? currentResHistory.RHI_ResidenceStartDate.Value.ToShortDateString() : String.Empty;
                    lblResidingTo.Text = currentResHistory.RHI_ResidenceEndDate.HasValue ? currentResHistory.RHI_ResidenceEndDate.Value.ToShortDateString() : "until date";
                }

            }

            #endregion

            Presenter.GetPkgPaymentOptions();

            lblPaymentMode.Text = CurrentViewContext.PaymentModeDisplayName;
            hdfPaymentType.Value = CurrentViewContext.PaymentModeCode;
            //lblGroupPrice.Text = String.Concat(Convert.ToString(Math.Round(itemPaymentContract.TotalPrice, 2, MidpointRounding.AwayFromZero)), "$");

            //   var _clientPaymentOptn = CurrentViewContext.lstPaymentOptions.Where(po => po.PaymentOptionId == CurrentViewContext.PaymentModeId).FirstOrDefault();
            // var _controlId = "pi_" + _clientPaymentOptn.PaymentOptionCode;
            //var _isControlAdded = (pnlInstructions.FindControl(_controlId) as System.Web.UI.Control).IsNullOrEmpty() ? false : true;

            //if (_clientPaymentOptn.IsNotNull() && !_isControlAdded)
            //{
            //    System.Web.UI.Control _piInstructions = Page.LoadControl("~/ComplianceOperations/UserControl/PaymentInstructions.ascx");
            //    (_piInstructions as PaymentInstructions).InstructionsText = CurrentViewContext.InstructionText;
            //    (_piInstructions as PaymentInstructions).PaymentModeText = CurrentViewContext.PaymentModeDisplayName;
            //    pnlInstructions.Controls.Add(_piInstructions);
            //}

            //Send itemstatus change notification
            Presenter.SendItemStatusChangeNotification(itemPaymentContract);
        }

        private void BindCreditCardUserAgreement()
        {
            dvUserAgreement.Visible = true;
            litText.Text = Presenter.GetCreditCardAgreement();
        }

        /// <summary>
        /// Bind the Payment Instructions for all the Payment Modes selected
        /// </summary>
        private void BindInstructions()
        {
            rptInstructions.DataSource = CurrentViewContext.lstClientPaymentOptns;
            rptInstructions.DataBind();
            if (rptInstructions.Items.Count != AppConsts.NONE)
                divPaymentInstruction.Visible = true;
            else
                divPaymentInstruction.Visible = false;
        }

        private void SaveItemPaymentCommunication()
        {
            if (hdnIsNotificationSent.Value == AppConsts.ZERO)
            {
                ItemPaymentContract itemPaymentContract = SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_PARKING_CART) as ItemPaymentContract;
                if (CurrentViewContext.IsInstructorPreceptorPackage)
                    itemPaymentContract = SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.INSTRUCTOR_PRECEPTOR_PARKING_CART) as ItemPaymentContract;
                if (_orgUserProfile.IsNullOrEmpty())
                    _orgUserProfile = Presenter.GetOrganizationUserProfileByOrganizationUserProfileID(itemPaymentContract.OrganizationUserProfileID);
                Presenter.SaveItemPaymentCommunication(itemPaymentContract, _orgUserProfile);
                hdnIsNotificationSent.Value = AppConsts.ONE.ToString();
            }
        }
        #endregion
        #endregion
    }
}