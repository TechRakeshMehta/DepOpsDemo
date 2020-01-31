#region Namespaces

#region SystemDefined

using System;
using Microsoft.Practices.ObjectBuilder;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

#endregion

#region UserDefined

using Entity.ClientEntity;
using INTSOF.Utils;
using CoreWeb.Shell;
using Telerik.Web.UI;
using INTSOF.UI.Contract.ComplianceOperation;

#endregion

#endregion

namespace CoreWeb.Search.Views
{
    public partial class ApplicantPortfolioSubscription : BaseUserControl, IApplicantPortfolioSubscriptionView
    {
        #region Variables

        #region Private Variables

        private ApplicantPortfolioSubscriptionPresenter _presenter = new ApplicantPortfolioSubscriptionPresenter();
        private String _viewType;
        #endregion

        #region Public Variables


        #endregion

        #endregion

        #region Properties


        public ApplicantPortfolioSubscriptionPresenter Presenter
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

        public IApplicantPortfolioSubscriptionView CurrentViewContext
        {
            get
            {
                return this;
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

        public Int32 TenantId
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

        /// <summary>
        /// Gets the data for Tree List.
        /// </summary>
        public List<GetPortfolioSubscriptionTree> TreeListDetail
        {
            get;
            set;
        }

        public WorkQueueType WorkQueueType
        {
            get
            {
                Dictionary<String, String> args = new Dictionary<String, String>();
                if (Request.QueryString["args"].IsNotNull())
                {
                    args.ToDecryptedQueryString(Request.QueryString["args"]);

                    if (args.ContainsKey("PageType"))
                    {
                        return (WorkQueueType)Enum.Parse(typeof(WorkQueueType), args["PageType"].ToString(), true);
                    }
                }
                return 0;
            }
        }

        #endregion

        #region Events

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
            }
            Presenter.OnViewLoaded();
        }


        #endregion

        #region Tree List Events

        /// <summary>
        /// Retrieve Block's Feature.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        protected void treeListDetail_NeedDataSource(object sender, TreeListNeedDataSourceEventArgs e)
        {
            try
            {
                this._presenter.GetTreeData();
                treeListDetail.DataSource = TreeListDetail;
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

        protected void treeListDetail_PreRender(object sender, EventArgs e)
        {
            try
            {
                treeListDetail.ExpandAllItems();
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

        protected void treeListDetail_ItemCreated(object sender, TreeListItemCreatedEventArgs e)
        {
            try
            {
                //Below code disables the expand button.
                if (e.Item is TreeListDataItem)
                {
                    Control expandButton = e.Item.FindControl("ExpandCollapseButton");
                    if (expandButton != null)
                    {
                        ((Button)expandButton).Enabled = true;
                        ((Button)expandButton).Visible = true;
                    }

                    TreeListDataItem dataItem = (TreeListDataItem)e.Item;

                    GetPortfolioSubscriptionTree subscriptionDetail = (GetPortfolioSubscriptionTree)dataItem.DataItem;
                    if (subscriptionDetail != null && subscriptionDetail.UICode == "PKG" && WorkQueueType == WorkQueueType.ComprehensiveSearch)
                    {
                        dataItem["ViewDetail"].Visible = true;
                    }
                    else
                    {
                        dataItem["ViewDetail"].Visible = false;
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

        protected void treeListDetail_ItemCommand(object sender, TreeListCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "ViewDetail")
                {
                    Dictionary<String, String> queryString = new Dictionary<String, String>();

                    Int32 packageId = Convert.ToInt32(((TreeListDataItem)(e.Item)).GetDataKeyValue("DataID"));
                    //Int32 orgUserId = Convert.ToInt32((e.Item.FindControl("hdfCatId") as HiddenField).Value);
                    //Int32 selectedPackageSubscriptionId = Convert.ToInt32((e.Item.FindControl("hdfPackSubscriptionId") as HiddenField).Value);
                    Int32 orgUserId = Convert.ToInt32((e.Item as TreeListDataItem)["OrganizationUserID"].Text);
                    Int32 selectedPackageSubscriptionId = Convert.ToInt32((e.Item as TreeListDataItem)["PackageSubscriptionID"].Text);
                    queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "TenantId", Convert.ToString(TenantId) },
                                                                    { "Child", ChildControls.VerificationDetailsNew},
                                                                    {"WorkQueueType",WorkQueueType.ComprehensiveSearch.ToString()},
                                                                    {"PackageId",Convert.ToString(packageId)},
                                                                    {"SelectedPackageSubscriptionId",Convert.ToString(selectedPackageSubscriptionId)},
                                                                    {"ShowOnlyRushOrders","false"},
                                                                    {"ApplicantId",Convert.ToString(orgUserId)}
                                                                 };
                    string url = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                    Response.Redirect(url, true);
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

        #region Methods


        #endregion
    }
}

