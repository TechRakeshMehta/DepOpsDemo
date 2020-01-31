#region Namespaces

#region SystemDefined

using System;
using Microsoft.Practices.ObjectBuilder;
using System.Data.Entity.Core.Objects;
using System.Collections.Generic;
using CoreWeb.Shell;
using System.Web.Services;
using System.Linq;
using System.Web.UI.WebControls;

#endregion

#region UserDefined

using Entity.ClientEntity;
using INTSOF.Utils;
using INTSOF.UI.Contract.ComplianceManagement;
using Telerik.Web.UI;
using INTERSOFT.WEB.UI.WebControls;
using System.Threading;
using INTSOF.UI.Contract.ComplianceOperation;

#endregion

#endregion

namespace CoreWeb.Search.Views
{
    public partial class ApplicantPortfolioOrderHistory : BaseUserControl, IApplicantPortfolioOrderHistoryView
    {
        #region Variables

        #region Private Variables

        private ApplicantPortfolioOrderHistoryPresenter _presenter = new ApplicantPortfolioOrderHistoryPresenter();
        private Int32 _currentUserTenantId;
        private String _viewType;

        #endregion

        #region Public Variables
        #endregion

        #endregion

        #region Properties

        #region Private Properties


        #endregion

        #region Public Properties


        public ApplicantPortfolioOrderHistoryPresenter Presenter
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
            get { return base.CurrentUserId; }
        }

        public Int32 OrganizationUserId
        {
            get
            {
                Dictionary<String, String> args = new Dictionary<String, String>();
                if (Request.QueryString["args"].IsNotNull())
                {
                    args.ToDecryptedQueryString(Request.QueryString["args"]);

                    if (args.ContainsKey("OrganizationUserId"))
                    {
                        return (Convert.ToInt32(args["OrganizationUserId"]));
                    }
                }
                return base.CurrentUserId;
            }
        }

        public Int32 TenantID
        {
            get
            {
                Dictionary<String, String> args = new Dictionary<String, String>();
                if (Request.QueryString["args"].IsNotNull())
                {
                    args.ToDecryptedQueryString(Request.QueryString["args"]);

                    if (args.ContainsKey("TenantId"))
                    {
                        return (Convert.ToInt32(args["TenantId"]));
                    }
                }
                return 0;
            }
        }

        public List<vwOrderDetail> ListOrderDetail
        {
            get;
            set;
        }

        public String SourceScreen
        {
            get;
            set;
        }

        public String CancelledPackages
        {
            get;
            set;
        }

        public Int32 OrderID
        {
            get;
            set;
        }

        #endregion

        #endregion

        #region Events

        #region Page Events

        protected override void OnInit(EventArgs e)
        {
            try
            {
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

        /// <summary>
        /// Loads the page ManageAssignmentProperties.aspx.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                if (!this.IsPostBack)
                {
                    Presenter.OnViewInitialized();
                    Dictionary<String, String> args = new Dictionary<String, String>();
                    if (!Request.QueryString["args"].IsNull())
                    {
                        args.ToDecryptedQueryString(Request.QueryString["args"]);
                    }
                    if (args.ContainsKey("PageType"))
                    {
                        SourceScreen = Convert.ToString(args["PageType"]);
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

        #region Grid Events

        protected void grdOrderHistory_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetOrderDetailList();
                grdOrderHistory.DataSource = ListOrderDetail;
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

        protected void grdOrderHistory_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("ViewDetail"))
                {
                    Dictionary<String, String> queryString = new Dictionary<String, String>();
                    String orderId = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrderId"].ToString();
                    queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "SelectedTenantId", Convert.ToString(TenantID) },
                                                                    { "Child", ChildControls.OrderPaymentDetails},
                                                                    { "OrderId", orderId},
                                                                    {"ShowApproveRejectButtons",false.ToString()}
                                                                 };
                    string url = String.Format("~/ComplianceOperations/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                    Response.Redirect(url, true);
                }

                if (e.CommandName == "InfoViewer")
                {
                    OrderID = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrderId"]);
                    Presenter.GetCancelledPackageByOrderID();
                    if (!CancelledPackages.IsNullOrEmpty())
                    {
                        hdnPopupInfoHtml.Value = CancelledPackages;
                        System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "showInfoPopup();", true);
                    }

                }
            }
            //Do not log thread abort exception if it is caused by Response.Redirect or Response.End
            //catch (ThreadAbortException thex)
            //{
            //    //You can ignore this 
            //}
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

        protected void grdOrderHistory_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
            {
                GridDataItem dataItem = (GridDataItem)e.Item;
                vwOrderDetail orderDetail = e.Item.DataItem as vwOrderDetail;
                dataItem["ArchiveState"].Text = Presenter.SetArchiveStateText(orderDetail);
                Label lblNvrCncl = (Label)e.Item.FindControl("lblNvrCncl");
                RadButton btnCancelStatus = (RadButton)e.Item.FindControl("btnCancelStatus");
                if (lblNvrCncl.IsNotNull() && btnCancelStatus.IsNotNull() && orderDetail.CancellationStatus == "Never Cancelled")
                {
                    lblNvrCncl.Visible = true;
                    btnCancelStatus.Visible = false;
                }
                else
                {
                    btnCancelStatus.Visible = true;
                    lblNvrCncl.Visible = false;
                }
            }
        }

        #endregion


        #region Button Events



        #endregion

        #endregion

        #region Methods

        #region Public Methods

        #endregion

        #region Private Methods



        #endregion

        #endregion

    }
}

