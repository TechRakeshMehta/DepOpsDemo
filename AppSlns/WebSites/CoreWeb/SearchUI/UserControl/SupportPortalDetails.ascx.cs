#region Namespaces

#region SystemDefined

using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Practices.ObjectBuilder;

#endregion

#region UserDefined
using INTSOF.UI.Contract.SysXSecurityModel;
using Entity.ClientEntity;
using INTSOF.Utils;
using CoreWeb.Shell;
using INTSOF.UI.Contract.ComplianceOperation;
using CoreWeb.IntsofSecurityModel;
using System.Web.UI.WebControls;
using Business.RepoManagers;
using System.Configuration;
using Telerik.Web.UI;
using INTSOF.UI.Contract.ProfileSharing;
using WebSiteUtils.SharedObjects;
using INTSOF.UI.Contract.SearchUI;

#endregion

#endregion

namespace CoreWeb.SearchUI.Views
{
    public partial class SupportPortalDetails : BaseUserControl, ISupportPortalDetailsView
    {
        #region Variables

        #region Private Variables

        private SupportPortalDetailsPresenter _presenter = new SupportPortalDetailsPresenter();
        private List<InvitationDataContract> lstInvitations = new List<InvitationDataContract>();
        private String _viewType;
        private Int32 tenantId = 0;
        private String _userId = String.Empty;
        private String ImagePathOrderStatus = "~/images/medium";
        private String path = "~/images/Status/";
        #endregion

        #region Public Variables


        #endregion

        #endregion

        #region Properties

        #region Public Properties


        public SupportPortalDetailsPresenter Presenter
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

        public Int32 CurrentLoggedInUserId
        {
            get { return base.CurrentUserId; }
        }

        /// <summary>
        /// To set or get Search Instance Id
        /// </summary>
        public Int32 SearchInstanceId
        {
            get
            {
                if (!ViewState["SearchInstanceId"].IsNull())
                {
                    return Convert.ToInt32(ViewState["SearchInstanceId"]);
                }
                return 0;
            }
            set
            {
                ViewState["SearchInstanceId"] = value;
            }
        }


        public Int32 OrganizationUserId
        {
            get
            {
                Dictionary<String, String> args = new Dictionary<String, String>();
                if (Request.QueryString["args"].IsNotNull())
                {
                    args.ToDecryptedQueryString(Request.QueryString["args"]);

                    if (args.ContainsKey("OrganizationUserId"))
                    {
                        return (Convert.ToInt32(args["OrganizationUserId"]));
                    }
                }
                return base.CurrentUserId;
            }

            set
            {
                OrganizationUserId = value;
            }
        }

        public Int32 SelectedTenantId
        {
            get
            {
                if (!ViewState["SelectedTenantId"].IsNull())
                {
                    return Convert.ToInt32(ViewState["SelectedTenantId"]);
                }
                return 0;
            }
            set
            {
                ViewState["SelectedTenantId"] = value;
            }
        }

        /// <summary>
        /// To set or get Master Page Tab Index
        /// </summary>
        public Int32 MasterPageTabIndex
        {
            get
            {
                if (!ViewState["MasterPageTabIndex"].IsNull())
                {
                    return Convert.ToInt32(ViewState["MasterPageTabIndex"]);
                }
                return 0;
            }
            set
            {
                ViewState["MasterPageTabIndex"] = value;
            }
        }

        public Int32 TenantId
        {
            get
            {
                if (tenantId == 0)
                {
                    //tenantId = Presenter.GetTenantId();
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

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        /// <remarks></remarks>
        public String Password
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the default password.
        /// </summary>
        /// <value>The default password.</value>
        /// <remarks></remarks>
        public String DefaultPassword
        {
            get;
            set;
        }

        public String EmailAddress
        {
            get;
            set;
        }

        public String FirstName
        {
            get;
            set;
        }

        public String LastName
        {
            get;
            set;
        }

        public String QueueType
        {
            get
            {
                if (!ViewState["QueueType"].IsNullOrEmpty())
                {
                    return Convert.ToString(ViewState["QueueType"]);
                }
                return null;
            }
            set
            {
                ViewState["QueueType"] = value;
            }
        }

        public Entity.OrganizationUser OrganizationUser
        {
            get
            {
                if (ViewState["OrganizationUser"] != null)
                    return (Entity.OrganizationUser)ViewState["OrganizationUser"];
                return null;
            }
            set
            {
                ViewState["OrganizationUser"] = value;
            }
        }

        List<InvitationDataContract> ISupportPortalDetailsView.lstInvitationsSent
        {
            set
            {
                lstInvitations = value;
            }
            get
            {
                return lstInvitations;
            }
        }

        public ISupportPortalDetailsView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// Whether the invitation is sent successfully.
        /// </summary>
        Boolean ISupportPortalDetailsView.IsInvitationSent
        {
            get;
            set;
        }

        public List<INTSOF.ServiceDataContracts.Modules.ClinicalRotation.AttestationDocumentContract> LstInvitationDocumentContract
        {
            get;
            set;
        }
        public List<Entity.Tenant> lstTenant
        {
            get;
            set;
        }
        public String OrgUserId
        {
            get
            {
                if (!ViewState["UserId"].IsNull())
                {
                    return Convert.ToString(ViewState["UserId"]);
                }
                return String.Empty;
            }
            set
            {
                ViewState["UserId"] = value;
            }
        }
        public List<SupportPortalOrderDetailContract> lstSuportPortalOrderData
        {
            get
            {
                if (!(ViewState["lstSuportPortalOrderData"] is List<SupportPortalOrderDetailContract>))
                {
                    ViewState["lstSuportPortalOrderData"] = new List<SupportPortalOrderDetailContract>();
                }
                return (List<SupportPortalOrderDetailContract>)ViewState["lstSuportPortalOrderData"];
            }
            set
            {
                ViewState["lstSuportPortalOrderData"] = value;
            }
        }

    
        #endregion

        #endregion

        #region Events

        #region Page Events
        /// <summary>
        /// OnInit event to set page titles
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            try
            {
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                ucApplicantPortfolioProfile.IsControlsHideForSupportPortal = true;
                base.OnInit(e);
                base.SetPageTitle("Support Portal Detail");
                base.Title = "Support Portal";
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
        /// Page_Load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {         
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();

                Dictionary<String, String> args = new Dictionary<String, String>();

                if (!Request.QueryString["args"].IsNull())
                {
                    args.ToDecryptedQueryString(Request.QueryString["args"]);
                    if (args.ContainsKey("SearchInstanceId"))
                    {
                        SearchInstanceId = Convert.ToInt32(args["SearchInstanceId"]);
                    }
                    if (args.ContainsKey("TenantId"))
                    {
                        CurrentViewContext.SelectedTenantId = Convert.ToInt32(args["TenantId"]);
                    }

                    if (args.ContainsKey("MasterPageTabIndex"))
                    {
                        MasterPageTabIndex = Convert.ToInt32(args["MasterPageTabIndex"]);
                    }
             
                    if (!OrganizationUserId.IsNullOrEmpty())
                    {
                        OrgUserId = Convert.ToString(CurrentViewContext.OrganizationUser.UserID);
                    }
                }

                if (!String.IsNullOrEmpty(OrgUserId))
                {
                    Boolean IsMultipletenantUser = Presenter.IsMultiTenantUser(new Guid(OrgUserId));
                    if (IsMultipletenantUser)
                    {
                        Presenter.GetTenants();
                        if (lstTenant.IsNotNull() && lstTenant.Count > AppConsts.ONE)
                        {
                            cmblstTenants.Visible = true;
                            dvTenants.Visible = true;
                            lblTenant.Visible = true;
                            cmblstTenants.DataSource = lstTenant;
                            cmblstTenants.DataBind();
                            cmblstTenants.SelectedValue = SelectedTenantId.ToString();
                        }
                    }
                }
                if (cmblstTenants.Visible == false)
                {
                    dvTenants.Visible = false;
                    lblTenant.Visible = false;
                }
               
               
            }
            HideShowApplicantLoginBtn(!this.IsPostBack);          

            hdnSelectedTenantID.Value = SelectedTenantId.ToString();
            hdnCurrentloggedInUserId.Value = CurrentLoggedInUserId.ToString();
            if (!CurrentViewContext.lstSuportPortalOrderData.IsNullOrEmpty() && CurrentViewContext.lstSuportPortalOrderData.Count > AppConsts.NONE)
            {
                ucSupportPortalNotes.lstOrder = CurrentViewContext.lstSuportPortalOrderData.Where(cond => !cond.BkgOrderId.IsNullOrEmpty() && cond.BkgOrderId > AppConsts.NONE).ToList();
            }
            ucApplicantRequirementRotations.QueueType = WorkQueueType.SupportPortalDetail.ToString();
            ucApplicantRequirementRotations.QueueTypeChild = WorkQueueType.SupportPortalDetail.ToString();
            Presenter.OnViewLoaded();

        }
        #endregion

        #region Button Events


        protected void lnkPortfolioDetails_Click(object sender, EventArgs e)
        {

            Dictionary<String, String> queryString = new Dictionary<String, String>();

            queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "TenantId", Convert.ToString(SelectedTenantId) },
                                                                    { "Child", ChildControls.ApplicantPortfolioDetailPage},
                                                                    { "OrganizationUserId", OrganizationUserId.ToString()},
                                                                    {"PageType", WorkQueueType.SupportPortalDetail.ToString()},
                                                                    {"UserId",CurrentViewContext.OrgUserId},
                                                                    {"PageTypeChild",WorkQueueType.ApplicantPortFolioDetail.ToString()}
                                                                 };
            string url = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
            Response.Redirect(url, true);
        }

        protected void lnkApplicantLogin_Click(object sender, EventArgs e)
        {
            // String applicantUserID = String.Empty;

            #region Switch to Applicant View
            SwitchToApplicant(OrgUserId, SelectedTenantId);
            #endregion

        }

        protected void lnkGoBack_Click(object sender, EventArgs e)
        {
            try
            {
                Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "Child", AppConsts.SUPPORT_PORTAL },
                                                                    {"CancelClick","true"}
                                                                 };
                string url = String.Format("~/SearchUI/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
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
        protected void grdInvitations_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.BindInvitations();
                grdInvitations.DataSource = CurrentViewContext.lstInvitationsSent;
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowInfoMessage("Failed to bind invitation.");
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowInfoMessage("Failed to bind invitation.");
            }
        }
        protected void grdInvitations_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
                {
                    InvitationDataContract gridData = (InvitationDataContract)e.Item.DataItem;
                    LinkButton lnkViewAttestation = (LinkButton)e.Item.FindControl("lnkViewAttestation");
                    lnkViewAttestation.Visible = !Convert.ToBoolean(gridData.IsIndividualShare);
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowInfoMessage("Failed to bind invitation.");
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowInfoMessage("Failed to bind invitation.");
            }
        }
        protected void grdInvitations_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "ViewAttestation")
                {
                    Dictionary<String, String> queryString = new Dictionary<String, String>();
                    String profileSharingInvID = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ID"].ToString();
                    ExportDocuments(Convert.ToInt32(profileSharingInvID));
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowInfoMessage("Failed to download attestation.");
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowInfoMessage("Failed to download attestation.");
            }
        }
        protected void grdSupportPortalOrderDetail_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetSupportPortalOrderDetal();
                grdSupportPortalOrderDetail.DataSource = CurrentViewContext.lstSuportPortalOrderData;
                if (CurrentViewContext.lstSuportPortalOrderData.Count == AppConsts.NONE)
                {
                    Panel pnlBackgroundNotes = ucSupportPortalNotes.FindControl("pnlBackgroundNotes") as Panel;
                    pnlBackgroundNotes.Visible = false;
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowInfoMessage("Failed to bind support portal order data.");
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowInfoMessage("Failed to bind support portal order data.");
            }
        }
        protected void grdSupportPortalOrderDetail_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
                {
                    LinkButton btnArchiveUnArchive = ((LinkButton)e.Item.FindControl("btnArchiveUnArchive"));
                    LinkButton lnkViewDetail = ((LinkButton)e.Item.FindControl("lnkViewDetail"));
                    Label lblOverComplianceStatus = ((Label)e.Item.FindControl("lblOverComplianceStatus"));
                    Image imgPackageComplianceStatus = ((Image)e.Item.FindControl("imgPackageComplianceStatus"));
                    Image imgOrderStatus = (Image)e.Item.FindControl("imgOrderStatus");
                    Label lblPkgRenew = ((Label)e.Item.FindControl("lblPkgRenew"));

                    String compliancePackageStatusCode = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CompliancePackageStatusCode"].IsNullOrEmpty() ? String.Empty : (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CompliancePackageStatusCode"].ToString();
                    String packageTypeCode = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["PackageTypeCode"].IsNullOrEmpty() ? String.Empty : (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["PackageTypeCode"].ToString();
                    String archiveStatus = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ArchiveStatus"].IsNullOrEmpty() ? String.Empty : (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ArchiveStatus"].ToString();
                    String orderStatusCode = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrderStatusCode"].IsNullOrEmpty() ? String.Empty : (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrderStatusCode"].ToString();
                    Boolean isOrderRenewed = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["IsOrderRenewed"].IsNullOrEmpty() ? false : Convert.ToBoolean( (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["IsOrderRenewed"]);

                    lblPkgRenew.Text = "";
                    if (!String.IsNullOrEmpty(archiveStatus) && archiveStatus.ToLower() == "active")
                    {
                        btnArchiveUnArchive.Text = "Archive";
                        btnArchiveUnArchive.CssClass = "autoRenewalLink";
                    }
                    else if (!String.IsNullOrEmpty(archiveStatus) && archiveStatus.ToLower() == "archived")
                    {
                        btnArchiveUnArchive.Text = "Un-Archive";
                        btnArchiveUnArchive.CssClass = "autoRenewalLinkOffButton";
                    }
                    else
                    {
                        btnArchiveUnArchive.Text = String.Empty;
                    }

                    if (!String.IsNullOrEmpty(packageTypeCode))
                    {
                        if (packageTypeCode.ToLower() == OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue().ToLower() || packageTypeCode.ToLower() == OrderPackageTypes.COMPLIANCE_AND_BACKGROUMD_PACKAGE.GetStringValue().ToLower() && !String.IsNullOrEmpty(compliancePackageStatusCode))
                        {
                            if (compliancePackageStatusCode.ToLower() == ApplicantPackageComplianceStatus.Not_Compliant.GetStringValue().ToLower())
                            {
                                imgPackageComplianceStatus.ImageUrl = AppConsts.PACKAGE_NON_COMPLIANCE_IMAGE_URL;
                                lblOverComplianceStatus.ForeColor = System.Drawing.Color.Red;
                            }
                            else
                            {
                                imgPackageComplianceStatus.ImageUrl = AppConsts.PACKAGE_COMPLIANCE_IMAGE_URL;
                                lblOverComplianceStatus.ForeColor = System.Drawing.Color.Green;
                            }

                            lblOverComplianceStatus.Visible = false;
                            imgPackageComplianceStatus.Visible = true;
                            imgOrderStatus.Visible = false;
                        }
                        else
                        {
                            String iconPath = String.Empty;
                            iconPath = ImagePathOrderStatus + "/Blank.gif";
                            imgOrderStatus.ImageUrl = iconPath;
                            imgOrderStatus.Visible = false;
                            Boolean IsServiceGroupFlagged = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["IsServiceGroupFlagged"].IsNullOrEmpty() ? false : Convert.ToBoolean((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["IsServiceGroupFlagged"]);
                            String ServicreGroupName = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ServicreGroupName"].IsNullOrEmpty() ? String.Empty : Convert.ToString((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ServicreGroupName"]);
                            Boolean IsServiceGroupStatusComplete = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["IsServiceGroupStatusComplete"].IsNullOrEmpty() ? false : Convert.ToBoolean((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["IsServiceGroupStatusComplete"]);
                            String OrderStatus = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrderStatus"].IsNullOrEmpty() ? String.Empty : (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrderStatus"].ToString();

                            if (IsServiceGroupStatusComplete)
                            {
                                if (IsServiceGroupFlagged)
                                {
                                    iconPath = ImagePathOrderStatus + "/Red.gif";
                                    imgOrderStatus.AlternateText = String.Concat(ServicreGroupName, " is flagged");
                                }
                                else
                                {
                                    iconPath = ImagePathOrderStatus + "/Green.gif";
                                    imgOrderStatus.AlternateText = String.Concat(ServicreGroupName, " is clear");
                                }

                                imgOrderStatus.ImageUrl = iconPath;
                                lblOverComplianceStatus.Visible = false;
                                imgPackageComplianceStatus.Visible = false;
                                imgOrderStatus.Visible = true;
                            }

                            //if (OrderStatus.ToLower() == "completed")
                            //{
                            //    Boolean IsOrderItemsComplete = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["IsOrderItemsComplete"].IsNullOrEmpty() ? false : Convert.ToBoolean((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["IsOrderItemsComplete"]);

                            //    Boolean OrderFlag = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrderFlag"].IsNullOrEmpty() ? false : Convert.ToBoolean((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrderFlag"]);
                            //    String orderNumber = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrderNumber"].IsNullOrEmpty() ? String.Empty : Convert.ToString((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrderNumber"]);
                            //    if (IsOrderItemsComplete && IsServiceGroupStatusComplete)
                            //    {
                            //        if (OrderFlag)
                            //        {
                            //            iconPath = ImagePathOrderStatus + "/Red.gif";
                            //            imgOrderStatus.AlternateText = String.Concat(orderNumber, " is flagged");
                            //        }
                            //        else
                            //        {
                            //            iconPath = ImagePathOrderStatus + "/Green.gif";
                            //            imgOrderStatus.AlternateText = String.Concat(orderNumber, " is clear");
                            //        }

                            //        imgOrderStatus.ImageUrl = iconPath;
                            //        lblOverComplianceStatus.Visible = false;
                            //        imgPackageComplianceStatus.Visible = false;
                            //        imgOrderStatus.Visible = true;
                            //    }
                            //}                    
                        }
                    }

                    //if (!String.IsNullOrEmpty(orderStatusCode) && !String.IsNullOrEmpty(packageTypeCode) && (packageTypeCode.ToLower() == OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue().ToLower() || packageTypeCode.ToLower() == OrderPackageTypes.COMPLIANCE_AND_BACKGROUMD_PACKAGE.GetStringValue().ToLower()) && orderStatusCode == ApplicantOrderStatus.Cancelled.GetStringValue())
                    //{
                    //    btnArchiveUnArchive.Visible = false;
                    //    lnkViewDetail.Visible = false;
                    //    lblOverComplianceStatus.Visible = false;
                    //    imgPackageComplianceStatus.Visible = false;
                    //    imgOrderStatus.Visible = false;
                    //}

                    if (!String.IsNullOrEmpty(orderStatusCode) && !String.IsNullOrEmpty(packageTypeCode) && (packageTypeCode.ToLower() == OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue().ToLower() || packageTypeCode.ToLower() == OrderPackageTypes.COMPLIANCE_AND_BACKGROUMD_PACKAGE.GetStringValue().ToLower()) && orderStatusCode != ApplicantOrderStatus.Paid.GetStringValue())
                    {
                        btnArchiveUnArchive.Visible = false;
                        lnkViewDetail.Visible = false;
                        lblOverComplianceStatus.Visible = false;
                        imgPackageComplianceStatus.Visible = false;
                        imgOrderStatus.Visible = false;
                        lblPkgRenew.Visible = false;
                        lblPkgRenew.Text = "";
                    }
                    //UAT-3807
                    else if (isOrderRenewed && (packageTypeCode.ToLower() == OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue().ToLower() || packageTypeCode.ToLower() == OrderPackageTypes.COMPLIANCE_AND_BACKGROUMD_PACKAGE.GetStringValue().ToLower()))
                    {
                        lblPkgRenew.Visible = true;
                        lblPkgRenew.Text = AppConsts.COMPLIANCE_PKG_RENEWAL_ORDER_PLACED_TEXT;
                        lnkViewDetail.Visible = false;
                        imgPackageComplianceStatus.Visible = false;
                    }
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowInfoMessage("Failed to bind support portal order data.");
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowInfoMessage("Failed to bind support portal order data.");
            }
        }
        protected void grdSupportPortalOrderDetail_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "ViewDetail")
                {
                    String workQueueType = WorkQueueType.SupportPortalDetail.ToString();

                    Dictionary<String, String> queryString = new Dictionary<String, String>();
                    //String itemDataId = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ApplicantComplianceItemID"].IsNull() ? String.Empty : (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ApplicantComplianceItemID"].ToString();
                    //String selectedCategoryId = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ComplianceCategoryID"].IsNull() ? String.Empty : (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ComplianceCategoryID"].ToString();
                    String selectedPackageSubscriptionId = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["PackageSubscriptionID"].IsNull() ? String.Empty : (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["PackageSubscriptionID"].ToString();

                    if (!String.IsNullOrEmpty(selectedPackageSubscriptionId))
                    {

                        queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "TenantId",Convert.ToString(SelectedTenantId)},
                                                                    { "Child", ChildControls.VerificationDetailsNew},
                                                                    {"SelectedPackageSubscriptionId",Convert.ToString(selectedPackageSubscriptionId)},
                                                                    {"ApplicantId",OrganizationUserId.ToString()},
                                                                    {"WorkQueueType", workQueueType},
                                                                    {"UserId",OrgUserId}

                                                                 };

                        String url = String.Format("~/ComplianceOperations/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                        Response.Redirect(url, true);
                    }
                    else
                    {

                        String orderId = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrderId"].ToString();
                        String orderNumber = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrderNumber"].ToString();


                        queryString = new Dictionary<String, String>
                                                                {
                                                                    { "SelectedTenantId", Convert.ToString(SelectedTenantId) },
                                                                    { "OrderId", orderId},
                                                                    { AppConsts.ORDER_NUMBER, orderNumber},
                                                                    {"OrganizationUserId",OrganizationUserId.ToString()},
                                                                    {"UserId",OrgUserId},
                                                                    {"WorkQueueType", workQueueType}
                                                                 };
                        string url = String.Format("~/BkgOperations/Pages/BkgOrderDetailPage.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                        Response.Redirect(url, true);

                    }

                }

                if (e.CommandName == "ViewOrderDetail")
                {

                    Dictionary<String, String> queryString = new Dictionary<String, String>();
                    String orderId = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrderId"].ToString();
                    String orderNumber = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrderNumber"].ToString();
                    queryString = new Dictionary<String, String>
                                                                                                            {
                                                                    { "SelectedTenantId", Convert.ToString( SelectedTenantId) },
                                                                    { "Child", ChildControls.OrderPaymentDetails},
                                                                    { "OrderId", orderId},
                                                                    {"ShowApproveRejectButtons",true.ToString()},
                                                                    {AppConsts.PARENT_QUEUE_QUERYSTRING, AppConsts.SUPPORT_PORTAL_DETAIL},

                                                                    {AppConsts.ORDER_NUMBER,orderNumber},
                                                                    {"OrganizationUserId",OrganizationUserId.ToString()},
                                                                    {"UserId",OrgUserId}

                                                                 };
                    string url = String.Format("~/ComplianceOperations/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                    Response.Redirect(url, true);
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowInfoMessage("Failed to redirect.");
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowInfoMessage("Failed to redirect.");
            }
        }

        #endregion

        #region DropDown Events
        protected void cmblstTenants_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            SelectedTenantId = Convert.ToInt32(cmblstTenants.SelectedValue);


            Entity.ClientEntity.OrganizationUser OrganizationUser = Presenter.GetOrganizationUserByUserID(OrgUserId, SelectedTenantId,true);
            Int32 ApplicantOrgUserId = OrganizationUser.OrganizationUserID;

            String selectedValue = cmblstTenants.SelectedValue;
            Dictionary<String, String> queryString = new Dictionary<String, String>();
            queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "TenantId", selectedValue},
                                                                    { "OrganizationUserId", ApplicantOrgUserId.ToString()},
                                                                    { "Child", ChildControls.SupportPortalDetail},
                                                                    {"PageType",WorkQueueType.SupportPortalDetail.ToString()},
                                                                     {"UserId", OrgUserId}
                                                                 };
            string url = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
            Response.Redirect(url, true);
        }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// Export Documents
        /// </summary>
        /// <param name="profileSharingInvID"></param>
        private void ExportDocuments(Int32 profileSharingInvID)
        {
            Presenter.GetAttestationDocumentsToExport(profileSharingInvID);
            if (!CurrentViewContext.LstInvitationDocumentContract.IsNullOrEmpty())
            {
                ifrExportDocument.Src = ProfileSharingHelper.ExportAttestationDocument(CurrentViewContext.LstInvitationDocumentContract, CurrentViewContext.SelectedTenantId);
            }
            else
            {
                base.ShowInfoMessage("No document(s) found to export.");
            }
        }

        /// <summary>
        /// Method to switch to Applicant View
        /// </summary>
        private void SwitchToApplicant(String applicantUserID, Int32 tenantID)
        {
            String switchingTargetURL = Presenter.GetSwitchingTargetUrl(tenantID);
            RedirectToTargetSwitchingView(tenantID, applicantUserID, switchingTargetURL);
        }
        /// <summary>
        /// Method To create/update WebApplicationData, Redirect to Target applicant View.
        /// </summary>
        /// <param name="tenantID"></param>
        /// <param name="switchingTargetURL"></param>
        private void RedirectToTargetSwitchingView(Int32 tenantID, String applicantUserID, String switchingTargetURL)
        {
            Dictionary<String, ApplicantInsituteDataContract> applicantData = new Dictionary<String, ApplicantInsituteDataContract>();
            ApplicantInsituteDataContract appInstData = new ApplicantInsituteDataContract();
            appInstData.UserID = applicantUserID;
            appInstData.TagetInstURL = switchingTargetURL;
            appInstData.TokenCreatedTime = DateTime.Now;
            appInstData.TenantID = tenantID;
            appInstData.UserTypeSwitchViewCode = UserTypeSwitchView.Applicant.GetStringValue();
            appInstData.AdminOrgUserID = CurrentLoggedInUserId;
            String key = Guid.NewGuid().ToString();

            Dictionary<String, ApplicantInsituteDataContract> applicationData = Presenter.GetDataByKey("ApplicantInstData");
            if (applicationData != null)
            {
                applicantData = applicationData;
                applicantData.Add(key, appInstData);
                Presenter.UpdateWebApplicationData("ApplicantInstData", applicantData);
            }
            else
            {
                applicantData.Add(key, appInstData);
                Presenter.AddWebApplicationData("ApplicantInstData", applicantData);
            }

            //Redirect to login page
            Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "TokenKey", key  }
                                                                 };

            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "OpenApplicantView('" + String.Format(appInstData.TagetInstURL + "/Login.aspx?TokenKey={0}&DeletePrevUsrState=true", key) + "');", true);
        }

        #region UAT-3407
         private void HideShowApplicantLoginBtn(Boolean isSupportDetailsLoaded)
        {
            if (isSupportDetailsLoaded)
            {
                 WclButton2.Visible=CurrentViewContext.OrganizationUser.IsNullOrEmpty()?false:CurrentViewContext.OrganizationUser.IsActive;
              
            }
            else
            {
                WclButton2.Visible = ucApplicantPortfolioProfile.IsNullOrEmpty() ? false : ucApplicantPortfolioProfile.IsActive;
              
            }
        }
        #endregion
      

        #endregion

        #region Apply Permissions

        public override List<ClsFeatureAction> ActionCollection
        {
            get
            {
                List<Entity.ClientEntity.ClsFeatureAction> actionCollection = new List<Entity.ClientEntity.ClsFeatureAction>();

                actionCollection.Add(new Entity.ClientEntity.ClsFeatureAction
                {
                    CustomActionId = "Save",
                    CustomActionLabel = "Save User Details",
                    ScreenName = "Applicant Portfolio Detail"
                });
                actionCollection.Add(new Entity.ClientEntity.ClsFeatureAction
                {
                    CustomActionId = "Reset",
                    CustomActionLabel = "Reset Password",
                    ScreenName = "Applicant Portfolio Detail"
                });
                return actionCollection;
            }

        }

        //protected override void ApplyActionLevelPermission(List<ClsFeatureAction> ctrlCollection, string screenName = "")
        //{
        //    base.ApplyActionLevelPermission(ctrlCollection, screenName);
        //    List<Entity.FeatureRoleAction> permission = base.ActionPermission;
        //    permission.ForEach(x =>
        //    {
        //        switch (x.PermissionID)
        //        {
        //            case AppConsts.ONE:
        //                {
        //                    break;
        //                }
        //            case AppConsts.THREE:
        //                {
        //                    if (x.FeatureAction.CustomActionId == "Save")
        //                    {
        //                        fsucCmdBarPortfolio.SaveButton.Enabled = false;
        //                    }
        //                    else if (x.FeatureAction.CustomActionId == "Reset")
        //                    {
        //                        fsucCmdBarPortfolio.ExtraButton.Enabled = false;
        //                    }
        //                    break;
        //                }
        //            case AppConsts.FOUR:
        //                {
        //                    if (x.FeatureAction.CustomActionId == "Save")
        //                    {
        //                        fsucCmdBarPortfolio.HideButtons(CommandBarButtons.Save);
        //                    }
        //                    else if (x.FeatureAction.CustomActionId == "Reset")
        //                    {
        //                        fsucCmdBarPortfolio.HideButtons(CommandBarButtons.Extra);
        //                    }
        //                    break;
        //                }
        //        }

        //    }
        //        );
        //}

        #endregion
     
    }
}

