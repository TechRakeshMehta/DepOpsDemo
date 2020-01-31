using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using Telerik.Web.UI;

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class ManageBulkArchive : BaseUserControl, IManageBulkArchiveView
    {
        #region Variables

        private ManageBulkArchivePresenter _presenter = new ManageBulkArchivePresenter();
        private String _viewType;
        private Int32 tenantId = 0;
        private Boolean? _isAdminLoggedIn = null;
        #endregion

        #region Properties


        public ManageBulkArchivePresenter Presenter
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

        //UAt-4214
        public Boolean IsReset
        {
            get;
            set;
        }

        public Boolean IsAdminLoggedIn
        {
            get
            {
                if (_isAdminLoggedIn.IsNull())
                {
                    Presenter.IsAdminLoggedIn();
                }
                return _isAdminLoggedIn.HasValue ? _isAdminLoggedIn.Value : true;
            }
            set
            {
                _isAdminLoggedIn = value;
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

        public IManageBulkArchiveView CurrentViewContext
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

        /// <summary>
        /// Gets or Sets the value for selected subscription ids for archive.
        /// </summary>
        List<Int32> IManageBulkArchiveView.SelectedSubscriptionsToArchive
        {
            get
            {
                if (!ViewState["SelectedSubscriptionIds"].IsNull())
                {
                    return ViewState["SelectedSubscriptionIds"] as List<Int32>;
                }

                return new List<Int32>();
            }
            set
            {
                ViewState["SelectedSubscriptionIds"] = value;
            }
        }

        List<UploadedDocumentApplicantDataContract> IManageBulkArchiveView.MatchedApplicantSubscriptionList
        {
            get;
            set;
        }

        String IManageBulkArchiveView.ApplicantXmlData
        {
            get
            {
                if (!ViewState["XmlData"].IsNull())
                {
                    return ViewState["XmlData"] as String;
                }

                return null;
            }
            set
            {
                ViewState["XmlData"] = value;
            }
        }
        // String IManageBulkArchiveView.ApplicantXmlData
        //{
        //    get
        //    {
        //        String tempXmlData = (String)SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_APPLICANT_DETAIL_XML_KEY);
        //        if (!tempXmlData.IsNull())
        //        {
        //            return tempXmlData;
        //        }

        //        return null;
        //    }
        //}

        List<ApplicantDetailContract> IManageBulkArchiveView.UnMatchedApplicantDetails
        {
            get;
            set;
        }

        /// <summary>
        /// This property return true if default subscription id list loaded 
        /// </summary>
        private Boolean IsSubscriptionToArchiveLoaded
        {
            get
            {
                if (!ViewState["IsSubscriptionLoaded"].IsNull())
                {
                    return Convert.ToBoolean(ViewState["IsSubscriptionLoaded"]);
                }

                return false;
            }
            set
            {
                ViewState["IsSubscriptionLoaded"] = value;
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
                return grdMatchedApplicantSubscription.MasterTableView.CurrentPageIndex + 1;
            }
            set
            {
                if (grdMatchedApplicantSubscription.MasterTableView.CurrentPageIndex > 0)
                {
                    grdMatchedApplicantSubscription.MasterTableView.CurrentPageIndex = value - 1;
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
                return grdMatchedApplicantSubscription.PageSize;
            }
            set
            {
                grdMatchedApplicantSubscription.PageSize = value;
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
                grdMatchedApplicantSubscription.VirtualItemCount = value;
                grdMatchedApplicantSubscription.MasterTableView.VirtualItemCount = value;
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

        #endregion

        #region Page Events
        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                base.Title = "Manage Bulk Archive";
                base.SetPageTitle("Manage Bulk Archive");

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
                    ShowHideControls(false);
                    //SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.SESSION_APPLICANT_DETAIL_XML_KEY, null);
                    Presenter.OnViewInitialized();
                    BindControls();
                }

                HideShowControlsForGranularPermission();


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
                if (uploadControl.UploadedFiles.Count > AppConsts.NONE)
                {
                    CurrentViewContext.IsReset = false;
                    ShowHideControls(true);
                    //Reset Selected subscription list to archive on search click.
                    CurrentViewContext.SelectedSubscriptionsToArchive = new List<Int32>();
                    IsSubscriptionToArchiveLoaded = false;
                    //SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.SESSION_APPLICANT_DETAIL_XML_KEY, null);
                    UploadDocument();
                }
                else
                {
                    base.ShowInfoMessage("Please upload document to perform search.");
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
        /// To reset controls
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarReset_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentViewContext.IsReset = true;
                ResetControl();
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
                //SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.SESSION_APPLICANT_DETAIL_XML_KEY, null);
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

        /// <summary>
        /// To reset controls
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdArchiveSubscriptionSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (CurrentViewContext.SelectedSubscriptionsToArchive.IsNotNull() && CurrentViewContext.SelectedSubscriptionsToArchive.Count > AppConsts.NONE)
                {
                    if (Presenter.ArchiveSelectedSubscriptions())
                    {
                        base.ShowSuccessMessage("Subscriptions archived sucessfully.");
                        CurrentViewContext.SelectedSubscriptionsToArchive = new List<Int32>();
                        IsSubscriptionToArchiveLoaded = false;
                        Presenter.SetQueueImaging(); //UAT-2422-Resync data to flat tables
                        grdMatchedApplicantSubscription.Rebind();
                    }
                    else
                    {
                        base.ShowErrorMessage("Subscriptions are not archived sucessfully. Please try again.");
                    }
                }
                else
                {
                    base.ShowErrorInfoMessage("Please select subscriptions for archiving.");
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

        ///// <summary>
        ///// Redirect to Home page
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void CmdArchiveSubscriptionCancel_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        //SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.SESSION_APPLICANT_DETAIL_XML_KEY, null);
        //        Dictionary<String, String> queryString = new Dictionary<String, String>();
        //        Response.Redirect(String.Format(AppConsts.DASHBOARD_PAGE_NAME), true);
        //    }
        //    catch (SysXException ex)
        //    {
        //        base.LogError(ex);
        //        base.ShowErrorMessage(ex.Message);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        base.LogError(ex);
        //        base.ShowErrorMessage(ex.Message);
        //    }
        //}
        #endregion

        #region Grid Events
        #region Matched Applicant Subscription Grid
        /// <summary>
        /// To set data source to the grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdMatchedApplicantSubscriptionh_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
                {
                    if (CurrentViewContext.IsReset && !CurrentViewContext.IsAdminLoggedIn)
                    {
                        grdMatchedApplicantSubscription.CurrentPageIndex = 0;
                        grdMatchedApplicantSubscription.MasterTableView.CurrentPageIndex = 0;
                        CurrentViewContext.VirtualRecordCount = 0;
                        CurrentViewContext.MatchedApplicantSubscriptionList = new List<UploadedDocumentApplicantDataContract>();
                    }
                    else
                        Presenter.GetSubscriptionData();
                    AssignSubscriptionsToArchiveOnLoad();
                    grdMatchedApplicantSubscription.DataSource = CurrentViewContext.MatchedApplicantSubscriptionList;
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

        protected void grdMatchedApplicantSubscription_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
                {
                    String packageSubscriptionID = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["PackageSubscriptionID"].ToString();
                    List<Int32> selectedSubscriptionsToArchive = CurrentViewContext.SelectedSubscriptionsToArchive;
                    CheckBox checkBox = ((CheckBox)e.Item.FindControl("chkSelectSubscription"));
                    if (selectedSubscriptionsToArchive.IsNotNull() && selectedSubscriptionsToArchive.Count > AppConsts.NONE
                        && Convert.ToInt32(packageSubscriptionID) != AppConsts.NONE)
                    {
                        if (selectedSubscriptionsToArchive.Contains(Convert.ToInt32(packageSubscriptionID)))
                        {
                            checkBox.Checked = true;
                        }
                    }
                }
                if (e.Item.ItemType.Equals(GridItemType.Footer))
                {
                    Int32 rowCount = grdMatchedApplicantSubscription.Items.Count;
                    if (rowCount > 0)
                    {
                        Int32 checkCount = 0;
                        foreach (GridDataItem item in grdMatchedApplicantSubscription.Items)
                        {
                            CheckBox checkBox = ((CheckBox)item.FindControl("chkSelectSubscription"));
                            if (checkBox.Checked)
                            {
                                checkCount++;
                            }
                        }
                        if (rowCount == checkCount)
                        {
                            GridHeaderItem item = grdMatchedApplicantSubscription.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
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

        #endregion

        #region Un-Matched Applicants Grid
        /// <summary>
        /// To set data source to the grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdUnMatchedApplicants_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetUnMatchedApplicantsData();
                grdUnMatchedApplicants.DataSource = CurrentViewContext.UnMatchedApplicantDetails;
                if (CurrentViewContext.UnMatchedApplicantDetails.IsNotNull() && CurrentViewContext.UnMatchedApplicantDetails.Count > AppConsts.NONE)
                {
                    mainUnMatchedApplicant.Visible = true;
                }
                else
                {
                    mainUnMatchedApplicant.Visible = false;
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
                //Reset Controls and Grid filters.
                ResetControl(false);
                //ResetGridFilters(); //UAT-3874
                //if (ddlTenantName.SelectedValue.IsNullOrEmpty() || SelectedTenantId == AppConsts.NONE)
                //{
                //    ResetGridFilters();
                //}
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
        /// Handel selected Subscription 
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        protected void chkSelectSubscription_CheckedChanged(object sender, EventArgs e)
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
                List<Int32> selectedSubscriptionList = CurrentViewContext.SelectedSubscriptionsToArchive;
                Int32 packageSubscriptionID = (Int32)dataItem.GetDataKeyValue("PackageSubscriptionID");
                isChecked = ((CheckBox)dataItem.FindControl("chkSelectSubscription")).Checked;

                if (selectedSubscriptionList.IsNotNull() && !selectedSubscriptionList.Contains(packageSubscriptionID) && isChecked)
                {
                    selectedSubscriptionList.Add(packageSubscriptionID);
                }
                else if (selectedSubscriptionList.IsNotNull() && selectedSubscriptionList.Contains(packageSubscriptionID) && !isChecked)
                {
                    selectedSubscriptionList.Remove(packageSubscriptionID);
                }

                CurrentViewContext.SelectedSubscriptionsToArchive = selectedSubscriptionList;
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


        #region UAT-3861 Client assignments should be retained on package copy (hierarchy or compliance mapping)

        private void ShowHideControls(Boolean IsVisible)
        {
            grdMatchedApplicantSubscription.Visible=IsVisible;
            hdrPkgSub.Visible = IsVisible;
            cmdArchiveSubscription.Visible=IsVisible;
        }

        #endregion
        
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
        }

        /// <summary>
        /// Removes all the filters and Sorting on the grid and clears the variables.
        /// </summary>
        private void ResetGridFilters()
        {
            grdMatchedApplicantSubscription.MasterTableView.SortExpressions.Clear();
            grdMatchedApplicantSubscription.CurrentPageIndex = 0;
            grdMatchedApplicantSubscription.MasterTableView.CurrentPageIndex = 0;
            grdMatchedApplicantSubscription.Rebind();

            //Reset Un-Mached Applicants grid
            grdUnMatchedApplicants.MasterTableView.SortExpressions.Clear();
            grdUnMatchedApplicants.CurrentPageIndex = 0;
            grdUnMatchedApplicants.MasterTableView.CurrentPageIndex = 0;
            grdUnMatchedApplicants.Rebind();
        }

        private void ResetControl(Boolean resetTenant = true)
        {
            CurrentViewContext.SelectedSubscriptionsToArchive = new List<Int32>();
            IsSubscriptionToArchiveLoaded = false;
            if (Presenter.IsDefaultTenant && resetTenant)
            {
                ddlTenantName.SelectedValue = AppConsts.ZERO;
            }
            CurrentViewContext.ApplicantXmlData = null;
            //SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.SESSION_APPLICANT_DETAIL_XML_KEY, null);
        }

        /// <summary>
        /// To save the uploaded files.
        /// </summary>
        private void UploadDocument()
        {

            foreach (UploadedFile item in uploadControl.UploadedFiles)
            {

                String tempFilePath = String.Empty;
                String fileName = String.Empty;
                String fileExtension = String.Empty;
                fileExtension = Path.GetExtension(item.FileName);
                try
                {
                    if (fileExtension == ".xls" || fileExtension == ".xlsx")
                    {
                        tempFilePath = WebConfigurationManager.AppSettings["TemporaryFileLocation"];
                        if (tempFilePath.IsNullOrEmpty())
                        {
                            base.LogError("Please provide path for TemporaryFileLocation in config.", null);
                            return;
                        }

                        if (!tempFilePath.EndsWith(@"\"))
                        {
                            tempFilePath += @"\";
                        }

                        tempFilePath += "Tenant_" + SelectedTenantId.ToString() + (DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("mmss")) + @"\";

                        String tempFileName = item.FileName;
                        if (!Directory.Exists(tempFilePath))
                            Directory.CreateDirectory(tempFilePath);

                        fileName = Guid.NewGuid().ToString() + fileExtension;

                        string newPath = Path.Combine(tempFilePath, fileName);
                        item.SaveAs(tempFilePath + fileName);

                        //Read Excel Data.
                        //ExcelReader excelReader = new ExcelReader();
                        List<ApplicantDetailContract> applicantDetails = ExcelReader.GetApplicantListFromFile(newPath);
                        CurrentViewContext.ApplicantXmlData = ExcelReader.ConvertApplicantDetailInXMLFormat(applicantDetails);
                        //String xmlData = ExcelReader.ConvertApplicantDetailInXMLFormat(applicantDetails);
                        //SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.SESSION_APPLICANT_DETAIL_XML_KEY, xmlData);
                        grdMatchedApplicantSubscription.Rebind();
                        grdUnMatchedApplicants.Rebind();
                    }
                    else
                    {
                        base.ShowInfoMessage("Please upload xls/xlsx file only.");
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
                finally
                {
                    //Delete directory after read excel sheet.
                    if (Directory.Exists(tempFilePath))
                    {
                        DirectoryInfo dirInfo = new DirectoryInfo(tempFilePath);
                        dirInfo.Delete(true);
                    }
                }
            }
        }

        /// <summary>
        /// Method to assign subscriptions ids on loading.
        /// </summary>
        private void AssignSubscriptionsToArchiveOnLoad()
        {
            if (CurrentViewContext.MatchedApplicantSubscriptionList.Count > AppConsts.NONE && !IsSubscriptionToArchiveLoaded)
            {
                CurrentViewContext.SelectedSubscriptionsToArchive = CurrentViewContext.MatchedApplicantSubscriptionList.Select(slct => slct.PackageSubscriptionID).ToList();
                IsSubscriptionToArchiveLoaded = true;
            }
        }

        //UAT- UAT-3010:-  Granular Permission for Client Admin, hide show controls.
        private void HideShowControlsForGranularPermission()
        {
            if (CurrentViewContext.ArchivePermissionCode.ToUpper() == EnumSystemPermissionCode.NO_ACCESS_PERMISSION.GetStringValue().ToUpper())
            {
                cmdArchiveSubscription.SaveButton.Style.Add("display", "none");
            }
        }

        #endregion
        #endregion
    }

}