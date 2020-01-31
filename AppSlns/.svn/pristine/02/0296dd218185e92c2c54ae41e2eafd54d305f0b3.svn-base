#region Namespaces

#region System Defined Namespaces

using System;
using System.Linq;
using Microsoft.Practices.ObjectBuilder;
using System.Collections.Generic;
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
using System.Xml;
using CoreWeb.IntsofSecurityModel;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.UI.Contract.CommonControls;

#endregion

#endregion

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class ComplianceSearchControl : BaseUserControl, IComplianceSearchControlView
    {
        #region Variables

        #region Public Variables
        #endregion

        #region Private Variables

        private ComplianceSearchControlPresenter _presenter = new ComplianceSearchControlPresenter();
        private String _viewType;
        Int32 tenantId = 0;
        private CustomPagingArgsContract _itemDataGridCustomPaging = null;
        private SearchItemDataContract _searchItemDataContract = null;
        private SearchItemDataContract _gridSearchContract = null;
        private List<String> _lstCodeForColumnConfig = new List<String>(); // UAT-2675


        #endregion

        #endregion

        #region Events

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            CurrentViewContext.SSNnumber = txtSSNNumber.TextWithPrompt;
            _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
            if (!this.IsPostBack)
            {
                //Set MinDate and MaxDate for DOB
                dpkrDOB.MinDate = Convert.ToDateTime("01-01-1900");
                dpkrDOB.MaxDate = DateTime.Now;

                if (!Request.QueryString["args"].IsNull())
                {
                    Args = new Dictionary<string, string>();
                    Args.ToDecryptedQueryString(Request.QueryString["args"]);
                }

                Presenter.OnViewInitialized();
                Init();
                ApplySSNMask();
                ShowHideControls(false);
                SetThePreviousValues();
                /*Start UAT-3157*/
                CurrentViewContext.IsDefaultPreferredTenant = false;
                GetPreferredSelectedTenant(false);
                /*End UAT-3157*/
            }
            SetValuesForTenant();
            Presenter.OnViewLoaded();
            //fsucCmdBar1.SubmitButton.CausesValidation = false;
            //(fsucCmdBar1 as CoreWeb.Shell.Views.CommandBar).SaveButton.ToolTip = "Click to search orders per the criteria entered above";
            //(fsucCmdBar1 as CoreWeb.Shell.Views.CommandBar).SubmitButton.ToolTip = "Click to remove all values entered in the search criteria above";
            //(fsucCmdBar1 as CoreWeb.Shell.Views.CommandBar).CancelButton.ToolTip = "Click to cancel. Any data entered will not be saved";
            HideShowControlsForGranularPermission();//UAT-806

            ManageColumnConfiguration();  //UAT-2675
        }

        private void Init()
        {
            ddlTenantName.DataSource = CurrentViewContext.lstTenant;
            ddlTenantName.DataBind();
            ddlTenantName.SelectedValue = TenantId.ToString();
            CurrentViewContext.SelectedTenantId = 0;
            if (Presenter.IsDefaultTenant || Presenter.IsThirdPartyTenant)
            {
                ddlTenantName.Enabled = true;
                if (Args != null && Args.ContainsKey("SelectedTenantId"))
                    CurrentViewContext.SelectedTenantId = Convert.ToInt32(Args["SelectedTenantId"]);

                ddlTenantName.SelectedIndex = CurrentViewContext.SelectedTenantId;

            }
            //if (Presenter.IsDefaultTenant)
            //{
            //    //fsucCmdBar1.ClearButton.Style.Add("display", "none");
            //    //fsucCmdBar1.ExtraButton.Style.Add("display", "none");
            //    ShowHideMenuButton("SendMessagemun", "btnSendMessage", false);
            //    ShowHideMenuButton("PassportReportmun", "btnPassportReport", false);
            //}

            //else
            //{
            //    CurrentViewContext.SelectedTenantId = Convert.ToInt32(ddlTenantName.SelectedValue);
            //    //fsucCmdBar1.ClearButton.Style.Clear();
            //    //fsucCmdBar1.ExtraButton.Style.Clear();
            //    ShowHideMenuButton("SendMessagemun", "btnSendMessage", true);
            //    ShowHideMenuButton("PassportReportmun", "btnPassportReport", true);
            //    rbSubscriptionState.Visible = true;
            //    Presenter.GetArchiveStateList();
            //}

            lblVerificationQueue.Text = "Manage Compliance Search";
            BasePage page = (base.Page) as BasePage;

            page.SetModuleTitle("Manage Search");
            BindItemComplianceStatus();
            BindOverAllComplianceStatus();
            BindCategoryComplianceStatus();
            BindUserGroups();
            chkItemStatus.Visible = true;

            if (Args != null && Args.ContainsKey("ApplicantFirstName"))
                CurrentViewContext.ApplicantFirstName = Args["ApplicantFirstName"];

            if (Args != null && Args.ContainsKey("ApplicantLastName"))
                CurrentViewContext.ApplicantLastName = Args["ApplicantLastName"];

            DateTime dob;
            if (Args != null && Args.ContainsKey("DateOfBirth") && DateTime.TryParse(Args["DateOfBirth"], out dob))
                CurrentViewContext.DateOfBirth = dob;

            if (Args != null && Args.ContainsKey("SelectedItemComplianceStatusId") && !String.IsNullOrEmpty(Args["SelectedItemComplianceStatusId"]))
            {
                SelectedItemComplianceStatusId = new List<int>(Args["SelectedItemComplianceStatusId"].Split(',').Select(int.Parse));
            }
            ActionType = ViewMode.Search.ToString();
        }

        #endregion

        #region Button Events

        protected void btnAddNotes_Click(object sender, EventArgs e)
        {
            RadButton btn = sender as RadButton;
            GridDataItem parentItem = btn.NamingContainer as GridDataItem;
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "key", "openAddNotesPopUp(" + btn.ClientID + ");", true);
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

        protected void CmdBarSearch_Click(object sender, EventArgs e)
        {
            //To reset AssignOrganizationUserIds
            CurrentViewContext.AssignOrganizationUserIds = new Dictionary<Int32, String>();
            CurrentViewContext.ListSubscriptionIds = new Dictionary<Int32, Int32>();
            ColumnsConfiguration.BindPageControls(); //UAT-2675

            ActionType = ViewMode.Search.ToString();
            grdItemData.MasterTableView.CurrentPageIndex = 0;
            //ItemDataGridCustomPaging.CurrentPageIndex = CurrentPageIndex;
            ItemDataGridCustomPaging.PageSize = PageSize;
            ItemDataGridCustomPaging.FilterColumns = SearchItemDataContract.FilterColumns;
            ItemDataGridCustomPaging.FilterOperators = SearchItemDataContract.FilterOperators;
            ItemDataGridCustomPaging.FilterValues = SearchItemDataContract.FilterValues;
            //Presenter.PerformSearch();
            ShowHideControls(true);
            grdItemData.Rebind();

            //if (grdItemData.Items.Count > 0)
            //{
            //    //fsucCmdBar1.ClearButton.Style.Clear();
            //    //fsucCmdBar1.ExtraButton.Style.Clear();

            //    ShowHideMenuButton("SendMessagemun", "btnSendMessage", true);
            //    ShowHideMenuButton("PassportReportmun", "btnPassportReport", true);
            //}
            //else
            //{
            //    //fsucCmdBar1.ClearButton.Style.Add("display", "none");
            //    //fsucCmdBar1.ExtraButton.Style.Add("display", "none");
            //    ShowHideMenuButton("SendMessagemun", "btnSendMessage", false);
            //    ShowHideMenuButton("PassportReportmun", "btnPassportReport", false);
            //}

            chkSelectAllResults.Checked = false;
        }

        public String CustomDataXML
        {
            get
            {
                return ucCustomAttributeLoader.GetCustomDataXML();
            }
        }

        public String DPM_Ids
        {
            get
            {
                return ucCustomAttributeLoader.DPM_ID;
            }
            set
            {
                ucCustomAttributeLoader.DPM_ID = value;
            }
        }

        public String NodeLable
        {
            get
            {
                return ucCustomAttributeLoader.nodeLable;
            }
            set
            {
                ucCustomAttributeLoader.nodeLable = value;
            }
        }

        public List<lkpArchiveState> lstArchiveState
        {
            set
            {
                dvRecords.Visible = false;
                rblOrderState.DataSource = value.OrderBy(x => x.AS_Code);
                rblOrderState.DataBind();
                rblOrderState.SelectedValue = ArchiveState.All.GetStringValue();
                rbSubscriptionState.DataSource = value.OrderBy(x => x.AS_Code);
                rbSubscriptionState.DataBind();
                rbSubscriptionState.SelectedValue = ArchiveState.Active.GetStringValue();//value.Where(x => x.AS_Code == lkpArchivalState.NonArchived.GetStringValue()).Select(x=>x.AS_Code).ToString();

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

        String IComplianceSearchControlView.SelectedExpiryStateCode
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
                        selectedCodes = rbSubscriptionExpiryState.SelectedValue;
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
        public String NodeIds
        {
            get
            {
                return ucCustomAttributeLoader.NodeIds;
            }
            set
            {
                ucCustomAttributeLoader.NodeIds = value;
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

        protected void CmdBarReset_Click(object sender, EventArgs e)
        {
            CurrentViewContext.ViewStateSearchData = null;
            Presenter.GetTenants();
            CurrentViewContext.SelectedItemComplianceStatusId = new List<int>();
            CurrentViewContext.SelectedCategoryComplianceStatusId = new List<int>();
            //UAT-2675
            ColumnsConfiguration.BindPageControls();
            if (!CurrentViewContext.IsDefaultPreferredTenant)  //UAT-3157, check to restrict uc controls to get hide and resets.
            {
                if (Presenter.IsDefaultTenant || Presenter.IsThirdPartyTenant)
                {
                    ucCustomAttributeLoader.Reset();
                }
                else
                {
                    ucCustomAttributeLoader.ResetControlData(true);
                }
            }
            Init();
            txtOrderId.Text = String.Empty;
            txtSSNNumber.Text = String.Empty;
            SSNnumber = null;
            txtFirstName.Text = String.Empty;
            txtLastName.Text = String.Empty;
            dpkrDOB.SelectedDate = null;
            ActionType = ViewMode.Search.ToString();
            rblOrderState.SelectedValue = null;
            dvRecords.Visible = false;
            rbSubscriptionState.SelectedValue = ArchiveState.Active.GetStringValue();
            //btnArchieve.Enabled = true;    
            ShowHideControl("Archivemun", "btnArchive", true);
            //UAT-4256
            radMenubtnArchUnArch.Visible = true;
            btnUnArch.Visible = false;
            //rbSubscriptionState.Visible = false;
            VirtualPageCount = 0;
            //Commented code for UAT-1456.
            //ViewState["SortExpression"] = null;
            //ViewState["SortDirection"] = null;
            grdItemData.MasterTableView.SortExpressions.Clear();
            //ddlTenantName.SelectedIndex = 0;
            //ddlTenantName.SelectedIndex = 0;
            /*Start UAT-3157*/
            GetPreferredSelectedTenant(true);
            /*End UAT-3157*/
            //grdItemData.Rebind();
            ShowHideControls(false);
            //To reset AssignOrganizationUserIds
            CurrentViewContext.AssignOrganizationUserIds = new Dictionary<Int32, String>();
            CurrentViewContext.ListSubscriptionIds = new Dictionary<Int32, Int32>();
            //if (!CurrentViewContext.IsDefaultPreferredTenant)
            //{
            //    if (Presenter.IsDefaultTenant)
            //    {
            //        //fsucCmdBar1.ClearButton.Style.Add("display", "none");
            //        //fsucCmdBar1.ExtraButton.Style.Add("display", "none");

            //        ShowHideMenuButton("SendMessagemun", "btnSendMessage", false);
            //        ShowHideMenuButton("PassportReportmun", "btnPassportReport", false);
            //        rbSubscriptionState.Visible = false;
            //    }
            //    else
            //    {
            //        //fsucCmdBar1.ClearButton.Style.Clear();
            //        //fsucCmdBar1.ExtraButton.Style.Clear();

            //        ShowHideMenuButton("SendMessagemun", "btnSendMessage", true);
            //        ShowHideMenuButton("PassportReportmun", "btnPassportReport", true);
            //        rbSubscriptionState.Visible = true;
            //    }
            //}
            //rbSubscriptionState.SelectedValue = null;

            CurrentViewContext.ItemDataGridCustomPaging.VirtualPageCount = 0;
            chkSelectAllResults.Checked = false;
        }

        protected void CmdBarCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(String.Format(AppConsts.DASHBOARD_PAGE_NAME));
        }

        /// <summary>
        /// To View Report
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnViewReport_Click(object sender, EventArgs e)
        {
            try
            {
                if (CurrentViewContext.ListSubscriptionIds.IsNotNull() && !CurrentViewContext.ListSubscriptionIds.Any())
                {
                    base.ShowErrorInfoMessage("Please select user(s) to generate report.");
                }
                else
                {
                    //Save Selected Subscription IDs in Session Variable
                    Session["SubscriptionIDs"] = String.Empty;
                    Session["SubscriptionIDs"] = string.Join(",", ListSubscriptionIds.Keys.Select(n => n.ToString()).ToArray());
                    Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 {
                                                                    { AppConsts.CHILD, @"~/ComplianceOperations/Reports/ReportViewer.aspx"},
                                                                    { "TenantID", Convert.ToString(SelectedTenantId)},
                                                                     { "IsApprovedItemsReport", "0"},
                                                                 };
                    String redirectUrl = "ComplianceOperations/Reports/ReportViewer.aspx?args={0}";
                    hdnSubscriptionIds.Value = String.Format(redirectUrl, queryString.ToEncryptedQueryString());
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "OpenReportPopup();", true);
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
        protected void btnPassportReportApprovedItems_Click(object sender, EventArgs e)
        {
            try
            {
                if (CurrentViewContext.ListSubscriptionIds.IsNotNull() && !CurrentViewContext.ListSubscriptionIds.Any())
                {
                    base.ShowErrorInfoMessage("Please select user(s) to generate report.");
                }
                else
                {
                    //Save Selected Subscription IDs in Session Variable
                    Session["SubscriptionIDs"] = String.Empty;
                    Session["SubscriptionIDs"] = string.Join(",", ListSubscriptionIds.Keys.Select(n => n.ToString()).ToArray());
                    Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 {
                                                                    { AppConsts.CHILD, @"~/ComplianceOperations/Reports/ReportViewer.aspx"},
                                                                    { "TenantID", Convert.ToString(SelectedTenantId)},
                                                                    { "IsApprovedItemsReport", "1"},
                                                                 };
                    String redirectUrl = "ComplianceOperations/Reports/ReportViewer.aspx?args={0}";
                    hdnSubscriptionIds.Value = String.Format(redirectUrl, queryString.ToEncryptedQueryString());
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "OpenReportPopup();", true);
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

        protected void btnArchieve_Click(object sender, EventArgs e)
        {
            try
            {
                if (CurrentViewContext.ListSubscriptionIds.IsNotNull() && !CurrentViewContext.ListSubscriptionIds.Any())
                {
                    base.ShowErrorInfoMessage("Please select subscriptions for archiving.");
                }
                else
                {
                    String result = Presenter.ArchieveSubscriptions();
                    if (result == "true")
                    {
                        base.ShowSuccessMessage("Subscriptions archived successfully.");
                        Presenter.SetQueueImaging(); //UAT-2422-Resync data to flat tables
                        grdItemData.Rebind();
                        UncheckGridItemsOnArchiveUnarchive(); //UAT-3162 - Refresh filtered results after archiving/unarchiving on compliance search
                        CurrentViewContext.ListSubscriptionIds = null;
                    }
                    else if (result == "The selected user(s) does not have any active subscription.")
                    {
                        base.ShowInfoMessage(result);
                    }
                    else
                    {
                        base.ShowErrorMessage("Subscriptions are not archived successfully. Please try again.");
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
        protected void btnUnArchive_Click(object sender, EventArgs e)
        {
            try
            {
                if (CurrentViewContext.ListSubscriptionIds.IsNotNull() && !CurrentViewContext.ListSubscriptionIds.Any())
                {
                    base.ShowErrorInfoMessage("Please select subscriptions for un-archiving.");
                }
                else
                {
                    Boolean result = Presenter.UnArchiveSubscription();
                    if (result)
                    {
                        base.ShowSuccessMessage("Subscriptions un-archived successfully.");
                        Presenter.SetQueueImaging(); //UAT-2422-Resync data to flat tables                       
                        grdItemData.Rebind();
                        UncheckGridItemsOnArchiveUnarchive();//UAT-3162 - Refresh filtered results after archiving/unarchiving on compliance search
                        CurrentViewContext.ListSubscriptionIds = null;
                    }
                    else
                    {
                        base.ShowInfoMessage("Subscriptions are not un-archived successfully. Please try again.");
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

        protected void grdItemData_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            CurrentViewContext.ItemDataGridCustomPaging.DefaultSortExpression = QueueConstants.DEFAULT_SORTING_FIELDS_COMPLIANCE_SEARCH;
            CurrentViewContext.ItemDataGridCustomPaging.PageSize = PageSize;
            CurrentViewContext.ItemDataGridCustomPaging.CurrentPageIndex = CurrentPageIndex;
            //Commented code for UAT-1456.
            //CurrentViewContext.ItemDataGridCustomPaging.SortExpression = Convert.ToString(ViewState["SortExpression"]);
            //CurrentViewContext.ItemDataGridCustomPaging.SortDirectionDescending = Convert.ToBoolean(ViewState["SortDirection"]);
            Presenter.PerformSearch();
            grdItemData.DataSource = CurrentViewContext.ItemData.IsNullOrEmpty() ? new List<ComplianceRecord>() : CurrentViewContext.ItemData;
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

                    Int32 _itemDataId = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ApplicantComplianceItemID"]);
                    Int32 _CategoryId = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ComplianceCategoryID"]);
                    Int32 _packageId = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CompliancePackageID"]);
                    Int32 _applicantId = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ApplicantId"]);

                    //Int32 _selectedCategoryId = Convert.ToInt32((e.Item.FindControl("hdfCatId") as HiddenField).Value);
                    Int32 _selectedPackageSubscriptionId = Convert.ToInt32((e.Item.FindControl("hdfPackSubscriptionId") as HiddenField).Value);
                    String _selectedArchiveStateCode = String.Empty;
                    if (!CurrentViewContext.SelectedArchiveStateCode.IsNullOrEmpty())
                    {
                        _selectedArchiveStateCode = CurrentViewContext.SelectedArchiveStateCode.FirstOrDefault();
                    }

                    #region UAT-4067
                    Presenter.GetSelectedNodeIDBySubscriptionID(CurrentViewContext.SelectedTenantId, _selectedPackageSubscriptionId);
                    Presenter.GetAllowedFileExtensions();
                    #endregion


                    //UAT-2460 : Empty Session Value
                    Session["SelectedArchiveStateCode"] = null;
                    queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "TenantId", Presenter.ClientId.ToString() },
                                                                    { "Child", ChildControls.VerificationDetailsNew},
                                                                    { "ItemDataId",Convert.ToString( _itemDataId)},
                                                                    {"WorkQueueType",WorkQueueType.ComplianceSearch.ToString()},
                                                                    {"PackageId",Convert.ToString(_packageId)},
                                                                    {"CategoryId",Convert.ToString(_CategoryId)},
                                                                   // {"SelectedProgramStudyId",SelectedProgramStudyId.ToString()},
                                                                    {"ApplicantFirstName",ApplicantFirstName},
                                                                    {"ApplicantLastName",ApplicantLastName},
                                                                    {"DateOfBirth",DateOfBirth.ToString()},

                                                                    //{"ItemLabel",ItemLabel},
                                                                    {"SelectedItemComplianceStatusId",String.Join(",", SelectedItemComplianceStatusId.ToArray()) },
                                                                     {"SelectedPackageSubscriptionId",Convert.ToString( _selectedPackageSubscriptionId)},
                                                                    {"SelectedComplianceCategoryId",Convert.ToString(_CategoryId)},
                                                                    {"ShowOnlyRushOrders","false"},
                                                                    {"ApplicantId",Convert.ToString(_applicantId)},
                                                                    {"SelectedArchiveStateCode",_selectedArchiveStateCode}
                                                                     ,{"allowedFileExtensions", String.Join(",",CurrentViewContext.allowedFileExtensions)} //UAT-4067
                                                                 };
                    string url = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                    Response.Redirect(url, true);
                }
            }
            else if (e.CommandName.ToLower() == "dataentry")
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

                    Int32 _itemDataId = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ApplicantComplianceItemID"]);
                    Int32 _CategoryId = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ComplianceCategoryID"]);
                    Int32 _packageId = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CompliancePackageID"]);
                    Int32 _applicantId = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ApplicantId"]);
                    //Int32 _selectedCategoryId = Convert.ToInt32((e.Item.FindControl("hdfCatId") as HiddenField).Value);
                    Int32 _selectedPackageSubscriptionId = Convert.ToInt32((e.Item.FindControl("hdfPackSubscriptionId") as HiddenField).Value);
                    //String hdfPermissionCode = (e.Item.FindControl("hdfPermissionCode") as HiddenField).Value;
                    Boolean isFullPermissiononVerification = Presenter.GetUserVerificationPermission(_selectedPackageSubscriptionId);
                    String _selectedArchiveStateCode = String.Empty;
                    if (!CurrentViewContext.SelectedArchiveStateCode.IsNullOrEmpty())
                    {
                        _selectedArchiveStateCode = CurrentViewContext.SelectedArchiveStateCode.FirstOrDefault();
                    }
                    //UAT-2460 : Empty Session Value.
                    Session["SelectedArchiveStateCode"] = null;
                    queryString = new Dictionary<String, String>
                                                                 {
                                                                    {"TenantId", Presenter.ClientId.ToString() },
                                                                    {"Child", ChildControls.SubscriptionDetail},
                                                                    {"WorkQueueType",WorkQueueType.ComplianceSearch.ToString()},
                                                                    {"PackageId",Convert.ToString(_packageId)},
                                                                    {"PackageSubscriptionId",Convert.ToString( _selectedPackageSubscriptionId)} ,
                                                                    {"ApplicantId",Convert.ToString(_applicantId)},
                                                                    {"IsFullPermissionForVerification",isFullPermissiononVerification.ToString()},
                                                                    {"SelectedArchiveStateCode",_selectedArchiveStateCode}
                                                                 };
                    //string url = String.Format(@"/ComplianceOperations/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                    string url = String.Format(@"/Dashboard/Pages/ApplicantDashboardMain.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
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

                    Int32 _itemDataId = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ApplicantComplianceItemID"]);
                    Int32 _CategoryId = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ComplianceCategoryID"]);
                    Int32 _packageId = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CompliancePackageID"]);
                    Int32 _applicantId = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ApplicantId"]);

                    //String itemDataId = ;
                    queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "SelectedTenantId", Presenter.ClientId.ToString() },
                                                                    { "Child", ChildControls.VerificationDetails},
                                                                    { "ItemDataId",Convert.ToString( _itemDataId)},
                                                                    {"WorkQueueType",WorkQueueType.ComplianceSearch.ToString()},
                                                                    {"PackageId",Convert.ToString(_packageId)},
                                                                    {"CategoryId",Convert.ToString(_CategoryId)},
                                                                    //{"SelectedProgramStudyId",SelectedProgramStudyId.ToString()},
                                                                    {"ApplicantFirstName",ApplicantFirstName},
                                                                    {"ApplicantLastName",ApplicantLastName},
                                                                    {"DateOfBirth",DateOfBirth.ToString()},
                                                                    //{"ItemLabel",ItemLabel},
                                                                    {"SelectedItemComplianceStatusId",String.Join(",", SelectedItemComplianceStatusId.ToArray()) },
                                                                    {"ApplicantId",Convert.ToString(_applicantId)}

                                                                 };
                    string url = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                    Response.Redirect(url, true);
                }
            }
            else if (e.CommandName.ToLower() == "notes") // UAT 2680
            {
                Int32 _selectedPackageSubscriptionId = Convert.ToInt32((e.Item.FindControl("hdfPackSubscriptionId") as HiddenField).Value);
                if (_selectedPackageSubscriptionId > 0)
                {
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "key", "openAddNotesPopUp(" + _selectedPackageSubscriptionId + "," + CurrentViewContext.SelectedTenantId + ");", true);
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
                    if (cmbExportFormat.IsNotNull() && (cmbExportFormat.SelectedValue == "Csv" || cmbExportFormat.SelectedValue == "Excel"))
                    {
                        if (grdItemData.MasterTableView.GetColumn("CustomAttributes").Display) //UAT-2675
                        {
                            grdItemData.MasterTableView.GetColumn("CustomAttributesTemp").Display = true;
                        }
                        if (grdItemData.MasterTableView.GetColumn("Notes").Display) //UAT-2675
                        {
                            grdItemData.MasterTableView.GetColumn("NotesTemp").Display = true;
                        }
                    }
                    else
                    {
                        grdItemData.MasterTableView.GetColumn("CustomAttributesTemp").Display = false;
                        grdItemData.MasterTableView.GetColumn("NotesTemp").Display = false;
                    }
                }
            }
            if (e.CommandName == "Cancel")
            {
                grdItemData.MasterTableView.GetColumn("CustomAttributesTemp").Display = false;
                grdItemData.MasterTableView.GetColumn("NotesTemp").Display = false;
            }
            #endregion

            #region For Filter command

            else if (e.CommandName == RadGrid.FilterCommandName)
            {
                Pair filter = (Pair)e.CommandArgument;
                ViewState["FilterPair"] = filter;
            }
            //FilterGridColumn();
            ActionType = ViewMode.Queue.ToString();

            #endregion

        }

        /// <summary>
        /// Event handler. Called by grdVerificationItemData for item data bound events.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that
        ///  contains the event data.</param>
        protected void grdItemData_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
                {
                    GridDataItem dataItem = (GridDataItem)e.Item;
                    if (Convert.ToString(dataItem["UserGroupName"].Text).Length > 20)
                    {
                        dataItem["UserGroupName"].ToolTip = dataItem["UserGroupName"].Text;
                        dataItem["UserGroupName"].Text = (dataItem["UserGroupName"].Text).ToString().Substring(0, 20) + "...";
                    }
                    if (Convert.ToString(dataItem["CustomAttributes"].Text).Length > 80)
                    {
                        dataItem["CustomAttributes"].ToolTip = dataItem["CustomAttributes"].Text;
                        dataItem["CustomAttributes"].Text = (dataItem["CustomAttributes"].Text).ToString().Substring(0, 80) + "...";
                    }
                    //Boolean isMapped = Convert.ToBoolean((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["IsUserGroupMatching"].ToString());

                    //Date : 27-Aug-2014 : Commented code to prevent auto selection of checkbox for perticular applicant checked earlier.
                    //String itemDataId = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ApplicantId"].ToString();
                    String itemDataId = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["PackageSubscriptionID"].ToString();

                    if (Convert.ToInt32(itemDataId) != 0)
                    {
                        //Date : 27-Aug-2014 : Now check/uncheck checkbox based on the PackageSubscriptionID not on the basis of ApplicantID.
                        //Dictionary<Int32, String> selectedItems = CurrentViewContext.AssignOrganizationUserIds;
                        Dictionary<Int32, Int32> selectedItems = CurrentViewContext.ListSubscriptionIds;
                        if (selectedItems.IsNotNull())
                        {
                            if (selectedItems.ContainsKey(Convert.ToInt32(itemDataId)))
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

                    ComplianceRecord complianceRecordData = (ComplianceRecord)e.Item.DataItem;
                    RadButton btnAddShowNotes = ((RadButton)e.Item.FindControl("btnAddShowNotes"));
                    if (String.IsNullOrEmpty(complianceRecordData.Notes))
                    {
                        btnAddShowNotes.Text = "Add";
                    }
                    else
                    {
                        if (complianceRecordData.Notes.Length > 50)
                        {
                            btnAddShowNotes.Text = complianceRecordData.Notes.Substring(0, 50) + " ...";
                        }
                        else { btnAddShowNotes.Text = complianceRecordData.Notes.Trim(); }

                        btnAddShowNotes.ToolTip = complianceRecordData.Notes.Trim();
                    }
                    //Label lblProfileStatus = ((Label)e.Item.FindControl("lblProfileStatus"));
                    //if (requirementSharesDataContract.ProfileShareStatus == "Shared")
                    //{
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
                    //ViewState["SortExpression"] = e.SortExpression;
                    //ViewState["SortDirection"] = e.NewSortOrder.Equals(GridSortOrder.Descending);
                    CurrentViewContext.ItemDataGridCustomPaging.SortExpression = e.SortExpression;
                    CurrentViewContext.ItemDataGridCustomPaging.SortDirectionDescending = e.NewSortOrder.Equals(GridSortOrder.Descending);
                }
                else
                {
                    //ViewState["SortExpression"] = String.Empty;
                    //ViewState["SortDirection"] = false;
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
            GridFilterMenu menu = grdItemData.FilterMenu;
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
        #endregion

        #region DropDown Events

        protected void ddlTenantName_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            BindItemComplianceStatus();
            BindOverAllComplianceStatus();
            BindCategoryComplianceStatus();
            BindUserGroups();
            if (!ddlTenantName.SelectedValue.IsNullOrEmpty())
            {
                //Presenter.GetArchiveStateList();
                ucCustomAttributeLoader.Reset(Convert.ToInt32(ddlTenantName.SelectedValue));
                //rbSubscriptionState.Visible = true;
                // btnArchieve.Enabled = true;
                if (rbSubscriptionState.SelectedValue == ArchiveState.Active.GetStringValue())
                {
                    ShowHideControl("Archivemun", "btnArchive", true);
                    ShowHideControl("Archivemun", "btnUnArchive", false);
                    //UAT-4256
                    radMenubtnArchUnArch.Visible = true;
                    btnUnArch.Visible = false;
                }


            }
            else
            {
               // rbSubscriptionState.Visible = false;
                //grdItemData.Rebind();
            }
        }

        protected void ddlTenantName_DataBound(object sender, EventArgs e)
        {
            ddlTenantName.Items.Insert(0, new RadComboBoxItem("--Select--"));
        }


        /// <summary>
        /// User group DataBound event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlUserGroup_DataBound(object sender, EventArgs e)
        {
            //ddlUserGroup.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--Select--"));
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
                Dictionary<Int32, Int32> selectedSubscriptionIDs = CurrentViewContext.ListSubscriptionIds;
                Int32 orgUserID = (Int32)dataItem.GetDataKeyValue("ApplicantId");
                String orgUserName = Convert.ToString(dataItem["ApplicantFirstName"].Text) + " " + Convert.ToString(dataItem["ApplicantLastName"].Text);
                isChecked = ((CheckBox)dataItem.FindControl("chkSelectUser")).Checked;
                Int32 subscriptionID = (Int32)dataItem.GetDataKeyValue("PackageSubscriptionID");

                if (isChecked)
                {
                    if (!selectedItems.ContainsKey(orgUserID))
                    {
                        selectedItems.Add(orgUserID, orgUserName);
                    }
                    if (!selectedSubscriptionIDs.ContainsKey(subscriptionID))
                    {
                        selectedSubscriptionIDs.Add(subscriptionID, orgUserID);
                    }
                }
                else
                {
                    if (selectedSubscriptionIDs != null)
                    {
                        selectedSubscriptionIDs.Remove(subscriptionID);
                    }
                    if (selectedItems != null && selectedItems.ContainsKey(orgUserID))
                    {
                        if (!selectedSubscriptionIDs.Any(item => item.Value == orgUserID))
                        {
                            selectedItems.Remove(orgUserID);
                        }
                    }

                }
                CurrentViewContext.AssignOrganizationUserIds = selectedItems;
                CurrentViewContext.ListSubscriptionIds = selectedSubscriptionIDs;

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

        protected void chkSelectAllResults_CheckedChanged(object sender, EventArgs e)
        {
            if (CurrentViewContext.ItemDataGridCustomPaging.VirtualPageCount > 0)
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
                    CurrentViewContext.ListSubscriptionIds = new Dictionary<int, int>();
                }


                foreach (GridDataItem item in grdItemData.Items)
                {
                    CheckBox checkBox = ((CheckBox)item.FindControl("chkSelectUser"));
                    checkBox.Checked = needToCheckboxChecked;
                }

                GridHeaderItem headerItem = grdItemData.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
                CheckBox headerCheckBox = ((CheckBox)headerItem.FindControl("chkSelectAll"));
                headerCheckBox.Checked = needToCheckboxChecked;
            }
        }


        #region Properties

        #region Public Properties

        public ComplianceSearchControlPresenter Presenter
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

        public Int32 SelectedTenantId
        {
            get
            {
                return Convert.ToInt32(ViewState["TenantIdSearch"]);
            }
            set
            {
                ViewState["TenantIdSearch"] = value.ToString();
            }
        }

        public Int32 SelectedPackageId
        {
            get
            {
                return 0;
            }
            set
            {
                SelectedPackageId = value;
            }
        }

        public List<Int32> SelectedItemComplianceStatusId
        {
            get
            {
                List<Int32> selectedIds = new List<Int32>();
                for (Int32 i = 0; i < chkItemStatus.Items.Count; i++)
                {
                    if (chkItemStatus.Items[i].Checked)
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
                    chkItemStatus.Items[i].Checked = value.Contains(Convert.ToInt32(chkItemStatus.Items[i].Value));
                }

            }

        }

        public List<Int32> SelectedOverAllComplianceStatusId
        {
            get
            {
                List<Int32> selectedIds = new List<Int32>();
                for (Int32 i = 0; i < chkOverAllStatus.Items.Count; i++)
                {
                    if (chkOverAllStatus.Items[i].Checked)
                    {
                        selectedIds.Add(Convert.ToInt32(chkOverAllStatus.Items[i].Value));
                    }
                }
                return selectedIds;
            }
            set
            {
                for (Int32 i = 0; i < chkOverAllStatus.Items.Count; i++)
                {
                    chkOverAllStatus.Items[i].Checked = value.Contains(Convert.ToInt32(chkOverAllStatus.Items[i].Value));
                }

            }

        }

        public List<Int32> SelectedCategoryComplianceStatusId
        {
            get
            {
                List<Int32> selectedIds = new List<Int32>();
                for (Int32 i = 0; i < chkCategoryStatus.Items.Count; i++)
                {
                    if (chkCategoryStatus.Items[i].Checked)
                    {
                        selectedIds.Add(Convert.ToInt32(chkCategoryStatus.Items[i].Value));
                    }
                }
                return selectedIds;
            }
            set
            {
                for (Int32 i = 0; i < chkCategoryStatus.Items.Count; i++)
                {
                    chkCategoryStatus.Items[i].Checked = value.Contains(Convert.ToInt32(chkCategoryStatus.Items[i].Value));
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

        public List<Entity.ClientEntity.CompliancePackage> lstCompliancePackage
        {
            get;
            set;
        }

        public List<Entity.ClientEntity.lkpPackageComplianceStatu> lstOverAllComplianceStatus
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

        public List<Entity.ClientEntity.lkpCategoryComplianceStatu> lstCategoryComplianceStatus
        {
            get;
            set;
        }

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


        public string SSNnumber
        {
            //get
            //{
            //    return txtSSNNumber.Text;
            //}
            //set
            //{
            //    txtSSNNumber.Text = value;
            //}
            get;
            set;
        }

        public Int32? OrderID
        {
            get;
            set;
        }

        public String OrderNumber
        {
            get
            {
                return txtOrderId.Text.IsNullOrEmpty() ? String.Empty : Convert.ToString(txtOrderId.Text);
            }
            set
            {
                txtOrderId.Text = value.ToString();
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

        public List<ComplianceRecord> ItemData
        {
            get
            {
                return ViewState["ItemData"] as List<ComplianceRecord>;
            }
            set
            {
                ViewState["ItemData"] = value;
            }
        }

        public Int32 CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        public IComplianceSearchControlView CurrentViewContext
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
                if (_gridSearchContract.IsNull())
                {
                    var serializer = new XmlSerializer(typeof(SearchItemDataContract));
                    TextReader reader = new StringReader(Convert.ToString(Session[AppConsts.SEARCH_OBJECT_SESSION_KEY]));
                    _gridSearchContract = (SearchItemDataContract)serializer.Deserialize(reader);
                }
                return _gridSearchContract;
            }
            set
            {
                var serializer = new XmlSerializer(typeof(SearchItemDataContract));
                var sb = new StringBuilder();
                using (TextWriter writer = new StringWriter(sb))
                {
                    serializer.Serialize(writer, value);
                }
                Session[AppConsts.SEARCH_OBJECT_SESSION_KEY] = sb.ToString();
            }
        }

        public List<UserGroup> lstUserGroup
        {
            get;
            set;
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

        public String UserGroupIds
        {
            get
            {
                if (ddlUserGroup.CheckedItems.Count > AppConsts.NONE)
                {
                    List<Int32> selectedUserGroup = new List<Int32>();
                    foreach (RadComboBoxItem slctdUserGroup in ddlUserGroup.CheckedItems)
                    {
                        selectedUserGroup.Add(Convert.ToInt32(slctdUserGroup.Value));
                    }
                    return String.Join(",", selectedUserGroup.ToArray());
                }
                return null;
            }
            set
            {
                if (!value.IsNullOrEmpty())
                {
                    String[] selectedIds = value.Split(',');
                    foreach (RadComboBoxItem item in ddlUserGroup.Items)
                    {
                        item.Checked = selectedIds.Contains(item.Value);
                    }
                }
            }
        }

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
                return grdItemData.PageSize;
                //                return grdItemData.PageSize > 100 ? 100 : grdItemData.PageSize;
            }
            set
            {
                grdItemData.PageSize = value;
            }

        }

        /// <summary>
        /// VirtualPageCount
        /// </summary>
        /// <value> Sets the value for VirtualPageCount.</value>
        public Int32 VirtualPageCount
        {
            set
            {
                grdItemData.VirtualItemCount = value;
                grdItemData.MasterTableView.VirtualItemCount = value;
            }
            get
            {
                return grdItemData.MasterTableView.VirtualItemCount;
            }
        }

        /// <summary>
        /// get object of shared class of custom paging
        /// </summary>
        public CustomPagingArgsContract ItemDataGridCustomPaging
        {
            get
            {
                if (ViewState["ItemDataGridCustomPaging"] == null)
                {
                    ViewState["ItemDataGridCustomPaging"] = new CustomPagingArgsContract();
                }
                return (CustomPagingArgsContract)ViewState["ItemDataGridCustomPaging"];
            }
            set
            {
                ViewState["ItemDataGridCustomPaging"] = value;
                VirtualPageCount = value.VirtualPageCount;
                PageSize = value.PageSize;
                CurrentPageIndex = value.CurrentPageIndex;
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

        public Dictionary<Int32, Int32> ListSubscriptionIds
        {
            get
            {
                if (ViewState["ListSubscriptionIds"] != null)
                    return ViewState["ListSubscriptionIds"] as Dictionary<Int32, Int32>;
                else
                    return new Dictionary<Int32, Int32>();
            }
            set
            {
                ViewState["ListSubscriptionIds"] = value;
            }
        }

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

        #region UAT-2675
        Int32 IComplianceSearchControlView.CurrentLoggedInUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }


        Int32 IComplianceSearchControlView.OrganizationUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }
        #endregion

        //START UAT-3157
        Int32 IComplianceSearchControlView.PreferredSelectedTenantID
        {
            get
            {
                if (!ViewState["PreferredSelectedTenantID"].IsNull())
                {
                    return (Int32)ViewState["PreferredSelectedTenantID"];
                }
                return AppConsts.NONE;
            }
            set
            {
                ViewState["PreferredSelectedTenantID"] = value;
            }
        }


        Boolean IComplianceSearchControlView.IsDefaultPreferredTenant
        {
            get
            {
                if (!ViewState["IsDefaultPreferredTenant"].IsNullOrEmpty())
                {
                    return (Boolean)ViewState["IsDefaultPreferredTenant"];
                }
                return false;
            }
            set
            {
                ViewState["IsDefaultPreferredTenant"] = value;
            }
        }
        //END UAT-3157

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

        #region Private Properties

        #endregion

        #endregion

        #region Methods

        #region Private Methods

        private void DisplayMessageSentStatus()
        {
            if (hdMessageSent.Value == "sent")
            {
                base.ShowSuccessMessage("Message has been sent successfully.");
                hdMessageSent.Value = "new";
            }
        }

        private void SetValuesForTenant()
        {
            if (!ddlTenantName.SelectedValue.IsNullOrEmpty())
            {
                CurrentViewContext.SelectedTenantId = Convert.ToInt32(ddlTenantName.SelectedValue);
                ucCustomAttributeLoader.TenantId = CurrentViewContext.SelectedTenantId;
                ucCustomAttributeLoader.ScreenType = "CommonScreen";
            }
            else
            {
                CurrentViewContext.SelectedTenantId = 0;
                ucCustomAttributeLoader.Reset();
                dvRecords.Visible = false;
            }
        }

        private void BindItemComplianceStatus()
        {
            Presenter.GetItemComplianceStatus();
            chkItemStatus.DataSource = CurrentViewContext.lstItemComplianceStatus;
            chkItemStatus.DataBind();
        }

        private void BindOverAllComplianceStatus()
        {
            Presenter.GetOverAllComplianceStatus();
            chkOverAllStatus.DataSource = CurrentViewContext.lstOverAllComplianceStatus;
            chkOverAllStatus.DataBind();
        }

        private void BindCategoryComplianceStatus()
        {
            Presenter.GetCategoryComplianceStatus();
            chkCategoryStatus.DataSource = CurrentViewContext.lstCategoryComplianceStatus;
            chkCategoryStatus.DataBind();
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

            if (ViewState["FilterPair"] != null)
            {
                Pair filter = (Pair)ViewState["FilterPair"];
                Int32 filterIndex = CurrentViewContext.SearchItemDataContract.FilterColumns.IndexOf(filter.Second.ToString());

                if (filter.First.ToString() != GridKnownFunction.NoFilter.ToString() && ActionType != ViewMode.Search.ToString())
                {
                    String filterValue = grdItemData.MasterTableView.Columns.FindByUniqueName(filter.Second.ToString()).CurrentFilterValue;

                    if (filterIndex != -1)
                    {
                        CurrentViewContext.SearchItemDataContract.FilterOperators[filterIndex] = filter.First.ToString();
                        if (grdItemData.MasterTableView.Columns.FindByUniqueName(filter.Second.ToString()).DataTypeName == "System.Decimal")
                        {
                            CurrentViewContext.SearchItemDataContract.FilterValues[filterIndex] = Convert.ToDecimal(filterValue);
                        }
                        else if (grdItemData.MasterTableView.Columns.FindByUniqueName(filter.Second.ToString()).DataTypeName == "System.Int32")
                        {
                            CurrentViewContext.SearchItemDataContract.FilterValues[filterIndex] = Convert.ToInt32(filterValue);
                        }
                        else if (grdItemData.MasterTableView.Columns.FindByUniqueName(filter.Second.ToString()).DataTypeName == "System.DateTime")
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
                        if (grdItemData.MasterTableView.Columns.FindByUniqueName(filter.Second.ToString()).DataTypeName == "System.Decimal")
                        {
                            CurrentViewContext.SearchItemDataContract.FilterValues.Add(Convert.ToDecimal(filterValue));
                        }
                        else if (grdItemData.MasterTableView.Columns.FindByUniqueName(filter.Second.ToString()).DataTypeName == "System.Int32")
                        {
                            CurrentViewContext.SearchItemDataContract.FilterValues.Add(Convert.ToInt32(filterValue));
                        }
                        else if (grdItemData.MasterTableView.Columns.FindByUniqueName(filter.Second.ToString()).DataTypeName == "System.DateTime")
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
                }
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
                ViewState["FilterPair"] = null;
            }
        }

        private void SetThePreviousValues()
        {
            if (Args.IsNotNull() && CurrentViewContext.ViewStateSearchData.IsNotNull())
            {
                SearchItemDataContract searchDataContract = new SearchItemDataContract();

                searchDataContract.IsBackToSearch = true;

                searchDataContract = CurrentViewContext.ViewStateSearchData;
                txtFirstName.Text = searchDataContract.ApplicantFirstName;
                txtLastName.Text = searchDataContract.ApplicantLastName;
                txtOrderId.Text = searchDataContract.OrderNumber.IsNullOrEmpty() ? String.Empty : searchDataContract.OrderNumber.ToString();
                dpkrDOB.SelectedDate = searchDataContract.DateOfBirth;
                //txtSSNNumber.Text = searchDataContract.ApplicantSSN;
                if (CurrentViewContext.SSNPermissionCode.ToUpper() == EnumSystemPermissionCode.MASKED_READ_PERMISSION.GetStringValue().ToUpper())
                {
                    if (!searchDataContract.ApplicantSSN.IsNullOrEmpty())
                    {
                        CurrentViewContext.SSNnumber = searchDataContract.ApplicantSSN.Substring(searchDataContract.ApplicantSSN.Length - AppConsts.FOUR);
                        txtSSNNumber.Text = CurrentViewContext.SSNnumber;
                    }
                }
                else
                {
                    CurrentViewContext.SSNnumber = searchDataContract.ApplicantSSN;
                    txtSSNNumber.Text = CurrentViewContext.SSNnumber;
                }
                ucCustomAttributeLoader.NodeIds = !searchDataContract.NodeIds.IsNullOrEmpty() ? Convert.ToString(searchDataContract.NodeIds) : String.Empty;
                ucCustomAttributeLoader.TenantId = CurrentViewContext.SelectedTenantId = searchDataContract.ClientID;
                ucCustomAttributeLoader.previousValues = searchDataContract.CustomFields;
                ucCustomAttributeLoader.nodeLable = searchDataContract.NodeLabel;
                ucCustomAttributeLoader.DPM_ID = searchDataContract.SelectedDPMIds;
                ucCustomAttributeLoader.ScreenType = "CommonScreen";
                ddlTenantName.SelectedValue = Convert.ToString(CurrentViewContext.SelectedTenantId);
                BindItemComplianceStatus();
                BindOverAllComplianceStatus();
                BindCategoryComplianceStatus();
                BindUserGroups();
                CurrentViewContext.SelectedItemComplianceStatusId = searchDataContract.StatusID;
                CurrentViewContext.SelectedCategoryComplianceStatusId = searchDataContract.CategoryIDList;
                CurrentViewContext.SelectedOverAllComplianceStatusId = searchDataContract.OverAllIDList;
                //grdItemData.PageSize = searchDataContract.GridCustomPagingArguments.PageSize;
                //grdItemData.MasterTableView.CurrentPageIndex = searchDataContract.GridCustomPagingArguments.CurrentPageIndex - 1;
                CurrentViewContext.ItemDataGridCustomPaging = searchDataContract.GridCustomPagingArguments;
                if (!CurrentViewContext.ItemDataGridCustomPaging.SortExpression.IsNullOrEmpty())
                {
                    GridSortExpression gridSortExpression = new GridSortExpression();
                    gridSortExpression.FieldName = CurrentViewContext.ItemDataGridCustomPaging.SortExpression;
                    gridSortExpression.SortOrder = CurrentViewContext.ItemDataGridCustomPaging.SortDirectionDescending ? GridSortOrder.Descending : GridSortOrder.Ascending;
                    grdItemData.MasterTableView.SortExpressions.Add(gridSortExpression);
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
                    //btnArchieve.Enabled = false;
                    ShowHideControl("Archivemun", "btnArchive", false);
                    ShowHideControl("Archivemun", "btnUnArchive", true);
                    //UAT-4256
                    radMenubtnArchUnArch.Visible = false;
                    btnUnArch.Visible = true;

                }

                #region UAT-3518
                rbSubscriptionExpiryState.SelectedValue = searchDataContract.SelectedExpiryStateCode == null ? "ZZ" : searchDataContract.SelectedExpiryStateCode;
                #endregion

                if (!searchDataContract.UserGroupIds.IsNullOrEmpty())
                {
                    //ddlUserGroup.SelectedValue = searchDataContract.MatchUserGroupID.ToString();
                    UserGroupIds = searchDataContract.UserGroupIds.ToString();
                }
                //Presenter.PerformSearch(ucCustomAttributeLoader.GetCustomDataXML(), ucCustomAttributeLoader.NodeId);
                //grdItemData.Rebind();

                //if SelectedTenantId has value i.e greater than 0,Send Message button will be visible.
                //if (!SelectedTenantId.IsNullOrEmpty() && SelectedTenantId > 0)
                //{
                //    //fsucCmdBar1.ClearButton.Style.Clear();
                //    //fsucCmdBar1.ExtraButton.Style.Clear();


                //    ShowHideMenuButton("SendMessagemun", "btnSendMessage", true);
                //    ShowHideMenuButton("PassportReportmun", "btnPassportReport", true);
                //}
                ShowHideControls(true);
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
                grdItemData.MasterTableView.GetColumn("DateOfBirth").Visible = false;
            }
            if (CurrentViewContext.SSNPermissionCode.ToUpper() == EnumSystemPermissionCode.NO_ACCESS_PERMISSION.GetStringValue().ToUpper())
            {
                divSSN.Visible = false;

            }
            //else if (CurrentViewContext.SSNPermissionCode.ToUpper() == EnumSystemPermissionCode.MASKED_READ_PERMISSION.GetStringValue().ToUpper())
            //{
            //    txtSSNNumber.Mask = AppConsts.SSN_MASK_FORMATE; //@"\#\#\#-\#\#-####"
            //}

            //UAT-3010:-  Granular Permission for Client Admin Users to Archive.

            if (CurrentViewContext.ArchivePermissionCode.ToUpper() == EnumSystemPermissionCode.NO_ACCESS_PERMISSION.GetStringValue().ToUpper())
            {
                //btnArchieve.Visible = false;               

                ShowHideControl("Archivemun", "btnArchivemun", false);
            }

        }

        private void ApplySSNMask()
        {
            if (CurrentViewContext.SSNPermissionCode.ToUpper() == EnumSystemPermissionCode.MASKED_READ_PERMISSION.GetStringValue().ToUpper())
            {
                //txtSSN.Mask = AppConsts.SSN_MASK_FORMATE; //@"\#\#\#-\#\#-####"
                txtSSNNumber.Mask = AppConsts.SSN_MASK_FORMAT_ALPHANUMERIC;
            }
        }

        //UAT-3162 - Refresh filtered results after archiving/unarchiving on compliance search
        private void UncheckGridItemsOnArchiveUnarchive()
        {
            foreach (GridDataItem item in grdItemData.Items)
            {
                CheckBox checkBox = ((CheckBox)item.FindControl("chkSelectUser"));
                checkBox.Checked = false;
            }

            GridHeaderItem headerItem = grdItemData.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
            CheckBox headerCheckBox = ((CheckBox)headerItem.FindControl("chkSelectAll"));
            headerCheckBox.Checked = false;
        }

        #endregion

        protected void rbSubscriptionState_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rbSubscriptionState.SelectedValue == ArchiveState.Archived.GetStringValue())
            {
                //btnArchieve.Enabled = false;
                ShowHideControl("Archivemun", "btnArchive", false);

                ShowHideControl("Archivemun", "btnUnArchive", true);
                //UAT-4256
                radMenubtnArchUnArch.Visible = false;
                btnUnArch.Visible = true;
            }
            else if (rbSubscriptionState.SelectedValue == ArchiveState.Active.GetStringValue())
            {
                ShowHideControl("Archivemun", "btnArchive", true);

                ShowHideControl("Archivemun", "btnUnArchive", false);


                //UAT-4256
                radMenubtnArchUnArch.Visible = true;
                btnUnArch.Visible = false;
            }
            else if (rbSubscriptionState.SelectedValue == ArchiveState.All.GetStringValue())
            {
                ShowHideControl("Archivemun", "btnArchive", true);

                ShowHideControl("Archivemun", "btnUnArchive", true);

                //UAT-4256
                radMenubtnArchUnArch.Visible = true;
                btnUnArch.Visible = false;
            }
        }

        #region UAT-2675
        private void ManageColumnConfiguration()
        {
            String grdCode = String.Empty;

            _lstCodeForColumnConfig.Add(Screen.grdManageComplianceSearch.GetStringValue());
            grdCode = INTSOF.Utils.Screen.grdManageComplianceSearch.GetStringValue();

            if (!grdCode.IsNullOrEmpty())
            {
                grdItemData.Attributes["GridCode"] = grdCode;
                ColumnsConfiguration.CurrentViewContext.CurrentLoggedInUserID = CurrentViewContext.CurrentLoggedInUserId;
                ColumnsConfiguration.CurrentViewContext.OrganisationUserID = CurrentViewContext.CurrentLoggedInUserId;
                ColumnsConfiguration.CurrentViewContext.lstGridCode = _lstCodeForColumnConfig;

                List<PreHiddenColumnsContract> lstpreHiddenColumnsContract = new List<PreHiddenColumnsContract>();
                lstpreHiddenColumnsContract.Add(new PreHiddenColumnsContract { GridCode = "AAAI", PredefinedHiddenColumn = "CustomAttributesTemp" });
                lstpreHiddenColumnsContract.Add(new PreHiddenColumnsContract { GridCode = "AAAI", PredefinedHiddenColumn = "NotesTemp" });
                ColumnsConfiguration.CurrentViewContext.lstPredefinedHIddenColumns = lstpreHiddenColumnsContract;
                grdItemData.Attributes["PreHiddenColumns"] = "CustomAttributesTemp,NotesTemp";

                List<String> lstColumnnsToHide = Presenter.GetScreenColumnsToHide(grdCode, CurrentViewContext.CurrentLoggedInUserId);
                if (!lstColumnnsToHide.IsNullOrEmpty())
                {
                    if (lstColumnnsToHide.Contains("CustomAttributes") && lstColumnnsToHide.Contains("Notes"))
                    {
                        grdItemData.Attributes["ExportingColumnsNotInGrid"] = String.Empty;
                    }
                    else if (lstColumnnsToHide.Contains("Notes") && !lstColumnnsToHide.Contains("CustomAttributes"))
                    {
                        grdItemData.Attributes["ExportingColumnsNotInGrid"] = "CustomAttributesTemp";
                    }
                    else if (lstColumnnsToHide.Contains("CustomAttributes") && !lstColumnnsToHide.Contains("Notes"))
                    {
                        grdItemData.Attributes["ExportingColumnsNotInGrid"] = "NotesTemp";
                    }
                    else
                    {
                        grdItemData.Attributes["ExportingColumnsNotInGrid"] = "CustomAttributesTemp,NotesTemp";
                    }
                }
                else
                {
                    grdItemData.Attributes["ExportingColumnsNotInGrid"] = "CustomAttributesTemp,NotesTemp";
                }
            }
        }
        #endregion

        #region UAT-3157:- Sticky institution

        private void GetPreferredSelectedTenant(Boolean IsResetClick)
        {
            if (CurrentViewContext.SelectedTenantId.IsNullOrEmpty() || CurrentViewContext.SelectedTenantId == AppConsts.NONE)
            {
                // Presenter.GetPreferredSelectedTenant();
                CurrentViewContext.PreferredSelectedTenantID = (Session["PreferredSelectedTenant"]).IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(Session["PreferredSelectedTenant"]);
                if (CurrentViewContext.PreferredSelectedTenantID > AppConsts.NONE)
                {
                    ddlTenantName.SelectedValue = Convert.ToString(CurrentViewContext.PreferredSelectedTenantID);
                    CurrentViewContext.SelectedTenantId = Convert.ToInt32(ddlTenantName.SelectedValue);

                    if (IsResetClick)
                    {
                        ucCustomAttributeLoader.TenantId = CurrentViewContext.SelectedTenantId;
                        ucCustomAttributeLoader.ResetControlData(true);  // Use to reset control values only
                        //SetThePreviousValues();
                        //ucCustomAttributeLoader.Presenter.GetCustomAttributes(CurrentViewContext.ViewStateSearchData.SelectedDPMIds, CustomAttributeUseTypeContext.Hierarchy.GetStringValue(), CurrentViewContext.TenantId, "CommonScreen");
                        //if (!ucCustomAttributeLoader.ShowUserGroupCustomAttribute)
                        //{
                        //    ucCustomAttributeLoader.lstTypeCustomAttributes.RemoveAll(x => x.CADataTypeCode.ToLower().Trim() == CustomAttributeDatatype.User_Group.GetStringValue().ToLower().Trim());
                        //}
                        //ucCustomAttributeLoader.SetCustomAttributeValues();
                    }

                    //SetValuesForTenant();
                    //Control ucCustomAttribute = Page.LoadControl(@"~\ComplianceAdministration\UserControl\CustomAttributeLoaderSearchMultipleNodes.ascx");
                    BindItemComplianceStatus();
                    BindOverAllComplianceStatus();
                    BindCategoryComplianceStatus();
                    BindUserGroups();
                    if (!ddlTenantName.SelectedValue.IsNullOrEmpty())
                    {
                        Presenter.GetArchiveStateList();
                        ucCustomAttributeLoader.Reset(Convert.ToInt32(ddlTenantName.SelectedValue), false);
                        //rbSubscriptionState.Visible = true;
                        // btnArchieve.Enabled = true;
                        if (rbSubscriptionState.SelectedValue == ArchiveState.Active.GetStringValue())
                        {
                            ShowHideControl("Archivemun", "btnArchive", true);
                            ShowHideControl("Archivemun", "btnUnArchive", false);
                            //UAT-4256
                            radMenubtnArchUnArch.Visible = true;
                            btnUnArch.Visible = false;
                        }

                    }
                    else
                    {
                        //rbSubscriptionState.Visible = false;
                        //grdItemData.Rebind();
                    }

                    ShowHideMenuButton("SendMessagemun", "btnSendMessage", true);
                    //  ShowHideMenuButton("PassportReportmun", "btnPassportReport", true);
                    ShowHideDivControl("PassportReportmun", "dvPassprotReport", true);

                    CurrentViewContext.ListSubscriptionIds = new Dictionary<Int32, Int32>();
                    CurrentViewContext.IsDefaultPreferredTenant = true;
                    //grdItemData.Rebind();
                }
            }
        }
        #endregion

        private void ShowHideControls(Boolean IsDisplay)
        {
            ShowHideMenuButton("SendMessagemun", "btnSendMessage", IsDisplay);
            //     ShowHideMenuButton("PassportReportmun", "btnPassportReport", IsDisplay);
            ShowHideDivControl("PassportReportmun", "dvPassprotReport", IsDisplay);
            //rbSubscriptionState.Visible = IsDisplay;
            grdItemData.Visible = IsDisplay;
        }
        #endregion

        #region Public Methods
        private void ShowHideControl(String menuText, String controlID, Boolean isVisible)
        {
            RadMenuItem menuItem = cmd.FindItemByText(menuText);

            foreach (RadMenuItem item in menuItem.Items)
            {
                RadButton btnMenu = (RadButton)item.FindControl(controlID);
                if (btnMenu.IsNotNull())
                {
                    btnMenu.Visible = isVisible;
                }
            }
        }
        private void ShowHideMenuButton(String menuText, String controlID, Boolean isVisible)
        {
            RadMenuItem menuItem = cmd.FindItemByText(menuText);

            if (menuItem.IsNotNull())
            {
                RadButton btnMenu = (RadButton)menuItem.FindControl(controlID);
                btnMenu.Visible = isVisible;
            }

        }
        private void ShowHideDivControl(String menuText, String controlID, Boolean isVisible)
        {
            RadMenuItem menuItem = cmd.FindItemByText(menuText);

            if (menuItem.IsNotNull())
            {
                Control dvPassport = (Control)menuItem.FindControl(controlID);
                if (dvPassport.IsNotNull())
                {
                    dvPassport.Visible = isVisible;
                }
            }
        }
        #endregion

        #endregion

    }
}

