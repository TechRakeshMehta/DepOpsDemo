using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using Telerik.Web.UI;

namespace CoreWeb.Search.Views
{
    public partial class ApplicantMessageGrid : BaseUserControl, IApplicantMessageGridView
    {

        #region Variables

        #region Private Variables

        private ApplicantMessageGridPresenter _presenter = new ApplicantMessageGridPresenter();
        private String _viewType;

        #endregion

        #region Public Variables


        #endregion

        #endregion

        /// <summary>
        /// To set or get Search Instance Id
        /// </summary>
        public Int32 SearchInstanceId
        {
            get
            {
                Dictionary<String, String> args = new Dictionary<String, String>();
                if (Request.QueryString["args"].IsNotNull())
                {
                    args.ToDecryptedQueryString(Request.QueryString["args"]);

                    if (args.ContainsKey("SearchInstanceId"))
                    {
                        return (Convert.ToInt32(args["SearchInstanceId"]));
                    }
                }
                return 0;
            }
        }

        /// <summary>
        /// To set or get Search Instance Id
        /// </summary>
        public Int32 MasterPageTabIndex
        {
            get
            {
                Dictionary<String, String> args = new Dictionary<String, String>();
                if (Request.QueryString["args"].IsNotNull())
                {
                    args.ToDecryptedQueryString(Request.QueryString["args"]);

                    if (args.ContainsKey("MasterPageTabIndex"))
                    {
                        return (Convert.ToInt32(args["MasterPageTabIndex"]));
                    }
                }
                return 0;
            }
        }

        public Int32 selectedTenantId
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
                return 1;
            }
        }

        public ApplicantMessageGridPresenter Presenter
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

        #region Properties

        public List<ApplicantMessageDetails> MessageDetailData { get; set; }

        /// <summary>
        /// CurrentPageIndex</summary>
        /// <value>
        /// Gets or sets the value for CurrentPageIndex.</value>
        public Int32 CurrentPageIndex
        {
            get
            {
                return grdLstMsg.MasterTableView.CurrentPageIndex + 1;
            }
            set
            {
                grdLstMsg.MasterTableView.CurrentPageIndex = value - 1;
            }
        }

        public Int32 PageSize
        {
            get
            {
                // Maximum 100 record allowed from DB. 
                //return grdLstMsg.PageSize > 100 ? 100 : grdLstMsg.PageSize;
                return grdLstMsg.PageSize;
            }
        }

        public Int32 GridTotalCount
        {
            set
            {
                grdLstMsg.VirtualItemCount = value;
                grdLstMsg.MasterTableView.VirtualItemCount = value;
            }
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

        #endregion

        /// <summary>
        /// OnInit event to set page titles
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            try
            {
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                base.OnInit(e);
                base.SetPageTitle("Applicant Message Detail");
                base.Title = "Applicant Messages";
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
                RouteBackSearch();
                RoutePageBack();
            }
        }

        /// <summary>
        /// Retrieve Block's Feature.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        protected void grdVerificationItemData_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetQueue();
                grdLstMsg.DataSource = MessageDetailData;
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
        /// To route Page Back
        /// </summary>
        /// <param name="showSuccessMessage"></param>
        private void RoutePageBack()
        {
            String childcontrolPath = null;
            if (SearchInstanceId != 0)
            {
                childcontrolPath = ChildControls.ApplicantPortFolioSearchCopyPage;
            }
            else
            {
                childcontrolPath = ChildControls.ApplicantPortFolioSearchPage;
            }
            Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "Child", childcontrolPath },
                                                                    {"CancelClicked", "CancelClicked" },
                                                                    {"SearchInstanceId", SearchInstanceId.ToString() },
                                                                    { "MasterPageTabIndex", MasterPageTabIndex.ToString()}
                                                                 };
            string url = String.Format("~/SearchUI/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
            lnkGoBack.HRef = url;
        }

        private void RouteBackSearch() 
        {
            Dictionary<String, String> queryString = new Dictionary<String, String>();
            queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "TenantId", Convert.ToString(selectedTenantId) },
                                                                    { "Child", ChildControls.ApplicantPortfolioDetailPage},
                                                                    { "OrganizationUserId", Convert.ToString(OrganizationUserId)},
                                                                    {"PageType", "ApplicantPortfolioSearch"}
                                                                 };
            string url = String.Format("~/SearchUI/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
            lnkDetail.HRef = url;
        }


    }
}