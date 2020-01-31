using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using Entity.ClientEntity;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.Utils;
using Telerik.Web.UI;


namespace CoreWeb.BkgOperations.Views
{
    public partial class BkgOrderHistory : BaseUserControl, IBkgOrderHistoryView
    {

        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private BkgOrderHistoryPresenter _presenter = new BkgOrderHistoryPresenter();
        private String _viewType;

        #endregion

        #endregion

        #region Properties

        #region Private Properties


        /// <summary>
        /// Gets the current view context.
        /// </summary>
        /// <remarks></remarks>
        private IBkgOrderHistoryView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        private BkgOrderHistoryPresenter Presenter
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

        Int32 IBkgOrderHistoryView.SelectedTenantId
        {
            get
            {
                return Convert.ToInt32(SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_SELECTED_TENANT_ID));
            }
        }

        Int32 IBkgOrderHistoryView.CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }


        Int32 IBkgOrderHistoryView.OrderID
        {
            get
            {
                return Convert.ToInt32(SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_ORDER_ID));
            }
        }

        List<OrderEventHistoryContract> IBkgOrderHistoryView.lstOrderEventHistory
        {
            get;
            set;
        }

        #endregion

        #endregion

        #region Events

        #region Page Events

        /// <summary>
        /// Page load event for initialized event in presenter.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
        /// <remarks></remarks>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    Presenter.OnViewInitialized();
                    Presenter.OnViewLoaded();
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

        protected override void OnInit(EventArgs e)
        {
            try
            {
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                base.Title = "Order History";
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

        #endregion

        #region Grid Related Events

        protected void grdOrderHistory_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetOrderEventHistory();
                grdOrderHistory.DataSource = CurrentViewContext.lstOrderEventHistory;//.OrderBy(col => col.BOEH_CreatedOn); Commented code for Wenatchee Valley Data Sync issue [22 June 2015]
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
    }
}