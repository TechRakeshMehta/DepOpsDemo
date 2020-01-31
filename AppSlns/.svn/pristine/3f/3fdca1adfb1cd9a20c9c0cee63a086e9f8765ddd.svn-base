using INTSOF.UI.Contract.ComplianceOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTSOF.Utils;
using INTSOF.UI.Contract.BkgOperations;
using Telerik.Web.UI;
using INTERSOFT.WEB.UI.WebControls;
using CoreWeb.BkgOperations.UserControl;

namespace CoreWeb.BkgOperations.Views
{
    public partial class DRDocsEntityMapping : BaseUserControl, IDRDocsEntityMappingView
    {
        #region Variables

        #region Private Variables

        DRDocsEntityMappingPresenter _presenter = new DRDocsEntityMappingPresenter();

        #endregion

        #region Public Variables

        #endregion

        #endregion

        #region Properties

        #region Private Properties

        List<DRDocsMappingContract> IDRDocsEntityMappingView.lstDRDocsMappingDetail { get; set; }

        List<LookupContract> IDRDocsEntityMappingView.lstElements { get; set; }

        List<LookupContract> IDRDocsEntityMappingView.BindCountryDropdown
        {
            set
            {
                cmbCountry.DataSource = value;
                cmbCountry.DataBind();
            }
        }
        List<LookupContract> IDRDocsEntityMappingView.BindStateDropdown
        {
            set
            {
                cmbState.DataSource = value;
                cmbState.DataBind();
            }
        }
        List<LookupContract> IDRDocsEntityMappingView.BindServiceDropdown
        {
            set
            {
                cmbService.DataSource = value;
                cmbService.DataBind();
            }
        }
        List<LookupContract> IDRDocsEntityMappingView.BindRegulatoryEntityTypeDropdown
        {
            set
            {
                cmbRegulatoryEntity.DataSource = value;
                cmbRegulatoryEntity.DataBind();
            }
        }
        List<LookupContract> IDRDocsEntityMappingView.BindDocumentsDropdown
        {
            set
            {
                cmbDRDocuments.DataSource = value;
                cmbDRDocuments.DataBind();
            }
        }
        List<LookupContract> IDRDocsEntityMappingView.BindTenantsDropdown
        {
            set
            {
                cmbTenant.DataSource = value;
                cmbTenant.DataBind();
            }
        }

        Int32 IDRDocsEntityMappingView.SelectedTenantID
        {
            get
            {
                if (!cmbTenant.SelectedValue.IsNullOrEmpty())
                    return Convert.ToInt32(cmbTenant.SelectedValue);
                return 0;
            }
            set
            {
                cmbTenant.SelectedValue = value.ToString();
            }

        }

        Int32 IDRDocsEntityMappingView.SelectedCountryID
        {
            get
            {
                if (!cmbCountry.SelectedValue.IsNullOrEmpty())
                {
                    if (String.Compare(cmbCountry.SelectedItem.Text, AppConsts.ALL_COUNTRIES_TEXT, true) == AppConsts.NONE)
                    {
                        return -1;
                    }
                    return Convert.ToInt32(cmbCountry.SelectedValue);
                }

                return 0;
            }
            set
            {
                cmbCountry.SelectedValue = value.ToString();
            }

        }
        Int32 IDRDocsEntityMappingView.SelectedStateID
        {
            get
            {
                if (!cmbState.SelectedValue.IsNullOrEmpty())
                    return Convert.ToInt32(cmbState.SelectedValue);
                return 0;
            }
            set
            {
                cmbState.SelectedValue = value.ToString();
            }

        }
        Int32 IDRDocsEntityMappingView.SelectedServiceID
        {
            get
            {
                if (!cmbService.SelectedValue.IsNullOrEmpty())
                    return Convert.ToInt32(cmbService.SelectedValue);
                return 0;
            }
            set
            {
                cmbService.SelectedValue = value.ToString();
            }

        }
        Int16 IDRDocsEntityMappingView.SelectedRegulatoryEntityTypeID
        {
            get
            {
                if (!cmbRegulatoryEntity.SelectedValue.IsNullOrEmpty())
                    return Convert.ToInt16(cmbRegulatoryEntity.SelectedValue);
                return 0;
            }
            set
            {
                cmbRegulatoryEntity.SelectedValue = value.ToString();
            }
        }
        Int32 IDRDocsEntityMappingView.SelectedDRDocumentID
        {
            get
            {
                if (!cmbDRDocuments.SelectedValue.IsNullOrEmpty())
                    return Convert.ToInt32(cmbDRDocuments.SelectedValue);
                return 0;
            }
            set
            {
                cmbDRDocuments.SelectedValue = value.ToString();
            }
        }
        Int32 IDRDocsEntityMappingView.loggedInUserId
        {
            get
            {
                Dictionary<String, String> args = new Dictionary<String, String>();
                if (!Request.QueryString["args"].IsNull())
                {
                    args.ToDecryptedQueryString(Request.QueryString["args"]);

                    if (args.ContainsKey("OrganizationUserId"))
                    {
                        return (Convert.ToInt32(args["OrganizationUserId"]));
                    }
                }
                return base.CurrentUserId;
            }
        }
        IDRDocsEntityMappingView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        //START UAT-3157
        Int32 IDRDocsEntityMappingView.PreferredSelectedTenantID
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
        //END UAT-3157
        #endregion

        #region Public properties

        public DRDocsEntityMappingPresenter Presenter
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

        #endregion

        #endregion

        #region Events

        #region Private Events

        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.Title = "Disclosure and Authorization Mapping";
                base.SetPageTitle("Disclosure and Authorization Mapping");
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
            lblMessage.Visible = false;
            if (!IsPostBack)
            {
                Presenter.OnViewInitialized();
                if (cmbCountry.Items.Count == 0)
                {
                    Presenter.GetAllCountriesList();
                    CurrentViewContext.BindCountryDropdown = CurrentViewContext.lstElements;
                    CurrentViewContext.BindStateDropdown = new List<LookupContract>();
                    Presenter.GetRegulatoryEntityTypeList();
                    CurrentViewContext.BindRegulatoryEntityTypeDropdown = CurrentViewContext.lstElements;
                    Presenter.GetAllDisclosureDocumentsList();
                    CurrentViewContext.BindDocumentsDropdown = CurrentViewContext.lstElements;
                    Presenter.GetBackgroundServiceList();
                    CurrentViewContext.BindServiceDropdown = CurrentViewContext.lstElements;
                }
                /*UAT-3157*/
                //if (IsAdminLoggedIn == true)
                GetPreferredSelectedTenant();
                /*END UAT-3157*/
            }
        }

        #region Grid Events
        protected void grdDRDocumentMapping_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            Presenter.GetDRDocumentEntityMappingList();
            grdDRDocumentMapping.DataSource = CurrentViewContext.lstDRDocsMappingDetail;
        }

        protected void grdResidentialHistory_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "PerformInsert")
                {
                    DRDocsMappingObjectIds drDocsMappingObjectIds = new DRDocsMappingObjectIds();
                    DocsEntityMappingTemplate _docsEntityMappingTemplate = (DocsEntityMappingTemplate)e.Item.FindControl("DocsEntityMappingTemplate");
                    if (_docsEntityMappingTemplate.IsNotNull())
                    {
                        drDocsMappingObjectIds.CountryId = _docsEntityMappingTemplate.SelectedCountryID;
                        drDocsMappingObjectIds.StateId = _docsEntityMappingTemplate.SelectedStateID;
                        drDocsMappingObjectIds.ServiceId = _docsEntityMappingTemplate.SelectedServiceID;
                        drDocsMappingObjectIds.TenantId = _docsEntityMappingTemplate.SelectedTenantID;
                        if (_docsEntityMappingTemplate.DeptProgramMappingID > 0)
                        {
                            drDocsMappingObjectIds.InstitutionHierarchyId = _docsEntityMappingTemplate.DeptProgramMappingID;
                            drDocsMappingObjectIds.TenantId = _docsEntityMappingTemplate.SelectedTenantID;
                        }
                        drDocsMappingObjectIds.RegulatoryEntityTypeId = _docsEntityMappingTemplate.SelectedRegulatoryEntityTypeID;
                        drDocsMappingObjectIds.DocumentId = _docsEntityMappingTemplate.SelectedDRDocumentID;

                        #region UAT-915
                        if (drDocsMappingObjectIds.TenantId.IsNotNull() && drDocsMappingObjectIds.TenantId > 0)
                        {
                            if (drDocsMappingObjectIds.InstitutionHierarchyId.IsNull() || drDocsMappingObjectIds.InstitutionHierarchyId == 0)
                            {
                                hdnValidateInstHierarchy.Value = "true";
                                e.Canceled = true;
                            }
                        }
                        #endregion

                        if (drDocsMappingObjectIds.DocumentId > 0 && hdnValidateInstHierarchy.Value != "true")
                        {
                            Boolean results = Presenter.SaveUpdateDRDocumentsEntityMapping(drDocsMappingObjectIds);
                            if (results)
                            {
                                lblMessage.Visible = true;
                                lblMessage.ShowMessage("Disclosure and Authorization document entity mapping has been added successfully", MessageType.SuccessMessage);
                            }
                        }
                    }
                }
                if (e.CommandName == "Update")
                {
                    Int32 disclosureDocumentMappingId = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["DisclosureDocumentMappingId"]);
                    DRDocsMappingObjectIds drDocsMappingObjectIds = new DRDocsMappingObjectIds();
                    drDocsMappingObjectIds.DisclosureDocumentMappingId = disclosureDocumentMappingId;
                    DocsEntityMappingTemplate _docsEntityMappingTemplate = (DocsEntityMappingTemplate)e.Item.FindControl("DocsEntityMappingTemplate");
                    if (_docsEntityMappingTemplate.IsNotNull())
                    {
                        drDocsMappingObjectIds.CountryId = _docsEntityMappingTemplate.SelectedCountryID;
                        drDocsMappingObjectIds.StateId = _docsEntityMappingTemplate.SelectedStateID;
                        drDocsMappingObjectIds.ServiceId = _docsEntityMappingTemplate.SelectedServiceID;
                        drDocsMappingObjectIds.TenantId = _docsEntityMappingTemplate.SelectedTenantID;
                        if (_docsEntityMappingTemplate.DeptProgramMappingID > 0)
                        {
                            drDocsMappingObjectIds.InstitutionHierarchyId = _docsEntityMappingTemplate.DeptProgramMappingID;
                            drDocsMappingObjectIds.TenantId = _docsEntityMappingTemplate.SelectedTenantID;
                        }
                        drDocsMappingObjectIds.RegulatoryEntityTypeId = _docsEntityMappingTemplate.SelectedRegulatoryEntityTypeID;
                        drDocsMappingObjectIds.DocumentId = _docsEntityMappingTemplate.SelectedDRDocumentID;

                        #region UAT-915
                        if (drDocsMappingObjectIds.TenantId.IsNotNull() && drDocsMappingObjectIds.TenantId > 0)
                        {
                            if (drDocsMappingObjectIds.InstitutionHierarchyId.IsNull() || drDocsMappingObjectIds.InstitutionHierarchyId == 0)
                            {
                                hdnValidateInstHierarchy.Value = "true";
                                e.Canceled = true;
                            }
                        }
                        #endregion
                        if (drDocsMappingObjectIds.DocumentId > 0 && hdnValidateInstHierarchy.Value != "true")
                        {
                            Boolean results = Presenter.SaveUpdateDRDocumentsEntityMapping(drDocsMappingObjectIds);
                            if (results)
                            {
                                lblMessage.Visible = true;
                                lblMessage.ShowMessage("Disclosure and Authorization document entity mapping has been updated successfully", MessageType.SuccessMessage);
                            }
                        }
                    }
                }
                if (e.CommandName == "Delete")
                {
                    Int32 disclosureDocumentMappingId = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["DisclosureDocumentMappingId"]);
                    Boolean results = Presenter.DeleteDRDocumentsEntityMapping(disclosureDocumentMappingId);
                    if (results)
                    {
                        lblMessage.Visible = true;
                        lblMessage.ShowMessage("Disclosure and Authorization document entity mapping has been deleted successfully", MessageType.SuccessMessage);
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

        protected void grdDRDocumentMapping_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {
                GridEditFormItem editform = (GridEditFormItem)e.Item;
                DocsEntityMappingTemplate _docsEntityMappingTemplate = (DocsEntityMappingTemplate)editform.FindControl("DocsEntityMappingTemplate");
                if (_docsEntityMappingTemplate.IsNotNull())
                {
                    Presenter.GetAllCountriesList();
                    _docsEntityMappingTemplate.BindCountryDropdown = CurrentViewContext.lstElements;
                    Presenter.GetBackgroundServiceList();
                    _docsEntityMappingTemplate.BindServiceDropdown = CurrentViewContext.lstElements;
                    Presenter.GetTenantList();
                    _docsEntityMappingTemplate.BindTenantsDropdown = CurrentViewContext.lstElements;
                    Presenter.GetRegulatoryEntityTypeList();
                    _docsEntityMappingTemplate.BindRegulatoryEntityTypeDropdown = CurrentViewContext.lstElements;
                    Presenter.GetAllDisclosureDocumentsList();
                    _docsEntityMappingTemplate.BindDocumentsDropdown = CurrentViewContext.lstElements;

                    if (e.Item is GridEditFormInsertItem || e.Item is GridDataInsertItem)
                    {
                        //Commented below code related to UAT-2833
                        //_docsEntityMappingTemplate.SelectedCountryID = AppConsts.COUNTRY_USA_ID;
                        _docsEntityMappingTemplate.SelectedCountryID = AppConsts.NONE;
                        //Presenter.GetAllStatesByCountryId(AppConsts.COUNTRY_USA_ID);
                        Presenter.GetAllStatesByCountryId(AppConsts.NONE);
                        _docsEntityMappingTemplate.BindStateDropdown = CurrentViewContext.lstElements;
                    }
                    else
                    {
                        var dataItem = ((DRDocsMappingContract)((e.Item).DataItem));
                        if (dataItem.IsNotNull())
                        {
                            _docsEntityMappingTemplate.SelectedCountryID = dataItem.CountryId;
                            Presenter.GetAllStatesByCountryId(dataItem.CountryId);
                            _docsEntityMappingTemplate.BindStateDropdown = CurrentViewContext.lstElements;
                            if (dataItem.StateId > 0)
                                _docsEntityMappingTemplate.SelectedStateID = dataItem.StateId;
                            if (dataItem.ServiceId > 0)
                                _docsEntityMappingTemplate.SelectedServiceID = dataItem.ServiceId;
                            if (dataItem.InstitutionHierarchyId > 0)
                            {
                                _docsEntityMappingTemplate.SelectedTenantID = dataItem.TenantId;
                                _docsEntityMappingTemplate.DeptProgramMappingID = dataItem.InstitutionHierarchyId;
                                _docsEntityMappingTemplate.InstitutionHierarchyLabel = Presenter.GetDeptProgMappingLabel(dataItem.InstitutionHierarchyId, dataItem.TenantId);
                            }
                            if (dataItem.DocumentId > 0)
                                _docsEntityMappingTemplate.SelectedDRDocumentID = dataItem.DocumentId;
                            if (dataItem.RegulatoryEntityTypeId > 0)
                                _docsEntityMappingTemplate.SelectedRegulatoryEntityTypeID = dataItem.RegulatoryEntityTypeId;
                        }
                    }
                }
            }
        }
        #endregion

        #region Drop Down Events
        protected void cmbTenant_DataBound(object sender, EventArgs e)
        {
            cmbTenant.Items.Insert(0, new RadComboBoxItem("--Select--"));
        }

        protected void cmbCountry_DataBound(object sender, EventArgs e)
        {
            cmbCountry.Items.Insert(0, new RadComboBoxItem("--Select--"));
        }

        protected void cmbState_DataBound(object sender, EventArgs e)
        {
            cmbState.Items.Insert(0, new RadComboBoxItem("--Select--"));
        }

        protected void cmbService_DataBound(object sender, EventArgs e)
        {
            cmbService.Items.Insert(0, new RadComboBoxItem("--Select--"));
        }

        protected void cmbRegulatoryEntity_DataBound(object sender, EventArgs e)
        {
            cmbRegulatoryEntity.Items.Insert(0, new RadComboBoxItem("--Select--"));
        }

        protected void cmbDRDocuments_DataBound(object sender, EventArgs e)
        {
            cmbDRDocuments.Items.Insert(0, new RadComboBoxItem("--Select--"));
        }
        #endregion

        #region Button Events

        /// <summary>
        /// Displays the records in the order queue based on the search criteria.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarSearch_Click(object sender, EventArgs e)
        {
            grdDRDocumentMapping.Rebind();
        }

        /// <summary>
        /// Resets all the search controls and displays the records in the order queue with deafult checkbox selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarReset_Click(object sender, EventArgs e)
        {
            ResetPageControls();
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
                Response.Redirect(String.Format(AppConsts.DASHBOARD_PAGE_NAME), false);
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

        #region Public Events

        #endregion

        #endregion

        #region Methods

        #region Private Methods

        private void ResetPageControls()
        {
            cmbCountry.SelectedValue = null;
            cmbCountry.Text = null;
            CurrentViewContext.BindStateDropdown = new List<LookupContract>();
            cmbState.SelectedIndex = 0;
            cmbTenant.SelectedValue = null;
            cmbTenant.Text = null;
            cmbService.SelectedValue = null;
            cmbService.Text = null;
            cmbDRDocuments.SelectedValue = null;
            cmbDRDocuments.Text = null;
            cmbRegulatoryEntity.SelectedValue = null;
            cmbRegulatoryEntity.Text = null;
            /* START UAT-3157*/
            GetPreferredSelectedTenant();
            /*END UAT-3157*/
            grdDRDocumentMapping.Rebind();
        }

        #region UAT-3157:- Sticky institution

        private void GetPreferredSelectedTenant()
        {
            if (CurrentViewContext.SelectedTenantID.IsNullOrEmpty() || CurrentViewContext.SelectedTenantID == AppConsts.NONE)
            {
                // Presenter.GetPreferredSelectedTenant();
                CurrentViewContext.PreferredSelectedTenantID = (Session["PreferredSelectedTenant"]).IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(Session["PreferredSelectedTenant"]);
                if (CurrentViewContext.PreferredSelectedTenantID > AppConsts.NONE)
                {
                    cmbTenant.SelectedValue = Convert.ToString(CurrentViewContext.PreferredSelectedTenantID);
                    CurrentViewContext.SelectedTenantID = Convert.ToInt32(cmbTenant.SelectedValue);
                }
            }
        }
        #endregion
        #endregion

        #region Public Methods

        #endregion

        #endregion
    }
}