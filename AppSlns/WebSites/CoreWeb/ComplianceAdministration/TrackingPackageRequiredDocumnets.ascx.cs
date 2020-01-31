using CoreWeb.ComplianceAdministration.Views;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using Entity.ClientEntity;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CoreWeb.ComplianceAdministration.UserControl
{
    public partial class TrackingPackageRequiredDocumnets : BaseUserControl, ITrackingPackageRequiredItemsView
    {
        #region Private Properties

        private Int32 _tenantid;
        private TrackingPackageRequiredContract _viewContract;
        private TrackingPackageRequiredPresenter _presenter = new TrackingPackageRequiredPresenter();
        private String _viewType;
        private Boolean? _isAdminLoggedIn = null;
        private Int32 _selectedTenantId = AppConsts.NONE;
        private Boolean isEditClicked = false;
        #endregion
        #region Public Properties
        public List<TrackingPackageRequiredContract> listTrackingPackageRequiredContract
        {
            get
           ;
            set
           ;
        }
        public List<CompliancePackage> listPackage
        {
            get
            ;
            set
            ;
        }
        /// <summary>
        /// OrganizationUserID of the currently logged in user
        /// </summary>
        public Int32 CurrentLoggedInUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        /// <summary>
        /// Represents the current view 
        /// </summary>
        public ITrackingPackageRequiredItemsView CurrentViewContext
        {
            get { return this; }
        }

        /// <summary>
        /// Contract to manage the properties of the ComplianceItems Entity
        /// </summary>
        public TrackingPackageRequiredContract ViewContract
        {
            get
            {
                if (_viewContract.IsNull())
                {
                    _viewContract = new TrackingPackageRequiredContract();
                }
                return _viewContract;
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

        Int32 ITrackingPackageRequiredItemsView.PreferredSelectedTenantID
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
        public TrackingPackageRequiredPresenter Presenter
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

        #region UAT-4249
        public List<CompliancePackage> ListCompliancePackages
        {
            set;
            get;
        }

        public List<Int32> SelectedPackageIDList
        {
            get
            {
                List<Int32> selectedIds = new List<Int32>();
                for (Int32 i = 0; i < ddlPackage.Items.Count; i++)
                {
                    if (ddlPackage.Items[i].Checked)
                    {
                        selectedIds.Add(Convert.ToInt32(ddlPackage.Items[i].Value));
                    }
                }
                return selectedIds;
            }
            set
            {
                for (Int32 i = 0; i < ddlPackage.Items.Count; i++)
                {
                    ddlPackage.Items[i].Checked = value.Contains(Convert.ToInt32(ddlPackage.Items[i].Value));
                }

            }
        }

        public string SelectedPackageIDs { get; set; }

        #endregion

        #endregion
        #region Private funcation
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
        /// Method to clear the search filters.
        /// </summary>
        private void ClearFilters()
        {
            if (IsAdminLoggedIn == true)
                GetPreferredSelectedTenant();
            SetDefaultSelectedTenantId();
            ddlPackage.ClearCheckedItems();
            hdnIsSearchClicked.Value = string.Empty;
            grdTrackingPackage.Rebind();
        }

        /// <summary>
        /// Removes all the filters and Sorting on the grid and clears the variables.
        /// </summary>
        private void ResetGridFilters()
        {
            grdTrackingPackage.MasterTableView.FilterExpression = null;            
            grdTrackingPackage.MasterTableView.SortExpressions.Clear();
            grdTrackingPackage.CurrentPageIndex = 0;
            grdTrackingPackage.MasterTableView.CurrentPageIndex = 0;
            grdTrackingPackage.Rebind();
        }

        #endregion
        #region Page Event
        protected override void OnInit(EventArgs e)
        {
            try
            {
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                Dictionary<String, String> args = new Dictionary<String, String>();

                 if (!Request.QueryString["args"].IsNull())
                 {
                     args.ToDecryptedQueryString(Request.QueryString["args"]);
                 }
                 base.Title = "Manage Document URL";
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
                if (IsAdminLoggedIn == true)
                    GetPreferredSelectedTenant();
                /*END UAT-3032*/
                BindPackage();
            }
            SetDefaultSelectedTenantId();
            base.SetPageTitle("Manage Compliance Package Document URL");
        }

        #endregion
        #region Grid and ddl Tenant Events

        protected void ddlTenant_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                hdnIsSearchClicked.Value = string.Empty;
                BindPackage();
                grdTrackingPackage.CurrentPageIndex = 0;
                grdTrackingPackage.MasterTableView.SortExpressions.Clear();
                grdTrackingPackage.MasterTableView.FilterExpression = null;

                foreach (GridColumn column in grdTrackingPackage.MasterTableView.OwnerGrid.Columns)
                {
                    column.CurrentFilterFunction = GridKnownFunction.NoFilter;
                    column.CurrentFilterValue = string.Empty;
                }
                grdTrackingPackage.Rebind();
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

        protected void grdTrackingPackage_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                BindGrid();
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

        private void BindGrid()
        {
            if (!String.IsNullOrEmpty(hdnIsSearchClicked.Value))
            {
                if (SelectedPackageIDList.Count > AppConsts.NONE)
                    SelectedPackageIDs = string.Join(",", SelectedPackageIDList);
                Presenter.GetTrackingPackageRequired(SelectedPackageIDs);
                grdTrackingPackage.DataSource = CurrentViewContext.listTrackingPackageRequiredContract;
            }
            else
            {
                grdTrackingPackage.DataSource = new List<TrackingPackageRequiredContract>();
            }
        }

        protected void grdTrackingPackage_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        private void BindPackagesForAddEditForm(RadComboBox ddlCategory)
        {

            Presenter.GetPackage();
            ddlCategory.DataSource = CurrentViewContext.listPackage;
            ddlCategory.DataBind();
            //ddlCategory.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT--"));
        }

        protected void grdTrackingPackage_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == RadGrid.PerformInsertCommandName || e.CommandName == RadGrid.UpdateCommandName)
                {
                    if (e.CommandName == RadGrid.UpdateCommandName)
                        CurrentViewContext.ViewContract.trackingPackageRequiredDOCURLId = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["trackingPackageRequiredDOCURLId"]);
                    WclComboBox ddlPackage = e.Item.FindControl("ddlPackage") as WclComboBox;

                    RequiredFieldValidator RequiredddlPackage = e.Item.FindControl("rfvPackage") as RequiredFieldValidator;

                    if (ddlPackage.CheckedItems.Count > 0)
                    {
                        foreach (var categoryId in ddlPackage.CheckedItems)
                        {

                            CurrentViewContext.ViewContract.PackageIds += categoryId.Value + ",";
                        }
                    }
                    else
                    {
                        base.ShowErrorInfoMessage("Please select at least one package.");
                        e.Canceled = true;
                        return;
                    }

                    CurrentViewContext.ViewContract.Name = (e.Item.FindControl("txtScreenName") as WclTextBox).Text.Trim().IsNullOrEmpty() ? String.Empty : (e.Item.FindControl("txtScreenName") as WclTextBox).Text.Trim();
                    CurrentViewContext.ViewContract.Label = (e.Item.FindControl("txtScreenLabel") as WclTextBox).Text.Trim().IsNullOrEmpty() ? String.Empty : (e.Item.FindControl("txtScreenLabel") as WclTextBox).Text.Trim();
                    CurrentViewContext.ViewContract.SampleDocFormURL = (e.Item.FindControl("txtURL") as WclTextBox).Text.Trim().IsNullOrEmpty() ? String.Empty : (e.Item.FindControl("txtURL") as WclTextBox).Text.Trim();
                    if (!Presenter.CheckDuplicateRecords())
                    {
                        Presenter.SavePackageRequired();
                        if (CurrentViewContext.IsOperationSuccessful)
                        {
                         
                            if (CurrentViewContext.ViewContract.trackingPackageRequiredDOCURLId == Convert.ToInt32(AppConsts.ZERO))
                                base.ShowSuccessMessage("Compliance Package Document URL added successfully.");
                            else
                                base.ShowSuccessMessage("Compliance Package Document URL updated successfully.");

                            BindGrid();
                        }

                    }
                    else { base.ShowInfoMessage("Compliance Package Document URL already exist."); e.Canceled = true; }
                    
                      
                }
                if (e.CommandName == "Delete")
                {
                    CurrentViewContext.ViewContract.trackingPackageRequiredDOCURLId = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["trackingPackageRequiredDOCURLId"]);
                    Presenter.DeletePackageAndURLData();
                    {
                        BindGrid();
                        base.ShowSuccessMessage("Compliance Package Document URL deleted successfully.");
                    }
                }
                if (e.CommandName.IsNullOrEmpty())
                {
                    grdTrackingPackage.MasterTableView.GetColumn("TempCountOfAssociated").Display = true;
                }
                if (e.CommandName == "Cancel")
                {
                    grdTrackingPackage.MasterTableView.GetColumn("TempCountOfAssociated").Display = false;
                }
                //if (e.CommandName == RadGrid.RebindGridCommandName)
                //{
                //    if (SelectedPackageIDList.Count > AppConsts.NONE)
                //        SelectedPackageIDs = string.Join(",", SelectedPackageIDList);
                //    grdTrackingPackage.Rebind();
                //}               

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        protected void grdTrackingPackage_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            try
            {
               if(isExport)
                   grdTrackingPackage.MasterTableView.GetColumn("PackageIds").Display = false;
           
                TrackingPackageRequiredContract agencyNodeMappingContract = e.Item.DataItem as TrackingPackageRequiredContract;
            


                if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
                {
                    GridEditFormItem editform = (e.Item as GridEditFormItem);
                    WclComboBox ddlPackage = e.Item.FindControl("ddlPackage") as WclComboBox;
                    if (ddlPackage != null)
                    {
                        BindPackagesForAddEditForm(ddlPackage);
                        if (agencyNodeMappingContract != null && agencyNodeMappingContract.PackageIds != string.Empty && agencyNodeMappingContract.PackageIds != null && ddlPackage.IsNotNull() && ddlPackage.Items.Count > 0)
                        {
                            foreach (string categoryMappingID in agencyNodeMappingContract.PackageIds.Split(','))
                            {
                                if (ddlPackage.FindItemByValue(categoryMappingID.Trim()).IsNotNull())
                                {
                                    ddlPackage.FindItemByValue(categoryMappingID.Trim()).Checked = true;
                                }
                            }
                        }

                    }
                }
                if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
                {

                    #region UAT-3871
                    //agencyNodeMappingContract.trackingPackageRequiredDOCURLId
                    HtmlAnchor lnkNameOfPackages = ((HtmlAnchor)e.Item.FindControl("lnkNameOfPackages"));
                    (e.Item.FindControl("lnkNameOfPackages") as HtmlAnchor).InnerText = Convert.ToString(agencyNodeMappingContract.CountOfAssociated);
                    if (!string.IsNullOrEmpty((e.Item.FindControl("lnkNameOfPackages") as HtmlAnchor).InnerText))
                    {
                        //Adding encrypted query string to lnkNameOfPackages
                        Dictionary<String, String> queryString = new Dictionary<String, String>();
                        queryString = new Dictionary<String, String>
                                                                 {
                                                                    {"TenantId", CurrentViewContext.SelectedTenantId.ToString() } ,
                                                                    {"trackingPackageRequiredDOCURLId",agencyNodeMappingContract.trackingPackageRequiredDOCURLId.ToString()}
                                                                 };
                        lnkNameOfPackages.Attributes.Add("args", queryString.ToEncryptedQueryString());
                    }
                    else
                    {
                        lnkNameOfPackages.Visible = false;
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            { throw ex; }

        }
        #endregion
        bool isExport = false;
        protected void grdTrackingPackage_GridExporting(object sender, GridExportingArgs e)
        {
            isExport = true;
            grdTrackingPackage.MasterTableView.GetColumn("PackageIds").Display = false;
           

           // Response.Redirect(Request.Url.ToString());

        }

        protected void grdTrackingPackage_PdfExporting(object sender, GridPdfExportingArgs e)
        {

            grdTrackingPackage.MasterTableView.GetColumn("PackageIds").Display = false;
        }

        #region UAT-4249
        /// <summary>
        /// To bind package dropdown
        /// </summary>
        private void BindPackage()
        {
            Presenter.GetCompliancePackages();
            ddlPackage.DataSource = ListCompliancePackages;
            ddlPackage.DataBind();            
            if (ListCompliancePackages.Count >= 10)
            {
                ddlPackage.Height = Unit.Pixel(200);
            }
            if (ListCompliancePackages.Count == AppConsts.NONE)
            {
                ddlPackage.EnableCheckAllItemsCheckBox = false;
            }
            else
            {
                ddlPackage.EnableCheckAllItemsCheckBox = true;
            }
        }

        /// <summary>
        /// Button click to perform search.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucSearch_SubmitClick(object sender, EventArgs e)
        {
            try
            {
                if (SelectedPackageIDList.Count > AppConsts.NONE)
                    SelectedPackageIDs = string.Join(",", SelectedPackageIDList);
                grdTrackingPackage.Rebind();
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
        /// To Reset Search Filters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucSearch_ExtraClick(object sender, EventArgs e)
        {
            try
            {
                BindTenant();
                BindPackage();
                ClearFilters();
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

        #endregion
    }
}