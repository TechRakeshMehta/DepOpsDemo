using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreWeb.ContractManagement.Views;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.UI.Contract.ContractManagement;
using Telerik.Web.UI;

namespace CoreWeb.ContractManagement.Views
{
    public partial class ManageSites : BaseUserControl, IManageSitesView
    {
        #region Variables

        #region Private Variables

        private ManageSitesPresenter _presenter = new ManageSitesPresenter();

        #endregion

        #endregion

        #region Properties

        public ManageSitesPresenter Presenter
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

        /// <summary>
        /// Gets the current view context.
        /// </summary>
        /// <remarks></remarks>
        public IManageSitesView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// Represents the contractid to which the current Sites belong to
        /// </summary>
        Int32 IManageSitesView.ContractId
        {
            get;
            set;
        }


        /// <summary>
        /// Represents the TenantId 
        /// </summary>
        Int32 IManageSitesView.TenantId
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the List of Sites under the selected Contract
        /// </summary>
        List<SiteContract> IManageSitesView.lstSiteContracts
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the current Logged in userid.
        /// </summary>
        Int32 IManageSitesView.CurrentUserId
        {
            get
            {
                return base.CurrentUserId;
            }
        }

        /// <summary>
        /// MappingId of the Contract and SiteContract i.e. CSCM_ID of 'ContractSitesContractMapping' table
        /// </summary> 
        Int32 IManageSitesView.CSCMId
        {
            get;
            set;
        }

        /// <summary>
        /// Site to be Added or Updated
        /// </summary>
        SiteContract IManageSitesView.SiteContract
        {
            get;
            set;
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            // TODO
            // CurrentViewContext.TenantId = 104;
        }

        protected void grdSites_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                // TODO   
                // CurrentViewContext.ContractId = 1;

                // Presenter.GetContractSites();
                grdSites.DataSource = CurrentViewContext.lstSiteContracts;
            }
            catch (Exception ex)
            {
            }
        }

        protected void grdSites_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == RadGrid.DeleteCommandName)
            {
                CurrentViewContext.CSCMId = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ContractSiteMappingId"]);
                //  Presenter.DeleteContractSite();
            }
            else if (e.CommandName == RadGrid.UpdateCommandName)
            {
                CurrentViewContext.SiteContract = new SiteContract();
                CurrentViewContext.SiteContract.SiteName = (e.Item.FindControl("txtSiteName") as WclTextBox).Text;
                CurrentViewContext.SiteContract.SiteAddress = (e.Item.FindControl("txtSiteAddress") as WclTextBox).Text;
                CurrentViewContext.SiteContract.SiteId = Convert.ToInt32((e.Item.FindControl("hdnSiteId") as HiddenField).Value);

                // Presenter.UpdateContractSite();
            }
        }
    }
}