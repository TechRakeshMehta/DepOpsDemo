using System;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.Utils;
using Entity.ClientEntity;
using System.Collections.Generic;
using INTSOF.UI.Contract.ComplianceManagement;
using Telerik.Web.UI;
using INTERSOFT.WEB.UI.WebControls;
using System.Web.UI.WebControls;
using CoreWeb.Shell;
using CoreWeb.IntsofSecurityModel;
using System.Configuration;
using INTSOF.IMAGE.MANAGER;
using CoreWeb.Shell.Views;


namespace CoreWeb.ComplianceAdministration.Views
{
    public partial class SetupComplianceItems : BaseUserControl, ISetupComplianceItemsView
    {
        #region Variables

        #region Private Variables

        private Int32 _tenantid;
        private ComplianceItemsContract _viewContract;
        private SetupComplianceItemsPresenter _presenter = new SetupComplianceItemsPresenter();
        private String _viewType;
        private Boolean? _isAdminLoggedIn = null;
        private Int32 _selectedTenantId = AppConsts.NONE;
        private Boolean isEditClicked = false;

        #endregion

        #region Public Variables


        #endregion

        #endregion

        #region Properties

        #region Private Properties


        #endregion

        #region Public Properties


        public SetupComplianceItemsPresenter Presenter
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
        /// Assign List of Items displayed in the grid
        /// </summary>
        public List<ComplianceItem> lstComplianceItems
        {
            get;
            set;
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
        /// Contract to manage the properties of the ComplianceItems Entity
        /// </summary>
        public ComplianceItemsContract ViewContract
        {
            get
            {
                if (_viewContract.IsNull())
                {
                    _viewContract = new ComplianceItemsContract();
                }
                return _viewContract;
            }
        }

        /// <summary>
        /// OrganizationUserID of the currently logged in user
        /// </summary>
        public Int32 CurrentLoggedInUserId
        {
            get
            {
                return base.CurrentUserId;
            }
        }

        public Int32 SelectedItemID
        {
            get
            {
                if (ViewState["SelectedItemID"] == null)
                {
                    ViewState["SelectedItemID"] = 0;
                }
                return (Int32)ViewState["SelectedItemID"];
            }
            set
            {
                ViewState["SelectedItemID"] = value;
            }
        }
        /// <summary>
        /// Represents the current view 
        /// </summary>
        public ISetupComplianceItemsView CurrentViewContext
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

        /// <summary>
        /// Status of update/deletion of the item
        /// </summary>
        public Boolean IsOperationSuccessful
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

        Int32 ISetupComplianceItemsView.PreferredSelectedTenantID
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
        /* END UAT - 3032*/
        #endregion

        #endregion

        #region Events

        #region Page Events

        protected override void OnInit(EventArgs e)
        {
            try
            {
                grdItem.WclGridDataObject = new GridObjectDataContainer();
                ((GridObjectDataContainer)(grdItem.WclGridDataObject)).ColumnsToSkipEncoding.Add("Description");
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                Dictionary<String, String> args = new Dictionary<String, String>();

                if (!Request.QueryString["args"].IsNull())
                {
                    args.ToDecryptedQueryString(Request.QueryString["args"]);
                }
                base.Title = "Master Compliance Items";
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
            // Page.SetFocus(grdItem);
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
                BindTenant();
                /*UAT-3032*/
                if(IsAdminLoggedIn == true)
                GetPreferredSelectedTenant();
                /*END UAT-3032*/
            }
            SetDefaultSelectedTenantId();

            //UAT-3872
            pnlInstitutionHierarchy.Visible = DefaultTenantId != SelectedTenantId;
            hdnTenantId.Value = SelectedTenantId.ToString();
            hfCurrentUserID.Value = CurrentUserId.ToString();
            lblinstituteHierarchy.Text = hdnHierarchyLabel.Value.HtmlEncode();

            base.SetPageTitle("Master Compliance Items");
        }

        #endregion

        #region Grid Events

        /// <summary>
        /// handle the commands fired from the grid
        /// </summary>
        /// <param name="sender">Current control i.e. Grid</param>
        /// <param name="e">Contains related data</param>
        protected void grdItem_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {

                if (e.CommandName == RadGrid.DeleteCommandName)
                {
                    CurrentViewContext.ViewContract.ComplianceItemId = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ComplianceItemID"]);

                    Presenter.DeleteComplianceItem();
                    if (CurrentViewContext.IsOperationSuccessful)
                        base.ShowSuccessMessage("Compliance item deleted successfully.");
                    else
                        base.ShowErrorInfoMessage(CurrentViewContext.ErrorMessage);
                }
                else if (e.CommandName == RadGrid.UpdateCommandName || e.CommandName == RadGrid.PerformInsertCommandName)
                {
                    if (e.CommandName == RadGrid.UpdateCommandName)
                        CurrentViewContext.ViewContract.ComplianceItemId = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ComplianceItemID"]);

                    CurrentViewContext.ViewContract.Name = (e.Item.FindControl("txtName") as WclTextBox).Text;
                    CurrentViewContext.ViewContract.ItemLabel = (e.Item.FindControl("txtLabel") as WclTextBox).Text;
                    CurrentViewContext.ViewContract.ScreenLabel = (e.Item.FindControl("txtScreenLabel") as WclTextBox).Text;
                    CurrentViewContext.ViewContract.IsActive = Convert.ToBoolean((e.Item.FindControl("chkActive") as IsActiveToggle).Checked);
                    CurrentViewContext.ViewContract.EffectiveDate = (e.Item.FindControl("dpkrEffectiveDate") as WclDatePicker).SelectedDate;
                    //CurrentViewContext.ViewContract.Description = (e.Item.FindControl("txtDescription") as WclTextBox).Text;
                    //CurrentViewContext.ViewContract.SampleDocFormURL = (e.Item.FindControl("txtSampleDocFormURL") as WclTextBox).Text;
                    //CurrentViewContext.ViewContract.ExplanatoryNotes = (e.Item.FindControl("txtNotes") as WclTextBox).Text;
                    CurrentViewContext.ViewContract.Description = (e.Item.FindControl("rdEditorDescription") as RadEditor).Content.Trim();
                    CurrentViewContext.ViewContract.Details = (e.Item.FindControl("rdEditorDetails") as RadEditor).Content.Trim();
                    CurrentViewContext.ViewContract.ExplanatoryNotes = (e.Item.FindControl("rdEditorEcplanatoryNotes") as RadEditor).Content.Trim();
                    if (CurrentViewContext.ViewContract.ComplianceItemId > AppConsts.NONE)
                        CurrentViewContext.ViewContract.ModifiedById = CurrentViewContext.CurrentLoggedInUserId;
                    else
                        CurrentViewContext.ViewContract.CreatedById = CurrentViewContext.CurrentLoggedInUserId;

                    var documenturls = (e.Item.FindControl("ucDocumentUrlInfo") as DocumentUrlInfo).DocumentUrlTempList;
                    CurrentViewContext.ViewContract.DocumentUrls = new List<DocumentUrlContract>();
                    if (documenturls != null && documenturls.Count > AppConsts.NONE)
                    {
                        foreach (var item in documenturls)
                        {
                            CurrentViewContext.ViewContract.DocumentUrls.Add(item);
                        }
                    }

                    //UAT-3077
                    CheckBox chkPaymentType = e.Item.FindControl("chkPaymentType") as CheckBox;
                    WclNumericTextBox txtAmount = e.Item.FindControl("txtAmount") as WclNumericTextBox;
                    CurrentViewContext.ViewContract.IsPaymentType = chkPaymentType.Checked;
                    if (chkPaymentType.Checked)
                    {
                        CurrentViewContext.ViewContract.Amount = Convert.ToDecimal(txtAmount.Text);
                    }

                    Presenter.SaveComplianceItem();

                    if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                    {
                        e.Canceled = true;
                        // base.ShowErrorMessage(CurrentViewContext.ErrorMessage);
                        // base.ShowErrorMessage(String.Format("{0} already exist", CurrentViewContext.ViewContract.PackageName));
                        (e.Item.FindControl("lblErrorMessage") as Label).ShowMessage(String.Format("{0} already exists.", CurrentViewContext.ViewContract.Name), MessageType.Error);
                    }

                    else if (CurrentViewContext.ViewContract.ComplianceItemId > AppConsts.NONE)
                    {
                        if (CurrentViewContext.IsOperationSuccessful)
                        {
                            e.Canceled = false;
                            base.ShowSuccessMessage("Compliance item updated successfully.");
                        }
                    }
                    else
                    {
                        e.Canceled = false;
                        base.ShowSuccessMessage("Compliance item saved successfully.");
                    }

                }
                else if (e.CommandName == "InitCopy")
                {
                    //(e.Item.FindControl("divCopyBlock") as System.Web.UI.HtmlControls.HtmlGenericControl).Visible = true;
                    //(e.Item.FindControl("divEditFormBlock") as System.Web.UI.HtmlControls.HtmlGenericControl).Visible = false;
                }
                //else if (e.CommandName == RadGrid.InitInsertCommandName)
                //{
                //    (e.Item.FindControl("divEditFormBlock") as System.Web.UI.HtmlControls.HtmlGenericControl).Visible = true;
                //    (e.Item.FindControl("divCopyBlock") as System.Web.UI.HtmlControls.HtmlGenericControl).Visible = false;
                //}

                if (e.CommandName == "Edit")
                {
                    CurrentViewContext.ViewContract.ComplianceItemId = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ComplianceItemID"]);
                    if (!CurrentViewContext.ViewContract.ComplianceItemId.IsNullOrEmpty() && CurrentViewContext.ViewContract.ComplianceItemId > AppConsts.NONE)
                    {
                        SelectedItemID = CurrentViewContext.ViewContract.ComplianceItemId;
                    }
                       
                    Presenter.GetLargeContent();
                }

                if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToPdfCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToWordCommandName
                 || e.CommandName == Telerik.Web.UI.RadGrid.ExportToExcelCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToCsvCommandName)
                {
                    base.ConfigureExport(grdItem);
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
        /// Binds the listing of compliance items which are not deleted
        /// </summary>
        /// <param name="sender">Current control i.e. Grid</param>
        /// <param name="e">Contains related data</param>
        protected void grdItem_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetComplianceIteme();
                grdItem.DataSource = CurrentViewContext.lstComplianceItems;
                grdItem.Columns.FindByUniqueName("TenantName").Visible = DefaultTenantId.Equals(SelectedTenantId);
                grdItem.Columns.FindByUniqueName("IsWithMultipleNodes").Visible = !DefaultTenantId.Equals(SelectedTenantId);
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
        /// Binds the listing of compliance items which are not deleted
        /// </summary>
        /// <param name="sender">Current control i.e. Grid</param>
        /// <param name="e">Contains related data</param>
        protected void grdItem_ItemCreated(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridEditFormItem && (e.Item.IsInEditMode))
                {
                    CategoriesItemsNodes ucCategoriesItemsNodes = e.Item.FindControl("ucCategoriesItemsNodes") as CategoriesItemsNodes;
                    DocumentUrlInfo documentUrlInfo = e.Item.FindControl("ucDocumentUrlInfo") as DocumentUrlInfo;
                    
                    if (Convert.ToInt32(ddlTenant.SelectedValue) > DefaultTenantId)
                    {
                        if (!ucCategoriesItemsNodes.IsNullOrEmpty())
                        {

                            ucCategoriesItemsNodes.SelectedTenantId = CurrentViewContext.SelectedTenantId;
                            ucCategoriesItemsNodes.ComplianceItemId = CurrentViewContext.ViewContract.ComplianceItemId;
                            if (!CurrentViewContext.ViewContract.ComplianceItemId.IsNullOrEmpty() && CurrentViewContext.ViewContract.ComplianceItemId == AppConsts.NONE)
                            {
                               ucCategoriesItemsNodes.ComplianceItemId = SelectedItemID;
                            }
                            //ucCategoriesItemsNodes.ComplianceItemId = null;
                            ucCategoriesItemsNodes.BindListofNodes();
                        }
                    }
                    RadEditor rdEditorDescription = e.Item.FindControl("rdEditorDescription") as RadEditor;
                    RadEditor rdEditorDetails = e.Item.FindControl("rdEditorDetails") as RadEditor;
                    RadEditor rdEditorEcplanatoryNotes = e.Item.FindControl("rdEditorEcplanatoryNotes") as RadEditor;
                    //UAT-3077
                    CheckBox chkPaymentType = e.Item.FindControl("chkPaymentType") as CheckBox;
                    System.Web.UI.HtmlControls.HtmlGenericControl divAmount = e.Item.FindControl("divAmount") as System.Web.UI.HtmlControls.HtmlGenericControl;
                    WclNumericTextBox txtAmount = e.Item.FindControl("txtAmount") as WclNumericTextBox;

                    SetImageManagerDirectory(rdEditorDescription, rdEditorEcplanatoryNotes, rdEditorDetails);
                    //WclTextBox txtName= e.Item.FindControl("txtName") as WclTextBox;
                    //if (txtName.IsNotNull())
                    //{
                    //    Page.SetFocus(txtName);
                    //}
                    e.Item.FindControl("pnl");
                    var complianceItem = (e.Item).DataItem as ComplianceItem;
                    if (complianceItem != null)
                    {
                        var docList = new List<DocumentUrlContract>();

                        foreach (var docUrl in complianceItem.ComplianceItemDocUrls)
                        {
                            if (docUrl.IsDeleted == false)
                            {
                                docList.Add(new DocumentUrlContract()
                                {
                                    SampleDocFormURL = docUrl.SampleDocFormURL,
                                    SampleDocFormUrlDisplayLabel =docUrl.SampleDocFormDisplayURLLabel,
                                    ID = docUrl.ComplianceItemDocUrlID
                                });
                            }
                        }
                        documentUrlInfo.DocumentUrlTempList = docList;
                        documentUrlInfo.RebindRptrDocumentUrl();
                    
                        //(e.Item.FindControl("txtNotes") as WclTextBox).Text = CurrentViewContext.ViewContract.ExplanatoryNotes;
                        (e.Item.FindControl("rdEditorDescription") as RadEditor).Content = complianceItem.Description;
                        (e.Item.FindControl("rdEditorDetails") as RadEditor).Content = complianceItem.Details;
                        (e.Item.FindControl("rdEditorEcplanatoryNotes") as RadEditor).Content = CurrentViewContext.ViewContract.ExplanatoryNotes;
                        if (Convert.ToInt32(ddlTenant.SelectedValue) > DefaultTenantId)
                        {
                            e.Item.FindControl("dvMappingHierarchy").Visible = true;
                        }
                        //UAT-3077
                        String isEnabledValidator = "false";
                        if (!complianceItem.IsPaymentType.IsNullOrEmpty())
                        {
                            chkPaymentType.Checked = complianceItem.IsPaymentType.Value;

                            divAmount.Style["display"] = chkPaymentType.Checked ? "block" : "none";
                            txtAmount.Text = complianceItem.Amount.HasValue ? Convert.ToString(complianceItem.Amount) : String.Empty;
                            isEnabledValidator = Convert.ToString(chkPaymentType.Checked);
                        }

                        System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "EnableDisableValidator('" + isEnabledValidator + "');", true);


                    }
                    if (ddlTenant.SelectedValue.IsNotNull())
                    {
                        if (ddlTenant.SelectedValue == DefaultTenantId.ToString())
                        {
                            (e.Item.FindControl("divEffectiveDate") as System.Web.UI.HtmlControls.HtmlGenericControl).Visible = false;
                            (e.Item.FindControl("divEffectiveDateOuter") as System.Web.UI.HtmlControls.HtmlGenericControl).Style.Add("display", "none");
                        }

                    }

                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "HideShowPanel(null,'true');", true);
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
        protected void grdItem_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                //Checks if item is GridDataItem type.
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    dataItem["IsActive"].Text = Convert.ToBoolean(dataItem["IsActive"].Text) == true ? Convert.ToString("Yes") : Convert.ToString("No");
                    dataItem["HasMoreInfoUrls"].Text = Convert.ToBoolean(dataItem["HasMoreInfoUrls"].Text) == true ? Convert.ToString("Yes") : Convert.ToString("No");

                    //Gets the value of field "IsCreatedByAdmin" which is kept in a hidden field.
                    HiddenField hdnfIsCreatedByAdmin = e.Item.FindControl("hdnfIsCreatedByAdmin") as HiddenField;

                    //Sets "DeleteColumn" visiblity false when Item is created by admin.
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
        /// Binds the items as per selected client.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        protected void ddlTenant_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                ResetFilters();
                grdItem.Visible = false;
                //grdItem.CurrentPageIndex = 0;
                //grdItem.MasterTableView.SortExpressions.Clear();
                //grdItem.MasterTableView.FilterExpression = null;

                //foreach (GridColumn column in grdItem.MasterTableView.OwnerGrid.Columns)
                //{
                //    column.CurrentFilterFunction = GridKnownFunction.NoFilter;
                //    column.CurrentFilterValue = string.Empty;
                //}
                //grdItem.Rebind();
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
            grdItem.CurrentPageIndex = 0;
            grdItem.MasterTableView.SortExpressions.Clear();
            grdItem.MasterTableView.FilterExpression = null;
            grdItem.MasterTableView.IsItemInserted = false;
            grdItem.MasterTableView.ClearEditItems();

            foreach (GridColumn column in grdItem.MasterTableView.OwnerGrid.Columns)
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
            grdItem.Visible = false;
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
            grdItem.Visible = true;
            //ResetFilters();
            grdItem.Rebind();
        }        

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
        private void SetImageManagerDirectory(RadEditor editorDescription, RadEditor editorNotes, RadEditor editorDetails)
        {
            String s3ImageManagerDirectory = ConfigurationManager.AppSettings["S3ImageManagerDirectory"];
            if (ConfigurationManager.AppSettings["FileManagerMode"] == "S3")
            {
                String[] viewImages = new String[] { s3ImageManagerDirectory };
                String[] uploadImages = new String[] { s3ImageManagerDirectory };
                String[] deleteImages = new String[] { s3ImageManagerDirectory };
                SetImageManagerSettingInEditor(editorDescription, viewImages, uploadImages, deleteImages);
                SetImageManagerSettingInEditor(editorDetails, viewImages, uploadImages, deleteImages);
                SetImageManagerSettingInEditor(editorNotes, viewImages, uploadImages, deleteImages);
                editorDescription.ImageManager.ContentProviderTypeName = typeof(S3ContentProvider).AssemblyQualifiedName;
                editorDetails.ImageManager.ContentProviderTypeName = typeof(S3ContentProvider).AssemblyQualifiedName;
                editorNotes.ImageManager.ContentProviderTypeName = typeof(S3ContentProvider).AssemblyQualifiedName;

            }
            else if (ConfigurationManager.AppSettings["FileManagerMode"] == "DB")
            {
                String[] viewImages = new String[] { "InstitutionImages/" };
                String[] uploadImages = new String[] { "InstitutionImages/" };
                String[] deleteImages = new String[] { "InstitutionImages/" };
                SetImageManagerSettingInEditor(editorDescription, viewImages, uploadImages, deleteImages);
                SetImageManagerSettingInEditor(editorDetails, viewImages, uploadImages, deleteImages);
                SetImageManagerSettingInEditor(editorNotes, viewImages, uploadImages, deleteImages);
                editorDescription.ImageManager.ContentProviderTypeName = typeof(DBContentProvider).AssemblyQualifiedName;
                editorDetails.ImageManager.ContentProviderTypeName = typeof(DBContentProvider).AssemblyQualifiedName;
                editorNotes.ImageManager.ContentProviderTypeName = typeof(DBContentProvider).AssemblyQualifiedName;
            }
            else
            {
                String[] viewImages = new String[] { "~/InstitutionImages" };
                String[] uploadImages = new String[] { "~/InstitutionImages" };
                String[] deleteImages = new String[] { "~/InstitutionImages" };
                SetImageManagerSettingInEditor(editorDescription, viewImages, uploadImages, deleteImages);
                SetImageManagerSettingInEditor(editorDetails, viewImages, uploadImages, deleteImages);
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
                //Presenter.GetPreferredSelectedTenant();
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

