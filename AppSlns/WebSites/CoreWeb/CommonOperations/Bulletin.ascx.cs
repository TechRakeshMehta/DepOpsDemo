using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using Entity;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.UI.Contract.PackageBundleManagement;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Web.UI;
using Telerik.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace CoreWeb.CommonOperations.Views
{
    public partial class Bulletin : BaseUserControl, IBulletinView
    {
        #region Private variables

        private BulletinPresenter _presenter = new BulletinPresenter();
        private String _viewType;
        private BulletinContract _viewContract = null;
        private List<Int32> _selectedTenantIds;
        private List<Int32> _selectedHierarchyIds;
        private Int32 _tenantId = 0;
        private bool _isNeedToLoadData = true;

        #endregion

        #region Properties

        public BulletinPresenter Presenter
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
        public IBulletinView CurrentViewContext
        {
            get
            {
                return this;
            }
        }
        String IBulletinView.ErrorMessage
        {
            get;
            set;
        }
        String IBulletinView.SuccessMessage
        { get; set; }

        public Boolean IsADBAdmin { get; set; }

        //public String BulletinTitle
        //{ get; set; }
        //public String BulletinContents
        //{ get; set; }
        List<BulletinContract> IBulletinView.BulletinDetails
        { get; set; }

        BulletinContract IBulletinView.ViewContract
        {
            get;
            set;
        }

        public Int32 CurrentUserId
        {
            get
            {
                return base.CurrentUserId;
            }
        }

        public List<Tenant> ListTenants
        {
            get;
            set;
        }

        Int32 IBulletinView.TenantId
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

        public List<Int32> SelectedTenantID
        {
            get
            {
                _selectedTenantIds = new List<int>();
                foreach (RadComboBoxItem item in ddlSearchTenant.Items)
                {
                    if (item.Checked == true)
                        _selectedTenantIds.Add(Convert.ToInt32(item.Value));
                }
                return _selectedTenantIds;
            }
            set
            {
                _selectedTenantIds = value;
                foreach (RadComboBoxItem item in ddlSearchTenant.Items)
                {
                    if (_selectedTenantIds.Contains(Convert.ToInt32(item.Value)))
                        item.Checked = true;
                }
            }
        }

        public List<Int32> SelectedHierarchyIds
        {
            get
            {
                _selectedHierarchyIds = new List<int>();

                if (hdnDepartmntPrgrmMppng.IsNotNull() && hdnDepartmntPrgrmMppng.Value.IsNotNull() && hdnDepartmntPrgrmMppng.Value.Length > 0)
                {
                    foreach (string item in hdnDepartmntPrgrmMppng.Value.Split(','))
                    {
                        _selectedHierarchyIds.Add(Convert.ToInt32(item.Trim()));
                    }
                }

                return _selectedHierarchyIds;
            }
            set
            {
                _selectedHierarchyIds = value;
            }
        }

        #endregion

        #region Page Events

        /// <summary>
        ///  set the page title on bread crumb
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            try
            {
                grdBulletin.WclGridDataObject = new GridObjectDataContainer();
                ((GridObjectDataContainer)(grdBulletin.WclGridDataObject)).ColumnsToSkipEncoding.Add("BulletinContent");                
                base.OnInit(e);
                base.Title = "Bulletin";
                base.SetPageTitle("Bulletin");
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
            try
            {
                if (Presenter.IsAdminLoggedIn())
                {
                    CurrentViewContext.IsADBAdmin = true;
                    grdBulletin.MasterTableView.GetColumn("DPMLabel").Visible = false;
                }

                if (!Page.IsPostBack)
                {
                    _isNeedToLoadData = false;
                    Presenter.OnViewLoaded();
                    ddlSearchTenant.DataSource = ListTenants;
                    ddlSearchTenant.DataBind();

                    if (CurrentViewContext.IsADBAdmin)
                    {
                        lblHieararchy.Visible = false;
                        lnkHierarchy.Visible = false;

                        if (ddlSearchTenant.IsNotNull() && ddlSearchTenant.Items.Count > 0)
                        {
                            foreach (RadComboBoxItem item in ddlSearchTenant.Items)
                            {
                                item.Checked = true;
                            }
                        }
                    }
                    else
                    {
                        if (ddlSearchTenant.FindItemByValue(Convert.ToString(CurrentViewContext.TenantId)).IsNotNull())
                        {
                            ddlSearchTenant.FindItemByValue(Convert.ToString(CurrentViewContext.TenantId)).Checked = true;
                            ddlSearchTenant.Enabled = false;
                            hdnTenantId.Value = Convert.ToString(CurrentViewContext.TenantId);
                        }
                    }
                }

                lblinstituteHierarchy.Text = hdnHierarchyLabel.Value.HtmlEncode();

                //Set Module Title
                BasePage basePage = base.Page as BasePage;
                if (basePage != null)
                {
                    basePage.SetModuleTitle("Manage Bulletin");
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

        #region GridEvents

        protected void grdBulletin_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                if (_isNeedToLoadData)
                {
                    Presenter.GetBulletin();
                }
                else
                {
                    CurrentViewContext.BulletinDetails = new List<BulletinContract>();
                }
                grdBulletin.DataSource = CurrentViewContext.BulletinDetails;

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

        protected void grdBulletin_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
                {
                    Presenter.OnViewLoaded();

                    GridEditFormItem editform = (e.Item as GridEditFormItem);
                    WclTextBox txtBulletinTitle = (e.Item.FindControl("txtBulletinTitle") as WclTextBox);
                    WclEditor rdEditorNotes = (e.Item.FindControl("rdEditorNotes") as WclEditor);
                    WclComboBox ddlTenant = (e.Item.FindControl("ddlTenant")) as WclComboBox;
                    CustomValidator cvHierarchySelection = (e.Item.FindControl("cvHierarchySelection")) as CustomValidator;

                    BulletinContract bulletinContract = e.Item.DataItem as BulletinContract;

                    ddlTenant.DataSource = ListTenants;
                    ddlTenant.DataBind();

                    if (CurrentViewContext.IsADBAdmin)
                    {
                        HtmlGenericControl lblHierarchy = (e.Item.FindControl("lblHierarchyWhileAddUpdate")) as HtmlGenericControl;
                        lblHierarchy.Visible = false;

                        HtmlGenericControl lnkHierarchy = (e.Item.FindControl("lnkHierarchyWhileAddUpdate")) as HtmlGenericControl;
                        lnkHierarchy.Visible = false;

                        if (bulletinContract.IsNotNull() && ddlTenant.IsNotNull() && ddlTenant.Items.Count > 0)
                        {
                            foreach (string tenantID in bulletinContract.InstitutionIds.Split(','))
                            {
                                if (ddlTenant.FindItemByValue(tenantID.Trim()).IsNotNull())
                                {
                                    ddlTenant.FindItemByValue(tenantID.Trim()).Checked = true;
                                }
                            }
                        }
                        foreach (Int32 tenantID in SelectedTenantID)
                        {
                            if (ddlTenant.FindItemByValue(tenantID.ToString()).IsNotNull())
                            {
                                ddlTenant.FindItemByValue(tenantID.ToString()).Checked = true;
                            }
                        }

                    }
                    else
                    {
                       cvHierarchySelection.Enabled = true;
                        ddlTenant.Enabled = false;
                        if (ddlTenant.FindItemByValue(Convert.ToString(CurrentViewContext.TenantId)).IsNotNull())
                            ddlTenant.FindItemByValue(Convert.ToString(CurrentViewContext.TenantId)).Checked = true;

                        HiddenField hdnDepartmntPrgrmMppngWhileAddUpdate = (e.Item.FindControl("hdnDepartmntPrgrmMppngWhileAddUpdate")) as HiddenField;

                        if (hdnDepartmntPrgrmMppngWhileAddUpdate.IsNotNull() && bulletinContract.IsNotNull())
                            hdnDepartmntPrgrmMppngWhileAddUpdate.Value = bulletinContract.HieararchyIds.Trim();


                        HiddenField hdnHierarchyLabelWhileAddUpdate = (e.Item.FindControl("hdnHierarchyLabelWhileAddUpdate")) as HiddenField;

                        if (hdnHierarchyLabelWhileAddUpdate.IsNotNull() && bulletinContract.IsNotNull())
                            hdnHierarchyLabelWhileAddUpdate.Value = bulletinContract.DPMLabel;


                        Label lblinstituteHierarchyWhileAddUpdate = (e.Item.FindControl("lblinstituteHierarchyWhileAddUpdate")) as Label;
                        if (lblinstituteHierarchyWhileAddUpdate.IsNotNull() && bulletinContract.IsNotNull())
                            lblinstituteHierarchyWhileAddUpdate.Text = bulletinContract.DPMLabel.HtmlEncode();


                    }

                    if (!(e.Item is GridEditFormInsertItem))
                    {
                        if (bulletinContract != null)
                        {
                            txtBulletinTitle.Text = bulletinContract.BulletinTitle;
                            rdEditorNotes.Content = bulletinContract.BulletinContent;
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

        protected void grdBulletin_ItemCommand(object sender, GridCommandEventArgs e)
        {

            try
            {
                if (e.CommandName == RadGrid.PerformInsertCommandName || e.CommandName == RadGrid.UpdateCommandName)
                {
                    CurrentViewContext.ViewContract = new BulletinContract();
                    if (e.CommandName == RadGrid.UpdateCommandName)
                    {
                        CurrentViewContext.ViewContract.BulletinID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["BulletinID"]);
                    }

                    CurrentViewContext.ViewContract.BulletinTitle = (e.Item.FindControl("txtBulletinTitle") as WclTextBox).Text.Trim();
                    CurrentViewContext.ViewContract.BulletinContent = (e.Item.FindControl("rdEditorNotes") as WclEditor).Content.Trim();
                    CurrentViewContext.ViewContract.IsCreatedByADBAdmin = CurrentViewContext.IsADBAdmin;

                    WclComboBox ddlTenant = (e.Item.FindControl("ddlTenant")) as WclComboBox;
                    CurrentViewContext.ViewContract.LstSelectedTenantID = new List<Int32>();
                    foreach (var tenantId in ddlTenant.CheckedItems)
                    {
                        CurrentViewContext.ViewContract.LstSelectedTenantID.Add(Convert.ToInt32(tenantId.Value));
                    }

                    if (!CurrentViewContext.IsADBAdmin)
                    {
                        HiddenField depPrgMappingId = e.Item.FindControl("hdnDepartmntPrgrmMppngWhileAddUpdate") as HiddenField;
                        CurrentViewContext.ViewContract.LstSelectedDepPrgMappingId = new List<Int32>();

                        if (depPrgMappingId.IsNotNull() && depPrgMappingId.Value.IsNotNull() && depPrgMappingId.Value.Length > 0)
                        {
                            foreach (var dpmId in depPrgMappingId.Value.Split(','))
                            {
                                CurrentViewContext.ViewContract.LstSelectedDepPrgMappingId.Add(Convert.ToInt32(dpmId.Trim()));
                            }
                        }
                    }

                    if (Presenter.SaveUpdateBulletin())
                    {
                        if (e.CommandName == RadGrid.UpdateCommandName)
                        {
                            base.ShowSuccessMessage("Bulletin updated successfully.");
                        }
                        else
                        {
                            base.ShowSuccessMessage("Bulletin saved successfully.");
                        }
                    }
                    else
                    {
                        e.Canceled = false;
                        base.ShowErrorMessage("Some error has occured due to which Bulletin can not be saved.");
                    }
                }

                if (e.CommandName == RadGrid.DeleteCommandName)
                {
                    CurrentViewContext.ViewContract = new BulletinContract();
                    CurrentViewContext.ViewContract.BulletinID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["BulletinID"]);
                    Presenter.DeleteBulletin();
                    base.ShowSuccessMessage("Bulletin deleted successfully.");
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

        #region Button_Click
        protected void fsucCmdBarSearch_Click(object sender, EventArgs e)
        {
            grdBulletin.Rebind();
        }

        protected void fsucCmdBarReset_Click(object sender, EventArgs e)
        {
            _isNeedToLoadData = false;
            if (CurrentViewContext.IsADBAdmin)
            {
                //ddlSearchTenant.DataSource = ListTenants;
                //ddlSearchTenant.DataBind();
                lblHieararchy.Visible = false;
                lnkHierarchy.Visible = false;

                if (ddlSearchTenant.IsNotNull() && ddlSearchTenant.Items.Count > 0)
                {
                    foreach (RadComboBoxItem item in ddlSearchTenant.Items)
                    {
                        item.Checked = true;
                    }
                }
            }
            else
            {
                if (ddlSearchTenant.FindItemByValue(Convert.ToString(CurrentViewContext.TenantId)).IsNotNull())
                {
                    ddlSearchTenant.FindItemByValue(Convert.ToString(CurrentViewContext.TenantId)).Checked = true;
                    ddlSearchTenant.Enabled = false;
                    hdnTenantId.Value = Convert.ToString(CurrentViewContext.TenantId);
                }

                hdnDepartmntPrgrmMppng.Value = string.Empty;
                hdnHierarchyLabel.Value = string.Empty;
                hdnInstitutionNodeId.Value = string.Empty;

                lblinstituteHierarchy.Text = string.Empty;
            }
            grdBulletin.Rebind();
        }

        protected void fsucCmdBarCancel_Click(object sender, EventArgs e)
        {
            try
            {
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


    }
}