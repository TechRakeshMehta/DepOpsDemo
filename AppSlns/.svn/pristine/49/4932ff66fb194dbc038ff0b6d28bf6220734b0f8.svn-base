#region NameSpace
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using Entity.ClientEntity;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Serialization;
using Telerik.Web.UI;
#endregion
namespace CoreWeb.BkgOperations.Views
{
    public partial class BkgOrderSearchQueue : BaseUserControl, IBkgOrderSearchQueueView
    {
        #region Private Variables
        private BkgOrderSearchQueuePresenter _presenter = new BkgOrderSearchQueuePresenter();
        private String _viewType;
        List<Entity.ClientEntity.InstitutionOrderFlag> lstOrderFlag = new List<InstitutionOrderFlag>();
        private BkgOrderSearchContract _gridSearchContract = null;
        private Int32 _tenantid;
        private CustomPagingArgsContract _gridCustomPaging = null;
        protected String ImagePath = "~/images/small";
        protected String ImagePathOrderStatus = "~/images/medium";
        protected String path = "~/images/Status/";
        private DateTime _minCalenderDate = Convert.ToDateTime("01/01/1980");

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// returns the object of type IOrderQueueView.
        /// </summary>
        public IBkgOrderSearchQueueView CurrentViewContext
        {
            get { return this; }
        }
        public List<Entity.ClientEntity.InstitutionOrderFlag> InstitutionOrderFlagList
        {
            get
            {
                if (lstOrderFlag.IsNotNull() && lstOrderFlag.Count > AppConsts.NONE)
                {
                    return lstOrderFlag;
                }
                else
                {
                    return Presenter.GetInstitutionStatusColor();
                }
            }
            set
            {
                lstOrderFlag = value;
            }
        }

        ///// <summary>
        ///// Sets the drop down with order status.
        ///// </summary>
        public List<Entity.ClientEntity.lkpOrderStatu> lstPaymentStatus
        {
            set
            {
                if (value.IsNotNull())
                {
                    chkPaymentStatusType.DataSource = value.OrderBy(x => x.Name);
                    chkPaymentStatusType.DataBind();
                    chkPaymentStatusType.Items.Insert(0, new RadComboBoxItem("--Select--"));
                }
            }
        }

        ///// <summary>
        ///// Sets the drop down with payment type.
        ///// </summary>      
        public List<Entity.ClientEntity.lkpOrderStatusType> lstOrderStatusType
        {
            set
            {
                if (value.IsNotNull())
                {
                    chkOrderStatusTypes.DataSource = value.OrderBy(x => x.OrderStatusTypeID);
                    chkOrderStatusTypes.DataBind();
                    chkOrderStatusTypes.Items.Insert(0, new RadComboBoxItem("--Select--"));
                }
            }
        }

        /// <summary>
        /// UAT 1417
        /// </summary>
        public List<Entity.ClientEntity.UserGroup> lstUserGroup
        {
            set
            {
                if (value.IsNotNull())
                {
                    ddlUserGroup.DataSource = value;
                    ddlUserGroup.DataBind();
                }
            }
        }

        public Boolean DisplayOrderArchiveStatus
        {
            get;
            set;
        }

        public Int32 HierarchyNodeID
        {
            get;
            set;
        }
        public Boolean DisplayOrderClientStatus
        {
            get;
            set;
        }

        ///// <summary>
        ///// Sets the drop down with backround services
        ///// </summary> 
        public List<Entity.ClientEntity.BackgroundService> lstBackroundServices
        {
            set
            {
                if (value.IsNotNull())
                {
                    chkServices.DataSource = value.OrderBy(x => x.BSE_Name);//UAT sort dropdowns by Name;
                    chkServices.DataBind();
                    chkServices.Items.Insert(0, new RadComboBoxItem("--Select--"));
                }
            }
        }

        public List<Entity.ClientEntity.BkgSvcGroup> lstBkgSvcGroup
        {
            set
            {
                if (value.IsNotNull())
                {
                    rcServiceGroup.DataSource = value.OrderBy(x => x.BSG_Name);//UAT sort dropdowns by Name;
                    rcServiceGroup.DataBind();
                    rcServiceGroup.Items.Insert(0, new RadComboBoxItem("--Select--"));
                }
            }
        }

        public List<Entity.ClientEntity.BkgOrderClientStatu> lstOrderClientStatus
        {

            set
            {
                if (value.IsNotNull())
                {
                    rcbClientStatus.DataSource = value.OrderBy(x => x.BOCS_ID);
                    rcbClientStatus.DataBind();
                    rcbClientStatus.Items.Insert(0, new RadComboBoxItem("--Select--"));
                }
            }
        }

        public List<Entity.ClientEntity.BackroundOrderSearch> lstBackroundOrderSearch
        {
            get;
            set;
        }

        public List<BackroundOrderContract> lstBackroundOrder
        {
            get;
            set;
        }

        public List<BackroundServiceGroupContract> lstBackroundServiceGroup
        {
            get;
            set;
        }

        public BackroundOrderSearchContract lstBackroundOrderSearchContract
        {
            get;
            set;
        }

        public List<BackroundServicesContract> lstBackroundServicesContract
        {
            get;
            set;
        }


        public List<GranularPermission> GranularPermission
        {
            get
            {
                if (!ViewState["GranularPermission"].IsNull())
                {
                    return (List<GranularPermission>)(ViewState["GranularPermission"]);
                }

                return new List<GranularPermission>();
            }
            set
            {
                ViewState["GranularPermission"] = value;
            }
        }
        /// <summary>
        /// Sets or gets the Selected Tenant Id from the select tenant dropdown.
        /// </summary>
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
        /// Sets or gets the Tenant Id for the logged-in user.
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
        /// returns the Current Logged In User Id.
        /// </summary>
        public Int32 CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        /// <summary>
        /// Gets or sets the list of active tenants.
        /// </summary>
        public List<Entity.ClientEntity.Tenant> lstTenant
        {
            set
            {
                ddlTenantName.DataSource = value;
                ddlTenantName.DataBind();
                if (!IsAdminUser)
                {
                    ddlTenantName.FindItemByValue(TenantId.ToString()).Selected = true;
                }
            }
        }

        public List<Entity.ClientEntity.lkpServiceFormStatu> lstServiceFormStatus
        {
            set
            {
                if (value.IsNotNull())
                {
                    cmbFormStatus.DataSource = value.OrderBy(x => x.SFS_ID);
                    cmbFormStatus.DataBind();
                    cmbFormStatus.Items.Insert(0, new RadComboBoxItem("--Select--"));
                }
            }
        }

        /// <summary>
        /// Gets or sets the Error Message
        /// </summary>
        public String ErrorMessage
        {
            get;
            set;
        }

        //UAT-1732: Change Yes/No checkbox for Is flagged to radio buttons with "Flagged", "Not Flagged", and "all"
        //public Boolean? IsFlagged
        //{
        //    get
        //    {
        //        if (chkFlaggedToggle.Checked.IsNotNull() && chkFlaggedToggle.Checked != false)
        //        {
        //            return Convert.ToBoolean(chkFlaggedToggle.Checked);
        //        }
        //        return null;
        //    }
        //    set
        //    {
        //        if (value.IsNotNull())
        //        {
        //            chkFlaggedToggle.Checked = Convert.ToBoolean(value);
        //        }
        //    }
        //}
        //UAT-1732: Change Yes/No checkbox for Is flagged to radio buttons with "Flagged", "Not Flagged", and "all"
        String IBkgOrderSearchQueueView.SelectedFlagged
        {
            get
            {
                if (!rblFlagged.SelectedValue.IsNullOrEmpty() && divFlaged.Visible == true)
                {
                    //return rblFlagged.SelectedValue == AppConsts.STR_TWO ? String.Empty : rblFlagged.SelectedValue.ToString();
                    return rblFlagged.SelectedValue.ToString();
                }
                else
                {
                    return String.Empty;
                }
            }
            set
            {
                rblFlagged.SelectedValue = value;
            }
        }

        /// <summary>
        /// Indicates wheather Select Client dropdown will be visible or not.
        /// </summary>
        public Boolean IsAdminUser
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsAdminUser"]);
            }
            set
            {
                ViewState["IsAdminUser"] = value;
            }
        }

        public String FirstNameSearch
        {
            get
            {
                return txtFirstName.Text.Trim();
            }
        }

        public String LastNameSearch
        {
            get
            {
                return txtLastName.Text.Trim();
            }
        }

        public String OrderNumberSearch
        {
            get
            {
                if (!txtOrderNumber.Text.IsNullOrEmpty())
                {
                    return Convert.ToString(txtOrderNumber.Text);
                }
                return String.Empty;
            }
        }

        public Int32? OrderPaymentStatusID
        {
            get
            {
                if (!chkPaymentStatusType.SelectedValue.IsNullOrEmpty())
                {
                    return Convert.ToInt32(chkPaymentStatusType.SelectedValue);
                }
                return 0;
            }
            set
            {
                if (value.IsNotNull())
                {
                    chkPaymentStatusType.SelectedValue = value.ToString();
                }
            }

        }

        public Int32? InstitutionStatusColorID
        {
            get
            {
                if (!ddlIcon.SelectedValue.IsNullOrEmpty())
                {
                    return Convert.ToInt32(ddlIcon.SelectedValue);
                }
                return 0;
            }
            set
            {
                if (value.IsNotNull())
                {
                    ddlIcon.SelectedValue = value.ToString();
                }
            }

        }

        public Int32? OrderStatusTypeID
        {
            get
            {
                if (!chkOrderStatusTypes.SelectedValue.IsNullOrEmpty())
                {
                    return Convert.ToInt32(chkOrderStatusTypes.SelectedValue);
                }
                return 0;
            }
            set
            {
                if (value.IsNotNull())
                {
                    chkOrderStatusTypes.SelectedValue = value.ToString();
                }
            }
        }

        public Int32? ServiceGroupId
        {
            get
            {
                if (!rcServiceGroup.SelectedValue.IsNullOrEmpty())
                {
                    return Convert.ToInt32(rcServiceGroup.SelectedValue);
                }
                return 0;
            }
            set
            {
                if (value.IsNotNull())
                {
                    rcServiceGroup.SelectedValue = value.ToString();
                }
            }
        }

        public Int32? OrderClientStatusID
        {
            get
            {
                if (!rcbClientStatus.SelectedValue.IsNullOrEmpty())
                {
                    return Convert.ToInt32(rcbClientStatus.SelectedValue);
                }
                return 0;
            }
            set
            {
                if (value.IsNotNull())
                {
                    rcbClientStatus.SelectedValue = value.ToString();
                }
            }
        }

        public Int32? ServiceID
        {
            get
            {
                if (!chkServices.SelectedValue.IsNullOrEmpty())
                {
                    return Convert.ToInt32(chkServices.SelectedValue);
                }
                return 0;
            }
            set
            {
                if (value.IsNotNull())
                {
                    chkServices.SelectedValue = value.ToString();
                }
            }
        }

        public DateTime? OrderFromDate
        {
            get
            {
                if (orderDateType.SelectedValue == BkgOrderDateType.Created.GetStringValue())
                {
                    return dpOdrCrtFrm.SelectedDate;
                }
                return null;
            }

        }

        public DateTime? OrderToDate
        {
            get
            {
                if (orderDateType.SelectedValue == BkgOrderDateType.Created.GetStringValue())
                {
                    return dpOdrCrtTo.SelectedDate;
                }
                return null;
            }

        }

        public DateTime? PaidFromDate
        {
            get
            {
                if (orderDateType.SelectedValue == BkgOrderDateType.Paid.GetStringValue())
                {
                    return dpOdrCrtFrm.SelectedDate;
                }
                return null;
            }

        }

        public DateTime? PaidToDate
        {
            get
            {
                if (orderDateType.SelectedValue == BkgOrderDateType.Paid.GetStringValue())
                {
                    return dpOdrCrtTo.SelectedDate;
                }
                return null;
            }

        }

        public DateTime? OrderCompletedFromDate
        {
            get
            {
                if (orderDateType.SelectedValue == BkgOrderDateType.Completed.GetStringValue())
                {
                    return dpOdrCrtFrm.SelectedDate;
                }
                return null;
            }
        }

        public DateTime? OrderCompletedToDate
        {
            get
            {
                if (orderDateType.SelectedValue == BkgOrderDateType.Completed.GetStringValue())
                {
                    return dpOdrCrtTo.SelectedDate;
                }
                return null;
            }
        }

        public String SSN
        {
            //get
            //{
            //    //return txtSSN.Text.Trim();
            //    return txtSSN.TextWithPrompt;
            //}
            get;
            set;

        }

        public Boolean IsBkgOrderNoteEnabled
        {
            get
            {
                if (ViewState["IsBkgOrderNoteEnabled"] != null)
                    return Convert.ToBoolean(ViewState["IsBkgOrderNoteEnabled"]);
                else
                    return false;
            }
            set
            {
                ViewState["IsBkgOrderNoteEnabled"] = value;
            }
        }

        public DateTime? DOB
        {
            get
            {
                if (!dpDob.SelectedDate.IsNullOrEmpty())
                {
                    return dpDob.SelectedDate;
                }
                return null;
            }
            set
            {
                if (value.IsNotNull())
                {
                    dpDob.SelectedDate = value;
                }
            }

        }

        public Boolean? IsArchive
        {
            get
            {
                if (!rcbArchiveStatus.SelectedValue.IsNullOrEmpty() && rcbArchiveStatus.SelectedValue != (AppConsts.NONE).ToString())
                {
                    return Convert.ToBoolean(rcbArchiveStatus.SelectedValue);
                }
                return null;
            }
            set
            {
                if (value.IsNotNull())
                {
                    rcbArchiveStatus.Items.FindItemByValue(value.ToString().ToLower()).Selected = true;
                }
            }
        }

        public Int32? ServiceFormStatusID
        {
            get
            {
                if (!cmbFormStatus.SelectedValue.IsNullOrEmpty())
                {
                    return Convert.ToInt32(cmbFormStatus.SelectedValue);
                }
                return 0;
            }
            set
            {
                if (value.IsNotNull())
                {
                    cmbFormStatus.SelectedValue = value.ToString();
                }
            }
        }

        public BkgOrderSearchContract SetBkgOrderSearchContract
        {
            set
            {
                //if (value.IsNotNull())
                //    value.NodeLabel = lblinstituteHierarchy.Text;
                var serializer = new XmlSerializer(typeof(BkgOrderSearchContract));
                var sb = new StringBuilder();
                using (TextWriter writer = new StringWriter(sb))
                {
                    serializer.Serialize(writer, value);
                }
                Session[AppConsts.BKG_SEARCH_OBJECT_SESSION_KEY] = sb.ToString();
            }
        }

        public Boolean IsPaymentStatusChecked
        {
            get
            {
                return chkPaymentStatusToggle.Checked;
            }
        }

        public Boolean IsClientStatusChecked
        {
            get
            {
                return chkCategoryToggle.Checked;
            }
        }

        //public Boolean IsFlaggedChecked
        //{
        //    get
        //    {
        //        return chkFlaggedToggle.Checked;
        //    }
        //}

        public Boolean IsArchiveChecked
        {
            get
            {
                return chkArchiveToggle.Checked;
            }
        }
        //public Int32? TargetHierarchyNodeId
        //{
        //    get
        //    {
        //        if (!String.IsNullOrEmpty(hdnDepartmntPrgrmMppng.Value))
        //        {
        //            return Convert.ToInt32(hdnDepartmntPrgrmMppng.Value);
        //        }
        //        return null;
        //    }
        //}

        //UAT-1055
        public String TargetHierarchyNodeIds
        {
            get
            {
                if (!String.IsNullOrEmpty(hdnDepartmntPrgrmMppng.Value))
                {
                    return Convert.ToString(hdnDepartmntPrgrmMppng.Value);
                }
                return String.Empty;
            }
        }

        /// <summary>
        /// get object of shared class of search contract
        /// </summary>
        public BkgOrderSearchContract GetBkgOrderSearchContract
        {
            get
            {
                if (_gridSearchContract.IsNull())
                {
                    var serializer = new XmlSerializer(typeof(BkgOrderSearchContract));
                    TextReader reader = new StringReader(Convert.ToString(Session[AppConsts.BKG_SEARCH_OBJECT_SESSION_KEY]));

                    using (reader)
                    {
                        _gridSearchContract = (BkgOrderSearchContract)serializer.Deserialize(reader);
                    }
                }
                return _gridSearchContract;
            }
        }

        /// <summary>
        /// CurrentPageIndex</summary>
        /// <value>
        /// Gets or sets the value for CurrentPageIndex.</value>
        public Int32 CurrentPageIndex
        {
            get
            {
                return grdBkgOrderDetails.MasterTableView.CurrentPageIndex + 1;
            }
            set
            {
                //if (grdBkgOrderDetails.MasterTableView.CurrentPageIndex > 0)
                //{
                //    grdBkgOrderDetails.MasterTableView.CurrentPageIndex = value - 1;
                //}
                grdBkgOrderDetails.MasterTableView.CurrentPageIndex = value == 0 ? 0 : value - 1;
            }
        }

        ///// <summary>
        ///// PageSize</summary>
        ///// <value>
        ///// Gets the value for PageSize.</value>
        public Int32 PageSize
        {
            get
            {
                // Maximum 100 record allowed from DB. 
                //return grdBkgOrderDetails.PageSize > 100 ? 100 : grdBkgOrderDetails.PageSize;
                return grdBkgOrderDetails.PageSize;
            }
            set
            {
                grdBkgOrderDetails.PageSize = value;
            }
        }

        ///// <summary>
        ///// VirtualPageCount</summary>
        ///// <value>
        ///// Sets the value for VirtualPageCount.</value>
        public Int32 VirtualPageCount
        {
            get
            {
                if (ViewState["ItemCount"] != null)
                    return Convert.ToInt32(ViewState["ItemCount"]);
                else
                    return 0;
            }
            set
            {
                ViewState["ItemCount"] = value;
                grdBkgOrderDetails.VirtualItemCount = value;
                grdBkgOrderDetails.MasterTableView.VirtualItemCount = value;
            }
        }

        ///// <summary>
        ///// get object of shared class of custom paging
        ///// </summary>      
        public CustomPagingArgsContract GridCustomPaging
        {
            get
            {
                if (ViewState["_gridCustomPaging"] == null)
                {
                    ViewState["_gridCustomPaging"] = new CustomPagingArgsContract();
                }
                return (CustomPagingArgsContract)ViewState["_gridCustomPaging"];
            }
            set
            {
                ViewState["_gridCustomPaging"] = value;
                VirtualPageCount = value.VirtualPageCount;
                PageSize = value.PageSize;
                CurrentPageIndex = value.CurrentPageIndex;
            }
        }

        public Dictionary<Int32, String> AssignOrganisationUserIDs
        {
            get;
            set;
        }

        #region UAT-806 Creation of granular permissions for Client Admin users

        public String SSNPermissionCode
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
        #endregion

        #region UAT-1075 WB:Admin Granular permissions for color flag and Result PDF
        public Boolean IsBkgColorFlagDisable
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsBkgColorFlagDisable"]);
            }
            set
            {
                ViewState["IsBkgColorFlagDisable"] = value;
            }
        }
        //public Boolean IsBkgResultReportDisable
        //{
        //    get
        //    {
        //        return Convert.ToBoolean(ViewState["IsBkgResultReportDisable"]);
        //    }
        //    set
        //    {
        //        ViewState["IsBkgResultReportDisable"] = value;
        //    }
        //}

        List<String> IBkgOrderSearchQueueView.LstBkgOrderResultPermissions
        {
            get
            {
                if (ViewState["LstBkgOrderResultPermissions"].IsNotNull())
                {
                    return (List<String>)(ViewState["LstBkgOrderResultPermissions"]);
                }
                return new List<String>();
            }
            set
            {
                ViewState["LstBkgOrderResultPermissions"] = value;
            }
        }

        #endregion

        public Int32? UserGroupID
        {
            get
            {
                if (!ddlUserGroup.SelectedValue.IsNullOrEmpty())
                {
                    return Convert.ToInt32(ddlUserGroup.SelectedValue);
                }
                return null;
            }
            set
            {
                if (value.IsNotNull())
                {
                    ddlUserGroup.SelectedValue = value.ToString();
                }
            }
        }

        public string SelectedOrderIds
        {
            get;
            set;
        }

        #region UAT-1795
        List<Int32> IBkgOrderSearchQueueView.lstSeletedOrderIds { get; set; }
        List<BkgOrderSearchQueueContract> IBkgOrderSearchQueueView.DocumentListToExport { get; set; }
        #endregion

        #region UAT-3010:- Granular Permission for Client Admin Users to Archive.

        public String ArchivePermissionCode
        {
            get
            {
                return Convert.ToString(ViewState["ArchivePermissionCode"]);
            }
            set
            {
                ViewState["ArchivePermissionCode"] = value;
            }
        }

        #endregion

        #endregion

        #region Private Properties

        private BkgOrderSearchQueuePresenter Presenter
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

        List<lkpArchiveState> IBkgOrderSearchQueueView.lstArchiveState
        {
            set
            {
                rbSubscriptionState.DataSource = value.OrderBy(x => x.AS_Code);
                rbSubscriptionState.DataBind();
                rbSubscriptionState.SelectedValue = ArchiveState.Active.GetStringValue();
            }
        }

        String IBkgOrderSearchQueueView.SelectedArchiveStateCode
        {
            get
            {
                if (!rbSubscriptionState.SelectedValue.IsNullOrEmpty())
                {
                    return rbSubscriptionState.SelectedValue == ArchiveState.All.GetStringValue() ? String.Empty : rbSubscriptionState.SelectedValue.ToString();
                }
                else
                {
                    return String.Empty;
                }
            }
            set
            {
                rbSubscriptionState.SelectedValue = value;
            }
        }

        //UAT-1725: Add searchable "Custom Attribute" to compliance, portfolio, and background order searches
        String IBkgOrderSearchQueueView.DPM_IDs
        {
            get
            {
                return ucCustomAttributeLoaderSearch.DPM_ID.IsNullOrEmpty() ? String.Empty : ucCustomAttributeLoaderSearch.DPM_ID;
            }
            set
            {
                ucCustomAttributeLoaderSearch.DPM_ID = value;
            }
        }

        String IBkgOrderSearchQueueView.CustomFields
        {
            get;
            set;
        }

        String IBkgOrderSearchQueueView.NodeLabel
        {
            get
            {
                return ucCustomAttributeLoaderSearch.nodeLable.IsNullOrEmpty() ? String.Empty : ucCustomAttributeLoaderSearch.nodeLable;
            }
            set
            {
                ucCustomAttributeLoaderSearch.nodeLable = value;
            }
        }

        String IBkgOrderSearchQueueView.NodeIds
        {
            get;
            set;
        }


        #endregion


        #region UAT-1996: Setting to allow Client admins the ability to edit color flags
        public Boolean IsBkgColorFlagFullPermission
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsBkgColorFlagFullPermission"]);
            }
            set
            {
                ViewState["IsBkgColorFlagFullPermission"] = value;
            }
        }
        #endregion

        //UAT-2178: Color flag column should only show when color flag is enabled for a tenant.
        Boolean IBkgOrderSearchQueueView.IsInstitutionHasOrderFlag
        {
            get
            {
                if (!ViewState["IsInstitutionHasOrderFlag"].IsNullOrEmpty())
                    return Convert.ToBoolean(ViewState["IsInstitutionHasOrderFlag"]);
                return false;
            }
            set
            {
                ViewState["IsInstitutionHasOrderFlag"] = value;
            }
        }

        #endregion

        #region Events

        #region Page Events

        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                base.Title = "Background  Order Queue";
                base.SetPageTitle("Background Order Queue");
                lblOrderQueue.Text = base.Title;
                fsucOrderCmdBar.SubmitButton.CausesValidation = false;
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
                CurrentViewContext.SSN = txtSSN.TextWithPrompt;
                if (!this.IsPostBack)
                {
                    fsucCmdExport.Visible = false;
                    grdBkgOrderDetails.Visible = false;
                    chkSelectAllResults.Visible = false;
                    Presenter.OnViewInitialized();
                    if (IsAdminUser)
                    {
                        SelectedTenantId = 0;
                        lblStatusColor.Text = "Must select institution to enable color search";
                        #region Show controls
                        ddlTenantName.Enabled = true;
                        lblStatusColor.Visible = true;
                        // ShowHideDiv(divAdminServices, true);
                        // ShowHideDiv(divServices, true);
                        //chkServices.Visible = true;
                        #endregion
                        grdBkgOrderDetails.Columns.FindByUniqueName("CanViewStatus").Display = false;
                        //UAT-1069: WB: add order date to the background order search grid
                        grdBkgOrderDetails.Columns.FindByUniqueName("CanViewOrderCreatedDate").Display = true;
                        hdnIsAdmin.Value = "true";
                    }
                    else
                    {
                        Presenter.GetClientAdminGranularPermission(); //UAT-4522
                        SelectedTenantId = TenantId;

                        #region Bind Controls
                        BindDropDownControl();
                        BindInstitutionColorFlag();
                        #endregion

                        #region Show hide columns
                        ddlTenantName.Enabled = false;
                        ShowGridColumns();
                        HideGridColumns();
                        #endregion
                        ShowHideDiv(divInstitutionHierarchy, true);
                        //UAT 1740:Move 604 notification from the time of login to when an admin attempts for view an employment result report.
                        hdnIsEdsAccepted.Value = Convert.ToString(Presenter.IsEDFormPreviouslyAccepted()).ToLower();
                        hdnEmployementDiscTypeCode.Value = DislkpDocumentType.EMPLOYMENT_DISCLOSURE_FORM.GetStringValue();
                        hdnOrgUsrId.Value = Convert.ToString(CurrentViewContext.CurrentLoggedInUserId);
                        grdBkgOrderDetails.Visible = false;
                    }
                    ApplySSNMask();
                    Dictionary<String, String> args = new Dictionary<String, String>();
                    if (!Request.QueryString["args"].IsNull())
                    {
                        args.ToDecryptedQueryString(Request.QueryString["args"]);
                        SetPageControl(args);
                    }
                    else
                    {
                        Session[AppConsts.BKG_SEARCH_OBJECT_SESSION_KEY] = null;
                        fsucOrderCmdBar.ClearButton.Style.Add("display", "none");
                    }

                    hdnOrderID.Value = null;

                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ResetSelectedUsers();", true);
                    //fsucOrderCmdBar.ClearButton.Style.Add("display", "none");
                }
                //UAT-1795 : Add D&A download button on Background Order Queue search.
                ifrExportDocument.Src = String.Empty;

                if (divTenant.Visible && !ddlTenantName.SelectedValue.IsNullOrEmpty())
                {
                    SelectedTenantId = Convert.ToInt32(ddlTenantName.SelectedValue);

                }
                if (!IsAdminUser && (SelectedTenantId.IsNull() || SelectedTenantId == AppConsts.NONE))
                {
                    SelectedTenantId = TenantId;
                    Presenter.GetInstitutionStatusColor();
                }
                DisplayOrderArchiveStatus = true;
                DisplayOrderClientStatus = true;
                ApplyActionLevelPermission(ActionCollection, "Background Order Queue");
                Presenter.OnViewLoaded();
                if (SelectedTenantId > AppConsts.NONE)
                {
                    hfTenantId.Value = SelectedTenantId.ToString();
                }
                //lblinstituteHierarchy.Text = hdnHierarchyLabel.Value;
                ucCustomAttributeLoaderSearch.TenantId = SelectedTenantId;
                ucCustomAttributeLoaderSearch.ScreenType = "BackgroundScreen";
                (fsucOrderCmdBar as CoreWeb.Shell.Views.CommandBar).SaveButton.ToolTip = "Click to search orders per the criteria entered above";
                (fsucOrderCmdBar as CoreWeb.Shell.Views.CommandBar).SubmitButton.ToolTip = "Click to remove all values entered in the search criteria above";
                (fsucOrderCmdBar as CoreWeb.Shell.Views.CommandBar).CancelButton.ToolTip = "Click to cancel. Any data entered will not be saved";
                HideShowControlsForGranularPermission();//UAt-806
                Presenter.GetBkgOrderNoteSetting();
              

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

        #region DropDown Events

        protected void ddlTenantName_ItemSelected(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (!ddlTenantName.SelectedValue.IsNullOrEmpty() && Convert.ToInt32(ddlTenantName.SelectedValue) != AppConsts.NONE)
            {
                SelectedTenantId = Convert.ToInt32(ddlTenantName.SelectedValue);
                BindDropDownControl();
                BindInstitutionColorFlag();
                Presenter.GetArchiveStateList();
                rbSubscriptionState.Visible = true;
                //fsucOrderCmdBar.ExtraButton.Enabled = true;
                ShowHideControl("Archivemun", "btnArchiveOrders", true); //UAT-4085
                ShowHideControl("Archivemun", "btnUnArchiveOrders", false); //UAT-4085
                //UAT-1725
                ucCustomAttributeLoaderSearch.Reset(SelectedTenantId);
            }
            else
            {
                ddlIcon.Visible = false;
                rbSubscriptionState.Visible = false;
                lblStatusColor.Visible = true;
                lblStatusColor.Text = "Must select Institution to enable color search.";
                ResetPageControls();
                hfTenantId.Value = String.Empty;
            }
            if (ddlTenantName.SelectedIndex > 0 && grdBkgOrderDetails.Items.Count > 0)
            {
                fsucOrderCmdBar.ClearButton.Style.Clear();
            }
            else
            {
                fsucOrderCmdBar.ClearButton.Style.Add("display", "none");
            }
        }

        protected void ddlTenantName_DataBound(object sender, EventArgs e)
        {
            ddlTenantName.Items.Insert(0, new RadComboBoxItem("--Select--"));
        }

        /// <summary>
        /// UserGroup Dropdown DataBound event. UAT 1417
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlUserGroup_DataBound(object sender, EventArgs e)
        {
            ddlUserGroup.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--Select--"));
        }

        protected void ddlIcon_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            grdBkgOrderDetails.Rebind();
            //UAT-1955 Set the focus on next control
            rcServiceGroup.Focus();
        }

        /// <summary>
        /// UAT-1955 Set the focus on next control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rblFlagged_SelectedIndexChanged(object sender, EventArgs e)
        {
            rblFlagged.Focus();
        }

        /// <summary>
        /// UAT-1955 Set the focus on next control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rcServiceGroup_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            rcServiceGroup.Focus();
        }

        #endregion

        #region Button Events

        /// <summary>
        /// Displays the records in the order queue based on the search criteria.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarSearch_Click(object sender, EventArgs e)
        {
            grdBkgOrderDetails.Visible = true;
            fsucCmdExport.Visible = true;
            chkSelectAllResults.Visible = true;
            if (!IsAdminUser)
            {
                grdBkgOrderDetails.Columns.FindByUniqueName("CanViewOrderClientStatus").Display = (chkCategoryToggle.Checked && DisplayOrderClientStatus);
                grdBkgOrderDetails.Columns.FindByUniqueName("CanViewPaymentStatus").Display = chkPaymentStatusToggle.Checked;
                grdBkgOrderDetails.Columns.FindByUniqueName("CanViewOrderArchiveStatus").Display = (chkArchiveToggle.Checked && DisplayOrderArchiveStatus);

                //UAT-1732: Change Yes/No checkbox for Is flagged to radio buttons with "Flagged", "Not Flagged", and "all"
                //grdBkgOrderDetails.Columns.FindByUniqueName("CanViewOrderFlag").Display = chkFlaggedToggle.Checked;
                if (!CurrentViewContext.SelectedFlagged.IsNullOrEmpty())
                {
                    if (CurrentViewContext.SelectedFlagged == AppConsts.ZERO)
                        grdBkgOrderDetails.Columns.FindByUniqueName("CanViewOrderFlag").Display = false;
                    else
                        grdBkgOrderDetails.Columns.FindByUniqueName("CanViewOrderFlag").Display = true;
                }
            }
            hdnOrderID.Value = null;
            grdBkgOrderDetails.Visible = true;
            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ResetSelectedUsers();", true);
            ResetGridFilters();
            if (grdBkgOrderDetails.Items.Count <= 0)
            {
                fsucOrderCmdBar.ClearButton.Style.Add("display", "none");
            }
            else
            {
                fsucOrderCmdBar.ClearButton.Style.Clear();
            }

            chkSelectAllResults.Checked = false;
            grdBkgOrderDetails.Focus();
        }

        /// <summary>
        /// Resets all the search controls and displays the records in the order queue with deafult checkbox selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarReset_Click(object sender, EventArgs e)
        {
            ResetPageControls();
            grdBkgOrderDetails.Visible = false;
        }

        /// <summary>
        /// Redirect to Home page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void CmdBarCancel_Click(object sender, EventArgs e)
        {
            try
            {
                SetBkgOrderSearchContract = null;
                Session[AppConsts.BKG_SEARCH_OBJECT_SESSION_KEY] = null;
                Response.Redirect(String.Format(AppConsts.DASHBOARD_PAGE_NAME), false);
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

        #region UAT-1683
        /// <summary>
        /// TO CHENGE THE ARCHIVE STATUS OF SELECTED ORDERIDS TO ARCHIVED
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucOrderCmdBar_ExtraClick(object sender, EventArgs e)
        {
            try
            {
                if (hdnOrderID.Value.IsNullOrEmpty())
                {
                    base.ShowErrorInfoMessage("Please select user(s) for archiving.");
                }
                else
                {
                    if (!Session["OrgUsersToList"].IsNullOrEmpty())
                    {
                        Session.Remove("OrgUsersToList");
                    }
                    String[] checkedOrderIDs = null;
                    List<Int32> lstSelectedBkgOrderIds = new List<Int32>();
                    if (hdnOrderID.Value != null)
                    {
                        checkedOrderIDs = hdnOrderID.Value.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    }
                    foreach (var item in checkedOrderIDs)
                    {
                        Int32 orderid;
                        orderid = Convert.ToInt32(item);
                        lstSelectedBkgOrderIds.Add(orderid);
                    }
                    String result = Presenter.ArchiveBkgOrder(lstSelectedBkgOrderIds);
                    if (result == "true")
                    {
                        base.ShowSuccessMessage("Selected Background Order(s) archived successfully.");
                        grdBkgOrderDetails.Rebind();
                    }
                    else if (result == "The selected user(s) does not have any active Background Order(s).")
                    {
                        base.ShowInfoMessage(result);
                    }
                    else
                    {
                        base.ShowErrorMessage("Selected Background Order(s) are not archived sucessfully. Please try again.");
                    }
                    hdnOrderID.Value = null;
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


        #region UAT-4085
        /// <summary>
        /// TO CHENGE THE ARCHIVE STATUS OF SELECTED ORDERIDS TO UNARCHIVED
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucOrderCmdBar_UnArchiveClick(object sender, EventArgs e)
        {
            try
            {
                if (hdnOrderID.Value.IsNullOrEmpty())
                {
                    base.ShowErrorInfoMessage("Please select Order(s) for unarchiving.");
                }
                else
                {
                    if (!Session["OrgUsersToList"].IsNullOrEmpty())
                    {
                        Session.Remove("OrgUsersToList");
                    }
                    String[] checkedOrderIDs = null;
                    List<Int32> lstSelectedBkgOrderIds = new List<Int32>();
                    if (hdnOrderID.Value != null)
                    {
                        checkedOrderIDs = hdnOrderID.Value.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    }
                    foreach (var item in checkedOrderIDs)
                    {
                        Int32 orderid;
                        orderid = Convert.ToInt32(item);
                        lstSelectedBkgOrderIds.Add(orderid);
                    }
                    String result = Presenter.UnArchiveBkgOrder(lstSelectedBkgOrderIds);
                    if (result == "true")
                    {
                        base.ShowSuccessMessage("Selected Background Order(s) unarchived successfully.");
                        grdBkgOrderDetails.Rebind();
                    }
                    else if (result == "The selected user(s) does not have any archived Background Order(s).")
                    {
                        base.ShowInfoMessage(result);
                    }
                    else
                    {
                        base.ShowErrorMessage("Selected Background Order(s) are not unarchived sucessfully. Please try again.");
                    }
                    hdnOrderID.Value = null;
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

        #region UAT-774
        protected void btnSendMessage_Click(object sender, EventArgs e)
        {
            try
            {
                if (hdnOrderID.Value.IsNullOrEmpty())
                {
                    base.ShowErrorInfoMessage("Please select user(s) to send message.");
                }
                else
                {
                    if (!Session["OrgUsersToList"].IsNullOrEmpty())
                    {
                        Session.Remove("OrgUsersToList");
                    }
                    String[] checkedOrderIDs = null;
                    if (hdnOrderID.Value != null)
                    {
                        checkedOrderIDs = hdnOrderID.Value.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    }
                    Presenter.GetOrganisationUserByOrder(checkedOrderIDs);
                    Session["OrgUsersToList"] = CurrentViewContext.AssignOrganisationUserIDs;
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "OpenPopup();", true);
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

        protected void btnAddNotes_Click(object sender, EventArgs e)
        {
            RadButton btn = sender as RadButton;
            GridDataItem parentItem = btn.NamingContainer as GridDataItem;
            Int32 orderID = Convert.ToInt32(((HiddenField)parentItem.FindControl("hdnOrderId")).Value);
            hfOrderID.Value = orderID.ToString();
            hfTenantId.Value = SelectedTenantId.ToString();
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "key", "openAddNotesPopUp(" + btn.ClientID + ");", true);
        }
        #endregion

        #region Grid Related Events

        /// <summary>
        /// Retrieves a list of Applicant Compliance Item Data.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        /// <remarks></remarks>
        protected void grdBkgOrderDetails_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                lstBackroundOrder = new List<BackroundOrderContract>();
                GridCustomPaging.CurrentPageIndex = CurrentPageIndex;
                GridCustomPaging.PageSize = PageSize;

                //UAT-1725
                if (CurrentViewContext.CustomFields.IsNullOrEmpty())
                {
                    CurrentViewContext.CustomFields = ucCustomAttributeLoaderSearch.GetCustomDataXML();
                }
                CurrentViewContext.NodeIds = CurrentViewContext.NodeIds.IsNullOrEmpty() ? ucCustomAttributeLoaderSearch.NodeIds : CurrentViewContext.NodeIds;
                Presenter.GetBkgOrderQueueData();
                grdBkgOrderDetails.DataSource = lstBackroundOrder;
                DisplayMessageSentStatus();

                //UAT-1732: Change Yes/No checkbox for Is flagged to radio buttons with "Flagged", "Not Flagged", and "all"
                if (!IsAdminUser)
                {
                    if (!CurrentViewContext.SelectedFlagged.IsNullOrEmpty())
                    {
                        if (CurrentViewContext.SelectedFlagged == AppConsts.ZERO)
                            grdBkgOrderDetails.Columns.FindByUniqueName("CanViewOrderFlag").Display = false;
                        else
                            grdBkgOrderDetails.Columns.FindByUniqueName("CanViewOrderFlag").Display = true;
                    }
                }

                if (rbSubscriptionState.SelectedValue == ArchiveState.Active.GetStringValue())
                {
                    ShowHideControl("Archivemun", "btnArchiveOrders", true);
                    ShowHideControl("Archivemun", "btnUnArchiveOrders", false);
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

        /// <summary>
        /// Redirect the user to the detail page.
        /// Sets the filetrs whn filtering is applied.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdBkgOrderDetails_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                #region DetailScreenNavigation

                if (e.CommandName.Equals("ViewOrderDetail"))
                {
                    Int32 selectedTenantId = 0;
                    if (!ddlTenantName.SelectedValue.IsNullOrEmpty())
                    {
                        selectedTenantId = SelectedTenantId;
                    }
                    else
                    {
                        selectedTenantId = TenantId;
                    }
                    Dictionary<String, String> queryString = new Dictionary<String, String>();
                    String orderId = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrderId"].ToString();
                    String orderNumber = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrderNumber"].ToString();
                    String url;
                    if (IsAdminUser)
                    {
                        queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "SelectedTenantId", Convert.ToString(selectedTenantId) },                                                      
                                                                    { "OrderId", orderId},
                                                                    { AppConsts.ORDER_NUMBER, orderNumber},
                                                                 };
                        url = String.Format("~/BkgOperations/Pages/BkgOrderDetailPage.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());

                    }
                    else
                    {
                        queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "SelectedTenantId", Convert.ToString(selectedTenantId) },                                                      
                                                                    { "OrderId", orderId}, 
                                                                    {"pageName", "Order Summary"},
                                                                     { AppConsts.ORDER_NUMBER, orderNumber},
                                                                 };
                        url = String.Format("~/BkgOperations/Pages/ClientAdminBkgOrderDetailPage.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                    }
                    Response.Redirect(url, true);
                }

                #endregion

                #region Export functionality
                // Implemented the export functionlaity for both admin and client admin and show and hide the columns accordingly
                if (e.CommandName.IsNullOrEmpty())
                {
                    //Int32 _hierarchyNodeID = e.Item.IsNullOrEmpty() && e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["HierarchyNodeID"].IsNullOrEmpty() ? 0 : Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["HierarchyNodeID"]); //commented for UAT 4767
                    Int32 _hierarchyNodeID = (e.Item.IsNullOrEmpty() && e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["HierarchyNodeID"].IsNullOrEmpty()) || (!e.Item.IsNullOrEmpty() && e.Item.ItemIndex==-1) ? 0 : Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["HierarchyNodeID"]);

                    if (e.Item is GridCommandItem)
                    {

                        WclComboBox cmbExportFormat = e.Item.FindControl("cmbExportFormat") as WclComboBox;
                        //UAT-1051 Complio: Full social security number should not be displayed on grid exports. Added Duplicate Column with masked SSN
                        // and displayed the masked column on Export instead of actual column.
                        if (IsExportCommand(cmbExportFormat))
                        {
                            grdBkgOrderDetails.MasterTableView.GetColumn("_SSN").Display = true;
                        }

                        if (IsAdminUser)
                        {
                            grdBkgOrderDetails.MasterTableView.GetColumn("ClearStarStatusTemp").Display = true;
                            //UAT-1069: WB: add order date to the background order search grid
                            //grdBkgOrderDetails.MasterTableView.GetColumn("CanViewOrderCreatedDate").Display = false;
                            //grdBkgOrderDetails.MasterTableView.GetColumn("CanViewOrderCreatedDate").Display = true;
                            grdBkgOrderDetails.MasterTableView.GetColumn("CanViewOrderCompletedDate").Display = false;
                            grdBkgOrderDetails.MasterTableView.GetColumn("CanViewDOB").Display = false;
                            grdBkgOrderDetails.MasterTableView.GetColumn("CanViewServiceGroups").Display = false;
                            grdBkgOrderDetails.MasterTableView.GetColumn("CanViewOrderClientStatus").Display = false;
                            grdBkgOrderDetails.MasterTableView.GetColumn("CanViewOrderFlag").Display = false;

                            //UAT-2110, Add Package Name(s) to Background Order Search for client admins.
                            //grdBkgOrderDetails.Columns.FindByUniqueName("BkgPackageNames").Display = false;

                            foreach (GridDataItem item in grdBkgOrderDetails.MasterTableView.Items)
                            {
                                item["OrderId"].Text = (item.FindControl("lbGridOrderId") as LinkButton).Text;
                                item["CanViewOrderStatus"].Text = (item.FindControl("lblGridOrderStatus") as Label).Text;
                                item["CanViewClearStarStatus"].Text = (item.FindControl("lbVendorStatus") as LinkButton).Text;
                                item["CanViewOrderCompletedDate"].Text = (item.FindControl("lblOrderCompletedDate") as Label).Text;
                                // item["CanViewOrderCreatedDate"].Text = (item.FindControl("lblOrderCreateDate") as Label).Text;
                            }
                        }
                        if (!IsAdminUser)
                        {
                            grdBkgOrderDetails.MasterTableView.GetColumn("CanViewServiceGroups").Display = true;
                            grdBkgOrderDetails.MasterTableView.GetColumn("CanViewClearStarStatus").Display = false;
                            //grdBkgOrderDetails.MasterTableView.GetColumn("CanViewOrderCreatedDate").Display = true;
                            grdBkgOrderDetails.MasterTableView.GetColumn("CanViewOrderCompletedDate").Display = true;

                            //UAT-2110, Add Package Name(s) to Background Order Search for client admins.
                            //grdBkgOrderDetails.Columns.FindByUniqueName("BkgPackageNames").Display = true;

                            //UAT-806 Creation of granular permissions for Client Admin users
                            if (CurrentViewContext.IsDOBDisable)
                                grdBkgOrderDetails.MasterTableView.GetColumn("CanViewDOB").Display = false;
                            else
                                grdBkgOrderDetails.MasterTableView.GetColumn("CanViewDOB").Display = true;

                         //   if (CurrentViewContext.LstBkgOrderResultPermissions.IsNotNull() &&
                         //(CurrentViewContext.LstBkgOrderResultPermissions.Contains(EnumSystemPermissionCode.Order_Result_Document.GetStringValue())
                         //   || CurrentViewContext.LstBkgOrderResultPermissions.Contains(EnumSystemPermissionCode.Service_Group_Result_Document.GetStringValue())
                         //   || CurrentViewContext.LstBkgOrderResultPermissions.Count() == AppConsts.NONE))
                               
                            if (CurrentViewContext.GranularPermission.IsNullOrEmpty() ||
                                CheckPermission(_hierarchyNodeID, EnumSystemPermissionCode.Order_Result_Document.GetStringValue()) ||
                                CheckPermission(_hierarchyNodeID, EnumSystemPermissionCode.Service_Group_Result_Document.GetStringValue()))

                            {
                                grdBkgOrderDetails.MasterTableView.GetColumn("CanViewOrderFlag").Display = true;
                            }

                            foreach (GridDataItem item in grdBkgOrderDetails.MasterTableView.Items)
                            {
                                item["OrderId"].Text = (item.FindControl("lbGridOrderId") as LinkButton).Text;
                                item["CanViewOrderStatus"].Text = (item.FindControl("lblGridOrderStatus") as Label).Text;
                                //item["CanViewOrderCreatedDate"].Text = (item.FindControl("lblOrderCreateDate") as Label).Text;
                                item["CanViewOrderCompletedDate"].Text = (item.FindControl("lblOrderCompletedDate") as Label).Text;
                                item["CanViewOrderClientStatus"].Text = (item.FindControl("lblOrderClientStatus") as Label).Text;
                            }
                        }

                        if (IsExportCommand(cmbExportFormat))
                        {
                            grdBkgOrderDetails.MasterTableView.GetColumn("OrderIdTemp").Display = true;
                            grdBkgOrderDetails.MasterTableView.GetColumn("BkgOrderStatusTemp").Display = true;
                            grdBkgOrderDetails.MasterTableView.GetColumn("CustomAttributesTemp").Display = true;
                            if (IsBkgOrderNoteEnabled)
                            {
                                //_hierarchyNodeID
                                //  if ((!CurrentViewContext.LstBkgOrderResultPermissions.IsNullOrEmpty() &&
                                //(CurrentViewContext.LstBkgOrderResultPermissions.Contains(EnumSystemPermissionCode.MANAGE_BKG_ORDER_NOTES.GetStringValue()))) || IsAdminUser)

                                if ((e.Item.ItemIndex == -1 && grdBkgOrderDetails.MasterTableView.GetColumn("OrderNote").Display == true))
                                {
                                    grdBkgOrderDetails.MasterTableView.GetColumn("_OrderNote").Display = true;
                                    grdBkgOrderDetails.MasterTableView.GetColumn("OrderNote").Display = false;
                                }

                                   if (CheckPermission(_hierarchyNodeID,EnumSystemPermissionCode.MANAGE_BKG_ORDER_NOTES.GetStringValue()) || IsAdminUser) //UAT-4522
                                {
                                    grdBkgOrderDetails.MasterTableView.GetColumn("_OrderNote").Display = true;
                                    grdBkgOrderDetails.MasterTableView.GetColumn("OrderNote").Display = false;
                                }
                            }
                        }
                        else
                        {
                            grdBkgOrderDetails.MasterTableView.GetColumn("OrderIdTemp").Display = false;
                            grdBkgOrderDetails.MasterTableView.GetColumn("BkgOrderStatusTemp").Display = false;
                            grdBkgOrderDetails.MasterTableView.GetColumn("CustomAttributesTemp").Display = false;
                            grdBkgOrderDetails.MasterTableView.GetColumn("_OrderNote").Display = false;
                            if (IsBkgOrderNoteEnabled)
                            {
                                //if (!CurrentViewContext.GranularPermission.IsNullOrEmpty() &&
                                //(CurrentViewContext.GranularPermission.Select(x => x.PermissionCode).Contains(EnumSystemPermissionCode.MANAGE_BKG_ORDER_NOTES.GetStringValue())))
                                if (CheckPermission(_hierarchyNodeID, EnumSystemPermissionCode.MANAGE_BKG_ORDER_NOTES.GetStringValue()))
                                {
                                    grdBkgOrderDetails.MasterTableView.GetColumn("OrderNote").Display = true;
                                }
                            }
                        }
                    }
                }
                if (e.CommandName == "Cancel")
                {
                    //Int32 _hierarchyNodeID = e.Item.IsNullOrEmpty() && e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["HierarchyNodeID"].IsNullOrEmpty() ? 0 : Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["HierarchyNodeID"]); //commented for UAT 4767
                    Int32 _hierarchyNodeID = (e.Item.IsNullOrEmpty() && e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["HierarchyNodeID"].IsNullOrEmpty()) || (!e.Item.IsNullOrEmpty() && e.Item.ItemIndex == -1) ? 0 : Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["HierarchyNodeID"]);

                    grdBkgOrderDetails.MasterTableView.GetColumn("OrderIdTemp").Display = false;
                    grdBkgOrderDetails.MasterTableView.GetColumn("OrderId").Display = true;
                    grdBkgOrderDetails.MasterTableView.GetColumn("BkgOrderStatusTemp").Display = false;
                    grdBkgOrderDetails.MasterTableView.GetColumn("CanViewOrderStatus").Display = true;
                    grdBkgOrderDetails.MasterTableView.GetColumn("CustomAttributesTemp").Display = false;
                    //UAT-1051 Complio: Full social security number should not be displayed on grid exports. Added Duplicate Column with masked SSN
                    // and displayed the masked column on Export instead of actual column.
                    grdBkgOrderDetails.MasterTableView.GetColumn("_SSN").Display = false;
                    if (IsBkgOrderNoteEnabled)
                    {
                        //if ((!CurrentViewContext.GranularPermission.IsNullOrEmpty() &&
                        //(CurrentViewContext.GranularPermission.Select(x => x.PermissionCode).Contains(EnumSystemPermissionCode.MANAGE_BKG_ORDER_NOTES.GetStringValue()))) || IsAdminUser)
                        if ((e.Item.ItemIndex == -1 && grdBkgOrderDetails.MasterTableView.GetColumn("_OrderNote").Display == true))
                        {
                            grdBkgOrderDetails.MasterTableView.GetColumn("_OrderNote").Display = false;
                            grdBkgOrderDetails.MasterTableView.GetColumn("OrderNote").Display = true;
                        }
                        if (CheckPermission(_hierarchyNodeID, EnumSystemPermissionCode.MANAGE_BKG_ORDER_NOTES.GetStringValue()) || IsAdminUser)
                        {
                            grdBkgOrderDetails.MasterTableView.GetColumn("_OrderNote").Display = false;
                            grdBkgOrderDetails.MasterTableView.GetColumn("OrderNote").Display = true;
                        }
                    }
                    grdBkgOrderDetails.MasterTableView.GetColumn("_OrderNote").Display = false;
                    if (!IsAdminUser)
                    {
                        grdBkgOrderDetails.MasterTableView.GetColumn("CanViewStatus").Display = true;
                        grdBkgOrderDetails.MasterTableView.GetColumn("CanViewServiceGroups").Display = true;
                        //UAT-2110, Add Package Name(s) to Background Order Search for client admins.
                        //grdBkgOrderDetails.Columns.FindByUniqueName("BkgPackageNames").Display = true;
                    }
                    if (IsAdminUser)
                    {
                        grdBkgOrderDetails.MasterTableView.GetColumn("ClearStarStatusTemp").Display = false;
                        grdBkgOrderDetails.MasterTableView.GetColumn("CanViewClearStarStatus").Display = true;
                    }
                }
                if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToPdfCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToWordCommandName
                    || e.CommandName == Telerik.Web.UI.RadGrid.ExportToExcelCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToCsvCommandName)
                {
                    base.ConfigureExport(grdBkgOrderDetails);
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

        private bool CheckPermission(Int32 hierarchyNodeID, String orderResultDocCode)
        {
            Boolean isTrue =false;
        //    if( CurrentViewContext.GranularPermission.IsNotNull() &&
        //                                (CurrentViewContext.GranularPermission.Where(cond => cond.HierarchyID == _hierarchyNodeID).Select(x => x.PermissionCode).Contains(EnumSystemPermissionCode.Order_Result_Document.GetStringValue())
        //                                   || CurrentViewContext.GranularPermission.Select(x => x.PermissionCode).Contains(EnumSystemPermissionCode.Service_Group_Result_Document.GetStringValue())
        //                                   || CurrentViewContext.GranularPermission.Count() == AppConsts.NONE) || !CurrentViewContext.GranularPermission.Where(cond => cond.HierarchyID == cond.MasterDpmId && cond.PermissionCode == orderResultDocCode).IsNullOrEmpty();

            isTrue = CurrentViewContext.GranularPermission.IsNotNull() &&
                   (((CurrentViewContext.GranularPermission.Where(cond => cond.HierarchyID == hierarchyNodeID && cond.PermissionCode == orderResultDocCode)).Count() > 0) ||
                    ((CurrentViewContext.GranularPermission.Where(cond => cond.HierarchyID == hierarchyNodeID).IsNullOrEmpty()) &&
                    (CurrentViewContext.GranularPermission.Where(cond => cond.HierarchyID == cond.MasterDpmId && cond.PermissionCode == orderResultDocCode).Count()) > 0)) ? true : false;
            return isTrue;
        }


        /// <summary>
        /// Grid sort expression
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdBkgOrderDetails_SortCommand(object sender, GridSortCommandEventArgs e)
        {
            try
            {
                if (e.NewSortOrder != GridSortOrder.None)
                {
                    GridCustomPaging.SortExpression = e.SortExpression;
                    GridCustomPaging.SortDirectionDescending = e.NewSortOrder.Equals(GridSortOrder.Descending);
                    CurrentViewContext.GridCustomPaging.SortExpression = e.SortExpression;
                    CurrentViewContext.GridCustomPaging.SortDirectionDescending = e.NewSortOrder.Equals(GridSortOrder.Descending);
                }
                else
                {
                    GridCustomPaging.SortExpression = String.Empty;
                    GridCustomPaging.SortDirectionDescending = false;
                    CurrentViewContext.GridCustomPaging.SortExpression = String.Empty;
                    CurrentViewContext.GridCustomPaging.SortDirectionDescending = false;
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
        /// <summary>
        /// Grid item item data bound
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdBkgOrderDetails_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
                {
                  Int32  _hierarchyNodeID = Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["HierarchyNodeID"]).IsNullOrEmpty() ? 0 : Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["HierarchyNodeID"]); //UAT-4522
                    if (e.Item is GridDataItem)
                    {

                        GridDataItem dataItem = (GridDataItem)e.Item;
                        //UAT-1051 Complio: Full social security number should not be displayed on grid exports. Added Duplicate Column with masked SSN
                        // and displayed the masked column on Export instead of actual column.
                        dataItem["_SSN"].Text = Presenter.GetMaskedSSN(Convert.ToString(dataItem["_SSN"].Text));


                        //UAT-806 Creation of granular permissions for Client Admin users
                        if (CurrentViewContext.SSNPermissionCode.ToUpper() == EnumSystemPermissionCode.MASKED_READ_PERMISSION.GetStringValue())
                        {
                            dataItem["SSN"].Text = Presenter.GetMaskedSSN(Convert.ToString(dataItem["SSN"].Text));
                        }
                        else
                        {
                            dataItem["SSN"].Text = Presenter.GetFormattedSSN(Convert.ToString(dataItem["SSN"].Text));
                        }

                        if (Convert.ToString(dataItem["CustomAttributes"].Text).Length > 80)
                        {
                            dataItem["CustomAttributes"].ToolTip = dataItem["CustomAttributes"].Text;
                            dataItem["CustomAttributes"].Text = (dataItem["CustomAttributes"].Text).ToString().Substring(0, 80) + "...";
                        }

                        //UAT 1659                        
                        if (!IsBkgOrderNoteEnabled)
                        {
                            grdBkgOrderDetails.MasterTableView.GetColumn("AddNotes").Visible = false;
                            grdBkgOrderDetails.MasterTableView.GetColumn("OrderNote").Visible = false;
                            grdBkgOrderDetails.MasterTableView.GetColumn("_OrderNote").Visible = false;
                        }
                        else
                        {
                            //if (!CurrentViewContext.GranularPermission.IsNullOrEmpty()
                            //   &&
                            //   (!CurrentViewContext.GranularPermission.Select(x => x.PermissionCode).Contains(EnumSystemPermissionCode.MANAGE_BKG_ORDER_NOTES.GetStringValue())))
                            if (!CurrentViewContext.GranularPermission.IsNullOrEmpty() && !CheckPermission(_hierarchyNodeID, EnumSystemPermissionCode.MANAGE_BKG_ORDER_NOTES.GetStringValue()))
                            {
                                //UAT-4522
                                RadButton btnAddNotes = (RadButton)e.Item.FindControl("btnAddNotes");
                                btnAddNotes.Visible = false;
                                dataItem["_OrderNote"].Text = "";
                                dataItem["OrderNote"].Text = "";
                                //btnAddNotes.Attributes()
                                //RadButton btnAddNotes = (RadButton)e.Item.FindControl("btnAddNotes");
                                //RadButton btnAddNotes = (RadButton)e.Item.FindControl("btnAddNotes");
                                //grdBkgOrderDetails.MasterTableView.GetColumn("AddNotes").Visible = false;
                                //grdBkgOrderDetails.MasterTableView.GetColumn("OrderNote").Visible = false;
                                //grdBkgOrderDetails.MasterTableView.GetColumn("_OrderNote").Visible = false;

                            }
                            else
                            {
                                RadButton btnAddNotes = (RadButton)e.Item.FindControl("btnAddNotes"); //UAT-4522
                                btnAddNotes.Visible = true; //UAT-4522
                            }
                            if (!CurrentViewContext.GranularPermission.IsNullOrEmpty() && CurrentViewContext.GranularPermission.Where(cond => cond.PermissionCode == EnumSystemPermissionCode.MANAGE_BKG_ORDER_NOTES.GetStringValue()).IsNullOrEmpty())
                            {
                                grdBkgOrderDetails.MasterTableView.GetColumn("AddNotes").Visible = false;
                                grdBkgOrderDetails.MasterTableView.GetColumn("OrderNote").Visible = false;
                                grdBkgOrderDetails.MasterTableView.GetColumn("_OrderNote").Visible = false;
                            }
                            else
                            {
                                grdBkgOrderDetails.MasterTableView.GetColumn("AddNotes").Visible = true; 
                                grdBkgOrderDetails.MasterTableView.GetColumn("OrderNote").Visible = true;
                                if (!dataItem["OrderNote"].Text.IsNullOrEmpty() && dataItem["OrderNote"].Text != AppConsts.NON_BREAKING_SPACE)
                                {
                                    //RadButton btnAddNotes = (RadButton)e.Item.FindControl("btnAddNotes");
                                    //btnAddNotes.Text = "Edit Note";
                                    if (Convert.ToString(dataItem["OrderNote"].Text).Length > 20)
                                    {
                                        dataItem["OrderNote"].ToolTip = dataItem["OrderNote"].Text;
                                        dataItem["OrderNote"].Text = (dataItem["OrderNote"].Text).ToString().Substring(0, 20) + "...";
                                    }
                                }
                            }
                        }
                        //if (Convert.ToString(dataItem["ManualServiceForms"].Text).Length > 20)
                        //{
                        //    dataItem["ManualServiceForms"].ToolTip = dataItem["ManualServiceForms"].Text;
                        //    dataItem["ManualServiceForms"].Text = (dataItem["ManualServiceForms"].Text).ToString().Substring(0, 20) + "...";
                        //}
                    }

                    if (IsAdminUser)
                    {
                        #region Order status as flagged
                        // Format if order is marked as "Flagged"
                        HiddenField flaggedIndicatorHiddenField = (HiddenField)e.Item.FindControl("hidGridOrderFlaggedIndicator");
                        Label orderStatusLabel = (Label)e.Item.FindControl("lblGridOrderStatus");
                        if ((flaggedIndicatorHiddenField != null) && (orderStatusLabel != null))
                        {
                            if (flaggedIndicatorHiddenField.Value.ToLower() == "true")
                            {
                                string origStatus = orderStatusLabel.Text;
                                orderStatusLabel.Text = "<span style=\"font-style:italic;\">Flagged</span><br />" + origStatus;
                            }
                        }
                        #endregion

                        //UAT-1069: WB: add order date to the background order search grid
                        BackroundOrderContract myOrder = (BackroundOrderContract)e.Item.DataItem;
                        //Label lblOrderCreateDate = e.Item.FindControl("lblOrderCreateDate") as Label;
                        //if (myOrder.OrderCreatedDate.IsNotNull())
                        //{
                        //    lblOrderCreateDate.Text = myOrder.OrderCreatedDate.Value.ToString("MM/dd/yyyy");
                        //}
                    }
                    if (!IsAdminUser)
                    {
                        Int32 orderID = (Int32)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrderId"];
                        String orderNumber = (String)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrderNumber"];
                        Presenter.GetServiceGroupListByOrderId(orderID);
                        BackroundOrderContract myOrder = (BackroundOrderContract)e.Item.DataItem;
                        Int32 InstitutionStatusColorID = myOrder.InstitutionStatusColorID;
                        #region Find Controls
                        //Label lblOrderCreateDate = e.Item.FindControl("lblOrderCreateDate") as Label;
                        Label lblOrderCompletedDate = e.Item.FindControl("lblOrderCompletedDate") as Label;
                        Label lblClientStatus = e.Item.FindControl("lblClientStatus") as Label;
                        Image imgOrderStatus = (Image)e.Item.FindControl("imgOrderStatus");
                        Image imgPDF = (Image)e.Item.FindControl("imgPDF");
                        Image imgInstitutionStatus = (Image)e.Item.FindControl("imgInstitutionStatus");

                        //UAT-1996
                        LinkButton lnkBtnColorFlag = (LinkButton)e.Item.FindControl("lnkBtnColorFlag");
                        #endregion

                        //Show the Order Completed date time
                        if (myOrder.OrderCompletedDate.IsNotNull())
                        {
                            lblOrderCompletedDate.Text = myOrder.OrderCompletedDate.Value.ToString();
                        }

                        //if (myOrder.OrderCreatedDate.IsNotNull())
                        //{
                        //    lblOrderCreateDate.Text = myOrder.OrderCreatedDate.Value.ToString("MM/dd/yyyy");
                        //}

                        #region Image settings

                        String iconPath = String.Empty;
                        iconPath = ImagePathOrderStatus + "/Blank.gif";
                        imgOrderStatus.ImageUrl = iconPath;
                        imgOrderStatus.Visible = false;
                        imgInstitutionStatus.ImageUrl = iconPath;
                        imgInstitutionStatus.Visible = false;
                        #endregion
                        if (myOrder.BkgOrderStatus.ToLower() == "completed")
                        {
                            if (InstitutionStatusColorID > AppConsts.NONE)
                            {
                                InstitutionOrderFlag institutionOrderFlag = InstitutionOrderFlagList.Where(obj => obj.IOF_ID == InstitutionStatusColorID).FirstOrDefault();

                                if (institutionOrderFlag.IsNotNull())
                                {
                                    Entity.ClientEntity.lkpOrderFlag flag = institutionOrderFlag.lkpOrderFlag;
                                    imgInstitutionStatus.ImageUrl = path + flag.OFL_FileName;
                                    //UAT-2439, Client Admin screen updates for text to icon
                                    imgInstitutionStatus.ToolTip = String.Concat(flag.OFL_Tooltip, " color flag.");
                                    imgInstitutionStatus.Visible = true;
                                }
                            }
                            if (myOrder.IsOrderItemsComplete && !lstBackroundServiceGroup.Any(cond => !cond.IsServiceGroupStatusComplete))
                            {
                                if (myOrder.OrderFlag)
                                {
                                    iconPath = ImagePathOrderStatus + "/Red.gif";
                                    imgOrderStatus.AlternateText = String.Concat(myOrder.OrderNumber, " is flagged");
                                }
                                else
                                {
                                    iconPath = ImagePathOrderStatus + "/Green.gif";
                                    imgOrderStatus.AlternateText = String.Concat(myOrder.OrderNumber, " is clear");
                                }

                                imgOrderStatus.ImageUrl = iconPath;
                                imgOrderStatus.Visible = true;
                                imgPDF.Visible = true;
                                imgPDF.ImageUrl = ImagePathOrderStatus + "/pdf.gif";
                            }

                            if (myOrder.OrderClientStatusID.IsNotNull() && myOrder.OrderClientStatusID > 0)
                            {
                                lblClientStatus.Text = myOrder.OrderClientStatusTypeName ?? "NA";
                            }
                            else
                            {
                                lblClientStatus.Text = "NA";
                            }
                        }

                        if (lstBackroundServiceGroup.Count == AppConsts.ONE && lstBackroundServiceGroup[0].IsOperationSupportAutoCompleteServiceType == true && lstBackroundServiceGroup[0].ServiceCount == AppConsts.ONE)
                        {
                            //Fixed UAT 624- Background Order Queue  For Client Admins only display Service Groups, not the services within the group. 
                            ////Int32 bkgOrderPackageSvcGroupID = lstBackroundServiceGroup[0].BkgOrderPackageSvcGroupID;
                            ////Presenter.GetServicesListByServiceGroupId(bkgOrderPackageSvcGroupID);
                            //if (lstBackroundServicesContract.Count == AppConsts.ONE && lstBackroundServicesContract[0].ServiceTypeCode == BkgServiceType.OPERATIONSUPPORTAUTOCOMPLETE.GetStringValue())
                            //{
                            //    imgPDF.Visible = false;
                            //}

                            imgPDF.Visible = false;

                        }
                        Repeater rptServiceGroups = (Repeater)e.Item.FindControl("rptServiceGroups");
                        CurrentViewContext.HierarchyNodeID = _hierarchyNodeID;
                        if (rptServiceGroups != null)
                        {
                            rptServiceGroups.DataSource = lstBackroundServiceGroup.Where(col => !col.IsAllServiceNonReportable).DistinctBy(cond => cond.BkgOrderPackageSvcGroupID);
                            rptServiceGroups.DataBind();
                        }
                        CurrentViewContext.HierarchyNodeID = 0;
                        #region UAT-1075 WB:Admin Granular permissions for color flag and Result PDF
                        //if (CurrentViewContext.IsBkgResultReportDisable)
                        //{
                        //    imgPDF.Visible = false;
                        //    //UAT-1151 Disable Clearstar Flags.
                        //    imgOrderStatus.Visible = false;
                        //}
                        //if (!CurrentViewContext.GranularPermission.IsNullOrEmpty()
                        //   &&
                        //   (!CurrentViewContext.GranularPermission.Select(x => x.PermissionCode).Contains(EnumSystemPermissionCode.Order_Result_Document.GetStringValue())
                        //   || CurrentViewContext.GranularPermission.Select(x => x.PermissionCode).Contains(EnumSystemPermissionCode.NONE.GetStringValue())))
                        if (!CurrentViewContext.GranularPermission.IsNullOrEmpty() && (!CheckPermission(_hierarchyNodeID, EnumSystemPermissionCode.Order_Result_Document.GetStringValue()) 
                            || CheckPermission(_hierarchyNodeID,EnumSystemPermissionCode.NONE.GetStringValue()))) //UAT-4522
                        {
                            imgPDF.Visible = false;
                        }

                        //UAT-1151 Disable Clearstar Flags.
                        //if (!CurrentViewContext.GranularPermission.IsNullOrEmpty()
                        //   &&
                        //   (!CurrentViewContext.GranularPermission.Select(x => x.PermissionCode).Contains(EnumSystemPermissionCode.Order_Result_Vendor_Status.GetStringValue())
                        //   || CurrentViewContext.GranularPermission.Select(x => x.PermissionCode).Contains(EnumSystemPermissionCode.NONE.GetStringValue())))
                        if (!CurrentViewContext.GranularPermission.IsNullOrEmpty() && (!CheckPermission(_hierarchyNodeID, EnumSystemPermissionCode.Order_Result_Vendor_Status.GetStringValue())
                         || CheckPermission(_hierarchyNodeID, EnumSystemPermissionCode.NONE.GetStringValue()))) //UAT-4522
                        {
                            imgOrderStatus.Visible = false;
                        }

                        if (CurrentViewContext.IsBkgColorFlagDisable)
                        {
                            imgInstitutionStatus.Visible = false;
                        }
                        //UAT-1996
                        else if (IsBkgColorFlagFullPermission && myOrder.BkgOrderStatus.ToLower() == "completed")
                        {
                            lnkBtnColorFlag.Visible = true;
                            if (InstitutionStatusColorID > AppConsts.NONE)
                            {
                                lnkBtnColorFlag.ToolTip = "Edit background order color flag for Order number " + orderNumber;
                                lnkBtnColorFlag.Text = "Edit Color Flag";
                            }
                            else
                            {
                                lnkBtnColorFlag.ToolTip = "Add background order color flag for Order number " + orderNumber;
                                lnkBtnColorFlag.Text = "Add Color Flag";
                            }
                        }

                        //UAT-2178: Color flag column should only show when color flag is enabled for a tenant.
                        if (!CurrentViewContext.IsInstitutionHasOrderFlag)
                        {
                            imgInstitutionStatus.Visible = false;
                            lnkBtnColorFlag.Visible = false;
                        }

                        //if (!CurrentViewContext.GranularPermission.IsNullOrEmpty()
                        //  &&
                        //  (!CurrentViewContext.GranularPermission.Select(x => x.PermissionCode).Contains(EnumSystemPermissionCode.Ability_To_See_Order_Details_Screen_On_Off.GetStringValue())
                        //  || CurrentViewContext.GranularPermission.Select(x => x.PermissionCode).Contains(EnumSystemPermissionCode.NONE.GetStringValue())))
                        if (!CurrentViewContext.GranularPermission.IsNullOrEmpty() && (!CheckPermission(_hierarchyNodeID, EnumSystemPermissionCode.Ability_To_See_Order_Details_Screen_On_Off.GetStringValue())
                         || CheckPermission(_hierarchyNodeID, EnumSystemPermissionCode.NONE.GetStringValue()))) //UAT-4522
                        {
                            LinkButton lbGridOrderId = (LinkButton)e.Item.FindControl("lbGridOrderId");
                            if (!lbGridOrderId.IsNullOrEmpty())
                            {
                                lbGridOrderId.Attributes.Remove("href");
                                lbGridOrderId.Attributes.CssStyle[HtmlTextWriterStyle.Cursor] = "default";
                                if (lbGridOrderId.Enabled != false)
                                {
                                    lbGridOrderId.Enabled = false;
                                }
                            }
                        }

                        #endregion
                    }

                    #region UAT-774

                    String[] checkedOrderIDs = null;
                    if (!hdnOrderID.Value.IsNullOrEmpty())
                    {
                        checkedOrderIDs = hdnOrderID.Value.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                        if (checkedOrderIDs.IsNotNull())
                        {
                            String orderID = Convert.ToString((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrderId"]);
                            if (!String.IsNullOrEmpty(orderID))
                            {
                                if (checkedOrderIDs.Any(cond => cond == orderID))
                                {
                                    CheckBox checkBox = ((CheckBox)e.Item.FindControl("chkSelectUser"));
                                    checkBox.Checked = true;
                                }
                            }
                        }
                    }
                    #endregion
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
        /// <summary>
        /// Hide the un-wanted columns from ExportColumns
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdBkgOrderDetails_ItemCreated(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridCommandItem)
                {
                    //WclComboBox cmdExportColumns = e.Item.FindControl("cmdExportColumns") as WclComboBox;
                    //if (cmdExportColumns.IsNotNull())
                    //{
                    //    cmdExportColumns.Items.FindItemByValue("OrderIdTemp").Remove();
                    //    cmdExportColumns.Items.FindItemByValue("ClearStarStatusTemp").Remove();
                    //    cmdExportColumns.Items.FindItemByValue("BkgOrderStatusTemp").Remove();

                    //    if (IsAdminUser)
                    //    {
                    //        cmdExportColumns.Items.FindItemByValue("CanViewOrderCreatedDate").Remove();
                    //        cmdExportColumns.Items.FindItemByValue("CanViewOrderCompletedDate").Remove();
                    //        cmdExportColumns.Items.FindItemByValue("CanViewDOB").Remove();
                    //        cmdExportColumns.Items.FindItemByValue("CanViewOrderFlag").Remove();
                    //        cmdExportColumns.Items.FindItemByValue("CanViewServiceGroups").Remove();
                    //        cmdExportColumns.Items.FindItemByValue("CanViewOrderClientStatus").Remove();
                    //        cmdExportColumns.Items.FindItemByValue("CanViewOrderArchiveStatus").Remove();
                    //    }
                    //    if (!IsAdminUser)
                    //    {
                    //        cmdExportColumns.Items.FindItemByValue("CanViewOrderPrice").Remove();
                    //        cmdExportColumns.Items.FindItemByValue("CanViewClearStarStatus").Remove();
                    //        cmdExportColumns.Items.FindItemByValue("CanViewServiceGroups").Remove();
                    //        cmdExportColumns.Items.FindItemByValue("CanViewOrderClientStatus").Remove();
                    //        cmdExportColumns.Items.FindItemByValue("CanViewOrderArchiveStatus").Remove();
                    //    }
                    //}
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

        #region Repeater Event
        /// <summary>
        /// Service group repeater set the status of service group and bind the service status.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rptServiceGroups_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                try
                {
                    Image imgStatus = e.Item.FindControl("imgStatus") as Image;
                    HtmlGenericControl divServiceGroup = (HtmlGenericControl)e.Item.FindControl("divGroupStatus");
                    HiddenField hdnBkgServiceGroupMappingId = e.Item.FindControl("hdnBkgServiceGroupMappingId") as HiddenField;
                    Image imgServiceGroupPDF = e.Item.FindControl("imgServiceGroupPDF") as Image;
                    BackroundServiceGroupContract bkgServiceGroup = (BackroundServiceGroupContract)e.Item.DataItem;
                    HiddenField hdnfServiceGroupID = e.Item.FindControl("hdnfServiceGroupID") as HiddenField;
                    HiddenField svcGrpName = e.Item.FindControl("svcGrpName") as HiddenField;
                    HiddenField hdnfBkgPkgSvcGrpID = e.Item.FindControl("hdnfBkgPkgSvcGrpID") as HiddenField;

                    //Flagged and Complete

                    //if (bkgServiceGroup.IsServiceGroupFlagged && bkgServiceGroup.IsServiceGroupComplete)
                    //{
                    //    imgStatus.ImageUrl = ImagePath + "/Red.gif";
                    //    imgStatus.AlternateText = "Red";
                    //    if (divServiceGroup.IsNotNull())
                    //        ShowHideDiv(divServiceGroup, true);
                    //}
                    ////Not flagged and Complete
                    //else if (bkgServiceGroup.IsServiceGroupComplete)
                    //{
                    //    imgStatus.ImageUrl = ImagePath + "/Green.gif";
                    //    imgStatus.AlternateText = "Green";
                    //    if (divServiceGroup.IsNotNull())
                    //        ShowHideDiv(divServiceGroup, true);
                    //}
                    //#endregion
                    ////Not Complete; Verify they can View the Service Status
                    //else
                    //{
                    //    #region Service form status

                    //    imgServiceGroupPDF.ImageUrl = ImagePath + "/Blank.gif";
                    //    HyperLink hlPackageGroupDocument = e.Item.FindControl("hlPackageGroupDocument") as HyperLink;
                    //    hlPackageGroupDocument.Enabled = false;

                    //    //Fixed UAT 624- Background Order Queue  For Client Admins only display Service Groups, not the services within the group. 

                    //    //if (bkgServiceGroup.OrderStatusType.ToLower() == "paid")
                    //    //{
                    //    //    if (hdnBkgServiceGroupMappingId.IsNotNull())
                    //    //    {
                    //    //        bkgServiceGroupMappingId = Convert.ToInt32(hdnBkgServiceGroupMappingId.Value);
                    //    //        GetServicesByServiceGroupId(bkgServiceGroupMappingId);
                    //    //        Repeater rptStatus = e.Item.FindControl("rptStatus") as Repeater;
                    //    //        rptStatus.DataSource = lstBackroundServicesContract;
                    //    //        rptStatus.DataBind();
                    //    //        rptStatus.Visible = true;
                    //    //    }
                    //    //}

                    //    #endregion

                    //}
                    //UAT-1300: WB: As a client admin, I should be able to see completed service group reports as completed service groups are completed
                    Int32 currentServiceGroupID = Convert.ToInt32(hdnfServiceGroupID.Value);
                    Int32 currentBkgPkgSvcGrpID = Convert.ToInt32(hdnfBkgPkgSvcGrpID.Value);
                    //if (!lstBackroundServiceGroup.Any(cond => !cond.IsServiceGroupStatusComplete) && !lstBackroundServiceGroup.Any(cond => !cond.IsServiceGroupComplete))
                    if (lstBackroundServiceGroup.Where(cond => cond.BkgPackageSvcGroupId == currentBkgPkgSvcGrpID).Select(col => col.IsServiceGroupStatusComplete).FirstOrDefault())
                    {
                        if (bkgServiceGroup.IsServiceGroupFlagged)
                        {
                            imgStatus.ImageUrl = ImagePath + "/Red.gif";
                            //UAT-2439,Client Admin screen updates for text to icon
                            imgStatus.AlternateText = String.Concat(svcGrpName.Value, " is flagged");

                            if (divServiceGroup.IsNotNull())
                                ShowHideDiv(divServiceGroup, true);
                        }
                        //Not flagged and Complete
                        else
                        {
                            imgStatus.ImageUrl = ImagePath + "/Green.gif";
                            //UAT-2439,Client Admin screen updates for text to icon
                            imgStatus.AlternateText = String.Concat(svcGrpName.Value, " is clear");

                            if (divServiceGroup.IsNotNull())
                                ShowHideDiv(divServiceGroup, true);
                        }
                    }
                    else
                    {
                        imgServiceGroupPDF.ImageUrl = ImagePath + "/Blank.gif";
                        HyperLink hlPackageGroupDocument = e.Item.FindControl("hlPackageGroupDocument") as HyperLink;
                        hlPackageGroupDocument.Enabled = false;
                    }

                    #region UAT-1075 WB:Admin Granular permissions for color flag and Result PDF
                    HyperLink linkPackageGroupDocument = e.Item.FindControl("hlPackageGroupDocument") as HyperLink;
                    //if (CurrentViewContext.IsBkgResultReportDisable)
                    //{
                    //    linkPackageGroupDocument.Visible = false;
                    //    imgStatus.Visible = false;
                    //}

                    //if (!CurrentViewContext.GranularPermission.IsNullOrEmpty()
                    //       &&
                    //       (!CurrentViewContext.GranularPermission.Select(x => x.PermissionCode).Contains(EnumSystemPermissionCode.Service_Group_Result_Document.GetStringValue())
                    //       || CurrentViewContext.GranularPermission.Select(x => x.PermissionCode).Contains(EnumSystemPermissionCode.NONE.GetStringValue())))
                    if (!CurrentViewContext.GranularPermission.IsNullOrEmpty() && (!CheckPermission(CurrentViewContext.HierarchyNodeID, EnumSystemPermissionCode.Service_Group_Result_Document.GetStringValue())
                        || CheckPermission(CurrentViewContext.HierarchyNodeID, EnumSystemPermissionCode.NONE.GetStringValue()))) //UAT-4522
                    {
                        linkPackageGroupDocument.Visible = false;
                    }

                    //if (!CurrentViewContext.GranularPermission.IsNullOrEmpty()
                    //       &&
                    //       (!CurrentViewContext.GranularPermission.Select(x => x.PermissionCode).Contains(EnumSystemPermissionCode.Service_Group_Vendor_Status.GetStringValue())
                    //       || CurrentViewContext.GranularPermission.Select(x => x.PermissionCode).Contains(EnumSystemPermissionCode.NONE.GetStringValue())))
                    if (!CurrentViewContext.GranularPermission.IsNullOrEmpty() && (!CheckPermission(CurrentViewContext.HierarchyNodeID, EnumSystemPermissionCode.Service_Group_Vendor_Status.GetStringValue())
                             || CheckPermission(CurrentViewContext.HierarchyNodeID, EnumSystemPermissionCode.NONE.GetStringValue()))) //UAT-4522
                    {
                        imgStatus.Visible = false;
                    }

                    //if (!CurrentViewContext.GranularPermission.IsNullOrEmpty()
                    //       &&
                    //    (!CurrentViewContext.GranularPermission.Select(x => x.PermissionCode).Contains(EnumSystemPermissionCode.Ability_To_See_Order_Details_Screen_On_Off.GetStringValue())
                    //    || CurrentViewContext.GranularPermission.Select(x => x.PermissionCode).Contains(EnumSystemPermissionCode.NONE.GetStringValue())))
                    if (!CurrentViewContext.GranularPermission.IsNullOrEmpty() && (!CheckPermission(CurrentViewContext.HierarchyNodeID, EnumSystemPermissionCode.Ability_To_See_Order_Details_Screen_On_Off.GetStringValue())
                             || CheckPermission(CurrentViewContext.HierarchyNodeID, EnumSystemPermissionCode.NONE.GetStringValue()))) //UAT-4522
                    {
                        RadButton hlViewPackageGroup = e.Item.FindControl("hlViewPackageGroup") as RadButton;
                        if (!hlViewPackageGroup.IsNullOrEmpty())
                        {
                            hlViewPackageGroup.Attributes.Remove("href");
                            hlViewPackageGroup.Attributes.CssStyle[HtmlTextWriterStyle.Cursor] = "default";
                            hlViewPackageGroup.Font.Underline = false;
                            if (hlViewPackageGroup.Enabled != false)
                            {
                                hlViewPackageGroup.Enabled = false;
                            }
                            hlViewPackageGroup.CssClass = "rbServiceGrp";
                        }
                    }
                    #endregion

                    if (lstBackroundServiceGroup.Count == AppConsts.ONE && lstBackroundServiceGroup[0].IsOperationSupportAutoCompleteServiceType == true && lstBackroundServiceGroup[0].ServiceCount == AppConsts.ONE)
                    {
                        imgServiceGroupPDF.Visible = false;
                    }
                    else if (lstBackroundServiceGroup.Count > AppConsts.ONE)
                    {
                        foreach (BackroundServiceGroupContract item in lstBackroundServiceGroup)
                        {
                            if (item.IsOperationSupportAutoCompleteServiceType == true && item.ServiceCount == AppConsts.ONE)
                            {
                                imgServiceGroupPDF.Visible = false;
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
        }

        private void GetServicesByServiceGroupId(Int32 bkgServiceGroupMappingId)
        {
            Presenter.GetServicesListByServiceGroupId(bkgServiceGroupMappingId);
        }
        #endregion

        #region Link Button Click Evnet
        /// <summary>
        /// Update the client status
        /// </summary>0
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void hlClientStatus_Click(object sender, EventArgs e)
        {
            RadButton btn = sender as RadButton;
            GridDataItem parentItem = btn.NamingContainer as GridDataItem;
            //Int32 orderID = Int32.Parse(((LinkButton)parentItem.FindControl("lbGridOrderId")).Text);
            Int32 orderID = Convert.ToInt32(((HiddenField)parentItem.FindControl("hdnOrderId")).Value);
            hfOrderID.Value = orderID.ToString();
            hfTenantId.Value = SelectedTenantId.ToString();
            Int32 clientStatusID = Presenter.GetClientStatusByOrderId(orderID);
            if (clientStatusID > 0)
            {
                hfClientOrderStatus.Value = clientStatusID.ToString();
            }
            else
            {
                hfClientOrderStatus.Value = Convert.ToString(AppConsts.NONE);
            }
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "key", "openPopUp();", true);
        }
        /// <summary>
        /// View the package group deatils
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void hlViewPackageGroup_Click(object sender, EventArgs e)
        {
            RadButton btn = sender as RadButton;
            GridDataItem parentItem = btn.NamingContainer as GridDataItem;
            //String orderID = ((LinkButton)((btn.NamingContainer.Parent).Parent).Parent.FindControl("lbGridOrderId")).Text;
            String orderID = Convert.ToString(((HiddenField)((btn.NamingContainer.Parent).Parent).Parent.FindControl("hdnOrderId")).Value);
            hfOrderID.Value = orderID.ToString();
            Int32 selectedTenantId = 0;
            if (!ddlTenantName.SelectedValue.IsNullOrEmpty())
            {
                selectedTenantId = SelectedTenantId;
            }
            else
            {
                selectedTenantId = TenantId;
            }
            Dictionary<String, String> queryString = new Dictionary<String, String>();
            queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "SelectedTenantId", Convert.ToString(selectedTenantId) },                                                      
                                                                    { "OrderId", orderID}, 
                                                                    {"pageName", "Service Groups"}
                                                                 };


            String url = String.Format("~/BkgOperations/Pages/ClientAdminBkgOrderDetailPage.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
            Response.Redirect(url, true);
        }
        #endregion

        #region Radio Button List Event
        /// <summary>
        /// Radio button list of date type event 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rbl_OnSelectedIndexChanged(Object sender, EventArgs e)
        {
            rfvOdrCrtTo.Enabled = true;
            rfvOdrCrtFrm.Enabled = true;

            orderDateType.Focus();
        }

        protected void rbSubscriptionState_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (rbSubscriptionState.SelectedValue == ArchiveState.Archived.GetStringValue())
            //    fsucOrderCmdBar.ExtraButton.Enabled = false;
            //else
            //    fsucOrderCmdBar.ExtraButton.Enabled = true;
            if (rbSubscriptionState.SelectedValue == ArchiveState.Active.GetStringValue())
            {
                ShowHideControl("Archivemun", "btnArchiveOrders", true);

                ShowHideControl("Archivemun", "btnUnArchiveOrders", false);
            }
            else if (rbSubscriptionState.SelectedValue == ArchiveState.Archived.GetStringValue())
            {
                ShowHideControl("Archivemun", "btnArchiveOrders", false);

                ShowHideControl("Archivemun", "btnUnArchiveOrders", true);
            }
            else if (rbSubscriptionState.SelectedValue == ArchiveState.All.GetStringValue())
            {
                ShowHideControl("Archivemun", "btnArchiveOrders", true);

                ShowHideControl("Archivemun", "btnUnArchiveOrders", true);
            }
            rbSubscriptionState.Focus();
        }
        #endregion

        #region Rad Button Event
        /// <summary>
        /// Update the archive status 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rbClientArchive_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                RadButton checkbox = sender as RadButton;
                GridDataItem parentItem = checkbox.NamingContainer as GridDataItem;
                //Int32 orderID = Int32.Parse(((LinkButton)parentItem.FindControl("lbGridOrderId")).Text);
                Int32 orderID = Convert.ToInt32(((HiddenField)parentItem.FindControl("hdnOrderId")).Value);
                BkgOrder bkgOrder = Presenter.GetBkgOrderDetail(orderID);
                if (bkgOrder.IsNotNull())
                {
                    String orderEventDetailNotes = String.Empty;
                    if (checkbox.Checked && (!bkgOrder.BOR_IsArchived))
                    {
                        orderEventDetailNotes = "Changed Order from Active to Archived";
                    }
                    else if (!checkbox.Checked && (bkgOrder.BOR_IsArchived))
                    {
                        orderEventDetailNotes = "Changed Order from Archived to Active";
                    }
                    Presenter.UpdateBkgOrderArchiveStatus(orderID, checkbox.Checked, orderEventDetailNotes, CurrentLoggedInUserId);
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


        #region UAT-1996:
        protected void btnDoPostBackForColorFlag_Click(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToBoolean(hdnColorFlagSavedStatus.Value))
                {
                    grdBkgOrderDetails.Rebind();
                    ShowSuccessMessage("Background order color flag saved successfully.");
                }
                else
                {
                    ShowSuccessMessage(String.Empty);
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
        #endregion

        #endregion

        #region Methods

        #region Private Methods
        /// <summary>
        /// Bind the drop down based on the selected tenant
        /// </summary>
        private void BindDropDownControl()
        {
            if (CurrentViewContext.SelectedTenantId > 0)
            {
                #region Bind Combo box
                Presenter.GetOrderStatusList();
                Presenter.GetOrderStatusType();
                Presenter.GetServiceFormStatus();
                Presenter.GetBackroundServiceList();
                //UAT 1417
                Presenter.GetAllUserGroups();
                //UAT-1683
                Presenter.GetArchiveStateList();
                rbSubscriptionState.Visible = true;
                #endregion

                //if (IsAdminUser)
                //{
                //    #region Bind Combo box

                //    #endregion
                //}
                if (!IsAdminUser)
                {
                    #region Bind Combo box
                    Presenter.GetBkgOrderClientStatus();
                    Presenter.GetBkgServiceGroup();
                    #endregion

                    #region Show Control
                    ShowHideDiv(divServiceGroup, true);
                    rcServiceGroup.Visible = true;
                    ShowHideDiv(divCategory, true);
                    rcbClientStatus.Visible = true;
                    ShowHideDiv(divArchiveStatus, true);
                    rcbArchiveStatus.Visible = true;
                    #endregion
                }
            }
        }

        /// <summary>
        /// Removes all the filters and Sorting on the grid and clears the variables.
        /// </summary>
        private void ResetGridFilters()
        {
            grdBkgOrderDetails.MasterTableView.FilterExpression = null;
            grdBkgOrderDetails.MasterTableView.SortExpressions.Clear();
            grdBkgOrderDetails.CurrentPageIndex = 0;
            grdBkgOrderDetails.MasterTableView.CurrentPageIndex = 0;
            grdBkgOrderDetails.Rebind();
        }
        /// <summary>
        /// Reset the field data clear  
        /// </summary>
        private void ResetPageControls()
        {
            #region Clear Controls
            SelectedTenantId = AppConsts.NONE;
            txtFirstName.Text = String.Empty;
            txtLastName.Text = String.Empty;
            txtOrderNumber.Text = String.Empty;
            //lblinstituteHierarchy.Text = String.Empty;
            hdnDepartmntPrgrmMppng.Value = String.Empty;
            hdnHierarchyLabel.Value = String.Empty;
            hdnInstitutionNodeId.Value = String.Empty;
            orderDateType.SelectedValue = "0";
            dpOdrCrtFrm.SelectedDate = null;
            dpOdrCrtTo.SelectedDate = null;
            txtSSN.Text = String.Empty;
            dpDob.SelectedDate = null;
            SetBkgOrderSearchContract = null;
            VirtualPageCount = AppConsts.NONE;
            //lblStatusColor.Text = "";
            ddlIcon.SelectedIndex = AppConsts.NONE;
            chkCategoryToggle.Checked = false;
            //chkFlaggedToggle.Checked = false;
            chkArchiveToggle.Checked = false;
            chkPaymentStatusToggle.Checked = false;
            rfvOdrCrtTo.Enabled = false;
            rfvOdrCrtFrm.Enabled = false;
            orderDateType.ClearSelection();
            lstUserGroup = new List<Entity.ClientEntity.UserGroup>();
            ddlUserGroup.SelectedIndex = AppConsts.NONE;
            CurrentViewContext.VirtualPageCount = 0;
            chkSelectAllResults.Checked = false;
            //fsucOrderCmdBar.ExtraButton.Enabled = true;
            ShowHideControl("Archivemun", "btnArchiveOrders", true); //UAT-4085
            //UAT-1732: Change Yes/No checkbox for Is flagged to radio buttons with "Flagged", "Not Flagged", and "all"
            rblFlagged.ClearSelection();
            rblFlagged.SelectedValue = AppConsts.STR_TWO;

            #endregion

            if (!IsAdminUser)
            {
                ddlIcon.Visible = true;
                rcbArchiveStatus.SelectedIndex = AppConsts.NONE;
                chkPaymentStatusType.SelectedIndex = AppConsts.NONE;
                chkOrderStatusTypes.SelectedIndex = AppConsts.NONE;
                cmbFormStatus.SelectedIndex = AppConsts.NONE;
                rcbClientStatus.SelectedIndex = AppConsts.NONE;
                rcbClientStatus.SelectedIndex = AppConsts.NONE;
                rcServiceGroup.SelectedIndex = AppConsts.NONE;
                chkServices.SelectedIndex = AppConsts.NONE;
                rbSubscriptionState.Visible = true;
                rbSubscriptionState.SelectedValue = ArchiveState.Active.GetStringValue();
            }
            else
            {
                rbSubscriptionState.ClearSelection();
                rbSubscriptionState.Visible = false;
                ddlTenantName.SelectedIndex = AppConsts.NONE;
                lstPaymentStatus = new List<Entity.ClientEntity.lkpOrderStatu>();
                lstOrderStatusType = new List<Entity.ClientEntity.lkpOrderStatusType>();
                lstBackroundServices = new List<Entity.ClientEntity.BackgroundService>();
                lstServiceFormStatus = new List<lkpServiceFormStatu>();
                chkPaymentStatusType.SelectedIndex = AppConsts.NONE;
                chkOrderStatusTypes.SelectedIndex = AppConsts.NONE;
                rcbClientStatus.SelectedIndex = AppConsts.NONE;
                chkServices.SelectedIndex = AppConsts.NONE;
                ddlIcon.Items.Clear();
                ddlIcon.Visible = false;
                lblStatusColor.Visible = true;
                cmbFormStatus.SelectedIndex = AppConsts.NONE;
            }
            if (IsAdminUser)
            {
                ucCustomAttributeLoaderSearch.Reset();
            }
            else
            {
                ucCustomAttributeLoaderSearch.ResetControlData(true);
            }
            ResetGridFilters();
            Session[AppConsts.BKG_SEARCH_OBJECT_SESSION_KEY] = null;

            #region UAT-774
            hdnOrderID.Value = null;
            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ResetSelectedUsers();", true);
            fsucOrderCmdBar.ClearButton.Style.Add("display", "none");
            #endregion
        }
        /// <summary>
        /// Show the hidden column grid column for client admin search screen
        /// </summary>
        private void ShowGridColumns()
        {
            grdBkgOrderDetails.Columns.FindByUniqueName("CanViewOrderCreatedDate").Display = true;
            grdBkgOrderDetails.Columns.FindByUniqueName("CanViewOrderCompletedDate").Display = true;
            //UAT-806 Creation of granular permissions for Client Admin users
            if (CurrentViewContext.IsDOBDisable)
                grdBkgOrderDetails.Columns.FindByUniqueName("CanViewDOB").Display = false;
            else
                grdBkgOrderDetails.Columns.FindByUniqueName("CanViewDOB").Display = true;

            //UAT-1075 WB:Admin Granular permissions for color flag and Result PDF
            //if (CurrentViewContext.IsBkgColorFlagDisable)
            //    grdBkgOrderDetails.Columns.FindByUniqueName("CanViewStatus").Display = false;
            //else
            //    grdBkgOrderDetails.Columns.FindByUniqueName("CanViewStatus").Display = true;

            grdBkgOrderDetails.Columns.FindByUniqueName("CanViewServiceGroups").Display = true;
            grdBkgOrderDetails.Columns.FindByUniqueName("CanViewStatus").Visible = true;

            //UAT-2110, Add Package Name(s) to Background Order Search for client admins.
            //grdBkgOrderDetails.Columns.FindByUniqueName("BkgPackageNames").Display = true;

            //UAT-1871OrderFlag column on background order search should be dynamic based on granular permissions
            if (CurrentViewContext.LstBkgOrderResultPermissions.IsNotNull() &&
                (CurrentViewContext.GranularPermission.Select(x => x.PermissionCode).Contains(EnumSystemPermissionCode.Order_Result_Document.GetStringValue())
                || CurrentViewContext.GranularPermission.Select(x => x.PermissionCode).Contains(EnumSystemPermissionCode.Service_Group_Result_Document.GetStringValue())
                || CurrentViewContext.LstBkgOrderResultPermissions.Count() == AppConsts.NONE))
            {
                ShowHideDiv(divFlaged, true);
                grdBkgOrderDetails.MasterTableView.GetColumn("CanViewOrderFlag").Display = true;
            }

            ShowHideDiv(divClientAdmin, true);
            chkPaymentStatusToggle.Visible = true;
        }
        /// <summary>
        /// Hide the Grid column for client admin search scrren
        /// </summary>
        private void HideGridColumns()
        {
            grdBkgOrderDetails.Columns.FindByUniqueName("CanViewOrderPrice").Display = false;
            grdBkgOrderDetails.Columns.FindByUniqueName("CanViewClearStarStatus").Display = false;
            grdBkgOrderDetails.Columns.FindByUniqueName("CanViewPaymentStatus").Display = false;
        }
        /// <summary>
        /// Bind the Institution Color combo box
        /// </summary>
        private void BindInstitutionColorFlag()
        {
            try
            {
                ddlIcon.Items.Clear();
                ddlIcon.Items.Add(new RadComboBoxItem("---Select---", "0"));

                Presenter.GetInstitutionStatusColor();
                List<Entity.ClientEntity.InstitutionOrderFlag> institutionOrderFlag = InstitutionOrderFlagList;

                if (institutionOrderFlag.IsNotNull() && institutionOrderFlag.Count() > 0)
                {
                    foreach (InstitutionOrderFlag orderFlag in institutionOrderFlag)
                    {
                        Entity.ClientEntity.lkpOrderFlag flag = orderFlag.lkpOrderFlag;
                        RadComboBoxItem item = new RadComboBoxItem();
                        item.ImageUrl = path + flag.OFL_FileName;
                        item.Value = orderFlag.IOF_ID.ToString();
                        item.Text = flag.OFL_Tooltip;
                        ddlIcon.Items.Add(item);
                    }
                    ddlIcon.DataBind();
                    ddlIcon.Visible = true;
                    ShowHideDiv(divStatusColor, true);
                    lblStatusColor.Visible = false;
                }
                else
                {
                    lblStatusColor.Visible = true;
                    ddlIcon.Visible = false;
                    lblStatusColor.Text = "The Institution does not have color search enabled!";
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
        /// <summary>
        /// Retain the filers on the search screen based on the page type
        /// </summary>
        /// <param name="args"></param>
        private void SetPageControl(Dictionary<String, String> args)
        {
            BkgOrderSearchContract searchItemDataContract = new BkgOrderSearchContract();

            if (Session[AppConsts.BKG_SEARCH_OBJECT_SESSION_KEY].IsNotNull())
            {
                #region Xml Deserialize

                StringReader reader = new StringReader(Convert.ToString(Session[AppConsts.BKG_SEARCH_OBJECT_SESSION_KEY]));
                XmlSerializer ser = new XmlSerializer(typeof(BkgOrderSearchContract));
                XmlTextReader XmlReader = new XmlTextReader(reader);
                try
                {
                    searchItemDataContract = (BkgOrderSearchContract)ser.Deserialize(XmlReader);
                }
                catch (SysXException ex)
                {
                    base.LogError(ex);
                    base.ShowErrorMessage(ex.Message);
                }
                finally
                {
                    XmlReader.Close();
                    reader.Close();
                }

                #endregion

                SelectedTenantId = searchItemDataContract.ClientID;
                if (divTenant.Visible)
                    ddlTenantName.SelectedValue = SelectedTenantId.ToString();
                Presenter.GetOrderStatusList();
                Presenter.GetOrderStatusType();
                Presenter.GetAllUserGroups();
                BindInstitutionColorFlag();
                Presenter.GetArchiveStateList();
                //Date of birth
                if (searchItemDataContract.DateOfBirth.IsNotNull())
                    DOB = searchItemDataContract.DateOfBirth;
                //payment Status
                if (searchItemDataContract.OrderPaymentStatusID.IsNotNull())
                    OrderPaymentStatusID = searchItemDataContract.OrderPaymentStatusID;
                //Order Status
                if (searchItemDataContract.OrderStatusTypeID.IsNotNull())
                    OrderStatusTypeID = searchItemDataContract.OrderStatusTypeID;
                //Institution Status Color
                if (searchItemDataContract.InstitutionStatusColorID.IsNotNull())
                    InstitutionStatusColorID = searchItemDataContract.InstitutionStatusColorID;
                //User Group
                if (searchItemDataContract.UserGroupID.IsNotNull())
                    UserGroupID = searchItemDataContract.UserGroupID;

                if (args.ContainsKey("PageType").IsNotNull() && args["PageType"] == BkgOrderDetailScreenType.AdminBkgOrderDetail.GetStringValue())
                {
                    #region Bind Control for admin
                    //Bind Background services and form status
                    Presenter.GetBackroundServiceList();
                    Presenter.GetServiceFormStatus();

                    if (searchItemDataContract.ServiceFormStatusID.IsNotNull())
                        ServiceFormStatusID = searchItemDataContract.ServiceFormStatusID;
                    if (searchItemDataContract.ServiceID.IsNotNull())
                        ServiceID = searchItemDataContract.ServiceID;

                    #endregion
                }
                else if (args.ContainsKey("PageType").IsNotNull() && args["PageType"] == BkgOrderDetailScreenType.ClientAdminBkgOrderDetail.GetStringValue())
                {
                    #region Bind Control for client admin

                    //Bind the Background client status and service group
                    Presenter.GetBkgOrderClientStatus();
                    Presenter.GetBkgServiceGroup();

                    if (searchItemDataContract.ServiceGroupId.IsNotNull())
                        ServiceGroupId = searchItemDataContract.ServiceGroupId;
                    if (searchItemDataContract.OrderClientStatusID.IsNotNull())
                        OrderClientStatusID = searchItemDataContract.OrderClientStatusID;
                    if (searchItemDataContract.IsArchive.IsNotNull())
                        IsArchive = searchItemDataContract.IsArchive;
                    if (searchItemDataContract.IsPaymentStatusChecked.IsNotNull())
                    {
                        chkPaymentStatusToggle.Checked = searchItemDataContract.IsPaymentStatusChecked;
                        grdBkgOrderDetails.Columns.FindByUniqueName("CanViewPaymentStatus").Display = chkPaymentStatusToggle.Checked;
                    }
                    if (searchItemDataContract.IsClientStatusChecked.IsNotNull())
                    {
                        chkCategoryToggle.Checked = searchItemDataContract.IsClientStatusChecked;
                        grdBkgOrderDetails.Columns.FindByUniqueName("CanViewOrderClientStatus").Display = (chkCategoryToggle.Checked && DisplayOrderClientStatus);
                    }

                    //UAT-1732: Change Yes/No checkbox for Is flagged to radio buttons with "Flagged", "Not Flagged", and "all"
                    //if (searchItemDataContract.IsFlagged.IsNotNull())
                    //    IsFlagged = searchItemDataContract.IsFlagged;
                    //if (searchItemDataContract.IsFlaggedChecked.IsNotNull())
                    //{
                    //    chkFlaggedToggle.Checked = searchItemDataContract.IsFlaggedChecked;
                    //    grdBkgOrderDetails.Columns.FindByUniqueName("CanViewOrderFlag").Display = chkFlaggedToggle.Checked;
                    //}

                    if (!searchItemDataContract.SelectedFlagged.IsNullOrEmpty())
                    {
                        CurrentViewContext.SelectedFlagged = searchItemDataContract.SelectedFlagged.ToString();
                        if (CurrentViewContext.SelectedFlagged == AppConsts.ZERO)
                            grdBkgOrderDetails.Columns.FindByUniqueName("CanViewOrderFlag").Display = false;
                        else
                            grdBkgOrderDetails.Columns.FindByUniqueName("CanViewOrderFlag").Display = true;
                    }

                    if (searchItemDataContract.IsArchiveChecked.IsNotNull())
                    {
                        chkArchiveToggle.Checked = searchItemDataContract.IsArchiveChecked;
                        grdBkgOrderDetails.Columns.FindByUniqueName("CanViewOrderArchiveStatus").Display = (chkArchiveToggle.Checked && DisplayOrderArchiveStatus);
                    }

                    if (searchItemDataContract.ServiceID.IsNotNull())
                        ServiceID = searchItemDataContract.ServiceID;
                    #endregion
                }
                txtFirstName.Text = searchItemDataContract.ApplicantFirstName;
                txtLastName.Text = searchItemDataContract.ApplicantLastName;
                txtOrderNumber.Text = searchItemDataContract.OrderNumber.IsNullOrEmpty() ? String.Empty : searchItemDataContract.OrderNumber;
                if (CurrentViewContext.SSNPermissionCode.ToUpper() == EnumSystemPermissionCode.MASKED_READ_PERMISSION.GetStringValue().ToUpper())
                {
                    if (!searchItemDataContract.ApplicantSSN.IsNullOrEmpty())
                    {
                        CurrentViewContext.SSN = searchItemDataContract.ApplicantSSN.Substring(searchItemDataContract.ApplicantSSN.Length - AppConsts.FOUR);
                        txtSSN.Text = CurrentViewContext.SSN;
                    }
                }
                else
                {
                    CurrentViewContext.SSN = searchItemDataContract.ApplicantSSN;
                    txtSSN.Text = CurrentViewContext.SSN;
                }
                if (searchItemDataContract.OrderFromDate.IsNotNull() && searchItemDataContract.OrderToDate.IsNotNull())
                {
                    dpOdrCrtFrm.SelectedDate = searchItemDataContract.OrderFromDate;
                    dpOdrCrtTo.SelectedDate = searchItemDataContract.OrderToDate;
                    orderDateType.SelectedValue = BkgOrderDateType.Created.GetStringValue();
                    rfvOdrCrtTo.Enabled = true;
                    rfvOdrCrtFrm.Enabled = true;
                }
                if (searchItemDataContract.PaidFromDate.IsNotNull() && searchItemDataContract.PaidToDate.IsNotNull())
                {
                    dpOdrCrtFrm.SelectedDate = searchItemDataContract.PaidFromDate;
                    dpOdrCrtTo.SelectedDate = searchItemDataContract.PaidToDate;
                    orderDateType.SelectedValue = BkgOrderDateType.Paid.GetStringValue();
                    rfvOdrCrtTo.Enabled = true;
                    rfvOdrCrtFrm.Enabled = true;
                }
                if (searchItemDataContract.OrderCompletedFromDate.IsNotNull() && searchItemDataContract.OrderCompletedToDate.IsNotNull())
                {
                    dpOdrCrtFrm.SelectedDate = searchItemDataContract.OrderCompletedFromDate;
                    dpOdrCrtTo.SelectedDate = searchItemDataContract.OrderCompletedToDate;
                    orderDateType.SelectedValue = BkgOrderDateType.Completed.GetStringValue();
                    rfvOdrCrtTo.Enabled = true;
                    rfvOdrCrtFrm.Enabled = true;
                }
                //if (searchItemDataContract.DeptProgramMappingID > 0)
                //{
                //    hdnTenantId.Value = searchItemDataContract.ClientID.ToString();
                //    hdnHierarchyLabel.Value = searchItemDataContract.NodeLabel;
                //    hdnDepartmntPrgrmMppng.Value = searchItemDataContract.DeptProgramMappingID.ToString();
                //}
                //UAT-1055 - Multiple Node Search

                //if (!searchItemDataContract.DeptProgramMappingIDs.IsNullOrEmpty())
                //{ commented for UAt-1783.

                hdnTenantId.Value = searchItemDataContract.ClientID.ToString();
                hdnHierarchyLabel.Value = searchItemDataContract.NodeLabel;
                hdnDepartmntPrgrmMppng.Value = searchItemDataContract.DeptProgramMappingIDs;

                //UAT-1725
                hdnInstitutionNodeId.Value = searchItemDataContract.NodeIds;
                CurrentViewContext.DPM_IDs = searchItemDataContract.DeptProgramMappingIDs;
                CurrentViewContext.NodeIds = searchItemDataContract.NodeIds;
                CurrentViewContext.NodeLabel = searchItemDataContract.NodeLabel;
                ucCustomAttributeLoaderSearch.previousValues = searchItemDataContract.CustomFields;
                ucCustomAttributeLoaderSearch.NodeIds = searchItemDataContract.NodeIds;

                //}

                //CurrentViewContext.PageSize = searchItemDataContract.GridCustomPagingArguments.PageSize;
                //CurrentViewContext.CurrentPageIndex = searchItemDataContract.GridCustomPagingArguments.CurrentPageIndex;
                //CurrentViewContext.VirtualPageCount = searchItemDataContract.GridCustomPagingArguments.VirtualPageCount;
                CurrentViewContext.GridCustomPaging = searchItemDataContract.GridCustomPagingArguments;
                if (!CurrentViewContext.GridCustomPaging.SortExpression.IsNullOrEmpty())
                {
                    GridSortExpression gridSortExpression = new GridSortExpression();
                    gridSortExpression.FieldName = CurrentViewContext.GridCustomPaging.SortExpression;
                    gridSortExpression.SortOrder = CurrentViewContext.GridCustomPaging.SortDirectionDescending ? GridSortOrder.Descending : GridSortOrder.Ascending;
                    grdBkgOrderDetails.MasterTableView.SortExpressions.Add(gridSortExpression);
                }

                #region UAT-774
                if (!SelectedTenantId.IsNullOrEmpty() && SelectedTenantId > 0)
                {
                    fsucOrderCmdBar.ClearButton.Style.Clear();
                }

                #endregion

                //UAT-1683
                if (!searchItemDataContract.SelectedArchieveStateId.IsNullOrEmpty())
                {
                    CurrentViewContext.SelectedArchiveStateCode = searchItemDataContract.SelectedArchieveStateId;
                }
                grdBkgOrderDetails.Visible = true;
            }
        }
        /// <summary>
        /// Show and hide the div ?
        /// </summary>
        /// <param name="divControl">divControl</param>
        /// <param name="visibility">visibility</param>
        private void ShowHideDiv(HtmlControl divControl, Boolean visibility)
        {
            divControl.Visible = visibility;
        }

        /// <summary>
        /// Method used to check command format of ExportFormat Dropdown.
        /// </summary>
        /// <param name="cmbExportFormat"></param>
        /// <returns>true if command selected is matched</returns>
        private static bool IsExportCommand(WclComboBox cmbExportFormat)
        {
            return cmbExportFormat.IsNotNull() && (cmbExportFormat.SelectedValue == "Csv" || cmbExportFormat.SelectedValue == "Pdf" || cmbExportFormat.SelectedValue == "Excel");
        }

        private void DisplayMessageSentStatus()
        {
            if (hdMessageSent.Value == "sent")
            {
                base.ShowSuccessMessage("Message has been sent successfully.");
                hdMessageSent.Value = "new";
            }
        }
        #endregion

        #region Public Method
        /// <summary>
        /// List of child controls
        /// </summary>
        public override List<String> ChildScreenPathCollection
        {
            get
            {
                List<String> childScreenPathCollection = new List<String>();
                childScreenPathCollection.Add(@"~/BkgOperations/UserControl/BkgNotes.ascx");
                childScreenPathCollection.Add(@"~/BkgOperations/UserControl/OrderDetailPage.ascx");
                return childScreenPathCollection;
            }
        }

        //UAT-4085
        private void ShowHideControl(String menuText, String controlID, Boolean isVisible)
        {
            RadMenuItem menuItem = cmd.FindItemByText(menuText);

            foreach (RadMenuItem item in menuItem.Items)
            {
                RadButton btnMenu = (RadButton)item.FindControl(controlID);
                if (btnMenu.IsNotNull())
                {
                    btnMenu.Visible = isVisible;
                }
            }
        }
        #endregion

        #endregion

        #region Action Permission
        /// <summary>
        /// Set action level permissions
        /// </summary>
        /// <param name="ctrlCollection">ctrlCollection</param>
        /// <param name="screenName">screenName</param>
        protected override void ApplyActionLevelPermission(List<Entity.ClientEntity.ClsFeatureAction> ctrlCollection, string screenName = "")
        {
            base.ApplyActionLevelPermission(ctrlCollection, screenName);
            ApplyPermisions();
        }
        /// <summary>
        /// Add the action permissions 
        /// </summary>
        public override List<Entity.ClientEntity.ClsFeatureAction> ActionCollection
        {
            get
            {
                List<Entity.ClientEntity.ClsFeatureAction> actionCollection = new List<Entity.ClientEntity.ClsFeatureAction>();
                Entity.ClientEntity.ClsFeatureAction objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                objClsFeatureAction.CustomActionId = "OrderClientStatus";
                objClsFeatureAction.CustomActionLabel = "Order Client Status";
                objClsFeatureAction.ScreenName = "Background Order Queue";
                actionCollection.Add(objClsFeatureAction);

                actionCollection.Add(new Entity.ClientEntity.ClsFeatureAction
                {
                    CustomActionId = "ArchiveStatus",
                    CustomActionLabel = "ArchiveStatus",
                    ScreenName = "Background Order Queue"
                });

                return actionCollection;
            }
        }


        /// <summary>
        /// Set the permission on control based action permission 
        /// </summary>
        private void ApplyPermisions()
        {
            List<Entity.FeatureRoleAction> permission = base.ActionPermission;
            if (permission.IsNotNull())
            {
                permission.ForEach(x =>
                {
                    switch (x.PermissionID)
                    {
                        case AppConsts.ONE:
                            {
                                break;
                            }
                        case AppConsts.THREE:
                        case AppConsts.FOUR:
                            {

                                if (x.FeatureAction.CustomActionId == "OrderClientStatus")
                                {
                                    grdBkgOrderDetails.MasterTableView.GetColumn("CanViewOrderClientStatus").Display = false;
                                    DisplayOrderClientStatus = false;
                                }

                                if (x.FeatureAction.CustomActionId == "ArchiveStatus")
                                {
                                    grdBkgOrderDetails.MasterTableView.GetColumn("CanViewOrderArchiveStatus").Display = false;
                                    DisplayOrderArchiveStatus = false;
                                }

                                break;
                            }
                    }

                }
                    );
            }
        }

        #endregion

        #region UAT-806 Creation of granular permissions for Client Admin users

        /// <summary>
        /// Hide Show grid and page controls
        /// </summary>
        private void HideShowControlsForGranularPermission()
        {
            if (CurrentViewContext.IsDOBDisable)
            {
                divDOB.Visible = false;
            }
            if (CurrentViewContext.SSNPermissionCode.ToUpper() == EnumSystemPermissionCode.NO_ACCESS_PERMISSION.GetStringValue().ToUpper())
            {
                divSSN.Visible = false;
                grdBkgOrderDetails.MasterTableView.GetColumn("SSN").Visible = false;
                //Hide Masked column if user does not have permission to view SSN Column.
                grdBkgOrderDetails.MasterTableView.GetColumn("_SSN").Visible = false;
            }

            //else if (!this.IsPostBack &&
            //    CurrentViewContext.SSNPermissionCode.ToUpper() == EnumSystemPermissionCode.MASKED_READ_PERMISSION.GetStringValue().ToUpper())
            //{
            //    //txtSSN.Mask = AppConsts.SSN_MASK_FORMATE; //@"\#\#\#-\#\#-####"
            //    txtSSN.Mask = AppConsts.SSN_MASK_FORMAT_ALPHANUMERIC; //@"\#\#\#-\#\#-####"
            //}

            //UAT-3010:-  Granular Permission for Client Admin Users to Archive.

            if (CurrentViewContext.ArchivePermissionCode.ToUpper() == EnumSystemPermissionCode.NO_ACCESS_PERMISSION.GetStringValue().ToUpper())
            {
                //fsucOrderCmdBar.ExtraButton.Style.Add("display", "none");
                ShowHideControl("Archivemun", "btnArchiveOrders", false); //UAT-4085
            }
        }
        #endregion

        private void ApplySSNMask()
        {
            if (CurrentViewContext.SSNPermissionCode.ToUpper() == EnumSystemPermissionCode.MASKED_READ_PERMISSION.GetStringValue().ToUpper())
            {
                //txtSSN.Mask = AppConsts.SSN_MASK_FORMATE; //@"\#\#\#-\#\#-####"
                txtSSN.Mask = AppConsts.SSN_MASK_FORMAT_ALPHANUMERIC;
            }
        }

        protected void chkSelectAllResults_CheckedChanged(object sender, EventArgs e)
        {
            if (CurrentViewContext.VirtualPageCount > 0)
            {
                bool needToCheckboxChecked = false;

                if (((CheckBox)sender).Checked)
                {
                    Presenter.GetAllOrderIds();
                    needToCheckboxChecked = true;
                    hdnOrderID.Value = CurrentViewContext.SelectedOrderIds;
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "AddOrderIdsInArray();", true);
                }
                else
                {
                    CurrentViewContext.SelectedOrderIds = string.Empty;
                    hdnOrderID.Value = string.Empty;
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ResetSelectedUsers();", true);
                }

                foreach (GridDataItem item in grdBkgOrderDetails.Items)
                {
                    CheckBox checkBox = ((CheckBox)item.FindControl("chkSelectUser"));
                    checkBox.Checked = needToCheckboxChecked;
                }

                GridHeaderItem headerItem = grdBkgOrderDetails.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
                CheckBox headerCheckBox = ((CheckBox)headerItem.FindControl("chkSelectAll"));
                headerCheckBox.Checked = needToCheckboxChecked;
            }

            chkSelectAllResults.Focus();
        }

        #region UAT-1795 : Add D&A download button on Background Order Queue search.

        protected void fsucCmdExport_ExtraClick(object sender, EventArgs e)
        {
            try
            {
                ExportSeletedDocument();
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

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                ExportSeletedDocument();
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

        private void ExportSeletedDocument()
        {
            CurrentViewContext.lstSeletedOrderIds = new List<Int32>();
            if (!hdnOrderID.Value.IsNullOrEmpty())
            {
                foreach (var orderId in hdnOrderID.Value.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
                {
                    CurrentViewContext.lstSeletedOrderIds.Add(Convert.ToInt32(orderId));
                }
                Presenter.GetAllDnADocument();
                ExportDocuments();
            }
            else
            {
                base.ShowInfoMessage("Please select Order(s) to Export D&A Document(s).");
            }
        }

        /// <summary>
        /// Method to export the document(s) as zip.
        /// </summary>
        private void ExportDocuments()
        {
            List<BkgOrderSearchQueueContract> documentList = new List<BkgOrderSearchQueueContract>();
            Int32 fileCount = AppConsts.NONE;
            documentList = CurrentViewContext.DocumentListToExport.DistinctBy(x => x.ApplicantDocumentID).ToList();
            if (documentList.IsNotNull() && documentList.Count > 0)
            {
                String tempFilePath = WebConfigurationManager.AppSettings["TemporaryFileLocation"];
                if (tempFilePath.IsNullOrEmpty())
                {
                    base.LogError("Please provide path for TemporaryFileLocation in config.", null);
                    return;
                }
                if (!tempFilePath.EndsWith(@"\"))
                {
                    tempFilePath += @"\";
                }
                tempFilePath += "Tenant_" + SelectedTenantId.ToString() + "_Zip_" + (DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("mmss")) + @"\";

                if (!Directory.Exists(tempFilePath))
                    Directory.CreateDirectory(tempFilePath);
                DirectoryInfo dirInfo = new DirectoryInfo(tempFilePath);
                try
                {
                    foreach (var applicantDocumentToExport in documentList)
                    {
                        String fileExtension = Path.GetExtension(applicantDocumentToExport.DocumentPath);
                        String fileName = GetFileName(applicantDocumentToExport.FileName);
                        fileName = Path.GetFileNameWithoutExtension(fileName);
                        String OrderNumber = applicantDocumentToExport.OrderNumber;
                        String finalFileName = fileName + "_" + OrderNumber + "_" + fileExtension;

                        String newTempFilePath = Path.Combine(tempFilePath, finalFileName);
                        byte[] fileBytes = null;
                        fileBytes = CommonFileManager.RetrieveDocument(applicantDocumentToExport.DocumentPath, FileType.ApplicantFileLocation.GetStringValue());

                        if (fileBytes.IsNotNull())
                        {
                            try
                            {
                                File.WriteAllBytes(newTempFilePath, fileBytes);
                            }
                            catch (Exception ex)
                            {
                                base.LogError("Error found in bytes write for DocumentID: " + applicantDocumentToExport.ApplicantDocumentID.ToString(), ex);
                            }
                        }
                    }
                    fileCount = Directory.GetFiles(tempFilePath).Count();
                    if (fileCount > AppConsts.NONE)
                    {
                        ifrExportDocument.Src = "~/ComplianceOperations/UserControl/DoccumentDownload.aspx?zipFilePath=" + tempFilePath + "&IsMultipleFileDownloadInZip=" + "True";
                    }
                    else
                    {
                        base.ShowInfoMessage("No D&A document(s) found to export.");
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
            else
            {
                base.ShowInfoMessage("No D&A document(s) found to export.");
            }
        }

        /// <summary>
        /// Replace spaces with '-' from the file Name
        /// </summary>
        /// <param name="fileNameWithExt"></param>
        /// <returns></returns>
        private String GetFileName(String fileNameWithExt)
        {
            fileNameWithExt = fileNameWithExt.Replace(@"\", @"-");
            fileNameWithExt = fileNameWithExt.Replace(@" ", @"_");
            return fileNameWithExt;
        }

        #endregion



      
    }
}