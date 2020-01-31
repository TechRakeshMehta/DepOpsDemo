using INTSOF.UI.Contract.PlacementMatching;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CoreWeb.PlacementMatching.Views
{
    public partial class GridViewControl : BaseUserControl, IGridView
    {
        private GridViewPresenter _presenter = new GridViewPresenter();
        public delegate void HandleView(Boolean isGridView);
        public event HandleView eventHandleView;

        public GridViewPresenter Presenter
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

        public IGridView CurrentViewContext
        {
            get
            {
                return this;
            }
        }
        public Int32 AgencyHierarchyID { get; set; }
        public String StatusCode { get; set; }
        Int32 IGridView.OpportunityId { get; set; }
        Int32 IGridView.RequestId { get; set; }
        
        List<RequestDetailContract> IGridView.lstPlacementMaching { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
               
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

        #region Grid Events

        protected void grdPlacementMatchingMapping_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
              
                Presenter.GetSearchRequestData(AgencyHierarchyID,StatusCode);
                grdPlacementMatchingMapping.DataSource = CurrentViewContext.lstPlacementMaching;
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

        protected void grdPlacementMatchingMapping_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Details")
                {
                    Int32 requestID = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["RequestId"]);
                    Int32 opportunityID = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OpportunityId"]);
                    var requestDetails = RequestDetails.REQUESTDETAILS.GetStringValue();
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "SearchRequestPopUp(" + "'" + requestID.ToString() + "'" + ",'" + opportunityID.ToString() + "','" + requestDetails + "');", true);
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

        protected void grdPlacementMatchingMapping_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
        {
            try
            {
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

        protected void grdPlacementMatchingMapping_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            try
            {
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

        protected void btnCalender_Click(object sender, EventArgs e)
        {
            try
            {
                eventHandleView(false);
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

        public void BindGrid()
        {
            grdPlacementMatchingMapping.Rebind();
        }
    }
}