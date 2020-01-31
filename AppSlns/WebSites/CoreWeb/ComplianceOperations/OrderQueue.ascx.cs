using System;
using Microsoft.Practices.ObjectBuilder;
using System.Configuration;
using INTSOF.Utils;
using Telerik.Web.UI;
using System.Collections.Generic;
using INTSOF.UI.Contract.ComplianceManagement;
using CoreWeb.Shell;
using Entity.ClientEntity;
using System.Collections;
using System.Web.UI;
using System.Xml.Serialization;
using System.Text;
using System.IO;
using System.Linq;
using System.Threading;
using CoreWeb.IntsofSecurityModel;
using System.Web.UI.WebControls;
using INTSOF.UI.Contract.ComplianceOperation;
using INTERSOFT.WEB.UI.WebControls;
using System.Web;

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class OrderQueue : BaseUserControl, IOrderQueueView
    {

        #region Variables

        #region Public Variables

        public string searchbtnToolTipText = "Click to search orders per the criteria entered above";
        public string resetbtnToolTipText = "Click to remove all values entered in the search criteria above";
        public string cancelbtnToolTipText = "Click to cancel. Any data entered will not be saved";


        #endregion

        #region Private Variables

        private OrderQueuePresenter _presenter = new OrderQueuePresenter();
        private String _viewType;
        private Int32 _tenantid;
        private Boolean _showClientDropdwn;
        private CustomPagingArgsContract _gridCustomPaging = null;
        private OrderApprovalQueueContract _gridSearchContract = null;
        private OrderContract _viewContract = null;
        private DateTime _minCalenderDate = Convert.ToDateTime("01/01/1980");

        #endregion

        #endregion

        #region Properties


        public OrderQueuePresenter Presenter
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
        /// Master Order package types list with Compliance Package, Background Package and Compliance and Background Package
        /// </summary>
        List<lkpOrderPackageType> IOrderQueueView.lstOrderPackageType
        {
            set
            {
                cmbOrderType.Items.Clear();

                foreach (var opt in value)
                {
                    AddOrderPackageTypes(opt);
                }
            }
        }

        /// <summary>
        /// List of Order package type codes
        /// </summary>
        List<String> IOrderQueueView.lstSelectedOrderPkgType
        {
            get
            {
                List<String> _lstCodes = new List<String>();
                for (Int32 i = 0; i < cmbOrderType.Items.Count; i++)
                {
                    if (cmbOrderType.Items[i].Checked)
                    {
                        _lstCodes.Add(cmbOrderType.Items[i].Value);
                    }
                }
                return _lstCodes;
            }
            set
            {
                for (Int32 i = 0; i < cmbOrderType.Items.Count; i++)
                {
                    cmbOrderType.Items[i].Checked = value.Contains(cmbOrderType.Items[i].Value);
                }
            }
        }

        #region Public Properties

        /// <summary>
        /// Sets or gets the Selected Tenant Id from the select tenant dropdown.
        /// </summary>
        public Int32 SelectedTenantId
        {
            get
            {
                if (!ViewState["SelectedTenantId"].IsNull())
                {
                    return Convert.ToInt32(ViewState["SelectedTenantId"]);
                }

                return 0;
            }
            set
            {
                ViewState["SelectedTenantId"] = value;
            }
        }

        /// <summary>
        /// Sets or gets the Tenant Id for the logged-in user.
        /// </summary>
        public Int32 TenantId
        {
            get
            {
                if (_tenantid == 0)
                {
                    //_tenantid = Presenter.GetTenantId();
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    if (user.IsNotNull())
                    {
                        _tenantid = user.TenantId.HasValue ? user.TenantId.Value : 0;
                    }
                }
                return _tenantid;
            }
            set { _tenantid = value; }
        }

        /// <summary>
        /// Sets the checkbox list with order status.
        /// </summary>
        public List<lkpOrderStatu> lstOrderStatus
        {
            set
            {
                chkOrderStatus.DataSource = value.OrderBy(x => x.Name);
                chkOrderStatus.DataBind();
            }
        }

        public List<lkpPaymentOption> lstPaymentType
        {
            set
            {
                chkPaymentType.DataSource = value.OrderBy(x => x.Name);
                chkPaymentType.DataBind();
            }
        }

        /// <summary>
        /// returns the object of type IOrderQueueView.
        /// </summary>
        public IOrderQueueView CurrentViewContext
        {
            get { return this; }
        }


        /// <summary>
        /// returns the Current Logged In User Id.
        /// </summary>
        public Int32 CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        /// <summary>
        /// Gets or sets the list of active tenants.
        /// </summary>
        public List<Tenant> lstTenant
        {
            set
            {
                ddlTenantName.DataSource = value;
                ddlTenantName.DataBind();
            }
        }

        /// <summary>
        /// Gets or sets the list of rows from ApplicantComplianceItemData table.
        /// </summary>
        public List<OrderContract> lstOrderQueue
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Error Message
        /// </summary>
        public String ErrorMessage
        {
            get;
            set;
        }

        public String InfoMessage
        {
            get;
            set;
        }

        public String SuccessMessage
        {
            get;
            set;
        }

        /// <summary>
        /// Indicates wheather Select Client dropdown will be visible or not.
        /// </summary>
        public Boolean ShowClientDropDown
        {
            get
            {
                return _showClientDropdwn;
            }
            set
            {
                _showClientDropdwn = value;
            }
        }

        public List<String> SelectedOrderStatusCode
        {
            get
            {
                List<String> selectedCodes = new List<String>();
                for (Int32 i = 0; i < chkOrderStatus.Items.Count; i++)
                {
                    if (chkOrderStatus.Items[i].Checked)
                    {
                        selectedCodes.Add(chkOrderStatus.Items[i].Value);
                    }
                }
                return selectedCodes;
            }
            set
            {
                for (Int32 i = 0; i < chkOrderStatus.Items.Count; i++)
                {
                    chkOrderStatus.Items[i].Checked = value.Contains(chkOrderStatus.Items[i].Value);
                }

            }

        }

        public List<String> SelectedPaymentTypeCode
        {
            get
            {
                List<String> selectedCodes = new List<String>();
                for (Int32 i = 0; i < chkPaymentType.Items.Count; i++)
                {
                    if (chkPaymentType.Items[i].Checked)
                    {
                        selectedCodes.Add(chkPaymentType.Items[i].Value);
                    }
                }
                return selectedCodes;
            }
            set
            {
                for (Int32 i = 0; i < chkPaymentType.Items.Count; i++)
                {
                    chkPaymentType.Items[i].Checked = value.Contains(chkPaymentType.Items[i].Value);
                }
            }
        }

        public String FirstNameSearch
        {
            get
            {
                return txtFirstName.Text.Trim();
            }
        }

        public String LastNameSearch
        {
            get
            {
                return txtLastName.Text.Trim();
            }
        }

        public String OrderNumberSearch
        {
            get
            {
                if (!txtOrderNumber.Text.IsNullOrEmpty())
                {
                    return Convert.ToString(txtOrderNumber.Text);
                }
                return String.Empty;
            }
        }

        /// <summary>
        /// Sets and gets option for showing Rush Orders in the Queue. 
        /// </summary>
        public Boolean ShowOnlyRushOrders
        {
            get
            {
                // return chkShowRushOrders.Checked;
                //UAT-2193: Remove Rush Orders check box
                return false;
            }
        }

        public List<DateTime> LstOrderCrtdDate
        {
            get
            {
                if (dpOdrCrtFrm.SelectedDate.IsNotNull() || dpOdrCrtTo.SelectedDate.IsNotNull())
                {
                    List<DateTime> tempList = new List<DateTime>();
                    tempList.Add(dpOdrCrtFrm.SelectedDate.IsNotNull() ? Convert.ToDateTime(dpOdrCrtFrm.SelectedDate) : _minCalenderDate);
                    tempList.Add(dpOdrCrtTo.SelectedDate.IsNotNull() ? Convert.ToDateTime(dpOdrCrtTo.SelectedDate).AddDays(1) : DateTime.Now.Date.AddDays(1));
                    return tempList;
                }
                return null;
            }
        }

        public List<DateTime> LstOrderPaidDate
        {
            get
            {
                if (dpOdrPaidFrm.SelectedDate.IsNotNull() || dpOdrPaidTo.SelectedDate.IsNotNull())
                {
                    List<DateTime> tempList = new List<DateTime>();
                    tempList.Add(dpOdrPaidFrm.SelectedDate.IsNotNull() ? Convert.ToDateTime(dpOdrPaidFrm.SelectedDate) : _minCalenderDate);
                    tempList.Add(dpOdrPaidTo.SelectedDate.IsNotNull() ? Convert.ToDateTime(dpOdrPaidTo.SelectedDate).AddDays(1) : DateTime.Now.Date.AddDays(1));
                    return tempList;
                }
                return null;
            }
        }

        public String DeptProgramMappingID
        {
            get
            {
                if (!hdnDepartmntPrgrmMppng.Value.IsNullOrEmpty())
                {
                    return hdnDepartmntPrgrmMppng.Value;
                }
                return String.Empty;
            }
        }

        public List<lkpArchiveState> lstArchiveState
        {
            set
            {
                //dvRecords.Visible = true;    //No more need of archival filters in Order Queue.
                rblOrderState.DataSource = value.OrderBy(x => x.AS_Code);
                rblOrderState.DataBind();
                rblOrderState.SelectedValue = ArchiveState.All.GetStringValue();
            }
        }

        public List<String> SelectedArchiveStateCode
        {
            get
            {
                if (!rblOrderState.SelectedValue.IsNullOrEmpty())
                {
                    List<String> selectedCodes = new List<String>();
                    if (rblOrderState.SelectedValue.Equals(ArchiveState.All.GetStringValue()))
                    {
                        return null;
                    }
                    else
                    {
                        selectedCodes.Add(rblOrderState.SelectedValue);
                    }
                    return selectedCodes;
                }
                else
                    return null;
            }
            set
            {
                rblOrderState.SelectedValue = value.FirstOrDefault();
            }
        }

        Boolean IOrderQueueView.isBkgScreen
        {
            get
            {
                return hdnScreenName.Value == "BackgroundScreen" ? true : false;
            }

        }
        #region Custom Paging


        /// <summary>
        /// CurrentPageIndex</summary>
        /// <value>
        /// Gets or sets the value for CurrentPageIndex.</value>
        public Int32 CurrentPageIndex
        {
            get
            {
                return grdOrderDetails.MasterTableView.CurrentPageIndex + 1;
            }
            set
            {
                //if (grdOrderDetails.MasterTableView.CurrentPageIndex > 0)
                //{
                //    grdOrderDetails.MasterTableView.CurrentPageIndex = value - 1;
                //}
                grdOrderDetails.MasterTableView.CurrentPageIndex = value == 0 ? 0 : value - 1;
            }
        }

        /// <summary>
        /// PageSize</summary>
        /// <value>
        /// Gets the value for PageSize.</value>
        //public Int32 PageSize
        //{
        //    get
        //    {
        //        // Maximum 100 record allowed from DB. 
        //        //return grdOrderDetails.PageSize > 100 ? 100 : grdOrderDetails.PageSize;
        //        return grdOrderDetails.PageSize;
        //    }
        //}

        public Int32 PageSize
        {
            get
            {
                // Maximum 100 record allowed from DB. 
                //return grdApplicantSearchData.PageSize > 100 ? 100 : grdApplicantSearchData.PageSize;
                return grdOrderDetails.PageSize;
            }
            set
            {
                grdOrderDetails.PageSize = value;
            }
        }

        /// <summary>
        /// VirtualPageCount</summary>
        /// <value>
        /// Sets the value for VirtualPageCount.</value>
        public Int32 VirtualPageCount
        {
            get
            {
                return grdOrderDetails.MasterTableView.VirtualItemCount;
            }
            set
            {
                grdOrderDetails.VirtualItemCount = value;
                grdOrderDetails.MasterTableView.VirtualItemCount = value;
            }
        }

        /// <summary>
        /// get object of shared class of custom paging
        /// </summary>
        //public CustomPagingArgsContract GridCustomPaging
        //{
        //    get
        //    {
        //        if (_gridCustomPaging.IsNull())
        //        {
        //            _gridCustomPaging = new CustomPagingArgsContract();
        //        }
        //        return _gridCustomPaging;
        //    }
        //}

        public CustomPagingArgsContract GridCustomPaging
        {
            get
            {
                if (ViewState["_gridCustomPaging"] == null)
                {
                    ViewState["_gridCustomPaging"] = new CustomPagingArgsContract();
                }
                return (CustomPagingArgsContract)ViewState["_gridCustomPaging"];
            }
            set
            {
                ViewState["_gridCustomPaging"] = value;
                CurrentViewContext.VirtualRecordCount = value.VirtualPageCount;
                CurrentViewContext.PageSize = value.PageSize;
                CurrentViewContext.CurrentPageIndex = value.CurrentPageIndex;
            }
        }

        /// <summary>
        /// Virtual Page Count</summary>
        /// <value>
        /// Sets the value for VirtualPageCount.</value>
        public Int32 VirtualRecordCount
        {
            get
            {
                if (ViewState["ItemCount"] != null)
                    return Convert.ToInt32(ViewState["ItemCount"]);
                else
                    return 0;
            }
            set
            {
                ViewState["ItemCount"] = value;
                grdOrderDetails.VirtualItemCount = value;
                grdOrderDetails.MasterTableView.VirtualItemCount = value;
            }
        }

        /// <summary>
        /// View Contract
        /// </summary>
        public OrderContract ViewContract
        {
            get
            {
                if (_viewContract.IsNull())
                {
                    _viewContract = new OrderContract();
                }
                return _viewContract;
            }
        }


        #endregion

        //public SearchItemDataContract SetSearchItemDataContract
        //{
        //    set
        //    {
        //        if (value.IsNotNull())
        //            value.NodeLabel = lblinstituteHierarchy.Text;
        //        var serializer = new XmlSerializer(typeof(SearchItemDataContract));
        //        var sb = new StringBuilder();
        //        using (TextWriter writer = new StringWriter(sb))
        //        {
        //            serializer.Serialize(writer, value);
        //        }
        //        Session[AppConsts.SEARCH_OBJECT_SESSION_KEY] = sb.ToString();
        //    }
        //}

        public OrderApprovalQueueContract SetOrderApprovalQueueContract
        {
            set
            {
                if (value.IsNotNull())
                    value.NodeLabel = lblinstituteHierarchy.Text;
                var serializer = new XmlSerializer(typeof(OrderApprovalQueueContract));
                var sb = new StringBuilder();
                using (TextWriter writer = new StringWriter(sb))
                {
                    serializer.Serialize(writer, value);
                }
                Session[AppConsts.SEARCH_OBJECT_SESSION_KEY] = sb.ToString();
            }
        }

        /// <summary>
        /// get object of shared class of search contract
        /// </summary>
        public OrderApprovalQueueContract GetGridSearchContract
        {
            get
            {
                if (_gridSearchContract.IsNull())
                {
                    var serializer = new XmlSerializer(typeof(OrderApprovalQueueContract));
                    TextReader reader = new StringReader(Convert.ToString(Session[AppConsts.SEARCH_OBJECT_SESSION_KEY]));
                    _gridSearchContract = (OrderApprovalQueueContract)serializer.Deserialize(reader);
                }
                return _gridSearchContract;
            }
        }

        /// <summary>
        /// Gets or Sets the value for selected Items.
        /// </summary>
        public Dictionary<Int32, Boolean> SelectedOrderIds
        {
            get
            {
                if (!ViewState["SelectedOrders"].IsNull())
                {
                    return ViewState["SelectedOrders"] as Dictionary<Int32, Boolean>;
                }

                return new Dictionary<Int32, Boolean>();
            }
            set
            {
                ViewState["SelectedOrders"] = value;
            }
        }


        public String ReferenceNumber
        {
            get
            {
                return txtReferenceText.Text.Trim();
            }
            set
            {
                txtReferenceText.Text = value;
            }
        }


        #region UAT-806 Creation of granular permissions for Client Admin users

        public String SSNPermissionCode
        {
            get
            {
                return Convert.ToString(ViewState["SSNPermissionCode"]);
            }
            set
            {
                ViewState["SSNPermissionCode"] = value;
            }
        }
        public Boolean IsDOBDisable
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsDOBDisabled"] ?? false);
            }
            set
            {
                ViewState["IsDOBDisabled"] = value;
            }
        }

        public String ApplicantSSN
        {
            get;
            set;
        }

        public Boolean IsSSNDisabled
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsSSNDisabled"] ?? false);
            }
            set
            {
                ViewState["IsSSNDisabled"] = value;
            }
        }

        #endregion

        #region UAT-796
        public String ParentPageName { get; set; }

        #endregion

        #endregion

        #region Private Properties

        #endregion

        #endregion

        #region Events

        #region Page Events

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"></see> event.
        /// Calls WebClientApplication.BuildItemWithCurrentContext after the events are handled
        /// to fire the DI engine.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
        /// <remarks></remarks>
        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.IsPolicyEnable = !ConfigurationManager.AppSettings[AppConsts.IS_POLICY_ENABLE].IsNull() && Convert.ToBoolean(ConfigurationManager.AppSettings[AppConsts.IS_POLICY_ENABLE]);
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                base.OnInit(e);
                base.Title = "Order Queue";
                lblOrderQueue.Text = base.Title;
                fsucOrderCmdBar.SubmitButton.CausesValidation = false;
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
        /// Page load event for initialized event in presenter.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
        /// <remarks></remarks>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ApplicantSSN = txtSSN.TextWithPrompt;
                if (!this.IsPostBack)
                {
                    Presenter.OnViewInitialized();

                    if (!ParentPageName.IsNullOrEmpty() && ParentPageName == AppConsts.QUEUE_TYPE_BKGORDER_QUEUE)
                    {
                        hdnScreenName.Value = "BackgroundScreen";
                        grdOrderDetails.MasterTableView.GetColumn("AutoRenewal").Display = false;
                    }
                    else
                    {
                        hdnScreenName.Value = "OrderQueue";
                    }

                    if (ShowClientDropDown)
                    {
                        divTenant.Visible = true;
                        SelectedTenantId = 0;
                    }
                    else
                    {
                        divTenant.Visible = false;
                        SelectedTenantId = TenantId;
                        BindControlsForSelectedTenant();
                    }
                    ApplySSNMask();
                    Dictionary<String, String> args = new Dictionary<String, String>();
                    if (!Request.QueryString["args"].IsNull())
                    {
                        args.ToDecryptedQueryString(Request.QueryString["args"]);
                        SetPageControls(args);
                    }
                    else
                        ShowHideControls(false);
                    List<Entity.ClientEntity.ClsFeatureAction> actionCollection = new List<ClsFeatureAction>();
                    ApplyActionLevelPermission(actionCollection, "Order Approval Queue");
                    Presenter.GetSSNSetting();
                    GetSessionValues();
                }
                if (divTenant.Visible && !ddlTenantName.SelectedValue.IsNullOrEmpty())
                {
                    SelectedTenantId = Convert.ToInt32(ddlTenantName.SelectedValue);
                }
                else if (divTenant.Visible && ddlTenantName.SelectedValue.IsNullOrEmpty())
                {
                    SelectedTenantId = 0;
                }
                hdnTenantId.Value = SelectedTenantId.ToString();
                hfCurrentUserID.Value = CurrentUserId.ToString();
                lblinstituteHierarchy.Text = hdnHierarchyLabel.Value;
                Presenter.OnViewLoaded();
                base.Title = "Order Queue";
                base.SetPageTitle("Order Queue");
                (fsucOrderCmdBar as CoreWeb.Shell.Views.CommandBar).SaveButton.ToolTip = searchbtnToolTipText;
                (fsucOrderCmdBar as CoreWeb.Shell.Views.CommandBar).SubmitButton.ToolTip = resetbtnToolTipText;
                (fsucOrderCmdBar as CoreWeb.Shell.Views.CommandBar).CancelButton.ToolTip = cancelbtnToolTipText;
                if (CurrentViewContext.IsSSNDisabled)
                {
                    txtSSN.Text = String.Empty;
                    divSSN.Visible = false;
                    grdOrderDetails.MasterTableView.GetColumn("ApplicantSSN").Visible = false;
                    grdOrderDetails.MasterTableView.GetColumn("_ApplicantSSN").Visible = false;
                }
                else
                {
                    divSSN.Visible = true;
                    grdOrderDetails.MasterTableView.GetColumn("ApplicantSSN").Visible = true;
                }
                HideShowControlsForGranularPermission();//UAT-806
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

        /// <summary>
        /// Sets the list of filters to be displayed in Order Queue. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdOrderDetails_Init(object sender, System.EventArgs e)
        {
            GridFilterMenu menu = grdOrderDetails.FilterMenu;
            int i = 0;
            while (i < menu.Items.Count)
            {
                if (menu.Items[i].Text == GridKnownFunction.Between.ToString() || menu.Items[i].Text == GridKnownFunction.NotBetween.ToString() ||
                    menu.Items[i].Text == GridKnownFunction.NotIsEmpty.ToString() || menu.Items[i].Text == GridKnownFunction.NotIsNull.ToString())
                {
                    menu.Items.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
        }

        /// <summary>
        /// Retrieves a list of Applicant Compliance Item Data.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        /// <remarks></remarks>
        protected void grdOrderDetails_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                lstOrderQueue = new List<OrderContract>();
                GridCustomPaging.CurrentPageIndex = CurrentPageIndex;
                GridCustomPaging.PageSize = PageSize;
                CurrentViewContext.GridCustomPaging = CurrentViewContext.GridCustomPaging;

                var serializer = new XmlSerializer(typeof(CustomPagingArgsContract));
                var sb = new StringBuilder();
                using (TextWriter writer = new StringWriter(sb))
                {
                    serializer.Serialize(writer, GridCustomPaging);
                }
                Session[AppConsts.ORDER_QUEUE_SESSION_KEY] = sb.ToString();
                Presenter.GetOrderQueueData();

                //if (lstOrderQueue.Any(x => x.IsInvoiceApproval))
                //    divInvoiceApproval.Attributes.Add("style", "text-align:center; vertical-align:central; display:block;");
                //else
                //    divInvoiceApproval.Attributes.Add("style", "display:none;");

                grdOrderDetails.DataSource = lstOrderQueue;
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
        /// Redirect the user to the detail page.
        /// Sets the filetrs whn filtering is applied.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdOrderDetails_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                #region DetailScreenNavigation

                if (e.CommandName.Equals("ViewDetail"))
                {
                    SetSessionValues();
                    Int32 selectedTenantId = 0;
                    if (!ddlTenantName.SelectedValue.IsNullOrEmpty())
                    {
                        selectedTenantId = SelectedTenantId;
                    }
                    else
                    {
                        selectedTenantId = TenantId;
                    }
                    Dictionary<String, String> queryString = new Dictionary<String, String>();
                    String orderId = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrderId"].ToString();
                    String orderNumber = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrderNumber"].ToString();
                    queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "SelectedTenantId", Convert.ToString( selectedTenantId) },
                                                                    { "Child", ChildControls.OrderPaymentDetails},
                                                                    { "OrderId", orderId},
                                                                    {"ShowApproveRejectButtons",true.ToString()},
                                                                    {AppConsts.PARENT_QUEUE_QUERYSTRING, !this.ParentPageName.IsNullOrEmpty()
                                                                                        ? AppConsts.QUEUE_TYPE_BKGORDER_QUEUE
                                                                                        : AppConsts.QUEUE_TYPE_CMPORDER_QUEUE },
                                                                     {AppConsts.ORDER_NUMBER,orderNumber}
                                                                 };
                    string url = String.Format("~/ComplianceOperations/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                    Response.Redirect(url, true);

                }
                #endregion

                #region For Filter command

                //if (!ViewState["SortExpression"].IsNull())
                //{
                //    CurrentViewContext.GridCustomPaging.SortExpression = Convert.ToString(ViewState["SortExpression"]);
                //    CurrentViewContext.GridCustomPaging.SortDirectionDescending = Convert.ToBoolean(ViewState["SortDirection"]);
                //}

                CurrentViewContext.GridCustomPaging.FilterColumns = ViewState["FilterColumns"] == null ? new List<String>() : (List<String>)(ViewState["FilterColumns"]);
                CurrentViewContext.GridCustomPaging.FilterOperators = ViewState["FilterOperators"] == null ? new List<String>() : (List<String>)(ViewState["FilterOperators"]);
                CurrentViewContext.GridCustomPaging.FilterValues = ViewState["FilterValues"] == null ? new ArrayList() : (ArrayList)(ViewState["FilterValues"]);

                if (e.CommandName == RadGrid.FilterCommandName)
                {
                    Pair filter = (Pair)e.CommandArgument;

                    Int32 filterIndex = CurrentViewContext.GridCustomPaging.FilterColumns.IndexOf(filter.Second.ToString());
                    if (filter.First.ToString() != GridKnownFunction.NoFilter.ToString())
                    {
                        String filteringType = (e.Item as GridFilteringItem)[filter.Second.ToString()].Controls[0].GetType().Name;
                        String filterValue = grdOrderDetails.MasterTableView.Columns.FindByUniqueName(filter.Second.ToString()).CurrentFilterValue;

                        if (filterIndex != -1)
                        {
                            CurrentViewContext.GridCustomPaging.FilterOperators[filterIndex] = filter.First.ToString();
                            if (grdOrderDetails.MasterTableView.Columns.FindByUniqueName(filter.Second.ToString()).DataTypeName == "System.Decimal")
                            {
                                if (!filterValue.IsNullOrEmpty())
                                {
                                    CurrentViewContext.GridCustomPaging.FilterValues[filterIndex] = Convert.ToDecimal(filterValue);
                                }
                                //If filter Value Is Null Or Empty then set to default value and set IsNull filter to other filter
                                if (CurrentViewContext.GridCustomPaging.FilterOperators.Contains("IsNull"))
                                {
                                    CurrentViewContext.GridCustomPaging.FilterOperators[filterIndex] = "NullOtherThanString";
                                    CurrentViewContext.GridCustomPaging.FilterValues[filterIndex] = AppConsts.NONE;
                                }
                            }
                            else if (grdOrderDetails.MasterTableView.Columns.FindByUniqueName(filter.Second.ToString()).DataTypeName == "System.Int32")
                            {
                                if (!filterValue.IsNullOrEmpty())
                                {
                                    CurrentViewContext.GridCustomPaging.FilterValues[filterIndex] = Convert.ToInt32(filterValue);
                                }

                                //If filter Value Is Null Or Empty then set to default value and set IsNull filter to other filter
                                if (CurrentViewContext.GridCustomPaging.FilterOperators.Contains("IsNull"))
                                {
                                    CurrentViewContext.GridCustomPaging.FilterOperators[filterIndex] = "NullOtherThanString";
                                    CurrentViewContext.GridCustomPaging.FilterValues[filterIndex] = AppConsts.NONE;
                                }
                            }
                            else if (grdOrderDetails.MasterTableView.Columns.FindByUniqueName(filter.Second.ToString()).DataTypeName == "System.DateTime")
                            {
                                if (!filterValue.IsNullOrEmpty())
                                {
                                    try
                                    {
                                        //try to convert any value to date
                                        CurrentViewContext.GridCustomPaging.FilterValues[filterIndex] = Convert.ToDateTime(filterValue);
                                    }
                                    catch
                                    {
                                        //date filter value could not be converted, set filter value to any default date
                                        CurrentViewContext.GridCustomPaging.FilterValues[filterIndex] = Convert.ToDateTime("01/01/1901");
                                        //return;
                                    }
                                }

                                //To set IsNull filter to other Date format filter and set to any default date in case of Null date
                                if (CurrentViewContext.GridCustomPaging.FilterOperators.Contains("IsNull"))
                                {
                                    CurrentViewContext.GridCustomPaging.FilterOperators[filterIndex] = "NullOtherThanString";
                                    CurrentViewContext.GridCustomPaging.FilterValues[filterIndex] = Convert.ToDateTime("01/01/1901");
                                }
                            }
                            else
                            {
                                CurrentViewContext.GridCustomPaging.FilterValues[filterIndex] = filterValue;
                            }
                        }
                        else
                        {
                            CurrentViewContext.GridCustomPaging.FilterColumns.Add(filter.Second.ToString());
                            CurrentViewContext.GridCustomPaging.FilterOperators.Add(filter.First.ToString());
                            if (grdOrderDetails.MasterTableView.Columns.FindByUniqueName(filter.Second.ToString()).DataTypeName == "System.Decimal")
                            {
                                if (!filterValue.IsNullOrEmpty())
                                {
                                    CurrentViewContext.GridCustomPaging.FilterValues.Add(Convert.ToDecimal(filterValue));
                                }

                                //If filter Value Is Null Or Empty then set to default value and set IsNull filter to other filter
                                if (CurrentViewContext.GridCustomPaging.FilterOperators.Contains("IsNull"))
                                {
                                    Int32 index = CurrentViewContext.GridCustomPaging.FilterOperators.IndexOf("IsNull");
                                    CurrentViewContext.GridCustomPaging.FilterOperators[index] = "NullOtherThanString";
                                    CurrentViewContext.GridCustomPaging.FilterValues.Add(AppConsts.NONE);
                                }
                            }
                            else if (grdOrderDetails.MasterTableView.Columns.FindByUniqueName(filter.Second.ToString()).DataTypeName == "System.Int32")
                            {

                                if (!filterValue.IsNullOrEmpty())
                                {
                                    CurrentViewContext.GridCustomPaging.FilterValues.Add(Convert.ToInt32(filterValue));
                                }

                                //If filter Value Is Null Or Empty then set to default value and set IsNull filter to other filter
                                if (CurrentViewContext.GridCustomPaging.FilterOperators.Contains("IsNull"))
                                {
                                    Int32 index = CurrentViewContext.GridCustomPaging.FilterOperators.IndexOf("IsNull");
                                    CurrentViewContext.GridCustomPaging.FilterOperators[index] = "NullOtherThanString";
                                    CurrentViewContext.GridCustomPaging.FilterValues.Add(AppConsts.NONE);
                                }
                            }
                            else if (grdOrderDetails.MasterTableView.Columns.FindByUniqueName(filter.Second.ToString()).DataTypeName == "System.DateTime")
                            {
                                if (!filterValue.IsNullOrEmpty())
                                {
                                    try
                                    {
                                        //try to convert any value to date
                                        CurrentViewContext.GridCustomPaging.FilterValues.Add(Convert.ToDateTime(filterValue));
                                    }
                                    catch
                                    {
                                        //date filter value could not be converted, set filter value to any default date
                                        CurrentViewContext.GridCustomPaging.FilterValues.Add(Convert.ToDateTime("01/01/1901"));
                                        //return;
                                    }
                                }

                                //To set IsNull filter to other Date format filter and set to any default date in case of Null date
                                if (CurrentViewContext.GridCustomPaging.FilterOperators.Contains("IsNull"))
                                {
                                    Int32 index = CurrentViewContext.GridCustomPaging.FilterOperators.IndexOf("IsNull");
                                    CurrentViewContext.GridCustomPaging.FilterOperators[index] = "NullOtherThanString";
                                    CurrentViewContext.GridCustomPaging.FilterValues.Add(Convert.ToDateTime("01/01/1901"));
                                }
                            }
                            else
                            {
                                CurrentViewContext.GridCustomPaging.FilterValues.Add(filterValue);
                            }

                        }
                    }
                    else if (filterIndex != -1)
                    {
                        CurrentViewContext.GridCustomPaging.FilterOperators.RemoveAt(filterIndex);
                        CurrentViewContext.GridCustomPaging.FilterValues.RemoveAt(filterIndex);
                        CurrentViewContext.GridCustomPaging.FilterColumns.RemoveAt(filterIndex);
                    }

                    ViewState["FilterColumns"] = CurrentViewContext.GridCustomPaging.FilterColumns;
                    ViewState["FilterOperators"] = CurrentViewContext.GridCustomPaging.FilterOperators;
                    ViewState["FilterValues"] = CurrentViewContext.GridCustomPaging.FilterValues;
                }

                #endregion
                // Hide filter when exportig to pdf or word
                if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToPdfCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToWordCommandName
                    || e.CommandName == Telerik.Web.UI.RadGrid.ExportToExcelCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToCsvCommandName)
                {
                    base.ConfigureExport(grdOrderDetails);

                }

                #region EXPORT FUNCTIONALITY
                //UAT-1051 Complio: Full social security number should not be displayed on grid exports. Added Duplicate Column with masked SSN
                // and displayed the masked column on Export instead of actual column.
                if (e.CommandName.IsNullOrEmpty())
                {
                    WclComboBox cmbExportFormat = e.Item.FindControl("cmbExportFormat") as WclComboBox;
                    if (cmbExportFormat.IsNotNull() && (cmbExportFormat.SelectedValue == "Csv" || cmbExportFormat.SelectedValue == "Pdf" || cmbExportFormat.SelectedValue == "Excel"))
                    {
                        grdOrderDetails.MasterTableView.GetColumn("_ApplicantSSN").Display = true;
                    }
                }
                else if (e.CommandName == "Cancel")
                {
                    grdOrderDetails.MasterTableView.GetColumn("_ApplicantSSN").Display = false;
                }
                #endregion

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

        /// <summary>
        /// Grid sort expression
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void grdOrderDetails_SortCommand(object sender, GridSortCommandEventArgs e)
        //{
        //    try
        //    {
        //        if (e.NewSortOrder != GridSortOrder.None)
        //        {
        //            ViewState["SortExpression"] = e.SortExpression;
        //            ViewState["SortDirection"] = e.NewSortOrder.Equals(GridSortOrder.Descending);
        //            CurrentViewContext.GridCustomPaging.SortExpression = e.SortExpression;
        //            CurrentViewContext.GridCustomPaging.SortDirectionDescending = e.NewSortOrder.Equals(GridSortOrder.Descending);

        //        }
        //        else
        //        {
        //            ViewState["SortExpression"] = String.Empty;
        //            ViewState["SortDirection"] = false;
        //            CurrentViewContext.GridCustomPaging.SortExpression = String.Empty;
        //            CurrentViewContext.GridCustomPaging.SortDirectionDescending = false;
        //        }
        //    }
        //    catch (SysXException ex)
        //    {
        //        base.LogError(ex);
        //        base.ShowErrorMessage(ex.Message);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        base.LogError(ex);
        //        base.ShowErrorMessage(ex.Message);
        //    }
        //}

        protected void grdOrderDetails_SortCommand(object sender, GridSortCommandEventArgs e)
        {
            try
            {
                if (e.NewSortOrder != GridSortOrder.None)
                {
                    CurrentViewContext.GridCustomPaging.SortExpression = e.SortExpression;
                    CurrentViewContext.GridCustomPaging.SortDirectionDescending = e.NewSortOrder.Equals(GridSortOrder.Descending);
                    CurrentViewContext.GridCustomPaging.SortExpression = e.SortExpression;
                    CurrentViewContext.GridCustomPaging.SortDirectionDescending = e.NewSortOrder.Equals(GridSortOrder.Descending);
                }
                else
                {
                    CurrentViewContext.GridCustomPaging.SortExpression = String.Empty;
                    CurrentViewContext.GridCustomPaging.SortDirectionDescending = false;
                    CurrentViewContext.GridCustomPaging.SortExpression = String.Empty;
                    CurrentViewContext.GridCustomPaging.SortDirectionDescending = false;
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

        protected void grdOrderDetails_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
                {
                    if (e.Item.DataItem is OrderContract)
                    {
                        OrderContract orderDetail = (OrderContract)(e.Item.DataItem);

                        if ((orderDetail.Amount) == (Decimal)AppConsts.NONE || orderDetail.Amount == null)
                        {
                            ((GridDataItem)e.Item)["PaymentType"].Text = String.Empty;
                        }
                        CheckBox checkBox = ((CheckBox)e.Item.FindControl("chkSelectItem"));
                        //For UAT- 2379, check of admin to see checkbox against "payment due" status records
                        if ((orderDetail.IsInvoiceApproval && orderDetail.OrderStatusCode == ApplicantOrderStatus.Payment_Due.GetStringValue()) || orderDetail.OrderStatusCode.Contains(ApplicantOrderStatus.Send_For_Online_Payment.GetStringValue()))
                        {
                            if (Presenter.IsDefaultTenant)
                            {
                                checkBox.Visible = true;
                            }
                            else
                            {
                                checkBox.Visible = false;
                            }
                        }

                        String itemDataId = orderDetail.OrderId.ToString();
                        Boolean isMapped = orderDetail.IsInvoiceApproval;

                        checkBox.Enabled = !orderDetail.IsInvoiceApprovalInitiated; //UAT-685  Only Enable if already Approval Process is not Initiated.
                        if (checkBox.Enabled)
                        {
                            checkBox.ToolTip = "Select order to approve.";
                        }
                        else
                        {
                            checkBox.ToolTip = "Order already queued for payment approval.";
                        }

                        //UAT-3216
                        Boolean IsCardApprovalMapped = orderDetail.IsCardWithApproval;
                        if (orderDetail.IsCardWithApproval)
                        {
                            checkBox.Visible = true;
                        }

                        if (Convert.ToInt32(itemDataId) != 0)
                        {
                            Dictionary<Int32, Boolean> selectedItems = CurrentViewContext.SelectedOrderIds;
                            if (selectedItems.IsNotNull() && selectedItems.Count > 0)
                            {
                                if ((isMapped || IsCardApprovalMapped) && selectedItems.ContainsKey(Convert.ToInt32(itemDataId)))
                                {
                                    checkBox.Checked = Convert.ToBoolean(selectedItems[Convert.ToInt32(itemDataId)].ToString());
                                }
                            }
                        }

                    }

                    //


                    #region UAT-796

                    Int32 currentOrderId = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrderId"].ToString());//.ToString();
                    OrderContract OrderQueueObj = CurrentViewContext.lstOrderQueue.Where(x => x.OrderId == currentOrderId).FirstOrDefault();
                    LinkButton lbtnAutoRenewal = ((LinkButton)e.Item.FindControl("btnAutoRenewal"));

                    //UAT-916 :WB: As an application admin, I should be able to define payment options at the package level in addition to the node level
                    OrderPaymentDetail tempOrderPaymentDetail = Presenter.GetOrderPaymentDetailForOrder(currentOrderId);
                    if ((tempOrderPaymentDetail.lkpPaymentOption.IsNotNull() && (tempOrderPaymentDetail.lkpPaymentOption.Code.Equals(PaymentOptions.InvoiceWithOutApproval.GetStringValue())
                        || tempOrderPaymentDetail.lkpPaymentOption.Code.Equals(PaymentOptions.InvoiceWithApproval.GetStringValue()))) &&
                        ((tempOrderPaymentDetail.lkpOrderStatu.IsNotNull() && tempOrderPaymentDetail.lkpOrderStatu.Code.Equals(ApplicantOrderStatus.Pending_Payment_Approval.GetStringValue()))
                        || OrderQueueObj.HasActiveComplianceSubscription
                       ) && OrderQueueObj.PackageID > 0
                        )

                    //if ((OrderQueueObj.PaymentType.Equals(OrderPaymentType.InvoicetoInstitution.GetStringValue())
                    //    || OrderQueueObj.PaymentType.Equals(OrderPaymentType.InvoiceWithApproval.GetStringValue())) &&
                    //    ((OrderQueueObj.OrderStatusCode.Equals(ApplicantOrderStatus.Pending_Payment_Approval.GetStringValue()))
                    //    || OrderQueueObj.HasActiveComplianceSubscription
                    //   ) && OrderQueueObj.PackageID > 0
                    //    )
                    {
                        lbtnAutoRenewal.Visible = true;
                        (e.Item as GridDataItem).OwnerTableView.Columns.FindByUniqueName("AutoRenewal").Visible = true;
                        if (!OrderQueueObj.IsAutomaticRenewalForPackage)
                        {
                            lbtnAutoRenewal.Text = "OFF";
                            lbtnAutoRenewal.ToolTip = String.Empty;
                            lbtnAutoRenewal.Attributes.Add("Enabled", "false");
                            lbtnAutoRenewal.CssClass = "autoRenewalLinkOffButton";
                        }
                        else
                        {
                            lbtnAutoRenewal.CssClass = "autoRenewalLink";
                            lbtnAutoRenewal.Text = Convert.ToBoolean(OrderQueueObj.AutomaticRenewalTurnedOff) ? "OFF " : "ON ";
                            lbtnAutoRenewal.ToolTip = Convert.ToBoolean(OrderQueueObj.AutomaticRenewalTurnedOff) ? "Click to Turn On Auto Renewal" : "Click to Turn Off Auto Renewal";
                        }
                    }


                    #endregion
                }

                if (e.Item.ItemType.Equals(GridItemType.Footer))
                {
                    Int32 checkBoxCount = 0;
                    Int32 rowCount = grdOrderDetails.Items.Count;
                    if (rowCount > 0)
                    {
                        Int32 checkCount = 0;
                        foreach (GridDataItem item in grdOrderDetails.Items)
                        {
                            CheckBox checkBox = ((CheckBox)item.FindControl("chkSelectItem"));
                            if (checkBox.IsNotNull() && checkBox.Visible)
                            {
                                checkBoxCount++;
                                if (checkBox.Checked)
                                {
                                    checkCount++;
                                }
                            }
                        }
                        if (checkBoxCount == checkCount && checkBoxCount > 0)
                        {
                            GridHeaderItem item = grdOrderDetails.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
                            CheckBox checkBox = ((CheckBox)item.FindControl("chkSelectAll"));
                            checkBox.Checked = true;
                        }
                    }
                }

                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = (GridDataItem)e.Item;

                    //UAT-1051 Complio: Full social security number should not be displayed on grid exports. Added Duplicate Column with masked SSN
                    // and displayed the masked column on Export instead of actual column.
                    dataItem["_ApplicantSSN"].Text = Presenter.GetMaskedSSN(Convert.ToString(dataItem["_ApplicantSSN"].Text));

                    // UAT-806 Creation of granular permissions for Client Admin users
                    if (!CurrentViewContext.IsSSNDisabled && CurrentViewContext.SSNPermissionCode.ToUpper() == EnumSystemPermissionCode.MASKED_READ_PERMISSION.GetStringValue())
                    {
                        dataItem["ApplicantSSN"].Text = Presenter.GetMaskedSSN(Convert.ToString(dataItem["ApplicantSSN"].Text));
                    }
                    else if (!CurrentViewContext.IsSSNDisabled)
                    {
                        dataItem["ApplicantSSN"].Text = Presenter.GetFormattedSSN(Convert.ToString(dataItem["ApplicantSSN"].Text));
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

        #region DropDown Events

        /// <summary>
        /// Rebinds the data in the grdVerificationItemData for selected client. 
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that
        ///  contains the event data.</param>
        protected void ddlTenantName_ItemSelected(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (!ddlTenantName.SelectedValue.IsNullOrEmpty())
            {
                SelectedTenantId = Convert.ToInt32(ddlTenantName.SelectedValue);
                BindControlsForSelectedTenant();
            }
            else
            {
                ResetPageControls();
            }
        }

        protected void ddlTenantName_DataBound(object sender, EventArgs e)
        {
            ddlTenantName.Items.Insert(0, new RadComboBoxItem("--Select--"));
        }

        #endregion

        #region Button Events

        /// <summary>
        /// Displays the records in the order queue based on the search criteria.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarSearch_Click(object sender, EventArgs e)
        {
            ShowHideControls(true);
            chkSelectAllResults.Checked = false;
            CurrentViewContext.SelectedOrderIds = new Dictionary<int, bool>();

            if (ViewState["SelectedOrders"] != null)
            {
                ViewState["SelectedOrders"] = null;
            }
            txtReferenceText.Text = "";
            ResetGridFilters();
            grdOrderDetails.Focus();
        }

        /// <summary>
        /// Resets all the search controls and displays the records in the order queue with deafult checkbox selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarReset_Click(object sender, EventArgs e)
        {
            if (ViewState["SelectedOrders"] != null)
            {
                ViewState["SelectedOrders"] = null;
            }
            txtReferenceText.Text = "";
            CurrentViewContext.VirtualRecordCount = 0;
            ResetPageControls();
        }

        /// <summary>
        /// Redirect to Home page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void CmdBarCancel_Click(object sender, EventArgs e)
        {
            try
            {
                SetOrderApprovalQueueContract = null;
                Response.Redirect(String.Format(AppConsts.DASHBOARD_PAGE_NAME), false);
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


        protected void btnInvoiceApproval_Click(object sender, EventArgs e)
        {
            try
            {
                //List<Int32> lstApproveInvoiceOrders = new List<Int32>(CurrentViewContext.SelectedOrderIds.Where(cond => cond.Value).Select(cond => cond.Key));                
                List<String> lstExistingpaidOrders = new List<String>();
                String strExistingOrders = String.Join(",", CurrentViewContext.SelectedOrderIds.Where(cond => cond.Value).Select(cond => cond.Key));
                if (!strExistingOrders.IsNullOrEmpty() && hdnConfirmSave.Value == "0")
                {

                    lstExistingpaidOrders = Presenter.IsOrderExistForCurrentYear(strExistingOrders);
                }

                if (lstExistingpaidOrders.Count > 0 && hdnConfirmSave.Value == "0")
                {
                    var popUpAlertMsg = "Some user(s) already have an approved order(s) in last 365 days associated with following order number(s): <br/><br/>" + String.Join("<br/>", lstExistingpaidOrders) + "<br/><br/>Do you still want to continue?";
                    //Strings = Strings + "<br/>";
                    //Strings = Strings + String.Join("<br/>", lstExistingpaidOrders);
                    //var a= HttpUtility.HtmlDecode(Strings);
                    //lblPopUpText.InnerText = Strings;
                    hdnPopUpText.Value = popUpAlertMsg;
                    //hdnPopUpText.Value = "Some user(s) already have an approved order in last 365 days associated with following order number(s): " + String.Join(Environment.NewLine, lstExistingpaidOrders) + ". Do you still want to continue?";
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "confirmClick();", true);
                    return;
                }
                else
                {

                    bool flagIsCardWithApproval = false;
                    bool isADBAdmin = SysXWebSiteUtils.SessionService.IsSysXAdmin;
                    List<String> lstCCApprovalOrders = new List<String>();
                    foreach (GridDataItem dataItem in grdOrderDetails.Items)
                    {
                        CheckBox checkBox = ((CheckBox)dataItem.FindControl("chkSelectItem"));
                        Label lblIsCardWithApproval = ((Label)dataItem.FindControl("lblIsCardWithApproval"));
                        if (checkBox.Checked && lblIsCardWithApproval.Text.ToLower() == "true")
                        {
                            flagIsCardWithApproval = true;
                        }

                    }
                    if (flagIsCardWithApproval && hdnConfirmApproveOrderPayment.Value == "0" && isADBAdmin)
                    {
                        //hdnConfirmApproveOrderPaymentPopUpText.Value = "Some or all of the selected orders are set up as School Approval Only and they need approval from applicant's school. Do you still want to continue?";
                        System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ShowCCOrdersRequirApprovalConfirmationPopup();", true);
                        return;
                    }
                    else
                    {
                        hdnConfirmSave.Value = "0";
                        hdnConfirmApproveOrderPayment.Value = "0";
                        Presenter.SaveApproveInvoiceOrdersValueSet();
                        if (!String.IsNullOrEmpty(CurrentViewContext.SuccessMessage))
                        {
                            base.ShowSuccessMessage(CurrentViewContext.SuccessMessage);
                            if (ViewState["SelectedOrders"] != null)
                            {
                                ViewState["SelectedOrders"] = null;
                            }
                            txtReferenceText.Text = "";
                            grdOrderDetails.Rebind();
                        }
                        if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                        {
                            base.ShowErrorMessage(CurrentViewContext.ErrorMessage);
                        }
                        if (!String.IsNullOrEmpty(CurrentViewContext.InfoMessage))
                        {
                            base.ShowInfoMessage(CurrentViewContext.InfoMessage);
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

        #endregion

        #region CheckBox Events

        protected void chkSelectItem_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox checkBox = sender as CheckBox;
                Boolean isChecked = false;
                //Boolean isMapped = false;
                if (checkBox.IsNull())
                {
                    return;
                }
                GridDataItem dataItem = (GridDataItem)checkBox.NamingContainer;
                Dictionary<Int32, Boolean> selectedItems = CurrentViewContext.SelectedOrderIds;
                Int32 orderID = (Int32)dataItem.GetDataKeyValue("OrderId");
                isChecked = ((CheckBox)dataItem.FindControl("chkSelectItem")).Checked;
                //isMapped = Convert.ToBoolean(((Label)(dataItem.FindControl("lblIsInvoiceApproval"))).Text);

                if (selectedItems.IsNotNull() && selectedItems.ContainsKey(orderID))//&& isMapped
                {
                    selectedItems[orderID] = isChecked;
                }
                else if (selectedItems.IsNotNull() && selectedItems.ContainsKey(orderID) && !isChecked)//&& !isMapped
                {
                    selectedItems.Remove(orderID);
                }
                else
                {
                    selectedItems.Add(orderID, isChecked);
                }

                CurrentViewContext.SelectedOrderIds = selectedItems;
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

        #region Methods

        #region Private Methods

        #region UAT-3861 Client assignments should be retained on package copy (hierarchy or compliance mapping)

        /// <summary>
        /// Show and Hide Controls 
        /// </summary>
        /// <param name="IsVisible"></param>
        private void ShowHideControls(Boolean IsVisible)
        {
            grdOrderDetails.Visible = IsVisible;
            txtReferenceText.Visible = IsVisible;
            btnInvoiceApproval.Visible = IsVisible;
            chkSelectAllResults.Visible = IsVisible;
        }

        #endregion

        /// <summary>
        /// Sets th controls on the page when page is loaded.
        /// </summary>
        /// <param name="args"></param>
        private void SetPageControls(Dictionary<String, String> args)
        {
            if (args.ContainsKey("UpdatedStatus"))
            {
                String updatedStatus = Convert.ToString(args["UpdatedStatus"]);

                if (!string.IsNullOrWhiteSpace(updatedStatus))
                {
                    ShowSuccessMessage(updatedStatus);
                }
            }
            if (args.ContainsKey("SelectedTenantId"))
            {
                SelectedTenantId = Convert.ToInt32(args["SelectedTenantId"]);
                if (divTenant.Visible)
                    ddlTenantName.SelectedValue = SelectedTenantId.ToString();
                if (GetGridSearchContract.IsNotNull())
                {
                    OrderApprovalQueueContract orderApprovalQueueContract = GetGridSearchContract;
                    SelectedTenantId = orderApprovalQueueContract.ClientID;
                    if (divTenant.Visible)
                        ddlTenantName.SelectedValue = SelectedTenantId.ToString();
                    BindControlsForSelectedTenant();
                    SelectedOrderStatusCode = orderApprovalQueueContract.LstStatusCode;
                    SelectedPaymentTypeCode = orderApprovalQueueContract.LstPaymentType;
                    CurrentViewContext.lstSelectedOrderPkgType = orderApprovalQueueContract.lstOrderPackageTypes;

                    txtFirstName.Text = orderApprovalQueueContract.FirstName;
                    txtLastName.Text = orderApprovalQueueContract.LastName;

                    if (CurrentViewContext.SSNPermissionCode.ToUpper() == EnumSystemPermissionCode.MASKED_READ_PERMISSION.GetStringValue().ToUpper())
                    {
                        if (!orderApprovalQueueContract.SSN.IsNullOrEmpty())
                        {
                            CurrentViewContext.ApplicantSSN = orderApprovalQueueContract.SSN.Substring(orderApprovalQueueContract.SSN.Length - AppConsts.FOUR);
                            txtSSN.Text = CurrentViewContext.ApplicantSSN;
                        }
                    }
                    else
                    {
                        CurrentViewContext.ApplicantSSN = orderApprovalQueueContract.SSN;
                        txtSSN.Text = CurrentViewContext.ApplicantSSN;
                    }

                    if (orderApprovalQueueContract.OrderID > AppConsts.NONE)
                        txtOrderNumber.Text = orderApprovalQueueContract.OrderID.ToString();
                    //UAT-2193: Remove Rush Orders check box
                    //if (orderApprovalQueueContract.ShowOnlyRushOrder.HasValue)
                    //    chkShowRushOrders.Checked = orderApprovalQueueContract.ShowOnlyRushOrder.Value;
                    if (orderApprovalQueueContract.LstOrderCreatedDate.IsNotNull() && orderApprovalQueueContract.LstOrderCreatedDate.Count > 0)
                    {
                        dpOdrCrtFrm.SelectedDate = orderApprovalQueueContract.LstOrderCreatedDate[0];
                        dpOdrCrtTo.SelectedDate = orderApprovalQueueContract.LstOrderCreatedDate[1].AddDays(-1);
                    }
                    if (orderApprovalQueueContract.LstOrderPaidDate.IsNotNull() && orderApprovalQueueContract.LstOrderPaidDate.Count > 0)
                    {
                        dpOdrPaidFrm.SelectedDate = orderApprovalQueueContract.LstOrderPaidDate[0];
                        dpOdrPaidTo.SelectedDate = orderApprovalQueueContract.LstOrderPaidDate[1].AddDays(-1);
                    }
                    if (!orderApprovalQueueContract.DeptProgramMappingIDs.IsNullOrEmpty())
                    {
                        hdnTenantId.Value = orderApprovalQueueContract.ClientID.ToString();
                        hdnHierarchyLabel.Value = orderApprovalQueueContract.NodeLabel;
                        hdnDepartmntPrgrmMppng.Value = orderApprovalQueueContract.DeptProgramMappingIDs.ToString();
                    }

                    //Custom paging arguments
                    //Changes related to UAT-1456.
                    CurrentViewContext.GridCustomPaging = orderApprovalQueueContract.GridCustomPagingArguments;
                    if (!CurrentViewContext.GridCustomPaging.SortExpression.IsNullOrEmpty())
                    {
                        GridSortExpression gridSortExpression = new GridSortExpression();
                        gridSortExpression.FieldName = CurrentViewContext.GridCustomPaging.SortExpression;
                        gridSortExpression.SortOrder = CurrentViewContext.GridCustomPaging.SortDirectionDescending ? GridSortOrder.Descending : GridSortOrder.Ascending;
                        grdOrderDetails.MasterTableView.SortExpressions.Add(gridSortExpression);
                    }
                    //CurrentViewContext.PageSize = orderApprovalQueueContract.GridCustomPagingArguments.PageSize;
                    //CurrentViewContext.CurrentPageIndex = orderApprovalQueueContract.GridCustomPagingArguments.CurrentPageIndex;
                    //CurrentViewContext.VirtualRecordCount = orderApprovalQueueContract.GridCustomPagingArguments.VirtualPageCount;

                    //if (orderApprovalQueueContract.LstArchiveState.Count > 0)
                    //{
                    //    rblOrderState.SelectedValue = orderApprovalQueueContract.LstArchiveState.FirstOrDefault();
                    //}
                }
                else
                {
                    if (divTenant.Visible)
                    {
                        SelectedTenantId = 0;
                        ddlTenantName.SelectedIndex = 0;
                    }
                    else
                    {
                        SelectedTenantId = TenantId;
                    }
                    hdnTenantId.Value = SelectedTenantId.ToString();
                }
            }
        }

        /// <summary>
        /// Removes all the filters and Sorting on the grid and clears the variables.
        /// </summary>
        private void ResetGridFilters()
        {
            grdOrderDetails.MasterTableView.FilterExpression = null;
            CurrentViewContext.GridCustomPaging.SortExpression = null;
            grdOrderDetails.MasterTableView.SortExpressions.Clear();
            grdOrderDetails.CurrentPageIndex = 0;
            grdOrderDetails.MasterTableView.CurrentPageIndex = 0;
            //foreach (GridColumn column in grdOrderDetails.MasterTableView.RenderColumns)
            //{
            //    if (column.ColumnType == "GridBoundColumn")
            //    {
            //        GridBoundColumn boundColumn = (GridBoundColumn)column;
            //        String columnName = boundColumn.UniqueName.ToString();
            //        grdOrderDetails.MasterTableView.GetColumnSafe(columnName).CurrentFilterFunction = GridKnownFunction.NoFilter;
            //        grdOrderDetails.MasterTableView.GetColumnSafe(columnName).CurrentFilterValue = String.Empty;
            //    }
            //}
            //CurrentViewContext.GridCustomPaging.FilterColumns = new List<String>();
            //CurrentViewContext.GridCustomPaging.FilterOperators = new List<String>();
            //CurrentViewContext.GridCustomPaging.FilterValues = new ArrayList();

            //ViewState["FilterColumns"] = CurrentViewContext.GridCustomPaging.FilterColumns;
            //ViewState["FilterOperators"] = CurrentViewContext.GridCustomPaging.FilterOperators;
            //ViewState["FilterValues"] = CurrentViewContext.GridCustomPaging.FilterValues;
            grdOrderDetails.Rebind();
        }

        /// <summary>
        /// Sets property visible true for the controls once tanent dropdown is populated and value is selected.
        /// </summary>
        private void BindControlsForSelectedTenant()
        {
            Presenter.GetOrderStatusList();
            Presenter.GetPaymentTypeList();
            //Presenter.GetArchiveStateList();
            Presenter.GetOrderPackageTypes();
            hdnDepartmntPrgrmMppng.Value = String.Empty;
            hdnHierarchyLabel.Value = String.Empty;
            lblinstituteHierarchy.Text = String.Empty;
        }

        private void ResetPageControls()
        {
            if (divTenant.Visible)
            {
                ddlTenantName.SelectedIndex = 0;
                SelectedTenantId = 0;
                lstOrderStatus = new List<lkpOrderStatu>();
                lstPaymentType = new List<lkpPaymentOption>();
                CurrentViewContext.lstOrderPackageType = new List<lkpOrderPackageType>();
            }
            SelectedOrderStatusCode = new List<String>();
            SelectedPaymentTypeCode = new List<String>();
            CurrentViewContext.lstSelectedOrderPkgType = new List<String>();
            txtFirstName.Text = String.Empty;
            txtLastName.Text = String.Empty;
            txtOrderNumber.Text = String.Empty;
            txtSSN.Text = String.Empty;
            ApplicantSSN = null;
            //UAT-2193: Remove Rush Orders check box
            // chkShowRushOrders.Checked = false;
            dpOdrCrtFrm.SelectedDate = null;
            dpOdrCrtTo.SelectedDate = null;
            dpOdrPaidFrm.SelectedDate = null;
            dpOdrPaidTo.SelectedDate = null;
            hdnDepartmntPrgrmMppng.Value = String.Empty;
            hdnHierarchyLabel.Value = String.Empty;
            lblinstituteHierarchy.Text = String.Empty;
            SetOrderApprovalQueueContract = null;
            VirtualPageCount = 0;
            rblOrderState.SelectedValue = null;
            dvRecords.Visible = false;
            ResetGridFilters();

            chkSelectAllResults.Checked = false;
            CurrentViewContext.SelectedOrderIds = new Dictionary<int, bool>();
        }

        #region UAT-806 Creation of granular permissions for Client Admin users

        /// <summary>
        /// Hide Show grid and page controls
        /// </summary>
        private void HideShowControlsForGranularPermission()
        {
            if (CurrentViewContext.IsDOBDisable)
            {
                grdOrderDetails.MasterTableView.GetColumn("DateOfBirth").Visible = false;
            }
            if (!CurrentViewContext.IsSSNDisabled && CurrentViewContext.SSNPermissionCode.ToUpper() == EnumSystemPermissionCode.NO_ACCESS_PERMISSION.GetStringValue().ToUpper())
            {
                divSSN.Visible = false;
                grdOrderDetails.MasterTableView.GetColumn("ApplicantSSN").Visible = false;
                grdOrderDetails.MasterTableView.GetColumn("_ApplicantSSN").Visible = false;
            }
        }
        #endregion

        private void SetSessionValues()
        {
            OrderApprovalQueueContract searchContract = new OrderApprovalQueueContract();
            searchContract.FirstName = txtFirstName.Text.Trim();
            searchContract.LastName =  txtLastName.Text.Trim();
            searchContract.OrderNumber = txtOrderNumber.Text.Trim();
            searchContract.OrderStatusCode = chkOrderStatus.SelectedValue;
            searchContract.PaymentTypeCode = chkPaymentType.SelectedValue;
            searchContract.OrderPackageTypeCode = cmbOrderType.SelectedValue;
            searchContract.OrderFromDate = dpOdrCrtFrm.SelectedDate;
            searchContract.OrderToDate = dpOdrCrtTo.SelectedDate;
            searchContract.OrderPaidFromDate = dpOdrPaidFrm.SelectedDate;
            searchContract.OrderPaidToDate = dpOdrPaidTo.SelectedDate;
            searchContract.SSN = txtSSN.Text.Trim();

            SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.ORDER_QUEUE_SEARCH_SESSION_KEY, searchContract);
        }

        private void GetSessionValues()
        {
             OrderApprovalQueueContract sessionContract = new OrderApprovalQueueContract();
             if (!Request.QueryString["args"].IsNull())
             {
                 Dictionary<String, String> args = new Dictionary<String, String>();
                 args.ToDecryptedQueryString(Request.QueryString["args"]);
                 if (args.ContainsKey("ReadSession")
                     && Convert.ToBoolean(args["ReadSession"])
                     && !Session[AppConsts.ORDER_QUEUE_SEARCH_SESSION_KEY].IsNullOrEmpty())
                 {
                     sessionContract = SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.ORDER_QUEUE_SEARCH_SESSION_KEY) as OrderApprovalQueueContract;
                     txtFirstName.Text=sessionContract.FirstName;
                     txtLastName.Text = sessionContract.LastName;
                     txtOrderNumber.Text = sessionContract.OrderNumber;
                     chkOrderStatus.SelectedValue = sessionContract.OrderStatusCode;
                     chkPaymentType.SelectedValue = sessionContract.PaymentTypeCode;
                     cmbOrderType.SelectedValue = sessionContract.OrderPackageTypeCode;
                     dpOdrCrtFrm.SelectedDate = sessionContract.OrderFromDate;
                     dpOdrPaidTo.SelectedDate = sessionContract.OrderToDate;
                     dpOdrPaidFrm.SelectedDate = sessionContract.OrderPaidFromDate;
                     dpOdrPaidTo.SelectedDate = sessionContract.OrderPaidToDate;
                     txtSSN.Text = sessionContract.SSN;
                     grdOrderDetails.Rebind();
                 }
             }
        }

        #endregion

        #region Public Methods

        #endregion

        #endregion

        #region Action Permission

        /// <summary>
        /// Set action level permissions
        /// </summary>
        /// <param name="ctrlCollection">ctrlCollection</param>
        /// <param name="screenName">screenName</param>
        protected override void ApplyActionLevelPermission(List<Entity.ClientEntity.ClsFeatureAction> ctrlCollection, string screenName = "")
        {
            base.ApplyActionLevelPermission(ctrlCollection, screenName);
            ApplyPermisions();
        }

        //public override List<ClsFeatureAction> ActionCollection
        //{
        //    get
        //    {

        //       // Dictionary<String, ClsFeatureAction> actionCollection = new Dictionary<String, ClsFeatureAction>();
        //        //actionCollection.Add("Institution", ddlTenantName);
        //        //actionCollection.Add("Applicant First Name", txtFirstName);
        //        //actionCollection.Add("Applicant Last Name", txtLastName);
        //        //actionCollection.Add("Order ID", txtOrderNumber);
        //        //actionCollection.Add("Payment Status(s)", chkOrderStatus);
        //        //actionCollection.Add("Payment Type(s)", chkPaymentType);
        //        //actionCollection.Add("Order Created Date From", dpOdrCrtFrm);
        //        //actionCollection.Add("Order Created Date To", dpOdrCrtTo);
        //        //actionCollection.Add("Order Paid Date From", dpOdrPaidFrm);
        //        //actionCollection.Add("Order Paid Date To", dpOdrPaidTo);
        //        //return actionCollection;

        //        return null;
        //    }
        //}

        /// <summary>
        /// Set the permission on control based action permission 
        /// </summary>
        private void ApplyPermisions()
        {
            List<Entity.FeatureRoleAction> permission = base.ActionPermission;
            if (permission.IsNotNull())
            {
                permission.ForEach(x =>
                {
                    switch (x.PermissionID)
                    {
                        case AppConsts.ONE:
                            {
                                break;
                            }
                        case AppConsts.THREE:
                        case AppConsts.FOUR:
                            {

                                if (x.FeatureAction.CustomActionId == "ViewDetail")
                                {
                                    grdOrderDetails.MasterTableView.GetColumn("ViewDetail").Display = false;
                                }
                                break;
                            }
                    }

                }
                    );
            }
        }
        #endregion

        /// <summary>
        /// Change the Order Package Type name to bind in dropdown, as per its code.
        /// </summary>
        /// <param name="orderPkgType"></param>
        private void AddOrderPackageTypes(lkpOrderPackageType orderPkgType)
        {
            if (orderPkgType.OPT_Code != OrderPackageTypes.COMPLIANCE_RUSHORDER_PACKAGE.GetStringValue())
            {
                var _typeName = String.Empty;

                if (orderPkgType.OPT_Code.ToLower() == OrderPackageTypes.BACKGROUND_PACKAGE.GetStringValue().ToLower())
                {
                    _typeName = "Screening Packages";
                }
                else if (orderPkgType.OPT_Code.ToLower() == OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue().ToLower())
                {
                    _typeName = "Tracking Packages";
                }
                else if (orderPkgType.OPT_Code.ToLower() == OrderPackageTypes.COMPLIANCE_AND_BACKGROUMD_PACKAGE.GetStringValue().ToLower())
                {
                    _typeName = "Tracking & Screening Packages";
                }

                #region UAT-3083
                else if (orderPkgType.OPT_Code.ToLower() == OrderPackageTypes.TRACKING_ITEM_PAYMENT.GetStringValue().ToLower())
                {
                    _typeName = "Tracking Item Order";
                }
                else if (orderPkgType.OPT_Code.ToLower() == OrderPackageTypes.REQUIREMENT_ITEM_PAYMENT.GetStringValue().ToLower())
                {
                    _typeName = "Rotation Item Order";
                }
                #endregion

                if (!_typeName.IsNullOrEmpty())
                {
                    cmbOrderType.Items.Add(new RadComboBoxItem
                    {
                        Text = _typeName,
                        Value = orderPkgType.OPT_Code
                    });
                }
            }
        }

        private void ApplySSNMask()
        {
            if (CurrentViewContext.SSNPermissionCode.ToUpper() == EnumSystemPermissionCode.MASKED_READ_PERMISSION.GetStringValue().ToUpper())
            {
                //txtSSN.Mask = AppConsts.SSN_MASK_FORMATE; //@"\#\#\#-\#\#-####"
                txtSSN.Mask = AppConsts.SSN_MASK_FORMAT_ALPHANUMERIC;
            }
        }

        protected void chkSelectAllResults_CheckedChanged(object sender, EventArgs e)
        {
            if (CurrentViewContext.VirtualPageCount > 0)
            {
                bool needToCheckboxChecked = false;

                if (((CheckBox)sender).Checked)
                {
                    Presenter.GetOrdersIdsToSelectAllRecords();
                    needToCheckboxChecked = true;
                }
                else
                {
                    CurrentViewContext.SelectedOrderIds = new Dictionary<int, bool>();
                }


                foreach (GridDataItem item in grdOrderDetails.Items)
                {
                    CheckBox checkBox = ((CheckBox)item.FindControl("chkSelectItem"));
                    checkBox.Checked = needToCheckboxChecked;
                }

                GridHeaderItem headerItem = grdOrderDetails.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
                CheckBox headerCheckBox = ((CheckBox)headerItem.FindControl("chkSelectAll"));
                headerCheckBox.Checked = needToCheckboxChecked;
            }
            chkSelectAllResults.Focus();
        }
    }
}

