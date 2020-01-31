using CoreWeb.BkgSetup.Views;
using CoreWeb.IntsofExceptionModel.Interface;
using CoreWeb.Shell;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CoreWeb.BkgSetup.Views
{
    public partial class BkgPackageServiceDetails : Page, IBkgPackageServiceDetailsView
    {

        #region Variables

        #region Private Variables
        private BkgPackageServiceDetailsPresenter _presenter = new BkgPackageServiceDetailsPresenter();
        private ISysXExceptionService _exceptionService = SysXWebSiteUtils.ExceptionService;
        protected String ImagePath = "~/images/small";
        protected String ImagePathMedium = "~/images/medium";
        #endregion

        #endregion

        #region Properties

        public BkgPackageServiceDetailsPresenter Presenter
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

        public Int32 CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        public IBkgPackageServiceDetailsView CurrentViewContext
        {
            get { return this; }
        }

        Int32 IBkgPackageServiceDetailsView.TenantId
        {
            get
            {
                return (Int32)(ViewState["TenantId"]);
            }
            set
            {
                ViewState["TenantId"] = value;
            }
        }

        Int32 IBkgPackageServiceDetailsView.OrderID
        {
            get;
            set;
        }

        List<ServiceLevelDetailsForOrderContract> IBkgPackageServiceDetailsView.LstServiceDetails
        {
            get
            {
                return ViewState["lstServiceGrpDetails"] as List<ServiceLevelDetailsForOrderContract>;
            }

            set
            {
                ViewState["lstServiceGrpDetails"] = value as List<ServiceLevelDetailsForOrderContract>;
            }
        }

        #endregion

        #region Events

        #region Page Events


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    Presenter.OnViewInitialized();
                    Presenter.OnViewLoaded();
                    if (Request.QueryString["TenantID"] != null)
                    {
                        CurrentViewContext.TenantId = Convert.ToInt32(Request.QueryString["TenantID"]);
                    }
                    if (Request.QueryString["OrderID"] != null)
                    {
                        CurrentViewContext.OrderID = Convert.ToInt32(Request.QueryString["OrderID"]);
                    }
                    Presenter.GetServiceLevelDetailsForOrder();
                    //hfTenantId.Value = CurrentViewContext.SelectedTenantId.ToString();
                    //hfOrderID.Value = CurrentViewContext.OrderID.ToString();

                    #region UAT-3481

                    if (Request.QueryString["TenantID"] != null)
                    {
                        SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.SESSION_SELECTED_TENANT_ID, Convert.ToInt32(Request.QueryString["TenantID"]));
                    }
                    if (Request.QueryString["OrderID"] != null)
                    {
                        SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.SESSION_ORDER_ID, Convert.ToInt32(Request.QueryString["OrderID"]));
                    }
                    //IsRedirectedFromOrderQueueDetails
                    ucBkgOrderServiceGroups.IsRedirectedFromOrderQueueDetails = true;
                    ucBkgOrderServiceLinePriceInfo.IsRedirectedFromOrderQueueDetails = true;

                    #endregion
                }
            }
            catch (SysXException ex)
            {
                LogError(ex.Message, ex);
            }
            catch (System.Exception ex)
            {
                LogError(ex.Message, ex);
            }
        }
        #endregion

        #region Grid Events

        protected void grdBkgPkgServiceDetails_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                grdBkgPkgServiceDetails.DataSource = CurrentViewContext.LstServiceDetails.DistinctBy(col => col.PackageID);
            }
            catch (SysXException ex)
            {
                LogError(ex.Message, ex);
            }
            catch (System.Exception ex)
            {
                LogError(ex.Message, ex);
            }
        }

        protected void grdServiceGrp_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                GridDataItem parentItem = ((sender as RadGrid).NamingContainer as GridNestedViewItem).ParentItem as GridDataItem;
                Int32? packageID = Convert.ToInt32(parentItem.GetDataKeyValue("PackageID"));
                (sender as RadGrid).DataSource = CurrentViewContext.LstServiceDetails.Where(col => col.PackageID == packageID).DistinctBy(col => col.ServiceGroupID);
            }
            catch (SysXException ex)
            {
                LogError(ex.Message, ex);
            }
            catch (System.Exception ex)
            {
                LogError(ex.Message, ex);
            }
        }

        protected void grdServices_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                GridDataItem parentItem = ((sender as RadGrid).NamingContainer as GridNestedViewItem).ParentItem as GridDataItem;
                Int32? serviceGroupID = Convert.ToInt32(parentItem.GetDataKeyValue("ServiceGroupID"));
                (sender as RadGrid).DataSource = CurrentViewContext.LstServiceDetails.Where(col => col.ServiceGroupID == serviceGroupID).DistinctBy(col => col.ServiceID);
            }
            catch (SysXException ex)
            {
                LogError(ex.Message, ex);
            }
            catch (System.Exception ex)
            {
                LogError(ex.Message, ex);
            }
        }
        
        protected void grdBkgPkgServiceDetails_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                GridDataItem dataItem = e.Item as GridDataItem;
                if (e.CommandName == RadGrid.ExpandCollapseCommandName && !e.Item.Expanded)
                {
                    GridDataItem parentItem = e.Item as GridDataItem;
                    RadGrid grdServiceGrp = parentItem.ChildItem.FindControl("grdServiceGrp") as RadGrid;
                    grdServiceGrp.MasterTableView.HierarchyDefaultExpanded = false;
                    grdServiceGrp.Rebind();
                }
            }
            catch (SysXException ex)
            {
                LogError(ex.Message, ex);
            }
            catch (System.Exception ex)
            {
                LogError(ex.Message, ex);
            }
        }

        protected void grdServiceGrp_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                GridDataItem dataItem = e.Item as GridDataItem;
                if (e.CommandName == RadGrid.ExpandCollapseCommandName && !e.Item.Expanded)
                {
                    GridDataItem parentItem = e.Item as GridDataItem;
                    RadGrid grdServices = parentItem.ChildItem.FindControl("grdServices") as RadGrid;
                    grdServices.Rebind();
                }
            }
            catch (SysXException ex)
            {
                LogError(ex.Message, ex);
            }
            catch (System.Exception ex)
            {
                LogError(ex.Message, ex);
            }
        }

        #endregion

        #endregion

        #region Private Methods
        /// <summary>
        /// Log Error
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="ex"></param>
        private void LogError(String errorMessage, System.Exception ex)
        {
            _exceptionService.HandleError(errorMessage, ex);
        }
        #endregion

    }
}