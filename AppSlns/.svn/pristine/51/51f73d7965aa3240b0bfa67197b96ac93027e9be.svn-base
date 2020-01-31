using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using Entity;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.UI.Contract.BkgSetup;
using INTSOF.Utils;
using Telerik.Web.UI;

namespace CoreWeb.SystemSetUp.Views
{
    public partial class InstitutionConfigurationGlobalFeeDetails : BaseWebPage, IInstitutionConfigurationGlobalFeeDetailsView
    {
        #region Variables

        #region Private Variables

        private InstitutionConfigurationGlobalFeeDetailsPresenter _presenter = new InstitutionConfigurationGlobalFeeDetailsPresenter();
        private Int32 _tenantid;
        private String _viewType;
        private Boolean? _isAdminLoggedIn = null;

        #endregion

        #region Public Variables


        #endregion

        #endregion

        #region Properties

        #region Private Properties
        #endregion

        #region Public Properties

        public InstitutionConfigurationGlobalFeeDetailsPresenter Presenter
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

        private IInstitutionConfigurationGlobalFeeDetailsView CurrentViewContext
        {
            get { return this; }
        }


        Int32 IInstitutionConfigurationGlobalFeeDetailsView.SelectedFeeItemId
        {
            get
            {
                return Convert.ToInt32(ViewState["SelectedFeeItemId"]);
            }
            set
            {
                ViewState["SelectedFeeItemId"] = value;
            }
        }

        String IInstitutionConfigurationGlobalFeeDetailsView.FeeTypeCode
        {
            get
            {
                return (String)(ViewState["FeeTypeCode"]);
            }
            set
            {
                ViewState["FeeTypeCode"] = value;
            }
        }

        /// <summary>
        /// Gets and set List of ServiceItemFeeType
        /// </summary>
        List<ServiceFeeItemRecordContract> IInstitutionConfigurationGlobalFeeDetailsView.ListServiceItemFeeRecordContract
        {
            set;
            get;
        }

        Int32 IInstitutionConfigurationGlobalFeeDetailsView.SelectedTenantId
        {
            get { return (Int32)(ViewState["SelectedTenantId"]); }
            set { ViewState["SelectedTenantId"] = value; }
        }

        Int32 IInstitutionConfigurationGlobalFeeDetailsView.DeptProgramMappingID
        {
            get { return (Int32)(ViewState["DepProgramMappingID"]); }
            set { ViewState["DepProgramMappingID"] = value; }
        }

        Int32 IInstitutionConfigurationGlobalFeeDetailsView.PackageID
        {
            get { return (Int32)(ViewState["PackageID"]); }
            set { ViewState["PackageID"] = value; }
        }

        Int32 IInstitutionConfigurationGlobalFeeDetailsView.PackageHierarchyID
        {
            get { return (Int32)(ViewState["PackageHierarchyID"]); }
            set { ViewState["PackageHierarchyID"] = value; }
        }

        #endregion

        #endregion


        #region Events

        #region Page Events
        protected override void OnInit(EventArgs e)
        {
            try
            {
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                //base.Title = "Manage Global Fee Record";
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
                    Presenter.OnViewInitialized();

                    Dictionary<String, String> args = new Dictionary<String, String>();
                    args.ToDecryptedQueryString(Request.QueryString["args"]);
                    if (!Request.QueryString["args"].IsNull())
                    {
                        if (args.ContainsKey("PSIF_ID"))
                        {
                            CurrentViewContext.SelectedFeeItemId = Convert.ToInt32(args["PSIF_ID"]);
                        }
                        if (args.ContainsKey("SIFT_Code"))
                        {
                            CurrentViewContext.FeeTypeCode = Convert.ToString(args["SIFT_Code"]);
                        }
                        if (args.ContainsKey("FeeItemName"))
                        {
                            hdnFeeItemName.Value = Convert.ToString(args["FeeItemName"]);
                        }
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
                    }
                }
                lblFeeItem.Text = hdnFeeItemName.Value.HtmlEncode();
                // base.SetPageTitle("Manage Global Fee Record");
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }
        #endregion

        #region Grid events
        protected void grdFeeRecord_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetServiceItemFeeRecordListContract();
                grdFeeRecord.DataSource = CurrentViewContext.ListServiceItemFeeRecordContract;
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        protected void grdFeeRecord_ItemCreated(object sender, GridItemEventArgs e)
        {
            try
            {

            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }
        #endregion


        protected void btnBackToQueue_Click(object sender, EventArgs e)
        {
            try
            {
                Dictionary<String, String> queryString = new Dictionary<String, String>();

                queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "PackageID", CurrentViewContext.PackageID.ToString() },
                                                                    { "HierarchyNodeID", CurrentViewContext.DeptProgramMappingID.ToString()},
                                                                    {"SelectedTenantID",CurrentViewContext.SelectedTenantId.ToString()},
                                                                    {"PackageHierarchyID",CurrentViewContext.PackageHierarchyID.ToString()}
                                                                 };

                String url = String.Empty;
                url = String.Format(@"~\SystemSetup\pages\InstitutionConfigurationScreeningDetails.aspx?args={0}", queryString.ToEncryptedQueryString());
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