using System;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.UI.Contract.ComplianceManagement;
using Entity.ClientEntity;
using System.Collections.Generic;
using Telerik.Web.UI;
using INTSOF.Utils;
using INTERSOFT.WEB.UI.WebControls;
using System.Web.UI.WebControls;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using System.Configuration;
using INTSOF.IMAGE.MANAGER;
using CoreWeb.Shell.Views;
using System.Web.UI.HtmlControls;



namespace CoreWeb.ComplianceAdministration.Views
{
    public partial class SetupComplianceCategories : BaseUserControl, ISetupComplianceCategoriesView
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private SetupComplianceCategoriesPresenter _presenter = new SetupComplianceCategoriesPresenter();
        private String _viewType;
        private ComplianceCategoryContract _viewContract;
        private Int32 _tenantid;
        private Boolean? _isAdminLoggedIn = null;
        private Int32 _selectedTenantId = AppConsts.NONE;

        #endregion

        #endregion

        #region Properties

        #region Private Properties

        #endregion

        #region Public Properties


        public SetupComplianceCategoriesPresenter Presenter
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

        /// <summary>
        /// Gets and sets Logged In User TenantId
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
        /// Gets and Sets list of tenants.
        /// </summary>
        public List<Tenant> ListTenants
        {
            set;
            get;
        }

        /// <summary>
        /// Gets and Sets list of tenants.
        /// </summary>
        public List<NodesContract> ListofNodes
        {
            set;
            get;
        }

        /// <summary>
        /// Gets and sets TenantId of selected tenant
        /// </summary>
        public Int32 SelectedTenantId
        {
            get
            {
                if (_selectedTenantId == AppConsts.NONE)
                {
                    Int32.TryParse(ddlTenant.SelectedValue, out _selectedTenantId);

                    if (_selectedTenantId == AppConsts.NONE)
                        _selectedTenantId = TenantId;
                }
                return _selectedTenantId;
            }
            set
            {
                _selectedTenantId = value;
            }
        }

        /// <summary>
        /// Gets the default TenantId
        /// </summary>
        public Int32 DefaultTenantId
        {
            get
            {
                return Convert.ToInt32(ViewState["DefaultTenantId"]);
            }
            set
            {
                ViewState["DefaultTenantId"] = value;
            }
        }

        public Boolean IsAdminLoggedIn
        {
            get
            {
                if (_isAdminLoggedIn.IsNull())
                {
                    Presenter.IsAdminLoggedIn();
                }
                return _isAdminLoggedIn.Value;
            }
            set
            {
                _isAdminLoggedIn = value;
            }
        }

        /// <summary>
        /// Returns the current logged-in user ID.
        /// </summary>
        Int32 ISetupComplianceCategoriesView.CurrentLoggedInUserId
        {
            get
            {
                return base.CurrentUserId;
            }
        }

        //Int32 ISetupComplianceCategoriesView.TenantId
        //{
        //    get;
        //    set;

        //}

        public ISetupComplianceCategoriesView CurrentViewContext
        {
            get { return this; }
        }

        public String DeptProgramMappingID
        {
            get
            {
                if (!hdnDepartmntPrgrmMppng.Value.IsNullOrEmpty())
                {
                    return hdnDepartmntPrgrmMppng.Value;
                }
                return String.Empty;
            }
        }

        ComplianceCategoryContract ISetupComplianceCategoriesView.ViewContract
        {
            get
            {
                if (_viewContract.IsNull())
                {
                    _viewContract = new ComplianceCategoryContract();
                }

                return _viewContract;
            }

        }

        List<ComplianceCategory> ISetupComplianceCategoriesView.ComplianceCategories
        {
            get;
            set;

        }

        public string ErrorMessage
        {
            get;
            set;
        }

        /*UAT - 3032*/

        Int32 ISetupComplianceCategoriesView.PreferredSelectedTenantID
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

        public List<DocumentUrlContract> ComplianceCategoryDocUrls
        {
            get;
            set;
        }


        /* END UAT - 3032*/
        #endregion

        #endregion

        #region Events

        #region Page Events

        protected override void OnInit(EventArgs e)
        {
            try
            {
                grdCategory.WclGridDataObject = new GridObjectDataContainer();
                ((GridObjectDataContainer)(grdCategory.WclGridDataObject)).ColumnsToSkipEncoding.Add("Description");
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                Dictionary<String, String> args = new Dictionary<String, String>();

                if (!Request.QueryString["args"].IsNull())
                {
                    args.ToDecryptedQueryString(Request.QueryString["args"]);
                }
                base.Title = "Master Compliance Categories";
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
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
                BindTenant();
                /*UAT-3032*/
                if (IsAdminLoggedIn == true)
                    GetPreferredSelectedTenant();
                /*END UAT-3032*/
            }

            SetDefaultSelectedTenantId();

            //UAT-3872
            pnlInstitutionHierarchy.Visible = DefaultTenantId != SelectedTenantId;
            hdnTenantId.Value = SelectedTenantId.ToString();
            hfCurrentUserID.Value = CurrentUserId.ToString();
            lblinstituteHierarchy.Text = hdnHierarchyLabel.Value.HtmlEncode();

            base.SetPageTitle("Master Compliance Categories");
        }

        #endregion

        #region Grid Related Events

        protected void grdCategory_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetComplianceCategories();
                grdCategory.DataSource = CurrentViewContext.ComplianceCategories;
                grdCategory.Columns.FindByUniqueName("TenantName").Visible = DefaultTenantId.Equals(SelectedTenantId);
                grdCategory.Columns.FindByUniqueName("IsWithMultipleNodes").Visible = !DefaultTenantId.Equals(SelectedTenantId);
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

        protected void grdCategory_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {

                if (e.CommandName == "PerformInsert")
                {

                    CurrentViewContext.ViewContract.CategoryName = (e.Item.FindControl("txtCategoryName") as WclTextBox).Text.Trim();
                    CurrentViewContext.ViewContract.CategoryLabel = (e.Item.FindControl("txtCategoryLabel") as WclTextBox).Text.Trim();
                    CurrentViewContext.ViewContract.ScreenLabel = (e.Item.FindControl("txtScreenLabel") as WclTextBox).Text.Trim();
                    CurrentViewContext.ViewContract.Active = Convert.ToBoolean((e.Item.FindControl("chkActive") as IsActiveToggle).Checked);

                    CurrentViewContext.ViewContract.SendItemDoconApproval = Convert.ToBoolean((e.Item.FindControl("rdbSendItemDocApp") as IsActiveToggle).Checked);  //UAT-3805
                    //CurrentViewContext.ViewContract.SampleDocFormURL = Convert.ToString((e.Item.FindControl("txtSampleDocFormURL") as WclTextBox).Text);
                    var documenturls = (e.Item.FindControl("ucDocumentUrlInfo") as DocumentUrlInfo).DocumentUrlTempList;
                    CurrentViewContext.ViewContract.DocumentUrls = new List<DocumentUrlContract>();
                    if (documenturls != null && documenturls.Count > AppConsts.NONE)
                    {
                        foreach (var item in documenturls)
                        {
                            CurrentViewContext.ViewContract.DocumentUrls.Add(item);
                        }
                    }

                    //CurrentViewContext.ViewContract.Description = (e.Item.FindControl("txtCatDescription") as WclTextBox).Text.Trim();
                    //CurrentViewContext.ViewContract.ExplanatoryNotes = (e.Item.FindControl("txtCatNotes") as WclTextBox).Text.Trim();
                    CurrentViewContext.ViewContract.Description = (e.Item.FindControl("rdEditorDescription") as RadEditor).Content.Trim();
                    CurrentViewContext.ViewContract.ExplanatoryNotes = (e.Item.FindControl("rdEditorEcplanatoryNotes") as RadEditor).Content.Trim();
                    CurrentViewContext.ViewContract.TenantID = TenantId;
                    //UAT-3161
                    //CurrentViewContext.ViewContract.MoreInfoText = Convert.ToString((e.Item.FindControl("txtMoreInfo") as WclTextBox).Text.Trim());
                    if (Convert.ToInt32(ddlTenant.SelectedValue) > DefaultTenantId)
                    {
                        CurrentViewContext.ViewContract.TriggerOtherCategoryRules = Convert.ToBoolean((e.Item.FindControl("rblTriggerComplianceCheck") as RadioButtonList).SelectedValue);
                    }
                    Presenter.SaveComplianceCategory();
                    if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                    {
                        e.Canceled = true;
                        //base.ShowErrorMessage(String.Format("{0} already exist", CurrentViewContext.ViewContract.CategoryName));
                        (e.Item.FindControl("lblErrorMessage") as Label).ShowMessage(String.Format("{0} already exist.", CurrentViewContext.ViewContract.CategoryName), MessageType.Error);
                    }
                    else
                    {
                        e.Canceled = false;
                        base.ShowSuccessMessage("Compliance Category added successfully.");
                    }

                }
                if (e.CommandName == "Update")
                {
                    CurrentViewContext.ViewContract.ComplianceCategoryId = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ComplianceCategoryID"]);
                    CurrentViewContext.ViewContract.CategoryName = (e.Item.FindControl("txtCategoryName") as WclTextBox).Text.Trim();
                    CurrentViewContext.ViewContract.CategoryLabel = (e.Item.FindControl("txtCategoryLabel") as WclTextBox).Text.Trim();
                    CurrentViewContext.ViewContract.ScreenLabel = (e.Item.FindControl("txtScreenLabel") as WclTextBox).Text.Trim();
                    CurrentViewContext.ViewContract.Active = Convert.ToBoolean((e.Item.FindControl("chkActive") as IsActiveToggle).Checked);
                    CurrentViewContext.ViewContract.SendItemDoconApproval = Convert.ToBoolean((e.Item.FindControl("rdbSendItemDocApp") as IsActiveToggle).Checked);  //UAT-3805
                    //CurrentViewContext.ViewContract.SampleDocFormURL = Convert.ToString((e.Item.FindControl("txtSampleDocFormURL") as WclTextBox).Text);
                    var documenturls = (e.Item.FindControl("ucDocumentUrlInfo") as DocumentUrlInfo).DocumentUrlTempList;
                    CurrentViewContext.ViewContract.DocumentUrls = new List<DocumentUrlContract>();
                    if (documenturls != null && documenturls.Count > AppConsts.NONE)
                    {
                        foreach (var item in documenturls)
                        {
                            CurrentViewContext.ViewContract.DocumentUrls.Add(item);
                        }
                    }

                    //CurrentViewContext.ViewContract.Description = (e.Item.FindControl("txtCatDescription") as WclTextBox).Text.Trim();
                    //CurrentViewContext.ViewContract.ExplanatoryNotes = (e.Item.FindControl("txtCatNotes") as WclTextBox).Text.Trim();
                    CurrentViewContext.ViewContract.Description = (e.Item.FindControl("rdEditorDescription") as RadEditor).Content.Trim();
                    CurrentViewContext.ViewContract.ExplanatoryNotes = (e.Item.FindControl("rdEditorEcplanatoryNotes") as RadEditor).Content.Trim();
                    //CurrentViewContext.ViewContract.TenantID = TenantId;

                    //UAT-3161
                    //CurrentViewContext.ViewContract.MoreInfoText = Convert.ToString((e.Item.FindControl("txtMoreInfo") as WclTextBox).Text.Trim());
                    if (Convert.ToInt32(ddlTenant.SelectedValue) > DefaultTenantId)
                    {
                        CurrentViewContext.ViewContract.TriggerOtherCategoryRules = Convert.ToBoolean((e.Item.FindControl("rblTriggerComplianceCheck") as RadioButtonList).SelectedValue);
                    }
                    Presenter.UpdateComplianceCategory();
                    if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                    {
                        e.Canceled = true;
                        //base.ShowErrorMessage(String.Format("{0} already exist", CurrentViewContext.ViewContract.CategoryName));
                        (e.Item.FindControl("lblErrorMessage") as Label).ShowMessage(String.Format("{0} already exist.", CurrentViewContext.ViewContract.CategoryName), MessageType.Error);
                    }
                    else
                    {
                        e.Canceled = false;
                        base.ShowSuccessMessage("Compliance Category updated successfully.");
                    }
                }
                if (e.CommandName == "Delete")
                {
                    Boolean isDeleted = true;
                    CurrentViewContext.ViewContract.ComplianceCategoryId = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ComplianceCategoryID"]);
                    isDeleted = Presenter.DeleteComplianceCategory();
                    if (isDeleted)
                    {
                        base.ShowSuccessMessage("Compliance Category deleted successfully.");
                    }
                    else
                    {
                        base.ShowErrorInfoMessage(CurrentViewContext.ErrorMessage);
                    }
                }
                if (e.CommandName == "Edit")
                {
                    CurrentViewContext.ViewContract.ComplianceCategoryId = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ComplianceCategoryID"]);
                    Presenter.GetLargeContent();
                }
                if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToPdfCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToWordCommandName
                  || e.CommandName == Telerik.Web.UI.RadGrid.ExportToExcelCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToCsvCommandName)
                {
                    base.ConfigureExport(grdCategory);
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

        protected void grdCategory_ItemCreated(object sender, GridItemEventArgs e)
        {
            try
            {

                if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
                {
                    RadEditor rdEditorDescription = e.Item.FindControl("rdEditorDescription") as RadEditor;
                    RadEditor rdEditorEcplanatoryNotes = e.Item.FindControl("rdEditorEcplanatoryNotes") as RadEditor;
                    DocumentUrlInfo documentUrlInfo = e.Item.FindControl("ucDocumentUrlInfo") as DocumentUrlInfo;

                    SetImageManagerDirectory(rdEditorDescription, rdEditorEcplanatoryNotes);
                    var complianceCategory = (e.Item).DataItem as ComplianceCategory;

                    if (complianceCategory != null)
                    {
                        var docList = new List<DocumentUrlContract>();

                        foreach (var docUrl in complianceCategory.ComplianceCategoryDocUrls)
                        {
                            if (docUrl.IsDeleted == false)
                            {
                                docList.Add(new DocumentUrlContract()
                                 {
                                     SampleDocFormURL = docUrl.SampleDocFormURL,
                                     SampleDocFormURLLabel = docUrl.SampleDocFormURLLabel,
                                     ID = docUrl.ComplianceCategoryDocUrlID,
                                 });
                            }
                        }
                        documentUrlInfo.DocumentUrlTempList = docList;
                        documentUrlInfo.RebindRptrDocumentUrl();
                        //(e.Item.FindControl("txtCatNotes") as WclTextBox).Text = CurrentViewContext.ViewContract.ExplanatoryNotes;
                        (e.Item.FindControl("rdEditorDescription") as RadEditor).Content = complianceCategory.Description;
                        (e.Item.FindControl("rdEditorEcplanatoryNotes") as RadEditor).Content = CurrentViewContext.ViewContract.ExplanatoryNotes;
                        if (Convert.ToInt32(ddlTenant.SelectedValue) > DefaultTenantId)
                        {
                            e.Item.FindControl("dvMappingHierarchy").Visible = true;
                            e.Item.FindControl("dvTriggerComplianceCheck").Visible = true;
                            (e.Item.FindControl("rblTriggerComplianceCheck") as RadioButtonList).SelectedValue = Convert.ToString(complianceCategory.TriggerOtherCategoryRules);
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
        /// Called when data is bound in grid.
        /// </summary>
        /// <param name="sender">Current control i.e. Grid</param>
        /// <param name="e">Contains related data</param>
        protected void grdCategory_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridEditableItem && e.Item.IsInEditMode)
                {
                    //UAT-2725
                    if (Convert.ToInt32(ddlTenant.SelectedValue) > DefaultTenantId)
                    {
                        e.Item.FindControl("dvTriggerComplianceCheck").Visible = true;
                        CategoriesItemsNodes ucCategoriesItemsNodes = e.Item.FindControl("ucCategoriesItemsNodes") as CategoriesItemsNodes;
                        if (!ucCategoriesItemsNodes.IsNullOrEmpty())
                        {
                            ucCategoriesItemsNodes.SelectedTenantId = CurrentViewContext.SelectedTenantId;
                            ucCategoriesItemsNodes.ComplianceCategoryId = CurrentViewContext.ViewContract.ComplianceCategoryId;
                            ucCategoriesItemsNodes.ComplianceItemId = null;
                            ucCategoriesItemsNodes.BindListofNodes();
                        }
                    }
                }
                //Checks if item is GridDataItem type.
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    dataItem["IsActive"].Text = Convert.ToBoolean(dataItem["IsActive"].Text) == true ? Convert.ToString("Yes") : Convert.ToString("No");
                    dataItem["HasMoreInfoUrls"].Text = Convert.ToBoolean(dataItem["HasMoreInfoUrls"].Text) == true ? Convert.ToString("Yes") : Convert.ToString("No");

                    //Gets the value of field "IsCreatedByAdmin" which is kept in a hidden field.
                    HiddenField hdnfIsCreatedByAdmin = e.Item.FindControl("hdnfIsCreatedByAdmin") as HiddenField;

                    //Sets "DeleteColumn" visiblity false when Package is created by admin.
                    if (IsAdminLoggedIn == false && hdnfIsCreatedByAdmin.Value == "True")
                    {
                        dataItem["DeleteColumn"].Controls[0].Visible = false;
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

        #region DropDown Events

        /// <summary>
        /// Binds the categories as per selected client.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        protected void ddlTenant_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                ResetFilters();
                grdCategory.Visible = false;
                //grdCategory.Rebind();
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

        private void ResetFilters()
        {
            grdCategory.CurrentPageIndex = 0;
            grdCategory.MasterTableView.SortExpressions.Clear();
            grdCategory.MasterTableView.FilterExpression = null;
            grdCategory.MasterTableView.IsItemInserted = false;
            grdCategory.MasterTableView.ClearEditItems();

            foreach (GridColumn column in grdCategory.MasterTableView.OwnerGrid.Columns)
            {
                column.CurrentFilterFunction = GridKnownFunction.NoFilter;
                column.CurrentFilterValue = string.Empty;
            }

            hdnDepartmntPrgrmMppng.Value = String.Empty;
            hdnHierarchyLabel.Value = String.Empty;
            lblinstituteHierarchy.Text = String.Empty;
        }

        #endregion

        #region Command Bar Events

        /// <summary>
        /// To reset controls
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarReset_Click(object sender, EventArgs e)
        {
            //Presenter.GetTenants();
            grdCategory.Visible = false;
            BindTenant();
            SetDefaultSelectedTenantId();
            pnlInstitutionHierarchy.Visible = DefaultTenantId != SelectedTenantId;
            ResetFilters();
        }

        /// <summary>
        /// Redirect to Home page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarCancel_Click(object sender, EventArgs e)
        {            
            Response.Redirect(String.Format(AppConsts.DASHBOARD_PAGE_NAME), true);
        }

        /// <summary>
        /// To perform search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarSearch_Click(object sender, EventArgs e)
        {             
            grdCategory.Visible = true;
            //ResetFilters();
            grdCategory.Rebind();
        }

        //private void ResetGridFilters()
        //{
        //    grdCategory.MasterTableView.FilterExpression = null;
        //    grdCategory.MasterTableView.SortExpressions.Clear();
        //    grdCategory.CurrentPageIndex = 0;
        //    grdCategory.MasterTableView.CurrentPageIndex = 0;
        //    foreach (GridColumn column in grdApplicantSearchData.MasterTableView.RenderColumns)
        //    {
        //        if (column.ColumnType == "GridBoundColumn")
        //        {
        //            GridBoundColumn boundColumn = (GridBoundColumn)column;
        //            String columnName = boundColumn.UniqueName.ToString();
        //            grdApplicantSearchData.MasterTableView.GetColumnSafe(columnName).CurrentFilterFunction = GridKnownFunction.NoFilter;
        //            grdApplicantSearchData.MasterTableView.GetColumnSafe(columnName).CurrentFilterValue = String.Empty;
        //        }
        //    }
        //    CurrentViewContext.GridCustomPaging.FilterColumns = new List<String>();
        //    CurrentViewContext.GridCustomPaging.FilterOperators = new List<String>();
        //    CurrentViewContext.GridCustomPaging.FilterValues = new ArrayList();

        //    ViewState["FilterColumns"] = CurrentViewContext.GridCustomPaging.FilterColumns;
        //    ViewState["FilterOperators"] = CurrentViewContext.GridCustomPaging.FilterOperators;
        //    ViewState["FilterValues"] = CurrentViewContext.GridCustomPaging.FilterValues;
        //    Presenter.GetSSNSetting();
        //    if (CurrentViewContext.IsSSNDisabled)
        //    {
        //        txtSSN.Text = String.Empty;
        //        ApplicantSSN = null;
        //        divSSN.Visible = false;
        //        grdApplicantSearchData.MasterTableView.GetColumn("ApplicantSSN").Visible = false;
        //    }
        //    else
        //    {
        //        divSSN.Visible = true;
        //        grdApplicantSearchData.MasterTableView.GetColumn("ApplicantSSN").Visible = true;
        //        HideShowControlsForGranularPermission();
        //    }
        //    grdApplicantSearchData.Rebind();
        //}

        #endregion

        #endregion

        #region Methods

        private void BindTenant()
        {
            if (IsAdminLoggedIn == true)
            {
                Presenter.GetTenants();
                ddlTenant.DataSource = ListTenants;
                ddlTenant.DataBind();
            }
            else
            {
                pnlTenant.Visible = false;
            }
        }

        private void SetDefaultSelectedTenantId()
        {
            if (ddlTenant.SelectedValue.IsNullOrEmpty())
            {
                SelectedTenantId = TenantId;
                ddlTenant.SelectedValue = Convert.ToString(TenantId);
            }
        }

        /// <summary>
        /// Method that set the Image Managere Directory in Content Editor.
        /// </summary>
        private void SetImageManagerDirectory(RadEditor editorDescription, RadEditor editorNotes)
        {
            String s3ImageManagerDirectory = ConfigurationManager.AppSettings["S3ImageManagerDirectory"];
            if (ConfigurationManager.AppSettings["FileManagerMode"] == "S3")
            {
                String[] viewImages = new String[] { s3ImageManagerDirectory };
                String[] uploadImages = new String[] { s3ImageManagerDirectory };
                String[] deleteImages = new String[] { s3ImageManagerDirectory };
                SetImageManagerSettingInEditor(editorDescription, viewImages, uploadImages, deleteImages);
                SetImageManagerSettingInEditor(editorNotes, viewImages, uploadImages, deleteImages);
                editorDescription.ImageManager.ContentProviderTypeName = typeof(S3ContentProvider).AssemblyQualifiedName;
                editorNotes.ImageManager.ContentProviderTypeName = typeof(S3ContentProvider).AssemblyQualifiedName;

            }
            else if (ConfigurationManager.AppSettings["FileManagerMode"] == "DB")
            {
                String[] viewImages = new String[] { "InstitutionImages/" };
                String[] uploadImages = new String[] { "InstitutionImages/" };
                String[] deleteImages = new String[] { "InstitutionImages/" };
                SetImageManagerSettingInEditor(editorDescription, viewImages, uploadImages, deleteImages);
                SetImageManagerSettingInEditor(editorNotes, viewImages, uploadImages, deleteImages);
                editorDescription.ImageManager.ContentProviderTypeName = typeof(DBContentProvider).AssemblyQualifiedName;
                editorNotes.ImageManager.ContentProviderTypeName = typeof(DBContentProvider).AssemblyQualifiedName;
            }
            else
            {
                String[] viewImages = new String[] { "~/InstitutionImages" };
                String[] uploadImages = new String[] { "~/InstitutionImages" };
                String[] deleteImages = new String[] { "~/InstitutionImages" };
                SetImageManagerSettingInEditor(editorDescription, viewImages, uploadImages, deleteImages);
                SetImageManagerSettingInEditor(editorNotes, viewImages, uploadImages, deleteImages);
            }
        }
        private void SetImageManagerSettingInEditor(RadEditor editor, String[] viewImages, String[] uploadImages, String[] deleteImages)
        {
            editor.ImageManager.ViewPaths = viewImages;
            editor.ImageManager.UploadPaths = uploadImages;
            editor.ImageManager.DeletePaths = deleteImages;
            editor.ImageManager.MaxUploadFileSize = 71000000;

        }

        #region UAT-3032:- Sticky institution

        private void GetPreferredSelectedTenant()
        {
            if (CurrentViewContext.SelectedTenantId.IsNullOrEmpty() || CurrentViewContext.SelectedTenantId == AppConsts.ONE)
            {
                // Presenter.GetPreferredSelectedTenant();

                CurrentViewContext.PreferredSelectedTenantID = (Session["PreferredSelectedTenant"]).IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(Session["PreferredSelectedTenant"]);

                if (CurrentViewContext.PreferredSelectedTenantID > AppConsts.NONE)
                {
                    ddlTenant.SelectedValue = Convert.ToString(CurrentViewContext.PreferredSelectedTenantID);
                    CurrentViewContext.SelectedTenantId = Convert.ToInt32(ddlTenant.SelectedValue);
                }

            }
        }
        #endregion

        #endregion

        
    }
}

