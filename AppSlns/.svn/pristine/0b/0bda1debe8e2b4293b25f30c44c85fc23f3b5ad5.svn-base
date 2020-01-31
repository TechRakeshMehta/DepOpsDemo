using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreWeb.Shell;
using INTSOF.UI.Contract.SystemSetUp;
using INTSOF.Utils;

namespace CoreWeb.SystemSetUp.Views
{
    public partial class CompliancePkgDetails : BaseWebPage, ICompliancePkgDetailsView
    {
        #region Variables

        #region Private Variables

        private CompliancePkgDetailsPresenter _presenter = new CompliancePkgDetailsPresenter();
        #endregion

        #endregion

        #region Properties

        #region Public Properties

        public CompliancePkgDetailsPresenter Presenter
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

        public ICompliancePkgDetailsView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// Gets the data for Tree List.
        /// </summary>
        CompliancePkgDetailContract ICompliancePkgDetailsView.CompliancePkgDetails
        {
            get { return (CompliancePkgDetailContract)(ViewState["CompliancePkgDetails"]); }
            set { ViewState["CompliancePkgDetails"] = value; }
        }

        Int32 ICompliancePkgDetailsView.CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        Int32 ICompliancePkgDetailsView.SelectedTenantId
        {
            get { return (Int32)(ViewState["SelectedTenantId"]); }
            set { ViewState["SelectedTenantId"] = value; }
        }

        Int32 ICompliancePkgDetailsView.DeptProgramMappingID
        {
            get { return (Int32)(ViewState["DepProgramMappingID"]); }
            set { ViewState["DepProgramMappingID"] = value; }
        }

        Int32 ICompliancePkgDetailsView.PackageID
        {
            get { return (Int32)(ViewState["PackageID"]); }
            set { ViewState["PackageID"] = value; }
        }

        Int32 ICompliancePkgDetailsView.PackageHierarchyID
        {
            get { return (Int32)(ViewState["PackageHierarchyID"]); }
            set { ViewState["PackageHierarchyID"] = value; }
        }

        String ICompliancePkgDetailsView.NodeLabel
        {
            get
            {
                return Convert.ToString(lblNodeTitle.Text);
            }
            set
            {
                lblNodeTitle.Text = value.ToString();
            }
        }

        #region UAT:2411
        String ICompliancePkgDetailsView.ParentScreenName
        {
            get { return (String)(ViewState["BundleDetail"]); }
            set { ViewState["BundleDetail"] = value; }
        }
        Int32 ICompliancePkgDetailsView.BundlePackageID
        {
            get { return (Int32)(ViewState["BundlePackageID"]); }
            set { ViewState["BundlePackageID"] = value; }
        }
        String ICompliancePkgDetailsView.MasterNodeLabel
        {
            get { return (String)(ViewState["NodeLabel"]); }
            set { ViewState["NodeLabel"] = value; }
        }
        String ICompliancePkgDetailsView.BundleName
        {
            get { return (String)(ViewState["BundleName"]); }
            set { ViewState["BundleName"] = value; }
        }
        #endregion

        #endregion
        #endregion

        #region Page Events
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    Presenter.OnViewInitialized();
                    Dictionary<String, String> args = new Dictionary<String, String>();

                    if (!Request.QueryString["args"].IsNull())
                    {
                        args.ToDecryptedQueryString(Request.QueryString["args"]);

                        if (args.ContainsKey("PackageID"))
                        {
                            CurrentViewContext.PackageID = Convert.ToInt32(args["PackageID"]);
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
                        #region UAT:2411
                        if (args.ContainsKey("ParentScreen"))
                        {
                            CurrentViewContext.ParentScreenName = args["ParentScreen"];
                        }
                        if (args.ContainsKey("BundlePackageID"))
                        {
                            CurrentViewContext.BundlePackageID = Convert.ToInt32(args["BundlePackageID"]);
                        }
                        if (args.ContainsKey("NodeLabel"))
                        {
                            CurrentViewContext.MasterNodeLabel = args["NodeLabel"];
                        }
                        if (args.ContainsKey("BundleName"))
                        {
                            CurrentViewContext.BundleName = args["BundleName"];
                        }
                        #endregion
                    }
                    Presenter.GetCompliancePkgDetails();
                    rptSubscriptionOption.DataSource = CurrentViewContext.CompliancePkgDetails.SubscriptionOptionDetails;
                    rptSubscriptionOption.DataBind();
                }

                Presenter.OnViewLoaded();
                //UAT:2411
                if (!string.IsNullOrEmpty(CurrentViewContext.ParentScreenName))
                    btnBackToQueue.Text = "Back To Bundle Details";
                else
                    btnBackToQueue.Text = "Back To Queue";
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

        #region Treelist events
        protected void treeListDetail_PreRender(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    treeListDetail.ExpandAllItems();
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

        protected void treeListDetail_NeedDataSource(object sender, Telerik.Web.UI.TreeListNeedDataSourceEventArgs e)
        {
            try
            {
                treeListDetail.DataSource = CurrentViewContext.CompliancePkgDetails.ReviewerTypeDetails;
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

        #region Button events
        protected void btnBackToQueue_Click(object sender, EventArgs e)
        {
            try
            {
                String url = string.Empty;
                #region UAT-2411
                Dictionary<String, String> queryString = new Dictionary<String, String>();
                if (!string.IsNullOrEmpty(CurrentViewContext.ParentScreenName))
                {
                    queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "PackageID", CurrentViewContext.BundlePackageID.ToString() },
                                                                    { "HierarchyNodeID", CurrentViewContext.DeptProgramMappingID.ToString()},
                                                                    {"SelectedTenantID",CurrentViewContext.SelectedTenantId.ToString()},
                                                                    {"PackageHierarchyID",CurrentViewContext.PackageHierarchyID.ToString()},
                                                                    {"PackageName",CurrentViewContext.BundleName},
                                                                    {"NodeLabel",CurrentViewContext.MasterNodeLabel}
                                                                 };
                    url = String.Format(@"InstitutionConfigurationBundleDetails.aspx?args={0}", queryString.ToEncryptedQueryString());
                }

                #endregion
               
                else
                   // url = String.Format(@"InstitutionConfigurationDetails.aspx?Id={0}&SelectedTenantID={1}&NodeName={2}", CurrentViewContext.DeptProgramMappingID, CurrentViewContext.SelectedTenantId, CurrentViewContext.CompliancePkgDetails.NodeLabel);
                    url = String.Format(@"InstitutionConfigurationDetails.aspx?Id={0}&SelectedTenantID={1}&NodeName={2}", CurrentViewContext.DeptProgramMappingID, CurrentViewContext.SelectedTenantId, CurrentViewContext.MasterNodeLabel.Replace("Node:", string.Empty).Trim());//UAT:2411

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
    }
}