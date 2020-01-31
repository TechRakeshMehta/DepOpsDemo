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
using System.Collections;
using System.Web.UI;
using System.Xml.Serialization;
using System.Text;
using System.IO;
using System.Threading;
using System.Linq;
using CoreWeb.IntsofSecurityModel;
using WebSiteUtils.SharedObjects;
using Business.RepoManagers;
using INTERSOFT.WEB.UI.WebControls;


namespace CoreWeb.ComplianceOperations.Views
{
    public partial class ItemsDataVerificationQueue : BaseUserControl, IItemsDataVerificationQueueView
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private ItemsDataVerificationQueuePresenter _presenter = new ItemsDataVerificationQueuePresenter();
        private String _viewType;
        private Int32 _tenantid;
        private Boolean _showClientDropdwn;
        private Int32 _verSelectedUserId;
        private Int32 _expSelectedUserId;
        private CustomPagingArgsContract _verificationGridCustomPaging = null;
        private CustomPagingArgsContract _exceptionGridCustomPaging = null;
        private ApplicantComplianceItemDataContract _verificationviewContract = null;
        private ApplicantComplianceItemDataContract _exceptionviewContract = null;

        #endregion

        #endregion

        #region Properties

        public ItemsDataVerificationQueuePresenter Presenter
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


        /// <summary>
        /// Sets or gets the Selected User Id.
        /// </summary>
        public Int32 VerSelectedUserId
        {
            get
            {
                return _verSelectedUserId;
            }
            set
            {
                _verSelectedUserId = value;
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
        public IItemsDataVerificationQueueView CurrentViewContext
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
                ddlVerSelectedUser.DataSource = value;
                ddlVerSelectedUser.DataBind();
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
        /// Gets or Sets the value for selected Items.
        /// </summary>
        public Dictionary<Int32, Boolean> SelectedVerificationItems
        {
            get
            {
                if (!ViewState["SelectedVerificationItems"].IsNull())
                {
                    return ViewState["SelectedVerificationItems"] as Dictionary<Int32, Boolean>;
                }

                return new Dictionary<Int32, Boolean>();
            }
            set
            {
                ViewState["SelectedVerificationItems"] = value;
            }
        }

        /// <summary>
        /// Gets or Sets the value for selected Nodes.
        /// </summary>
        public List<KeyValuePair<Int32, Int32>> SelectedVerificationNodes
        {
            get
            {
                if (!ViewState["SelectedVerificationNodes"].IsNull())
                {
                    return ViewState["SelectedVerificationNodes"] as List<KeyValuePair<Int32, Int32>>;
                }

                return new List<KeyValuePair<Int32, Int32>>();
            }
            set
            {
                ViewState["SelectedVerificationNodes"] = value;
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
        /// Sets and gets option for showing incomplete Items in the Queue. 
        /// </summary>
        public Boolean ShowIcompleteItems
        {
            get
            {
                return chkShowIncompleteItems.Checked;
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
            {
                // Maximum 100 record allowed from DB. 
                // return grdVerificationItemData.PageSize > 100 ? 100 : grdVerificationItemData.PageSize;
                return grdVerificationItemData.PageSize;
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
                return grdVerificationItemData.MasterTableView.VirtualItemCount;
            }
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

        public List<UserGroup> lstUserGroup
        {
            get;
            set;
        }

        public Boolean IsEscalationRecords
        {
            get { return Convert.ToBoolean(ViewState["IsEscalationRecords_Verification"]); }
            set { ViewState["IsEscalationRecords_Verification"] = value; }
        }

        public String QueueCode
        {
            get { return Convert.ToString(ViewState["QueueCode_Verification"]); }
            set { ViewState["QueueCode_Verification"] = value; }
        }

        //UAT 2809
        Boolean IItemsDataVerificationQueueView.IsUserAlreadyAssigned
        {
            get;
            set;
        }

        //UAT 2809
        Boolean IItemsDataVerificationQueueView.IsMutipleTimesAssignmentAllowed
        {
            get
            {
                if (!ViewState["IsMutipleTimesAssignmentAllowed"].IsNullOrEmpty())
                {
                    return Convert.ToBoolean(ViewState["IsMutipleTimesAssignmentAllowed"]);
                }
                return false;
            }
            set
            {
                ViewState["IsMutipleTimesAssignmentAllowed"] = value;
            }
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
                String title = "Verification Queue";
                if (!Request.QueryString["args"].IsNull())
                {
                    args.ToDecryptedQueryString(Request.QueryString["args"]);
                    if (args.ContainsKey("QCODE"))
                    {
                        title = "Verification Queue (Escalation)";
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
                            //IsEscalationRecords = LookupManager.GetLookUpData<Entity.ClientEntity.QueueMetaData>(2).Where(x => x.QMD_IsDeleted == false && x.QMD_Code == QueueCode).FirstOrDefault().QMD_IsEscalationQueue;
                            //if (IsEscalationRecords)
                            //{
                            dvIncmplt.Style.Add("display", "none");
                            base.Title = "Verification Queue (Escalation)";
                            base.SetPageTitle("Verification Queue (Escalation)");
                            // }
                        }
                        GetSessionValues();
                    }
                    else
                    {
                        grdVerificationItemData.Visible = false;
                        lblPageHdr.Visible = false;
                        chkSelectAllResults.Visible = false;
                    }

                    if (!Presenter.IsDefaultTenant)
                    {
                        btnAutomaticItemAssigning.Visible = false;
                    }

                }
                //this._presenter.OnViewLoaded();
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
                IsVerificationGrid = true;
                VerificationGridCustomPaging.CurrentPageIndex = CurrentPageIndex;
                VerificationGridCustomPaging.PageSize = PageSize;
                VerificationGridCustomPaging.FilterColumns = VerificationViewContract.FilterColumns;
                VerificationGridCustomPaging.FilterOperators = VerificationViewContract.FilterOperators;
                VerificationGridCustomPaging.FilterValues = VerificationViewContract.FilterValues;
                VerificationGridCustomPaging.FilterTypes = VerificationViewContract.FilterTypes;
                Session[AppConsts.VERIFICATION_QUEUE_SESSION_KEY] = VerificationGridCustomPaging;
                Presenter.GetVerificationQueueData();
                grdVerificationItemData.DataSource = lstVerificationQueue;

                if (lstVerificationQueue.IsNotNull() && lstVerificationQueue.Count > 0)
                {
                    pnlVerShowUsers.Visible = true;
                    Presenter.GetUserListForSelectedTenant();
                }
                else
                {
                    pnlVerShowUsers.Visible = false;
                }

                if (IsEscalationRecords)
                {
                    grdVerificationItemData.MasterTableView.GetColumn("ReviewLevel").Visible = false;
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
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that
        ///  contains the event data.</param>
        protected void grdVerificationItemData_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
                {
                    String itemDataId = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ApplicantComplianceItemID"].ToString();
                    String verificationStatusCode = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["VerificationStatusCode"].ToString();

                    //To Fixed UAT 410-Addition of "Custom Field" column to searches and verification queues
                    GridDataItem dataItem = (GridDataItem)e.Item;
                    if (Convert.ToString(dataItem["CustomAttributes"].Text).Length > 80)
                    {
                        dataItem["CustomAttributes"].ToolTip = dataItem["CustomAttributes"].Text;
                        dataItem["CustomAttributes"].Text = (dataItem["CustomAttributes"].Text).ToString().Substring(0, 80) + "...";
                    }

                    if ((Convert.ToInt32(itemDataId) != 0) && (!verificationStatusCode.IsNullOrEmpty() && verificationStatusCode != "INCM"))
                    {
                        //Dictionary<Int32, Boolean> selectedItems = CurrentViewContext.SelectedVerificationItems;
                        List<AssignQueueRecords> selectedItems = SelectedVerificationItemsNew;
                        if (selectedItems.Any(x => x.ApplicantComplienceItemId == (Convert.ToInt32(itemDataId)) && x.IsChecked))
                        {
                            CheckBox checkBox = ((CheckBox)e.Item.FindControl("chkSelectItem"));
                            checkBox.Checked = true;
                        }
                    }
                    else
                    {
                        CheckBox checkBox = ((CheckBox)e.Item.FindControl("chkSelectItem"));
                        checkBox.Enabled = false;
                        RadButton button = ((RadButton)e.Item.FindControl("btnEditNew"));
                        button.Enabled = false;
                    }

                    //UAT 2528
                    Boolean IsAdminLoggedIn = (Presenter.IsDefaultTenant);
                    //Boolean IsAdminLoggedIn = (SecurityManager.DefaultTenantID == TenantId);
                    if (IsAdminLoggedIn)
                    {
                        Boolean IsUiRulesViolate = Convert.ToBoolean(dataItem["IsUiRulesViolate"].Text);
                        if (IsUiRulesViolate)
                        {
                            dataItem.Attributes.Add("Style", "background-color:#ff6666 !important");
                        }
                    }

                }
                if (e.Item.ItemType.Equals(GridItemType.Footer))
                {
                    Int32 rowCount = grdVerificationItemData.Items.Count;
                    if (rowCount > 0)
                    {
                        Int32 checkCount = 0;
                        foreach (GridDataItem item in grdVerificationItemData.Items)
                        {
                            CheckBox checkBox = ((CheckBox)item.FindControl("chkSelectItem"));
                            if (checkBox.Checked)
                            {
                                checkCount++;
                            }
                        }
                        if (rowCount == checkCount)
                        {
                            GridHeaderItem item = grdVerificationItemData.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
                            CheckBox checkBox = ((CheckBox)item.FindControl("chkSelectAll"));
                            checkBox.Checked = true;
                            GridFooterItem itemFooter = grdVerificationItemData.MasterTableView.GetItems(GridItemType.Footer)[0] as GridFooterItem;
                            CheckBox checkBoxFooter = ((CheckBox)itemFooter.FindControl("chkSelectAllFooter"));
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

        protected void grdVerificationItemData_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                #region DetailScreenNavigation

                String workQueueType = String.Empty;

                if (IsEscalationRecords)
                {
                    workQueueType = WorkQueueType.EsclationAssignmentWorkQueue.ToString();
                }
                else
                {
                    workQueueType = WorkQueueType.AssignmentWorkQueue.ToString();
                }

                if (e.CommandName.Equals("ViewDetail"))
                {
                    Int32 selectedTenantId = 0;
                    if (!ddlTenantName.SelectedValue.IsNullOrEmpty())
                    {
                        selectedTenantId = SelectedTenantId;
                    }
                    else
                    {
                        selectedTenantId = TenantId;
                    }

                    if (ddlVerSelectedUser.SelectedValue.IsNullOrEmpty())
                    {
                        VerSelectedUserId = 0;
                    }
                    else
                    {
                        VerSelectedUserId = Convert.ToInt32(ddlVerSelectedUser.SelectedValue);
                    }
                    SetSessionValues();
                    Dictionary<String, String> queryString = new Dictionary<String, String>();
                    String itemDataId = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ApplicantComplianceItemID"].ToString();
                    String applicantId = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ApplicantId"].ToString();
                    queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "SelectedTenantId", Convert.ToString( selectedTenantId) },
                                                                    { "Child", ChildControls.VerificationDetails},
                                                                    { "ItemDataId", itemDataId},
                                                                    {"IsException","false"},
                                                                    {"WorkQueueType",workQueueType},
                                                                    {"PackageId",SelectedPackageId.ToString()},
                                                                    {"CategoryId",SelectedCategoryId.ToString()}, 
                                                                    {"AssignedToVerUser",VerSelectedUserId.ToString()},
                                                                    {"IncludeIncompleteItems",chkShowIncompleteItems.Checked.ToString()},
                                                                    {"ShowOnlyRushOrders",ShowOnlyRushOrders.ToString()},
                                                                    {"UserGroupId",SelectedUserGroupId.ToString()},
                                                                    {"ApplicantId",applicantId},
                                                                    {"IsEscalationRecords",Convert.ToString(IsEscalationRecords) }
                                                                 };
                    string url = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                    Response.Redirect(url, true);

                }
                #endregion

                #region DetailScreenNavigationNew

                if (e.CommandName.Equals("ViewDetailNew"))
                {
                    Int32 selectedTenantId = 0;
                    if (!ddlTenantName.SelectedValue.IsNullOrEmpty())
                    {
                        selectedTenantId = SelectedTenantId;
                    }
                    else
                    {
                        selectedTenantId = TenantId;
                    }

                    if (ddlVerSelectedUser.SelectedValue.IsNullOrEmpty())
                    {
                        VerSelectedUserId = 0;
                    }
                    else
                    {
                        VerSelectedUserId = Convert.ToInt32(ddlVerSelectedUser.SelectedValue);
                    }
                    SetSessionValues();
                    Int32 _selectedCategoryId = Convert.ToInt32((e.Item.FindControl("hdfCatId") as HiddenField).Value);
                    Int32 _selectedPackageSubscriptionId = Convert.ToInt32((e.Item.FindControl("hdfPackSubscriptionId") as HiddenField).Value);

                    Dictionary<String, String> queryString = new Dictionary<String, String>();
                    String itemDataId = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ApplicantComplianceItemID"].ToString();
                    String applicantId = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ApplicantId"].ToString();

                    #region UAT-4067
                    Presenter.GetSelectedNodeIDBySubscriptionID(CurrentViewContext.SelectedTenantId, _selectedPackageSubscriptionId);
                    Presenter.GetAllowedFileExtensions();
                    #endregion


                    queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "TenantId", Convert.ToString( selectedTenantId) },
                                                                    { "Child", ChildControls.VerificationDetailsNew},
                                                                    { "ItemDataId", itemDataId},
                                                                    {"WorkQueueType",workQueueType},
                                                                    {"PackageId",SelectedPackageId.ToString()},
                                                                    {"IsException","false"},
                                                                    {"CategoryId",SelectedCategoryId.ToString()}, 
                                                                    {"AssignedToVerUser",VerSelectedUserId.ToString()},
                                                                    {"IncludeIncompleteItems",chkShowIncompleteItems.Checked.ToString()},
                                                                    {"SelectedComplianceCategoryId",Convert.ToString( _selectedCategoryId)},
                                                                    {"SelectedPackageSubscriptionId",Convert.ToString( _selectedPackageSubscriptionId)},
                                                                    {"ShowOnlyRushOrders",Convert.ToString( chkShowRushOrders.Checked)},
                                                                    {"UserGroupId",SelectedUserGroupId.ToString()},
                                                                    {"ApplicantId",applicantId},
                                                                    {"IsEscalationRecords",Convert.ToString(IsEscalationRecords) }
                                                                    ,{"allowedFileExtensions", String.Join(",",CurrentViewContext.allowedFileExtensions)} //UAT-4067
                                                                 };
                    string url = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                    Response.Redirect(url, true);

                }
                #endregion

                base.InsertOrEdit(grdVerificationItemData, e);

                #region For Filter command
                SetVerGridFilters();
                if (e.CommandName == RadGrid.FilterCommandName)
                {
                    /*UAT-2904(set FilterFunction value of column filters to NoFilter,If no action is performed on them)*/
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

        /// <summary>
        /// Handles the assignment of items to the users.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        protected void chkSelectVerItem_CheckedChanged(object sender, EventArgs e)
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
                //Dictionary<Int32, Boolean> selectedItems = CurrentViewContext.SelectedVerificationItems;
                Int32 applicantComplianceItemID = (Int32)dataItem.GetDataKeyValue("ApplicantComplianceItemID");
                Int32 complianceItemId = (Int32)dataItem.GetDataKeyValue("ComplianceItemId");
                Int32 categoryId = (Int32)dataItem.GetDataKeyValue("CategoryId");
                String status = (String)dataItem.GetDataKeyValue("VerificationStatusCode");
                String itemName = (String)dataItem.GetDataKeyValue("ItemName");
                isChecked = ((CheckBox)dataItem.FindControl("chkSelectItem")).Checked;
                assignQueueRecords.ApplicantComplienceItemId = applicantComplianceItemID;
                assignQueueRecords.ComplianceItemId = complianceItemId;
                assignQueueRecords.CategoryId = categoryId;
                assignQueueRecords.verificationStatusCode = status;
                assignQueueRecords.ItemName = itemName;
                assignQueueRecords.IsChecked = isChecked;
                assignQueueRecords.tenantID = SelectedTenantId;
                assignQueueRecords.IsDefaultThirdPartyTenant = true;
                assignQueueRecords.IsEsclationRecord = IsEscalationRecords;
                assignQueueRecords.QueueCode = QueueCode;

                if (!(Presenter.IsDefaultTenant || Presenter.IsThirdPartyTenant))
                {
                    assignQueueRecords.IsDefaultThirdPartyTenant = false;
                    assignQueueRecords.tenantID = TenantId;
                    List<KeyValuePair<Int32, Int32>> selectedNodes = CurrentViewContext.SelectedVerificationNodes;
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
                    CurrentViewContext.SelectedVerificationNodes = selectedNodes;
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
                SelectedTenantId = 0;
                ddlPackage.Enabled = false;
                ddlCategory.Enabled = false;
                ddlUserGroup.Enabled = false;
                pnlVerification.Visible = false;
                chkSelectAllResults.Visible = false;
                ucCustomAttributeLoaderNodeSearch.Reset();
            }
            else
            {
                SetDataForTenantSelected();
                ucCustomAttributeLoaderNodeSearch.Reset(Convert.ToInt32(ddlTenantName.SelectedValue));
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

        protected void ddlVerSelectedUser_DataBound(object sender, EventArgs e)
        {
            ddlVerSelectedUser.Items.Insert(0, new RadComboBoxItem("--Select--"));
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

        protected void CmdBarSearch_Click(object sender, EventArgs e)
        {
            grdVerificationItemData.Visible = true;
            lblPageHdr.Visible = true;
            chkSelectAllResults.Visible = true;
            CurrentViewContext.SelectedVerificationItemsNew = new List<AssignQueueRecords>();
            chkSelectAllResults.Checked = false;

            if (!ddlTenantName.SelectedValue.IsNullOrEmpty())
            {
                //ClearViewStatesForFilter(); 
                SetVerGridFilters();
                grdVerificationItemData.Rebind();
                pnlVerification.Visible = true;
                chkSelectAllResults.Visible = true;
            }
        }

        protected void CmdBarReset_Click(object sender, EventArgs e)
        {
            Presenter.OnViewInitialized();
            //  BindTenantDropDown();
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
            CurrentPageIndex = 1;
            ViewState["SortExpression"] = null;
            ViewState["SortDirection"] = null;
            grdVerificationItemData.MasterTableView.SortExpressions.Clear();
            grdVerificationItemData.MasterTableView.FilterExpression = String.Empty; //UAT-2904
            ddlCategory.SelectedIndex = 0;
            ddlPackage.SelectedIndex = 0;
            ddlUserGroup.SelectedIndex = 0;
            chkShowIncompleteItems.Checked = false;
            chkShowRushOrders.Checked = false;
            ClearViewStatesForFilter();
            SetVerGridFilters();

            if (ShowClientDropDown)
            {

                ddlTenantName.SelectedIndex = 0;
                SelectedTenantId = 0;
                CurrentViewContext.SelectedTenantId = 0;
                ddlPackage.Enabled = false;
                ddlCategory.Enabled = false;
                ddlUserGroup.Enabled = false;
                pnlVerification.Visible = false;
                chkSelectAllResults.Visible = false;
            }

            CurrentViewContext.SelectedVerificationItemsNew = new List<AssignQueueRecords>();
            chkSelectAllResults.Checked = false;
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

        protected void btnVerAssignUser_Click(object sender, EventArgs e)
        {
            if (ddlVerSelectedUser.SelectedValue != "")
            {
                VerSelectedUserId = Convert.ToInt32(ddlVerSelectedUser.SelectedValue);
                CurrentViewContext.IsMutipleTimesAssignmentAllowed = Convert.ToBoolean(hdnIsMutipleTimesAssignmentAllowed.Value); //UAT 2809
                if (Presenter.AssignItemsToUser(VerSelectedUserId))
                {
                    SelectedVerificationItemsNew = new List<AssignQueueRecords>();
                    SelectedVerificationNodes = new List<KeyValuePair<Int32, Int32>>();
                    SetVerGridFilters();
                    grdVerificationItemData.Rebind();
                    ddlVerSelectedUser.DataBind();
                    ddlVerSelectedUser.SelectedIndex = 0;
                    ShowStatusMessage("sucs", AppConsts.MSG_ITEM_ASSIGNED_SUCCESS, true);
                    chkSelectAllResults.Checked = false;
                    hdnIsMutipleTimesAssignmentAllowed.Value = "false";
                }
                else
                {
                    //UAT 2809
                    if (CurrentViewContext.IsUserAlreadyAssigned && !String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                    {
                        lblConfirmMessage.Text = CurrentViewContext.ErrorMessage;
                        System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ShowConfirmationPopUp();", true);
                        return;
                    }
                    if (String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                    {
                        ShowStatusMessage("error", AppConsts.MSG_SELECT_ITEM, true);
                    }
                    else
                    {
                        CurrentViewContext.ErrorMessage = String.Format(CurrentViewContext.ErrorMessage, ddlVerSelectedUser.SelectedItem.Text);
                        ShowStatusMessage("error", CurrentViewContext.ErrorMessage, true);
                    }
                }
            }
            else
            {
                ShowStatusMessage("error", AppConsts.MSG_SELECT_USER, true);
            }
        }

        #endregion

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
                chkSelectAllResults.Visible = true;

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
                if (args.ContainsKey("IncludeIncompleteItems"))
                {
                    chkShowIncompleteItems.Checked = Convert.ToBoolean(args["IncludeIncompleteItems"]);
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
                if (args.ContainsKey("AssignedToVerUser") && pnlVerShowUsers.Visible == true)
                {
                    ddlVerSelectedUser.SelectedValue = args["AssignedToVerUser"].ToString();

                }
            }
        }

        private void ShowStatusMessage(String cssClass, String message, Boolean isVerificationMsg)
        {

            lblVerError.Text = message;
            lblVerError.CssClass = cssClass;
            pnlVerError.Update();

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
            if (VerSelectedUserId > 0)
            {
                verificationQueueFilters.VerAssignToUserID = VerSelectedUserId;
            }

            #region Custom Attributes

            if (!NodeLable.IsNullOrEmpty())
            {
                verificationQueueFilters.NodeLabel = NodeLable;
            }
            if (!CustomDataXML.IsNullOrEmpty())
            {
                verificationQueueFilters.CustomFields = CustomDataXML;
            }

            #region UAT 1055 Changes

            if (!String.IsNullOrEmpty(NodeIds))
            {
                verificationQueueFilters.NodeIds = this.NodeIds;
            }

            if (!String.IsNullOrEmpty(DPMIds))
            {
                verificationQueueFilters.SelectedDPMIds = DPMIds;
            }

            //if (NodeId > 0)
            //{
            //    verificationQueueFilters.NodeId = NodeId;
            //}
            ////if (DPM_Id > 0)
            ////{
            ////    verificationQueueFilters.DPM_Id = DPM_Id;
            ////}

            #endregion
            #endregion

            verificationQueueFilters.FilterColumns = ViewState["FilterColumns"].IsNullOrEmpty() ? null : (List<String>)ViewState["FilterColumns"];
            verificationQueueFilters.FilterOperators = ViewState["FilterOperators"].IsNullOrEmpty() ? null : (List<String>)ViewState["FilterOperators"];
            verificationQueueFilters.FilterValues = ViewState["FilterValues"].IsNullOrEmpty() ? null : (ArrayList)ViewState["FilterValues"];
            verificationQueueFilters.FilterTypes = ViewState["FilterTypes"].IsNullOrEmpty() ? null : (List<String>)ViewState["FilterTypes"];

            verificationQueueFilters.ShowIncompleteItems = ShowIcompleteItems;
            verificationQueueFilters.ShowRushOrders = ShowOnlyRushOrders;

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
                    chkSelectAllResults.Visible = true;

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
                    chkShowIncompleteItems.Checked = verificationQueueFilters.ShowIncompleteItems;
                    chkShowRushOrders.Checked = verificationQueueFilters.ShowRushOrders;
                    if (verificationQueueFilters.UserGroupId > 0)
                    {
                        SelectedUserGroupId = verificationQueueFilters.UserGroupId;
                        ddlUserGroup.SelectedValue = SelectedUserGroupId.ToString();
                    }
                    //grdVerificationItemData.Rebind();
                    if (verificationQueueFilters.VerAssignToUserID > 0 && pnlVerShowUsers.Visible == true)
                    {
                        VerSelectedUserId = verificationQueueFilters.VerAssignToUserID;
                        ddlVerSelectedUser.SelectedValue = VerSelectedUserId.ToString();
                    }
                }
                ViewState["FilterColumns"] = CurrentViewContext.VerificationViewContract.FilterColumns = verificationQueueFilters.FilterColumns;
                ViewState["FilterOperators"] = CurrentViewContext.VerificationViewContract.FilterOperators = verificationQueueFilters.FilterOperators;
                ViewState["FilterValues"] = CurrentViewContext.VerificationViewContract.FilterValues = verificationQueueFilters.FilterValues;
                ViewState["FilterTypes"] = CurrentViewContext.VerificationViewContract.FilterTypes = verificationQueueFilters.FilterTypes;
                SetFilterValues();

                #region Custom Attributes
                // uAT 1055
                //ucCustomAttributeLoaderNodeSearch.NodeId = verificationQueueFilters.NodeId.IsNotNull() ? Convert.ToInt32(verificationQueueFilters.NodeId) : 0;
                ucCustomAttributeLoaderNodeSearch.NodeIds = !verificationQueueFilters.NodeIds.IsNullOrEmpty() ? verificationQueueFilters.NodeIds : String.Empty;

                ucCustomAttributeLoaderNodeSearch.TenantId = CurrentViewContext.SelectedTenantId = verificationQueueFilters.TenantId;
                ucCustomAttributeLoaderNodeSearch.previousValues = verificationQueueFilters.CustomFields;
                ucCustomAttributeLoaderNodeSearch.nodeLable = verificationQueueFilters.NodeLabel;
                // UAT 1055
                ////ucCustomAttributeLoader.DPM_ID = Convert.ToInt32(verificationQueueFilters.DPM_Id);
                ucCustomAttributeLoaderNodeSearch.DPM_ID = verificationQueueFilters.SelectedDPMIds;
                ucCustomAttributeLoaderNodeSearch.ScreenType = "CommonScreen";

                #endregion


                SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.SESSION_KEY_VERIFICATIONQUEUE, null);
            }
        }

        private void SetVerGridFilters()
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

        protected void chkSelectAllResults_CheckedChanged(object sender, EventArgs e)
        {
            if (CurrentViewContext.VirtualPageCount > 0)
            {
                bool needToCheckboxChecked = false;
                bool needToMarkHeadCheckboxChecked = true;

                if (((CheckBox)sender).Checked)
                {
                    SetVerGridFilters();
                    VerificationGridCustomPaging.FilterColumns = VerificationViewContract.FilterColumns;
                    VerificationGridCustomPaging.FilterOperators = VerificationViewContract.FilterOperators;
                    VerificationGridCustomPaging.FilterValues = VerificationViewContract.FilterValues;
                    VerificationGridCustomPaging.FilterTypes = VerificationViewContract.FilterTypes;
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

                    foreach (GridDataItem item in grdVerificationItemData.Items)
                    {
                        String itemDataId = item.OwnerTableView.DataKeyValues[item.ItemIndex]["ApplicantComplianceItemID"].ToString();
                        CheckBox checkBox = ((CheckBox)item.FindControl("chkSelectItem"));

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
                    foreach (GridDataItem item in grdVerificationItemData.Items)
                    {
                        CheckBox checkBox = ((CheckBox)item.FindControl("chkSelectItem"));
                        checkBox.Checked = needToCheckboxChecked;
                    }
                }


                GridHeaderItem headerItem = grdVerificationItemData.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
                CheckBox headerCheckBox = ((CheckBox)headerItem.FindControl("chkSelectAll"));
                headerCheckBox.Checked = needToMarkHeadCheckboxChecked;
                GridFooterItem footerItem = grdVerificationItemData.MasterTableView.GetItems(GridItemType.Footer)[0] as GridFooterItem;
                CheckBox footerCheckBox = ((CheckBox)footerItem.FindControl("chkSelectAllFooter"));
                footerCheckBox.Checked = needToMarkHeadCheckboxChecked;
            }
        }

        protected void btnAutomaticItemAssigning_Click(object sender, EventArgs e)
        {
            try
            {
                var result = Presenter.GetAppConfiguration(AppConsts.Automatic_Items_Assign_To_Admin_In_Progress);
                if (result.Item1)
                {
                    SetVerGridFilters();
                    VerificationGridCustomPaging.FilterColumns = VerificationViewContract.FilterColumns;
                    VerificationGridCustomPaging.FilterOperators = VerificationViewContract.FilterOperators;
                    VerificationGridCustomPaging.FilterValues = VerificationViewContract.FilterValues;
                    VerificationGridCustomPaging.FilterTypes = VerificationViewContract.FilterTypes;
                    Presenter.ActivateAutomaticAssignItemToUserProcessForAllTenants();
                    base.ShowSuccessMessage(AppConsts.Automatic_Items_Assign_To_Admin_Initiation_Process_Message);
                }
                else
                {
                    if (String.Equals(result.Item2, AppConsts.Automatic_Items_Assign_To_Admin_Error_Message))
                    {
                        base.ShowErrorMessage(AppConsts.Automatic_Items_Assign_To_Admin_Error_Message);
                    }
                    else
                    {
                        base.ShowInfoMessage(AppConsts.Automatic_Items_Assign_To_Admin_In_Progress_Message);
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

        #region Public Methods

        #endregion

        #endregion

    }
}

