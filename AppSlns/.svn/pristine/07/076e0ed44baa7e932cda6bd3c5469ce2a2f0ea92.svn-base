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
    public partial class ShotSeriesListing : BaseUserControl, IShotSeriesListingView
    {
        private ComplianceCategoryItemContract _viewContract;
        private ShotSeriesListingPresenter _presenter = new ShotSeriesListingPresenter();
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
            }
            Presenter.OnViewLoaded();
        }


        public ShotSeriesListingPresenter Presenter
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

        Int32 IShotSeriesListingView.DefaultTenantId
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

        String IShotSeriesListingView.ErrorMessage
        {
            get;
            set;
        }

        public Int32 CategoryID
        {
            get;
            set;
        }

        protected override void OnInit(EventArgs e)
        {
            try
            {
                grdShotSeries.WclGridDataObject = new GridObjectDataContainer();
                ((GridObjectDataContainer)(grdShotSeries.WclGridDataObject)).ColumnsToSkipEncoding.Add("IS_Description");
                ((GridObjectDataContainer)(grdShotSeries.WclGridDataObject)).ColumnsToSkipEncoding.Add("IS_Details"); 
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

        protected void grdShotSeries_NeedDatasource(object sender, GridNeedDataSourceEventArgs e)
        {
            Presenter.GetItemShotSeries();
            if (!CurrentViewContext.LstShotSeries.IsNullOrEmpty())
            {
                grdShotSeries.Visible = true;
                this.Parent.FindControl("lblTitle").Visible = true;
                grdShotSeries.DataSource = CurrentViewContext.LstShotSeries;
            }
            else
            {
                grdShotSeries.Visible = false;
                this.Parent.FindControl("lblTitle").Visible = false;
            }
        }

        protected void grdShotSeries_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == RadGrid.DeleteCommandName)
            {
                Int32 itemSeriesID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["IS_ID"]);
                if (Presenter.DeleteItemSeries(itemSeriesID))
                {
                    grdShotSeries.Rebind();
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                    (this.Page as BaseWebPage).ShowSuccessMessage("Shot Series deleted successfully.");
                }
                else
                {
                    (this.Page as BaseWebPage).ShowErrorMessage("Some error occured while deleting shot series. Please try again.");
                }
            }
        }

        List<ItemSery> IShotSeriesListingView.LstShotSeries
        {
            get;
            set;
        }

        public IShotSeriesListingView CurrentViewContext
        {
            get { return this; }
        }


        Int32 IShotSeriesListingView.TenantId
        {
            get;
            set;
        }

        Int32 IShotSeriesListingView.CurrentLoggedInUserId
        {
            get { return base.CurrentUserId; }

        }

        public void RebindGrid()
        {
            grdShotSeries.Rebind();
        }
    }
}

