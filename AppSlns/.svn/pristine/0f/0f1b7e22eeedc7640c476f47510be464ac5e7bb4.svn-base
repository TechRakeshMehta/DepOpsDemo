using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using Entity.ClientEntity;
using INTSOF.Utils;
using Telerik.Web.UI;

namespace CoreWeb.BkgOperations.Views
{
    public partial class ClientAdminBkgOrderDetailPage : BaseWebPage, IClientAdminOrderDetailView
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private ClientAdminOrderDetailPresenter _presenter = new ClientAdminOrderDetailPresenter();
        private String _viewType;

        #endregion

        #endregion

        #region Properties

        #region Private Properties


        /// <summary>
        /// Gets the current view context.
        /// </summary>
        /// <remarks></remarks>
        private IClientAdminOrderDetailView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        private ClientAdminOrderDetailPresenter Presenter
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

        Int32 IClientAdminOrderDetailView.SelectedTenantId
        {
            get
            {
                return Convert.ToInt32(SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_SELECTED_TENANT_ID));
            }
        }

        Int32 IClientAdminOrderDetailView.CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }


        Int32 IClientAdminOrderDetailView.OrderID
        {
            get
            {
                return Convert.ToInt32(SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_ORDER_ID));
            }
        }

        Int32 IClientAdminOrderDetailView.OrderPkgSvcGroupID
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

        String IClientAdminOrderDetailView.ParentScreenName
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

        Boolean IClientAdminOrderDetailView.IsBkgColorFlagDisable
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsBkgColorFlagDisable"]);
            }
            set
            {
                ViewState["IsBkgColorFlagDisable"] = value;
            }
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
                Dictionary<String, String> queryString = new Dictionary<String, String>();
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

                        if (args.ContainsKey(AppConsts.ORDER_NUMBER))
                        {
                            SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.SESSION_ORDER_NUMBER, Convert.ToString(args[AppConsts.ORDER_NUMBER]));
                        }

                        #region UAT-844
                        if (args.ContainsKey(AppConsts.QUERYSTRING_PARENT_SCREEN_NAME))
                        {
                            CurrentViewContext.ParentScreenName = Convert.ToString(args[AppConsts.QUERYSTRING_PARENT_SCREEN_NAME]);
                        }
                        if (args.ContainsKey(AppConsts.QUERYSTRING_ORDER_PACKAGE_SERVICEGROUP_ID))
                        {
                            CurrentViewContext.OrderPkgSvcGroupID = Convert.ToInt32(args[AppConsts.QUERYSTRING_ORDER_PACKAGE_SERVICEGROUP_ID]);
                        }


                        Dictionary<String, String> queryStringForFirstLoad = new Dictionary<String, String>();
                        args = new Dictionary<String, String>();
                        if (!Request.QueryString["args"].IsNull())
                        {
                            args.ToDecryptedQueryString(Request.QueryString["args"]);
                            if (args.ContainsKey(AppConsts.QUERYSTRING_PARENT_SCREEN_NAME) && args.GetValue(AppConsts.QUERYSTRING_PARENT_SCREEN_NAME).ToString().ToLower() == AppConsts.BKG_ORDER_REVIEW_QUEUE.ToLower())
                            {
                                queryStringForFirstLoad.Add(AppConsts.QUERYSTRING_ORDER_PACKAGE_SERVICEGROUP_ID, CurrentViewContext.OrderPkgSvcGroupID.ToString());
                                queryStringForFirstLoad.Add(AppConsts.QUERYSTRING_PARENT_SCREEN_NAME, CurrentViewContext.ParentScreenName.ToString());
                            }
                        }
                        Int32 menuID = 0;
                        String encryptedQueryString = "";// = queryString.ToEncryptedQueryString();
                        #endregion

                        if (args.ContainsKey("pageName"))
                        {
                            String pageName = args["pageName"];
                            if (pageName.Equals("Service Groups"))
                            {
                                menuID = AppConsts.TWO;
                                queryStringForFirstLoad.Add("menuID", Convert.ToString(menuID));
                                encryptedQueryString = queryStringForFirstLoad.ToEncryptedQueryString();
                                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), "LoadOrderControls", "LoadOrderControls(" + '"' + encryptedQueryString + '"' + ");", true);
                                lstOrderDetailMenuItem.SelectedValue = Convert.ToString(AppConsts.TWO);
                            }
                            else if (pageName.Equals("Order Summary"))
                            {
                                menuID = AppConsts.ONE;
                                queryStringForFirstLoad.Add("menuID", Convert.ToString(menuID));
                                encryptedQueryString = queryStringForFirstLoad.ToEncryptedQueryString();
                                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), "LoadOrderControls", "LoadOrderControls(" + '"' + encryptedQueryString + '"' + ");", true);
                                lstOrderDetailMenuItem.SelectedValue = "Order Summary";
                                lstOrderDetailMenuItem.SelectedValue = Convert.ToString(AppConsts.ONE);
                            }
                        }
                    }
                }

                #region Go-Back
                String goBackChild = "";

                goBackChild = ChildControls.BackgroundOrderSearchQueue;
                lblLinkGoBack.Text = "Back to Order Search Queue";
                #endregion

                if (!this.IsPostBack)
                {
                    Presenter.OnViewInitialized();
                    Presenter.OnViewLoaded();

                    #region IFRAME
                    //String iframURL = "";
                    //src="~/BkgOperations/Pages/BkgOrderDetailMain.aspx?menuID=-1"
                    //queryString = new Dictionary<String, String>
                    //                                            { 
                    //                                               { "menuID",AppConsts.MINUS_ONE.ToString()}
                    //                                            };
                    //

                    Dictionary<String, String> args = new Dictionary<String, String>();
                    if (!Request.QueryString["args"].IsNull())
                    {
                        args.ToDecryptedQueryString(Request.QueryString["args"]);
                        if (args.ContainsKey(AppConsts.QUERYSTRING_PARENT_SCREEN_NAME) && args.GetValue(AppConsts.QUERYSTRING_PARENT_SCREEN_NAME).ToString().ToLower() == AppConsts.BKG_ORDER_REVIEW_QUEUE.ToLower())
                        {
                            goBackChild = ChildControls.BackgroundOrderReviewQueue;
                            lblLinkGoBack.Text = "Back to Order Review Queue";
                            //queryString.Add(AppConsts.QUERYSTRING_ORDER_PACKAGE_SERVICEGROUP_ID, CurrentViewContext.OrderPkgSvcGroupID.ToString());
                            //queryString.Add(AppConsts.QUERYSTRING_PARENT_SCREEN_NAME, CurrentViewContext.ParentScreenName.ToString());
                        }

                    }

                    //iframURL = String.Format("~/BkgOperations/Pages/ClientAdminBkgOrderDetailPageMain.aspx?args={0}", queryString.ToEncryptedQueryString());
                    //Iframe1.Src = iframURL;
                    #endregion
                }

                #region Go-Back
                Dictionary<String, String> goBackQueryString = new Dictionary<String, String>();
                goBackQueryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "Child", goBackChild},
                                                                    { "PageType",BkgOrderDetailScreenType.ClientAdminBkgOrderDetail.GetStringValue()}
                                                               };

                String _url = String.Format("~/BkgOperations/Default.aspx?ucid={0}&args={1}", _viewType, goBackQueryString.ToEncryptedQueryString());
                lnkGoBack.HRef = _url;
                #endregion

                BindOrderDetailMenu();
                BindOrderDetailHeader();
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
                base.Title = "Order Details";
                base.SetPageTitle("Order Details");
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

        #region Dropdown Events

        protected void lstOrderDetailMenuItem_ItemDataBound(object sender, Telerik.Web.UI.RadListBoxItemEventArgs e)
        {
            try
            {
                RadListBoxItem item = e.Item as RadListBoxItem;
                Int32 menuID = Convert.ToInt32(item.Value);
                HtmlAnchor lnkOrderDetailMenuItem = (HtmlAnchor)e.Item.FindControl("lnkOrderDetailMenuItem");
                //lnkOrderDetailMenuItem.Attributes.Add("onclick", "LoadOrderControls(" + menuID + ");");

                #region UAT-844
                Dictionary<String, String> queryString = new Dictionary<String, String>();
                Dictionary<String, String> args = new Dictionary<String, String>();
                queryString = new Dictionary<String, String> { { "menuID", Convert.ToString(menuID) } };
                if (!Request.QueryString["args"].IsNull())
                {
                    args.ToDecryptedQueryString(Request.QueryString["args"]);
                    if (args.ContainsKey(AppConsts.QUERYSTRING_PARENT_SCREEN_NAME) && args.GetValue(AppConsts.QUERYSTRING_PARENT_SCREEN_NAME).ToString().ToLower() == AppConsts.BKG_ORDER_REVIEW_QUEUE.ToLower())
                    {
                        queryString.Add(AppConsts.QUERYSTRING_ORDER_PACKAGE_SERVICEGROUP_ID, CurrentViewContext.OrderPkgSvcGroupID.ToString());
                        queryString.Add(AppConsts.QUERYSTRING_PARENT_SCREEN_NAME, CurrentViewContext.ParentScreenName.ToString());
                    }
                }

                String encryptedQueryString = queryString.ToEncryptedQueryString();
                lnkOrderDetailMenuItem.Attributes.Add("onclick", "LoadOrderControls(" + '"' + encryptedQueryString + '"' + ");");                
                #endregion
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

        #endregion

        #region Methods

        void BindOrderDetailMenu()
        {
            if (lstOrderDetailMenuItem.Items.Count == AppConsts.NONE)
            {
                Dictionary<Int32, String> menuItems = new Dictionary<Int32, String>();
                menuItems.Add(AppConsts.ONE, "Order Summary");
                menuItems.Add(AppConsts.TWO, "Service Group");
                menuItems.Add(AppConsts.THREE, "Order History");
                menuItems.Add(AppConsts.FOUR, "Disclosure and Authorization");
                //UAT - 1111 - As a client admin, I should be able to see Residential History on the background order details screen
                menuItems.Add(AppConsts.FIVE, "Residential History");
                lstOrderDetailMenuItem.DataSource = menuItems;
                lstOrderDetailMenuItem.DataTextField = "Value";
                lstOrderDetailMenuItem.DataValueField = "Key";
                lstOrderDetailMenuItem.DataBind();
            }
        }

        void BindOrderDetailHeader()
        {
            //Bind data of Order Ticket
            OrderDetailHeaderInfo orderDetailHeaderInfo = Presenter.GetOrderDetailHeaderInfo();
            lnkOrderNo.Text = '#' + orderDetailHeaderInfo.OrderNumber.ToString();
            lblCreated.Text = orderDetailHeaderInfo.OrderDate.ToString("MM/dd/yyyy");
            lblUserName.Text = orderDetailHeaderInfo.ApplicantName.HtmlEncode();
            lblGender.Text = orderDetailHeaderInfo.Gender;
            if (orderDetailHeaderInfo.StatusType.ToString().ToLower() == "completed" && orderDetailHeaderInfo.InstitutionColorStatusID.IsNotNull())
            {
                imgOrderFlag.ImageUrl = "~/" + orderDetailHeaderInfo.InstitutionColorStatus;
                //UAT-2439,Client Admin screen updates for text to icon
                imgOrderFlag.AlternateText = String.Concat(orderDetailHeaderInfo.InstitutionColorStatusTooltip, " color flag.");
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
            //lblAmount.Text = '$' + orderDetailHeaderInfo.TotalPrice.ToString();
            lblAmount.Text = String.Format(CultureInfo.CurrentCulture, "{0:$#,0.00}", orderDetailHeaderInfo.TotalPrice);

            lblStatus.Text = orderDetailHeaderInfo.StatusType;
            lblType.Text = orderDetailHeaderInfo.PaymentType.Length > 22 ? (orderDetailHeaderInfo.PaymentType.Substring(0, 20) + "...")
                                                                         : orderDetailHeaderInfo.PaymentType;
            lblType.ToolTip = orderDetailHeaderInfo.PaymentType;

            lblPaymentStatus.Text = orderDetailHeaderInfo.PaymentStatus.Length > 22 ? (orderDetailHeaderInfo.PaymentStatus.Substring(0, 20) + "...")
                                                                                    : orderDetailHeaderInfo.PaymentStatus;
            lblPaymentStatus.ToolTip = orderDetailHeaderInfo.PaymentStatus;

            //lblPaymentStatus.Text = orderDetailHeaderInfo.PaymentStatus;
            //lblType.Text = orderDetailHeaderInfo.PaymentType;

            //if (orderDetailHeaderInfo.PaymentType.Length > 30)
            //{
            //    lblType.Text = orderDetailHeaderInfo.PaymentType.Substring(0, 27) + "...";
            //    lblType.ToolTip = orderDetailHeaderInfo.PaymentType;
            //    lblPaymentStatus.Text = orderDetailHeaderInfo.PaymentStatus.Substring(0, 27) + "...";
            //    lblPaymentStatus.ToolTip = orderDetailHeaderInfo.PaymentStatus;
            //}

            #region UAT-1075 WB:Admin Granular permissions for color flag and Result PDF
            Presenter.GetGranularPermissionForClientAdmins();
            if (CurrentViewContext.IsBkgColorFlagDisable)
            {
                dvBkgOrderColorFlag.Visible = false;
            }
            else
            {
                dvBkgOrderColorFlag.Visible = true;
            }
            #endregion

        }

        #endregion

    }
}