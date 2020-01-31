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
using Entity;
using Entity.ClientEntity;
using Telerik.Web.UI;
using INTSOF.UI.Contract.SystemSetUp;
using CoreWeb.ComplianceAdministration.Views;

namespace CoreWeb.Shell.Views
{
    public partial class AdditionalAccountVerification : BaseWebPage, IAdditionalAccountVerification
    {
        #region VARIABLES
        private AdditionalAccountVerificationPresenter _presenter = new AdditionalAccountVerificationPresenter();
        #endregion

        #region PROPERTIES

        #region PRIVATE PROPERTIES

        #endregion

        #region PUBLIC PROPERTIES

        public AdditionalAccountVerificationPresenter Presenter
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

        public Guid UserId
        {
            get
            {
                return new Guid(SysXWebSiteUtils.SessionService.UserId);
            }
        }

        public String UsrVerCode
        {
            get
            {
                return Convert.ToString(ViewState["UsrVerCode"]);
            }
            set
            {
                ViewState["UsrVerCode"] = value;
            }
        }

        public Entity.OrganizationUser OrganizationUser
        {
            get
            {
                return (ViewState["OrganizationUser"]) as Entity.OrganizationUser;
            }
            set
            {
                ViewState["OrganizationUser"] = value;
            }
        }

        public Boolean AccountVerificationMainSetting
        {
            get
            {
                return Convert.ToBoolean(ViewState["AccountVerificationMainSetting"]);
            }
            set
            {
                ViewState["AccountVerificationMainSetting"] = value;
            }
        }

        public Boolean AccVerificationProcessResponseReqdSetting
        {
            get
            {
                return Convert.ToBoolean(ViewState["AccVerificationProcessResponseReqdSetting"]);
            }
            set
            {
                ViewState["AccVerificationProcessResponseReqdSetting"] = value;
            }
        }

        public Boolean AccVerificationProcessDOBSetting
        {
            get
            {
                return Convert.ToBoolean(ViewState["AccVerificationProcessDOBSetting"]);
            }
            set
            {
                ViewState["AccVerificationProcessDOBSetting"] = value;
            }
        }

        public Boolean AccVerificationProcessSSNSetting
        {
            get
            {
                return Convert.ToBoolean(ViewState["AccVerificationProcessSSNSetting"]);
            }
            set
            {
                ViewState["AccVerificationProcessSSNSetting"] = value;
            }
        }

        public Boolean AccVerificationProcessLSSNSetting
        {
            get
            {
                return Convert.ToBoolean(ViewState["AccVerificationProcessLSSNSetting"]);
            }
            set
            {
                ViewState["AccVerificationProcessLSSNSetting"] = value;
            }
        }

        public Boolean AccVerificationProcessProfCustAttrSetting
        {
            get
            {
                return Convert.ToBoolean(ViewState["AccVerificationProcessProfCustAttrSetting"]);
            }
            set
            {
                ViewState["AccVerificationProcessProfCustAttrSetting"] = value;
            }
        }

        public String AccVerificationProcessDOBTextSetting
        {
            get
            {
                return Convert.ToString(ViewState["AccVerificationProcessDOBTextSetting"]);
            }
            set
            {
                ViewState["AccVerificationProcessDOBTextSetting"] = value;
            }
        }

        public String AccVerificationProcessSSNTextSetting
        {
            get
            {
                return Convert.ToString(ViewState["AccVerificationProcessSSNTextSetting"]);
            }
            set
            {
                ViewState["AccVerificationProcessSSNTextSetting"] = value;
            }
        }

        public String AccVerificationProcessLSSNTextSetting
        {
            get
            {
                return Convert.ToString(ViewState["AccVerificationProcessLSSNTextSetting"]);
            }
            set
            {
                ViewState["AccVerificationProcessLSSNTextSetting"] = value;
            }
        }

        public String AccVerificationProcessProfCustAttrTextSetting
        {
            get
            {
                return Convert.ToString(ViewState["AccVerificationProcessProfCustAttrTextSetting"]);
            }
            set
            {
                ViewState["AccVerificationProcessProfCustAttrTextSetting"] = value;
            }
        }

        public List<Entity.ClientEntity.lkpSetting> LstSettings { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public String SSN { get; set; }

        public String LSSN { get; set; }

        public List<TypeCustomAttributes> LstProfileAttr { get; set; }

        public List<TypeCustomAttributes> lstCustomAttrUserData { get; set; }

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
            try
            {
                if (!IsPostBack)
                {
                    Dictionary<String, String> args = new Dictionary<String, String>();
                    if (!Request.QueryString["UsrVerCode"].IsNull())
                    {
                        UsrVerCode = Convert.ToString(Request.QueryString["UsrVerCode"]);
                    }
                }

                Presenter.GetOrganizationUserByVerificationCode();
                if (OrganizationUser.IsNotNull())
                {
                    TenantID = Convert.ToInt32(OrganizationUser.Organization.TenantID);
                    Presenter.GetAccountVerificationSettings();

                    if (AccountVerificationMainSetting)
                    {
                        if (AccVerificationProcessResponseReqdSetting)
                        {
                            //All Permission
                            BindAllPermissionSection();
                        }
                        else
                        {
                            //Any Permission
                            dvSectionWithAnyPrmsn.Visible = true;
                            if (!IsPostBack)
                            {
                                BindVerificationQuestions();
                            }
                        }
                    }
                    else
                    {
                        RedirectToLogin();
                    }
                }
                else
                {
                    RedirectToLogin();
                }

                int selectedCustomAttrID;
                if (Int32.TryParse(cmbQuestions.SelectedValue, out selectedCustomAttrID))
                {
                    List<ClientSettingCustomAttributeContract> lstClientSettingustAttributes = Presenter.GetClientSettingCustomAttribute().Where(x => x.CustomAttributeID == selectedCustomAttrID).ToList();
                    //Control caProfileCustomAttributesAny = Page.LoadControl("~/ComplianceAdministration/UserControl/CustomAttributeProfileLoader.ascx");
                    caProfileCustomAttributesAny.ID = "caProfileCustomAttributesAny";
                    caProfileCustomAttributesAny.ClientIDMode = ClientIDMode.Static;
                    caProfileCustomAttributesAny.EnableViewState = false;
                    caProfileCustomAttributesAny.LstProfileCustomAttributeOverride = lstClientSettingustAttributes;
                    caProfileCustomAttributesAny.TenantId = TenantID;
                    caProfileCustomAttributesAny.TypeCode = CustomAttributeUseTypeContext.Profile.GetStringValue();
                    caProfileCustomAttributesAny.DataSourceModeType = DataSourceMode.Ids;
                    caProfileCustomAttributesAny.ControlDisplayMode = DisplayMode.Controls;
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

        protected void cmbQuestions_DataBound(object sender, EventArgs e)
        {
            try
            {
                cmbQuestions.Items.Insert(0, new RadComboBoxItem("--Select--"));
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

        protected void cmbQuestions_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                if (cmbQuestions.SelectedIndex > AppConsts.NONE)
                {
                    dvAnswer.Style["display"] = "block";
                }
                else
                {
                    dvAnswer.Style["display"] = "none";
                }
                SetAnswerControlByQuestionCode();
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

        protected void fsucCmdBarButton_SubmitClick(object sender, EventArgs e)
        {
            try
            {
                if (AccVerificationProcessResponseReqdSetting)
                {
                    //If setting is on the get the value.
                    if (AccVerificationProcessDOBSetting)
                    {
                        DateOfBirth = dpkrDOB.SelectedDate;
                    }
                    if (AccVerificationProcessSSNSetting)
                    {
                        SSN = txtSSN.Text;
                    }
                    if (AccVerificationProcessLSSNSetting)
                    {
                        LSSN = txtLSSN.Text;
                    }
                    if (AccVerificationProcessProfCustAttrSetting)
                    {
                        LstProfileAttr = caProfileCustomAttributes.GetCustomAttributeValues();
                    }

                    if (ValidateUser())
                    {
                        Presenter.ActivateUser();
                        hdnIsUserActive.Value = FormsAuthentication.LoginUrl + "?IsUserActivated=1";
                        System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "returnToParent();", true);
                    }
                    else
                    {
                        lblSuccess.Visible = true;
                        lblSuccess.ShowMessage("Information provided by you doesn't match with our record, please try again", MessageType.Information);
                    }
                }
                else
                {
                    String PrsmnCode = cmbQuestions.SelectedValue;
                    if (ValidateUserWithAnyOnePermission(PrsmnCode))
                    {
                        Presenter.ActivateUser();
                        hdnIsUserActive.Value = FormsAuthentication.LoginUrl + "?IsUserActivated=1";
                        System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "returnToParent();", true);
                        //Response.Redirect(FormsAuthentication.LoginUrl + "?IsUserActivated=1");
                    }
                    else
                    {
                        lblSuccess.ShowMessage("Information provided by you doesn't match with our record, please try again", MessageType.Information);
                        lblSuccess.Visible = true;
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

        protected void fsucCmdBarButton_CancelClick(object sender, EventArgs e)
        {
            try
            {
                RedirectToLogin();
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

        #region PRIVATE METHODS

        private void BindVerificationQuestions()
        {
            Presenter.GetLkpSettings();
            cmbQuestions.DataSource = LstSettings;
            cmbQuestions.DataBind();
            cmbQuestions.Focus();
        }

        /// <summary>
        /// Method to Redirect the User to login page
        /// </summary>
        private void RedirectToLogin()
        {
            SysXWebSiteUtils.SessionService.ClearSession(true);
            Response.Redirect(FormsAuthentication.LoginUrl);
        }

        private void SetAnswerControlByQuestionCode()
        {
            rfvDOBAny.Enabled = false;
            rfvSSNAny.Enabled = false;
            revSSNAny.Enabled = false;
            rfvLastSSNAny.Enabled = false;
            revLastSSNAny.Enabled = false;
            dpkrDOBAny.Visible = false;
            txtSSNAny.Visible = false;
            txtLastSSNAny.Visible = false;
            dvCustAttrAny.Visible = false;

            if (cmbQuestions.SelectedValue == Setting.ACCOUNT_VERIFICATION_PROCESS_DOB_PERMISSION.GetStringValue())
            {
                dpkrDOBAny.Visible = true;
                rfvDOBAny.Enabled = true;
            }
            if (cmbQuestions.SelectedValue == Setting.ACCOUNT_VERIFICATION_PROCESS_SSN_PERMISSION.GetStringValue())
            {
                txtSSNAny.Visible = true;
                rfvSSNAny.Enabled = true;
                revSSNAny.Enabled = true;
            }
            if (cmbQuestions.SelectedValue == Setting.ACCOUNT_VERIFICATION_PROCESS_LSSN_PERMISSION.GetStringValue())
            {
                txtLastSSNAny.Visible = true;
                rfvLastSSNAny.Enabled = true;
                revLastSSNAny.Enabled = true;
            }

            int selectedCustomAttrID;
            if (Int32.TryParse(cmbQuestions.SelectedValue, out selectedCustomAttrID))
            {
                //List<ClientSettingCustomAttributeContract> lstClientSettingustAttributes = Presenter.GetClientSettingCustomAttribute().Where(x => x.CustomAttributeID == Convert.ToInt32(cmbQuestions.SelectedValue)).ToList();
                //Control caProfileCustomAttributesAny = Page.LoadControl("~/ComplianceAdministration/UserControl/CustomAttributeProfileLoader.ascx");
                //(caProfileCustomAttributesAny as CustomAttributeProfileLoader).ID = "caProfileCustomAttributesAny";
                //(caProfileCustomAttributesAny as CustomAttributeProfileLoader).ClientIDMode = ClientIDMode.Static;
                //(caProfileCustomAttributesAny as CustomAttributeProfileLoader).LstProfileCustomAttributeOverride = lstClientSettingustAttributes;
                //(caProfileCustomAttributesAny as CustomAttributeProfileLoader).TenantId = TenantID;
                //(caProfileCustomAttributesAny as CustomAttributeProfileLoader).TypeCode = CustomAttributeUseTypeContext.Profile.GetStringValue();
                //(caProfileCustomAttributesAny as CustomAttributeProfileLoader).DataSourceModeType = DataSourceMode.Ids;
                //(caProfileCustomAttributesAny as CustomAttributeProfileLoader).ControlDisplayMode = DisplayMode.Controls;
                //pnlCustAttrAny.Controls.Add(caProfileCustomAttributesAny);

                dvCustAttrAny.Visible = true;
                Control ucAttributes = caProfileCustomAttributesAny as Control;
                Control hdfCAId = ucAttributes.FindServerControlRecursively("hdfCAId");
                HiddenField hdCAID = hdfCAId as HiddenField;
                hdCAID.Value = Convert.ToString(selectedCustomAttrID);
            }
        }

        private void BindAllPermissionSection()
        {
            //All Permission
            dvSectionWithAllPrmsn.Visible = true;
            dvSectionWithAllPrmsn.Focus();
            if (AccVerificationProcessDOBSetting)
            {
                lblDOB.Text = (AccVerificationProcessDOBTextSetting.IsNullOrEmpty() ? "Date of Birth" : AccVerificationProcessDOBTextSetting).HtmlEncode();
                dvDOB.Visible = true;
                rfvDOB.Enabled = true;
            }
            if (AccVerificationProcessSSNSetting)
            {
                lblSSN.Text = (AccVerificationProcessSSNTextSetting.IsNullOrEmpty() ? "Social Security Number" : AccVerificationProcessSSNTextSetting).HtmlEncode();
                dvSSN.Visible = true;
                rfvSSN.Enabled = true;
                revtxtSSN.Enabled = true;
            }
            if (AccVerificationProcessLSSNSetting)
            {
                lblLSSN.Text = (AccVerificationProcessLSSNTextSetting.IsNullOrEmpty() ? "Last four SSN" : AccVerificationProcessLSSNTextSetting).HtmlEncode();
                dvLSSN.Visible = true;
                rfvLSSN.Enabled = true;
                revLSSN.Enabled = true;
            }
            if (AccVerificationProcessProfCustAttrSetting)
            {
                List<ClientSettingCustomAttributeContract> lstClientSettingustAttributes = Presenter.GetClientSettingCustomAttribute();

                caProfileCustomAttributes.TenantId = TenantID;
                caProfileCustomAttributes.LstProfileCustomAttributeOverride = lstClientSettingustAttributes;
                caProfileCustomAttributes.TypeCode = CustomAttributeUseTypeContext.Profile.GetStringValue();
                caProfileCustomAttributes.DataSourceModeType = DataSourceMode.Ids;
                caProfileCustomAttributes.ControlDisplayMode = DisplayMode.Controls;
                //dvProfileCustAttr.Visible = true;
                dvProfileCustAttr.Style["display"] = "block";
            }
        }

        private Boolean ValidateUser()
        {
            Boolean isValidUser = false;
            if (AccVerificationProcessDOBSetting)
            {
                //Check DOB
                if (OrganizationUser.DOB.IsNotNull() && OrganizationUser.DOB.Value.Date == DateOfBirth.Value.Date)
                {
                    isValidUser = true;
                }
                else
                {
                    return isValidUser = false;
                }
            }
            if (AccVerificationProcessSSNSetting)
            {
                //Check SSN
                if (OrganizationUser.SSN.IsNotNull() && OrganizationUser.SSN == SSN)
                {
                    isValidUser = true;
                }
                else
                {
                    return isValidUser = false;
                }
            }

            if (AccVerificationProcessLSSNSetting)
            {
                //Check Last Four SSN
                if (OrganizationUser.SSNL4.IsNotNull() && OrganizationUser.SSNL4 == LSSN)
                {
                    isValidUser = true;
                }
                else
                {
                    return isValidUser = false;
                }
            }

            if (AccVerificationProcessProfCustAttrSetting)
            {
                //todo
                Presenter.GetProfileCustomAttributeByOrgUserID();
                if (!lstCustomAttrUserData.IsNullOrEmpty())
                {
                    foreach (var newProfAttrEnteredData in LstProfileAttr)
                    {
                        TypeCustomAttributes prevProfAttrEnteredData = lstCustomAttrUserData.Where(x => x.CAId == newProfAttrEnteredData.CAId).FirstOrDefault();
                        if (prevProfAttrEnteredData.IsNotNull() && prevProfAttrEnteredData.CAValue == newProfAttrEnteredData.CAValue)
                        {
                            isValidUser = true;
                        }
                        else
                        {
                            return isValidUser = false;
                        }
                    }

                }

            }
            return isValidUser;
        }

        private Boolean ValidateUserWithAnyOnePermission(String PrsmnCode)
        {
            if (PrsmnCode == Setting.ACCOUNT_VERIFICATION_PROCESS_DOB_PERMISSION.GetStringValue())
            {
                DateOfBirth = dpkrDOBAny.SelectedDate;
                if (OrganizationUser.DOB.IsNotNull() && OrganizationUser.DOB.Value.Date == DateOfBirth.Value.Date)
                {
                    return true;
                }
            }
            if (PrsmnCode == Setting.ACCOUNT_VERIFICATION_PROCESS_SSN_PERMISSION.GetStringValue())
            {
                SSN = txtSSNAny.Text;
                if (OrganizationUser.SSN.IsNotNull() && OrganizationUser.SSN == SSN)
                {
                    return true;
                }
            }
            if (PrsmnCode == Setting.ACCOUNT_VERIFICATION_PROCESS_LSSN_PERMISSION.GetStringValue())
            {
                LSSN = txtLastSSNAny.Text;
                if (OrganizationUser.SSNL4.IsNotNull() && OrganizationUser.SSNL4 == LSSN)
                {
                    return true;
                }
            }

            int selectedCustomAttrID;
            if (Int32.TryParse(PrsmnCode, out selectedCustomAttrID))
            {
                Boolean isValidUser = false;
                CustomAttributeProfileLoader caProfileCustomAttributesAny = pnlCustAttrAny.FindServerControlRecursively("caProfileCustomAttributesAny") as CustomAttributeProfileLoader;
                LstProfileAttr = caProfileCustomAttributesAny.GetCustomAttributeValues();
                Presenter.GetProfileCustomAttributeByOrgUserID();
                if (!lstCustomAttrUserData.IsNullOrEmpty())
                {
                    foreach (var newProfAttrEnteredData in LstProfileAttr)
                    {
                        TypeCustomAttributes prevProfAttrEnteredData = lstCustomAttrUserData.Where(x => x.CAId == newProfAttrEnteredData.CAId).FirstOrDefault();
                        if (prevProfAttrEnteredData.IsNotNull() && prevProfAttrEnteredData.CAValue == newProfAttrEnteredData.CAValue)
                        {
                            isValidUser = true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    return isValidUser;
                }
            }
            return false;
        }



        //        dvCustAttrAny.Visible = true;
        //    }
        //}

        //private void BindAllPermissionSection()
        //{
        //    //All Permission
        //    dvSectionWithAllPrmsn.Visible = true;
        //    dvSectionWithAllPrmsn.Focus();
        //    if (AccVerificationProcessDOBSetting)
        //    {
        //        lblDOB.Text = AccVerificationProcessDOBTextSetting.IsNullOrEmpty() ? "Date of Birth" : AccVerificationProcessDOBTextSetting;
        //        dvDOB.Visible = true;
        //        rfvDOB.Enabled = true;
        //    }
        //    if (AccVerificationProcessSSNSetting)
        //    {
        //        lblSSN.Text = AccVerificationProcessSSNTextSetting.IsNullOrEmpty() ? "Social Security Number" : AccVerificationProcessSSNTextSetting;
        //        dvSSN.Visible = true;
        //        rfvSSN.Enabled = true;
        //        revtxtSSN.Enabled = true;
        //    }
        //    if (AccVerificationProcessLSSNSetting)
        //    {
        //        lblLSSN.Text = AccVerificationProcessLSSNTextSetting.IsNullOrEmpty() ? "Last four SSN" : AccVerificationProcessLSSNTextSetting;
        //        dvLSSN.Visible = true;
        //        rfvLSSN.Enabled = true;
        //        revLSSN.Enabled = true;
        //    }
        //    if (AccVerificationProcessProfCustAttrSetting)
        //    {
        //        if (!AccVerificationProcessProfCustAttrTextSetting.IsNullOrEmpty())
        //        {
        //            caProfileCustomAttributes.Title = AccVerificationProcessProfCustAttrTextSetting;
        //        }
        //        caProfileCustomAttributes.TenantId = TenantID;
        //        caProfileCustomAttributes.TypeCode = CustomAttributeUseTypeContext.Profile.GetStringValue();
        //        caProfileCustomAttributes.DataSourceModeType = DataSourceMode.Ids;
        //        caProfileCustomAttributes.ControlDisplayMode = DisplayMode.Controls;
        //        dvProfileCustAttr.Visible = true;
        //    }
        //}

        #endregion
    }
}