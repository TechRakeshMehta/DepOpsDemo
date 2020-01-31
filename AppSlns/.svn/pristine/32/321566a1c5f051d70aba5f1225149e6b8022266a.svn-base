#region Namespace

#region System Defined
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Collections;
using System.Web.UI;
using System.Xml.Serialization;
using System.Text;
using System.IO;
using System.Threading;
using System.Linq;
#endregion

#region Project Specific
using Microsoft.Practices.ObjectBuilder;
using INTSOF.Utils;
using Telerik.Web.UI;
using INTSOF.UI.Contract.ComplianceManagement;
using CoreWeb.Shell;
using Entity.ClientEntity;
using CoreWeb.IntsofSecurityModel;
using WebSiteUtils.SharedObjects;
using Business.RepoManagers;
using INTERSOFT.WEB.UI.WebControls;

#endregion

#endregion

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class ExceptionVerificationQueue : BaseUserControl, IExceptionVerificationQueueView
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private ExceptionVerificationQueuePresenter _presenter = new ExceptionVerificationQueuePresenter();
        private String _viewType;
        private Int32 _tenantid;
        private Boolean _showClientDropdwn;
        private Int32 _expSelectedUserId;
        private CustomPagingArgsContract _exceptionGridCustomPaging = null;
        private ApplicantComplianceItemDataContract _exceptionviewContract = null;

        #endregion

        #endregion

        #region Properties

        public ExceptionVerificationQueuePresenter Presenter
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


        public List<AssignQueueRecords> SelectedVerificationItemsNew
        {
            get
            {
                if (!ViewState["SelectedVerificationItemsNew"].IsNull())
                {
                    return ViewState["SelectedVerificationItemsNew"] as List<AssignQueueRecords>;
                }

                return new List<AssignQueueRecords>();
            }
            set
            {
                ViewState["SelectedVerificationItemsNew"] = value;
            }
        }

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
        /// Sets or gets the Selected User Id.
        /// </summary>
        public Int32 ExpSelectedUserId
        {
            get
            {
                return _expSelectedUserId;
            }
            set
            {
                _expSelectedUserId = value;
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
        /// returns the object of type IItemsDataVerificationQueueView.
        /// </summary>
        public IExceptionVerificationQueueView CurrentViewContext
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
        /// Populates the dropdown with the list of active users in the organisation.
        /// </summary>
        public List<Entity.OrganizationUser> lstOrganizationUser
        {
            set
            {
                ddlExpSelectedUser.DataSource = value;
                ddlExpSelectedUser.DataBind();
            }
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


        public Boolean IsEscalationRecords
        {
            get { return Convert.ToBoolean(ViewState["IsEscalationRecords"]); }
            set { ViewState["IsEscalationRecords"] = value; }
        }

        public String QueueCode
        {
            get { return Convert.ToString(ViewState["QueueCode_Exception"]); }
            set { ViewState["QueueCode_Exception"] = value; }
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
        /// Gets or Sets the value for selected Items.
        /// </summary>
        public Dictionary<Int32, Boolean> SelectedExceptionItems
        {
            get
            {
                if (!ViewState["SelectedExceptionItems"].IsNull())
                {
                    return ViewState["SelectedExceptionItems"] as Dictionary<Int32, Boolean>;
                }

                return new Dictionary<Int32, Boolean>();
            }
            set
            {
                ViewState["SelectedExceptionItems"] = value;
            }
        }

        /// <summary>
        /// Gets or Sets the value for selected Items.
        /// </summary>
        public List<KeyValuePair<Int32, Int32>> SelectedExceptionNodes
        {
            get
            {
                if (!ViewState["SelectedExceptionNodse"].IsNull())
                {
                    return ViewState["SelectedExceptionNodes"] as List<KeyValuePair<Int32, Int32>>;
                }

                return new List<KeyValuePair<Int32, Int32>>();
            }
            set
            {
                ViewState["SelectedExceptionNodes"] = value;
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

        public WorkQueueType SelectedWorkQueueType
        {
            get
            {
                if (!ViewState["WorkQueueType"].IsNull())
                {
                    return (WorkQueueType)ViewState["WorkQueueType"];
                }

                return 0;
            }
            set
            {
                ViewState["WorkQueueType"] = value;
            }
        }

        #region Exception Queue Properties

        public List<ApplicantComplianceItemDataContract> lstExceptionQueue
        {
            get;
            set;
        }

        #endregion

        #region Custom Paging
        /// <summary>
        /// CurrentPageIndex</summary>
        /// <value>
        /// Gets or sets the value for CurrentPageIndex.</value>
        public Int32 CurrentPageIndex
        {
            get
            {
                return grdExceptionItemData.MasterTableView.CurrentPageIndex + 1;
            }
            set
            {
                grdExceptionItemData.MasterTableView.CurrentPageIndex = value - 1;
            }
        }

        /// <summary>
        /// PageSize</summary>
        /// <value>
        /// Gets the value for PageSize.</value>
        public Int32 PageSize
        {
            get
            {
                // Maximum 100 record allowed from DB. 
                //return grdExceptionItemData.PageSize > 100 ? 100 : grdExceptionItemData.PageSize;
                return grdExceptionItemData.PageSize;
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
                return grdExceptionItemData.MasterTableView.VirtualItemCount;
            }
            set
            {
                grdExceptionItemData.VirtualItemCount = value;
                grdExceptionItemData.MasterTableView.VirtualItemCount = value;
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

        public List<UserGroup> lstUserGroup
        {
            get;
            set;
        }
        #region UAT-4067
        public List<Int32> selectedNodeIDs
        {
            get
            {
                if (!ViewState["selectedNodeIDs"].IsNull())
                {
                    return (ViewState["selectedNodeIDs"]) as List<Int32>;
                }
                return new List<Int32>();
            }
            set { ViewState["selectedNodeIDs"] = value; }
        }

        public List<String> allowedFileExtensions
        {
            get
            {
                if (!ViewState["allowedFileExtensions"].IsNull())
                {
                    return (ViewState["allowedFileExtensions"]) as List<String>;
                }
                return new List<String>();
            }
            set { ViewState["allowedFileExtensions"] = value; }
        }

        #endregion-4067
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
        /// DPMID's of the Selected Nodes - UAT 1055
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
                if (!this.IsPostBack)
                {
                    Presenter.OnViewInitialized();
                    BindTenantDropDown();
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
                        pnlException.Visible = true;
                        if (SelectedWorkQueueType == WorkQueueType.ExceptionAssignmentWorkQueue)
                            chkSelectAllResults.Visible = true;
                    }
                    ddlCategory.Enabled = false;
                    //Show Approved/Rejected success message
                    Dictionary<String, String> args = new Dictionary<String, String>();
                    if (!Request.QueryString["args"].IsNull())
                    {
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
                        }
                        //SetPageControls(args);
                        GetSessionValues();
                    }
                    else
                    {
                        grdExceptionItemData.Visible = false;
                        lblExceptionHdr.Visible = false;
                        chkSelectAllResults.Visible = false;
                    }
                }
                lblExceptionQueue.Text = "Queue Lookup";
                //this._presenter.OnViewLoaded();
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
        protected void grdExceptionItemData_Init(object sender, System.EventArgs e)
        {
            GridFilterMenu menu = grdExceptionItemData.FilterMenu;

            if (grdExceptionItemData.clearFilterMethod == null)
                grdExceptionItemData.clearFilterMethod = new WclGrid.ClearFilters(ClearViewStatesForFilter);

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
        /// Retrieves a list of all tenant with its address and contact details.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        /// <remarks></remarks>
        protected void grdExceptionItemData_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                ExceptionGridCustomPaging.CurrentPageIndex = CurrentPageIndex;
                ExceptionGridCustomPaging.PageSize = PageSize;
                ExceptionGridCustomPaging.FilterColumns = ExceptionViewContract.FilterColumns;
                ExceptionGridCustomPaging.FilterOperators = ExceptionViewContract.FilterOperators;
                ExceptionGridCustomPaging.FilterValues = ExceptionViewContract.FilterValues;
                ExceptionGridCustomPaging.FilterTypes = ExceptionViewContract.FilterTypes;
                Session[AppConsts.EXCEPTION_QUEUE_SESSION_KEY] = ExceptionGridCustomPaging;
                Presenter.GetExceptionQueueData();
                grdExceptionItemData.DataSource = lstExceptionQueue;
                if (SelectedWorkQueueType == WorkQueueType.ExceptionUserWorkQueue)
                {
                    grdExceptionItemData.MasterTableView.GetColumn("AssignedUserName").Visible = false;
                    grdExceptionItemData.MasterTableView.GetColumn("AssignItems").Visible = false;
                    //grdExceptionItemData.MasterTableView.GetColumn("ReviewLevel").Visible = false;  //for UAT-545
                }

                if (IsEscalationRecords)
                {
                    grdExceptionItemData.MasterTableView.GetColumn("ReviewLevel").Visible = false;
                }

                if (lstExceptionQueue.IsNotNull() && lstExceptionQueue.Count > 0)
                {
                    if (SelectedWorkQueueType == WorkQueueType.ExceptionAssignmentWorkQueue)
                    {
                        pnlExpShowUsers.Visible = true;
                        Presenter.GetUserListForSelectedTenant();
                    }
                    else if (SelectedWorkQueueType == WorkQueueType.ExceptionUserWorkQueue)
                    {
                        pnlExpShowUsers.Visible = false;
                    }
                }
                else if (lstExceptionQueue.IsNull())
                {
                    pnlException.Visible = false;
                    chkSelectAllResults.Visible = false;
                }
                else
                {
                    pnlExpShowUsers.Visible = false;
                }
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

        /// <summary>
        /// Event handler. Called by grdVerificationItemData for item data bound events.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">     An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that
        ///  contains the event data.</param>
        protected void grdExceptionItemData_ItemDataBound(object sender, GridItemEventArgs e)
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
                    String itemDataId = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ApplicantComplianceItemID"].ToString();
                    // Dictionary<Int32, Boolean> selectedItems = CurrentViewContext.SelectedExceptionItems;
                    List<AssignQueueRecords> selectedItems = SelectedVerificationItemsNew;
                    if (selectedItems.Any(x => x.ApplicantComplienceItemId == (Convert.ToInt32(itemDataId)) && x.IsChecked))
                    {
                        CheckBox checkBox = ((CheckBox)e.Item.FindControl("chkSelectedException"));
                        checkBox.Checked = true;
                    }
                }

                if (e.Item.ItemType.Equals(GridItemType.Footer))
                {
                    Int32 rowCount = grdExceptionItemData.Items.Count;
                    if (rowCount > 0)
                    {
                        Int32 checkCount = 0;
                        foreach (GridDataItem item in grdExceptionItemData.Items)
                        {
                            CheckBox checkBox = ((CheckBox)item.FindControl("chkSelectedException"));
                            if (checkBox.Checked)
                            {
                                checkCount++;
                            }
                        }
                        if (rowCount == checkCount)
                        {
                            GridHeaderItem item = grdExceptionItemData.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
                            CheckBox checkBox = ((CheckBox)item.FindControl("chkSelectAllExceptions"));
                            checkBox.Checked = true;

                            GridFooterItem itemFooter = grdExceptionItemData.MasterTableView.GetItems(GridItemType.Footer)[0] as GridFooterItem;
                            CheckBox checkBoxFooter = ((CheckBox)itemFooter.FindControl("chkSelectAllExceptionsFooter"));
                            checkBoxFooter.Checked = true;
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

        protected void grdExceptionItemData_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                #region DetailScreenNavigation
                String workQueueType = String.Empty;
                if (e.CommandName.Equals("ViewDetail"))
                {
                    //UAT-839: Previous/Next student (Purple) buttons in Verification Details screen turning gray when items remain in queue.
                    Session["CurentPackageSubscriptionID"] = null;
                    Session["CurrentSubscriptionIDList"] = null;

                    Int32 selectedTenantId = 0;

                    Int32 _selectedCategoryId = Convert.ToInt32((e.Item.FindControl("hdfCatId") as HiddenField).Value);
                    Int32 _selectedPackageSubscriptionId = Convert.ToInt32((e.Item.FindControl("hdfPackSubscriptionId") as HiddenField).Value);
                    if (!ddlTenantName.SelectedValue.IsNullOrEmpty())
                    {
                        selectedTenantId = SelectedTenantId;
                    }
                    else
                    {
                        selectedTenantId = TenantId;
                    }

                    if (ddlExpSelectedUser.SelectedValue.IsNullOrEmpty())
                    {
                        ExpSelectedUserId = 0;
                    }
                    else
                    {
                        ExpSelectedUserId = Convert.ToInt32(ddlExpSelectedUser.SelectedValue);
                    }
                    SetSessionValues();
                    Dictionary<String, String> queryString = new Dictionary<String, String>();
                    String itemDataId = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ApplicantComplianceItemID"].ToString();
                    String applicantId = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ApplicantId"].ToString();
                    if (SelectedWorkQueueType == WorkQueueType.ExceptionAssignmentWorkQueue)
                    {
                        if (IsEscalationRecords)
                        {
                            workQueueType = WorkQueueType.EsclationAssignmentWorkQueue.ToString();
                        }
                        else
                        {
                            workQueueType = WorkQueueType.AssignmentWorkQueue.ToString();
                        }
                        Presenter.GetSelectedNodeIDBySubscriptionID(selectedTenantId.ToString(), _selectedPackageSubscriptionId.ToString());
                        Presenter.GetAllowedFileExtensions(selectedTenantId.ToString());
                        queryString = new Dictionary<String, String>
                                                                 { 
                                                                    //{ "SelectedTenantId", Convert.ToString( selectedTenantId) },
                                                                    //{ "Child", ChildControls.ExceptionDetails},
                                                                    { "TenantId", Convert.ToString( selectedTenantId) },
                                                                    { "Child", ChildControls.VerificationDetailsNew},
                                                                    { "ItemDataId", itemDataId},
                                                                    {"IsException","true"},
                                                                    {"WorkQueueType",workQueueType},
                                                                    {"PackageId",SelectedPackageId.ToString()},
                                                                    {"CategoryId",SelectedCategoryId.ToString()}, 
                                                                    {"AssignedToExpUser",ExpSelectedUserId.ToString()},
                                                                    {"SelectedComplianceCategoryId",Convert.ToString( _selectedCategoryId)},
                                                                    {"SelectedPackageSubscriptionId",Convert.ToString( _selectedPackageSubscriptionId)},
                                                                    {"ShowOnlyRushOrders",Convert.ToString( chkShowRushOrders.Checked)},
                                                                    {"UserGroupId",SelectedUserGroupId.ToString()},
                                                                    {"ApplicantId",applicantId},
                                                                    {"IsEscalationRecords",Convert.ToString(IsEscalationRecords) },
                                                                    {"allowedFileExtensions", String.Join(",",CurrentViewContext.allowedFileExtensions)}, //UAT-4067	
                                                                 };
                    }
                    else if (SelectedWorkQueueType == WorkQueueType.ExceptionUserWorkQueue)
                    {

                        if (IsEscalationRecords)
                        {
                            workQueueType = WorkQueueType.EsclationUserWorkQueue.ToString();
                        }
                        else
                        {
                            workQueueType = WorkQueueType.UserWorkQueue.ToString();
                        }

                        queryString = new Dictionary<String, String>
                                                                 { 
                                                                    //{ "SelectedTenantId", Convert.ToString( selectedTenantId) },
                                                                    //{ "Child", ChildControls.ExceptionDetails},
                                                                    { "TenantId", Convert.ToString( selectedTenantId) },
                                                                    { "Child", ChildControls.VerificationDetailsNew},
                                                                    { "ItemDataId", itemDataId},
                                                                    {"WorkQueueType",workQueueType},
                                                                    {"PackageId",SelectedPackageId.ToString()},
                                                                    {"IsException","true"},
                                                                    {"CategoryId",SelectedCategoryId.ToString()},
                                                                    {"SelectedPackageSubscriptionId",Convert.ToString( _selectedPackageSubscriptionId)},
                                                                    {"SelectedComplianceCategoryId",Convert.ToString( _selectedCategoryId)},
                                                                    {"ShowOnlyRushOrders",Convert.ToString( chkShowRushOrders.Checked)},
                                                                    {"UserGroupId",SelectedUserGroupId.ToString()},
                                                                    {"ApplicantId",applicantId},
                                                                    {"IsEscalationRecords",Convert.ToString(IsEscalationRecords) }
                                                                 };
                    }
                    string url = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                    Response.Redirect(url, true);
                }
                #endregion

                base.InsertOrEdit(grdExceptionItemData, e);

                #region For Filter command

                SetExpGridFilters();

                if (e.CommandName == RadGrid.FilterCommandName)
                {
                    /*UAT-2904*/
                    foreach (GridColumn item in grdExceptionItemData.MasterTableView.Columns)
                    {
                        string filterFunction = item.CurrentFilterFunction.ToString();
                        string filterValue = item.CurrentFilterValue;
                        if (filterValue.IsNullOrEmpty())
                        {
                            item.CurrentFilterFunction = Telerik.Web.UI.GridKnownFunction.NoFilter;
                        }
                    }
                    /*UAT-2904 Ends Here*/
                    Pair filter = (Pair)e.CommandArgument;

                    Int32 filterIndex = CurrentViewContext.ExceptionViewContract.FilterColumns.IndexOf(filter.Second.ToString());
                    if (filter.First.ToString() != GridKnownFunction.NoFilter.ToString())
                    {
                        String filteringType = grdExceptionItemData.MasterTableView.Columns.FindByUniqueName(filter.Second.ToString()).DataTypeName;
                        String filterValue = grdExceptionItemData.MasterTableView.Columns.FindByUniqueName(filter.Second.ToString()).CurrentFilterValue;

                        if (filterIndex != -1)
                        {
                            CurrentViewContext.ExceptionViewContract.FilterOperators[filterIndex] = filter.First.ToString();
                            CurrentViewContext.ExceptionViewContract.FilterOperators[filterIndex] = filter.First.ToString();
                            CurrentViewContext.ExceptionViewContract.FilterValues[filterIndex] = filterValue;
                        }
                        else
                        {
                            CurrentViewContext.ExceptionViewContract.FilterTypes.Add(filteringType);
                            CurrentViewContext.ExceptionViewContract.FilterColumns.Add(filter.Second.ToString());
                            CurrentViewContext.ExceptionViewContract.FilterOperators.Add(filter.First.ToString());
                            CurrentViewContext.ExceptionViewContract.FilterValues.Add(filterValue);

                        }
                    }
                    else if (filterIndex != -1)
                    {
                        CurrentViewContext.ExceptionViewContract.FilterOperators.RemoveAt(filterIndex);
                        CurrentViewContext.ExceptionViewContract.FilterValues.RemoveAt(filterIndex);
                        CurrentViewContext.ExceptionViewContract.FilterColumns.RemoveAt(filterIndex);
                        CurrentViewContext.ExceptionViewContract.FilterTypes.RemoveAt(filterIndex);
                    }

                    ViewState["ExceptionFilterColumns"] = CurrentViewContext.ExceptionViewContract.FilterColumns;
                    ViewState["ExceptionFilterOperators"] = CurrentViewContext.ExceptionViewContract.FilterOperators;
                    ViewState["ExceptionFilterValues"] = CurrentViewContext.ExceptionViewContract.FilterValues;
                    ViewState["ExceptionFilterTypes"] = CurrentViewContext.ExceptionViewContract.FilterTypes;
                }

                #endregion
                //Hide filter when exportig to pdf or word
                if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToPdfCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToWordCommandName
                    || e.CommandName == Telerik.Web.UI.RadGrid.ExportToExcelCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToCsvCommandName)
                {
                    base.ConfigureExport(grdExceptionItemData);

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
                            grdExceptionItemData.MasterTableView.GetColumn("CustomAttributesTemp").Display = true;
                        }
                        else
                        {
                            grdExceptionItemData.MasterTableView.GetColumn("CustomAttributesTemp").Display = false;
                        }
                    }
                }
                if (e.CommandName == "Cancel")
                {
                    grdExceptionItemData.MasterTableView.GetColumn("CustomAttributesTemp").Display = false;
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
        protected void grdExceptionItemData_SortCommand(object sender, GridSortCommandEventArgs e)
        {
            try
            {
                if (e.NewSortOrder != GridSortOrder.None)
                {
                    ViewState["ExceptionSortExpression"] = e.SortExpression;
                    ViewState["ExceptionSortDirection"] = e.NewSortOrder.Equals(GridSortOrder.Descending);
                    CurrentViewContext.ExceptionGridCustomPaging.SortExpression = e.SortExpression;
                    CurrentViewContext.ExceptionGridCustomPaging.SortDirectionDescending = e.NewSortOrder.Equals(GridSortOrder.Descending);
                }
                else
                {
                    ViewState["ExceptionSortExpression"] = String.Empty;
                    ViewState["ExceptionSortDirection"] = false;
                    CurrentViewContext.ExceptionGridCustomPaging.SortExpression = String.Empty;
                    CurrentViewContext.ExceptionGridCustomPaging.SortDirectionDescending = false;
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
        /// Handles the assignment of items to the users.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        protected void chkSelectExpItem_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox checkBox = sender as CheckBox;
                Boolean isChecked = false;
                if (checkBox.IsNull())
                {
                    return;
                }
                GridDataItem dataItem = (GridDataItem)checkBox.NamingContainer;
                AssignQueueRecords assignQueueRecords = new AssignQueueRecords();
                List<AssignQueueRecords> items = SelectedVerificationItemsNew;
                Int32 applicantComplianceItemID = (Int32)dataItem.GetDataKeyValue("ApplicantComplianceItemID");
                Int32 complianceItemId = (Int32)dataItem.GetDataKeyValue("ComplianceItemId");
                Int32 categoryId = (Int32)dataItem.GetDataKeyValue("CategoryId");
                String status = (String)dataItem.GetDataKeyValue("VerificationStatusCode");
                String itemName = (String)dataItem.GetDataKeyValue("ItemName");
                isChecked = ((CheckBox)dataItem.FindControl("chkSelectedException")).Checked;
                assignQueueRecords.ApplicantComplienceItemId = applicantComplianceItemID;
                assignQueueRecords.ComplianceItemId = complianceItemId;
                assignQueueRecords.CategoryId = categoryId;
                assignQueueRecords.verificationStatusCode = status;
                assignQueueRecords.ItemName = itemName;
                assignQueueRecords.IsChecked = isChecked;
                assignQueueRecords.tenantID = SelectedTenantId;
                assignQueueRecords.workQueueType = SelectedWorkQueueType.ToString();
                assignQueueRecords.IsDefaultThirdPartyTenant = true;
                assignQueueRecords.IsEsclationRecord = IsEscalationRecords;
                assignQueueRecords.QueueCode = QueueCode;

                if (!(Presenter.IsDefaultTenant || Presenter.IsThirdPartyTenant))
                {
                    assignQueueRecords.IsDefaultThirdPartyTenant = false;
                    assignQueueRecords.tenantID = TenantId;
                    List<KeyValuePair<Int32, Int32>> selectedNodes = CurrentViewContext.SelectedExceptionNodes;
                    Int32 hierarchyNodeID = (Int32)dataItem.GetDataKeyValue("HierarchyNodeID");
                    //if (selectedNodes.ContainsKey(hierarchyNodeID) && (selectedNodes[hierarchyNodeID] == complianceItemID || selectedNodes[hierarchyNodeID] == 0))
                    if (selectedNodes.Where(x => x.Key == hierarchyNodeID && x.Value == applicantComplianceItemID).Count() > 0)
                    {
                        if (isChecked)
                            selectedNodes.Add(new KeyValuePair<Int32, Int32>(hierarchyNodeID, applicantComplianceItemID));
                        else
                            selectedNodes.Remove(new KeyValuePair<Int32, Int32>(hierarchyNodeID, applicantComplianceItemID));
                        //selectedNodes[hierarchyNodeID] = isChecked ? complianceItemID : 0;
                    }
                    else
                    {
                        selectedNodes.Add(new KeyValuePair<Int32, Int32>(hierarchyNodeID, applicantComplianceItemID));
                        //selectedNodes.Add(hierarchyNodeID, complianceItemID);
                    }
                    CurrentViewContext.SelectedExceptionNodes = selectedNodes;
                }
                if (items.Any(x => x.ApplicantComplienceItemId == applicantComplianceItemID))
                {
                    items.FirstOrDefault(x => x.ApplicantComplienceItemId == applicantComplianceItemID).IsChecked = isChecked;
                }
                else
                {
                    items.Add(assignQueueRecords);
                }
                SelectedVerificationItemsNew = items;
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
                pnlException.Visible = false;
                chkSelectAllResults.Visible = false;
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

        protected void ddlExpSelectedUser_DataBound(object sender, EventArgs e)
        {
            ddlExpSelectedUser.Items.Insert(0, new RadComboBoxItem("--Select--"));
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

        #region Button Events

        protected void btnExpAssignUser_Click(object sender, EventArgs e)
        {
            if (ddlExpSelectedUser.SelectedValue != "")
            {
                ExpSelectedUserId = Convert.ToInt32(ddlExpSelectedUser.SelectedValue);
                if (Presenter.AssignItemsToUser(SelectedExceptionItems, SelectedExceptionNodes, ExpSelectedUserId))
                {
                    SelectedVerificationItemsNew = new List<AssignQueueRecords>();
                    SelectedExceptionNodes = new List<KeyValuePair<Int32, Int32>>();
                    SetExpGridFilters();
                    grdExceptionItemData.Rebind();
                    ddlExpSelectedUser.DataBind();
                    ddlExpSelectedUser.SelectedIndex = 0;
                    ShowStatusMessage("sucs", AppConsts.MSG_ITEM_ASSIGNED_SUCCESS, false);
                    chkSelectAllResults.Checked = false;
                }
                else
                {
                    if (String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                    {
                        ShowStatusMessage("error", AppConsts.MSG_SELECT_ITEM, false);
                    }
                    else
                    {
                        CurrentViewContext.ErrorMessage = String.Format(CurrentViewContext.ErrorMessage, ddlExpSelectedUser.SelectedItem.Text);
                        ShowStatusMessage("error", CurrentViewContext.ErrorMessage, false);
                    }
                }
            }
            else
            {
                ShowStatusMessage("error", AppConsts.MSG_SELECT_USER, false);
            }
        }

        protected void CmdBarSearch_Click(object sender, EventArgs e)
        {
            //Presenter.PerformSearch();
            //grdItemData.DataSource = CurrentViewContext.ItemData;
            //grdItemData.DataBind();
            grdExceptionItemData.Visible = true;
            lblExceptionHdr.Visible = true;
            chkSelectAllResults.Visible = true;
            CurrentViewContext.SelectedVerificationItemsNew = new List<AssignQueueRecords>();

            if (!ddlTenantName.SelectedValue.IsNullOrEmpty())
            {
                //ClearViewStatesForFilter(); //Commented for UAT-2904
                SetExpGridFilters();
                grdExceptionItemData.Rebind();
                pnlException.Visible = true;
                if (SelectedWorkQueueType == WorkQueueType.ExceptionAssignmentWorkQueue)
                    chkSelectAllResults.Visible = true;
            }
            chkSelectAllResults.Checked = false;
        }

        protected void CmdBarReset_Click(object sender, EventArgs e)
        {
            Presenter.OnViewInitialized();
            // BindTenantDropDown();    UAT-789
            CurrentViewContext.SelectedUserGroupId = 0;
            CurrentViewContext.SelectedPackageId = 0;
            CurrentViewContext.SelectedCategoryId = 0;
            CurrentPageIndex = 1;
            ClearViewStatesForFilter();
            SetExpGridFilters();
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
            ddlCategory.SelectedIndex = 0;
            ddlPackage.SelectedIndex = 0;
            grdExceptionItemData.MasterTableView.SortExpressions.Clear();
            grdExceptionItemData.MasterTableView.FilterExpression = String.Empty;  //UAT-2904
            chkShowRushOrders.Checked = false;
            ddlUserGroup.SelectedIndex = 0;

            if (ShowClientDropDown)
            {
                ddlTenantName.SelectedIndex = 0;
                CurrentViewContext.SelectedTenantId = 0;
                SelectedTenantId = 0;
                ddlPackage.Enabled = false;
                ddlCategory.Enabled = false;
                ddlUserGroup.Enabled = false;
                pnlException.Visible = false;
                chkSelectAllResults.Visible = false;
            }

            CurrentViewContext.VirtualPageCount = 0;
            chkSelectAllResults.Checked = false;
            /*UAT-2904*/
            foreach (GridColumn column in grdExceptionItemData.MasterTableView.Columns)
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

        protected void chkShowRushOrders_CheckedChanged(object sender, EventArgs e)
        {
            grdExceptionItemData.Rebind();
        }
        #endregion

        #region Methods

        #region Private Methods

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

        private void ShowStatusMessage(String cssClass, String message, Boolean isVerificationMsg)
        {
            lblExpError.Text = message;
            lblExpError.CssClass = cssClass;
            pnlExpError.Update();
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
            if (ExpSelectedUserId > 0)
            {
                verificationQueueFilters.ExpAssignToUserID = ExpSelectedUserId;
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
                verificationQueueFilters.SelectedDPMIds = DPMIds;
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
            verificationQueueFilters.WorkQueueType = SelectedWorkQueueType;

            verificationQueueFilters.FilterColumns = ViewState["ExceptionFilterColumns"].IsNullOrEmpty() ? null : (List<String>)ViewState["ExceptionFilterColumns"];
            verificationQueueFilters.FilterOperators = ViewState["ExceptionFilterOperators"].IsNullOrEmpty() ? null : (List<String>)ViewState["ExceptionFilterOperators"];
            verificationQueueFilters.FilterValues = ViewState["ExceptionFilterValues"].IsNullOrEmpty() ? null : (ArrayList)ViewState["ExceptionFilterValues"];
            verificationQueueFilters.FilterTypes = ViewState["ExceptionFilterTypes"].IsNullOrEmpty() ? null : (List<String>)ViewState["ExceptionFilterTypes"];
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
                    pnlException.Visible = true;
                    if (SelectedWorkQueueType == WorkQueueType.ExceptionAssignmentWorkQueue)
                        chkSelectAllResults.Visible = true;
                    SelectedWorkQueueType = verificationQueueFilters.WorkQueueType;
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
                    chkShowRushOrders.Checked = verificationQueueFilters.ShowRushOrders;
                    if (verificationQueueFilters.UserGroupId > 0)
                    {
                        SelectedUserGroupId = verificationQueueFilters.UserGroupId;
                        ddlUserGroup.SelectedValue = SelectedUserGroupId.ToString();
                    }
                    //grdExceptionItemData.Rebind();
                    if (verificationQueueFilters.ExpAssignToUserID > 0 && pnlExpShowUsers.Visible == true)
                    {
                        ExpSelectedUserId = verificationQueueFilters.ExpAssignToUserID;
                        ddlExpSelectedUser.SelectedValue = ExpSelectedUserId.ToString();
                    }

                    #region Custom Attributes
                    //ucCustomAttributeLoaderNodeSearch.NodeId = verificationQueueFilters.NodeId.IsNotNull() ? Convert.ToInt32(verificationQueueFilters.NodeId) : 0;
                    ucCustomAttributeLoaderNodeSearch.NodeIds = !verificationQueueFilters.NodeIds.IsNullOrEmpty() ? verificationQueueFilters.NodeIds : String.Empty;
                    ucCustomAttributeLoaderNodeSearch.TenantId = CurrentViewContext.SelectedTenantId = verificationQueueFilters.TenantId;
                    ucCustomAttributeLoaderNodeSearch.previousValues = verificationQueueFilters.CustomFields;

                    ucCustomAttributeLoaderNodeSearch.nodeLable = verificationQueueFilters.NodeLabel;

                    ucCustomAttributeLoaderNodeSearch.DPM_ID = verificationQueueFilters.SelectedDPMIds;
                    ucCustomAttributeLoaderNodeSearch.ScreenType = "CommonScreen";
                    #endregion

                    ViewState["ExceptionFilterColumns"] = CurrentViewContext.ExceptionViewContract.FilterColumns = verificationQueueFilters.FilterColumns;
                    ViewState["ExceptionFilterOperators"] = CurrentViewContext.ExceptionViewContract.FilterOperators = verificationQueueFilters.FilterOperators;
                    ViewState["ExceptionFilterValues"] = CurrentViewContext.ExceptionViewContract.FilterValues = verificationQueueFilters.FilterValues;
                    ViewState["ExceptionFilterTypes"] = CurrentViewContext.ExceptionViewContract.FilterTypes = verificationQueueFilters.FilterTypes;
                    SetFilterValues();
                }
                SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.SESSION_KEY_VERIFICATIONQUEUE, null);
            }
        }

        public void SetFilterValues()
        {
            if (!CurrentViewContext.ExceptionViewContract.FilterColumns.IsNullOrEmpty() && CurrentViewContext.ExceptionViewContract.FilterColumns.Count > 0)
            {
                CurrentViewContext.ExceptionViewContract.FilterColumns.ForEach(x =>
                    grdExceptionItemData.Columns.FindByUniqueName(x).CurrentFilterValue = CurrentViewContext.ExceptionViewContract.FilterValues[CurrentViewContext.ExceptionViewContract.FilterColumns.IndexOf(x)].ToString()
                    );
            }
        }

        private void SetExpGridFilters()
        {
            if (!ViewState["ExceptionSortExpression"].IsNull())
            {
                CurrentViewContext.ExceptionGridCustomPaging.SortExpression = Convert.ToString(ViewState["ExceptionSortExpression"]);
                CurrentViewContext.ExceptionGridCustomPaging.SortDirectionDescending = Convert.ToBoolean(ViewState["ExceptionSortDirection"]);
            }

            CurrentViewContext.ExceptionViewContract.FilterColumns = ViewState["ExceptionFilterColumns"] == null ? new List<String>() : (List<String>)(ViewState["ExceptionFilterColumns"]);
            CurrentViewContext.ExceptionViewContract.FilterOperators = ViewState["ExceptionFilterOperators"] == null ? new List<String>() : (List<String>)(ViewState["ExceptionFilterOperators"]);
            CurrentViewContext.ExceptionViewContract.FilterValues = ViewState["ExceptionFilterValues"] == null ? new ArrayList() : (ArrayList)(ViewState["ExceptionFilterValues"]);
            CurrentViewContext.ExceptionViewContract.FilterTypes = ViewState["ExceptionFilterTypes"] == null ? new List<String>() : (List<String>)(ViewState["ExceptionFilterTypes"]);
        }

        public void ClearViewStatesForFilter()
        {
            ViewState["ExceptionFilterColumns"] = null;
            ViewState["ExceptionFilterOperators"] = null;
            ViewState["ExceptionFilterValues"] = null;
            ViewState["ExceptionFilterTypes"] = null;
        }

        #endregion

        protected void chkSelectAllResults_CheckedChanged(object sender, EventArgs e)
        {
            if (CurrentViewContext.VirtualPageCount > 0)
            {
                bool needToCheckboxChecked = false;
                bool needToMarkHeadCheckboxChecked = true;

                if (((CheckBox)sender).Checked)
                {
                    SetExpGridFilters();
                    ExceptionGridCustomPaging.FilterColumns = ExceptionViewContract.FilterColumns;
                    ExceptionGridCustomPaging.FilterOperators = ExceptionViewContract.FilterOperators;
                    ExceptionGridCustomPaging.FilterValues = ExceptionViewContract.FilterValues;
                    ExceptionGridCustomPaging.FilterTypes = ExceptionViewContract.FilterTypes;

                    Presenter.GetAllVerificationItems();
                    needToCheckboxChecked = true;
                    needToMarkHeadCheckboxChecked = true;
                }
                else
                {
                    CurrentViewContext.SelectedVerificationItemsNew = new List<AssignQueueRecords>();
                    needToMarkHeadCheckboxChecked = false;
                }

                if (needToCheckboxChecked)
                {
                    List<AssignQueueRecords> selectedItems = CurrentViewContext.SelectedVerificationItemsNew;

                    foreach (GridDataItem item in grdExceptionItemData.Items)
                    {
                        CheckBox checkBox = ((CheckBox)item.FindControl("chkSelectedException"));
                        String itemDataId = item.OwnerTableView.DataKeyValues[item.ItemIndex]["ApplicantComplianceItemID"].ToString();

                        if (!selectedItems.IsNullOrEmpty() && selectedItems.Any(x => x.ApplicantComplienceItemId == (Convert.ToInt32(itemDataId)) && x.IsChecked))
                        {
                            checkBox.Checked = needToCheckboxChecked;
                        }
                        else
                        {
                            checkBox.Checked = false;
                            needToMarkHeadCheckboxChecked = false;
                        }
                    }
                }
                else
                {
                    foreach (GridDataItem item in grdExceptionItemData.Items)
                    {
                        CheckBox checkBox = ((CheckBox)item.FindControl("chkSelectedException"));
                        checkBox.Checked = needToCheckboxChecked;
                    }
                }

                GridHeaderItem headerItem = grdExceptionItemData.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
                CheckBox headerCheckBox = ((CheckBox)headerItem.FindControl("chkSelectAllExceptions"));
                headerCheckBox.Checked = needToMarkHeadCheckboxChecked;

                GridFooterItem footerItem = grdExceptionItemData.MasterTableView.GetItems(GridItemType.Footer)[0] as GridFooterItem;
                CheckBox footerCheckBox = ((CheckBox)footerItem.FindControl("chkSelectAllExceptionsFooter"));
                footerCheckBox.Checked = needToMarkHeadCheckboxChecked;
            }
        }

        #region Public Methods

        #endregion

        #endregion

    }
}

