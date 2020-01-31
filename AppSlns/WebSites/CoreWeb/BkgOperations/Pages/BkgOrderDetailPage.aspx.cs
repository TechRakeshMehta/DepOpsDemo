using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CoreWeb.Shell;
using Entity.ClientEntity;
using INTSOF.Utils;
using Telerik.Web.UI;
using System.Globalization;


namespace CoreWeb.BkgOperations.Views
{
    public partial class BkgOrderDetailPage : BaseWebPage, IBkgOrderDetailView
    {

        #region Variables

        Int32 _selectedTenantID;
        Int32 _orderID;
        private String _viewType;
        private BkgOrderDetailPresenter _presenter = new BkgOrderDetailPresenter();
        String QueueType = String.Empty;
        #endregion

        #region Events

        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                base.Title = "Order Detail";
                base.SetPageTitle("Order Detail");
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                if (!SysXWebSiteUtils.SessionService.IsNull())
                {
                    if (!HttpContext.Current.IsNull() && !HttpContext.Current.Items.Contains(AppConsts.SESSION_ORDER_ID))
                    {
                        Dictionary<String, String> args = new Dictionary<String, String>();
                        args.ToDecryptedQueryString(Request.QueryString["args"]);
                        if (args.ContainsKey("OrderId"))
                        {
                            SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.SESSION_ORDER_ID, Convert.ToInt32(args["OrderId"]));
                        }
                        if (args.ContainsKey("SelectedTenantId"))
                        {
                            SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.SESSION_SELECTED_TENANT_ID, Convert.ToInt32(args["SelectedTenantId"]));
                        }
                        if (args.ContainsKey(AppConsts.QUERYSTRING_PARENT_SCREEN_NAME))
                        {
                            CurrentViewContext.ParentScreenName = Convert.ToString(args[AppConsts.QUERYSTRING_PARENT_SCREEN_NAME]);
                        }
                        if (args.ContainsKey(AppConsts.QUERYSTRING_ORDER_PACKAGE_SERVICEGROUP_ID))
                        {
                            CurrentViewContext.OrderPkgSvcGroupID = Convert.ToInt32(args[AppConsts.QUERYSTRING_ORDER_PACKAGE_SERVICEGROUP_ID]);
                        }
                        if (args.ContainsKey(AppConsts.ORDER_NUMBER))
                        {
                            SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.SESSION_ORDER_NUMBER, Convert.ToString(args[AppConsts.ORDER_NUMBER]));
                        }
                        //UAT-2117:"Continue" button behavior
                        if (args.ContainsKey(AppConsts.SOURCE_SCREEN_NAME) && args[AppConsts.SOURCE_SCREEN_NAME] == AppConsts.ORDER_DETAIL_FOR_SERVICE_ITEM_SUPPLEMENT
                             && args.ContainsKey(AppConsts.MENU_ID))
                        {
                            CurrentViewContext.MenuID = Convert.ToInt32(args[AppConsts.MENU_ID]);
                        }
                        if (args.ContainsKey(AppConsts.QUERYSTRING_SUPPLEMENT_AUTOMATION_STATUS_ID))
                        {
                            CurrentViewContext.SupplementAutomationStatusID = Convert.ToInt32(args[AppConsts.QUERYSTRING_SUPPLEMENT_AUTOMATION_STATUS_ID]);
                        }
                        //UAT-2971
                        if (args.ContainsKey("WorkQueueType"))
                        {
                            QueueType = Convert.ToString(args["WorkQueueType"]);
                        }
                        if (args.ContainsKey("OrganizationUserId"))
                        {
                            SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.APPLICANT_ORGANIZATION_USER_ID, Convert.ToInt32(args["OrganizationUserId"]));
                        }
                        if (args.ContainsKey("UserId"))
                        {
                            SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.APPLICANT_GUID, Convert.ToString(args["UserId"]));
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

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Dictionary<String, String> queryString = new Dictionary<String, String>();
                String child = "";

                child = ChildControls.BackgroundOrderSearchQueue;
                lblLinkGoBack.Text = "Back to Order Search Queue";

                if (!this.IsPostBack)
                {
                    Presenter.OnViewInitialized();
                    Presenter.OnViewLoaded();

                    #region IFRAME
                    String iframURL = "";
                    //src="~/BkgOperations/Pages/BkgOrderDetailMain.aspx?menuID=-1"
                    //UAT-2117:"Continue" button behavior
                    //Redirect on "Vendor Services" menu if supplement order is not placed and no success indicator is applicable on order.
                    Int32 menuID = CurrentViewContext.MenuID == AppConsts.NONE ? AppConsts.MINUS_ONE : CurrentViewContext.MenuID;
                    queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "menuID",menuID.ToString()}
                                                                 };

                    Dictionary<String, String> args = new Dictionary<String, String>();
                    if (!Request.QueryString["args"].IsNull())
                    {
                        args.ToDecryptedQueryString(Request.QueryString["args"]);
                        if (args.ContainsKey(AppConsts.QUERYSTRING_PARENT_SCREEN_NAME) && args.GetValue("ParentScreenName").ToString().ToLower() == AppConsts.BKG_ORDER_REVIEW_QUEUE.ToLower())
                        {
                            child = ChildControls.BackgroundOrderReviewQueue;
                            lblLinkGoBack.Text = "Back to Order Review Queue";
                            queryString.Add(AppConsts.QUERYSTRING_ORDER_PACKAGE_SERVICEGROUP_ID, CurrentViewContext.OrderPkgSvcGroupID.ToString());
                            queryString.Add(AppConsts.QUERYSTRING_PARENT_SCREEN_NAME, CurrentViewContext.ParentScreenName.ToString());
                            queryString.Add(AppConsts.QUERYSTRING_SUPPLEMENT_AUTOMATION_STATUS_ID, CurrentViewContext.SupplementAutomationStatusID.ToString());
                        }
                    }

                    iframURL = String.Format("~/BkgOperations/Pages/BkgOrderDetailMain.aspx?args={0}", queryString.ToEncryptedQueryString());
                    Iframe1.Src = iframURL;
                    #endregion
                }

                //UAT-2971
                if (QueueType == WorkQueueType.SupportPortalDetail.ToString())
                {
                    lblLinkGoBack.Text = "Back to Support Portal Detail";


                   String tenantId =  Convert.ToString(SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_SELECTED_TENANT_ID));
                   String OrganizationUserId = Convert.ToString(SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.APPLICANT_ORGANIZATION_USER_ID));
                   String UserId = Convert.ToString(SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.APPLICANT_GUID));
                   Dictionary<String, String> queryStringForlink = new Dictionary<String, String>
                                                                 { 

                                                                    { "Child", ChildControls.SupportPortalDetails},
                                                                    { "PageType",QueueType.ToString()},
                                                                    { "OrganizationUserId",OrganizationUserId},
                                                                    {"TenantId",tenantId},
                                                                    {"UserId",UserId}
                                                               };
                   String _url = String.Format("~/SearchUI/Default.aspx?ucid={0}&args={1}", _viewType, queryStringForlink.ToEncryptedQueryString());
                    lnkGoBack.HRef = _url;
                }
                else
                {
                    //querystring used for go to back link
                    Dictionary<String, string> queryStringForlink = new Dictionary<String, String>
                                                                 { 

                                                                    { "Child", child},
                                                                    { "PageType",BkgOrderDetailScreenType.AdminBkgOrderDetail.GetStringValue()}
                                                               };
                    String _url = String.Format("~/BkgOperations/Default.aspx?ucid={0}&args={1}", _viewType, queryStringForlink.ToEncryptedQueryString());
                    lnkGoBack.HRef = _url;
                }
                //BackgroundOrderSearchQueue
                BindOrderDetailMenu();
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

        protected void lstOrderDetailMenuItem_ItemDataBound(object sender, Telerik.Web.UI.RadListBoxItemEventArgs e)
        {
            try
            {
                RadListBoxItem item = e.Item as RadListBoxItem;
                Int32 menuID = ((Entity.ClientEntity.OrderDetailMenuItem)(e.Item.DataItem)).MenuID;
                HtmlAnchor lnkOrderDetailMenuItem = (HtmlAnchor)e.Item.FindControl("lnkOrderDetailMenuItem");

                Dictionary<String, String> queryString = new Dictionary<String, String>();
                Dictionary<String, String> args = new Dictionary<String, String>();
                queryString = new Dictionary<String, String> { { "menuID", Convert.ToString(menuID) } };
                if (!Request.QueryString["args"].IsNull())
                {
                    args.ToDecryptedQueryString(Request.QueryString["args"]);
                    if (args.ContainsKey(AppConsts.QUERYSTRING_PARENT_SCREEN_NAME) && args.GetValue("ParentScreenName").ToString().ToLower() == AppConsts.BKG_ORDER_REVIEW_QUEUE.ToLower())
                    {
                        queryString.Add(AppConsts.QUERYSTRING_ORDER_PACKAGE_SERVICEGROUP_ID, CurrentViewContext.OrderPkgSvcGroupID.ToString());
                        queryString.Add(AppConsts.QUERYSTRING_PARENT_SCREEN_NAME, CurrentViewContext.ParentScreenName.ToString());
                        queryString.Add(AppConsts.QUERYSTRING_SUPPLEMENT_AUTOMATION_STATUS_ID, CurrentViewContext.SupplementAutomationStatusID.ToString());
                    }
                }
                //String iframeURL = String.Format("~/BkgOperations/Pages/BkgOrderDetailMain.aspx?args={0}", queryString.ToEncryptedQueryString());
                String encryptedQueryString = queryString.ToEncryptedQueryString();
                lnkOrderDetailMenuItem.Attributes.Add("onclick", "NavigateTo(" + '"' + encryptedQueryString + '"' + ");");
                //    lnkOrderDetailMenuItem.Attributes.Add("onclick", "NavigateTo(" + menuID + ");");

                //UAT-2117:"Continue" button behavior
                if (CurrentViewContext.MenuID != AppConsts.NONE && CurrentViewContext.MenuID == menuID)
                {
                    item.Selected = true;
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

        #region Properties
        /// <summary>
        /// Gets the current view context.
        /// </summary>
        /// <remarks></remarks>
        public IBkgOrderDetailView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public BkgOrderDetailPresenter Presenter
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

        Int32 IBkgOrderDetailView.SelectedTenantId
        {
            get
            {
                return Convert.ToInt32(SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_SELECTED_TENANT_ID));
            }
        }

        Int32 IBkgOrderDetailView.CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }


        Int32 IBkgOrderDetailView.OrderID
        {
            get
            {
                return Convert.ToInt32(SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_ORDER_ID));
            }
        }

        Int32 IBkgOrderDetailView.OrderPkgSvcGroupID
        {
            get
            {
                //return Convert.ToInt32(SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_ORDER_PACKAGE_SERVICEGROUP_ID));
                return Convert.ToInt32(ViewState["OrderPkgSvcGroupID"]);
            }
            set
            {
                ViewState["OrderPkgSvcGroupID"] = value;
            }
        }

        String IBkgOrderDetailView.ParentScreenName
        {
            get
            {
                //return SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_PARENT_SCREEN_NAME).ToString();
                return Convert.ToString(ViewState["ParentScreenName"]);
            }
            set
            {
                ViewState["ParentScreenName"] = value;
            }
        }

        String IBkgOrderDetailView.OrderNumber
        {
            get
            {
                return Convert.ToString(SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_ORDER_NUMBER));
            }
        }

        Int32 IBkgOrderDetailView.SupplementAutomationStatusID
        {
            get
            {
                return Convert.ToInt32(ViewState["SupplementAutomationStatusID"]);
            }
            set
            {
                ViewState["SupplementAutomationStatusID"] = value;
            }
        }

        #region UAT-2117:"Continue" button behavior
        Int32 IBkgOrderDetailView.MenuID { get; set; }
        #endregion

        #endregion

        #region Methods

        void BindOrderDetailMenu()
        {
            OrderDetailMain orderDetailMain = new OrderDetailMain();
            orderDetailMain = Presenter.GetOrderDetailMenuItem();

            if (lstOrderDetailMenuItem.Items.Count == AppConsts.NONE)
            {
                lstOrderDetailMenuItem.DataSource = orderDetailMain.OrderDetailMenuItem;
                lstOrderDetailMenuItem.DataTextField = "MenuName";
                lstOrderDetailMenuItem.DataValueField = "MenuID";
                lstOrderDetailMenuItem.DataBind();
            }
            //Bind data of Order Ticket
            OrderDetailHeaderInfo orderDetailHeaderInfo = Presenter.GetOrderDetailMenuItem().OrderDetailHeaderInfo;
            lnkOrderNo.Text = ('#' + orderDetailHeaderInfo.OrderNumber.ToString());
            lblCreated.Text = orderDetailHeaderInfo.OrderDate.ToString("MM/dd/yyyy");
            lblUserName.Text = orderDetailHeaderInfo.ApplicantName.HtmlEncode();
            lblGender.Text = orderDetailHeaderInfo.Gender;

            if (orderDetailHeaderInfo.InstitutionColorStatusID.IsNotNull())
            {
                imgOrderFlag.Visible = true;
                imgOrderFlag.ImageUrl = "~/" + orderDetailHeaderInfo.InstitutionColorStatus;
            }
            else
            {
                imgOrderFlag.Style.Add("display", "none");
            }

            //lblDOB.Text = orderDetailHeaderInfo.DOB;
            if (!orderDetailHeaderInfo.DOB.IsNullOrEmpty())
            {
                lblDOB.Text = (DateTime.ParseExact(orderDetailHeaderInfo.DOB, "dd-MM-yyyy", null)).ToString("MM/dd/yyyy");
            }
            else
            {
                lblDOB.Text = String.Empty;
            }
            lblAmount.Text = String.Format(CultureInfo.CurrentCulture, "{0:$#,0.00}", orderDetailHeaderInfo.TotalPrice);
            lblStatus.Text = orderDetailHeaderInfo.StatusType;

            lblType.Text = orderDetailHeaderInfo.PaymentType.Length > 22 ? (orderDetailHeaderInfo.PaymentType.Substring(0, 20) + "...").HtmlEncode()
                                                                         : orderDetailHeaderInfo.PaymentType;
            lblType.ToolTip = orderDetailHeaderInfo.PaymentType;

            lblPaymentStatus.Text = orderDetailHeaderInfo.PaymentStatus.Length > 22 ? (orderDetailHeaderInfo.PaymentStatus.Substring(0, 20) + "...").HtmlEncode()
                                                                                    : orderDetailHeaderInfo.PaymentStatus;
            lblPaymentStatus.ToolTip = orderDetailHeaderInfo.PaymentStatus;
        }

        #endregion
    }
}