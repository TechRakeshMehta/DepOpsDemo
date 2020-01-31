using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using Entity;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.Utils;
using Telerik.Web.UI;

namespace CoreWeb.ComplianceAdministration.Views
{
    public partial class MangeSystemEntityUserPermissions : BaseUserControl, IMangeSystemEntityUserPermissionsView
    {
        #region Private Variables

        private MangeSystemEntityUserPermissionsPresenter _presenter = new MangeSystemEntityUserPermissionsPresenter();
        private String _viewType;
        private Int32 _tenantid;
        private Boolean? _isAdminLoggedIn = null;
        private Int32 _selectedTenantId = AppConsts.NONE;

        #endregion

        #region Properties

        public MangeSystemEntityUserPermissionsPresenter Presenter
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

        public IMangeSystemEntityUserPermissionsView CurrentViewContext
        {
            get { return this; }
        }

        /// <summary>
        /// Gets and Sets list of tenants.
        /// </summary>
        List<Tenant> IMangeSystemEntityUserPermissionsView.ListTenants
        {
            set;
            get;
        }

        /// <summary>
        /// Gets and sets Logged In User TenantId
        /// </summary>
        Int32 IMangeSystemEntityUserPermissionsView.TenantId
        {
            get
            {
                if (_tenantid == 0)
                {
                    //_tenantid = Presenter.GetTenantId();
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    if (user.IsNotNull())
                    {
                        _tenantid = user.TenantId.HasValue ? user.TenantId.Value : 0;
                    }
                }
                return _tenantid;
            }
            set { _tenantid = value; }
        }

        /// <summary>
        /// Gets and sets TenantId of selected tenant
        /// </summary>
        Int32 IMangeSystemEntityUserPermissionsView.SelectedTenantId
        {
            get
            {
                if (_selectedTenantId == AppConsts.NONE)
                {
                    Int32.TryParse(cmbTenant.SelectedValue, out _selectedTenantId);
                }
                return _selectedTenantId;
            }
            set
            {
                _selectedTenantId = value;
                hdnSelectedTenantID.Value = value.ToString();
            }
        }

        /// <summary>
        /// Gets the default TenantId
        /// </summary>
        Int32 IMangeSystemEntityUserPermissionsView.DefaultTenantId
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

        Boolean IMangeSystemEntityUserPermissionsView.IsAdminLoggedIn
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

        Int32 IMangeSystemEntityUserPermissionsView.currentLoggedInUserId
        {
            get
            {
                return base.CurrentUserId;
            }
        }

        List<LkpSystemEntity> IMangeSystemEntityUserPermissionsView.SystemEntityList
        {
            get;
            set;
        }

        List<SystemEntityUserPermissionData> IMangeSystemEntityUserPermissionsView.SystemEntityUserPermissionList
        {
            get;
            set;
        }

        Int32 IMangeSystemEntityUserPermissionsView.SelectedEntityId
        {
            get
            {
                if (cmbEntityType.SelectedValue == String.Empty)
                {
                    return 0;
                }
                return Convert.ToInt32(cmbEntityType.SelectedValue);
            }
            set
            {
                cmbEntityType.SelectedValue = Convert.ToString(value);
            }
        }

        List<OrgUser> IMangeSystemEntityUserPermissionsView.UsersApplicableForAssigningPermission
        {
            get;
            set;
        }
        List<SystemEntityPermission> IMangeSystemEntityUserPermissionsView.PermissionList
        {
            get;
            set;
        }

        Int32 IMangeSystemEntityUserPermissionsView.EntityPermissionId
        {
            get;
            set;
        }

        Int32 IMangeSystemEntityUserPermissionsView.CurrentOrganisationUserId
        {
            get;
            set;
        }

        Int32 IMangeSystemEntityUserPermissionsView.SEUP_ID
        {
            get;
            set;
        }

        string IMangeSystemEntityUserPermissionsView.UserFirstName
        {
            get
            {
                return txtFirstName.Text;
            }
            set
            {
                txtFirstName.Text = value;
            }
        }

        String IMangeSystemEntityUserPermissionsView.UserLastName
        {
            get
            {
                return txtLastName.Text;
            }
            set
            {
                txtLastName.Text = value;
            }
        }

        String IMangeSystemEntityUserPermissionsView.EmailAddress
        {
            get;
            set;
        }

        public int CurrentPageIndex
        {
            get
            {
                return grdUserPermissions.MasterTableView.CurrentPageIndex + 1;
            }
            set
            {

                grdUserPermissions.MasterTableView.CurrentPageIndex = value - 1;

            }
        }

        public int PageSize
        {
            get
            {
                // Maximum 100 record allowed from DB. 
                //return grdApplicantComprehensiveSearchData.PageSize > 100 ? 100 : grdApplicantComprehensiveSearchData.PageSize;
                return grdUserPermissions.PageSize;
            }
            set
            {
                grdUserPermissions.PageSize = value;
            }
        }

        public int VirtualRecordCount
        {
            get
            {
                if (ViewState["ItemCount"] != null)
                    return Convert.ToInt32(ViewState["ItemCount"]);
                else
                    return 0;
            }
            set
            {
                ViewState["ItemCount"] = value;
                grdUserPermissions.VirtualItemCount = value;
                grdUserPermissions.MasterTableView.VirtualItemCount = value;
            }
        }

        public CustomPagingArgsContract GridCustomPaging
        {
            get
            {
                if (ViewState["GridCustomPaging"] == null)
                {
                    ViewState["GridCustomPaging"] = new CustomPagingArgsContract();
                }
                return (CustomPagingArgsContract)ViewState["GridCustomPaging"];
            }
            set
            {
                ViewState["GridCustomPaging"] = value;
                VirtualRecordCount = value.VirtualPageCount;
                PageSize = value.PageSize;
                CurrentPageIndex = value.CurrentPageIndex;
            }
        }

        public Dictionary<int, bool> LstSelectedBkgOdrResPermissions
        {
            get;
            set;
        }

        public List<Int32> LstEntityPermissionIds { get; set; }

        Int32? IMangeSystemEntityUserPermissionsView.SelectedHierarchyId
        {
            get
            {
                if (!String.IsNullOrEmpty(hdnDepartmentProgmapNew.Value))
                {
                    return Convert.ToInt32( hdnDepartmentProgmapNew.Value);
                }
                return null;
            }
            set
            {
                hdnDepartmentProgmapNew.Value = Convert.ToString(value);
            }
        }

        #endregion

        #region Events

        #region PageEvents
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                base.Title = "Manage System Entity User Permissions";
                base.SetPageTitle("Manage System Entity User Permissions");
                CmdBarSearch.SubmitButton.CausesValidation = false;
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    Presenter.OnViewInitialized();
                    BindTenant();
                    BindEntityType();
                }
                (CmdBarSearch as CoreWeb.Shell.Views.CommandBar).SaveButton.ToolTip = "Click to search orders per the criteria entered above";
                (CmdBarSearch as CoreWeb.Shell.Views.CommandBar).SubmitButton.ToolTip = "Click to remove all values entered in the search criteria above";
                (CmdBarSearch as CoreWeb.Shell.Views.CommandBar).CancelButton.ToolTip = "Click to cancel. Any data entered will not be saved";
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdUserPermissions_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                GridCustomPaging.CurrentPageIndex = CurrentPageIndex;
                GridCustomPaging.PageSize = PageSize;
                CurrentViewContext.GridCustomPaging = GridCustomPaging;
                Presenter.GetSystemEntityUserPermissionList();
                grdUserPermissions.DataSource = CurrentViewContext.SystemEntityUserPermissionList;

                //Tenant not selected,grid is hidden 
                if (CurrentViewContext.SelectedTenantId < AppConsts.NONE || CurrentViewContext.SelectedTenantId == AppConsts.NONE
                    || CurrentViewContext.SelectedEntityId < AppConsts.NONE || CurrentViewContext.SelectedEntityId == AppConsts.NONE)
                {
                    grdUserPermissions.Visible = false;
                    grdUserPermissions.MasterTableView.IsItemInserted = false;
                    grdUserPermissions.MasterTableView.ClearEditItems();
                }
                else
                {
                    hdnSelectedTenantID.Value = CurrentViewContext.SelectedTenantId.ToString();
                    grdUserPermissions.Visible = true;
                }
                Presenter.GetSystemEntities();
                Int32 bkgOrderResultEntityID = CurrentViewContext.SystemEntityList.Where(cond => cond.SE_CODE == EnumSystemEntity.BKG_ORDER_RESULT_REPORT.GetStringValue()).Select(sel => sel.SE_ID).FirstOrDefault();
                if (CurrentViewContext.SelectedEntityId != bkgOrderResultEntityID)
                    grdUserPermissions.MasterTableView.GetColumn("HierarchyNodeLabel").Visible = false;
                else
                    grdUserPermissions.MasterTableView.GetColumn("HierarchyNodeLabel").Visible = true;
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdUserPermissions_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == RadGrid.UpdateCommandName || e.CommandName == RadGrid.PerformInsertCommandName)
                {

                    if (e.CommandName == RadGrid.UpdateCommandName)
                        CurrentViewContext.SEUP_ID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["SEUP_ID"]);

                    WclComboBox ddlUserlistName = e.Item.FindControl("cmbUsers") as WclComboBox;
                    CurrentViewContext.CurrentOrganisationUserId = Convert.ToInt32(ddlUserlistName.SelectedValue);


                    if (IsBackgroundOrderResultReportEntitySelected())
                    {
                        if (hdnDepartmentProgmapNew.Value == AppConsts.ZERO || hdnDepartmentProgmapNew.Value == String.Empty)
                        {
                            e.Canceled = true;
                            (e.Item.FindControl("lblInstitutionHierarchyPB") as Label).Text = String.Empty;
                            (e.Item.FindControl("lblName1") as Label).ShowMessage("Please select a hierarchy node.", MessageType.Error);
                            return;
                        }
                        if (e.CommandName == RadGrid.PerformInsertCommandName)
                        {
                            if (Presenter.IsBackgroundOrderResultUserPermissionExists())
                            {
                                e.Canceled = true;
                                //(e.Item.FindControl("lblInstitutionHierarchyPB") as Label).Text = String.Empty;
                                (e.Item.FindControl("lblName1") as Label).ShowMessage("User Permission already exists for selected node.", MessageType.Error);
                                hdnDepartmentProgmapNew.Value = String.Empty;
                                return;
                            }
                        }

                        WclComboBox cmbPermissionList = e.Item.FindControl("cmbPermissionList") as WclComboBox;
                        LstSelectedBkgOdrResPermissions = new Dictionary<int, bool>();

                        var noneOption = CurrentViewContext.PermissionList.Where(cond => cond.SEP_PermissionCode == EnumSystemPermissionCode.NONE.GetStringValue()).FirstOrDefault();
                        int nonePermissionId = noneOption.IsNotNull() ? noneOption.SEP_ID : 0;

                        if (cmbPermissionList.FindItemByValue(nonePermissionId.ToString()).Checked)
                        {
                            for (Int32 i = 0; i < cmbPermissionList.Items.Count; i++)
                            {
                                if (cmbPermissionList.Items[i].Value == nonePermissionId.ToString())
                                    LstSelectedBkgOdrResPermissions.Add(Convert.ToInt32(cmbPermissionList.Items[i].Value), true);
                                else
                                    LstSelectedBkgOdrResPermissions.Add(Convert.ToInt32(cmbPermissionList.Items[i].Value), false);
                            }
                        }
                        else
                        {
                            for (Int32 i = 0; i < cmbPermissionList.Items.Count; i++)
                            {
                                LstSelectedBkgOdrResPermissions.Add(Convert.ToInt32(cmbPermissionList.Items[i].Value), cmbPermissionList.Items[i].Checked);
                            }
                        }
                    }
                    else if (IsStudentBucketAssignmentEntitySelected())
                    {
                        WclComboBox cmbPermissionList = e.Item.FindControl("cmbPermissionList") as WclComboBox;
                        LstSelectedBkgOdrResPermissions = new Dictionary<int, bool>();

                        var noneOption = CurrentViewContext.PermissionList.Where(cond => cond.SEP_PermissionCode == EnumSystemPermissionCode.NONPER.GetStringValue()).FirstOrDefault();
                        int nonePermissionId = noneOption.IsNotNull() ? noneOption.SEP_ID : 0;

                        if (cmbPermissionList.FindItemByValue(nonePermissionId.ToString()).Checked)
                        {
                            for (Int32 i = 0; i < cmbPermissionList.Items.Count; i++)
                            {
                                if (cmbPermissionList.Items[i].Value == nonePermissionId.ToString())
                                    LstSelectedBkgOdrResPermissions.Add(Convert.ToInt32(cmbPermissionList.Items[i].Value), true);
                                else
                                    LstSelectedBkgOdrResPermissions.Add(Convert.ToInt32(cmbPermissionList.Items[i].Value), false);
                            }
                        }
                        else
                        {
                            for (Int32 i = 0; i < cmbPermissionList.Items.Count; i++)
                            {
                                LstSelectedBkgOdrResPermissions.Add(Convert.ToInt32(cmbPermissionList.Items[i].Value), cmbPermissionList.Items[i].Checked);
                            }
                        }
                    }
                    else if (IsNotificationEntitySelected())
                    {
                        WclComboBox cmbPermissionList = e.Item.FindControl("cmbPermissionList") as WclComboBox;
                        LstSelectedBkgOdrResPermissions = new Dictionary<int, bool>();

                        var noneOption = CurrentViewContext.PermissionList.Where(cond => cond.SEP_PermissionCode == EnumSystemPermissionCode.NONPER.GetStringValue()).FirstOrDefault();
                        int nonePermissionId = noneOption.IsNotNull() ? noneOption.SEP_ID : 0;

                        if (cmbPermissionList.FindItemByValue(nonePermissionId.ToString()).Checked)
                        {
                            for (Int32 i = 0; i < cmbPermissionList.Items.Count; i++)
                            {
                                if (cmbPermissionList.Items[i].Value == nonePermissionId.ToString())
                                    LstSelectedBkgOdrResPermissions.Add(Convert.ToInt32(cmbPermissionList.Items[i].Value), true);
                                else
                                    LstSelectedBkgOdrResPermissions.Add(Convert.ToInt32(cmbPermissionList.Items[i].Value), false);
                            }
                        }
                        else
                        {
                            for (Int32 i = 0; i < cmbPermissionList.Items.Count; i++)
                            {
                                LstSelectedBkgOdrResPermissions.Add(Convert.ToInt32(cmbPermissionList.Items[i].Value), cmbPermissionList.Items[i].Checked);
                            }
                        }
                    }                  
                    else
                    {
                        RadioButtonList permissionType = e.Item.FindControl("rblPermissionList") as RadioButtonList;
                        CurrentViewContext.EntityPermissionId = Convert.ToInt32(permissionType.SelectedValue);
                    }


                    Presenter.SaveUpdateEntityUserPermission();
                    if (e.CommandName == RadGrid.PerformInsertCommandName)
                    {
                        base.ShowSuccessMessage("User Permission saved successfully");
                    }
                    else
                    {
                        base.ShowSuccessMessage("User Permission updated successfully");
                    }
                }

                if (e.CommandName == RadGrid.DeleteCommandName)
                {
                    CurrentViewContext.CurrentOrganisationUserId = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrganizationUserId"]);
                    CurrentViewContext.SelectedHierarchyId = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["HierarchyNodeID"]);
                    if (IsBackgroundOrderResultReportEntitySelected() || IsStudentBucketAssignmentEntitySelected() || IsNotificationEntitySelected())
                    {
                        string entityPermissionIds = ((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["EntityPermissionId"]).ToString();
                        LstEntityPermissionIds = new List<int>();
                        foreach (string entityPermission in entityPermissionIds.Split(','))
                        {
                            LstEntityPermissionIds.Add(Convert.ToInt32(entityPermission.Trim()));
                        }
                    }

                    CurrentViewContext.SEUP_ID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["SEUP_ID"]);

                    if (Presenter.DeleteEntityUserPermission())
                    {
                        base.ShowSuccessMessage("User Permission deleted successfully");
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdUserList_ItemCreated(object sender, GridItemEventArgs e)
        {
            try
            {
                if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
                {
                    GridEditFormItem editform = (e.Item as GridEditFormItem);
                    WclComboBox ddlUserlistName = editform.FindControl("cmbUsers") as WclComboBox;

                    RadioButtonList permissionType = editform.FindControl("rblPermissionList") as RadioButtonList;
                    RequiredFieldValidator rfvPermissionList = editform.FindControl("rfvPermissionList") as RequiredFieldValidator;
                    RequiredFieldValidator rfvCmbPermissionList = editform.FindControl("rfvCmbPermissionList") as RequiredFieldValidator;

                    HiddenField InstHierarchyLabel = editform.FindControl("hdnInstHierarchyLabel") as HiddenField;

                    HtmlGenericControl dvInstHierarchy = (HtmlGenericControl)editform.FindControl("dvInstHierarchy");

                    WclComboBox cmbPermissionList = editform.FindControl("cmbPermissionList") as WclComboBox;

                    Presenter.GetPermissionByEntityId();

                    bool isBackgroundOrderResultReportEntitySelected = IsBackgroundOrderResultReportEntitySelected();

                    if (isBackgroundOrderResultReportEntitySelected || IsStudentBucketAssignmentEntitySelected() || IsNotificationEntitySelected())
                    {
                        if (isBackgroundOrderResultReportEntitySelected)
                        {
                            String prmsNone = EnumSystemPermissionCode.NONE.GetStringValue();
                            var noneOption = CurrentViewContext.PermissionList.Where(cond => cond.SEP_PermissionCode == prmsNone).FirstOrDefault();
                            int nonePermissionId = noneOption.IsNotNull() ? noneOption.SEP_ID : 0;
                            hdnNoneOptionValue.Value = nonePermissionId.ToString();
                        }
                        else if (IsStudentBucketAssignmentEntitySelected())
                        {
                            String prmsNone = EnumSystemPermissionCode.NONPER.GetStringValue();
                            var noneOption = CurrentViewContext.PermissionList.Where(cond => cond.SEP_PermissionCode == prmsNone).FirstOrDefault();
                            int nonePermissionId = noneOption.IsNotNull() ? noneOption.SEP_ID : 0;
                            hdnNoneOptionValue.Value = nonePermissionId.ToString();
                        }
                        else if (IsNotificationEntitySelected())
                        {
                            String prmsNone = EnumSystemPermissionCode.NONPER.GetStringValue();
                            var noneOption = CurrentViewContext.PermissionList.Where(cond => cond.SEP_PermissionCode == prmsNone).FirstOrDefault();
                            int nonePermissionId = noneOption.IsNotNull() ? noneOption.SEP_ID : 0;
                            hdnNoneOptionValue.Value = nonePermissionId.ToString();
                        }
                        permissionType.Visible = false;
                        rfvPermissionList.Enabled = false;
                        cmbPermissionList.DataSource = CurrentViewContext.PermissionList;
                        cmbPermissionList.DataTextField = "SEP_PermissionName";
                        cmbPermissionList.DataValueField = "SEP_ID";
                        cmbPermissionList.DataBind();
                    }
                    else
                    {
                        cmbPermissionList.Visible = false;
                        rfvCmbPermissionList.Enabled = false;
                        permissionType.DataSource = CurrentViewContext.PermissionList;
                        permissionType.DataTextField = "SEP_PermissionName";
                        permissionType.DataValueField = "SEP_ID";
                        permissionType.DataBind();
                    }

                    var userPermission = (e.Item).DataItem as SystemEntityUserPermissionData;
                    if (userPermission != null)
                    {
                        Presenter.GetOrgUserListForAsigningPermission(userPermission.OrganizationUserId);
                        ddlUserlistName.Items.Clear();
                        ddlUserlistName.DataSource = CurrentViewContext.UsersApplicableForAssigningPermission;
                        ddlUserlistName.DataBind();
                        ddlUserlistName.SelectedValue = userPermission.OrganizationUserId.ToString();
                        ddlUserlistName.Enabled = false;

                        if (!isBackgroundOrderResultReportEntitySelected && !IsStudentBucketAssignmentEntitySelected() && !IsNotificationEntitySelected())
                        {
                            permissionType.SelectedValue = Convert.ToString(userPermission.EntityPermissionId);
                        }
                    }
                    else
                    {
                        Presenter.GetOrgUserListForAsigningPermission();
                        ddlUserlistName.Items.Clear();
                        ddlUserlistName.DataSource = CurrentViewContext.UsersApplicableForAssigningPermission;
                        ddlUserlistName.DataBind();
                    }
                    //UAT 4522 
                    if (isBackgroundOrderResultReportEntitySelected)
                    {
                        dvInstHierarchy.Visible = true;
                    }
                    //Dictionary<Int32, String> dicDefaultNode = Presenter.GetDefaultPermissionForClientAdmin();
                    //Label lblInstitutionHierarchyPB = editform.FindControl("lblInstitutionHierarchyPB") as Label;
                    //if (!dicDefaultNode.IsNullOrEmpty())
                    //{
                    //    hdnDepartmentProgmapNew.Value = Convert.ToString(dicDefaultNode.Keys.FirstOrDefault());
                    //    hdnInstNodeIdNew.Value = Convert.ToString(dicDefaultNode.Keys.FirstOrDefault());
                    //    hdnInstNodeLabel.Value = dicDefaultNode.Values.FirstOrDefault();
                    //    lblInstitutionHierarchyPB.Text = dicDefaultNode.Values.FirstOrDefault();

                    //}
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

        private bool IsBackgroundOrderResultReportEntitySelected()
        {
            bool isEntitySelected = false;

            if (CurrentViewContext.SystemEntityList.IsNull())
                Presenter.GetSystemEntities();

            if (CurrentViewContext.SystemEntityList != null)
            {
                var selectedEntity = CurrentViewContext.SystemEntityList.Where(cond => cond.SE_ID == CurrentViewContext.SelectedEntityId).SingleOrDefault();
                if (selectedEntity != null && string.Compare(selectedEntity.SE_CODE, EnumSystemEntity.BKG_ORDER_RESULT_REPORT.GetStringValue()) == 0)
                {
                    isEntitySelected = true;
                }
            }
            return isEntitySelected;
        }

        /// <summary>
        /// UAT-2056
        /// </summary>
        /// <returns></returns>
        private bool IsStudentBucketAssignmentEntitySelected()
        {
            bool isEntitySelected = false;

            if (CurrentViewContext.SystemEntityList.IsNull())
                Presenter.GetSystemEntities();

            if (!CurrentViewContext.SystemEntityList.IsNullOrEmpty())
            {
                var selectedEntity = CurrentViewContext.SystemEntityList.Where(cond => cond.SE_ID == CurrentViewContext.SelectedEntityId).SingleOrDefault();
                if (selectedEntity != null && string.Compare(selectedEntity.SE_CODE, EnumSystemEntity.STUDENT_BUCKET_ASSIGNMENT.GetStringValue()) == 0)
                {
                    isEntitySelected = true;
                }
            }
            return isEntitySelected;
        }

        /// <summary>
        /// UAT-3364
        /// </summary>
        /// <returns></returns>
        private bool IsNotificationEntitySelected()
        {
            bool isEntitySelected = false;

            if (CurrentViewContext.SystemEntityList.IsNull())
                Presenter.GetSystemEntities();

            if (!CurrentViewContext.SystemEntityList.IsNullOrEmpty())
            {
                var selectedEntity = CurrentViewContext.SystemEntityList.Where(cond => cond.SE_ID == CurrentViewContext.SelectedEntityId).SingleOrDefault();
                if (selectedEntity != null && string.Compare(selectedEntity.SE_CODE, EnumSystemEntity.ROTATION_NOTIFICATION.GetStringValue()) == 0)
                {
                    isEntitySelected = true;
                }
            }
            return isEntitySelected;
        }


        /// <summary>
        /// Grid sort expression
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdUserList_SortCommand(object sender, GridSortCommandEventArgs e)
        {
            try
            {
                if (e.NewSortOrder != GridSortOrder.None)
                {
                    GridCustomPaging.SortExpression = e.SortExpression;
                    GridCustomPaging.SortDirectionDescending = e.NewSortOrder.Equals(GridSortOrder.Descending);
                    CurrentViewContext.GridCustomPaging.SortExpression = e.SortExpression;
                    CurrentViewContext.GridCustomPaging.SortDirectionDescending = e.NewSortOrder.Equals(GridSortOrder.Descending);
                }
                else
                {
                    GridCustomPaging.SortExpression = String.Empty;
                    GridCustomPaging.SortDirectionDescending = false;
                    CurrentViewContext.GridCustomPaging.SortExpression = String.Empty;
                    CurrentViewContext.GridCustomPaging.SortDirectionDescending = false;
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
        /// Grid Item databound opeartion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdUserPermissions_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
                {
                    hdnDepartmentProgmapNew.Value = String.Empty;
                    //Edit Mode
                    if (!(e.Item is GridEditFormInsertItem || e.Item is GridDataInsertItem))
                    {
                        bool isBackgroundOrderResultReportEntitySelected = IsBackgroundOrderResultReportEntitySelected();
                        //UAT4522
                        if (isBackgroundOrderResultReportEntitySelected)
                        {
                            HtmlAnchor anchorInstitutionHierarchyPB = e.Item.FindControl("lnkInstitutionHierarchyPB") as HtmlAnchor;
                            anchorInstitutionHierarchyPB.Attributes["Class"] = "disabled";
                            anchorInstitutionHierarchyPB.Style.Value = "color: grey; cursor :default !Important;";
                            HtmlGenericControl dvInstHierarchy = e.Item.FindControl("dvInstHierarchy") as HtmlGenericControl;
                            dvInstHierarchy.Attributes["title"] = "";
                            var userPermission = (e.Item).DataItem as SystemEntityUserPermissionData;
                            if (!userPermission.HierarchyNodeLabel.IsNullOrEmpty())
                            {
                                Label lblInstitutionHierarchyPB = e.Item.FindControl("lblInstitutionHierarchyPB") as Label;
                                lblInstitutionHierarchyPB.Text = userPermission.HierarchyNodeLabel.HtmlEncode();
                                hdnDepartmentProgmapNew.Value = userPermission.HierarchyNodeId.ToString();
                            }
                        }
                        if (isBackgroundOrderResultReportEntitySelected || IsStudentBucketAssignmentEntitySelected() || IsNotificationEntitySelected())
                        {
                            SystemEntityPermission noneOption;
                            int nonePermissionId = 0;
                            if (isBackgroundOrderResultReportEntitySelected)
                            {
                                noneOption = CurrentViewContext.PermissionList.Where(cond => cond.SEP_PermissionCode == EnumSystemPermissionCode.NONE.GetStringValue()).FirstOrDefault();
                                nonePermissionId = noneOption.IsNotNull() ? noneOption.SEP_ID : 0;
                            }
                            else if (IsStudentBucketAssignmentEntitySelected())
                            {
                                noneOption = CurrentViewContext.PermissionList.Where(cond => cond.SEP_PermissionCode == EnumSystemPermissionCode.NONPER.GetStringValue()).FirstOrDefault();
                                nonePermissionId = noneOption.IsNotNull() ? noneOption.SEP_ID : 0;
                            }
                            else if (IsNotificationEntitySelected())
                            {
                                noneOption = CurrentViewContext.PermissionList.Where(cond => cond.SEP_PermissionCode == EnumSystemPermissionCode.NONPER.GetStringValue()).FirstOrDefault();
                                nonePermissionId = noneOption.IsNotNull() ? noneOption.SEP_ID : 0;
                            }

                            bool isNonePermissionSelected = false;

                            WclComboBox cmbPermissionList = e.Item.FindControl("cmbPermissionList") as WclComboBox;
                            var userPermission = (e.Item).DataItem as SystemEntityUserPermissionData;

                            foreach (string permissionID in userPermission.EntityPermissionId.Split(','))
                            {
                                if (string.Compare(permissionID.Trim(), nonePermissionId.ToString()) == 0)
                                {
                                    isNonePermissionSelected = true;
                                }

                                if (cmbPermissionList.Items.FindItemByValue(permissionID.Trim()).IsNotNull())
                                {
                                    cmbPermissionList.Items.FindItemByValue(permissionID.Trim()).Checked = true;
                                }
                                //EnumSystemPermissionCode
                            }

                            if (isNonePermissionSelected)
                            {
                                foreach (RadComboBoxItem item in cmbPermissionList.Items)
                                {
                                    if (item.Value != nonePermissionId.ToString())
                                    {
                                        item.Enabled = false;
                                        item.Checked = false;
                                    }
                                    else
                                    {
                                        item.Enabled = true;
                                        item.Checked = true;
                                    }
                                }
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

        #region DropDown Events
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbEntityType_DataBound(object sender, EventArgs e)
        {
            try
            {
                cmbEntityType.Items.Insert(0, new RadComboBoxItem("--Select--"));
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbTenant_DataBound(object sender, EventArgs e)
        {
            try
            {
                cmbTenant.Items.Insert(0, new RadComboBoxItem("--Select--"));
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
        /// <summary>
        /// To perform search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarSearch_SearchClick(object sender, EventArgs e)
        {
            try
            {
                grdUserPermissions.MasterTableView.IsItemInserted = false;
                grdUserPermissions.MasterTableView.ClearEditItems();
                ResetGridFilters();
                //To reset grid filters 
                if (grdUserPermissions.Items.Count > 0)
                {
                    CmdBarSearch.ClearButton.Style.Clear();
                    CmdBarSearch.ExtraButton.Style.Clear();
                }
                else
                {
                    CmdBarSearch.ClearButton.Style.Add("display", "none");
                    CmdBarSearch.ExtraButton.Style.Add("display", "none");
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
        /// To reset controls
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarSearch_ResetClick(object sender, EventArgs e)
        {
            try
            {
                BindTenant();
                BindEntityType();
                cmbTenant.SelectedValue = String.Empty;
                cmbTenant.SelectedValue = String.Empty;
                hdnDepartmentProgmapNew.Value = String.Empty;
                hdnSelectedTenantID.Value = String.Empty;
                hdnInstitutionHierarchyPBLbl.Value = String.Empty;
                ResetGridFilters();
                Session[AppConsts.APPLICANT_SEARCH_SESSION_KEY] = null;

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
        /// Redirect to Home page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarSearch_CancelClick(object sender, EventArgs e)
        {
            try
            {
                //Reset session
                Session[AppConsts.APPLICANT_SEARCH_SESSION_KEY] = null;
                Dictionary<String, String> queryString = new Dictionary<String, String>();
                Response.Redirect(String.Format(AppConsts.DASHBOARD_PAGE_NAME), true);
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


        #region Private Methods
        private void BindTenant()
        {
            if (CurrentViewContext.IsAdminLoggedIn == true)
            {
                Presenter.GetTenants();
                cmbTenant.DataSource = CurrentViewContext.ListTenants;
                cmbTenant.DataBind();
            }
            else
            {
                pnlTenant.Visible = false;
            }
        }

        public void BindEntityType()
        {
            Presenter.GetSystemEntities();
            cmbEntityType.DataSource = CurrentViewContext.SystemEntityList;
            cmbEntityType.DataBind();
        }
    

        /// <summary>
        /// Removes all the filters and Sorting on the grid and clears the variables.
        /// </summary>
        private void ResetGridFilters()
        {
            grdUserPermissions.MasterTableView.SortExpressions.Clear();
            grdUserPermissions.CurrentPageIndex = 0;
            grdUserPermissions.MasterTableView.CurrentPageIndex = 0;
            grdUserPermissions.Rebind();
        }
        #endregion

    }
}