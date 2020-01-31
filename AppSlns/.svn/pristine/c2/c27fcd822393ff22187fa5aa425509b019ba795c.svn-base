#region NameSpaces
#region System Defined
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
#endregion

#region Project Specific
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using Entity.ClientEntity;
using INTSOF.UI.Contract.BkgOperations;
using System.Configuration;
using System.Globalization;
#endregion
#endregion

namespace CoreWeb.AdminEntryPortal.Views
{
    #region Custom EventArg Classes
    public class WebCCFErrorEventArgs : EventArgs
    {
        public string ErrorMessage { get; set; }
    }

    public class WebCCFSuccessEventArgs : EventArgs
    {
        public String RegistrationID { get; set; }
        public String SuccessMessage { get; set; }
        public String IsUSCitizen { get; set; }
        public String DonorGender { get; set; }
        public String DonorEmail { get; set; }
        public String ExpirationDate { get; set; } //UAT-4251
        public String LabName { get; set; } //UAT-5114
        public String CategoryReason { get; set; } //UAT-5114
        public String ExpirationDateCCF { get; set; } //UAT-5114
    }
    #endregion

    public partial class AdminEntryWebCCF : BaseUserControl, IAdminEntryWebCCFView
    {
        #region Delegates and Events
        public delegate void OnErrorWebCCF(object sender, WebCCFErrorEventArgs e);
        public event OnErrorWebCCF WebCCFError;
        public delegate void OnSuccessWebCCF(object sender, WebCCFSuccessEventArgs e);
        public event OnSuccessWebCCF WebCCFSuccess;
        #endregion

        #region Private Variables
        private bool _isReview = false;
        private bool _isOrderConfirmation = false;
        private bool _showRegistrationId = false;
        private AdminEntryWebCCFPresenter _presenter = new AdminEntryWebCCFPresenter();
        private Int32 _tenantId = 0;
        #endregion

        #region Properties

        #region Private Properties

        #endregion

        #region public Properties
        public AdminEntryWebCCFPresenter Presenter
        {
            get
            {
                this._presenter.View = this;
                return this._presenter;
            }
            set
            {
                this._presenter = value;
                this._presenter.View = this;
            }
        }

        String IAdminEntryWebCCFView.StateAbbreviation{get;set;}

        String IAdminEntryWebCCFView.ZipCode{get;set;}

        List<Int32> IAdminEntryWebCCFView.BackgroundPackageIdList{get;set;}

        String IAdminEntryWebCCFView.ClearStarServiceId{get;set;}

        /// <summary>
        /// Gets and sets Logged In User TenantId
        /// </summary>
        Int32 IAdminEntryWebCCFView.TenantId
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
            set { _tenantId = value; }
        }

        List<Int32> IAdminEntryWebCCFView.BPHMIds{get;set;}

        String IAdminEntryWebCCFView.ExtVendorAccountNumber{get;set;}

        String IAdminEntryWebCCFView.IsUSCitizen{get;set;}
        /// <summary>
        /// This property is to hide the WebCCF javascript
        /// </summary>
        public bool IsReview
        {
            get { return _isReview; }
            set { _isReview = value; }
        }

        /// <summary>
        /// This property is to show the order confirmation.
        /// </summary>
        public bool IsOrderConfirmation
        {
            get { return _isOrderConfirmation; }
            set { _isOrderConfirmation = value; }
        }

        /// <summary>
        /// This property is used to show the resgistration id on order confirmation
        /// </summary>
        public bool ShowRegistrationID
        {
            get { return _showRegistrationId; }
            set { _showRegistrationId = value; }
        }
        public Int32 AttributeGroupId
        {
            get
            {
                if (!hdnAttributeGroupId.IsNullOrEmpty())
                    return Convert.ToInt32(hdnAttributeGroupId.Value);
                return AppConsts.NONE;
            }
            set
            {
                hdnAttributeGroupId.Value = value.ToString();
            }
        }

        public Int32 CustomFormId
        {
            get
            {
                if (!hdnCustomFormId.IsNullOrEmpty())
                    return Convert.ToInt32(hdnCustomFormId.Value);
                return AppConsts.NONE;
            }
            set
            {
                hdnCustomFormId.Value = value.ToString();
            }
        }

        public List<BackgroundOrderData> LstBackgroundOrderData
        {
            get;
            set;
        }

        public List<AttributesForCustomFormContract> LstAttributeForCustomFormContract
        {
            get;
            set;
        }

        public Int32 BOID
        {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["ClearstarBOID"]); }
        }

        public Boolean IsAdminOrderScreen
        {
            get
            {
                if (ViewState["IsAdminOrderScreen"].IsNullOrEmpty())
                    return false;
                return Convert.ToBoolean(ViewState["IsAdminOrderScreen"]);
            }
            set
            {
                ViewState["IsAdminOrderScreen"] = value;
            }
        }

        Int32 IAdminEntryWebCCFView.SelectedNodeId
        {
            get
            {
                if (ViewState["SelectedNodeId"].IsNullOrEmpty())
                    return AppConsts.NONE;
                return Convert.ToInt32(ViewState["SelectedNodeId"]);
            }
            set
            {
                ViewState["SelectedNodeId"] = value;
            }
        }

        #region Current View Context
        private IAdminEntryWebCCFView CurrentViewContext
        {
            get { return this; }
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
                if (!Page.IsPostBack)
                {
                    if (!ConfigurationManager.AppSettings["ClearstarWidgetUrl"].IsNullOrEmpty())
                        MIS.Src = ConfigurationManager.AppSettings["ClearstarWidgetUrl"];

                    HideShowPanel();
                    if (IsReview || IsOrderConfirmation)
                    {
                        //UAT-2842 
                        //Get AplicantOrder Cart
                        ApplicantOrderCart applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
                        if (applicantOrderCart.TenantId > AppConsts.NONE)
                        {
                            CurrentViewContext.TenantId = applicantOrderCart.TenantId;
                            IsAdminOrderScreen = true;
                        }
                        BackgroundOrderData temp = LstBackgroundOrderData.FirstOrDefault(cond => cond.BkgSvcAttributeGroupId == AttributeGroupId && cond.CustomFormId == CustomFormId);
                        Dictionary<Int32, String> dicAttributeData = temp.CustomFormData;
                        //String attributeName = Presenter.GetRegistrationIdAttributeName(AttributeGroupId);
                        Int32 registrationIdMappId = LstAttributeForCustomFormContract.FirstOrDefault(cond => cond.AttributeGroupMappingCode.ToUpper() == AppConsts.DRUG_SCREEN_REGISTRATION_ID).AtrributeGroupMappingId;
                        String registrationId = dicAttributeData.GetValue(registrationIdMappId);
                        Boolean success = (registrationId != String.Empty);
                        if (ShowRegistrationID)
                        {
                            divRegistrationID.Style["display"] = "block";
                            lblRegistrationId.Text = registrationId;
                            divRegistrationIdConfrm.Style["display"] = "block";
                            lblRegistrationIdConfirm.Text = registrationId;
                        }
                        lblSuccess.Visible = success;
                        lblFailure.Visible = !success;

                        //UAT-2842
                        if (IsAdminOrderScreen && !success)
                        {
                            lblFailure.Text = "NOT SCHEDULED";
                            divEditButton.Style["display"] = "block";
                        }
                        else
                        {
                            divEditButton.Style["display"] = "none";
                        }

                        if (IsOrderConfirmation)
                        {
                            lblSuccessConfirm.Visible = true;
                            lblRegistrationIdConfirm.Text = registrationId;
                        }
                    }
                    else
                    {
                        SetRequiredWebCCFVariables();
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
        #endregion

        #region Button Events
        protected void btnDrugScreenComplete_Click(object sender, EventArgs e)
        {
            try
            {
                //Bubble up the Complete...
                if (WebCCFSuccess != null)
                {
                    //UAT-4251 
                    ExternalVendors.ClearStarVendor.ClearStarCCF objClearstarCCf = new ExternalVendors.ClearStarVendor.ClearStarCCF();
                    GetDonorCCF donorCCFData = null;
                    if (!hdnBOID.IsNullOrEmpty() && !hdnRegistrationID.IsNullOrEmpty() && !hdnCustomerID.IsNullOrEmpty())
                    {
                        donorCCFData = objClearstarCCf.GetClearstarDonorCCF(Convert.ToInt32(ConfigurationManager.AppSettings["ClearstarBOID"]), hdnCustomerID.Value,
                            ConfigurationManager.AppSettings["ClearstarUserName"], ConfigurationManager.AppSettings["ClearstarPassword"], hdnRegistrationID.Value);
                    }
                    //END UAT
                    WebCCFSuccessEventArgs args = new WebCCFSuccessEventArgs();
            
                    args.RegistrationID = hdnRegistrationID.Value;
                    args.DonorEmail = hdnEmail.Value;
                    args.DonorGender = hdnGender.Value;
                    args.IsUSCitizen = hdnIsUSCitizen.Value;                   
                    args.SuccessMessage = "Drug Sceening scheduled successfully";
                    //UAT-4251
                    if (donorCCFData.IsNotNull())   
                    {
                        var est = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                        if(!donorCCFData.Subject.IsNullOrEmpty())
                        {
                            Int32 indexOfExpiresOn = donorCCFData.Subject.IndexOf("Drug Screen Expires on ");
                            Int32 lengthOfText = ("Drug Screen Expires on ").Length;
                            Int32 lengthOfSubject = donorCCFData.Subject.Length;
                            if(indexOfExpiresOn > 0)
                            {
                                args.ExpirationDateCCF = donorCCFData.Subject.Substring(indexOfExpiresOn + lengthOfText, donorCCFData.Subject.Length - (indexOfExpiresOn + lengthOfText));
                                args.ExpirationDate = donorCCFData.Subject.Substring(indexOfExpiresOn + lengthOfText, donorCCFData.Subject.Length - (indexOfExpiresOn + lengthOfText));
                            }
                            else
                            {
                                DateTime expirationDateTime =  TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(donorCCFData.ExpirationDate), est);
                                args.ExpirationDate= expirationDateTime.ToString("MM/dd/yyyy hh:mm:ss tt") + " EST";
                                args.ExpirationDateCCF = expirationDateTime.ToString("MM/dd/yyyy hh:mm:ss tt") + " EST";
                            }
                        }
                      //  args.ExpirationDate = donorCCFData.ExpirationDate;
                        args.CategoryReason = donorCCFData.TestReason;
                        args.LabName = donorCCFData.LabName;
                        //args.ExpirationDateCCF = donorCCFData.Subject.Substring(124, 26);
                    }

                        
                    if (args.RegistrationID == "1111111111")
                    {
                        if (WebCCFError != null)
                        {
                            WebCCFErrorEventArgs bargs = new WebCCFErrorEventArgs();
                            bargs.ErrorMessage = "Registration Number Issued from system is not verifiable";
                            WebCCFError(this, bargs);
                        }
                    }
                    else
                    {
                        WebCCFSuccess(this, args);
                    }
                }
                else
                {
                    WebCCFErrorEventArgs bargs = new WebCCFErrorEventArgs();
                    WebCCFError(this, bargs);
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

        protected void btnDrugScreenError_Click(object sender, EventArgs e)
        {
            try
            {
                //Bubble up the Error...
                if (WebCCFError != null)
                {
                    WebCCFErrorEventArgs args = new WebCCFErrorEventArgs();
                    args.ErrorMessage = hdnErrorMessage.Value;
                    WebCCFError(this, args);
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
        #endregion
        #endregion

        

        #region Methods
        #region Private Methods
        /// <summary>
        /// Method to set the WebCCF required Variables
        /// </summary>
        private void SetRequiredWebCCFVariables()
        {
            //Get AplicantOrder Cart
            ApplicantOrderCart applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
            //Get Current Address of applicant.
            if (!applicantOrderCart.IsNullOrEmpty())
            {
                PreviousAddressContract applicantCurrentResidence = applicantOrderCart.lstPrevAddresses.FirstOrDefault(cond => cond.isCurrent == true);

                //UAT-2842 
                if (applicantOrderCart.TenantId > AppConsts.NONE)
                {
                    CurrentViewContext.TenantId = applicantOrderCart.TenantId;
                    IsAdminOrderScreen = true;
                }
                //Get Organization profile of Applicant.
                OrganizationUserProfile userProfile = applicantOrderCart.lstApplicantOrder[0].OrganizationUserProfile;
                CurrentViewContext.BackgroundPackageIdList = applicantOrderCart.lstApplicantOrder[0].lstPackages.Select(slct => slct.BPAId).ToList();
                CurrentViewContext.BPHMIds = applicantOrderCart.lstApplicantOrder[0].lstPackages.Select(slct => slct.BPHMId).ToList();

                //UAT-3056
                CurrentViewContext.SelectedNodeId = applicantOrderCart.SelectedHierarchyNodeID.HasValue ? applicantOrderCart.SelectedHierarchyNodeID.Value : AppConsts.NONE;

                //Set County id.
                CurrentViewContext.ZipCode = applicantCurrentResidence.Zipcode;
                //Call method to set state Abbriviation property.
                Presenter.GetStateAbbriviationByZipCode();
                //Method that set ClearStarServiceId and ExtVendorAccountNumber properties.
                Presenter.GetClearStarSvcIdAndVendorAcctNumber();

                #region Set Hidden Fields for WebCCF required Variables
               
                hdnBOID.Value = Convert.ToString(BOID);
                hdnIsHavingSSN.Value = userProfile.SSN == AppConsts.DefaultSSN ? "False" : "True"; ; //UAT-4503
                hdnCustomerID.Value = CurrentViewContext.ExtVendorAccountNumber;//TODO eva.AccountNumber;
              
                hdnFirstName.Value = userProfile.FirstName;
                hdnMiddleName.Value = userProfile.MiddleName;
                hdnLastName.Value = userProfile.LastName;
                hdnSuffix.Value = string.Empty;
                hdnAddress1.Value = applicantCurrentResidence.Address1;
                hdnAddress2.Value = applicantCurrentResidence.Address2;
                hdnCity.Value = applicantCurrentResidence.CityName;
                hdnState.Value = CurrentViewContext.StateAbbreviation;
                hdnZip.Value = applicantCurrentResidence.Zipcode;
                hdnEmail.Value = userProfile.PrimaryEmailAddress;
                hdnPhoneDay.Value = userProfile.PhoneNumber;
                hdnPhoneEvening.Value = userProfile.PhoneNumber;
                hdnDOB.Value = Convert.ToDateTime(userProfile.DOB).ToShortDateString();
                hdnSSN.Value = userProfile.SSN;
                hdnGender.Value = userProfile.Gender.Value == 2 ? Gender.Female.GetStringValue() : Gender.Male.GetStringValue();
                
                hdnTest.Value = CurrentViewContext.ClearStarServiceId;
                
                hdnReason.Value = "P";
                hdnIsUSCitizen.Value = CurrentViewContext.IsUSCitizen.IsNullOrEmpty() ? "Yes" : CurrentViewContext.IsUSCitizen;
                #endregion
            }
        }

        private void HideShowPanel()
        {

            if (IsOrderConfirmation)
            {
                divOrderConfirmation.Visible = true;
                divReadOnly.Visible = false;
                divCapture.Visible = false;
            }
            else
            {
                pnlCapture.Visible = !IsReview;
                pnlReadOnly.Visible = IsReview;
                divReadOnly.Visible = IsReview;
                divCapture.Visible = !IsReview;
                divOrderConfirmation.Visible = IsOrderConfirmation;
            }

        }
        #endregion

        #region Public Methods
        public void Reload(Boolean isReload)
        {
            if (isReload)
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Reload", "Reload();", true);
        }
        #endregion

        protected void cmdbarEdit_Click(object sender, EventArgs e)
        {
            ApplicantOrderCart applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
            if (applicantOrderCart.TenantId > AppConsts.NONE)
            {
                CurrentViewContext.TenantId = applicantOrderCart.TenantId;
                IsAdminOrderScreen = true;
            }            
            String url = "AdminEntryPortal/Pages/AdminEntryCustomFormPage.aspx?SelectedTenantId=" + Convert.ToString(CurrentViewContext.TenantId) + "&IsAdminEditMode=false&IsNewCustomForm=false&CustomFormId=" + Convert.ToString(CustomFormId) + "&IsPrevious=1";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "OpenCustomForm('" + url + "');", true);
        }
        #endregion
    }
}