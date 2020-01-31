using System;
using System.Collections.Generic;
using Entity;
using INTSOF.Utils;
using System.IO;
using System.Web.Configuration;
using Business.RepoManagers;
using INTSOF.UI.Contract.ComplianceOperation;
using System.Threading;
using System.Configuration;
using System.Web.UI.HtmlControls;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;
using System.Linq;
using Telerik.Web.UI;
using INTSOF.UI.Contract.Globalization;

namespace CoreWeb.IntsofSecurityModel.Views
{
    public partial class ITSUserRegistration : BaseUserControl, IITSUserRegistrationView
    {
        private ITSUserRegistrationPresenter _presenter = new ITSUserRegistrationPresenter();
        //private const String NONE_OF_THESE = "None of the above, Create new account.";
        //private const String NONE_OF_THESE = Resources.Language.NONEOFABOVE;
        //UAT-862 :- WB: As a student or an admin, I should not be allowed to upload documents with a size of 0 
        private Boolean _isCorruptedFileUploaded = false;
        #region Properties

        #region Public Properties
        public ITSUserRegistrationPresenter Presenter
        {
            get
            {
                this._presenter.View = this; return this._presenter;
            }
            set
            {
                this._presenter = value;
                this._presenter.View = this; ;
            }
        }

        // TODO: Forward events to the presenter and show state to the user.
        // For examples of this, see the View-Presenter (with Application Controller) QuickStart:
        //	

        public IITSUserRegistrationView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public List<Tenant> Tenants
        {
            set
            {
                cmbOrganization.DataSource = value;
                cmbOrganization.DataBind();
                cmbOrganization.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT--"));
            }
        }

        public LanguageContract CurrentLanguageContract
        {
            get
            {
                return LanguageTranslateUtils.GetCurrentLanguageFromSession();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<lkpGender> Gender
        {
            set
            {
                cmbGender.DataSource = value.Where(col => col.LanguageID == LanguageTranslateUtils.GetCurrentLanguageFromSession().LanguageID);
                // cmbGender.DataSource = value;
                cmbGender.DataBind();
                cmbGender.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(Resources.Language.SELECTWITHHYPENS, "0"));
            }
        }

        /// <summary>
        /// ErrorMessage
        /// </summary>
        public string ErrorMessage
        {
            set
            {
                msgBox.Attributes.CssStyle.Add("display", "block");
                lblMessage.ShowMessage(value, MessageType.Error);
            }
        }

        /// <summary>
        /// LoginErrorMessage
        /// </summary>
        public String LoginErrorMessage
        {
            set
            {
                lblErrorMessage.ShowMessage(value, MessageType.Error);
            }
        }

        /// <summary>
        /// SuccessMessage
        /// </summary>
        public string SuccessMessage
        {
            set
            {
                msgBox.Attributes.CssStyle.Add("display", "block");
                lblMessage.ShowMessage(value, MessageType.SuccessMessage);
            }
        }

        string IITSUserRegistrationView.UserName
        {
            get
            {
                return txtUsername.Text.Trim();
            }
        }
        Boolean IITSUserRegistrationView.IsAutoActive
        {
            get;
            set;
        }

        public string Password
        {
            get
            {
                return txtPassword.Text;
            }
        }

        string IITSUserRegistrationView.FirstName
        {
            get
            {
                return txtFirstName.Text;
            }
        }

        string IITSUserRegistrationView.MiddleName
        {
            get
            {
                //UAT-1612 : As an applicant, my middle name should be required.
                if (chkMiddleNameRequired.Checked)
                {
                    return String.Empty;
                }
                else
                {
                    return txtMiddleName.Text;
                }
            }
        }

        string IITSUserRegistrationView.LastName
        {
            get
            {
                return txtLastName.Text;
            }
        }

        DateTime? IITSUserRegistrationView.DOB
        {
            get
            {
                return dpkrDOB.SelectedDate;
            }
        }

        public String SSN
        {
            get
            {
                return txtSSN.Text;
                //return Convert.ToString(hdnSSN.Value);
            }
        }

        public int SelectedGenderId
        {
            get
            {
                return Convert.ToInt32(cmbGender.SelectedValue);
            }
        }

        string IITSUserRegistrationView.PrimaryPhone
        {
            get
            {
                if (chkIsMaskingRequiredPrimary.Checked)
                    return txtPrimaryPhoneNonMasking.Text;
                else
                    return txtPrimaryPhone.Text;
            }
        }

        string IITSUserRegistrationView.PrimaryEmail
        {
            get
            {
                return txtPrimaryEmail.Text;
            }
        }
        string IITSUserRegistrationView.Suffix
        {
            get
            {
                return txtSuffix.Text;
            }
            set
            {

            }
        }
        Boolean IITSUserRegistrationView.IsSuffixDropDownType
        {
            get
            {
                if (ViewState["IsSuffixDropDownType"] != null)
                    return (Boolean)(ViewState["IsSuffixDropDownType"]);
                return false;
            }
            set
            {
                ViewState["IsSuffixDropDownType"] = value;
            }

        }
        public string Address1
        {
            get
            {
                return txtAddress1.Text;
            }
        }

        public string Address2
        {
            get
            {
                return txtAddress2.Text;
            }
        }

        public int ZipId
        {
            get
            {

                //if (locationTenant.ZipId == 0 && ViewState["ZipId"].IsNotNull())
                //    return Convert.ToInt32(ViewState["ZipId"]);
                //return locationTenant.ZipId;
                if (locationTenant.ZipId == 0 && ViewState["ZipId"].IsNotNull())
                    return Convert.ToInt32(ViewState["ZipId"]);
                return locationTenant.MasterZipcodeID.Value;
            }
            set
            {
                ViewState["ZipId"] = locationTenant.MasterZipcodeID.Value;
                //ViewState["ZipId"] = locationTenant.ZipId;
            }
        }

        public int SelectedTenantId
        {
            get
            {
                return ViewState["TenantId"].IsNull() ? SecurityManager.DefaultTenantID : Convert.ToInt32(ViewState["TenantId"]);
            }
            set
            {
                ViewState["TenantId"] = value;
            }
        }

        public string FilePath
        {
            get;
            set;
        }

        public string OriginalFileName
        {
            get;
            set;
        }

        public string SecondaryPhone
        {
            get
            {
                if (chkIsMaskingRequiredSecondary.Checked)
                    return txtSecondaryPhoneNonMasking.Text;
                else
                    return txtSecondaryPhone.Text;
            }
        }

        public string SecondaryEmail
        {
            get
            {
                return txtSecondaryEmail.Text;
            }
        }

        public List<PersonAliasContract> PersonAliasList
        {
            get
            {
                if (CurrentViewContext.IsLocationServiceTenant)
                {
                    return ucPersonAlias.PersonAliasTempListMaxOcc;
                }
                else
                {
                    return ucPersonAlias.PersonAliasList;
                }
            }
            set
            {
                ucPersonAlias.PersonAliasList = value;
            }
        }

        public String UserFirstName
        {
            get
            {
                return ucPersonAlias.UserFirstName;
            }
            set
            {
                ucPersonAlias.UserFirstName = value;
            }
        }

        public String UserLastName
        {
            get
            {
                return ucPersonAlias.UserLastName;
            }
            set
            {
                ucPersonAlias.UserLastName = value;
            }
        }

        public String UserMiddleName
        {
            get
            {
                return ucPersonAlias.UserMiddleName;
            }
            set
            {
                ucPersonAlias.UserMiddleName = value;
            }
        }

        //public string Alias1
        //{
        //    get
        //    {
        //        return txtAlias1.Text;
        //    }
        //}

        //public string Alias2
        //{
        //    get
        //    {
        //        return txtAlias2.Text;
        //    }
        //}

        //public string Alias3
        //{
        //    get
        //    {
        //        return txtAlias3.Text;
        //    }
        //}

        public String WebsiteUrl
        {
            get;
            set;
        }

        /// <summary>
        /// Current User Id.
        /// </summary>
        /// <value>
        /// The identifier of the current user.
        /// </value>
        public Int32 CurrentUserId
        {
            get
            {
                return base.CurrentUserId;
            }
        }

        public List<LookupContract> ExistingUsersList
        {
            set
            {
                LookupContract lookupContract = new LookupContract();
                lookupContract.Name = Resources.Language.NONEOFABOVE;
                lookupContract.Code = Resources.Language.NONEOFABOVE;
                value.Add(lookupContract);
                rblExistingProfiles.DataSource = value;
                rblExistingProfiles.DataBind();
            }
        }

        public String LoginUserName
        {
            get
            {
                return txtLoginUserName.Text;
            }
        }

        public String LoginPassword
        {
            get
            {
                return txtLoginPassword.Text;
            }
        }

        public String setSubmitbuttonText
        {
            set
            {
                cbPswdVerification.SubmitButton.Text = value;
                cbPswdVerification.ShowButtons(CommandBarButtons.Save);
                if (PasswordAttemptCount == 3)
                {
                    cbPswdVerification.HideButtons(CommandBarButtons.Submit);
                }
            }
        }

        public OrganizationUser ExistingOrganisationUser
        {
            get;
            set;
        }

        public Boolean ShowExistingUsers
        {
            get
            {
                return ViewState["ShowExistingUsers"].IsNull() ? true : Convert.ToBoolean(ViewState["ShowExistingUsers"]);
            }
            set
            {
                ViewState["ShowExistingUsers"] = value.ToString();
            }
        }

        public Int32 PasswordAttemptCount
        {
            get
            {
                return ViewState["PasswordAttemptCount"].IsNull() ? 0 : Convert.ToInt32(ViewState["PasswordAttemptCount"]);
            }
            set
            {
                ViewState["PasswordAttemptCount"] = value.ToString();
            }
        }
        public String StateName
        {
            get
            {
                return locationTenant.RSLStateName;
            }
        }

        public String CityName
        {
            get
            {
                return locationTenant.RSLCityName;
            }
        }

        public String PostalCode
        {
            get
            {
                return locationTenant.RSLZipCode;
            }
        }

        public Int32 CountryId
        {
            get
            {
                return locationTenant.RSLCountryId;
            }
        }

        Boolean IITSUserRegistrationView.IsSSNDisabled
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

        List<Entity.ClientEntity.TypeCustomAttributes> IITSUserRegistrationView.SaveCustomAttributeList
        {
            get;
            set;
        }

        #region UAT-2515
        Int32 IITSUserRegistrationView.ExternalUserTenantId
        {
            get;
            set;
        }

        Int32 IITSUserRegistrationView.IntegrationClientId
        {
            get
            {
                return Convert.ToInt32(ViewState["IntegrationClientId"]);
            }
            set
            {
                ViewState["IntegrationClientId"] = value;
            }
        }
        String IITSUserRegistrationView.ExternalId
        {
            get
            {
                return ViewState["ExternalId"].IsNotNull() ? ViewState["ExternalId"].ToString() : string.Empty;
            }
            set
            {
                ViewState["ExternalId"] = value;
            }
        }
        String IITSUserRegistrationView.MappingCode
        {

            get
            {
                return ViewState["MappingCode"].IsNotNull() ? ViewState["MappingCode"].ToString() : String.Empty;
            }
            set
            {
                ViewState["MappingCode"] = value;
            }

        }
        List<INTSOF.UI.Contract.SysXSecurityModel.ExternalLoginDataContract> IITSUserRegistrationView.matchingUserList
        {
            get;
            set;
        }

        String IITSUserRegistrationView.WebsiteLoginUrl
        {
            get;
            set;
        }
        #endregion

        #region UAT-2447
        Boolean IITSUserRegistrationView.IsMaskingOfPrimaryPhoneNumber
        {
            get
            {
                return chkIsMaskingRequiredPrimary.Checked;
            }
        }
        Boolean IITSUserRegistrationView.IsMaskingOfSecondaryPhoneNumber
        {
            get
            {
                return chkIsMaskingRequiredSecondary.Checked;
            }
        }
        #endregion

        #region UAT-2792
        Boolean IITSUserRegistrationView.IsShibbolethLogin
        {
            get
            {
                if (!ViewState["IsShibbolethLogin"].IsNullOrEmpty())
                {
                    return Convert.ToBoolean(ViewState["IsShibbolethLogin"]);
                }
                return false;
            }
            set
            {
                ViewState["IsShibbolethLogin"] = value;
            }
        }
        //UAT-2883
        String IITSUserRegistrationView.ShibbolethUniqueIdentifier
        {
            get
            {
                if (!ViewState["ShibbolethUniqueIdentifier"].IsNullOrEmpty())
                {
                    return Convert.ToString(ViewState["ShibbolethUniqueIdentifier"]);
                }
                return String.Empty;
            }
            set
            {
                ViewState["ShibbolethUniqueIdentifier"] = value;
            }
        }
        String IITSUserRegistrationView.ShibbolethAttributeId
        {
            get
            {
                if (!ViewState["ShibbolethAttributeId"].IsNullOrEmpty())
                {
                    return Convert.ToString(ViewState["ShibbolethAttributeId"]);
                }
                return String.Empty;
            }
            set
            {
                ViewState["ShibbolethAttributeId"] = value;
            }
        }
        String IITSUserRegistrationView.ShibbolethFirstName
        {
            get
            {
                if (!ViewState["ShibbolethFirstName"].IsNullOrEmpty())
                {
                    return Convert.ToString(ViewState["ShibbolethFirstName"]);
                }
                return String.Empty;
            }
            set
            {
                ViewState["ShibbolethFirstName"] = value;
            }
        }
        String IITSUserRegistrationView.ShibbolethLastName
        {
            get
            {
                if (!ViewState["ShibbolethLastName"].IsNullOrEmpty())
                {
                    return Convert.ToString(ViewState["ShibbolethLastName"]);
                }
                return String.Empty;
            }
            set
            {
                ViewState["ShibbolethLastName"] = value;
            }
        }
        String IITSUserRegistrationView.ShibbolethUserName
        {
            get
            {
                if (!ViewState["ShibbolethUserName"].IsNullOrEmpty())
                {
                    return Convert.ToString(ViewState["ShibbolethUserName"]);
                }
                return String.Empty;
            }
            set
            {
                ViewState["ShibbolethUserName"] = value;
            }
        }
        String IITSUserRegistrationView.ShibbolethHandlerType
        {
            get
            {
                if (!ViewState["ShibbolethHandlerType"].IsNullOrEmpty())
                {
                    return Convert.ToString(ViewState["ShibbolethHandlerType"]);
                }
                return String.Empty;
            }
            set
            {
                ViewState["ShibbolethHandlerType"] = value;
            }
        }
        String IITSUserRegistrationView.ShibbolethHost
        {
            get
            {
                if (!ViewState["ShibbolethHost"].IsNullOrEmpty())
                {
                    return Convert.ToString(ViewState["ShibbolethHost"]);
                }
                return String.Empty;
            }
            set
            {
                ViewState["ShibbolethHost"] = value;
            }
        }
        List<String> IITSUserRegistrationView.lstShibbolethRole
        {
            get
            {
                if (!ViewState["lstShibbolethRole"].IsNullOrEmpty())
                {
                    return ViewState["lstShibbolethRole"] as List<String>;
                }
                return new List<String>();
            }
            set
            {
                ViewState["lstShibbolethRole"] = value;
            }
        }
        String IITSUserRegistrationView.ShibbolethEmail
        {
            get
            {
                if (!ViewState["ShibbolethEmail"].IsNullOrEmpty())
                {
                    return Convert.ToString(ViewState["ShibbolethEmail"]);
                }
                return String.Empty;
            }
            set
            {
                ViewState["ShibbolethEmail"] = value;
            }
        }
        Int32 IITSUserRegistrationView.ShibbolethHostID
        {
            get
            {
                if (!ViewState["ShibbolethHostID"].IsNullOrEmpty())
                {
                    return Convert.ToInt32(ViewState["ShibbolethHostID"]);
                }
                return AppConsts.NONE;
            }
            set
            {
                ViewState["ShibbolethHostID"] = value;
            }
        }
        #endregion

        public Boolean ProceedToAccountLinking
        {
            get
            {
                return ViewState["ProceedToAccountLinking"].IsNull() ? false : Convert.ToBoolean(ViewState["ProceedToAccountLinking"]);
            }
            set
            {
                ViewState["ProceedToAccountLinking"] = value;
            }
        }


        //UAT-2958
        Boolean IITSUserRegistrationView.IsRandomGeneratedPassword
        {
            get
            {
                if (!ViewState["IsRandomGeneratedPassword"].IsNullOrEmpty())
                {
                    return Convert.ToBoolean(ViewState["IsRandomGeneratedPassword"]);
                }
                return false;
            }
            set
            {
                ViewState["IsRandomGeneratedPassword"] = value;
            }
        }
        //UAT-2958
        String IITSUserRegistrationView.RandomGeneratedPassword
        {
            get
            {
                if (!ViewState["RandomGeneratedPassword"].IsNullOrEmpty())
                {
                    return Convert.ToString(ViewState["RandomGeneratedPassword"]);
                }
                return String.Empty;
            }
            set
            {
                ViewState["RandomGeneratedPassword"] = value;
            }
        }

        //UAT-3067
        String IITSUserRegistrationView.AttributesWithID
        {
            get
            {
                if (!ViewState["AttributesWithID"].IsNullOrEmpty())
                {
                    return Convert.ToString(ViewState["AttributesWithID"]);
                }
                return String.Empty;
            }
            set
            {
                ViewState["AttributesWithID"] = value;
            }
        }

        //UAT-3601
        Boolean IITSUserRegistrationView.IsPasswordRetain
        {
            get
            {
                if (!ViewState["IsPasswordRetain"].IsNullOrEmpty())
                {
                    return Convert.ToBoolean(ViewState["IsPasswordRetain"]);
                }
                return false;
            }
            set
            {
                ViewState["IsPasswordRetain"] = value;
            }
        }

        //UAT-3540
        String IITSUserRegistrationView.ShibbolethRoleString
        {
            get
            {
                if (!ViewState["ShibbolethRoleString"].IsNullOrEmpty())
                {
                    return Convert.ToString(ViewState["ShibbolethRoleString"]);
                }
                return String.Empty;
            }
            set
            {
                ViewState["ShibbolethRoleString"] = value;
            }
        }

        // CBI || CABS// Release-158
        Boolean IITSUserRegistrationView.IsLocationServiceTenant
        {
            get
            {
                if (!ViewState["IsLocationServiceTenant"].IsNullOrEmpty())
                    return Convert.ToBoolean(ViewState["IsLocationServiceTenant"]);
                return false;
            }
            set
            {
                ViewState["IsLocationServiceTenant"] = value;
                hdnIsLocationTenant.Value = Convert.ToString(value);
            }
        }

        List<lkpSuffix> IITSUserRegistrationView.lstSuffixes
        {
            get
            {
                if (!ViewState["lstSuffixes"].IsNullOrEmpty())
                    return (List<lkpSuffix>)ViewState["lstSuffixes"];
                return new List<lkpSuffix>();
            }
            set
            {
                ViewState["lstSuffixes"] = value;
            }
        }

        Int32? IITSUserRegistrationView.SelectedSuffixID
        {
            get
            {
                if (!cmbSuffix.SelectedValue.IsNullOrEmpty())
                    return Convert.ToInt32(cmbSuffix.SelectedValue);
                return null;
            }
        }

        public String UserSuffix
        {
            get
            {
                return ucPersonAlias.UserSuffix;
            }
            set
            {
                ucPersonAlias.UserSuffix = value;
            }
        }

        public List<lkpLanguage> CommLanguage
        {
            set
            {
                cmbCommLang.DataSource = value;
                cmbCommLang.DataBind();

            }

        }
        Int32? IITSUserRegistrationView.SelectedCommLang
        {
            get
            {
                if (ViewState["SelectedCommLang"] != null)
                {
                    return (int)ViewState["SelectedCommLang"];
                }
                else
                {
                    return (int?)null;
                }
            }
            set
            {
                ViewState["SelectedCommLang"] = value;
            }
        }

        String IITSUserRegistrationView.UsernameAlreadyInUse
        {
            get
            {
                return Resources.Language.USERNAMEALREADYINUSE;
            }
        }

        String IITSUserRegistrationView.EmailIdAlreadyInUse
        {
            get
            {
                return Resources.Language.EMAILALREADYINUSE;
            }
        }

        String IITSUserRegistrationView.TryAgain
        {
            get
            {
                return Resources.Language.TRYAGAIN;
            }
        }

        String IITSUserRegistrationView.FromText
        {
            get
            {
                return Resources.Language.FROM;
            }
        }

        String IITSUserRegistrationView.IamText
        {
            get
            {
                return Resources.Language.IAM;
            }
        }

        String IITSUserRegistrationView.InvalidUsernamePswd
        {
            get
            {
                return Resources.Language.INCRTUSERNAMEPSWD;
            }
        }

        String IITSUserRegistrationView.LanguageCode
        {
            get
            {
                LanguageContract currentLanguage = LanguageTranslateUtils.GetCurrentLanguageFromSession();
                if (!currentLanguage.IsNullOrEmpty())
                {
                    return currentLanguage.LanguageCode;
                }
                return Languages.ENGLISH.GetStringValue();
            }
        }

        #endregion

        #region Private Properties

        #region UAT-2169:Send Middle Name and Email address to clearstar in Complio

        public String NoMiddleNameText
        {
            get
            {
                String noMiddleNameText = String.Empty;
                if (!CurrentViewContext.IsLocationServiceTenant)  // Check for location service tenant, if it is a loaction service tenant then no middle name text should be blank rather than '-----'.
                    noMiddleNameText = WebConfigurationManager.AppSettings[AppConsts.NO_MIDDLE_NAME_TEXT_KEY];
                if (noMiddleNameText.IsNull())
                {
                    noMiddleNameText = String.Empty;
                }
                return noMiddleNameText;
            }
        }
        #endregion
        #endregion

        #endregion

        #region Events

        /// <summary>
        /// Page_Load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {

                //Set MinDate and MaxDate for DOB
                dpkrDOB.MinDate = Convert.ToDateTime("01-01-1900");
                dvRetypeScndryEmail.Attributes["title"] = Resources.Language.RTYPEADDTNLEMAILADDRESS;
                dvRetypeEmail.Attributes["title"] = Resources.Language.RTYPEMAILADDRS;
                dvSecondEmail.Attributes["title"] = Resources.Language.SCNDEMAILTOINCLUDE;
                rngvDOB.MaximumValue = DateTime.Now.Date.AddYears(-1).ToShortDateString();
                rngvDOB.MinimumValue = Convert.ToDateTime("01-01-1900").ToShortDateString();
                //rngvDOB.ErrorMessage = "Date of birth should not be less than a year.";
                rngvDOB.ErrorMessage = Resources.Language.DOBNOTLESSTHANYEAR;

                //UAT-1848
                RangeValidator1.MaximumValue = DateTime.Now.Date.AddYears(-1).ToShortDateString();
                RangeValidator1.MinimumValue = Convert.ToDateTime("01-01-1900").ToShortDateString();

                dpkrDOB.DatePopupButton.Visible = false;
                Presenter.OnViewInitialized();

                CurrentViewContext.WebsiteUrl = Page.Request.ServerVariables.Get("server_name");//   "CBI.complio.com"; //
                Presenter.GetWebsiteTenantId();

                Presenter.IsLocationServiceTenant();
                //  (this.Page as UserRegistration).IsLocationServiceTenant = CurrentViewContext.IsLocationServiceTenant;
                //ManageLanguageTranslation();
                //UAT-3910
                locationTenant.IsLocationServiceTenant = CurrentViewContext.IsLocationServiceTenant;

                if (CurrentViewContext.IsLocationServiceTenant)
                {
                    Presenter.IsDropDownSuffixType();
                    rngvDOB.MaximumValue = DateTime.Now.Date.AddYears(-10).ToShortDateString();
                    //rngvDOB.ErrorMessage = "Date of birth should not be less than 10 year.";
                    rngvDOB.ErrorMessage = Resources.Language.DOBNOTLESSTHAT10YEAR;
                    dpkrDOB.DateInput.ClientEvents.OnKeyPress = "OnKeyPress";
                    pnlCommLang.Style.Add("display", "");

                    //UAT-3860
                    dvAddress2.Visible = false;
                    //lblAddress1.Text = "Address";
                    lblAddress1.Text = Resources.Language.ADDRESS;
                    //UAT-4312
                    specialchar.Visible = false;

                    //RequiredFieldValidator14.ErrorMessage = "Address is required.";
                    //rfvAddress1.ErrorMessage = "Address is required.";
                    RequiredFieldValidator14.ErrorMessage = Resources.Language.ADDRESSREQ;
                    rfvAddress1.ErrorMessage = Resources.Language.ADDRESSREQ;
                }
                else
                {
                    lblAddress1.Text = Resources.Language.ADDRESS1;
                    RequiredFieldValidator14.ErrorMessage = Resources.Language.ADDRESS1REQ;
                    rfvAddress1.ErrorMessage = Resources.Language.ADDRESS1REQ;
                    //UAT-4312
                    specialcharcbi.Visible = false;
                }
                //Release 158 CBI
                ucPersonAlias.SelectedTenantId = CurrentViewContext.SelectedTenantId;
                ucPersonAlias.PageType = PersonAliasPageType.ITSUserRegistration.GetStringValue();

                if (CurrentViewContext.SelectedTenantId == SecurityManager.DefaultTenantID)
                {
                    pnl.Visible = true;
                    Presenter.GetTenants();
                    cmbOrganization.Focus();
                }
                else
                {
                    txtUsername.Focus();
                    Presenter.GetClientSettings();
                }

                Presenter.GetSSNSetting();
                if (CurrentViewContext.IsLocationServiceTenant)
                {
                    dvFirstName.Style.Add("Width", "28%");
                    dvSpnFirstName.Style.Add("Width", "40%");
                    txtFirstName.Width = 140;

                    dvMiddleName.Style.Add("Width", "28%");
                    dvSpnMiddleName.Style.Add("Width", "40%");
                    txtMiddleName.Width = 140;
                    // txtMiddleName.Style.Add("Width", "50%");

                    dvLastName.Style.Add("Width", "28%");
                    dvSpnLastName.Style.Add("Width", "40%");
                    txtLastName.Width = 120;
                    //txtLastName.Style.Add("Width", "50%");

                    dvSuffix.Style.Add("display", "inline-block");
                    dvSuffix.Style.Add("Width", "13%");
                    Presenter.GetSuffixes();
                    if (CurrentViewContext.IsSuffixDropDownType)
                    {
                        cmbSuffix.Visible = true;
                        BindSuffixDropdown();
                        txtSuffix.Visible = false;
                    }
                    else
                    {
                        cmbSuffix.Visible = false;
                        txtSuffix.Visible = true;
                    }
                }
                else
                {
                    dvSuffix.Style.Add("display", "none");
                }
                //Upto here //Suffix Dropdown added for location service Tenant.
                #region UAT-2169:Send Middle Name and Email address to clearstar in Complio
                hdnNoMiddleNameText.Value = NoMiddleNameText;
                #endregion


                Dictionary<String, String> encryptedQueryString = null;
                if (!Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT].IsNull())
                {
                    encryptedQueryString = new Dictionary<String, String>();
                    encryptedQueryString.ToDecryptedQueryString(Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT]);
                }
                #region UAT-2515 : Get External User matching list
                ProceedToAccountLinking = false;
                if (!encryptedQueryString.IsNull())
                {
                    if (encryptedQueryString.ContainsKey("IsExternalUserRegistration"))
                    {
                        if (Convert.ToBoolean(encryptedQueryString["IsExternalUserRegistration"]))
                        {
                            if (!encryptedQueryString["FirstName"].IsNullOrEmpty())
                            {
                                txtFirstName.Text = encryptedQueryString["FirstName"];
                            }
                            if (!encryptedQueryString["LastName"].IsNullOrEmpty())
                            {
                                txtLastName.Text = encryptedQueryString["LastName"];
                            }
                            if (!encryptedQueryString["DOB"].IsNullOrEmpty())
                            {
                                dpkrDOB.SelectedDate = Convert.ToDateTime(encryptedQueryString["DOB"]);
                            }
                            if (!encryptedQueryString["SSN"].IsNullOrEmpty())
                            {
                                txtSSN.Text = encryptedQueryString["SSN"];
                            }
                            if (!encryptedQueryString["UserName"].IsNullOrEmpty())
                            {
                                txtUsername.Text = encryptedQueryString["UserName"];
                            }
                            if (!encryptedQueryString["PrimaryEmail"].IsNullOrEmpty())
                            {
                                txtPrimaryEmail.Text = encryptedQueryString["PrimaryEmail"];
                            }
                            if (!encryptedQueryString["SecondaryEmail"].IsNullOrEmpty())
                            {
                                txtSecondaryEmail.Text = encryptedQueryString["SecondaryEmail"];
                            }
                            if (!encryptedQueryString["PrimaryPhone"].IsNullOrEmpty())
                            {
                                txtPrimaryPhone.Text = encryptedQueryString["PrimaryPhone"];
                            }
                            if (!encryptedQueryString["ExternalID"].IsNullOrEmpty())
                            {
                                CurrentViewContext.ExternalId = encryptedQueryString["ExternalID"];
                            }
                            if (!encryptedQueryString["IntegrationClientId"].IsNullOrEmpty())
                            {
                                CurrentViewContext.IntegrationClientId = Convert.ToInt32(encryptedQueryString["IntegrationClientId"]);
                            }
                            if (!encryptedQueryString["MappingCode"].IsNullOrEmpty())
                            {
                                CurrentViewContext.MappingCode = encryptedQueryString["MappingCode"];
                            }

                            //Call the SP will bind all organizationuserids which are matching
                            Presenter.GetMatchingOrganizationUserDetails();
                            if (CurrentViewContext.matchingUserList.Count > AppConsts.NONE)
                            {
                                hdnShowError.Value = false.ToString();
                                showHideSections(false, true, false);
                                ProceedToAccountLinking = true;
                            }
                        }
                    }
                }
                #endregion

                #region UAT-2792 : UCONN SSO Process
                //if (!encryptedQueryString.IsNull())
                //{
                //    if (encryptedQueryString.ContainsKey("IsShibbolethLogin") && !encryptedQueryString["IsShibbolethLogin"].IsNullOrEmpty())
                //    {
                //        CurrentViewContext.IsShibbolethLogin = Convert.ToBoolean(encryptedQueryString["IsShibbolethLogin"]);
                //    }
                //    if (CurrentViewContext.IsShibbolethLogin)
                //    {
                //        if (encryptedQueryString.ContainsKey("PeopleSoftID") && !encryptedQueryString["PeopleSoftID"].IsNullOrEmpty())
                //        {
                //            CurrentViewContext.PeopleSoftID = encryptedQueryString["PeopleSoftID"];
                //        }
                //        if (encryptedQueryString.ContainsKey("NetID") && !encryptedQueryString["NetID"].IsNullOrEmpty())
                //        {
                //            CurrentViewContext.NetID = encryptedQueryString["NetID"];
                //        }
                //        if (encryptedQueryString.ContainsKey("EmailID") && !encryptedQueryString["EmailID"].IsNullOrEmpty())
                //        {
                //            CurrentViewContext.ShibbolethEmail = encryptedQueryString["EmailID"];
                //        }
                //        if (encryptedQueryString.ContainsKey("Host") && !encryptedQueryString["Host"].IsNullOrEmpty())
                //        {
                //            CurrentViewContext.Host = (encryptedQueryString["Host"]);
                //        }
                //        if (encryptedQueryString.ContainsKey("Role") && !encryptedQueryString["Role"].IsNullOrEmpty())
                //        {
                //            String Role = encryptedQueryString["Role"];
                //            if (!Role.IsNullOrEmpty())
                //            {
                //                CurrentViewContext.lstShibbolethRole = new List<String>(Role.Split(';'));
                //            }
                //        }
                //        if (encryptedQueryString.ContainsKey("IntegrationClientID") && !encryptedQueryString["IntegrationClientID"].IsNullOrEmpty())
                //        {
                //            CurrentViewContext.IntegrationClientId = Convert.ToInt32(encryptedQueryString["IntegrationClientID"]);
                //        }
                //        CurrentViewContext.ShibbolethHostID = Presenter.GetTenantIDByURL(CurrentViewContext.Host);


                //        //Call the SP will bind all organizationuserids which are matching
                //        Boolean isApplicant = false;
                //        if (!CurrentViewContext.lstShibbolethRole.IsNullOrEmpty() && ISStudent())
                //        {
                //            isApplicant = true;
                //            //call matching Sp and all other functionality
                //            CallMatchingLogicForSSO(isApplicant);
                //        }
                //        else
                //        {
                //            if (!CurrentViewContext.lstShibbolethRole.IsNullOrEmpty() && ISClientAdmin())
                //            {
                //                CallMatchingLogicForSSO(isApplicant);
                //            }
                //            else
                //            {
                //                showHideSections(false, false, false);
                //                dvShibbonethRoleErrorMessage.Style.Add("display", "block");
                //            }
                //        }
                //        //  CallMatchingLogicForSSO(isApplicant);
                //        txtUsername.Text = CurrentViewContext.NetID;
                //        txtPrimaryEmail.Text = CurrentViewContext.ShibbolethEmail.IsNullOrEmpty() ? String.Empty : CurrentViewContext.ShibbolethEmail;
                //        txtConfrimPrimayEmail.Text = CurrentViewContext.ShibbolethEmail.IsNullOrEmpty() ? String.Empty : CurrentViewContext.ShibbolethEmail;
                //    }
                //}
                #endregion


                //UAT-2883
                if (!encryptedQueryString.IsNull())
                {
                    if (encryptedQueryString.ContainsKey("IsShibbolethLogin") && !encryptedQueryString["IsShibbolethLogin"].IsNullOrEmpty())
                    {
                        CurrentViewContext.IsShibbolethLogin = Convert.ToBoolean(encryptedQueryString["IsShibbolethLogin"]);
                    }
                    if (CurrentViewContext.IsShibbolethLogin)
                    {
                        if (encryptedQueryString.ContainsKey("TokenKey") && !encryptedQueryString["TokenKey"].IsNullOrEmpty())
                        {
                            String key = encryptedQueryString["TokenKey"];
                            if (!key.IsNullOrEmpty())
                            {
                                Presenter.GetWebApplicationData(key);
                            }
                            CurrentViewContext.ShibbolethHostID = Presenter.GetTenantIDByURL(CurrentViewContext.ShibbolethHost);


                            //Call the SP will bind all organizationuserids which are matching
                            Boolean isApplicant = false;
                            if ((!CurrentViewContext.lstShibbolethRole.IsNullOrEmpty() || !CurrentViewContext.ShibbolethRoleString.IsNullOrEmpty()) && ISStudent())
                            {
                                isApplicant = true;
                                //call matching Sp and all other functionality
                                CallMatchingLogicForSSO(isApplicant);
                            }
                            else
                            {
                                if ((!CurrentViewContext.lstShibbolethRole.IsNullOrEmpty() || !CurrentViewContext.ShibbolethRoleString.IsNullOrEmpty()) && IsClientAdmin())
                                {
                                    CallMatchingLogicForSSO(isApplicant);
                                }
                                else
                                {
                                    showHideSections(false, false, false);
                                    dvShibbonethRoleErrorMessage.Style.Add("display", "block");
                                }
                            }
                            if (!CurrentViewContext.ShibbolethHandlerType.IsNullOrEmpty() && CurrentViewContext.ShibbolethHandlerType.ToLower() == AppConsts.SHIBBOLETH_UCONN)
                            {
                                txtUsername.Text = CurrentViewContext.ShibbolethAttributeId;
                                //UAT-3133
                                txtPrimaryEmail.Enabled = false;
                                txtConfrimPrimayEmail.Enabled = false;
                                txtUsername.Enabled = false;
                            }
                            else if (!CurrentViewContext.ShibbolethHandlerType.IsNullOrEmpty() && CurrentViewContext.ShibbolethHandlerType.ToLower() == AppConsts.SHIBBOLETH_WGU)
                            {
                                txtUsername.Text = CurrentViewContext.ShibbolethUserName;
                                txtFirstName.Text = CurrentViewContext.ShibbolethFirstName;
                                txtLastName.Text = CurrentViewContext.ShibbolethLastName;
                            }
                            //UAT-3272
                            else if (!CurrentViewContext.ShibbolethHandlerType.IsNullOrEmpty() && (CurrentViewContext.ShibbolethHandlerType == AppConsts.SHIBBOLETH_UPENN || CurrentViewContext.ShibbolethHandlerType == AppConsts.SHIBBOLETH_UPENN_DENTAL))
                            {
                                txtUsername.Text = CurrentViewContext.ShibbolethUniqueIdentifier;
                                txtFirstName.Text = CurrentViewContext.ShibbolethFirstName;
                                txtLastName.Text = CurrentViewContext.ShibbolethLastName;
                            }
                            //UAT-3540
                            else if (!CurrentViewContext.ShibbolethHandlerType.IsNullOrEmpty() && CurrentViewContext.ShibbolethHandlerType == AppConsts.SHIBBOLETH_NYU)
                            {
                                txtUsername.Text = CurrentViewContext.ShibbolethUniqueIdentifier;
                                txtFirstName.Text = CurrentViewContext.ShibbolethFirstName;
                                txtLastName.Text = CurrentViewContext.ShibbolethLastName;
                            }
                            //Release 175 NSC SSO
                            else if (!CurrentViewContext.ShibbolethHandlerType.IsNullOrEmpty() && CurrentViewContext.ShibbolethHandlerType == AppConsts.SHIBBOLETH_NSC)
                            {
                                txtUsername.Text = CurrentViewContext.ShibbolethUserName;
                                txtFirstName.Text = CurrentViewContext.ShibbolethFirstName;
                                txtLastName.Text = CurrentViewContext.ShibbolethLastName;
                            }
                            //UAT-4694  Ball State SSO Dev
                            else if (!CurrentViewContext.ShibbolethHandlerType.IsNullOrEmpty() && CurrentViewContext.ShibbolethHandlerType == AppConsts.SHIBBOLETH_BALL_STATE)
                            {
                                txtFirstName.Text = CurrentViewContext.ShibbolethFirstName;
                                txtLastName.Text = CurrentViewContext.ShibbolethLastName;
                            }
                            //Release 181:4998
                            else if (!CurrentViewContext.ShibbolethHandlerType.IsNullOrEmpty() && CurrentViewContext.ShibbolethHandlerType == AppConsts.SHIBBOLETH_ROSS)
                            {
                                txtFirstName.Text = CurrentViewContext.ShibbolethFirstName;
                                txtLastName.Text = CurrentViewContext.ShibbolethLastName;
                                txtUsername.Text = CurrentViewContext.ShibbolethUserName;
                            }
                            txtPrimaryEmail.Text = CurrentViewContext.ShibbolethEmail.IsNullOrEmpty() ? String.Empty : CurrentViewContext.ShibbolethEmail;
                            txtConfrimPrimayEmail.Text = CurrentViewContext.ShibbolethEmail.IsNullOrEmpty() ? String.Empty : CurrentViewContext.ShibbolethEmail;
                            //UAT-2958
                            Presenter.IsRandomGeneratedPassword();
                        }
                    }
                }
                //UAT-2447
                rfvTxtMobilePrmyNonMasking.Enabled = false;
                ShowHidePhoneNumberControls();
            }
            Presenter.OnViewLoaded();
            ValidateMiddleName();
            ValidateSSN();
            msgBox.Attributes.CssStyle.Add("display", "block");
            divSSN.Visible = !(CurrentViewContext.IsSSNDisabled);
            UserFirstName = txtFirstName.Text;
            UserLastName = txtLastName.Text;
            //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
            UserMiddleName = txtMiddleName.Text;
            lblUserNameMessage.Text = String.Empty;
            //CBI || CABS
            if (CurrentViewContext.IsLocationServiceTenant)
            {
                //if (!CurrentViewContext.lstSuffixes.IsNullOrEmpty() && !cmbSuffix.SelectedValue.IsNullOrEmpty())
                //{
                //    Int32 selectedSuffixId = Convert.ToInt32(cmbSuffix.SelectedValue);
                //    UserSuffix = CurrentViewContext.lstSuffixes.Where(cond => cond.SuffixID == selectedSuffixId).FirstOrDefault().Suffix;
                //}
                UserSuffix = txtSuffix.Text;
                revAddress1.ErrorMessage = Resources.Language.ADDRESSASCIIINVALIDCODE;
                revAddress1.Enabled = true;
                RegularExpressionValidator6.ErrorMessage = Resources.Language.ADDRESSASCIIINVALIDCODE;
                RegularExpressionValidator6.Enabled = true;
            }
            //lblCaptchaErrMsg.Text = String.Empty;
            #region "Hide Residential History for CBI Tenant Applicant"
            ValidatePersonalInformation(CurrentViewContext.IsLocationServiceTenant);
            #endregion
            //UAT-2958
            if (CurrentViewContext.IsRandomGeneratedPassword)
            {
                rfvPassword.Enabled = false;
                revNewPassword.Enabled = false;
                rfvConfirmPassword.Enabled = false;
                RequiredFieldValidator1.Enabled = false;
                RequiredFieldValidator4.Enabled = false;
                RequiredFieldValidator5.Enabled = false;
                RegularExpressionValidator2.Enabled = false;

                dvPasswordSection.Style["display"] = "none";
                Presenter.GenerateRandomPassword();
                txtPassword.Text = CurrentViewContext.RandomGeneratedPassword;
            }

            if (CurrentViewContext.SelectedTenantId > AppConsts.ONE)
            {
                caProfileCustomAttributes.TenantId = CurrentViewContext.SelectedTenantId;
                caProfileCustomAttributes.TypeCode = CustomAttributeUseTypeContext.Profile.GetStringValue();
                //customAttribute.CurrentLoggedInUserId = CurrentViewContext.loggedInUserId;
                caProfileCustomAttributes.DataSourceModeType = DataSourceMode.Ids;
                caProfileCustomAttributes.ControlDisplayMode = DisplayMode.Controls;
                //caProfileCustomAttributes.ValidationGroup = "grpFormSubmit";

                //UAT-2792
                //if (!CurrentViewContext.ShibbolethHandlerType.IsNullOrEmpty() && CurrentViewContext.ShibbolethHandlerType.ToLower() == AppConsts.SHIBBOLETH_UCONN)
                //{
                //    caProfileCustomAttributes.NetIDAttributeValue = CurrentViewContext.ShibbolethAttributeId;
                //    caProfileCustomAttributes.PeopleSoftAttributeValue = CurrentViewContext.ShibbolethUniqueIdentifier;//UAT-3067
                //}
                //else if (!CurrentViewContext.ShibbolethHandlerType.IsNullOrEmpty() && CurrentViewContext.ShibbolethHandlerType.ToLower() == AppConsts.SHIBBOLETH_WGU)
                //{
                //    caProfileCustomAttributes.StudentIdAttributeValue = CurrentViewContext.ShibbolethAttributeId;
                //}


                //UAT-3067
                if (!CurrentViewContext.ShibbolethHandlerType.IsNullOrEmpty() && !CurrentViewContext.AttributesWithID.IsNullOrEmpty())
                {
                    // Dictionary<Int32, String> dicAttributesWithID = new Dictionary<Int32, String>();
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    Dictionary<String, String> dicAttributesWithID = js.Deserialize<Dictionary<String, String>>(CurrentViewContext.AttributesWithID);
                    if (!dicAttributesWithID.IsNullOrEmpty())
                        caProfileCustomAttributes.dictShibbolethCustomAttributes = dicAttributesWithID;
                }
                //UAT-3133
                if (!CurrentViewContext.ShibbolethHandlerType.IsNullOrEmpty())
                {
                    caProfileCustomAttributes.ShibbolethHandlerType = CurrentViewContext.ShibbolethHandlerType;
                }
            }
            else
            {
                caProfileCustomAttributes.Visible = false;
            }
            ucPersonAlias.HasGreyBackground = true;
            if (CurrentViewContext.IsPasswordRetain)
            {
                if (!txtPassword.Text.IsNullOrEmpty())
                    hdnPassword.Value = txtPassword.Text;
                if (!txtConfirmPassword.Text.IsNullOrEmpty())
                    hdnConfirmPassword.Value = txtConfirmPassword.Text;
            }
        }

        /// <summary>
        /// Page_PreRender event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                // To setup client-side input processing
                // To set UserInputClientID property of BotDetect Captcha
                //bdCaptcha.UserInputClientID = txtCaptchaInput.ClientID;
            }
            //on PostBack these div loose this Attribute
            HtmlGenericControl dvLblRSLCounty = this.locationTenant.FindControl("dvLblRSLCounty") as HtmlGenericControl;
            if (dvLblRSLCounty != null)
            {
                dvLblRSLCounty.Style.Add("display", "inline-block");
            }
            HtmlGenericControl dvCntRSLCounty = this.locationTenant.FindControl("dvCntRSLCounty") as HtmlGenericControl;
            if (dvCntRSLCounty != null)
            {
                dvCntRSLCounty.Style.Add("display", "inline-block");
            }
        }

        protected void btnCheckUsername_Click(object sender, EventArgs e)
        {
            //String reg = @"^[\w\d\s\-\.\@\+]{4,50}$";
            //String reg = @"^[\.\@a-z0-9_-]{4,50}$";

            //if (String.IsNullOrEmpty(CurrentViewContext.UserName.Trim()))
            //{
            //    lblUserNameMessage.Text = "Username is required.";
            //    lblUserNameMessage.ForeColor = System.Drawing.Color.Red;

            //}
            //else if (Regex.IsMatch(CurrentViewContext.UserName, reg) == false)
            //{
            //    lblUserNameMessage.Text = "Invalid username. Must have at least 4 chars (a-z 0-9 . _ - @).";
            //    //lblUserNameMessage.Text = SysXUtils.GetMessage(ResourceConst.SECURITY_INVALID_CHARACTER);
            //    lblUserNameMessage.ForeColor = System.Drawing.Color.Red;

            //}
            //else
            //{
            if (Presenter.IsExistsUserName())
            {
                //lblUserNameMessage.Text = "This Username is not available. Try another?";
                //lblUserNameMessage.Text = "This Username is not available. Try another?";
                lblUserNameMessage.Text = Resources.Language.USERNAMEALREADYINUSE;
                lblUserNameMessage.ForeColor = System.Drawing.Color.Red;

            }
            else
            {
                //lblUserNameMessage.Text = "This Username is available.";
                lblUserNameMessage.Text = Resources.Language.USERNAMEAVLBLE;
                lblUserNameMessage.ForeColor = System.Drawing.Color.Green;
            }
            //UAT-2447
            ShowHidePhoneNumberControls();
            txtUsername.Focus();
        }

        protected void fsucCmdBar1_SubmitClick(object sender, EventArgs e)
        {
            try
            {
                ProceedToAccountLinking = false;
                CurrentViewContext.IsAutoActive = Presenter.IsAutoActivateAndLogin();

                CurrentViewContext.SelectedCommLang = Convert.ToInt32(cmbCommLang.SelectedValue);

                if (String.Equals(hdnIsGoogleRecaptchaVerified.Value, AppConsts.ZERO))
                {
                    //ErrorMessage = "Captcha is invalid, please retry.";
                    ErrorMessage = Resources.Language.INVALIDCAPTCHA;
                    return;
                }
                //if (ValidateUserName()) 
                //{
                //    return;
                //}
                ShowHidePhoneNumberControls();
                if (!String.IsNullOrEmpty(txtSecondaryEmail.Text.Trim()) && String.IsNullOrEmpty(txtConfirmSecEmail.Text.Trim()))
                {
                    //ErrorMessage = "Secondary Confirm Email Address is required.";
                    ErrorMessage = Resources.Language.SCNDRYCNFRMEMAILADDRSREQ;
                    return;
                }

                if (!ucPersonAlias.NewFirstNameAlias.IsNullOrEmpty())
                {
                    Regex regexFirstName = null;
                    if (CurrentViewContext.IsLocationServiceTenant)
                    {
                        regexFirstName = new Regex(@"^[a-zA-Z ]{1,30}$");
                    }
                    else
                    {
                        regexFirstName = new Regex(@"^[a-zA-Z ]{1,30}$");
                    }

                    if (!regexFirstName.IsMatch(ucPersonAlias.NewFirstNameAlias))
                    {
                        ErrorMessage = "Alias/Maiden First Name must be between 1 to 30 characters and must contains letters.";
                        return;
                    }
                }

                //Implementation for: Based on CBI's response below we need to allow for only one character per name (first, middle, last).
                //The scenario we have to think about is the one where the applicant opts to provide no middle name with only one character for the first and last name.

                if (!txtFirstName.IsNullOrEmpty() || !txtMiddleName.IsNullOrEmpty() || !txtLastName.IsNullOrEmpty() || !txtSuffix.IsNullOrEmpty())
                {
                    if (CurrentViewContext.IsLocationServiceTenant)
                    {
                        Int32 suffixLength = CurrentViewContext.IsSuffixDropDownType ? cmbSuffix.SelectedItem.Index>0 ? cmbSuffix.SelectedItem.Text.Length : 0 : txtSuffix.Text.Length;
                        Int32 totalLength = (txtFirstName.Text.Length) + (txtMiddleName.Text.Length) + (txtLastName.Text.Length) + suffixLength;
                        if (totalLength < AppConsts.THREE)
                        {
                            //ErrorMessage = "Total length for Full Name should be atleast 3 characters.";
                            ErrorMessage = Resources.Language.TTLLENFULLNAMEATLEASTTHREECHARS;
                            return;
                        }
                    }
                }


                //if (!ucPersonAlias.NewMiddleNameAlias.IsNullOrEmpty())
                //{
                //    Regex regex = new Regex(@"^[a-zA-Z]{1,30}$");
                //    if (!regex.IsMatch(ucPersonAlias.NewMiddleNameAlias))
                //    {
                //        ErrorMessage = "Alias/Maiden Middle Name must be between 1 to 30 characters and can contains letters.";
                //        return;
                //    }
                //}
                if (!ucPersonAlias.NewLastNameAlias.IsNullOrEmpty())
                {
                    Regex regexFirstName = new Regex(@"^[a-zA-Z-]{1,30}$");
                    if (CurrentViewContext.IsLocationServiceTenant)
                    {
                        regexFirstName = new Regex(@"^([a-zA-Z]+)?-?([a-zA-Z]+)?$");
                    }
                    else
                    {
                        regexFirstName = new Regex(@"^[a-zA-Z- ]{1,30}$");
                    }

                    if (!regexFirstName.IsMatch(ucPersonAlias.NewLastNameAlias))
                    {
                        ErrorMessage = "Alias/Maiden Last Name must be between 1 to 30 characters and must contains letters, hyphen(-).";
                        return;
                    }
                }

                if (ucPersonAlias.NewFirstNameAlias.IsNullOrEmpty() && !ucPersonAlias.NewLastNameAlias.IsNullOrEmpty())
                {
                    ErrorMessage = Resources.Language.ALIASFRSTNMEREQIFLSTNME;//"Alias/Maiden First Name is required if Alias/Maiden Last Name is entered.";
                    return;
                }

                if (!ucPersonAlias.NewFirstNameAlias.IsNullOrEmpty() && ucPersonAlias.NewLastNameAlias.IsNullOrEmpty())
                {
                    ErrorMessage = Resources.Language.ALIASLSTNMEREQIFFRSTNME;//"Alias/Maiden Last Name is required if Alias/Maiden First Name is entered.";
                    return;
                }

                if (ucPersonAlias.HasDuplicateNames)
                {

                    //ErrorMessage = "Duplicate names cannot be added.";
                    ErrorMessage = Resources.Language.DPLNAMECNTADD;
                    return;
                }

                //Commented code to implement new BotDetect Captcha
                /* if (!radCpatchaPassword.IsValid)
                {
                    lblCaptchaErrMsg.Text = "Verification code is invalid, please re-enter";
                    lblCaptchaErrMsg.ForeColor = System.Drawing.Color.Red;

                    // msgBox.Attributes.CssStyle.Add("display", "none");
                    ErrorMessage = radCpatchaPassword.ErrorMessage;

                    return;
                } */

                //if (!Validate())
                //{
                //    //lblCaptchaErrMsg.Text = "Verification Code is not valid. Please re-enter.";
                //    //lblCaptchaErrMsg.ForeColor = System.Drawing.Color.Red;
                //    ErrorMessage = "Verification Code you entered is not valid. Please re-enter.";
                //    return;
                //}

                if (Presenter.IsExistingUser() && ShowExistingUsers)
                {
                    ZipId = 0;
                    showHideSections(false, true, false);
                }
                else if (Presenter.IsExistsUserName())
                {
                    //ErrorMessage = "Account with username '" + CurrentViewContext.UserName + "' already exists.";
                    ErrorMessage = Resources.Language.ACCWITHUSERNAME + CurrentViewContext.UserName + Resources.Language.ALREADYEXISTS;
                    showHideSections(true, false, false);
                    return;
                }
                else
                {
                    showHideSections(true, false, false);
                    aspnet_Users user = SecurityManager.GetUserByName(CurrentViewContext.UserName, true, true);
                    if (user.IsNullOrEmpty())
                    {
                        NormalRegistration();
                    }
                    else
                    {
                        //ErrorMessage = "Account with username '" + CurrentViewContext.UserName + "' already exists.";
                        ErrorMessage = Resources.Language.ACCWITHUSERNAME + CurrentViewContext.UserName + Resources.Language.ALREADYEXISTS;
                        showHideSections(true, false, false);
                        return;
                    }



                }
                //on PostBack these div loose this Attribute
                HtmlGenericControl dvLblRSLCounty = this.locationTenant.FindControl("dvLblRSLCounty") as HtmlGenericControl;
                if (dvLblRSLCounty != null)
                {
                    dvLblRSLCounty.Style.Add("display", "inline-block");
                }
                HtmlGenericControl dvCntRSLCounty = this.locationTenant.FindControl("dvCntRSLCounty") as HtmlGenericControl;
                if (dvCntRSLCounty != null)
                {
                    dvCntRSLCounty.Style.Add("display", "inline-block");
                }

                //if (AutoLogin())
                //{
                //    Dictionary<String, String> queryString = new Dictionary<String, String>
                //                                                 { 
                //                                                    { AppConsts.CHILD, AppConsts.APPLICANT_LANDING_PAGE_CONTROL_NAME},
                //                                                    { AppConsts.NEW_REGISTERED_USER, AppConsts.TRUE}
                //                                                 };
                //    string url = String.Format("~/Main/Default.aspx?args={0}", queryString.ToEncryptedQueryString());
                //    System.Web.HttpContext.Current.Response.Redirect(url);
                //}


            }
            //Do not log thread abort exception if it is caused by Response.Redirect or Response.End
            catch (ThreadAbortException thex)
            {
                //You can ignore this 
            }
            catch (Exception ex)
            {
                //LoginErrorMessage = "An unknown exception occurred while connecting to the server. The possible reasons could be- 1) Expiration of Session or 2) Injection of Special character(s), where not allowed (for example using <> in filters or forms) or 3) Internet Connection failure. Please Re-login, remove special characters or check internet connection. If problem persists contact System Admin.";
                LoginErrorMessage = Resources.Language.LOGINEXCEPTION;
                //setSubmitbuttonText = "Try Again"; 
                setSubmitbuttonText = Resources.Language.TRYAGAIN;
            }

        }

        protected void fsucCmdBar1_CancelClick(object sender, EventArgs e)
        {
            Response.Redirect("~/Login.aspx");
        }

        protected void cmbOrganization_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(cmbOrganization.SelectedValue))
            {
                CurrentViewContext.SelectedTenantId = Convert.ToInt32(cmbOrganization.SelectedValue);
                Presenter.IsLocationServiceTenant();
                Presenter.GetSSNSetting();
                divSSN.Visible = !(CurrentViewContext.IsSSNDisabled);
            }
            else
            {
                divSSN.Visible = true;
            }

            cmbOrganization.Focus();
            if (!String.IsNullOrEmpty(cmbOrganization.SelectedValue))
            {
                caProfileCustomAttributes.TenantId = CurrentViewContext.SelectedTenantId;
                caProfileCustomAttributes.TypeCode = CustomAttributeUseTypeContext.Profile.GetStringValue();
                //customAttribute.CurrentLoggedInUserId = CurrentViewContext.loggedInUserId;
                caProfileCustomAttributes.DataSourceModeType = DataSourceMode.Ids;
                caProfileCustomAttributes.ControlDisplayMode = DisplayMode.Controls;
                caProfileCustomAttributes.ValidationGroup = "grpFormSubmit";
            }
            //UAT-2447
            ShowHidePhoneNumberControls();
        }

        #endregion

        #region Methods

        private bool AutoLogin()
        {
            CoreWeb.Shell.Views.LoginPresenter loginPresenter = new Shell.Views.LoginPresenter();
            return loginPresenter.AutoLogIn(CurrentViewContext.UserName, CurrentViewContext.Password);
        }

        private void UploadAttachment()
        {
            if (uploadControl.UploadedFiles.Count > 0)
            {
                Boolean aWSUseS3 = false;
                if (!ConfigurationManager.AppSettings["AWSUseS3"].IsNullOrEmpty())
                {
                    aWSUseS3 = Convert.ToBoolean(ConfigurationManager.AppSettings["AWSUseS3"]);
                }
                List<String> extensions = new List<String>();
                extensions.Add(".jpg");
                extensions.Add(".jpeg");
                extensions.Add(".tiff");
                extensions.Add(".bmp");
                extensions.Add(".bitmap");
                extensions.Add(".png");

                String fileExtension = Path.GetExtension(uploadControl.UploadedFiles[0].FileName);
                if (extensions.Contains(fileExtension.ToLower()))
                {
                    OriginalFileName = uploadControl.UploadedFiles[0].FileName;
                    String fileName = Guid.NewGuid().ToString() + Path.GetExtension(uploadControl.UploadedFiles[0].FileName);

                    String tempFilePath = WebConfigurationManager.AppSettings["TemporaryFileLocation"];
                    if (tempFilePath.IsNullOrEmpty())
                    {
                        base.LogError("Please provide path for TemporaryFileLocation in config", null);
                        return;
                    }
                    if (!tempFilePath.EndsWith(@"\"))
                    {
                        tempFilePath += @"\";
                    }
                    tempFilePath += "Tenant(" + CurrentViewContext.SelectedTenantId.ToString() + @")\" + @"Pics\";
                    if (!Directory.Exists(tempFilePath))
                        Directory.CreateDirectory(tempFilePath);
                    String newTempFilePath = Path.Combine(tempFilePath, fileName);
                    //Upload file
                    uploadControl.UploadedFiles[0].SaveAs(newTempFilePath);

                    //Check whether use AWS S3, true if need to use
                    if (aWSUseS3 == false)
                    {
                        FilePath = WebConfigurationManager.AppSettings[AppConsts.APPLICANT_FILE_LOCATION];
                        if (FilePath.IsNullOrEmpty())
                        {
                            base.LogError("Please provide path for " + AppConsts.APPLICANT_FILE_LOCATION + " in web config", null);
                            return;
                        }
                        if (!FilePath.EndsWith(@"\"))
                            FilePath += @"\";

                        FilePath += "Tenant(" + CurrentViewContext.SelectedTenantId.ToString() + @")\" + @"Pics\";

                        if (!Directory.Exists(FilePath))
                            Directory.CreateDirectory((FilePath));

                        FilePath = Path.Combine(FilePath, fileName);
                        MoveFile(newTempFilePath, FilePath);
                    }
                    else
                    {
                        FilePath = WebConfigurationManager.AppSettings[AppConsts.APPLICANT_FILE_LOCATION];
                        if (FilePath.IsNullOrEmpty())
                        {
                            base.LogError("Please provide path for " + AppConsts.APPLICANT_FILE_LOCATION + " in web config", null);
                            return;
                        }
                        if (!FilePath.EndsWith(@"/"))
                            FilePath += @"/";

                        //AWS code to save document to S3 location
                        AmazonS3Documents objAmazonS3 = new AmazonS3Documents();
                        String destFolder = FilePath + "Tenant(" + CurrentViewContext.SelectedTenantId.ToString() + @")/" + @"Pics/";
                        String returnFilePath = objAmazonS3.SaveDocument(newTempFilePath, fileName, destFolder);
                        try
                        {
                            if (!String.IsNullOrEmpty(newTempFilePath))
                                File.Delete(newTempFilePath);
                        }
                        catch (Exception) { }
                        //UAT-862 :- WB: As a student or an admin, I should not be allowed to upload documents with a size of 0 
                        if (returnFilePath.IsNullOrEmpty())
                        {
                            _isCorruptedFileUploaded = true;
                        }
                        FilePath = returnFilePath; //Path.Combine(destFolder, fileName);
                    }
                }
            }
        }

        /// <summary>
        /// Move file to other location
        /// </summary>
        /// <param name="sourceFilePath"></param>
        /// <param name="destinationFilePath"></param>
        /// <returns></returns>
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

        /// <summary>
        /// To Validate user input for Captcha
        /// </summary>
        /// <returns></returns>
        private bool Validate()
        {
            ////Capture if the code is correct.
            //bool isHuman = bdCaptcha.Validate(txtCaptchaInput.Text);

            ////Clear the text input
            //txtCaptchaInput.Text = string.Empty;

            //return isHuman;
            return true;
        }

        //private Boolean ValidateUserName() 
        //{
        //    String reg = @"^[\.\@a-z0-9_-]{4,50}$";

        //    if (String.IsNullOrEmpty(CurrentViewContext.UserName.Trim()))
        //    {
        //        lblUserNameMessage.Text = "Username is required.";
        //        lblUserNameMessage.ForeColor = System.Drawing.Color.Red;
        //        return true;
        //    }
        //    else if (Regex.IsMatch(CurrentViewContext.UserName, reg) == false)
        //    {
        //        lblUserNameMessage.Text = SysXUtils.GetMessage(ResourceConst.SECURITY_INVALID_CHARACTER);
        //        lblUserNameMessage.ForeColor = System.Drawing.Color.Red;
        //        return true;
        //    }
        //    return false;

        //}


        private void NormalRegistration()
        {
            if (!IsValidAddress())
            {
                if (hdnShowError.Value.ToLower() == "true")//UAT-2515
                    //ErrorMessage = "Please select a valid ZipCode.";
                    ErrorMessage = Resources.Language.PLSSELVALIDZIP;

                return;
            }

            if (Presenter.IsValidateUser())
                UploadAttachment();

            CurrentViewContext.SaveCustomAttributeList = caProfileCustomAttributes.GetCustomAttributeValues();

            if (_isCorruptedFileUploaded)
            {
                //String corruptedFileMessage = "Your profile picture is not uploaded.Please try again. ";
                String corruptedFileMessage = Resources.Language.PROFILEPICNTUPLOADED;
                //base.ShowErrorInfoMessage("Your profile picture is not uploaded.Please try again. ");
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ShowCallBackMessage('" + corruptedFileMessage + "');", true);
            }

            else if (Presenter.AddUser())
            {
                //UAT-2853:UCONN SSO Account activation requirement
                if (CurrentViewContext.IsShibbolethLogin)
                {
                    //Auto Login through Shibboleth
                    Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                                {
                                                                                    { "IsShibbolethLogin", true.ToString()},
                                                                                    { "IsAutoLoginThroughShibboleth", true.ToString()},
                                                                                    {"IntegrationClientID",Convert.ToString(CurrentViewContext.IntegrationClientId)},
                                                                                    {"UserName",txtUsername.Text},
                                                                                    {"Host",CurrentViewContext.ShibbolethHost},
                                                                                    {"TenantID",Convert.ToString(CurrentViewContext.ShibbolethHostID)},
                                                                                    {"IsShibbolethApplicant",Convert.ToString(true)}
                                                                                };
                    string url = String.Format("~/login.aspx?shibbolethArgs={0}", queryString.ToEncryptedQueryString());
                    Response.Redirect(url);
                }
                if (CurrentViewContext.IsAutoActive)
                {


                    Session["IsAutoActivateAndLogin"] = "true";
                    Dictionary<String, String> queryString = new Dictionary<String, String>
                                                             {
                                                                { AppConsts.UserName,CurrentViewContext.UserName }

                                                             };
                    string url = String.Format("~/Login.aspx?autoLoginArgs={0}", queryString.ToEncryptedQueryString());
                    System.Web.HttpContext.Current.Response.Redirect(url);
                    //Response.Redirect("~/Login.aspx");
                }
                else
                {
                    Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 {
                                                                    { AppConsts.CHILD, txtPrimaryEmail.Text},
                                                                    { "IsNewAccount", true.ToString()}
                                                                 };

                    string url = String.Format("~/PostRegistration.aspx?args={0}", queryString.ToEncryptedQueryString());
                    System.Web.HttpContext.Current.Response.Redirect(url);
                }
            }
        }

        /// <summary>
        /// Method to avoid registration with 0 ZipCodeId for USA address - UAT 934
        /// </summary>
        private Boolean IsValidAddress()
        {
            if (locationTenant.MasterZipcodeID == AppConsts.NONE && locationTenant.RSLCountryId == AppConsts.COUNTRY_USA_ID)
                return false;
            return true;
        }

        private void showHideSections(Boolean showUserReg, Boolean showExistUsers, Boolean showPswdVerification)
        {
            dvUserReg.Visible = showUserReg;
            dvExistingProfiles.Visible = showExistUsers;
            dvPswdVerification.Visible = showPswdVerification;
            HtmlGenericControl dvLanguage = Parent.FindControl("dvLanguage") as HtmlGenericControl;
            dvLanguage.Visible = showUserReg;
        }
        #endregion

        #region ExistingProfiles

        protected void cbExistingProfiles_SubmitClick(object sender, EventArgs e)
        {
            if (rblExistingProfiles.SelectedValue == Resources.Language.NONEOFABOVE)
            {
                CurrentViewContext.IsAutoActive = Presenter.IsAutoActivateAndLogin();
                NormalRegistration();
                showHideSections(true, false, false);
                ShowExistingUsers = false;
            }
            else
            {
                txtLoginUserName.Text = rblExistingProfiles.SelectedValue;
                //if (Presenter.IsUsernameExistInTenantDB() && String.IsNullOrEmpty(CurrentViewContext.ExternalId))
                if (Presenter.IsUsernameExistInTenantDB() && ProceedToAccountLinking == false)
                {
                    //ErrorMessage = "Account with username '" + LoginUserName + "' already exist under this institution.";
                    ErrorMessage = Resources.Language.ACCWITHUSERNAME + LoginUserName + " " + Resources.Language.ALREADYEXISTSUNDERINST;
                    showHideSections(true, false, false);
                    return;
                }
                showHideSections(false, false, true);
                cbPswdVerification.HideButtons(CommandBarButtons.Save);
                LoginErrorMessage = String.Empty;
            }
            //on PostBack these div loose this Attribute
            HtmlGenericControl dvLblRSLCounty = this.locationTenant.FindControl("dvLblRSLCounty") as HtmlGenericControl;
            if (dvLblRSLCounty != null)
            {
                dvLblRSLCounty.Style.Add("display", "inline-block");
            }
            HtmlGenericControl dvCntRSLCounty = this.locationTenant.FindControl("dvCntRSLCounty") as HtmlGenericControl;
            if (dvCntRSLCounty != null)
            {
                dvCntRSLCounty.Style.Add("display", "inline-block");
            }

        }

        #endregion

        #region Password Verification

        protected void cbPswdVerification_SubmitClick(object sender, EventArgs e)
        {
            try
            {
                Presenter.ValidateUserNameAndPassword();
                if (ExistingOrganisationUser.IsNotNull())
                {
                    if (!Presenter.IsUsernameExistInTenantDB() && ProceedToAccountLinking == false)
                    {
                        UploadAttachment();
                        if (_isCorruptedFileUploaded)
                        {
                            //String corruptedFileMessage = "Your profile picture is not uploaded.Please try again. ";
                            String corruptedFileMessage = Resources.Language.PROFILEPICNTUPLOADED;
                            //base.ShowErrorInfoMessage("Your profile picture is not uploaded.Please try again. ");
                            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ShowCallBackMessage('" + corruptedFileMessage + "');", true);
                        }
                        else
                        {
                            Presenter.AddLinkedUserProfile();
                        }
                    }
                    else if (ProceedToAccountLinking)
                    {
                        Presenter.InsertAceMappLoginIntegrationEntry(ExistingOrganisationUser.OrganizationUserID);
                    }
                    if (!_isCorruptedFileUploaded)
                    {
                        Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 {
                                                                    { AppConsts.CHILD, txtPrimaryEmail.Text},
                                                                    { "IsNewAccount", false.ToString()}
                                                                 };

                        string url = String.Format("~/PostRegistration.aspx?args={0}", queryString.ToEncryptedQueryString());
                        System.Web.HttpContext.Current.Response.Redirect(url);
                    }

                }
                else
                {
                    PasswordAttemptCount += 1;
                }
            }
            //Do not log thread abort exception if it is caused by Response.Redirect or Response.End
            catch (ThreadAbortException thex)
            {
                //You can ignore this 
            }
            catch (Exception ex)
            {
                LoginErrorMessage = ex.Message;
                //setSubmitbuttonText = "Try Again";
                setSubmitbuttonText = Resources.Language.TRYAGAIN;
            }
        }

        protected void cbPswdVerification_CancelClick(object sender, EventArgs e)
        {
            try
            {
                PasswordAttemptCount = 0;
                showHideSections(true, false, false);
                ErrorMessage = String.Empty;
                return;
            }
            //Do not log thread abort exception if it is caused by Response.Redirect or Response.End
            catch (ThreadAbortException thex)
            {
                //You can ignore this 
            }
            catch (Exception ex)
            {
                LoginErrorMessage = ex.Message;
            }
        }

        protected void cbPswdVerification_SaveClick(object sender, EventArgs e)
        {
            try
            {
                ErrorMessage = String.Empty;
                ShowExistingUsers = false;
                NormalRegistration();
                showHideSections(true, false, false);
                return;
            }
            //Do not log thread abort exception if it is caused by Response.Redirect or Response.End
            catch (ThreadAbortException thex)
            {
                //You can ignore this 
            }
            catch (Exception ex)
            {
                LoginErrorMessage = ex.Message;
            }
        }
        #endregion

        #region
        /// <summary>
        /// UAT-1612 : As an applicant, my middle name should be required.
        /// </summary>
        /// <param name="middleName"></param>
        private void ValidateMiddleName()
        {
            if (chkMiddleNameRequired.Checked)
            {
                if (CurrentViewContext.IsLocationServiceTenant)
                {
                    txtMiddleName.Text = String.Empty;
                }
                else
                {
                    txtMiddleName.Text = NoMiddleNameText;
                }

                txtMiddleName.Enabled = false;
                rfvMiddleName.Enabled = false;
                spnMiddleName.Style["display"] = "none";
                RequiredFieldValidator7.Enabled = false;
                RequiredFieldValidator4.Enabled = false;
            }
            else
            {
                txtMiddleName.Enabled = true;
                rfvMiddleName.Enabled = true;
                spnMiddleName.Style["display"] = "";
                spnMiddleName.Visible = true;
                RequiredFieldValidator7.Enabled = true;
            }
        }
        #endregion

        #region uat-2447
        private void ShowHidePhoneNumberControls()
        {
            if (chkIsMaskingRequiredPrimary.Checked)
            {
                dvUnmasking.Style["display"] = "block";
                dvMasking.Style["display"] = "none";
                revTxtMobile.Enabled = false;
                rfvTxtMobile.Enabled = false;
                rfvTxtMobilePrmyNonMasking.Enabled = true;
                revTxtMobilePrmyNonMasking.Enabled = true;
                RequiredFieldValidator15.Enabled = false;
                RegularExpressionValidator9.Enabled = false;
                RequiredFieldValidator17.Enabled = true;
                RegularExpressionValidator12.Enabled = true;
            }
            else
            {
                dvUnmasking.Style["display"] = "none";
                dvMasking.Style["display"] = "block";
                revTxtMobile.Enabled = true;
                rfvTxtMobile.Enabled = true;
                rfvTxtMobilePrmyNonMasking.Enabled = false;
                revTxtMobilePrmyNonMasking.Enabled = false;
                RequiredFieldValidator15.Enabled = true;
                RegularExpressionValidator9.Enabled = true;
                RequiredFieldValidator17.Enabled = false;
                RegularExpressionValidator12.Enabled = false;

            }
            if (chkIsMaskingRequiredSecondary.Checked)
            {
                dvUnMaskingSecondary.Style["display"] = "block";
                dvMaskingSecondary.Style["display"] = "none";
            }
            else
            {
                dvUnMaskingSecondary.Style["display"] = "none";
                dvMaskingSecondary.Style["display"] = "block";
            }
        }
        #endregion

        #region UAT-2792
        protected void cbShibbonethConfirmation_SubmitClick(object sender, EventArgs e)
        {
            Boolean isShibbolethApplicant = false;
            if (ISStudent())
            {
                isShibbolethApplicant = true;
            }
            //Redirected To login Page if user says that he has an account with complio.
            Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                                {
                                                                                    { "IsShibbolethLogin", true.ToString()},
                                                                                    { "ShibbolethUniqueIdentifier", CurrentViewContext.ShibbolethUniqueIdentifier},
                                                                                    {"IntegrationClientID",Convert.ToString(CurrentViewContext.IntegrationClientId)},
                                                                                    {"IsExistingAccount",false.ToString()},
                                                                                    {"TenantID",Convert.ToString(CurrentViewContext.ShibbolethHostID)},
                                                                                    {"Host",CurrentViewContext.ShibbolethHost},
                                                                                    {"IsShibbolethApplicant",Convert.ToString(isShibbolethApplicant)}
                                                                                };
            string url = String.Format("~/login.aspx?shibbolethArgs={0}", queryString.ToEncryptedQueryString());
            Response.Redirect(url);
        }

        protected void cbShibbonethConfirmation_CancelClick(object sender, EventArgs e)
        {
            dvShibbonethConfirmation.Style.Add("display", "none");

            if (!ISStudent())
            {
                showHideSections(false, false, false);
                dvShibbonethErrorMessage.Style.Add("display", "block");
            }
            else
            {
                showHideSections(true, false, false);
                dvShibbonethErrorMessage.Style.Add("display", "none");
            }

        }

        private bool ISStudent()
        {
            //UAT-3272
            if (CurrentViewContext.ShibbolethHandlerType.ToLower() == AppConsts.SHIBBOLETH_UCONN)
            {
                return Presenter.CheckRoleForShibbolethUCONN(true);
            }
            else if (CurrentViewContext.ShibbolethHandlerType.ToLower() == AppConsts.SHIBBOLETH_WGU)
            {
                return CurrentViewContext.lstShibbolethRole.Contains("student");
            }
            else if (CurrentViewContext.ShibbolethHandlerType.ToLower() == AppConsts.SHIBBOLETH_UPENN )
            {
                return CurrentViewContext.lstShibbolethRole.Contains("compliostudents");
            }
            //UAT-3540
            else if (CurrentViewContext.ShibbolethHandlerType.ToLower() == AppConsts.SHIBBOLETH_NYU)
            {
                return Presenter.CheckRoleForShibbolethNYU(true);
            }
            //Relase 175 NSC SSO
            else if (CurrentViewContext.ShibbolethHandlerType.ToLower() == AppConsts.SHIBBOLETH_NSC)
            {
                return Presenter.CheckRoleForShibbolethNSC(true);
            }
            //Release 181:4998
            else if (CurrentViewContext.ShibbolethHandlerType.ToLower() == AppConsts.SHIBBOLETH_ROSS)
            {
                return Presenter.CheckRoleForShibbolethRoss(true);
            }
            //Upenn Dental SSO Integration
            else if (CurrentViewContext.ShibbolethHandlerType.ToLower() == AppConsts.SHIBBOLETH_UPENN_DENTAL)
            {
                return Presenter.CheckRoleForShibbolethUpennDental(true);
            }
            //UAT-4694 Ball State SSO Dev
            else if (CurrentViewContext.ShibbolethHandlerType.ToLower() == AppConsts.SHIBBOLETH_BALL_STATE)
            {
                return Presenter.CheckRoleForShibbolethBSU(true);
            }
            return false;

        }

        private bool IsClientAdmin()
        {
            if (!CurrentViewContext.ShibbolethHandlerType.IsNullOrEmpty())
            {
                if (CurrentViewContext.ShibbolethHandlerType.ToLower() == AppConsts.SHIBBOLETH_UCONN)
                {
                    //return CurrentViewContext.lstShibbolethRole.Contains("staff") || CurrentViewContext.lstShibbolethRole.Contains("faculty");
                    return Presenter.CheckRoleForShibbolethUCONN(false);
                }
                else if (CurrentViewContext.ShibbolethHandlerType.ToLower() == AppConsts.SHIBBOLETH_WGU)
                {
                    return CurrentViewContext.lstShibbolethRole.Contains("employee");
                }
                //for upenn
                else if (CurrentViewContext.ShibbolethHandlerType.ToLower() == AppConsts.SHIBBOLETH_UPENN)
                {
                    return CurrentViewContext.lstShibbolethRole.Contains("complioadmins");
                }
                //UAT-3540
                else if (CurrentViewContext.ShibbolethHandlerType.ToLower() == AppConsts.SHIBBOLETH_NYU)
                {
                    return Presenter.CheckRoleForShibbolethNYU(false);
                }
                //Relase 175 NSC SSO
                else if (CurrentViewContext.ShibbolethHandlerType.ToLower() == AppConsts.SHIBBOLETH_NSC)
                {
                    return Presenter.CheckRoleForShibbolethNSC(false);
                }
                //Release 181:4998
                else if (CurrentViewContext.ShibbolethHandlerType.ToLower() == AppConsts.SHIBBOLETH_ROSS)
                {
                    return Presenter.CheckRoleForShibbolethRoss(false);
                }
                //Upenn Dental SSO Integration
                else if (CurrentViewContext.ShibbolethHandlerType.ToLower() == AppConsts.SHIBBOLETH_UPENN_DENTAL)
                {
                    return Presenter.CheckRoleForShibbolethUpennDental(false);
                }
                //UAT-4694 Ball State SSO Dev
                else if (CurrentViewContext.ShibbolethHandlerType.ToLower() == AppConsts.SHIBBOLETH_BALL_STATE)
                {
                    return Presenter.CheckRoleForShibbolethBSU(false);
                }
            }
            return false;
        }

        private void CallMatchingLogicForSSO(Boolean isApplicant)
        {
            Presenter.GetMatchingUsersForShibboleth(CurrentViewContext.ShibbolethEmail, CurrentViewContext.ShibbolethHostID, isApplicant);
            if (CurrentViewContext.matchingUserList.Count > AppConsts.NONE)
            {
                CurrentViewContext.matchingUserList.ForEach(x =>
                {
                    if (x.IsFirstLogin)
                    {
                        //login page redirect
                        Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                                {
                                                                                    { "IsShibbolethLogin", true.ToString()},
                                                                                    { "ShibbolethUniqueIdentifier", CurrentViewContext.ShibbolethUniqueIdentifier},
                                                                                    {"TenantID",Convert.ToString(CurrentViewContext.ShibbolethHostID)},
                                                                                    {"IntegrationClientID",Convert.ToString(CurrentViewContext.IntegrationClientId)},
                                                                                    {"Host",CurrentViewContext.ShibbolethHost},
                                                                                    {"IsExistingAccount",true.ToString()},
                                                                                    {"IsShibbolethApplicant",Convert.ToString(isApplicant)}
                                                                                };
                        string url = String.Format("~/login.aspx?shibbolethArgs={0}", queryString.ToEncryptedQueryString());
                        Response.Redirect(url);
                    }
                    else
                    {
                        //Auto Login through Shibboleth
                        Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                                {
                                                                                    { "IsShibbolethLogin", true.ToString()},
                                                                                    { "IsAutoLoginThroughShibboleth", true.ToString()},
                                                                                    {"IntegrationClientID",Convert.ToString(CurrentViewContext.IntegrationClientId)},
                                                                                    {"UserName",x.UserName},
                                                                                    {"Host",CurrentViewContext.ShibbolethHost},
                                                                                    {"TenantID",Convert.ToString(CurrentViewContext.ShibbolethHostID)},
                                                                                    {"IsShibbolethApplicant",Convert.ToString(isApplicant)}
                                                                                };
                        string url = String.Format("~/login.aspx?shibbolethArgs={0}", queryString.ToEncryptedQueryString());
                        Response.Redirect(url);
                    }
                });
            }
            else
            {
                CurrentViewContext.SelectedTenantId = CurrentViewContext.ShibbolethHostID;
                showHideSections(false, false, false);
                dvShibbonethConfirmation.Style.Add("display", "block");
                if (!isApplicant)
                {
                    cbShibbonethConfirmation.CancelButtonText = "I do not have an account with Complio";
                    paraText.InnerText = "If you have an account in Complio then please click on “Yes” button, if you don’t have an account with Complio then please click on “I do not have an account with Complio” button.";
                }
            }
        }
        #endregion

        private void ValidateSSN()
        {
            if (Convert.ToBoolean(rblSSN.SelectedValue))
            {
                //txtSSN.Text = "";
                txtSSN.Enabled = true;
                rfvSSN.Enabled = true;
                rfvtxtSSN.Enabled = true;
                rgvSSN.Enabled = true;
                revtxtSSN.Enabled = true;
                spnSSN.Visible = true;
            }
            else
            {
                txtSSN.Text = AppConsts.DefaultSSN;
                txtSSN.Enabled = false;
                rfvSSN.Enabled = false;
                rfvtxtSSN.Enabled = false;
                rgvSSN.Enabled = false;
                revtxtSSN.Enabled = false;
                spnSSN.Visible = false;
            }
        }

        private void ValidatePersonalInformation(Boolean IsLocationTenant)

        {
            if (IsLocationTenant)
            {
                //revFirstName.ValidationExpression = "^(?=.{1,30}$)([a-zA-Z])+( ?[a-zA-Z]+)$";
                //revMiddleName.ValidationExpression = "^(?=.{1,30}$)((([a-zA-Z])+( ?[a-zA-Z]+))|-{5})$";
                //revLastName.ValidationExpression = "^(?=.{1,30}$)([a-zA-Z])+(-?[a-zA-Z]+)$";
                revFirstName.ValidationExpression = "^(?=.{1,30}$)(([a-zA-Z])+( ?[a-zA-Z]+)|([a-zA-Z]{1}))$";
                revSuffix.ValidationExpression = "^[a-z A-Z]*-?[a-z A-Z]*$";
                revMiddleName.ValidationExpression = "^(?=.{1,30}$)((([a-zA-Z])+( ?[a-zA-Z]+))|(-{5})|([a-zA-Z]{1}))$";
                revLastName.ValidationExpression = "^(?=.{1,30}$)(([a-zA-Z])+(-?[a-zA-Z]+)|([a-zA-Z]{1}))$";
                RegularExpressionValidator3.ValidationExpression = "^(?=.{1,30}$)(([a-zA-Z])+( ?[a-zA-Z]+)|([a-zA-Z]{1}))$";
                RegularExpressionValidator4.ValidationExpression = "^(?=.{1,30}$)((([a-zA-Z])+( ?[a-zA-Z]+))|(-{5})|([a-zA-Z]{1}))$";
                RegularExpressionValidator5.ValidationExpression = "^(?=.{1,30}$)(([a-zA-Z])+(-?[a-zA-Z]+)|([a-zA-Z]{1}))$";

                //revSuffix.ErrorMessage = "Only alphabets or single hyphen(-) is allowed in the suffix up to maximum of 10 characters.";
                //revFirstName.ErrorMessage = "Only alphabets and one space between the first names are allowed up to maximum of 30 characters.";
                //revMiddleName.ErrorMessage = "Only alphabets and one space between the middle names are allowed up to maximum of 30 characters.";
                //revLastName.ErrorMessage = "Only alphabets or single hyphen(-) is allowed in the last name up to maximum of 30 characters.";

                //RegularExpressionValidator3.ErrorMessage = "Only alphabets and one space between the first names are allowed up to maximum of 30 characters.";
                //RegularExpressionValidator4.ErrorMessage = "Only alphabets and one space between the middle names are allowed up to maximum of 30 characters.";
                //RegularExpressionValidator5.ErrorMessage = "Only alphabets or single hyphen(-) is allowed in the last name up to maximum of 30 characters.";

                revSuffix.ErrorMessage = Resources.Language.SUFFIXNAMEVALDT;
                revFirstName.ErrorMessage = Resources.Language.FIRSTNAMEVALDT;
                revMiddleName.ErrorMessage = Resources.Language.MIDDLENAMEVALDT;
                revLastName.ErrorMessage = Resources.Language.LASTNAMEVALDT;

                RegularExpressionValidator3.ErrorMessage = Resources.Language.FIRSTNAMEVALDT;
                RegularExpressionValidator4.ErrorMessage = Resources.Language.MIDDLENAMEVALDT;
                RegularExpressionValidator5.ErrorMessage = Resources.Language.LASTNAMEVALDT;
            }
            else
            {
                revFirstName.ValidationExpression = "^[\\w\\d\\s\\-\\.\\']{1,50}$";
                revMiddleName.ValidationExpression = "^[\\w\\d\\s\\-\\.\\']{1,50}$";
                revLastName.ValidationExpression = "^[\\w\\d\\s\\-\\.\\']{1,50}$";
                revSuffix.ValidationExpression = "^[\\w\\d\\s\\-\\.\\']{1,10}$";
            }

        }


        #region CBI|| CABS
        /// <summary>
        /// Bind suffix dropdown, only in case of location service tenant.
        /// </summary>
        private void BindSuffixDropdown()
        {

            cmbSuffix.DataSource = CurrentViewContext.lstSuffixes.Where(a => a.IsSystem);
            cmbSuffix.DataBind();
            cmbSuffix.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(Resources.Language.SELECTSUFFIX, "0"));
        }

        #endregion

        #region Language Translation

        //private void ManageLanguageTranslation()
        //{
        //    if (CurrentViewContext.IsLocationServiceTenant)
        //    {
        //        dvLanguage.Style.Add("display", "block");
        //        btnLanguage.Visible = true;
        //        btnLanguage.Text = "Spanish";
        //        btnLanguage.ToolTip = "Click for Spanish";
        //    }
        //    else
        //    {
        //        dvLanguage.Style.Add("display", "none");
        //        btnLanguage.Visible = false;

        //    }
        //}

        //protected override void Construct()
        //{
        //    base.Construct();
        //}

        protected void btnDoPostBack_Click(object sender, EventArgs e)
        {

        }
        #endregion

        //protected void btnLanguage_Click(object sender, EventArgs e)
        //{
        //    String languageCode = String.Empty;
        //    //String LanguageCulture = String.Empty;
        //    languageCode = hdnLanguageCode.Value;
        //    switch (languageCode)
        //    {
        //        case ("SPN"):
        //            Session["LanguageCulture"] = AppConsts.SPANISH_CULTURE;
        //            Server.Transfer(Request.Url.PathAndQuery, false);
        //            break;
        //        case ("ENG"):
        //            Session["LanguageCulture"] = AppConsts.ENGLISH_CULTURE;
        //            Server.Transfer(Request.Url.PathAndQuery, false);
        //            break;
        //    }
        //}

    }
}

