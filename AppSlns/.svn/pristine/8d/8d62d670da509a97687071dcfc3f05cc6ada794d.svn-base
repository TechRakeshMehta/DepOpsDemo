#region Namespaces

#region System Defined Namespaces

using System;
using System.Collections.Generic;
using System.Linq;


#endregion

#region User Defined Namespaces

using INTSOF.Utils;
using Telerik.Web.UI;
using CoreWeb.Shell;
using INTSOF.UI.Contract.SystemSetUp;
using System.Web.UI.WebControls;
using Entity.ClientEntity;


#endregion

#endregion

namespace CoreWeb.SystemSetUp.Views
{
    public partial class InstitutionConfigurationLocalFeeDetails : BaseWebPage, IInstitutionConfigurationLocalFeeDetailsView
    {
        #region Variables

        #region Private Variables
        private InstitutionConfigurationLocalFeeDetailsPresenter _presenter = new InstitutionConfigurationLocalFeeDetailsPresenter();
        #endregion

        #region public variables

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        public InstitutionConfigurationLocalFeeDetailsPresenter Presenter
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

        public IInstitutionConfigurationLocalFeeDetailsView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        String IInstitutionConfigurationLocalFeeDetailsView.ErrorMessage { get; set; }

        Int32 IInstitutionConfigurationLocalFeeDetailsView.CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        Int32 IInstitutionConfigurationLocalFeeDetailsView.SelectedTenantId
        {
            get
            {
                return Convert.ToInt32(ViewState["SelectedTenantId"]);
            }
            set
            {
                ViewState["SelectedTenantId"] = value;
            }
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

        public Int32 PackageID
        {
            get { return (Int32)(ViewState["PackageID"]); }
            set { ViewState["PackageID"] = value; }
        }

        Int32 IInstitutionConfigurationLocalFeeDetailsView.PackageServiceItemFeeID
        {
            get { return (Int32)(ViewState["PackageServiceItemFeeID"]); }
            set { ViewState["PackageServiceItemFeeID"] = value; }
        }

        List<LocalFeeRecordsInfo> IInstitutionConfigurationLocalFeeDetailsView.ListServiceItemFeeRecord
        {
            get;
            set;
        }

        String IInstitutionConfigurationLocalFeeDetailsView.FeeItemName
        {
            get { return (Convert.ToString(ViewState["FeeItemName"])); }
            set { ViewState["FeeItemName"] = value; }
        }

        Int32 IInstitutionConfigurationLocalFeeDetailsView.PackageHierarchyID
        {
            get { return (Int32)(ViewState["PackageHierarchyID"]); }
            set { ViewState["PackageHierarchyID"] = value; }
        }

        #endregion

        #endregion

        #region Events

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
                        if (args.ContainsKey("PackageServiceItemFeeID"))
                        {
                            CurrentViewContext.PackageServiceItemFeeID = Convert.ToInt32(args["PackageServiceItemFeeID"]);
                        }
                        if (args.ContainsKey("FeeItemName"))
                        {
                            CurrentViewContext.FeeItemName = Convert.ToString(args["FeeItemName"]);
                        }
                        if (args.ContainsKey("PackageHierarchyID"))
                        {
                            CurrentViewContext.PackageHierarchyID = Convert.ToInt32(args["PackageHierarchyID"]);
                        }
                    }
                    Presenter.OnViewInitialized();
                }
                lblFeeItem.Text = "Fee Record > " + CurrentViewContext.FeeItemName.HtmlEncode();
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

        #region Grid Events

        protected void grdFeeRecord_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            Presenter.GetServiceItemFeeRecordList();
            grdFeeRecord.DataSource = CurrentViewContext.ListServiceItemFeeRecord;
        }

        #endregion

        #region Button events

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
                String url = String.Format(@"~\SystemSetup\pages\InstitutionConfigurationScreeningDetails.aspx?args={0}", queryString.ToEncryptedQueryString());
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

        #endregion

        #region Methods

        #region Private Methods

        #endregion

        #endregion
    }
}