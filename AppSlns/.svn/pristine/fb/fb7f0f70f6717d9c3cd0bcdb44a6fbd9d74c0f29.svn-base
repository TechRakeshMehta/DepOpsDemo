using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.SearchUI.Views;
using CoreWeb.Shell;
using INTSOF.UI.Contract.SearchUI;
using INTSOF.Utils;
using Telerik.Web.UI;
using Entity.ClientEntity;
using INTERSOFT.WEB.UI.WebControls;

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class UserGroupMapping : BaseWebPage, IUserGroupMappingView
    {
        #region  Variables
        #region Private Variables
        private UserGroupMappingPresenter _presenter = new UserGroupMappingPresenter();
        private Int32 _tenantId;
        #endregion
        #endregion

        #region Properties

        #region Public

        public UserGroupMappingPresenter Presenter
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

        public Int32 SelectedTenantID
        {
            get
            {
                if (ViewState["SelectedTenantID"].IsNotNull())
                {
                    return Convert.ToInt32(ViewState["SelectedTenantID"]);
                }
                return 0;
            }
            set
            {
                ViewState["SelectedTenantID"] = value;
            }
        }

        public IUserGroupMappingView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public Int32 CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
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

        String IUserGroupMappingView.HierarchyNode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or Sets the value for selected Items.
        /// </summary>
        public List<Int32> AssignUserGroupIds
        {
            get
            {
                if (!ViewState["AssignUserGroupIds"].IsNull())
                {
                    return ViewState["AssignUserGroupIds"] as List<Int32>;
                }

                return new List<Int32>();
            }
            set
            {
                ViewState["AssignUserGroupIds"] = value;
            }
        }

        public List<Int32> ApplicantUserIds
        {
            get
            {
                if (!Session["OrgUsersToList"].IsNullOrEmpty())
                {
                    var orgUsersToList = Session["OrgUsersToList"] as Dictionary<Int32, String>;
                    return orgUsersToList.Keys.ToList();
                }
                return new List<Int32>();
            }
        }

        public String ScreenMode
        {
            get
            {
                if (ViewState["ScreenMode"].IsNotNull())
                {
                    return Convert.ToString(ViewState["ScreenMode"]);
                }
                return null;
            }
            set
            {
                ViewState["ScreenMode"] = value;
            }
        }

        #endregion

        #endregion

        #region Events

        #region  Page Load Event

        protected override void OnInit(EventArgs e)
        {
            base.Title = "User Group Mapping";
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                //Capture Querystring parameters
                CaptureQuerystringParameters();

                if (Presenter.IsAdminLoggedIn())
                {

                }
            }
            ShowHideControls();

        }

        #endregion

        #region Grid Related Events

        protected void grdUserGroup_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetAllUserGroup();
                grdUserGroup.DataSource = CurrentViewContext.ListUserGroup;

                if (CurrentViewContext.ScreenMode.ToLower() != "assign")
                    grdUserGroup.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;
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
            // Hide filter when exportig to pdf or word
            //if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToPdfCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToWordCommandName
            //    || e.CommandName == Telerik.Web.UI.RadGrid.ExportToExcelCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToCsvCommandName)
            //{
            //    base.ConfigureExport(grdUserGroup);

            //}
        }

        protected void grdUserGroup_InsertCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                CurrentViewContext.UserGroupName = (e.Item.FindControl("txtUserGroupeName") as WclTextBox).Text.Trim();
                CurrentViewContext.UserGroupDescription = (e.Item.FindControl("txtUserGroupeDescription") as WclTextBox).Text.Trim();
                //UAT-3381
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
                        base.ShowErrorInfoMessage(CurrentViewContext.ErrorMessage);
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

        protected void grdUserGroup_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                //To select checkboxes
                if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
                {

                    GridDataItem dataItem = (GridDataItem)e.Item;
                    String usergroupID = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UG_ID"].ToString();

                    if (Convert.ToInt32(usergroupID) != 0)
                    {
                        List<Int32> selectedItems = CurrentViewContext.AssignUserGroupIds;
                        if (selectedItems.IsNotNull())
                        {
                            if (selectedItems.Contains(Convert.ToInt32(usergroupID)))
                            {
                                CheckBox checkBox = ((CheckBox)e.Item.FindControl("chkSelectItem"));
                                checkBox.Checked = true;
                            }
                        }
                    }
                    else
                    {
                        CheckBox checkBox = ((CheckBox)e.Item.FindControl("chkSelectItem"));
                        checkBox.Enabled = false;
                    }
                }
                if (e.Item.ItemType.Equals(GridItemType.Footer))
                {
                    Int32 rowCount = grdUserGroup.Items.Count;
                    if (rowCount > 0)
                    {
                        Int32 checkCount = 0;
                        foreach (GridDataItem item in grdUserGroup.Items)
                        {
                            CheckBox checkBox = ((CheckBox)item.FindControl("chkSelectItem"));
                            if (checkBox.Checked)
                            {
                                checkCount++;
                            }
                        }
                        if (rowCount == checkCount)
                        {
                            GridHeaderItem item = grdUserGroup.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
                            CheckBox checkBox = ((CheckBox)item.FindControl("chkSelectAll"));
                            checkBox.Checked = true;
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

        #region Button Events

        protected void CmdBarAssign_Click(object sender, EventArgs e)
        {
            try
            {
                if (CurrentViewContext.SelectedTenantID == 0)
                {
                    base.ShowInfoMessage("Please select an institute to assign applicant(s) to user group(s).");
                }
                else if (!AssignUserGroupIds.Any())
                {
                    base.ShowInfoMessage("Please select at least one user group to assign applicant(s).");
                }
                else
                {
                    if (Presenter.AssignUserGroups())
                        base.ShowSuccessMessage("User group(s) assigned successfully.");
                    else
                        base.ShowErrorInfoMessage("Some error occurred. Please try again.");

                    CurrentViewContext.AssignUserGroupIds = new List<Int32>();
                    grdUserGroup.Rebind();
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

        protected void CmdBarUnassign_Click(object sender, EventArgs e)
        {
            try
            {
                if (CurrentViewContext.SelectedTenantID == 0)
                {
                    base.ShowInfoMessage("Please select an institute to unassign applicant(s) from user group(s).");
                }
                else if (!AssignUserGroupIds.Any())
                {
                    base.ShowInfoMessage("Please select at least one user group to unassign applicant(s).");
                }
                else
                {
                    if (Presenter.UnassignUserGroups())
                        base.ShowSuccessMessage("User group(s) unassigned successfully.");
                    else
                        base.ShowErrorInfoMessage("Some error occurred. Please try again.");

                    CurrentViewContext.AssignUserGroupIds = new List<Int32>();
                    grdUserGroup.Rebind();
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

        protected void CmdBarCancel_Click(object sender, EventArgs e)
        {
            //Reset session
            //Session[AppConsts.APPLICANT_SEARCH_SESSION_KEY] = null;
            //Dictionary<String, String> queryString = new Dictionary<String, String>();
            //Response.Redirect(String.Format(AppConsts.DASHBOARD_PAGE_NAME), true);
        }

        #endregion

        protected void chkSelectItem_CheckedChanged(object sender, EventArgs e)
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
                List<Int32> selectedItems = CurrentViewContext.AssignUserGroupIds;
                Int32 usergroupID = (Int32)dataItem.GetDataKeyValue("UG_ID");
                isChecked = ((CheckBox)dataItem.FindControl("chkSelectItem")).Checked;

                if (isChecked)
                {
                    if (!selectedItems.Contains(usergroupID))
                    {
                        selectedItems.Add(usergroupID);
                    }
                }
                else
                {
                    if (selectedItems != null && selectedItems.Contains(usergroupID))
                    {
                        selectedItems.Remove(usergroupID);
                    }
                }

                CurrentViewContext.AssignUserGroupIds = selectedItems;
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

        #region Methods

        /// <summary>
        /// Sets the properties from the arguments recieved through querystring.
        /// </summary>
        /// <param name="args"></param>
        private void CaptureQuerystringParameters()
        {
            if (!Request.QueryString["TenantID"].IsNullOrEmpty())
            {
                CurrentViewContext.SelectedTenantID = Convert.ToInt32(Request.QueryString["TenantID"]);
            }
            if (!Request.QueryString["ScreenMode"].IsNullOrEmpty())
            {
                CurrentViewContext.ScreenMode = Request.QueryString["ScreenMode"].ToString();
            }

        }

        private void ShowHideControls()
        {
            if (CurrentViewContext.ScreenMode.ToLower() == "assign")
            {
                fsucCmdBarButton.SubmitButton.Style.Add("display", "none");
                fsucCmdBarButton.SubmitButton.Visible = false;
            }
            else //Unassign
            {
                fsucCmdBarButton.SaveButton.Style.Add("display", "none");
                fsucCmdBarButton.SaveButton.Visible = false;
            }
        }

        #endregion

    }
}