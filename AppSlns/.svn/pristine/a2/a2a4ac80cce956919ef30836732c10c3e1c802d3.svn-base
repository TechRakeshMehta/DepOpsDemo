using System;
using System.Linq;
using System.Collections.Generic;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using Telerik.Web.UI;
using System.Web.UI.WebControls;
using INTERSOFT.WEB.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Web.Configuration;
using System.IO;
using INTSOF.Utils;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.ClientContact;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using INTSOF.ServiceDataContracts.Modules.Common;
using System.Web.UI;
using Entity.ClientEntity;
using CoreWeb.AgencyHierarchy.Views;
using INTSOF.UI.Contract.CommonControls;



namespace CoreWeb.ClinicalRotation.Views
{
    public partial class ManageRotation : BaseUserControl, IManageRotationView
    {
        #region Private Variables
        private ManageRotationPresenter _presenter = new ManageRotationPresenter();
        private String _viewType;
        private ClinicalRotationDetailContract _viewContract = null;
        private ClinicalRotationDetailContract _searchContract = null;
        private Int32 tenantId = 0;
        private Boolean IsFieldsUpdated = false;
        private Boolean IsAgencyChanged = false;
        //UAT-2034
        private Int32 _agencyID = 0;
        //UAT-2696
        private List<String> _lstCodeForColumnConfig = new List<String>();
        #endregion
        #region Public Variables
        //UAT-3032
        public Boolean IsPreferredDefaultTenantSelected = false;
        #endregion
        #region Properties
        #region Public Properties
        public ManageRotationPresenter Presenter
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

        public IManageRotationView CurrentViewContext
        {
            get
            {
                return this;
            }
        }


        /// <summary>
        /// Gets the view contract.
        /// </summary>
        /// <remarks></remarks>
        ClinicalRotationDetailContract IManageRotationView.ViewContract
        {
            get
            {
                if (_viewContract.IsNull())
                {
                    _viewContract = new ClinicalRotationDetailContract();
                }
                return _viewContract;
            }
        }

        List<ClinicalRotationDetailContract> IManageRotationView.ClinicalRotationData
        {
            get;
            set;
        }

        Boolean IManageRotationView.IsReset
        {
            get;
            set;
        }

        Int32 IManageRotationView.TenantID
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

        //Represnts Selected Institution ID
        Int32 IManageRotationView.SelectedTenantID
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
                }
                else
                {
                    ddlTenantName.SelectedIndex = value;
                }
            }
        }

        /// <summary>
        /// Returns the current logged-in user ID.
        /// </summary>
        Int32 IManageRotationView.CurrentLoggedInUserId
        {
            get
            {
                return base.CurrentUserId;
            }
        }

        List<TenantDetailContract> IManageRotationView.lstTenant
        {
            get
            {
                if (!ViewState["lstTenant"].IsNull())
                {
                    return ViewState["lstTenant"] as List<TenantDetailContract>;
                }

                return new List<TenantDetailContract>();
            }
            set
            {
                ViewState["lstTenant"] = value;
            }
        }

        bool IManageRotationView.IsAdminLoggedIn
        {
            get;
            set;
        }

        //Represents the list of all Agencies
        List<AgencyDetailContract> IManageRotationView.lstAgency
        {
            get
            {
                if (!ViewState["lstAgency"].IsNull())
                {
                    return ViewState["lstAgency"] as List<AgencyDetailContract>;
                }

                return new List<AgencyDetailContract>();
            }
            set
            {
                ViewState["lstAgency"] = value;
            }
        }

        //Represents Selected AgencyID
        String IManageRotationView.SelectedAgencyIDs
        {
            get
            {
                AgencyhierarchyCollection agencyhierarchyCollection = ucAgencyHierarchyMultipleToSearchRotation.GetAgencyHierarchyCollection();
                string agencyIDs = string.Empty;

                if (!agencyhierarchyCollection.IsNullOrEmpty() && !agencyhierarchyCollection.agencyhierarchy.IsNullOrEmpty())
                    agencyIDs = string.Join(",", agencyhierarchyCollection.agencyhierarchy.Select(d => d.AgencyID).Distinct().ToList());

                return agencyIDs;
            }
            set
            {
                ucAgencyHierarchyMultipleToSearchRotation.SelectedAgecnyIds = value;
            }
        }

        //Represents ErrorMessage
        String IManageRotationView.ErrorMessage
        {

            get;
            set;
        }

        //Represents SuccessMessage
        String IManageRotationView.SuccessMessage
        {
            get;
            set;
        }

        List<ClientContactContract> IManageRotationView.ClientContactList
        {
            get
            {
                if (!ViewState["ClientContactList"].IsNull())
                {
                    return ViewState["ClientContactList"] as List<ClientContactContract>;
                }

                return new List<ClientContactContract>();
            }
            set
            {
                ViewState["ClientContactList"] = value;
            }
        }

        List<WeekDayContract> IManageRotationView.WeekDayList
        {
            get
            {
                if (!ViewState["WeekDayList"].IsNull())
                {
                    return ViewState["WeekDayList"] as List<WeekDayContract>;
                }

                return new List<WeekDayContract>();
            }
            set
            {
                ViewState["WeekDayList"] = value;
            }
        }

        /// <summary>
        /// Gets the view contract.
        /// </summary>
        /// <remarks></remarks>
        ClinicalRotationDetailContract IManageRotationView.SearchContract
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
                SetSearchParameters();
            }
        }

        /// <summary>
        /// Get and set custom attribute list of type hierarchy.
        /// </summary>
        List<CustomAttribteContract> IManageRotationView.GetCustomAttributeList
        {
            get;
            set;
        }

        List<CustomAttribteContract> IManageRotationView.SaveCustomAttributeList
        {
            get;
            set;
        }

        Int32? IManageRotationView.RotationID
        {
            get
            {
                if (!ViewState["RotationID"].IsNull())
                {
                    return (Int32?)ViewState["RotationID"];
                }
                return null;
            }
            set
            {
                ViewState["WeekDayList"] = value;
            }
        }

        List<int> IManageRotationView.SelectedClientContacts
        {
            get;
            set;
        }

        List<AgencyDetailContract> IManageRotationView.lstAgencyForAddForm
        {
            get;
            set;
        }


        Int32 IManageRotationView.SelectedAgencyIDForAddForm
        {
            get;
            set;
        }

        Int32 IManageRotationView.SelectedTenantIDForAddForm
        {
            get
            {
                if (!ViewState["SelectedTenantIDForAddForm"].IsNull())
                {
                    return (Int32)ViewState["SelectedTenantIDForAddForm"];
                }
                return AppConsts.NONE;
            }
            set
            {
                ViewState["SelectedTenantIDForAddForm"] = value;
            }
        }

        List<ClientContactContract> IManageRotationView.ClientContactListForAddForm
        {
            get;
            set;
        }

        List<WeekDayContract> IManageRotationView.WeekDayListForAddForm
        {
            get;
            set;
        }

        String IManageRotationView.HierarchyNode
        {
            get;
            set;
        }

        List<lkpSharedUserRotationReviewStatu> IManageRotationView.lstRotationReviewStatus
        {
            get
            {
                if (!ViewState["RotationReviewStatusList"].IsNull())
                {
                    return ViewState["RotationReviewStatusList"] as List<lkpSharedUserRotationReviewStatu>;
                }

                return new List<lkpSharedUserRotationReviewStatu>();
            }
            set
            {
                ViewState["RotationReviewStatusList"] = value;
            }
        }

        RotationsMappedToAgenciesContract IManageRotationView.RotationsMappedToAgenciesData
        {
            get;
            set;
        }

        //UAT-1778
        public String CustomDataXML
        {
            get
            {
                //SharedUserCustomAttributeForm caCustomAttributesID = Page.FindServerControlRecursively("caCustomAttributesID") as SharedUserCustomAttributeForm;
                if (caCustomAttributesID.IsNotNull())
                    return caCustomAttributesID.GetCustomDataXML();
                else
                    return null;
            }
        }


        /// <summary>
        /// Granular Permissions of the logged-in user.
        /// </summary>
        Dictionary<String, String> IManageRotationView.dicGranularPermissions
        {
            get;
            set;
        }

        #region UAT-2424
        public List<ClinicalRotationDetailContract> lstClinicalRotation
        {
            get
            {
                if (!ViewState["lstClinicalRotation"].IsNull())
                {
                    return ViewState["lstClinicalRotation"] as List<ClinicalRotationDetailContract>;
                }

                return new List<ClinicalRotationDetailContract>();
            }
            set
            {
                ViewState["lstClinicalRotation"] = value;
            }
        }

        Int32 IManageRotationView.SelectedRotationIDForCloning
        {
            get
            {
                if (!ViewState["SelectedRotationIDForCloning"].IsNull())
                {
                    return (Int32)ViewState["SelectedRotationIDForCloning"];
                }
                return AppConsts.NONE;
            }
            set
            {
                ViewState["SelectedRotationIDForCloning"] = value;
            }
        }

        public ClinicalRotationDetailContract CloneContract
        {
            get
            {
                if (!ViewState["CloneContract"].IsNull())
                {
                    return ViewState["CloneContract"] as ClinicalRotationDetailContract;
                }

                return new ClinicalRotationDetailContract();
            }
            set
            {
                ViewState["CloneContract"] = value;
            }
        }
        #endregion

        #region UAT-2034:Phase 4 (16): Manage Rotation screen updates
        Dictionary<Int32, Int32> IManageRotationView.DicOfSelectedRotation
        {
            get
            {
                if (!ViewState["DicOfSelectedRotation"].IsNull())
                {
                    return (Dictionary<Int32, Int32>)ViewState["DicOfSelectedRotation"];
                }
                return new Dictionary<Int32, Int32>();
            }
            set
            {
                ViewState["DicOfSelectedRotation"] = value;
            }
        }
        #endregion

        #region UAT-2666
        List<RotationFieldUpdatedByAgencyContract> IManageRotationView.lstRotationFieldUpdaeByAgency
        {
            get
            {
                if (!ViewState["lstRotationFieldUpdaeByAgency"].IsNull())
                {
                    return ViewState["lstRotationFieldUpdaeByAgency"] as List<RotationFieldUpdatedByAgencyContract>;
                }

                return new List<RotationFieldUpdatedByAgencyContract>();
            }
            set
            {
                ViewState["lstRotationFieldUpdaeByAgency"] = value;
            }
        }
        #endregion

        #region Custom paging parameters
        public Int32 CurrentPageIndex
        {
            get
            {
                return grdRotations.MasterTableView.CurrentPageIndex + 1;
            }
            set
            {
                grdRotations.MasterTableView.CurrentPageIndex = value - 1;
            }
        }

        public int PageSize
        {
            get
            {
                return grdRotations.PageSize;
            }
            set
            {
                grdRotations.MasterTableView.PageSize = value;
                grdRotations.PageSize = value;
            }
        }

        public int VirtualRecordCount
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
                grdRotations.VirtualItemCount = value;
                grdRotations.MasterTableView.VirtualItemCount = value;
            }
        }

        public CustomPagingArgsContract GridCustomPaging
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


        #region UAT-2545

        public List<lkpArchiveState> lstArchiveState
        {
            set
            {
                rbArchiveStatus.DataSource = value.OrderBy(x => x.AS_Code);
                rbArchiveStatus.DataBind();
                rbArchiveStatus.SelectedValue = ArchiveState.Active.GetStringValue();//value.Where(x => x.AS_Code == lkpArchivalState.NonArchived.GetStringValue()).Select(x=>x.AS_Code).ToString();
            }
        }
        public List<String> SelectedArchiveStatusCode
        {
            get
            {
                if (!rbArchiveStatus.SelectedValue.IsNullOrEmpty())
                {
                    List<String> selectedCodes = new List<String>();
                    if (rbArchiveStatus.SelectedValue.Equals(ArchiveState.All.GetStringValue()))
                    {
                        return null;
                    }
                    else
                    {
                        selectedCodes.Add(rbArchiveStatus.SelectedValue);
                    }
                    return selectedCodes;
                }
                else
                    return null;
            }
            set
            {
                rbArchiveStatus.SelectedValue = value.FirstOrDefault();
            }
        }
        #endregion

        #region UAT UAT-268

        String IManageRotationView.SelectedAgencyIDsForAddForm
        {
            get;
            set;
        }

        String IManageRotationView.AgencyIDs
        {
            get;
            set;
        }

        #endregion

        //UAT-2696
        Int32 IManageRotationView.OrganizationUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        /*UAT-2979*/
        String IManageRotationView.DeptProgramMappingID
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
        /*End UAT-2979*/

        /*UAT - 3032*/

        Int32 IManageRotationView.PreferredSelectedTenantID
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

        #region UAT-3121
        Boolean IManageRotationView.IsApplicantPkgNotAssignedThroughCloning
        {
            get
            {
                if (!ViewState["IsApplicantPkgNotAssignedThroughCloning"].IsNull())
                {
                    return Convert.ToBoolean(ViewState["IsApplicantPkgNotAssignedThroughCloning"]);
                }
                return false;
            }
            set
            {
                ViewState["IsApplicantPkgNotAssignedThroughCloning"] = value;
            }
        }
        Boolean IManageRotationView.IsInstructorPkgNotAssignedThroughCloning
        {
            get
            {
                if (!ViewState["IsInstructorPkgNotAssignedThroughCloning"].IsNull())
                {
                    return Convert.ToBoolean(ViewState["IsInstructorPkgNotAssignedThroughCloning"]);
                }
                return false;
            }
            set
            {
                ViewState["IsInstructorPkgNotAssignedThroughCloning"] = value;
            }
        }
        #endregion

        #endregion
        #endregion

        #region Page Events

        /// <summary>
        /// Page OnInit Event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                base.Title = "Manage Rotations";
                base.SetPageTitle("Manage Rotations");
                //UAT-2696
                _lstCodeForColumnConfig.Add(Screen.grdAdminManageRotations.GetStringValue());
                ColumnsConfiguration.CurrentViewContext.CurrentLoggedInUserID = CurrentViewContext.CurrentLoggedInUserId;
                ColumnsConfiguration.CurrentViewContext.OrganisationUserID = CurrentViewContext.OrganizationUserId;
                ColumnsConfiguration.CurrentViewContext.lstGridCode = _lstCodeForColumnConfig;
                List<PreHiddenColumnsContract> lstpreHiddenColumnsContract = new List<PreHiddenColumnsContract>();
                lstpreHiddenColumnsContract.Add(new PreHiddenColumnsContract { GridCode = Screen.grdAdminManageRotations.GetStringValue(), PredefinedHiddenColumn = "CustomAttributesTemp" });
                grdRotations.Attributes["PreHiddenColumns"] = "CustomAttributesTemp";
                //grdRotations.MasterTableView.GetColumn("CustomAttributesTemp").Visible = true;
                //if (Presenter.IsCustomAttributeChecked(CurrentViewContext.OrganizationUserId, AppConsts.UNIQUE_NAME_CUSTOM_ATTRIBUTE_GRD, Screen.grdAdminManageRotations.GetStringValue()))
                //{
                //    grdRotations.MasterTableView.GetColumn("CustomAttributesGrd").Display = true;
                //    //grdRotations.MasterTableView.GetColumn("CustomAttributesTemp").Display = true;
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

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                (fsucCmdBarButton as CoreWeb.Shell.Views.CommandBar).SaveButton.ToolTip = "Click to search Rotations as per the criteria entered above";
                (fsucCmdBarButton as CoreWeb.Shell.Views.CommandBar).CancelButton.ToolTip = "Click to cancel. Any data entered will not be saved";
                (fsucCmdBarButton as CoreWeb.Shell.Views.CommandBar).SubmitButton.ToolTip = "Click to remove all values entered in the search criteria above";
                if (!Session["SourceScreen"].IsNullOrEmpty())//UAT-2313
                {
                    Session["SourceScreen"] = null;
                }
                if (!this.IsPostBack)
                {
                    if (Request.QueryString["args"].IsNull())
                    {
                        cmdAssignBtns.Visible = false;
                        grdRotations.Visible = false;
                    }
                    Presenter.OnViewInitialized();
                    BindTenant();
                    CaptureQuerystringParameters();
                    /*UAT-3032*/
                    GetPreferredSelectedTenant();
                    /*END UAT-3032*/
                    BindControls();
                    //fix for: Radiobuttonlist(Rotation Archive Status) value not retained after coming back from Detail screen.
                    BindArchiveFilter();
                    GetSessionValues();
                    fsucCmdBarButton.SaveButton.ValidationGroup = "grpFormSearch";
                    //BindArchiveFilter();
                }
                Presenter.OnViewLoaded();


                //UAT-1778
                CurrentViewContext.SelectedTenantIDForAddForm = CurrentViewContext.SelectedTenantID;
                if (CurrentViewContext.SelectedTenantIDForAddForm > AppConsts.NONE)
                {
                    caCustomAttributesID.IsSearchTypeControl = true;
                    caCustomAttributesID.TenantId = CurrentViewContext.SelectedTenantID;
                    caCustomAttributesID.TypeCode = CustomAttributeUseTypeContext.Hierarchy.GetStringValue();
                    caCustomAttributesID.DataSourceModeType = DataSourceMode.Ids;
                    caCustomAttributesID.Title = "Other Details";
                    caCustomAttributesID.ControlDisplayMode = DisplayMode.Controls;
                    caCustomAttributesID.CurrentLoggedInUserId = base.CurrentUserId;
                    caCustomAttributesID.ValidationGroup = "grpFormSubmitSearchType";
                    caCustomAttributesID.IsReadOnly = false;

                    Presenter.GetCustomAttributeList(null);
                    if (!CurrentViewContext.GetCustomAttributeList.IsNullOrEmpty())
                        caCustomAttributesID.lstTypeCustomAttributes = CurrentViewContext.GetCustomAttributeList;
                    if (caCustomAttributesID.IsNotNull() && _searchContract.IsNotNull())
                        caCustomAttributesID.previousValues = _searchContract.CustomAttributes;

                    //CreateCustomAttributes();
                    //SharedUserCustomAttributeForm caCustomAttributesID = Page.FindServerControlRecursively("caCustomAttributesID") as SharedUserCustomAttributeForm;
                    //if (caCustomAttributesID.IsNotNull() && _searchContract.IsNotNull())
                    //    caCustomAttributesID.previousValues = _searchContract.CustomAttributes;

                }

                ucAgencyHierarchyMultipleToSearchRotation.TenantId = CurrentViewContext.SelectedTenantID == AppConsts.NONE ? AppConsts.MINUS_ONE : CurrentViewContext.SelectedTenantID; //UAT-2600                
                ucAgencyHierarchyMultipleToSearchRotation.AgencyHierarchyNodeSelection = true;


                /*UAT-2979: Add the Institution Hierarchy link as a way to filter out existing rotations on the Manage Rotation Search?*/
                lblinstituteHierarchy.Text = hdnHierarchyLabel.Value.HtmlEncode();
                /*END UAT-2979*/
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
        protected void grdRotations_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                GridCustomPaging.CurrentPageIndex = CurrentPageIndex;
                GridCustomPaging.PageSize = PageSize;
                if (!CurrentViewContext.IsAdminLoggedIn && CurrentViewContext.IsReset)
                {
                    grdRotations.CurrentPageIndex = 0;
                    grdRotations.MasterTableView.CurrentPageIndex = 0;
                    CurrentViewContext.VirtualRecordCount = 0;
                    CurrentViewContext.ClinicalRotationData = new List<ClinicalRotationDetailContract>();
                   
                }
                else
                    Presenter.GetRotationDetail();

                grdRotations.DataSource = CurrentViewContext.ClinicalRotationData;
                //Tenant not selected,grid is hidden 
                //if (CurrentViewContext.SelectedTenantID == AppConsts.NONE || CurrentViewContext.SelectedAgencyID == AppConsts.NONE)
                //{
                //    grdRotations.MasterTableView.IsItemInserted = false;
                //    grdRotations.MasterTableView.ClearEditItems();
                //    dvGrdRotations.Visible = false;
                //}
                //if (CurrentViewContext.SelectedTenantID > 0 && CurrentViewContext.SelectedAgencyID > AppConsts.NONE)
                //{
                //    dvGrdRotations.Visible = true;
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

        protected void grdRotations_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "ViewDetail")
                {
                    //Reset rotation detail session
                    if (!Session[AppConsts.ROTATION_DETAIL_SESSION_KEY].IsNullOrEmpty())
                    {
                        Session[AppConsts.ROTATION_DETAIL_SESSION_KEY] = null;
                    }
                    SetSessionValues();
                    String ID = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["RotationID"].ToString();
                    //UAT-3053 : As an admin, after creating a rotation I should be directed to the rotation details screen of that rotation.
                    String _agencyId = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["AgencyID"].ToString();
                    String TenantId = Convert.ToString(CurrentViewContext.SelectedTenantID);

                    //UAT-3041
                    String IsClientAdminEditPermission = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["IsEditableByClientAdmin"].ToString();
                    String IsAgencyUserEditPermission = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["IsEditableByAgencyUser"].ToString();

                    RedirectToDetail(ID, _agencyId, TenantId, false, IsClientAdminEditPermission, IsAgencyUserEditPermission);

                }

                #region Export functionality
                //UAT-1778: Addition of "custom attribute" column to the manage rotation grid
                // Implemented the export functionlaity for exporting custom attribute columns accordingly

                if (e.CommandName.IsNullOrEmpty())
                {
                    if (e.Item is GridCommandItem)
                    {
                        WclComboBox cmdExportColumns = e.Item.FindControl("cmdExportColumns") as WclComboBox;
                        WclComboBox cmbExportFormat = e.Item.FindControl("cmbExportFormat") as WclComboBox;
                        if (cmbExportFormat.IsNotNull() && (cmbExportFormat.SelectedValue == "Csv" || cmbExportFormat.SelectedValue == "Pdf" || cmbExportFormat.SelectedValue == "Excel"))
                        {
                            //checking whether CustomAttributesGrd is checked in column configuration or not.
                            if (!Presenter.IsCustomAttributeChecked(CurrentViewContext.OrganizationUserId, AppConsts.UNIQUE_NAME_CUSTOM_ATTRIBUTE_GRD, Screen.grdAdminManageRotations.GetStringValue()))
                            {
                                grdRotations.MasterTableView.GetColumn("CustomAttributesGrd").Display = false;
                                grdRotations.MasterTableView.GetColumn("CustomAttributesGrd").Visible = false;
                                grdRotations.MasterTableView.GetColumn("CustomAttributesTemp").Display = false;
                                grdRotations.MasterTableView.GetColumn("CustomAttributesTemp").Visible = false;
                            }

                            else
                            {
                                if (!cmdExportColumns.IsNullOrEmpty())
                                {
                                    var CustomAttributeGrd = cmdExportColumns.Items.Where(cond => cond.Value == "CustomAttributesGrd").FirstOrDefault();

                                    if (!CustomAttributeGrd.IsNullOrEmpty() && CustomAttributeGrd.Checked == true)
                                    {
                                        grdRotations.MasterTableView.GetColumn("CustomAttributesGrd").Display = false;
                                        grdRotations.MasterTableView.GetColumn("CustomAttributesGrd").Visible = false;
                                        grdRotations.MasterTableView.GetColumn("CustomAttributesTemp").Display = true;
                                        grdRotations.MasterTableView.GetColumn("CustomAttributesTemp").Visible = true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            grdRotations.MasterTableView.GetColumn("CustomAttributesGrd").Display = false;
                            grdRotations.MasterTableView.GetColumn("CustomAttributesGrd").Visible = false;
                            grdRotations.MasterTableView.GetColumn("CustomAttributesTemp").Display = false;
                            grdRotations.MasterTableView.GetColumn("CustomAttributesTemp").Visible = false;
                        }
                    }

                    // grdRotations.MasterTableView.GetColumn("CustomAttributesGrd").Display = true;
                }
                if (e.CommandName == "Cancel")
                {
                    grdRotations.MasterTableView.GetColumn("CustomAttributesGrd").Display = true;
                    grdRotations.MasterTableView.GetColumn("CustomAttributesGrd").Visible = true;
                    grdRotations.MasterTableView.GetColumn("CustomAttributesTemp").Display = false;
                    //grdRotations.MasterTableView.GetColumn("CustomAttributesTemp").Visible = false;
                }
                #endregion

                if (e.CommandName == RadGrid.EditCommandName)
                {
                    hdnRotationId.Value = Convert.ToString((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["RotationID"]);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "OpenAddEditRotationPopup();", true);
                    e.Canceled = true;
                    return;
                }
                else if (e.CommandName == "InitInsert")
                {
                    hdnRotationId.Value = AppConsts.MINUS_ONE.ToString();
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "OpenAddEditRotationPopup();", true);
                    e.Canceled = true;
                    return;
                }

                //Insert/Update functionality
                if (e.CommandName == RadGrid.PerformInsertCommandName || e.CommandName == RadGrid.UpdateCommandName)
                {
                    if (e.CommandName == RadGrid.UpdateCommandName)
                    {
                        CurrentViewContext.ViewContract.RotationID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["RotationID"]);

                        #region UAT-2514
                        Dictionary<Boolean, DateTime> result = Presenter.IsRotationEndDateRangeNeedToManage();
                        RadDatePicker dpStartDateForMinDate = e.Item.FindControl("dpStartDate") as RadDatePicker;
                        if (result.FirstOrDefault().Key
                            && !hdnCurrentRotStartDate.Value.IsNullOrEmpty()
                            && dpStartDateForMinDate.SelectedDate != Convert.ToDateTime(hdnCurrentRotStartDate.Value)
                            && dpStartDateForMinDate.SelectedDate.Value < result.FirstOrDefault().Value)
                        {
                            //ShowInfoMessage("Rotation Start Date can not be greater than assigned package effective start date " + result.FirstOrDefault().Value.ToString("MM/dd/yyyy"));
                            e.Canceled = true;
                            (e.Item.FindControl("lblName1") as Label).ShowMessage("Rotation Start Date can not be less than assigned package effective start date " + result.FirstOrDefault().Value.ToString("MM/dd/yyyy"), MessageType.Information);
                            return;
                        }

                        #endregion
                    }
                    WclComboBox ddlTenant = e.Item.FindControl("ddlTenant") as WclComboBox;
                    if (!hdnInstNodeLabel.Value.IsNullOrEmpty())
                        (e.Item.FindControl("lblInstitutionHierarchyPB") as Label).Text = hdnInstNodeLabel.Value.HtmlEncode();

                    if (hdnInstNodeIdNew.Value == AppConsts.ZERO || hdnInstNodeIdNew.Value == String.Empty)
                    {
                        e.Canceled = true;
                        (e.Item.FindControl("lblName1") as Label).ShowMessage("Please select Institution Hierarchy.", MessageType.Error);
                        (e.Item.FindControl("lblInstitutionHierarchyPB") as Label).Text = String.Empty;
                        return;
                    }
                    List<Int32> lstSelectedAgencyHierarchyIDs = new List<int>();
                    AgencyHierarchyMultipleSelection ucAgencyHierarchyAddRotationMultiple = e.Item.FindControl("ucAgencyHierarchyAddRotationMultiple") as AgencyHierarchyMultipleSelection;
                    AgencyhierarchyCollection ucAgencyHierarchyAddEditRotationCollection = ucAgencyHierarchyAddRotationMultiple.GetAgencyHierarchyCollection();

                    if (ucAgencyHierarchyAddEditRotationCollection.IsNotNull() && ucAgencyHierarchyAddEditRotationCollection.agencyhierarchy.IsNotNull())
                    {
                        lstSelectedAgencyHierarchyIDs.AddRange(ucAgencyHierarchyAddEditRotationCollection.agencyhierarchy.Select(d => d.AgencyID).Distinct().ToList());
                    }

                    String agencyHierarchyIDs = String.Join(",", ucAgencyHierarchyAddEditRotationCollection.agencyhierarchy.Select(x => x.AgencyNodeID).ToList());
                    string agencyIds = String.Join(",", ucAgencyHierarchyAddEditRotationCollection.agencyhierarchy.Select(x => x.AgencyID).ToList());

                    if (ucAgencyHierarchyAddEditRotationCollection.agencyhierarchy.IsNullOrEmpty())
                    {
                        e.Canceled = true;
                        (e.Item.FindControl("lblName1") as Label).ShowMessage("Please select Agency Hierarchy.", MessageType.Error);
                        return;
                    }

                    if (ucAgencyHierarchyAddRotationMultiple.IsNullOrEmpty())
                    {
                        Label agencyErrMsg = e.Item.FindControl("lblAgencyErr") as Label;
                        agencyErrMsg.Visible = true;
                        e.Canceled = true;
                        return;
                    }
                    //CurrentViewContext.SelectedAgencyIDForAddForm = ucAgencyHierarchyAddEditRotation.AgencyId;
                    CurrentViewContext.SelectedAgencyIDsForAddForm = String.Join(",", lstSelectedAgencyHierarchyIDs.ToArray());

                    WclComboBox ddlInstructor = e.Item.FindControl("ddlInstructor") as WclComboBox;

                    //if (Presenter.IsPreceptorRequiredForAgency())
                    //{
                    //    if (ddlInstructor.CheckedItems.Count == AppConsts.NONE)
                    //    {
                    //        e.Canceled = true;
                    //        (e.Item.FindControl("lblName1") as Label).ShowMessage("Instructor/Preceptor is required.", MessageType.Error);
                    //        return;
                    //    }
                    //}

                    CurrentViewContext.HierarchyNode = hdnDepartmentProgmapNew.Value;

                    //  WclComboBox ddlAgency = e.Item.FindControl("ddlAgency") as WclComboBox; [ddl]
                    WclComboBox ddlRotation = e.Item.FindControl("ddlRotation") as WclComboBox;
                    CurrentViewContext.ViewContract.TenantID = Convert.ToInt32(ddlTenant.SelectedValue);
                    //CurrentViewContext.ViewContract.AgencyID = Convert.ToInt32(ddlAgency.SelectedValue); [ddl]
                    CurrentViewContext.ViewContract.AgencyIdList = CurrentViewContext.SelectedAgencyIDsForAddForm;
                    CurrentViewContext.ViewContract.RotationName = (e.Item.FindControl("txtClassification") as WclTextBox).Text.Trim().IsNullOrEmpty() ? String.Empty : (e.Item.FindControl("txtClassification") as WclTextBox).Text.Trim();
                    CurrentViewContext.ViewContract.Department = (e.Item.FindControl("txtDepartment") as WclTextBox).Text.Trim().IsNullOrEmpty() ? String.Empty : (e.Item.FindControl("txtDepartment") as WclTextBox).Text.Trim();
                    CurrentViewContext.ViewContract.Program = (e.Item.FindControl("txtProgram") as WclTextBox).Text.Trim().IsNullOrEmpty() ? String.Empty : (e.Item.FindControl("txtProgram") as WclTextBox).Text.Trim();
                    CurrentViewContext.ViewContract.Course = (e.Item.FindControl("txtCourse") as WclTextBox).Text.Trim().IsNullOrEmpty() ? String.Empty : (e.Item.FindControl("txtCourse") as WclTextBox).Text.Trim();
                    CurrentViewContext.ViewContract.UnitFloorLoc = (e.Item.FindControl("txtUnit") as WclTextBox).Text.Trim().IsNullOrEmpty() ? String.Empty : (e.Item.FindControl("txtUnit") as WclTextBox).Text.Trim();
                    //UAT-1467: Addition of "Type/Specialty" field on rotation creation and rotation details
                    CurrentViewContext.ViewContract.TypeSpecialty = (e.Item.FindControl("txtTypeSpecialty") as WclTextBox).Text.Trim().IsNullOrEmpty() ? String.Empty : (e.Item.FindControl("txtTypeSpecialty") as WclTextBox).Text.Trim();

                    //UAT 1414 notification to go out prior to student's start date for clinical rotation.
                    // CurrentViewContext.ViewContract.DaysBefore = Convert.ToInt32((e.Item.FindControl("txtDaysBefore") as WclTextBox).Text);
                    if (!String.IsNullOrEmpty((e.Item.FindControl("txtDaysBefore") as WclNumericTextBox).Text.Trim()))
                    {
                        CurrentViewContext.ViewContract.DaysBefore = Convert.ToInt32((e.Item.FindControl("txtDaysBefore") as WclNumericTextBox).Text.Trim());
                    }
                    else
                    {
                        CurrentViewContext.ViewContract.DaysBefore = null;
                    }
                    // CurrentViewContext.ViewContract.DaysBefore = (e.Item.FindControl("txtDaysBefore") as WclNumericTextBox).Text.IsNullOrEmpty() ? (int?)null : Convert.ToInt32((e.Item.FindControl("txtDaysBefore") as WclNumericTextBox).Text.Trim());
                    CurrentViewContext.ViewContract.Frequency = (e.Item.FindControl("txtFrequency") as WclNumericTextBox).Text.Trim().IsNullOrEmpty() ? String.Empty : (e.Item.FindControl("txtFrequency") as WclNumericTextBox).Text.Trim();

                    if (!String.IsNullOrEmpty((e.Item.FindControl("txtRecommendedHrs") as WclNumericTextBox).Text.Trim()))
                    {
                        CurrentViewContext.ViewContract.RecommendedHours = float.Parse((e.Item.FindControl("txtRecommendedHrs") as WclNumericTextBox).Text.Trim());
                    }
                    else
                    {
                        CurrentViewContext.ViewContract.RecommendedHours = null;
                    }
                    //UAT-1769 Addition of "# of Students" field on rotation creation and rotation details for all except students
                    if (!String.IsNullOrEmpty((e.Item.FindControl("txtStudents") as WclNumericTextBox).Text.Trim()))
                    {
                        CurrentViewContext.ViewContract.Students = float.Parse((e.Item.FindControl("txtStudents") as WclNumericTextBox).Text.Trim());
                    }
                    else
                    {
                        CurrentViewContext.ViewContract.Students = null;
                    }
                    CurrentViewContext.ViewContract.Shift = (e.Item.FindControl("txtShift") as WclTextBox).Text.Trim();
                    //UAT 1355 Addition of Term field to the Create rotation, rotation details screens
                    CurrentViewContext.ViewContract.Term = (e.Item.FindControl("txtTerm") as WclTextBox).Text.Trim();
                    CurrentViewContext.ViewContract.StartDate = (e.Item.FindControl("dpStartDate") as WclDatePicker).SelectedDate;
                    CurrentViewContext.ViewContract.EndDate = (e.Item.FindControl("dpEndDate") as WclDatePicker).SelectedDate;
                    CurrentViewContext.ViewContract.StartTime = (e.Item.FindControl("tpStartTime") as WclTimePicker).SelectedTime;
                    CurrentViewContext.ViewContract.EndTime = (e.Item.FindControl("tpEndTime") as WclTimePicker).SelectedTime;
                    //UAT-2289 : Rotation - New Field - Compliance Deadline date
                    CurrentViewContext.ViewContract.DeadlineDate = (e.Item.FindControl("dpDeadlineDate") as WclDatePicker).SelectedDate;
                    //UAT:- 2905: New field to allow notification when applicant submitted an item.
                    CurrentViewContext.ViewContract.IsAllowNotification = (e.Item.FindControl("chkAllowNotification") as WclButton).Checked;

                    WclComboBox ddlDays = e.Item.FindControl("ddlDays") as WclComboBox;

                    List<Int32> selectedDays = new List<Int32>();
                    foreach (RadComboBoxItem slctdItem in ddlDays.CheckedItems)
                    {
                        selectedDays.Add(Convert.ToInt32(slctdItem.Value));
                    }

                    List<Int32> selectedContacts = new List<Int32>();
                    foreach (RadComboBoxItem slctdContact in ddlInstructor.CheckedItems)
                    {
                        selectedContacts.Add(Convert.ToInt32(slctdContact.Value));
                    }
                    CurrentViewContext.ViewContract.DaysIdList = String.Join(",", selectedDays.ToArray());
                    CurrentViewContext.SelectedClientContacts = selectedContacts;
                    CurrentViewContext.ViewContract.ContactIdList = String.Join(",", selectedContacts.ToArray());
                    CurrentViewContext.ViewContract.HierarchyNodeIDList = CurrentViewContext.HierarchyNode;
                    WclAsyncUpload fileUpload = (e.Item.FindControl("uploadControl") as WclAsyncUpload);
                    Label lblUploadFormName = (e.Item.FindControl("lblUploadFormName") as Label);
                    Label lblUploadFormPath = (e.Item.FindControl("lblUploadFormPath") as Label);
                    Boolean isFileuploaeded = true;
                    Label lblOldFilePath = (e.Item.FindControl("lblUploadFormPath") as Label);
                    if (!hdnFileRemoved.Value.IsNullOrEmpty() && hdnFileRemoved.Value == "True")
                    {
                        CurrentViewContext.ViewContract.IfSyllabusFileRemoved = true;
                    }
                    if (fileUpload.UploadedFiles.Count > AppConsts.NONE)
                    {
                        String savedFilePath = UploadSyllabusDocuments(fileUpload);
                        if (savedFilePath.IsNullOrEmpty())
                            isFileuploaeded = false;
                        CurrentViewContext.ViewContract.SyllabusFilePath = savedFilePath;
                        CurrentViewContext.ViewContract.SyllabusFileName = fileUpload.UploadedFiles[0].FileName;
                        CurrentViewContext.ViewContract.SyllabusFileSize = fileUpload.UploadedFiles[0].ContentLength;
                    }
                    else
                    {
                        Label lblSyllabusDocumentError = e.Item.FindControl("lblSyllabusDocumentError") as Label; //Make conditionally aestrik visible in case instructor preceptor required 
                        if (hdnValidateFileUploadControl.Value == "true")
                        {
                            if (fileUpload.UploadedFiles.Count < 1 && lblUploadFormName.Text.IsNullOrWhiteSpace())
                            {
                                lblSyllabusDocumentError.Style["display"] = "block";
                                e.Canceled = true;

                                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "GetRotationRequiredFieldOptions('" + Convert.ToInt32(ddlTenant.SelectedValue) + "','" + agencyIds + "');", true);
                                return;
                            }
                        }
                    }

                    if (e.Item.FindControl("caCustomAttributes").IsNotNull())
                    {
                        SharedUserCustomAttributeForm caCustomAttributes = e.Item.FindControl("caCustomAttributes") as SharedUserCustomAttributeForm;
                        CurrentViewContext.SaveCustomAttributeList = caCustomAttributes.GetCustomAttributeValues();
                    }
                    else
                        CurrentViewContext.SaveCustomAttributeList = new List<CustomAttribteContract>();

                    //UAT 3041 
                    if (e.Item.FindControl("chkEditPermission").IsNotNull())
                    {
                        CheckBoxList chkEditPermission = (e.Item.FindControl("chkEditPermission") as CheckBoxList);

                        foreach (ListItem item in chkEditPermission.Items)
                        {
                            if (item.Value.ToString() == "CA")
                            {
                                CurrentViewContext.ViewContract.IsEditableByClientAdmin = item.Selected;
                            }
                            else if (item.Value.ToString() == "AGU")
                            {
                                CurrentViewContext.ViewContract.IsEditableByAgencyUser = item.Selected;
                            }
                        }
                    }

                    Presenter.IsAdminLoggedIn();
                    if (!CurrentViewContext.IsAdminLoggedIn)
                    {
                        CurrentViewContext.ViewContract.IsEditableByClientAdmin = true;
                    }


                    if (isFileuploaeded)
                    {
                        //UAT-2424 Check if it is normal rotation save click or cloning a rotation.
                        if (!ddlRotation.SelectedValue.IsNullOrEmpty() && Convert.ToInt32(ddlRotation.SelectedValue) != AppConsts.NONE)
                        {
                            if (fileUpload.UploadedFiles.Count == AppConsts.NONE && !lblUploadFormName.Text.IsNullOrEmpty()) //In Cloning mode fileupload control count is zero and file doesn't get updated or uploaded.File name and path is saved in label.
                            {
                                CurrentViewContext.ViewContract.SyllabusFilePath = lblUploadFormPath.Text;
                                CurrentViewContext.ViewContract.SyllabusFileName = lblUploadFormName.Text;
                            }
                            //Is any Field of rotation is updated or not ,if not then raise a info msg.
                            IsRotationFieldsUpdated(CloneContract, CurrentViewContext.ViewContract);

                            if (IsFieldsUpdated)
                            {
                                CurrentViewContext.ViewContract.IsCloningRotation = true;
                                CurrentViewContext.ViewContract.RequirementPackageID = Presenter.GetApplicantPackage();
                                CurrentViewContext.ViewContract.InstructorPreceptorPkgID = Presenter.GetInstructorPackage();
                                CurrentViewContext.ViewContract.IsAgencyUpdated = IsAgencyChanged;
                            }
                            else
                            {
                                e.Canceled = true;
                                (e.Item.FindControl("lblName1") as Label).ShowMessage("Please update at least one field to complete rotation clone.", MessageType.Error);
                                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "GetRotationRequiredFieldOptions('" + Convert.ToInt32(ddlTenant.SelectedValue) + "','" + agencyIds + "');", true);
                                return;
                            }
                        }

                        if (Presenter.SaveUpdateClinicalRotation())
                        {
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ResetValidator();", true);
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ClearDefaultValidator();", true);

                            if (e.CommandName == RadGrid.UpdateCommandName)
                            {
                                // base.ShowSuccessMessage("Clinical rotation updated successfully.");
                                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ConfirmationMessage('Clinical rotation updated successfully.','sucs');", true);
                            }
                            else
                            {
                                if (!Session[AppConsts.ROTATION_DETAIL_SESSION_KEY].IsNullOrEmpty())
                                {
                                    Session[AppConsts.ROTATION_DETAIL_SESSION_KEY] = null;
                                }
                                // base.ShowSuccessMessage("Clinical rotation saved successfully.");
                                //System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ConfirmationMessage('Clinical rotation saved successfully.','sucs');", true);

                                //UAT-3053 : As an admin, after creating a rotation I should be directed to the rotation details screen of that rotation.
                                String RotationId = Convert.ToString(CurrentViewContext.ViewContract.RotationID);
                                //String AgencyId = Convert.ToString(CurrentViewContext.ViewContract.AgencyID);
                                String AgencyId = String.Empty;
                                if (!CurrentViewContext.ViewContract.AgencyIdList.IsNullOrEmpty())
                                    AgencyId = Convert.ToString(CurrentViewContext.ViewContract.AgencyIdList.Split(',').FirstOrDefault());

                                String TenantId = Convert.ToString(ddlTenant.SelectedValue);

                                //UAT-3041
                                String IsEditableByClientAdmin = Convert.ToString(CurrentViewContext.ViewContract.IsEditableByClientAdmin);
                                String IsEditableByAgencyUser = Convert.ToString(CurrentViewContext.ViewContract.IsEditableByAgencyUser);

                                RedirectToDetail(RotationId, AgencyId, TenantId, true, IsEditableByClientAdmin, IsEditableByAgencyUser);
                            }
                            e.Canceled = false;
                        }

                    }
                    else
                    {
                        e.Canceled = false;
                        base.ShowErrorMessage("some error has occured due to which rotation can not be saved.");
                    }
                }
                //Delete functionality
                if (e.CommandName == RadGrid.DeleteCommandName)
                {
                    CurrentViewContext.ViewContract.RotationID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["RotationID"]);
                    //if clinical rotation members exist for rotation then user not able to deleted the rotation.
                    if (Presenter.IsClinicalRotationMembersExistForRotation())
                    {
                        //base.ShowInfoMessage("You cannot delete this rotation as it is associated with other objects.");
                        System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ConfirmationMessage('You cannot delete this rotation as it is associated with other objects.','info');", true);
                    }
                    else
                    {
                        Presenter.DeleteClinicalRotation();
                        //base.ShowSuccessMessage("Clinical rotation deleted successfully.");
                        System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ConfirmationMessage('Clinical rotation deleted successfully.','sucs');", true);
                    }
                }
                if (e.CommandName.IsNullOrEmpty())
                {
                    if (!hdnInstNodeLabel.Value.IsNullOrEmpty())
                        (e.Item.FindControl("lblInstitutionHierarchyPB") as Label).Text = hdnInstNodeLabel.Value.HtmlEncode();
                    //grdRotations.MasterTableView.GetColumn("CustomAttributesGrd").Display = true;
                }
                //   grdRotations.MasterTableView.GetColumn("CustomAttributesGrd").Display = true;



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

        protected void grdRotations_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
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

        protected void grdRotations_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            try
            {
                if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
                {
                    //ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ResetValidator();", true);
                    GridEditFormItem editform = (e.Item as GridEditFormItem);
                    Presenter.IsAdminLoggedIn();
                    hdnDepartmentProgmapNew.Value = String.Empty;
                    hdnInstNodeIdNew.Value = String.Empty;
                    WclComboBox ddlTenant = editform.FindControl("ddlTenant") as WclComboBox;
                    WclComboBox ddlAgency = editform.FindControl("ddlAgency") as WclComboBox;
                    WclComboBox ddlRotation = editform.FindControl("ddlRotation") as WclComboBox;
                    HtmlAnchor lnkInstitutionHierarchyPB = editform.FindControl("lnkInstitutionHierarchyPB") as HtmlAnchor;
                    //AgencyHierarchySelection ucAgencyHierarchyAddEditRotation = editform.FindControl("ucAgencyHierarchyAddEditRotation") as AgencyHierarchySelection;
                    AgencyHierarchyMultipleSelection ucAgencyHierarchyAddRotationMultiple = editform.FindControl("ucAgencyHierarchyAddRotationMultiple") as AgencyHierarchyMultipleSelection;
                    lnkInstitutionHierarchyPB.Attributes["Class"] = string.Empty;
                    ddlTenant.DataSource = CurrentViewContext.lstTenant;
                    ddlTenant.DataBind();
                    ddlTenant.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT--"));


                    //ucAgencyHierarchyAddEditRotation.TenantId = 0;
                    //ucAgencyHierarchyAddEditRotation.IsReadOnlyMode = false;
                    //ucAgencyHierarchyAddEditRotation.IsInstitutionHierarchyRequired = true;

                    HtmlGenericControl divAgencyHierarchyMulti = editform.FindControl("divAgencyHierarchyMulti") as HtmlGenericControl;
                    divAgencyHierarchyMulti.Style["display"] = "block";

                    ucAgencyHierarchyAddRotationMultiple.AgencyHierarchyNodeSelection = true;
                    ucAgencyHierarchyAddRotationMultiple.IsInstitutionHierarchyRequired = true;
                    ucAgencyHierarchyAddRotationMultiple.IsAgencyNodeCheckable = false;

                    //ucAgencyHierarchyAddRotationMultiple.IsInDisableMode = true;

                    List<AgencyDetailContract> agency = CurrentViewContext.lstAgency;


                    if (CurrentViewContext.SelectedTenantID > AppConsts.NONE)
                    {
                        //ucAgencyHierarchyAddEditRotation.TenantId = CurrentViewContext.SelectedTenantID;                       
                        ucAgencyHierarchyAddRotationMultiple.TenantId = CurrentViewContext.SelectedTenantID;

                        ddlTenant.Enabled = false;
                        ddlTenant.SelectedValue = CurrentViewContext.SelectedTenantID.ToString();
                        CurrentViewContext.lstAgencyForAddForm = CurrentViewContext.lstAgency;

                        //if (CurrentViewContext.SelectedAgencyID > AppConsts.NONE)
                        //{
                        //    CurrentViewContext.SelectedAgencyIDForAddForm = CurrentViewContext.SelectedAgencyID;
                        //}
                    }
                    else
                    {
                        ddlTenant.Enabled = true;
                        ucAgencyHierarchyAddRotationMultiple.TenantId = -1;
                    }

                    //UAT-2034:Phase 4 (16): Manage Rotation screen updates
                    Int32 rotAgencyID = AppConsts.NONE;
                    string agencyHierarchyID = string.Empty;
                    Int32 rootNodeID = AppConsts.NONE;

                    if (!(e.Item is GridEditFormInsertItem))
                    {
                        string agencyIds = Convert.ToString((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["AgencyIDs"]);
                        rotAgencyID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["AgencyID"]);
                        agencyHierarchyID = Convert.ToString((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["AgencyHierarchyID"]);
                        rootNodeID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["RootNodeID"]);
                        ucAgencyHierarchyAddRotationMultiple.SelectedNodeIds = agencyHierarchyID;
                        ucAgencyHierarchyAddRotationMultiple.SelectedAgecnyIds = agencyIds;
                        Session["NodeSelected"] = agencyHierarchyID;
                        Session["AgencySelected"] = agencyIds;
                        ucAgencyHierarchyAddRotationMultiple.SelectedRootNodeId = rootNodeID;

                        // ucAgencyHierarchyAddEditRotation.IsReadOnlyMode = true;
                        ucAgencyHierarchyAddRotationMultiple.IsInDisableMode = true;

                        lnkInstitutionHierarchyPB.Attributes["Class"] = "disabled";
                    }

                    // ucAgencyHierarchyAddEditRotation.NodeId = agencyHierarchyID;
                    // ucAgencyHierarchyAddEditRotation.SelectedRootNodeId = rootNodeID;




                    //BindAgencyForAddForm(ddlAgency, rotAgencyID);
                    // BindAgencyForAddForm(ucAgencyHierarchy.AgencyId, rotAgencyID);

                    //if (!String.IsNullOrEmpty(CurrentViewContext.SelectedAgencyIDsForAddForm))
                    //{
                    //    ucAgencyHierarchyAddRotationMultiple.SelectedAgecnyIds = CurrentViewContext.SelectedAgencyIDsForAddForm;
                    //}

                    if (!String.IsNullOrEmpty(CurrentViewContext.SelectedAgencyIDsForAddForm))
                    {
                        ucAgencyHierarchyAddRotationMultiple.SelectedAgecnyIds = CurrentViewContext.SelectedAgencyIDsForAddForm;
                    }
                    //UAT-2034:Phase 4 (16): Manage Rotation screen updates
                    else if (rotAgencyID > AppConsts.NONE)
                    {
                        ucAgencyHierarchyAddRotationMultiple.SelectedAgecnyIds = rotAgencyID.ToString();
                        Session["AgencySelected"] = rotAgencyID.ToString();
                    }

                    BindRotationForAddForm(ddlRotation);

                    WclComboBox cmbDays = editform.FindControl("ddlDays") as WclComboBox;
                    BindWeekDaysForAddForm(cmbDays);

                    WclComboBox cmbContacts = editform.FindControl("ddlInstructor") as WclComboBox;
                    BindContactssForAddForm(cmbContacts);
                    CustomValidator cstValidator = editform.FindControl("cstValidator") as CustomValidator;
                    HtmlGenericControl spnInsPre = editform.FindControl("spnInsPre") as HtmlGenericControl;
                    List<Int32> AgencyIds = new List<int>();
                    AgencyhierarchyCollection ucAgencyHierarchyAddEditRotationCollection = ucAgencyHierarchyAddRotationMultiple.GetAgencyHierarchyCollection();
                    if (ucAgencyHierarchyAddEditRotationCollection.IsNotNull() && ucAgencyHierarchyAddEditRotationCollection.agencyhierarchy.IsNotNull())
                    {
                        AgencyIds.AddRange(ucAgencyHierarchyAddEditRotationCollection.agencyhierarchy.Select(d => d.AgencyID).Distinct().ToList());
                    }
                    CurrentViewContext.SelectedAgencyIDsForAddForm = String.Join(",", AgencyIds.ToArray());
                    //if (Presenter.IsPreceptorRequiredForAgency()) //UAT-2554
                    //{
                    //    cstValidator.Enabled = true;
                    //    spnInsPre.Attributes["class"] = "reqd";
                    //}
                    //else
                    //{
                    //    cstValidator.Enabled = false;
                    //    spnInsPre.Attributes["class"] = "controlHidden reqd";
                    //}

                    WclDatePicker dpStartDate = editform.FindControl("dpStartDate") as WclDatePicker;
                    WclDatePicker dpEndDate = editform.FindControl("dpEndDate") as WclDatePicker;
                    WclTimePicker tpStartTime = editform.FindControl("tpStartTime") as WclTimePicker;
                    WclTimePicker tpEndTime = editform.FindControl("tpEndTime") as WclTimePicker;
                    HtmlGenericControl dvComplioId = editform.FindControl("dvComplioId") as HtmlGenericControl;
                    WclAsyncUpload fileUpload = editform.FindControl("uploadControl") as WclAsyncUpload;
                    Label lblUploadFormName = editform.FindControl("lblUploadFormName") as Label;
                    Label lblUploadFormPath = editform.FindControl("lblUploadFormPath") as Label;
                    LinkButton lnkRemove = editform.FindControl("lnkRemove") as LinkButton;
                    //UAT-2289
                    WclDatePicker dpDeadlineDate = editform.FindControl("dpDeadlineDate") as WclDatePicker;
                    //UAT-2905
                    WclButton chkAllowNotification = editform.FindControl("chkAllowNotification") as WclButton;
                    WclNumericTextBox txtDaysBefore = editform.FindControl("txtDaysBefore") as WclNumericTextBox;
                    WclNumericTextBox txtFrequency = editform.FindControl("txtFrequency") as WclNumericTextBox;

                    //UAT 3041 
                    CheckBoxList chkEditPermission = editform.FindControl("chkEditPermission") as CheckBoxList;
                    //If Client Admin Logged In
                    if (!CurrentViewContext.IsAdminLoggedIn)
                    {
                        chkEditPermission.Items.Remove(chkEditPermission.Items.FindByValue("CA"));
                    }
                    //Label lblAgencyName = editform.FindControl("lblAgencyName") as Label;
                    //lblAgencyName.Text = ddlAgency.Text;
                    dvComplioId.Visible = false;
                    ClinicalRotationDetailContract clinicalRotationDetail = e.Item.DataItem as ClinicalRotationDetailContract;
                    //UAT 1414 notification to go out prior to student's start date for clinical rotation
                    txtDaysBefore.Text = "30";
                    txtFrequency.Text = "15";
                    if (e.Item is GridEditFormInsertItem)
                    {
                        if (!CurrentViewContext.IsAdminLoggedIn)
                        {
                            Dictionary<Int32, String> dicDefaultNode = Presenter.GetDefaultPermissionForClientAdmin();
                            if (!dicDefaultNode.IsNullOrEmpty())
                            {
                                hdnDepartmentProgmapNew.Value = Convert.ToString(dicDefaultNode.Keys.FirstOrDefault());
                                hdnInstNodeIdNew.Value = Convert.ToString(dicDefaultNode.Keys.FirstOrDefault());
                                (e.Item.FindControl("lblInstitutionHierarchyPB") as Label).Text = dicDefaultNode.Values.FirstOrDefault();
                                hdnInstNodeLabel.Value = dicDefaultNode.Values.FirstOrDefault();
                                ucAgencyHierarchyAddRotationMultiple.SelectedInstitutionNodeIds = Convert.ToString(dicDefaultNode.Keys.FirstOrDefault());

                            }
                        }
                    }

                    if (clinicalRotationDetail != null)
                    {
                        dvComplioId.Visible = true;
                        //ddlAgency.Enabled = false; [ddl]
                        //ddlAgency.SelectedValue = clinicalRotationDetail.AgencyID.ToString();
                        dpStartDate.SelectedDate = clinicalRotationDetail.StartDate;
                        dpEndDate.SelectedDate = clinicalRotationDetail.EndDate;
                        tpStartTime.SelectedTime = clinicalRotationDetail.StartTime;
                        tpEndTime.SelectedTime = clinicalRotationDetail.EndTime;
                        //UAT-2514
                        hdnCurrentRotStartDate.Value = dpStartDate.SelectedDate.Value.ToString();

                        //UAT-2289
                        dpDeadlineDate.SelectedDate = clinicalRotationDetail.DeadlineDate;
                        //UAT-2905
                        chkAllowNotification.Checked = clinicalRotationDetail.IsAllowNotification;

                        //UAT 3041 
                        foreach (ListItem item in chkEditPermission.Items)
                        {
                            if (item.Value.ToString() == "CA")
                            {
                                item.Selected = clinicalRotationDetail.IsEditableByClientAdmin;
                            }

                            if (item.Value.ToString() == "AGU")
                            {
                                item.Selected = clinicalRotationDetail.IsEditableByAgencyUser;
                            }
                        }

                        //UAT 1414 notification to go out prior to student's start date for clinical rotation
                        txtDaysBefore.Text = clinicalRotationDetail.DaysBefore.ToString();
                        txtFrequency.Text = clinicalRotationDetail.Frequency;

                        if (!clinicalRotationDetail.ContactIdList.IsNullOrEmpty())
                        {
                            String[] selectedContactIds = clinicalRotationDetail.ContactIdList.Split(',');
                            foreach (RadComboBoxItem item in cmbContacts.Items)
                            {
                                item.Checked = selectedContactIds.Contains(item.Value);
                            }
                        }
                        if (!clinicalRotationDetail.DaysIdList.IsNullOrEmpty())
                        {
                            String[] selectedDayIds = clinicalRotationDetail.DaysIdList.Split(',');
                            foreach (RadComboBoxItem item in cmbDays.Items)
                            {
                                item.Checked = selectedDayIds.Contains(item.Value);
                            }
                        }
                        if (!clinicalRotationDetail.SyllabusFileName.IsNullOrEmpty())
                        {
                            lblUploadFormName.Text = clinicalRotationDetail.SyllabusFileName.HtmlEncode();
                            lblUploadFormPath.Text = clinicalRotationDetail.SyllabusFilePath.HtmlEncode();
                            lblUploadFormName.Visible = true;
                            lnkRemove.Visible = true;
                            fileUpload.Visible = false;
                        }

                        hdnDepartmentProgmapNew.Value = clinicalRotationDetail.HierarchyNodeIDList;
                        hdnInstNodeIdNew.Value = clinicalRotationDetail.HierarchyNodeIDList;
                        (e.Item.FindControl("lblInstitutionHierarchyPB") as Label).Text = clinicalRotationDetail.HierarchyNodes;

                        if (!hdnInstNodeIdNew.Value.IsNullOrEmpty())
                        {
                            ucAgencyHierarchyAddRotationMultiple.SelectedInstitutionNodeIds = Convert.ToString(hdnInstNodeIdNew.Value);
                        }
                        #region UAT-2666
                        Int32 currentClinicalRotationId = clinicalRotationDetail.RotationID;
                        RotationFieldUpdatedByAgencyContract RotationFieldUpdaeByAgency = CurrentViewContext.lstRotationFieldUpdaeByAgency.Where(cmd => cmd.ClinicalRotationID == currentClinicalRotationId).FirstOrDefault();

                        HtmlGenericControl dvRotationName = (editform.FindControl("dvRotationName") as HtmlGenericControl);
                        HtmlGenericControl dvType = (editform.FindControl("dvType") as HtmlGenericControl);
                        HtmlGenericControl dvDepartment = (editform.FindControl("dvDepartment") as HtmlGenericControl);
                        HtmlGenericControl dvProgram = (editform.FindControl("dvProgram") as HtmlGenericControl);
                        HtmlGenericControl dvCourse = (editform.FindControl("dvCourse") as HtmlGenericControl);
                        HtmlGenericControl dvTerm = (editform.FindControl("dvTerm") as HtmlGenericControl);
                        HtmlGenericControl dvUnitLoc = (editform.FindControl("dvUnitLoc") as HtmlGenericControl);
                        HtmlGenericControl dvStudent = (editform.FindControl("dvStudent") as HtmlGenericControl);
                        HtmlGenericControl dvHour = (editform.FindControl("dvHour") as HtmlGenericControl);
                        HtmlGenericControl dvShift = (editform.FindControl("dvShift") as HtmlGenericControl);

                        if (!RotationFieldUpdaeByAgency.IsNullOrEmpty())
                        {
                            if (RotationFieldUpdaeByAgency.IsCourseUpdated)
                            {
                                dvCourse.Attributes.Add("class", "highlight");
                            }
                            else
                            {
                                dvCourse.Attributes.Add("class", "");
                            }
                            if (RotationFieldUpdaeByAgency.IsDepartmentUpdated)
                            {
                                dvDepartment.Attributes.Add("class", "highlight");
                            }
                            else
                            {
                                dvDepartment.Attributes.Add("class", "");
                            }

                            if (RotationFieldUpdaeByAgency.IsNoOfHoursUpdated)
                            {
                                dvHour.Attributes.Add("class", "highlight");
                            }
                            else
                            {
                                dvHour.Attributes.Add("class", "");
                            }

                            if (RotationFieldUpdaeByAgency.IsNoOfStudentsUpdated)
                            {
                                dvStudent.Attributes.Add("class", "highlight");
                            }
                            else
                            {
                                dvStudent.Attributes.Add("class", "");
                            }

                            if (RotationFieldUpdaeByAgency.IsProgramUpdated)
                            {
                                dvProgram.Attributes.Add("class", "highlight");
                            }
                            else
                            {
                                dvProgram.Attributes.Add("class", "");
                            }

                            if (RotationFieldUpdaeByAgency.IsRotationNameUpadted)
                            {
                                dvRotationName.Attributes.Add("class", "highlight");
                            }
                            else
                            {
                                dvRotationName.Attributes.Add("class", "");
                            }

                            if (RotationFieldUpdaeByAgency.IsRotationShiftUpdated)
                            {
                                dvShift.Attributes.Add("class", "highlight");
                            }
                            else
                            {
                                dvShift.Attributes.Add("class", "");
                            }

                            if (RotationFieldUpdaeByAgency.IsTermUpdated)
                            {
                                dvTerm.Attributes.Add("class", "highlight");
                            }
                            else
                            {
                                dvTerm.Attributes.Add("class", "");
                            }

                            if (RotationFieldUpdaeByAgency.IsTypeSpecialtyUpdated)
                            {
                                dvType.Attributes.Add("class", "highlight");
                            }
                            else
                            {
                                dvType.Attributes.Add("class", "");
                            }

                            if (RotationFieldUpdaeByAgency.IsUnitFloorLocUpdated)
                            {
                                dvUnitLoc.Attributes.Add("class", "highlight");
                            }
                            else
                            {
                                dvUnitLoc.Attributes.Add("class", "");
                            }
                        }
                        #endregion

                        //UAT-3221
                        if (!(e.Item is GridEditFormInsertItem))
                        {
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "markAgencyHierarchyLinkDisabled();", true);
                        }
                    }
                }
                //UAT-1778: Addition of "custom attribute" column to the manage rotation grid
                if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
                {
                    ClinicalRotationDetailContract clinicalRotationDetail = e.Item.DataItem as ClinicalRotationDetailContract;
                    GridDataItem dataItem = (GridDataItem)e.Item;
                    if (Convert.ToString(dataItem["CustomAttributesGrd"].Text).Length > 80)
                    {
                        dataItem["CustomAttributesGrd"].ToolTip = dataItem["CustomAttributesGrd"].Text;
                        dataItem["CustomAttributesGrd"].Text = (dataItem["CustomAttributesGrd"].Text).ToString().Substring(0, 80) + "...";
                    }
                    #region UAT-2666
                    Int32 currentClinicalRotationId = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["RotationID"]);
                    RotationFieldUpdatedByAgencyContract RotationFieldUpdaeByAgency = CurrentViewContext.lstRotationFieldUpdaeByAgency.Where(cmd => cmd.ClinicalRotationID == currentClinicalRotationId).FirstOrDefault();

                    #region UAT-3682
                    //(e.Item.FindControl("lnkNumberOfStudents") as HtmlAnchor).InnerText = Convert.ToString(clinicalRotationDetail.Students);
                    // String numberOfStudents = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Students"].ToString();
                    HtmlAnchor lnkNumberOfStudents = ((HtmlAnchor)e.Item.FindControl("lnkNumberOfStudents"));
                    (e.Item.FindControl("lnkNumberOfStudents") as HtmlAnchor).InnerText = Convert.ToString(clinicalRotationDetail.Students);
                    if (!string.IsNullOrEmpty((e.Item.FindControl("lnkNumberOfStudents") as HtmlAnchor).InnerText))
                    {
                        //Adding encrypted query string to lnkNumberOfStudents
                        Dictionary<String, String> queryString = new Dictionary<String, String>();

                        String agencyid = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["AgencyID"].ToString();

                        queryString = new Dictionary<String, String>
                                                                 {
                                                                    {"TenantId", CurrentViewContext.SelectedTenantID.ToString() },
                                                                    {"RotationID",Convert.ToString(currentClinicalRotationId)},
                                                                    {"AgencyId",Convert.ToString(agencyid)},
                                                                    {"CurrentLoggedInUserId",Convert.ToString(CurrentViewContext.CurrentLoggedInUserId)} 
                                                                 };


                        lnkNumberOfStudents.Attributes.Add("args", queryString.ToEncryptedQueryString());

                    }
                    else
                    {
                        lnkNumberOfStudents.Visible = false;
                    }
                    #endregion

                    if (!RotationFieldUpdaeByAgency.IsNullOrEmpty())
                    {
                        if (RotationFieldUpdaeByAgency.IsRotationNameUpadted)
                        {
                            dataItem["RotationName"].CssClass = "highlightGrid";
                        }
                        else
                        {
                            dataItem["RotationName"].CssClass = "";
                        }
                        if (RotationFieldUpdaeByAgency.IsStartDateUpdated)
                        {
                            dataItem["StartDate"].CssClass = "highlightGrid";
                        }
                        else
                        {
                            dataItem["StartDate"].CssClass = "";
                        }
                        if (RotationFieldUpdaeByAgency.IsEndDateUpdated)
                        {
                            dataItem["EndDate"].CssClass = "highlightGrid";
                        }
                        else
                        {
                            dataItem["EndDate"].CssClass = "";
                        }
                        if (RotationFieldUpdaeByAgency.IsTypeSpecialtyUpdated)
                        {
                            dataItem["TypeSpecialty"].CssClass = "highlightGrid";
                        }
                        else
                        {
                            dataItem["TypeSpecialty"].CssClass = "";
                        }

                        if (RotationFieldUpdaeByAgency.IsDepartmentUpdated)
                        {
                            dataItem["Department"].CssClass = "highlightGrid";
                        }
                        else
                        {
                            dataItem["Department"].CssClass = "";
                        }

                        if (RotationFieldUpdaeByAgency.IsProgramUpdated)
                        {
                            dataItem["Program"].CssClass = "highlightGrid";
                        }
                        else
                        {
                            dataItem["Program"].CssClass = "";
                        }

                        if (RotationFieldUpdaeByAgency.IsCourseUpdated)
                        {
                            dataItem["Course"].CssClass = "highlightGrid";
                        }
                        else
                        {
                            dataItem["Course"].CssClass = "";
                        }

                        if (RotationFieldUpdaeByAgency.IsTermUpdated)
                        {
                            dataItem["Term"].CssClass = "highlightGrid";
                        }
                        else
                        {
                            dataItem["Term"].CssClass = "";
                        }

                        if (RotationFieldUpdaeByAgency.IsUnitFloorLocUpdated)
                        {
                            dataItem["unit"].CssClass = "highlightGrid";
                        }
                        else
                        {
                            dataItem["unit"].CssClass = "";
                        }

                        if (RotationFieldUpdaeByAgency.IsNoOfStudentsUpdated)
                        {
                            dataItem["Students"].CssClass = "highlightGrid";
                        }
                        else
                        {
                            dataItem["Students"].CssClass = "";
                        }

                        if (RotationFieldUpdaeByAgency.IsNoOfHoursUpdated)
                        {
                            dataItem["RecommendedHours"].CssClass = "highlightGrid";
                        }
                        else
                        {
                            dataItem["RecommendedHours"].CssClass = "";
                        }

                        if (RotationFieldUpdaeByAgency.IsRotationShiftUpdated)
                        {
                            dataItem["Shift"].CssClass = "highlightGrid";
                        }
                        else
                        {
                            dataItem["Shift"].CssClass = "";
                        }
                    }

                    #endregion
                    //UAT 3041
                    Presenter.IsAdminLoggedIn();
                    if (!CurrentViewContext.IsAdminLoggedIn)
                    {
                        Boolean IsEditableByClientAdmin = Convert.ToBoolean((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["IsEditableByClientAdmin"]);
                        if (!IsEditableByClientAdmin)
                        {
                            //dataItem["DeleteColumn"].Controls.Clear();
                            //dataItem["EditCommandColumn"].Controls.Clear();
                            dataItem["ViewDetail"].ColumnSpan = 3;
                            dataItem["EditCommandColumn"].Attributes.Add("Style", "display:none");
                            dataItem["DeleteColumn"].Attributes.Add("Style", "display:none");
                        }
                        CheckBox checkBox = ((CheckBox)e.Item.FindControl("chkSelectRotation"));
                        checkBox.Enabled = IsEditableByClientAdmin;
                    }

                    //UAT-2034:Phase 4 (16): Manage Rotation screen updates
                    Int32 rotationID = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["RotationID"]);
                    Int32 agencyID = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["AgencyID"]);
                    Dictionary<Int32, Int32> selectedRotationDic = CurrentViewContext.DicOfSelectedRotation;

                    if (selectedRotationDic.IsNotNull() && rotationID != AppConsts.NONE && selectedRotationDic.ContainsKey(rotationID))
                    {
                        CheckBox checkBox = ((CheckBox)e.Item.FindControl("chkSelectRotation"));
                        checkBox.Checked = true;
                    }
                }
                //UAT-2034:Phase 4 (16): Manage Rotation screen updates
                if (e.Item.ItemType.Equals(GridItemType.Footer))
                {
                    Int32 rowCount = grdRotations.Items.Count;
                    if (rowCount > 0)
                    {
                        Int32 checkCount = 0;
                        foreach (GridDataItem item in grdRotations.Items)
                        {
                            CheckBox checkBox = ((CheckBox)item.FindControl("chkSelectRotation"));
                            if (checkBox.Checked)
                            {
                                checkCount++;
                            }
                        }
                        if (rowCount == checkCount)
                        {
                            GridHeaderItem item = grdRotations.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
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

        protected void grdRotations_ItemCreated(object sender, GridItemEventArgs e)
        {
            try
            {
                if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ResetValidator();", true);
                    GridEditFormItem editform = (e.Item as GridEditFormItem);
                    WclComboBox ddlTenant = editform.FindControl("ddlTenant") as WclComboBox;

                    if (CurrentViewContext.SelectedTenantID > AppConsts.NONE)
                    {
                        CurrentViewContext.SelectedTenantIDForAddForm = CurrentViewContext.SelectedTenantID;
                    }
                    if (!ddlTenant.SelectedValue.IsNullOrEmpty())
                    {
                        CurrentViewContext.SelectedTenantIDForAddForm = Convert.ToInt32(ddlTenant.SelectedValue);
                    }
                    if (CurrentViewContext.SelectedTenantIDForAddForm > AppConsts.NONE)
                    {
                        SharedUserCustomAttributeForm caCustomAttributes = (SharedUserCustomAttributeForm)Page.LoadControl("~\\ClinicalRotation\\UserControl\\SharedUserCustomAttributeForm.ascx");
                        caCustomAttributes.ID = "caCustomAttributes";

                        Int32? rotationID = null;
                        String AgencyHierarchyID = string.Empty;

                        WclComboBox ddlRotation = editform.FindControl("ddlRotation") as WclComboBox;
                        if (!(e.Item is GridEditFormInsertItem))
                        {
                            rotationID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["RotationID"]);
                            AgencyHierarchyID = Convert.ToString((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["AgencyHierarchyID"]);
                            Int32 RootNodeID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["RootNodeID"]);
                            string agencyIds = Convert.ToString((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["AgencyIDs"]);


                            if (!ddlTenant.SelectedValue.IsNullOrEmpty())
                            {
                                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "GetRotationRequiredFieldOptions('" + Convert.ToInt32(ddlTenant.SelectedValue) + "','" + agencyIds + "');", true);
                            }
                            else
                            {
                                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "GetRotationRequiredFieldOptions('" + CurrentViewContext.SelectedTenantID + "','" + agencyIds + "');", true);

                            }
                        }
                        else if (CurrentViewContext.SelectedRotationIDForCloning > AppConsts.NONE)
                        {
                            rotationID = CurrentViewContext.SelectedRotationIDForCloning;
                        }
                        else if (!ddlRotation.SelectedValue.IsNullOrEmpty())
                        {
                            rotationID = Convert.ToInt32(ddlRotation.SelectedValue);
                        }

                        Presenter.GetCustomAttributeList(rotationID);
                        if (!CurrentViewContext.GetCustomAttributeList.IsNullOrEmpty())
                        {
                            GenerateCustomAttributes(caCustomAttributes);
                            Panel pnlEditForm = editform.FindControl("pnlEditForm") as Panel;
                            pnlEditForm.Controls.Add(caCustomAttributes);
                        }


                        // Get Granular Permissions, only for Client Admin
                        if (!CurrentViewContext.IsAdminLoggedIn)
                        {
                            Presenter.GetGranularPermissions();
                            ApplyGranularPermissions(editform);
                        }

                        //WclComboBox ddlAgency = editform.FindControl("ddlAgency") as WclComboBox;
                        //if (!ddlAgency.SelectedValue.IsNullOrEmpty())
                        //{
                        //    CurrentViewContext.SelectedAgencyIDForAddForm = Convert.ToInt32(ddlAgency.SelectedValue);
                        //}

                        AgencyHierarchyMultipleSelection ucAgencyHierarchyAddEditRotationMultiple = editform.FindControl("ucAgencyHierarchyAddEditRotation") as AgencyHierarchyMultipleSelection;
                        List<Int32> AgencyIds = new List<int>();


                        if (ucAgencyHierarchyAddEditRotationMultiple.IsNotNull())
                        {
                            AgencyhierarchyCollection ucAgencyHierarchyAddEditRotationCollection = ucAgencyHierarchyAddEditRotationMultiple.GetAgencyHierarchyCollection();
                            if (ucAgencyHierarchyAddEditRotationCollection.IsNotNull() && ucAgencyHierarchyAddEditRotationCollection.agencyhierarchy.IsNotNull())
                            {
                                AgencyIds.AddRange(ucAgencyHierarchyAddEditRotationCollection.agencyhierarchy.Select(d => d.AgencyID).Distinct().ToList());
                            }
                            CurrentViewContext.SelectedAgencyIDsForAddForm = String.Join(",", AgencyIds.ToArray());
                        }

                        //AgencyHierarchySelection ucAgencyHierarchyAddEditRotation = editform.FindControl("ucAgencyHierarchyAddEditRotation") as AgencyHierarchySelection;
                        //if (!ucAgencyHierarchyAddEditRotation.AgencyId.IsNullOrEmpty() && ucAgencyHierarchyAddEditRotation.AgencyId > 0)
                        //{
                        //    CurrentViewContext.SelectedAgencyIDForAddForm = ucAgencyHierarchyAddEditRotation.AgencyId;

                        //}                       

                        // CustomValidator cstValidator = editform.FindControl("cstValidator") as CustomValidator;
                        // HtmlGenericControl spnInsPre = editform.FindControl("spnInsPre") as HtmlGenericControl;
                        //if (Presenter.IsPreceptorRequiredForAgency()) //UAT-2554
                        //{
                        //    cstValidator.Enabled = true;
                        //    spnInsPre.Attributes["class"] = "reqd";
                        //}
                        //else
                        //{
                        //    cstValidator.Enabled = false;
                        //    spnInsPre.Attributes["class"] = "controlHidden reqd";
                        //}

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

        #region Button Events
        protected void fsucCmdBarButton_SearchClick(object sender, EventArgs e)
        {
            try
            {
                cmdAssignBtns.Visible = true;
                grdRotations.Visible = true;
                CurrentViewContext.IsReset = false;
                //UAT-2034:
                CurrentViewContext.DicOfSelectedRotation = new Dictionary<Int32, Int32>();
                CurrentViewContext.SelectedRotationIDForCloning = AppConsts.NONE;
                ResetGridFilters();
                grdRotations.Rebind();
                //UAT-2696
                ColumnsConfiguration.BindPageControls();
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

        protected void fsucCmdBarButton_ResetClick(object sender, EventArgs e)
        {
            try
            {
                CurrentViewContext.IsReset = true;
                ResetTenant();
                ResetControls();
                ResetGridFilters();
                ucAgencyHierarchyMultipleToSearchRotation.Reset();
                /*UAT-3032*/
                //GetPreferredSelectedTenant();
                /*END UAT-3032*/
                //UAT-2696
                ColumnsConfiguration.BindPageControls();
                //IsPreferredDefaultTenantSelected = true;
                //GetPreferredSelectedTenant();
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

        protected void fsucCmdBarButton_CancelClick(object sender, EventArgs e)
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

        protected void lnkRemove_Click(object sender, EventArgs e)
        {
            try
            {
                // sender.na
                GridEditFormItem editForm = (sender as LinkButton).NamingContainer as GridEditFormItem;
                if (editForm.IsNotNull())
                {
                    WclAsyncUpload fileUpload = editForm.FindControl("uploadControl") as WclAsyncUpload;
                    Label lblUploadFormName = editForm.FindControl("lblUploadFormName") as Label;
                    LinkButton lnkRemove = editForm.FindControl("lnkRemove") as LinkButton;
                    lblUploadFormName.Visible = false;
                    lblUploadFormName.Text = String.Empty;
                    fileUpload.Visible = true;
                    lnkRemove.Visible = false;
                }
                hdnFileRemoved.Value = true.ToString();
                WclComboBox ddlTenant = editForm.FindControl("ddlTenant") as WclComboBox;
                // AgencyHierarchySelection ucAgencyHierarchyAddEditRotation = editForm.FindControl("ucAgencyHierarchyAddEditRotation") as AgencyHierarchySelection;
                String agencyHierarchyIDs = String.Empty;
                String agencyIDs = String.Empty;

                AgencyHierarchyMultipleSelection ucAgencyHierarchyAddRotationMultiple = editForm.FindControl("ucAgencyHierarchyAddRotationMultiple") as AgencyHierarchyMultipleSelection;
                AgencyhierarchyCollection ucAgencyHierarchyAddEditRotationCollection = ucAgencyHierarchyAddRotationMultiple.GetAgencyHierarchyCollection();
                if (ucAgencyHierarchyAddEditRotationCollection.IsNotNull() && ucAgencyHierarchyAddEditRotationCollection.agencyhierarchy.IsNotNull())
                {
                    agencyHierarchyIDs = String.Join(",", ucAgencyHierarchyAddEditRotationCollection.agencyhierarchy.Select(d => d.AgencyNodeID).Distinct().ToList());
                    agencyIDs = String.Join(",", ucAgencyHierarchyAddEditRotationCollection.agencyhierarchy.Select(d => d.AgencyID).Distinct().ToList());
                }
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "GetRotationRequiredFieldOptions('" + Convert.ToInt32(ddlTenant.SelectedValue) + "','" + agencyIDs + "');", true);
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

        #region UAT-2424

        protected void btnClone_Click(object sender, EventArgs e)
        {

            GridEditFormInsertItem insertItem = (sender as WclButton).NamingContainer as GridEditFormInsertItem;
            WclComboBox ddlTenant = insertItem.FindControl("ddlTenant") as WclComboBox;
            WclComboBox ddlRotation = insertItem.FindControl("ddlRotation") as WclComboBox;

            CurrentViewContext.SelectedRotationIDForCloning = Convert.ToInt32(ddlRotation.SelectedValue); //Set Property
            CurrentViewContext.SelectedTenantIDForAddForm = Convert.ToInt32(ddlTenant.SelectedValue);

            CloneContract = Presenter.GetClinicalRotationDetailsByID();  //GetDetails for the Rotation that needs to be cloned

            #region Get Grid Controls

            AgencyHierarchyMultipleSelection ucAgencyHierarchyAddEditRotationMultiple = insertItem.FindControl("ucAgencyHierarchyAddRotationMultiple") as AgencyHierarchyMultipleSelection;
            WclComboBox ddlAgency = insertItem.FindControl("ddlAgency") as WclComboBox;
            WclDatePicker dpStartDate = insertItem.FindControl("dpStartDate") as WclDatePicker;
            WclDatePicker dpEndDate = insertItem.FindControl("dpEndDate") as WclDatePicker;
            WclTimePicker tpStartTime = insertItem.FindControl("tpStartTime") as WclTimePicker;
            WclTimePicker tpEndTime = insertItem.FindControl("tpEndTime") as WclTimePicker;
            HtmlGenericControl dvComplioId = insertItem.FindControl("dvComplioId") as HtmlGenericControl;
            WclAsyncUpload fileUpload = insertItem.FindControl("uploadControl") as WclAsyncUpload;
            Label lblUploadFormName = insertItem.FindControl("lblUploadFormName") as Label;
            Label lblUploadFormPath = insertItem.FindControl("lblUploadFormPath") as Label;
            LinkButton lnkRemove = insertItem.FindControl("lnkRemove") as LinkButton;
            WclDatePicker dpDeadlineDate = insertItem.FindControl("dpDeadlineDate") as WclDatePicker;
            WclNumericTextBox txtDaysBefore = insertItem.FindControl("txtDaysBefore") as WclNumericTextBox;
            WclNumericTextBox txtFrequency = insertItem.FindControl("txtFrequency") as WclNumericTextBox;
            WclComboBox cmbContacts = insertItem.FindControl("ddlInstructor") as WclComboBox;
            WclComboBox cmbDays = insertItem.FindControl("ddlDays") as WclComboBox;
            WclTextBox txtRotationName = insertItem.FindControl("txtClassification") as WclTextBox;
            WclTextBox txtDepartment = insertItem.FindControl("txtDepartment") as WclTextBox;
            WclTextBox txtProgram = insertItem.FindControl("txtProgram") as WclTextBox;
            WclTextBox txtTypeSpecialty = insertItem.FindControl("txtTypeSpecialty") as WclTextBox;
            WclTextBox txtComplioId = insertItem.FindControl("txtComplioId") as WclTextBox;
            WclTextBox txtTerm = insertItem.FindControl("txtTerm") as WclTextBox;
            WclTextBox txtCourse = insertItem.FindControl("txtCourse") as WclTextBox;
            WclTextBox txtUnit = insertItem.FindControl("txtUnit") as WclTextBox;
            WclNumericTextBox txtStudents = insertItem.FindControl("txtStudents") as WclNumericTextBox;
            WclNumericTextBox txtRecommendedHrs = insertItem.FindControl("txtRecommendedHrs") as WclNumericTextBox;
            WclTextBox txtShift = insertItem.FindControl("txtShift") as WclTextBox;
            WclButton chkAllowNotification = insertItem.FindControl("chkAllowNotification") as WclButton; //UAT-2905 
            CheckBoxList chkEditPermission = insertItem.FindControl("chkEditPermission") as CheckBoxList; //UAT 3041
            #endregion

            if (CloneContract != null)
            {
                txtRotationName.Text = CloneContract.RotationName;
                txtComplioId.Text = CloneContract.ComplioID;
                txtCourse.Text = CloneContract.Course;
                txtDepartment.Text = CloneContract.Department;
                txtProgram.Text = CloneContract.Program;
                txtRecommendedHrs.Text = CloneContract.RecommendedHours.ToString();
                txtShift.Text = CloneContract.Shift;
                txtTypeSpecialty.Text = CloneContract.TypeSpecialty;
                txtUnit.Text = CloneContract.UnitFloorLoc;
                txtTerm.Text = CloneContract.Term;
                txtStudents.Text = CloneContract.Students.ToString();
                dvComplioId.Visible = true;
                //ddlAgency.SelectedValue = CloneContract.AgencyID.ToString();

                String formattedAgencyIds = String.Empty;
                List<Int32> agencyIds = new List<Int32>();
                if (!CloneContract.AgencyIDs.IsNullOrEmpty())
                {
                    agencyIds = CloneContract.AgencyIDs.Split(',').Select(t => Int32.Parse(t) * -1).ToList();
                    formattedAgencyIds = String.Join(",", agencyIds);
                }

                ucAgencyHierarchyAddEditRotationMultiple.SelectedAgecnyIds = formattedAgencyIds;
                ucAgencyHierarchyAddEditRotationMultiple.SelectedNodeIds = CloneContract.AgencyHierarchyIDs;
                ucAgencyHierarchyAddEditRotationMultiple.SelectedRootNodeId = CloneContract.RootNodeID;
                Session["NodeSelected"] = CloneContract.AgencyHierarchyIDs;
                Session["AgencySelected"] = formattedAgencyIds;

                //UAT-3241
                List<String> selectedAgencyNames = Presenter.GetAgencyNamesByIds(CloneContract.AgencyIDs.Split(',').ConvertIntoIntList());
                String agencyNames = string.Join(", ", selectedAgencyNames);

                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "GetRotationRequiredFieldOptions('" + Convert.ToInt32(ddlTenant.SelectedValue) + "','" + CloneContract.AgencyIDs + "','" + agencyNames + "');", true);
                //ucAgencyHierarchyAddEditRotationMultiple. = string.Empty;
                //ucAgencyHierarchyAddEditRotationMultiple. = string.Empty;
                ucAgencyHierarchyAddEditRotationMultiple.IsInstitutionHierarchyRequired = true;

                if (!CloneContract.HierarchyNodeIDList.IsNullOrEmpty())
                    ucAgencyHierarchyAddEditRotationMultiple.SelectedInstitutionNodeIds = Convert.ToString(CloneContract.HierarchyNodeIDList);

                dpStartDate.SelectedDate = CloneContract.StartDate;
                dpEndDate.SelectedDate = CloneContract.EndDate;
                tpStartTime.SelectedTime = CloneContract.StartTime;
                tpEndTime.SelectedTime = CloneContract.EndTime;
                if (!dpStartDate.SelectedDate.IsNullOrEmpty())
                {
                    hdnCurrentRotStartDate.Value = dpStartDate.SelectedDate.Value.ToString();
                }
                dpDeadlineDate.SelectedDate = CloneContract.DeadlineDate;
                //UAT-2905
                chkAllowNotification.Checked = CloneContract.IsAllowNotification;
                //UAT 3041 
                foreach (ListItem item in chkEditPermission.Items)
                {
                    if (item.Value.ToString() == "CA")
                    {
                        item.Selected = CloneContract.IsEditableByClientAdmin;
                    }
                    else if (item.Value.ToString() == "AGU")
                    {
                        item.Selected = CloneContract.IsEditableByAgencyUser;
                    }
                }

                txtDaysBefore.Text = CloneContract.DaysBefore.ToString();
                txtFrequency.Text = CloneContract.Frequency;
                if (!CloneContract.ContactIdList.IsNullOrEmpty())
                {
                    String[] selectedContactIds = CloneContract.ContactIdList.Split(',');
                    foreach (RadComboBoxItem item in cmbContacts.Items)
                    {
                        item.Checked = selectedContactIds.Contains(item.Value);
                    }
                }
                else
                {
                    cmbContacts.ClearCheckedItems();
                }
                if (!CloneContract.DaysIdList.IsNullOrEmpty())
                {
                    String[] selectedDayIds = CloneContract.DaysIdList.Split(',');
                    foreach (RadComboBoxItem item in cmbDays.Items)
                    {
                        item.Checked = selectedDayIds.Contains(item.Value);
                    }
                }
                else
                {
                    cmbDays.ClearCheckedItems();
                }
                if (!CloneContract.SyllabusFileName.IsNullOrEmpty())
                {
                    lblUploadFormName.Text = CloneContract.SyllabusFileName;
                    lblUploadFormPath.Text = CloneContract.SyllabusFilePath;
                    lblUploadFormName.Visible = true;
                    lnkRemove.Visible = true;
                    fileUpload.Visible = false;
                }
                else
                {
                    lblUploadFormName.Text = "";
                    lblUploadFormPath.Text = "";
                    fileUpload.Visible = true;
                    lblUploadFormName.Visible = false;
                    lnkRemove.Visible = false;

                }

                hdnDepartmentProgmapNew.Value = CloneContract.HierarchyNodeIDList;
                hdnInstNodeIdNew.Value = CloneContract.HierarchyNodeIDList;
                (insertItem.FindControl("lblInstitutionHierarchyPB") as Label).Text = CloneContract.HierarchyNodes;
                Presenter.GetCustomAttributeList(CurrentViewContext.SelectedRotationIDForCloning);
                if (!CurrentViewContext.GetCustomAttributeList.IsNullOrEmpty())
                {
                    Panel pnlEditForm = insertItem.FindControl("pnlEditForm") as Panel;
                    SharedUserCustomAttributeForm caCustomAttributesExisting = pnlEditForm.FindControl("caCustomAttributes") as SharedUserCustomAttributeForm;
                    pnlEditForm.Controls.Remove(caCustomAttributesExisting);//First remove the control of custome attributes that is already added in itemcreated event
                    SharedUserCustomAttributeForm caCustomAttributes = (SharedUserCustomAttributeForm)Page.LoadControl("~\\ClinicalRotation\\UserControl\\SharedUserCustomAttributeForm.ascx");
                    caCustomAttributes.ID = "caCustomAttributes";
                    GenerateCustomAttributes(caCustomAttributes);
                    pnlEditForm.Controls.Add(caCustomAttributes);//Add custom attribute control again with the values associated with rotation id (that is to be cloned)
                }
                CurrentViewContext.SelectedAgencyIDForAddForm = CloneContract.AgencyID;
                //CustomValidator cstValidator = insertItem.FindControl("cstValidator") as CustomValidator;
                //HtmlGenericControl spnInsPre = insertItem.FindControl("spnInsPre") as HtmlGenericControl;
                //if (Presenter.IsPreceptorRequiredForAgency()) //UAT-2554
                //{
                //    cstValidator.Enabled = true;
                //    spnInsPre.Attributes["class"] = "reqd";
                //}
                //else
                //{
                //    cstValidator.Enabled = false;
                //    spnInsPre.Attributes["class"] = "controlHidden reqd";
                //} 
            }
        }

        protected void btnRelod_Click(object sender, EventArgs e)
        {

            if (!hdnRotationId.Value.IsNullOrEmpty() && hdnRotationId.Value == AppConsts.MINUS_ONE.ToString())
            {
                if (!Session[AppConsts.ROTATION_DETAIL_SESSION_KEY].IsNullOrEmpty())
                {
                    Session[AppConsts.ROTATION_DETAIL_SESSION_KEY] = null;
                }
                // base.ShowSuccessMessage("Clinical rotation saved successfully.");
                //System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ConfirmationMessage('Clinical rotation saved successfully.','sucs');", true);

                //UAT-3053 : As an admin, after creating a rotation I should be directed to the rotation details screen of that rotation.
                String RotationId = hdnClinicalRotationId.Value;
                //String AgencyId = Convert.ToString(CurrentViewContext.ViewContract.AgencyID);
                String AgencyId = hdnAgencyId.Value;

                String TenantId = hdnTenantId.Value;

                //UAT-3041
                String IsEditableByClientAdmin = hdnIsEditableByClientAdmin.Value;
                String IsEditableByAgencyUser = hdnIsEditableByAgencyUser.Value;
                CurrentViewContext.IsApplicantPkgNotAssignedThroughCloning = Convert.ToBoolean(hdnIsApplicantPkgNotAssignedThroughCloning.Value);
                CurrentViewContext.IsInstructorPkgNotAssignedThroughCloning = Convert.ToBoolean(hdnIsInstructorPkgNotAssignedThroughCloning.Value);

                RedirectToDetail(RotationId, AgencyId, TenantId, true, IsEditableByClientAdmin, IsEditableByAgencyUser);
                //System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ConfirmationMessage('Clinical rotation saved successfully.','sucs');", true);
            }
            else if (!hdnRotationId.Value.IsNullOrEmpty() && hdnRotationId.Value != AppConsts.MINUS_ONE.ToString())
            {
                grdRotations.Rebind();

                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ConfirmationMessage('Clinical rotation updated successfully.','sucs');", true);
            }

        }
        #endregion

        #region UAT-2034:
        protected void cmdAssignBtns_AssignStudentPkgClick(object sender, EventArgs e)
        {
            try
            {
                RedirectToManageRotationAssignments(RotationAssignmentType.ASSIGN_STUDENT_PACKAGES.GetStringValue());
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

        protected void cmdAssignBtns_AssignPreceptorPkgClick(object sender, EventArgs e)
        {
            try
            {
                RedirectToManageRotationAssignments(RotationAssignmentType.ASSIGN_PRECEPTOR_PACKAGES.GetStringValue());
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

        protected void cmdAssignBtns_AssignPreceptorClick(object sender, EventArgs e)
        {
            try
            {
                RedirectToManageRotationAssignments(RotationAssignmentType.ASSIGN_PRECEPTOR.GetStringValue());
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

        protected void cmdAssignBtns_UploadSyllabusClick(object sender, EventArgs e)
        {
            try
            {
                RedirectToManageRotationAssignments(RotationAssignmentType.UPLOAD_SYLLABUS.GetStringValue());
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

        #region UAT-2545 - Rotation Archive Functionality
        protected void btnArchive_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CurrentViewContext.DicOfSelectedRotation.IsNullOrEmpty())
                {
                    Boolean result = Presenter.ArchiveSelectedRotation();
                    if (result)
                    {
                        ShowSuccessMessage("Rotation(s) archived successfully.");
                        grdRotations.Rebind();
                    }
                }
                else
                {
                    ShowInfoMessage("Please select rotation(s) to archive.");
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

        #region UAT-3138 - Rotation UnArchive Functionality
        protected void btnUnArchive_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CurrentViewContext.DicOfSelectedRotation.IsNullOrEmpty())
                {
                    if (Presenter.UnArchiveSelectedRotation())
                    {
                        ShowSuccessMessage("Rotation(s) UnArchived successfully.");
                        grdRotations.Rebind();
                    }
                    else
                    {
                        ShowErrorMessage("Some error occur. Please contact system administrator or try again later.");
                    }
                }
                else
                {
                    ShowInfoMessage("Please select rotation(s) to UnArchive.");
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

        #region Dropdown events
        protected void ddlTenantName_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                ViewState["IsBind"] = null;
                ResetControls();

                BindArchiveFilter();
                //ResetGridFilters(); //UAT-3874
                ucAgencyHierarchyMultipleToSearchRotation.Reset();
                BindRotationReviewStatus();
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


        protected void ddlTenantName_DataBound(object sender, EventArgs e)
        {
            try
            {
                ddlTenantName.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT--"));
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

        //protected void ddlAgency_DataBound(object sender, EventArgs e)  [ddl]
        //{
        //    try
        //    {
        //        ddlAgency.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT--"));
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

        protected void ddlTenant_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                GridEditFormInsertItem insertItem = (sender as WclComboBox).NamingContainer as GridEditFormInsertItem;
                String selectedValue = (sender as WclComboBox).SelectedValue;
                CurrentViewContext.SelectedTenantIDForAddForm = selectedValue.IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(selectedValue);
                WclComboBox ddlAgency = insertItem.FindControl("ddlAgency") as WclComboBox;
                //BindAgencyForAddForm(ddlAgency);

                //AgencyHierarchySelection ucAgencyHierarchyAddEditRotation = insertItem.FindControl("ucAgencyHierarchyAddEditRotation") as AgencyHierarchySelection;
                //ucAgencyHierarchyAddEditRotation.TenantId = CurrentViewContext.SelectedTenantIDForAddForm;

                AgencyHierarchyMultipleSelection ucAgencyHierarchyAddEditRotationMultiple = insertItem.FindControl("ucAgencyHierarchyAddRotationMultiple") as AgencyHierarchyMultipleSelection;
                ucAgencyHierarchyAddEditRotationMultiple.TenantId = CurrentViewContext.SelectedTenantIDForAddForm == 0 ? -1 : CurrentViewContext.SelectedTenantIDForAddForm;

                if (CurrentViewContext.SelectedAgencyIDForAddForm > AppConsts.NONE)
                {
                    //    ucAgencyHierarchyAddEditRotation.AgencyId = CurrentViewContext.SelectedAgencyIDForAddForm;
                }
                else
                {
                    //  ucAgencyHierarchyAddEditRotation.AgencyId = 0;
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ResetAgencyHierarchySelection();", true);
                }

                WclComboBox cmbDays = insertItem.FindControl("ddlDays") as WclComboBox;
                BindWeekDaysForAddForm(cmbDays);

                WclComboBox cmbContacts = insertItem.FindControl("ddlInstructor") as WclComboBox;
                BindContactssForAddForm(cmbContacts);

                WclComboBox ddlRotation = insertItem.FindControl("ddlRotation") as WclComboBox;
                BindRotationForAddForm(ddlRotation);
                if (CurrentViewContext.SelectedTenantIDForAddForm > AppConsts.NONE)
                {
                    Int32? rotationID = null;
                    Presenter.GetCustomAttributeList(rotationID);
                    if (!CurrentViewContext.GetCustomAttributeList.IsNullOrEmpty())
                    {
                        Panel pnlEditForm = insertItem.FindControl("pnlEditForm") as Panel;
                        SharedUserCustomAttributeForm caCustomAttributesExisting = pnlEditForm.FindControl("caCustomAttributes") as SharedUserCustomAttributeForm;
                        pnlEditForm.Controls.Remove(caCustomAttributesExisting); //First remove the control of custome attributes that is already added 
                        SharedUserCustomAttributeForm caCustomAttributes = (SharedUserCustomAttributeForm)Page.LoadControl("~\\ClinicalRotation\\UserControl\\SharedUserCustomAttributeForm.ascx");
                        GenerateCustomAttributes(caCustomAttributes);
                        pnlEditForm.Controls.Add(caCustomAttributes);//Add the control again that have the updated custom attributes of changed Tenant
                    }
                }

                #region UAT-2424 : Clear all Edit Panel controls of grid
                //Get All Grid Edit Panel Controls
                WclDatePicker dpStartDate = insertItem.FindControl("dpStartDate") as WclDatePicker;
                WclDatePicker dpEndDate = insertItem.FindControl("dpEndDate") as WclDatePicker;
                WclTimePicker tpStartTime = insertItem.FindControl("tpStartTime") as WclTimePicker;
                WclTimePicker tpEndTime = insertItem.FindControl("tpEndTime") as WclTimePicker;
                HtmlGenericControl dvComplioId = insertItem.FindControl("dvComplioId") as HtmlGenericControl;
                WclAsyncUpload fileUpload = insertItem.FindControl("uploadControl") as WclAsyncUpload;
                Label lblUploadFormName = insertItem.FindControl("lblUploadFormName") as Label;
                Label lblUploadFormPath = insertItem.FindControl("lblUploadFormPath") as Label;
                LinkButton lnkRemove = insertItem.FindControl("lnkRemove") as LinkButton;
                WclDatePicker dpDeadlineDate = insertItem.FindControl("dpDeadlineDate") as WclDatePicker;
                WclNumericTextBox txtDaysBefore = insertItem.FindControl("txtDaysBefore") as WclNumericTextBox;
                WclNumericTextBox txtFrequency = insertItem.FindControl("txtFrequency") as WclNumericTextBox;
                WclTextBox txtRotationName = insertItem.FindControl("txtClassification") as WclTextBox;
                WclTextBox txtDepartment = insertItem.FindControl("txtDepartment") as WclTextBox;
                WclTextBox txtProgram = insertItem.FindControl("txtProgram") as WclTextBox;
                WclTextBox txtTypeSpecialty = insertItem.FindControl("txtTypeSpecialty") as WclTextBox;
                WclTextBox txtComplioId = insertItem.FindControl("txtComplioId") as WclTextBox;
                WclTextBox txtTerm = insertItem.FindControl("txtTerm") as WclTextBox;
                WclTextBox txtCourse = insertItem.FindControl("txtCourse") as WclTextBox;
                WclTextBox txtUnit = insertItem.FindControl("txtUnit") as WclTextBox;
                WclNumericTextBox txtStudents = insertItem.FindControl("txtStudents") as WclNumericTextBox;
                WclNumericTextBox txtRecommendedHrs = insertItem.FindControl("txtRecommendedHrs") as WclNumericTextBox;
                WclTextBox txtShift = insertItem.FindControl("txtShift") as WclTextBox;
                WclButton chkAllowNotification = insertItem.FindControl("chkAllowNotification") as WclButton; //UAT-2905

                //Clear Control Values
                CurrentViewContext.SelectedAgencyIDs = string.Empty;
                txtComplioId.Text = String.Empty;
                txtRotationName.Text = String.Empty;
                txtDepartment.Text = String.Empty;
                txtProgram.Text = String.Empty;
                txtCourse.Text = String.Empty;
                txtUnit.Text = String.Empty;
                txtRecommendedHrs.Text = String.Empty;
                txtStudents.Text = String.Empty;
                txtShift.Text = String.Empty;
                txtDaysBefore.Text = String.Empty;
                txtFrequency.Text = String.Empty;
                dpStartDate.Clear();
                dpEndDate.Clear();
                tpStartTime.Clear();
                tpEndTime.Clear();
                dpDeadlineDate.Clear();
                ddlDays.ClearCheckedItems();
                ddlContacts.ClearCheckedItems();
                ddlRotationReviewStatus.ClearCheckedItems();
                txtTerm.Text = String.Empty;
                txtTypeSpecialty.Text = String.Empty;
                hdnInstNodeIdNew.Value = String.Empty;
                chkAllowNotification.Checked = true; //UAT-2905
                #endregion

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

        #region UAT-2554
        protected void ddlAgency_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            GridEditFormInsertItem insertItem = (sender as WclComboBox).NamingContainer as GridEditFormInsertItem;
            String selectedValue = (sender as WclComboBox).SelectedValue;
            CurrentViewContext.SelectedAgencyIDForAddForm = Convert.ToInt32(selectedValue);
            CustomValidator cstValidator = insertItem.FindControl("cstValidator") as CustomValidator;
            HtmlGenericControl spnInsPre = insertItem.FindControl("spnInsPre") as HtmlGenericControl; //Make conditionally aestrik visible in case instructor preceptor required 
            if (Presenter.IsPreceptorRequiredForAgency())
            {
                cstValidator.Enabled = true;
                spnInsPre.Visible = true;
            }
            else
            {
                cstValidator.Enabled = false;
                spnInsPre.Visible = false;
            }
        }
        #endregion

        #region UAT-2034:Phase 4 (16): Manage Rotation screen updates
        /// <summary>
        /// Handel selected checkboxes 
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        protected void chkSelectRotation_CheckedChanged(object sender, EventArgs e)
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
                if (CurrentViewContext.DicOfSelectedRotation.IsNull())
                {
                    CurrentViewContext.DicOfSelectedRotation = new Dictionary<Int32, Int32>();
                }

                Dictionary<Int32, Int32> selectedRotationDic = CurrentViewContext.DicOfSelectedRotation;
                Int32 rotationID = (Int32)dataItem.GetDataKeyValue("RotationID");
                Int32 agencyID = (Int32)dataItem.GetDataKeyValue("AgencyID");
                isChecked = ((CheckBox)dataItem.FindControl("chkSelectRotation")).Checked;

                if (!selectedRotationDic.ContainsKey(rotationID) && isChecked)
                {
                    selectedRotationDic.Add(rotationID, agencyID);
                }
                else if (selectedRotationDic.ContainsKey(rotationID) && !isChecked)
                {
                    selectedRotationDic.Remove(rotationID);
                }
                CurrentViewContext.DicOfSelectedRotation = selectedRotationDic;
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

        #region Radiobutton events
        protected void rbArchiveStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (rbArchiveStatus.SelectedValue == ArchiveState.Archived.GetStringValue())
            //    btnArchive.Enabled = false;
            //else
            //    btnArchive.Enabled = true;
            //UAT-3138
            if (rbArchiveStatus.SelectedValue == ArchiveState.Archived.GetStringValue())
            {
                //btnArchieve.Enabled = false;
                ShowHideControl("Archivemun", "btnArchive", false);

                ShowHideControl("Archivemun", "btnUnArchive", true);
            }
            else if (rbArchiveStatus.SelectedValue == ArchiveState.Active.GetStringValue())
            {
                ShowHideControl("Archivemun", "btnArchive", true);

                ShowHideControl("Archivemun", "btnUnArchive", false);
            }
            else if (rbArchiveStatus.SelectedValue == ArchiveState.All.GetStringValue())
            {
                ShowHideControl("Archivemun", "btnArchive", true);

                ShowHideControl("Archivemun", "btnUnArchive", true);
            }
        }
        #endregion

        #region ShowHideControl method
        private void ShowHideControl(String menuText, String controlID, Boolean isVisible)
        {
            RadMenuItem menuItem = cmdArchive.FindItemByText(menuText);

            foreach (RadMenuItem item in menuItem.Items)
            {
                RadButton btnMenu = (RadButton)item.FindControl(controlID);
                if (btnMenu.IsNotNull())
                {
                    btnMenu.Visible = isVisible;
                }
            }
        }
        #endregion

        #region Private Methods
        private void BindArchiveFilter()
        {
            if (!ddlTenantName.SelectedValue.IsNullOrEmpty())
            {
                rbArchiveStatus.Visible = true;
                //btnArchive.Enabled = true; //UAT-3138
                if (rbArchiveStatus.SelectedValue == ArchiveState.Active.GetStringValue())
                {
                    ShowHideControl("Archivemun", "btnArchive", true);
                    ShowHideControl("Archivemun", "btnUnArchive", false);
                }
                Presenter.GetArchiveStateList();
            }
            else
            {
                rbArchiveStatus.Visible = false;
            }
        }

        /// <summary>
        /// Method to Bind Tenant Dropdown and call to Bind UserGroup and Agency Dropdown
        /// </summary>
        private void BindControls()
        {
            BindRotationReviewStatus();
            BindContacts();
            BindWeekDays();
        }

        private void BindTenant()
        {
            ddlTenantName.DataSource = CurrentViewContext.lstTenant;
            ddlTenantName.DataBind();
            Presenter.IsAdminLoggedIn();
            if (CurrentViewContext.IsAdminLoggedIn)
            {
                ddlTenantName.Enabled = true;
                CurrentViewContext.SelectedTenantID = 0;
            }
            else
            {
                CurrentViewContext.SelectedTenantID = CurrentViewContext.TenantID;
            }
        }

        /// <summary>
        /// Method to Bind Agencies
        /// </summary>
        //private void BindAgency()  [ddl]
        //{
        //    Presenter.GetAllAgency();
        //    ddlAgency.DataSource = CurrentViewContext.lstAgency;
        //    ddlAgency.DataBind();
        //}

        /// <summary>
        /// Method to Bind Agencies
        /// </summary>
        private void BindContacts()
        {
            Presenter.GetClientContacts();
            ddlContacts.DataSource = CurrentViewContext.ClientContactList;
            ddlContacts.DataBind();
        }

        private void BindWeekDays()
        {
            Presenter.GetWeekDays();
            ddlDays.DataSource = CurrentViewContext.WeekDayList;
            ddlDays.DataBind();
        }

        private void BindRotationReviewStatus()
        {
            if (!ddlTenantName.SelectedValue.IsNullOrEmpty())
            {
                Presenter.GetRotationReviewStatus();
                ddlRotationReviewStatus.DataSource = CurrentViewContext.lstRotationReviewStatus;
                ddlRotationReviewStatus.DataBind();
                ddlRotationReviewStatus.Visible = true;
            }
            else
            {
                ddlRotationReviewStatus.Visible = false;
            }

        }

        /// <summary>
        /// Method to Reset Grid 
        /// </summary>
        private void ResetGridFilters()
        {
            grdRotations.MasterTableView.SortExpressions.Clear();
            grdRotations.CurrentPageIndex = 0;
            grdRotations.MasterTableView.CurrentPageIndex = 0;
            grdRotations.MasterTableView.IsItemInserted = false;
            grdRotations.MasterTableView.ClearEditItems();
            grdRotations.Rebind();
        }

        private void ResetTenant()
        {
            Presenter.IsAdminLoggedIn();
            if (CurrentViewContext.IsAdminLoggedIn)
            {
                CurrentViewContext.SelectedTenantID = AppConsts.NONE;

                ucAgencyHierarchyMultipleToSearchRotation.TenantId = CurrentViewContext.SelectedTenantID == AppConsts.NONE ? AppConsts.MINUS_ONE : CurrentViewContext.SelectedTenantID;
                //UAT-1778
                //SharedUserCustomAttributeForm caCustomAttributesID = Page.FindServerControlRecursively("caCustomAttributesID") as SharedUserCustomAttributeForm;
                if (caCustomAttributesID.IsNotNull())
                    caCustomAttributesID.Reset();
                /*UAT-3032*/
                IsPreferredDefaultTenantSelected = true;
                GetPreferredSelectedTenant();
                /*END UAT-3032*/
            }
            else
            {
                CurrentViewContext.SelectedTenantID = CurrentViewContext.TenantID;
            }
        }
        private void ResetControls()
        {
            BindControls();
            CurrentViewContext.SelectedAgencyIDs = string.Empty;
            //ucAgencyHierarchyMultipleToSearchRotation.Label = String.Empty;
            txtComplioId.Text = String.Empty;
            txtRotationName.Text = String.Empty;
            txtDepartment.Text = String.Empty;
            txtProgram.Text = String.Empty;
            txtCourse.Text = String.Empty;
            txtUnit.Text = String.Empty;
            txtRecommendedHrs.Text = String.Empty;
            //UAT-1769 Addition of "# of Students" field on rotation creation and rotation details for all except students
            txtStudents.Text = String.Empty;
            txtShift.Text = String.Empty;
            dpStartDate.Clear();
            dpEndDate.Clear();
            tpStartTime.Clear();
            tpEndTime.Clear();
            ddlDays.ClearCheckedItems();
            ddlContacts.ClearCheckedItems();
            ShowHideControl("Archivemun", "btnArchive", true); //UAT-3138
            ShowHideControl("Archivemun", "btnUnArchive", false); //UAT-3138
            ddlRotationReviewStatus.ClearCheckedItems();
            //UAT 1355 Addition of Term field to the Create rotation, rotation details screens
            txtTerm.Text = String.Empty;
            //UAT-1467: Addition of "Type/Specialty" field on rotation creation and rotation details
            txtTypeSpecialty.Text = String.Empty;
            hdnInstNodeIdNew.Value = String.Empty;
            hdnRotationId.Value = String.Empty;
            hdnIsEditableByClientAdmin.Value = String.Empty;
            hdnIsEditableByAgencyUser.Value = String.Empty;
            hdnAgencyId.Value = String.Empty;
            hdnClinicalRotationId.Value = String.Empty;
            hdnIsApplicantPkgNotAssignedThroughCloning.Value = String.Empty;
            hdnIsInstructorPkgNotAssignedThroughCloning.Value = String.Empty;

            //UAT-1778
            caCustomAttributesID.ResetCustomAttributes();
            if (!ddlTenantName.SelectedValue.IsNullOrEmpty())
            {
                rbArchiveStatus.Visible = true;
                Presenter.GetArchiveStateList();
                //btnArchive.Enabled = true;   //UAT-3138
            }
            else
            {
                rbArchiveStatus.Visible = false;
                // btnArchive.Enabled = true; //UAT-3138
            }
            //UAT-2034:
            CurrentViewContext.DicOfSelectedRotation = new Dictionary<Int32, Int32>();

            hdnDepartmntPrgrmMppng.Value = string.Empty;
            hdnHierarchyLabel.Value = string.Empty;
            hdnInstitutionNodeId.Value = string.Empty;
            lblinstituteHierarchy.Text = string.Empty;
        }

        void GenerateCustomAttributes(SharedUserCustomAttributeForm caCustomAttributes)
        {
            // Generate the control using database, but set the values from the session
            caCustomAttributes.TenantId = CurrentViewContext.SelectedTenantID;
            caCustomAttributes.TypeCode = CustomAttributeUseTypeContext.Hierarchy.GetStringValue();
            //caCustomAttributes.MappingRecordId = this.NodeId;
            //caCustomAttributes.ValueRecordId = this.DeptProgId;
            caCustomAttributes.DataSourceModeType = DataSourceMode.Ids;
            caCustomAttributes.Title = "Other Details";
            caCustomAttributes.ControlDisplayMode = DisplayMode.Controls;
            caCustomAttributes.CurrentLoggedInUserId = base.CurrentUserId;
            caCustomAttributes.ValidationGroup = "grpFormSubmit";
            caCustomAttributes.IsReadOnly = false;
            caCustomAttributes.lstTypeCustomAttributes = CurrentViewContext.GetCustomAttributeList;
        }

        /// <summary>
        /// Get search parameters
        /// </summary>
        private void GetSearchParameters()
        {
            _searchContract = new ClinicalRotationDetailContract();
            _searchContract.AgencyIDs = CurrentViewContext.SelectedAgencyIDs;
            _searchContract.TenantID = CurrentViewContext.SelectedTenantID;
            if (!txtComplioId.Text.Trim().IsNullOrEmpty())
                _searchContract.ComplioID = txtComplioId.Text.Trim();
            if (!txtRotationName.Text.Trim().IsNullOrEmpty())
                _searchContract.RotationName = txtRotationName.Text.Trim();
            if (!txtDepartment.Text.Trim().IsNullOrEmpty())
                _searchContract.Department = txtDepartment.Text.Trim();
            if (!txtProgram.Text.Trim().Trim().IsNullOrEmpty())
                _searchContract.Program = txtProgram.Text.Trim();
            if (!txtCourse.Text.Trim().IsNullOrEmpty())
                _searchContract.Course = txtCourse.Text.Trim();
            if (!txtUnit.Text.Trim().IsNullOrEmpty())
                _searchContract.UnitFloorLoc = txtUnit.Text.Trim();
            if (!txtRecommendedHrs.Text.Trim().IsNullOrEmpty())
                _searchContract.RecommendedHours = float.Parse(txtRecommendedHrs.Text.Trim());
            //UAT-1769 Addition of "# of Students" field on rotation creation and rotation details for all except students
            if (!txtStudents.Text.Trim().IsNullOrEmpty())
                _searchContract.Students = float.Parse(txtStudents.Text.Trim());
            if (!txtShift.Text.Trim().IsNullOrEmpty())
                _searchContract.Shift = txtShift.Text.Trim();
            //UAT 1355 Addition of Term field to the Create rotation, rotation details screens
            if (!txtTerm.Text.Trim().IsNullOrEmpty())
                _searchContract.Term = txtTerm.Text.Trim();
            if (!dpStartDate.SelectedDate.IsNullOrEmpty())
                _searchContract.StartDate = dpStartDate.SelectedDate;
            if (!dpEndDate.SelectedDate.IsNullOrEmpty())
                _searchContract.EndDate = dpEndDate.SelectedDate;
            if (!tpStartTime.SelectedTime.IsNullOrEmpty())
                _searchContract.StartTime = tpStartTime.SelectedTime;
            if (!tpEndTime.SelectedTime.IsNullOrEmpty())
                _searchContract.EndTime = tpEndTime.SelectedTime;
            //UAT-1467: Addition of "Type/Specialty" field on rotation creation and rotation details
            if (!txtTypeSpecialty.Text.Trim().IsNullOrEmpty())
                _searchContract.TypeSpecialty = txtTypeSpecialty.Text.Trim();
            _searchContract.DaysIdList = String.Join(",", ddlDays.CheckedItems.Select(x => x.Value));
            _searchContract.ContactIdList = String.Join(",", ddlContacts.CheckedItems.Select(x => x.Value));
            _searchContract.RotationReviewStatusIdList = String.Join(",", ddlRotationReviewStatus.CheckedItems.Select(x => x.Value));
            //UAT-1778
            if (!CustomDataXML.IsNullOrEmpty())
            {
                _searchContract.CustomAttributes = CustomDataXML;
            }
        }

        /// <summary>
        /// Set search parameters
        /// </summary>
        private void SetSearchParameters()
        {
            CurrentViewContext.SelectedAgencyIDs = _searchContract.AgencyIDs;
            CurrentViewContext.SelectedTenantID = _searchContract.TenantID;
            txtComplioId.Text = _searchContract.ComplioID.IsNullOrEmpty() ? String.Empty : _searchContract.ComplioID;
            txtRotationName.Text = _searchContract.RotationName.IsNullOrEmpty() ? String.Empty : _searchContract.RotationName;
            txtDepartment.Text = _searchContract.Department.IsNullOrEmpty() ? String.Empty : _searchContract.Department;
            txtProgram.Text = _searchContract.Program.IsNullOrEmpty() ? String.Empty : _searchContract.Program;
            txtCourse.Text = _searchContract.Course.IsNullOrEmpty() ? String.Empty : _searchContract.Course;
            txtUnit.Text = _searchContract.UnitFloorLoc.IsNullOrEmpty() ? String.Empty : _searchContract.UnitFloorLoc;
            txtRecommendedHrs.Text = _searchContract.RecommendedHours.IsNullOrEmpty() ? String.Empty : _searchContract.RecommendedHours.ToString();
            //UAT-1769 Addition of "# of Students" field on rotation creation and rotation details for all except students
            txtStudents.Text = _searchContract.Students.IsNullOrEmpty() ? String.Empty : _searchContract.Students.ToString();
            txtShift.Text = _searchContract.Shift.IsNullOrEmpty() ? String.Empty : _searchContract.Shift;
            //UAT 1355 Addition of Term field to the Create rotation, rotation details screens
            txtTerm.Text = _searchContract.Term.IsNullOrEmpty() ? String.Empty : _searchContract.Term;
            dpStartDate.SelectedDate = _searchContract.StartDate;
            dpEndDate.SelectedDate = _searchContract.EndDate;
            tpStartTime.SelectedTime = _searchContract.StartTime;
            tpEndTime.SelectedTime = _searchContract.EndTime;
            //UAT-1467: Addition of "Type/Specialty" field on rotation creation and rotation details
            txtTypeSpecialty.Text = _searchContract.TypeSpecialty.IsNullOrEmpty() ? String.Empty : _searchContract.TypeSpecialty;

            String[] daysId = _searchContract.DaysIdList.Split(',');
            foreach (RadComboBoxItem item in ddlDays.Items)
            {
                item.Checked = daysId.Contains(item.Value);
            }

            String[] contactIds = _searchContract.ContactIdList.Split(',');
            foreach (RadComboBoxItem item in ddlContacts.Items)
            {
                item.Checked = contactIds.Contains(item.Value);
            }
            String[] rotationReviewStatusIds = _searchContract.RotationReviewStatusIdList.Split(',');
            foreach (RadComboBoxItem item in ddlRotationReviewStatus.Items)
            {
                item.Checked = rotationReviewStatusIds.Contains(item.Value);
            }
            //UAT-1778
            //SharedUserCustomAttributeForm caCustomAttributesID = Page.FindServerControlRecursively("caCustomAttributesID") as SharedUserCustomAttributeForm;
            if (caCustomAttributesID.IsNotNull())
                caCustomAttributesID.previousValues = _searchContract.CustomAttributes;
        }

        /// <summary>
        /// To set controls values in session
        /// </summary>
        private void SetSessionValues()
        {
            ManageRotationSearchContract searchDataContract = new ManageRotationSearchContract();
            CurrentViewContext.SearchContract.ArchieveStatusId = rbArchiveStatus.SelectedValue; //UAT-2545
            searchDataContract.SearchParameters = CurrentViewContext.SearchContract;

            searchDataContract.GridCustomPagingArguments = CurrentViewContext.GridCustomPaging;
            //UAT-2034:
            searchDataContract.DicOfSelectedRotation = CurrentViewContext.DicOfSelectedRotation;

            AgencyhierarchyCollection agencyhierarchyCollection = ucAgencyHierarchyMultipleToSearchRotation.GetAgencyHierarchyCollection();
            string agencyIDs = string.Empty;
            string nodeIds = string.Empty;

            if (!agencyhierarchyCollection.IsNullOrEmpty() && !agencyhierarchyCollection.agencyhierarchy.IsNullOrEmpty())
                agencyIDs = string.Join(",", agencyhierarchyCollection.agencyhierarchy.Select(d => d.AgencyID).Distinct().ToList());

            if (!agencyhierarchyCollection.IsNullOrEmpty() && !agencyhierarchyCollection.agencyhierarchy.IsNullOrEmpty())
                nodeIds = string.Join(",", agencyhierarchyCollection.agencyhierarchy.Select(d => d.AgencyNodeID).Distinct().ToList());

            //searchDataContract.SearchParameters.AgencyHierarchyID = ucAgencyHierarchy.NodeId;
            searchDataContract.SearchParameters.AgencyIDs = agencyIDs;
            searchDataContract.SearchParameters.RootNodeID = ucAgencyHierarchyMultipleToSearchRotation.SelectedRootNodeId;
            searchDataContract.SearchParameters.HierarchyNodeIDList = nodeIds;

            searchDataContract.SearchParameters.DPMIds = hdnDepartmntPrgrmMppng.Value;
            searchDataContract.SearchParameters.InstituteHierarchySelectedNode = hdnHierarchyLabel.Value;
            //Session for maintaining control values
            SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.ROTATION_SEARCH_SESSION_KEY, searchDataContract);
        }

        /// <summary>
        /// To get session values for controls
        /// </summary>
        private void GetSessionValues()
        {
            ManageRotationSearchContract searchDataContract = new ManageRotationSearchContract();
            if (Session[AppConsts.ROTATION_SEARCH_SESSION_KEY].IsNotNull())
            {
                searchDataContract = SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.ROTATION_SEARCH_SESSION_KEY) as ManageRotationSearchContract;
                CurrentViewContext.SearchContract = searchDataContract.SearchParameters;
                CurrentViewContext.GridCustomPaging = searchDataContract.GridCustomPagingArguments;
                //UAT-2034:
                CurrentViewContext.DicOfSelectedRotation = searchDataContract.DicOfSelectedRotation;
                rbArchiveStatus.SelectedValue = searchDataContract.SearchParameters.ArchieveStatusId;

                //fix for: After coming back from detail screen, 'Unarchive' button is not visible; when Radiobutton 'Archive' is selected.
                if (rbArchiveStatus.SelectedValue == ArchiveState.Archived.GetStringValue())
                {
                    //btnArchieve.Enabled = false;
                    ShowHideControl("Archivemun", "btnArchive", false);

                    ShowHideControl("Archivemun", "btnUnArchive", true);
                }
                else if (rbArchiveStatus.SelectedValue == ArchiveState.Active.GetStringValue())
                {
                    ShowHideControl("Archivemun", "btnArchive", true);

                    ShowHideControl("Archivemun", "btnUnArchive", false);
                }
                else if (rbArchiveStatus.SelectedValue == ArchiveState.All.GetStringValue())
                {
                    ShowHideControl("Archivemun", "btnArchive", true);

                    ShowHideControl("Archivemun", "btnUnArchive", true);
                }

                if (!searchDataContract.SearchParameters.AgencyIDs.IsNullOrEmpty())
                {
                    //ucAgencyHierarchyMultipleToSearchRotation.AgencyHierarchyIds = ;
                    ucAgencyHierarchyMultipleToSearchRotation.SelectedAgecnyIds = searchDataContract.SearchParameters.AgencyIDs;
                    ucAgencyHierarchyMultipleToSearchRotation.SelectedRootNodeId = searchDataContract.SearchParameters.RootNodeID;
                    ucAgencyHierarchyMultipleToSearchRotation.SelectedNodeIds = searchDataContract.SearchParameters.HierarchyNodeIDList;

                    //ucAgencyHierarchy.NodeId = searchDataContract.SearchParameters.AgencyHierarchyID;
                    //ucAgencyHierarchy.AgencyId = searchDataContract.SearchParameters.AgencyID;
                    //ucAgencyHierarchy.SelectedRootNodeId = searchDataContract.SearchParameters.RootNodeID;
                }
                if (!searchDataContract.SearchParameters.DPMIds.IsNullOrEmpty())
                {
                    hdnDepartmntPrgrmMppng.Value = searchDataContract.SearchParameters.DPMIds;
                    hdnHierarchyLabel.Value = searchDataContract.SearchParameters.InstituteHierarchySelectedNode;
                }
                grdRotations.Rebind();
                //Reset session
                Session[AppConsts.ROTATION_SEARCH_SESSION_KEY] = null;

            }
        }

        private void CaptureQuerystringParameters()
        {
            Dictionary<String, String> args = new Dictionary<String, String>();
            if (!Request.QueryString["args"].IsNull())
            {
                args.ToDecryptedQueryString(Request.QueryString["args"]);

                if (args.ContainsKey(ProfileSharingQryString.SelectedTenantId))
                {
                    CurrentViewContext.SelectedTenantID = Convert.ToInt32(args[ProfileSharingQryString.SelectedTenantId]);
                    if (!ddlTenantName.SelectedValue.IsNullOrEmpty())
                    {
                        Presenter.GetArchiveStateList();
                    }
                }
                if (args.ContainsKey(ProfileSharingQryString.AgencyId))
                {
                    CurrentViewContext.SelectedAgencyIDs = Convert.ToString(args[ProfileSharingQryString.AgencyId]);
                }
            }
            //if user navigate to other feature from detail screen and return to manage rotation again.
            else
                Session[AppConsts.ROTATION_SEARCH_SESSION_KEY] = null;
        }

        public String UploadSyllabusDocuments(WclAsyncUpload fileUpload)
        {

            String fileName = String.Empty;
            String savedFilePath = String.Empty;

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


            var item = fileUpload.UploadedFiles[0];
            fileName = Guid.NewGuid().ToString() + Path.GetExtension(item.FileName);
            //Save file on temp location
            String newTempFilePath = Path.Combine(tempFilePath, fileName);
            item.SaveAs(newTempFilePath);

            String date = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("mmss") + DateTime.Now.Millisecond.ToString();
            String destFileName = "RotationSyllabus_" + CurrentViewContext.SelectedTenantID.ToString() + "_" + date + Path.GetExtension(newTempFilePath);
            String desFilePath = "Tenant(" + CurrentViewContext.SelectedTenantID.ToString() + @")\" + destFileName;

            savedFilePath = CommonFileManager.SaveDocument(newTempFilePath, desFilePath, FileType.SystemDocumentLocation.GetStringValue());

            return savedFilePath;
        }

        /// <summary>
        /// Method to Bind Agencies
        /// </summary>
        private void BindAgencyForAddForm(RadComboBox ddlAgencyOnAddForm, Int32 rotationAgencyID = AppConsts.NONE)
        {
            if (CurrentViewContext.lstAgencyForAddForm.IsNullOrEmpty())
            {
                Presenter.GetAllAgencyForAddForm();
            }
            ddlAgencyOnAddForm.DataSource = CurrentViewContext.lstAgencyForAddForm;
            ddlAgencyOnAddForm.DataBind();
            ddlAgencyOnAddForm.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT--"));
            if (CurrentViewContext.SelectedAgencyIDForAddForm > AppConsts.NONE)
            {
                ddlAgencyOnAddForm.SelectedValue = CurrentViewContext.SelectedAgencyIDForAddForm.ToString();
            }
            //UAT-2034:Phase 4 (16): Manage Rotation screen updates
            else if (rotationAgencyID > AppConsts.NONE)
            {
                ddlAgencyOnAddForm.SelectedValue = rotationAgencyID.ToString();
            }
        }

        //UAT-2424
        private void BindRotationForAddForm(RadComboBox ddlRotationOnAddForm)
        {
            Presenter.GetClinicalRotationsForAddForm();
            ddlRotationOnAddForm.DataSource = CurrentViewContext.lstClinicalRotation;
            ddlRotationOnAddForm.DataBind();
            ddlRotationOnAddForm.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT--"));
            CurrentViewContext.SelectedRotationIDForCloning = AppConsts.NONE;
        }

        private void BindWeekDaysForAddForm(RadComboBox cmbDays)
        {
            Presenter.GetWeekDaysForAddForm();
            cmbDays.DataSource = CurrentViewContext.WeekDayListForAddForm;
            cmbDays.DataBind();
        }

        private void BindContactssForAddForm(RadComboBox cmbContacts)
        {
            Presenter.GetClientContactsForAddForm();
            cmbContacts.DataSource = CurrentViewContext.ClientContactListForAddForm;
            cmbContacts.DataBind();
            //cmbContacts.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT--"));
        }

        /// <summary>
        /// Create and load Custom Attributes
        /// </summary>
        private void CreateCustomAttributes()
        {
            SharedUserCustomAttributeForm caCustomAttributesID = (SharedUserCustomAttributeForm)Page.LoadControl("~\\ClinicalRotation\\UserControl\\SharedUserCustomAttributeForm.ascx");
            caCustomAttributesID.ID = "caCustomAttributesID";
            caCustomAttributesID.IsSearchTypeControl = true;
            Int32? rotationID = null;
            Presenter.GetCustomAttributeList(rotationID);
            if (!CurrentViewContext.GetCustomAttributeList.IsNullOrEmpty())
            {
                caCustomAttributesID.Reset();
                GenerateCustomAttributes(caCustomAttributesID);
                //pnlCustomAttributes.Controls.Clear();
                //pnlCustomAttributes.Controls.Add(caCustomAttributes);
                caCustomAttributesID.ViewStateMode = System.Web.UI.ViewStateMode.Disabled;
            }
        }


        /// <summary>
        /// UAT-1784: Apply the granular permissions for the Nag-Notification settings.
        /// </summary>
        private void ApplyGranularPermissions(GridEditFormItem editform)
        {
            var _managePkgPermissions = CurrentViewContext.dicGranularPermissions.Where(gp => gp.Key == EnumSystemEntity.ASSIGN_ROTATION_PACKAGE.GetStringValue()).FirstOrDefault();

            if (_managePkgPermissions.IsNotNull() && _managePkgPermissions.Key.IsNotNull() && _managePkgPermissions.Value.IsNotNull())
            {
                if (_managePkgPermissions.Value == EnumSystemPermissionCode.NO_ACCESS_PERMISSION.GetStringValue())
                {
                    var _txtDaysBefore = (editform.FindControl("txtDaysBefore") as WclNumericTextBox);
                    var _txtFrequency = (editform.FindControl("txtFrequency") as WclNumericTextBox);

                    if (_txtDaysBefore.IsNotNull() && _txtFrequency.IsNotNull())
                    {
                        _txtDaysBefore.Enabled = false;
                        _txtFrequency.Enabled = false;
                    }
                }
            }
        }

        #region UAT-2034:Phase 4 (16): Manage Rotation screen updates
        private String IsSelectedRotationsHaveSameAgency()
        {
            if (!CurrentViewContext.DicOfSelectedRotation.IsNullOrEmpty())
            {
                String selectedRots = String.Join(",", CurrentViewContext.DicOfSelectedRotation.Select(slct => slct.Key));
                Presenter.GetRotationsMappedToAgencies(selectedRots);
                if (CurrentViewContext.RotationsMappedToAgenciesData.IsRotationAgencyCountMatched)
                    return CurrentViewContext.RotationsMappedToAgenciesData.AgencyIDs;
                else
                    return String.Empty;
            }
            return String.Empty;
        }

        private void RedirectToManageRotationAssignments(String rotationAssignmentType)
        {
            if (!CurrentViewContext.DicOfSelectedRotation.IsNullOrEmpty())
            {
                String selectedRotationsHaveSameAgencies = String.Empty;

                if ((String.Compare(rotationAssignmentType, RotationAssignmentType.ASSIGN_PRECEPTOR_PACKAGES.GetStringValue(), true) == 0
                      || String.Compare(rotationAssignmentType, RotationAssignmentType.ASSIGN_STUDENT_PACKAGES.GetStringValue(), true) == 0))
                {
                    selectedRotationsHaveSameAgencies = IsSelectedRotationsHaveSameAgency();
                }
                if ((
                     (String.Compare(rotationAssignmentType, RotationAssignmentType.ASSIGN_PRECEPTOR_PACKAGES.GetStringValue(), true) == 0
                      || String.Compare(rotationAssignmentType, RotationAssignmentType.ASSIGN_STUDENT_PACKAGES.GetStringValue(), true) == 0) && !String.IsNullOrEmpty(selectedRotationsHaveSameAgencies)
                     )
                     || (String.Compare(rotationAssignmentType, RotationAssignmentType.UPLOAD_SYLLABUS.GetStringValue(), true) == 0
                     || String.Compare(rotationAssignmentType, RotationAssignmentType.ASSIGN_PRECEPTOR.GetStringValue(), true) == 0)
                    )
                {
                    String agencyId = Convert.ToString(selectedRotationsHaveSameAgencies);
                    String selectedRotationIDs = string.Empty;
                    selectedRotationIDs = String.Join(",", CurrentViewContext.DicOfSelectedRotation.Select(slct => slct.Key).ToList());
                    if (String.Compare(rotationAssignmentType, RotationAssignmentType.ASSIGN_PRECEPTOR_PACKAGES.GetStringValue(), true) == 0 && !Presenter.IsPreceptorAssignedForAllRotations(selectedRotationIDs))
                    {
                        ShowInfoMessage("Please map Instructor / Preceptor user with rotation before assigning Instructor/Preceptor package.");
                        return;
                    }
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "OpenPopupForManageRotAssignments('" + agencyId + "','" + selectedRotationIDs + "','" + rotationAssignmentType + "','" + CurrentViewContext.SelectedTenantID + "');", true);
                }
                else
                {
                    ShowInfoMessage("Please select rotations of same agency.");
                }
            }
            else
            {
                ShowInfoMessage("Please select rotations to proceed.");
            }
        }
        #endregion

        void IsRotationFieldsUpdated(ClinicalRotationDetailContract OldRotation, ClinicalRotationDetailContract NewRotation)
        {
            List<Int32> newAgencyIds = NewRotation.AgencyIdList.Split(',').Select(Int32.Parse).ToList();
            List<Int32> oldAgencyIds = OldRotation.AgencyIDs.Split(',').Select(Int32.Parse).ToList();
            if (newAgencyIds.Except(oldAgencyIds).Count() > 0
                || oldAgencyIds.Except(newAgencyIds).Count() > 0)
            {
                IsAgencyChanged = true;
                IsFieldsUpdated = true;
            }
            if (NewRotation.HierarchyNodeIDList != OldRotation.HierarchyNodeIDList)
            {
                IsFieldsUpdated = true;
            }
            if (NewRotation.RotationName.Trim() != OldRotation.RotationName.Trim())
            {
                IsFieldsUpdated = true;
            }
            if (NewRotation.TypeSpecialty.Trim() != OldRotation.TypeSpecialty.Trim())
            {
                IsFieldsUpdated = true;
            }
            if (NewRotation.Department.Trim() != OldRotation.Department.Trim())
            {
                IsFieldsUpdated = true;
            }
            if (NewRotation.Program.Trim() != OldRotation.Program.Trim())
            {
                IsFieldsUpdated = true;
            }
            if (NewRotation.Course.Trim() != OldRotation.Course.Trim())
            {
                IsFieldsUpdated = true;
            }
            if (NewRotation.Term.Trim() != OldRotation.Term.Trim())
            {
                IsFieldsUpdated = true;
            }
            if (NewRotation.UnitFloorLoc.Trim() != OldRotation.UnitFloorLoc.Trim())
            {
                IsFieldsUpdated = true;
            }

            if (NewRotation.Students != OldRotation.Students)
            {
                IsFieldsUpdated = true;
            }
            if (NewRotation.RecommendedHours != OldRotation.RecommendedHours)
            {
                IsFieldsUpdated = true;
            }
            if (NewRotation.DaysIdList.Trim() != OldRotation.DaysIdList.Trim())
            {
                IsFieldsUpdated = true;
            }
            if (NewRotation.Shift.Trim() != OldRotation.Shift.Trim())
            {
                IsFieldsUpdated = true;
            }
            if (NewRotation.ContactIdList.Trim() != OldRotation.ContactIdList.Trim())
            {
                IsFieldsUpdated = true;
            }
            if (NewRotation.StartTime != OldRotation.StartTime)
            {
                IsFieldsUpdated = true;
            }
            if (NewRotation.EndTime != OldRotation.EndTime)
            {
                IsFieldsUpdated = true;
            }
            if (NewRotation.StartDate != OldRotation.StartDate)
            {
                IsFieldsUpdated = true;
            }
            if (NewRotation.EndDate != OldRotation.EndDate)
            {
                IsFieldsUpdated = true;
            }

            NewRotation.SyllabusFileName = NewRotation.SyllabusFileName.IsNullOrEmpty() ? String.Empty : NewRotation.SyllabusFileName;
            NewRotation.SyllabusFilePath = NewRotation.SyllabusFilePath.IsNullOrEmpty() ? String.Empty : NewRotation.SyllabusFilePath;
            if ((NewRotation.SyllabusFileName != OldRotation.SyllabusFileName) || (NewRotation.SyllabusFilePath != OldRotation.SyllabusFilePath))
            {
                IsFieldsUpdated = true;
            }
            if (NewRotation.DeadlineDate != OldRotation.DeadlineDate)
            {
                IsFieldsUpdated = true;
            }
            if (NewRotation.DaysBefore != OldRotation.DaysBefore)
            {
                IsFieldsUpdated = true;
            }
            if (NewRotation.Frequency != OldRotation.Frequency)
            {
                IsFieldsUpdated = true;
            }
            //UAT-2905
            if (NewRotation.IsAllowNotification != OldRotation.IsAllowNotification)
            {
                IsFieldsUpdated = true;
            }
        }


        //UAT-3053
        private void RedirectToDetail(String RotationId, String AgencyId, String TenantId, Boolean IsDisplayMessage, String IsClientAdminEditPermission, String IsAgencyUserEditPermission)
        {
            Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 {
                                                                    { ProfileSharingQryString.SelectedTenantId, TenantId },
                                                                    { "Child",  AppConsts.ROTATION_DETAIL_CONTROL},
                                                                    { "ID", RotationId},
                                                                    {ProfileSharingQryString.AgencyId, AgencyId},
                                                                    {AppConsts.IS_EDITABLE_BY_CLIENT_ADMIN,IsClientAdminEditPermission},
                                                                    {AppConsts.IS_EDITABLE_BY_AGENCY_USER,IsAgencyUserEditPermission},
                                                                    {"HighlightRotationFieldUpdatedByAgencies",AppConsts.TRUE},
                                                                    {"IsDisplaySuccessMessage",Convert.ToString(IsDisplayMessage)},
                                                                    {"IsApplicantPkgNotAssignedThroughCloning",Convert.ToString(CurrentViewContext.IsApplicantPkgNotAssignedThroughCloning)},//UAT-3121
                                                                    {"IsInstructorPkgNotAssignedThroughCloning",Convert.ToString(CurrentViewContext.IsInstructorPkgNotAssignedThroughCloning)}//UAT-3121
                                                                 };
            string url = String.Format("~/ClinicalRotation/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
            Response.Redirect(url, true);
        }

        #endregion

        #region UAT-3032:- Sticky institution

        private void GetPreferredSelectedTenant()
        {
            Presenter.IsAdminLoggedIn();
            if (CurrentViewContext.SelectedTenantID.IsNullOrEmpty() || CurrentViewContext.SelectedTenantID == AppConsts.NONE && CurrentViewContext.IsAdminLoggedIn == true)
            {
                //Presenter.GetPreferredSelectedTenant();
                CurrentViewContext.PreferredSelectedTenantID = Convert.ToInt32(Session["PreferredSelectedTenant"]);
                if (CurrentViewContext.PreferredSelectedTenantID > AppConsts.NONE)
                {
                    ddlTenantName.SelectedValue = Convert.ToString(CurrentViewContext.PreferredSelectedTenantID);
                    ucAgencyHierarchyMultipleToSearchRotation.TenantId = Convert.ToInt32(ddlTenantName.SelectedValue);

                    //Custom Attributes
                    if (!ddlTenantName.SelectedValue.IsNullOrEmpty())
                    {
                        CurrentViewContext.SelectedTenantIDForAddForm = Convert.ToInt32(ddlTenantName.SelectedValue);
                        caCustomAttributesID.IsSearchTypeControl = true;
                        caCustomAttributesID.TenantId = CurrentViewContext.SelectedTenantID;
                        caCustomAttributesID.TypeCode = CustomAttributeUseTypeContext.Hierarchy.GetStringValue();
                        caCustomAttributesID.DataSourceModeType = DataSourceMode.Ids;
                        caCustomAttributesID.Title = "Other Details";
                        caCustomAttributesID.ControlDisplayMode = DisplayMode.Controls;
                        caCustomAttributesID.CurrentLoggedInUserId = base.CurrentUserId;
                        caCustomAttributesID.ValidationGroup = "grpFormSubmitSearchType";
                        caCustomAttributesID.IsReadOnly = false;

                        Presenter.GetCustomAttributeList(null);
                        if (!CurrentViewContext.GetCustomAttributeList.IsNullOrEmpty())
                            caCustomAttributesID.lstTypeCustomAttributes = CurrentViewContext.GetCustomAttributeList;
                        if (caCustomAttributesID.IsNotNull() && _searchContract.IsNotNull())
                            caCustomAttributesID.previousValues = _searchContract.CustomAttributes;
                        if (IsPreferredDefaultTenantSelected)
                            caCustomAttributesID.SetCustomAttributeValues();
                    }
                    //ResetControls();
                    // CreateCustomAttributes();
                    //BindArchiveFilter();
                    //ResetGridFilters();
                    //ucAgencyHierarchyMultipleToSearchRotation.Reset();
                    //BindRotationReviewStatus();
                    // grdRotations.Rebind();
                }
                // grdRotations.Rebind();
            }
            //else
            //{
            //    ResetTenant();
            //}
        }

        #endregion

    }
}