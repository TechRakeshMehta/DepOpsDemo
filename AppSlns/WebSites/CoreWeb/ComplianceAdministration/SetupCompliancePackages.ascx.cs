using System;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.UI.Contract.ComplianceManagement;
using Entity.ClientEntity;
using System.Collections.Generic;
using System.Linq;
using INTSOF.Utils;
using Telerik.Web.UI;
using INTERSOFT.WEB.UI.WebControls;
using System.Web.UI.WebControls;
using CoreWeb.Shell;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell.Views;
using System.Web.UI.HtmlControls;


namespace CoreWeb.ComplianceAdministration.Views
{
    public partial class SetupCompliancePackages : BaseUserControl, ISetupCompliancePackagesView
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private SetupCompliancePackagesPresenter _presenter = new SetupCompliancePackagesPresenter();

        private String _viewType;

        private CompliancePackageContract _viewContract;

        private Int32 _tenantid;

        private Boolean? _isAdminLoggedIn = null;

        private Int32 _selectedTenantId = AppConsts.NONE;

        #endregion

        #endregion

        #region Properties

        #region Private Properties

        #endregion

        #region Public Properties


        public SetupCompliancePackagesPresenter Presenter
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
            set
            {
                _tenantid = value;
            }
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
                //if (_selectedTenantId == AppConsts.NONE)
                //{
                    Int32.TryParse(ddlTenant.SelectedValue, out _selectedTenantId);

                    if (_selectedTenantId == AppConsts.NONE)
                        _selectedTenantId = TenantId;
                //}
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
        /// CurrentUserID.
        /// </summary>
        /// <value>
        /// Gets or sets the value for current user's id.
        /// </value>
        Int32 ISetupCompliancePackagesView.CurrentLoggedInUserId
        {
            get
            {
                return base.CurrentUserId;
            }
        }

        public ISetupCompliancePackagesView CurrentViewContext
        {
            get { return this; }
        }

        public CompliancePackageContract ViewContract
        {
            get
            {
                if (_viewContract.IsNull())
                {
                    _viewContract = new CompliancePackageContract();
                }

                return _viewContract;
            }

        }

        /// <summary>
        /// CompliancePackages
        /// </summary>
        /// <value>Gets or sets the list of all Compliance Packages.</value>
        /// <remarks></remarks>
        List<CompliancePackage> ISetupCompliancePackagesView.CompliancePackages
        {
            get;
            set;

        }

        public string ErrorMessage
        {
            get;
            set;
        }


        Int32 ISetupCompliancePackagesView.NotesPositionId { get; set; }

        #endregion

        #endregion

        #region Events

        #region Page Events

        protected override void OnInit(EventArgs e)
        {
            try
            {
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                base.OnInit(e);
                base.Title = "Master Compliance Packages";
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
            }
            SetDefaultSelectedTenantId();
            base.SetPageTitle("Master Compliance Packages");
        }

        #endregion

        #region Grid Related Events

        protected void grdPackage_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetCompliancePackages();
                grdPackage.DataSource = CurrentViewContext.CompliancePackages;
                grdPackage.Columns.FindByUniqueName("TenantName").Visible = DefaultTenantId.Equals(SelectedTenantId);
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

        protected void grdPackage_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "PerformInsert")
                {
                    CurrentViewContext.ViewContract.PackageName = (e.Item.FindControl("txtPackageName") as WclTextBox).Text.Trim();
                    CurrentViewContext.ViewContract.Description = (e.Item.FindControl("txtPkgDescription") as WclTextBox).Text.Trim();
                    CurrentViewContext.ViewContract.PackageLabel = (e.Item.FindControl("txtPackageLabel") as WclTextBox).Text.Trim();
                    CurrentViewContext.ViewContract.ScreenLabel = (e.Item.FindControl("txtScreenLabel") as WclTextBox).Text.Trim();
                    CurrentViewContext.ViewContract.State = Convert.ToBoolean((e.Item.FindControl("chkActive") as IsActiveToggle).Checked);
                    CurrentViewContext.ViewContract.ExplanatoryNotes = (e.Item.FindControl("txtPkgNotes") as WclTextBox).Text.Trim();
                    CurrentViewContext.ViewContract.ExceptionDescription = (e.Item.FindControl("txtPkgExceptionDesc") as WclTextBox).Text.Trim();
                    CurrentViewContext.ViewContract.ViewDetails = Convert.ToBoolean((e.Item.FindControl("chkViewdetails") as WclButton).Checked);
                    CurrentViewContext.ViewContract.PackageDetail = (e.Item.FindControl("rdEditorPackageDetail") as RadEditor).Content.Trim(); //UAT 1006
                    CurrentViewContext.ViewContract.TenantID = TenantId;
                    CurrentViewContext.ViewContract.CompliancePackageTypeID = Convert.ToInt32((e.Item.FindControl("cmbCompliancePackageType") as WclComboBox).SelectedValue);
                    CurrentViewContext.ViewContract.ChecklistDocumentURL = Convert.ToString((e.Item.FindControl("txtChkDocumentURL") as WclTextBox).Text); //UAT 1337
                    Presenter.GetPackageNotesPosition((e.Item.FindControl("rbtnDisplayPosition") as RadioButtonList).SelectedValue);
                    CurrentViewContext.ViewContract.NotesDisplayPositionId = CurrentViewContext.NotesPositionId;
                    Presenter.SavePackagedetail();

                    if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                    {
                        e.Canceled = true;
                        // base.ShowErrorMessage(CurrentViewContext.ErrorMessage);
                        // base.ShowErrorMessage(String.Format("{0} already exist", CurrentViewContext.ViewContract.PackageName));
                        (e.Item.FindControl("lblErrorMessage") as Label).ShowMessage(String.Format("{0} already exist", CurrentViewContext.ViewContract.PackageName), MessageType.Error);
                    }
                    else
                    {
                        e.Canceled = false;

                        base.ShowSuccessMessage("Compliance Package saved successfully.");
                    }

                    grdPackage.MasterTableView.CurrentPageIndex = grdPackage.PageCount;
                }
                if (e.CommandName == "Update")
                {
                    CurrentViewContext.ViewContract.CompliancePackageId = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CompliancePackageID"]);
                    CurrentViewContext.ViewContract.PackageName = (e.Item.FindControl("txtPackageName") as WclTextBox).Text.Trim();
                    CurrentViewContext.ViewContract.Description = (e.Item.FindControl("txtPkgDescription") as WclTextBox).Text.Trim();
                    CurrentViewContext.ViewContract.PackageLabel = (e.Item.FindControl("txtPackageLabel") as WclTextBox).Text.Trim();
                    CurrentViewContext.ViewContract.ScreenLabel = (e.Item.FindControl("txtScreenLabel") as WclTextBox).Text.Trim();
                    CurrentViewContext.ViewContract.State = Convert.ToBoolean((e.Item.FindControl("chkActive") as IsActiveToggle).Checked);
                    CurrentViewContext.ViewContract.ExplanatoryNotes = (e.Item.FindControl("txtPkgNotes") as WclTextBox).Text.Trim();
                    CurrentViewContext.ViewContract.ExceptionDescription = (e.Item.FindControl("txtPkgExceptionDesc") as WclTextBox).Text.Trim();
                    CurrentViewContext.ViewContract.ViewDetails = Convert.ToBoolean((e.Item.FindControl("chkViewdetails") as WclButton).Checked);
                    CurrentViewContext.ViewContract.PackageDetail = (e.Item.FindControl("rdEditorPackageDetail") as RadEditor).Content.Trim(); //UAT 1006
                    CurrentViewContext.ViewContract.CompliancePackageTypeID = Convert.ToInt32((e.Item.FindControl("cmbCompliancePackageType") as WclComboBox).SelectedValue);
                    CurrentViewContext.ViewContract.ChecklistDocumentURL = Convert.ToString((e.Item.FindControl("txtChkDocumentURL") as WclTextBox).Text); //UAT 1337
                    Presenter.GetPackageNotesPosition((e.Item.FindControl("rbtnDisplayPosition") as RadioButtonList).SelectedValue);
                    CurrentViewContext.ViewContract.NotesDisplayPositionId = CurrentViewContext.NotesPositionId;
                    //CurrentViewContext.ViewContract.TenantID = TenantId;
                    Presenter.UpdatePackageDetail();
                    if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                    {
                        e.Canceled = true;
                        //(e.Item.FindControl("lblMessage") as Label).ShowMessage(CurrentViewContext.ErrorMessage, MessageType.Error);

                        //  base.ShowErrorMessage(String.Format("{0} already exist", CurrentViewContext.ViewContract.PackageName));
                        (e.Item.FindControl("lblErrorMessage") as Label).ShowMessage(String.Format("{0} already exist", CurrentViewContext.ViewContract.PackageName), MessageType.Error);

                    }
                    else
                    {
                        e.Canceled = false;
                        base.ShowSuccessMessage("Compliance Package updated sucessfully.");
                    }
                }
                if (e.CommandName == "Delete")
                {
                    Boolean isDeleted = true;

                    CurrentViewContext.ViewContract.CompliancePackageId = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CompliancePackageID"]);
                    isDeleted = Presenter.DeletePackage();

                    if (isDeleted)
                    {
                        base.ShowSuccessMessage("Compliance Package deleted successfully.");
                    }
                    else
                    {
                        base.ShowErrorInfoMessage(CurrentViewContext.ErrorMessage);
                    }
                }
                if (e.CommandName == "Edit")
                {
                    CurrentViewContext.ViewContract.CompliancePackageId = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CompliancePackageID"]);

                    Presenter.GetLargeContent();
                }
                //Hide filter when exportig to pdf or word
                if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToPdfCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToWordCommandName
                  || e.CommandName == Telerik.Web.UI.RadGrid.ExportToExcelCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToCsvCommandName)
                {
                    base.ConfigureExport(grdPackage);
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

        protected void grdPackage_ItemCreated(object sender, GridItemEventArgs e)
        {
            try
            {
                if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
                {
                    var compliancePackage = (e.Item).DataItem as CompliancePackage;
                    var _selectedValue = AppConsts.ZERO;

                    //UAT-2950
                    HtmlGenericControl dvMappingHierarchy = e.Item.FindControl("dvMappingHierarchy") as HtmlGenericControl;
                    if (CurrentViewContext.DefaultTenantId != SelectedTenantId && !dvMappingHierarchy.IsNullOrEmpty())
                    {
                        dvMappingHierarchy.Style["display"] = "block";
                    }
                    else
                    {
                        dvMappingHierarchy.Style["display"] = "none";
                    }

                    if (compliancePackage != null)
                    {
                        (e.Item.FindControl("rdEditorPackageDetail") as RadEditor).Content = compliancePackage.PackageDetail;
                        (e.Item.FindControl("txtPkgNotes") as WclTextBox).Text = CurrentViewContext.ViewContract.ExplanatoryNotes;
                        (e.Item.FindControl("txtPkgExceptionDesc") as WclTextBox).Text = CurrentViewContext.ViewContract.ExceptionDescription;
                        CurrentViewContext.ViewContract.CompliancePackageId = compliancePackage.CompliancePackageID;
                        _selectedValue = compliancePackage.CompliancePackageTypeID.ToString();

                        RadioButtonList rbtnNotesDisplayPosition = (RadioButtonList)e.Item.FindControl("rbtnDisplayPosition");
                        rbtnNotesDisplayPosition.SelectedValue = compliancePackage.lkpPackageNotesPosition.PNP_Code;

                        //UAT-2950
                        Label lblMappedHierarchy = e.Item.FindControl("lblMappedHierarchy") as Label;
                        if (CurrentViewContext.DefaultTenantId != SelectedTenantId && !dvMappingHierarchy.IsNullOrEmpty() && !lblMappedHierarchy.IsNullOrEmpty())
                        {
                            lblMappedHierarchy.Text = GetHierarchyText(compliancePackage).HtmlEncode();
                        }
                    }                    

                    WclComboBox ctrlType = (e.Item.FindControl("cmbCompliancePackageType") as WclComboBox);
                    if (ctrlType.IsNotNull())
                    {
                        var _lstPackageTypes = Presenter.GetCompliancePackageTypes();
                        _lstPackageTypes.Insert(AppConsts.NONE, new lkpCompliancePackageType
                        {
                            CPT_Name = AppConsts.COMBOBOX_ITEM_SELECT,
                            CPT_ID = AppConsts.NONE
                        });

                        ctrlType.DataSource = _lstPackageTypes;
                        ctrlType.DataBind();
                        ctrlType.SelectedValue = _selectedValue;
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
        protected void grdPackage_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                //Checks if item is GridDataItem type.
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
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
        /// Binds the packages as per selected client.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        protected void ddlTenant_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                grdPackage.CurrentPageIndex = 0;
                grdPackage.MasterTableView.SortExpressions.Clear();
                grdPackage.MasterTableView.FilterExpression = null;
                grdPackage.MasterTableView.IsItemInserted = false;
                grdPackage.MasterTableView.ClearEditItems();

                foreach (GridColumn column in grdPackage.MasterTableView.OwnerGrid.Columns)
                {
                    column.CurrentFilterFunction = GridKnownFunction.NoFilter;
                    column.CurrentFilterValue = string.Empty;
                }
                grdPackage.Rebind();
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
        
        //UAT-2950
        private String GetHierarchyText(CompliancePackage compliancePackage)
        {
            String HierarchyNodeText = String.Empty;
            if (!compliancePackage.DeptProgramPackages.IsNullOrEmpty())
            {
                foreach (var deptProgramPackage in compliancePackage.DeptProgramPackages)
                {
                    if (!deptProgramPackage.DPP_IsDeleted && !deptProgramPackage.DeptProgramMapping.DPM_IsDeleted)
                    {
                        HierarchyNodeText = HierarchyNodeText + ", " + deptProgramPackage.DeptProgramMapping.DPM_Label;
                    }
                }
                if (HierarchyNodeText.StartsWith(", "))
                    HierarchyNodeText = HierarchyNodeText.Substring(2, HierarchyNodeText.Length - 2);
            }
            return HierarchyNodeText;
        }
        #endregion
    }
}

