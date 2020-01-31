using System;
using System.Linq;
using System.Collections.Generic;
using Business.RepoManagers;
using INTSOF.Utils;
using CoreWeb.CommonControls.Views;
using System.Web.Configuration;
using System.IO;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceManagement;
using CoreWeb.Shell;
using System.Text.RegularExpressions;
using CoreWeb.IntsofSecurityModel;
using INTSOF.UI.Contract.ComplianceOperation;
using System.Configuration;
using System.Web.UI.WebControls;
using CoreWeb.ComplianceAdministration.Views;
using Telerik.Web.UI;
using System.Web.UI;
using INTSOF.UI.Contract.FingerPrintSetup;
using System.Web.UI.HtmlControls;
using Entity;

namespace CoreWeb.ApplicantModule.Views
{
    public partial class EditProfile : BaseUserControl, IEditProfileView
    {
        private EditProfilePresenter _presenter = new EditProfilePresenter();
        private Int32 _tenantid;
        private String _viewType;
        private Entity.Address addressFields;
        Entity.OrganizationUser _myOrgUser;
        //private String _reg = @"^[\w\d\s\-\.\@\+]{4,50}$"; 
        private String _reg = @"^[\.\@a-zA-Z0-9_-]{4,50}$"; //UAT-3416
        //UAT-862 :- WB: As a student or an admin, I should not be allowed to upload documents with a size of 0 
        private Boolean _isCorruptedFileUploaded = false;

        public bool IsuserIntegrationClient = false;//3133
        public delegate void UpdateProfileInformation(Entity.OrganizationUser orgUser);
        public event UpdateProfileInformation NotifyProfileUpdate;

        #region Set Public Properties

        public List<Entity.lkpGender> Gender
        {
            set
            {
                //cmbGender.DataSource = value;
                cmbGender.DataSource = value.Where(cond => cond.LanguageID == LanguageTranslateUtils.GetCurrentLanguageFromSession().LanguageID);
                cmbGender.DataBind();
            }
        }

        public int loggedInUserId
        {
            get
            {
                Dictionary<String, String> args = new Dictionary<String, String>();
                if (!Request.QueryString["args"].IsNull())
                {
                    args.ToDecryptedQueryString(Request.QueryString["args"]);

                    if (args.ContainsKey("OrganizationUserId"))
                    {
                        return (Convert.ToInt32(args["OrganizationUserId"]));
                    }
                }
                return base.CurrentUserId;
            }

        }

        Boolean IEditProfileView.IsApplicant
        {
            get
            {
                return loggedInUserId == base.CurrentUserId;
            }
        }

        String IEditProfileView.UserName
        {
            get
            {
                return txtUsername.Text.Trim();
            }
        }
        String IEditProfileView.FirstName
        {
            get
            {
                return txtFirstName.Text.Trim();
            }
        }

        String IEditProfileView.LastName
        {
            get
            {
                return txtLastName.Text.Trim();
            }
        }
        String IEditProfileView.MiddleName
        {
            get
            {
                if (chkMiddleNameRequired.Checked)
                {
                    return String.Empty;
                }
                else
                {
                    return txtMiddleName.Text.Trim();
                }
            }
        }

        String IEditProfileView.SSN
        {
            get
            {
                return txtSSN.Text.Trim();
            }
        }

        Int32 IEditProfileView.SelectedTenantId
        {
            get
            {
                if (_tenantid == 0)
                {
                    _tenantid = Presenter.GetTenantID();
                }
                return _tenantid;
            }
        }
        DateTime? IEditProfileView.DOB
        {
            get
            {
                return dpkrDOB.SelectedDate;
            }
        }
        Int32 IEditProfileView.SelectedGenderId
        {
            get
            {
                return Convert.ToInt32(cmbGender.SelectedValue);
            }
        }
        String IEditProfileView.PrimaryPhone
        {
            get
            {
                //UAT-2447
                if (!chkPrimaryPhone.Checked)
                {
                    return txtPrimaryPhone.Text;
                }
                else
                {
                    return txtUnmaskedPrimaryPhone.Text;
                }
            }
        }

        String IEditProfileView.PrimaryEmail
        {
            get
            {
                return txtPrimaryEmail.Text.Trim();
            }
        }

        String IEditProfileView.PswdRecoveryEmail
        {
            get
            {
                return txtPswdRecoveryEmail.Text;
            }
            set
            {
                txtPswdRecoveryEmail.Text = value;
            }
        }

        String IEditProfileView.CurrentEmailAddress
        {
            get
            {
                if (ViewState["CurrentEmailAddress"].IsNotNull())
                    return ViewState["CurrentEmailAddress"].ToString().Trim();
                return String.Empty;
            }
            set
            {
                ViewState["CurrentEmailAddress"] = value.Trim();
            }
        }
        String IEditProfileView.Address1
        {
            get
            {
                return txtAddress1.Text;
            }
        }

        String IEditProfileView.Address2
        {
            get
            {
                return txtAddress2.Text;
            }
        }

        Int32 IEditProfileView.ZipId
        {
            get
            {
                return locationTenant.MasterZipcodeID.Value;
                //return Convert.ToInt32(locationTenant.ZipId);
            }
        }
        String IEditProfileView.SecondaryPhone
        {
            get
            {
                //UAT-2447
                if (!chkSecondaryPhone.Checked)
                {
                    return txtSecondaryPhone.Text;
                }
                else
                {
                    return txtUnmaskedSecondaryPhone.Text;
                }
            }
        }

        String IEditProfileView.SecondaryEmail
        {
            get
            {
                return txtSecondaryEmail.Text;
            }
        }
        String IEditProfileView.ErrorMessage
        {
            set
            {
                msgBox.Attributes.CssStyle.Add("display", "block");
                lblMessage.Text = value;
            }
        }
        String IEditProfileView.SuccessMessage
        {
            get;
            set;

        }
        IEditProfileView CurrentViewContext
        {
            get
            {
                return this;
            }
        }
        List<ApplicantInstitutionHierarchyMapping> IEditProfileView.lstApplicantInstitutionHierarchyMapping
        {
            get;
            set;
        }
        public String FilePath
        { get; set; }

        public String OriginalFileName
        { get; set; }

        public List<ApplicantDocument> ToSaveApplicantUploadedDocuments
        { get; set; }

        public String PageType
        {
            get
            {
                if (ViewState["PageType"] != null)
                    return (Convert.ToString(ViewState["PageType"]));
                return (Convert.ToString(ViewState["PageType"]));
            }
            set
            {
                ViewState["PageType"] = value;
            }
        }

        public SearchItemDataContract searchDataContract
        {
            get { return (SearchItemDataContract)(ViewState["searchDataContract"]); }
            set { ViewState["searchDataContract"] = value; }
        }

        public Boolean UpdateAspnetEmail
        {
            get
            {
                return chkChangeEmail.Checked;
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
        public List<PreviousAddressContract> ResidentialHistoryList
        {
            get
            {
                return PrevResident.ResidentialHistoryList;
            }
            set
            {
                PrevResident.ResidentialHistoryList = value;
            }
        }

        DateTime? IEditProfileView.DateResidentFrom
        {
            get
            {
                return dpCurResidentFrom.SelectedDate;
            }
        }

        String IEditProfileView.StateName
        {
            get
            {
                return locationTenant.RSLStateName;
            }
        }

        String IEditProfileView.CityName
        {
            get
            {
                return locationTenant.RSLCityName;
            }
        }

        String IEditProfileView.PostalCode
        {
            get
            {
                return locationTenant.RSLZipCode;
            }
        }

        Int32 IEditProfileView.CountryId
        {
            get
            {
                return locationTenant.RSLCountryId;
            }
        }

        Boolean IEditProfileView.IsSSNDisabled
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

        public Boolean Visiblity
        {
            get
            {
                if (ViewState["Visiblity"] != null)
                    return (Convert.ToBoolean(ViewState["Visiblity"]));
                return true;
            }
            set
            {
                ViewState["Visiblity"] = value;
            }
        }

        public List<UserNodePermissionsContract> lstUserNodePermissionsContract
        {
            get;
            set;
        }

        //UAT-781
        String IEditProfileView.DecryptedSSN
        {
            get;
            set;
        }

        #region UAT-806 Creation of granular permissions for Client Admin users

        public String SSNPermissionCode
        {
            get
            {
                return Convert.ToString(ViewState["SSNPermissionCode"]);
            }
            set
            {
                ViewState["SSNPermissionCode"] = value;
            }
        }
        public Boolean IsDOBDisable
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsDOBDisabled"] ?? false);
            }
            set
            {
                ViewState["IsDOBDisabled"] = value;
            }
        }

        public Int32 CurrentUserId
        {
            get
            {
                return base.CurrentUserId;
            }

        }
        #endregion

        //UAT 1438
        Boolean IEditProfileView.IsUserGroupCustomAttributeExist { get; set; }

        #region UAT-1578 : Addition of SMS notification
        Boolean IEditProfileView.IsReceiveTextNotification
        {
            get
            {
                if (rdbTextNotification.SelectedValue.IsNullOrEmpty())
                    return false;

                return Convert.ToBoolean(rdbTextNotification.SelectedValue);
            }
            set
            {
                rdbTextNotification.SelectedValue = Convert.ToString(value);
            }
        }
        String IEditProfileView.PhoneNumber
        {
            get
            {
                return txtPhoneNumber.Text;
            }
            set
            {
                txtPhoneNumber.Text = value;
            }
        }
        Entity.OrganisationUserTextMessageSetting IEditProfileView.OrganisationUserTextMessageSettingData { get; set; }

        String IEditProfileView.SMSNotificationErrorMessage { get; set; }
        #endregion

        //UAT-1261: WB: As an ADB admin, I should be able to "login" as any student to see what they see.
        Int32 IEditProfileView.OrgUsrID
        {
            get
            {
                if (!System.Web.HttpContext.Current.Session["AppViewAdminOrgUsrID"].IsNullOrEmpty())
                    return Convert.ToInt32(System.Web.HttpContext.Current.Session["AppViewAdminOrgUsrID"]);
                else
                    return loggedInUserId;
            }
        }

        List<Entity.ClientEntity.TypeCustomAttributes> IEditProfileView.ProfileCustomAttributeList
        {
            get;
            set;
        }

        //UAT-2447
        Boolean IEditProfileView.IsInternationalPhoneNumber
        {
            get
            {
                return chkPrimaryPhone.Checked;
            }
        }
        //UAT-2447
        Boolean IEditProfileView.IsInternationalSecondaryPhone
        {
            get
            {
                return chkSecondaryPhone.Checked;
            }
        }

        //UAT-3084
        Boolean IEditProfileView.IsPersonAliasAddUpdate
        {
            get;
            set;
        }

        //UAT-3455
        Boolean IEditProfileView.IsMultipleValsSelected
        {
            get
            {
                if (!ViewState["IsMultipleValsSelected"].IsNullOrEmpty())
                    return (Boolean)ViewState["IsMultipleValsSelected"];
                return false;
            }
            set
            {
                customAttribute.IsMultipleValsSelected = value;
                ViewState["IsMultipleValsSelected"] = value;
            }
        }

        Boolean IEditProfileView.IsMultiSelectionAllowed
        {
            get
            {
                if (!ViewState["IsMultiSelectionAllowed"].IsNullOrEmpty())
                    return (Boolean)ViewState["IsMultiSelectionAllowed"];
                return false;
            }
            set
            {
                ViewState["IsMultiSelectionAllowed"] = value;
            }
        }

        #endregion

        // CBI || CABS// Release-158
        Boolean IEditProfileView.IsLocationServiceTenant
        {
            get
            {
                Presenter.IsLocationServiceTenant();
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
        //UAT-4280
        Boolean IEditProfileView.IsUserGroupExist
        {
            get
            {

                if (!ViewState["IsUserGroupExist"].IsNullOrEmpty())
                    return Convert.ToBoolean(ViewState["IsUserGroupExist"]);
                return false;
            }
            set
            {
                ViewState["IsUserGroupExist"] = value;

            }
        }

        List<lkpSuffix> IEditProfileView.lstSuffixes
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

        Int32? IEditProfileView.SelectedSuffixID
        {
            get
            {
                if (!cmbSuffix.SelectedValue.IsNullOrEmpty())
                    return Convert.ToInt32(cmbSuffix.SelectedValue);
                return null;
            }
        }

        string IEditProfileView.Suffix
        {
            get
            {
                return txtSuffix.Text.Trim();
            }
            set
            {

            }
        }
        Boolean IEditProfileView.IsSuffixDropDownType
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

        public List<Entity.lkpLanguage> CommLanguage
        {
            set
            {
                cmbCommLang.DataSource = value;
                cmbCommLang.DataBind();
            }
        }

        Int32? IEditProfileView.SelectedCommLang
        {
            get;
            set;
        }



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
        #region UAT-4280

        public IQueryable<Entity.ClientEntity.UserGroup> lstUserGroups { get; set; }

        public IList<Entity.ClientEntity.UserGroup> lstUserGroupsForUser { get; set; }



        List<Int32> IEditProfileView.lstUserGroupIDs

        {
            get
            {
                List<Int32> lstUserGroupIDs = new List<Int32>();
                foreach (RadComboBoxItem item in cmdUserGroup.CheckedItems)
                {
                    lstUserGroupIDs.Add(Convert.ToInt32(item.Value));
                }
                return lstUserGroupIDs;
            }
            //set
            //{


            //        foreach (RadComboBoxItem item in cmdUserGroup.Items)
            //        {
            //            var Temp = CurrentViewContext.lstUserGroupsForUser.Where(x => x.UG_ID ==
            //              Convert.ToInt32(item.Value)).FirstOrDefault();
            //            if (Temp != null)
            //            {
            //                item.Checked = true;
            //            }
            //        }

            //}
        }
        #endregion
        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (CurrentViewContext.IsLocationServiceTenant)
            {
                divSSN.Visible = false;
                divDOB.Visible = false;
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack || (!(this.hdnPostbacksource.Text == "EP") && this.PageType == AppConsts.DASHBOARD))
                {
                    Presenter.IsLocationServiceTenant();
                }

                //UAT-3910
                locationTenant.IsLocationServiceTenant = CurrentViewContext.IsLocationServiceTenant;

                if (CurrentViewContext.IsLocationServiceTenant)
                {
                    Presenter.IsDropDownSuffixType();
                    revAddress1.ErrorMessage = Resources.Language.ADDRESSASCIIINVALIDCODE;
                    revAddress1.Enabled = true;
                }

                ShowhideTwoFactorAuthentication();//UAT-2930
                                                  //Release 158 CBI
                ucPersonAlias.SelectedTenantId = CurrentViewContext.SelectedTenantId;
                ucPersonAlias.PageType = PersonAliasPageType.EditProfile.GetStringValue();
                //change done for applicant dashboard redesign.
                if (Visiblity.IsNullOrEmpty() || Visiblity == true)
                {
                    Presenter.GetApplicantInstitutionHierarchyMapping();
                    _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                    Dictionary<String, String> args = new Dictionary<String, String>();

                    if (!Request.QueryString["args"].IsNull())
                    {
                        args.ToDecryptedQueryString(Request.QueryString["args"]);
                        if (args.ContainsKey("PageType"))
                        {
                            PageType = args["PageType"].ToString();
                        }
                    }
                    if (PageType == AppConsts.DASHBOARD)
                    {
                        txtUsername.ReadOnly = false;
                        btnCheckUsername.Visible = true;
                        dvProfilePic.Visible = false;
                        cBarMain.DisplayButtons = CommandBarButtons.Save;
                        cBarMain.HideButtons(CommandBarButtons.Cancel);
                    }
                    else if (PageType != "MyProfile")
                    {
                        // uat 509
                        String permissionCode = Presenter.GetUserNodePermission();
                        if (permissionCode.Equals(LkpPermission.ReadOnly.GetStringValue()))
                        {
                            DisableControls();
                        }
                        lnkGoBack.Visible = true;
                        txtUsername.ReadOnly = true;
                        btnCheckUsername.Visible = false;
                        //base.SetPageTitle("Applicant’s Profile");                    
                        base.SetPageTitle(Resources.Language.APPLICANTSPROFILE);
                        //UAT-968:As an ADB admin, I should be able to create/view/edit "notes" in a student's profile search details.
                        if (Presenter.IsDefaultTenant(base.CurrentUserId))
                        {
                            ucApplicantNotes.Visible = true;
                            divProfileNotes.Visible = true;
                            ucApplicantNotes.ApplicantUserID = CurrentViewContext.loggedInUserId;
                            ucApplicantNotes.SelectedTenantId = CurrentViewContext.SelectedTenantId;
                        }
                        btnLinkAccount.Visible = false;
                    }
                    else
                    {
                        txtUsername.ReadOnly = false;
                        btnCheckUsername.Visible = true;
                        //base.SetPageTitle("My Profile");
                        base.SetPageTitle(Resources.Language.MYPROFILE);
                    }
                    _myOrgUser = Presenter.GetUserData();
                    LocationInfo location = locationTenant;

                    #region UAT-781 ENCRYPTED SSN

                    Presenter.GetDecryptedSSN(_myOrgUser.OrganizationUserID, false);
                    #endregion

                    //UAT 712
                    if (SysXWebSiteUtils.SessionService.OrganizationUserId == _myOrgUser.OrganizationUserID)
                    {
                        lnkGoBack.Visible = false;
                        divChangePwd.Visible = true;
                        lnkChangePassword.Enabled = true;
                        lnkChangePassword.Visible = true;
                        Dictionary<String, String> queryString = new Dictionary<String, String>();
                        queryString = RedirectToChangePassword(queryString);
                    }

                    if (!(this.hdnPostbacksource.Text == "EP") && this.PageType == AppConsts.DASHBOARD)
                    {
                        locationTenant.IsFalsePostBack = true;
                        ucPersonAlias.IsFalsePostBack = true;
                        PrevResident.IsFalsePostBack = true;
                    }
                    else
                    {
                        locationTenant.IsFalsePostBack = false;
                        ucPersonAlias.IsFalsePostBack = false;
                        PrevResident.IsFalsePostBack = false;
                    }

                    if (!this.IsPostBack || (!(this.hdnPostbacksource.Text == "EP") && this.PageType == AppConsts.DASHBOARD))
                    {
                        //Set MinDate and MaxDate for DOB
                        dpkrDOB.MinDate = Convert.ToDateTime("01-01-1900");
                        dpkrDOB.MaxDate = DateTime.Now;

                        rngvDOB.MaximumValue = DateTime.Now.Date.AddYears(-1).ToShortDateString();
                        rngvDOB.MinimumValue = Convert.ToDateTime("01-01-1900").ToShortDateString();
                        //rngvDOB.ErrorMessage = "Date of birth should not be less than a year.";
                        rngvDOB.ErrorMessage = Resources.Language.DOBNOTLESSTHANYEAR;

                        dpCurResidentFrom.MaxDate = DateTime.Now;

                        Presenter.OnViewInitialized();
                        this.Gender = Presenter.GetGenderList();
                        Presenter.GetSSNSetting();
                        Presenter.GetCommLang(_myOrgUser.UserID);

                        cmbCommLang.SelectedValue = CurrentViewContext.SelectedCommLang.HasValue ? Convert.ToString(CurrentViewContext.SelectedCommLang.Value) : "0";
                        if (CurrentViewContext.IsLocationServiceTenant)
                        {
                            rngvDOB.MaximumValue = DateTime.Now.Date.AddYears(-10).ToShortDateString();
                            //rngvDOB.ErrorMessage = "Date of birth should not be less than 10 year.";
                            rngvDOB.ErrorMessage = Resources.Language.DOBNOTLESSTHAT10YEAR;
                            dpkrDOB.DateInput.ClientEvents.OnKeyPress = "OnKeyPress";
                            dvCommLang.Style.Add("Display", "");
                            //UAT-3860
                            dvAddress2.Visible = false;
                            lblAddress1.Text = Resources.Language.ADDRESS;
                            rfvAddress1.Text = Resources.Language.ADDRESSREQ;
                            //rfvAddress1.Text = "Address is required.";

                        }
                        AddSuffixDropdownAndDesignChange();


                        if (!_myOrgUser.IsNull())
                        {
                            txtUsername.Text = _myOrgUser.aspnet_Users.UserName;
                            //UAT-806 Creation of granular permissions for Client Admin users
                            if (PageType != "MyProfile" && !CurrentViewContext.IsSSNDisabled && CurrentViewContext.SSNPermissionCode.ToUpper() == EnumSystemPermissionCode.MASKED_READ_PERMISSION.GetStringValue())
                            {
                                //txtSSNMasked.Text = Presenter.GetMaskedSSN(_myOrgUser.SSN);//UAT-781
                                txtSSNMasked.Text = Presenter.GetMaskedSSN(CurrentViewContext.DecryptedSSN);//UAT-781
                                divSSNMasked.Visible = true;
                                divSSN.Visible = false;
                            }

                            //txtSSN.Text = _myOrgUser.SSN;//UAT-781
                            txtSSN.Text = CurrentViewContext.DecryptedSSN;//UAT-781
                            ManageSSNRdBtn();
                            txtOrganisation.Text = _myOrgUser.Organization.OrganizationName;
                            cmbGender.SelectedValue = Convert.ToString(_myOrgUser.Gender);
                            txtFirstName.Text = UserFirstName = _myOrgUser.FirstName;
                            //UAT-1612 : As an applicant, my middle name should be required.
                            txtMiddleName.Text = UserMiddleName = _myOrgUser.MiddleName;

                            txtLastName.Text = UserLastName = _myOrgUser.LastName;

                            if (!CurrentViewContext.lstSuffixes.IsNullOrEmpty())
                            {
                                // cmbSuffix.SelectedValue = _myOrgUser.UserTypeID.IsNullOrEmpty() ? String.Empty : _myOrgUser.UserTypeID.ToString();
                                if (!CurrentViewContext.lstSuffixes.IsNullOrEmpty() && _myOrgUser.UserTypeID > AppConsts.NONE)
                                    UserSuffix = CurrentViewContext.lstSuffixes.Where(cond => cond.SuffixID == _myOrgUser.UserTypeID).FirstOrDefault().Suffix;
                                _myOrgUser.Suffix = UserSuffix;
                            }
                            if (CurrentViewContext.IsSuffixDropDownType)
                            {
                                BindSuffixDropdown();
                                if (!_myOrgUser.Suffix.IsNullOrEmpty())
                                {
                                    var item = cmbSuffix.FindItemByText(_myOrgUser.Suffix, true);
                                    if (!item.IsNullOrEmpty())
                                    {
                                        cmbSuffix.Items.FindItemByText(_myOrgUser.Suffix, true).Selected = true;
                                    }
                                }
                            }
                            else
                            {
                                txtSuffix.Text = UserSuffix = CurrentViewContext.lstSuffixes.Where(a => a.Suffix == _myOrgUser.Suffix && !a.IsSystem).Any() ? _myOrgUser.Suffix : String.Empty;
                            }
                            dpkrDOB.SelectedDate = _myOrgUser.DOB;

                            //UAT-2447                      
                            if (_myOrgUser.IsInternationalPhoneNumber)
                            {
                                chkPrimaryPhone.Checked = true;
                                dvMaskedPrimaryPhone.Style["display"] = "none";
                                rfvTxtMobile.Enabled = false;
                                dvUnmaskedPrimaryPhone.Style["display"] = "block";
                            }
                            if (_myOrgUser.IsInternationalSecondaryPhone)
                            {
                                chkSecondaryPhone.Checked = true;
                                dvMaskedSecondaryPhone.Style["display"] = "none";
                                dvUnMaskedSecondaryPhone.Style["display"] = "block";
                            }
                            txtUnmaskedPrimaryPhone.Text = _myOrgUser.PhoneNumber;
                            ShowHidePhoneControls(_myOrgUser.IsInternationalPhoneNumber, 1);
                            ShowHidePhoneControls(_myOrgUser.IsInternationalPhoneNumber, 2);

                            txtPrimaryPhone.Text = _myOrgUser.PhoneNumber;
                            txtPrimaryEmail.Text = _myOrgUser.PrimaryEmailAddress;
                            txtConfrimPrimayEmail.Text = _myOrgUser.PrimaryEmailAddress;
                            CurrentViewContext.CurrentEmailAddress = _myOrgUser.PrimaryEmailAddress;
                            CurrentViewContext.PswdRecoveryEmail = _myOrgUser.aspnet_Users.aspnet_Membership.Email;
                            txtSecondaryPhone.Text = _myOrgUser.SecondaryPhone;
                            //UAT-2447
                            txtUnmaskedSecondaryPhone.Text = _myOrgUser.SecondaryPhone;

                            txtSecondaryEmail.Text = _myOrgUser.SecondaryEmailAddress;
                            txtConfirmSecEmail.Text = _myOrgUser.SecondaryEmailAddress;

                            if (_myOrgUser.AddressHandle.IsNotNull() && _myOrgUser.AddressHandle.Addresses.IsNotNull())
                            {
                                addressFields = _myOrgUser.AddressHandle.Addresses.FirstOrDefault(p => p.AddressHandleID.Equals(_myOrgUser.AddressHandleID));
                                txtAddress1.Text = addressFields.Address1;
                                txtAddress2.Text = addressFields.Address2;
                                locationTenant.MasterZipcodeID = addressFields.ZipCodeID;
                                if (addressFields.ZipCodeID == 0 && addressFields.AddressExts.IsNotNull() && addressFields.AddressExts.Count > 0)
                                {
                                    Entity.AddressExt addressExt = addressFields.AddressExts.FirstOrDefault();
                                    //UAT-3910
                                    if (CurrentViewContext.IsLocationServiceTenant)
                                    {
                                        locationTenant.RSLCountryId = addressExt.AE_County == null ? AppConsts.NONE : Convert.ToInt32(addressExt.AE_County);
                                    }
                                    else
                                    {
                                        locationTenant.RSLCountryId = addressExt.AE_CountryID;
                                    }
                                    locationTenant.RSLStateName = addressExt.AE_StateName;
                                    locationTenant.RSLCityName = addressExt.AE_CityName;
                                    locationTenant.RSLZipCode = addressExt.AE_ZipCode;
                                }
                            }

                            var resHisoryAddress = _myOrgUser.ResidentialHistories.FirstOrDefault(obj => obj.RHI_IsDeleted == false && obj.RHI_IsCurrentAddress == true);
                            if (resHisoryAddress.IsNotNull())
                            {
                                dpCurResidentFrom.SelectedDate = resHisoryAddress.RHI_ResidenceStartDate;
                            }
                            CurrentViewContext.ResidentialHistoryList = Presenter.GetResidentialHistories(_myOrgUser.OrganizationUserID);
                            if (_myOrgUser.PersonAlias.IsNotNull())
                            {
                                var pAlias = _myOrgUser.PersonAlias.Where(x => x.PA_IsDeleted == false).Select(cond => new PersonAliasContract
                                {
                                    FirstName = cond.PA_FirstName,
                                    LastName = cond.PA_LastName,
                                    //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                                    MiddleName = cond.PA_MiddleName,
                                    ID = cond.PA_ID,
                                    Suffix = !CurrentViewContext.IsLocationServiceTenant.IsNullOrEmpty() && CurrentViewContext.IsLocationServiceTenant ? (!cond.PersonAliasExtensions.IsNullOrEmpty() && !cond.PersonAliasExtensions.FirstOrDefault(con => !con.PAE_IsDeleted).IsNullOrEmpty() && !cond.PersonAliasExtensions.FirstOrDefault(con => !con.PAE_IsDeleted).PAE_Suffix.IsNullOrEmpty() ? cond.PersonAliasExtensions.FirstOrDefault(con => !con.PAE_IsDeleted).PAE_Suffix : String.Empty) : String.Empty,
                                    // SuffixID = !CurrentViewContext.IsLocationServiceTenant.IsNullOrEmpty() && CurrentViewContext.IsLocationServiceTenant ? (!cond.PersonAliasExtensions.IsNullOrEmpty() && !cond.PersonAliasExtensions.FirstOrDefault(con => !con.PAE_IsDeleted).IsNullOrEmpty() && !cond.PersonAliasExtensions.FirstOrDefault(con => !con.PAE_IsDeleted).PAE_Suffix.IsNullOrEmpty() ? CurrentViewContext.lstSuffixes.Where(con => con.Suffix == cond.PersonAliasExtensions.FirstOrDefault(x => !x.PAE_IsDeleted).PAE_Suffix).FirstOrDefault().SuffixID : (Int32?)null) : (Int32?)null,
                                }).ToList();
                                if (CurrentViewContext.IsLocationServiceTenant)
                                {
                                    ucPersonAlias.PersonAliasList = pAlias;
                                }
                                else
                                {
                                    CurrentViewContext.PersonAliasList = pAlias;
                                }
                            }

                            //cmbProgram.SelectedValue = Convert.ToString(_myOrgUser.OrganizationUserPrograms.FirstOrDefault(p => p.OrganizationUserID.Equals(loggedInUserId)).ProgramStudyID);
                            //chkBoxList.DataValueField = Convert.ToString(_myOrgUser.OrganizationUserPrograms.FirstOrDefault(p => p.OrganizationUserID.Equals(loggedInUserId)).ProgramStudyID);

                            if (_myOrgUser.PhotoName.IsNotNull())
                                imgCntrl.ImageUrl = String.Format("/ComplianceOperations/UserControl/DocumentViewer.aspx?UserId={0}&DocumentType={1}", loggedInUserId, "ProfilePicture");
                            else
                                //imgCntrl.AlternateText = "Profile picture not available";
                                imgCntrl.AlternateText = Resources.Language.PROFILEPICNOTAVIALBLE;

                            //Setting up initials
                            UpdateInitials();

                            //UAT-1578 : Addition of SMS notification
                            HideShowControlsForSMSNotification();
                            #region "Hide Residential History for CBI Tenant Applicant"

                            if (Convert.ToBoolean(CurrentViewContext.IsApplicant) && CurrentViewContext.IsLocationServiceTenant)
                            {
                                dvResHistory.Visible = false;
                            }

                            ValidatePersonalInformation(CurrentViewContext.IsLocationServiceTenant);
                            #endregion
                        }
                        #region UAT-4280
                        BindUserGroup();
                        #endregion

                    }
                    UserFirstName = txtFirstName.Text;
                    UserLastName = txtLastName.Text;
                    //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                    UserMiddleName = txtMiddleName.Text;
                    //UAT-1612 : As an applicant, my middle name should be required.
                    ValidateMiddleName(CurrentViewContext.MiddleName);
                    uploadControl.MaxFileSize = Convert.ToInt32(WebConfigurationManager.AppSettings[AppConsts.MAXIMUM_FILE_SIZE]);
                    Presenter.OnViewLoaded();
                    //cBarMain.SaveButton.ToolTip = "Click to save any updates made to your profile";
                    cBarMain.SaveButton.ToolTip = Resources.Language.CLKTOSAVECHANGESTOPROFILE;
                    //cBarMain.CancelButton.ToolTip = "Click to cancel. Any updates made to your profile will not be saved";
                    cBarMain.CancelButton.ToolTip = Resources.Language.CLKTOCNCL;

                    #region UAT 4280: Enhancement to allow students to select a User Group.
                    Presenter.GetUserGroupsForUser();
                    if (CurrentViewContext.lstApplicantInstitutionHierarchyMapping.IsNullOrEmpty())
                    {
                        if (!CurrentViewContext.lstUserGroupsForUser.IsNullOrEmpty())
                        {
                            dvUsrGrp.Style["display"] = "block";
                            CurrentViewContext.IsUserGroupExist = true;
                        }


                    }

                    #endregion
                    #region  UAT 1438: Enhancement to allow students to select a User Group
                    if (!CurrentViewContext.lstApplicantInstitutionHierarchyMapping.IsNullOrEmpty())
                    {
                        List<Int32> lstHierarchyNodeIds = CurrentViewContext.lstApplicantInstitutionHierarchyMapping.Where(cond => cond.InstitutionNode_ID != null).Select(col => col.InstitutionNode_ID).ToList();
                        if (Presenter.IsUserGroupCustomAttributeExist(lstHierarchyNodeIds))
                        {
                            dvMergedUserGroup.Visible = true;
                            customAttribute.Visible = true;
                            customAttribute.ControlDisplayMode = DisplayMode.Controls;
                            customAttribute.TenantId = CurrentViewContext.SelectedTenantId;
                            customAttribute.CurrentLoggedInUserId = CurrentViewContext.loggedInUserId;
                            customAttribute.DataSourceModeType = DataSourceMode.Ids;
                            customAttribute.ValidationGroup = "grpFormSubmit";
                            customAttribute.ShowUserGroupCustomAttribute = false;
                            customAttribute.ShowUserGroupCustAttributeMerged = true;

                            CurrentViewContext.IsUserGroupCustomAttributeExist = true;
                        }
                        else
                        {


                            if (!CurrentViewContext.lstUserGroupsForUser.IsNullOrEmpty())
                            {
                                dvUsrGrp.Style["display"] = "block";
                                CurrentViewContext.IsUserGroupExist = true;
                            }


                        }

                    }


                    #endregion
                    HideShowControlsForGranularPermission();//UAT-806:- Creation of granular permissions for Client Admin users
                                                            //UAT-1578
                    HideShowControlsForSMSNotification(false);
                    #region UAT-2169:Send Middle Name and Email address to clearstar in Complio
                    hdnNoMiddleNameText.Value = NoMiddleNameText;
                    #endregion

                    //UAT-2440: Profile level custom attributes by tenant.
                    caProfileCustomAttributes.TenantId = CurrentViewContext.SelectedTenantId;
                    caProfileCustomAttributes.TypeCode = CustomAttributeUseTypeContext.Profile.GetStringValue();
                    caProfileCustomAttributes.CurrentLoggedInUserId = CurrentViewContext.loggedInUserId;
                    caProfileCustomAttributes.DataSourceModeType = DataSourceMode.Ids;
                    caProfileCustomAttributes.ControlDisplayMode = DisplayMode.Controls;
                    caProfileCustomAttributes.ValidationGroup = "grpFormSubmit";
                    caProfileCustomAttributes.IsIntegrationClientOrganisationUser = false; //UAT-3133

                    #region UAT-3133
                    if (Presenter.IsIntegrationClientOrganisationUser())
                    {
                        txtPrimaryEmail.Enabled = false;
                        txtConfrimPrimayEmail.Enabled = false;
                        caProfileCustomAttributes.IsIntegrationClientOrganisationUser = true;
                        IsuserIntegrationClient = true; //3133
                    }
                    #endregion
                }

                if (CurrentViewContext.IsApplicant)
                {
                    customAttribute.IsUserGroupSlctdValuesdisabled = true; //UAT-3455
                }
            }
            catch(Exception ex)
            {
                base.ShowErrorMessage(ex.Message);
            }


        }

        private void DisableControls()
        {
            cBarMain.SaveButton.Enabled = false;
            cBarMain.CancelButton.Enabled = false;
            ucPersonAlias.IsReadOnly = true;
            PrevResident.HideAddNewButton = true;
            PrevResident.ShowDeleteColumn = false;
            PrevResident.ShowEditColumn = false;
            uploadControl.Enabled = false;
            //UAT-968:As an ADB admin, I should be able to create/view/edit "notes" in a student's profile search details.
            ucApplicantNotes.IsReadOnly = true;
        }

        /// <summary>
        /// Used for open the change password page.
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        private Dictionary<string, string> RedirectToChangePassword(Dictionary<String, String> queryString)
        {
            queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "Child", @"~/IntsofSecurityModel/ChangePassword.ascx"},
                                                                    {"PageType",PageType}
                                                                 };
            _viewType = "Applicant";
            lnkChangePassword.NavigateUrl = String.Format("~/IntsofSecurityModel/default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());

            //Adding tooltip for the user profile link only if link is enabled
            if (!string.IsNullOrWhiteSpace(lnkChangePassword.NavigateUrl))
            {
                lnkChangePassword.ToolTip = Resources.Language.CLICKTOCHANGEPASSWORD;
            }

            //Setting iframe as a target 
            lnkChangePassword.Target = "pageFrame";
            return queryString;
        }
        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                //base.SetPageTitle("My Profile");
                base.BreadCrumbTitleKey = "Key_PROFILE";
                base.Title = Resources.Language.PROFILE;
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


        public EditProfilePresenter Presenter
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

        private void UpdateInitials()
        {
            if (!_myOrgUser.IsNull())
            {
                //Setting Name initials
                if (!string.IsNullOrWhiteSpace(_myOrgUser.FirstName))
                {
                    lblNameInitials.Text = _myOrgUser.FirstName.Substring(0, 1);
                }
                if (!string.IsNullOrWhiteSpace(_myOrgUser.LastName))
                {
                    lblNameInitials.Text = lblNameInitials.Text + _myOrgUser.LastName.Substring(0, 1);
                }

                if (_myOrgUser.PhotoName.IsNotNull())
                {
                    imgCntrl.Visible = true;
                    lblNameInitials.Visible = false;
                }
                else
                {
                    imgCntrl.Visible = false;
                    lblNameInitials.Visible = true;
                }
            }
        }

        #region Button Click Events

        /// <summary>
        /// On Click of Update button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucCmdBar1_SaveClick(object sender, EventArgs e)
        {

            SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
            lblUserNameMessage.Text = String.Empty;
            CurrentViewContext.SelectedCommLang = Convert.ToInt32(cmbCommLang.SelectedValue);
            #region UAT 536- Add Custom Fields (editable) to the Applicant search
            List<ApplicantCustomAttributeContract> applicantCustomAttributeContract = new List<ApplicantCustomAttributeContract>();

            //UAT 1438: Enhancement to allow students to select a User Group. 
            List<ApplicantUserGroupMapping> lstApplicantuserGroupMapping = new List<ApplicantUserGroupMapping>();

            List<TypeCustomAttributes> lstCustomAttribute = new List<TypeCustomAttributes>();

            foreach (RepeaterItem repeated in rptrCustomAttribute.Items)
            {
                CustomAttributeLoader CustomAttributeLoader = repeated.FindControl("customAttribute") as CustomAttributeLoader;
                lstCustomAttribute = CustomAttributeLoader.GetCustomAttributeValues();
                foreach (TypeCustomAttributes item in lstCustomAttribute)
                {
                    applicantCustomAttributeContract.Add(new ApplicantCustomAttributeContract
                    {
                        CAM_ID = item.CAMId.Value,
                        CAV_ID = item.CAVId.HasValue ? item.CAVId.Value : 0,
                        CAV_AttributeValue = item.CAValue,
                        HierarchyNodeID = item.HierarchyNodeID.HasValue ? item.HierarchyNodeID.Value : 0
                    });
                }
            }
            #endregion

            //UAT 1438: Enhancement to allow students to select a User Group.
            //lstApplicantuserGroupMapping is null or empty.

            //UAT-3455
            if (CurrentViewContext.IsApplicant)
            {
                CurrentViewContext.IsMultiSelectionAllowed = Presenter.IsMultiUserGroupSelectionAllowed();  //UAT-4731 : Restrict Applicant To One User Group In Order Process
                customAttribute.ClearMultiSelectedValues(CurrentViewContext.IsMultiSelectionAllowed);
            }

            if (lstApplicantuserGroupMapping.IsNullOrEmpty())
            {
                List<Int32> tmpLstUserGroupIDs = customAttribute.GetUserGroupCustomAttributeValues();
                AddToApplicantUserGroupMappingList(lstApplicantuserGroupMapping, tmpLstUserGroupIDs);

                //UAT 4280
                if (CurrentViewContext.IsUserGroupExist)
                {
                    AddToApplicantUserGroupMappingList(lstApplicantuserGroupMapping, CurrentViewContext.lstUserGroupIDs);
                }
            }

            if (String.IsNullOrEmpty(CurrentViewContext.UserName.Trim()))
            {
                //lblUserNameMessage.Text = "Username is required.";
                lblUserNameMessage.Text = Resources.Language.USERNAMEREQ;
                lblUserNameMessage.ForeColor = System.Drawing.Color.Red;
                //ScriptManager.RegisterStartupScript(this, Page.GetType(), Guid.NewGuid().ToString(), "ValidateUsername();", true);
                return;
            }
            if (Regex.IsMatch(CurrentViewContext.UserName, _reg) == false)
            {
                //lblUserNameMessage.Text = "Invalid username. Must have at least 4 chars (A-Z a-z 0-9 . _ - @).";
                lblUserNameMessage.Text = Resources.Language.INVALIDUSERNAME;
                lblUserNameMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }
            //Secondary Confirm Email Address check
            if (!String.IsNullOrEmpty(txtSecondaryEmail.Text.Trim()) && String.IsNullOrEmpty(txtConfirmSecEmail.Text.Trim()))
            {
                lblMessage.Visible = true;
                //base.ShowErrorInfoMessage("Secondary Confirm Email Address is required.");
                base.ShowErrorInfoMessage(Resources.Language.SCNDRYCNFRMEMAILADDRSREQ);
                return;
            }

            if (!CurrentViewContext.IsLocationServiceTenant && ucPersonAlias.NewFirstNameAlias.IsNullOrEmpty() && !ucPersonAlias.NewLastNameAlias.IsNullOrEmpty())
            {
                lblMessage.Visible = true;
                //base.ShowErrorInfoMessage("Alias/Maiden First Name is required if Alias/Maiden Last Name is entered.");
                base.ShowErrorInfoMessage(Resources.Language.ALIASFRSTNMEREQIFLSTNME);
                return;
            }

            if (!CurrentViewContext.IsLocationServiceTenant && !ucPersonAlias.NewFirstNameAlias.IsNullOrEmpty() && ucPersonAlias.NewLastNameAlias.IsNullOrEmpty())
            {
                lblMessage.Visible = true;
                //base.ShowErrorInfoMessage("Alias/Maiden Last Name is required if Alias/Maiden First Name is entered.");
                base.ShowErrorInfoMessage(Resources.Language.ALIASLSTNMEREQIFFRSTNME);
                return;
            }
            String newFirstNameAlias = ucPersonAlias.NewFirstNameAlias;
            String newLastNameAlias = ucPersonAlias.NewLastNameAlias;
            if (!CurrentViewContext.IsLocationServiceTenant && ucPersonAlias.HasDuplicateNames)
            {
                lblMessage.Visible = true;
                //base.ShowErrorInfoMessage("Duplicate names cannot be added.");
                base.ShowErrorInfoMessage(Resources.Language.DPLNAMECNTADD);
                return;
            }

            //Primary Email Address check
            if (CurrentViewContext.UpdateAspnetEmail &&
                CurrentViewContext.PrimaryEmail.ToLower() != CurrentViewContext.PswdRecoveryEmail.ToLower() &&
                Presenter.IsExistsPrimaryEmail(user.UserId))
            {
                lblMessage.Visible = true;
                //base.ShowErrorInfoMessage("This email address is already in use. Try another?");
                base.ShowErrorInfoMessage(Resources.Language.EMAILALREADYINUSE);
                return;
            }
            //Username check
            if (CurrentViewContext.IsApplicant && Presenter.IsExistsUserName(user.UserId))
            {
                lblMessage.Visible = true;
                //base.ShowErrorInfoMessage("This username is already in use. Try another?");
                base.ShowErrorInfoMessage(Resources.Language.USERNAMEALREADYINUSE);
                return;
            }
            //UAT-3910
            if (!CurrentViewContext.IsLocationServiceTenant)
                if (!IsValidAddress())
                {
                    //base.ShowErrorInfoMessage("Please select a valid ZipCode.");
                    base.ShowErrorInfoMessage(Resources.Language.PLSSELVALIDZIP);
                    return;
                }

            if (customAttribute.IsMultipleValsSelected) //UAT-3455
            {
                errMsgUserGroup.Visible = true;
                //base.ShowErrorInfoMessage("Please select one user group only.");
                base.ShowErrorInfoMessage(Resources.Language.PLSSLCTONLYONEUSERGRP);
                return;
            }


            //Implementation for: Based on CBI's response below we need to allow for only one character per name (first, middle, last).
            //The scenario we have to think about is the one where the applicant opts to provide no middle name with only one character for the first and last name.

            if (!txtFirstName.IsNullOrEmpty() || !txtMiddleName.IsNullOrEmpty() || !txtLastName.IsNullOrEmpty() || !txtSuffix.IsNullOrEmpty())
            {
                if (CurrentViewContext.IsLocationServiceTenant)
                {
                    Int32 suffixLength = CurrentViewContext.IsSuffixDropDownType ? cmbSuffix.SelectedItem.Index > 0 ? cmbSuffix.SelectedItem.Text.Length : 0 : txtSuffix.Text.Length;
                    Int32 totalLength = (txtFirstName.Text.Length) + (txtMiddleName.Text.Length) + (txtLastName.Text.Length) + suffixLength;
                    if (totalLength < AppConsts.THREE)
                    {
                        //base.ShowErrorInfoMessage("Total length for Full Name should be atleast 3 characters.");
                        base.ShowErrorInfoMessage(Resources.Language.TTLLENFULLNAMEATLEASTTHREECHARS);
                        return;
                    }
                }
            }



            if (!CurrentViewContext.IsApplicant || (CurrentViewContext.IsApplicant && Presenter.SaveSMSNotificationData()))
            {
                if (Presenter.SaveUserData())
                {
                    //UAT-3068 Add text message option to two factor authentication options. (applicant should be able to choose Google authenticator or to get code in text message)
                    String SelectedAuthenticationType = String.Empty;
                    if (!String.IsNullOrEmpty(hdnrdbSpecifyAuthenticationCalculatedValue.Value))
                    {
                        TwoFactorAuthentication.SelectedAuthenticationType = hdnrdbSpecifyAuthenticationCalculatedValue.Value;
                    }

                    SelectedAuthenticationType = TwoFactorAuthentication.SelectedAuthenticationType;

                    //if (!CurrentViewContext.IsReceiveTextNotification && TwoFactorAuthentication.SelectedAuthenticationType == INTSOF.Utils.AuthenticationMode.Text_Message.GetStringValue())
                    //{
                    //    SelectedAuthenticationType = INTSOF.Utils.AuthenticationMode.None.GetStringValue();
                    //}
                    //else
                    //{
                    //    SelectedAuthenticationType = TwoFactorAuthentication.SelectedAuthenticationType;
                    //}

                    if (Presenter.SaveAuthenticationData(Convert.ToString(user.UserId), SelectedAuthenticationType))
                    {
                        hdnrdbSpecifyAuthenticationCalculatedValue.Value = String.Empty;
                    }

                    //UAT 536- Add Custom Fields (editable) to the Applicant search
                    //UAT 4280 
                    Presenter.SaveUpdateApplicantCustomAttribute(applicantCustomAttributeContract);

                    if (CurrentViewContext.IsUserGroupCustomAttributeExist || CurrentViewContext.IsUserGroupExist)
                    {
                        //UAT 1438: Enhancement to allow students to select a User Group. 
                        Presenter.SaveUpdateApplicantUserGroupCustomAttribute(lstApplicantuserGroupMapping);
                    }

                    //UAT-968:As an ADB admin, I should be able to create/view/edit "notes" in a student's profile search details.
                    if (Presenter.IsDefaultTenant(base.CurrentUserId))
                    {
                        ucApplicantNotes.SaveUpdateProfileNote();
                    }

                    //UAT-2440: Profile level custom attributes by tenant.
                    if (CurrentViewContext.IsApplicant)
                    {
                        CurrentViewContext.ProfileCustomAttributeList = caProfileCustomAttributes.GetCustomAttributeValues();
                        Presenter.SaveUpdateProfileCustomAttributes();
                    }

                    lblMessage.Visible = true;
                    //CurrentViewContext.SuccessMessage += "<br />Data updated successfully.";
                    //base.ShowSuccessMessage(CurrentViewContext.SuccessMessage);
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "SetOnChangeEventOnRadioButtonList();", true);

                    //if (CurrentViewContext.UpdateAspnetEmail)
                    //{
                    //    CurrentViewContext.PswdRecoveryEmail = CurrentViewContext.PrimaryEmail;
                    //}
                    if (CurrentViewContext.IsApplicant)
                    {
                        txtPrimaryEmail.Text = CurrentViewContext.CurrentEmailAddress;
                        txtConfrimPrimayEmail.Text = CurrentViewContext.CurrentEmailAddress;
                        ResidentialHistoryList = CurrentViewContext.ResidentialHistoryList = Presenter.GetResidentialHistories(CurrentViewContext.loggedInUserId);
                    }

                    if (NotifyProfileUpdate.IsNotNull())
                    {
                        NotifyProfileUpdate(_myOrgUser);
                    }

                    //imgCntrl.ImageUrl = String.Format("/ComplianceOperations/UserControl/DocumentViewer.aspx?UserId={0}&DocumentType={1}&_={2}", loggedInUserId, "ProfilePicture", DateTime.Now.Millisecond.ToString());
                    UpdateInitials();

                    if (((SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser).OrganizationUserId.Equals(CurrentViewContext.loggedInUserId))
                    {
                        UpdateUserInSession();
                    }

                    //UAT-1578 : Addition for SMS Notification
                    HideShowControlsForSMSNotification();
                    //UAT-2447
                    ShowHidePhoneControls(chkPrimaryPhone.Checked, 1);
                    ShowHidePhoneControls(chkSecondaryPhone.Checked, 2);

                    //UAT-3068
                    //TwoFactorAuthentication.IsSMSSubscriptionConfirmed();

                    //UAT-1612 : As an applicant, my middle name should be required.
                    if (chkMiddleNameRequired.Checked)
                    {
                        ValidateMiddleName(String.Empty);
                    }
                    else
                    {
                        ValidateMiddleName(txtMiddleName.Text);
                    }

                    #region UAT-3084
                    Boolean isNeedToShowSuccessMessage = true;

                    if (CurrentViewContext.IsPersonAliasAddUpdate)
                    {
                        List<RejectedItemListContract> lstRejectedItemListContract = Presenter.GetRejectedItemListForReSubmission();
                        if (!lstRejectedItemListContract.IsNullOrEmpty() && lstRejectedItemListContract.Count > AppConsts.NONE)
                        {
                            isNeedToShowSuccessMessage = false;
                            //creatd popup here.
                            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "NavigateToRejectedItemListSubmission('" + CurrentViewContext.loggedInUserId + "','" + CurrentViewContext.SelectedTenantId + "');", true);
                        }
                    }

                    if (isNeedToShowSuccessMessage)
                    {
                        errMsgUserGroup.Visible = false;
                        errMsgUserGroup.Text = "";
                        CurrentViewContext.SuccessMessage += "<br />" + Resources.Language.DATAUPDATEDSUCCESSFULLY;
                        //CurrentViewContext.SuccessMessage += "<br />Data updated successfully.";                        
                        base.ShowSuccessMessage(CurrentViewContext.SuccessMessage);
                        ucPersonAlias.ResetAliasNewControls();
                    }
                    #endregion

                }
                else
                {
                    //base.ShowInfoMessage("Data is not saved.");
                    base.ShowInfoMessage(Resources.Language.DATANOTSAVED);
                }
            }
            else
            {
                if (!CurrentViewContext.SMSNotificationErrorMessage.IsNullOrEmpty())
                {
                    base.ShowInfoMessage(CurrentViewContext.SMSNotificationErrorMessage);
                    //if (CurrentViewContext.IsReceiveTextNotification)
                    //{
                    //    divHideShowPhoneNumber.Style["display"] = "block";
                    //}
                }
            }
        }

        /// <summary>
        /// Add to ApplicantUserGroupMappingContract to save the UserGroup Custom Attributes.
        /// </summary>
        /// <param name="lstApplicantuserGroupMapping"></param>
        /// <param name="lstUserGroupIDs"></param>
        private void AddToApplicantUserGroupMappingList(List<ApplicantUserGroupMapping> lstApplicantuserGroupMapping, List<Int32> lstUserGroupIDs)
        {
            if (!lstUserGroupIDs.IsNullOrEmpty())
            {
                foreach (Int32 userGroupID in lstUserGroupIDs)
                {
                    ApplicantUserGroupMapping applicantUserGroupMapping = new ApplicantUserGroupMapping();
                    applicantUserGroupMapping.AUGM_UserGroupID = userGroupID;
                    applicantUserGroupMapping.AUGM_OrganizationUserID = loggedInUserId;
                    applicantUserGroupMapping.AUGM_IsDeleted = false;
                    applicantUserGroupMapping.AUGM_CreatedByID = CurrentViewContext.OrgUsrID;
                    applicantUserGroupMapping.AUGM_CreatedOn = DateTime.Now;

                    lstApplicantuserGroupMapping.Add(applicantUserGroupMapping);

                }
            }
        }

        /// <summary>
        /// On Click of cancel button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucCmdBar1_CancelClick(object sender, EventArgs e)
        {
            try
            {
                RedirectToApplicantdashboard(false);
            }
            //Do not log thread abort exception if it is caused by Response.Redirect or Response.End
            //catch (ThreadAbortException thex)
            //{
            //    //You can ignore this 
            //}
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
            //Dictionary<String, String> queryString = new Dictionary<String, String>();
            //queryString = new Dictionary<String, String>
            //                                                     { 

            //                                                        { "Child", @"ApplicantDashboard.ascx"},

            //                                                     };
            //Response.Redirect(String.Format("~/Main/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString()));
        }

        protected void lnkGoBack_click(object sender, EventArgs e)
        {
            RedirectToApplicantdashboard(false);
        }

        /// <summary>
        /// Method for uploading and saving of profile picture
        /// </summary>
        private void CheckAndSaveProfilePic()
        {
            try
            {
                if (uploadControl.UploadedFiles.Count > 0)
                {
                    Entity.OrganizationUser organizationUser = SecurityManager.GetOrganizationUser(loggedInUserId);
                    Boolean aWSUseS3 = false;
                    if (!ConfigurationManager.AppSettings["AWSUseS3"].IsNullOrEmpty())
                    {
                        aWSUseS3 = Convert.ToBoolean(ConfigurationManager.AppSettings["AWSUseS3"]);
                    }
                    DeleteOriginalFile(organizationUser, aWSUseS3);
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
                        String date = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("mmss");
                        String fileName = "UP_" + CurrentViewContext.SelectedTenantId.ToString() + "_" + loggedInUserId.ToString() + "_" + date + Path.GetExtension(uploadControl.UploadedFiles[0].FileName);

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
                        //String fileName = Guid.NewGuid().ToString() + Path.GetExtension(uploadControl.UploadedFiles[0].FileName);
                        tempFilePath = Path.Combine(tempFilePath, fileName);
                        uploadControl.UploadedFiles[0].SaveAs(tempFilePath);
                        FilePath = WebConfigurationManager.AppSettings[AppConsts.APPLICANT_FILE_LOCATION];
                        //Check whether use AWS S3, true if need to use
                        if (aWSUseS3 == false)
                        {
                            if (FilePath.IsNullOrEmpty())
                            {
                                base.LogError("Please provide path for " + AppConsts.APPLICANT_FILE_LOCATION + " in config", null);
                                return;
                            }
                            if (!FilePath.EndsWith(@"\"))
                            {
                                FilePath += @"\";
                            }
                            FilePath += "Tenant(" + CurrentViewContext.SelectedTenantId.ToString() + @")\";
                            FilePath = FilePath + @"Pics\";
                            if (!Directory.Exists(FilePath))
                                Directory.CreateDirectory(FilePath);

                            FilePath = Path.Combine(FilePath, fileName);
                            MoveFile(tempFilePath, FilePath);
                        }
                        else
                        {
                            if (FilePath.IsNullOrEmpty())
                            {
                                base.LogError("Please provide path for " + AppConsts.APPLICANT_FILE_LOCATION + " in config", null);
                                return;
                            }
                            if (!FilePath.EndsWith("//"))
                            {
                                FilePath += "//";
                            }
                            //AWS code to save document to S3 location
                            AmazonS3Documents objAmazonS3 = new AmazonS3Documents();
                            String destFolder = FilePath + "Tenant(" + CurrentViewContext.SelectedTenantId.ToString() + @")/" + @"Pics/";
                            String returnFilePath = objAmazonS3.SaveDocument(tempFilePath, fileName, destFolder);
                            try
                            {
                                if (!String.IsNullOrEmpty(tempFilePath))
                                    File.Delete(tempFilePath);
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
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Method to delete the photo when new photo is uploaded
        /// </summary>
        /// <param name="organizationUser"></param>
        private static void DeleteOriginalFile(Entity.OrganizationUser organizationUser, Boolean aWSUseS3)
        {
            var oldPhotoPath = organizationUser.PhotoName;
            //Check whether use AWS S3, true if need to use
            if (aWSUseS3 == false)
            {
                if (System.IO.File.Exists(oldPhotoPath))
                {
                    System.IO.File.Copy(oldPhotoPath, String.Concat(oldPhotoPath.Substring(0, oldPhotoPath.LastIndexOf(".")), "_Deleted", oldPhotoPath.Substring(oldPhotoPath.LastIndexOf("."))), true);
                    System.IO.File.Delete(oldPhotoPath);
                }
            }
            else
            {
                AmazonS3Documents objAmazonS3Documents = new AmazonS3Documents();
                objAmazonS3Documents.DeleteDocument(oldPhotoPath);
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

        private void RedirectToApplicantdashboard(bool ShowSuccessMessage)
        {
            String childcontrolPath;
            if (PageType == "MyProfile")
            {
                //childcontrolPath = ChildControls.ExternalDashboard;
                //Dictionary<String, String> queryString = new Dictionary<String, String>
                //                                                 { 
                //                                                    { "Child", childcontrolPath}
                //                                                 };
                //string url = String.Format("~/DashBoard/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                Response.Redirect(String.Format("~/Main/Default.aspx"), true);
            }
            else
            {
                childcontrolPath = ChildControls.ApplicantSearch;

                Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "Child",childcontrolPath},
                                                                    {"CancelClicked", "CancelClicked"}
                                                                 };
                string url = String.Format("~/SearchUI/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                Response.Redirect(url, true);
            }
        }

        /// <summary>
        /// Method to update the LastName and FirstName in session service (SysXMembershipUser) to update the userName link on Update the profile.
        /// </summary>
        private void UpdateUserInSession()
        {
            //SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
            //String loginUserName = user.UserName;
            String loginUserName = CurrentViewContext.UserName;

            System.Web.HttpContext.Current.Items.Add("GetDataByOrgUserID", CurrentViewContext.loggedInUserId);
            SysXMembershipUser user = System.Web.Security.Membership.GetUser(loginUserName) as SysXMembershipUser;
            System.Web.HttpContext.Current.Items.Remove("GetDataByOrgUserID");

            SysXWebSiteUtils.SessionService.SetSysXMembershipUser(user);
            String userName = ((String.IsNullOrEmpty(user.LastName)) ? "" : user.LastName + ", ") + user.FirstName;
            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), "setUserName", "parent.SetUserLink('" + userName + "');", true);
        }

        #region Upload Control Events

        /// <summary>
        /// save uploaded profile photo and show on screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpload_Click(Object sender, EventArgs e)
        {
            try
            {
                CheckAndSaveProfilePic();
                //UAT-862 :- WB: As a student or an admin, I should not be allowed to upload documents with a size of 0 
                if (_isCorruptedFileUploaded)
                {
                    //String corruptedFileMessage = "Your profile picture is not uploaded.Please try again. ";
                    String corruptedFileMessage = Resources.Language.PROFILEPICNTUPLOADED;
                    //base.ShowErrorInfoMessage("Your profile picture is not uploaded.Please try again. ");
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ShowCallBackMessage('" + corruptedFileMessage + "');", true);
                }
                else if (Presenter.SaveProfilePhoto())
                {
                    imgCntrl.ImageUrl = String.Format("/ComplianceOperations/UserControl/DocumentViewer.aspx?UserId={0}&DocumentType={1}&_={2}", loggedInUserId, "ProfilePicture", DateTime.Now.Millisecond.ToString());
                    UpdateInitials();
                }

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

        /// <summary>
        /// To Validate Username
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCheckUsername_Click(object sender, EventArgs e)
        {
            lblUserNameMessage.Text = String.Empty;
            if (String.IsNullOrEmpty(CurrentViewContext.UserName.Trim()))
            {
                //lblUserNameMessage.Text = "Username is required.";
                lblUserNameMessage.Text = Resources.Language.USERNAMEREQ;
                lblUserNameMessage.ForeColor = System.Drawing.Color.Red;
                //hdnIsValidatedCurrent.Value = "1";
                //ScriptManager.RegisterStartupScript(this, Page.GetType(), Guid.NewGuid().ToString(), "ValidateUsername();", true);
            }
            else if (Regex.IsMatch(CurrentViewContext.UserName, _reg) == false)
            {
                //lblUserNameMessage.Text = SysXUtils.GetMessage(ResourceConst.SECURITY_INVALID_CHARACTER);
                //lblUserNameMessage.Text = "Invalid username. Must have at least 4 chars (A-Z a-z 0-9 . _ - @).";
                lblUserNameMessage.Text = Resources.Language.INVALIDUSERNAME;
                lblUserNameMessage.ForeColor = System.Drawing.Color.Red;
                //hdnIsValidatedCurrent.Value = "1";
            }
            else
            {
                SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                if (CurrentViewContext.IsApplicant && Presenter.IsExistsUserName(user.UserId))
                {
                    //lblUserNameMessage.Text = "This Username is not available. Try another.";
                    lblUserNameMessage.Text = Resources.Language.USERNAMEALREADYINUSE;
                    lblUserNameMessage.ForeColor = System.Drawing.Color.Red;
                    //hdnIsValidatedCurrent.Value = "0";
                }
                else
                {
                    //lblUserNameMessage.Text = "This Username is available.";
                    lblUserNameMessage.Text = Resources.Language.USERNAMEAVLBLE;
                    lblUserNameMessage.ForeColor = System.Drawing.Color.Green;
                    //hdnIsValidatedCurrent.Value = "0";
                }
            }
        }

        protected void btnLinkAccount_Click(object sender, EventArgs e)
        {
            _viewType = "Applicant";
            Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "Child",ChildControls.OtherAccountLinking}
                                                                 };
            string url = String.Format("~/IntsofSecurityModel/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
            Response.Redirect(url, true);
        }

        #endregion

        #region Repeater Event

        protected void rptrCustomAttribute_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                CustomAttributeLoader loader = e.Item.FindControl("customAttribute") as CustomAttributeLoader;
                if (loader.IsNotNull())
                {
                    loader.TenantId = CurrentViewContext.SelectedTenantId;
                    loader.TypeCode = CustomAttributeUseTypeContext.Hierarchy.GetStringValue();
                    loader.CurrentLoggedInUserId = CurrentViewContext.loggedInUserId;
                    loader.DataSourceModeType = DataSourceMode.Ids;
                    loader.ControlDisplayMode = DisplayMode.Controls;
                    loader.ValidationGroup = "grpFormSubmit";
                    loader.IsIntegrationClientOrganisationUser = IsuserIntegrationClient;
                    if (PageType != "ApplicantSearch")
                    {
                        loader.NeedTocheckCustomAttributeEditableSetting = true; //UAT 4829
                    }

                    //UAT 1438: Enhancement to allow students to select a User Group. Check where page is loaded from orderflow or not.
                    loader.ShowUserGroupCustomAttribute = false;
                }
            }

        }
        protected void rptrCustomAttribute_Load(object sender, EventArgs e)
        {
            try
            {
                BindCustomAttribute();
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
        #region UAT-2031:Custom attribute section is appearing on applicant's profile, if applicant has purchased the order from node which doesn't have custom attributes mapped.
        protected void rptrCustomAttribute_PreRender(object sender, EventArgs e)
        {
            try
            {
                Boolean isCustomAttributeExist = false;
                foreach (RepeaterItem repeated in rptrCustomAttribute.Items)
                {
                    CustomAttributeLoader CustomAttributeLoader = repeated.FindControl("customAttribute") as CustomAttributeLoader;
                    if (!CustomAttributeLoader.IsNullOrEmpty() && !CustomAttributeLoader.lstTypeCustomAttributes.IsNullOrEmpty())
                    {
                        isCustomAttributeExist = true;
                        break;
                    }
                }
                if (!isCustomAttributeExist && !CurrentViewContext.IsUserGroupCustomAttributeExist)
                {
                    divcontent.Visible = false;
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

        #region RadioButton Event

        protected void rblSSN_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToBoolean(rblSSN.SelectedValue.ToLower()))
                {
                    txtSSN.Text = String.Empty;
                    txtSSN.Enabled = true;
                    rfvSSN.Enabled = true;
                    if (CurrentViewContext.IsLocationServiceTenant)
                    {
                        dvSSNMain.Visible = true;
                        rgvSSNCBI.Enabled = true;
                        revtxtSSN.Enabled = false;
                    }

                }
                else
                {
                    txtSSN.Text = AppConsts.DefaultSSN;
                    rfvSSN.Enabled = false;
                    txtSSN.Enabled = false;
                    if (CurrentViewContext.IsLocationServiceTenant)
                    {
                        dvSSNMain.Visible = false;
                        rgvSSNCBI.Enabled = false;
                        revtxtSSN.Enabled = true;
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

        #region Private Method

        private void BindCustomAttribute()
        {
            try
            {
                if (CurrentViewContext.lstApplicantInstitutionHierarchyMapping.IsNotNull())
                {
                    rptrCustomAttribute.DataSource = CurrentViewContext.lstApplicantInstitutionHierarchyMapping.Where(cond => cond.RecordID != null);
                    rptrCustomAttribute.DataBind();
                    if (rptrCustomAttribute.Items.Count == 0 && rptrCustomAttribute.IsNotNull())
                    {
                        divcontent.Visible = false;
                    }
                    else
                    {
                        divcontent.Visible = true;
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

        #region UAT-806 Creation of granular permissions for Client Admin users

        /// <summary>
        /// Hide Show grid and page controls
        /// </summary>
        private void HideShowControlsForGranularPermission()
        {
            if (divSSNMasked.Visible == true)
            {
                divSSN.Visible = false;
            }
            else
            {
                divSSN.Visible = !(CurrentViewContext.IsSSNDisabled);
            }
            if (PageType != "MyProfile")
            {
                if (CurrentViewContext.IsDOBDisable)
                {
                    divDOB.Visible = false;
                }
                if (!CurrentViewContext.IsSSNDisabled && CurrentViewContext.SSNPermissionCode.ToUpper() == EnumSystemPermissionCode.NO_ACCESS_PERMISSION.GetStringValue().ToUpper())
                {
                    divSSN.Visible = false;
                }
            }
        }
        #endregion

        /// <summary>
        /// Method to avoid registration with 0 ZipCodeId for USA address - UAT 934
        /// </summary>
        private Boolean IsValidAddress()
        {
            if (locationTenant.MasterZipcodeID.Value == AppConsts.NONE && locationTenant.RSLCountryId == AppConsts.COUNTRY_USA_ID)
                return false;
            return true;
        }

        /// <summary>
        /// UAT-1578 : Addition of SMS notification
        /// </summary>
        private void HideShowControlsForSMSNotification(Boolean isRefreshLoading = true)
        {
            if (CurrentViewContext.IsApplicant)
            {
                if (isRefreshLoading)
                {
                    divSMSNotification.Visible = true;
                    //Presenter.UpdateSubscriptionStatusFromAmazon();
                    Presenter.GetUserSMSNotificationData();
                    if (!CurrentViewContext.OrganisationUserTextMessageSettingData.IsNullOrEmpty()
                        && CurrentViewContext.OrganisationUserTextMessageSettingData.OUTMS_ID > AppConsts.NONE
                        && !CurrentViewContext.OrganisationUserTextMessageSettingData.OUTMS_IsDeleted)
                    {
                        CurrentViewContext.IsReceiveTextNotification = CurrentViewContext.OrganisationUserTextMessageSettingData.OUTMS_ReceiveTextNotification;
                    }

                    bool isPhoneNumberRequired = CurrentViewContext.IsReceiveTextNotification;

                    TwoFactorAuthentication.Presenter.CheckAuthenticationType();

                    if (TwoFactorAuthentication.SelectedAuthenticationType == INTSOF.Utils.AuthenticationMode.Text_Message.GetStringValue())
                        isPhoneNumberRequired = true;

                    //divHideShowPhoneNumber.Style["display"] = CurrentViewContext.IsReceiveTextNotification ? "block;" : "none;";
                    hdnIsConfirmMsgVisible.Value = isPhoneNumberRequired ? "1" : "0";
                    CurrentViewContext.PhoneNumber = CurrentViewContext.OrganisationUserTextMessageSettingData.IsNullOrEmpty() ? "" : CurrentViewContext.OrganisationUserTextMessageSettingData.OUTMS_MobileNumber;
                    rfvPhoneNumber.Enabled = isPhoneNumberRequired;
                    spnPhoneNumberReq.Style["display"] = isPhoneNumberRequired == false ? "none" : "";
                }
                else
                {
                    bool isPhoneNumberRequired = CurrentViewContext.IsReceiveTextNotification;

                    TwoFactorAuthentication.Presenter.CheckAuthenticationType();

                    if (TwoFactorAuthentication.SelectedAuthenticationType == INTSOF.Utils.AuthenticationMode.Text_Message.GetStringValue())
                        isPhoneNumberRequired = true;

                    if (!isPhoneNumberRequired)
                    {
                        //divHideShowPhoneNumber.Style["display"] = "none";
                        //divConfirmationStatus.Style["display"] = "none";
                        //lnkReSendSubMessage.Visible = false;
                        rfvPhoneNumber.Enabled = false;
                        spnPhoneNumberReq.Style["display"] = "none";
                    }
                    else
                    {
                        //divHideShowPhoneNumber.Style["display"] = "block";
                        //divConfirmationStatus.Style["display"] = hdnIsConfirmMsgVisible.Value == "1" ? "block" : "none";
                        rfvPhoneNumber.Enabled = true;
                        spnPhoneNumberReq.Style["display"] = "";
                    }
                }
            }
            else
            {
                divSMSNotification.Visible = false;
            }
        }

        /// <summary>
        /// UAT-1612 : As an applicant, my middle name should be required.
        /// </summary>
        /// <param name="middleName"></param>
        private void ValidateMiddleName(String middleName)
        {
            if (CurrentViewContext.IsApplicant)
            {
                if (middleName.IsNullOrEmpty())
                {
                    divMiddleNameCheckBox.Visible = true;
                    chkMiddleNameRequired.Checked = true;
                    txtMiddleName.Text = NoMiddleNameText;
                    txtMiddleName.Enabled = false;
                    rfvMiddleName.Enabled = false;
                    spnMiddleName.Style["display"] = "none";

                    if (!CurrentViewContext.IsLocationServiceTenant)
                    {
                        txtMiddleName.Attributes.Add("PlaceHolder", Resources.Language.IFYOUDONTHAVEMIDDLENAME);
                        txtMiddleName.ToolTip = Resources.Language.IFYOUDONTHAVEMIDDLENAME;
                    }


                }
                else
                {
                    divMiddleNameCheckBox.Visible = true;
                    chkMiddleNameRequired.Checked = false;
                    txtMiddleName.Text = middleName;
                    txtMiddleName.Enabled = true;
                    rfvMiddleName.Enabled = true;
                    spnMiddleName.Style["display"] = "";
                    spnMiddleName.Visible = true;
                    txtMiddleName.ToolTip = Resources.Language.IFYOUDONTHAVEMIDDLENAME;
                    txtMiddleName.Attributes.Add("PlaceHolder", Resources.Language.IFYOUDONTHAVEMIDDLENAME);
                }
            }
            else
            {
                divMiddleNameCheckBox.Visible = false;
                txtMiddleName.Text = middleName;
                rfvMiddleName.Enabled = false;
                spnMiddleName.Style["display"] = "none";
            }
        }

        private void ShowHidePhoneControls(Boolean IsInternationalNumber, Int32 ControlType)
        {
            if (ControlType == AppConsts.ONE)
            {
                if (IsInternationalNumber)
                {
                    dvUnmaskedPrimaryPhone.Style["display"] = "block";
                    dvMaskedPrimaryPhone.Style["display"] = "none";
                    revTxtMobile.Enabled = false;
                    rfvTxtMobile.Enabled = false;
                    rfvTxtMobileUnmasked.Enabled = true;
                    revTxtMobilePrmyNonMasking.Enabled = true;
                }

                else
                {

                    dvUnmaskedPrimaryPhone.Style["display"] = "none";
                    dvMaskedPrimaryPhone.Style["display"] = "block";
                    revTxtMobile.Enabled = true;
                    rfvTxtMobile.Enabled = true;
                    rfvTxtMobileUnmasked.Enabled = false;
                    revTxtMobilePrmyNonMasking.Enabled = false;
                }
            }
            if (ControlType == AppConsts.TWO)
            {
                if (IsInternationalNumber)
                {
                    dvUnMaskedSecondaryPhone.Style["display"] = "block";
                    dvMaskedSecondaryPhone.Style["display"] = "none";
                }

                else
                {
                    dvUnMaskedSecondaryPhone.Style["display"] = "none";
                    dvMaskedSecondaryPhone.Style["display"] = "block";
                }
            }
        }

        //UAT-2930
        private void ShowhideTwoFactorAuthentication()
        {
            String userId = SysXWebSiteUtils.SessionService.UserId;
            if (!userId.IsNullOrEmpty() && Presenter.ShowhideTwoFactorAuthentication(userId))
            {
                dvTwoFactorAuthentication.Style["display"] = "block";
            }
            else
            {
                dvTwoFactorAuthentication.Style["display"] = "none";
            }
        }

        /// <summary>
        /// Method to select the radion button for 'Do you have SSN?' on the basis of the SSN of Applicant.
        /// </summary>
        private void ManageSSNRdBtn()
        {
            String AppSSN = txtSSN.Text.Trim();
            if (AppSSN == AppConsts.DefaultSSN)
            {
                rblSSN.SelectedValue = "false";
                rfvSSN.Enabled = false;
                txtSSN.Enabled = false;
                if (CurrentViewContext.IsLocationServiceTenant)
                {
                    dvSSNMain.Visible = false;
                    rgvSSNCBI.Enabled = false;
                    revtxtSSN.Enabled = true;
                }
            }
            else
            {
                rblSSN.SelectedValue = "true";
                txtSSN.Enabled = true;
                rfvSSN.Enabled = true;
                if (CurrentViewContext.IsLocationServiceTenant)
                {
                    dvSSNMain.Visible = true;
                    rgvSSNCBI.Enabled = true;
                    revtxtSSN.Enabled = false;
                }
            }
        }

        private void ValidatePersonalInformation(Boolean IsLocationTenant)
        {
            if (IsLocationTenant)
            {
                revFirstName.ValidationExpression = "^(?=.{1,30}$)(([a-zA-Z])+( ?[a-zA-Z]+)|([a-zA-Z]{1}))$";
                //revSuffix.ValidationExpression = "^(?=.{1,10}$)(([a-zA-Z])+( ?[a-zA-Z]+)|([a-zA-Z]{1}))$";
                revMiddleName.ValidationExpression = "^(?=.{1,30}$)((([a-zA-Z])+( ?[a-zA-Z]+))|(-{5})|([a-zA-Z]{1}))$";
                revLastName.ValidationExpression = "^(?=.{1,30}$)(([a-zA-Z])+(-?[a-zA-Z]+)|([a-zA-Z]{1}))$";
                revSuffix.ValidationExpression = "^[a-z A-Z]*-?[a-z A-Z]*$";
                //revSuffix.ErrorMessage = "Only alphabets or single hyphen(-) is allowed in the suffix up to maximum of 10 characters.";
                //revFirstName.ErrorMessage = "Only alphabets and one space between the first names are allowed up to maximum of 30 characters.";
                //revMiddleName.ErrorMessage = "Only alphabets and one space between the middle names are allowed up to maximum of 30 characters.";
                //revLastName.ErrorMessage = "Only alphabets or single hyphen(-) is allowed in the last name up to maximum of 30 characters.";

                revSuffix.ErrorMessage = Resources.Language.SUFFIXNAMEVALDT;
                revFirstName.ErrorMessage = Resources.Language.FIRSTNAMEVALDT;
                revMiddleName.ErrorMessage = Resources.Language.MIDDLENAMEVALDT;
                revLastName.ErrorMessage = Resources.Language.LASTNAMEVALDT;
            }
            else
            {
                revFirstName.ValidationExpression = "^[\\w\\d\\s\\-\\.\\']{1,30}$";
                revMiddleName.ValidationExpression = "^[\\w\\d\\s\\-\\.\\']{1,30}$";
                revSuffix.ValidationExpression = "^[\\w\\d\\s\\-\\.\\']{1,10}$";
                revLastName.ValidationExpression = "^[\\w\\d\\s\\-\\.\\']{1,30}$";
            }

        }

        private void AddSuffixDropdownAndDesignChange()
        {
            if (CurrentViewContext.IsLocationServiceTenant)
            {
                dvSpnFirstName.Style.Add("Width", "10%");
                dvSpnMiddleName.Style.Add("Width", "10%");
                dvSpnLastName.Style.Add("Width", "10%");

                dvFirstName.Style.Add("Width", "17%");
                dvMiddleName.Style.Add("Width", "17%");
                dvLastName.Style.Add("Width", "17%");

                dvSuffix.Style.Add("display", "inline-block");
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
        }

        /// <summary>
        /// Bind suffix dropdown, only in case of location service tenant.
        /// </summary>
        private void BindSuffixDropdown()
        {

            cmbSuffix.DataSource = CurrentViewContext.lstSuffixes.Where(a => a.IsSystem).ToList();
            cmbSuffix.DataBind();
            cmbSuffix.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(Resources.Language.SELECTSUFFIX, "0"));
        }

        #endregion


        #region 4280
        private void BindUserGroup()
        {
            try
            {
                Presenter.GetUserGroupsForUser();

                // String usrGrpCode = CustomAttributeDatatype.User_Group.GetStringValue().ToLower();
                Presenter.GetAllUserGroup();
                cmdUserGroup.DataSource = CurrentViewContext.lstUserGroups;
                cmdUserGroup.DataBind();
                if (!CurrentViewContext.lstUserGroupsForUser.IsNullOrEmpty())
                {
                    foreach (RadComboBoxItem item in cmdUserGroup.Items)
                    {
                        var Temp = CurrentViewContext.lstUserGroupsForUser.Where(x => x.UG_ID ==
                          Convert.ToInt32(item.Value)).FirstOrDefault();
                        if (Temp != null)
                        {

                            item.Checked = true;
                            if (PageType == "MyProfile")
                            {
                                item.Enabled = false;
                            }
                        }
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

    }
}