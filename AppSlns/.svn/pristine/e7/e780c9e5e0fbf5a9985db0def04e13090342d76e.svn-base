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
using System.Web.UI.HtmlControls;
using System.Threading;
using INTSOF.UI.Contract.ComplianceOperation;
using CoreWeb.IntsofSecurityModel;


#endregion

#endregion

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class PackageSubscription : BaseUserControl, IPackageSubscriptionView
    {
        #region Variables

        #region Private Variables

        private PackageSubscriptionPresenter _presenter = new PackageSubscriptionPresenter();
        private Int32 _currentUserTenantId;
        private String _viewType;
        private List<Int32> _recentPackagesIDList = null;
        private Boolean _isFalsePostBack = false;
        #endregion

        #region Public Variables
        #endregion

        #endregion

        #region Properties

        #region Private Properties


        #endregion

        #region Public Properties


        public PackageSubscriptionPresenter Presenter
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

        public Int32 CurrentUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        public Int32 CurrentUserTenantId
        {
            //get
            //{
            //    if (_currentUserTenantId == 0)
            //    {
            //        _currentUserTenantId = Convert.ToInt32(ViewState["TenantID"]);
            //    }
            //    return _currentUserTenantId;
            //}
            //set
            //{
            //    ViewState["TenantID"] = value;
            //}

            get
            {
                if (_currentUserTenantId == 0)
                {
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    if (user.IsNotNull())
                    {
                        _currentUserTenantId = user.TenantId.HasValue ? user.TenantId.Value : 0;
                    }
                }
                return _currentUserTenantId;
            }
            set { _currentUserTenantId = value; }
        }

        public List<vwSubscription> ListSubscription
        {
            get;
            set;
        }

        public Int32 ClientSettingBeforeExpiry
        {
            get
            {
                if (Convert.ToInt32(ViewState["BeforeExpirySettingValue"]) != 0)
                {
                    return Convert.ToInt32(ViewState["BeforeExpirySettingValue"]);
                }
                return 30;
            }
            set
            {
                ViewState["BeforeExpirySettingValue"] = value;
            }
        }

        public Int32 ClientSettingAfterExpiry
        {
            get
            {
                if (Convert.ToInt32(ViewState["AfterExpirySettingValue"]) != 0)
                {
                    return Convert.ToInt32(ViewState["AfterExpirySettingValue"]);
                }
                return 30;
            }
            set
            {
                ViewState["AfterExpirySettingValue"] = value;
            }
        }

        public List<Int32> RecentPackagesIDList
        {
            get
            {
                if (_recentPackagesIDList.IsNull())
                {
                    Presenter.GetRecentPackagesIDList();
                }
                return _recentPackagesIDList;
            }
            set
            {
                _recentPackagesIDList = value;
            }
        }

        public Boolean Visiblity
        {
            get
            {
                if (ViewState["Visiblity"] != null)
                    return (Convert.ToBoolean(ViewState["Visiblity"]));
                return true;
            }
            set
            {
                ViewState["Visiblity"] = value;
            }
        }

        public String ControlUseType
        {
            get
            {
                if (ViewState["ControlUseType"] != null)
                    return (Convert.ToString(ViewState["ControlUseType"]));
                return String.Empty;
            }
            set
            {
                ViewState["ControlUseType"] = value;
            }
        }
       public  List<SubscriptionFrequency> lstSubscriptionFrequencies
        {
            get;set;
        }

        public Boolean IsFalsePostBack
        {
            get
            {
                return _isFalsePostBack;
            }
            set
            {
                _isFalsePostBack = value;
            }
        }

        #region UAT-977: Additional work towards archive ability
        public Boolean IsArchivedSubscription { get; set; }
        public Boolean IsExpiredSubscription { get; set; }
        #endregion

        //UAT-1261: WB: As an ADB admin, I should be able to "login" as any student to see what they see.
        public Int32 OrgUsrID
        {
            get
            {
                if (!System.Web.HttpContext.Current.Session["AppViewAdminOrgUsrID"].IsNullOrEmpty())
                    return Convert.ToInt32(System.Web.HttpContext.Current.Session["AppViewAdminOrgUsrID"]);
                else
                    return CurrentUserId;
            }
        }

        public Boolean IsFromRenewOrderPopup
        {
            get
            {
                if (ViewState["IsFromRenewOrderPopup"] != null)
                    return (Convert.ToBoolean(ViewState["IsFromRenewOrderPopup"]));
                return false;
            }
            set
            {
                ViewState["IsFromRenewOrderPopup"] = value;
            }
        }

        #endregion

        #endregion

        #region Events

        #region Page Events

        protected override void OnInit(EventArgs e)
        {
            try
            {
                //change done for applicant dashboard redesign.
                if (Visiblity.IsNullOrEmpty() || Visiblity == true)
                {
                    // _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                    base.OnInit(e);
                    if (ControlUseType != AppConsts.DASHBOARD)
                    {
                        base.Title = "Package Subscriptions";
                        //base.SetPageTitle("Package Subscriptions");
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

        /// <summary>
        /// Loads the page ManageAssignmentProperties.aspx.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //change done for applicant dashboard redesign.
                if (Visiblity.IsNullOrEmpty() || Visiblity == true)
                {
                    _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                    if (!this.IsPostBack)
                    {
                        //CurrentUserTenantId = Presenter.GetTenantId();
                        Presenter.OnViewInitialized();
                    }
                    Presenter.GetSubscriptionFrequencies();
                    //Presenter.GetClientSettingDetails();
                    if (ControlUseType != AppConsts.DASHBOARD)
                    {
                        base.SetPageTitle("Package Subscriptions");
                    }
                    if (IsFalsePostBack)
                    {
                        grdPurchasedSubscription.Rebind();
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

        protected void grdPurchasedSubscription_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                //change done for applicant dashboard redesign.
                if (Visiblity.IsNullOrEmpty() || Visiblity == true)
                {
                    Presenter.GetSubscriptionList();

                    //UAT-2003: Add ability to extend/renew when clicking "place order"
                    if (IsFromRenewOrderPopup)
                    {
                        List<vwSubscription> packageSubscriptionList = new List<vwSubscription>();
                        packageSubscriptionList.AddRange(ListSubscription);

                        foreach (var packageSubscription in packageSubscriptionList)
                        {
                            Boolean isValidRenewSubscription = false;
                            Int32 remaingDays = 0;
                            Int32? archivalGracePeriod = packageSubscription.ArchivalGracePeriod;
                            SetSubscriptionExpiredAndArchiveStatus(packageSubscription.ExpiryDate.Value, packageSubscription.ArchiveStateCode);
                            Boolean isOrderApproved = packageSubscription.IsOrderApproved ?? false;

                            if (!packageSubscription.ExpiryDate.IsNullOrEmpty())
                            {
                                DateTime ExpiryDate = packageSubscription.ExpiryDate.Value;
                                if (ExpiryDate.Date > DateTime.Now.Date)
                                {
                                    remaingDays = (ExpiryDate.Date - DateTime.Now.Date).Days;
                                }

                                else if (archivalGracePeriod.IsNotNull())
                                {
                                    remaingDays = (ExpiryDate.AddDays(archivalGracePeriod.Value).Date - DateTime.Now.Date).Days;
                                }
                            }

                            if (isOrderApproved == true && Presenter.IsCustomPriceSetAndSubsRenewalValid(packageSubscription.OrderID))
                            {
                                if (((remaingDays <= lstSubscriptionFrequencies.FirstOrDefault(a=>a.CompliancePackageID==packageSubscription.CompliancePackageID).DPM_SubscriptionBeforeExpFrequency && !IsExpiredSubscription)
                                    || ((archivalGracePeriod.IsNotNull() && remaingDays >= 0 && IsExpiredSubscription) || (archivalGracePeriod.IsNull() && IsExpiredSubscription)))
                                    && !IsArchivedSubscription)
                                {
                                    isValidRenewSubscription = true;
                                }
                            }
                            if (Presenter.IsPackageChangeSubscription(packageSubscription.OrderID)
                                || packageSubscription.SubscriptionMobilityStatusCode == LkpSubscriptionMobilityStatus.MobilitySwitched
                                || packageSubscription.SubscriptionMobilityStatusCode == LkpSubscriptionMobilityStatus.DataMovementDue)
                            {
                                isValidRenewSubscription = false;
                            }
                            if (!isValidRenewSubscription)
                            {
                                ListSubscription.Remove(packageSubscription);
                            }
                        }
                    }

                    grdPurchasedSubscription.DataSource = ListSubscription;
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

        protected void grdPurchasedSubscription_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                //Checks if item is GridDataItem type.
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    //Int32 compliancePackageID = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CompliancePackageID"]);

                    ////Displays the Change Institute link only for the most recent node's subscription.
                    //if (RecentPackagesIDList.IsNotNull() && !RecentPackagesIDList.Contains(compliancePackageID))
                    //{
                    //    LinkButton changeInstitutionColumn = dataItem["ChangeInstitutionColumn"].Controls[0] as LinkButton;
                    //    changeInstitutionColumn.Visible = false;
                    //}
                    Int32 pendingProgramDuration = 0;
                    Int32.TryParse(dataItem["PendingProgramDuration"].Text, out pendingProgramDuration);
                    String subscriptionStatus = Convert.ToString(dataItem["SubscriptionStatus"].Text);
                    Boolean isOrderApproved = Convert.ToBoolean(dataItem["IsOrderApproved"].Text);
                    RadButton enterDataColumn = dataItem["EnterDataColumn"].Controls[1] as RadButton;
                    enterDataColumn.ToolTip = "Enter immunization details";

                    Int32 subscriptionMonth = 0;
                    Int32.TryParse(dataItem["SubscriptionMonth"].Text, out subscriptionMonth);
                    Int32 subscriptionYear = 0;
                    Int32.TryParse(dataItem["SubscriptionYear"].Text, out subscriptionYear);
                    Int32 programDuration = 0;
                    Int32.TryParse(dataItem["ProgramDuration"].Text, out programDuration);
                    Int32 remaingDays = 0;
                    String OrderId = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrderID"].ToString();
                    vwSubscription packageSubscription = (vwSubscription)e.Item.DataItem;
                    Int32 packageSubscriptionId = packageSubscription.IsNotNull() ? packageSubscription.PackageSubscriptionID.IsNotNull() ? packageSubscription.PackageSubscriptionID.Value : 0 : 0;
                    //Boolean IsExpired = true;
                    /* Call a method that set the subscription Expired and subscription archived status
                     *UAT-977: Additional work towards archive ability */
                    Int32? archivalGracePeriod = packageSubscription.ArchivalGracePeriod;
                    SetSubscriptionExpiredAndArchiveStatus(packageSubscription.ExpiryDate.Value, packageSubscription.ArchiveStateCode);
                    if (!packageSubscription.ExpiryDate.IsNullOrEmpty())
                    {
                        DateTime ExpiryDate = Convert.ToDateTime(dataItem["ExpiryDate"].Text);
                        if (ExpiryDate.Date > DateTime.Now.Date)
                        {
                            remaingDays = (ExpiryDate.Date - DateTime.Now.Date).Days;
                            //IsExpired = false;
                        }

                        else if (archivalGracePeriod.IsNotNull())
                        {
                            //remaingDays = (ExpiryDate.AddDays(ClientSettingAfterExpiry).Date - DateTime.Now.Date).Days;
                            remaingDays = (ExpiryDate.AddDays(archivalGracePeriod.Value).Date - DateTime.Now.Date).Days;
                            //IsExpired = true;
                        }
                    }
                    //LinkButton renewSubscriptionColumn = dataItem["RenewSubscriptionColumn"].Controls[0] as LinkButton;
                    //UAT-2003: Add ability to extend/renew when clicking "place order".
                    //HtmlAnchor ancRenewSubscription = (HtmlAnchor)e.Item.FindControl("ancRenewSubscription");
                    LinkButton ancRenewSubscription = (LinkButton)e.Item.FindControl("lnkbtnRenewSubscription");
                    LinkButton ChangeSubscriptionColumn = dataItem["ChangeSubscriptionColumn"].Controls[0] as LinkButton;
                    ChangeSubscriptionColumn.ToolTip = "Edit or update subscription details";
                    ChangeSubscriptionColumn.Visible = false;
                    //[UAT-977: Additional work towards archive ability]
                    LinkButton SendUnArchiveRequest = ((LinkButton)e.Item.FindControl("lnkbtnUnArchiveRequest"));
                    Label lblUnArchiverequestSent = ((Label)e.Item.FindControl("lblUnArchiverequestSent"));
                    SendUnArchiveRequest.ToolTip = "Click here to send request for Un-Archive";
                    SendUnArchiveRequest.Visible = false;
                    lblUnArchiverequestSent.Visible = false;

                    if (isOrderApproved == true && Presenter.IsCustomPriceSetAndSubsRenewalValid(Convert.ToInt32(OrderId)))
                    {
                        /*Show Renewal Subscription link if subscription is expired and not archived 
                         * [UAT-977: Additional work towards archive ability] */
                        // if (((remaingDays <= ClientSettingBeforeExpiry && !IsExpired) || (remaingDays >= 0 && IsExpired)))

                        if (((remaingDays <= lstSubscriptionFrequencies.FirstOrDefault(a=>a.CompliancePackageID==packageSubscription.CompliancePackageID).DPM_SubscriptionBeforeExpFrequency && !IsExpiredSubscription) || ((archivalGracePeriod.IsNotNull() && remaingDays >= 0 && IsExpiredSubscription) || (archivalGracePeriod.IsNull() && IsExpiredSubscription))) && !IsArchivedSubscription)
                        {
                            //if (pendingProgramDuration <= 0 && isOrderApproved == false)
                            //{

                            //(e.Item as GridEditableItem)["RenewSubscriptionColumn"].Controls[0].Visible = false;
                            ancRenewSubscription.Visible = true;
                            //}
                        }
                    }
                    //if (isOrderApproved == true && Presenter.IsCustomPriceSet(Convert.ToInt32(OrderId)))

                    //Fixed Bugzilla#5320
                    //Added check to hide the Change subscription for Archived subscription [UAT-977:Additional work towards archive ability]
                    if (isOrderApproved == true && !IsArchivedSubscription)
                    {
                        ChangeSubscriptionColumn.Visible = true;
                    }

                    //Added check to show the send request to unArchive link for Archived subscription [UAT-977:Additional work towards archive ability]
                    if (!IsExpiredSubscription && IsArchivedSubscription)
                    {
                        if (Presenter.IsActiveUnArchiveRequestForPkgSubscriptionId(packageSubscriptionId))
                        {
                            lblUnArchiverequestSent.Visible = true;
                        }
                        else
                        {
                            SendUnArchiveRequest.Visible = true;
                        }
                    }

                    //Hide Compliance Status if subscription is expired OR Archived.
                    if (IsExpiredSubscription || IsArchivedSubscription)
                    {
                        dataItem["ComplianceStatus"].Text = String.Empty;
                    }
                    Session.Remove(ResourceConst.APPLICANT_ORDER_CART);
                    Session.Remove(ResourceConst.APPLICANT_DRUG_SCREENING);

                    //UAT-2003: Add ability to extend/renew when clicking "place order".
                    //Dictionary<String, String> queryString = new Dictionary<String, String>();

                    //if (ControlUseType == AppConsts.DASHBOARD)
                    //{
                    //    queryString = new Dictionary<String, String>
                    //                  { 
                    //                                                { "OrderId", Convert.ToString(OrderId) },
                    //                                                {"IsDashbordNavigation","true"},
                    //                                                { "Child",ChildControls.RenewalOrderOptions},//{ "Child",ChildControls.RenewalOrder}, //UAT-1188
                    //                                                {"ParentControl",AppConsts.DASHBOARD}
                    //                                             };
                    //}
                    //else
                    //{
                    //    queryString = new Dictionary<String, String>
                    //                                             { 
                    //                                                { "OrderId", Convert.ToString(OrderId) },
                    //                                                { "Child",ChildControls.RenewalOrderOptions}//{ "Child",ChildControls.RenewalOrder} //UAT-1188
                    //                                             };
                    //}
                    //ancRenewSubscription.HRef = String.Format("~/ComplianceOperations/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());

                    if (subscriptionStatus == "Expired" || isOrderApproved == false || ControlUseType == AppConsts.DASHBOARD)
                    {
                        enterDataColumn.Visible = false;
                    }

                    //UAT 265 Check for Change Subscription:
                    if (Presenter.IsPackageChangeSubscription(Convert.ToInt32(OrderId)))
                    {
                        ancRenewSubscription.Visible = false;
                        ChangeSubscriptionColumn.Visible = false;
                    }

                    #region Mobility Changes on the basis of mobility status.
                    switch (packageSubscription.SubscriptionMobilityStatusCode)
                    {
                        case LkpSubscriptionMobilityStatus.MobilitySwitched:
                            {
                                ((e.Item as GridDataItem)["EnterDataColumn"].Controls[1] as RadButton).Text = "View Data";
                                ancRenewSubscription.Visible = false;
                                ChangeSubscriptionColumn.Visible = false;
                                SendUnArchiveRequest.Visible = false;
                                break;
                            }

                        case LkpSubscriptionMobilityStatus.DataMovementDue:
                            {
                                (e.Item as GridDataItem)["EnterDataColumn"].Text = "Movement Pending";
                                ancRenewSubscription.Visible = false;
                                ChangeSubscriptionColumn.Visible = false;
                                break;
                            }
                        default:
                            {
                                break;
                            }

                    }
                    #endregion
                    //UAT-2003: Add ability to extend/renew when clicking "place order".
                    if (IsFromRenewOrderPopup)
                    {
                        enterDataColumn.Visible = false;
                        ChangeSubscriptionColumn.Visible = false;
                        SendUnArchiveRequest.Visible = false;
                        lblUnArchiverequestSent.Visible = false;
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

        protected void grdPurchasedSubscription_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("EnterData"))
                {
                    String compliancePackageID = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CompliancePackageID"].ToString();

                    Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "TenantId", Convert.ToString( CurrentUserTenantId) },
                                                                    { "PackageId", Convert.ToString( compliancePackageID) },
                                                                    { "Child", ChildControls.SubscriptionDetail}
                                                                    
                                                                 };
                    String url = String.Format("~/ComplianceOperations/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                    Response.Redirect(url, true);
                }
                else if (e.CommandName.Equals("ChangeSubscription"))
                {
                    Int32 OrderID = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrderID"].ToString());
                    if (ChangeSubscription(OrderID) == true)
                    {
                        Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "TenantId", Convert.ToString( CurrentUserTenantId) }, 
                                                                    { "Child", ChildControls.ApplicantPendingOrder},
                                                                    { AppConsts.PENDING_ORDER_NAVIGATION_FROM, PendingOrderNavigationFrom.ApplicantChangeSubscription.GetStringValue()}
                                                                 };
                        String url = String.Format("~/ComplianceOperations/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                        Response.Redirect(url, true);
                    }
                }
                else if (e.CommandName.Equals("UnArchiveRequest"))
                {
                    Int32 packageSubscriptionId = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["PackageSubscriptionID"].ToString());
                    if (packageSubscriptionId > AppConsts.NONE)
                    {
                        if (Presenter.IsActiveUnArchiveRequestForPkgSubscriptionId(packageSubscriptionId))
                            base.ShowInfoMessage("Un-archive request for this purchased subscription is already sent.");
                        else
                        {
                            if (Presenter.SaveCompSubscriptionUnArchiveRequest(packageSubscriptionId))
                            {
                                grdPurchasedSubscription.Rebind();
                                base.ShowInfoMessage("Un-archive request is successfully sent to admin.");
                            }
                            else
                                base.ShowInfoMessage("Some error has occurred. Please try again.");
                        }
                    }
                }
                //UAT-2003: Add ability to extend/renew when clicking "place order".
                else if (e.CommandName.Equals("RenewSubscription"))
                {
                    Int32 OrderId = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrderID"].ToString());
                    Dictionary<String, String> queryString = new Dictionary<String, String>();

                    if (IsFromRenewOrderPopup)
                    {
                        queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "OrderId", Convert.ToString(OrderId) },
                                                                    {"IsDashbordNavigation","true"},
                                                                    { "Child",ChildControls.RenewalOrderOptions},//{ "Child",ChildControls.RenewalOrder}, //UAT-1188
                                                                    {"ParentControl",AppConsts.DASHBOARD}
                                                                 };
                    }
                    else
                    {
                        if (ControlUseType == AppConsts.DASHBOARD)
                        {
                            queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "OrderId", Convert.ToString(OrderId) },
                                                                    {"IsDashbordNavigation","true"},
                                                                    { "Child",ChildControls.RenewalOrderOptions},//{ "Child",ChildControls.RenewalOrder}, //UAT-1188
                                                                    {"ParentControl",AppConsts.DASHBOARD}
                                                                 };
                        }
                        else
                        {
                            queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "OrderId", Convert.ToString(OrderId) },
                                                                    { "Child",ChildControls.RenewalOrderOptions}//{ "Child",ChildControls.RenewalOrder} //UAT-1188
                                                                 };
                        }
                    }

                    if (IsFromRenewOrderPopup)
                    {
                        String renewalUrl = String.Format("ComplianceOperations/Default.aspx?args={1}", _viewType, queryString.ToEncryptedQueryString());
                        //Close Popup and redirect
                        System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "CloseRenewOrderPopup('" + renewalUrl + "');", true);
                    }
                    else
                    {
                        String renewalUrl = String.Format("~/ComplianceOperations/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                        Response.Redirect(renewalUrl, true);
                    }
                }
                //else if (e.CommandName.Equals("ChangeInsitution"))
                //{
                //    Int32 OrderID = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrderID"].ToString());
                //    Dictionary<String, String> queryString = new Dictionary<String, String>
                //                                                 { 
                //                                                    { "TenantId", Convert.ToString( CurrentUserTenantId) }, 
                //                                                    { "Child", ChildControls.InstituteChangeRequestScreen}
                //                                                 };
                //    String url = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                //    Response.Redirect(url, true);
                //}
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

        #endregion

        #region Button Events



        #endregion

        #region DropDown Events



        #endregion

        #endregion

        #region Methods

        #region Public Methods
        #endregion

        #region Private Methods
        private bool ChangeSubscription(Int32 orderID)
        {
            Dictionary<String, String> objDic = Presenter.GetOrderDetails(orderID);
            if (objDic != null && objDic.Count > 0)
            {
                if (objDic.ContainsKey("ExpiryDate") && objDic.ContainsKey("PackagePrice") && objDic.ContainsKey("TotalMonthsInPackage"))
                {
                    //Code commented. Fixed UAT-339. No need of Custom monthly in case Change Subscription
                    //if (objDic.ContainsKey("MonthlyPrice"))
                    //{
                    //    Decimal MonthlyPrice = Convert.ToDecimal(objDic["MonthlyPrice"] ?? "0");
                    //}

                    //UAT-339 Change to Previous Package Settlement formula.

                    DateTime ExpiryDate = Convert.ToDateTime(objDic["ExpiryDate"]);

                    ApplicantOrderCart applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
                    if (applicantOrderCart == null)
                        applicantOrderCart = new ApplicantOrderCart();
                    applicantOrderCart.PrevOrderId = orderID;
                    applicantOrderCart.OrderRequestType = OrderRequestType.ChangeSubscription.GetStringValue();
                    applicantOrderCart.ParentControlType = ControlUseType;
                    applicantOrderCart.SettleAmount = AppConsts.NONE;

                    Decimal packagePrice = Convert.ToDecimal(objDic["PackagePrice"]);
                    Int32 totalMonthsInPackage = Convert.ToInt32(objDic["TotalMonthsInPackage"]);
                    //UAT-2859
                    DateTime DateTillPriceDontEffect = ExpiryDate.AddMonths(-(totalMonthsInPackage - 1));

                    if (ExpiryDate.Date > DateTime.Now.Date)
                    {
                        int remainingMonth = 0;

                        if (DateTillPriceDontEffect.Date >= DateTime.Now.Date)
                        {
                            remainingMonth = totalMonthsInPackage;
                        }
                        else
                        {
                            if (ExpiryDate.Day < DateTime.Now.Day)
                                remainingMonth = ((ExpiryDate.Year - DateTime.Now.Year) * 12) + ExpiryDate.Month - DateTime.Now.Month - 1;
                            else
                                remainingMonth = ((ExpiryDate.Year - DateTime.Now.Year) * 12) + ExpiryDate.Month - DateTime.Now.Month;
                        }

                        if (remainingMonth > 0)
                        {

                            //UAT-339 Change to Previous Package Settlement formula.
                            //The pricing implementation for an Applicant changing subscriptions is as follows: Net Price = (New Package Price) - (Previous Package Settlement)
                            //With the Previous Package Settlement price currently calculated as:
                            //Previous Package Settlement = (Months Remaining) * (Custom Monthly Price)
                            //This method of calculating the Previous Package Settlement needs to be changed from the above to the following:
                            //Previous Package Settlement = (Months Remaining) * ((Previous Package Price)/(Total Months in Previous Package)
                            //applicantOrderCart.SettleAmount = remainingMonth * MonthlyPrice;
                            Decimal settlementAmount = (remainingMonth * ((packagePrice) / (totalMonthsInPackage)));
                            applicantOrderCart.SettleAmount = Decimal.Round(settlementAmount, 2, MidpointRounding.AwayFromZero);

                            //Convert.ToDecimal(String.Format("${0}", settlementAmount.ToString("0.00")));
                            //change done for Applicant Dashboard Redesign. 
                        }
                        // Code commented for UAT 1188 - As an applicant, I should be able to do a subscription change as part of my package renewal 
                        //else
                        //{
                        //    //base.ShowInfoMessage("Your pending subscription period is less than one month, so it could not be adjusted.");
                        //}
                    }
                    // Code commented for UAT 1188 - As an applicant, I should be able to do a subscription change as part of my package renewal 
                    //else
                    //{
                    // 
                    //    applicantOrderCart.SettleAmount = AppConsts.NONE;
                    //}
                    SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.APPLICANT_ORDER_CART, applicantOrderCart);
                    return true;
                }
            }
            return false;
        }

        #region UAT-977: Additional work towards archive ability
        /// <summary>
        /// Method to Set the Archive and Expired subscription properties
        /// </summary>
        /// <param name="expirydate">expiry date of subscription</param>
        /// <param name="ArchiveStatusCode">Archive Code of Subscription</param>
        private void SetSubscriptionExpiredAndArchiveStatus(DateTime expirydate, String ArchiveStatusCode)
        {
            IsArchivedSubscription = ArchiveStatusCode.IsNullOrEmpty() ? false : ArchiveStatusCode.Equals(ArchiveState.Archived.GetStringValue()) ? true : false;
            IsExpiredSubscription = expirydate.IsNullOrEmpty() ? false : expirydate.Date < DateTime.Now.Date ? true : false;
        }
        #endregion
        #endregion

        #endregion

    }
}

