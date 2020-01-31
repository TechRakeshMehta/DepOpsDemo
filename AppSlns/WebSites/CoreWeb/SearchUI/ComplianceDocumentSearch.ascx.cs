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
using Entity.ClientEntity;
using CoreWeb.Shell;
using System.Xml.Serialization;
using System.IO;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.UI.Contract.SearchUI;
using System.Text;
using System.Web.UI;
using System.Threading;
using CoreWeb.IntsofSecurityModel;
using Business.RepoManagers;
using System.Web.Configuration;
using Ionic.Zip;

#endregion

#endregion

namespace CoreWeb.Search.Views
{
    public partial class ComplianceDocumentSearch : BaseUserControl, IComplianceDocumentSearchView
    {

        #region Variables

        private ComplianceDocumentSearchPresenter _presenter = new ComplianceDocumentSearchPresenter();
        private String _viewType;
        private Int32 tenantId = 0;
        private CustomPagingArgsContract _gridCustomPaging = null;
        private SearchItemDataContract _gridSearchContract = null;

        #endregion

        #region Properties


        public ComplianceDocumentSearchPresenter Presenter
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

        public IComplianceDocumentSearchView CurrentViewContext
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

        public List<UserGroup> lstUserGroup
        {
            get;
            set;
        }

        public List<ComplianceItem> lstComplianceItem { get; set; }

        public List<Int32> SelectedUserGroupIds
        {
            get
            {
                List<Int32> selectedIds = new List<Int32>();
                for (Int32 i = 0; i < ddlUserGroup.Items.Count; i++)
                {
                    if (ddlUserGroup.Items[i].Checked)
                    {
                        selectedIds.Add(Convert.ToInt32(ddlUserGroup.Items[i].Value));
                    }
                }
                return selectedIds;
            }
            set
            {
                for (Int32 i = 0; i < ddlUserGroup.Items.Count; i++)
                {
                    ddlUserGroup.Items[i].Checked = value.Contains(Convert.ToInt32(ddlUserGroup.Items[i].Value));
                }
            }
        }

        //Commented below code for UAT-1175:Update Category and Document dropdowns to only display one per unique value (from name or label whichever is used)
        //public List<Int32> SelectedComplianceItemIds
        //{
        //    get
        //    {
        //        List<Int32> selectedIds = new List<Int32>();
        //        for (Int32 i = 0; i < chkComplianceItem.Items.Count; i++)
        //        {
        //            if (chkComplianceItem.Items[i].Checked)
        //            {
        //                selectedIds.Add(Convert.ToInt32(chkComplianceItem.Items[i].Value));
        //            }
        //        }
        //        return selectedIds;
        //    }
        //    set
        //    {
        //        for (Int32 i = 0; i < chkComplianceItem.Items.Count; i++)
        //        {
        //            chkComplianceItem.Items[i].Checked = value.Contains(Convert.ToInt32(chkComplianceItem.Items[i].Value));
        //        }

        //    }

        //}

        //UAT-1175:Update Category and Document dropdowns to only display one per unique value (from name or label whichever is used)
        public List<String> SelectedComplianceItemNames
        {
            get
            {
                List<String> selectedItems = new List<String>();
                for (Int32 i = 0; i < chkComplianceItem.Items.Count; i++)
                {
                    if (chkComplianceItem.Items[i].Checked)
                    {
                        selectedItems.Add(chkComplianceItem.Items[i].Text);
                    }
                }
                return selectedItems;
            }
            set
            {
                for (Int32 i = 0; i < chkComplianceItem.Items.Count; i++)
                {
                    chkComplianceItem.Items[i].Checked = value.Contains(chkComplianceItem.Items[i].Text);
                }

            }

        }

        public Int32 DPM_ID
        {
            get;
            set;
        }

        public String DPM_IDs
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

        public List<ComplianceDocumentSearchContract> ComplianceDocumentList { get; set; }

        /// <summary>
        /// Gets or Sets the value for selected Items.
        /// </summary>
        public Dictionary<Int32, ComplianceDocumentSearchContract> DocumentListToExport
        {
            get
            {
                if (!ViewState["SelectedDocuments"].IsNull())
                {
                    return ViewState["SelectedDocuments"] as Dictionary<Int32, ComplianceDocumentSearchContract>;
                }

                return new Dictionary<Int32, ComplianceDocumentSearchContract>();
            }
            set
            {
                ViewState["SelectedDocuments"] = value;
            }
        }

        //UAT 2566
        DateTime? IComplianceDocumentSearchView.DocumentFromDate
        {
            get
            {
                return dpFromDate.SelectedDate;
            }
            set
            {
                if (!value.IsNullOrEmpty())
                {
                    dpFromDate.SelectedDate = value;
                }
            }

        }

        //UAT 2566
        DateTime? IComplianceDocumentSearchView.DocumentToDate
        {
            get
            {
                return dpToDate.SelectedDate;
            }
            set
            {
                if (!value.IsNullOrEmpty())
                {
                    dpToDate.SelectedDate = value;
                }
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
                return grdComplianceDocSearch.MasterTableView.CurrentPageIndex + 1;
            }
            set
            {
                if (grdComplianceDocSearch.MasterTableView.CurrentPageIndex > 0)
                {
                    grdComplianceDocSearch.MasterTableView.CurrentPageIndex = value - 1;
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
                return grdComplianceDocSearch.PageSize;
            }
            set
            {
                grdComplianceDocSearch.PageSize = value;
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
                grdComplianceDocSearch.VirtualItemCount = value;
                grdComplianceDocSearch.MasterTableView.VirtualItemCount = value;
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

        #endregion

        #region Events

        #region Page Events
        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                base.Title = "Compliance Document Search";
                base.SetPageTitle("Compliance Document Search");

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
                    grdComplianceDocSearch.Visible = false;
                    fsucCmdExport.Visible = false;
                    Presenter.OnViewInitialized();
                    BindControls();

                    CmdBarSearch.SaveButton.ValidationGroup = "grpFormSubmit";
                    CmdBarSearch.ClearButton.ValidationGroup = "grpFormSubmit";
                }
                ifrExportDocument.Src = String.Empty;
                Presenter.OnViewLoaded();
                hdnTenantId.Value = SelectedTenantId.ToString();
                lblinstituteHierarchy.Text = hdnHierarchyLabel.Value.HtmlEncode();
                fsucCmdExport.ExtraButton.ToolTip = "Click to Export the selected document";
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
                grdComplianceDocSearch.Visible = true;
                fsucCmdExport.Visible = true;
                //Reset Document list to export on search click.
                DocumentListToExport = new Dictionary<Int32, ComplianceDocumentSearchContract>();
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
                Dictionary<String, String> queryString = new Dictionary<String, String>();
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

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (CurrentViewContext.DocumentListToExport != null && CurrentViewContext.DocumentListToExport.Count > 0)
                {
                    ExportDocuments();
                }
                else
                {
                    base.ShowInfoMessage("Please select Document(s) to Export.");
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
        //UAT-1547 Add Export documents button at the top as well as the bottom (current position) in 'Compliance Doc Search. 
        protected void CmdBarSearch_ExportClick(object sender, EventArgs e)
        {
            try
            {
                if (CurrentViewContext.DocumentListToExport != null && CurrentViewContext.DocumentListToExport.Count > 0)
                {
                    ExportDocuments();
                }
                else
                {
                    base.ShowInfoMessage("Please select Document(s) to Export.");
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
        /// To bind Admin Program Study dropdown when Tenant Name changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTenantName_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                //Reset Document list to export on institution change.
                DocumentListToExport = new Dictionary<Int32, ComplianceDocumentSearchContract>();
                ResetInstitutionHierarchy();
                if (ddlTenantName.SelectedValue.IsNullOrEmpty() || SelectedTenantId == AppConsts.NONE)
                {
                    ResetControls();
                    ResetGridFilters();
                }
                BindUserGroups();
                BindComplianceItems();
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

        /// <summary>
        /// Handel selected document 
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        protected void chkSelectDocument_CheckedChanged(object sender, EventArgs e)
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
                Dictionary<Int32, ComplianceDocumentSearchContract> documentExportList = CurrentViewContext.DocumentListToExport;
                Int32 applicantDocumentID = (Int32)dataItem.GetDataKeyValue("ApplicantDocumentID");
                Int32 applicantDocumentSearchdataID = (Int32)dataItem.GetDataKeyValue("ID");
                Int32 applicantID = (Int32)dataItem.GetDataKeyValue("ApplicantID");
                String documentPath = ((Label)dataItem.FindControl("lblDocumentPath")).Text.Trim();
                String fileName = ((Label)dataItem.FindControl("lblFileName")).Text.Trim();
                String ApplicantName = Convert.ToString(dataItem["ApplicantName"].Text.Trim());
                isChecked = ((CheckBox)dataItem.FindControl("chkSelectDocument")).Checked;

                if (documentExportList.IsNotNull() && !documentExportList.ContainsKey(applicantDocumentSearchdataID) && isChecked)
                {

                    ComplianceDocumentSearchContract tempDocumentData = new ComplianceDocumentSearchContract();
                    tempDocumentData.ID = applicantDocumentSearchdataID;
                    tempDocumentData.ApplicantDocumentID = applicantDocumentID;
                    tempDocumentData.DocumentPath = documentPath;
                    tempDocumentData.FileName = fileName;
                    tempDocumentData.ApplicantID = applicantID;
                    tempDocumentData.ApplicantName = ApplicantName;
                    documentExportList.Add(applicantDocumentSearchdataID, tempDocumentData);
                }
                else if (documentExportList.IsNotNull() && documentExportList.ContainsKey(applicantDocumentSearchdataID) && !isChecked)
                {
                    documentExportList.Remove(applicantDocumentSearchdataID);
                }

                CurrentViewContext.DocumentListToExport = documentExportList;
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
        protected void grdComplianceDocSearch_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                   GridCustomPaging.CurrentPageIndex = CurrentPageIndex;
                    GridCustomPaging.PageSize = PageSize;
                    CurrentViewContext.GridCustomPaging = GridCustomPaging;
                    if (!hdnDepartmntPrgrmMppng.Value.IsNullOrEmpty())
                        CurrentViewContext.DPM_IDs = hdnDepartmntPrgrmMppng.Value;
                    Presenter.PerformSearch();
                    grdComplianceDocSearch.DataSource = CurrentViewContext.ComplianceDocumentList;
                
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

        protected void grdComplianceDocSearch_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
                {
                    ComplianceDocumentSearchContract compDoc = e.Item.DataItem as ComplianceDocumentSearchContract;
                    if (!compDoc.IsNullOrEmpty())
                    {
                        HtmlAnchor anchor = (HtmlAnchor)e.Item.FindControl("ancManageDocument");
                        //anchor.HRef = String.Format("/ComplianceOperations/UserControl/DoccumentDownload.aspx?documentId={0}&tenantId={1}&IsPDFFileDownload={2}", compDoc.ApplicantDocumentID, SelectedTenantId, true);
                        //Above line of code is commented In UAT-3962 and below is added.
                        anchor.HRef = String.Format("/ComplianceOperations/UserControl/DoccumentDownload.aspx?documentId={0}&tenantId={1}&IsPDFFileDownload={2}&IsComplianceIndividualDoc={3}", compDoc.ApplicantDocumentID, SelectedTenantId, true,true);
                    }

                    String applicantDocumentSearchDataID = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ID"].ToString();
                    Dictionary<Int32, ComplianceDocumentSearchContract> selectedDocumentExportList = CurrentViewContext.DocumentListToExport;

                    if (selectedDocumentExportList.IsNotNull() && Convert.ToInt32(applicantDocumentSearchDataID) != AppConsts.NONE)
                    {
                        if (selectedDocumentExportList.ContainsKey(Convert.ToInt32(applicantDocumentSearchDataID)))
                        {
                            CheckBox checkBox = ((CheckBox)e.Item.FindControl("chkSelectDocument"));
                            checkBox.Checked = true;
                        }
                    }
                    //else
                    //{
                    //    CheckBox checkBox = ((CheckBox)e.Item.FindControl("chkSelectDocument"));
                    //    checkBox.Enabled = false;
                    //}
                }
                if (e.Item.ItemType.Equals(GridItemType.Footer))
                {
                    Int32 rowCount = grdComplianceDocSearch.Items.Count;
                    if (rowCount > 0)
                    {
                        Int32 checkCount = 0;
                        foreach (GridDataItem item in grdComplianceDocSearch.Items)
                        {
                            CheckBox checkBox = ((CheckBox)item.FindControl("chkSelectDocument"));
                            if (checkBox.Checked)
                            {
                                checkCount++;
                            }
                        }
                        if (rowCount == checkCount)
                        {
                            GridHeaderItem item = grdComplianceDocSearch.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
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
        /// Grid Item Command event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdComplianceDocSearch_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == RadGrid.RebindGridCommandName)
                {
                    DocumentListToExport = new Dictionary<int, ComplianceDocumentSearchContract>();
                }
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
                    base.ConfigureExport(grdComplianceDocSearch);
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
        protected void grdComplianceDocSearch_SortCommand(object sender, GridSortCommandEventArgs e)
        {
            try
            {
                if (e.NewSortOrder != GridSortOrder.None)
                {
                    ViewState["SortExpression"] = e.SortExpression;
                    ViewState["SortDirection"] = e.NewSortOrder.Equals(GridSortOrder.Descending);
                    CurrentViewContext.GridCustomPaging.SortExpression = e.SortExpression;
                    CurrentViewContext.GridCustomPaging.SortDirectionDescending = e.NewSortOrder.Equals(GridSortOrder.Descending);
                }
                else
                {
                    ViewState["SortExpression"] = String.Empty;
                    ViewState["SortDirection"] = false;
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
        #endregion


        #region Methods

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
            BindComplianceItems();
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
        /// To bind program dropdown
        /// </summary>
        private void BindComplianceItems()
        {
            Presenter.GetComplianceItems();
            chkComplianceItem.DataSource = CurrentViewContext.lstComplianceItem;
            chkComplianceItem.DataBind();
        }

        /// <summary>
        /// Removes all the filters and Sorting on the grid and clears the variables.
        /// </summary>
        private void ResetGridFilters()
        {
            grdComplianceDocSearch.MasterTableView.SortExpressions.Clear();
            grdComplianceDocSearch.CurrentPageIndex = 0;
            grdComplianceDocSearch.MasterTableView.CurrentPageIndex = 0;
            grdComplianceDocSearch.Rebind();
        }

        private void ResetControls()
        {
            txtFirstName.Text = String.Empty;
            txtLastName.Text = String.Empty;
            dpFromDate.Clear();
            dpToDate.Clear();
            ResetInstitutionHierarchy();
            DocumentListToExport = new Dictionary<Int32, ComplianceDocumentSearchContract>();
            if (Presenter.IsDefaultTenant)
            {
                hdnTenantId.Value = String.Empty;
            }
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

        /// <summary>
        /// Method to export the document(s) as zip.
        /// </summary>
        private void ExportDocuments()
        {
            List<ComplianceDocumentSearchContract> documentList = new List<ComplianceDocumentSearchContract>();
            Int32 fileCount = AppConsts.NONE;
            documentList = CurrentViewContext.DocumentListToExport.Select(cond => cond.Value).DistinctBy(x => x.ApplicantDocumentID).ToList();
            if (documentList.IsNotNull() && documentList.Count > 0)
            {
                String tempFilePath = WebConfigurationManager.AppSettings["TemporaryFileLocation"];
                if (tempFilePath.IsNullOrEmpty())
                {
                    base.LogError("Please provide path for TemporaryFileLocation in config.", null);
                    return;
                }
                if (!tempFilePath.EndsWith(@"\"))
                {
                    tempFilePath += @"\";
                }
                tempFilePath += "Tenant_" + SelectedTenantId.ToString() + "_Zip_" + (DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("mmss")) + @"\";

                if (!Directory.Exists(tempFilePath))
                    Directory.CreateDirectory(tempFilePath);
                DirectoryInfo dirInfo = new DirectoryInfo(tempFilePath);
                try
                {
                    foreach (ComplianceDocumentSearchContract applicantDocumentToExport in documentList)
                    {
                        String fileExtension = Path.GetExtension(applicantDocumentToExport.DocumentPath);
                        //String fileName = Guid.NewGuid().ToString() + "_" + applicantDocumentToExport.FileName;
                        String fileName = GetFileName(applicantDocumentToExport.FileName);
                        String applicantName = String.Empty;
                        applicantName = applicantDocumentToExport.ApplicantName.Trim().Replace("  ", "_");

                        String applicantID = Convert.ToString(applicantDocumentToExport.ApplicantID);
                        //UAT-3962 : Add applicant name to downloaded file name on compliance doc search download.
                        //String finalFileName = fileName + (applicantName.IsNullOrEmpty() ? "_" : "_" + applicantName + "_") + applicantID + fileExtension;

                        String finalFileName = (applicantName.IsNullOrEmpty() ? "" : applicantName + "_") + fileName + "_" + applicantID + fileExtension;

                        String newTempFilePath = Path.Combine(tempFilePath, finalFileName);
                        byte[] fileBytes = null;
                        fileBytes = CommonFileManager.RetrieveDocument(applicantDocumentToExport.DocumentPath, FileType.ApplicantFileLocation.GetStringValue());

                        if (fileBytes.IsNotNull())
                        {
                            try
                            {
                                File.WriteAllBytes(newTempFilePath, fileBytes);
                            }
                            catch (Exception ex)
                            {
                                base.LogError("Error found in bytes write for DocumentID: " + applicantDocumentToExport.ApplicantDocumentID.ToString(), ex);
                            }
                        }
                        //tempbytes = fileBytes;
                    }
                    fileCount = Directory.GetFiles(tempFilePath).Count();
                    if (fileCount > AppConsts.NONE)
                    {
                        ifrExportDocument.Src = "~/ComplianceOperations/UserControl/DoccumentDownload.aspx?zipFilePath=" + tempFilePath + "&IsMultipleFileDownloadInZip=" + "True";
                    }
                    else
                    {
                        base.ShowInfoMessage("No document(s) found to export.");
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
        }

        private String GetFileName(String fileNameWithExt)
        {
            //String[] splitFileName = fileNameWithExt.Split('.');
            //String tempFileName = String.Join(".", splitFileName.Take(splitFileName.Length - 1));
            fileNameWithExt = fileNameWithExt.Replace(@"\", @"-");
            fileNameWithExt = fileNameWithExt.Replace(@" ", @"_");
            return fileNameWithExt;
        }
        #endregion



        #endregion
    }
}