#region Namespaces

#region System Defined

using System;
using System.Linq;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using Microsoft.Practices.ObjectBuilder;
using System.Collections;

#endregion

#region Application Specific

using INTSOF.Utils;
using Telerik.Web.UI;
using INTSOF.UI.Contract.IntsofSecurityModel;
using CoreWeb.CommonControls.Views;
using INTSOF.Utils.Consts;
using INTERSOFT.WEB.UI.WebControls;
//using Microsoft.Practices.CompositeWeb.Web.UI;
using Entity.ClientEntity;
using CoreWeb.Shell;
using System.Xml.Serialization;
using System.IO;
using System.Web;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.ServiceDataContracts.Modules.Common;
using System.Web.Configuration;
using CoreWeb.IntsofSecurityModel;

#endregion

#endregion

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class VerificationApplicantPanel : BaseUserControl, IVerificationApplicantPanelView
    {
        #region Variables

        private VerificationApplicantPanelPresenter _presenter = new VerificationApplicantPanelPresenter();
        private System.Delegate _ReLoadDataItemPanel;
        private Int32 tenantId = 0;

        #region Private Variables

        private String _viewType;
        private CustomPagingArgsContract _verificationGridCustomPaging = null;
        private CustomPagingArgsContract _exceptionGridCustomPaging = null;

        #endregion

        #endregion

        #region Properties

        public System.Delegate ReLoadDataItemPanel
        {
            set { _ReLoadDataItemPanel = value; }
        }

        public VerificationApplicantPanelPresenter Presenter
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
        /// Gets the current view context.
        /// </summary>
        /// <remarks></remarks>

        public IVerificationApplicantPanelView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// Get or Set the Tenant ID
        /// </summary>


        public Entity.Tenant Tenant
        {
            get
            {
                if (ViewState["Tenant"] == null)
                    ViewState["Tenant"] = Presenter.GetTenant(this.TenantId_Global);
                return (Entity.Tenant)ViewState["Tenant"];
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

        public List<INTSOF.Utils.CommonPocoClasses.ComplianceCategoryPocoClass> ApplicantComplianceCategoryDataList
        {
            get
            {
                if (ViewState["ApplicantComplianceCategoryDataList"] != null)
                    return (List<INTSOF.Utils.CommonPocoClasses.ComplianceCategoryPocoClass>)ViewState["ApplicantComplianceCategoryDataList"];
                return null;
            }
            set
            {
                ViewState["ApplicantComplianceCategoryDataList"] = value;
            }
        }

        public String viewType
        {
            get
            {
                if (ViewState["viewType"] != null)
                    return (String)ViewState["viewType"];
                return null;
            }
            set
            {
                ViewState["viewType"] = value;
            }
        }

        public Boolean IsException
        {
            get
            {
                return (Boolean)(ViewState["IsException_Applicant"]);
            }
            set
            {
                ViewState["IsException_Applicant"] = value;
            }
        }

        public WorkQueueType WorkQueue
        {
            get
            {
                if (ViewState["WorkQueue"] != null)
                    return (WorkQueueType)ViewState["WorkQueue"];
                return WorkQueueType.AssignmentWorkQueue;
            }
            set
            {
                ViewState["WorkQueue"] = value;
            }
        }

        //public List<Int32?> PackageSubscriptionIdList
        public List<PkgSubscriptionIDList> PackageSubscriptionIdList
        {
            get
            {
                if (ViewState["PackageSubscriptionIdList"] != null)
                    return (List<PkgSubscriptionIDList>)ViewState["PackageSubscriptionIdList"];
                return null;
            }
            set
            {
                ViewState["PackageSubscriptionIdList"] = value;
            }
        }


        public Int32 CurrentLoggedInUserId
        {

            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }
        public Entity.Tenant LoggedInUser
        {
            get
            {
                if (ViewState["LoggedInUser"] == null)
                    ViewState["LoggedInUser"] = Presenter.GetTenant(this.CurrentLoggedInUserId);
                return (Entity.Tenant)ViewState["LoggedInUser"];
            }
        }

        public Entity.OrganizationUser OrganizationUserData
        {
            get
            {
                if (ViewState["OrganizationUserData"] != null)
                    return (Entity.OrganizationUser)ViewState["OrganizationUserData"];
                return null;
            }
            set
            {
                ViewState["OrganizationUserData"] = value;
            }
        }
        #region UAT-749:WB: Addition of "User Groups" to left panel of Verification Details screen
        public List<Entity.ClientEntity.UserGroup> UserGroupDataList
        {
            get
            {
                if (ViewState["UserGroupDataList"] != null)
                    return (List<Entity.ClientEntity.UserGroup>)ViewState["UserGroupDataList"];
                return null;
            }
            set
            {
                ViewState["UserGroupDataList"] = value;
            }
        }
        #endregion

        public String OrganizationUserName
        {
            get { return (String)(ViewState["OrganizationUserName"] ?? ""); }
            set { ViewState["OrganizationUserName"] = value; }
        }


        /// <summary>
        /// set IncludeIncompleteItems for return back to queue.
        /// </summary>
        public Boolean IncludeIncompleteItems
        {
            get
            {
                if (!ViewState["IncludeIncompleteItems"].IsNull())
                {
                    return (Boolean)ViewState["IncludeIncompleteItems"];
                }
                return false;
            }
            set
            {
                ViewState["IncludeIncompleteItems"] = value;
            }
        }

        public Boolean ShowOnlyRushOrders
        {
            get
            {
                if (!ViewState["ShowOnlyRushOrders"].IsNull())
                {
                    return (Boolean)ViewState["ShowOnlyRushOrders"];
                }
                return false;
            }
            set
            {
                ViewState["ShowOnlyRushOrders"] = value;
            }
        }

        public Boolean IsRushOrder
        {
            get
            {
                if (!ViewState["IsRushOrder"].IsNull())
                {
                    return (Boolean)ViewState["IsRushOrder"];
                }
                return false;
            }
            set
            {
                ViewState["IsRushOrder"] = value;
            }
        }


        public string UIInputException { get; set; }



        public Int32 SelectedOrderId
        {
            get
            {
                return Convert.ToInt32(ViewState["SelectedOrderId"] ?? "0");
            }
            set
            {
                ViewState["SelectedOrderId"] = value;
            }
        }

        public String SelectedOrderNumber
        {
            get
            {
                return Convert.ToString(ViewState["SelectedOrderNumber"] ?? String.Empty);
            }
            set
            {
                ViewState["SelectedOrderNumber"] = value;
            }
        }
        /// <summary>
        /// set package id for return back to queue.
        /// </summary>
        public Int32 PackageId
        {
            get
            {
                return Convert.ToInt32(ViewState["PackageId"] ?? "0");
            }
            set
            {
                ViewState["PackageId"] = value;
            }
        }

        /// <summary>
        /// set Category id for return back to queue.
        /// </summary>
        public Int32 CategoryId
        {
            get
            {
                return Convert.ToInt32(ViewState["CategoryId"] ?? "0");
            }
            set
            {
                ViewState["CategoryId"] = value;
            }
        }

        /// <summary>
        /// set user group id for return back to queue.
        /// </summary>
        public Int32 UserGroupId
        {
            get
            {
                return Convert.ToInt32(ViewState["UserGroupId"] ?? "0");
            }
            set
            {
                ViewState["UserGroupId"] = value;
            }
        }

        public List<ApplicantDocuments> lstApplicantDocument
        {
            get;
            set;
        }

        /// <summary>
        /// get object of shared class of custom paging
        /// </summary>
        public CustomPagingArgsContract VerificationGridCustomPaging
        {
            get
            {
                if (_verificationGridCustomPaging.IsNull())
                {
                    //var serializer = new XmlSerializer(typeof(CustomPagingArgsContract));
                    if (!String.IsNullOrEmpty(Convert.ToString(Session[AppConsts.VERIFICATION_QUEUE_SESSION_KEY])))
                    {
                        //TextReader reader = new StringReader(Convert.ToString(Session[AppConsts.VERIFICATION_QUEUE_SESSION_KEY]));
                        _verificationGridCustomPaging = (CustomPagingArgsContract)(Session[AppConsts.VERIFICATION_QUEUE_SESSION_KEY]);
                    }
                }
                return _verificationGridCustomPaging;
            }
        }

        public String CurrentCompliancePackageStatus
        {
            get { return (String)(ViewState["CurrentCompliancePackageStatus"] ?? ""); }
            set { ViewState["CurrentCompliancePackageStatus"] = value; }
        }

        public String CurrentCompliancePackageStatusCode
        {
            get { return (String)(ViewState["CurrentCompliancePackageStatusCode"] ?? ""); }
            set { ViewState["CurrentCompliancePackageStatusCode"] = value; }
        }

        /// <summary>
        ///get and  set package id .
        /// </summary>
        public Int32 PrevPackageSubscriptionID
        {
            get { return Convert.ToInt32(Session["PrevPackageSubscriptionID"] ?? "0"); }
            set { Session["PrevPackageSubscriptionID"] = value; }
        }
        public Int32 NextPackageSubscriptionID
        {
            get { return Convert.ToInt32(Session["NextPackageSubscriptionID"] ?? "0"); }
            set { Session["NextPackageSubscriptionID"] = value; }
        }
        public Int32 PrevAppCmpItemID
        {
            get { return Convert.ToInt32(ViewState["PrevAppCmpItemID"] ?? "0"); }
            set { ViewState["PrevAppCmpItemID"] = value; }
        }
        public Int32 NextAppCmpItemID
        {
            get { return Convert.ToInt32(ViewState["NextAppCmpItemID"] ?? "0"); }
            set { ViewState["NextAppCmpItemID"] = value; }
        }

        public Int32 ReviewerUserId
        {
            get
            {
                if (ViewState["ReviewerUserId"] != null)
                    return Convert.ToInt32(ViewState["ReviewerUserId"]);
                return 0;
            }
            set
            {
                ViewState["ReviewerUserId"] = value;
            }
        }

        public String CurrentPackageBredCrum
        {
            get { return (String)(ViewState["CurrentPackageBredCrum"] ?? ""); }
            set { ViewState["CurrentPackageBredCrum"] = value; }
        }

        public Int32 SubPageIndex
        {
            get
            {
                if (Session["SubPageIndex"] != null)
                    return Convert.ToInt32(Session["SubPageIndex"]);
                return 0;
            }
            set
            {
                Session["SubPageIndex"] = value;
            }
        }
        public Int32 SubTotalPages
        {
            get
            {
                if (Session["SubTotalPages"] != null)
                    return Convert.ToInt32(Session["SubTotalPages"]);
                return 0;
            }
            set
            {
                Session["SubTotalPages"] = value;
            }
        }

        public Boolean IsEscalationRecords
        {
            get
            {
                if (ViewState["IsEscalationRecords"] != null)
                    return Convert.ToBoolean(ViewState["IsEscalationRecords"]);
                return false;
            }
            set
            {
                ViewState["IsEscalationRecords"] = value;
            }
        }

        /// <summary>
        /// Data of the Applicant.
        /// </summary>
        OrganizationUserContract IVerificationApplicantPanelView.ApplicantData
        {
            get;
            set;
        }

        #region UAT-806 Creation of granular permissions for Client Admin users
        public Boolean IsDOBDisable
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsDOBDisabled"] ?? false);
            }
            set
            {
                ViewState["IsDOBDisabled"] = value;
            }
        }

        String IVerificationApplicantPanelView.SSNPermissionCode
        {
            get
            {
                return Convert.ToString(ViewState["SSNPermissionCode"]);
            }
            set
            {
                ViewState["SSNPermissionCode"] = value;
            }
        }

        DateTime? IVerificationApplicantPanelView.OrderApprovalDate
        {
            get
            {
                return Convert.ToDateTime(ViewState["OrderApprovalDate"]);
            }
            set
            {
                ViewState["OrderApprovalDate"] = value;
            }
        }

        DateTime? IVerificationApplicantPanelView.SubscriptionExpirationDate
        {
            get;
            set;
        }

        #endregion

        #region UAT-2460
        public String SelectedArchiveStateCode
        {
            get
            {
                if (ViewState["SelectedArchiveStateCode"] != null)
                    return Convert.ToString(ViewState["SelectedArchiveStateCode"]);
                return String.Empty;
            }
            set
            {
                ViewState["SelectedArchiveStateCode"] = value;
            }
        }
        #endregion

        #region UAT-3744
        public String SelectedComplianceItemReconciliationDataID
        {
            get
            {
                if (ViewState["SelectedComplianceItemReconciliationDataID"].IsNotNull())
                    return Convert.ToString(ViewState["SelectedComplianceItemReconciliationDataID"]);
                return String.Empty;
            }
            set
            {
                ViewState["SelectedComplianceItemReconciliationDataID"] = value;
            }
        }
        #endregion

        #region Private Properties

        #region UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
        private String NoMiddleNameText
        {
            get
            {
                String noMiddleNameText = WebConfigurationManager.AppSettings[AppConsts.NO_MIDDLE_NAME_TEXT_KEY];
                if (noMiddleNameText.IsNull())
                {
                    noMiddleNameText = String.Empty;
                }
                return noMiddleNameText;
            }
        }
        #endregion
        #endregion
        #endregion

        #region "Global Properties"

        public Int32 TenantId_Global
        {
            get { return Convert.ToInt32(ViewState["TenantId"] ?? "0"); }
            set { ViewState["TenantId"] = value; }
        }

        /// <summary>
        ///get and  set Applicant id 
        /// </summary>
        public Int32 CurrentApplicantId_Global
        {
            get { return Convert.ToInt32(ViewState["CurrentApplicantId"] ?? "0"); }
            set { ViewState["CurrentApplicantId"] = value; }
        }

        /// <summary>
        ///get and  set Category id 
        /// </summary>
        public Int32 SelectedComplianceCategoryId_Global
        {
            get { return Convert.ToInt32(ViewState["SelectedComplianceCategoryId"] ?? "0"); }
            set { ViewState["SelectedComplianceCategoryId"] = value; }
        }

        public Int32 PrevComplianceCategoryId_Global
        {
            get { return Convert.ToInt32(ViewState["PrevComplianceCategoryId_Global"] ?? "0"); }
            set { ViewState["PrevComplianceCategoryId_Global"] = value; }
        }

        public Int32 NextComplianceCategoryId_Global
        {
            get { return Convert.ToInt32(ViewState["NextComplianceCategoryId_Global"] ?? "0"); }
            set { ViewState["NextComplianceCategoryId_Global"] = value; }
        }

        public Int32 SelectedPackageSubscriptionID_Global
        {
            get { return Convert.ToInt32(ViewState["SelectedPackageSubscriptionID"] ?? "0"); }
            set { ViewState["SelectedPackageSubscriptionID"] = value; }
        }

        /// <summary>
        ///get and  set package id .
        /// </summary>
        public Int32 CurrentCompliancePackageId_Global
        {
            get { return Convert.ToInt32(ViewState["CurrentCompliancePackageId"] ?? "0"); }
            set { ViewState["CurrentCompliancePackageId"] = value; }
        }

        public Int32 ItemDataId_Global
        {
            get { return Convert.ToInt32(ViewState["ItemDataId"] ?? "0"); }
            set { ViewState["ItemDataId"] = value; }
        }

        public String packageName
        {
            get { return (String)(ViewState["PackageName"] ?? ""); }
            set { ViewState["PackageName"] = value; }
        }

        public String ActionType
        {
            get;
            set;
        }

        //UAT-509 WB: Ability to limit admin access to read only on the ver details and applicant search details screen
        public Boolean IsFullPermissionForVerification
        {
            get
            {
                return (Boolean)(ViewState["IsFullPermissionForVerification"]);
            }
            set
            {
                ViewState["IsFullPermissionForVerification"] = value;
            }
        }

        public Boolean OptionalCategoryClientSetting
        {
            get
            {
                if (!ViewState["OptionalCategoryClientSetting"].IsNullOrEmpty())
                {
                    return (Boolean)(ViewState["OptionalCategoryClientSetting"]);
                }
                return false;
            }
            set
            {
                ViewState["OptionalCategoryClientSetting"] = value;
            }
        }
        public String AllowedFileExtensions
        {
            get
            {
                if (!ViewState["AllowedFileExtensions"].IsNull())
                {
                    return Convert.ToString(ViewState["AllowedFileExtensions"]);
                }
                return String.Empty;
            }
            set
            {
                ViewState["AllowedFileExtensions"] = value;
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// OnInit event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            try
            {

                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                base.OnInit(e);
                base.Title = "Verification Detail";
                BasePage basePage = base.Page as BasePage;
                if (basePage != null)
                {
                    basePage.HideTitleBars();
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

        public event DataSaved DataSavedClick;
        public delegate void DataSaved(object sender, EventArgs e);

        /// <summary>
        /// Page Load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
                //UAT 3106
                //commented for UAT 3683
                //ClientSetting OptionalCategorySetting = Presenter.GetClientSettingByCode(); 
                //if (!OptionalCategorySetting.IsNullOrEmpty())
                //{
                //OptionalCategoryClientSetting = OptionalCategorySetting.CS_SettingValue == AppConsts.STR_ONE ? true : false;
                //}
                OptionalCategoryClientSetting = Presenter.GetOptionalCategorySettingNode(); //UAT 3683
                SetPageDataAndLayout(!this.IsPostBack);

            }
            Presenter.OnViewLoaded();
        }

        protected void btnPrevApp_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (this.PrevPackageSubscriptionID == AppConsts.MINUS_TWO)
            {
                this.SubPageIndex--;
                this.SelectedPackageSubscriptionID_Global = AppConsts.MINUS_TWO;
            }
            else
            {
                this.SelectedPackageSubscriptionID_Global = this.PrevPackageSubscriptionID;
                this.ItemDataId_Global = this.PrevAppCmpItemID;
            }

            ComplianceOperationsVerifications.RedirectToVerificationDetailScreen(this.TenantId_Global,
                    this.ItemDataId_Global,
                    this.WorkQueue,
                    this.PackageId,
                    this.CategoryId,
                    this.UserGroupId,
                    this.IncludeIncompleteItems,
                    this.SelectedPackageSubscriptionID_Global,
                    this.SelectedComplianceCategoryId_Global,
                    ChildControls.VerificationDetailsNew,
                    this.viewType, this.ShowOnlyRushOrders, IsException, IsEscalationRecords, this.SelectedComplianceItemReconciliationDataID);

        }

        protected void btnNextApp_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {

            if (this.NextPackageSubscriptionID == AppConsts.MINUS_ONE)
            {
                this.SubPageIndex++;
                this.SelectedPackageSubscriptionID_Global = AppConsts.MINUS_ONE;
            }
            else
            {
                this.SelectedPackageSubscriptionID_Global = this.NextPackageSubscriptionID;
                this.ItemDataId_Global = NextAppCmpItemID;
            }

            ComplianceOperationsVerifications.RedirectToVerificationDetailScreen(this.TenantId_Global,
                this.ItemDataId_Global,
                this.WorkQueue,
                this.PackageId,
                this.CategoryId,
                this.UserGroupId,
                this.IncludeIncompleteItems,
                this.SelectedPackageSubscriptionID_Global,
                this.SelectedComplianceCategoryId_Global,
                ChildControls.VerificationDetailsNew,
                this.viewType, this.ShowOnlyRushOrders, IsException, IsEscalationRecords, this.SelectedComplianceItemReconciliationDataID);
        }

        protected void lstCategories_DataBound(object sender, RadListBoxItemEventArgs e)
        {
            RadListBoxItem item = e.Item as RadListBoxItem;
            INTSOF.Utils.CommonPocoClasses.ComplianceCategoryPocoClass ccpc = (INTSOF.Utils.CommonPocoClasses.ComplianceCategoryPocoClass)item.DataItem;
            HtmlAnchor lnkCategoriesNavigation = (HtmlAnchor)e.Item.FindControl("lnkCategoriesNavigation");
            //RadButton btnCategorylnk = (RadButton)e.Item.FindControl("btnCategorylnk");
            //if (btnCategorylnk.IsNotNull())
            if (lnkCategoriesNavigation.IsNotNull())
            {
                String qs = ComplianceOperationsVerifications.GetVerificationDetailScreenQueryString(this.TenantId_Global,
                        this.ItemDataId_Global,
                        this.WorkQueue,
                        this.PackageId,
                        this.CategoryId,
                        this.UserGroupId,
                        this.IncludeIncompleteItems,
                        this.SelectedPackageSubscriptionID_Global,
                        Convert.ToInt32(ccpc.CategoryId),
                        ChildControls.VerificationDetailsNew,
                        this.viewType,
                        this.ShowOnlyRushOrders, IsException, CurrentApplicantId_Global, VerificationDetailActionType.CategoryPreviousNextLeft.GetStringValue(), IsEscalationRecords, this.SelectedArchiveStateCode, this.SelectedComplianceItemReconciliationDataID);

                String unifiedDocPageMapping = String.Empty;
                String appDocumentIds = String.Empty;
                lnkCategoriesNavigation.Attributes.Add("CategoryID", Convert.ToString(ccpc.CategoryId));
                //btnCategorylnk.Attributes.Add("CategoryID", Convert.ToString(ccpc.CategoryId));
                var selectedCatUnifiedDocList = lstApplicantDocument.Where(cond => cond.CategoryID == ccpc.CategoryId).ToList();
                if (selectedCatUnifiedDocList.IsNotNull() && selectedCatUnifiedDocList.Count() > 0)
                {

                    Int16 counter = 0;
                    foreach (var item1 in selectedCatUnifiedDocList)
                    {
                        if (counter == AppConsts.NONE)
                        {
                            unifiedDocPageMapping = item1.UnifiedDocumentStartPageID + "-" + item1.UnifiedDocumentEndPageID;
                        }
                        else
                        {
                            unifiedDocPageMapping = unifiedDocPageMapping + "," + item1.UnifiedDocumentStartPageID + "-" + item1.UnifiedDocumentEndPageID;
                        }
                        counter++;
                        appDocumentIds = appDocumentIds + "," + item1.ApplicantDocumentId;
                    }
                    lnkCategoriesNavigation.Attributes.Add("UnifiedDocPageMapping", Convert.ToString(unifiedDocPageMapping));
                    //UAT-1538:
                    lnkCategoriesNavigation.Attributes.Add("appDocumentIds", Convert.ToString(appDocumentIds));
                    //btnCategorylnk.Attributes.Add("UnifiedDocPageMapping", Convert.ToString(unifiedDocPageMapping));
                    ////UAT-1538:
                    //btnCategorylnk.Attributes.Add("appDocumentIds", Convert.ToString(appDocumentIds));

                }
                else
                {
                    lnkCategoriesNavigation.Attributes.Add("UnifiedDocPageMapping", Convert.ToString(unifiedDocPageMapping));
                    //UAT-1538:
                    lnkCategoriesNavigation.Attributes.Add("appDocumentIds", Convert.ToString(appDocumentIds));
                    //btnCategorylnk.Attributes.Add("UnifiedDocPageMapping", Convert.ToString(unifiedDocPageMapping));
                    ////UAT-1538:
                    //btnCategorylnk.Attributes.Add("appDocumentIds", Convert.ToString(appDocumentIds));
                }
                lnkCategoriesNavigation.HRef = String.Format("~/ComplianceOperations/Default.aspx?ucid={0}&args={1}", _viewType, qs.ToString());

                //btnCategorylnk.Attributes.Add("Url", String.Format("Default.aspx?ucid={0}&args={1}", _viewType, qs.ToString()));
                //btnCategorylnk.Icon.PrimaryIconUrl = GetImageUrl(ccpc.CategoryStatusCode, ccpc.CategoryExceptionStatusCode, ccpc.IsComplianceRequired);
                //btnCategorylnk.Icon.PrimaryIconHeight = 20;
                //btnCategorylnk.Icon.PrimaryIconWidth = 20;
                //btnCategorylnk.ToolTip = GetStatus(ccpc.CategoryStatusCode, ccpc.CategoryExceptionStatusCode, ccpc.CategoryStatusName);
            }
        }

        protected void lstCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SelectedComplianceCategoryId_Global = Convert.ToInt32(lstCategories.SelectedValue);
            List<Int32> categoryIdsOfAssignedItems = Presenter.GetCategoryListOfAssignedItems();
            //Set the category id for the Assigned category on next applicant click.
            if ((WorkQueue == WorkQueueType.UserWorkQueue || WorkQueue == WorkQueueType.EsclationUserWorkQueue || (WorkQueue == WorkQueueType.UserWorkQueue && IsException)) && (this.ActionType == VerificationDetailActionType.SubscriptionPreviousNext.GetStringValue()))
            {
                if (categoryIdsOfAssignedItems.IsNotNull() && categoryIdsOfAssignedItems.Count > 0 && !categoryIdsOfAssignedItems.Contains(SelectedComplianceCategoryId_Global))
                {
                    var categoryListItem = lstCategories.Items.FirstOrDefault(cond => categoryIdsOfAssignedItems.Contains(Convert.ToInt32(cond.Value)));
                    if (categoryListItem.IsNotNull())
                    {
                        String categoryIDToSelect = categoryListItem.Value;
                        this.SelectedComplianceCategoryId_Global = Convert.ToInt32(categoryIDToSelect);
                        lstCategories.SelectedValue = categoryIDToSelect;
                    }
                }
            }
            //To set the prev category for navigation buttons on middle panel
            if (lstCategories.SelectedIndex > 0)
            {
                if (WorkQueue == WorkQueueType.UserWorkQueue || WorkQueue == WorkQueueType.EsclationUserWorkQueue || (WorkQueue == WorkQueueType.UserWorkQueue && IsException))
                {
                    var tempCategoryListPrev = lstCategories.Items.Where(cnd => cnd.Index < lstCategories.SelectedIndex).OrderByDescending(ord => ord.Index);
                    foreach (var category in tempCategoryListPrev) //lstCategories.Items.Where(cnd => cnd.Index < lstCategories.SelectedIndex))
                    {
                        Int32 prevCategoryIdTemp = Convert.ToInt32(category.Value);
                        if (categoryIdsOfAssignedItems.Contains(prevCategoryIdTemp))
                        {
                            this.PrevComplianceCategoryId_Global = prevCategoryIdTemp;
                            break;
                        }
                        else
                        {
                            this.PrevComplianceCategoryId_Global = 0;
                        }
                    }
                }
                else
                    this.PrevComplianceCategoryId_Global = Convert.ToInt32(lstCategories.Items[lstCategories.SelectedIndex - 1].Value);
            }
            else
                this.PrevComplianceCategoryId_Global = 0;
            //To set the next category for navigation buttons on middle panel
            if (lstCategories.SelectedIndex < (lstCategories.Items.Count - 1))
            {
                if (WorkQueue == WorkQueueType.UserWorkQueue || WorkQueue == WorkQueueType.EsclationUserWorkQueue || (WorkQueue == WorkQueueType.UserWorkQueue && IsException))
                {
                    var tempCategoryList = lstCategories.Items.Where(cnd => cnd.Index > lstCategories.SelectedIndex);
                    foreach (var category in tempCategoryList)// lstCategories.Items.Where(cnd => cnd.Index > lstCategories.SelectedIndex))
                    {
                        Int32 nextCategoryIdTemp = Convert.ToInt32(category.Value);
                        if (categoryIdsOfAssignedItems.Contains(nextCategoryIdTemp))
                        {
                            this.NextComplianceCategoryId_Global = nextCategoryIdTemp;
                            break;
                        }
                        else
                        {
                            this.NextComplianceCategoryId_Global = 0;
                        }
                    }

                }
                else
                    this.NextComplianceCategoryId_Global = Convert.ToInt32(lstCategories.Items[lstCategories.SelectedIndex + 1].Value);
            }
            else
                this.NextComplianceCategoryId_Global = 0;

            if (sender != null)
            {
                ComplianceOperationsVerifications.RedirectToVerificationDetailScreen(this.TenantId_Global,
                        this.ItemDataId_Global,
                        this.WorkQueue,
                        this.PackageId,
                        this.CategoryId,
                        this.UserGroupId,
                        this.IncludeIncompleteItems,
                        this.SelectedPackageSubscriptionID_Global,
                        this.SelectedComplianceCategoryId_Global,
                        ChildControls.VerificationDetailsNew,
                        this.viewType, this.ShowOnlyRushOrders, IsException, IsEscalationRecords, this.SelectedComplianceItemReconciliationDataID);
            }
            else
            {
                Dictionary<string, string> objDic = new Dictionary<string, string>();
                objDic.Add("SelectedComplianceCategoryId_Global", this.SelectedComplianceCategoryId_Global.ToString());
                objDic.Add("PrevComplianceCategoryId_Global", this.PrevComplianceCategoryId_Global.ToString());
                objDic.Add("NextComplianceCategoryId_Global", this.NextComplianceCategoryId_Global.ToString());
                objDic.Add("TenantId_Global", this.TenantId_Global.ToString());
                objDic.Add("CurrentCompliancePackageId_Global", this.CurrentCompliancePackageId_Global.ToString());
                objDic.Add("CurrentApplicantId_Global", this.CurrentApplicantId_Global.ToString());
                objDic.Add("SelectedPackageSubscriptionID_Global", this.SelectedPackageSubscriptionID_Global.ToString());
                objDic.Add("ItemDataId_Global", this.ItemDataId_Global.ToString());
                objDic.Add("UserGroupId", this.UserGroupId.ToString());
                objDic.Add("ComplianceItemReconciliationDataID_Global", this.SelectedComplianceItemReconciliationDataID);//UAT-3744
                this._ReLoadDataItemPanel.DynamicInvoke(objDic);
            }
        }
        #endregion

        #region Methods

        #region Private Methods

        private void BindAttributes()
        {
            Presenter.GetApplicantData();
            lblAddress.Text = CurrentViewContext.ApplicantData.Address1.HtmlEncode();
            lblAddress.Text += ", " + CurrentViewContext.ApplicantData.City.HtmlEncode();
            lblAddress.Text += ", " + CurrentViewContext.ApplicantData.State.HtmlEncode();
            lblAddress.Text += ", " + CurrentViewContext.ApplicantData.Country;
            lblAddress.Text += " " + CurrentViewContext.ApplicantData.ZipCode.HtmlEncode();

            if (CurrentViewContext.SubscriptionExpirationDate.IsNotNull())
            {
                lblExpirationDate.Text = CurrentViewContext.SubscriptionExpirationDate.Value.ToShortDateString();
            }

            if (!this.OrganizationUserData.FirstName.IsNullOrEmpty())
            {
                lblApplicantName.Text = this.OrganizationUserData.FirstName.HtmlEncode();
            }
            if (!this.OrganizationUserData.MiddleName.IsNullOrEmpty())
            {
                lblApplicantMiddleName.Text = this.OrganizationUserData.MiddleName.HtmlEncode();
                divUser.Attributes.Add("style", "vertical-align:middle; height:90%; padding-top:4px;");
            }
            if (!this.OrganizationUserData.LastName.IsNullOrEmpty())
            {
                lblApplicantLastName.Text = this.OrganizationUserData.LastName.HtmlEncode();
            }
            lblEmail.Text = this.OrganizationUserData.PrimaryEmailAddress.HtmlEncode();
            //UAT-1696 : Add Secondary email to the left panel of the verification details screen
            if (!this.OrganizationUserData.SecondaryEmailAddress.IsNullOrEmpty())
            {
                lblSecondaryEmail.Text = this.OrganizationUserData.SecondaryEmailAddress.HtmlEncode();
                dvSecondaryEmail.Visible = true;
            }
            else
            {
                dvSecondaryEmail.Visible = false;
            }
            lblOrder.Text = this.SelectedOrderNumber.ToString().HtmlEncode();
            lblPhones.Text = (this.OrganizationUserData.IsInternationalPhoneNumber ? this.OrganizationUserData.PhoneNumber : Presenter.GetFormattedPhoneNumber(this.OrganizationUserData.PhoneNumber)) + ", " + (this.OrganizationUserData.IsInternationalSecondaryPhone ? this.OrganizationUserData.SecondaryPhone : Presenter.GetFormattedPhoneNumber(this.OrganizationUserData.SecondaryPhone));
            lblBredCrum.Text = (this.CurrentPackageBredCrum + " > " + this.packageName).HtmlEncode();
            lblOverComplianceStatus.Text = this.CurrentCompliancePackageStatus;

            if (CurrentViewContext.OrderApprovalDate.HasValue)
            {
                lblOrderApprovalDate.Text = CurrentViewContext.OrderApprovalDate.Value.ToShortDateString();
            }
            else
            {
                lblOrderApprovalDate.Text = "N/A";
            }

            //UAT-806 Creation of granular permissions for Client Admin users
            if (CurrentViewContext.IsDOBDisable)
            {
                lblApplicantDOB.Visible = false;
            }
            else
                lblApplicantDOB.Visible = true;
            //Date of birth of the applicant
            lblApplicantDOB.Text = this.OrganizationUserData.DOB.HasValue ? "(" + this.OrganizationUserData.DOB.Value.ToShortDateString() + ")" : "";
            lblApplicantDOB.ToolTip = this.OrganizationUserData.DOB.HasValue ? "DOB: " + this.OrganizationUserData.DOB.Value.ToString("MMMM d, yyyy") : "";

            String unformattedSSN = Presenter.GetApplicantSSN();

            if (Presenter.IsDefaultTenant || CurrentViewContext.SSNPermissionCode.ToUpper() == EnumSystemPermissionCode.MASKED_READ_PERMISSION.GetStringValue())
            {
                lblSSN.Text = Presenter.GetMaskedSSN(unformattedSSN);
            }
            else if (CurrentViewContext.SSNPermissionCode.ToUpper() == EnumSystemPermissionCode.NO_ACCESS_PERMISSION.GetStringValue())
            {
                divSSN.Visible = false;
            }
            else
            {
                lblSSN.Text = Presenter.GetFormattedSSN(unformattedSSN);
            }

            //Alias of the applicant
            if (this.OrganizationUserData.PersonAlias.IsNotNull())
            {
                String AliasName = String.Empty;
                //Implemented Changes for middle name related to UAT-2212
                this.OrganizationUserData.PersonAlias.Where(x => !x.PA_IsDeleted)
                                         .ForEach(x => AliasName += Convert.ToString(x.PA_FirstName + " " + (x.PA_MiddleName.IsNullOrEmpty() ? NoMiddleNameText : x.PA_MiddleName)
                                                                                                         + " " + x.PA_LastName) + ", ");

                if (AliasName.EndsWith(", "))
                    AliasName = AliasName.Substring(0, AliasName.Length - 2);

                if (OrganizationUserData.PersonAlias.Count() > 0 && !AliasName.IsNullOrEmpty())
                {
                    dvAlias.Visible = true;
                    lblAlias.Text = AliasName.HtmlEncode();
                }
            }
            //Applicants User Groups
            if (this.UserGroupDataList.IsNotNull() && UserGroupDataList.Count > 0)
            {
                String userGroup = String.Empty;
                this.UserGroupDataList.ForEach(x => userGroup += Convert.ToString(x.UG_Name) + ", ");

                if (userGroup.EndsWith(", "))
                    userGroup = userGroup.Substring(0, userGroup.Length - 2);
                if (!userGroup.IsNullOrEmpty())
                {
                    dvUserGroup.Visible = true;
                    lblUserGroups.Text = userGroup.HtmlEncode();
                }
            }
            if (this.CurrentCompliancePackageStatusCode.ToLower() == ApplicantPackageComplianceStatus.Not_Compliant.GetStringValue().ToLower())
            {
                imgPackageComplianceStatus.ImageUrl = AppConsts.PACKAGE_NON_COMPLIANCE_IMAGE_URL;
                lblOverComplianceStatus.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                imgPackageComplianceStatus.ImageUrl = AppConsts.PACKAGE_COMPLIANCE_IMAGE_URL;
                lblOverComplianceStatus.ForeColor = System.Drawing.Color.Green;
            }

            if (this.OrganizationUserData.PhotoName.IsNotNull())
                imgApplicantPhoto.ImageUrl = String.Format("/ComplianceOperations/UserControl/DocumentViewer.aspx?UserId={0}&DocumentType={1}", this.OrganizationUserData.OrganizationUserID, "ProfilePicture");
            else
            {
                //imgApplicantPhoto.AlternateText = "Profile picture not available";
            }

            lstCategories.DataSource = this.ApplicantComplianceCategoryDataList;
            lstCategories.DataTextField = "CategoryName";
            lstCategories.DataValueField = "CategoryId";
            lstCategories.DataBind();
            if (lstCategories.FindItemByValue(this.SelectedComplianceCategoryId_Global.ToString()) != null)
                lstCategories.SelectedValue = this.SelectedComplianceCategoryId_Global.ToString();
            else if (lstCategories.Items.Count > 0)
            {
                lstCategories.SelectedIndex = 0;
            }
            lstCategories_SelectedIndexChanged(null, null);
            this.SelectedComplianceCategoryId_Global = Convert.ToInt32(lstCategories.SelectedValue);



            btnNextApp.ImageUrl = (this.NextPackageSubscriptionID == 0) ? "~/Resources/Mod/Compliance/images/right-arrowd.png" :
                "~/Resources/Mod/Compliance/images/right-arrow.png";
            //btnNextApp.Enabled = (this.NextPackageSubscriptionID == 0) ? false : true;
            lnkBtnNextApp.Disabled = (this.NextPackageSubscriptionID == 0) ? true : false;

            ////btnNextApplicant.Enabled = (this.NextPackageSubscriptionID == 0) ? false : true;
            //btnNextApplicant.Image.EnableImageButton = true;
            //btnNextApplicant.Icon.PrimaryIconUrl = (this.NextPackageSubscriptionID == 0)
            //                                 ? "~/Resources/Mod/Compliance/images/right-arrowd.png"
            //                                 : "~/Resources/Mod/Compliance/images/right-arrow.png";

            btnPrevApp.ImageUrl = (this.PrevPackageSubscriptionID == 0) ? "~/Resources/Mod/Compliance/images/left-arrowd.png" :
                 "~/Resources/Mod/Compliance/images/left-arrow-blue.png";
            //btnPrevApp.Enabled = (this.PrevPackageSubscriptionID == 0) ? false : true;
            lnkBtnPrevApp.Disabled = (this.PrevPackageSubscriptionID == 0) ? true : false;

            //// btnPrevApplicant.Enabled = (this.PrevPackageSubscriptionID == 0) ? false : true;
            // btnPrevApplicant.Image.EnableImageButton = true;
            //btnPrevApplicant.Icon.PrimaryIconUrl = (this.PrevPackageSubscriptionID == 0)
            //                                  ? "~/Resources/Mod/Compliance/images/left-arrowd.png"
            //                                  : "~/Resources/Mod/Compliance/images/left-arrow.png";

            hdnApplicantId.Value = this.CurrentApplicantId_Global.ToString();
            hdnApplicantName.Value = this.OrganizationUserData.FirstName + " " + this.OrganizationUserData.LastName;
            imgRushOrder.Visible = this.IsRushOrder;

            //Setting Name initials
            if (!string.IsNullOrWhiteSpace(this.OrganizationUserData.FirstName))
            {
                lblNameInitials.Text = this.OrganizationUserData.FirstName.Substring(0, 1);

            }
            if (!string.IsNullOrWhiteSpace(this.OrganizationUserData.LastName))
            {

                lblNameInitials.Text = lblNameInitials.Text + this.OrganizationUserData.LastName.Substring(0, 1);
            }
        }

        protected string GetImageUrl(string status, string excStatus, Boolean isComplianceRequired, String rulesStatusID)
        {
            string url = "";
            if (!(String.IsNullOrEmpty(excStatus)) && excStatus == "AAAA" && status == ApplicantCategoryComplianceStatus.Incomplete.GetStringValue())
            {
                url = ResolveUrl("~/Resources/Mod/Compliance/icons/attn16.png");
            }
            else if (!(String.IsNullOrEmpty(excStatus)) && excStatus == "AAAD" && status == ApplicantCategoryComplianceStatus.Approved.GetStringValue())
            {
                url = ResolveUrl("~/Resources/Mod/Compliance/icons/yesx16.png");
            }
            else if (status.IsNullOrEmpty() || status == ApplicantCategoryComplianceStatus.Incomplete.GetStringValue())
            {
                url = ResolveUrl("~/Resources/Mod/Compliance/icons/no16.png");
            }
            else if (status == ApplicantCategoryComplianceStatus.Approved.GetStringValue())
            {
                url = ResolveUrl("~/Resources/Mod/Compliance/icons/yes16.png");
            }
            else if (status == ApplicantCategoryComplianceStatus.Approved_With_Exception.GetStringValue())
            {
                url = ResolveUrl("~/Resources/Mod/Compliance/icons/yesx16.png");
            }
            else if (status == ApplicantCategoryComplianceStatus.Pending_Review.GetStringValue())
            {
                url = ResolveUrl("~/Resources/Mod/Compliance/icons/attn16.png");
            }

            if (!isComplianceRequired)
            {
                if (!OptionalCategoryClientSetting)
                {
                    url = ResolveUrl("~/Resources/Mod/Compliance/icons/optional.png");
                }
                else if (rulesStatusID != AppConsts.STR_ONE)
                {
                    url = ResolveUrl("~/Resources/Mod/Compliance/icons/optional.png");
                }
                else if (rulesStatusID == AppConsts.STR_ONE)
                {
                    url = ResolveUrl("~/Resources/Mod/Compliance/icons/yes16.png");
                }
            }

            //if (!isComplianceRequired && status != ApplicantCategoryComplianceStatus.Approved.GetStringValue())
            //{
            //    url = ResolveUrl("~/Resources/Mod/Compliance/icons/optional.png");
            //}
            //else if (!isComplianceRequired && status == ApplicantCategoryComplianceStatus.Approved.GetStringValue())
            //{
            //    url = ResolveUrl("~/Resources/Mod/Compliance/icons/yes16.png");
            //}

            return url;
        }

        protected string GetStatus(string status, string excStatus, string categoryStatusName)
        {
            string catStatus = categoryStatusName;
            if (!(String.IsNullOrEmpty(excStatus)) && excStatus == "AAAA" && status == ApplicantCategoryComplianceStatus.Incomplete.GetStringValue())
            {
                catStatus = "Pending Review";
            }
            else if (status.IsNullOrEmpty()) //UAT-3611
            {
                catStatus = "Incomplete";
            }
            //[Below code commented by Sachin Singh to resolved the issue in tooltip of category status image]
            //else if (String.IsNullOrEmpty(excStatus))
            //{
            //    catStatus = "Incomplete";
            //}
            if (!excStatus.IsNullOrEmpty() && excStatus == "AAAD" && status == "APRD")
                return "Approved Override";
            else
                return catStatus;
        }
        #region UAT-3611
        protected string GetComplianceRequiredDateSet(bool isComplianceReq, String complianceStartDate, String complianceEndDate)
        {
            String complianceDateSet = String.Empty;
            String startDate = String.Empty;
            String endDate = String.Empty;
            String complianceReq = "(Compliance " + Convert.ToString(isComplianceReq == true ? "" : "not ") + "required: ";

            if (isComplianceReq)
            {
                if (!complianceStartDate.IsNullOrEmpty() && !complianceEndDate.IsNullOrEmpty())
                {
                    complianceReq = "(Compliance not required: ";
                    DateTime dtStart = Convert.ToDateTime(complianceEndDate).AddDays(1);
                    DateTime dtEnd = (Convert.ToDateTime(complianceStartDate)).AddDays(-1);

                    startDate = Convert.ToString(complianceStartDate) == String.Empty ? String.Empty : Convert.ToString(dtStart.Month + "/" + dtStart.Day);
                    endDate = Convert.ToString(complianceEndDate) == String.Empty ? String.Empty : Convert.ToString(dtEnd.Month + "/" + dtEnd.Day);
                }
            }
            else
            {
                startDate = Convert.ToString(complianceStartDate) == String.Empty ? String.Empty : Convert.ToString(Convert.ToDateTime(complianceStartDate).Month + "/" + Convert.ToDateTime(complianceStartDate).Day);
                endDate = Convert.ToString(complianceEndDate) == String.Empty ? String.Empty : Convert.ToString(Convert.ToDateTime(complianceEndDate).Month + "/" + Convert.ToDateTime(complianceEndDate).Day);
            }

            if (!startDate.IsNullOrEmpty() && !endDate.IsNullOrEmpty())
            {
                complianceDateSet = complianceReq + startDate + "-" + endDate + ")";
            }
            else
            {
                //complianceDateSet = complianceReq + ")";
                complianceDateSet = String.Empty;
            }

            return complianceDateSet;
        }
        #endregion



        #endregion

        #region "Public Methods"

        public void SetPageDataAndLayout(Boolean GetFreshData)
        {
            if (GetFreshData)
            {
                BindAttributes();
                ManageSubscriptionLinksNavigations();
            }

            if (WorkQueue.Equals(WorkQueueType.ComplianceSearch) || WorkQueue.Equals(WorkQueueType.DataItemSearch) || WorkQueue.Equals(WorkQueueType.AssigneeDataItemSearch))
            {
                lnkBtnPrevApp.Visible = lnkBtnNextApp.Visible = false;
                ////btnPrevApplicant.Visible = btnNextApplicant.Visible = false;
            }

        }

        private void ManageSubscriptionLinksNavigations()
        {
            //UAT-3293
            if (this.WorkQueue == WorkQueueType.ReconciliationQueue || this.WorkQueue == WorkQueueType.ReconciliationDetail)
            {
                lnkBtnPrevApp.Disabled = true;
                lnkBtnPrevApp.HRef = String.Empty;
                btnPrevApp.ImageUrl = "~/Resources/Mod/Compliance/images/left-arrowd.png";
                lnkBtnNextApp.Disabled = true;
                lnkBtnNextApp.HRef = String.Empty;
                btnNextApp.ImageUrl = "~/Resources/Mod/Compliance/images/right-arrowd.png";
            }
            if (this.PrevPackageSubscriptionID == AppConsts.MINUS_TWO)
            {
                this.SubPageIndex--;
                this.SelectedPackageSubscriptionID_Global = AppConsts.MINUS_TWO;
            }
            else
            {
                this.SelectedPackageSubscriptionID_Global = this.PrevPackageSubscriptionID;
                this.ItemDataId_Global = this.PrevAppCmpItemID;
            }

            //ComplianceOperationsVerifications.RedirectToVerificationDetailScreen(this.TenantId_Global,
            //        this.ItemDataId_Global,
            //        this.WorkQueue,
            //        this.PackageId,
            //        this.CategoryId,
            //        this.UserGroupId,
            //        this.IncludeIncompleteItems,
            //        this.SelectedPackageSubscriptionID_Global,
            //        this.SelectedComplianceCategoryId_Global,
            //        ChildControls.VerificationDetailsNew,
            //        this.viewType, this.ShowOnlyRushOrders, IsException);

            String prevQueryString = ComplianceOperationsVerifications.GetVerificationDetailScreenQueryString(this.TenantId_Global,
                 this.ItemDataId_Global,
                 this.WorkQueue,
                 this.PackageId,
                 this.CategoryId,
                 this.UserGroupId,
                 this.IncludeIncompleteItems,
                 this.SelectedPackageSubscriptionID_Global,
                 this.SelectedComplianceCategoryId_Global,
                 ChildControls.VerificationDetailsNew,
                 this.viewType,
                 this.ShowOnlyRushOrders, IsException, CurrentApplicantId_Global, VerificationDetailActionType.SubscriptionPreviousNext.GetStringValue(), IsEscalationRecords, this.SelectedArchiveStateCode,this.SelectedComplianceItemReconciliationDataID);

            if (lnkBtnPrevApp.Disabled)
                lnkBtnPrevApp.HRef = String.Empty;
            else
                lnkBtnPrevApp.HRef = String.Format("~/ComplianceOperations/Default.aspx?ucid={0}&args={1}", viewType, prevQueryString.ToString());

            hdnPreviousSubscriptionURLApplicant.Value = lnkBtnPrevApp.HRef;

            ////if (!btnPrevApplicant.Enabled)
            ////    btnPrevApplicant.Attributes.Add("Url", String.Empty);
            ////else
            ////    btnPrevApplicant.Attributes.Add("Url", String.Format("~/ComplianceOperations/Default.aspx?ucid={0}&args={1}", viewType, prevQueryString.ToString()));

            if (this.NextPackageSubscriptionID == AppConsts.MINUS_ONE)
            {
                this.SubPageIndex++;
                this.SelectedPackageSubscriptionID_Global = AppConsts.MINUS_ONE;
            }
            else
            {
                this.SelectedPackageSubscriptionID_Global = this.NextPackageSubscriptionID;
                this.ItemDataId_Global = this.NextAppCmpItemID;
            }

            //ComplianceOperationsVerifications.RedirectToVerificationDetailScreen(this.TenantId_Global,
            //                                this.ItemDataId_Global,
            //                                this.WorkQueue,
            //                                this.PackageId,
            //                                this.CategoryId,
            //                                this.UserGroupId,
            //                                this.IncludeIncompleteItems,
            //                                this.SelectedPackageSubscriptionID_Global,
            //                                this.SelectedComplianceCategoryId_Global,
            //                                ChildControls.VerificationDetailsNew,
            //                                this.viewType, this.ShowOnlyRushOrders, IsException);


            String nextQueryString = ComplianceOperationsVerifications.GetVerificationDetailScreenQueryString(this.TenantId_Global,
                  this.ItemDataId_Global,
                  this.WorkQueue,
                  this.PackageId,
                  this.CategoryId,
                  this.UserGroupId,
                  this.IncludeIncompleteItems,
                  this.SelectedPackageSubscriptionID_Global,
                  this.SelectedComplianceCategoryId_Global,
                  ChildControls.VerificationDetailsNew,
                  this.viewType,
                  this.ShowOnlyRushOrders, IsException, CurrentApplicantId_Global, VerificationDetailActionType.SubscriptionPreviousNext.GetStringValue(), IsEscalationRecords, this.SelectedArchiveStateCode, this.SelectedComplianceItemReconciliationDataID);

            if (lnkBtnNextApp.Disabled)
                lnkBtnNextApp.HRef = String.Empty;
            else
                lnkBtnNextApp.HRef = String.Format("~/ComplianceOperations/Default.aspx?ucid={0}&args={1}", viewType, nextQueryString.ToString());

            hdnNextSubscriptionURLApplicant.Value = lnkBtnNextApp.HRef;

            ////if (!btnNextApplicant.Enabled)
            ////    btnNextApplicant.Attributes.Add("Url", String.Empty);
            ////else
            ////    btnNextApplicant.Attributes.Add("Url", String.Format("~/ComplianceOperations/Default.aspx?ucid={0}&args={1}", viewType, nextQueryString.ToString()));
        }

        #endregion

        #endregion

        protected void btnCategorylnk_Click(object sender, EventArgs e)
        {
            RadButton btnCategorylnk = sender as Telerik.Web.UI.RadButton;
            String url = btnCategorylnk.Attributes["Url"];
            if (!url.IsNullOrEmpty())
            {
                Response.Redirect(url);
            }
        }

        /// <summary>
        /// NOT IN USE
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPrevApplicant_Click(object sender, EventArgs e)
        {
            ////String url = btnPrevApplicant.Attributes["Url"];
            ////if (!url.IsNullOrEmpty())
            ////{
            ////    Response.Redirect(url);
            ////}
        }

        /// <summary>
        /// NOT IN USE
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNextApplicant_Click(object sender, EventArgs e)
        {
            ////String url = btnNextApplicant.Attributes["Url"];
            ////if (!url.IsNullOrEmpty())
            ////{
            ////    Response.Redirect(url);
            ////}
        }

    }
}

