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
using Business.RepoManagers;


#endregion

#endregion

namespace CoreWeb.Search.Views
{
    public partial class ApplicantUserGroup : BaseUserControl, IApplicantUserGroupView
    {
        #region Variables

        private ApplicantUserGroupPresenter _presenter = new ApplicantUserGroupPresenter();
        private String _viewType;
        private Int32 tenantId = 0;
        private CustomPagingArgsContract _gridCustomPaging = null;
        private SearchItemDataContract _gridSearchContract = null;

        #endregion

        #region Properties

        public ApplicantUserGroupPresenter Presenter
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

        public Int32 CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        public IApplicantUserGroupView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public List<Entity.Tenant> lstTenant
        {
            get;
            set;
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

        #region UAT-2535
        public List<Int32> LstSelectedUserGrpIDs
        {
            get
            {
                List<Int32> selectedIds = new List<Int32>();
                for (Int32 i = 0; i < ddlSearchUserGroup.Items.Count; i++)
                {
                    if (ddlSearchUserGroup.Items[i].Checked)
                    {
                        selectedIds.Add(Convert.ToInt32(ddlSearchUserGroup.Items[i].Value));
                    }
                }
                return selectedIds;
            }
            set
            {
                for (Int32 i = 0; i < ddlSearchUserGroup.Items.Count; i++)
                {
                    ddlSearchUserGroup.Items[i].Checked = value.Contains(Convert.ToInt32(ddlSearchUserGroup.Items[i].Value));
                }

            }
        }
        #endregion

        /// <summary>
        /// Gets or Sets the value for selected Items.
        /// </summary>
        public Dictionary<Int32, Boolean> AssignOrganizationUserIds
        {
            get
            {
                if (!ViewState["SelectedApplicants"].IsNull())
                {
                    return ViewState["SelectedApplicants"] as Dictionary<Int32, Boolean>;
                }

                return new Dictionary<Int32, Boolean>();
            }
            set
            {
                ViewState["SelectedApplicants"] = value;
            }
        }


        public string SortColumnName
        {
            get
            {
                if (!ViewState["SortColumnName"].IsNull())
                {
                    return ViewState["SortColumnName"] as string;
                }

                return string.Empty ;
            }
            set
            {
                ViewState["SortColumnName"] = value;
            }
        }


        public bool SortColumnNameType
        {
            get
            {
                if (!ViewState["SortColumnNameType"].IsNull())
                {
                    return Convert.ToBoolean( ViewState["SortColumnNameType"]);
                }

                return false;
            }
            set
            {
                ViewState["SortColumnNameType"] = value;
            }
        }

        /// <summary>
        /// Is Active
        /// </summary>
        /// 
        public Int32 IsResult
        {
            get
            {
                return Convert.ToInt32(rbtnResults.SelectedValue);
            }
            set
            {
                rbtnResults.SelectedValue = value.ToString();
            }
        }
        //public Boolean IsResult
        //{
        //    get
        //    {
        //        return Convert.ToBoolean(rbtnResults.SelectedValue);
        //    }
        //    set
        //    {
        //        rbtnResults.SelectedValue = value.ToString() == "true" ? "true" : "false";
        //    }
        //}


        public Boolean? IsUserGroupAssigned
        {
            get { return Convert.ToBoolean(rbtnResults.SelectedValue == "1" ? "true" : "false"); }
            set { rbtnResults.SelectedValue = value.ToString() == "1" ? "true" : "false"; }
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

        public String ErrorMessage
        {
            get;
            set;
        }

        public String SuccessMessage
        {
            get;
            set;
        }

        public String InfoMessage
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
                if (grdApplicantSearchData.MasterTableView.CurrentPageIndex > 0)
                {
                    grdApplicantSearchData.MasterTableView.CurrentPageIndex = value - 1;
                }
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
                if (_gridCustomPaging.IsNull())
                {
                    _gridCustomPaging = new CustomPagingArgsContract();
                }
                return _gridCustomPaging;
            }
        }

        #endregion

        #region UAT-1088 - Add order date ranges to the user group mapping screen

        public DateTime? OrderCreatedFrom
        {
            get
            {
                return Convert.ToDateTime(dpOrderCreatedFrom.SelectedDate);
            }
            set
            {
                dpOrderCreatedFrom.SelectedDate = Convert.ToDateTime(value);
            }
        }
        public DateTime? OrderCreatedTo
        {
            get
            {
                return Convert.ToDateTime(dpOrderCreatedTo.SelectedDate);
            }
            set
            {
                dpOrderCreatedTo.SelectedDate = Convert.ToDateTime(value);
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

        #region UAT-1688
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

        #endregion

        #region Events

        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                base.Title = "Applicant User Group Mapping";
                base.SetPageTitle("Applicant User Group Mapping");

                //grdApplicantSearchData.MasterTableView.GetColumn("_SSN").Display = false;
                //grdApplicantSearchData.MasterTableView.GetColumn("SSN").Display = true;

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
            SSN = txtSSN.TextWithPrompt;
            _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
            (fsucCmdBarButton as CoreWeb.Shell.Views.CommandBar).SaveButton.ToolTip = "Click to search orders per the criteria entered above";
            (fsucCmdBarButton as CoreWeb.Shell.Views.CommandBar).SaveButton.ValidationGroup = "grpFormSubmit";
            (fsucCmdBarButton as CoreWeb.Shell.Views.CommandBar).CancelButton.ToolTip = "Click to cancel. Any data entered will not be saved";
            (fsucCmdBarButton as CoreWeb.Shell.Views.CommandBar).SubmitButton.ToolTip = "Click to remove all values entered in the search criteria above";
            if (!this.IsPostBack)
            {
                Session[AppConsts.APPLICANT_SEARCH_SESSION_KEY] = null;
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
                    if (args.ContainsKey("CancelClicked") && args["CancelClicked"].IsNotNull())
                    {
                        GetSessionValues();
                    }
                }
                else
                    grdApplicantSearchData.Visible = false;
                fsucCmdBarButton.ClearButton.Style.Add("display", "none");
                fsucCmdBarButton.SaveButton.ValidationGroup = "grpFormSubmit";
                fsucCmdBarButton.ClearButton.ValidationGroup = "grpFormSubmit";
            }
            Presenter.OnViewLoaded();
            ucCustomAttributeLoaderSearch.TenantId = SelectedTenantId;
            ucCustomAttributeLoaderSearch.ScreenType = "CommonScreen";
            HideShowControlsForGranularPermission();//UAt-806
        }

        /// <summary>
        /// To perform search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarSearch_Click(object sender, EventArgs e)
        {
            if (!ddlUserGroup.SelectedValue.IsNullOrEmpty())
            {
                grdApplicantSearchData.Visible = true;
                if (ddlUserGroup.SelectedValue == AppConsts.MINUS_ONE.ToString() && ddlSearchUserGroup.Items.Where(s => s.Checked).Any())
                {
                    base.ShowErrorInfoMessage(AppConsts.MANAGE_APPLICANT_USER_GROUP_MAPPING_SEARCH_FILTER_ERROR);
                    return;
                }
            }
            if (ViewState["SelectedApplicants"] != null)
            {
                ViewState["SelectedApplicants"] = null;
            }
            ViewState["IsBind"] = null;
            //To reset grid filters 
            ResetGridFilters();

            if (Convert.ToInt32(ddlUserGroup.SelectedValue) > 0 && grdApplicantSearchData.Items.Count > 0)
            {
                fsucCmdBarButton.ClearButton.Style.Clear();
            }
            else
            {
                fsucCmdBarButton.ClearButton.Style.Add("display", "none");
            }

            grdApplicantSearchData.Focus();
        }

        /// <summary>
        /// To reset controls
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarReset_Click(object sender, EventArgs e)
        {
            CurrentViewContext.VirtualRecordCount = 0;
            ViewState["IsBind"] = null;
            if (ViewState["SelectedApplicants"] != null)
            {
                ViewState["SelectedApplicants"] = null;
            }
            Presenter.GetTenants();
            BindControls();
            txtFirstName.Text = String.Empty;
            txtUserID.Text = String.Empty;
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
            rbtnResults.SelectedValue = "0";
            dpOrderCreatedFrom.SelectedDate = null;
            dpOrderCreatedTo.SelectedDate = null;
            //To reset grid filters 
            ResetGridFilters();
            //Reset session
            Session[AppConsts.APPLICANT_SEARCH_SESSION_KEY] = null;
            if (ddlUserGroup.SelectedIndex <= 0)
            {
                fsucCmdBarButton.ClearButton.Style.Add("display", "none");
            }
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

        protected void btnAssign_Click(object sender, EventArgs e)
        {
            try
            {
                Presenter.AssignUserGroupToUsers(CurrentViewContext.AssignOrganizationUserIds);
                if (!String.IsNullOrEmpty(CurrentViewContext.SuccessMessage))
                {
                    base.ShowSuccessMessage(CurrentViewContext.SuccessMessage);
                    if (ViewState["SelectedApplicants"] != null)
                    {
                        ViewState["SelectedApplicants"] = null;
                    }
                    Presenter.SetAssignedUsersDic();
                }
                if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                {
                    base.ShowErrorMessage(CurrentViewContext.ErrorMessage);
                }
                if (!String.IsNullOrEmpty(CurrentViewContext.InfoMessage))
                {
                    base.ShowInfoMessage(CurrentViewContext.InfoMessage);
                }
                if (grdApplicantSearchData.Items.Count > 0)
                {
                    ViewState["IsBind"] = null;
                }
                else
                {
                    CurrentViewContext.ApplicantSearchData = new List<ApplicantDataList>();
                }
                grdApplicantSearchData.Rebind();
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
        /// Handles the assignment of users to user group.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        protected void chkSelectVerItem_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox checkBox = sender as CheckBox;
                Boolean isChecked = false;
                Boolean isMapped = false;
                if (checkBox.IsNull())
                {
                    return;
                }
                GridDataItem dataItem = (GridDataItem)checkBox.NamingContainer;
                Dictionary<Int32, Boolean> selectedItems = CurrentViewContext.AssignOrganizationUserIds;
                Int32 orgUserID = (Int32)dataItem.GetDataKeyValue("OrganizationUserId");
                isChecked = ((CheckBox)dataItem.FindControl("chkSelectItem")).Checked;
                if (((Label)(dataItem.FindControl("lblIsUserGroup"))).Text != String.Empty)
                {
                    isMapped = Convert.ToBoolean(((Label)(dataItem.FindControl("lblIsUserGroup"))).Text);
                }

                //if (selectedItems.IsNotNull() && selectedItems.ContainsKey(orgUserID) && isMapped)
                if (selectedItems.IsNotNull() && selectedItems.ContainsKey(orgUserID))
                {
                    selectedItems[orgUserID] = isChecked;
                }
                else if (selectedItems.IsNotNull() && selectedItems.ContainsKey(orgUserID) && !isMapped && !isChecked)
                {
                    selectedItems.Remove(orgUserID);
                    // selectedItems[orgUserID] = false;

                }
                else
                {
                    if (!selectedItems.ContainsKey(orgUserID))
                        selectedItems.Add(orgUserID, isChecked);
                }

                CurrentViewContext.AssignOrganizationUserIds = selectedItems;
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
                if (ViewState["IsBind"] == null)
                {
                    GridCustomPaging.CurrentPageIndex = CurrentPageIndex;
                    GridCustomPaging.PageSize = PageSize;
                    //UAT-1055: WB: As an admin, I should be able to search multiple nodes at a time.
                    //CurrentViewContext.DPM_ID = ucCustomAttributeLoaderSearch.DPM_ID;
                    CurrentViewContext.DPM_IDs = ucCustomAttributeLoaderSearch.DPM_ID;
                    CurrentViewContext.CustomFields = ucCustomAttributeLoaderSearch.GetCustomDataXML();
                    Presenter.PerformSearch();
                }
                grdApplicantSearchData.DataSource = CurrentViewContext.ApplicantSearchData;
                ApplicantDataList applicantDataList = CurrentViewContext.ApplicantSearchData.FirstOrDefault();
                if (!applicantDataList.IsNullOrEmpty())
                {
                    hdnTotalUsersAssigned.Value = Convert.ToString(applicantDataList.TotalUsersAssigned);
                    if (IsResult == 2)
                    {
                        hdnTotalUsersUnassigned.Value = "UnAssigned";
                    }
                    else
                    {
                        hdnTotalUsersUnassigned.Value = "";
                    }
                }
                else
                {
                    hdnTotalUsersAssigned.Value = "0";
                }

                //UAT-2210
                if (!ddlUserGroup.SelectedValue.IsNullOrEmpty() && Convert.ToInt32(ddlUserGroup.SelectedValue) == -1 && CurrentViewContext.IsResult == 0)
                {
                    hdnTotalUsersAssigned.Value = "NA";
                    hdnTotalUsersUnassigned.Value = "";
                }
                else if (!ddlUserGroup.SelectedValue.IsNullOrEmpty() && Convert.ToInt32(ddlUserGroup.SelectedValue) == -1 && CurrentViewContext.IsResult == 2)
                {
                    hdnTotalUsersAssigned.Value = applicantDataList.IsNullOrEmpty() ? "0" : Convert.ToString(applicantDataList.TotalUsersAssigned);
                    hdnTotalUsersUnassigned.Value = "UnAssigned";
                }


                //To set controls values in session
                SetSessionValues();
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
        protected void grdApplicantSearchData_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
                {
                    GridDataItem dataItem = (GridDataItem)e.Item;
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
                        ///Formatting SSN as ###-##-####
                        dataItem["SSN"].Text = Presenter.GetFormattedSSN(Convert.ToString(dataItem["SSN"].Text));
                    }
                    if (Convert.ToString(dataItem["UserGroups"].Text).Length > 20)
                    {
                        dataItem["UserGroups"].ToolTip = dataItem["UserGroups"].Text;
                        dataItem["UserGroups"].Text = (dataItem["UserGroups"].Text).ToString().Substring(0, 20) + "...";
                    }

                    String itemDataId = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrganizationUserId"].ToString();
                    Boolean isMapped = Convert.ToBoolean((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["IsUserGroupMatching"].ToString());
                    if (Convert.ToInt32(itemDataId) != 0)
                    {
                        Dictionary<Int32, Boolean> selectedItems = CurrentViewContext.AssignOrganizationUserIds;
                        if (selectedItems.IsNotNull())
                        {
                            if (selectedItems.ContainsKey(Convert.ToInt32(itemDataId)))
                            {
                                if (!isMapped && Convert.ToBoolean(selectedItems[Convert.ToInt32(itemDataId)].ToString()))
                                {
                                    CheckBox checkBox = ((CheckBox)e.Item.FindControl("chkSelectItem"));
                                    checkBox.Checked = true;
                                }
                                else if (isMapped)
                                {
                                    CheckBox checkBox = ((CheckBox)e.Item.FindControl("chkSelectItem"));
                                    checkBox.Checked = Convert.ToBoolean(selectedItems[Convert.ToInt32(itemDataId)].ToString());
                                }
                            }
                        }
                    }
                    else
                    {
                        CheckBox checkBox = ((CheckBox)e.Item.FindControl("chkSelectItem"));
                        checkBox.Enabled = false;
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
                            CheckBox checkBox = ((CheckBox)item.FindControl("chkSelectItem"));
                            if (checkBox.Checked)
                            {
                                checkCount++;
                            }
                        }
                        if (rowCount == checkCount)
                        {
                            GridHeaderItem headerItem = grdApplicantSearchData.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
                            CheckBox checkBox = ((CheckBox)headerItem.FindControl("chkSelectAll"));
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

        private String GetFormattedSSN(String unformattedSSN)
        {
            try
            {
                return ApplicationDataManager.GetFormattedSSN(unformattedSSN);
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
                return null;
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
                return null;
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


                #region For Filter command

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

                #region EXPORT FUNCTIONALITY
                //UAT-1051 Complio: Full social security number should not be displayed on grid exports. Added Duplicate Column with masked SSN
                // and displayed the masked column on Export instead of actual column.
                if (e.CommandName.IsNullOrEmpty())
                {
                    WclComboBox cmbExportFormat = e.Item.FindControl("cmbExportFormat") as WclComboBox;
                    if (cmbExportFormat.IsNotNull() && (cmbExportFormat.SelectedValue == "Csv" || cmbExportFormat.SelectedValue == "Pdf" || cmbExportFormat.SelectedValue == "Excel"))
                    {
                        grdApplicantSearchData.MasterTableView.GetColumn("_SSN").Display = true;

                    }
                }
                else if (e.CommandName == "Cancel")
                {
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
                    ViewState["SortExpression"] = e.SortExpression;
                    ViewState["SortDirection"] = e.NewSortOrder.Equals(GridSortOrder.Descending);
                    SortColumnNameType = e.NewSortOrder.Equals(GridSortOrder.Descending);
                    SortColumnName = e.SortExpression;
                    CurrentViewContext.GridCustomPaging.SortExpression = e.SortExpression;
                    CurrentViewContext.GridCustomPaging.SortDirectionDescending = e.NewSortOrder.Equals(GridSortOrder.Descending);
                }
                else
                {
                    ViewState["SortExpression"] = String.Empty;
                    ViewState["SortDirection"] = false;
                    SortColumnNameType = false;
                    SortColumnName = String.Empty;
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

        #endregion

        /// <summary>
        /// To bind Admin Program Study dropdown when Tenant Name changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTenantName_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            ViewState["IsBind"] = null;
            if (ddlTenantName.SelectedIndex <= 0)
            {
                ucCustomAttributeLoaderSearch.Reset();
            }
            else
            {
                ucCustomAttributeLoaderSearch.Reset(SelectedTenantId);

            }
            BindUserGroups();
            Presenter.GetArchiveStateList();
            if (ddlUserGroup.SelectedIndex <= 0)
            {
                fsucCmdBarButton.ClearButton.Style.Add("display", "none");
            }
        }

        /// <summary>
        /// To Reset filters on change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlUserGroup_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (!ddlUserGroup.SelectedValue.IsNullOrEmpty())
            {
                if (ddlUserGroup.SelectedValue == AppConsts.MINUS_ONE.ToString() && ddlSearchUserGroup.Items.Where(s => s.Checked).Any())
                {
                    base.ShowErrorInfoMessage(AppConsts.MANAGE_APPLICANT_USER_GROUP_MAPPING_SEARCH_FILTER_ERROR);
                    return;
                }
            }
            ViewState["IsBind"] = true;
            if (ViewState["SelectedApplicants"] != null)
            {
                ViewState["SelectedApplicants"] = null;
            }
            //Reset session
            Session[AppConsts.APPLICANT_SEARCH_SESSION_KEY] = null;

            txtFirstName.Text = String.Empty;
            txtUserID.Text = String.Empty;
            txtLastName.Text = String.Empty;
            dpkrDOB.SelectedDate = null;
            txtEmail.Text = String.Empty;
            txtSSN.Text = String.Empty;
            SSN = null;
            ucCustomAttributeLoaderSearch.Reset(SelectedTenantId);
            CurrentViewContext.VirtualRecordCount = 0;
            GridCustomPaging.CurrentPageIndex = 0;
            //To reset grid filters 
            CurrentViewContext.ApplicantSearchData = new List<ApplicantDataList>();
            grdApplicantSearchData.MasterTableView.SortExpressions.Clear();
            grdApplicantSearchData.CurrentPageIndex = 0;
            grdApplicantSearchData.MasterTableView.CurrentPageIndex = 0;
            grdApplicantSearchData.Rebind();
            if (ddlUserGroup.SelectedIndex > 0 && grdApplicantSearchData.Items.Count > 0)
            {
                fsucCmdBarButton.ClearButton.Style.Add("display", "block");
                fsucCmdBarButton.ClearButton.Style.Clear();
            }
            else
            {
                fsucCmdBarButton.ClearButton.Style.Add("display", "none");
            }

            ddlUserGroup.Focus();
        }

        /// <summary>
        /// Tenant Name DataBound event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTenantName_DataBound(object sender, EventArgs e)
        {
            ddlTenantName.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT--"));
        }

        /// <summary>
        /// Tenant Name DataBound event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlUserGroup_DataBound(object sender, EventArgs e)
        {
            //UAT-2210
            ddlUserGroup.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("Not Assigned", "-1"));
            ddlUserGroup.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT--"));
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
            }
            else
            {
                CurrentViewContext.SelectedTenantId = CurrentViewContext.TenantId;
            }
            BindUserGroups();
            Presenter.GetArchiveStateList();
        }

        /// <summary>
        /// To bind program dropdown
        /// </summary>
        private void BindUserGroups()
        {
            Presenter.GetAllUserGroups();
            ddlUserGroup.DataSource = CurrentViewContext.lstUserGroup;
            ddlUserGroup.DataBind();
            ddlSearchUserGroup.DataSource = CurrentViewContext.lstUserGroup;
            ddlSearchUserGroup.DataBind();
        }

        /// <summary>
        /// To set controls values in session
        /// </summary>
        private void SetSessionValues()
        {
            SearchItemDataContract searchDataContract = new SearchItemDataContract();

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
            //UAT-1688:As an admin, I should be able to select All/Active/Archived on the Manage Applicant User Group Mapping screen
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

                if (CurrentViewContext.SSN.ToUpper() == EnumSystemPermissionCode.MASKED_READ_PERMISSION.GetStringValue().ToUpper())
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

                OrganizationUserID = searchDataContract.OrganizationUserId;

                #region UAT-1088
                OrderCreatedFrom = searchDataContract.OrderCreatedFrom;
                OrderCreatedTo = searchDataContract.OrderCreatedTo;
                #endregion

                //UAT-1688:As an admin, I should be able to select All/Active/Archived on the Manage Applicant User Group Mapping screen
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
            //    txtSSN.Mask = AppConsts.SSN_MASK_FORMATE; //@"\#\#\#-\#\#-####";
            //}
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

        protected void ddlSearchUserGroup_DataBound(object sender, EventArgs e)
        {
            //  ddlSearchUserGroup.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT--",AppConsts.ZERO));
        }

        #endregion
    }
}

