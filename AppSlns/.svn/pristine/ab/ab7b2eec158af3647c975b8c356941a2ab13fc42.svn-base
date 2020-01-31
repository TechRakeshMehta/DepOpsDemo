using System;
using Microsoft.Practices.ObjectBuilder;
using System.Configuration;
using INTSOF.Utils;
using Telerik.Web.UI;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using INTSOF.UI.Contract.ComplianceManagement;
using CoreWeb.Shell;
using Entity.ClientEntity;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Collections;
using System.Xml.Serialization;
using System.Text;
using System.IO;
using CoreWeb.IntsofSecurityModel;
using Business.RepoManagers;
using System.Linq;
using INTERSOFT.WEB.UI.WebControls;
namespace CoreWeb.ComplianceOperations.Views
{
    public partial class UserItemsDataQueue : BaseUserControl, IUserItemsDataQueueView
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private UserItemsDataQueuePresenter _presenter = new UserItemsDataQueuePresenter();
        private String _viewType;
        private Int32 _tenantid;
        private Boolean _showClientDropdwn;
        private CustomPagingArgsContract _verificationGridCustomPaging = null;
        private CustomPagingArgsContract _exceptionGridCustomPaging = null;
        private ApplicantComplianceItemDataContract _verificationviewContract = null;
        private ApplicantComplianceItemDataContract _exceptionviewContract = null;

        #endregion

        #endregion

        #region Properties

        public UserItemsDataQueuePresenter Presenter
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


        public Boolean IsEscalationRecords
        {
            get { return Convert.ToBoolean(ViewState["IsEscalationRecords_User"]); }
            set { ViewState["IsEscalationRecords_User"] = value; }
        }

        public String QueueCode
        {
            get { return Convert.ToString(ViewState["QueueCode_User"]); }
            set { ViewState["QueueCode_User"] = value; }
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
                    //tenantId = Presenter.GetTenantId();
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
        /// returns the object of type IItemsDataVerificationQueueView.
        /// </summary>
        public IUserItemsDataQueueView CurrentViewContext
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
            get;
            set;
        }

        /// <summary>
        /// Populates the dropdown with the list of Compliance Package.
        /// </summary>
        public List<ComplaincePackageDetails> lstCompliancePackage
        {
            set
            {
                ddlPackage.DataSource = value;
                ddlPackage.DataBind();
            }
        }

        /// <summary>
        /// Populates the dropdown with the list of Compliance Category.
        /// </summary>
        public List<ComplianceCategory> lstComplianceCategory
        {
            set
            {
                ddlCategory.DataSource = value;
                ddlCategory.DataBind();
            }
        }

        public List<UserGroup> lstUserGroup
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the list of rows from ApplicantComplianceItemData table.
        /// </summary>
        public List<ApplicantComplianceItemDataContract> lstVerificationQueue
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


        /// <summary>
        /// Sets or gets the Selected Package Id from the select tenant dropdown.
        /// </summary>
        public Int32 SelectedPackageId
        {
            get
            {
                if (!ViewState["SelectedPackageId"].IsNull())
                {
                    return Convert.ToInt32(ViewState["SelectedPackageId"]);
                }

                return 0;
            }
            set
            {
                ViewState["SelectedPackageId"] = value;
            }
        }

        /// <summary>
        /// Sets or gets the Selected Category Id from the select tenant dropdown.
        /// </summary>
        public Int32 SelectedCategoryId
        {
            get
            {
                if (!ViewState["SelectedCategoryId"].IsNull())
                {
                    return Convert.ToInt32(ViewState["SelectedCategoryId"]);
                }

                return 0;
            }
            set
            {
                ViewState["SelectedCategoryId"] = value;
            }
        }

        /// <summary>
        /// Sets or gets the Selected User group Id from the select user group dropdown.
        /// </summary>
        public Int32 SelectedUserGroupId
        {
            get
            {
                if (!ViewState["SelectedUserGroupId"].IsNull())
                {
                    return Convert.ToInt32(ViewState["SelectedUserGroupId"]);
                }

                return 0;
            }
            set
            {
                ViewState["SelectedUserGroupId"] = value;
            }
        }

        /// <summary>
        /// Sets and gets option for showing Rush Orders in the Queue. 
        /// </summary>
        public Boolean ShowOnlyRushOrders
        {
            get
            {
                return chkShowRushOrders.Checked;
            }
        }

        #endregion

        #region Custom Paging

        /// <summary>
        /// Returns true if call is made for verification grid.
        /// </summary>
        public Boolean IsVerificationGrid
        {
            get;
            set;
        }

        /// <summary>
        /// CurrentPageIndex</summary>
        /// <value>
        /// Gets or sets the value for CurrentPageIndex.</value>
        public Int32 CurrentPageIndex
        {
            get
            {
                return grdVerificationItemData.MasterTableView.CurrentPageIndex + 1;
            }
            set
            {
                grdVerificationItemData.MasterTableView.CurrentPageIndex = value - 1;
            }
        }

        /// <summary>
        /// PageSize</summary>
        /// <value>
        /// Gets the value for PageSize.</value>
        public Int32 PageSize
        {
            get
            {    // Maximum 100 record allowed from DB. 
                //return grdVerificationItemData.PageSize > 100 ? 100 : grdVerificationItemData.PageSize;
                return grdVerificationItemData.PageSize;
            }
        }

        /// <summary>
        /// VirtualPageCount</summary>
        /// <value>
        /// Sets the value for VirtualPageCount.</value>
        public Int32 VirtualPageCount
        {
            set
            {
                grdVerificationItemData.VirtualItemCount = value;
                grdVerificationItemData.MasterTableView.VirtualItemCount = value;
            }
        }

        /// <summary>
        /// get object of shared class of custom paging
        /// </summary>
        public CustomPagingArgsContract VerificationGridCustomPaging
        {
            get
            {
                if (_verificationGridCustomPaging.IsNull())
                {
                    _verificationGridCustomPaging = new CustomPagingArgsContract();
                }
                return _verificationGridCustomPaging;
            }
        }

        /// <summary>
        /// get object of shared class of custom paging
        /// </summary>
        public CustomPagingArgsContract ExceptionGridCustomPaging
        {
            get
            {
                if (_exceptionGridCustomPaging.IsNull())
                {
                    _exceptionGridCustomPaging = new CustomPagingArgsContract();
                }
                return _exceptionGridCustomPaging;
            }
        }

        /// <summary>
        /// View Contract
        /// </summary>
        public ApplicantComplianceItemDataContract VerificationViewContract
        {
            get
            {
                if (_verificationviewContract.IsNull())
                {
                    _verificationviewContract = new ApplicantComplianceItemDataContract();
                }
                return _verificationviewContract;
            }
        }

        /// <summary>
        /// View Contract
        /// </summary>
        public ApplicantComplianceItemDataContract ExceptionViewContract
        {
            get
            {
                if (_exceptionviewContract.IsNull())
                {
                    _exceptionviewContract = new ApplicantComplianceItemDataContract();
                }
                return _exceptionviewContract;
            }
        }

        #endregion

        #region Custom Attributes

        // UAT 1055
        //public Int32? NodeId
        //{
        //    get
        //    {
        //        return ucCustomAttributeLoader.NodeId;
        //    }
        //    set
        //    {
        //        ucCustomAttributeLoader.NodeId = Convert.ToInt32(value);
        //    }
        //}

        //public Int32? DPM_Id
        //{
        //    get
        //    {
        //        return ucCustomAttributeLoader.DPM_ID;
        //    }
        //    set
        //    {
        //        ucCustomAttributeLoader.DPM_ID = Convert.ToInt32(value);
        //    }
        //}

        /// <summary>
        /// NodeID's of the Selected DPM Nodes - UAT 1055
        /// </summary>
        public String NodeIds
        {
            get
            {
                return ucCustomAttributeLoaderNodeSearch.NodeIds;
            }
            set
            {
                ucCustomAttributeLoaderNodeSearch.NodeIds = value;
            }
        }

        /// <summary>
        /// CSV - DPMID's of the Selected Nodes - UAT 1055
        /// </summary>
        public String DPMIds
        {
            get
            {
                return ucCustomAttributeLoaderNodeSearch.DPM_ID;
            }
            set
            {
                ucCustomAttributeLoaderNodeSearch.DPM_ID = Convert.ToString(value);
            }
        }

        public String CustomDataXML
        {
            get
            {
                // UAT 1055
                //return ucCustomAttributeLoader.GetCustomDataXML();
                return ucCustomAttributeLoaderNodeSearch.GetCustomDataXML();
            }
        }

        public String NodeLable
        {
            get
            {
                return ucCustomAttributeLoaderNodeSearch.nodeLable;
            }
            set
            {
                ucCustomAttributeLoaderNodeSearch.nodeLable = value;
            }
        }

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
                Dictionary<String, String> args = new Dictionary<String, String>();
                String title = "User Verification Queue";
                if (!Request.QueryString["args"].IsNull())
                {
                    args.ToDecryptedQueryString(Request.QueryString["args"]);
                    if (args.ContainsKey("QCODE"))
                    {
                        title = "User Verification Queue (Escalation)";
                    }
                }
                base.Title = title;
                base.SetPageTitle(title);
                lblVerificationQueue.Text = "Queue Lookup";
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
                if (!this.IsPostBack)
                {
                    Presenter.OnViewInitialized();
                    BindTenantDropDown();
                    QueueCode = Presenter.GetQueueCode();
                    if (ShowClientDropDown)
                    {
                        ddlTenantName.Enabled = true;
                        ddlPackage.Enabled = false;
                        ddlCategory.Enabled = false;
                        ddlUserGroup.Enabled = false;
                    }
                    else
                    {
                        ddlTenantName.SelectedValue = Convert.ToString(CurrentViewContext.TenantId);
                        SetDataForTenantSelected();
                        pnlVerification.Visible = true;
                    }
                    ddlCategory.Enabled = false;
                    //Show Approved/Rejected success message
                    Dictionary<String, String> args = new Dictionary<String, String>();

                    if (!Request.QueryString["args"].IsNull())
                    {
                        QueueCode = null;             //
                        args.ToDecryptedQueryString(Request.QueryString["args"]);
                        if (args.ContainsKey("UpdatedStatus"))
                        {
                            String updatedStatus = Convert.ToString(args["UpdatedStatus"]);

                            if (!string.IsNullOrWhiteSpace(updatedStatus))
                            {
                                ShowSuccessMessage("Verification Detail " + updatedStatus + " successfully.");
                            }
                        }
                        if (args.ContainsKey("QCODE"))
                        {
                            QueueCode = Convert.ToString(args["QCODE"]);
                            base.Title = "User Verification Queue (Escalation)";
                            base.SetPageTitle("User Verification Queue (Escalation)");
                            //queueCode = QueueMetaDataType.Verification_Queue_For_ClientAdmin.GetStringValue();
                            // 
                            //IsEscalationRecords = LookupManager.GetLookUpData<Entity.ClientEntity.QueueMetaData>(2).Where(x => x.QMD_IsDeleted == false && x.QMD_Code == QCode).FirstOrDefault().QMD_IsEscalationQueue;
                        }
                        GetSessionValues();
                    }
                    else
                    {
                        grdVerificationItemData.Visible = false;
                        lblPageHdr.Visible = false;
                    }
                }

                //UAT-839: Previous/Next student (Purple) buttons in Verification Details screen turning gray when items remain in queue.
                Session["CurentPackageSubscriptionID"] = null;
                Session["CurrentSubscriptionIDList"] = null;
                if (!ddlTenantName.SelectedValue.IsNullOrEmpty())
                {
                    CurrentViewContext.SelectedTenantId = Convert.ToInt32(ddlTenantName.SelectedValue);
                    ucCustomAttributeLoaderNodeSearch.TenantId = CurrentViewContext.SelectedTenantId;
                    ucCustomAttributeLoaderNodeSearch.ScreenType = "CommonScreen";
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

        #region Grid Related Events

        protected void grdVerificationItemData_Init(object sender, System.EventArgs e)
        {
            GridFilterMenu menu = grdVerificationItemData.FilterMenu;
            if (grdVerificationItemData.clearFilterMethod == null)
                grdVerificationItemData.clearFilterMethod = new WclGrid.ClearFilters(ClearViewStatesForFilter);
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
        protected void grdVerificationItemData_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                VerificationGridCustomPaging.CurrentPageIndex = CurrentPageIndex;
                VerificationGridCustomPaging.PageSize = PageSize;
                VerificationGridCustomPaging.FilterColumns = VerificationViewContract.FilterColumns;
                VerificationGridCustomPaging.FilterOperators = VerificationViewContract.FilterOperators;
                VerificationGridCustomPaging.FilterValues = VerificationViewContract.FilterValues;
                VerificationGridCustomPaging.FilterTypes = VerificationViewContract.FilterTypes;
                Session[AppConsts.VERIFICATION_QUEUE_SESSION_KEY] = VerificationGridCustomPaging;
                Presenter.GetVerificationQueueData();
                //To check if current use of this control is for verification by one of adb admin, client admin or third party 
                if (QueueCode == QueueMetaDataType.Verification_Queue_For_Admin.GetStringValue() ||
                    QueueCode == QueueMetaDataType.Verification_Queue_For_ClientAdmin.GetStringValue() ||
                    QueueCode == QueueMetaDataType.Verification_Queue_For_Third_Party.GetStringValue())
                {
                    GridColumn column = grdVerificationItemData.MasterTableView.Columns.FindByUniqueName("ReviewLevel");
                    column.Visible = true;
                }
                //if (IsEscalationRecords)
                //{
                //    GridColumn column = grdVerificationItemData.MasterTableView.Columns.FindByUniqueName("ReviewLevel");
                //    column.Visible = false;
                //}                  
                grdVerificationItemData.DataSource = lstVerificationQueue;
                SetFilterValues();
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

        protected void grdVerificationItemData_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                base.InsertOrEdit(grdVerificationItemData, e);
                #region For Filter command

                SetGridFilterValues();
                if (e.CommandName == RadGrid.FilterCommandName)
                {
                    /*UAT-2904*/
                    foreach (GridColumn item in grdVerificationItemData.MasterTableView.Columns)
                    {
                        string filterFunction = item.CurrentFilterFunction.ToString();
                        string filterValue = item.CurrentFilterValue;
                        if (filterValue.IsNullOrEmpty())
                        {
                            item.CurrentFilterFunction = Telerik.Web.UI.GridKnownFunction.NoFilter;
                        }
                    }
                    /*UAT-2904 ends here*/
                    Pair filter = (Pair)e.CommandArgument;

                    Int32 filterIndex = CurrentViewContext.VerificationViewContract.FilterColumns.IndexOf(filter.Second.ToString());
                    if (filter.First.ToString() != GridKnownFunction.NoFilter.ToString())
                    {
                        String filteringType = grdVerificationItemData.MasterTableView.Columns.FindByUniqueName(filter.Second.ToString()).DataTypeName;
                        String filterValue = grdVerificationItemData.MasterTableView.Columns.FindByUniqueName(filter.Second.ToString()).CurrentFilterValue;

                        if (filterIndex != -1)
                        {
                            CurrentViewContext.VerificationViewContract.FilterTypes[filterIndex] = filteringType;
                            CurrentViewContext.VerificationViewContract.FilterOperators[filterIndex] = filter.First.ToString();
                            CurrentViewContext.VerificationViewContract.FilterValues[filterIndex] = filterValue;
                        }
                        else
                        {
                            CurrentViewContext.VerificationViewContract.FilterTypes.Add(filteringType);
                            CurrentViewContext.VerificationViewContract.FilterColumns.Add(filter.Second.ToString());
                            CurrentViewContext.VerificationViewContract.FilterOperators.Add(filter.First.ToString());
                            CurrentViewContext.VerificationViewContract.FilterValues.Add(filterValue);
                        }
                    }
                    else if (filterIndex != -1)
                    {
                        CurrentViewContext.VerificationViewContract.FilterOperators.RemoveAt(filterIndex);
                        CurrentViewContext.VerificationViewContract.FilterValues.RemoveAt(filterIndex);
                        CurrentViewContext.VerificationViewContract.FilterColumns.RemoveAt(filterIndex);
                        CurrentViewContext.VerificationViewContract.FilterTypes.RemoveAt(filterIndex);
                    }

                    ViewState["FilterColumns"] = CurrentViewContext.VerificationViewContract.FilterColumns;
                    ViewState["FilterOperators"] = CurrentViewContext.VerificationViewContract.FilterOperators;
                    ViewState["FilterValues"] = CurrentViewContext.VerificationViewContract.FilterValues;
                    ViewState["FilterTypes"] = CurrentViewContext.VerificationViewContract.FilterTypes;
                }
                #endregion


                String workQueueType = String.Empty;

                if (IsEscalationRecords)
                {
                    workQueueType = WorkQueueType.EsclationUserWorkQueue.ToString();
                }
                else
                {
                    workQueueType = WorkQueueType.UserWorkQueue.ToString();
                }

                if (e.CommandName.Equals("ViewDetail"))
                {
                    Int32 _selectedCategoryId = Convert.ToInt32((e.Item.FindControl("hdfCatId") as HiddenField).Value);
                    Int32 _selectedPackageSubscriptionId = Convert.ToInt32((e.Item.FindControl("hdfPackSubscriptionId") as HiddenField).Value);

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
                    String itemDataId = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ApplicantComplianceItemID"].ToString();
                    String applicantId = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ApplicantId"].ToString();
                    queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "TenantId", Convert.ToString( selectedTenantId) },
                                                                    { "Child", ChildControls.VerificationDetailsNew},
                                                                    { "ItemDataId", itemDataId},
                                                                    {"WorkQueueType",workQueueType},
                                                                    {"PackageId",SelectedPackageId.ToString()},
                                                                    {"CategoryId",SelectedCategoryId.ToString()},
                                                                    {"IsException","false"},
                                                                    //{"ShowOnlyRushOrders",ShowOnlyRushOrders.ToString()},
                                                                    {"SelectedPackageSubscriptionId",Convert.ToString( _selectedPackageSubscriptionId)},
                                                                    {"SelectedComplianceCategoryId",Convert.ToString( _selectedCategoryId)},
                                                                    {"ShowOnlyRushOrders",Convert.ToString( chkShowRushOrders.Checked)},
                                                                    {"UserGroupId",SelectedUserGroupId.ToString()},
                                                                    {"ApplicantId",applicantId},
                                                                    {"IsEscalationRecords",Convert.ToString(IsEscalationRecords) }
                                                                 };
                    //HtmlAnchor ancItemDataDetail = (HtmlAnchor)e.Item.FindControl("ancItemDataDetail");
                    string url = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                    SetSessionValues();
                    Response.Redirect(url, true);
                }

                //Hide filter when exportig to pdf or word
                if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToPdfCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToWordCommandName
                    || e.CommandName == Telerik.Web.UI.RadGrid.ExportToExcelCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToCsvCommandName)
                {
                    base.ConfigureExport(grdVerificationItemData);

                }
                #region Export functionality
                // Implemented the export functionlaity for exporting custom attribute columns accordingly
                if (e.CommandName.IsNullOrEmpty())
                {
                    if (e.Item is GridCommandItem)
                    {
                        WclComboBox cmbExportFormat = e.Item.FindControl("cmbExportFormat") as WclComboBox;
                        if (cmbExportFormat.IsNotNull() && (cmbExportFormat.SelectedValue == "Csv" || cmbExportFormat.SelectedValue == "Pdf" || cmbExportFormat.SelectedValue == "Excel"))
                        {
                            grdVerificationItemData.MasterTableView.GetColumn("CustomAttributesTemp").Display = true;
                            grdVerificationItemData.MasterTableView.GetColumn("IsUiRulesViolate").Visible = true;
                        }
                        else
                        {
                            grdVerificationItemData.MasterTableView.GetColumn("CustomAttributesTemp").Display = false;
                        }
                    }
                }
                if (e.CommandName == "Cancel")
                {
                    grdVerificationItemData.MasterTableView.GetColumn("CustomAttributesTemp").Display = false;
                    grdVerificationItemData.MasterTableView.GetColumn("IsUiRulesViolate").Visible = true;
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
        protected void grdVerificationItemData_SortCommand(object sender, GridSortCommandEventArgs e)
        {
            try
            {
                if (e.NewSortOrder != GridSortOrder.None)
                {
                    ViewState["SortExpression"] = e.SortExpression;
                    ViewState["SortDirection"] = e.NewSortOrder.Equals(GridSortOrder.Descending);
                    CurrentViewContext.VerificationGridCustomPaging.SortExpression = e.SortExpression;
                    CurrentViewContext.VerificationGridCustomPaging.SortDirectionDescending = e.NewSortOrder.Equals(GridSortOrder.Descending);

                }
                else
                {
                    ViewState["SortExpression"] = String.Empty;
                    ViewState["SortDirection"] = false;
                    CurrentViewContext.VerificationGridCustomPaging.SortExpression = String.Empty;
                    CurrentViewContext.VerificationGridCustomPaging.SortDirectionDescending = false;
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

        protected void grdVerificationItemData_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
                {

                    //To Fixed UAT 410-Addition of "Custom Field" column to searches and verification queues
                    GridDataItem dataItem = (GridDataItem)e.Item;
                    if (Convert.ToString(dataItem["CustomAttributes"].Text).Length > 80)
                    {
                        dataItem["CustomAttributes"].ToolTip = dataItem["CustomAttributes"].Text;
                        dataItem["CustomAttributes"].Text = (dataItem["CustomAttributes"].Text).ToString().Substring(0, 80) + "...";
                    }

                    //UAT 2528
                    Boolean IsAdminLoggedIn = (Presenter.IsDefaultTenant);
                    if (IsAdminLoggedIn)
                    {
                        Boolean IsUiRulesViolate = Convert.ToBoolean(dataItem["IsUiRulesViolate"].Text);
                        if (IsUiRulesViolate)
                        {
                            dataItem.Attributes.Add("Style", "background-color:#ff6666 !important");
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

        #region DropDown Events

        /// <summary>
        /// Rebinds the data in the grdVerificationItemData for selected client. 
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that
        ///  contains the event data.</param>
        protected void ddlTenantName_ItemSelected(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {

            if (ddlTenantName.SelectedValue.IsNullOrEmpty())
            {
                ucCustomAttributeLoaderNodeSearch.Reset();
                SelectedTenantId = 0;
                ddlPackage.Enabled = false;
                ddlCategory.Enabled = false;
                ddlUserGroup.Enabled = false;
                pnlVerification.Visible = false;
            }
            else
            {
                SetDataForTenantSelected();
            }
            SelectedPackageId = 0;
            ddlPackage.SelectedIndex = 0;
            SelectedCategoryId = 0;
            ddlCategory.SelectedIndex = 0;
            SelectedUserGroupId = 0;
            ddlUserGroup.SelectedIndex = 0;
        }

        private void SetDataForTenantSelected()
        {
            SelectedTenantId = Convert.ToInt32(ddlTenantName.SelectedValue);
            Presenter.GetCompliancePackage();
            BindUserGroups();
            ddlPackage.Enabled = true;
            ddlUserGroup.Enabled = true;
            ddlCategory.Enabled = false;
            ucCustomAttributeLoaderNodeSearch.TenantId = CurrentViewContext.SelectedTenantId;
            ucCustomAttributeLoaderNodeSearch.ScreenType = "CommonScreen";
        }

        protected void ddlPackage_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {

            if (ddlPackage.SelectedValue.IsNullOrEmpty())
            {
                SelectedPackageId = 0;
                ddlCategory.Enabled = false;
            }
            else
            {
                SelectedPackageId = Convert.ToInt32(ddlPackage.SelectedValue);
                ddlCategory.Enabled = true;
            }
            Presenter.GetComplianceCategory();
            SelectedCategoryId = 0;
            ddlCategory.SelectedIndex = 0;
        }


        protected void ddlCategory_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (ddlCategory.SelectedValue.IsNullOrEmpty())
            {
                SelectedCategoryId = 0;
            }
            else
            {
                SelectedCategoryId = Convert.ToInt32(ddlCategory.SelectedValue);
            }
        }


        protected void ddlUserGroup_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (ddlUserGroup.SelectedValue.IsNullOrEmpty())
            {
                SelectedUserGroupId = 0;
            }
            else
            {
                SelectedUserGroupId = Convert.ToInt32(ddlUserGroup.SelectedValue);
            }
        }

        protected void ddlTenantName_DataBound(object sender, EventArgs e)
        {
            ddlTenantName.Items.Insert(0, new RadComboBoxItem("--Select--"));
        }

        protected void ddlPackage_DataBound(object sender, EventArgs e)
        {
            ddlPackage.Items.Insert(0, new RadComboBoxItem("--Select--"));
        }

        protected void ddlCategory_DataBound(object sender, EventArgs e)
        {
            ddlCategory.Items.Insert(0, new RadComboBoxItem("--Select--"));
        }

        /// <summary>
        /// User group DataBound event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlUserGroup_DataBound(object sender, EventArgs e)
        {
            ddlUserGroup.Items.Insert(0, new RadComboBoxItem("--Select--"));
        }
        #endregion

        protected void chkShowRushOrders_CheckedChanged(object sender, EventArgs e)
        {
            grdVerificationItemData.Rebind();
        }

        #region Button Events

        protected void CmdBarSearch_Click(object sender, EventArgs e)
        {
            grdVerificationItemData.Visible = true;
            lblPageHdr.Visible = true;
            //Presenter.PerformSearch();
            //grdItemData.DataSource = CurrentViewContext.ItemData;
            //grdItemData.DataBind();
            if (!ddlTenantName.SelectedValue.IsNullOrEmpty())
            {
                // ClearViewStatesForFilter();
                SetGridFilterValues();
                grdVerificationItemData.Rebind();
                pnlVerification.Visible = true;
            }
        }

        protected void CmdBarReset_Click(object sender, EventArgs e)
        {
            Presenter.OnViewInitialized();
            // BindTenantDropDown();   UAT-789
            CurrentViewContext.SelectedUserGroupId = 0;
            CurrentViewContext.SelectedPackageId = 0;
            CurrentViewContext.SelectedCategoryId = 0;
            if (Presenter.IsDefaultTenant)
            {
                ucCustomAttributeLoaderNodeSearch.Reset();
            }
            else
            {
                ucCustomAttributeLoaderNodeSearch.ResetControlData(true);
            }
            VirtualPageCount = 0;
            ViewState["SortExpression"] = null;
            ViewState["SortDirection"] = null;
            grdVerificationItemData.MasterTableView.SortExpressions.Clear();
            grdVerificationItemData.MasterTableView.FilterExpression = String.Empty; //UAT-2904
            ddlCategory.SelectedIndex = 0;
            ddlPackage.SelectedIndex = 0;
            ddlUserGroup.SelectedIndex = 0;
            chkShowRushOrders.Checked = false;
            ClearViewStatesForFilter();
            SetGridFilterValues();

            if (ShowClientDropDown)
            {
                ddlTenantName.SelectedIndex = 0;
                CurrentViewContext.SelectedTenantId = 0;
                SelectedTenantId = 0;
                ddlPackage.Enabled = false;
                ddlCategory.Enabled = false;
                ddlUserGroup.Enabled = false;
                pnlVerification.Visible = false;
            }

            /*UAT-2904*/
            foreach (GridColumn column in grdVerificationItemData.MasterTableView.Columns)
            {
                column.CurrentFilterFunction = GridKnownFunction.NoFilter;
                column.CurrentFilterValue = string.Empty;
            }
            /*End UAT-2904*/
        }

        protected void CmdBarCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(String.Format(AppConsts.DASHBOARD_PAGE_NAME));
        }


        #endregion

        #endregion

        #region Methods

        #region Private Methods

        private void SetGridFilterValues()
        {
            if (!ViewState["SortExpression"].IsNull())
            {
                CurrentViewContext.VerificationGridCustomPaging.SortExpression = Convert.ToString(ViewState["SortExpression"]);
                CurrentViewContext.VerificationGridCustomPaging.SortDirectionDescending = Convert.ToBoolean(ViewState["SortDirection"]);
            }

            CurrentViewContext.VerificationViewContract.FilterColumns = ViewState["FilterColumns"] == null ? new List<String>() : (List<String>)(ViewState["FilterColumns"]);
            CurrentViewContext.VerificationViewContract.FilterOperators = ViewState["FilterOperators"] == null ? new List<String>() : (List<String>)(ViewState["FilterOperators"]);
            CurrentViewContext.VerificationViewContract.FilterValues = ViewState["FilterValues"] == null ? new ArrayList() : (ArrayList)(ViewState["FilterValues"]);
            CurrentViewContext.VerificationViewContract.FilterTypes = ViewState["FilterTypes"] == null ? new List<String>() : (List<String>)(ViewState["FilterTypes"]);
        }

        /// <summary>
        /// Binds the list of active tenants in the select client dropdown.
        /// </summary>
        private void BindTenantDropDown()
        {
            ddlTenantName.DataSource = lstTenant;
            ddlTenantName.DataBind();
        }

        /// <summary>
        /// To bind User group dropdown
        /// </summary>
        private void BindUserGroups()
        {
            Presenter.GetAllUserGroups();
            ddlUserGroup.DataSource = CurrentViewContext.lstUserGroup;
            ddlUserGroup.DataBind();
        }

        private void SetPageControls(Dictionary<String, String> args)
        {
            if (args.ContainsKey("SelectedTenantId"))
            {
                SelectedTenantId = Convert.ToInt32(args["SelectedTenantId"]);
                ddlTenantName.SelectedValue = SelectedTenantId.ToString();
                Presenter.GetCompliancePackage();
                BindUserGroups();
                ddlPackage.Enabled = true;
                ddlUserGroup.Enabled = true;
                ddlCategory.Enabled = false;
                pnlVerification.Visible = true;

                if (args.ContainsKey("PackageId"))
                {
                    SelectedPackageId = Convert.ToInt32(args["PackageId"]);
                    ddlPackage.SelectedValue = SelectedPackageId.ToString();
                    ddlCategory.Enabled = true;
                    Presenter.GetComplianceCategory();

                    if (args.ContainsKey("CategoryId"))
                    {
                        SelectedCategoryId = Convert.ToInt32(args["CategoryId"]);
                        ddlCategory.SelectedValue = SelectedCategoryId.ToString();
                    }
                }
                if (args.ContainsKey("ShowOnlyRushOrders"))
                {
                    chkShowRushOrders.Checked = Convert.ToBoolean(args["ShowOnlyRushOrders"]);
                }
                if (args.ContainsKey("UserGroupId"))
                {
                    SelectedUserGroupId = Convert.ToInt32(args["UserGroupId"]);
                    ddlUserGroup.SelectedValue = SelectedUserGroupId.ToString();
                }
                grdVerificationItemData.Rebind();
            }
        }

        private void SetSessionValues()
        {
            VerificationQueueFiltersContract verificationQueueFilters = new VerificationQueueFiltersContract();
            if ((Presenter.IsDefaultTenant || Presenter.IsThirdPartyTenant) && SelectedTenantId > 0)
            {
                verificationQueueFilters.TenantId = SelectedTenantId;
            }
            else if (TenantId > 0)
            {
                verificationQueueFilters.TenantId = TenantId;
            }
            if (SelectedPackageId > 0)
            {
                verificationQueueFilters.PackageID = SelectedPackageId;
            }
            if (SelectedCategoryId > 0)
            {
                verificationQueueFilters.CategoryId = SelectedCategoryId;
            }
            if (SelectedUserGroupId > 0)
            {
                verificationQueueFilters.UserGroupId = SelectedUserGroupId;
            }

            #region Custom Attributes

            #region UAT 1055 Changes

            //if (NodeId > 0)
            //{
            //    verificationQueueFilters.NodeId = NodeId;
            //}
            //if (DPM_Id > 0)
            //{
            //    verificationQueueFilters.DPM_Id = DPM_Id;
            //}

            if (!this.NodeIds.IsNullOrEmpty())
            {
                verificationQueueFilters.NodeIds = this.NodeIds;
            }
            if (!this.DPMIds.IsNullOrEmpty())
            {
                verificationQueueFilters.SelectedDPMIds = this.DPMIds;
            }

            #endregion

            if (!NodeLable.IsNullOrEmpty())
            {
                verificationQueueFilters.NodeLabel = NodeLable;
            }
            if (!CustomDataXML.IsNullOrEmpty())
            {
                verificationQueueFilters.CustomFields = CustomDataXML;
            }

            #endregion
            verificationQueueFilters.ShowRushOrders = ShowOnlyRushOrders;
            verificationQueueFilters.FilterColumns = ViewState["FilterColumns"].IsNullOrEmpty() ? null : (List<String>)ViewState["FilterColumns"];
            verificationQueueFilters.FilterOperators = ViewState["FilterOperators"].IsNullOrEmpty() ? null : (List<String>)ViewState["FilterOperators"];
            verificationQueueFilters.FilterValues = ViewState["FilterValues"].IsNullOrEmpty() ? null : (ArrayList)ViewState["FilterValues"];
            verificationQueueFilters.FilterTypes = ViewState["FilterTypes"].IsNullOrEmpty() ? null : (List<String>)ViewState["FilterTypes"];
            SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.SESSION_KEY_VERIFICATIONQUEUE, verificationQueueFilters);
        }

        private void GetSessionValues()
        {
            VerificationQueueFiltersContract verificationQueueFilters = (VerificationQueueFiltersContract)SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_KEY_VERIFICATIONQUEUE);
            if (verificationQueueFilters.IsNotNull())
            {
                if (verificationQueueFilters.TenantId > 0)
                {
                    if (Presenter.IsDefaultTenant || Presenter.IsThirdPartyTenant)
                    {
                        SelectedTenantId = verificationQueueFilters.TenantId;
                        ddlTenantName.SelectedValue = SelectedTenantId.ToString();
                    }
                    Presenter.GetCompliancePackage();
                    BindUserGroups();
                    ddlPackage.Enabled = true;
                    ddlUserGroup.Enabled = true;
                    ddlCategory.Enabled = false;
                    pnlVerification.Visible = true;

                    if (verificationQueueFilters.PackageID > 0)
                    {
                        SelectedPackageId = verificationQueueFilters.PackageID;
                        ddlPackage.SelectedValue = SelectedPackageId.ToString();
                        ddlCategory.Enabled = true;
                        Presenter.GetComplianceCategory();

                        if (verificationQueueFilters.CategoryId > 0)
                        {
                            SelectedCategoryId = verificationQueueFilters.CategoryId;
                            ddlCategory.SelectedValue = SelectedCategoryId.ToString();
                        }
                    }
                    if (verificationQueueFilters.UserGroupId > 0)
                    {
                        SelectedUserGroupId = verificationQueueFilters.UserGroupId;
                        ddlUserGroup.SelectedValue = SelectedUserGroupId.ToString();
                    }
                    chkShowRushOrders.Checked = verificationQueueFilters.ShowRushOrders;

                    #region Custom Attributes
                    //ucCustomAttributeLoaderNodeSearch.NodeId = verificationQueueFilters.NodeId.IsNotNull() ? Convert.ToInt32(verificationQueueFilters.NodeId) : 0;
                    ucCustomAttributeLoaderNodeSearch.NodeIds = !verificationQueueFilters.NodeIds.IsNullOrEmpty() ? verificationQueueFilters.NodeIds : String.Empty;
                    ucCustomAttributeLoaderNodeSearch.TenantId = CurrentViewContext.SelectedTenantId = verificationQueueFilters.TenantId;
                    ucCustomAttributeLoaderNodeSearch.previousValues = verificationQueueFilters.CustomFields;
                    ucCustomAttributeLoaderNodeSearch.nodeLable = verificationQueueFilters.NodeLabel;
                    ucCustomAttributeLoaderNodeSearch.ScreenType = "CommonScreen";
                    ucCustomAttributeLoaderNodeSearch.DPM_ID = verificationQueueFilters.SelectedDPMIds;
                    #endregion

                    ViewState["FilterColumns"] = CurrentViewContext.VerificationViewContract.FilterColumns = verificationQueueFilters.FilterColumns;
                    ViewState["FilterOperators"] = CurrentViewContext.VerificationViewContract.FilterOperators = verificationQueueFilters.FilterOperators;
                    ViewState["FilterValues"] = CurrentViewContext.VerificationViewContract.FilterValues = verificationQueueFilters.FilterValues;
                    ViewState["FilterTypes"] = CurrentViewContext.VerificationViewContract.FilterTypes = verificationQueueFilters.FilterTypes;
                    SetFilterValues();
                    // grdVerificationItemData.Rebind();
                }
                SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.SESSION_KEY_VERIFICATIONQUEUE, null);
            }
        }

        public void SetFilterValues()
        {
            if (!CurrentViewContext.VerificationViewContract.FilterColumns.IsNullOrEmpty() && CurrentViewContext.VerificationViewContract.FilterColumns.Count > 0)
            {
                CurrentViewContext.VerificationViewContract.FilterColumns.ForEach(x =>
                    grdVerificationItemData.Columns.FindByUniqueName(x).CurrentFilterValue = CurrentViewContext.VerificationViewContract.FilterValues[CurrentViewContext.VerificationViewContract.FilterColumns.IndexOf(x)].ToString()
                    );
            }
        }

        public void ClearViewStatesForFilter()
        {
            ViewState["FilterColumns"] = null;
            ViewState["FilterOperators"] = null;
            ViewState["FilterValues"] = null;
            ViewState["FilterTypes"] = null;
        }


        #endregion

        #region Public Methods

        #endregion

        #endregion

    }
}

