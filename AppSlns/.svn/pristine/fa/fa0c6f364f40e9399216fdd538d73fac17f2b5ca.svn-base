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
using CoreWeb.BkgSetup.UserControl;
using CoreWeb.BkgOperations.Views;
using INTSOF.UI.Contract.BkgSetup;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using System.Web.UI.HtmlControls;

namespace CoreWeb.BkgSetup.Views
{
    public partial class ServiceFormInstitutionMapping : BaseUserControl, IServiceFormInstitutionMappingView
    {
        #region Variables

        #region Private Variables

        ServiceFormInstitutionMappingPresenter _presenter = new ServiceFormInstitutionMappingPresenter();
        private Int32 tenantId = 0;

        #endregion

        #region Public Variables

        #endregion

        #endregion

        #region Properties

        #region Private Properties

        //Int32? IServiceFormInstitutionMappingView.NodeId
        //{
        //    get
        //    {
        //        return ucCustomAttributeLoaderSearch.NodeId;
        //    }
        //    set
        //    {
        //        ucCustomAttributeLoaderSearch.NodeId = Convert.ToInt32(value);
        //    }
        //}

        Int32? IServiceFormInstitutionMappingView.ServiceFormID
        {
            get
            {
                if (String.IsNullOrEmpty(cmbServiceForm.SelectedValue))
                    return (int?)null;
                return Convert.ToInt32(cmbServiceForm.SelectedValue);
            }
            set
            {
                if (value > 0)
                {
                    cmbServiceForm.SelectedValue = value.ToString();
                }
                else
                {
                    cmbServiceForm.SelectedIndex = 0;
                }
            }
        }

        Int32? IServiceFormInstitutionMappingView.ServiceID
        {
            get
            {
                if (String.IsNullOrEmpty(cmbService.SelectedValue))
                    return (int?)null;
                return Convert.ToInt32(cmbService.SelectedValue);
            }
            set
            {
                if (value > 0)
                {
                    cmbService.SelectedValue = value.ToString();
                }
                else
                {
                    cmbService.SelectedIndex = 0;
                }
            }
        }

        Int32? IServiceFormInstitutionMappingView.MappingTypeID
        {
            get
            {
                if (String.IsNullOrEmpty(cmbMappingType.SelectedValue))
                    return (int?)null;
                return Convert.ToInt32(cmbMappingType.SelectedValue);
            }
            set
            {
                if (value > 0)
                {
                    cmbMappingType.SelectedValue = value.ToString();
                }
                else
                {
                    cmbMappingType.SelectedIndex = 0;
                }
            }
        }

        Int32? IServiceFormInstitutionMappingView.DPM_ID
        {
            get
            {
                if (!hdnParentDepartmntPrgrmMppng.Value.IsNullOrEmpty())
                {
                    return Convert.ToInt32(hdnParentDepartmntPrgrmMppng.Value);
                }
                return 0;
            }
            set
            {
                hdnParentTenantId.Value = CurrentViewContext.SelectedTenantId.ToString();
                hdnParentDepartmntPrgrmMppng.Value = value.ToString();
            }
        }

        List<ServiceFormInstitutionMappingContract> IServiceFormInstitutionMappingView.lstServiceFormInstitutionMapping
        {
            get;
            set;
        }

        List<ServiceForm> IServiceFormInstitutionMappingView.lstServiceForm
        {
            get
            {
                if (ViewState["ServiceFormList"] != null)
                    return (List<ServiceForm>)(ViewState["ServiceFormList"]);
                return null;
            }
            set
            {
                cmbServiceForm.DataSource = value;
                cmbServiceForm.DataBind();
                ViewState["ServiceFormList"] = value;
            }
        }

        List<SvcFormMappingType> IServiceFormInstitutionMappingView.lstMappingType
        {
            get
            {
                if (ViewState["MappingList"] != null)
                    return (List<SvcFormMappingType>)(ViewState["MappingList"]);
                return null;
            }
            set
            {
                cmbMappingType.DataSource = value;
                cmbMappingType.DataBind();
                ViewState["MappingList"] = value;
            }
        }

        List<BackgroundServiceMapping> IServiceFormInstitutionMappingView.lstBackgroundServiceMapping
        {
            get
            {
                if (ViewState["ServiceList"] != null)
                    return (List<BackgroundServiceMapping>)(ViewState["ServiceList"]);
                return null;
            }
            set
            {
                cmbService.DataSource = value;
                cmbService.DataBind();
                ViewState["ServiceList"] = value;
            }
        }

        List<LookupContract> IServiceFormInstitutionMappingView.lstElements
        {
            get
            {
                if (ViewState["TenantList"] != null)
                    return (List<LookupContract>)(ViewState["TenantList"]);
                return null;
            }
            set
            {
                ViewState["TenantList"] = value;
            }
        }

        Int32 IServiceFormInstitutionMappingView.SelectedTenantId
        {
            get
            {
                if (String.IsNullOrEmpty(cmbTenant.SelectedValue))
                    return 0;
                return Convert.ToInt32(cmbTenant.SelectedValue);
            }
            set
            {
                if (value > 0)
                {
                    cmbTenant.SelectedValue = value.ToString();
                }
                else
                {
                    cmbTenant.SelectedIndex = 0;
                }
            }
        }

        List<LookupContract> IServiceFormInstitutionMappingView.BindTenantsDropdown
        {
            set
            {
                cmbTenant.DataSource = value;
                cmbTenant.DataBind();
            }
        }

        Int32 IServiceFormInstitutionMappingView.TenantId
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

        IServiceFormInstitutionMappingView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        #endregion

        #region Public properties

        public ServiceFormInstitutionMappingPresenter Presenter
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
                base.Title = "Service Form Institution Mapping";
                base.SetPageTitle("Service Form Institution Mapping");
                base.OnInit(e);
                fsucOrderCmdBar.SubmitButton.CausesValidation = false;
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
            }
            //ucCustomAttributeLoaderSearch.TenantId = CurrentViewContext.SelectedTenantId;
            if (!String.IsNullOrEmpty(hdnParentHierarchyLabel.Value))
                lblParentinstituteHierarchy.Text = Convert.ToString(hdnParentHierarchyLabel.Value).HtmlEncode();
            else
                lblParentinstituteHierarchy.Text = String.Empty;
        }

        #region Grid Events

        protected void grdServiceFormInstitutionMapping_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            //CurrentViewContext.DPM_ID = ucCustomAttributeLoaderSearch.DPM_ID;
            Presenter.GetServiceFormMappingAllandSpecificInstitution();
            if (CurrentViewContext.lstServiceFormInstitutionMapping.IsNotNull() && CurrentViewContext.lstServiceFormInstitutionMapping.Count > 0)
            {
                grdServiceFormInstitutionMapping.DataSource = CurrentViewContext.lstServiceFormInstitutionMapping;
            }
            else
            {
                grdServiceFormInstitutionMapping.DataSource = new List<ServiceFormInstitutionMappingContract>();
            }

            if (CurrentViewContext.SelectedTenantId > 0)
            {
                grdServiceFormInstitutionMapping.Columns.FindByUniqueName("DPM_Label").Visible = true;
            }
            else
            {
                grdServiceFormInstitutionMapping.Columns.FindByUniqueName("DPM_Label").Visible = false;
            }
            if (CurrentViewContext.MappingTypeID != null)
            {
                grdServiceFormInstitutionMapping.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = true;
            }
            else
            {
                grdServiceFormInstitutionMapping.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;
            }
        }

        protected void grdServiceFormInstitutionMapping_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {
                GridEditFormItem editform = (GridEditFormItem)e.Item;
                ServiceFormMappingTemplate _serviceFormMappingTemplate = (ServiceFormMappingTemplate)editform.FindControl("ServiceFormMappingTemplate");
                if (_serviceFormMappingTemplate.IsNotNull())
                {
                    _serviceFormMappingTemplate.SelectedTenantID = CurrentViewContext.SelectedTenantId;
                    _serviceFormMappingTemplate.lstServiceForm = CurrentViewContext.lstServiceForm;
                    _serviceFormMappingTemplate.lstBackgroundServiceMapping = CurrentViewContext.lstBackgroundServiceMapping;
                    _serviceFormMappingTemplate.DepartmentProgramMappingID = CurrentViewContext.DPM_ID.HasValue ? CurrentViewContext.DPM_ID.Value : 0;
                    _serviceFormMappingTemplate.HierarchyVldLabel = String.Empty;
                    HtmlGenericControl divHierarchy = (HtmlGenericControl)_serviceFormMappingTemplate.FindControl("divHierarchy");
                    HtmlGenericControl divDispatchedMode = (HtmlGenericControl)_serviceFormMappingTemplate.FindControl("divDispatchedMode");
                    RadioButtonList rblDispatchMode = (RadioButtonList)_serviceFormMappingTemplate.FindControl("rblDispatchMode");
                    if (CurrentViewContext.SelectedTenantId > 0)
                    {
                        divHierarchy.Visible = true;
                    }
                    else
                    {
                        divHierarchy.Visible = false;
                    }

                    if (e.Item is GridEditFormInsertItem || e.Item is GridDataInsertItem)
                    {
                        _serviceFormMappingTemplate.InstitutionHierarchyLabel = String.Empty;
                        _serviceFormMappingTemplate.SelectedDispatchedType = Convert.ToBoolean(Convert.ToInt32(DispatchType.Manual));
                        divDispatchedMode.Visible = false;
                    }
                    else
                    {
                        var dataItem = ((ServiceFormInstitutionMappingContract)((e.Item).DataItem));
                        if (dataItem.IsNotNull())
                        {
                            _serviceFormMappingTemplate.SelectedServiceFormID = dataItem.SF_ID;
                            _serviceFormMappingTemplate.SelectedServiceID = dataItem.BSE_ID;
                            _serviceFormMappingTemplate.DepartmentProgramMappingID = dataItem.DPM_ID.HasValue ? dataItem.DPM_ID.Value : 0;
                            _serviceFormMappingTemplate.SelectedDispatchedType = dataItem.EnforceManual.HasValue ? !dataItem.EnforceManual.Value : dataItem.IsAutomatic;
                            if (dataItem.DPM_ID.HasValue)
                            {
                                _serviceFormMappingTemplate.InstitutionHierarchyLabel = Presenter.GetDeptProgMappingLabel(dataItem.DPM_ID.Value, CurrentViewContext.SelectedTenantId);
                            }
                            else
                            {
                                _serviceFormMappingTemplate.InstitutionHierarchyLabel = String.Empty;
                            }
                            _serviceFormMappingTemplate.ExistingServiceID = dataItem.BSE_ID;

                            rblDispatchMode.Enabled = dataItem.IsAutomatic;
                        }
                    }
                }
            }
        }

        protected void grdServiceFormInstitutionMapping_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "PerformInsert")
                {
                    if (Page.IsValid)
                    {
                        ServiceFormInstitutionMappingContract svcFormInstitutionMappingContract = new ServiceFormInstitutionMappingContract();
                        ServiceFormMappingTemplate _serviceFormMappingTemplate = (ServiceFormMappingTemplate)e.Item.FindControl("ServiceFormMappingTemplate");
                        if (_serviceFormMappingTemplate.IsNotNull())
                        {
                            _serviceFormMappingTemplate.HierarchyVldLabel = String.Empty;
                            svcFormInstitutionMappingContract.SF_ID = _serviceFormMappingTemplate.SelectedServiceFormID;
                            svcFormInstitutionMappingContract.BSE_ID = _serviceFormMappingTemplate.SelectedServiceID;
                            svcFormInstitutionMappingContract.MappingTypeID = Convert.ToInt16(CurrentViewContext.MappingTypeID);
                            svcFormInstitutionMappingContract.DPM_ID = _serviceFormMappingTemplate.DepartmentProgramMappingID;
                            if (_serviceFormMappingTemplate.IsManualByDefault)
                            {
                                svcFormInstitutionMappingContract.EnforceManual = false;
                            }
                            else
                            {
                                svcFormInstitutionMappingContract.EnforceManual = !_serviceFormMappingTemplate.SelectedDispatchedType;
                            }
                            if (CurrentViewContext.SelectedTenantId > 0 && svcFormInstitutionMappingContract.DPM_ID <= 0)
                            {
                                //lblMessage.Visible = true;
                                //lblMessage.ShowMessage("Please Select Institution Hierarchy", MessageType.Information);
                                _serviceFormMappingTemplate.HierarchyVldLabel = "Please Select Institution Hierarchy";
                                e.Canceled = true;
                            }
                            else
                            {
                                String errorMessage = Presenter.SaveServiceFormInstitutionMapping(svcFormInstitutionMappingContract, base.CurrentUserId);
                                if (errorMessage.Equals(String.Empty))
                                {
                                    lblMessage.Visible = true;
                                    lblMessage.ShowMessage("Service Form Institution Mapping has been added successfully", MessageType.SuccessMessage);
                                }
                                else
                                {
                                    lblMessage.Visible = true;
                                    lblMessage.ShowMessage(errorMessage, MessageType.Information);
                                    e.Canceled = true;
                                }
                            }
                        }
                    }
                }
                if (e.CommandName == "Update")
                {
                    if (Page.IsValid)
                    {
                        Int32 svcFormMappingId = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["SFM_ID"]);
                        Int32? svcFormHierarchyMappID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["SAFHM_ID"]);

                        ServiceFormInstitutionMappingContract svcFormInstitutionMappingContract = new ServiceFormInstitutionMappingContract();
                        ServiceFormMappingTemplate _serviceFormMappingTemplate = (ServiceFormMappingTemplate)e.Item.FindControl("ServiceFormMappingTemplate");
                        if (_serviceFormMappingTemplate.IsNotNull())
                        {
                            _serviceFormMappingTemplate.HierarchyVldLabel = String.Empty;
                            svcFormInstitutionMappingContract.SF_ID = _serviceFormMappingTemplate.SelectedServiceFormID;
                            svcFormInstitutionMappingContract.BSE_ID = _serviceFormMappingTemplate.SelectedServiceID;
                            svcFormInstitutionMappingContract.MappingTypeID = Convert.ToInt16(CurrentViewContext.MappingTypeID);
                            svcFormInstitutionMappingContract.DPM_ID = _serviceFormMappingTemplate.DepartmentProgramMappingID;
                            if (_serviceFormMappingTemplate.IsManualByDefault)
                            {
                                svcFormInstitutionMappingContract.EnforceManual = false;
                            }
                            else
                            {
                                svcFormInstitutionMappingContract.EnforceManual = !_serviceFormMappingTemplate.SelectedDispatchedType;
                            }
                            svcFormInstitutionMappingContract.SFM_ID = svcFormMappingId;
                            svcFormInstitutionMappingContract.SAFHM_ID = svcFormHierarchyMappID;

                            if (CurrentViewContext.SelectedTenantId > 0 && svcFormInstitutionMappingContract.DPM_ID <= 0)
                            {
                                //lblMessage.Visible = true;
                                //lblMessage.ShowMessage("Please Select Institution Hierarchy", MessageType.Information);
                                _serviceFormMappingTemplate.HierarchyVldLabel = "Please Select Institution Hierarchy";
                                e.Canceled = true;
                            }
                            else
                            {
                                String errorMessage = Presenter.UpdateServiceFormInstitutionMapping(svcFormInstitutionMappingContract, base.CurrentUserId);
                                if (errorMessage.Equals(String.Empty))
                                {
                                    lblMessage.Visible = true;
                                    lblMessage.ShowMessage("Service Form Institution Mapping has been updated successfully", MessageType.SuccessMessage);
                                }
                                else
                                {
                                    lblMessage.Visible = true;
                                    lblMessage.ShowMessage(errorMessage, MessageType.Information);
                                    e.Canceled = true;
                                }
                            }
                        }
                    }
                }
                if (e.CommandName == "Delete")
                {
                    Int32 serviceFormMappingID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["SFM_ID"]);
                    Int32? serviceFormHierarchyMappingID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["SAFHM_ID"]);
                    Boolean results = Presenter.DeleteServiceFormInstitutionMapping(serviceFormMappingID, serviceFormHierarchyMappingID, base.CurrentUserId);
                    if (results)
                    {
                        lblMessage.Visible = true;
                        lblMessage.ShowMessage("Service Form Institution Mapping has been deleted successfully", MessageType.SuccessMessage);
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

        #region Drop Down Events

        protected void cmbMappingType_DataBound(object sender, EventArgs e)
        {
            cmbMappingType.Items.Insert(0, new RadComboBoxItem("--Select--"));
        }

        protected void cmbTenant_DataBound(object sender, EventArgs e)
        {
            cmbTenant.Items.Insert(0, new RadComboBoxItem("--Select--"));
        }

        protected void cmbServiceForm_DataBound(object sender, EventArgs e)
        {
            cmbServiceForm.Items.Insert(0, new RadComboBoxItem("--Select--"));
        }

        protected void cmbService_DataBound(object sender, EventArgs e)
        {
            cmbService.Items.Insert(0, new RadComboBoxItem("--Select--"));
        }

        protected void cmbMappingType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            List<SvcFormMappingType> lstSvcFormMappType = (List<SvcFormMappingType>)(ViewState["MappingList"]);
            if (!(String.IsNullOrEmpty(cmbMappingType.SelectedValue))
                && lstSvcFormMappType.Where(x => x.MT_ID == Convert.ToInt32(cmbMappingType.SelectedValue)).FirstOrDefault().IsNotNull()
                && lstSvcFormMappType.Where(x => x.MT_ID == Convert.ToInt32(cmbMappingType.SelectedValue)).FirstOrDefault().MT_Code == "AAAB")
            {
                divTenant.Visible = true;
            }
            else
            {
                cmbTenant.SelectedValue = "0";
                CurrentViewContext.SelectedTenantId = 0;
                //ucCustomAttributeLoaderSearch.Reset();
                divTenant.Visible = false;
            }
        }

        protected void cmbTenant_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            CurrentViewContext.DPM_ID = 0;
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
            if (divTenant.Visible == true && CurrentViewContext.SelectedTenantId <= 0)
            {
                base.ShowErrorInfoMessage("Please select Institution.");
            }
            else
            {
                grdServiceFormInstitutionMapping.Rebind();
            }
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
            CurrentViewContext.SelectedTenantId = 0;
            CurrentViewContext.MappingTypeID = null;
            //CurrentViewContext.NodeId = 0;
            CurrentViewContext.DPM_ID = null;
            CurrentViewContext.ServiceFormID = null;
            CurrentViewContext.ServiceID = null;
            cmbMappingType.SelectedValue = "0";
            cmbTenant.SelectedValue = "0";
            cmbService.SelectedValue = "0";
            cmbServiceForm.SelectedValue = "0";
            divTenant.Visible = false;
            hdnParentDepartmntPrgrmMppng.Value = "0";
            hdnParentHierarchyLabel.Value = "";
            hdnParentTenantId.Value = "0";
            lblParentinstituteHierarchy.Text = String.Empty;
            //ucCustomAttributeLoaderSearch.Reset();
            grdServiceFormInstitutionMapping.MasterTableView.IsItemInserted = false;
            grdServiceFormInstitutionMapping.MasterTableView.Rebind();
        }

        #endregion

        #region Public Methods

        #endregion

        #endregion
    }
}