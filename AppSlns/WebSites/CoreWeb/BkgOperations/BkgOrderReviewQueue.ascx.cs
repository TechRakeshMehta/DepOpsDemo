
#region Namespaces

#region System Defined

using System;
using System.Linq;
using System.Collections.Generic;



#endregion

#region Application Specific

using INTSOF.Utils;
using Telerik.Web.UI;
using Entity.ClientEntity;
using CoreWeb.Shell;
using CoreWeb.IntsofSecurityModel;
using INTSOF.UI.Contract.BkgOperations;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System.Xml;

#endregion

#endregion

namespace CoreWeb.BkgOperations.Views
{
    public partial class BkgOrderReviewQueue : BaseUserControl, IBkgOrderReviewQueueView
    {

        #region Variables

        private BkgOrderReviewQueuePresenter _presenter = new BkgOrderReviewQueuePresenter();
        private String _viewType;
        private Int32 tenantId = 0;
        private CustomPagingArgsContract _gridCustomPaging = null;
        private BkgOrderReviewQueueContract _gridBkgOrderReviewQueueContract = null;
        #endregion

        #region Properties


        public BkgOrderReviewQueuePresenter Presenter
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

        Int32 IBkgOrderReviewQueueView.SelectedTenantId
        {
            get
            {
                if (String.IsNullOrEmpty(ddlTenantName.SelectedValue))
                    return 0;
                return Convert.ToInt32(ddlTenantName.SelectedValue);
            }
            set
            {
                if (value > 0)
                {
                    ddlTenantName.SelectedValue = value.ToString();
                }
                else
                {
                    ddlTenantName.SelectedIndex = value;
                }
            }
        }

        Int32 IBkgOrderReviewQueueView.TenantId
        {
            get
            {
                if (tenantId == 0)
                {
                    // tenantId = Presenter.GetTenantId();
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    if (user.IsNotNull())
                    {
                        tenantId = user.TenantId.HasValue ? user.TenantId.Value : 0;
                    }
                }
                return tenantId;
            }
            set { tenantId = value; }
        }

        Int32 IBkgOrderReviewQueueView.CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        public IBkgOrderReviewQueueView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        List<Entity.Tenant> IBkgOrderReviewQueueView.LstTenant
        {
            get;
            set;
        }

        String IBkgOrderReviewQueueView.ApplicantFirstName
        {
            get
            {
                return txtFirstName.Text;
            }
            set
            {
                txtFirstName.Text = value;
            }
        }

        String IBkgOrderReviewQueueView.ApplicantLastName
        {
            get
            {
                return txtLastName.Text;
            }
            set
            {
                txtLastName.Text = value;
            }
        }

        String IBkgOrderReviewQueueView.OrderNumber
        {
            get
            {
                if (!txtOrderNumber.Text.IsNullOrEmpty())
                {
                    return Convert.ToString(txtOrderNumber.Text);
                }
                return String.Empty;
            }
            set
            {
                txtOrderNumber.Text = Convert.ToString(value);
            }
        }

        List<BkgReviewCriteria> IBkgOrderReviewQueueView.LstReviewCriterias
        {
            set
            {
                if (value.IsNotNull())
                {
                    cmbReviewCriteria.DataSource = value;
                    cmbReviewCriteria.DataBind();
                    cmbReviewCriteria.Items.Insert(0, new RadComboBoxItem("--Select--"));
                }
            }
        }

        List<lkpBkgSvcGrpReviewStatusType> IBkgOrderReviewQueueView.LstSvcGrpReviewStatus
        {
            set
            {
                if (!value.IsNullOrEmpty())
                {
                    cmbSGReviewStatusTypes.DataSource = value.OrderBy(x => x.BSGRS_SortOrder);
                    cmbSGReviewStatusTypes.DataBind();
                    //cmbSGReviewStatusTypes.Items.Insert(0, new RadComboBoxItem("--Select--"));

                    //List<Int32?> lstDefaultSelectedReviewStatusType = new List<Int32?>();
                    //lstDefaultSelectedReviewStatusType.Add(Presenter.GetSvcGroupReviewStatusTypeIdByCode(BkgSvcGrpReviewStatusType.FIRST_REVIEW.GetStringValue()));
                    //lstDefaultSelectedReviewStatusType.Add(Presenter.GetSvcGroupReviewStatusTypeIdByCode(BkgSvcGrpReviewStatusType.SECOND_REVIEW.GetStringValue()));
                    //foreach (RadComboBoxItem item in cmbSGReviewStatusTypes.Items)
                    //{
                    //    if (lstDefaultSelectedReviewStatusType.Contains(Convert.ToInt32(item.Value)))
                    //    {
                    //        item.Checked = true;
                    //    }
                    //}

                }
            }
        }

        List<Int32?> IBkgOrderReviewQueueView.SvcGrpReviewStatusTypeIDs
        {
            get
            {
                List<Int32?> lstSelectedSGReviewStatusType = new List<Int32?>();
                if (cmbSGReviewStatusTypes.CheckedItems.IsNotNull() && cmbSGReviewStatusTypes.CheckedItems.Count > 0)
                {
                    foreach (RadComboBoxItem item in cmbSGReviewStatusTypes.Items)
                    {
                        if (item.Checked)
                            lstSelectedSGReviewStatusType.Add(Convert.ToInt32(item.Value));
                    }
                }
                return lstSelectedSGReviewStatusType;
            }
            set
            {
                List<Int32?> lstSelectedSGReviewStatusType = value;
                foreach (RadComboBoxItem item in cmbSGReviewStatusTypes.Items)
                {
                    if (lstSelectedSGReviewStatusType.Contains(Convert.ToInt32(item.Value)))
                        item.Checked = true;
                }
            }
        }

        List<lkpBkgSvcGrpStatusType> IBkgOrderReviewQueueView.LstSvcGrpStatus
        {
            set
            {
                if (value.IsNotNull())
                {
                    cmbSGStatusTypes.DataSource = value.OrderBy(x => x.BSGS_ID);
                    cmbSGStatusTypes.DataBind();
                    cmbSGStatusTypes.Items.Insert(0, new RadComboBoxItem("--Select--"));
                }
            }
        }

        Int32? IBkgOrderReviewQueueView.SelectedReviewCriteriaId
        {
            get
            {
                if (!cmbReviewCriteria.SelectedValue.IsNullOrEmpty())
                {
                    return Convert.ToInt32(cmbReviewCriteria.SelectedValue);
                }
                return 0;
            }
            set
            {
                if (value.IsNotNull())
                {
                    cmbReviewCriteria.SelectedValue = Convert.ToString(value);
                }
            }
        }

        //Int32? IBkgOrderReviewQueueView.SvcGrpReviewStatusTypeID
        //{
        //    get
        //    {
        //        //if (!cmbSGReviewStatusTypes.SelectedValue.IsNullOrEmpty())
        //        //{
        //        //    return Convert.ToInt32(cmbSGReviewStatusTypes.SelectedValue);
        //        //}
        //        return 0;
        //    }
        //    set
        //    {
        //        //if (value.IsNotNull())
        //        //{
        //        //    cmbSGReviewStatusTypes.SelectedValue = value.ToString();
        //        //}
        //    }
        //}

        Int32? IBkgOrderReviewQueueView.SvcGrpStatusTypeID
        {
            get
            {
                if (!cmbSGStatusTypes.SelectedValue.IsNullOrEmpty())
                {
                    return Convert.ToInt32(cmbSGStatusTypes.SelectedValue);
                }
                return 0;
            }
            set
            {
                if (value.IsNotNull())
                {
                    cmbSGStatusTypes.SelectedValue = value.ToString();
                }
            }
        }

        String IBkgOrderReviewQueueView.ErrorMessage
        {
            get;
            set;
        }

        String IBkgOrderReviewQueueView.SuccessMessage
        {
            get;
            set;
        }

        String IBkgOrderReviewQueueView.InfoMessage
        {
            get;
            set;
        }

        List<BkgOrderReviewQueueContract> IBkgOrderReviewQueueView.BkgOrderReviewQueueData { get; set; }

        DateTime? IBkgOrderReviewQueueView.OrderFromDate
        {
            get
            {
                return dpOdrCrtFrm.SelectedDate;
            }
            set
            {
                if (!value.IsNullOrEmpty())
                {
                    dpOdrCrtFrm.SelectedDate = value;
                }
            }

        }

        DateTime? IBkgOrderReviewQueueView.OrderToDate
        {
            get
            {
                return dpOdrCrtTo.SelectedDate;
            }
            set
            {
                if (!value.IsNullOrEmpty())
                {
                    dpOdrCrtTo.SelectedDate = value;
                }
            }

        }

        DateTime? IBkgOrderReviewQueueView.SvcGrpUpdatedFromDate
        {
            get
            {
                return dpSGUpdtFrm.SelectedDate;
            }
            set
            {
                if (!value.IsNullOrEmpty())
                {
                    dpSGUpdtFrm.SelectedDate = value;
                }
            }

        }

        DateTime? IBkgOrderReviewQueueView.SvcGrpUpdatedToDate
        {
            get
            {
                return dpSGUpdtTo.SelectedDate;
            }
            set
            {
                if (!value.IsNullOrEmpty())
                {
                    dpSGUpdtTo.SelectedDate = value;
                }
            }
        }

        //Int32? IBkgOrderReviewQueueView.TargetHierarchyNodeId
        //{
        //    get
        //    {
        //        if (!String.IsNullOrEmpty(hdnDepartmntPrgrmMppng.Value))
        //        {
        //            return Convert.ToInt32(hdnDepartmntPrgrmMppng.Value);
        //        }
        //        return null;
        //    }
        //}

        String IBkgOrderReviewQueueView.TargetHierarchyNodeIds
        {
            get
            {
                if (!String.IsNullOrEmpty(hdnDepartmntPrgrmMppng.Value))
                {
                    return hdnDepartmntPrgrmMppng.Value;
                }
                return String.Empty;
            }
        }

        /// <summary>
        /// Indicates wheather Select Client dropdown will be visible or not.
        /// </summary>
        Boolean IBkgOrderReviewQueueView.IsAdminUser
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsAdminUser"]);
            }
            set
            {
                ViewState["IsAdminUser"] = value;
            }
        }

        BkgOrderReviewQueueContract IBkgOrderReviewQueueView.SetBkgOrderReviewQueueContract
        {
            set
            {
                if (value.IsNotNull())
                    value.HierarchyLabel = lblinstituteHierarchy.Text.HtmlDecode();
                var serializer = new XmlSerializer(typeof(BkgOrderReviewQueueContract));
                var sb = new StringBuilder();
                using (TextWriter writer = new StringWriter(sb))
                {
                    serializer.Serialize(writer, value);
                }
                Session[AppConsts.BKG_REVIEW_QUEUE_OBJECT_SESSION_KEY] = sb.ToString();
            }
        }

        /// <summary>
        /// get object of shared class of search contract
        /// </summary>
        BkgOrderReviewQueueContract IBkgOrderReviewQueueView.GetBkgOrderReviewQueueContract
        {
            get
            {
                if (_gridBkgOrderReviewQueueContract.IsNull())
                {
                    var serializer = new XmlSerializer(typeof(BkgOrderReviewQueueContract));
                    TextReader reader = new StringReader(Convert.ToString(Session[AppConsts.BKG_REVIEW_QUEUE_OBJECT_SESSION_KEY]));

                    using (reader)
                    {
                        _gridBkgOrderReviewQueueContract = (BkgOrderReviewQueueContract)serializer.Deserialize(reader);
                    }
                }
                return _gridBkgOrderReviewQueueContract;
            }
        }
        //UAT-1683
        List<lkpArchiveState> IBkgOrderReviewQueueView.lstArchiveState
        {
            set
            {
                rbSubscriptionState.DataSource = value.OrderBy(x => x.AS_Code);
                rbSubscriptionState.DataBind();
                rbSubscriptionState.SelectedValue = ArchiveState.Active.GetStringValue();
            }
        }

        String IBkgOrderReviewQueueView.SelectedArchiveStateCode
        {
            get
            {
                if (!rbSubscriptionState.SelectedValue.IsNullOrEmpty())
                {
                    return rbSubscriptionState.SelectedValue == ArchiveState.All.GetStringValue() ? String.Empty : rbSubscriptionState.SelectedValue.ToString();
                }
                else
                {
                    return String.Empty;
                }
            }
            set
            {
                rbSubscriptionState.SelectedValue = value;
            }
        }

        String IBkgOrderReviewQueueView.SelectedSvcGrpReviewType
        {
            get
            {
                if (!rblReviewType.SelectedValue.IsNullOrEmpty())
                {
                    return rblReviewType.SelectedValue.ToString();
                }
                else
                {
                    return String.Empty;
                }
            }
            set
            {
                rblReviewType.SelectedValue = value;
            }
        }

        #region Custom Paging


        /// <summary>
        /// Current Page Index</summary>
        /// <value>
        /// Gets or sets the value for CurrentPageIndex.</value>
        Int32 IBkgOrderReviewQueueView.CurrentPageIndex
        {
            get
            {
                return grdBkgOrderReview.MasterTableView.CurrentPageIndex + 1;
            }
            set
            {
                if (grdBkgOrderReview.MasterTableView.CurrentPageIndex > 0)
                {
                    grdBkgOrderReview.MasterTableView.CurrentPageIndex = value - 1;
                }
            }
        }

        /// <summary>
        /// Page Size</summary>
        /// <value>
        /// Gets the value for PageSize.</value>
        Int32 IBkgOrderReviewQueueView.PageSize
        {
            get
            {
                // Maximum 100 record allowed from DB. 
                //return grdApplicantSearchData.PageSize > 100 ? 100 : grdApplicantSearchData.PageSize;
                return grdBkgOrderReview.PageSize;
            }
            set
            {
                grdBkgOrderReview.PageSize = value;
            }
        }

        /// <summary>
        /// Virtual Page Count</summary>
        /// <value>
        /// Sets the value for VirtualPageCount.</value>
        Int32 IBkgOrderReviewQueueView.VirtualRecordCount
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
                grdBkgOrderReview.VirtualItemCount = value;
                grdBkgOrderReview.MasterTableView.VirtualItemCount = value;
            }
        }

        /// <summary>
        /// To get object of shared class of custom paging
        /// </summary>
        CustomPagingArgsContract IBkgOrderReviewQueueView.GridCustomPaging
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

        #endregion

        #endregion

        #region Events

        #region Page Events
        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                base.Title = "Background Order Review Queue";
                base.SetPageTitle("Background Order Review Queue");

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
        /// Page Load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                if (!this.IsPostBack)
                {
                    Presenter.OnViewInitialized();
                    BindControls();
                    Dictionary<String, String> args = new Dictionary<String, String>();
                    if (!Request.QueryString["args"].IsNull())
                    {
                        args.ToDecryptedQueryString(Request.QueryString["args"]);
                        //UAT-2066:"Continue" button click from the supplement review screen should return the user to the order review queue
                        //This functionality worked in case if user redirect from supplement order screen
                        if (args.ContainsKey("ShowSuppSuccessMessage") && !args["ShowSuppSuccessMessage"].IsNullOrEmpty() && Convert.ToBoolean(args["ShowSuppSuccessMessage"]))
                        {
                            ShowMessageForSupplementOrder(args);
                        }
                        else if (args.ContainsKey("ShowSuppAutoReviewSuccessMessage") && !args["ShowSuppAutoReviewSuccessMessage"].IsNullOrEmpty() && Convert.ToBoolean(args["ShowSuppAutoReviewSuccessMessage"]))
                        {
                            ShowMessageForSupplementOrder(args);
                        }

                        SetPageControl(args);
                    }
                    else
                    {
                        grdBkgOrderReview.Visible = false;
                        Session[AppConsts.BKG_REVIEW_QUEUE_OBJECT_SESSION_KEY] = null;
                        CmdBarSearch.ClearButton.Style.Add("display", "none");
                    }
                    CmdBarSearch.SaveButton.ValidationGroup = "grpFormSubmit";
                    CmdBarSearch.ClearButton.ValidationGroup = "grpFormSubmit";

                    //Checking Default SG review status type as FR and SR

                }
                Presenter.OnViewLoaded();
                hdnTenantId.Value = CurrentViewContext.SelectedTenantId.ToString();
                lblinstituteHierarchy.Text = hdnHierarchyLabel.Value.HtmlEncode();
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

        #region Button Events
        /// <summary>
        /// To perform search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarSearch_Click(object sender, EventArgs e)
        {
            try
            {
                grdBkgOrderReview.Visible = true;
                //To reset grid filters 
                ResetGridFilters();
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
        /// To reset controls
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarReset_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentViewContext.VirtualRecordCount = 0;
                Presenter.GetTenants();
                BindControls();
                ResetControls();
                //To reset grid filters 
                ResetGridFilters();
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
        /// Redirect to Home page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarCancel_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentViewContext.SetBkgOrderReviewQueueContract = null;
                Session[AppConsts.BKG_REVIEW_QUEUE_OBJECT_SESSION_KEY] = null;
                Response.Redirect(String.Format(AppConsts.DASHBOARD_PAGE_NAME), true);
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

        protected void ddlTenantName_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                ResetInstitutionHierarchy();
                if (ddlTenantName.SelectedValue.IsNullOrEmpty() || CurrentViewContext.SelectedTenantId == AppConsts.NONE)
                {
                    ResetControls();
                    ResetGridFilters();
                }
                else
                {
                    CurrentViewContext.SelectedTenantId = Convert.ToInt32(ddlTenantName.SelectedValue);
                    BindDropDownControl();
                    Presenter.GetArchiveStateList();
                    rbSubscriptionState.Visible = true;
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
        /// Tenant Name DataBound event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTenantName_DataBound(object sender, EventArgs e)
        {
            try
            {
                ddlTenantName.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--Select--"));
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

        /// <summary>
        /// To set data source to the grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdBkgOrderReview_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                CurrentViewContext.BkgOrderReviewQueueData = new List<BkgOrderReviewQueueContract>();
                CurrentViewContext.GridCustomPaging.CurrentPageIndex = CurrentViewContext.CurrentPageIndex;
                CurrentViewContext.GridCustomPaging.PageSize = CurrentViewContext.PageSize;
                CurrentViewContext.GridCustomPaging = CurrentViewContext.GridCustomPaging;
                Presenter.PerformSearch();
                grdBkgOrderReview.DataSource = CurrentViewContext.BkgOrderReviewQueueData;
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
        /// Grid Item Command event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdBkgOrderReview_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                #region UAT-844 - ORDER REVIEW DETAILS SCREEN

                if (e.CommandName == "ViewDetail")
                {
                    //TO-DO code to open the Order Details screen.
                    Int32 selectedTenantId = 0;
                    if (!ddlTenantName.SelectedValue.IsNullOrEmpty())
                    {
                        selectedTenantId = CurrentViewContext.SelectedTenantId;
                    }
                    else
                    {
                        selectedTenantId = CurrentViewContext.TenantId;
                    }
                    Dictionary<String, String> queryString = new Dictionary<String, String>();
                    String orderPackageSvcGrpID = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrderPackageSvcGrpID"].ToString();
                    String orderId = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrderID"].ToString();
                    String orderNumber = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrderNumber"].ToString();
                    String supplementAutomationStatusID = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["SupplementAutomationStatusID"].ToString();
                    String url;
                    if (CurrentViewContext.IsAdminUser)//Is admin?
                    {
                        queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "SelectedTenantId", Convert.ToString(selectedTenantId) },
                                                                    { "OrderId", orderId},
                                                                    { AppConsts.QUERYSTRING_ORDER_PACKAGE_SERVICEGROUP_ID, orderPackageSvcGrpID},
                                                                    { AppConsts.QUERYSTRING_PARENT_SCREEN_NAME, AppConsts.BKG_ORDER_REVIEW_QUEUE},
                                                                    { AppConsts.ORDER_NUMBER, orderNumber},
                                                                    { AppConsts.QUERYSTRING_SUPPLEMENT_AUTOMATION_STATUS_ID, supplementAutomationStatusID},
                                                                 };

                        url = String.Format("~/BkgOperations/Pages/BkgOrderDetailPage.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());

                    }
                    else
                    {
                        queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "SelectedTenantId", Convert.ToString(selectedTenantId) },
                                                                    { "OrderId", orderId},
                                                                    { AppConsts.QUERYSTRING_ORDER_PACKAGE_SERVICEGROUP_ID, orderPackageSvcGrpID}, 
                                                                    { AppConsts.QUERYSTRING_PARENT_SCREEN_NAME, AppConsts.BKG_ORDER_REVIEW_QUEUE},
                                                                    {"pageName", "Order Summary"},
                                                                    { AppConsts.ORDER_NUMBER, orderNumber},
                                                                 };
                        url = String.Format("~/BkgOperations/Pages/ClientAdminBkgOrderDetailPage.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                    }
                    Response.Redirect(url, true);
                }

                #endregion

                #region EXPORT FUNCTIONALITY

                //This command fired when exporting start.
                if (e.CommandName.IsNullOrEmpty())
                {
                    if (e.Item is GridCommandItem)
                    {
                        #region HIDE SHOW ORDER ID COLUMN
                        grdBkgOrderReview.MasterTableView.GetColumn("OrderId").Display = true;
                        grdBkgOrderReview.MasterTableView.GetColumn("OrderIdTemp").Display = false;
                        #endregion
                    }
                }

                // Hide filter when exportig to pdf or word
                if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToPdfCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToWordCommandName
                    || e.CommandName == Telerik.Web.UI.RadGrid.ExportToExcelCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToCsvCommandName)
                {
                    base.ConfigureExport(grdBkgOrderReview);
                }

                // This command fired when exporting done
                if (e.CommandName == "Cancel")
                {
                    if (e.Item is GridCommandItem)
                    {
                        #region HIDE SHOW ORDER ID COLUMN
                        grdBkgOrderReview.MasterTableView.GetColumn("OrderId").Display = false;
                        grdBkgOrderReview.MasterTableView.GetColumn("OrderIdTemp").Display = true;
                        #endregion
                    }
                }

                #endregion
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
        /// Grid sort expression
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdBkgOrderReview_SortCommand(object sender, GridSortCommandEventArgs e)
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

        protected void grdBkgOrderReview_ItemDataBound(object sender, GridItemEventArgs e)
        {

            try
            {
                if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
                {
                    if (e.Item is GridDataItem)
                    {
                        GridDataItem dataItem = (GridDataItem)e.Item;

                        if (Convert.ToString(dataItem["CustomAttributes"].Text).Length > 80)
                        {
                            dataItem["CustomAttributes"].ToolTip = dataItem["CustomAttributes"].Text;
                            dataItem["CustomAttributes"].Text = (dataItem["CustomAttributes"].Text).ToString().Substring(0, 80) + "...";
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

        #region Methods

        #region Private Methods
        /// <summary>
        /// To bind controls
        /// </summary>
        private void BindControls()
        {
            ddlTenantName.DataSource = CurrentViewContext.LstTenant;
            ddlTenantName.DataBind();

            if (Presenter.IsDefaultTenant)
            {
                ddlTenantName.Enabled = true;
                CurrentViewContext.SelectedTenantId = 0;
            }
            else
            {
                CurrentViewContext.SelectedTenantId = CurrentViewContext.TenantId;
                BindDropDownControl();
            }
        }

        /// <summary>
        /// Removes all the filters and Sorting on the grid and clears the variables.
        /// </summary>
        private void ResetGridFilters()
        {
            grdBkgOrderReview.MasterTableView.SortExpressions.Clear();
            CurrentViewContext.GridCustomPaging.SortExpression = null;
            grdBkgOrderReview.CurrentPageIndex = 0;
            grdBkgOrderReview.MasterTableView.CurrentPageIndex = 0;
            grdBkgOrderReview.Rebind();
        }

        private void ResetControls()
        {
            CurrentViewContext.SetBkgOrderReviewQueueContract = null;
            txtFirstName.Text = String.Empty;
            txtLastName.Text = String.Empty;
            txtOrderNumber.Text = String.Empty;
            dpOdrCrtFrm.Clear();
            dpOdrCrtTo.Clear();
            dpSGUpdtFrm.Clear();
            dpSGUpdtTo.Clear();
            ResetInstitutionHierarchy();
            if (Presenter.IsDefaultTenant)
            {
                hdnTenantId.Value = String.Empty;
                CurrentViewContext.LstSvcGrpReviewStatus = new List<lkpBkgSvcGrpReviewStatusType>();
                CurrentViewContext.LstSvcGrpStatus = new List<lkpBkgSvcGrpStatusType>();
                CurrentViewContext.LstReviewCriterias = new List<BkgReviewCriteria>();
                cmbSGReviewStatusTypes.DataSource = new List<lkpBkgSvcGrpReviewStatusType>();
                cmbSGReviewStatusTypes.DataBind();
                cmbReviewCriteria.SelectedIndex = AppConsts.NONE;
                //cmbSGReviewStatusTypes.SelectedIndex = AppConsts.NONE;
                //cmbReviewCriteria.Items.Clear();
                //cmbReviewCriteria.CheckedItems.Clear();
                cmbSGStatusTypes.SelectedIndex = AppConsts.NONE;
            }
            else
            {
                cmbReviewCriteria.SelectedIndex = AppConsts.NONE;
                cmbSGReviewStatusTypes.SelectedIndex = AppConsts.NONE;
                cmbSGStatusTypes.SelectedIndex = AppConsts.NONE;
            }
            rbSubscriptionState.Visible = false;
            rbSubscriptionState.ClearSelection();
            //UAT-2304
            rblReviewType.SelectedValue = SvcGrpReviewType.AUTOMATIC_REVIEWED.GetStringValue();
            Session[AppConsts.BKG_REVIEW_QUEUE_OBJECT_SESSION_KEY] = null;

        }
        /// <summary>
        /// Reset institution hierarchy control.
        /// </summary>
        private void ResetInstitutionHierarchy()
        {
            lblinstituteHierarchy.Text = String.Empty;
            hdnDepartmntPrgrmMppng.Value = String.Empty;
            hdnHierarchyLabel.Value = String.Empty;
            hdnInstitutionNodeId.Value = String.Empty;
        }

        private void BindDropDownControl(Boolean isBackFromQueue = false)
        {
            if (CurrentViewContext.SelectedTenantId > 0)
            {
                Presenter.GetAllSvcGrpReviewStatuses();
                if (!isBackFromQueue)
                {
                    List<Int32?> lstDefaultSelectedReviewStatusType = new List<Int32?>();
                    lstDefaultSelectedReviewStatusType.Add(Presenter.GetSvcGroupReviewStatusTypeIdByCode(BkgSvcGrpReviewStatusType.FIRST_REVIEW.GetStringValue()));
                    lstDefaultSelectedReviewStatusType.Add(Presenter.GetSvcGroupReviewStatusTypeIdByCode(BkgSvcGrpReviewStatusType.SECOND_REVIEW.GetStringValue()));
                    //UAT-2304
                    lstDefaultSelectedReviewStatusType.Add(Presenter.GetSvcGroupReviewStatusTypeIdByCode(BkgSvcGrpReviewStatusType.MANUAL_REVIEW_COMPLETED.GetStringValue()));
                    CurrentViewContext.SvcGrpReviewStatusTypeIDs = lstDefaultSelectedReviewStatusType;
                }
                Presenter.GetAllSvcGrpStatuses();
                Presenter.GetAllReviewCriterias();
            }
        }

        /// <summary>
        /// Retain the filers on the search screen based on the page type
        /// </summary>
        /// <param name="args"></param>
        private void SetPageControl(Dictionary<String, String> args)
        {
            BkgOrderReviewQueueContract bkgOrderReviewQueueContract = new BkgOrderReviewQueueContract();

            if (Session[AppConsts.BKG_REVIEW_QUEUE_OBJECT_SESSION_KEY].IsNotNull())
            {
                #region Xml Deserialize

                StringReader reader = new StringReader(Convert.ToString(Session[AppConsts.BKG_REVIEW_QUEUE_OBJECT_SESSION_KEY]));
                XmlSerializer ser = new XmlSerializer(typeof(BkgOrderReviewQueueContract));
                XmlTextReader XmlReader = new XmlTextReader(reader);
                try
                {
                    bkgOrderReviewQueueContract = (BkgOrderReviewQueueContract)ser.Deserialize(XmlReader);
                }
                catch (SysXException ex)
                {
                    base.LogError(ex);
                    base.ShowErrorMessage(ex.Message);
                }
                finally
                {
                    XmlReader.Close();
                    reader.Close();
                }

                #endregion

                CurrentViewContext.SelectedTenantId = bkgOrderReviewQueueContract.ClientID;
                if (divTenant.Visible)
                    ddlTenantName.SelectedValue = CurrentViewContext.SelectedTenantId.ToString();

                BindDropDownControl(true);

                //Applicant First Name
                if (bkgOrderReviewQueueContract.ApplicantFirstName.IsNotNull())
                    CurrentViewContext.ApplicantFirstName = bkgOrderReviewQueueContract.ApplicantFirstName;

                //Applicant Last Name
                if (bkgOrderReviewQueueContract.ApplicantLastName.IsNotNull())
                    CurrentViewContext.ApplicantLastName = bkgOrderReviewQueueContract.ApplicantLastName;

                //Order ID
                if (bkgOrderReviewQueueContract.OrderNumber.IsNotNull())
                    CurrentViewContext.OrderNumber = bkgOrderReviewQueueContract.OrderNumber;

                //Review Criteria
                if (bkgOrderReviewQueueContract.SelectedReviewCriteriaId.IsNotNull())
                    CurrentViewContext.SelectedReviewCriteriaId = bkgOrderReviewQueueContract.SelectedReviewCriteriaId;

                //Service Group Review Status Type
                if (bkgOrderReviewQueueContract.SvcGrpReviewStatusTypeIDs.IsNotNull())
                    CurrentViewContext.SvcGrpReviewStatusTypeIDs = bkgOrderReviewQueueContract.SvcGrpReviewStatusTypeIDs;


                //Service Group Status Type
                if (bkgOrderReviewQueueContract.SvcGrpStatusTypeID.IsNotNull())
                    CurrentViewContext.SvcGrpStatusTypeID = bkgOrderReviewQueueContract.SvcGrpStatusTypeID;

                //Order Created dates
                if (bkgOrderReviewQueueContract.OrderFromDate.IsNotNull() && bkgOrderReviewQueueContract.OrderToDate.IsNotNull())
                {
                    CurrentViewContext.OrderFromDate = bkgOrderReviewQueueContract.OrderFromDate;
                    CurrentViewContext.OrderToDate = bkgOrderReviewQueueContract.OrderToDate;
                }

                //Service Group last updated date
                if (bkgOrderReviewQueueContract.SvcGrpUpdatedFromDate.IsNotNull() && bkgOrderReviewQueueContract.SvcGrpUpdatedToDate.IsNotNull())
                {
                    CurrentViewContext.SvcGrpUpdatedFromDate = bkgOrderReviewQueueContract.SvcGrpUpdatedFromDate;
                    CurrentViewContext.SvcGrpUpdatedToDate = bkgOrderReviewQueueContract.SvcGrpUpdatedToDate;
                }

                ////Hierarchy label
                //if (bkgOrderReviewQueueContract.DeptProgramMappingID > 0)
                //{
                //    hdnTenantId.Value = bkgOrderReviewQueueContract.ClientID.ToString();
                //    hdnHierarchyLabel.Value = bkgOrderReviewQueueContract.HierarchyLabel;
                //    hdnDepartmntPrgrmMppng.Value = bkgOrderReviewQueueContract.DeptProgramMappingID.ToString();
                //}

                //Hierarchy label
                if (!bkgOrderReviewQueueContract.DeptProgramMappingIDs.IsNullOrEmpty())
                {
                    hdnTenantId.Value = bkgOrderReviewQueueContract.ClientID.ToString();
                    hdnHierarchyLabel.Value = bkgOrderReviewQueueContract.HierarchyLabel;
                    hdnDepartmntPrgrmMppng.Value = bkgOrderReviewQueueContract.DeptProgramMappingIDs;
                }

                //UAT-1683
                Presenter.GetArchiveStateList();
                if (!bkgOrderReviewQueueContract.SelectedArchiveStateCode.IsNullOrEmpty())
                {
                    CurrentViewContext.SelectedArchiveStateCode = bkgOrderReviewQueueContract.SelectedArchiveStateCode.ToString();
                }

                //UAT-2304:
                if (!bkgOrderReviewQueueContract.SvcGrpReviewType.IsNullOrEmpty())
                {
                    CurrentViewContext.SelectedSvcGrpReviewType = bkgOrderReviewQueueContract.SvcGrpReviewType.ToString();
                }
                if (!bkgOrderReviewQueueContract.CurrentPageIndex.IsNullOrEmpty())
                {
                    grdBkgOrderReview.MasterTableView.CurrentPageIndex = bkgOrderReviewQueueContract.CurrentPageIndex;
                }
                //Custom paging arguments
                CurrentViewContext.PageSize = bkgOrderReviewQueueContract.GridCustomPagingArguments.PageSize;
                CurrentViewContext.CurrentPageIndex = bkgOrderReviewQueueContract.GridCustomPagingArguments.CurrentPageIndex;
                CurrentViewContext.VirtualRecordCount = bkgOrderReviewQueueContract.GridCustomPagingArguments.VirtualPageCount;

                //UAT-2066:"Continue" button click from the supplement review screen should return the user to the order review queue
                if (!bkgOrderReviewQueueContract.GridCustomPagingArguments.SortExpression.IsNullOrEmpty())
                {
                    CurrentViewContext.GridCustomPaging = bkgOrderReviewQueueContract.GridCustomPagingArguments;
                    GridSortExpression srtExpression = new GridSortExpression();
                    srtExpression.FieldName = bkgOrderReviewQueueContract.GridCustomPagingArguments.SortExpression;
                    srtExpression.SortOrder = bkgOrderReviewQueueContract.GridCustomPagingArguments.SortDirectionDescending ? GridSortOrder.Descending : GridSortOrder.Ascending;
                    grdBkgOrderReview.MasterTableView.SortExpressions.AddSortExpression(srtExpression);
                }

                if (!CurrentViewContext.SelectedTenantId.IsNullOrEmpty() && CurrentViewContext.SelectedTenantId > 0)
                {
                    CmdBarSearch.ClearButton.Style.Clear();
                }
            }
        }

        #region UAT-2117:"Continue" button behavior
        /// <summary>
        /// Method to show message after successfully placed/Update the order for supplement.
        /// </summary>
        /// <param name="args"></param>
        private void ShowMessageForSupplementOrder(Dictionary<String, String> args)
        {
            if (args.ContainsKey("MessageType"))
            {
                if (String.Compare(Convert.ToString(args["MessageType"]), "error", true) == 0)
                {
                    base.ShowErrorMessage(Convert.ToString(args["Message"]));
                }
                else if (String.Compare(Convert.ToString(args["MessageType"]), "info", true) == 0)
                {
                    base.ShowInfoMessage(Convert.ToString(args["Message"]));
                }
                else
                {
                    base.ShowSuccessMessage(Convert.ToString(args["Message"]));
                }
            }
        }
        #endregion

        #endregion

        #endregion

        #endregion

    }
}