using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTSOF.Utils;
using Telerik.Web.UI;
using System.Drawing;
using CoreWeb.Shell;
using INTSOF.UI.Contract.SystemSetUp;

namespace CoreWeb.SystemSetUp.Views // CoreWeb.SystemSetUp.Pages
{
    public partial class InstitutionConfigurationBundleDetails : BaseWebPage, IBundleDetailsView
    {

        #region Variables

        #region Private Variables

        private BundleDetailsPresenter _presenter = new BundleDetailsPresenter();
        //  private String _viewType;

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        public BundleDetailsPresenter Presenter
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

        public IBundleDetailsView CurrentViewContext
        {
            get
            {
                return this;
            }
        }
       

        String IBundleDetailsView.BundleName
        {
            get { return Convert.ToString(ViewState["BundleName"]); }
            set { ViewState["BundleName"] = value; }
        }

        Int32 IBundleDetailsView.CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        Int32 IBundleDetailsView.SelectedTenantId
        {
            get { return (Int32)(ViewState["SelectedTenantId"]); }
            set { ViewState["SelectedTenantId"] = value; }
        }

        Int32 IBundleDetailsView.DefaultTenantId
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

        Int32 IBundleDetailsView.DeptProgramMappingID
        {
            get { return (Int32)(ViewState["DepProgramMappingID"]); }
            set { ViewState["DepProgramMappingID"] = value; }
        }

        Int32 IBundleDetailsView.BundlePackageID
        {
            get { return (Int32)(ViewState["BundlePackageID"]); }
            set { ViewState["BundlePackageID"] = value; }
        }

        Int32 IBundleDetailsView.ParentID
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

        Int32 IBundleDetailsView.NodeId
        {
            get;
            set;
        }


        Int32 IBundleDetailsView.OrganizationUserID
        {
            get;
            set;
        }

        String IBundleDetailsView.NodeLabel
        {
            get
            {
                return Convert.ToString(lblNodeTitle.Text);
            }
            set
            {
                lblNodeTitle.Text = value.ToString().HtmlEncode();
            }
        }

        String IBundleDetailsView.MasterNodeLabel
        {
            get { return Convert.ToString(ViewState["NodeLabel"]); }
            set { ViewState["NodeLabel"] = value; }
        }

        List<InstitutionConfigurationPackageDetails> IBundleDetailsView.InstitutionConfigurationPackageDetailsList
        {
            get;
            set;
        }

        Int32 IBundleDetailsView.PackageHierarchyID
        {
            get { return (Int32)(ViewState["PackageHierarchyID"]); }
            set { ViewState["PackageHierarchyID"] = value; }
        }
        #endregion

        #endregion



        #region Page Events
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    Dictionary<String, String> args = new Dictionary<String, String>();

                    if (!Request.QueryString["args"].IsNull())
                    {
                        args.ToDecryptedQueryString(Request.QueryString["args"]);

                        if (args.ContainsKey("PackageID"))
                        {
                            CurrentViewContext.BundlePackageID = Convert.ToInt32(args["PackageID"]);
                        }
                        if (args.ContainsKey("SelectedTenantID"))
                        {
                            CurrentViewContext.SelectedTenantId = Convert.ToInt32(args["SelectedTenantID"]);
                        }
                        if (args.ContainsKey("HierarchyNodeID"))
                        {
                            CurrentViewContext.DeptProgramMappingID = Convert.ToInt32(args["HierarchyNodeID"]);
                        }
                        if (args.ContainsKey("PackageHierarchyID"))
                        {
                            CurrentViewContext.PackageHierarchyID = Convert.ToInt32(args["PackageHierarchyID"]);
                        }
                        if (args.ContainsKey("NodeLabel"))
                        {
                            CurrentViewContext.MasterNodeLabel = args["NodeLabel"];
                        }
                        if (args.ContainsKey("PackageName"))
                        {
                            CurrentViewContext.BundleName = args["PackageName"];
                        }
                        Presenter.OnViewInitialized();
                    }

                    Presenter.OnViewLoaded();
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
        protected void btnBackToQueue_Click(object sender, EventArgs e)
        {
            try
            {
                String url = String.Format(@"InstitutionConfigurationDetails.aspx?Id={0}&SelectedTenantID={1}&NodeName={2}", CurrentViewContext.DeptProgramMappingID, CurrentViewContext.SelectedTenantId, CurrentViewContext.MasterNodeLabel.Replace("Node:", string.Empty));
                Response.Redirect(url, true);
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

        protected void grdPackages_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
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
                    String packageType = (e.Item as GridDataItem).GetDataKeyValue("PackageType").ToString();

                    queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "PackageID", packageID },
                                                                    { "HierarchyNodeID", CurrentViewContext.DeptProgramMappingID.ToString()},
                                                                    {"SelectedTenantID",CurrentViewContext.SelectedTenantId.ToString()},
                                                                    {"PackageHierarchyID",packageHierarchyID},
                                                                    {"ParentScreen","InstitutionConfigurationBundleDetails"},
                                                                    {"BundlePackageID",CurrentViewContext.BundlePackageID.ToString()},
                                                                    {"NodeLabel",CurrentViewContext.MasterNodeLabel },
                                                                    {"BundleName",CurrentViewContext.BundleName}
                                                                 };

                    String url = String.Empty;
                    if (isCompliancePackage)
                    {
                        url = String.Format(@"~\SystemSetup\pages\CompliancePkgDetails.aspx?args={0}", queryString.ToEncryptedQueryString());
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

        protected void grdPackages_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    Boolean isParentPackage = Convert.ToBoolean(dataItem.GetDataKeyValue("IsParentPackage"));
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

        protected void grdPackages_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetInstitutionConfigurationBundlePackageDetails();
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
        #endregion
    }
}