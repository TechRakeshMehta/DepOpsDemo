using System;
using Microsoft.Practices.ObjectBuilder;
using Entity;
using System.Collections.Generic;
using Telerik.Web.UI;
using INTSOF.Utils;
using INTERSOFT.WEB.UI.WebControls;
using System.Web.UI.WebControls;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using INTSOF.UI.Contract.BkgSetup;
using System.Linq;

namespace CoreWeb.BkgSetup.Views
{
    public partial class ManageService : BaseUserControl, IManageServiceView
    {

        #region Variables
        #region public Variables
        #endregion

        #region private Variables
        private ManageServicePresenter _presenter = new ManageServicePresenter();
        private String _viewType;
        private ManageServiceContract _viewContract;
        private Int32 _tenantid;
        private Boolean? _isAdminLoggedIn = null;
        private Int32 _selectedTenantId = AppConsts.NONE;
        #endregion

        #endregion

        #region Properties

        #region Private Properties

        #endregion
        #region Public Properties
        public ManageServicePresenter Presenter
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
        /// Gets and sets Logged In User TenantId
        /// </summary>
        public Int32 TenantId
        {
            get
            {
                if (_tenantid == 0)
                {
                    _tenantid = Presenter.GetTenantId();
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

        public List<Tenant> ListTenants
        {
            set;
            get;
        }

        //public Int32 SelectedTenantId
        //{
        //    get
        //    {
        //        //if (_selectedTenantId == AppConsts.NONE)
        //        //{
        //        //    Int32.TryParse(ddlTenant.SelectedValue, out _selectedTenantId);

        //        //    if (_selectedTenantId == AppConsts.NONE)
        //        //        _selectedTenantId = TenantId;
        //        //}
        //        //return _selectedTenantId;
        //    }
        //    set
        //    {
        //        //_selectedTenantId = value;
        //    }
        //}

        /// <summary>
        /// Gets the default TenantId
        /// </summary>
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

        public Boolean IsAdminLoggedIn
        {
            get
            {
                if (_isAdminLoggedIn.IsNull())
                {
                    //Presenter.IsAdminLoggedIn();
                }
                return _isAdminLoggedIn.HasValue ? _isAdminLoggedIn.Value : true;
            }
            set
            {
                _isAdminLoggedIn = value;
            }
        }

        /// <summary>
        /// Returns the current logged-in user ID.
        /// </summary>
        Int32 IManageServiceView.CurrentLoggedInUserId
        {
            get
            {
                return base.CurrentUserId;
            }
        }
        /// <summary>
        /// List of all background service
        /// </summary>
        List<BackgroundService> IManageServiceView.MasterServiceList
        {
            get;
            set;

        }

        public IManageServiceView CurrentViewContext
        {
            get { return this; }
        }

        ManageServiceContract IManageServiceView.ViewContract
        {
            get
            {
                if (_viewContract.IsNull())
                {
                    _viewContract = new ManageServiceContract();
                }

                return _viewContract;
            }

        }
        public List<lkpBkgSvcType> BkgServiceTypeList
        {
            get;
            set;
        }

        public string ErrorMessage
        {
            get;
            set;
        }

        public string InfoMessage
        {
            get;
            set;
        }

        //UAT-1728: Create ability to add cofigurable text to the result report (and flagged only and service group reports) by service. 
        String IManageServiceView.ConfigurableServiceText
        {
            get;
            set;
        }
        #region Derived From Services
        List<BackgroundService> IManageServiceView.BkgDerivedFromServiceList
        {
            get;
            set;
        }
        #endregion
        #endregion
        #endregion


        #region Events
        #region Page Events


        protected override void OnInit(EventArgs e)
        {
            try
            {
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                //Dictionary<String, String> args = new Dictionary<String, String>();

                //if (!Request.QueryString["args"].IsNull())
                //{
                //    args.ToDecryptedQueryString(Request.QueryString["args"]);
                //}
                base.Title = "Manage Services";
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

            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
                ApplyActionLevelPermission(ActionCollection, "Manage Service");
                //BindTenant();
            }
            //ActionPermission();
            base.SetPageTitle("Manage Service");
        }
        #endregion

        #region DropDown Events

        /// <summary>
        /// Binds the categories as per selected client.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        //protected void ddlTenant_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        grdManageService.CurrentPageIndex = 0;
        //        grdManageService.MasterTableView.SortExpressions.Clear();
        //        grdManageService.MasterTableView.FilterExpression = null;

        //        foreach (GridColumn column in grdManageService.MasterTableView.OwnerGrid.Columns)
        //        {
        //            column.CurrentFilterFunction = GridKnownFunction.NoFilter;
        //            column.CurrentFilterValue = string.Empty;
        //        }
        //        SelectedTenantId = Convert.ToInt32(ddlTenant.SelectedValue);
        //        dvServiceGroup.Visible = true;
        //        grdManageService.Rebind();
        //    }
        //    catch (SysXException ex)
        //    {
        //        base.LogError(ex);
        //        base.ShowErrorMessage(ex.Message);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        base.LogError(ex);
        //        base.ShowErrorMessage(ex.Message);
        //    }
        //}


        #endregion

        #region grid Events
        protected void grdManageService_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetMasterServices();
                grdManageService.DataSource = CurrentViewContext.MasterServiceList;
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

        protected void grdManageService_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                //Checks if item is GridDataItem type.
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;

                    HiddenField hdnfIsCreatedByAdmin = e.Item.FindControl("hdnfIsEditable") as HiddenField;
                    if (hdnfIsCreatedByAdmin.Value == "False")
                    {
                        //master predefined Service shouldn't be edited or deleted
                        dataItem["DeleteColumn"].Controls[0].Visible = false;
                        dataItem["EditCommandColumn"].Controls[0].Visible = false;
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
        protected void grdManageService_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                dvServiceGroup.Visible = true;
                if (e.CommandName == "PerformInsert")
                {
                    Int32 parentServiceId = Convert.ToInt32((e.Item.FindControl("ddlDerivedFromServices") as WclComboBox).SelectedValue);
                    CurrentViewContext.ViewContract.ServiceName = (e.Item.FindControl("txtServiceName") as WclTextBox).Text.Trim();

                    //UAT-1728:Create ability to add cofigurable text to the result report (and flagged only and service group reports) by service. 
                    CurrentViewContext.ViewContract.ConfigurableServiceText = (e.Item.FindControl("txtConfServiceText") as WclTextBox).Text.Trim();

                    CurrentViewContext.ViewContract.ServiceDesc = (e.Item.FindControl("txtSvcDescription") as WclTextBox).Text.Trim();
                    WclButton chkPackageCount = e.Item.FindControl("chkPackageCount") as WclButton;
                   // WclButton chkYear = e.Item.FindControl("chkYear") as WclButton;
                    WclButton chkMinOccurences = e.Item.FindControl("chkMinOccurences") as WclButton;
                    WclButton chkMaxOccurences = e.Item.FindControl("chkMaxOccurences") as WclButton;
                    WclButton chkSendDoc = e.Item.FindControl("chkSendDoc") as WclButton;
                    WclButton chkIsSupplemental = e.Item.FindControl("chkIsSupplemental") as WclButton;
                    //WclButton chkIgnore = e.Item.FindControl("chkIgnore") as WclButton;
                    //CurrentViewContext.ViewContract.ShowIgnoreResidentialHistory = chkIgnore.Checked;
                    CurrentViewContext.ViewContract.ShowIsSupplemental = chkIsSupplemental.Checked;
                    CurrentViewContext.ViewContract.ShowMaxOcuurence = chkMaxOccurences.Checked;
                    CurrentViewContext.ViewContract.ShowMinOcuurence = chkMinOccurences.Checked;
                    CurrentViewContext.ViewContract.ShowPackageCount = chkPackageCount.Checked;
                    //CurrentViewContext.ViewContract.ShowResidenceYears = chkYear.Checked;
                    CurrentViewContext.ViewContract.ShowSendDocument = chkSendDoc.Checked;
                    CurrentViewContext.ViewContract.ServiceTypeID = Convert.ToInt32((e.Item.FindControl("ddlServiceType") as WclComboBox).SelectedValue);
                    if (parentServiceId > 0)
                        CurrentViewContext.ViewContract.ParentServiceID = parentServiceId;
                    //CurrentViewContext.ViewContract.ServiceTypeID = (e.item.FindControl("") as WclTextBox)
                    Presenter.SaveMasterService();
                    if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                    {
                        e.Canceled = true;
                        //base.ShowErrorMessage(String.Format("{0} already exist", CurrentViewContext.ViewContract.ServiceName));
                        (e.Item.FindControl("lblErrorMessage") as Label).ShowMessage(String.Format("{0} already exist.", CurrentViewContext.ViewContract.ServiceName), MessageType.Error);
                    }
                    else
                    {
                        e.Canceled = false;
                        base.ShowSuccessMessage("Service added successfully.");
                    }

                }
                if (e.CommandName == "Update")
                {
                    CurrentViewContext.ViewContract.ServiceID = Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["BSE_ID"]);
                    CurrentViewContext.ViewContract.ServiceName = (e.Item.FindControl("txtServiceName") as WclTextBox).Text.Trim();

                    //UAT-1728:Create ability to add cofigurable text to the result report (and flagged only and service group reports) by service.
                    CurrentViewContext.ViewContract.ConfigurableServiceText = (e.Item.FindControl("txtConfServiceText") as WclTextBox).Text.Trim();

                    CurrentViewContext.ViewContract.ServiceDesc = (e.Item.FindControl("txtSvcDescription") as WclTextBox).Text.Trim();
                    CurrentViewContext.ViewContract.ServiceTypeID = Convert.ToInt32((e.Item.FindControl("ddlServiceType") as WclComboBox).SelectedValue);
                    WclButton chkPackageCount = e.Item.FindControl("chkPackageCount") as WclButton;
                    //WclButton chkYear = e.Item.FindControl("chkYear") as WclButton;
                    WclButton chkMinOccurences = e.Item.FindControl("chkMinOccurences") as WclButton;
                    WclButton chkMaxOccurences = e.Item.FindControl("chkMaxOccurences") as WclButton;
                    WclButton chkSendDoc = e.Item.FindControl("chkSendDoc") as WclButton;
                    WclButton chkIsSupplemental = e.Item.FindControl("chkIsSupplemental") as WclButton;
                    //WclButton chkIgnore = e.Item.FindControl("chkIgnore") as WclButton;
                    //CurrentViewContext.ViewContract.ShowIgnoreResidentialHistory = chkIgnore.Checked;
                    CurrentViewContext.ViewContract.ShowIsSupplemental = chkIsSupplemental.Checked;
                    CurrentViewContext.ViewContract.ShowMaxOcuurence = chkMaxOccurences.Checked;
                    CurrentViewContext.ViewContract.ShowMinOcuurence = chkMinOccurences.Checked;
                    CurrentViewContext.ViewContract.ShowPackageCount = chkPackageCount.Checked;
                    //CurrentViewContext.ViewContract.ShowResidenceYears = chkYear.Checked;
                    CurrentViewContext.ViewContract.ShowSendDocument = chkSendDoc.Checked;
                    Int32 parentServiceId = Convert.ToInt32((e.Item.FindControl("ddlDerivedFromServices") as WclComboBox).SelectedValue);
                    if (parentServiceId > 0)
                        CurrentViewContext.ViewContract.ParentServiceID = parentServiceId;
                    //CurrentViewContext.ViewContract.Active = Convert.ToBoolean((e.Item.FindControl("chkActive") as WclButton).Checked);
                    //CurrentViewContext.ViewContract.TenantID = TenantId;
                    Presenter.UpdateServiceGroup();
                    if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                    {
                        e.Canceled = true;
                        //base.ShowErrorMessage(String.Format("{0} already exist", CurrentViewContext.ViewContract.ServiceName));
                        (e.Item.FindControl("lblErrorMessage") as Label).ShowMessage(String.Format("{0} already exist.", CurrentViewContext.ViewContract.ServiceName), MessageType.Error);
                    }
                    else
                    {
                        e.Canceled = false;
                        base.ShowSuccessMessage("Service updated successfully.");
                    }
                }
                if (e.CommandName == "Delete")
                {

                    CurrentViewContext.ViewContract.ServiceID = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["BSE_ID"]);
                    if (Presenter.IsServiceMAppedToClient(Convert.ToInt32(CurrentViewContext.ViewContract.ServiceID)))
                    {
                        Presenter.DeletebackgroundService(Convert.ToInt32(CurrentViewContext.ViewContract.ServiceID));
                    }
                    if (!String.IsNullOrEmpty(CurrentViewContext.InfoMessage))
                    {
                        base.ShowErrorInfoMessage(CurrentViewContext.InfoMessage);
                    }
                    else if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                    {
                        base.ShowErrorInfoMessage(CurrentViewContext.ErrorMessage);
                    }
                    else
                    {

                        base.ShowSuccessMessage("Service deleted successfully.");
                    }
                }

                #region DetailScreenNavigation

                if (e.CommandName.Equals("MapAttributeGroup"))
                {

                    if (e.Item is GridDataItem)
                    {
                        GridDataItem dataItem = e.Item as GridDataItem;
                        CurrentViewContext.ViewContract.ServiceName = Convert.ToString(dataItem["BSE_Name"].Text);
                    }

                    //SetSessionValues();
                    Dictionary<String, String> queryString = new Dictionary<String, String>();
                    String ServiceID = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["BSE_ID"].ToString();
                    String ServiceName = CurrentViewContext.ViewContract.ServiceName.ToString();
                    queryString = new Dictionary<String, String>
                                                                 { 
                                                                    
                                                                    { "Child", ChildControls.ManageServiceAttributeGroup},
                                                                    { "ServiceID",Convert.ToString(ServiceID)},
                                                                 
                                                                   
                                                                    {"ServiceName", ServiceName}
                                                                 };
                    string url = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                    Response.Redirect(url, true);

                }
                #endregion
                //MapCustomForm
                #region DetailScreenNavigation

                if (e.CommandName.Equals("MapCustomForm"))
                {

                    if (e.Item is GridDataItem)
                    {
                        GridDataItem dataItem = e.Item as GridDataItem;
                        CurrentViewContext.ViewContract.ServiceName = Convert.ToString(dataItem["BSE_Name"].Text);
                    }

                    //SetSessionValues();
                    Dictionary<String, String> queryString = new Dictionary<String, String>();
                    String ServiceID = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["BSE_ID"].ToString();
                    String ServiceName = CurrentViewContext.ViewContract.ServiceName.ToString();
                    queryString = new Dictionary<String, String>
                                                                 { 
                                                                    
                                                                    { "Child", ChildControls.ManageCustomForm},
                                                                    { "ServiceID",Convert.ToString(ServiceID)},
                                                                    {"ServiceName", ServiceName}
                                                                 };
                    string url = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                    Response.Redirect(url, true);

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

        protected void grdManageService_ItemCreated(object sender, GridItemEventArgs e)
        {
            try
            {
                //insert operation
                if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
                {
                    GridEditFormItem editform = (e.Item as GridEditFormItem);
                    WclComboBox ddlServiceType = editform.FindControl("ddlServiceType") as WclComboBox;
                    WclComboBox ddlDerivedFromServices = editform.FindControl("ddlDerivedFromServices") as WclComboBox;
                    if (ddlServiceType.IsNotNull())
                    {
                        //Bind Service type dropdown
                        Presenter.GetBkgServiceTypes();

                        if (CurrentViewContext.BkgServiceTypeList.IsNotNull())
                        {
                            BindCombo(ddlServiceType, CurrentViewContext.BkgServiceTypeList);
                        }

                    }
                    if (ddlDerivedFromServices.IsNotNull())
                    {
                        //Bind Services dropdown
                        Presenter.GetDerivedFromServiceList();

                        if (CurrentViewContext.BkgDerivedFromServiceList.IsNotNull())
                        {
                            BindCombo(ddlDerivedFromServices, CurrentViewContext.BkgDerivedFromServiceList);
                        }

                    }
                    //update operation
                    if (!(e.Item.DataItem is GridInsertionObject))
                    {
                        BackgroundService bkgService = (BackgroundService)e.Item.DataItem;
                        if (bkgService.IsNotNull())
                        {
                            if (ddlServiceType.IsNotNull())
                            {
                                Presenter.GetBkgServiceTypes();
                                if (CurrentViewContext.BkgServiceTypeList.IsNotNull())
                                {
                                    BindCombo(ddlServiceType, CurrentViewContext.BkgServiceTypeList);
                                }
                                ddlServiceType.SelectedValue = Convert.ToString(bkgService.BSE_SvcTypeID);
                            }

                            if (ddlDerivedFromServices.IsNotNull())
                            {
                                Presenter.GetDerivedFromServiceList(bkgService.BSE_ID);
                                if (CurrentViewContext.BkgDerivedFromServiceList.IsNotNull())
                                {
                                    BindCombo(ddlDerivedFromServices, CurrentViewContext.BkgDerivedFromServiceList);
                                }
                                ddlDerivedFromServices.SelectedValue = Convert.ToString(bkgService.BSE_ParentServiceID);
                            }
                            ApplicableServiceSetting applicableServiceSetting = bkgService.ApplicableServiceSettings.FirstOrDefault();
                            if (!applicableServiceSetting.IsNullOrEmpty())
                            {
                                WclButton chkPackageCount = e.Item.FindControl("chkPackageCount") as WclButton;
                                if (chkPackageCount.IsNotNull())
                                {
                                    chkPackageCount.Checked = applicableServiceSetting.ASSE_ShowPackageCount.HasValue ? applicableServiceSetting.ASSE_ShowPackageCount.Value : false;
                                }
                                //WclButton chkYear = e.Item.FindControl("chkYear") as WclButton;
                                //if (chkYear.IsNotNull())
                               // {
                                 //   chkYear.Checked = applicableServiceSetting.ASSE_ShowResidenceYears.HasValue ? applicableServiceSetting.ASSE_ShowResidenceYears.Value : false;
                                //}
                                WclButton chkMinOccurences = e.Item.FindControl("chkMinOccurences") as WclButton;
                                if (chkMinOccurences.IsNotNull())
                                {
                                    chkMinOccurences.Checked = applicableServiceSetting.ASSE_ShowMinOcuurence.HasValue ? applicableServiceSetting.ASSE_ShowMinOcuurence.Value : false;
                                }
                                WclButton chkMaxOccurences = e.Item.FindControl("chkMaxOccurences") as WclButton;
                                if (chkMaxOccurences.IsNotNull())
                                {
                                    chkMaxOccurences.Checked = applicableServiceSetting.ASSE_ShowMaxOcuurence.HasValue ? applicableServiceSetting.ASSE_ShowMaxOcuurence.Value : false;
                                }
                                WclButton chkSendDoc = e.Item.FindControl("chkSendDoc") as WclButton;
                                if (chkSendDoc.IsNotNull())
                                {
                                    chkSendDoc.Checked = applicableServiceSetting.ASSE_ShowSendDocument.HasValue ? applicableServiceSetting.ASSE_ShowSendDocument.Value : false;
                                }
                                WclButton chkIsSupplemental = e.Item.FindControl("chkIsSupplemental") as WclButton;
                                if (chkIsSupplemental.IsNotNull())
                                {
                                    chkIsSupplemental.Checked = applicableServiceSetting.ASSE_ShowIsSupplemental.HasValue ? applicableServiceSetting.ASSE_ShowIsSupplemental.Value : false;
                                }
                                //WclButton chkIgnore = e.Item.FindControl("chkIgnore") as WclButton;
                                //if (chkIgnore.IsNotNull())
                                //{
                                //    chkIgnore.Checked = applicableServiceSetting.ASSE_ShowIgnoreResidentialHistory.HasValue ? applicableServiceSetting.ASSE_ShowIgnoreResidentialHistory.Value : false;
                                //}
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


        #endregion
        #endregion

        #region Methods
        #region Public Methods
        #endregion

        #region Private Methods
        //private void BindTenant()
        //{
        //    if (IsAdminLoggedIn == true)
        //    {
        //        dvServiceGroup.Visible = false;
        //        Presenter.GetTenants();
        //        ddlTenant.DataSource = ListTenants;
        //        ddlTenant.DataBind();
        //    }
        //    else
        //    {
        //        pnlTenant.Visible = false;
        //    }
        //}

        //private void SetDefaultSelectedTenantId()
        //{
        //    if (ddlTenant.SelectedValue.IsNullOrEmpty())
        //    {
        //        SelectedTenantId = TenantId;
        //        ddlTenant.SelectedValue = Convert.ToString(TenantId);
        //    }
        //}

        private void BindCombo(WclComboBox cmbBox, Object dataSource)
        {
            //if (dataSource==String.Empty)
            cmbBox.Items.Clear();
            cmbBox.DataSource = dataSource;
            cmbBox.DataBind();
            if (cmbBox.Items.FindItemByText(AppConsts.COMBOBOX_ITEM_SELECT).IsNull())
            {
                cmbBox.AddFirstEmptyItem();
            }
        }

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
                                if (x.FeatureAction.CustomActionId == "Add")
                                {
                                    grdManageService.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "Edit")
                                {
                                    grdManageService.MasterTableView.GetColumn("EditCommandColumn").Display = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "Delete")
                                {
                                    grdManageService.MasterTableView.GetColumn("DeleteColumn").Display = false;
                                }
                                break;
                            }
                    }

                }
                    );
            }
        }



        #endregion




        #endregion

        protected override void ApplyActionLevelPermission(List<Entity.ClientEntity.ClsFeatureAction> ctrlCollection, string screenName = "")
        {
            base.ApplyActionLevelPermission(ctrlCollection, screenName);
            ApplyPermisions();

        }


        public override List<Entity.ClientEntity.ClsFeatureAction> ActionCollection
        {
            get
            {
                List<Entity.ClientEntity.ClsFeatureAction> actionCollection = new List<Entity.ClientEntity.ClsFeatureAction>();
                Entity.ClientEntity.ClsFeatureAction objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                objClsFeatureAction.CustomActionId = "Add";
                objClsFeatureAction.CustomActionLabel = "Add New";
                objClsFeatureAction.ScreenName = "Manage Service";
                actionCollection.Add(objClsFeatureAction);
                objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                objClsFeatureAction.CustomActionId = "Edit";
                objClsFeatureAction.CustomActionLabel = "Edit";
                objClsFeatureAction.ScreenName = "Manage Service";
                actionCollection.Add(objClsFeatureAction);
                objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                objClsFeatureAction.CustomActionId = "Delete";
                objClsFeatureAction.CustomActionLabel = "Delete";
                objClsFeatureAction.ScreenName = "Manage Service";
                actionCollection.Add(objClsFeatureAction);
                return actionCollection;
            }
        }
        public override List<String> ChildScreenPathCollection
        {
            get
            {
                List<String> childScreenPathCollection = new List<String>();
                childScreenPathCollection.Add(@"~/BkgSetup/UserControl/ManageServiceAttributeGroup.ascx");
                childScreenPathCollection.Add(@"~/BkgSetup/UserControl/ManageBkgServiceCustomForm.ascx");
                return childScreenPathCollection;
            }
        }


    }
}