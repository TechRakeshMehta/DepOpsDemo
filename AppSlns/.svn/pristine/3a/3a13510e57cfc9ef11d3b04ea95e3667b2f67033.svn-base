using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using INTSOF.Utils;
using Telerik.Web.UI;
using System.Linq;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Core;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using Entity.ClientEntity;

namespace CoreWeb.ClinicalRotation.Views
{
    public partial class RequirementVerificationQueue : BaseUserControl, IRequirementVerificationQueueView
    {
        #region Variables

        private RequirementVerificationQueuePresenter _presenter = new RequirementVerificationQueuePresenter();
        private String _viewType;
        private Int32 _tenantId = 0;

        #endregion;

        #region Properties

        public RequirementVerificationQueuePresenter Presenter
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

        public IRequirementVerificationQueueView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        Int32 IRequirementVerificationQueueView.SelectedTenantId
        {
            get
            {
                if (String.IsNullOrEmpty(ddlTenantName.SelectedValue))
                    return 0;
                return Convert.ToInt32(ddlTenantName.SelectedValue);
            }
            set
            {
                if (value > 0)
                {
                    ddlTenantName.SelectedValue = Convert.ToString(value);
                }
                else
                {
                    ddlTenantName.SelectedIndex = value;
                }
            }
        }

        Int32 IRequirementVerificationQueueView.TenantId
        {
            get
            {
                if (_tenantId == 0)
                {
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    if (user.IsNotNull())
                    {
                        _tenantId = user.TenantId.HasValue ? user.TenantId.Value : 0;
                    }
                }
                return _tenantId;
            }
            set { _tenantId = value; }
        }

        Int32 IRequirementVerificationQueueView.CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        List<TenantDetailContract> IRequirementVerificationQueueView.lstTenant
        {
            get;
            set;
        }

        Boolean IRequirementVerificationQueueView.IsAdminLoggedIn
        {
            get;
            set;
        }

        String IRequirementVerificationQueueView.ApplicantFirstName
        {
            get
            {
                return txtFirstName.Text;
            }
            set
            {
                txtFirstName.Text = value;
            }
        }

        String IRequirementVerificationQueueView.ApplicantLastName
        {
            get
            {
                return txtLastName.Text;
            }
            set
            {
                txtLastName.Text = value;
            }
        }

        Int32 IRequirementVerificationQueueView.SelectedAgencyID
        {
            get
            {
                if (String.IsNullOrEmpty(cmbAgency.SelectedValue))
                    return 0;
                return Convert.ToInt32(cmbAgency.SelectedValue);
            }
            set
            {
                if (value > 0)
                {
                    cmbAgency.SelectedValue = value.ToString();
                }
                else
                {
                    cmbAgency.SelectedIndex = value;
                }
            }
        }

        String IRequirementVerificationQueueView.SelectedPackageID
        {
            get
            {
                List<Int32> lstSelectedPackageID = new List<Int32>();
                foreach (RadComboBoxItem item in cmbPackage.CheckedItems)
                {
                    lstSelectedPackageID.Add(Convert.ToInt32(item.Value));
                }
                return String.Join(",", lstSelectedPackageID);
            }
            set
            {
                if (!value.IsNullOrEmpty())
                {
                    List<String> lstSelectedPackageID = value.Split(',').ToList();
                    foreach (RadComboBoxItem item in cmbPackage.Items)
                    {
                        if (lstSelectedPackageID.Contains(item.Value))
                            item.Checked = true;
                    }
                }
            }
        }

        
        DateTime? IRequirementVerificationQueueView.RotationStartDate
        {
            get
            {
                return dpRotationStartDate.SelectedDate;
            }
            set
            {
                dpRotationStartDate.SelectedDate = value;
            }
        }

        DateTime? IRequirementVerificationQueueView.RotationEndDate
        {
            get
            {
                return dpRotationEndDate.SelectedDate;
            }
            set
            {
                dpRotationEndDate.SelectedDate = value;
            }
        }

        DateTime? IRequirementVerificationQueueView.SubmissionDate
        {
            get
            {
                return dpSubmissionDate.SelectedDate;
            }
            set
            {
                dpSubmissionDate.SelectedDate = value;
            }
        }

        Boolean IRequirementVerificationQueueView.IsCurrentRotation
        {
            get
            {
                return chkCurrentRotation.Checked;
            }
            set
            {
                chkCurrentRotation.Checked = value;
            }
        }

       public String ReqCategoryId //UAT-4705
        {
            get
            {
                if (!ddlCategoryLabel.SelectedValue.IsNullOrEmpty())
                    return ddlCategoryLabel.SelectedValue;
                else
                    return String.Empty;
            }
            set
            {
                if (ddlCategoryLabel.Items.Count > 0)
                {
                    ddlCategoryLabel.SelectedValue = value.ToString();
                }
            }
        }

        public String CategoryId
        {
            get
            {
                if (!ddlCategoryLabel.SelectedValue.IsNullOrEmpty())
                    return ddlCategoryLabel.SelectedValue;
                else
                    return String.Empty;
            }
            set
            {
                if (ddlCategoryLabel.Items.Count > 0)
                {
                    ddlCategoryLabel.SelectedValue = value.ToString();
                }
            }
        }
        public String ReqItemId
        {
            get
            {
                if (!ddlItemLabel.SelectedValue.IsNullOrEmpty())
                    return ddlItemLabel.SelectedValue;
                else
                    return string.Empty;
            }
            set
            {
                if (ddlItemLabel.Items.Count > 0)
                {
                    ddlItemLabel.SelectedValue = value.ToString();
                }
            }
        }

        public List<RequirementCategoryContract> LstRequirementCategory
        {
            set
            {
                ddlCategoryLabel.DataSource = value;
                ddlCategoryLabel.DataBind();
            }
        }

        public List<RequirementItemContract> LstRequirementItems
        {
            set
            {
                ddlItemLabel.DataSource = value;
                ddlItemLabel.DataBind();
            }
        }


        String IRequirementVerificationQueueView.RequirementPackageTypes
        {
            get
            {
                //UAT-2197:Requirement Verification Queue: "Requirement Package Type" should not be required
                String selectedPackageType = String.Empty;
                if (!IsRotationPackageVerificationQueue)
                {
                    List<Int32> lstSelectedRequirementPackageTypeID = new List<Int32>();
                    foreach (RadComboBoxItem item in cmbRequirementPackageType.CheckedItems)
                    {
                        lstSelectedRequirementPackageTypeID.Add(Convert.ToInt32(item.Value));
                    }
                    selectedPackageType = String.Join(",", lstSelectedRequirementPackageTypeID);
                }
                return selectedPackageType;

            }
            set
            {
                //cmbRequirementPackageType.SelectedValue = value.ToString();
                ////UAT-2197:Requirement Verification Queue: "Requirement Package Type" should not be required
                List<String> lstSelectedRequirementPackageType = new List<String>();
                if (!value.IsNullOrEmpty())
                {
                    lstSelectedRequirementPackageType = value.Split(',').ToList();
                }

                cmbRequirementPackageType.Items.ForEach(itm =>
                {
                    if (lstSelectedRequirementPackageType.Contains(itm.Value))
                        itm.Checked = true;
                    else
                        itm.Checked = false;
                });

            }
        }

        List<AgencyDetailContract> IRequirementVerificationQueueView.lstAgency
        {
            get;
            set;
        }

        List<RequirementPackageTypeContract> IRequirementVerificationQueueView.lstRequirementPackageType
        {
            get;
            set;
        }

        List<RequirementPackageContract> IRequirementVerificationQueueView.LstRequirementPackage
        {
            get;
            set;
        }

        List<RequirementVerificationQueueContract> IRequirementVerificationQueueView.ApplicantSearchData
        {
            get;
            set;
        }

        Boolean IRequirementVerificationQueueView.IsSearchClicked
        {
            get
            {
                if (!ViewState["IsSearchClicked"].IsNull())
                {
                    return Convert.ToBoolean(ViewState["IsSearchClicked"]);
                }
                return false;
            }
            set
            {
                ViewState["IsSearchClicked"] = value;
            }
        }

        public List<Int32> SelectedPackageIds
        {
            get
            {
                return ViewState["SelectedPackageIds"] as List<Int32>;
            }
            set
            {
                ViewState["SelectedPackageIds"] = value;
            }
        }


        public List<Int32> SelectedCategoryIds
        {
            get
            {
                return ViewState["SelectedCategoryIds"] as List<Int32>;
            }
            set
            {
                ViewState["SelectedCategoryIds"] = value;
            }
        }

        public List<Int32> SelectedItemID
        {
            get
            {
                return ViewState["SelectedCategoryIds"] as List<Int32>;
            }
            set
            {
                ViewState["SelectedCategoryIds"] = value;
            }
        }


        String IRequirementVerificationQueueView.ErrorMessage
        {
            get;
            set;
        }

        String IRequirementVerificationQueueView.SuccessMessage
        {
            get;
            set;
        }

        String IRequirementVerificationQueueView.InfoMessage
        {
            get;
            set;
        }

        public Boolean IsRotationPackageVerificationQueue
        {
            get
            {
                if (!ViewState["IsRotationPackageVerificationQueue"].IsNull())
                {
                    return Convert.ToBoolean(ViewState["IsRotationPackageVerificationQueue"]);
                }
                return false;
            }
            set
            {
                ViewState["IsRotationPackageVerificationQueue"] = value;
            }
        }

        //UAT-4014
        //Represents the list of user types: Applicant and Instr/Preceptor
        Dictionary<String, String> IRequirementVerificationQueueView.dicUserTypes
        {
            get
            {
                if (!ViewState["dicUserTypes"].IsNullOrEmpty())
                    return (Dictionary<String, String>)ViewState["dicUserTypes"];
                return new Dictionary<String, String>();
            }
            set
            {
                ViewState["dicUserTypes"] = value;
            }
        }
        String IRequirementVerificationQueueView.SelectedUserTypeIds
        {
            get
            {
                return String.Join(",", ddlUserType.CheckedItems.Select(x => x.Value));
            }
        }

        public Int32 CurrentTenantId_Global
        {
            get { return (Int32)(ViewState["CurrentTenantId"]); }
            set { ViewState["CurrentTenantId"] = value; }
        }

        #region Custom Paging

        /// <summary>
        /// Current Page Index</summary>
        /// <value>
        /// Gets or sets the value for CurrentPageIndex.</value>
        public Int32 CurrentPageIndex
        {
            get
            {
                return grdRequirementVerificationQueue.MasterTableView.CurrentPageIndex + 1;
            }
            set
            {
                if (grdRequirementVerificationQueue.MasterTableView.CurrentPageIndex > 0)
                {
                    grdRequirementVerificationQueue.MasterTableView.CurrentPageIndex = value - 1;
                }
            }
        }

        /// <summary>
        /// Page Size</summary>
        /// <value>
        /// Gets the value for PageSize.</value>
        public Int32 PageSize
        {
            get
            {
                return grdRequirementVerificationQueue.PageSize;
            }
            set
            {
                grdRequirementVerificationQueue.PageSize = value;
            }
        }

        /// <summary>
        /// Virtual Page Count</summary>
        /// <value>
        /// Sets the value for VirtualPageCount.</value>
        public Int32 VirtualRecordCount
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
                grdRequirementVerificationQueue.VirtualItemCount = value;
                grdRequirementVerificationQueue.MasterTableView.VirtualItemCount = value;
            }
        }

        /// <summary>
        /// To get object of shared class of custom paging
        /// </summary>
        public CustomPagingArgsContract GridCustomPaging
        {
            get
            {
                if (ViewState["GridCustomPaging"] == null)
                {
                    ViewState["GridCustomPaging"] = new CustomPagingArgsContract();
                }
                return (CustomPagingArgsContract)ViewState["GridCustomPaging"];
            }
            set
            {
                ViewState["GridCustomPaging"] = value;
                VirtualRecordCount = value.VirtualPageCount;
                PageSize = value.PageSize;
                CurrentPageIndex = value.CurrentPageIndex;
            }
        }

       //List<RequirementItemContract> IRequirementVerificationQueueView.lstRequirementItems { set => CurrentViewContext.lstRequirementItems = value; }
       

        #endregion

        #endregion

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

        /// <summary>
        /// Page_Load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    BindTenant();
                    CaptureQuerystringParameters();
                    BindControls();
                    GetSessionValues();
                    ShowHideControls();
                    //UAT-4461
                    if (!Session["NextPrevNavigationData"].IsNullOrEmpty())
                        Session["NextPrevNavigationData"] = null;
                    if (!Session["IsCurrentARIDRecordAlreadySaved"].IsNullOrEmpty())
                        Session["IsCurrentARIDRecordAlreadySaved"] = null;
                    if (!Session["ApplicantRequirementItemDataId"].IsNullOrEmpty())
                        Session["ApplicantRequirementItemDataId"] = null;
                    //End UAT
                }

                //string parameter = Request["__EVENTARGUMENT"];
                //if (parameter == "LoadCategories")
                //    cmbRequirementPackageType_SelectedIndexChanged(sender, null);

                if (IsRotationPackageVerificationQueue)
                {
                    header.InnerText = "Rotation Package Verification Queue";
                    base.SetPageTitle("Rotation Package Verification Queue");
                    base.Title = "Rotation Package Verification Queue";
                }
                else
                {
                    header.InnerText = "Requirement Verification queue";
                    base.SetPageTitle("Requirement Verification queue");
                    base.Title = "Requirement Verification queue";
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

        #region Grid Events

        /// <summary>
        /// Grid NeedDataSource event to bind grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdRequirementVerificationQueue_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                //Note:-Submission Date in Rotation Verification Queue is wrong.So, We may have ticket in future to fix - submission date issue then save and next and save and previous navigation should be test and updated as well.
                GridCustomPaging.CurrentPageIndex = CurrentPageIndex;
                GridCustomPaging.PageSize = PageSize;
                //CurrentViewContext.GridCustomPaging = GridCustomPaging;
                Presenter.PerformSearch();
                grdRequirementVerificationQueue.DataSource = CurrentViewContext.ApplicantSearchData;

                if (CurrentViewContext.ApplicantSearchData.Count > AppConsts.NONE)
                {
                    List<RFQSelectedDataContract> _lstSelectedData = new List<RFQSelectedDataContract>();
                    CurrentViewContext.ApplicantSearchData.ForEach(rvqc => _lstSelectedData.Add(new RFQSelectedDataContract
                    {
                        RPSId = rvqc.RequirementPackageSubscriptionID,
                        OrganizationUserId = rvqc.OrganizationUserID,
                        RotationId = rvqc.ClinicalRotationID
                    }));
                    SysXWebSiteUtils.SessionService.SetCustomData("QueueData", _lstSelectedData);
                }
                else
                {
                    Session.Remove("QueueData");
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
        /// Grid ItemCommand event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdRequirementVerificationQueue_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "ViewDetail")
                {
                    SetSessionValues();
                    Dictionary<String, String> queryString = new Dictionary<String, String>();
                    String rotationID = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ClinicalRotationID"].ToString();
                    String ReqPkgSubscriptionId = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["RequirementPackageSubscriptionID"].ToString();
                    String RequirementItemId = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["RequirementItemId"].ToString();
                    String ApplicantRequirementItemId = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ApplicantRequirementItemId"].ToString();
                    String organizationUserID = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrganizationUserID"].ToString();
                    String RequirementPackageTypeId = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["RequirementPackageTypeID"].ToString();

                    Int32 _selectedReqCategoryId = Convert.ToInt32((e.Item.FindControl("hdfReqCatId") as HiddenField).Value);
                    Int32 _selectedReqPackageSubscriptionId = Convert.ToInt32((e.Item.FindControl("hdfReqPackSubscriptionId") as HiddenField).Value);

                    queryString = new Dictionary<String, String>
                                                                 {
                                                                    { ProfileSharingQryString.SelectedTenantId, Convert.ToString(CurrentViewContext.SelectedTenantId) },
                                                                    // { "Child",  AppConsts.REQUIREMENT_VERIFICATION_DETAIL_CONTROL},
                                                                    { "Child",  AppConsts.REQUIREMENT_VERIFICATION_DETAIL__NEW_CONTROL},
                                                                    { ProfileSharingQryString.ReqPkgSubscriptionId, ReqPkgSubscriptionId },
                                                                    { ProfileSharingQryString.RotationId, rotationID },
                                                                    { ProfileSharingQryString.ApplicantId , organizationUserID },
                                                                    { "PackageTypeId" , RequirementPackageTypeId},
                                                                    {"RequirementItemId", RequirementItemId },
                                                                    {"ApplicantRequirementItemId", ApplicantRequirementItemId },
                                                                    { ProfileSharingQryString.ControlUseType,AppConsts.ROTATION_VERIFICATION_QUEUE},
                                                                    {"SelectedReqComplianceCategoryId",Convert.ToString( _selectedReqCategoryId)},
                                                                    {"SelectedReqPackageSubscriptionId",Convert.ToString( _selectedReqPackageSubscriptionId)}
                                                                 };
                    string url = String.Format("~/ClinicalRotation/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                    Response.Redirect(url, true);
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
        ///  Grid SortCommand event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdRequirementVerificationQueue_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
        {
            try
            {
                if (e.NewSortOrder != GridSortOrder.None)
                {
                    ViewState["SortExpression"] = e.SortExpression;
                    ViewState["SortDirection"] = e.NewSortOrder.Equals(GridSortOrder.Descending);
                    CurrentViewContext.GridCustomPaging.SortExpression = e.SortExpression;
                    CurrentViewContext.GridCustomPaging.SortDirectionDescending = e.NewSortOrder.Equals(GridSortOrder.Descending);
                }
                else
                {
                    ViewState["SortExpression"] = String.Empty;
                    ViewState["SortDirection"] = false;
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

        #endregion

        #region Button Events

        /// <summary>
        /// Search click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucCmdBarButton_SearchClick(object sender, EventArgs e)
        {
            try
            {
                if(CategoryId.IsNullOrEmpty() && !ReqCategoryId.IsNullOrEmpty() && (ddlCategoryLabel.SelectedIndex > 0 && !ddlCategoryLabel.SelectedItem.Text.IsNullOrWhiteSpace())) //UAT-4705
                {
                    CategoryId = ddlCategoryLabel.SelectedValue;
                }
                if(ReqItemId.IsNullOrEmpty() && (ddlItemLabel.SelectedIndex > 0 && !ddlItemLabel.SelectedItem.Text.IsNullOrWhiteSpace()))
                {
                    ReqItemId = ddlItemLabel.SelectedValue;
                }
                CurrentViewContext.IsSearchClicked = true;
                //To reset grid filters 
                ResetReqVerificationQueueGridFilters();
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
        /// Reset search filters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucCmdBarButton_ResetClick(object sender, EventArgs e)
        {
            try
            {
                CurrentViewContext.IsSearchClicked = false;
                ResetControls();
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
        /// Cancel click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucCmdBarButton_CancelClick(object sender, EventArgs e)
        {
            try
            {
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

        #endregion

        #region Dropdown Events

        protected void ddlTenantName_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                CurrentViewContext.IsSearchClicked = false;
                ResetControls(true);
                BindControls();
                ShowHideControls();
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
        /// Tenant dropdown DataBound event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTenantName_DataBound(object sender, EventArgs e)
        {
            try
            {
                ddlTenantName.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT--"));
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
        /// Agency dropdown DataBound event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbAgency_DataBound(object sender, EventArgs e)
        {
            try
            {
                cmbAgency.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT--"));
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

        protected void ddlCategoryLabel_DataBound(object sender, EventArgs e)
        {
            try
            {
                ddlCategoryLabel.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT--"));
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

        protected void ddlItemLabel_DataBound(object sender, EventArgs e)
        {
            try
            {
                ddlItemLabel.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT--"));
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

        #region Private Methods

        /// <summary>
        /// Bind tenant dropdown
        /// </summary>
        private void BindTenant()
        {
            Presenter.GetTenants();
            ddlTenantName.DataSource = CurrentViewContext.lstTenant;
            ddlTenantName.DataBind();
            Presenter.IsAdminLoggedIn();
            if (CurrentViewContext.IsAdminLoggedIn)
            {
                ddlTenantName.Enabled = true;
                CurrentViewContext.SelectedTenantId = 0;
            }
            else
            {
                CurrentViewContext.SelectedTenantId = CurrentViewContext.TenantId;
            }
        }

        //UAT-4014
        private void BindUserTypes()
        {
            Presenter.GetUserType();
            ddlUserType.DataSource = CurrentViewContext.dicUserTypes;
            ddlUserType.DataBind();
        }


        /// <summary>
        /// To bind controls
        /// </summary>
        private void BindControls()
        {
            BindAgency();
            BindRequirementPackage();
            BindRequirementPackageTypes();
            BindUserTypes(); //UAT-4014
            BindCategories();
            BindCategoryItemLabel();
        }

        private void ShowHideControls()
        {
            if (IsRotationPackageVerificationQueue)
            {
                grdRequirementVerificationQueue.Columns.FindByUniqueName("RequirementPackageName").Visible = true;
                divRequirementPackageType.Visible = false;
            }
            else
            {
                grdRequirementVerificationQueue.Columns.FindByUniqueName("RequirementPackageName").Visible = false;
                divPackage.Visible = false;
            }

            if (CurrentViewContext.IsAdminLoggedIn)
                grdRequirementVerificationQueue.Columns.FindByUniqueName("ReqReviewByDesc").Visible = true;
            else
                grdRequirementVerificationQueue.Columns.FindByUniqueName("ReqReviewByDesc").Visible = false;
        }

        /// <summary>
        /// Method to bind Agencies
        /// </summary>
        private void BindAgency()
        {
            Presenter.GetAllAgency();
            cmbAgency.DataSource = CurrentViewContext.lstAgency;
            cmbAgency.DataBind();
        }

        private void BindCategorylabel()
        {
            Presenter.GetRequirementCategory();
        }

        private void BindCategoryItemLabel()
        {
            Presenter.GetRequirementItem();
        }

        /// <summary>
        /// Method to bind Requirement Package Types
        /// </summary>
        private void BindRequirementPackageTypes()
        {
            Presenter.GetRequirementPackageTypes();

            //Commented below code regarding [UAT-2197:Requirement Verification Queue: "Requirement Package Type" should not be required]
            //CurrentViewContext.lstRequirementPackageType.Insert(AppConsts.NONE, new RequirementPackageTypeContract
            //{
            //    Name = AppConsts.COMBOBOX_ITEM_SELECT,
            //    ID = AppConsts.NONE
            //});

            cmbRequirementPackageType.DataSource = CurrentViewContext.lstRequirementPackageType;
            cmbRequirementPackageType.DataBind();

            //UAT-2197:Requirement Verification Queue: "Requirement Package Type" should not be required
            cmbRequirementPackageType.Items.ForEach(itm =>
            {
                itm.Checked = true;
            });
        }

        /// <summary>
        /// Method to bind Requirement Package Types
        /// </summary>
        private void BindRequirementPackage()
        {
            Presenter.GetRequirementPackages();
            cmbPackage.DataSource = CurrentViewContext.LstRequirementPackage;
            cmbPackage.DataBind();
        }


        /// <summary>
        /// Removes all the filters and Sorting on the grid and clears the variables.
        /// </summary>
        private void ResetReqVerificationQueueGridFilters()
        {
            grdRequirementVerificationQueue.MasterTableView.SortExpressions.Clear();
            grdRequirementVerificationQueue.CurrentPageIndex = 0;
            grdRequirementVerificationQueue.MasterTableView.CurrentPageIndex = 0;
            grdRequirementVerificationQueue.Rebind();
        }

        /// <summary>
        /// To reset search controls
        /// </summary>
        private void ResetControls(Boolean IsTenantChanged = false)
        {
            if (!IsTenantChanged)
            {
                Presenter.IsAdminLoggedIn();
                if (CurrentViewContext.IsAdminLoggedIn)
                {
                    CurrentViewContext.SelectedTenantId = AppConsts.NONE;
                    if (!IsRotationPackageVerificationQueue)
                    {
                        divPackage.Visible = false;
                        cmbRequirementPackageType.Items.Clear();
                        cmbRequirementPackageType.Items.Add(new RadComboBoxItem
                        {
                            Text = AppConsts.COMBOBOX_ITEM_SELECT,
                            Value = AppConsts.ZERO
                        });
                    }
                    else
                    {
                        divRequirementPackageType.Visible = false;
                    }
                }
                else
                {
                    if (!IsRotationPackageVerificationQueue)
                    {
                        divPackage.Visible = false;
                        cmbRequirementPackageType.SelectedValue = AppConsts.ZERO;
                        CurrentViewContext.SelectedTenantId = CurrentViewContext.TenantId;
                    }
                    else
                    {
                        divRequirementPackageType.Visible = false;
                    }
                }
            }
            CurrentViewContext.VirtualRecordCount = 0;
            CurrentViewContext.ApplicantFirstName = String.Empty;
            CurrentViewContext.ApplicantLastName = String.Empty;
            CurrentViewContext.RotationStartDate = null;
            CurrentViewContext.RotationEndDate = null;
            CurrentViewContext.SubmissionDate = null;
            CurrentViewContext.IsCurrentRotation = false;
            CurrentViewContext.SelectedAgencyID = -1;
            ReqCategoryId = string.Empty; //UAT-4705
            CategoryId = string.Empty;
            ReqItemId = string.Empty;
            ResetReqVerificationQueueGridFilters();
            ddlUserType.ClearCheckedItems();  //UAT-4014
            LstRequirementCategory = new List<RequirementCategoryContract>();
            LstRequirementItems = new List<RequirementItemContract>();
        }

        /// <summary>
        /// To set controls values in session
        /// </summary>
        private void SetSessionValues()
        {
            SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.REQ_VERIFICATION_QUEUE_SESSION_KEY, null);
            RequirementVerificationQueueContract searchDataContract = new RequirementVerificationQueueContract();
            searchDataContract.IsBackToSearch = CurrentViewContext.IsSearchClicked;
            searchDataContract.ApplicantFirstName = CurrentViewContext.ApplicantFirstName;
            searchDataContract.ApplicantLastName = CurrentViewContext.ApplicantLastName;
            searchDataContract.RotationStartDate = CurrentViewContext.RotationStartDate;
            searchDataContract.RotationEndDate = CurrentViewContext.RotationEndDate;
            searchDataContract.SubmissionDate = CurrentViewContext.SubmissionDate;
            searchDataContract.IsCurrentRotation = CurrentViewContext.IsCurrentRotation;
            searchDataContract.AgencyID = CurrentViewContext.SelectedAgencyID;
            searchDataContract.TenantID = CurrentViewContext.SelectedTenantId;
            //searchDataContract.RequirementPackageTypeID = CurrentViewContext.RequirementPackageTypeID;
            //UAT-2197:Requirement Verification Queue: "Requirement Package Type" should not be required
            searchDataContract.SelectedRequirementPackageTypes = CurrentViewContext.RequirementPackageTypes;
            searchDataContract.RequirementPackageID = CurrentViewContext.SelectedPackageID;
            searchDataContract.LoggedInUserId = CurrentViewContext.CurrentLoggedInUserId;
            searchDataContract.IsRotationPackageVerificationQueue = CurrentViewContext.IsRotationPackageVerificationQueue;
            //searchDataContract.LoggedInUserId = CurrentViewContext.CurrentLoggedInUserId;
            searchDataContract.GridCustomPagingArguments = CurrentViewContext.GridCustomPaging;

            searchDataContract.ReqCategoryLabel = CurrentViewContext.CategoryId;
            searchDataContract.ReqItemLabel = CurrentViewContext.ReqItemId;
            //Session for maintaining control values
            SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.REQ_VERIFICATION_QUEUE_SESSION_KEY, searchDataContract);
        }

        /// <summary>
        /// To get session values for controls
        /// </summary>
        private void GetSessionValues()
        {
            RequirementVerificationQueueContract searchDataContract = new RequirementVerificationQueueContract();
            if (Session[AppConsts.REQ_VERIFICATION_QUEUE_SESSION_KEY].IsNotNull())
            {
                searchDataContract = SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.REQ_VERIFICATION_QUEUE_SESSION_KEY) as RequirementVerificationQueueContract;
                CurrentViewContext.IsSearchClicked = searchDataContract.IsBackToSearch;
                CurrentViewContext.ApplicantFirstName = searchDataContract.ApplicantFirstName;
                CurrentViewContext.ApplicantLastName = searchDataContract.ApplicantLastName;
                CurrentViewContext.RotationStartDate = searchDataContract.RotationStartDate;
                CurrentViewContext.RotationEndDate = searchDataContract.RotationEndDate;
                CurrentViewContext.SubmissionDate = searchDataContract.SubmissionDate;
                CurrentViewContext.IsCurrentRotation = searchDataContract.IsCurrentRotation;
                CurrentViewContext.SelectedAgencyID = searchDataContract.AgencyID ?? 0;
                CurrentViewContext.RequirementPackageTypes = searchDataContract.SelectedRequirementPackageTypes;
                CurrentViewContext.GridCustomPaging = searchDataContract.GridCustomPagingArguments;
                CurrentViewContext.IsRotationPackageVerificationQueue = searchDataContract.IsRotationPackageVerificationQueue;
                CurrentViewContext.SelectedPackageID = searchDataContract.RequirementPackageID;
                CurrentViewContext.CategoryId = searchDataContract.ReqCategoryLabel;
                ddlCategoryLabel_SelectedIndexChanged(null,null);
                CurrentViewContext.ReqItemId = searchDataContract.ReqItemLabel;

                //Rebind grids
                grdRequirementVerificationQueue.Rebind();
                //Reset session
                Session[AppConsts.REQ_VERIFICATION_QUEUE_SESSION_KEY] = null;
            }
        }

        /// <summary>
        /// capture query string parameters
        /// </summary>
        private void CaptureQuerystringParameters()
        {
            Dictionary<String, String> args = new Dictionary<String, String>();
            if (!Request.QueryString["args"].IsNull())
            {
                args.ToDecryptedQueryString(Request.QueryString["args"]);

                if (args.ContainsKey(ProfileSharingQryString.SelectedTenantId))
                {
                    CurrentViewContext.SelectedTenantId = Convert.ToInt32(args[ProfileSharingQryString.SelectedTenantId]);
                }
            }
            //if user navigate to other feature from detail screen and return to manage rotation again.
            else
                Session[AppConsts.REQ_VERIFICATION_QUEUE_SESSION_KEY] = null;
        }

        #endregion

        protected void BindCategories()
        {
            //ReqCategoryId = 0;
            CategoryId = string.Empty;
            BindCategorylabel();
            ReqItemId = string.Empty;
        }

        protected void ddlCategoryLabel_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            ReqItemId = string.Empty;
            // ReqCategoryId = 0;
            if (ddlCategoryLabel.SelectedIndex >= 0 && !ddlCategoryLabel.SelectedValue.IsNullOrWhiteSpace())
            {
                ReqCategoryId = ddlCategoryLabel.SelectedValue; //UAT-4705
                BindCategoryItemLabel();
            }
            else
                LstRequirementItems = new List<RequirementItemContract>();
        }
    }
}