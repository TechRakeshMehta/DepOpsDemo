using CoreWeb.ContractManagement.Views;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ContractManagement;
using INTSOF.UI.Contract.PackageBundleManagement;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using INTERSOFT.WEB.UI.WebControls;
using System.Text;
using System.Web.Configuration;
using System.IO;
using System.Web.UI.HtmlControls;
using CoreWeb.Shell.Views;

namespace CoreWeb.ContractManagement.Views
{
    public partial class ManageContracts : BaseUserControl, IManageContract
    {
        #region Private Variables

        private ManageContractPresenter _presenter = new ManageContractPresenter();
        private Int32 tenantId = 0;
        private ContractManagementContract _searchContract = null;
        List<ContractManagementContract> IManageContract.ContractDataList
        {
            get;
            set;
        }
        ContractManagementContract IManageContract.SearchContract
        {
            get
            {
                if (_searchContract.IsNull())
                {
                    GetSearchParameters();
                }
                return _searchContract;
            }
            set
            {
                _searchContract = value;
                //SetSearchParameters();
            }
        }

        private String _viewStateContractSites = "contractSites";
        private String _viewStateContractContacts = "contractcontacts";
        private String _viewStateContractDocuments = "contractDocs";
        private string _viewIsContractInEditMode = "IsContractInEditMode";
        private String _viewSiteDocuments = "siteDocs";
        private string _viewIsSiteInEditMode = "IsContractInEditMode";
        private String _viewStateSiteContacts = "sitecontacts";

        #endregion

        #region Public Properties

        public ManageContractPresenter Presenter
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
        public IManageContract CurrentViewContext
        {
            get
            {
                return this;
            }
        }
        public Int32 CurrentUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }
        Boolean IManageContract.IsReset
        {
            get;
            set;
        }
        string IManageContract.SuccessMessage
        {
            get;
            set;
        }
        string IManageContract.ErrorMessage
        {
            get;
            set;
        }
        bool IManageContract.IsAdminLoggedIn
        {
            get;
            set;
        }


        /// <summary>
        /// Represents the List of Sites under the selected Contract
        /// </summary>
        List<SiteContract> IManageContract.lstSiteContracts
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the contractid to which the current Sites belong to
        /// </summary>
        Int32 IManageContract.ContractId
        {
            get
            {
                if (ViewState["ContractId"].IsNotNull())
                {
                    return Convert.ToInt32(ViewState["ContractId"]);
                }
                return 0;
            }
            set
            {
                ViewState["ContractId"] = value;
            }
        }


        /// <summary>
        /// OrganizationUserID of the loggedIn user.
        /// </summary>
        Int32 IManageContract.CurrentUserId
        {
            get
            {
                return base.CurrentUserId;
            }
        }

        /// <summary>
        /// Contract Data to Save
        /// </summary>
        ContractManagementContract IManageContract.ContractData
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the List of Contracts under the selected Contract
        /// </summary>
        List<ContactContract> IManageContract.lstContactContract
        {
            get;
            set;
        }


        /// <summary>
        /// ID of the Root node 
        /// </summary>
        String IManageContract.RootDPMId
        {
            get;
            set;
        }
        /// <summary>
        /// Represents the List of Documents under the selected Contract
        /// </summary>
        List<ContractDocumentContract> IManageContract.ContractDocumentContractList
        {
            get;
            set;
        }

        String SavedDocPath { get; set; }

        String TempDocPath { get; set; }

        /// <summary>
        /// Represents the List of Documents under the selected Contract
        /// </summary>
        List<SiteDocumentContract> IManageContract.SiteDocumentContractList
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the sITEiD
        /// </summary>
        Int32 IManageContract.SiteId
        {
            get;
            set;
        }

        //UAT-1475	Make it easier to view a contract entry.

        #region Custom paging parameters
        public Int32 CurrentPageIndex
        {
            get
            {
                return grdContract.MasterTableView.CurrentPageIndex + 1;
            }
            set
            {
                grdContract.MasterTableView.CurrentPageIndex = value - 1;
            }
        }

        public Int32 PageSize
        {
            get
            {
                return grdContract.PageSize;
            }
            set
            {
                grdContract.PageSize = value;
            }
        }

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
                grdContract.VirtualItemCount = value;
                grdContract.MasterTableView.VirtualItemCount = value;
            }
        }

        CustomPagingArgsContract IManageContract.GridCustomPaging
        {
            get
            {
                if (ViewState["GridCustomPaging"] == null)
                {
                    ViewState["GridCustomPaging"] = new CustomPagingArgsContract();
                }
                return (CustomPagingArgsContract)ViewState["GridCustomPaging"];
            }
            set
            {
                ViewState["GridCustomPaging"] = value;
                VirtualRecordCount = value.VirtualPageCount;
                PageSize = value.PageSize;
                CurrentPageIndex = value.CurrentPageIndex;
            }
        }
        #endregion

        List<ContactContract> IManageContract.lstSiteContacts
        {
            get;
            set;
        }

        List<Int32> IManageContract.lstContractIDs
        {
            get;
            set;
        }

        #endregion

        #region Private Properties
        Int32 IManageContract.SelectedTenantID
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
                    ddlTenantName.SelectedValue = Convert.ToString(value);
                    hdnTenantId.Value = Convert.ToString(value);
                }
                else
                {
                    ddlTenantName.SelectedIndex = value;
                }
            }
        }

        Int32 IManageContract.TenantID
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

        List<Entity.Tenant> IManageContract.lstTenant
        {
            get
            {
                if (!ViewState["lstTenant"].IsNull())
                {
                    return ViewState["lstTenant"] as List<Entity.Tenant>;
                }

                return new List<Entity.Tenant>();
            }
            set
            {
                ViewState["lstTenant"] = value;
            }
        }

        Boolean IManageContract.IsReadOnlyPermission
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsReadOnlyPermission"] ?? false);
            }
            set
            {
                ViewState["IsReadOnlyPermission"] = value;
            }
        }

        #endregion

        #region Page Events

        /// <summary>
        /// set the page title on bread crumb
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.Title = "Manage Contracts";
                base.SetPageTitle("Manage Contracts");
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
            try
            {
                if (!this.IsPostBack)
                {
                    grdContract.Visible = false;
                    cmbExport.Visible = false;
                    cbExport.Visible = false;
                    Presenter.OnViewInitialized();
                    BindTenant();
                    CurrentViewContext.SelectedTenantID = Convert.ToInt32(ddlTenantName.SelectedValue);
                    BindRenewalType(ddlRenewalSrch);
                    BindContractType(cmbContractType);
                    BindDocumentStatus(cmbDocumentStatus);
                    Presenter.GetGranularPermission();
                }
                ifrExportDocument.Src = String.Empty;
                hdnTenantId.Value = CurrentViewContext.SelectedTenantID.ToString();
                lblInstituteHierarchyName.Text = hdnHierarchyLabel.Value.HtmlEncode();
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

        #region GridEvents

        #region Contract Grid Events

        protected void grdContract_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                CurrentViewContext.GridCustomPaging.CurrentPageIndex = CurrentPageIndex;
                CurrentViewContext.GridCustomPaging.PageSize = PageSize;
                if (CurrentViewContext.IsReset && !CurrentViewContext.IsAdminLoggedIn)
                {
                    grdContract.CurrentPageIndex = 0;
                    grdContract.MasterTableView.CurrentPageIndex = 0;
                    CurrentViewContext.VirtualRecordCount = 0;
                    CurrentViewContext.ContractDataList = new List<ContractManagementContract>();
                }
                else
                    Presenter.GetContractSearch();
                grdContract.DataSource = CurrentViewContext.ContractDataList;
                if (CurrentViewContext.IsReadOnlyPermission)
                {
                    grdContract.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;
                    grdContract.Columns.FindByUniqueName("DeleteColumn").Visible = false;
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

        protected void grdContract_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {

                if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
                {
                    // If NOT Insert Mode
                    if (!(e.Item is GridEditFormInsertItem))
                    {
                        CurrentViewContext.ContractId = Convert.ToInt32((e.Item as GridEditFormItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ContractId"]);
                        ViewState[_viewIsContractInEditMode] = true;
                    }
                    else
                    {
                        CurrentViewContext.ContractId = AppConsts.NONE;
                        hdnDPMGridMode.Value = String.Empty;
                        ViewState[_viewIsContractInEditMode] = null;
                    }

                    var _cmbRenewalType = (e.Item.FindControl("cmbRenewalType") as WclComboBox);
                    BindRenewalType(_cmbRenewalType);

                    var cmbContractType = (e.Item.FindControl("cmbContractType") as WclComboBox);
                    BindContractTypeInAddMode(cmbContractType);

                    var _cmbTenantGridMode = (e.Item.FindControl("cmbTenantGridMode") as WclComboBox);
                    if (_cmbTenantGridMode.IsNotNull())
                    {
                        _cmbTenantGridMode.DataSource = CurrentViewContext.lstTenant;
                        _cmbTenantGridMode.DataBind();
                        _cmbTenantGridMode.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(AppConsts.COMBOBOX_ITEM_SELECT, AppConsts.ZERO));
                    }
                    if (CurrentViewContext.SelectedTenantID > AppConsts.NONE)
                    {
                        _cmbTenantGridMode.Enabled = false;
                        _cmbTenantGridMode.SelectedValue = CurrentViewContext.SelectedTenantID.ToString();
                    }
                    else
                    {
                        _cmbTenantGridMode.Enabled = true;
                    }

                    if (e.Item.GetType() != typeof(GridEditFormInsertItem) && (e.Item.GetType() != typeof(GridDataInsertItem)))
                    {
                        Presenter.GetContractSitesContacts();
                        BindContractDocuments(e);
                        BindContractSites(e);
                        BindContractContacts(e);
                        // CurrentViewContext.ContractId = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ContractId"]);
                        Presenter.GetContractDetails();

                        ContractManagementContract contractManagementContract = e.Item.DataItem as ContractManagementContract;

                        (e.Item.FindControl("txtAffiliation") as WclTextBox).Text = CurrentViewContext.ContractData.AffiliationName;
                        (e.Item.FindControl("dpStartDate") as WclDatePicker).SelectedDate = CurrentViewContext.ContractData.StartDate;
                        (e.Item.FindControl("dpEdate") as WclDatePicker).SelectedDate = CurrentViewContext.ContractData.EndDate;

                        (e.Item.FindControl("txtDaysBefore") as WclTextBox).Text = CurrentViewContext.ContractData.DaysBeforeFrequency;
                        (e.Item.FindControl("txtFrequencyAfter") as WclNumericTextBox).Text = Convert.ToString(CurrentViewContext.ContractData.AfterFrequency);


                        (e.Item.FindControl("cmbRenewalType") as WclComboBox).SelectedValue = CurrentViewContext.ContractData.ExpTypeCode ?? CurrentViewContext.ContractData.ExpTypeCode;
                        (e.Item.FindControl("lblInstitutionHierarchyGridMode") as Label).Text = CurrentViewContext.ContractData.HierarchyNodes.HtmlEncode();

                        WclNumericTextBox _txtTerms = (WclNumericTextBox)e.Item.FindControl("txtTerms");
                        WclDatePicker _dpExpirationDate = (WclDatePicker)e.Item.FindControl("dpExpirationDate");
                        Panel _pnlCriteria = (Panel)e.Item.FindControl("pnlCriteria");
                        Literal _litLabel = (Literal)e.Item.FindControl("litLabel");

                        BindExpirationCriteria(_txtTerms, _dpExpirationDate, _pnlCriteria, _litLabel, CurrentViewContext.ContractData.ExpTypeCode);

                        if (CurrentViewContext.ContractData.ExpTypeCode == ContractExpirationType.EXPIRATION_DATE.GetStringValue())
                        {
                            _dpExpirationDate.SelectedDate = CurrentViewContext.ContractData.ExpirationDate;

                        }
                        else if (CurrentViewContext.ContractData.ExpTypeCode == ContractExpirationType.TERM.GetStringValue())
                        {
                            _txtTerms.Text = Convert.ToString(CurrentViewContext.ContractData.TermMonths);
                        }

                        (e.Item.FindControl("txtNotes") as WclTextBox).Text = CurrentViewContext.ContractData.Notes;

                        if (!contractManagementContract.ContractTypeIdList.IsNullOrEmpty())
                        {
                            String[] selectedIds = contractManagementContract.ContractTypeIdList.Split(',');
                            foreach (RadComboBoxItem item in cmbContractType.Items)
                            {
                                item.Checked = selectedIds.Contains(item.Value);
                            }
                        }

                        if (!CurrentViewContext.ContractData.lstNodeIds.IsNullOrEmpty())
                        {
                            StringBuilder _sbNodeIds = new StringBuilder();
                            foreach (var nodeId in CurrentViewContext.ContractData.lstNodeIds)
                            {
                                _sbNodeIds.Append(nodeId + ",");
                            }
                            if (_sbNodeIds.Length > 0)
                            {
                                var _selectedNodeIds = Convert.ToString(_sbNodeIds);
                                hdnDPMGridMode.Value = _selectedNodeIds.Substring(0, _selectedNodeIds.LastIndexOf(','));
                            }
                        }
                    }

                    if (CurrentViewContext.IsReadOnlyPermission)
                    {
                        DisableContractGridControls(e);
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

        protected void grdContract_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
        {
            try
            {
                if (e.NewSortOrder != GridSortOrder.None)
                {
                    CurrentViewContext.GridCustomPaging.SortExpression = e.SortExpression;
                    CurrentViewContext.GridCustomPaging.SortDirectionDescending = e.NewSortOrder.Equals(GridSortOrder.Descending);
                }
                else
                {
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

        protected void grdContract_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == RadGrid.InitInsertCommandName)
                {
                    ViewState[_viewStateContractSites] = null;
                    ViewState[_viewStateContractContacts] = null;
                    ViewState[_viewStateContractDocuments] = null;
                    ViewState[_viewIsContractInEditMode] = null;
                }
                else if (e.CommandName == RadGrid.PerformInsertCommandName || e.CommandName == RadGrid.UpdateCommandName)
                {
                    try
                    {
                        CurrentViewContext.ContractData = new ContractManagementContract();
                        if (e.CommandName == RadGrid.UpdateCommandName)
                        {
                            var editFormItem = e.Item as GridEditableItem;
                            CurrentViewContext.ContractData.ContractId = Convert.ToInt32(editFormItem.GetDataKeyValue("ContractId"));
                        }

                        CurrentViewContext.ContractData.AffiliationName = (e.Item.FindControl("txtAffiliation") as WclTextBox).Text;
                        CurrentViewContext.ContractData.StartDate = (e.Item.FindControl("dpStartDate") as WclDatePicker).SelectedDate;
                        CurrentViewContext.ContractData.EndDate = (e.Item.FindControl("dpEdate") as WclDatePicker).SelectedDate;
                        CurrentViewContext.ContractData.ExpTypeCode = (e.Item.FindControl("cmbRenewalType") as WclComboBox).SelectedValue;
                        CurrentViewContext.ContractData.Notes = (e.Item.FindControl("txtNotes") as WclTextBox).Text;
                        CurrentViewContext.ContractData.DaysBeforeFrequency = (e.Item.FindControl("txtDaysBefore") as WclTextBox).Text;

                        var _afterExpiryFrequency = (e.Item.FindControl("txtFrequencyAfter") as WclNumericTextBox).Text;
                        CurrentViewContext.ContractData.AfterFrequency = _afterExpiryFrequency.IsNullOrEmpty() ? (Int32?)null : Convert.ToInt32(_afterExpiryFrequency);

                        if (CurrentViewContext.ContractData.ExpTypeCode == ContractExpirationType.TERM.GetStringValue())
                        {
                            CurrentViewContext.ContractData.TermMonths = Convert.ToInt32((e.Item.FindControl("txtTerms") as WclNumericTextBox).Text);
                            CurrentViewContext.ContractData.ExpirationDate = Convert.ToDateTime(CurrentViewContext.ContractData.StartDate).AddMonths(Convert.ToInt32(CurrentViewContext.ContractData.TermMonths));
                        }
                        else if (CurrentViewContext.ContractData.ExpTypeCode == ContractExpirationType.EXPIRATION_DATE.GetStringValue())
                        {
                            CurrentViewContext.ContractData.ExpirationDate = (e.Item.FindControl("dpExpirationDate") as WclDatePicker).SelectedDate;
                        }
                        else if (CurrentViewContext.ContractData.ExpTypeCode == ContractExpirationType.BASED_ON_END_DATE.GetStringValue())
                        {
                            CurrentViewContext.ContractData.ExpirationDate = CurrentViewContext.ContractData.EndDate;
                        }

                        CurrentViewContext.ContractData.lstNodeIds = new List<Int32>();

                        if (!hdnDPMGridMode.Value.IsNullOrEmpty())
                        {
                            var _selectedDPMIds = hdnDPMGridMode.Value.Split(',');
                            for (int i = 0; i < _selectedDPMIds.Count(); i++)
                            {
                                CurrentViewContext.ContractData.lstNodeIds.Add(Convert.ToInt32(_selectedDPMIds[i]));
                            }
                        }
                        //else
                        //{
                        //    Presenter.GetRootNodeDPMId();
                        //    CurrentViewContext.ContractData.lstNodeIds.Add(Convert.ToInt32(CurrentViewContext.RootDPMId));
                        //}

                        var cmbContractType = (e.Item.FindControl("cmbContractType") as WclComboBox);
                        List<Int32> selectedContractType = new List<Int32>();
                        foreach (RadComboBoxItem slctdContractType in cmbContractType.CheckedItems)
                        {
                            selectedContractType.Add(Convert.ToInt32(slctdContractType.Value));
                        }
                        CurrentViewContext.ContractData.ContractTypeIdList = String.Join(",", selectedContractType.ToArray());

                        CurrentViewContext.ContractData.lstSites = new List<SiteContract>();

                        var _lstSites = ViewState[_viewStateContractSites] as List<SiteContract>;

                        if (!_lstSites.IsNullOrEmpty())
                        {
                            foreach (var site in _lstSites)
                            {
                                CurrentViewContext.ContractData.lstSites.Add(new SiteContract
                                {
                                    SiteName = site.SiteName,
                                    SiteAddress = site.SiteAddress,
                                    ContractSiteMappingId = site.ContractSiteMappingId,
                                    IsTempDeleted = site.IsTempDeleted,
                                    lstSiteDocumentContract = site.lstSiteDocumentContract,
                                    lstSiteContacts = site.lstSiteContacts,
                                    StartDate = site.StartDate,
                                    EndDate = site.EndDate,
                                    ExpTypeCode = site.ExpTypeCode,
                                    Notes = site.Notes,
                                    DaysBeforeFrequency = site.DaysBeforeFrequency,
                                    AfterFrequency = site.AfterFrequency,
                                    TermMonths = site.TermMonths,
                                    ExpirationDate = site.ExpirationDate,
                                    ContractTypeIdList = site.ContractTypeIdList
                                });
                            }
                        }



                        #region Contract Document

                        CurrentViewContext.ContractData.ContractDocumentContractList = new List<ContractDocumentContract>();
                        if (!ViewState[_viewStateContractDocuments].IsNullOrEmpty())
                        {
                            foreach (ContractDocumentContract doc in (ViewState[_viewStateContractDocuments] as List<ContractDocumentContract>))
                            {
                                CurrentViewContext.ContractData.ContractDocumentContractList.Add(new ContractDocumentContract
                                {
                                    ContractDocumentMappingID = doc.ContractDocumentMappingID,
                                    ClientSystemDocumentID = doc.ClientSystemDocumentID,
                                    DocumentName = doc.DocumentName,
                                    DocTypeCode = doc.DocTypeCode,
                                    ParentDocID = doc.ParentDocID,
                                    DocFileName = doc.DocFileName,
                                    DocSize = doc.DocSize,
                                    DocPath = doc.DocPath,
                                    DocStartDate = DateTime.Now,
                                    DocEndDate = doc.DocEndDate,
                                    IsDeleted = doc.IsDeleted,
                                    IsActive = doc.IsActive,
                                    //UAT-1665
                                    DocStatusID = doc.DocStatusID > AppConsts.NONE ? doc.DocStatusID : (int?)null,
                                });
                            }
                        }


                        #endregion

                        CurrentViewContext.ContractData.lstContacts = new List<ContactContract>();
                        if (!ViewState[_viewStateContractContacts].IsNullOrEmpty())
                        {
                            var _lstContacts = ViewState[_viewStateContractContacts] as List<ContactContract>;

                            if (!_lstContacts.IsNullOrEmpty())
                            {
                                foreach (var contact in _lstContacts)
                                {
                                    CurrentViewContext.ContractData.lstContacts.Add(new ContactContract
                                    {
                                        FirstName = contact.FirstName,
                                        LastName = contact.LastName,
                                        Email = contact.Email,
                                        Phone = contact.Phone,
                                        IsInternationalPhone = contact.IsInternationalPhone,
                                        Title = contact.Title,
                                        ContractContactMappingId = contact.ContractContactMappingId,
                                        IsTempDeleted = contact.IsTempDeleted
                                    });
                                }
                            }
                        }


                        if (CurrentViewContext.ContractData.ContractId == AppConsts.NONE)
                        {
                            Presenter.SaveContracts();
                            base.ShowSuccessMessage("Contract saved successfully.");
                        }
                        else
                        {
                            Presenter.UpdateContract();
                            base.ShowSuccessMessage("Contract updated successfully.");
                        }
                        hdnDPMGridMode.Value = String.Empty;
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

                //UAT-1475	Make it easier to view a contract entry.
                if (e.CommandName.Equals("ViewDocument"))
                {
                    Int32 ContractId = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ContractId"]);
                    String ContrcatName = e.CommandArgument.ToString();

                    Presenter.GetContractDocumentForViewDocument(ContractId);

                    ShowDocumentPopUp(ContrcatName, ContractId);
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

        protected void grdContract_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                Int32 contractID = Convert.ToInt32((e.Item as GridDataItem).GetDataKeyValue("ContractId"));
                if (Presenter.DeleteContract(contractID))
                {
                    base.ShowSuccessMessage("Contract deleted successfully.");
                }
                else
                {
                    base.ShowErrorMessage("Some error occured while deleting contract.");
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

        #region Sites Grid Events

        protected void grdSites_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                WclGrid _grdSites = (WclGrid)sender;
                _grdSites.DataSource = GetSiteList();
                if (CurrentViewContext.IsReadOnlyPermission)
                {
                    _grdSites.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;
                    _grdSites.Columns.FindByUniqueName("DeleteColumn").Visible = false;
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

        protected void grdSites_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == RadGrid.InitInsertCommandName)
                {
                    ViewState[_viewSiteDocuments] = null;
                    ViewState[_viewIsSiteInEditMode] = null;
                    ViewState[_viewStateSiteContacts] = null;
                }

                //UAT-1475	Make it easier to view a contract entry.
                if (e.CommandName.Equals("ViewDocument"))
                {
                    CurrentViewContext.SiteId = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["SiteId"]);
                    String siteName = e.CommandArgument.ToString();

                    Presenter.GetSiteDocuments();
                    ShowSiteDocumentPopUp(siteName);
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

        protected void grdSites_DataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
                {
                    // If NOT Insert Mode
                    if (!(e.Item is GridEditFormInsertItem))
                    {
                        CurrentViewContext.SiteId = Convert.ToInt32((e.Item as GridEditFormItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["SiteId"]);

                        ViewState[_viewIsSiteInEditMode] = true;
                        GridDataItem parentItem = e.Item.OwnerTableView.ParentItem;
                    }

                    var _cmbRenewalType = (e.Item.FindControl("cmbSiteRenewalType") as WclComboBox);
                    BindRenewalType(_cmbRenewalType);

                    var cmbContractType = (e.Item.FindControl("cmbContractType") as WclComboBox);
                    BindContractTypeInAddMode(cmbContractType);

                    if (e.Item.GetType() != typeof(GridEditFormInsertItem) && (e.Item.GetType() != typeof(GridDataInsertItem)))
                    {
                        BindSiteDocuments(e);
                        BindSiteContacts(e);
                        if (CurrentViewContext.IsReadOnlyPermission)
                        {
                            DisableSiteGridControls(e);
                        }

                        SiteContract siteContractData = e.Item.DataItem as SiteContract;
                        (e.Item.FindControl("dpSiteStartDate") as WclDatePicker).SelectedDate = siteContractData.StartDate;
                        (e.Item.FindControl("dpSiteEdate") as WclDatePicker).SelectedDate = siteContractData.EndDate;

                        (e.Item.FindControl("txtSiteDaysBefore") as WclTextBox).Text = siteContractData.DaysBeforeFrequency;
                        (e.Item.FindControl("txtSiteFrequencyAfter") as WclNumericTextBox).Text = Convert.ToString(siteContractData.AfterFrequency);


                        (e.Item.FindControl("cmbSiteRenewalType") as WclComboBox).SelectedValue = siteContractData.ExpTypeCode ?? siteContractData.ExpTypeCode;


                        WclNumericTextBox _txtTerms = (WclNumericTextBox)e.Item.FindControl("txtSiteTerms");
                        WclDatePicker _dpExpirationDate = (WclDatePicker)e.Item.FindControl("dpSiteExpirationDate");
                        Panel _pnlCriteria = (Panel)e.Item.FindControl("pnlCriteria");
                        Literal _litLabel = (Literal)e.Item.FindControl("litLabel");

                        BindExpirationCriteria(_txtTerms, _dpExpirationDate, _pnlCriteria, _litLabel, siteContractData.ExpTypeCode);

                        if (siteContractData.ExpTypeCode == ContractExpirationType.EXPIRATION_DATE.GetStringValue())
                        {
                            _dpExpirationDate.SelectedDate = siteContractData.ExpirationDate;

                        }
                        else if (siteContractData.ExpTypeCode == ContractExpirationType.TERM.GetStringValue())
                        {
                            _txtTerms.Text = Convert.ToString(siteContractData.TermMonths);
                        }

                        (e.Item.FindControl("txtSiteNotes") as WclTextBox).Text = siteContractData.Notes;

                        if (!siteContractData.ContractTypeIdList.IsNullOrEmpty())
                        {
                            String[] selectedIds = siteContractData.ContractTypeIdList.Split(',');
                            foreach (RadComboBoxItem item in cmbContractType.Items)
                            {
                                item.Checked = selectedIds.Contains(item.Value);
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

        protected void grdSites_InsertCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                List<SiteContract> list = GetSiteList(true);
                var _maxId = 0;
                if (!list.IsNullOrEmpty())
                {
                    _maxId = list.Max(cs => cs.TempSiteId);
                }

                var _lstSiteDocuments = ViewState[_viewSiteDocuments] as List<SiteDocumentContract>;
                var _lstSiteContacts = ViewState[_viewStateSiteContacts] as List<ContactContract>;

                SiteContract siteContract = new SiteContract();
                siteContract.SiteName = (e.Item.FindControl("txtSiteName") as WclTextBox).Text;
                siteContract.SiteAddress = (e.Item.FindControl("txtSiteAddress") as WclTextBox).Text;
                siteContract.TempSiteId = _maxId + 1;
                siteContract.lstSiteDocumentContract = _lstSiteDocuments;
                siteContract.lstSiteContacts = _lstSiteContacts;
                siteContract.StartDate = (e.Item.FindControl("dpSiteStartDate") as WclDatePicker).SelectedDate;
                siteContract.EndDate = (e.Item.FindControl("dpSiteEdate") as WclDatePicker).SelectedDate;
                siteContract.ExpTypeCode = (e.Item.FindControl("cmbSiteRenewalType") as WclComboBox).SelectedValue;
                siteContract.Notes = (e.Item.FindControl("txtSiteNotes") as WclTextBox).Text;
                siteContract.DaysBeforeFrequency = (e.Item.FindControl("txtSiteDaysBefore") as WclTextBox).Text;

                var _afterExpiryFrequency = (e.Item.FindControl("txtSiteFrequencyAfter") as WclNumericTextBox).Text;
                siteContract.AfterFrequency = _afterExpiryFrequency.IsNullOrEmpty() ? (Int32?)null : Convert.ToInt32(_afterExpiryFrequency);

                if (siteContract.ExpTypeCode == ContractExpirationType.TERM.GetStringValue())
                {
                    siteContract.TermMonths = Convert.ToInt32((e.Item.FindControl("txtSiteTerms") as WclNumericTextBox).Text);
                    siteContract.ExpirationDate = Convert.ToDateTime(siteContract.StartDate).AddMonths(Convert.ToInt32(siteContract.TermMonths));
                }
                else if (siteContract.ExpTypeCode == ContractExpirationType.EXPIRATION_DATE.GetStringValue())
                {
                    siteContract.ExpirationDate = (e.Item.FindControl("dpSiteExpirationDate") as WclDatePicker).SelectedDate;
                }
                else if (siteContract.ExpTypeCode == ContractExpirationType.BASED_ON_END_DATE.GetStringValue())
                {
                    siteContract.ExpirationDate = siteContract.EndDate;
                }

                var cmbContractType = (e.Item.FindControl("cmbContractType") as WclComboBox);
                List<Int32> selectedContractType = new List<Int32>();
                foreach (RadComboBoxItem slctdContractType in cmbContractType.CheckedItems)
                {
                    selectedContractType.Add(Convert.ToInt32(slctdContractType.Value));
                }
                siteContract.ContractTypeIdList = String.Join(",", selectedContractType.ToArray());

                list.Add(siteContract);
                ViewState[_viewStateContractSites] = list;
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

        protected void grdSites_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                List<SiteContract> list = GetSiteList(true);
                var _tempSiteId = Convert.ToInt32((e.Item as GridEditFormItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TempSiteId"]);

                var _siteToUpdate = list.Where(cs => cs.TempSiteId == _tempSiteId).First();

                var _lstSiteDocuments = ViewState[_viewSiteDocuments] as List<SiteDocumentContract>;
                var _lstSiteContacts = ViewState[_viewStateSiteContacts] as List<ContactContract>;

                _siteToUpdate.SiteName = (e.Item.FindControl("txtSiteName") as WclTextBox).Text;
                _siteToUpdate.SiteAddress = (e.Item.FindControl("txtSiteAddress") as WclTextBox).Text;
                _siteToUpdate.lstSiteDocumentContract = _lstSiteDocuments;
                _siteToUpdate.lstSiteContacts = _lstSiteContacts;

                _siteToUpdate.StartDate = (e.Item.FindControl("dpSiteStartDate") as WclDatePicker).SelectedDate;
                _siteToUpdate.EndDate = (e.Item.FindControl("dpSiteEdate") as WclDatePicker).SelectedDate;
                _siteToUpdate.ExpTypeCode = (e.Item.FindControl("cmbSiteRenewalType") as WclComboBox).SelectedValue;
                _siteToUpdate.Notes = (e.Item.FindControl("txtSiteNotes") as WclTextBox).Text;
                _siteToUpdate.DaysBeforeFrequency = (e.Item.FindControl("txtSiteDaysBefore") as WclTextBox).Text;

                var _afterExpiryFrequency = (e.Item.FindControl("txtSiteFrequencyAfter") as WclNumericTextBox).Text;
                _siteToUpdate.AfterFrequency = _afterExpiryFrequency.IsNullOrEmpty() ? (Int32?)null : Convert.ToInt32(_afterExpiryFrequency);

                if (_siteToUpdate.ExpTypeCode == ContractExpirationType.TERM.GetStringValue())
                {
                    _siteToUpdate.TermMonths = Convert.ToInt32((e.Item.FindControl("txtSiteTerms") as WclNumericTextBox).Text);
                    _siteToUpdate.ExpirationDate = Convert.ToDateTime(_siteToUpdate.StartDate).AddMonths(Convert.ToInt32(_siteToUpdate.TermMonths));
                }
                else if (_siteToUpdate.ExpTypeCode == ContractExpirationType.EXPIRATION_DATE.GetStringValue())
                {
                    _siteToUpdate.ExpirationDate = (e.Item.FindControl("dpSiteExpirationDate") as WclDatePicker).SelectedDate;
                }
                else if (_siteToUpdate.ExpTypeCode == ContractExpirationType.BASED_ON_END_DATE.GetStringValue())
                {
                    _siteToUpdate.ExpirationDate = _siteToUpdate.EndDate;
                }

                var cmbContractType = (e.Item.FindControl("cmbContractType") as WclComboBox);
                List<Int32> selectedContractType = new List<Int32>();
                foreach (RadComboBoxItem slctdContractType in cmbContractType.CheckedItems)
                {
                    selectedContractType.Add(Convert.ToInt32(slctdContractType.Value));
                }
                _siteToUpdate.ContractTypeIdList = String.Join(",", selectedContractType.ToArray());

                ViewState[_viewStateContractSites] = list;
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

        protected void grdSites_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                List<SiteContract> list = GetSiteList(true);
                var _tempSiteId = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TempSiteId"]);

                var _deleteSite = list.Where(cs => cs.TempSiteId == _tempSiteId).First();
                if (_deleteSite.SiteId > AppConsts.NONE)
                {
                    _deleteSite.IsTempDeleted = true;
                }
                else
                {
                    list.Remove(_deleteSite);
                }
                ViewState[_viewStateContractSites] = list;
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

        #region Contact Grid events
        protected void grdContacts_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            WclGrid _grdSites = (WclGrid)sender;
            _grdSites.DataSource = GetContactList();
            if (CurrentViewContext.IsReadOnlyPermission)
            {
                _grdSites.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;
                _grdSites.Columns.FindByUniqueName("DeleteColumn").Visible = false;
            }
        }

        protected void grdContacts_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
                {
                    if (e.Item.GetType() != typeof(GridEditFormInsertItem) && (e.Item.GetType() != typeof(GridDataInsertItem)))
                    {
                        if (CurrentViewContext.IsReadOnlyPermission)
                        {
                            DisableContactGridControls(e);
                        }
                    }
                    #region UAT-2447
                    var chkInternationalPhone1 = (e.Item.FindControl("chkInternationalPhone1") as WclCheckBox);
                    ShowHidePhoneControlsgrdContacts(chkInternationalPhone1.Checked, e.Item as GridEditFormItem);
                    #endregion
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

        protected void grdContacts_InsertCommand(object sender, GridCommandEventArgs e)
        {
            List<ContactContract> list = GetContactList(true);
            var _maxId = 0;
            if (!list.IsNullOrEmpty())
            {
                _maxId = list.Max(cc => cc.TempContactId);
            }
            //UAT-2247
            ContactContract contactDetail = new ContactContract();
            contactDetail.FirstName = (e.Item.FindControl("txtFirstName") as WclTextBox).Text;
            contactDetail.LastName = (e.Item.FindControl("txtLastName") as WclTextBox).Text;
            contactDetail.Email = (e.Item.FindControl("txtEmail") as WclTextBox).Text;
            if ((e.Item.FindControl("chkInternationalPhone1") as WclCheckBox).Checked)
            {
                contactDetail.Phone = (e.Item.FindControl("txtInternationalPhone1") as WclTextBox).Text;
            }
            else
            {
                contactDetail.Phone = (e.Item.FindControl("txtPhone1") as WclMaskedTextBox).Text;
            }
            contactDetail.IsInternationalPhone = (e.Item.FindControl("chkInternationalPhone1") as WclCheckBox).Checked;

            contactDetail.Title = (e.Item.FindControl("txtTitle") as WclTextBox).Text;
            contactDetail.TempContactId = _maxId + 1;

            list.Add(contactDetail);

            ViewState[_viewStateContractContacts] = list;
        }

        protected void grdContacts_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            List<ContactContract> list = GetContactList(true);
            var _tempContactId = Convert.ToInt32((e.Item as GridEditFormItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TempContactId"]);

            var _contactToUpdate = list.Where(cc => cc.TempContactId == _tempContactId).First();

            _contactToUpdate.FirstName = (e.Item.FindControl("txtFirstName") as WclTextBox).Text;
            _contactToUpdate.LastName = (e.Item.FindControl("txtLastName") as WclTextBox).Text;
            _contactToUpdate.Title = (e.Item.FindControl("txtTitle") as WclTextBox).Text;
            //UAT-2447
            _contactToUpdate.IsInternationalPhone = (e.Item.FindControl("chkInternationalPhone1") as WclCheckBox).Checked;
            if ((e.Item.FindControl("chkInternationalPhone1") as WclCheckBox).Checked)
            {
                _contactToUpdate.Phone = (e.Item.FindControl("txtInternationalPhone1") as WclTextBox).Text;
            }
            else
            {
                _contactToUpdate.Phone = (e.Item.FindControl("txtPhone1") as WclMaskedTextBox).Text;
            }
            _contactToUpdate.Email = (e.Item.FindControl("txtEmail") as WclTextBox).Text;

            ViewState[_viewStateContractContacts] = list;
        }

        protected void grdContacts_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            List<ContactContract> list = GetContactList(true);
            var _tempContactId = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TempContactId"]);

            //list.Remove(list.Where(cc => cc.TempContactId == _tempContactId).First());

            var _deleteContact = list.Where(cs => cs.TempContactId == _tempContactId).First();
            if (_deleteContact.ContactId > AppConsts.NONE)
            {
                _deleteContact.IsTempDeleted = true;
            }
            else
            {
                list.Remove(_deleteContact);
            }

            ViewState[_viewStateContractContacts] = list;
        }
        #endregion

        #region Contract Document

        protected void grdContractDocuments_InsertCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (!ManageUploadDocument(e.Item, false) || SavedDocPath.IsNullOrEmpty() || TempDocPath.IsNullOrEmpty())
                {
                    e.Canceled = true;
                    return;
                }
                List<ContractDocumentContract> list = GetDocumentList(true);
                var _maxId = 0;
                if (!list.IsNullOrEmpty())
                {
                    _maxId = list.Max(cs => cs.TempDocID);
                }

                list.Add(new ContractDocumentContract
                {
                    DocumentName = (e.Item.FindControl("txtDocumentName") as WclTextBox).Text,
                    DocTypeCode = (e.Item.FindControl("cmbDocType") as WclComboBox).SelectedValue,
                    DocFileName = hdnDocumentFileName.Value,
                    DocSize = Convert.ToInt32(new FileInfo(TempDocPath).Length),
                    DocPath = SavedDocPath,
                    DocumentTypeName = (e.Item.FindControl("cmbDocType") as WclComboBox).Text,
                    IsActive = true,
                    IsDeleted = false,
                    ParentDocID = null,
                    TempDocID = _maxId + AppConsts.ONE,
                    //UAT-1665: There should be a way to identify a master agreement and site agreement as Pending or Executed.
                    DocStatusID = Convert.ToInt32((e.Item.FindControl("cmbDocStatus") as WclComboBox).SelectedValue) > AppConsts.NONE ?
                                    Convert.ToInt32((e.Item.FindControl("cmbDocStatus") as WclComboBox).SelectedValue) : (int?)null,
                    DocStatusName = Convert.ToInt32((e.Item.FindControl("cmbDocStatus") as WclComboBox).SelectedValue) > AppConsts.NONE ?
                    Convert.ToString((e.Item.FindControl("cmbDocStatus") as WclComboBox).SelectedItem.Text) : String.Empty,
                });

                ViewState[_viewStateContractDocuments] = list;
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

        protected void grdContractDocuments_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                List<ContractDocumentContract> list = GetDocumentList(true);
                var tempDocID = Convert.ToInt32((e.Item as GridEditFormItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TempDocID"]);

                ContractDocumentContract docToUpdate = list.Where(cs => cs.TempDocID == tempDocID).First();


                if (!ViewState[_viewIsContractInEditMode].IsNullOrEmpty() && Convert.ToBoolean(ViewState[_viewIsContractInEditMode])
                    && (e.Item.FindControl("chkCreateVersion") as RadButton).Checked)
                {
                    // mark current doc as inactive and change end date
                    docToUpdate.IsActive = false;
                    docToUpdate.DocEndDate = DateTime.Now;
                    //create new version
                    list.Add(new ContractDocumentContract()
                    {
                        ClientSystemDocumentID = docToUpdate.ClientSystemDocumentID,
                        DocEndDate = null,
                        DocStartDate = DateTime.Now,
                        DocumentName = (e.Item.FindControl("txtDocumentName") as WclTextBox).Text,
                        DocumentTypeName = docToUpdate.DocumentTypeName,
                        IsActive = true,
                        IsDeleted = false,
                        IsNewVersion = true,
                        ParentDocID = docToUpdate.ContractDocumentMappingID,
                        DocTypeCode = docToUpdate.DocTypeCode,
                        DocTypeID = docToUpdate.DocTypeID,
                        DocFileName = docToUpdate.DocFileName,
                        DocPath = docToUpdate.DocPath,
                        //UAT-1665
                        DocStatusID = Convert.ToInt32((e.Item.FindControl("cmbDocStatus") as WclComboBox).SelectedValue) > AppConsts.NONE ?
                                        Convert.ToInt32((e.Item.FindControl("cmbDocStatus") as WclComboBox).SelectedValue) : (int?)null,
                        DocStatusName = Convert.ToInt32((e.Item.FindControl("cmbDocStatus") as WclComboBox).SelectedValue) > AppConsts.NONE ?
                        Convert.ToString((e.Item.FindControl("cmbDocStatus") as WclComboBox).SelectedItem.Text) : String.Empty,

                    });
                }
                else if (ManageUploadDocument(e.Item, false) && !SavedDocPath.IsNullOrEmpty() && !TempDocPath.IsNullOrEmpty())
                {
                    docToUpdate.DocumentName = (e.Item.FindControl("txtDocumentName") as WclTextBox).Text;
                    docToUpdate.DocFileName = hdnDocumentFileName.Value;
                    docToUpdate.DocSize = Convert.ToInt32(new FileInfo(TempDocPath).Length);
                    docToUpdate.DocPath = SavedDocPath;
                    docToUpdate.IsDocUpdated = true;
                    //UAT-1665
                    docToUpdate.DocStatusID = Convert.ToInt32((e.Item.FindControl("cmbDocStatus") as WclComboBox).SelectedValue) > AppConsts.NONE ?
                                                Convert.ToInt32((e.Item.FindControl("cmbDocStatus") as WclComboBox).SelectedValue) : (int?)null;
                    docToUpdate.DocStatusName = Convert.ToInt32((e.Item.FindControl("cmbDocStatus") as WclComboBox).SelectedValue) > AppConsts.NONE ?
                  Convert.ToString((e.Item.FindControl("cmbDocStatus") as WclComboBox).SelectedItem.Text) : String.Empty;
                }
                else
                {
                    docToUpdate.DocumentName = (e.Item.FindControl("txtDocumentName") as WclTextBox).Text;
                    docToUpdate.IsDocUpdated = true;
                    //UAT-1665
                    docToUpdate.DocStatusID = Convert.ToInt32((e.Item.FindControl("cmbDocStatus") as WclComboBox).SelectedValue) > AppConsts.NONE ?
                                                Convert.ToInt32((e.Item.FindControl("cmbDocStatus") as WclComboBox).SelectedValue) : (int?)null;
                    docToUpdate.DocStatusName = Convert.ToInt32((e.Item.FindControl("cmbDocStatus") as WclComboBox).SelectedValue) > AppConsts.NONE ?
                   Convert.ToString((e.Item.FindControl("cmbDocStatus") as WclComboBox).SelectedItem.Text) : String.Empty;
                }
                ViewState[_viewStateContractDocuments] = list;
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

        protected void grdContractDocuments_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                List<ContractDocumentContract> list = GetDocumentList(true);
                Int32 tempDocID = Convert.ToInt32((e.Item as GridDataItem).GetDataKeyValue("TempDocID"));
                ContractDocumentContract docDeleted = list.Where(cond => cond.TempDocID == tempDocID).First();
                if (docDeleted.ContractDocumentMappingID > AppConsts.NONE)
                {
                    docDeleted.IsDeleted = true;
                }
                else
                {
                    list.Remove(docDeleted);
                }

                ViewState[_viewStateContractDocuments] = list;
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

        protected void grdContractDocuments_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {

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

        protected void grdContractDocuments_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                WclGrid grdContractDocuments = (WclGrid)sender;
                grdContractDocuments.DataSource = GetDocumentList();
                if (CurrentViewContext.IsReadOnlyPermission)
                {
                    grdContractDocuments.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;
                    grdContractDocuments.Columns.FindByUniqueName("DeleteColumn").Visible = false;
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

        protected void grdContractDocuments_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridEditableItem && e.Item.IsInEditMode)
                {
                    GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                    WclComboBox cmbDocType = gridEditableItem.FindControl("cmbDocType") as WclComboBox;
                    Label lblUploadFormName = gridEditableItem.FindControl("lblUploadFormName") as Label;
                    //UAT-1665: There should be a way to identify a master agreement and site agreement as Pending or Executed.
                    WclComboBox cmbDocStatus = gridEditableItem.FindControl("cmbDocStatus") as WclComboBox;

                    BindDocumentType(cmbDocType);
                    //UAT-1665: There should be a way to identify a master agreement and site agreement as Pending or Executed.
                    BindDocumentStatus(cmbDocStatus);

                    if (!(e.Item is GridEditFormInsertItem))
                    {
                        ContractDocumentContract currentDoc = e.Item.DataItem as ContractDocumentContract;
                        if (!currentDoc.IsNullOrEmpty())
                        {
                            cmbDocType.SelectedValue = currentDoc.DocTypeCode;
                            cmbDocType.Enabled = false;
                            lblUploadFormName.Visible = true;
                            lblUploadFormName.Text = currentDoc.DocFileName.HtmlEncode();
                            //UAT-1665: There should be a way to identify a master agreement and site agreement as Pending or Executed.
                            //This should also be Search Criteria when searching for master agreements or contracts.
                            cmbDocStatus.SelectedValue = Convert.ToString(currentDoc.DocStatusID);

                            if (!ViewState[_viewIsContractInEditMode].IsNullOrEmpty() && Convert.ToBoolean(ViewState[_viewIsContractInEditMode])
                                //&& cmbDocType.SelectedValue == DocumentType.CONTRACT_DOCUMENT.GetStringValue()
                               && !currentDoc.IsDocUpdated && !currentDoc.IsNewVersion && currentDoc.ContractDocumentMappingID > AppConsts.NONE)
                            {
                                (gridEditableItem.FindControl("divDocVersion") as HtmlGenericControl).Style.Add("display", "block");
                            }
                            if (currentDoc.IsNewVersion)
                            {
                                (gridEditableItem.FindControl("divUploadDoc") as HtmlGenericControl).Style.Add("display", "none");
                            }
                        }
                    }
                    if (CurrentViewContext.IsReadOnlyPermission)
                    {
                        DisableDocumentGridControls(e);
                    }
                }
                else if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    ContractDocumentContract document = e.Item.DataItem as ContractDocumentContract;
                    if (!document.IsActive)
                    {
                        dataItem["DeleteColumn"].Controls[0].Visible = false;
                        dataItem["EditCommandColumn"].Controls[0].Visible = false;
                    }
                    else
                    {
                        dataItem["DeleteColumn"].Controls[0].Visible = true;
                        dataItem["EditCommandColumn"].Controls[0].Visible = true;
                    }
                }
                if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
                {
                    ContractDocumentContract currentDoc = e.Item.DataItem as ContractDocumentContract;
                    HtmlAnchor lnkDoc = (HtmlAnchor)e.Item.FindControl("lnkDoc");
                    if (currentDoc.DocPath.IsNullOrEmpty())
                    {
                        lnkDoc.Visible = false;
                    }
                    else
                    {
                        lnkDoc.HRef = String.Format("../ComplianceOperations/UserControl/DoccumentDownload.aspx?IsFileDownloadFromFilePath=true&FilePath={0}&FileName={1}", currentDoc.DocPath, currentDoc.DocFileName);
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

        #region SiteDocuments grid events
        protected void grdSiteDocuments_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                WclGrid grdSiteDocuments = (WclGrid)sender;
                grdSiteDocuments.DataSource = GetSiteDocumentList();
                if (CurrentViewContext.IsReadOnlyPermission)
                {
                    grdSiteDocuments.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;
                    grdSiteDocuments.Columns.FindByUniqueName("DeleteColumn").Visible = false;
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

        protected void grdSiteDocuments_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {

                if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
                {

                    GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                    WclComboBox cmbDocType = gridEditableItem.FindControl("cmbDocType") as WclComboBox;
                    Label lblUploadFormName = gridEditableItem.FindControl("lblUploadFormName") as Label;
                    //UAT-1665: There should be a way to identify a master agreement and site agreement as Pending or Executed.
                    WclComboBox cmbDocStatus = gridEditableItem.FindControl("cmbDocStatus") as WclComboBox;

                    BindDocumentType(cmbDocType);
                    //UAT-1665: There should be a way to identify a master agreement and site agreement as Pending or Executed.
                    BindDocumentStatus(cmbDocStatus);

                    if (!(e.Item is GridEditFormInsertItem))
                    {
                        SiteDocumentContract currentDoc = e.Item.DataItem as SiteDocumentContract;
                        if (!currentDoc.IsNullOrEmpty())
                        {
                            cmbDocType.SelectedValue = currentDoc.DocTypeCode;
                            cmbDocType.Enabled = false;
                            lblUploadFormName.Visible = true;
                            lblUploadFormName.Text = currentDoc.DocFileName.HtmlEncode();
                            //UAT-1665: There should be a way to identify a master agreement and site agreement as Pending or Executed.
                            cmbDocStatus.SelectedValue = Convert.ToString(currentDoc.DocStatusID);

                            if (!ViewState[_viewIsSiteInEditMode].IsNullOrEmpty() && Convert.ToBoolean(ViewState[_viewIsSiteInEditMode])
                                //&& cmbDocType.SelectedValue == DocumentType.CONTRACT_DOCUMENT.GetStringValue()
                               && !currentDoc.IsDocUpdated && !currentDoc.IsNewVersion && currentDoc.SiteDocumentMappingID > AppConsts.NONE)
                            {
                                (gridEditableItem.FindControl("divDocVersion") as HtmlGenericControl).Style.Add("display", "block");
                            }
                            if (currentDoc.IsNewVersion)
                            {
                                (gridEditableItem.FindControl("divUploadDoc") as HtmlGenericControl).Style.Add("display", "none");
                            }
                        }
                    }
                    if (CurrentViewContext.IsReadOnlyPermission)
                    {
                        DisableDocumentGridControls(e);
                        CommandBar fsucCmdBarPermission = (e.Item.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.FindControl("fsucCmdBarPermission") as CommandBar);
                        if (fsucCmdBarPermission.IsNotNull())
                        {
                            fsucCmdBarPermission.SaveButton.Visible = false;
                        }
                    }

                }
                else if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    SiteDocumentContract document = e.Item.DataItem as SiteDocumentContract;
                    if (!document.IsActive)
                    {
                        dataItem["DeleteColumn"].Controls[0].Visible = false;
                        dataItem["EditCommandColumn"].Controls[0].Visible = false;
                    }
                    else
                    {
                        dataItem["DeleteColumn"].Controls[0].Visible = true;
                        dataItem["EditCommandColumn"].Controls[0].Visible = true;
                    }
                }
                if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
                {
                    SiteDocumentContract currentDoc = e.Item.DataItem as SiteDocumentContract;
                    HtmlAnchor lnkDoc = (HtmlAnchor)e.Item.FindControl("lnkDoc");
                    if (currentDoc.DocPath.IsNullOrEmpty())
                    {
                        lnkDoc.Visible = false;
                    }
                    else
                    {
                        lnkDoc.HRef = String.Format("../ComplianceOperations/UserControl/DoccumentDownload.aspx?IsFileDownloadFromFilePath=true&FilePath={0}&FileName={1}", currentDoc.DocPath, currentDoc.DocFileName);
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

        protected void grdSiteDocuments_InsertCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (!ManageUploadDocument(e.Item, false) || SavedDocPath.IsNullOrEmpty() || TempDocPath.IsNullOrEmpty())
                {
                    e.Canceled = true;
                    return;
                }
                List<SiteDocumentContract> list = GetSiteDocumentList(true);
                var _maxId = 0;
                if (!list.IsNullOrEmpty())
                {
                    _maxId = list.Max(cs => cs.TempDocID);
                }

                list.Add(new SiteDocumentContract
                {
                    DocumentName = (e.Item.FindControl("txtDocumentName") as WclTextBox).Text,
                    DocStartDate = DateTime.Now,
                    DocTypeCode = (e.Item.FindControl("cmbDocType") as WclComboBox).SelectedValue,
                    DocFileName = hdnDocumentFileName.Value,
                    DocSize = Convert.ToInt32(new FileInfo(TempDocPath).Length),
                    DocPath = SavedDocPath,
                    DocumentTypeName = (e.Item.FindControl("cmbDocType") as WclComboBox).Text,
                    IsActive = true,
                    IsDeleted = false,
                    ParentDocID = null,
                    TempDocID = _maxId + AppConsts.ONE,
                    //UAT-1665: There should be a way to identify a master agreement and site agreement as Pending or Executed.
                    DocStatusID = Convert.ToInt32((e.Item.FindControl("cmbDocStatus") as WclComboBox).SelectedValue) > AppConsts.NONE ?
                                    Convert.ToInt32((e.Item.FindControl("cmbDocStatus") as WclComboBox).SelectedValue) : (int?)null,
                    DocStatusName = Convert.ToInt32((e.Item.FindControl("cmbDocStatus") as WclComboBox).SelectedValue) > AppConsts.NONE ?
                    Convert.ToString((e.Item.FindControl("cmbDocStatus") as WclComboBox).SelectedItem.Text) : String.Empty,
                });

                ViewState[_viewSiteDocuments] = list;
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

        protected void grdSiteDocuments_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                List<SiteDocumentContract> list = GetSiteDocumentList(true);
                var tempDocID = Convert.ToInt32((e.Item as GridEditFormItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TempDocID"]);

                SiteDocumentContract docToUpdate = list.Where(cs => cs.TempDocID == tempDocID).First();

                if (!ViewState[_viewIsSiteInEditMode].IsNullOrEmpty() && Convert.ToBoolean(ViewState[_viewIsSiteInEditMode])
                    && (e.Item.FindControl("chkCreateVersion") as RadButton).Checked)
                {
                    // mark current doc as inactive and change end date
                    docToUpdate.IsActive = false;
                    docToUpdate.DocEndDate = DateTime.Now;
                    //create new version
                    list.Add(new SiteDocumentContract()
                    {
                        ClientSystemDocumentID = docToUpdate.ClientSystemDocumentID,
                        DocEndDate = null,
                        DocStartDate = DateTime.Now,
                        DocumentName = (e.Item.FindControl("txtDocumentName") as WclTextBox).Text,
                        DocumentTypeName = docToUpdate.DocumentTypeName,
                        IsActive = true,
                        IsDeleted = false,
                        IsNewVersion = true,
                        ParentDocID = docToUpdate.SiteDocumentMappingID,
                        DocTypeCode = docToUpdate.DocTypeCode,
                        DocTypeID = docToUpdate.DocTypeID,
                        DocFileName = docToUpdate.DocFileName,
                        DocPath = docToUpdate.DocPath,
                        //UAT-1665
                        DocStatusID = Convert.ToInt32((e.Item.FindControl("cmbDocStatus") as WclComboBox).SelectedValue) > AppConsts.NONE ?
                                    Convert.ToInt32((e.Item.FindControl("cmbDocStatus") as WclComboBox).SelectedValue) : (int?)null,
                        DocStatusName = Convert.ToInt32((e.Item.FindControl("cmbDocStatus") as WclComboBox).SelectedValue) > AppConsts.NONE ?
                        Convert.ToString((e.Item.FindControl("cmbDocStatus") as WclComboBox).SelectedItem.Text) : String.Empty,
                    });
                }
                else if (ManageUploadDocument(e.Item, false) && !SavedDocPath.IsNullOrEmpty() && !TempDocPath.IsNullOrEmpty())
                {
                    docToUpdate.DocumentName = (e.Item.FindControl("txtDocumentName") as WclTextBox).Text;
                    docToUpdate.DocFileName = hdnDocumentFileName.Value;
                    docToUpdate.DocSize = Convert.ToInt32(new FileInfo(TempDocPath).Length);
                    docToUpdate.DocPath = SavedDocPath;
                    docToUpdate.IsDocUpdated = true;
                    //UAT-1665
                    docToUpdate.DocStatusID = Convert.ToInt32((e.Item.FindControl("cmbDocStatus") as WclComboBox).SelectedValue) > AppConsts.NONE ?
                                    Convert.ToInt32((e.Item.FindControl("cmbDocStatus") as WclComboBox).SelectedValue) : (int?)null;
                    docToUpdate.DocStatusName = Convert.ToInt32((e.Item.FindControl("cmbDocStatus") as WclComboBox).SelectedValue) > AppConsts.NONE ?
                   Convert.ToString((e.Item.FindControl("cmbDocStatus") as WclComboBox).SelectedItem.Text) : String.Empty;
                }
                else
                {
                    docToUpdate.DocumentName = (e.Item.FindControl("txtDocumentName") as WclTextBox).Text;
                    docToUpdate.IsDocUpdated = true;
                    //UAT-1665
                    docToUpdate.DocStatusID = Convert.ToInt32((e.Item.FindControl("cmbDocStatus") as WclComboBox).SelectedValue) > AppConsts.NONE ?
                                    Convert.ToInt32((e.Item.FindControl("cmbDocStatus") as WclComboBox).SelectedValue) : (int?)null;
                    docToUpdate.DocStatusName = Convert.ToInt32((e.Item.FindControl("cmbDocStatus") as WclComboBox).SelectedValue) > AppConsts.NONE ?
                   Convert.ToString((e.Item.FindControl("cmbDocStatus") as WclComboBox).SelectedItem.Text) : String.Empty;
                }
                ViewState[_viewSiteDocuments] = list;
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

        protected void grdSiteDocuments_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                List<SiteDocumentContract> list = GetSiteDocumentList(true);
                Int32 tempDocID = Convert.ToInt32((e.Item as GridDataItem).GetDataKeyValue("TempDocID"));
                SiteDocumentContract docDeleted = list.Where(cond => cond.TempDocID == tempDocID).First();
                if (docDeleted.SiteDocumentMappingID > AppConsts.NONE)
                {
                    docDeleted.IsDeleted = true;
                }
                else
                {
                    list.Remove(docDeleted);
                }

                ViewState[_viewSiteDocuments] = list;
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

        #region SiteContacts grid events
        protected void grdSiteContacts_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                WclGrid _grdSiteContacts = (WclGrid)sender;
                _grdSiteContacts.DataSource = GetSiteContactList();
                if (CurrentViewContext.IsReadOnlyPermission)
                {
                    _grdSiteContacts.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;
                    _grdSiteContacts.Columns.FindByUniqueName("DeleteColumn").Visible = false;
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

        protected void grdSiteContacts_InsertCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                List<ContactContract> list = GetSiteContactList(true);
                var _maxId = 0;
                if (!list.IsNullOrEmpty())
                {
                    _maxId = list.Max(cc => cc.TempContactId);
                }

                //list.Add(new ContactContract
                //{
                //    FirstName = (e.Item.FindControl("txtFirstName") as WclTextBox).Text,
                //    LastName = (e.Item.FindControl("txtLastName") as WclTextBox).Text,
                //    Email = (e.Item.FindControl("txtEmail") as WclTextBox).Text,
                //    Phone = (e.Item.FindControl("txtPhone") as WclMaskedTextBox).Text,
                //    Title = (e.Item.FindControl("txtTitle") as WclTextBox).Text,
                //    TempContactId = _maxId + 1
                //});
                //UAT-2447
                ContactContract contactDetail = new ContactContract();
                contactDetail.FirstName = (e.Item.FindControl("txtFirstName") as WclTextBox).Text;
                contactDetail.LastName = (e.Item.FindControl("txtLastName") as WclTextBox).Text;
                contactDetail.Email = (e.Item.FindControl("txtEmail") as WclTextBox).Text;

                //UAT-2447
                contactDetail.IsInternationalPhone = (e.Item.FindControl("chkInternationalPhone") as WclCheckBox).Checked;
                if ((e.Item.FindControl("chkInternationalPhone") as WclCheckBox).Checked)
                {
                    contactDetail.Phone = (e.Item.FindControl("txtInternationalPhone") as WclTextBox).Text;
                }
                else
                {
                    contactDetail.Phone = (e.Item.FindControl("txtPhone") as WclMaskedTextBox).Text;
                }

                contactDetail.Title = (e.Item.FindControl("txtTitle") as WclTextBox).Text;
                contactDetail.TempContactId = _maxId + 1;

                list.Add(contactDetail);

                ViewState[_viewStateSiteContacts] = list;
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

        protected void grdSiteContacts_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                List<ContactContract> list = GetSiteContactList(true);
                var _tempContactId = Convert.ToInt32((e.Item as GridEditFormItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TempContactId"]);

                var _contactToUpdate = list.Where(cc => cc.TempContactId == _tempContactId).First();

                _contactToUpdate.FirstName = (e.Item.FindControl("txtFirstName") as WclTextBox).Text;
                _contactToUpdate.LastName = (e.Item.FindControl("txtLastName") as WclTextBox).Text;
                _contactToUpdate.Title = (e.Item.FindControl("txtTitle") as WclTextBox).Text;
                //UAT-2447
                _contactToUpdate.IsInternationalPhone = (e.Item.FindControl("chkInternationalPhone") as WclCheckBox).Checked;
                if ((e.Item.FindControl("chkInternationalPhone") as WclCheckBox).Checked)
                {
                    _contactToUpdate.Phone = (e.Item.FindControl("txtInternationalPhone") as WclTextBox).Text;
                }
                else
                {
                    _contactToUpdate.Phone = (e.Item.FindControl("txtPhone") as WclMaskedTextBox).Text;
                }
                _contactToUpdate.Email = (e.Item.FindControl("txtEmail") as WclTextBox).Text;

                ViewState[_viewStateSiteContacts] = list;
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

        protected void grdSiteContacts_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridEditableItem && e.Item.IsInEditMode)
                {
                    GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                    if (CurrentViewContext.IsReadOnlyPermission)
                    {
                        DisableContactGridControls(e);
                        CommandBar fsucCmdBarPermission = (e.Item.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.FindControl("fsucCmdBarPermission") as CommandBar);
                        if (fsucCmdBarPermission.IsNotNull())
                        {
                            fsucCmdBarPermission.SaveButton.Visible = false;
                        }
                    }
                    #region UAT-2447
                    var chkInternationalPhone = (e.Item.FindControl("chkInternationalPhone") as WclCheckBox);
                    ShowHidePhoneControlsgrdSiteContacts(chkInternationalPhone.Checked, e.Item as GridEditFormItem);
                    #endregion
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

        protected void grdSiteContacts_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                List<ContactContract> list = GetSiteContactList(true);
                var _tempContactId = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TempContactId"]);

                //list.Remove(list.Where(cc => cc.TempContactId == _tempContactId).First());

                var _deleteContact = list.Where(cs => cs.TempContactId == _tempContactId).First();
                if (_deleteContact.ContactId > AppConsts.NONE)
                {
                    _deleteContact.IsTempDeleted = true;
                }
                else
                {
                    list.Remove(_deleteContact);
                }
                ViewState[_viewStateSiteContacts] = list;
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

        #region Button Events
        protected void CmdBarSearch_SaveClick(object sender, EventArgs e)
        {
            try
            {
                CurrentViewContext.IsReset = false;
                grdContract.Visible = true;
                cmbExport.Visible = true;
                cbExport.Visible = true;
                ViewState["ReBindGrid"] = null;
                grdContract.Rebind();
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

        protected void CmdBarSearch_ResetClick(object sender, EventArgs e)
        {
            try
            {
                CurrentViewContext.IsReset = true;
                ResetTenant();
                ResetControls();
                ResetGridFilters(grdContract);
                CurrentViewContext.VirtualRecordCount = 0;
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

        #region DropDown events
        protected void cmbRenewalType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                RadComboBox _combo = (RadComboBox)sender;

                var item = (GridEditFormItem)_combo.NamingContainer;

                if (_combo.NamingContainer.GetType() == typeof(GridEditFormInsertItem))
                {
                    item = (GridEditFormInsertItem)_combo.NamingContainer;
                }

                WclNumericTextBox _txtTerms = (WclNumericTextBox)item.FindControl("txtTerms");
                WclDatePicker _dpExpirationDate = (WclDatePicker)item.FindControl("dpExpirationDate");
                Panel _pnlCriteria = (Panel)item.FindControl("pnlCriteria");
                Literal _litLabel = (Literal)item.FindControl("litLabel");
                Label lblInstitutionHierarchyGridMode = (Label)item.FindControl("lblInstitutionHierarchyGridMode");
                if (lblInstitutionHierarchyGridMode.Text == String.Empty)
                {
                    lblInstitutionHierarchyGridMode.Text = hdnInstitutionHierarchyGridMode.Value.HtmlEncode();
                }
                BindExpirationCriteria(_txtTerms, _dpExpirationDate, _pnlCriteria, _litLabel, _combo.SelectedValue);
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

        protected void ddlTenantSaveMode_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadComboBox _combo = (RadComboBox)sender;
            GridEditFormInsertItem item = (GridEditFormInsertItem)_combo.NamingContainer;

            var _cmbRenewalType = (item.FindControl("cmbRenewalType") as WclComboBox);
            CurrentViewContext.SelectedTenantID = Convert.ToInt32(_combo.SelectedValue);
            BindRenewalType(_cmbRenewalType);
            var cmbContractType = (item.FindControl("cmbContractType") as WclComboBox);
            BindContractTypeInAddMode(cmbContractType);
            Label lblInstitutionHierarchyGridMode = (Label)item.FindControl("lblInstitutionHierarchyGridMode");
            lblInstitutionHierarchyGridMode.Text = String.Empty;
            hdnInstitutionHierarchyGridMode.Value = String.Empty;
            hdnDPMGridMode.Value = String.Empty;

        }

        protected void ddlTenantName_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                BindRenewalType(ddlRenewalSrch);
                BindContractType(cmbContractType);
                BindDocumentStatus(cmbDocumentStatus);
                ResetControls();
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

        protected void cmbSiteRenewalType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                RadComboBox _combo = (RadComboBox)sender;

                var item = (GridEditFormItem)_combo.NamingContainer;

                if (_combo.NamingContainer.GetType() == typeof(GridEditFormInsertItem))
                {
                    item = (GridEditFormInsertItem)_combo.NamingContainer;
                }

                WclNumericTextBox _txtTerms = (WclNumericTextBox)item.FindControl("txtSiteTerms");
                WclDatePicker _dpExpirationDate = (WclDatePicker)item.FindControl("dpSiteExpirationDate");
                Panel _pnlCriteria = (Panel)item.FindControl("pnlCriteria");
                Literal _litLabel = (Literal)item.FindControl("litLabel");
                BindExpirationCriteria(_txtTerms, _dpExpirationDate, _pnlCriteria, _litLabel, _combo.SelectedValue);
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

        #region Private Methods

        private void BindExpirationCriteria(WclNumericTextBox _txtTerms, WclDatePicker _dpExpirationDate, Panel _pnlCriteria, Literal _litLabel, String selectedValue)
        {
            if (selectedValue == ContractExpirationType.EXPIRATION_DATE.GetStringValue())
            {
                _txtTerms.Text = String.Empty;
                _txtTerms.Visible = false;
                _dpExpirationDate.Visible = true;
                _pnlCriteria.Visible = true;
                (_pnlCriteria.FindControl("rfvExpirationDate") as RequiredFieldValidator).Enabled = true;
                (_pnlCriteria.FindControl("rfvExpirationDate") as RequiredFieldValidator).Visible = true;
                (_pnlCriteria.FindControl("rfvTerms") as RequiredFieldValidator).Enabled = false;
                (_pnlCriteria.FindControl("rfvTerms") as RequiredFieldValidator).Visible = false;
                (_pnlCriteria.FindControl("rngvTerms") as RangeValidator).Enabled = false;
                (_pnlCriteria.FindControl("rngvTerms") as RangeValidator).Visible = false;

                (_pnlCriteria.FindControl("rfvStartDate") as RequiredFieldValidator).Enabled = false;
                (_pnlCriteria.FindControl("rfvStartDate") as RequiredFieldValidator).Enabled = false;

                (_pnlCriteria.FindControl("rfvEndDate") as RequiredFieldValidator).Enabled = false;
                (_pnlCriteria.FindControl("rfvEndDate") as RequiredFieldValidator).Enabled = false;

                _litLabel.Text = "Expiration Date";
                (_pnlCriteria.FindControl("spnRequired") as HtmlGenericControl).Visible = true;

            }
            else if (selectedValue == ContractExpirationType.TERM.GetStringValue())
            {
                _dpExpirationDate.SelectedDate = null;
                _dpExpirationDate.Visible = false;
                _txtTerms.Visible = true;
                _pnlCriteria.Visible = true;
                (_pnlCriteria.FindControl("rfvTerms") as RequiredFieldValidator).Enabled = true;
                (_pnlCriteria.FindControl("rfvTerms") as RequiredFieldValidator).Visible = true;
                (_pnlCriteria.FindControl("rngvTerms") as RangeValidator).Enabled = true;
                (_pnlCriteria.FindControl("rngvTerms") as RangeValidator).Visible = true;
                (_pnlCriteria.FindControl("rfvExpirationDate") as RequiredFieldValidator).Enabled = false;
                (_pnlCriteria.FindControl("rfvExpirationDate") as RequiredFieldValidator).Visible = false;

                (_pnlCriteria.FindControl("rfvStartDate") as RequiredFieldValidator).Enabled = true;
                (_pnlCriteria.FindControl("rfvStartDate") as RequiredFieldValidator).Enabled = true;

                (_pnlCriteria.FindControl("rfvEndDate") as RequiredFieldValidator).Enabled = true;
                (_pnlCriteria.FindControl("rfvEndDate") as RequiredFieldValidator).Enabled = true;

                _litLabel.Text = "No. of Months";
                (_pnlCriteria.FindControl("spnRequired") as HtmlGenericControl).Visible = true;
            }
            else if (selectedValue == ContractExpirationType.BASED_ON_END_DATE.GetStringValue())
            {
                _txtTerms.Text = String.Empty;
                _txtTerms.Visible = false;
                _dpExpirationDate.SelectedDate = null;
                _dpExpirationDate.Visible = false;
                _pnlCriteria.Visible = false;
                (_pnlCriteria.FindControl("rfvExpirationDate") as RequiredFieldValidator).Enabled = false;
                (_pnlCriteria.FindControl("rfvTerms") as RequiredFieldValidator).Enabled = false;
                (_pnlCriteria.FindControl("rfvExpirationDate") as RequiredFieldValidator).Visible = false;
                (_pnlCriteria.FindControl("rfvTerms") as RequiredFieldValidator).Visible = false;
                (_pnlCriteria.FindControl("rngvTerms") as RangeValidator).Enabled = false;
                (_pnlCriteria.FindControl("rngvTerms") as RangeValidator).Visible = false;

                (_pnlCriteria.FindControl("rfvStartDate") as RequiredFieldValidator).Enabled = true;
                (_pnlCriteria.FindControl("rfvStartDate") as RequiredFieldValidator).Enabled = true;

                (_pnlCriteria.FindControl("rfvEndDate") as RequiredFieldValidator).Enabled = true;
                (_pnlCriteria.FindControl("rfvEndDate") as RequiredFieldValidator).Enabled = true;
                _litLabel.Text = String.Empty;
                (_pnlCriteria.FindControl("spnRequired") as HtmlGenericControl).Visible = false;
            }
            else
            {
                (_pnlCriteria.FindControl("rfvStartDate") as RequiredFieldValidator).Enabled = false;
                (_pnlCriteria.FindControl("rfvStartDate") as RequiredFieldValidator).Enabled = false;

                (_pnlCriteria.FindControl("rfvEndDate") as RequiredFieldValidator).Enabled = false;
                (_pnlCriteria.FindControl("rfvEndDate") as RequiredFieldValidator).Enabled = false;

                _pnlCriteria.Visible = false;

            }
        }

        private void BindTenant()
        {
            _presenter.GetTenants();
            ddlTenantName.DataSource = CurrentViewContext.lstTenant;
            ddlTenantName.DataTextField = "TenantName";
            ddlTenantName.DataValueField = "TenantID";
            ddlTenantName.DataBind();
            ddlTenantName.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(AppConsts.COMBOBOX_ITEM_SELECT, AppConsts.ZERO));

            Presenter.IsAdminLoggedIn();
            if (CurrentViewContext.IsAdminLoggedIn)
            {
                ddlTenantName.Enabled = true;
                CurrentViewContext.SelectedTenantID = AppConsts.NONE;
            }
            else
            {
                CurrentViewContext.SelectedTenantID = CurrentViewContext.TenantID;
            }
        }

        private void BindRenewalType(WclComboBox cmbRenewalType)
        {
            List<lkpContractExpirationType> contractRenewalType = new List<lkpContractExpirationType>();
            contractRenewalType = Presenter.GetRenewalType();
            cmbRenewalType.DataSource = contractRenewalType.Where(cond => !cond.CET_IsDeleted);
            cmbRenewalType.DataTextField = "CET_NAME";
            cmbRenewalType.DataValueField = "CET_Code";
            cmbRenewalType.DataBind();
            cmbRenewalType.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(AppConsts.COMBOBOX_ITEM_SELECT, AppConsts.ZERO));
        }

        private void BindContractTypeInAddMode(WclComboBox cmbContractType)
        {
            List<ContractType> contractType = new List<ContractType>();
            contractType = Presenter.GetContractType();
            cmbContractType.DataSource = contractType.Where(cond => !cond.CT_IsDeleted);
            cmbContractType.DataTextField = "CT_Name";
            cmbContractType.DataValueField = "CT_ID";
            cmbContractType.DataBind();
        }

        private void BindContractType(WclComboBox cmbContractType)
        {
            List<ContractType> contractType = new List<ContractType>();
            contractType = Presenter.GetContractType();
            cmbContractType.DataSource = contractType.Where(cond => !cond.CT_IsDeleted);
            cmbContractType.DataTextField = "CT_Name";
            cmbContractType.DataValueField = "CT_ID";
            cmbContractType.DataBind();
            //cmbContractType.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(AppConsts.COMBOBOX_ITEM_SELECT, AppConsts.ZERO));
        }

        private void GetSearchParameters()
        {
            _searchContract = new ContractManagementContract();
            _searchContract.TenantId = CurrentViewContext.SelectedTenantID;
            if (!txtAffiliationSrch.Text.Trim().IsNullOrEmpty())
            {
                _searchContract.AffiliationName = txtAffiliationSrch.Text.Trim();
            }
            if (!txtSite.Text.Trim().IsNullOrEmpty())
            {
                _searchContract.Sites = txtSite.Text.Trim();
            }
            if (!hdnInstitutionNodeId.Value.Trim().IsNullOrEmpty())
            {
                _searchContract.HierarchyNodes = hdnDepartmntPrgrmMppng.Value.Trim();
            }
            if (!ddlRenewalSrch.SelectedValue.IsNullOrEmpty())
            {
                _searchContract.ExpCode = ddlRenewalSrch.SelectedValue;
            }
            if (!txtSdateSrch.SelectedDate.IsNullOrEmpty())
            {
                _searchContract.StartDate = txtSdateSrch.SelectedDate;
            }
            if (!txtEdateSrch.SelectedDate.IsNullOrEmpty())
            {
                _searchContract.EndDate = txtEdateSrch.SelectedDate;
            }
            if (!ddlRenewalSrch.SelectedValue.IsNullOrEmpty() && ddlRenewalSrch.SelectedValue != "0")
            {
                _searchContract.ExpTypeCode = ddlRenewalSrch.SelectedValue;
            }
            if (!rbSearchLevel.SelectedValue.IsNullOrEmpty() && rbSearchLevel.SelectedValue.IsNotNull())
            {
                _searchContract.SearchType = rbSearchLevel.SelectedValue;
            }
            if (cmbContractType.CheckedItems.Count > AppConsts.NONE)
            {
                _searchContract.ContractTypeIdList = String.Join(",", cmbContractType.CheckedItems.Select(cond => cond.Value).ToArray());
            }
            if (!cmbDocumentStatus.SelectedValue.IsNullOrEmpty() && cmbDocumentStatus.SelectedValue != "0")
            {
                _searchContract.DocumentStatusId = Convert.ToInt32(cmbDocumentStatus.SelectedValue);
            }
        }

        private void ResetControls()
        {
            txtAffiliationSrch.Text = String.Empty;
            txtSite.Text = String.Empty;
            hdnDepartmntPrgrmMppng.Value = String.Empty;
            hdnHierarchyLabel.Value = String.Empty;
            hdnInstitutionNodeId.Value = String.Empty;
            lblInstituteHierarchyName.Text = String.Empty;
            ddlRenewalSrch.ClearSelection();
            txtSdateSrch.Clear();
            txtEdateSrch.Clear();
            BindRenewalType(ddlRenewalSrch);
            BindContractType(cmbContractType);
            BindDocumentStatus(cmbDocumentStatus);
           // ResetGridFilters(grdContract);  //UAT-4214
            rbSearchLevel.SelectedValue = "MSTR";
        }

        private void ResetTenant()
        {
            if (CurrentViewContext.IsAdminLoggedIn)
            {
                CurrentViewContext.SelectedTenantID = AppConsts.NONE;
            }
            else
            {
                CurrentViewContext.SelectedTenantID = CurrentViewContext.TenantID;
            }
        }

        /// <summary>
        /// Method to Reset Grid 
        /// </summary>
        private void ResetGridFilters(RadGrid grdToReset)
        {
            grdToReset.MasterTableView.SortExpressions.Clear();
            grdToReset.CurrentPageIndex = 0;
            grdToReset.MasterTableView.CurrentPageIndex = 0;
            grdToReset.MasterTableView.IsItemInserted = false;
            grdToReset.MasterTableView.ClearEditItems();
            grdToReset.Rebind();
        }

        private List<SiteContract> GetSiteList(Boolean isIncludeDeletedSites = false)
        {
            List<SiteContract> list = null;
            if (!(ViewState[_viewStateContractSites] as List<SiteContract>).IsNullOrEmpty())
            {
                list = (ViewState[_viewStateContractSites] as List<SiteContract>).Where(cs => isIncludeDeletedSites || cs.IsTempDeleted == false).ToList();
            }
            else
            {
                list = new List<SiteContract>();
            }
            return list;
        }

        private List<ContactContract> GetContactList(Boolean isIncludeDeleted = false)
        {
            List<ContactContract> list = null;
            if (!(ViewState[_viewStateContractContacts] as List<ContactContract>).IsNullOrEmpty())
            {
                list = (ViewState[_viewStateContractContacts] as List<ContactContract>).Where(cs => isIncludeDeleted || cs.IsTempDeleted == false).ToList();
            }
            else
            {
                list = new List<ContactContract>();
            }
            return list;
        }

        private void BindContractSites(GridItemEventArgs e)
        {
            var _tempCount = 0;
            var _grdSites = (e.Item.FindControl("grdSites") as WclGrid);
            foreach (var site in CurrentViewContext.lstSiteContracts)
            {
                _tempCount += 1;
                site.TempSiteId = _tempCount;
            }

            ViewState[_viewStateContractSites] = CurrentViewContext.lstSiteContracts;
            _grdSites.DataSource = CurrentViewContext.lstSiteContracts;
            _grdSites.DataBind();
        }

        private void BindContractContacts(GridItemEventArgs e)
        {
            var _tempCount = 0;
            var _grdSites = (e.Item.FindControl("grdContacts") as WclGrid);
            foreach (var contact in CurrentViewContext.lstContactContract)
            {
                _tempCount += 1;
                contact.TempContactId = _tempCount;
            }

            ViewState[_viewStateContractContacts] = CurrentViewContext.lstContactContract;
            _grdSites.DataSource = CurrentViewContext.lstContactContract;
            _grdSites.DataBind();
        }

        private void BindContractDocuments(GridItemEventArgs e)
        {
            var _tempCount = 0;
            var grdContractDocuments = (e.Item.FindControl("grdContractDocuments") as WclGrid);
            Presenter.GetContractDocuments();
            foreach (ContractDocumentContract docContract in CurrentViewContext.ContractDocumentContractList)
            {
                _tempCount += 1;
                docContract.TempDocID = _tempCount;
            }
            ViewState[_viewStateContractDocuments] = CurrentViewContext.ContractDocumentContractList;
            grdContractDocuments.DataSource = CurrentViewContext.ContractDocumentContractList;
            grdContractDocuments.DataBind();
        }

        private List<ContractDocumentContract> GetDocumentList(Boolean isIncludeDeleted = false)
        {
            List<ContractDocumentContract> listDocs = null;
            if (!(ViewState[_viewStateContractDocuments] as List<ContractDocumentContract>).IsNullOrEmpty())
            {
                listDocs = ((List<ContractDocumentContract>)ViewState[_viewStateContractDocuments]).Where(cond => isIncludeDeleted || !cond.IsDeleted).ToList();
            }
            else
            {
                listDocs = new List<ContractDocumentContract>();
            }
            return listDocs;
        }

        private void BindDocumentType(WclComboBox cmbDocType)
        {
            List<lkpDocumentType> lstDocType = new List<lkpDocumentType>();
            lstDocType = Presenter.GetDocumentType();
            cmbDocType.DataSource = lstDocType;
            cmbDocType.DataTextField = "DMT_Name";
            cmbDocType.DataValueField = "DMT_Code";
            cmbDocType.DataBind();
            cmbDocType.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(AppConsts.COMBOBOX_ITEM_SELECT, AppConsts.ZERO));
        }

        private void BindSiteDocuments(GridItemEventArgs e)
        {
            var _tempCount = 0;
            var grdSiteDocuments = (e.Item.FindControl("grdSiteDocuments") as WclGrid);
            Presenter.GetSiteDocuments();
            foreach (SiteDocumentContract docContract in CurrentViewContext.SiteDocumentContractList)
            {
                _tempCount += 1;
                docContract.TempDocID = _tempCount;
            }
            ViewState[_viewSiteDocuments] = CurrentViewContext.SiteDocumentContractList;
            grdSiteDocuments.DataSource = CurrentViewContext.SiteDocumentContractList;
            grdSiteDocuments.DataBind();
        }

        private List<SiteDocumentContract> GetSiteDocumentList(Boolean isIncludeDeleted = false)
        {
            List<SiteDocumentContract> listDocs = null;
            if (!(ViewState[_viewSiteDocuments] as List<SiteDocumentContract>).IsNullOrEmpty())
            {
                listDocs = ((List<SiteDocumentContract>)ViewState[_viewSiteDocuments]).Where(cond => isIncludeDeleted || !cond.IsDeleted).ToList();
            }
            else
            {
                listDocs = new List<SiteDocumentContract>();
            }
            return listDocs;
        }

        private Boolean ManageUploadDocument(GridItem gridItem, Boolean isUpdate)
        {
            SavedDocPath = String.Empty;
            WclAsyncUpload uploadControl = gridItem.FindControl("uploadControl") as WclAsyncUpload;
            Label lblUploadFormMsg = (gridItem.FindControl("lblUploadFormMsg") as Label);
            lblUploadFormMsg.Visible = false;
            if (uploadControl.UploadedFiles.Count > 0)
            {
                String destFilePath = UploadDocument(uploadControl);
                if (destFilePath.IsNullOrEmpty())
                {
                    base.ShowErrorMessage("Some error occurred while uploading Contract Document. Please try again or contact System Administrator.");
                    SavedDocPath = String.Empty;
                    return false;
                }
                else
                {
                    SavedDocPath = SaveDocument(destFilePath);
                    return true;
                }
            }
            else if (!isUpdate)
            {
                lblUploadFormMsg.Visible = true;
                SavedDocPath = String.Empty;
                return false;
            }
            return true;
        }

        private String UploadDocument(WclAsyncUpload uploadControl)
        {
            String tempFilePath = WebConfigurationManager.AppSettings["TemporaryFileLocation"];
            if (tempFilePath.IsNullOrEmpty())
            {
                base.LogError("Please provide path for TemporaryFileLocation in config.", null);
                return String.Empty;
            }
            if (!tempFilePath.EndsWith(@"\"))
            {
                tempFilePath += @"\";
            }
            tempFilePath += "Tenant(" + CurrentViewContext.SelectedTenantID.ToString() + @")\";

            if (!Directory.Exists(tempFilePath))
                Directory.CreateDirectory(tempFilePath);

            UploadedFile item = uploadControl.UploadedFiles[0];
            hdnDocumentFileName.Value = item.FileName;
            String fileName = Guid.NewGuid().ToString() + Path.GetExtension(item.FileName);

            //Save file
            String newTempFilePath = Path.Combine(tempFilePath, fileName);
            item.SaveAs(newTempFilePath);
            TempDocPath = newTempFilePath;
            return newTempFilePath;
        }

        private String SaveDocument(String filePath)
        {

            String date = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("mmss") + DateTime.Now.Millisecond.ToString();
            String destFileName = "ContractRelatedDocument_" + CurrentViewContext.SelectedTenantID.ToString() + "_" + CurrentViewContext.CurrentUserId.ToString() + "_" + date + Path.GetExtension(filePath);
            String desFilePath = "Tenant(" + CurrentViewContext.SelectedTenantID.ToString() + @")\" + destFileName;

            String savedFilePath = CommonFileManager.SaveDocument(filePath, desFilePath, FileType.SystemDocumentLocation.GetStringValue());

            return savedFilePath;
        }

        //UAT-1475	Make it easier to view a contract entry.
        /// <summary>
        /// This method is creating html for creating table for displaying contracts name and passed to OpenPopUpViewDocument() in js file  for pop up.
        /// <param name="ContrcatName"></param>
        private void ShowDocumentPopUp(String ContrcatName, Int32 contractId)
        {
            StringBuilder documentDescription = new StringBuilder();
            Int32 count = CurrentViewContext.ContractDocumentContractList.IsNull() ? AppConsts.NONE : CurrentViewContext.ContractDocumentContractList.Count;
            documentDescription.Append(@"<div style='padding:10px;background-color:#f0f0f0;'>");
            documentDescription.Append(@"<div style='margin-bottom:10px'><div style='font-size:11px;font-weight:bold;float:left;margin-right:3px;'>Attached to: </div><div >" + ContrcatName + "</div>");
            documentDescription.Append(@"<div style='overflow:auto;max-height: 150px;'>");
            documentDescription.Append(@"<table border='0' cellpadding='0' id='documentDescription' cellspacing='0'>");
            documentDescription.AppendFormat("<tr><td class='td-one'></td><td class='td-two' style='padding: 5px; border: 1px solid black; border-image: none;width:100%;'><b>Document Name</b></td></tr>");
            foreach (var docName in CurrentViewContext.ContractDocumentContractList)
            {
                if (docName.DocPath.IsNullOrEmpty())
                {
                    documentDescription.AppendFormat("<tr><td class='td-one'></td><td class='td-two' style=' padding: 5px; border: 1px solid black; border-image: none;'><span>{2}</span></td></tr>", docName.ClientSystemDocumentID.ToString(), CurrentViewContext.SelectedTenantID.ToString(), docName.DocumentName);
                }
                else
                {
                    //documentDescription.AppendFormat("<tr><td class='td-one'></td><td class='td-two' style=' padding: 5px; border: 1px solid black; border-image: none;'><a onclick=DownloadForm('../ComplianceOperations/UserControl/DoccumentDownload.aspx?clientSystemDocumentId={0}&tenantId={1}');>{2}</a></td></tr>", docName.Key.ToString(), CurrentViewContext.SelectedTenantID.ToString(), docName.Value);
                    documentDescription.AppendFormat("<tr><td class='td-one'></td><td class='td-two' style=' padding: 5px; border: 1px solid black; border-image: none;'><a href='../ComplianceOperations/UserControl/DoccumentDownload.aspx?clientSystemDocumentId={0}&tenantId={1}');>{2}</a></td></tr>", docName.ClientSystemDocumentID.ToString(), CurrentViewContext.SelectedTenantID.ToString(), docName.DocumentName);
                }
            }
            if (count == AppConsts.NONE)
            {
                documentDescription.AppendFormat("<tr><td class='td-one'></td><td class='td-two' style=' padding: 5px; border: 1px solid black; border-image: none;'>{0}</td></tr>", "No document(s) to display.");
            }
            documentDescription.AppendFormat("</table> </div> </div> </div>");

            //for viewing documents attched onsites.
            Int32 docsCount = AppConsts.NONE;
            List<ContractSite> lstContractSite = Presenter.GetContractsites(contractId);
            documentDescription.Append(@"<div style='padding:10px;background-color:#f0f0f0;'> ");
            documentDescription.Append(@"<div style='margin-bottom:10px'><div style='font-size:11px;font-weight:bold;float:left;margin-right:3px;'>Attached to </div><div >Sites:</div>");
            documentDescription.Append(@"<div style='overflow:auto;max-height: 150px;'>");
            documentDescription.Append(@"<table border='0' cellpadding='0' id='documentDescription' cellspacing='0' '>");
            documentDescription.AppendFormat("<tr style='width:100%;'><td class='td-two' style='padding: 5px; border: 1px solid black; border-image: none;width:290px;'><b>Document Name</b></td><td class='td-three' style='padding: 5px; border: 1px solid black; border-image: none;width:110px;'><b>Site Name</b></td></tr>");
            foreach (var contractSite in lstContractSite)
            {

                var lstSitedocs = contractSite.SiteDocumentMappings.Where(cond => !cond.SDM_IsDeleted).ToList();
                docsCount = docsCount + lstSitedocs.Count;
                foreach (var docName in lstSitedocs)
                {
                    if (docName.ClientSystemDocument.CSD_DocumentPath.IsNullOrEmpty())
                    {
                        documentDescription.AppendFormat("<tr><td class='td-two' style=' padding: 5px; border: 1px solid black; border-image: none;'><span>{2}</span></td><td class='td-two' style=' padding: 5px; border: 1px solid black; border-image: none;'><span>{3}</span></td></tr>",
                                                                        docName.SDM_CSDID.ToString(), CurrentViewContext.SelectedTenantID.ToString(), docName.SDM_DocumentName, contractSite.CS_Name);
                    }
                    else
                    {
                        documentDescription.AppendFormat("<tr><td class='td-two' style=' padding: 5px; border: 1px solid black; border-image: none;'><a href='../ComplianceOperations/UserControl/DoccumentDownload.aspx?clientSystemDocumentId={0}&tenantId={1}');>{2}</a></td><td class='td-three' style=' padding: 5px; border: 1px solid black; border-image: none;'><span>{3}</span></td></tr>",
                                                        docName.SDM_CSDID.ToString(), CurrentViewContext.SelectedTenantID.ToString(), docName.SDM_DocumentName, contractSite.CS_Name);
                    }
                }
            }
            if (docsCount == AppConsts.NONE)
            {
                documentDescription.AppendFormat("<tr><td class='td-two' style=' padding: 5px; border: 1px solid black; border-image: none;'>{0}</td><td class='td-three' style=' padding: 5px; border: 1px solid black; border-image: none;'></td></tr>", "No document(s) to display.");
            }
            documentDescription.AppendFormat("</table>  </div></div></div>");
            hdnDocHtml.Value = documentDescription.ToString();
            if (CurrentViewContext.IsReadOnlyPermission)
            {

                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "OpenPopUpViewDocument(true);", true);
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "OpenPopUpViewDocument(false);", true);
            }


        }


        private void ShowSiteDocumentPopUp(String siteName)
        {
            StringBuilder documentDescription = new StringBuilder();
            Int32 count = CurrentViewContext.SiteDocumentContractList.IsNull() ? AppConsts.NONE : CurrentViewContext.SiteDocumentContractList.Count;
            documentDescription.Append(@"<div style='padding:10px;background-color:#f0f0f0;overflow:auto;' ");
            documentDescription.Append(@"<div style='margin-bottom:10px'><div style='font-size:11px;font-weight:bold;float:left;margin-right:3px;'>Attached to: </div><div >" + siteName + "</div>");

            documentDescription.Append(@"<table border='0' cellpadding='0' id='documentDescription' cellspacing='0'>");
            documentDescription.AppendFormat("<tr><td class='td-one'></td><td class='td-two' style='padding: 5px; border: 1px solid black; border-image: none;width:100%;'><b>Document Name</b></td></tr>");
            foreach (var docName in CurrentViewContext.SiteDocumentContractList)
            {
                if (docName.DocPath.IsNullOrEmpty())
                {
                    documentDescription.AppendFormat("<tr><td class='td-one'></td><td class='td-two' style=' padding: 5px; border: 1px solid black; border-image: none;'><span>{2}</span></td></tr>", docName.ClientSystemDocumentID.ToString(), CurrentViewContext.SelectedTenantID.ToString(), docName.DocumentName);
                }
                else
                {
                    //documentDescription.AppendFormat("<tr><td class='td-one'></td><td class='td-two' style=' padding: 5px; border: 1px solid black; border-image: none;'><a onclick=DownloadForm('../ComplianceOperations/UserControl/DoccumentDownload.aspx?clientSystemDocumentId={0}&tenantId={1}');>{2}</a></td></tr>", docName.Key.ToString(), CurrentViewContext.SelectedTenantID.ToString(), docName.Value);
                    documentDescription.AppendFormat("<tr><td class='td-one'></td><td class='td-two' style=' padding: 5px; border: 1px solid black; border-image: none;'><a href='../ComplianceOperations/UserControl/DoccumentDownload.aspx?clientSystemDocumentId={0}&tenantId={1}');>{2}</a></td></tr>", docName.ClientSystemDocumentID.ToString(), CurrentViewContext.SelectedTenantID.ToString(), docName.DocumentName);
                }
            }
            if (count == AppConsts.NONE)
            {
                documentDescription.AppendFormat("<tr><td class='td-one'></td><td class='td-two' style=' padding: 5px; border: 1px solid black; border-image: none;'>{0}</td></tr>", "No document(s) to display.");
            }
            documentDescription.AppendFormat("</table> </div>");
            hdnDocHtml.Value = documentDescription.ToString();
            if (CurrentViewContext.IsReadOnlyPermission)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "OpenPopUpSiteViewDocument(true);", true);
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "OpenPopUpSiteViewDocument(false);", true);
            }

        }

        private List<ContactContract> GetSiteContactList(Boolean isIncludeDeleted = false)
        {
            List<ContactContract> list = null;
            if (!(ViewState[_viewStateSiteContacts] as List<ContactContract>).IsNullOrEmpty())
            {
                list = (ViewState[_viewStateSiteContacts] as List<ContactContract>).Where(cs => isIncludeDeleted || cs.IsTempDeleted == false).ToList();
            }
            else
            {
                list = new List<ContactContract>();
            }
            return list;
        }

        private void BindSiteContacts(GridItemEventArgs e)
        {
            var _tempCount = 0;
            var grdSiteContacts = (e.Item.FindControl("grdSiteContacts") as WclGrid);
            Presenter.GetSitesContacts();
            foreach (var contact in CurrentViewContext.lstSiteContacts)
            {
                _tempCount += 1;
                contact.TempContactId = _tempCount;
            }

            ViewState[_viewStateSiteContacts] = CurrentViewContext.lstSiteContacts;
            grdSiteContacts.DataSource = CurrentViewContext.lstSiteContacts;
            grdSiteContacts.DataBind();
        }

        #endregion

        #region UAT - 1666 (Code to disable grid controls)

        private void DisableContractGridControls(GridItemEventArgs e)
        {
            try
            {
                RadComboBox cmbTenantGridMode = (e.Item.FindControl("cmbTenantGridMode") as WclComboBox);
                if (cmbTenantGridMode.IsNotNull())
                {
                    cmbTenantGridMode.Enabled = false;
                }
                WclTextBox txtAffiliation = (e.Item.FindControl("txtAffiliation") as WclTextBox);
                if (txtAffiliation.IsNotNull())
                {
                    txtAffiliation.Enabled = false;
                }
                WclDatePicker dpStartDate = (e.Item.FindControl("dpStartDate") as WclDatePicker);
                if (dpStartDate.IsNotNull())
                {
                    dpStartDate.Enabled = false;
                }
                WclDatePicker dpEdate = (e.Item.FindControl("dpEdate") as WclDatePicker);
                if (dpEdate.IsNotNull())
                {
                    dpEdate.Enabled = false;
                }
                WclTextBox txtDaysBefore = (e.Item.FindControl("txtDaysBefore") as WclTextBox);
                if (txtDaysBefore.IsNotNull())
                {
                    txtDaysBefore.Enabled = false;
                }
                WclNumericTextBox txtFrequencyAfter = (e.Item.FindControl("txtFrequencyAfter") as WclNumericTextBox);
                if (txtFrequencyAfter.IsNotNull())
                {
                    txtFrequencyAfter.Enabled = false;
                }
                WclComboBox cmbRenewalType = (e.Item.FindControl("cmbRenewalType") as WclComboBox);
                if (cmbRenewalType.IsNotNull())
                {
                    cmbRenewalType.Enabled = false;
                }
                WclNumericTextBox txtTerms = (WclNumericTextBox)e.Item.FindControl("txtTerms");
                if (txtTerms.IsNotNull())
                {
                    txtTerms.Enabled = false;
                }
                WclDatePicker dpExpirationDate = (WclDatePicker)e.Item.FindControl("dpExpirationDate");
                if (dpExpirationDate.IsNotNull())
                {
                    dpExpirationDate.Enabled = false;
                }
                WclTextBox txtNotes = (e.Item.FindControl("txtNotes") as WclTextBox);
                if (txtNotes.IsNotNull())
                {
                    txtNotes.Enabled = false;
                }
                HtmlAnchor lnkInstitutionHierarchyGridMode = (e.Item.FindControl("lnkInstitutionHierarchyGridMode") as HtmlAnchor);
                if (lnkInstitutionHierarchyGridMode.IsNotNull())
                {
                    lnkInstitutionHierarchyGridMode.Visible = false;
                }

                WclComboBox cmbContractType = (e.Item.FindControl("cmbContractType") as WclComboBox);
                if (cmbContractType.IsNotNull())
                {
                    cmbContractType.EnableCheckAllItemsCheckBox = false;
                    foreach (RadComboBoxItem item in cmbContractType.Items)
                    {
                        item.Enabled = false;
                    }
                    //cmbContractType.Enabled = false;
                }

                CommandBar fsucCmdBarPermission = (e.Item.FindControl("fsucCmdBarPermission") as CommandBar);
                if (fsucCmdBarPermission.IsNotNull())
                {
                    fsucCmdBarPermission.SaveButton.Visible = false;
                    // fsucCmdBarPermission.Visible= false;
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

        private void DisableSiteGridControls(GridItemEventArgs e)
        {
            try
            {
                WclTextBox txtSiteName = (e.Item.FindControl("txtSiteName") as WclTextBox);
                if (txtSiteName.IsNotNull())
                {
                    txtSiteName.Enabled = false;
                }
                WclTextBox txtSiteAddress = (e.Item.FindControl("txtSiteAddress") as WclTextBox);
                if (txtSiteAddress.IsNotNull())
                {
                    txtSiteAddress.Enabled = false;
                }

                //WclComboBox cmbDocType = (e.Item.FindControl("cmbDocType") as WclComboBox);
                //if (cmbDocType.IsNotNull())
                //{
                //    cmbDocType.Enabled = false;
                //}
                //WclTextBox txtDocumentName = (e.Item.FindControl("txtDocumentName") as WclTextBox);
                //if (txtDocumentName.IsNotNull())
                //{
                //    txtDocumentName.Enabled = false;
                //}
                //WclAsyncUpload uploadControl = (e.Item.FindControl("uploadControl") as WclAsyncUpload);
                //if (uploadControl.IsNotNull())
                //{
                //    uploadControl.Enabled = false;
                //}
                //WclButton chkCreateVersion = (e.Item.FindControl("chkCreateVersion") as WclButton);
                //if (chkCreateVersion.IsNotNull())
                //{
                //    chkCreateVersion.Enabled = false;
                //}

                WclDatePicker dpSiteStartDate = (e.Item.FindControl("dpSiteStartDate") as WclDatePicker);
                if (dpSiteStartDate.IsNotNull())
                {
                    dpSiteStartDate.Enabled = false;
                }
                WclDatePicker dpSiteEdate = (e.Item.FindControl("dpSiteEdate") as WclDatePicker);
                if (dpSiteEdate.IsNotNull())
                {
                    dpSiteEdate.Enabled = false;
                }
                WclTextBox txtSiteDaysBefore = (e.Item.FindControl("txtSiteDaysBefore") as WclTextBox);
                if (txtSiteDaysBefore.IsNotNull())
                {
                    txtSiteDaysBefore.Enabled = false;
                }

                WclComboBox cmbSiteRenewalType = (e.Item.FindControl("cmbSiteRenewalType") as WclComboBox);
                if (cmbSiteRenewalType.IsNotNull())
                {
                    cmbSiteRenewalType.Enabled = false;
                }
                WclTextBox txtSiteNotes = (e.Item.FindControl("txtSiteNotes") as WclTextBox);
                if (txtSiteNotes.IsNotNull())
                {
                    txtSiteNotes.Enabled = false;
                }
                WclComboBox cmbContractType = (e.Item.FindControl("cmbContractType") as WclComboBox);
                if (cmbContractType.IsNotNull())
                {
                    foreach (RadComboBoxItem item in cmbContractType.Items)
                    {
                        item.Enabled = false;
                    }
                    //cmbContractType.Enabled = false;
                }
                WclDatePicker dpSiteExpirationDate = (e.Item.FindControl("dpSiteExpirationDate") as WclDatePicker);
                if (dpSiteExpirationDate.IsNotNull())
                {
                    dpSiteExpirationDate.Enabled = false;
                }
                WclNumericTextBox txtSiteTerms = (e.Item.FindControl("txtSiteTerms") as WclNumericTextBox);
                if (txtSiteTerms.IsNotNull())
                {
                    txtSiteTerms.Enabled = false;
                }
                WclNumericTextBox txtSiteFrequencyAfter = (e.Item.FindControl("txtSiteFrequencyAfter") as WclNumericTextBox);
                if (txtSiteFrequencyAfter.IsNotNull())
                {
                    txtSiteFrequencyAfter.Enabled = false;
                }

                CommandBar fsucCmdBarSite = (e.Item.FindControl("fsucCmdBarSite") as CommandBar);
                if (fsucCmdBarSite.IsNotNull())
                {
                    fsucCmdBarSite.Visible = false;
                }
                CommandBar fsucCmdBarPermission = (e.Item.Parent.Parent.Parent.Parent.Parent.Parent.Parent.FindControl("fsucCmdBarPermission") as CommandBar);
                if (fsucCmdBarPermission.IsNotNull())
                {
                    fsucCmdBarPermission.SaveButton.Visible = false;
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

        private void DisableDocumentGridControls(GridItemEventArgs e)
        {
            try
            {
                RadComboBox cmbDocType = (e.Item.FindControl("cmbDocType") as WclComboBox);
                if (cmbDocType.IsNotNull())
                {
                    cmbDocType.Enabled = false;
                }
                WclTextBox txtDocumentName = (e.Item.FindControl("txtDocumentName") as WclTextBox);
                if (txtDocumentName.IsNotNull())
                {
                    txtDocumentName.Enabled = false;
                }
                WclAsyncUpload uploadControl = (e.Item.FindControl("uploadControl") as WclAsyncUpload);
                if (uploadControl.IsNotNull())
                {
                    uploadControl.Enabled = false;
                }
                WclButton chkCreateVersion = (e.Item.FindControl("chkCreateVersion") as WclButton);
                if (chkCreateVersion.IsNotNull())
                {
                    chkCreateVersion.Enabled = false;
                }

                WclComboBox cmbDocStatus = (e.Item.FindControl("cmbDocStatus") as WclComboBox);
                if (cmbDocStatus.IsNotNull())
                {
                    cmbDocStatus.Enabled = false;
                }

                CommandBar fsucCmdBarSite = (e.Item.FindControl("fsucCmdBarSite") as CommandBar);
                if (fsucCmdBarSite.IsNotNull())
                {
                    fsucCmdBarSite.Visible = false;
                }
                CommandBar fsucCmdBarPermission = (e.Item.Parent.Parent.Parent.Parent.Parent.Parent.Parent.FindControl("fsucCmdBarPermission") as CommandBar);
                if (fsucCmdBarPermission.IsNotNull())
                {
                    fsucCmdBarPermission.SaveButton.Visible = false;
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

        private void DisableContactGridControls(GridItemEventArgs e)
        {
            try
            {
                WclTextBox txtFirstName = (e.Item.FindControl("txtFirstName") as WclTextBox);
                if (txtFirstName.IsNotNull())
                {
                    txtFirstName.Enabled = false;
                }
                WclTextBox txtLastName = (e.Item.FindControl("txtLastName") as WclTextBox);
                if (txtLastName.IsNotNull())
                {
                    txtLastName.Enabled = false;
                }
                WclTextBox txtTitle = (e.Item.FindControl("txtTitle") as WclTextBox);
                if (txtTitle.IsNotNull())
                {
                    txtTitle.Enabled = false;
                }
                WclTextBox txtEmail = (e.Item.FindControl("txtEmail") as WclTextBox);
                if (txtEmail.IsNotNull())
                {
                    txtEmail.Enabled = false;
                }
                WclMaskedTextBox txtPhone = (e.Item.FindControl("txtPhone") as WclMaskedTextBox);
                if (txtPhone.IsNotNull())
                {
                    txtPhone.Enabled = false;
                }
                //UAT-2447
                WclMaskedTextBox txtPhone1 = (e.Item.FindControl("txtPhone1") as WclMaskedTextBox);
                if (txtPhone1.IsNotNull())
                {
                    txtPhone1.Enabled = false;
                }
                WclTextBox txtInternationalPhone = (e.Item.FindControl("txtInternationalPhone") as WclTextBox);
                if (txtInternationalPhone.IsNotNull())
                {
                    txtInternationalPhone.Enabled = false;
                }
                WclTextBox txtInternationalPhone1 = (e.Item.FindControl("txtInternationalPhone1") as WclTextBox);
                if (txtInternationalPhone1.IsNotNull())
                {
                    txtInternationalPhone1.Enabled = false;
                }
                WclCheckBox chkInternationalPhone = (e.Item.FindControl("chkInternationalPhone") as WclCheckBox);
                if (chkInternationalPhone.IsNotNull())
                {
                    chkInternationalPhone.Enabled = false;
                }
                WclCheckBox chkInternationalPhone1 = (e.Item.FindControl("chkInternationalPhone1") as WclCheckBox);
                if (chkInternationalPhone1.IsNotNull())
                {
                    chkInternationalPhone1.Enabled = false;
                }

                CommandBar fsucCmdBarContact = (e.Item.FindControl("fsucCmdBarContact") as CommandBar);
                if (fsucCmdBarContact.IsNotNull())
                {
                    fsucCmdBarContact.Visible = false;
                }
                CommandBar fsucCmdBarPermission = (e.Item.Parent.Parent.Parent.Parent.Parent.Parent.Parent.FindControl("fsucCmdBarPermission") as CommandBar);
                if (fsucCmdBarPermission.IsNotNull())
                {
                    fsucCmdBarPermission.SaveButton.Visible = false;
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

        #region UAT-1665: Agreement Status

        private void BindDocumentStatus(WclComboBox cmb)
        {
            List<lkpContractDocumentStatu> lstDocStatus = new List<lkpContractDocumentStatu>();
            lstDocStatus = Presenter.GetDocumentStatus();
            cmb.DataSource = lstDocStatus;
            cmb.DataTextField = "CDS_Name";
            cmb.DataValueField = "CDS_ID";
            cmb.DataBind();
            cmb.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(AppConsts.COMBOBOX_ITEM_SELECT, AppConsts.ZERO));
        }

        #endregion

        protected void cbExport_SubmitClick(object sender, EventArgs e)
        {
            try
            {
                List<Int32> lstContractIDs = Presenter.GetContractIDs();
                if (!lstContractIDs.IsNullOrEmpty())
                {
                    String csvContractIDs = String.Join(",", lstContractIDs.ToArray());
                    ifrExportDocument.Src = "~/ComplianceOperations/UserControl/DocumentViewer.aspx?ContractIDs=" + csvContractIDs + "&ReportType=" + cmbExport.SelectedValue + "&tenantId=" + CurrentViewContext.SelectedTenantID;
                }
                else
                {
                    base.ShowInfoMessage("No Affiliation/Site to export.");
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

        #region UAT-2447
        private void ShowHidePhoneControlsgrdSiteContacts(Boolean IsInternationalNumber, GridEditFormItem pnlControl)
        {
            if (IsInternationalNumber)
            {
                (pnlControl.FindControl("txtInternationalPhone") as WclTextBox).Style["display"] = "block";
                (pnlControl.FindControl("revTxtMobilePrmyNonMasking") as RegularExpressionValidator).Enabled = true;
                (pnlControl.FindControl("txtPhone") as WclMaskedTextBox).Style["display"] = "none";
            }
            else
            {
                (pnlControl.FindControl("txtInternationalPhone") as WclTextBox).Style["display"] = "none";
                (pnlControl.FindControl("revTxtMobilePrmyNonMasking") as RegularExpressionValidator).Enabled = false;
                (pnlControl.FindControl("txtPhone") as WclMaskedTextBox).Style["display"] = "block";
            }
        }

        private void ShowHidePhoneControlsgrdContacts(Boolean IsInternationalNumber, GridEditFormItem pnlControl)
        {
            if (IsInternationalNumber)
            {
                (pnlControl.FindControl("txtInternationalPhone1") as WclTextBox).Style["display"] = "block";
                (pnlControl.FindControl("revTxtMobilePrmyNonMasking1") as RegularExpressionValidator).Enabled = true;
                (pnlControl.FindControl("txtPhone1") as WclMaskedTextBox).Style["display"] = "none";
            }
            else
            {
                (pnlControl.FindControl("txtInternationalPhone1") as WclTextBox).Style["display"] = "none";
                (pnlControl.FindControl("revTxtMobilePrmyNonMasking1") as RegularExpressionValidator).Enabled = false;
                (pnlControl.FindControl("txtPhone1") as WclMaskedTextBox).Style["display"] = "block";
            }
        }

        protected void chkInternationalPhone_CheckedChanged(object sender, EventArgs e)
        {
            var chkIsMaskingRequired = sender as WclCheckBox;
            ShowHidePhoneControlsgrdSiteContacts(chkIsMaskingRequired.Checked, chkIsMaskingRequired.Parent.NamingContainer as GridEditFormItem);
        }

        protected void chkInternationalPhone1_CheckedChanged(object sender, EventArgs e)
        {
            var chkIsMaskingRequired = sender as WclCheckBox;
            ShowHidePhoneControlsgrdContacts(chkIsMaskingRequired.Checked, chkIsMaskingRequired.Parent.NamingContainer as GridEditFormItem);
        }
        #endregion
    }
}