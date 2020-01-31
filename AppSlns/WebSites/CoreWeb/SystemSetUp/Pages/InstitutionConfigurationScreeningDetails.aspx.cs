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


#endregion

#endregion

namespace CoreWeb.SystemSetUp.Views
{
    public partial class InstitutionConfigurationScreeningDetails : BaseWebPage, IInstitutionConfigurationScreeningDetailsView
    {
        #region Variables

        #region Private Variables
        private InstitutionConfigurationScreeningDetailsPresenter _presenter = new InstitutionConfigurationScreeningDetailsPresenter();
        #endregion

        #region public variables
        protected String ImagePath = "~/images/small";
        #endregion

        #endregion

        #region Properties

        #region Public Properties

        public InstitutionConfigurationScreeningDetailsPresenter Presenter
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

        public IInstitutionConfigurationScreeningDetailsView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public String ErrorMessage { get; set; }

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

        public Int32 PackageID
        {
            get { return (Int32)(ViewState["PackageID"]); }
            set { ViewState["PackageID"] = value; }
        }
        String _nodeLabel;
        public String NodeLabel
        {
            get
            {
                _nodeLabel = Convert.ToString(lblNodeTitle.Text).HtmlDecode();
                return _nodeLabel;
            }
            set
            {
                _nodeLabel = value.ToString();
                lblNodeTitle.Text = _nodeLabel.HtmlEncode();
            }
        }

        ScreeningDetailsForConfigurationContract IInstitutionConfigurationScreeningDetailsView.ScreeningDetailsForConfigurationContract { get; set; }
        List<BackgroundPackageDetailsForConfigurationContract> IInstitutionConfigurationScreeningDetailsView.BackgroundPackageDetailsForConfigurationList
        {
            get
            {
                return ViewState["BackgroundPackageDetailsForConfigurationList"] as List<BackgroundPackageDetailsForConfigurationContract>;
            }

            set
            {
                ViewState["BackgroundPackageDetailsForConfigurationList"] = value as List<BackgroundPackageDetailsForConfigurationContract>;
            }
        }
        List<ServiceFormDetailsForConfigurationContract> IInstitutionConfigurationScreeningDetailsView.ServiceFormDetailsForConfigurationList
        {
            get
            {
                return ViewState["ServiceFormDetailsForConfigurationList"] as List<ServiceFormDetailsForConfigurationContract>;
            }

            set
            {
                ViewState["ServiceFormDetailsForConfigurationList"] = value as List<ServiceFormDetailsForConfigurationContract>;
            }
        }

        List<ServiceItemFeeDetailsForConfigurationContract> IInstitutionConfigurationScreeningDetailsView.ServiceItemFeeDetailsList
        {
            get
            {
                return ViewState["ServiceItemFeeDetailsList"] as List<ServiceItemFeeDetailsForConfigurationContract>;
            }

            set
            {
                ViewState["ServiceItemFeeDetailsList"] = value as List<ServiceItemFeeDetailsForConfigurationContract>;
            }
        }

        Int32 IInstitutionConfigurationScreeningDetailsView.PackageHierarchyID
        {
            get { return (Int32)(ViewState["PackageHierarchyID"]); }
            set { ViewState["PackageHierarchyID"] = value; }
        }

        #region UAT:2411
        String IInstitutionConfigurationScreeningDetailsView.ParentScreenName
        {
            get { return (String)(ViewState["BundleDetail"]); }
            set { ViewState["BundleDetail"] = value; }
        }
        Int32 IInstitutionConfigurationScreeningDetailsView.BundlePackageID
        {
            get { return (Int32)(ViewState["BundlePackageID"]); }
            set { ViewState["BundlePackageID"] = value; }
        }
        String IInstitutionConfigurationScreeningDetailsView.MasterNodeLabel
        {
            get { return (String)(ViewState["NodeLabel"]); }
            set { ViewState["NodeLabel"] = value; }
        }
        String IInstitutionConfigurationScreeningDetailsView.BundleName
        {
            get { return (String)(ViewState["BundleName"]); }
            set { ViewState["BundleName"] = value; }
        }
        #endregion

        #endregion

        #endregion

        #region Events

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                btnDummy.Style.Add("display", "none");
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
                    Presenter.OnViewInitialized();
                    Presenter.GetInstitutionConfigurationDetails();
                    if (!string.IsNullOrEmpty(CurrentViewContext.ParentScreenName))
                        btnBackToQueue.Text = "Back To Bundle Details";
                    else
                        btnBackToQueue.Text = "Back To Queue";
                }
                Presenter.OnViewLoaded();
                ifrExportDocument.Src = String.Empty;
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

        #region Grid Related Events

        protected void grdServiceGrp_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                grdServiceGrp.DataSource = CurrentViewContext.BackgroundPackageDetailsForConfigurationList.Where(col => col.ServiceGroupID != null)
                        .DistinctBy(col => col.ServiceGroupID);
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        protected void grdServices_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                GridDataItem parentItem = ((sender as RadGrid).NamingContainer as GridNestedViewItem).ParentItem as GridDataItem;
                Int32 serviceGroupID = Convert.ToInt32(parentItem.GetDataKeyValue("ServiceGroupID"));
                (sender as RadGrid).DataSource = CurrentViewContext.BackgroundPackageDetailsForConfigurationList.Where(col => col.ServiceGroupID == serviceGroupID && col.ServiceID != null)
                                    .DistinctBy(col => col.ServiceID);
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        protected void grdServices_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
                {
                    Int32 serviceID = Convert.ToInt32((e.Item as GridDataItem).GetDataKeyValue("ServiceID"));
                    Repeater rptServiceForms = (Repeater)e.Item.FindControl("rptServiceForms");
                    if (rptServiceForms != null)
                    {
                        rptServiceForms.DataSource = CurrentViewContext.ServiceFormDetailsForConfigurationList.Where(cond => cond.ServiceID == serviceID && cond.SendAutomatically).DistinctBy(cond => cond.ServiceAtachedFormID)
                                                    .OrderBy(col => col.ServiceAtachedFormName);
                        rptServiceForms.DataBind();
                    }
                    Repeater rptServiceFormsManual = (Repeater)e.Item.FindControl("rptServiceFormsManual");
                    if (rptServiceForms != null)
                    {
                        rptServiceFormsManual.DataSource = CurrentViewContext.ServiceFormDetailsForConfigurationList.Where(cond => cond.ServiceID == serviceID && !cond.SendAutomatically).DistinctBy(cond => cond.ServiceAtachedFormID)
                                                    .OrderBy(col => col.ServiceAtachedFormName);
                        rptServiceFormsManual.DataBind();
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

        protected void grdServiceGrp_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                GridDataItem dataItem = e.Item as GridDataItem;
                if (e.CommandName == RadGrid.ExpandCollapseCommandName && !e.Item.Expanded)
                {
                    GridDataItem parentItem = e.Item as GridDataItem;
                    RadGrid grdServices = parentItem.ChildItem.FindControl("grdServices") as RadGrid;
                    grdServices.MasterTableView.HierarchyDefaultExpanded = false;
                    grdServices.Rebind();
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

        protected void grdServices_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                GridDataItem dataItem = e.Item as GridDataItem;
                if (e.CommandName == RadGrid.ExpandCollapseCommandName && !e.Item.Expanded)
                {
                    GridDataItem parentItem = e.Item as GridDataItem;
                    RadGrid grdServiceItems = parentItem.ChildItem.FindControl("grdServiceItems") as RadGrid;
                    grdServiceItems.Rebind();
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

        protected void grdServiceItems_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
                {
                    Int32 packageServiceItemID = Convert.ToInt32((e.Item as GridDataItem).GetDataKeyValue("PackageServiceItemID"));
                    Repeater rptLocalFees = (Repeater)e.Item.FindControl("rptLocalFees");
                    if (rptLocalFees != null)
                    {
                        rptLocalFees.DataSource = CurrentViewContext.ServiceItemFeeDetailsList.Where(cond => cond.PackageServiceItemID == packageServiceItemID);
                        rptLocalFees.DataBind();
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

        protected void grdServiceItems_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                GridDataItem parentItem = ((sender as RadGrid).NamingContainer as GridNestedViewItem).ParentItem as GridDataItem;
                Int32 serviceID = Convert.ToInt32(parentItem.GetDataKeyValue("ServiceID"));
                (sender as RadGrid).DataSource = CurrentViewContext.BackgroundPackageDetailsForConfigurationList.Where(col => col.ServiceID == serviceID && col.PackageServiceItemID != null)
                                .DistinctBy(col => col.PackageServiceItemID);
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        #endregion

        #region Repeater Events

        protected void rptServiceForms_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {

                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    LinkButton lnkDownldSvcFrm = (LinkButton)e.Item.FindControl("lnkDownldSvcFrm");
                    if (lnkDownldSvcFrm.IsNotNull())
                    {
                        ImageButton imgPDF = (ImageButton)e.Item.FindControl("imgPDF");
                        ServiceFormDetailsForConfigurationContract serviceFormContract = (ServiceFormDetailsForConfigurationContract)e.Item.DataItem;
                        lnkDownldSvcFrm.Text = serviceFormContract.ServiceAtachedFormName;
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

        protected void rptLocalFees_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    LinkButton lnkLocalFees = (LinkButton)e.Item.FindControl("lnkLocalFees");
                    Label lblLocalFees = (Label)e.Item.FindControl("lblLocalFees");
                    if (lnkLocalFees.IsNotNull())
                    {
                        ServiceItemFeeDetailsForConfigurationContract itemFeeContract = (ServiceItemFeeDetailsForConfigurationContract)e.Item.DataItem;
                        lblLocalFees.Text = lnkLocalFees.Text = itemFeeContract.FeeItemType.HtmlEncode() ;
                        if (!itemFeeContract.LocalFeeTypeCode.Equals(ServiceItemFeeType.FIXED_FEE.GetStringValue()))
                        {
                            lnkLocalFees.Visible = true;
                            lblLocalFees.Visible = false;
                        }
                        else
                        {
                            lnkLocalFees.Visible = false;
                            lblLocalFees.Visible = true;
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

        protected void rptServiceFormsManual_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {

                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    Label lblServiceForm = (Label)e.Item.FindControl("lblServiceForm");
                    if (lblServiceForm.IsNotNull())
                    {
                        ServiceFormDetailsForConfigurationContract serviceFormContract = (ServiceFormDetailsForConfigurationContract)e.Item.DataItem;
                        lblServiceForm.Text = serviceFormContract.ServiceAtachedFormName.HtmlEncode();
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

        #region Button events

        protected void hlViewServiceForm_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lnkDownldSvcFrm = sender as LinkButton;
                RepeaterItem repeaterItem = (RepeaterItem)lnkDownldSvcFrm.NamingContainer;
                HiddenField hdnServiceForm = repeaterItem.FindControl("hdnServiceForm") as HiddenField;
                ifrExportDocument.Src = String.Format("/ComplianceOperations/UserControl/DoccumentDownload.aspx?systemDocumentId={0}&systemDocumentType={1}", Convert.ToInt32(hdnServiceForm.Value), "DownloadServiceForm");
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

        protected void imgPDF_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            try
            {
                ImageButton imgPDF = sender as ImageButton;
                RepeaterItem repeaterItem = (RepeaterItem)imgPDF.NamingContainer;
                HiddenField hdnServiceForm = repeaterItem.FindControl("hdnServiceForm") as HiddenField;
                ifrExportDocument.Src = String.Format("/ComplianceOperations/UserControl/DoccumentDownload.aspx?systemDocumentId={0}&systemDocumentType={1}", Convert.ToInt32(hdnServiceForm.Value), "DownloadServiceForm");
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

        protected void lnkGlobalFeeName_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lnkGlobalFeeName = sender as LinkButton;
                GridDataItem gridDataItem = (GridDataItem)lnkGlobalFeeName.NamingContainer;
                var feeItemId = gridDataItem.DataItem;
                Dictionary<String, String> queryString = new Dictionary<String, String>();
                String PSIF_ID = String.Empty;
                String feeItemName = String.Empty;
                HiddenField hdnPackageServiceItemFeeID = gridDataItem.FindControl("hdnPackageServiceItemFeeID") as HiddenField;
                HiddenField hdnGlobalFeeName = gridDataItem.FindControl("hdnGlobalFeeName") as HiddenField;
                feeItemName = hdnGlobalFeeName.Value;
                PSIF_ID = hdnPackageServiceItemFeeID.Value;
                queryString = new Dictionary<String, String>
                                                                 {  
                                                                    { "Child", ChildControls.ManageFeeRecord},
                                                                    { "PSIF_ID",Convert.ToString(PSIF_ID)},
                                                                    {"FeeItemName",feeItemName},
                                                                    { "PackageID",CurrentViewContext.PackageID.ToString()},
                                                                    { "HierarchyNodeID", CurrentViewContext.DeptProgramMappingID.ToString()},
                                                                    {"SelectedTenantID",CurrentViewContext.SelectedTenantId.ToString()},
                                                                    {"PackageHierarchyID",CurrentViewContext.PackageHierarchyID.ToString()}
                                                                 };
                Response.Redirect(String.Format(@"~\SystemSetup\pages\InstitutionConfigurationGlobalFeeDetails.aspx?args={0}", queryString.ToEncryptedQueryString()));
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



        protected void lnkLocalFees_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lnkLocalFees = sender as LinkButton;
                RepeaterItem repeaterItem = (RepeaterItem)lnkLocalFees.NamingContainer;
                HiddenField hdnPackageServiceItemFeeID = repeaterItem.FindControl("hdnPackageServiceItemFeeID") as HiddenField;
                HiddenField hdnLocalFeeName = repeaterItem.FindControl("hdnLocalFeeName") as HiddenField;
                Dictionary<String, String> queryString = new Dictionary<String, String>();
                queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "PackageID", CurrentViewContext.PackageID.ToString() },
                                                                    { "HierarchyNodeID", CurrentViewContext.DeptProgramMappingID.ToString()},
                                                                    {"SelectedTenantID",CurrentViewContext.SelectedTenantId.ToString()},
                                                                    {"PackageServiceItemFeeID",hdnPackageServiceItemFeeID.Value},
                                                                    {"FeeItemName",hdnLocalFeeName.Value},
                                                                    {"PackageHierarchyID",CurrentViewContext.PackageHierarchyID.ToString()}
                                                                 };
                String url = String.Format(@"~\SystemSetup\pages\InstitutionConfigurationLocalFeeDetails.aspx?args={0}", queryString.ToEncryptedQueryString());
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
                 //url = String.Format(@"InstitutionConfigurationDetails.aspx?Id={0}&SelectedTenantID={1}&NodeName={2}", CurrentViewContext.DeptProgramMappingID, CurrentViewContext.SelectedTenantId
                 //   , CurrentViewContext.BackgroundPackageDetailsForConfigurationList.IsNullOrEmpty() ? "" : CurrentViewContext.BackgroundPackageDetailsForConfigurationList.FirstOrDefault().HierarchyLabel);
                    url = String.Format(@"InstitutionConfigurationDetails.aspx?Id={0}&SelectedTenantID={1}&NodeName={2}", CurrentViewContext.DeptProgramMappingID, CurrentViewContext.SelectedTenantId
                       , CurrentViewContext.MasterNodeLabel.Replace("Node:",string.Empty).Trim());//UAT:2411

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