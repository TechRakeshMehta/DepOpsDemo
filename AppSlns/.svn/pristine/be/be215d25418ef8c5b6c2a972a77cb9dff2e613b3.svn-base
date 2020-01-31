using CoreWeb.Shell;
using Entity;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using CoreWeb.IntsofSecurityModel;

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class ManageCompliancePriorityObjectMapping : BaseUserControl, IManageCompliancePriorityObjectMappingView
    {

        #region Variables
        private ManageCompliancePriorityObjectMappingPresenter _presenter = new ManageCompliancePriorityObjectMappingPresenter();
        private Int32 _tenantid;
        #endregion

        #region Properties
        public ManageCompliancePriorityObjectMappingPresenter Presenter
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

        public IManageCompliancePriorityObjectMappingView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        Int32 IManageCompliancePriorityObjectMappingView.CurrentLoggedInUserID
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        Int32 IManageCompliancePriorityObjectMappingView.TenantId
        {
            get
            {
                if (_tenantid == 0)
                {
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

        Boolean IManageCompliancePriorityObjectMappingView.IsAdminLoggedIn
        {
            get;
            set;
        }
        public List<Tenant> lstTenants
        {
            get
            {
                if (!ViewState["lstTenants"].IsNullOrEmpty())
                {
                    return ViewState["lstTenants"] as List<Tenant>;
                }
                return new List<Tenant>();
            }
            set
            {
                ViewState["lstTenants"] = value;
            }
        }

        public List<CompliancePriorityObjectContract> lstCompObjMappings
        {
            get
            {
                if (!ViewState["lstCompObjMappings"].IsNullOrEmpty())
                {
                    return ViewState["lstCompObjMappings"] as List<CompliancePriorityObjectContract>;
                }
                return new List<CompliancePriorityObjectContract>();
            }
            set
            {
                ViewState["lstCompObjMappings"] = value;
            }
        }

        public List<CompliancePriorityObjectContract> lstCompPriorityObjects
        {
            get
            {
                if (!ViewState["lstCompPriorityObjects"].IsNullOrEmpty())
                {
                    return ViewState["lstCompPriorityObjects"] as List<CompliancePriorityObjectContract>;
                }
                return new List<CompliancePriorityObjectContract>();
            }
            set
            {
                ViewState["lstCompPriorityObjects"] = value;
            }
        }

        public Int32 selectedTenantID
        {
            get
            {
                if (String.IsNullOrEmpty(ddlTenant.SelectedValue))
                    return 0;
                return Convert.ToInt32(ddlTenant.SelectedValue);
            }
            set
            {
                if (value > 0)
                {
                    ddlTenant.SelectedValue = Convert.ToString(value);
                }
                else
                {
                    ddlTenant.SelectedIndex = value;
                }
            }

        }

        public Int32 selectedCompObjID
        {
            get;
            set;
        }
        public Int32 selectedCategoryID
        {
            get;
            set;

        }

        public List<CompliancePriorityObjectContract> lstCategoryItems
        {
            get
            {
                if (!ViewState["lstCategoryItems"].IsNullOrEmpty())
                {
                    return ViewState["lstCategoryItems"] as List<CompliancePriorityObjectContract>;
                }
                return new List<CompliancePriorityObjectContract>();
            }
            set
            {
                ViewState["lstCategoryItems"] = value;
            }
        }

        public List<CompliancePriorityObjectContract> lstCategory
        {
            get
            {
                if (!ViewState["lstCategory"].IsNullOrEmpty())
                {
                    return ViewState["lstCategory"] as List<CompliancePriorityObjectContract>;
                }
                return new List<CompliancePriorityObjectContract>();
            }
            set
            {
                ViewState["lstCategory"] = value;
            }
        }

        public List<CompliancePriorityObjectContract> lstItem
        {
            get
            {
                if (!ViewState["lstItem"].IsNullOrEmpty())
                {
                    return ViewState["lstItem"] as List<CompliancePriorityObjectContract>;
                }
                return new List<CompliancePriorityObjectContract>();
            }
            set
            {
                ViewState["lstItem"] = value;
            }
        }

        #endregion

        #region Events

        #region Page Events

        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                base.Title = "Manage Compliance Priority Object Mapping";
                base.SetPageTitle("Manage Compliance Priority Object Mapping");
                //if (!CurrentViewContext.selectedTenantID.IsNullOrEmpty() && CurrentViewContext.selectedTenantID>AppConsts.NONE)
                //    grdCompObjMapping.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = true;
                //else
                //    grdCompObjMapping.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;
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
                if (!this.IsPostBack)
                {
                    BindTenant();
                    BindCompPriorityObject();
                }
                if (!CurrentViewContext.selectedTenantID.IsNullOrEmpty() && CurrentViewContext.selectedTenantID > AppConsts.NONE)
                    grdCompObjMapping.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = true;
                else
                    grdCompObjMapping.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;
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

        #region Grid Events
        protected void grdCompObjMapping_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetCompObjMappings();
                grdCompObjMapping.DataSource = CurrentViewContext.lstCompObjMappings;
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

        protected void grdCompObjMapping_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == RadGrid.PerformInsertCommandName || e.CommandName == RadGrid.UpdateCommandName)
                {
                    WclComboBox ddlObject = e.Item.FindControl("ddlObject") as WclComboBox;
                    WclComboBox ddlCategory = e.Item.FindControl("ddlCategory") as WclComboBox;
                    WclComboBox ddlItem = e.Item.FindControl("ddlItem") as WclComboBox;

                    CompliancePriorityObjectContract compObjMapping = new CompliancePriorityObjectContract();
                    compObjMapping.CategoryID = Convert.ToInt32(ddlCategory.SelectedValue);
                    compObjMapping.CPO_ID = Convert.ToInt32(ddlObject.SelectedValue);
                    compObjMapping.ItemID = ddlItem.SelectedValue.IsNullOrEmpty() ? (Int32?)null : Convert.ToInt32(ddlItem.SelectedValue);
                    if (e.CommandName == RadGrid.UpdateCommandName)
                        compObjMapping.CCIPOM_ID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CCIPOM_ID"]);
                    if (Presenter.SaveCompObjMapping(compObjMapping))
                    {

                        if (e.CommandName == RadGrid.UpdateCommandName)
                            base.ShowSuccessMessage("Compliance object mapping is updated successfully.");
                        else
                            base.ShowSuccessMessage("Compliance object mapping is saved successfully.");
                        e.Canceled = false;
                    }
                    else
                    {
                        e.Canceled = true;
                        base.ShowErrorMessage("Some error has occurred. Please try again.");
                    }


                }
                if (e.CommandName == RadGrid.DeleteCommandName)
                {
                    //Work to be done here, Needed to check if the mapping is used in tracking configuration.
                    Int32 compObjMappingId = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CCIPOM_ID"]);
                    if (!compObjMappingId.IsNullOrEmpty() && compObjMappingId > AppConsts.NONE)
                    {
                        if (Presenter.DeleteCompObjMapping(compObjMappingId))
                        {
                            base.ShowSuccessMessage("Compliance object mapping is deleted successfully.");
                        }
                        else
                        {
                            e.Canceled = true;
                            base.ShowErrorMessage("Some error has occurred. Please try again.");
                        }
                    }
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

        protected void grdCompObjMapping_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            try
            {
                if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
                {
                    GridEditFormItem editform = (e.Item as GridEditFormItem);
                    WclComboBox ddlObject = e.Item.FindControl("ddlObject") as WclComboBox;
                    WclComboBox ddlCategory = e.Item.FindControl("ddlCategory") as WclComboBox;
                    WclComboBox ddlItem = e.Item.FindControl("ddlItem") as WclComboBox;

                    ddlObject.DataSource = CurrentViewContext.lstCompPriorityObjects;
                    ddlObject.DataBind();
                    ddlObject.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT--"));
                    BindCategory();
                    ddlCategory.DataSource = CurrentViewContext.lstCategory;
                    ddlCategory.DataBind();
                    ddlCategory.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT--"));


                    // Code for edit mode
                    Int32 compObjmappingID = AppConsts.NONE;

                    if (e.Item.ItemIndex >= AppConsts.NONE)
                    {
                        compObjmappingID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CCIPOM_ID"]);
                    }
                    if (!compObjmappingID.IsNullOrEmpty() && compObjmappingID > AppConsts.NONE)
                    {
                        CompliancePriorityObjectContract compObjMapping = CurrentViewContext.lstCompObjMappings.Where(cond => cond.CCIPOM_ID == compObjmappingID).FirstOrDefault();
                        if (!compObjMapping.IsNullOrEmpty())
                        {
                            ddlObject.SelectedValue = Convert.ToString(compObjMapping.CPO_ID);
                            ddlObject.Enabled = false;
                            ddlCategory.SelectedValue = Convert.ToString(compObjMapping.CategoryID);

                            //if (!compObjMapping.ItemID.IsNullOrEmpty() && compObjMapping.ItemID > AppConsts.NONE)
                            //{
                            CurrentViewContext.selectedCategoryID = compObjMapping.CategoryID;
                            BindItem(true, compObjmappingID);
                            List<Int32> lstCompObjMappingsToRemove = CurrentViewContext.lstCompObjMappings.Where(cond => cond.ItemID == AppConsts.NONE).Select(sel => sel.CCIPOM_ID).ToList();
                            CurrentViewContext.lstCompObjMappings.RemoveAll(x => lstCompObjMappingsToRemove.Contains(x.CCIPOM_ID));
                            List<Int32?> lstItemsAlreadyMapped = CurrentViewContext.lstCompObjMappings.Where(cond => cond.CategoryID == CurrentViewContext.selectedCategoryID)
                                                                                                           .Select(sel => sel.ItemID).ToList();
                            //if (compObjMapping.ItemID.IsNullOrEmpty() || compObjMapping.ItemID == AppConsts.NONE)
                            //{
                            //    CurrentViewContext.lstItem.RemoveAll(x => lstItemsAlreadyMapped.Contains(x.ItemID));
                            //}
                            ddlItem.Enabled = true;
                            ddlItem.DataSource = CurrentViewContext.lstItem;
                            ddlItem.DataBind();
                            ddlItem.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT--"));
                            ddlItem.SelectedValue = Convert.ToString(compObjMapping.ItemID);
                            // }
                        }
                    }
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

        #region Button Events

        protected void cmdBarCompObjMapping_SearchClick(object sender, EventArgs e)
        {
            try
            {
                if (!CurrentViewContext.selectedTenantID.IsNullOrEmpty() && CurrentViewContext.selectedTenantID > AppConsts.NONE)
                {
                    ResetGridFilters();
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

        protected void cmdBarCompObjMapping_ResetClick(object sender, EventArgs e)
        {
            try
            { //Reset Tenant 
                ResetTenant();
                grdCompObjMapping.Rebind();
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

        protected void cmdBarCompObjMapping_CancelClick(object sender, EventArgs e)
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

        #region DropDown Events

        protected void ddlTenant_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                if (!ddlTenant.SelectedValue.IsNullOrEmpty())
                {
                    ResetGridFilters();
                }
                else
                {
                    ResetTenant();
                    grdCompObjMapping.Rebind();
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

        protected void ddlTenant_DataBound(object sender, EventArgs e)
        {
            try
            {
                ddlTenant.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT--"));
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

        protected void ddlCategory_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                WclComboBox ddlCategory = sender as WclComboBox;
                if (!ddlCategory.SelectedValue.IsNullOrEmpty())
                {
                    CurrentViewContext.selectedCategoryID = Convert.ToInt32(ddlCategory.SelectedValue);
                    WclComboBox ddlItem = ddlCategory.NamingContainer.FindControl("ddlItem") as WclComboBox;
                    BindItem(false, AppConsts.NONE);
                    if (!CurrentViewContext.lstItem.IsNullOrEmpty())
                    {
                        ddlItem.Enabled = true;
                        ddlItem.DataSource = CurrentViewContext.lstItem;
                        ddlItem.DataBind();
                    }
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

        #region Methods

        private void BindTenant()
        {
            Presenter.GetTenants();
            ddlTenant.DataSource = CurrentViewContext.lstTenants;
            ddlTenant.DataBind();
        }

        private void ResetGridFilters()
        {
            grdCompObjMapping.MasterTableView.SortExpressions.Clear();
            grdCompObjMapping.CurrentPageIndex = 0;
            grdCompObjMapping.MasterTableView.CurrentPageIndex = 0;
            grdCompObjMapping.MasterTableView.IsItemInserted = false;
            grdCompObjMapping.MasterTableView.ClearEditItems();
            grdCompObjMapping.Rebind();
        }

        private void BindCompPriorityObject()
        {
            Presenter.GetCompPriorityObject();
        }

        private void BindCategory()
        {
            Presenter.GetSelectedTenantCategory();
        }

        private void BindItem(Boolean isEditMode, Int32 compObjmappingID)
        {
            Presenter.GetItems(isEditMode, compObjmappingID);
        }

        private void ResetTenant()
        {
            Presenter.IsAdminLoggedIn();
            if (CurrentViewContext.IsAdminLoggedIn)
            {
                CurrentViewContext.selectedTenantID = AppConsts.NONE;
            }
            if (!CurrentViewContext.selectedTenantID.IsNullOrEmpty() && CurrentViewContext.selectedTenantID > AppConsts.NONE)
                grdCompObjMapping.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = true;
            else
                grdCompObjMapping.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;
        }

        #endregion

    }
}