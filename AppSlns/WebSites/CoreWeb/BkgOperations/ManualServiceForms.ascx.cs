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
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Serialization;
using Telerik.Web.UI;

#endregion

namespace CoreWeb.BkgOperations.Views
{
    public partial class ManualServiceForms : BaseUserControl, IManualServiceFormsView
    {
        #region Private Variables

        private ManualServiceFormsPresenter _presenter = new ManualServiceFormsPresenter();
        private String _viewType;
        private ManualServiceFormsSearchContract _gridSearchContract = null;
        private Int32 _tenantid;
        private CustomPagingArgsContract _gridCustomPaging = null;

        #endregion


        #region Properties

        #region Public Properties

        /// <summary>
        /// returns the object of type IOrderQueueView.
        /// </summary>
        public IManualServiceFormsView CurrentViewContext
        {
            get { return this; }
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

        public List<BackroundServicesContract> lstBackroundServicesContract
        {
            get;
            set;
        }

        /// <summary>
        /// Gets and set List of ServiceFormStatus
        /// </summary>
        public List<Entity.ClientEntity.lkpServiceFormStatu> ListServiceFormStatus
        {
            set;
            get;
        }
        public ManualServiceFormContract lstManualServiceFormSearchContract
        {
            get;
            set;
        }

        //public Int32? DeptProgramMappingID
        //{
        //    get
        //    {
        //        if (!hdnDepartmntPrgrmMppng.Value.IsNullOrEmpty())
        //        {
        //            return Convert.ToInt32(hdnDepartmntPrgrmMppng.Value);
        //        }
        //        return null;
        //    }
        //}

        public ManualServiceFormsSearchContract SetManualServiceFormsSearchContract
        {
            set
            {
                if (value.IsNotNull())
                    value.NodeLabel = lblinstituteHierarchy.Text.HtmlDecode();
                var serializer = new XmlSerializer(typeof(ManualServiceFormsSearchContract));
                var sb = new StringBuilder();
                using (TextWriter writer = new StringWriter(sb))
                {
                    serializer.Serialize(writer, value);
                }
                //Session[AppConsts.MANUAL_SERVICE_FORMS_OBJECT_SESSION_KEY] = sb.ToString();
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

        public List<ManualServiceFormContract> lstManualServiceForm
        {
            get;
            set;
        }

        public Int32 SelectedServiceFormStatusId
        {
            get;
            set;
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

        /// <summary>
        /// Gets or sets the Error Message
        /// </summary>
        public String ErrorMessage
        {
            get;
            set;
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

        /// <summary>
        /// CurrentPageIndex</summary>
        /// <value>
        /// Gets or sets the value for CurrentPageIndex.</value>
        public Int32 CurrentPageIndex
        {
            get
            {
                return grdManualServiceForms.MasterTableView.CurrentPageIndex + 1;
            }
            set
            {
                if (grdManualServiceForms.MasterTableView.CurrentPageIndex > 0)
                {
                    grdManualServiceForms.MasterTableView.CurrentPageIndex = value - 1;
                }
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
                //return grdManualServiceForms.PageSize > 100 ? 100 : grdManualServiceForms.PageSize;
                return grdManualServiceForms.PageSize;
            }
            set
            {
                //grdManualServiceForms.PageSize = value;
            }
        }

        ///// <summary>
        ///// VirtualPageCount</summary>
        ///// <value>
        ///// Sets the value for VirtualPageCount.</value>
        public Int32 VirtualPageCount
        {
            set
            {
                grdManualServiceForms.VirtualItemCount = value;
                grdManualServiceForms.MasterTableView.VirtualItemCount = value;
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

        public String SelectedDeptProgramMappingID
        {
            get
            {
                if (!hdnDepartmntPrgrmMppng.Value.IsNullOrEmpty())
                {
                    return hdnDepartmntPrgrmMppng.Value;
                }
                return null;
            }
        }

        #endregion

        #region Private Properties

        private ManualServiceFormsPresenter Presenter
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

        #endregion
        #endregion





        #region Events

        #region Page Events

        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                base.Title = "Manual Service Forms";
                base.SetPageTitle("Manual Service Forms");
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
                if (!this.IsPostBack)
                {
                    Presenter.OnViewInitialized();
                    grdManualServiceForms.Visible = false;
                    if (IsAdminUser)
                    {
                        SelectedTenantId = 0;
                        ddlTenantName.Enabled = true;
                    }
                    else
                    {
                        SelectedTenantId = TenantId;
                        ddlTenantName.Enabled = false;
                    }
                    BindDropDownControl();
                    ApplyActionLevelPermission(ActionCollection, "Manual Service Forms");
                }
                if (divTenant.Visible && !ddlTenantName.SelectedValue.IsNullOrEmpty())
                {
                    SelectedTenantId = Convert.ToInt32(ddlTenantName.SelectedValue);
                }
                if (!IsAdminUser && (SelectedTenantId.IsNull() || SelectedTenantId == AppConsts.NONE))
                {
                    SelectedTenantId = TenantId;
                }
                lblinstituteHierarchy.Text = hdnHierarchyLabel.Value.HtmlEncode();
                Presenter.OnViewLoaded();
                hfTenantId.Value = SelectedTenantId.ToString();
                (fsucOrderCmdBar as CoreWeb.Shell.Views.CommandBar).SaveButton.ToolTip = "Click to search orders per the criteria entered above";
                (fsucOrderCmdBar as CoreWeb.Shell.Views.CommandBar).SubmitButton.ToolTip = "Click to remove all values entered in the search criteria above";
                (fsucOrderCmdBar as CoreWeb.Shell.Views.CommandBar).CancelButton.ToolTip = "Click to cancel. Any data entered will not be saved";
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
                hdnDepartmntPrgrmMppng.Value = String.Empty;
                hdnHierarchyLabel.Value = String.Empty;
                lblinstituteHierarchy.Text = String.Empty;
                BindDropDownControl();
            }
            else
            {
                ResetPageControls();
            }
        }

        protected void ddlTenantName_DataBound(object sender, EventArgs e)
        {
            ddlTenantName.Items.Insert(0, new RadComboBoxItem("--Select--"));
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
            grdManualServiceForms.Visible = true;
            ResetGridFilters();
        }

        /// <summary>
        /// Resets all the search controls and displays the records in the order queue with deafult checkbox selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarReset_Click(object sender, EventArgs e)
        {
            ResetPageControls();
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

        #region Grid Related Events

        /// <summary>
        /// Retrieves a list of Applicant Compliance Item Data.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        /// <remarks></remarks>
        protected void grdManualServiceForms_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                lstManualServiceForm = new List<ManualServiceFormContract>();
                GridCustomPaging.CurrentPageIndex = CurrentPageIndex;
                GridCustomPaging.PageSize = PageSize;
                Presenter.GetManualServiceFormsData();
                grdManualServiceForms.DataSource = lstManualServiceForm;
                //grdManualServiceForms.DataSource = String.Empty;
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
        protected void grdManualServiceForms_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                #region Export functionality
                if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToPdfCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToWordCommandName
      || e.CommandName == Telerik.Web.UI.RadGrid.ExportToExcelCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToCsvCommandName)
                {
                    base.ConfigureExport(grdManualServiceForms);
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

        /// <summary>
        /// Grid sort expression
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdManualServiceForms_SortCommand(object sender, GridSortCommandEventArgs e)
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
        protected void grdManualServiceForms_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
                {

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

        protected void grdManualServiceForms_ItemCreated(object sender, GridItemEventArgs e)
        {
            try
            {
                if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
                {
                    GridEditFormItem editform = (e.Item as GridEditFormItem);
                    WclComboBox cmbSvcFrmStatus = editform.FindControl("cmbSvcFrmStatus") as WclComboBox;
                    Presenter.GetServiceFormStatusList();
                    cmbSvcFrmStatus.DataSource = CurrentViewContext.ListServiceFormStatus;
                    cmbSvcFrmStatus.DataBind();
                    if (!(e.Item.DataItem is GridInsertionObject))
                    {
                        ManualServiceFormContract manualServiceFormContract = e.Item.DataItem as ManualServiceFormContract;

                        if (manualServiceFormContract != null)
                        {
                            CurrentViewContext.SelectedServiceFormStatusId = manualServiceFormContract.SFStatusId;
                            if (!CurrentViewContext.SelectedServiceFormStatusId.IsNullOrEmpty())
                            {
                                cmbSvcFrmStatus.SelectedValue = CurrentViewContext.SelectedServiceFormStatusId.ToString();
                            }
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


        protected void grdManualServiceForms_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                Int32 orderServiceFormId = Convert.ToInt32(gridEditableItem.GetDataKeyValue("OrderServiceFormId"));
                Int32 notificationId = Convert.ToInt32(gridEditableItem.GetDataKeyValue("NotificationId"));
                Int32 oldStatusId = Convert.ToInt32(gridEditableItem.GetDataKeyValue("SFStatusId"));
                CurrentViewContext.SelectedServiceFormStatusId = Convert.ToInt32((e.Item.FindControl("cmbSvcFrmStatus") as WclComboBox).SelectedValue);
                //Boolean isUpdated = Presenter.UpdateOrderServiceServiceFormStatus(orderServiceFormId);
                Boolean isUpdated = Presenter.UpdateOrderServiceServiceFormStatus(notificationId, oldStatusId);
                if (isUpdated)
                {
                    //UAT-2002: New Student notification and comm copy setting to confirm we received manual service form.
                    GridEditFormItem item = (GridEditFormItem)e.Item;
                    ManualServiceFormContract manualServiceFormContract = new ManualServiceFormContract();
                    manualServiceFormContract.HierarchyNodeID = Convert.ToInt32(gridEditableItem.GetDataKeyValue("HierarchyNodeID"));
                    manualServiceFormContract.OrganizationUserId = Convert.ToInt32(gridEditableItem.GetDataKeyValue("OrganizationUserId"));
                    manualServiceFormContract.OrderNumber = Convert.ToString(gridEditableItem.GetDataKeyValue("OrderNumber"));
                    manualServiceFormContract.ApplicantFirstName = item["ApplicantFirstName"].Text.Trim();
                    manualServiceFormContract.ApplicantLastName = item["ApplicantLastName"].Text.Trim();
                    manualServiceFormContract.ApplicantEmailAddress = item["ApplicantEmailAddress"].Text.Trim();
                    //UAT-2156 :New Notification for students with Comm Copy setting for Form Dispatched (Manual Service Forms).
                    manualServiceFormContract.ServiceName = Convert.ToString(gridEditableItem.GetDataKeyValue("ServiceName"));
                    manualServiceFormContract.SFName = Convert.ToString(gridEditableItem.GetDataKeyValue("SFName"));
                    manualServiceFormContract.PackageName = Convert.ToString(gridEditableItem.GetDataKeyValue("PackageName"));
                    //UAT-2671
                    manualServiceFormContract.ServiceGroupName = Convert.ToString(gridEditableItem.GetDataKeyValue("ServiceGroupName"));

                    Presenter.SendSvcFormStsChangeNotification(oldStatusId, manualServiceFormContract);

                    e.Canceled = false;
                    base.ShowSuccessMessage("Service Form Status updated successfully.");
                }
                else
                {
                    e.Canceled = true;
                    base.ShowErrorMessage("Some error occured, Service Form Status not updated successfully"); ;
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }


        #endregion

        #region Public Events

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
                Presenter.GetBackroundServiceList();
                Presenter.GetServiceFormStatusList();
                cmbFormStatus.DataSource = CurrentViewContext.ListServiceFormStatus.OrderBy(col => col.SFS_ID);
                cmbFormStatus.DataBind();
            }
        }

        /// <summary>
        /// Removes all the filters and Sorting on the grid and clears the variables.
        /// </summary>
        private void ResetGridFilters()
        {
            grdManualServiceForms.MasterTableView.FilterExpression = null;
            grdManualServiceForms.MasterTableView.SortExpressions.Clear();
            grdManualServiceForms.CurrentPageIndex = 0;
            grdManualServiceForms.MasterTableView.CurrentPageIndex = 0;
            grdManualServiceForms.Rebind();
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
            VirtualPageCount = AppConsts.NONE;
            lblinstituteHierarchy.Text = String.Empty;
            hdnDepartmntPrgrmMppng.Value = String.Empty;
            hdnHierarchyLabel.Value = String.Empty;
            #endregion
            if (IsAdminUser)
            {
                ddlTenantName.SelectedIndex = AppConsts.NONE;
                hfTenantId.Value = String.Empty;
                lstBackroundServices = new List<Entity.ClientEntity.BackgroundService>();
                cmbFormStatus.DataSource = new List<lkpServiceFormStatu>();
                cmbFormStatus.DataBind();
                cmbFormStatus.Items.Insert(0, new RadComboBoxItem("--Select--"));
            }
            cmbFormStatus.SelectedIndex = AppConsts.NONE;
            chkServices.SelectedIndex = AppConsts.NONE;
            ResetGridFilters();
            //Session[AppConsts.MANUAL_SERVICE_FORMS_OBJECT_SESSION_KEY] = null;
        }

        #endregion


        #region Public Method

        #endregion

        #endregion

        #region Apply Permissions

        public override List<ClsFeatureAction> ActionCollection
        {
            get
            {
                List<ClsFeatureAction> actionCollection = new List<ClsFeatureAction>();
                actionCollection.Add(new Entity.ClientEntity.ClsFeatureAction
                {
                    CustomActionId = "Edit",
                    CustomActionLabel = "Edit",
                    ScreenName = "Manual Service Forms"
                });
                return actionCollection;
            }
        }

        protected override void ApplyActionLevelPermission(List<ClsFeatureAction> ctrlCollection, string screenName = "")
        {
            base.ApplyActionLevelPermission(ctrlCollection, screenName);
            List<Entity.FeatureRoleAction> permission = base.ActionPermission;
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
                            if (x.FeatureAction.CustomActionId == "Edit")
                            {
                                grdManualServiceForms.MasterTableView.GetColumn("EditCommandColumn").Display = false;
                            }
                            break;
                        }
                }

            }
                );
        }

        #endregion



    }
}