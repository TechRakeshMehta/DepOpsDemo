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
using INTSOF.UI.Contract.ComplianceOperation;

#endregion

#endregion

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class DataEntryQueue : BaseUserControl, IDataEntryQueueView
    {

        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private DataEntryQueuePresenter _presenter = new DataEntryQueuePresenter();
        private List<Int32> _selectedTenantIds = null;
        private String _viewType;
        private DataEntryQueueFilterContract _dataEntryFilterContract = null;
        private bool _IsGridExportClick = false; // UAT-2499 || Bug ID: 16084 .
        #endregion

        #endregion

        #region Properties
        Int32 IDataEntryQueueView.TenantId
        {
            get
            {
                SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                if (user.IsNotNull())
                {
                    return user.TenantId.HasValue ? user.TenantId.Value : AppConsts.NONE;
                }
                return AppConsts.NONE;
            }
        }

        public DataEntryQueuePresenter Presenter
        {
            get
            {
                this._presenter.View = this;
                return this._presenter;
            }
            set
            {
                this._presenter = value;
                this._presenter.View = this;
            }
        }

        /// <summary>
        /// returns the object of type IItemsDataVerificationQueueView.
        /// </summary>
        public IDataEntryQueueView CurrentViewContext
        {
            get { return this; }
        }

        List<Entity.OrganizationUser> IDataEntryQueueView.lstOrganizationUser
        {
            set
            {
                cmbVerSelectedUser.DataSource = value;
                cmbVerSelectedUser.DataBind();
                cmbVerSelectedUser.Items.Insert(0, new RadComboBoxItem("--Select--"));
            }
        }
        /// <summary>
        /// list of tenants 
        /// </summary>
        public List<Entity.Tenant> lstTenant
        {
            get;
            set;
        }

        public Int32 CurrentLoggedInUserID
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        public String QueueType
        {
            get
            {
                if (ViewState["QueueType"].IsNotNull())
                {
                    return Convert.ToString(ViewState["QueueType"]);
                }
                return String.Empty;
            }
            set
            {
                ViewState["QueueType"] = value;
            }
        }

        /// <summary>
        /// Return the checked tenant from the combo box
        /// </summary>
        public List<Int32> SelectedTenantIds
        {
            get
            {
                _selectedTenantIds = new List<int>();
                foreach (RadComboBoxItem item in cmbTenantName.Items)
                {
                    if (item.Checked == true)
                        _selectedTenantIds.Add(Convert.ToInt32(item.Value));
                }
                return _selectedTenantIds;
            }
            set
            {
                _selectedTenantIds = value;
                foreach (RadComboBoxItem item in cmbTenantName.Items)
                {
                    if (_selectedTenantIds.Contains(Convert.ToInt32(item.Value)))
                        item.Checked = true;
                }
            }
        }

        public List<Int32> DocumentIdListToAssign
        {
            get
            {
                if (ViewState["DocumentIdListToAssign"].IsNotNull())
                {
                    return ViewState["DocumentIdListToAssign"] as List<Int32>;
                }
                return new List<Int32>();
            }
            set
            {
                ViewState["DocumentIdListToAssign"] = value;
            }
        }

        Boolean IDataEntryQueueView.IsAdminAssignmentQueue
        {
            get
            {
                if (QueueType.ToLower() == DataEntryQueueType.DATA_ENTRY_ASSIGNMENT_QUEUE.GetStringValue().ToLower())
                    return true;
                else
                    return false;
            }
        }

        List<DataEntryQueueContract> IDataEntryQueueView.lstDataEntryQueueDetail
        {
            get;
            set;
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
                return grdDataEntry.MasterTableView.CurrentPageIndex + 1;
            }
            set
            {
                //if (grdDataEntry.MasterTableView.CurrentPageIndex > 0)
                //{
                grdDataEntry.MasterTableView.CurrentPageIndex = value - 1;
                //}
            }
        }

        ///// <summary>
        ///// PageSize</summary>
        ///// <value>
        ///// Gets the value for PageSize.</value>
        public Int32 PageSize
        {
            get
            {
                // Maximum 100 record allowed from DB. 
                // return grdMultiTenantVerificationItemData.PageSize > 100 ? 100 : grdMultiTenantVerificationItemData.PageSize;
                return grdDataEntry.PageSize;
            }
            set
            {
                grdDataEntry.PageSize = value;
            }
        }

        ///// <summary>
        ///// VirtualPageCount</summary>
        ///// <value>
        ///// Sets the value for VirtualPageCount.</value>
        public Int32 VirtualPageCount
        {
            set
            {
                grdDataEntry.VirtualItemCount = value;
                grdDataEntry.MasterTableView.VirtualItemCount = value;
            }
        }

        /// <summary>
        /// get object of shared class of custom paging
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
                VirtualPageCount = value.VirtualPageCount;
                PageSize = value.PageSize;
                CurrentPageIndex = value.CurrentPageIndex;
            }
        }

        /// <summary>
        /// View Contract
        /// </summary>
        public DataEntryQueueFilterContract DataEntryFilterContract
        {
            get
            {
                if (_dataEntryFilterContract.IsNull())
                {
                    _dataEntryFilterContract = new DataEntryQueueFilterContract();
                }
                return _dataEntryFilterContract;
            }
        }

        #endregion
        #region UAT-3354
        /// <summary>
        /// NodeID's of the Selected DPM Nodes - UAT 1055
        /// </summary>
        public String NodeIds
        {
            get;
            set;
        }

        ///// <summary>
        ///// DPMID's of the Selected Nodes - UAT 1055
        ///// </summary>
        //public String DPMIds
        //{
        //    get;
        //    set;
        //}

        //public String CustomDataXML
        //{
        //    get;
        //}

        //public String NodeLabel
        //{
        //    get;
        //    set;
        //}
        #endregion
        #endregion


        #region Events

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    BindTenant();
                    Dictionary<String, String> args = new Dictionary<String, String>();
                    if (!Request.QueryString["args"].IsNull())
                    {
                        args.ToDecryptedQueryString(Request.QueryString["args"]);
                        if (args.ContainsKey("QueueType"))
                        {
                            GetSessionValues();
                        }
                        else
                        {
                            SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.SESSION_KEY_DATA_ENTRY_QUEUE, null);
                        }
                    }
                    else
                    {
                        SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.SESSION_KEY_DATA_ENTRY_QUEUE, null);
                    }
                }
                HideShowControlsAndSetHeader();
                CmdBarSearch.SaveButton.ValidationGroup = "grpFormSubmit";
                if (CurrentViewContext.SelectedTenantIds.Count == AppConsts.ONE)
                {
                    dvMultipleInstitutionHierarchy.Visible = true;
                    lblinstituteHierarchy.Text = hdnHierarchyLabel.Value;
                    hdnSelectedTenantID.Value = Convert.ToString(CurrentViewContext.SelectedTenantIds.FirstOrDefault());
                }
                else
                    ResetInstitutionHierarchy();
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

        protected void grdDataEntry_Init(object sender, EventArgs e)
        {
            try
            {
                GridFilterMenu menu = grdDataEntry.FilterMenu;

                if (grdDataEntry.clearFilterMethod == null)
                    grdDataEntry.clearFilterMethod = new WclGrid.ClearFilters(ClearViewStatesForFilter);

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
        /// Retrieves a list of Multi Institution Assignment Data.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        /// <remarks></remarks>
        protected void grdDataEntry_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Session["lstPkgSubForDocDiscard"] = null; //UAT-2742
                Session["IsAnySubsForSaveAndDone"] = null;//UAT-2742

                SetGridFilters();

                GridCustomPaging.CurrentPageIndex = CurrentPageIndex;
                GridCustomPaging.PageSize = PageSize;
                GridCustomPaging.DefaultSortExpression = "DateUploaded";
                if (CurrentViewContext.SelectedTenantIds.Count == AppConsts.ONE)
                {
                    if (!hdnDepartmntPrgrmMppng.Value.IsNullOrEmpty())
                        CurrentViewContext.NodeIds = hdnDepartmntPrgrmMppng.Value;
                }
                Presenter.GetDataEntryAssignmentData();
                grdDataEntry.DataSource = CurrentViewContext.lstDataEntryQueueDetail;

                if (CurrentViewContext.lstDataEntryQueueDetail.IsNotNull() && CurrentViewContext.lstDataEntryQueueDetail.Count > AppConsts.NONE
                     && CurrentViewContext.IsAdminAssignmentQueue
                    )
                {
                    pnlShowUsers.Visible = true;
                    Presenter.GetUserListForSelectedTenant();
                }
                else
                {
                    pnlShowUsers.Visible = false;
                }
                SetGridFilterValues();
                //Hide columns for User work queue.
                grdDataEntry.MasterTableView.Columns.FindByUniqueName("AssignToUserName").Visible = CurrentViewContext.IsAdminAssignmentQueue;
                if (!_IsGridExportClick) // UAT-2499 || Bug ID: 16084 .
                {
                    grdDataEntry.MasterTableView.Columns.FindByUniqueName("SelectDocuments").Visible = CurrentViewContext.IsAdminAssignmentQueue;
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

        protected void grdDataEntry_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
                {
                    String[] checkedFVDIDs = null;
                    if (hdnFDEQ_IDs.Value != null && !hdnFDEQ_IDs.Value.ToString().IsNullOrEmpty())
                    {
                        checkedFVDIDs = hdnFDEQ_IDs.Value.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                        if (checkedFVDIDs.IsNotNull())
                        {
                            String FDEQ_ID = Convert.ToString((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FDEQ_ID"]);
                            if (!String.IsNullOrEmpty(FDEQ_ID))
                            {
                                if (checkedFVDIDs.Any(cond => cond == FDEQ_ID))
                                {
                                    CheckBox checkBox = ((CheckBox)e.Item.FindControl("chkSelectItem"));
                                    checkBox.Checked = true;
                                }
                            }
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

        protected void grdDataEntry_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == RadGrid.FilterCommandName)
                {
                    SetGridFilters();
                    Pair filter = (Pair)e.CommandArgument;

                    Int32 filterIndex = CurrentViewContext.GridCustomPaging.FilterColumns.IndexOf(filter.Second.ToString());
                    if (filter.First.ToString() != GridKnownFunction.NoFilter.ToString())
                    {
                        String filteringType = grdDataEntry.MasterTableView.Columns.FindByUniqueName(filter.Second.ToString()).DataTypeName;
                        String filterValue = grdDataEntry.MasterTableView.Columns.FindByUniqueName(filter.Second.ToString()).CurrentFilterValue;

                        if (filterIndex != -1)
                        {
                            CurrentViewContext.GridCustomPaging.FilterTypes[filterIndex] = filteringType;
                            CurrentViewContext.GridCustomPaging.FilterOperators[filterIndex] = filter.First.ToString();
                            CurrentViewContext.GridCustomPaging.FilterValues[filterIndex] = filterValue;
                        }
                        else
                        {
                            CurrentViewContext.GridCustomPaging.FilterTypes.Add(filteringType);
                            CurrentViewContext.GridCustomPaging.FilterColumns.Add(filter.Second.ToString());
                            CurrentViewContext.GridCustomPaging.FilterOperators.Add(filter.First.ToString());
                            CurrentViewContext.GridCustomPaging.FilterValues.Add(filterValue);
                        }
                    }
                    else if (filterIndex != -1)
                    {
                        CurrentViewContext.GridCustomPaging.FilterOperators.RemoveAt(filterIndex);
                        CurrentViewContext.GridCustomPaging.FilterValues.RemoveAt(filterIndex);
                        CurrentViewContext.GridCustomPaging.FilterColumns.RemoveAt(filterIndex);
                        CurrentViewContext.GridCustomPaging.FilterTypes.RemoveAt(filterIndex);
                    }

                    ViewState["FilterColumns"] = CurrentViewContext.GridCustomPaging.FilterColumns;
                    ViewState["FilterOperators"] = CurrentViewContext.GridCustomPaging.FilterOperators;
                    ViewState["FilterValues"] = CurrentViewContext.GridCustomPaging.FilterValues;
                    ViewState["FilterTypes"] = CurrentViewContext.GridCustomPaging.FilterTypes;
                }
                #region Detail Screen Navigation
                if (e.CommandName.Equals("ViewDetail"))
                {
                    Int32 selectedTenantID = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TenantID"]);
                    Int32 applicantID = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ApplicantOrganizationUserID"]);
                    Int32 applicantDocumentID = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ApplicantDocumentID"]);
                    Int32 FDEQ_ID = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FDEQ_ID"]);
                    //UAT-2456:Admin Data Entry: "Discard Documents" should have to go through the process twice
                    Int32 DiscardDocumentCount = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["DiscardDocumentCount"]);

                    List<PackageSubscriptionForDataEntry> lstPackageSubscription = new List<PackageSubscriptionForDataEntry>();
                    hdnApplicantUserID.Value = applicantID.ToString();
                    hdnSelectedTenantID.Value = selectedTenantID.ToString();
                    hdnApplicantDocumentID.Value = applicantDocumentID.ToString();
                    hdnFDEQ_ID.Value = FDEQ_ID.ToString();
                    //UAT-2456:Admin Data Entry: "Discard Documents" should have to go through the process twice
                    hdnDiscardDocumentCount.Value = DiscardDocumentCount.ToString();
                    //Get list of package subscription to check the count of subscriptions, If applicant have only one subscription then directly redirect to the data entry 
                    //detail page else open the popup window to select package subscription.
                    lstPackageSubscription = Presenter.GetPackageSubscriptionOfApplicant(selectedTenantID, applicantID);
                    GridDataItem GridItem = (GridDataItem)e.Item;
                    String DocumentName = GridItem["DocumentName"].Text;
                    //Session["DocumentName"] = DocumentName; -- will use Other way
                    //DataEntryQueueContract data =e.Item as DataEntryQueueContract;
                    if (!lstPackageSubscription.IsNullOrEmpty() && lstPackageSubscription.Count > AppConsts.NONE)
                    {
                        if (lstPackageSubscription.Count == AppConsts.ONE)
                        {
                            Int32 packageSubscriptionID = lstPackageSubscription.First().PackageSubscriptionID;

                            //UAT-2456:Admin Data Entry: "Discard Documents" should have to go through the process twice
                            RedirectToDetailPage(selectedTenantID.ToString(), packageSubscriptionID.ToString(), applicantDocumentID.ToString(), FDEQ_ID.ToString(), DiscardDocumentCount.ToString());
                        }
                        else
                        {
                            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "OpenMutlipleSubscriptionsPopup();", true);
                        }
                    }
                    else
                    {
                        base.ShowInfoMessage("No active subscription found for this document.");
                    }

                }
                #endregion

                if (e.CommandName.IsNullOrEmpty())  // UAT-2499 || Bug ID: 16084 .
                {
                    _IsGridExportClick = true;
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
        protected void grdDataEntry_SortCommand(object sender, GridSortCommandEventArgs e)
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

        #endregion

        #region Button Events

        /// <summary>
        /// This event is used to search the assignment data in multi institutions based on the selected tenants
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">EventArgs</param>
        protected void CmdBarSearch_SaveClick(object sender, EventArgs e)
        {
            try
            {
                hdnFDEQ_IDs.Value = null;
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ResetSelectedItems();", true);
                grdDataEntry.Rebind();
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
        /// This event is used to reset the screen data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarSearch_SubmitClick(object sender, EventArgs e)
        {
            try
            {
                SelectedTenantIds = new List<int>();
                hdnFDEQ_IDs.Value = null;
                cmbTenantName.ClearCheckedItems();
                grdDataEntry.MasterTableView.SortExpressions.Clear();
                grdDataEntry.MasterTableView.FilterExpression = "";
                foreach (GridColumn column in grdDataEntry.MasterTableView.OwnerGrid.Columns)
                {
                    column.CurrentFilterFunction = GridKnownFunction.NoFilter;
                    column.CurrentFilterValue = string.Empty;
                }
                grdDataEntry.MasterTableView.FilterExpression = string.Empty;
                ClearViewStatesForFilter();
                ResetInstitutionHierarchy();
                grdDataEntry.Rebind();
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
        /// This event is used to redirect the user on dashboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarSearch_CancelClick(object sender, EventArgs e)
        {
            try
            {
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

        /// <summary>
        /// This event is used to assignment of data entry record to the specific user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAssignUser_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbVerSelectedUser.SelectedValue != "")
                {
                    Int32 selectedAssigneeUserId = Convert.ToInt32(cmbVerSelectedUser.SelectedValue);
                    String selectedAsigneeUserName = cmbVerSelectedUser.SelectedItem.Text.Trim();
                    if (Presenter.AssignDocumentsToUser(selectedAssigneeUserId, selectedAsigneeUserName))
                    {
                        CurrentViewContext.DocumentIdListToAssign = new List<Int32>();
                        cmbVerSelectedUser.SelectedIndex = 0;
                        base.ShowSuccessMessage("Document(s) assigned to the user successfully.");
                        hdnFDEQ_IDs.Value = null;
                        System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ResetSelectedItems();", true);
                        grdDataEntry.Rebind();
                    }
                    else
                    {
                        base.ShowInfoMessage("Please select atleast one document to be assigned.");
                    }
                }
                else
                {
                    base.ShowInfoMessage("Please select a user to assign document(s).");
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

        protected void btnRedirect_Click(object sender, EventArgs e)
        {
            try
            {
                //UAT-2456:Admin Data Entry: "Discard Documents" should have to go through the process twice
                RedirectToDetailPage(hdnSelectedTenantID.Value, hdnPackageSubscriptionID.Value, hdnApplicantDocumentID.Value, hdnFDEQ_ID.Value, hdnDiscardDocumentCount.Value);
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

        protected void btnPostback_Click(object sender, EventArgs e)
        {
            try
            {
                if (CurrentViewContext.SelectedTenantIds.Count > AppConsts.NONE && CurrentViewContext.SelectedTenantIds.Count == AppConsts.ONE)
                {
                    dvMultipleInstitutionHierarchy.Visible = true;
                    hdnSelectedTenantID.Value = Convert.ToString(CurrentViewContext.SelectedTenantIds.FirstOrDefault());
                    lblinstituteHierarchy.Text = String.Empty;
                    // ucCustomAttributeLoaderNodeSearch.TenantId = CurrentViewContext.SelectedTenantIds.FirstOrDefault();
                    // ucCustomAttributeLoaderNodeSearch.ScreenType = "CommonScreen";
                }
                else
                {
                    dvMultipleInstitutionHierarchy.Visible = false;
                    // ucCustomAttributeLoaderNodeSearch.Reset();
                    ResetInstitutionHierarchy();
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
                if (checkBox.IsNull())
                {
                    return;
                }
                GridDataItem dataItem = (GridDataItem)checkBox.NamingContainer;
                List<Int32> _documentIdListToAssign = CurrentViewContext.DocumentIdListToAssign;
                Int32 FDEQ_ID = (Int32)dataItem.GetDataKeyValue("FDEQ_ID");
                isChecked = ((CheckBox)dataItem.FindControl("chkSelectItem")).Checked;

                if (_documentIdListToAssign.IsNotNull() && !_documentIdListToAssign.Contains(FDEQ_ID) && isChecked)
                {
                    _documentIdListToAssign.Add(FDEQ_ID);
                }
                else if (_documentIdListToAssign.IsNotNull() && _documentIdListToAssign.Contains(FDEQ_ID) && !isChecked)
                {
                    _documentIdListToAssign.Remove(FDEQ_ID);
                }

                CurrentViewContext.DocumentIdListToAssign = _documentIdListToAssign;
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

        /// <summary>
        /// Bind tenant combo box
        /// </summary>
        private void BindTenant()
        {
            Presenter.GetTenantList();
            cmbTenantName.DataSource = lstTenant;
            cmbTenantName.DataBind();
        }

        /// <summary>
        /// Method to hide and show Assign to user panel and also set the page header.
        /// </summary>
        private void HideShowControlsAndSetHeader()
        {
            pnlShowUsers.Visible = CurrentViewContext.IsAdminAssignmentQueue;
            lblPageHdr.Text = "Pending Documents";

            //if (CurrentViewContext.IsAdminAssignmentQueue)
            //{
            //    lblPageHdr.Text = "Pending Documents";
            //}
            //else
            //{
            //    lblPageHdr.Text = "Pending Document";
            //}
        }

        /// <summary>
        /// Method to set the Grid filter in contract.
        /// </summary>
        private void SetGridFilters()
        {
            if (!ViewState["SortExpression"].IsNull())
            {
                CurrentViewContext.GridCustomPaging.SortExpression = Convert.ToString(ViewState["SortExpression"]);
                CurrentViewContext.GridCustomPaging.SortDirectionDescending = Convert.ToBoolean(ViewState["SortDirection"]);
            }

            CurrentViewContext.GridCustomPaging.FilterColumns = ViewState["FilterColumns"] == null ? new List<String>() : (List<String>)(ViewState["FilterColumns"]);
            CurrentViewContext.GridCustomPaging.FilterOperators = ViewState["FilterOperators"] == null ? new List<String>() : (List<String>)(ViewState["FilterOperators"]);
            CurrentViewContext.GridCustomPaging.FilterValues = ViewState["FilterValues"] == null ? new ArrayList() : (ArrayList)(ViewState["FilterValues"]);
            CurrentViewContext.GridCustomPaging.FilterTypes = ViewState["FilterTypes"] == null ? new List<String>() : (List<String>)(ViewState["FilterTypes"]);
        }

        private void SetGridFilterValues()
        {
            if (!CurrentViewContext.GridCustomPaging.FilterColumns.IsNullOrEmpty() && CurrentViewContext.GridCustomPaging.FilterColumns.Count > 0)
            {
                CurrentViewContext.GridCustomPaging.FilterColumns.ForEach(x =>
                    grdDataEntry.Columns.FindByUniqueName(x).CurrentFilterValue = CurrentViewContext.GridCustomPaging.FilterValues[CurrentViewContext.GridCustomPaging.FilterColumns.IndexOf(x)].ToString()
                    );
            }
        }

        /// <summary>
        /// Method to redirect on data entry detail page 
        /// </summary>
        /// <param name="selectedTenantID">Selected document tenantID</param>
        /// <param name="packageSubscriptionID"> selected package subscription id</param>
        /// <param name="applicantDocumentID">selected applicant document id</param>
        /// <param name="FDEQ_ID"></param>
        /// <param name="applicantId">OrganizationUserID of the Applicant to wghom the document belongs to.</param>
        private void RedirectToDetailPage(String selectedTenantID, String packageSubscriptionID, String applicantDocumentID, String FDEQ_ID, String discardDocumentCount)
        {
            SetSessionValues();
            Dictionary<String, String> queryString = new Dictionary<String, String>();
            queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "TenantId", selectedTenantID },
                                                                    { "Child", ChildControls.DataEntryViewDetail},
                                                                    { "PkgSubId", packageSubscriptionID },
                                                                    { "AppDocId", applicantDocumentID },
                                                                    { "QueueType", QueueType },
                                                                    {"FDEQ_ID" ,FDEQ_ID},
                                                                    {"ApplicantId" , hdnApplicantUserID.Value},
                                                                    //UAT-2456:Admin Data Entry: "Discard Documents" should have to go through the process twice
                                                                    {"DiscardDocumentCount" ,discardDocumentCount},
                                                                 };
            string url = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
            Response.Redirect(url, true);
        }

        /// <summary>
        /// Set filters values in session.
        /// </summary>
        private void SetSessionValues()
        {
            DataEntryQueueFilterContract dataEntryQueueFilters = new DataEntryQueueFilterContract();
            dataEntryQueueFilters.QueueType = QueueType;
            dataEntryQueueFilters.SelectedTenantIds = CurrentViewContext.SelectedTenantIds;
            dataEntryQueueFilters.CurrentPageIndex = GridCustomPaging.CurrentPageIndex;
            dataEntryQueueFilters.PageSize = GridCustomPaging.PageSize;
            dataEntryQueueFilters.VirtualPageCount = grdDataEntry.MasterTableView.VirtualItemCount;
            dataEntryQueueFilters.DefaultSortExpression = GridCustomPaging.DefaultSortExpression;
            dataEntryQueueFilters.SecondarySortExpression = GridCustomPaging.SecondarySortExpression;
            #region Set Grid Filters in Session

            dataEntryQueueFilters.FilterColumns = GridCustomPaging.FilterColumns;
            dataEntryQueueFilters.FilterOperators = GridCustomPaging.FilterOperators;
            dataEntryQueueFilters.FilterValues = GridCustomPaging.FilterValues;
            dataEntryQueueFilters.FilterTypes = GridCustomPaging.FilterTypes;

            #endregion
            //lblinstituteHierarchy
            dataEntryQueueFilters.InstituteHierarchyLabel = lblinstituteHierarchy.Text;
            if (CurrentViewContext.SelectedTenantIds.Count() == AppConsts.ONE)
                dataEntryQueueFilters.DepartmntPrgrmMppngIds = hdnDepartmntPrgrmMppng.Value;
            else
                dataEntryQueueFilters.DepartmntPrgrmMppngIds = String.Empty;
            SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.SESSION_KEY_DATA_ENTRY_QUEUE, dataEntryQueueFilters);
        }

        /// <summary>
        /// Get filter values from session.
        /// </summary>
        private void GetSessionValues()
        {
            DataEntryQueueFilterContract dataEntryQueueFilters = (DataEntryQueueFilterContract)SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_KEY_DATA_ENTRY_QUEUE);
            if (!dataEntryQueueFilters.IsNullOrEmpty())
            {
                if (!dataEntryQueueFilters.QueueType.IsNullOrEmpty())
                {
                    QueueType = dataEntryQueueFilters.QueueType;
                }
                if (dataEntryQueueFilters.SelectedTenantIds.IsNotNull() && dataEntryQueueFilters.SelectedTenantIds.Count > 0)
                {
                    CurrentViewContext.SelectedTenantIds = dataEntryQueueFilters.SelectedTenantIds;
                }
                GridCustomPaging.CurrentPageIndex = CurrentPageIndex = dataEntryQueueFilters.CurrentPageIndex;
                GridCustomPaging.PageSize = PageSize = dataEntryQueueFilters.PageSize;
                ViewState["FilterColumns"] = CurrentViewContext.GridCustomPaging.FilterColumns = dataEntryQueueFilters.FilterColumns;
                ViewState["FilterOperators"] = CurrentViewContext.GridCustomPaging.FilterOperators = dataEntryQueueFilters.FilterOperators;
                ViewState["FilterValues"] = CurrentViewContext.GridCustomPaging.FilterValues = dataEntryQueueFilters.FilterValues;
                ViewState["FilterTypes"] = CurrentViewContext.GridCustomPaging.FilterTypes = dataEntryQueueFilters.FilterTypes;
                SetGridFilterValues();
                hdnHierarchyLabel.Value = dataEntryQueueFilters.InstituteHierarchyLabel;
                hdnDepartmntPrgrmMppng.Value = dataEntryQueueFilters.DepartmntPrgrmMppngIds;
                SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.SESSION_KEY_DATA_ENTRY_QUEUE, null);
            }
            SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.SESSION_KEY_DATA_ENTRY_QUEUE, null);
        }

        private void ResetInstitutionHierarchy()
        {
            lblinstituteHierarchy.Text = String.Empty;
            hdnDepartmntPrgrmMppng.Value = String.Empty;
            hdnHierarchyLabel.Value = String.Empty;
            hdnInstitutionNodeId.Value = String.Empty;
            dvMultipleInstitutionHierarchy.Visible = false;
        }
        #endregion

        #region Public Methods

        public void ClearViewStatesForFilter()
        {
            ViewState["FilterColumns"] = null;
            ViewState["FilterOperators"] = null;
            ViewState["FilterValues"] = null;
            ViewState["FilterTypes"] = null;
        }
        #endregion

        #endregion
    }
}