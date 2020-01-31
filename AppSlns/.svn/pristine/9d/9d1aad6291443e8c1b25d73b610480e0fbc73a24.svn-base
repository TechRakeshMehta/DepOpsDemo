#region Namespaces

#region SystemDefined

using System;
using Microsoft.Practices.ObjectBuilder;
using System.Data.Entity.Core.Objects;
using System.Collections.Generic;
using CoreWeb.Shell;
using System.Web.UI;
using System.Web.Services;
using System.Linq;

#endregion

#region UserDefined

using Entity.ClientEntity;
using INTSOF.Utils;
using INTSOF.UI.Contract.ComplianceManagement;
using Telerik.Web.UI;
using INTERSOFT.WEB.UI.WebControls;
using System.Web.UI.WebControls;
using CoreWeb.IntsofSecurityModel;


#endregion

#endregion

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class ManageUserGroups : BaseUserControl, IManageUserGroupsView
    {
        #region  Variables
        #region Private Variables
        private ManageUserGroupsPresenter _presenter = new ManageUserGroupsPresenter();
        private Int32 _tenantId;
        public List<Int32> ListSelectedUserGroupIds
        {
            get
            {
                if (ViewState["ListSelectedUserGroupIds"] != null)
                    return ViewState["ListSelectedUserGroupIds"] as List<Int32>;
                else
                    return new List<Int32>();
            }
            set
            {
                ViewState["ListSelectedUserGroupIds"] = value;
            }
        }
        #endregion
        #endregion

        #region  Page Load Event
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                //if (!this.IsPostBack)
                //{
                //    this._presenter.OnViewInitialized();
                //}
                grdUserGroup.Visible = false;
                Presenter.OnViewInitialized();
                ddlTenant.DataSource = ListTenants;
                ddlTenant.DataBind();
                if (Presenter.IsAdminLoggedIn())
                {
                    dvUserGroup.Visible = false;
                    //rbUserGroupState.DataSource = GetArchiveState();
                    //rbUserGroupState.DataBind();
                }
                else
                {
                    SelectedTenantID = TenantId;
                    ddlTenant.SelectedValue = TenantId.ToString();
                    ddlTenant.Enabled = false;
                }
            }
            //this._presenter.OnViewLoaded();
            lblInstitutionHierarchyFilter.Text = hdnHierarchyLabel.Value;
        }
        protected override void OnInit(EventArgs e)
        {
            base.Title = "Manage User Group";
            base.SetPageTitle("Manage User Group");
            base.OnInit(e);
            rbUserGroupState.DataSource = GetArchiveState();
            rbUserGroupState.DataBind();
            rbUserGroupState.SelectedValue = ArchiveState.Active.ToString();
        }
        #endregion

        #region Properties

        #region Public

        public List<Tenant> ListTenants
        {
            set;
            get;
        }
        public Int32 TenantId
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
        public IEnumerable<object> GetArchiveState()
        {
            List<object> result = new List<object>() {
        new { rbText = "Active", rbValue = "Active",
                Selected = true, Enabled = true },
        new { rbText = "Archived", rbValue = "Archived",
                Selected = false, Enabled = true },
        new { rbText = "All", rbValue = "All",
                Selected = false, Enabled = true }
    };

            return result;
        }

        public Int32 SelectedTenantID
        {
            get
            {
                if (ViewState["ClientTenantID"].IsNotNull())
                {
                    return Convert.ToInt32(ViewState["ClientTenantID"]);
                }
                return 0;
            }
            set
            {
                ViewState["ClientTenantID"] = value;
            }
        }
        public IManageUserGroupsView CurrentViewContext
        {
            get
            {
                return this;
            }

        }
        public String UserGroupName
        {
            get;
            set;
        }

        public String UserGroupDescription
        {
            get;
            set;
        }

        public String ErrorMessage
        {
            get;
            set;
        }

        public String SuccessMessage
        {
            get;
            set;
        }
        public Int32 UserGroupId
        {
            get;
            set;
        }
        public List<UserGroup> ListUserGroup
        {
            get;
            set;
        }


        public ManageUserGroupsPresenter Presenter
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

        String IManageUserGroupsView.HierarchyNode
        {
            get;
            set;
        }
        #endregion

        #endregion

        #region Events

        #region Drop Down Selection
        protected void ddlTenant_SelectedIndexChange(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                if (!ddlTenant.SelectedValue.IsNullOrEmpty())
                {
                    SelectedTenantID = Convert.ToInt32(ddlTenant.SelectedValue);
                    if (dvUserGroup.Visible)
                    {
                        dvUserGroup.Visible = true;
                    }
                    if (rbUserGroupState.SelectedValue == ArchiveState.Archived.ToString())
                    {
                        ShowHideControl("Archivemun", "btnArchive", false);

                        ShowHideControl("Archivemun", "btnUnArchive", true);
                    }
                    else if (rbUserGroupState.SelectedValue == ArchiveState.Active.ToString())
                    {
                        ShowHideControl("Archivemun", "btnArchive", true);

                        ShowHideControl("Archivemun", "btnUnArchive", false);
                    }
                    else if (rbUserGroupState.SelectedValue == ArchiveState.All.ToString())
                    {
                        ShowHideControl("Archivemun", "btnArchive", true);

                        ShowHideControl("Archivemun", "btnUnArchive", true);
                    }
                    //grdUserGroup.Rebind();
                    hdnHierarchyLabel.Value = String.Empty;
                    hdnDepartmentProgmapNewFilter.Value = String.Empty;
                    lblInstitutionHierarchyFilter.Text = String.Empty;
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

        #region Grid Related Events

        protected void grdUserGroup_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                    string selectedHierarchyIds = hdnDepartmentProgmapNewFilter.Value;
                    Presenter.GetAllUserGroup(selectedHierarchyIds);
                    if (rbUserGroupState.SelectedValue == ArchiveState.Active.ToString())
                    {
                        CurrentViewContext.ListUserGroup.RemoveAll(cond => cond.UG_IsArchived);
                    }
                    if (rbUserGroupState.SelectedValue == ArchiveState.Archived.ToString())
                    {
                        CurrentViewContext.ListUserGroup.RemoveAll(cond => !cond.UG_IsArchived);
                    }
                    grdUserGroup.DataSource = CurrentViewContext.ListUserGroup;
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

        protected void grdUserGroup_ItemCommand(object sender, GridCommandEventArgs e)
        {
            dvUserGroup.Visible = true;
            // Hide filter when exportig to pdf or word
            if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToPdfCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToWordCommandName
                || e.CommandName == Telerik.Web.UI.RadGrid.ExportToExcelCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToCsvCommandName)
            {
                base.ConfigureExport(grdUserGroup);

            }
        }
        protected void grdUserGroup_InsertCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                CurrentViewContext.UserGroupName = (e.Item.FindControl("txtUserGroupeName") as WclTextBox).Text.Trim();
                CurrentViewContext.UserGroupDescription = (e.Item.FindControl("txtUserGroupeDescription") as WclTextBox).Text.Trim();
                if (hdnDepartmentProgmapNew.Value.IsNullOrEmpty())
                {
                    e.Canceled = true;
                    base.ShowInfoMessage("Please select at least one institution hierarchy.");
                }
                else
                {
                    CurrentViewContext.HierarchyNode = hdnDepartmentProgmapNew.Value;

                    Presenter.SaveUserGroup();
                    if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                    {
                        e.Canceled = true;
                        base.ShowInfoMessage(CurrentViewContext.ErrorMessage);
                    }
                    else
                    {
                        e.Canceled = false;
                        base.ShowSuccessMessage(CurrentViewContext.SuccessMessage);
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
        protected void grdUserGroup_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                CurrentViewContext.UserGroupName = (e.Item.FindControl("txtUserGroupeName") as WclTextBox).Text.Trim();
                CurrentViewContext.UserGroupDescription = (e.Item.FindControl("txtUserGroupeDescription") as WclTextBox).Text.Trim();
                CurrentViewContext.UserGroupId = Convert.ToInt32((e.Item.FindControl("txtUserGroupeId") as WclTextBox).Text.Trim());

                if (hdnDepartmentProgmapNew.Value.IsNullOrEmpty())
                {
                    e.Canceled = true;
                    base.ShowInfoMessage("Please select at least one institution hierarchy.");
                }
                else
                {
                    CurrentViewContext.HierarchyNode = hdnDepartmentProgmapNew.Value;
                    Presenter.UpdateUserGroup();
                    if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                    {
                        e.Canceled = true;
                        base.ShowInfoMessage(CurrentViewContext.ErrorMessage);
                    }
                    else
                    {
                        e.Canceled = false;
                        base.ShowSuccessMessage(CurrentViewContext.SuccessMessage);
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
        protected void grdUserGroup_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                CurrentViewContext.UserGroupId = Convert.ToInt32(gridEditableItem.GetDataKeyValue("UG_ID"));
                Presenter.DeleteUserGroup();
                if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                {
                    e.Canceled = true;

                    base.ShowInfoMessage(CurrentViewContext.ErrorMessage);
                    grdUserGroup.Rebind();
                }
                else
                {
                    e.Canceled = false;
                    base.ShowSuccessMessage(CurrentViewContext.SuccessMessage);
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
        /// Grid Item Bound Expression
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdUserGroup_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                //Checks if item is GridDataItem type.
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    ImageButton editCommandColumn = dataItem["EditCommandColumn"].Controls[0] as ImageButton;
                    String userGroupName = dataItem["UG_Name"].Text;

                    editCommandColumn.ToolTip = "Click to edit this user group: " + userGroupName;
                    ImageButton deleteColumn = dataItem["DeleteColumn"].Controls[0] as ImageButton;
                    deleteColumn.ToolTip = "Click to delete this user group: " + userGroupName;


                }

                if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
                {
                    //UAT-1955
                    hdnControlToSetFocus.Value = "lblTitlePriceAdjustment";

                    hdnDepartmentProgmapNew.Value = String.Empty;
                    UserGroup userGroup = e.Item.DataItem as UserGroup;
                    if (!userGroup.IsNullOrEmpty())
                    {
                        hdnDepartmentProgmapNew.Value = userGroup.HierarchyNodeIdList;
                        hdnInstNodeIdNew.Value = userGroup.HierarchyNodeIdList;
                        (e.Item.FindControl("lblInstitutionHierarchyPB") as Label).Text = userGroup.HierarchyNodeLabelList;
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

        #endregion
        private void ShowHideControl(String menuText, String controlID, Boolean isVisible)
        {
            RadMenuItem menuItem = cmd.FindItemByText(menuText);
            foreach (RadMenuItem item in menuItem.Items)
            {
                RadButton btnMenu = (RadButton)item.FindControl(controlID);
                if (btnMenu.IsNotNull())
                {
                    btnMenu.Visible = isVisible;
                }

            }
        }
        protected void CmdBarSearch_Click(object sender, EventArgs e)
        {
            grdUserGroup.Visible = true;
            grdUserGroup.Rebind();
            dvUserGroup.Visible = true;
        }

        protected void CmdBarReset_Click(object sender, EventArgs e)
        {
            ddlTenant.DataSource = ListTenants;
            ddlTenant.DataBind();
            dvUserGroup.Visible = false;
            rbUserGroupState.SelectedValue = ArchiveState.Active.ToString();
            ddlTenant.SelectedIndex = AppConsts.NONE;
            hdnHierarchyLabel.Value = String.Empty;
            hdnDepartmentProgmapNewFilter.Value = String.Empty;
            lblInstitutionHierarchyFilter.Text = String.Empty;
        }

        protected void CmdBarCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(String.Format(AppConsts.DASHBOARD_PAGE_NAME));
        }

        /// <summary>
        /// UAT 3492 - User Group Archiving and Unarchiving
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnArchieve_Click(object sender, EventArgs e)
        {
            try
            {
                if (CurrentViewContext.ListSelectedUserGroupIds.IsNotNull() && !CurrentViewContext.ListSelectedUserGroupIds.Any())
                {
                    base.ShowErrorInfoMessage("Please select user groups for archiving.");
                }
                else
                {
                    bool isArchive = true;
                    String result = Presenter.ArchiveUnArchiveUserGroups(isArchive);
                    if (result == "true")
                    {
                        base.ShowSuccessMessage("User Group(s) archived successfully.");
                        grdUserGroup.Rebind();
                        //UncheckGridItemsOnArchiveUnarchive(); //UAT-3162 - Refresh filtered results after archiving/unarchiving on compliance search
                        CurrentViewContext.ListSelectedUserGroupIds = null;
                    }
                    else if (result == "false")
                    {
                        base.ShowInfoMessage("The selected user group(s) are already archived.");
                    }
                    else
                    {
                        base.ShowErrorMessage("User Groups are not archived successfully. Please try again.");
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
        /// UAT 3492 - User Group Archiving and Unarchiving
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUnArchive_Click(object sender, EventArgs e)
        {
            try
            {
                if (CurrentViewContext.ListSelectedUserGroupIds.IsNotNull() && !CurrentViewContext.ListSelectedUserGroupIds.Any())
                {
                    base.ShowErrorInfoMessage("Please select user groups for unarchiving.");
                }
                else
                {
                    bool isArchive = false;
                    String result = Presenter.ArchiveUnArchiveUserGroups(isArchive);
                    if (result == "true")
                    {
                        base.ShowSuccessMessage("User Group(s) unarchived successfully.");
                        grdUserGroup.Rebind();
                        //UncheckGridItemsOnArchiveUnarchive(); //UAT-3162 - Refresh filtered results after archiving/unarchiving on compliance search
                        CurrentViewContext.ListSelectedUserGroupIds = null;
                    }
                    else if (result == "false")
                    {
                        base.ShowInfoMessage("The selected user group(s) are already unarchived.");
                    }
                    else
                    {
                        base.ShowErrorMessage("User Groups are not unarchived successfully. Please try again.");
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

        protected void rbUserGroupState_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rbUserGroupState.SelectedValue == ArchiveState.Archived.ToString())
            {
                //btnArchieve.Enabled = false;
                ShowHideControl("Archivemun", "btnArchive", false);

                ShowHideControl("Archivemun", "btnUnArchive", true);
            }
            else if (rbUserGroupState.SelectedValue == ArchiveState.Active.ToString())
            {
                ShowHideControl("Archivemun", "btnArchive", true);

                ShowHideControl("Archivemun", "btnUnArchive", false);
            }
            else if (rbUserGroupState.SelectedValue == ArchiveState.All.ToString())
            {
                ShowHideControl("Archivemun", "btnArchive", true);

                ShowHideControl("Archivemun", "btnUnArchive", true);
            }
            foreach (GridDataItem item in grdUserGroup.Items)
            {
                ((CheckBox)item.FindControl("chkSelectUser")).Checked = false;
            }
        }

        protected void chkSelectUser_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox checkBox = sender as CheckBox;
                Boolean isChecked = false;
                if (checkBox.IsNull())
                {
                    return;
                }
                GridDataItem dataItem = (GridDataItem)checkBox.NamingContainer;
                List<Int32> selectedUserGroupIds = CurrentViewContext.ListSelectedUserGroupIds;
                isChecked = ((CheckBox)dataItem.FindControl("chkSelectUser")).Checked;
                Int32 userGroupID = (Int32)dataItem.GetDataKeyValue("UG_ID");

                if (isChecked)
                {
                    if (!selectedUserGroupIds.Contains(userGroupID))
                    {
                        selectedUserGroupIds.Add(userGroupID);
                    }
                }
                CurrentViewContext.ListSelectedUserGroupIds = selectedUserGroupIds;

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

        protected void ddlTenant_DataBound(object sender, EventArgs e)
        {
            var selectInsert = new RadComboBoxItem("--Select--");
            if (!ddlTenant.Items[0].Text.Equals("--Select--"))
            {
                ddlTenant.Items.Insert(0, new RadComboBoxItem("--Select--"));
            }
        }
    }
}

