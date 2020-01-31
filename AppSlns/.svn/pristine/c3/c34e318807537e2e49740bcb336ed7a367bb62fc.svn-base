using CoreWeb.IntsofSecurityModel;
using CoreWeb.SearchUI.Views;
using CoreWeb.Shell;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.UI.Contract.SearchUI;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CoreWeb.Search.Views
{
    public partial class AdditionalDocumentSearch : BaseUserControl, IAdditionalDocumentSearchView
    {
        #region Variables

        private AdditionalDocumentSearchPresenter _presenter = new AdditionalDocumentSearchPresenter();
        private Int32 tenantId = 0;
        //private CustomPagingArgsContract _gridCustomPaging = null;
        //private SearchItemDataContract _gridSearchContract = null;

        #endregion

        #region Properties

        public AdditionalDocumentSearchPresenter Presenter
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

        Int32 IAdditionalDocumentSearchView.TenantID
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

        Int32 IAdditionalDocumentSearchView.SelectedTenantId
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

        List<Entity.Tenant> IAdditionalDocumentSearchView.lstTenant { get; set; }

        Int32 IAdditionalDocumentSearchView.CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        public IAdditionalDocumentSearchView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        String IAdditionalDocumentSearchView.ApplicantFirstName
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

        String IAdditionalDocumentSearchView.ApplicantLastName
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

        String IAdditionalDocumentSearchView.DocumentName
        {
            get
            {
                return txtDocumentName.Text;
            }
            set
            {
                txtDocumentName.Text = value;
            }
        }

        List<ComplianceDocumentSearchContract> IAdditionalDocumentSearchView.AdditionalDocumentList { get; set; }

        /// <summary>
        /// Gets or Sets the value for selected Items.
        /// </summary>
        Dictionary<Int32, ComplianceDocumentSearchContract> IAdditionalDocumentSearchView.DocumentListToExport
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

        String IAdditionalDocumentSearchView.ErrorMessage
        {
            get;
            set;
        }

        String IAdditionalDocumentSearchView.SuccessMessage
        {
            get;
            set;
        }

        String IAdditionalDocumentSearchView.InfoMessage
        {
            get;
            set;
        }

        #region Custom Paging Parameters

        /// <summary>
        /// Current Page Index</summary>
        /// <value>
        /// Gets or sets the value for CurrentPageIndex.</value>
        Int32 IAdditionalDocumentSearchView.CurrentPageIndex
        {
            get
            {
                return grdAdditionalDocSearch.MasterTableView.CurrentPageIndex + 1;
            }
            set
            {
                if (grdAdditionalDocSearch.MasterTableView.CurrentPageIndex > 0)
                {
                    grdAdditionalDocSearch.MasterTableView.CurrentPageIndex = value - 1;
                }
            }
        }

        /// <summary>
        /// Page Size</summary>
        /// <value>
        /// Gets the value for PageSize.</value>
        Int32 IAdditionalDocumentSearchView.PageSize
        {
            get
            {
                return grdAdditionalDocSearch.PageSize;
            }
            set
            {
                grdAdditionalDocSearch.PageSize = value;
            }
        }

        /// <summary>
        /// Virtual Page Count</summary>
        /// <value>
        /// Sets the value for VirtualPageCount.</value>
        Int32 IAdditionalDocumentSearchView.VirtualRecordCount
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
                grdAdditionalDocSearch.VirtualItemCount = value;
                grdAdditionalDocSearch.MasterTableView.VirtualItemCount = value;
            }
        }

        /// <summary>
        /// To get object of shared class of custom paging
        /// </summary>
        CustomPagingArgsContract IAdditionalDocumentSearchView.GridCustomPaging
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

        #region Page Event
        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                base.Title = "Additional Document Search";
                base.SetPageTitle("Additional Document Search");

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
                    grdAdditionalDocSearch.Visible = false;
                    fsucCmdExport.Visible = false;
                    Presenter.OnViewInitialized();
                    BindControl();
                }
                ifrExportDocument.Src = String.Empty;
                Presenter.OnViewLoaded();
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

        #region DropDown Event

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

        #region Button Event
        protected void CmdBarSearch_ResetClick(object sender, EventArgs e)
        {
            try
            {
                CurrentViewContext.VirtualRecordCount = 0;
                Presenter.GetTenants();
                BindControl();
                ResetControls();
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

        protected void CmdBarSearch_SearchClick(object sender, EventArgs e)
        {
            try
            {
                grdAdditionalDocSearch.Visible = true;
                fsucCmdExport.Visible = true;
                CurrentViewContext.DocumentListToExport = new Dictionary<Int32, ComplianceDocumentSearchContract>();
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

        protected void CmdBarSearch_CancelClick(object sender, EventArgs e)
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

        protected void fsucCmdExport_ExportClick(object sender, EventArgs e)
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

        #region Grid Event
        protected void grdAdditionalDocSearch_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                CurrentViewContext.GridCustomPaging.CurrentPageIndex = CurrentViewContext.CurrentPageIndex;
                CurrentViewContext.GridCustomPaging.PageSize = CurrentViewContext.PageSize;

                Presenter.PerformSearch();
                grdAdditionalDocSearch.DataSource = CurrentViewContext.AdditionalDocumentList;
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

        protected void grdAdditionalDocSearch_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
                {
                    ComplianceDocumentSearchContract compDoc = e.Item.DataItem as ComplianceDocumentSearchContract;
                    if (!compDoc.IsNullOrEmpty())
                    {
                        HtmlAnchor anchor = (HtmlAnchor)e.Item.FindControl("ancManageDocument");
                        anchor.HRef = String.Format("/ComplianceOperations/UserControl/DoccumentDownload.aspx?documentId={0}&tenantId={1}&IsPDFFileDownload={2}", compDoc.ApplicantDocumentID, CurrentViewContext.SelectedTenantId, true);
                    }

                    String applicantDocumentSearchDataID = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ApplicantDocumentID"].ToString();
                    Dictionary<Int32, ComplianceDocumentSearchContract> selectedDocumentExportList = CurrentViewContext.DocumentListToExport;

                    if (selectedDocumentExportList.IsNotNull() && Convert.ToInt32(applicantDocumentSearchDataID) != AppConsts.NONE)
                    {
                        if (selectedDocumentExportList.ContainsKey(Convert.ToInt32(applicantDocumentSearchDataID)))
                        {
                            CheckBox checkBox = ((CheckBox)e.Item.FindControl("chkSelectDocument"));
                            checkBox.Checked = true;
                        }
                    }
                }
                if (e.Item.ItemType.Equals(GridItemType.Footer))
                {
                    Int32 rowCount = grdAdditionalDocSearch.Items.Count;
                    if (rowCount > 0)
                    {
                        Int32 checkCount = 0;
                        foreach (GridDataItem item in grdAdditionalDocSearch.Items)
                        {
                            CheckBox checkBox = ((CheckBox)item.FindControl("chkSelectDocument"));
                            if (checkBox.Checked)
                            {
                                checkCount++;
                            }
                        }
                        if (rowCount == checkCount)
                        {
                            GridHeaderItem item = grdAdditionalDocSearch.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
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

        protected void grdAdditionalDocSearch_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == RadGrid.RebindGridCommandName)
                {
                    CurrentViewContext.DocumentListToExport = new Dictionary<int, ComplianceDocumentSearchContract>();
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
                    base.ConfigureExport(grdAdditionalDocSearch);
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

        protected void grdAdditionalDocSearch_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
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
                Int32 applicantID = (Int32)dataItem.GetDataKeyValue("ApplicantID");
                String documentPath = ((Label)dataItem.FindControl("lblDocumentPath")).Text.Trim();
                String fileName = ((Label)dataItem.FindControl("lblFileName")).Text.Trim();
                String FirstName = Convert.ToString(dataItem["FirstName"].Text.Trim());
                String LastName = Convert.ToString(dataItem["LastName"].Text.Trim());
                isChecked = ((CheckBox)dataItem.FindControl("chkSelectDocument")).Checked;

                if (documentExportList.IsNotNull() && !documentExportList.ContainsKey(applicantDocumentID) && isChecked)
                {

                    ComplianceDocumentSearchContract tempDocumentData = new ComplianceDocumentSearchContract();
                    tempDocumentData.ApplicantDocumentID = applicantDocumentID;
                    tempDocumentData.DocumentPath = documentPath;
                    tempDocumentData.FileName = fileName;
                    tempDocumentData.ApplicantID = applicantID;
                    tempDocumentData.FirstName = FirstName;
                    tempDocumentData.LastName = LastName;
                    documentExportList.Add(applicantDocumentID, tempDocumentData);
                }
                else if (documentExportList.IsNotNull() && documentExportList.ContainsKey(applicantDocumentID) && !isChecked)
                {
                    documentExportList.Remove(applicantDocumentID);
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

        #region Methods

        #region Private Methods
        /// <summary>
        /// to bind tenant dropdown
        /// </summary>
        private void BindControl()
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
                CurrentViewContext.SelectedTenantId = CurrentViewContext.TenantID;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ResetControls()
        {
            txtFirstName.Text = String.Empty;
            txtLastName.Text = String.Empty;
            txtDocumentName.Text = String.Empty;
            CurrentViewContext.DocumentListToExport = new Dictionary<Int32, ComplianceDocumentSearchContract>();
            if (!CurrentViewContext.GridCustomPaging.IsNullOrEmpty())
            {
                CurrentViewContext.GridCustomPaging.SortExpression = String.Empty;
                CurrentViewContext.GridCustomPaging.SortDirectionDescending = false;
            }
        }

        /// <summary>
        /// Removes all the filters and Sorting on the grid and clears the variables.
        /// </summary>
        private void ResetGridFilters()
        {
            if (CurrentViewContext.GridCustomPaging.SortExpression.IsNullOrEmpty())
            {
                grdAdditionalDocSearch.MasterTableView.SortExpressions.Clear();
            }
            grdAdditionalDocSearch.CurrentPageIndex = 0;
            grdAdditionalDocSearch.MasterTableView.CurrentPageIndex = 0;
            grdAdditionalDocSearch.Rebind();
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
                tempFilePath += "Tenant_" + CurrentViewContext.SelectedTenantId.ToString() + "_Zip_" + (DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("mmss")) + @"\";

                if (!Directory.Exists(tempFilePath))
                    Directory.CreateDirectory(tempFilePath);
                DirectoryInfo dirInfo = new DirectoryInfo(tempFilePath);
                try
                {
                    foreach (ComplianceDocumentSearchContract applicantDocumentToExport in documentList)
                    {
                        String fileExtension = Path.GetExtension(applicantDocumentToExport.DocumentPath);
                        String fileName = GetFileName(applicantDocumentToExport.FileName);
                        //String applicantName = String.Empty;
                        //applicantName = applicantDocumentToExport.FirstName.Trim().Replace(" ", "_") + "_" + applicantDocumentToExport.LastName.Trim().Replace(" ", "_");
                        //String applicantID = Convert.ToString(applicantDocumentToExport.ApplicantID);
                        String finalFileName = fileName + fileExtension;

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
            fileNameWithExt = fileNameWithExt.Replace(@"\", @"-");
            fileNameWithExt = fileNameWithExt.Replace(@" ", @"_");
            return fileNameWithExt;
        }
        #endregion

        #region Public Methods

        #endregion

        #endregion
    }
}