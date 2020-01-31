#region Namespaces

#region SystemDefined

using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Practices.ObjectBuilder;

#endregion

#region UserDefined

using Entity.ClientEntity;
using INTSOF.Utils;
using CoreWeb.Shell;
using INTSOF.UI.Contract.ComplianceOperation;
using CoreWeb.IntsofSecurityModel;
using System.Configuration;
using System.Web.Configuration;
using System.IO;
using System.Web.UI.WebControls;
using CoreWeb.ComplianceAdministration.Views;
using Business.RepoManagers;
using Entity;
using Telerik.Web.UI;


#endregion

#endregion

namespace CoreWeb.Search.Views
{
    public partial class ApplicantPortfolioProfile : BaseUserControl, IApplicantPortfolioProfileView
    {
        #region Variables

        #region Private Variables

        private ApplicantPortfolioProfilePresenter _presenter = new ApplicantPortfolioProfilePresenter();
        private String _viewType;
        //UAT-862 :- WB: As a student or an admin, I should not be allowed to upload documents with a size of 0 
        private Boolean _isCorruptedFileUploaded = false;
        private Boolean? _isAdminLoggedIn = null;
        #endregion

        //#region Delegates and Events
        //public delegate void ReloadTwoFactor(object sender);
        //public event ReloadTwoFactor EventReloadTwoFactor;
        //#endregion

        #region Public Variables


        #endregion

        #endregion

        #region Properties

        #region Private Properties

        #region UAT-2169:Send Middle Name and Email address to clearstar in Complio

        public String NoMiddleNameText
        {
            get
            {
                String noMiddleNameText = String.Empty;
                if (!CurrentViewContext.IsLocationServiceTenant) // Check for location service tenant, if it is a loaction service tenant then no middle name text should be blank rather than '-----'.
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

        #region Public Properties


        public ApplicantPortfolioProfilePresenter Presenter
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

        public List<PersonAliasContract> PersonAliasList
        {
            get
            {
                return ucPersonAlias.PersonAliasList;
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

        public IApplicantPortfolioProfileView CurrentViewContext
        {
            get { return this; }
        }

        public Int32 CurrentLoggedInUserId
        {
            get
            {
                hdncurrentLoggedInUserId.Value = Convert.ToString(base.CurrentUserId);
                return base.CurrentUserId;
            }
        }

        public Int32 OrganizationUserId
        {
            get
            {
                Dictionary<String, String> args = new Dictionary<String, String>();
                if (Request.QueryString["args"].IsNotNull())
                {
                    args.ToDecryptedQueryString(Request.QueryString["args"]);

                    if (args.ContainsKey("OrganizationUserId"))
                    {
                        //
                        hdnorganizationUserId.Value = args["OrganizationUserId"];
                        return (Convert.ToInt32(args["OrganizationUserId"]));
                    }
                }
                hdnorganizationUserId.Value = Convert.ToString(base.CurrentUserId);
                return base.CurrentUserId;
            }
        }

        public Entity.OrganizationUser OrganizationUser
        {
            get
            {
                if (ViewState["OrganizationUser"] != null)
                    return (Entity.OrganizationUser)ViewState["OrganizationUser"];
                return null;
            }
            set
            {
                ViewState["OrganizationUser"] = value;
                if (value != null)
                    AddressHandleId = value.AddressHandleID;
            }
        }

        public Int32 TenantId
        {
            get
            {
                Dictionary<String, String> args = new Dictionary<String, String>();
                if (Request.QueryString["args"].IsNotNull())
                {
                    args.ToDecryptedQueryString(Request.QueryString["args"]);

                    if (args.ContainsKey("TenantId"))
                    {
                        return (Convert.ToInt32(args["TenantId"]));
                    }
                }
                SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                if (user.IsNotNull())
                {
                    return user.TenantId.HasValue ? user.TenantId.Value : 0;
                }
                return 0;


                // return Presenter.GetTenantID();
            }
        }

        public Guid? AddressHandleId
        {
            get
            {
                if (ViewState["AddressHandleId"] != null)
                    return (Guid)ViewState["AddressHandleId"];
                return Guid.NewGuid();
            }
            set
            {
                ViewState["AddressHandleId"] = value;
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

        public Int32 ZipCodeId
        {
            get;
            set;
        }

        public Entity.ZipCode ApplicantZipCodeDetails
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
        #endregion

        //UAT-781
        public String DecryptedSSN { get; set; }

        public List<Entity.lkpGender> GenderList
        {
            set
            {
                cmbGender.DataSource = value;
                cmbGender.DataBind();
            }
        }

        String IApplicantPortfolioProfileView.UserName
        {
            get
            {
                return txtUsername.Text.Trim();
            }
        }
        String IApplicantPortfolioProfileView.FirstName
        {
            get
            {
                return txtFirstName.Text.Trim();
            }
        }
        String IApplicantPortfolioProfileView.LastName
        {
            get
            {
                return txtLastName.Text.Trim();
            }
        }
        String IApplicantPortfolioProfileView.MiddleName
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

        String IApplicantPortfolioProfileView.SSN
        {
            get
            {
                return txtSSN.Text.Trim();
            }
        }

        DateTime? IApplicantPortfolioProfileView.DOB
        {
            get
            {
                return dpkrDOB.SelectedDate;
            }
        }
        Int32 IApplicantPortfolioProfileView.SelectedGenderId
        {
            get
            {
                return Convert.ToInt32(cmbGender.SelectedValue);
            }
        }
        String IApplicantPortfolioProfileView.PrimaryPhone
        {
            get
            {
                if (chkIsMaskingRequiredPrimary.Checked)
                    return txtPrimaryPhoneNonMasking.Text;
                else
                    return txtPrimaryPhone.Text;
            }
        }

        String IApplicantPortfolioProfileView.PrimaryEmail
        {
            get
            {
                return txtPrimaryEmail.Text.Trim();
            }
        }

        String IApplicantPortfolioProfileView.PswdRecoveryEmail
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

        String IApplicantPortfolioProfileView.CurrentEmailAddress
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
        String IApplicantPortfolioProfileView.Address1
        {
            get
            {
                return txtAddress1.Text;
            }
        }

        String IApplicantPortfolioProfileView.Address2
        {
            get
            {
                return txtAddress2.Text;
            }
        }

        Int32 IApplicantPortfolioProfileView.ZipId
        {
            get
            {
                return locationTenant.MasterZipcodeID.Value;
            }
        }
        String IApplicantPortfolioProfileView.SecondaryPhone
        {
            get
            {
                if (chkIsMaskingRequiredSecondary.Checked)
                    return txtSecondaryPhoneNonMasking.Text;
                else
                    return txtSecondaryPhone.Text;
            }
        }

        String IApplicantPortfolioProfileView.SecondaryEmail
        {
            get
            {
                return txtSecondaryEmail.Text;
            }
        }

        List<ApplicantInstitutionHierarchyMapping> IApplicantPortfolioProfileView.lstApplicantInstitutionHierarchyMapping
        {
            get;
            set;
        }

        public Boolean UpdateAspnetEmail
        {
            get
            {
                return chkChangeEmail.Checked;
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

        DateTime? IApplicantPortfolioProfileView.DateResidentFrom
        {
            get
            {
                return dpCurResidentFrom.SelectedDate;
            }
        }

        String IApplicantPortfolioProfileView.StateName
        {
            get
            {
                return locationTenant.RSLStateName;
            }
        }

        String IApplicantPortfolioProfileView.CityName
        {
            get
            {
                return locationTenant.RSLCityName;
            }
        }

        String IApplicantPortfolioProfileView.PostalCode
        {
            get
            {
                return locationTenant.RSLZipCode;
            }
        }

        Int32 IApplicantPortfolioProfileView.CountryId
        {
            get
            {
                return locationTenant.RSLCountryId;
            }
        }

        Boolean IApplicantPortfolioProfileView.IsSSNDisabled
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

        #region UAT-2447
        Boolean IApplicantPortfolioProfileView.IsInternationalPhoneNumber
        {
            get
            {
                return chkIsMaskingRequiredPrimary.Checked;
            }
        }
        Boolean IApplicantPortfolioProfileView.IsInternationalSecondaryPhone
        {
            get
            {
                return chkIsMaskingRequiredSecondary.Checked;
            }
        }
        #endregion

        public String FilePath
        { get; set; }

        public String OriginalFileName
        { get; set; }

        public List<UserNodePermissionsContract> lstUserNodePermissionsContract
        {
            get;
            set;
        }

        Boolean IApplicantPortfolioProfileView.IsUserGroupCustomAttributeExist { get; set; }

        #region UAT-1579
        Boolean IApplicantPortfolioProfileView.IsReceiveTextNotification
        {
            //TODO: 
            get
            {
                return Convert.ToBoolean(rdbTextNotification.SelectedValue);
            }
            set
            {
                rdbTextNotification.SelectedValue = Convert.ToString(value);
            }
        }
        String IApplicantPortfolioProfileView.PhoneNumber
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

        Entity.OrganisationUserTextMessageSetting IApplicantPortfolioProfileView.OrganisationUserTextMessageSettingData
        {
            get
            {
                if (ViewState["ApplicantSMSSubscriptionData"].IsNotNull())
                    return ViewState["ApplicantSMSSubscriptionData"] as Entity.OrganisationUserTextMessageSetting;
                return new Entity.OrganisationUserTextMessageSetting();
            }
            set { ViewState["ApplicantSMSSubscriptionData"] = value; }
        }

        String IApplicantPortfolioProfileView.SMSNotificationErrorMessage { get; set; }

        #endregion


        #region UAT-2930
        //Boolean IApplicantPortfolioProfileView.IsUserTwoFactorAuthenticated
        //{
        //    //TODO: 
        //    get
        //    {
        //        return Convert.ToBoolean(rdbTwoFactorAuth.SelectedValue);
        //    }
        //    set
        //    {
        //        rdbTwoFactorAuth.SelectedValue = Convert.ToString(value);
        //    }
        //}

        String IApplicantPortfolioProfileView.IsUserTwoFactorAuthenticatedPrevious
        {
            get
            {
                if (!ViewState["IsUserTwoFactorAuthenticatedPrevious"].IsNull())
                {
                    return Convert.ToString(ViewState["IsUserTwoFactorAuthenticatedPrevious"]);
                }
                return String.Empty;
            }
            set
            {
                if (value.IsNullOrEmpty())
                {
                    ViewState["IsUserTwoFactorAuthenticatedPrevious"] = INTSOF.Utils.AuthenticationMode.None.GetStringValue();
                }
                else
                {
                    ViewState["IsUserTwoFactorAuthenticatedPrevious"] = value;
                }
            }
        }
        #endregion

        #region UAT-3068
        String IApplicantPortfolioProfileView.SelectedAuthenticationType
        {
            get
            {
                return rdbSpecifyAuthentication.SelectedValue;
            }
            set
            {
                if (value.IsNullOrEmpty())
                {
                    rdbSpecifyAuthentication.SelectedValue = INTSOF.Utils.AuthenticationMode.None.GetStringValue();
                }
                else
                {
                    rdbSpecifyAuthentication.SelectedValue = value;
                }
            }
        }
        #endregion

        public String UserNodePermissionCode
        {
            get
            {
                return Convert.ToString(ViewState["UserNodePermissionCode"]);
            }
            set
            {
                ViewState["UserNodePermissionCode"] = value;
            }
        }

        public String PageType
        {
            get
            {
                return Convert.ToString(ViewState["PageType"]);
            }
            set
            {
                ViewState["PageType"] = value;
            }
        }

        #region UAT-2276:Regression testing and performance optimization
        Boolean IApplicantPortfolioProfileView.IsSMSDataAvailableForSave
        {
            get
            {
                if (!hdnSMSDataAvailableForSave.Value.IsNullOrEmpty())
                {
                    return Convert.ToBoolean(hdnSMSDataAvailableForSave.Value);
                }
                return false;
            }
        }
        #endregion

        List<TypeCustomAttributes> IApplicantPortfolioProfileView.ProfileCustomAttributeList
        {
            get;
            set;
        }
        #endregion
        public Boolean IsAdminLoggedIn
        {
            get
            {
                if (_isAdminLoggedIn.IsNull())
                {
                    Presenter.IsAdminLoggedIn();
                }
                return _isAdminLoggedIn.Value;
            }
            set
            {
                _isAdminLoggedIn = value;
            }
        }
        public Boolean IsControlsHideForSupportPortal
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsControlsHideForSupportPortal"]);
            }
            set
            {
                ViewState["IsControlsHideForSupportPortal"] = value;
            }
        }
        #region UAT-3047
        public Boolean IsActive
        {
            get
            {
                return chkActive.Checked;
            }
            set
            {
                chkActive.Checked = value;
            }
        }

        public Boolean IsLocked
        {
            get
            {
                return chkLocked.Checked;
            }
            set
            {
                chkLocked.Checked = value;
            }
        }

        #endregion

        // CBI || CABS// Release-158
        Boolean IApplicantPortfolioProfileView.IsLocationServiceTenant
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

        List<Entity.lkpSuffix> IApplicantPortfolioProfileView.lstSuffixes
        {
            get
            {
                if (!ViewState["lstSuffixes"].IsNullOrEmpty())
                    return (List<Entity.lkpSuffix>)ViewState["lstSuffixes"];
                return new List<Entity.lkpSuffix>();
            }
            set
            {
                ViewState["lstSuffixes"] = value;
            }
        }

        Int32? IApplicantPortfolioProfileView.SelectedSuffixID
        {
            get
            {
                if (!cmbSuffix.SelectedValue.IsNullOrEmpty())
                    return Convert.ToInt32(cmbSuffix.SelectedValue);
                return null;
            }
        }
        string IApplicantPortfolioProfileView.Suffix
        {
            get
            {
                return txtSuffix.Text.Trim();
            }
            set
            {

            }
        }
        #endregion

        //UAT-4280
        Boolean IApplicantPortfolioProfileView.IsUserGroupExist
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
        #region UAT-4280

        public IQueryable<Entity.ClientEntity.UserGroup> lstUserGroups { get; set; }

        public IList<Entity.ClientEntity.UserGroup> lstUserGroupsForUser { get; set; }


        List<Int32> IApplicantPortfolioProfileView.lstUserGroupIDs

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
        }
        #endregion
        Boolean IApplicantPortfolioProfileView.IsSuffixDropDownType
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

        #region Events
        protected override void OnInit(EventArgs e)
        {
            try
            {
                if (IsControlsHideForSupportPortal)
                {
                    cBarMain.CancelButton.Visible = false;
                    cBarMain.CancelButton.Enabled = false;
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
        protected void Page_Load(object sender, EventArgs e)
        {
            _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];

            if (!this.IsPostBack)
            {
                //Release 158 CBI
                ucPersonAlias.SelectedTenantId = CurrentViewContext.TenantId;
                if (ViewState["OrganizationUser"] != null)
                {
                    CurrentViewContext.OrganizationUser = (Entity.OrganizationUser)ViewState["OrganizationUser"];
                    ucApplicantNotes.OrganizationUser = (Entity.OrganizationUser)ViewState["OrganizationUser"];
                }
                else
                {
                    Presenter.GetOrganizationUser();
                    ucApplicantNotes.OrganizationUser = CurrentViewContext.OrganizationUser;
                }

                Presenter.OnViewInitialized();
                //CBI|| CABS || Add suffix dropdown.
                Presenter.IsLocationServiceTenant();
                Presenter.IsDropDownSuffixType();
                AddSuffixDropdown();
                //

                BindData();
                Dictionary<String, String> args = new Dictionary<String, String>();
                if (!Request.QueryString["args"].IsNull())
                {
                    args.ToDecryptedQueryString(Request.QueryString["args"]);
                    if (args.ContainsKey("PageType"))
                    {
                        PageType = Convert.ToString(args["PageType"]);
                    }
                }
                #region UAT-2169:Send Middle Name and Email address to clearstar in Complio
                hdnNoMiddleNameText.Value = NoMiddleNameText;
                #endregion

                #region UAT-4280
                BindUserGroup();
                #endregion
            }

            //UAT-1180: WB: Combine applicant and portfolio search.
            Presenter.GetApplicantInstitutionHierarchyMapping();
            String permissionCode = Presenter.GetUserNodePermission();
            //UAT-1020
            UserNodePermissionCode = permissionCode;
            if (permissionCode.Equals(LkpPermission.ReadOnly.GetStringValue()))
            {
                DisableControls();
            }
            UserFirstName = txtFirstName.Text;
            UserLastName = txtLastName.Text;
            //UAT-1612 : As an applicant, my middle name should be required.
            ValidateMiddleName(CurrentViewContext.MiddleName);
            //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
            UserMiddleName = txtMiddleName.Text;
            uploadControl.MaxFileSize = Convert.ToInt32(WebConfigurationManager.AppSettings[AppConsts.MAXIMUM_FILE_SIZE]);
            cBarMain.SaveButton.ToolTip = "Click to save any updates made to your profile";
            cBarMain.CancelButton.ToolTip = "Click to cancel. Any updates made to your profile will not be saved";
            Presenter.OnViewLoaded();
            HideShowControlsForGranularPermission();//UAT-806

            #region UAT-968:As an ADB admin, I should be able to create/view/edit "notes" in a student's profile search details.
            //Start UAT-5052
            // if (Presenter.IsDefaultTenant)  
            // {
            //End UAT-5052
            ucApplicantNotes.IsReadOnly = true;
                divProfileNotes.Visible = true;
                ucApplicantNotes.ApplicantUserID = CurrentViewContext.OrganizationUserId;
                ucApplicantNotes.Visible = true;
                ucApplicantNotes.SelectedTenantId = TenantId;
            // } UAT-5052
            #endregion

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
                    //UAT-1020
                    if (UserNodePermissionCode == LkpPermission.ReadOnly.GetStringValue())
                        customAttribute.ControlDisplayMode = DisplayMode.ReadOnlyLabels;
                    else
                        customAttribute.ControlDisplayMode = DisplayMode.Controls;
                    customAttribute.TenantId = CurrentViewContext.TenantId;
                    customAttribute.CurrentLoggedInUserId = this.OrganizationUserId;
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
            BindSMSNotificationDetail(false);
            //For UAT-2276: Regression testing and performance optimization
            HideShowSMSPanel();

            caProfileCustomAttributes.TenantId = CurrentViewContext.TenantId;
            caProfileCustomAttributes.TypeCode = CustomAttributeUseTypeContext.Profile.GetStringValue();
            caProfileCustomAttributes.CurrentLoggedInUserId = CurrentViewContext.OrganizationUserId;
            caProfileCustomAttributes.DataSourceModeType = DataSourceMode.Ids;
            caProfileCustomAttributes.ControlDisplayMode = DisplayMode.Controls;
            caProfileCustomAttributes.ValidationGroup = "grpFormSubmit";

            if (IsControlsHideForSupportPortal)
            {
                //dvAccountInfo.Visible = false;
                //dvProfilePicture.Visible = false;
                //  divIntegrationSection.Visible = false;
                divProfileNotes.Visible = false;
                caProfileCustomAttributes.Visible = false;

                cBarMain.DisplayButtons = CommandBarButtons.Save;
                cBarMain.CancelButton.Visible = false;
                cBarMain.CancelButton.Enabled = false;
                Boolean result = Presenter.IsApplicantGraduated();
                divGraduatedStatus.Visible = true;
                lblGraduatedStatus.Text = result == true ? "Yes" : "No";
                dvMergedUserGroup.Visible = false;
                dvUsrGrp.Visible = false;

                #region UAT-3047
                divAccountSettings.Visible = true;
                fsucCmdBarPortfolio.ShowButtons(CommandBarButtons.Save);
                #endregion
                if (!this.IsPostBack)
                {
                    Presenter.IsUserTwoFactorAuthenticated();
                    if (Presenter.ShowhideTwoFactorAuthentication())
                    {
                        //UserTwoFactorAuthentication usrTwofactorAuth = 
                        divTwoFactorAuthentication.Visible = true;
                        //if (!usrTwofactorAuth.IsNullOrEmpty())
                        //{
                        //    if (!usrTwofactorAuth.UTFA_IsVerified)
                        //        spnVerified.InnerText = "(Not Verified)";
                        //    else
                        //        spnVerified.InnerText = "";

                        //    rdbTwoFactorAuth.SelectedValue = "True";
                        //    hdnIsTwoFactorAuthenticationPrevious.Value = "True";
                        //}
                        //else
                        //{
                        //    rdbTwoFactorAuth.SelectedValue = "False";
                        //    rdbTwoFactorAuth.Enabled = false;
                        //    spnVerified.InnerText = "";
                        //}
                    }
                    else
                    {
                        divTwoFactorAuthentication.Visible = false;
                    }
                }
            }
        }

        #region Button Click Events

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
                    String corruptedFileMessage = "Your profile picture is not uploaded.Please try again. ";
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ShowCallBackMessage('" + corruptedFileMessage + "');", true);
                }
                else if (Presenter.SaveProfilePhoto())
                {
                    imgCntrl.ImageUrl = String.Format("/ComplianceOperations/UserControl/DocumentViewer.aspx?UserId={0}&DocumentType={1}&_={2}", OrganizationUserId, "ProfilePicture", DateTime.Now.Millisecond.ToString());
                    ShowInitialsOrProfilePicture();
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
        /// On Click of Update button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucCmdBar1_SaveClick(object sender, EventArgs e)
        {
            SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
            lblUserNameMessage.Text = String.Empty;

            #region UAT 536- Add Custom Fields (editable) to the Applicant search
            List<ApplicantCustomAttributeContract> applicantCustomAttributeContract = new List<ApplicantCustomAttributeContract>();
            List<TypeCustomAttributes> lstCustomAttribute = new List<TypeCustomAttributes>();
            //UAT 1438: Enhancement to allow students to select a User Group. 
            List<ApplicantUserGroupMapping> lstApplicantuserGroupMapping = new List<ApplicantUserGroupMapping>();

            foreach (RepeaterItem repeated in rptrCustomAttribute.Items)
            {
                NewCustomAttributeLoader CustomAttributeLoader = repeated.FindControl("customAttribute") as NewCustomAttributeLoader;
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
            //UAT-2447
            ShowHidePhoneNumberControls();
            //Secondary Confirm Email Address check
            if (!String.IsNullOrEmpty(txtSecondaryEmail.Text.Trim()) && String.IsNullOrEmpty(txtConfirmSecEmail.Text.Trim()))
            {
                lblMessage.Visible = true;
                base.ShowErrorInfoMessage("Secondary Confirm Email Address is required.");
                return;
            }

            if (ucPersonAlias.NewFirstNameAlias.IsNullOrEmpty() && !ucPersonAlias.NewLastNameAlias.IsNullOrEmpty())
            {
                lblMessage.Visible = true;
                base.ShowErrorInfoMessage("Alias/Maiden First Name is required if Alias/Maiden Last Name is entered.");
                return;
            }

            if (!ucPersonAlias.NewFirstNameAlias.IsNullOrEmpty() && ucPersonAlias.NewLastNameAlias.IsNullOrEmpty())
            {
                lblMessage.Visible = true;
                base.ShowErrorInfoMessage("Alias/Maiden Last Name is required if Alias/Maiden First Name is entered.");
                return;
            }
            String newFirstNameAlias = ucPersonAlias.NewFirstNameAlias;
            String newLastNameAlias = ucPersonAlias.NewLastNameAlias;

            if (ucPersonAlias.HasDuplicateNames)
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
                base.ShowErrorInfoMessage("This email address is already in use. Try another?");
                return;
            }

            if (!IsValidAddress())
            {
                base.ShowErrorInfoMessage("Please select a valid ZipCode.");
                return;
            }


            //Implementation for: Based on CBI's response below we need to allow for only one character per name (first, middle, last).
            //The scenario we have to think about is the one where the applicant opts to provide no middle name with only one character for the first and last name.

            if (!txtFirstName.IsNullOrEmpty() || !txtMiddleName.IsNullOrEmpty() || !txtLastName.IsNullOrEmpty() || !txtSuffix.IsNullOrEmpty())
            {
                if (CurrentViewContext.IsLocationServiceTenant)
                {
                    Int32 suffixLength = CurrentViewContext.IsSuffixDropDownType ? cmbSuffix.SelectedItem.IsNotNull() && cmbSuffix.SelectedItem.Index > 0 ? cmbSuffix.SelectedItem.Text.Length : 0 : txtSuffix.Text.Length;
                    Int32 totalLength = (txtFirstName.Text.Length) + (txtMiddleName.Text.Length) + (txtLastName.Text.Length) + suffixLength;
                    if (totalLength < AppConsts.THREE)
                    {
                        base.ShowErrorInfoMessage("Total length for Full Name should be atleast 3 characters.");
                        return;
                    }
                }
            }



            if (Presenter.SaveUpdateSMSData())
            {
                //UAT-3068
                //if (!CurrentViewContext.IsReceiveTextNotification)
                //{
                //    String authenticationType = INTSOF.Utils.AuthenticationMode.None.GetStringValue();
                //    Presenter.SaveAuthenticationData(Convert.ToString(CurrentViewContext.OrganizationUser.UserID), authenticationType);

                //    Presenter.IsUserTwoFactorAuthenticated();
                //    if (!EventReloadTwoFactor.IsNullOrEmpty())
                //    {
                //        EventReloadTwoFactor(this);
                //    }
                //}

                if (Presenter.SaveUserData())
                {
                    //UAT 536- Add Custom Fields (editable) to the Applicant search
                    Presenter.SaveUpdateApplicantCustomAttribute(applicantCustomAttributeContract);

                    if (CurrentViewContext.IsUserGroupCustomAttributeExist || CurrentViewContext.IsUserGroupExist)
                    {
                        //UAT 1438: Enhancement to allow students to select a User Group. 
                        Presenter.SaveUpdateApplicantUserGroupCustomAttribute(lstApplicantuserGroupMapping);
                    }

                    //UAT-2440: Profile level custom attributes by tenant.
                    CurrentViewContext.ProfileCustomAttributeList = caProfileCustomAttributes.GetCustomAttributeValues();
                    Presenter.SaveUpdateProfileCustomAttributes();

                    //UAT-968:As an ADB admin, I should be able to create/view/edit "notes" in a student's profile search details.
                    //Start UAT-5052
                    // if (Presenter.IsDefaultTenant)
                    // {
                    ucApplicantNotes.SaveUpdateProfileNote();
                    //  }
                    //End UAT-5052
                    lblMessage.Visible = true;
                    base.ShowSuccessMessage("Data updated successfully.");

                    //if (NotifyProfileUpdate.IsNotNull())
                    //{
                    //    NotifyProfileUpdate(CurrentViewContext.OrganizationUser);
                    //}

                    ShowInitialsOrProfilePicture();

                    if (((SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser).OrganizationUserId.Equals(CurrentViewContext.OrganizationUserId))
                    {
                        UpdateUserInSession();
                    }
                    //UAT-1579:WB: Add SMS opt in status to portfolio details screen for ADB and Client Admins
                    BindSMSNotificationDetail();
                    //UAT-1612 : As an applicant, my middle name should be required.
                    if (chkMiddleNameRequired.Checked)
                    {
                        ValidateMiddleName(String.Empty);
                    }
                    else
                    {
                        ValidateMiddleName(txtMiddleName.Text);
                    }

                    ucPersonAlias.ResetAliasNewControls();
                }
                else
                {
                    base.ShowInfoMessage("Data is not saved.");
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
        /// On Click of cancel button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucCmdBar1_CancelClick(object sender, EventArgs e)
        {
            try
            {
                RedirectToParent();
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

        #region UAT-3047
        protected void fsucCmdBarPortfolio_SaveClick(object sender, EventArgs e)
        {
            try
            {
                #region UAT-2930
                //if (Convert.ToString(rdbTwoFactorAuth.SelectedValue) == "True")
                //{
                //    CurrentViewContext.IsUserTwoFactorAuthenticated = true;
                //    rdbTwoFactorAuth.Enabled = true;
                //}
                //else
                //{
                //    CurrentViewContext.IsUserTwoFactorAuthenticated = false;
                //    rdbTwoFactorAuth.Enabled = false;
                //    spnVerified.InnerText = String.Empty;

                //}
                //CurrentViewContext.IsUserTwoFactorAuthenticatedPrevious = Convert.ToBoolean(hdnIsTwoFactorAuthenticationPrevious.Value);
                #endregion

                if (Presenter.UpdateUser())
                {
                    //if (CurrentViewContext.IsUserTwoFactorAuthenticated == true)
                    //    hdnIsTwoFactorAuthenticationPrevious.Value = "True";
                    //else
                    //    hdnIsTwoFactorAuthenticationPrevious.Value = "False"; 

                    base.ShowSuccessMessage("User Details updated successfully.");
                }
                else
                {
                    base.ShowErrorMessage("User Details not updated successfully.");
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
        #endregion

        #region Repeater Event

        protected void rptrCustomAttribute_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                NewCustomAttributeLoader loader = e.Item.FindControl("customAttribute") as NewCustomAttributeLoader;
                if (loader.IsNotNull())
                {
                    loader.TenantId = CurrentViewContext.TenantId;
                    loader.TypeCode = CustomAttributeUseTypeContext.Hierarchy.GetStringValue();
                    loader.CurrentLoggedInUserId = CurrentViewContext.OrganizationUserId;
                    loader.DataSourceModeType = DataSourceMode.Ids;
                    //UAT-1020
                    if (UserNodePermissionCode == LkpPermission.ReadOnly.GetStringValue())
                        loader.ControlDisplayMode = DisplayMode.ReadOnlyLabels;
                    else
                        loader.ControlDisplayMode = DisplayMode.Controls;
                    loader.ValidationGroup = "grpFormSubmit";
                    //UAT 1438: Enhancement to allow students to select a User Group.
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
                    NewCustomAttributeLoader CustomAttributeLoader = repeated.FindControl("customAttribute") as NewCustomAttributeLoader;
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

        #region RadioButton Events

        protected void rblSSN_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToBoolean(rblSSN.SelectedValue.ToLower()))
                {
                    txtSSN.Text = String.Empty;
                    txtSSN.Enabled = true;
                    rfvSSN.Enabled = true;
                }
                else
                {
                    txtSSN.Text = AppConsts.DefaultSSN;
                    rfvSSN.Enabled = false;
                    txtSSN.Enabled = false;
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

        private void BindData()
        {
            BindOrganizationUserData();
            //UAT-1579:WB: Add SMS opt in status to portfolio details screen for ADB and Client Admins

            BindSMSNotificationDetail();
        }

        private void BindOrganizationUserData()
        {
            if (CurrentViewContext.OrganizationUser.IsNotNull())
            {
                #region UAT-781 ENCRYPTED SSN
                Presenter.GetDecryptedSSN(CurrentViewContext.OrganizationUser.OrganizationUserID, false);
                #endregion

                //UAT-1180: WB: Combine applicant and portfolio search.
                locationTenant.IsFalsePostBack = false;
                ucPersonAlias.IsFalsePostBack = false;
                PrevResident.IsFalsePostBack = false;

                //Set MinDate and MaxDate for DOB
                dpkrDOB.MinDate = Convert.ToDateTime("01-01-1900");
                dpkrDOB.MaxDate = DateTime.Now;
                rngvDOB.MaximumValue = DateTime.Now.Date.AddYears(-1).ToShortDateString();
                rngvDOB.MinimumValue = Convert.ToDateTime("01-01-1900").ToShortDateString();
                dpCurResidentFrom.MaxDate = DateTime.Now;

                txtUsername.Text = CurrentViewContext.OrganizationUser.aspnet_Users.UserName;
                txtOrganization.Text = CurrentViewContext.OrganizationUser.Organization.OrganizationName;
                txtFirstName.Text = UserFirstName = CurrentViewContext.OrganizationUser.FirstName;
                txtMiddleName.Text = UserMiddleName = CurrentViewContext.OrganizationUser.MiddleName;
                txtLastName.Text = UserLastName = CurrentViewContext.OrganizationUser.LastName;
                //cmbSuffix.SelectedValue = CurrentViewContext.OrganizationUser.UserTypeID.IsNullOrEmpty() ? String.Empty : CurrentViewContext.OrganizationUser.UserTypeID.ToString();

                if (!CurrentViewContext.lstSuffixes.IsNullOrEmpty() && CurrentViewContext.OrganizationUser.UserTypeID > AppConsts.NONE)
                    CurrentViewContext.OrganizationUser.Suffix = CurrentViewContext.lstSuffixes.Where(cond => cond.SuffixID == CurrentViewContext.OrganizationUser.UserTypeID).FirstOrDefault().Suffix;

                if (CurrentViewContext.IsSuffixDropDownType)
                {
                    BindSuffixDropdown();
                    if (!CurrentViewContext.OrganizationUser.Suffix.IsNullOrEmpty())
                    {
                        var item = cmbSuffix.FindItemByText(CurrentViewContext.OrganizationUser.Suffix, true);
                        if (!item.IsNullOrEmpty())
                        {
                            cmbSuffix.Items.FindItemByText(CurrentViewContext.OrganizationUser.Suffix, true).Selected = true;
                        }
                    }
                }
                else
                {
                    txtSuffix.Text = CurrentViewContext.lstSuffixes.Where(a => a.Suffix == CurrentViewContext.OrganizationUser.Suffix && !a.IsSystem).Any() ? CurrentViewContext.OrganizationUser.Suffix : String.Empty;
                }

                //txtAlias1.Text = CurrentViewContext.OrganizationUser.Alias1;
                //txtAlias2.Text = CurrentViewContext.OrganizationUser.Alias2;
                //txtAlias3.Text = CurrentViewContext.OrganizationUser.Alias3;
                //txtDOB.Text = CurrentViewContext.OrganizationUser.DOB.HasValue ? CurrentViewContext.OrganizationUser.DOB.Value.ToShortDateString() : String.Empty;

                //UAT-806:-
                if (CurrentViewContext.SSNPermissionCode.ToUpper() == EnumSystemPermissionCode.MASKED_READ_PERMISSION.GetStringValue())
                {
                    //txtSSNMasked.Text = Presenter.GetMaskedSSN(CurrentViewContext.OrganizationUser.SSN); //UAT-781
                    txtSSNMasked.Text = Presenter.GetMaskedSSN(CurrentViewContext.DecryptedSSN); //UAT-781
                    divSSNMasked.Visible = true;
                    divSSN.Visible = false;
                    dvCheckSSN.Visible = false;
                }
                else
                {
                    divSSNMasked.Visible = false;
                    divSSN.Visible = true;
                    dvCheckSSN.Visible = true;
                }
                //txtSSN.Text = CurrentViewContext.OrganizationUser.SSN; //UAT-781
                txtSSN.Text = CurrentViewContext.DecryptedSSN; //UAT-781
                ManageSSNRdBtn();
                //rblGender.SelectedValue = CurrentViewContext.OrganizationUser.Gender.Value.ToString();
                #region UAT-2447
                chkIsMaskingRequiredPrimary.Checked = CurrentViewContext.OrganizationUser.IsInternationalPhoneNumber;
                chkIsMaskingRequiredSecondary.Checked = CurrentViewContext.OrganizationUser.IsInternationalSecondaryPhone;
                if (CurrentViewContext.OrganizationUser.IsInternationalPhoneNumber)
                {
                    txtPrimaryPhoneNonMasking.Text = CurrentViewContext.OrganizationUser.PhoneNumber;
                }
                else
                {
                    txtPrimaryPhone.Text = CurrentViewContext.OrganizationUser.PhoneNumber;
                }
                if (CurrentViewContext.OrganizationUser.IsInternationalSecondaryPhone)
                {
                    txtSecondaryPhoneNonMasking.Text = CurrentViewContext.OrganizationUser.SecondaryPhone;
                }
                else
                {
                    txtSecondaryPhone.Text = CurrentViewContext.OrganizationUser.SecondaryPhone;
                }
                ShowHidePhoneNumberControls();
                #endregion
                txtPrimaryEmail.Text = CurrentViewContext.OrganizationUser.PrimaryEmailAddress;
                txtSecondaryEmail.Text = CurrentViewContext.OrganizationUser.SecondaryEmailAddress;

                //UAT-1180: WB: Combine applicant and portfolio search.
                CurrentViewContext.GenderList = Presenter.GetGenderList();
                //Presenter.GetSSNSetting();
                dpkrDOB.SelectedDate = CurrentViewContext.OrganizationUser.DOB;
                cmbGender.SelectedValue = Convert.ToString(CurrentViewContext.OrganizationUser.Gender);
                txtConfrimPrimayEmail.Text = CurrentViewContext.OrganizationUser.PrimaryEmailAddress;
                CurrentViewContext.CurrentEmailAddress = CurrentViewContext.OrganizationUser.PrimaryEmailAddress;
                CurrentViewContext.PswdRecoveryEmail = CurrentViewContext.OrganizationUser.aspnet_Users.aspnet_Membership.Email;
                txtConfirmSecEmail.Text = CurrentViewContext.OrganizationUser.SecondaryEmailAddress;

                //CurrentViewContext.GenderId = Convert.ToInt32(CurrentViewContext.OrganizationUser.Gender);
                //Presenter.GetGender();
                //txtGender.Text = CurrentViewContext.Gender;

                if (CurrentViewContext.OrganizationUser.AddressHandle != null
                    && CurrentViewContext.OrganizationUser.AddressHandle.Addresses != null)
                {
                    Entity.Address address = CurrentViewContext.OrganizationUser.AddressHandle.Addresses.Where(addHandle => addHandle.AddressHandleID == CurrentViewContext.OrganizationUser.AddressHandleID).FirstOrDefault();

                    if (address.IsNotNull())
                    {
                        //UAT-1180: WB: Combine applicant and portfolio search.
                        //CurrentViewContext.ZipCodeId = Convert.ToInt32(address.ZipCodeID);
                        locationTenant.MasterZipcodeID = address.ZipCodeID;
                        txtAddress1.Text = address.Address1;
                        txtAddress2.Text = address.Address2;
                        if (address.ZipCodeID == 0 && address.AddressExts.IsNotNull() && address.AddressExts.Count > 0)
                        {
                            Entity.AddressExt addressExt = address.AddressExts.FirstOrDefault();
                            locationTenant.RSLCountryId = addressExt.AE_CountryID;
                            locationTenant.RSLStateName = addressExt.AE_StateName;
                            locationTenant.RSLCityName = addressExt.AE_CityName;
                            locationTenant.RSLZipCode = addressExt.AE_ZipCode;
                        }

                        //Presenter.GetApplicantAddressData();
                        //txtZipCode.Text = CurrentViewContext.ApplicantZipCodeDetails.ZipCode1;
                        //txtCity.Text = CurrentViewContext.ApplicantZipCodeDetails.City.CityName;
                        //txtState.Text = CurrentViewContext.ApplicantZipCodeDetails.County.State.StateName;
                        //txtCountry.Text = INTSOF.Utils.Consts.SysXSecurityConst.SYSX_DEFAULT_COUNTRY_NAME;
                    }
                }
                if (CurrentViewContext.OrganizationUser.PersonAlias.IsNotNull() && CurrentViewContext.OrganizationUser.PersonAlias.Where(x => !x.PA_IsDeleted).Any())
                {
                    //dvPersonalAlias.Visible = true;
                    CurrentViewContext.PersonAliasList = CurrentViewContext.OrganizationUser.PersonAlias.Where(x => x.PA_IsDeleted == false).Select(cond => new PersonAliasContract
                    {
                        FirstName = cond.PA_FirstName,
                        LastName = cond.PA_LastName,
                        //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                        MiddleName = cond.PA_MiddleName,
                        Suffix = !CurrentViewContext.IsLocationServiceTenant.IsNullOrEmpty() && CurrentViewContext.IsLocationServiceTenant ? (!cond.PersonAliasExtensions.IsNullOrEmpty() && !cond.PersonAliasExtensions.FirstOrDefault(con => !con.PAE_IsDeleted).IsNullOrEmpty() && !cond.PersonAliasExtensions.FirstOrDefault(con => !con.PAE_IsDeleted).PAE_Suffix.IsNullOrEmpty() ? cond.PersonAliasExtensions.FirstOrDefault(con => !con.PAE_IsDeleted).PAE_Suffix : String.Empty) : String.Empty,
                        /*  SuffixID = !CurrentViewContext.IsLocationServiceTenant.IsNullOrEmpty() && CurrentViewContext.IsLocationServiceTenant ? (!cond.PersonAliasExtensions.IsNullOrEmpty() && !cond.PersonAliasExtensions.FirstOrDefault(con => !con.PAE_IsDeleted).IsNullOrEmpty() && !cond.PersonAliasExtensions.FirstOrDefault(con => !con.PAE_IsDeleted).PAE_Suffix.IsNullOrEmpty() ? CurrentViewContext.lstSuffixes.Where(con => con.Suffix == cond.PersonAliasExtensions.FirstOrDefault().PAE_Suffix).FirstOrDefault().SuffixID : (Int32?)null) : (Int32?)null,*/
                        ID = cond.PA_ID
                    }).ToList();
                }
                var resHisoryAddress = CurrentViewContext.OrganizationUser.ResidentialHistories.FirstOrDefault(obj => obj.RHI_IsDeleted == false && obj.RHI_IsCurrentAddress == true);
                if (resHisoryAddress.IsNotNull())
                {
                    dpCurResidentFrom.SelectedDate = resHisoryAddress.RHI_ResidenceStartDate;
                }
                CurrentViewContext.ResidentialHistoryList = Presenter.GetResidentialHistories(CurrentViewContext.OrganizationUser.OrganizationUserID);

                if (CurrentViewContext.OrganizationUser.PhotoName.IsNotNull())
                    imgCntrl.ImageUrl = String.Format("/ComplianceOperations/UserControl/DocumentViewer.aspx?UserId={0}&DocumentType={1}", OrganizationUserId, "ProfilePicture");
                else
                    imgCntrl.AlternateText = "Profile Picture is not available.";

                //To show Initials or profile picture
                ShowInitialsOrProfilePicture();

                //Start UAT-5052
                //#region UAT-968:As an ADB admin, I should be able to create/view/edit "notes" in a student's profile search details.
                //if (Presenter.IsDefaultTenant)
                //{
                //    ucApplicantNotes.IsReadOnly = true;
                //    divProfileNotes.Visible = true;
                //    ucApplicantNotes.ApplicantUserID = CurrentViewContext.OrganizationUserId;
                //    ucApplicantNotes.Visible = true;
                //    ucApplicantNotes.SelectedTenantId = TenantId;
                //}
                //#endregion
                //End UAT-5052

                #region UAT-3047
                CurrentViewContext.IsActive = CurrentViewContext.OrganizationUser.IsActive;
                CurrentViewContext.IsLocked = CurrentViewContext.OrganizationUser.aspnet_Users.aspnet_Membership.IsLockedOut;
                #endregion
            }

        }

        //public bool CheckDefaultTenant()
        //{
        //    if (ViewState["OrganizationUser"] != null) { 

        //    }
        //}

        /// <summary>
        /// To show Initials or profile picture
        /// </summary>
        private void ShowInitialsOrProfilePicture()
        {
            //Setting Name initials
            if (!String.IsNullOrWhiteSpace(CurrentViewContext.OrganizationUser.FirstName))
            {
                lblNameInitials.Text = CurrentViewContext.OrganizationUser.FirstName.Substring(0, 1).HtmlEncode();
            }
            if (!String.IsNullOrWhiteSpace(CurrentViewContext.OrganizationUser.LastName))
            {
                lblNameInitials.Text = lblNameInitials.Text + CurrentViewContext.OrganizationUser.LastName.Substring(0, 1).HtmlEncode();
            }

            if (CurrentViewContext.OrganizationUser.PhotoName.IsNotNull())
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

        #region UAT-806 Creation of granular permissions for Client Admin users

        /// <summary>
        /// Hide Show grid and page controls
        /// </summary>
        private void HideShowControlsForGranularPermission()
        {
            if (CurrentViewContext.IsDOBDisable)
            {
                divDOB.Visible = false;
            }
            if (CurrentViewContext.SSNPermissionCode.ToUpper() == EnumSystemPermissionCode.NO_ACCESS_PERMISSION.GetStringValue().ToUpper())
            {
                divSSN.Visible = false;
                dvCheckSSN.Visible = false;
            }
        }
        #endregion

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

            //UAT-1020: WB: Bugs relating to limited admin access to applicant search details
            chkMiddleNameRequired.Enabled = false;
            txtFirstName.ReadOnly = true;
            txtMiddleName.ReadOnly = true;
            txtLastName.ReadOnly = true;
            cmbGender.Enabled = false;
            dpkrDOB.Enabled = false;
            txtSSN.ReadOnly = true;
            //chkAutoFillSSN.Enabled = false;

            txtSSNMasked.ReadOnly = true;
            txtPrimaryEmail.ReadOnly = true;
            txtConfrimPrimayEmail.ReadOnly = true;
            chkChangeEmail.Enabled = false;
            txtSecondaryEmail.ReadOnly = true;
            txtConfirmSecEmail.ReadOnly = true;
            txtPrimaryPhone.ReadOnly = true;
            //UAT-2447
            txtPrimaryPhoneNonMasking.ReadOnly = true;
            txtSecondaryPhone.ReadOnly = true;
            //UAT-2447
            txtSecondaryPhoneNonMasking.ReadOnly = true;
            txtAddress1.ReadOnly = true;
            txtAddress2.ReadOnly = true;
            dpCurResidentFrom.Enabled = false;
            rdbTextNotification.Enabled = false;
            txtPhoneNumber.ReadOnly = true;
            customAttribute.IsReadOnly = true;

            locationTenant.Enable = false;
            locationTenant.DisableAllControls();
        }

        #region Check and save profile pic

        /// <summary>
        /// Method for uploading and saving of profile picture
        /// </summary>
        private void CheckAndSaveProfilePic()
        {
            try
            {
                if (uploadControl.UploadedFiles.Count > 0)
                {
                    Entity.OrganizationUser organizationUser = SecurityManager.GetOrganizationUser(OrganizationUserId);
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
                        String fileName = "UP_" + CurrentViewContext.TenantId.ToString() + "_" + OrganizationUserId.ToString() + "_" + date + Path.GetExtension(uploadControl.UploadedFiles[0].FileName);

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
                        tempFilePath += "Tenant(" + CurrentViewContext.TenantId.ToString() + @")\" + @"Pics\";
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
                            FilePath += "Tenant(" + CurrentViewContext.TenantId.ToString() + @")\";
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
                            String destFolder = FilePath + "Tenant(" + CurrentViewContext.TenantId.ToString() + @")/" + @"Pics/";
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
        /// Method to update the LastName and FirstName in session service (SysXMembershipUser) to update the userName link on Update the profile.
        /// </summary>
        private void UpdateUserInSession()
        {
            String loginUserName = CurrentViewContext.UserName;
            SysXMembershipUser user = System.Web.Security.Membership.GetUser(loginUserName) as SysXMembershipUser;
            SysXWebSiteUtils.SessionService.SetSysXMembershipUser(user);
            String userName = ((String.IsNullOrEmpty(user.LastName)) ? "" : user.LastName + ", ") + user.FirstName;
            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), "setUserName", "parent.SetUserLink('" + userName + "');", true);
        }

        private void RedirectToParent()
        {
            String childcontrolPath = String.Empty;
            if (!PageType.IsNullOrEmpty() && PageType == WorkQueueType.SupportPortalDetail.ToString())
            {
                childcontrolPath = ChildControls.SupportPortalDetails;
                Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "Child", childcontrolPath},
                                                                    {"OrganizationUserId", CurrentViewContext.OrganizationUserId.ToString()},
                                                                    {"TenantId", CurrentViewContext.TenantId.ToString()}
                                                                 };

                string url = String.Format("~/SearchUI/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                Response.Redirect(url, true);
            }
            else
            {
                if (!PageType.IsNullOrEmpty() && PageType == WorkQueueType.ComprehensiveSearch.ToString())
                {
                    childcontrolPath = ChildControls.ApplicantComprehensiveSearchPage;
                }

                else
                {
                    childcontrolPath = ChildControls.ApplicantPortFolioSearchPage;
                }
                Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "Child", childcontrolPath},
                                                                    {"CancelClicked", "CancelClicked"}
                                                                 };

                string url = String.Format("~/SearchUI/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                Response.Redirect(url, true);
            }
        }

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
                    applicantUserGroupMapping.AUGM_OrganizationUserID = OrganizationUserId;
                    applicantUserGroupMapping.AUGM_IsDeleted = false;
                    applicantUserGroupMapping.AUGM_CreatedByID = CurrentLoggedInUserId;
                    applicantUserGroupMapping.AUGM_CreatedOn = DateTime.Now;

                    lstApplicantuserGroupMapping.Add(applicantUserGroupMapping);
                }
            }
        }

        #region UAT-1579
        private void BindSMSNotificationDetail(Boolean isRefreshLoading = true)
        {
            //if (isRefreshLoading)
            //{
            //[13/10/2016]:- Commented below code related to optimization of SMS notification functionality(This functionality shifted to SearchUI/Default.aspx page.)

            //Presenter.UpdateSubscriptionStatusFromAmazon();
            //Presenter.GetUserSMSNotificationData();
            //if (!CurrentViewContext.ApplicantSMSSubscriptionData.IsNullOrEmpty() && CurrentViewContext.ApplicantSMSSubscriptionData.ASSB_ID > AppConsts.NONE
            //    && CurrentViewContext.ApplicantSMSSubscriptionData.ApplicantSMSSubsciptionDetails.IsNotNull()
            //    && CurrentViewContext.ApplicantSMSSubscriptionData.ApplicantSMSSubsciptionDetails.Any(cnd => !cnd.ASSD_IsDeleted))
            //{
            //    CurrentViewContext.IsReceiveTextNotification = true;
            //    CurrentViewContext.PhoneNumber = CurrentViewContext.ApplicantSMSSubscriptionData.ApplicantSMSSubsciptionDetails.FirstOrDefault(cnd => !cnd.ASSD_IsDeleted).ASSD_MobileNo;
            //    divHideShowPhoneNumber.Style["display"] = "block";
            //    divConfirmationStatus.Style["display"] = "block";
            //    hdnIsConfirmMsgVisible.Value = "1";
            //    rfvPhoneNumber.Enabled = true;
            //    if (!CurrentViewContext.ApplicantSMSSubscriptionData.ApplicantSMSSubsciptionDetails.FirstOrDefault(cnd => !cnd.ASSD_IsDeleted).ASSD_IsSubscriptionConfirm)
            //    {
            //        lblConfirmationStatus.Text = SMSSubscriptionStatus.NOT_CONFIRMED.GetStringValue();
            //        //lnkReSendSubMessage.Visible = true;
            //    }
            //    else
            //    {
            //        lblConfirmationStatus.Text = SMSSubscriptionStatus.CONFIRMED.GetStringValue();
            //        //lnkReSendSubMessage.Visible = false;
            //    }
            //}
            //else
            //{
            //    CurrentViewContext.IsReceiveTextNotification = false;
            //    divHideShowPhoneNumber.Style["display"] = "none";
            //    divConfirmationStatus.Style["display"] = "none";
            //    hdnIsConfirmMsgVisible.Value = "0";
            //    //lnkReSendSubMessage.Visible = false;
            //    rfvPhoneNumber.Enabled = false;
            //}
            // }
            // else
            if (!isRefreshLoading)
            {
                bool isPhoneNumberRequired = CurrentViewContext.IsReceiveTextNotification;

                if (rdbSpecifyAuthentication.SelectedValue == "AAAB")
                {
                    isPhoneNumberRequired = true;
                }

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
        #endregion

        /// <summary>
        /// UAT-1612 : As an applicant, my middle name should be required.
        /// </summary>
        /// <param name="middleName"></param>
        private void ValidateMiddleName(String middleName)
        {
            if (middleName.IsNullOrEmpty())
            {
                chkMiddleNameRequired.Checked = true;
                txtMiddleName.Text = NoMiddleNameText;
                txtMiddleName.Enabled = false;
                rfvMiddleName.Enabled = false;
                spnMiddleName.Style["display"] = "none";

                if (CurrentViewContext.IsLocationServiceTenant)
                {
                    txtMiddleName.Attributes.Add("PlaceHolder", "");
                    txtMiddleName.ToolTip = "";
                }
            }
            else
            {
                chkMiddleNameRequired.Checked = false;
                txtMiddleName.Text = middleName;
                txtMiddleName.Enabled = true;
                rfvMiddleName.Enabled = true;
                spnMiddleName.Style["display"] = "";
                spnMiddleName.Visible = true;
            }
        }

        private void HideShowSMSPanel()
        {
            if (!hdnIsCollapsed.Value.IsNullOrEmpty() && String.Compare(hdnIsCollapsed.Value, "false", true) == AppConsts.NONE)
            {
                divSMSNotification.Attributes["class"] = "section";
                divNotificationHeader.Attributes["class"] = "mhdr";
                divContentNotification.Style["display"] = "block";
            }
            else
            {
                divSMSNotification.Attributes["class"] = "section collapsed";
                divNotificationHeader.Attributes["class"] = "mhdr colps";
                divContentNotification.Style["display"] = "none";
            }
        }

        private void ManageSSNRdBtn()
        {
            String AppSSN = txtSSN.Text.Trim();
            if (AppSSN == AppConsts.DefaultSSN)
            {
                rblSSN.SelectedValue = "false";
                rfvSSN.Enabled = false;
                txtSSN.Enabled = false;
            }
            else
            {
                rblSSN.SelectedValue = "true";
                txtSSN.Enabled = true;
                rfvSSN.Enabled = true;
            }
        }

        #region CBI || CABS

        private void AddSuffixDropdown()
        {
            if (CurrentViewContext.IsLocationServiceTenant)
            {
                //dvSpnFirstName.Style.Add("Width", "10%");
                //dvSpnMiddleName.Style.Add("Width", "10%");
                //dvSpnLastName.Style.Add("Width", "10%");

                //dvFirstName.Style.Add("Width", "18%");
                //dvMiddleName.Style.Add("Width", "18%");
                //dvLastName.Style.Add("Width", "18%");

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

            cmbSuffix.DataSource = CurrentViewContext.lstSuffixes.Where(a => a.IsSystem);
            cmbSuffix.DataBind();
            cmbSuffix.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(Resources.Language.SELECTSUFFIX, "0"));
        }

        #endregion

        #endregion

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
            }
            else
            {
                dvUnmasking.Style["display"] = "none";
                dvMasking.Style["display"] = "block";
                revTxtMobile.Enabled = true;
                rfvTxtMobile.Enabled = true;
                rfvTxtMobilePrmyNonMasking.Enabled = false;
                revTxtMobilePrmyNonMasking.Enabled = false;

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

        #region 4280
        private void BindUserGroup()
        {
            try
            {
                Presenter.GetUserGroupsForUser();

                String usrGrpCode = CustomAttributeDatatype.User_Group.GetStringValue().ToLower();
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

