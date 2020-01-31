#region Namespaces

#region System Defined Namespaces

using System;
using System.Collections.Generic;


#endregion

#region User Defined Namespaces

using INTSOF.Utils;
using Telerik.Web.UI;
using CoreWeb.Shell;
using Entity.ClientEntity;
using INTSOF.UI.Contract.SystemSetUp;
using System.Web.UI.WebControls;
using System.Drawing;


#endregion

#endregion

namespace CoreWeb.SystemSetUp.Views
{
    public partial class InstitutionConfigurationDetails : BaseWebPage, IInstitutionConfigurationDetailsView
    {
        #region Variables

        #region Private Variables

        private InstitutionConfigurationDetailsPresenter _presenter = new InstitutionConfigurationDetailsPresenter();
        private String _viewType;

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        public InstitutionConfigurationDetailsPresenter Presenter
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

        public IInstitutionConfigurationDetailsView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public String ErrorMessage { get; set; }

        public String PageType
        {
            get { return Convert.ToString(ViewState["PageType"]); }
            set { ViewState["PageType"] = value; }
        }

        public Int32 CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        public Int32 SelectedTenantId
        {
            get { return (Int32)(ViewState["SelectedTenantId"]); }
            set { ViewState["SelectedTenantId"] = value; }
        }

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

        public Int32 DeptProgramMappingID
        {
            get { return (Int32)(ViewState["DepProgramMappingID"]); }
            set { ViewState["DepProgramMappingID"] = value; }
        }

        public Int32 ParentID
        {
            get
            {
                return Convert.ToInt32(ViewState["ParentID"]);
            }
            set
            {
                ViewState["ParentID"] = value;
            }
        }

        public Int32 NodeId
        {
            get;
            set;
        }

        public List<Entity.OrganizationUser> OrganizationUserList
        {
            get;
            set;
        }

        public Int32 OrganizationUserID
        {
            get;
            set;
        }
        string _nodeLabel = string.Empty;
        public String NodeLabel
        {
            get
            {
                //_nodeLabel= Convert.ToString(lblNodeTitle.Text);
                return lblNodeTitle.Text.HtmlDecode();
            }
            set
            { 
                lblNodeTitle.Text = value.ToString().HtmlEncode();
            }
        }

        String _splashScreenURL;
        String IInstitutionConfigurationDetailsView.SplashScreenURL
        {
            get
            {
                return _splashScreenURL;
            }
            set
            {
                _splashScreenURL = value.ToString();
                lblSplashURL.Text = _splashScreenURL.HtmlEncode();
            }
        }

        #region UAT-1758 : Add Overall compliance note change to incomplete after a resubmit setting to Client settings, also display this setting on the Institution Configuration search

        String IInstitutionConfigurationDetailsView.OverallComplianceStatus
        {
            set
            {
                lblOverallComplianceStatusValue.Text = value.ToString();
            }
        }
        #endregion                     

        InstitutionConfigurationDetailsContract IInstitutionConfigurationDetailsView.InstitutionConfigurationDetailsContract { get; set; }
        List<InstitutionConfigurationPackageDetails> IInstitutionConfigurationDetailsView.InstitutionConfigurationPackageDetailsList { get; set; }
        List<InstitutionConfigurationAdministratorDetails> IInstitutionConfigurationDetailsView.InstitutionConfigurationAdministratorDetailsList { get; set; }

        #endregion

        #endregion

        #region Events

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                btnDummy.Style.Add("display", "none");
                if (!this.IsPostBack)
                {
                    if (!Request.QueryString["PageType"].IsNullOrEmpty())
                    {
                        CurrentViewContext.PageType = Request.QueryString["PageType"];
                    }
                    if (!Request.QueryString["SelectedTenantId"].IsNullOrEmpty())
                    {
                        CurrentViewContext.SelectedTenantId = Convert.ToInt32(Request.QueryString["SelectedTenantId"]);
                    }

                    if (!Request.QueryString["Id"].IsNullOrEmpty())
                    {
                        CurrentViewContext.DeptProgramMappingID = Convert.ToInt32(Request.QueryString["Id"]);
                    }
                    if (Request.QueryString["ParentID"].IsNullOrEmpty())
                    {
                        CurrentViewContext.ParentID = AppConsts.NONE;
                    }
                    else
                    {
                        CurrentViewContext.ParentID = Convert.ToInt32(Request.QueryString["ParentID"]);
                    }
                    if (!Request.QueryString["NodeName"].IsNullOrEmpty())
                    {
                        CurrentViewContext.NodeLabel = "Node: " + Request.QueryString["NodeName"];
                    }
                    Presenter.OnViewInitialized();
                }
                ifrExportDocument.Src = String.Empty;
                Presenter.GetInstitutionConfigurationDetails();
                Presenter.OnViewLoaded();
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

        /// <summary>
        /// Node grid Need Data Source event to bind Node grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdPackages_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                grdPackages.DataSource = CurrentViewContext.InstitutionConfigurationPackageDetailsList;
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
        /// Redirect the user to the detail page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdPackages_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                #region DetailScreenNavigation

                if (e.CommandName.Equals("ViewDetail"))
                {
                    Dictionary<String, String> queryString = new Dictionary<String, String>();
                    String packageID = (e.Item as GridDataItem).GetDataKeyValue("PackageID").ToString();
                    String packageHierarchyID = (e.Item as GridDataItem).GetDataKeyValue("PackageHierarchyID").ToString();
                    Boolean isCompliancePackage = Convert.ToBoolean((e.Item as GridDataItem).GetDataKeyValue("IsCompliancePackage"));
                    String packageType = (e.Item as GridDataItem).GetDataKeyValue("PackageType").ToString();//UAT-2411
                    String packageName = (e.Item as GridDataItem).GetDataKeyValue("PackageName").ToString();//UAT-2411
                    queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "PackageID", packageID },
                                                                    { "HierarchyNodeID", CurrentViewContext.DeptProgramMappingID.ToString()},
                                                                    {"SelectedTenantID",CurrentViewContext.SelectedTenantId.ToString()},
                                                                    {"PackageHierarchyID",packageHierarchyID},
                                                                    {"PackageName",packageName},//UAT-2411
                                                                    {"NodeLabel",CurrentViewContext.NodeLabel},//UAT-2411,
                                                                 };

                    String url = String.Empty;
                    if (isCompliancePackage)
                    {
                        url = String.Format(@"~\SystemSetup\pages\CompliancePkgDetails.aspx?args={0}", queryString.ToEncryptedQueryString());
                    }
                    else if (packageType == AppConsts.INSTITUTION_CONFIGURATION_BUNDLE_PACKAGE_TYPE)//UAT-2411
                    {
                        url = String.Format(@"~\SystemSetup\pages\InstitutionConfigurationBundleDetails.aspx?args={0}", queryString.ToEncryptedQueryString());
                    }
                    else
                    {
                        url = String.Format(@"~\SystemSetup\pages\InstitutionConfigurationScreeningDetails.aspx?args={0}", queryString.ToEncryptedQueryString());
                    }

                    Response.Redirect(url, true);

                }
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

        protected void grdAdministrators_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                grdAdministrators.DataSource = CurrentViewContext.InstitutionConfigurationAdministratorDetailsList;
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

        protected void grdAdministrators_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                #region DETAILS SCREEN NAVIGATION
                if (e.CommandName == "ViewDetail")
                {
                    Dictionary<String, String> queryString = new Dictionary<String, String>();
                    String organizationUserId = (e.Item as GridDataItem).GetDataKeyValue("OrganizationUserId").ToString();
                    queryString = new Dictionary<String, String>
                                                                 {
                                                                    {"TenantID", Convert.ToString(CurrentViewContext.SelectedTenantId) },
                                                                    {"OrganizationUserId", organizationUserId},
                                                                    {"DeptProgramMappingID",CurrentViewContext.DeptProgramMappingID.ToString()},
                                                                     {"NodeLabel",CurrentViewContext.NodeLabel}//UAT-2411
                                                                 };
                    string url = String.Format(@"~\SystemSetup\pages\ClientProfilePage.aspx?args={0}", queryString.ToEncryptedQueryString());
                    Response.Redirect(url, true);
                }

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

        #endregion

        protected void grdPackages_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    Boolean isParentPackage = Convert.ToBoolean(dataItem.GetDataKeyValue("IsParentPackage"));
                    #region UAT:2411
                    String packageName = (e.Item as GridDataItem).GetDataKeyValue("PackageType").ToString();//UAT-2411
                    if (packageName == AppConsts.INSTITUTION_CONFIGURATION_BUNDLE_PACKAGE_TYPE)
                    {
                        dataItem["Fee"].Text = string.Empty;
                    }
                    #endregion
                    if (isParentPackage)
                    {
                        dataItem["PackageType"].ForeColor = dataItem["PackageName"].ForeColor = Color.Green;
                        dataItem["PackageType"].Font.Bold = dataItem["PackageName"].Font.Bold = true;
                        dataItem["PackageType"].BorderStyle = dataItem["PackageName"].BorderStyle = BorderStyle.Solid;
                        dataItem["PackageType"].BorderColor = dataItem["PackageName"].BorderColor = Color.Black;
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





        protected void btnExportConfigurationReport_Click(object sender, EventArgs e)
        {
            try
            {
                // List<Int32> lstContractIDs = Presenter.GetContractIDs();
                if (CurrentViewContext.SelectedTenantId > 0)
                {

                    ifrExportDocument.Src = "~/ComplianceOperations/UserControl/DocumentViewer.aspx?tenantId=" + CurrentViewContext.SelectedTenantId + "&ReportType=InstitutionConfigurationExport";
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

        #region Methods

        #region Private Methods

        #endregion

        #endregion
    }
}