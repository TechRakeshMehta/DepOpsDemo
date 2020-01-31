#region Namespaces

#region System Defined Namespaces

using System;
using System.Linq;
using Microsoft.Practices.ObjectBuilder;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Collections;

#endregion

#region User Defined Namespaces

using INTSOF.Utils;
using Telerik.Web.UI;
using CoreWeb.Shell;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceManagement;
using CoreWeb.IntsofSecurityModel;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.UI.Contract.ComplianceOperation;

#endregion

#endregion

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class SearchControl : BaseUserControl, ISearchControlView
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private SearchControlPresenter _presenter = new SearchControlPresenter();
        private String _viewType;
        Int32 tenantId = 0;
        private CustomPagingArgsContract _itemDataGridCustomPaging = null;
        private SearchItemDataContract _searchItemDataContract = null;
        private SearchItemDataContract _searchQuery = null;
        private String _SelectedCategoryId;
        private String _SelectedPkgId; //UAT-4136
        private String _SelectedUserGroupid;
        public Boolean needSendMessageFunctionality = false;

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        public SearchControlPresenter Presenter
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

        public Int32? AssignedToUserId
        {
            get
            {
                if (ViewState["AssignedToUserId"] != null)
                    return Convert.ToInt32(ViewState["AssignedToUserId"]);
                return null;
            }
            set
            {
                if (value != null)
                    (grdItemData.MasterTableView.GetColumn("AssignedUserName") as GridBoundColumn).Visible = false;

                ViewState["AssignedToUserId"] = value;
            }
        }

        bool ISearchControlView.IsAdminLoggedIn
        {
            get;
            set;
        }

        public List<Entity.ClientEntity.CompliancePackage> lstCompliancePackage2 
        { 
            get; 
            set; 
        }

        public Int32 SelectedTenantId
        {
            get
            {
                if (String.IsNullOrEmpty(ddlTenantName.SelectedValue))
                {
                    return 0;
                }
                else
                {
                    ViewState["SelectedTenantId"] = Convert.ToInt32(ddlTenantName.SelectedValue);
                    return Convert.ToInt32(ViewState["SelectedTenantId"]);
                }
            }
            set
            {
                ViewState["SelectedTenantId"] = value;
                ddlTenantName.SelectedValue = Convert.ToString(ViewState["SelectedTenantId"]);
            }
        }

        public Int32 SelectedPackageId
        {
            get;
            set;
        }

        public Int32 SelectedCategoryId
        {
            get;
            //{
            //if (String.IsNullOrEmpty(ddlCategory.SelectedValue))
            //    return 0;
            //return Convert.ToInt32(ddlCategory.SelectedValue);
            //}
            set;
            //   {
            //ddlCategory.SelectedValue = value.ToString();
            //  }
        }

        public Int32 SelectedProgramStudyId
        {
            get
            {
                if (String.IsNullOrEmpty(ddlProgram.SelectedValue))
                    return 0;
                return Convert.ToInt32(ddlProgram.SelectedValue);
            }
            set
            {
                ddlProgram.SelectedValue = value.ToString();
            }
        }

        public List<Int32> SelectedItemComplianceStatusId
        {
            get
            {
                List<Int32> selectedIds = new List<Int32>();
                for (Int32 i = 0; i < chkItemStatus.Items.Count; i++)
                {
                    if (chkItemStatus.Items[i].Selected)
                    {
                        selectedIds.Add(Convert.ToInt32(chkItemStatus.Items[i].Value));
                    }
                }
                return selectedIds;
            }
            set
            {
                for (Int32 i = 0; i < chkItemStatus.Items.Count; i++)
                {
                    chkItemStatus.Items[i].Selected = value.Contains(Convert.ToInt32(chkItemStatus.Items[i].Value));
                }

            }

        }

        public Int32 TenantId
        {
            get
            {
                if (tenantId == 0)
                {
                    //tenantId = Presenter.GetTenantId();
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

        public List<Tenant> lstTenant
        {
            get;
            set;
        }

        public List<Entity.ClientEntity.ComplaincePackageDetails> lstCompliancePackage
        {
            get;
            set;
        }

        public List<Entity.ClientEntity.ComplianceCategory> lstComplianceCategory
        {
            get;
            set;
        }

        public List<Entity.ClientEntity.lkpItemComplianceStatu> lstItemComplianceStatus
        {
            get;
            set;
        }

        //public List<Entity.AdminProgramStudy> lstAdminProgramStudy
        //{
        //    get;
        //    set;
        //}

        public string ApplicantFirstName
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

        public string ApplicantLastName
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

        public DateTime? DateOfBirth
        {
            get
            {
                return dpkrDOB.SelectedDate;
            }
            set
            {
                dpkrDOB.SelectedDate = value;
            }
        }

        //UAT-5063
        public DateTime? SubmissionDateFrom
        {
            get
            {
                return dpkrSubmissionDateFrom.SelectedDate;
            }
            set
            {
                dpkrSubmissionDateFrom.SelectedDate = value;
            }
        }

        public DateTime? SubmissionDateTo
        {
            get
            {
                return dpkrSubmissionDateTo.SelectedDate;
            }
            set
            {
                dpkrSubmissionDateTo.SelectedDate = value;
            }
        }

        public string ItemLabel
        {
            get
            {
                return txtItemLabel.Text;
            }
            set
            {
                txtItemLabel.Text = value;
            }
        }

        public string AssignedUser
        {
            get
            {
                return txtAssignedUser.Text;
            }
            set
            {
                txtAssignedUser.Text = value;
            }
        }

        public List<ItemDataSearchContract> ItemData
        {
            get;
            set;
        }

        public Int32 CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        public ISearchControlView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public WorkQueueType WorkQueue
        {
            get
            {
                if (ViewState["WorkQueue"] != null)
                    return (WorkQueueType)(ViewState["WorkQueue"]);
                return WorkQueueType.DataItemSearch;
            }
            set
            {
                ViewState["WorkQueue"] = value;
            }
        }

        public Dictionary<String, String> Args { get; set; }

        public String ActionType
        {
            get;
            set;
        }

        public SearchItemDataContract ViewStateSearchData
        {
            get
            {
                if (_searchQuery.IsNullOrEmpty() && ViewState["SearchQuery"] != null)
                {
                    var serializer = new XmlSerializer(typeof(SearchItemDataContract));
                    TextReader reader = new StringReader(Convert.ToString(ViewState["SearchQuery"]));
                    _searchQuery = (SearchItemDataContract)serializer.Deserialize(reader);
                }
                return _searchQuery;
                //return (SearchItemDataContract)ViewState["SearchQuery"];
            }
            set
            {
                var serializer = new XmlSerializer(typeof(SearchItemDataContract));
                var sb = new StringBuilder();
                using (TextWriter writer = new StringWriter(sb))
                {
                    serializer.Serialize(writer, value);
                }
                ViewState["SearchQuery"] = sb.ToString();
            }
        }

        #region UAT-422
        public List<lkpArchiveState> lstArchiveState
        {
            set
            {
                rbSubscriptionState.DataSource = value.OrderBy(x => x.AS_Code);
                rbSubscriptionState.DataBind();
                rbSubscriptionState.SelectedValue = ArchiveState.Active.GetStringValue();//ArchiveState.All.GetStringValue();
            }
        }

        public List<String> SelectedArchiveStateCode
        {
            get
            {
                if (!rbSubscriptionState.SelectedValue.IsNullOrEmpty())
                {
                    List<String> selectedCodes = new List<String>();
                    if (rbSubscriptionState.SelectedValue.Equals(ArchiveState.All.GetStringValue()))
                    {
                        return null;
                    }
                    else
                    {
                        selectedCodes.Add(rbSubscriptionState.SelectedValue);
                    }
                    return selectedCodes;
                }
                else
                    return null;
            }
            set
            {
                rbSubscriptionState.SelectedValue = value.FirstOrDefault();
            }
        }

        #endregion

        #region UAT-3518
  
        String ISearchControlView.SelectedExpiryStateCode
        {
            get
            {
                if (!rbSubscriptionExpiryState.SelectedValue.IsNullOrEmpty())
                {
                    String selectedCodes = "";
                    if (rbSubscriptionExpiryState.SelectedValue.Equals("ZZ"))
                    {
                        return null;
                    }
                    else
                    {
                        selectedCodes= rbSubscriptionExpiryState.SelectedValue;
                    }
                    return selectedCodes;
                }
                else
                    return null;
            }
            set
            {
                rbSubscriptionExpiryState.SelectedValue = value;
            }
        }
        #endregion

        String ISearchControlView.SelectedSystemStatus
        {
            get
            {
                if (!rbSystemStatus.SelectedValue.IsNullOrEmpty())
                {
                    String selectedCode = "";
                    if (rbSystemStatus.SelectedValue.Equals("AA"))
                    {
                        return null;
                    }
                    else
                    {
                        selectedCode = rbSystemStatus.SelectedValue;
                    }
                    return selectedCode;
                }
                else
                    return null;
            }
            set
            {
                rbSystemStatus.SelectedValue = value;
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
        #endregion

        #region Custom Paging Properties

        /// <summary>
        /// CurrentPageIndex
        /// </summary>
        /// <value> Gets or sets the value for CurrentPageIndex.</value>
        public Int32 CurrentPageIndex
        {
            get
            {
                return grdItemData.MasterTableView.CurrentPageIndex + 1;
            }
            set
            {
                grdItemData.MasterTableView.CurrentPageIndex = value == 0 ? 0 : value - 1;
            }
        }

        /// <summary>
        /// PageSize
        /// </summary>
        /// <value> Gets the value for PageSize.</value>
        public Int32 PageSize
        {
            get
            {
                // Maximum 100 record allowed from DB. 
                //return grdItemData.PageSize > 100 ? 100 : grdItemData.PageSize;
                return grdItemData.PageSize;
            }
        }

        /// <summary>
        /// VirtualPageCount
        /// </summary>
        /// <value> Sets the value for VirtualPageCount.</value>
        public Int32 VirtualPageCount
        {
            get
            {
                return grdItemData.MasterTableView.VirtualItemCount;
            }
            set
            {
                grdItemData.VirtualItemCount = value;
                grdItemData.MasterTableView.VirtualItemCount = value;
            }

        }

        /// <summary>
        /// get object of shared class of custom paging
        /// </summary>
        public CustomPagingArgsContract ItemDataGridCustomPaging
        {
            get
            {
                if (_itemDataGridCustomPaging.IsNull())
                {
                    _itemDataGridCustomPaging = new CustomPagingArgsContract();
                }
                return _itemDataGridCustomPaging;
            }
        }

        /// <summary>
        /// View Contract
        /// </summary>
        public SearchItemDataContract SearchItemDataContract
        {
            get
            {
                if (_searchItemDataContract.IsNull())
                {
                    _searchItemDataContract = new SearchItemDataContract();
                }
                return _searchItemDataContract;
            }
        }


        #endregion

        public Dictionary<Int32, String> AssignOrganizationUserIds
        {
            get
            {
                if (!ViewState["SelectedApplicants"].IsNull())
                {
                    return ViewState["SelectedApplicants"] as Dictionary<Int32, String>;
                }

                return new Dictionary<Int32, String>();
            }
            set
            {
                ViewState["SelectedApplicants"] = value;
            }
        }

        #endregion

        #region Custom Attributes
        //UAT-1055
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

        public String CustomDataXML
        {
            get
            {
                //UAT-1055
                // return ucCustomAttributeLoader.GetCustomDataXML();
                return ucCustomAttributeLoaderNodeSearch.GetCustomDataXML();
            }
        }

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

        public String DPM_Ids
        {
            get
            {
                return ucCustomAttributeLoaderNodeSearch.DPM_ID;
            }
            set
            {
                ucCustomAttributeLoaderNodeSearch.DPM_ID = value;
            }
        }


        public String IsTreeHierarchyChanged
        {
            get
            {
                return ucCustomAttributeLoaderNodeSearch.IsTreeHierachyChanged;
            }
            set
            {
                ucCustomAttributeLoaderNodeSearch.IsTreeHierachyChanged = value;
            }
        }


        public String NodeLable
        {
            get
            {
                //UAT-1055
                //return ucCustomAttributeLoader.nodeLable;
                return ucCustomAttributeLoaderNodeSearch.nodeLable;
            }
            set
            {
                //UAT-1055
                // ucCustomAttributeLoader.nodeLable = value;
                ucCustomAttributeLoaderNodeSearch.nodeLable = value;
            }
        }

        #endregion

        #region Private Properties

        String ISearchControlView.SelectedCategoryIds
        {
            get
            {
                foreach (RadComboBoxItem item in ddlCategory.CheckedItems)
                {
                    _SelectedCategoryId = _SelectedCategoryId + item.Value + ',';

                }
                return _SelectedCategoryId;
            }
            set
            {
                _SelectedCategoryId = value;
            }
        }

        List<UserGroup> ISearchControlView.lstUserGroup
        {
            get;
            set;
        }

        String ISearchControlView.SelectedUserGroupIDs
        {
            get
            {
                foreach (RadComboBoxItem item in ddlUserGroup.CheckedItems)
                {
                    _SelectedUserGroupid = _SelectedUserGroupid + item.Value + ',';
                }
                return _SelectedUserGroupid;
            }
            set
            {
                _SelectedUserGroupid = value;
            }
        }

        //UAT-3519
        /// <summary>
        /// Get or Set Selected Package ids.
        /// </summary>
        public List<Int32> SelectedPackageIds
        {
            get;
            set;
        }

        String ISearchControlView.SelectedPkgIds
        {
            get
            {
                if (!ViewState["SelectedPkgIds"].IsNullOrEmpty())
                    return Convert.ToString(ViewState["SelectedPkgIds"]);
                return String.Empty;
            }
            set
            {
                _SelectedPkgId = value;
                ViewState["SelectedPkgIds"] = value;
            }
        }

        #endregion

        #endregion

        #region Events

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
            (fsucCmdBar1 as CoreWeb.Shell.Views.CommandBar).SaveButton.ToolTip = "Click to search orders per the criteria entered above";
            (fsucCmdBar1 as CoreWeb.Shell.Views.CommandBar).CancelButton.ToolTip = "Click to cancel. Any data entered will not be saved";
            (fsucCmdBar1 as CoreWeb.Shell.Views.CommandBar).SubmitButton.ToolTip = "Click to remove all values entered in the search criteria above";
            Presenter.IsAdminLoggedIn();
            if (WorkQueue == WorkQueueType.DataItemSearch)
                needSendMessageFunctionality = true;
            else
                divSelectAllResults.Visible = false;
            
            grdItemData.AllowFilteringByColumn = false; //UAT-5063

            //UAT-4056 :- Category Search filtering updates
            if (CurrentViewContext.IsTreeHierarchyChanged=="true")
            {
                BindPackage();
                IsTreeHierarchyChanged ="false";
            }
            if (!this.IsPostBack)
            {
                //Set MinDate and MaxDate for DOB
                dpkrDOB.MinDate = Convert.ToDateTime("01-01-1900");
                dpkrDOB.MaxDate = DateTime.Now;

                if (!Request.QueryString["args"].IsNull())
                {
                    Args = new Dictionary<string, string>();
                    Args.ToDecryptedQueryString(Request.QueryString["args"]);
                    GetSessionValues();
                }
                else
                {
                    grdItemData.Visible = false;
                    chkSelectAllResults.Visible = false;
                }

                Presenter.OnViewInitialized();
                ddlTenantName.DataSource = CurrentViewContext.lstTenant;
                ddlTenantName.DataBind();
                Init();
                fsucCmdBar1.ClearButton.Style.Add("display", "none");
            }
            Presenter.OnViewLoaded();
            HideShowControlsForGranularPermission();
            if (!ddlTenantName.SelectedValue.IsNullOrEmpty())
            {
                CurrentViewContext.SelectedTenantId = Convert.ToInt32(ddlTenantName.SelectedValue);
                ucCustomAttributeLoaderNodeSearch.TenantId = CurrentViewContext.SelectedTenantId;
            }
        }

        private void Init()
        {
            //UAT-789
            //ddlTenantName.DataSource = CurrentViewContext.lstTenant;
            //ddlTenantName.DataBind();
            if (Presenter.IsDefaultTenant || Presenter.IsThirdPartyTenant)
            {
                ddlTenantName.Enabled = true;
                if (Args != null && Args.ContainsKey("SelectedTenantId"))
                    CurrentViewContext.SelectedTenantId = Convert.ToInt32(Args["SelectedTenantId"]);
            }
            else
            {
                CurrentViewContext.SelectedTenantId = CurrentViewContext.TenantId;
                //UAT-1055
                //ucCustomAttributeLoader.TenantId = Convert.ToInt32(CurrentViewContext.TenantId);
                ucCustomAttributeLoaderNodeSearch.TenantId = Convert.ToInt32(CurrentViewContext.TenantId);
                rbSubscriptionState.Visible = true;
                rbSubscriptionExpiryState.Visible = true;
                Presenter.GetArchiveStateList();
            }

            BindPackage();
            BindUserGroups();
            BindItemComplianceStatus();
            BindAdminProgramStudy();

            if (Args != null && Args.ContainsKey("ApplicantFirstName"))
                CurrentViewContext.ApplicantFirstName = Args["ApplicantFirstName"];

            if (Args != null && Args.ContainsKey("ApplicantLastName"))
                CurrentViewContext.ApplicantLastName = Args["ApplicantLastName"];

            DateTime dob;
            if (Args != null && Args.ContainsKey("DateOfBirth") && DateTime.TryParse(Args["DateOfBirth"], out dob))
                CurrentViewContext.DateOfBirth = dob;

            //UAT-5063
            DateTime submissionDateFrom;
            if (Args != null && Args.ContainsKey("SubmissionDateFrom") && DateTime.TryParse(Args["SubmissionDateFrom"], out submissionDateFrom))
                CurrentViewContext.SubmissionDateFrom = submissionDateFrom;

            DateTime submissionDateTo;
            if (Args != null && Args.ContainsKey("SubmissionDateTo") && DateTime.TryParse(Args["SubmissionDateTo"], out submissionDateTo))
                CurrentViewContext.SubmissionDateTo = submissionDateTo;

            if (Args != null && Args.ContainsKey("AssignedUser"))
                CurrentViewContext.AssignedUser = Args["AssignedUser"];

            if (Args != null && Args.ContainsKey("ItemLabel"))
                CurrentViewContext.ItemLabel = Args["ItemLabel"];

            if (Args != null && Args.ContainsKey("SelectedItemComplianceStatusId") && !String.IsNullOrEmpty(Args["SelectedItemComplianceStatusId"]))
            {
                SelectedItemComplianceStatusId = new List<int>(Args["SelectedItemComplianceStatusId"].Split(',').Select(int.Parse));
            }
            ActionType = ViewMode.Search.ToString();

            if (Args != null && Args.ContainsKey("ProgramId"))
                CurrentViewContext.SelectedProgramStudyId = Convert.ToInt32(Args["ProgramId"]);

        }

        #endregion

        #region Button Events

        protected void CmdBarSearch_Click(object sender, EventArgs e)
        {
            //Presenter.PerformSearch();
            //grdItemData.DataSource = CurrentViewContext.ItemData;
            //grdItemData.DataBind();
            grdItemData.Visible = true;
            chkSelectAllResults.Visible = true;
            if ((!CurrentViewContext.TenantId.IsNullOrEmpty() && CurrentViewContext.TenantId > 0) || !ddlTenantName.SelectedValue.IsNullOrEmpty())
            {
                chkSelectAllResults.Checked = false;
                CurrentViewContext.AssignOrganizationUserIds = new Dictionary<int, string>();

                ActionType = ViewMode.Search.ToString();
                ClearViewStatesForFilter();
                FilterGridColumn();
                grdItemData.Rebind();
            }
        }

        protected void CmdBarReset_Click(object sender, EventArgs e)
        {
            CurrentViewContext.AssignOrganizationUserIds = new Dictionary<Int32, String>();
            Presenter.GetTenants();
            ddlCategory.ClearSelection();
            ddlUserGroup.ClearSelection();
            CurrentViewContext.SelectedProgramStudyId = 0;
            CurrentViewContext.SelectedTenantId = 0;
            CurrentViewContext.SelectedPackageId = 0;
            CurrentViewContext.SelectedItemComplianceStatusId = new List<int>();
            CurrentViewContext.DPM_Ids = String.Empty;
            Init();
            CurrentViewContext.SelectedPackageId = 0;
            //START - UAT-4136
            CurrentViewContext.SelectedPkgIds = String.Empty; 
            ddlPackage.ClearSelection();
            ddlPackage.ClearCheckedItems();
            //END - UAT-4136
            CurrentViewContext.SelectedCategoryId = 0;
            CurrentViewContext.SelectedCategoryIds = String.Empty;
            CurrentViewContext.SelectedUserGroupIDs = String.Empty;
            txtFirstName.Text = String.Empty;
            txtLastName.Text = String.Empty;
            dpkrDOB.SelectedDate = null;
            //UAT-5063
            dpkrSubmissionDateFrom.SelectedDate = null;
            dpkrSubmissionDateTo.SelectedDate = null;
            txtAssignedUser.Text = String.Empty;
            rbSystemStatus.SelectedValue = "AA";
            txtItemLabel.Text = String.Empty;
            //UAT-1055
            //ucCustomAttributeLoader.Reset();
            if (Presenter.IsDefaultTenant)
            {
                ucCustomAttributeLoaderNodeSearch.Reset();
            }
            else
            {
                ucCustomAttributeLoaderNodeSearch.ResetControlData(true);
            }
            //rbSubscriptionState.Visible = false;
            if (Presenter.IsDefaultTenant)
            {
                rbSubscriptionState.Visible = false;
                rbSubscriptionExpiryState.Visible = false;
            }
            else
            {
                rbSubscriptionState.Visible = true;
                rbSubscriptionExpiryState.Visible = true;
            }

            fsucCmdBar1.ClearButton.Style.Add("display", "none");

            VirtualPageCount = 0;
            ViewState["SortExpression"] = null;
            ViewState["SortDirection"] = null;
            grdItemData.MasterTableView.SortExpressions.Clear();
            //Presenter.PerformSearch();
            //grdItemData.DataSource = CurrentViewContext.ItemData;
            //grdItemData.DataBind();
            ActionType = ViewMode.Search.ToString();
            ClearViewStatesForFilter();
            FilterGridColumn();
            grdItemData.Rebind();

            chkSelectAllResults.Checked = false;
        }

        protected void CmdBarCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(String.Format(AppConsts.DASHBOARD_PAGE_NAME));
        }

        #endregion

        #region Grid Events

        protected void grdItemData_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            ItemDataGridCustomPaging.CurrentPageIndex = CurrentPageIndex;
            ItemDataGridCustomPaging.PageSize = PageSize;
            ItemDataGridCustomPaging.FilterColumns = SearchItemDataContract.FilterColumns;
            ItemDataGridCustomPaging.FilterOperators = SearchItemDataContract.FilterOperators;
            ItemDataGridCustomPaging.FilterValues = SearchItemDataContract.FilterValues;
            ItemDataGridCustomPaging.FilterTypes = SearchItemDataContract.FilterTypes;
            if (CurrentViewContext.ViewStateSearchData == null)
            {
                ActionType = ViewMode.Search.ToString();
            }
            Presenter.PerformSearch();
            grdItemData.DataSource = CurrentViewContext.ItemData;
            SetFilterValues();
            DisplayMessageSentStatus();
        }

        protected void grdItemData_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName.ToLower() == "verificationdetails")
            {
                if (e.Item.IsNotNull() && e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
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
                    Dictionary<String, String> queryString = new Dictionary<String, String>();

                    SetSessionValues();//SearchItemDataContract
                    Int32 _itemDataId = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ApplicantComplianceItemID"]);
                    Int32 _applicantId = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ApplicantId"]);

                    Int32 _selectedCategoryId = Convert.ToInt32((e.Item.FindControl("hdfCatId") as HiddenField).Value);
                    Int32 _selectedPackageSubscriptionId = Convert.ToInt32((e.Item.FindControl("hdfPackSubscriptionId") as HiddenField).Value);

                    //UAT-4136
                    SelectedPackageId = Convert.ToInt32((e.Item.FindControl("hdnPkgId") as HiddenField).Value);

                    queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "TenantId", Presenter.ClientId.ToString() },
                                                                    { "Child", ChildControls.VerificationDetailsNew},
                                                                    { "ItemDataId",Convert.ToString( _itemDataId)},
                                                                    {"WorkQueueType",WorkQueue.ToString()},
                                                                    {"PackageId",SelectedPackageId.ToString()},
                                                                    {"CategoryId",SelectedCategoryId.ToString()},
                                                                    {"SelectedProgramStudyId",SelectedProgramStudyId.ToString()},
                                                                    {"ApplicantFirstName",ApplicantFirstName},
                                                                    {"ApplicantLastName",ApplicantLastName},
                                                                    {"DateOfBirth",DateOfBirth.ToString()},
                                                                    {"ItemLabel",ItemLabel},
                                                                    {"SelectedItemComplianceStatusId",String.Join(",", SelectedItemComplianceStatusId.ToArray()) },
                                                                     {"SelectedPackageSubscriptionId",Convert.ToString( _selectedPackageSubscriptionId)},
                                                                    {"SelectedComplianceCategoryId",Convert.ToString( _selectedCategoryId)},
                                                                    {"ShowOnlyRushOrders","false"},
                                                                    {"ApplicantId",Convert.ToString(_applicantId)},
                                                                    {"SubmissionDateFrom",SubmissionDateFrom.ToString()},
                                                                    {"SubmissionDateTo",SubmissionDateTo.ToString()},
                                                                    {"AssignedUser",AssignedUser},
                                                                 };

                    string url = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                    Response.Redirect(url, true);
                }
            }
            else if (e.CommandName.ToLower() == "ViewDetail") // Will be removed after new screen
            {
                if (e.Item.IsNotNull() && e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
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
                    Dictionary<String, String> queryString = new Dictionary<String, String>();
                    SetSessionValues();
                    Int32 _itemDataId = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ApplicantComplianceItemID"]);
                    Int32 _applicantId = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ApplicantId"]);

                    //String itemDataId = ;
                    queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "SelectedTenantId", Presenter.ClientId.ToString() },
                                                                    { "Child", ChildControls.VerificationDetails},
                                                                    { "ItemDataId",Convert.ToString( _itemDataId)},
                                                                    {"WorkQueueType",WorkQueue.ToString()},
                                                                    {"PackageId",SelectedPackageId.ToString()},
                                                                    {"CategoryId",SelectedCategoryId.ToString()},
                                                                    {"SelectedProgramStudyId",SelectedProgramStudyId.ToString()},
                                                                    {"ApplicantFirstName",ApplicantFirstName},
                                                                    {"ApplicantLastName",ApplicantLastName},
                                                                    {"DateOfBirth",DateOfBirth.ToString()},
                                                                    {"ItemLabel",ItemLabel},
                                                                    {"SelectedItemComplianceStatusId",String.Join(",", SelectedItemComplianceStatusId.ToArray()) },
                                                                    {"ApplicantId",Convert.ToString(_applicantId)}
                                                                    
                                                                 };
                    string url = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                    Response.Redirect(url, true);
                }
            }
            // Hide filter when exportig to pdf or word
            if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToPdfCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToWordCommandName
                || e.CommandName == Telerik.Web.UI.RadGrid.ExportToExcelCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToCsvCommandName)
            {
                base.ConfigureExport(grdItemData);

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
                        grdItemData.MasterTableView.GetColumn("CustomAttributesTemp").Display = true;
                        grdItemData.MasterTableView.GetColumn("UserGroupsTemp").Display = true;
                    }
                    else
                    {
                        grdItemData.MasterTableView.GetColumn("CustomAttributesTemp").Display = false;
                        grdItemData.MasterTableView.GetColumn("UserGroupsTemp").Display = false;
                    }
                }
            }
            if (e.CommandName == "Cancel")
            {
                grdItemData.MasterTableView.GetColumn("CustomAttributesTemp").Display = false;
                grdItemData.MasterTableView.GetColumn("UserGroupsTemp").Display = false;
            }
            #endregion

            #region For Filter command

            else if (e.CommandName == RadGrid.FilterCommandName)
            {
                /*UAT-2904*/
                foreach (GridColumn item in grdItemData.MasterTableView.Columns)
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
                ViewState["FilterPair"] = filter;
            }
            FilterGridColumn();
            ActionType = ViewMode.Queue.ToString();

            #endregion

        }

        private void SetSessionValues()
        {
            SearchItemDataContract searchItemData = new SearchItemDataContract();

            #region Custom Attributes
            //UAt-1055
            //if (NodeId > 0)
            //{
            //    searchItemData.NodeId = NodeId;
            //}
            if (!NodeIds.IsNullOrEmpty())
            {
                searchItemData.NodeIds = NodeIds;
            }
            if (!NodeLable.IsNullOrEmpty())
            {
                searchItemData.NodeLabel = NodeLable;
            }
            if (!CustomDataXML.IsNullOrEmpty())
            {
                searchItemData.CustomFields = CustomDataXML;
            }
            //UAT-1055
            //if (DPM_Id > 0)
            //{
            //    searchItemData.DPM_Id = DPM_Id;
            //}
            if (!DPM_Ids.IsNullOrEmpty())
            {
                searchItemData.SelectedDPMIds = DPM_Ids;
            }


            #endregion

            searchItemData.ClientID = CurrentViewContext.SelectedTenantId;
            searchItemData.FilterColumns = (List<String>)ViewState["FilterColumns"];
            searchItemData.FilterOperators = (List<String>)ViewState["FilterOperators"];
            searchItemData.FilterValues = (ArrayList)ViewState["FilterValues"];
            //UAT-812
            searchItemData.FilterTypes = (List<String>)ViewState["FilterTypes"];
            #region UAT-422
            searchItemData.LstArchiveState = CurrentViewContext.SelectedArchiveStateCode;
            #endregion
            #region UAT-3518
            searchItemData.SelectedExpiryStateCode = CurrentViewContext.SelectedExpiryStateCode;
            #endregion
            //UAT-5063
            searchItemData.SelectedSystemStatus = CurrentViewContext.SelectedSystemStatus;

            //UAT 1680
            searchItemData.CategoryIDs = CurrentViewContext.SelectedCategoryIds.IsNullOrEmpty() ? String.Empty : CurrentViewContext.SelectedCategoryIds;
            //UAT 1681 1686
            searchItemData.SelectedUserGroupIDs = CurrentViewContext.SelectedUserGroupIDs.IsNullOrEmpty() ? String.Empty : CurrentViewContext.SelectedUserGroupIDs;
            //UAT-4136
            searchItemData.PackageIDs = CurrentViewContext.SelectedPkgIds.IsNullOrEmpty() ? String.Empty : CurrentViewContext.SelectedPkgIds;


            SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.SESSION_KEY_SEARCHQUEUE, searchItemData);
        }

        private void GetSessionValues()
        {
            SearchItemDataContract searchItemData = (SearchItemDataContract)SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_KEY_SEARCHQUEUE);
            if (!searchItemData.IsNullOrEmpty())
            {
                #region Custom Attributes
                //UAt-1055
                //ucCustomAttributeLoader.NodeId = searchItemData.NodeId.IsNotNull() ? Convert.ToInt32(searchItemData.NodeId) : 0;

                //ucCustomAttributeLoader.TenantId = CurrentViewContext.SelectedTenantId = searchItemData.ClientID;
                //ucCustomAttributeLoader.previousValues = searchItemData.CustomFields;

                //ucCustomAttributeLoader.nodeLable = searchItemData.NodeLabel;

                //ucCustomAttributeLoader.DPM_ID = Convert.ToInt32(searchItemData.DPM_Id);
                ucCustomAttributeLoaderNodeSearch.NodeIds = searchItemData.NodeIds;

                ucCustomAttributeLoaderNodeSearch.TenantId = CurrentViewContext.SelectedTenantId = searchItemData.ClientID;
                ucCustomAttributeLoaderNodeSearch.previousValues = searchItemData.CustomFields;

                ucCustomAttributeLoaderNodeSearch.nodeLable = searchItemData.NodeLabel;

                ucCustomAttributeLoaderNodeSearch.DPM_ID = searchItemData.SelectedDPMIds;
                #endregion


                ViewState["FilterColumns"] = SearchItemDataContract.FilterColumns = searchItemData.FilterColumns;
                ViewState["FilterOperators"] = SearchItemDataContract.FilterOperators = searchItemData.FilterOperators;
                ViewState["FilterValues"] = SearchItemDataContract.FilterValues = searchItemData.FilterValues;
                //UAT-812
                ViewState["FilterTypes"] = SearchItemDataContract.FilterTypes = searchItemData.FilterTypes;
                SetFilterValues();

                //UAT-422
                if (searchItemData.ClientID != 0)
                {
                    Presenter.GetArchiveStateList();
                }
                if (searchItemData.LstArchiveState.IsNotNull() && searchItemData.LstArchiveState.Count > 0)
                {
                    rbSubscriptionState.SelectedValue = searchItemData.LstArchiveState.FirstOrDefault();
                }
                else if (searchItemData.LstArchiveState.IsNull())
                {
                    rbSubscriptionState.SelectedValue = ArchiveState.All.GetStringValue();
                }

                #region UAT-3518
                rbSubscriptionExpiryState.SelectedValue = searchItemData.SelectedExpiryStateCode == null ? "ZZ" : searchItemData.SelectedExpiryStateCode;
                #endregion
                //UAT-5063
                rbSystemStatus.SelectedValue = searchItemData.SelectedSystemStatus == null ? "AA" : searchItemData.SelectedSystemStatus;

                CurrentViewContext.SelectedCategoryIds = searchItemData.CategoryIDs;
                CurrentViewContext.SelectedUserGroupIDs = searchItemData.SelectedUserGroupIDs;

                //UAT-4136
                CurrentViewContext.SelectedPkgIds = searchItemData.PackageIDs;

                SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.SESSION_KEY_SEARCHQUEUE, null);
            }
            SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.SESSION_KEY_SEARCHQUEUE, null);
        }

        public void SetFilterValues()
        {
            if (!CurrentViewContext.SearchItemDataContract.FilterColumns.IsNullOrEmpty() && CurrentViewContext.SearchItemDataContract.FilterColumns.Count > 0)
            {
                CurrentViewContext.SearchItemDataContract.FilterColumns.ForEach(x =>
                    grdItemData.Columns.FindByUniqueName(x).CurrentFilterValue = CurrentViewContext.SearchItemDataContract.FilterValues[CurrentViewContext.SearchItemDataContract.FilterColumns.IndexOf(x)].ToString()
                    );
            }
        }

        /// <summary>
        /// Grid sort expression
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdItemData_SortCommand(object sender, GridSortCommandEventArgs e)
        {
            try
            {
                if (e.NewSortOrder != GridSortOrder.None)
                {
                    ViewState["SortExpression"] = e.SortExpression;
                    ViewState["SortDirection"] = e.NewSortOrder.Equals(GridSortOrder.Descending);
                    CurrentViewContext.ItemDataGridCustomPaging.SortExpression = e.SortExpression;
                    CurrentViewContext.ItemDataGridCustomPaging.SortDirectionDescending = e.NewSortOrder.Equals(GridSortOrder.Descending);
                }
                else
                {
                    ViewState["SortExpression"] = String.Empty;
                    ViewState["SortDirection"] = false;
                    CurrentViewContext.ItemDataGridCustomPaging.SortExpression = String.Empty;
                    CurrentViewContext.ItemDataGridCustomPaging.SortDirectionDescending = false;
                }
                ActionType = ViewMode.Queue.ToString();

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

        protected void grdItemData_Init(object sender, System.EventArgs e)
        {
            grdItemData.ShowClearFiltersButton = false; //UAT-5063
            GridFilterMenu menu = grdItemData.FilterMenu;
            if (grdItemData.clearFilterMethod == null)
                grdItemData.clearFilterMethod = new WclGrid.ClearFilters(ClearViewStatesForFilter);
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

        protected void grdItemData_ItemDataBound(object sender, GridItemEventArgs e)
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
                    //UAT-1994: user group column added the the category search screen
                    if (Convert.ToString(dataItem["UserGroups"].Text).Length > 20)
                    {
                        dataItem["UserGroups"].ToolTip = dataItem["UserGroups"].Text;
                        dataItem["UserGroups"].Text = (dataItem["UserGroups"].Text).ToString().Substring(0, 20) + "...";
                    }

                    if (needSendMessageFunctionality)
                    {
                        String applicantId = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ApplicantId"].ToString();

                        if (CurrentViewContext.AssignOrganizationUserIds.IsNotNull() && CurrentViewContext.AssignOrganizationUserIds.Count > 0 &&
                               CurrentViewContext.AssignOrganizationUserIds.ContainsKey(Convert.ToInt32(applicantId)))
                        {
                            CheckBox checkBox = ((CheckBox)e.Item.FindControl("chkSelectUser"));
                            checkBox.Checked = true;
                        }
                    }
                    else
                    {
                        CheckBox checkBox = ((CheckBox)e.Item.FindControl("chkSelectUser"));
                        checkBox.Visible = false;
                    }
                }

                if (!needSendMessageFunctionality && e.Item.ItemType.Equals(GridItemType.Header))
                {
                    CheckBox checkBox = ((CheckBox)e.Item.FindControl("chkSelectAll"));
                    checkBox.Visible = false;
                }


                if (e.Item.ItemType.Equals(GridItemType.Footer))
                {
                    Int32 rowCount = grdItemData.Items.Count;
                    if (rowCount > 0)
                    {
                        Int32 checkCount = 0;
                        foreach (GridDataItem item in grdItemData.Items)
                        {
                            CheckBox checkBox = ((CheckBox)item.FindControl("chkSelectUser"));
                            if (checkBox.Checked)
                            {
                                checkCount++;
                            }
                        }
                        if (rowCount == checkCount)
                        {
                            GridHeaderItem item = grdItemData.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
                            CheckBox checkBox = ((CheckBox)item.FindControl("chkSelectAll"));
                            checkBox.Checked = true;
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

        protected void chkSelectUser_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (needSendMessageFunctionality)
                {
                    CheckBox checkBox = sender as CheckBox;
                    Boolean isChecked = false;
                    if (checkBox.IsNull())
                    {
                        return;
                    }
                    GridDataItem dataItem = (GridDataItem)checkBox.NamingContainer;
                    Dictionary<Int32, String> selectedItems = CurrentViewContext.AssignOrganizationUserIds;
                    Int32 orgUserID = (Int32)dataItem.GetDataKeyValue("ApplicantId");
                    String orgUserName = Convert.ToString(dataItem["FirstName"].Text) + " " + Convert.ToString(dataItem["LastName"].Text);
                    isChecked = ((CheckBox)dataItem.FindControl("chkSelectUser")).Checked;

                    if (isChecked)
                    {
                        if (!selectedItems.ContainsKey(orgUserID))
                        {
                            selectedItems.Add(orgUserID, orgUserName);
                        }
                    }
                    else
                    {
                        if (selectedItems != null && selectedItems.ContainsKey(orgUserID))
                        {
                            selectedItems.Remove(orgUserID);
                        }
                    }
                    CurrentViewContext.AssignOrganizationUserIds = selectedItems;
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
        /// To send message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSendMessage_Click(object sender, EventArgs e)
        {
            try
            {
                if (needSendMessageFunctionality)
                {
                    if (CurrentViewContext.AssignOrganizationUserIds.IsNotNull() && !CurrentViewContext.AssignOrganizationUserIds.Any())
                    {
                        base.ShowErrorInfoMessage("Please select user(s) to send message.");
                    }
                    else
                    {
                        //Get and set OrgUsersToList in session
                        if (!Session["OrgUsersToList"].IsNullOrEmpty())
                        {
                            Session.Remove("OrgUsersToList");
                        }
                        Session["OrgUsersToList"] = CurrentViewContext.AssignOrganizationUserIds;
                        //SetSessionValues();
                        System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "OpenPopup();", true);
                    }
                }
                else
                {
                    base.ShowErrorInfoMessage("Send message functionality is not available for current screen.");
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

        protected void ddlTenantName_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            //UAT-1055
            //ucCustomAttributeLoader.Reset();
            ddlPackage.ClearCheckedItems();
            ddlPackage.ClearSelection();
            CurrentViewContext.SelectedPkgIds = String.Empty;
            if (!ddlTenantName.SelectedIndex.IsNullOrEmpty() && ddlTenantName.SelectedIndex > 0)
            {
                Presenter.GetArchiveStateList();
                rbSubscriptionState.Visible = true;
                rbSubscriptionExpiryState.Visible = true;
                //UAt-1055
                //ucCustomAttributeLoader.TenantId = CurrentViewContext.SelectedTenantId;
                ucCustomAttributeLoaderNodeSearch.TenantId = CurrentViewContext.SelectedTenantId;
            }
            else
            {
                rbSubscriptionState.Visible = false;
                rbSubscriptionExpiryState.Visible = false;
                //Hide Send Message Button
                fsucCmdBar1.ClearButton.Style.Add("display", "none");
                ucCustomAttributeLoaderNodeSearch.Reset();
            }
            BindPackage();
            BindUserGroups();
            BindItemComplianceStatus();
            BindAdminProgramStudy();
            //UAT-789
            if (ddlTenantName.SelectedIndex <= 0)
            {
                grdItemData.Rebind();
            }

            if (needSendMessageFunctionality)
            {
                if (grdItemData.Items.Count > 0)
                    fsucCmdBar1.ClearButton.Style.Clear();
                else
                    fsucCmdBar1.ClearButton.Style.Add("display", "none");
            }
        }

        //protected void ddlPackage_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        //{
        //    BindCategory();
        //}

        protected void ddlTenantName_DataBound(object sender, EventArgs e)
        {
            ddlTenantName.Items.Insert(0, new RadComboBoxItem("--Select--"));
        }

        //protected void ddlPackage_DataBound(object sender, EventArgs e)
        //{
        //    // ddlPackage.Items.Insert(0, new RadComboBoxItem("--Select--"));
        //}

        protected void ddlCategory_DataBound(object sender, EventArgs e)
        {
            //  ddlCategory.Items.Insert(0, new RadComboBoxItem("--Select--"));
        }

        protected void ddlProgram_DataBound(object sender, EventArgs e)
        {
            ddlProgram.Items.Insert(0, new RadComboBoxItem("--Select--"));
        }


        #endregion

        #endregion

        #region Methods

        #region Private Methods
        private void BindPackage()
        {
            Presenter.GetCompliancePackage();
            ddlPackage.DataSource = CurrentViewContext.lstCompliancePackage2;
            ddlPackage.DataBind();

            if (Args != null && Args.ContainsKey("PackageId"))
            {
                CurrentViewContext.SelectedPackageId = Convert.ToInt32(Args["PackageId"]);
            }
            else
            {
                CurrentViewContext.SelectedPackageId = 0;
            }

            //UAT-4136
            List<String> lstPkgs = new List<String>();
            if (!CurrentViewContext.SelectedPkgIds.IsNullOrEmpty())
                lstPkgs = CurrentViewContext.SelectedPkgIds.Split(',').ToList();

            if (!lstPkgs.IsNullOrEmpty())
            {
                ddlPackage.Items.Where(x => lstPkgs.Contains(x.Value)).ForEach(y =>
                {
                    y.Checked = true;
                });
            }

            BindCategory();

        }

        private void BindCategory()
        {
            Presenter.GetComplianceCategory();

            CurrentViewContext.lstComplianceCategory.ForEach(cat => cat.CategoryName =
                String.IsNullOrEmpty(cat.CategoryLabel) ? cat.CategoryName : cat.CategoryLabel);

            ddlCategory.DataSource = CurrentViewContext.lstComplianceCategory;
            ddlCategory.DataBind();

            if (Args != null && Args.ContainsKey("CategoryId"))
                CurrentViewContext.SelectedCategoryId = Convert.ToInt32(Args["CategoryId"]);
            else
                CurrentViewContext.SelectedCategoryId = 0;

            if (!CurrentViewContext.SelectedCategoryIds.IsNullOrEmpty())
            {
                String[] selectedIds = CurrentViewContext.SelectedCategoryIds.Split(',');
                foreach (RadComboBoxItem item in ddlCategory.Items)
                {
                    if (selectedIds.Contains(item.Value))
                    {
                        item.Checked = true;
                    }

                }
            }
        }

        /// <summary>
        /// To bind program dropdown
        /// </summary>
        private void BindUserGroups()
        {
            Presenter.GetAllUserGroups();

            ddlUserGroup.DataSource = CurrentViewContext.lstUserGroup;
            ddlUserGroup.DataBind();

            if (!CurrentViewContext.SelectedUserGroupIDs.IsNullOrEmpty())
            {
                foreach (RadComboBoxItem item in ddlUserGroup.Items)
                {
                    String[] selectedIds = CurrentViewContext.SelectedUserGroupIDs.Split(',');
                    if (selectedIds.Contains(item.Value))
                    {
                        item.Checked = true;
                    }
                }
            }
        }

        private void BindItemComplianceStatus()
        {
            Presenter.GetItemComplianceStatus();
            chkItemStatus.DataSource = CurrentViewContext.lstItemComplianceStatus;
            chkItemStatus.DataBind();
        }

        private void BindAdminProgramStudy()
        {
            //Presenter.GetAdminProgramStudy();
            //ddlProgram.DataSource = CurrentViewContext.lstAdminProgramStudy;
            //ddlProgram.DataBind();

            //if (Args != null && Args.ContainsKey("SelectedProgramStudyId"))
            //    CurrentViewContext.SelectedProgramStudyId = Convert.ToInt32(Args["SelectedProgramStudyId"]);
            //else
            //    CurrentViewContext.SelectedProgramStudyId = 0;
        }

        private void FilterGridColumn()
        {
            if (!ViewState["SortExpression"].IsNull())
            {
                CurrentViewContext.ItemDataGridCustomPaging.SortExpression = Convert.ToString(ViewState["SortExpression"]);
                CurrentViewContext.ItemDataGridCustomPaging.SortDirectionDescending = Convert.ToBoolean(ViewState["SortDirection"]);
            }
            CurrentViewContext.SearchItemDataContract.FilterColumns = ViewState["FilterColumns"] == null ? new List<String>() : (List<String>)(ViewState["FilterColumns"]);
            CurrentViewContext.SearchItemDataContract.FilterOperators = ViewState["FilterOperators"] == null ? new List<String>() : (List<String>)(ViewState["FilterOperators"]);
            CurrentViewContext.SearchItemDataContract.FilterValues = ViewState["FilterValues"] == null ? new ArrayList() : (ArrayList)(ViewState["FilterValues"]);
            CurrentViewContext.SearchItemDataContract.FilterTypes = ViewState["FilterTypes"] == null ? new List<String>() : (List<String>)(ViewState["FilterTypes"]);

            if (ViewState["FilterPair"] != null)
            {
                Pair filter = (Pair)ViewState["FilterPair"];
                Int32 filterIndex = CurrentViewContext.SearchItemDataContract.FilterColumns.IndexOf(filter.Second.ToString());
                String filterValue = grdItemData.MasterTableView.Columns.FindByUniqueName(filter.Second.ToString()).CurrentFilterValue;
                if (filter.First.ToString() != GridKnownFunction.NoFilter.ToString() && ActionType != ViewMode.Search.ToString())
                {
                    String filterTypes = grdItemData.MasterTableView.Columns.FindByUniqueName(filter.Second.ToString()).DataTypeName;
                    if (filterIndex != -1)
                    {
                        CurrentViewContext.SearchItemDataContract.FilterOperators[filterIndex] = filter.First.ToString();
                        CurrentViewContext.SearchItemDataContract.FilterTypes[filterIndex] = filterTypes.ToString();
                        if (filterTypes == "System.Decimal")
                        {
                            CurrentViewContext.SearchItemDataContract.FilterValues[filterIndex] = Convert.ToDecimal(filterValue);
                        }
                        else if (filterTypes == "System.Int32")
                        {
                            CurrentViewContext.SearchItemDataContract.FilterValues[filterIndex] = Convert.ToInt32(filterValue);
                        }
                        else if (filterTypes == "System.DateTime")
                        {
                            if (!filterValue.IsNullOrEmpty())
                            {
                                try
                                {
                                    //try to convert any value to date
                                    CurrentViewContext.SearchItemDataContract.FilterValues[filterIndex] = Convert.ToDateTime(filterValue);
                                }
                                catch
                                {
                                    //date filter value could not be converted, set filter value to any default date
                                    CurrentViewContext.SearchItemDataContract.FilterValues[filterIndex] = Convert.ToDateTime("01/01/1901");
                                    //return;
                                }
                            }

                            //To set IsNull filter to other Date format filter and set to any default date in case of Null date
                            if (CurrentViewContext.SearchItemDataContract.FilterOperators.Contains("IsNull"))
                            {
                                CurrentViewContext.SearchItemDataContract.FilterOperators[filterIndex] = "NullOtherThanString";
                                CurrentViewContext.SearchItemDataContract.FilterValues[filterIndex] = Convert.ToDateTime("01/01/1901");
                            }
                        }
                        else
                        {
                            CurrentViewContext.SearchItemDataContract.FilterValues[filterIndex] = filterValue;
                        }
                    }
                    else
                    {
                        CurrentViewContext.SearchItemDataContract.FilterColumns.Add(filter.Second.ToString());
                        CurrentViewContext.SearchItemDataContract.FilterOperators.Add(filter.First.ToString());
                        CurrentViewContext.SearchItemDataContract.FilterTypes.Add(filterTypes.ToString());
                        if (filterTypes == "System.Decimal")
                        {
                            CurrentViewContext.SearchItemDataContract.FilterValues.Add(Convert.ToDecimal(filterValue));
                        }
                        else if (filterTypes == "System.Int32")
                        {
                            CurrentViewContext.SearchItemDataContract.FilterValues.Add(Convert.ToInt32(filterValue));
                        }
                        else if (filterTypes == "System.DateTime")
                        {
                            if (!filterValue.IsNullOrEmpty())
                            {
                                try
                                {
                                    //try to convert any value to date
                                    CurrentViewContext.SearchItemDataContract.FilterValues.Add(Convert.ToDateTime(filterValue));
                                }
                                catch
                                {
                                    //date filter value could not be converted, set filter value to any default date
                                    CurrentViewContext.SearchItemDataContract.FilterValues.Add(Convert.ToDateTime("01/01/1901"));
                                    //return;
                                }
                            }

                            //To set IsNull filter to other Date format filter and set to any default date in case of Null date
                            if (CurrentViewContext.SearchItemDataContract.FilterOperators.Contains("IsNull"))
                            {
                                Int32 index = CurrentViewContext.SearchItemDataContract.FilterOperators.IndexOf("IsNull");
                                CurrentViewContext.SearchItemDataContract.FilterOperators[index] = "NullOtherThanString";
                                CurrentViewContext.SearchItemDataContract.FilterValues.Add(Convert.ToDateTime("01/01/1901"));
                            }
                        }
                        else
                        {
                            CurrentViewContext.SearchItemDataContract.FilterValues.Add(filterValue);
                        }
                    }
                }
                else if (filterIndex != -1)
                {
                    CurrentViewContext.SearchItemDataContract.FilterOperators.RemoveAt(filterIndex);
                    CurrentViewContext.SearchItemDataContract.FilterValues.RemoveAt(filterIndex);
                    CurrentViewContext.SearchItemDataContract.FilterColumns.RemoveAt(filterIndex);
                    CurrentViewContext.SearchItemDataContract.FilterTypes.RemoveAt(filterIndex);
                }

                ViewState["FilterColumns"] = CurrentViewContext.SearchItemDataContract.FilterColumns;
                ViewState["FilterOperators"] = CurrentViewContext.SearchItemDataContract.FilterOperators;
                ViewState["FilterValues"] = CurrentViewContext.SearchItemDataContract.FilterValues;
                ViewState["FilterTypes"] = CurrentViewContext.SearchItemDataContract.FilterTypes;
                ViewState["FilterPair"] = null;
            }
            if (ActionType == ViewMode.Search.ToString())
            {
                grdItemData.MasterTableView.FilterExpression = null;
                grdItemData.MasterTableView.SortExpressions.Clear();
                grdItemData.CurrentPageIndex = 0;
                grdItemData.MasterTableView.CurrentPageIndex = 0;
                foreach (GridColumn column in grdItemData.MasterTableView.RenderColumns)
                {
                    if (column.ColumnType == "GridBoundColumn")
                    {
                        GridBoundColumn boundColumn = (GridBoundColumn)column;
                        String columnName = boundColumn.UniqueName.ToString();
                        grdItemData.MasterTableView.GetColumnSafe(columnName).CurrentFilterFunction = GridKnownFunction.NoFilter;
                        grdItemData.MasterTableView.GetColumnSafe(columnName).CurrentFilterValue = String.Empty;
                    }
                }
                //ViewState["FilterPair"] = null;
                ClearViewStatesForFilter();
            }
        }

        public void ClearViewStatesForFilter()
        {
            ViewState["FilterColumns"] = null;
            ViewState["FilterOperators"] = null;
            ViewState["FilterPair"] = null;
            ViewState["FilterValues"] = null;
            ViewState["FilterTypes"] = null;
        }

        #region UAT-806 Creation of granular permissions for Client Admin users

        /// <summary>
        /// Hide Show grid and page controls
        /// </summary>
        private void HideShowControlsForGranularPermission()
        {
            if (CurrentViewContext.IsDOBDisable)
            {
                divDOB.Visible = false;
            }
        }
        #endregion

        private void DisplayMessageSentStatus()
        {
            if (hdMessageSent.Value == "sent")
            {
                base.ShowSuccessMessage("Message has been sent successfully.");
                hdMessageSent.Value = "new";
            }
        }
        #endregion

        protected void grdItemData_PreRender(object sender, EventArgs e)
        {
            if (needSendMessageFunctionality)
            {
                if (grdItemData.MasterTableView.Items.Count > 0)
                    fsucCmdBar1.ClearButton.Style.Clear();
                else
                    fsucCmdBar1.ClearButton.Style.Add("display", "none");
            }
        }

        protected void chkSelectAllResults_CheckedChanged(object sender, EventArgs e)
        {
            if (needSendMessageFunctionality && VirtualPageCount > 0)
            {
                bool needToCheckboxChecked = false;
                bool needToMarkHeadCheckboxChecked = true;

                if (((CheckBox)sender).Checked)
                {
                    FilterGridColumn();
                    ItemDataGridCustomPaging.FilterColumns = SearchItemDataContract.FilterColumns;
                    ItemDataGridCustomPaging.FilterOperators = SearchItemDataContract.FilterOperators;
                    ItemDataGridCustomPaging.FilterValues = SearchItemDataContract.FilterValues;
                    ItemDataGridCustomPaging.FilterTypes = SearchItemDataContract.FilterTypes;

                    Presenter.GetAllOrganisationUserIds();
                    needToCheckboxChecked = true;
                    needToMarkHeadCheckboxChecked = true;
                }
                else
                {
                    CurrentViewContext.AssignOrganizationUserIds = new Dictionary<int, string>();
                    needToMarkHeadCheckboxChecked = false;
                }

                if (needToCheckboxChecked)
                {
                    Dictionary<Int32, String> selectedItems = CurrentViewContext.AssignOrganizationUserIds;

                    foreach (GridDataItem item in grdItemData.Items)
                    {
                        String applicantId = item.OwnerTableView.DataKeyValues[item.ItemIndex]["ApplicantId"].ToString();

                        CheckBox checkBox = ((CheckBox)item.FindControl("chkSelectUser"));

                        if (!selectedItems.IsNullOrEmpty() && selectedItems.ContainsKey(Convert.ToInt32(applicantId)))
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
                    foreach (GridDataItem item in grdItemData.Items)
                    {
                        CheckBox checkBox = ((CheckBox)item.FindControl("chkSelectUser"));
                        checkBox.Checked = needToCheckboxChecked;
                    }
                }

                GridHeaderItem headerItem = grdItemData.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
                CheckBox headerCheckBox = ((CheckBox)headerItem.FindControl("chkSelectAll"));
                headerCheckBox.Checked = needToMarkHeadCheckboxChecked;
            }
        }

        protected void btnPackageChecked_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentViewContext.SelectedPkgIds = String.Empty;
                foreach (RadComboBoxItem item in ddlPackage.CheckedItems)
                {
                    CurrentViewContext.SelectedPkgIds = String.IsNullOrEmpty(CurrentViewContext.SelectedPkgIds) ? item.Value : CurrentViewContext.SelectedPkgIds + "," + item.Value;
                }

                BindCategory();

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

