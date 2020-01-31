#region Namespaces

#region System Defined

using System;
using System.Linq;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using Microsoft.Practices.ObjectBuilder;
using System.Collections;

#endregion

#region Application Specific

using INTSOF.Utils;
using Telerik.Web.UI;
using INTSOF.UI.Contract.IntsofSecurityModel;
using CoreWeb.CommonControls.Views;
using INTSOF.Utils.Consts;
using INTERSOFT.WEB.UI.WebControls;
//using Microsoft.Practices.CompositeWeb.Web.UI;
using Entity.ClientEntity;
using CoreWeb.Shell;
using System.Xml.Serialization;
using System.IO;
using INTSOF.UI.Contract.ComplianceManagement;
using System.Text;
using System.Web.UI;
using System.Threading;
using CoreWeb.IntsofSecurityModel;
using System.Reflection;
using INTSOF.UI.Contract.SysXSecurityModel;
using INTSOF.UI.Contract.ComplianceOperation;

#endregion

#endregion

namespace CoreWeb.Search.Views
{
    public partial class ApplicantPortFolioSearch : BaseUserControl, IApplicantPortFolioSearchView
    {
        #region Variables

        private ApplicantPortFolioSearchPresenter _presenter = new ApplicantPortFolioSearchPresenter();
        private String _viewType;
        private Int32 tenantId = 0;
        private CustomPagingArgsContract _gridCustomPaging = null;
        private SearchItemDataContract _gridSearchContract = null;

        #endregion

        #region Properties

        public ApplicantPortFolioSearchPresenter Presenter
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

        public List<CustomComplianceContract> lstPackageSubscription
        {
            get;
            set;
        }

        public Int32 SelectedTenantId
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
                    hdnSelectedTenantID.Value = CurrentViewContext.SelectedTenantId.ToString();
                }
                else
                {
                    ddlTenantName.SelectedIndex = value;
                }
            }
        }

        public Int32? OrganizationUserID
        {
            get
            {
                if (String.IsNullOrEmpty(txtUserID.Text))
                    return 0;
                return Convert.ToInt32(txtUserID.Text);
            }
            set
            {
                if (value == null)
                    txtUserID.Text = String.Empty;
                else
                    txtUserID.Text = value.ToString();
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

        public Int32 FilterUserGroupId
        {
            get
            {
                if (String.IsNullOrEmpty(ddlUserGroup.SelectedValue))
                    return 0;
                return Convert.ToInt32(ddlUserGroup.SelectedValue);
            }
            set
            {
                if (value > 0)
                {
                    ddlUserGroup.SelectedValue = value.ToString();
                }
                else
                {
                    ddlUserGroup.SelectedIndex = value;
                }
            }
        }

        public Int32 MatchUserGroupId
        {
            get
            {
                if (String.IsNullOrEmpty(ddlUserGroup.SelectedValue))
                    return 0;
                return Convert.ToInt32(ddlUserGroup.SelectedValue);
            }
            set
            {
                if (value > 0)
                {
                    ddlUserGroup.SelectedValue = value.ToString();
                }
                else
                {
                    ddlUserGroup.SelectedIndex = value;
                }
            }
        }

        public Int32 CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        public IApplicantPortFolioSearchView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public String ApplicantName
        {
            get
            {
                if (!ViewState["ApplicantName"].IsNull())
                {
                    return ViewState["ApplicantName"] as String;
                }

                return String.Empty;
            }
            set
            {
                ViewState["ApplicantName"] = value;
            }
        }
        public List<Entity.Tenant> lstTenant
        {
            get;
            set;
        }

        public List<CustomComplianceContract> AssignOrganizationUsers
        {
            get
            {
                if (!ViewState["AssignOrganizationUsers"].IsNull())
                {
                    return ViewState["AssignOrganizationUsers"] as List<CustomComplianceContract>;
                }

                return new List<CustomComplianceContract>();
            }
            set
            {
                ViewState["AssignOrganizationUsers"] = value;
            }
        }

        public String ApplicantFirstName
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

        public String ApplicantLastName
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

        public String EmailAddress
        {
            get
            {
                return txtEmail.Text;
            }
            set
            {
                txtEmail.Text = value;
            }
        }

        public String SSN
        {
            //get
            //{
            //    return txtSSN.Text;
            //}
            //set
            //{
            //    txtSSN.Text = value;
            //}
            get;
            set;
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

        public List<ApplicantDataList> ApplicantSearchData
        {
            get;
            set;
        }

        public List<UserGroup> lstUserGroup
        {
            get;
            set;
        }

        public Int32 UserGroupId
        {
            get
            {
                if (String.IsNullOrEmpty(ddlUserGroup.SelectedValue))
                    return 0;
                return Convert.ToInt32(ddlUserGroup.SelectedValue);
            }
            set
            {
                if (value > 0)
                {
                    ddlUserGroup.SelectedValue = value.ToString();
                }
                else
                {
                    ddlUserGroup.SelectedIndex = value;
                }
            }
        }

        public Int32 DPM_ID
        {
            get;
            set;
        }

        //UAT-1055: WB: As an admin, I should be able to search multiple nodes at a time.
        public String DPM_IDs
        {
            get;
            set;
        }

        public String CustomFields
        {
            get;
            set;
        }

        public Int32? NodeId
        {
            get
            {
                return ucCustomAttributeLoaderSearch.NodeId;
            }
            set
            {
                ucCustomAttributeLoaderSearch.NodeId = Convert.ToInt32(value);
            }
        }

        //UAT-1055: WB: As an admin, I should be able to search multiple nodes at a time.
        public String NodeIds
        {
            get
            {
                return ucCustomAttributeLoaderSearch.NodeIds;
            }
            set
            {
                ucCustomAttributeLoaderSearch.NodeIds = value;
            }
        }

        #region Custom Paging


        /// <summary>
        /// Current Page Index</summary>
        /// <value>
        /// Gets or sets the value for CurrentPageIndex.</value>
        public Int32 CurrentPageIndex
        {
            get
            {
                return grdApplicantSearchData.MasterTableView.CurrentPageIndex + 1;
            }
            set
            {

                grdApplicantSearchData.MasterTableView.CurrentPageIndex = value - 1;

            }
        }

        /// <summary>
        /// Page Size</summary>
        /// <value>
        /// Gets the value for PageSize.</value>
        public Int32 PageSize
        {
            get
            {
                // Maximum 100 record allowed from DB. 
                //return grdApplicantSearchData.PageSize > 100 ? 100 : grdApplicantSearchData.PageSize;
                return grdApplicantSearchData.PageSize;
            }
            set
            {
                grdApplicantSearchData.PageSize = value;
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
                grdApplicantSearchData.VirtualItemCount = value;
                grdApplicantSearchData.MasterTableView.VirtualItemCount = value;
            }
        }

        /// <summary>
        /// To get object of shared class of custom paging
        /// </summary>
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
                VirtualRecordCount = value.VirtualPageCount;
                PageSize = value.PageSize;
                CurrentPageIndex = value.CurrentPageIndex;
            }
        }

        #endregion

        #region UAT-977- Archival Ability
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

        #region UAT-3010:- Granular Permission for Client Admin Users to Archive.

        public String ArchivePermissionCode
        {
            get
            {
                return Convert.ToString(ViewState["ArchivePermissionCode"]);
            }
            set
            {
                ViewState["ArchivePermissionCode"] = value;
            }
        }

        #endregion

        /// <summary>
        /// To set shared class object of search contract
        /// </summary>
        public SearchItemDataContract SetSearchItemDataContract
        {
            set
            {
                var serializer = new XmlSerializer(typeof(SearchItemDataContract));
                var sb = new StringBuilder();
                using (TextWriter writer = new StringWriter(sb))
                {
                    serializer.Serialize(writer, value);
                }
                //Session for maintaning Grid Filter, Paging and Index
                Session[AppConsts.APPLICANT_SEARCH_GRID_SESSION_KEY] = sb.ToString();
            }
        }

        /// <summary>
        /// To get shared class object of search contract
        /// </summary>
        public SearchItemDataContract GetSearchItemDataContract
        {
            get
            {
                if (_gridSearchContract.IsNull())
                {
                    var serializer = new XmlSerializer(typeof(SearchItemDataContract));
                    TextReader reader = new StringReader(Convert.ToString(Session[AppConsts.APPLICANT_SEARCH_GRID_SESSION_KEY]));
                    _gridSearchContract = (SearchItemDataContract)serializer.Deserialize(reader);
                }
                return _gridSearchContract;
            }
        }

        /// <summary>
        /// Gets or Sets the value for selected Items.
        /// </summary>
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

        public Dictionary<String, List<Int32>> DicSubscriptionIDs
        {
            get;
            set;
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

        public List<UserNodePermissionsContract> lstUserNodePermissionsContract
        {
            get;
            set;
        }

        public List<ApplicantInstitutionHierarchyMapping> lstApplicantInstitutionHierarchyMapping
        {
            get;
            set;
        }

        public String OrganisationUserIds { get; set; }

        //public Boolean ShowActiveOrdersOnly { get; set; } //UAT-4273

        //UAT-4273
        Boolean IApplicantPortFolioSearchView.ShowActiveOrdersOnly
        {
            get
            {
                if (!rbShowActiveOrdersOnly.SelectedValue.IsNullOrEmpty())
                {
                    if (rbShowActiveOrdersOnly.SelectedValue.Equals("True"))
                    {                        
                        ViewState["ShowActiveOrdersOnly"] = true;
                    }
                    else
                    {                        
                        ViewState["ShowActiveOrdersOnly"] = false;
                    }
                }
                return (Boolean)ViewState["ShowActiveOrdersOnly"];
            }
            set
            {
                rbShowActiveOrdersOnly.SelectedValue = Convert.ToString(value);
                ViewState["ShowActiveOrdersOnly"] = value;
            }
        }
        //UAT-4273

        #endregion

        #region Events

        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                base.Title = "Applicant Portfolio Search";
                base.SetPageTitle("Applicant Portfolio Search");
                fsucCmdBarButton.SubmitButton.CausesValidation = false;

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

        protected void Page_PreRender(object sender, EventArgs e)
        {
            //System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "GetDataFromLocalStorage();", true);
        }

        /// <summary>
        /// Page Load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Form.Attributes.Add("role", "application");
            CurrentViewContext.SSN = txtSSN.TextWithPrompt;
            _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];

            if (!this.IsPostBack)
            {
                //Set MinDate and MaxDate for DOB
                dpkrDOB.MinDate = Convert.ToDateTime("01-01-1900");
                dpkrDOB.MaxDate = DateTime.Now;

                Presenter.OnViewInitialized();
                BindControls();

                ApplySSNMask();

                //To check if cancel button is clicked on Edit Profile page
                //and get session values for controls
                Dictionary<String, String> args = new Dictionary<String, String>();

                if (!Request.QueryString["args"].IsNull())
                {
                    args.ToDecryptedQueryString(Request.QueryString["args"]);
                    if (args.ContainsKey("CancelClicked") && args["CancelClicked"].IsNotNull()
                        || args.ContainsKey("PageType") && args["PageType"].IsNotNull() && (args["PageType"] == "PortfolioSearch"))
                    {
                        GetSessionValues();
                    }

                    else
                        Session[AppConsts.APPLICANT_SEARCH_SESSION_KEY] = null;
                }
                else
                {
                    fsucCmdBarButton.ClearButton.Style.Add("display", "none");
                    fsucCmdBarButton.ExtraButton.Style.Add("display", "none");
                    grdApplicantSearchData.Visible = false;
                    chkSelectAllResults.Visible = false;
                }

                ApplyActionLevelPermission(ActionCollection, "Applicant Portfolio Search");
            }
            Presenter.OnViewLoaded();
            ucCustomAttributeLoaderSearch.TenantId = SelectedTenantId;
            ucCustomAttributeLoaderSearch.ScreenType = "CommonScreen";
            (fsucCmdBarButton as CoreWeb.Shell.Views.CommandBar).SaveButton.ToolTip = "Click to search orders per the criteria entered above";
            (fsucCmdBarButton as CoreWeb.Shell.Views.CommandBar).SubmitButton.ToolTip = "Click to remove all values entered in the search criteria above";
            (fsucCmdBarButton as CoreWeb.Shell.Views.CommandBar).CancelButton.ToolTip = "Click to cancel. Any data entered will not be saved";
            //UAt-806:- 
            HideShowControlsForGranularPermission();
            if (!Presenter.IsDefaultTenant)
            {
                grdApplicantSearchData.MasterTableView.GetColumn("ApplicantView").Visible = false;
            }

        }

        /// <summary>
        /// To perform search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarSearch_Click(object sender, EventArgs e)
        {
            //To reset AssignOrganizationUserIds
            CurrentViewContext.AssignOrganizationUserIds = new Dictionary<Int32, String>();
            CurrentViewContext.AssignOrganizationUsers = new List<CustomComplianceContract>(); //UAT:4218
            grdApplicantSearchData.Visible = true;
            chkSelectAllResults.Visible = true;
            //To reset grid filters 
            ResetGridFilters();

            if (grdApplicantSearchData.Items.Count > 0)
            {
                fsucCmdBarButton.ClearButton.Style.Clear();
                fsucCmdBarButton.ExtraButton.Style.Clear();
            }
            else
            {
                fsucCmdBarButton.ClearButton.Style.Add("display", "none");
                fsucCmdBarButton.ExtraButton.Style.Add("display", "none");
            }

            chkSelectAllResults.Checked = false;
            //UAT-1955
            grdApplicantSearchData.Focus();
        }

        /// <summary>
        /// To reset controls
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarReset_Click(object sender, EventArgs e)
        {
            Presenter.GetTenants();
            BindControls();
            txtUserID.Text = String.Empty;
            txtFirstName.Text = String.Empty;
            txtLastName.Text = String.Empty;
            dpkrDOB.SelectedDate = null;
            txtEmail.Text = String.Empty;
            txtSSN.Text = String.Empty;
            SSN = null;
            if (Presenter.IsDefaultTenant)
            {
                ucCustomAttributeLoaderSearch.Reset();
            }
            else
            {
                ucCustomAttributeLoaderSearch.ResetControlData(true);
            }
            //To reset AssignOrganizationUserIds
            CurrentViewContext.AssignOrganizationUserIds = new Dictionary<Int32, String>();
            CurrentViewContext.AssignOrganizationUsers = new List<CustomComplianceContract>(); //UAT:4218
            //To reset grid filters 
            ResetGridFilters();
            btnArchieve.Enabled = true;
            //rbSubscriptionState.Visible = false;

            //Reset session
            Session[AppConsts.APPLICANT_SEARCH_SESSION_KEY] = null;
            if (Presenter.IsDefaultTenant)
            {
                fsucCmdBarButton.ClearButton.Style.Add("display", "none");
                fsucCmdBarButton.ExtraButton.Style.Add("display", "none");
                rbSubscriptionState.Visible = false;
            }
            else
            {
                fsucCmdBarButton.ClearButton.Style.Clear();
                fsucCmdBarButton.ExtraButton.Style.Clear();
                rbSubscriptionState.Visible = true;
            }

            CurrentViewContext.VirtualRecordCount = 0;
            chkSelectAllResults.Checked = false;
        }

        /// <summary>
        /// Redirect to Home page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarCancel_Click(object sender, EventArgs e)
        {
            //Reset session
            Session[AppConsts.APPLICANT_SEARCH_SESSION_KEY] = null;
            Dictionary<String, String> queryString = new Dictionary<String, String>();
            Response.Redirect(String.Format(AppConsts.DASHBOARD_PAGE_NAME), true);
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
                    //grdApplicantSearchData.Rebind();
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

        #region Grid Events

        /// <summary>
        /// To set data source to the grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdApplicantSearchData_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                GridCustomPaging.CurrentPageIndex = CurrentPageIndex;
                GridCustomPaging.PageSize = PageSize;
                CurrentViewContext.GridCustomPaging = GridCustomPaging;
                //UAT-1055: WB: As an admin, I should be able to search multiple nodes at a time.
                //CurrentViewContext.DPM_ID = ucCustomAttributeLoaderSearch.DPM_ID;
                CurrentViewContext.DPM_IDs = ucCustomAttributeLoaderSearch.DPM_ID;
                CurrentViewContext.CustomFields = ucCustomAttributeLoaderSearch.GetCustomDataXML();
                Presenter.PerformSearch();

                Presenter.GetUserNodePermission();

                if (!CurrentViewContext.ApplicantSearchData.IsNullOrEmpty())
                {
                    CurrentViewContext.OrganisationUserIds = string.Join(",", CurrentViewContext.ApplicantSearchData.Select(cond => cond.OrganizationUserId).Distinct());
                    Presenter.GetApplicantInstitutionHierarchyMapping();
                }
                else
                {
                    CurrentViewContext.lstApplicantInstitutionHierarchyMapping = new List<ApplicantInstitutionHierarchyMapping>();
                }

                grdApplicantSearchData.DataSource = CurrentViewContext.ApplicantSearchData;
                SubscriptionsArchivedFromPopup();
                DisplayMessageSentStatus();
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

        private void DisplayMessageSentStatus()
        {
            if (hdMessageSent.Value == "sent")
            {
                base.ShowSuccessMessage("Message has been sent successfully.");
                hdMessageSent.Value = "new";
                //CurrentViewContext.AssignOrganizationUserIds = new Dictionary<Int32, String>();
            }
        }



        /// <summary>
        /// Grid Item Command event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdApplicantSearchData_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                #region DetailScreenNavigation

                if (e.CommandName.Equals("ViewDetail"))
                {
                    Int32 selectedTenantId = 0;
                    SetSessionValues();
                    if (!ddlTenantName.SelectedValue.IsNullOrEmpty())
                    {
                        selectedTenantId = SelectedTenantId;
                    }
                    else
                    {
                        selectedTenantId = TenantId;
                    }

                    Dictionary<String, String> queryString = new Dictionary<String, String>();
                    String organizationUserId = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrganizationUserId"].ToString();
                    queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "TenantId", Convert.ToString(selectedTenantId) },
                                                                    { "Child", ChildControls.ApplicantPortfolioDetailPage},
                                                                    { "OrganizationUserId", organizationUserId},
                                                                    {"PageType", WorkQueueType.ApplicantPortFolioSearch.ToString()}
                                                                 };
                    string url = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                    Response.Redirect(url, true);

                }
                #endregion

                #region ManageDocumentScreenNavigation

                if (e.CommandName.Equals("ManageDocuments"))
                {
                    Int32 selectedTenantId = 0;
                    SetSessionValues();
                    if (!ddlTenantName.SelectedValue.IsNullOrEmpty())
                    {
                        selectedTenantId = SelectedTenantId;
                    }
                    else
                    {
                        selectedTenantId = TenantId;
                    }

                    Dictionary<String, String> queryString = new Dictionary<String, String>();
                    String organizationUserId = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrganizationUserId"].ToString();
                    queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "TenantID", Convert.ToString(selectedTenantId) },
                                                                    { "Child", ChildControls.ManageUploadDocuments},
                                                                    { "OrganizationUserId", organizationUserId},
                                                                    {"PageType","PortfolioSearch"}
                                                                 };
                    string url = String.Format(@"/ComplianceOperations/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                    Response.Redirect(url, true);

                }
                #endregion

                #region UAT-1261: WB: As an ADB admin, I should be able to "login" as any student to see what they see.
                //Applicant View Dashboard Navigation

                if (e.CommandName.Equals("ApplicantView"))
                {
                    String applicantUserID = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ApplicantUserID"].ToString();
                    Int32 tenantID = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TenantID"]);

                    #region Switch to Applicant View
                    SwitchToApplicant(applicantUserID, tenantID);
                    #endregion

                }
                #endregion

                #region For Sort command

                if (!ViewState["SortExpression"].IsNull())
                {
                    CurrentViewContext.GridCustomPaging.SortExpression = Convert.ToString(ViewState["SortExpression"]);
                    CurrentViewContext.GridCustomPaging.SortDirectionDescending = Convert.ToBoolean(ViewState["SortDirection"]);
                }

                #endregion

                // Hide filter when exportig to pdf or word
                if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToPdfCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToWordCommandName
                    || e.CommandName == Telerik.Web.UI.RadGrid.ExportToExcelCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToCsvCommandName)
                {
                    base.ConfigureExport(grdApplicantSearchData);
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
                            grdApplicantSearchData.MasterTableView.GetColumn("CustomAttributesTemp").Display = true;
                            //UAT-1051 Complio: Full social security number should not be displayed on grid exports. Added Duplicate Column with masked SSN
                            // and displayed the masked column on Export instead of actual column.
                            grdApplicantSearchData.MasterTableView.GetColumn("_SSN").Display = true;
                        }
                        else
                        {
                            grdApplicantSearchData.MasterTableView.GetColumn("CustomAttributesTemp").Display = false;
                            //UAT-1051 Complio: Full social security number should not be displayed on grid exports. Added Duplicate Column with masked SSN
                            // and displayed the masked column on Export instead of actual column.
                            grdApplicantSearchData.MasterTableView.GetColumn("_SSN").Display = false;
                        }
                    }
                }
                if (e.CommandName == "Cancel")
                {
                    grdApplicantSearchData.MasterTableView.GetColumn("CustomAttributesTemp").Display = false;
                    grdApplicantSearchData.MasterTableView.GetColumn("_SSN").Display = false;
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
        protected void grdApplicantSearchData_SortCommand(object sender, GridSortCommandEventArgs e)
        {
            try
            {
                if (e.NewSortOrder != GridSortOrder.None)
                {
                    GridCustomPaging.SortExpression = e.SortExpression;
                    GridCustomPaging.SortDirectionDescending = e.NewSortOrder.Equals(GridSortOrder.Descending);
                    CurrentViewContext.GridCustomPaging.SortExpression = e.SortExpression;
                    CurrentViewContext.GridCustomPaging.SortDirectionDescending = e.NewSortOrder.Equals(GridSortOrder.Descending);
                }
                else
                {
                    GridCustomPaging.SortExpression = String.Empty;
                    GridCustomPaging.SortDirectionDescending = false;
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
        /// <summary>
        /// Grid Item Bound Expression
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdApplicantSearchData_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                //Checks if item is GridDataItem type.
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;

                    //UAT-1051 Complio: Full social security number should not be displayed on grid exports. Added Duplicate Column with masked SSN
                    // and displayed the masked column on Export instead of actual column.
                    dataItem["_SSN"].Text = Presenter.GetMaskedSSN(Convert.ToString(dataItem["_SSN"].Text));

                    //UAT-806 Creation of granular permissions for Client Admin users
                    if (CurrentViewContext.SSNPermissionCode.ToUpper() == EnumSystemPermissionCode.MASKED_READ_PERMISSION.GetStringValue())
                    {
                        dataItem["SSN"].Text = Presenter.GetMaskedSSN(Convert.ToString(dataItem["SSN"].Text));
                    }
                    else
                    {
                        ///Formatting SSN
                        dataItem["SSN"].Text = Presenter.GetFormattedSSN(Convert.ToString(dataItem["SSN"].Text));
                    }

                    RadButton ViewDetail = dataItem["ViewDetail"].Controls[1] as RadButton;
                    ViewDetail.ToolTip = "Click to view the applicant's profile, subscription, and order history details";
                    LinkButton ManageDocuments = dataItem["ManageDocuments"].Controls[0] as LinkButton;
                    string organizationUserId = dataItem["UserID"].Text.ToString();
                    ManageDocuments.ToolTip = string.Concat("Click to go to an admin view of the applicant's Upload Documents screen for User ID: ", organizationUserId);
                }

                //To select checkboxes
                if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
                {

                    GridDataItem dataItem = (GridDataItem)e.Item;
                    if (Convert.ToString(dataItem["CustomAttributes"].Text).Length > 80)
                    {
                        dataItem["CustomAttributes"].ToolTip = dataItem["CustomAttributes"].Text;
                        dataItem["CustomAttributes"].Text = (dataItem["CustomAttributes"].Text).ToString().Substring(0, 80) + "...";
                    }

                    String organizationUserId = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrganizationUserId"].ToString();
                    //Boolean isMapped = Convert.ToBoolean((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["IsUserGroupMatching"].ToString());
                    if (Convert.ToInt32(organizationUserId) != 0)
                    {
                        Dictionary<Int32, String> selectedItems = CurrentViewContext.AssignOrganizationUserIds;
                        if (selectedItems.IsNotNull())
                        {
                            if (selectedItems.ContainsKey(Convert.ToInt32(organizationUserId)))
                            {
                                CheckBox checkBox = ((CheckBox)e.Item.FindControl("chkSelectUser"));
                                checkBox.Checked = true;
                            }
                        }
                    }
                    else
                    {
                        CheckBox checkBox = ((CheckBox)e.Item.FindControl("chkSelectUser"));
                        checkBox.Enabled = false;
                    }

                    if (!HavePermissionToViewDocuments(organizationUserId))
                    {
                        LinkButton lnkBtn = (LinkButton)dataItem["ManageDocuments"].Controls[0];
                        lnkBtn.Visible = false;
                    }
                }
                if (e.Item.ItemType.Equals(GridItemType.Footer))
                {
                    Int32 rowCount = grdApplicantSearchData.Items.Count;
                    if (rowCount > 0)
                    {
                        Int32 checkCount = 0;
                        foreach (GridDataItem item in grdApplicantSearchData.Items)
                        {
                            CheckBox checkBox = ((CheckBox)item.FindControl("chkSelectUser"));
                            if (checkBox.Checked)
                            {
                                checkCount++;
                            }
                        }
                        if (rowCount == checkCount)
                        {
                            GridHeaderItem item = grdApplicantSearchData.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
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

        private bool HavePermissionToViewDocuments(String organizationUserId)
        {
            string permissionCode = string.Empty;
            List<Int32> lstLoggedInUserNodes = CurrentViewContext.lstUserNodePermissionsContract.Select(col => col.DPM_ID).ToList();
            List<Int32> lstApplicantUserNodes = CurrentViewContext.lstApplicantInstitutionHierarchyMapping.Where(cond => cond.OrganisationUserId == Convert.ToInt32(organizationUserId)).Select(col => col.DPM_ID).ToList();
            List<Int32> lstMatchingUserNodes = lstLoggedInUserNodes.Intersect(lstApplicantUserNodes).ToList();

            if (!lstMatchingUserNodes.IsNullOrEmpty())
            {
                Int32 profilePermissionIdForMinimumPermission = Convert.ToInt32(CurrentViewContext.lstUserNodePermissionsContract.Where(cond => lstMatchingUserNodes.Contains(cond.DPM_ID)).Max(col => col.ProfilePermissionID));
                permissionCode = CurrentViewContext.lstUserNodePermissionsContract.Where(cond => cond.ProfilePermissionID == profilePermissionIdForMinimumPermission).Select(col => col.ProfilePermissionCode).FirstOrDefault();
            }
            else if (CurrentViewContext.lstUserNodePermissionsContract.Any(cond => cond.ParentNodeID == null))
            {
                permissionCode = CurrentViewContext.lstUserNodePermissionsContract.Where(cond => cond.ParentNodeID == null).Select(col => col.ProfilePermissionCode).FirstOrDefault();
            }
            else
            {
                permissionCode = LkpPermission.FullAccess.GetStringValue();
            }

            if (string.Compare(permissionCode, LkpPermission.FullAccess.GetStringValue()) == 0)
                return true;
            return false;
        }

        #endregion

        /// <summary>
        /// To bind Admin Program Study dropdown when Tenant Name changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTenantName_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            BindUserGroups();
            if (ddlTenantName.SelectedIndex <= 0)
            {
                //CurrentViewContext.lstArchiveState = null;
                rbSubscriptionState.Visible = false;
                ucCustomAttributeLoaderSearch.Reset();
                fsucCmdBarButton.ClearButton.Visible = false;
                fsucCmdBarButton.ExtraButton.Visible = false;
                ResetGridFilters();
            }
            else
            {
                rbSubscriptionState.Visible = true;
                btnArchieve.Enabled = true;
                Presenter.GetArchiveStateList();
                ucCustomAttributeLoaderSearch.Reset(SelectedTenantId);

            }

            if (grdApplicantSearchData.Items.Count > 0)
            {
                fsucCmdBarButton.ClearButton.Style.Clear();
                fsucCmdBarButton.ExtraButton.Style.Clear();
            }
            else
            {
                fsucCmdBarButton.ClearButton.Style.Add("display", "none");
                fsucCmdBarButton.ExtraButton.Style.Add("display", "none");
            }
        }

        /// <summary>
        /// Tenant Name DataBound event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTenantName_DataBound(object sender, EventArgs e)
        {
            ddlTenantName.Items.Insert(0, new RadComboBoxItem("--Select--"));
        }

        /// <summary>
        /// Tenant Name DataBound event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlUserGroup_DataBound(object sender, EventArgs e)
        {
            ddlUserGroup.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT--"));
        }

        /// <summary>
        /// Handles the selection of users to send message.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        protected void chkSelectUser_CheckedChanged(object sender, EventArgs e)
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
                Dictionary<Int32, String> selectedItems = CurrentViewContext.AssignOrganizationUserIds;
                List<CustomComplianceContract> selectedItems2 = CurrentViewContext.AssignOrganizationUsers; //UAT-4218
                Int32 orgUserID = (Int32)dataItem.GetDataKeyValue("OrganizationUserId");
                String orgUserName = Convert.ToString(dataItem["ApplicantFirstName"].Text) + Convert.ToString(dataItem["ApplicantLastName"].Text);
                isChecked = ((CheckBox)dataItem.FindControl("chkSelectUser")).Checked;
                String ApplicantName = Convert.ToString(dataItem["ApplicantFirstName"].Text) + " " + Convert.ToString(dataItem["ApplicantLastName"].Text); //UAT-4218
                if (isChecked)
                {
                    if (!selectedItems.ContainsKey(orgUserID))
                    {
                        selectedItems.Add(orgUserID, orgUserName);
                        selectedItems2.Add(new CustomComplianceContract { OrganizationUserID = orgUserID, ApplicantName = Convert.ToString(dataItem["ApplicantFirstName"].Text) + " " + Convert.ToString(dataItem["ApplicantLastName"].Text) }); //UAT-4218
                    }
                }
                else
                {
                    if (selectedItems != null && selectedItems.ContainsKey(orgUserID))
                    {
                        selectedItems.Remove(orgUserID);
                        selectedItems2.RemoveAll(x => x.OrganizationUserID == orgUserID); //UAT-4218
                    }
                }
                CurrentViewContext.AssignOrganizationUserIds = selectedItems;
                CurrentViewContext.AssignOrganizationUsers = selectedItems2; //UAT-4218
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

        #region Private Methods

        /// <summary>
        /// To bind controls
        /// </summary>
        private void BindControls()
        {
            ddlTenantName.DataSource = CurrentViewContext.lstTenant;
            ddlTenantName.DataBind();

            if (Presenter.IsDefaultTenant)
            {
                ddlTenantName.Enabled = true;
                CurrentViewContext.SelectedTenantId = 0;
                fsucCmdBarButton.ClearButton.Style.Add("display", "none");
                fsucCmdBarButton.ExtraButton.Style.Add("display", "none");
            }
            else
            {
                CurrentViewContext.SelectedTenantId = CurrentViewContext.TenantId;
                fsucCmdBarButton.ClearButton.Style.Clear();
                fsucCmdBarButton.ExtraButton.Style.Clear();
                rbSubscriptionState.Visible = true;
                Presenter.GetArchiveStateList();
            }
            BindUserGroups();
        }

        /// <summary>
        /// To bind program dropdown
        /// </summary>
        private void BindUserGroups()
        {
            Presenter.GetAllUserGroups();
            ddlUserGroup.DataSource = CurrentViewContext.lstUserGroup;
            ddlUserGroup.DataBind();
        }

        /// <summary>
        /// To set controls values in session
        /// </summary>
        private void SetSessionValues()
        {
            SearchItemDataContract searchDataContract = new SearchItemDataContract();

            searchDataContract.IsBackToSearch = true;

            searchDataContract.ClientID = SelectedTenantId;
            searchDataContract.ApplicantFirstName = ApplicantFirstName;
            searchDataContract.ApplicantLastName = ApplicantLastName;
            searchDataContract.EmailAddress = EmailAddress;
            searchDataContract.DateOfBirth = DateOfBirth;
            searchDataContract.ApplicantSSN = SSN;
            searchDataContract.OrganizationUserId = OrganizationUserID.Value;
            //UAT-1055: WB: As an admin, I should be able to search multiple nodes at a time.
            //searchDataContract.DPM_Id = ucCustomAttributeLoaderSearch.DPM_ID;
            searchDataContract.SelectedDPMIds = ucCustomAttributeLoaderSearch.DPM_ID;
            searchDataContract.CustomFields = ucCustomAttributeLoaderSearch.GetCustomDataXML();
            searchDataContract.NodeLabel = ucCustomAttributeLoaderSearch.nodeLable;
            //UAT-1055: WB: As an admin, I should be able to search multiple nodes at a time.
            //searchDataContract.NodeId = ucCustomAttributeLoaderSearch.NodeId;
            searchDataContract.NodeIds = ucCustomAttributeLoaderSearch.NodeIds;
            searchDataContract.GridCustomPagingArguments = GridCustomPaging;
            if (ddlUserGroup.SelectedIndex > 0)
            {
                searchDataContract.FilterUserGroupID = Convert.ToInt32(ddlUserGroup.SelectedValue);
                searchDataContract.MatchUserGroupID = searchDataContract.FilterUserGroupID;
            }
            searchDataContract.LstArchiveState = CurrentViewContext.SelectedArchiveStateCode;
            var serializer = new XmlSerializer(typeof(SearchItemDataContract));
            var strbuilder = new StringBuilder();

            using (TextWriter writer = new StringWriter(strbuilder))
            {
                serializer.Serialize(writer, searchDataContract);
            }
            //Session for maintaining control values
            Session[AppConsts.APPLICANT_SEARCH_SESSION_KEY] = strbuilder.ToString();
        }

        /// <summary>
        /// To get session values for controls
        /// </summary>
        private void GetSessionValues()
        {
            var serializer = new XmlSerializer(typeof(SearchItemDataContract));
            SearchItemDataContract searchDataContract = new SearchItemDataContract();
            if (Session[AppConsts.APPLICANT_SEARCH_SESSION_KEY].IsNotNull())
            {
                TextReader reader = new StringReader(Convert.ToString(Session[AppConsts.APPLICANT_SEARCH_SESSION_KEY]));
                searchDataContract = (SearchItemDataContract)serializer.Deserialize(reader);

                SelectedTenantId = searchDataContract.ClientID;
                ApplicantFirstName = searchDataContract.ApplicantFirstName;
                ApplicantLastName = searchDataContract.ApplicantLastName;
                EmailAddress = searchDataContract.EmailAddress;
                DateOfBirth = searchDataContract.DateOfBirth;
                //SSN = searchDataContract.ApplicantSSN;

                if (CurrentViewContext.SSNPermissionCode.ToUpper() == EnumSystemPermissionCode.MASKED_READ_PERMISSION.GetStringValue().ToUpper())
                {
                    if (!searchDataContract.ApplicantSSN.IsNullOrEmpty())
                    {
                        CurrentViewContext.SSN = searchDataContract.ApplicantSSN.Substring(searchDataContract.ApplicantSSN.Length - AppConsts.FOUR);
                        txtSSN.Text = CurrentViewContext.SSN;
                    }
                }
                else
                {
                    CurrentViewContext.SSN = searchDataContract.ApplicantSSN;
                    txtSSN.Text = CurrentViewContext.SSN;
                }

                if (searchDataContract.OrganizationUserId != null && searchDataContract.OrganizationUserId > 0)
                    OrganizationUserID = searchDataContract.OrganizationUserId;
                CustomFields = searchDataContract.CustomFields;
                //UAT-1055: WB: As an admin, I should be able to search multiple nodes at a time.
                //DPM_ID = searchDataContract.DPM_Id ?? 0;
                //ucCustomAttributeLoaderSearch.DPM_ID = Convert.ToInt32(searchDataContract.DPM_Id);
                DPM_IDs = searchDataContract.SelectedDPMIds;
                ucCustomAttributeLoaderSearch.DPM_ID = searchDataContract.SelectedDPMIds;
                ucCustomAttributeLoaderSearch.previousValues = CustomFields;
                ucCustomAttributeLoaderSearch.TenantId = searchDataContract.ClientID;
                ucCustomAttributeLoaderSearch.nodeLable = searchDataContract.NodeLabel;
                //UAT-1055: WB: As an admin, I should be able to search multiple nodes at a time.
                //ucCustomAttributeLoaderSearch.NodeId = searchDataContract.NodeId.IsNotNull() ? Convert.ToInt32(searchDataContract.NodeId) : 0;
                ucCustomAttributeLoaderSearch.NodeIds = searchDataContract.NodeIds;
                ucCustomAttributeLoaderSearch.ScreenType = "CommonScreen";
                BindUserGroups();
                FilterUserGroupId = searchDataContract.FilterUserGroupID ?? 0;
                MatchUserGroupId = searchDataContract.MatchUserGroupID ?? 0;
                CurrentViewContext.GridCustomPaging = searchDataContract.GridCustomPagingArguments;
                //Changes related to UAT-1456.
                if (!CurrentViewContext.GridCustomPaging.SortExpression.IsNullOrEmpty())
                {
                    GridSortExpression gridSortExpression = new GridSortExpression();
                    gridSortExpression.FieldName = CurrentViewContext.GridCustomPaging.SortExpression;
                    gridSortExpression.SortOrder = CurrentViewContext.GridCustomPaging.SortDirectionDescending ? GridSortOrder.Descending : GridSortOrder.Ascending;
                    grdApplicantSearchData.MasterTableView.SortExpressions.Add(gridSortExpression);
                }
                //if SelectedTenantId has value i.e greater than 0,Send Message button will be visible.
                if (!SelectedTenantId.IsNullOrEmpty() && SelectedTenantId > 0)
                {
                    fsucCmdBarButton.ClearButton.Style.Clear();
                    fsucCmdBarButton.ExtraButton.Style.Clear();
                }
                if (searchDataContract.ClientID != 0)
                {
                    Presenter.GetArchiveStateList();
                }
                if (searchDataContract.LstArchiveState.IsNotNull() && searchDataContract.LstArchiveState.Count > 0)
                {
                    rbSubscriptionState.SelectedValue = searchDataContract.LstArchiveState.FirstOrDefault();
                }
                else if (searchDataContract.LstArchiveState.IsNotNull() && searchDataContract.LstArchiveState.Count == 0)
                {
                    rbSubscriptionState.SelectedValue = ArchiveState.All.GetStringValue();
                }
                if (rbSubscriptionState.SelectedValue == ArchiveState.Archived.GetStringValue())
                {
                    btnArchieve.Enabled = false;
                }
                //Reset session
                Session[AppConsts.APPLICANT_SEARCH_SESSION_KEY] = null;
            }
        }

        /// <summary>
        /// Removes all the filters and Sorting on the grid and clears the variables.
        /// </summary>
        private void ResetGridFilters()
        {
            grdApplicantSearchData.MasterTableView.SortExpressions.Clear();
            grdApplicantSearchData.CurrentPageIndex = 0;
            grdApplicantSearchData.MasterTableView.CurrentPageIndex = 0;
            grdApplicantSearchData.Rebind();
        }

        /// <summary>
        /// If subscriptions archived successfully from popup
        /// </summary>
        private void SubscriptionsArchivedFromPopup()
        {
            //If subscriptions archived successfully from popup
            if (hdnArchivedFromPopup.Value == "true")
            {
                base.ShowSuccessMessage("Subscription(s) archived successfully.");
                hdnArchivedFromPopup.Value = "false";
                //Need to retain items selected
                //CurrentViewContext.AssignOrganizationUserIds = new Dictionary<Int32, String>();
            }

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
                grdApplicantSearchData.MasterTableView.GetColumn("DateOfBirth").Visible = false;
            }
            if (CurrentViewContext.SSNPermissionCode.ToUpper() == EnumSystemPermissionCode.NO_ACCESS_PERMISSION.GetStringValue().ToUpper())
            {
                divSSN.Visible = false;
                grdApplicantSearchData.MasterTableView.GetColumn("SSN").Visible = false;
                //Hide Masked column if user does not have permission to view SSN Column.
                grdApplicantSearchData.MasterTableView.GetColumn("_SSN").Visible = false;
            }
            //else if (CurrentViewContext.SSNPermissionCode.ToUpper() == EnumSystemPermissionCode.MASKED_READ_PERMISSION.GetStringValue().ToUpper())
            //{
            //    txtSSN.Mask = AppConsts.SSN_MASK_FORMATE; //@"\#\#\#-\#\#-####"
            //}

            // UAT-3010:-  Granular Permission for Client Admin Users to Archive.
            if (CurrentViewContext.ArchivePermissionCode.ToUpper() == EnumSystemPermissionCode.NO_ACCESS_PERMISSION.GetStringValue().ToUpper())
            {
                btnArchieve.Visible = false;
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
        #endregion

        #region UAT-1261: WB: As an ADB admin, I should be able to "login" as any student to see what they see.

        /// <summary>
        /// Method to switch to Applicant View
        /// </summary>
        private void SwitchToApplicant(String applicantUserID, Int32 tenantID)
        {
            String switchingTargetURL = Presenter.GetSwitchingTargetUrl(tenantID);
            RedirectToTargetSwitchingView(tenantID, applicantUserID, switchingTargetURL);
        }

        /// <summary>
        /// Method To create/update WebApplicationData, Redirect to Target applicant View.
        /// </summary>
        /// <param name="tenantID"></param>
        /// <param name="switchingTargetURL"></param>
        private void RedirectToTargetSwitchingView(Int32 tenantID, String applicantUserID, String switchingTargetURL)
        {
            Dictionary<String, ApplicantInsituteDataContract> applicantData = new Dictionary<String, ApplicantInsituteDataContract>();
            ApplicantInsituteDataContract appInstData = new ApplicantInsituteDataContract();
            appInstData.UserID = applicantUserID;
            appInstData.TagetInstURL = switchingTargetURL;
            appInstData.TokenCreatedTime = DateTime.Now;
            appInstData.TenantID = tenantID;
            appInstData.UserTypeSwitchViewCode = UserTypeSwitchView.Applicant.GetStringValue();
            appInstData.AdminOrgUserID = CurrentLoggedInUserId;
            String key = Guid.NewGuid().ToString();

            Dictionary<String, ApplicantInsituteDataContract> applicationData = Presenter.GetDataByKey("ApplicantInstData");
            if (applicationData != null)
            {
                applicantData = applicationData;
                applicantData.Add(key, appInstData);
                Presenter.UpdateWebApplicationData("ApplicantInstData", applicantData);
            }
            else
            {
                applicantData.Add(key, appInstData);
                Presenter.AddWebApplicationData("ApplicantInstData", applicantData);
            }

            //Log out from application then redirect to selected tenant url, append key in querystring.
            // On login page get data from Application Variable.
            //Presenter.DoLogOff(true);

            //Redirect to login page
            Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "TokenKey", key  }
                                                                 };

            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "OpenApplicantView('" + String.Format(appInstData.TagetInstURL + "/Login.aspx?TokenKey={0}&DeletePrevUsrState=true", key) + "');", true);
            //Response.Redirect(String.Format(appInstData.TagetInstURL + "/Login.aspx?TokenKey={0}", key));
        }

        #endregion

        #endregion

        #region Apply Permission

        public override List<String> ChildScreenPathCollection
        {
            get
            {
                List<String> childScreenPathCollection = new List<String>();
                childScreenPathCollection.Add(@"~/SearchUI/UserControl/ApplicantPortfolioDetails.ascx");
                return childScreenPathCollection;
            }
        }

        #endregion

        #region UAT 696
        protected void btnReportViewer_Click(object sender, EventArgs e)
        {
            if (CurrentViewContext.AssignOrganizationUserIds.IsNotNull() && !CurrentViewContext.AssignOrganizationUserIds.Any())
            {
                base.ShowErrorInfoMessage("Please select user(s) to generate Report.");
            }
            else
            { 
                //Get the list of Selected Subscription IDs
                Presenter.FetchSelectedSubscriptionIDs();
                Session["SubscriptionIDs"] = String.Empty;
                Session["SubscriptionIDs"] = string.Join(",", ListSubscriptionIds.Select(n => n.ToString()).ToArray());
                //START UAT-4218
                CurrentViewContext.ApplicantName = String.Empty;
                foreach (var item in CurrentViewContext.AssignOrganizationUsers)
                {
                    if (!(CurrentViewContext.lstPackageSubscription.Any(x => x.OrganizationUserID == item.OrganizationUserID)))
                    {
                        CurrentViewContext.ApplicantName = CurrentViewContext.ApplicantName + "," + item.ApplicantName;
                    }
                }
                //END UAT
                Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { AppConsts.CHILD, @"~/ComplianceOperations/Reports/ReportViewer.aspx"},
                                                                    { "TenantID", Convert.ToString(SelectedTenantId)},
                                                                    {"ApplicantName",CurrentViewContext.ApplicantName},  //UAT-4218
                                                                 };
                String redirectUrl = "ComplianceOperations/Reports/ReportViewer.aspx?args={0}";
                hdnSubscriptionIds.Value = String.Format(redirectUrl, queryString.ToEncryptedQueryString());
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "OpenReportPopup();", true);
            }
        }


        public List<Int32> ListSubscriptionIds
        {
            get;
            set;
        }
        #endregion

        protected void btnArchieve_Click(object sender, EventArgs e)
        {
            try
            {
                if (CurrentViewContext.AssignOrganizationUserIds.IsNotNull() && !CurrentViewContext.AssignOrganizationUserIds.Any())
                {
                    base.ShowErrorInfoMessage("Please select user(s) for archiving.");
                }
                else
                {
                    //Code to open popup if selected applicants have more than one subscription
                    Presenter.GetSubscriptionsListForArchival();
                    if (CurrentViewContext.DicSubscriptionIDs.IsNotNull() && CurrentViewContext.DicSubscriptionIDs.ContainsKey("lstMultipleSubscriptions"))
                    {
                        hdnCurrentUserID.Value = CurrentViewContext.CurrentLoggedInUserId.ToString();
                        hdnSelectedTenantID.Value = CurrentViewContext.SelectedTenantId.ToString();
                        Session["SubscriptionIDs"] = DicSubscriptionIDs;
                        //SetSessionValues();
                        //openpopup;
                        System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "OpenMutlipleSubscriptionsPopup();", true);
                    }
                    else
                    {
                        //Get the list of Selected Subscription IDs
                        //Presenter.FetchSelectedSubscriptionIDs(); //TO-DO Add Selected Applicants from popup
                        if (CurrentViewContext.DicSubscriptionIDs.IsNotNull() && CurrentViewContext.DicSubscriptionIDs.ContainsKey("lstSingleSubscriptions"))
                        {
                            CurrentViewContext.ListSubscriptionIds = CurrentViewContext.DicSubscriptionIDs.GetValue("lstSingleSubscriptions");
                            String result = Presenter.ArchieveSubscriptions();
                            if (result == "true")
                            {
                                base.ShowSuccessMessage("Subscriptions archived sucessfully.");
                                Presenter.SetQueueImaging(); //UAT-2422-Resync the data to flat tables
                                CurrentViewContext.AssignOrganizationUserIds = new Dictionary<Int32, String>();
                                CurrentViewContext.AssignOrganizationUsers = new List<CustomComplianceContract>(); //UAT:4218
                                grdApplicantSearchData.Rebind();
                            }
                            else if (result == "The selected user(s) does not have any active subscription.")
                            {
                                base.ShowInfoMessage(result);
                            }
                            else
                            {
                                base.ShowErrorMessage("Subscriptions are not archived sucessfully. Please try again.");
                            }
                        }
                        else
                        {
                            base.ShowInfoMessage("The selected user(s) does not have any active subscription.");
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

        protected void rbSubscriptionState_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rbSubscriptionState.SelectedValue == ArchiveState.Archived.GetStringValue())
                btnArchieve.Enabled = false;
            else
                btnArchieve.Enabled = true;

            chkSelectAllResults.Focus();
        }

        protected void chkSelectAllResults_CheckedChanged(object sender, EventArgs e)
        {
            if (CurrentViewContext.VirtualRecordCount > 0)
            {
                bool needToCheckboxChecked = false;

                if (((CheckBox)sender).Checked)
                {
                    Presenter.GetAllOrganisationUserIds();
                    needToCheckboxChecked = true;
                }
                else
                {
                    CurrentViewContext.AssignOrganizationUserIds = new Dictionary<int, string>();
                    CurrentViewContext.AssignOrganizationUsers = new List<CustomComplianceContract>();
                }

                foreach (GridDataItem item in grdApplicantSearchData.Items)
                {
                    CheckBox checkBox = ((CheckBox)item.FindControl("chkSelectUser"));
                    checkBox.Checked = needToCheckboxChecked;
                }

                GridHeaderItem headerItem = grdApplicantSearchData.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
                CheckBox headerCheckBox = ((CheckBox)headerItem.FindControl("chkSelectAll"));
                headerCheckBox.Checked = needToCheckboxChecked;
            }
            chkSelectAllResults.Focus();
        }



    }
}
