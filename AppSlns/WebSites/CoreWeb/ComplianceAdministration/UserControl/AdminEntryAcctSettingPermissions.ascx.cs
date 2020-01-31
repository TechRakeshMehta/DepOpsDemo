using CoreWeb.ComplianceAdministration.Views;
using CoreWeb.Shell;
using Entity.ClientEntity;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CoreWeb.ComplianceAdministration.UserControl
{
    public partial class AdminEntryAcctSettingPermissions : BaseUserControl, IAdminEntryAcctSettingPermissionsView
    {
        #region Private Variables

        private AdminEntryAcctSettingPermissionsPresenter _presenter = new AdminEntryAcctSettingPermissionsPresenter();
        #endregion

        #region Properties
        public AdminEntryAcctSettingPermissionsPresenter Presenter
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

        public IAdminEntryAcctSettingPermissionsView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public int CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        Int32 IAdminEntryAcctSettingPermissionsView.TenantId
        {
            get
            {
                return (Int32)(ViewState["TenantId"]);
            }
            set
            {
                ViewState["TenantId"] = value;
            }
        }

        Int32 IAdminEntryAcctSettingPermissionsView.DefaultTenantId
        {
            get
            {
                return Convert.ToInt32(ViewState["DefaultTenantId"]);
            }
            set
            {
                ViewState["DefaultTenantId"] = value;
            }
        }

        Int32 IAdminEntryAcctSettingPermissionsView.DeptProgramMappingID
        {
            get
            {
                return (Int32)(ViewState["DepProgramMappingID"]);
            }
            set
            {
                ViewState["DepProgramMappingID"] = value;
            }
        }

        List<lkpApplicantInviteSubmitStatusType> IAdminEntryAcctSettingPermissionsView.lstApplicantInviteSubmitStatus { get; set; }

        List<lkpAdminEntryAccountSetting> IAdminEntryAcctSettingPermissionsView.lstAdminEntryAccountSetting
        {
            get
            {
                return (List<lkpAdminEntryAccountSetting>)(ViewState["lstAdminEntryAccountSetting"]);
            }
            set
            {
                ViewState["lstAdminEntryAccountSetting"] = value;
            }
        }

        String IAdminEntryAcctSettingPermissionsView.ErrorMessage { get; set; }
        String IAdminEntryAcctSettingPermissionsView.SuccessMessage { get; set; }
        String IAdminEntryAcctSettingPermissionsView.InfoMessage { get; set; }

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                CurrentViewContext.TenantId = Convert.ToInt32(Request.QueryString["SelectedTenantId"]);
                CurrentViewContext.DeptProgramMappingID = Convert.ToInt32(Request.QueryString["Id"]);
                Presenter.OnViewInitialized();
                BindApplicantInviteSubmitStatus();
                BindControls();
            }
        }

        public void BindControls()
        {
            List<DeptProgramAdminEntryAcctSetting> deptProgramAdminEntryAccts = Presenter.GetDeptProgramAdminEntryAcctSettings();
            if (!deptProgramAdminEntryAccts.IsNullOrEmpty() && deptProgramAdminEntryAccts.Count > 0)
            {
                foreach (var item in deptProgramAdminEntryAccts)
                {
                    if (item.lkpAdminEntryAccountSetting.AEAS_Code == AdminEntryAccountSettingEnum.AddNewOrder_Active.GetStringValue())
                    {
                        rbtnOrderActive.SelectedValue = item.DPAEAS_SettingValue;
                    }
                    else if (item.lkpAdminEntryAccountSetting.AEAS_Code == AdminEntryAccountSettingEnum.AddApplicantInvite_Active.GetStringValue())
                    {
                        rbtnAppInviteActive.SelectedValue = item.DPAEAS_SettingValue;
                    }
                    else if (item.lkpAdminEntryAccountSetting.AEAS_Code == AdminEntryAccountSettingEnum.Auto_ArchiveTimeLine.GetStringValue())
                    {
                        ntxtDays.Text = item.DPAEAS_SettingValue.IsNullOrEmpty()?"30": item.DPAEAS_SettingValue;
                    }
                    else if (item.lkpAdminEntryAccountSetting.AEAS_Code == AdminEntryAccountSettingEnum.OnHoldStatus.GetStringValue())
                    {
                        rbtnHoldStatus.SelectedValue = item.DPAEAS_SettingValue;
                    }
                    else if (item.lkpAdminEntryAccountSetting.AEAS_Code == AdminEntryAccountSettingEnum.ApplicantInvite_SubmitStatus.GetStringValue())
                    {
                        ddlApplicantInviteStatus.SelectedValue = item.DPAEAS_SettingValue;
                    }
                }
            }
            else
            {
                ntxtDays.Text = "30";
            }
        }

        public void BindApplicantInviteSubmitStatus()
        {
            Presenter.GetApplicantInviteSubmitStatus();
            ddlApplicantInviteStatus.DataSource = CurrentViewContext.lstApplicantInviteSubmitStatus;
            ddlApplicantInviteStatus.DataBind();
        }

        protected void fsucCmdBarVendorAcctSettings_SaveClick(object sender, EventArgs e)
        {
            try
            {
                String IsAddNewOrderActive = rbtnOrderActive.SelectedValue;
                String IsAppInviteActive = rbtnAppInviteActive.SelectedValue;
                //String archiveTimeline = ntxtDays.Text.IsNullOrEmpty() ? "30" : ntxtDays.Text;
                String archiveTimeline = !ntxtDays.Text.IsNullOrEmpty() ? ntxtDays.Text : null;
                String IsHoldStatus = rbtnHoldStatus.SelectedValue;
                String selectedApplicantInviteStatusId = Convert.ToString(ddlApplicantInviteStatus.SelectedValue);

                Dictionary<String, String> dicSettings = new Dictionary<String, String>
                                          {
                    { AdminEntryAccountSettingEnum.AddNewOrder_Active.GetStringValue(), IsAddNewOrderActive},
                    { AdminEntryAccountSettingEnum.AddApplicantInvite_Active.GetStringValue(), IsAppInviteActive},
                    { AdminEntryAccountSettingEnum.Auto_ArchiveTimeLine.GetStringValue(), archiveTimeline},
                    { AdminEntryAccountSettingEnum.OnHoldStatus.GetStringValue(), IsHoldStatus},
                    { AdminEntryAccountSettingEnum.ApplicantInvite_SubmitStatus.GetStringValue(), selectedApplicantInviteStatusId }
                                          };

                Presenter.SaveNodeSettingsForAdminEntry(dicSettings);
                if (String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                {
                    (this.Page as BaseWebPage).ShowSuccessMessage(CurrentViewContext.SuccessMessage);
                }
                else
                {
                    (this.Page as BaseWebPage).ShowInfoMessage(CurrentViewContext.ErrorMessage);
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

        protected void ddlApplicantInviteStatus_DataBound(object sender, EventArgs e)
        {
            try
            {
                ddlApplicantInviteStatus.Items.Insert(0, new RadComboBoxItem("--Select--"));
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
    }
}