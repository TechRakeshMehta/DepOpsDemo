#region NAMESPACES
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity;
using Entity.ClientEntity;
using INTSOF.UI.Contract.SysXSecurityModel;
using INTSOF.Utils;
using Telerik.Web.UI;
#endregion

namespace CoreWeb.Search.Views
{
    public partial class ClientProfile : BaseUserControl, IClientProfile
    {
        #region VARIABLES
        private ClientProfilePresenter _presenter = new ClientProfilePresenter();
        private String _viewType;
        private Int32 organizationUserID;
        #endregion

        #region PROPERTIES

        #region PRIVATE PROPERTIES
        #endregion

        #region PUBLIC PROPERTIES

        String IClientProfile.MasterNodeLabel
        {
            get { return (String)(ViewState["MasterNodeLabel"]); }
            set { ViewState["MasterNodeLabel"] = value; }
        }

        /// <summary>
        /// Property to initialize Presenter
        /// </summary>
        public ClientProfilePresenter Presenter
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

        /// <summary>
        /// Property to get/set Tenant ID of selected client user
        /// </summary>
        public Int32 ClientTenantID
        {
            get
            {
                Dictionary<String, String> args = new Dictionary<String, String>();
                if (Request.QueryString["args"].IsNotNull())
                {
                    args.ToDecryptedQueryString(Request.QueryString["args"]);
                    if (args.ContainsKey("TenantID"))
                    {
                        return Convert.ToInt32(args.GetValue("TenantID"));
                    }
                }
                return 0;
            }
            set { }
        }

        /// <summary>
        /// Property to get Client User OrganizationUserID from QueryString
        /// </summary>
        public Int32 OrganizationUserID
        {
            get
            {
                if (!IsFromConfigurationPage)
                {
                    Dictionary<String, String> args = new Dictionary<String, String>();
                    if (Request.QueryString["args"].IsNotNull())
                    {
                        args.ToDecryptedQueryString(Request.QueryString["args"]);
                        if (args.ContainsKey("OrganizationUserId"))
                        {
                            return Convert.ToInt32(args.GetValue("OrganizationUserId"));
                        }
                    }
                    return base.CurrentUserId;
                }
                else
                {
                    return organizationUserID;
                }

            }
            set
            {
                organizationUserID = value;
            }
        }


        /// <summary>
        /// UAT-2266
        /// Property to get all assigned user role names.
        /// </summary>
        String IClientProfile.AssignedRoles
        {
            get;
            set;
        }

        /// <summary>
        /// Property to get Client User OrganizationUserID from QueryString
        /// </summary>
        public Int32 DeptProgramMappingID
        {
            get { return (Int32)(ViewState["DeptProgramMappingID"]); }
            set { ViewState["DeptProgramMappingID"] = value; }
        }

        public Boolean IsFromConfigurationPage
        {
            get;
            set;
        }

        /// <summary>
        /// Property to get/set CLient user FirstName
        /// </summary>
        public String ClientFirstName
        {
            get
            {
                return txtFirstName.Text.Trim();
            }
            set
            {
                txtFirstName.Text = value;
            }
        }

        /// <summary>
        /// Property to get/set CLient user Lastname
        /// </summary>
        public String ClientLastName
        {
            get
            {
                return txtLastName.Text.Trim();
            }
            set
            {
                txtLastName.Text = value;
            }
        }

        /// <summary>
        /// Property to get/set CLient user Phone Number
        /// </summary>
        public String PhoneNumber
        {
            get
            {
                return txtPhone1.Text.Trim();
            }
            set
            {
                txtPhone1.Text = value;
            }

        }
        //UAT-2447
        public String PhoneNumberUnMasked
        {
            get
            {
                return txtPhoneUnMasking.Text.Trim();
            }
            set
            {
                txtPhoneUnMasking.Text = value;
            }

        }

        /// <summary>
        /// Property to get/set CLient user Email Address
        /// </summary>
        public String EmailAddress
        {
            get
            {
                return txtEmail.Text.Trim();
            }
            set
            {
                txtEmail.Text = value;
            }
        }

        /// <summary>
        /// Property to get/set CLient username. 
        /// UAT 1008 Add Username to Client User Search Details screen
        /// </summary>
        public String UserName
        {
            get
            {
                return txtUserName.Text.Trim();
            }
            set
            {
                txtUserName.Text = value;
            }
        }

        /// <summary>
        /// Property to get/set CLient Lock User. 
        /// UAT 983 As an ADB admin, I should be able to unlock a client admin's account from the Client User Search details
        /// </summary>
        public String LockUser
        {
            get
            {
                return btnUnlockUser.CommandName;
            }
            set
            {
                btnUnlockUser.CommandName = value;
            }
        }

        public string LockUserStatus
        {
            get
            {
                return btnUnlockUser.Text;
            }
            set
            {
                btnUnlockUser.Text = value;
            }
        }

        public string LockUserCommandName
        {
            get
            {
                return btnUnlockUser.CommandName;
            }
            set
            {
                btnUnlockUser.CommandName = value;
            }
        }

        public string LockUnlockUserTooltip
        {
            get
            {
                return btnUnlockUser.ToolTip;
            }
            set
            {
                btnUnlockUser.ToolTip = value;
            }
        }

        public Int32 CurrentLoggedInUserId
        {
            get { return base.CurrentUserId; }
        }

        /// <summary>
        /// Property to get/setContext for current View
        /// </summary>
        public IClientProfile CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// Property to get/set List of tree data 
        /// </summary>
        public List<GetDepartmentTree> lstTreeData { set; get; }

        public List<InstituteHierarchyNodesList> lstTreeHierarchyData { set; get; } //UAT-3369

        public List<GetDepartmentTree> LstBackgroundTreeData { set; get; }

        /// <summary>
        /// Property to get/set OrganizationUserData
        /// </summary>
        public Entity.OrganizationUser OrganizationUserData
        {
            get;
            set;
        }

        public List<FeatureActionContract> FeaturePermissionData { set; get; }

        List<Entity.SystemEntityUserPermissionData> IClientProfile.SystemEntityUserPermissionData { set; get; }

        List<Entity.ClientEntity.TypeCustomAttributes> IClientProfile.ProfileCustomAttributeList { get; set; }
        #endregion

        #endregion

        #region EVENTS

        #region PAGE EVENTS

        /// <summary>
        /// This event fired on page initialization
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            try
            {
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                base.OnInit(e);
                base.SetPageTitle("Client Detail");
                base.Title = "Client Detail";
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
        /// This event fired on page load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];

            if (!IsPostBack)
            {
                //UAT-3326: Creation of internal notes function (like on portfolio details) for client admins on the Client User Details screen.
                if (ViewState["OrganizationUserData"] != null)
                {
                    CurrentViewContext.OrganizationUserData = (Entity.OrganizationUser)ViewState["OrganizationUserData"];
                    ucApplicantNotes.OrganizationUser = (Entity.OrganizationUser)ViewState["OrganizationUserData"];
                }
                else
                {
                    Presenter.GetOrganizationUserData();
                    ucApplicantNotes.OrganizationUser = CurrentViewContext.OrganizationUserData;
                }

                Presenter.OnViewInitialized();
                Presenter.GetOrganizationUserData();
                Presenter.GetAssignedUserRoleNames();
                BindClientUserDetails();
                SetButtonText();


                #region UAT:2411
                Dictionary<String, String> args = new Dictionary<String, String>();

                if (!Request.QueryString["args"].IsNull())
                {
                    args.ToDecryptedQueryString(Request.QueryString["args"]);

                    if (args.ContainsKey("NodeLabel"))
                    {
                        CurrentViewContext.MasterNodeLabel = Convert.ToString(args["NodeLabel"]);
                    }

                    if (args.ContainsKey("AgencyUserID") && (Convert.ToInt32(args["AgencyUserID"]) == 0))
                    {
                        //UAT-2930
                        if (Presenter.ShowhideTwoFactorAuthentication())
                        {
                            divTwoFactorAuth.Visible = true;
                            SetTwoFactorAuthButtonText();//UAT-2930
                        }
                        else
                        {
                            divTwoFactorAuth.Visible = false;
                        }
                    }

                }
                #endregion

            }

            #region [UAT-3326]
            ucApplicantNotes.IsAdminNotes = true;
            #endregion

            #region UAT-3326: Creation of internal notes function (like on portfolio details) for client admins on the Client User Details screen
            if (!CurrentViewContext.ClientTenantID.IsNullOrEmpty()) //!CurrentViewContext.ClientTenantID.IsNullOrEmpty()
            {
                ucApplicantNotes.IsReadOnly = true;
                divProfileNotes.Visible = true;
                ucApplicantNotes.ApplicantUserID = CurrentViewContext.OrganizationUserID;
                ucApplicantNotes.Visible = true;
                ucApplicantNotes.SelectedTenantId = CurrentViewContext.ClientTenantID;
            }

            #endregion

            Presenter.OnViewLoaded();

            caProfileCustomAttributes.TenantId = CurrentViewContext.ClientTenantID;
            hdnTenantId.Value = Convert.ToString(CurrentViewContext.ClientTenantID);
            caProfileCustomAttributes.TypeCode = CustomAttributeUseTypeContext.Profile.GetStringValue();
            caProfileCustomAttributes.CurrentLoggedInUserId = CurrentViewContext.OrganizationUserID;
            caProfileCustomAttributes.DataSourceModeType = DataSourceMode.Ids;
            caProfileCustomAttributes.ControlDisplayMode = DisplayMode.Controls;
            caProfileCustomAttributes.ValidationGroup = "grpFormSubmit";
        }

        //UAT-3430
        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (caProfileCustomAttributes.IsNeedToHideCommandBar)
            {
                cmb_EditProfile.Visible = false;
            }
            else
            {
                cmb_EditProfile.Visible = true;
            }
        }


        #endregion

        #region TREE EVENTS

        /// <summary>
        /// Institution Hierarchy tree Need Data source event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void treelstHiearachyDetails_NeedDataSource(object sender, Telerik.Web.UI.TreeListNeedDataSourceEventArgs e)
        {
            try
            {
                treelstHiearachyDetails.DataSource = CurrentViewContext.lstTreeHierarchyData;
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
        /// Institution Hieararchy tree Item created event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void treelstHiearachyDetails_ItemCreated(object sender, Telerik.Web.UI.TreeListItemCreatedEventArgs e)
        {
            try
            {
                // Below code disables the expand button.
                if (e.Item is TreeListDataItem)
                {
                    Control expandButton = e.Item.FindControl("ExpandCollapseButton");
                    if (expandButton != null)
                    {
                        ((Button)expandButton).Enabled = false;
                        ((Button)expandButton).Visible = false;
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
        /// Institution Hierarchy tree Pre-Render event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void treelstHiearachyDetails_PreRender(object sender, EventArgs e)
        {
            try
            {
                treelstHiearachyDetails.ExpandAllItems();
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

        #region Grid Events

        /// <summary>
        /// Notification Setting grid need datasource event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdNotificationSetup_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            List<Entity.CommunicationCCUser> lstNotificationSettingData = Presenter.GetNotificationSettingData();
            if (!lstNotificationSettingData.IsNullOrEmpty())
            {
                grdNotificationSetup.DataSource = lstNotificationSettingData;
            }
            else
            {
                grdNotificationSetup.DataSource = new List<Entity.CommunicationCCUser>();
            }
        }

        protected void grdEntityPermissions_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetSystemEntityUserPermissionData();
                if (!CurrentViewContext.SystemEntityUserPermissionData.IsNullOrEmpty())
                {
                    grdEntityPermissions.DataSource = CurrentViewContext.SystemEntityUserPermissionData;
                }
                else
                {
                    grdEntityPermissions.DataSource = new List<Entity.SystemEntityUserPermissionData>();
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

        #region COMMANDBAR EVENTS

        /// <summary>
        /// Reset Password Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdbar_ClientProfile_ExtraClick(object sender, EventArgs e)
        {
            try
            {
                if (CurrentViewContext.OrganizationUserID.IsNotNull())
                {
                    String password = radCpatchaPassword.CaptchaImage.Text;
                    if (Presenter.ResetPassword(password))
                    {
                        Presenter.SendPasswordResetMail(password);
                        Boolean mailStatus = true;
                        if (mailStatus)
                        {
                            base.ShowSuccessMessage(SysXUtils.GetMessage(ResourceConst.SECURITY_PASSWORD_SEND_SUCEESFULLY));
                        }
                        else
                        {
                            base.ShowInfoMessage(SysXUtils.GetMessage(ResourceConst.SECURITY_FAILED_TO_SEND_EMAIL));
                        }
                    }
                }
                else
                {
                    base.ShowErrorMessage("Password did not reset succesfully.");
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

        protected void cmb_EditProfile_SaveClick(object sender, EventArgs e)
        {
            CurrentViewContext.ProfileCustomAttributeList = caProfileCustomAttributes.GetCustomAttributeValues();
            if (Presenter.SaveUpdateProfileCustomAttributes())
            {
                base.ShowSuccessMessage("Profile Information updated successfully.");
            }
            else
            {
                base.ShowErrorMessage("Profile Information Updation Failed.");
            }

            //UAT-3326
            if (!CurrentViewContext.ClientTenantID.IsNullOrEmpty())
                ucApplicantNotes.SaveUpdateProfileNote();
        }

        protected void cmb_EditProfile_ClearClick(object sender, EventArgs e)
        {

        }
        #endregion

        #region METHODS

        #region PRIVATE METHODS

        /// <summary>
        /// Method to Bind Client user details
        /// </summary>
        private void BindClientUserDetails()
        {
            if (IsFromConfigurationPage)
            {
                divClientUserDetails.Visible = false;
                divClientUserDetails1.Visible = false;
                divBackToConfiguration.Visible = true;
            }
            BindPersonalInfo();
            BindHierarchyTreeData();
            BindFeaturePermissionTreeData();
            #region UAT-3326: Creation of internal notes function (like on portfolio details) for client admins on the Client User Details screen
            if (!CurrentViewContext.ClientTenantID.IsNullOrEmpty())
            {
                ucApplicantNotes.IsReadOnly = true;
                divProfileNotes.Visible = true;
                ucApplicantNotes.ApplicantUserID = CurrentViewContext.OrganizationUserID;
                ucApplicantNotes.Visible = true;
                ucApplicantNotes.SelectedTenantId = CurrentViewContext.ClientTenantID;
            }
            #endregion
        }

        /// <summary>
        /// Method to Bind Hierarchy Tree Data for the Selected ClientUser
        /// </summary>
        private void BindHierarchyTreeData()
        {
            Presenter.GetHierarchyTreeData();
        }

        /// <summary>
        /// Method to bind Client's Personal Information
        /// </summary>
        private void BindPersonalInfo()
        {
            ClientFirstName = CurrentViewContext.OrganizationUserData.FirstName.IsNullOrEmpty() ? String.Empty : CurrentViewContext.OrganizationUserData.FirstName;
            ClientLastName = CurrentViewContext.OrganizationUserData.LastName.IsNullOrEmpty() ? String.Empty : CurrentViewContext.OrganizationUserData.LastName;
            //UAT-2447
            if (CurrentViewContext.OrganizationUserData.IsInternationalPhoneNumber)
            {
                PhoneNumberUnMasked = CurrentViewContext.OrganizationUserData.aspnet_Users.MobileAlias.IsNullOrEmpty() ? String.Empty : CurrentViewContext.OrganizationUserData.aspnet_Users.MobileAlias;
                txtPhone1.Visible = false;
            }
            else
            {
                PhoneNumber = CurrentViewContext.OrganizationUserData.aspnet_Users.MobileAlias.IsNullOrEmpty() ? String.Empty : CurrentViewContext.OrganizationUserData.aspnet_Users.MobileAlias;
                txtPhoneUnMasking.Visible = false;
            }
            EmailAddress = CurrentViewContext.OrganizationUserData.aspnet_Users.aspnet_Membership.Email.IsNullOrEmpty() ? String.Empty : CurrentViewContext.OrganizationUserData.aspnet_Users.aspnet_Membership.Email;
            //UAT 1008 Add Username to Client User Search Details screen
            UserName = CurrentViewContext.OrganizationUserData.aspnet_Users.UserName.IsNullOrEmpty() ? String.Empty : CurrentViewContext.OrganizationUserData.aspnet_Users.UserName;
            //UAT 983 As an ADB admin, I should be able to unlock a client admin's account from the Client User Search details.
            btnUnlockUser.Text = CurrentViewContext.OrganizationUserData.aspnet_Users.aspnet_Membership.IsLockedOut ? AppConsts.LOCKED : AppConsts.UNLOCKED;
            LockUser = CurrentViewContext.OrganizationUserData.aspnet_Users.aspnet_Membership.IsLockedOut ? AppConsts.CMD_UNLOCK : AppConsts.CMD_LOCK;
            LockUnlockUserTooltip = CurrentViewContext.OrganizationUserData.aspnet_Users.aspnet_Membership.IsLockedOut ? AppConsts.UNLOCK_TOOLTIP : AppConsts.LOCK_TOOLTIP;
            txtAssignedRoles.Text = CurrentViewContext.AssignedRoles;

            //UAT-3326: Creation of internal notes function (like on portfolio details) for client admins on the Client User Details screen
            if (!CurrentViewContext.ClientTenantID.IsNullOrEmpty())
                ucApplicantNotes.SaveUpdateProfileNote();
        }


        /// <summary>
        /// Method to Bind Feature Permission Data for the Selected ClientUser
        /// </summary>
        private void BindFeaturePermissionTreeData()
        {
            Presenter.GetFeaturePermissionTreeData();
        }

        /// <summary>
        /// Set the button text for 'btnNotifications', 
        /// based on the settingsin OrganizationUser table in Security
        /// </summary>
        private void SetButtonText()
        {
            var _isMsgEnabled = Convert.ToBoolean(CurrentViewContext.OrganizationUserData.IsInternalMsgEnabled);
            btnNotifications.Text = _isMsgEnabled ? "Yes" : "No";
            btnNotifications.ToolTip = _isMsgEnabled ? "Click to disallow" : "Click to allow";
            btnNotifications.CommandArgument = CurrentViewContext.OrganizationUserID.ToString();
            hdfCrntUsrId.Value = Convert.ToString(this.CurrentUserId);
        }

        //UAT-2930
        private void SetTwoFactorAuthButtonText()
        {
            UserTwoFactorAuthentication userTwoFactorAuth = Presenter.SetTwoFactorAuthButtonText(Convert.ToString(CurrentViewContext.OrganizationUserData.UserID));
            if (!userTwoFactorAuth.IsNullOrEmpty())
            {
                btnTwoFactorAuthentication.Text = "Enabled";
                btnTwoFactorAuthentication.Enabled = true;
                btnTwoFactorAuthentication.ToolTip = "Click to disable";
            }
            else
            {
                btnTwoFactorAuthentication.Text = "Disabled";
                btnTwoFactorAuthentication.Enabled = false;
                btnTwoFactorAuthentication.ToolTip = "Disabled";
            }
            hdnUserId.Value = Convert.ToString(CurrentViewContext.OrganizationUserData.UserID);
            hdnCurrentUserId.Value = Convert.ToString(CurrentViewContext.OrganizationUserID);
        }

        #endregion

        /// <summary>
        /// Back to search Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkGoBack_Click(object sender, EventArgs e)
        {
            Dictionary<String, String> queryString = new Dictionary<String, String>
            {
                    {"Child",ChildControls.ClientUserSearchPage},
                    {"CancelClicked", "CancelClicked"}                    
            };
            string url = String.Format("~/SearchUI/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
            Response.Redirect(url, true);

        }

        #endregion


        #endregion

        protected void treelstFeaturePermissionDetails_NeedDataSource(object sender, TreeListNeedDataSourceEventArgs e)
        {
            try
            {
                treelstFeaturePermissionDetails.DataSource = CurrentViewContext.FeaturePermissionData;
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

        protected void treelstFeaturePermissionDetails_PreRender(object sender, EventArgs e)
        {
            try
            {
                treelstFeaturePermissionDetails.ExpandAllItems();
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

        protected void treelstFeaturePermissionDetails_ItemCreated(object sender, TreeListItemCreatedEventArgs e)
        {
            try
            {
                // Below code disables the expand button.
                if (e.Item is TreeListDataItem)
                {
                    Control expandButton = e.Item.FindControl("ExpandCollapseButton");
                    if (expandButton != null)
                    {
                        ((Button)expandButton).Enabled = false;
                        ((Button)expandButton).Visible = false;
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
        /// UAT 983 As an ADB admin, I should be able to unlock a client admin's account from the Client User Search details.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUnlockUser_Click(object sender, EventArgs e)
        {
            try
            {
                Presenter.LockUnlockUser();
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

        protected void btnBackToQueue_Click(object sender, EventArgs e)
        {
            try
            {
                String url = string.Empty;
                if (!string.IsNullOrEmpty(CurrentViewContext.MasterNodeLabel))
                    url = String.Format(@"~\SystemSetup\Pages\InstitutionConfigurationDetails.aspx?Id={0}&SelectedTenantID={1}&NodeName={2}", DeptProgramMappingID, CurrentViewContext.ClientTenantID, CurrentViewContext.MasterNodeLabel.Replace("Node:", string.Empty).Trim());

                else
                    url = String.Format(@"~\SystemSetup\Pages\InstitutionConfigurationDetails.aspx?Id={0}&SelectedTenantID={1}", DeptProgramMappingID, CurrentViewContext.ClientTenantID);

                Response.Redirect(url, true);
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

        protected void treelstBkgHiearachyDetails_NeedDataSource(object sender, TreeListNeedDataSourceEventArgs e)
        {
            try
            {
                treelstBkgHiearachyDetails.DataSource = CurrentViewContext.LstBackgroundTreeData;
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

        protected void treelstBkgHiearachyDetails_ItemCreated(object sender, TreeListItemCreatedEventArgs e)
        {
            try
            {
                // Below code disables the expand button.
                if (e.Item is TreeListDataItem)
                {
                    Control expandButton = e.Item.FindControl("ExpandCollapseButton");
                    if (expandButton != null)
                    {
                        ((Button)expandButton).Enabled = false;
                        ((Button)expandButton).Visible = false;
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

        protected void treelstBkgHiearachyDetails_PreRender(object sender, EventArgs e)
        {
            try
            {
                treelstBkgHiearachyDetails.ExpandAllItems();
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

        #region PUBLIC METHODS
        #endregion

    }
}