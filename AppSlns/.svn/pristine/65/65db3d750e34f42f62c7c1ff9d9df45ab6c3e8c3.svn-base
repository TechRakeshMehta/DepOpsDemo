using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using INTSOF.UI.Contract.BkgSetup;
using INTSOF.Utils;
using Telerik.Web.UI;

namespace CoreWeb.BkgSetup.Views
{
    public partial class MapServicesToClient : BaseUserControl, IMapServicesToClientView
    {
        #region VARIABLES

        #region PUBLIC VARIABLES

        #endregion

        #region PRIVATE VARIABLES
        private MapServicesToClientPresenter _presenter = new MapServicesToClientPresenter();
        private Boolean? _isAdminLoggedIn = null;
        private Int32 _tenantid;
        #endregion

        #endregion

        #region PRESENTER

        public MapServicesToClientPresenter Presenter
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
        #endregion

        #region PAGE METHODS
        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.Title = "Map Services To Client";
                base.OnInit(e);
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
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
                ApplyActionLevelPermission(ActionCollection, "Map Services To Client");
                BindTenant();
                grdService.Visible = false;
                //Start UAT-3157
                if (IsAdminLoggedIn)
                {
                    GetPreferredSelectedTenant();
                }
                //END UAT 3157
                if (TenantId != DefaultTenantId)
                {
                    SelectedTenantId = TenantId;
                    Presenter.GetAlreadyMappedServices();
                    grdService.Visible = true;
                }
            }
            base.SetPageTitle("Map Services To Client");
        }
        #endregion

        #region EVENTS

        #region DROPDOWN EVENTS

        protected void ddlTenant_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!ddlTenant.SelectedValue.IsNullOrEmpty() && Convert.ToInt32(ddlTenant.SelectedValue) != AppConsts.NONE)
                {
                    SelectedTenantId = Convert.ToInt32(ddlTenant.SelectedValue);
                    lblInstitution.Visible = false;
                }
                else
                {
                    txtSvcIDs.Text = String.Empty;
                    txtName.Text = String.Empty;
                    txtCode.Text = String.Empty;
                    ddlTenant.SelectedIndex = AppConsts.NONE;
                    grdService.DataSource = new List<MapServicesToClientContract>();
                    grdService.DataBind();
                    //lblInstitution.Visible = true;
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

        #region BUTTON EVENTS

        /// <summary>
        /// Click Event of the CommandBar Submit Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucCmdBar1_SubmitClick(object sender, EventArgs e)
        {
            try
            {
                if (hdnRefreshIds.Value == "true")
                {
                    hdnBSE_ID.Value = "";
                }
                SelectedTenantId = ddlTenant.SelectedValue != String.Empty ? Convert.ToInt32(ddlTenant.SelectedValue) : TenantId;
                SelectedServices = hdnBSE_ID.Value;
                if (ddlTenant.SelectedValue != String.Empty)
                {
                    if (!SelectedServices.IsNullOrEmpty())
                    {
                        Presenter.SaveMappings();
                    }
                    if (IsServicesMapped)
                    {
                        foreach (GridDataItem item in grdService.Items)
                        {
                            CheckBox chkIsServicesMapped = (CheckBox)item["AlreadyMappedServices"].FindControl("chkSelectItem");
                            if (chkIsServicesMapped.Checked)
                            {
                                RadButton btnDeactivate = (RadButton)item["DeactivateMappings"].FindControl("btnDeactivate");
                                btnDeactivate.Visible = true;
                                chkIsServicesMapped.Enabled = false;
                            }
                        }
                        Int32[] oldIds = ViewState["PreMappedServicesIds"] as Int32[];
                        String[] temp = SelectedServices.Split(',');
                        Int32[] newIds = Array.ConvertAll(temp, int.Parse);
                        ViewState["PreMappedServicesIds"] = oldIds.Concat(newIds).ToArray();
                        hdnRefreshIds.Value = "true";
                        ShowSuccessMessage("Services mapped successfully.");
                    }
                    else
                    {
                        if (SelectedServices.IsNullOrEmpty())
                        {
                            ShowErrorMessage("Please select atleast one service.");
                        }
                        else
                        {
                            ShowErrorMessage("Services mapping failed.");
                        }
                    }
                }
                else
                {
                    ShowErrorMessage("Please select the institution.");
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

        #region GRID EVENTS

        protected void grdService_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                if (ddlTenant.SelectedValue != "0")
                {
                    lblInstitution.Visible = false;
                    Int32? svcID = txtSvcIDs.Text == String.Empty ? (Int32?)null : Convert.ToInt32(txtSvcIDs.Text);
                    String svcName = txtName.Text == String.Empty ? null : txtName.Text;
                    String extCode = txtCode.Text == String.Empty ? null : txtCode.Text;
                    Presenter.GetServices(svcID, svcName, extCode);
                    grdService.DataSource = BackgroundServicesList;
                }
                else
                {
                    lblInstitution.Visible = true;
                    txtSvcIDs.Text = String.Empty;
                    txtName.Text = String.Empty;
                    txtCode.Text = String.Empty;
                    ddlTenant.SelectedIndex = AppConsts.NONE;
                    grdService.DataSource = new List<MapServicesToClientContract>();
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

        protected void grdService_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            try
            {
                //Get the list of Background Services from ViewState
                Int32[] PreMappedServiceIds = ViewState["PreMappedServicesIds"] as Int32[];
                GridDataItem dataItem = e.Item as GridDataItem;
                if (dataItem != null && PreMappedServiceIds != null)
                {
                    foreach (var item in PreMappedServiceIds)
                    {
                        if (Convert.ToInt32(dataItem.GetDataKeyValue("BSE_ID")) == item)
                        {
                            //Display the deactivate buttons for premapped services
                            CheckBox chkIsServicesMapped = (CheckBox)dataItem.FindControl("chkSelectItem");
                            RadButton btnDeactivate = (RadButton)dataItem.FindControl("btnDeactivate");
                            chkIsServicesMapped.Checked = true;
                            chkIsServicesMapped.Enabled = false;
                            btnDeactivate.Visible = true;
                        }
                    }
                }

                //to maintain checkboxes selection throughout grid.
                if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
                {
                    String[] checkedIDs = hdnBSE_ID.Value.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    if (checkedIDs.IsNotNull())
                    {
                        String bseID = Convert.ToString((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["BSE_ID"]);
                        if (!String.IsNullOrEmpty(bseID))
                        {
                            if (checkedIDs.Any(cond => cond == bseID))
                            {
                                CheckBox checkBox = ((CheckBox)e.Item.FindControl("chkSelectItem"));
                                checkBox.Checked = true;
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

        protected void grdService_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                SelectedTenantId = ddlTenant.SelectedValue != String.Empty ? Convert.ToInt32(ddlTenant.SelectedValue) : TenantId;
                if (e.CommandName == "DeactivateMapping")
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    DeactivateServiceId = Convert.ToInt32(dataItem["BSE_ID"].Text);
                    Presenter.DeactivateMapping();
                    if (IsServiceDeactivated)
                    {
                        CheckBox chkIsServicesMapped = (CheckBox)dataItem.FindControl("chkSelectItem");
                        RadButton btnDeactivate = (RadButton)dataItem.FindControl("btnDeactivate");
                        chkIsServicesMapped.Checked = false;
                        chkIsServicesMapped.Enabled = true;
                        btnDeactivate.Visible = false;
                        ShowSuccessMessage("Services Deactivated Successfully.");
                        Int32[] premappedIds = ViewState["PreMappedServicesIds"] as Int32[];
                        ViewState["PreMappedServicesIds"] = premappedIds.Where(val => val != DeactivateServiceId).ToArray();
                    }
                    else
                    {
                        ShowErrorMessage("Services Deactivated Failed.");
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

        #region METHODS
        /// <summary>
        /// Method for binding Tenant Dropdown
        /// </summary>
        private void BindTenant()
        {
            try
            {
                if (IsAdminLoggedIn == true)
                {
                    Presenter.GetTenants();
                    ddlTenant.DataSource = ListTenants;
                    ddlTenant.DataBind();
                }
                else
                {
                    pnlTenant.Visible = false;
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

        #region UAT-3157:- Sticky institution

        private void GetPreferredSelectedTenant()
        {
            if (SelectedTenantId.IsNullOrEmpty() || SelectedTenantId == AppConsts.NONE)
            {
                // Presenter.GetPreferredSelectedTenant();
                PreferredSelectedTenantID = (Session["PreferredSelectedTenant"]).IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(Session["PreferredSelectedTenant"]);
                if (PreferredSelectedTenantID > AppConsts.NONE)
                {
                    ddlTenant.SelectedValue = Convert.ToString(PreferredSelectedTenantID);
                    SelectedTenantId = Convert.ToInt32(ddlTenant.SelectedValue);

                    lblInstitution.Visible = false;
                    Presenter.GetAlreadyMappedServices(); //Get premapped services list.
                    rfvTenant.Enabled = false;
                    // grdService.Rebind();
                    grdService.Visible = true;
                    fsucCmdBar1.Visible = true;
                    ApplyActionLevelPermission(ActionCollection, "Map Services To Client");
                    Int32? svcID = txtSvcIDs.Text == String.Empty ? (Int32?)null : Convert.ToInt32(txtSvcIDs.Text);
                    String svcName = txtName.Text == String.Empty ? null : txtName.Text;
                    String extCode = txtCode.Text == String.Empty ? null : txtCode.Text;
                    Presenter.GetServices(svcID, svcName, extCode);
                    grdService.DataSource = BackgroundServicesList;
                    grdService.DataBind();
                }
            }
        }
        #endregion
        #endregion

        #region PROPERTIES

        /// <summary>
        /// Gets and sets Logged In User TenantId
        /// </summary>
        public Int32 TenantId
        {
            get
            {
                if (_tenantid == 0)
                {
                    _tenantid = Presenter.GetTenantId();
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

        public int CurrentLoggedInUserId
        {
            get
            {
                return base.CurrentUserId;
            }
        }

        public List<Entity.ClientEntity.Tenant> ListTenants
        {
            get;
            set;
        }

        public List<Entity.ClientEntity.BackgroundService> ListServices
        {
            get;
            set;
        }

        public int DefaultTenantId
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

        public Boolean IsAdminLoggedIn
        {
            get
            {
                if (_isAdminLoggedIn.IsNull())
                {
                    Presenter.IsAdminLoggedIn();
                }
                return _isAdminLoggedIn.HasValue ? _isAdminLoggedIn.Value : true;
            }
            set
            {
                _isAdminLoggedIn = value;
            }
        }

        List<Entity.ClientEntity.BackgroundService> IMapServicesToClientView.ListServices
        {
            get;
            set;
        }

        public List<MapServicesToClientContract> BackgroundServicesList
        {
            get;
            set;
        }

        public String SelectedServices
        {
            get;
            set;
        }

        public int SelectedTenantId
        {
            get;
            set;
        }

        public bool IsServicesMapped
        {
            get;
            set;
        }

        public int DeactivateServiceId
        {
            get;
            set;
        }

        public bool IsServiceDeactivated
        {
            get;
            set;
        }


        protected override void ApplyActionLevelPermission(List<Entity.ClientEntity.ClsFeatureAction> ctrlCollection, string screenName = "")
        {
            base.ApplyActionLevelPermission(ctrlCollection, screenName);
            ApplyPermisions();

        }

        private void ApplyPermisions()
        {
            List<Entity.FeatureRoleAction> permission = base.ActionPermission;
            if (permission.IsNotNull())
            {
                permission.ForEach(x =>
                {
                    switch (x.PermissionID)
                    {
                        case AppConsts.ONE:
                            {
                                break;
                            }
                        case AppConsts.THREE:
                        case AppConsts.FOUR:
                            {
                                if (x.FeatureAction.CustomActionId == "Deactivate")
                                {
                                    grdService.MasterTableView.GetColumn("DeactivateMappings").Display = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "Save")
                                {
                                    fsucCmdBar1.Visible = false;
                                    fsucCmdBar1.SubmitButton.Visible = false;
                                }
                                break;
                            }
                    }

                }
                    );
            }
        }

        public override List<Entity.ClientEntity.ClsFeatureAction> ActionCollection
        {
            get
            {
                List<Entity.ClientEntity.ClsFeatureAction> actionCollection = new List<Entity.ClientEntity.ClsFeatureAction>();
                Entity.ClientEntity.ClsFeatureAction objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                objClsFeatureAction.CustomActionId = "Deactivate";
                objClsFeatureAction.CustomActionLabel = "Deactivate";
                objClsFeatureAction.ScreenName = "Map Services To Client";
                actionCollection.Add(objClsFeatureAction);
                objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                objClsFeatureAction.CustomActionId = "Save";
                objClsFeatureAction.CustomActionLabel = "Save Mapping";
                objClsFeatureAction.ScreenName = "Map Services To Client";
                actionCollection.Add(objClsFeatureAction);




                return actionCollection;
            }
        }

        public int[] PreMappedServicesIds
        {
            get
            {
                ViewState["PreMappedServicesIds"] = PreMappedServicesIds;
                return ViewState["PreMappedServicesIds"] as Int32[];
            }
            set
            {
                ViewState["PreMappedServicesIds"] = value;
            }
        }

        //START UAT-3157
        public Int32 PreferredSelectedTenantID
        {
            get
            {
                if (!ViewState["PreferredSelectedTenantID"].IsNull())
                {
                    return (Int32)ViewState["PreferredSelectedTenantID"];
                }
                return AppConsts.NONE;
            }
            set
            {
                ViewState["PreferredSelectedTenantID"] = value;
            }
        }
        //END UAT-3157

        #endregion

        #region SEARCH BUTTONS
        protected void CmdBarSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlTenant.SelectedValue != "0")
                {
                    lblInstitution.Visible = false;
                    SelectedTenantId = ddlTenant.SelectedValue != String.Empty ? Convert.ToInt32(ddlTenant.SelectedValue) : TenantId;
                    Presenter.GetAlreadyMappedServices(); //Get premapped services list.
                    rfvTenant.Enabled = false;
                    // grdService.Rebind();
                    grdService.Visible = true;
                    fsucCmdBar1.Visible = true;
                    ApplyActionLevelPermission(ActionCollection, "Map Services To Client");
                    Int32? svcID = txtSvcIDs.Text == String.Empty ? (Int32?)null : Convert.ToInt32(txtSvcIDs.Text);
                    String svcName = txtName.Text == String.Empty ? null : txtName.Text;
                    String extCode = txtCode.Text == String.Empty ? null : txtCode.Text;
                    Presenter.GetServices(svcID, svcName, extCode);
                    grdService.DataSource = BackgroundServicesList;
                    grdService.DataBind();
                }
                else
                {
                    lblInstitution.Visible = true;
                    txtSvcIDs.Text = String.Empty;
                    txtName.Text = String.Empty;
                    txtCode.Text = String.Empty;
                    ddlTenant.SelectedIndex = AppConsts.NONE;
                    grdService.DataSource = new List<MapServicesToClientContract>();
                    grdService.DataBind();
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

        protected void CmdBarReset_Click(object sender, EventArgs e)
        {
            try
            {
                lblInstitution.Visible = false;
                txtSvcIDs.Text = String.Empty;
                txtName.Text = String.Empty;
                txtCode.Text = String.Empty;
                //Presenter.GetServices();
                ddlTenant.SelectedIndex = AppConsts.NONE;
                grdService.DataSource = new List<MapServicesToClientContract>();
                grdService.DataBind();
                /*Start UAT-3157*/
                GetPreferredSelectedTenant();
                /*End UAT-3157*/
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
        protected void CmdBarCancel_Click(object sender, EventArgs e)
        {
            try
            {
                BackgroundServicesList = null;
                Response.Redirect(String.Format(AppConsts.DASHBOARD_PAGE_NAME), false);
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

        protected void ddlTenant_DataBound(object sender, EventArgs e)
        {
            //ddlTenant.Items.Insert(0, new DropDownListItem("--Select--"));
            //ddlTenant.Items.Add(new RadComboBoxItem("---Select---","0"));
        }
    }
}