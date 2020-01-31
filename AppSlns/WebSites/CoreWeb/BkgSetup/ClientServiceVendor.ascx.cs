
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using INTSOF.Utils;
using Telerik.Web.UI;
using Entity;
using INTSOF.UI.Contract.BkgSetup;
using INTERSOFT.WEB.UI.WebControls;

namespace CoreWeb.BkgSetup.Views
{
    public partial class ClientServiceVendor : BaseUserControl, IClientServiceVendorView
    {

        #region Variable
        private ClientServiceVendorPresenter _presenter = new ClientServiceVendorPresenter();
        private String _viewType;
        private Int32 tenantId = 0;
        private Int32 _selectedTenantId = AppConsts.NONE;
        private Boolean? _isAdminLoggedIn = null;
        private Boolean _isupdate = false;
        private Boolean _a = false;
        #endregion

        #region Properties
        public ClientServiceVendorPresenter Presenter
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

        public IClientServiceVendorView CurrentViewContext
        {
            get
            {
                return this;
            }
        }
        public List<Tenant> ListTenants
        {
            set;
            get;
        }
        /// <summary>
        /// get tenant id
        /// </summary>
        public Int32 TenantId
        {
            get
            {
                if (tenantId == 0)
                {
                    //tenantId = Presenter.GetTenantId();
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    if (user != null)
                    {
                        tenantId = user.TenantId.HasValue ? user.TenantId.Value : 0;
                    }
                }
                return tenantId;

            }
            set { tenantId = value; }
        }
        public Int32 SelectedTenantId
        {
            get
            {
                if (_selectedTenantId == AppConsts.NONE)
                {
                    Int32.TryParse(ddlTenant.SelectedValue, out _selectedTenantId);

                    if (_selectedTenantId == AppConsts.NONE)
                        _selectedTenantId = TenantId;
                }
                return _selectedTenantId;
            }
            set
            {
                _selectedTenantId = value;
            }
        }

        /// <summary>
        /// get Current logged In Id
        /// </summary>
        public Int32 CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
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
        public Boolean IsAdminLoggedIn
        {
            get
            {
                if (_isAdminLoggedIn.IsNull())
                {
                    Presenter.IsAdminLoggedIn();
                }
                return _isAdminLoggedIn.HasValue ? _isAdminLoggedIn.Value : true;
            }
            set
            {
                _isAdminLoggedIn = value;
            }
        }

        public List<ClientServiceVendorContract> GetMappedServiceStateList
        {
            get;
            set;
        }
        public List<Entity.ClientEntity.BackgroundService> BackgroundServicesLst
        {
            get;
            set;
        }
        public Int32 SelectedServiceID
        {
            get;
            set;
        }
        public List<Entity.ExternalBkgSvc> ExtBkgServicesLst
        {
            get;
            set;
        }
        public Int32 SelectedExtbKGSvcid
        {
            get;
            set;
        }
        public Boolean IsAllState
        {
            get;
            set;
        }
        public List<Entity.State> ListStates
        { get; set; }
        /// <summary>
        /// Error Message property
        /// </summary>
        public String ErrorMessage { get; set; }
        /// <summary>
        /// Success Message property
        /// </summary>
        public String SuccessMessage { get; set; }
        /// <summary>
        /// Info Message property
        /// </summary>
        public String InfoMessage { get; set; }
        #endregion


        #region Events

        #region Page Events

        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                base.Title = "Client Service Vendor";
                base.SetPageTitle("Client Service Vendor");
                //fsucCmdBarButton.SubmitButton.CausesValidation = false;

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
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];

                if (!this.IsPostBack)
                {
                    BindTenant();
                    Presenter.OnViewInitialized();
                    //to call Permissions 
                    ApplyActionLevelPermission(ActionCollection, "Client Service Vendor");
                }
                SetDefaultSelectedTenantId();
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

        #region DropDown

        protected void ddlTenant_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                grdClientSvcVendor.CurrentPageIndex = 0;
                grdClientSvcVendor.MasterTableView.SortExpressions.Clear();
                grdClientSvcVendor.MasterTableView.FilterExpression = null;

                foreach (GridColumn column in grdClientSvcVendor.MasterTableView.OwnerGrid.Columns)
                {
                    column.CurrentFilterFunction = GridKnownFunction.NoFilter;
                    column.CurrentFilterValue = string.Empty;
                }
                SelectedTenantId = Convert.ToInt32(ddlTenant.SelectedValue);
                dvClientSvcVendor.Visible = true;
                grdClientSvcVendor.Rebind();
                if (ddlTenant.SelectedValue.IsNullOrEmpty() || ddlTenant.SelectedValue==AppConsts.ZERO)
                {
                    dvClientSvcVendor.Visible = false;
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

        protected void cmbBkgServices_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                WclComboBox cmbBkgService = sender as WclComboBox;
                CurrentViewContext.SelectedServiceID = Convert.ToInt32(cmbBkgService.SelectedValue);
                WclComboBox cmbExtBkgService = cmbBkgService.Parent.NamingContainer.FindControl("cmbExtBkgServices") as WclComboBox;
                
                if (cmbExtBkgService.IsNotNull())
                {
                    Presenter.GetExtBkgSvcCorrespondsToBkgSvc(CurrentViewContext.SelectedServiceID,_isupdate);
                    if (CurrentViewContext.ExtBkgServicesLst.IsNotNull())
                    {
                        BindCombo(cmbExtBkgService, CurrentViewContext.ExtBkgServicesLst);
                    }
                }

                //grdClientSvcVendor.Rebind();
                //WclGrid grdCSVendor =(WclGrid)((((cmbBkgService.Parent.Parent.BindingContainer).BindingContainer).BindingContainer).BindingContainer);
                //grdCSVendor.Rebind();
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
        protected void cmbExtBkgServices_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                WclComboBox cmbExtBkgServices = sender as WclComboBox;
                CurrentViewContext.SelectedExtbKGSvcid = !(cmbExtBkgServices.SelectedValue.IsNullOrEmpty()) ? Convert.ToInt32(cmbExtBkgServices.SelectedValue) : AppConsts.NONE;
                Label lblExtSvcCode = cmbExtBkgServices.Parent.NamingContainer.FindControl("lblExtSvcCode") as Label;
                if (CurrentViewContext.SelectedExtbKGSvcid == AppConsts.NONE)
                {
                    //cmbExtBkgServices.Items.Clear();
                    cmbExtBkgServices.Text = String.Empty;
                    lblExtSvcCode.Text = String.Empty;
                }
                else
                {
                    lblExtSvcCode.Text = Presenter.FetchExternalBkgServiceCodeByID();                    
                }
                WclComboBox cmbState = cmbExtBkgServices.Parent.NamingContainer.FindControl("cmbState") as WclComboBox;
                List<Int32> AlreadyStateMappedIds = new List<Int32>();
                AlreadyStateMappedIds = _presenter.GetMAppedStatesIdtoExtSvc(Convert.ToInt32(CurrentViewContext.SelectedExtbKGSvcid));
                if (cmbState.IsNotNull() && (AlreadyStateMappedIds.Count > AppConsts.NONE))
                {
                    Presenter.GetAllStates();

                    if (CurrentViewContext.ListStates.IsNotNull())
                    {
                        BindCombo(cmbState, CurrentViewContext.ListStates);
                    }
               //cmbState.Items.WhereSelect(condition => AlreadyStateMappedIds.Contains(Convert.ToInt32(condition.Value)))
               //               .ForEach(condition => condition.Checked = true);
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

        protected void grdClientSvcVendor_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetMappedBkgSvcExtSvcToState();
                if (CurrentViewContext.GetMappedServiceStateList.Count > 0)
                {
                    grdClientSvcVendor.DataSource = CurrentViewContext.GetMappedServiceStateList;
                }
                else
                {
                    grdClientSvcVendor.DataSource = String.Empty;
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
        protected void grdClientSvcVendor_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {

                if (e.Item is GridEditableItem && e.Item.IsInEditMode)
                {
                    // insert item
                    GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                    WclComboBox cmbBkgServices = (WclComboBox)gridEditableItem.FindControl("cmbBkgServices");
                    WclComboBox cmbExtBkgServices = (WclComboBox)gridEditableItem.FindControl("cmbExtBkgServices");
                    WclComboBox cmbState = (WclComboBox)gridEditableItem.FindControl("cmbState");
                    WclButton chkAllState = (WclButton)gridEditableItem.FindControl("chkAllState");
                    Label lblExtSvcCode = gridEditableItem.FindControl("lblExtSvcCode") as Label;
                
                    //   grdNodeDeadline.NeedDataSource -= grdNodeDeadline_NeedDataSource;
                    // cmbExtBkgServices.SelectedIndexChanged -= cmbExtBkgServices_SelectedIndexChanged;
                    if (cmbBkgServices.IsNotNull())
                    {
                        //Bind BkgService dropdown
                        Presenter.GetBkgService();
                        if (CurrentViewContext.BackgroundServicesLst.IsNotNull())
                        {
                            BindCombo(cmbBkgServices, CurrentViewContext.BackgroundServicesLst);
                        }
                    }
                    if (cmbState.IsNotNull())
                    {

                        Presenter.GetAllStates();
                        if (CurrentViewContext.ListStates.IsNotNull())
                        {
                            BindCombo(cmbState, CurrentViewContext.ListStates);
                        }
                    }
                    //CurrentViewContext.IsAllState = Convert.ToBoolean(chkAllState.Value);
                    if (!(e.Item is GridEditFormInsertItem || e.Item is GridDataInsertItem))
                    {
                        // edit item
                        ClientServiceVendorContract clientServiceVendorContract = (ClientServiceVendorContract)e.Item.DataItem;
                        if (clientServiceVendorContract.IsNotNull()) 
                        {
                            if (clientServiceVendorContract.State == "ALL States") 
                            {
                                CurrentViewContext.IsAllState = true;
                            }
                        }
                        chkAllState.Checked = CurrentViewContext.IsAllState;
                        var BkgServiceId = Convert.ToInt32(gridEditableItem.GetDataKeyValue("BkgServiceID"));
                        var ExtServiceId = Convert.ToInt32(gridEditableItem.GetDataKeyValue("ExtServiceID"));
                        _isupdate = true;
                        Presenter.GetBkgService();
                        BindCombo(cmbBkgServices, CurrentViewContext.BackgroundServicesLst);
                        if (BkgServiceId.IsNotNull() && BkgServiceId > 0)
                        {
                            CurrentViewContext.SelectedServiceID = Convert.ToInt32(BkgServiceId);
                            cmbBkgServices.SelectedValue = BkgServiceId.ToString();
                            cmbBkgServices.Enabled = false;
                            if (cmbExtBkgServices.IsNotNull())
                            {
                               // cmbExtBkgServices.AutoPostBack = true;
                                Presenter.GetExtBkgSvcCorrespondsToBkgSvc(BkgServiceId, _isupdate);
                                if (CurrentViewContext.ExtBkgServicesLst.IsNotNull())
                                {
                                    BindCombo(cmbExtBkgServices, CurrentViewContext.ExtBkgServicesLst);
                                }
                                CurrentViewContext.SelectedExtbKGSvcid = Convert.ToInt32(ExtServiceId);
                                lblExtSvcCode.Text = Presenter.FetchExternalBkgServiceCodeByID();
                                cmbExtBkgServices.SelectedValue = ExtServiceId.ToString();
                                cmbExtBkgServices.Enabled = false;
                                List<Int32> AlreadyStateMappedIds = new List<Int32>();
                                AlreadyStateMappedIds = _presenter.GetMAppedStatesIdtoExtSvc(Convert.ToInt32(CurrentViewContext.SelectedExtbKGSvcid));
                                if (cmbState.IsNotNull() && (AlreadyStateMappedIds.Count > AppConsts.NONE))
                                {
                                    Presenter.GetAllStates();

                                    if (CurrentViewContext.ListStates.IsNotNull())
                                    {
                                        BindCombo(cmbState, CurrentViewContext.ListStates);
                                    }
                                    cmbState.Items.WhereSelect(condition => AlreadyStateMappedIds.Contains(Convert.ToInt32(condition.Value)))
                              .ForEach(condition => condition.Checked = true);
                                }
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
        protected void grdClientSvcVendor_InsertCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                CurrentViewContext.SelectedServiceID = Convert.ToInt32((e.Item.FindControl("cmbBkgServices") as WclComboBox).SelectedValue);
                CurrentViewContext.SelectedExtbKGSvcid = Convert.ToInt32((e.Item.FindControl("cmbExtBkgServices") as WclComboBox).SelectedValue);
                WclComboBox cmbState = (e.Item.FindControl("cmbState") as WclComboBox);
                List<Int32> selectedNewMappedStateIds = new List<Int32>();
                CurrentViewContext.IsAllState = (e.Item.FindControl("chkAllState") as WclButton).Checked;
                if (CurrentViewContext.IsAllState == false && cmbState.CheckedItems.Count == AppConsts.NONE)
                {
                    CurrentViewContext.IsAllState = true;
                    //CurrentViewContext.ErrorMessage = "Please select atleast any state or all state.";
                    //if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                    //{
                    //    (e.Item.FindControl("lblErrorMessage") as Label).ShowMessage(CurrentViewContext.ErrorMessage, MessageType.Error);
                    //    e.Canceled = true;
                    //    return;
                    //}

                }
                for (Int32 i = 0; i < cmbState.CheckedItems.Count; i++)
                {
                    if (cmbState.CheckedItems[i].Checked)
                    {
                        selectedNewMappedStateIds.Add(Convert.ToInt32(cmbState.CheckedItems[i].Value));
                    }
                }
                if (CurrentViewContext.IsAllState) 
                {
                    selectedNewMappedStateIds.Clear() ;
                    selectedNewMappedStateIds.Add(Convert.ToInt32(AppConsts.ZERO));
                }
                _presenter.SaveClientServiceNewMapping(selectedNewMappedStateIds, CurrentViewContext.SelectedServiceID, CurrentViewContext.SelectedExtbKGSvcid);
                if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                {
                    e.Canceled = true;
                    //base.ShowErrorMessage(String.Format("{0} already exist", CurrentViewContext.ViewContract.ServiceName));
                    (e.Item.FindControl("lblErrorMessage") as Label).ShowMessage(String.Format("{0}.", CurrentViewContext.ErrorMessage), MessageType.Error);
                }
                else
                {
                    e.Canceled = false;
                    base.ShowSuccessMessage("States mappped with service successfully.");
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
        protected void grdClientSvcVendor_ItemCreated(object sender, GridItemEventArgs e)
        {
            try
            {
                //if (e.Item is GridEditableItem && e.Item.IsInEditMode)
                //{
                //    if (e.Item is GridEditFormInsertItem || e.Item is GridDataInsertItem)
                //    {// insert item
                //        GridEditFormItem editform = (GridEditFormItem)e.Item;
                //        WclComboBox cmbBkgServices = (WclComboBox)editform.FindControl("cmbBkgServices");
                //        WclComboBox cmbExtBkgServices = (WclComboBox)editform.FindControl("cmbExtBkgServices");
                //        WclComboBox cmbState = (WclComboBox)editform.FindControl("cmbState");
                //        if (cmbBkgServices.IsNotNull())
                //        {
                //            //Bind BkgService dropdown
                //            Presenter.GetBkgService();
                //            if (CurrentViewContext.BackgroundServicesLst.IsNotNull())
                //            {
                //                BindCombo(cmbBkgServices, CurrentViewContext.BackgroundServicesLst);
                //            }
                //        }
                //    }
                //    else
                //    {
                //        // edit item
                //    }
                //}
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

        protected void grdClientSvcVendor_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                CurrentViewContext.SelectedServiceID = Convert.ToInt32((e.Item.FindControl("cmbBkgServices") as WclComboBox).SelectedValue);
                CurrentViewContext.SelectedExtbKGSvcid = Convert.ToInt32((e.Item.FindControl("cmbExtBkgServices") as WclComboBox).SelectedValue);
                CurrentViewContext.IsAllState = (e.Item.FindControl("chkAllState") as WclButton).Checked;
                WclComboBox cmbState = (e.Item.FindControl("cmbState") as WclComboBox);
                List<Int32> selectedNewMappedStateIds = new List<Int32>();
                if (CurrentViewContext.IsAllState == false && cmbState.CheckedItems.Count == AppConsts.NONE)
                {
                    CurrentViewContext.IsAllState = true;
                    //CurrentViewContext.ErrorMessage = "Please select atleast any state or all state.";
                    //if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                    //{
                    //    (e.Item.FindControl("lblErrorMessage") as Label).ShowMessage(CurrentViewContext.ErrorMessage, MessageType.Error);
                    //    e.Canceled = true;
                    //    return;
                    //}

                }
                for (Int32 i = 0; i < cmbState.CheckedItems.Count; i++)
                {
                    if (cmbState.CheckedItems[i].Checked)
                    {
                        selectedNewMappedStateIds.Add(Convert.ToInt32(cmbState.CheckedItems[i].Value));
                    }
                }
                if (CurrentViewContext.IsAllState)
                {
                    selectedNewMappedStateIds.Clear();
                    selectedNewMappedStateIds.Add(Convert.ToInt32(AppConsts.ZERO));
                }
                _presenter.UpdateClientSvcVendorMapping(selectedNewMappedStateIds, CurrentViewContext.SelectedServiceID, CurrentViewContext.SelectedExtbKGSvcid);
                if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                {
                    e.Canceled = true;
                    //base.ShowErrorMessage(String.Format("{0} already exist", CurrentViewContext.ViewContract.ServiceName));
                    (e.Item.FindControl("lblErrorMessage") as Label).ShowMessage(String.Format("{0}", CurrentViewContext.ErrorMessage), MessageType.Error);
                }
                else
                {
                    e.Canceled = false;
                    base.ShowSuccessMessage("State mappped with service Updated successfully.");
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
        protected void grdClientSvcVendor_PreRender(object sender, EventArgs e)
        {
            try
            {
                //Merging the rows of grid if the value of the following columns Are same:-BkgServiceName,ExtServiceName.
                //foreach (GridDataItem dataItem in grdClientSvcVendor.Items)
                //{
                    //GridDataItem dataItem = grdClientSvcVendor.;
                    Int16 count = 0;
                    Int16 First = 0;
                    //GridTableView grdTableView = (GridTableView)dataItem.OwnerTableView;
                   // int a = grdTableView.Items.Count - 2;
                    for (int rowIndex = grdClientSvcVendor.Items.Count - 2; rowIndex >= 0; rowIndex--)
                    {

                        GridDataItem row = grdClientSvcVendor.Items[rowIndex];
                        GridDataItem previousRow = grdClientSvcVendor.Items[rowIndex + 1];
                        if (row["BkgServiceName"].Text == previousRow["BkgServiceName"].Text && row["ExtServiceName"].Text == previousRow["ExtServiceName"].Text)
                        {
                            row["BkgServiceName"].RowSpan = previousRow["BkgServiceName"].RowSpan < 2 ? 2 : previousRow["BkgServiceName"].RowSpan + 1;
                            previousRow["BkgServiceName"].Visible = false;
                            //previousRow["BkgServiceName"].Text = "&nbsp;";
                            //if (row["ExtServiceName"].Text == previousRow["ExtServiceName"].Text)
                            //{
                                row["ExtServiceName"].RowSpan = previousRow["ExtServiceName"].RowSpan < 2 ? 2 : previousRow["ExtServiceName"].RowSpan + 1;
                                previousRow["ExtServiceName"].Visible = false;

                                row["ExtServiceCode"].RowSpan = previousRow["ExtServiceCode"].RowSpan < 2 ? 2 : previousRow["ExtServiceCode"].RowSpan + 1;
                                previousRow["ExtServiceCode"].Visible = false;
                                //previousRow["ExtServiceName"].Text = "&nbsp;";
                            //}
                            if (count != 0)
                            {
                                previousRow["EditCommandColumn"].Text = "&nbsp;";
                                previousRow["DeleteColumn"].Text = "&nbsp;";
                            }
                            count += 1;

                            if (rowIndex == 0)
                            {
                                row["EditCommandColumn"].Text = "&nbsp;";
                                row["DeleteColumn"].Text = "&nbsp;";
                            }
                        }

                        else
                        {
                            if ((row["BkgServiceName"].Text != "&nbsp;" && previousRow["BkgServiceName"].Text != "&nbsp;"))
                            {
                               
                                if (count > 0)
                                {
                                    previousRow["EditCommandColumn"].Text = "&nbsp;";
                                    previousRow["DeleteColumn"].Text = "&nbsp;";
                                }
                               
                                row["DeleteColumn"].BorderColor = System.Drawing.Color.Black;
                                row["DeleteColumn"].BorderStyle = BorderStyle.Solid;
                                row["EditCommandColumn"].BorderColor = System.Drawing.Color.Black;
                                row["EditCommandColumn"].BorderStyle = BorderStyle.Solid;
                            }

                           
                            count = 0;

                        }
                        row["BkgServiceName"].BorderColor = System.Drawing.Color.Black;
                        row["BkgServiceName"].BorderStyle = BorderStyle.Solid;
                        row["ExtServiceName"].BorderColor = System.Drawing.Color.Black;
                        row["ExtServiceName"].BorderStyle = BorderStyle.Solid;
                        row["ExtServiceName"].BackColor = System.Drawing.Color.FromArgb(194, 195, 190);
                        previousRow["ExtServiceName"].BackColor = System.Drawing.Color.FromArgb(194, 195, 190);
                        row["ExtServiceCode"].BorderColor = System.Drawing.Color.Black;
                        row["ExtServiceCode"].BorderStyle = BorderStyle.Solid;
                        row["ExtServiceCode"].BackColor = System.Drawing.Color.FromArgb(194, 195, 190);
                        previousRow["ExtServiceCode"].BackColor = System.Drawing.Color.FromArgb(194, 195, 190);
                        row["State"].BorderColor = System.Drawing.Color.Black;
                        row["State"].BorderStyle = BorderStyle.Solid;
                        row["State"].BackColor = System.Drawing.Color.FromArgb(228, 219, 210);
                        previousRow["State"].BackColor = System.Drawing.Color.FromArgb(228, 219, 210);

                    }

                //}
                grdClientSvcVendor.ClientSettings.EnableRowHoverStyle = false;
                grdClientSvcVendor.ClientSettings.Selecting.AllowRowSelect = false;
                grdClientSvcVendor.ClientSettings.EnableAlternatingItems = false;
                grdClientSvcVendor.GridLines = GridLines.None;
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

        protected void grdClientSvcVendor_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                String BkgServiceName = String.Empty;
                String BkgsvcID = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["BkgServiceID"].ToString();
                String ExtServiceID = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ExtServiceID"].ToString();
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    BkgServiceName = Convert.ToString(dataItem["BkgServiceName"].Text);
                }
                _presenter.DeleteClientVendorExtSvcMapping(Convert.ToInt32(BkgsvcID), Convert.ToInt32(ExtServiceID), BkgServiceName);
                if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                {

                    base.ShowErrorInfoMessage(CurrentViewContext.ErrorMessage);
                }
                else
                {

                    base.ShowSuccessMessage("Mappping deleted successfully.");
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
        private void BindTenant()
        {
            if (IsAdminLoggedIn == true)
            {
                dvClientSvcVendor.Visible = false;
                Presenter.GetTenants();
                ddlTenant.DataSource = ListTenants;
                ddlTenant.DataBind();
            }
            else
            {
                pnlTenant.Visible = false;
            }
        }

        private void SetDefaultSelectedTenantId()
        {
            if (ddlTenant.SelectedValue.IsNullOrEmpty())
            {
                SelectedTenantId = TenantId;
                ddlTenant.SelectedValue = Convert.ToString(TenantId);
            }
        }

        private void BindCombo(WclComboBox cmbBox, Object dataSource)
        {
            //if (dataSource==String.Empty)
            cmbBox.Items.Clear();
            cmbBox.DataSource = dataSource;
            cmbBox.DataBind();
            //if (cmbBox.Items.FindItemByText(AppConsts.COMBOBOX_ITEM_SELECT).IsNull())
            //{
            //    cmbBox.AddFirstEmptyItem();
            //}
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
                                    grdClientSvcVendor.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "Edit")
                                {
                                    grdClientSvcVendor.MasterTableView.GetColumn("EditCommandColumn").Display = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "Delete")
                                {
                                    grdClientSvcVendor.MasterTableView.GetColumn("DeleteColumn").Display = false;
                                }
                                break;
                            }
                    }

                }
                    );
            }
        }
        #endregion

        #region Action Permission

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
                objClsFeatureAction.CustomActionLabel = "Add New Mapping";
                objClsFeatureAction.ScreenName = "Client Service Vendor";
                actionCollection.Add(objClsFeatureAction);
                objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                objClsFeatureAction.CustomActionId = "Edit";
                objClsFeatureAction.CustomActionLabel = "Edit";
                objClsFeatureAction.ScreenName = "Client Service Vendor";
                actionCollection.Add(objClsFeatureAction);
                objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                objClsFeatureAction.CustomActionId = "Delete";
                objClsFeatureAction.CustomActionLabel = "Delete";
                objClsFeatureAction.ScreenName = "Client Service Vendor";
                actionCollection.Add(objClsFeatureAction);
                return actionCollection;
            }
        }
        #endregion

    }
}