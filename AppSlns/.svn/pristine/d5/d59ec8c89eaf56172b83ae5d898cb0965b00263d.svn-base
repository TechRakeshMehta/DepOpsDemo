using System;
using Microsoft.Practices.ObjectBuilder;
using Telerik.Web.UI;
using INTSOF.UI.Contract.ComplianceManagement;
using Entity.ClientEntity;
using INTSOF.Utils;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Linq;

namespace CoreWeb.ComplianceAdministration.Views
{
    public partial class ItemsListing : BaseUserControl, IItemsListingView
    {

        private ComplianceCategoryItemContract _viewContract;
        private ItemsListingPresenter _presenter = new ItemsListingPresenter();

        public int ParentCategoryId { get; set; }

        /// <summary>
        /// Gets and sets TenantId of selected tenant
        /// </summary>
        public Int32 SelectedTenantId
        {
            get
            {
                return Convert.ToInt32(ViewState["SelectedTenantId"]);
            }
            set
            {
                ViewState["SelectedTenantId"] = value;
            }
        }
        public Int32 CountOfDisplayOrder
        {
            get
            {
                return Convert.ToInt32(ViewState["CountOfDisplayOrder"]);
            }
            set
            {
                ViewState["CountOfDisplayOrder"] = value;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            try
            {
                grdItems.WclGridDataObject = new GridObjectDataContainer();
                ((GridObjectDataContainer)(grdItems.WclGridDataObject)).ColumnsToSkipEncoding.Add("ComplianceItem.Description");
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
            }
            Presenter.OnViewLoaded();
            if (SelectedTenantId == DefaultTenantId)
                ((GridButtonColumn)grdItems.MasterTableView.GetColumnSafe("DeleteColumn")).ConfirmText = "Are you sure you want to delete this record?";
            else
                ((GridButtonColumn)grdItems.MasterTableView.GetColumnSafe("DeleteColumn")).ConfirmText = "Deleting this record will impact existing orders. Are you sure you want to delete?";
        }


        public ItemsListingPresenter Presenter
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

        public Int32 DefaultTenantId
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

        public String ErrorMessage
        {
            get;
            set;
        }

        // TODO: Forward events to the presenter and show state to the user.
        // For examples of this, see the View-Presenter (with Application Controller) QuickStart:
        //	

        protected void grdItems_NeedDatasource(object sender, GridNeedDataSourceEventArgs e)
        {
            ViewContract.CCI_CategoryId = ParentCategoryId;
            Presenter.GetComplianceItems();
            if (lstComplianceItems.Count > 0)
                CountOfDisplayOrder = lstComplianceItems.OrderByDescending(x => x.CCI_DisplayOrder).Select(x => x.CCI_DisplayOrder).First();
            else
                CountOfDisplayOrder = 0;
            if (CurrentViewContext.lstComplianceItems.Count > 0)
            {
                grdItems.Visible = true;
                this.Parent.FindControl("lblTitle").Visible = true;
                grdItems.DataSource = CurrentViewContext.lstComplianceItems.OrderBy(col => col.CCI_DisplayOrder);
                grdItems.Columns.FindByUniqueName("TenantName").Visible = DefaultTenantId.Equals(SelectedTenantId);
                // grdItems.Columns.FindByUniqueName("EffectiveDate").Visible = DefaultTenantId.Equals(SelectedTenantId);
            }
            else
            {
                grdItems.Visible = false;
                this.Parent.FindControl("lblTitle").Visible = false;

            }
        }

        protected void grdItems_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == RadGrid.DeleteCommandName)
            {
                CurrentViewContext.ViewContract.CCI_Id = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CCI_ID"]);
                HiddenField hdnfComplianceItemID = (e.Item as GridDataItem).FindControl("hdnfComplianceItemID") as HiddenField;
                if (Presenter.IfItemCanBeRemoved(Convert.ToInt32(hdnfComplianceItemID.Value), ParentCategoryId))
                {
                    if (Presenter.DeleteCategoryItemMapping(Convert.ToInt32(hdnfComplianceItemID.Value), ParentCategoryId))
                    {
                        grdItems.Rebind();
                        System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                        (this.Page as BaseWebPage).ShowSuccessMessage("Compliance category item mapping deleted successfully.");
                    }
                }
                else
                {
                    (this.Page as BaseWebPage).ShowErrorInfoMessage(CurrentViewContext.ErrorMessage);
                }
            }
        }
        protected void grdItems_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem)
                {
                    if (SelectedTenantId.IsNotNull())
                    {
                        if (SelectedTenantId == DefaultTenantId)
                        {
                            grdItems.Columns.FindByUniqueName("EffectiveDate").Visible = false;
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


        public ComplianceCategoryItemContract ViewContract
        {
            get
            {
                if (_viewContract.IsNull())
                {
                    _viewContract = new ComplianceCategoryItemContract();
                }
                return _viewContract;
            }
        }

        public List<ComplianceCategoryItem> lstComplianceItems
        {
            get;
            set;
        }

        public IItemsListingView CurrentViewContext
        {
            get { return this; }
        }


        public Int32 TenantId
        {
            get;
            set;
        }

        public Int32 CurrentLoggedInUserId
        {
            get { return base.CurrentUserId; }

        }

        public void RebindGrid()
        {
            grdItems.Rebind();
        }
    }
}

