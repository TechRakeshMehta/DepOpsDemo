using INTSOF.UI.Contract.SearchUI;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CoreWeb.SearchUI.Views
{
    public partial class SharedUserSearchDetails : BaseUserControl, ISharedUserSearchDetails
    {
        #region VARIABLES
        private SharedUserSearchDetailsPresenter _presenter = new SharedUserSearchDetailsPresenter();
        private String _viewType;

        public String ApplicantInvitationSourceCode = InvitationSourceTypes.APPLICANT.GetStringValue();
        #endregion

        #region PROPERTIES

       

        public SharedUserSearchDetailsPresenter Presenter
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

        /// <summary>
        /// Represents the list of Invitation Details
        /// </summary>
        List<SharedUserSearchInvitationDetailsContract> ISharedUserSearchDetails.SharedUserInvitationDetails
        {
            get
            {
                return ViewState["SharedUserInvitationDetails"] as List<SharedUserSearchInvitationDetailsContract>;
            }
            set
            {
                ViewState["SharedUserInvitationDetails"] = value;
            }
        }

        /// <summary>
        /// Represensts the Current Context
        /// </summary>
        ISharedUserSearchDetails CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// Represents the Id of the Current Invitation being viewed, during Expand/Collapse
        /// </summary>
        Int32 ISharedUserSearchDetails.CurrentInvitationId
        {
            get
            {
                if (!ViewState["CurrentInvitationId"].IsNullOrEmpty())
                {
                    return Convert.ToInt32(ViewState["CurrentInvitationId"]);
                }
                return 0;
            }
            set
            {
                ViewState["CurrentInvitationId"] = value;
            }
        }

        /// <summary>
        /// Represents the current invitation groupID
        /// </summary>
        Int32 ISharedUserSearchDetails.SharedUserID
        {
            get
            {
                if (!ViewState["SharedUserID"].IsNullOrEmpty())
                {
                    return Convert.ToInt32(ViewState["SharedUserID"]);
                }
                return 0;
            }
            set
            {
                ViewState["SharedUserID"] = value;
            }
        }

        /// <summary>
        /// Represents the list of shared packages
        /// </summary>
        List<SharedPackages> ISharedUserSearchDetails.lstSharedPkgs
        {
            get
            {
                if (!ViewState["lstSharedPkgs"].IsNullOrEmpty())
                {
                    return ViewState["lstSharedPkgs"] as List<SharedPackages>;
                }
                return new List<SharedPackages>();
            }
            set
            {
                ViewState["lstSharedPkgs"] = value;
            }
        }

        String ISharedUserSearchDetails.SharedCategoryIDs
        {
            get
            {
                return Convert.ToString(ViewState["SharedCategoryIDs"]);
            }
            set
            {
                ViewState["SharedCategoryIDs"] = value;
            }
        }

        String ISharedUserSearchDetails.InvitationSourceCode
        {
            get
            {
                return Convert.ToString(ViewState["InvitationSourceCode"]);
            }
            set
            {
                ViewState["InvitationSourceCode"] = value;
            }
        }

        Int32? ISharedUserSearchDetails.TenantID
        {
            get
            {
                if (!ViewState["TenantID"].IsNullOrEmpty())
                {
                    return Convert.ToInt32(ViewState["TenantID"]);
                }
                return 0;
            }
            set
            {
                ViewState["TenantID"] = value;
            }
        }

        #endregion

        #region EVENTS

        #region PAGE EVENTS

        /// <summary>
        /// Page Init Event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            try
            {
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                base.OnInit(e);
                base.SetPageTitle("Invitation Details");
                base.Title = "Invitation Details";
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
        /// Page Load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                (fsucCmdBarButton as CoreWeb.Shell.Views.CommandBar).CancelButton.ToolTip = "Click to go to back to search";

                if (!IsPostBack)
                {
                    //Getting Data from querystring 
                    Dictionary<String, String> args = new Dictionary<String, String>();
                    if (!Request.QueryString["args"].IsNull())
                    {
                        args.ToDecryptedQueryString(Request.QueryString["args"]);
                        if (args.ContainsKey("SharedUserID") && args["SharedUserID"].IsNotNull())
                        {
                            CurrentViewContext.SharedUserID = Convert.ToInt32(args["SharedUserID"]);
                        }
                    }
                    Presenter.GetSharedUserInvitationDetails();
                }
            }
        }
        #endregion

        #region BUTTON EVENTS

        /// <summary>
        /// Cancel Button Click Button. Redirects to back to search.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucCmdBarButton_CancelClick(object sender, EventArgs e)
        {
            try
            {
                Dictionary<String, String> queryString = new Dictionary<String, String>();
                queryString = new Dictionary<String, String>
                                                                 { 
                                                                    {"Child", ChildControls.SharedUserSearch},
                                                                    {"BackToSharedUserSearch", Convert.ToString(true)},
                                                                 };
                string url = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                Response.Redirect(url, false);
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

        #region GRID EVENTS

        #region GRID INVITATION DETAILS - LEVEL 1

        /// <summary>
        /// Level-1 Grid InvitationDetails NeedDataSource Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdInvitationDetails_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                grdInvitationDetails.DataSource = CurrentViewContext.SharedUserInvitationDetails;
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
            }
        }

        /// <summary>
        /// Level-1 Grid InvitationDetails ItemCommand Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdInvitationDetails_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == RadGrid.RebindGridCommandName) 
                {
                    Presenter.GetSharedUserInvitationDetails();
                }
                if (e.CommandName == RadGrid.ExpandCollapseCommandName && !e.Item.Expanded)
                {
                    GridDataItem parentItem = e.Item as GridDataItem;
                    RadGrid grdSharedPackages = parentItem.ChildItem.FindControl("grdSharedPackages") as RadGrid;
                    grdSharedPackages.Rebind();
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
            }
        }

        /// <summary>
        /// Level-1 Grid InvitationDetails ItemDataBound Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdInvitationDetails_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem dataItem = (GridDataItem)e.Item;
                dataItem["InviteType"].Text = GetInvitationSourceName(Convert.ToString(dataItem["InviteType"].Text));
            }
        }

        #endregion

        #region GRID SHARED PACKAGES DETAILS - LEVEL 2

        /// <summary>
        /// Level-2 Grid SharedPackages NeedDataSource Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdSharedPackages_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                GridDataItem parentItem = ((sender as RadGrid).NamingContainer as GridNestedViewItem).ParentItem as GridDataItem;
                CurrentViewContext.CurrentInvitationId = Convert.ToInt32(parentItem.GetDataKeyValue("InvitationID"));
                if (!CurrentViewContext.SharedUserInvitationDetails.IsNullOrEmpty())
                {
                    CurrentViewContext.lstSharedPkgs = CurrentViewContext.SharedUserInvitationDetails.Where(col => col.InvitationID == CurrentViewContext.CurrentInvitationId).First().lstSharedPackages.ToList();
                    CurrentViewContext.InvitationSourceCode = CurrentViewContext.SharedUserInvitationDetails.Where(col => col.InvitationID == CurrentViewContext.CurrentInvitationId).First().InvitationSourceCode;
                    CurrentViewContext.TenantID = CurrentViewContext.SharedUserInvitationDetails.Where(col => col.InvitationID == CurrentViewContext.CurrentInvitationId).First().TenantID;
                    (sender as RadGrid).DataSource = CurrentViewContext.SharedUserInvitationDetails.Where(col => col.InvitationID == CurrentViewContext.CurrentInvitationId).First().lstSharedPackages;
                }
                else
                {
                    (sender as RadGrid).DataSource = new List<SharedPackages>();
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
            }
        }

        /// <summary>
        /// Level-2 Grid SharedPackages ItemCommand Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdSharedPackages_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == RadGrid.ExpandCollapseCommandName && !e.Item.Expanded)
                {
                    GridDataItem parentItem = e.Item as GridDataItem;
                    RadGrid grdSharedEntity = parentItem.ChildItem.FindControl("grdSharedEntity") as RadGrid;
                    if (parentItem.GetDataKeyValue("PackageTypeCode").ToString() == SystemPackageTypes.COMPLIANCE_PKG.GetStringValue() || parentItem.GetDataKeyValue("PackageTypeCode").ToString() == SystemPackageTypes.REQUIREMENT_ROT_PKG.GetStringValue())
                        grdSharedEntity.MasterTableView.GetColumn("SharedEntityName").HeaderText = "Category Name";
                    else
                        grdSharedEntity.MasterTableView.GetColumn("SharedEntityName").HeaderText = "Service Group Name";
                    grdSharedEntity.Rebind();
                }
                else if (e.CommandName == "ViewPassportReport")
                {
                    GridDataItem item = e.Item as GridDataItem;

                    //Getting PackageID
                    Int32 packageID = Convert.ToInt32(item.GetDataKeyValue("PackageID"));
                    String sharedCategoryIDs = String.Empty;
                    Int32 snapShotID = 0;
                    Int32 packageSubscriptionID = 0;

                    LinkButton lbtnPassportReport = e.Item.FindControl("btnPassportReport") as LinkButton;
                    packageSubscriptionID = Convert.ToInt32(lbtnPassportReport.Attributes["packagesubscriptionid"]);//Getting PackageSubscriptionID

                    if (CurrentViewContext.InvitationSourceCode == InvitationSourceTypes.APPLICANT.GetStringValue())
                    {
                        //Getting SharedCategoryIDs
                        Presenter.GetSharedCategoryList(packageSubscriptionID);
                        //sharedCategoryIDs = CurrentViewContext.SharedCategoryIDs;
                    }
                    else
                    {
                        //Getting SnapshotID
                        snapShotID = Convert.ToInt32(lbtnPassportReport.Attributes["snapshotid"]);//Getting SnapShotID
                    }
                    String documentType = "ReportDocument";
                    String reportType = String.Empty;
                    String url = string.Empty;

                    if (snapShotID == 0)
                    {
                        reportType = "Data Report";
                        url = String.Format("~/ComplianceOperations/Pages/ComplianceReportViewer.aspx?Psid={0}&ShrdCatIds={1}&DocumentType={2}&ReportType={3}&tenantId={4}"
                            , packageSubscriptionID, CurrentViewContext.SharedCategoryIDs, documentType, reportType, CurrentViewContext.TenantID);
                    }
                    else
                    {
                        reportType = "Status Report";
                        url = String.Format("~/ComplianceOperations/Pages/ComplianceReportViewer.aspx?Psid={0}&SnpShtId={1}&DocumentType={2}&ReportType={3}&tenantId={4}"
                            , packageSubscriptionID, snapShotID, documentType, reportType, CurrentViewContext.TenantID);
                    }

                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "OpenPassportReportPopup('" + url + "');", true);
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
            }
        }

        /// <summary>
        /// Level-2 Grid SharedPackages ItemDataBound Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdSharedPackages_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
                {
                    GridDataItem dataItem = (GridDataItem)e.Item;
                    LinkButton btnPassportReport = dataItem.FindControl("btnPassportReport") as LinkButton;
                   
                    if (dataItem.GetDataKeyValue("PackageTypeCode").ToString() == SystemPackageTypes.COMPLIANCE_PKG.GetStringValue())
                    {
                        dataItem["PackageTypeCode"].Text = "Compliance Package";
                    }
                    else if (dataItem.GetDataKeyValue("PackageTypeCode").ToString() == SystemPackageTypes.BACKGROUND_PKG.GetStringValue())
                    {
                        dataItem["PackageTypeCode"].Text = "Background Package";
                        btnPassportReport.Visible = false;
                    }
                    else if (dataItem.GetDataKeyValue("PackageTypeCode").ToString() == SystemPackageTypes.REQUIREMENT_ROT_PKG.GetStringValue())
                    {
                        dataItem["PackageTypeCode"].Text = "Requirement Package";
                        btnPassportReport.Visible = false;
                    }

                    if (Convert.ToInt32(dataItem.GetDataKeyValue("OrderID").ToString()) == AppConsts.NONE)
                        dataItem["OrderID"].Text = "NA";
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
            }
        }

        #endregion

        #region GRID SHARED ENTITIES DETAILS - LEVEL 3

        /// <summary>
        /// Level-3 Grid SharedEntity NeedDataSource Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdSharedEntity_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                GridDataItem parentItem = ((sender as RadGrid).NamingContainer as GridNestedViewItem).ParentItem as GridDataItem;
                Int32 packageId = Convert.ToInt32(parentItem.GetDataKeyValue("PackageID"));
                Guid uniqueId = Guid.Parse(Convert.ToString(parentItem.GetDataKeyValue("PackageIdentifier")));

                if (!CurrentViewContext.SharedUserInvitationDetails.IsNullOrEmpty())
                {
                    var _lstSharedPackages = CurrentViewContext.SharedUserInvitationDetails.Where(inv => inv.InvitationID == CurrentViewContext.CurrentInvitationId).First().lstSharedPackages;

                    if (!_lstSharedPackages.IsNullOrEmpty())
                    {
                        List<SharedEntity> lstSharedEntityToBind = _lstSharedPackages.Where(pkg => pkg.PackageID == packageId && pkg.PackageIdentifier == uniqueId).First().lstSharedEntity;
                        (sender as RadGrid).DataSource = lstSharedEntityToBind;
                    }
                }
                else
                {
                    (sender as RadGrid).DataSource = new List<SharedEntity>();
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
            }
        }

        /// <summary>
        /// Level-3 Grid SharedEntity ItemCommand Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdSharedEntity_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "ResultReport")
                {
                    GridDataItem item = (GridDataItem)e.Item;
                    GridTableView nestedview = (GridTableView)item.OwnerTableView;
                    GridNestedViewItem nestedviewItem = (GridNestedViewItem)nestedview.NamingContainer.NamingContainer;
                    GridDataItem dataItem = (GridDataItem)nestedviewItem.ParentItem;
                    String currentOrderID = dataItem.GetDataKeyValue("OrderID").ToString();

                    String documentType = "ReportDocument";
                    String reportType = "OrderCompletion";
                    String currentSvcGroupID = item.GetDataKeyValue("SharedEntityID").ToString();

                    String url = String.Format("~/BkgOperations/Pages/ServiceFormViewer.aspx?OrderID={0}&DocumentType={1}&ServiceGroupID={2}&ReportType={3}&tenantId={4}",
                                                        currentOrderID, documentType, currentSvcGroupID, reportType, CurrentViewContext.TenantID);

                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ShowReportResultForServiceGroup('" + url + "');", true);
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
            }
        }


        #endregion

        #endregion

        #endregion

        #region METHODS
        /// <summary>
        /// Method to get Concatenated category IDs based on packageID
        /// </summary>
        /// <param name="packageID"></param>
        /// <returns></returns>
        private void GetConcatCategoryIds(Int32 invitationID, Int32 packageSubscriptionID)
        {
            //List<SharedEntity> lstSharedEntity = CurrentViewContext.lstSharedPkgs.Where(cond => cond.PackageID == packageID).Select(col => col.lstSharedEntity).FirstOrDefault().ToList();

            //Get Shared category ids based on current package subscription id and invitation id
            Presenter.GetSharedCategoryList(packageSubscriptionID);
        }

        private string GetInvitationSourceName(string invitationSourceCode)
        {
            String retName = "";
            if (invitationSourceCode == InvitationSourceTypes.ADMIN.GetStringValue())
            {
                retName = "Admin";
            }
            else if (invitationSourceCode == InvitationSourceTypes.CLIENTADMIN.GetStringValue())
            {
                retName = "Client Admin";
            }
            else if (invitationSourceCode == InvitationSourceTypes.APPLICANT.GetStringValue())
            {
                retName = "Applicant";
            }
            return retName;
        }
        #endregion

     
    }
}

